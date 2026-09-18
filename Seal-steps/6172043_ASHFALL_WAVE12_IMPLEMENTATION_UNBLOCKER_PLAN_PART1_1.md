# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 1.1
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 1.1 master plan for Generation Wave 12, derived from the supplied Wave 12 Part 1 specification and limited to the four fully specified A-series tasks.

**Scope of Part 1.1**
1. **Task A1 — C1[16] Depth Passes: Dead-Bucket Content Rails (Plan 49A/49B/49C)**
2. **Task A2 — C1[17] The Presented Game: Spatial Holdfast & Wasteland Map Bounded State (Plan 51A/51B/51C)**
3. **Task A3 — C1[18] Weight & Hygiene: Asset Budgets, LFS Retention & Reproducible Tooling (Plan 56A/56B/56C)**
4. **Task A4 — C1[19] Medicine Made Legible: Disease Vectors, Dependency Care & Palliative Continuity (Plan 60A/60B/60C)**

**Reserved for Part 1.2:** C2[15]–C2[18] from the supplied execution set. This file does not invent their detailed procedures; it finishes the four provided A-series plans and emits an explicit handoff contract for B1–B4.

**Wave 12 Part 1.1 objective:** convert the first half of the Wave 12 Part 1 queue into bounded, testable implementation packages while preserving ASHFALL's current authority graph, deterministic campaign simulation, additive persistence, Godot-only runtime, engine-free Core, and data-authoritative content workflow.

---

# 0. OPERATING CONTRACT

## 0.1 Allowed terminal states

Every task must end as exactly one of:

- **IMPLEMENTED** — missing mechanism, consumer, contract, or surface is authored through the canonical owner and all required focused gates are green.
- **DECIDED-DEFERRED** — a material product/architecture choice requires signature; the decision memo is recorded and the exact remainder is explicit.
- **RETIRED** — obsolete or duplicate file/shim/mechanism/content row is intentionally removed/unregistered with reference proof and index regeneration.
- **VERIFIED-RESOLVED** — current `HEAD` already seals the blocker; redundant work is skipped and evidence is recorded.
- **ROUTED-REPAIR** — investigation exposes a genuine production defect outside the task's bounded remit; a repair package with characterization evidence is registered.

“Mostly done”, “future”, “probably covered”, “manual follow-up”, and “TBD” are not terminal states.

## 0.2 Non-negotiable hard rules

1. **Godot is authoritative; Unity remains retired.** Do not add Unity dependencies, shims, or runtime references.
2. **Core stays engine-free.** `Assets/Ashfall.Core/` remains `netstandard2.1` and must not reference Godot, Unity, scene nodes, or engine lifecycle APIs.
3. **JSON data remains authoritative.** Authored runtime data lives under the repository's current canonical StreamingAssets/Data path.
4. **Preserve determinism.** No `System.Random`, no unseeded wall-clock randomness, no filesystem-order selection in Core.
5. **Preserve persistence.** New persisted state is additive and old-save neutral unless an existing migration contract explicitly says otherwise.
6. **One authority per concern.** No duplicate registries, shadow managers, panel-owned simulation, second medical runtime, or second content resolver.
7. **Claims before edits.** Record exact path ownership in `WORKTREE_OWNERSHIP.md` or the current claim table before modifying shared files.
8. **The 20 numbered substeps are procedural instructions, not child tasks.**
9. **Each named mini-task contains exactly four mini-substeps.**
10. **Focused testing first.** Use `scripts/run_test.sh` or the repository's current bounded test wrapper.
11. **Production changes preserve the 0-error/0-warning baseline.** If current `HEAD` already has warnings, characterize them before claiming this gate.
12. **Generated artifacts are regenerated from their source.** Never hand-patch generated registries or indexes.
13. **Presentation remains projection.** A2 views may cache render state but never own domain state.
14. **Content activation requires a real consumer.** A1 may not “improve utilization” by merely registering rows.
15. **Tooling policy must be source-grounded.** A3 may only enforce thresholds defined by Plan 56/current signed policy.
16. **Medical behavior is game-contract behavior.** A4 must not invent clinical probabilities, treatments, or pharmacology from general knowledge.
17. **Current repository names win.** If a supplied path/class moved, map it to the current canonical owner rather than recreating stale names.
18. **No hidden balance tuning.** If a wiring change exposes balance drift, route it.
19. **Every package writes an implementation log and updates census/ledger truth.**
20. **Part 1.2 consumes final Part 1.1 evidence, not this planning document's assumptions.**

## 0.3 Universal pre-edit evidence bundle

Capture before production/data edits:

- current commit;
- worktree status;
- active claim state;
- source-plan revision;
- current census row;
- canonical owner files/types;
- current focused test baseline;
- save owner/schema where persistence is involved;
- current generated registry/index status;
- current integrity/utilization metrics where data is involved.

## 0.4 Universal stop conditions

Stop and route if:

- an active claim overlaps required files;
- a historical owner/path is gone and the replacement authority is ambiguous;
- content has no defined consumer semantics;
- presentation work requires new simulation state;
- an asset-budget value is not actually authorized;
- a medical rule requires design values absent from Plan 60/live data;
- a save change cannot be additive/neutral;
- a change materially alters balance/difficulty outside this task.

---

# 1. PART 1.1 DEPENDENCY GRAPH

## 1.1 Recommended order

**A1 → A2 → A3 → A4**

A3 may proceed in parallel when claims are disjoint, but generated registries/indexes must be serialized.

## 1.2 Prior rails expected to remain sealed

Reverify rather than assume:
- DayEventVocabulary / SemanticKind;
- quest runtime;
- distress follow-up scheduler;
- content acceptance gates;
- location/travel authority;
- survivor needs/modifier stack;
- save/restore contracts;
- panel lifecycle/a11y gates;
- host/composition patterns.

A regression in a prerequisite becomes a repair package.

---

# 2. TASK A1 — C1[16] DEPTH PASSES: DEAD-BUCKET CONTENT RAILS

**Source:** `C-integration-plans/C1_planintegration[16].md` — Plan 49A/49B/49C\
**Blocker class:** UNCONSUMED CONTENT / REACHABILITY GAP\
**Canonical ownership:** current Narrative / Quest / catalog owners, plus existing consumer systems\
**Primary acceptance:** measurable reduction of genuinely orphaned rows, with every activated row reachable through an existing authority and covered by deterministic/save-safe characterization tests.

## 2.1 Objective

Drain dead authored content by reconnecting rows to **existing** gameplay rails. The goal is not an arbitrary utilization percentage. The goal is a truthful classification and execution state for every touched row:

- live/reachable;
- activate through a bounded inbound edge;
- reserved;
- retired;
- decision-blocked;
- repair-routed.

## 2.2 Authority boundaries

A1 may connect:
- narrative/quest definitions;
- weather/event conditions;
- radio stages;
- recipes/acquisition;
- companion discovery;
- faction/relationship consequences.

A1 may not create:
- a second event resolver;
- a second quest runtime;
- a second crafting registry;
- a second radio scheduler;
- a second companion ownership system.

## 2.3 Exact 20 procedural substeps

1. Run the current content-utilization selftest and capture the exact baseline unreachable/orphan counts by catalog.
2. Enumerate the live narrative-facing catalogs under the canonical Data root, including current equivalents of encounter, event, radio, quest, item, and recipe data.
3. Cross-reference every unconsumed row against `CATALOG_REGISTRY`, implementation logs, and reserve/future annotations to distinguish genuinely dead from intentionally dormant content.
4. Confirm each candidate can route through an existing authority; prohibit any new catch-all content resolver or second choice system.
5. For encounter fragments with no inbound trigger, identify the exact current semantic event/condition rail and add only the smallest declarative/runtime binding needed.
6. For item/recipe rows with no acquisition/crafting consumer, classify intent before connecting them to existing workstation, shop, salvage, drop, or quest rails.
7. Extend the current integrity validator only where Plan 49 requires a missing reference/reachability check; do not duplicate existing validation.
8. Validate every quest completion reward against canonical item/currency/reputation owners and fail closed on unknown IDs.
9. Reconcile distress-signal stage reachability with the current stage resolver and follow-up scheduler; bind only genuinely unreachable stage transitions.
10. Reconnect orphaned companion rescue/discovery content through current wildlife/companion discovery rails without creating a second companion acquisition path.
11. Audit moral-choice outcomes so every promised domain consequence resolves through canonical faction, relationship, needs/morale, quest, or inventory APIs.
12. Review only touched text for ASHFALL tone/continuity; do not rewrite sealed content for style.
13. Validate modified JSON against the **current** schema/version policy; do not blindly force `schema_version: 1` if live policy differs.
14. Add focused Plan 49 characterization tests proving each promoted row has at least one reachable trigger/consumer path.
15. Prove save/restore continuity for newly reachable stateful quest/event/radio stages using existing save sections only.
16. Add deterministic replay coverage proving the same campaign seed/state yields the same eligible set and selection for newly activated branches.
17. Run the current data-integrity selftest and record zero new validation failures.
18. Run content-utilization again and record the before/after counts per touched catalog.
19. Regenerate the catalog/asset/docs indexes through canonical generators and run their parity/check mode.
20. Hand off A1 with a row-level activation matrix, utilization diff, save/replay evidence, focused test commands, generated-artifact checks, and zero-warning build.

## 2.4 Mini-task A1.1 — Encounter Dead-Bucket Reconnection

### A1.1.a
Query encounter rows with zero inbound edge from current campaign/day-event/weather/medical/world trigger registries.

### A1.1.b
Map weather-related orphan encounters to the current weather transition/event contract rather than direct weather-system internals.

### A1.1.c
Map medical-hazard encounters to the current disease/exposure dispatch only where Plan 49/live content explicitly expects that causal edge.

### A1.1.d
Add characterization tests that construct the real trigger state and prove the previously dormant encounter becomes reachable/selectable.

## 2.5 Mini-task A1.2 — Orphan Item & Recipe Activation

### A1.2.a
Enumerate item IDs with zero references across recipes, shops, salvage, drops, quest rewards, starting kits, and scripted acquisition.

### A1.2.b
Classify each as `RESERVED`, `RETIRED`, `SHOULD-BE-OBTAINABLE`, or `DECISION-BLOCKED` before editing data.

### A1.2.c
For `SHOULD-BE-OBTAINABLE`, attach to existing acquisition/production rails while preserving current tier, research, tool, scarcity, and workstation semantics.

### A1.2.d
Run integrity plus focused acquisition/crafting tests proving no dangling IDs and no free/invalid acquisition path.

## 2.6 Mini-task A1.3 — Radio Broadcast Stage Binding

### A1.3.a
Trace live broadcast/distress definitions whose graph terminates before a plan-defined completion/follow-up edge.

### A1.3.b
Wire terminal discoveries to the current journal/event owner only if the radio contract specifies journal projection.

### A1.3.c
Preserve the validated follow-up grammar, legal campaign-day delay, and exactly-once scheduling semantics.

### A1.3.d
Run focused radio/distress tests proving stage progression, journal parity, restore parity, and no duplicate follow-up firing.

## 2.7 Mini-task A1.4 — Content Integrity & Metrics Gate

### A1.4.a
Run the utilization scanner and record reachable/unreachable counts for every touched catalog.

### A1.4.b
Run cycle/reachability checks for quest prerequisites, event flags, and follow-up chains.

### A1.4.c
Verify modified JSON encoding/formatting against repository policy, including actual UTF-8/BOM convention.

### A1.4.d
Regenerate asset/content registry outputs from source and run the generator check/parity gate.

## 2.8 Reachability classification

| Classification | Meaning | Allowed action |
|---|---|---|
| REACHABLE-LIVE | scanner/index stale or dynamic reference | fix metadata/index only |
| ACTIVATE | intended row lacks one bounded inbound edge | wire through current owner |
| RESERVED | explicitly future/expansion | document, leave unreachable |
| RETIRED | obsolete content | unregister/remove per policy |
| DECISION-BLOCKED | activation semantics affect design/balance | decision memo |
| ROUTED-REPAIR | consumer exists but is broken | repair package |

## 2.9 Row activation worksheet

| Row | Catalog | Existing refs | Intended consumer | Classification | Change | Save impact | Test |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

## 2.10 Valid activation edge

Must be:
- explicit;
- deterministic;
- semantically justified;
- routed through a current owner;
- reachable in a characterization test.

Invalid:
- global “always include” pool;
- random catch-all injection;
- UI-triggered domain event;
- new generic resolver.

## 2.11 Reward integrity

For every touched quest/event:
- reward reference resolves;
- amount/range valid;
- owner API used;
- terminal payout exactly once;
- save/restore does not duplicate reward.

## 2.12 Moral consequence integrity

If content promises a standing/relationship/morale/item consequence, it must call the current owner. Text-only “consequence” is not enough when the row contract promises gameplay effect.

## 2.13 Radio-stage acceptance

Prove:
- source signal selectable;
- stage trigger producible;
- delay valid;
- terminal/follow-up stage reachable;
- journal/audio references valid;
- restore does not duplicate.

## 2.14 Companion rescue acceptance

Use existing:
- discovery event;
- companion owner;
- capacity/ownership rules;
- contamination/disease payload if current mechanics require.

No rescue-only shadow inventory.

## 2.15 Save policy

Expected: **zero new save roots**.

If a dormant row becomes live and exposes a missing persistence defect, route a repair rather than hiding schema work in content activation.

## 2.16 Determinism policy

For every newly activated selection family:
- stable candidate ordering;
- same seed/state → same result;
- UI/journal reads consume no RNG;
- JSON object/dictionary iteration cannot change selection.

## 2.17 Metrics

Report:
- baseline orphan count;
- final orphan count;
- activated;
- reserved;
- retired;
- decision-blocked;
- repair-routed.

A percentage is secondary.

## 2.18 Rollback conditions

Stop activation if:
- consumer semantics unknown;
- new path changes economy/scarcity/balance materially;
- owner absent;
- reference targets retired system;
- current save cannot preserve state;
- selection cannot be deterministic.

## 2.19 A1 acceptance

A1 reaches IMPLEMENTED when all touched rows have explicit classification, every activated row has a live consumer, focused tests are green, save/replay is stable, integrity is green, utilization is truthfully improved, generated indexes match, and remaining unreachable content is explicitly intentional or separately blocked.

---

# 3. TASK A2 — C1[17] THE PRESENTED GAME: SPATIAL HOLDFAST & WASTELAND MAP

**Source:** `C-integration-plans/C1_planintegration[17].md` — Plan 51A/51B/51C\
**Blocker class:** PRESENTATION GAP / READ-MODEL VIEW BOUNDING\
**Canonical ownership:** current Godot UI + Host layer\
**Primary acceptance:** event-driven spatial shelter/map projections with no independent simulation or persistence.

## 3.1 Objective

Deliver bounded Holdfast and Wasteland map views as pure projections of canonical state. The views:
- bind through current host/router seams;
- use stable IDs;
- update from domain events/read models;
- support keyboard/gamepad focus;
- unbind cleanly;
- reconstruct after save/load;
- introduce no save section.

## 3.2 Exact 20 procedural substeps

1. Audit `src/Main.cs`, UI router, existing shelter/map panels, and host sessions to identify current route/binding conventions.
2. Prove the invariant that spatial views read canonical state and dispatch commands but never persist or simulate domain state.
3. Claim current target view/presenter/host/test/doc paths before edits.
4. Implement/adapt the Holdfast spatial view using current Godot `Control`/`Node2D` patterns and canonical shelter room/grid descriptors.
5. Bind room visual states to the current shelter/environment owner, mapping supplied state names to live domain vocabulary.
6. Project occupancy/worker indicators from the current duty-roster assignment read model without per-frame polling.
7. Route room activation through the central UI router/host command seam; no panel-side needs/resource math.
8. Implement/adapt the Wasteland map from canonical location/place registry state.
9. Project route connections from current travel/expedition graph data, including live distance/hazard/terrain semantics.
10. Project active expedition markers from authoritative expedition progress without separately persisting marker state.
11. Subscribe to current expedition/world/location events and remove any polling used solely for domain synchronization.
12. Make every interactive room/map node keyboard/gamepad focusable in stable traversal order.
13. Add design-system-compliant focus indication and non-color-only state cues.
14. Unbind every domain/host subscription on view teardown according to the current lifecycle pattern.
15. Add focused headless/engine integration tests for instantiate/bind/open/close/reopen and command routing.
16. Extend/execute lifecycle stress for 100 open/close cycles with no retained or duplicated subscriptions.
17. Verify no presentation-specific save section or authoritative DTO is introduced.
18. Add fresh-view save/load reconstruction tests for mid-expedition and changed shelter-state cases.
19. Run current panel lifecycle, UI accessibility/focus, route-parity, and relevant snapshot gates.
20. Update UI architecture documentation and hand off with event-subscription map, reconstruction proof, accessibility evidence, and zero-warning build.

## 3.3 Mini-task A2.1 — Holdfast Room Node Binding

### A2.1.a
Build the room-cell projection from canonical room IDs/layout definitions, not presentation-only identity.

### A2.1.b
Render hazard/environment overlays from current shelter/environment read state using live hazard vocabulary.

### A2.1.c
Route room activation to the current room-detail route with stable room ID.

### A2.1.d
Test that a domain room-state change event updates the presenter state without polling.

## 3.4 Mini-task A2.2 — Wasteland Map Route Projection

### A2.2.a
Read canonical location coordinates or presentation-layout metadata from the current place/map source.

### A2.2.b
Render route vectors from the graph/travel owner and translate hazard/travel tier through current visual vocabulary.

### A2.2.c
Route node activation to expedition preparation/detail with the stable target location preselected.

### A2.2.d
Test discovery/knowledge gating so unknown locations/routes remain hidden or masked according to current world-knowledge authority.

## 3.5 Mini-task A2.3 — Event-Driven Refresh Discipline

### A2.3.a
Remove/avoid `_Process` or timer polling whose only purpose is discovering domain changes.

### A2.3.b
Bind the current power-grid change event to the affected room projections.

### A2.3.c
Bind current water/flood/contamination events to affected room visual projections.

### A2.3.d
Use lifecycle/profiling evidence to prove no idle **domain polling**; do not claim literal zero CPU usage.

## 3.6 Mini-task A2.4 — Accessibility & Navigation Integration

### A2.4.a
Construct explicit or computed stable focus-neighbor topology for all interactive rooms and map nodes.

### A2.4.b
Verify keyboard/gamepad traversal reaches every visible enabled target without mouse input.

### A2.4.c
Pair color-dependent state with text, icon, pattern, or other non-color indicator.

### A2.4.d
Run current accessibility/focus tests and any repository-supported contrast checks.

## 3.7 Presentation authority table

| Concern | Domain owner | Presenter state | Persisted by presenter? | Command route |
|---|---|---|---|---|
| room status | shelter/environment | render projection | no | shelter host |
| occupancy | duty roster | render projection | no | roster/room route |
| place discovery | world knowledge | render projection | no | world/expedition |
| travel graph | travel owner | line geometry | no | expedition |
| expedition progress | expedition state | token projection | no | expedition |

## 3.8 Reconstruction invariant

A fresh view created after restore must render the same:
- room states;
- occupancy;
- known locations;
- legal/visible routes;
- expedition progress,
given the same authoritative restored state.

## 3.9 Coordinate rule

Presentation layout coordinates may be presentation-owned when no gameplay coordinates exist. They must not become travel-distance authority.

## 3.10 Event subscription map

| Event | Source | View subscriber | Update scope | Unbind |
|---|---|---|---|---|
|  |  |  |  |  |

Every bind needs a matching unbind.

## 3.11 Focus lifecycle

Open:
- bind once;
- render current state;
- choose deterministic valid focus.

Close:
- unbind all.

Reopen:
- no duplicate callbacks;
- hidden/disabled targets excluded.

## 3.12 Route activation

A map-node activation selects/navigates according to the current UI contract. It must not silently launch an expedition unless that is already the canonical behavior.

## 3.13 Hidden knowledge

Unknown place/route state is read from the knowledge owner. No separate UI discovery flags.

## 3.14 Snapshot discipline

Only update snapshots for intentional changed surfaces. Inspect diff; do not bulk accept unrelated snapshot churn.

## 3.15 Headless test boundary

Keep Godot-dependent tests in the engine/UI test layer. Core xUnit tests may cover read-model mapping but must not import Godot.

## 3.16 Performance guard

No domain-sync polling. Event callbacks should update only the affected nodes where practical.

## 3.17 Lifecycle oracle

After 100 cycles:
- subscriber count returns to baseline;
- each event invokes current instance once;
- no disposed instance receives callbacks;
- host/view references are releasable.

## 3.18 Part 1.2 input handoff

A2 must publish:
- focus topology conventions;
- gamepad-visible controls;
- current input action assumptions;
so C2[15] can implement controller parity/rebinding without rediscovering the spatial surfaces.

## 3.19 A2 acceptance

IMPLEMENTED requires pure projection, event-driven refresh, lifecycle cleanliness, save reconstruction, route correctness, focus/a11y compliance, reviewed snapshots, and zero-warning build.


# 4. TASK A3 — C1[18] WEIGHT & HYGIENE: ASSET BUDGETS, LFS RETENTION & REPRODUCIBLE TOOLING

**Source:** `C-integration-plans/C1_planintegration[18].md` — Plan 56A/56B/56C\
**Blocker class:** REPOSITORY BLOAT / TOOLING HYGIENE\
**Canonical ownership:** `scripts/ci/`, `.gitattributes`, `.gitignore`, current tooling/docs governance\
**Primary acceptance:** reproducible, policy-backed hygiene enforcement with no destructive history rewriting and no arbitrary unsanctioned thresholds.

## 4.1 Objective

Turn repository hygiene from one-off manual inspection into repeatable verification around:

- current checkout mass;
- Git/LFS policy;
- binary tracking;
- Godot UID sidecars;
- generated-registry purity;
- asset budgets;
- Core engine-free isolation;
- shell/tooling health;
- Tier-1 `verify-fast` composition;
- clean-checkout reproducibility.

The task must distinguish **current repository defects** from **historical Git bloat** and **local generated caches**.

## 4.2 Exact 20 procedural substeps

1. Measure current repository/working-tree footprint by top-level category, separating `.git`, LFS objects, `.godot`, assets, docs, source, generated artifacts, test outputs, and caches.
2. Audit `.gitattributes` against the **actual** binary extensions/paths and current Plan 56 LFS policy rather than assuming every PNG/WAV/TTF must use LFS.
3. Search tracked files for stray build outputs, temporary logs, crash files, IDE state, generated caches, and retired Unity-era remnants.
4. Run the current UID sidecar gate and record missing and dangling sidecars separately before attempting repair.
5. Implement or extend asset-budget validation using only thresholds defined by Plan 56/current signed policy; if supplied rough thresholds conflict with live policy, live policy wins.
6. Validate current Godot import/source relationships using repository-supported checks; do not treat local `.godot/imported` cache as authoritative source.
7. Audit the generated asset registry for nonexistent source paths, stale entries, duplicate IDs, or generated-order drift.
8. Run the canonical asset-registry generator in check/purity mode and record any source-vs-generated differences.
9. Validate `docs/INDEX.md` against actual documents/frontmatter via the canonical docs-index tool; measure current doc count instead of assuming “2,400+”.
10. Run the current secret/credential scan, or add the narrow Plan 56 gate if absent, ensuring candidate secret values are redacted from logs.
11. Audit `.gitignore` against current transient Godot/IDE/test/crash/generated outputs while preserving intentionally tracked project/tool configuration.
12. Verify `scripts/run_test.sh` timeout, filtering, quoting, exit propagation, and flag passthrough against its **current** contract; do not rewrite solely to match an assumed 180-second value.
13. Add or strengthen a static/compile barrier proving `Assets/Ashfall.Core/` references no Godot, Unity, or engine assemblies/types.
14. Audit script executable permissions and shell syntax according to repository policy; avoid unrelated formatting churn.
15. Verify `scripts/ci/verify-fast.sh` composes the current Tier-1 gates in the intended order without duplicating expensive checks.
16. Measure clean-checkout/clone weight using a documented reproducible method, stating whether LFS/history are included.
17. Reconcile project-controlled pre-commit hook scripts with the supported installation process; do not treat local `.git/hooks/` as version-controlled source.
18. Benchmark `CatalogIntegrityValidator` using the approved harness and compare against a budget only if Plan 56/current policy defines one.
19. Run `verify-fast.sh` after tooling changes and confirm every newly added gate passes without introducing build/test warnings.
20. Publish/update `docs/tools/REPOSITORY_HYGIENE_GUIDE.md` with policy sources, gate commands, remediation, clone-weight methodology, and handoff evidence.

## 4.3 Mini-task A3.1 — Git LFS & Binary Tracking Audit

### A3.1.a
Compare actual tracked binary extensions and directories with `.gitattributes` and the current repository LFS policy.

### A3.1.b
Inspect Git object history/current index for oversized binary blobs and classify historical legacy bloat separately from current tracking violations.

### A3.1.c
Run `git lfs status` and equivalent attribute checks to prove active assets follow declared policy.

### A3.1.d
Add a deterministic CI gate that fails only when a newly/currently tracked binary violates the declared LFS rule.

## 4.4 Mini-task A3.2 — UID Sidecar Integrity Sweep

### A3.2.a
Run the repository's current UID sidecar gate over the source roots it actually governs.

### A3.2.b
Classify missing sidecars, dangling sidecars, and intentionally excluded files separately.

### A3.2.c
Repair missing/dangling UIDs using Godot/current repository tooling; do **not** fabricate a UID algorithm in Python or shell.

### A3.2.d
Rerun the UID gate and record zero unresolved defects inside the governed scope.

## 4.5 Mini-task A3.3 — Asset Size & Budget Gate

### A3.3.a
Extract the authoritative category thresholds and exception policy from Plan 56/current signed asset policy.

### A3.3.b
Implement the scanner under the canonical CI tooling path with stable relative-path ordering and clear category reporting.

### A3.3.c
Report existing oversized assets with optimization recommendations; do not automatically downsample/re-encode production art or audio.

### A3.3.d
Integrate the budget check into the verification tier specified by Plan 56 based on runtime cost and blocking severity.

## 4.6 Mini-task A3.4 — Core Engine-Free Barrier Verification

### A3.4.a
Inspect Core `.csproj` references plus source imports for forbidden engine dependencies.

### A3.4.b
Assert no `Godot`, `UnityEngine`, engine package references, engine attributes, or engine base types are introduced into Core.

### A3.4.c
Use lifecycle-name scanning (`_Ready`, `_Process`, `Node2D`, etc.) only as a supplemental heuristic, not as the sole proof.

### A3.4.d
Integrate the cheap barrier into `verify-fast.sh` if Plan 56 requires Tier-1 enforcement.

## 4.7 Repository-weight model

Report separately:

| Category | Meaning | Include in clean checkout metric? |
|---|---|---|
| source/assets | tracked working tree | yes |
| docs | tracked docs | yes |
| `.git` | full history/object DB | separate |
| LFS objects | downloaded binary store | separate |
| `.godot` | local import/cache | no |
| generated artifacts | policy-dependent | separate |
| test/log outputs | transient | no |

Do not compare two measurements unless methodology is identical.

## 4.8 LFS policy guard

Do not infer “binary = LFS” automatically.

The gate should be based on:
- path pattern;
- extension;
- size threshold if policy says;
- generated/source distinction.

## 4.9 Historical Git bloat

If large non-LFS blobs exist only in old history:
- document;
- quantify;
- do not rewrite history in A3.

History rewrite requires a separate signed governance package.

## 4.10 Asset-budget contract

Every enforced category needs:
- category matcher;
- threshold;
- policy source;
- exception model;
- failure severity;
- remediation guidance.

No policy source means advisory measurement, not hard failure.

## 4.11 Secret scanning

Requirements:
- redact values;
- deterministic nonzero exit on confirmed violation;
- current allowlist/fixture handling;
- no secrets copied into closeout artifacts.

## 4.12 `.gitignore` safety

Before patching:
- capture current diff;
- preserve unrelated user changes;
- classify each new ignore pattern.

Do not broadly ignore source directories or data formats.

## 4.13 `scripts/run_test.sh` contract

Verify:
- timeout;
- filters;
- quotes;
- exit code;
- additional args;
- cleanup.

Only change proven defects or explicit Plan 56 requirements.

## 4.14 Core barrier implementation

Strongest proof:
1. Core project references remain engine-free.
2. Core builds independently.
3. source scan catches accidental imports/types.

Use all appropriate layers.

## 4.15 Script-permission policy

Not every script necessarily needs `+x`.

Classify:
- direct executable;
- interpreter-invoked library/helper.

Apply current policy.

## 4.16 `verify-fast.sh` gate table

| Gate | Purpose | Cost | Tier-1? | Existing? | Action |
|---|---|---:|---|---|---|
| build | compile |  | yes |  |  |
| Core barrier | architecture | low | plan-defined |  |  |
| UID | Godot integrity | low |  |  |  |
| asset budget | asset hygiene |  |  |  |  |
| registry parity | generated truth | low |  |  |  |
| docs index | docs truth | low |  |  |  |

Avoid duplicate expensive invocation.

## 4.17 Integrity performance

If the plan truly sets a 2.0-second budget:
- benchmark with multiple controlled runs;
- report median/worst per policy;
- do not fail on a single noisy local sample without CI methodology.

If no signed budget exists:
- record timing and route threshold decision.

## 4.18 Reproducible tooling requirements

New/changed scripts:
- repo-relative paths;
- stable ordering;
- nonzero on violation;
- no machine-specific absolute paths in output;
- no timestamps in generated files unless explicitly required.

## 4.19 A3 terminal acceptance

A3 reaches IMPLEMENTED when:
- all current hygiene defects in scope are classified;
- policy-backed gates are installed;
- Core barrier green;
- LFS/UID/registry/docs checks green;
- verify-fast green;
- clean-checkout/weight methodology documented;
- no history rewrite or destructive asset optimization occurred.

---

# 5. TASK A4 — C1[19] MEDICINE MADE LEGIBLE: DISEASE VECTORS, DEPENDENCY CARE & PALLIATIVE CONTINUITY

**Source:** `C-integration-plans/C1_planintegration[19].md` — Plan 60A/60B/60C\
**Blocker class:** MEDICAL CONTINUITY / CAUSAL TRANSMISSION GAP\
**Canonical ownership:** current Medical, Dose, Needs, Inventory, Weather, Roster and related owners\
**Primary acceptance:** causal deterministic medical progression, save-stable dependency/palliative flows, typed diagnostic projection, and no parallel health runtime.

## 5.1 Objective

Close Plan 60's verified medical gaps while respecting current data and owners.

A4 covers:
- exposure/vector causality;
- incubation/progression;
- treatment continuity;
- dependency/withdrawal;
- palliative comfort;
- quarantine duty restrictions;
- diagnostic legibility.

It does **not** authorize inventing real-world disease probabilities, medication regimens, or clinical advice.

## 5.2 Exact 20 procedural substeps

1. Re-measure the live disease catalog and vector taxonomy; record current counts rather than hardcoding “15 diseases / 4 vectors”.
2. Trace the current medical pipeline from exposure/contact through incubation, active illness, treatment, recovery/convalescence, and terminal outcomes.
3. Confirm the current medical authority and adjacent Dose, Inventory, Needs, Weather, Roster, Memorial/Morale owners; prohibit a second disease runtime.
4. Wire contaminated-water consumption through the current typed exposure/infection API only where live data/Plan 60 defines a waterborne exposure path.
5. Wire current airborne/weather hazard exposure through a typed weather→medical exposure seam, reading protection from the canonical equipment/gear owner.
6. Add or complete day/tick-keyed deterministic incubation/progression under the current campaign clock/medical state.
7. Reconcile chemical dependency against authoritative dose history, using only substances and threshold semantics defined by Plan 60/live data.
8. Route withdrawal effects through the existing shared modifier stack using named stable contributors and neutral behavior when dependency is absent.
9. Add/complete palliative-care actions within the medical authority for plan-defined terminal/no-curative-path states.
10. Verify all treatment actions consume canonical medicine/item IDs actually present in live data; example names in the draft are not assumed current.
11. Expose a typed medical read model with only information the current diagnosis/medical contract permits: symptoms, suspected/confirmed vector/pathogen, stage, treatment information.
12. Bind the medical ward presentation to that read model; no diagnosis, infection rolls, progression, or treatment math in the panel.
13. Enforce quarantine/duty restrictions through roster/domain preflight and return typed refusal reasons; panel disabling alone is insufficient.
14. Emit/reuse canonical semantic events for medical transitions such as infection, recovery, dependency formation, adding IDs only when Plan 60 requires missing vocabulary.
15. Add parameterized Plan 60 continuity tests over the live disease catalog plus targeted vector/treatment edge cases.
16. Prove fixed seed + fixed exposure state yields identical infection/progression outcomes.
17. Prove save/restore continuity mid-incubation, mid-treatment, during dependency/withdrawal, and in palliative/quarantine states where persisted.
18. Run data-integrity validation for disease/treatment/medicine references and record zero new warnings.
19. Run only the focused medical/health/triage/dose/needs/roster suites touched by A4, followed by the required package close gates.
20. Update the survivor-state authority matrix and hand off with causal-flow diagram, state/save mapping, RNG stream, typed UI read model, focused tests, and zero-warning build.

## 5.3 Mini-task A4.1 — Causal Vector Transmission Hookup

### A4.1.a
Route contaminated-water consumption through the current exposure API using live waterborne-risk data and the registered seeded medical RNG stream.

### A4.1.b
Route toxic fog/dust/airborne exposure through current weather→exposure seams, reading protective gear condition/effectiveness from the equipment owner.

### A4.1.c
Route bloodborne exposure from current injury/procedure events only when Plan 60/live data defines such transmission.

### A4.1.d
Add tests showing protective measures reduce/prevent transmission **according to the actual game contract**; do not assume absolute prevention unless the data specifies 100%.

## 5.4 Mini-task A4.2 — Chemical Dependency & Withdrawal Loop

### A4.2.a
Audit current dose-history state and identify which medicines/substances Plan 60 marks as dependency-forming.

### A4.2.b
Implement/complete dependency threshold evaluation from authoritative dose history without creating a second medication ledger.

### A4.2.c
Route withdrawal effects into the shared modifier stack with stable contribution IDs for plan-defined fatigue/nausea/agitation or current equivalents.

### A4.2.d
Add deterministic taper/cold-stop journey tests only for taper/withdrawal rules actually defined by Plan 60.

## 5.5 Mini-task A4.3 — Palliative Care & Terminal Comfort

### A4.3.a
Derive palliative eligibility from current treatment reachability/terminal state rather than a presentation-only flag.

### A4.3.b
Route comfort actions through canonical inventory/medicine plus pain/needs/morale owners.

### A4.3.c
Connect palliative context to memorial/grief behavior only where the current Plan 60/memorial contract explicitly defines that consequence.

### A4.3.d
Verify consumed resources appear in current inventory/economy/daily expenditure accounting rather than a palliative-only ledger.

## 5.6 Mini-task A4.4 — Medical Ward Diagnostic Projection

### A4.4.a
Expose symptoms, suspected vector, confirmed classification, and treatment information through a typed read model with uncertainty represented explicitly.

### A4.4.b
Render incubation/recovery timing using authoritative campaign-day/tick fields; do not invent “hours” if the simulation is not hourly.

### A4.4.c
Route quarantine controls through the medical/roster command seam and revalidate domain state at commit.

### A4.4.d
Run panel lifecycle, focus/a11y, and intended snapshot gates for the changed medical surface.

## 5.7 Medical authority map

| Concern | Canonical owner | A4 action | Persistence |
|---|---|---|---|
| disease definitions | data catalog | validate/read | data |
| exposure | medical/exposure owner | typed input | medical state |
| infection stage | medical | advance | medical save |
| dose history | dose ledger | query/write via dose API | dose save |
| dependency | current medical/dose owner | derive/transition | current owner |
| withdrawal | needs modifier stack | derived contribution | source state, not duplicate |
| medicine inventory | inventory | consume via transaction | inventory |
| quarantine duty | roster + medical | preflight/refusal | owner state |
| palliative state | medical | command/state if plan | medical |
| grief/morale | memorial/morale | route only | owner |

## 5.8 Causal disease flow

`exposure source → typed exposure → protection evaluation → seeded infection resolution → incubation → active illness → treatment/recovery/death → semantic event/read model`.

Weather/UI never directly set infection.

## 5.9 Vector taxonomy

Live catalog wins.

If Plan 60's historical vectors differ from current data:
- document;
- implement current authoritative taxonomy;
- update plan log/census truth.

## 5.10 Exposure RNG

Use current registered RNG architecture.

Requirements:
- named/stable stream;
- no `System.Random`;
- stable ordering;
- restore/replay consistent;
- UI reads consume no RNG.

## 5.11 Protective gear

Medical code may query protection but does not own equipment durability/effect math.

Avoid duplicating mask/filter/gear calculations.

## 5.12 Incubation

Persist only authoritative state needed to reconstruct:
- disease ID;
- infection/exposure timing;
- stage;
- treatment state,
according to current design.

Display countdown is derived.

## 5.13 Dependency

Use Plan 60/live game data only.

This is game mechanics, not real-world pharmacology.

If thresholds are missing:
- DECIDED-DEFERRED or data package.

## 5.14 Withdrawal modifiers

One stable contribution per survivor/dependency effect family.

Repeated daily evaluation must not stack duplicate modifiers.

## 5.15 Palliative care

Separate:
- cure;
- disease-modifying treatment;
- symptom relief/comfort.

Palliative action cannot secretly cure unless Plan 60 explicitly says.

## 5.16 Quarantine

Domain preflight decides whether contagious/quarantined survivor may perform shared duty.

UI reflects typed refusal.

## 5.17 Diagnostic uncertainty

If diagnosis is not confirmed:
- panel must not reveal hidden pathogen certainty.

Use current suspected/probable/confirmed vocabulary.

## 5.18 Semantic events

Events should be transition events:
- infection begins;
- recovery occurs;
- dependency forms,
not repeated “state exists” spam each tick.

## 5.19 Catalog-wide tests

Use:
- total reference/schema validation across all current diseases;
- parameterized vector/treatment tests;
- representative progression journeys;
- dedicated outlier tests.

Avoid brittle duplicated cases where one parameterized test proves the rule.

## 5.20 Save journey matrix

| Case | Save point | Restore expectation |
|---|---|---|
| incubation | mid-stage | identical stage/timing suffix |
| treatment | after dose/action | no duplicate consumption/effect |
| dependency | formed | same dependency state |
| withdrawal | active | contribution restored once |
| palliative | active if persisted | same comfort/status |
| quarantine | restricted | same roster preflight result |

## 5.21 UI no-math rule

Panel never:
- rolls infection;
- advances disease;
- calculates treatment success;
- edits roster directly;
- owns dose state.

## 5.22 Balance boundary

If implementation reveals:
- infection too lethal;
- dependency threshold too aggressive;
- palliative resources too cheap,
record a balance finding. Do not tune inline.

## 5.23 A4 terminal acceptance

A4 reaches IMPLEMENTED only when causal exposure rails, deterministic progression, dependency/withdrawal, palliative behavior, diagnostic read model, quarantine command routing, save/replay, data integrity, and focused suites are green without a new medical authority.

---

# 6. CROSS-TASK SAVE & DETERMINISM CONTRACT

| Task | Expected save change | RNG | Restore proof |
|---|---|---|---|
| A1 | normally none | existing consumer streams only | newly active stages restore |
| A2 | none | none | fresh view reconstructs |
| A3 | none | none | n/a |
| A4 | additive within current owners only | medical/exposure stream where defined | mid-flow cases match |

## 6.1 Save rules

- no presentation save root;
- no Plan49-specific content save root;
- no tooling-related save changes;
- no second medical save store.

## 6.2 Determinism rules

Any new ordering:
- stable IDs;
- stable sorted collections;
- no wall clock;
- no filesystem enumeration;
- no UI-driven RNG.

---

# 7. CLAIM / MERGE DISCIPLINE

## 7.1 A1 claims

Claim:
- touched catalogs;
- validator files;
- narrative/quest/radio/companion tests;
- generator inputs;
- implementation log.

## 7.2 A2 claims

Claim:
- presenter/view files;
- host/router seams;
- UI tests;
- UI architecture docs.

## 7.3 A3 claims

Claim:
- CI scripts;
- `.gitattributes`/`.gitignore` only if needed;
- tooling docs;
- generator scripts.

## 7.4 A4 claims

Claim:
- medical/dose/needs/roster integration files;
- touched disease/treatment data;
- medical UI/read model;
- focused tests;
- authority matrix.

## 7.5 Generated-output coordination

When two tasks affect:
- asset registry;
- docs index;
- catalog registry,
serialize the source changes and regenerate from final merged inputs.

---

# 8. FOCUSED TEST POLICY

## 8.1 A1
1. row-specific characterization;
2. affected consumer suite;
3. data integrity;
4. utilization;
5. save/replay.

## 8.2 A2
1. read-model/presenter;
2. host command;
3. lifecycle;
4. accessibility/focus;
5. save reconstruction;
6. snapshots.

## 8.3 A3
1. each new/changed script test;
2. Core barrier;
3. registry/docs checks;
4. verify-fast.

## 8.4 A4
1. exposure;
2. progression;
3. dose/dependency;
4. palliative;
5. quarantine/roster;
6. save/replay;
7. focused medical suite.

Broad suites are only justified when repository close policy requires or a focused failure indicates cross-system risk.

---

# 9. ROLLBACK / ROUTING MATRIX

| Task | Finding | Required action |
|---|---|---|
| A1 | no defined consumer | reserve/decision-block |
| A1 | activation changes balance | decision memo |
| A1 | stage state not persisted | repair package |
| A2 | view needs authoritative state | stop; add/read existing read model |
| A2 | lifecycle leak | repair before merge |
| A2 | route ownership ambiguous | claim/architecture review |
| A3 | threshold not sourced | advisory report/decision, not hard failure |
| A3 | old large blobs in history | report only |
| A3 | UID repair method unclear | use current Godot tooling, no fabrication |
| A4 | disease probability/threshold missing | decision/data package |
| A4 | medical owner changed | remap, no parallel runtime |
| A4 | gear mitigation semantics unclear | use owner contract / block |
| A4 | hospice-grief consequence unspecified | do not invent |

---

# 10. IMPLEMENTATION LOG STANDARD

Each A-series log contains:

1. plan identity;
2. current census status;
3. dependencies;
4. starting premise;
5. current premise;
6. claims;
7. authority map;
8. exact substep/mini-task completion matrix;
9. sealed/resolved rows;
10. files/data changed;
11. save impact;
12. RNG/determinism;
13. focused commands/results;
14. broader gates;
15. generated outputs/docs;
16. terminal state;
17. remaining blocker;
18. census/ledger update;
19. claim handoff;
20. Part 1.2 dependency impact.

---

# 11. PART 1.1 MASTER ACCEPTANCE MATRIX

| Task | Blocker | Terminal proof |
|---|---|---|
| A1 | dead authored rows | activation/classification matrix + utilization/integrity/save/replay |
| A2 | missing bounded spatial presentation | pure/event-driven views + lifecycle/a11y/reconstruction |
| A3 | repository/tooling hygiene drift | policy-backed gates + LFS/UID/Core/registry/verify-fast |
| A4 | medical causal/continuity gap | deterministic exposure/progression/dependency/palliative/UI/save |

---

# 12. PART 1.2 HANDOFF CONTRACT

## 12.1 B1 — C2[15] Input Reality

Consumes:
- A2 focus topology;
- A2 gamepad-focusable controls;
- A2 host routing;
- A2 lifecycle baseline.

Part 1.2 owns full controller parity and rebinding, not A2.

## 12.2 B2 — C2[16] Session Durability

Consumes:
- A1/A4 final save schema/restore evidence;
- A2 lifecycle stress;
- A3 fast verification/tooling reliability.

## 12.3 B3 — C2[17] Authored Survivor Identity

Consumes:
- A1 content rails;
- A3 content/asset hygiene;
- A4 survivor-state ownership;
- no duplicate identity authority.

## 12.4 B4 — C2[18] Deterministic Survivor Voice

Consumes:
- A1 narrative/radio reachability;
- A2 presentation lifecycle;
- current deterministic selection rules.

Part 1.2 must reverify these outputs at the final Part 1.1 `HEAD`.

---

# 13. PART 1.1 CLOSEOUT CHECKLIST

1. A1 utilization baseline recorded.
2. Every touched orphan row classified.
3. Every activated row has a real consumer.
4. A1 save/replay green.
5. A1 integrity/utilization green.
6. A2 views are projection-only.
7. A2 has no domain polling loop.
8. A2 lifecycle stress green.
9. A2 accessibility/focus green.
10. A2 save reconstruction green.
11. A3 repository-weight methodology documented.
12. A3 LFS audit complete.
13. A3 UID audit complete.
14. A3 budget source recorded.
15. A3 Core barrier green.
16. A3 verify-fast green.
17. A4 live disease/vector baseline measured.
18. A4 exposure rails proven.
19. A4 dependency/withdrawal uses current Dose/Needs owners.
20. A4 palliative behavior does not become a cure.
21. A4 panel uses read model only.
22. A4 quarantine is domain/roster-enforced.
23. A4 save/replay green.
24. Build warning baseline preserved.
25. All implementation logs exist.
26. Census/ledger updated.
27. Claims handed off.
28. Generated docs/indexes green.
29. Part 1.2 handoff packet published.
30. No silent decision blocker remains.

---

# 14. PART 1.1 NON-GOALS

- no B1–B4 implementation;
- no broad narrative rewrite;
- no second resolver/manager;
- no simulation state in UI;
- no Git history rewrite;
- no arbitrary asset thresholds;
- no medical realism expansion beyond Plan 60/live data;
- no hidden balance tuning;
- no new save roots for convenience;
- no full-suite churn as substitute for focused gates.


# APPENDIX A — A1 DETAILED CONTENT ACTIVATION AUDIT

## A.1 Candidate provenance packet

For every candidate row record:

**Row ID:**\
**Catalog:**\
**Schema/version:**\
**Last known plan/package:**\
**Current runtime references:**\
**Reserved/future annotation:**\
**Current consumer:**\
**Current reachability:**\
**Classification:**\
**Proposed action:**\
**Save impact:**\
**Determinism impact:**\
**Focused test:**\

A candidate without this packet is not ready for activation.

## A.2 Consumer proof levels

Use this review scale:

- **Level 0 — Loaded:** parser accepts row.
- **Level 1 — Indexed:** registry/catalog exposes row.
- **Level 2 — Eligible:** runtime can construct a reachable state in which row enters an eligibility set.
- **Level 3 — Triggerable/Selectable:** a deterministic characterization test proves the row actually fires/selects.
- **Level 4 — Stateful continuity:** any resulting quest/stage/consequence survives save/restore and does not duplicate.

A1 activation requires at least Level 3. Any stateful row requires Level 4.

## A.3 Dead-vs-reserved decision tree

1. Is the row referenced by current runtime code/data?
   - yes → scanner/index may be stale; verify and classify REACHABLE-LIVE.
   - no → continue.
2. Is it explicitly reserved by plan/comment/expansion metadata?
   - yes → RESERVED.
3. Does Plan 49/current catalog registry name an intended consumer?
   - yes → ACTIVATE.
4. Is that intended consumer retired?
   - yes → RETIRED or plan correction.
5. Does activation require new gameplay policy/balance?
   - yes → DECISION-BLOCKED.
6. Does a current consumer exist but malfunction?
   - yes → ROUTED-REPAIR.

## A.4 Inbound-edge proof

For each ACTIVATED row the test must show:

`reachable campaign state -> canonical owner/event -> eligibility -> exact row -> expected typed outcome`.

A row merely being present in a list, registry, or generated index does not prove reachability.

## A.5 Encounter activation constraints

Encounter binding must:
- use existing condition/event vocabulary;
- preserve current encounter selection RNG;
- avoid changing unrelated candidate weights;
- preserve neutral parity when new row's condition is false.

If adding one row to a pool changes all probabilities materially, treat it as a balance finding and route.

## A.6 Weather encounter activation

For weather-linked rows:
- weather owner emits/updates current state;
- encounter eligibility reads that state/event;
- narrative does not mutate weather.

Use exact live `WeatherKind`/effect vocabulary.

## A.7 Medical encounter activation

For medical hazard rows:
- medical/exposure owner remains authority;
- encounter can respond to outbreak/exposure state;
- no duplicate disease roll inside narrative.

If the row itself represents an exposure choice, route outcome through the current medical API.

## A.8 Quest prerequisite integrity

Every newly reachable quest must pass:
- prerequisite IDs valid;
- no cycle;
- every required flag can be produced;
- every terminal state reachable;
- rewards valid;
- save/restore stage state stable.

## A.9 Quest reward exactly-once

Characterization:
1. reach completion;
2. execute completion;
3. assert reward;
4. save;
5. restore;
6. revisit completion path;
7. assert no duplicate reward.

## A.10 Orphan item classification

Unused item rows may be:
- planned expansion content;
- quest-only future;
- mod contract placeholder;
- retired;
- genuine omission.

Do not add them to shops/salvage merely to improve utilization metrics.

## A.11 Recipe activation safety

For a recipe newly connected:
- existing workstation tag;
- existing research gate;
- existing tool requirement;
- valid ingredients;
- valid output;
- no free-craft path;
- no duplicate equivalent recipe unless intentional.

## A.12 Shop/salvage activation safety

If a product enters shop/salvage:
- region/tier/scarcity placement must be plan/data-backed;
- do not invent price or rarity values;
- existing economy owner computes price.

Unknown placement becomes decision-blocked.

## A.13 Radio stage graph audit

For each distress/broadcast chain:
- stage IDs unique;
- stage edge trigger valid;
- delay legal;
- no cycle unless explicitly modeled;
- terminal stage reachable;
- follow-up exactly once;
- audio cue valid;
- journal projection valid where contract requires.

## A.14 Radio stage restore test

Save:
- after initial signal;
- before follow-up due;
- after stage transition.

Restore and prove:
- same next stage;
- no duplicate scheduling;
- no replayed journal entry.

## A.15 Companion rescue activation

Verify:
- rescue event can be triggered;
- companion identity unique;
- ownership/capacity rules enforced;
- contamination/disease state preserved if relevant;
- save/restore does not duplicate companion.

## A.16 Moral outcome mapping

| Content outcome | Canonical owner | Typed command/result | Save owner |
|---|---|---|---|
| faction standing | faction/standing |  |  |
| relationship | relationship |  |  |
| morale/needs | needs/modifier |  |  |
| item/currency | inventory/economy |  |  |
| quest flag | quest |  |  |

No direct field mutation in narrative layer.

## A.17 Tone/continuity review

Only touched rows.

Checklist:
- restrained;
- concrete;
- no fake readable text where art contract forbids;
- no stylistic rewrite of unrelated corpus;
- IDs/localization structure preserved.

## A.18 Encoding/schema audit

Verify:
- parser accepts;
- current schema declaration;
- UTF-8 policy;
- no accidental BOM change if repository forbids;
- stable formatting;
- no generated-file manual edit.

## A.19 Utilization report

| Catalog | Total | Baseline unreachable | Activated | Reserved | Retired | Decision/repair | Final unreachable |
|---|---:|---:|---:|---:|---:|---:|---:|
|  |  |  |  |  |  |  |  |

Explain denominator changes.

## A.20 A1 merge-readiness checklist

- [ ] baseline captured;
- [ ] all touched candidates classified;
- [ ] every activated row Level 3+;
- [ ] stateful rows Level 4;
- [ ] no new resolver;
- [ ] no hidden balance changes;
- [ ] focused tests green;
- [ ] save/replay green;
- [ ] integrity green;
- [ ] utilization truthful;
- [ ] registry/index regenerated;
- [ ] implementation log complete.

---

# APPENDIX B — A2 SPATIAL PRESENTATION EXECUTION PACKET

## B.1 Current UI architecture census

Populate before implementation:

| Surface | Route | Presenter/View | Host session | Read model | Events | Lifecycle test |
|---|---|---|---|---|---|---|
| shelter |  |  |  |  |  |  |
| room detail |  |  |  |  |  |  |
| map |  |  |  |  |  |  |
| expedition |  |  |  |  |  |  |

## B.2 Holdfast presenter contract

Presenter may hold ephemeral render descriptors:

- room stable ID;
- layout cell/rect;
- display label;
- current visual status;
- occupancy count;
- hazard glyphs;
- interaction enabled/disabled state.

It may **not** own:
- room health;
- power quantities;
- water amounts;
- production progress;
- assignment state;
- save serialization.

## B.3 Wasteland map contract

Presenter may hold:
- stable location ID;
- layout coordinate;
- discovered/hidden state;
- route geometry;
- hazard tier projection;
- expedition marker.

It may not own:
- canonical travel distance;
- route legality;
- world knowledge;
- expedition progress;
- save state.

## B.4 Host-session boundary

Host session may:
- query domain/read models;
- subscribe to domain events;
- dispatch typed commands.

View/presenter may:
- display;
- focus;
- translate user activation into host/router command.

## B.5 Event-to-update matrix

| Event | Source | Affected view elements | Requery | Full rebuild? |
|---|---|---|---|---|
| room changed | shelter | one room | bounded | no |
| power state | power/shelter | affected rooms | bounded | no |
| flood/contamination | environment | affected rooms | bounded | no |
| assignment changed | roster | one/more room occupancy | bounded | no |
| location discovered | world knowledge | node/routes | bounded | maybe local |
| expedition advanced | expedition | token | one projection | no |
| expedition completed | expedition | token/route state | bounded | no |

## B.6 Room layout source

If `shelter_rooms.json` provides grid/layout:
- use it.

If domain data has no UI layout:
- use presentation-owned layout metadata keyed by stable room ID.

Do not use screen coordinate as domain identity.

## B.7 Room status mapping

Explicitly map current domain states to presentation layers.

Example conceptual mapping only:
- structural damage;
- flood/water;
- power;
- contamination/radiation;
- occupancy.

Do not collapse multiple independent states into one fake enum if the domain supports simultaneous conditions.

## B.8 Hazard overlay composition

A room may be both powerless and flooded.

Presenter should compose independent overlays/icons when the domain permits concurrent hazards.

## B.9 Room command routing

Activation:
- stable room ID;
- route command;
- detail surface.

No direct repair/assign action from map cell unless current UX explicitly defines one.

## B.10 Map node identity

Use canonical place/location stable ID.

Do not identify nodes by display name or vector coordinate.

## B.11 Map route projection

Travel graph supplies:
- connectivity;
- distance;
- cost/hazard metadata.

Presenter chooses visual style but cannot modify route semantics.

## B.12 Discovery gating

Unknown locations:
- hidden/masked exactly as world-knowledge contract states;
- excluded from focus if not interactable;
- no tooltip leaking hidden name if contract forbids.

## B.13 Expedition token interpolation

If `ProgressFraction` exists:
- use as projection.

If not:
- use current authoritative progress representation.

Do not persist pixel position.

## B.14 Event batching

If several events arrive in one campaign tick:
- update final read state once where current host pattern supports;
- no simulation debounce.

## B.15 Idle performance

Success criterion:
- no continuous domain-state polling loop.

Rendering itself naturally consumes engine resources; do not assert “zero CPU”.

## B.16 Focus topology

Requirements:
- every visible enabled interactive control reachable;
- deterministic initial focus;
- hidden disabled targets skipped;
- back navigation returns to a sensible prior surface/focus.

## B.17 Gamepad handoff

Part 1.1 must make controls focusable/actionable.

Full input-map/controller/rebinding parity belongs to C2[15] in Part 1.2.

## B.18 Non-color cues

Every state conveyed by color also has:
- icon;
- text;
- shape/pattern,
according to design system.

## B.19 Subscription lifecycle

For every bind:
- source;
- callback;
- unbind.

100-cycle stress verifies baseline subscriber count restored.

## B.20 Fresh-instance restore

Never serialize UI node.

Test:
1. campaign with mid-expedition/shelter hazards;
2. save;
3. destroy view/runtime;
4. restore;
5. instantiate new view;
6. compare projection.

## B.21 UI snapshot review

Snapshots are evidence only after:
- lifecycle;
- command routing;
- a11y.

No screenshot-only acceptance.

## B.22 A2 merge-readiness checklist

- [ ] projection-only;
- [ ] stable IDs;
- [ ] no polling;
- [ ] route commands canonical;
- [ ] subscriptions balanced;
- [ ] 100-cycle stress green;
- [ ] focus reachable;
- [ ] non-color cues;
- [ ] restore reconstruction;
- [ ] intended snapshots approved;
- [ ] Core remains engine-free;
- [ ] implementation log/census updated.

---

# APPENDIX C — A3 REPOSITORY HYGIENE EXECUTION PACKET

## C.1 Repository weight report

| Category | Size | Tracked? | Generated/local? | Policy action |
|---|---:|---|---|---|
| source |  | yes | source | retain |
| assets |  | yes | source | budget/LFS |
| docs |  | yes | source | index |
| `.git` |  | n/a | history | report separately |
| LFS objects |  | n/a | local store | report separately |
| `.godot` |  | usually no | generated/cache | ignore |
| artifacts |  | mixed | generated | policy |
| tests/logs |  | no | transient | ignore |

## C.2 Measurement reproducibility

Record:
- command;
- OS/tool version if relevant;
- full vs shallow clone;
- LFS fetched or not;
- caches removed or not;
- same exclusions before/after.

## C.3 `.gitattributes` audit

Check:
- LFS filter patterns;
- text/binary markers;
- line endings;
- generated-file rules;
- merge/diff policies.

No wildcard expansion without current policy.

## C.4 Current binary scan

For every tracked binary:
- path;
- extension;
- size;
- attribute;
- LFS object state.

Classify compliant/violation.

## C.5 Historical blob scan

Large blob in old history:
- report hash/path/size;
- do not rewrite.

A3 may recommend a separate history-cleanup package if policy values it.

## C.6 LFS gate

The permanent check should inspect:
- tracked file;
- matching attributes;
- pointer/object state where available.

No need to scan the entire Git history on every fast verification run.

## C.7 UID sidecar audit

| `.cs` | `.cs.uid` | Status | Repair path |
|---|---|---|---|
|  |  |  |  |

Classify:
- valid;
- missing;
- dangling;
- excluded by policy.

## C.8 UID repair

Use:
- Godot import/editor/headless supported UID generation;
- current repository helper.

Never invent UID values from hashes unless that is literally the current Godot/repo algorithm.

## C.9 Asset-budget source table

| Category | Threshold | Policy source | Existing exceptions | Gate tier |
|---|---:|---|---|---|
| UI icon |  |  |  |  |
| texture |  |  |  |  |
| SFX |  |  |  |  |
| music |  |  |  |  |

If the table has blank policy source, do not hard-fail.

## C.10 Budget violation report

Output:
- relative path;
- category;
- size;
- threshold;
- suggested remediation.

No automatic transcoding/downsampling.

## C.11 Godot import audit

Source truth:
- source asset;
- import settings where tracked;
- project references.

Local `.godot/imported` is rebuildable.

Do not commit generated cache merely to “fix” a local mismatch.

## C.12 Asset registry purity

Generator must be:
- deterministic;
- stable-order;
- repo-relative;
- no timestamp/machine path drift.

`--check` detects unregenerated output.

## C.13 Docs index

Measure actual docs.

Verify:
- every indexed path exists;
- every governed doc appears;
- frontmatter valid per policy;
- no stale count assumptions.

## C.14 Secret scanning

Findings report:
- file;
- line/rule;
- masked value.

Never paste secret candidate into logs/handoff.

## C.15 `.gitignore`

Review current transient families:
- Godot cache;
- IDE;
- test output;
- crash dumps;
- generated temp.

Do not ignore canonical source.

## C.16 Test wrapper

Characterization tests for `run_test.sh` should cover:
- no args;
- filter args;
- timeout override if supported;
- exit propagation;
- spaces/quoting.

## C.17 Core engine-free barrier

Preferred layers:
1. project-reference validation;
2. source namespace/type scan;
3. standalone Core build.

The gate should fail with exact violating file/reference.

## C.18 Script health

Use:
- bash syntax;
- current shellcheck policy if present;
- executable bit expectations.

No style cleanup beyond scope.

## C.19 `verify-fast` tiering

Heavy checks may be:
- CI-only;
- release-only;
- nightly.

Keep Tier-1 bounded.

## C.20 Integrity benchmark

If there is an official time budget:
- use controlled repeated measurement;
- do not run with warm caches on one side and cold caches on the other.

## C.21 Pre-commit hooks

Project controls a hook source/installer, not `.git/hooks` itself.

Document:
- installation command;
- checks;
- bypass/emergency policy if current governance defines.

## C.22 A3 merge-readiness checklist

- [ ] weight report reproducible;
- [ ] current LFS violations resolved/classified;
- [ ] historical bloat not destructively rewritten;
- [ ] UID gate green;
- [ ] asset budget policy sourced;
- [ ] registry pure;
- [ ] docs index green;
- [ ] secrets scan green/triaged;
- [ ] Core barrier green;
- [ ] verify-fast green;
- [ ] hygiene guide updated;
- [ ] implementation log/census updated.

---

# APPENDIX D — A4 MEDICAL CONTINUITY EXECUTION PACKET

## D.1 Disease catalog audit

| Disease | Vector(s) | Incubation model | Active stages | Recovery | Treatment refs | Contagious stage(s) |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

Current data defines the table.

## D.2 Exposure-source inventory

| Exposure source | Vector | Canonical event/API | Protection owner | RNG stream | Consumer |
|---|---|---|---|---|---|
| water |  |  |  |  |  |
| airborne/weather |  |  |  |  |  |
| blood/injury/procedure |  |  |  |  |  |
| other live vector |  |  |  |  |  |

## D.3 Infection transaction

Recommended contract, adapted to live owner:

1. receive typed exposure;
2. query protection;
3. resolve seeded risk;
4. create/update infection state if transition occurs;
5. emit semantic transition event;
6. read model reflects new state.

No panel/weather presentation directly writes infection.

## D.4 Protection semantics

Test actual contract:
- no protection;
- partial/degraded protection;
- full protection if the game defines it.

Do not assume HEPA/boiled-water absolute prevention without data.

## D.5 Disease progression state

Persist only authoritative fields required by current model.

Likely:
- disease/pathogen ID;
- infection/exposure day;
- stage;
- stage timing;
- treatment state.

Derived display timing is not separately saved.

## D.6 Stage transition rules

Day/tick keyed.

No wall-clock.

Repeated evaluation in the same authoritative tick should not advance twice.

## D.7 Infection event exactly-once

`OnSurvivorInfected` or current equivalent emits on state transition, not every day while infected.

Same for:
- recovered;
- dependency formed.

## D.8 Treatment transaction

Before treatment:
- resolve item;
- validate disease/stage/applicability;
- check stock;
- consume through inventory;
- apply medical effect.

No item duplication/loss on rejected treatment.

## D.9 Invalid treatment

Use typed result.

Panel displays refusal/reason.

Do not silently consume inapplicable medicine unless current game contract explicitly does.

## D.10 Dose history

Use existing DoseLedger or current equivalent.

Do not duplicate:
- administered doses;
- dates;
- medication identity.

## D.11 Dependency formation

Plan/data defines:
- eligible substances;
- dose band/threshold;
- time window.

If missing:
- decision-block.

## D.12 Withdrawal state

Derived from dependency + missed dose schedule where current contract defines.

Do not invent withdrawal physiology.

## D.13 Needs modifier contributions

Each withdrawal effect:
- stable ID;
- bounded value from plan/data;
- applied once;
- removed/replaced deterministically.

## D.14 Tapering

Only implement if source plan defines.

Tests compare:
- plan-defined tapered path;
- plan-defined abrupt stop path.

This is fictional game behavior, not clinical instruction.

## D.15 Palliative eligibility

Use the current treatment graph/state to determine plan-defined non-curative/terminal status.

No panel-owned terminal checkbox.

## D.16 Palliative action

Action may:
- consume comfort item;
- apply pain/comfort effect;
- alter plan-defined morale consequence.

It must not resolve/cure disease unless the plan explicitly says.

## D.17 Palliative resource accounting

Inventory/economy owner records consumed resource.

No hidden “free hospice” unless intended.

## D.18 Memorial interaction

If hospice context changes grief:
- memorial/morale owner computes that effect from typed context.

Medical system does not own grief weights.

## D.19 Quarantine state

Medical owner determines contagious/quarantine eligibility; roster enforces assignment restriction.

Do not duplicate a quarantine flag in UI.

## D.20 Roster refusal

Attempt shared duty:
- domain preflight;
- typed refusal;
- stable reason;
- no assignment committed.

## D.21 Diagnostic read model

May expose:
- symptoms;
- suspected vector;
- confirmed pathogen only if known;
- stage;
- treatment options;
- quarantine eligibility;
- timeline projection.

No hidden exact values beyond diagnostic contract.

## D.22 Timeline

If simulation uses campaign days:
- display days/ticks.

Do not invent hours.

## D.23 UI lifecycle

Medical panel:
- bind once;
- event-driven refresh;
- unbind on close;
- domain revalidation on commands.

## D.24 Disease totality test

For every live row:
- vector refs valid;
- treatment refs valid;
- symptom refs valid;
- progression valid;
- schema valid.

## D.25 Representative progression tests

Cover each vector/rule family, plus unique special cases.

Avoid 15 copy-paste tests if parameterization can prove the same invariant.

## D.26 Save/reload journey

At least:
- mid incubation;
- mid treatment;
- dependency formed;
- withdrawal active;
- quarantine active;
- palliative state if persisted.

Continuous vs restored suffix equal.

## D.27 Replay determinism

Fixed seed + exposure schedule:
- same infections;
- same transition days;
- same dependency outcome.

## D.28 Balance finding protocol

If observed:
- infection distribution extreme;
- withdrawal too punitive;
- treatment inaccessible;
record exact data and route balance package.

Do not modify numeric values within A4 unless Plan 60 explicitly assigns them.

## D.29 Medical authority matrix update

Document exact owner for:
- exposure;
- infection;
- dose;
- dependency;
- withdrawal;
- treatment;
- quarantine;
- palliative;
- grief projection.

## D.30 A4 merge-readiness checklist

- [ ] live catalog measured;
- [ ] vectors source-grounded;
- [ ] exposure APIs wired;
- [ ] deterministic RNG;
- [ ] incubation stable;
- [ ] treatment transactional;
- [ ] dependency uses dose ledger;
- [ ] withdrawal uses modifier stack;
- [ ] palliative plan-defined;
- [ ] quarantine domain-enforced;
- [ ] UI read-model only;
- [ ] save/replay green;
- [ ] data integrity green;
- [ ] focused suites green;
- [ ] zero warnings;
- [ ] authority docs/census updated.

---

# APPENDIX E — FAILURE-INJECTION REGISTRY

## E.1 A1

Test or classify:
- unknown reward ID;
- impossible trigger flag;
- quest prerequisite cycle;
- radio follow-up cycle;
- dead stage;
- reserved row mistakenly promoted;
- activated encounter false-condition parity;
- newly reachable state saved/restored;
- companion rescue duplicate attempt;
- moral outcome owner missing.

## E.2 A2

Test:
- duplicate subscription;
- stale disposed instance;
- hidden node focusable;
- route target unavailable;
- undiscovered node data leak;
- save/load mid-expedition;
- room receives simultaneous hazards;
- reopen ×100;
- focus return after detail.

## E.3 A3

Test:
- binary violates declared LFS policy;
- generated registry drift;
- missing UID;
- dangling UID;
- engine reference added to Core fixture;
- secret-scan fixture;
- over-budget asset fixture where policy defines;
- malformed script;
- docs-index missing path.

## E.4 A4

Test:
- exposure with no protection;
- exposure with partial protection;
- full protection if supported;
- treatment missing stock;
- invalid treatment;
- save mid-incubation;
- restore during treatment;
- duplicate dependency transition;
- withdrawal modifier duplicate prevention;
- palliative action ineligible;
- quarantine duty attempt;
- UI reopen lifecycle.

---

# APPENDIX F — COMMAND / EVIDENCE LOG TEMPLATE

**Task:**\
**Substep / mini-task:**\
**Command:**\
**HEAD:**\
**Claim state:**\
**Expected:**\
**Actual:**\
**Case count:**\
**Warnings/errors:**\
**Duration if relevant:**\
**Artifact/log:**\
**Disposition:**\

Every handoff statement should point to a command/result or deterministic artifact.

---

# APPENDIX G — IMPLEMENTATION HANDOFF TEMPLATE

**Task:**\
**Source plan:**\
**Starting census state:**\
**Final terminal state:**\
**HEAD:**\
**Claims:**\
**Dependencies:**\
**Historical premise:**\
**Current premise:**\
**Authority map:**\
**Files changed:**\
**Data changed:**\
**Save impact:**\
**RNG/determinism:**\
**Focused tests:**\
**Selftests:**\
**UI gates:**\
**Build/warnings:**\
**Generated artifacts:**\
**Docs:**\
**Remaining blocker:**\
**Part 1.2 impact:**\

---

# APPENDIX H — PART 1.2 ENTRY PACKET

Before B1–B4 start, publish a consolidated packet.

## H.1 A1 outputs
- utilization before/after;
- activated content families;
- reserved/retired rows;
- narrative/radio replay status;
- any new content-related decision blockers.

## H.2 A2 outputs
- current spatial surfaces;
- focus topology;
- gamepad-focusable controls;
- host/router command map;
- lifecycle stress result;
- no-polling proof.

## H.3 A3 outputs
- current `verify-fast` composition;
- Core engine-free gate;
- LFS/asset-budget policy;
- repository weight;
- generator purity status.

## H.4 A4 outputs
- medical authority matrix;
- changed save DTOs/fields;
- RNG stream;
- semantic events;
- panel read-model contract;
- quarantine refusal contract.

Part 1.2 re-verifies these outputs at the merged `HEAD`.

---

# APPENDIX I — FINAL REVIEW QUESTIONS

## I.1 A1
- Did every activated row gain a real inbound edge?
- Did we avoid activating reserved content for metric optics?
- Does each stateful row survive restore?
- Is selection deterministic?
- Did any activation alter scarcity/balance without approval?

## I.2 A2
- Can the view be deleted and rebuilt without losing state?
- Does every domain update arrive by event/read-model?
- Are commands routed through host/router?
- Is every interactive element keyboard/gamepad-focusable?
- Are hidden nodes excluded?
- Are subscriptions balanced?

## I.3 A3
- Are hard thresholds backed by Plan 56/current policy?
- Are history bloat and current violations separated?
- Does Core barrier inspect project references as well as source text?
- Is UID repair delegated to Godot/current tooling?
- Are expensive checks in the correct verification tier?

## I.4 A4
- Is each disease mechanic source-grounded?
- Does medical authority own infection state?
- Does equipment own protection?
- Does Dose own dose history?
- Does Needs own modifier application?
- Does roster enforce quarantine?
- Is the panel read-only projection?
- Are save/replay and RNG deterministic?
- Did we avoid real-world medical extrapolation?

---

# APPENDIX J — FINAL NO-FALSE-CLOSURE RULES

Part 1.1 is not complete if:

- A1 only adds registry references without runtime reachability;
- A1 invents an acquisition tier/rarity to activate an item;
- A1 creates a second resolver;
- A2 persists UI state as gameplay truth;
- A2 uses polling for domain synchronization;
- A2 claims literal zero CPU rather than no polling;
- A2 leaks subscriptions after stress;
- A3 hardcodes unsourced budget thresholds;
- A3 rewrites Git history;
- A3 fabricates Godot UID values;
- A3 turns local cache into tracked source;
- A4 hardcodes stale disease/vector counts;
- A4 invents real-world treatment/protection probabilities;
- A4 adds a second disease/dependency runtime;
- A4 makes quarantine or treatment panel-owned;
- any task lacks focused test evidence;
- generated artifacts are hand-edited;
- warning baseline worsens;
- implementation logs/census are stale;
- Part 1.2 starts from assumptions rather than the final handoff.

**Final invariant:** Part 1.1 must leave ASHFALL with reachable content rails, bounded/event-driven spatial presentation, reproducible repository tooling, and causal/save-stable medical continuity—without creating a second authority in any domain.


# APPENDIX K — TASK-SPECIFIC TERMINAL-STATE DECISION TREES

## K.1 A1

1. Does current `HEAD` already consume the row?
   - **Yes:** VERIFIED-RESOLVED; correct stale utilization metadata if needed.
   - **No:** continue.
2. Is the row explicitly reserved?
   - **Yes:** leave reserved; document classification.
   - **No:** continue.
3. Is there a named canonical consumer?
   - **No:** DECIDED-DEFERRED or RETIRED according to plan intent.
   - **Yes:** continue.
4. Can activation be expressed as a bounded edge with no new authority?
   - **No:** route architecture/repair.
   - **Yes:** implement.
5. Do reachability, save, replay, integrity and utilization gates pass?
   - **Yes:** IMPLEMENTED for that row.
   - **No:** revert/repair the exact failing edge.

A1 is terminal only when every touched candidate has one of these outcomes.

## K.2 A2

1. Does a current spatial surface already satisfy the Plan 51 clause?
   - **Yes:** VERIFIED-RESOLVED for that clause.
2. Does the missing surface require new simulation state?
   - **Yes:** stop and redesign read-model boundary.
3. Can current domain events/read models project required state?
   - **Yes:** implement presenter/view.
4. Is command routing current and typed?
   - **No:** fix host/router seam, not panel math.
5. Do lifecycle, focus, reconstruction and a11y gates pass?
   - **Yes:** IMPLEMENTED.
   - **No:** ROUTED-REPAIR or continue bounded fix.

## K.3 A3

1. Is the observed issue a current violation or merely historical/local cache?
2. Is there a signed/current policy defining the desired rule?
   - **No:** report/advisory, do not create hard gate.
3. Can enforcement be deterministic and cheap enough for its intended tier?
   - **Yes:** implement gate.
4. Would remediation rewrite Git history or destructively optimize source assets?
   - **Yes:** route separate governance package.
5. Does final `verify-fast` remain healthy?
   - **Yes:** IMPLEMENTED.

## K.4 A4

1. Does Plan 60/live data define the medical mechanic?
   - **No:** decision/data blocker.
2. Does a current owner already implement it?
   - **Yes:** verify and close existing clause.
3. Can missing integration route through current Medical/Dose/Needs/Inventory/Roster owners?
   - **Yes:** implement bounded seam.
4. Are probabilities/thresholds present in authoritative data?
   - **No:** do not invent.
5. Do deterministic, save, causal, and UI/read-model gates pass?
   - **Yes:** IMPLEMENTED.
   - **No:** repair exact defect.

---

# APPENDIX L — PART 1.1 COMMIT BOUNDARIES

## L.1 A1 recommended commits

1. baseline + classification artifact;
2. encounter trigger reconnections;
3. item/recipe acquisition reconnections;
4. radio/companion outcome reconnections;
5. validator/tests;
6. registry/index/log/census.

Each production/data commit should be small enough that a failed activation can be reverted without losing unrelated classifications.

## L.2 A2 recommended commits

1. current-view/read-model inventory;
2. Holdfast presenter;
3. Wasteland map presenter;
4. event-driven subscriptions;
5. focus/a11y;
6. lifecycle/save reconstruction;
7. docs/log/census.

## L.3 A3 recommended commits

1. measurements/audit docs;
2. LFS/UID fixes;
3. asset budget gate;
4. Core engine-free barrier;
5. verify-fast integration;
6. hygiene guide/log/census.

Keep policy decisions separate from mechanical fixes.

## L.4 A4 recommended commits

1. medical premise/vector audit;
2. exposure rails;
3. progression/save;
4. dependency/withdrawal;
5. palliative/quarantine;
6. read model/UI;
7. focused tests/authority docs/census.

Do not mix balance retuning into any commit.

---

# APPENDIX M — CROSS-TASK GENERATED-ARTIFACT OWNERSHIP

Shared generated artifacts are a concurrency hazard.

| Generated output | Canonical input | Likely tasks touching | Rule |
|---|---|---|---|
| asset registry | asset/source manifests | A1/A3 | serialize generator input edits |
| catalog registry | data/catalog source | A1/A4 | regenerate after merged source |
| docs index | docs source | all | regenerate at task close, final rerun |
| UI snapshots | UI source | A2/A4 | update only intended surfaces |

If two tasks modify the same generator input:
- claim/serialize;
- never manually merge generated output while inputs diverge.

---

# APPENDIX N — CROSS-TASK OLD-SAVE MATRIX

| Task | Old save behavior |
|---|---|
| A1 | newly reachable content uses existing state defaults; dormant content must not retroactively double-fire |
| A2 | no view state exists in save; fresh views rebuild from restored domain |
| A3 | no game save impact |
| A4 | missing new medical field defaults to neutral/current migration semantics |

Any old-save load crash blocks merge.

---

# APPENDIX O — CROSS-TASK DETERMINISM CHECKLIST

- [ ] no `System.Random` introduced in Core;
- [ ] no `DateTime.Now/UtcNow` used for simulation selection;
- [ ] no filesystem enumeration controls gameplay order;
- [ ] no unordered dictionary iteration controls selection;
- [ ] UI open/close does not advance RNG;
- [ ] save/restore suffix matches continuous run;
- [ ] generated tooling output stable between repeated runs;
- [ ] medical exposure stream registered/stable if new use added.

---

# APPENDIX P — FINAL PART 1.1 RELEASE NOTE TEMPLATE

# ASHFALL Wave 12 Part 1.1 — Closeout

## A1 — C1[16] Depth Passes
**Terminal state:**\
**Baseline orphan count:**\
**Final orphan count:**\
**Activated:**\
**Reserved:**\
**Retired:**\
**Decision-blocked:**\
**Repair-routed:**\
**Integrity:**\
**Utilization:**\
**Replay/save:**\

## A2 — C1[17] Presented Game
**Terminal state:**\
**Holdfast:**\
**Wasteland map:**\
**Event-driven refresh:**\
**Lifecycle ×100:**\
**Focus/gamepad readiness:**\
**Accessibility:**\
**Save reconstruction:**\
**Snapshots:**\

## A3 — C1[18] Weight & Hygiene
**Terminal state:**\
**Checkout weight:**\
**LFS violations:**\
**UID defects:**\
**Asset-budget policy:**\
**Core barrier:**\
**Registry purity:**\
**Docs index:**\
**verify-fast:**\

## A4 — C1[19] Medicine Made Legible
**Terminal state:**\
**Live disease/vector baseline:**\
**Exposure rails:**\
**Progression:**\
**Dependency/withdrawal:**\
**Palliative:**\
**Quarantine:**\
**Medical UI/read model:**\
**Save/replay:**\
**Integrity:**\

## Quality summary
**Build:**\
**Warnings:**\
**Claims handed off:**\
**Census:**\
**Ledger:**\
**Docs/index:**\

## Part 1.2 handoff
**Current HEAD:**\
**B1 input/focus dependencies:**\
**B2 session/save dependencies:**\
**B3 identity/content dependencies:**\
**B4 voice/narrative dependencies:**\
**Open decisions:**\
**Open repairs:**\

---

# APPENDIX Q — FOREMAN / REVIEWER SIGN-OFF

The reviewer signs only with evidence:

- [ ] A1 activated content through existing authorities rather than a second resolver.
- [ ] Every activated stateful row survives save/restore.
- [ ] A1 utilization improvement reflects true reachability, not metric manipulation.
- [ ] A2 presentation contains no authoritative simulation state.
- [ ] A2 updates from events/read models rather than domain polling.
- [ ] A2 teardown releases all subscriptions.
- [ ] A2 focus/accessibility groundwork is sufficient for C2[15].
- [ ] A3 hard gates are backed by current policy.
- [ ] A3 did not rewrite history or destructively alter production assets.
- [ ] A3 proves Core engine-free through project/reference checks.
- [ ] A4 uses current medical/dose/needs/inventory/roster owners.
- [ ] A4 does not invent medical probabilities or real-world regimens.
- [ ] A4 save/replay behavior is deterministic.
- [ ] A4 UI is a typed projection, not a simulation owner.
- [ ] Production build warning baseline is preserved.
- [ ] Generated outputs are in sync.
- [ ] Claims, implementation logs, census, and ledger agree.
- [ ] Part 1.2 handoff reflects final merged state.

If any statement cannot be signed, the owning task remains open or transitions to an explicit non-IMPLEMENTED terminal state.

---

# APPENDIX R — PART 1.2 PROMOTION FILTER

Part 1.2 consists of the four supplied B-series plan heads, but their execution order must be reverified after Part 1.1.

## R.1 B1 — C2[15] Input Reality

Promote when:
- A2 surfaces are stable;
- focus graph exists;
- no lifecycle leak;
- current input action map/host ownership known;
- no active claim.

## R.2 B2 — C2[16] Session Durability

Promote when:
- A1/A4 save migrations are stable;
- A2 lifecycle stress green;
- A3 verification tooling green;
- soak/recovery owner paths unclaimed.

## R.3 B3 — C2[17] Authored Survivor Identity

Promote when:
- A1 content rails accepted;
- A4 survivor state ownership current;
- item/content tag catalogs pass A3/A1 gates;
- identity owner clear.

## R.4 B4 — C2[18] Deterministic Survivor Voice

Promote when:
- A1 narrative/radio rails deterministic;
- B3 identity contract is either a hard dependency or explicitly not required;
- voice selection/delivery authority clear;
- no duplicate social speech owner.

## R.5 Rerank rule

Do not assume B1→B2→B3→B4 if the census DAG says otherwise.

Publish:
`Task | Current premise | Dependencies | Claims | Decision | Ready? | Rank`.

---

# APPENDIX S — FINAL ACTUALITY HEARTBEAT

Immediately before each A task begins, answer:

1. Has the source plan changed?
2. Has concurrent work already sealed any clause?
3. Has a claim appeared?
4. Has a dependency changed state?
5. Does the current class/path still exist?
6. Does current data contradict a supplied hardcoded count/example?
7. Is the task still bounded?
8. Has a decision memo already resolved the apparently open question?

Update the implementation log premise before coding whenever the answer changes scope.

---

# FINAL EXECUTION NOTE

This Part 1.1 plan deliberately preserves the supplied **20-step + four-mini-substep structure** while tightening several assumptions so an implementation agent does not mistake draft examples for repository truth.

The most important execution constraints are:

1. **A1:** reachability, not utilization optics.
2. **A2:** presentation projection, not a second simulation.
3. **A3:** policy-backed hygiene, not arbitrary repository rules.
4. **A4:** Plan/data-defined fictional medical mechanics, not invented real-world medicine.
5. **All tasks:** claims, focused tests, deterministic save behavior, generated-artifact truth, implementation logs, and census/ledger synchronization.

When these four nodes reach truthful terminal states, generate **Wave 12 Part 1.2** from the final merged `HEAD` and consume the A-series handoff rather than reusing the assumptions in the original Wave 12 premise.


# APPENDIX T — FINAL MERGE-READINESS AUDIT

This is the last pre-handoff audit and exists to prevent a task from being marked complete because its implementation “looks right” while one of the governing contracts is still false.

## T.1 A1 merge readiness

A1 is merge-ready only when:
- every changed catalog row has a classification and source-plan rationale;
- every activated row has a reachable test path through a canonical consumer;
- no reserved row was promoted solely to improve utilization statistics;
- no reward, item, recipe, radio stage, companion event, or moral outcome points to a missing authority;
- all stateful content survives restore without duplicate completion/reward/follow-up;
- deterministic selection remains stable under repeated same-seed runs;
- registry and docs generators are in sync.

## T.2 A2 merge readiness

A2 is merge-ready only when:
- Holdfast and Wasteland map state can be destroyed/recreated from domain state;
- no presenter/panel field becomes authoritative;
- room and map updates are event/read-model driven;
- all event subscriptions have matching teardown;
- the 100-cycle lifecycle test demonstrates no duplicate callbacks;
- hidden/disabled nodes are not accidentally keyboard/gamepad focus targets;
- route commands use stable IDs and current router/host seams;
- restored campaign state produces the same fresh-view projection.

## T.3 A3 merge readiness

A3 is merge-ready only when:
- repository weight measurements are reproducible;
- LFS findings distinguish current policy violations from historical object-store bloat;
- all enforced asset limits cite the plan/current policy;
- UID sidecars are repaired by supported tooling;
- generated registry output is reproducible;
- the Core engine-free barrier checks project references as well as source heuristics;
- `verify-fast` remains appropriately fast and green;
- no destructive history rewrite or automatic asset degradation occurred.

## T.4 A4 merge readiness

A4 is merge-ready only when:
- current disease/vector data has been remeasured;
- every implemented exposure route enters through the medical authority;
- protection is queried from the equipment owner;
- dose history remains singular and authoritative;
- dependency/withdrawal effects are deterministic and non-duplicating;
- palliative care follows Plan 60 rather than invented clinical logic;
- quarantine is enforced at the domain/roster boundary;
- diagnostic presentation respects uncertainty and does not reveal hidden state;
- mid-flow save/restore and same-seed replay are green;
- no numeric medical balance value was invented to fill a missing design field.

## T.5 Final handoff invariant

The Part 1.1 handoff is valid only when a Part 1.2 implementer can answer, without re-auditing these four plans:

- Which content rails are now live and deterministic?
- Which spatial controls are focusable and controller-ready?
- Which tooling gates are authoritative and fast?
- What medical state/save/event contracts changed?
- What claims remain active?
- What decision or repair packages remain open?
- What exact merged `HEAD` should B1–B4 consume?

If any answer is absent, Part 1.1 remains incomplete.

**End of Wave 12 Part 1.1.**
