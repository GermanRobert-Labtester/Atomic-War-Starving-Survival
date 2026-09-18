# ASHFALL — GENERATION WAVE 9 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 2

**Purpose:** execution-grade continuation of the Wave 9 surviving-blocker ledger.

**Scope:** only the six remaining Wave 9 packages that survived the source ledger’s `HEAD` re-verification:
1. **C1 — Merchant Restock Priority**
2. **C2 — SignalTrust Availability Consumer**
3. **C3 — Radiation Balance Findings F1–F8**
4. **D1 — Distress Follow-Up / Audio Content Tranches**
5. **D2 — F1/F9 Governance Decisions**
6. **D3 — Test Quarantine Promotion Continuation**

This document assumes Wave 9 Part 1 is executed or independently tracked. It does not duplicate A1–A4/B1–B2 implementation work. Every task below re-verifies its blocker before edits because decisions, active claims, current data counts, and quarantined test state can change while Part 1 is executing.

**Quality bar:** no speculative systems, no policy invented by implementers, no broad cleanup disguised as blocker work, no filler. Every section exists to define authority, decision scope, implementation order, evidence, rollback, verification, or closeout.

---

# 0. WAVE 9 PART 2 OPERATING CONTRACT

## 0.1 Allowed terminal states

Each task ends in one of:

- **IMPLEMENTED** — signed or non-decision work landed through the correct owner and all acceptance gates pass.
- **DECIDED-DEFERRED** — the missing decision is now explicitly resolved as “do not implement / retain current behavior,” with the blocker removed from the ambiguous ledger.
- **RETIRED** — a dormant/obsolete policy or artifact is intentionally tombstoned with evidence.
- **VERIFIED-RESOLVED** — `HEAD` already satisfies the historical blocker; no production change needed.
- **ROUTED-REPAIR** — a newly discovered production regression is split into a dedicated repair package rather than hidden inside decision/content/test-debt work.
- **BATCH-PARTIAL** — allowed only for D1/D3 where the source explicitly authorizes bounded tranches/batches. The batch itself must be complete, and remaining rows/files must be enumerated.

“Still waiting”, “mostly done”, “tests updated”, or “reviewed” are not terminal statuses.

## 0.2 Mandatory execution rules

1. Re-verify the task premise at current `HEAD`.
2. Check active ownership claims before edits.
3. Stop at every foreman signature gate.
4. Implementation follows signed policy; it does not create policy.
5. Existing authority remains the sole state owner.
6. Derived policy/read state should not create new save sections.
7. New persisted state requires additive compatibility and explicit old-save defaults.
8. Same seed + same state remains deterministic.
9. UI reopening/refreshing does not create state changes unless the domain command explicitly runs.
10. Data edits are permitted only after signed balance/product decisions where the source requires them.
11. Content batches do not modify mechanisms unless a genuine runtime bug is discovered and separately routed.
12. Quarantined tests are promoted one file at a time under the repository’s promotion protocol.
13. Production APIs are not bent to satisfy stale quarantined tests.
14. Generated docs/indices/manifests are changed through their source and regenerated.
15. No full-suite run unless repository policy or a named diagnostic hypothesis requires it.

## 0.3 Per-task evidence bundle

Every task or bounded batch must include:

- blocker re-verification;
- current active-claim state;
- decision memo/signature where applicable;
- authority/owner map;
- implementation or content/test change matrix;
- focused test command/result;
- broader owning-suite result;
- save/replay evidence if applicable;
- data integrity/utilization evidence if applicable;
- build result;
- generated-artifact checks;
- closeout/debt/ledger update;
- explicit remaining blockers.

## 0.4 Hard stop conditions

Stop work if:
- a signature-gated policy is still unsigned;
- the current owner differs from the historical plan and the difference changes architecture;
- an implementation would create a second selection/restock/trust/radiation/audio authority;
- a balance sweep contradicts the old findings materially;
- a distress row cannot be consumed by existing mechanisms;
- a governance action risks deleting live local workspace files instead of merely untracking them;
- a quarantined test depends on retired behavior and no current contract justifies restoration;
- a test promotion requires touching an active-claim owner’s production/test files.

---

# 1. DEPENDENCY AND EXECUTION ORDER

Recommended order:

1. **C1 decision memo** — independent product decision.
2. **C2 decision memo** — independent product decision.
3. **C3 current balance sweep + decision memo** — decision before data edits.
4. **D2 governance memos** — independent repo policy work.
5. **D1 first content tranche** — mechanism already complete.
6. **D3 quarantine batch** — bounded and independent except for active claims.
7. Return to **C1/C2/C3/D2** for execution after signatures.
8. Run Part 2 aggregate closeout and Wave 10 re-verification census.

Parallel work is allowed only when claims, generated artifacts, and ledger files do not overlap. Decision memos can be prepared in parallel, but execution waits on their individual signatures.

---

# 2. TASK C1 — MERCHANT RESTOCK PRIORITY: SIGNED DESIGN → OWNER EXTENSION

## 2.1 Objective

Resolve the longstanding ambiguity around merchant restock “priority” without disturbing the already-correct day-gate, per-arrival restock, pinned-stock, and no-reroll semantics. The implementer must first determine exactly what “priority” means under current economy architecture, obtain a signed decision, then extend `ShelterBarterSystem.RestockCaravan` or its current owner minimally.

## 2.2 Premise re-verification

Before writing the memo:
- locate current `ShelterBarterSystem.RestockCaravan`;
- verify restock is still day/arrival gated;
- verify same-day reopen does not reroll;
- verify stock remains pinned after evaluation;
- locate current dynamic economy category indices;
- locate trade-pressure state;
- locate active shock state and v2/v3 economy modifiers;
- locate Plan 147 completion report and 210–213 deferral;
- identify any newer restock-related work since the deferral.

If current source already contains priority ordering, compare it with the historical deferral and close as VERIFIED-RESOLVED only after tests prove policy and determinism.

## 2.3 Decision scope

The signed memo must decide exactly one of these semantics, or another equally explicit variant:

### Option A — selection priority
Priority changes **which** eligible stock items are selected first when capacity/slot constraints prevent stocking everything.

Blast radius:
- selection outcomes;
- deterministic ordering;
- potentially player-accessible supply mix.

### Option B — quantity priority
Priority changes **how much** of selected items is stocked.

Blast radius:
- quantities;
- stronger balance impact;
- requires bounded quantity rule and more simulation evidence.

### Option C — evaluation/display order only
Priority changes ordering without changing selected stock/quantity.

Blast radius:
- smallest;
- may satisfy only presentation/inspection intent, not scarcity response.

### Option D — keep current unordered/current behavior
No production change. Deferral closes as DECIDED-DEFERRED.

The memo must identify which interpretation best matches the original plan intent. Implementers cannot silently choose the highest-impact interpretation.

## 2.4 Inputs allowed in the design

Only authoritative current state:
- scarcity/category index;
- stock deficit;
- active economic shock;
- trade pressure or equivalent current economy pressure;
- item identity for deterministic tie-break.

Do not use:
- UI sort state;
- random roll;
- hidden player preference;
- local panel cache;
- unrelated reputation/trust unless signed.

## 2.5 Priority formula requirements

If signed:
- pure function;
- deterministic;
- bounded;
- stable tie-break;
- no new RNG;
- no new save state;
- re-evaluated only at restock event;
- same restock event uses one coherent snapshot of inputs;
- stock remains pinned until next legal restock.

Create:
`RestockCandidate -> PriorityScore -> StableRank`.

Tie-break should use an existing canonical stable item ordering or ID ordinal. Do not rely on hash-map enumeration.

## 2.6 Legacy parity mode

The signed memo must define a neutral/default behavior.

Preferred:
- when priority feature is disabled/not bound/not applicable, selection output is byte-for-byte or set/order equivalent to current pre-feature behavior;
- old saves require no migration;
- no new fields.

If priority is unconditional, parity is instead pinned for scenarios where all candidates have equal score.

## 2.7 Implementation boundary

Allowed changes:
- priority computation in existing restock owner;
- deterministic ordering before current selection loop;
- read-model exposure if existing market UI already has an appropriate restock-inspection field;
- focused tests;
- docs/closeout.

Not allowed:
- separate RestockPrioritySystem;
- restock button;
- per-panel sorting that masquerades as domain priority;
- global economy rebalance;
- restock cadence changes.

## 2.8 Transaction and lifecycle invariants

- one arrival/restock event → one evaluation;
- reopening market does not recompute stock;
- same-day UI interaction does not consume RNG;
- restock result remains save-compatible under existing caravan/market state;
- if stock is persisted today, the same persisted result remains authoritative;
- priority affects only the legal restock edge.

## 2.9 Focused test matrix

Must include:
1. deterministic order from identical state;
2. stable tie-break;
3. scarcity change moves expected candidate relative to another where signed policy says it should;
4. active shock changes expected candidate where signed;
5. neutral/no-priority parity;
6. same-day reopen no-reroll;
7. save/restore after restock preserves pinned stock;
8. next legal restock re-evaluates priority;
9. panel read, if added, does not trigger restock;
10. identical seed/state replay produces identical restock ledger.

## 2.10 Cross-system scenario

Extend an existing winter/economy scenario only with a **new** assertion/case. Do not mutate historical baseline pins.

Example shape:
- create scarcity/shock conditions;
- execute legal restock edge;
- assert signed priority effect;
- preserve conservation and stock pinning.

## 2.11 30-day simulation

If restock participates in the sanctioned 30-day lifecycle harness:
- run it;
- assert no stock creation/destruction anomalies beyond normal market rules;
- assert no reroll from repeated panel visits;
- compare pre/post priority policy metrics only where signed behavior intentionally changes them.

## 2.12 UI/read model

Only if existing market UI meaningfully exposes restock reasoning:
- read model may expose rank/reason;
- no UI recomputation;
- no “priority” text if domain does not provide it;
- accessibility text must describe state, not infer hidden probabilities.

No new panel is required.

## 2.13 Verification order

1. new priority unit tests;
2. restock owner tests;
3. Plan 147 focused tests;
4. economy suite;
5. same-day no-reroll regression;
6. replay/fingerprint;
7. 30-day selftest if in lifecycle;
8. build;
9. integrity;
10. verify-fast.

## 2.14 C1 decision memo template

**Current behavior:**<br>
**Historical deferral:**<br>
**Current economy inputs:**<br>
**Interpretation options:**<br>
**Recommended blast radius:**<br>
**Formula:**<br>
**Bounds:**<br>
**Tie-break:**<br>
**Persistence:** none/explicit justification<br>
**Legacy parity:**<br>
**No-reroll preservation:**<br>
**Tests:**<br>
**Chosen option:**<br>
**Foreman signature/date:**<br>

## 2.15 C1 closeout

Update:
- 210–213 deferral;
- Plan 147 closeout;
- balance corpus index if memo is part of balance corpus;
- integration ledger.

Terminal status:
- IMPLEMENTED, or
- DECIDED-DEFERRED (“keep current ordering”).

## 2.16 C1 rollback rules

Rollback/stop if:
- policy cannot be expressed through current restock owner;
- priority requires hidden mutable state;
- same-day reopen changes stock;
- tests reveal existing stock pinning is already broken;
- signed memo and current economy authority conflict.

---

# 3. TASK C2 — SIGNALTRUST AVAILABILITY CONSUMER: BUILD THE POOL OR RETIRE THE POLICY

## 3.1 Objective

Resolve a tested-but-dormant `SignalTrustAvailability` policy. The task ends either with:
- a signed, deterministic, canonical selection-pool consumer, or
- an explicit retirement/tombstone of the dormant policy.

The key product decision is whether trust should alter future distress-signal availability/pacing.

## 3.2 Premise re-verification

Before decision memo:
- locate `SIGNAL_TRUST_CONTRACT.md` §5;
- read `SignalTrustAvailability`;
- verify its bounds/monotonic/order-preserving behavior;
- locate trust ledger persistence;
- map current signal detection/selection path;
- verify whether a dynamic selection pool still does not exist;
- locate `RadioHostSession` selection point;
- locate follow-up scheduler and exactly-once ledgers;
- locate replay/fingerprint harness.

If a selection pool has since landed, test whether it already consumes trust; if yes, close stale deferral with evidence.

## 3.3 Decision options

### Option A — build trust-weighted availability pool
Trust modifies candidate availability weight inside current selection owner.

Must preserve:
- one selection authority;
- bounded weight envelope;
- deterministic day-keyed selection;
- independent follow-up scheduler semantics;
- no additional trust persistence.

### Option B — retire/dormant policy
Keep class/tests only as documented dormant utility or remove/tombstone according to repository policy.

Must:
- mark contract clearly;
- remove ambiguous “awaiting future seam” language;
- ensure no current code relies on it;
- preserve unit test if useful as contract tombstone, or retire test with documented reason.

## 3.4 Selection-owner forensic

Map current selection process:
`candidate source → eligibility filter → scheduling/day gate → deterministic ordering/random fork → interception/detection edge → presentation/audio`.

Identify exactly where availability weighting can enter without creating a second selection path.

If current selection is a deterministic catalog walk, the memo must explain how weighted selection changes pacing and legacy behavior.

## 3.5 Availability input contract

The pool may consume:
- canonical trust ledger/state;
- existing candidate eligibility;
- current day/seed;
- signal identity;
- existing sender/source categories if already part of contract.

Do not introduce:
- second trust score;
- hidden panel-local weights;
- mutable availability cache unless strictly necessary and signed;
- wall-clock time.

## 3.6 Deterministic RNG contract

If Option A:
- dedicated day-keyed seeded fork if current policy requires random weighted selection;
- stable stream ID;
- stream name documented;
- host-independent;
- same save + same day + same seed → same candidate selection;
- opening radio UI does not advance stream;
- failed/no-candidate selection does not accidentally consume unrelated streams.

If existing selection authority already has a stream, reuse it unless signed contract says a dedicated fork is required.

## 3.7 Weighting invariants

`SignalTrustAvailability` contract remains:
- bounded;
- monotonic;
- integer-only if currently specified;
- order-preserving.

Add:
- zero/neutral trust parity;
- no zeroing of otherwise eligible candidates unless signed;
- deterministic normalization;
- no floating-point platform drift if contract is integer-only.

## 3.8 Neutral-trust parity

At neutral trust:
- pre-pool and post-pool candidate behavior should match as closely as the signed design requires;
- if exact byte-identical selection is promised, pin it;
- if selection algorithm necessarily changes but distribution remains neutral, the memo must explicitly waive byte parity and define a new deterministic oracle.

Do not claim parity that the signed algorithm cannot mathematically preserve.

## 3.9 Save/load

Preferred design:
- pool derived per selection/day;
- trust ledger already persists;
- no new pool state.

Required tests:
- continuous vs mid-reload same selection sequence;
- trust changes before save produce same post-restore weighted state;
- no duplicate follow-up scheduling;
- selection state and follow-up state remain independent.

## 3.10 Follow-up scheduler non-coupling

One explicit test must prove:
- trust-weighted initial signal selection does not mutate follow-up exactly-once ledger;
- follow-up scheduling remains based on its own canonical triggers;
- selecting or not selecting a primary signal does not fabricate follow-up completion state.

## 3.11 Host wiring

`RadioHostSession` or current selection owner calls the pool.

No RadioPanel rewrite unless signed design includes a visible availability hint.

If UI hint is unspecified:
- do not expose hidden weights;
- behavior remains observable through which signals are selected over time, not a debug probability display.

## 3.12 Replay scenario

Add a new replay scenario:
- fixed seed;
- fixed catalog;
- known trust trajectory;
- continuous run;
- save mid-window and restore;
- compare selected signals/day;
- compare follow-up ledger;
- compare trust ledger.

Then a second case:
- altered trust trajectory;
- same seed;
- availability/selection measurably differs in signed direction.

## 3.13 Option B retirement path

If foreman chooses retirement:
- update contract §5 to DORMANT/RETIRED;
- state why no consumer will be built;
- retain or remove class according to project tombstone policy;
- add/retain a small test if necessary to preserve documented bounds without implying runtime use;
- update acceptance/debt ledger;
- run Radio suite unchanged.

Retirement is a successful unblock because ambiguity is removed.

## 3.14 Verification

Option A:
1. availability unit tests;
2. selection-owner tests;
3. neutral-trust parity;
4. replay continuous/mid-reload;
5. follow-up non-coupling;
6. Radio suite;
7. integrity/utilization;
8. build;
9. verify-fast.

Option B:
1. dormant/tombstone test as applicable;
2. no-consumer search;
3. Radio suite;
4. build;
5. docs/ledger checks.

## 3.15 C2 rollback rules

Stop if:
- current selection owner cannot accept weighting without architectural duplication;
- deterministic stream semantics are unclear;
- pool needs persistent candidate cache without signed save design;
- trust-weighted selection affects follow-up ledger directly;
- neutral parity requirement conflicts with chosen algorithm and memo has not resolved it.

---

# 4. TASK C3 — RADIATION BALANCE FINDINGS F1–F8: CURRENT SWEEP → SIGNED DATA CHANGES

## 4.1 Objective

Re-run the radiation balance evidence at current `HEAD`, classify each historic F1–F8 finding, obtain explicit foreman decisions, and apply only approved data-authored adjustments. The package exists to convert “known bad/proposal only” into signed, measured outcomes.

## 4.2 Premise discipline

The old findings are not automatically current truth because:
- anomaly caps may have landed;
- shielding model changed;
- weather/effects integration changed;
- gear economics/wear may have changed;
- expedition radiation path may have changed.

Therefore the **first implementation act is measurement**, not editing data.

## 4.3 Re-run study harness

Read the full balance study and reproduce:
- seeds;
- scenarios;
- environmental inputs;
- surface rates;
- expedition rates;
- gear/shielding assumptions;
- time horizon;
- health/dose endpoints.

Record any unavoidable harness drift from the original study.

## 4.4 Finding classification

Each F1–F8 receives one:

- **SURVIVES — ORIGINAL PROPOSAL STILL APPROPRIATE**
- **SURVIVES — PROPOSAL NEEDS REVISION**
- **MITIGATED BY LATER WORK**
- **NO LONGER REPRODUCIBLE**
- **WORSE / NEW RELATED FINDING**
- **INVALID HISTORICAL ASSUMPTION**

Do not collapse multiple findings into one generic “radiation too high” statement.

## 4.5 Decision memo table

Columns:
`Finding | Original measurement | Current measurement | Player impact | Current owner/data row | Proposed delta | Predicted result | Shielding interaction | Gear-economy interaction | Recommendation | Foreman verdict`.

Every proposed numeric change needs current measured evidence.

## 4.6 Player-impact translation

For each surviving finding, translate rates into survival-loop terms:
- approximate time to serious dose threshold;
- approximate expedition exposure window;
- effect of baseline shelter/shielding;
- effect of available protective gear;
- whether current rate invalidates intended decision-making by becoming instantly lethal.

This is explanatory evidence, not a new balance model.

## 4.7 Signature gate

No data edit before verdict.

Allowed verdicts:
- APPROVE AS PROPOSED;
- APPROVE REVISED DELTA;
- DECLINE / KEEP CURRENT;
- HOLD pending named dependency.

A decline with reason closes the finding. A hold must include a concrete condition.

## 4.8 Data-authority implementation

Approved edits only:
- existing effects/radiation data;
- existing exposure-rate catalogs;
- existing shielding tables if finding explicitly targets them.

Do not:
- hard-code overrides;
- modify unrelated health thresholds;
- compensate one bad rate by secretly changing another system.

Each finding’s edit should be attributable.

## 4.9 Test-retarget discipline

If old tests pinned old values:
- prove they represent old balance, not invariant correctness;
- change expected value with finding/decision citation;
- preserve invariant tests such as bounds, monotonicity, conservation;
- never loosen assertion precision simply to pass.

## 4.10 Before/after sweep

After each approved batch:
- rerun same study scenarios;
- compare exact before/current/post;
- verify movement direction and magnitude;
- flag overshoot;
- do not stack additional unsanctioned “fine tuning”.

If result misses target materially, return to foreman with evidence rather than iterating hidden balance changes.

## 4.11 Cross-system regression

Required neighborhoods:
- Radiation suite;
- World;
- Expeditions;
- anomaly radiation cap scenarios;
- shielding model;
- protective gear/wear where relevant;
- 30-day playtest radiation axis;
- 360-day soak radiation axis if available and sanctioned.

## 4.12 First-hour audit

If approved changes materially alter early exposure:
- re-run relevant tutorial/teach-vs-demand row;
- confirm player is taught mitigation before lethal demand;
- update first-hour audit only if measured truth changed.

No tutorial redesign unless a real teaching gap is found and separately scoped.

## 4.13 Data integrity and determinism

- edited data validates;
- same seed + same data → same dose ledger;
- no new RNG;
- no save schema change;
- no code-generated hidden multiplier.

## 4.14 C3 closeout statuses per finding

Each F1–F8 must end:
- APPLIED;
- DECLINED;
- RETIRED/MITIGATED;
- HOLD WITH CONDITION.

No “recorded for foreman” remains.

## 4.15 C3 rollback rules

Stop/rollback if:
- current sweep does not reproduce the old premise;
- edit affects a different finding’s target unexpectedly;
- post-fix measurement overshoots the signed band;
- data row ownership is ambiguous;
- test failures indicate an invariant regression rather than intentional balance movement.

---

# 5. TASK D1 — DISTRESS CONTENT TRANCHES: MECHANISM-COMPLETE, CONTENT-EMPTY

## 5.1 Objective

Author the first bounded distress content tranche over the already-complete follow-up and audio-cue mechanisms, prove runtime reachability/utilization, and update the deferred content record. Mechanism code stays unchanged unless a real bug is exposed and routed separately.

## 5.2 Entry proof

Before authoring:
- read follow-up contract;
- read audio contract;
- read tasks-9-12 closeout;
- enumerate all current signal identities;
- measure current follow-up coverage;
- measure current base `audio_cue` coverage;
- measure stage override coverage;
- enumerate valid trigger grammar;
- inventory current audio cue registry;
- run baseline integrity/utilization/audio tests.

Do not trust historical “43” count without remeasurement.

## 5.3 First tranche size

Target approximately 10–12 high-traffic signals.

Selection criteria:
- frequently encountered;
- moral-choice adjacent;
- trap/ignored-consequence opportunities;
- representative of different trigger outcomes;
- existing audio family can support them.

Do not select only easiest rows.

## 5.4 Follow-up chain rules

Each chain:
- 1–2 links for first tranche unless contract explicitly supports/needs more;
- valid trigger;
- valid campaign-day delay;
- self-contained;
- no cross-catalog cycle;
- no new save fields;
- exactly-once scheduling via existing mechanism.

No random trigger conditions.

## 5.5 Trigger grammar

Use only current contract grammar, e.g.:
- answered;
- rescue_success;
- rescue_failed;
- expired;
- trap_fallen_for.

If current contract differs, live contract wins.

Do not invent synonyms.

## 5.6 Audio cue authoring

For uncovered signals:
- prefer existing cue IDs/families;
- base cue first;
- stage override only when tone genuinely changes;
- cue registry references must validate.

Do not create new audio architecture. New cue assets/IDs, if genuinely required, must follow existing registry/content process.

## 5.7 Tone contract

Content should:
- match existing distress fragment voice;
- remain restrained;
- avoid melodrama inflation;
- remain fictional;
- avoid copying existing external prose;
- keep follow-up consequences legible.

Tone review is content QA, not a reason to alter mechanics.

## 5.8 Reachability proof

For every authored follow-up:
- trigger must be producible by runtime;
- source signal must be selectable/detectable under current system;
- delay path must schedule;
- follow-up event must surface;
- exactly-once ledger must prevent repeat.

For every authored cue:
- signal/stage can reach the cue;
- cue ID resolves;
- playback edge remains interception-only where contract specifies.

## 5.9 Determinism

Content introduces:
- no new RNG streams;
- no random delays unless existing mechanism already owns them;
- no new time source.

Follow-up scheduling remains host-day/campaign-day deterministic.

## 5.10 Replay coverage

Add **new** scenario cases for authored content.

Do not mutate historical replay pins whose purpose is to protect prior behavior.

Cases:
- answered → follow-up;
- failed rescue → follow-up;
- expired or ignored → follow-up;
- trap consequence;
- save between initial and delayed follow-up;
- restore → follow-up fires once.

## 5.11 Audio selftest

Extend only as needed to prove:
- cue IDs resolve;
- stage override chosen correctly;
- missing invalid IDs are rejected by integrity before runtime;
- playback edge remains canonical.

## 5.12 Content-utilization

Run utilization and record:
- authored row IDs;
- detected consumer path;
- any unreachable/dead row.

No authored row is accepted as “future content” inside this tranche.

## 5.13 Tranche report

Required columns:
`Signal ID | Trigger | Delay | Follow-up ID/text row | Base cue | Stage override | Runtime reachability | Replay case | Integrity | Utilization`.

Also record:
- existing rows untouched;
- dead rows intentionally not changed;
- new cue IDs vs reused cue IDs.

## 5.14 Second tranche policy

Do not automatically author the remaining signals.

First tranche must close cleanly.

Second tranche becomes:
- next package, or
- Wave 10 candidate,
depending on foreman breadth decision and remaining content need.

## 5.15 D1 verification

1. JSON/schema validation;
2. data-integrity;
3. content-utilization;
4. follow-up focused tests;
5. new replay scenarios;
6. Radio suite;
7. audio selftest;
8. build;
9. snapshots only if visible text layout changes materially;
10. verify-fast.

## 5.16 D1 rollback rules

Reject/revert row if:
- trigger cannot occur;
- cue ID does not resolve;
- chain cycles;
- follow-up duplicates another semantic row without purpose;
- content requires code change to be reachable;
- replay duplicates exactly-once event;
- tone violates established corpus contract.

---

# 6. TASK D2 — F1/F9 GOVERNANCE: REPOSITORY POLICY DECISIONS

## 6.1 Objective

Resolve two governance findings that are mechanical to execute but unsafe to decide implicitly:
- F1 tracked AI-tool workspace directories vs ignore policy;
- F9 canonical skills corpus vs `.qwen/skills/` mirror drift.

No game behavior changes.

## 6.2 Re-verification

Run current repo-hygiene report and record:
- tracked file counts by workspace directory;
- which dirs are currently ignored;
- current dirty working tree state;
- current `.gitignore` modifications;
- skills in `.agents/skills/`;
- skills in `.qwen/skills/`;
- unmapped/mismatched set;
- root agent-rule file set;
- dispatch references.

Historical counts are evidence, not guaranteed current values.

## 6.3 F1 policy options

### Option A — ignore tool-local workspaces
Likely:
- keep canonical shared project assets tracked;
- ignore per-tool caches/session/workspace dirs;
- `git rm --cached` only;
- preserve files on disk.

Memo must list each directory independently.

### Option B — deliberately track selected dirs
Update ignore policy/documentation to reflect actual intent.

### Option C — mixed
Most realistic: canonical `.agents/` stays tracked while per-tool ephemeral dirs are ignored.

The decision must be per directory, not one blanket rule unless foreman explicitly chooses that.

## 6.4 Safety rule: no disk deletion

Execution may use:
- `.gitignore` edits;
- `git rm --cached`.

Execution may not:
- `rm -rf` live tool workspaces;
- overwrite user local settings;
- revert unrelated dirty `.gitignore` edits;
- rewrite history.

Before applying:
- save current diff;
- patch additively;
- inspect staged diff.

## 6.5 F1 verification

- `git ls-files` no longer lists ignored directories;
- files remain on disk;
- hygiene report F1 warning clears;
- tracked-file count before/after;
- clone/repo size metric if tooling already reports it;
- no agent dispatch breaks.

## 6.6 F9 policy options

### Option A — `.agents/skills/` canonical, no `.qwen/skills/` mirror
Remove/untrack mirror according to signed policy and update dispatch/docs.

### Option B — maintained mirror
Keep mirror but add deterministic no-drift validation.

### Option C — generated mirror
Only if project already has a generation/sync pattern and foreman chooses it. Do not invent heavy sync tooling without need.

## 6.7 F9 no-drift gate

If mirror retained:
- compare relative skill names;
- compare canonical content according to documented mirror rules;
- fail on missing/extra/drift;
- exclude known non-semantic platform wrapper differences only if contract explicitly allows them.

Avoid a noisy CI check that flags unrelated generated/platform-specific files.

## 6.8 Agent-rule sync

Hash/compare root rule files according to `ashfall-agents-sync` contract.

Preserve:
- archived pre-foreman snapshots;
- intentional client-specific sections;
- canonical source direction.

Do not normalize all rulebooks blindly.

## 6.9 Dispatch integrity

Check:
- `.claude/` and other dispatch references;
- referenced skills exist;
- post-policy paths resolve;
- canonical skill location matches docs.

Broken dispatch is a release/tooling blocker and must be fixed inside signed governance scope.

## 6.10 User dirty-tree protection

Before `.gitignore` or tracking changes:
- capture `git status --short`;
- capture `.gitignore` diff;
- preserve user lines;
- patch only signed policy lines;
- never reset whole file.

If conflict is ambiguous, stop and route for manual merge rather than discarding user edits.

## 6.11 Governance candidate sweep

After F1/F9:
- run hygiene report;
- list sibling governance warnings with same pattern;
- do not execute them automatically;
- include candidate table for next wave.

## 6.12 D2 verification

1. hygiene report;
2. `git ls-files` checks;
3. on-disk existence checks for untracked workspaces;
4. skill mirror/no-mirror contract test;
5. dispatch resolution;
6. `AgentRuleIntegrityTests`;
7. docs-index check;
8. build;
9. verify-fast.

## 6.13 D2 closeout

Update `POTENTIALCLUTTER.md`:
- F1 verdict/date/evidence;
- F9 verdict/date/evidence.

If foreman defers:
- record named condition;
- do not leave generic “needs decision”.

## 6.14 D2 rollback rules

Stop if:
- signed policy is ambiguous per directory;
- `.gitignore` contains overlapping user changes impossible to patch safely;
- untracking would remove canonical project assets unintentionally;
- skill mirror removal breaks active dispatch and no signed replacement path exists.

---

# 7. TASK D3 — TEST QUARANTINE CONTINUATION: BOUNDED PER-FILE PROMOTION

## 7.1 Objective

Promote a bounded batch of quarantined test files using the proven per-file protocol. Start with the named next candidate (`AutopsyProcedures` if still current), then 2–4 additional candidates. Each file must independently satisfy current production contracts before its `Compile Remove` is removed permanently.

## 7.2 Current-state census

Before changes:
- read `TEST_POLICY.md`;
- read quarantine manifest;
- recount current `Compile Remove` entries;
- identify files already promoted since prior measure;
- identify active claims affecting candidate owners;
- identify source-recovery location for each candidate.

Record:
`File | Manifest reason | Source location | Recoverability | Owner | Active claim? | Candidate priority`.

## 7.3 Candidate classes

### Git-recoverable
Source available in history.

### Twin/archive recoverable
Use archive copy only after SHA verification against manifest or trusted record.

### Dead API
Test targets retired behavior; likely retirement candidate.

### Current bug exposure
Recovered test maps to current contract and fails due to real production regression; route repair.

## 7.4 AutopsyProcedures protocol

If still next candidate:
1. locate manifest entry;
2. recover source;
3. verify provenance/hash if archive;
4. remove only this file’s `Compile Remove`;
5. build;
6. read compile failures;
7. map stale symbols to current canonical owners;
8. rematch test;
9. run file alone;
10. classify remaining failures.

Do not edit production API just to preserve old test shape.

## 7.5 Stale contract rematch

Allowed:
- namespace/type rename to current owner;
- loader/API call update;
- expected catalog count/ID update after current truth verified;
- fixture construction update;
- assertion wording preserving same invariant.

Not allowed:
- reintroducing retired production behavior;
- weakening invariant;
- broad exception swallowing;
- replacing real assertions with `Assert.NotNull`;
- skipping failing cases.

## 7.6 Current-truth test retarget

When catalog/schema changed:
- inspect current production/data;
- prove new count/ID/schema is intentional;
- update test with drift comment;
- cite current contract or completion log.

If current truth appears wrong, stop and route production bug.

## 7.7 Focused run order

For each candidate:
1. build after enabling;
2. file alone;
3. adjacent focused directory;
4. tooling/project tests if csproj changed;
5. only then proceed to next candidate.

Never enable 5 files and discover 80 compile errors with no attribution.

## 7.8 Active-claim rule

If candidate owner/test path is in an active claim:
- defer;
- record claim owner and exact path;
- choose next unclaimed candidate.

No racing Plan 24 or other active work.

## 7.9 Permanent-retirement path

For dead API:
- prove production behavior intentionally retired;
- document one-line retirement reason;
- remove stale test artifact from quarantine accounting according to policy;
- do not count it as “promoted”;
- update manifest count/status.

## 7.10 Production bug path

If recovered test exposes a real regression:
- keep test evidence;
- create repair package;
- do not rewrite test to green;
- decide whether test remains quarantined pending repair according to policy;
- record owner.

## 7.11 Batch sizing

3–5 files total.

Why:
- attributable failures;
- bounded review;
- manageable manifest updates;
- avoids mass stale-contract churn.

Stop batch early if:
- first two candidates reveal systemic API drift;
- tooling breaks;
- active claims block remaining candidates;
- repair findings dominate.

## 7.12 Manifest and debt synchronization

After each file:
- status;
- date;
- evidence;
- focused result;
- disposition.

Update remaining count immediately or at batch close, but ensure manifest and debt ledger agree.

## 7.13 Tooling integrity

Because `Compile Remove` edits change project compilation:
- run project/tooling tests;
- ensure XML/project file remains valid;
- ensure duplicate compile inclusion does not occur;
- build clean.

## 7.14 D3 closeout table

`File | Starting state | Source provenance | Current contract | Changes | File result | Directory result | Final disposition | Remaining count`.

Also name next candidate.

## 7.15 D3 verification

- each promoted file green alone;
- each promoted file green in adjacent suite;
- build;
- tooling tests;
- verify-fast at batch close;
- manifest/debt counts match.

## 7.16 D3 rollback rules

Stop/restore quarantine for candidate if:
- source provenance cannot be trusted;
- owner contract is unclear;
- current behavior is actively being changed by another claim;
- test only validates retired behavior and retirement has not been formally established;
- enabling test destabilizes project due to unrelated unresolved tooling issue.

---

# 8. CROSS-TASK DECISION MEMO STANDARD

All signature-gated Part 2 tasks use the same format.

## 8.1 Header
- decision ID;
- source blocker;
- current `HEAD`;
- current owner;
- current measurement/behavior.

## 8.2 Why a decision is required
State the exact policy ambiguity:
- C1: meaning/blast radius of restock priority;
- C2: build trust-weighted selection vs retire policy;
- C3: approve/decline specific balance deltas;
- D2: tracking/mirror policy.

## 8.3 Options
Each option must state:
- behavior;
- owner;
- persistence;
- determinism;
- UI/content impact;
- compatibility;
- test impact;
- rollback.

## 8.4 Recommendation
Architecture-safe recommendation only. The final product decision belongs to foreman.

## 8.5 Signature
- chosen option;
- signer;
- date;
- conditions.

No implementation dependent on the memo begins before this section is populated.

---

# 9. SAVE / REPLAY / DETERMINISM RULES

## 9.1 C1
Priority derived at legal restock edge. No new save state expected. Save preserves already-restocked pinned stock.

## 9.2 C2
Selection pool derived from persisted trust and day/seed. No pool save state expected. Replay must match continuous vs restored run.

## 9.3 C3
Balance data changes do not alter save schema. Same seed/data reproduces dose ledger.

## 9.4 D1
Content rows use existing scheduler/ledger. No new save fields. Follow-up exactly-once restore remains mandatory.

## 9.5 D2
No game save implications.

## 9.6 D3
Tests only; no production save change unless a promoted test exposes a bug and a separate repair package is created.

---

# 10. DATA / CONTENT POLICY

Applies primarily to C3 and D1.

## 10.1 Data edits
- signed where balance-sensitive;
- smallest attributable rows;
- no schema changes unless separately approved;
- integrity after every batch;
- count pins retargeted only to proven new truth.

## 10.2 Content edits
- live consumer required;
- references must resolve;
- runtime reachability required;
- no filler rows to hit arbitrary counts;
- no duplication for utilization optics.

## 10.3 Before/after evidence
For every data/content batch:
`Row ID | Before | After | Reason | Signed finding/contract | Consumer | Test`.

---

# 11. FOCUSED TEST POLICY

## 11.1 C1
Priority function → restock owner → economy suite → scenario → replay.

## 11.2 C2
Availability policy → selection owner → replay → Radio suite → non-coupling.

## 11.3 C3
Balance harness → radiation owner → affected suites → playtest/soak.

## 11.4 D1
Schema/integrity → follow-up/audio tests → replay cases → Radio/audio selftest.

## 11.5 D2
Repo hygiene → rule integrity → dispatch → build/verify-fast.

## 11.6 D3
Enabled file → adjacent directory → tooling → batch aggregate.

A wider suite is never a substitute for the exact focused gate.

---

# 12. IMPLEMENTATION CONTROL SHEETS

## 12.1 C1 control sheet

### Pre-edit
- [ ] restock method located;
- [ ] day gate proven;
- [ ] pinned-stock behavior proven;
- [ ] same-day no-reroll baseline recorded;
- [ ] scarcity/shock/pressure inputs located;
- [ ] memo signed.

### Implementation
- [ ] pure priority function;
- [ ] deterministic tie-break;
- [ ] no RNG;
- [ ] one evaluation per restock;
- [ ] no save schema;
- [ ] no cadence change.

### Verification
- [ ] deterministic order;
- [ ] shock/scarcity effect;
- [ ] neutral parity;
- [ ] no-reroll;
- [ ] save pinned stock;
- [ ] economy suite;
- [ ] replay;
- [ ] build.

## 12.2 C2 control sheet

### Pre-edit
- [ ] trust contract read;
- [ ] selection reality mapped;
- [ ] no current pool confirmed;
- [ ] follow-up ledger mapped;
- [ ] decision signed.

### Option A
- [ ] pool inside existing selection owner;
- [ ] deterministic stream documented;
- [ ] no new save field;
- [ ] neutral parity defined;
- [ ] replay continuous/restore;
- [ ] follow-up non-coupling.

### Option B
- [ ] dormant/retired contract wording;
- [ ] no runtime consumer;
- [ ] tombstone/retirement test policy followed;
- [ ] ledger closed.

## 12.3 C3 control sheet

### Before signature
- [ ] rerun sweep;
- [ ] classify F1–F8;
- [ ] current measurements;
- [ ] predicted deltas;
- [ ] player-impact translation;
- [ ] shielding/gear interactions;
- [ ] verdict per finding.

### After signature
- [ ] only approved rows edited;
- [ ] tests retargeted with citation;
- [ ] sweep rerun;
- [ ] no overshoot;
- [ ] cross-system suites;
- [ ] balance doc updated.

## 12.4 D1 control sheet

- [ ] live signal count measured;
- [ ] current follow-up coverage measured;
- [ ] current cue coverage measured;
- [ ] trigger grammar locked;
- [ ] 10–12 signal tranche selected;
- [ ] every follow-up reachable;
- [ ] cue IDs resolve;
- [ ] exactly-once restore cases;
- [ ] utilization;
- [ ] tranche report.

## 12.5 D2 control sheet

- [ ] hygiene report current;
- [ ] dirty tree captured;
- [ ] F1 decision signed;
- [ ] F9 decision signed;
- [ ] no disk deletion;
- [ ] `.gitignore` user lines preserved;
- [ ] skill dispatch resolves;
- [ ] rule integrity tests;
- [ ] docs index;
- [ ] health metrics.

## 12.6 D3 control sheet

- [ ] current quarantine count;
- [ ] candidate table;
- [ ] AutopsyProcedures rechecked;
- [ ] source provenance verified;
- [ ] one file enabled at a time;
- [ ] current contract rematch;
- [ ] file-alone green;
- [ ] directory green;
- [ ] manifest/debt updated;
- [ ] next candidate named.

---

# 13. COMMIT BOUNDARIES

## 13.1 C1
1. evidence + signed memo;
2. priority function/tests;
3. restock integration;
4. optional read model;
5. closeout.

## 13.2 C2
1. selection forensics + memo;
2. pool core OR retirement/tombstone;
3. host wiring/replay tests;
4. docs/ledger closeout.

## 13.3 C3
1. current sweep evidence;
2. signed verdict table;
3. data edits in small approved batches;
4. test retargets;
5. post-fix measurement/closeout.

## 13.4 D1
1. coverage census;
2. follow-up tranche;
3. audio cue tranche;
4. replay/integrity/utilization;
5. closeout.

## 13.5 D2
1. hygiene evidence + memos;
2. F1 execution;
3. F9 execution;
4. rule/dispatch validation;
5. closeout.

## 13.6 D3
One candidate file per commit when practical. Batch closeout separate.

---

# 14. ROLLBACK MATRIX

| Task | Failure | Action |
|---|---|---|
| C1 | same-day reopen changes stock | revert priority integration, preserve memo, repair restock semantics |
| C1 | priority requires mutable state | stop for redesign |
| C2 | pool breaks deterministic replay | revert pool, inspect stream contract |
| C2 | pool couples follow-up ledger | revert coupling, restore separation |
| C3 | post-fix rate overshoots signed band | revert that finding’s delta, return evidence |
| C3 | old finding no longer reproduces | retire/mitigate finding, do not edit |
| D1 | authored follow-up unreachable | remove row or fix content trigger within contract |
| D1 | cue requires mechanism code | split bug/feature package |
| D2 | untracking threatens live files | stop; use cached-only semantics or revise policy |
| D2 | skill policy breaks dispatch | revert execution, repair signed path model |
| D3 | test targets retired API | retirement path, not production resurrection |
| D3 | test exposes real bug | route repair, preserve test evidence |

---

# 15. EVIDENCE TABLE TEMPLATES

## 15.1 C1 restock

| Scenario | Scarcity | Shock | Deficit | Expected rank/order | Actual | No-reroll | Result |
|---|---:|---|---:|---|---|---|---|
|  |  |  |  |  |  |  |  |

## 15.2 C2 signal availability

| Signal | Trust state | Base eligibility | Availability weight | Day/seed | Selected? | Continuous | Restored |
|---|---|---|---:|---|---|---|---|
|  |  |  |  |  |  |  |  |

## 15.3 C3 radiation findings

| Finding | Original | Current | Verdict | Approved delta | Post-fix | Target band | Status |
|---|---:|---:|---|---:|---:|---|---|
| F1 |  |  |  |  |  |  |  |
| F2 |  |  |  |  |  |  |  |
| F3 |  |  |  |  |  |  |  |
| F4 |  |  |  |  |  |  |  |
| F5 |  |  |  |  |  |  |  |
| F6 |  |  |  |  |  |  |  |
| F7 |  |  |  |  |  |  |  |
| F8 |  |  |  |  |  |  |  |

## 15.4 D1 distress tranche

| Signal | Trigger | Delay | Follow-up | Cue | Stage override | Reachable | Replay | Utilized |
|---|---|---:|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

## 15.5 D2 governance

| Finding | Option | Signed verdict | Files affected | Safety check | CI check | Final status |
|---|---|---|---|---|---|---|
| F1 |  |  |  |  |  |  |
| F9 |  |  |  |  |  |  |

## 15.6 D3 quarantine

| Test file | Provenance | Old contract | Current contract | Changes | File result | Directory result | Disposition |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

---

# 16. PART 2 MASTER ACCEPTANCE MATRIX

| Task | Primary blocker | Signature | Production/data change | Mandatory close evidence |
|---|---|---:|---:|---|
| C1 | unsigned restock priority semantics | yes | only after signature | policy memo, deterministic order, no-reroll, economy/replay gates |
| C2 | dormant SignalTrust policy + no consumer | yes | pool or retirement | decision, replay parity, Radio suite, follow-up independence |
| C3 | unanswered radiation F1–F8 | yes | approved data rows only | current sweep, verdicts, before/after measurements, cross-system gates |
| D1 | follow-up/audio content empty | no | content only | reachability, replay, integrity, utilization, tranche report |
| D2 | repo governance F1/F9 | yes | repo tracking/sync policy only | hygiene, rule integrity, dispatch, safe untracking |
| D3 | quarantined test debt | no | tests/project only unless separate repair | per-file evidence, focused pass, manifest/debt sync |

---

# 17. WAVE 9 PART 2 FINAL CLOSEOUT

When all six tasks/batches reach terminal states:

1. Re-read the Wave 9 ledger.
2. Confirm C1 has a signed terminal outcome.
3. Confirm same-day restock no-reroll remains green.
4. Confirm C2 no longer says “awaiting future seam”.
5. Confirm SignalTrust selection, if built, is deterministic and save-neutral.
6. Confirm follow-up ledger is independent from availability pool.
7. Confirm every radiation F1–F8 row has a verdict.
8. Confirm no unapproved radiation data change landed.
9. Confirm distress tranche rows are all runtime-consumed.
10. Confirm content mechanisms were not rewritten.
11. Confirm F1/F9 governance findings have signed outcomes or named hold conditions.
12. Confirm no live workspace files were deleted by repo hygiene work.
13. Confirm skill dispatch resolves after F9.
14. Confirm quarantine count is remeasured.
15. Confirm every promoted quarantined file has file-alone and adjacent-suite evidence.
16. Confirm manifest and KNOWN_DEBT agree.
17. Run build.
18. Run verify-fast.
19. Run integrity/utilization after data/content work.
20. Generate `WAVE9_PART2_CLOSEOUT.md` with terminal state per task, remaining blockers, and Wave 10 census input.

---

# 18. WAVE 10 RE-VERIFICATION FILTER

Wave 10 is generated from **surviving current blockers only**.

Candidate sources:
- unsigned/held decisions from C1/C2/C3/D2;
- remaining distress content breadth after D1 first tranche;
- remaining quarantine files after D3 batch;
- Part 1 claim-deferred consumers;
- portfolio HOLD conditions from Wave 8;
- new blockers surfaced by execution.

For each candidate record:
- current blocker;
- current evidence;
- what changed since Wave 9;
- owner;
- active claim;
- required decision;
- dependency state;
- exact package type;
- acceptance gate;
- reason to exclude if already resolved.

If no real blockers survive, convert the series to maintenance cadence rather than inventing Wave 10 tasks.

---

# 19. PART 2 NON-GOALS

- no restock button;
- no random restock priority;
- no new market/economy system;
- no second trust ledger;
- no RadioPanel redesign unless signed;
- no hidden pacing changes beyond C2 decision;
- no unapproved radiation tuning;
- no hardcoded radiation overrides;
- no mechanism rewrite during distress content authoring;
- no new distress save fields;
- no `.agents/` deletion without explicit signed policy;
- no disk deletion of tool workspaces;
- no skill-content rewrite during mirror governance;
- no mass quarantine re-enable;
- no production API resurrection for stale tests;
- no full-suite churn for progress optics.



# APPENDIX A — C1 MICRO EXECUTION CARDS

## C1.01 Current restock truth
Record restock entry point, legal trigger, current ordering, stock pinning, and save behavior.

## C1.02 Same-day reopen oracle
Capture a deterministic fixture proving repeated open/close does not reroll.

## C1.03 Economy input inventory
List scarcity, deficit, shock, trade pressure APIs and their units.

## C1.04 Decision blast-radius comparison
Compare selection vs quantity vs evaluation-order effects.

## C1.05 Pure-function prototype
Test priority calculation without integrating into restock.

## C1.06 Stable tie-break
Use canonical stable item order/ID.

## C1.07 Neutral parity
Prove no-priority/equal-score state preserves legacy result.

## C1.08 Restock-edge integration
Apply ordering exactly once inside legal restock edge.

## C1.09 Save pin
Save after restock, restore, confirm same stock.

## C1.10 Next restock reevaluation
Advance to next legal edge and confirm policy reevaluates.

## C1.11 Shock case
Use signed shock effect to prove rank movement.

## C1.12 Scarcity case
Use signed scarcity input to prove rank movement.

## C1.13 No UI side effects
Read market panel/read model repeatedly; no domain change.

## C1.14 Replay
Restock-heavy window same fingerprint/ledger.

## C1.15 Closeout
Update deferral and Plan 147 truth.

---

# APPENDIX B — C2 MICRO EXECUTION CARDS

## C2.01 Contract freeze
Capture bounds and neutral behavior of `SignalTrustAvailability`.

## C2.02 Selection-map
Document current candidate assembly and selection owner.

## C2.03 Product memo
Pool vs retire.

## C2.04 Candidate eligibility separation
Trust weights only candidates already eligible.

## C2.05 Weight calculation
Integer/bounded/order-preserving.

## C2.06 Stream decision
Reuse or dedicate deterministic stream; document.

## C2.07 Neutral oracle
Define exact neutral selection expectation.

## C2.08 Pool implementation
Inside current selection owner.

## C2.09 Host binding
Use existing radio host seam.

## C2.10 No-panel policy
Do not expose hidden weights unless signed.

## C2.11 Continuous replay
Record selection sequence.

## C2.12 Mid-reload replay
Restore and match suffix.

## C2.13 Trust-shift case
Change trust and prove signed directional effect.

## C2.14 Follow-up isolation
No mutation of follow-up ledger.

## C2.15 Retirement path
If Option B, tombstone policy and remove ambiguity.

## C2.16 Contract update
Mark wired/retired, not “future”.

---

# APPENDIX C — C3 MICRO EXECUTION CARDS

## C3.01 Harness provenance
Record exact balance sim script/data and historical parameters.

## C3.02 Current baseline
Rerun unmodified HEAD.

## C3.03 F1 classification
Current measurement and verdict candidate.

## C3.04 F2 classification
Current measurement and verdict candidate.

## C3.05 F3 classification
Current measurement and verdict candidate.

## C3.06 F4 classification
Current measurement and verdict candidate.

## C3.07 F5 classification
Current measurement and verdict candidate.

## C3.08 F6 classification
Current measurement and verdict candidate.

## C3.09 F7 classification
Current measurement and verdict candidate.

## C3.10 F8 classification
Current measurement and verdict candidate.

## C3.11 Shielding interaction
Recalculate with current shielding authority.

## C3.12 Gear economics interaction
Check protective gear availability/wear implications.

## C3.13 Foreman verdict table
No edit before populated.

## C3.14 Approved data batch
One coherent subset at a time.

## C3.15 Test retargets
Only expected intentional-value pins.

## C3.16 Post-fix sweep
Same harness/settings.

## C3.17 Overshoot check
No hidden second tuning pass.

## C3.18 World/Expedition/Radiation suites
All affected owners green.

## C3.19 Long-window checks
30-day/soak where sanctioned.

## C3.20 Balance-doc closure
Every finding terminal.

---

# APPENDIX D — D1 MICRO EXECUTION CARDS

## D1.01 Signal census
Current total and coverage.

## D1.02 Trigger coverage
Which runtime outcomes produce each grammar trigger.

## D1.03 Cue registry inventory
Reusable cue IDs.

## D1.04 Tranche selection
10–12 high-value signals.

## D1.05 Follow-up row 1..N
Each row validated independently.

## D1.06 Cycle check
No structural/self cycles.

## D1.07 Delay check
Valid campaign day ordering.

## D1.08 Base cues
Fill uncovered signals using existing families.

## D1.09 Stage overrides
Only meaningful tonal transitions.

## D1.10 Tone review
Corpus-matched, restrained.

## D1.11 Reachability tests
Runtime trigger → schedule → fire.

## D1.12 Save between links
Restore and fire once.

## D1.13 Audio resolution
Every cue resolves.

## D1.14 Utilization
Every new row consumed.

## D1.15 Tranche report
All authored IDs and evidence.

---

# APPENDIX E — D2 MICRO EXECUTION CARDS

## D2.01 Hygiene snapshot
Current warnings/counts.

## D2.02 Dirty-tree snapshot
Especially `.gitignore`.

## D2.03 Per-directory F1 classification
Shared/canonical vs ephemeral.

## D2.04 F1 signed policy
No execution before signature.

## D2.05 Safe untracking
`--cached`, never disk deletion.

## D2.06 F1 verification
Tracked paths gone; files remain.

## D2.07 Skills inventory
Canonical and mirror sets.

## D2.08 F9 signed policy
Remove mirror vs maintain mirror.

## D2.09 Mirror gate
If retained, deterministic drift check.

## D2.10 Rulebook sync
Preserve archived snapshots.

## D2.11 Dispatch verification
All skill references resolve.

## D2.12 Integrity tests
Agent rule tests green.

## D2.13 Docs-index
Regenerate/check.

## D2.14 Health metrics
Before/after tracked count and size.

## D2.15 Sibling governance list
Candidates only.

---

# APPENDIX F — D3 MICRO EXECUTION CARDS

## D3.01 Recount quarantine
Current `Compile Remove` total.

## D3.02 Candidate ranking
Prefer named and high-recoverability candidates.

## D3.03 Provenance
Git history first; archive SHA second.

## D3.04 One-file enable
No batching at compile step.

## D3.05 Compile drift
Map each symbol failure to current owner.

## D3.06 Contract rematch
Test follows current API.

## D3.07 File-alone
Must pass.

## D3.08 Adjacent suite
Must pass.

## D3.09 Real regression fork
Create repair package.

## D3.10 Dead API retirement
Document; do not resurrect.

## D3.11 Candidate two
Repeat full cycle.

## D3.12 Candidate three
Repeat full cycle.

## D3.13 Optional candidate four/five
Only if batch remains clean.

## D3.14 Project/tooling check
Compile Remove edits valid.

## D3.15 Manifest sync
Per-file evidence.

## D3.16 Debt count
Update remaining total.

## D3.17 Next candidate
Name next file.

---

# APPENDIX G — REVIEW QUESTIONS

## C1
- What exact behavior does “priority” change?
- Is it signed?
- Is priority deterministic?
- Does same-day reopen remain inert?
- Is stock persisted/pinned exactly as before?
- Did implementation create any new economy owner?

## C2
- Is the current selection owner still unique?
- Does trust only weight eligible candidates?
- Is selection deterministic across restore?
- Is neutral-trust behavior explicitly defined?
- Is follow-up scheduling independent?
- If retired, is contract ambiguity truly removed?

## C3
- Were current measurements rerun before data edits?
- Does each F1–F8 row have a verdict?
- Are only approved rows changed?
- Did post-fix measurements land inside signed expectations?
- Were invariant tests preserved?
- Did any later system already mitigate a historic finding?

## D1
- Is every authored chain reachable?
- Does every cue resolve?
- Does follow-up fire exactly once?
- Did content work avoid mechanism changes?
- Is the first tranche bounded?
- Are replay cases added rather than historical pins mutated?

## D2
- Are local workspace files preserved?
- Are user `.gitignore` changes preserved?
- Is `.agents/skills/` treated according to signed canonical policy?
- Does dispatch still resolve?
- Is mirror drift impossible or explicitly checked?
- Are archived rulebooks untouched?

## D3
- Was each test promoted independently?
- Is source provenance trusted?
- Was current production contract used?
- Were real bugs routed instead of hidden?
- Were dead APIs retired rather than resurrected?
- Do manifest and debt counts agree?

---

# APPENDIX H — HANDOFF TEMPLATES

## H.1 Decision task handoff

**Task:**<br>
**Historical blocker:**<br>
**Current re-verification:**<br>
**Decision memo:**<br>
**Signed verdict:**<br>
**Files/data changed:**<br>
**Authority owner:**<br>
**Persistence impact:**<br>
**Determinism impact:**<br>
**Focused tests:**<br>
**Broader suites:**<br>
**Build:**<br>
**Docs/ledger updated:**<br>
**Terminal state:**<br>
**Remaining dependency:**<br>

## H.2 Content batch handoff

**Tranche:**<br>
**Rows authored:**<br>
**Existing mechanisms used:**<br>
**Reference validation:**<br>
**Reachability:**<br>
**Replay cases:**<br>
**Audio/content selftests:**<br>
**Integrity:**<br>
**Utilization:**<br>
**Build:**<br>
**Remaining breadth:**<br>

## H.3 Quarantine promotion handoff

**File:**<br>
**Manifest reason:**<br>
**Source provenance:**<br>
**Current owner contract:**<br>
**Rematch changes:**<br>
**File-alone result:**<br>
**Directory result:**<br>
**Production bug found?:**<br>
**Final disposition:**<br>
**Remaining quarantine count:**<br>

---

# APPENDIX I — WAVE 10 INPUT RECORD

At Part 2 close, create one row per surviving item:

| Candidate | Current blocker | Why still open | Owner | Decision needed | Claim | Exact next package | Acceptance |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

Exclude:
- signed declined items;
- retired policies;
- verified-resolved stale blockers;
- fully promoted quarantine files;
- content breadth the foreman explicitly does not want.

Wave 10 exists only if rows remain after this filter.



# APPENDIX J — NEGATIVE-CASE REGISTRY

Every row must be tested or marked not-applicable with a contract citation.

## J.1 C1
- [ ] no scarcity/shock → neutral/legacy result.
- [ ] equal scores → stable ID tie-break.
- [ ] same-day reopen.
- [ ] save after restock and restore.
- [ ] next-day/new-arrival legal restock.
- [ ] missing/neutral economy input.
- [ ] market panel read without mutation.
- [ ] replay repeated with same seed.
- [ ] signed option “keep unordered”.

## J.2 C2
- [ ] no eligible signals.
- [ ] one eligible signal.
- [ ] many equal-weight signals.
- [ ] neutral trust.
- [ ] high trust.
- [ ] low trust.
- [ ] save before selection.
- [ ] restore and select.
- [ ] repeated radio panel opens.
- [ ] follow-up pending while new primary selection occurs.
- [ ] retired-policy option.

## J.3 C3
- [ ] current sweep reproduces old finding.
- [ ] current sweep does not reproduce old finding.
- [ ] shielding present.
- [ ] shielding absent.
- [ ] protective gear present.
- [ ] protective gear unavailable.
- [ ] surface exposure.
- [ ] expedition exposure.
- [ ] anomaly additive cap.
- [ ] post-fix target band.
- [ ] signed decline.
- [ ] signed hold.

## J.4 D1
- [ ] answered trigger.
- [ ] rescue success.
- [ ] rescue failure.
- [ ] expiry.
- [ ] trap fallen for.
- [ ] save during delay.
- [ ] restore before follow-up.
- [ ] repeated trigger attempt.
- [ ] missing cue rejected.
- [ ] stage override.
- [ ] no stage override.
- [ ] selected signal never becomes reachable: reject row.

## J.5 D2
- [ ] ignored directory still exists on disk.
- [ ] `git ls-files` no longer tracks signed ignored dirs.
- [ ] canonical tracked dir remains tracked.
- [ ] `.gitignore` unrelated user edits remain.
- [ ] mirror removed and dispatch updated.
- [ ] mirror retained and no-drift check fails on synthetic drift.
- [ ] archived rulebook unchanged.

## J.6 D3
- [ ] recoverable source compiles after rematch.
- [ ] stale count pin retarget.
- [ ] stale API rename.
- [ ] dead API retirement.
- [ ] real production regression.
- [ ] active-claim defer.
- [ ] archive SHA mismatch → reject source.
- [ ] csproj duplicate include protection.
- [ ] batch count/manifest mismatch detection.

---

# APPENDIX K — DISCOVERY WORKSHEETS

## K.1 C1
- restock owner:
- restock trigger:
- pinning persistence:
- economy scarcity API:
- shock API:
- trade-pressure API:
- item stable ordering:
- no-reroll test:
- Plan 147 closeout:
- 210–213 deferral:

## K.2 C2
- trust ledger:
- availability policy:
- current candidate source:
- eligibility owner:
- selection owner:
- RNG stream:
- detection edge:
- follow-up scheduler:
- replay harness:
- RadioHostSession seam:

## K.3 C3
- balance sim script:
- radiation data files:
- shielding data/owner:
- expedition radiation owner:
- anomaly cap:
- protective gear data:
- 30-day harness:
- soak harness:
- F1–F8 source doc:
- balance corpus index:

## K.4 D1
- distress JSON files:
- signal count:
- follow-up contract:
- audio contract:
- trigger grammar:
- cue registry:
- replay tests:
- audio selftest:
- utilization selftest:
- tasks-9-12 closeout:

## K.5 D2
- hygiene script:
- current tracked workspace dirs:
- `.gitignore` diff:
- canonical skills path:
- mirror skills path:
- dispatch files:
- agent rule files:
- sync skill contract:
- integrity test:
- potential clutter doc:

## K.6 D3
- quarantine manifest:
- current Compile Remove count:
- AutopsyProcedures location:
- Twin/archive location:
- SHA record:
- current owner:
- active claim:
- file test command:
- adjacent directory:
- tooling test:
- KNOWN_DEBT row:

# APPENDIX L — TASK-SPECIFIC EXECUTION ORACLES

These oracles define what “correct” means in terms strong enough for an implementation agent to know when to stop. They are not extra features; they turn the source ledger’s blocker statements into falsifiable acceptance.

## L.1 C1 restock-priority oracle

### Domain oracle
Given the same caravan, same legal restock edge, same economy state, and same signed priority policy:
- candidate set is identical;
- computed priority order is identical;
- selected stock/quantities are identical according to the signed blast radius;
- no extra RNG is consumed;
- stock remains pinned after commit.

### Reopen oracle
Given a committed same-day caravan stock:
- open market;
- close market;
- open again;
- inspect stock;
- stock identity and quantities remain unchanged;
- no restock event fires;
- no economy input is re-sampled for the already-pinned stock.

### Next-edge oracle
Advance to next legal restock edge:
- current scarcity/shock/deficit state is read once;
- priority is recomputed once;
- new restock may differ legitimately;
- result is then pinned again.

### Legacy oracle
If signed policy supports neutral/no-priority:
- neutral inputs yield the historical selection behavior;
- exact comparison uses the current repository’s canonical stock snapshot/fingerprint, not a handwritten list.

### Failure oracle
If an economy input is unavailable:
- behavior follows signed fallback;
- no null-driven/random fallback;
- missing read does not silently promote arbitrary item order.

## L.2 C2 SignalTrust pool oracle

### Eligibility oracle
Trust never makes an ineligible signal eligible unless the signed design explicitly changes eligibility semantics. Availability weighting occurs after canonical eligibility.

### Weight oracle
For any two eligible signals where trust state differs only in the direction covered by the contract:
- weight moves monotonically according to `SignalTrustAvailability`;
- bounds are enforced exactly;
- integer-only semantics remain integer-only.

### Selection oracle
With fixed day/seed/catalog/trust:
- candidate ordering/weights are identical;
- selected signal is identical;
- continuous and restored run match.

### Neutral oracle
The signed memo must specify one:
- exact historical selection parity at neutral trust, or
- deterministic neutral distribution/oracle if the algorithm necessarily changes.

Do not leave neutral behavior implicit.

### Independence oracle
Initial selection must not:
- mark follow-up fired;
- alter follow-up scheduled keys;
- advance follow-up delay;
- replay follow-up events.

### Retirement oracle
If policy is retired:
- no runtime consumer remains expected;
- contract says DORMANT/RETIRED;
- ledger no longer calls it blocked on a “future seam”;
- next wave does not re-promote it absent a new signed product decision.

## L.3 C3 radiation-balance oracle

### Measurement oracle
Every finding is judged against a reproduced current scenario, not historical numbers alone.

### Data-change oracle
A signed finding maps to exact authored parameter rows. No compensating hidden code constants.

### Outcome oracle
Post-change sweep:
- movement is in signed direction;
- result lies inside or near signed expected band;
- if outside, task pauses rather than applying unsanctioned second-stage tuning.

### Invariant oracle
Even with changed balance:
- dose accumulation remains deterministic;
- caps remain respected;
- shielding remains monotonic/protective as designed;
- zero exposure does not create dose;
- conservation/accounting invariants remain green.

### Decline oracle
A declined finding is closed only when:
- verdict recorded;
- reason recorded;
- no data changed for that finding;
- balance doc stops presenting it as awaiting decision.

## L.4 D1 distress-content oracle

### Structural oracle
Every authored follow-up:
- uses legal trigger;
- has legal delay;
- does not create a structural cycle;
- resolves all IDs;
- uses existing mechanism.

### Runtime oracle
The initial signal can reach the trigger, scheduler accepts the follow-up, due day arrives, follow-up surfaces once.

### Restore oracle
Save after scheduling but before due day:
- restore;
- advance;
- exactly one follow-up fires;
- no duplicate journal/audio event.

### Audio oracle
Cue references resolve through canonical registry. Stage override, if present, supersedes base cue only on intended stage.

### Tranche oracle
All rows in first tranche are reachable. “Future consumer” is not accepted.

## L.5 D2 governance oracle

### F1 safety oracle
After untracking signed tool-local directories:
- files still exist locally;
- Git no longer tracks them;
- `.gitignore` covers them per signed policy;
- canonical shared directories remain tracked if decision says so.

### Dirty-tree oracle
Unrelated pre-existing `.gitignore` changes remain byte-for-byte/semantically preserved.

### F9 oracle
If mirror removed:
- all dispatch references point to canonical skills;
- no runtime/tooling reference requires removed path.

If mirror retained:
- no-drift check catches a deliberately introduced synthetic mismatch in test/fixture context.

### Archive oracle
Archived rulebook snapshots remain untouched.

## L.6 D3 quarantine-promotion oracle

### Provenance oracle
Recovered source is trusted:
- git historical blob, or
- archive with matching recorded SHA/manifest proof.

### Contract oracle
Every stale symbol/assertion is mapped to current production contract before edit.

### Promotion oracle
A file is promoted only when:
- it compiles in normal project;
- it passes alone;
- adjacent suite passes;
- no active claim conflict exists;
- manifest entry is updated.

### Regression oracle
If current production violates a still-valid assertion:
- classify real bug;
- create repair package;
- do not rewrite assertion.

### Retirement oracle
If API/feature intentionally retired:
- test is retired with reason;
- production behavior is not resurrected.

---

# APPENDIX M — SIGNATURE PACKETS

## M.1 C1 restock signature packet

The packet must contain enough detail for the foreman to sign without reverse-engineering code.

### Current facts
- current restock trigger;
- pinned stock behavior;
- no-reroll invariant;
- current scarcity/shock/pressure inputs;
- current selection algorithm.

### Options table
| Option | Changes selection? | Changes quantity? | Player balance impact | Save impact | Determinism impact | Complexity |
|---|---|---|---|---|---|---|
| A selection priority | yes | no unless current selection implies | medium | none expected | deterministic | bounded |
| B quantity priority | maybe | yes | high | none expected | deterministic | larger |
| C evaluation order only | no | no | low | none | deterministic | smallest |
| D retain current | no | no | none | none | none | none |

### Required signature fields
- chosen option;
- allowed inputs;
- formula/weight bounds;
- neutral behavior;
- tie-break;
- visible read-model requirement yes/no;
- whether balance study is required;
- approval/date.

## M.2 C2 SignalTrust signature packet

### Current facts
- trust ledger persists;
- policy class tested;
- no consumer/selection pool;
- current selection path;
- current deterministic behavior.

### Options
A. Build trust-weighted pool.<br>
B. Retire/dormant policy.

### If A, signer must approve
- selection owner;
- eligible-candidate boundary;
- neutral behavior;
- deterministic stream strategy;
- whether weights are player-visible;
- no new save state;
- acceptable pacing change.

### If B
- tombstone/retirement policy;
- whether class remains for future reference;
- whether tests remain;
- contract wording.

## M.3 C3 radiation signature packet

One page/table per finding:
- finding ID;
- original measurement;
- current measurement;
- current gameplay impact;
- exact proposed row/value change;
- predicted outcome;
- regression exposure;
- approve/revise/decline/hold;
- reason.

No aggregate “approve radiation rebalance” signature. Findings remain independently traceable.

## M.4 D2 governance signature packet

### F1 per-directory table
`Directory | Current tracked count | Purpose | Shared project asset? | Ephemeral/tool-local? | Proposed policy | Foreman verdict`.

### F9 table
`Skill corpus | Canonical? | Consumers/dispatch | Drift risk | Option | Foreman verdict`.

Execution must match per-row verdicts exactly.

---

# APPENDIX N — FAILURE CLASSIFICATION AND ROUTING

## N.1 C1 failures

### Policy mismatch
Signed formula cannot be implemented without changing cadence or state ownership.
**Route:** reopen design memo.

### Restock regression
No-reroll or pinning already broken before priority.
**Route:** separate restock repair; priority waits.

### Determinism regression
Identical state yields different ordering.
**Route:** eliminate unstable ordering/RNG before merge.

## N.2 C2 failures

### Current pool discovered
Historical premise stale.
**Route:** compare against policy; verified-resolve or gap-only task.

### Stream collision
New weighted selection perturbs unrelated radio randomness.
**Route:** fix stream ownership; do not accept replay pin churn.

### Neutral mismatch
Signed exact neutral parity cannot be achieved with proposed algorithm.
**Route:** decision revision, not silent oracle change.

## N.3 C3 failures

### Finding disappeared
Later work mitigated it.
**Route:** mark mitigated with evidence.

### New worse rate
Current sweep shows new regression.
**Route:** add new finding, do not shoehorn under old F-ID without documentation.

### Cross-system break
Approved rate breaks shielding/gear/anomaly invariant.
**Route:** revert affected delta and return evidence.

## N.4 D1 failures

### Invalid cue
Fix authored reference or content registry process; do not add runtime fallback solely for one row unless contract already supports it.

### Unreachable trigger
Remove/re-author row inside existing grammar.

### Exactly-once duplicate
Route mechanism bug separately if existing mechanism fails independently of new content.

## N.5 D2 failures

### Untracking removes required source asset
Policy classification wrong. Revert and re-sign.

### Mirror removal breaks client dispatch
Restore until canonical dispatch migration is designed/signed.

### Hygiene script ambiguity
Fix/report script separately; do not infer policy from incorrect metric.

## N.6 D3 failures

### Compile failure from namespace/API drift
Rematch test.

### Assertion failure from intentional current change
Retarget with evidence.

### Assertion failure from production bug
Route repair.

### Source provenance mismatch
Do not promote; candidate remains quarantined.

---

# APPENDIX O — PERFORMANCE AND COMPLEXITY GUARDS

These tasks are not performance projects, but several changes could accidentally add repeated work.

## O.1 C1
Priority computation occurs at restock edge, not per frame or every panel render. Candidate sorting must be bounded by actual caravan stock candidate set.

## O.2 C2
Pool assembly occurs at canonical selection edge. Do not recompute weighted pools continuously. Avoid allocation-heavy repeated normalization if current radio path has a bounded candidate set; follow repository patterns.

## O.3 C3
Balance changes are data-only. No performance change expected. Any performance regression means implementation escaped scope.

## O.4 D1
Content volume increase should not cause per-frame scanning. Existing mechanism should already handle catalogs; if utilization exposes an O(N²) issue, route performance repair separately.

## O.5 D2
CI no-drift check must be cheap. Avoid scanning entire repository when two skill trees are sufficient.

## O.6 D3
Promoted tests should remain within repository test-policy time budgets. If old test is pathological, refactor test structure without weakening coverage.

---

# APPENDIX P — SAVE AND COMPATIBILITY MATRICES

## P.1 C1

| State | Before feature | After feature | Persisted? | Old-save behavior |
|---|---|---|---|---|
| merchant stock after restock | canonical existing | canonical existing | existing mechanism | unchanged |
| priority score | absent | derived | no | derived at next legal restock |
| policy config | design/data/code per signed choice | current build | not campaign save | n/a |

## P.2 C2

| State | Existing owner | New? | Persisted? | Restore behavior |
|---|---|---|---|---|
| signal trust | trust ledger | no | yes existing | unchanged |
| availability weight | policy | consumer only | no | recomputed |
| candidate pool | selection owner | derived | no | rebuilt deterministically |
| follow-up ledger | follow-up scheduler | no | yes existing | unchanged |

## P.3 C3
No save fields expected. Balance data version is build/content version, not campaign mutation.

## P.4 D1
No new save fields. Existing scheduled-follow-up persistence is exercised by new content.

## P.5 D2/D3
No game save impact.

---

# APPENDIX Q — TEST COMMAND LOG TEMPLATE

For every task, record commands exactly rather than paraphrasing “tests passed.”

## Q.1 Entry
**Command:**<br>
**Purpose:**<br>
**Commit:**<br>
**Result:**<br>
**Cases:**<br>
**Duration:**<br>
**Warnings:**<br>
**Artifact/log:**<br>

## Q.2 Required sequence examples

### C1
- priority test file;
- restock test file;
- economy directory/suite;
- Plan 147 tests;
- replay;
- 30-day if required;
- build;
- verify-fast.

### C2
- availability unit file;
- selection file;
- replay scenario;
- Radio suite;
- integrity/utilization;
- build;
- verify-fast.

### C3
- balance sweep;
- radiation suite;
- world;
- expeditions;
- playtest/soak;
- integrity;
- build;
- verify-fast.

### D1
- integrity;
- utilization;
- follow-up tests;
- replay;
- Radio;
- audio selftest;
- build;
- verify-fast.

### D2
- hygiene;
- `git ls-files` assertions;
- agent rule integrity;
- dispatch check;
- docs index;
- build;
- verify-fast.

### D3
- build after enable;
- file alone;
- adjacent directory;
- tooling;
- batch verify-fast.

---

# APPENDIX R — PART 2 REVIEW CHECKLIST

## R.1 Architectural
- [ ] no duplicate authority;
- [ ] signed decisions precede policy implementation;
- [ ] derived state is not unnecessarily persisted;
- [ ] UI/tooling does not own game policy;
- [ ] existing deterministic streams are respected.

## R.2 Behavioral
- [ ] C1 no-reroll preserved;
- [ ] C2 follow-up isolation preserved;
- [ ] C3 only approved balance deltas applied;
- [ ] D1 content mechanisms unchanged;
- [ ] D2 local files preserved;
- [ ] D3 production APIs not bent to stale tests.

## R.3 Verification
- [ ] focused tests first;
- [ ] exact commands logged;
- [ ] save/replay evidence where applicable;
- [ ] integrity/utilization for data/content;
- [ ] build green;
- [ ] verify-fast green;
- [ ] generated docs/indices checked.

## R.4 Truth
- [ ] every historical deferral receives terminal/current status;
- [ ] no stale “waiting for future seam” after C2 decision;
- [ ] no F1–F8 “recorded for foreman” rows remain unanswered;
- [ ] F1/F9 governance rows show signed outcome;
- [ ] quarantine count matches manifest;
- [ ] Wave 10 candidates are current, not copied.

---

# APPENDIX S — WAVE 9 PART 2 CLOSEOUT DOCUMENT STRUCTURE

Create `WAVE9_PART2_CLOSEOUT.md` with:

## 1. Executive status
One row per C1/C2/C3/D1/D2/D3.

## 2. Decisions
All signatures and verdicts.

## 3. Production/data changes
Grouped by authority.

## 4. Verification table
Exact tests, suites, builds, selftests.

## 5. Save/determinism
Explicit statement per task.

## 6. Content/utilization
Distress and radiation data evidence.

## 7. Governance
Repo hygiene and skill mirror result.

## 8. Quarantine trajectory
Starting count, promoted, retired, routed-repair, remaining.

## 9. Remaining blockers
Only current, proven blockers.

## 10. Wave 10 recommendation
- generate Wave 10 if blockers remain;
- otherwise maintenance cadence.

---

# APPENDIX T — MAINTENANCE-CADENCE EXIT CRITERIA

The blocker-wave series should stop creating numbered waves when:

1. every decision register item is signed/declined/held with condition;
2. no mechanism-complete/content-empty blocker is considered critical;
3. quarantine remainder is either zero or fully dispositioned as intentional long-term debt;
4. no active chain such as C1 has incomplete required waves;
5. all generated architecture/debt ledgers reflect current source;
6. new blockers are handled as ordinary per-release repair/integration packages.

At that point maintain:
- release verification gates;
- standing decision register;
- debt ledger;
- periodic content-utilization/integrity checks;
- targeted gap audits.

The wave counter is never itself a reason to invent additional work.

# APPENDIX U — TERMINAL-STATE DECISION TREES

## U.1 C1 restock priority

1. Does current `HEAD` already contain a restock-priority mechanism?
   - **Yes:** compare it to the historical deferral and signed/current policy.
     - If contract-complete and tests prove no-reroll/determinism → **VERIFIED-RESOLVED**.
     - If partial → implement only delta after policy confirmation.
   - **No:** continue.
2. Is a foreman decision signed?
   - **No:** task remains decision-blocked; memo is deliverable but implementation stops.
   - **Yes:** continue.
3. Did foreman choose current/unordered behavior?
   - **Yes:** **DECIDED-DEFERRED**, update deferral record.
   - **No:** implement signed policy.
4. Do no-reroll, parity, economy, replay gates pass?
   - **Yes:** **IMPLEMENTED**.
   - **No:** revert/reroute failing owner.

## U.2 C2 SignalTrust

1. Does a current selection pool exist?
   - **Yes:** inspect whether it consumes `SignalTrustAvailability`.
     - Fully compliant → **VERIFIED-RESOLVED**.
     - Partial → decision/gap-only work.
   - **No:** continue.
2. Is pool-vs-retire decision signed?
   - **No:** stop.
   - **Retire:** tombstone/update contract → **RETIRED/DECIDED-DEFERRED**.
   - **Build:** continue.
3. Are neutral behavior, stream, save impact, and owner explicit?
   - **No:** memo incomplete.
4. Does continuous/reload replay match and follow-up remain independent?
   - **Yes:** **IMPLEMENTED**.
   - **No:** rollback selection integration.

## U.3 C3 radiation

For each F1–F8:
1. Does current sweep reproduce issue?
   - **No:** mark MITIGATED/NO-LONGER-REPRODUCIBLE with evidence.
   - **Yes:** continue.
2. Is verdict signed?
   - **Decline:** CLOSED-DECLINED.
   - **Hold:** HOLD WITH CONDITION.
   - **Approve:** edit exact data rows.
3. Does post-fix sweep land in signed band?
   - **Yes:** APPLIED.
   - **No:** revert delta and reopen decision.
4. Cross-system invariant regression?
   - **Yes:** revert/reroute.
   - **No:** finding terminal.

C3 task closes only when all F1–F8 have terminal rows.

## U.4 D1 distress tranche

1. Are mechanisms still complete?
   - **No:** route mechanism regression separately.
   - **Yes:** continue.
2. Can selected signals reach valid triggers?
   - **No:** do not author those rows.
3. Do authored rows pass integrity/utilization?
   - **No:** fix/remove affected rows.
4. Do replay and exactly-once restore pass?
   - **Yes:** tranche **BATCH-PARTIAL** or **IMPLEMENTED** for intended scope.
5. Is remaining breadth required now?
   - **No:** record remaining breadth as optional/not blocker.
   - **Yes:** next bounded tranche.

## U.5 D2 governance

For each F1/F9:
1. Does finding still reproduce?
   - **No:** VERIFIED-RESOLVED.
2. Is policy signed?
   - **No:** stop.
3. Is execution safe with dirty tree/current dispatch?
   - **No:** route/manual merge.
4. Does hygiene/rule/dispatch verification pass?
   - **Yes:** IMPLEMENTED.
   - **No:** rollback affected governance change.

## U.6 D3 quarantine

Per file:
1. Does file remain quarantined?
   - **No:** skip, update census.
2. Is source trusted/recoverable?
   - **No:** remain quarantined with reason.
3. Is owner currently claimed?
   - **Yes:** defer candidate.
4. Does current production contract still represent intended behavior?
   - **No/retired:** RETIRE TEST.
   - **Yes:** rematch.
5. Does test fail because production is wrong?
   - **Yes:** ROUTED-REPAIR.
6. Does file + adjacent suite pass?
   - **Yes:** PROMOTED.
7. Update manifest/count before next candidate.

---

# APPENDIX V — IMPLEMENTATION-AUDIT TABLES

## V.1 C1 authority audit

| State/value | Writer | Reader used by priority | New write introduced? | Persisted? | Safe? |
|---|---|---|---|---|---|
| caravan stock |  |  | no |  |  |
| scarcity |  |  | no |  |  |
| shock |  |  | no |  |  |
| trade pressure |  |  | no |  |  |
| priority score | derived | restock owner | n/a | no |  |

The audit fails if priority writes scarcity/shock or creates a persistent score without signed need.

## V.2 C2 authority audit

| State/value | Canonical owner | Pool access | Mutation allowed? | Save owner | Result |
|---|---|---|---|---|---|
| trust |  | read | no | existing |  |
| eligibility |  | read | no | existing/derived |  |
| availability weight | derived | compute | no persistence | none |  |
| selected signal | selection owner | write through owner | yes only there | current semantics |  |
| follow-up ledger | follow-up owner | no mutation | no | existing |  |

## V.3 C3 balance audit

| Parameter | Data owner | Finding(s) | Current value | Approved value | Code override exists? | Test pin |
|---|---|---|---:|---:|---|---|
|  |  |  |  |  |  |  |

No approved value should coexist with a contradictory hard-coded multiplier.

## V.4 D1 content audit

| Row | Schema valid | IDs valid | Trigger reachable | Consumer exists | Save semantics existing | Utilized |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## V.5 D2 repository audit

| Path/corpus | Signed policy | Tracked before | Tracked after | Exists on disk after | Dispatch impact |
|---|---|---:|---:|---|---|
|  |  |  |  |  |  |

## V.6 D3 promotion audit

| File | Compile Remove before | Source trusted | Current API mapped | Production changed? | Promoted/retired/repair | Compile Remove after |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

---

# APPENDIX W — ACCEPTANCE CHECKLISTS

## W.1 C1 final acceptance
- [ ] blocker reverified;
- [ ] signed semantic interpretation;
- [ ] priority is pure/deterministic;
- [ ] stable tie-break;
- [ ] no new RNG;
- [ ] no new save section;
- [ ] no-reroll regression green;
- [ ] pinned stock restore green;
- [ ] neutral/legacy parity green where required;
- [ ] economy suite green;
- [ ] replay green;
- [ ] deferral/closeout updated.

## W.2 C2 final acceptance
- [ ] current selection path mapped;
- [ ] decision signed;
- [ ] one selection authority;
- [ ] trust weights eligible candidates only;
- [ ] weighting bounds/monotonicity green;
- [ ] deterministic stream documented;
- [ ] neutral behavior pinned;
- [ ] no new save fields;
- [ ] continuous/reload selection matches;
- [ ] follow-up ledger unchanged;
- [ ] Radio suite green;
- [ ] contract §5 terminal.

## W.3 C3 final acceptance
- [ ] current sweep rerun;
- [ ] F1–F8 individually classified;
- [ ] verdict per finding;
- [ ] no data edit without approval;
- [ ] approved edits data-authored;
- [ ] intentional test pins retargeted with evidence;
- [ ] post-fix sweep;
- [ ] radiation suite;
- [ ] world suite;
- [ ] expedition suite;
- [ ] long-window check as applicable;
- [ ] integrity;
- [ ] balance docs terminal.

## W.4 D1 final acceptance
- [ ] live signal/cue/follow-up counts measured;
- [ ] first tranche bounded;
- [ ] legal triggers only;
- [ ] chains acyclic;
- [ ] cue IDs valid;
- [ ] reachability proven;
- [ ] save/restore exactly-once;
- [ ] Radio suite;
- [ ] audio selftest;
- [ ] integrity;
- [ ] utilization;
- [ ] tranche report;
- [ ] remaining breadth explicitly classified.

## W.5 D2 final acceptance
- [ ] current hygiene report;
- [ ] current dirty-tree snapshot;
- [ ] F1 signed;
- [ ] F9 signed;
- [ ] no disk deletion;
- [ ] user `.gitignore` edits preserved;
- [ ] signed tracked dirs correct;
- [ ] skill corpus policy enforced;
- [ ] dispatch resolves;
- [ ] rule integrity green;
- [ ] docs index green;
- [ ] findings terminal.

## W.6 D3 final acceptance
- [ ] quarantine recount;
- [ ] candidate batch bounded;
- [ ] each source provenance trusted;
- [ ] each owner claim checked;
- [ ] each file rematched to current contract;
- [ ] file-alone passes or explicit repair/retire;
- [ ] adjacent suite passes for promoted files;
- [ ] tooling/project integrity;
- [ ] manifest updated;
- [ ] debt count updated;
- [ ] next candidate named.

---

# APPENDIX X — FOREMAN REVIEW SUMMARY FORMAT

When presenting all Part 2 decisions together, use this concise executive table:

| Decision | Why needed | Current measured truth | Recommended option | Risk if unchanged | Implementation scope after approval |
|---|---|---|---|---|---|
| C1 restock | semantics unsigned |  |  |  |  |
| C2 SignalTrust | consumer semantics unsigned |  |  |  |  |
| C3 F1 | balance finding |  |  |  |  |
| C3 F2 | balance finding |  |  |  |  |
| C3 F3 | balance finding |  |  |  |  |
| C3 F4 | balance finding |  |  |  |  |
| C3 F5 | balance finding |  |  |  |  |
| C3 F6 | balance finding |  |  |  |  |
| C3 F7 | balance finding |  |  |  |  |
| C3 F8 | balance finding |  |  |  |  |
| D2 F1 | repo tracking policy |  |  |  |  |
| D2 F9 | skills mirror policy |  |  |  |  |

This table is a review surface, not a substitute for the detailed memos.

---

# APPENDIX Y — POST-MERGE OBSERVABILITY

No telemetry system is added by this plan, but existing logs/selftests should make regressions diagnosable.

## Y.1 C1
On legal restock in debug/test context, the implementation may expose test-readable priority inputs/rank through existing diagnostic hooks. Production UI should not show debug score unless signed.

## Y.2 C2
Replay harness is the primary observability tool. Avoid production logging every weight calculation.

## Y.3 C3
Balance study output is the canonical measurement artifact.

## Y.4 D1
Tranche report + utilization are canonical content observability.

## Y.5 D2
Hygiene report + Git tracking state are canonical repo observability.

## Y.6 D3
Manifest + focused test result are canonical quarantine observability.

---

# APPENDIX Z — FINAL “NO FALSE CLOSURE” RULES

A task is **not closed** when:

- C1 memo is signed but code was required and not implemented;
- C1 code exists but same-day no-reroll is untested;
- C2 pool exists but replay differs after restore;
- C2 was “retired” but docs still say awaiting a future consumer;
- C3 data changed without a finding verdict;
- C3 findings remain “for foreman” after review;
- D1 JSON rows exist but utilization cannot find a runtime consumer;
- D1 content relies on unsupported trigger strings;
- D2 `.gitignore` changed but tracked files remain contrary to signed policy;
- D2 mirror policy changed but dispatch is broken;
- D3 `Compile Remove` was removed but test was never run alone;
- D3 manifest and project file disagree;
- any task’s closeout omits a surviving blocker discovered during execution.

The purpose of Wave 9 Part 2 is not to reduce a checklist count. It is to convert ambiguous, stale, or deferred seams into verifiable terminal states while keeping ASHFALL’s authority graph, save behavior, deterministic simulation, content integrity, repository hygiene, and test policy coherent.
