# ASHFALL — GENERATION WAVE 9 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 1

**Purpose:** execution-grade continuation of the ASHFALL gap-sealing / blocker-unblocking series.

**Source of truth:** the Wave 9 surviving-blocker ledger supplied for this package. This document expands only blockers that survived its `HEAD` re-verification. It does not reactivate blockers the ledger says are stale, invent replacement authorities for missing owners, or pull later Wave 9 decision work into Part 1.

**Part 1 execution set:**
1. **A1 — C1.4 Crisis Prediction + real coordination gate**
2. **A2 — C1.5 Cloud Seeding runtime consumer**
3. **A3 — C1.6 Trophies + C1.7/C1.8 Kennel completion**
4. **A4 — C1.9 hardening + C1.10 content tranche / C1 chain close**
5. **B1 — Plan 31 semantic-kind authority**
6. **B2 — C2[2] deferred delta: ducking scope-map, acquisition sweep, 17B deep matrix**

**Document target:** implementation, verification, and handoff precision. No filler. Every section exists to constrain edits, preserve authority boundaries, define evidence, or prevent false closure.

---

# 0. OPERATING CONTRACT

## 0.1 Terminal states

A Part 1 task may close only as one of:

- **IMPLEMENTED** — missing mechanism/consumer/surface now exists through the correct owner and all required gates pass.
- **PARTIAL-IMPLEMENTED / CLAIM-DEFERRED** — only when a live ownership claim prevents the final consumer/surface. The independently safe portion must be fully landed, and the remaining exact path/owner is recorded.
- **VERIFIED-RESOLVED** — `HEAD` re-verification proves the blocker was already sealed by concurrent work; no duplicate implementation is created.
- **DECIDED-DEFERRED** — only for an explicitly decision-gated sub-item whose signed outcome is to remain deferred.
- **ROUTED-REPAIR** — a discovered regression or incompatible contract is moved to a bounded repair package rather than hidden in this work.

“Docs updated”, “tests adjusted”, “panel added”, or “data authored” are not terminal states by themselves.

## 0.2 Series-wide mandatory rules

1. Re-verify every premise at current `HEAD` before editing.
2. Read and honor active claims before touching paths.
3. One task at a time unless paths, generators, and verification state are demonstrably disjoint.
4. Existing authority wins. Extend it; do not create a parallel owner.
5. A missing authority is a design/ownership blocker, not permission to improvise.
6. Presentation reads domain truth and routes commands; it does not own domain state or policy.
7. Prefer derived state over new persistence.
8. Any new persisted field is additive/versioned and old-save neutral.
9. Deterministic simulation remains deterministic. UI opening, read-model evaluation, or refresh may not consume gameplay RNG.
10. Exactly-once events remain exactly-once across restore.
11. Generated artifacts are changed through their inputs and regenerated; outputs are never hand-edited.
12. Focused tests run before adjacent suites; broader aggregates do not replace focused regression coverage.
13. A stale contract is rematched only after current production truth is independently proven.
14. Real regressions are fixed by their owner or routed as repair packages; do not weaken tests to absorb them.
15. Active Plan 24 claims remain untouched unless formally handed off.

## 0.3 Evidence bundle required for every package

Each task produces:

- premise evidence: current commit, blocker proof, current claim state;
- path ownership/claim record;
- authority map for touched state;
- implementation/change matrix;
- focused tests with exact command and result;
- save/load evidence when persisted state is touched;
- replay/fingerprint evidence when simulation state or deterministic projections are touched;
- UI route/lifecycle/a11y/snapshot evidence when UI is touched;
- data integrity/content-utilization evidence when catalogs are touched;
- generator/index `--check` evidence when generated artifacts are touched;
- build result;
- closeout/debt/plan truth update;
- explicit remaining blocker, if any.

## 0.4 Abort conditions

Stop and route instead of improvising when:

- the expected owner is absent;
- the target path is actively claimed;
- the live code contradicts the written plan in a way that changes architecture;
- implementation requires a second writer for the same state;
- a “pure read model” would need mutation or new persistence;
- cloud seeding would require a second weather path;
- trophy conditions require unavailable source state;
- semantic kinds would duplicate an existing classification authority;
- audio ducking’s current implementation already fully satisfies the contract;
- a content row cannot be consumed without a mechanism A1–A3 have not landed;
- the only way to pass a test is to weaken an active invariant.

---

# 1. PART 1 DEPENDENCY GRAPH

## 1.1 Hard order

**A1 → A2 → A3 → A4** is the C1 chain order.

- A4 depends on A2 because its cloud-seeding content must have a consumer.
- A4 depends on A3 because its trophy and kennel tranche rows need the mechanism/surfaces already landed.
- A1 may split Core prediction from DailyBriefing integration while the active Plan 24 claim still covers the briefing path.
- B1 and B2 are logically independent from the C1 chain, but both must re-check Plan 24 / shared vocabulary or Campaign ownership before touching overlapping paths.

## 1.2 Recommended execution sequence

1. **A1 premise/claim resolution**
2. **A1 Core predictor**
3. **A1 visible consumer only if ownership is clear**
4. **A2 cloud-seeding consumer**
5. **A3 trophy mechanism**
6. **A3 kennel remainder**
7. **A4 hardening**
8. **A4 content tranche**
9. **B1 semantic-kind authority**
10. **B2 ducking delta**
11. **B2 acquisition sweep**
12. **B2 17B deep matrix**
13. **Part 1 aggregate closeout**

B1/B2 may be scheduled before A4 if claims and shared files are disjoint, but shared generators/docs must never be regenerated concurrently.

---

# 2. TASK A1 — C1.4 CRISIS PREDICTION + REAL COORDINATION GATE

## 2.1 Objective

Implement the C1.4 crisis prediction authority as a deterministic, derived Campaign read model, while replacing the stale “second Campaign/DailyBriefing agent” premise with the actual coordination fact: the active Plan 24 claim currently covers briefing-related files. The package must be capable of landing Core prediction without violating the claim and must defer only the contested consumer surface if necessary.

## 2.2 Starting facts that must be re-proven

- No crisis prediction mechanism currently exists in Campaign Core.
- The old concurrent-agent blocker is stale.
- The active Plan 24 claim still covers `DailyBriefingReportBuilder.cs` and relevant Campaign-owner wiring.
- C1.4 remains the next unclosed C1 chain wave.
- Prediction inputs already exist in owned authorities and can be read without mutation.

Record current evidence before implementation. If the Plan 24 claim closed since the ledger was written, record that and perform the full C1.4 package through the now-unclaimed briefing seam.

## 2.3 Authority boundary

### Predictor owns
- classification of predicted crisis records;
- projected day or horizon;
- confidence band according to the C1 contract;
- stable reason IDs/explanations;
- deterministic aggregation of read-only inputs.

### Predictor does not own
- food inventory;
- water state;
- power generation/storage;
- radiation state;
- disease/sanitation;
- distress follow-up queues;
- cohort state;
- DailyBriefing event ownership;
- save state for derivable predictions.

### Consumer owns
- briefing/journal/panel formatting;
- ordering among other report items;
- accessibility/presentation behavior.

## 2.4 Phase A1.0 — claim and contract freeze

1. Read the exact C1.4 contract and C1 completion chain.
2. Read the active claim table.
3. Record current owner and status of the Plan 24 claim.
4. Enumerate every C1.4 target path named by the plan.
5. Split target paths into **unclaimed-safe** and **claim-controlled**.
6. Claim only unclaimed-safe paths first.
7. Capture baseline Campaign test results for the smallest suite containing the intended predictor host seam.
8. Capture baseline replay/fingerprint for a short seeded campaign where prediction inputs vary.
9. Write `A1_COORDINATION_RECORD.md` with:
   - stale old premise;
   - current claim;
   - files inside the claim;
   - files safe now;
   - exact condition for deferred consumer integration.

No code edit precedes this record.

## 2.5 Phase A1.1 — input forensics

Build an input matrix with columns:

`Crisis class | Input | Canonical owner | Query/read API | Units | Missing/neutral state | Time basis | Mutability | Predictor use`.

At minimum inspect the C1 contract’s intended classes and the ledger-named inputs:
- food trajectory;
- water trajectory;
- power stock/production;
- radiation conditions;
- sanitation/disease exposure modifiers;
- distress follow-up pending state;
- cohort ratios or relevant population ratios.

The matrix must identify actual current APIs. Do not add broad service-locator reads solely for convenience.

### Input rules
- Missing data yields explicit neutral/unknown behavior, never an exception-driven guess.
- Dead/empty roster is a valid input state.
- Empty stock is valid and must not divide by zero.
- Rates are derived from existing campaign history/state only if such history is already canonical.
- Predictor evaluation cannot advance subsystem ticks.
- Predictor evaluation cannot consume items, power, water, or event queues.
- Predictor evaluation cannot alter briefing state.

## 2.6 Phase A1.2 — crisis record contract

Define one typed record or repository-equivalent structure with the minimum fields demanded by C1.4:

- crisis kind/class;
- projected day/horizon;
- confidence band/value;
- stable reason ID(s);
- optional supporting metrics if the plan already requires them.

Do not add speculative “AI advice”, free-form recommendations, or a second severity model unless C1.4 explicitly requires them.

### Record invariants
- same authoritative input state produces identical output;
- stable ordering when multiple crises tie;
- neutral/no-crisis state is representable without sentinel exceptions;
- projected day never precedes the evaluation day unless representing an already-active crisis is explicitly part of the contract;
- confidence calculation is bounded;
- reason IDs are stable and presentation-neutral;
- output is derivable and therefore not persisted.

## 2.7 Phase A1.3 — prediction algorithm implementation

Implement the smallest algorithm satisfying the existing C1.4 contract.

### Required properties
1. Pure read-model evaluation.
2. Zero new RNG.
3. No mutation.
4. Stable iteration/order.
5. Bounded confidence.
6. Defined behavior with missing/empty inputs.
7. No hidden reliance on presentation text.
8. No second copy of subsystem thresholds when a canonical threshold/query exists.

If the plan uses threshold windows, read the current C1 source before choosing exact math. This implementation plan does not authorize new balancing numbers.

### Anti-fork rule
If food/water/power/radiation already expose warning/forecast APIs, compose those rather than recoding their formulas. The predictor’s responsibility is cross-domain projection/classification, not reimplementation of each domain.

## 2.8 Phase A1.4 — focused tests

Create tests before or alongside implementation for:

- healthy neutral campaign → no crisis;
- food depletion trajectory → food crisis predicted;
- water depletion trajectory if in contract;
- power depletion trajectory if in contract;
- radiation risk input if in contract;
- empty stock;
- zero survivors/dead roster;
- missing optional source;
- exact threshold boundary;
- stable ordering of multiple predicted crises;
- same state evaluated repeatedly → identical output;
- evaluation does not alter state fingerprint;
- prediction read does not consume RNG;
- no save section added.

### Monotonic approach test
Construct a seeded campaign state where a resource crisis is approaching. Across legitimate day transitions, the predicted horizon should move toward the crisis consistently with real state. This test must drive the real underlying owner rather than directly editing the predictor’s outputs.

If consumption changes such that the crisis legitimately recedes, the test must account for the changed input; do not hard-code monotonicity beyond the controlled fixture.

## 2.9 Phase A1.5 — host seam

Locate the plan-named evaluation seam.

Preferred options:
- day-tick evaluation;
- briefing-request evaluation;
- explicit read-query requested by consumer.

Choose the contract-defined one. Do not run prediction per frame.

The host seam:
- calls predictor once per intended evaluation point;
- returns typed records;
- does not cache stale mutable references;
- does not persist prediction output unless the source plan explicitly requires historical snapshots.

## 2.10 Phase A1.6 — claim-safe consumer strategy

### If DailyBriefing path remains claimed
- do **not** edit claimed files;
- land predictor + tests + unclaimed host seam;
- if an existing unclaimed journal/read surface is explicitly permitted by C1.4, use it only if this does not create a parallel permanent surface;
- otherwise defer presentation integration cleanly;
- write `A1_BRIEFING_DEFERRED.md` naming exact path, owner claim, and required follow-up.

### If claim is handed off
- integrate through the existing DailyBriefing builder;
- use existing event/vocabulary contracts;
- add new vocabulary entry only through the owning vocabulary process;
- preserve existing ordering/format;
- run parity and snapshot gates.

The package may close **PARTIAL-IMPLEMENTED / CLAIM-DEFERRED** only if Core predictor is complete and all remaining work is exactly the claim-controlled surface.

## 2.11 Phase A1.7 — save/replay/determinism proof

Because prediction is derived:
- no predictor DTO/save section should be added;
- a save/restore at the same authoritative state must produce the same prediction;
- campaign state fingerprint before and after evaluation must be identical;
- repeated evaluation must not advance RNG streams;
- replay result should be unchanged apart from any presentation-only output not included in simulation state.

Create a regression test or harness assertion that would detect accidental mutation.

## 2.12 Phase A1.8 — visible surface acceptance

If briefing integration lands:
- warning text derives from stable reason IDs;
- no fabricated precision beyond the confidence model;
- no claim that a crisis “will” happen if the model only provides a risk/projection;
- keyboard/a11y rules of the existing briefing surface remain intact;
- snapshot diffs are reviewed and rebaselined only for intended new lines.

## 2.13 A1 verification commands/evidence

Run in this order:
1. new predictor test file;
2. owning Campaign test neighborhood;
3. Campaign baseline suite referenced by current claims, rechecking current case count rather than trusting historical numbers;
4. replay/fingerprint test;
5. build;
6. data integrity;
7. briefing/vocabulary tests if touched;
8. a11y/snapshot gates if visible surface touched;
9. verify-fast if applicable.

## 2.14 A1 closeout

Update:
- C1 completion chain: C1.4 complete, or Core-complete/surface-deferred;
- stale coordination note with current claim truth;
- integration ledger/checkpoint;
- any vocabulary/parity matrix if consumer landed.

Handoff table:
`Artifact | Owner | Before | After | Claim status | Test | Remaining work`.

## 2.15 A1 non-goals

- no new crisis state authority;
- no persisted predictor state by default;
- no RNG;
- no simulation mutation;
- no parallel briefing system;
- no edits to active Plan 24 paths;
- no rebalance of resource/radiation/disease thresholds;
- no speculative advice system.

---

# 3. TASK A2 — C1.5 CLOUD SEEDING: DATA-TO-RUNTIME CONSUMER

## 3.1 Objective

Turn existing cloud-seeding authored data from dead references into a canonical, player-triggerable runtime path through the existing weather authority, research authority, inventory authority, and approved action surface. The package closes only when the complete chain is live and content-utilization proves the family is consumed.

## 3.2 Premise proof

Before implementation:
- enumerate every `cloud_seed`/cloud-seeding reference in catalogs;
- classify each reference as definition, prerequisite, recipe, documentation, test-only, or runtime consumer;
- prove no runtime consumer currently exists;
- locate current weather mutation/severity authority;
- locate current research/knowledge gate;
- locate canonical inventory consume path;
- locate C1.5’s intended surface and cooldown semantics.

If a consumer has landed since the ledger, stop greenfield implementation and perform gap/parity verification instead.

## 3.3 Ownership contract

### Weather owner
Owns:
- legal weather transitions;
- severity/effects;
- seeded weather state;
- any canonical cooldown state if weather-scoped;
- event emission for weather changes.

### Inventory owner
Owns:
- material availability;
- atomic consume/rollback semantics.

### Research owner
Owns:
- unlock/knowledge prerequisite.

### Host/UI
Owns:
- command routing;
- result display;
- disabled reason display.

### Cloud-seeding feature
Does not create:
- a second weather state;
- panel-side weather mutation;
- direct catalog-to-UI behavior that bypasses authority;
- an independent RNG stream unless C1.5 explicitly requires one and repository RNG policy approves it.

## 3.4 Phase A2.0 — data family census

Create a table:
`Catalog | ID | Field/reference | Current consumer | Expected consumer | Status`.

Include:
- recipe references;
- research knowledge references;
- manual/library references;
- weather/effect IDs referenced by the C1 contract.

Run current integrity/utilization before changes and record the dead-reference baseline for this family.

## 3.5 Phase A2.1 — C1.5 contract extraction

Extract exact requirements from the existing C1.5 plan:
- action name/intent;
- required materials;
- unlock gate;
- legal/illegal source weather states;
- weather effect/transition;
- duration/cooldown;
- persistence expectation;
- presentation surface;
- player-visible result;
- deterministic/RNG behavior.

Any item not specified is **not** authorized to be invented here. Missing product semantics become a decision blocker.

## 3.6 Phase A2.2 — command preflight

Build a typed preflight result through the owning weather/action seam.

Preflight checks, only when supported by current contract:
1. research unlocked;
2. required materials exist;
3. weather state permits action;
4. cooldown permits action;
5. any existing location/facility requirement is met.

Preflight must not consume materials.

Stable failure reasons are returned for UI and tests.

## 3.7 Phase A2.3 — transaction order

Define command transaction explicitly:

1. validate immutable prerequisites;
2. reserve/consume materials through canonical inventory;
3. request canonical weather transition/effect;
4. if transition cannot commit after material reservation, use existing rollback/transaction semantics;
5. commit cooldown/state only after successful action;
6. emit authoritative result/event;
7. refresh consumer surface from state.

If existing APIs cannot guarantee a non-half-applied action, route a narrow atomicity repair before shipping.

## 3.8 Phase A2.4 — weather effect integration

Cloud seeding must apply the existing weather effect authority’s semantics.

Required checks:
- the seeding result maps to an existing weather/effects row or C1-authorized transition;
- naturally occurring and seeded equivalent weather use the same effect table;
- downstream systems read weather state normally and do not need seeding-specific branches;
- effect ordering with radiation, travel, trapping, power, or shelter weather consumers remains unchanged unless C1 explicitly specifies interaction.

Do not add a “cloud seeding modifier” to every downstream system.

## 3.9 Phase A2.5 — cooldown/persistence

Prefer a derivable cooldown using existing day/state data.

Only add persisted state when:
- C1.5 requires action history that cannot be reconstructed;
- an existing weather-state DTO is the proper owner;
- old saves can default to “no active cooldown” without changing behavior;
- capture/restore tests are added.

If adding a field:
- additive;
- version-aware if repository save format requires;
- neutral default;
- no new top-level section unless existing owner cannot represent it.

## 3.10 Phase A2.6 — deterministic transition

Use the existing weather authority’s deterministic stream or transition semantics.

Tests must prove:
- same seed/state/action inputs → same result;
- opening action UI does not consume RNG;
- failed preflight does not consume RNG;
- save/restore before action preserves deterministic result where repository RNG policy promises this;
- no new time-based `DateTime`/system randomness enters simulation.

## 3.11 Phase A2.7 — research gate

If research knowledge is a prerequisite:
- action is unavailable/locked until canonical research owner reports unlock;
- UI reason is explicit;
- research data is not duplicated into weather state;
- unlocking causes action availability through normal state/event refresh;
- old saves with existing research state behave correctly.

## 3.12 Phase A2.8 — action surface

Use the plan-named weather or command surface.

Required behavior:
- current availability;
- material requirement summary from canonical data/read model;
- cooldown state if applicable;
- research lock reason;
- weather legality reason;
- success/failure result;
- keyboard-operable action;
- no per-frame polling if authority exposes events;
- cleanup/unsubscribe on close.

## 3.13 Phase A2.9 — focused tests

Cover:
- unlocked + materials + legal weather → success;
- missing research → fail/no mutation;
- missing materials → fail/no mutation;
- illegal weather → fail/no mutation;
- cooldown active → fail/no mutation if cooldown exists;
- canonical inventory delta exactly once;
- weather transition/effect exactly once;
- equivalent natural vs seeded weather uses same effects row;
- save/restore of cooldown if persisted;
- deterministic result;
- failed attempt consumes no RNG/materials;
- repeated panel opens do not trigger action;
- content references become consumed.

## 3.14 Phase A2.10 — utilization/integrity proof

Run:
- data-integrity selftest;
- content-utilization selftest.

Record before/after for the cloud-seeding family. The blocker is not closed merely because code exists; the authored rows must be discoverably consumed by runtime.

Any remaining dead row is individually explained.

## 3.15 A2 closeout

Update C1 completion C1.5 row only after:
- runtime consumer chain proven;
- tests green;
- dead references resolved for the family;
- panel/action accessible;
- save/determinism evidence complete.

Handoff chain:
`catalog → loader → research gate → inventory preflight → weather authority → command result → surface → utilization gate`.

## 3.16 A2 non-goals

- no second weather authority;
- no weather rebalance;
- no new research system;
- no panel-side material consumption;
- no new RNG stream without explicit plan requirement;
- no invented environmental side effects;
- no content expansion beyond C1.5 mechanism needs; A4 owns C1.10 tranche.

---

# 4. TASK A3 — C1.6 TROPHIES + C1.7/C1.8 KENNEL COMPLETION

## 4.1 Objective

Land the missing trophy mechanism end-to-end and close only the remaining kennel scope not already supplied by the existing `KennelPanel` / `CompanionAnimalSystem` integration. Mechanism is completed here; C1.10 expansion counts remain A4’s responsibility.

## 4.2 First principle: do not rebuild what exists

For kennel work:
- read `KennelPanel.cs`;
- enumerate commands/read models/events already bound;
- compare C1.7/C1.8 requirement-by-requirement;
- classify each row **ALREADY COVERED**, **MISSING**, or **NO LONGER APPLICABLE**;
- implement only MISSING rows.

For trophies:
- confirm no catalog/mechanism has appeared under a different name;
- verify every intended condition can be observed from existing authoritative state/events before authoring the trophy row.

## 4.3 Trophy architecture

Use a standard chain:

`catalog → validated condition definition → existing domain events/read state → trophy evaluator → exactly-once award ledger → host/read model → journal/panel`.

The trophy system observes; it does not own the source facts.

## 4.4 Trophy catalog contract

Before creating `trophies.json` or equivalent:
- verify repository naming/ID-prefix registry;
- verify catalog registration process;
- identify schema conventions from comparable catalogs;
- identify localization/text fields;
- identify condition representation expected by C1.6.

Validation must cover:
- unique IDs;
- valid referenced species/items/collectibles/milestones;
- supported condition kinds;
- awardability/reachability where practical;
- no unresolved references.

Do not author A4’s +3 tranche here. A3 lands the baseline C1.6 mechanism/content count required by the original plan.

## 4.5 Trophy source-of-truth matrix

For every baseline trophy row create:
`Trophy ID | Condition | Source owner | Event/query | Existing persisted source? | Exactly-once key | Test fixture`.

No trophy-specific duplicate counters if source ownership already contains the fact.

Examples of source categories named by the source plan may include species encounters, collectibles, or milestones; use only verified current sources.

## 4.6 Award evaluator

Required properties:
- event-driven when canonical events exist;
- derived read on day tick only when no event exists and C1 permits;
- stable deterministic evaluation order;
- exactly-once award;
- safe repeated evaluation;
- no reward duplication on restore;
- old saves begin with neutral award ledger unless migration contract specifies otherwise.

## 4.7 Award ledger persistence

The ledger exists only to remember awarded trophy IDs if exactly-once cannot be derived from another canonical immutable fact.

Rules:
- additive save state under the trophy/achievement owner;
- stable ID storage;
- unknown IDs handled according to repository save compatibility rules;
- old saves default to empty;
- restore does not fire award side effects again;
- award history remains readable after catalog expansion.

Tests:
- award → save → restore → reevaluate → no duplicate;
- old save → eligible condition → exactly one award;
- repeated day tick → no duplicate;
- catalog +3 later in A4 does not disturb prior IDs.

## 4.8 Journal/event presentation

Award event:
- emits one canonical player-visible record;
- journal line is generated from the award result/catalog;
- no panel polling is needed to discover a just-awarded trophy if event binding exists.

Tone and localization follow existing game text conventions.

## 4.9 Trophy panel

Only implement if C1.6 requires it.

Route requirements:
- registry descriptor;
- player-surface ID;
- open/switch route;
- host binding;
- deterministic list order;
- awarded/unawarded representation as allowed by product contract;
- keyboard navigation;
- text labels beyond color;
- lifecycle disposal;
- snapshot targets for seeded states.

No trophy condition logic lives in the panel.

## 4.10 Kennel gap matrix

Build:
`C1.7/C1.8 clause | Existing CompanionAnimalSystem support | Existing KennelPanel support | Missing Core? | Missing host? | Missing UI? | Action`.

Expected outcome should be mostly “already covered” if Plan 174 presentation already landed. Do not modify Core solely to make the task look substantial.

Any missing behaviors route through `CompanionAnimalSystem`.

## 4.11 Kennel restrictions

- no second pet/companion state;
- no local hunger/health counters in panel;
- no breed/event content expansion beyond baseline mechanism needs;
- no A4 +2 breeds/+2 events here;
- no new veterinary system if existing companion authority already owns it;
- no silent command duplication.

## 4.12 A3 focused tests

### Trophy
- each supported condition kind;
- boundary condition;
- invalid/unreachable reference rejected by integrity;
- exactly-once;
- save/restore;
- old-save neutral;
- repeated evaluation safe;
- deterministic order;
- panel route/lifecycle/a11y if panel exists.

### Kennel
For every MISSING row:
- host command/read binding;
- Core authority remains sole writer;
- panel refresh;
- error/preflight reason;
- lifecycle;
- accessibility.

Also rerun the existing CompanionAnimal/Kennel tests to prove no regressions.

## 4.13 A3 utilization

Run integrity and utilization. Every baseline trophy row must have a live award evaluator. Every newly wired kennel content/reference must have a runtime consumer.

Do not “fix” utilization by artificially touching rows in a generic loop that never influences game behavior.

## 4.14 A3 determinism

Awarding is deterministic from canonical state. Repeated replay:
- same state/events → same trophies;
- no trophy evaluation mutates source systems;
- award ledger changes only when a new condition first becomes true;
- UI open/close does not affect awards.

## 4.15 A3 closeout

Update:
- C1.6 complete;
- C1.7/C1.8 complete with row-by-row already-covered/newly-wired evidence;
- Plan 174-related docs only where they describe the now-closed shared surface;
- integration ledger/checkpoint;
- snapshot coverage for new trophy panel.

Hand off the exact baseline counts that A4 will expand from. A4 must remeasure rather than trust them blindly.

## 4.16 A3 non-goals

- no A4 content tranche;
- no new companion authority;
- no unrelated pet AI;
- no trophy rewards/economy unless C1.6 explicitly defines them;
- no duplicate source counters;
- no broad narrative expansion.

---

# 5. TASK A4 — C1.9 HARDENING + C1.10 CONTENT TRANCHE / C1 CHAIN CLOSE

## 5.1 Objective

Close the entire C1 chain only after A1–A3 mechanisms are complete or truthfully claim-deferred, then execute the C1.9 hardening requirements and the exact C1.10/Phase 22 authored content expansion against live counts and live consumers.

## 5.2 Entry gate

A4 does not begin content authoring until:
- A1 predictor mechanism is complete;
- A2 cloud-seeding consumer is live;
- A3 trophy mechanism is live;
- A3 kennel remainder is closed;
- active claims for any target content/surface are clear.

If A1’s briefing surface remains Plan-24-claim-deferred, classify whether C1.10 crisis-advice rows would be dead/unreachable. If yes, defer those rows rather than authoring dead content.

## 5.3 C1.9 hardening first

Hardening precedes content.

Extract the exact C1.9 checklist and create:
`Hardening clause | Current evidence | Existing test | Gap | New/changed test | Result`.

Do not replace the plan’s actual DoD with a generic checklist.

Likely categories mentioned in the source ledger include:
- empty catalogs;
- missing region behavior;
- embargo/weather/shock interaction ordering;
- old save restoration neutrality;
- save corruption/migration;
- section/codec count pins.

Each is verified against current source before work.

## 5.4 Save hardening

Run the existing save corruption/migration focused tests before content changes.

For every C1-owned save addition from prior waves:
- missing old field defaults neutral;
- unknown/new IDs handled safely;
- corrupted section fails according to repository policy;
- capture/restore round-trip stable;
- section/codec counts reflect actual current schema.

Never update a count pin just because it fails. First prove the schema actually changed as intended.

## 5.5 Interaction hardening

Where C1.9 names economy/weather combinations:
- use production authorities;
- test interaction order explicitly;
- avoid balance changes;
- pin only invariants, not incidental implementation order unless policy requires it.

If an interaction currently violates plan policy, treat as a real C1.9 fix. If the plan wording is stale relative to a later signed authority, document the superseding contract before updating tests.

## 5.6 Live-count census before C1.10

Immediately before authoring, record current counts for every target:
- embargo rules;
- atlas entries;
- crisis advice;
- cloud-seeding recipes;
- trophies;
- kennel breeds/events;
- radio flavor.

The historical targets from the source plan are guides; the implementation must compute the actual delta from live state so concurrent additions do not create duplicates.

## 5.7 Content tranche rule

For each catalog:
1. identify live current count;
2. identify intended target or incremental delta;
3. inspect existing coverage;
4. author only missing distinct rows;
5. validate references;
6. prove runtime utilization;
7. run focused data tests;
8. update count-pin tests only after proving new truth.

No schema changes are authorized by A4.

## 5.8 Embargo rules tranche

For each new embargo rule:
- use existing weather/region/goods vocabulary;
- close a demonstrable case/coverage gap;
- avoid duplicating an existing semantic case with only wording changes;
- reference only valid region/good IDs;
- exercise existing embargo engine;
- add focused evaluation coverage when a new combination exposes an untested branch.

Do not rebalance existing rules.

## 5.9 Atlas tranche

New atlas entries:
- use canonical region vocabulary;
- fill identified region/category coverage gaps;
- do not create duplicate aliases for the same location;
- pass all reference and localization constraints;
- surface through existing atlas consumer;
- preserve stable ordering/IDs.

If “coastal” is a first-class region in the current authority, use it exactly; if live source says otherwise, follow live authority and document the premise change.

## 5.10 Crisis-advice / briefing rows

Only author if the A1 consumer path is live or the existing briefing system can consume them without violating the Plan 24 claim.

Requirements:
- existing text shape/schema;
- stable reason/event linkage;
- no unsupported certainty;
- restrained fictional tone;
- no duplicate advice for same trigger unless variants are explicitly supported;
- content-utilization proves reachable consumer.

If claim still blocks the consumer, split this tranche and record exact deferred rows.

## 5.11 Cloud-seeding content

A4 may add the planned extra bulk recipe only after A2’s runtime consumer exists.

Check:
- recipe inputs are valid canonical items;
- research prerequisite valid;
- output/action mapping valid;
- utilization sees the recipe;
- action preflight consumes the correct row;
- no second cloud-seeding mechanism.

## 5.12 Trophy expansion

Expand from the live baseline to the C1.10 target only after A3.

Every new trophy:
- condition is supported by existing evaluator;
- source fact exists;
- IDs unique;
- references valid;
- reachable in intended gameplay;
- exactly-once ledger works with added ID;
- species-referencing rows pass the authoring verification gate.

Do not add new trophy condition-language/schema in A4.

## 5.13 Kennel breed/event expansion

Use canonical species IDs and `CompanionAnimalSystem`.

For new breed/event rows:
- IDs unique;
- species valid;
- condition/trigger supported by existing mechanism;
- content consumed;
- no duplicate semantic event;
- no new Core mechanic.

## 5.14 Radio flavor expansion

Rows must:
- use existing radio catalog schema;
- satisfy current tone/style constraints;
- reference valid event/channel IDs;
- be consumed by current selection path;
- preserve deterministic/random-stream policy;
- not interfere with alert-priority policy.

## 5.15 `water_sample_contaminated` equipability decision

This is decision-gated.

Prepare a short memo:
- current item row;
- current category/equipability;
- all current consumers;
- why it is flagged;
- option A: correct classification;
- option B: intentionally retain;
- save/content impact;
- tests impacted.

Stop for foreman signature before changing the item. If retained, record it as intentional so future waves stop reopening it.

## 5.16 Per-batch gates

After each content batch:
- data integrity;
- content utilization;
- owning focused tests;
- count-pin adjustment only if justified;
- no new dead rows;
- no unexpected primary-wins/dead-data warnings.

Keep batches small enough that a failed utilization row can be traced.

## 5.17 C1 chain close

C1 is closed only when:
- A1–A4 statuses are terminal and truthful;
- C1.4–C1.10 completion rows match implementation;
- save hardening passes;
- all authored rows validate and are consumed;
- no known C1 surface remains inaccessible;
- any claim-deferred consumer is explicitly separated so “C1 closed” is not falsely asserted.

If A1 consumer or A4 crisis advice remains claim-deferred, use `C1 CORE/MECHANISMS CLOSED — SURFACE DEFERRED` rather than full closure until the claim handoff finishes.

## 5.18 A4 verification

Run:
1. C1.9 new/changed hardening tests;
2. save corruption/migration suite;
3. per-catalog integrity/utilization;
4. economy focused suite;
5. radio focused suite;
6. world/weather focused suite;
7. companion/trophy suites;
8. relevant panel snapshots;
9. docs/catalog registry generators;
10. build;
11. verify-fast.

Current test counts are remeasured; historical counts are not treated as hard-coded truth.

## 5.19 A4 non-goals

- no schema changes;
- no new mechanisms;
- no balance overhaul;
- no dead content ahead of missing consumers;
- no unsanctioned item-classification fix;
- no broad prose expansion beyond tranche targets.

---

# 6. TASK B1 — PLAN 31 SEMANTIC-KIND AUTHORITY

## 6.1 Objective

Implement the deferred Plan 31 semantic-kind layer as a total, test-enforced classification over the existing `DayEventVocabulary` authority family, then wire only the consumers explicitly required by the closure report and safe under current claims.

## 6.2 Premise proof

Before work:
- confirm no `SemanticKind`/`semantic_kind` implementation exists;
- read C2[2] closure report Plan 31 entry;
- read Plan 31 source plan;
- read current `DayEventVocabulary`;
- read parity matrix and source gate tests;
- identify named consumers and their claim state.

If a later semantic classification already exists under another name, stop and compare contracts rather than introducing `SemanticKind`.

## 6.3 Ownership rule

Semantic kind is an extension of the current day-event vocabulary authority family.

Do not create:
- a second registry in unrelated Core;
- data-authored classification if vocabulary is code-owned and the plan does not demand data;
- consumer-specific private classifications.

There must be one total mapping.

## 6.4 Taxonomy extraction

Derive taxonomy from:
- current vocabulary population;
- current parity matrix;
- Plan 31 intent;
- actual consumer need.

Do not start by choosing attractive abstract categories.

For every event ID:
`Event ID | Current vocabulary meaning | Proposed kind | Consumer(s) | Rationale | Stability note`.

Taxonomy names should be few, stable, and semantically distinct.

## 6.5 Totality contract

Every vocabulary ID must map to exactly one kind.

Enforcement:
- test enumerates all vocabulary IDs;
- mapping returns no implicit “unknown” for registered IDs unless Plan 31 explicitly defines an Unknown kind;
- adding a vocabulary ID without classification fails the gate;
- duplicate mappings fail;
- a kind change is an explicit reviewed contract change.

## 6.6 No-silent-drop rule

The existing vocabulary/parity contract’s no-silent-drop principle extends to semantic kinds.

Consumers must not:
- ignore unrecognized registered IDs;
- default everything new to a broad category without test visibility;
- silently skip events because kind mapping is absent.

## 6.7 Consumer map

Read closure report to identify actual intended consumers.

For each:
`Consumer | Why it needs kind | Existing query seam | Claimed? | Wiring action | Test`.

If DailyBriefing is named and Plan 24 still owns the file:
- land authority + totality gate first;
- defer that consumer;
- record exact follow-up.

Other consumers may be wired only if they are named by the Plan 31/C2[2] contract.

## 6.8 Matrix/generator update

If semantic parity matrix is generated:
- add kind to source model/input;
- regenerate matrix;
- run source gate;
- do not hand-edit output.

If the matrix is maintained through another repository process, follow that process exactly.

## 6.9 Tests

Required:
- total mapping;
- exactly one kind per ID;
- stable expected kind for representative IDs;
- newly registered fake/test ID without kind fails gate;
- consumer grouping/filtering behavior;
- no state mutation;
- deterministic result;
- parity/source gate remains green.

Avoid giant snapshot-only tests as the only semantic guarantee.

## 6.10 Save/determinism

Classification is static/derived:
- no save changes;
- no RNG;
- no campaign state mutation;
- replay fingerprints unchanged.

If implementation unexpectedly needs persisted kind state, stop: that contradicts this authority model and requires design review.

## 6.11 B1 closeout

Update:
- C2[2] closure Plan 31 row;
- vocabulary documentation;
- parity matrix;
- integration/debt ledger;
- consumer status.

If consumer is claim-blocked, status is:
`SEMANTIC AUTHORITY COMPLETE — <consumer> WIRING DEFERRED TO CLAIM HANDOFF`.

## 6.12 B1 non-goals

- no parallel taxonomy;
- no new day-event authority;
- no data schema conversion;
- no unrelated briefing rewrite;
- no speculative consumers.

---

# 7. TASK B2 — C2[2] DEFERRED DELTA: DUCKING, ACQUISITION, 17B DEEP MATRIX

## 7.1 Objective

Close only the remaining verified delta in three deferred C2[2] areas:
1. 17C Phase I alert ducking/concurrency;
2. 17C Phase E acquisition/retry/failure visibility;
3. 17B deep route/visibility test matrix.

The ledger explicitly changes the ducking premise from “absent” to “partially built”. This package begins with a scope map and may legitimately produce zero ducking code if 20C already satisfies the entire contract.

## 7.2 Phase B2.0 — scope map first

Read:
- C2[2] closure report entries;
- 17C Phase I;
- 17C Phase E;
- 17B matrix definition;
- `AudioStateCoordinator`;
- alert priority policy;
- audio bridge/playback failure paths;
- current route/visibility tests.

Produce:
`Contract clause | Current implementation | Current test | Delta | Action`.

No code change before this table exists.

## 7.3 Ducking current-state audit

For `AudioStateCoordinator.ApplyDucking`, record:
- inputs;
- snapshot/state source;
- affected buses/channels;
- amount calculation;
- alert priority input;
- concurrency behavior;
- release behavior;
- lifecycle;
- whether repeated alerts compound gain reduction;
- whether non-alert cues trigger ducking;
- tests already present.

Do not infer behavior from method name alone.

## 7.4 Ducking policy authority

`RADIO_ALERT_PRIORITY` or current equivalent remains policy authority.

The coordinator enforces; it does not invent priorities.

If policy table is not currently consumed:
- wire exact existing values;
- do not rebalance them.

If policy and code disagree, determine which is the newer signed contract before changing either.

## 7.5 Concurrency invariants

For overlapping alerts:
- ducking is bounded;
- duplicate same-class alerts do not multiply attenuation uncontrollably;
- higher-priority behavior follows signed policy;
- release waits for active relevant alerts according to current contract;
- releasing one of several active alerts does not restore too early;
- no alert leak leaves ambience permanently ducked;
- no non-alert cue triggers the path.

Use stable deterministic tests with fake time/state if available; avoid wall clock.

## 7.6 Ducking tests

Minimum:
- one alert enters ducked state;
- release restores;
- non-alert does not duck;
- two concurrent alerts are bounded;
- priority interaction;
- duplicate/repeated alert does not stack beyond bound;
- close/reset restores neutral;
- save/replay unaffected because presentation/audio state is non-simulation state.

If every test passes before production edits and matches Phase I contract, close ducking as VERIFIED-RESOLVED with scope-map evidence.

## 7.7 Acquisition sweep objective

Find audio cue acquisition/playback paths that:
- silently drop missing cue;
- silently fail when player/channel busy;
- retry inconsistently;
- spam logs on repeated missing IDs;
- lack text/result fallback where C2 contract requires it.

This is a bounded sweep over the named audio acquisition surface, not a repository-wide audio rewrite.

## 7.8 Missing-cue behavior

Follow existing distress/audio failure pattern:
- missing cue is explicit;
- log once per stable key/session rule as repository contract defines;
- player-facing text fallback when required;
- no crash;
- no infinite retry;
- no repeated log spam;
- no substitution with unrelated audio unless a canonical fallback exists.

## 7.9 Busy/concurrency acquisition behavior

When playback resource/channel is busy:
- return an honest typed result or current equivalent;
- do not swallow the request silently;
- follow alert-priority policy for preemption/queueing only if already defined;
- do not invent a queue if 17C does not authorize one;
- tests cover the policy branch.

## 7.10 Acquisition tests

Cover:
- missing cue;
- repeated missing cue → bounded logging;
- busy path;
- successful acquisition;
- canonical fallback if one exists;
- bridge result propagation;
- no gameplay-state mutation;
- no deterministic simulation fingerprint change.

Extend the existing selftest rather than creating an isolated test harness that production never uses.

## 7.11 17B matrix inventory

Read the exact 17B matrix scope.

Build:
`Matrix row | Route/visibility state | Existing test | Missing variant | New test target`.

The matrix should test the route/visibility contract, not pad case counts.

Potential row types are included only if present in 17B:
- registered route reachable;
- hidden/locked route not visible;
- visibility transition;
- route parity across registry/switch/flow;
- restore/rebind behavior if the contract includes it;
- keyboard/accessibility if 17B includes presentation.

## 7.12 Test construction rules

- keep each test focused;
- use production route descriptors/registry;
- avoid copying route lists into tests when generator/enumeration can provide canonical set;
- fail loudly when a new route/visibility state is added without coverage if the plan calls for totality;
- run new files alone first;
- maintain execution-time limits.

## 7.13 Audio selftest integration

After ducking/acquisition:
- run existing audio selftest;
- extend it only with checks that directly represent closed gaps;
- preserve existing check semantics;
- investigate any newly exposed old warning rather than blanket-rebaselining.

## 7.14 Radio/bridge neighborhood

Run:
- radio-focused suite;
- bridge selftest;
- any audio state coordinator focused tests;
- lifecycle/leak checks if new subscriptions/state were added.

D3 leak work from prior Wave 8 remains the authority for shutdown leak classification; do not add global cleanup here.

## 7.15 Determinism

Audio/presentation behavior should not change simulation fingerprints.

Prove:
- same simulation replay fingerprint;
- audio concurrency does not write gameplay state;
- no wall-clock-based simulation decision;
- missing/busy cue handling cannot alter campaign domain outcomes unless an existing domain event contract explicitly says it can.

## 7.16 B2 closeout

Update each deferred C2[2] row separately:
- Ducking: `VERIFIED-RESOLVED` or `DELTA IMPLEMENTED`.
- Acquisition: `SWEPT / FAILURE PATHS SEALED`.
- 17B matrix: `MATRIX COMPLETE`.

Attach the scope map so future waves do not reclassify already-landed 20C work as absent.

## 7.17 B2 non-goals

- no audio architecture rewrite;
- no priority rebalance;
- no new mixer system;
- no arbitrary retry queue;
- no test padding;
- no unrelated panel work;
- no global leak suppression.

---

# 8. CROSS-TASK VERIFICATION POLICY

## 8.1 Focused-first ladder

For every change:
1. exact new/changed test;
2. test file;
3. owning subsystem;
4. relevant selftest;
5. build;
6. sanctioned fast aggregate;
7. full suite only when policy or a named diagnostic hypothesis requires it.

Do not use a broad green suite to justify the absence of a focused regression test.

## 8.2 Save/load standard

Any persisted change uses:
1. seeded setup;
2. pre-save authoritative snapshot;
3. save;
4. continuous-run terminal result;
5. fresh-runtime restore;
6. same inputs;
7. terminal state comparison.

Exactly-once ledgers additionally verify that restore does not replay side effects.

## 8.3 Replay standard

For derived/presentation work:
- capture simulation fingerprint before evaluation;
- evaluate/read/open/close;
- capture fingerprint again;
- assert equality.

For a legitimate domain action such as cloud seeding:
- fingerprint changes only because canonical action state changed;
- same seed/input reproduces same transition.

## 8.4 Data standard

Every new authored row must:
- validate;
- resolve references;
- have a runtime consumer;
- be reachable under intended conditions;
- avoid semantic duplication;
- satisfy localization/text-shape requirements;
- be covered by the relevant utilization gate.

## 8.5 UI standard

Changed surfaces must pass:
- route/registry parity where applicable;
- bind/unbind lifecycle;
- keyboard navigation;
- focus behavior;
- disabled/error explanation in text;
- no color-only meaning;
- snapshot review;
- supported-resolution overflow;
- no per-frame refresh when an event seam exists.

## 8.6 Claim-safe completion language

Use exact state language.

Allowed examples:
- `CORE COMPLETE — BRIEFING CONSUMER DEFERRED TO ACTIVE PLAN 24 CLAIM`.
- `SEMANTIC AUTHORITY COMPLETE — CLAIMED CONSUMER NOT TOUCHED`.
- `DUCKING VERIFIED-RESOLVED — NO PRODUCTION CHANGE REQUIRED`.

Forbidden:
- `DONE` when a named acceptance surface remains claim-blocked.
- `BLOCKED` when only stale documentation remains.
- `IMPLEMENTED` when only data exists without a consumer.

---

# 9. TASK ARTIFACT STANDARD

For each task create or update equivalent repository artifacts:

## Premise record
- source blocker;
- `HEAD`;
- active claims;
- current evidence;
- changed premise since prior wave.

## Change matrix
`File/path | Owner | Change | Why | Behavior impact | Save impact | Test`.

## Acceptance matrix
`Requirement | Evidence | Command | Result | Status`.

## Handoff
- terminal state;
- files;
- decisions;
- tests;
- build;
- replay/save/data/UI evidence;
- remaining blocker;
- next safe task.

---

# 10. PART 1 MASTER ACCEPTANCE MATRIX

| Task | Blocker class | Key dependency | Required implementation result | Required proof |
|---|---|---|---|---|
| A1 | unstarted + stale coordination premise | Plan 24 claim state | deterministic crisis predictor; briefing only when claim-safe | predictor tests, Campaign suite, fingerprint, coordination record |
| A2 | data without runtime consumer | A1 only in chain order | canonical cloud-seeding command through weather authority | utilization, weather tests, research/inventory preflights, deterministic action |
| A3 | trophy unstarted + kennel partial | A2 by chain order | trophy chain + only missing kennel remainder | exactly-once/save tests, companion parity, panel/data gates |
| A4 | hardening + content tail | A1–A3 | C1.9 hardening and live-consumed C1.10 tranche | migration/hardening tests, per-batch integrity/utilization, chain close |
| B1 | missing semantic-kind authority | vocabulary/parity owner, Plan 24 claim for some consumers | total kind mapping + safe named consumers | totality gate, parity matrix, Campaign tests |
| B2 | partial/unverified/test gap | current 20C audio state | only actual ducking delta + acquisition closure + deep matrix | scope map, audio/radio/bridge tests, matrix green |

---

# 11. FAILURE ROUTING

## 11.1 If A1 discovers contradictory prediction ownership
Stop. Produce an authority decision memo. Do not make the predictor own underlying domain thresholds.

## 11.2 If A2 cannot make cloud seeding atomic
Create a narrow transaction repair package around existing inventory/weather APIs. Do not ship material loss without effect or free weather effect without cost.

## 11.3 If A3 trophy condition source is absent
Do not create a shadow counter solely for the trophy. Either select a condition supported by the signed baseline catalog or route a source-authority decision.

## 11.4 If A4 new content is dead
Do not suppress utilization. Identify whether the mechanism is incomplete, the row is invalid, or the content target is stale.

## 11.5 If B1 taxonomy conflicts with an existing classifier
Compare and consolidate under the canonical owner; do not ship both.

## 11.6 If B2 ducking is already complete
Close as verified-resolved. Do not create code churn to satisfy an old task description.

---

# 12. MERGE / REVIEW CHECKLIST

Before merge of any task:
- [ ] blocker re-proven at HEAD;
- [ ] active claims checked;
- [ ] correct owner used;
- [ ] no parallel authority;
- [ ] no accidental new save scope;
- [ ] focused tests exist;
- [ ] old-save behavior tested if persistence changed;
- [ ] deterministic/replay implication tested;
- [ ] UI gates tested if UI changed;
- [ ] content integrity/utilization tested if data changed;
- [ ] generated artifacts regenerated from inputs;
- [ ] docs/debt/closeout truth updated;
- [ ] remaining blocker stated exactly.

---

# 13. WAVE 9 PART 1 CLOSEOUT PROCEDURE

When A1–A4/B1–B2 reach terminal states:

1. Re-read the Wave 9 surviving ledger.
2. Compare each Part 1 blocker with current `HEAD`.
3. Confirm C1.4–C1.10 status is truthful.
4. Confirm the stale coordination note no longer points to a vanished agent.
5. Confirm no Plan 24 claimed path was edited without handoff.
6. Confirm cloud-seeding data now has a live runtime consumer or the task is explicitly unresolved.
7. Confirm trophy catalog and award ledger have live consumers and exactly-once restore semantics.
8. Confirm kennel work did not create a second companion authority.
9. Confirm C1.10 rows all validate and are consumed.
10. Confirm the water-sample quirk has a recorded signed disposition if A4 reached it.
11. Confirm semantic kind is total over current vocabulary.
12. Confirm no second semantic registry exists.
13. Confirm ducking status reflects actual current 20C implementation, not historical absence.
14. Confirm acquisition failure paths are explicit and bounded.
15. Confirm 17B matrix covers only its named contract and all rows pass.
16. Run final build.
17. Run verify-fast.
18. Run data integrity and utilization after all A4 content is present.
19. Regenerate/check docs/catalog/parity artifacts.
20. Write `WAVE9_PART1_CLOSEOUT.md` with terminal state per task and exact Part 2 surviving blocker list.

Part 2 should be generated only from blockers that still survive after this closeout.

---

# 14. NON-GOALS FOR PART 1

- no Plan 24 labor/recovery implementation;
- no merchant-restock priority decision;
- no SignalTrust availability consumer decision;
- no radiation F1–F8 balance decision;
- no distress-content expansion beyond content directly required by the six Part 1 packages;
- no F1/F9 governance resolution;
- no test-quarantine mass cleanup;
- no unrelated `water_sample_contaminated` change without A4’s signature step;
- no global audio rewrite;
- no new crisis state owner;
- no second weather owner;
- no second companion owner;
- no alternate day-event vocabulary;
- no schema changes inside A4 content tranche;
- no speculative content to inflate counts.



# APPENDIX A — A1 EXECUTION CARDS

## A1-CARD-01 — Premise snapshot
**Action:** capture the live C1.4 absence, active claim row, C1 completion row, and current Campaign test baseline.
**Evidence:** file/type searches, claim row, commands/results.
**Reject completion if:** the evidence is copied from the Wave 9 ledger without checking current HEAD.

## A1-CARD-02 — Claim split
**Action:** list every target file and mark SAFE / CLAIMED / GENERATED / DOC.
**Evidence:** ownership ledger and path table.
**Reject completion if:** any claimed path is changed before handoff.

## A1-CARD-03 — Input-owner table
**Action:** bind every predictor input to one canonical owner/query.
**Evidence:** exact current API references.
**Reject completion if:** predictor reaches directly into another subsystem’s mutable internals when a read seam exists.

## A1-CARD-04 — Neutral states
**Action:** define empty/missing/dead-roster behavior for every input.
**Evidence:** test cases.
**Reject completion if:** divide-by-zero, null-driven behavior, or guessed defaults remain.

## A1-CARD-05 — Pure output contract
**Action:** define typed prediction record.
**Evidence:** API diff and unit tests.
**Reject completion if:** the record contains presentation strings as domain identity.

## A1-CARD-06 — No-RNG proof
**Action:** record RNG state/fingerprint before/after prediction.
**Evidence:** deterministic test.
**Reject completion if:** repeated reads differ.

## A1-CARD-07 — Threshold reuse
**Action:** reuse canonical threshold/query semantics where available.
**Evidence:** call graph/implementation note.
**Reject completion if:** threshold constants are duplicated for convenience.

## A1-CARD-08 — Multiple-crisis ordering
**Action:** define stable ordering/tie-break.
**Evidence:** test.
**Reject completion if:** hash/dictionary iteration order determines display order.

## A1-CARD-09 — Projected-day test
**Action:** drive a controlled depletion fixture across days.
**Evidence:** horizon assertions based on real owner transitions.
**Reject completion if:** test mutates predictor output directly.

## A1-CARD-10 — Host seam
**Action:** invoke prediction at plan-defined cadence.
**Evidence:** host test.
**Reject completion if:** per-frame evaluation is introduced.

## A1-CARD-11 — Claim-deferred surface
**Action:** if briefing remains claimed, ship explicit deferred record.
**Evidence:** owner/path/condition.
**Reject completion if:** an alternative permanent presentation path is invented merely to avoid coordination.

## A1-CARD-12 — Briefing integration
**Action:** when claim-safe, surface via existing builder/vocabulary.
**Evidence:** parity/vocabulary tests.
**Reject completion if:** free-form parallel report path appears.

## A1-CARD-13 — Save neutrality
**Action:** prove no predictor section added.
**Evidence:** codec/DTO diff and save test.
**Reject completion if:** derived output is persisted without contract requirement.

## A1-CARD-14 — Replay neutrality
**Action:** prediction evaluation must not alter simulation fingerprint.
**Evidence:** replay test.
**Reject completion if:** fingerprint changes without underlying domain change.

## A1-CARD-15 — Closeout truth
**Action:** update C1.4 and coordination note.
**Evidence:** diff and handoff.
**Reject completion if:** status says complete while a required claimed consumer remains unrecorded.

---

# APPENDIX B — A2 EXECUTION CARDS

## A2-CARD-01 — Dead-data census
Enumerate every cloud-seeding reference and its consumer status. Record current utilization output.

## A2-CARD-02 — Authority collision search
Search weather, atmosphere, environment, research, recipe, and action systems for equivalent functionality under another name.

## A2-CARD-03 — Plan semantics freeze
Extract legal weather states, effect, cost, cooldown, unlock, and surface from C1.5. Missing semantics become questions/decisions, not invented defaults.

## A2-CARD-04 — Research preflight
Use canonical knowledge owner. Test locked/unlocked transition.

## A2-CARD-05 — Material preflight
Read recipe through existing data loader. Do not hard-code material list in command.

## A2-CARD-06 — Weather legality
Ask weather authority. Do not duplicate legal-state rules in panel.

## A2-CARD-07 — Atomic commit
Prove materials and effect cannot half-commit. Add a targeted failure-path test.

## A2-CARD-08 — Cooldown owner
Prefer derived day-keyed state; if persistence is required, extend existing weather state only.

## A2-CARD-09 — Deterministic transition
Use canonical seeded stream. Failed preflight consumes no RNG.

## A2-CARD-10 — Equivalent-effect parity
Seeded weather and natural equivalent must use the same effects authority.

## A2-CARD-11 — Host command
Return typed result. UI does not infer success from state changes.

## A2-CARD-12 — UI disabled reasons
Surface research/material/weather/cooldown failures as text.

## A2-CARD-13 — Event-driven refresh
Refresh only on relevant authority changes/action result.

## A2-CARD-14 — Save round trip
Required only if cooldown/action state persists.

## A2-CARD-15 — Content-utilization closure
Cloud-seeding rows must move from dead to consumed; record before/after IDs rather than only aggregate counts.

## A2-CARD-16 — Data-integrity closure
All references valid. New validator logic, if needed, validates the actual consumer contract.

## A2-CARD-17 — Weather suite
Run narrow tests then owning World/Weather neighborhood.

## A2-CARD-18 — Research suite
Run if unlock wiring changed.

## A2-CARD-19 — Snapshot/a11y
Run only for changed visible surface.

## A2-CARD-20 — C1.5 handoff
Provide full data→effect chain and exact save/RNG statement.

---

# APPENDIX C — A3 EXECUTION CARDS

## A3-CARD-01 — Trophy absence recheck
Search catalogs, systems, panels, save DTOs, host sessions, registries, and achievement synonyms.

## A3-CARD-02 — Kennel clause matrix
One row per C1.7/C1.8 requirement; classify existing vs missing.

## A3-CARD-03 — Trophy ID contract
Verify prefix/registry conventions before authoring.

## A3-CARD-04 — Condition-source audit
Every condition must resolve to a canonical source event/query.

## A3-CARD-05 — Reachability
Reject baseline trophy conditions that cannot be achieved with current content unless the source plan explicitly expects future content.

## A3-CARD-06 — No duplicate counters
Use existing source facts; evaluator may aggregate but not become a second owner.

## A3-CARD-07 — Award ledger
Persist IDs only as necessary for exactly-once semantics.

## A3-CARD-08 — Old-save default
Empty award set and no spontaneous replay of past side effects unless migration policy explicitly says otherwise.

## A3-CARD-09 — Restore no-repeat
Eligible awarded trophy stays awarded without firing award event again.

## A3-CARD-10 — Stable ordering
Panel/read model order independent of dictionary iteration.

## A3-CARD-11 — Journal record
One award → one canonical player-visible record.

## A3-CARD-12 — Trophy panel route
Use full route/registry/flow pattern if panel is in C1.6.

## A3-CARD-13 — Trophy accessibility
Keyboard, focus, text semantics.

## A3-CARD-14 — Kennel host reuse
All missing actions route to `CompanionAnimalSystem`.

## A3-CARD-15 — Kennel no-Core-churn
If missing scope is already covered, close with evidence rather than editing Core.

## A3-CARD-16 — Baseline content only
Do not consume A4 +3 trophies or +2/+2 kennel tranche.

## A3-CARD-17 — Integrity
Catalog IDs/references/condition kinds.

## A3-CARD-18 — Utilization
Every authored baseline trophy is evaluated.

## A3-CARD-19 — Companion parity
Existing companion tests remain green.

## A3-CARD-20 — A4 handoff
Record live baseline counts and supported condition kinds for expansion.

---

# APPENDIX D — A4 EXECUTION CARDS

## A4-CARD-01 — Entry gate
Verify A1–A3 terminal states and live consumer availability.

## A4-CARD-02 — Hardening checklist extraction
Use exact C1.9 clauses, not generic assumptions.

## A4-CARD-03 — Save migration baseline
Run before content so failures are not misattributed.

## A4-CARD-04 — Empty catalog cases
Only for catalogs C1.9 explicitly covers.

## A4-CARD-05 — Region fallback
Verify missing/unknown region handling against current authority.

## A4-CARD-06 — Interaction ordering
Pin signed order of embargo/weather/shock effects where required.

## A4-CARD-07 — Live content counts
Remeasure immediately before each batch.

## A4-CARD-08 — Embargo +4
Fill real coverage gaps; avoid semantic duplicates.

## A4-CARD-09 — Atlas +6
Use canonical region IDs and close measured coverage gaps.

## A4-CARD-10 — Crisis advice +8
Author only if live consumer exists and claim-safe.

## A4-CARD-11 — Cloud seeding +1
A2 consumer must exist first.

## A4-CARD-12 — Trophies 8→11
Use A3-supported conditions only.

## A4-CARD-13 — Kennel breeds +2
Canonical species IDs, existing consumer.

## A4-CARD-14 — Kennel events +2
Existing event mechanism; no new Core semantics.

## A4-CARD-15 — Radio flavor +4
Existing selection and priority policy.

## A4-CARD-16 — Per-batch integrity
Stop batch on first invalid reference.

## A4-CARD-17 — Per-batch utilization
No authored dead row.

## A4-CARD-18 — Count-pin rematch
Only after proving live count change.

## A4-CARD-19 — Water-sample memo
No item edit before signature.

## A4-CARD-20 — Registry/docs regeneration
Use generators/checks.

## A4-CARD-21 — Snapshot review
Only intended content-driven diffs.

## A4-CARD-22 — C1 chain state
Do not claim full close while claim-deferred required surface exists.

## A4-CARD-23 — Tranche report
Rows by catalog, references, utilization, tests.

## A4-CARD-24 — Final C1 gate
Build + verify-fast + save migration + integrity/utilization.

---

# APPENDIX E — B1 EXECUTION CARDS

## B1-CARD-01 — Absence proof
Search semantic kind and functional synonyms.

## B1-CARD-02 — Vocabulary owner read
Understand registration, defaults, source gate.

## B1-CARD-03 — Consumer motivation
Use closure report; do not invent.

## B1-CARD-04 — Taxonomy from population
Classify actual events.

## B1-CARD-05 — Category minimality
Avoid near-synonym kinds with no consumer distinction.

## B1-CARD-06 — Total mapping
All current IDs exactly once.

## B1-CARD-07 — Future-ID failure
Test that a newly added unmapped vocabulary ID breaks the gate.

## B1-CARD-08 — Stability
Representative IDs pin reviewed category.

## B1-CARD-09 — Matrix generation
Update source input and regenerate.

## B1-CARD-10 — Claim-aware consumer
Do not touch Plan 24-owned consumer before handoff.

## B1-CARD-11 — Journal/filter consumer
Only if named by Plan 31.

## B1-CARD-12 — No save
Static classification.

## B1-CARD-13 — No RNG
Static classification.

## B1-CARD-14 — Campaign parity
Existing day-event behavior unchanged.

## B1-CARD-15 — Closure report
Strike Plan 31 gap or state authority-complete/consumer-deferred.

---

# APPENDIX F — B2 EXECUTION CARDS

## B2-CARD-01 — C2[2] source extraction
List exact deferred clauses and cited evidence.

## B2-CARD-02 — ApplyDucking call graph
Find every caller and state source.

## B2-CARD-03 — Policy table comparison
Map alert priority clauses to code.

## B2-CARD-04 — Single-alert baseline
Capture current result before edits.

## B2-CARD-05 — Concurrency baseline
Two alerts same/overlapping windows.

## B2-CARD-06 — Bound check
Prove attenuation cannot compound past policy.

## B2-CARD-07 — Release check
Remaining active alert retains required duck.

## B2-CARD-08 — Non-alert exclusion
Non-alert cue never enters alert ducking.

## B2-CARD-09 — Verified-resolved option
If all clauses already pass, make zero production change.

## B2-CARD-10 — Acquisition grep
Enumerate missing/busy failure handling.

## B2-CARD-11 — Missing cue result
Explicit, bounded, non-crashing.

## B2-CARD-12 — Log-once
Repeated failure does not spam.

## B2-CARD-13 — Busy path
Return truthful result; no invented queue.

## B2-CARD-14 — Text fallback
Only where current contract requires.

## B2-CARD-15 — Audio selftest extension
Exercise production path.

## B2-CARD-16 — 17B matrix extraction
List all required rows.

## B2-CARD-17 — Existing coverage map
Do not duplicate tests.

## B2-CARD-18 — Missing focused tests
Create only uncovered variants.

## B2-CARD-19 — Route parity
Use canonical registry/flow.

## B2-CARD-20 — Visibility transitions
Exercise actual state changes.

## B2-CARD-21 — Radio neighborhood
Run after policy enforcement changes.

## B2-CARD-22 — Bridge neighborhood
Run after acquisition/result changes.

## B2-CARD-23 — Replay neutrality
Audio presentation does not affect simulation fingerprint.

## B2-CARD-24 — Closure map
Each deferred row gets explicit final status.

---

# APPENDIX G — REVIEW QUESTIONS BY TASK

## A1 reviewer questions
- Does prediction consume only authoritative reads?
- Can evaluating prediction alter campaign state?
- Is projected-day math supported by the actual C1.4 contract?
- Are all confidence bounds defined?
- Is the Plan 24 claim respected?
- If briefing integration is deferred, is the defer exact and temporary rather than an invented alternate surface?
- Does replay prove purity?
- Does save/restore reproduce the same derived output?
- Are stale coordination notes corrected?

## A2 reviewer questions
- Is cloud-seeding data actually consumed at runtime?
- Does the command use canonical inventory and weather owners?
- Is action atomic?
- Does failure consume no materials and no RNG?
- Is research gating canonical?
- Are natural and seeded weather effects unified?
- Is cooldown derived where possible?
- Are save changes justified?
- Does utilization prove the dead-data blocker is gone?

## A3 reviewer questions
- Did trophy implementation avoid duplicate source counters?
- Are conditions reachable?
- Is award exactly once?
- Does restore avoid duplicate award events?
- Is the kennel delta limited to real missing clauses?
- Were existing Plan 174 paths reused?
- Did A3 avoid A4’s content tranche?
- Are panel routes/lifecycle/a11y complete?

## A4 reviewer questions
- Did hardening precede content?
- Were live counts remeasured?
- Is every new row consumed?
- Were count-pin tests updated only after actual data change?
- Is crisis content blocked if consumer is claim-blocked?
- Did A4 avoid schema/mechanism changes?
- Is the water-sample decision signed?
- Is C1 closure language truthful?

## B1 reviewer questions
- Is there one semantic-kind mapping?
- Is it total?
- Does new vocabulary without kind fail?
- Was taxonomy derived from actual events/consumer need?
- Are claim-controlled consumers respected?
- Is parity matrix generated correctly?
- Is classification save/RNG neutral?

## B2 reviewer questions
- Did the team prove the ducking delta before editing?
- If no delta existed, was it closed with evidence instead of churn?
- Are concurrent alerts bounded?
- Is release correct?
- Are missing/busy cue failures explicit?
- Is logging bounded?
- Does 17B matrix cover actual missing rows rather than inflate test counts?
- Are audio changes simulation-neutral?

---

# APPENDIX H — HANDOFF TEMPLATES

## H.1 Task handoff

**Task:**<br>
**Terminal state:**<br>
**HEAD / worktree:**<br>
**Original blocker:**<br>
**Premise re-verification:**<br>
**Active claims checked:**<br>
**Paths claimed:**<br>
**Authority owners:**<br>
**Files changed:**<br>
**Behavior before:**<br>
**Behavior after:**<br>
**Save impact:**<br>
**Determinism/RNG impact:**<br>
**UI impact:**<br>
**Data impact:**<br>
**Focused tests:**<br>
**Subsystem suites:**<br>
**Selftests:**<br>
**Build:**<br>
**Generators/checks:**<br>
**Snapshots/a11y:**<br>
**Docs/debt/closeout updated:**<br>
**Known remaining blocker:**<br>
**Next safe task:**<br>

## H.2 Claim-deferred consumer handoff

**Completed authority/mechanism:**<br>
**Deferred consumer:**<br>
**Claim owner:**<br>
**Exact blocked paths:**<br>
**Why no workaround was created:**<br>
**Required integration action after handoff:**<br>
**Tests already available:**<br>
**Tests still required:**<br>
**Condition to resume:**<br>

## H.3 Verified-resolved handoff

**Historical blocker:**<br>
**Current implementation proving closure:**<br>
**Commit/current evidence:**<br>
**Contract comparison:**<br>
**Tests run:**<br>
**Production changes:** none<br>
**Docs corrected:**<br>
**Reason no new implementation was needed:**<br>

---

# APPENDIX I — FINAL PART 1 RELEASE GATE

Part 1 is ready to hand off only when the following is true:

- A1 has a complete crisis predictor or a documented live-premise change proving it already exists.
- A1 never edited Plan 24 claim paths without handoff.
- A2 cloud-seeding family has a runtime consumer and utilization evidence.
- A3 trophy baseline is real, exactly-once, save-safe, and reachable.
- A3 kennel gap matrix has no unexplained OPEN row.
- A4 hardening is green before the content tranche is accepted.
- A4’s authored rows all have live consumers.
- A4’s water-sample quirk is signed or explicitly left outside the completed tranche with decision blocker preserved.
- B1 semantic mapping is total and enforced.
- B1 did not create a second vocabulary.
- B2 ducking status reflects present implementation.
- B2 missing/busy acquisition paths are explicit.
- B2 17B deep matrix is complete within named scope.
- Build passes.
- Verify-fast passes.
- Data integrity passes.
- Content utilization passes.
- Generated artifact checks pass.
- No new standing red test is left unnamed.
- Every remaining blocker is named for Wave 9 Part 2 rather than hidden in “follow-up”.

**End of Wave 9 Part 1 implementation plan.**


# APPENDIX J — NEGATIVE-CASE REGISTRY

This registry is mandatory because blocker-unblocking work often passes happy-path tests while preserving the original edge-condition failure. Each row below must either be implemented as a focused test or marked NOT APPLICABLE with a source-contract reason.


## A1 negative-case checklist
- [ ] **all resource inputs healthy** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **one resource exactly at warning threshold** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **empty stock with nonzero population** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **empty roster** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **missing optional domain input** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **multiple crises with equal projected day** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **already-active crisis** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **save/restore same-state prediction** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **repeated read** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **briefing consumer unavailable because claim remains active** — source contract checked; expected authoritative state written; focused test/evidence attached.

## A2 negative-case checklist
- [ ] **research locked** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **research unlocked but materials missing** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **materials present but weather illegal** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **cooldown active** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **successful action** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **second transaction leg failure** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **panel reopened without action** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **same seeded state repeated** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **old save without cooldown field** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **dead-data utilization before/after** — source contract checked; expected authoritative state written; focused test/evidence attached.

## A3 negative-case checklist
- [ ] **trophy condition false** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **condition becomes true once** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **condition remains true across later days** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **save immediately after award** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **restore and reevaluate** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **old save before trophy system** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **invalid reference in trophy catalog** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **new trophy condition unsupported by evaluator** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **kennel clause already covered** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **kennel missing command failure** — source contract checked; expected authoritative state written; focused test/evidence attached.

## A4 negative-case checklist
- [ ] **empty target catalog where hardening requires safe behavior** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **missing region** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **old save version** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **corrupt section** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **new catalog row with bad reference** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **new row valid but unreachable** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **live count already above historical target** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **A1 consumer still claim-blocked** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **water sample decision retains current state** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **water sample decision changes classification** — source contract checked; expected authoritative state written; focused test/evidence attached.

## B1 negative-case checklist
- [ ] **every registered event maps** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **test-only new event lacks mapping** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **duplicate mapping** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **representative kind changes unexpectedly** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **consumer requests kind for valid event** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **claim-blocked briefing consumer** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **matrix regeneration** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **no save/RNG mutation** — source contract checked; expected authoritative state written; focused test/evidence attached.

## B2 negative-case checklist
- [ ] **single alert** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **two equal-priority alerts** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **two different-priority alerts** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **release one while one remains** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **release final alert** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **non-alert cue** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **missing cue** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **repeated missing cue** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **busy player/channel** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **existing route visible** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **locked route hidden** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **visibility transition** — source contract checked; expected authoritative state written; focused test/evidence attached.
- [ ] **new matrix row uncovered** — source contract checked; expected authoritative state written; focused test/evidence attached.


# APPENDIX K — DISCOVERY WORKSHEETS

Fill these from live repository evidence before implementation. The worksheets deliberately use role descriptions rather than invented file names.

## K.1 A1 discovery
- C1.4 plan path:
- C1 completion path:
- active claim row:
- Campaign composition/root:
- food read API:
- water read API:
- power read API:
- radiation read API:
- sanitation/disease read API:
- distress pending read API:
- cohort/population read API:
- intended host seam:
- intended briefing consumer:
- vocabulary owner:
- predictor test namespace:
- replay/fingerprint harness:

## K.2 A2 discovery
- cloud seeding IDs:
- recipe row:
- research row:
- manual/library row:
- weather owner:
- severity/effects owner:
- weather RNG/transition seam:
- canonical inventory consume API:
- research query API:
- action host seam:
- intended player surface:
- save owner if cooldown persists:
- integrity validator:
- utilization validator:
- weather focused tests:

## K.3 A3 discovery
- trophy catalog registration pattern:
- comparable exactly-once ledger:
- save DTO owner:
- collectible event/query:
- species encounter event/query:
- milestone event/query:
- journal event seam:
- player-surface registry:
- KennelPanel path:
- CompanionAnimalSystem path:
- kennel host session:
- C1.7/C1.8 clauses:
- Plan 174 relevant tests:
- baseline content counts:

## K.4 A4 discovery
- C1.9 exact checklist:
- save corruption/migration test:
- current embargo count:
- current atlas count:
- current crisis-advice count:
- current cloud-seeding recipe count:
- current trophy count:
- current kennel breed/event counts:
- current radio flavor count:
- catalog registry generator:
- docs index generator:
- count-pin tests:
- water-sample item row:
- claim 17 deferred note:

## K.5 B1 discovery
- C2[2] closure report:
- Plan 31 plan:
- DayEventVocabulary owner:
- event enumeration API:
- parity matrix source:
- parity matrix generator:
- source gate tests:
- named consumers:
- current claim state:
- Campaign focused tests:

## K.6 B2 discovery
- 17C Phase I contract:
- 17C Phase E contract:
- 17B matrix contract:
- AudioStateCoordinator:
- ApplyDucking callers:
- current snapshot model:
- alert priority policy:
- playback acquisition path:
- missing-cue behavior:
- busy behavior:
- bridge result type:
- audio selftest:
- route registry:
- visibility model:
- existing 17B tests:
- leak/lifecycle selftests:

# APPENDIX L — COMMIT BOUNDARIES, ROLLBACK POINTS, AND EVIDENCE PACKS

This appendix defines the smallest reviewable implementation units. The goal is to prevent a blocker-unblocking task from becoming one monolithic change where authority migration, UI wiring, authored content, and test rematching are impossible to review independently.

## L.1 A1 recommended commit sequence

### A1-1 — Premise/coordination evidence
**Contents:**
- claim-state record;
- blocker re-verification;
- input-owner matrix;
- no production behavior change.

**Gate:**
- docs/evidence only;
- no claimed path touched.

**Rollback:** trivial document rollback.

### A1-2 — Predictor type + pure algorithm
**Contents:**
- typed prediction record;
- pure evaluator;
- narrow unit tests.

**Gate:**
- repeated same-state evaluation identical;
- no state fingerprint change;
- no RNG change.

**Rollback trigger:**
- evaluator needs mutation or duplicated subsystem policy.

### A1-3 — Host/query seam
**Contents:**
- canonical invocation seam outside claimed files;
- host tests.

**Gate:**
- no per-frame evaluation;
- no save change;
- no presentation dependency.

### A1-4 — Consumer integration
**Contents only when claim-safe:**
- DailyBriefing/journal consumer;
- vocabulary/parity update;
- snapshot/a11y evidence.

**Gate:**
- Plan 24 ownership formally clear;
- existing report authority reused.

If claim remains active, this commit is not created; deferred handoff becomes the terminal consumer artifact.

### A1-5 — closeout
**Contents:**
- C1 completion update;
- stale coordination correction;
- handoff matrix.

**Do not squash away** the claim-resolution evidence if project workflow benefits from preserving why the split occurred.

---

## L.2 A2 recommended commit sequence

### A2-1 — consumer census and C1.5 contract
Evidence only. Captures current dead references and authoritative owner seams.

### A2-2 — Core preflight/command
Contains:
- research/material/weather checks;
- typed result;
- no panel work.

Focused test gate:
- all failure paths no-op;
- success uses canonical authority.

### A2-3 — atomicity + cooldown
Only if needed by actual C1.5 contract.

Gate:
- no half-commit;
- old-save neutral if persisted field added;
- deterministic behavior.

### A2-4 — host/UI surface
Contains:
- command routing;
- disabled reasons;
- result rendering;
- lifecycle/a11y.

Must not contain weather math.

### A2-5 — utilization/integrity + closeout
Contains:
- validator/utilization updates if actually required;
- C1.5 status;
- before/after dead-reference evidence.

Rollback point is A2-2 if UI behavior exposes a domain flaw; fix domain transaction before proceeding.

---

## L.3 A3 recommended commit sequence

### A3-1 — kennel gap matrix + trophy source matrix
Evidence only.

### A3-2 — trophy catalog registration/schema use
No award evaluator yet unless repository conventions require atomic introduction.

Gate:
- integrity validates;
- IDs/references legal.

### A3-3 — evaluator + exactly-once ledger
Gate:
- award once;
- restore no-repeat;
- old save neutral.

### A3-4 — journal/host read model
Gate:
- one award produces one visible canonical record.

### A3-5 — trophy panel
Only if required by C1.6.

Gate:
- route;
- lifecycle;
- a11y;
- deterministic ordering;
- snapshots.

### A3-6 — kennel delta
One commit per meaningful missing C1.7/C1.8 clause family. If gap matrix says nothing missing, do not create this commit.

### A3-7 — closeout
Includes:
- exact live baseline counts for A4;
- utilization;
- companion parity;
- C1.6–C1.8 status.

---

## L.4 A4 recommended commit sequence

A4 should be intentionally granular because it mixes verification and content.

### A4-1 — C1.9 hardening tests/fixes
No content tranche yet.

Gate:
- save migration/corruption;
- exact hardening checklist green.

### A4-2 — embargo tranche
Data only + required count-pin/test changes.

### A4-3 — atlas tranche
Data only + required coverage tests.

### A4-4 — crisis advice/radio content
Only if consumers live and claim-safe.

### A4-5 — cloud-seeding recipe
Only after A2 consumer.

### A4-6 — trophy tranche
Only after A3 evaluator.

### A4-7 — kennel tranche
Only after A3 kennel/companion mechanism.

### A4-8 — water-sample signed disposition
Separate from content so the product decision is reviewable.

### A4-9 — generated artifacts / tranche report / C1 closeout
Gate:
- integrity;
- utilization;
- focused suites;
- build;
- verify-fast.

Each authored batch must remain revertible without affecting unrelated catalogs.

---

## L.5 B1 recommended commit sequence

### B1-1 — taxonomy evidence table
No code.

### B1-2 — semantic kind mapping + totality test
No consumers yet.

Gate:
- every current vocabulary ID exactly once;
- unmapped future/test ID fails.

### B1-3 — parity matrix/generator integration
Gate:
- generated artifact check passes.

### B1-4 — safe consumer wiring
One consumer family per commit.
Claim-blocked consumer omitted.

### B1-5 — closeout
C2[2] row updated to authority-complete or fully closed.

---

## L.6 B2 recommended commit sequence

### B2-1 — scope-map evidence
No production changes.

### B2-2 — ducking delta
Created only if scope map proves a delta.

### B2-3 — acquisition failure paths
Missing/busy/log-once behavior.

### B2-4 — selftest additions
Production path exercised.

### B2-5 — 17B matrix
Focused test files only.

### B2-6 — closeout
Each deferred row gets independent disposition.

If B2-1 proves ducking complete, record VERIFIED-RESOLVED and skip B2-2 entirely.

---

# APPENDIX M — TASK-SPECIFIC ROLLBACK MATRIX

| Task | Finding | Required action | Prohibited shortcut |
|---|---|---|---|
| A1 | predictor needs mutable write access | stop and re-evaluate authority | write through predictor |
| A1 | DailyBriefing still claimed | defer consumer | edit claimed file or invent parallel permanent UI |
| A1 | canonical subsystem already forecasts same crisis | compose/reuse it | copy formulas |
| A2 | material consumed but weather commit can fail | add/repair atomic transaction seam | accept partial loss |
| A2 | no legal weather effect mapping exists | decision/re-scope | create hidden second effects table |
| A2 | cooldown requires unclear persistence owner | stop for ownership decision | new top-level save blob |
| A3 | trophy source fact does not exist | revise signed condition or route source gap | add shadow counter |
| A3 | kennel clause already landed | close with evidence | rewrite for churn |
| A3 | trophy restore re-awards | fix ledger/restore semantics | suppress UI duplicate only |
| A4 | authored row has no consumer | stop batch | whitelist dead row |
| A4 | live count already meets target | record no-op/adjust truthful delta | duplicate rows to hit historical increment |
| A4 | count-pin fails unexpectedly | investigate schema/data truth | blindly change expected count |
| B1 | equivalent classifier already exists | consolidate/verified-resolve | second registry |
| B1 | vocabulary ownership differs from plan | follow live owner and document premise change | force old plan architecture |
| B2 | Phase I already fully implemented | verified-resolve | rewrite ducking |
| B2 | busy behavior has no policy | route decision | invent queue/preemption |
| B2 | matrix row duplicates existing test | reuse/record coverage | add redundant case for count |

---

# APPENDIX N — EVIDENCE TABLES TO FILL DURING EXECUTION

## N.1 A1 evidence table

| Requirement | Live source/path | Before state | After state | Focused test | Result |
|---|---|---|---|---|---|
| prediction authority absent/present |  |  |  |  |  |
| food input |  |  |  |  |  |
| water input |  |  |  |  |  |
| power input |  |  |  |  |  |
| radiation input |  |  |  |  |  |
| disease/sanitation input |  |  |  |  |  |
| distress input |  |  |  |  |  |
| cohort input |  |  |  |  |  |
| deterministic output |  |  |  |  |  |
| no mutation |  |  |  |  |  |
| briefing consumer |  |  |  |  |  |
| Plan 24 claim status |  |  |  | n/a |  |

## N.2 A2 evidence table

| Requirement | Canonical owner | Before | After | Test | Result |
|---|---|---|---|---|---|
| research unlock |  |  |  |  |  |
| material preflight |  |  |  |  |  |
| weather legality |  |  |  |  |  |
| transition/effect |  |  |  |  |  |
| cooldown |  |  |  |  |  |
| atomicity |  |  |  |  |  |
| deterministic RNG |  |  |  |  |  |
| UI command |  |  |  |  |  |
| utilization |  |  |  |  |  |
| integrity |  |  |  |  |  |

## N.3 A3 trophy evidence table

| Trophy/condition kind | Source owner | Runtime evaluator | Exactly-once key | Save test | Reachability | Status |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## N.4 A3 kennel evidence table

| C1 clause | Current CompanionAnimal support | Current panel support | Missing delta | Change | Test | Status |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## N.5 A4 content tranche table

| Catalog | Live start count | Intended target/delta | Rows added | Consumer | Integrity | Utilization | Count-pin update |
|---|---:|---:|---:|---|---|---|---|
| embargo |  |  |  |  |  |  |  |
| atlas |  |  |  |  |  |  |  |
| crisis advice |  |  |  |  |  |  |  |
| cloud seeding |  |  |  |  |  |  |  |
| trophies |  |  |  |  |  |  |  |
| kennel breeds |  |  |  |  |  |  |  |
| kennel events |  |  |  |  |  |  |  |
| radio flavor |  |  |  |  |  |  |  |

## N.6 B1 taxonomy evidence table

| Event ID | Current meaning | Semantic kind | Named consumer | Totality covered | Parity matrix | Notes |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## N.7 B2 scope map

| Contract clause | Current implementation | Existing test | Delta | Production change | New test | Final status |
|---|---|---|---|---|---|---|
| alert ducking |  |  |  |  |  |  |
| concurrency |  |  |  |  |  |  |
| release |  |  |  |  |  |  |
| missing cue |  |  |  |  |  |  |
| busy cue |  |  |  |  |  |  |
| 17B route matrix |  |  |  |  |  |  |
| 17B visibility matrix |  |  |  |  |  |  |

---

# APPENDIX O — PART 1 → PART 2 HANDOFF FILTER

Wave 9 Part 2 must not simply copy every remaining ledger bullet. It should be generated from a fresh post-Part-1 blocker census.

For each candidate remaining blocker, apply:

1. **Re-verify:** does the blocker still exist at current `HEAD`?
2. **Classify:** missing mechanism, missing consumer, unsigned decision, content-empty, governance, test quarantine, balance decision, or stale truth.
3. **Check dependency:** did Part 1 create an owner/consumer that changes the blocker shape?
4. **Check claim state:** is any required path actively owned?
5. **Check decision state:** did the foreman sign anything during Part 1 that resolves it?
6. **Check duplication:** did another system already satisfy the intended capability?
7. **Bound package:** can the work be expressed as one authority-safe implementation or decision package?
8. **Define acceptance:** can closure be proven by focused tests/evidence?
9. **Reject speculative promotion:** if product semantics remain undefined, keep it decision-gated.
10. **Record changed premise:** Part 2 must state how the blocker differs from earlier waves.

Likely Part 2 candidates from the supplied Wave 9 ledger include the still-surviving decision/content/test/governance items, but their inclusion is conditional on this fresh census. Part 1 closeout is the authority for what survives.

## O.1 Mandatory Part 2 candidate record

For each surviving item:

**Blocker:**<br>
**Live evidence:**<br>
**Changed since Wave 9 source ledger:**<br>
**Owner:**<br>
**Required decision:**<br>
**Dependencies now satisfied:**<br>
**Dependencies still missing:**<br>
**Claim state:**<br>
**Package type:**<br>
**Focused acceptance:**<br>
**Do not include if:**<br>

This prevents Part 2 from becoming stale before implementation begins.

---

# APPENDIX P — FINAL QUALITY BAR

The Wave 9 Part 1 package is considered flagship-quality only if:

- an implementation agent can begin each task without needing to infer the owner;
- decision/claim stops are explicit before edit steps;
- every mechanism has a player/runtime consumer or an honest defer;
- every persisted behavior has restore evidence;
- every derived read model is proven non-mutating;
- every content batch has utilization evidence;
- every UI action surfaces authoritative result/failure;
- every generated artifact has a source/input path and a check;
- every historical premise that changed is corrected;
- no task measures success by code volume;
- verified no-op closure is accepted when current code already satisfies the contract;
- no task declares completion while a required named acceptance item remains hidden;
- the final closeout can enumerate exactly what Wave 9 Part 2 still needs to solve.
