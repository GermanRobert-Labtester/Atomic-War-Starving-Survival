# ASHFALL — GENERATION WAVE 11 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 1

**Document role:** execution-grade Part 1 plan for Wave 11, derived only from the verified queue and premise table in the supplied Wave 11 roadmap.

**Wave 11 premise:** the queue remains real. The Wave 10 corpus census leaves roughly 73 unclaimed plan files beyond the executed frontier, and the next segment has been re-verified at current source. This document executes only the first verified chain segment and preserves the standing cadence:

**read the plan doc → premise-check → consume the census row → execute the remainder → write the implementation log → update the ledger**

**Part 1 task set:**
1. **A1 — C1[11] “The Year Turns”**
2. **A2 — C1[12] “Memory That Acts”**
3. **A3 — C1[13] “Governing Together”**
4. **A4 — C1[14] “The Content Acceptance Pipeline”**
5. **A5 — C1[15] “The Mod & Content-Pack Contract”**
6. **B1 — C2[10] “Autonomous Outside World”**
7. **B2 — C2[11] “One Place Authority, Graph-Native Travel, Geographic Knowledge”**

**Primary rule:** historical plan text defines intended scope; current source defines present truth; the census defines the current remainder. No package may reimplement already-sealed work simply because the original plan described it.

---

# 0. GLOBAL EXECUTION CONTRACT

## 0.1 Allowed terminal states

Every task/sub-scope ends as one of:

- **SEALED** — the current plan contract is fully satisfied.
- **SEALED-ELSEWHERE** — already completed by earlier work; evidence recorded.
- **PARTIALLY-SEALED** — exact remainder remains and is named.
- **DEPENDENCY-BLOCKED** — valid work waits on one or more unsealed prerequisites.
- **CLAIM-BLOCKED** — valid work waits on active path ownership.
- **DECISION-BLOCKED** — implementation semantics require foreman signature.
- **SUPERSEDED** — a newer authority/contract replaces the historical plan.
- **STALE-PREMISE** — the plan’s central premise is no longer true.
- **RETIRED** — product/architecture decision closes the plan.
- **ROUTED-REPAIR** — execution exposes a genuine production regression that belongs in a bounded repair package.

No task may finish as “mostly done”, “probably covered”, “future”, or “TBD”.

## 0.2 Hard rules

1. Claims before edits.
2. Re-verify every premise at `HEAD`.
3. Consume the current census row before executing.
4. Respect each plan’s declared mandatory order.
5. Current owners remain authoritative.
6. No parallel calendar, memory, leadership, content-acceptance, mod-loader, politics, or place authority.
7. Derived behavior does not create persistence unless unavoidable and authorized.
8. New persisted state is additive/versioned and old-save neutral.
9. Exactly-once semantics are tested across restore when relevant.
10. Same seed + same state remains deterministic.
11. UI/read models never become domain owners.
12. Missing-consumer findings are either wired through the named owner, retired with evidence, or decision-routed.
13. Decision-gated consolidation or balance changes stop for signature.
14. Focused verification first.
15. Generated docs/manifests/checks are updated through their source process.
16. Every executed corpus plan gets an implementation log.
17. Census and integration ledger are updated after each terminal package.
18. A larger-than-package migration is promoted, not smuggled inside a “delta” task.
19. Content authoring never substitutes for a missing mechanism.
20. A mechanism never gets invented merely to justify pre-existing content.

## 0.3 Required evidence bundle

Each task handoff includes:

- current commit / worktree;
- consumed census row;
- dependency state;
- active claims;
- full clause/remainder matrix;
- sealed-elsewhere evidence;
- exact files changed;
- authority map;
- save/persistence impact;
- determinism impact;
- focused tests;
- owning suites/selftests;
- build;
- integrity/utilization where relevant;
- UI lifecycle/a11y/snapshot evidence where relevant;
- implementation log;
- census update;
- integration ledger update;
- remaining blocker;
- next DAG node released.

## 0.4 Stop conditions

Stop implementation when:

- a hard dependency is unsealed;
- a required path is actively claimed;
- a current owner differs materially from the historical plan’s assumed owner;
- a new state authority would be required;
- a proposed behavioral consumer has no plan-defined semantics;
- a plan remainder expands into a cross-repo consolidation touching many consumers;
- a content gate or mod compatibility rule requires policy that is not defined by the plan;
- outside-world autonomy would require speculative actor classes not named by the plan;
- place-authority consolidation touches multiple live owners without signed migration design.

---

# 1. ORDERING AND DEPENDENCIES

## 1.1 Recommended execution order

**A1 → A2 → A3 → A4 → A5 → B1 → B2**

This ordering follows the provided Wave 11 chain segment. However, the current census DAG remains authoritative. If B1/B2 are independently ready and claims are disjoint, they may execute earlier.

## 1.2 Expected dependency themes

- A1 may depend on the event layer / Plan 31 and Plan 27 fidelity rails.
- A2 consumes memory systems already sealed in earlier waves.
- A3 may consume duty roster, grievance, leadership, and consent-adjacent rails.
- A4 consumes existing integrity/utilization/canon/quality gates.
- A5 consumes A4’s acceptance pipeline where content-pack acceptance is required.
- B1 likely consumes event/intel/economy/faction-ecology rails from earlier chain nodes.
- B2 may require current map/travel/knowledge owners and can become decision-blocked if true consolidation is broad.

Every task must read its plan header rather than trusting these expected relationships.

---

# 2. TASK A1 — C1[11] “THE YEAR TURNS”

## 2.1 Objective

Extend the existing campaign calendar/clock infrastructure with the plan’s missing deadline-and-seasonal-consequence contract without creating a parallel deadline system. The task must support authored deadlines, exactly-once met/missed resolution, consequence routing through existing owners, save/restore stability, and player-visible warning state where the plan requires it.

## 2.2 Entry gate

Before edits:

- read `C1_planintegration[11].md` fully;
- consume current census row;
- resolve declared dependencies;
- inspect `AsSimClock` / campaign calendar owner;
- inspect day/season derivation;
- inspect 20C seasonal weather table;
- inspect deadline-adjacent systems:
  - embargo decay windows;
  - loan due dates;
  - distress/follow-up scheduling;
  - stage-day ranges;
  - any quest/event due windows;
- inspect current day-event vocabulary;
- inspect current briefing/journal warning surfaces and claims.

Possible outcome:
- calendar authority already satisfies more of the plan than historical text suggests;
- deadline mechanism remains open;
- seasonal hooks may be partially sealed elsewhere.

## 2.3 Calendar authority boundary

The calendar owner may own:
- campaign day;
- season derivation;
- authored deadline evaluation;
- deadline fired/missed ledger if Plan 11 places it here.

The calendar owner does **not** own:
- economy shock state;
- quest state;
- weather effects;
- survivor needs;
- faction state;
- player UI.

Deadline consequences route to those existing authorities.

## 2.4 Deadline catalog contract

Use the plan-named catalog or repository-equivalent.

Each row should include only fields required by the plan, such as:

- stable deadline ID;
- active day/window;
- condition / success criterion;
- consequence class;
- consequence target/reference;
- warning lead time if contract defines;
- semantic event ID / vocabulary mapping if required.

Do not invent a generic scripting language.

## 2.5 Integrity rules

Validate:

- IDs unique;
- windows legal and reachable;
- start ≤ end;
- referenced consequence owner/ID exists;
- warning lead does not underflow before campaign start;
- no impossible season/day combination;
- no duplicate exactly-once key;
- every authored row has a runtime evaluator.

## 2.6 Deadline evaluation model

Preferred model:

1. calendar advances to authoritative day;
2. evaluate deadlines whose window edge is relevant;
3. determine met / still-active / missed;
4. emit plan-defined semantic event;
5. route consequence through existing owner;
6. write fired/missed key exactly once;
7. expose read-model state.

No per-frame evaluation.

## 2.7 Exactly-once ledger

Required properties:

- stable deadline ID key;
- met/missed terminal state not replayed;
- save captures ledger;
- restore rehydrates without refiring;
- old saves default empty/neutral;
- repeated day evaluation safe.

If existing generic fired-key ledger can own this safely, reuse it. Do not add a second top-level save section merely for naming convenience.

## 2.8 Met vs missed semantics

Read exact plan.

For each authored deadline:
- **met**: condition satisfied inside allowed window;
- **missed**: window closes without condition;
- optional **expired/waived** only if plan defines.

Do not invent “partial credit”.

## 2.9 Consequence routing

Build:

`Consequence class | Existing owner | Command/event API | Save owner | Test`.

Potential classes mentioned by source:
- economy shock;
- quest trigger;
- day event;
- seasonal event.

Use only classes actually specified by plan/catalog.

## 2.10 No parallel due-date system

Existing loan due dates, follow-ups, etc. may remain under their owners.

The shared calendar authority provides:
- day basis;
- optional shared authored-deadline mechanism.

Do not migrate every existing due date into C1[11] unless plan explicitly demands consolidation.

## 2.11 Seasonal consequence hooks

Read plan’s named hooks.

For each:
- identify current seasonal source;
- identify existing owner;
- determine whether C1[11] only schedules/activates it.

Examples from source context:
- seasonal weather crises;
- seasonal events.

No duplicate weather/economy logic.

## 2.12 Player visibility

If plan requires briefing/journal warnings:

Read model may expose:
- deadline name/ID;
- days remaining;
- state;
- target source.

Presentation:
- uses vocabulary;
- does not mutate deadline;
- does not evaluate consequence;
- route to owner/detail if Plan 31 navigation supports.

If briefing file is claimed, split and defer surface rather than race.

## 2.13 Tests

Minimum:

1. future deadline remains inactive;
2. active window visible;
3. condition met before close → fires met once;
4. miss at window close → fires missed once;
5. repeated tick → no duplicate;
6. save before deadline → restore → same outcome;
7. save after terminal → restore → no refire;
8. old save → empty ledger;
9. invalid catalog window rejected;
10. unresolved consequence reference rejected;
11. consequence reaches owner measurably;
12. seeded calendar reachability for every authored sample.

## 2.14 Determinism

Deadline evaluation:
- day-keyed;
- no RNG by default;
- stable ordering when several resolve same day;
- deterministic tie/order by stable ID;
- fingerprint stable across continuous vs mid-reload.

## 2.15 Sample content discipline

Mechanism samples only.

Do not author broad seasonal content tranche.

Samples exist to prove:
- met;
- missed;
- at least one consequence class.

## 2.16 Save compatibility

If adding ledger:
- additive field/section under correct owner;
- default empty;
- capture/restore tests;
- schema count/version only if repository requires.

No unrelated save changes.

## 2.17 Focused verification

Order:
1. deadline parser/integrity tests;
2. calendar deadline tests;
3. consequence owner integration tests;
4. save round-trip;
5. Campaign suite;
6. World/calendar/season suite;
7. briefing/journal UI gates if touched;
8. replay/fingerprint;
9. integrity;
10. utilization;
11. build;
12. verify-fast.

## 2.18 Implementation log centerpiece

`Deadline clause | Historical requirement | Existing calendar support | Implemented delta | Consequence owner | Save | Test | Status`.

## 2.19 Non-goals

- no second clock;
- no second due-date engine;
- no broad seasonal content expansion;
- no per-frame evaluation;
- no new consequence authority;
- no speculative recurring-calendar feature beyond plan.

---

# 3. TASK A2 — C1[12] “MEMORY THAT ACTS”

## 3.1 Objective

Reconcile memorials, heirlooms, place memory, and generational continuity against current systems, then wire only the plan-defined behavioral consumers needed to turn recorded memory into decision-relevant behavior. Existing memory authorities remain authoritative; the new work is primarily consumer wiring.

## 3.2 Entry gate

Read:
- C1[12] fully;
- census row;
- memorial system;
- `HeirloomSystem.cs`;
- generational links / 19B;
- narrative memory corpus;
- location/place evolution or memory systems;
- relationship/morale/needs modifier rails.

Build clause matrix before code.

## 3.3 “Acts” acceptance test

For every memory type:

`Memory source → persisted state → behavioral consumer → measurable effect`.

If a memory only displays text and the plan requires action:
- gap remains.

If display-only is intended for a sub-clause:
- do not invent gameplay effect.

## 3.4 Memorials

Audit:
- memorial creation;
- persistence;
- mourning/relationship interactions;
- current behavioral consumers.

If Wave 8 A3 memorial/mourning already creates morale/journal behavior:
- credit it.

Only implement remaining plan-specific effect.

## 3.5 Heirlooms

Audit:
- ownership;
- save state;
- item effects;
- inheritance/generation transfer;
- current UI.

If plan requires behavioral value:
- wire through existing modifier/stat/morale rail;
- bounded effect;
- unbound parity;
- no private heirloom stat authority.

## 3.6 Place memory

Determine exact plan concept:

Potentially:
- prior visit;
- event history;
- destruction/change;
- relationship to place;
- remembered hazard/resource.

Map current location evolution/knowledge systems.

Behavioral consumer may be:
- revisit encounter selection;
- route context;
- journal/briefing;
- NPC/survivor reaction.

Use exact plan seam only.

## 3.7 Generational continuity

Credit:
- 19B links;
- Wave 6 arc work;
- existing lineage/heirloom transfer.

Remainder only.

No second lineage system.

## 3.8 Modifier integration

If memory contributes to morale/needs/stats:
- use existing shared modifier stack;
- derived from persisted memory/heirloom;
- stable contribution ID;
- no double application;
- visible contributor if current stack exposes contributors.

## 3.9 Event/encounter integration

If place memory affects encounter selection:
- add read-only condition/weight through existing encounter authority;
- no second RNG stream unless plan requires;
- stable seed behavior;
- neutral parity without memory.

## 3.10 Exactly-once vs derived

Classify each effect:

### Derived
Heirloom held → modifier exists.
No extra save.

### Exactly-once
Memorial action triggers one consequence.
Use existing fired ledger if required.

Avoid persisting derived effect separately.

## 3.11 Tests

For each implemented wiring:
- no memory → legacy behavior;
- memory present → measurable plan-defined effect;
- save/restore memory → effect restored;
- repeated evaluation → no double contribution;
- removing/loss state updates effect correctly if allowed;
- encounter deterministic with/without memory;
- memorial exactly-once if applicable.

## 3.12 Text/tone

Any new memory-driven text:
- uses existing corpus voice;
- no prose rewrite of sealed content;
- no new narrative authority;
- localization shape consistent.

## 3.13 Determinism

Run a memory-heavy seeded window:
- loss/memorial;
- heirloom;
- revisit;
- generational link if supported.

Continuous and restored fingerprints match.

## 3.14 Implementation log

Centerpiece:

`Memory | Owner | Persisted? | Existing consumer | Plan-required behavior | Bounded wiring | Effect | Test`.

## 3.15 Verification

- memorial/phantom suites;
- heirloom tests;
- relationship/morale/needs neighborhood;
- location/encounter tests;
- save round-trip;
- replay/fingerprint;
- integrity;
- build;
- verify-fast.

## 3.16 Non-goals

- no new memory store;
- no prose rewrite;
- no display-only expansion;
- no unbounded stat system;
- no duplicate lineage.

---

# 4. TASK A3 — C1[13] “GOVERNING TOGETHER”

## 4.1 Objective

Execute the current remainder of leadership, policy, consent, and crew-refusal mechanics by extending existing leadership, roster, grievance, and assignment owners. Every acceptance/refusal must be explicit and typed; silent consent and silent refusal are forbidden where the plan requires agency.

## 4.2 Entry gate

Read:
- C1[13] full plan;
- census row;
- `LeadershipSystem.cs`;
- duty roster/assignment command path;
- grievance/cohort-conflict systems;
- policy/governance mechanics under any current name;
- Wave 8 A2 assignment explicit-confirmation flow if landed.

## 4.3 Clause matrix

`Sub-scope | Current owner | Existing behavior | Plan remainder | Decision needed? | Action`.

Sub-scopes:
- leadership;
- policy cadence;
- consent;
- refusal;
- consequences.

## 4.4 Leadership authority

Do not replace `LeadershipSystem.cs`.

Determine:
- succession;
- authority;
- modifiers;
- policy ownership if any.

If leadership sub-plan already sealed:
- cite it.

## 4.5 Policy mechanism

Read exact plan.

Possible contract may include:
- policy proposal;
- cadence;
- approval/consent;
- active policy state;
- expiry/change.

Do not invent policy categories.

If no current owner and plan defines one:
- extend leadership/governance owner according to plan.

If design semantics missing:
- decision-block.

## 4.6 Consent contract

Consent must be a typed result of the relevant command.

Potential result:
- accepted;
- accepted-with-warning if plan defines;
- refused with reason.

No panel-side boolean.

## 4.7 Refusal conditions

Use exact plan conditions.

Potential inputs could include:
- exhaustion;
- grievance;
- injury;
- relationship;
- dangerous duty.

Do not invent thresholds.

Read current owners for those states.

## 4.8 Refusal consequences

Refusal consequence belongs to existing owners:
- grievance;
- morale;
- standing;
- roster vacancy;
- leadership authority.

No parallel “refusal score”.

## 4.9 Assignment integration

If duty roster owns assignment:
- command preflight checks refusal/consent;
- explicit confirm flows remain;
- auto-assignment cannot silently override refusal unless plan explicitly authorizes.

## 4.10 Policy/crew conflict

If policy command needs group consent:
- use plan’s aggregation semantics;
- deterministic;
- no unordered roster dependence;
- no hidden random vote.

If actual democratic voting simulation is not in plan, do not create it.

## 4.11 Persistence

Prefer:
- active policy persisted by policy/leadership owner;
- grievances persisted by grievance owner;
- roster assignment by roster owner.

Refusal result itself often transient/derived.

Add ledger only if plan requires lasting consent/refusal record.

## 4.12 UI

Roster/policy surface:
- shows refusal reason;
- requires explicit acknowledgement where plan says;
- keyboard accessible;
- no red/green-only semantics;
- current state revalidated before commit.

## 4.13 Tests

Minimum:
- accepted assignment;
- refusal condition;
- refusal reason;
- consequence routed;
- no assignment committed on refusal;
- restore preserves underlying state and yields same result;
- leadership change affects policy only per plan;
- deterministic roster ordering;
- auto-assignment cannot silently bypass.

## 4.14 Governance-heavy replay

Seeded window:
- leadership state;
- assignment;
- refusal/consent;
- consequence;
- save/restore;
- fingerprint parity.

## 4.15 Implementation log

`Clause | Current owner | Existing state | New delta | Typed result | Consequence owner | Save | Test`.

## 4.16 Verification

- leadership tests;
- duty roster;
- grievance/cohort;
- relevant needs/health if inputs;
- UI lifecycle/a11y;
- replay;
- save;
- integrity;
- build;
- verify-fast.

## 4.17 Non-goals

- no second leadership authority;
- no broad political/democracy simulation;
- no silent outcomes;
- no invented refusal thresholds;
- no AI personality simulation beyond current inputs.

---

# 5. TASK A4 — C1[14] “THE CONTENT ACCEPTANCE PIPELINE”

## 5.1 Objective

Turn the existing `ContentAcceptanceLadder` / `ContentAcceptanceRung` mechanism into the plan-defined acceptance pipeline: current content gates composed in the specified order, fail-fast with existing messages, wired into the appropriate CI/manifest path, and documented as the mandatory acceptance path for future content batches.

## 5.2 Entry gate

Read:
- C1[14];
- census row;
- ladder/rung classes;
- current consumers;
- integrity selftest;
- content-utilization selftest;
- canon audits;
- narrative/content quality checks;
- CI manifest/gate composition.

Classify current ladder:
- advisory only;
- partially gating;
- fully gating.

## 5.3 Rung truth pass

For each rung:

`Rung | Historical criterion | Current gate | Still valid? | Owner | Command | Result semantics`.

If rung references retired criterion:
- update plan/ladder according to current contract;
- do not keep zombie gate.

No new acceptance criterion in A4.

## 5.4 Pipeline order

Use exact plan.

Source suggests likely:
1. integrity;
2. utilization;
3. canon;
4. quality.

But live plan text is authoritative.

## 5.5 Orchestrator design

Thin composition only.

Responsibilities:
- run named gates in order;
- stop/fail according to plan;
- preserve each gate’s error output;
- aggregate report if plan requires;
- return process/status code.

Does not duplicate validator logic.

## 5.6 Fail-fast semantics

If plan says first red rung stops:
- stop immediately;
- show rung;
- preserve original error.

If plan requires full report:
- collect without changing individual gate semantics.

Read plan.

## 5.7 Scope input

Acceptance may run:
- entire current corpus;
- changed batch/manifest;
- content pack.

Implement exact plan path.

Avoid a slow full-corpus run on every local edit if plan expects scoped batch.

## 5.8 CI wiring

One manifest edit.

Requirements:
- no duplicate execution of same gate;
- correct dependency order;
- correct failure propagation;
- local command documented.

No CI restructuring beyond plan.

## 5.9 Sample-batch test

Create/identify controlled content batch:
- all-green case;
- deliberate integrity fail fixture;
- deliberate utilization fail fixture where test infrastructure permits;
- verify rung identification.

Do not pollute production catalogs.

## 5.10 Baseline current-corpus run

Run pipeline once over current content as plan requires.

Possible result:
- green;
- existing red findings.

If red:
- route findings to owners;
- do not weaken pipeline to force green.

## 5.11 Documentation

Acceptance pipeline doc:
- purpose;
- rung order;
- command;
- batch scope;
- failure semantics;
- how content authors respond;
- relation to mod/content-pack acceptance if A5 later consumes it.

## 5.12 Pipeline tests

- all rungs green;
- first rung fails;
- middle rung fails;
- gate output identifies rung;
- order deterministic;
- no gate skipped;
- no duplicate execution;
- manifest invocation works.

## 5.13 Implementation log

`Rung | Existing gate | Historical plan | Current validity | Pipeline action | Test`.

## 5.14 Verification

- tooling tests;
- each underlying gate;
- pipeline test;
- current corpus baseline;
- CI manifest check;
- docs index;
- build;
- verify-fast.

## 5.15 Non-goals

- no new validator criteria;
- no content rewrite;
- no CI architecture rewrite;
- no hiding existing red rows;
- no new content-authoring system.

---

# 6. TASK A5 — C1[15] “THE MOD & CONTENT-PACK CONTRACT”

## 6.1 Objective

Extend the existing live mod loader and `MOD_CONTRACT.md` with the plan-defined compatibility governance, deterministic overlay proof, and content-pack semantics. No loader rewrite and no mod-manager UI.

## 6.2 Entry gate

Read:
- C1[15];
- census row;
- Wave 2 D3 forensic/closeout;
- current mod loader;
- manifest schema;
- prefix/override rules;
- traversal/order semantics;
- schema-version check;
- sample mod fixture;
- A4 acceptance pipeline status.

## 6.3 Clause matrix

Axes:
- stable boundaries;
- deterministic overlays;
- compatibility governance;
- content pack.

For each:
`Historical requirement | Existing loader | Remainder | Action`.

## 6.4 Stable boundary audit

Identify:
- mod-stable catalogs;
- ID/prefix rules;
- overrideable fields/catalogs;
- forbidden/internal areas.

Do not expand public mod API without plan.

## 6.5 Compatibility semantics

Read exact plan.

Potential requirements:
- game version range;
- schema version;
- minimum/maximum compatible version;
- capability/contract version.

Implement only named semantics.

Typed rejection:
- incompatible game version;
- incompatible schema;
- malformed manifest;
- unsupported pack type.

No silent skip.

## 6.6 Manifest evolution

If fields added:
- versioned;
- old sample mod behavior defined;
- missing field default according to contract;
- parser validation.

Do not break old mods without explicit compatibility rule.

## 6.7 Content-pack class

If plan defines:
- multi-catalog bundle;
- one pack manifest;
- ordered catalog list;
- acceptance gate via A4;
- deterministic overlay order.

Use existing loader.

No second pack loader.

## 6.8 Overlay determinism

Same:
- base data;
- mod set;
- mod order;
- seed

must produce identical merged data / campaign fingerprint.

Define ordering:
- manifest order;
- stable mod ID order;
- repository contract.

No filesystem enumeration dependence.

## 6.9 Conflict semantics

Read existing override rules.

If multiple mods target same ID:
- apply current deterministic precedence;
- reject conflict only if contract says;
- do not invent merge logic.

## 6.10 Modded replay

Use sample mod/content pack:
- load;
- run seeded scenario;
- record ledger/fingerprint;
- repeat;
- save/restore if current modded save semantics are part of contract.

## 6.11 A4 pipeline integration

If content pack requires acceptance:
- invoke pipeline per pack/batch;
- fail load/acceptance according to plan;
- do not duplicate rungs in loader.

## 6.12 MOD_CONTRACT.md

Update:
- manifest fields;
- game/schema compatibility;
- overlay order;
- content packs;
- acceptance;
- typed rejection;
- deterministic guarantees.

This is the mod API contract.

## 6.13 Tests

- compatible range accepted;
- too-old/new rejected per contract;
- schema mismatch;
- malformed range;
- old manifest default;
- multi-catalog pack;
- deterministic overlay;
- conflict precedence;
- acceptance pipeline invocation;
- modded replay;
- integrity under mod load.

## 6.14 Save compatibility

If save records mod set today:
- verify unchanged or extend only per plan.

If not part of plan:
- do not add.

## 6.15 Implementation log

`Clause | Existing loader support | Delta | Manifest field | Rejection | Determinism test | Status`.

## 6.16 Verification

- mod test family;
- loader tests;
- A4 pipeline tests;
- modded integrity;
- replay/fingerprint;
- build;
- verify-fast;
- docs index.

## 6.17 Non-goals

- no mod manager UI;
- no Steam Workshop;
- no loader rewrite;
- no unbounded public API expansion;
- no nondeterministic filesystem ordering.

---

# 7. TASK B1 — C2[10] “AUTONOMOUS OUTSIDE WORLD”

## 7.1 Objective

Implement the current C2[10] remainder only after its DAG prerequisites are sealed. The plan’s key open scope is deterministic off-screen world autonomy—neighbour/faction actors evolving outside the player’s immediate view and projecting consequences into existing player-visible systems.

## 7.2 Entry gate

Read C2[10] fully.

Resolve:
- event-layer dependency;
- intel dependency;
- faction ecology;
- war/standing;
- economy shock;
- encounter;
- wildlife/anomaly movement if named.

If any hard prerequisite unsealed:
- mark DEPENDENCY-BLOCKED;
- queue at correct position;
- no speculative autonomy system.

## 7.3 Premise reconciliation

Current world already changes through:
- faction ecology/war;
- wildlife migration;
- anomaly movement;
- market shocks.

Therefore C2[10] is not “make world move” if those are live.

Identify exact remainder:
- off-screen actor driver;
- neighbour simulation;
- faction-to-faction autonomous transitions;
- consequence reach.

## 7.4 Actor authority

Do not create a second politics engine.

Use current faction/war/standing owners.

If plan defines neighbour actor record:
- place it under owning world/faction authority;
- stable ID;
- minimal state.

No speculative actor classes beyond plan.

## 7.5 Autonomous tick

Day-keyed.

Inputs:
- current faction/region/world state;
- seeded RNG fork if plan requires stochastic transition;
- current season/economy if plan names.

No wall clock.

## 7.6 RNG registry

Use dedicated registered stream where appropriate.

Properties:
- stable stream ID;
- independent from player-visible unrelated RNG;
- same seed/state → same transition sequence;
- save/restore continues same trajectory.

## 7.7 Transition contract

Read exact plan.

Examples could include:
- relation shift;
- conflict/escalation;
- territorial/control change;
- economic action.

Do not invent extra political model.

## 7.8 Projection reach

Every autonomous transition required by plan must reach at least one existing consumer.

Audit:

`Actor transition | Owner | Projection event/state | Consumer | Player-visible effect`.

Possible consumers:
- radio;
- market shock/ticker;
- encounter selection;
- map/world knowledge.

No new panel unless named.

## 7.9 No invisible autonomy

If a transition changes hidden state but no plan-required player consumer can observe it:
- gap remains;
- wire projection or retire with signed reason.

## 7.10 Save plan

Prefer existing owner state.

If a new actor-state ledger is required by plan:
- additive/versioned;
- old-save neutral initial state;
- round-trip;
- mid-autonomy restore.

## 7.11 Autonomy replay test

Seeded window:
1. initialize factions/world;
2. advance days;
3. record transitions/projections;
4. save midpoint;
5. continue;
6. restore;
7. compare suffix;
8. fingerprint.

## 7.12 Projection tests

Each transition class:
- causes named projection once;
- projection consumed;
- no duplicate on restore;
- no direct UI mutation from autonomy owner.

## 7.13 30-day balance guard

Run existing 30-day windows.

Measure:
- pressure shifts;
- encounter/economy activity;
- no runaway difficulty.

Findings are routed to balance memo, not tuned inline.

## 7.14 UI/content

Use existing:
- faction radio;
- market;
- encounter surfaces.

Content authoring only if plan’s mechanism sample is necessary; no broad faction prose tranche.

## 7.15 Implementation log

Centerpiece:

`Actor | Current owner | Transition | Projection | Consumer | Save | RNG stream | Test`.

## 7.16 Verification

- faction/war suites;
- ecology/world;
- market;
- encounter;
- radio if projections;
- save round-trip;
- replay/fingerprint;
- 30-day window;
- integrity/utilization where data/content touched;
- build;
- verify-fast.

## 7.17 Non-goals

- no second politics engine;
- no un-gated balance tuning;
- no speculative actors;
- no new world panel unless plan requires;
- no hidden wall-clock simulation.

---

# 8. TASK B2 — C2[11] “ONE PLACE AUTHORITY, GRAPH-NATIVE TRAVEL, GEOGRAPHIC KNOWLEDGE”

## 8.1 Objective

Reconcile the plan against current map/place/travel/knowledge systems, determine whether the “one place authority” and graph-native travel demand are already satisfied or require consolidation, execute only bounded current remainders, and promote any broad consolidation/rewrite behind an explicit signed proposal.

## 8.2 Entry gate

Read:
- C2[11];
- census row;
- current location/place owner(s);
- region/sector/waystation representations;
- route/travel system;
- embargo/caravan routes;
- geographic knowledge/discovery;
- map panels/routes;
- save owners.

## 8.3 Place-authority census

Build:

`Place concept | Owner | ID type | Save owner | Consumers | Overlap with other concept`.

Examples:
- location;
- sector;
- region;
- waystation.

Determine whether these are:
- intentionally different layers;
- duplicated authority.

Do not consolidate merely because names differ.

## 8.4 Consolidation decision rule

If multiple systems genuinely own the same semantic fact:
- create signed consolidation proposal;
- include all consumers;
- migration plan;
- save compatibility;
- no inline rewrite.

If current layering satisfies plan:
- close sub-scope with evidence.

## 8.5 Graph-native travel audit

Map current representation:
- nodes;
- edges/routes;
- costs;
- gates;
- hazards;
- embargo;
- knowledge constraints.

Compare to plan.

If current routes are already graph-native:
- cite and close.

If not:
- determine whether plan demands rewrite or only graph abstraction around existing routes.

## 8.6 Travel consolidation guard

No broad route rewrite without signed proposal.

A migration touching:
- all routes;
- save IDs;
- map UI;
- caravan;
- expeditions
is not a “delta”.

Promote.

## 8.7 Geographic knowledge

Audit:
- discovery;
- reveal;
- map knowledge;
- cartography;
- encounter/travel gating.

Plan remainder may require knowledge to influence:
- route visibility;
- route legality;
- encounter knowledge;
- map detail.

Use exact contract.

## 8.8 Knowledge authority

One owner for knowledge state.

Travel reads it; map reads it.

Do not store separate “known route” booleans in UI.

## 8.9 Save/determinism

If executing bounded wiring:
- no new save if knowledge already persists;
- route calculations deterministic;
- same knowledge state → same graph accessibility;
- restore parity.

## 8.10 Tests

Depending on remainder:
- place identity consistency;
- duplicate owner detection;
- graph route reachability;
- known/unknown route gating;
- discovery unlock;
- restore;
- deterministic route costs;
- embargo/hazard composition unchanged.

## 8.11 Verdict table

For each sub-scope:

`Sub-scope | Historical demand | Current architecture | Verdict | Evidence | Executed delta / promoted proposal`.

Verdicts:
- SATISFIED;
- EXECUTED-HERE;
- CONSOLIDATION-PROPOSAL;
- DEPENDENCY-BLOCKED;
- STALE.

## 8.12 Implementation log

Use verdict table as centerpiece.

## 8.13 Verification

- map/travel suites;
- knowledge/discovery;
- route gates;
- caravan/expedition where touched;
- save;
- replay/fingerprint;
- build;
- verify-fast.

## 8.14 Non-goals

- no ungated consolidation;
- no travel rewrite without proposal;
- no new place authority beside current owners;
- no UI-local knowledge state.

---

# 9. CROSS-TASK AUTHORITY MAP

| Task | Primary existing authority | New state allowed? | Main integration risk |
|---|---|---|---|
| A1 | calendar/clock | fired-deadline ledger only if needed | parallel due-date system |
| A2 | memory/heirloom/memorial/place owners | derived wiring; no duplicate memory | display-only masquerading as behavior |
| A3 | leadership/roster/grievance | only plan-required policy/ledger | silent consent/refusal |
| A4 | existing acceptance gates | no new criteria | duplicate validation logic |
| A5 | existing mod loader | manifest fields per plan | loader fork/nondeterminism |
| B1 | faction/war/world owners | actor state only if plan requires | second politics engine |
| B2 | existing place/travel/knowledge owners | only bounded delta | broad unsafe consolidation |

---

# 10. SAVE / REPLAY POLICY

## A1
Deadline fired ledger exactly once.

## A2
Memory behavior derived from existing persisted memory.

## A3
Policy/refusal state persists only under owning systems.

## A4
No game save impact.

## A5
Game save impact only if mod contract already tracks mod set; no speculative save design.

## B1
Autonomy state persists through existing world/faction owners or one plan-authorized actor ledger.

## B2
Place/knowledge state preserves current save ownership.

Continuous vs restored runs must match where stateful changes are made.

---

# 11. CONTENT / DATA POLICY

## A1
Mechanism sample deadlines only.

## A2
No prose rewrite.

## A3
No broad policy-content tranche.

## A4
No content authoring; acceptance orchestration only.

## A5
Sample mod/pack fixture only.

## B1
Mechanism sample projections only.

## B2
No broad map/content rewrite.

All data rows:
- validate;
- resolve references;
- have consumer;
- pass utilization where applicable.

---

# 12. IMPLEMENTATION LOG STANDARD

Every task log contains:

1. plan identity;
2. source baseline;
3. dependency status;
4. historical premise;
5. current premise;
6. census row;
7. clause matrix;
8. sealed-elsewhere evidence;
9. executed remainder;
10. claims;
11. files;
12. save/determinism;
13. focused tests;
14. broader gates;
15. acceptance;
16. remaining blocker;
17. census/ledger update;
18. next chain node.

---

# 13. COMMIT BOUNDARIES

## A1
1. premise/deadline audit;
2. catalog + validator;
3. evaluator/ledger;
4. consequence routing;
5. surface;
6. log/census.

## A2
1. acts matrix;
2. heirloom/memorial wiring;
3. place/revisit wiring;
4. tests;
5. log/census.

## A3
1. leadership/policy/refusal audit;
2. typed command results;
3. consequence wiring;
4. UI;
5. log/census.

## A4
1. rung truth pass;
2. pipeline orchestrator;
3. CI wiring;
4. docs/baseline;
5. log/census.

## A5
1. compatibility audit;
2. manifest/version rules;
3. pack semantics;
4. determinism replay;
5. contract/log.

## B1
1. autonomy audit;
2. actor driver;
3. projections;
4. save/replay;
5. 30-day guard;
6. log.

## B2
1. place/travel/knowledge audit;
2. bounded delta;
3. proposal if broad;
4. tests;
5. log.

---

# 14. ROLLBACK / ROUTING MATRIX

| Task | Finding | Required action |
|---|---|---|
| A1 | consequence owner absent | decision/package, no new authority |
| A1 | existing generic fired ledger sufficient | reuse, no new save section |
| A2 | memory display-only by design | close clause, do not invent effect |
| A2 | behavioral effect requires balance decision | decision-block |
| A3 | refusal semantics undefined | decision-block |
| A3 | assignment owner actively claimed | claim-block |
| A4 | existing rung red on current corpus | route finding, do not weaken gate |
| A4 | plan requires new criterion not currently defined | decision/package |
| A5 | compatibility range semantics absent | stop for plan/decision reading |
| A5 | loader order nondeterministic | fix under existing loader |
| B1 | hard dependency open | dependency-block |
| B1 | autonomy causes balance drift | route balance memo |
| B2 | multiple place owners intentionally layered | no consolidation |
| B2 | consolidation broad | signed proposal/promotion |

---

# 15. MASTER ACCEPTANCE MATRIX

| Task | Blocker | Mandatory close proof |
|---|---|---|
| A1 | calendar/deadline mechanism absent | deadline fire/miss, exactly-once, save, consequence routing |
| A2 | memory partially passive | memory→behavior matrix with measurable effects |
| A3 | leadership partial | consent/refusal typed outcomes + consequences |
| A4 | ladder advisory/partial | pipeline gate composes existing rungs and baseline run |
| A5 | mod governance partial | compatibility + packs + deterministic overlay proof |
| B1 | outside-world autonomy absent | autonomous transitions + player projections + replay |
| B2 | place/travel/knowledge partial | verdict table, bounded deltas, consolidation proposal if needed |

---

# 16. WAVE 11 PART 1 CLOSEOUT

Before Part 1 closes:

1. A1 exact plan dependencies resolved.
2. Deadline mechanism either sealed or correctly blocked.
3. Deadline save semantics proven.
4. A2 every memory sub-scope classified.
5. No new memory authority.
6. A3 leadership/policy/refusal matrix terminal.
7. No silent refusal/consent.
8. A4 rung truth pass complete.
9. Pipeline command/gate installed if plan requires.
10. Current corpus baseline executed.
11. A5 loader remainder reconciled.
12. Modded replay deterministic.
13. B1 DAG readiness proven before execution.
14. Every autonomous transition has projection.
15. 30-day drift guard run.
16. B2 place-authority verdict recorded.
17. No broad consolidation executed without proposal.
18. Every task has implementation log.
19. Census updated after each task.
20. Integration ledger current.
21. Build green.
22. Verify-fast green.
23. Integrity/utilization green where applicable.
24. Save/replay gates green.
25. Wave 11 Part 2 generated only from latest census/DAG.

---

# 17. PART 1 NON-GOALS

- no C1[16] depth pass yet;
- no C2[12]+ execution yet;
- no new deadline engine separate from calendar;
- no new memory authority;
- no democracy simulation beyond plan;
- no new content acceptance criteria;
- no mod manager UI;
- no Steam Workshop;
- no second politics engine;
- no unsafe place/travel consolidation;
- no inline balance tuning.




# APPENDIX A — A1 DEADLINE EXECUTION CONTROL SHEETS

## A.1 Existing deadline-adjacent systems audit

| Mechanic | Owner | Day basis | Terminal state | Exactly-once? | Save owner | Should migrate? |
|---|---|---|---|---|---|---|
| loan due date |  |  |  |  |  | no unless plan |
| embargo window |  |  |  |  |  | no unless plan |
| distress follow-up |  |  |  |  |  | no unless plan |
| stage day range |  |  |  |  |  | no unless plan |
| quest due window |  |  |  |  |  |  |

The audit prevents the shared calendar from absorbing owners it does not need to own.

## A.2 Deadline row audit

| ID | Window | Condition | Consequence class | Target | Warning | Reachable? | Test |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

## A.3 Fire-state matrix

| State | Before window | Inside window | At close | After terminal |
|---|---|---|---|---|
| unmet | inactive | active | missed fires | terminal |
| met | inactive | met if condition | no miss | terminal |

Use exact plan semantics.

## A.4 Same-day repeated evaluation
Call evaluator repeatedly on same day:
- no duplicate event;
- no duplicate consequence;
- no ledger growth.

## A.5 Save before close
- active deadline;
- save;
- restore;
- meet/miss;
- exactly one consequence.

## A.6 Save after close
- terminal;
- save;
- restore;
- evaluator no-ops.

## A.7 Old save
No ledger:
- defaults empty;
- only future/current deadlines evaluate;
- no retroactive mass firing unless plan explicitly requires.

## A.8 Consequence routing matrix

| Consequence | Owner | Command/event | State moved | UI surface | Test |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## A.9 Ordering
Multiple deadlines same day:
- stable ID order or plan-defined priority;
- deterministic event order.

## A.10 Claim-safe briefing
If briefing claimed:
- evaluator lands;
- read model lands if safe;
- builder consumer deferred with exact path.

---

# APPENDIX B — A2 MEMORY “ACTS” CONTROL SHEETS

## B.1 Acts matrix

| Memory | Produced by | Persisted by | Current display | Current behavior | Plan-required effect | Consumer | Result |
|---|---|---|---|---|---|---|---|
| memorial |  |  |  |  |  |  |  |
| heirloom |  |  |  |  |  |  |  |
| place |  |  |  |  |  |  |  |
| generational |  |  |  |  |  |  |  |

## B.2 Heirloom effect checks
- possession/ownership canonical;
- effect bounded;
- modifier stack reused;
- no double contribution;
- old save behavior;
- transfer/loss updates effect.

## B.3 Memorial behavior checks
- existing mourning actions credited;
- no duplicate memorial consequence;
- exactly-once where relevant.

## B.4 Place-memory checks
- memory recorded;
- revisit recognized;
- encounter/decision consumer uses it if plan requires;
- no UI-local place memory.

## B.5 Generational checks
- lineage state existing;
- inherited memory/heirloom semantics per plan;
- no second genealogy.

## B.6 Restore parity
For each memory:
- save;
- fresh restore;
- effect recomputed;
- no duplicate.

## B.7 Neutral parity
Memory absent/unbound:
- existing behavior unchanged.

---

# APPENDIX C — A3 LEADERSHIP / REFUSAL CONTROL SHEETS

## C.1 Leadership remainder matrix

| Clause | Current `LeadershipSystem` support | Current consumer | Remainder | Owner | Test |
|---|---|---|---|---|---|
| succession |  |  |  |  |  |
| policy |  |  |  |  |  |
| consent |  |  |  |  |  |
| refusal |  |  |  |  |  |
| consequence |  |  |  |  |  |

## C.2 Typed result contract

A refusal result should expose stable identity, such as repository-equivalent:
- result code;
- reason ID;
- optional actor ID.

Do not encode refusal only in display text.

## C.3 No-silent-outcome invariant
Command must return:
- success;
- refusal;
- invalid/preflight failure.

No ambiguous no-op.

## C.4 Auto-assignment
If assignment automation exists:
- refuses according to plan;
- does not silently force;
- may skip/refill according to existing roster policy.

## C.5 Grievance consequence
If refusal generates grievance:
- use grievance owner;
- exactly once;
- restore stable.

## C.6 UI dialog
- reason;
- confirm/cancel where relevant;
- focus;
- keyboard;
- no color-only status.

## C.7 Replay
Governance window same seed:
- same assignments/refusals;
- same consequences;
- restore suffix identical.

---

# APPENDIX D — A4 CONTENT ACCEPTANCE PIPELINE CONTROL SHEETS

## D.1 Rung inventory

| Rung | Class/enum | Current criterion | Existing gate command | Advisory/gating | Still valid? |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## D.2 Pipeline order
Record exact plan order.

## D.3 Orchestrator contract
- invoke existing gates;
- preserve exit/error;
- identify failed rung;
- deterministic order;
- no criterion duplication.

## D.4 Failure fixtures
Use test-only fixture/batch:
- invalid reference;
- dead content;
- canon mismatch;
- quality fail only if current quality gate supports deterministic fixture.

Do not add bad rows to production data.

## D.5 Baseline current-corpus result

| Rung | Result | Findings | Owner routed | Blocks gate? |
|---|---|---|---|---|
|  |  |  |  |  |

## D.6 CI manifest
- one entry;
- dependency position correct;
- local command same behavior;
- no duplicate underlying gates.

## D.7 Documentation
Content author can answer:
- what command;
- what order;
- what failure means;
- how to fix;
- what constitutes accepted batch.

---

# APPENDIX E — A5 MOD COMPATIBILITY CONTROL SHEETS

## E.1 Loader current contract

| Concern | Current behavior | Source | Plan requirement | Delta |
|---|---|---|---|---|
| manifest |  |  |  |  |
| schema version |  |  |  |  |
| game version |  |  |  |  |
| prefix |  |  |  |  |
| override order |  |  |  |  |
| traversal |  |  |  |  |
| pack |  |  |  |  |

## E.2 Compatibility range tests
- exact compatible;
- lower boundary;
- upper boundary;
- too low;
- too high;
- malformed;
- missing old field.

Use exact version semantics from plan.

## E.3 Typed rejection
Every rejected mod:
- reason;
- mod ID;
- field if useful.

No silent ignore.

## E.4 Overlay order
Test:
- mods A/B same base target;
- load in defined order;
- merged result expected;
- repeated load identical.

## E.5 Filesystem nondeterminism
Enumerate mods/catalogs through stable sorting/manifest order.
Never OS directory order.

## E.6 Pack acceptance
Pack:
- manifest;
- catalog list;
- A4 pipeline;
- loader only accepts according to plan.

## E.7 Modded replay
Run N repeated:
- same mod set;
- same seed;
- same fingerprint.

## E.8 Existing mod tests
Must remain green unchanged except intentional contract additions.

---

# APPENDIX F — B1 AUTONOMOUS OUTSIDE WORLD CONTROL SHEETS

## F.1 Current moving-world inventory

| Existing system | Autonomous today? | Owner | Save | Player projection | C2[10] overlap |
|---|---|---|---|---|---|
| faction war |  |  |  |  |  |
| faction standing |  |  |  |  |  |
| wildlife migration |  |  |  |  |  |
| anomaly movement |  |  |  |  |  |
| market shocks |  |  |  |  |  |

## F.2 Plan remainder
Do not call existing motion “missing”.
Identify exact neighbour/off-screen coherence.

## F.3 Actor matrix

| Actor | Existing owner | State | Day transition | RNG stream | Projection | Consumer |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## F.4 Transition invariants
- day-keyed;
- bounded;
- legal state transition;
- stable seeded outcome;
- no duplicate transition on restore.

## F.5 Projection invariants
Every plan-required transition:
- projection emitted;
- existing consumer receives;
- player can observe consequence;
- exactly once.

## F.6 Save midpoint
- run autonomy;
- save;
- continue;
- restore;
- identical suffix.

## F.7 30-day drift report

| Metric | Baseline | With autonomy | Delta | Expected? | Action |
|---|---:|---:|---:|---|---|
| economy pressure |  |  |  |  |  |
| encounter pressure |  |  |  |  |  |
| faction events |  |  |  |  |  |

No inline balance adjustment.

---

# APPENDIX G — B2 PLACE/TRAVEL/KNOWLEDGE CONTROL SHEETS

## G.1 Place authority inventory

| Concept | Type/owner | Stable ID | Persisted | Consumers | Duplicate semantic fact? |
|---|---|---|---|---|---|
| location |  |  |  |  |  |
| sector |  |  |  |  |  |
| region |  |  |  |  |  |
| waystation |  |  |  |  |  |

## G.2 Layer vs duplicate test

Different representations are legitimate if:
- different semantic granularity;
- explicit mapping;
- one owner per fact.

Duplicate if:
- same fact independently writable;
- divergent IDs/state;
- competing consumers.

## G.3 Graph-native audit

| Graph concept | Current representation | Plan demand | Satisfied? | Delta |
|---|---|---|---|---|
| node |  |  |  |  |
| edge |  |  |  |  |
| cost |  |  |  |  |
| gate |  |  |  |  |
| hazard |  |  |  |  |
| embargo |  |  |  |  |
| knowledge |  |  |  |  |

## G.4 Knowledge gating
Tests:
- unknown place/route;
- discovered place;
- known route;
- knowledge lost/changed only if system supports;
- save/restore;
- UI visibility follows owner.

## G.5 Consolidation proposal packet
If needed:
- duplicated owners;
- all consumers;
- migration target;
- save impact;
- ID mapping;
- phases;
- parity gates;
- rollback;
- signature.

No migration before approval.

---

# APPENDIX H — IMPLEMENTATION LOG TEMPLATE

# `<CORPUS KEY>` IMPLEMENTATION LOG

## Identity
**Plan file:**<br>
**Title:**<br>
**Source baseline:**<br>

## Census
**Previous status:**<br>
**Current row:**<br>

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

## Save / determinism
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

# APPENDIX I — NEGATIVE-CASE REGISTRY

## I.1 A1
- deadline window impossible;
- unresolved consequence;
- met and missed both fire;
- repeated tick;
- old save;
- same-day multiple deadlines;
- claimed briefing builder.

## I.2 A2
- memory exists but no consumer;
- double modifier;
- lost heirloom leaves effect;
- revisit without memory;
- restore duplicates memorial effect.

## I.3 A3
- refusal no reason;
- refusal still commits assignment;
- auto-assignment bypass;
- grievance double fires;
- restore changes refusal outcome.

## I.4 A4
- first rung red;
- middle rung red;
- duplicate gate invocation;
- current corpus contains existing red;
- retired rung.

## I.5 A5
- version incompatible;
- malformed manifest;
- filesystem order changes overlay;
- pack fails acceptance;
- modded replay diverges.

## I.6 B1
- actor transition has no projection;
- same seed different transition;
- restore diverges;
- projection duplicate;
- 30-day pressure runaway.

## I.7 B2
- same place fact writable by two owners;
- route hidden but knowledge says known;
- save loses discovery;
- graph route differs by unordered iteration;
- proposed migration too broad.

---

# APPENDIX J — WAVE 11 PART 2 PROMOTION FILTER

Part 2 is generated from the post-Part-1 census.

Source preview names:
- C1[16] depth passes;
- C2[12] difficulty/completion record;
- C2[13] port contracts;
- C2[14] duplicate Goods Must Arrive reconciliation;
- later C1/C2 corpus.

But inclusion requires re-verification.

For each candidate:

| Rank | Corpus | Current status | Remainder | Dependencies | Claims | Decision | Acceptance |
|---:|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

Rules:
1. duplicate plans reconcile before execution;
2. dependencies terminal;
3. premise current;
4. remainder bounded;
5. no active claim;
6. decision signed;
7. highest DAG-unlock value wins.

No numeric-file automatic progression.

---

# APPENDIX K — FINAL REVIEW QUESTIONS

## A1
- Is calendar authority reused?
- Are deadlines authored, reachable, exactly-once?
- Do consequences route to real owners?
- Does restore avoid refire?

## A2
- Does every required memory actually change behavior?
- Are existing memory owners preserved?
- Are derived effects unpersisted?
- Is neutral parity proven?

## A3
- Are consent/refusal typed?
- Can automation bypass?
- Are consequences owned elsewhere?
- Is refusal human-readable without becoming UI logic?

## A4
- Are rungs existing criteria only?
- Is pipeline composition thin?
- Does failure identify rung?
- Did current corpus baseline reveal findings honestly?

## A5
- Are compatibility semantics plan-defined?
- Is overlay deterministic?
- Does pack reuse loader?
- Does A4 pipeline gate packs?

## B1
- Is existing moving-world work credited?
- Are autonomous transitions plan-defined?
- Does every transition reach player?
- Is 30-day drift routed, not tuned?

## B2
- Are different place layers actually duplicates?
- Is travel already graph-native?
- Does knowledge gate current travel correctly?
- Is broad consolidation promoted for signature?

---

# APPENDIX L — FINAL NO-FALSE-CLOSURE RULES

Wave 11 Part 1 is not complete if:

- A1 creates a second deadline authority;
- deadline consequences only log and do not reach their owner;
- A2 adds narrative display but no behavioral consumer where the plan requires one;
- A3 refusal exists only as UI text;
- A4 merely defines rung classes without making the plan-required gate executable;
- A5 keeps compatibility governance as documentation only when the loader must enforce it;
- B1 adds hidden autonomous state with no player projection;
- B1 silently tunes difficulty after 30-day drift;
- B2 consolidates place/travel authorities without signed scope;
- any implementation log is missing;
- census/ledger remain stale after task close;
- Part 2 queue is generated from the preview instead of current DAG.

**Final invariant:** every Wave 11 Part 1 package either lands the verified remainder through the current owner, proves that the remainder was already sealed, or stops at an explicit dependency/claim/decision boundary. Ambiguity is not a terminal state.

# APPENDIX M — A1 DEADLINE MECHANISM IMPLEMENTATION PACKET

## M.1 Current calendar reality audit

Before coding, populate:

| Concern | Current owner | Current API/state | Persisted? | C1[11] requirement | Delta |
|---|---|---|---|---|---|
| campaign day |  |  |  |  |  |
| season derivation |  |  |  |  |  |
| authored scheduled events |  |  |  |  |  |
| loan deadlines |  |  |  |  |  |
| embargo windows |  |  |  |  |  |
| follow-up scheduler |  |  |  |  |  |
| deadline fired keys |  |  |  |  |  |

The table exists to prevent the deadline mechanism from absorbing unrelated owners.

## M.2 Deadline lifecycle state machine

Use the exact plan states. A minimal model may resemble:

`FUTURE -> ACTIVE -> MET`
`FUTURE -> ACTIVE -> MISSED`

Optional plan-defined states are allowed only when sourced from C1[11].

### Invariants
- terminal state never returns to ACTIVE;
- MET and MISSED mutually exclusive;
- one deadline ID reaches one terminal event once;
- a save made after terminal state restores terminal state;
- evaluation of a terminal deadline is no-op.

## M.3 Window boundary tests

Test:
- day before open;
- first active day;
- middle day;
- final valid day;
- first missed day;
- condition satisfied exactly on final valid day.

Boundary behavior must come from plan.

## M.4 Multiple conditions

If plan supports only one condition per deadline:
- do not create expression language.

If multiple conditions are supported:
- use exact composition semantics;
- deterministic order;
- explicit AND/OR structure;
- validate empty condition set.

## M.5 Consequence execution contract

A deadline evaluator should return/emit a typed resolution rather than calling arbitrary systems through hardcoded switch logic if the project has a current routing pattern.

Document:
`deadline ID -> consequence reference -> owner -> command/event -> result`.

If consequence owner rejects:
- follow plan’s semantics;
- do not mark successful consequence when owner rejected.

## M.6 Exactly-once and failed consequence

If the deadline becomes terminal but downstream consequence fails due to an unexpected runtime error:
- do not invent retry semantics.
Read current event/exactly-once policy.
Potentially route the failure as repair rather than silently firing repeatedly.

## M.7 Player warning timing

If warning lead time exists:
- warning is derived from current day/window;
- warning does not mutate terminal state;
- repeated reads safe;
- expired/terminal deadline no longer warns.

## M.8 Briefing navigation

If Plan 31 navigation exists:
- deadline warning may route to source owner/detail.
The deadline system does not own route.

## M.9 Catalog evolution

Adding a new deadline row should require:
- valid ID;
- valid window;
- valid consequence;
- valid vocabulary;
- utilization.

The mechanism should not need code edits for each authored row unless consequence class is genuinely new.

## M.10 Deadline content sample criteria

Samples should cover distinct mechanism branches, not content breadth:
- one met;
- one missed;
- one warning.

No more unless plan explicitly requires.

## M.11 Save DTO review

Before adding field:
- inspect existing calendar/campaign save DTO;
- inspect versioning conventions;
- add at owning layer;
- old missing field = empty ledger;
- unknown future deadline ID in old save handled per repository ID policy.

## M.12 Restore reconstruction

Derived active deadline state recomputes from:
- current day;
- catalog;
- fired ledger.

Do not persist “days remaining” redundantly.

## M.13 Deadline implementation closeout table

| Requirement | Before | After | Save | Determinism | Test | Status |
|---|---|---|---|---|---|---|
| authored deadline |  |  |  |  |  |  |
| met resolution |  |  |  |  |  |  |
| missed resolution |  |  |  |  |  |  |
| warning |  |  |  |  |  |  |
| consequence routing |  |  |  |  |  |  |
| exactly-once |  |  |  |  |  |  |

---

# APPENDIX N — A2 MEMORY-CONSUMER EXECUTION PACKET

## N.1 Memory ownership audit

For every memory form:

| Memory | Writer | Save state | Reader(s) | Current player surface | Current behavioral consumer |
|---|---|---|---|---|---|
| memorial |  |  |  |  |  |
| heirloom |  |  |  |  |  |
| place memory |  |  |  |  |  |
| generational link |  |  |  |  |  |

A memory with no behavioral consumer is not automatically a bug; compare with exact plan clause first.

## N.2 Bounded-contribution rule

When memory contributes to a numeric system:
- use current modifier mechanism;
- one stable contribution;
- bounded according to plan/current policy;
- visible attribution if contributor UI exists;
- removal/loss recalculates.

No direct mutation sprinkled across day tick.

## N.3 Heirloom consumer cases

Potential plan-supported consumers may include:
- morale;
- relationship;
- survivor stat/modifier.

Implement only named contract.

Test:
- survivor holds heirloom;
- effect present;
- heirloom transferred/lost;
- effect moves/disappears;
- restore.

## N.4 Memorial consumer cases

If memorial already supports mourning:
- credit.

If additional C1[12] behavior:
- identify exact event;
- route through existing relationship/morale owner.

No repeated daily grief bonus unless plan defines.

## N.5 Place-memory key

Use existing place/location stable ID.

Do not store display name as identity.

## N.6 Revisit behavior

If plan says prior memory alters revisit:
- encounter authority reads memory;
- no place-memory-owned RNG;
- same seed/state/memory → deterministic altered selection;
- no memory → legacy parity.

## N.7 Memory decay

Only if plan defines decay.
Do not invent forgetting mechanics.

## N.8 Generational transfer

If plan says memory/heirloom transfers across generations:
- use current 19B link;
- define exactly what transfers;
- no entire narrative state copy unless plan says.

## N.9 Text generation

Memory-driven text uses:
- existing templates/catalogs;
- stable IDs;
- current localization.

Do not generate dynamic unbounded prose in domain code.

## N.10 Memory-heavy journey

Scenario:
1. create/loss event;
2. memorial recorded;
3. obtain/transfer heirloom if supported;
4. visit/revisit place;
5. save;
6. restore;
7. confirm all behavioral effects;
8. compare fingerprint.

## N.11 Consumer failure handling

If consumer cannot resolve memory target:
- follow owner’s missing-reference semantics;
- no crash;
- integrity should prevent authored bad reference.

## N.12 A2 closeout table

| Memory | Sealed prior behavior | New consumer | Effect | Save owner | Neutral parity | Test |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

---

# APPENDIX O — A3 CONSENT / REFUSAL IMPLEMENTATION PACKET

## O.1 Command-boundary rule

Consent/refusal belongs at the domain command/preflight seam, never as a panel-only confirmation.

A UI dialog may ask, but the owner validates again on commit.

## O.2 Refusal result design

Use existing repository result conventions.

Must include:
- success/failure/refusal category;
- stable reason;
- actor/survivor reference if needed.

Presentation text maps from reason.

## O.3 Candidate refusal inputs

Only include plan-defined/current source inputs:
- injury;
- exhaustion;
- grievance;
- dangerous assignment;
- relationship/leadership state;
- policy conflict.

Each input remains owned by its system.

## O.4 Refusal precedence

If multiple reasons apply:
- use plan/current preflight precedence;
- stable deterministic ordering;
- test tie/multiple reasons.

Do not depend on unordered collection.

## O.5 Consent state

If plan defines explicit consent ledger:
- identify owner;
- persist only if future behavior depends on historical consent.

If one-shot command result:
- keep transient.

## O.6 Forced override

If plan supports leader forcing assignment:
- requires explicit command/path;
- consequences routed through grievance/morale owner;
- never silently invoked by auto-assignment.

If plan does not support force:
- refusal terminal for that command.

## O.7 Auto-assignment behavior

Possible safe behavior:
- skip refusing survivor;
- select next candidate;
- return incomplete assignment state.

Use current roster contract.

No hidden forced assignment.

## O.8 Policy cadence

If policy changes at defined cadence:
- use calendar/day authority;
- no wall clock;
- persist active policy under owner;
- old save neutral.

## O.9 Consequence test

Example pattern:
- setup refusal condition;
- attempt assignment;
- assert no assignment;
- assert typed refusal;
- assert grievance/standing consequence once if plan says;
- save/restore;
- retry according to current state.

## O.10 Leadership transition

If leader changes:
- verify policy/refusal authority remains consistent;
- no stale leader ID driving result.

## O.11 UI stale-state revalidation

Open dialog, then mutate underlying state through test seam before confirmation:
- domain command revalidates;
- UI handles changed result.

## O.12 A3 closeout table

| Command | Consent/refusal input | Result | Consequence | Persistence | UI | Test |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

---

# APPENDIX P — A4 ACCEPTANCE-PIPELINE IMPLEMENTATION PACKET

## P.1 Gate inventory exactness

The pipeline may only orchestrate gates the plan names and that current source supports.

Never create a “quality” rung whose implementation is subjective/non-deterministic unless the plan already defines a deterministic tool/check.

## P.2 Gate adapter

If existing checks have different command interfaces:
- thin adapter allowed;
- preserve their exit codes/messages;
- do not move validator logic.

## P.3 Batch descriptor

If pipeline operates on changed content batch:
- define current repository’s batch/manifest input;
- paths deterministic;
- no arbitrary directory scan order.

## P.4 Full-corpus mode

If required for baseline:
- explicit command flag;
- not necessarily default CI fast path.

## P.5 Failure report

Example fields:
- rung;
- command/check;
- item/path;
- original failure message;
- remediation hint only if existing gate provides one.

## P.6 Short-circuit tests

If fail-fast:
- rung 1 fails, rung 2 not invoked;
- rung 2 fails after rung 1 passes;
- all pass.

Use spies/fakes only around command runner, not validators.

## P.7 Gate duplication audit

Check current CI already runs integrity/utilization separately.

If pipeline is added to same CI:
- avoid running duplicates if plan expects replacement/composition.
- preserve any gate required in other independent release stages.

## P.8 Existing-red policy

If current corpus fails a rung:
- pipeline is still valid;
- baseline report becomes blocker finding.
Do not change rung thresholds inside A4.

## P.9 Content author workflow

Document:
1. create batch;
2. run pipeline;
3. inspect first failing rung;
4. fix;
5. rerun;
6. merge when green.

## P.10 Pack workflow handoff

A5 can call same pipeline for pack acceptance.

No duplicated mod-specific acceptance ladder.

## P.11 Pipeline observability

Print:
- rung start;
- rung result;
- elapsed time if current tooling convention;
- final status.

Do not drown CI in per-row debug unless failure.

## P.12 A4 closeout table

| Rung | Existing owner | Existing command | Pipeline position | Failure test | Baseline result |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

---

# APPENDIX Q — A5 MOD / CONTENT-PACK GOVERNANCE PACKET

## Q.1 Compatibility matrix model

Use exact plan-defined semantics.

Illustrative structure:
`Game version | Mod compatibility range | Schema version | Accept? | Reason`.

Do not invent semantic-version interpretation if current project uses another scheme.

## Q.2 Manifest parser behavior

Missing optional fields:
- default according to backward-compat contract.

Unknown fields:
- follow existing parser policy.

Invalid fields:
- typed rejection.

## Q.3 Stable mod identity

Use canonical mod ID.
Do not identify by filesystem folder name if manifest provides ID.

## Q.4 Overlay set identity

A deterministic mod set should be representable by:
- mod IDs;
- versions;
- order if order is meaningful.

This can be used in replay diagnostics if current harness permits.

## Q.5 Overlay precedence tests

Create two sample mods targeting same valid override:
- order A then B;
- expected winner;
- repeat;
- reverse if loader permits explicit order and assert reversed according to contract.

## Q.6 Content-pack manifest

If plan defines:
- pack ID;
- version;
- catalog members;
- compatibility;
- dependency list if plan supports.

No package dependency system unless specified.

## Q.7 Acceptance failure

Pack failing A4:
- rejected before runtime merge according to plan;
- original rung failure surfaced.

## Q.8 Modded save

If save stores only merged state:
- confirm replay/load semantics.

If contract records mods:
- test mismatch behavior.

No new behavior beyond plan.

## Q.9 Mod integrity

Run current data integrity after overlay, not only base data.

## Q.10 Replay stability

Record:
`mod set fingerprint + campaign seed + final state fingerprint`.

Repeat N times.

## Q.11 Contract versioning

When `MOD_CONTRACT.md` changes:
- record compatibility contract version if current project does so;
- update sample fixtures;
- do not silently break external expectations.

## Q.12 A5 closeout table

| Contract axis | Existing behavior | New enforcement | Backward compatibility | Test |
|---|---|---|---|---|
|  |  |  |  |  |

---

# APPENDIX R — B1 AUTONOMY IMPLEMENTATION PACKET

## R.1 Off-screen actor state

Only fields required by plan.

Each field must have:
- owner;
- transition rule;
- save behavior;
- player consequence relevance.

Avoid rich simulation for simulation’s sake.

## R.2 Day tick ordering

Map autonomous tick relative to:
- weather;
- economy;
- faction events;
- encounter generation;
- radio reporting.

Use current coordinator ordering.

If order matters and undefined:
- decision/architecture review rather than arbitrary placement.

## R.3 RNG stream independence

Autonomy stream must not perturb:
- loot;
- combat;
- unrelated encounters;
unless current RNG architecture intentionally shares.

Register stream.

## R.4 Transition event

Prefer existing faction/world events.

A transition result should be readable by projection consumers.

## R.5 Projection latency

Read plan:
- same-day;
- next briefing;
- next market restock;
- next encounter.

Do not assume immediate UI update.

## R.6 Market projection

If transition affects market:
- use existing shock/economy seam;
- no direct price mutation from politics owner.

## R.7 Encounter projection

Use encounter conditions/weights.
No separate politics encounter generator.

## R.8 Radio projection

Use radio content/event system.
No direct UI string from simulation owner unless current architecture uses typed event-to-text mapping.

## R.9 Knowledge projection

If player must discover off-screen event:
- use existing knowledge/intel availability.
Do not expose omniscient world state unless plan says.

## R.10 Restore sequence

Save mid-window:
- actor states;
- RNG state/seed semantics;
- projection ledgers if persisted;
- restore;
- same future transitions.

## R.11 No-runaway guard

30-day report:
- transition frequency;
- war escalation;
- market shocks;
- encounter modifiers.

Only report drift.

## R.12 B1 closeout matrix

| Actor | Transition | RNG | Owner | Projection | Consumer | Save | Test |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

---

# APPENDIX S — B2 PLACE-AUTHORITY DECISION PACKET

## S.1 Semantic fact audit

For every candidate duplicate:
- what exact fact is owned?;
- who writes it?;
- who persists it?;
- who consumes it?;
- can values diverge?

Only same writable fact is duplicate authority.

## S.2 Layered geography example test

A region containing locations is not duplicate authority by itself.

A waystation as a location subtype may be legitimate.

Do not flatten hierarchy blindly.

## S.3 Canonical ID map

If two systems use different IDs for same place:
- map evidence;
- check whether mapping is authoritative;
- determine if plan requires consolidation.

## S.4 Graph model proof

Prove current route representation mathematically/structurally:
- nodes identified;
- edges identified;
- adjacency;
- costs;
- gates.

If yes, graph-native clause may already be sealed even without a class named `Graph`.

## S.5 Knowledge visibility

Route visibility should derive from knowledge owner.

Tests:
- unknown place hidden;
- discovery reveals;
- save/restore;
- no panel-local cache divergence.

## S.6 Geographic knowledge effect

If plan requires knowledge beyond visibility:
- identify exact consumer;
- no speculative bonuses.

## S.7 Consolidation proposal

Required when true duplicate owners.

Proposal table:
`Current owner A | Current owner B | Target owner | Consumers | Save migration | ID migration | Phase | Gate`.

## S.8 Migration gates

If later signed:
- current save loads;
- IDs stable/mapped;
- routes same;
- UI same;
- caravan/expedition same;
- replay fingerprint where behavior should remain identical.

## S.9 B2 closeout matrix

| Sub-scope | Verdict | Evidence | Inline delta | Proposal | Final |
|---|---|---|---|---|---|
| place authority |  |  |  |  |  |
| graph travel |  |  |  |  |  |
| geographic knowledge |  |  |  |  |  |

---

# APPENDIX T — CLAIM / DEPENDENCY CONTROL

Before every task populate:

| Required path/system | Claim owner | Status | Safe? | Dependency | Dependency state |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

If any hard dependency not terminal:
- do not begin implementation.

If claim only blocks one surface:
- split only when plan can truthfully close mechanism separately;
- record exact remainder.

---

# APPENDIX U — DETERMINISM AUDIT

## U.1 A1
No RNG expected.
Stable same-day ordering.

## U.2 A2
Memory effects derived.
Encounter weighting uses existing seeded stream if applicable.

## U.3 A3
Refusal deterministic from state unless plan explicitly includes seeded behavior.

## U.4 A4
Tooling deterministic given same batch.

## U.5 A5
Overlay order deterministic.

## U.6 B1
Dedicated day-keyed seeded streams.

## U.7 B2
Route/knowledge deterministic.

Any hidden `DateTime.UtcNow`, unordered dictionary dependence, or filesystem enumeration ordering found in new code blocks closure.

---

# APPENDIX V — SAVE COMPATIBILITY AUDIT

| Task | New persisted state? | Owner | Old-save default | Round-trip | Mid-flow restore |
|---|---|---|---|---|---|
| A1 | maybe fired ledger | calendar | empty | required | required |
| A2 | no expected | existing memory | n/a | effect parity | required where stateful |
| A3 | maybe policy ledger | leadership/roster | neutral | if added | if added |
| A4 | no | n/a | n/a | n/a | n/a |
| A5 | per existing contract | mod/save owner | existing | as required | replay |
| B1 | maybe actor state | faction/world | plan-defined | required | required |
| B2 | no expected | place/knowledge | existing | parity | parity |

No new save section merely for convenience.

---

# APPENDIX W — TEST COMMAND LOG TEMPLATE

For every test/gate:

**Task:**<br>
**Clause:**<br>
**Command:**<br>
**HEAD:**<br>
**Expected:**<br>
**Actual:**<br>
**Cases/checks:**<br>
**Warnings:**<br>
**Duration:**<br>
**Artifact/log:**<br>
**Disposition:**<br>

Implementation logs should contain exact commands.

---

# APPENDIX X — PART 1 MERGE ORDER

Recommended merge:

1. A1
2. A2
3. A3
4. A4
5. A5
6. B1 if dependencies
7. B2

After every merge:
- refresh `HEAD`;
- refresh claims;
- update census;
- recalculate DAG.

B1/B2 may reorder if census dictates.

---

# APPENDIX Y — WAVE 11 PART 2 INPUT CONTRACT

After Part 1, reread current census.

Expected preview candidates from supplied source:
- C1[16] Depth Passes;
- C2[12] Difficulty Authority / immutable completion record;
- C2[13] Port Contracts;
- C2[14] duplicate Goods Must Arrive reconciliation.

But Part 2 only includes rows still current.

For each candidate:

`Corpus key | Historical title | Current status | Remainder | Dependencies | Claims | Decision | Duplicate? | Acceptance`.

Special rule:
**C2[14] duplicate found** must reconcile against C1[10]/Wave 10 B5 before any execution.

Do not execute duplicate plan twice.

---

# APPENDIX Z — FINAL PART 1 RELEASE NOTE TEMPLATE

**Wave:** 11 Part 1<br>
**Commit:**<br>
**Date:**<br>

### Chain results
- C1[11]:
- C1[12]:
- C1[13]:
- C1[14]:
- C1[15]:
- C2[10]:
- C2[11]:

### Dependency / claim blockers
-

### Save/determinism
-

### Quality gates
- Build:
- Verify-fast:
- Integrity:
- Utilization:
- Save:
- Replay:
- 30-day:

### Queue movement
**Dependencies released:**<br>
**New decision blockers:**<br>
**Part 2 top node:**<br>
**Reason:**<br>

The release note must be generated from latest census, not from initial Wave 11 roadmap.

---

# APPENDIX AA — FINAL NO-FALSE-CLOSURE RULES

Wave 11 Part 1 is not complete if:

- A1 has deadline rows but no exactly-once persistence;
- A1 consequences only log instead of moving their owner;
- A2 records memory without behavioral effect where the plan demands action;
- A2 duplicates a memory authority;
- A3 refusal is only visual;
- A3 auto-assignment can bypass plan-defined refusal;
- A4 ladder classes exist but the acceptance pipeline is not executable;
- A4 current-corpus reds are hidden rather than routed;
- A5 documents compatibility but does not enforce required loader semantics;
- A5 overlay order depends on filesystem enumeration;
- B1 autonomous transitions remain invisible to the player where consequence reach is required;
- B1 difficulty drift is silently tuned inline;
- B2 calls legitimate geographic layers “duplicates” without semantic proof;
- B2 performs broad consolidation without signed proposal;
- an implementation log is missing;
- census/ledger is stale after close;
- Part 2 is generated directly from the preview instead of current DAG.

**End-state invariant:** each Wave 11 Part 1 plan node becomes either genuinely sealed, explicitly blocked by a named current dependency/claim/decision, or reduced to a bounded remainder that the census can promote next. Nothing remains ambiguous.

# APPENDIX AB — A1 DEADLINE FAILURE-ROUTING TABLE

| Failure | Interpretation | Action | Prohibited response |
|---|---|---|---|
| deadline row unreachable | authored-data defect or stale plan sample | fix/remove row | weaken reachability test |
| consequence reference missing | content/schema defect | reject row | silently no-op |
| consequence owner rejects | domain-state conflict | surface typed failure / route repair per contract | mark fired success anyway |
| ledger duplicates | exactly-once bug | repair calendar/ledger | UI de-dup only |
| old save mass-fires past deadlines | migration bug | neutral old-save policy | accept because deterministic |
| same-day order differs | unstable iteration | stable sort | rely on runtime hash order |
| briefing surface claimed | coordination blocker | defer surface | alternate briefing system |

---

# APPENDIX AC — A2 MEMORY EFFECT REVIEW TABLE

For each memory effect, reviewers must answer:

1. Is the memory already persisted?
2. Is the effect required by C1[12], or merely attractive?
3. Which existing system owns the effect?
4. Can the effect be derived rather than saved?
5. What is neutral parity?
6. Can repeated evaluation double-apply?
7. Can loss/transfer remove it cleanly?
8. Does save/restore reconstruct it?
9. Is the effect visible/explainable where current UX exposes contributors?
10. Does the wiring alter balance enough to require a signed decision?

A2 pauses on question 10 if the plan does not already specify the value.

---

# APPENDIX AD — A3 REFUSAL PRECEDENCE TEST TABLE

| Case | Injury | Exhaustion | Grievance | Policy conflict | Expected reason | Assignment committed? | Consequence |
|---|---|---|---|---|---|---|---|
| baseline | no | no | no | no | accepted | yes |  |
| single refusal |  |  |  |  |  | no |  |
| multiple reasons |  |  |  |  | stable precedence | no |  |
| state changes before confirm |  |  |  |  | revalidated result |  |  |

Use actual plan inputs instead of these placeholders.

---

# APPENDIX AE — A4 PIPELINE BASELINE FINDING ROUTING

If the first full-corpus run is red:

## AE.1 Classify each failure
- invalid reference;
- dead/unconsumed content;
- canon conflict;
- narrative/quality gate;
- tooling false positive;
- stale rung.

## AE.2 Route
`Finding | Rung | Owner | Existing debt? | New package | Blocks acceptance?`.

## AE.3 Do not
- whitelist without owner;
- change rung to advisory simply to obtain green;
- delete content without data-owner review;
- alter quality thresholds outside C1[14].

The acceptance pipeline is successful even if it exposes real existing red content; the repository is not release-green until those findings are handled according to current policy.

---

# APPENDIX AF — A5 MODDED REPLAY TEST PACKET

## AF.1 Fixture identity
**Base dataset:**<br>
**Sample mod:**<br>
**Sample pack:**<br>
**Mod IDs/versions:**<br>
**Load order:**<br>
**Seed:**<br>

## AF.2 Run
- load base;
- validate mod/pack compatibility;
- apply overlays;
- run acceptance pipeline if required;
- compose campaign;
- execute deterministic scenario;
- capture merged-data fingerprint if supported;
- capture final state fingerprint.

## AF.3 Repeat
Run same configuration multiple times.

## AF.4 Restore
If modded save is in contract:
- save;
- restore with same mod set;
- compare state;
- test missing/incompatible mod-set behavior only if contract defines it.

## AF.5 Failure
Any overlay divergence blocks closure.

---

# APPENDIX AG — B1 AUTONOMY TRANSITION REVIEW

For each autonomous transition:

- Is it explicitly within C2[10]?
- Which current owner changes state?
- What RNG stream, if any?
- Can it occur off-screen?
- What player-facing projection reveals it?
- What downstream system consumes it?
- What is saved?
- What prevents duplicate projection after restore?
- What 30-day metric could drift?

No transition enters production until all answers exist.

---

# APPENDIX AH — B2 PLACE CONSOLIDATION DECISION CRITERIA

A signed consolidation proposal is required if any two live systems:

1. independently write the same place identity/fact;
2. persist separate values that can diverge;
3. expose incompatible IDs for the same semantic entity;
4. force consumers to reconcile conflicting truth.

A proposal is **not** required merely because:
- region and location are different layers;
- waystation is a subtype;
- map presentation caches derived layout;
- travel owns route cost while place owns identity.

The audit must distinguish authority duplication from normal composition.

---

# APPENDIX AI — PART 1 IMPLEMENTATION HANDOFF TEMPLATE

**Task / corpus key:**<br>
**Plan file:**<br>
**Starting census status:**<br>
**Final census status:**<br>
**HEAD/worktree:**<br>
**Dependencies:**<br>
**Claims:**<br>
**Historical premise:**<br>
**Current premise:**<br>
**Sealed-elsewhere clauses:**<br>
**Executed remainder:**<br>
**Files changed:**<br>
**Data/schema changes:**<br>
**Save impact:**<br>
**Determinism/RNG:**<br>
**Focused tests:**<br>
**Owning suites:**<br>
**UI gates:**<br>
**Integrity/utilization:**<br>
**Build:**<br>
**Verify-fast:**<br>
**Implementation log:**<br>
**Ledger update:**<br>
**Remaining blocker:**<br>
**Next DAG node:**<br>

A handoff that cannot answer these fields is incomplete.

---

# APPENDIX AJ — WAVE 11 PART 1 MERGE-READINESS REVIEW

## AJ.1 A1
- [ ] plan read fully;
- [ ] dependencies resolved;
- [ ] calendar owner reused;
- [ ] exactly-once ledger;
- [ ] consequence routing;
- [ ] save migration;
- [ ] deterministic ordering;
- [ ] samples reachable;
- [ ] implementation log;
- [ ] census/ledger updated.

## AJ.2 A2
- [ ] sealed memory pieces credited;
- [ ] acts matrix complete;
- [ ] only plan-required effects;
- [ ] modifier/encounter owners reused;
- [ ] no duplicate save;
- [ ] replay parity;
- [ ] log/census.

## AJ.3 A3
- [ ] leadership owner retained;
- [ ] policy remainder explicit;
- [ ] typed consent/refusal;
- [ ] no silent assignment;
- [ ] consequences routed;
- [ ] UI revalidation;
- [ ] restore parity;
- [ ] log/census.

## AJ.4 A4
- [ ] rungs current;
- [ ] no new criteria;
- [ ] pipeline order exact;
- [ ] fail contract;
- [ ] CI integration;
- [ ] current-corpus baseline;
- [ ] findings routed;
- [ ] log/census.

## AJ.5 A5
- [ ] current loader credited;
- [ ] compatibility semantics exact;
- [ ] typed rejections;
- [ ] pack class only if plan;
- [ ] deterministic overlay;
- [ ] modded replay;
- [ ] contract updated;
- [ ] log/census.

## AJ.6 B1
- [ ] DAG ready;
- [ ] existing world motion credited;
- [ ] named actor scope only;
- [ ] day-keyed deterministic autonomy;
- [ ] projection reach;
- [ ] restore parity;
- [ ] 30-day drift report;
- [ ] log/census.

## AJ.7 B2
- [ ] place layers mapped;
- [ ] true duplicates distinguished;
- [ ] graph-native status proven;
- [ ] knowledge owner retained;
- [ ] bounded wiring only;
- [ ] broad consolidation proposed, not executed;
- [ ] log/census.

---

# APPENDIX AK — WAVE 11 PART 2 PREVIEW RECONCILIATION RULES

The source preview is not a commitment.

## AK.1 C1[16] Depth Passes
Promote only if:
- Part 1 rails/acceptance pipeline are sealed;
- dead content can now land on real consumers;
- dependencies current.

## AK.2 C2[12] Difficulty / completion record
Reverify:
- Wave 6 difficulty assignment;
- 19A epilogue;
- immutable completion record remainder.

Do not redo epilogue.

## AK.3 C2[13] Port contracts
Reverify current standing unbound-effect/legacy-neutral patterns and close only missing formal contract/wiring validation.

## AK.4 C2[14] duplicate Goods Must Arrive
First action is duplicate reconciliation with C1[10] / Wave 10 B5:
- identical plan? mark duplicate/superseded;
- extra unique clauses? extract remainder;
- no second delivery implementation.

Part 2 must use latest census classification.

---

# APPENDIX AL — RELEASE-CADENCE OBSERVABILITY

No new analytics subsystem is required. Existing artifacts provide observability:

- A1: deadline ledger + event trace;
- A2: modifier/encounter outcome;
- A3: typed command/refusal result;
- A4: per-rung pipeline output;
- A5: loader rejection + replay fingerprint;
- B1: autonomy transition/projection trace;
- B2: route/knowledge verdict tests.

Logs must aid debugging without becoming authoritative state.

---

# APPENDIX AM — FINAL RELEASE NOTE TEMPLATE

# Wave 11 Part 1 Closeout

## Chain results
| Task | Start status | End status | Key delta | Evidence |
|---|---|---|---|---|
| C1[11] | OPEN |  |  |  |
| C1[12] | PARTIAL |  |  |  |
| C1[13] | PARTIAL |  |  |  |
| C1[14] | PARTIAL |  |  |  |
| C1[15] | PARTIAL |  |  |  |
| C2[10] | OPEN |  |  |  |
| C2[11] | PARTIAL |  |  |  |

## Quality
- Build:
- Verify-fast:
- Save:
- Replay:
- Integrity:
- Utilization:
- Pipeline:
- Mod replay:
- 30-day autonomy:

## Governance
- Active claims:
- Decision blockers:
- Consolidation proposals:

## Queue
**Newly released dependencies:**<br>
**Top Part 2 node:**<br>
**Duplicate reconciliations required:**<br>
**Why this node is next:**<br>

---

# APPENDIX AN — FINAL ACTUALITY CHECK

Immediately before each task begins, answer:

1. Does this plan file still exist unchanged or has it been revised?
2. Did a concurrent wave/package already land part of the remainder?
3. Did a claim appear on required paths?
4. Did a dependency close or reopen?
5. Did the census status change?
6. Does the current owner still match the plan’s expected seam?
7. Is the package still bounded?

If any answer changes scope, update the implementation log premise before coding.

This rule prevents the plan itself from becoming stale while Wave 11 executes.

**End of Wave 11 Part 1.**

# APPENDIX AO — FINAL FAILURE-INJECTION CHECKS

These are not new mechanics. They are explicit attempts to prove the new contracts fail safely.

## AO.1 Calendar/deadline
- remove consequence reference in test fixture → integrity rejects;
- duplicate deadline ID → validator rejects;
- restore terminal deadline → no refire;
- same-day repeated evaluator call → no duplicate.

## AO.2 Memory
- missing remembered place target → safe current-owner behavior;
- heirloom transferred → contribution follows owner correctly;
- repeated restore → no stacked contribution.

## AO.3 Leadership
- refusal condition appears between UI open and confirm → domain revalidation catches it;
- two refusal causes → stable reason precedence;
- auto-assignment encounters refusal → current roster policy handles without silent force.

## AO.4 Acceptance pipeline
- rung 1 red → later rungs follow exact plan fail policy;
- underlying gate command unavailable → pipeline reports infrastructure failure rather than “content accepted”;
- duplicate manifest registration → tooling test catches duplicate execution where applicable.

## AO.5 Mods/content packs
- incompatible version → typed rejection;
- same mod set loaded twice in fresh runs → identical overlay;
- deliberately reordered filesystem enumeration → contract-defined order still wins;
- content pack with bad reference → A4 pipeline blocks acceptance.

## AO.6 Autonomous outside world
- projection consumer absent in fixture → transition cannot be falsely marked player-visible;
- save immediately before off-screen transition → restored suffix identical;
- duplicate day tick → no unintended double autonomous advance.

## AO.7 Place/travel/knowledge
- UI cache disagrees with knowledge owner → owner state wins;
- unknown route ID → typed/current failure, no fabricated route;
- consolidation proposal test fixture demonstrates why two true owners can diverge before migration.

---

# APPENDIX AP — FINAL REVIEWER SIGN-OFF

The reviewer signs each statement:

- [ ] I can identify the authoritative owner for every new/changed state.
- [ ] I can identify the exact plan clause each production change satisfies.
- [ ] I can distinguish already-sealed work from Wave 11 deltas.
- [ ] I have seen save/restore evidence for every new persisted behavior.
- [ ] I have seen determinism evidence for every seeded/derived behavior.
- [ ] No UI surface became a domain authority.
- [ ] No decision-gated balance/consolidation choice was made implicitly.
- [ ] Every implementation log and census row agrees.
- [ ] Every remaining blocker is explicit.
- [ ] The Wave 11 Part 2 queue is generated from the latest DAG rather than the preview.

If any box cannot be signed with evidence, the owning task remains open.

**Final invariant:** Wave 11 Part 1 does not succeed because seven plan files were processed. It succeeds when the first seven verified queue nodes become truthful, tested execution states and the next queue segment can proceed without rediscovering ownership, dependencies, persistence, or hidden remainder.

# FINAL EXECUTION NOTE

Immediately before handing Part 1 to the next agent, rerun the census readiness calculation and claim check at the final merged `HEAD`. If A1–A5 or B1–B2 released a prerequisite, changed a claim boundary, or converted a partial plan into a sealed node, update the ranked queue before Wave 11 Part 2 is authored. The final queue—not the initial preview—is authoritative.
