# Plan 126 — Crossing Items: Twenty-Five Catalog Entries, Inventory Semantics, and Crossing Expansion Ownership

> **Rebuild status:** TERMINAL 25-ITEM CONTENT + INVENTORY/QUEST REACHABILITY AUDIT
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

The historical baseline was 4,909 characters in Git `HEAD`. The current working-tree file is being rebuilt from live source, live JSON, current ledgers, and the read-only compiled authority. Character count is verified externally after writing. The quality sequence is: premise correction → integration architecture → code-seam precision → deep polish → final reaccuracy → QA.

### Evidence labels

- **VERIFIED CURRENT:** path exists and was read in this rebase; the cited declaration, row, or hash is current at capture time.
- **HISTORICAL RECORD:** an older ledger/closeout says a package once landed; it is not a fresh test result.
- **INFERENCE:** a likely route supported by adjacent current seams; it still requires a claim and focused proof.
- **PROPOSAL:** a future design direction, not a current API.
- **UNKNOWN:** deliberately unresolved; no fallback fact is invented.

# 1. Objective

Keep the twenty-five authored Crossing items as definitions inside the current Crossing expansion while separating catalog content from canonical inventory, quest, barter, encounter, and ending ownership. The historical 11→25 data expansion is complete; the next step is a current item-reference, custody, and reachability audit—not more items or an implicit item economy.

**Bounded outcome:** Audit `crossing_items.json`, `CrossingCatalog`/`CrossingItemEntry`, Crossing quests/factions/encounters/crises, `ExpansionHostSession`, Crossing panels, inventory/item loader, and focused tests. Determine which rows are live inventory, quest tokens, trade goods, or descriptive content and document the safe custody route for any future grant.

**Non-goals:** no second inventory or Crossing economy, no arbitrary item growth, no free grant from a catalog row, no new save section, no production/data/test/UI edits in this package

# 2. Current Decision and Terminal/Residual Status

- VERIFIED CURRENT: `crossing_items.json` contains 25 items.
- VERIFIED CURRENT: `CrossingCatalog` exposes typed Crossing item lookup and loads the Crossing catalog family.
- VERIFIED CURRENT: Crossing quest/arbitration state is composed inside `ExpansionHostSession`/expansion hub.
- The current item custody/reachability of every Crossing row is an explicit premise question.
- HISTORICAL RECORD: Plan 126/Wave 41 records the 11→25 expansion; this package does not claim a fresh test run.

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

- `Assets/StreamingAssets/Data/crossing_items.json` exists at 11,588 bytes; SHA-256 `c08f6110df6d05e0fbd5aa0d3ba77f417e10f7f94cf6c7a1c546faad22501b11`.
- `Assets/StreamingAssets/Data/crossing_factions.json` exists at 6,289 bytes; SHA-256 `71f072a6dd213655a8ea3c2fbebbc617d31491ce3348166509517fe367ef27ec`.
- `Assets/StreamingAssets/Data/crossing_encounters.json` exists at 28,037 bytes; SHA-256 `02ccdbff76776a6b5653a065640e59f5c1d7ddb9e3cfe31968e89338cb31423c`.
- `Assets/StreamingAssets/Data/crossing_quests.json` exists at 34,791 bytes; SHA-256 `94d5932902ac39bdac0f2caa6c7e56a1bdb33247063e397ca9d2ac5949315734`.
- `Assets/StreamingAssets/Data/items.json` exists at 390,056 bytes; SHA-256 `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.

# 3. Required Delta

Replace the old pure-data brief with a current 25-row catalog/custody audit. Preserve Crossing quest/arbitration and canonical Inventory owners; classify metadata versus executable effects.

# 4. Current Evidence and Premise Audit

The current evidence is deliberately split into: (a) the authored catalog census in Appendix B; (b) current source declarations and bounded source snapshots in Appendix C; (c) a sampled caller graph in Appendix D; (d) current test declarations in Appendix E; and (e) the read-only authority slices in Appendix A. A declaration proves an API exists. A row proves content exists. Neither proves a live player route, a fresh passing test, or a persisted state transition.

### Premise questions answered by this rebase

Which Crossing rows are actually granted/consumed by current quests, encounters, or inventory commands?
Do current item types/effects match the canonical ItemDefinition contract?
Does expansion-hub persistence include only mutable Crossing state and not duplicate physical items?
Which panels expose a real command versus a descriptive row?

# 5. Existing Extension Seams

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| Crossing item definitions | `CrossingCatalogLoader / CrossingItemEntry` | `Assets/Ashfall.Core/CrossingCatalog.cs` | Owns typed Crossing item rows and lookup only. |
| Crossing quest/choice state | `CrossingQuestSystem` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | Owns Crossing quest lifecycle; item row is not a quest. |
| inventory/custody | `Inventory / ItemCatalogLoader` | `Assets/Ashfall.Core/Inventory/; Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | Owns physical item definitions, grants, and consumption. |
| Crossing host/persistence | `ExpansionHostSession / ExpansionHub` | `src/Host/ExpansionHostSession.cs; src/Main.ExpansionHub.cs` | Composes Crossing state inside the existing expansion hub. |
| Crossing UI | `CrossingQuestPanel / CrossingSafeConductVouchPanel` | `src/UI/CrossingQuestPanel.cs; src/UI/CrossingSafeConductVouchPanel.cs` | Presentation and existing commands only. |

The implementation rule is **EXTEND → ADAPT → PROJECT → VERIFY**. Do not create a second catalog, owner, RNG stream, save section, panel cache, or narrative ledger for Crossing item catalog.

# 6. Proposed Architecture

```text
Authored JSON / current owner state
              │
              ▼
┌──────────────────────────────────────────────────────────────┐
│ Crossing Items: Twenty-Five Catalog Entries, Inventory Semantics, and Crossing Expansion Ownership                                               │
│ Integration route: DATA-ONLY + current inventory/quest custody audit                             │
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

1. **Crossing rows are definitions.**
2. **Quest system owns Crossing lifecycle.**
3. **Inventory owns custody.**
4. **Expansion hub owns existing mutable state.**
5. **Panels never grant items.**

# 7. Ownership Matrix

| Concern | Sole current owner | Current path(s) | Boundary |
|---|---|---|---|
| Crossing item definitions | `CrossingCatalogLoader / CrossingItemEntry` | `Assets/Ashfall.Core/CrossingCatalog.cs` | Owns typed Crossing item rows and lookup only. |
| Crossing quest/choice state | `CrossingQuestSystem` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | Owns Crossing quest lifecycle; item row is not a quest. |
| inventory/custody | `Inventory / ItemCatalogLoader` | `Assets/Ashfall.Core/Inventory/; Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | Owns physical item definitions, grants, and consumption. |
| Crossing host/persistence | `ExpansionHostSession / ExpansionHub` | `src/Host/ExpansionHostSession.cs; src/Main.ExpansionHub.cs` | Composes Crossing state inside the existing expansion hub. |
| Crossing UI | `CrossingQuestPanel / CrossingSafeConductVouchPanel` | `src/UI/CrossingQuestPanel.cs; src/UI/CrossingSafeConductVouchPanel.cs` | Presentation and existing commands only. |

**Single-owner test:** before any future change, search for another mutable collection, catalog copy, save field, event producer, or UI cache claiming the same concern. A duplicate is a blocker or an explicit projection, never a convenience authority.

# 8. Data Flow

1. load Crossing item definitions through CrossingCatalogLoader
2. resolve item type/weight/value/effects against the current item vocabulary
3. let a Crossing quest/encounter request a typed item id
4. route the grant through canonical Inventory/ItemCatalogLoader
5. charge/consume through the canonical inventory owner
6. project current Crossing state in the existing panels and expansion-hub save

Every arrow is one-way for authority. A presenter may call a command, but the resulting state must return through the owner mutation/event. No view-local “temporary truth” may become a save fact.

# 9. State Model and Invariants

- Crossing item ids are unique and stable
- item types and numeric fields use the current inventory contract
- a catalog row does not create physical custody
- quest/encounter references resolve to current item/quest ids
- grant and consumption are atomic through Inventory
- a Trading/Quest item cannot silently become a medical effect without a current consumer
- expansion-hub restore preserves current quest/arbitration state

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

Contract rules for Crossing item catalog:

- Refusal is named and stable; no silent default success.
- Unknown ids remain unknown or are rejected with a diagnostic, according to the current loader contract.
- Preview and execute use the same gate calculation; UI cannot bypass a prerequisite.
- Events are emitted after the owning mutation commits and before presentation refresh.
- Any repeated event has an explicit idempotency key or a documented at-most-once policy.

# 11. Data Plan and Catalog Authority

`crossing_items.json` remains the Crossing item-definition authority with 25 rows. Audit every `type`, `weight`, `tradeValue`, restoration field, and description against the current `ItemDefinition`/Crossing consumers. The original plan’s “item effects” are not automatically inventory effects; a future grant must use the current item/inventory contract.

The JSON data authority remains under `Assets/StreamingAssets/Data/`. A future row requires a schema/version decision, stable id, bounded fields, a named consumer, validation, continuity review, and a focused test. Text must describe modeled state and must not invent mechanics.

# 12. Save, Restore, and Migration

Crossing definition rows are static. Mutable Crossing quest/arbitration state belongs to the existing expansion-hub owner and its current codec. Do not add a `crossing_items` save section or mirror inventory. A future physical item grant uses canonical Inventory persistence.

**Save proof matrix:** current owner state → deep capture → serialize → restore to a fresh instance → continue the same action sequence → compare state, ordering, and checksum/fingerprint. A catalog test or snapshot does not substitute for this matrix. Legacy input must produce the documented neutral/default state, never an invented favorable outcome.

# 13. Determinism and Replay

Crossing definitions and lookup ordering are ordinal-stable. Quest/choice resolution uses existing seeded owner state; item grant/consumption is not a random operation. Paired replay compares quest state, inventory deltas, crossing access, and events.

**Replay proof:** same seed, catalog version, command sequence, and save fixture produce the same ordered ids, events, state transitions, and visible projection. If a new random decision is genuinely required, use an existing seeded stream or a deliberately forked `CampaignRngManager` stream; never use wall-clock time, hash iteration order, or `System.Random` in deterministic Core behavior.

# 14. System and Event Wiring

Crossing quests/encounters emit their current facts; the host routes item rewards through Inventory and journal/ending consequences through canonical owners. A row’s trade value is metadata until a market owner consumes it. No panel creates a physical item.

**Event ordering:** owner mutation → canonical fact/event → host consumer → UI projection → dirty-save flush. A host adapter may translate an owner fact into a canonical consequence only through the owning system’s existing API. Optional presentation may be absent; it may not fabricate a live command.

# 15. Godot Host Integration

**Current host surfaces:**

- `src/Host/ExpansionHostSession.cs` — composes Crossing catalog, quests, arbitration, and expansion persistence
- `src/Main.ExpansionHub.cs` — routes the Crossing surface and save handoff
- `src/UI/CrossingQuestPanel.cs` — renders current quest/choice state
- `src/UI/CrossingSafeConductVouchPanel.cs` — renders current vouch/access state
- `src/Main.PlayerSurfaces.cs` — route/lifecycle integration

The Godot layer is limited to composition, input, routing, binding, refresh, accessibility, audio/visual presentation, and lifecycle cleanup. Shared `Main`/panel/save composition roots are integrator-owned and must be claimed exactly before an implementation change.

**UI truth contract:** show the current owner’s value, source, availability, refusal, and next consequence. Use text/icon/shape in addition to color. Preserve close/back, focus traversal, controller navigation, reduced motion, and truthful empty/loading/error states.

# 16. Narrative and Content Integration

Crossing items should feel like records, tokens, tools, and trade goods inside the existing fictional settlement. Descriptions may explain custody or scarcity, but cannot grant a good, bypass a checkpoint, or declare a faction relationship that the owner has not recorded.

Content must remain fictional, restrained, human, and grounded in the actual model. A record may describe an event only if the event system can produce it. Do not use prose to smuggle in a new resource, faction, casualty, relationship, or ending.

# 17. Failure Modes and Negative Contracts

# Appendix F — Scenario and negative-contract matrix

Each row is a required review question for a future owner. A negative result must fail closed, remain visible, and never fabricate a replacement authority.
| ID | Condition | Safe response | Evidence gate |
|---|---|---|---|

# 18. Test Strategy

The implementation owner should run the smallest target first, then only directly affected regional tests. The planning package does not claim these commands were freshly executed.

### Focused Core/data targets

1. `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs`

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
| Phase 0 — item/consumer census | read 25 rows, Crossing DTOs, inventory loader, quests, encounters, and tests | all fields and current references are explicit | no undocumented scope or shortcut |
| Phase 1 — custody/reachability trace | follow quest/encounter → grant → inventory → panel | live, dormant, and conflicting rows are classified | no undocumented scope or shortcut |
| Phase 2 — save/determinism/accessibility audit | check expansion hub, Inventory, empty/error states, and stable ordering | no shadow inventory or misleading affordance | no undocumented scope or shortcut |
| Phase 3 — bounded residual | only a proven item/consumer defect is promoted | one owner and focused tests | no undocumented scope or shortcut |

**First safe implementation step:** Phase 0 is a read-only current census. No phase starts by creating a type named only in the historical baseline. If the owner, save path, loader schema, or event seam differs from this plan, return `STALE_PLAN` and update the claim.

# 20. File Impact Map

| Path/area | Action in this planning package | Future implementation disposition |
|---|---|---|
| `Assets/StreamingAssets/Data/crossing_items.json` | READ ONLY; MODIFY only for a proven reference/consumer defect | retain as Crossing item authority |
| `Assets/Ashfall.Core/CrossingCatalog.cs` | READ ONLY | typed Crossing catalog |
| `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | READ ONLY | canonical item contract |
| `src/Host/ExpansionHostSession.cs` | READ ONLY | existing Crossing host/persistence |
| `src/UI/CrossingQuestPanel.cs` | READ ONLY | truthful Crossing presentation |

Any path not listed is out of scope for this plan. A newly discovered path is a finding with an owner and evidence, not an invitation to widen the package.

# 21. Risks and Mitigations

| Risk | Control / stop condition |
|---|---|
| parallel inventory | route all custody through Inventory |
| free rewards from static rows | use a typed grant command |
| reference drift | run strict data/reference tests |
| expansion save collision | use existing expansion hub only |

# 22. Explicit Non-Goals

- no second inventory or Crossing economy, no arbitrary item growth, no free grant from a catalog row, no new save section, no production/data/test/UI edits in this package

# 23. Rollback and Recovery

- This planning-only change is reversible by restoring the prior version of the exact plan path; no runtime rollback is required because no production, data, test, UI, save, or generated-index file is changed here.
- A future implementation must keep the prior valid owner state and catalog schema available until its focused migration/round-trip target passes.
- If a new owner, codec, event seam, or shared composition root is required, stop and return `STALE_PLAN`/a decision packet rather than improvising a rollback for a parallel architecture.
- For a future data change, retain the prior valid JSON fixture and document whether recovery is a revert, additive default, or explicit migration. Never silently down-convert a newer state.

# 24. Definition of Done

- The current owner, data authority, host/UI boundary, save owner, determinism rule, and failure contracts for Crossing item catalog are named from current evidence.
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

- A current 25-row item-to-type/consumer/custody matrix.
- An explicit distinction between authored metadata and executable effects.
- A bounded residual only for a proven inventory/quest/catalog gap.

## MUST NOT DO

- create a second item/inventory store
- grant all Crossing items at boot
- turn tradeValue into an unowned price formula
- add a static-item save section

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

Phase 0: read CrossingCatalog, CrossingItemEntry, crossing_items.json, ItemCatalogLoader, CrossingQuestSystem, ExpansionHostSession, and focused tests; map every row to current consumers.

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

### Authority lines 716–721
00716: **A-25 · C10 · Massive-expansion corpus prose audit.** Subject: a prose-depth audit of `quests_massive_expansion_200.json` (200 records — the largest single prose debt surface in the data authority), converting skeleton records into contracted fields over several tranches. Evidence: catalog verified live; scale is structural evidence of thin per-record prose. Route: DATA-ONLY, multi-tranche. Confidence: HIGH CONFIDENCE.
00717:
00718: **A-26 · C11 · Ledger-debt statement prose.** Subject: debtor statements and collection notices for `ledger_debt_templates.json` rows. Evidence: catalog verified live; `LedgerDebtSystem` with consequence dispatchers is canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00719:
00720: **A-27 · C12 · Storm-window almanac entries.** Subject: almanac prose conditioned on `year_of_ash_storm_windows.json` entries. Evidence: catalog verified live; `weather_almanac_expansion` exists in the corpus. Route: DATA-ONLY, within the Year-of-Ash window (180–360) canon. Confidence: HIGH CONFIDENCE.
00721:

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

### Authority lines 951–960
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
00952:
00953: **DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.
00954:
00955: **DM-12 — Weather and Year of Ash (C12).** Owners: weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash family (events/items/locations/questlines/quests/radio/survivors/storm windows). Live catalogs: all of the above verified live. Hosts: WeatherHardening, YearOfAsh widgets, WeatherStationSystem. Openings: A-27, B-03, B-19, C-09, E-09, F-02, G-07, plus the F-002 campaign. Constraint: tick window 180–360 canon.
00956:
00957: **DM-13 — Endgame and epilogue (C13).** Owners: Reckoning, verdict ending evaluator, epilogue matrix runtime, epilogue chronicle, standing records, census, muster epilogues, holdfast endings. Live catalogs: `endings`, `campaign_epilogues`, `epilogue_chronicle`, `verdict_data/items/locations/npcs/questlines/radio`, `standing_record_factions/layouts/memory/quests`, `muster_epilogues`. Hosts: Endgame, Verdict, StandingRecord. Openings: A-28, B-20, D-03, E-10, F-03, G-04, plus the F-005 campaign. Constraint: main ending cannot be invalidated by optional content.
00958:
00959: **DM-14 — Ecology and wildlife (C14).** Owners: migration, trapping, ecosystem, seasonal calendar, bestiary, underground flora, infestations, contagion, pathogens, crop genomes. Live catalogs: `wildlife_ecosystem`, `wildlife_trapping_catalog`, `wasteland_wildlife_bestiary`, `underground_flora`, `ecological_infestations`, `contagion_events`, `pathogens`, `crop_strains`, `mutations`. Hosts: WildlifeEcosystem, WildlifeTrapping. Openings: A-29, B-21, C-10, plus the F-002 blight arc. Constraint: zoonosis bridge and campfire sanitization are the owned seams.
00960:

### Authority lines 4905–4910
04905:
04906: - The incident surface's classification as Case D is a seal the factory respects: no scheduler, no consequence dispatch, no history schema except through a follow-on plan the foreman requests (Q-3 of Volume 41).
04907: - **NEW SEED — A-42 · C1 · Incident prose depth continuation.** Within the sealed surface's own terms (the five DTO fields), incident prose (`bodyText`) is the authorable surface: a census of the 25 entries' depth against the incident genre's contract, then authored tranches if the census warrants. Route: DATA-ONLY. Verification: the Plan 57 test file (12 contract tests, verified) extended per tranche; `data_integrity` gate. Confidence: HIGH CONFIDENCE (surface verified; depth unmeasured).
04908:
04909: ## 43.4 Registration consequences for F-007
04910:

# Appendix B — Current authored-data census and row audit

# Appendix B — Current authored-data census and row audit

The JSON files below are the current authored authorities. Row summaries are generated from the current files; no row is treated as reachable merely because it parses.

## `Assets/StreamingAssets/Data/crossing_items.json`
- Bytes: 11,588; SHA-256: `c08f6110df6d05e0fbd5aa0d3ba77f417e10f7f94cf6c7a1c546faad22501b11`
- Root keys: `items, schema_version`
- `items`: list[25]; union fields: `description, displayName, healthEffect, hungerRestore, id, moraleEffect, stackMax, thirstRestore, tradeValue, type, weight`
  - row 1: `{"description":"A matchbook-sized slip with a name written bluntly on it. It is not a document of authority; it is someone's name, offered as a word for you. The Crossing trusts the name, not the slip.","displayName":"Vouch Token","hungerRestore":0,"id":"item_vouch_token_crossing","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":50,"type":"Quest","weight":0.1}`
  - row 2: `{"description":"A cast iron slug of exact mass, kept because keeping it exact makes everything else arguable in a straight line. The scale never lies; whoever holds the weight is reminded that lying is what human beings do instead.","displayName":"Calibration Weight","hungerRestore":0,"id":"item_calibration_weight","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":80,"type":"Tool","weight":2.0}`
  - row 3: `{"description":"Sacked grain that moved through Stallrow's scale, weighed honest and priced to match. Somewhere upstream an acre of it once grew; now it just feeds people who got a fair number on it.","displayName":"Crossing Grain","hungerRestore":0,"id":"item_crossing_traded_grain","moraleEffect":0,"stackMax":10,"thirstRestore":0,"tradeValue":30,"type":"Trade","weight":12}`
  - row 4: `{"description":"Coarse salt in a waxed sack, one of the few things the Drown yields that keeps. The price on it never drifts far from the scale's word, because too many people weigh it on the way back out.","displayName":"Crossing Salt","hungerRestore":0,"id":"item_crossing_traded_salt","moraleEffect":0,"stackMax":8,"thirstRestore":0,"tradeValue":22,"type":"Trade","weight":3}`
  - row 5: `{"description":"A debt recorded as a document. It names the principal, the term, and the forfeit in plain words, and it is read twice before anyone signs — the second time out loud, on the record.","displayName":"Pledge Slip","hungerRestore":0,"id":"item_crossing_pledge_slip","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":5,"type":"Quest","weight":0.1}`
  - row 6: `{"description":"The original three leaves of the Crossing Charter, pulled from the records room iron box. Not a grand constitution — just a practical list of who promised to weigh honest, who backed them, and who signed their names.","displayName":"Three Dry Pages","hungerRestore":0,"id":"item_charter_three_pages","moraleEffect":10,"stackMax":1,"thirstRestore":0,"tradeValue":100,"type":"Quest","weight":0.1}`
  - row 7: `{"description":"A carbon copy of the Underwrite pledge. The ink is clear, the rate is fixed, and the forfeit is written on the front line where anyone can read it.","displayName":"Debt Contract Copy","hungerRestore":0,"id":"item_debt_contract_copy","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":10,"type":"Quest","weight":0.1}`
  - row 8: `{"description":"Charcoal on wax paper taken from the Nightfire bronze marker. The names of the founding weighmasters are legible, but the lower section fades into corroded pit-marks.","displayName":"Marker Rubbing","hungerRestore":0,"id":"item_marker_rubbing","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":15,"type":"Quest","weight":0.1}`
  - row 9: `{"description":"A charred ledger page from the pre-Charter era documenting an axle-weight dispute at the viaduct approach. It proves why the first rules were drafted.","displayName":"Duty Log Fragment","hungerRestore":0,"id":"item_duty_log_fragment","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":25,"type":"Quest","weight":0.1}`
  - row 10: `{"description":"An official three-part manifest slip from the Scalehouse depot used to clear weighed cargo across the viaduct.","displayName":"Blank Trade Manifest","hungerRestore":0,"id":"item_trade_manifest_blank","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":12,"type":"Tool","weight":0.2}`
  - row 11: `{"description":"A receipt signed with Dessa Vane's seal confirming Wyn Sabler's grain obligation was satisfied in full without asset seizure.","displayName":"Fulfilled Forfeit Receipt","hungerRestore":0,"id":"item_wyn_receipt_paid","moraleEffect":5,"stackMax":1,"thirstRestore":0,"tradeValue":5,"type":"Quest","weight":0.1}`
  - row 12: `{"description":"A numbered token from the Crossing arbitration desk. It puts a dispute on the day's board; it does not promise which way the board will turn.","displayName":"Arbitration Token","hungerRestore":0,"id":"item_arbitration_token","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":12,"type":"Quest","weight":0.05}`
  - row 13: `{"description":"A heavy little stamp cut with one of the Crossing's accepted charter marks. The impression matters only when the ledger copy and the issuing desk agree.","displayName":"Charter Stamp","hungerRestore":0,"id":"item_charter_stamp","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":18,"type":"Quest","weight":0.05}`
  - row 14: `{"description":"A numbered slip issued after cargo crosses the Deck Scale. It records what the scales said, which is enough to start an argument with a cleaner edge.","displayName":"Weighbridge Chit","hungerRestore":0,"id":"item_weighbridge_chit","moraleEffect":0,"stackMax":10,"thirstRestore":0,"tradeValue":6,"type":"Trade","weight":0.01}`
  - row 15: `{"description":"A sealed course of clinic stock with the public label scraped away and a second price written underneath. Nobody asks where it came from while the seal is intact; the people who need it most rarely set the off-ledger rate.","displayName":"Off-Ledger Medicine","healthEffect":20,"hungerRestore":0,"id":"item_smuggled_medicine","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":24,"type":"Medical","weight":0.1}`
  - row 16: `{"description":"A dense loaf from the communal ovens, baked dark enough that nobody asks how much husk went into the flour. It keeps well, cuts badly, and tastes the same whether bought with coin, labor, or a receipt.","displayName":"Granary Bread","hungerRestore":22,"id":"item_crossing_bread","moraleEffect":1,"stackMax":15,"thirstRestore":0,"tradeValue":8,"type":"Food","weight":0.2}`
  - row 17: `{"description":"A measured bottle of lamp oil from the Crossing night market, each batch marked with the seller's scratch code. Good oil burns clean enough to read by; the market certifies only the measure.","displayName":"Crossing Lamp Oil","hungerRestore":0,"id":"item_lamp_oil_crossing","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":6,"type":"Fuel","weight":1.0}`
  - row 18: `{"description":"Water drawn through the Crossing's filtration line and sealed with a paper strip carrying the day's batch number. The strip proves where it passed before sale, not that tomorrow's committee will honor today's mark.","displayName":"Committee Water","hungerRestore":0,"id":"item_filtered_water_crossing","moraleEffect":0,"stackMax":5,"thirstRestore":40,"tradeValue":12,"type":"Water","weight":1.0}`
  - row 19: `{"description":"Paper-and-cloth bands issued at the quarantine gate after a screening. Their color belongs to the day's rulebook, so yesterday's band is a claim until the clerk decides otherwise.","displayName":"Quarantine Bands","hungerRestore":0,"id":"item_quarantine_bands","moraleEffect":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Quest","weight":0.01}`
  - row 20: `{"description":"A receipt for grain held under a household, crew, or faction mark in the communal granary. Inside the Crossing it is a claim; outside it is only paper with someone else's initials.","displayName":"Granary Receipt","hungerRestore":0,"id":"item_granary_receipt","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":8,"type":"Trade","weight":0.01}`
  - row 21: `{"description":"A narrow ledger written in abbreviated weights, initials, and route marks that never appear in the official books. Half the entries need a key; the other half are dangerous because they do not.","displayName":"Smuggler's Ledger","hungerRestore":0,"id":"item_smugglers_ledger","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":15,"type":"Quest","weight":0.1}`
  - row 22: `{"description":"An official notice stating that a claim was heard and rejected, stamped hard enough to emboss the paper beneath it. It cannot reopen the case, but it proves the dispute existed.","displayName":"Arbitration Rejection Notice","hungerRestore":0,"id":"item_rejection_notice","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":3,"type":"Quest","weight":0.01}`
  - row 23: `{"description":"A hand-corrected map of service lanes, blocked alleys, weighing yards, and committee doors. Some routes are legal only at certain hours, which is why the useful marks are in pencil.","displayName":"Crossing Back-Route Map","hungerRestore":0,"id":"item_crossing_map","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":12,"type":"Quest","weight":0.05}`
  - row 24: `{"description":"A waxed pouch with a false inner seam and no maker's mark. The Crossing uses it for documents, medicine, and anything whose official weight is meant to stay uncertain.","displayName":"Off-Ledger Pouch","hungerRestore":0,"id":"item_black_market_pouch","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":8,"type":"Trade","weight":0.2}`
  - row 25: `{"description":"A marked draft of a proposed charter amendment, dense with crossed-out clauses and signatures that stop just short of commitment. Everyone named in the margins denies the paper is current; three factions still want to read it.","displayName":"Charter Amendment Draft","hungerRestore":0,"id":"item_charter_draft","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":20,"type":"Quest","weight":0.05}`
- Bytes: 11,588; SHA-256: `c08f6110df6d05e0fbd5aa0d3ba77f417e10f7f94cf6c7a1c546faad22501b11`
- Root keys: `items, schema_version`
- `items`: list[25]; union fields: `description, displayName, healthEffect, hungerRestore, id, moraleEffect, stackMax, thirstRestore, tradeValue, type, weight`
  - row 1: `{"description":"A matchbook-sized slip with a name written bluntly on it. It is not a document of authority; it is someone's name, offered as a word for you. The Crossing trusts the name, not the slip.","displayName":"Vouch Token","hungerRestore":0,"id":"item_vouch_token_crossing","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":50,"type":"Quest","weight":0.1}`
  - row 2: `{"description":"A cast iron slug of exact mass, kept because keeping it exact makes everything else arguable in a straight line. The scale never lies; whoever holds the weight is reminded that lying is what human beings do instead.","displayName":"Calibration Weight","hungerRestore":0,"id":"item_calibration_weight","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":80,"type":"Tool","weight":2.0}`
  - row 3: `{"description":"Sacked grain that moved through Stallrow's scale, weighed honest and priced to match. Somewhere upstream an acre of it once grew; now it just feeds people who got a fair number on it.","displayName":"Crossing Grain","hungerRestore":0,"id":"item_crossing_traded_grain","moraleEffect":0,"stackMax":10,"thirstRestore":0,"tradeValue":30,"type":"Trade","weight":12}`
  - row 4: `{"description":"Coarse salt in a waxed sack, one of the few things the Drown yields that keeps. The price on it never drifts far from the scale's word, because too many people weigh it on the way back out.","displayName":"Crossing Salt","hungerRestore":0,"id":"item_crossing_traded_salt","moraleEffect":0,"stackMax":8,"thirstRestore":0,"tradeValue":22,"type":"Trade","weight":3}`
  - row 5: `{"description":"A debt recorded as a document. It names the principal, the term, and the forfeit in plain words, and it is read twice before anyone signs — the second time out loud, on the record.","displayName":"Pledge Slip","hungerRestore":0,"id":"item_crossing_pledge_slip","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":5,"type":"Quest","weight":0.1}`
  - row 6: `{"description":"The original three leaves of the Crossing Charter, pulled from the records room iron box. Not a grand constitution — just a practical list of who promised to weigh honest, who backed them, and who signed their names.","displayName":"Three Dry Pages","hungerRestore":0,"id":"item_charter_three_pages","moraleEffect":10,"stackMax":1,"thirstRestore":0,"tradeValue":100,"type":"Quest","weight":0.1}`
  - row 7: `{"description":"A carbon copy of the Underwrite pledge. The ink is clear, the rate is fixed, and the forfeit is written on the front line where anyone can read it.","displayName":"Debt Contract Copy","hungerRestore":0,"id":"item_debt_contract_copy","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":10,"type":"Quest","weight":0.1}`
  - row 8: `{"description":"Charcoal on wax paper taken from the Nightfire bronze marker. The names of the founding weighmasters are legible, but the lower section fades into corroded pit-marks.","displayName":"Marker Rubbing","hungerRestore":0,"id":"item_marker_rubbing","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":15,"type":"Quest","weight":0.1}`
  - row 9: `{"description":"A charred ledger page from the pre-Charter era documenting an axle-weight dispute at the viaduct approach. It proves why the first rules were drafted.","displayName":"Duty Log Fragment","hungerRestore":0,"id":"item_duty_log_fragment","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":25,"type":"Quest","weight":0.1}`
  - row 10: `{"description":"An official three-part manifest slip from the Scalehouse depot used to clear weighed cargo across the viaduct.","displayName":"Blank Trade Manifest","hungerRestore":0,"id":"item_trade_manifest_blank","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":12,"type":"Tool","weight":0.2}`
  - row 11: `{"description":"A receipt signed with Dessa Vane's seal confirming Wyn Sabler's grain obligation was satisfied in full without asset seizure.","displayName":"Fulfilled Forfeit Receipt","hungerRestore":0,"id":"item_wyn_receipt_paid","moraleEffect":5,"stackMax":1,"thirstRestore":0,"tradeValue":5,"type":"Quest","weight":0.1}`
  - row 12: `{"description":"A numbered token from the Crossing arbitration desk. It puts a dispute on the day's board; it does not promise which way the board will turn.","displayName":"Arbitration Token","hungerRestore":0,"id":"item_arbitration_token","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":12,"type":"Quest","weight":0.05}`
  - row 13: `{"description":"A heavy little stamp cut with one of the Crossing's accepted charter marks. The impression matters only when the ledger copy and the issuing desk agree.","displayName":"Charter Stamp","hungerRestore":0,"id":"item_charter_stamp","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":18,"type":"Quest","weight":0.05}`
  - row 14: `{"description":"A numbered slip issued after cargo crosses the Deck Scale. It records what the scales said, which is enough to start an argument with a cleaner edge.","displayName":"Weighbridge Chit","hungerRestore":0,"id":"item_weighbridge_chit","moraleEffect":0,"stackMax":10,"thirstRestore":0,"tradeValue":6,"type":"Trade","weight":0.01}`
  - row 15: `{"description":"A sealed course of clinic stock with the public label scraped away and a second price written underneath. Nobody asks where it came from while the seal is intact; the people who need it most rarely set the off-ledger rate.","displayName":"Off-Ledger Medicine","healthEffect":20,"hungerRestore":0,"id":"item_smuggled_medicine","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":24,"type":"Medical","weight":0.1}`
  - row 16: `{"description":"A dense loaf from the communal ovens, baked dark enough that nobody asks how much husk went into the flour. It keeps well, cuts badly, and tastes the same whether bought with coin, labor, or a receipt.","displayName":"Granary Bread","hungerRestore":22,"id":"item_crossing_bread","moraleEffect":1,"stackMax":15,"thirstRestore":0,"tradeValue":8,"type":"Food","weight":0.2}`
  - row 17: `{"description":"A measured bottle of lamp oil from the Crossing night market, each batch marked with the seller's scratch code. Good oil burns clean enough to read by; the market certifies only the measure.","displayName":"Crossing Lamp Oil","hungerRestore":0,"id":"item_lamp_oil_crossing","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":6,"type":"Fuel","weight":1.0}`
  - row 18: `{"description":"Water drawn through the Crossing's filtration line and sealed with a paper strip carrying the day's batch number. The strip proves where it passed before sale, not that tomorrow's committee will honor today's mark.","displayName":"Committee Water","hungerRestore":0,"id":"item_filtered_water_crossing","moraleEffect":0,"stackMax":5,"thirstRestore":40,"tradeValue":12,"type":"Water","weight":1.0}`
  - row 19: `{"description":"Paper-and-cloth bands issued at the quarantine gate after a screening. Their color belongs to the day's rulebook, so yesterday's band is a claim until the clerk decides otherwise.","displayName":"Quarantine Bands","hungerRestore":0,"id":"item_quarantine_bands","moraleEffect":0,"stackMax":10,"thirstRestore":0,"tradeValue":4,"type":"Quest","weight":0.01}`
  - row 20: `{"description":"A receipt for grain held under a household, crew, or faction mark in the communal granary. Inside the Crossing it is a claim; outside it is only paper with someone else's initials.","displayName":"Granary Receipt","hungerRestore":0,"id":"item_granary_receipt","moraleEffect":0,"stackMax":5,"thirstRestore":0,"tradeValue":8,"type":"Trade","weight":0.01}`
  - row 21: `{"description":"A narrow ledger written in abbreviated weights, initials, and route marks that never appear in the official books. Half the entries need a key; the other half are dangerous because they do not.","displayName":"Smuggler's Ledger","hungerRestore":0,"id":"item_smugglers_ledger","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":15,"type":"Quest","weight":0.1}`
  - row 22: `{"description":"An official notice stating that a claim was heard and rejected, stamped hard enough to emboss the paper beneath it. It cannot reopen the case, but it proves the dispute existed.","displayName":"Arbitration Rejection Notice","hungerRestore":0,"id":"item_rejection_notice","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":3,"type":"Quest","weight":0.01}`
  - row 23: `{"description":"A hand-corrected map of service lanes, blocked alleys, weighing yards, and committee doors. Some routes are legal only at certain hours, which is why the useful marks are in pencil.","displayName":"Crossing Back-Route Map","hungerRestore":0,"id":"item_crossing_map","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":12,"type":"Quest","weight":0.05}`
  - row 24: `{"description":"A waxed pouch with a false inner seam and no maker's mark. The Crossing uses it for documents, medicine, and anything whose official weight is meant to stay uncertain.","displayName":"Off-Ledger Pouch","hungerRestore":0,"id":"item_black_market_pouch","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":8,"type":"Trade","weight":0.2}`
  - row 25: `{"description":"A marked draft of a proposed charter amendment, dense with crossed-out clauses and signatures that stop just short of commitment. Everyone named in the margins denies the paper is current; three factions still want to read it.","displayName":"Charter Amendment Draft","hungerRestore":0,"id":"item_charter_draft","moraleEffect":0,"stackMax":1,"thirstRestore":0,"tradeValue":20,"type":"Quest","weight":0.05}`

## `Assets/StreamingAssets/Data/crossing_factions.json`
- Bytes: 6,289; SHA-256: `71f072a6dd213655a8ea3c2fbebbc617d31491ce3348166509517fe367ef27ec`
- Root keys: `actions, schema_version`
- `actions`: list[8]; union fields: `access_rule, alignment, badge_asset_id, display_name, home_region, id, is_active, offers, signature_quote, trust, wants`
  - row 1: `{"access_rule":"An agonizingly precise weigh-in is the price of doing business at Stallrow. Contest a true reading without cause, and the market stays open—but your name goes on the slate.","alignment":"conditional","badge_asset_id":"","display_name":"The Scale","home_region":"region_crossing","id":"faction_the_scale","is_active":true,"offers":["stallrow_trade_access","verification"],"signature_quote":"The brass scales do not lie, and the numbers have no conscience. What people infer from their poverty is not my problem.","trust":0,"wants":["trade_goods"]}`
  - row 2: `{"access_rule":"Their 'help' is genuine, offered against a plainly named, brutal forfeit—a child's labor, a pound of flesh, a year of servitude. Sign, negotiate, or walk away; no hidden clause survives a second reading.","alignment":"conditional","badge_asset_id":"","display_name":"The Underwrite","home_region":"region_crossing","id":"faction_the_underwrite","is_active":true,"offers":["seed_stock","covered_loss","favour_bank"],"signature_quote":"Read it twice, under the sodium glare. I'll say it twice. After the second time, there is only the ink, and the debt it binds you to.","trust":0,"wants":["pledged_goods"]}`
  - row 3: `{"access_rule":"Sign the blood-flecked draft and you are on the record. Perrin will not ratify a clause he knows will break a man—but he has not yet seen every way the words can be twisted.","alignment":"peaceful","badge_asset_id":"","display_name":"The Compact","home_region":"region_crossing","id":"faction_the_compact","is_active":true,"offers":["charter_draft","ratification"],"signature_quote":"There is a document now, stained with ash and thumbprints. Let them argue with the paper instead of each other's throats.","trust":0,"wants":["signatories"]}`
  - row 4: `{"access_rule":"Contribute fuel stores or clean reflector glass to the maintenance pool to ensure uninterrupted sector lighting; tampering with street lanterns or siphoning communal oil causes street lamps in your quarter to go dark.","alignment":"conditional","badge_asset_id":"","display_name":"The Lamplighters","home_region":"region_crossing","id":"faction_the_lamplighters","is_active":true,"offers":["street_lighting","night_watch_escort","route_visibility"],"signature_quote":"A dark street is an argument someone decided to settle with iron; keep the wicks trimmed and the town remembers what it agreed to.","trust":0,"wants":["fuel_stores",…`
  - row 5: `{"access_rule":"Honor minuted ration allotments and return dry grain sacks intact; unauthorized withdrawals or concealing spoilage revokes access to the communal reserve.","alignment":"conditional","badge_asset_id":"","display_name":"The Granary Wardens","home_region":"region_crossing","id":"faction_the_granary_wardens","is_active":true,"offers":["ration_distribution","emergency_grain_draw","spoilage_inspection"],"signature_quote":"The lock on the grain crib is not to keep hunger out; it is to ensure there is still seed in the hopper ninety days from now.","trust":0,"wants":["staple_grain","burlap_sacks"]}`
  - row 6: `{"access_rule":"Submit all bulk draw containers for sediment inspection and contribute clean filter charcoal or chlorine titration chits; drawing from closed filtration beds voids communal tap rights.","alignment":"conditional","badge_asset_id":"","display_name":"The Water Committee","home_region":"region_crossing","id":"faction_the_water_committee","is_active":true,"offers":["clean_water_rights","well_head_access","quality_certification"],"signature_quote":"You can argue with the charter all afternoon, but the sand filter does not care about your politics—run tainted water through it and everybody drinks poison.","trust":0,"wants":["filter_…`
  - row 7: `{"access_rule":"Submit to mandatory forty-eight-hour isolation during outbreak declarations and declare pulmonary symptoms; forging health clearance slips or breaking containment cordons results in immediate gate expulsion.","alignment":"neutral","badge_asset_id":"","display_name":"The Quarantine Post","home_region":"region_crossing","id":"faction_the_quarantine_post","is_active":true,"offers":["gate_health_screening","quarantine_clearance","outbreak_warning"],"signature_quote":"We do not weigh your goods or examine your coins; we look at the sweat on your collar and the rattling in your lungs.","trust":0,"wants":["medical_tinctures","respir…`
  - row 8: `{"access_rule":"Keep informal transport routes strictly unrecorded and settle off-ledger cargo losses privately; leading official gate wardens to a culvert cache closes all discreet transit routes permanently.","alignment":"conditional","badge_asset_id":"","display_name":"The Smugglers' Court","home_region":"region_crossing","id":"faction_the_smugglers_court","is_active":true,"offers":["off_ledger_trade","culvert_transit","discreet_arbitration"],"signature_quote":"What the Scale will not weigh and the Compact will not write down still has to cross the ditch; we only charge for the dark.","trust":0,"wants":["unchartered_salvage","route_intell…`
- Bytes: 6,289; SHA-256: `71f072a6dd213655a8ea3c2fbebbc617d31491ce3348166509517fe367ef27ec`
- Root keys: `actions, schema_version`
- `actions`: list[8]; union fields: `access_rule, alignment, badge_asset_id, display_name, home_region, id, is_active, offers, signature_quote, trust, wants`
  - row 1: `{"access_rule":"An agonizingly precise weigh-in is the price of doing business at Stallrow. Contest a true reading without cause, and the market stays open—but your name goes on the slate.","alignment":"conditional","badge_asset_id":"","display_name":"The Scale","home_region":"region_crossing","id":"faction_the_scale","is_active":true,"offers":["stallrow_trade_access","verification"],"signature_quote":"The brass scales do not lie, and the numbers have no conscience. What people infer from their poverty is not my problem.","trust":0,"wants":["trade_goods"]}`
  - row 2: `{"access_rule":"Their 'help' is genuine, offered against a plainly named, brutal forfeit—a child's labor, a pound of flesh, a year of servitude. Sign, negotiate, or walk away; no hidden clause survives a second reading.","alignment":"conditional","badge_asset_id":"","display_name":"The Underwrite","home_region":"region_crossing","id":"faction_the_underwrite","is_active":true,"offers":["seed_stock","covered_loss","favour_bank"],"signature_quote":"Read it twice, under the sodium glare. I'll say it twice. After the second time, there is only the ink, and the debt it binds you to.","trust":0,"wants":["pledged_goods"]}`
  - row 3: `{"access_rule":"Sign the blood-flecked draft and you are on the record. Perrin will not ratify a clause he knows will break a man—but he has not yet seen every way the words can be twisted.","alignment":"peaceful","badge_asset_id":"","display_name":"The Compact","home_region":"region_crossing","id":"faction_the_compact","is_active":true,"offers":["charter_draft","ratification"],"signature_quote":"There is a document now, stained with ash and thumbprints. Let them argue with the paper instead of each other's throats.","trust":0,"wants":["signatories"]}`
  - row 4: `{"access_rule":"Contribute fuel stores or clean reflector glass to the maintenance pool to ensure uninterrupted sector lighting; tampering with street lanterns or siphoning communal oil causes street lamps in your quarter to go dark.","alignment":"conditional","badge_asset_id":"","display_name":"The Lamplighters","home_region":"region_crossing","id":"faction_the_lamplighters","is_active":true,"offers":["street_lighting","night_watch_escort","route_visibility"],"signature_quote":"A dark street is an argument someone decided to settle with iron; keep the wicks trimmed and the town remembers what it agreed to.","trust":0,"wants":["fuel_stores",…`
  - row 5: `{"access_rule":"Honor minuted ration allotments and return dry grain sacks intact; unauthorized withdrawals or concealing spoilage revokes access to the communal reserve.","alignment":"conditional","badge_asset_id":"","display_name":"The Granary Wardens","home_region":"region_crossing","id":"faction_the_granary_wardens","is_active":true,"offers":["ration_distribution","emergency_grain_draw","spoilage_inspection"],"signature_quote":"The lock on the grain crib is not to keep hunger out; it is to ensure there is still seed in the hopper ninety days from now.","trust":0,"wants":["staple_grain","burlap_sacks"]}`
  - row 6: `{"access_rule":"Submit all bulk draw containers for sediment inspection and contribute clean filter charcoal or chlorine titration chits; drawing from closed filtration beds voids communal tap rights.","alignment":"conditional","badge_asset_id":"","display_name":"The Water Committee","home_region":"region_crossing","id":"faction_the_water_committee","is_active":true,"offers":["clean_water_rights","well_head_access","quality_certification"],"signature_quote":"You can argue with the charter all afternoon, but the sand filter does not care about your politics—run tainted water through it and everybody drinks poison.","trust":0,"wants":["filter_…`
  - row 7: `{"access_rule":"Submit to mandatory forty-eight-hour isolation during outbreak declarations and declare pulmonary symptoms; forging health clearance slips or breaking containment cordons results in immediate gate expulsion.","alignment":"neutral","badge_asset_id":"","display_name":"The Quarantine Post","home_region":"region_crossing","id":"faction_the_quarantine_post","is_active":true,"offers":["gate_health_screening","quarantine_clearance","outbreak_warning"],"signature_quote":"We do not weigh your goods or examine your coins; we look at the sweat on your collar and the rattling in your lungs.","trust":0,"wants":["medical_tinctures","respir…`
  - row 8: `{"access_rule":"Keep informal transport routes strictly unrecorded and settle off-ledger cargo losses privately; leading official gate wardens to a culvert cache closes all discreet transit routes permanently.","alignment":"conditional","badge_asset_id":"","display_name":"The Smugglers' Court","home_region":"region_crossing","id":"faction_the_smugglers_court","is_active":true,"offers":["off_ledger_trade","culvert_transit","discreet_arbitration"],"signature_quote":"What the Scale will not weigh and the Compact will not write down still has to cross the ditch; we only charge for the dark.","trust":0,"wants":["unchartered_salvage","route_intell…`

## `Assets/StreamingAssets/Data/crossing_encounters.json`
- Bytes: 28,037; SHA-256: `02ccdbff76776a6b5653a065640e59f5c1d7ddb9e3cfe31968e89338cb31423c`
- Root keys: `crises, encounters, schema_version`
- `encounters`: list[25]; union fields: `choices, description, id, name, target_location, threat_level`
  - row 1: `{"choices":[{"cost_items":["item_crossing_pledge_slip"],"result":"The collector crosses the line out in purple ink, gives a low nod, and moves on.","text":"Produce the signed voucher copy and clear the day's interest."},{"result":"The collector makes a single neat marginal mark and promises a visit to the Lockup.","text":"Refuse to open the ledger."}],"description":"An Underwrite collector waits by the gate with a bound ledger. Polite, procedural, exactly as threatening as the contract terms say and no more.","id":"enc_nc_collector_visit","name":"Underwrite Collector","target_location":"loc_crossing_viaduct_gate","threat_level":"Procedural"}`
  - row 2: `{"choices":[{"cost_items":["item_crossing_traded_salt"],"result":"She pockets the token and promises her hand will be in the air when the quorum is called.","text":"Hand over the salt to secure her voice."},{"result":"She shrugs. Principles do not preserve winter meat.","text":"Argue the principle of the matter without paying."}],"description":"A veteran stall-holder leans over the counter. She is willing to stand behind your ruling at the next Standing, but she wants a crate of salt first.","id":"enc_nc_backer_pressure","name":"Backer Pressure","target_location":"loc_crossing_stallrow","threat_level":"Negotiation"}`
  - row 3: `{"choices":[{"cost_items":["item_wyn_receipt_paid"],"result":"They step aside without a word. The chain rattles loose.","text":"Show the formal redemption note."},{"result":"No one pursues. The contract holds its ground.","text":"Back away slowly."}],"description":"Two heavies in salt-stained coats stand outside the Lockup gate. They are guarding forfeited goods under an active Underwrite lien.","id":"enc_nc_lockup_muscle","name":"Collateral Escort","target_location":"loc_crossing_underwrite_hall","threat_level":"Guarded"}`
  - row 4: `{"choices":[{"result":"The scout observes the disciplined guard shift, makes a notation, and melts into the dust.","text":"Hold ground under the Scale's neutral banner."}],"description":"A scout in hammered sheet steel perches on the viaduct approach, watching the gate with binoculars. They do not cross the survey seam.","id":"enc_nc_iron_raiders_scout","name":"Iron Raiders Scout","target_location":"loc_crossing_viaduct_gate","threat_level":"Observation"}`
  - row 5: `{"choices":[{"result":"Perrin Ashby stamps their transient pass. The Garrison detachment on the ridge takes note.","text":"Grant them shelter under the Compact annex rubric."},{"result":"They leave quietly into the ash. Osran approves of the caution.","text":"Turn them back to protect Crossing neutrality."}],"description":"A pair of exhausted deserters ask for two days of uninspected passage through the Crossing under Compact escort.","id":"enc_nc_deserter_passage","name":"Deserter Coalition Safe Passage","target_location":"loc_crossing_viaduct_gate","threat_level":"Diplomatic Risk"}`
  - row 6: `{"choices":[{"cost_items":["item_calibration_weight"],"result":"The scale drops true. The rogue merchant is escorted out without violence.","text":"Back Osran's calibration test with the True Weight."},{"result":"A shouting match erupts, lowering Stallrow trade trust for the week.","text":"Let the crowd settle the weight."}],"description":"A blacklisted scrap merchant tries to trade uncertified brass at Stallrow. Osran Kell steps up to inspect the mark.","id":"enc_nc_scavenger_dispute","name":"Scavenger Dispute","target_location":"loc_crossing_stallrow","threat_level":"Commercial"}`
  - row 7: `{"choices":[{"result":"The envoy smiles thinly, records the defiance, and withdraws to report to the board.","text":"Quote the charter rubric: 'No Power is in the room.'"}],"description":"An envoy from the Central Grain Exchange arrives to ask why the Crossing maintains its own independent weighbridge without sending a delegate to the Board.","id":"enc_nc_grain_exchange_envoy","name":"Grain Exchange Envoy","target_location":"loc_crossing_stallrow","threat_level":"Political"}`
  - row 8: `{"choices":[{"result":"The exchange is completed quickly. The convoy rolls westward into the sun.","text":"Trade clean water for their optical filters."}],"description":"A small convoy of Sun-Seekers in mirrored goggles passes through during a False Spring window, trading tinted glass for potable water.","id":"enc_nc_sun_seekers_pass","name":"Sun-Seekers Caravan Passage","target_location":"loc_crossing_viaduct_gate","threat_level":"Trade"}`
  - row 9: `{"choices":[{"result":"No voice is raised. The procedure is cold, clear, and undisputed.","text":"Stand in the crowd and witness the reading."}],"description":"A debtor's granary keys are surrendered at noon in front of the assembled stallholders. Dessa Vane reads the receipt aloud twice.","id":"enc_nc_forfeit_witness","name":"The Public Forfeit","target_location":"loc_crossing_stallrow","threat_level":"Atmospheric"}`
  - row 10: `{"choices":[{"cost_items":["item_vouch_token_crossing"],"result":"The authentic backers step forward. The quorum holds firm and the hijack fails.","text":"Spend your gathered Favour Tokens to call the honest quorum."}],"description":"A rival faction attempts to flood the Standing with last-minute proxy voters to overturn an arbitration ruling.","id":"enc_nc_standing_ambush","name":"Standing Ambush","target_location":"loc_crossing_stallrow","threat_level":"Crisis"}`
  - row 11: `{"choices":[{"result":"The crowd retreats to the staging ditches, grumbling but orderly.","text":"Deploy fire hoses and non-lethal deterrents to maintain order."},{"cost_items":["item_crossing_pledge_slip"],"result":"Fifty vulnerable refugees are sheltered safely in the lower arcade.","text":"Open the side wicket and process women and injured first."}],"description":"Hundreds of displaced refugees from the southern suburbs gather outside the heavy gate, clamoring for passage before the ash storm hits.","id":"enc_nc_mass_crossing_surge","name":"Mass Crossing Surge","target_location":"loc_crossing_viaduct_gate","threat_level":"Crisis"}`
  - row 12: `{"choices":[{"result":"The commander checks the wax seal and orders his crew to withdraw.","text":"Present the formal Charter Demarcation Treaty bearing garrison stamps."},{"cost_items":["item_crossing_traded_salt"],"result":"The armored car shifts into reverse and clears the highway lane.","text":"Pay a transit indemnity of forty rounds of rifle ammunition."}],"description":"An armored half-track from the Central Garrison parks across the approach road, mounting a heavy machine gun and demanding search rights.","id":"enc_nc_garrison_iron_blockade","name":"Garrison Armored Blockade","target_location":"loc_crossing_viaduct_gate","threat_level…`
  - row 13: `{"choices":[{"cost_items":["water_filter"],"result":"The panic subsides as systematic disinfection begins.","text":"Distribute respirator filters and carbolic disinfectant to stallholders."},{"result":"You break through the cordon, but market reputation is severely damaged.","text":"Force a passage through the southern barrier with weapons drawn."}],"description":"A yellow quarantine banner has been hoisted over the bazaar following three deaths from lung-rot. Armed market guards block all exit points.","id":"enc_nc_pestilence_quarantine_lockdown","name":"Pestilence Quarantine Standoff","target_location":"loc_crossing_stallrow","threat_level…`
  - row 14: `{"choices":[{"result":"The broker curses, but the munitions are cataloged into the public armory.","text":"Reject the bribe and order immediate inspection of the contraband."},{"result":"The heavy crate rolls through unchecked. Your fuel tanks are filled.","text":"Accept the fuel and sign the transit manifest without opening the crate."}],"description":"A syndicate broker in a tailored wool coat offers a leather pouch containing twenty litres of refined diesel fuel to overlook an uninspected munitions crate.","id":"enc_nc_syndicate_bribe_overture","name":"The Under-the-Table Voucher","target_location":"loc_crossing_underwrite_hall","threat_l…`
  - row 15: `{"choices":[{"cost_items":["item_trade_manifest_blank"],"result":"The blank manifest becomes a witness record. The attackers withdraw before the cargo seal is broken.","text":"Stand with the caravan and preserve the sealed cargo."},{"result":"The convoy moves again, but the first broken seal becomes an argument at the next Standing.","text":"Offer a measured share to get the road open."},{"result":"You leave the cargo untouched. The Scale receives a precise account and a difficult responsibility.","text":"Record the breach and leave the claim to the Crossing."}],"description":"A bonded caravan has stopped beneath the Scalehouse awning. Two g…`
  - row 16: `{"choices":[{"cost_items":["item_crossing_traded_salt"],"result":"The barrier opens. The receipt is new, but the authority it claims is not.","text":"Pay the fee and keep the convoy moving."},{"result":"The watch records both names and orders the barrier lowered until the claim is heard.","text":"Ask the Watchtower to test the checkpoint's jurisdiction."},{"result":"The road remains closed to you, and the checkpoint keeps collecting from those with less time.","text":"Turn back before an unofficial toll becomes a recognized one."}],"description":"An improvised barrier blocks the unmarked road below the Watchtower. The people holding it call …`
  - row 17: `{"choices":[{"cost_items":["item_trade_manifest_blank"],"result":"The cargo stays sealed while the first legible claim is copied into the Crossing record.","text":"Secure the manifest and ask the Records Room to hold the claim."},{"result":"The passage clears, but every claimant can point to the moment the seal was disturbed.","text":"Take only the supplies needed to free the route."},{"result":"No property changes hands. The ice and the claims remain in place for whoever comes next.","text":"Leave the barge frozen and move the convoy around it."}],"description":"A cargo barge has frozen into the Drown's edge below the viaduct. Its manifest …`
  - row 18: `{"choices":[{"cost_items":["item_crossing_traded_salt"],"result":"The barrier lifts. The payment buys safety and gives the new toll a history.","text":"Pay the toll and keep the crossing from becoming a firefight."},{"cost_items":["item_charter_three_pages"],"result":"The crew reads the clause twice. Nobody agrees what it means, but the rifles lower for now.","text":"Present the charter pages and demand the common route remain open."},{"result":"Your cargo arrives late. The bridge crew keeps calling the delay proof that their toll is necessary.","text":"Take the longer approach and leave the claim untested."}],"description":"A faction crew h…`
  - row 19: `{"choices":[{"result":"The span holds. The watch records the load order as a temporary rule.","text":"Cross one load at a time under the Watchtower's count."},{"result":"The road goes quiet. Traders complain, but nobody has to explain a second collapse.","text":"Stop and ask the Crossing to close the route until it is reinforced."},{"result":"You keep your people back. The route remains open in theory and less trustworthy in practice.","text":"Let another convoy test the chalked limit first."}],"description":"A section of the approach has fallen away, leaving a narrow span built from doors and cable. Someone has chalked load limits on both s…`
  - row 20: `{"choices":[{"result":"The cargo waits under seal. The argument moves from rumor to a page that can be read by everyone.","text":"Hold the casks and enter the discrepancy in the load record."},{"result":"The convoy leaves on time. If the residue was fresh, the record will show who chose speed.","text":"Accept the trader's assurance and keep the route open."},{"result":"The safer route costs a day. The warning travels farther than the original claim.","text":"Reroute around the ford and warn the next carrier."}],"description":"Oily residue clings to the water casks arriving at the Scalehouse. A trader insists the contamination is old; the loa…`
  - row 21: `{"choices":[{"result":"The crossing takes longer, but the manifest survives with every seal accounted for.","text":"Unload and shuttle the cargo across in smaller weights."},{"result":"The convoy arrives late and intact. Nobody calls the detour wasteful after dark.","text":"Take the bank route and accept the lost daylight."},{"result":"The route holds for now. The next convoy will not be able to say it was unwarned.","text":"Continue straight with only the lightest loads aboard."}],"description":"The ice gives a low cracking report beneath the first loaded sled at the viaduct approach. The shortest route stays straight ahead; the safer bank …`
  - row 22: `{"choices":[{"result":"The route costs time, but the warning stakes gain a fresh line in the Crossing record.","text":"Backtrack and mark the approach closed."},{"result":"The convoy passes one careful step at a time. The safe line becomes a fragile piece of shared knowledge.","text":"Follow the old marked line without leaving it."},{"result":"The crossing is abandoned for the day. Nothing is gained except the chance to try again alive.","text":"Send no one into the field and wait for another route."}],"description":"Old warning stakes emerge from the snow around the Founders' Marker. Beyond them, disturbed ground and one half-buried casing …`
  - row 23: `{"choices":[{"result":"The road stays blocked, but the names and needs in the petition become part of the public record.","text":"Hear the petition before asking the carts to move."},{"cost_items":["item_crossing_pledge_slip"],"result":"The carts clear one lane. The sponsor accepts a duty that can be read back later.","text":"Offer a temporary place in the Annex under a named sponsor."},{"result":"The route opens by order. The petition remains, and so does the grievance.","text":"Enforce the road's passage and leave admission to the next assembly."}],"description":"A group denied immediate admission has stopped carts outside the Petition Ten…`
  - row 24: `{"choices":[{"result":"The family receives a place in the Annex. The ledger gains an obligation no office can pretend not to see.","text":"Honor the chit under the household named on it."},{"result":"The paper remains intact, but the family waits outside the rule it was meant to invoke.","text":"Require a new sponsor before entry."},{"result":"The family gets a night indoors. Tomorrow's ruling will decide whether the temporary measure becomes a precedent.","text":"Grant temporary entry while the Records Room verifies the claim."}],"description":"A family presents an admission chit issued months ago by a sponsor who is now missing. The gate c…`
  - row 25: `{"choices":[{"result":"The gate honors the name before anyone can prove what happened to the household behind it.","text":"Admit the child under the old household record."},{"result":"The child is safe for the night. The Crossing must decide whether verification is protection or delay.","text":"Hold the child in the Annex while the Records Room checks the token."},{"result":"The request moves through the Petition Tent. The token remains a claim, not yet an admission.","text":"Seek a current sponsor before opening the gate."}],"description":"A child arrives alone with an adult's charter token and a surname the gate clerk recognizes. No guardi…`
- `crises`: list[12]; union fields: `description, id, name, phases, resolution`
  - row 1: `{"description":"The Underwrite moves to seize the community granary unless the debt is paid or renegotiated.","id":"crisis_the_forfeit","name":"Wyn's Granary Forfeit","phases":["Notice","Terms Read","Broker Attempt","Resolution"],"resolution":"Debt honoured or fairly renegotiated through arbitration."}`
  - row 2: `{"description":"The Compact brings the revised charter rubric to an open assembly vote.","id":"crisis_the_vote","name":"Draft Four Ratification","phases":["Call","Canvas","Interference","Count"],"resolution":"Clean or contested ratification by majority show of hands."}`
  - row 3: `{"description":"A high-stakes trade arbitration is challenged by a bloc claiming corrupted calibration.","id":"crisis_the_standing_contested","name":"The Contested Standing","phases":["Call","Backer Recruitment","Rival Recruitment","Verdict"],"resolution":"Ruling holds or is honestly overturned through verified weights."}`
  - row 4: `{"description":"The original pre-war charter pages are recovered from the Records Room archive.","id":"crisis_the_charter_found","name":"Three Dry Pages Discovered","phases":["Request","Read","Verify","Decide"],"resolution":"The truth of the crossing is revealed to all three blocs."}`
  - row 5: `{"description":"The final confrontation determining whether Scale, Underwrite, Compact, or Collapse governs Sector 4.","id":"crisis_who_holds_the_ledger","name":"Who Holds the Ledger","phases":["Summons","Final Backing","Quorum Ruling","Aftermath"],"resolution":"One of four canonical faction resolutions is established."}`
  - row 6: `{"description":"A maintenance bloc claims that keeping the Crossing's water source working gives it the right to control access. Others answer that common water is written into the charter because nobody survives a private monopoly.","id":"crisis_the_water_claim","name":"The Water Claim","phases":["Claim","Counter-Petition","Arbitration","Ruling"],"resolution":"The dispute is settled through the existing arbitration path: common access, a maintenance concession, or a publicly recorded usage rule."}`
  - row 7: `{"description":"Grain deliveries fall behind ration promises, and the warehouse inventory no longer matches the public ledger. The crowd outside wants the doors opened before an audit can finish.","id":"crisis_the_grain_riot","name":"The Grain Riot","phases":["Shortage","Hoarding Accusation","Distribution","Calm or Collapse"],"resolution":"An existing vote, arbitration, or forfeit path determines whether the grain is distributed, held for proof, or seized under a recorded obligation."}`
  - row 8: `{"description":"A bloc proposes narrowing charter protection for residents it calls temporary. The amendment is legal enough to reach the floor, which makes the argument more dangerous than an open threat.","id":"crisis_the_charter_amendment","name":"The Charter Amendment","phases":["Proposal","Debate","Vote","Enactment"],"resolution":"The existing Crossing vote path records a clean, contested, or failed amendment without creating a new charter-law interpreter."}`
  - row 9: `{"description":"A debtor bloc asks that emergency obligations from the last season be erased together. Creditors argue that forgiving them now means no one will extend supplies on promise again.","id":"crisis_the_debt_forgiveness","name":"Debt Forgiveness","phases":["Demand","Counteroffer","Assembly","Verdict"],"resolution":"Existing assembly or arbitration mechanics produce a recorded settlement, partial forfeit, or refusal; no new debt authority is implied."}`
  - row 10: `{"description":"A refugee group requests admission under the Crossing's nobody-owned charter. The claim is morally simple and administratively expensive: food, space, and names must be entered into systems already under strain.","id":"crisis_the_refugee_admission","name":"Refugee Admission","phases":["Arrival","Vetting","Resource Test","Admission or Refusal"],"resolution":"The existing vote or arbitration route records admission, sponsorship, or refusal without adding a refugee simulation."}`
  - row 11: `{"description":"Payment records suggest an arbitrator received supplies from one side of a pending claim. The arbitrator says the transfer was repayment of an older debt and asks to remain seated.","id":"crisis_the_arbitrator_bribe","name":"The Arbitrator Bribe","phases":["Accusation","Evidence","Hearing","Replacement or Vindication"],"resolution":"Existing arbitration and backer mechanics determine whether the ruling is held, challenged, or re-called; no tribunal subsystem is introduced."}`
  - row 12: `{"description":"A resident leaves quarantine before clearance and spends several hours near the market district. The medical question is urgent; the civic question begins when people ask whether the resident still has charter protection.","id":"crisis_the_quarantine_break","name":"The Quarantine Break","phases":["Break","Exposure Trace","Containment","Reconciliation"],"resolution":"The existing Crossing political path records containment and reintegration decisions; disease state remains owned by the active disease system."}`
- Bytes: 28,037; SHA-256: `02ccdbff76776a6b5653a065640e59f5c1d7ddb9e3cfe31968e89338cb31423c`
- Root keys: `crises, encounters, schema_version`
- `encounters`: list[25]; union fields: `choices, description, id, name, target_location, threat_level`
  - row 1: `{"choices":[{"cost_items":["item_crossing_pledge_slip"],"result":"The collector crosses the line out in purple ink, gives a low nod, and moves on.","text":"Produce the signed voucher copy and clear the day's interest."},{"result":"The collector makes a single neat marginal mark and promises a visit to the Lockup.","text":"Refuse to open the ledger."}],"description":"An Underwrite collector waits by the gate with a bound ledger. Polite, procedural, exactly as threatening as the contract terms say and no more.","id":"enc_nc_collector_visit","name":"Underwrite Collector","target_location":"loc_crossing_viaduct_gate","threat_level":"Procedural"}`
  - row 2: `{"choices":[{"cost_items":["item_crossing_traded_salt"],"result":"She pockets the token and promises her hand will be in the air when the quorum is called.","text":"Hand over the salt to secure her voice."},{"result":"She shrugs. Principles do not preserve winter meat.","text":"Argue the principle of the matter without paying."}],"description":"A veteran stall-holder leans over the counter. She is willing to stand behind your ruling at the next Standing, but she wants a crate of salt first.","id":"enc_nc_backer_pressure","name":"Backer Pressure","target_location":"loc_crossing_stallrow","threat_level":"Negotiation"}`
  - row 3: `{"choices":[{"cost_items":["item_wyn_receipt_paid"],"result":"They step aside without a word. The chain rattles loose.","text":"Show the formal redemption note."},{"result":"No one pursues. The contract holds its ground.","text":"Back away slowly."}],"description":"Two heavies in salt-stained coats stand outside the Lockup gate. They are guarding forfeited goods under an active Underwrite lien.","id":"enc_nc_lockup_muscle","name":"Collateral Escort","target_location":"loc_crossing_underwrite_hall","threat_level":"Guarded"}`
  - row 4: `{"choices":[{"result":"The scout observes the disciplined guard shift, makes a notation, and melts into the dust.","text":"Hold ground under the Scale's neutral banner."}],"description":"A scout in hammered sheet steel perches on the viaduct approach, watching the gate with binoculars. They do not cross the survey seam.","id":"enc_nc_iron_raiders_scout","name":"Iron Raiders Scout","target_location":"loc_crossing_viaduct_gate","threat_level":"Observation"}`
  - row 5: `{"choices":[{"result":"Perrin Ashby stamps their transient pass. The Garrison detachment on the ridge takes note.","text":"Grant them shelter under the Compact annex rubric."},{"result":"They leave quietly into the ash. Osran approves of the caution.","text":"Turn them back to protect Crossing neutrality."}],"description":"A pair of exhausted deserters ask for two days of uninspected passage through the Crossing under Compact escort.","id":"enc_nc_deserter_passage","name":"Deserter Coalition Safe Passage","target_location":"loc_crossing_viaduct_gate","threat_level":"Diplomatic Risk"}`
  - row 6: `{"choices":[{"cost_items":["item_calibration_weight"],"result":"The scale drops true. The rogue merchant is escorted out without violence.","text":"Back Osran's calibration test with the True Weight."},{"result":"A shouting match erupts, lowering Stallrow trade trust for the week.","text":"Let the crowd settle the weight."}],"description":"A blacklisted scrap merchant tries to trade uncertified brass at Stallrow. Osran Kell steps up to inspect the mark.","id":"enc_nc_scavenger_dispute","name":"Scavenger Dispute","target_location":"loc_crossing_stallrow","threat_level":"Commercial"}`
  - row 7: `{"choices":[{"result":"The envoy smiles thinly, records the defiance, and withdraws to report to the board.","text":"Quote the charter rubric: 'No Power is in the room.'"}],"description":"An envoy from the Central Grain Exchange arrives to ask why the Crossing maintains its own independent weighbridge without sending a delegate to the Board.","id":"enc_nc_grain_exchange_envoy","name":"Grain Exchange Envoy","target_location":"loc_crossing_stallrow","threat_level":"Political"}`
  - row 8: `{"choices":[{"result":"The exchange is completed quickly. The convoy rolls westward into the sun.","text":"Trade clean water for their optical filters."}],"description":"A small convoy of Sun-Seekers in mirrored goggles passes through during a False Spring window, trading tinted glass for potable water.","id":"enc_nc_sun_seekers_pass","name":"Sun-Seekers Caravan Passage","target_location":"loc_crossing_viaduct_gate","threat_level":"Trade"}`
  - row 9: `{"choices":[{"result":"No voice is raised. The procedure is cold, clear, and undisputed.","text":"Stand in the crowd and witness the reading."}],"description":"A debtor's granary keys are surrendered at noon in front of the assembled stallholders. Dessa Vane reads the receipt aloud twice.","id":"enc_nc_forfeit_witness","name":"The Public Forfeit","target_location":"loc_crossing_stallrow","threat_level":"Atmospheric"}`
  - row 10: `{"choices":[{"cost_items":["item_vouch_token_crossing"],"result":"The authentic backers step forward. The quorum holds firm and the hijack fails.","text":"Spend your gathered Favour Tokens to call the honest quorum."}],"description":"A rival faction attempts to flood the Standing with last-minute proxy voters to overturn an arbitration ruling.","id":"enc_nc_standing_ambush","name":"Standing Ambush","target_location":"loc_crossing_stallrow","threat_level":"Crisis"}`
  - row 11: `{"choices":[{"result":"The crowd retreats to the staging ditches, grumbling but orderly.","text":"Deploy fire hoses and non-lethal deterrents to maintain order."},{"cost_items":["item_crossing_pledge_slip"],"result":"Fifty vulnerable refugees are sheltered safely in the lower arcade.","text":"Open the side wicket and process women and injured first."}],"description":"Hundreds of displaced refugees from the southern suburbs gather outside the heavy gate, clamoring for passage before the ash storm hits.","id":"enc_nc_mass_crossing_surge","name":"Mass Crossing Surge","target_location":"loc_crossing_viaduct_gate","threat_level":"Crisis"}`
  - row 12: `{"choices":[{"result":"The commander checks the wax seal and orders his crew to withdraw.","text":"Present the formal Charter Demarcation Treaty bearing garrison stamps."},{"cost_items":["item_crossing_traded_salt"],"result":"The armored car shifts into reverse and clears the highway lane.","text":"Pay a transit indemnity of forty rounds of rifle ammunition."}],"description":"An armored half-track from the Central Garrison parks across the approach road, mounting a heavy machine gun and demanding search rights.","id":"enc_nc_garrison_iron_blockade","name":"Garrison Armored Blockade","target_location":"loc_crossing_viaduct_gate","threat_level…`
  - row 13: `{"choices":[{"cost_items":["water_filter"],"result":"The panic subsides as systematic disinfection begins.","text":"Distribute respirator filters and carbolic disinfectant to stallholders."},{"result":"You break through the cordon, but market reputation is severely damaged.","text":"Force a passage through the southern barrier with weapons drawn."}],"description":"A yellow quarantine banner has been hoisted over the bazaar following three deaths from lung-rot. Armed market guards block all exit points.","id":"enc_nc_pestilence_quarantine_lockdown","name":"Pestilence Quarantine Standoff","target_location":"loc_crossing_stallrow","threat_level…`
  - row 14: `{"choices":[{"result":"The broker curses, but the munitions are cataloged into the public armory.","text":"Reject the bribe and order immediate inspection of the contraband."},{"result":"The heavy crate rolls through unchecked. Your fuel tanks are filled.","text":"Accept the fuel and sign the transit manifest without opening the crate."}],"description":"A syndicate broker in a tailored wool coat offers a leather pouch containing twenty litres of refined diesel fuel to overlook an uninspected munitions crate.","id":"enc_nc_syndicate_bribe_overture","name":"The Under-the-Table Voucher","target_location":"loc_crossing_underwrite_hall","threat_l…`
  - row 15: `{"choices":[{"cost_items":["item_trade_manifest_blank"],"result":"The blank manifest becomes a witness record. The attackers withdraw before the cargo seal is broken.","text":"Stand with the caravan and preserve the sealed cargo."},{"result":"The convoy moves again, but the first broken seal becomes an argument at the next Standing.","text":"Offer a measured share to get the road open."},{"result":"You leave the cargo untouched. The Scale receives a precise account and a difficult responsibility.","text":"Record the breach and leave the claim to the Crossing."}],"description":"A bonded caravan has stopped beneath the Scalehouse awning. Two g…`
  - row 16: `{"choices":[{"cost_items":["item_crossing_traded_salt"],"result":"The barrier opens. The receipt is new, but the authority it claims is not.","text":"Pay the fee and keep the convoy moving."},{"result":"The watch records both names and orders the barrier lowered until the claim is heard.","text":"Ask the Watchtower to test the checkpoint's jurisdiction."},{"result":"The road remains closed to you, and the checkpoint keeps collecting from those with less time.","text":"Turn back before an unofficial toll becomes a recognized one."}],"description":"An improvised barrier blocks the unmarked road below the Watchtower. The people holding it call …`
  - row 17: `{"choices":[{"cost_items":["item_trade_manifest_blank"],"result":"The cargo stays sealed while the first legible claim is copied into the Crossing record.","text":"Secure the manifest and ask the Records Room to hold the claim."},{"result":"The passage clears, but every claimant can point to the moment the seal was disturbed.","text":"Take only the supplies needed to free the route."},{"result":"No property changes hands. The ice and the claims remain in place for whoever comes next.","text":"Leave the barge frozen and move the convoy around it."}],"description":"A cargo barge has frozen into the Drown's edge below the viaduct. Its manifest …`
  - row 18: `{"choices":[{"cost_items":["item_crossing_traded_salt"],"result":"The barrier lifts. The payment buys safety and gives the new toll a history.","text":"Pay the toll and keep the crossing from becoming a firefight."},{"cost_items":["item_charter_three_pages"],"result":"The crew reads the clause twice. Nobody agrees what it means, but the rifles lower for now.","text":"Present the charter pages and demand the common route remain open."},{"result":"Your cargo arrives late. The bridge crew keeps calling the delay proof that their toll is necessary.","text":"Take the longer approach and leave the claim untested."}],"description":"A faction crew h…`
  - row 19: `{"choices":[{"result":"The span holds. The watch records the load order as a temporary rule.","text":"Cross one load at a time under the Watchtower's count."},{"result":"The road goes quiet. Traders complain, but nobody has to explain a second collapse.","text":"Stop and ask the Crossing to close the route until it is reinforced."},{"result":"You keep your people back. The route remains open in theory and less trustworthy in practice.","text":"Let another convoy test the chalked limit first."}],"description":"A section of the approach has fallen away, leaving a narrow span built from doors and cable. Someone has chalked load limits on both s…`
  - row 20: `{"choices":[{"result":"The cargo waits under seal. The argument moves from rumor to a page that can be read by everyone.","text":"Hold the casks and enter the discrepancy in the load record."},{"result":"The convoy leaves on time. If the residue was fresh, the record will show who chose speed.","text":"Accept the trader's assurance and keep the route open."},{"result":"The safer route costs a day. The warning travels farther than the original claim.","text":"Reroute around the ford and warn the next carrier."}],"description":"Oily residue clings to the water casks arriving at the Scalehouse. A trader insists the contamination is old; the loa…`
  - row 21: `{"choices":[{"result":"The crossing takes longer, but the manifest survives with every seal accounted for.","text":"Unload and shuttle the cargo across in smaller weights."},{"result":"The convoy arrives late and intact. Nobody calls the detour wasteful after dark.","text":"Take the bank route and accept the lost daylight."},{"result":"The route holds for now. The next convoy will not be able to say it was unwarned.","text":"Continue straight with only the lightest loads aboard."}],"description":"The ice gives a low cracking report beneath the first loaded sled at the viaduct approach. The shortest route stays straight ahead; the safer bank …`
  - row 22: `{"choices":[{"result":"The route costs time, but the warning stakes gain a fresh line in the Crossing record.","text":"Backtrack and mark the approach closed."},{"result":"The convoy passes one careful step at a time. The safe line becomes a fragile piece of shared knowledge.","text":"Follow the old marked line without leaving it."},{"result":"The crossing is abandoned for the day. Nothing is gained except the chance to try again alive.","text":"Send no one into the field and wait for another route."}],"description":"Old warning stakes emerge from the snow around the Founders' Marker. Beyond them, disturbed ground and one half-buried casing …`
  - row 23: `{"choices":[{"result":"The road stays blocked, but the names and needs in the petition become part of the public record.","text":"Hear the petition before asking the carts to move."},{"cost_items":["item_crossing_pledge_slip"],"result":"The carts clear one lane. The sponsor accepts a duty that can be read back later.","text":"Offer a temporary place in the Annex under a named sponsor."},{"result":"The route opens by order. The petition remains, and so does the grievance.","text":"Enforce the road's passage and leave admission to the next assembly."}],"description":"A group denied immediate admission has stopped carts outside the Petition Ten…`
  - row 24: `{"choices":[{"result":"The family receives a place in the Annex. The ledger gains an obligation no office can pretend not to see.","text":"Honor the chit under the household named on it."},{"result":"The paper remains intact, but the family waits outside the rule it was meant to invoke.","text":"Require a new sponsor before entry."},{"result":"The family gets a night indoors. Tomorrow's ruling will decide whether the temporary measure becomes a precedent.","text":"Grant temporary entry while the Records Room verifies the claim."}],"description":"A family presents an admission chit issued months ago by a sponsor who is now missing. The gate c…`
  - row 25: `{"choices":[{"result":"The gate honors the name before anyone can prove what happened to the household behind it.","text":"Admit the child under the old household record."},{"result":"The child is safe for the night. The Crossing must decide whether verification is protection or delay.","text":"Hold the child in the Annex while the Records Room checks the token."},{"result":"The request moves through the Petition Tent. The token remains a claim, not yet an admission.","text":"Seek a current sponsor before opening the gate."}],"description":"A child arrives alone with an adult's charter token and a surname the gate clerk recognizes. No guardi…`
- `crises`: list[12]; union fields: `description, id, name, phases, resolution`
  - row 1: `{"description":"The Underwrite moves to seize the community granary unless the debt is paid or renegotiated.","id":"crisis_the_forfeit","name":"Wyn's Granary Forfeit","phases":["Notice","Terms Read","Broker Attempt","Resolution"],"resolution":"Debt honoured or fairly renegotiated through arbitration."}`
  - row 2: `{"description":"The Compact brings the revised charter rubric to an open assembly vote.","id":"crisis_the_vote","name":"Draft Four Ratification","phases":["Call","Canvas","Interference","Count"],"resolution":"Clean or contested ratification by majority show of hands."}`
  - row 3: `{"description":"A high-stakes trade arbitration is challenged by a bloc claiming corrupted calibration.","id":"crisis_the_standing_contested","name":"The Contested Standing","phases":["Call","Backer Recruitment","Rival Recruitment","Verdict"],"resolution":"Ruling holds or is honestly overturned through verified weights."}`
  - row 4: `{"description":"The original pre-war charter pages are recovered from the Records Room archive.","id":"crisis_the_charter_found","name":"Three Dry Pages Discovered","phases":["Request","Read","Verify","Decide"],"resolution":"The truth of the crossing is revealed to all three blocs."}`
  - row 5: `{"description":"The final confrontation determining whether Scale, Underwrite, Compact, or Collapse governs Sector 4.","id":"crisis_who_holds_the_ledger","name":"Who Holds the Ledger","phases":["Summons","Final Backing","Quorum Ruling","Aftermath"],"resolution":"One of four canonical faction resolutions is established."}`
  - row 6: `{"description":"A maintenance bloc claims that keeping the Crossing's water source working gives it the right to control access. Others answer that common water is written into the charter because nobody survives a private monopoly.","id":"crisis_the_water_claim","name":"The Water Claim","phases":["Claim","Counter-Petition","Arbitration","Ruling"],"resolution":"The dispute is settled through the existing arbitration path: common access, a maintenance concession, or a publicly recorded usage rule."}`
  - row 7: `{"description":"Grain deliveries fall behind ration promises, and the warehouse inventory no longer matches the public ledger. The crowd outside wants the doors opened before an audit can finish.","id":"crisis_the_grain_riot","name":"The Grain Riot","phases":["Shortage","Hoarding Accusation","Distribution","Calm or Collapse"],"resolution":"An existing vote, arbitration, or forfeit path determines whether the grain is distributed, held for proof, or seized under a recorded obligation."}`
  - row 8: `{"description":"A bloc proposes narrowing charter protection for residents it calls temporary. The amendment is legal enough to reach the floor, which makes the argument more dangerous than an open threat.","id":"crisis_the_charter_amendment","name":"The Charter Amendment","phases":["Proposal","Debate","Vote","Enactment"],"resolution":"The existing Crossing vote path records a clean, contested, or failed amendment without creating a new charter-law interpreter."}`
  - row 9: `{"description":"A debtor bloc asks that emergency obligations from the last season be erased together. Creditors argue that forgiving them now means no one will extend supplies on promise again.","id":"crisis_the_debt_forgiveness","name":"Debt Forgiveness","phases":["Demand","Counteroffer","Assembly","Verdict"],"resolution":"Existing assembly or arbitration mechanics produce a recorded settlement, partial forfeit, or refusal; no new debt authority is implied."}`
  - row 10: `{"description":"A refugee group requests admission under the Crossing's nobody-owned charter. The claim is morally simple and administratively expensive: food, space, and names must be entered into systems already under strain.","id":"crisis_the_refugee_admission","name":"Refugee Admission","phases":["Arrival","Vetting","Resource Test","Admission or Refusal"],"resolution":"The existing vote or arbitration route records admission, sponsorship, or refusal without adding a refugee simulation."}`
  - row 11: `{"description":"Payment records suggest an arbitrator received supplies from one side of a pending claim. The arbitrator says the transfer was repayment of an older debt and asks to remain seated.","id":"crisis_the_arbitrator_bribe","name":"The Arbitrator Bribe","phases":["Accusation","Evidence","Hearing","Replacement or Vindication"],"resolution":"Existing arbitration and backer mechanics determine whether the ruling is held, challenged, or re-called; no tribunal subsystem is introduced."}`
  - row 12: `{"description":"A resident leaves quarantine before clearance and spends several hours near the market district. The medical question is urgent; the civic question begins when people ask whether the resident still has charter protection.","id":"crisis_the_quarantine_break","name":"The Quarantine Break","phases":["Break","Exposure Trace","Containment","Reconciliation"],"resolution":"The existing Crossing political path records containment and reintegration decisions; disease state remains owned by the active disease system."}`

## `Assets/StreamingAssets/Data/crossing_quests.json`
- Bytes: 34,791; SHA-256: `94d5932902ac39bdac0f2caa6c7e56a1bdb33247063e397ca9d2ac5949315734`
- Root keys: `quests, schema_version`
- `quests`: list[23]; union fields: `briefing, choices, display_name, id, knowledge_key, min_day, prereq_quest_id, stages, target_location_id, type`
  - row 1: `{"briefing":"Bram Ostrowski names the Crossing and will sell you a rough sketch of the approach. He will not walk there himself. \"I sold them a map once. That was the whole transaction. I'd like it to stay that way.\"","choices":[{"id":"vouch_ostrowski_reluctant","set_flag":"flag_crossing_vouched_clean","text":"Ostrowski vouches — once, and he would like it to stay that way."},{"id":"vouch_mattis_at_truss","set_flag":"flag_crossing_vouched_clean","text":"Take Mattis Cray's name at the truss and register it on the ledger."}],"display_name":"A Name at the Gate","id":"quest_crossing_the_vouch","knowledge_key":"lore_nc_the_vouch","min_day":70,"…`
  - row 2: `{"briefing":"Osran weighs your goods on the depot scale. The number is real. What people infer from it is not his problem. Present your load, answer his questions about the shelter, and either accept the recorded weight or contest it — honestly, or not.","choices":[{"id":"first_weigh_accept_true","set_flag":"mutation_crossing_honest_trader","text":"Accept the true weight, even when it is less favourable than hoped."},{"id":"first_weigh_contest","set_flag":"mark_crossing_difficult","text":"Contest the true weight anyway. Access still granted; the exchange notes it."},{"id":"first_weigh_bribe","set_flag":"mutation_crossing_bribe_attempted","te…`
  - row 3: `{"briefing":"The scale is honest because it is checked. Help prove that on the record — calibrate against the weight, verify a disputed load, and leave Stallrow able to trust the number without trusting anyone to have set it.","choices":[{"id":"scale_integrity_clear","set_flag":"mutation_crossing_honest_trader","text":"The scale reads clean. Stallrow keeps trusting the number."},{"id":"scale_integrity_silent","set_flag":"flag_crossing_scale_verified_silent","text":"Verify honestly but decline to record it. The trust stays, unspoken."}],"display_name":"What the Weight Doesn't Move","id":"quest_crossing_scale_integrity","knowledge_key":"lore_n…`
  - row 4: `{"briefing":"Dessa Vane offers seed, a covered loss, or a favour bank against a plainly named forfeit. The contract is read twice before it is signed. After the second reading there is only the ink. What you agree to, you will owe — and the forfeit is named up front, not hidden in a clause.","choices":[{"id":"terms_sign_pay","set_flag":"mutation_crossing_underwrite_reliable","text":"Sign and later pay. The Underwrite remembers reliable debtors."},{"id":"terms_decline","set_flag":"flag_crossing_underwrite_untested","text":"Decline the contract. The hall stays open. Dessa notes it without judgement."},{"id":"terms_sign_dump","set_flag":"mutati…`
  - row 5: `{"briefing":"Perrin Ashby asks for an early signature on the Compact's draft charter. If you read it, there is a scoring clause — who gets a vote, and how much it weighs. He has not noticed that it resembles a Reconstruction Utility Rating. He will, if you tell him.","choices":[{"id":"petition_revise","set_flag":"mutation_crossing_petition_revised","text":"Suggest a revision pass. The draft improves. Perrin is grateful, quietly."},{"id":"petition_decline","set_flag":"flag_crossing_petition_unsigned","text":"Decline to sign. The Compact loses an early supporter. Perrin does not hold it against you."},{"id":"petition_leak","set_flag":"mutation…`
  - row 6: `{"briefing":"A dispute at Stallrow needs settling. Three backers must hold the ruling or it is just noise. Learn how the Crossing resolves what no Power will hear: a name spoken in public, by people who have to eat tomorrow, about what is fair today.","choices":[{"id":"standing_back_honest","set_flag":"flag_crossing_standing_honest","text":"Back the side you believe. The ruling will hold honestly."},{"id":"standing_decline","set_flag":"","text":"Decline to back. The ruling may still hold, without your name."},{"id":"standing_bribe","set_flag":"flag_crossing_standing_rigged","text":"Promise both sides or bribe a backer. The ruling holds, but …`
  - row 7: `{"briefing":"The old bronze marker at Nightfire is corroded past the third line. Everyone in the Crossing tells a different story about what was written below the names. Inspect the marker, hear the tellers, and learn where the original paper went.","choices":[{"id":"marker_report_contradictions","set_flag":"flag_crossing_marker_contradictions_noted","text":"Report the contradictions as they are. The mystery remains honest."},{"id":"marker_seed_legend","set_flag":"mutation_crossing_myth_seeded","text":"Seed a tailored legend among the tellers to favor your preferred bloc."}],"display_name":"What the Plaque Doesn't Say","id":"quest_crossing_t…`
  - row 8: `{"briefing":"Wyn Sabler's pledge term has expired. Dessa Vane is coming to collect the exact named forfeit, on the record, in the Underwrite Lockup. Wyn will not ask for help, but she cannot pay alone.","choices":[{"id":"forfeit_pay_debt","set_flag":"mutation_crossing_forfeit_honoured","text":"Pay the grain principal for Wyn. The Underwrite marks the contract fulfilled."},{"id":"forfeit_let_proceed","set_flag":"flag_crossing_forfeit_collected","text":"Let the forfeit proceed. Dessa collects calmly without violence."},{"id":"forfeit_help_flee","set_flag":"mutation_crossing_underwrite_burned","text":"Help Wyn smuggle the pledged sacks out. Des…`
  - row 9: `{"briefing":"Perrin Ashby has called a general gathering to ratify Draft Four of the Compact Charter. Dessa Vane plans to stall the vote by calling in a key backer's overdue debt right on the floor.","choices":[{"id":"vote_cover_debt","set_flag":"mutation_crossing_vote_clean","text":"Cover the claim so the vote proceeds cleanly."},{"id":"vote_sabotage_compact","set_flag":"mutation_crossing_vote_sabotaged","text":"Back the Underwrite's challenge and derail the ratification."}],"display_name":"Draft Four, Called","id":"quest_crossing_the_vote_that_isnt","knowledge_key":"lore_nc_the_standing","min_day":90,"prereq_quest_id":"quest_crossing_the_p…`
  - row 10: `{"briefing":"Ivo Fenn opens the iron box in the Records Room and presents the actual founding Charter. It is only three dry pages: calibration standards, revenue splits, and signatures from people who survived the first winter.","choices":[{"id":"charter_publish_all","set_flag":"mutation_crossing_charter_revealed","text":"Publish the three pages openly on the Stallrow board."},{"id":"charter_keep_hidden","set_flag":"flag_crossing_charter_hidden","text":"Leave the pages locked in the box for history."}],"display_name":"The Charter","id":"quest_crossing_three_dry_pages","knowledge_key":"lore_nc_three_legends","min_day":95,"prereq_quest_id":"qu…`
  - row 11: `{"briefing":"The final Standing is called across the entire Viaduct. Every trade, debt, and ruling you have touched comes together to determine who speaks for the Crossing.","choices":[{"id":"ledger_scale_holds","set_flag":"mutation_crossing_honest_trader","text":"The Scale remains sovereign; fair weights govern the viaduct."},{"id":"ledger_underwrite_holds","set_flag":"mutation_crossing_underwrite_reliable","text":"The Underwrite takes custody of all pledges and traffic."},{"id":"ledger_compact_holds","set_flag":"mutation_crossing_vote_clean","text":"The Compact ratifies Draft Four and establishes a council."}],"display_name":"Who Holds the…`
  - row 12: `{"briefing":"Mattis Cray explains the debt from the first person he ever vouched for who burned him. Paying down that lingering obligation is the only way he can ever offer a second clean vouch.","choices":[{"id":"mattis_debt_cleared","set_flag":"flag_crossing_mattis_redeemed","text":"Pay the debt in full and clear Mattis's standing."}],"display_name":"The Name He Gave","id":"quest_crossing_companion_mattis","knowledge_key":"lore_nc_the_vouch","min_day":80,"prereq_quest_id":"quest_crossing_the_vouch","stages":[{"id":"speak_mattis","text":"Speak to Mattis alone under the viaduct arch."},{"id":"resolve_old_debt","text":"Settle the old grain de…`
  - row 13: `{"briefing":"Corporal Kael of the Central Garrison has abandoned his post at Checkpoint Gamma and seeks asylum under the Viaduct Truss. Garrison enforcers demand his immediate extradition under threat of mortar bombardment.","choices":[{"id":"grant_asylum","set_flag":"flag_crossing_kael_asylum_granted","text":"Grant political sanctuary under Section 9 of the Charter. Garrison standing drops -5."},{"id":"extradite_kael","set_flag":"flag_crossing_kael_extradited","text":"Extradite Kael to the garrison enforcers in exchange for a crate of 7.62mm ammunition."}],"display_name":"The Deserters Plea","id":"quest_crossing_asylum_in_the_truss","knowle…`
  - row 14: `{"briefing":"A merchant caravan attempting to enter Stallrow is carrying thirty uninspected ampoules of broad-spectrum antibiotics concealed inside sealed butter crocks.","choices":[{"id":"confiscate_to_clinic","set_flag":"flag_crossing_medicine_confiscated","text":"Confiscate the medicine and transfer it directly to the crossing clinic."},{"id":"levy_heavy_duty","set_flag":"flag_crossing_medicine_taxed","text":"Charge a fifty-percent import tariff in salt rations and permit sale."}],"display_name":"The Waxed Ampoules","id":"quest_crossing_contraband_medical_vial","knowledge_key":"","min_day":80,"prereq_quest_id":"quest_crossing_first_weigh"…`
  - row 15: `{"briefing":"A heavy six-wheel flatbed hauler sits impounded in the Underwrite Lockup. The Scale claims a debt mortgage, while the Cutters claim salvage rights after towing it off the ice.","choices":[{"id":"award_to_cutters","set_flag":"flag_crossing_rig_to_cutters","text":"Award the hauler to the Cutters as legitimate ice salvage."},{"id":"award_to_scale","set_flag":"flag_crossing_rig_to_scale","text":"Enforce the Scales mortgage lien and return the hauler upon debt settlement."}],"display_name":"The Impounded Rig","id":"quest_crossing_vehicle_lien_arbitration","knowledge_key":"","min_day":85,"prereq_quest_id":"quest_crossing_the_marker","…`
  - row 16: `{"briefing":"A family of five displaced from the southern ruins arrives at the weighbridge. Their total possessions fall three kilograms short of the statutory crossing bond.","choices":[{"id":"sponsor_family","set_flag":"flag_crossing_family_sponsored","text":"Pledge personal ration credit to cover their crossing bond."},{"id":"refuse_entry","set_flag":"flag_crossing_family_refused","text":"Enforce the strict regulation and deny passage across the viaduct."}],"display_name":"The Family on the Ramp","id":"quest_crossing_displaced_kin_roll","knowledge_key":"","min_day":90,"prereq_quest_id":"quest_crossing_first_weigh","stages":[{"id":"stage_1…`
  - row 17: `{"briefing":"A batch of waterlogged barley from the coastal flats slipped past the health inspection point, causing acute dysentery among twelve stallholders.","choices":[{"id":"banish_merchant","set_flag":"flag_crossing_vane_banished","text":"Burn the spoiled grain and permanently banish the merchant from the market."},{"id":"fine_and_cure","set_flag":"flag_crossing_vane_fined","text":"Impose a heavy medical restitution fine to pay for survivor treatment."}],"display_name":"Fever at the Sump","id":"quest_crossing_quarantine_breach_trial","knowledge_key":"","min_day":95,"prereq_quest_id":"quest_crossing_first_weigh","stages":[{"id":"stage_1"…`
  - row 18: `{"briefing":"A Flotilla sloop carrying dried fish and brine salt has moored at the viaduct pilings without paying mooring duties, claiming maritime emergency status.","choices":[{"id":"allow_emergency_dock","set_flag":"flag_crossing_sloop_moored_free","text":"Grant forty-eight hours of free mooring while rudder repairs are made."},{"id":"charge_fish_tithe","set_flag":"flag_crossing_sloop_tithed","text":"Demand ten kilograms of dried fish as mooring fee before opening the slip."}],"display_name":"The Tied-Up Sloop","id":"quest_crossing_flotilla_docking_rights","knowledge_key":"","min_day":100,"prereq_quest_id":"quest_crossing_the_vouch","stag…`
  - row 19: `{"briefing":"The Foundry has placed an embargo on raw scrap shipments to the western camps. A convoy of three armored buggies asks for secret passage through the viaduct underpass.","choices":[{"id":"enforce_foundry_embargo","set_flag":"flag_crossing_embargo_upheld","text":"Turn the convoy back and uphold the treaty obligations."},{"id":"smuggle_parts_through","set_flag":"flag_crossing_embargo_broken","text":"Open the night sluice and let the lathe parts reach the farmers."}],"display_name":"The Blockade Runner","id":"quest_crossing_embargo_transit_escort","knowledge_key":"","min_day":105,"prereq_quest_id":"quest_crossing_the_marker","stages…`
  - row 20: `{"briefing":"A catastrophic radioactive dust storm is blowing in from Ground Zero. Convene an emergency quorum of faction representatives to decide whether to seal the viaduct gates entirely.","choices":[{"id":"vote_total_lockdown","set_flag":"flag_crossing_total_lockdown","text":"Drop the iron portcullises and seal the viaduct for ten days."},{"id":"vote_stay_open_filtered","set_flag":"flag_crossing_filtered_passage","text":"Keep the filtered lower tunnel open with mandatory decontaminations."}],"display_name":"The Quorum on the Span","id":"quest_crossing_the_null_charter_vote","knowledge_key":"","min_day":110,"prereq_quest_id":"quest_cross…`
  - row 21: `{"briefing":"A pre-war non-aggression covenant signed by the river baronies was preserved in a lead tube. The water wardens seek to ratify its terms once more.","choices":[{"id":"choice_ratify_accord","moral_delta":5,"set_flag":"flag_covenant_salvaged_accord_active","text":"Ratify the salvaged accord with water seal."},{"id":"choice_repudiate_accord","moral_delta":-5,"set_flag":"flag_covenant_salvaged_accord_breached","text":"Repudiate the accord and break the seal."}],"display_name":"The Salvaged Accord","id":"quest_crossing_the_salvaged_accord","knowledge_key":"","min_day":25,"prereq_quest_id":"quest_crossing_the_vouch","stages":[{"id":"re…`
  - row 22: `{"briefing":"Two convoy lineages claim the same registration ledger at Stallrow. Without arbitration, supply wagons will remain impounded on the causeway.","choices":[{"id":"choice_uphold_registry","moral_delta":5,"set_flag":"flag_dispute_registry_claim_resolved","text":"Uphold the elder lineage's claim under the charter."},{"id":"choice_escalate_registry","moral_delta":-5,"set_flag":"flag_dispute_registry_claim_escalated","text":"Seize the disputed cargo for the crossing arsenal."}],"display_name":"The Registry Dispute","id":"quest_crossing_the_registry_dispute","knowledge_key":"","min_day":35,"prereq_quest_id":"quest_crossing_the_salvaged_…`
  - row 23: `{"briefing":"The crossing gate requires upkeep that scrap tithes no longer cover. The gatekeepers demand a permanent salt covenant from all outbound caravans.","choices":[{"id":"choice_endow_toll","moral_delta":5,"set_flag":"flag_covenant_bridge_toll_active","text":"Endow the crossing gate with the salt covenant."},{"id":"choice_reject_toll","moral_delta":-5,"set_flag":"flag_covenant_bridge_toll_breached","text":"Refuse the toll and force open transit."}],"display_name":"The Long Toll","id":"quest_crossing_the_long_toll","knowledge_key":"","min_day":50,"prereq_quest_id":"quest_crossing_the_registry_dispute","stages":[{"id":"inspect_hydraulic…`
- Bytes: 34,791; SHA-256: `94d5932902ac39bdac0f2caa6c7e56a1bdb33247063e397ca9d2ac5949315734`
- Root keys: `quests, schema_version`
- `quests`: list[23]; union fields: `briefing, choices, display_name, id, knowledge_key, min_day, prereq_quest_id, stages, target_location_id, type`
  - row 1: `{"briefing":"Bram Ostrowski names the Crossing and will sell you a rough sketch of the approach. He will not walk there himself. \"I sold them a map once. That was the whole transaction. I'd like it to stay that way.\"","choices":[{"id":"vouch_ostrowski_reluctant","set_flag":"flag_crossing_vouched_clean","text":"Ostrowski vouches — once, and he would like it to stay that way."},{"id":"vouch_mattis_at_truss","set_flag":"flag_crossing_vouched_clean","text":"Take Mattis Cray's name at the truss and register it on the ledger."}],"display_name":"A Name at the Gate","id":"quest_crossing_the_vouch","knowledge_key":"lore_nc_the_vouch","min_day":70,"…`
  - row 2: `{"briefing":"Osran weighs your goods on the depot scale. The number is real. What people infer from it is not his problem. Present your load, answer his questions about the shelter, and either accept the recorded weight or contest it — honestly, or not.","choices":[{"id":"first_weigh_accept_true","set_flag":"mutation_crossing_honest_trader","text":"Accept the true weight, even when it is less favourable than hoped."},{"id":"first_weigh_contest","set_flag":"mark_crossing_difficult","text":"Contest the true weight anyway. Access still granted; the exchange notes it."},{"id":"first_weigh_bribe","set_flag":"mutation_crossing_bribe_attempted","te…`
  - row 3: `{"briefing":"The scale is honest because it is checked. Help prove that on the record — calibrate against the weight, verify a disputed load, and leave Stallrow able to trust the number without trusting anyone to have set it.","choices":[{"id":"scale_integrity_clear","set_flag":"mutation_crossing_honest_trader","text":"The scale reads clean. Stallrow keeps trusting the number."},{"id":"scale_integrity_silent","set_flag":"flag_crossing_scale_verified_silent","text":"Verify honestly but decline to record it. The trust stays, unspoken."}],"display_name":"What the Weight Doesn't Move","id":"quest_crossing_scale_integrity","knowledge_key":"lore_n…`
  - row 4: `{"briefing":"Dessa Vane offers seed, a covered loss, or a favour bank against a plainly named forfeit. The contract is read twice before it is signed. After the second reading there is only the ink. What you agree to, you will owe — and the forfeit is named up front, not hidden in a clause.","choices":[{"id":"terms_sign_pay","set_flag":"mutation_crossing_underwrite_reliable","text":"Sign and later pay. The Underwrite remembers reliable debtors."},{"id":"terms_decline","set_flag":"flag_crossing_underwrite_untested","text":"Decline the contract. The hall stays open. Dessa notes it without judgement."},{"id":"terms_sign_dump","set_flag":"mutati…`
  - row 5: `{"briefing":"Perrin Ashby asks for an early signature on the Compact's draft charter. If you read it, there is a scoring clause — who gets a vote, and how much it weighs. He has not noticed that it resembles a Reconstruction Utility Rating. He will, if you tell him.","choices":[{"id":"petition_revise","set_flag":"mutation_crossing_petition_revised","text":"Suggest a revision pass. The draft improves. Perrin is grateful, quietly."},{"id":"petition_decline","set_flag":"flag_crossing_petition_unsigned","text":"Decline to sign. The Compact loses an early supporter. Perrin does not hold it against you."},{"id":"petition_leak","set_flag":"mutation…`
  - row 6: `{"briefing":"A dispute at Stallrow needs settling. Three backers must hold the ruling or it is just noise. Learn how the Crossing resolves what no Power will hear: a name spoken in public, by people who have to eat tomorrow, about what is fair today.","choices":[{"id":"standing_back_honest","set_flag":"flag_crossing_standing_honest","text":"Back the side you believe. The ruling will hold honestly."},{"id":"standing_decline","set_flag":"","text":"Decline to back. The ruling may still hold, without your name."},{"id":"standing_bribe","set_flag":"flag_crossing_standing_rigged","text":"Promise both sides or bribe a backer. The ruling holds, but …`
  - row 7: `{"briefing":"The old bronze marker at Nightfire is corroded past the third line. Everyone in the Crossing tells a different story about what was written below the names. Inspect the marker, hear the tellers, and learn where the original paper went.","choices":[{"id":"marker_report_contradictions","set_flag":"flag_crossing_marker_contradictions_noted","text":"Report the contradictions as they are. The mystery remains honest."},{"id":"marker_seed_legend","set_flag":"mutation_crossing_myth_seeded","text":"Seed a tailored legend among the tellers to favor your preferred bloc."}],"display_name":"What the Plaque Doesn't Say","id":"quest_crossing_t…`
  - row 8: `{"briefing":"Wyn Sabler's pledge term has expired. Dessa Vane is coming to collect the exact named forfeit, on the record, in the Underwrite Lockup. Wyn will not ask for help, but she cannot pay alone.","choices":[{"id":"forfeit_pay_debt","set_flag":"mutation_crossing_forfeit_honoured","text":"Pay the grain principal for Wyn. The Underwrite marks the contract fulfilled."},{"id":"forfeit_let_proceed","set_flag":"flag_crossing_forfeit_collected","text":"Let the forfeit proceed. Dessa collects calmly without violence."},{"id":"forfeit_help_flee","set_flag":"mutation_crossing_underwrite_burned","text":"Help Wyn smuggle the pledged sacks out. Des…`
  - row 9: `{"briefing":"Perrin Ashby has called a general gathering to ratify Draft Four of the Compact Charter. Dessa Vane plans to stall the vote by calling in a key backer's overdue debt right on the floor.","choices":[{"id":"vote_cover_debt","set_flag":"mutation_crossing_vote_clean","text":"Cover the claim so the vote proceeds cleanly."},{"id":"vote_sabotage_compact","set_flag":"mutation_crossing_vote_sabotaged","text":"Back the Underwrite's challenge and derail the ratification."}],"display_name":"Draft Four, Called","id":"quest_crossing_the_vote_that_isnt","knowledge_key":"lore_nc_the_standing","min_day":90,"prereq_quest_id":"quest_crossing_the_p…`
  - row 10: `{"briefing":"Ivo Fenn opens the iron box in the Records Room and presents the actual founding Charter. It is only three dry pages: calibration standards, revenue splits, and signatures from people who survived the first winter.","choices":[{"id":"charter_publish_all","set_flag":"mutation_crossing_charter_revealed","text":"Publish the three pages openly on the Stallrow board."},{"id":"charter_keep_hidden","set_flag":"flag_crossing_charter_hidden","text":"Leave the pages locked in the box for history."}],"display_name":"The Charter","id":"quest_crossing_three_dry_pages","knowledge_key":"lore_nc_three_legends","min_day":95,"prereq_quest_id":"qu…`
  - row 11: `{"briefing":"The final Standing is called across the entire Viaduct. Every trade, debt, and ruling you have touched comes together to determine who speaks for the Crossing.","choices":[{"id":"ledger_scale_holds","set_flag":"mutation_crossing_honest_trader","text":"The Scale remains sovereign; fair weights govern the viaduct."},{"id":"ledger_underwrite_holds","set_flag":"mutation_crossing_underwrite_reliable","text":"The Underwrite takes custody of all pledges and traffic."},{"id":"ledger_compact_holds","set_flag":"mutation_crossing_vote_clean","text":"The Compact ratifies Draft Four and establishes a council."}],"display_name":"Who Holds the…`
  - row 12: `{"briefing":"Mattis Cray explains the debt from the first person he ever vouched for who burned him. Paying down that lingering obligation is the only way he can ever offer a second clean vouch.","choices":[{"id":"mattis_debt_cleared","set_flag":"flag_crossing_mattis_redeemed","text":"Pay the debt in full and clear Mattis's standing."}],"display_name":"The Name He Gave","id":"quest_crossing_companion_mattis","knowledge_key":"lore_nc_the_vouch","min_day":80,"prereq_quest_id":"quest_crossing_the_vouch","stages":[{"id":"speak_mattis","text":"Speak to Mattis alone under the viaduct arch."},{"id":"resolve_old_debt","text":"Settle the old grain de…`
  - row 13: `{"briefing":"Corporal Kael of the Central Garrison has abandoned his post at Checkpoint Gamma and seeks asylum under the Viaduct Truss. Garrison enforcers demand his immediate extradition under threat of mortar bombardment.","choices":[{"id":"grant_asylum","set_flag":"flag_crossing_kael_asylum_granted","text":"Grant political sanctuary under Section 9 of the Charter. Garrison standing drops -5."},{"id":"extradite_kael","set_flag":"flag_crossing_kael_extradited","text":"Extradite Kael to the garrison enforcers in exchange for a crate of 7.62mm ammunition."}],"display_name":"The Deserters Plea","id":"quest_crossing_asylum_in_the_truss","knowle…`
  - row 14: `{"briefing":"A merchant caravan attempting to enter Stallrow is carrying thirty uninspected ampoules of broad-spectrum antibiotics concealed inside sealed butter crocks.","choices":[{"id":"confiscate_to_clinic","set_flag":"flag_crossing_medicine_confiscated","text":"Confiscate the medicine and transfer it directly to the crossing clinic."},{"id":"levy_heavy_duty","set_flag":"flag_crossing_medicine_taxed","text":"Charge a fifty-percent import tariff in salt rations and permit sale."}],"display_name":"The Waxed Ampoules","id":"quest_crossing_contraband_medical_vial","knowledge_key":"","min_day":80,"prereq_quest_id":"quest_crossing_first_weigh"…`
  - row 15: `{"briefing":"A heavy six-wheel flatbed hauler sits impounded in the Underwrite Lockup. The Scale claims a debt mortgage, while the Cutters claim salvage rights after towing it off the ice.","choices":[{"id":"award_to_cutters","set_flag":"flag_crossing_rig_to_cutters","text":"Award the hauler to the Cutters as legitimate ice salvage."},{"id":"award_to_scale","set_flag":"flag_crossing_rig_to_scale","text":"Enforce the Scales mortgage lien and return the hauler upon debt settlement."}],"display_name":"The Impounded Rig","id":"quest_crossing_vehicle_lien_arbitration","knowledge_key":"","min_day":85,"prereq_quest_id":"quest_crossing_the_marker","…`
  - row 16: `{"briefing":"A family of five displaced from the southern ruins arrives at the weighbridge. Their total possessions fall three kilograms short of the statutory crossing bond.","choices":[{"id":"sponsor_family","set_flag":"flag_crossing_family_sponsored","text":"Pledge personal ration credit to cover their crossing bond."},{"id":"refuse_entry","set_flag":"flag_crossing_family_refused","text":"Enforce the strict regulation and deny passage across the viaduct."}],"display_name":"The Family on the Ramp","id":"quest_crossing_displaced_kin_roll","knowledge_key":"","min_day":90,"prereq_quest_id":"quest_crossing_first_weigh","stages":[{"id":"stage_1…`
  - row 17: `{"briefing":"A batch of waterlogged barley from the coastal flats slipped past the health inspection point, causing acute dysentery among twelve stallholders.","choices":[{"id":"banish_merchant","set_flag":"flag_crossing_vane_banished","text":"Burn the spoiled grain and permanently banish the merchant from the market."},{"id":"fine_and_cure","set_flag":"flag_crossing_vane_fined","text":"Impose a heavy medical restitution fine to pay for survivor treatment."}],"display_name":"Fever at the Sump","id":"quest_crossing_quarantine_breach_trial","knowledge_key":"","min_day":95,"prereq_quest_id":"quest_crossing_first_weigh","stages":[{"id":"stage_1"…`
  - row 18: `{"briefing":"A Flotilla sloop carrying dried fish and brine salt has moored at the viaduct pilings without paying mooring duties, claiming maritime emergency status.","choices":[{"id":"allow_emergency_dock","set_flag":"flag_crossing_sloop_moored_free","text":"Grant forty-eight hours of free mooring while rudder repairs are made."},{"id":"charge_fish_tithe","set_flag":"flag_crossing_sloop_tithed","text":"Demand ten kilograms of dried fish as mooring fee before opening the slip."}],"display_name":"The Tied-Up Sloop","id":"quest_crossing_flotilla_docking_rights","knowledge_key":"","min_day":100,"prereq_quest_id":"quest_crossing_the_vouch","stag…`
  - row 19: `{"briefing":"The Foundry has placed an embargo on raw scrap shipments to the western camps. A convoy of three armored buggies asks for secret passage through the viaduct underpass.","choices":[{"id":"enforce_foundry_embargo","set_flag":"flag_crossing_embargo_upheld","text":"Turn the convoy back and uphold the treaty obligations."},{"id":"smuggle_parts_through","set_flag":"flag_crossing_embargo_broken","text":"Open the night sluice and let the lathe parts reach the farmers."}],"display_name":"The Blockade Runner","id":"quest_crossing_embargo_transit_escort","knowledge_key":"","min_day":105,"prereq_quest_id":"quest_crossing_the_marker","stages…`
  - row 20: `{"briefing":"A catastrophic radioactive dust storm is blowing in from Ground Zero. Convene an emergency quorum of faction representatives to decide whether to seal the viaduct gates entirely.","choices":[{"id":"vote_total_lockdown","set_flag":"flag_crossing_total_lockdown","text":"Drop the iron portcullises and seal the viaduct for ten days."},{"id":"vote_stay_open_filtered","set_flag":"flag_crossing_filtered_passage","text":"Keep the filtered lower tunnel open with mandatory decontaminations."}],"display_name":"The Quorum on the Span","id":"quest_crossing_the_null_charter_vote","knowledge_key":"","min_day":110,"prereq_quest_id":"quest_cross…`
  - row 21: `{"briefing":"A pre-war non-aggression covenant signed by the river baronies was preserved in a lead tube. The water wardens seek to ratify its terms once more.","choices":[{"id":"choice_ratify_accord","moral_delta":5,"set_flag":"flag_covenant_salvaged_accord_active","text":"Ratify the salvaged accord with water seal."},{"id":"choice_repudiate_accord","moral_delta":-5,"set_flag":"flag_covenant_salvaged_accord_breached","text":"Repudiate the accord and break the seal."}],"display_name":"The Salvaged Accord","id":"quest_crossing_the_salvaged_accord","knowledge_key":"","min_day":25,"prereq_quest_id":"quest_crossing_the_vouch","stages":[{"id":"re…`
  - row 22: `{"briefing":"Two convoy lineages claim the same registration ledger at Stallrow. Without arbitration, supply wagons will remain impounded on the causeway.","choices":[{"id":"choice_uphold_registry","moral_delta":5,"set_flag":"flag_dispute_registry_claim_resolved","text":"Uphold the elder lineage's claim under the charter."},{"id":"choice_escalate_registry","moral_delta":-5,"set_flag":"flag_dispute_registry_claim_escalated","text":"Seize the disputed cargo for the crossing arsenal."}],"display_name":"The Registry Dispute","id":"quest_crossing_the_registry_dispute","knowledge_key":"","min_day":35,"prereq_quest_id":"quest_crossing_the_salvaged_…`
  - row 23: `{"briefing":"The crossing gate requires upkeep that scrap tithes no longer cover. The gatekeepers demand a permanent salt covenant from all outbound caravans.","choices":[{"id":"choice_endow_toll","moral_delta":5,"set_flag":"flag_covenant_bridge_toll_active","text":"Endow the crossing gate with the salt covenant."},{"id":"choice_reject_toll","moral_delta":-5,"set_flag":"flag_covenant_bridge_toll_breached","text":"Refuse the toll and force open transit."}],"display_name":"The Long Toll","id":"quest_crossing_the_long_toll","knowledge_key":"","min_day":50,"prereq_quest_id":"quest_crossing_the_registry_dispute","stages":[{"id":"inspect_hydraulic…`

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

### `CrossingCatalogLoader` (18 sampled current references)
- Assets/Ashfall.Core/CrossingCatalog.cs:147: public sealed class CrossingCatalogLoader
- Assets/Ashfall.Core/CrossingCatalog.cs:166: public CrossingCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
- Assets/Ashfall.Core/CrossingHeadlessDemo.cs:79: if (loc.dangerLevel < CrossingCatalogLoader.MinDanger - 0.01f
- Assets/Ashfall.Core/CrossingHeadlessDemo.cs:80: || loc.dangerLevel > CrossingCatalogLoader.MaxDanger + 0.01f)
- Assets/Ashfall.Core/CrossingHeadlessDemo.cs:82: if (loc.baseRadsPerHour < CrossingCatalogLoader.MinRads - 0.01f
- Assets/Ashfall.Core/CrossingHeadlessDemo.cs:83: || loc.baseRadsPerHour > CrossingCatalogLoader.MaxRads + 0.01f)
- Assets/Ashfall.Core/CrossingSession.cs:23: var loader = new CrossingCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), log);
- Ashfall.Core.Tests/CrossingFactionExpansionTests.cs:29: var loader = new CrossingCatalogLoader(files, json);
- Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs:107: var loader = new CrossingCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
- Ashfall.Core.Tests/ExpansionsIntegrationTests.cs:74: CrossingCatalogLoader.MinDanger, CrossingCatalogLoader.MaxDanger);
- Ashfall.Core.Tests/ExpansionsIntegrationTests.cs:76: CrossingCatalogLoader.MinRads, CrossingCatalogLoader.MaxRads);
- Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:195: // Load Crossing Catalog via CrossingCatalogLoader
- Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:196: var crossingLoader = new CrossingCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs:110: var loader = new CrossingCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs:155: var crossingLoader = new CrossingCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs:195: var crossingLoader = new CrossingCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs:91: var loader = new CrossingCatalogLoader(files, json);
- Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs:162: var crossing = new CrossingCatalogLoader(files, json).Load(DataDirectory);
### `CrossingItemEntry` (4 sampled current references)
- Assets/Ashfall.Core/CrossingCatalog.cs:67: public class CrossingItemEntry
- Assets/Ashfall.Core/CrossingCatalog.cs:118: public List<CrossingItemEntry> Items { get; } = new List<CrossingItemEntry>();
- Assets/Ashfall.Core/CrossingCatalog.cs:125: public CrossingItemEntry? GetItem(string id) => Find(Items, id, item => item.id);
- Assets/Ashfall.Core/CrossingCatalog.cs:185: catalog.Items.AddRange(LoadList<CrossingItemEntry>(_files.Combine(dataDirectory, ItemsFile), "items"));
### `CrossingQuestSystem` (18 sampled current references)
- Assets/Ashfall.Core/ExpansionHubSave.cs:42: public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
- Assets/Ashfall.Core/ExpansionHubSave.cs:75: public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
- Assets/Ashfall.Core/ExpansionHubSave.cs:100: public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
- Assets/Ashfall.Core/ExpansionHubSave.cs:119: public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
- Assets/Ashfall.Core/ExpansionHubSave.cs:139: public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
- Assets/Ashfall.Core/ExpansionHubSave.cs:161: public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
- Assets/Ashfall.Core/ExpansionHubSave.cs:186: CrossingQuestSystem? crossingQuests = null,
- Assets/Ashfall.Core/ExpansionHubSave.cs:258: crossingQuests = v1.crossingQuests ?? new CrossingQuestSystemState(),
- Assets/Ashfall.Core/ExpansionHubSave.cs:286: crossingQuests = v2.crossingQuests ?? new CrossingQuestSystemState(),
- Assets/Ashfall.Core/ExpansionHubSave.cs:314: crossingQuests = v3.crossingQuests ?? new CrossingQuestSystemState(),
- Assets/Ashfall.Core/ExpansionHubSave.cs:342: crossingQuests = v4.crossingQuests ?? new CrossingQuestSystemState(),
- Assets/Ashfall.Core/ExpansionHubSave.cs:373: crossingQuests = v5.crossingQuests ?? new CrossingQuestSystemState(),
- Assets/Ashfall.Core/ExpansionHubSave.cs:459: CrossingQuestSystem? crossingQuests = null,
- Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:68: public class CrossingQuestSystemState
- Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:70: public string systemId = CrossingQuestSystem.SystemId;
- Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:85: public class CrossingQuestSystem
- Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:90: private CrossingQuestSystemState _state = new();
- Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:101: public event Action<CrossingQuestSystemState> OnStateChanged;
### `ExpansionHostSession` (18 sampled current references)
- src/Main.ExpansionHub.cs:34: private ExpansionHostSession _expansions = null!;
- src/Main.ExpansionHub.cs:54: _expansions = ExpansionHostSession.Create(
- src/Main.OrphanSealWave1.cs:40: private ShelterExpansionHostSession? _shelterExpansion;
- src/Main.OrphanSealWave1.cs:253: _shelterExpansion = new ShelterExpansionHostSession(system);
- src/Host/ExpansionHostSession.cs:19: public sealed class ExpansionHostSession
- src/Host/ExpansionHostSession.cs:51: public ExpansionHostSession(
- src/Host/ExpansionHostSession.cs:127: public static ExpansionHostSession Create(
- src/Host/ExpansionHostSession.cs:144: var session = new ExpansionHostSession(
- src/Host/ExpansionHostSession.cs:201: log.Error("[ExpansionHostSession] debt catalog: " + session.DebtCatalog.Errors[i]);
- src/Host/DoseLedgerHostSession.cs:17: /// ExpansionHostSession pattern. Persistence via DoseLedgerSaveStore.
- src/Host/OrphanSealWave1HostSessions.cs:395: public sealed class ShelterExpansionHostSession : HostSessionBase
- src/Host/OrphanSealWave1HostSessions.cs:399: public ShelterExpansionHostSession(ShelterExpansionSystem system)
- src/Host/ShelterOperationsHostSession.cs:28: public ShelterExpansionHostSession Construction { get; }
- src/Host/ShelterOperationsHostSession.cs:33: ShelterExpansionHostSession construction,
- src/Host/HostCli.PanelTests.cs:322: var session = ExpansionHostSession.Create(dataDirectory);
- src/Host/HostCli.PanelTests.cs:354: var fresh = ExpansionHostSession.Create(dataDirectory);
- src/UI/CrossingQuestPanel.cs:16: /// through ExpansionHostSession; this panel is purely presentational.
- src/UI/CrossingQuestPanel.cs:22: private ExpansionHostSession? _expansions;
### `CrossingQuestPanel` (18 sampled current references)
- src/Main.PanelLifecycle.cs:63: _crossingQuestPanel,
- src/Main.PanelLifecycle.cs:240: private void CloseCrossingQuestPanel()
- src/Main.PanelLifecycle.cs:242: if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(_crossingQuestPanel))
- src/Main.PanelLifecycle.cs:243: _crossingQuestPanel.Visible = false;
- src/Main.UiHandlers.cs:259: public void OpenCrossingQuestPanel()
- src/Main.UiHandlers.cs:262: _crossingQuestPanel.Bind(_expansions, _expansions.Vouch, _simDay);
- src/Main.UiHandlers.cs:263: _crossingQuestPanel.Open();
- src/Main.UiPanels.cs:77: private CrossingQuestPanel _crossingQuestPanel = null!;
- src/Main.UiPanels.cs:388: _questsPanel.OnCrossingPanelRequested += OpenCrossingQuestPanel;
- src/Main.UiPanels.cs:816: _crossingQuestPanel = new CrossingQuestPanel();
- src/Main.UiPanels.cs:817: _crossingQuestPanel.OnClose += CloseCrossingQuestPanel;
- src/Main.UiPanels.cs:818: AddChild(_crossingQuestPanel);
- src/Main.GameFlow.cs:639: _crossingQuestPanel.Bind(_expansions, _expansions?.Vouch, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
- src/Main.GameFlow.cs:640: _crossingQuestPanel.Open();
- src/Main.PlayerSurfaces.cs:434: bindAction: () => { SetupExpansions(); _crossingQuestPanel.Bind(_expansions, _expansions?.Vouch, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay); },
- src/Main.PlayerSurfaces.cs:435: openAction: () => _crossingQuestPanel.Open(),
- src/Main.PlayerSurfaces.cs:436: closeAction: () => CloseCrossingQuestPanel());
- src/UI/CrossingQuestPanel.cs:18: public partial class CrossingQuestPanel : Control, IBindablePanel

# Appendix E — Current focused-test inventory

Current test declaration inventory: 62 sampled declarations across 4 named targets. Declaration presence is not a fresh pass claim.
### `Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs` — 14 test declarations; bytes=9,850; SHA-256=`ed286004dcd14d3885275fbff1ed4a6a3663bff6d4de7182b9de7c712fb68811`
- 00102: [Fact]
- 00103: public void CrossingCatalog_ContainsExactlyTwentyFiveItems()
- 00112: [Fact]
- 00113: public void CrossingCatalog_PreservesOriginalElevenAndAddsExactlyFourteen()
- 00126: [Fact]
- 00127: public void CrossingCatalog_AllEntriesUseSupportedTypesAndNumericRanges()
- 00144: [Fact]
- 00145: public void CrossingCatalog_OriginalNumericDefinitionsRemainUnchanged()
- 00177: [Fact]
- 00178: public void GlobalCatalog_RegistersAllFourteenNewItems()
- 00185: [Fact]
- 00186: public void GlobalCatalog_UsesCanonicalConsumableSemantics()
- 00207: [Fact]
- 00208: public void ProposedIdsDoNotCollideAcrossGlobalItemFiles()
### `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs` — 32 test declarations; bytes=15,117; SHA-256=`1bb5e0d2b6d7c7297e4ef0ef0c990f1f93660599bdea2268843187d64f958520`
- 00033: [Fact]
- 00034: public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
- 00041: [Fact]
- 00042: public void BaselineThreeFactions_PreservedVerbatim()
- 00089: [Fact]
- 00090: public void NewFiveFactions_ArePresentWithExpectedAttributes()
- 00160: [Fact]
- 00161: public void FactionIds_AreUnique_AndStartWithPrefix()
- 00177: [Fact]
- 00178: public void DisplayNames_AreNonEmpty_AndDistinct()
- 00192: [Fact]
- 00193: public void Alignments_AreValid()
- 00207: [Fact]
- 00208: public void HomeRegions_AreValidCrossingRegion()
- 00217: [Fact]
- 00218: public void IsActive_IsTrueForAll()
- 00227: [Fact]
- 00228: public void Trust_ValuesAreWithinValidRange()
- 00237: [Fact]
- 00238: public void Wants_AreNonEmptyArrays_WithValidNonEmptyItems()
- 00253: [Fact]
- 00254: public void Offers_AreNonEmptyArrays_WithValidNonEmptyItems()
- 00269: [Fact]
- 00270: public void SignatureQuotes_AreNonEmpty_SingleSentences()
- 00282: [Fact]
- 00283: public void AccessRules_AreNonEmpty_AndSubstantive()
- 00294: [Fact]
- 00295: public void TradeProfiles_AreDistinctAcrossAllEight()
- 00316: [Fact]
- 00317: public void CrossingIds_ConstantsMatchDataCatalog()
- 00340: [Fact]
- 00341: public void FactionIconCatalog_ResolvesOrFallsBackSafely()
### `Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs` — 8 test declarations; bytes=11,079; SHA-256=`aa4f405426b00ae803ede9eb65259a230dcd601dee7ed142ef8d794a56f1beb2`
- 00086: [Fact]
- 00087: public void Plan126_CrossingItems_LoadsExactTwentyFiveItemsAndValidatesExpansionTier()
- 00115: [Fact]
- 00116: public void Plan129_FoundryProduction_LoadsExactThirtyFiveProductsAndValidatesNineExpansionProducts()
- 00157: [Fact]
- 00158: public void Plan126_129_CrossDomainCoherence_IndustrialSupplyChainAndBorderCommerceLinkages()
- 00193: [Fact]
- 00194: public void Plan126_129_DeterministicInventoryTradeAndProductionSimulation()
### `Ashfall.Core.Tests/World/Plan118_120StandingCrossingIntegrationTests.cs` — 8 test declarations; bytes=11,493; SHA-256=`36858a772bf3a9b288f691c8ce875f00e2dd2d87b2535111569166067220e739`
- 00053: [Fact]
- 00054: public void Plan118_StandingRecordQuests_LoadsAllAuthoredQuestsAndVerifiesPlan118TenExpansionQuests()
- 00105: [Fact]
- 00106: public void Plan120_CrossingFactions_LoadsExactEightFactionsWithDistinctTradeProfiles()
- 00149: [Fact]
- 00150: public void Plan118_120_CrossDomainCoherence_BorderTerritoryAndLamplighterLinkages()
- 00186: [Fact]
- 00187: public void Plan118_120_DeterministicQuestChoiceResolutionAndFactionTrust()

# Appendix H/I/J — Deep polishing and final precision passes

# Appendix H — Deep polishing pass 1: content, premise, and evidence depth

**Pass intent:** improve `Crossing Items: Twenty-Five Catalog Entries, Inventory Semantics, and Crossing Expansion Ownership` without inflating row counts or reopening sealed architecture. The pass asks whether every historical verb (“expand”, “wire”, “save”, “autonomous”, “completed”) matches a current declaration, caller, or explicitly labeled residual.

## H.1 Content corrections
- The historical plan calls all item effects live and wires grants broadly; current evidence requires consumer proof.
- The historical plan proposes new item ids without proving canonical inventory admission.

## H.2 Evidence-strength corrections
- Separate metadata from mechanics.
- Trace actual quest/encounter/inventory consumers.
- Preserve atomic custody and expansion persistence.

## H.3 Anti-filler gate
- Remove generated “100 tests”, “600-day trace”, fictional dossiers, and repeated variants unless the named current file or catalog actually contains the corresponding evidence.
- A long source appendix is acceptable only when every included file is a current owner, loader, host, UI, data, or focused-test seam. It is not permission to duplicate the same file or paste unrelated code.
- Keep historical ledger claims in a historical column. Never convert an old PASS count into a current verification statement.

# Appendix I — Deep polishing pass 2: integration architecture and code seams

**Pass intent:** make the next builder’s route executable for Crossing Items: Twenty-Five Catalog Entries, Inventory Semantics, and Crossing Expansion Ownership while preserving one authority per concern. The route is data → loader/validator → Core owner → existing save section → host adapter → event/fact → UI projection → focused verification.

## I.1 Architectural decisions
- Use CrossingCatalog for Crossing-specific definitions.
- Use CrossingQuestSystem/Arbitration for Crossing state.
- Use Inventory/ItemCatalogLoader for physical custody and effects.
- Use expansion-hub persistence for existing Crossing state.
- Use current panels as projections only.

## I.2 Host and presentation contract
- The Godot layer may compose `the current host owner`, bind providers, route commands, and render truthful state. It may not reimplement crossing items: twenty-five catalog entries, inventory semantics, and crossing expansion ownership arithmetic or persist a shadow copy.
- Shared panel registries, `Main` composition roots, save orchestrators, and generated indexes remain integrator-owned unless a future package claims them exactly.

## I.3 Code-level seam checklist
- Confirm the exact current public method and field names from the declaration indexes in Appendix C before writing code.
- Confirm the current save section/store and restore path by reading the owner and its host façade; do not infer persistence from a `CaptureState` method alone.
- Confirm event ordering and exactly-once semantics at the first mutation edge; a panel refresh is not an event producer.
- Keep deterministic collections ordinal-stable, use existing `ISeededRng` streams only where the owner already requires randomness, and use invariant formatting for checksums.

# Appendix J — Final precision, reaccuracy, and full repolishing phase

This pass is intentionally performed after the architecture pass. It re-reads the current source/data hashes, checks every named path, removes stale terminology, downgrades unsupported claims, and records the exact bounded residual. It is the final full repolishing phase: it does not add scope, but it does reconcile the entire plan against current authority before handoff.

## J.1 Final corrections applied
- No static row is treated as a physical item until Inventory accepts it.
- No new Crossing inventory or save owner is proposed.

## J.2 Questions deliberately left open
- Which Crossing items are intended to be quest tokens versus tradable stock?
- Should Crossing rows be added to the canonical item catalog or remain a typed overlay?

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

The original file at `HEAD:piagentsplans/126-crossing-items-expansion.md` contained 4,909 characters. It is retained as provenance, not as current implementation authority. The generated working-tree expansion is superseded by this rebase.

```markdown
# Plan 126 — Crossing Items Expansion (11 → 25 items)

## Goal (2 lines)
Expand `crossing_items.json` from 11 items to 25. The Crossing expansion's
item catalog (`ItemCatalogLoader.cs` confirmed live) defines
Crossing-specific items with full item properties (type, weight, trade value,
thirst/hunger/morale effects). 11 items for the charter settlement's economy
is thin; the Crossing's trade, arbitration, and black-market themes need
more unique items.

## Why (P2)
- Verified: `crossing_items.json` has 11 items in `items` array. Each has
  id, displayName, description, type, stackMax, weight, tradeValue,
  thirstRestore, hungerRestore, moraleEffect. `ItemCatalogLoader.cs`
  loads it.
- The Crossing is a trade-and-arbitration settlement. 11 items means the
  Crossing economy is sparse — the factions (Plan 120) want and offer
  items that don't exist yet, and the crises (Plan 115) reference items
  that aren't there.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/crossing_items.json` (expand `items` 11 → 25)
- Read-only: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` (confirm
  item DTO and valid `type` values)

## Content grammar (per item)
- `id`: snake_case, prefix `item_` (confirmed convention).
- `displayName`: evocative item name.
- `description`: 2–4 sentences in the established Crossing voice.
- `type`: item type string (confirm valid set in step 1 — Quest,
  Consumable, Tool, Trade, etc.).
- `stackMax`: integer max stack size.
- `weight`: float weight.
- `tradeValue`: integer trade value.
- `thirstRestore` / `hungerRestore` / `moraleEffect`: integer/float
  effects (0 if none).

## Steps
1. Read `ItemCatalogLoader.cs` to confirm the item DTO and all valid
   `type` values.
2. Inventory the 11 existing items. Identify which Crossing themes
   (trade, arbitration, black market, water, food, fuel, documents) lack
   items.
3. Author 14 new items:
   - `item_arbitration_token`: a token granting one arbitration hearing;
     Quest type, high trade value.
   - `item_charter_stamp`: an official stamp that validates a charter
     document; Quest type.
   - `item_weighbridge_chit`: a chit from the weighbridge recording a
     verified weight; Trade type.
   - `item_smuggled_medicine`: off-ledger medicine; Consumable, restores
     health, high value.
   - `item_crossing_bread`: dense ration bread baked at the granary;
     Consumable, restores hunger.
   - `item_lamp_oil_crossing`: Crossing-sourced lamp oil; Tool/Fuel type.
   - `item_filtered_water_crossing`: water from the Crossing committee's
     filtration; Consumable, restores thirst.
   - `item_quarantine_bands`: colored bands marking disease screening
     status; Quest type.
   - `item_granary_receipt`: a receipt for grain stored in the communal
     granary; Quest/Trade type.
   - `item_smugglers_ledger`: an off-ledger trade record; Quest type,
     contraband.
   - `item_rejection_notice`: an official notice of rejected
     arbitration; Quest type.
   - `item_crossing_map`: a map of the Crossing's internal routes and
     back channels; Tool type.
   - `item_black_market_pouch`: a pouch for carrying off-ledger goods
     discreetly; Tool type.
   - `item_charter_draft`: a draft charter amendment; Quest type, high
     political value.
4. Each item: distinct type, balanced weight/tradeValue/effects,
   description in the Crossing voice.
5. Cross-reference: every item id unique; every id follows `item_` prefix;
   no two items share the same displayName.
6. Wire 4 items to Plan 120 (crossing factions — factions want/offer new
  items).
7. Wire 3 items to Plan 115 (crossing encounters — encounters grant/
  require items).
8. Wire 2 items to Plan 126's sibling plans (Holdfast/Verdict items
  where Crossing items appear in cross-expansion trade).
9. Validate: `--data-integrity-selftest` (all item ids resolve).
10. xUnit: Crossing item catalog loads 25 items, all ids unique, all
    types valid, all descriptions non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `type` validation (step 1): if the item
loader enforces a specific enum, invalid types will fail. Confirm the
valid set before authoring.

## Definition of Done
- `crossing_items.json` has 25 items, all ids unique, all types valid, 4
  wired to crossing factions, 3 to crossing encounters, integrity +
  tests green.

## Follow-on
- Plan 120 (crossing factions) — factions want/offer new items.
- Plan 115 (crossing encounters) — encounters grant/require items.
- Plan 116 (deep lore locations) — Crossing items appear in Crossing
  location loot tables.
- Plan 99 (hardcore economy tuning) — Crossing items get price tiers.
- Plan 105 (trade specialties) — Crossing items match profession patterns.

```

## End of Plan 126 — current-evidence rebase

# Appendix C — Current source and test evidence (verbatim, bounded)

Each item below is an evidence snapshot, not a proposed replacement. A bounded excerpt is explicitly marked; the SHA-256 identifies the complete current file. Paths are read-only for this planning package.

## `Assets/Ashfall.Core/CrossingCatalog.cs` — 429 lines; 19,786 bytes; SHA-256 `4f6d4ec15b065b40aa0f44a27c03ce0d190c2387e6f2124830e93ca3ac57e043`
Declaration index:
- 00009: public class CrossingFactionEntry
- 00024: public class CrossingLocationEntry
- 00038: public class CrossingQuestStageEntry
- 00044: public class CrossingQuestChoiceEntry
- 00051: public class CrossingQuestEntry
- 00067: public class CrossingItemEntry
- 00081: public class CrossingChoiceEntry
- 00088: public class CrossingEncounterEntry
- 00098: public class CrossingCrisisEntry
- 00107: public class CrossingEncountersContainer
- 00113: public sealed class CrossingCatalog
- 00122: public CrossingFactionEntry? GetFaction(string id) => Find(Factions, id, f => f.id);
- 00123: public CrossingLocationEntry? GetLocation(string id) => Find(Locations, id, e => e.id);
- 00124: public CrossingQuestEntry? GetQuest(string id) => Find(Quests, id, q => q.id);
- 00125: public CrossingItemEntry? GetItem(string id) => Find(Items, id, item => item.id);
- 00126: public CrossingEncounterEntry? GetEncounter(string id) => Find(Encounters, id, enc => enc.id);
- 00127: public CrossingCrisisEntry? GetCrisis(string id) => Find(Crises, id, c => c.id);
- 00147: public sealed class CrossingCatalogLoader
- 00173: public CrossingCatalog Load(string dataDirectory)
- 00190: private void LoadEncountersAndCrises(string path, CrossingCatalog catalog)
- 00264: public static class CrossingIds
- 00290: public static class Quests
- 00307: public static class Locations
- 00325: public static class Items
- 00341: public static class Flags
- 00357: public static class Npcs
- 00368: public static class Knowledge
- 00379: public static class Mutations
- 00395: public static class Endings
- 00405: public static class Encounters
- 00420: public static class Crises
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using System.Text.Json;
00006:
00007: namespace Ashfall.Core
00008: {
00009:     public class CrossingFactionEntry
00010:     {
00011:         public string id;
00012:         public string display_name;
00013:         public string alignment;
00014:         public string home_region;
00015:         public bool is_active;
00016:         public float trust;
00017:         public string[] wants;
00018:         public string[] offers;
00019:         public string signature_quote;
00020:         public string access_rule;
00021:         public string badge_asset_id;
00022:     }
00023:
00024:     public class CrossingLocationEntry
00025:     {
00026:         public string id;
00027:         public string displayName;
00028:         public string inspect;
00029:         public string description;
00030:         public float dangerLevel;
00031:         public float travelHours;
00032:         public float baseRadsPerHour;
00033:         public string region;
00034:         public bool overlay_on_unlock;
00035:         public bool recast_always;
00036:     }
00037:
00038:     public class CrossingQuestStageEntry
00039:     {
00040:         public string id;
00041:         public string text;
00042:     }
00043:
00044:     public class CrossingQuestChoiceEntry
00045:     {
00046:         public string id;
00047:         public string text;
00048:         public string set_flag;
00049:     }
00050:
00051:     public class CrossingQuestEntry
00052:     {
00053:         public string id;
00054:         public string display_name;
00055:         public string type;
00056:         public string briefing;
00057:         public string prereq_quest_id;
00058:         public int min_day;
00059:         public CrossingQuestStageEntry[] stages;
00060:         public CrossingQuestChoiceEntry[] choices;
00061:         public string knowledge_key;
00062:         public string target_location_id;
00063:
00064:         public int StageCount => stages != null ? stages.Length : 0;
00065:     }
00066:
00067:     public class CrossingItemEntry
00068:     {
00069:         public string id;
00070:         public string displayName;
00071:         public string description;
00072:         public string type;
00073:         public int stackMax;
00074:         public float weight;
00075:         public float tradeValue;
00076:         public float thirstRestore;
00077:         public float hungerRestore;
00078:         public float moraleEffect;
00079:     }
00080:
00081:     public class CrossingChoiceEntry
00082:     {
00083:         public string text;
00084:         public string[] cost_items;
00085:         public string result;
00086:     }
00087:
00088:     public class CrossingEncounterEntry
00089:     {
00090:         public string id;
00091:         public string name;
00092:         public string target_location;
00093:         public string description;
00094:         public string threat_level;
00095:         public CrossingChoiceEntry[] choices;
00096:     }
00097:
00098:     public class CrossingCrisisEntry
00099:     {
00100:         public string id;
00101:         public string name;
00102:         public string[] phases;
00103:         public string description;
00104:         public string resolution;
00105:     }
00106:
00107:     public class CrossingEncountersContainer
00108:     {
00109:         public CrossingEncounterEntry[] encounters;
00110:         public CrossingCrisisEntry[] crises;
00111:     }
00112:
00113:     public sealed class CrossingCatalog
00114:     {
00115:         public List<CrossingFactionEntry> Factions { get; } = new List<CrossingFactionEntry>();
00116:         public List<CrossingLocationEntry> Locations { get; } = new List<CrossingLocationEntry>();
00117:         public List<CrossingQuestEntry> Quests { get; } = new List<CrossingQuestEntry>();
00118:         public List<CrossingItemEntry> Items { get; } = new List<CrossingItemEntry>();
00119:         public List<CrossingEncounterEntry> Encounters { get; } = new List<CrossingEncounterEntry>();
00120:         public List<CrossingCrisisEntry> Crises { get; } = new List<CrossingCrisisEntry>();
00121:
00122:         public CrossingFactionEntry? GetFaction(string id) => Find(Factions, id, f => f.id);
00123:         public CrossingLocationEntry? GetLocation(string id) => Find(Locations, id, e => e.id);
00124:         public CrossingQuestEntry? GetQuest(string id) => Find(Quests, id, q => q.id);
00125:         public CrossingItemEntry? GetItem(string id) => Find(Items, id, item => item.id);
00126:         public CrossingEncounterEntry? GetEncounter(string id) => Find(Encounters, id, enc => enc.id);
00127:         public CrossingCrisisEntry? GetCrisis(string id) => Find(Crises, id, c => c.id);
00128:
00129:         private static T? Find<T>(List<T> list, string id, Func<T, string> getId) where T : class
00130:         {
00131:             if (string.IsNullOrEmpty(id) || list == null) return null;
00132:             for (int i = 0; i < list.Count; i++)
00133:                 if (list[i] != null && getId(list[i]) == id)
00134:                     return list[i];
00135:             return null;
00136:         }
00137:     }
00138:
00139:     /// <summary>
00140:     /// Loads crossing_*.json from StreamingAssets/Data.
00141:     /// Scale fix (2026-08-14): live catalogs use danger ~1–10 and rads ~18–52.
00142:     /// Crossing cards were authored on a 0–1 / sub-1 scale; they are rescaled
00143:     /// to danger ~3–6 and rads ~8–25 so the depot is not the safest site in the game.
00144:     /// loc_crossing_weighbridge stays a distinct id; display is "The Deck Scale"
00145:     /// so it is not a third Weighbridge thesis beside loc_weighbridge / loc_cut_weigh_hut.
00146:     /// </summary>
00147:     public sealed class CrossingCatalogLoader
00148:     {
00149:         public const string FactionsFile = "crossing_factions.json";
00150:         public const string LocationsFile = "crossing_locations.json";
00151:         public const string QuestsFile = "crossing_quests.json";
00152:         public const string ItemsFile = "crossing_items.json";
00153:         public const string EncountersFile = "crossing_encounters.json";
00154:
00155:         /// <summary>Live schema: danger 3–6 after the unit fix.</summary>
00156:         public const float MinDanger = 3f;
00157:         public const float MaxDanger = 6f;
00158:         /// <summary>Live schema: rads 8–25 after the unit fix.</summary>
00159:         public const float MinRads = 8f;
00160:         public const float MaxRads = 25f;
00161:
00162:         private readonly IFileIO _files;
00163:         private readonly IJsonSerializer _json;
00164:         private readonly ILog _log;
00165:
00166:         public CrossingCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
00167:         {
00168:             _files = files ?? throw new ArgumentNullException(nameof(files));
00169:             _json = json ?? throw new ArgumentNullException(nameof(json));
00170:             _log = log ?? NullLog.Instance;
00171:         }
00172:
00173:         public CrossingCatalog Load(string dataDirectory)
00174:         {
00175:             var catalog = new CrossingCatalog();
00176:             if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
00177:             {
00178:                 _log.Warn("Crossing catalog directory missing: " + dataDirectory);
00179:                 return catalog;
00180:             }
00181:
00182:             catalog.Factions.AddRange(LoadList<CrossingFactionEntry>(_files.Combine(dataDirectory, FactionsFile), "factions"));
00183:             catalog.Locations.AddRange(LoadList<CrossingLocationEntry>(_files.Combine(dataDirectory, LocationsFile), "locations"));
00184:             catalog.Quests.AddRange(LoadList<CrossingQuestEntry>(_files.Combine(dataDirectory, QuestsFile), "quests"));
00185:             catalog.Items.AddRange(LoadList<CrossingItemEntry>(_files.Combine(dataDirectory, ItemsFile), "items"));
00186:             LoadEncountersAndCrises(_files.Combine(dataDirectory, EncountersFile), catalog);
00187:             return catalog;
00188:         }
00189:
00190:         private void LoadEncountersAndCrises(string path, CrossingCatalog catalog)
00191:         {
00192:             if (!_files.FileExists(path))
00193:             {
00194:                 _log.Warn("Crossing encounters file missing: " + path);
00195:                 return;
00196:             }
00197:
00198:             try
00199:             {
00200:                 string json = _files.ReadAllText(path);
00201:                 var container = _json.Deserialize<CrossingEncountersContainer>(json);
00202:                 if (container?.encounters != null)
00203:                 {
00204:                     for (int i = 0; i < container.encounters.Length; i++)
00205:                     {
00206:                         if (container.encounters[i] != null)
00207:                             catalog.Encounters.Add(container.encounters[i]);
00208:                     }
00209:                 }
00210:                 if (container?.crises != null)
00211:                 {
00212:                     for (int i = 0; i < container.crises.Length; i++)
00213:                     {
00214:                         if (container.crises[i] != null)
00215:                             catalog.Crises.Add(container.crises[i]);
00216:                     }
00217:                 }
00218:             }
00219:             catch (Exception e)
00220:             {
00221:                 _log.Error("Crossing encounters parse failed: " + e.Message);
00222:             }
00223:         }
00224:
00225:         private List<T> LoadList<T>(string path, string label) where T : class
00226:         {
00227:             if (!_files.FileExists(path))
00228:             {
00229:                 _log.Warn("Crossing " + label + " file missing: " + path);
00230:                 return new List<T>();
00231:             }
00232:
00233:             try
00234:             {
00235:                 string json = _files.ReadAllText(path);
00236:
00237:                 // Support wrapped catalogs: {"schema_version": N, "items"/"locations"/"factions"/"quests": [...]}
00238:                 using var doc = JsonDocument.Parse(json);
00239:                 if (doc.RootElement.ValueKind == JsonValueKind.Object)
00240:                 {
00241:                     foreach (var prop in doc.RootElement.EnumerateObject())
00242:                     {
00243:                         if (prop.Name.Equals("schema_version", StringComparison.OrdinalIgnoreCase))
00244:                             continue;
00245:                         if (prop.Value.ValueKind == JsonValueKind.Array)
00246:                         {
00247:                             var list = CatalogLocator.LoadWrappedList<T>(prop.Value.GetRawText(), SystemTextJsonSerializer.Options);
00248:                             return list ?? new List<T>();
00249:                         }
00250:                     }
00251:                 }
00252:
00253:                 var items = CatalogLocator.LoadWrappedList<T>(json, SystemTextJsonSerializer.Options);
00254:                 return items;
00255:             }
00256:             catch (Exception e)
00257:             {
00258:                 _log.Error("Crossing " + label + " parse failed: " + e.Message);
00259:                 return new List<T>();
00260:             }
00261:         }
00262:     }
00263:
00264:     public static class CrossingIds
00265:     {
00266:         public const string Expansion = "expansion_nobodys_charter";
00267:         public const string Region = "region_crossing";
00268:         public const string TheVouch = "quest_crossing_the_vouch";
00269:         public const string FirstWeigh = "quest_crossing_first_weigh";
00270:         public const string ScaleIntegrity = "quest_crossing_scale_integrity";
00271:         public const string TheStanding = "quest_crossing_the_standing";
00272:         public const string TheTerms = "quest_crossing_the_terms";
00273:         public const string ViaductGate = "loc_crossing_viaduct_gate";
00274:         public const string Scalehouse = "loc_crossing_scalehouse";
00275:         public const string Weighbridge = "loc_crossing_weighbridge";
00276:         public const string NpcMattis = "npc_mattis_cray";
00277:         public const string NpcOsran = "npc_osran_kell";
00278:         public const string NpcWyn = "npc_wyn_sabler";
00279:         public const string NpcIvo = "npc_ivo_fenn";
00280:         public const string FactionScale = "faction_the_scale";
00281:         public const string FactionUnderwrite = "faction_the_underwrite";
00282:         public const string FactionCompact = "faction_the_compact";
00283:         public const string FactionLamplighters = "faction_the_lamplighters";
00284:         public const string FactionGranaryWardens = "faction_the_granary_wardens";
00285:         public const string FactionWaterCommittee = "faction_the_water_committee";
00286:         public const string FactionQuarantinePost = "faction_the_quarantine_post";
00287:         public const string FactionSmugglersCourt = "faction_the_smugglers_court";
00288:
00289:         /// <summary>Canonical quest ids (bible §4.1 main questline + §4.2 side).</summary>
00290:         public static class Quests
00291:         {
00292:             public const string TheVouch   = "quest_crossing_the_vouch";
00293:             public const string FirstWeigh = "quest_crossing_first_weigh";
00294:             public const string TheTerms   = "quest_crossing_the_terms";
00295:             public const string ThePetition= "quest_crossing_the_petition";
00296:             public const string TheStanding= "quest_crossing_the_standing";
00297:             public const string TheMarker  = "quest_crossing_the_marker";
00298:             public const string TheForfeit = "quest_crossing_the_forfeit";
00299:             public const string TheVoteNot = "quest_crossing_the_vote_that_isnt";
00300:             public const string ScaleIntegrity = "quest_crossing_scale_integrity";
00301:             public const string CharterCore = "quest_crossing_three_dry_pages";
00302:             public const string WhoHoldsLedger = "quest_crossing_who_holds_the_ledger";
00303:             public const string CompanionMattis = "quest_crossing_companion_mattis";
00304:         }
00305:
00306:         /// <summary>Canonical location ids (bible §2.4).</summary>
00307:         public static class Locations
00308:         {
00309:             public const string ViaductGate  = "loc_crossing_viaduct_gate";
00310:             public const string Scalehouse   = "loc_crossing_scalehouse";
00311:             public const string Stallrow     = "loc_crossing_stallrow";
00312:             public const string Watchtower   = "loc_crossing_watchtower";
00313:             public const string Weighbridge  = "loc_crossing_weighbridge";
00314:             public const string Underwrite   = "loc_crossing_underwrite_hall";
00315:             public const string RecordsRoom  = "loc_crossing_records_room";
00316:             public const string Nightfire    = "loc_crossing_nightfire";
00317:             public const string TheLockup    = "loc_crossing_the_lockup";
00318:             public const string GranaryPledge = "loc_crossing_granary_pledge";
00319:             public const string PetitionTent = "loc_crossing_petition_tent";
00320:             public const string FoundersMarker = "loc_crossing_founders_marker";
00321:             public const string TheAnnex     = "loc_crossing_the_annex";
00322:         }
00323:
00324:         /// <summary>Canonical item ids (bible §7).</summary>
00325:         public static class Items
00326:         {
00327:             public const string VouchToken     = "item_vouch_token_crossing";
00328:             public const string CalibrationWeight = "item_calibration_weight";
00329:             public const string TradedGrain    = "item_crossing_traded_grain";
00330:             public const string TradedSalt     = "item_crossing_traded_salt";
00331:             public const string PledgeSlip     = "item_crossing_pledge_slip";
00332:             public const string CharterPages   = "item_charter_three_pages";
00333:             public const string DebtContractCopy = "item_debt_contract_copy";
00334:             public const string MarkerRubbing  = "item_marker_rubbing";
00335:             public const string DutyLogFragment = "item_duty_log_fragment";
00336:             public const string TradeManifestBlank = "item_trade_manifest_blank";
00337:             public const string WynReceiptPaid = "item_wyn_receipt_paid";
00338:         }
00339:
00340:         /// <summary>World / story flag keys (bible §3 branching table).</summary>
00341:         public static class Flags
00342:         {
00343:             public const string VouchedClean   = "flag_crossing_vouched_clean";
00344:             public const string VouchBurned    = "flag_crossing_vouch_burned";
00345:             public const string AccessSoftened = "flag_crossing_access_softened";
00346:             public const string UnderwriteUntested = "flag_crossing_underwrite_untested";
00347:             public const string PetitionUnsigned = "flag_crossing_petition_unsigned";
00348:             public const string StandingHonest = "flag_crossing_standing_honest";
00349:             public const string StandingRigged = "flag_crossing_standing_rigged";
00350:             /// <summary>Set when the Ostrowski rumour starts — never at boot.</summary>
00351:             public const string ExpansionUnlocked = "exp_nobodys_charter_unlocked";
00352:             /// <summary>The vouch quest reward (token + lore) has been granted once.</summary>
00353:             public const string VouchRewarded = "flag_crossing_vouch_rewarded";
00354:         }
00355:
00356:         /// <summary>Named Crossing NPCs (ids must exist in characters.json).</summary>
00357:         public static class Npcs
00358:         {
00359:             public const string OsranKell = "npc_osran_kell";
00360:             public const string MattisCray = "npc_mattis_cray";
00361:             public const string DessaVane = "npc_dessa_vane";
00362:             public const string PerrinAshby = "npc_perrin_ashby";
00363:             public const string IvoFenn = "npc_ivo_fenn";
00364:             public const string WynSabler = "npc_wyn_sabler";
00365:         }
00366:
00367:         /// <summary>Knowledge keys granted by quest completion.</summary>
00368:         public static class Knowledge
00369:         {
00370:             public const string TheVouch       = "lore_nc_the_vouch";
00371:             public const string ReadAgain      = "lore_nc_read_again";
00372:             public const string RubricAgain    = "lore_nc_the_rubric_again";
00373:             public const string ThreeLegends   = "lore_nc_three_legends";
00374:             public const string TheForfeit     = "lore_nc_the_forfeit";
00375:             public const string TheStanding    = "lore_nc_the_standing";
00376:         }
00377:
00378:         /// <summary>World-state mutation ids (bible §6 endings / WorldStateConsequenceSystem).</summary>
00379:         public static class Mutations
00380:         {
00381:             public const string CharterRevealed = "mutation_crossing_charter_revealed";
00382:             public const string HonestTrader    = "mutation_crossing_honest_trader";
00383:             public const string UnderwriteBurned = "mutation_crossing_underwrite_burned";
00384:             public const string UnderwriteReliable = "mutation_crossing_underwrite_reliable";
00385:             public const string PetitionRevised = "mutation_crossing_petition_revised";
00386:             public const string PetitionLeaked = "mutation_crossing_petition_leaked";
00387:             public const string StandingRigged  = "mutation_crossing_standing_rigged";
00388:             public const string StandingHonest  = "mutation_crossing_standing_honest";
00389:             public const string ForfeitHonoured = "mutation_crossing_forfeit_honoured";
00390:             public const string VoteClean       = "mutation_crossing_vote_clean";
00391:             public const string VoteSabotaged   = "mutation_crossing_vote_sabotaged";
00392:         }
00393:
00394:         /// <summary>Ending ids (bible §3 Endings).</summary>
00395:         public static class Endings
00396:         {
00397:             public const string Scale       = "ending_crossing_scale";
00398:             public const string Underwrite  = "ending_crossing_underwrite";
00399:             public const string Compact     = "ending_crossing_compact";
00400:             public const string None        = "ending_crossing_none";
00401:             public const string Walked      = "ending_crossing_walked";
00402:         }
00403:
00404:         /// <summary>Canonical Crossing encounters (bible §6.2).</summary>
00405:         public static class Encounters
00406:         {
00407:             public const string CollectorVisit = "enc_nc_collector_visit";
00408:             public const string BackerPressure = "enc_nc_backer_pressure";
00409:             public const string LockupMuscle = "enc_nc_lockup_muscle";
00410:             public const string IronRaidersScout = "enc_nc_iron_raiders_scout";
00411:             public const string DeserterPassage = "enc_nc_deserter_passage";
00412:             public const string ScavengerDispute = "enc_nc_scavenger_dispute";
00413:             public const string GrainExchangeEnvoy = "enc_nc_grain_exchange_envoy";
00414:             public const string SunSeekersPass = "enc_nc_sun_seekers_pass";
00415:             public const string ForfeitWitness = "enc_nc_forfeit_witness";
00416:             public const string StandingAmbush = "enc_nc_standing_ambush";
00417:         }
00418:
00419:         /// <summary>Canonical multi-phase Crises (bible §6.3).</summary>
00420:         public static class Crises
00421:         {
00422:             public const string TheForfeit = "crisis_the_forfeit";
00423:             public const string TheVote = "crisis_the_vote";
00424:             public const string TheStandingContested = "crisis_the_standing_contested";
00425:             public const string TheCharterFound = "crisis_the_charter_found";
00426:             public const string WhoHoldsTheLedger = "crisis_who_holds_the_ledger";
00427:         }
00428:     }
00429: }
```

## `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` — 489 lines; 20,000 bytes; SHA-256 `ae757ef1db3c06fc7c23741ada561df5e2b07ca1585de1c2e6578811bae2e1de`
Declaration index:
- 00015: public class CrossingQuestStage
- 00021: public class CrossingQuestChoice
- 00029: public class CrossingQuestDef
- 00045: public class CrossingStageNarrativeEvent
- 00057: public class CrossingQuestProgress
- 00068: public class CrossingQuestSystemState
- 00085: public class CrossingQuestSystem
- 00105: public void BindCatalog(IReadOnlyList<CrossingQuestDef> catalog)
- 00110: public void BindMoralSystem(Ashfall.Core.MoralChoice.MoralChoiceSystem? moralSystem)
- 00115: public CrossingQuestDef? GetDef(string questId)
- 00130: public void BindConsequenceLedger(IFlagLedger? ledger)
- 00140: public List<CrossingQuestDef> GetVisibleQuests(int currentDay)
- 00158: public List<CrossingQuestDef> GetEligibleQuests(int currentDay, bool hasVouchAccess = false)
- 00175: public List<CrossingQuestDef> GetAvailableQuests(int currentDay) => GetEligibleQuests(currentDay, false);
- 00177: public bool IsQuestCompleted(string questId)
- 00183: public bool IsQuestFailed(string questId)
- 00189: public bool IsQuestStarted(string questId)
- 00195: public CrossingQuestProgress? GetProgress(string questId)
- 00207: public void TickDaily(int currentDay, bool hasVouchAccess = false)
- 00230: public bool StartQuest(string questId, int currentDay)
- 00255: public bool FailQuest(string questId)
- 00266: public int AdvanceStage(string questId)
- 00293: private void EmitStageNarrative(CrossingQuestDef def, int stageIndex, bool isCompletion)
- 00327: public bool MakeChoice(string questId, string choiceId)
- 00379: public bool HasFlag(string flag) => _state.setFlags.Contains(flag);
- 00386: public CrossingQuestSystemState CaptureState()
- 00415: public void RestoreState(CrossingQuestSystemState? saved)
- 00448: private void ProjectFlagsToLedger()
- 00458: private void RaiseChanged() => OnStateChanged?.Invoke(_state);
- 00463: public static class CrossingQuestCatalogLoader
- 00467: public static List<CrossingQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using System.IO;
00006: using System.Text.Json;
00007: using System.Text.Json.Serialization;
00008:
00009: using Ashfall.Core.Flags;
00010: using Ashfall.Core.IO;
00011: namespace Ashfall.Core.Crossing
00012: {
00013:     // ── Data model (matches crossing_quests.json) ───────────────
00014:
00015:     public class CrossingQuestStage
00016:     {
00017:         [JsonPropertyName("id")] public string id { get; set; } = "";
00018:         [JsonPropertyName("text")] public string text { get; set; } = "";
00019:     }
00020:
00021:     public class CrossingQuestChoice
00022:     {
00023:         [JsonPropertyName("id")] public string id { get; set; } = "";
00024:         [JsonPropertyName("text")] public string text { get; set; } = "";
00025:         [JsonPropertyName("set_flag")] public string set_flag { get; set; } = "";
00026:         [JsonPropertyName("moral_delta")] public int moral_delta { get; set; }
00027:     }
00028:
00029:     public class CrossingQuestDef
00030:     {
00031:         [JsonPropertyName("id")] public string id { get; set; } = "";
00032:         [JsonPropertyName("display_name")] public string display_name { get; set; } = "";
00033:         [JsonPropertyName("type")] public string type { get; set; } = "";
00034:         [JsonPropertyName("briefing")] public string briefing { get; set; } = "";
00035:         [JsonPropertyName("prereq_quest_id")] public string prereq_quest_id { get; set; } = "";
00036:         [JsonPropertyName("min_day")] public int min_day { get; set; }
00037:         [JsonPropertyName("stages")] public List<CrossingQuestStage> stages { get; set; } = new();
00038:         [JsonPropertyName("choices")] public List<CrossingQuestChoice> choices { get; set; } = new();
00039:         [JsonPropertyName("knowledge_key")] public string knowledge_key { get; set; } = "";
00040:         [JsonPropertyName("target_location_id")] public string target_location_id { get; set; } = "";
00041:     }
00042:
00043:     // ── Runtime state ───────────────────────────────────────────
00044:
00045:     public class CrossingStageNarrativeEvent
00046:     {
00047:         public string questId { get; set; } = "";
00048:         public string questDisplayName { get; set; } = "";
00049:         public int stageIndex { get; set; }
00050:         public string stageId { get; set; } = "";
00051:         public string stageText { get; set; } = "";
00052:         public string briefing { get; set; } = "";
00053:         public bool isCompletion { get; set; }
00054:     }
00055:
00056:     [Serializable]
00057:     public class CrossingQuestProgress
00058:     {
00059:         public string questId = "";
00060:         public int currentStage;
00061:         public bool started;
00062:         public bool completed;
00063:         public bool failed;
00064:         public string chosenChoiceId = "";
00065:     }
00066:
00067:     [Serializable]
00068:     public class CrossingQuestSystemState
00069:     {
00070:         public string systemId = CrossingQuestSystem.SystemId;
00071:         public int lastTickedDay;
00072:         public List<CrossingQuestProgress> quests = new();
00073:         public HashSet<string> setFlags = new();
00074:         public HashSet<string> dispatchedStageEvents = new();
00075:     }
00076:
00077:     // ── System ──────────────────────────────────────────────────
00078:
00079:     /// <summary>
00080:     /// ASHFALL: NOBODY'S CHARTER — quest runtime for the Crossing.
00081:     /// Loads crossing_quests.json, tracks stage progress, handles choices/flags.
00082:     /// Integrates with VouchAccessSystem for the opening quest and daily auto-start.
00083:     /// Spec: docs/expansions/expansion_04_nobodys_charter_plan.md
00084:     /// </summary>
00085:     public class CrossingQuestSystem
00086:     {
00087:         public const string SystemId = "crossing_quest_system";
00088:         public const string OpeningQuest = "quest_crossing_the_vouch";
00089:
00090:         private CrossingQuestSystemState _state = new();
00091:         private IReadOnlyList<CrossingQuestDef> _catalog = Array.Empty<CrossingQuestDef>();
00092:         private IFlagLedger? _consequenceLedger;
00093:         private Ashfall.Core.MoralChoice.MoralChoiceSystem? _moralSystem;
00094:
00095:         public event Action<string, int> OnQuestStageChanged;
00096:         public event Action<string> OnQuestStarted;
00097:         public event Action<string> OnQuestCompleted;
00098:         public event Action<string> OnQuestFailed;
00099:         public event Action<string, string> OnFlagSet;
00100:         public event Action<CrossingStageNarrativeEvent> OnStageNarrativeEmitted;
00101:         public event Action<CrossingQuestSystemState> OnStateChanged;
00102:
00103:         public CrossingQuestSystemState State => _state;
00104:
00105:         public void BindCatalog(IReadOnlyList<CrossingQuestDef> catalog)
00106:         {
00107:             _catalog = catalog ?? Array.Empty<CrossingQuestDef>();
00108:         }
00109:
00110:         public void BindMoralSystem(Ashfall.Core.MoralChoice.MoralChoiceSystem? moralSystem)
00111:         {
00112:             _moralSystem = moralSystem;
00113:         }
00114:
00115:         public CrossingQuestDef? GetDef(string questId)
00116:         {
00117:             if (string.IsNullOrEmpty(questId)) return null;
00118:             for (int i = 0; i < _catalog.Count; i++)
00119:                 if (_catalog[i]?.id == questId) return _catalog[i];
00120:             return null;
00121:         }
00122:
00123:         public IReadOnlyList<CrossingQuestDef> Catalog => _catalog;
00124:
00125:         /// <summary>
00126:         /// Binds the campaign's existing consequence ledger. Crossing keeps its
00127:         /// local projection for quest save compatibility, while every new flag
00128:         /// is also recorded in the canonical campaign ledger.
00129:         /// </summary>
00130:         public void BindConsequenceLedger(IFlagLedger? ledger)
00131:         {
00132:             _consequenceLedger = ledger;
00133:             ProjectFlagsToLedger();
00134:         }
00135:
00136:         /// <summary>
00137:         /// Returns quests the player has unlocked/can see in the hub (prereqs met, min_day reached),
00138:         /// regardless of vouch access. Hub should show them as locked rather than hidden.
00139:         /// </summary>
00140:         public List<CrossingQuestDef> GetVisibleQuests(int currentDay)
00141:         {
00142:             var visible = new List<CrossingQuestDef>();
00143:             for (int i = 0; i < _catalog.Count; i++)
00144:             {
00145:                 var def = _catalog[i];
00146:                 if (def == null) continue;
00147:                 if (IsQuestCompleted(def.id)) continue;
00148:                 if (def.min_day > currentDay) continue;
00149:                 if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsQuestCompleted(def.prereq_quest_id)) continue;
00150:                 visible.Add(def);
00151:             }
00152:             return visible;
00153:         }
00154:
00155:         /// <summary>
00156:         /// Returns quests the player can actually start (visible + vouch access gate passed).
00157:         /// </summary>
00158:         public List<CrossingQuestDef> GetEligibleQuests(int currentDay, bool hasVouchAccess = false)
00159:         {
00160:             var visible = GetVisibleQuests(currentDay);
00161:             var eligible = new List<CrossingQuestDef>();
00162:             bool vouchPassed = hasVouchAccess || IsQuestCompleted(OpeningQuest);
00163:             for (int i = 0; i < visible.Count; i++)
00164:             {
00165:                 var def = visible[i];
00166:                 if (def.id == OpeningQuest || vouchPassed)
00167:                 {
00168:                     eligible.Add(def);
00169:                 }
00170:             }
00171:             return eligible;
00172:         }
00173:
00174:         /// <summary>Quests available given current day, prereqs, and flags. Backward-compatible alias for GetEligibleQuests.</summary>
00175:         public List<CrossingQuestDef> GetAvailableQuests(int currentDay) => GetEligibleQuests(currentDay, false);
00176:
00177:         public bool IsQuestCompleted(string questId)
00178:         {
00179:             var progress = GetProgress(questId);
00180:             return progress != null && progress.completed;
00181:         }
00182:
00183:         public bool IsQuestFailed(string questId)
00184:         {
00185:             var progress = GetProgress(questId);
00186:             return progress != null && progress.failed;
00187:         }
00188:
00189:         public bool IsQuestStarted(string questId)
00190:         {
00191:             var progress = GetProgress(questId);
00192:             return progress != null && progress.started;
00193:         }
00194:
00195:         public CrossingQuestProgress? GetProgress(string questId)
00196:         {
00197:             for (int i = 0; i < _state.quests.Count; i++)
00198:                 if (_state.quests[i].questId == questId) return _state.quests[i];
00199:             return null;
00200:         }
00201:
00202:         /// <summary>
00203:         /// Authoritative daily tick for Crossing quests.
00204:         /// Idempotent against repeated ticks on the same day and after save/load.
00205:         /// Automatically starts eligible quests once prerequisites and day threshold are met.
00206:         /// </summary>
00207:         public void TickDaily(int currentDay, bool hasVouchAccess = false)
00208:         {
00209:             if (_state.lastTickedDay == currentDay) return;
00210:             _state.lastTickedDay = currentDay;
00211:
00212:             for (int i = 0; i < _catalog.Count; i++)
00213:             {
00214:                 var def = _catalog[i];
00215:                 if (def == null) continue;
00216:                 if (IsQuestStarted(def.id) || IsQuestCompleted(def.id) || IsQuestFailed(def.id)) continue;
00217:                 if (def.min_day > currentDay) continue;
00218:                 if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsQuestCompleted(def.prereq_quest_id)) continue;
00219:
00220:                 // Post-vouch quests require either active vouch or opening quest completion
00221:                 if (def.id != OpeningQuest && !hasVouchAccess && !IsQuestCompleted(OpeningQuest)) continue;
00222:
00223:                 StartQuest(def.id, currentDay);
00224:             }
00225:
00226:             RaiseChanged();
00227:         }
00228:
00229:         /// <summary>Start a quest. Returns false if prereqs not met, already started, completed, or failed.</summary>
00230:         public bool StartQuest(string questId, int currentDay)
00231:         {
00232:             if (string.IsNullOrEmpty(questId)) return false;
00233:             var def = GetDef(questId);
00234:             if (def == null) return false;
00235:             if (IsQuestStarted(questId) || IsQuestCompleted(questId) || IsQuestFailed(questId)) return false;
00236:             if (def.min_day > currentDay) return false;
00237:             if (!string.IsNullOrEmpty(def.prereq_quest_id) && !IsQuestCompleted(def.prereq_quest_id)) return false;
00238:
00239:             var progress = new CrossingQuestProgress
00240:             {
00241:                 questId = questId,
00242:                 currentStage = 0,
00243:                 started = true,
00244:                 completed = false,
00245:                 failed = false
00246:             };
00247:             _state.quests.Add(progress);
00248:             OnQuestStarted?.Invoke(questId);
00249:             EmitStageNarrative(def, 0, isCompletion: false);
00250:             RaiseChanged();
00251:             return true;
00252:         }
00253:
00254:         /// <summary>Marks an active quest as failed.</summary>
00255:         public bool FailQuest(string questId)
00256:         {
00257:             var progress = GetProgress(questId);
00258:             if (progress == null || progress.completed || progress.failed) return false;
00259:             progress.failed = true;
00260:             OnQuestFailed?.Invoke(questId);
00261:             RaiseChanged();
00262:             return true;
00263:         }
00264:
00265:         /// <summary>Advance to the next stage. Returns the new stage index, or -1 if quest completed.</summary>
00266:         public int AdvanceStage(string questId)
00267:         {
00268:             var progress = GetProgress(questId);
00269:             if (progress == null || progress.completed || progress.failed) return -1;
00270:             var def = GetDef(questId);
00271:             if (def == null) return -1;
00272:
00273:             progress.currentStage++;
00274:             if (progress.currentStage >= def.stages.Count)
00275:             {
00276:                 progress.completed = true;
00277:                 OnQuestCompleted?.Invoke(questId);
00278:                 EmitStageNarrative(def, progress.currentStage, isCompletion: true);
00279:
00280:                 // The opening quest completion softens the gate
00281:                 if (questId == OpeningQuest)
00282:                     OnOpeningQuestCompleted?.Invoke();
00283:             }
00284:             else
00285:             {
00286:                 OnQuestStageChanged?.Invoke(questId, progress.currentStage);
00287:                 EmitStageNarrative(def, progress.currentStage, isCompletion: false);
00288:             }
00289:             RaiseChanged();
00290:             return progress.completed ? -1 : progress.currentStage;
00291:         }
00292:
00293:         private void EmitStageNarrative(CrossingQuestDef def, int stageIndex, bool isCompletion)
00294:         {
00295:             string eventKey = $"{def.id}:{stageIndex}:{(isCompletion ? "complete" : "stage")}";
00296:             if (_state.dispatchedStageEvents.Contains(eventKey)) return;
00297:             _state.dispatchedStageEvents.Add(eventKey);
00298:
00299:             string stageId = "";
00300:             string stageText = "";
00301:             if (isCompletion)
00302:             {
00303:                 stageId = "complete";
00304:                 stageText = $"[CHARTER RESOLVED] {def.display_name} concluded.";
00305:             }
00306:             else if (def.stages != null && stageIndex >= 0 && stageIndex < def.stages.Count)
00307:             {
00308:                 stageId = def.stages[stageIndex].id ?? "";
00309:                 stageText = def.stages[stageIndex].text ?? "";
00310:             }
00311:
00312:             var evt = new CrossingStageNarrativeEvent
00313:             {
00314:                 questId = def.id,
00315:                 questDisplayName = def.display_name,
00316:                 stageIndex = stageIndex,
00317:                 stageId = stageId,
00318:                 stageText = stageText,
00319:                 briefing = def.briefing,
00320:                 isCompletion = isCompletion
00321:             };
00322:
00323:             OnStageNarrativeEmitted?.Invoke(evt);
00324:         }
00325:
00326:         /// <summary>Make a choice for a quest. Sets the associated flag.</summary>
00327:         public bool MakeChoice(string questId, string choiceId)
00328:         {
00329:             var progress = GetProgress(questId);
00330:             if (progress == null || !progress.started || progress.completed || progress.failed) return false;
00331:             var def = GetDef(questId);
00332:             if (def == null) return false;
00333:
00334:             for (int i = 0; i < def.choices.Count; i++)
00335:             {
00336:                 var choice = def.choices[i];
00337:                 if (choice.id != choiceId) continue;
00338:                 if (!string.IsNullOrEmpty(progress.chosenChoiceId))
00339:                     return progress.chosenChoiceId == choiceId;
00340:
00341:                 progress.chosenChoiceId = choiceId;
00342:                 if (!string.IsNullOrEmpty(choice.set_flag))
00343:                 {
00344:                     _state.setFlags.Add(choice.set_flag);
00345:                     _consequenceLedger?.Set(
00346:                         choice.set_flag,
00347:                         SystemId,
00348:                         questId);
00349:                     OnFlagSet?.Invoke(questId, choice.set_flag);
00350:                 }
00351:
00352:                 if (_moralSystem != null)
00353:                 {
00354:                     if (!string.IsNullOrEmpty(choice.set_flag))
00355:                         _moralSystem.SetFlag(choice.set_flag);
00356:
00357:                     int delta = choice.moral_delta;
00358:                     if (delta == 0 && !string.IsNullOrEmpty(choice.set_flag))
00359:                     {
00360:                         if (choice.set_flag.StartsWith("flag_covenant_", StringComparison.Ordinal))
00361:                             delta = 5;
00362:                         else if (choice.set_flag.StartsWith("flag_dispute_", StringComparison.Ordinal))
00363:                             delta = -5;
00364:                     }
00365:                     if (delta != 0)
00366:                     {
00367:                         string moralFlag = delta > 0
00368:                             ? $"flag_moral_crossing_positive_{questId}"
00369:                             : $"flag_moral_crossing_negative_{questId}";
00370:                         _moralSystem.SetFlag(moralFlag);
00371:                     }
00372:                 }
00373:                 RaiseChanged();
00374:                 return true;
00375:             }
00376:             return false;
00377:         }
00378:
00379:         public bool HasFlag(string flag) => _state.setFlags.Contains(flag);
00380:
00381:         /// <summary>Event fired when the opening vouch quest is completed.</summary>
00382:         public event Action? OnOpeningQuestCompleted;
00383:
00384:         // ── Save / Load ─────────────────────────────────────────
00385:
00386:         public CrossingQuestSystemState CaptureState()
00387:         {
00388:             var stateCopy = new CrossingQuestSystemState
00389:             {
00390:                 systemId = SystemId,
00391:                 lastTickedDay = _state.lastTickedDay,
00392:                 quests = new List<CrossingQuestProgress>(_state.quests.Count),
00393:                 setFlags = new HashSet<string>(_state.setFlags),
00394:                 dispatchedStageEvents = new HashSet<string>(_state.dispatchedStageEvents)
00395:             };
00396:
00397:             for (int i = 0; i < _state.quests.Count; i++)
00398:             {
00399:                 var q = _state.quests[i];
00400:                 if (q == null) continue;
00401:                 stateCopy.quests.Add(new CrossingQuestProgress
00402:                 {
00403:                     questId = q.questId,
00404:                     currentStage = q.currentStage,
00405:                     started = q.started,
00406:                     completed = q.completed,
00407:                     failed = q.failed,
00408:                     chosenChoiceId = q.chosenChoiceId
00409:                 });
00410:             }
00411:
00412:             return stateCopy;
00413:         }
00414:
00415:         public void RestoreState(CrossingQuestSystemState? saved)
00416:         {
00417:             if (saved == null) return;
00418:             _state.systemId = SystemId;
00419:             _state.lastTickedDay = saved.lastTickedDay;
00420:             _state.quests = new List<CrossingQuestProgress>();
00421:             if (saved.quests != null)
00422:             {
00423:                 for (int i = 0; i < saved.quests.Count; i++)
00424:                 {
00425:                     var q = saved.quests[i];
00426:                     if (q == null) continue;
00427:                     _state.quests.Add(new CrossingQuestProgress
00428:                     {
00429:                         questId = q.questId ?? string.Empty,
00430:                         currentStage = q.currentStage,
00431:                         started = q.started,
00432:                         completed = q.completed,
00433:                         failed = q.failed,
00434:                         chosenChoiceId = q.chosenChoiceId ?? string.Empty
00435:                     });
00436:                 }
00437:             }
00438:             _state.setFlags = saved.setFlags != null
00439:                 ? new HashSet<string>(saved.setFlags)
00440:                 : new HashSet<string>();
00441:             _state.dispatchedStageEvents = saved.dispatchedStageEvents != null
00442:                 ? new HashSet<string>(saved.dispatchedStageEvents)
00443:                 : new HashSet<string>();
00444:             ProjectFlagsToLedger();
00445:             RaiseChanged();
00446:         }
00447:
00448:         private void ProjectFlagsToLedger()
00449:         {
00450:             if (_consequenceLedger == null || _state.setFlags == null) return;
00451:             foreach (string flag in _state.setFlags)
00452:             {
00453:                 if (!string.IsNullOrEmpty(flag))
00454:                     _consequenceLedger.Set(flag, SystemId);
00455:             }
00456:         }
00457:
00458:         private void RaiseChanged() => OnStateChanged?.Invoke(_state);
00459:     }
00460:
00461:     // ── Catalog loader ──────────────────────────────────────────
00462:
00463:     public static class CrossingQuestCatalogLoader
00464:     {
00465:         public const string FileName = "crossing_quests.json";
00466:
00467:         public static List<CrossingQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
00468:         {
00469:             fileIO ??= new FileSystemIO();
00470:             serializer ??= new SystemTextJsonSerializer();
00471:             string path = Path.Combine(dataDir, FileName);
00472:             if (!fileIO.FileExists(path)) return new List<CrossingQuestDef>();
00473:
00474:             string json = fileIO.ReadAllText(path);
00475:             if (string.IsNullOrEmpty(json)) return new List<CrossingQuestDef>();
00476:
00477:             try
00478:             {
00479:                 var quests = CatalogLocator.LoadWrappedList<CrossingQuestDef>(json, SystemTextJsonSerializer.Options);
00480:                 return quests ?? new List<CrossingQuestDef>();
00481:             }
00482:             catch (Exception ex_CATDIAG)
00483:             {
00484:                 CatalogDiagnostics.Warn(path, "CrossingQuestDef list", ex_CATDIAG);
00485:                 return new List<CrossingQuestDef>();
00486:             }
00487:         }
00488:     }
00489: }
```

## `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` — 489 lines; 20,704 bytes; SHA-256 `13a36a288db0b04c398b1774dea0842564b935818e4edb1b099da951d383445f`
Declaration index:
- 00020: public class BackerDef
- 00031: public class StandingRuling
- 00042: public enum RulingShape
- 00056: public enum BribeResult
- 00064: public class CrossingArbitrationState
- 00076: public class CrossingArbitrationSystem
- 00095: public void LoadBackerPool(IReadOnlyList<BackerDef> defs)
- 00120: public BackerDef? GetBacker(string id)
- 00131: public StandingRuling? GetRuling(string topic)
- 00146: public List<StandingRuling> GetRulingHistory(string topic)
- 00159: public List<BackerDef> GetAvailableBackers(string topic)
- 00178: public bool IsRulingHeld(string topic)
- 00189: public bool IsRulingActive(string topic)
- 00195: public bool IsRulingOverturned(string topic)
- 00209: public bool CallStanding(string topic, int currentDay)
- 00249: public bool DeclareBacker(string topic, string backerId)
- 00281: public BribeResult TryBribeBacker(string topic, string backerId)
- 00327: public bool OverturnRuling(string topic, IReadOnlyList<string> counterBackerIds)
- 00367: public bool RemoveBacker(string backerId)
- 00396: private RulingShape ResolveHoldShape(StandingRuling ruling)
- 00408: private bool HasPrincipledMajority(StandingRuling ruling)
- 00421: public CrossingArbitrationState CaptureState()
- 00430: private static CrossingArbitrationState CloneState(CrossingArbitrationState from)
- 00479: public void RestoreState(CrossingArbitrationState saved)
- 00487: private void RaiseChanged() => OnStateChanged?.Invoke(_state);
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core
00007: {
00008:     /// <summary>
00009:     /// ASHFALL: NOBODY'S CHARTER — §5.1 CrossingArbitrationSystem.
00010:     /// The Standing. A ruling is real for as long as three backers hold it.
00011:     /// Engine-agnostic extract of Assets/_Game/Core/CrossingArbitrationSystem.cs.
00012:     /// Core additions over the Unity original (house pattern): OnStateChanged
00013:     /// on every mutation, defensive CaptureState copy, null-safe RestoreState.
00014:     /// Not a political sim. Not agent-based. Scripted micro-disputes only.
00015:     /// </summary>
00016:
00017:     // ── Data types ─────────────────────────────────────────────────────
00018:
00019:     [Serializable]
00020:     public class BackerDef
00021:     {
00022:         public string id;
00023:         public string displayName;
00024:         public string wants;        // what motivates this backer
00025:         public string willNot;      // hard limit — will not cross
00026:         public bool principled;     // cannot be bribed; caps pure-buy rulings
00027:         public bool isAlive = true; // dead backers lose their hold
00028:     }
00029:
00030:     [Serializable]
00031:     public class StandingRuling
00032:     {
00033:         public string topic;       // dispute subject (quest id or freeform)
00034:         public List<string> backers = new List<string>(); // backer ids holding this
00035:         public RulingShape shape;
00036:         public int dayCalled;
00037:         public List<string> bribedBackers = new List<string>(); // bought, not earned
00038:         public int bribeMarks; // public refusals / known-bought marks on this ruling
00039:         public List<string> refusedBribes = new List<string>(); // principled backers who refused publicly
00040:     }
00041:
00042:     public enum RulingShape
00043:     {
00044:         Pending,    // called but not yet 3 backers
00045:         Honest,     // 3+ backers, none bought, principled majority behind it
00046:         Rigged,     // 3+ backers, but bought (or no principled majority to vouch for it)
00047:         Overturned  // 3+ counter-backers
00048:     }
00049:
00050:     /// <summary>
00051:     /// Outcome of attempting to buy a backer. A principled backer refuses
00052:     /// outright and says so publicly — the refusal is itself a mark on the
00053:     /// ruling (bible §5.1: "some backers refuse a bribe outright and will
00054:     /// say so publicly if pushed, which itself becomes a mark").
00055:     /// </summary>
00056:     public enum BribeResult
00057:     {
00058:         Invalid,          // no pending ruling / dead / unknown / committed backer
00059:         Accepted,          // a non-principled backer took the bribe
00060:         RefusedPrincipled  // a principled backer refused, publicly (a mark)
00061:     }
00062:
00063:     [Serializable]
00064:     public class CrossingArbitrationState
00065:     {
00066:         public string systemId = CrossingArbitrationSystem.SystemId;
00067:         public List<BackerDef> backerPool = new List<BackerDef>();
00068:         public List<StandingRuling> rulings = new List<StandingRuling>();
00069:         public int rulingsCalled;
00070:         public int rulingsOverturned;
00071:         public int standingRepeats; // re-Standings called after an overturn
00072:     }
00073:
00074:     // ── System ─────────────────────────────────────────────────────────
00075:
00076:     public class CrossingArbitrationSystem
00077:     {
00078:         public const string SystemId = "crossing_arbitration_system";
00079:         public const int BackersToHold = 3;
00080:
00081:         private CrossingArbitrationState _state = new CrossingArbitrationState();
00082:
00083:         public event Action<string> OnStandingCalled;          // topic
00084:         public event Action<StandingRuling> OnRulingMade;      // the ruling that now holds
00085:         public event Action<StandingRuling> OnRulingOverturned;
00086:         public event Action<string, string> OnBribeRefused;    // backerId, topic (public mark)
00087:         public event Action<CrossingArbitrationState> OnStateChanged;
00088:
00089:         public CrossingArbitrationState State => _state;
00090:         public IReadOnlyList<BackerDef> BackerPool => _state.backerPool;
00091:         public IReadOnlyList<StandingRuling> Rulings => _state.rulings;
00092:
00093:         // ── Initialisation ─────────────────────────────────────────────
00094:
00095:         public void LoadBackerPool(IReadOnlyList<BackerDef> defs)
00096:         {
00097:             _state.backerPool.Clear();
00098:             if (defs == null) return;
00099:             for (int i = 0; i < defs.Count; i++)
00100:             {
00101:                 var d = defs[i];
00102:                 if (d == null) continue;
00103:                 // Deep copy: the pool owns its backers, so a caller mutating
00104:                 // (or reusing) the source list cannot change live rulings.
00105:                 _state.backerPool.Add(new BackerDef
00106:                 {
00107:                     id = d.id,
00108:                     displayName = d.displayName,
00109:                     wants = d.wants,
00110:                     willNot = d.willNot,
00111:                     principled = d.principled,
00112:                     isAlive = d.isAlive
00113:                 });
00114:             }
00115:             RaiseChanged();
00116:         }
00117:
00118:         // ── Queries ────────────────────────────────────────────────────
00119:
00120:         public BackerDef? GetBacker(string id)
00121:         {
00122:             if (string.IsNullOrEmpty(id)) return null;
00123:             for (int i = 0; i < _state.backerPool.Count; i++)
00124:             {
00125:                 var b = _state.backerPool[i];
00126:                 if (b != null && b.id == id) return b;
00127:             }
00128:             return null;
00129:         }
00130:
00131:         public StandingRuling? GetRuling(string topic)
00132:         {
00133:             if (string.IsNullOrEmpty(topic)) return null;
00134:             // Latest match wins: an overturned ruling can be re-Stood, so a
00135:             // topic may carry history. The active ruling is the most recent.
00136:             StandingRuling? result = null;
00137:             for (int i = 0; i < _state.rulings.Count; i++)
00138:             {
00139:                 var r = _state.rulings[i];
00140:                 if (r != null && r.topic == topic) result = r;
00141:             }
00142:             return result;
00143:         }
00144:
00145:         /// <summary>Every ruling ever called on this topic, oldest first (history).</summary>
00146:         public List<StandingRuling> GetRulingHistory(string topic)
00147:         {
00148:             var result = new List<StandingRuling>();
00149:             if (string.IsNullOrEmpty(topic)) return result;
00150:             for (int i = 0; i < _state.rulings.Count; i++)
00151:             {
00152:                 var r = _state.rulings[i];
00153:                 if (r != null && r.topic == topic) result.Add(r);
00154:             }
00155:             return result;
00156:         }
00157:
00158:         /// <summary>All living backers not already committed to this topic.</summary>
00159:         public List<BackerDef> GetAvailableBackers(string topic)
00160:         {
00161:             var result = new List<BackerDef>();
00162:             var existing = GetRuling(topic);
00163:             for (int i = 0; i < _state.backerPool.Count; i++)
00164:             {
00165:                 var b = _state.backerPool[i];
00166:                 if (b == null || !b.isAlive) continue;
00167:                 if (existing != null && existing.backers.Contains(b.id)) continue;
00168:                 result.Add(b);
00169:             }
00170:             return result;
00171:         }
00172:
00173:         /// <summary>
00174:         /// True when the topic's ruling is held honestly (3+ backers, none
00175:         /// bought, principled majority). A rigged ruling is on the board but
00176:         /// is not "held honestly" — use IsRulingActive for control queries.
00177:         /// </summary>
00178:         public bool IsRulingHeld(string topic)
00179:         {
00180:             var r = GetRuling(topic);
00181:             return r != null && r.shape == RulingShape.Honest && r.backers.Count >= BackersToHold;
00182:         }
00183:
00184:         /// <summary>
00185:         /// True when the topic's ruling is currently on the board — held
00186:         /// honestly or held bought (rigged). Quest/mutation logic reads this
00187:         /// for "who currently controls X at the Crossing" (bible §5.1).
00188:         /// </summary>
00189:         public bool IsRulingActive(string topic)
00190:         {
00191:             var r = GetRuling(topic);
00192:             return r != null && (r.shape == RulingShape.Honest || r.shape == RulingShape.Rigged);
00193:         }
00194:
00195:         public bool IsRulingOverturned(string topic)
00196:         {
00197:             var r = GetRuling(topic);
00198:             return r != null && r.shape == RulingShape.Overturned;
00199:         }
00200:
00201:         // ── Actions ────────────────────────────────────────────────────
00202:
00203:         /// <summary>
00204:         /// Call a Standing on a topic. Creates a pending ruling if none exists.
00205:         /// A held (honest/rigged) ruling must be challenged via OverturnRuling,
00206:         /// not re-called. An overturned ruling may be re-Stood — nothing is
00207:         /// permanently settled (bible §5.1).
00208:         /// </summary>
00209:         public bool CallStanding(string topic, int currentDay)
00210:         {
00211:             if (string.IsNullOrEmpty(topic)) return false;
00212:             var existing = GetRuling(topic);
00213:
00214:             if (existing != null)
00215:             {
00216:                 if (existing.shape == RulingShape.Pending)
00217:                 {
00218:                     // Idempotent re-call on a pending ruling.
00219:                     _state.rulingsCalled++;
00220:                     OnStandingCalled?.Invoke(topic);
00221:                     RaiseChanged();
00222:                     return true;
00223:                 }
00224:                 if (existing.shape == RulingShape.Honest || existing.shape == RulingShape.Rigged)
00225:                     return false; // held — challenge via OverturnRuling
00226:                 // Overturned: re-Standing. Fall through to a fresh pending ruling.
00227:                 _state.standingRepeats++;
00228:             }
00229:
00230:             var ruling = new StandingRuling
00231:             {
00232:                 topic = topic,
00233:                 shape = RulingShape.Pending,
00234:                 dayCalled = currentDay
00235:             };
00236:             _state.rulings.Add(ruling);
00237:
00238:             _state.rulingsCalled++;
00239:             OnStandingCalled?.Invoke(topic);
00240:             RaiseChanged();
00241:             return true;
00242:         }
00243:
00244:         /// <summary>
00245:         /// A backer declares support for the topic's ruling.
00246:         /// Returns false if the backer is dead, already committed, or the
00247:         /// ruling is already final (held / overturned).
00248:         /// </summary>
00249:         public bool DeclareBacker(string topic, string backerId)
00250:         {
00251:             if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(backerId)) return false;
00252:
00253:             var backer = GetBacker(backerId);
00254:             if (backer == null || !backer.isAlive) return false;
00255:
00256:             var ruling = GetRuling(topic);
00257:             if (ruling == null) return false; // CallStanding first
00258:             if (ruling.shape == RulingShape.Overturned) return false;
00259:             if (ruling.backers.Contains(backerId)) return false;
00260:
00261:             ruling.backers.Add(backerId);
00262:
00263:             if (ruling.backers.Count >= BackersToHold && ruling.shape == RulingShape.Pending)
00264:             {
00265:                 ruling.shape = ResolveHoldShape(ruling);
00266:                 OnRulingMade?.Invoke(ruling);
00267:             }
00268:
00269:             RaiseChanged();
00270:             return true;
00271:         }
00272:
00273:         /// <summary>
00274:         /// Attempt to buy a backer's support (bible §5.1 principled cap).
00275:         /// A principled backer refuses outright and the refusal is a public
00276:         /// mark on the ruling; a non-principled backer accepts and the ruling
00277:         /// is then known-bought — it will hold Rigged, never Honest.
00278:         /// Only valid while the ruling is Pending; held/overturned/missing
00279:         /// rulings and dead or already-committed backers return Invalid.
00280:         /// </summary>
00281:         public BribeResult TryBribeBacker(string topic, string backerId)
00282:         {
00283:             if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(backerId))
00284:                 return BribeResult.Invalid;
00285:
00286:             var ruling = GetRuling(topic);
00287:             if (ruling == null || ruling.shape != RulingShape.Pending)
00288:                 return BribeResult.Invalid;
00289:
00290:             var backer = GetBacker(backerId);
00291:             if (backer == null || !backer.isAlive || ruling.backers.Contains(backerId))
00292:                 return BribeResult.Invalid;
00293:
00294:             if (backer.principled)
00295:             {
00296:                 if (ruling.refusedBribes == null) ruling.refusedBribes = new List<string>();
00297:                 // A principled backer refuses once, publicly. Pushing again
00298:                 // yields nothing new — the refusal is already a mark.
00299:                 if (ruling.refusedBribes.Contains(backerId)) return BribeResult.Invalid;
00300:                 ruling.refusedBribes.Add(backerId);
00301:                 ruling.bribeMarks++;
00302:                 OnBribeRefused?.Invoke(backerId, topic);
00303:                 RaiseChanged();
00304:                 return BribeResult.RefusedPrincipled;
00305:             }
00306:
00307:             ruling.backers.Add(backerId);
00308:             if (ruling.bribedBackers == null) ruling.bribedBackers = new List<string>();
00309:             ruling.bribedBackers.Add(backerId);
00310:
00311:             if (ruling.backers.Count >= BackersToHold && ruling.shape == RulingShape.Pending)
00312:             {
00313:                 ruling.shape = ResolveHoldShape(ruling);
00314:                 OnRulingMade?.Invoke(ruling);
00315:             }
00316:
00317:             RaiseChanged();
00318:             return BribeResult.Accepted;
00319:         }
00320:
00321:         /// <summary>
00322:         /// Overturn an existing ruling by bringing 3+ counter-backers.
00323:         /// Counters must be distinct, living backers and a *different* set
00324:         /// from the current holders (bible §5.1: "a different 3+ backers").
00325:         /// The ruling's shape becomes Overturned; backers are cleared.
00326:         /// </summary>
00327:         public bool OverturnRuling(string topic, IReadOnlyList<string> counterBackerIds)
00328:         {
00329:             if (string.IsNullOrEmpty(topic) || counterBackerIds == null) return false;
00330:
00331:             var ruling = GetRuling(topic);
00332:             if (ruling == null) return false;
00333:             if (ruling.shape != RulingShape.Honest && ruling.shape != RulingShape.Rigged)
00334:                 return false;
00335:
00336:             if (counterBackerIds.Count < BackersToHold) return false;
00337:
00338:             // Counters must be distinct, living backers, and not the same
00339:             // set that currently holds the ruling.
00340:             var seen = new HashSet<string>();
00341:             bool differsFromHolders = false;
00342:             for (int i = 0; i < counterBackerIds.Count; i++)
00343:             {
00344:                 var id = counterBackerIds[i];
00345:                 if (string.IsNullOrEmpty(id)) return false;
00346:                 var b = GetBacker(id);
00347:                 if (b == null || !b.isAlive) return false;
00348:                 if (!seen.Add(id)) return false; // duplicate counter
00349:                 if (!ruling.backers.Contains(id)) differsFromHolders = true;
00350:             }
00351:             if (!differsFromHolders) return false;
00352:
00353:             ruling.shape = RulingShape.Overturned;
00354:             ruling.backers.Clear();
00355:             if (ruling.bribedBackers != null) ruling.bribedBackers.Clear();
00356:             _state.rulingsOverturned++;
00357:             OnRulingOverturned?.Invoke(ruling);
00358:             RaiseChanged();
00359:             return true;
00360:         }
00361:
00362:         /// <summary>
00363:         /// Kill a backer (death, exile, departure). Their held rulings
00364:         /// lose one backer; if a held ruling drops below 3, it reverts
00365:         /// to Pending.
00366:         /// </summary>
00367:         public bool RemoveBacker(string backerId)
00368:         {
00369:             var backer = GetBacker(backerId);
00370:             if (backer == null || !backer.isAlive) return false;
00371:
00372:             backer.isAlive = false;
00373:
00374:             // Check all rulings this backer held
00375:             for (int i = 0; i < _state.rulings.Count; i++)
00376:             {
00377:                 var r = _state.rulings[i];
00378:                 if (r == null || !r.backers.Contains(backerId)) continue;
00379:                 r.backers.Remove(backerId);
00380:                 if (r.bribedBackers != null) r.bribedBackers.Remove(backerId);
00381:                 if (r.shape != RulingShape.Overturned && r.backers.Count < BackersToHold)
00382:                     r.shape = RulingShape.Pending;
00383:             }
00384:             RaiseChanged();
00385:             return true;
00386:         }
00387:
00388:         // ── Helpers ─────────────────────────────────────────────────────
00389:
00390:         /// <summary>
00391:         /// A ruling that reaches three backers holds. It is Honest only when
00392:         /// no backer was bought and a principled majority stands behind it; a
00393:         /// bought ruling is Rigged even if principled backers also hold it —
00394:         /// the purchase is public knowledge (bible §5.1).
00395:         /// </summary>
00396:         private RulingShape ResolveHoldShape(StandingRuling ruling)
00397:         {
00398:             if (ruling.bribedBackers != null && ruling.bribedBackers.Count > 0)
00399:                 return RulingShape.Rigged;
00400:             return HasPrincipledMajority(ruling) ? RulingShape.Honest : RulingShape.Rigged;
00401:         }
00402:
00403:         /// <summary>
00404:         /// A principled majority means most backers cannot be bribed.
00405:         /// If a majority are principled, the ruling is honest even if
00406:         /// some non-principled backers were bought.
00407:         /// </summary>
00408:         private bool HasPrincipledMajority(StandingRuling ruling)
00409:         {
00410:             int principled = 0;
00411:             for (int i = 0; i < ruling.backers.Count; i++)
00412:             {
00413:                 var b = GetBacker(ruling.backers[i]);
00414:                 if (b != null && b.principled) principled++;
00415:             }
00416:             return principled > ruling.backers.Count / 2;
00417:         }
00418:
00419:         // ── Save / Load ────────────────────────────────────────────────
00420:
00421:         public CrossingArbitrationState CaptureState()
00422:         {
00423:             return CloneState(_state);
00424:         }
00425:
00426:         /// <summary>
00427:         /// Deep copy: the live system and the serialized envelope must never
00428:         /// alias the same lists, or a later mutation corrupts the save.
00429:         /// </summary>
00430:         private static CrossingArbitrationState CloneState(CrossingArbitrationState from)
00431:         {
00432:             var copy = new CrossingArbitrationState
00433:             {
00434:                 systemId = from.systemId,
00435:                 rulingsCalled = from.rulingsCalled,
00436:                 rulingsOverturned = from.rulingsOverturned,
00437:                 standingRepeats = from.standingRepeats,
00438:                 backerPool = new List<BackerDef>(),
00439:                 rulings = new List<StandingRuling>()
00440:             };
00441:             if (from.backerPool != null)
00442:             {
00443:                 for (int i = 0; i < from.backerPool.Count; i++)
00444:                 {
00445:                     var b = from.backerPool[i];
00446:                     if (b == null) continue;
00447:                     copy.backerPool.Add(new BackerDef
00448:                     {
00449:                         id = b.id,
00450:                         displayName = b.displayName,
00451:                         wants = b.wants,
00452:                         willNot = b.willNot,
00453:                         principled = b.principled,
00454:                         isAlive = b.isAlive
00455:                     });
00456:                 }
00457:             }
00458:             if (from.rulings != null)
00459:             {
00460:                 for (int i = 0; i < from.rulings.Count; i++)
00461:                 {
00462:                     var r = from.rulings[i];
00463:                     if (r == null) continue;
00464:                     copy.rulings.Add(new StandingRuling
00465:                     {
00466:                         topic = r.topic,
00467:                         shape = r.shape,
00468:                         dayCalled = r.dayCalled,
00469:                         bribeMarks = r.bribeMarks,
00470:                         backers = r.backers != null ? new List<string>(r.backers) : new List<string>(),
00471:                         bribedBackers = r.bribedBackers != null ? new List<string>(r.bribedBackers) : new List<string>(),
00472:                         refusedBribes = r.refusedBribes != null ? new List<string>(r.refusedBribes) : new List<string>()
00473:                     });
00474:                 }
00475:             }
00476:             return copy;
00477:         }
00478:
00479:         public void RestoreState(CrossingArbitrationState saved)
00480:         {
00481:             if (saved == null) return;
00482:             _state = CloneState(saved);
00483:             if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
00484:             RaiseChanged();
00485:         }
00486:
00487:         private void RaiseChanged() => OnStateChanged?.Invoke(_state);
00488:     }
00489: }
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

## `src/UI/CrossingQuestPanel.cs` — 493 lines; 22,286 bytes; SHA-256 `04e9b7fbf0c954b832ccda943f6bb59e98f4bad06478aa9043548f24e92d71d3`
Declaration index:
- 00018: public partial class CrossingQuestPanel : Control, IBindablePanel
- 00037: public void Bind(ExpansionHostSession expansions, VouchAccessSystem? vouch, int currentDay)
- 00060: private void OnStateChangedHandler(CrossingQuestSystemState state) => RefreshView();
- 00061: private void OnVouchChangedHandler(VouchAccessSystemState state) => RefreshView();
- 00063: public void Open()
- 00167: public void RefreshView()
- 00299: private Control BuildActiveQuestCard(CrossingQuestDef def, CrossingQuestProgress prog)
- 00433: private Control BuildAvailableQuestCard(CrossingQuestDef def, bool locked, string lockReason)
- 00469: private static void ClearContainer(VBoxContainer container)
- 00475: public void Unbind()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Crossing;
00007: using Ashfall.Core.UI;
00008: using CoreTheme = Ashfall.Core.UI.Theme;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Nobody's Charter (Exp 04) Crossing quest panel.
00014:     /// Presents the active Crossing quest, stage objectives, narrative briefing,
00015:     /// available interaction choices, and gate/lock reasons. All state mutations go
00016:     /// through ExpansionHostSession; this panel is purely presentational.
00017:     /// </summary>
00018:     public partial class CrossingQuestPanel : Control, IBindablePanel
00019:     {
00020:         public event Action? OnClose;
00021:
00022:         private ExpansionHostSession? _expansions;
00023:         private VouchAccessSystem? _vouch;
00024:         private int _currentDay = 1;
00025:
00026:         // ── UI nodes ──────────────────────────────────────────────────
00027:         private Label _gateStatus = null!;
00028:         private VBoxContainer _activeQuestContainer = null!;
00029:         private VBoxContainer _availableQuestsContainer = null!;
00030:         private VBoxContainer _completedQuestsContainer = null!;
00031:         private Label _emptyState = null!;
00032:
00033:         public bool IsBound => _expansions != null;
00034:
00035:         // ── Bind ──────────────────────────────────────────────────────
00036:
00037:         public void Bind(ExpansionHostSession expansions, VouchAccessSystem? vouch, int currentDay)
00038:         {
00039:             if (_expansions != null)
00040:             {
00041:                 _expansions.CrossingQuests.OnStateChanged -= OnStateChangedHandler;
00042:                 if (_vouch != null)
00043:                     _vouch.OnStateChanged -= OnVouchChangedHandler;
00044:             }
00045:
00046:             _expansions = expansions;
00047:             _vouch = vouch ?? _expansions?.Vouch;
00048:             _currentDay = currentDay;
00049:
00050:             if (_expansions != null)
00051:             {
00052:                 _expansions.CrossingQuests.OnStateChanged += OnStateChangedHandler;
00053:                 if (_vouch != null)
00054:                     _vouch.OnStateChanged += OnVouchChangedHandler;
00055:             }
00056:
00057:             RefreshView();
00058:         }
00059:
00060:         private void OnStateChangedHandler(CrossingQuestSystemState state) => RefreshView();
00061:         private void OnVouchChangedHandler(VouchAccessSystemState state) => RefreshView();
00062:
00063:         public void Open()
00064:         {
00065:             Visible = true;
00066:             RefreshView();
00067:         }
00068:
00069:         // ── Godot lifecycle ───────────────────────────────────────────
00070:
00071:         public override void _Ready()
00072:         {
00073:             SetAnchorsPreset(LayoutPreset.FullRect);
00074:             Visible = false;
00075:
00076:             var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
00077:             bg.SetAnchorsPreset(LayoutPreset.FullRect);
00078:             AddChild(bg);
00079:
00080:             var scroll = new ScrollContainer();
00081:             scroll.SetAnchorsPreset(LayoutPreset.FullRect);
00082:             scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
00083:             AddChild(scroll);
00084:
00085:             var center = new CenterContainer();
00086:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00087:             center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00088:             center.SizeFlagsVertical = SizeFlags.ExpandFill;
00089:             scroll.AddChild(center);
00090:
00091:             var rootBox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingMd);
00092:             rootBox.CustomMinimumSize = new Vector2(760, 0);
00093:             center.AddChild(rootBox);
00094:
00095:             // ── Header ─────────────────────────────────────────────
00096:             var header = AshfallUiHelpers.MakeTitle("NOBODY'S CHARTER // CROSSING PROTOCOLS", CoreTheme.FontSizeH1);
00097:             header.HorizontalAlignment = HorizontalAlignment.Center;
00098:             rootBox.AddChild(header);
00099:
00100:             var subtitle = AshfallUiHelpers.MakeSmall("Active obligations under the Crossing. The ledger is honest. The gate is patient.");
00101:             subtitle.HorizontalAlignment = HorizontalAlignment.Center;
00102:             subtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00103:             rootBox.AddChild(subtitle);
00104:
00105:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00106:
00107:             // ── Gate status bar ─────────────────────────────────────
00108:             _gateStatus = AshfallUiHelpers.MakeMono("GATE: —");
00109:             _gateStatus.HorizontalAlignment = HorizontalAlignment.Center;
00110:             _gateStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Entropy));
00111:             rootBox.AddChild(_gateStatus);
00112:
00113:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00114:
00115:             // ── Active Quest Container ──────────────────────────────
00116:             _activeQuestContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00117:             rootBox.AddChild(_activeQuestContainer);
00118:
00119:             // ── Empty State ─────────────────────────────────────────
00120:             _emptyState = AshfallUiHelpers.MakeBody("No active Crossing protocol. Return when you hold a vouch or the gate day arrives.");
00121:             _emptyState.HorizontalAlignment = HorizontalAlignment.Center;
00122:             _emptyState.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00123:             _emptyState.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00124:             rootBox.AddChild(_emptyState);
00125:
00126:             // ── Available Quests Container ──────────────────────────
00127:             _availableQuestsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00128:             rootBox.AddChild(_availableQuestsContainer);
00129:
00130:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00131:
00132:             // ── Completed Quests Container ──────────────────────────
00133:             _completedQuestsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
00134:             rootBox.AddChild(_completedQuestsContainer);
00135:
00136:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00137:
00138:             // ── Footer buttons ──────────────────────────────────────
00139:             var btnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingMd);
00140:             btnRow.Alignment = BoxContainer.AlignmentMode.Center;
00141:
00142:             var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [Esc]", () => OnClose?.Invoke(), false);
00143:             btnClose.CustomMinimumSize = new Vector2(260, 42);
00144:             btnRow.AddChild(btnClose);
00145:
00146:             rootBox.AddChild(btnRow);
00147:
00148:             var hint = AshfallUiHelpers.MakeSmall("Press [Esc] to return");
00149:             hint.HorizontalAlignment = HorizontalAlignment.Center;
00150:             hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00151:             rootBox.AddChild(hint);
00152:         }
00153:
00154:         public override void _UnhandledInput(InputEvent @event)
00155:         {
00156:             if (!Visible) return;
00157:
00158:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00159:             {
00160:                 OnClose?.Invoke();
00161:                 GetViewport().SetInputAsHandled();
00162:             }
00163:         }
00164:
00165:         // ── View refresh ──────────────────────────────────────────────
00166:
00167:         public void RefreshView()
00168:         {
00169:             if (_activeQuestContainer == null) return; // _Ready not yet called
00170:
00171:             ClearContainer(_activeQuestContainer);
00172:             ClearContainer(_availableQuestsContainer);
00173:             ClearContainer(_completedQuestsContainer);
00174:
00175:             if (_expansions == null)
00176:             {
00177:                 _emptyState.Text = "Panel not bound to session.";
00178:                 _emptyState.Visible = true;
00179:                 return;
00180:             }
00181:
00182:             // Gate status
00183:             bool gateOpen = _vouch?.HasAccess ?? (_expansions.Vouch?.HasAccess ?? false);
00184:             string vouchedBy = _vouch?.VouchedBy ?? (_expansions.Vouch?.VouchedBy ?? "");
00185:             string vouchDesc = string.IsNullOrEmpty(vouchedBy) ? "" : $" (Vouched by {vouchedBy})";
00186:             string gateLabel = gateOpen
00187:                 ? $"GATE: OPEN — Vouch on ledger{vouchDesc}"
00188:                 : "GATE: CLOSED — Vouch required to cross the viaduct";
00189:
00190:             _gateStatus.Text = gateLabel;
00191:             _gateStatus.AddThemeColorOverride("font_color",
00192:                 gateOpen
00193:                     ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
00194:                     : AshfallUiHelpers.ToColor(CoreTheme.Entropy));
00195:
00196:             var catalog = _expansions.CrossingQuests.Catalog;
00197:             CrossingQuestDef? activeDef = null;
00198:             CrossingQuestProgress? activeProgress = null;
00199:             var availableList = new List<(CrossingQuestDef def, bool locked, string reason)>();
00200:             var completedList = new List<(CrossingQuestDef def, CrossingQuestProgress prog)>();
00201:
00202:             for (int i = 0; i < catalog.Count; i++)
00203:             {
00204:                 var def = catalog[i];
00205:                 if (def == null) continue;
00206:                 var prog = _expansions.CrossingQuests.GetProgress(def.id);
00207:
00208:                 if (prog != null && prog.completed)
00209:                 {
00210:                     completedList.Add((def, prog));
00211:                 }
00212:                 else if (prog != null && prog.started && !prog.failed)
00213:                 {
00214:                     if (activeDef == null)
00215:                     {
00216:                         activeDef = def;
00217:                         activeProgress = prog;
00218:                     }
00219:                 }
00220:                 else
00221:                 {
00222:                     // Evaluate availability and lock reasons
00223:                     bool locked = false;
00224:                     string reason = "";
00225:
00226:                     if (prog != null && prog.failed)
00227:                     {
00228:                         locked = true;
00229:                         reason = "Protocol failed and closed on the record.";
00230:                     }
00231:                     else if (def.min_day > _currentDay)
00232:                     {
00233:                         locked = true;
00234:                         reason = $"Available on Day {def.min_day} (Current: Day {_currentDay})";
00235:                     }
00236:                     else if (!string.IsNullOrEmpty(def.prereq_quest_id) && !_expansions.IsCrossingQuestCompleted(def.prereq_quest_id))
00237:                     {
00238:                         var prereqDef = _expansions.CrossingQuests.GetDef(def.prereq_quest_id);
00239:                         string prereqName = prereqDef?.display_name ?? def.prereq_quest_id;
00240:                         locked = true;
00241:                         reason = $"Requires completion of: {prereqName}";
00242:                     }
00243:                     else if (def.id != CrossingQuestSystem.OpeningQuest && !gateOpen && !_expansions.IsCrossingQuestCompleted(CrossingQuestSystem.OpeningQuest))
00244:                     {
00245:                         locked = true;
00246:                         reason = "Requires gate vouch access or opening charter resolution.";
00247:                     }
00248:
00249:                     availableList.Add((def, locked, reason));
00250:                 }
00251:             }
00252:
00253:             // ── Render Active Quest ──────────────────────────────────
00254:             if (activeDef != null && activeProgress != null)
00255:             {
00256:                 _emptyState.Visible = false;
00257:                 var activeCard = BuildActiveQuestCard(activeDef, activeProgress);
00258:                 _activeQuestContainer.AddChild(activeCard);
00259:             }
00260:             else
00261:             {
00262:                 _emptyState.Text = "No active Crossing protocol in progress. Review available charters below.";
00263:                 _emptyState.Visible = availableList.Count == 0;
00264:             }
00265:
00266:             // ── Render Available Protocols ───────────────────────────
00267:             if (availableList.Count > 0)
00268:             {
00269:                 var availHeader = AshfallUiHelpers.MakeSectionHeader("AVAILABLE CROSSING PROTOCOLS");
00270:                 _availableQuestsContainer.AddChild(availHeader);
00271:
00272:                 foreach (var item in availableList)
00273:                 {
00274:                     var card = BuildAvailableQuestCard(item.def, item.locked, item.reason);
00275:                     _availableQuestsContainer.AddChild(card);
00276:                 }
00277:             }
00278:
00279:             // ── Render Completed Protocols ───────────────────────────
00280:             if (completedList.Count > 0)
00281:             {
00282:                 var compCard = AshfallUiHelpers.MakeCardFrame("RESOLVED CROSSING CHARTERS", "LEDGER ARCHIVE");
00283:                 var compBox = compCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00284:
00285:                 foreach (var item in completedList)
00286:                 {
00287:                     string resolution = string.IsNullOrEmpty(item.prog.chosenChoiceId)
00288:                         ? "Charter resolved and recorded."
00289:                         : $"Resolved · Choice recorded [{item.prog.chosenChoiceId}]";
00290:                     compBox.AddChild(AshfallUiHelpers.MakeDataRow($"✓ {item.def.display_name}", resolution, AshfallUiHelpers.ToColor(CoreTheme.Pale)));
00291:                 }
00292:
00293:                 _completedQuestsContainer.AddChild(compCard);
00294:             }
00295:         }
00296:
00297:         // ── Active Quest Card ─────────────────────────────────────────
00298:
00299:         private Control BuildActiveQuestCard(CrossingQuestDef def, CrossingQuestProgress prog)
00300:         {
00301:             int totalStages = def.stages?.Count ?? 0;
00302:             int currentStageIdx = prog.currentStage;
00303:             string subtitle = $"TYPE: {def.type?.ToUpperInvariant() ?? "EXPEDITION"} · STAGE {currentStageIdx + 1}/{Math.Max(1, totalStages)} · TARGET: {def.target_location_id ?? "—"}";
00304:
00305:             var card = AshfallUiHelpers.MakeCardFrame(def.display_name, subtitle);
00306:             var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00307:
00308:             // Briefing
00309:             if (!string.IsNullOrEmpty(def.briefing))
00310:             {
00311:                 var briefHeader = AshfallUiHelpers.MakeSubsectionHeader("DIRECTIVE BRIEFING");
00312:                 cardBox.AddChild(briefHeader);
00313:
00314:                 var briefLbl = AshfallUiHelpers.MakeBody(def.briefing);
00315:                 briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
00316:                 cardBox.AddChild(briefLbl);
00317:                 cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
00318:             }
00319:
00320:             // Stage Objectives
00321:             var stageHeader = AshfallUiHelpers.MakeSubsectionHeader("STAGE OBJECTIVES");
00322:             cardBox.AddChild(stageHeader);
00323:
00324:             if (def.stages != null && def.stages.Count > 0)
00325:             {
00326:                 for (int i = 0; i < def.stages.Count; i++)
00327:                 {
00328:                     var stage = def.stages[i];
00329:                     string marker;
00330:                     Color markerColor;
00331:
00332:                     if (i < currentStageIdx)
00333:                     {
00334:                         marker = "[✓]";
00335:                         markerColor = AshfallUiHelpers.ToColor(CoreTheme.Warm);
00336:                     }
00337:                     else if (i == currentStageIdx)
00338:                     {
00339:                         marker = "[►]";
00340:                         markerColor = AshfallUiHelpers.ToColor(CoreTheme.Hot);
00341:                     }
00342:                     else
00343:                     {
00344:                         marker = "[ ]";
00345:                         markerColor = AshfallUiHelpers.ToColor(CoreTheme.Dim);
00346:                     }
00347:
00348:                     var stageRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00349:                     var stageLbl = AshfallUiHelpers.MakeMono($"{marker} {stage.text ?? "—"}");
00350:                     stageLbl.AddThemeColorOverride("font_color", markerColor);
00351:                     stageLbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00352:                     stageLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00353:                     stageRow.AddChild(stageLbl);
00354:                     cardBox.AddChild(stageRow);
00355:                 }
00356:             }
00357:
00358:             cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
00359:
00360:             // Choices / Interactions
00361:             var choicesHeader = AshfallUiHelpers.MakeSubsectionHeader("AVAILABLE INTERACTIONS & DIRECTIVES");
00362:             cardBox.AddChild(choicesHeader);
00363:
00364:             bool hasChoices = def.choices != null && def.choices.Count > 0;
00365:             bool choiceMade = !string.IsNullOrEmpty(prog.chosenChoiceId);
00366:
00367:             if (hasChoices)
00368:             {
00369:                 foreach (var choice in def.choices!)
00370:                 {
00371:                     bool isThisChoice = prog.chosenChoiceId == choice.id;
00372:                     var choiceBox = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00373:
00374:                     var choiceText = AshfallUiHelpers.MakeBody($"• {choice.text ?? "—"}");
00375:                     choiceText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00376:                     choiceText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00377:                     if (isThisChoice)
00378:                         choiceText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Hot));
00379:                     choiceBox.AddChild(choiceText);
00380:
00381:                     if (isThisChoice)
00382:                     {
00383:                         var chosenTag = AshfallUiHelpers.MakeSmall("[CHOSEN]");
00384:                         chosenTag.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
00385:                         choiceBox.AddChild(chosenTag);
00386:                     }
00387:                     else if (!choiceMade)
00388:                     {
00389:                         string cId = choice.id;
00390:                         string qId = def.id;
00391:                         var btnChoose = AshfallUiHelpers.MakeButton("SELECT", () =>
00392:                         {
00393:                             _expansions?.MakeCrossingChoice(qId, cId);
00394:                             RefreshView();
00395:                         });
00396:                         btnChoose.CustomMinimumSize = new Vector2(100, 32);
00397:                         choiceBox.AddChild(btnChoose);
00398:                     }
00399:
00400:                     cardBox.AddChild(choiceBox);
00401:                 }
00402:             }
00403:             else
00404:             {
00405:                 var noChoiceLbl = AshfallUiHelpers.MakeMetadata("No choice decisions at this stage. Proceed with stage execution.");
00406:                 noChoiceLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
00407:                 cardBox.AddChild(noChoiceLbl);
00408:             }
00409:
00410:             // Action row: Advance stage
00411:             var actRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00412:             actRow.Alignment = BoxContainer.AlignmentMode.End;
00413:
00414:             string advanceText = currentStageIdx + 1 >= totalStages
00415:                 ? "RESOLVE CHARTER PROTOCOL"
00416:                 : $"ADVANCE TO STAGE {currentStageIdx + 2}";
00417:
00418:             string questId = def.id;
00419:             var btnAdvance = AshfallUiHelpers.MakeButton(advanceText, () =>
00420:             {
00421:                 _expansions?.AdvanceCrossingQuestStage(questId);
00422:                 RefreshView();
00423:             });
00424:             btnAdvance.CustomMinimumSize = new Vector2(220, 36);
00425:             actRow.AddChild(btnAdvance);
00426:             cardBox.AddChild(actRow);
00427:
00428:             return card;
00429:         }
00430:
00431:         // ── Available Quest Card ──────────────────────────────────────
00432:
00433:         private Control BuildAvailableQuestCard(CrossingQuestDef def, bool locked, string lockReason)
00434:         {
00435:             string reqs = locked ? "LOCKED" : $"ELIGIBLE · MIN DAY {def.min_day}";
00436:             var card = AshfallUiHelpers.MakeCardFrame(def.display_name, reqs);
00437:             var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00438:
00439:             if (!string.IsNullOrEmpty(def.briefing))
00440:             {
00441:                 var briefLbl = AshfallUiHelpers.MakeSmall(def.briefing);
00442:                 briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
00443:                 cardBox.AddChild(briefLbl);
00444:             }
00445:
00446:             if (locked)
00447:             {
00448:                 var lockLbl = AshfallUiHelpers.MakeSmall($"⚠ {lockReason}");
00449:                 lockLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
00450:                 cardBox.AddChild(lockLbl);
00451:             }
00452:             else
00453:             {
00454:                 var btnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
00455:                 string qId = def.id;
00456:                 var btnStart = AshfallUiHelpers.MakeButton($"INITIATE PROTOCOL // [{def.display_name}]", () =>
00457:                 {
00458:                     _expansions?.StartCrossingQuest(qId, _currentDay);
00459:                     RefreshView();
00460:                 });
00461:                 btnStart.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00462:                 btnRow.AddChild(btnStart);
00463:                 cardBox.AddChild(btnRow);
00464:             }
00465:
00466:             return card;
00467:         }
00468:
00469:         private static void ClearContainer(VBoxContainer container)
00470:         {
00471:             AshfallUiHelpers.EmptyChildren(container);
00472:         }
00473:
00474:
00475:     public void Unbind()
00476:     {
00477:         if (_expansions?.CrossingQuests != null)
00478:             {
00479:                 _expansions.CrossingQuests.OnStateChanged -= OnStateChangedHandler;
00480:             }
00481:             if (_vouch != null)
00482:             {
00483:                 _vouch.OnStateChanged -= OnVouchChangedHandler;
00484:             }
00485:     }
00486:
00487:     public override void _ExitTree()
00488:         {
00489:             Unbind();
00490:             base._ExitTree();
00491:         }
00492:     }
00493: }
```

## `src/UI/CrossingSafeConductVouchPanel.cs` — 100 lines; 5,150 bytes; SHA-256 `16fd13b921b1614eadc4becd8e17e90fd2e6a9fe8ce275e7b87c2f5e562e4825`
Declaration index:
- 00009: public partial class CrossingSafeConductVouchPanel : Control, IBindablePanel
- 00029: public void Open()
- 00035: public void Bind(object? session)
- 00041: public void Unbind()
- 00046: public void RefreshView()
- 00054: private void BuildInterface()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core.UI;
00005: using DesignTheme = Ashfall.Core.UI.Theme;
00006:
00007: namespace AtomicWar.GodotApp.UI
00008: {
00009:     public partial class CrossingSafeConductVouchPanel : Control, IBindablePanel
00010:     {
00011:         public event Action? OnClose;
00012:
00013:         private Label? _headerTitleLabel;
00014:         private Label? _statusBadgeLabel;
00015:         private Button? _closeButton;
00016:         private VBoxContainer? _telemetryContainer;
00017:         private VBoxContainer? _buttonContainer;
00018:         private VBoxContainer? _dataContainer;
00019:         private Label? _logOutputLabel;
00020:
00021:         public bool IsBound { get; private set; } = true;
00022:
00023:         public override void _Ready()
00024:         {
00025:             SetAnchorsPreset(LayoutPreset.FullRect);
00026:             BuildInterface();
00027:         }
00028:
00029:         public void Open()
00030:         {
00031:             Visible = true;
00032:             RefreshView();
00033:         }
00034:
00035:         public void Bind(object? session)
00036:         {
00037:             IsBound = true;
00038:             RefreshView();
00039:         }
00040:
00041:         public void Unbind()
00042:         {
00043:             IsBound = false;
00044:         }
00045:
00046:         public void RefreshView()
00047:         {
00048:             if (_statusBadgeLabel != null)
00049:             {
00050:                 _statusBadgeLabel.Text = "STATUS: BRIDGEHEAD ARMED - TRANSIT PERMITS: 14 / SURCHARGE: +25%";
00051:             }
00052:         }
00053:
00054:         private void BuildInterface()
00055:         {
00056:             var chrome = ThreePanePanelScaffold.BuildChrome(
00057:                 this,
00058:                 "FRONTIER CHECKPOINT // CROSSING SAFE-CONDUCT VOUCH [VOUCH-01]",
00059:                 "STATUS: BRIDGEHEAD ARMED - TRANSIT PERMITS: 14 / SURCHARGE: +25%",
00060:                 AshfallUiHelpers.ToColor(DesignTheme.Warm),
00061:                 "[X] CLOSE CONSOLE",
00062:                 "[VOUCH-01] Caravan Salt Walker cleared checkpoint. Toll paid 150 Scrip.\n[VOUCH-01] Contraband scan negative on sector gate 2.",
00063:                 () => OnClose?.Invoke());
00064:             _headerTitleLabel = chrome.Title;
00065:             _statusBadgeLabel = chrome.Status;
00066:             _closeButton = chrome.Close;
00067:             _logOutputLabel = chrome.Log;
00068:             var bodyHBox = chrome.Body;
00069:
00070:             // Left Column (Telemetry)
00071:             var leftPanel = ThreePanePanelScaffold.CreatePanelFrame("RIVER GORGE SECTOR GRID & QUEUE");
00072:             bodyHBox.AddChild(leftPanel);
00073:             _telemetryContainer = ThreePanePanelScaffold.CreateColumn(leftPanel, 8);
00074:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SENTRY ALERTNESS INDEX", "92% [LETHAL FORCE AUTHORIZED]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
00075:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("INCOMING CARAVAN", "SALT WALKER (6 PACK BEASTS)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00076:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("SUSPICIOUS DESERTER CELL", "GRAY RATS (3 ARMED MEN)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
00077:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CONTRABAND SCANNER", "NO ACTIVE ISOTOPES DETECTED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00078:             _telemetryContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("GORGE WIND VELOCITY", "42 KM/H CROSSWIND", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00079:
00080:             // Center Column (Interactive Controls)
00081:             var centerPanel = ThreePanePanelScaffold.CreatePanelFrame("PARCHMENT STAMP & TARIFF CALCULATOR");
00082:             bodyHBox.AddChild(centerPanel);
00083:             _buttonContainer = ThreePanePanelScaffold.CreateColumn(centerPanel, 12);
00084:             _buttonContainer.AddChild(new Button { Text = "[STAMP AUTHORIZED SAFE-CONDUCT PASS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
00085:             _buttonContainer.AddChild(new Button { Text = "[CONFISCATE CONTRABAND & DETAIN]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
00086:             _buttonContainer.AddChild(new Button { Text = "[RAISE HEAVY SPIKE BARRIER]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
00087:             _buttonContainer.AddChild(new Button { Text = "[FIRE WARNING SHOT ACROSS GORGE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
00088:
00089:             // Right Column (Data & Logistics)
00090:             var rightPanel = ThreePanePanelScaffold.CreatePanelFrame("TOLL VAULT & REFUGEE QUOTA");
00091:             bodyHBox.AddChild(rightPanel);
00092:             _dataContainer = ThreePanePanelScaffold.CreateColumn(rightPanel, 8);
00093:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("COLLECTED TOLL SCRIP", "1,840 SCRIP IN VAULT", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00094:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("CONFISCATED WEAPONS", "6 RIFLES / 1 DYNAMITE CRATE", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
00095:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("DAILY REFUGEE QUOTA", "14 / 20 PERMITS ISSUED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
00096:             _dataContainer.AddChild(ThreePanePanelScaffold.CreateTelemetryRow("RUST GUILD STANDING", "+60 [PREFERENTIAL RATE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
00097:
00098:         }
00099:     }
00100: }
```

## `Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs` — 228 lines; 9,850 bytes; SHA-256 `ed286004dcd14d3885275fbff1ed4a6a3663bff6d4de7182b9de7c712fb68811`
Declaration index:
- 00019: public sealed class CrossingItemsPlan126Tests
- 00061: private static string ResolveDataDir()
- 00070: private static JsonElement LoadRoot(string file)
- 00076: private static JsonElement LoadItemsArray()
- 00083: private static JsonElement FindEntry(string id)
- 00094: private static ItemCatalog LoadGlobalCatalog()
- 00103: public void CrossingCatalog_ContainsExactlyTwentyFiveItems()
- 00113: public void CrossingCatalog_PreservesOriginalElevenAndAddsExactlyFourteen()
- 00127: public void CrossingCatalog_AllEntriesUseSupportedTypesAndNumericRanges()
- 00145: public void CrossingCatalog_OriginalNumericDefinitionsRemainUnchanged()
- 00178: public void GlobalCatalog_RegistersAllFourteenNewItems()
- 00186: public void GlobalCatalog_UsesCanonicalConsumableSemantics()
- 00208: public void ProposedIdsDoNotCollideAcrossGlobalItemFiles()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.IO;
00009: using Ashfall.Core.Inventory;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests
00013: {
00014:     /// <summary>
00015:     /// Plan 126 — Crossing item catalog expansion. The tests keep the original
00016:     /// eleven definitions stable while proving the fourteen additions remain
00017:     /// valid in both the local Crossing catalog and the merged global registry.
00018:     /// </summary>
00019:     public sealed class CrossingItemsPlan126Tests
00020:     {
00021:         private static readonly string[] OriginalIds =
00022:         {
00023:             "item_vouch_token_crossing",
00024:             "item_calibration_weight",
00025:             "item_crossing_traded_grain",
00026:             "item_crossing_traded_salt",
00027:             "item_crossing_pledge_slip",
00028:             "item_charter_three_pages",
00029:             "item_debt_contract_copy",
00030:             "item_marker_rubbing",
00031:             "item_duty_log_fragment",
00032:             "item_trade_manifest_blank",
00033:             "item_wyn_receipt_paid"
00034:         };
00035:
00036:         private static readonly string[] NewIds =
00037:         {
00038:             "item_arbitration_token",
00039:             "item_charter_stamp",
00040:             "item_weighbridge_chit",
00041:             "item_smuggled_medicine",
00042:             "item_crossing_bread",
00043:             "item_lamp_oil_crossing",
00044:             "item_filtered_water_crossing",
00045:             "item_quarantine_bands",
00046:             "item_granary_receipt",
00047:             "item_smugglers_ledger",
00048:             "item_rejection_notice",
00049:             "item_crossing_map",
00050:             "item_black_market_pouch",
00051:             "item_charter_draft"
00052:         };
00053:
00054:         private static readonly HashSet<string> AcceptedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
00055:         {
00056:             "Food", "Water", "IrradiatedWater", "Medical", "AntiRad", "Iodine",
00057:             "Protective", "Tool", "Fuel", "Filter", "Material", "Trade", "Comfort",
00058:             "Quest", "Device", "Weapon", "Corpse", "ContaminatedFood", "Relic"
00059:         };
00060:
00061:         private static string ResolveDataDir()
00062:         {
00063:             string baseDir = AppDomain.CurrentDomain.BaseDirectory;
00064:             string dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../../Assets/StreamingAssets/Data"));
00065:             if (!Directory.Exists(dataDir))
00066:                 dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../Assets/StreamingAssets/Data"));
00067:             return dataDir;
00068:         }
00069:
00070:         private static JsonElement LoadRoot(string file)
00071:         {
00072:             using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(ResolveDataDir(), file)));
00073:             return document.RootElement.Clone();
00074:         }
00075:
00076:         private static JsonElement LoadItemsArray()
00077:         {
00078:             var root = LoadRoot("crossing_items.json");
00079:             Assert.Equal(JsonValueKind.Object, root.ValueKind);
00080:             return root.GetProperty("items");
00081:         }
00082:
00083:         private static JsonElement FindEntry(string id)
00084:         {
00085:             foreach (var entry in LoadItemsArray().EnumerateArray())
00086:             {
00087:                 if (entry.GetProperty("id").GetString() == id)
00088:                     return entry;
00089:             }
00090:
00091:             throw new Xunit.Sdk.XunitException($"Crossing item '{id}' is missing");
00092:         }
00093:
00094:         private static ItemCatalog LoadGlobalCatalog()
00095:         {
00096:             return ItemCatalogLoader.LoadCatalog(
00097:                 ResolveDataDir(),
00098:                 new FileSystemIO(),
00099:                 new SystemTextJsonSerializer());
00100:         }
00101:
00102:         [Fact]
00103:         public void CrossingCatalog_ContainsExactlyTwentyFiveItems()
00104:         {
00105:             Assert.Equal(25, LoadItemsArray().GetArrayLength());
00106:
00107:             var loader = new CrossingCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
00108:             var catalog = loader.Load(ResolveDataDir());
00109:             Assert.Equal(25, catalog.Items.Count);
00110:         }
00111:
00112:         [Fact]
00113:         public void CrossingCatalog_PreservesOriginalElevenAndAddsExactlyFourteen()
00114:         {
00115:             var ids = LoadItemsArray()
00116:                 .EnumerateArray()
00117:                 .Select(e => e.GetProperty("id").GetString()!)
00118:                 .ToList();
00119:
00120:             Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
00121:             Assert.Equal(11, OriginalIds.Intersect(ids, StringComparer.Ordinal).Count());
00122:             Assert.Equal(14, NewIds.Intersect(ids, StringComparer.Ordinal).Count());
00123:             Assert.Equal(25, OriginalIds.Concat(NewIds).Intersect(ids, StringComparer.Ordinal).Count());
00124:         }
00125:
00126:         [Fact]
00127:         public void CrossingCatalog_AllEntriesUseSupportedTypesAndNumericRanges()
00128:         {
00129:             foreach (var entry in LoadItemsArray().EnumerateArray())
00130:             {
00131:                 string id = entry.GetProperty("id").GetString()!;
00132:                 string type = entry.GetProperty("type").GetString()!;
00133:                 Assert.Contains(type, AcceptedTypes);
00134:                 Assert.True(entry.GetProperty("stackMax").GetInt32() >= 1, $"{id} stackMax must be positive");
00135:                 Assert.True(float.IsFinite(entry.GetProperty("weight").GetSingle()), $"{id} weight must be finite");
00136:                 Assert.True(entry.GetProperty("weight").GetSingle() >= 0f, $"{id} weight must be non-negative");
00137:                 Assert.True(float.IsFinite(entry.GetProperty("tradeValue").GetSingle()), $"{id} tradeValue must be finite");
00138:                 Assert.True(entry.GetProperty("tradeValue").GetSingle() >= 0f, $"{id} tradeValue must be non-negative");
00139:                 Assert.False(string.IsNullOrWhiteSpace(entry.GetProperty("displayName").GetString()), $"{id} display name missing");
00140:                 Assert.False(string.IsNullOrWhiteSpace(entry.GetProperty("description").GetString()), $"{id} description missing");
00141:             }
00142:         }
00143:
00144:         [Fact]
00145:         public void CrossingCatalog_OriginalNumericDefinitionsRemainUnchanged()
00146:         {
00147:             var expected = new Dictionary<string, (string type, int stack, float weight, float value, float thirst, float hunger, float morale)>
00148:             {
00149:                 ["item_vouch_token_crossing"] = ("Quest", 1, 0.1f, 50f, 0f, 0f, 0f),
00150:                 ["item_calibration_weight"] = ("Tool", 1, 2f, 80f, 0f, 0f, 0f),
00151:                 ["item_crossing_traded_grain"] = ("Trade", 10, 12f, 30f, 0f, 0f, 0f),
00152:                 ["item_crossing_traded_salt"] = ("Trade", 8, 3f, 22f, 0f, 0f, 0f),
00153:                 ["item_crossing_pledge_slip"] = ("Quest", 1, 0.1f, 5f, 0f, 0f, 0f),
00154:                 ["item_charter_three_pages"] = ("Quest", 1, 0.1f, 100f, 0f, 0f, 10f),
00155:                 ["item_debt_contract_copy"] = ("Quest", 1, 0.1f, 10f, 0f, 0f, 0f),
00156:                 ["item_marker_rubbing"] = ("Quest", 1, 0.1f, 15f, 0f, 0f, 0f),
00157:                 ["item_duty_log_fragment"] = ("Quest", 1, 0.1f, 25f, 0f, 0f, 0f),
00158:                 ["item_trade_manifest_blank"] = ("Tool", 5, 0.2f, 12f, 0f, 0f, 0f),
00159:                 ["item_wyn_receipt_paid"] = ("Quest", 1, 0.1f, 5f, 0f, 0f, 5f)
00160:             };
00161:
00162:             foreach (var id in OriginalIds)
00163:             {
00164:                 var entry = FindEntry(id);
00165:                 var actual = (
00166:                     entry.GetProperty("type").GetString()!,
00167:                     entry.GetProperty("stackMax").GetInt32(),
00168:                     entry.GetProperty("weight").GetSingle(),
00169:                     entry.GetProperty("tradeValue").GetSingle(),
00170:                     entry.GetProperty("thirstRestore").GetSingle(),
00171:                     entry.GetProperty("hungerRestore").GetSingle(),
00172:                     entry.GetProperty("moraleEffect").GetSingle());
00173:                 Assert.Equal(expected[id], actual);
00174:             }
00175:         }
00176:
00177:         [Fact]
00178:         public void GlobalCatalog_RegistersAllFourteenNewItems()
00179:         {
00180:             var catalog = LoadGlobalCatalog();
00181:             foreach (string id in NewIds)
00182:                 Assert.True(catalog.Contains(id), $"global item registry missing '{id}'");
00183:         }
00184:
00185:         [Fact]
00186:         public void GlobalCatalog_UsesCanonicalConsumableSemantics()
00187:         {
00188:             var catalog = LoadGlobalCatalog();
00189:
00190:             var bread = catalog.Get("item_crossing_bread")!;
00191:             Assert.Equal(ItemType.Food, bread.type);
00192:             Assert.Equal(22f, bread.hungerRestore);
00193:             Assert.Equal(0f, bread.thirstRestore);
00194:
00195:             var water = catalog.Get("item_filtered_water_crossing")!;
00196:             Assert.Equal(ItemType.Water, water.type);
00197:             Assert.Equal(40f, water.thirstRestore);
00198:             Assert.Equal(0f, water.hungerRestore);
00199:
00200:             var medicine = catalog.Get("item_smuggled_medicine")!;
00201:             Assert.Equal(ItemType.Medical, medicine.type);
00202:             Assert.Equal(20f, medicine.healthEffect);
00203:             Assert.Equal(0f, medicine.hungerRestore);
00204:             Assert.Equal(0f, medicine.thirstRestore);
00205:         }
00206:
00207:         [Fact]
00208:         public void ProposedIdsDoNotCollideAcrossGlobalItemFiles()
00209:         {
00210:             var occurrences = NewIds.ToDictionary(id => id, _ => 0, StringComparer.Ordinal);
00211:             foreach (string file in Directory.GetFiles(ResolveDataDir(), "*items.json"))
00212:             {
00213:                 using var document = JsonDocument.Parse(File.ReadAllText(file));
00214:                 if (!document.RootElement.TryGetProperty("items", out var items) || items.ValueKind != JsonValueKind.Array)
00215:                     continue;
00216:                 foreach (var entry in items.EnumerateArray())
00217:                 {
00218:                     string? id = entry.TryGetProperty("id", out var idElement) ? idElement.GetString() : null;
00219:                     if (id != null && occurrences.ContainsKey(id))
00220:                         occurrences[id]++;
00221:                 }
00222:             }
00223:
00224:             foreach (var pair in occurrences)
00225:                 Assert.Equal(1, pair.Value);
00226:         }
00227:     }
00228: }
```

## `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs` — 353 lines; 15,117 bytes; SHA-256 `1bb5e0d2b6d7c7297e4ef0ef0c990f1f93660599bdea2268843187d64f958520`
Declaration index:
- 00012: public sealed class CrossingFactionExpansionTests
- 00014: private static string ResolveDataDir()
- 00024: private static CrossingCatalog LoadCatalog()
- 00034: public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
- 00042: public void BaselineThreeFactions_PreservedVerbatim()
- 00090: public void NewFiveFactions_ArePresentWithExpectedAttributes()
- 00161: public void FactionIds_AreUnique_AndStartWithPrefix()
- 00178: public void DisplayNames_AreNonEmpty_AndDistinct()
- 00193: public void Alignments_AreValid()
- 00208: public void HomeRegions_AreValidCrossingRegion()
- 00218: public void IsActive_IsTrueForAll()
- 00228: public void Trust_ValuesAreWithinValidRange()
- 00238: public void Wants_AreNonEmptyArrays_WithValidNonEmptyItems()
- 00254: public void Offers_AreNonEmptyArrays_WithValidNonEmptyItems()
- 00270: public void SignatureQuotes_AreNonEmpty_SingleSentences()
- 00283: public void AccessRules_AreNonEmpty_AndSubstantive()
- 00295: public void TradeProfiles_AreDistinctAcrossAllEight()
- 00317: public void CrossingIds_ConstantsMatchDataCatalog()
- 00341: public void FactionIconCatalog_ResolvesOrFallsBackSafely()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Xunit;
00007: using Ashfall.Core;
00008: using Ashfall.Core.UI;
00009:
00010: namespace Ashfall.Core.Tests
00011: {
00012:     public sealed class CrossingFactionExpansionTests
00013:     {
00014:         private static string ResolveDataDir()
00015:         {
00016:             string start = Directory.GetCurrentDirectory();
00017:             if (CatalogLocator.TryFindDataDirectory(start, out string found))
00018:                 return found;
00019:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00020:                 return found;
00021:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00022:         }
00023:
00024:         private static CrossingCatalog LoadCatalog()
00025:         {
00026:             string dataDir = ResolveDataDir();
00027:             var files = new FileSystemIO();
00028:             var json = new SystemTextJsonSerializer();
00029:             var loader = new CrossingCatalogLoader(files, json);
00030:             return loader.Load(dataDir);
00031:         }
00032:
00033:         [Fact]
00034:         public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
00035:         {
00036:             var catalog = LoadCatalog();
00037:             Assert.NotNull(catalog);
00038:             Assert.Equal(8, catalog.Factions.Count);
00039:         }
00040:
00041:         [Fact]
00042:         public void BaselineThreeFactions_PreservedVerbatim()
00043:         {
00044:             var catalog = LoadCatalog();
00045:
00046:             // 1. The Scale
00047:             var scale = catalog.GetFaction(CrossingIds.FactionScale);
00048:             Assert.NotNull(scale);
00049:             Assert.Equal("The Scale", scale.display_name);
00050:             Assert.Equal("conditional", scale.alignment);
00051:             Assert.Equal(CrossingIds.Region, scale.home_region);
00052:             Assert.True(scale.is_active);
00053:             Assert.Equal(0, scale.trust);
00054:             Assert.Equal(new[] { "trade_goods" }, scale.wants);
00055:             Assert.Equal(new[] { "stallrow_trade_access", "verification" }, scale.offers);
00056:             Assert.Equal("The brass scales do not lie, and the numbers have no conscience. What people infer from their poverty is not my problem.", scale.signature_quote);
00057:             Assert.Equal("An agonizingly precise weigh-in is the price of doing business at Stallrow. Contest a true reading without cause, and the market stays open—but your name goes on the slate.", scale.access_rule);
00058:             Assert.Equal(string.Empty, scale.badge_asset_id);
00059:
00060:             // 2. The Underwrite
00061:             var underwrite = catalog.GetFaction(CrossingIds.FactionUnderwrite);
00062:             Assert.NotNull(underwrite);
00063:             Assert.Equal("The Underwrite", underwrite.display_name);
00064:             Assert.Equal("conditional", underwrite.alignment);
00065:             Assert.Equal(CrossingIds.Region, underwrite.home_region);
00066:             Assert.True(underwrite.is_active);
00067:             Assert.Equal(0, underwrite.trust);
00068:             Assert.Equal(new[] { "pledged_goods" }, underwrite.wants);
00069:             Assert.Equal(new[] { "seed_stock", "covered_loss", "favour_bank" }, underwrite.offers);
00070:             Assert.Equal("Read it twice, under the sodium glare. I'll say it twice. After the second time, there is only the ink, and the debt it binds you to.", underwrite.signature_quote);
00071:             Assert.Equal("Their 'help' is genuine, offered against a plainly named, brutal forfeit—a child's labor, a pound of flesh, a year of servitude. Sign, negotiate, or walk away; no hidden clause survives a second reading.", underwrite.access_rule);
00072:             Assert.Equal(string.Empty, underwrite.badge_asset_id);
00073:
00074:             // 3. The Compact
00075:             var compact = catalog.GetFaction(CrossingIds.FactionCompact);
00076:             Assert.NotNull(compact);
00077:             Assert.Equal("The Compact", compact.display_name);
00078:             Assert.Equal("peaceful", compact.alignment);
00079:             Assert.Equal(CrossingIds.Region, compact.home_region);
00080:             Assert.True(compact.is_active);
00081:             Assert.Equal(0, compact.trust);
00082:             Assert.Equal(new[] { "signatories" }, compact.wants);
00083:             Assert.Equal(new[] { "charter_draft", "ratification" }, compact.offers);
00084:             Assert.Equal("There is a document now, stained with ash and thumbprints. Let them argue with the paper instead of each other's throats.", compact.signature_quote);
00085:             Assert.Equal("Sign the blood-flecked draft and you are on the record. Perrin will not ratify a clause he knows will break a man—but he has not yet seen every way the words can be twisted.", compact.access_rule);
00086:             Assert.Equal(string.Empty, compact.badge_asset_id);
00087:         }
00088:
00089:         [Fact]
00090:         public void NewFiveFactions_ArePresentWithExpectedAttributes()
00091:         {
00092:             var catalog = LoadCatalog();
00093:
00094:             // 4. The Lamplighters
00095:             var lamplighters = catalog.GetFaction(CrossingIds.FactionLamplighters);
00096:             Assert.NotNull(lamplighters);
00097:             Assert.Equal("The Lamplighters", lamplighters.display_name);
00098:             Assert.Equal("conditional", lamplighters.alignment);
00099:             Assert.Equal(CrossingIds.Region, lamplighters.home_region);
00100:             Assert.True(lamplighters.is_active);
00101:             Assert.Equal(0, lamplighters.trust);
00102:             Assert.Contains("fuel_stores", lamplighters.wants);
00103:             Assert.Contains("street_lighting", lamplighters.offers);
00104:             Assert.False(string.IsNullOrWhiteSpace(lamplighters.signature_quote));
00105:             Assert.False(string.IsNullOrWhiteSpace(lamplighters.access_rule));
00106:
00107:             // 5. The Granary Wardens
00108:             var granary = catalog.GetFaction(CrossingIds.FactionGranaryWardens);
00109:             Assert.NotNull(granary);
00110:             Assert.Equal("The Granary Wardens", granary.display_name);
00111:             Assert.Equal("conditional", granary.alignment);
00112:             Assert.Equal(CrossingIds.Region, granary.home_region);
00113:             Assert.True(granary.is_active);
00114:             Assert.Equal(0, granary.trust);
00115:             Assert.Contains("staple_grain", granary.wants);
00116:             Assert.Contains("ration_distribution", granary.offers);
00117:             Assert.False(string.IsNullOrWhiteSpace(granary.signature_quote));
00118:             Assert.False(string.IsNullOrWhiteSpace(granary.access_rule));
00119:
00120:             // 6. The Water Committee
00121:             var water = catalog.GetFaction(CrossingIds.FactionWaterCommittee);
00122:             Assert.NotNull(water);
00123:             Assert.Equal("The Water Committee", water.display_name);
00124:             Assert.Equal("conditional", water.alignment);
00125:             Assert.Equal(CrossingIds.Region, water.home_region);
00126:             Assert.True(water.is_active);
00127:             Assert.Equal(0, water.trust);
00128:             Assert.Contains("filter_media", water.wants);
00129:             Assert.Contains("clean_water_rights", water.offers);
00130:             Assert.False(string.IsNullOrWhiteSpace(water.signature_quote));
00131:             Assert.False(string.IsNullOrWhiteSpace(water.access_rule));
00132:
00133:             // 7. The Quarantine Post
00134:             var quarantine = catalog.GetFaction(CrossingIds.FactionQuarantinePost);
00135:             Assert.NotNull(quarantine);
00136:             Assert.Equal("The Quarantine Post", quarantine.display_name);
00137:             Assert.Equal("neutral", quarantine.alignment);
00138:             Assert.Equal(CrossingIds.Region, quarantine.home_region);
00139:             Assert.True(quarantine.is_active);
00140:             Assert.Equal(0, quarantine.trust);
00141:             Assert.Contains("medical_tinctures", quarantine.wants);
00142:             Assert.Contains("gate_health_screening", quarantine.offers);
00143:             Assert.False(string.IsNullOrWhiteSpace(quarantine.signature_quote));
00144:             Assert.False(string.IsNullOrWhiteSpace(quarantine.access_rule));
00145:
00146:             // 8. The Smugglers' Court
00147:             var smugglers = catalog.GetFaction(CrossingIds.FactionSmugglersCourt);
00148:             Assert.NotNull(smugglers);
00149:             Assert.Equal("The Smugglers' Court", smugglers.display_name);
00150:             Assert.Equal("conditional", smugglers.alignment);
00151:             Assert.Equal(CrossingIds.Region, smugglers.home_region);
00152:             Assert.True(smugglers.is_active);
00153:             Assert.Equal(0, smugglers.trust);
00154:             Assert.Contains("unchartered_salvage", smugglers.wants);
00155:             Assert.Contains("off_ledger_trade", smugglers.offers);
00156:             Assert.False(string.IsNullOrWhiteSpace(smugglers.signature_quote));
00157:             Assert.False(string.IsNullOrWhiteSpace(smugglers.access_rule));
00158:         }
00159:
00160:         [Fact]
00161:         public void FactionIds_AreUnique_AndStartWithPrefix()
00162:         {
00163:             var catalog = LoadCatalog();
00164:             var seen = new HashSet<string>(StringComparer.Ordinal);
00165:
00166:             foreach (var f in catalog.Factions)
00167:             {
00168:                 Assert.NotNull(f.id);
00169:                 Assert.StartsWith("faction_the_", f.id);
00170:                 Assert.DoesNotContain(" ", f.id);
00171:                 Assert.True(seen.Add(f.id), $"Duplicate faction ID '{f.id}' found.");
00172:             }
00173:
00174:             Assert.Equal(8, seen.Count);
00175:         }
00176:
00177:         [Fact]
00178:         public void DisplayNames_AreNonEmpty_AndDistinct()
00179:         {
00180:             var catalog = LoadCatalog();
00181:             var names = new HashSet<string>(StringComparer.Ordinal);
00182:
00183:             foreach (var f in catalog.Factions)
00184:             {
00185:                 Assert.False(string.IsNullOrWhiteSpace(f.display_name));
00186:                 Assert.True(names.Add(f.display_name), $"Duplicate display name '{f.display_name}' found.");
00187:             }
00188:
00189:             Assert.Equal(8, names.Count);
00190:         }
00191:
00192:         [Fact]
00193:         public void Alignments_AreValid()
00194:         {
00195:             var catalog = LoadCatalog();
00196:             var validAlignments = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
00197:             {
00198:                 "conditional", "peaceful", "neutral", "allied", "hostile"
00199:             };
00200:
00201:             foreach (var f in catalog.Factions)
00202:             {
00203:                 Assert.Contains(f.alignment, validAlignments);
00204:             }
00205:         }
00206:
00207:         [Fact]
00208:         public void HomeRegions_AreValidCrossingRegion()
00209:         {
00210:             var catalog = LoadCatalog();
00211:             foreach (var f in catalog.Factions)
00212:             {
00213:                 Assert.Equal(CrossingIds.Region, f.home_region);
00214:             }
00215:         }
00216:
00217:         [Fact]
00218:         public void IsActive_IsTrueForAll()
00219:         {
00220:             var catalog = LoadCatalog();
00221:             foreach (var f in catalog.Factions)
00222:             {
00223:                 Assert.True(f.is_active, $"Faction '{f.id}' should be active.");
00224:             }
00225:         }
00226:
00227:         [Fact]
00228:         public void Trust_ValuesAreWithinValidRange()
00229:         {
00230:             var catalog = LoadCatalog();
00231:             foreach (var f in catalog.Factions)
00232:             {
00233:                 Assert.InRange(f.trust, -50f, 50f);
00234:             }
00235:         }
00236:
00237:         [Fact]
00238:         public void Wants_AreNonEmptyArrays_WithValidNonEmptyItems()
00239:         {
00240:             var catalog = LoadCatalog();
00241:             foreach (var f in catalog.Factions)
00242:             {
00243:                 Assert.NotNull(f.wants);
00244:                 Assert.NotEmpty(f.wants);
00245:                 foreach (string want in f.wants)
00246:                 {
00247:                     Assert.False(string.IsNullOrWhiteSpace(want));
00248:                     Assert.Equal(want.ToLowerInvariant(), want);
00249:                 }
00250:             }
00251:         }
00252:
00253:         [Fact]
00254:         public void Offers_AreNonEmptyArrays_WithValidNonEmptyItems()
00255:         {
00256:             var catalog = LoadCatalog();
00257:             foreach (var f in catalog.Factions)
00258:             {
00259:                 Assert.NotNull(f.offers);
00260:                 Assert.NotEmpty(f.offers);
00261:                 foreach (string offer in f.offers)
00262:                 {
00263:                     Assert.False(string.IsNullOrWhiteSpace(offer));
00264:                     Assert.Equal(offer.ToLowerInvariant(), offer);
00265:                 }
00266:             }
00267:         }
00268:
00269:         [Fact]
00270:         public void SignatureQuotes_AreNonEmpty_SingleSentences()
00271:         {
00272:             var catalog = LoadCatalog();
00273:             foreach (var f in catalog.Factions)
00274:             {
00275:                 Assert.False(string.IsNullOrWhiteSpace(f.signature_quote));
00276:                 string quote = f.signature_quote.Trim();
00277:                 Assert.True(quote.EndsWith(".") || quote.EndsWith("!") || quote.EndsWith("?"),
00278:                     $"Quote for '{f.id}' does not end with sentence-final punctuation: {quote}");
00279:             }
00280:         }
00281:
00282:         [Fact]
00283:         public void AccessRules_AreNonEmpty_AndSubstantive()
00284:         {
00285:             var catalog = LoadCatalog();
00286:             foreach (var f in catalog.Factions)
00287:             {
00288:                 Assert.False(string.IsNullOrWhiteSpace(f.access_rule));
00289:                 Assert.True(f.access_rule.Length >= 40,
00290:                     $"Access rule for '{f.id}' is suspiciously short ({f.access_rule.Length} chars).");
00291:             }
00292:         }
00293:
00294:         [Fact]
00295:         public void TradeProfiles_AreDistinctAcrossAllEight()
00296:         {
00297:             var catalog = LoadCatalog();
00298:             var wantsProfiles = new HashSet<string>(StringComparer.Ordinal);
00299:             var offersProfiles = new HashSet<string>(StringComparer.Ordinal);
00300:
00301:             foreach (var f in catalog.Factions)
00302:             {
00303:                 string sortedWants = string.Join(",", f.wants.OrderBy(w => w, StringComparer.Ordinal));
00304:                 string sortedOffers = string.Join(",", f.offers.OrderBy(o => o, StringComparer.Ordinal));
00305:
00306:                 Assert.True(wantsProfiles.Add(sortedWants),
00307:                     $"Duplicate wants profile '{sortedWants}' in faction '{f.id}'.");
00308:                 Assert.True(offersProfiles.Add(sortedOffers),
00309:                     $"Duplicate offers profile '{sortedOffers}' in faction '{f.id}'.");
00310:             }
00311:
00312:             Assert.Equal(8, wantsProfiles.Count);
00313:             Assert.Equal(8, offersProfiles.Count);
00314:         }
00315:
00316:         [Fact]
00317:         public void CrossingIds_ConstantsMatchDataCatalog()
00318:         {
00319:             var catalog = LoadCatalog();
00320:             string[] expectedIds =
00321:             {
00322:                 CrossingIds.FactionScale,
00323:                 CrossingIds.FactionUnderwrite,
00324:                 CrossingIds.FactionCompact,
00325:                 CrossingIds.FactionLamplighters,
00326:                 CrossingIds.FactionGranaryWardens,
00327:                 CrossingIds.FactionWaterCommittee,
00328:                 CrossingIds.FactionQuarantinePost,
00329:                 CrossingIds.FactionSmugglersCourt
00330:             };
00331:
00332:             foreach (string expectedId in expectedIds)
00333:             {
00334:                 var faction = catalog.GetFaction(expectedId);
00335:                 Assert.NotNull(faction);
00336:                 Assert.Equal(expectedId, faction.id);
00337:             }
00338:         }
00339:
00340:         [Fact]
00341:         public void FactionIconCatalog_ResolvesOrFallsBackSafely()
00342:         {
00343:             var catalog = LoadCatalog();
00344:             foreach (var f in catalog.Factions)
00345:             {
00346:                 string iconPath = FactionIconCatalog.Resolve(f.id);
00347:                 Assert.False(string.IsNullOrWhiteSpace(iconPath));
00348:                 Assert.StartsWith("assets/ui/Icons/", iconPath);
00349:                 Assert.EndsWith(".png", iconPath);
00350:             }
00351:         }
00352:     }
00353: }
```

## `Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs` — 233 lines; 11,079 bytes; SHA-256 `aa4f405426b00ae803ede9eb65259a230dcd601dee7ed142ef8d794a56f1beb2`
Declaration index:
- 00013: public sealed class Plan126_129CrossingFoundryIntegrationTests
- 00059: private sealed class FoundryCatalogJson
- 00065: private sealed class FoundryProductJson
- 00080: private sealed class FoundryIngredientJson
- 00087: public void Plan126_CrossingItems_LoadsExactTwentyFiveItemsAndValidatesExpansionTier()
- 00116: public void Plan129_FoundryProduction_LoadsExactThirtyFiveProductsAndValidatesNineExpansionProducts()
- 00158: public void Plan126_129_CrossDomainCoherence_IndustrialSupplyChainAndBorderCommerceLinkages()
- 00194: public void Plan126_129_DeterministicInventoryTradeAndProductionSimulation()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using System.Text.Json;
00007: using Ashfall.Core;
00008: using Ashfall.Core.IO;
00009: using Xunit;
00010:
00011: namespace Ashfall.Core.Tests.World
00012: {
00013:     public sealed class Plan126_129CrossingFoundryIntegrationTests
00014:     {
00015:         private static string DataDirectory
00016:         {
00017:             get
00018:             {
00019:                 string start = Directory.GetCurrentDirectory();
00020:                 if (CatalogLocator.TryFindDataDirectory(start, out string found))
00021:                     return found;
00022:                 if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
00023:                     return found;
00024:                 throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
00025:             }
00026:         }
00027:
00028:         private static readonly string[] s_plan126ExpansionItemIds = new[]
00029:         {
00030:             "item_arbitration_token",
00031:             "item_charter_stamp",
00032:             "item_weighbridge_chit",
00033:             "item_smuggled_medicine",
00034:             "item_crossing_bread",
00035:             "item_lamp_oil_crossing",
00036:             "item_filtered_water_crossing",
00037:             "item_quarantine_bands",
00038:             "item_granary_receipt",
00039:             "item_smugglers_ledger",
00040:             "item_rejection_notice",
00041:             "item_crossing_map",
00042:             "item_black_market_pouch",
00043:             "item_charter_draft"
00044:         };
00045:
00046:         private static readonly string[] s_plan129ExpansionProductIds = new[]
00047:         {
00048:             "foundry_prod_bronze_datum_plate",
00049:             "foundry_prod_flywheel_rotor_shaft",
00050:             "foundry_prod_flywheel_containment_ring",
00051:             "foundry_prod_culvert_brace",
00052:             "foundry_prod_sealed_lead_pig",
00053:             "foundry_prod_ground_anchor_spikes",
00054:             "foundry_prod_turbine_blade_blank",
00055:             "foundry_prod_rail_grinding_head",
00056:             "foundry_prod_press_tooling_set"
00057:         };
00058:
00059:         private sealed class FoundryCatalogJson
00060:         {
00061:             public int schema_version { get; set; }
00062:             public List<FoundryProductJson>? products { get; set; }
00063:         }
00064:
00065:         private sealed class FoundryProductJson
00066:         {
00067:             public string? product_id { get; set; }
00068:             public string? display_name { get; set; }
00069:             public string? category { get; set; }
00070:             public string? result_item_id { get; set; }
00071:             public int result_amount { get; set; }
00072:             public List<FoundryIngredientJson>? ingredients { get; set; }
00073:             public float labor_hours { get; set; }
00074:             public float cast_hours { get; set; }
00075:             public int fuel_units { get; set; }
00076:             public int water_litres { get; set; }
00077:             public string? treaty_id { get; set; }
00078:         }
00079:
00080:         private sealed class FoundryIngredientJson
00081:         {
00082:             public string? item_id { get; set; }
00083:             public int amount { get; set; }
00084:         }
00085:
00086:         [Fact]
00087:         public void Plan126_CrossingItems_LoadsExactTwentyFiveItemsAndValidatesExpansionTier()
00088:         {
00089:             var files = new FileSystemIO();
00090:             var json = new SystemTextJsonSerializer();
00091:             var loader = new CrossingCatalogLoader(files, json);
00092:             var catalog = loader.Load(DataDirectory);
00093:
00094:             Assert.NotNull(catalog);
00095:             Assert.Equal(25, catalog.Items.Count);
00096:
00097:             var itemMap = catalog.Items.ToDictionary(i => i.id, StringComparer.Ordinal);
00098:
00099:             foreach (string expectedId in s_plan126ExpansionItemIds)
00100:             {
00101:                 Assert.True(itemMap.ContainsKey(expectedId), $"Crossing catalog missing Plan 126 item '{expectedId}'");
00102:                 var item = itemMap[expectedId];
00103:
00104:                 Assert.NotNull(item);
00105:                 Assert.StartsWith("item_", item.id, StringComparison.Ordinal);
00106:                 Assert.False(string.IsNullOrWhiteSpace(item.displayName), $"Item '{expectedId}' has empty displayName");
00107:                 Assert.False(string.IsNullOrWhiteSpace(item.description), $"Item '{expectedId}' has empty description");
00108:                 Assert.False(string.IsNullOrWhiteSpace(item.type), $"Item '{expectedId}' has empty type");
00109:                 Assert.True(item.stackMax > 0, $"Item '{expectedId}' stackMax must be positive");
00110:                 Assert.True(item.weight >= 0f, $"Item '{expectedId}' weight must be non-negative");
00111:                 Assert.True(item.tradeValue >= 0, $"Item '{expectedId}' tradeValue must be non-negative");
00112:             }
00113:         }
00114:
00115:         [Fact]
00116:         public void Plan129_FoundryProduction_LoadsExactThirtyFiveProductsAndValidatesNineExpansionProducts()
00117:         {
00118:             string path = Path.Combine(DataDirectory, "foundry_production.json");
00119:             Assert.True(File.Exists(path), "foundry_production.json must exist");
00120:
00121:             var catalog = JsonSerializer.Deserialize<FoundryCatalogJson>(
00122:                 File.ReadAllText(path), SystemTextJsonSerializer.Options);
00123:
00124:             Assert.NotNull(catalog);
00125:             Assert.Equal(1, catalog!.schema_version);
00126:             Assert.NotNull(catalog.products);
00127:             Assert.Equal(35, catalog.products!.Count);
00128:
00129:             var productMap = catalog.products.ToDictionary(p => p.product_id!, StringComparer.Ordinal);
00130:
00131:             foreach (string expectedId in s_plan129ExpansionProductIds)
00132:             {
00133:                 Assert.True(productMap.ContainsKey(expectedId), $"Foundry catalog missing Plan 129 product '{expectedId}'");
00134:                 var prod = productMap[expectedId];
00135:
00136:                 Assert.NotNull(prod);
00137:                 Assert.StartsWith("foundry_prod_", prod.product_id!, StringComparison.Ordinal);
00138:                 Assert.False(string.IsNullOrWhiteSpace(prod.display_name), $"Product '{expectedId}' has empty display_name");
00139:                 Assert.False(string.IsNullOrWhiteSpace(prod.category), $"Product '{expectedId}' has empty category");
00140:                 Assert.False(string.IsNullOrWhiteSpace(prod.result_item_id), $"Product '{expectedId}' has empty result_item_id");
00141:                 Assert.StartsWith("item_", prod.result_item_id!, StringComparison.Ordinal);
00142:                 Assert.True(prod.result_amount > 0, $"Product '{expectedId}' result_amount must be positive");
00143:                 Assert.True(prod.labor_hours > 0f, $"Product '{expectedId}' labor_hours must be positive");
00144:                 Assert.True(prod.cast_hours > 0f, $"Product '{expectedId}' cast_hours must be positive");
00145:                 Assert.True(prod.fuel_units > 0, $"Product '{expectedId}' fuel_units must be positive");
00146:
00147:                 Assert.NotNull(prod.ingredients);
00148:                 Assert.NotEmpty(prod.ingredients!);
00149:                 foreach (var ing in prod.ingredients!)
00150:                 {
00151:                     Assert.False(string.IsNullOrWhiteSpace(ing.item_id), $"Ingredient in '{expectedId}' has empty item_id");
00152:                     Assert.True(ing.amount > 0, $"Ingredient '{ing.item_id}' in '{expectedId}' must have positive amount");
00153:                 }
00154:             }
00155:         }
00156:
00157:         [Fact]
00158:         public void Plan126_129_CrossDomainCoherence_IndustrialSupplyChainAndBorderCommerceLinkages()
00159:         {
00160:             var files = new FileSystemIO();
00161:             var json = new SystemTextJsonSerializer();
00162:             var crossing = new CrossingCatalogLoader(files, json).Load(DataDirectory);
00163:
00164:             string foundryPath = Path.Combine(DataDirectory, "foundry_production.json");
00165:             var foundry = JsonSerializer.Deserialize<FoundryCatalogJson>(
00166:                 File.ReadAllText(foundryPath), SystemTextJsonSerializer.Options);
00167:
00168:             // Crossing uses culvert transit and drainage:
00169:             var smugglersCourt = crossing.GetFaction(CrossingIds.FactionSmugglersCourt);
00170:             Assert.NotNull(smugglersCourt);
00171:             Assert.Contains("culvert_transit", smugglersCourt!.offers);
00172:
00173:             // Foundry produces heavy culvert braces:
00174:             var culvertBrace = foundry!.products!.FirstOrDefault(p => p.product_id == "foundry_prod_culvert_brace");
00175:             Assert.NotNull(culvertBrace);
00176:             Assert.Equal("item_high_tensile_steel_culvert_brace", culvertBrace!.result_item_id);
00177:
00178:             // Crossing economy requires precise calibration weights:
00179:             var calWeight = crossing.Items.FirstOrDefault(i => i.id == CrossingIds.Items.CalibrationWeight);
00180:             Assert.NotNull(calWeight);
00181:             Assert.Equal(80, calWeight!.tradeValue);
00182:
00183:             // Foundry produces bronze datum plates for survey and measurement registration:
00184:             var datumPlate = foundry.products.FirstOrDefault(p => p.product_id == "foundry_prod_bronze_datum_plate");
00185:             Assert.NotNull(datumPlate);
00186:             Assert.Equal("item_datum_plate_bronze", datumPlate!.result_item_id);
00187:
00188:             // Rule 5: Ensure strict separation of concern
00189:             Assert.All(crossing.Items, i => Assert.StartsWith("item_", i.id));
00190:             Assert.All(foundry.products, p => Assert.StartsWith("foundry_prod_", p.product_id));
00191:         }
00192:
00193:         [Fact]
00194:         public void Plan126_129_DeterministicInventoryTradeAndProductionSimulation()
00195:         {
00196:             var files = new FileSystemIO();
00197:             var json = new SystemTextJsonSerializer();
00198:             var crossing = new CrossingCatalogLoader(files, json).Load(DataDirectory);
00199:
00200:             string foundryPath = Path.Combine(DataDirectory, "foundry_production.json");
00201:             var foundry = JsonSerializer.Deserialize<FoundryCatalogJson>(
00202:                 File.ReadAllText(foundryPath), SystemTextJsonSerializer.Options);
00203:
00204:             // Seeded deterministic simulation of a border trade & foundry run
00205:             var rng1 = new SeededRng(126129);
00206:             var rng2 = new SeededRng(126129);
00207:
00208:             // Select an expansion crossing item to trade
00209:             int crossingItemIdx1 = rng1.Next(0, s_plan126ExpansionItemIds.Length);
00210:             int crossingItemIdx2 = rng2.Next(0, s_plan126ExpansionItemIds.Length);
00211:             Assert.Equal(crossingItemIdx1, crossingItemIdx2);
00212:
00213:             string tradedItem = s_plan126ExpansionItemIds[crossingItemIdx1];
00214:             var crossingItem = crossing.Items.First(i => i.id == tradedItem);
00215:
00216:             // Select an expansion foundry product to cast
00217:             int foundryProdIdx1 = rng1.Next(0, s_plan129ExpansionProductIds.Length);
00218:             int foundryProdIdx2 = rng2.Next(0, s_plan129ExpansionProductIds.Length);
00219:             Assert.Equal(foundryProdIdx1, foundryProdIdx2);
00220:
00221:             string castProduct = s_plan129ExpansionProductIds[foundryProdIdx1];
00222:             var prod = foundry!.products!.First(p => p.product_id == castProduct);
00223:
00224:             // Simulate deterministic production cost calculations
00225:             float totalLabor1 = prod.labor_hours * (float)(rng1.NextDouble() * 0.2 + 0.9);
00226:             float totalLabor2 = prod.labor_hours * (float)(rng2.NextDouble() * 0.2 + 0.9);
00227:             Assert.Equal(totalLabor1, totalLabor2);
00228:
00229:             Assert.True(crossingItem.tradeValue >= 0);
00230:             Assert.True(prod.result_amount > 0);
00231:         }
00232:     }
00233: }
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

## `Ashfall.Core.Tests/CrossingQuestSystemTests.cs` — 605 lines; 22,735 bytes; SHA-256 `65e9b7c974a0ecb74ccba161056565a5efd48c922dd4f6b9899aa5b180b6b319`
Declaration index:
- 00009: public class CrossingQuestSystemTests
- 00011: private static List<CrossingQuestDef> SampleCatalog()
- 00060: private static CrossingQuestSystem FreshSystem()
- 00068: public void BindCatalog_Populates_Catalog()
- 00077: public void StartQuest_Succeeds_When_PrereqsMet()
- 00089: public void StartQuest_Fails_Before_MinDay()
- 00097: public void StartQuest_Fails_When_PrereqNotCompleted()
- 00104: public void StartQuest_Fails_When_AlreadyStarted()
- 00112: public void AdvanceStage_Progresses_Through_Stages()
- 00128: public void AdvanceStage_Completes_When_PastLastStage()
- 00146: public void OpeningQuestCompletion_Fires_Event()
- 00161: public void MakeChoice_SetsFlag()
- 00175: public void MakeChoice_ForwardsFlagToCanonicalConsequenceLedger_AndIsIdempotent()
- 00195: public void RestoreState_DoesNotAliasSavedCollections()
- 00212: public void RestoreState_ProjectsPersistedFlagsToCanonicalLedger()
- 00228: public void MakeChoice_Fails_For_InvalidChoice()
- 00236: public void GetAvailableQuests_FiltersByDay_And_Prereqs()
- 00253: public void GetAvailableQuests_UnlocksAfter_PrereqCompleted()
- 00266: public void GetAvailableQuests_Excludes_Completed()
- 00279: public void SaveLoad_RoundTrip()
- 00304: public void RestoreState_Null_IsSafe()
- 00312: public void ExpansionHubSave_Includes_CrossingQuests()
- 00346: public void TickDaily_StartsEligibleQuest_Once()
- 00359: public void TickDaily_Idempotent_SameDay()
- 00373: public void TickDaily_NoStart_BeforeMinDay()
- 00386: public void TickDaily_DoesNotStart_AlreadyStarted()
- 00399: public void TickDaily_DoesNotStart_AlreadyCompleted()
- 00415: public void TickDaily_SaveLoad_NoRestartAfterRestore()
- 00436: public void TickDaily_ManualStart_PlusTick_NoDoubleStart()
- 00455: public void StartQuest_EmitsStageNarrative_ExactlyOnce()
- 00470: public void AdvanceStage_EmitsNarrative_ExactlyOnce_PerStage()
- 00489: public void SaveLoad_DoesNotReplayStageNarrative()
- 00510: public void Stage_Narrative_Key_IsUnique_PerStage()
- 00527: public void FailQuest_MarksQuestFailed_And_FiresEvent()
- 00542: public void FailQuest_PreventsAdvance_And_TickDailyRestart()
- 00559: public void SaveLoad_PreservesFailedState()
- 00576: public void TickDaily_PostVouchGating_RequiresVouchOrOpeningCompletion()
- 00592: public void TickDaily_PostVouchGating_WithVouchAccess_AutoStarts()
```csharp
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Xunit;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Crossing;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class CrossingQuestSystemTests
00010:     {
00011:         private static List<CrossingQuestDef> SampleCatalog()
00012:         {
00013:             return new List<CrossingQuestDef>
00014:             {
00015:                 new CrossingQuestDef
00016:                 {
00017:                     id = "quest_crossing_the_vouch",
00018:                     display_name = "A Name at the Gate",
00019:                     type = "expedition",
00020:                     briefing = "Bram Ostrowski names the Crossing.",
00021:                     prereq_quest_id = "",
00022:                     min_day = 10,
00023:                     stages = new List<CrossingQuestStage>
00024:                     {
00025:                         new CrossingQuestStage { id = "s0", text = "Hear Ostrowski out." },
00026:                         new CrossingQuestStage { id = "s1", text = "Find a name." },
00027:                         new CrossingQuestStage { id = "s2", text = "Walk the approach." }
00028:                     },
00029:                     choices = new List<CrossingQuestChoice>
00030:                     {
00031:                         new CrossingQuestChoice { id = "vouch_ostrowski", text = "Ostrowski vouches.", set_flag = "flag_vouched_clean" }
00032:                     },
00033:                     knowledge_key = "lore_nc_the_vouch",
00034:                     target_location_id = "loc_crossing_viaduct_gate"
00035:                 },
00036:                 new CrossingQuestDef
00037:                 {
00038:                     id = "quest_crossing_first_weigh",
00039:                     display_name = "What the Scale Says",
00040:                     type = "expedition",
00041:                     briefing = "Osran weighs your goods.",
00042:                     prereq_quest_id = "quest_crossing_the_vouch",
00043:                     min_day = 20,
00044:                     stages = new List<CrossingQuestStage>
00045:                     {
00046:                         new CrossingQuestStage { id = "s0", text = "Present goods." },
00047:                         new CrossingQuestStage { id = "s1", text = "Accept weight." }
00048:                     },
00049:                     choices = new List<CrossingQuestChoice>
00050:                     {
00051:                         new CrossingQuestChoice { id = "accept_true", text = "Accept.", set_flag = "flag_honest_trader" },
00052:                         new CrossingQuestChoice { id = "contest", text = "Contest.", set_flag = "flag_difficult" }
00053:                     },
00054:                     knowledge_key = "lore_nc_read_again",
00055:                     target_location_id = "loc_crossing_scalehouse"
00056:                 }
00057:             };
00058:         }
00059:
00060:         private static CrossingQuestSystem FreshSystem()
00061:         {
00062:             var sys = new CrossingQuestSystem();
00063:             sys.BindCatalog(SampleCatalog());
00064:             return sys;
00065:         }
00066:
00067:         [Fact]
00068:         public void BindCatalog_Populates_Catalog()
00069:         {
00070:             var sys = FreshSystem();
00071:             Assert.Equal(2, sys.Catalog.Count);
00072:             Assert.NotNull(sys.GetDef("quest_crossing_the_vouch"));
00073:             Assert.Null(sys.GetDef("nonexistent"));
00074:         }
00075:
00076:         [Fact]
00077:         public void StartQuest_Succeeds_When_PrereqsMet()
00078:         {
00079:             var sys = FreshSystem();
00080:             bool started = false;
00081:             sys.OnQuestStarted += id => started = true;
00082:
00083:             Assert.True(sys.StartQuest("quest_crossing_the_vouch", 10));
00084:             Assert.True(started);
00085:             Assert.True(sys.IsQuestStarted("quest_crossing_the_vouch"));
00086:         }
00087:
00088:         [Fact]
00089:         public void StartQuest_Fails_Before_MinDay()
00090:         {
00091:             var sys = FreshSystem();
00092:             Assert.False(sys.StartQuest("quest_crossing_the_vouch", 5));
00093:             Assert.False(sys.IsQuestStarted("quest_crossing_the_vouch"));
00094:         }
00095:
00096:         [Fact]
00097:         public void StartQuest_Fails_When_PrereqNotCompleted()
00098:         {
00099:             var sys = FreshSystem();
00100:             Assert.False(sys.StartQuest("quest_crossing_first_weigh", 25));
00101:         }
00102:
00103:         [Fact]
00104:         public void StartQuest_Fails_When_AlreadyStarted()
00105:         {
00106:             var sys = FreshSystem();
00107:             Assert.True(sys.StartQuest("quest_crossing_the_vouch", 10));
00108:             Assert.False(sys.StartQuest("quest_crossing_the_vouch", 10));
00109:         }
00110:
00111:         [Fact]
00112:         public void AdvanceStage_Progresses_Through_Stages()
00113:         {
00114:             var sys = FreshSystem();
00115:             sys.StartQuest("quest_crossing_the_vouch", 10);
00116:
00117:             int? reportedStage = null;
00118:             sys.OnQuestStageChanged += (id, stage) => reportedStage = stage;
00119:
00120:             Assert.Equal(1, sys.AdvanceStage("quest_crossing_the_vouch"));
00121:             Assert.Equal(1, reportedStage);
00122:
00123:             Assert.Equal(2, sys.AdvanceStage("quest_crossing_the_vouch"));
00124:             Assert.Equal(2, reportedStage);
00125:         }
00126:
00127:         [Fact]
00128:         public void AdvanceStage_Completes_When_PastLastStage()
00129:         {
00130:             var sys = FreshSystem();
00131:             sys.StartQuest("quest_crossing_the_vouch", 10);
00132:
00133:             bool completed = false;
00134:             sys.OnQuestCompleted += id => completed = true;
00135:
00136:             sys.AdvanceStage("quest_crossing_the_vouch"); // 0→1
00137:             sys.AdvanceStage("quest_crossing_the_vouch"); // 1→2
00138:             var result = sys.AdvanceStage("quest_crossing_the_vouch"); // 2→complete
00139:
00140:             Assert.Equal(-1, result);
00141:             Assert.True(completed);
00142:             Assert.True(sys.IsQuestCompleted("quest_crossing_the_vouch"));
00143:         }
00144:
00145:         [Fact]
00146:         public void OpeningQuestCompletion_Fires_Event()
00147:         {
00148:             var sys = FreshSystem();
00149:             bool openingFired = false;
00150:             sys.OnOpeningQuestCompleted += () => openingFired = true;
00151:
00152:             sys.StartQuest("quest_crossing_the_vouch", 10);
00153:             sys.AdvanceStage("quest_crossing_the_vouch");
00154:             sys.AdvanceStage("quest_crossing_the_vouch");
00155:             sys.AdvanceStage("quest_crossing_the_vouch");
00156:
00157:             Assert.True(openingFired);
00158:         }
00159:
00160:         [Fact]
00161:         public void MakeChoice_SetsFlag()
00162:         {
00163:             var sys = FreshSystem();
00164:             sys.StartQuest("quest_crossing_the_vouch", 10);
00165:
00166:             string setFlag = null;
00167:             sys.OnFlagSet += (qid, flag) => setFlag = flag;
00168:
00169:             Assert.True(sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski"));
00170:             Assert.Equal("flag_vouched_clean", setFlag);
00171:             Assert.True(sys.HasFlag("flag_vouched_clean"));
00172:         }
00173:
00174:         [Fact]
00175:         public void MakeChoice_ForwardsFlagToCanonicalConsequenceLedger_AndIsIdempotent()
00176:         {
00177:             var sys = FreshSystem();
00178:             var ledger = new Ashfall.Core.Flags.InMemoryFlagLedger();
00179:             sys.BindConsequenceLedger(ledger);
00180:             sys.StartQuest("quest_crossing_the_vouch", 10);
00181:
00182:             int eventCount = 0;
00183:             sys.OnFlagSet += (_, _) => eventCount++;
00184:
00185:             Assert.True(sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski"));
00186:             Assert.True(ledger.IsSet("flag_vouched_clean"));
00187:             Assert.Equal(1, eventCount);
00188:
00189:             Assert.True(sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski"));
00190:             Assert.Equal(1, eventCount);
00191:             Assert.False(sys.MakeChoice("quest_crossing_the_vouch", "vouch_mattis_at_truss"));
00192:         }
00193:
00194:         [Fact]
00195:         public void RestoreState_DoesNotAliasSavedCollections()
00196:         {
00197:             var sys = FreshSystem();
00198:             sys.StartQuest("quest_crossing_the_vouch", 10);
00199:             sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski");
00200:             var saved = sys.CaptureState();
00201:
00202:             var restored = FreshSystem();
00203:             restored.RestoreState(saved);
00204:             saved.quests[0].chosenChoiceId = "mutated";
00205:             saved.setFlags.Clear();
00206:
00207:             Assert.Equal("vouch_ostrowski", restored.GetProgress("quest_crossing_the_vouch")!.chosenChoiceId);
00208:             Assert.True(restored.HasFlag("flag_vouched_clean"));
00209:         }
00210:
00211:         [Fact]
00212:         public void RestoreState_ProjectsPersistedFlagsToCanonicalLedger()
00213:         {
00214:             var source = FreshSystem();
00215:             source.StartQuest("quest_crossing_the_vouch", 10);
00216:             source.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski");
00217:             var saved = source.CaptureState();
00218:
00219:             var restored = FreshSystem();
00220:             var ledger = new Ashfall.Core.Flags.InMemoryFlagLedger();
00221:             restored.BindConsequenceLedger(ledger);
00222:             restored.RestoreState(saved);
00223:
00224:             Assert.True(ledger.IsSet("flag_vouched_clean"));
00225:         }
00226:
00227:         [Fact]
00228:         public void MakeChoice_Fails_For_InvalidChoice()
00229:         {
00230:             var sys = FreshSystem();
00231:             sys.StartQuest("quest_crossing_the_vouch", 10);
00232:             Assert.False(sys.MakeChoice("quest_crossing_the_vouch", "nonexistent_choice"));
00233:         }
00234:
00235:         [Fact]
00236:         public void GetAvailableQuests_FiltersByDay_And_Prereqs()
00237:         {
00238:             var sys = FreshSystem();
00239:
00240:             var day5 = sys.GetAvailableQuests(5);
00241:             Assert.Empty(day5);
00242:
00243:             var day10 = sys.GetAvailableQuests(10);
00244:             Assert.Single(day10);
00245:             Assert.Equal("quest_crossing_the_vouch", day10[0].id);
00246:
00247:             var day25 = sys.GetAvailableQuests(25);
00248:             Assert.Single(day25);
00249:             Assert.Equal("quest_crossing_the_vouch", day25[0].id);
00250:         }
00251:
00252:         [Fact]
00253:         public void GetAvailableQuests_UnlocksAfter_PrereqCompleted()
00254:         {
00255:             var sys = FreshSystem();
00256:             sys.StartQuest("quest_crossing_the_vouch", 10);
00257:             sys.AdvanceStage("quest_crossing_the_vouch");
00258:             sys.AdvanceStage("quest_crossing_the_vouch");
00259:             sys.AdvanceStage("quest_crossing_the_vouch");
00260:
00261:             var available = sys.GetAvailableQuests(25);
00262:             Assert.Contains(available, q => q.id == "quest_crossing_first_weigh");
00263:         }
00264:
00265:         [Fact]
00266:         public void GetAvailableQuests_Excludes_Completed()
00267:         {
00268:             var sys = FreshSystem();
00269:             sys.StartQuest("quest_crossing_the_vouch", 10);
00270:             sys.AdvanceStage("quest_crossing_the_vouch");
00271:             sys.AdvanceStage("quest_crossing_the_vouch");
00272:             sys.AdvanceStage("quest_crossing_the_vouch");
00273:
00274:             var available = sys.GetAvailableQuests(10);
00275:             Assert.DoesNotContain(available, q => q.id == "quest_crossing_the_vouch");
00276:         }
00277:
00278:         [Fact]
00279:         public void SaveLoad_RoundTrip()
00280:         {
00281:             var sys = FreshSystem();
00282:             sys.StartQuest("quest_crossing_the_vouch", 10);
00283:             sys.AdvanceStage("quest_crossing_the_vouch");
00284:             sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski");
00285:
00286:             var saved = sys.CaptureState();
00287:             Assert.Single(saved.quests);
00288:             Assert.Contains("flag_vouched_clean", saved.setFlags);
00289:
00290:             var sys2 = FreshSystem();
00291:             sys2.RestoreState(saved);
00292:
00293:             Assert.True(sys2.IsQuestStarted("quest_crossing_the_vouch"));
00294:             Assert.False(sys2.IsQuestCompleted("quest_crossing_the_vouch"));
00295:             Assert.True(sys2.HasFlag("flag_vouched_clean"));
00296:
00297:             var progress = sys2.GetProgress("quest_crossing_the_vouch");
00298:             Assert.NotNull(progress);
00299:             Assert.Equal(1, progress.currentStage);
00300:             Assert.Equal("vouch_ostrowski", progress.chosenChoiceId);
00301:         }
00302:
00303:         [Fact]
00304:         public void RestoreState_Null_IsSafe()
00305:         {
00306:             var sys = FreshSystem();
00307:             sys.RestoreState(null);
00308:             Assert.Empty(sys.State.quests);
00309:         }
00310:
00311:         [Fact]
00312:         public void ExpansionHubSave_Includes_CrossingQuests()
00313:         {
00314:             var sys = FreshSystem();
00315:             sys.StartQuest("quest_crossing_the_vouch", 10);
00316:             sys.MakeChoice("quest_crossing_the_vouch", "vouch_ostrowski");
00317:
00318:             var vouch = new VouchAccessSystem();
00319:             var waystation = new WaystationSystem();
00320:             var greenhouse = new GreenhouseSystem(1);
00321:             var arbitration = new CrossingArbitrationSystem();
00322:             var ledger = new LedgerDebtSystem();
00323:             var layouts = new LocationLayoutSystem(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
00324:             var memory = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
00325:             var siteEncounters = new SiteEncounterSystem();
00326:
00327:             var save = ExpansionHubSaveCodec.Capture(42,
00328:                 waystation, layouts, memory, siteEncounters, vouch, greenhouse,
00329:                 arbitration, ledger, sys);
00330:
00331:             Assert.Single(save.crossingQuests.quests);
00332:             Assert.Contains("flag_vouched_clean", save.crossingQuests.setFlags);
00333:
00334:             var sys2 = FreshSystem();
00335:             ExpansionHubSaveCodec.Restore(save,
00336:                 waystation, layouts, memory, siteEncounters, vouch, greenhouse,
00337:                 arbitration, ledger, sys2);
00338:
00339:             Assert.True(sys2.IsQuestStarted("quest_crossing_the_vouch"));
00340:             Assert.True(sys2.HasFlag("flag_vouched_clean"));
00341:         }
00342:
00343:         // ── Daily auto-start (TickDaily) ───────────────────────────────────────────
00344:
00345:         [Fact]
00346:         public void TickDaily_StartsEligibleQuest_Once()
00347:         {
00348:             var sys = FreshSystem();
00349:             int startCount = 0;
00350:             sys.OnQuestStarted += _ => startCount++;
00351:
00352:             sys.TickDaily(10);
00353:
00354:             Assert.Equal(1, startCount);
00355:             Assert.True(sys.IsQuestStarted("quest_crossing_the_vouch"));
00356:         }
00357:
00358:         [Fact]
00359:         public void TickDaily_Idempotent_SameDay()
00360:         {
00361:             var sys = FreshSystem();
00362:             int startCount = 0;
00363:             sys.OnQuestStarted += _ => startCount++;
00364:
00365:             sys.TickDaily(10);
00366:             sys.TickDaily(10); // repeated tick same day — must be a no-op
00367:             sys.TickDaily(10);
00368:
00369:             Assert.Equal(1, startCount);
00370:         }
00371:
00372:         [Fact]
00373:         public void TickDaily_NoStart_BeforeMinDay()
00374:         {
00375:             var sys = FreshSystem();
00376:             int startCount = 0;
00377:             sys.OnQuestStarted += _ => startCount++;
00378:
00379:             sys.TickDaily(5); // min_day is 10 in sample catalog
00380:
00381:             Assert.Equal(0, startCount);
00382:             Assert.False(sys.IsQuestStarted("quest_crossing_the_vouch"));
00383:         }
00384:
00385:         [Fact]
00386:         public void TickDaily_DoesNotStart_AlreadyStarted()
00387:         {
00388:             var sys = FreshSystem();
00389:             sys.StartQuest("quest_crossing_the_vouch", 10);
00390:
00391:             int startCount = 0;
00392:             sys.OnQuestStarted += _ => startCount++;
00393:
00394:             sys.TickDaily(11); // next day tick — quest already started
00395:             Assert.Equal(0, startCount);
00396:         }
00397:
00398:         [Fact]
00399:         public void TickDaily_DoesNotStart_AlreadyCompleted()
00400:         {
00401:             var sys = FreshSystem();
00402:             sys.StartQuest("quest_crossing_the_vouch", 10);
00403:             sys.AdvanceStage("quest_crossing_the_vouch");
00404:             sys.AdvanceStage("quest_crossing_the_vouch");
00405:             sys.AdvanceStage("quest_crossing_the_vouch");
00406:             Assert.True(sys.IsQuestCompleted("quest_crossing_the_vouch"));
00407:
00408:             int startCount = 0;
00409:             sys.OnQuestStarted += _ => startCount++;
00410:             sys.TickDaily(11);
00411:             Assert.Equal(0, startCount);
00412:         }
00413:
00414:         [Fact]
00415:         public void TickDaily_SaveLoad_NoRestartAfterRestore()
00416:         {
00417:             var sys = FreshSystem();
00418:             sys.TickDaily(10); // starts the vouch quest
00419:
00420:             // Save and restore
00421:             var saved = sys.CaptureState();
00422:             Assert.Equal(10, saved.lastTickedDay);
00423:
00424:             var sys2 = FreshSystem();
00425:             sys2.RestoreState(saved);
00426:
00427:             int startCount = 0;
00428:             sys2.OnQuestStarted += _ => startCount++;
00429:
00430:             sys2.TickDaily(10); // same day after restore — must not restart
00431:             Assert.Equal(0, startCount);
00432:             Assert.Single(sys2.State.quests);
00433:         }
00434:
00435:         [Fact]
00436:         public void TickDaily_ManualStart_PlusTick_NoDoubleStart()
00437:         {
00438:             var sys = FreshSystem();
00439:             bool manuallyCounted = false;
00440:             sys.OnQuestStarted += _ => manuallyCounted = true;
00441:             sys.StartQuest("quest_crossing_the_vouch", 10);
00442:             Assert.True(manuallyCounted);
00443:
00444:             int tickStartCount = 0;
00445:             sys.OnQuestStarted += _ => tickStartCount++;
00446:
00447:             // Tick same day after manual start
00448:             sys.TickDaily(10);
00449:             Assert.Equal(0, tickStartCount);
00450:         }
00451:
00452:         // ── Exactly-once stage narrative dispatch ──────────────────────────────────
00453:
00454:         [Fact]
00455:         public void StartQuest_EmitsStageNarrative_ExactlyOnce()
00456:         {
00457:             var sys = FreshSystem();
00458:             var emitted = new List<CrossingStageNarrativeEvent>();
00459:             sys.OnStageNarrativeEmitted += e => emitted.Add(e);
00460:
00461:             sys.StartQuest("quest_crossing_the_vouch", 10);
00462:
00463:             Assert.Single(emitted);
00464:             Assert.Equal("quest_crossing_the_vouch", emitted[0].questId);
00465:             Assert.Equal(0, emitted[0].stageIndex);
00466:             Assert.False(emitted[0].isCompletion);
00467:         }
00468:
00469:         [Fact]
00470:         public void AdvanceStage_EmitsNarrative_ExactlyOnce_PerStage()
00471:         {
00472:             var sys = FreshSystem();
00473:             var emitted = new List<CrossingStageNarrativeEvent>();
00474:             sys.OnStageNarrativeEmitted += e => emitted.Add(e);
00475:
00476:             sys.StartQuest("quest_crossing_the_vouch", 10); // stage 0 emitted at start
00477:             sys.AdvanceStage("quest_crossing_the_vouch");   // stage 1
00478:             sys.AdvanceStage("quest_crossing_the_vouch");   // stage 2
00479:             sys.AdvanceStage("quest_crossing_the_vouch");   // completion
00480:
00481:             Assert.Equal(4, emitted.Count);
00482:             Assert.False(emitted[0].isCompletion);
00483:             Assert.False(emitted[1].isCompletion);
00484:             Assert.False(emitted[2].isCompletion);
00485:             Assert.True(emitted[3].isCompletion);
00486:         }
00487:
00488:         [Fact]
00489:         public void SaveLoad_DoesNotReplayStageNarrative()
00490:         {
00491:             var sys = FreshSystem();
00492:             var emitted = new List<CrossingStageNarrativeEvent>();
00493:             sys.OnStageNarrativeEmitted += e => emitted.Add(e);
00494:
00495:             sys.StartQuest("quest_crossing_the_vouch", 10);
00496:             sys.AdvanceStage("quest_crossing_the_vouch");
00497:             int countBeforeSave = emitted.Count;
00498:
00499:             var saved = sys.CaptureState();
00500:
00501:             var sys2 = FreshSystem();
00502:             sys2.OnStageNarrativeEmitted += e => emitted.Add(e);
00503:             sys2.RestoreState(saved);
00504:
00505:             // RestoreState must not re-emit any previously dispatched stage events
00506:             Assert.Equal(countBeforeSave, emitted.Count);
00507:         }
00508:
00509:         [Fact]
00510:         public void Stage_Narrative_Key_IsUnique_PerStage()
00511:         {
00512:             var sys = FreshSystem();
00513:             var keys = new HashSet<string>();
00514:             sys.OnStageNarrativeEmitted += e => keys.Add($"{e.questId}:{e.stageIndex}:{e.isCompletion}");
00515:
00516:             sys.StartQuest("quest_crossing_the_vouch", 10);
00517:             sys.AdvanceStage("quest_crossing_the_vouch");
00518:             sys.AdvanceStage("quest_crossing_the_vouch");
00519:             sys.AdvanceStage("quest_crossing_the_vouch");
00520:
00521:             Assert.Equal(4, keys.Count);
00522:         }
00523:
00524:         // ── Failure handling & Post-vouch gating ───────────────────────────────────
00525:
00526:         [Fact]
00527:         public void FailQuest_MarksQuestFailed_And_FiresEvent()
00528:         {
00529:             var sys = FreshSystem();
00530:             sys.StartQuest("quest_crossing_the_vouch", 10);
00531:
00532:             string failedQuestId = null;
00533:             sys.OnQuestFailed += qId => failedQuestId = qId;
00534:
00535:             Assert.True(sys.FailQuest("quest_crossing_the_vouch"));
00536:             Assert.Equal("quest_crossing_the_vouch", failedQuestId);
00537:             Assert.True(sys.IsQuestFailed("quest_crossing_the_vouch"));
00538:             Assert.False(sys.IsQuestCompleted("quest_crossing_the_vouch"));
00539:         }
00540:
00541:         [Fact]
00542:         public void FailQuest_PreventsAdvance_And_TickDailyRestart()
00543:         {
00544:             var sys = FreshSystem();
00545:             sys.StartQuest("quest_crossing_the_vouch", 10);
00546:             sys.FailQuest("quest_crossing_the_vouch");
00547:
00548:             // Advance must fail
00549:             Assert.Equal(-1, sys.AdvanceStage("quest_crossing_the_vouch"));
00550:
00551:             // Daily tick must not restart a failed quest
00552:             int startCount = 0;
00553:             sys.OnQuestStarted += _ => startCount++;
00554:             sys.TickDaily(11);
00555:             Assert.Equal(0, startCount);
00556:         }
00557:
00558:         [Fact]
00559:         public void SaveLoad_PreservesFailedState()
00560:         {
00561:             var sys = FreshSystem();
00562:             sys.StartQuest("quest_crossing_the_vouch", 10);
00563:             sys.FailQuest("quest_crossing_the_vouch");
00564:
00565:             var saved = sys.CaptureState();
00566:             Assert.Single(saved.quests);
00567:             Assert.True(saved.quests[0].failed);
00568:
00569:             var sys2 = FreshSystem();
00570:             sys2.RestoreState(saved);
00571:             Assert.True(sys2.IsQuestFailed("quest_crossing_the_vouch"));
00572:             Assert.False(sys2.IsQuestCompleted("quest_crossing_the_vouch"));
00573:         }
00574:
00575:         [Fact]
00576:         public void TickDaily_PostVouchGating_RequiresVouchOrOpeningCompletion()
00577:         {
00578:             var sys = FreshSystem();
00579:             // Opening quest completed
00580:             sys.StartQuest("quest_crossing_the_vouch", 10);
00581:             sys.AdvanceStage("quest_crossing_the_vouch");
00582:             sys.AdvanceStage("quest_crossing_the_vouch");
00583:             sys.AdvanceStage("quest_crossing_the_vouch");
00584:             Assert.True(sys.IsQuestCompleted("quest_crossing_the_vouch"));
00585:
00586:             // Next quest (min_day 20) with opening completed should auto-start on day 20
00587:             sys.TickDaily(20, hasVouchAccess: false);
00588:             Assert.True(sys.IsQuestStarted("quest_crossing_first_weigh"));
00589:         }
00590:
00591:         [Fact]
00592:         public void TickDaily_PostVouchGating_WithVouchAccess_AutoStarts()
00593:         {
00594:             var sys = FreshSystem();
00595:             // Start and complete opening quest
00596:             sys.StartQuest("quest_crossing_the_vouch", 10);
00597:             sys.AdvanceStage("quest_crossing_the_vouch");
00598:             sys.AdvanceStage("quest_crossing_the_vouch");
00599:             sys.AdvanceStage("quest_crossing_the_vouch");
00600:
00601:             sys.TickDaily(20, hasVouchAccess: true);
00602:             Assert.True(sys.IsQuestStarted("quest_crossing_first_weigh"));
00603:         }
00604:     }
00605: }
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
