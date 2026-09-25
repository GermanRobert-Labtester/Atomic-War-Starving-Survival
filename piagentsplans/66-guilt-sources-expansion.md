# Plan 66 — Guilt Sources: Forty-Row Moral Consequence Vocabulary and Phase-0 Guilt/Sleep Ownership

> **Rebuild status:** TERMINAL 40-ROW CONTENT + LIVE CONSUMER AUDIT
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

The historical baseline was 4,966 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the forty authored guilt-source rows as a consequence vocabulary while preserving `GuiltInsomniaSystem` as the owner of guilt records, insomnia severity, sleep quality, therapy relief, expiry, and phase-0 persistence. The historical 20→40 expansion is complete; the important residual is a current catalog-to-phase-0 consumer binding audit, because the current host supplies severity at command time and does not visibly load the catalog.

**Bounded outcome:** Audit `guilt_sources.json`, `GuiltSourceCatalog`, `GuiltInsomniaSystem`, `Phase0HostSession`, `Main.Phase0`/medical/UI consumers, and focused tests. Determine whether choice patterns map to current commands, how descriptions are formatted, and where a future typed binding should live without creating a second guilt ledger.

**Non-goals:** no second guilt/insomnia store, no direct psychological punishment from JSON, no arbitrary row growth, no new save section, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `guilt_sources.json` contains 40 rows.
- VERIFIED CURRENT: `GuiltSourceCatalog` parses choice_pattern, severity, title, and description.
- VERIFIED CURRENT: `GuiltInsomniaSystem` owns mutable records, insomnia, sleep quality, relief, and capture/restore.
- The current production binding between GuiltSourceCatalog and Phase0HostSession is an explicit reachability question; the host’s public RecordGuilt method currently accepts source id and severity.
- HISTORICAL RECORD: Plan 66/Wave 38 records the 20→40 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/guilt_sources.json` exists at 10,440 bytes; SHA-256 `88f7a18ce18f408d3924b9b3ec4be831fd2f92b7ea61b72fd09ed1bdf28e198e`.
- `Assets/StreamingAssets/Data/mental_arcs.json` exists at 1,904 bytes; SHA-256 `bde0a4215bc4f8602dff19d5d735c9d1413baa1e518ac8ff8cf542b07858784e`.
- `Assets/StreamingAssets/Data/confession_secrets.json` exists at 95,078 bytes; SHA-256 `3fff37f541bfff91b6265fc2596e5ec7d6fcf664f18fecdc39b883f57d9f7349`.

# 3. Required Delta

Replace the old pure-data brief with a current 40-row vocabulary/consumer audit. Preserve GuiltInsomniaSystem and phase-0 persistence, and explicitly investigate the catalog-to-command seam instead of claiming it exists.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Does current production load GuiltSourceCatalog, or do all callers pass source id/severity directly?
Which current choice/incident/final-wish commands can emit a typed guilt intent?
How are duplicate deliveries, expiry, relief, and phase-0 restore pinned?
Which UI surfaces expose current guilt/insomnia without overclaiming?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| guilt row definitions and pattern lookup | `GuiltSourceCatalog` | `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` | Static choice-pattern/severity/title/description vocabulary. |
| guilt records, insomnia, sleep, relief, expiry, and capture/restore | `GuiltInsomniaSystem` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | Sole mutable guilt/insomnia owner. |
| phase-0 lifecycle and persistence façade | `Phase0HostSession / Phase0SaveStore` | `src/Host/Phase0HostSession.cs; src/Host/Phase0SaveStore.cs` | Composes guilt state inside phase-0 owner. |
| medical/psychology consumers | `Medical/Psychology owners` | `src/Main.Medical.cs; Assets/Ashfall.Core/Medical/` | Consume current guilt/sleep facts; do not own the ledger. |
| player presentation | `Phase0Panel / AfflictionsPanel` | `src/UI/Phase0Panel.cs; src/UI/AfflictionsPanel.cs` | Show current guilt/insomnia state and available relief. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for guilt-source catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Guilt Sources: Forty-Row Moral Consequence Vocabulary and Phase-0 Guilt/Sleep Ownership                                               │
│ Integration route: DATA-ONLY + current phase-0 command/save audit                             │
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

1. **Catalog defines guilt vocabulary.**
2. **GuiltInsomniaSystem owns guilt/sleep.**
3. **Phase0 owns lifecycle/persistence.**
4. **Canonical choices emit intent.**
5. **UI projects current state.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| guilt row definitions and pattern lookup | `GuiltSourceCatalog` | `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` | Static choice-pattern/severity/title/description vocabulary. |
| guilt records, insomnia, sleep, relief, expiry, and capture/restore | `GuiltInsomniaSystem` | `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | Sole mutable guilt/insomnia owner. |
| phase-0 lifecycle and persistence façade | `Phase0HostSession / Phase0SaveStore` | `src/Host/Phase0HostSession.cs; src/Host/Phase0SaveStore.cs` | Composes guilt state inside phase-0 owner. |
| medical/psychology consumers | `Medical/Psychology owners` | `src/Main.Medical.cs; Assets/Ashfall.Core/Medical/` | Consume current guilt/sleep facts; do not own the ledger. |
| player presentation | `Phase0Panel / AfflictionsPanel` | `src/UI/Phase0Panel.cs; src/UI/AfflictionsPanel.cs` | Show current guilt/insomnia state and available relief. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load or resolve a guilt definition through GuiltSourceCatalog
2. a canonical choice/incident owner emits a typed guilt intent
3. Phase0HostSession maps the current choice pattern to severity/description
4. GuiltInsomniaSystem records one bounded guilt fact and updates insomnia
5. Needs/medical/therapy consumers read sleep/condition projections
6. capture/restore phase-0 guilt state and present truthful status

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- choice patterns are unique and descriptions are restrained/diegetic
- severity is finite and bounded
- a source id cannot create duplicate guilt on repeated delivery
- guilt expiry and therapy relief are owner-controlled
- sleep quality remains a projection, not a second needs ledger
- phase-0 restore preserves records and timers
- unresolved catalog rows do not fabricate a guilt consequence

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

Contract rules for guilt-source catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`guilt_sources.json` remains the forty-row definition authority. Current rows have choice_pattern, severity, title, and description with `{name}` formatting. Audit whether the host currently loads this catalog or receives source id/severity directly. A future binding must use a typed choice-pattern vocabulary and current owner command; do not infer that a row has a live incident/final-wish consumer.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Guilt state is carried by the existing phase-0 save state and `GuiltInsomniaSystem.CaptureState`/`RestoreState`; no `guilt_sources` save section is justified. A future catalog binding must preserve existing records and legacy absent-catalog defaults.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

RecordGuilt, expiry, sleep projection, and relief are deterministic for the same state/day. Descriptions are static formatting; no RNG or wall-clock. Paired replay compares source ids, severity, timers, sleep multipliers, and restore.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

A canonical choice/incident emits a typed intent; Phase0HostSession records it once; GuiltInsomniaSystem emits current guilt/insomnia facts. A JSON row does not itself fire an incident, final wish, confession, or psychological crisis.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/Phase0HostSession.cs` — composes GuiltInsomniaSystem, commands, tick, and phase-0 persistence
- `src/Main.Phase0.cs` — current choice/phase-0 host binding
- `src/Main.Medical.cs` — medical/psychology consumer and relief seam
- `src/UI/Phase0Panel.cs` — phase-0 state presentation
- `src/UI/AfflictionsPanel.cs` — current affliction/mental-state projection

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Guilt descriptions should be quiet, embodied, and specific to a survivor’s memory. They may name a choice pattern, but they cannot diagnose, punish, or create a new faction/war consequence outside the current moral/incident owners.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`

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
| Phase 0 — row/owner census | read 40 rows, catalog, system, phase-0 host, and tests | all fields and current consumers are explicit | no undocumented scope or shortcut |
| Phase 1 — trigger-binding trace | map choice patterns to current commands/incidents | live/unresolved rows are classified | no undocumented scope or shortcut |
| Phase 2 — sleep/save/determinism audit | check timers, relief, duplicates, and phase-0 restore | no shadow guilt state | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven catalog-to-command gap is promoted | one owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/guilt_sources.json` | READ ONLY; MODIFY only for a proven trigger/consumer defect | retain as guilt vocabulary |
| `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` | READ ONLY | row parser/lookup |
| `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | READ ONLY | mutable owner |
| `src/Host/Phase0HostSession.cs` | READ ONLY | host command/persistence |
| `src/UI/AfflictionsPanel.cs` | READ ONLY | truthful presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel guilt/psychology state | use GuiltInsomniaSystem and phase-0 owner |
| catalog-to-command drift | trace current choice producers |
| duplicate consequence | use one idempotent record edge |
| save regression | phase-0 round-trip tests |

# 22. Explicit Non-Goals

- no second guilt/insomnia store, no direct psychological punishment from JSON, no arbitrary row growth, no new save section, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for guilt-source catalog are named from current evidence.
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

- A current 40-row pattern/consumer/command matrix.
- An explicit finding on catalog loading versus direct severity injection.
- A bounded typed binding plan only if a real choice/incident gap is proven.

## MUST NOT DO

- create a second guilt ledger
- make every row auto-trigger
- apply psychological effects from description text
- add a static-catalog save section

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read guilt_sources.json, GuiltSourceCatalog, GuiltInsomniaSystem, Phase0HostSession, Main.Phase0, and focused tests; compare catalog patterns with current RecordGuilt callers.

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

### Authority lines 116–121
00116: ### Cluster definitions
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
00121:

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

### Authority lines 674–679
00674: **A-04 · C2 · Dose-treatment narrative pairing.** Subject: therapy-note and casebook twins for each row of `MEDICAL_DOSE_TREATMENT_MATRIX.md` (live, DR-03), so every mechanical treatment has a clinical-document voice. Evidence: matrix document verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00675:
00676: **A-05 · C2 · Therapist session notes batch 4.** Subject: a fourth batch keyed to guilt sources and insomnia states added since batch 3. Evidence: therapist batches 1–3 exist in the corpus; guilt sources and guilt insomnia are canon systems. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00677:
00678: **A-06 · C3 · Preservation and processing assay twins.** Subject: assay/log corpus entries for every `food_preservation.json` and `grain_processing.json` process lacking a narrative twin — the Part 16.4 pattern applied to the food domain. Evidence: both catalogs verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00679:

### Authority lines 706–715
00706: **A-20 · C8 · Radio program rundown expansion.** Subject: rundown batches for stations with thin programming against `radio_programs.json` and `radio_stations.json`. Evidence: both catalogs verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE. Note: distress-signal content is SEALED (`CF-P1-DISTRESS-CONTENT-SEAL`); rundowns must not add signal scenarios.
00707:
00708: **A-21 · C9 · Phantom-memory trigger expansion keyed to surviving cohorts.** Subject: heirloom-trigger entries conditioned on cohort survival state, deepening the generational line. Evidence: `phantom_heirlooms.json`, `phantom_triggers.json` verified live; cohort/lineage systems are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00709:
00710: **A-22 · C9 · Final-wishes document corpus.** Subject: unsent-letter and testament prose for `final_wishes.json` entries lacking document twins. Evidence: catalog verified live; `unsent_letters_batch_2` demonstrates the genre. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00711:
00712: **A-23 · C9 · Intake interview continuation.** Subject: new-arrival intake interviews conditioned on the arrival channels that exist (rescue, crossing, holdfast). Evidence: `new_arrival_intake_interviews` exists; arrival channels are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00713:
00714: **A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00715:

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

### Authority lines 4167–4172
04167: 1. CI gates: `bash scripts/ci/verify-fast.sh` (fast tier, 53 gates at manifest 1.1.0) and `python3 scripts/ci/run-gates.py` (the manifest-driven runner). Full tier per manifest classification (4 non-fast gates at 1.1.0, including `save_support_window` and the performance gate per the v1.0 inventory).
04168: 2. xUnit: `Ashfall.Core.Tests/` (root), built per the manifest's `build_core_tests` gate ("Build Ashfall.Core.Tests (net9.0)" — verified gate name).
04169: 3. Headless selftests: the `*HeadlessDemo.cs` classes in `Assets/Ashfall.Core/` — verified live: `BrineWaterHeadlessDemo`, `CensusHeadlessDemo`, `Cluster12CHeadlessDemo`, `CrossingArbitrationHeadlessDemo`, `CrossingHeadlessDemo`, `DeepCoastHeadlessDemo`, `EndingsHeadlessDemo`, `GeothermalAquiferHeadlessDemo`, `HoldfastHeadlessDemo`, `IceRoadHeadlessDemo`, `InfrastructureHeadlessDemo`, `LedgerDebtHeadlessDemo`, `NarrativeHeadlessDemo`, `PersonalQuestHeadlessDemo`, `SurvivorsHeadlessDemo`, `TravelEncounterHeadlessDemo`, `TravelingCaravanHeadlessDemo`. Plus the Godot-side surface: `src/CSharpVerificationTest.cs`, `src/Main.UiTests.*.cs` (20 verified suites: CompositionRoot, Dose, DutyRoster, Economy, Expeditions, Holdfast, Inventory, Journal, Muster, Phase0, Plans198_201, PlayerPanels, RealCampaignJourney, SilentFoundry, StartingCohortLifecycle, Survivors, UtilityAi, Verdict, Wave6, WorkshopRelic), `src/Main.WorldPlaytest.cs`, `src/HostCliRegistry.cs`/`HostTestSummary.cs` (the selftest manifest surface — `generate-selftest-manifest.py` is its verified generator).
04170:
04171: ## 28.2 Per-cluster mapping (C1–C17)
04172:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/guilt_sources.json`
- Bytes: 10,440; SHA-256: `88f7a18ce18f408d3924b9b3ec4be831fd2f92b7ea61b72fd09ed1bdf28e198e`
- Root keys: `items, schema_version`
- `items`: list[40]; union fields: `choice_pattern, description, severity, title`
  - row 1: `{"choice_pattern":"cut_ration","description":"Cutting rations for those who could not contribute. The hollow eyes of the hungry follow {name} through every corridor.","severity":0.8,"title":"The Ration Cut"}`
  - row 2: `{"choice_pattern":"reduce_food","description":"Reducing food distribution below subsistence. {name} remembers every bowl they made smaller.","severity":0.75,"title":"The Empty Bowls"}`
  - row 3: `{"choice_pattern":"starve","description":"Deliberately withholding food. The sound of hunger — a quiet, persistent thing — haunts {name}'s sleep.","severity":0.9,"title":"The Starvation Order"}`
  - row 4: `{"choice_pattern":"leave_behind","description":"Leaving a companion behind during an expedition. {name} still checks over their shoulder, expecting to see the face they abandoned.","severity":0.7,"title":"The Abandoned"}`
  - row 5: `{"choice_pattern":"abandon","description":"Abandoning someone who trusted {name}. Trust, once broken, leaves a scar on both sides.","severity":0.75,"title":"The Betrayal of Trust"}`
  - row 6: `{"choice_pattern":"refuse_help","description":"Turning away a stranger in need. {name} wonders if that person survived the night.","severity":0.5,"title":"The Turned Back"}`
  - row 7: `{"choice_pattern":"turn_away","description":"Closing the hatch on desperate faces. {name} can still hear the knocking.","severity":0.55,"title":"The Closed Hatch"}`
  - row 8: `{"choice_pattern":"execute","description":"Taking a life in cold judgment. {name} sees that face every time they close their eyes.","severity":0.9,"title":"The Execution"}`
  - row 9: `{"choice_pattern":"kill","description":"Ending a life, even in necessity. {name} counts the names in the dark hours before dawn.","severity":0.85,"title":"The Taking of Life"}`
  - row 10: `{"choice_pattern":"shoot","description":"Pulling the trigger on another human being. The recoil leaves a bruise that never heals.","severity":0.8,"title":"The Shot"}`
  - row 11: `{"choice_pattern":"steal","description":"Taking what wasn't offered. {name} hides the stolen goods and the shame in equal measure.","severity":0.45,"title":"The Theft"}`
  - row 12: `{"choice_pattern":"hoard","description":"Keeping more than a fair share while others go without. The excess weighs heavier than hunger.","severity":0.4,"title":"The Hoard"}`
  - row 13: `{"choice_pattern":"take_all","description":"Stripping a cache bare, leaving nothing for those who might follow. {name} knows they condemned someone.","severity":0.6,"title":"The Empty Cache"}`
  - row 14: `{"choice_pattern":"lie","description":"A lie told to protect oneself at another's expense. The truth surfaces in {name}'s dreams.","severity":0.35,"title":"The Lie"}`
  - row 15: `{"choice_pattern":"deceive","description":"A deception that shifted burden onto the innocent. {name} avoids the eyes of those they deceived.","severity":0.4,"title":"The Deception"}`
  - row 16: `{"choice_pattern":"betray","description":"A betrayal of someone who called {name} friend. The word 'friend' has tasted like ash ever since.","severity":0.75,"title":"The Betrayal"}`
  - row 17: `{"choice_pattern":"harsh","description":"Harsh words spoken in a moment of weakness. {name} wishes they could take them back.","severity":0.2,"title":"The Harsh Word"}`
  - row 18: `{"choice_pattern":"refuse","description":"A refusal that cost someone dearly. {name} replays the conversation, looking for a different answer.","severity":0.25,"title":"The Refusal"}`
  - row 19: `{"choice_pattern":"deny","description":"Denying someone what they needed. The denial was logical at the time — now it just feels cruel.","severity":0.25,"title":"The Denial"}`
  - row 20: `{"choice_pattern":"sacrifice_other","description":"Sacrificing another to save the many. The math is sound; the heart rejects it completely.","severity":0.85,"title":"The Sacrifice of Another"}`
  - row 21: `{"choice_pattern":"hoard_medicine_while_needed","description":"The locked cabinet still holds the vial that was deemed too valuable to dispense. {name} knows someone in the infirmary died counting the hours until the next dose.","severity":0.8,"title":"The Last Dose"}`
  - row 22: `{"choice_pattern":"barter_away_needed_food","description":"The trade crates were hauled away for ammunition and scrap. {name} stands in front of the ration pantry, staring at the dust rings where the flour bags sat.","severity":0.55,"title":"Weight of the Crates"}`
  - row 23: `{"choice_pattern":"issue_known_contaminated_supplies","description":"The radiation warning tape was peeled away before the tins were brought to the table. {name} watches the children eat, saying nothing about the grease pencil mark beneath the label.","severity":0.7,"title":"Marked Unsafe"}`
  - row 24: `{"choice_pattern":"burn_critical_fuel_for_comfort","description":"For one evening the stove burned hot enough to sleep without coats. {name} wakes to find black frost choking the hydroponics lines three rooms down.","severity":0.3,"title":"One Warm Room"}`
  - row 25: `{"choice_pattern":"refuse_refugee_entry","description":"The surveillance monitor kept flickering after {name} cut the exterior intercom. In the morning, the snow outside the heavy outer door is disturbed and empty.","severity":0.65,"title":"Closed Door"}`
  - row 26: `{"choice_pattern":"expel_survivor_for_efficiency","description":"Their bunk was reassigned before the sheets went cold. {name} avoids looking at the tally marks carved into the timber post beside the mattress.","severity":0.7,"title":"The Empty Bunk"}`
  - row 27: `{"choice_pattern":"hide_cache_from_allies","description":"The false floorboard sits flush with the subfloor. {name} looks away as an ally divides the remaining dried lentils into three equal, insufficient portions.","severity":0.45,"title":"Behind the Panel"}`
  - row 28: `{"choice_pattern":"abandon_committed_rescue","description":"The grease pencil marker stays pinned to the sector map where the expedition turned around. {name} cannot bring themselves to wipe the glass clean.","severity":0.6,"title":"Turned Back"}`
  - row 29: `{"choice_pattern":"leave_wounded_behind","description":"The head count on the way back had one fewer name. Through the long corridor walk home, {name} kept listening for footsteps that never caught up.","severity":0.8,"title":"One Less Footstep"}`
  - row 30: `{"choice_pattern":"retreat_from_rescue","description":"The distress signal was still cycling when {name} switched off the receiver to conserve battery. The speaker clicks softly in the dark before going dead.","severity":0.65,"title":"Radio Still Calling"}`
  - row 31: `{"choice_pattern":"execute_surrendered_enemy","description":"The rifle was already dropped in the dirt when {name} pulled the trigger. What stays in memory is how slowly the empty hands drifted downward.","severity":0.85,"title":"Hands Visible"}`
  - row 32: `{"choice_pattern":"use_civilians_as_bait","description":"The diversion drew the patrol away from the supply cache exactly as calculated. {name} got the team out alive, and that is the part that makes sleep impossible.","severity":0.9,"title":"The Safer Route"}`
  - row 33: `{"choice_pattern":"kill_former_ally","description":"The insignia on the jacket was new, but the voice across the barricade was not. {name} cleaned their weapon afterward without looking at the brass casing on the floor.","severity":0.8,"title":"Known Face"}`
  - row 34: `{"choice_pattern":"betray_faction_trust","description":"The signed pact is still filed in the dispatch locker with {name}'s name on the seal. The people who honored it are no longer answering on the wire.","severity":0.6,"title":"Terms Broken"}`
  - row 35: `{"choice_pattern":"inform_on_survivor","description":"The patrol only asked for a name once before handing over the supply voucher. {name} holds the canned meat in their palms, unable to open it.","severity":0.7,"title":"Name Given"}`
  - row 36: `{"choice_pattern":"break_final_wish_promise","description":"There is no one left alive to ask whether the last promise was kept. {name} carries the unfulfilled words like lead in their chest.","severity":0.85,"title":"The Promise"}`
  - row 37: `{"choice_pattern":"withhold_pain_relief","description":"The ampoule remains unbroken in the medical kit for a future emergency. {name} remembers the sound of breathing in the dark ward after the lantern went out.","severity":0.7,"title":"Saved for Later"}`
  - row 38: `{"choice_pattern":"triage_by_utility","description":"The triage tag marked priority by work output rather than blood loss. {name} wrote the numbers down with steady hands that now shake when holding a pen.","severity":0.75,"title":"Useful Enough"}`
  - row 39: `{"choice_pattern":"take_family_last_supplies","description":"The pantry shelves were scraped bare down to the wood shavings. {name} noticed the pencil height marks on the doorframe only after the rucksack was already zipped.","severity":0.7,"title":"Nothing Left"}`
  - row 40: `{"choice_pattern":"order_survivor_to_death","description":"They asked only once if there was another way before stepping into the irradiated conduit. {name} gave the order, and the shelter went quiet.","severity":0.9,"title":"The Order Given"}`
- Bytes: 10,440; SHA-256: `88f7a18ce18f408d3924b9b3ec4be831fd2f92b7ea61b72fd09ed1bdf28e198e`
- Root keys: `items, schema_version`
- `items`: list[40]; union fields: `choice_pattern, description, severity, title`
  - row 1: `{"choice_pattern":"cut_ration","description":"Cutting rations for those who could not contribute. The hollow eyes of the hungry follow {name} through every corridor.","severity":0.8,"title":"The Ration Cut"}`
  - row 2: `{"choice_pattern":"reduce_food","description":"Reducing food distribution below subsistence. {name} remembers every bowl they made smaller.","severity":0.75,"title":"The Empty Bowls"}`
  - row 3: `{"choice_pattern":"starve","description":"Deliberately withholding food. The sound of hunger — a quiet, persistent thing — haunts {name}'s sleep.","severity":0.9,"title":"The Starvation Order"}`
  - row 4: `{"choice_pattern":"leave_behind","description":"Leaving a companion behind during an expedition. {name} still checks over their shoulder, expecting to see the face they abandoned.","severity":0.7,"title":"The Abandoned"}`
  - row 5: `{"choice_pattern":"abandon","description":"Abandoning someone who trusted {name}. Trust, once broken, leaves a scar on both sides.","severity":0.75,"title":"The Betrayal of Trust"}`
  - row 6: `{"choice_pattern":"refuse_help","description":"Turning away a stranger in need. {name} wonders if that person survived the night.","severity":0.5,"title":"The Turned Back"}`
  - row 7: `{"choice_pattern":"turn_away","description":"Closing the hatch on desperate faces. {name} can still hear the knocking.","severity":0.55,"title":"The Closed Hatch"}`
  - row 8: `{"choice_pattern":"execute","description":"Taking a life in cold judgment. {name} sees that face every time they close their eyes.","severity":0.9,"title":"The Execution"}`
  - row 9: `{"choice_pattern":"kill","description":"Ending a life, even in necessity. {name} counts the names in the dark hours before dawn.","severity":0.85,"title":"The Taking of Life"}`
  - row 10: `{"choice_pattern":"shoot","description":"Pulling the trigger on another human being. The recoil leaves a bruise that never heals.","severity":0.8,"title":"The Shot"}`
  - row 11: `{"choice_pattern":"steal","description":"Taking what wasn't offered. {name} hides the stolen goods and the shame in equal measure.","severity":0.45,"title":"The Theft"}`
  - row 12: `{"choice_pattern":"hoard","description":"Keeping more than a fair share while others go without. The excess weighs heavier than hunger.","severity":0.4,"title":"The Hoard"}`
  - row 13: `{"choice_pattern":"take_all","description":"Stripping a cache bare, leaving nothing for those who might follow. {name} knows they condemned someone.","severity":0.6,"title":"The Empty Cache"}`
  - row 14: `{"choice_pattern":"lie","description":"A lie told to protect oneself at another's expense. The truth surfaces in {name}'s dreams.","severity":0.35,"title":"The Lie"}`
  - row 15: `{"choice_pattern":"deceive","description":"A deception that shifted burden onto the innocent. {name} avoids the eyes of those they deceived.","severity":0.4,"title":"The Deception"}`
  - row 16: `{"choice_pattern":"betray","description":"A betrayal of someone who called {name} friend. The word 'friend' has tasted like ash ever since.","severity":0.75,"title":"The Betrayal"}`
  - row 17: `{"choice_pattern":"harsh","description":"Harsh words spoken in a moment of weakness. {name} wishes they could take them back.","severity":0.2,"title":"The Harsh Word"}`
  - row 18: `{"choice_pattern":"refuse","description":"A refusal that cost someone dearly. {name} replays the conversation, looking for a different answer.","severity":0.25,"title":"The Refusal"}`
  - row 19: `{"choice_pattern":"deny","description":"Denying someone what they needed. The denial was logical at the time — now it just feels cruel.","severity":0.25,"title":"The Denial"}`
  - row 20: `{"choice_pattern":"sacrifice_other","description":"Sacrificing another to save the many. The math is sound; the heart rejects it completely.","severity":0.85,"title":"The Sacrifice of Another"}`
  - row 21: `{"choice_pattern":"hoard_medicine_while_needed","description":"The locked cabinet still holds the vial that was deemed too valuable to dispense. {name} knows someone in the infirmary died counting the hours until the next dose.","severity":0.8,"title":"The Last Dose"}`
  - row 22: `{"choice_pattern":"barter_away_needed_food","description":"The trade crates were hauled away for ammunition and scrap. {name} stands in front of the ration pantry, staring at the dust rings where the flour bags sat.","severity":0.55,"title":"Weight of the Crates"}`
  - row 23: `{"choice_pattern":"issue_known_contaminated_supplies","description":"The radiation warning tape was peeled away before the tins were brought to the table. {name} watches the children eat, saying nothing about the grease pencil mark beneath the label.","severity":0.7,"title":"Marked Unsafe"}`
  - row 24: `{"choice_pattern":"burn_critical_fuel_for_comfort","description":"For one evening the stove burned hot enough to sleep without coats. {name} wakes to find black frost choking the hydroponics lines three rooms down.","severity":0.3,"title":"One Warm Room"}`
  - row 25: `{"choice_pattern":"refuse_refugee_entry","description":"The surveillance monitor kept flickering after {name} cut the exterior intercom. In the morning, the snow outside the heavy outer door is disturbed and empty.","severity":0.65,"title":"Closed Door"}`
  - row 26: `{"choice_pattern":"expel_survivor_for_efficiency","description":"Their bunk was reassigned before the sheets went cold. {name} avoids looking at the tally marks carved into the timber post beside the mattress.","severity":0.7,"title":"The Empty Bunk"}`
  - row 27: `{"choice_pattern":"hide_cache_from_allies","description":"The false floorboard sits flush with the subfloor. {name} looks away as an ally divides the remaining dried lentils into three equal, insufficient portions.","severity":0.45,"title":"Behind the Panel"}`
  - row 28: `{"choice_pattern":"abandon_committed_rescue","description":"The grease pencil marker stays pinned to the sector map where the expedition turned around. {name} cannot bring themselves to wipe the glass clean.","severity":0.6,"title":"Turned Back"}`
  - row 29: `{"choice_pattern":"leave_wounded_behind","description":"The head count on the way back had one fewer name. Through the long corridor walk home, {name} kept listening for footsteps that never caught up.","severity":0.8,"title":"One Less Footstep"}`
  - row 30: `{"choice_pattern":"retreat_from_rescue","description":"The distress signal was still cycling when {name} switched off the receiver to conserve battery. The speaker clicks softly in the dark before going dead.","severity":0.65,"title":"Radio Still Calling"}`
  - row 31: `{"choice_pattern":"execute_surrendered_enemy","description":"The rifle was already dropped in the dirt when {name} pulled the trigger. What stays in memory is how slowly the empty hands drifted downward.","severity":0.85,"title":"Hands Visible"}`
  - row 32: `{"choice_pattern":"use_civilians_as_bait","description":"The diversion drew the patrol away from the supply cache exactly as calculated. {name} got the team out alive, and that is the part that makes sleep impossible.","severity":0.9,"title":"The Safer Route"}`
  - row 33: `{"choice_pattern":"kill_former_ally","description":"The insignia on the jacket was new, but the voice across the barricade was not. {name} cleaned their weapon afterward without looking at the brass casing on the floor.","severity":0.8,"title":"Known Face"}`
  - row 34: `{"choice_pattern":"betray_faction_trust","description":"The signed pact is still filed in the dispatch locker with {name}'s name on the seal. The people who honored it are no longer answering on the wire.","severity":0.6,"title":"Terms Broken"}`
  - row 35: `{"choice_pattern":"inform_on_survivor","description":"The patrol only asked for a name once before handing over the supply voucher. {name} holds the canned meat in their palms, unable to open it.","severity":0.7,"title":"Name Given"}`
  - row 36: `{"choice_pattern":"break_final_wish_promise","description":"There is no one left alive to ask whether the last promise was kept. {name} carries the unfulfilled words like lead in their chest.","severity":0.85,"title":"The Promise"}`
  - row 37: `{"choice_pattern":"withhold_pain_relief","description":"The ampoule remains unbroken in the medical kit for a future emergency. {name} remembers the sound of breathing in the dark ward after the lantern went out.","severity":0.7,"title":"Saved for Later"}`
  - row 38: `{"choice_pattern":"triage_by_utility","description":"The triage tag marked priority by work output rather than blood loss. {name} wrote the numbers down with steady hands that now shake when holding a pen.","severity":0.75,"title":"Useful Enough"}`
  - row 39: `{"choice_pattern":"take_family_last_supplies","description":"The pantry shelves were scraped bare down to the wood shavings. {name} noticed the pencil height marks on the doorframe only after the rucksack was already zipped.","severity":0.7,"title":"Nothing Left"}`
  - row 40: `{"choice_pattern":"order_survivor_to_death","description":"They asked only once if there was another way before stepping into the irradiated conduit. {name} gave the order, and the shelter went quiet.","severity":0.9,"title":"The Order Given"}`

## `Assets/StreamingAssets/Data/mental_arcs.json`
- Bytes: 1,904; SHA-256: `bde0a4215bc4f8602dff19d5d735c9d1413baa1e518ac8ff8cf542b07858784e`
- Root keys: `arcs, schema_version`
- `arcs`: list[4]; union fields: `behavior, behavior_chance, behavior_cooldown_days, crisis_behavior_min_stage, display_name, id, minimum_stress_days, relapse_cooldown_days, stress_threshold, tags, treatment_tags`
  - row 1: `{"behavior":"stash_transfer","behavior_chance":0.25,"behavior_cooldown_days":3,"crisis_behavior_min_stage":1,"display_name":"Compulsive Stashing","id":"arc_compulsive_hoarding","minimum_stress_days":6,"relapse_cooldown_days":30,"stress_threshold":90,"tags":["hoarding","anxiety"],"treatment_tags":["therapy_work_rhythm_restoration","therapy_watch_rotation_counseling"]}`
  - row 2: `{"behavior":"unsafe_fire_request","behavior_chance":0.06,"behavior_cooldown_days":10,"crisis_behavior_min_stage":3,"display_name":"Fire Fixation","id":"arc_fire_fixation","minimum_stress_days":8,"relapse_cooldown_days":60,"stress_threshold":92,"tags":["fixation","rare"],"treatment_tags":["therapy_cognitive_catharsis","therapy_watch_rotation_counseling"]}`
  - row 3: `{"behavior":"refuse_assignment","behavior_chance":0.3,"behavior_cooldown_days":2,"crisis_behavior_min_stage":2,"display_name":"Persecutory Crisis","id":"arc_persecutory_crisis","minimum_stress_days":7,"relapse_cooldown_days":30,"stress_threshold":90,"tags":["persecutory","conflict"],"treatment_tags":["therapy_cognitive_catharsis","therapy_work_rhythm_restoration"]}`
  - row 4: `{"behavior":"withdraw_self_care","behavior_chance":0.35,"behavior_cooldown_days":2,"crisis_behavior_min_stage":2,"display_name":"Shutdown Withdrawal","id":"arc_shutdown_withdrawal","minimum_stress_days":9,"relapse_cooldown_days":45,"stress_threshold":95,"tags":["withdrawal","care"],"treatment_tags":["therapy_work_rhythm_restoration","therapy_cognitive_catharsis"]}`
- Bytes: 1,904; SHA-256: `bde0a4215bc4f8602dff19d5d735c9d1413baa1e518ac8ff8cf542b07858784e`
- Root keys: `arcs, schema_version`
- `arcs`: list[4]; union fields: `behavior, behavior_chance, behavior_cooldown_days, crisis_behavior_min_stage, display_name, id, minimum_stress_days, relapse_cooldown_days, stress_threshold, tags, treatment_tags`
  - row 1: `{"behavior":"stash_transfer","behavior_chance":0.25,"behavior_cooldown_days":3,"crisis_behavior_min_stage":1,"display_name":"Compulsive Stashing","id":"arc_compulsive_hoarding","minimum_stress_days":6,"relapse_cooldown_days":30,"stress_threshold":90,"tags":["hoarding","anxiety"],"treatment_tags":["therapy_work_rhythm_restoration","therapy_watch_rotation_counseling"]}`
  - row 2: `{"behavior":"unsafe_fire_request","behavior_chance":0.06,"behavior_cooldown_days":10,"crisis_behavior_min_stage":3,"display_name":"Fire Fixation","id":"arc_fire_fixation","minimum_stress_days":8,"relapse_cooldown_days":60,"stress_threshold":92,"tags":["fixation","rare"],"treatment_tags":["therapy_cognitive_catharsis","therapy_watch_rotation_counseling"]}`
  - row 3: `{"behavior":"refuse_assignment","behavior_chance":0.3,"behavior_cooldown_days":2,"crisis_behavior_min_stage":2,"display_name":"Persecutory Crisis","id":"arc_persecutory_crisis","minimum_stress_days":7,"relapse_cooldown_days":30,"stress_threshold":90,"tags":["persecutory","conflict"],"treatment_tags":["therapy_cognitive_catharsis","therapy_work_rhythm_restoration"]}`
  - row 4: `{"behavior":"withdraw_self_care","behavior_chance":0.35,"behavior_cooldown_days":2,"crisis_behavior_min_stage":2,"display_name":"Shutdown Withdrawal","id":"arc_shutdown_withdrawal","minimum_stress_days":9,"relapse_cooldown_days":45,"stress_threshold":95,"tags":["withdrawal","care"],"treatment_tags":["therapy_work_rhythm_restoration","therapy_cognitive_catharsis"]}`

## `Assets/StreamingAssets/Data/confession_secrets.json`
- Bytes: 95,078; SHA-256: `3fff37f541bfff91b6265fc2596e5ec7d6fcf664f18fecdc39b883f57d9f7349`
- Root keys: `items, schema_version`
- `items`: list[38]; union fields: `archetype_id, blackmail_hardening_delta, blackmail_outcome, blackmail_resource_gain, category, discovery_path, discovery_source_id, expose_guilt_delta, expose_outcome, expose_standing_delta, expose_standing_faction, forgiveness_affinity, forgiveness_morale, forgiveness_outcome, gating_flag, grudge_affinity, grudge_morale, grudge_outcome, keep_outcome, keep_trust_delta, secret_id, secret_text, secret_title, subject_id`
  - row 1: `{"archetype_id":"the_surgeon","blackmail_hardening_delta":0.15,"blackmail_outcome":"You wait until the clinic is empty and tell {name} that your name would like to be first on the list, next time there is a list. You do not say the word triage. You do not have to. From then on your doses are checked twice, your wounds are closed rather than taped, and the antibiotics with the dates still legible are the ones that find their way to your bunk. {name} does all of it competently and without expression, and does not look at you while doing it.","blackmail_resource_gain":"medical_supplies","category":"npc_personal","discovery_path":"direct_confess…`
  - row 2: `{"archetype_id":"the_soldier","blackmail_hardening_delta":0.2,"blackmail_outcome":"You tell {name} you want the tactical weapons stored under their bunk — the ones that weren't declared in the intake inventory. You don't say the words. You look at the board, then at {name}, and {name} understands. The weapons appear in your stores two days later, cleaned and oiled.","blackmail_resource_gain":"ammunition","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"dog_tags","expose_guilt_delta":15,"expose_outcome":"You write it on the watch-board in plain language: eighteen civilians, one family shelter, command orde…`
  - row 3: `{"archetype_id":"the_pharmacist","blackmail_hardening_delta":0.15,"blackmail_outcome":"You lean close enough to smell the sweat on their collar and whisper: one ampoule a week, uncut, delivered to the second ventilation grate. {name}'s hands tremble as they nod. From now on, they dispense your private supply with the same careful precision they once reserved for the dying. The irony is not lost on either of you.","blackmail_resource_gain":"medicine","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"morphine","expose_guilt_delta":8,"expose_outcome":"You lay the empty vials on the mess table during evening c…`
  - row 4: `{"archetype_id":"the_mother","blackmail_hardening_delta":0.25,"blackmail_outcome":"You remind {name} that the child they saved still needs a roof and clean water, and that those things aren't free. 'I need two extra shifts on the water pump,' you say. {name} looks at their living child, swallows hard, and nods. They work until their hands bleed, driven by a ghost they can't bury.","blackmail_resource_gain":"labor","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"childs_mitten","expose_guilt_delta":20,"expose_outcome":"The story works its way through the bunks like a slow infection. Mothers look at {name} …`
  - row 5: `{"archetype_id":"the_mechanic","blackmail_hardening_delta":0.15,"blackmail_outcome":"You corner {name} by the fuel tanks. 'My bunk needs a reinforced lock and a personal heater,' you say. 'You know how to route the power.' The mechanic clenches their jaw, trapped by their own lie. For the rest of the winter, your quarters are the warmest in the shelter, bought with extorted labor.","blackmail_resource_gain":"crafting_parts","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"mechanic_gloves","expose_guilt_delta":12,"expose_outcome":"You write the truth on the maintenance board for everyone to see. The next t…`
  - row 6: `{"archetype_id":"the_teacher","blackmail_hardening_delta":0.1,"blackmail_outcome":"You don't ask for much. Just private tutoring sessions, after hours, focusing on pre-war history. 'You owe the future something, after what you burned,' you tell {name}. The teacher spends their evenings exhausted, reciting memorized texts to you until their voice gives out, paying off a debt measured in pages.","blackmail_resource_gain":"research","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"pocket_notebook","expose_guilt_delta":5,"expose_outcome":"The ashes of the lost library are cataloged in the shelter's archive. T…`
  - row 7: `{"archetype_id":"the_refugee","blackmail_hardening_delta":0.15,"blackmail_outcome":"You tell {name} what you know. You say you will not act on it — provided their ration allocation is quietly supplemented from the undeclared stores they mentioned last month. {name}'s face does not change much. They were probably expecting this. They nod.","blackmail_resource_gain":"rations","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"undelivered_mail","expose_guilt_delta":10,"expose_outcome":"You submit the identity discrepancy to the registry ledger with a note: documents belonged to a deceased individual, current h…`
  - row 8: `{"archetype_id":"the_electrician","blackmail_hardening_delta":0.1,"blackmail_outcome":"You point out the unauthorized tap line. 'Route it to my sector,' you say. 'Nobody else has to know.' {name} spends two days crawling through the ductwork, rewiring the grid to siphon power to your quarters. You sleep in warmth while the rest of the shelter shivers, and {name} carries the guilt of both.","blackmail_resource_gain":"power","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"engineers_slide_rule","expose_guilt_delta":8,"expose_outcome":"You post the cause of the blackout on the community board. The survivor w…`
  - row 9: `{"archetype_id":"the_cook","blackmail_hardening_delta":0.15,"blackmail_outcome":"You visit the kitchen after lights out. 'I like my soup thick,' you whisper, tapping the hidden tin. {name} closes their eyes and nods. From then on, your bowl always has a piece of dried fruit or a scrap of real meat at the bottom, stolen from the mouths of {name}'s family to buy your silence.","blackmail_resource_gain":"rations","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"recipe_tin","expose_guilt_delta":12,"expose_outcome":"You carry the tins into the mess hall and stack them on the center table during the mid-day mea…`
  - row 10: `{"archetype_id":"the_engineer","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand full structural fortification of your private quarters.","blackmail_resource_gain":"building_materials","category":"npc_personal","discovery_path":"document","discovery_source_id":"engineers_slide_rule","expose_guilt_delta":25,"expose_outcome":"The engineering certificate is pinned to the town square bulletin.","expose_standing_delta":20,"expose_standing_faction":"faction_rebel","forgiveness_affinity":12,"forgiveness_morale":8,"forgiveness_outcome":"'The bureaucrats threatened to shoot you if you didn't sign,' the listener says. 'The blame belongs …`
  - row 11: `{"archetype_id":"the_farmer","blackmail_hardening_delta":0.15,"blackmail_outcome":"You demand the remaining secret heirloom seed packet.","blackmail_resource_gain":"seed_packet","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"family_heirloom_seeds","expose_guilt_delta":15,"expose_outcome":"The agricultural loss is entered into the provincial ledger.","expose_standing_delta":-10,"expose_standing_faction":"faction_independent","forgiveness_affinity":14,"forgiveness_morale":10,"forgiveness_outcome":"'You were resisting military looting,' the listener says. 'Nobody knew the world was ending tomorrow.'","gati…`
  - row 12: `{"archetype_id":"the_priest","blackmail_hardening_delta":0.2,"blackmail_outcome":"You sit in the confessional. 'You still have their ears,' you say. 'When the council votes on rationing tomorrow, tell them God demands sacrifice from the lower levels.' The priest bows their head, broken. The sermon the next morning is a masterpiece of manipulation, and you get exactly what you wanted.","blackmail_resource_gain":"influence","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"pocket_notebook","expose_guilt_delta":10,"expose_outcome":"The truth of the priest's empty faith spreads like a rot through the shelter. …`
  - row 13: `{"archetype_id":"the_journalist","blackmail_hardening_delta":0.15,"blackmail_outcome":"You demand the journalist's pre-war dossier on local faction leaders.","blackmail_resource_gain":"intelligence","category":"npc_personal","discovery_path":"document","discovery_source_id":"undelivered_mail","expose_guilt_delta":20,"expose_outcome":"The pre-war press suppression article is published on the shelter wall.","expose_standing_delta":15,"expose_standing_faction":"faction_rebel","forgiveness_affinity":12,"forgiveness_morale":8,"forgiveness_outcome":"'They would have disappeared you into a military prison if you ran it,' the listener says.","gating…`
  - row 14: `{"archetype_id":"the_pilot","blackmail_hardening_delta":0.15,"blackmail_outcome":"You extract navigation charts and flight instruments.","blackmail_resource_gain":"aviation_parts","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"tarnished_medal","expose_guilt_delta":18,"expose_outcome":"The court-martial charge sheet is made known to veteran groups.","expose_standing_delta":-25,"expose_standing_faction":"faction_military","forgiveness_affinity":18,"forgiveness_morale":12,"forgiveness_outcome":"'You refused a suicide run for corrupt politicians,' the listener says. 'That wasn't cowardice, it was sanity.'",…`
  - row 15: `{"archetype_id":"the_scientist","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand private filtration cartridges for personal use.","blackmail_resource_gain":"water_filters","category":"npc_personal","discovery_path":"document","discovery_source_id":"dosimeter","expose_guilt_delta":22,"expose_outcome":"The original water contamination telemetry is exposed.","expose_standing_delta":-15,"expose_standing_faction":"faction_independent","forgiveness_affinity":10,"forgiveness_morale":8,"forgiveness_outcome":"'Panic in a sealed shelter kills faster than cesium,' the listener says quietly.","gating_flag":"flag_secret_scientist_confessed…`
  - row 16: `{"archetype_id":"the_hunter","blackmail_hardening_delta":0.25,"blackmail_outcome":"You demand prime game cuts and hunting munitions.","blackmail_resource_gain":"food_and_ammo","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"engraved_lighter","expose_guilt_delta":30,"expose_outcome":"The wilderness homicide is reported to the border settlement.","expose_standing_delta":-20,"expose_standing_faction":"faction_independent","forgiveness_affinity":8,"forgiveness_morale":5,"forgiveness_outcome":"'Starvation turns human beings into wolves,' the listener whispers in horror. 'God forgive us all for what we became …`
  - row 17: `{"archetype_id":"the_nurse","blackmail_hardening_delta":0.15,"blackmail_outcome":"You keep a copy of the altered chart. From that day on, emergency medical packs and clean analgesics are diverted to your personal footlocker whenever you ask.","blackmail_resource_gain":"medical_supplies","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"nurse_fob_watch","expose_guilt_delta":18,"expose_outcome":"You bring the falsified triage log to the clinic council. Trust in the medical ward fractures, and {name} is barred from administering schedule-one narcotics.","expose_standing_delta":-10,"expose_standing_faction":"f…`
  - row 18: `{"archetype_id":"the_carpenter","blackmail_hardening_delta":0.2,"blackmail_outcome":"You threaten to call a public inspection of the workshop. In return, {name} reinforces your private quarters with double-thickness oak planks and reinforced iron braces.","blackmail_resource_gain":"building_materials","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"box_of_nails_10","expose_guilt_delta":15,"expose_outcome":"You show the cracked green pine joist to the construction crew. {name} is stripped of structural lead and demoted to sawing firewood under constant supervision.","expose_standing_delta":-12,"expose_sta…`
  - row 19: `{"archetype_id":"the_child","blackmail_hardening_delta":0.25,"blackmail_outcome":"You remind {name} that bad deeds follow people into adulthood unless someone protects them. The child timidly delivers their scavenged surface trinkets and shiny scrap to your bunk each week.","blackmail_resource_gain":"scavenged_trinkets","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"childs_drawing","expose_guilt_delta":25,"expose_outcome":"The story slips out among the younger survivors in the sleeping bays. The other children stop inviting {name} to their games, whispering that they will leave you behind if the sirens …`
  - row 20: `{"archetype_id":"the_old_man","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand the remaining municipal ration vouchers and civil defense stamps {name} kept in his lockbox. The old clerk complies without a word, his authority broken forever.","blackmail_resource_gain":"ration_stamps","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"tarnished_pocket_watch","expose_guilt_delta":16,"expose_outcome":"The old registry stamps and clerk rosters are revealed to the assembly. Several families whose relatives vanished during the purges confront the old man, demanding his immediate expulsion from the c…`
  - row 21: `{"archetype_id":"faction_independent","blackmail_hardening_delta":0.15,"blackmail_outcome":"You agree that the ledger was lost in a fire. The syndicate agrees that your bunker's supply requisition will always be the first one loaded onto the outbound wagons.","blackmail_resource_gain":"trade_goods","category":"faction_institutional","discovery_path":"document","discovery_source_id":"farm_ledger","expose_guilt_delta":0,"expose_outcome":"You nail the ledger pages to the public board at the water exchange. The syndicate's credit rating collapses before noon, and their caravans are turned away at three borders.","expose_standing_delta":-30,"expo…`
  - row 22: `{"archetype_id":"faction_military","blackmail_hardening_delta":0.2,"blackmail_outcome":"You hand over the audit. In exchange, two crates of military-issue cartridges and a box of ceramic armor plates fall off a transport truck near your hatch.","blackmail_resource_gain":"military_weapons","category":"faction_institutional","discovery_path":"document","discovery_source_id":"train_ticket_book","expose_guilt_delta":0,"expose_outcome":"You staple copies of the audit to the checkpoint barricades. By evening, three supply convoys have been blocked by civilians who know exactly how many calories were stolen.","expose_standing_delta":25,"expose_stan…`
  - row 23: `{"archetype_id":"faction_rebel","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand the codes for the safe-house network and a crate of smuggled rifles. They pay it, but the courier who delivers it does not look you in the eye.","blackmail_resource_gain":"smuggled_gear","category":"faction_institutional","discovery_path":"document","discovery_source_id":"undelivered_mail","expose_guilt_delta":0,"expose_outcome":"You read the dispatch aloud over the neutral frequency. The farmers burn the rebel banners they had hung in their barns.","expose_standing_delta":25,"expose_standing_faction":"faction_military","forgiveness_affinity":5,"f…`
  - row 24: `{"archetype_id":"faction_iron_clique","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hand over the tape drive. Two days later, a heavy cargo truck drops four barrels of diesel and a set of carbide machine tools at your perimeter.","blackmail_resource_gain":"fuel_and_tools","category":"faction_institutional","discovery_path":"radio","discovery_source_id":"civil_defense_radio","expose_guilt_delta":0,"expose_outcome":"You play the blackbox audio over the emergency band. The sound of fists on steel echoes through every radio in the valley, and the Clique's recruitment dries up overnight.","expose_standing_delta":-40,"expose_standing_f…`
  - row 25: `{"archetype_id":"faction_meridian","blackmail_hardening_delta":0.2,"blackmail_outcome":"You surrender the original manifests. At the start of every month, a courier delivers a sealed box of iodine, clean bandages, and sterile sutures to your gate.","blackmail_resource_gain":"antibiotics","category":"faction_institutional","discovery_path":"document","discovery_source_id":"train_ticket_book","expose_guilt_delta":0,"expose_outcome":"You pin the shipping bills to the door of the largest Free Clinic. The next day, a Meridian envoy's vehicle is turned over and set on fire in the street.","expose_standing_delta":-35,"expose_standing_faction":"fact…`
  - row 26: `{"archetype_id":"faction_order","blackmail_hardening_delta":0.2,"blackmail_outcome":"You trade the encrypted drives for two crates of heavy mortar shells and a rebuilt diesel generator. The Order considers it a cheap price for their reputation.","blackmail_resource_gain":"generator_parts","category":"faction_institutional","discovery_path":"radio","discovery_source_id":"civil_defense_radio","expose_guilt_delta":0,"expose_outcome":"You hand the telemetry keys to the remaining neutral envoys. The Order is immediately expelled from the regional trade council, and their caravans are shot on sight.","expose_standing_delta":-30,"expose_standing_fa…`
  - row 27: `{"archetype_id":"the_quartermaster","blackmail_hardening_delta":0.2,"blackmail_outcome":"You tap the top case with your boot. 'I'm suddenly developing a taste for protein,' you say. From that day forward, the quartermaster's hidden reserve becomes your personal larder. You eat well in the dark, and they sweat every time the inventory books are opened.","blackmail_resource_gain":"canned_meat","category":"bunker_internal","discovery_path":"shelter_search","discovery_source_id":"recipe_tin","expose_guilt_delta":5,"expose_outcome":"You drag the three cases into the center of the mess hall. The quartermaster tries to explain it was for winter, bu…`
  - row 28: `{"archetype_id":"the_overseer","blackmail_hardening_delta":0.15,"blackmail_outcome":"You seal the panel again. Then you tell the three people most likely to find it that there is nothing behind it — water damage, not worth exploring. The room stays yours. You take the iodide for yourself and leave the tanks where they are. A private retreat, clean air, full oxygen. In this shelter that is a kind of wealth.","blackmail_resource_gain":"private_quarters","category":"bunker_internal","discovery_path":"shelter_search","discovery_source_id":"engineers_slide_rule","expose_guilt_delta":0,"expose_outcome":"You tear out the false wall panel by hand, w…`
  - row 29: `{"archetype_id":"the_pharmacist","blackmail_hardening_delta":0.2,"blackmail_outcome":"You slide four of the vials into your own coat. You leave two behind with a note: 'I'll take the rest next week.' The user in the infirmary becomes your personal indentured servant, terrified of the withdrawal that awaits when you finally empty the stash.","blackmail_resource_gain":"pure_morphine","category":"bunker_internal","discovery_path":"shelter_search","discovery_source_id":"morphine","expose_guilt_delta":5,"expose_outcome":"You pull the tape and let the vials spill out onto the triage cot in full view of the morning clinic line. The trust in the med…`
  - row 30: `{"archetype_id":"the_guard","blackmail_hardening_delta":0.15,"blackmail_outcome":"You tap the torn edge of the ledger. 'My bunk duty and my sanitation shifts. For the rest of the year.' The guard nods miserably. You sleep soundly while they work double shifts, terrified you'll reveal the night they left the door open.","blackmail_resource_gain":"guard_favors","category":"bunker_internal","discovery_path":"document","discovery_source_id":"pocket_notebook","expose_guilt_delta":5,"expose_outcome":"You pin the forged logbook page to the bulletin board alongside a drawing of a raider's knife. Panic spreads through the bunker. The guard is strippe…`
  - row 31: `{"archetype_id":"the_surgeon","blackmail_hardening_delta":0.25,"blackmail_outcome":"You keep the page and the story, and {name} keeps operating — for your people first, at your hours, on your say-so. The best hands in the valley now answer to a piece of folded paper in your pocket. {name} does not hate you for it, which is worse: {name} understands the leverage the way a surgeon understands a clamp, and has quietly begun, you notice, teaching the young ones nothing that would ever need you.","blackmail_resource_gain":"medicine","category":"npc_personal","discovery_path":"shelter_search","discovery_source_id":"silver_scalpel","expose_guilt_de…`
  - row 32: `{"archetype_id":"the_quartermaster","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold the error and {name} holds the pencil, which means you hold the pencil. Requisitions you favor move to the top of the stack; the nine percent flows where you point it. {name} signs your slips with the same neat hand that signed the lie, and you both understand the arrangement perfectly: the book was already a fiction. You are simply the newest author.","blackmail_resource_gain":"rations","category":"bunker_internal","discovery_path":"document","discovery_source_id":"farm_ledger","expose_guilt_delta":20,"expose_outcome":"Exposed, the padding bec…`
  - row 33: `{"archetype_id":"the_journalist","blackmail_hardening_delta":0.25,"blackmail_outcome":"You own the masthead now, which means you own the voice: {name} writes what you want remembered, buries what you want forgotten, and teaches the shelter's record-keeping your grammar. A journalist who fears publication is the perfect instrument of it — every line {name} writes for you is written by someone who knows precisely what words cost. You are not spending that knowledge. You are compounding it.","blackmail_resource_gain":"influence","category":"npc_personal","discovery_path":"shelter_search","discovery_source_id":"engraved_lighter","expose_guilt_de…`
  - row 34: `{"archetype_id":"the_mechanic","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold the signature and {name} holds the wrenches, so you hold the wrenches: the shelter's machines get maintained in the order you prefer, and inspections you care about come back clean in the same soft-handed script that once cost eleven years of sleep. {name} does not resent the leverage. That is the telling part — {name} has been waiting, all this while, for someone else to hold the card.","blackmail_resource_gain":"labor","category":"npc_personal","discovery_path":"shelter_search","discovery_source_id":"mechanic_gloves","expose_guilt_delta":20,"expos…`
  - row 35: `{"archetype_id":"the_pilot","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold the manifest and {name} holds every convoy plan the shelter will ever need — weights, routes, load discipline, the pilot's eye that sees a bad plan from the air it never flies. The leverage buys all of it, promptly and without argument, because {name} has been carrying an unpaid debt for eleven years and you have simply offered terms. The terms are yours. The debt was always the point; the interest is whatever you decide it is.","blackmail_resource_gain":"trade_goods","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":…`
  - row 36: `{"archetype_id":"the_refugee","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold eleven names and {name} holds nothing else, so you hold {name}: the sharpest road-memory in the shelter, every route and every fixer's face from here to the south, spent at your discretion and logged in nobody's book but yours. {name} walks the routes, counts the caches, names the honest doors. It is good work and it is purchased work, and every settlement {name} passes on your errand is a settlement where the list could still, someday, be read aloud.","blackmail_resource_gain":"intelligence","category":"npc_personal","discovery_path":"document","dis…`
  - row 37: `{"archetype_id":"the_overseer","blackmail_hardening_delta":0.25,"blackmail_outcome":"You own the falsified column, which means you own the column-keeper: rosters that suit you, quotas that lean your way, ghosts of your choosing fed or starved in ledgers nobody audits. {name} signs your arrangements in the same careful hand that signed the dead man's eleven weeks, and the terrible efficiency of it is this — {name} does not even pretend to be corrupted. {name} is, and always was, a person who feeds whoever the pencil points at. You are simply holding the pencil now.","blackmail_resource_gain":"rations","category":"bunker_internal","discovery_p…`
  - row 38: `{"archetype_id":"the_teacher","blackmail_hardening_delta":0.25,"blackmail_outcome":"Forty-one letters, forty-one threads into families and settlements across the valley — and you hold the tin. {name} reads for you now: which families endure, which bloodlines still answer to which names, which old loyalties a folded page could still wake. It is the finest intelligence the valley contains, gathered by children who trusted a teacher with their hearts, spent by a teacher who has learned, at last, exactly what confiscation means from the other side of the desk.","blackmail_resource_gain":"influence","category":"npc_personal","discovery_path":"she…`
- Bytes: 95,078; SHA-256: `3fff37f541bfff91b6265fc2596e5ec7d6fcf664f18fecdc39b883f57d9f7349`
- Root keys: `items, schema_version`
- `items`: list[38]; union fields: `archetype_id, blackmail_hardening_delta, blackmail_outcome, blackmail_resource_gain, category, discovery_path, discovery_source_id, expose_guilt_delta, expose_outcome, expose_standing_delta, expose_standing_faction, forgiveness_affinity, forgiveness_morale, forgiveness_outcome, gating_flag, grudge_affinity, grudge_morale, grudge_outcome, keep_outcome, keep_trust_delta, secret_id, secret_text, secret_title, subject_id`
  - row 1: `{"archetype_id":"the_surgeon","blackmail_hardening_delta":0.15,"blackmail_outcome":"You wait until the clinic is empty and tell {name} that your name would like to be first on the list, next time there is a list. You do not say the word triage. You do not have to. From then on your doses are checked twice, your wounds are closed rather than taped, and the antibiotics with the dates still legible are the ones that find their way to your bunk. {name} does all of it competently and without expression, and does not look at you while doing it.","blackmail_resource_gain":"medical_supplies","category":"npc_personal","discovery_path":"direct_confess…`
  - row 2: `{"archetype_id":"the_soldier","blackmail_hardening_delta":0.2,"blackmail_outcome":"You tell {name} you want the tactical weapons stored under their bunk — the ones that weren't declared in the intake inventory. You don't say the words. You look at the board, then at {name}, and {name} understands. The weapons appear in your stores two days later, cleaned and oiled.","blackmail_resource_gain":"ammunition","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"dog_tags","expose_guilt_delta":15,"expose_outcome":"You write it on the watch-board in plain language: eighteen civilians, one family shelter, command orde…`
  - row 3: `{"archetype_id":"the_pharmacist","blackmail_hardening_delta":0.15,"blackmail_outcome":"You lean close enough to smell the sweat on their collar and whisper: one ampoule a week, uncut, delivered to the second ventilation grate. {name}'s hands tremble as they nod. From now on, they dispense your private supply with the same careful precision they once reserved for the dying. The irony is not lost on either of you.","blackmail_resource_gain":"medicine","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"morphine","expose_guilt_delta":8,"expose_outcome":"You lay the empty vials on the mess table during evening c…`
  - row 4: `{"archetype_id":"the_mother","blackmail_hardening_delta":0.25,"blackmail_outcome":"You remind {name} that the child they saved still needs a roof and clean water, and that those things aren't free. 'I need two extra shifts on the water pump,' you say. {name} looks at their living child, swallows hard, and nods. They work until their hands bleed, driven by a ghost they can't bury.","blackmail_resource_gain":"labor","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"childs_mitten","expose_guilt_delta":20,"expose_outcome":"The story works its way through the bunks like a slow infection. Mothers look at {name} …`
  - row 5: `{"archetype_id":"the_mechanic","blackmail_hardening_delta":0.15,"blackmail_outcome":"You corner {name} by the fuel tanks. 'My bunk needs a reinforced lock and a personal heater,' you say. 'You know how to route the power.' The mechanic clenches their jaw, trapped by their own lie. For the rest of the winter, your quarters are the warmest in the shelter, bought with extorted labor.","blackmail_resource_gain":"crafting_parts","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"mechanic_gloves","expose_guilt_delta":12,"expose_outcome":"You write the truth on the maintenance board for everyone to see. The next t…`
  - row 6: `{"archetype_id":"the_teacher","blackmail_hardening_delta":0.1,"blackmail_outcome":"You don't ask for much. Just private tutoring sessions, after hours, focusing on pre-war history. 'You owe the future something, after what you burned,' you tell {name}. The teacher spends their evenings exhausted, reciting memorized texts to you until their voice gives out, paying off a debt measured in pages.","blackmail_resource_gain":"research","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"pocket_notebook","expose_guilt_delta":5,"expose_outcome":"The ashes of the lost library are cataloged in the shelter's archive. T…`
  - row 7: `{"archetype_id":"the_refugee","blackmail_hardening_delta":0.15,"blackmail_outcome":"You tell {name} what you know. You say you will not act on it — provided their ration allocation is quietly supplemented from the undeclared stores they mentioned last month. {name}'s face does not change much. They were probably expecting this. They nod.","blackmail_resource_gain":"rations","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"undelivered_mail","expose_guilt_delta":10,"expose_outcome":"You submit the identity discrepancy to the registry ledger with a note: documents belonged to a deceased individual, current h…`
  - row 8: `{"archetype_id":"the_electrician","blackmail_hardening_delta":0.1,"blackmail_outcome":"You point out the unauthorized tap line. 'Route it to my sector,' you say. 'Nobody else has to know.' {name} spends two days crawling through the ductwork, rewiring the grid to siphon power to your quarters. You sleep in warmth while the rest of the shelter shivers, and {name} carries the guilt of both.","blackmail_resource_gain":"power","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"engineers_slide_rule","expose_guilt_delta":8,"expose_outcome":"You post the cause of the blackout on the community board. The survivor w…`
  - row 9: `{"archetype_id":"the_cook","blackmail_hardening_delta":0.15,"blackmail_outcome":"You visit the kitchen after lights out. 'I like my soup thick,' you whisper, tapping the hidden tin. {name} closes their eyes and nods. From then on, your bowl always has a piece of dried fruit or a scrap of real meat at the bottom, stolen from the mouths of {name}'s family to buy your silence.","blackmail_resource_gain":"rations","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"recipe_tin","expose_guilt_delta":12,"expose_outcome":"You carry the tins into the mess hall and stack them on the center table during the mid-day mea…`
  - row 10: `{"archetype_id":"the_engineer","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand full structural fortification of your private quarters.","blackmail_resource_gain":"building_materials","category":"npc_personal","discovery_path":"document","discovery_source_id":"engineers_slide_rule","expose_guilt_delta":25,"expose_outcome":"The engineering certificate is pinned to the town square bulletin.","expose_standing_delta":20,"expose_standing_faction":"faction_rebel","forgiveness_affinity":12,"forgiveness_morale":8,"forgiveness_outcome":"'The bureaucrats threatened to shoot you if you didn't sign,' the listener says. 'The blame belongs …`
  - row 11: `{"archetype_id":"the_farmer","blackmail_hardening_delta":0.15,"blackmail_outcome":"You demand the remaining secret heirloom seed packet.","blackmail_resource_gain":"seed_packet","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"family_heirloom_seeds","expose_guilt_delta":15,"expose_outcome":"The agricultural loss is entered into the provincial ledger.","expose_standing_delta":-10,"expose_standing_faction":"faction_independent","forgiveness_affinity":14,"forgiveness_morale":10,"forgiveness_outcome":"'You were resisting military looting,' the listener says. 'Nobody knew the world was ending tomorrow.'","gati…`
  - row 12: `{"archetype_id":"the_priest","blackmail_hardening_delta":0.2,"blackmail_outcome":"You sit in the confessional. 'You still have their ears,' you say. 'When the council votes on rationing tomorrow, tell them God demands sacrifice from the lower levels.' The priest bows their head, broken. The sermon the next morning is a masterpiece of manipulation, and you get exactly what you wanted.","blackmail_resource_gain":"influence","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"pocket_notebook","expose_guilt_delta":10,"expose_outcome":"The truth of the priest's empty faith spreads like a rot through the shelter. …`
  - row 13: `{"archetype_id":"the_journalist","blackmail_hardening_delta":0.15,"blackmail_outcome":"You demand the journalist's pre-war dossier on local faction leaders.","blackmail_resource_gain":"intelligence","category":"npc_personal","discovery_path":"document","discovery_source_id":"undelivered_mail","expose_guilt_delta":20,"expose_outcome":"The pre-war press suppression article is published on the shelter wall.","expose_standing_delta":15,"expose_standing_faction":"faction_rebel","forgiveness_affinity":12,"forgiveness_morale":8,"forgiveness_outcome":"'They would have disappeared you into a military prison if you ran it,' the listener says.","gating…`
  - row 14: `{"archetype_id":"the_pilot","blackmail_hardening_delta":0.15,"blackmail_outcome":"You extract navigation charts and flight instruments.","blackmail_resource_gain":"aviation_parts","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"tarnished_medal","expose_guilt_delta":18,"expose_outcome":"The court-martial charge sheet is made known to veteran groups.","expose_standing_delta":-25,"expose_standing_faction":"faction_military","forgiveness_affinity":18,"forgiveness_morale":12,"forgiveness_outcome":"'You refused a suicide run for corrupt politicians,' the listener says. 'That wasn't cowardice, it was sanity.'",…`
  - row 15: `{"archetype_id":"the_scientist","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand private filtration cartridges for personal use.","blackmail_resource_gain":"water_filters","category":"npc_personal","discovery_path":"document","discovery_source_id":"dosimeter","expose_guilt_delta":22,"expose_outcome":"The original water contamination telemetry is exposed.","expose_standing_delta":-15,"expose_standing_faction":"faction_independent","forgiveness_affinity":10,"forgiveness_morale":8,"forgiveness_outcome":"'Panic in a sealed shelter kills faster than cesium,' the listener says quietly.","gating_flag":"flag_secret_scientist_confessed…`
  - row 16: `{"archetype_id":"the_hunter","blackmail_hardening_delta":0.25,"blackmail_outcome":"You demand prime game cuts and hunting munitions.","blackmail_resource_gain":"food_and_ammo","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"engraved_lighter","expose_guilt_delta":30,"expose_outcome":"The wilderness homicide is reported to the border settlement.","expose_standing_delta":-20,"expose_standing_faction":"faction_independent","forgiveness_affinity":8,"forgiveness_morale":5,"forgiveness_outcome":"'Starvation turns human beings into wolves,' the listener whispers in horror. 'God forgive us all for what we became …`
  - row 17: `{"archetype_id":"the_nurse","blackmail_hardening_delta":0.15,"blackmail_outcome":"You keep a copy of the altered chart. From that day on, emergency medical packs and clean analgesics are diverted to your personal footlocker whenever you ask.","blackmail_resource_gain":"medical_supplies","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"nurse_fob_watch","expose_guilt_delta":18,"expose_outcome":"You bring the falsified triage log to the clinic council. Trust in the medical ward fractures, and {name} is barred from administering schedule-one narcotics.","expose_standing_delta":-10,"expose_standing_faction":"f…`
  - row 18: `{"archetype_id":"the_carpenter","blackmail_hardening_delta":0.2,"blackmail_outcome":"You threaten to call a public inspection of the workshop. In return, {name} reinforces your private quarters with double-thickness oak planks and reinforced iron braces.","blackmail_resource_gain":"building_materials","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"box_of_nails_10","expose_guilt_delta":15,"expose_outcome":"You show the cracked green pine joist to the construction crew. {name} is stripped of structural lead and demoted to sawing firewood under constant supervision.","expose_standing_delta":-12,"expose_sta…`
  - row 19: `{"archetype_id":"the_child","blackmail_hardening_delta":0.25,"blackmail_outcome":"You remind {name} that bad deeds follow people into adulthood unless someone protects them. The child timidly delivers their scavenged surface trinkets and shiny scrap to your bunk each week.","blackmail_resource_gain":"scavenged_trinkets","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"childs_drawing","expose_guilt_delta":25,"expose_outcome":"The story slips out among the younger survivors in the sleeping bays. The other children stop inviting {name} to their games, whispering that they will leave you behind if the sirens …`
  - row 20: `{"archetype_id":"the_old_man","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand the remaining municipal ration vouchers and civil defense stamps {name} kept in his lockbox. The old clerk complies without a word, his authority broken forever.","blackmail_resource_gain":"ration_stamps","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":"tarnished_pocket_watch","expose_guilt_delta":16,"expose_outcome":"The old registry stamps and clerk rosters are revealed to the assembly. Several families whose relatives vanished during the purges confront the old man, demanding his immediate expulsion from the c…`
  - row 21: `{"archetype_id":"faction_independent","blackmail_hardening_delta":0.15,"blackmail_outcome":"You agree that the ledger was lost in a fire. The syndicate agrees that your bunker's supply requisition will always be the first one loaded onto the outbound wagons.","blackmail_resource_gain":"trade_goods","category":"faction_institutional","discovery_path":"document","discovery_source_id":"farm_ledger","expose_guilt_delta":0,"expose_outcome":"You nail the ledger pages to the public board at the water exchange. The syndicate's credit rating collapses before noon, and their caravans are turned away at three borders.","expose_standing_delta":-30,"expo…`
  - row 22: `{"archetype_id":"faction_military","blackmail_hardening_delta":0.2,"blackmail_outcome":"You hand over the audit. In exchange, two crates of military-issue cartridges and a box of ceramic armor plates fall off a transport truck near your hatch.","blackmail_resource_gain":"military_weapons","category":"faction_institutional","discovery_path":"document","discovery_source_id":"train_ticket_book","expose_guilt_delta":0,"expose_outcome":"You staple copies of the audit to the checkpoint barricades. By evening, three supply convoys have been blocked by civilians who know exactly how many calories were stolen.","expose_standing_delta":25,"expose_stan…`
  - row 23: `{"archetype_id":"faction_rebel","blackmail_hardening_delta":0.2,"blackmail_outcome":"You demand the codes for the safe-house network and a crate of smuggled rifles. They pay it, but the courier who delivers it does not look you in the eye.","blackmail_resource_gain":"smuggled_gear","category":"faction_institutional","discovery_path":"document","discovery_source_id":"undelivered_mail","expose_guilt_delta":0,"expose_outcome":"You read the dispatch aloud over the neutral frequency. The farmers burn the rebel banners they had hung in their barns.","expose_standing_delta":25,"expose_standing_faction":"faction_military","forgiveness_affinity":5,"f…`
  - row 24: `{"archetype_id":"faction_iron_clique","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hand over the tape drive. Two days later, a heavy cargo truck drops four barrels of diesel and a set of carbide machine tools at your perimeter.","blackmail_resource_gain":"fuel_and_tools","category":"faction_institutional","discovery_path":"radio","discovery_source_id":"civil_defense_radio","expose_guilt_delta":0,"expose_outcome":"You play the blackbox audio over the emergency band. The sound of fists on steel echoes through every radio in the valley, and the Clique's recruitment dries up overnight.","expose_standing_delta":-40,"expose_standing_f…`
  - row 25: `{"archetype_id":"faction_meridian","blackmail_hardening_delta":0.2,"blackmail_outcome":"You surrender the original manifests. At the start of every month, a courier delivers a sealed box of iodine, clean bandages, and sterile sutures to your gate.","blackmail_resource_gain":"antibiotics","category":"faction_institutional","discovery_path":"document","discovery_source_id":"train_ticket_book","expose_guilt_delta":0,"expose_outcome":"You pin the shipping bills to the door of the largest Free Clinic. The next day, a Meridian envoy's vehicle is turned over and set on fire in the street.","expose_standing_delta":-35,"expose_standing_faction":"fact…`
  - row 26: `{"archetype_id":"faction_order","blackmail_hardening_delta":0.2,"blackmail_outcome":"You trade the encrypted drives for two crates of heavy mortar shells and a rebuilt diesel generator. The Order considers it a cheap price for their reputation.","blackmail_resource_gain":"generator_parts","category":"faction_institutional","discovery_path":"radio","discovery_source_id":"civil_defense_radio","expose_guilt_delta":0,"expose_outcome":"You hand the telemetry keys to the remaining neutral envoys. The Order is immediately expelled from the regional trade council, and their caravans are shot on sight.","expose_standing_delta":-30,"expose_standing_fa…`
  - row 27: `{"archetype_id":"the_quartermaster","blackmail_hardening_delta":0.2,"blackmail_outcome":"You tap the top case with your boot. 'I'm suddenly developing a taste for protein,' you say. From that day forward, the quartermaster's hidden reserve becomes your personal larder. You eat well in the dark, and they sweat every time the inventory books are opened.","blackmail_resource_gain":"canned_meat","category":"bunker_internal","discovery_path":"shelter_search","discovery_source_id":"recipe_tin","expose_guilt_delta":5,"expose_outcome":"You drag the three cases into the center of the mess hall. The quartermaster tries to explain it was for winter, bu…`
  - row 28: `{"archetype_id":"the_overseer","blackmail_hardening_delta":0.15,"blackmail_outcome":"You seal the panel again. Then you tell the three people most likely to find it that there is nothing behind it — water damage, not worth exploring. The room stays yours. You take the iodide for yourself and leave the tanks where they are. A private retreat, clean air, full oxygen. In this shelter that is a kind of wealth.","blackmail_resource_gain":"private_quarters","category":"bunker_internal","discovery_path":"shelter_search","discovery_source_id":"engineers_slide_rule","expose_guilt_delta":0,"expose_outcome":"You tear out the false wall panel by hand, w…`
  - row 29: `{"archetype_id":"the_pharmacist","blackmail_hardening_delta":0.2,"blackmail_outcome":"You slide four of the vials into your own coat. You leave two behind with a note: 'I'll take the rest next week.' The user in the infirmary becomes your personal indentured servant, terrified of the withdrawal that awaits when you finally empty the stash.","blackmail_resource_gain":"pure_morphine","category":"bunker_internal","discovery_path":"shelter_search","discovery_source_id":"morphine","expose_guilt_delta":5,"expose_outcome":"You pull the tape and let the vials spill out onto the triage cot in full view of the morning clinic line. The trust in the med…`
  - row 30: `{"archetype_id":"the_guard","blackmail_hardening_delta":0.15,"blackmail_outcome":"You tap the torn edge of the ledger. 'My bunk duty and my sanitation shifts. For the rest of the year.' The guard nods miserably. You sleep soundly while they work double shifts, terrified you'll reveal the night they left the door open.","blackmail_resource_gain":"guard_favors","category":"bunker_internal","discovery_path":"document","discovery_source_id":"pocket_notebook","expose_guilt_delta":5,"expose_outcome":"You pin the forged logbook page to the bulletin board alongside a drawing of a raider's knife. Panic spreads through the bunker. The guard is strippe…`
  - row 31: `{"archetype_id":"the_surgeon","blackmail_hardening_delta":0.25,"blackmail_outcome":"You keep the page and the story, and {name} keeps operating — for your people first, at your hours, on your say-so. The best hands in the valley now answer to a piece of folded paper in your pocket. {name} does not hate you for it, which is worse: {name} understands the leverage the way a surgeon understands a clamp, and has quietly begun, you notice, teaching the young ones nothing that would ever need you.","blackmail_resource_gain":"medicine","category":"npc_personal","discovery_path":"shelter_search","discovery_source_id":"silver_scalpel","expose_guilt_de…`
  - row 32: `{"archetype_id":"the_quartermaster","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold the error and {name} holds the pencil, which means you hold the pencil. Requisitions you favor move to the top of the stack; the nine percent flows where you point it. {name} signs your slips with the same neat hand that signed the lie, and you both understand the arrangement perfectly: the book was already a fiction. You are simply the newest author.","blackmail_resource_gain":"rations","category":"bunker_internal","discovery_path":"document","discovery_source_id":"farm_ledger","expose_guilt_delta":20,"expose_outcome":"Exposed, the padding bec…`
  - row 33: `{"archetype_id":"the_journalist","blackmail_hardening_delta":0.25,"blackmail_outcome":"You own the masthead now, which means you own the voice: {name} writes what you want remembered, buries what you want forgotten, and teaches the shelter's record-keeping your grammar. A journalist who fears publication is the perfect instrument of it — every line {name} writes for you is written by someone who knows precisely what words cost. You are not spending that knowledge. You are compounding it.","blackmail_resource_gain":"influence","category":"npc_personal","discovery_path":"shelter_search","discovery_source_id":"engraved_lighter","expose_guilt_de…`
  - row 34: `{"archetype_id":"the_mechanic","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold the signature and {name} holds the wrenches, so you hold the wrenches: the shelter's machines get maintained in the order you prefer, and inspections you care about come back clean in the same soft-handed script that once cost eleven years of sleep. {name} does not resent the leverage. That is the telling part — {name} has been waiting, all this while, for someone else to hold the card.","blackmail_resource_gain":"labor","category":"npc_personal","discovery_path":"shelter_search","discovery_source_id":"mechanic_gloves","expose_guilt_delta":20,"expos…`
  - row 35: `{"archetype_id":"the_pilot","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold the manifest and {name} holds every convoy plan the shelter will ever need — weights, routes, load discipline, the pilot's eye that sees a bad plan from the air it never flies. The leverage buys all of it, promptly and without argument, because {name} has been carrying an unpaid debt for eleven years and you have simply offered terms. The terms are yours. The debt was always the point; the interest is whatever you decide it is.","blackmail_resource_gain":"trade_goods","category":"npc_personal","discovery_path":"direct_confession","discovery_source_id":…`
  - row 36: `{"archetype_id":"the_refugee","blackmail_hardening_delta":0.25,"blackmail_outcome":"You hold eleven names and {name} holds nothing else, so you hold {name}: the sharpest road-memory in the shelter, every route and every fixer's face from here to the south, spent at your discretion and logged in nobody's book but yours. {name} walks the routes, counts the caches, names the honest doors. It is good work and it is purchased work, and every settlement {name} passes on your errand is a settlement where the list could still, someday, be read aloud.","blackmail_resource_gain":"intelligence","category":"npc_personal","discovery_path":"document","dis…`
  - row 37: `{"archetype_id":"the_overseer","blackmail_hardening_delta":0.25,"blackmail_outcome":"You own the falsified column, which means you own the column-keeper: rosters that suit you, quotas that lean your way, ghosts of your choosing fed or starved in ledgers nobody audits. {name} signs your arrangements in the same careful hand that signed the dead man's eleven weeks, and the terrible efficiency of it is this — {name} does not even pretend to be corrupted. {name} is, and always was, a person who feeds whoever the pencil points at. You are simply holding the pencil now.","blackmail_resource_gain":"rations","category":"bunker_internal","discovery_p…`
  - row 38: `{"archetype_id":"the_teacher","blackmail_hardening_delta":0.25,"blackmail_outcome":"Forty-one letters, forty-one threads into families and settlements across the valley — and you hold the tin. {name} reads for you now: which families endure, which bloodlines still answer to which names, which old loyalties a folded page could still wake. It is the finest intelligence the valley contains, gathered by children who trusted a teacher with their hearts, spent by a teacher who has learned, at last, exactly what confiscation means from the other side of the desk.","blackmail_resource_gain":"influence","category":"npc_personal","discovery_path":"she…`

# Appendix D — Current caller/reference graph

### `GuiltSourceCatalog` (11 sampled current references)
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:32: public sealed class GuiltSourceCatalog
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:50: public GuiltSourceCatalog() { }
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:52: public GuiltSourceCatalog(IEnumerable<GuiltSourceDefinition> items)
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:65: public static GuiltSourceCatalog FromJson(string json)
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:68: return new GuiltSourceCatalog();
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:75: return new GuiltSourceCatalog(root?.Items ?? (IEnumerable<GuiltSourceDefinition>)Array.Empty<GuiltSourceDefinition>());
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:78: public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory)
- Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs:82: return new GuiltSourceCatalog();
- Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs:22: public void GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle()
- Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs:25: var catalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
- Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs:143: var guiltCatalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
### `GuiltInsomniaSystem` (18 sampled current references)
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:191: /// Observe-only projection of <see cref="GuiltInsomniaSystem"/>
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:194: /// <see cref="GuiltInsomniaSystem.HighSeverityThreshold"/> reads as
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:204: private readonly GuiltInsomniaSystem _guilt;
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:206: public GuiltInsomniaAfflictionHandler(GuiltInsomniaSystem guilt)
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:226: StageLabel = severity >= GuiltInsomniaSystem.HighSeverityThreshold
- Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs:37: public class GuiltInsomniaSystem
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:399: ["guilt_sources.json"] = new[] { "GuiltInsomniaSystem" },
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:726: ["guilt_sources.json"] = "GuiltInsomniaSystem",
- Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1005: ["guilt_sources.json"] = new[] { "GuiltInsomniaSystem" },
- Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs:84: GuiltInsomniaSystem? guilt = null,
- Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs:19: /// GuiltInsomniaSystem, LeadershipSystem) without introducing any parallel faith or piety meters.
- src/Main.FlagshipInstitutions.cs:387: /// GuiltInsomniaSystem. Siege paranoia reads as extreme
- src/Host/Phase0HostSession.cs:235: public GuiltInsomniaSystem Guilt { get; }
- src/Host/Phase0HostSession.cs:351: Guilt = new GuiltInsomniaSystem();
- src/Host/HostCli.PanelTests.cs:2749: Check(session.Guilt.GetInsomniaSeverity("elena_vasquez") >= Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold,
- src/UI/Phase0Panel.cs:173: fx.guiltInsomniaSeverity >= Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold
- src/UI/Phase0Panel.cs:291: if (v < Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold) return "MODERATE";
- Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:73: var guilt = new GuiltInsomniaSystem();
### `RecordGuilt` (18 sampled current references)
- Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs:65: public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay)
- Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs:106: guilt.RecordGuilt(entry.subject_id, $"secret_exposed_{secretId}", entry.expose_guilt_delta, currentDay);
- src/Main.UiTests.Phase0.cs:55: _phase0.RecordGuilt("elena_vasquez", "choice_imposed_hardship", 0.8f);
- src/Host/Phase0HostSession.cs:719: public string RecordGuilt(string survivorId, string sourceId, float severity)
- src/Host/Phase0HostSession.cs:721: Guilt.RecordGuilt(survivorId, sourceId, severity, CurrentDay);
- src/Host/HostCli.PanelTests.cs:2748: session.RecordGuilt("elena_vasquez", "choice_left_ally_behind", 0.9f);
- src/UI/Phase0Panel.cs:244: () => _phase0.RecordGuilt(id, "choice_imposed_hardship", 0.8f)));
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:10: public void RecordGuilt_IncreasesSeverity()
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:13: sys.RecordGuilt("sv_1", "ration_cutting", 0.5f, 100);
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:19: public void RecordGuilt_MultipleSources_CapsAt1()
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:22: sys.RecordGuilt("sv_1", "source_a", 0.6f, 100);
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:23: sys.RecordGuilt("sv_1", "source_b", 0.6f, 101);
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:28: public void RecordGuilt_FiresEvent()
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:33: sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:38: public void RecordGuilt_CriticalThreshold_FiresEvent()
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:43: sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:51: sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
- Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs:67: sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
### `Phase0HostSession` (18 sampled current references)
- src/Main.FlagshipInstitutions.cs:397: private Phase0HostSession? P0
- src/Main.Phase0.cs:36: private Phase0HostSession _phase0 = null!;
- src/Main.Phase0.cs:105: _phase0 = new Phase0HostSession(dependency: _medical.Engine);
- src/Host/Phase0SaveStore.cs:5: // Host Caller: Main.Phase0 / Phase0HostSession
- src/Host/Phase0HostSession.cs:77: /// fully-bound instance and assigns it to <see cref="Phase0HostSession.Consumers"/>
- src/Host/Phase0HostSession.cs:229: public sealed class Phase0HostSession
- src/Host/Phase0HostSession.cs:301: public Phase0HostSession(int seed = DefaultSeed, ChemicalDependencySystem dependency = null!)
- src/Host/HostCli.PanelTests.cs:2519: var session = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:2818: var fresh = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:4307: var phase0resp = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:4353: var phase0respFresh = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:4360: var phase0b = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:4366: var phase0c = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:4397: var intPhase0 = new Phase0HostSession();
- src/Host/HostCli.PanelTests.cs:4405: var intRestored = new Phase0HostSession();
- src/UI/Phase0Panel.cs:26: private Phase0HostSession? _phase0;
- src/UI/Phase0Panel.cs:95: public void Bind(Phase0HostSession phase0, SurvivorsHostSession? survivors = null, MedicalPipelineCoordinator? pipeline = null)
- src/UI/Phase0Panel.cs:266: /// <c>Phase0HostSession.ApplyInhaler</c> (that stays CLI/test-only).
### `GuiltInsomniaAfflictionHandler` (10 sampled current references)
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:198: public sealed class GuiltInsomniaAfflictionHandler : PsychologyObserverHandlerBase
- Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs:206: public GuiltInsomniaAfflictionHandler(GuiltInsomniaSystem guilt)
- src/Main.Medical.cs:264: pipeline.RegisterHandler(new Ashfall.Core.Medical.GuiltInsomniaAfflictionHandler(_phase0.Guilt));
- Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs:266: Assert.Contains("GuiltInsomniaAfflictionHandler", source);
- Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs:37: public GuiltInsomniaAfflictionHandler GuiltHandler { get; }
- Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs:43: GuiltHandler = new GuiltInsomniaAfflictionHandler(Guilt);
- Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs:168: Assert.Equal(GuiltInsomniaAfflictionHandler.StageInsomnia, episode!.StageLabel);
- Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs:171: Assert.Equal(GuiltInsomniaAfflictionHandler.SymptomInsomnia, symptom.SymptomId);
- Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs:183: Assert.Equal(GuiltInsomniaAfflictionHandler.StageCriticalInsomnia, episode!.StageLabel);
- Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs:257: Assert.Contains(record.Symptoms, s => s.SymptomId == GuiltInsomniaAfflictionHandler.SymptomInsomnia);

# Appendix E — Current focused-test inventory

Current test declaration inventory: 62 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs` — 30 test declarations; bytes=5,609; SHA-256=`d1fb46927f6c5062bb6d128b215a21eb690cfa2dc7fb2fe58fa8a3b0f89fcc04`
- 00009: [Fact]
- 00010: public void RecordGuilt_IncreasesSeverity()
- 00018: [Fact]
- 00019: public void RecordGuilt_MultipleSources_CapsAt1()
- 00027: [Fact]
- 00028: public void RecordGuilt_FiresEvent()
- 00037: [Fact]
- 00038: public void RecordGuilt_CriticalThreshold_FiresEvent()
- 00047: [Fact]
- 00048: public void ApplySedative_ReducesSeverity()
- 00056: [Fact]
- 00057: public void ApplySedative_NoGuilt_ReturnsFalse()
- 00063: [Fact]
- 00064: public void ResolveDialogue_RemovesMostRecentGuilt()
- 00073: [Fact]
- 00074: public void ResolveDialogue_LastSource_FiresResolved()
- 00084: [Fact]
- 00085: public void SleepQuality_LowerWithGuilt()
- 00093: [Fact]
- 00094: public void SleepQuality_SedativeHalvesPenalty()
- 00104: [Fact]
- 00105: public void Tick_ExpiresOldGuilt()
- 00114: [Fact]
- 00115: public void Tick_DecaysSedative()
- 00126: [Fact]
- 00127: public void CaptureRestore_Roundtrip()
- 00143: [Fact]
- 00144: public void RestoreNull_DoesNotCrash()
- 00151: [Fact]
- 00152: public void RecordGuilt_RejectsEmptyId()
### `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs` — 16 test declarations; bytes=13,182; SHA-256=`f35442bfdf970acda3b73451801713ca7b2a86ab4acf2f51c3640138067767af`
- 00028: [Fact]
- 00029: public void Catalog_LoadsAndHasExactly40GuiltSources()
- 00105: [Fact]
- 00106: public void Catalog_CategoryDistribution_MatchesPlan66Specification()
- 00181: [Fact]
- 00182: public void Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid()
- 00223: [Fact]
- 00224: public void Catalog_SeverityDistribution_IsWellCalibrated()
- 00249: [Fact]
- 00250: public void GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically()
- 00284: [Fact]
- 00285: public void GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold()
- 00311: [Fact]
- 00312: public void GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords()
- 00336: [Fact]
- 00337: public void GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly()
### `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` — 6 test declarations; bytes=9,232; SHA-256=`2447468de613d9a6dc5dfa4f4324293977ead2b359f3fcd1ba9af931770176e5`
- 00021: [Fact]
- 00022: public void GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle()
- 00095: [Fact]
- 00096: public void WallCarvingCatalog_And_MoraleBandSelection_FullContract()
- 00139: [Fact]
- 00140: public void PsychologicalStress_And_CulturalTrace_SystemCoupling()
### `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs` — 10 test declarations; bytes=5,153; SHA-256=`ce1fab1d5f8ae306d6a960403b523ec53733f42d05b45511e5a686a4a6ae84eb`
- 00015: [Fact]
- 00016: public void PhantomMemory_Motivation_BoostsWorkSpeedAndDecays()
- 00041: [Fact]
- 00042: public void TradeSpecialty_CraftingItems_AdvancesTierAndMasters()
- 00075: [Fact]
- 00076: public void FinalWish_CompletedWish_GrantsPermanentShelterMoraleBuff()
- 00097: [Fact]
- 00098: public void GuiltInsomnia_RecordedGuilt_RaisesInsomniaSeverity()
- 00112: [Fact]
- 00113: public void RespiratoryDegeneration_AshZoneExposure_ReducesStamina()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Guilt Sources: Forty-Row Moral Consequence Vocabulary and Phase-0 Guilt/Sleep Ownership` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan calls all five incident/three wish/three psychology wires live; current caller proof is required.
- A severity in JSON is not itself a gameplay mutation.

## H.2 Evidence-strength corrections
- Expose the catalog-loading gap instead of hiding it.
- Trace one real trigger end-to-end.
- Preserve idempotency and restore.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Guilt Sources: Forty-Row Moral Consequence Vocabulary and Phase-0 Guilt/Sleep Ownership while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use GuiltSourceCatalog for static pattern/description/severity vocabulary.
- Use GuiltInsomniaSystem for mutable guilt and sleep consequences.
- Use Phase0HostSession as the existing lifecycle/save façade.
- Use canonical choice/incident/medical owners for triggers and relief.
- Present current state only.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement guilt sources: forty-row moral consequence vocabulary and phase-0 guilt/sleep ownership arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No new guilt owner or save section is proposed.
- The final handoff must state whether catalog binding is live, direct, or unresolved.

## J.2 Questions deliberately left open
- Should Phase0HostSession own a strict catalog binding, or should each canonical choice owner resolve its own row?
- Which current choice vocabulary is closed enough for validation?

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

The original file at `HEAD:piagentsplans/66-guilt-sources-expansion.md` contained 4,966 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 66 — Guilt Sources Expansion (20 → 40 guilt triggers)

## Goal (2 lines)
Expand `guilt_sources.json` from 20 verified entries to 40. The guilt system tracks
psychological consequences of player choices (cutting rations, refusing shelter, leaving
someone behind, taking from the dead). Each guilt source has a choice pattern, severity,
description, and title. The system is wired but 20 triggers is too few for a full campaign.

## Why (P2)
- Verified: `guilt_sources.json` has 20 entries (choice_pattern, severity, description,
  title). The guilt system feeds the psychological-contamination pillar (existing 27C).
- Creates the moral-weight pillar: guilt is the invisible cost of survival decisions.
  More triggers mean more choices carry weight — the player can't avoid guilt, only
  choose which guilt to carry.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/guilt_sources.json` (expand 20 → 40 guilt triggers)
- Read-only: confirm the guilt system consumer — `grep -rn "guilt\|Guilt" Assets/Ashfall.Core/`
  to find the loader and confirm the schema (choice_pattern, severity 0.0–1.0, description,
  title)

## Content grammar (per guilt source)
- choice_pattern: the player action that triggers guilt (cut_ration, refuse_shelter,
  leave_behind, take_from_dead, abandon_quest, ignore_distress, sacrifice_survivor,
  betray_faction, execute_prisoner, steal_from_ally, etc.).
- severity: 0.1 (minor) → 1.0 (devastating) — affects how much guilt accumulates.
- description: 1-2 sentences in ASHFALL tone (cold, exhausted, human, restrained). The
  guilt is shown through physical/emotional detail, not moralizing. Skill `ashfall-write`.
- title: 2-5 words, evocative.
- system_link: optional — which system the guilt source connects to (NeedsSystem for
  ration cuts, CombatTraumaSystem for combat guilt, MemorialSystem for death guilt).

## Steps
1. Find the guilt system consumer to confirm the schema and how severity applies.
2. Read the 20 existing guilt sources to understand the choice patterns and avoid
   duplication.
3. Author 20 new guilt sources across 8 categories:
   - Resource decisions (4): hoard medicine while others die, trade away food a
     settlement needs, use contaminated supplies knowingly, burn fuel for comfort while
     others freeze.
   - Shelter decisions (3): refuse a refugee entry, expel a survivor for efficiency,
     hide a cache from allies.
   - Expedition decisions (3): abandon a quest to save resources, leave a wounded
     survivor behind, retreat from a rescue to avoid combat.
   - Combat decisions (3): execute a surrendered enemy, use civilians as bait, kill a
     former ally on the other side (feeds Plan 45/63).
   - Social decisions (3): betray a faction's trust for personal gain, inform on a
     survivor to a faction, break a promise to a dying survivor (feeds Plan 65).
   - Medical decisions (2): withhold painkillers from a dying survivor to save them,
     triage someone last because they're less useful (feeds existing 09B).
   - Scavenging decisions (1): take a family's last supplies from their home.
   - Leadership decisions (1): order a survivor to their death for the group's survival.
4. Give each source: choice_pattern, severity, description, title, optional system_link.
5. Cross-reference: every system_link references an existing system; every choice_pattern
   is unique (no duplicates).
6. Wire 5 guilt sources to Plan 57 incidents (guilt triggers fire as shelter incidents
   — a survivor confronts the player about a past choice).
7. Wire 3 guilt sources to Plan 65 final wishes (breaking a promise to a dying survivor
   generates devastating guilt).
8. Wire 3 guilt sources to existing 27C psychological contamination (accumulated guilt
   triggers trauma episodes).
9. Validate: `--data-integrity-selftest`; confirm guilt accumulates on the triggering
   choice in a headless boot.
10. xUnit: guilt catalog loads, all choice_patterns unique, severity applies correctly,
    guilt accumulates and persists, save round-trip preserves guilt state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `guilt_sources.json` has 40 guilt sources (20 existing + 20 new), all choice_patterns
  unique, 5 wired to incidents, 3 wired to final wishes, 3 wired to psychological
  contamination, guilt accumulates and persists, save round-trip green, integrity +
  tests green.

## Follow-on
- Plan 57 (incidents) — guilt triggers fire as shelter confrontations.
- Plan 65 (final wishes) — breaking a promise generates guilt.
- Existing 27C (psychological contamination) — accumulated guilt triggers trauma.
- Existing 21C (confessions) — guilt drives survivors to confess.
- Existing 30B (mourning) — guilt from death-related choices feeds mourning.

```

## End of Plan 66 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` — 106 lines; 3,555 bytes; SHA-256 `9b564dbdeb0c72951c742128a02b2c85a3908d198c821e66aedfe5e87276859e`
Declaration index:
- 00010: public sealed class GuiltSourceDefinition
- 00024: public string FormatDescription(string survivorName)
- 00032: public sealed class GuiltSourceCatalog
- 00034: private sealed class CatalogRoot
- 00065: public static GuiltSourceCatalog FromJson(string json)
- 00078: public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory)
- 00088: public GuiltSourceDefinition? GetByPattern(string choicePattern)
- 00094: public bool TryGetSeverity(string choicePattern, out float severity)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using System.Text.Json.Serialization;
00007:
00008: namespace Ashfall.Core.Survivors
00009: {
00010:     public sealed class GuiltSourceDefinition
00011:     {
00012:         [JsonPropertyName("choice_pattern")]
00013:         public string ChoicePattern { get; set; } = string.Empty;
00014:
00015:         [JsonPropertyName("severity")]
00016:         public float Severity { get; set; }
00017:
00018:         [JsonPropertyName("title")]
00019:         public string Title { get; set; } = string.Empty;
00020:
00021:         [JsonPropertyName("description")]
00022:         public string Description { get; set; } = string.Empty;
00023:
00024:         public string FormatDescription(string survivorName)
00025:         {
00026:             if (string.IsNullOrEmpty(Description))
00027:                 return string.Empty;
00028:             return Description.Replace("{name}", survivorName ?? "Someone");
00029:         }
00030:     }
00031:
00032:     public sealed class GuiltSourceCatalog
00033:     {
00034:         private sealed class CatalogRoot
00035:         {
00036:             [JsonPropertyName("schema_version")]
00037:             public int SchemaVersion { get; set; }
00038:
00039:             [JsonPropertyName("items")]
00040:             public List<GuiltSourceDefinition> Items { get; set; } = new List<GuiltSourceDefinition>();
00041:         }
00042:
00043:         private readonly List<GuiltSourceDefinition> _items = new List<GuiltSourceDefinition>();
00044:         private readonly Dictionary<string, GuiltSourceDefinition> _byPattern =
00045:             new Dictionary<string, GuiltSourceDefinition>(StringComparer.Ordinal);
00046:
00047:         public IReadOnlyList<GuiltSourceDefinition> Items => _items;
00048:         public int Count => _items.Count;
00049:
00050:         public GuiltSourceCatalog() { }
00051:
00052:         public GuiltSourceCatalog(IEnumerable<GuiltSourceDefinition> items)
00053:         {
00054:             if (items != null)
00055:             {
00056:                 foreach (var item in items)
00057:                 {
00058:                     _items.Add(item);
00059:                     if (!string.IsNullOrEmpty(item.ChoicePattern))
00060:                         _byPattern[item.ChoicePattern] = item;
00061:                 }
00062:             }
00063:         }
00064:
00065:         public static GuiltSourceCatalog FromJson(string json)
00066:         {
00067:             if (string.IsNullOrWhiteSpace(json))
00068:                 return new GuiltSourceCatalog();
00069:
00070:             var root = JsonSerializer.Deserialize<CatalogRoot>(json, new JsonSerializerOptions
00071:             {
00072:                 PropertyNameCaseInsensitive = true
00073:             });
00074:
00075:             return new GuiltSourceCatalog(root?.Items ?? (IEnumerable<GuiltSourceDefinition>)Array.Empty<GuiltSourceDefinition>());
00076:         }
00077:
00078:         public static GuiltSourceCatalog LoadFromDirectory(string dataDirectory)
00079:         {
00080:             var filePath = Path.Combine(dataDirectory, "guilt_sources.json");
00081:             if (!File.Exists(filePath))
00082:                 return new GuiltSourceCatalog();
00083:
00084:             var json = File.ReadAllText(filePath);
00085:             return FromJson(json);
00086:         }
00087:
00088:         public GuiltSourceDefinition? GetByPattern(string choicePattern)
00089:         {
00090:             if (string.IsNullOrEmpty(choicePattern)) return null;
00091:             return _byPattern.TryGetValue(choicePattern, out var def) ? def : null;
00092:         }
00093:
00094:         public bool TryGetSeverity(string choicePattern, out float severity)
00095:         {
00096:             severity = 0f;
00097:             if (string.IsNullOrEmpty(choicePattern)) return false;
00098:             if (_byPattern.TryGetValue(choicePattern, out var def))
00099:             {
00100:                 severity = def.Severity;
00101:                 return true;
00102:             }
00103:             return false;
00104:         }
00105:     }
00106: }
```

## `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` — 224 lines; 8,840 bytes; SHA-256 `0edeee9f38c11329e4e73ccd86621ba448cf4458d0432560f479a9e406685275`
Declaration index:
- 00009: public sealed class GuiltRecord
- 00017: public sealed class GuiltInsomniaSaveState
- 00023: public sealed class GuiltSurvivorState
- 00037: public class GuiltInsomniaSystem
- 00055: private GuiltSurvivorState GetOrCreate(string survivorId)
- 00065: public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay)
- 00080: public bool ApplySedative(string survivorId)
- 00091: public bool ResolveGuiltThroughDialogue(string survivorId)
- 00105: public void ApplyTherapyRelief(string survivorId, float fraction)
- 00116: public float GetSleepQualityMultiplier(string survivorId)
- 00124: public float GetInsomniaSeverity(string survivorId)
- 00129: public int GetGuiltSourceCount(string survivorId)
- 00134: public void Tick(string survivorId, float gameHours, int currentDay)
- 00165: private void UpdateInsomniaSeverity(GuiltSurvivorState state)
- 00175: public GuiltInsomniaSaveState CaptureState()
- 00199: public void RestoreState(GuiltInsomniaSaveState save)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Survivors
00007: {
00008:     [Serializable]
00009:     public sealed class GuiltRecord
00010:     {
00011:         public string sourceId = string.Empty;
00012:         public int dayRecorded;
00013:         public float severity;
00014:     }
00015:
00016:     [Serializable]
00017:     public sealed class GuiltInsomniaSaveState
00018:     {
00019:         public List<GuiltSurvivorState> survivors = new List<GuiltSurvivorState>();
00020:     }
00021:
00022:     [Serializable]
00023:     public sealed class GuiltSurvivorState
00024:     {
00025:         public string survivorId = string.Empty;
00026:         public float insomniaSeverity;
00027:         public float sedativeCompensationHours;
00028:         public List<GuiltRecord> guiltSources = new List<GuiltRecord>();
00029:     }
00030:
00031:     /// <summary>
00032:     /// ASHFALL: THE MASSIVE CONTENT EXPANSION — Guilt-Driven Insomnia System.
00033:     /// Ruthless decisions create guilt records that multiply sleep quality penalties.
00034:     /// Sedatives or interpersonal dialogue can compensate. Engine-agnostic: uses
00035:     /// string survivor IDs, raises events, save/load safe.
00036:     /// </summary>
00037:     public class GuiltInsomniaSystem
00038:     {
00039:         public const float SleepQualityPenaltyPerSeverity = 0.50f;
00040:         public const float SedativeCompensationHours = 12f;
00041:         public const float SedativeSeverityReduction = 0.40f;
00042:         public const float DialogueSeverityReduction = 0.25f;
00043:         public const float NaturalDecayPerDay = 0.05f;
00044:         public const float HighSeverityThreshold = 0.7f;
00045:         public const int GuiltExpiryDays = 30;
00046:
00047:         public event Action<string, GuiltRecord> OnGuiltRecorded;
00048:         public event Action<string> OnGuiltResolved;
00049:         public event Action<string> OnGuiltInsomniaCritical;
00050:         public event Action OnStateChanged;
00051:
00052:         private readonly Dictionary<string, GuiltSurvivorState> _bySurvivor =
00053:             new Dictionary<string, GuiltSurvivorState>(StringComparer.Ordinal);
00054:
00055:         private GuiltSurvivorState GetOrCreate(string survivorId)
00056:         {
00057:             if (!_bySurvivor.TryGetValue(survivorId, out var state))
00058:             {
00059:                 state = new GuiltSurvivorState { survivorId = survivorId };
00060:                 _bySurvivor[survivorId] = state;
00061:             }
00062:             return state;
00063:         }
00064:
00065:         public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay)
00066:         {
00067:             if (string.IsNullOrEmpty(survivorId) || severity <= 0f) return;
00068:             var state = GetOrCreate(survivorId);
00069:             state.guiltSources.Add(new GuiltRecord
00070:             {
00071:                 sourceId = sourceId ?? string.Empty,
00072:                 dayRecorded = Math.Max(1, currentDay),
00073:                 severity = severity
00074:             });
00075:             UpdateInsomniaSeverity(state);
00076:             OnGuiltRecorded?.Invoke(survivorId, state.guiltSources[state.guiltSources.Count - 1]);
00077:             OnStateChanged?.Invoke();
00078:         }
00079:
00080:         public bool ApplySedative(string survivorId)
00081:         {
00082:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
00083:             if (state.insomniaSeverity <= 0f) return false;
00084:             state.sedativeCompensationHours = SedativeCompensationHours;
00085:             float old = state.insomniaSeverity;
00086:             state.insomniaSeverity = Math.Max(0f, state.insomniaSeverity - SedativeSeverityReduction);
00087:             OnStateChanged?.Invoke();
00088:             return state.insomniaSeverity < old;
00089:         }
00090:
00091:         public bool ResolveGuiltThroughDialogue(string survivorId)
00092:         {
00093:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
00094:             if (state.guiltSources.Count == 0) return false;
00095:             state.guiltSources.RemoveAt(state.guiltSources.Count - 1);
00096:             UpdateInsomniaSeverity(state);
00097:             if (state.guiltSources.Count == 0)
00098:                 OnGuiltResolved?.Invoke(survivorId);
00099:             OnStateChanged?.Invoke();
00100:             return true;
00101:         }
00102:
00103:         /// <summary>Canonical therapeutic relief (flagship sanatorium Task 8):
00104:         /// scales insomnia severity down by the authored fraction (0..1).</summary>
00105:         public void ApplyTherapyRelief(string survivorId, float fraction)
00106:         {
00107:             if (string.IsNullOrEmpty(survivorId)) return;
00108:             fraction = Math.Clamp(fraction, 0f, 1f);
00109:             if (fraction <= 0f) return;
00110:             var state = _bySurvivor.TryGetValue(survivorId, out var s) ? s : null;
00111:             if (state == null) return;
00112:             state.insomniaSeverity = Math.Max(0f, state.insomniaSeverity * (1f - fraction));
00113:             OnStateChanged?.Invoke();
00114:         }
00115:
00116:         public float GetSleepQualityMultiplier(string survivorId)
00117:         {
00118:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return 1f;
00119:             float penalty = state.insomniaSeverity * SleepQualityPenaltyPerSeverity;
00120:             if (state.sedativeCompensationHours > 0f) penalty *= 0.5f;
00121:             return Math.Max(0.1f, 1f - penalty);
00122:         }
00123:
00124:         public float GetInsomniaSeverity(string survivorId)
00125:         {
00126:             return _bySurvivor.TryGetValue(survivorId, out var state) ? state.insomniaSeverity : 0f;
00127:         }
00128:
00129:         public int GetGuiltSourceCount(string survivorId)
00130:         {
00131:             return _bySurvivor.TryGetValue(survivorId, out var state) ? state.guiltSources.Count : 0;
00132:         }
00133:
00134:         public void Tick(string survivorId, float gameHours, int currentDay)
00135:         {
00136:             if (!_bySurvivor.TryGetValue(survivorId, out var state)) return;
00137:
00138:             if (state.sedativeCompensationHours > 0f)
00139:             {
00140:                 state.sedativeCompensationHours = Math.Max(0f, state.sedativeCompensationHours - gameHours);
00141:                 if (state.sedativeCompensationHours <= 0f)
00142:                     UpdateInsomniaSeverity(state);
00143:             }
00144:
00145:             if (state.guiltSources.Count > 0)
00146:             {
00147:                 for (int i = state.guiltSources.Count - 1; i >= 0; i--)
00148:                 {
00149:                     if (currentDay - state.guiltSources[i].dayRecorded > GuiltExpiryDays)
00150:                         state.guiltSources.RemoveAt(i);
00151:                 }
00152:                 if (state.guiltSources.Count == 0)
00153:                 {
00154:                     state.insomniaSeverity = 0f;
00155:                     OnGuiltResolved?.Invoke(survivorId);
00156:                 }
00157:                 else
00158:                 {
00159:                     UpdateInsomniaSeverity(state);
00160:                 }
00161:             }
00162:             OnStateChanged?.Invoke();
00163:         }
00164:
00165:         private void UpdateInsomniaSeverity(GuiltSurvivorState state)
00166:         {
00167:             float total = 0f;
00168:             for (int i = 0; i < state.guiltSources.Count; i++)
00169:                 total += state.guiltSources[i].severity;
00170:             state.insomniaSeverity = Math.Min(1f, total);
00171:             if (state.insomniaSeverity >= HighSeverityThreshold)
00172:                 OnGuiltInsomniaCritical?.Invoke(state.survivorId);
00173:         }
00174:
00175:         public GuiltInsomniaSaveState CaptureState()
00176:         {
00177:             var save = new GuiltInsomniaSaveState();
00178:             foreach (var kv in _bySurvivor)
00179:             {
00180:                 var s = kv.Value;
00181:                 var copy = new GuiltSurvivorState
00182:                 {
00183:                     survivorId = s.survivorId,
00184:                     insomniaSeverity = s.insomniaSeverity,
00185:                     sedativeCompensationHours = s.sedativeCompensationHours
00186:                 };
00187:                 foreach (var g in s.guiltSources)
00188:                     copy.guiltSources.Add(new GuiltRecord
00189:                     {
00190:                         sourceId = g.sourceId,
00191:                         dayRecorded = g.dayRecorded,
00192:                         severity = g.severity
00193:                     });
00194:                 save.survivors.Add(copy);
00195:             }
00196:             return save;
00197:         }
00198:
00199:         public void RestoreState(GuiltInsomniaSaveState save)
00200:         {
00201:             _bySurvivor.Clear();
00202:             if (save?.survivors == null) return;
00203:             foreach (var s in save.survivors)
00204:             {
00205:                 if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
00206:                 var copy = new GuiltSurvivorState
00207:                 {
00208:                     survivorId = s.survivorId,
00209:                     insomniaSeverity = s.insomniaSeverity,
00210:                     sedativeCompensationHours = s.sedativeCompensationHours
00211:                 };
00212:                 if (s.guiltSources != null)
00213:                     foreach (var g in s.guiltSources)
00214:                         copy.guiltSources.Add(new GuiltRecord
00215:                         {
00216:                             sourceId = g.sourceId,
00217:                             dayRecorded = g.dayRecorded,
00218:                             severity = g.severity
00219:                         });
00220:                 _bySurvivor[s.survivorId] = copy;
00221:             }
00222:         }
00223:     }
00224: }
```

## `src/Host/Phase0HostSession.cs` — 1,095 lines; 56,129 bytes; SHA-256 `56eab2884bd11ab7fedfa64542e059d3126ad3d3b977bdbca6ceb41f8559a199`
Declaration index:
- 00022: public class Phase0SurvivorEffects
- 00060: public class Phase0EffectsSaveState
- 00092: public sealed class Phase0EffectConsumers
- 00183: public static Phase0EffectConsumers NoOp(
- 00229: public sealed class Phase0HostSession
- 00276: public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);
- 00526: public void ValidateConsumers()
- 00547: public void LoadTradeSpecialties(string dataDir)
- 00557: public void LoadPhantomRules(string dataDir)
- 00614: public void LoadFinalWishCatalog(string dataDir)
- 00631: public void RegisterDefaultRules()
- 00648: public void RegisterSurvivors(IEnumerable<string> ids)
- 00667: public void SeedDemoRoster()
- 00674: public string ScavengeItem(string survivorId, string itemId)
- 00691: public string RaiseNoise(string survivorId)
- 00699: public string CraftItem(string survivorId, string professionId, string itemId)
- 00709: public string RecordMoralChoice(string survivorId, bool isEmpathyChoice)
- 00719: public string RecordGuilt(string survivorId, string sourceId, float severity)
- 00728: public string RegisterCombatSurvived(string survivorId)
- 00737: public string ConsumeSubstance(string survivorId, string itemId, ChemicalDependencyKind kind)
- 00746: public string DeclareTerminalPrognosis(string survivorId, string archetypeId)
- 00754: public string AdvanceFinalWish(string survivorId, string stepId)
- 00764: public string ApplyInhaler(string survivorId)
- 00780: public string TickHour(float gameHours = 1f)
- 00803: public string TickDay(int day)
- 00815: public string StatusLine()
- 00840: public Phase0EffectsSaveState CaptureSave()
- 00877: public void RestoreSave(Phase0EffectsSaveState save)
- 00942: private Phase0SurvivorEffects fx(string survivorId) => GetOrCreateEffects(survivorId);
- 00949: private void RecomputeSurvivorEffects(string survivorId)
- 00980: private void PopulateFinalWishEffects(Phase0SurvivorEffects fx, string survivorId)
- 01005: private void RecomputeAllEffects()
- 01011: private Phase0SurvivorEffects GetOrCreateEffects(string survivorId)
- 01023: private PhaseProgressionState GetOrCreatePhaseState(string survivorId)
- 01034: private MoralBranchState GetOrCreateMoralState(string survivorId)
- 01045: private static string InferBackground(string survivorId)
- 01055: private sealed class CoreSeededRng : ISeededRng
- 01060: public int Next(int min, int max) => _rng.Next(min, max);
- 01061: public float NextFloat() => _rng.NextFloat();
- 01062: public double NextDouble() => _rng.NextDouble();
- 01065: public void BindShelterAssignment(ShelterAssignmentSystem shelterAssignment)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Survivors;
00008: using Ashfall.Core.Medical;
00009: using Ashfall.Core.Radiation;
00010: using Ashfall.Core.Phantoms;
00011: using Ashfall.Core.Shelter;
00012:
00013: namespace AtomicWar.GodotApp
00014: {
00015:     /// <summary>
00016:     /// Host-side aggregate view of one survivor's Phase-0 effect state.
00017:     /// The values here are DERIVED from the Core systems for presentation and
00018:     /// save; the Core systems own the rules. Real gameplay consumers (NeedsSystem
00019:     /// morale/health/fatigue, CraftingSystem craft time, ExpeditionSystem stamina,
00020:     /// Journal narrative) are reached through <see cref="Phase0EffectConsumers"/>.
00021:     /// </summary>
00022:     public class Phase0SurvivorEffects
00023:     {
00024:         public string survivorId = string.Empty;
00025:         /// <summary>Combined work-efficiency factor from phantom motivation and flashback penalty.</summary>
00026:         public float workEfficiencyMultiplier = 1f;
00027:         /// <summary>Hours the survivor refuses to work (phantom breakdown).</summary>
00028:         public float workRefusalHours = 0f;
00029:         /// <summary>Stamina multiplier (respiratory degeneration).</summary>
00030:         public float staminaMultiplier = 1f;
00031:         /// <summary>Guilt insomnia severity 0..1.</summary>
00032:         public float guiltInsomniaSeverity = 0f;
00033:         /// <summary>Combat trauma hypervigilance 0..1 (defense bonus).</summary>
00034:         public float hypervigilance = 0f;
00035:         /// <summary>Moral branch direction (Neutral until decided).</summary>
00036:         public string moralBranch = "Neutral";
00037:         /// <summary>Radiation sickness phase.</summary>
00038:         public string radiationPhase = "Healthy";
00039:         /// <summary>Dependency crafting penalty factor (0 = none).</summary>
00040:         public float dependencyCraftingPenalty = 0f;
00041:         /// <summary>Dependency combat penalty factor (0 = none).</summary>
00042:         public float dependencyCombatPenalty = 0f;
00043:         /// <summary>Final-wish state (empty / active / completed / failed).</summary>
00044:         public string finalWishState = string.Empty;
00045:         /// <summary>Authored final-wish title (empty when no catalog entry is bound).</summary>
00046:         public string finalWishTitle = string.Empty;
00047:         /// <summary>Authored final-wish description shown while active.</summary>
00048:         public string finalWishDescription = string.Empty;
00049:         /// <summary>Days remaining in the terminal-prognosis window (active only).</summary>
00050:         public float finalWishDaysRemaining;
00051:         /// <summary>Steps completed so far in the wish questline.</summary>
00052:         public int finalWishStepsDone;
00053:         /// <summary>Total steps required to complete the wish (0 when unknown).</summary>
00054:         public int finalWishStepsTotal;
00055:         /// <summary>Authored completion text shown when the wish is completed.</summary>
00056:         public string finalWishCompletionText = string.Empty;
00057:     }
00058:
00059:     /// <summary>Serialized Phase-0 effects envelope (all 10 systems).</summary>
00060:     public class Phase0EffectsSaveState
00061:     {
00062:         public PhaseProgressionSaveState radiationPhase = new PhaseProgressionSaveState();
00063:         public PhantomMemoryEngineState phantom = new PhantomMemoryEngineState();
00064:         public GuiltInsomniaSaveState guilt = new GuiltInsomniaSaveState();
00065:         public CombatTraumaSaveState combatTrauma = new CombatTraumaSaveState();
00066:         public SomaticFlashbackSaveState flashbacks = new SomaticFlashbackSaveState();
00067:         public MoralBranchingSaveState moral = new MoralBranchingSaveState();
00068:         public TradeSpecialtySaveState tradeSpecialty = new TradeSpecialtySaveState();
00069:         public FinalWishSaveState finalWishes = new FinalWishSaveState();
00070:         public RespiratoryDegenerationState respiratory = new RespiratoryDegenerationState();
00071:         public List<Phase0SurvivorEffects> effects = new List<Phase0SurvivorEffects>();
00072:         public float permanentShelterMoraleBuff = 0f;
00073:     }
00074:
00075:     /// <summary>
00076:     /// Immutable real-consumer wiring bundle. The host (Main.cs) constructs one
00077:     /// fully-bound instance and assigns it to <see cref="Phase0HostSession.Consumers"/>
00078:     /// so Phase-0 effects reach the authoritative gameplay consumers instead of
00079:     /// living in a display value. There are no mutable fields to silently leave
00080:     /// unwired.
00081:     ///
00082:     /// Effect classification:
00083:     ///  - Essential (health/morale/fatigue/shelter-morale): default to named
00084:     ///    no-op adapters when null, tracked as unbound.
00085:     ///  - Production-required (progression/medical/narrative): stay null when
00086:     ///    unbound so the session's null-check skips preserve headless behavior;
00087:     ///    tracked as unbound for startup validation.
00088:     ///  - Truly optional (ApplyWorkRefusalHours): null when unbound, not tracked.
00089:     ///
00090:     /// Use <see cref="NoOp"/> (with optional overrides) for isolated tests.
00091:     /// </summary>
00092:     public sealed class Phase0EffectConsumers
00093:     {
00094:         // ── Essential effects (health / morale / fatigue) — no-op when unbound ──
00095:
00096:         /// <summary>survivorId, morale delta → NeedsSystem.</summary>
00097:         public Action<string, float> ApplyMoraleDelta { get; }
00098:         /// <summary>survivorId, health delta → NeedsSystem.</summary>
00099:         public Action<string, float> ApplyHealthDelta { get; }
00100:         /// <summary>survivorId, fatigue delta → NeedsSystem.</summary>
00101:         public Action<string, float> ApplyFatigueDelta { get; }
00102:         /// <summary>Shelter-wide morale delta (final wish / moral branching).</summary>
00103:         public Action<float> ApplyShelterMoraleDelta { get; }
00104:
00105:         // ── Production-required (progression / medical / narrative) — null when unbound ──
00106:
00107:         /// <summary>survivorId, work-efficiency multiplier → work/task consumer.</summary>
00108:         public Action<string, float> ApplyWorkEfficiencyMultiplier { get; }
00109:         /// <summary>survivorId, crafting penalty factor → CraftingSystem time multiplier.</summary>
00110:         public Action<string, float> ApplyCraftingPenaltyFactor { get; }
00111:         /// <summary>survivorId, combat penalty factor → expedition/combat consumer.</summary>
00112:         public Action<string, float> ApplyCombatPenaltyFactor { get; }
00113:         /// <summary>survivorId, stamina drain multiplier → ExpeditionSystem stamina drain.</summary>
00114:         public Action<string, float> ApplyStaminaDrainMultiplier { get; }
00115:         /// <summary>narrativeId, survivorId → Journal / event runner.</summary>
00116:         public Action<string, string> FireNarrativeEvent { get; }
00117:         /// <summary>survivorId, afflictionId → medical / chronic-illness authority.</summary>
00118:         public Action<string, string> GrantChronicIllness { get; }
00119:         /// <summary>survivorId → RadiationSystem dose reset (Prodromal metabolized the acute dose).</summary>
00120:         public Action<string> ResetRadiationDose { get; }
00121:
00122:         // ── Truly optional — null when unbound, not validated ──
00123:
00124:         /// <summary>survivorId, work-refusal hours → work/task consumer.</summary>
00125:         public Action<string, float> ApplyWorkRefusalHours { get; }
00126:
00127:         private readonly List<string> _unboundRequired;
00128:
00129:         /// <summary>
00130:         /// Names of production-required effects that were not explicitly bound
00131:         /// (null passed → no-op/null). Empty when fully wired. Checked at startup
00132:         /// so a production system cannot silently run with a missing health,
00133:         /// morale, inventory, or progression effect.
00134:         /// </summary>
00135:         public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;
00136:
00137:         public Phase0EffectConsumers(
00138:             Action<string, float>? applyMoraleDelta,
00139:             Action<string, float>? applyHealthDelta,
00140:             Action<string, float>? applyFatigueDelta,
00141:             Action<float>? applyShelterMoraleDelta,
00142:             Action<string, float>? applyWorkEfficiencyMultiplier = null,
00143:             Action<string, float>? applyCraftingPenaltyFactor = null,
00144:             Action<string, float>? applyCombatPenaltyFactor = null,
00145:             Action<string, float>? applyStaminaDrainMultiplier = null,
00146:             Action<string, string>? fireNarrativeEvent = null,
00147:             Action<string, string>? grantChronicIllness = null,
00148:             Action<string>? resetRadiationDose = null,
00149:             Action<string, float>? applyWorkRefusalHours = null)
00150:         {
00151:             ApplyMoraleDelta = applyMoraleDelta ?? NoOpMoraleDelta;
00152:             ApplyHealthDelta = applyHealthDelta ?? NoOpHealthDelta;
00153:             ApplyFatigueDelta = applyFatigueDelta ?? NoOpFatigueDelta;
00154:             ApplyShelterMoraleDelta = applyShelterMoraleDelta ?? NoOpShelterMoraleDelta;
00155:             ApplyWorkEfficiencyMultiplier = applyWorkEfficiencyMultiplier;
00156:             ApplyCraftingPenaltyFactor = applyCraftingPenaltyFactor;
00157:             ApplyCombatPenaltyFactor = applyCombatPenaltyFactor;
00158:             ApplyStaminaDrainMultiplier = applyStaminaDrainMultiplier;
00159:             FireNarrativeEvent = fireNarrativeEvent;
00160:             GrantChronicIllness = grantChronicIllness;
00161:             ResetRadiationDose = resetRadiationDose;
00162:             ApplyWorkRefusalHours = applyWorkRefusalHours;
00163:
00164:             _unboundRequired = new List<string>();
00165:             if (applyMoraleDelta == null) _unboundRequired.Add(nameof(ApplyMoraleDelta));
00166:             if (applyHealthDelta == null) _unboundRequired.Add(nameof(ApplyHealthDelta));
00167:             if (applyFatigueDelta == null) _unboundRequired.Add(nameof(ApplyFatigueDelta));
00168:             if (applyShelterMoraleDelta == null) _unboundRequired.Add(nameof(ApplyShelterMoraleDelta));
00169:             if (applyWorkEfficiencyMultiplier == null) _unboundRequired.Add(nameof(ApplyWorkEfficiencyMultiplier));
00170:             if (applyCraftingPenaltyFactor == null) _unboundRequired.Add(nameof(ApplyCraftingPenaltyFactor));
00171:             if (applyCombatPenaltyFactor == null) _unboundRequired.Add(nameof(ApplyCombatPenaltyFactor));
00172:             if (applyStaminaDrainMultiplier == null) _unboundRequired.Add(nameof(ApplyStaminaDrainMultiplier));
00173:             if (fireNarrativeEvent == null) _unboundRequired.Add(nameof(FireNarrativeEvent));
00174:             if (grantChronicIllness == null) _unboundRequired.Add(nameof(GrantChronicIllness));
00175:             if (resetRadiationDose == null) _unboundRequired.Add(nameof(ResetRadiationDose));
00176:         }
00177:
00178:         /// <summary>
00179:         /// All-effects-unbound instance for isolated tests, with optional
00180:         /// overrides. Essential effects use no-op adapters; production-required
00181:         /// effects are null (session null-checks skip them).
00182:         /// </summary>
00183:         public static Phase0EffectConsumers NoOp(
00184:             Action<string, float>? applyMoraleDelta = null,
00185:             Action<string, float>? applyHealthDelta = null,
00186:             Action<string, float>? applyFatigueDelta = null,
00187:             Action<float>? applyShelterMoraleDelta = null,
00188:             Action<string, float>? applyWorkEfficiencyMultiplier = null,
00189:             Action<string, float>? applyCraftingPenaltyFactor = null,
00190:             Action<string, float>? applyCombatPenaltyFactor = null,
00191:             Action<string, float>? applyStaminaDrainMultiplier = null,
00192:             Action<string, string>? fireNarrativeEvent = null,
00193:             Action<string, string>? grantChronicIllness = null,
00194:             Action<string>? resetRadiationDose = null,
00195:             Action<string, float>? applyWorkRefusalHours = null)
00196:             => new Phase0EffectConsumers(
00197:                 applyMoraleDelta, applyHealthDelta, applyFatigueDelta, applyShelterMoraleDelta,
00198:                 applyWorkEfficiencyMultiplier, applyCraftingPenaltyFactor, applyCombatPenaltyFactor,
00199:                 applyStaminaDrainMultiplier, fireNarrativeEvent, grantChronicIllness,
00200:                 resetRadiationDose, applyWorkRefusalHours);
00201:
00202:         // ── Named no-op adapters for essential effects ──
00203:
00204:         public static readonly Action<string, float> NoOpMoraleDelta = (_, __) => { };
00205:         public static readonly Action<string, float> NoOpHealthDelta = (_, __) => { };
00206:         public static readonly Action<string, float> NoOpFatigueDelta = (_, __) => { };
00207:         public static readonly Action<float> NoOpShelterMoraleDelta = _ => { };
00208:     }
00209:
00210:     /// <summary>
00211:     /// Thin Godot-host session for ALL Phase-0 psychological/medical effects.
00212:     /// Owns the ten engine-agnostic Core systems and wires every effect event to
00213:     /// the injected <see cref="Consumers"/> so effects reach real gameplay consumers.
00214:     /// Host-derived per-survivor views are a pure function of Core state. All rules
00215:     /// live in Ashfall.Core; this session only wires and presents.
00216:     ///
00217:     /// Owned systems:
00218:     ///  1. Radiation Phase Progression
00219:     ///  2. Phantom Memory
00220:     ///  3. Guilt Insomnia
00228:     /// </summary>
00229:     public sealed class Phase0HostSession
00230:     : HostSessionBase{
00231:         public const int DefaultSeed = 808;
00232:
00233:         public RadiationPhaseProgression RadiationPhase { get; }
00234:         public PhantomMemoryEngine Phantom { get; }
00235:         public GuiltInsomniaSystem Guilt { get; }
00236:         public CombatTraumaSystem CombatTrauma { get; }
00237:         public SomaticFlashbackSystem Flashbacks { get; }
00238:         public MoralBranchingSystem Moral { get; }
00239:         /// <summary>
00240:         /// Chemical Dependency authority. Shares the MedicalHostSession's instance
00245:         /// </summary>
00246:         public ChemicalDependencySystem Dependency { get; }
00247:
00248:         public TradeSpecialtySystem TradeSpecialty { get; }
00249:         public FinalWishSystem FinalWish { get; }
00250:         public RespiratoryDegenerationSystem Respiratory { get; }
00251:
00252:         /// <summary>Real-consumer wiring bundle. Set by the host (Main.cs) with a fully-bound instance.</summary>
00253:         public Phase0EffectConsumers Consumers { get; set; } = Phase0EffectConsumers.NoOp();
00254:
00255:         /// <summary>Accumulated permanent shelter-wide morale buff from completed final wishes.</summary>
00256:         public float PermanentShelterMoraleBuff { get; private set; }
00257:
00258:         /// <summary>Set by the host from the current expedition/zone (real ash-zone signal).</summary>
00259:         public bool IsInAshZone { get; set; }
00260:
00261:         /// <summary>Set by the host from the world state (real fallout-storm signal).</summary>
00262:         public bool IsInFalloutStorm { get; set; }
00263:
00264:         /// <summary>Set by the host from the photoperiod (real night signal for trauma false alarms).</summary>
00265:         public bool IsNightTime { get; set; }
00266:
00267:         /// <summary>Current sim day, injected by the host (guilt expiry, wishes, phases).</summary>
00268:         public int CurrentDay { get; set; } = 1;
00269:
00270:         /// <summary>Air-filtration health 0..100, injected by the shelter host.</summary>
00271:         public Func<float> GetFilterHealth;
00272:
00273:         public IReadOnlyList<Phase0SurvivorEffects> Effects => _effects;
00274:
00275:         /// <summary>Public accessor for the derived host view of one survivor.</summary>
00276:         public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);
00277:
00278:         public string LastEvent { get; private set; } = string.Empty;
00279:
00280:         // ── Named relay handlers so UnsubscribeSystemEvents can clean up ──
00281:         private Action _onRadiationPhaseStateChanged = null!;
00282:         private Action<PhantomMemoryEngineState> _onPhantomStateChanged = null!;
00283:         private Action _onGuiltStateChanged = null!;
00284:         private Action _onCombatTraumaStateChanged = null!;
00285:         private Action _onFlashbacksStateChanged = null!;
00286:         private Action _onMoralStateChanged = null!;
00287:         private Action _onDependencyStateChanged = null!;
00288:         private Action _onTradeSpecialtyStateChanged = null!;
00289:         private Action _onFinalWishStateChanged = null!;
00290:         private Action _onRespiratoryStateChanged = null!;
00291:         /// <summary>Authored final-wish catalog (final_wishes.json); null until <see cref="LoadFinalWishCatalog"/> runs.</summary>
00292:         private IFinalWishCatalog? _finalWishCatalog;
00293:         private readonly List<Phase0SurvivorEffects> _effects = new List<Phase0SurvivorEffects>();
00294:         private readonly List<string> _aliveSurvivorIds = new List<string>();
00295:         private readonly Dictionary<string, MoralBranchState> _moralStates = new Dictionary<string, MoralBranchState>();
00296:         private readonly Dictionary<string, PhaseProgressionState> _phaseStates = new Dictionary<string, PhaseProgressionState>();
00297:         private readonly ISeededRng _rng;
00298:         /// <summary>True when this session constructed its own ChemicalDependencySystem (headless selftests). Production shares the MedicalHostSession instance and never ticks it (Task #133).</summary>
00299:         private readonly bool _ownsDependency;
00300:
00301:         public Phase0HostSession(int seed = DefaultSeed, ChemicalDependencySystem dependency = null!)
00302:         {
00303:             _rng = new CoreSeededRng(seed);
00525:         /// </summary>
00526:         public void ValidateConsumers()
00527:         {
00528:             var unbound = Consumers.UnboundRequiredEffects;
00539:         /// <summary>
00540:         /// Load trade_specialties.json profession patterns into the Phase-0
00541:         /// specialty system. Without this the wired specialty loop (events,
00542:         /// save, host hooks) runs with zero patterns and mastery can never
00546:         /// </summary>
00547:         public void LoadTradeSpecialties(string dataDir)
00548:         {
00549:             if (string.IsNullOrEmpty(dataDir)) return;
00555:
00556:         /// <summary>Load the phantom_triggers.json catalog into the engine (the authority).</summary>
00557:         public void LoadPhantomRules(string dataDir)
00558:         {
00559:             if (string.IsNullOrEmpty(dataDir)) return;
00607:         /// <summary>
00608:         /// Load <c>final_wishes.json</c> and bind it to <see cref="FinalWish"/> so a
00609:         /// terminal prognosis draws an authored wish from the archetype's pool and the
00610:         /// panel can surface title/description/completion text. Mirrors
00613:         /// </summary>
00614:         public void LoadFinalWishCatalog(string dataDir)
00615:         {
00616:             if (string.IsNullOrEmpty(dataDir)) return;
00630:         /// <summary>Built-in fallback phantom rules (host demo convenience).</summary>
00631:         public void RegisterDefaultRules()
00632:         {
00633:             Phantom.RegisterRule("former_soldier", "military", 0.40f, "desc",
00647:         /// <summary>Register the alive survivor ids (the host's roster authority).</summary>
00648:         public void RegisterSurvivors(IEnumerable<string> ids)
00649:         {
00650:             _aliveSurvivorIds.Clear();
00666:         /// <summary>Seed a small demo roster (host demo convenience).</summary>
00667:         public void SeedDemoRoster()
00668:         {
00669:             RegisterSurvivors(new[] { "survivor_dr_sarah_chen", "survivor_gunner_mikhail", "elena_vasquez" });
00673:
00674:         public string ScavengeItem(string survivorId, string itemId)
00675:         {
00676:             var sv = new PhantomSurvivorSnapshot
00690:
00691:         public string RaiseNoise(string survivorId)
00692:         {
00693:             Flashbacks.OnAudioEvent("siren", 1f);
00698:
00699:         public string CraftItem(string survivorId, string professionId, string itemId)
00700:         {
00701:             TradeSpecialty.OnItemCrafted(survivorId, professionId, itemId);
00708:         /// <summary>Record a moral choice for a survivor (real event/narrative flow).</summary>
00709:         public string RecordMoralChoice(string survivorId, bool isEmpathyChoice)
00710:         {
00711:             var state = GetOrCreateMoralState(survivorId);
00718:         /// <summary>Record guilt from a ruthless choice (real guilt source).</summary>
00719:         public string RecordGuilt(string survivorId, string sourceId, float severity)
00720:         {
00721:             Guilt.RecordGuilt(survivorId, sourceId, severity, CurrentDay);
00727:         /// <summary>Register a combat survival (real raid/skirmish outcome).</summary>
00728:         public string RegisterCombatSurvived(string survivorId)
00729:         {
00730:             CombatTrauma.OnCombatSurvived(survivorId);
00736:         /// <summary>Consume a substance (real inventory consumption → dependency).</summary>
00737:         public string ConsumeSubstance(string survivorId, string itemId, ChemicalDependencyKind kind)
00738:         {
00739:             Dependency.OnSubstanceConsumed(survivorId, itemId, kind);
00745:         /// <summary>Declare a terminal prognosis and open the final wish questline.</summary>
00746:         public string DeclareTerminalPrognosis(string survivorId, string archetypeId)
00747:         {
00748:             FinalWish.DeclareTerminalPrognosis(survivorId, archetypeId, true);
00753:
00754:         public string AdvanceFinalWish(string survivorId, string stepId)
00755:         {
00756:             bool completed = FinalWish.AdvanceWishStep(survivorId, stepId);
00763:
00764:         public string ApplyInhaler(string survivorId)
00765:         {
00766:             bool ok = Respiratory.ApplyInhaler(survivorId);
00773:
00774:         // ── Tick ──────────────────────────────────────────────────────
00775:
00776:         /// <summary>
00777:         /// Tick all Phase-0 systems for elapsed game hours. Called by the host on
00778:         /// the authoritative clock (hourly progression and day advance).
00779:         /// </summary>
00780:         public string TickHour(float gameHours = 1f)
00781:         {
00782:             if (gameHours <= 0f) return "No time elapsed.";
00786:                 Phantom.TickHour(id, gameHours);
00787:                 Guilt.Tick(id, gameHours, CurrentDay);
00788:                 CombatTrauma.Tick(id, gameHours, IsNightTime);
00789:                 Flashbacks.Tick(id, gameHours);
00790:                 if (_ownsDependency)
00791:                     Dependency.TickHours(id, gameHours); // shared ledgers are ticked by MedicalDiseaseDayOwner only (Task #133)
00792:                 Respiratory.TickHours(id, gameHours);
00793:                 FinalWish.Tick(id, gameHours, true);
00794:             }
00795:             RadiationPhase.Tick(gameHours);
00796:             RecomputeAllEffects();
00797:             LastEvent = $"Phase-0 effects ticked {gameHours:F0}h.";
00802:         /// <summary>Daily boundary: reset per-night trauma flags and advance the sim day.</summary>
00803:         public string TickDay(int day)
00804:         {
00805:             CurrentDay = day;
00814:
00815:         public string StatusLine()
00816:         {
00817:             var sb = new System.Text.StringBuilder();
00837:
00838:         // ── Save / Load ────────────────────────────────────────────────
00839:
00840:         public Phase0EffectsSaveState CaptureSave()
00841:         {
00842:             var save = new Phase0EffectsSaveState
00876:
00877:         public void RestoreSave(Phase0EffectsSaveState save)
00878:         {
00879:             if (save == null) return;
00941:
00942:         private Phase0SurvivorEffects fx(string survivorId) => GetOrCreateEffects(survivorId);
00943:
00944:         /// <summary>
00948:         /// </summary>
00949:         private void RecomputeSurvivorEffects(string survivorId)
00950:         {
00951:             var fx = GetOrCreateEffects(survivorId);
00976:         /// Resolve authored final-wish narrative fields onto the effects view from the
00977:         /// loaded catalog + the survivor's bound wishId. Clears them when no wish is
00978:         /// active/completed/failed or no catalog entry is available (graceful degrade).
00979:         /// </summary>
00980:         private void PopulateFinalWishEffects(Phase0SurvivorEffects fx, string survivorId)
00981:         {
00982:             fx.finalWishTitle = string.Empty;
00983:             fx.finalWishDescription = string.Empty;
00984:             fx.finalWishDaysRemaining = 0f;
00985:             fx.finalWishStepsDone = 0;
00986:             fx.finalWishStepsTotal = 0;
00987:             fx.finalWishCompletionText = string.Empty;
00988:
00989:             if (string.IsNullOrEmpty(fx.finalWishState)) return;
00990:
00991:             string wishId = FinalWish.GetWishId(survivorId);
00992:             if (string.IsNullOrEmpty(wishId)) return;
00993:
00994:             var entry = _finalWishCatalog?.GetEntry(wishId);
00995:             if (entry == null) return;
00996:
00997:             fx.finalWishTitle = entry.wish_title ?? string.Empty;
00998:             fx.finalWishDescription = entry.wish_description ?? string.Empty;
00999:             fx.finalWishCompletionText = entry.completion_text ?? string.Empty;
01000:             fx.finalWishStepsTotal = entry.steps?.Count ?? 0;
01001:             fx.finalWishStepsDone = FinalWish.GetStepsCompleted(survivorId);
01002:             fx.finalWishDaysRemaining = FinalWish.GetDaysRemaining(survivorId);
01003:         }
01004:
01005:         private void RecomputeAllEffects()
01006:         {
01007:             for (int i = 0; i < _aliveSurvivorIds.Count; i++)
01008:                 RecomputeSurvivorEffects(_aliveSurvivorIds[i]);
01009:         }
01010:
01011:         private Phase0SurvivorEffects GetOrCreateEffects(string survivorId)
01012:         {
01013:             for (int i = 0; i < _effects.Count; i++)
01014:             {
01015:                 var e = _effects[i];
01016:                 if (e != null && e.survivorId == survivorId) return e;
01017:             }
01018:             var fx = new Phase0SurvivorEffects { survivorId = survivorId };
01019:             _effects.Add(fx);
01020:             return fx;
01021:         }
01022:
01023:         private PhaseProgressionState GetOrCreatePhaseState(string survivorId)
01024:         {
01025:             if (!_phaseStates.TryGetValue(survivorId, out var state))
01026:             {
01027:                 state = new PhaseProgressionState { Id = survivorId, IsAlive = true };
01028:                 _phaseStates[survivorId] = state;
01029:                 RadiationPhase.Register(state);
01030:             }
01031:             return state;
01032:         }
01033:
01034:         private MoralBranchState GetOrCreateMoralState(string survivorId)
01035:         {
01036:             if (!_moralStates.TryGetValue(survivorId, out var state))
01037:             {
01038:                 state = new MoralBranchState { SurvivorId = survivorId, IsAlive = true };
01039:                 _moralStates[survivorId] = state;
01040:                 Moral.Register(state);
01041:             }
01042:             return state;
01043:         }
01044:
01045:         private static string InferBackground(string survivorId)
01046:         {
01047:             if (string.IsNullOrEmpty(survivorId)) return "generic";
01048:             if (survivorId.Contains("gunner") || survivorId.Contains("soldier")) return "former_soldier";
01049:             if (survivorId.Contains("sarah") || survivorId.Contains("nurse")) return "nurse";
01050:             if (survivorId.Contains("teacher")) return "teacher";
01051:             return "generic";
01052:         }
01053:
01054:         /// <summary>Deterministic ISeededRng adapter delegating to the core SeededRng.</summary>
01055:         private sealed class CoreSeededRng : ISeededRng
01056:         {
01057:             private readonly SeededRng _rng;
01058:             public int Seed { get; }
01059:             public CoreSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
01060:             public int Next(int min, int max) => _rng.Next(min, max);
01061:             public float NextFloat() => _rng.NextFloat();
01062:             public double NextDouble() => _rng.NextDouble();
01063:         }
01064:
01065:         public void BindShelterAssignment(ShelterAssignmentSystem shelterAssignment)
01066:         {
01067:             if (shelterAssignment == null)
01068:             {
01069:                 Flashbacks.IsCompanionInSameRoom = (a, b) => false;
01070:                 return;
01071:             }
01072:
01073:             Flashbacks.IsCompanionInSameRoom = shelterAssignment.AreInSameRoom;
01074:             shelterAssignment.OnAssignmentChanged += ev =>
01075:             {
01076:                 LastEvent = $"[Phase0] Shelter assignment changed: {ev?.SurvivorId ?? "unknown"} -> {ev?.RoomId ?? "unknown"}";
01077:                 RaiseStateChanged();
01078:             };
01079:         }
01080:
01081:         protected override void UnsubscribeSystemEvents()
01082:         {
01083:             RadiationPhase.OnStateChanged -= _onRadiationPhaseStateChanged;
01084:             Phantom.OnStateChanged -= _onPhantomStateChanged;
01085:             Guilt.OnStateChanged -= _onGuiltStateChanged;
01086:             CombatTrauma.OnStateChanged -= _onCombatTraumaStateChanged;
01087:             Flashbacks.OnStateChanged -= _onFlashbacksStateChanged;
01088:             Moral.OnStateChanged -= _onMoralStateChanged;
01089:             Dependency.OnStateChanged -= _onDependencyStateChanged;
01090:             TradeSpecialty.OnStateChanged -= _onTradeSpecialtyStateChanged;
01091:             FinalWish.OnStateChanged -= _onFinalWishStateChanged;
01092:             Respiratory.OnStateChanged -= _onRespiratoryStateChanged;
01093:         }
01094:     }
01095: }
```

## `src/Main.Phase0.cs` — 487 lines; 20,350 bytes; SHA-256 `17e1d2026b7034eec7ffe613a720851aed49bcb82b064c25d44ebe1b80c7f8dd`
Declaration index:
- 00032: public partial class Main : Control
- 00042: private void SetupPhantom()
- 00080: private void OnPhantomScavengeClicked()
- 00086: private void OnPhantomTickClicked()
- 00092: private void SavePhantomMemory()
- 00099: private void SetupPhase0()
- 00258: private void SavePhase0()
- 00268: private void FlushPhase0IfDirty()
- 00279: private void OnCraftCompletedForSpecialty(Recipe recipe, string crafterId)
- 00300: private string ResolveSurvivorProfessionId(string survivorId, string? definitionId = null)
- 00316: private string AutoAssignSpecialtyCrafter(string itemId)
- 00340: private void OnPhase0ScavengeClicked()
- 00346: private void OnPhase0NoiseClicked()
- 00352: private void OnPhase0CraftClicked()
- 00358: private void OnPhase0TickClicked()
- 00364: private void SetupDoseLedger()
- 00414: private void OnDoseRegisterClicked()
- 00420: private void OnDoseSealClicked()
- 00429: private void OnDoseScribeClicked()
- 00438: private void OnDoseDiagnoseClicked()
- 00447: private void OnDoseCohortClicked()
- 00456: private void OnDoseVolunteerClicked()
- 00465: private void SaveDoseLedger()
- 00476: private void FlushDoseLedgerIfDirty()
- 00481: private void ClosePhase0Panel()
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
00011: using Ashfall.Core.Crafting;
00012: using Ashfall.Core.Economy;
00013: using Ashfall.Core.Expeditions;
00014: using Ashfall.Core.Foundry;
00015: using Ashfall.Core.Inventory;
00016: using Ashfall.Core.Journal;
00017: using Ashfall.Core.Muster;
00018: using Ashfall.Core.YearOfAsh;
00019: using Ashfall.Core.Radio;
00020: using Ashfall.Core.Survivors;
00021: using AtomicWar.GodotApp.Economy;
00022: using AtomicWar.GodotApp.YearOfAsh;
00023: using AtomicWar.GodotApp.Muster;
00024: using AtomicWar.GodotApp.Dose;
00025: using AtomicWar.GodotApp.UtilityAI;
00026: using AtomicWar.GodotApp.Radio;
00027: using AtomicWar.GodotApp.Audio;
00028: using AtomicWar.GodotApp.UI;
00029:
00030: namespace AtomicWar.GodotApp
00031: {
00032:     public partial class Main : Control
00033:     {
00034:         // ── Phase 0 / Dose fields (GAP-ARCH-01 Phase 1) ──
00035:         private PhantomMemoryHostSession _phantomMemory = null!;
00036:         private Phase0HostSession _phase0 = null!;
00037:         private bool _phase0Dirty;
00038:         private DoseLedgerHostSession _doseLedger = null!;
00039:         private bool _doseLedgerDirty;
00040:         private DoseRegisterSurface _doseSurface = null!;
00041:
00042:         private void SetupPhantom()
00043:         {
00044:             if (_phantomMemory != null) return;
00045:             SetupCampaignDay();
00046:             var rng = _campaignDay?.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Psychology).Rng;
00047:             _phantomMemory = PhantomMemoryHostSession.Create(_dataDir, rng);
00048:             _phantomMemory.StateChanged += () => SavePhantomMemory();
00049:             SetupSurvivors();
00050:             SetupEnrichment();
00051:             if (_survivors != null)
00052:             {
00053:                 // Enrichment is the explicit background authority when present;
00054:                 // profession mapping remains the fallback for roster entries
00055:                 // that do not have an enrichment row.
00056:                 _phantomMemory.BindSurvivors(_survivors, _enrichment);
00057:             }
00058:             SetupInventory();
00059:             if (_inventory != null)
00060:             {
00061:                 _phantomMemory.BindInventory(_inventory);
00062:             }
00063:             _phantomMemory.Engine.OnPhantomMemoryResolved += (svId, itemId, isMotivation, moraleDelta, guiltDelta) =>
00064:             {
00065:                 var sv = _survivors?.Find(svId);
00066:                 if (sv != null && moraleDelta != 0f)
00067:                 {
00068:                     _survivors!.Needs.Modify(sv, NeedKind.Morale, moraleDelta);
00069:                 }
00070:             };
00071:
00072:             var save = PhantomMemorySaveStore.TryLoad();
00073:             if (save != null)
00074:             {
00075:                 _phantomMemory.RestoreSave(save);
00076:                 GD.Print("[Ashfall Godot] Phantom Memory state restored.");
00077:             }
00078:         }
00079:
00080:         private void OnPhantomScavengeClicked()
00081:         {
00082:             SetupPhantom();
00083:             _statusLabel.Text = _phantomMemory.ScavengeItem("survivor_gunner_mikhail", "dog_tags");
00084:         }
00085:
00086:         private void OnPhantomTickClicked()
00087:         {
00088:             SetupPhantom();
00089:             _statusLabel.Text = _phantomMemory.TickDemo();
00090:         }
00091:
00092:         private void SavePhantomMemory()
00093:         {
00094:             if (_phantomMemory == null) return;
00095:             if (CaptureSection("phantom_memory", PhantomMemorySaveStore.TryCapturePersisted(_phantomMemory.CaptureSave())))
00096:                 GD.Print("[Ashfall Godot] Phantom Memory save written.");
00097:         }
00098:
00099:         private void SetupPhase0()
00100:         {
00101:             if (_phase0 != null) return;
00102:             SetupMedical();
00103:             // Task #133: share the MedicalHostSession-owned dependency ledger so
00104:             // there is exactly one chem-dep authority, and Phase-0 does not tick it.
00105:             _phase0 = new Phase0HostSession(dependency: _medical.Engine);
00106:             _phase0.StateChanged += () => _phase0Dirty = true;
00107:             // Feed the specialty catalog — without it the wired specialty loop
00108:             // runs patternless and mastery can never progress.
00109:             _phase0.LoadTradeSpecialties(_dataDir);
00110:
00111:             // ── Wire every Phase-0 effect to the REAL gameplay consumer ──
00112:             SetupSurvivors();
00113:             SetupJournal();
00114:             SetupCrafting();
00115:             SetupExpeditions();
00116:             SetupMedical();
00117:             SetupEnrichment();
00118:
00119:             // Recorded craft-attribution contract: crafting owns recipe completion,
00120:             // this host supplies the survivor, profession, and result item. Both
00121:             // _phase0 and _crafting reset through the lifecycle registry, so this
00122:             // subscription is rebuilt against the fresh engine on a new campaign.
00123:             _crafting.Engine.OnCraftCompleted += OnCraftCompletedForSpecialty;
00124:
00125:             _phase0.Consumers = new Phase0EffectConsumers(
00126:                 applyMoraleDelta: (sv, delta) =>
00127:                 {
00128:                     var survivor = _survivors.Find(sv);
00129:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
00130:                 },
00131:                 applyHealthDelta: (sv, delta) =>
00132:                 {
00133:                     var survivor = _survivors.Find(sv);
00134:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Health, delta);
00135:                 },
00136:                 applyFatigueDelta: (sv, delta) =>
00137:                 {
00138:                     var survivor = _survivors.Find(sv);
00139:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Fatigue, delta);
00140:                 },
00141:                 applyShelterMoraleDelta: delta =>
00142:                 {
00143:                     for (int i = 0; i < _survivors.RosterState.Count; i++)
00144:                     {
00145:                         var s = _survivors.RosterState[i];
00146:                         if (s != null && s.IsAliveState)
00147:                             _survivors.Needs.Modify(s, NeedKind.Morale, delta);
00148:                     }
00149:                 },
00150:                 applyWorkEfficiencyMultiplier: (sv, mult) =>
00151:                 {
00152:                     if (_crafting == null) return;
00153:                     _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
00154:                         id == sv ? MathfCompat.Max(0.1f, 1f / MathfCompat.Max(0.1f, mult)) : 1f);
00155:                 },
00156:                 applyCraftingPenaltyFactor: (sv, factor) =>
00157:                 {
00158:                     if (_crafting == null) return;
00159:                     _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
00160:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00161:                 },
00162:                 applyCombatPenaltyFactor: (sv, factor) =>
00163:                 {
00164:                     if (_expeditions == null) return;
00165:                     _expeditions.Engine.SetStaminaDrainMultiplier(id =>
00166:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00167:                 },
00168:                 applyStaminaDrainMultiplier: (sv, factor) =>
00169:                 {
00170:                     if (_expeditions == null) return;
00171:                     _expeditions.Engine.SetStaminaDrainMultiplier(id =>
00172:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00173:                 },
00174:                 fireNarrativeEvent: (narrativeId, sv) =>
00175:                 {
00176:                     int day = _holdfastRuntime?.Day ?? _simDay;
00177:                     string sourceId = $"{narrativeId}_{sv}_{day}";
00178:
00179:                     // events.json is the prose authority for narrative event ids.
00180:                     // Authored ids dispatch through the same catalog seam the
00181:                     // wildlife bycatch beat uses (journal + codex unlock + HUD).
00182:                     SetupEventsHost();
00183:                     if (_eventsHost != null
00184:                         && _eventsHost.TryGetEvent(narrativeId, out var authored)
00185:                         && authored != null
00186:                         && !string.IsNullOrWhiteSpace(authored.BodyText))
00187:                     {
00188:                         SetupEventAdapter();
00189:                         _hostEventAdapter?.DispatchCatalogEvent(authored.Id, authored.BodyText, day, sourceId);
00190:                         return;
00191:                     }
00192:
00193:                     // Unauthored id (narrative_final_wish_completed has no events.json
00194:                     // row): keep the beat visible instead of dropping it silently.
00195:                     GD.PushWarning($"[Phase0] narrative event '{narrativeId}' is not authored in events.json; writing placeholder journal entry.");
00196:                     _journal.TryAddRawEntry(
00197:                         sourceId,
00198:                         $"{sv}: {narrativeId.Replace('_', ' ')}.",
00199:                         author: null!,
00200:                         day: day);
00201:                 },
00202:                 grantChronicIllness: (sv, afflictionId) =>
00203:                 {
00204:                     var rad = _survivors.RadStateFor(sv);
00205:                     if (rad != null && !rad.HasChronicIllness)
00206:                     {
00207:                         rad.HasChronicIllness = true;
00208:                         SaveSurvivors();
00209:                     }
00210:                 },
00211:                 resetRadiationDose: sv =>
00212:                 {
00213:                     var rad = _survivors.RadStateFor(sv);
00214:                     if (rad != null) _survivors.Radiation.SetDose(rad, 0f);
00215:                 },
00216:                 applyWorkRefusalHours: null);
00217:             _phase0.ValidateConsumers();
00218:
00219:             // Environment signals from the real world/shelter hosts.
00220:             _phase0.CurrentDay = _simDay;
00221:             _phase0.GetFilterHealth = () =>
00222:             {
00223:                 var filter = _expansions?.Waystation?.State != null
00224:                     ? _expansions.Waystation.State.filterHealth : 100f;
00225:                 return filter;
00226:             };
00227:             // Host flags: updated each tick from the real world/shelter state.
00228:             _phase0.IsInFalloutStorm = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
00229:             _phase0.IsNightTime = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.BlackRain;
00230:
00231:             var ids = new System.Collections.Generic.List<string>();
00232:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00233:             {
00234:                 var s = _survivors.RosterState[i];
00235:                 if (s != null && s.IsAliveState) ids.Add(s.Id);
00236:             }
00237:             _phase0.RegisterSurvivors(ids);
00238:
00239:             if (_shelterAssignment != null)
00240:             {
00241:                 _phase0.BindShelterAssignment(_shelterAssignment.System);
00242:             }
00243:
00244:             var save = Phase0SaveStore.TryLoad();
00245:             if (save != null)
00246:             {
00247:                 _phase0.RestoreSave(save);
00248:                 _phase0Dirty = false; // restore just raised state-change events
00249:                 GD.Print("[Ashfall Godot] Phase-0 effects restored.");
00250:             }
00251:
00252:             // Bind the authored final-wish catalog so terminal prognoses draw from a
00253:             // per-archetype pool and the panel can surface authored text. Safe to run
00254:             // after restore: it only affects future DeclareTerminalPrognosis calls.
00255:             _phase0.LoadFinalWishCatalog(_dataDir);
00256:         }
00257:
00258:         private void SavePhase0()
00259:         {
00260:             if (_phase0 == null) return;
00261:             if (CaptureSection("phase0", Phase0SaveStore.TryCapturePersisted(_phase0.CaptureSave())))
00262:             {
00263:                 _phase0Dirty = false;
00264:                 GD.Print("[Ashfall Godot] Phase-0 effects save written.");
00265:             }
00266:         }
00267:
00268:         private void FlushPhase0IfDirty()
00269:         {
00270:             if (_phase0Dirty) SavePhase0();
00271:         }
00272:
00273:         /// <summary>
00274:         /// Bridge a completed production craft into trade specialty progression.
00275:         /// A player-assigned crafter is authoritative; an unassigned shelter craft
00276:         /// falls back to a living survivor whose trade actually covers the item.
00277:         /// A survivor whose profession resolves to no specialty has no tree to advance.
00278:         /// </summary>
00279:         private void OnCraftCompletedForSpecialty(Recipe recipe, string crafterId)
00280:         {
00281:             if (_phase0 == null || recipe?.result == null) return;
00282:
00283:             string itemId = recipe.result.id;
00284:             string survivorId = string.IsNullOrWhiteSpace(crafterId)
00285:                 ? AutoAssignSpecialtyCrafter(itemId)
00286:                 : crafterId;
00287:             if (string.IsNullOrEmpty(survivorId)) return;
00288:
00289:             string professionId = ResolveSurvivorProfessionId(survivorId);
00290:             if (string.IsNullOrEmpty(professionId)) return;
00291:
00292:             _phase0.CraftItem(survivorId, professionId, itemId);
00293:         }
00294:
00295:         /// <summary>
00296:         /// Resolve a survivor's trade specialty id. An authored pre_war_profession_id
00297:         /// wins; otherwise the roster profession label is matched against the
00298:         /// authored profession_aliases in trade_specialties.json.
00299:         /// </summary>
00300:         private string ResolveSurvivorProfessionId(string survivorId, string? definitionId = null)
00301:         {
00302:             if (string.IsNullOrEmpty(survivorId)) return string.Empty;
00303:             SetupEnrichment();
00304:             string explicitId = _enrichment?.GetSurvivorFields(survivorId)?.pre_war_profession_id ?? string.Empty;
00305:             string label = _survivors?.Roster?.FindDefinition(definitionId ?? survivorId)?.profession ?? string.Empty;
00306:             return TradeSpecialtySystem.ResolveProfessionId(explicitId, label);
00307:         }
00308:
00309:         /// <summary>
00310:         /// Attribute an unassigned craft to a living survivor whose trade covers the
00311:         /// item. Deterministic: candidates are ordinal-sorted before the forked
00312:         /// campaign RNG stream picks one, so a replay credits the same survivor and
00313:         /// no wall-clock or hash-iteration order is involved. Empty when nobody's
00314:         /// trade matches, which leaves the craft advancing no specialty.
00315:         /// </summary>
00316:         private string AutoAssignSpecialtyCrafter(string itemId)
00317:         {
00318:             var roster = _survivors?.Roster;
00319:             if (roster == null || string.IsNullOrEmpty(itemId)) return string.Empty;
00320:
00321:             var candidates = new List<string>();
00322:             for (int i = 0; i < roster.Roster.Count; i++)
00323:             {
00324:                 var entry = roster.Roster[i];
00325:                 if (entry == null || !entry.isAlive || string.IsNullOrEmpty(entry.survivorId)) continue;
00326:                 string professionId = ResolveSurvivorProfessionId(entry.survivorId, entry.definitionId);
00327:                 if (string.IsNullOrEmpty(professionId)) continue;
00328:                 if (!TradeSpecialtySystem.ProfessionMatchesItem(professionId, itemId)) continue;
00329:                 candidates.Add(entry.survivorId);
00330:             }
00331:
00332:             if (candidates.Count == 0) return string.Empty;
00333:             if (candidates.Count == 1) return candidates[0];
00334:
00335:             candidates.Sort(StringComparer.Ordinal);
00336:             var rng = _campaignDay?.Rng?.Fork("trade_specialty_attribution");
00337:             return rng != null ? candidates[rng.Next(0, candidates.Count)] : candidates[0];
00338:         }
00339:
00340:         private void OnPhase0ScavengeClicked()
00341:         {
00342:             SetupPhase0();
00343:             _statusLabel.Text = _phase0.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
00344:         }
00345:
00346:         private void OnPhase0NoiseClicked()
00347:         {
00348:             SetupPhase0();
00349:             _statusLabel.Text = _phase0.RaiseNoise("siren");
00350:         }
00351:
00352:         private void OnPhase0CraftClicked()
00353:         {
00354:             SetupPhase0();
00355:             _statusLabel.Text = _phase0.CraftItem("elena_vasquez", "machinist", "wrench_standard");
00356:         }
00357:
00358:         private void OnPhase0TickClicked()
00359:         {
00360:             SetupPhase0();
00361:             _statusLabel.Text = _phase0.TickHour(6f);
00362:         }
00363:
00364:         private void SetupDoseLedger()
00365:         {
00366:             if (_doseLedger != null) return;
00367:             SetupCampaignDay();
00368:             _doseLedger = DoseLedgerHostSession.Create(_dataDir, campaignRng: _campaignDay.Rng);
00369:             _doseLedger.StateChanged += () => _doseLedgerDirty = true;
00370:
00371:             // CORE-MECH W2: bind the live campaign day and the authored Year-of-Ash
00372:             // fallout windows so radiation bookings are conditioned by the season.
00373:             // The provider is read-only and pure; the dose ledger keeps owning every
00374:             // reading rule (AA.2 receiver contract).
00375:             _doseLedger.DayProvider = () => _simDay;
00376:             try
00377:             {
00378:                 var catalogPath = CatalogPath.ResolveCatalog("year_of_ash_events.json");
00379:                 var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00380:                 if (catalogIo.FileExists(catalogPath))
00381:                 {
00382:                     var yoaEvents = YearOfAshCatalogLoader.LoadEvents(
00383:                         CatalogPath.ResolveDataDir(), catalogIo, new SystemTextJsonSerializer());
00384:                     _doseLedger.FalloutWindowProviderRef = new FalloutWindowProvider(yoaEvents);
00385:                 }
00386:             }
00387:             catch (Exception ex)
00388:             {
00389:                 // Fail-closed: no calendar ⇒ neutral multiplier (1.0), never a crash
00390:                 // and never a silently reduced exposure.
00391:                 GD.Print("[Ashfall Godot] Dose fallout windows unavailable: " + ex.Message);
00392:             }
00393:
00394:             var save = DoseLedgerSaveStore.TryLoad();
00395:             if (save != null)
00396:             {
00397:                 _doseLedger.RestoreSave(save);
00398:                 _doseLedgerDirty = false; // restore just raised state-change events
00399:                 GD.Print("[Ashfall Godot] Dose Ledger state restored.");
00400:             }
00401:
00402:             if (_doseSurface == null && _rightColumn != null)
00403:             {
00404:                 _doseSurface = new DoseRegisterSurface();
00405:                 _rightColumn.AddChild(_doseSurface);
00406:             }
00407:             if (_doseSurface != null)
00408:             {
00409:                 _doseSurface.BindSession(_doseLedger);
00410:                 _doseSurface.RefreshView();
00411:             }
00412:         }
00413:
00414:         private void OnDoseRegisterClicked()
00415:         {
00416:             SetupDoseLedger();
00417:             _statusLabel.Text = "The Dose Register is open. Four tabs, four people who keep books.";
00418:         }
00419:
00420:         private void OnDoseSealClicked()
00421:         {
00422:             SetupDoseLedger();
00423:             _doseLedger.SealDemoSurvivors();
00424:             _statusLabel.Text = "Dosimeters sealed: Gunner Mikhail (tag_1), Elena Vasquez (tag_2).";
00425:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00426:             FlushDoseLedgerIfDirty();
00427:         }
00428:
00429:         private void OnDoseScribeClicked()
00430:         {
00431:             SetupDoseLedger();
00432:             string result = _doseLedger.ScribeReading(180f, highEnergy: false);
00433:             _statusLabel.Text = result;
00434:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00435:             FlushDoseLedgerIfDirty();
00436:         }
00437:
00438:         private void OnDoseDiagnoseClicked()
00439:         {
00440:             SetupDoseLedger();
00441:             string result = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed);
00442:             _statusLabel.Text = result;
00443:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00444:             FlushDoseLedgerIfDirty();
00445:         }
00446:
00447:         private void OnDoseCohortClicked()
00448:         {
00449:             SetupDoseLedger();
00450:             string result = _doseLedger.BookDemoChild();
00451:             _statusLabel.Text = result;
00452:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00453:             FlushDoseLedgerIfDirty();
00454:         }
00455:
00456:         private void OnDoseVolunteerClicked()
00457:         {
00458:             SetupDoseLedger();
00459:             string result = _doseLedger.SignDemoVolunteer();
00460:             _statusLabel.Text = result;
00461:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00462:             FlushDoseLedgerIfDirty();
00463:         }
00464:
00465:         private void SaveDoseLedger()
00466:         {
00467:             if (_doseLedger == null) return;
00468:             int day = _core != null ? _core.Clock.Day : _simDay;
00469:             if (CaptureSection("dose_ledger", DoseLedgerSaveStore.TryCapturePersisted(_doseLedger.CaptureSave(day))))
00470:             {
00471:                 _doseLedgerDirty = false;
00472:                 GD.Print($"[Ashfall Godot] Dose Ledger save written (day {day}).");
00473:             }
00474:         }
00475:
00476:         private void FlushDoseLedgerIfDirty()
00477:         {
00478:             if (_doseLedgerDirty) SaveDoseLedger();
00479:         }
00480:
00481:         private void ClosePhase0Panel()
00482:         {
00483:             _phase0Panel.Visible = false;
00484:         }
00485:
00486:     }
00487: }
```

## `src/Main.Medical.cs` — 651 lines; 31,882 bytes; SHA-256 `56732cc5acbf9c9129f4159be2c23cb6164bf13e1a3b6b2521152b10fa90cf39`
Declaration index:
- 00032: public partial class Main : Control
- 00039: private string DiseaseStatusLine()
- 00052: private void FlushMedicalIfDirty()
- 00057: private void SetupMedical()
- 00098: public IReadOnlyList<string> GetActiveAfflictionIds(string survivorId)
- 00189: public bool IsQuestBlockedForSurvivor(string survivorId, string questTag, out List<string> blockingAfflictions)
- 00208: public IReadOnlyList<string> GetUnlockedQuestsForSurvivor(string survivorId)
- 00221: private void EnsureMedicalPipeline()
- 00320: private void MigrateDiseaseSuspicions(Ashfall.Core.Medical.MedicalPipelineCoordinator pipeline)
- 00343: private Ashfall.Core.Medical.PatientAvailability ResolvePatientAvailability(string survivorId)
- 00351: private string GetRadiationPhaseName(string survivorId)
- 00364: private void MigrateLegacyMedicalDiagnoses()
- 00382: private void SaveMedicalPipeline()
- 00391: private void SaveMedical()
- 00404: private void SetupMedicalWard()
- 00466: private void SaveMedicalWard()
- 00503: private void LoadMedicalWard()
- 00521: private void SaveDisease()
- 00534: private void SetupDisease()
- 00645: private void CloseMedicalPanel()
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
00020: using Ashfall.Core.Feedback;
00021: using AtomicWar.GodotApp.Economy;
00022: using AtomicWar.GodotApp.YearOfAsh;
00023: using AtomicWar.GodotApp.Muster;
00024: using AtomicWar.GodotApp.Dose;
00025: using AtomicWar.GodotApp.UtilityAI;
00026: using AtomicWar.GodotApp.Radio;
00027: using AtomicWar.GodotApp.Audio;
00028: using AtomicWar.GodotApp.UI;
00029:
00030: namespace AtomicWar.GodotApp
00031: {
00032:     public partial class Main : Control
00033:     {
00034:         // ── Medical fields (GAP-ARCH-01 Phase 1) ──
00035:         private MedicalHostSession _medical = null!;
00036:         private bool _medicalDirty;
00037:         private AtomicWar.GodotApp.DiseaseHostSession _disease = null!;
00038:
00039:         private string DiseaseStatusLine()
00040:         {
00041:             if (_expansions?.Disease == null) return "DISEASE WARD: offline";
00042:             if (_disease == null) SetupDisease();
00043:             if (_disease == null) return "DISEASE WARD: offline";
00044:             var s = _disease.Engine.GetSnapshot();
00045:             return $"——— DISEASE WARD ———\n" +
00046:                 $"infections {s.total_infected} · quarantined {s.total_quarantined} · " +
00047:                 $"outbreaks {s.total_outbreaks} (prevented {s.total_outbreaks_prevented}) · " +
00048:                 $"recovered {s.total_recovered} · deaths {s.total_deaths}" +
00049:                 (s.total_contagious > 0 ? "  ★ " + s.total_contagious + " CONTAGIOUS UNISOLATED" : "");
00050:         }
00051:
00052:         private void FlushMedicalIfDirty()
00053:         {
00054:             if (_medicalDirty) SaveMedical();
00055:         }
00056:
00057:         private void SetupMedical()
00058:         {
00059:             if (_medical != null) return;
00060:             _medical = MedicalHostSession.Create(_dataDir);
00061:             _medical.StateChanged += () =>
00062:             {
00063:                 _medicalDirty = true;
00064:                 _medicalPanel?.RefreshView();
00065:             };
00066:
00067:             // Plan 60 / D6 — a vigil the player keeps is care, and care must be
00068:             // recorded where the campaign remembers it: the consequence ledger already
00069:             // rides the save, so no new persistence is introduced. The names worth
00070:             // reciting are the dead this holdfast has already kept.
00071:             SetupMemorial();
00072:             _medical.BindVigilContext(
00073:                 () => _simDay,
00074:                 _consequenceLedger,
00075:                 () =>
00076:                 {
00077:                     var remembered = new List<string>();
00078:                     if (_memorial?.Entries != null)
00079:                     {
00080:                         for (int i = 0; i < _memorial.Entries.Count && remembered.Count < 6; i++)
00081:                         {
00082:                             var entry = _memorial.Entries[i];
00083:                             if (entry == null || string.IsNullOrEmpty(entry.SurvivorId)) continue;
00084:                             remembered.Add(FormatSurvivorName(entry.SurvivorId));
00085:                         }
00086:                     }
00087:                     return remembered;
00088:                 });
00089:
00090:             GD.Print("[Ashfall Godot] Medical host ready.");
00091:         }
00092:
00093:         /// <summary>
00094:         /// Plan 143: Gathers all canonical active medical affliction IDs for a survivor.
00095:         /// Queries the active pipeline handlers (respiratory, radiation, health deficit,
00096:         /// chemical dependency, psychology/trauma, diseases) and physiological limb conditions.
00097:         /// </summary>
00098:         public IReadOnlyList<string> GetActiveAfflictionIds(string survivorId)
00099:         {
00100:             var activeIds = new List<string>();
00101:             if (string.IsNullOrEmpty(survivorId)) return activeIds;
00102:
00103:             SetupMedical();
00104:             EnsureMedicalPipeline();
00105:
00106:             // 1. Pipeline active episodes
00107:             if (_medical?.Pipeline != null && Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var svId))
00108:             {
00109:                 foreach (var handler in _medical.Pipeline.Handlers)
00110:                 {
00111:                     if (handler == null) continue;
00112:                     var episode = handler.GetEpisode(svId);
00113:                     if (episode != null && episode.IsActive)
00114:                     {
00115:                         if (!activeIds.Contains(handler.DefinitionId.Value))
00116:                             activeIds.Add(handler.DefinitionId.Value);
00117:                     }
00118:                 }
00119:             }
00120:
00121:             // 2. Direct domain fallbacks
00122:             var rad = _survivors?.RadStateFor(survivorId);
00123:             if (rad is { HasAcuteRadiationSickness: true })
00124:             {
00125:                 if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId))
00126:                     activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
00127:             }
00128:
00129:             float respDeg = _phase0?.Respiratory?.RespiratoryDegradation(survivorId) ?? 0f;
00130:             if (respDeg > 0f)
00131:             {
00132:                 if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId))
00133:                     activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
00134:             }
00135:
00136:             if (_phase0?.CombatTrauma != null && _phase0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.6f)
00137:             {
00138:                 if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.CombatTraumaId))
00139:                     activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.CombatTraumaId);
00140:             }
00141:
00142:             if (_medical?.Engine != null && _medical.Engine.Ledger.TryGetValue(survivorId, out var deps))
00143:             {
00144:                 if (deps.Any(d => d.dependencyLevel >= Ashfall.Core.Medical.ChemicalDependencySystem.DependencyThreshold || d.inColdTurkey))
00145:                 {
00146:                     if (!activeIds.Contains(Ashfall.Core.Medical.MedicalTreatmentCatalog.ChemicalDependencyId))
00147:                         activeIds.Add(Ashfall.Core.Medical.MedicalTreatmentCatalog.ChemicalDependencyId);
00148:                 }
00149:             }
00150:
00151:             // 3. Limb conditions (Plan 190 AmputationSystem integration)
00152:             if (_amputation != null && _amputation.State.survivorLimbs.TryGetValue(survivorId, out var limbs))
00153:             {
00154:                 if (limbs != null)
00155:                 {
00156:                     foreach (var limb in limbs)
00157:                     {
00158:                         if (limb == null) continue;
00159:                         if (limb.limb == Ashfall.Core.Medical.LimbId.LeftLeg || limb.limb == Ashfall.Core.Medical.LimbId.RightLeg)
00160:                         {
00161:                             if (limb.condition == Ashfall.Core.Medical.LimbCondition.Wounded
00162:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Amputated
00163:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Gangrenous)
00164:                             {
00165:                                 if (!activeIds.Contains("affliction_broken_leg"))
00166:                                     activeIds.Add("affliction_broken_leg");
00167:                             }
00168:                         }
00169:                         else if (limb.limb == Ashfall.Core.Medical.LimbId.LeftArm || limb.limb == Ashfall.Core.Medical.LimbId.RightArm)
00170:                         {
00171:                             if (limb.condition == Ashfall.Core.Medical.LimbCondition.Wounded
00172:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Amputated
00173:                                 || limb.condition == Ashfall.Core.Medical.LimbCondition.Gangrenous)
00174:                             {
00175:                                 if (!activeIds.Contains("affliction_broken_arm"))
00176:                                     activeIds.Add("affliction_broken_arm");
00177:                             }
00178:                         }
00179:                     }
00180:                 }
00181:             }
00182:
00183:             return activeIds;
00184:         }
00185:
00186:         /// <summary>
00187:         /// Plan 143: Pure query checking if a quest tag is blocked for a survivor.
00188:         /// </summary>
00189:         public bool IsQuestBlockedForSurvivor(string survivorId, string questTag, out List<string> blockingAfflictions)
00190:         {
00191:             blockingAfflictions = new List<string>();
00192:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(questTag)) return false;
00193:
00194:             SetupMedical();
00195:             var activeAfflictions = GetActiveAfflictionIds(survivorId);
00196:             var gateResult = _medical.Bridge.QueryQuestGate(questTag, activeAfflictions);
00197:             if (gateResult.IsBlocked)
00198:             {
00199:                 blockingAfflictions.AddRange(gateResult.BlockingAfflictionIds);
00200:                 return true;
00201:             }
00202:             return false;
00203:         }
00204:
00205:         /// <summary>
00206:         /// Plan 143: Pure query returning quest tags unlocked by the survivor's medical conditions.
00207:         /// </summary>
00208:         public IReadOnlyList<string> GetUnlockedQuestsForSurvivor(string survivorId)
00209:         {
00210:             if (string.IsNullOrEmpty(survivorId)) return Array.Empty<string>();
00211:
00212:             SetupMedical();
00213:             var activeAfflictions = GetActiveAfflictionIds(survivorId);
00214:             return _medical.Bridge.GetUnlockedQuestTags(activeAfflictions);
00215:         }
00216:
00217:         /// <summary>
00218:         /// Task #133: construct and bind the unified medical pipeline once the
00219:         /// inventory, survivors, and Phase-0 sessions exist. Idempotent.
00220:         /// </summary>
00221:         private void EnsureMedicalPipeline()
00222:         {
00223:             SetupMedical();
00224:             if (_medical.Pipeline != null) return;
00225:             SetupSurvivors();
00226:             SetupInventory();
00227:             SetupPhase0();
00228:
00229:             var pipeline = new Ashfall.Core.Medical.MedicalPipelineCoordinator(
00230:                 _inventory.Inventory,
00231:                 new Ashfall.Core.Medical.DiagnosisKnowledgeStore(),
00232:                 new Ashfall.Core.Medical.MedicalReservationLedger(),
00233:                 new Ashfall.Core.Medical.MedicalProcedureSchedule(),
00234:                 sv => ResolvePatientAvailability(sv.Value),
00235:                 () => _simDay,
00236:                 // SHELTER_EMP_MEDICAL_POWER: clinic/ward power read from the grid.
00237:                 // Null-safe: an absent grid leaves the clinic powered (legacy behavior).
00238:                 () => _powerGrid?.System == null || _powerGrid.System.IsRoomPowered("room_clinic"),
00239:                 capabilityCheck: knowledgeId => EnsureSharedResearch().HasCapability(knowledgeId));
00240:
00241:             var respiratoryDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
00242:             var radiationDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
00243:
00244:             pipeline.RegisterHandler(new Ashfall.Core.Medical.RespiratoryAfflictionHandler(_phase0.Respiratory));
00245:             pipeline.RegisterHandler(new Ashfall.Core.Medical.RadiationSicknessAfflictionHandler(
00246:                 getDose: id => _survivors.RadStateFor(id)?.RadiationDose ?? 0f,
00247:                 getPhaseName: GetRadiationPhaseName,
00248:                 hasAcuteSickness: id => _survivors.RadStateFor(id)?.HasAcuteRadiationSickness ?? false,
00249:                 applyIodine: id => !_survivors.AdministerIodine(id).StartsWith("Unknown survivor", StringComparison.Ordinal),
00250:                 applyAntiRad: (id, rads) => !_survivors.AdministerAntiRad(id, rads).StartsWith("Unknown survivor", StringComparison.Ordinal)));
00251:             pipeline.RegisterHandler(new Ashfall.Core.Medical.HealthDeficitAfflictionHandler(
00252:                 getHealth: id => _survivors.Find(id)?.Health ?? 100f,
00253:                 getMaxHealth: id => _survivors.Find(id)?.MaxHealthCap ?? 100f,
00254:                 applyHeal: (id, amount) => !_survivors.HealSurvivor(id, amount).StartsWith("Unknown survivor", StringComparison.Ordinal)));
00255:
00256:             // Task #133 P1b — chemical-dependency detox starts flow through
00257:             // the pipeline; the shared engine keeps every withdrawal clock.
00258:             pipeline.RegisterHandler(new Ashfall.Core.Medical.ChemicalDependencyAfflictionHandler(_medical.Engine));
00259:
00260:             // Task #133 P1c — observe-only psychology projection: Phase-0
00261:             // combat trauma, flashbacks, and guilt insomnia surface as
00262:             // read-only patient rows. The handlers write nothing and own no
00263:             // clock; phase0_psychology keeps every rule and tick.
00264:             pipeline.RegisterHandler(new Ashfall.Core.Medical.GuiltInsomniaAfflictionHandler(_phase0.Guilt));
00265:             pipeline.RegisterHandler(new Ashfall.Core.Medical.SomaticFlashbackAfflictionHandler(_phase0.Flashbacks));
00266:             pipeline.RegisterHandler(new Ashfall.Core.Medical.CombatTraumaAfflictionHandler(_phase0.CombatTrauma));
00267:
00268:             // Task #133 P1 — disease write-path: one handler per authored
00269:             // disease plus the four camp-wide vector protocols. The disease
00270:             // domain keeps every clinical rule; the pipeline owns the
00271:             // validate → consume → apply transaction.
00272:             SetupDisease();
00273:             if (_disease != null)
00274:             {
00275:                 Ashfall.Core.Medical.DiseaseAfflictionHandler.RegisterAll(pipeline, _disease.Engine, _disease.Catalog);
00276:                 Ashfall.Core.Medical.DiseaseProtocolHandler.RegisterAll(pipeline, _disease.Engine, () => _simDay);
00277:
00278:                 // Auto-suspect on live infection (never confirms — the player
00279:                 // identifies the illness explicitly through the examination).
00280:                 _disease.Engine.OnInfection += (survivorId, diseaseId) =>
00281:                 {
00282:                     if (Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)
00283:                         && Ashfall.Core.Medical.AfflictionId.IsValid(diseaseId, out _))
00284:                         pipeline.SuspectFromEvidence(sv, new Ashfall.Core.Medical.AfflictionId(diseaseId), _simDay, "infection_event");
00285:                 };
00286:
00287:                 MigrateDiseaseSuspicions(pipeline);
00288:             }
00289:
00290:             // Auto-suspect: domain threshold crossings raise the shelter's
00291:             // knowledge to Suspected (never Confirmed — confirmation is explicit).
00292:             _phase0.Respiratory.OnSevereCoughStarted += survivorId =>
00293:             {
00294:                 if (Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
00295:                     pipeline.SuspectFromEvidence(sv, respiratoryDef, _simDay, "severe_cough_threshold");
00296:             };
00297:             _phase0.RadiationPhase.OnPhaseChanged += (survivorId, oldPhase, newPhase) =>
00298:             {
00299:                 if (newPhase != Ashfall.Core.Radiation.RadiationSicknessPhase.Healthy &&
00300:                     Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
00301:                     pipeline.SuspectFromEvidence(sv, radiationDef, _simDay, "radiation_phase_" + newPhase);
00302:             };
00303:
00304:             _medical.BindPipeline(pipeline);
00305:             // Task #133 P1b: share the pipeline with the ward and chem-dep
00306:             // sessions when already constructed (they backfill from
00307:             // _medical.Pipeline otherwise, covering every setup order).
00308:             if (_medicalWardSession != null) _medicalWardSession.Pipeline = pipeline;
00309:             if (_chemicalDependency != null) _chemicalDependency.Pipeline = pipeline;
00310:             MigrateLegacyMedicalDiagnoses();
00311:             GD.Print("[Ashfall Godot] Medical pipeline bound (Task #133).");
00312:         }
00313:
00314:         /// <summary>
00315:         /// Task #133 P1: restored infections that carry no diagnosis knowledge
00316:         /// are raised to Suspected (never Confirmed) so a load does not wipe
00317:         /// the suspicion trail. Idempotent — SuspectFromEvidence only moves
00318:         /// Unknown episodes.
00319:         /// </summary>
00320:         private void MigrateDiseaseSuspicions(Ashfall.Core.Medical.MedicalPipelineCoordinator pipeline)
00321:         {
00322:             if (_disease == null || _survivors == null) return;
00323:             var catalog = _disease.Catalog;
00324:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00325:             {
00326:                 var survivor = _survivors.RosterState[i];
00327:                 if (survivor == null || !survivor.IsAlive) continue;
00328:                 for (int d = 0; d < catalog.Diseases.Count; d++)
00329:                 {
00330:                     var disease = catalog.Diseases[d];
00331:                     if (disease == null || string.IsNullOrEmpty(disease.id)) continue;
00332:                     if (!Ashfall.Core.Medical.AfflictionId.IsValid(disease.id, out _)) continue;
00333:                     if (!_disease.Engine.TryGetInfection(survivor.Id, disease.id, out int _, out bool _)) continue;
00334:                     pipeline.SuspectFromEvidence(
00335:                         new Ashfall.Core.Survivors.SurvivorId(survivor.Id),
00336:                         new Ashfall.Core.Medical.AfflictionId(disease.id),
00337:                         _simDay, "restored_infection");
00338:                 }
00339:             }
00340:         }
00341:
00342:         /// <summary>Canonical lifecycle gate for the pipeline (Task #132 semantics; roster is the live authority until the entity store is host-mounted).</summary>
00343:         private Ashfall.Core.Medical.PatientAvailability ResolvePatientAvailability(string survivorId)
00344:         {
00345:             var survivor = _survivors?.Find(survivorId);
00346:             if (survivor == null) return Ashfall.Core.Medical.PatientAvailability.Blocked("patient_unknown");
00347:             if (!survivor.IsAlive) return Ashfall.Core.Medical.PatientAvailability.Blocked("patient_dead");
00348:             return Ashfall.Core.Medical.PatientAvailability.Ok();
00349:         }
00350:
00351:         private string GetRadiationPhaseName(string survivorId)
00352:         {
00353:             var phase = _phase0?.RadiationPhase;
00354:             if (phase != null && phase.Survivors.TryGetValue(survivorId, out var state))
00355:                 return state.Phase.ToString();
00356:             return "Healthy";
00357:         }
00358:
00359:         /// <summary>
00360:         /// Legacy-save migration: the pre-pipeline game displayed these
00361:         /// conditions openly, so restored episodes arrive Confirmed. New
00362:         /// progression starts Unknown/Suspected; this runs once per load.
00363:         /// </summary>
00364:         private void MigrateLegacyMedicalDiagnoses()
00365:         {
00366:             var pipeline = _medical.Pipeline;
00367:             if (pipeline == null) return;
00368:             var respiratoryDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
00369:             var radiationDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
00370:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00371:             {
00372:                 var survivor = _survivors.RosterState[i];
00373:                 if (survivor == null) continue;
00374:                 if (_phase0.Respiratory.RespiratoryDegradation(survivor.Id) > 0f)
00375:                     pipeline.ConfirmForLegacySave(new Ashfall.Core.Survivors.SurvivorId(survivor.Id), respiratoryDef, _simDay);
00376:                 if (!string.Equals(GetRadiationPhaseName(survivor.Id), "Healthy", StringComparison.Ordinal)
00377:                     || (_survivors.RadStateFor(survivor.Id)?.HasAcuteRadiationSickness ?? false))
00378:                     pipeline.ConfirmForLegacySave(new Ashfall.Core.Survivors.SurvivorId(survivor.Id), radiationDef, _simDay);
00379:             }
00380:         }
00381:
00382:         private void SaveMedicalPipeline()
00383:         {
00384:             if (_medical?.Pipeline == null) return;
00385:             var save = _medical.CapturePipelineSave();
00386:             if (save == null) return;
00387:             if (CaptureSection("medical_pipeline", MedicalPipelineSaveStore.TryCapturePersisted(save)))
00388:                 GD.Print("[Ashfall Godot] Medical pipeline save written.");
00389:         }
00390:
00391:         private void SaveMedical()
00392:         {
00393:             if (_medical == null) return;
00394:             if (CaptureSection("medical", MedicalSaveStore.TryCapturePersisted(_medical.CaptureSave())))
00395:             {
00396:                 _medicalDirty = false;
00397:                 GD.Print("[Ashfall Godot] Medical save written.");
00398:             }
00399:         }
00400:
00401:         private MedicalWardHostSession _medicalWardSession = null!;
00402:         private MedicalWardPanel _medicalWardPanel = null!;
00403:
00404:         private void SetupMedicalWard()
00405:         {
00406:             if (_medicalWardSession != null) return;
00407:             var beds = new List<Ashfall.Core.Medical.MedicalBed>
00408:             {
00409:                 new Ashfall.Core.Medical.MedicalBed("bed_general_a", "General A", Ashfall.Core.Medical.MedicalBedCategory.General),
00410:                 new Ashfall.Core.Medical.MedicalBed("bed_general_b", "General B", Ashfall.Core.Medical.MedicalBedCategory.General),
00411:                 new Ashfall.Core.Medical.MedicalBed("bed_surgical", "Surgical", Ashfall.Core.Medical.MedicalBedCategory.Surgical),
00412:                 new Ashfall.Core.Medical.MedicalBed("bed_isolation", "Isolation", Ashfall.Core.Medical.MedicalBedCategory.Isolation, isolation: true),
00413:                 new Ashfall.Core.Medical.MedicalBed("bed_chelation", "Chelation", Ashfall.Core.Medical.MedicalBedCategory.Chelation)
00414:             };
00415:             var procs = new List<Ashfall.Core.Medical.MedicalProcedureDef>
00416:             {
00417:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_bandage", "Bandage", "MedicalSystem"),
00418:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_chelation", "Chelation", "DoseLedgerSystem"),
00419:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_surgery", "Surgery", "MedicalSystem")
00420:             };
00421:             _medicalWard = new Ashfall.Core.Medical.MedicalWardSystem(
00422:                 new Ashfall.Core.Medical.MedicalWardState(), beds, procs);
00423:             _medicalWardSession = new MedicalWardHostSession(_medicalWard);
00424:             _medicalWardSession.Procedures = procs;
00425:             _medicalWardSession.SimDay = _simDay;
00426:             // Task #133 P1b: share the pipeline when already bound; otherwise
00427:             // EnsureMedicalPipeline backfills this reference once it runs.
00428:             _medicalWardSession.Pipeline = _medical?.Pipeline;
00429:             _medicalWardSession.StateChanged += () => _medicalWardDirty = true;
00430:             _medicalWard.OnWardChanged += _ => _medicalWardDirty = true;
00431:             _medicalWard.OnPatientAdmitted += patientId =>
00432:             {
00433:                 // An admitted patient is no longer available labor. The duty
00434:                 // roster remains the labor authority; the ward only announces
00435:                 // the transition so the host can vacate that existing entry.
00436:                 if (_dutyRoster?.Roster != null)
00437:                 {
00438:                     _dutyRoster.Roster.RemoveAssignmentsFor(patientId);
00439:                     _dutyRosterDirty = true;
00440:                 }
00441:             };
00442:             _medicalWard.StaffingPreflight = () =>
00443:             {
00444:                 if (_dutyRoster?.Roster == null) return true;
00445:                 string staffId = _dutyRoster.Roster.GetAssignment(DutyRosterIds.RoleWard);
00446:                 return !string.IsNullOrEmpty(staffId);
00447:             };
00448:             LoadMedicalWard();
00449:             if (_medicalWardPanel == null)
00450:             {
00451:                 _medicalWardPanel = new MedicalWardPanel();
00452:                 _medicalWardPanel.Bind(_medicalWardSession);
00453:                 _medicalWardPanel.Visible = false;
00454:                 AddChild(_medicalWardPanel);
00455:             }
00456:
00457:             // Plan 60 / D2 + D6 — the bed is where the clinical note, the authorised
00458:             // treatment, and the vigil belong, so all three are offered from the one
00459:             // surface the player is already looking at.
00460:             SetupDisease();
00461:             SetupMedical();
00462:             _medicalWardPanel.BindDisease(_disease);
00463:             _medicalWardPanel.BindVigil(_medical);
00464:         }
00465:
00466:         private void SaveMedicalWard()
00467:         {
00468:             if (_medicalWardSession == null || _medicalWard == null) return;
00469:             try
00470:             {
00471:                 _medicalWardSession.SimDay = _simDay;
00472:                 var save = new Ashfall.Core.Medical.MedicalWardSave
00473:                 {
00474:                     simDay = _medicalWardSession.SimDay,
00475:                     Beds = new List<Ashfall.Core.Medical.MedicalBedSave>(),
00476:                     Procedures = new List<Ashfall.Core.Medical.MedicalProcedureDef>(_medicalWardSession.Procedures),
00477:                     State = _medicalWard.CaptureState()
00478:                 };
00479:                 foreach (var bed in _medicalWard.Beds)
00480:                 {
00481:                     save.Beds.Add(new Ashfall.Core.Medical.MedicalBedSave
00482:                     {
00483:                         BedId = bed.BedId,
00484:                         DisplayName = bed.DisplayName,
00485:                         Category = (int)bed.Category,
00486:                         Isolation = bed.Isolation
00487:                     });
00488:                 }
00489:
00490:                 if (CaptureSection("medical_ward", MedicalWardSaveStore.TryCapturePersisted(save)))
00491:                 {
00492:                     _medicalWardDirty = false;
00493:                     _medicalWardSession.ClearDirty();
00494:                 }
00495:             }
00496:             catch (Exception e)
00497:             {
00498:                 GD.PushWarning("[Ashfall Godot] MedicalWard save failed: " + e.Message);
00499:                 CaptureSection("medical_ward", string.Empty);
00500:             }
00501:         }
00502:
00503:         private void LoadMedicalWard()
00504:         {
00505:             try
00506:             {
00507:                 var loaded = MedicalWardSaveStore.TryLoad();
00508:                 if (loaded != null)
00509:                 {
00510:                     _medicalWardSession?.RestoreSave(loaded);
00511:                     if (_medicalWardSession != null)
00512:                         _medicalWard = _medicalWardSession.System;
00513:                 }
00514:             }
00515:             catch (Exception e)
00516:             {
00517:                 GD.PushWarning("[Ashfall Godot] MedicalWard load failed: " + e.Message);
00518:             }
00519:         }
00520:
00521:         private void SaveDisease()
00522:         {
00523:             if (_disease == null) return;
00524:             try
00525:             {
00526:                 CaptureSection("disease", DiseaseSaveStore.TryCapturePersisted(_disease.Engine.CaptureState()));
00527:             }
00528:             catch (Exception e)
00529:             {
00530:                 GD.PushWarning("[Ashfall Godot] Disease save failed: " + e.Message);
00531:             }
00532:         }
00533:
00534:         private void SetupDisease()
00535:         {
00536:             if (_disease != null) return;
00537:             SetupExpansions();
00538:             var engine = _expansions.Disease;
00539:             if (engine == null)
00540:             {
00541:                 GD.PrintErr("[Ashfall Godot] Disease Expansion missing from expansion hub; ward offline.");
00542:                 return;
00543:             }
00544:             _disease = new AtomicWar.GodotApp.DiseaseHostSession(engine, _expansions.DiseaseData);
00545:             // The exposure pool is the people actually in the shelter tonight
00546:             // (duty-roster home occupants). Pure presentation wiring — the
00547:             // engine owns all rules.
00548:             // Plan 60 / D4 — protocols are armed with the day they are applied, so
00549:             // the authored window counts from the moment the work is done.
00550:             _disease.BindDayProvider(() => _simDay);
00551:             _disease.BindPopulationProvider(() =>
00552:             {
00553:                 var occupants = BuildHomeOccupantSnapshot();
00554:                 var ids = new List<string>();
00555:                 for (int i = 0; i < occupants.Count; i++)
00556:                 {
00557:                     var o = occupants[i];
00558:                     if (o != null && !string.IsNullOrEmpty(o.survivorId))
00559:                         ids.Add(o.survivorId);
00560:                 }
00561:                 return ids;
00562:             });
00563:             // Ward state rides the expansion-hub save (restored above); any
00564:             // change marks the hub dirty so nothing is lost at day end.
00565:             _disease.StateChanged += () => { _expansionHubDirty = true; };
00566:
00567:             // Plan 60 / D3 — treatment has to spend from the one item authority the
00568:             // rest of the game already uses, and a dose has to be recorded where the
00569:             // player can read it back. The ward UI could not otherwise tell a cured
00570:             // patient from one that merely got company.
00571:             _disease.BindSupply((itemId, count) =>
00572:             {
00573:                 if (count <= 0 || string.IsNullOrEmpty(itemId)) return false;
00574:                 SetupInventory();
00575:                 if (_inventory?.Inventory == null) return false;
00576:                 bool spent = _inventory.Inventory.TryConsume(itemId, count);
00577:                 if (spent) SaveInventory();
00578:                 return spent;
00579:             });
00580:
00581:             // SHELTER_FAILURE_EFFECTS: campaign quarantine containment — the
00582:             // coordinator assigns/releases isolation and computes daily isolation
00583:             // quality against the canonical disease engine. Ventilation power
00584:             // reads the room_ward_quarantine breaker (G5 delegate); daily care
00585:             // consumption mirrors BindSupply above.
00586:             SetupMedicalWard();
00587:             var quarantineCoordinator = new Ashfall.Core.Disease.DiseaseQuarantineCoordinator(
00588:                 _medicalWard,
00589:                 engine,
00590:                 _dutyRoster?.Roster,
00591:                 tryConsumeItem: (itemId, count) =>
00592:                 {
00593:                     SetupInventory();
00594:                     return _inventory?.Inventory != null && _inventory.Inventory.TryConsume(itemId, count);
00595:                 },
00596:                 containmentProvider: () => Ashfall.Core.Disease.ContainmentCapability.FromResearch(
00597:                     k => _sharedResearch?.State.completedIds.Contains(k) ?? false),
00598:                 isolationPowerCheck: () => _powerGrid?.System == null
00599:                     || _powerGrid.System.IsRoomPowered("room_ward_quarantine"));
00600:             _disease.BindCoordinator(quarantineCoordinator);
00601:             _disease.Engine.OnTreatmentApplied += (survivorId, diseaseId, itemId, role, day) =>
00602:             {
00603:                 SetupJournal();
00604:                 _journal?.TryAddRawEntry(
00605:                     $"treatment_{survivorId}_{day}_{diseaseId}",
00606:                     $"{survivorId}: {role} treatment with {itemId} for {diseaseId}.",
00607:                     null!, day);
00608:
00609:                 var patientName = FormatSurvivorName(survivorId);
00610:                 FeedbackMessages.Emit(new FeedbackEvent(
00611:                     key: "medical_treatment_success",
00612:                     arguments: new object[] { patientName },
00613:                     category: "success",
00614:                     dedupeKey: $"treatment_{survivorId}_{day}"
00615:                 ));
00616:             };
00617:
00618:             _disease.Engine.OnInfection += (survivorId, diseaseId) =>
00619:             {
00620:                 FeedbackMessages.Emit(new FeedbackEvent(
00621:                     key: "disease_alert",
00622:                     category: "alert",
00623:                     dedupeKey: $"disease_alert_{survivorId}"
00624:                 ));
00625:             };
00626:
00627:             _disease.Engine.OnOutbreakDeclared += (diseaseId) =>
00628:             {
00629:                 FeedbackMessages.Emit(new FeedbackEvent(
00630:                     key: "disease_outbreak",
00631:                     category: "warning",
00632:                     dedupeKey: $"disease_outbreak_{diseaseId}"
00633:                 ));
00634:             };
00635:
00636:             GD.Print("[Ashfall Godot] Disease Expansion ward ready (contagion · quarantine · outbreak · treatment).");
00637:
00638:             // The bed inspector is where a player stands when someone in their ward
00639:             // is dying, so treatment is offered there rather than on a parallel
00640:             // disease screen. Idempotent: BindDisease just re-points and refreshes.
00641:             SetupMedicalWard();
00642:             _medicalWardPanel?.BindDisease(_disease);
00643:         }
00644:
00645:         private void CloseMedicalPanel()
00646:         {
00647:             _medicalPanel.Visible = false;
00648:         }
00649:
00650:     }
00651: }
```

## `src/UI/Phase0Panel.cs` — 338 lines; 15,668 bytes; SHA-256 `0bef3106cde142175b083fd32dcdc96059aa3bfffb4c3f27d75318f646ffdb8d`
Declaration index:
- 00019: public partial class Phase0Panel : Control, IBindablePanel
- 00095: public void Bind(Phase0HostSession phase0, SurvivorsHostSession? survivors = null, MedicalPipelineCoordinator? pipeline = null)
- 00107: private void OnPhase0StateChanged() => RefreshView();
- 00109: public void RefreshView()
- 00146: private Control BuildSurvivorCard(Phase0SurvivorEffects fx)
- 00196: private void AppendFinalWishCard(Control card, Phase0SurvivorEffects fx)
- 00227: private void BuildCommands()
- 00268: private void ApplyInhalerThroughPipeline(string survivorId)
- 00280: private bool InhalerPreviewAllowed(string survivorId)
- 00287: private static string SeverityLabel(float v)
- 00295: private static string FormatName(string id)
- 00307: public void Open()
- 00314: public void Unbind()
- 00330: internal static class Phase0PanelLabelExtensions
- 00332: public static Label? WithColor(this Label label, Color color)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core.UI;
00005: using Ashfall.Core.Medical;
00006: using AtomicWar.GodotApp.UI;
00007:
00008: namespace AtomicWar.GodotApp.UI
00009: {
00010:     /// <summary>
00011:     /// ASHFALL — Phase 0 panel (psychological &amp; medical effects).
00012:     /// Shared survivor-condition surface for the ten Phase-0 systems: radiation
00013:     /// phase progression, phantom memory, guilt insomnia, combat trauma, somatic
00014:     /// flashback, moral branching, chemical dependency, trade specialty, final
00015:     /// wish, and respiratory degeneration. Reads Core/session state and calls
00016:     /// existing host commands only — no eligibility, tick, resource, medical,
00017:     /// morale, or narrative rules live here.
00018:     /// </summary>
00019:     public partial class Phase0Panel : Control, IBindablePanel
00020:     {
00021:         public event Action? OnClose;
00022:
00023:         private VBoxContainer _conditionList = null!;
00024:         private VBoxContainer _commandList = null!;
00025:
00026:         private Phase0HostSession? _phase0;
00027:         private SurvivorsHostSession? _survivors;
00028:         // Task #133 P1c: when bound, the APPLY INHALER action routes through
00029:         // the medical pipeline (validate → consume → apply), exactly like
00030:         // MedicalPanel. Unbound, the button stays disabled.
00031:         private MedicalPipelineCoordinator? _pipeline;
00032:
00033:         public bool IsBound => _phase0 != null;
00034:         public int RenderedConditionCount => _conditionList?.GetChildCount() ?? 0;
00035:
00036:         public override void _Ready()
00037:         {
00038:             SetAnchorsPreset(LayoutPreset.FullRect);
00039:             Visible = false;
00040:
00041:             var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
00042:             bg.SetAnchorsPreset(LayoutPreset.FullRect);
00043:             AddChild(bg);
00044:
00045:             var center = new CenterContainer();
00046:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00047:             AddChild(center);
00048:
00049:             var panel = AshfallUiHelpers.MakePanel(760, 640);
00050:             center.AddChild(panel);
00051:
00052:             var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
00053:             panel.AddChild(margins);
00054:
00055:             var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
00056:             margins.AddChild(vbox);
00057:
00058:             var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00059:             var title = AshfallUiHelpers.MakeTitle(
00060:                 "PHASE-0 // PSYCHOLOGICAL & MEDICAL CONDITIONS", Ashfall.Core.UI.Theme.FontSizeH2);
00061:             title.HorizontalAlignment = HorizontalAlignment.Left;
00062:             title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00063:             header.AddChild(title);
00064:             var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
00065:             btnClose.CustomMinimumSize = new Vector2(110, 32);
00066:             header.AddChild(btnClose);
00067:             vbox.AddChild(header);
00068:
00069:             vbox.AddChild(AshfallUiHelpers.MakeSeparator());
00070:
00071:             var scroll = new ScrollContainer
00072:             {
00073:                 CustomMinimumSize = new Vector2(720, 500),
00074:                 SizeFlagsVertical = Control.SizeFlags.ExpandFill
00075:             };
00076:             vbox.AddChild(scroll);
00077:
00078:             var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
00079:             contentBox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00080:             scroll.AddChild(contentBox);
00081:
00082:             contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVIVOR CONDITIONS"));
00083:             _conditionList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
00084:             _conditionList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00085:             contentBox.AddChild(_conditionList);
00086:
00087:             contentBox.AddChild(AshfallUiHelpers.MakeSeparator());
00088:
00089:             contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENTS & RECORD"));
00090:             _commandList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
00091:             _commandList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00092:             contentBox.AddChild(_commandList);
00093:         }
00094:
00095:         public void Bind(Phase0HostSession phase0, SurvivorsHostSession? survivors = null, MedicalPipelineCoordinator? pipeline = null)
00096:         {
00097:             if (_phase0 != null)
00098:                 _phase0.StateChanged -= OnPhase0StateChanged;
00099:             _phase0 = phase0;
00100:             if (_phase0 != null)
00101:                 _phase0.StateChanged += OnPhase0StateChanged;
00102:             _survivors = survivors;
00103:             _pipeline = pipeline;
00104:             RefreshView();
00105:         }
00106:
00107:         private void OnPhase0StateChanged() => RefreshView();
00108:
00109:         public void RefreshView()
00110:         {
00111:             if (_conditionList == null || _commandList == null) return;
00112:
00113:             AshfallUiHelpers.EmptyChildren(_conditionList);
00114:             AshfallUiHelpers.EmptyChildren(_commandList);
00115:
00116:             if (_phase0 == null)
00117:             {
00118:                 _conditionList.AddChild(AshfallUiHelpers.MakeMetadata("No Phase-0 session bound."));
00119:                 return;
00120:             }
00121:
00122:             _conditionList.AddChild(AshfallUiHelpers.MakeMetadata(
00123:                 $"Permanent shelter morale: +{_phase0.PermanentShelterMoraleBuff:0}"));
00124:
00125:             var roster = _survivors != null ? _survivors.RosterState : null;
00126:             int shown = 0;
00127:             for (int i = 0; i < (_phase0.Effects?.Count ?? 0); i++)
00128:             {
00129:                 var fx = _phase0.Effects![i];
00130:                 if (fx == null || string.IsNullOrEmpty(fx.survivorId)) continue;
00131:                 bool alive = roster == null || roster.Exists(s => s != null && s.Id == fx.survivorId && s.IsAliveState);
00132:                 if (!alive) continue;
00133:
00134:                 var card = BuildSurvivorCard(fx);
00135:                 _conditionList.AddChild(card);
00136:                 shown++;
00137:                 if (shown >= 40) break;
00138:             }
00139:
00140:             if (shown == 0)
00141:                 _conditionList.AddChild(AshfallUiHelpers.MakeMetadata("No survivors with Phase-0 conditions."));
00142:
00143:             BuildCommands();
00144:         }
00145:
00146:         private Control BuildSurvivorCard(Phase0SurvivorEffects fx)
00147:         {
00148:             var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
00149:             card.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00150:
00151:             var nameRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00152:             var name = AshfallUiHelpers.MakeSmall(FormatName(fx.survivorId));
00153:             name.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
00154:             nameRow.AddChild(name);
00155:
00156:             var radColor = fx.radiationPhase == "ManifestIllness" || fx.radiationPhase == "ChronicFibrosis"
00157:                 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)
00158:                 : fx.radiationPhase == "Prodromal"
00159:                     ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)
00160:                     : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe);
00161:             nameRow.AddChild(AshfallUiHelpers.MakeMono($"RAD {fx.radiationPhase}").WithColor(radColor));
00162:             card.AddChild(nameRow);
00163:
00164:             // ── Modifier rows with their gameplay source ──────────────
00165:             card.AddChild(AshfallUiHelpers.MakeDataRow("WORK", $"×{fx.workEfficiencyMultiplier:F2}",
00166:                 fx.workEfficiencyMultiplier < 1f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00167:             if (fx.workRefusalHours > 0f)
00168:                 card.AddChild(AshfallUiHelpers.MakeDataRow("WORK REFUSAL", $"{fx.workRefusalHours:0.0}h",
00169:                     AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
00170:             card.AddChild(AshfallUiHelpers.MakeDataRow("STAMINA", $"×{fx.staminaMultiplier:F2}",
00171:                 fx.staminaMultiplier < 1f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00172:             card.AddChild(AshfallUiHelpers.MakeDataRow("GUILT INSOMNIA", SeverityLabel(fx.guiltInsomniaSeverity),
00173:                 fx.guiltInsomniaSeverity >= Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold
00174:                     ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00175:             card.AddChild(AshfallUiHelpers.MakeDataRow("HYPERVIGILANCE", $"{fx.hypervigilance:F2}",
00176:                 fx.hypervigilance > 0.5f ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot) : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00177:             card.AddChild(AshfallUiHelpers.MakeDataRow("MORAL BRANCH", fx.moralBranch));
00178:             if (fx.dependencyCraftingPenalty > 0f)
00179:                 card.AddChild(AshfallUiHelpers.MakeDataRow("CRAFT PENALTY", $"-{fx.dependencyCraftingPenalty * 100:0}%",
00180:                     AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
00181:             if (fx.dependencyCombatPenalty > 0f)
00182:                 card.AddChild(AshfallUiHelpers.MakeDataRow("COMBAT PENALTY", $"-{fx.dependencyCombatPenalty * 100:0}%",
00183:                     AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
00184:             if (!string.IsNullOrEmpty(fx.finalWishState))
00185:                 AppendFinalWishCard(card, fx);
00186:
00187:             card.AddChild(AshfallUiHelpers.MakeSeparator());
00188:             return card;
00189:         }
00190:
00191:         /// <summary>
00192:         /// Render the final-wish block: authored title + (active) description with a
00193:         /// day/step progress row, or (completed) the completion text. Mirrors the
00194:         /// Journal house rule — text is rendered verbatim, never paraphrased.
00195:         /// </summary>
00196:         private void AppendFinalWishCard(Control card, Phase0SurvivorEffects fx)
00197:         {
00198:             bool isActive = fx.finalWishState == "active";
00199:             var accent = isActive
00200:                 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)
00201:                 : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe);
00202:
00203:             card.AddChild(AshfallUiHelpers.MakeDataRow("FINAL WISH", fx.finalWishState.ToUpperInvariant(), accent));
00204:
00205:             if (!string.IsNullOrEmpty(fx.finalWishTitle))
00206:             {
00207:                 var title = AshfallUiHelpers.MakeSmall(fx.finalWishTitle);
00208:                 title.AddThemeColorOverride("font_color", accent);
00209:                 card.AddChild(title);
00210:             }
00211:
00212:             if (isActive)
00213:             {
00214:                 if (!string.IsNullOrEmpty(fx.finalWishDescription))
00215:                     card.AddChild(AshfallUiHelpers.MakeBody(fx.finalWishDescription));
00216:                 if (fx.finalWishStepsTotal > 0)
00217:                     card.AddChild(AshfallUiHelpers.MakeDataRow("PROGRESS",
00218:                         $"{fx.finalWishStepsDone}/{fx.finalWishStepsTotal} steps · {fx.finalWishDaysRemaining:0.0} days left",
00219:                         AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00220:             }
00221:             else if (fx.finalWishState == "completed" && !string.IsNullOrEmpty(fx.finalWishCompletionText))
00222:             {
00223:                 card.AddChild(AshfallUiHelpers.MakeBody(fx.finalWishCompletionText));
00224:             }
00225:         }
00226:
00227:         private void BuildCommands()
00228:         {
00229:             _commandList.AddChild(AshfallUiHelpers.MakeSectionHeader("TREATMENTS & RECORD"));
00230:
00231:             foreach (var fx in _phase0!.Effects)
00232:             {
00233:                 if (fx == null || string.IsNullOrEmpty(fx.survivorId)) continue;
00234:                 string id = fx.survivorId;
00235:
00236:                 _commandList.AddChild(AshfallUiHelpers.MakeButton(
00237:                     $"RECORD MORAL CHOICE — {FormatName(id)} (EMPATHY)",
00238:                     () => _phase0.RecordMoralChoice(id, true)));
00239:                 _commandList.AddChild(AshfallUiHelpers.MakeButton(
00240:                     $"RECORD MORAL CHOICE — {FormatName(id)} (PRAGMATISM)",
00241:                     () => _phase0.RecordMoralChoice(id, false)));
00242:                 _commandList.AddChild(AshfallUiHelpers.MakeButton(
00243:                     $"RECORD GUILT — {FormatName(id)} (SEVERE)",
00244:                     () => _phase0.RecordGuilt(id, "choice_imposed_hardship", 0.8f)));
00245:                 _commandList.AddChild(AshfallUiHelpers.MakeButton(
00246:                     $"SURVIVED COMBAT — {FormatName(id)}",
00247:                     () => _phase0.RegisterCombatSurvived(id)));
00248:                 _commandList.AddChild(AshfallUiHelpers.MakeButton(
00249:                     $"ADVANCE FINAL WISH — {FormatName(id)}",
00250:                     () => _phase0.AdvanceFinalWish(id, "step_next")));
00251:                 var btnInhaler = AshfallUiHelpers.MakeButton(
00252:                     $"APPLY INHALER — {FormatName(id)}",
00253:                     () => ApplyInhalerThroughPipeline(id));
00254:                 // Task #133 P1c: enabled only when the pipeline is bound and
00255:                 // its preview clears (inhaler in stock, lung damage present,
00256:                 // patient available). Unbound keeps the button disabled.
00257:                 btnInhaler.Disabled = !InhalerPreviewAllowed(id);
00258:                 _commandList.AddChild(btnInhaler);
00259:             }
00260:         }
00261:
00262:         /// <summary>
00263:         /// Task #133 P1c: the inhaler action runs through the medical
00264:         /// pipeline's transaction path so it consumes inventory exactly like
00265:         /// MedicalPanel. This panel never calls the host's raw
00266:         /// <c>Phase0HostSession.ApplyInhaler</c> (that stays CLI/test-only).
00267:         /// </summary>
00268:         private void ApplyInhalerThroughPipeline(string survivorId)
00269:         {
00270:             if (_pipeline == null) return;
00271:             if (!Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)) return;
00272:
00273:             var result = _pipeline.ExecuteTreatment(sv, MedicalTreatmentCatalog.TreatmentInhaler);
00274:             if (!result.Success)
00275:                 GD.PushWarning($"[Phase0] inhaler refused for {survivorId}: {result.ReasonCode}");
00276:             RefreshView();
00277:         }
00278:
00279:         /// <summary>Preview gate for the inhaler button (side-effect free).</summary>
00280:         private bool InhalerPreviewAllowed(string survivorId)
00281:         {
00282:             if (_pipeline == null) return false;
00283:             if (!Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)) return false;
00284:             return _pipeline.PreviewTreatment(sv, MedicalTreatmentCatalog.TreatmentInhaler).IsAvailable;
00285:         }
00286:
00287:         private static string SeverityLabel(float v)
00288:         {
00289:             if (v <= 0f) return "NONE";
00290:             if (v < 0.5f) return "LIGHT";
00291:             if (v < Ashfall.Core.Survivors.GuiltInsomniaSystem.HighSeverityThreshold) return "MODERATE";
00292:             return "CRITICAL";
00293:         }
00294:
00295:         private static string FormatName(string id)
00296:         {
00297:             if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
00298:             return id switch
00299:             {
00300:                 "survivor_dr_sarah_chen" => "Dr. Sarah Chen",
00301:                 "survivor_gunner_mikhail" => "Gunner Mikhail",
00302:                 "elena_vasquez" => "Elena Vasquez",
00303:                 _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
00304:             };
00305:         }
00306:
00307:         public void Open()
00308:         {
00309:             RefreshView();
00310:             Visible = true;
00311:         }
00312:
00313:
00314:     public void Unbind()
00315:     {
00316:         if (_phase0 != null)
00317:             {
00318:                 _phase0.StateChanged -= OnPhase0StateChanged;
00319:             }
00320:         _pipeline = null;
00321:     }
00322:
00323:     public override void _ExitTree()
00324:         {
00325:             Unbind();
00326:             base._ExitTree();
00327:         }
00328:     }
00329:
00330:     internal static class Phase0PanelLabelExtensions
00331:     {
00332:         public static Label? WithColor(this Label label, Color color)
00333:         {
00334:             if (label != null) label.AddThemeColorOverride("font_color", color);
00335:             return label;
00336:         }
00337:     }
00338: }
```

## `src/UI/AfflictionsPanel.cs` — 488 lines; 23,541 bytes; SHA-256 `189133c74fdd0eff3dc5e7bc8aa518c8cef986ef5e05c8fa49d900f3219bcccf`
Declaration index:
- 00022: public partial class AfflictionsPanel : Control
- 00041: public void Bind(
- 00064: private static MedicalTextCatalog? LoadDefaultMedicalTexts()
- 00095: public void RefreshView()
- 00109: private void RenderActive()
- 00277: // Plan 193/198 — bounded medical record projection (day + event id only;
- 00304: private static string MedicalRecordLabel(string kind) => kind switch
- 00317: private void RenderChronic()
- 00391: private void RenderTreatments()
- 00421: private void AddAffliction(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
- 00430: private void AddDimSubline(VBoxContainer parent, string text)
- 00439: private Label MakeDimLine(string text)
- 00448: private static bool IsPsychologyAffliction(string afflictionId)
- 00455: private static string Name(string id)
- 00462: private int CountItem(string primaryId, string fallbackId = null!)
- 00471: public void Open()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core.UI;
00006: using Ashfall.Core.Medical;
00007: using AtomicWar.GodotApp.Host;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     /// <summary>
00012:     /// ASHFALL — Afflictions panel showing current afflictions, chronic
00013:     /// conditions, and available treatments. Bound to the live Medical /
00014:     /// Survivors / Respiratory / Inventory sessions.
00015:     ///
00016:     /// Ticket #125: layout chrome (dialog frame, sections, separators,
00017:     /// close button, hint) is owned by
00018:     /// <c>res://assets/ui/panels/AfflictionsPanel.tscn</c>. This binder
00019:     /// projects presentation data into the dynamic lists (active,
00020:     /// chronic, treatments) and wires the close action.
00021:     /// </summary>
00022:     public partial class AfflictionsPanel : Control
00023:     {
00024:         public event Action? OnClose;
00025:
00026:         private SceneBinder? _binder;
00027:
00028:         private VBoxContainer _activeList = null!;
00029:         private VBoxContainer _chronicList = null!;
00030:         private VBoxContainer _treatmentList = null!;
00031:         private Button _closeButton = null!;
00032:         public bool IsBound { get; private set; }
00033:         public int RenderedActiveCount { get; private set; }
00034:
00035:         private MedicalHostSession? _medical;
00036:         private SurvivorsHostSession? _survivors;
00037:         private InventoryHostSession? _inventory;
00038:         private RespiratoryDegenerationSystem? _respiratory;
00039:         private MedicalTextCatalog? _medicalTexts;
00040:
00041:         public void Bind(
00042:             MedicalHostSession? medical = null,
00043:             SurvivorsHostSession? survivors = null,
00044:             InventoryHostSession? inventory = null,
00045:             RespiratoryDegenerationSystem? respiratory = null,
00046:             MedicalTextCatalog? medicalTexts = null)
00047:         {
00048:             // Live refresh: affliction rows track survivor state while open.
00049:             if (_survivors != null) _survivors.StateChanged -= RefreshView;
00050:             if (_inventory != null) _inventory.StateChanged -= RefreshView;
00051:
00052:             _medical = medical;
00053:             _survivors = survivors;
00054:             _inventory = inventory;
00055:             _respiratory = respiratory;
00056:             _medicalTexts = medicalTexts ?? LoadDefaultMedicalTexts();
00057:             IsBound = _medical != null || _survivors != null;
00058:
00059:             if (_survivors != null) _survivors.StateChanged += RefreshView;
00060:             if (_inventory != null) _inventory.StateChanged += RefreshView;
00061:             RefreshView();
00062:         }
00063:
00064:         private static MedicalTextCatalog? LoadDefaultMedicalTexts()
00065:         {
00066:             try
00067:             {
00068:                 string dataDir = CatalogPath.ResolveDataDir();
00069:                 var fileIo = CatalogPath.CreateFileIOForDataDir(dataDir);
00070:                 return MedicalTextCatalog.LoadFromDirectory(dataDir, fileIo);
00071:             }
00072:             catch
00073:             {
00074:                 return null;
00075:             }
00076:         }
00077:
00078:         public override void _Ready()
00079:         {
00080:             _binder = new SceneBinder(this, typeof(AfflictionsPanel));
00081:             _binder.Require<VBoxContainer>("ActiveList");
00082:             _binder.Require<VBoxContainer>("ChronicList");
00083:             _binder.Require<VBoxContainer>("TreatmentList");
00084:             _binder.Require<Button>("CloseButton");
00085:
00086:             _activeList = _binder.Get<VBoxContainer>("ActiveList");
00087:             _chronicList = _binder.Get<VBoxContainer>("ChronicList");
00088:             _treatmentList = _binder.Get<VBoxContainer>("TreatmentList");
00089:             _closeButton = _binder.Get<Button>("CloseButton");
00090:             _closeButton.Pressed += () => OnClose?.Invoke();
00091:
00092:             Visible = false;
00093:         }
00094:
00095:         public void RefreshView()
00096:         {
00097:             if (_activeList == null || _chronicList == null || _treatmentList == null) return;
00098:
00099:             AshfallUiHelpers.EmptyChildren(_activeList);
00100:             AshfallUiHelpers.EmptyChildren(_chronicList);
00101:             AshfallUiHelpers.EmptyChildren(_treatmentList);
00102:
00103:             RenderedActiveCount = 0;
00104:             RenderActive();
00105:             RenderChronic();
00106:             RenderTreatments();
00107:         }
00108:
00109:         private void RenderActive()
00110:         {
00111:             if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
00112:             {
00113:                 _activeList.AddChild(MakeDimLine("No survivor roster bound."));
00114:                 return;
00115:             }
00116:
00117:             foreach (var s in _survivors.RosterState)
00118:             {
00119:                 if (s == null || !s.IsAlive) continue;
00120:                 var rad = _survivors.RadStateFor(s.Id);
00121:                 float respDeg = _respiratory?.RespiratoryDegradation(s.Id) ?? 0f;
00122:
00123:                 if (s.Health < 30f)
00124:                 {
00125:                     AddAffliction(_activeList, $"{Name(s.Id)} — Critical health ({s.Health:0}/100)",
00126:                         Ashfall.Core.UI.Theme.Critical);
00127:                     RenderedActiveCount++;
00128:                     if (_medicalTexts != null)
00129:                     {
00130:                         var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.HealthDeficitId, s.Id);
00131:                         if (prose != null)
00132:                         {
00133:                             string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00134:                             AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
00135:                         }
00136:                     }
00137:                 }
00138:                 if (rad is { HasAcuteRadiationSickness: true })
00139:                 {
00140:                     AddAffliction(_activeList, $"{Name(s.Id)} — Acute radiation sickness (dose {rad.RadiationDose:0} mSv)",
00141:                         Ashfall.Core.UI.Theme.Critical);
00142:                     RenderedActiveCount++;
00143:                     if (_medicalTexts != null)
00144:                     {
00145:                         var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.RadiationSicknessId, s.Id);
00146:                         if (prose != null)
00147:                         {
00148:                             string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00149:                             AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
00150:                         }
00151:                     }
00152:                 }
00153:                 if (respDeg >= RespiratoryDegenerationSystem.SevereCoughThreshold)
00154:                 {
00155:                     AddAffliction(_activeList, $"{Name(s.Id)} — Severe respiratory degeneration ({respDeg:0}%)",
00156:                         Ashfall.Core.UI.Theme.Critical);
00157:                     RenderedActiveCount++;
00158:                     if (_medicalTexts != null)
00159:                     {
00160:                         var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.RespiratoryDegenerationId, s.Id);
00161:                         if (prose != null)
00162:                         {
00163:                             string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00164:                             AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
00165:                         }
00166:                     }
00167:                 }
00168:                 else if (respDeg > 0f)
00169:                 {
00170:                     AddAffliction(_activeList, $"{Name(s.Id)} — Respiratory irritation ({respDeg:0}%)",
00171:                         Ashfall.Core.UI.Theme.Warm);
00172:                     RenderedActiveCount++;
00173:                 }
00174:
00175:                 // Task #133 P1 — disease rows from the pipeline projection.
00176:                 // Identities stay masked until an explicit identify confirms
00177:                 // them; this panel is read-only (actions live in MedicalPanel).
00178:                 // Task #133 P1c — psychology rows (trauma / flashbacks / guilt
00179:                 // insomnia) ride the same PatientRecord projection, read-only.
00180:                 if (_medical?.Pipeline != null
00181:                     && Ashfall.Core.Survivors.SurvivorId.TryParse(s.Id, out var projectSv))
00182:                 {
00183:                     var record = new PatientRecordProjector(_medical.Pipeline).Project(projectSv);
00184:                     foreach (var affliction in record.Afflictions)
00185:                     {
00186:                         bool unidentified = string.Equals(
00187:                             affliction.AfflictionId,
00188:                             MedicalTreatmentCatalog.UnidentifiedIllnessId,
00189:                             StringComparison.Ordinal);
00190:                         bool isDisease = !unidentified
00191:                             && affliction.AfflictionId.StartsWith("disease_", StringComparison.Ordinal);
00192:                         bool isPsychology = IsPsychologyAffliction(affliction.AfflictionId);
00193:                         if (!unidentified && !isDisease && !isPsychology)
00194:                             continue;
00195:
00196:                         if (unidentified)
00197:                         {
00198:                             AddAffliction(_activeList,
00199:                                 $"{Name(s.Id)} — {affliction.StageLabel} (unidentified)",
00200:                                 Ashfall.Core.UI.Theme.Warm);
00201:                         }
00202:                         else if (isPsychology)
00203:                         {
00204:                             // Phase-0 conditions are player-facing; the stage
00205:                             // label carries the state (severity stays with the
00206:                             // Phase-0 panel until a diagnosis flow exists).
00207:                             bool critical = affliction.StageLabel.Contains("CRITICAL", StringComparison.Ordinal);
00208:                             AddAffliction(_activeList,
00209:                                 $"{Name(s.Id)} — {affliction.StageLabel}",
00210:                                 critical ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm);
00211:                             if (_medicalTexts != null)
00212:                             {
00213:                                 var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, affliction.AfflictionId, s.Id);
00214:                                 if (prose != null)
00215:                                 {
00216:                                     string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00217:                                     AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
00218:                                 }
00219:                             }
00220:                         }
00221:                         else
00222:                         {
00223:                             AddAffliction(_activeList,
00224:                                 $"{Name(s.Id)} — {affliction.StageLabel} (day {affliction.SeverityValue:0})",
00225:                                 Ashfall.Core.UI.Theme.Critical);
00226:                             if (_medicalTexts != null)
00227:                             {
00228:                                 var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, affliction.AfflictionId, s.Id);
00229:                                 if (prose != null)
00230:                                 {
00231:                                     string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00232:                                     AddDimSubline(_activeList, $"   ↳ {summary} · Observe: {prose.SymptomLine}");
00233:                                 }
00234:                             }
00235:                         }
00236:                         RenderedActiveCount++;
00237:                     }
00238:                 }
00239:
00240:                 // Plan 143: Medical Afflictions -> Quest & Work Bridge projection
00241:                 if (_medical?.Bridge != null)
00242:                 {
00243:                     var activeAfflictionIds = new List<string>();
00244:                     if (s.Health < 30f) activeAfflictionIds.Add(MedicalTreatmentCatalog.HealthDeficitId);
00245:                     if (rad is { HasAcuteRadiationSickness: true }) activeAfflictionIds.Add(MedicalTreatmentCatalog.RadiationSicknessId);
00246:                     if (respDeg > 0f) activeAfflictionIds.Add(MedicalTreatmentCatalog.RespiratoryDegenerationId);
00247:                     if (_medical.Pipeline != null && Ashfall.Core.Survivors.SurvivorId.TryParse(s.Id, out var projSv))
00248:                     {
00249:                         var rec = new PatientRecordProjector(_medical.Pipeline).Project(projSv);
00250:                         foreach (var a in rec.Afflictions)
00251:                         {
00252:                             if (!string.IsNullOrEmpty(a.AfflictionId) && !activeAfflictionIds.Contains(a.AfflictionId))
00253:                                 activeAfflictionIds.Add(a.AfflictionId);
00254:                         }
00255:                     }
00256:
00257:                     if (activeAfflictionIds.Count > 0)
00258:                     {
00259:                         var workMods = _medical.Bridge.CalculateWorkModifiers(activeAfflictionIds);
00260:                         if (workMods.SpeedMultiplier < 1.0f || workMods.ExcludedDutyTypes.Count > 0)
00261:                         {
00262:                             string dutyExcl = workMods.ExcludedDutyTypes.Count > 0 ? $" · Excluded duties: {string.Join(", ", workMods.ExcludedDutyTypes)}" : string.Empty;
00263:                             AddDimSubline(_activeList, $"   ↳ Work Impact: {workMods.SpeedMultiplier * 100:0}% speed, {workMods.QualityMultiplier * 100:0}% quality{dutyExcl}");
00264:                         }
00265:                         var unlocked = _medical.Bridge.GetUnlockedQuestTags(activeAfflictionIds);
00266:                         if (unlocked.Count > 0)
00267:                         {
00268:                             AddDimSubline(_activeList, $"   ↳ Unlocked Quests: {string.Join(", ", unlocked)}");
00269:                         }
00270:                     }
00271:                 }
00272:             }
00273:
00274:             if (RenderedActiveCount == 0)
00275:                 _activeList.AddChild(MakeDimLine("No active afflictions."));
00276:
00277:             // Plan 193/198 — bounded medical record projection (day + event id only;
00278:             // no free-text notes, no second diagnosis store).
00279:             if (_medical?.Pipeline != null)
00280:             {
00281:                 var rosterIds = _survivors?.RosterState;
00282:                 if (rosterIds != null)
00283:                 {
00284:                     for (int ri = 0; ri < rosterIds.Count; ri++)
00285:                     {
00286:                         var rs = rosterIds[ri];
00287:                         if (rs == null) continue;
00288:                         var recent = _medical.Pipeline.Record.ForSurvivor(rs.Id, 2);
00289:                         if (recent.Count == 0) continue;
00290:
00291:                         _activeList.AddChild(MakeDimLine($"Recent medical record — {Name(rs.Id)}:"));
00292:                         for (int ei = 0; ei < recent.Count; ei++)
00293:                         {
00294:                             var entry = recent[ei];
00295:                             string detail = string.IsNullOrEmpty(entry.detail) ? string.Empty : $" · {entry.detail}";
00296:                             AddDimSubline(_activeList, $"   Day {entry.day} — {MedicalRecordLabel(entry.kind)}{detail}");
00297:                         }
00298:                     }
00299:                 }
00300:             }
00301:         }
00302:
00303:         /// <summary>Non-stigmatizing display label for a recorded medical event kind.</summary>
00304:         private static string MedicalRecordLabel(string kind) => kind switch
00305:         {
00306:             MedicalRecordKinds.DiagnosisSuspected => "condition suspected",
00307:             MedicalRecordKinds.DiagnosisConfirmed => "condition identified",
00308:             MedicalRecordKinds.PatientStabilized => "patient stabilized",
00309:             MedicalRecordKinds.PatientRecovered => "patient recovered",
00310:             MedicalRecordKinds.TreatmentScheduled => "treatment scheduled",
00311:             MedicalRecordKinds.TreatmentCompleted => "treatment completed",
00312:             MedicalRecordKinds.TreatmentRefused => "treatment not given",
00313:             MedicalRecordKinds.ProtocolExecuted => "camp protocol carried out",
00314:             _ => "medical event"
00315:         };
00316:
00317:         private void RenderChronic()
00318:         {
00319:             if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
00320:             {
00321:                 _chronicList.AddChild(MakeDimLine("No survivor roster bound."));
00322:                 return;
00323:             }
00324:
00325:             int chronicCount = 0;
00326:             foreach (var s in _survivors.RosterState)
00327:             {
00328:                 if (s == null || !s.IsAlive) continue;
00329:                 var rad = _survivors.RadStateFor(s.Id);
00330:
00331:                 if (rad is { HasChronicIllness: true })
00332:                 {
00333:                     AddAffliction(_chronicList, $"{Name(s.Id)} — Chronic radiation illness (lifetime {rad.LifetimeRadiationExposure:0} mSv)",
00334:                         Ashfall.Core.UI.Theme.Entropy);
00335:                     chronicCount++;
00336:                     if (_medicalTexts != null)
00337:                     {
00338:                         var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, "chronic_radiation", s.Id);
00339:                         if (prose != null)
00340:                         {
00341:                             string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00342:                             AddDimSubline(_chronicList, $"   ↳ {summary}");
00343:                         }
00344:                     }
00345:                 }
00346:                 if (_respiratory is { } r && r.HasPermanentLungDamage(s.Id))
00347:                 {
00348:                     AddAffliction(_chronicList, $"{Name(s.Id)} — Permanent lung damage", Ashfall.Core.UI.Theme.Entropy);
00349:                     chronicCount++;
00350:                     if (_medicalTexts != null)
00351:                     {
00352:                         var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, "permanent_lung_damage", s.Id);
00353:                         if (prose != null)
00354:                         {
00355:                             string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00356:                             AddDimSubline(_chronicList, $"   ↳ {summary}");
00357:                         }
00358:                     }
00359:                 }
00360:             }
00361:
00362:             if (_medical?.Engine != null)
00363:             {
00364:                 foreach (var kv in _medical.Engine.Ledger)
00365:                 {
00366:                     foreach (var dep in kv.Value)
00367:                     {
00368:                         if (dep.dependencyLevel >= ChemicalDependencySystem.DependencyThreshold)
00369:                         {
00370:                             AddAffliction(_chronicList, $"{Name(kv.Key)} — {dep.kind} dependency ({dep.dependencyLevel:P0})",
00371:                                 Ashfall.Core.UI.Theme.Entropy);
00372:                             chronicCount++;
00373:                             if (_medicalTexts != null)
00374:                             {
00375:                                 var prose = MedicalConditionResolver.GetClinicalProse(_medicalTexts, MedicalTreatmentCatalog.ChemicalDependencyId, kv.Key);
00376:                                 if (prose != null)
00377:                                 {
00378:                                     string summary = prose.DiagnosisSummary.Length > 70 ? prose.DiagnosisSummary.Substring(0, 67) + "..." : prose.DiagnosisSummary;
00379:                                     AddDimSubline(_chronicList, $"   ↳ {summary}");
00380:                                 }
00381:                             }
00382:                         }
00383:                     }
00384:                 }
00385:             }
00386:
00387:             if (chronicCount == 0)
00388:                 _chronicList.AddChild(MakeDimLine("No chronic conditions."));
00389:         }
00390:
00391:         private void RenderTreatments()
00392:         {
00393:             if (_inventory?.Inventory == null)
00394:             {
00395:                 _treatmentList.AddChild(MakeDimLine("No inventory session bound."));
00396:                 return;
00397:             }
00398:
00399:             var rows = new (string label, int count)[]
00400:             {
00401:                 ("Bandage (+25 HP)", CountItem("bandage", "item_bandage")),
00402:                 ("Iodine Pills (rad resistance)", CountItem("iodine_pills", "item_potassium_iodide")),
00403:                 ("Anti-Rad / Chelation (−40 mSv)", CountItem("rad_away", "item_rad_away")),
00404:                 ("Inhaler (respiratory relief)", CountItem("inhaler")),
00405:                 ("Herbal Tea (respiratory soothe)", CountItem("herbal_tea")),
00406:                 ("Antibiotics (infection)", CountItem("antibiotics", "item_antibiotics")),
00407:             };
00408:
00409:             bool any = false;
00410:             foreach (var (label, count) in rows)
00411:             {
00412:                 if (count <= 0) continue;
00413:                 AddAffliction(_treatmentList, $"{label} — {count} in stock", Ashfall.Core.UI.Theme.Warm);
00414:                 any = true;
00415:             }
00416:
00417:             if (!any)
00418:                 _treatmentList.AddChild(MakeDimLine("No treatment supplies in stock."));
00419:         }
00420:
00421:         private void AddAffliction(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
00422:         {
00423:             var label = new Label { Text = text };
00424:             label.CustomMinimumSize = new Vector2(400, 0);
00425:             label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
00426:             label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
00427:             parent.AddChild(label);
00428:         }
00429:
00430:         private void AddDimSubline(VBoxContainer parent, string text)
00431:         {
00432:             var label = new Label { Text = text };
00433:             label.CustomMinimumSize = new Vector2(400, 0);
00434:             label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
00435:             label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00436:             parent.AddChild(label);
00437:         }
00438:
00439:         private Label MakeDimLine(string text)
00440:         {
00441:             var l = new Label { Text = text };
00442:             l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
00443:             l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00444:             return l;
00445:         }
00446:
00447:         /// <summary>Task #133 P1c: the three observe-only Phase-0 psychology projections.</summary>
00448:         private static bool IsPsychologyAffliction(string afflictionId)
00449:         {
00450:             return afflictionId == MedicalTreatmentCatalog.CombatTraumaId
00451:                 || afflictionId == MedicalTreatmentCatalog.SomaticFlashbackId
00452:                 || afflictionId == MedicalTreatmentCatalog.GuiltInsomniaId;
00453:         }
00454:
00455:         private static string Name(string id)
00456:         {
00457:             if (string.IsNullOrEmpty(id)) return "Unknown";
00458:             int us = id.IndexOf('_');
00459:             return us >= 0 ? id.Substring(us + 1).Replace('_', ' ') : id;
00460:         }
00461:
00462:         private int CountItem(string primaryId, string fallbackId = null!)
00463:         {
00464:             if (_inventory?.Inventory == null) return 0;
00465:             int count = _inventory.Inventory.CountById(primaryId);
00466:             if (count == 0 && fallbackId != null)
00467:                 count = _inventory.Inventory.CountById(fallbackId);
00468:             return count;
00469:         }
00470:
00471:         public void Open()
00472:         {
00473:             Visible = true;
00474:             QueueRedraw();
00475:         }
00476:
00477:         public override void _UnhandledInput(InputEvent @event)
00478:         {
00479:             if (!Visible) return;
00480:
00481:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00482:             {
00483:                 OnClose?.Invoke();
00484:                 GetViewport().SetInputAsHandled();
00485:             }
00486:         }
00487:     }
00488: }
```

## `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs` — 160 lines; 5,609 bytes; SHA-256 `d1fb46927f6c5062bb6d128b215a21eb690cfa2dc7fb2fe58fa8a3b0f89fcc04`
Declaration index:
- 00007: public class GuiltInsomniaSystemTests
- 00010: public void RecordGuilt_IncreasesSeverity()
- 00019: public void RecordGuilt_MultipleSources_CapsAt1()
- 00028: public void RecordGuilt_FiresEvent()
- 00038: public void RecordGuilt_CriticalThreshold_FiresEvent()
- 00048: public void ApplySedative_ReducesSeverity()
- 00057: public void ApplySedative_NoGuilt_ReturnsFalse()
- 00064: public void ResolveDialogue_RemovesMostRecentGuilt()
- 00074: public void ResolveDialogue_LastSource_FiresResolved()
- 00085: public void SleepQuality_LowerWithGuilt()
- 00094: public void SleepQuality_SedativeHalvesPenalty()
- 00105: public void Tick_ExpiresOldGuilt()
- 00115: public void Tick_DecaysSedative()
- 00127: public void CaptureRestore_Roundtrip()
- 00144: public void RestoreNull_DoesNotCrash()
- 00152: public void RecordGuilt_RejectsEmptyId()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using Ashfall.Core.Survivors;
00003: using Xunit;
00004:
00005: namespace Ashfall.Core.Tests
00006: {
00007:     public class GuiltInsomniaSystemTests
00008:     {
00009:         [Fact]
00010:         public void RecordGuilt_IncreasesSeverity()
00011:         {
00012:             var sys = new GuiltInsomniaSystem();
00013:             sys.RecordGuilt("sv_1", "ration_cutting", 0.5f, 100);
00014:             Assert.Equal(0.5f, sys.GetInsomniaSeverity("sv_1"));
00015:             Assert.Equal(1, sys.GetGuiltSourceCount("sv_1"));
00016:         }
00017:
00018:         [Fact]
00019:         public void RecordGuilt_MultipleSources_CapsAt1()
00020:         {
00021:             var sys = new GuiltInsomniaSystem();
00022:             sys.RecordGuilt("sv_1", "source_a", 0.6f, 100);
00023:             sys.RecordGuilt("sv_1", "source_b", 0.6f, 101);
00024:             Assert.Equal(1f, sys.GetInsomniaSeverity("sv_1"));
00025:         }
00026:
00027:         [Fact]
00028:         public void RecordGuilt_FiresEvent()
00029:         {
00030:             var sys = new GuiltInsomniaSystem();
00031:             string firedFor = null;
00032:             sys.OnGuiltRecorded += (id, _) => firedFor = id;
00033:             sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
00034:             Assert.Equal("sv_1", firedFor);
00035:         }
00036:
00037:         [Fact]
00038:         public void RecordGuilt_CriticalThreshold_FiresEvent()
00039:         {
00040:             var sys = new GuiltInsomniaSystem();
00041:             string criticalFor = null;
00042:             sys.OnGuiltInsomniaCritical += id => criticalFor = id;
00043:             sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
00044:             Assert.Equal("sv_1", criticalFor);
00045:         }
00046:
00047:         [Fact]
00048:         public void ApplySedative_ReducesSeverity()
00049:         {
00050:             var sys = new GuiltInsomniaSystem();
00051:             sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
00052:             Assert.True(sys.ApplySedative("sv_1"));
00053:             Assert.Equal(0.4f, sys.GetInsomniaSeverity("sv_1"), 2);
00054:         }
00055:
00056:         [Fact]
00057:         public void ApplySedative_NoGuilt_ReturnsFalse()
00058:         {
00059:             var sys = new GuiltInsomniaSystem();
00060:             Assert.False(sys.ApplySedative("sv_1"));
00061:         }
00062:
00063:         [Fact]
00064:         public void ResolveDialogue_RemovesMostRecentGuilt()
00065:         {
00066:             var sys = new GuiltInsomniaSystem();
00067:             sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
00068:             sys.RecordGuilt("sv_1", "source_b", 0.3f, 101);
00069:             Assert.True(sys.ResolveGuiltThroughDialogue("sv_1"));
00070:             Assert.Equal(1, sys.GetGuiltSourceCount("sv_1"));
00071:         }
00072:
00073:         [Fact]
00074:         public void ResolveDialogue_LastSource_FiresResolved()
00075:         {
00076:             var sys = new GuiltInsomniaSystem();
00077:             sys.RecordGuilt("sv_1", "source_a", 0.3f, 100);
00078:             string resolvedFor = null;
00079:             sys.OnGuiltResolved += id => resolvedFor = id;
00080:             sys.ResolveGuiltThroughDialogue("sv_1");
00081:             Assert.Equal("sv_1", resolvedFor);
00082:         }
00083:
00084:         [Fact]
00085:         public void SleepQuality_LowerWithGuilt()
00086:         {
00087:             var sys = new GuiltInsomniaSystem();
00088:             Assert.Equal(1f, sys.GetSleepQualityMultiplier("sv_1"));
00089:             sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
00090:             Assert.True(sys.GetSleepQualityMultiplier("sv_1") < 1f);
00091:         }
00092:
00093:         [Fact]
00094:         public void SleepQuality_SedativeHalvesPenalty()
00095:         {
00096:             var sys = new GuiltInsomniaSystem();
00097:             sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
00098:             float before = sys.GetSleepQualityMultiplier("sv_1");
00099:             sys.ApplySedative("sv_1");
00100:             float after = sys.GetSleepQualityMultiplier("sv_1");
00101:             Assert.True(after > before);
00102:         }
00103:
00104:         [Fact]
00105:         public void Tick_ExpiresOldGuilt()
00106:         {
00107:             var sys = new GuiltInsomniaSystem();
00108:             sys.RecordGuilt("sv_1", "source_a", 0.5f, 100);
00109:             sys.Tick("sv_1", 1f, 131);
00110:             Assert.Equal(0, sys.GetGuiltSourceCount("sv_1"));
00111:             Assert.Equal(0f, sys.GetInsomniaSeverity("sv_1"));
00112:         }
00113:
00114:         [Fact]
00115:         public void Tick_DecaysSedative()
00116:         {
00117:             var sys = new GuiltInsomniaSystem();
00118:             sys.RecordGuilt("sv_1", "source_a", 0.8f, 100);
00119:             sys.ApplySedative("sv_1");
00120:             float withSedative = sys.GetSleepQualityMultiplier("sv_1");
00121:             sys.Tick("sv_1", 13f, 100); // sedative lasts 12h, so expires
00122:             float afterExpiry = sys.GetSleepQualityMultiplier("sv_1");
00123:             Assert.True(afterExpiry < withSedative, $"Expected {afterExpiry} < {withSedative}");
00124:         }
00125:
00126:         [Fact]
00127:         public void CaptureRestore_Roundtrip()
00128:         {
00129:             var sys = new GuiltInsomniaSystem();
00130:             sys.RecordGuilt("sv_1", "source_a", 0.5f, 100);
00131:             sys.RecordGuilt("sv_2", "source_b", 0.3f, 101);
00132:             sys.ApplySedative("sv_1");
00133:
00134:             var save = sys.CaptureState();
00135:             Assert.Equal(2, save.survivors.Count);
00136:
00137:             var restored = new GuiltInsomniaSystem();
00138:             restored.RestoreState(save);
00139:             Assert.Equal(1, restored.GetGuiltSourceCount("sv_1"));
00140:             Assert.Equal(1, restored.GetGuiltSourceCount("sv_2"));
00141:         }
00142:
00143:         [Fact]
00144:         public void RestoreNull_DoesNotCrash()
00145:         {
00146:             var sys = new GuiltInsomniaSystem();
00147:             sys.RestoreState(null);
00148:             Assert.Equal(0f, sys.GetInsomniaSeverity("sv_1"));
00149:         }
00150:
00151:         [Fact]
00152:         public void RecordGuilt_RejectsEmptyId()
00153:         {
00154:             var sys = new GuiltInsomniaSystem();
00155:             sys.RecordGuilt("", "source", 0.5f, 100);
00156:             sys.RecordGuilt(null, "source", 0.5f, 100);
00157:             Assert.Equal(0, sys.GetGuiltSourceCount(""));
00158:         }
00159:     }
00160: }
```

## `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs` — 357 lines; 13,182 bytes; SHA-256 `f35442bfdf970acda3b73451801713ca7b2a86ab4acf2f51c3640138067767af`
Declaration index:
- 00016: public class GuiltSourcesPlan66CatalogTests : CatalogTestBase
- 00020: private static JsonDocument LoadCatalog(string filename)
- 00029: public void Catalog_LoadsAndHasExactly40GuiltSources()
- 00106: public void Catalog_CategoryDistribution_MatchesPlan66Specification()
- 00182: public void Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid()
- 00224: public void Catalog_SeverityDistribution_IsWellCalibrated()
- 00250: public void GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically()
- 00285: public void GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold()
- 00312: public void GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords()
- 00337: public void GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Plan 66 — Guilt Sources Expansion: 20 -> 40 Psychological Consequence Triggers
00003: // Pinned contract tests for the expanded guilt_sources.json catalog, pattern uniqueness,
00004: // severity calibration, description grammar, GuiltInsomniaSystem accumulation, and save round-trip.
00005:
00006: using System;
00007: using System.Collections.Generic;
00008: using System.IO;
00009: using System.Linq;
00010: using System.Text.Json;
00011: using Xunit;
00012: using Ashfall.Core.Survivors;
00013:
00014: namespace Ashfall.Core.Tests;
00015:
00016: public class GuiltSourcesPlan66CatalogTests : CatalogTestBase
00017: {
00018:     private static string DataDir => DataDirectory;
00019:
00020:     private static JsonDocument LoadCatalog(string filename)
00021:     {
00022:         var path = Path.Combine(DataDir, filename);
00023:         Assert.True(File.Exists(path), $"Catalog file not found: {path}");
00024:         var text = File.ReadAllText(path);
00025:         return JsonDocument.Parse(text);
00026:     }
00027:
00028:     [Fact]
00029:     public void Catalog_LoadsAndHasExactly40GuiltSources()
00030:     {
00031:         using var doc = LoadCatalog("guilt_sources.json");
00032:         var root = doc.RootElement;
00033:         Assert.True(root.TryGetProperty("schema_version", out var schemaProp));
00034:         Assert.Equal(1, schemaProp.GetInt32());
00035:
00036:         Assert.True(root.TryGetProperty("items", out var itemsProp));
00037:         Assert.Equal(40, itemsProp.GetArrayLength());
00038:
00039:         var patterns = new List<string>();
00040:         foreach (var item in itemsProp.EnumerateArray())
00041:         {
00042:             Assert.True(item.TryGetProperty("choice_pattern", out var patProp));
00043:             patterns.Add(patProp.GetString()!);
00044:         }
00045:
00046:         // 20 original baseline patterns preserved
00047:         var baseline20 = new[]
00048:         {
00049:             "cut_ration",
00050:             "reduce_food",
00051:             "starve",
00052:             "leave_behind",
00053:             "abandon",
00054:             "refuse_help",
00055:             "turn_away",
00056:             "execute",
00057:             "kill",
00058:             "shoot",
00059:             "steal",
00060:             "hoard",
00061:             "take_all",
00062:             "lie",
00063:             "deceive",
00064:             "betray",
00065:             "harsh",
00066:             "refuse",
00067:             "deny",
00068:             "sacrifice_other"
00069:         };
00070:         for (int i = 0; i < baseline20.Length; i++)
00071:         {
00072:             Assert.Equal(baseline20[i], patterns[i]);
00073:         }
00074:
00075:         // 20 new patterns present
00076:         var new20 = new[]
00077:         {
00078:             "hoard_medicine_while_needed",
00079:             "barter_away_needed_food",
00080:             "issue_known_contaminated_supplies",
00081:             "burn_critical_fuel_for_comfort",
00082:             "refuse_refugee_entry",
00083:             "expel_survivor_for_efficiency",
00084:             "hide_cache_from_allies",
00085:             "abandon_committed_rescue",
00086:             "leave_wounded_behind",
00087:             "retreat_from_rescue",
00088:             "execute_surrendered_enemy",
00089:             "use_civilians_as_bait",
00090:             "kill_former_ally",
00091:             "betray_faction_trust",
00092:             "inform_on_survivor",
00093:             "break_final_wish_promise",
00094:             "withhold_pain_relief",
00095:             "triage_by_utility",
00096:             "take_family_last_supplies",
00097:             "order_survivor_to_death"
00098:         };
00099:         for (int i = 0; i < new20.Length; i++)
00100:         {
00101:             Assert.Equal(new20[i], patterns[20 + i]);
00102:         }
00103:     }
00104:
00105:     [Fact]
00106:     public void Catalog_CategoryDistribution_MatchesPlan66Specification()
00107:     {
00108:         // 20 new sources categorized per Plan 66:
00109:         // Resource (4), Shelter (3), Expedition (3), Combat (3), Social (3), Medical (2), Scavenging (1), Leadership (1)
00110:         var resourcePatterns = new[]
00111:         {
00112:             "hoard_medicine_while_needed",
00113:             "barter_away_needed_food",
00114:             "issue_known_contaminated_supplies",
00115:             "burn_critical_fuel_for_comfort"
00116:         };
00117:         var shelterPatterns = new[]
00118:         {
00119:             "refuse_refugee_entry",
00120:             "expel_survivor_for_efficiency",
00121:             "hide_cache_from_allies"
00122:         };
00123:         var expeditionPatterns = new[]
00124:         {
00125:             "abandon_committed_rescue",
00126:             "leave_wounded_behind",
00127:             "retreat_from_rescue"
00128:         };
00129:         var combatPatterns = new[]
00130:         {
00131:             "execute_surrendered_enemy",
00132:             "use_civilians_as_bait",
00133:             "kill_former_ally"
00134:         };
00135:         var socialPatterns = new[]
00136:         {
00137:             "betray_faction_trust",
00138:             "inform_on_survivor",
00139:             "break_final_wish_promise"
00140:         };
00141:         var medicalPatterns = new[]
00142:         {
00143:             "withhold_pain_relief",
00144:             "triage_by_utility"
00145:         };
00146:         var scavengingPatterns = new[]
00147:         {
00148:             "take_family_last_supplies"
00149:         };
00150:         var leadershipPatterns = new[]
00151:         {
00152:             "order_survivor_to_death"
00153:         };
00154:
00155:         Assert.Equal(4, resourcePatterns.Length);
00156:         Assert.Equal(3, shelterPatterns.Length);
00157:         Assert.Equal(3, expeditionPatterns.Length);
00158:         Assert.Equal(3, combatPatterns.Length);
00159:         Assert.Equal(3, socialPatterns.Length);
00160:         Assert.Equal(2, medicalPatterns.Length);
00161:         Assert.Single(scavengingPatterns);
00162:         Assert.Single(leadershipPatterns);
00163:
00164:         int totalNew = resourcePatterns.Length + shelterPatterns.Length + expeditionPatterns.Length +
00165:                        combatPatterns.Length + socialPatterns.Length + medicalPatterns.Length +
00166:                        scavengingPatterns.Length + leadershipPatterns.Length;
00167:         Assert.Equal(20, totalNew);
00168:
00169:         using var doc = LoadCatalog("guilt_sources.json");
00170:         var items = doc.RootElement.GetProperty("items").EnumerateArray().ToList();
00171:         var catalogPatterns = items.Select(x => x.GetProperty("choice_pattern").GetString()!).ToHashSet(StringComparer.Ordinal);
00172:
00173:         foreach (var p in resourcePatterns.Concat(shelterPatterns).Concat(expeditionPatterns)
00174:                                          .Concat(combatPatterns).Concat(socialPatterns).Concat(medicalPatterns)
00175:                                          .Concat(scavengingPatterns).Concat(leadershipPatterns))
00176:         {
00177:             Assert.Contains(p, catalogPatterns);
00178:         }
00179:     }
00180:
00181:     [Fact]
00182:     public void Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid()
00183:     {
00184:         using var doc = LoadCatalog("guilt_sources.json");
00185:         var patterns = new HashSet<string>(StringComparer.Ordinal);
00186:         var titles = new HashSet<string>(StringComparer.Ordinal);
00187:
00188:         int index = 0;
00189:         foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
00190:         {
00191:             var pattern = item.GetProperty("choice_pattern").GetString()!;
00192:             var title = item.GetProperty("title").GetString()!;
00193:             var desc = item.GetProperty("description").GetString()!;
00194:             var severity = item.GetProperty("severity").GetSingle();
00195:
00196:             // Uniqueness
00197:             Assert.True(patterns.Add(pattern), $"Duplicate choice_pattern: {pattern}");
00198:             Assert.True(titles.Add(title), $"Duplicate title: {title}");
00199:
00200:             // Title word count: 2 to 5 words
00201:             var wordCount = title.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
00202:             Assert.InRange(wordCount, 2, 5);
00203:
00204:             // Description must be non-empty
00205:             Assert.False(string.IsNullOrWhiteSpace(desc));
00206:
00207:             // All 20 new items must use {name} template
00208:             if (index >= 20)
00209:             {
00210:                 Assert.Contains("{name}", desc);
00211:             }
00212:
00213:             // Severity strictly within 0.1 to 1.0
00214:             Assert.InRange(severity, 0.1f, 1.0f);
00215:
00216:             index++;
00217:         }
00218:
00219:         Assert.Equal(40, patterns.Count);
00220:         Assert.Equal(40, titles.Count);
00221:     }
00222:
00223:     [Fact]
00224:     public void Catalog_SeverityDistribution_IsWellCalibrated()
00225:     {
00226:         using var doc = LoadCatalog("guilt_sources.json");
00227:         int minorModerate = 0;   // 0.10 - 0.50
00228:         int moderateHigh = 0;    // 0.55 - 0.70
00229:         int severeDevastating = 0; // 0.75 - 1.00
00230:
00231:         foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
00232:         {
00233:             var severity = item.GetProperty("severity").GetSingle();
00234:             if (severity <= 0.50f)
00235:                 minorModerate++;
00236:             else if (severity <= 0.70f)
00237:                 moderateHigh++;
00238:             else
00239:                 severeDevastating++;
00240:         }
00241:
00242:         // Must not be top-heavy (>60% in severe) or bottom-heavy
00243:         Assert.True(minorModerate >= 8, $"Expected >= 8 minor/moderate, got {minorModerate}");
00244:         Assert.True(moderateHigh >= 10, $"Expected >= 10 moderate/high, got {moderateHigh}");
00245:         Assert.True(severeDevastating >= 12, $"Expected >= 12 severe/devastating, got {severeDevastating}");
00246:         Assert.Equal(40, minorModerate + moderateHigh + severeDevastating);
00247:     }
00248:
00249:     [Fact]
00250:     public void GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically()
00251:     {
00252:         using var doc = LoadCatalog("guilt_sources.json");
00253:         var items = doc.RootElement.GetProperty("items").EnumerateArray().Skip(20).ToList();
00254:         Assert.Equal(20, items.Count);
00255:
00256:         for (int i = 0; i < items.Count; i++)
00257:         {
00258:             var pattern = items[i].GetProperty("choice_pattern").GetString()!;
00259:             var severity = items[i].GetProperty("severity").GetSingle();
00260:
00261:             var sys = new GuiltInsomniaSystem();
00262:             string recordedSurvivor = null;
00263:             GuiltRecord recordedRecord = null;
00264:             sys.OnGuiltRecorded += (sId, r) =>
00265:             {
00266:                 recordedSurvivor = sId;
00267:                 recordedRecord = r;
00268:             };
00269:
00270:             var survId = $"sv_test_{i}";
00271:             sys.RecordGuilt(survId, pattern, severity, currentDay: 10 + i);
00272:
00273:             Assert.Equal(survId, recordedSurvivor);
00274:             Assert.NotNull(recordedRecord);
00275:             Assert.Equal(pattern, recordedRecord.sourceId);
00276:             Assert.Equal(severity, recordedRecord.severity, 3);
00277:             Assert.Equal(10 + i, recordedRecord.dayRecorded);
00278:
00279:             Assert.Equal(1, sys.GetGuiltSourceCount(survId));
00280:             Assert.Equal(severity, sys.GetInsomniaSeverity(survId), 3);
00281:         }
00282:     }
00283:
00284:     [Fact]
00285:     public void GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold()
00286:     {
00287:         using var doc = LoadCatalog("guilt_sources.json");
00288:         var highSources = doc.RootElement.GetProperty("items").EnumerateArray()
00289:             .Skip(20)
00290:             .Where(x => x.GetProperty("severity").GetSingle() >= GuiltInsomniaSystem.HighSeverityThreshold)
00291:             .ToList();
00292:
00293:         Assert.NotEmpty(highSources);
00294:
00295:         foreach (var src in highSources)
00296:         {
00297:             var pattern = src.GetProperty("choice_pattern").GetString()!;
00298:             var severity = src.GetProperty("severity").GetSingle();
00299:
00300:             var sys = new GuiltInsomniaSystem();
00301:             string criticalSurvivor = null;
00302:             sys.OnGuiltInsomniaCritical += id => criticalSurvivor = id;
00303:
00304:             sys.RecordGuilt("sv_critical", pattern, severity, currentDay: 1);
00305:
00306:             Assert.Equal("sv_critical", criticalSurvivor);
00307:             Assert.True(sys.GetInsomniaSeverity("sv_critical") >= GuiltInsomniaSystem.HighSeverityThreshold);
00308:         }
00309:     }
00310:
00311:     [Fact]
00312:     public void GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords()
00313:     {
00314:         var sys1 = new GuiltInsomniaSystem();
00315:         sys1.RecordGuilt("sv_leader", "order_survivor_to_death", 0.90f, 5);
00316:         sys1.RecordGuilt("sv_leader", "burn_critical_fuel_for_comfort", 0.30f, 6);
00317:         sys1.RecordGuilt("sv_scout", "leave_wounded_behind", 0.80f, 7);
00318:         sys1.RecordGuilt("sv_medic", "withhold_pain_relief", 0.70f, 8);
00319:
00320:         var save = sys1.CaptureState();
00321:         Assert.Equal(3, save.survivors.Count);
00322:
00323:         var sys2 = new GuiltInsomniaSystem();
00324:         sys2.RestoreState(save);
00325:
00326:         Assert.Equal(2, sys2.GetGuiltSourceCount("sv_leader"));
00327:         Assert.Equal(1f, sys2.GetInsomniaSeverity("sv_leader")); // 0.9 + 0.3 clamped at 1.0
00328:
00329:         Assert.Equal(1, sys2.GetGuiltSourceCount("sv_scout"));
00330:         Assert.Equal(0.80f, sys2.GetInsomniaSeverity("sv_scout"), 3);
00331:
00332:         Assert.Equal(1, sys2.GetGuiltSourceCount("sv_medic"));
00333:         Assert.Equal(0.70f, sys2.GetInsomniaSeverity("sv_medic"), 3);
00334:     }
00335:
00336:     [Fact]
00337:     public void GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly()
00338:     {
00339:         var sys = new GuiltInsomniaSystem();
00340:         sys.RecordGuilt("sv_1", "break_final_wish_promise", 0.85f, 1);
00341:         sys.RecordGuilt("sv_1", "hide_cache_from_allies", 0.45f, 10);
00342:
00343:         Assert.Equal(2, sys.GetGuiltSourceCount("sv_1"));
00344:         Assert.Equal(1f, sys.GetInsomniaSeverity("sv_1")); // 0.85 + 0.45 clamped to 1.0
00345:
00346:         // Dialogue resolves newest (hide_cache_from_allies)
00347:         bool dialogueOk = sys.ResolveGuiltThroughDialogue("sv_1");
00348:         Assert.True(dialogueOk);
00349:         Assert.Equal(1, sys.GetGuiltSourceCount("sv_1"));
00350:         Assert.Equal(0.85f, sys.GetInsomniaSeverity("sv_1"), 3);
00351:
00352:         // Advance 32 days -> first record (break_final_wish_promise at day 1) expires (>30 days)
00353:         sys.Tick("sv_1", 24f, 33);
00354:         Assert.Equal(0, sys.GetGuiltSourceCount("sv_1"));
00355:         Assert.Equal(0f, sys.GetInsomniaSeverity("sv_1"));
00356:     }
00357: }
```

## `Ashfall.Core.Tests/Shelter/Plan66_68GuiltWallCarvingIntegrationTests.cs` — 197 lines; 9,232 bytes; SHA-256 `2447468de613d9a6dc5dfa4f4324293977ead2b359f3fcd1ba9af931770176e5`
Declaration index:
- 00012: public sealed class Plan66_68GuiltWallCarvingIntegrationTests
- 00014: private static string GetDataDir()
- 00022: public void GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle()
- 00096: public void WallCarvingCatalog_And_MoraleBandSelection_FullContract()
- 00140: public void PsychologicalStress_And_CulturalTrace_SystemCoupling()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Xunit;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Shelter;
00008: using Ashfall.Core.Survivors;
00009:
00010: namespace Ashfall.Core.Tests.Shelter
00011: {
00012:     public sealed class Plan66_68GuiltWallCarvingIntegrationTests
00013:     {
00014:         private static string GetDataDir()
00015:         {
00016:             if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
00017:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
00018:             throw new DirectoryNotFoundException("StreamingAssets/Data directory could not be located.");
00019:         }
00020:
00021:         [Fact]
00022:         public void GuiltSourceCatalog_And_GuiltInsomniaSystem_FullLifecycle()
00023:         {
00024:             var dataDir = GetDataDir();
00025:             var catalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
00026:
00027:             // Plan 66 catalog integrity: exactly 40 authored items
00028:             Assert.Equal(40, catalog.Count);
00029:             Assert.Equal(40, catalog.Items.Count);
00030:
00031:             foreach (var item in catalog.Items)
00032:             {
00033:                 Assert.False(string.IsNullOrWhiteSpace(item.ChoicePattern));
00034:                 Assert.False(string.IsNullOrWhiteSpace(item.Title));
00035:                 Assert.False(string.IsNullOrWhiteSpace(item.Description));
00036:                 Assert.InRange(item.Severity, 0.1f, 1.0f);
00037:             }
00038:
00039:             // Description formatting verification
00040:             var orderDeathDef = catalog.GetByPattern("order_survivor_to_death");
00041:             Assert.NotNull(orderDeathDef);
00042:             var formatted = orderDeathDef!.FormatDescription("Elena Vance");
00043:             Assert.Contains("Elena Vance", formatted);
00044:
00045:             // System integration
00046:             var insomniaSystem = new GuiltInsomniaSystem();
00047:             string criticalSurvivorId = null;
00048:             insomniaSystem.OnGuiltInsomniaCritical += sId => criticalSurvivorId = sId;
00049:
00050:             // Record moderate guilt
00051:             bool foundFuel = catalog.TryGetSeverity("burn_critical_fuel_for_comfort", out float fuelSeverity);
00052:             Assert.True(foundFuel);
00053:             insomniaSystem.RecordGuilt("surv_marcus", "burn_critical_fuel_for_comfort", fuelSeverity, currentDay: 1);
00054:
00055:             Assert.Equal(1, insomniaSystem.GetGuiltSourceCount("surv_marcus"));
00056:             Assert.Equal(fuelSeverity, insomniaSystem.GetInsomniaSeverity("surv_marcus"), 3);
00057:             Assert.Null(criticalSurvivorId);
00058:
00059:             // Record high-severity guilt pushing survivor past critical threshold
00060:             bool foundDeath = catalog.TryGetSeverity("order_survivor_to_death", out float deathSeverity);
00061:             Assert.True(foundDeath);
00062:             Assert.True(deathSeverity >= GuiltInsomniaSystem.HighSeverityThreshold);
00063:
00064:             insomniaSystem.RecordGuilt("surv_marcus", "order_survivor_to_death", deathSeverity, currentDay: 2);
00065:             Assert.Equal("surv_marcus", criticalSurvivorId);
00066:             Assert.Equal(2, insomniaSystem.GetGuiltSourceCount("surv_marcus"));
00067:             Assert.Equal(1.0f, insomniaSystem.GetInsomniaSeverity("surv_marcus"), 3);
00068:
00069:             // Sleep quality penalty
00070:             float sleepMultiplier = insomniaSystem.GetSleepQualityMultiplier("surv_marcus");
00071:             Assert.True(sleepMultiplier < 1.0f);
00072:
00073:             // Sedative compensation
00074:             bool sedativeApplied = insomniaSystem.ApplySedative("surv_marcus");
00075:             Assert.True(sedativeApplied);
00076:             Assert.True(insomniaSystem.GetInsomniaSeverity("surv_marcus") < 1.0f);
00077:
00078:             // Save / restore round-trip
00079:             var saveState = insomniaSystem.CaptureState();
00080:             Assert.NotNull(saveState);
00081:             Assert.Single(saveState.survivors);
00082:
00083:             var restoredSystem = new GuiltInsomniaSystem();
00084:             restoredSystem.RestoreState(saveState);
00085:
00086:             Assert.Equal(2, restoredSystem.GetGuiltSourceCount("surv_marcus"));
00087:             Assert.Equal(insomniaSystem.GetInsomniaSeverity("surv_marcus"), restoredSystem.GetInsomniaSeverity("surv_marcus"), 3);
00088:
00089:             // Dialogue resolution removes newest guilt record
00090:             bool dialogueOk = restoredSystem.ResolveGuiltThroughDialogue("surv_marcus");
00091:             Assert.True(dialogueOk);
00092:             Assert.Equal(1, restoredSystem.GetGuiltSourceCount("surv_marcus"));
00093:         }
00094:
00095:         [Fact]
00096:         public void WallCarvingCatalog_And_MoraleBandSelection_FullContract()
00097:         {
00098:             var dataDir = GetDataDir();
00099:             var catalog = WallCarvingCatalog.LoadFromDirectory(dataDir);
00100:
00101:             // Plan 68 catalog integrity: exactly 3 bands, 20 templates per band = 60 total
00102:             Assert.Equal(3, catalog.Bands.Count);
00103:             Assert.Equal(60, catalog.TotalTemplateCount);
00104:
00105:             var highBand = catalog.GetBandForMorale(75f);
00106:             Assert.NotNull(highBand);
00107:             Assert.Equal("high", highBand!.MoraleBand);
00108:             Assert.Equal(20, highBand.Templates.Count);
00109:             Assert.Equal(0.3f, highBand.CarvingChance, 2);
00110:
00111:             var medBand = catalog.GetBandForMorale(45f);
00112:             Assert.NotNull(medBand);
00113:             Assert.Equal("medium", medBand!.MoraleBand);
00114:             Assert.Equal(20, medBand.Templates.Count);
00115:             Assert.Equal(0.2f, medBand.CarvingChance, 2);
00116:
00117:             var lowBand = catalog.GetBandForMorale(15f);
00118:             Assert.NotNull(lowBand);
00119:             Assert.Equal("low", lowBand!.MoraleBand);
00120:             Assert.Equal(20, lowBand.Templates.Count);
00121:             Assert.Equal(0.15f, lowBand.CarvingChance, 2);
00122:
00123:             // Boundary and clamping tests
00124:             Assert.Equal("high", catalog.GetBandForMorale(100f)!.MoraleBand);
00125:             Assert.Equal("high", catalog.GetBandForMorale(125f)!.MoraleBand);
00126:             Assert.Equal("high", catalog.GetBandForMorale(60f)!.MoraleBand);
00127:             Assert.Equal("medium", catalog.GetBandForMorale(59f)!.MoraleBand);
00128:             Assert.Equal("medium", catalog.GetBandForMorale(30f)!.MoraleBand);
00129:             Assert.Equal("low", catalog.GetBandForMorale(29f)!.MoraleBand);
00130:             Assert.Equal("low", catalog.GetBandForMorale(0f)!.MoraleBand);
00131:             Assert.Equal("low", catalog.GetBandForMorale(-20f)!.MoraleBand);
00132:
00133:             // Deterministic template picking
00134:             int selectedIndex = 3;
00135:             var pickedTemplate = catalog.GetRandomTemplate(80f, max => selectedIndex % max);
00136:             Assert.Equal(highBand.Templates[selectedIndex], pickedTemplate);
00137:         }
00138:
00139:         [Fact]
00140:         public void PsychologicalStress_And_CulturalTrace_SystemCoupling()
00141:         {
00142:             var dataDir = GetDataDir();
00143:             var guiltCatalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
00144:             var carvingCatalog = WallCarvingCatalog.LoadFromDirectory(dataDir);
00145:             var insomniaSystem = new GuiltInsomniaSystem();
00146:
00147:             // Initial shelter state: steady morale (75f) -> high morale wall carving
00148:             float shelterMorale = 75f;
00149:             var initialBand = carvingCatalog.GetBandForMorale(shelterMorale);
00150:             Assert.NotNull(initialBand);
00151:             Assert.Equal("high", initialBand!.MoraleBand);
00152:             var hopefulTemplate = carvingCatalog.GetRandomTemplate(shelterMorale, _ => 0);
00153:             Assert.False(string.IsNullOrWhiteSpace(hopefulTemplate));
00154:
00155:             // Ruthless decisions induce guilt across multiple dwellers
00156:             string[] survivors = { "surv_leader", "surv_medic", "surv_scout" };
00157:             string[] patterns = { "order_survivor_to_death", "triage_by_utility", "leave_wounded_behind" };
00158:
00159:             float cumulativeInsomnia = 0f;
00160:             for (int i = 0; i < survivors.Length; i++)
00161:             {
00162:                 if (guiltCatalog.TryGetSeverity(patterns[i], out float sev))
00163:                 {
00164:                     insomniaSystem.RecordGuilt(survivors[i], patterns[i], sev, currentDay: 5);
00165:                     cumulativeInsomnia += insomniaSystem.GetInsomniaSeverity(survivors[i]);
00166:                 }
00167:             }
00168:
00169:             Assert.True(cumulativeInsomnia >= 2.0f);
00170:
00171:             // Morale drops as psychological consequences spread through the shelter
00172:             shelterMorale -= (cumulativeInsomnia * 25f);
00173:             shelterMorale = Math.Max(5f, shelterMorale);
00174:
00175:             // Wall carving culture reflects community despair
00176:             var depressedBand = carvingCatalog.GetBandForMorale(shelterMorale);
00177:             Assert.NotNull(depressedBand);
00178:             Assert.Equal("low", depressedBand!.MoraleBand);
00179:
00180:             var bleakTemplate = carvingCatalog.GetRandomTemplate(shelterMorale, _ => 0);
00181:             Assert.False(string.IsNullOrWhiteSpace(bleakTemplate));
00182:             Assert.NotEqual(hopefulTemplate, bleakTemplate);
00183:
00184:             // Therapy and reconciliation lift community out of critical despair
00185:             for (int i = 0; i < survivors.Length; i++)
00186:             {
00187:                 insomniaSystem.ApplyTherapyRelief(survivors[i], 0.80f);
00188:             }
00189:
00190:             // Morale recovers toward medium band
00191:             shelterMorale += 35f;
00192:             var recoveredBand = carvingCatalog.GetBandForMorale(shelterMorale);
00193:             Assert.NotNull(recoveredBand);
00194:             Assert.Equal("medium", recoveredBand!.MoraleBand);
00195:         }
00196:     }
00197: }
```

## `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs` — 130 lines; 5,153 bytes; SHA-256 `ce1fab1d5f8ae306d6a960403b523ec53733f42d05b45511e5a686a4a6ae84eb`
Declaration index:
- 00013: public class Phase0EffectsBridgeTests
- 00016: public void PhantomMemory_Motivation_BoostsWorkSpeedAndDecays()
- 00042: public void TradeSpecialty_CraftingItems_AdvancesTierAndMasters()
- 00045: // a parallel test class had populated the shared static
- 00076: public void FinalWish_CompletedWish_GrantsPermanentShelterMoraleBuff()
- 00098: public void GuiltInsomnia_RecordedGuilt_RaisesInsomniaSeverity()
- 00113: public void RespiratoryDegeneration_AshZoneExposure_ReducesStamina()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Medical;
00006: using Ashfall.Core.Phantoms;
00007: using Ashfall.Core.Radiation;
00008: using Ashfall.Core.Survivors;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests
00012: {
00013:     public class Phase0EffectsBridgeTests
00014:     {
00015:         [Fact]
00016:         public void PhantomMemory_Motivation_BoostsWorkSpeedAndDecays()
00017:         {
00018:             var phantom = new PhantomMemoryEngine();
00019:             phantom.RegisterRule("former_soldier", "military", 1.0f, "test", "Motivated", "Breakdown");
00020:
00021:             var sv = new PhantomSurvivorSnapshot
00022:             {
00023:                 survivorId = "sv_soldier",
00024:                 displayName = "Soldier",
00025:                 backgroundId = "former_soldier",
00026:                 isAlive = true
00027:             };
00028:
00029:             var rng = new SeededRng(12345);
00030:             var outcome = phantom.OnItemScavenged(sv, "item_dog_tags", rng);
00031:
00032:             Assert.Equal(TriggerOutcome.Motivation, outcome);
00033:             float workMult = phantom.GetWorkEfficiencyMultiplier("sv_soldier");
00034:             Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus, workMult);
00035:
00036:             // Tick past motivation duration (8h)
00037:             phantom.TickHour("sv_soldier", 9f);
00038:             Assert.Equal(1f, phantom.GetWorkEfficiencyMultiplier("sv_soldier"));
00039:         }
00040:
00041:         [Fact]
00042:         public void TradeSpecialty_CraftingItems_AdvancesTierAndMasters()
00043:         {
00044:             // D1 drift/isolation fix 2026-09-17: earlier runs depended on whether
00045:             // a parallel test class had populated the shared static
00046:             // TradeSpecialtySystem.ProfessionInfo catalog. A test-only profession
00047:             // id (never authored) guarantees no catalog entry exists, so the
00048:             // GetNarrativeEventId fallback path is exercised deterministically.
00049:             const string profession = "test_trade_probe_zzz";
00050:             TradeSpecialtySystem.RegisterProfessionPatterns(profession, new[] { "craftprobe" });
00051:
00052:             var specialty = new TradeSpecialtySystem
00053:             {
00054:                 GetNarrativeEventId = prof => $"narrative_trade_mastery_{prof}"
00055:             };
00056:             int narrativeFired = 0;
00057:             string lastNarrativeId = null!;
00058:
00059:             specialty.FireNarrativeEvent = (id, sv) =>
00060:             {
00061:                 narrativeFired++;
00062:                 lastNarrativeId = id;
00063:             };
00064:
00065:             specialty.OnItemCrafted("elena_vasquez", profession, "craftprobe_1");
00066:             specialty.OnItemCrafted("elena_vasquez", profession, "craftprobe_2");
00067:             Assert.Equal(2, specialty.GetMasteryTier("elena_vasquez"));
00068:
00069:             specialty.OnItemCrafted("elena_vasquez", profession, "craftprobe_3");
00070:             Assert.True(specialty.HasMasteredTrade("elena_vasquez"));
00071:             Assert.Equal(1, narrativeFired);
00072:             Assert.Equal("narrative_trade_mastery_test_trade_probe_zzz", lastNarrativeId);
00073:         }
00074:
00075:         [Fact]
00076:         public void FinalWish_CompletedWish_GrantsPermanentShelterMoraleBuff()
00077:         {
00078:             float shelterMoraleDelta = 0f;
00079:             var finalWish = new FinalWishSystem
00080:             {
00081:                 Rng = new SeededRng(42),
00082:                 ApplyPermanentShelterMoraleBuff = delta => shelterMoraleDelta += delta
00083:             };
00084:
00085:             finalWish.RegisterWish("parent", FinalWishSystem.WishBuildMemorial);
00086:             finalWish.DeclareTerminalPrognosis("survivor_parent", "parent", true);
00087:
00088:             Assert.True(finalWish.HasActiveWish("survivor_parent"));
00089:             finalWish.AdvanceWishStep("survivor_parent", "find_keepsake");
00090:             finalWish.AdvanceWishStep("survivor_parent", "build_shrine");
00091:             finalWish.AdvanceWishStep("survivor_parent", "inscribe_names");
00092:
00093:             Assert.True(finalWish.HasCompletedWish("survivor_parent"));
00094:             Assert.True(shelterMoraleDelta > 0f);
00095:         }
00096:
00097:         [Fact]
00098:         public void GuiltInsomnia_RecordedGuilt_RaisesInsomniaSeverity()
00099:         {
00100:             var guilt = new GuiltInsomniaSystem();
00101:             guilt.RecordGuilt("sv_scout", "abandon_refugees", 0.8f, currentDay: 1);
00102:
00103:             float severity = guilt.GetInsomniaSeverity("sv_scout");
00104:             Assert.True(severity > 0f);
00105:
00106:             // Tick 24 hours
00107:             guilt.Tick("sv_scout", 24f, currentDay: 2);
00108:             float severityDay2 = guilt.GetInsomniaSeverity("sv_scout");
00109:             Assert.True(severityDay2 > 0f);
00110:         }
00111:
00112:         [Fact]
00113:         public void RespiratoryDegeneration_AshZoneExposure_ReducesStamina()
00114:         {
00115:             bool inAshZone = true;
00116:             var respiratory = new RespiratoryDegenerationSystem
00117:             {
00118:                 GetFilterHealth = () => 0f,
00119:                 IsInFalloutStorm = () => false,
00120:                 IsInAshZone = () => inAshZone
00121:             };
00122:
00123:             // Ash zone accumulates 0.25f per hour; 205 hours = 51.25f >= 50f (SevereCoughThreshold)
00124:             respiratory.TickHours("sv_explorer", 205f);
00125:
00126:             float stamina = respiratory.GetStaminaMultiplier("sv_explorer");
00127:             Assert.True(stamina < 1f, "Ash zone exposure must reduce stamina multiplier");
00128:         }
00129:     }
00130: }
```

## `Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs` — 367 lines; 16,570 bytes; SHA-256 `b1ebdde6540be3203cad6eff01d7e15a9db3ed32164d9b475b0788d256385f01`
Declaration index:
- 00018: public class PsychologyProjectionTests
- 00022: private sealed class Fixture
- 00058: public void ActivateAllThree()
- 00066: private static string PsychologyChecksum(Fixture fx)
- 00074: public void Trauma_EpisodeAppears_WhenHypervigilancePositive()
- 00088: public void Trauma_NoEpisode_WhenHypervigilanceZero()
- 00097: public void Trauma_ProjectsHypervigilanceSymptom()
- 00111: public void Flashback_SusceptibilityOnly_ShowsSusceptibleStage()
- 00126: public void Flashback_ActiveHours_ShowFlashbackStage_AndSymptom()
- 00149: public void Flashback_NoState_NoEpisode()
- 00160: public void Guilt_BelowThreshold_ShowsInsomniaStage()
- 00175: public void Guilt_AtHighSeverityThreshold_ShowsCriticalInsomnia()
- 00189: public void PsychologyTreatments_AreAlwaysRefused_WithoutConsuming()
- 00222: public void PsychologyHandlers_NeverMutateTheirDomains()
- 00240: public void PsychologyHandlers_ProjectThroughPatientRecord()
- 00261: public void PsychologyHandlers_AreObserveOnly_BySource()
- 00276: public void DomainTicks_StillAdvance_AndProjectionsFollow()
- 00303: public void PipelineInhaler_PreviewUnavailable_WithoutDamageOrSupply()
- 00321: public void PipelineInhaler_ExecutesThroughPipeline_ConsumesOne()
- 00335: public void PipelineInhaler_MatchesDirectApplyInhaler_Checksum()
- 00354: private static string FindRepoFile(params string[] segments)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // Task #133 P1c — Psychology observe-only projection + Phase0 inhaler pipeline path.
00003: using System.Linq;
00004: using Ashfall.Core.Medical;
00005: using Ashfall.Core.Survivors;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests.Medical
00009: {
00010:     /// <summary>
00011:     /// Task #133 P1c: the three Phase-0 psychology conditions (combat trauma,
00012:     /// somatic flashbacks, guilt insomnia) project into the medical pipeline
00013:     /// as read-only patient rows. The handlers never treat, never tick, and
00014:     /// never touch the Phase-0 day owner's clocks; the Phase-0 inhaler action
00015:     /// flows through the same pipeline ExecuteTreatment path MedicalPanel
00016:     /// uses (consume + parity with the raw domain ApplyInhaler).
00017:     /// </summary>
00018:     public class PsychologyProjectionTests
00019:     {
00020:         private const string SvId = "survivor_psych_patient";
00021:
00022:         private sealed class Fixture
00023:         {
00024:             public Ashfall.Core.Inventory.Inventory Inventory { get; }
00025:                 = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
00026:             public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
00027:             public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
00028:             public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
00029:             public CombatTraumaSystem Trauma { get; } = new CombatTraumaSystem();
00030:             public SomaticFlashbackSystem Flashbacks { get; } = new SomaticFlashbackSystem();
00031:             public GuiltInsomniaSystem Guilt { get; } = new GuiltInsomniaSystem();
00032:             public RespiratoryDegenerationSystem Respiratory { get; } = new RespiratoryDegenerationSystem();
00033:             public MedicalPipelineCoordinator Pipeline { get; }
00034:             public PatientRecordProjector Projector { get; }
00035:             public CombatTraumaAfflictionHandler TraumaHandler { get; }
00036:             public SomaticFlashbackAfflictionHandler FlashbackHandler { get; }
00037:             public GuiltInsomniaAfflictionHandler GuiltHandler { get; }
00038:
00039:             public Fixture()
00040:             {
00041:                 TraumaHandler = new CombatTraumaAfflictionHandler(Trauma);
00042:                 FlashbackHandler = new SomaticFlashbackAfflictionHandler(Flashbacks);
00043:                 GuiltHandler = new GuiltInsomniaAfflictionHandler(Guilt);
00044:                 Pipeline = new MedicalPipelineCoordinator(
00045:                     Inventory, Diagnosis, Reservations, Schedule,
00046:                     _ => PatientAvailability.Ok(), () => 1);
00047:                 Pipeline.RegisterHandler(new RespiratoryAfflictionHandler(Respiratory));
00048:                 Pipeline.RegisterHandler(TraumaHandler);
00049:                 Pipeline.RegisterHandler(FlashbackHandler);
00050:                 Pipeline.RegisterHandler(GuiltHandler);
00051:                 Projector = new PatientRecordProjector(Pipeline);
00052:                 Inventory.TryProduce("inhaler", 3);
00053:                 Inventory.TryProduce("bandage", 3);
00054:             }
00055:
00056:             public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);
00057:
00058:             public void ActivateAllThree()
00059:             {
00060:                 Trauma.OnCombatSurvived(SvId);
00061:                 Flashbacks.IncreaseSusceptibility(SvId, 0.3f);
00062:                 Guilt.RecordGuilt(SvId, "choice_test", 0.5f, 2);
00063:             }
00064:         }
00065:
00066:         private static string PsychologyChecksum(Fixture fx)
00067:             => Ashfall.Core.SaveChecksum.Compute(fx.Trauma.CaptureState())
00068:              + "|" + Ashfall.Core.SaveChecksum.Compute(fx.Flashbacks.CaptureState())
00069:              + "|" + Ashfall.Core.SaveChecksum.Compute(fx.Guilt.CaptureState());
00070:
00071:         // ── Combat trauma projection ─────────────────────────────────
00072:
00073:         [Fact]
00074:         public void Trauma_EpisodeAppears_WhenHypervigilancePositive()
00075:         {
00076:             var fx = new Fixture();
00077:             fx.Trauma.OnCombatSurvived(SvId);
00078:
00079:             var episode = fx.TraumaHandler.GetEpisode(fx.Sv);
00080:
00081:             Assert.NotNull(episode);
00082:             Assert.Equal(CombatTraumaAfflictionHandler.StageHypervigilant, episode!.StageLabel);
00083:             Assert.Equal(fx.Trauma.GetHypervigilanceLevel(SvId) * 100f, episode.SeverityValue, 3);
00084:             Assert.True(episode.IsActive);
00085:         }
00086:
00087:         [Fact]
00088:         public void Trauma_NoEpisode_WhenHypervigilanceZero()
00089:         {
00090:             var fx = new Fixture();
00091:
00092:             Assert.Null(fx.TraumaHandler.GetEpisode(fx.Sv));
00093:             Assert.Empty(fx.TraumaHandler.ProjectSymptoms(fx.Sv));
00094:         }
00095:
00096:         [Fact]
00097:         public void Trauma_ProjectsHypervigilanceSymptom()
00098:         {
00099:             var fx = new Fixture();
00100:             fx.Trauma.OnCombatSurvived(SvId);
00101:
00102:             var symptoms = fx.TraumaHandler.ProjectSymptoms(fx.Sv);
00103:
00104:             var symptom = Assert.Single(symptoms);
00105:             Assert.Equal(CombatTraumaAfflictionHandler.SymptomHypervigilance, symptom.SymptomId);
00106:         }
00107:
00108:         // ── Flashback projection ─────────────────────────────────────
00109:
00110:         [Fact]
00111:         public void Flashback_SusceptibilityOnly_ShowsSusceptibleStage()
00112:         {
00113:             var fx = new Fixture();
00114:             fx.Flashbacks.IncreaseSusceptibility(SvId, 0.3f);
00115:
00116:             var episode = fx.FlashbackHandler.GetEpisode(fx.Sv);
00117:
00118:             Assert.NotNull(episode);
00119:             Assert.Equal(SomaticFlashbackAfflictionHandler.StageSusceptible, episode!.StageLabel);
00120:             Assert.Equal(30f, episode.SeverityValue, 3);
00121:             // No active flashback yet: the flashback symptom stays hidden.
00122:             Assert.Empty(fx.FlashbackHandler.ProjectSymptoms(fx.Sv));
00123:         }
00124:
00125:         [Fact]
00126:         public void Flashback_ActiveHours_ShowFlashbackStage_AndSymptom()
00127:         {
00128:             var fx = new Fixture();
00129:             var state = new FlashbackSurvivorState
00130:             {
00131:                 survivorId = SvId,
00132:                 susceptibility = 0.3f,
00133:                 activeRemainingHours = 4f
00134:             };
00135:             var save = new SomaticFlashbackSaveState();
00136:             save.survivors.Add(state);
00137:             fx.Flashbacks.RestoreState(save);
00138:
00139:             var episode = fx.FlashbackHandler.GetEpisode(fx.Sv);
00140:
00141:             Assert.NotNull(episode);
00142:             Assert.Equal(SomaticFlashbackAfflictionHandler.StageFlashback, episode!.StageLabel);
00143:             Assert.Equal(4f, episode.SeverityValue, 3);
00144:             var symptom = Assert.Single(fx.FlashbackHandler.ProjectSymptoms(fx.Sv));
00145:             Assert.Equal(SomaticFlashbackAfflictionHandler.SymptomFlashback, symptom.SymptomId);
00146:         }
00147:
00148:         [Fact]
00149:         public void Flashback_NoState_NoEpisode()
00150:         {
00151:             var fx = new Fixture();
00152:
00153:             Assert.Null(fx.FlashbackHandler.GetEpisode(fx.Sv));
00154:             Assert.Empty(fx.FlashbackHandler.ProjectSymptoms(fx.Sv));
00155:         }
00156:
00157:         // ── Guilt insomnia projection ────────────────────────────────
00158:
00159:         [Fact]
00160:         public void Guilt_BelowThreshold_ShowsInsomniaStage()
00161:         {
00162:             var fx = new Fixture();
00163:             fx.Guilt.RecordGuilt(SvId, "choice_test", 0.5f, currentDay: 2);
00164:
00165:             var episode = fx.GuiltHandler.GetEpisode(fx.Sv);
00166:
00167:             Assert.NotNull(episode);
00168:             Assert.Equal(GuiltInsomniaAfflictionHandler.StageInsomnia, episode!.StageLabel);
00169:             Assert.Equal(50f, episode.SeverityValue, 3);
00170:             var symptom = Assert.Single(fx.GuiltHandler.ProjectSymptoms(fx.Sv));
00171:             Assert.Equal(GuiltInsomniaAfflictionHandler.SymptomInsomnia, symptom.SymptomId);
00172:         }
00173:
00174:         [Fact]
00175:         public void Guilt_AtHighSeverityThreshold_ShowsCriticalInsomnia()
00176:         {
00177:             var fx = new Fixture();
00178:             fx.Guilt.RecordGuilt(SvId, "choice_test", 0.8f, currentDay: 2);
00179:
00180:             var episode = fx.GuiltHandler.GetEpisode(fx.Sv);
00181:
00182:             Assert.NotNull(episode);
00183:             Assert.Equal(GuiltInsomniaAfflictionHandler.StageCriticalInsomnia, episode!.StageLabel);
00184:         }
00185:
00186:         // ── Observe-only contract ────────────────────────────────────
00187:
00188:         [Fact]
00189:         public void PsychologyTreatments_AreAlwaysRefused_WithoutConsuming()
00190:         {
00191:             var fx = new Fixture();
00192:             fx.ActivateAllThree();
00193:             int inhalersBefore = fx.Inventory.CountById("inhaler");
00194:             int bandagesBefore = fx.Inventory.CountById("bandage");
00195:
00196:             var targets = new[]
00197:             {
00198:                 new AfflictionId(MedicalTreatmentCatalog.CombatTraumaId),
00199:                 new AfflictionId(MedicalTreatmentCatalog.SomaticFlashbackId),
00200:                 new AfflictionId(MedicalTreatmentCatalog.GuiltInsomniaId)
00201:             };
00202:             foreach (var target in targets)
00203:             {
00204:                 foreach (string treatmentId in new[]
00205:                 {
00206:                     MedicalTreatmentCatalog.TreatmentBandage,
00207:                     MedicalTreatmentCatalog.TreatmentInhaler
00208:                 })
00209:                 {
00210:                     var result = fx.Pipeline.ExecuteTreatment(fx.Sv, treatmentId, target: target);
00211:                     Assert.False(result.Success);
00212:                     Assert.Equal("treatment_not_for_affliction", result.ReasonCode);
00213:                 }
00214:             }
00215:
00216:             Assert.Equal(inhalersBefore, fx.Inventory.CountById("inhaler"));
00217:             Assert.Equal(bandagesBefore, fx.Inventory.CountById("bandage"));
00218:             Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
00219:         }
00220:
00221:         [Fact]
00222:         public void PsychologyHandlers_NeverMutateTheirDomains()
00223:         {
00224:             var fx = new Fixture();
00225:             fx.ActivateAllThree();
00226:             string before = PsychologyChecksum(fx);
00227:
00228:             _ = fx.TraumaHandler.GetEpisode(fx.Sv);
00229:             _ = fx.TraumaHandler.ProjectSymptoms(fx.Sv);
00230:             _ = fx.TraumaHandler.ValidateTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentBandage);
00231:             _ = fx.FlashbackHandler.GetEpisode(fx.Sv);
00232:             _ = fx.FlashbackHandler.ProjectSymptoms(fx.Sv);
00233:             _ = fx.GuiltHandler.GetEpisode(fx.Sv);
00234:             _ = fx.GuiltHandler.ProjectSymptoms(fx.Sv);
00235:
00236:             Assert.Equal(before, PsychologyChecksum(fx));
00237:         }
00238:
00239:         [Fact]
00240:         public void PsychologyHandlers_ProjectThroughPatientRecord()
00241:         {
00242:             var fx = new Fixture();
00243:             fx.ActivateAllThree();
00244:
00245:             var record = fx.Projector.Project(fx.Sv);
00246:
00247:             var ids = record.Afflictions.Select(a => a.AfflictionId).ToList();
00248:             Assert.Contains(MedicalTreatmentCatalog.CombatTraumaId, ids);
00249:             Assert.Contains(MedicalTreatmentCatalog.SomaticFlashbackId, ids);
00250:             Assert.Contains(MedicalTreatmentCatalog.GuiltInsomniaId, ids);
00251:             // Player-facing by design: rows render without any diagnosis traffic.
00252:             Assert.Contains(record.Afflictions, a =>
00253:                 a.AfflictionId == MedicalTreatmentCatalog.CombatTraumaId
00254:                 && a.DiagnosisStatus == "unknown"
00255:                 && a.StageLabel == CombatTraumaAfflictionHandler.StageHypervigilant);
00256:             Assert.Contains(record.Symptoms, s => s.SymptomId == CombatTraumaAfflictionHandler.SymptomHypervigilance);
00257:             Assert.Contains(record.Symptoms, s => s.SymptomId == GuiltInsomniaAfflictionHandler.SymptomInsomnia);
00258:         }
00259:
00260:         [Fact]
00261:         public void PsychologyHandlers_AreObserveOnly_BySource()
00262:         {
00263:             // Mirrors the architecture gate at the unit level: the handler
00264:             // source must not call the Phase-0 mutation or clock APIs.
00265:             string source = System.IO.File.ReadAllText(FindRepoFile(
00266:                 "Assets", "Ashfall.Core", "Medical", "PsychologyAfflictionHandlers.cs"));
00267:             Assert.DoesNotContain(".Tick(", source);
00268:             Assert.DoesNotContain(".ApplySedative(", source);
00269:             Assert.DoesNotContain(".OnCombatSurvived(", source);
00270:             Assert.DoesNotContain(".IncreaseSusceptibility(", source);
00271:         }
00272:
00273:         // ── Phase-0 keeps the clocks (handlers never steal them) ─────
00274:
00275:         [Fact]
00276:         public void DomainTicks_StillAdvance_AndProjectionsFollow()
00277:         {
00278:             var fx = new Fixture();
00279:             fx.ActivateAllThree();
00280:             Assert.NotNull(fx.TraumaHandler.GetEpisode(fx.Sv));
00281:
00282:             // The Phase-0 day owner drives the domains directly; the handlers
00283:             // never intercept. 96h past the combat-decay threshold erases
00284:             // hypervigilance (0.05 − 0.02 × 4 days).
00285:             fx.Trauma.Tick(SvId, 96f, isNightTime: false);
00286:             Assert.Equal(0f, fx.Trauma.GetHypervigilanceLevel(SvId));
00287:             Assert.Null(fx.TraumaHandler.GetEpisode(fx.Sv));
00288:
00289:             // Flashback susceptibility decays 0.03/day; 4 days erodes 0.3 → 0.18.
00290:             fx.Flashbacks.Tick(SvId, 96f);
00291:             Assert.Equal(0.18f, fx.Flashbacks.GetSusceptibility(SvId), 2);
00292:             Assert.Equal(18f, fx.FlashbackHandler.GetEpisode(fx.Sv)!.SeverityValue, 1);
00293:
00294:             // Guilt sources expire after 30 days; the insomnia episode closes.
00295:             fx.Guilt.Tick(SvId, 1f, currentDay: 40);
00296:             Assert.Equal(0f, fx.Guilt.GetInsomniaSeverity(SvId));
00297:             Assert.Null(fx.GuiltHandler.GetEpisode(fx.Sv));
00298:         }
00299:
00300:         // ── Phase-0 inhaler through the pipeline (Task #133 P1c) ─────
00301:
00302:         [Fact]
00303:         public void PipelineInhaler_PreviewUnavailable_WithoutDamageOrSupply()
00304:         {
00305:             var fx = new Fixture();
00306:
00307:             // No lung damage: blocked regardless of stock.
00308:             var noDamage = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
00309:             Assert.False(noDamage.IsAvailable);
00310:             Assert.Equal("no_respiratory_damage", noDamage.FailureCode);
00311:
00312:             // Damage but no inhaler: blocked as missing medicine.
00313:             fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
00314:             fx.Inventory.TryConsume("inhaler", 3);
00315:             var noMedicine = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
00316:             Assert.False(noMedicine.IsAvailable);
00317:             Assert.Equal("missing_medicine", noMedicine.FailureCode);
00318:         }
00319:
00320:         [Fact]
00321:         public void PipelineInhaler_ExecutesThroughPipeline_ConsumesOne()
00322:         {
00323:             var fx = new Fixture();
00324:             fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
00325:
00326:             var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
00327:
00328:             Assert.True(result.Success, result.ReasonCode);
00329:             Assert.Equal(2, fx.Inventory.CountById("inhaler"));
00330:             Assert.Equal(10f, fx.Respiratory.RespiratoryDegradation(SvId), 3); // 20 − 10
00331:             Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
00332:         }
00333:
00334:         [Fact]
00335:         public void PipelineInhaler_MatchesDirectApplyInhaler_Checksum()
00336:         {
00337:             // Path A: the pipeline path the Phase-0 panel now drives.
00338:             var fx = new Fixture();
00339:             fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 55f;
00340:             var viaPipeline = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
00341:
00342:             // Path B: the raw domain call the host CLI/tests keep using
00343:             // (Phase0HostSession.ApplyInhaler forwards here 1:1).
00344:             var legacy = new RespiratoryDegenerationSystem();
00345:             legacy.GetOrCreate(SvId).respiratoryDegradation = 55f;
00346:             legacy.ApplyInhaler(SvId);
00347:
00348:             Assert.True(viaPipeline.Success, viaPipeline.ReasonCode);
00349:             Assert.Equal(
00350:                 Ashfall.Core.SaveChecksum.Compute(legacy.CaptureState()),
00351:                 Ashfall.Core.SaveChecksum.Compute(fx.Respiratory.CaptureState()));
00352:         }
00353:
00354:         private static string FindRepoFile(params string[] segments)
00355:         {
00356:             var dir = new System.IO.DirectoryInfo(System.AppContext.BaseDirectory);
00357:             while (dir != null)
00358:             {
00359:                 string probe = System.IO.Path.Combine(dir.FullName, "Ashfall.csproj");
00360:                 if (System.IO.File.Exists(probe))
00361:                     return System.IO.Path.Combine(new[] { dir.FullName }.Concat(segments).ToArray());
00362:                 dir = dir.Parent;
00363:             }
00364:             throw new System.IO.DirectoryNotFoundException("Repository root not found");
00365:         }
00366:     }
00367: }
```

## `Ashfall.Core.Tests/InstitutionCanonicalReliefTests.cs` — 96 lines; 3,789 bytes; SHA-256 `2584de199a20251ed65eadfea5efb160e141da4b33bf7691f9ed0b6d5913c80a`
Declaration index:
- 00013: public class InstitutionCanonicalReliefTests
- 00016: public void CombatTrauma_TherapyRelief_ScalesHypervigilanceDown()
- 00039: public void Flashback_ReduceSusceptibility_FloorsAtZero()
- 00058: public void GuiltInsomnia_TherapyRelief_ScalesInsomniaDown()
- 00073: public void Vinyl_MergeRecord_AddsWithoutReplacing_Catalog()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Survivors;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     /// <summary>
00010:     /// Host-binding wave 2: the canonical relief APIs the sanatorium port
00011:     /// consumes, and the vinyl merge path the culture archive cutting uses.
00012:     /// </summary>
00013:     public class InstitutionCanonicalReliefTests
00014:     {
00015:         [Fact]
00016:         public void CombatTrauma_TherapyRelief_ScalesHypervigilanceDown()
00017:         {
00018:             var sys = new CombatTraumaSystem();
00019:             sys.RegisterSurvivor("sv");
00020:             sys.OnCombatSurvived("sv");
00021:             sys.OnCombatSurvived("sv");
00022:             sys.OnCombatSurvived("sv");
00023:             float before = sys.GetHypervigilanceLevel("sv");
00024:             Assert.True(before > 0f);
00025:
00026:             sys.ApplyTherapyRelief("sv", 0.5f);
00027:             Assert.Equal(before * 0.5f, sys.GetHypervigilanceLevel("sv"), 4);
00028:
00029:             sys.ApplyTherapyRelief("sv", 1f);
00030:             Assert.Equal(0f, sys.GetHypervigilanceLevel("sv"), 4);
00031:
00032:             // unknown survivor / zero fraction are no-ops
00033:             sys.ApplyTherapyRelief("ghost", 1f);
00034:             sys.ApplyTherapyRelief("sv", 0f);
00035:             Assert.Equal(0f, sys.GetHypervigilanceLevel("sv"), 4);
00036:         }
00037:
00038:         [Fact]
00039:         public void Flashback_ReduceSusceptibility_FloorsAtZero()
00040:         {
00041:             var sys = new SomaticFlashbackSystem();
00042:             sys.IncreaseSusceptibility("sv", 0.4f);
00043:             Assert.Equal(0.4f, sys.GetSusceptibility("sv"), 4);
00044:
00045:             sys.ReduceSusceptibility("sv", 0.15f);
00046:             Assert.Equal(0.25f, sys.GetSusceptibility("sv"), 4);
00047:
00048:             sys.ReduceSusceptibility("sv", 9f);
00049:             Assert.Equal(0f, sys.GetSusceptibility("sv"), 4);
00050:
00051:             // unknown survivor no-op, negative amounts rejected
00052:             sys.ReduceSusceptibility("ghost", 1f);
00053:             sys.ReduceSusceptibility("sv", -1f);
00054:             Assert.Equal(0f, sys.GetSusceptibility("sv"), 4);
00055:         }
00056:
00057:         [Fact]
00058:         public void GuiltInsomnia_TherapyRelief_ScalesInsomniaDown()
00059:         {
00060:             var sys = new GuiltInsomniaSystem();
00061:             sys.RecordGuilt("sv", "source_a", 0.8f, currentDay: 1);
00062:             float before = sys.GetInsomniaSeverity("sv");
00063:             Assert.True(before > 0f);
00064:
00065:             sys.ApplyTherapyRelief("sv", 0.25f);
00066:             Assert.Equal(before * 0.75f, sys.GetInsomniaSeverity("sv"), 4);
00067:
00068:             sys.ApplyTherapyRelief("ghost", 1f);
00069:             Assert.Equal(before * 0.75f, sys.GetInsomniaSeverity("sv"), 4);
00070:         }
00071:
00072:         [Fact]
00073:         public void Vinyl_MergeRecord_AddsWithoutReplacing_Catalog()
00074:         {
00075:             var sys = new VinylMoraleSystem();
00076:             var preWar = new VinylRecordDefinition { record_id = "pre_war_a", display_name = "Pre-War" };
00077:             sys.LoadCatalog(new List<VinylRecordDefinition> { preWar });
00078:
00079:             sys.MergeRecord(new VinylRecordDefinition { record_id = "archive_disc_dream_sv", display_name = "Cut Disc" });
00080:
00081:             // both resolvable — replace-all semantics were not triggered
00082:             Assert.NotNull(sys.GetRecord("pre_war_a"));
00083:             Assert.NotNull(sys.GetRecord("archive_disc_dream_sv"));
00084:
00085:             // re-cutting the same id overwrites (no duplicate), pre-war intact
00086:             sys.MergeRecord(new VinylRecordDefinition { record_id = "archive_disc_dream_sv", display_name = "Recut" });
00087:             Assert.Equal("Recut", sys.GetRecord("archive_disc_dream_sv")!.display_name);
00088:             Assert.NotNull(sys.GetRecord("pre_war_a"));
00089:
00090:             // null / empty-id merges are no-ops
00091:             sys.MergeRecord(null);
00092:             sys.MergeRecord(new VinylRecordDefinition());
00093:             Assert.NotNull(sys.GetRecord("pre_war_a"));
00094:         }
00095:     }
00096: }
```

## `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs` — 287 lines; 11,146 bytes; SHA-256 `298c3db6a1591f12e675af437475529e374983544ef7cabca0f21e698f731c94`
Declaration index:
- 00012: public sealed class SecretChoiceRecord
- 00020: public sealed class ConfessionSecretState
- 00028: public sealed class ConfessionSecretSystem
- 00055: public bool IsDiscovered(string secretId) =>
- 00058: public bool IsResolved(string secretId) =>
- 00061: public SecretChoiceRecord? GetChoice(string secretId)
- 00068: public bool DiscoverSecret(string secretId, int currentDay, string sourceId = "")
- 00080: public bool ExposeSecret(
- 00115: public bool BlackmailSecret(
- 00148: public bool KeepSecret(
- 00177: public bool ResolveInterpersonal(
- 00214: public ConfessionSecretState CaptureState()
- 00242: public void RestoreState(ConfessionSecretState state)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL Core: Confession & secret discovery and moral leverage system (Plan 21).
00003:
00004: using System;
00005: using System.Collections.Generic;
00006: #pragma warning disable CS8618
00007: using Ashfall.Core.Survivors;
00008:
00009: namespace Ashfall.Core.Phantoms
00010: {
00011:     [Serializable]
00012:     public sealed class SecretChoiceRecord
00013:     {
00014:         public string secretId = string.Empty;
00015:         public string choice = string.Empty; // "expose", "blackmail", "keep", "forgive", "grudge"
00016:         public int dayResolved;
00017:     }
00018:
00019:     [Serializable]
00020:     public sealed class ConfessionSecretState
00021:     {
00022:         public string systemId = ConfessionSecretSystem.SystemId;
00023:         public List<string> discoveredSecretIds = new List<string>();
00024:         public List<string> resolvedSecretIds = new List<string>();
00025:         public List<SecretChoiceRecord> leverageChoices = new List<SecretChoiceRecord>();
00026:     }
00027:
00028:     public sealed class ConfessionSecretSystem
00029:     {
00030:         public const string SystemId = "confession_secret_system";
00031:
00032:         private readonly ConfessionSecretCatalog _catalog;
00033:         private readonly ILog _log;
00034:         private readonly HashSet<string> _discovered = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00035:         private readonly HashSet<string> _resolved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
00036:         private readonly Dictionary<string, SecretChoiceRecord> _choices =
00037:             new Dictionary<string, SecretChoiceRecord>(StringComparer.OrdinalIgnoreCase);
00038:
00039:         public event Action<string, string>? OnSecretDiscovered; // secretId, sourceId
00040:         public event Action<string, string>? OnSecretExposed;    // secretId, factionId
00041:         public event Action<string, string>? OnSecretBlackmailed;// secretId, resourceGain
00042:         public event Action<string>? OnSecretKept;               // secretId
00043:         public event Action<string, bool>? OnConfessionResolved; // secretId, isForgiven
00044:         public event Action? OnStateChanged;
00045:
00046:         public ConfessionSecretSystem(ConfessionSecretCatalog catalog, ILog? log = null)
00047:         {
00048:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
00049:             _log = log ?? NullLog.Instance;
00050:         }
00051:
00052:         public IReadOnlyCollection<string> DiscoveredSecrets => _discovered;
00053:         public IReadOnlyCollection<string> ResolvedSecrets => _resolved;
00054:
00055:         public bool IsDiscovered(string secretId) =>
00056:             !string.IsNullOrEmpty(secretId) && _discovered.Contains(secretId);
00057:
00058:         public bool IsResolved(string secretId) =>
00059:             !string.IsNullOrEmpty(secretId) && _resolved.Contains(secretId);
00060:
00061:         public SecretChoiceRecord? GetChoice(string secretId)
00062:         {
00063:             if (string.IsNullOrEmpty(secretId)) return null;
00064:             _choices.TryGetValue(secretId, out var choice);
00065:             return choice;
00066:         }
00067:
00068:         public bool DiscoverSecret(string secretId, int currentDay, string sourceId = "")
00069:         {
00070:             if (string.IsNullOrEmpty(secretId) || !_catalog.Contains(secretId)) return false;
00071:             if (_discovered.Contains(secretId)) return false;
00072:
00073:             _discovered.Add(secretId);
00074:             _log.Info($"[Secret] Discovered {secretId} from {sourceId} on day {currentDay}");
00075:             OnSecretDiscovered?.Invoke(secretId, sourceId);
00076:             OnStateChanged?.Invoke();
00077:             return true;
00078:         }
00079:
00080:         public bool ExposeSecret(
00081:             string secretId,
00082:             int currentDay,
00083:             NeedsSystem? needs = null,
00084:             GuiltInsomniaSystem? guilt = null,
00085:             Action<string, float>? onFactionStandingChanged = null)
00086:         {
00087:             if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
00088:             var entry = _catalog.GetById(secretId);
00089:             if (entry == null) return false;
00090:
00091:             _resolved.Add(secretId);
00092:             _choices[secretId] = new SecretChoiceRecord
00093:             {
00094:                 secretId = secretId,
00095:                 choice = "expose",
00096:                 dayResolved = Math.Max(1, currentDay)
00097:             };
00098:
00099:             if (entry.expose_standing_delta != 0 && !string.IsNullOrEmpty(entry.expose_standing_faction))
00100:             {
00101:                 onFactionStandingChanged?.Invoke(entry.expose_standing_faction, entry.expose_standing_delta);
00102:             }
00103:
00104:             if (entry.expose_guilt_delta > 0 && guilt != null && !string.IsNullOrEmpty(entry.subject_id))
00105:             {
00106:                 guilt.RecordGuilt(entry.subject_id, $"secret_exposed_{secretId}", entry.expose_guilt_delta, currentDay);
00107:             }
00108:
00109:             _log.Info($"[Secret] Exposed {secretId} on day {currentDay}");
00110:             OnSecretExposed?.Invoke(secretId, entry.expose_standing_faction ?? string.Empty);
00111:             OnStateChanged?.Invoke();
00112:             return true;
00113:         }
00114:
00115:         public bool BlackmailSecret(
00116:             string secretId,
00117:             int currentDay,
00118:             MoralBranchingSystem? moral = null)
00119:         {
00120:             if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
00121:             var entry = _catalog.GetById(secretId);
00122:             if (entry == null) return false;
00123:
00124:             _resolved.Add(secretId);
00125:             _choices[secretId] = new SecretChoiceRecord
00126:             {
00127:                 secretId = secretId,
00128:                 choice = "blackmail",
00129:                 dayResolved = Math.Max(1, currentDay)
00130:             };
00131:
00132:             // Moral hardening consequence
00133:             if (entry.blackmail_hardening_delta > 0 && moral != null && !string.IsNullOrEmpty(entry.subject_id))
00134:             {
00135:                 var branchState = moral.GetState(entry.subject_id);
00136:                 if (branchState != null)
00137:                 {
00138:                     branchState.NumbedResilienceLevel = Math.Min(1.0f, branchState.NumbedResilienceLevel + entry.blackmail_hardening_delta);
00139:                 }
00140:             }
00141:
00142:             _log.Info($"[Secret] Blackmailed {secretId} on day {currentDay}");
00143:             OnSecretBlackmailed?.Invoke(secretId, entry.blackmail_resource_gain ?? string.Empty);
00144:             OnStateChanged?.Invoke();
00145:             return true;
00146:         }
00147:
00148:         public bool KeepSecret(
00149:             string secretId,
00150:             int currentDay,
00151:             SurvivorRelationsSystem? relations = null,
00152:             string confidantSurvivorId = "")
00153:         {
00154:             if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
00155:             var entry = _catalog.GetById(secretId);
00156:             if (entry == null) return false;
00157:
00158:             _resolved.Add(secretId);
00159:             _choices[secretId] = new SecretChoiceRecord
00160:             {
00161:                 secretId = secretId,
00162:                 choice = "keep",
00163:                 dayResolved = Math.Max(1, currentDay)
00164:             };
00165:
00166:             if (relations != null && !string.IsNullOrEmpty(entry.subject_id) && !string.IsNullOrEmpty(confidantSurvivorId))
00167:             {
00168:                 relations.ModifyTrust(entry.subject_id, confidantSurvivorId, entry.keep_trust_delta);
00169:             }
00170:
00171:             _log.Info($"[Secret] Kept secret {secretId} on day {currentDay}");
00172:             OnSecretKept?.Invoke(secretId);
00173:             OnStateChanged?.Invoke();
00174:             return true;
00175:         }
00176:
00177:         public bool ResolveInterpersonal(
00178:             string secretId,
00179:             int currentDay,
00180:             bool forgive,
00181:             string confessorId,
00182:             string listenerId,
00183:             SurvivorRelationsSystem? relations = null,
00184:             NeedsSystem? needs = null)
00185:         {
00186:             if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
00187:             var entry = _catalog.GetById(secretId);
00188:             if (entry == null) return false;
00189:
00190:             _resolved.Add(secretId);
00191:             _choices[secretId] = new SecretChoiceRecord
00192:             {
00193:                 secretId = secretId,
00194:                 choice = forgive ? "forgive" : "grudge",
00195:                 dayResolved = Math.Max(1, currentDay)
00196:             };
00197:
00198:             if (relations != null && !string.IsNullOrEmpty(confessorId) && !string.IsNullOrEmpty(listenerId))
00199:             {
00200:                 float affinityDelta = forgive ? entry.forgiveness_affinity : entry.grudge_affinity;
00201:                 relations.ModifyAffinity(confessorId, listenerId, affinityDelta);
00202:                 if (forgive)
00203:                 {
00204:                     relations.ModifyTrust(confessorId, listenerId, 15f);
00205:                 }
00206:             }
00207:
00208:             _log.Info($"[Secret] Resolved interpersonal confession {secretId}: forgive={forgive}");
00209:             OnConfessionResolved?.Invoke(secretId, forgive);
00210:             OnStateChanged?.Invoke();
00211:             return true;
00212:         }
00213:
00214:         public ConfessionSecretState CaptureState()
00215:         {
00216:             var state = new ConfessionSecretState { systemId = SystemId };
00217:
00218:             var discList = new List<string>(_discovered);
00219:             discList.Sort(string.CompareOrdinal);
00220:             state.discoveredSecretIds = discList;
00221:
00222:             var resList = new List<string>(_resolved);
00223:             resList.Sort(string.CompareOrdinal);
00224:             state.resolvedSecretIds = resList;
00225:
00226:             var choiceKeys = new List<string>(_choices.Keys);
00227:             choiceKeys.Sort(string.CompareOrdinal);
00228:             for (int i = 0; i < choiceKeys.Count; i++)
00229:             {
00230:                 var r = _choices[choiceKeys[i]];
00231:                 state.leverageChoices.Add(new SecretChoiceRecord
00232:                 {
00233:                     secretId = r.secretId,
00234:                     choice = r.choice,
00235:                     dayResolved = r.dayResolved
00236:                 });
00237:             }
00238:
00239:             return state;
00240:         }
00241:
00242:         public void RestoreState(ConfessionSecretState state)
00243:         {
00244:             if (state == null) return;
00245:             _discovered.Clear();
00246:             _resolved.Clear();
00247:             _choices.Clear();
00248:
00249:             if (state.discoveredSecretIds != null)
00250:             {
00251:                 for (int i = 0; i < state.discoveredSecretIds.Count; i++)
00252:                 {
00253:                     if (!string.IsNullOrEmpty(state.discoveredSecretIds[i]))
00254:                         _discovered.Add(state.discoveredSecretIds[i]);
00255:                 }
00256:             }
00257:
00258:             if (state.resolvedSecretIds != null)
00259:             {
00260:                 for (int i = 0; i < state.resolvedSecretIds.Count; i++)
00261:                 {
00262:                     if (!string.IsNullOrEmpty(state.resolvedSecretIds[i]))
00263:                         _resolved.Add(state.resolvedSecretIds[i]);
00264:                 }
00265:             }
00266:
00267:             if (state.leverageChoices != null)
00268:             {
00269:                 for (int i = 0; i < state.leverageChoices.Count; i++)
00270:                 {
00271:                     var r = state.leverageChoices[i];
00272:                     if (r != null && !string.IsNullOrEmpty(r.secretId))
00273:                     {
00274:                         _choices[r.secretId] = new SecretChoiceRecord
00275:                         {
00276:                             secretId = r.secretId,
00277:                             choice = r.choice ?? string.Empty,
00278:                             dayResolved = r.dayResolved
00279:                         };
00280:                     }
00281:                 }
00282:             }
00283:
00284:             OnStateChanged?.Invoke();
00285:         }
00286:     }
00287: }
```
# Appendix M — External verification handoff

The following checks are to be run by the owning integrator after writing: character count, SHA-256 revalidation, path-token resolution, duplicate-heading/unsupported-claim scan, and `git diff --check`. The final ledger entry must report actual results, not this template.
