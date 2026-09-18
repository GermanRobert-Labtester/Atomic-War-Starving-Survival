# ASHFALL — GENERATION WAVE 10 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 2

**Document role:** execution-grade Part 2 plan for Wave 10, continuing from the Wave 10 corpus census / claim-hygiene / micro-deferral / C1[6] / C1[7] work.

**Part 2 execution set:**
1. **B3 — C1[8] “The Event Layer Speaks” (Plan 31 full contract)**
2. **B4 — C1[9] “Intel Has To Be Worth Something”**
3. **B5 — C1[10] “Goods Must Arrive” (Plans 35/36 delivery chain)**
4. **C1 — C2[8] “Ship Gate” census-driven delta**
5. **C2 — C2[9] “Orchestration Spine” census-driven delta**
6. **D1 — Seven-Day Slice Standing Product Proof**
7. **E1 — Standing Decision Register execution pass**
8. **F1 — E1 corpus plan identity/prerequisite resolution**

**Primary discipline:** every package begins with **read the plan → consume the census row → re-verify current source → resolve claims/dependencies → execute only the current delta → write implementation log → update census/ledger**.

Part 2 assumes Part 1 has produced:
- a current `UNCLAIMED_CORPUS_CENSUS.md`;
- truthful claim-state data;
- a micro-deferral disposition report;
- current C1[6]/C1[7] status;
- a reranked DAG.

If any of those inputs are absent or stale at execution time, regenerate/reconcile them before claiming the corresponding Part 2 package.

---

# 0. GLOBAL EXECUTION CONTRACT

## 0.1 Terminal states

Each task/sub-plan ends as exactly one of:

- **SEALED** — current contract fully satisfied.
- **SEALED-ELSEWHERE** — current contract already satisfied by prior work.
- **PARTIALLY-SEALED** — exact remainder remains and is named.
- **DEPENDENCY-BLOCKED** — current valid work waits on an unsealed prerequisite.
- **CLAIM-BLOCKED** — valid work waits on active path ownership.
- **DECISION-BLOCKED** — valid work waits on foreman verdict.
- **SUPERSEDED** — newer architecture/plan replaces historical contract.
- **STALE-PREMISE** — plan’s premise is contradicted by current source.
- **RETIRED** — product/architecture decision closes the plan without implementation.
- **ROUTED-REPAIR** — execution exposes a genuine production regression outside the package’s intended delta.

No task may finish as “mostly done”, “TBD”, “probably covered”, or “future”.

## 0.2 Hard rules

1. Claims before edits.
2. Census row before chain execution.
3. Dependencies before promotion.
4. Plan doc defines scope; current source defines truth.
5. Earlier wave work gets credited; it is never reimplemented for completion optics.
6. A missing authority is a blocker, not permission to invent one.
7. Decision-gated behavior stops for signature.
8. No new parallel authority for briefing, intel, production sinks, release gates, orchestration, or decision tracking.
9. Save changes are additive only when required and old-save neutral.
10. Derived diagnostics/read models do not create new persistence.
11. No hidden balance changes in delivery/intel/release/orchestration tasks.
12. Focused verification first.
13. Generated docs/manifests/checklists are updated through their source/process.
14. Plan logs are mandatory for newly executed corpus plans.
15. Census is updated after each terminal package.
16. If current source already satisfies the historical plan, close the delta honestly and do not churn code.
17. No big-bang migration when the plan’s current delta is larger than one reviewable package; promote the migration separately.

## 0.3 Required evidence bundle

Every task handoff includes:

- current `HEAD`;
- consumed census row;
- dependency status;
- active claim status;
- plan-clause matrix;
- sealed-elsewhere evidence;
- executed-delta file list;
- save impact;
- determinism impact;
- focused test commands/results;
- owning suite/selftests;
- build;
- generator/index checks;
- implementation log;
- census update;
- integration-ledger update;
- exact remaining blocker;
- next DAG node released.

## 0.4 Abort conditions

Stop if:
- a prerequisite is unsealed;
- active Plan 24 or another claim owns required paths;
- Wave 9 narrower work did not land and the full plan assumes it;
- current architecture contradicts a historical owner assumption;
- B5 producer delivery would require a new sink/resource currency;
- C1 ship-gate work would duplicate release infrastructure instead of extending it;
- C2 manifest work expands into a repo-wide migration beyond bounded scope;
- D1 slice would need new game systems rather than composing existing ones;
- E1 decision item has no verdict;
- F1 E1 corpus identity remains unread/ambiguous.

---

# 1. DEPENDENCY GRAPH AND EXECUTION ORDER

## 1.1 Recommended order

1. **B3 — C1[8]**
2. **B4 — C1[9]**
3. **B5 — C1[10]**, only after Plan 23 + B1 + B3 gates
4. **C1 — C2[8]**
5. **C2 — C2[9]**
6. **D1 — Seven-Day Slice**, only after required chain nodes are terminal
7. **E1 — Decision Register Pass**
8. **F1 — E1 corpus plan identity/prerequisite resolution**

This is a default execution order. The census DAG may place C1/C2 earlier because they are independent of C1[8]–[10]. Preserve DAG readiness over narrative order.

## 1.2 Hard chain dependencies

- B3 consumes Wave 9 B1 state and Plan 24 claim state.
- B4 follows B3 if C1[9] declares C1[8] a hard dependency.
- B5 requires:
  - Plan 22A sealed;
  - Plan 21 sealed;
  - B1 / Plan 27 sealed where its fixtures are required;
  - B3 / Plan 31 sealed;
  - Plan 23 power/heat prerequisite resolved;
  - producer-port sub-plan 36A executed in plan order.
- D1 slice waits on whatever chain nodes the D1 plan explicitly names.
- F1 waits on A1 census identity row for E1 but may be used to complete that row.

---

# 2. TASK B3 — C1[8] “THE EVENT LAYER SPEAKS” / PLAN 31 FULL CONTRACT

## 2.1 Objective

Reconcile and execute the **full** C1[8] Plan 31 contract: semantic day events, navigable briefings, and replayable diagnostics. Wave 9 B1 covered only the narrower semantic-kind authority; B3 must first determine whether that work landed, then deliver only the true remainder.

## 2.2 Entry gate

Before edits:
- read C1[8] in full;
- consume current census row;
- check Wave 9 B1 implementation/log status;
- check Plan 24 claim status around `DailyBriefingReportBuilder.cs`;
- inspect current replay/debug harness;
- locate event vocabulary/parity authority;
- locate current briefing route/panel mechanics.

Possible entry states:

### Case A — Wave 9 B1 landed
Scope starts at:
- navigable briefings;
- replayable diagnostics delta;
- any richer semantic-event clauses beyond B1.

### Case B — Wave 9 B1 did not land
B3 begins with semantic-kind totality and parity extension, then proceeds to remainder.

### Case C — current source already satisfies all three axes
Close SEALED-ELSEWHERE/VERIFIED-RESOLVED with clause evidence; do not reimplement.

## 2.3 Clause matrix

Build:

`Sub-plan clause | Historical requirement | Current implementation | Sealed by | Current remainder | Claim/dependency | Action`.

Required axes:
- semantic day-event classification;
- navigable briefing;
- replayable diagnostics.

Do not treat these as one opaque feature.

## 2.4 Semantic-event axis

If missing:
- implement/finish the semantic-kind layer beside current event vocabulary;
- total mapping;
- loud failure for unmapped registered IDs;
- static/derived classification;
- no save state;
- no duplicate event registry.

If already present:
- run totality/parity gates;
- record SEALED-ELSEWHERE under Wave 9 B1 or current implementation.

## 2.5 Navigable briefing contract extraction

Read exact C1[8] DoD.

Determine whether navigation means:
- entry → panel/source route;
- entry → detail view;
- entry → system tab;
- entry → event provenance;
- drill-down and back navigation.

Do not invent richer UX than contract.

## 2.6 Briefing authority boundary

Briefing builder owns:
- event/entry assembly;
- semantic identity/vocabulary.

Navigation layer owns:
- route target metadata;
- user command to navigate;
- focus/back behavior.

Navigation must **not**:
- mutate briefing content;
- create new domain events;
- duplicate event storage;
- infer route from display text.

## 2.7 Route target contract

Preferred typed metadata:

`BriefingEntry -> SemanticKind/EventId -> NavigationTarget`.

Target can include:
- route ID;
- entity/source ID if current route framework supports;
- optional focus/detail key.

Stable IDs only. No free-form string parsing from localized text.

## 2.8 Claim coordination

If Plan 24 still owns the briefing builder:
- do not edit claimed file;
- split B3:
  - semantic/diagnostics work lands outside claim;
  - navigation metadata/consumer waits;
- mark CLAIM-BLOCKED exact remainder.

If claim has formally handed off:
- claim path and proceed.

No alternate briefing builder.

## 2.9 Navigation tests

Must cover:
- briefing entry with valid target;
- entry with no target if contract allows;
- route target resolves;
- drill-down opens correct panel/detail;
- back returns to briefing;
- focus returns predictably;
- keyboard navigation;
- disabled/unavailable target handled honestly;
- localized text change does not alter routing.

## 2.10 Replayable diagnostics contract extraction

Read exact plan requirement.

Map to current Wave 6 replay/debug harness:
- trace capture;
- deterministic replay;
- diagnostic event listing;
- state fingerprints;
- event provenance.

Classify:
- already satisfied;
- partially satisfied;
- missing.

## 2.11 Diagnostic trace model

If delta requires semantic trace:

Each trace row may include, only as plan/current harness supports:
- day/tick;
- event ID;
- semantic kind;
- source owner/system;
- relevant entity ID;
- route target;
- deterministic ordering.

Diagnostics remain read-only.

## 2.12 Replay invariants

- same input/seed → same trace;
- diagnostics do not mutate campaign state;
- replay trace does not consume RNG;
- trace ordering stable;
- semantic-kind mapping total;
- save/restore suffix trace matches where harness promises it.

## 2.13 Briefing-event replay

If plan explicitly requires retracing a day’s briefings:
- replay actual event trace;
- reconstruct/report the same semantic entries;
- do not persist a duplicate briefing history unless plan requires it;
- prefer trace-derived view.

## 2.14 Focused tests

1. semantic totality/parity;
2. navigation target resolution;
3. keyboard/back navigation;
4. diagnostics deterministic trace;
5. replay trace comparison;
6. read-only fingerprint;
7. claim-safe split behavior if necessary.

## 2.15 UI gates

For changed briefing surface:
- route parity;
- lifecycle;
- accessibility;
- keyboard;
- focus return;
- snapshot.

No snapshot-only acceptance.

## 2.16 B3 implementation log

Must reconcile:
- full C1[8] contract;
- Wave 9 B1 narrower contract;
- actual delivered state.

Include:
`Clause | Wave 9 B1 | Existing source | B3 delta | Evidence | Final status`.

## 2.17 Census / ledger update

After B3:
- C1[8] terminal status;
- C1[9] readiness recomputed;
- Wave 9 B1 note updated to show subsumption/partial relationship;
- integration ledger row added/updated.

## 2.18 Verification order

1. semantic tests;
2. navigation focused tests;
3. replay diagnostics tests;
4. Campaign suite;
5. parity gate;
6. route/lifecycle/a11y;
7. replay/fingerprint;
8. snapshots;
9. build;
10. verify-fast.

## 2.19 B3 non-goals

- no parallel briefing authority;
- no new event registry;
- no content authoring;
- no claim racing;
- no persistence for derived diagnostics without plan requirement.

---

# 3. TASK B4 — C1[9] “INTEL HAS TO BE WORTH SOMETHING”

## 3.1 Objective

Execute the current remainder of C1[9] by proving that intel outputs—signals, traces, forecasts, reliability/comms information—have real, authoritative consumers that measurably influence player decision paths. Credit Waves 2–3 distress/trust/follow-up work, Plan 173 radio production, and 20C forecast/reliability work before implementing anything.

## 3.2 Entry gate

- B3/C1[8] terminal if hard dependency.
- Current census row consumed.
- Full C1[9] read.
- Radio/distress/forecast systems mapped.
- A3 radio weather-prediction content status checked.

## 3.3 Premise reconciliation

Build:

`Historical intel requirement | Current owner | Current output | Current consumer | Decision impact | Remainder`.

Intel categories may include:
- distress signals;
- traces;
- forecasts;
- reliability;
- communications infrastructure.

Use exact plan categories.

## 3.4 “Worth something” acceptance

The plan’s core demand must be translated into measurable behavior.

For each intel output:
- there is a consumer;
- consumer affects an available player decision or strategic state;
- with intel vs without intel, the observable decision context differs;
- output is not merely decorative text unless contract explicitly treats it as flavor.

Do not force player behavior; prove that the information changes actionable state/choices.

## 3.5 Consumer-chain audit

For every intel type:

`Output -> Read model/event -> Consumer surface/system -> Player action/decision -> Observable consequence`.

Classify:
- COMPLETE;
- DEAD OUTPUT;
- PARTIAL;
- PRESENTATION-ONLY BY DESIGN;
- DECISION-NEEDED.

## 3.6 Missing consumer rule

If output is dead:
- first read C1[9] named intended consumer;
- wire through existing owner;
- if plan does not define consumer semantics and product choice is material, route decision;
- never invent generic “intel score”.

## 3.7 Forecast reliability axis

If C1[9] requires reliability:
- map onto current 20C model;
- verify confidence/reliability exposed to correct consumer;
- no second reliability calculation;
- no raw probability claims unsupported by current model.

If already complete, cite and close.

## 3.8 Radio weather-prediction content relationship

A3 may author content.

B4 owns mechanism/value chain:
- forecast exists;
- radio delivers it;
- reliability correctly represented;
- player can act on it through existing systems.

No extra authored rows unless C1[9] itself names mechanical test content and A3 did not own it.

## 3.9 Distress/trust/follow-up relationship

Credit existing:
- trust ledger;
- follow-up scheduler;
- distress audio/content;
- selection policy if Wave 9 C2 lands.

Check whether C1[9] requires these to feed decision context beyond what exists.

Do not duplicate them.

## 3.10 Comms infrastructure axis

Compare plan to Plan 173/current radio station production.

Possible statuses:
- SEALED-ELSEWHERE;
- partial;
- open.

If current radio infrastructure already satisfies transmission/channel/program mechanics, no rewrite.

## 3.11 Intel-value tests

For each relevant output:
- construct same campaign state;
- case A has intel available;
- case B lacks/withholds intel according to canonical mechanics;
- assert decision surface/options/context differs as plan expects;
- assert downstream state only changes when player/system acts, not merely because intel is displayed.

This is decision-context measurability, not forced deterministic player choice.

## 3.12 Missing-consumer failure behavior

If intel cannot be consumed:
- typed/unambiguous state;
- no silent drop if plan forbids;
- journal/diagnostic indication only if existing pattern requires.

## 3.13 Determinism

Intel evaluation:
- state-derived;
- no new RNG unless current source owner already uses seeded selection;
- same seed/state → same intel;
- read surfaces do not mutate simulation;
- fingerprint stable when only reading intel.

## 3.14 UI acceptance

For changed intel surfaces:
- route;
- lifecycle;
- a11y;
- clear reliability language;
- no fake precision;
- snapshots.

## 3.15 B4 log

Include:
`Intel output | Historical intent | Current owner | Consumer | Decision impact | Work executed | Test | Final status`.

## 3.16 Verification

- focused consumer-chain tests;
- Radio suite;
- World/Weather where forecasts touched;
- Campaign where decision context touched;
- replay/fingerprint;
- UI gates;
- build;
- verify-fast.

## 3.17 B4 non-goals

- no new intel authority;
- no generic intel currency;
- no content expansion beyond plan mechanics;
- no second forecast/reliability model;
- no player-choice automation.

---

# 4. TASK B5 — C1[10] “GOODS MUST ARRIVE” / PLANS 35–36 DELIVERY CHAIN

## 4.1 Objective

Implement the production-delivery integrity chain so every producer either:
1. commits a real persisted effect into the correct authoritative sink; or
2. returns a typed, player-visible refusal.

The permanent invariant is: **silent production loss is forbidden**.

Mandatory internal order:
**35A → 36A → 35B → 35C**.

## 4.2 Entry gate

Before B5:
- Plan 22A sealed;
- Plan 21 sealed;
- B1/Plan 27 fixture fidelity available if required;
- B3/Plan 31 semantic events available if required;
- Plan 23 power/heat status resolved;
- active claims checked;
- full C1[10] read.

If Plan 23 unsealed and hard:
- B5 DEPENDENCY-BLOCKED;
- promote Plan 23 package according to census.

## 4.3 Guardrails

No:
- new resource currency;
- new producer subsystem;
- rival bill/work-order framework;
- hidden storage arithmetic;
- panel-local delivery;
- content expansion before delivery rails.

## 4.4 35A producer→sink audit

Enumerate every producer named by plan/current system, such as:
- greenhouse;
- kitchen;
- workshop/foundry;
- trapping;
- compost;
- medical production;
- other plan-listed producers.

For each:

`Producer | Command/tick | Output | Intended sink | Current delivery | Persisted? | Refusal? | Silent-loss risk | Status`.

## 4.5 Sink authority identification

Possible sink types:
- inventory;
- needs/food state;
- power/heat;
- medical stock;
- world storage.

Each output maps to existing sink owner.

No producer directly edits sink internals outside canonical API.

## 4.6 Silent-loss detection

A silent-loss row exists when:
- producer reports success;
- expected sink state does not change;
- no typed refusal/error;
- output disappears or is ignored.

Tests should intentionally construct:
- valid delivery;
- full/blocked sink;
- missing prerequisite;
- invalid output mapping.

## 4.7 Typed refusal contract

Refusal includes stable reason identity appropriate to owner.

Examples only if current plan supports:
- storage_full;
- missing_power;
- missing_materials;
- destination_unavailable;
- producer_disabled.

Do not invent reason vocabulary without current contract.

Player-visible surface uses typed result.

## 4.8 Atomic delivery

Production transaction:
1. validate prerequisites;
2. compute output;
3. attempt sink commit;
4. commit production cost/state only when semantics allow;
5. if sink refuses, return typed result without silent loss;
6. emit event/report if plan requires.

If producer consumes inputs before sink commit, rollback/transaction semantics must be explicit.

## 4.9 36A producer-port gate

Read exact Plan 36 contract.

Likely purpose: a common producer contract proving delivery capability.

Implement only the source-plan contract.

Requirements:
- producers conform through existing architecture;
- no ad hoc per-producer delegate explosion;
- gate enumerates/validates producers;
- any new producer must satisfy delivery/refusal contract.

## 4.10 Permanent no-silent-loss gate

Create/extend test family that fails when:
- registered producer lacks sink mapping;
- success path does not mutate authoritative sink;
- failure path lacks typed refusal;
- persistence loses output.

Prefer structured producer registration/enumeration over hard-coded duplicate list where current architecture supports it.

## 4.11 35B storage/spoilage integrity

Read exact plan.

Verify:
- produced item reaches storage;
- capacity rules;
- spoilage owner receives stored item;
- save/restore preserves quantity/age/state;
- no duplicate spoilage arithmetic in producer.

Only current delta.

## 4.12 35C research/unlock/content separation

If plan requires:
- production recipe/unlock reads canonical research owner;
- locked producer refuses honestly;
- content rows remain data;
- mechanism not coupled to authored expansion.

No content wave in B5.

## 4.13 Labour/power dependencies

Plan 24 labour and Plan 23 power/heat may gate production.

Use canonical current APIs:
- workforce assignment/skill-to-yield;
- power availability;
- heat/state.

B5 must not implement missing labour/power systems itself beyond required dependency integration.

## 4.14 Save round-trip

For every changed producer family:
- produce;
- confirm sink;
- save;
- restore;
- compare output/sink state;
- advance spoilage/use if relevant.

Comprehensive save battery required.

## 4.15 Determinism

Production-heavy seeded window:
- same seed/input → same outputs;
- delivery ordering stable;
- no collection-order loss;
- fingerprint stable across restore.

## 4.16 Player-visible refusal

Affected UI/host:
- surfaces reason;
- no false success notification;
- no production animation/text if sink rejected unless UI explicitly communicates rejection.

## 4.17 B5 log centerpiece

Producer audit before/after:

`Producer | Output | Sink | Before behavior | After behavior | Refusal | Save test | Focused suite`.

## 4.18 Verification

- 35A tests;
- 36A gate;
- 35B storage/spoilage;
- 35C unlock;
- greenhouse;
- kitchen;
- foundry/workshop;
- trapping;
- relevant medical/compost;
- comprehensive save;
- replay/fingerprint;
- data integrity;
- utilization where mappings/data touched;
- build;
- verify-fast.

## 4.19 B5 non-goals

- no new sink authority;
- no content expansion;
- no new currency;
- no alternate bills framework;
- no hidden UI arithmetic;
- no Plan 23/24 redesign.

---

# 5. TASK C1 — C2[8] “SHIP GATE” CENSUS-DRIVEN DELTA

## 5.1 Objective

Reconcile C2[8] against current release/export infrastructure, credit already-sealed export smoke, CI parity, platform presets, and performance budgets, then implement only missing deltas in:
- single data path authority;
- real export smoke;
- performance budgets;
- release-blocking ship gate composition.

## 5.2 Clause reconciliation

`C2[8] clause | Prior sealing package | Current evidence | Remainder | Action`.

Cross-reference:
- Wave 2 D4;
- Wave 4 D1/D2/D3;
- existing perf budgets;
- release-captain docs;
- `RELEASE_EXPORT` docs.

## 5.3 Single data path authority

Read exact plan.

Audit:
- repository canonical Data location;
- export copy location;
- generation/copy process;
- drift detection;
- runtime read path.

Goal: one authoritative source even if export physically contains a copy.

## 5.4 Data-copy parity gate

If current delta requires:
- deterministic copy step;
- checksum/manifest comparison;
- export fails when copy differs from source.

Do not require runtime to read repo path from shipped build.

“Single authority” means source-of-truth discipline, not necessarily one physical directory.

## 5.5 Export smoke delta

Compare plan acceptance to current smoke:
- shipped binary used?;
- real exported data?;
- real composition root?;
- headless/runtime arguments?;
- startup to meaningful state?;
- failure on fallback/mock path?

Implement only missing assertions.

## 5.6 Performance budgets

Audit plan-named axes:
- startup;
- frame/update;
- save/load;
- content load;
- other named budgets.

Credit existing gates.

Missing budget:
- benchmark current baseline;
- use plan/signed threshold;
- add focused gate;
- avoid arbitrary threshold invention.

## 5.7 Ship-gate composition

If plan defines release blocker:
- enumerate required gates;
- compare to release checklist;
- add missing current-valid gates;
- remove stale/superseded gate references only with evidence.

One canonical release battery.

## 5.8 Failure semantics

Ship gate should fail clearly on:
- data drift;
- export boot failure;
- required perf breach;
- missing artifact.

No silent warnings for release-blocking contract.

## 5.9 Implementation log

Include:
`Clause | sealed by | delta | gate | result`.

## 5.10 Verification

- data parity gate;
- export smoke;
- performance gates;
- release battery;
- build;
- verify-fast;
- docs index/check.

## 5.11 C1 non-goals

- no CI rewrite;
- no second release pipeline;
- no reexecution of sealed smoke;
- no arbitrary perf thresholds.

---

# 6. TASK C2 — C2[9] “ORCHESTRATION SPINE” CONSOLIDATION CONTRACT

## 6.1 Objective

Reconcile C2[9] against current composition/orchestration architecture, credit Wave 2 decomposition and Wave 7 loader/event/clock consolidation, close small lifecycle/declarative gaps, and promote any repo-wide manifest migration as a separate package rather than performing a big-bang rewrite.

## 6.2 Entry gate

- census row current;
- full C2[9] read;
- current composition root mapped;
- current campaign day coordinator owner registry mapped;
- setup/save/flush triad gate mapped;
- active claims checked.

## 6.3 Clause matrix

Axes:
- orchestration spine;
- declarative subsystem manifest;
- behavior-preserving decomposition;
- lifecycle contracts.

For each:
`Historical contract | Current architecture | Prior sealing | Delta | Size | Action`.

## 6.4 Current orchestration map

Identify:
- subsystem construction;
- registration;
- setup;
- save capture;
- restore;
- flush/dispose;
- day tick;
- event bus ownership.

No assumptions from historical Main partials without source verification.

## 6.5 Declarative manifest delta

If current owner registry already acts as partial manifest:
- list what it declares;
- list what remains scattered;
- identify whether plan requires construction declaration too.

If migration touches many setup calls:
- create promoted package;
- do not execute in C2.

## 6.6 Small manifest deltas

Allowed inline only if:
- one bounded registry;
- behavior-neutral;
- no broad constructor migration;
- triad gate covers it.

## 6.7 Behavior preservation

Before any movement:
- pin current fingerprints;
- setup/save/flush parity;
- ordering;
- current tests.

After:
- identical behavior.

No feature/balance change.

## 6.8 Lifecycle contract audit

Compare plan phases to current gates:
- construct;
- setup;
- day tick;
- save;
- restore;
- flush;
- dispose.

Identify missing phase/gate only if plan/current architecture still requires it.

## 6.9 Triad gate extension

If lifecycle gap small:
- extend existing gate family;
- do not create parallel lifecycle test framework.

## 6.10 Large migration promotion

Create queue package:
- exact scattered paths;
- intended manifest owner;
- migration phases;
- parity gates;
- rollback.

C2 then closes PARTIALLY-SEALED with promoted remainder.

## 6.11 Implementation log

Show:
- clauses sealed elsewhere;
- current small deltas;
- promoted migration;
- behavioral proof.

## 6.12 Verification

- triad gate;
- focused orchestration tests;
- composition/bootstrap;
- replay/fingerprint;
- build;
- verify-fast.

## 6.13 C2 non-goals

- no big-bang manifest migration;
- no behavior change;
- no new global service locator;
- no duplicate lifecycle framework.

---

# 7. TASK D1 — SEVEN-DAY SLICE AS STANDING PRODUCT PROOF

## 7.1 Objective

Deliver the D1 plan’s standing product-level acceptance artifact: a repeatable seven-day ASHFALL slice combining:
- deterministic scripted campaign evidence;
- human seven-day playthrough checklist;
- release-cadence integration.

This is not another subsystem test. It proves the core player loop composes end-to-end.

## 7.2 Entry gate

Read D1 fully and identify dependencies.

If required C1[8]/[9]/[10] nodes remain open:
- D1 DEPENDENCY-BLOCKED;
- do not fake missing loops.

## 7.3 Slice contract extraction

Record:
- day 0/1 start;
- day 7 terminal;
- required systems;
- required player decisions;
- required failures/recoveries;
- product acceptance;
- scripted/human split;
- release cadence.

No guessed slice content.

## 7.4 Scripted half

Use existing replay/scenario infrastructure.

Requirements:
- real composition;
- production catalogs;
- seeded deterministic state;
- seven day advances;
- per-day assertions;
- save/restore if plan requires;
- day-7 fingerprint;
- no synthetic demo world unless plan explicitly defines.

## 7.5 Core loop coverage

Use exact D1 plan, likely including:
- needs;
- production;
- intel;
- medical;
- social;
- shelter;
- expedition/world if required;
- resource decisions.

For each:
`Loop link | Day observed | Player/system action | State evidence | Failure if absent`.

## 7.6 Per-day oracle

Every day:
- inputs;
- meaningful action/decision;
- state changes;
- required event/briefing;
- no impossible state.

Avoid brittle assertions on incidental text.

## 7.7 Save midpoint

If product proof includes persistence:
- save around day 3/4;
- continuous run terminal;
- restored run terminal;
- compare day-7 state/fingerprint.

## 7.8 N-run stability

Run scripted slice same seed N times.

No flake.

If different seeds are required, each seed has its own oracle; do not statistically wave away variance.

## 7.9 Human playthrough half

Create checklist with:
- setup/build/version;
- day-by-day objectives;
- observations;
- blockers;
- UX friction;
- screenshots only if current QA discipline requires;
- severity/owner;
- pass/fail.

Human half validates player experience, not deterministic internal values.

## 7.10 Human finding policy

Findings:
- regression;
- UX issue;
- balance concern;
- content gap;
- false positive.

Do not fix unrelated findings inline; route them.

## 7.11 Standing release cadence

Add scripted slice to release checklist/battery at cadence defined by D1.

Human slice:
- per release window if plan requires;
- not every commit unless specified.

## 7.12 Artifact set

- D1 implementation log;
- standing slice definition;
- scripted command;
- expected per-day oracle;
- human checklist;
- first execution report;
- release checklist update.

## 7.13 Verification

- scripted slice green;
- N-run stable;
- day-7 fingerprint;
- save/restore parity;
- human checklist executed;
- build;
- verify-fast;
- release-battery integration.

## 7.14 D1 non-goals

- no new systems;
- no balance changes;
- no content tranche;
- no masking open chain dependencies.

---

# 8. TASK E1 — STANDING DECISION REGISTER EXECUTION PASS

## 8.1 Objective

Convert the accumulated decision-gated seam set from scattered memos into a standing register with terminal verdicts, then execute or queue signed decisions according to their existing memo contracts.

Success metric:
**zero unsigned-without-condition items**.

## 8.2 Register discovery

Search:
- Waves 8–9 decision memos;
- earlier decision docs;
- KNOWN_DEBT;
- integration ledger;
- closeouts.

Include known classes:
- ward staffing;
- black-market funds/goods;
- amputation equipment;
- portfolio dispositions;
- merchant restock;
- SignalTrust pool-or-retire;
- radiation F1–F8;
- F1/F9 governance;
- water sample quirk;
- undo;
- voice-over;
- tamper posture;
- localization expansion;
- 170–199 HOLD conditions;
- any A3 routed decisions.

Do not invent new memos in this pass.

## 8.3 Register schema

`Decision ID | Topic | Memo | Routed date | Owner | Current verdict | Condition | Execution package | Evidence | Recheck`.

Verdicts:
- SIGNED;
- DECLINED;
- DEFERRED-WITH-CONDITION;
- UNSIGNED.

## 8.4 One-session foreman review

Present concise decision packet:
- topic;
- current evidence;
- options from existing memo;
- recommended architecture-safe option if memo includes one;
- consequence.

Collect verdicts.

E1 does not substitute its own judgment.

## 8.5 Signed verdict execution

For each SIGNED:
- if bounded and memo authorizes inline, execute with memo gates;
- if large, create ledger package;
- update source doc.

Do not execute before signature.

## 8.6 Declined verdict

Record:
- decline;
- reason;
- date;
- owning blocker closed.

Do not keep “awaiting decision”.

## 8.7 Deferred verdict

Must include:
- named condition;
- recheck trigger;
- owner.

No indefinite deferral.

## 8.8 Inline execution limits

Inline only when:
- bounded;
- no broad claims;
- no major migration;
- existing memo exact;
- focused verification known.

Large:
- queue package.

## 8.9 Source-of-truth updates

Update:
- memo;
- debt row;
- closeout;
- integration ledger;
- census if decision unblocks a plan.

Avoid mismatched verdict state.

## 8.10 Decision register artifact

Create/maintain one standing register doc.

It owns:
- current decision status;
- links to detailed memos.

It does not duplicate full memo content.

## 8.11 Recheck cadence

Propose:
- per wave;
- per release;
- on condition trigger.

Use project governance.

## 8.12 Verification

For inline executions:
- memo-specific focused gates;
- touched suites;
- build;
- verify-fast.

For doc-only verdicts:
- docs index;
- ledger consistency.

## 8.13 E1 non-goals

- no new policy choice by implementer;
- no execution without signature;
- no duplicate detailed memos;
- no silent indefinite blocker.

---

# 9. TASK F1 — E1 CORPUS PLAN IDENTITY & PREREQUISITE RESOLUTION

## 9.1 Objective

Fully classify `E1_planintegration.md`, whose title/dependencies were not verified in the earlier evidence pass. Resolve its identity, premise, DAG position, and first executable sub-plan if ready.

## 9.2 First action

Read E1 in full.

Capture:
- title;
- source baseline;
- dependencies;
- mandatory order;
- owners;
- acceptance;
- non-goals;
- decisions;
- current premise.

No speculation before reading.

## 9.3 Census reconciliation

Compare to A1’s provisional E1 row.

Correct:
- title;
- dependencies;
- status;
- queue rank.

Census reaches complete coverage only when E1 is fully classified.

## 9.4 Premise check

Search current source/closeouts for E1 scope.

Classify:
- SEALED-ELSEWHERE;
- PARTIALLY-SEALED;
- OPEN;
- STALE;
- SUPERSEDED.

## 9.5 DAG placement

Determine whether E1:
- heads separate chain;
- joins C1/C2;
- depends on D1;
- is governance/product layer.

Do not force into existing chain based on letter.

## 9.6 Ready execution

If open and dependencies sealed:
- execute first sub-plan only if bounded;
- follow plan order;
- claim exact paths;
- write implementation log.

If first sub-plan is broad:
- promote package rather than overexpand F1.

## 9.7 Dependency-blocked path

If blocked:
- exact prerequisite;
- current status;
- queue position.

No execution.

## 9.8 Stale/superseded path

Add status banner with evidence.

## 9.9 Verification

Any execution uses E1’s own acceptance.

Always:
- census updated;
- docs index;
- ledger updated;
- build/verify-fast if production/test work.

## 9.10 F1 non-goals

- no speculative identity;
- no execution before dependencies;
- no full E1 plan if first sub-plan alone is the bounded package.

---

# 10. CROSS-TASK SAVE / DETERMINISM CONTRACT

## 10.1 B3
Semantic/navigation/diagnostics mostly derived/presentation:
- no new save state by default;
- diagnostics read-only;
- replay fingerprint unchanged by viewing.

## 10.2 B4
Intel evaluation derived from current state:
- no duplicate persistent intel score;
- existing trust/forecast/follow-up save owners remain canonical.

## 10.3 B5
Delivery and storage are stateful:
- sink state persists;
- refusal does not create phantom output;
- save round-trip mandatory.

## 10.4 C1
Release/ship gates do not affect game save.

## 10.5 C2
Orchestration migration, if any, must preserve save behavior byte/semantic parity.

## 10.6 D1
Seven-day slice exercises existing save, not new schema.

## 10.7 E1/F1
Save impact depends only on executed signed/sub-plan work and must follow its memo/plan.

---

# 11. IMPLEMENTATION LOG STANDARD

Every newly executed corpus plan gets:

1. identity;
2. source baseline;
3. dependencies;
4. historical premise;
5. current premise;
6. clause matrix;
7. sealed-elsewhere evidence;
8. executed delta;
9. claims;
10. files;
11. save/determinism;
12. focused tests;
13. broader gates;
14. acceptance;
15. remainder;
16. census update;
17. next chain head.

No implementation log may simply say “done”.

---

# 12. CENSUS UPDATE STANDARD

After each task:

`Corpus key | Previous status | New status | Sealed clauses | Remaining clauses | Dependencies released | Queue rank change | Evidence`.

The census is a live queue source.

---

# 13. CLAIM DISCIPLINE

Before each task:
- read active claims;
- normalize exact paths;
- check parent/child overlap;
- check generated source/output overlap.

If conflict:
- wait for formal handoff;
- or choose disjoint sub-plan;
- never create alternate authority.

---

# 14. FOCUSED VERIFICATION POLICY

## B3
semantic/parity → navigation → diagnostics → Campaign → UI → replay.

## B4
intel consumer tests → Radio/World/Campaign → replay → UI.

## B5
producer-specific → producer-port gate → save → affected producer suites → integrity/utilization.

## C1
data-path parity → export smoke → perf → release gate.

## C2
lifecycle/manifest focused → triad gate → composition → replay.

## D1
slice scenario → N-repeat → save parity → human checklist → release battery.

## E1
per signed memo.

## F1
per E1 sub-plan.

---

# 15. COMMIT BOUNDARIES

## B3
1. clause reconciliation;
2. semantic delta if needed;
3. navigation;
4. diagnostics;
5. log/census.

## B4
1. premise/consumer audit;
2. one intel-axis delta per commit;
3. tests/surfaces;
4. log/census.

## B5
1. producer audit;
2. 35A delivery rails;
3. 36A gate;
4. 35B storage/spoilage;
5. 35C unlock;
6. logs/gates.

## C1
1. clause delta;
2. data parity;
3. smoke/perf/release delta;
4. log/census.

## C2
1. clause delta;
2. small lifecycle fixes;
3. promoted manifest proposal;
4. log/census.

## D1
1. slice definition;
2. scripted battery;
3. human checklist;
4. release wiring;
5. log/census.

## E1
1. register;
2. verdict session;
3. bounded executions individually;
4. queue/doc updates.

## F1
1. identity;
2. status/DAG;
3. first sub-plan if ready;
4. log/census.

---

# 16. ROLLBACK / ROUTING MATRIX

| Task | Finding | Action |
|---|---|---|
| B3 | Wave 9 B1 absent | execute semantic layer first |
| B3 | briefing file still claimed | split/defer navigation |
| B3 | diagnostics already complete | verified no-op |
| B4 | intel output has no named consumer semantics | decision route |
| B4 | reliability already complete | cite sealed |
| B5 | Plan 23 unsealed | dependency-blocked |
| B5 | sink missing | architecture decision; no new sink |
| B5 | success silently loses output | repair in owner before close |
| C1 | ship-gate clause already covered | cite sealed |
| C1 | data parity requires different source-of-truth model | promote decision/package |
| C2 | manifest migration repo-wide | promote separately |
| C2 | lifecycle delta changes behavior | rollback and isolate |
| D1 | required chain node open | dependency-blocked |
| D1 | human run finds unrelated bug | route finding |
| E1 | no verdict | remain unsigned; no execution |
| F1 | E1 dependency unknown | queue as dependency-blocked |

---

# 17. MASTER ACCEPTANCE MATRIX

| Task | Core blocker | Terminal proof |
|---|---|---|
| B3 | full Plan 31 remainder | three-axis clause matrix terminal; parity/navigation/diagnostics gates |
| B4 | intel value chain | every required intel output has consumer/decision evidence |
| B5 | producer delivery | no-silent-loss gate + save round-trips |
| C1 | ship-gate delta | data/export/perf/release deltas closed |
| C2 | orchestration delta | small deltas closed, large migration promoted |
| D1 | missing standing product proof | 7-day scripted + human slice integrated |
| E1 | scattered unsigned decisions | zero unsigned-without-condition |
| F1 | unknown E1 corpus identity | E1 classified, DAG position resolved |

---

# 18. PART 2 CLOSEOUT

Before Wave 10 closes:

1. B3 Plan 31 full contract reconciled.
2. B4 intel chain terminal or explicitly blocked.
3. B5 delivery chain terminal or dependency-blocked with exact prerequisite.
4. C2[8] ship-gate delta logged.
5. C2[9] orchestration delta logged.
6. D1 slice either delivered or dependency-blocked.
7. Decision register current.
8. E1 corpus identity resolved.
9. Census updated after every package.
10. Claim table truthful.
11. Integration ledger current.
12. Implementation logs present.
13. Build green.
14. Verify-fast green.
15. Data integrity/utilization green for affected content/data.
16. Save battery green where B5/D1 require.
17. Replay/fingerprint gates green.
18. Docs index/generator checks green.
19. Queue reranked.
20. Wave 11 generated only from current queue.

---

# 19. PART 2 NON-GOALS

- no automatic execution of C1[11]–[45] / C2[10]–[45];
- no parallel briefing authority;
- no second intel/reliability system;
- no new production sink/resource currency;
- no content expansion before delivery rails;
- no CI/release rewrite beyond C2[8] delta;
- no big-bang orchestration manifest migration;
- no new systems for seven-day slice;
- no unsigned decision execution;
- no speculative E1 identity.




# APPENDIX A — B3 PLAN 31 CONTROL SHEETS

## A.1 Full-plan reconciliation table

| Clause | Wave 9 B1 status | Current source | Claim state | B3 action | Test | Final |
|---|---|---|---|---|---|---|
| semantic kinds |  |  |  |  |  |  |
| navigable briefings |  |  |  |  |  |  |
| replayable diagnostics |  |  |  |  |  |  |

## A.2 Semantic totality checklist
- [ ] all registered event IDs mapped;
- [ ] one kind per ID;
- [ ] loud failure for unmapped ID;
- [ ] parity matrix updated/generated;
- [ ] no save;
- [ ] no RNG;
- [ ] representative stability tests.

## A.3 Navigation checklist
- [ ] typed route metadata;
- [ ] no localization parsing;
- [ ] valid target opens;
- [ ] missing target honest;
- [ ] back path;
- [ ] focus return;
- [ ] keyboard;
- [ ] a11y;
- [ ] lifecycle;
- [ ] snapshot.

## A.4 Diagnostic checklist
- [ ] plan-required trace fields;
- [ ] stable ordering;
- [ ] semantic kind included where required;
- [ ] source owner provenance;
- [ ] deterministic replay;
- [ ] save/reload suffix if required;
- [ ] viewing does not mutate state.

## A.5 Claim split
**Briefing path:**<br>
**Claim owner:**<br>
**Handoff condition:**<br>
**Safe B3 subset:**<br>
**Deferred exact clause:**<br>

## A.6 Negative cases
- unmapped event ID;
- briefing entry target removed;
- target panel unavailable;
- localized text changed;
- replay opened twice;
- empty-day trace;
- save/restore between event generation and diagnostic view.

---

# APPENDIX B — B4 INTEL-VALUE CONTROL SHEETS

## B.1 Intel chain inventory

| Intel output | Owner | Current surface | Consumer | Player decision affected | Dead? | Action |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## B.2 Value-proof test pattern

For each intel output:
1. construct same authoritative state;
2. case A exposes intel;
3. case B withholds intel through canonical availability;
4. inspect available action/context/read model;
5. prove a meaningful difference;
6. do not automate the player decision itself.

## B.3 Forecast reliability checklist
- [ ] current 20C owner reused;
- [ ] confidence semantics unchanged;
- [ ] radio copy reflects uncertainty;
- [ ] no second calculation;
- [ ] deterministic.

## B.4 Distress intel checklist
- [ ] trust owner reused;
- [ ] follow-up owner reused;
- [ ] signal selection owner reused;
- [ ] no duplicate ledger;
- [ ] dead consumer eliminated or retired.

## B.5 Comms checklist
- [ ] Plan 173 infrastructure mapped;
- [ ] channels/programs current;
- [ ] historical C1[9] clause compared;
- [ ] only remainder executed.

## B.6 Negative cases
- intel exists but surface hidden;
- intel shown but no actionable context;
- reliability missing;
- reliability stale;
- consumer reads display text rather than typed state;
- panel open mutates intel state;
- no-signal/no-forecast state.

---

# APPENDIX C — B5 DELIVERY CONTROL SHEETS

## C.1 Producer audit template

| Producer | Output | Preconditions | Sink | Current success | Persisted | Failure result | Silent-loss risk |
|---|---|---|---|---|---|---|---|
| greenhouse |  |  |  |  |  |  |  |
| kitchen |  |  |  |  |  |  |  |
| workshop |  |  |  |  |  |  |  |
| foundry |  |  |  |  |  |  |  |
| trapping |  |  |  |  |  |  |  |
| compost |  |  |  |  |  |  |  |
| medical |  |  |  |  |  |  |  |

## C.2 Delivery transaction checklist
- [ ] preflight canonical;
- [ ] input cost timing explicit;
- [ ] sink commit canonical;
- [ ] refusal typed;
- [ ] no silent success;
- [ ] no duplicate output;
- [ ] save state authoritative;
- [ ] event/report accurate.

## C.3 Producer-port gate checklist
- [ ] exact Plan 36 contract read;
- [ ] shared contract/interface;
- [ ] all registered producers enumerated;
- [ ] gate catches unmapped producer;
- [ ] no ad hoc delegates;
- [ ] stable registration.

## C.4 Storage/spoilage checklist
- [ ] storage owner;
- [ ] capacity;
- [ ] item age/state;
- [ ] spoilage owner;
- [ ] save/restore;
- [ ] no producer-side duplicate spoilage.

## C.5 Unlock checklist
- [ ] research owner;
- [ ] locked refusal;
- [ ] unlocked path;
- [ ] content separate;
- [ ] old save behavior.

## C.6 Negative cases
- full storage;
- missing power;
- missing labour;
- locked research;
- invalid recipe/output;
- sink unavailable;
- save immediately after delivery;
- restore and spoil;
- repeated producer tick;
- producer success event but sink no change.

---

# APPENDIX D — C2[8] SHIP GATE CONTROL SHEETS

## D.1 Delta table

| Axis | Historical contract | Current implementation | Prior wave | Delta | Gate |
|---|---|---|---|---|---|
| single data path |  |  |  |  |  |
| export smoke |  |  |  |  |  |
| perf budgets |  |  |  |  |  |
| ship gate |  |  |  |  |  |

## D.2 Data authority checklist
- [ ] canonical repo Data path;
- [ ] export copy process;
- [ ] deterministic manifest/checksum;
- [ ] mismatch fails release;
- [ ] no runtime dependence on dev path.

## D.3 Export smoke checklist
- [ ] real exported binary;
- [ ] real exported Data;
- [ ] production composition root;
- [ ] meaningful boot acceptance;
- [ ] no mock fallback;
- [ ] clean exit/shutdown status.

## D.4 Performance checklist
- [ ] exact plan axes;
- [ ] current baseline measured;
- [ ] signed/existing thresholds;
- [ ] reproducible harness;
- [ ] failure blocks release if plan requires.

## D.5 Release gate checklist
- [ ] one canonical release checklist;
- [ ] all required current gates;
- [ ] no stale gate refs;
- [ ] artifact failure explicit.

---

# APPENDIX E — C2[9] ORCHESTRATION CONTROL SHEETS

## E.1 Current orchestration inventory

| Lifecycle/concern | Current owner | Registry/manifest? | Scattered paths | Gate | Delta |
|---|---|---|---|---|---|
| construct |  |  |  |  |  |
| setup |  |  |  |  |  |
| tick |  |  |  |  |  |
| save |  |  |  |  |  |
| restore |  |  |  |  |  |
| flush |  |  |  |  |  |
| dispose |  |  |  |  |  |

## E.2 Manifest-migration promotion packet

If large:
**Current scatter:**<br>
**Target manifest owner:**<br>
**Subsystem count:**<br>
**Migration phases:**<br>
**Parity gates:**<br>
**Rollback:**<br>
**Claims:**<br>
**Why not inline:**<br>

## E.3 Behavior-preservation checklist
- [ ] pre-fingerprint;
- [ ] triad gate;
- [ ] ordering;
- [ ] save parity;
- [ ] post-fingerprint;
- [ ] no feature change.

## E.4 Negative cases
- subsystem in manifest but not constructed;
- constructed but not saved;
- saved but not restored;
- flush missing;
- duplicate construction;
- order changes;
- active claim path.

---

# APPENDIX F — D1 SEVEN-DAY SLICE CONTROL SHEETS

## F.1 Slice dependency table

| Required system/plan | Status | Evidence | Blocks slice? |
|---|---|---|---|
|  |  |  |  |

## F.2 Seven-day scripted oracle

| Day | Required loop | Action/trigger | State assertion | Event/briefing | Save? | Result |
|---:|---|---|---|---|---|---|
| 1 |  |  |  |  |  |  |
| 2 |  |  |  |  |  |  |
| 3 |  |  |  |  |  |  |
| 4 |  |  |  |  |  |  |
| 5 |  |  |  |  |  |  |
| 6 |  |  |  |  |  |  |
| 7 |  |  |  |  |  |  |

## F.3 Scripted slice rules
- authority-backed data;
- real composition;
- seeded;
- per-day assertions;
- no incidental log pins;
- save/restore if required;
- day-7 fingerprint;
- N-run stability.

## F.4 Human checklist format

**Build/version:**<br>
**Tester:**<br>
**Date:**<br>

For each day:
- intended objective;
- actual player actions;
- blocking bug?;
- confusing UX?;
- balance note?;
- content gap?;
- screenshot/evidence if required.

## F.5 Finding disposition
`Finding | Severity | Type | Owner | Repro | Blocks slice? | Package`.

## F.6 Release cadence
- scripted slice cadence:
- human slice cadence:
- evidence retention:
- failure escalation:

---

# APPENDIX G — E1 DECISION REGISTER CONTROL SHEETS

## G.1 Register table

| ID | Topic | Memo | Owner | Routed | Verdict | Condition | Execution | Status |
|---|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

## G.2 Verdict semantics

### SIGNED
Implementation authorized per memo.

### DECLINED
No implementation; blocker closed with reason.

### DEFERRED-WITH-CONDITION
No implementation until named condition.

### UNSIGNED
Still blocks; must appear in session.

## G.3 Decision-session checklist
- [ ] memos current;
- [ ] current evidence refreshed;
- [ ] options unchanged or premise update called out;
- [ ] verdict recorded;
- [ ] signer/date;
- [ ] condition if deferred.

## G.4 Inline execution checklist
- [ ] bounded;
- [ ] exact memo;
- [ ] claims clear;
- [ ] focused gates;
- [ ] source docs updated.

## G.5 Queue execution checklist
- [ ] package row;
- [ ] exact paths;
- [ ] acceptance;
- [ ] dependencies;
- [ ] priority.

## G.6 Register success
Zero `UNSIGNED` without explicit pending-session status and owner; zero `DEFERRED` without condition.

---

# APPENDIX H — F1 E1 CORPUS PLAN CONTROL SHEETS

## H.1 Identity packet

**File:**<br>
**Title:**<br>
**Source baseline:**<br>
**Dependencies:**<br>
**Order:**<br>
**Acceptance:**<br>
**Owners:**<br>
**Non-goals:**<br>

## H.2 Current-state matrix

| Clause | Current implementation | Evidence | Status | Remainder |
|---|---|---|---|---|
|  |  |  |  |  |

## H.3 DAG placement
**Chain:**<br>
**Prerequisites:**<br>
**Dependents:**<br>
**Ready?:**<br>
**Rank:**<br>

## H.4 Execution rule
Only first bounded sub-plan if all dependencies sealed.

---

# APPENDIX I — IMPLEMENTATION LOG TEMPLATE

# `<CORPUS KEY>` IMPLEMENTATION LOG

## Identity
**Plan file:**<br>
**Title:**<br>
**Source baseline:**<br>

## Dependencies
| Dependency | Status | Evidence |
|---|---|---|
|  |  |  |

## Premise reconciliation
| Historical premise | Current truth | Consequence |
|---|---|---|
|  |  |  |

## Clause matrix
| Clause | Sealed elsewhere | Executed here | Remainder | Evidence |
|---|---|---|---|---|
|  |  |  |  |  |

## Claims
**Paths:**<br>
**Owner:**<br>
**Handoff:**<br>

## Files changed
-

## Save/determinism
-

## Verification
| Command | Purpose | Result |
|---|---|---|
|  |  |  |

## Acceptance
-

## Final status
**Status:**<br>
**Remaining blocker:**<br>
**Census update:**<br>
**Next chain node:**<br>

---

# APPENDIX J — NEGATIVE-CASE REGISTRY

## J.1 B3
- Wave 9 B1 absent;
- semantic ID unmapped;
- briefing target missing;
- briefing target unavailable;
- localized text altered;
- diagnostics opened twice;
- empty day trace;
- save/restore trace suffix;
- Plan 24 claim active.

## J.2 B4
- intel output with no consumer;
- consumer with stale data;
- reliability unknown;
- no-signal state;
- no-forecast state;
- UI hidden;
- same intel read repeatedly;
- trust/follow-up state independently restored.

## J.3 B5
- full storage;
- missing power;
- missing labour;
- locked research;
- sink failure;
- save after delivery;
- restore;
- spoilage;
- duplicate day tick;
- producer unregistered in gate.

## J.4 C1
- exported Data mismatch;
- export missing Data;
- binary boot fallback/mock;
- perf breach;
- missing required release gate.

## J.5 C2
- subsystem missing from registry;
- duplicate construction;
- setup order drift;
- save missing;
- restore missing;
- flush missing;
- manifest migration too broad.

## J.6 D1
- dependency open;
- day transition fails;
- save midpoint mismatch;
- repeated run fingerprint mismatch;
- human blocker found.

## J.7 E1
- signed verdict with stale memo;
- declined but source doc still says pending;
- deferred without condition;
- large execution attempted inline.

## J.8 F1
- E1 file title not parseable;
- dependency collision;
- stale premise;
- broad first sub-plan.

---

# APPENDIX K — WAVE 11 PROMOTION FILTER

Wave 11 must consume the **post-Part-2 census**, not the raw source corpus.

For every ready node:
- current status;
- current remainder;
- dependencies;
- active claims;
- decisions;
- package boundedness;
- downstream unlock;
- acceptance.

Potential corpus families mentioned by the source ledger—calendar authority, memory/leadership/content pipeline, mod-contract, world politics, balance evidence, radiation economics, personal quests, photography—are **candidates only**. They enter Wave 11 only when their DAG prerequisites are sealed and current premise survives re-verification.

Queue row:

| Rank | Corpus key | Title | Remainder | Dependencies | Claims | Decision | Downstream | Acceptance |
|---:|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

No “next numeric file” scheduling.

---

# APPENDIX L — PART 2 RELEASE REVIEW

## L.1 Architecture review
- one briefing authority;
- one intel owner per state;
- one sink authority;
- one release battery;
- one orchestration source per lifecycle dimension;
- one decision register.

## L.2 Save review
- B5 sink round-trip;
- D1 slice save parity;
- no unnecessary B3/B4 derived-state persistence.

## L.3 Determinism review
- B3 diagnostics read-only;
- B4 intel derived;
- B5 production seeded/stable;
- D1 N-run stable.

## L.4 Governance review
- claims truthful;
- census current;
- ledger current;
- decision register terminal/conditioned.

## L.5 No-false-closure review
A task is not closed if:
- plan clause remains unclassified;
- sealed-elsewhere evidence absent;
- dependency assumed;
- claim ignored;
- implementation log missing;
- census not updated.

---

# APPENDIX M — FINAL PART 2 CLOSEOUT TABLE

| Task | Start status | End status | Work executed | Sealed elsewhere | Remaining blocker | Next node |
|---|---|---|---|---|---|---|
| B3 C1[8] |  |  |  |  |  |  |
| B4 C1[9] |  |  |  |  |  |  |
| B5 C1[10] |  |  |  |  |  |  |
| C1 C2[8] |  |  |  |  |  |  |
| C2 C2[9] |  |  |  |  |  |  |
| D1 slice |  |  |  |  |  |  |
| E1 register |  |  |  |  |  |  |
| F1 E1 plan |  |  |  |  |  |  |

**End of Wave 10 Part 2 implementation-unblocker plan.**

# APPENDIX N — B3 NAVIGABLE-BRIEFING EXECUTION PACKET

## N.1 Current briefing surface audit

Before edits, capture:

| Concern | Current implementation | Owner/path | Claim state | Plan 31 expectation | Delta |
|---|---|---|---|---|---|
| entry assembly |  |  |  |  |  |
| semantic kind |  |  |  |  |  |
| route metadata |  |  |  |  |  |
| drill-down |  |  |  |  |  |
| back navigation |  |  |  |  |  |
| focus return |  |  |  |  |  |
| diagnostics link |  |  |  |  |  |

Do not infer navigation by whether a panel happens to be adjacent in the UI.

## N.2 Navigation-target design constraints

A valid target:
- is based on stable route/entity IDs;
- resolves through the current routing owner;
- can represent “no navigable source” when appropriate;
- does not depend on localized entry text;
- does not carry domain mutation.

The builder may attach metadata, but the route system remains authoritative for opening.

## N.3 Provenance

If Plan 31 requires “event layer speaks” provenance:
- entry identifies its originating semantic event or source owner;
- diagnostics can use the same provenance;
- do not duplicate event payload into a separate briefing database.

## N.4 Drill-down states

Test at least:
- source panel exists and route enabled;
- route exists but entity/detail no longer valid;
- event refers to historical/dead entity;
- target is intentionally non-navigable;
- route locked by current gameplay;
- back returns to prior briefing focus.

Use current route failure semantics.

## N.5 Accessibility

Navigation affordance:
- discoverable by keyboard;
- focusable;
- target label text available;
- no color-only “clickable” distinction;
- screen-reader/accessibility text uses existing UI contract if applicable.

## N.6 Snapshot review

Snapshot change is intentional only when:
- navigation affordance appears;
- target indicator/entry layout changes.

Do not rebaseline unrelated text wrapping without investigation.

---

# APPENDIX O — B3 REPLAYABLE-DIAGNOSTICS EXECUTION PACKET

## O.1 Trace-source audit

Identify current replay/debug data:

| Trace component | Exists? | Owner | Persisted? | Deterministic? | Plan 31 need |
|---|---|---|---|---|---|
| day events |  |  |  |  |  |
| event semantic kind |  |  |  |  |  |
| source subsystem |  |  |  |  |  |
| entity/source ID |  |  |  |  |  |
| state fingerprint |  |  |  |  |  |
| route target |  |  |  |  |  |

## O.2 Diagnostics non-authority rule

Diagnostics may:
- collect;
- classify;
- display;
- replay existing traces.

Diagnostics may not:
- decide domain outcomes;
- mutate day-event state;
- rerun simulation logic to “recreate” events if a canonical trace exists;
- become a save authority for campaign state.

## O.3 Continuous vs replay comparison

For a seeded day:
1. run normally;
2. capture canonical trace;
3. replay/diagnose from recorded inputs or harness;
4. compare ordered semantic events;
5. compare fingerprint;
6. verify no extra event emission due to diagnostic read.

## O.4 Save/reload case

If replay harness supports mid-reload:
- run days 1–N;
- save;
- continue;
- restore;
- rerun same suffix;
- compare diagnostic trace.

## O.5 Empty/partial trace behavior

Diagnostics must handle:
- no day events;
- event with no navigation target;
- event whose entity was removed;
- unknown historical semantic kind if backward compatibility exists.

Do not crash or silently fabricate.

---

# APPENDIX P — B4 INTEL CONSUMER CHAIN PACKETS

## P.1 Distress intel

Audit:
- detection event;
- trust effect;
- signal content;
- follow-up;
- player-visible action;
- downstream consequence.

Questions:
- Does knowing signal origin/trust meaningfully change available action context?
- Are trap/ignored consequences surfaced before/after decision as intended?
- Does intel simply exist as flavor with no state consumer?

If dead:
- follow plan’s consumer;
- otherwise decision-route.

## P.2 Weather intel

Audit:
`Weather authority -> forecast model -> reliability -> radio/panel surface -> player planning context`.

Acceptance can include:
- expedition risk display changes;
- shelter preparation information;
- timing of action;
only if C1[9] explicitly names or current system already consumes these.

Do not make forecasts directly change weather.

## P.3 Trace intel

If the plan treats traces as world intel:
- verify trace data reaches a planning/inspection surface;
- verify stale/expired trace semantics;
- verify no duplicate world-state authority.

## P.4 Decision measurability table

| Intel | With intel | Without intel | Decision context difference | Domain outcome only after action? | Test |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## P.5 Reliability language

Player-facing copy should match current model:
- certain;
- likely;
- uncertain;
or whatever exact vocabulary exists.

No raw percentages unless current contract uses them.

## P.6 Dead-consumer disposition

For each dead output:
- wire;
- retire;
- defer with decision.

No permanent dead output accepted as “future”.

---

# APPENDIX Q — B5 PRODUCER-PORT GATE DESIGN

## Q.1 Purpose

The gate prevents newly added or existing producers from reporting successful production without a canonical sink commit/refusal contract.

## Q.2 Registration source

Use existing producer registration/manifest if available.

If no shared registry exists, follow exact Plan 36 design before introducing one.

## Q.3 Gate contract per producer

The gate must prove:

1. producer is discoverable;
2. output types map to canonical sink;
3. success reaches sink;
4. refusal is typed;
5. save/restore preserves sink state;
6. no silent-loss path.

## Q.4 Gate test data

Use authority-backed fixture:
- valid input;
- deterministic day;
- sink with capacity;
- sink blocked/full variant.

## Q.5 Failure reporting

Gate failure identifies:
- producer ID/type;
- output;
- expected sink;
- observed result;
- missing refusal if any.

No generic “producer failed” only.

## Q.6 New producer onboarding

Future producer must:
- register;
- map output to sink;
- pass gate.

This is the permanent regression protection promised by Plan 35/36.

---

# APPENDIX R — B5 DELIVERY ATOMICITY PACKET

## R.1 Delivery transaction matrix

| Producer | Inputs consumed when | Output computed when | Sink commit | Rollback path | Success event | Failure result |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## R.2 Failure ordering

If sink can reject:
- ideally preflight before destructive input consumption;
- if transaction API supports reservation/rollback, use it;
- never report success before sink commit.

## R.3 Partial output

If producer can produce variable quantity:
- sink acceptance semantics explicit;
- full vs partial commit follows current owner policy;
- no hidden truncation.

## R.4 Storage full

Expected:
- typed refusal or allowed partial according to contract;
- no vanishing goods;
- no false journal line.

## R.5 Power loss mid-production

Use current production cadence:
- if production resolves atomically at day tick, evaluate power at that authoritative point;
- do not invent mid-frame interruption semantics.

## R.6 Labour loss

Use Plan 24 current worker-hour/assignment authority if dependency landed.

No B5 local workforce list.

## R.7 Save boundary

Test:
- immediately before production;
- immediately after successful delivery;
- immediately after refusal if refusal affects any producer state;
- restore and continue.

---

# APPENDIX S — B5 STORAGE / SPOILAGE PACKET

## S.1 Ownership

Storage owns:
- capacity;
- stored quantities;
- item state where current model assigns it.

Spoilage owner owns:
- age/decay;
- spoil transition.

Producer owns neither.

## S.2 Delivery-to-spoilage journey

1. produce item;
2. sink commit;
3. save;
4. restore;
5. advance spoilage interval;
6. assert expected decay/spoil state;
7. ensure producer does not re-add lost/spoiled item.

## S.3 Inventory identity

If batches carry metadata:
- preserve required identity/quality/condition;
- do not collapse metadata silently unless canonical inventory does so.

## S.4 Refusal with full storage

Assert:
- no output disappears;
- producer result truthful;
- any consumed input policy matches signed/current contract.

---

# APPENDIX T — C2[8] DATA-PATH PARITY PACKET

## T.1 Authority model

Distinguish:
- **authoritative source data** in repo;
- **exported deployment copy** in artifact;
- **runtime read location** in shipped build.

Single data authority means the exported copy is derived from canonical source.

## T.2 Parity manifest

Preferred artifact:
`relative_path | source_hash | export_hash | size | status`.

Use project-supported hashing.

## T.3 Gate failure cases

- missing export file;
- extra stale export file where plan forbids;
- hash mismatch;
- copy from wrong source path;
- generated data not regenerated;
- case-sensitivity/path issue on target platform.

## T.4 Release integration

Data parity runs:
- during export/packaging;
- or release verification;
according to plan.

Do not add expensive whole-repo hashing to every fast unit run unless required.

---

# APPENDIX U — C2[8] REAL EXPORT SMOKE PACKET

## U.1 Artifact identity

Record:
- build/export ID;
- target platform;
- binary path;
- data path;
- build commit.

## U.2 Smoke acceptance

Plan-specific, likely:
- process starts;
- canonical composition root initializes;
- required data loads;
- selected selftest/boot state reached;
- no fallback/demo path;
- exit status correct.

## U.3 Failure injection

Where harness supports:
- remove/corrupt data copy;
- assert smoke fails;
- confirm gate detects real artifact defect.

## U.4 Environment parity

Use release-like environment:
- no editor-only dependencies;
- no source-tree assumptions;
- no developer cache dependency.

---

# APPENDIX V — C2[8] PERFORMANCE-BUDGET PACKET

## V.1 Budget row

| Axis | Current harness | Current baseline | Threshold authority | Gate exists? | Delta |
|---|---|---:|---|---|---|
| startup |  |  |  |  |  |
| save |  |  |  |  |  |
| load |  |  |  |  |  |
| update/tick |  |  |  |  |  |
| content load |  |  |  |  |  |

Use exact plan axes only.

## V.2 Threshold rule

Never invent threshold from one machine run.

Use:
- plan-defined;
- signed;
- existing CI threshold.

If absent, decision-route/benchmark package.

## V.3 Noise handling

Performance gate needs stable harness:
- warmup if current policy;
- bounded variance;
- same environment.

Do not weaken gate merely because local workstation noisy.

---

# APPENDIX W — C2[9] MANIFEST MIGRATION PROMOTION PLAN

If census confirms a large manifest delta, create a separate package with:

## W.1 Goal
Move subsystem construction/registration toward one declarative source without behavior change.

## W.2 Inventory
All current scattered construction/setup sites.

## W.3 Target manifest
Exact owner/schema.

## W.4 Migration phases
1. read-only inventory;
2. dual representation with parity check if project permits;
3. migrate bounded subsystem family;
4. remove old setup;
5. repeat;
6. final single authority.

## W.5 Parity gates
- construction count;
- setup order;
- save/restore;
- flush/dispose;
- replay fingerprint;
- test suites.

## W.6 Stop conditions
- behavior drift;
- active claim;
- lifecycle ambiguity;
- circular dependency.

C2 itself should not perform this full migration.

---

# APPENDIX X — D1 SEVEN-DAY SCRIPTED SCENARIO TEMPLATE

# Seven-Day Slice — Scripted

## Initial state
**Seed:**<br>
**Build:**<br>
**Roster:**<br>
**Shelter state:**<br>
**World state:**<br>
**Starting resources:**<br>

## Day 1
**Required loop:**<br>
**Action:**<br>
**Expected authoritative state:**<br>
**Player-visible evidence:**<br>
**Fingerprint:**<br>

## Day 2
Same fields.

## Day 3
Same fields.

## Day 4
Include save/restore midpoint if contract.

## Day 5
Same fields.

## Day 6
Same fields.

## Day 7
**Terminal state:**<br>
**Core-loop completeness:**<br>
**Fingerprint:**<br>
**Acceptance:**<br>

## Repeat table

| Run | Seed | Day-7 fingerprint | Duration | Result |
|---:|---|---|---:|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

---

# APPENDIX Y — D1 HUMAN PLAYTHROUGH TEMPLATE

## Build and environment
**Commit/build:**<br>
**Platform:**<br>
**Tester:**<br>
**Date:**<br>

## Per-day checklist

For each day:
- Can player identify immediate priorities?
- Can player act on needs/production/intel?
- Are consequences visible?
- Does day transition complete?
- Are blockers encountered?
- Is any system silently failing?
- Are critical UI routes accessible?
- Is save/load viable where required?

## Finding record

`Day | Finding | Severity | Repro | Owner | Blocks release? | Package`.

Human playthrough does not replace scripted assertions.

---

# APPENDIX Z — E1 DECISION REGISTER EXECUTION ORACLES

## Z.1 Signed

A signed item is not terminal until:
- execution completed, or
- queued with exact package if too large;
- owning docs updated.

## Z.2 Declined

Terminal when:
- reason recorded;
- blocker state changed from waiting;
- no code change.

## Z.3 Deferred

Valid only with:
- named condition;
- recheck event/date/wave;
- owner.

## Z.4 Register consistency

No item may have:
- SIGNED in memo but UNSIGNED in register;
- DECLINED in closeout but OPEN in debt;
- DEFERRED without condition.

## Z.5 Decision execution package

For large signed item:
`Decision ID | Memo | Chosen option | Dependencies | Claims | Execution package | Acceptance | Queue rank`.

---

# APPENDIX AA — F1 E1 CORPUS IDENTITY ORACLE

F1 is complete only when:

- E1 title read exactly;
- source baseline identified;
- dependencies identified;
- mandatory order identified;
- current premise checked;
- status assigned;
- DAG position assigned;
- census row complete;
- first sub-plan executed only if ready/bounded;
- implementation log/queue/banner created as appropriate.

“E1 exists” is not a classification.

---

# APPENDIX AB — PART 2 NEGATIVE-CASE REVIEW

## AB.1 B3
- semantic layer missing despite Wave 9 note;
- navigation route target stale;
- diagnostics trace nondeterministic;
- briefing builder still claimed.

## AB.2 B4
- output exists but no action context;
- reliability duplicated;
- comms already sealed;
- dead intel output requiring product decision.

## AB.3 B5
- producer success with no sink mutation;
- sink full;
- research locked;
- power unavailable;
- labour unavailable;
- save loses delivered item;
- spoilage double-applied.

## AB.4 C1
- source/export data drift;
- artifact boot relies on source tree;
- perf gate missing authority;
- release checklist duplicates gate list.

## AB.5 C2
- scattered construction beyond bounded delta;
- manifest would change order;
- lifecycle gate incomplete;
- duplicate subsystem creation.

## AB.6 D1
- required chain dependency not sealed;
- scripted run green but human loop blocked;
- human run green but scripted fingerprint flaky.

## AB.7 E1
- unsigned item accidentally executed;
- deferred item no condition;
- decision source docs disagree.

## AB.8 F1
- E1 scope mistaken due header parsing;
- dependency cycle;
- first sub-plan too large.

---

# APPENDIX AC — HANDOFF TEMPLATE

**Task:**<br>
**Corpus key:**<br>
**Plan title:**<br>
**Current HEAD:**<br>
**Consumed census row:**<br>
**Dependencies:**<br>
**Claims:**<br>
**Historical premise:**<br>
**Current premise:**<br>
**Sealed elsewhere:**<br>
**Executed delta:**<br>
**Files:**<br>
**Save impact:**<br>
**Determinism impact:**<br>
**Focused tests:**<br>
**Owning suites:**<br>
**Selftests:**<br>
**Build:**<br>
**Verify-fast:**<br>
**Generators/docs:**<br>
**Implementation log:**<br>
**Census status after:**<br>
**Ledger status after:**<br>
**Remaining blocker:**<br>
**Next DAG node:**<br>

---

# APPENDIX AD — WAVE 11 INPUT TABLE

| Rank | Corpus | Title | Status | Current remainder | Dependencies | Claims | Decisions | Acceptance |
|---:|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

Wave 11 planner must re-verify top rows before generating tasks.

Do not blindly include all C1[11]–[45]/C2[10]–[45]. The source ledger explicitly says each enters as prerequisites seal.

---

# APPENDIX AE — FINAL QUALITY BAR

Wave 10 Part 2 is complete only when:

- B3 reconciles the **full** Plan 31 contract, not only Wave 9’s semantic subset;
- B4 proves intel has a real consumer/value chain rather than decorative presence;
- B5 establishes a permanent no-silent-loss producer delivery gate;
- C2[8] has one coherent ship/release gate view with current deltas closed;
- C2[9] does not hide a big-bang migration inside a “small delta” task;
- D1 provides actual product-level seven-day evidence or is honestly dependency-blocked;
- E1 decision register has no unsigned-without-condition ambiguity after the review pass;
- F1 resolves E1 corpus identity and DAG position;
- every executed corpus plan has an implementation log;
- census and integration ledger match current reality;
- the Wave 11 queue is derived from the latest DAG rather than file numbering.

The success criterion remains the same as Part 1: **truthful executable state**. A smaller verified delta is better than a large implementation built from a stale premise.

# APPENDIX AF — B5 PRODUCER-FAMILY VERIFICATION MATRIX

This matrix is intentionally repetitive at the **behavioral contract** level because the core risk is that one producer family silently bypasses the delivery rails while another correctly uses them.

## AF.1 Greenhouse family

### Premise
Verify current greenhouse output, labour/power dependencies, and inventory/storage sink.

### Success case
- all prerequisites valid;
- output computed;
- canonical sink accepts;
- authoritative inventory/storage changes;
- producer reports success;
- save/restore preserves result.

### Failure cases
- no labour if required;
- no power if required;
- storage full;
- invalid/missing recipe/content;
- sink unavailable.

### Forbidden
- panel-local grant;
- private greenhouse stock;
- output discarded after success.

### Evidence
`Command/tick -> output -> sink API -> persisted state -> UI/result`.

## AF.2 Kitchen family

Audit:
- inputs;
- recipe/meal authority;
- produced meal/needs effect;
- storage/direct-consumption semantics;
- labour;
- power/fuel.

Special check:
If kitchen production affects meal quality rather than inventory item, identify canonical sink explicitly. “Sink” may be a needs/meal-state authority rather than inventory.

No B5 redesign of nutrition/needs formulas.

## AF.3 Workshop/foundry family

Audit:
- bill/recipe authority;
- material consume;
- power;
- labour/skill;
- item output;
- canonical inventory sink.

Failure:
- insufficient materials;
- no power;
- storage capacity;
- unavailable recipe.

No rival bill framework.

## AF.4 Trapping family

Audit:
- trap resolution;
- prey/bycatch;
- contamination/disease payload if current mechanics;
- inventory/food sink;
- no duplicate catch.

B5 ensures delivered output reaches sink; it does not rebalance catch chances.

## AF.5 Compost family

Identify output:
- fertilizer/soil modifier/material;
- canonical sink.

Ensure output is not only logged.

No new farming currency.

## AF.6 Medical production family

If current medical system produces:
- medicine;
- treatment supplies;
- crafted medical items.

Map to canonical medical/inventory sink.

Do not change diagnosis/treatment semantics.

## AF.7 Producer-family closure table

| Family | Success test | Failure test | Save test | Port gate | UI/refusal | Final |
|---|---|---|---|---|---|---|
| greenhouse |  |  |  |  |  |  |
| kitchen |  |  |  |  |  |  |
| workshop |  |  |  |  |  |  |
| foundry |  |  |  |  |  |  |
| trapping |  |  |  |  |  |  |
| compost |  |  |  |  |  |  |
| medical |  |  |  |  |  |  |

A producer family cannot be marked closed on the basis of another family’s passing gate.

---

# APPENDIX AG — B5 NO-SILENT-LOSS PERMANENT GATE

## AG.1 What the gate protects

The gate exists so future producers cannot:
- report success without sink effect;
- produce an output with no registered sink;
- lose output when capacity blocks delivery without returning refusal;
- skip persistence contract.

## AG.2 Candidate implementation patterns

Use whichever pattern the current architecture supports:
- enumerate producer registry;
- shared producer-port interface;
- generated producer manifest;
- contract tests per registered producer.

Do not introduce reflection-heavy magic solely for the gate if the repo uses explicit registration.

## AG.3 Gate failure examples

**Unmapped output**
> Producer `x` emitted output type `y`, but no authoritative sink mapping exists.

**False success**
> Producer `x` returned success; sink state did not change.

**Missing refusal**
> Sink rejected output; producer result lacks refusal reason.

**Persistence mismatch**
> Output present before save but absent after restore.

## AG.4 Gate stability

The gate must fail when a new producer is added without delivery contract. This is intentional friction.

## AG.5 Gate scope

Do not require every historical/non-producing system to implement producer port. Use current plan’s definition of producer.

---

# APPENDIX AH — B4 INTEL-VALUE ORACLE LIBRARY

## AH.1 Forecast-to-expedition oracle

Only if current C1[9] contract connects forecast to expedition planning:

- same world state;
- with forecast, expedition preparation/read model shows weather risk context;
- without forecast, that context absent/less informed;
- expedition domain state does not change until player acts.

## AH.2 Distress-to-response oracle

- signal intel arrives;
- available response options reflect canonical signal state;
- trust/follow-up history may alter context if current contract says so;
- merely reading signal does not mutate trust unless existing policy explicitly does.

## AH.3 Trace-to-world oracle

If traces inform location/world decision:
- trace appears;
- target/location relevance resolves;
- route/planning context changes;
- no hidden world-state mutation from inspection.

## AH.4 Reliability oracle

- reliable and unreliable forecast/signals present different confidence/context according to current model;
- no second confidence score.

## AH.5 Intel expiry

If current plan has expiry:
- expired intel is clearly stale/unavailable;
- consumer does not treat as fresh;
- no silent use.

## AH.6 Save/reload

If intel state persists:
- save;
- restore;
- same consumer context.

If derived:
- recompute identically.

---

# APPENDIX AI — B3/B4 SHARED EVENT–INTEL BOUNDARY

Plan 31 and C1[9] are adjacent but must not collapse into one authority.

## AI.1 Event layer owns
- semantic event identity;
- event provenance;
- briefing semantics.

## AI.2 Intel systems own
- signal/forecast/trace state;
- reliability/trust according to current owners;
- availability/consumption.

## AI.3 Presentation joins them
A briefing may point to intel source, but:
- event layer does not become trust owner;
- radio does not become briefing owner.

## AI.4 Cross-plan test

Create one integration case if plans require:
- intel event emitted;
- briefing semantic kind correct;
- navigation opens intel surface;
- reading surface does not duplicate event/trust effects;
- diagnostics trace contains event.

This test proves composition without authority fusion.

---

# APPENDIX AJ — C2[8] RELEASE-GATE RECONCILIATION TABLE

Build the table from the C2[8] source plan and current release docs:

| Required gate | Historical plan | Current gate name | Current command | Blocking? | Missing delta |
|---|---|---|---|---|---|
| build |  |  |  |  |  |
| data parity |  |  |  |  |  |
| export smoke |  |  |  |  |  |
| perf |  |  |  |  |  |
| save |  |  |  |  |  |
| integrity |  |  |  |  |  |
| verify-fast |  |  |  |  |  |

Only include gates actually required by plan/current release policy.

## AJ.1 Duplicate-gate prevention

If two scripts perform same acceptance:
- designate canonical one;
- do not run duplicate merely to satisfy historical naming.

## AJ.2 Release failure report

A failed ship gate should state:
- gate;
- command;
- artifact/build ID;
- expected;
- actual;
- log.

No opaque “release failed”.

---

# APPENDIX AK — C2[9] LIFECYCLE CONTRACT MATRIX

For every subsystem category touched by C2[9]:

| Subsystem | Construct | Setup | Tick | Save | Restore | Flush | Dispose | Gate coverage |
|---|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

The matrix identifies lifecycle gaps without forcing all subsystems into phases they do not need.

## AK.1 Required vs not-applicable

A subsystem that has no disposable resource may mark Dispose N/A with reason. Do not add empty lifecycle methods solely to satisfy matrix symmetry.

## AK.2 Order invariants

If setup order matters:
- pin current canonical order;
- manifest migration must preserve.

## AK.3 Duplicate registration

Gate should detect where feasible:
- subsystem constructed twice;
- same owner registered twice;
- duplicate event subscription due migration.

## AK.4 Save/restore mapping

Any manifest/declarative registry must not accidentally omit save participants.

---

# APPENDIX AL — C2[9] SMALL-DELTA VS PROMOTION TEST

A delta may execute inline if all are true:

- ≤ one subsystem family or one lifecycle phase;
- no broad composition-root rewrite;
- no changes to dozens of Main partials;
- existing triad gate can prove parity;
- rollback is local.

Promote if any:
- construction of many subsystems moves;
- registry schema introduced;
- startup order reorganized;
- broad dependency injection changes;
- large save participant migration.

The promotion is not a failure; it is correct scope control.

---

# APPENDIX AM — D1 PRODUCT-PROOF ACCEPTANCE MODEL

The seven-day slice must prove **composition**, not perfect balance.

## AM.1 Product-proof categories

### Functional
Core loop actions work.

### Continuity
State persists day-to-day and across save if required.

### Consequence
Player decisions produce visible/system consequences.

### Comprehensibility
Human tester can understand critical state/choices.

### Stability
No blocking crash/softlock/irrecoverable route failure.

### Determinism
Scripted half reproducible.

## AM.2 Not automatically failure
- minor balance imperfection;
- cosmetic issue;
- optional content gap.

These are findings unless D1 contract says release-blocking.

## AM.3 Blocking failure
Examples:
- cannot advance day;
- producer silently loses critical output;
- required panel inaccessible;
- save corrupts;
- scripted state diverges nondeterministically;
- core required loop absent.

## AM.4 Day-7 proof

Final artifact contains:
- state fingerprint;
- resource/survivor summary as plan requires;
- key decisions made;
- critical events;
- no impossible state.

---

# APPENDIX AN — D1 SCRIPTED/HUMAN DIVERGENCE HANDLING

If scripted slice green but human slice fails:
- classify UX/manual-path issue;
- record exact player path;
- do not alter scripted oracle to hide it.

If human slice green but scripted fails:
- diagnose determinism/harness/hidden state;
- do not waive automated proof.

Both halves are required if D1 plan says dual product proof.

---

# APPENDIX AO — E1 DECISION REGISTER SOURCE CONSISTENCY

For every item, compare:

| Decision | Memo verdict | Debt row | Closeout | Integration ledger | Register | Consistent? |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

The register pass fixes contradictions.

## AO.1 Precedence

Detailed signed memo/verdict is primary evidence for chosen option.
Register summarizes.
Owning doc/debt records execution status.
Integration ledger records package lifecycle.

## AO.2 Decision reopened by changed premise

If current source materially invalidates old unsigned memo:
- do not sign stale options;
- route memo refresh as decision-preparation work.
This is the one case where “no new memos” gives way to actuality: execution cannot rely on false premise. Record why refresh was needed.

## AO.3 Signed but obsolete

If decision signed but later implementation already satisfies it:
- verify;
- close as executed/resolved;
- no duplicate execution.

---

# APPENDIX AP — DECISION REGISTER CADENCE

Recommended cadence options for foreman:

### Per-wave
Review before each blocker wave close.

### Per-release
Review unresolved product/governance choices at release planning.

### Event-triggered
Recheck when named dependency condition becomes true.

The register itself should support all three via `recheck trigger`.

Success is not “zero deferred decisions”; success is zero **ambiguous** decisions.

---

# APPENDIX AQ — F1 E1 FIRST-SUBPLAN PROMOTION CHECKLIST

If E1 is ready:

- [ ] first sub-plan exact title;
- [ ] all prerequisites sealed;
- [ ] current premise valid;
- [ ] owner exists;
- [ ] paths unclaimed;
- [ ] no decision missing;
- [ ] bounded package;
- [ ] acceptance measurable;
- [ ] implementation log location known.

If any false:
- queue/block, do not execute.

---

# APPENDIX AR — IMPLEMENTATION COMMAND LOG TEMPLATE

For every focused command:

**Task:**<br>
**Plan clause:**<br>
**Command:**<br>
**Commit:**<br>
**Expected:**<br>
**Actual:**<br>
**Cases/checks:**<br>
**Duration:**<br>
**Warnings:**<br>
**Artifact:**<br>
**Disposition:**<br>

This exact command log goes into implementation log or linked acceptance artifact.

---

# APPENDIX AS — CROSS-TASK MERGE ORDER

Recommended:

1. B3 semantic/diagnostic/navigation delta.
2. B4 intel remainder.
3. B5 only once dependencies confirmed.
4. C1/C2 independent deltas.
5. D1 after chain readiness.
6. E1 register pass can occur when foreman available.
7. F1 identity resolution can occur early after census, but execution follows DAG.

After each merge:
- refresh claims;
- rerun census readiness;
- update queue.

Do not build later package from pre-merge assumptions.

---

# APPENDIX AT — PART 2 METRICS THAT MATTER

Report:
- C1[8] clauses sealed elsewhere vs executed;
- C1[9] intel outputs with live consumers;
- producers audited / silent-loss gaps fixed;
- producer-port gate coverage;
- C2[8] release clauses sealed vs delta;
- C2[9] lifecycle clauses sealed vs promoted migration;
- D1 scripted run stability and human blocking findings;
- decision register counts by verdict;
- E1 corpus status;
- ready Wave 11 nodes.

Do not report success by:
- lines changed;
- number of docs written;
- raw test count;
- number of plan files touched.

---

# APPENDIX AU — FINAL PRE-WAVE-11 AUDIT

Before handing to Wave 11:

1. Re-enumerate active claims.
2. Recompute census DAG readiness.
3. Confirm B3/B4/B5 logs.
4. Confirm C2[8]/C2[9] logs.
5. Confirm D1 status.
6. Confirm decision register.
7. Confirm E1 row.
8. Confirm no new micro-deferral escaped logs.
9. Confirm build.
10. Confirm verify-fast.
11. Confirm data integrity/utilization.
12. Confirm save battery where relevant.
13. Confirm release gate where relevant.
14. Confirm docs index.
15. Generate ranked Wave 11 queue.

Any mismatch returns to owning task before Wave 11 planning.

---

# APPENDIX AV — FINAL NO-FALSE-CLOSURE RULES

Wave 10 Part 2 is not complete if:

- B3 only implements semantic kinds but ignores navigation/diagnostics;
- B3 claims navigation complete while Plan 24 path is still unhandled/claimed;
- B4 declares intel valuable because it is displayed, without a real consumer chain;
- B5 reports producer success without authoritative sink evidence;
- B5 lacks permanent no-silent-loss regression protection;
- C2[8] duplicates release gates instead of reconciling them;
- C2[9] hides a large manifest migration inside an “executed delta”;
- D1 scripted slice does not traverse the plan-defined core loop;
- D1 human half is omitted when plan requires it;
- E1 register retains unsigned items with no owner/condition;
- F1 still does not know what E1 is;
- implementation logs are missing;
- census/ledger disagree;
- Wave 11 queue is generated by numeric sequence instead of readiness.

**Final invariant:** every Wave 10 Part 2 package must leave the repository more truthful than it found it. A plan may close through implementation, evidence that it was already sealed, an explicit dependency/claim block, or a recorded decision. It may not close through ambiguity.

# APPENDIX AW — FINAL MERGE-READINESS PACKET

## AW.1 B3
Ready to merge only when:
- full Plan 31 clause matrix is terminal;
- Wave 9 B1 overlap is explicitly reconciled;
- any active briefing claim is resolved or exact remainder stays claim-blocked;
- navigation uses stable route metadata;
- diagnostics replay is deterministic/read-only;
- Campaign/parity/UI gates pass;
- implementation log and census update are committed together.

## AW.2 B4
Ready only when:
- every plan-required intel output has a classified consumer state;
- dead outputs are wired, retired, or decision-routed;
- reliability/comms work reuses existing authorities;
- intel-value tests prove decision-context impact rather than mere display presence;
- replay fingerprint remains stable for read-only evaluation;
- implementation log names all sealed-elsewhere work.

## AW.3 B5
Ready only when:
- Plan 23/B1/B3 dependencies are terminal as required;
- producer audit covers the plan’s complete producer set;
- every success reaches a canonical sink;
- every sink refusal is typed/player-visible where contract requires;
- producer-port gate is permanent;
- delivery persists across save/restore;
- no-silent-loss test family passes;
- no new resource/sink/bill authority was added.

## AW.4 C2[8]
Ready only when:
- data source/export copy authority is unambiguous;
- parity check detects drift;
- real exported artifact smoke is current;
- performance budgets use existing/signed thresholds;
- one reconciled ship gate exists;
- release docs and implementation log agree.

## AW.5 C2[9]
Ready only when:
- current orchestration/lifecycle matrix is complete;
- small deltas are behavior-preserving;
- large manifest migration is promoted, not smuggled inline;
- triad/lifecycle/fingerprint gates remain green;
- census status distinguishes executed delta from promoted remainder.

## AW.6 D1
Ready only when:
- all hard dependencies are sealed;
- scripted seven-day run traverses plan-defined loop;
- repeated runs are stable;
- save parity passes if required;
- human checklist completed;
- release cadence updated;
- findings routed rather than silently fixed outside scope.

## AW.7 E1
Ready only when:
- register includes every known routed decision;
- every item has signed/declined/deferred-with-condition/queued status;
- bounded signed executions pass their memo-specific gates;
- larger executions have live ledger packages;
- owning docs agree with register;
- no unsigned-without-condition item remains.

## AW.8 F1
Ready only when:
- E1 corpus identity is exact;
- dependencies and mandatory order are known;
- current premise/status verified;
- DAG position recorded;
- first sub-plan executed only if ready/bounded;
- census reaches full classified coverage.

---

# APPENDIX AX — WAVE 10 PART 2 RELEASE NOTE TEMPLATE

**Wave:** 10 Part 2<br>
**Commit:**<br>
**Date:**<br>

### Corpus execution
- C1[8]:
- C1[9]:
- C1[10]:
- C2[8]:
- C2[9]:
- D1:
- E1 corpus:

### Governance
- Decision register unsigned count before:
- Decision register unsigned-without-condition after:
- Active claims before:
- Active claims after:

### Quality gates
- Build:
- Verify-fast:
- Save battery:
- Data integrity:
- Content utilization:
- Replay/fingerprint:
- Export smoke:
- Performance:
- Seven-day slice:

### Queue impact
**Dependencies released:**<br>
**New blockers discovered:**<br>
**Wave 11 top ready node:**<br>
**Why it is next:**<br>

The release note must be generated from the latest census/ledger state, not copied from the Wave 10 planning source.

**End of Wave 10 Part 2.**

# FINAL EXECUTION NOTE

Re-run the census readiness calculation after the final merge and before any Wave 11 package is claimed. A dependency released by B3/B5/C1/C2 may reorder the queue, and that reordered queue—not historical corpus numbering—is the authoritative next-step source.
