# ASHFALL — GENERATION WAVE 10 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 1

**Document role:** execution-grade Part 1 plan for Wave 10, derived from the re-verified unclaimed-plan corpus ledger.

**Wave 10 premise:** the blocker ledger is not empty. The major newly verified blocker class is not a single subsystem defect but an **unreconciled flagship planning corpus**: roughly 78 `C-integration-plans/` files beyond the executed frontier, with explicit dependency DAGs, mandatory execution orders, overlapping historical scopes, and unknown prerequisite status. This plan therefore begins with truth-reconciliation and claim hygiene before executing any corpus chain head.

**Part 1 task set:**
1. **A1 — Unclaimed Corpus Census:** classify the ~78 plan files, build the DAG, identify real remainders, and rank the executable queue.
2. **A2 — Claim Hygiene:** close the stale ACTIVE economy claim and sweep the claim table for contradictory governance state.
3. **A3 — Recorded Micro-Deferral Sweep:** drain small documented gaps, including the known radio weather-prediction content gap, and route larger/decision-gated items.
4. **B1 — C1[6] “Tests That Mean It” / Plan 27:** execute the first verified chain head only after prerequisite 26A/26B status is resolved.
5. **B2 — C1[7] “One Truth”:** execute only the census-proven remainder after subtracting Waves 3–5 and other already-sealed governance work.

**Primary rule:** Wave 10 does not execute a historical plan merely because the file exists. It executes only the **current remainder** after dependency, overlap, premise, claim, and source verification.

---

# 0. EXECUTION CONTRACT

## 0.1 Allowed terminal states

Every plan/sub-plan/corpus row handled in Part 1 must end in one of these states:

- **SEALED** — the current plan contract is fully satisfied, either by work executed here or by verified earlier work.
- **SEALED-ELSEWHERE** — the current plan scope was completed under another package; close with exact evidence.
- **PARTIALLY-SEALED** — some clauses are complete, and the exact remainder is named and queued.
- **OPEN-UNCLAIMED** — current source does not satisfy the plan and no active package owns it.
- **DEPENDENCY-BLOCKED** — current scope is valid but one or more prerequisite plans/authorities are unsealed.
- **CLAIM-BLOCKED** — valid work exists but required paths are owned by an active claim.
- **SUPERSEDED** — a newer signed/implemented plan replaced the old contract.
- **STALE-PREMISE** — current source contradicts the historical plan premise.
- **DECISION-BLOCKED** — implementation semantics require foreman approval.
- **RETIRED** — historical plan or micro-gap is explicitly no longer required.
- **ROUTED-REPAIR** — investigation finds an actual production regression that belongs in a repair package.

No row ends as “unknown”, “probably done”, “later”, or “seems implemented”.

## 0.2 Part 1 hard rules

1. **Census before corpus execution.**
2. **Claims before edits.**
3. **Dependencies before package promotion.**
4. **Current source beats historical plan assumptions.**
5. **Executed work is never re-executed for documentation optics.**
6. **Every SEALED-ELSEWHERE row needs evidence.**
7. **Every PARTIALLY-SEALED row names its exact remainder.**
8. **Every OPEN-UNCLAIMED row names its prerequisites and likely owner.**
9. **Generated docs/indices are regenerated from inputs; never hand-patched.**
10. **Decision-gated micro-deferrals stop at signature.**
11. **Focused tests first.**
12. **No active claim is bypassed by building an alternate subsystem.**
13. **No vanity coverage target in Plan 27.**
14. **No synthetic production selftest fixtures when the plan requires authority-backed evidence.**
15. **No flaky journey test is retained.**
16. **Governance work changes truth/enforcement, not unrelated game behavior.**
17. **A plan whose premise is dead receives a STALE/SUPERSEDED status, not an implementation attempt.**

## 0.3 Evidence bundle required for every task

Each task handoff must include:

- `HEAD` commit and worktree.
- Relevant claim-state snapshot.
- Exact source plan file(s).
- Current implementation evidence.
- Dependency evidence.
- Status/disposition table.
- Changed files.
- Focused commands/results.
- Broader suite/selftest result where applicable.
- Build result.
- Generator/index checks.
- Remaining blocker(s).
- Next chain-head candidate.

For B1/B2 additionally include:
- implementation log;
- before/after fidelity/governance metrics;
- path claims;
- execution-order evidence.

## 0.4 Abort conditions

Stop and route instead of improvising when:

- dependency status cannot be established;
- an active claim owns the required path;
- the historical plan conflicts with a newer signed authority;
- a corpus plan would duplicate an already-landed subsystem;
- a governance plan would require rearchitecting beyond its named contract;
- a micro-deferral reveals a larger mechanism gap;
- Plan 27’s test target requires production API changes solely to make stale tests pass;
- a new journey is flaky across seeded repeated runs;
- C1[7] remainder cannot be distinguished from already-sealed Waves 3–5 work.

---

# 1. PART 1 ORDERING AND DEPENDENCIES

## 1.1 Hard execution order

**A1 → A2 → A3 → B1 → B2** is the recommended order.

Reasoning:

- **A1** establishes the authoritative corpus map and current chain head.
- **A2** removes stale claim-state blockers so corpus work is not falsely path-blocked.
- **A3** drains small deferred gaps and prevents parent plans from remaining deceptively partial.
- **B1** executes C1[6] only after A1 verifies its prerequisites and chain-head status.
- **B2** executes C1[7] only after A1 proves its true remainder and B1 moves the chain.

## 1.2 Conditional execution

B1 may not execute if:
- Plan 26A/26B is unsealed and mandatory;
- A1 proves another prerequisite is open;
- required paths are actively claimed.

B2 may not execute if:
- B1 is not terminal where C1[7] depends on it;
- A1 shows C1[7] is entirely SEALED-ELSEWHERE;
- remainder is larger than one bounded package and must be promoted separately.

## 1.3 Part 1 does not execute

Unless A1/A3 discover a tiny bounded remainder already within their mandate, Part 1 does **not** execute:
- C1[8]+;
- C2[8]+;
- D1;
- E1;
- C1[10] Goods Must Arrive;
- outside-world, radiation-economics, personal-quests, photography, or seven-day-slice plans.

These become queue outputs, not scope creep.

---

# 2. TASK A1 — UNCLAIMED CORPUS CENSUS

## 2.1 Objective

Convert the approximately 78-plan `C-integration-plans/` corpus from an ambiguous backlog into a source-grounded, dependency-ordered execution queue. Every file must be classified against current source, executed ledgers, closeouts, claims, and Waves 1–9 work.

The deliverable is not “a list of files.” It is an evidence-backed **status map + DAG + ranked chain queue + remainder map**.

## 2.2 Census corpus

Mechanically enumerate:
- `C1_planintegration[6]` through `[45]` where present;
- `C2_planintegration[8]` through `[45]` where present;
- `D1_planintegration*`;
- `E1_planintegration*`.

Do not assume exact numeric continuity; enumerate actual files on disk.

For each file capture:
- filename;
- title;
- source baseline Plan ID(s);
- declared dependencies;
- mandatory execution order;
- named prerequisites;
- named output/acceptance;
- explicitly deferred pieces;
- known plan-family relationships.

## 2.3 Raw extraction schema

Use a structured table:

| Field | Required value |
|---|---|
| Corpus key | `C1[6]`, `C2[20]`, etc. |
| File | exact path |
| Title | exact title |
| Source baseline | Plan ID(s) |
| Dependencies | exact declared plan IDs/files |
| Mandatory order | exact sub-plan order |
| Named owners/systems | source text |
| Acceptance | compact extracted contract |
| Historical premise | what must be true for plan to make sense |
| Current overlap candidates | Waves/closeouts/claims to check |
| Current status | census classification |
| Remainder | exact current gap |
| Evidence | paths/docs/tests |
| Queue rank | after DAG resolution |

Do not summarize away mandatory order.

## 2.4 Build the dependency DAG

### Extraction
Parse every header/dependency section and build directed edges:
`prerequisite -> dependent`.

### Validation
- verify every referenced prerequisite can be resolved to a plan/corpus row or known executed package;
- record unresolved references explicitly;
- detect cycles;
- detect numbering collisions;
- distinguish historical plan ID from corpus index.

### DAG integrity
A chain head is executable only when:
- all hard prerequisites are SEALED/SEALED-ELSEWHERE/SUPERSEDED by compatible current authority;
- no required path is actively claimed;
- no decision blocker remains;
- premise is still valid.

## 2.5 Cross-reference sources

For every corpus row search:

1. `INTEGRATION_PLANS.md`;
2. claim table / `WORKTREE_OWNERSHIP` equivalent;
3. `docs/plans/*_IMPLEMENTATION_LOG.md`;
4. plan closeouts;
5. Wave 1–9 roadmap/closeout files;
6. current source type/API names;
7. current tests/selftests;
8. generated architecture/canon registries if relevant.

No plan is classified solely from one historical doc.

## 2.6 Classification rules

### SEALED-ELSEWHERE
Use when:
- current plan acceptance is fully met;
- evidence points to one or more prior packages;
- no current remainder exists.

Required evidence:
`plan clause -> sealing package -> source/test proof`.

### PARTIALLY-SEALED
Use when:
- some clauses are proven complete;
- some remain current.

Required:
- sealed rows;
- remaining rows;
- dependencies for remainder.

### OPEN-UNCLAIMED
Use when:
- current plan remains valid;
- source lacks required mechanism/evidence;
- no active claim/package owns it.

### SUPERSEDED
Use when:
- a newer plan/authority intentionally replaced contract.

### STALE-PREMISE
Use when:
- historical assumption is contradicted by current architecture.

## 2.7 Known anchor overlaps

Audit these first because they validate the method:

### C1[8] / Plan 31
Cross-reference Wave 9 B1 semantic-kind work.

Classify:
- fully sealed if Wave 9 B1 completes richer contract;
- partially sealed if the full C1[8] plan includes additional navigable briefing/replay diagnostics;
- open only for exact remainder.

### C1[7] “One Truth”
Cross-reference:
- Wave 3 D3 agent sync;
- Wave 4 B1 docs atlas;
- Wave 5 A2 skill-doc truth;
- Wave 10 A1 census itself.

### C2[8] ship gate
Cross-reference Waves 2 D4 and 4 D2/D3.

### C2[9] orchestration/decomposition
Cross-reference Waves 2 D1 and 7 A1–A3.

For each produce clause-level delta, not a binary guess.

## 2.8 Prerequisite unknowns

Resolve at minimum:
- Plan 26A/26B status for C1[6];
- Plan 23 power/heat for C1[10];
- Plan 36 producer-port gate;
- any dependency chain referenced by C2[8]/C2[9].

Unknown prerequisite means dependent plan is DEPENDENCY-BLOCKED, not OPEN.

## 2.9 Numbering collision audit

Because historical corpora may reuse plan numbers:
- build `corpus key -> source baseline -> historical family`;
- compare against 170–199, 210–213, and other known renumbered sets;
- do not merge different plans merely because numeric ID matches;
- use file path/title + source baseline as identity.

Document collisions.

## 2.10 Current-source premise check

For each plan:
- search named systems/classes/data;
- verify “missing” claims;
- verify “single owner” assumptions;
- verify save/UI/test architecture assumptions.

If current source contradicts premise:
- classify STALE-PREMISE;
- add a status banner to source plan doc only after evidence;
- do not rewrite the body historically.

## 2.11 Remainder extraction

For PARTIALLY-SEALED rows create:

`Plan clause | historical requirement | current implementation | sealing evidence | remaining delta | dependency | owner`.

This remainder table is the real future work product.

## 2.12 Queue ranking

Rank OPEN-UNCLAIMED/PARTIALLY-SEALED remainder plans using:

1. DAG readiness;
2. number of downstream dependents unblocked;
3. authority clarity;
4. active claim availability;
5. decision readiness;
6. package boundedness.

Do not rank by novelty or file number alone.

## 2.13 Chain-head selection

The next executable package must:
- have all dependencies sealed;
- have current valid premise;
- have no active conflicting claim;
- be bounded enough to claim;
- have clear acceptance.

A1 proposes chain head to foreman/integrator; it does not execute it.

## 2.14 Census document

Create:
`docs/plans/UNCLAIMED_CORPUS_CENSUS.md`

Sections:
1. corpus inventory;
2. methodology;
3. classification summary;
4. dependency DAG;
5. known overlap audits;
6. prerequisite unknown resolution;
7. numbering/collision notes;
8. per-plan status table;
9. per-plan remainder table;
10. ranked queue;
11. proposed next chain head;
12. stale/superseded premise list.

## 2.15 STALE banners

For plans proven stale:
- add concise header banner;
- state current disposition;
- cite sealing/superseding evidence;
- preserve historical plan body.

No large rewrite.

## 2.16 Zero-production-change guarantee

A1 is documentation/forensics only.

Allowed:
- census doc;
- plan status banners;
- index regeneration.

Not allowed:
- Core/UI/data/test behavior changes.

## 2.17 Verification

- every enumerated corpus file appears once;
- dependency references resolve or are explicitly unresolved;
- no hidden duplicate identity;
- status evidence present;
- DAG acyclic or cycles documented as blockers;
- docs index `--check`;
- zero production diff.

## 2.18 A1 handoff

Handoff tables:
- classification counts;
- open queue;
- dependency blockers;
- stale premise list;
- overlaps sealed elsewhere;
- next chain head.

## 2.19 A1 non-goals

- no corpus execution;
- no merging corpora;
- no speculative status;
- no broad plan rewriting;
- no dependency waiver.

---

# 3. TASK A2 — CLAIM HYGIENE: CLOSE STALE ACTIVE CLAIMS

## 3.1 Objective

Make the ownership/claim table truthful so completed work no longer blocks unrelated agents. Close the verified stale Plan 14 economy claim and sweep for sibling stale ACTIVE/HANDED_OFF states.

## 3.2 Known blocker

The economy-core claim is marked ACTIVE while completion docs record:
- 14A complete;
- 14B complete;
- composition complete;
- gates green.

This contradiction is a governance blocker because agents must respect claim state.

## 3.3 Premise proof

Read:
- full claim row;
- exact paths claimed;
- completion docs;
- plan checkpoints;
- AGY handoff;
- current source;
- cited focused tests.

Do not change status from documentation alone if current code is broken.

## 3.4 Focused closure verification

Run current equivalents of:
- economy suite;
- trade embargo tests;
- regional price atlas tests;
- data integrity;
- any claim-specific acceptance commands.

Record current counts; historical counts are reference only.

If claim package currently fails acceptance:
- do not close;
- classify acceptance debt;
- route repair/foreman review.

## 3.5 Status transition

Use table’s existing convention:
- ACTIVE -> HANDED_OFF;
- or ACTIVE -> DONE;
- whichever repository policy defines.

Add:
- date;
- completion evidence;
- closeout pointer.

One-cell/row-level governance edit only.

## 3.6 Full claim-table sweep

For every ACTIVE row:
- locate linked plan/package;
- locate closeout;
- verify acceptance;
- compare claimed paths with current work.

Classify:
- LEGITIMATELY ACTIVE;
- STALE ACTIVE — COMPLETE;
- STALE ACTIVE — SUPERSEDED;
- ACTIVE BUT ACCEPTANCE-DEBT;
- UNKNOWN — FOREMAN REVIEW.

## 3.7 HANDED_OFF audit

For every HANDED_OFF row:
- check whether acceptance exists;
- if accepted per policy, normalize to final status if convention requires;
- if no acceptance, flag acceptance debt;
- do not grant acceptance yourself unless governance rules explicitly authorize integrator.

## 3.8 Overlap invariant

Check active rows for overlapping path claims.

If overlap:
- identify exact paths;
- identify claim creation dates/owners;
- flag to foreman;
- do not silently close either unless completion evidence proves one stale.

## 3.9 Expected remaining active state

The source ledger expects Plan 24 to remain legitimately active while 24B/24C are partial.

Re-verify. If Plan 24 closed since:
- update based on evidence;
- do not preserve expected state merely because Wave 10 source said so.

## 3.10 Ledger synchronization

Update `INTEGRATION_PLANS.md` checkpoints when claim status changes plan truth.

Claim table and integration ledger must not disagree.

## 3.11 Governance guard

No production path changes.

Allowed:
- claim status;
- evidence pointers;
- checkpoint truth.

## 3.12 Verification

- newly closed claims’ focused gates green;
- no stale ACTIVE with complete acceptance;
- no silent ACTIVE overlap;
- build;
- verify-fast;
- claim table syntax/format intact.

## 3.13 A2 handoff

Before/after table:
`Claim | Before | Evidence | Verification | After | Remaining note`.

Also list:
- acceptance-debt rows;
- overlapping active rows;
- genuinely active claims.

## 3.14 A2 non-goals

- no acceptance fabrication;
- no table schema redesign;
- no production edits;
- no path reassignment without foreman.

---

# 4. TASK A3 — RECORDED MICRO-DEFERRAL SWEEP

## 4.1 Objective

Build and drain a bounded ledger of small, explicitly documented deferred items scattered across completed claims/closeouts. Close content-authorable items, strike already-resolved items, route decision-needed items, and promote larger code gaps to the A1 queue.

## 4.2 Discovery sources

Search:
- full claim table;
- closeouts from recent waves;
- `KNOWN_DEBT.md`;
- completion docs;
- handoffs.

Candidate markers:
- deferred;
- not-yet-authored;
- remaining;
- flagged;
- quirk;
- later;
- follow-up;
- TODO only where tied to a recorded plan.

Do not convert arbitrary TODOs into this sweep.

## 4.3 Inventory schema

`Item | Parent package | Recorded text | Type | Current evidence | Required owner | Disposition | Verification`.

Types:
- CONTENT-AUTHORABLE;
- DECISION-NEEDED;
- CODE-GAP-SMALL;
- CODE-GAP-LARGE;
- ALREADY-RESOLVED;
- STALE/RETIRED.

## 4.4 Known item: radio weather predictions

Re-verify:
- forecast mechanism exists;
- radio-side content remains absent/partial;
- owning catalog;
- required reliability/tone constraints;
- radio consumer exists.

If already authored:
- classify already-resolved;
- cite rows/consumer/tests.

## 4.5 Weather prediction content authoring

Only if still missing.

Extract from current weather/radio contract:
- forecast states;
- reliability semantics;
- radio message shape;
- IDs;
- localization/text structure;
- selection rules.

Author only rows needed to satisfy recorded gap.

No new forecast mechanics.

## 4.6 Content rules

- grounded in existing forecast reliability model;
- fictional in-world voice;
- no deterministic certainty if forecast is probabilistic;
- no fake technical claims unsupported by current model;
- no duplicate semantic rows;
- valid references.

## 4.7 Content verification

Run:
- data integrity;
- content utilization;
- Radio suite;
- World/Weather suite;
- relevant radio selftest;
- snapshot if rendered layout changes.

Every row must be consumed.

## 4.8 Decision-needed items

For each:
- create decision-register row;
- record exact options only if existing plan already defines them;
- otherwise record the question/owner;
- do not execute.

## 4.9 Small code gaps

A small code gap is eligible only when:
- one clear owner;
- no new state authority;
- bounded wiring;
- no product decision;
- focused tests obvious.

Example size: missing existing-event subscription/read call.

If larger:
- promote to A1 census queue;
- do not “just wire it” opportunistically.

## 4.10 Already-resolved items

Strike stale deferral text with:
- sealing package;
- date;
- source/test evidence.

Do not delete historical record entirely if repository preserves audit history; mark resolved.

## 4.11 Sweep report

Create:
`docs/plans/WAVE10_MICRO_DEFERRAL_SWEEP.md`

Sections:
- sources searched;
- inventory;
- dispositions;
- authored rows;
- decisions routed;
- code gaps promoted;
- stale notes corrected.

## 4.12 Parent-plan truth

A parent plan may move from partial to fully complete only if all its recorded deferrals are terminal.

Do not use A3 to falsely close a parent that still has a large queue item.

## 4.13 Verification

- every discovered recorded deferral has classification;
- authored content validates and is consumed;
- small wiring has focused tests;
- decision items untouched;
- promoted items appear in queue;
- build;
- verify-fast.

## 4.14 A3 non-goals

- no unsigned decisions;
- no invented content beyond recorded gap;
- no broad cleanup;
- no large mechanism implementation.

---

# 5. TASK B1 — C1[6] “TESTS THAT MEAN IT” / PLAN 27

## 5.1 Objective

Execute C1[6] only after prerequisite status is resolved and after subtracting fidelity work already landed in Waves 1–9. Follow mandatory order **27A → 27B → 27C**. The goal is not more tests; it is stronger authority-backed evidence, behavior assertions, runtime fidelity, and real campaign journeys.

## 5.2 Entry gate: Plan 26A/26B

Before claiming B1:
- locate Plan 26A/26B docs;
- determine status from A1 census;
- identify required artifact/bootstrap fidelity;
- verify current source.

Possible outcomes:
- SEALED -> proceed;
- SEALED-ELSEWHERE -> cite/proceed;
- PARTIAL -> execute/queue prerequisite first as DAG demands;
- OPEN -> B1 DEPENDENCY-BLOCKED.

Do not waive prerequisite because tests currently pass.

## 5.3 Read C1[6] fully

Extract exact 27A/27B/27C:
- scope;
- prohibited shortcuts;
- named failure classes;
- mandatory artifacts;
- acceptance;
- verification commands;
- dependency assumptions.

Build clause table before implementation.

## 5.4 Re-measure current baseline

Historical plan metrics are stale by design.

Measure:
- current test file count;
- current test case count;
- quarantined exclusions;
- journey/replay files;
- save/restore coverage;
- constructor-only tests;
- “does not throw”-only tests;
- synthetic-fixture production selftests;
- known flaky tests.

Do not use gross coverage percentage as primary quality metric.

## 5.5 Subtract already-sealed fidelity work

Cross-reference Waves 1–9:
- replay harnesses;
- save journey parity;
- scenario batteries;
- authority-backed fixtures;
- removed fake consoles;
- ending input fixes;
- callback fixes;
- quarantine promotions.

For each Plan 27 clause:
`Historical gap | sealed package | current evidence | remainder`.

## 5.6 27A — artifact/bootstrap fidelity

Follow actual C1[6] contract.

Typical verification questions:
- do tests construct production-like composition roots where required?
- are artifact/bootstrap paths real rather than mocks?
- do exported/runtime bootstrap assumptions appear in tests?
- are fixtures using canonical loaders/registries?

Do not generalize beyond exact plan.

## 5.7 27A implementation discipline

For each target:
- prove low-fidelity current pattern;
- identify production authority/bootstrap seam;
- replace synthetic/inert setup with authority-backed fixture;
- keep test deterministic and fast;
- run alone;
- run adjacent.

Production code should not change unless test exposes a real bug, which becomes repair package.

## 5.8 27B — fidelity inventory

Script-assisted inventory for candidate anti-patterns:
- test only constructs object;
- only asserts no exception;
- only asserts non-null;
- callback supplied but never asserts invocation/result;
- fake console/adapter not matching production contract;
- save test writes but never restores;
- event test registers but never asserts event payload;
- UI route test opens but never asserts bound state.

The script produces candidates, not automatic judgments.

## 5.9 Candidate ranking

Rank by:
1. critical subsystem;
2. history of continuity failures;
3. current absence of behavioral assertion;
4. downstream dependency;
5. ease of authority-backed fixture.

Choose bounded first batch.

## 5.10 Behavior assertion upgrade pattern

Constructor-only:
- setup through current owner;
- perform meaningful command/tick;
- assert authoritative state change.

Does-not-throw:
- assert exact typed result;
- assert state/event/no-mutation on failure.

Callback:
- assert callback payload/count/order.

Save:
- capture -> restore -> compare.

Event:
- trigger real authority edge -> assert event once with payload.

## 5.11 No production-fixture contamination

Plan guardrail forbids implicit demo fixture inside production selftest.

Audit:
- synthetic hardcoded data in production selftests;
- hidden fixture injection;
- fake catalogs that differ from runtime.

Replace with:
- canonical catalog loader;
- authority-backed minimal data;
- explicit test-only fixture outside production runtime where policy allows.

## 5.12 27C — real campaign journey definition

Extract exact journeys from plan.

Each journey should:
- traverse multiple real owners;
- use real composition;
- assert state transitions;
- include save/restore where contract requires;
- have deterministic seed;
- have clear terminal oracle.

Do not create giant brittle end-to-end scripts asserting every incidental log line.

## 5.13 Journey structure

Recommended:
1. deterministic campaign seed;
2. authority-backed bootstrap;
3. initial invariant snapshot;
4. player/system actions;
5. intermediate state assertions;
6. optional save/restore midpoint;
7. terminal action;
8. authoritative terminal-state comparison;
9. replay fingerprint.

## 5.14 Flaky journey ban

Run each new journey N times with same seed.

Requirements:
- identical pass/fail;
- identical authoritative state;
- identical fingerprints where expected;
- no timing races.

If flaky:
- fix root cause;
- or do not merge.

No retry wrappers.

## 5.15 Golden/snapshot guard

If Plan 27 touches golden artifacts:
- no unreviewed refresh;
- diff first;
- classify intentional vs regression;
- rebaseline only intentional changes with evidence.

## 5.16 Fidelity metric

Use Plan 27’s defined runtime-evidence/fidelity metric.

If plan metric is stale/unavailable:
- do not invent vanity percentage;
- report concrete upgraded targets and journey evidence;
- route metric ambiguity if necessary.

## 5.17 Comprehensive save evidence

Run current comprehensive save battery because Plan 27 targets continuity/fidelity.

If battery has standing known failures:
- use current reconciled baseline;
- do not hide new failure as pre-existing.

## 5.18 Claims

Claim exact:
- test files;
- fixture helpers;
- generated artifacts if needed;
- no Plan 24 active paths.

If a highest-priority target is claimed:
- choose another target;
- record deferral.

## 5.19 Implementation log

Create C1[6] log using C1[5] pattern.

Include:
- premise;
- prerequisites;
- clause matrix;
- sealed-elsewhere deductions;
- files;
- tests;
- N-run journeys;
- save battery;
- fidelity metric;
- remaining gaps;
- terminal status.

## 5.20 Census update

After B1:
- C1[6] -> SEALED / PARTIALLY-SEALED with exact remainder;
- DAG readiness recomputed;
- next chain head identified.

Do not automatically assume C1[7] if census says another hard dependency intervenes.

## 5.21 Integration ledger

Add/update package row:
- premise;
- exact claims;
- acceptance;
- focused verify;
- status.

## 5.22 B1 verification

Order:
1. each changed test alone;
2. adjacent target;
3. new journey alone;
4. journey N-repeat;
5. touched subsystem suites;
6. save battery;
7. build;
8. verify-fast.

## 5.23 B1 non-goals

- no global coverage target;
- no test-count target;
- no production API bending;
- no flaky retries;
- no synthetic production fixtures;
- no bulk golden refresh.

---

# 6. TASK B2 — C1[7] “ONE TRUTH”: CENSUS-DRIVEN REMAINDER

## 6.1 Objective

Reconcile C1[7] clause-by-clause against current governance/documentation infrastructure, credit work already sealed by Waves 3–5 and Wave 10 A1, then execute only the verified remainder.

## 6.2 Entry gate

Requires:
- A1 census row for C1[7];
- B1/C1[6] terminal if DAG requires;
- current claim availability;
- full C1[7] read.

If A1 says C1[7] SEALED-ELSEWHERE:
- write implementation log citing evidence;
- no production/governance work.

## 6.3 Known overlap families

Cross-reference at minimum:
- agents/rulebook sync -> Wave 3 D3;
- docs atlas/index -> Wave 4 B1;
- skill-doc truth -> Wave 5 A2;
- corpus census -> Wave 10 A1;
- claim truth/governance -> Wave 10 A2 where applicable.

Do not re-run projects solely to claim C1[7] progress.

## 6.4 Clause matrix

`C1[7] clause | Intended truth owner | Prior sealing package | Current evidence | Remainder | Action`.

Only remainder becomes B2 implementation.

## 6.5 Typical remainder: canon registry truthfulness

If in C1[7]:
- locate implemented-canon/capability registry;
- enumerate rows;
- verify each against current source;
- find overstated/understated rows;
- correct data/docs source;
- add/extend cheap drift gate if plan requires enforcement.

No hand-wavy “registry looks correct”.

## 6.6 Canon verification rules

For every capability row:
- source owner exists;
- status matches actual implementation;
- player/runtime consumer state correct;
- closeout reference current;
- no stale “planned” for shipped feature;
- no “implemented” for docs-only stub.

## 6.7 Typical remainder: roadmap single-truth enforcement

If plan requires one authoritative roadmap/ledger:
- identify current canonical ledger;
- identify mirrors/derived docs;
- verify contributors know generation/update process;
- add `--check`-style gate only if named by plan and currently missing.

Do not create another ledger.

## 6.8 Governance gate design

A cheap CI/document check should:
- verify generated/derived docs match source;
- detect duplicate/current-status drift;
- avoid semantic guesses too complex for static check;
- produce actionable failure.

Avoid brittle grep over prose when structured source exists.

## 6.9 Rulebook/canon preservation

Historical snapshots:
- remain immutable if debt policy says so.

Current rulebooks:
- sync only under canonical process;
- client-specific intentional differences preserved.

Do not flatten all tooling docs.

## 6.10 Remainder sizing

B2 executes only remainder small enough for one package.

If census shows a large unresolved governance subsystem:
- promote separately;
- B2 closes as PARTIALLY-SEALED with queue row.

## 6.11 Implementation log

Create C1[7] implementation log:
- full clause list;
- sealed-elsewhere evidence;
- current remainder;
- work executed here;
- gates;
- zero-op clauses;
- remaining queue.

The log is the authoritative reconciliation record.

## 6.12 Census update

Update:
- C1[7] status;
- dependencies released;
- next chain head.

## 6.13 Verification

Depending on remainder:
- canon registry validator;
- docs index;
- agent rule integrity;
- relevant generator `--check`;
- build;
- verify-fast.

No broad runtime suites unless remainder touches runtime.

## 6.14 B2 non-goals

- no reexecution of Waves 3–5;
- no new governance architecture beyond plan;
- no duplicate ledger;
- no mass rulebook rewrite;
- no production feature changes.

---

# 7. CROSS-TASK CORPUS STATUS MODEL

## 7.1 Status precedence

When multiple labels seem applicable:

1. STALE-PREMISE / SUPERSEDED if historical contract no longer applies.
2. SEALED / SEALED-ELSEWHERE if fully satisfied.
3. PARTIALLY-SEALED if valid remainder exists.
4. DEPENDENCY-BLOCKED / CLAIM-BLOCKED / DECISION-BLOCKED if work valid but blocked.
5. OPEN-UNCLAIMED if ready but unowned.

Do not call a stale plan “blocked”.

## 7.2 Evidence minimum

SEALED requires:
- implementation/source;
- test/acceptance;
- closeout or equivalent current proof.

A class/type existing is not sufficient.

## 7.3 Remainder discipline

A remainder must be executable language:
- “wire X authoritative event into Y read model and add totality gate”
not:
- “finish feature”.

## 7.4 Queue discipline

Only queue:
- valid current remainder;
- dependencies known;
- owner clear;
- package bounded.

---

# 8. CLAIM HYGIENE STANDARD

## 8.1 Claim row required fields

Every ACTIVE claim should expose:
- package ID;
- owner;
- paths;
- start/date;
- status;
- completion/handoff pointer if terminal.

## 8.2 Closure evidence

A claim closes when:
- package acceptance green;
- handoff recorded;
- no in-flight edits remain requiring exclusive ownership.

## 8.3 Stale claim detection

Candidate stale if:
- linked closeout says complete;
- ledger says DONE;
- no recent in-flight handoff;
- tests green.

Still verify.

## 8.4 Overlap detection

Normalize claimed paths and compare.

Flag:
- exact overlap;
- parent/child overlap;
- generated source/input overlap where concurrent edits race.

---

# 9. MICRO-DEFERRAL POLICY

## 9.1 Eligible micro-gap

- documented;
- bounded;
- parent known;
- owner clear.

## 9.2 Not eligible

- new feature;
- vague TODO;
- unresolved product design;
- multi-subsystem architecture;
- dependency chain requiring new plan.

## 9.3 Terminal dispositions

- EXECUTED;
- RESOLVED-ELSEWHERE;
- DECISION-ROUTED;
- QUEUE-PROMOTED;
- RETIRED.

---

# 10. PLAN 27 FIDELITY POLICY

## 10.1 Evidence beats count

A test is meaningful when it proves:
- state transition;
- event semantics;
- persistence parity;
- deterministic journey;
- authority integration;
- player-visible contract.

Not merely:
- constructor works;
- no throw;
- non-null.

## 10.2 Current owner contract

Every upgraded test reads current production contract first.

No historical API resurrection.

## 10.3 Repeated seeded journeys

Use same seed and inputs N times.

No statistical “usually passes”.

## 10.4 Selftest integrity

Production selftests should exercise production data/authorities, not hidden demo worlds unless the plan explicitly defines a demo fixture as production behavior.

---

# 11. C1[7] ONE-TRUTH POLICY

## 11.1 One canonical truth per domain

- claim status -> claim table;
- integration status -> integration ledger;
- corpus status -> census;
- generated docs -> source/generator;
- skill rules -> canonical rulebook/skills corpus per governance.

Avoid redundant mutable copies.

## 11.2 Derived documentation

Derived docs:
- generated/checkable;
- not manually authoritative.

## 11.3 Historical docs

Historical plans/logs preserved with status banners rather than rewritten into present tense.

---

# 12. PART 1 ACCEPTANCE MATRIX

| Task | Blocker | Mandatory result | Verification |
|---|---|---|---|
| A1 | unreconciled ~78-plan corpus | every plan classified, DAG, queue, remainders | census completeness, docs index, zero production changes |
| A2 | stale ACTIVE claim(s) | claim table matches actual active work | focused acceptance, overlap sweep, build, verify-fast |
| A3 | recorded micro-deferrals | every discovered item dispositioned; known radio content closed if live | integrity/utilization, owning suites, sweep report |
| B1 | C1[6] Plan 27 chain head | prerequisite resolved; 27A→27B→27C current remainder executed | focused tests, N-run journeys, save battery, log |
| B2 | C1[7] partial overlap | sealed-elsewhere clauses credited; current remainder executed | governance checks, docs/generator gates, log |

---

# 13. COMMIT BOUNDARIES

## A1
1. corpus inventory/parser output;
2. dependency/status analysis;
3. census doc;
4. stale banners;
5. docs index.

## A2
1. completion evidence;
2. economy claim closure;
3. sibling stale-claim corrections;
4. ledger sync.

## A3
1. micro-gap inventory;
2. known radio prediction content;
3. small code-gap commits individually;
4. sweep report/deferral updates.

## B1
1. prerequisite evidence;
2. 27A bounded changes;
3. 27B each fidelity target one commit where practical;
4. 27C journeys;
5. implementation log/census/ledger.

## B2
1. clause reconciliation;
2. canon truth fixes;
3. governance enforcement gate if required;
4. log/census.

---

# 14. ROLLBACK / ROUTING MATRIX

| Finding | Action |
|---|---|
| A1 dependency reference unresolved | mark DEPENDENCY-BLOCKED; do not promote |
| A1 plan already fully sealed | SEALED-ELSEWHERE |
| A2 completed claim tests fail | acceptance debt; do not close claim |
| A2 ACTIVE overlap | foreman conflict resolution |
| A3 gap larger than bounded wiring | promote to census queue |
| A3 content row has no consumer | do not author/accept |
| B1 26A/26B unsealed | execute prerequisite first |
| B1 upgraded test exposes bug | repair package |
| B1 journey flaky | fix or remove; never retain flaky |
| B2 clause already sealed | cite, no reexecution |
| B2 governance gate too broad/brittle | narrow to structured truth source |

---

# 15. EVIDENCE TABLE TEMPLATES

## 15.1 Corpus census

| Corpus | Plan | Dependency | Status | Evidence | Remainder | Queue rank |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## 15.2 Claim hygiene

| Claim | Paths | Before | Completion evidence | Verify | After |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## 15.3 Micro-deferrals

| Item | Parent | Class | Current state | Disposition | Evidence |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## 15.4 Plan 27 fidelity

| Target | Old test pattern | Current owner | New behavioral oracle | Save/replay | Result |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## 15.5 C1[7] clause reconciliation

| Clause | Historical requirement | Sealed by | Current evidence | Remainder | Action |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

---

# 16. PART 1 FINAL CLOSEOUT

Before declaring Wave 10 Part 1 complete:

1. Confirm every actual corpus file was enumerated.
2. Confirm every corpus row has one status.
3. Confirm dependency DAG is valid or cycles/unresolved refs are explicit blockers.
4. Confirm C1[6] prerequisite status is known.
5. Confirm Plan 23/Plan 36 prerequisite status is known for future queueing.
6. Confirm stale economy claim is closed only if current acceptance passes.
7. Confirm all remaining ACTIVE claims are genuinely active or flagged.
8. Confirm known radio weather-prediction micro-gap is terminal or correctly routed.
9. Confirm all discovered micro-deferrals have dispositions.
10. Confirm B1 followed 27A→27B→27C.
11. Confirm no vanity coverage goal was introduced.
12. Confirm no synthetic production selftest fixture was retained where forbidden.
13. Confirm new journeys are repeatably deterministic.
14. Confirm comprehensive save evidence is green.
15. Confirm C1[6] implementation log exists.
16. Confirm C1[7] implementation log credits prior work rather than redoing it.
17. Confirm census updated after B1/B2.
18. Confirm docs-index/generator checks.
19. Run build.
20. Run verify-fast.
21. Write `WAVE10_PART1_CLOSEOUT.md`.
22. Derive Part 2 only from the newly ranked queue and surviving blockers.

---

# 17. PART 1 NON-GOALS

- no mass execution of the 78-plan corpus;
- no assumption that higher-numbered plan is next;
- no dependency waiver;
- no historical plan body rewrite;
- no stale claim closure without acceptance;
- no arbitrary TODO sweep;
- no unbounded content authoring;
- no global test coverage percentage campaign;
- no production API changes solely to satisfy low-fidelity tests;
- no flaky journey retention;
- no duplicate governance ledger;
- no reexecution of Waves 3–5 governance work;
- no C1[8]+ execution unless A1 proves it is the current bounded chain head and Part 2 explicitly promotes it.




# APPENDIX A — A1 CORPUS CENSUS EXECUTION CARDS

## A1.01 Mechanical enumeration
**Action:** list actual files under `C-integration-plans/`.
**Evidence:** sorted file list.
**Fail if:** expected numeric range is substituted for real enumeration.

## A1.02 Header extraction
Extract title/source baseline/dependencies/order from every file.

## A1.03 Identity normalization
Use corpus key + file + title + source baseline to avoid numeric collisions.

## A1.04 Dependency edge build
One edge per declared hard prerequisite.

## A1.05 Missing dependency resolution
Search other corpora/closeouts before calling unresolved.

## A1.06 Cycle detection
Report cycles as planning blockers.

## A1.07 Ledger cross-reference
Search `INTEGRATION_PLANS`.

## A1.08 Claims cross-reference
Search DONE/HANDED_OFF/ACTIVE claims.

## A1.09 Closeout cross-reference
Search implementation logs and completion docs.

## A1.10 Wave cross-reference
Map Waves 1–9 tasks to plan clauses.

## A1.11 C1[8] anchor
Plan 31 overlap with Wave 9 B1.

## A1.12 C1[7] anchor
One Truth overlap with prior governance waves.

## A1.13 C2[8] anchor
Ship gate overlap.

## A1.14 C2[9] anchor
Orchestration/decomposition overlap.

## A1.15 Plan 26 prerequisite
Resolve 26A/26B.

## A1.16 Plan 23 prerequisite
Resolve power/heat.

## A1.17 Plan 36 prerequisite
Resolve producer-port gate.

## A1.18 Source-premise grep
Check named current systems.

## A1.19 SEALED-ELSEWHERE evidence
Clause-level citations.

## A1.20 PARTIALLY-SEALED remainder
Executable wording.

## A1.21 STALE-PREMISE banner
Header only; history preserved.

## A1.22 SUPERSEDED link
Name newer owner/plan.

## A1.23 Queue readiness
Dependencies and claims all clear.

## A1.24 Criticality rank
Prefer DAG-unblocking chain head.

## A1.25 Census document
Generate status table/DAG/queue.

## A1.26 Docs index
Regenerate/check.

## A1.27 Zero production diff
Verify.

## A1.28 Handoff
Queue proposal + unresolved blockers.

---

# APPENDIX B — A2 CLAIM-HYGIENE EXECUTION CARDS

## A2.01 Economy claim evidence
Read claim + C1 completion.

## A2.02 Economy suite baseline
Current pass required.

## A2.03 New-system focused tests
Trade embargo / price atlas current pass.

## A2.04 Integrity
Current pass.

## A2.05 Claim status transition
Use existing convention.

## A2.06 Completion pointer
Add closeout/evidence.

## A2.07 ACTIVE sweep
Classify every active row.

## A2.08 HANDED_OFF sweep
Find acceptance debt.

## A2.09 Overlap detection
Normalize paths.

## A2.10 Plan 24 recheck
Do not assume still active.

## A2.11 Ledger sync
Update contradictory checkpoints.

## A2.12 Table integrity
No formatting/schema churn.

## A2.13 Build
Governance-only assertion.

## A2.14 Verify-fast
Cheap all-clear.

## A2.15 Handoff
Before/after state.

---

# APPENDIX C — A3 MICRO-DEFERRAL EXECUTION CARDS

## A3.01 Marker inventory
Search only recorded deferral sources.

## A3.02 Deduplicate
Same item may appear in claim + closeout + debt.

## A3.03 Classify
Content/decision/small-code/large-code/resolved.

## A3.04 Known radio forecast gap
Reverify.

## A3.05 Forecast catalog owner
Locate exact data source.

## A3.06 Reliability contract
Copy from current model, not intuition.

## A3.07 Author rows
Only missing recorded scope.

## A3.08 Reference validation
IDs/schema.

## A3.09 Utilization
Every row consumed.

## A3.10 Radio tests
Current suite.

## A3.11 World/weather tests
Current suite.

## A3.12 Snapshot
Only if visible diff.

## A3.13 Decision route
No implementation.

## A3.14 Small wiring
One owner/two-call scale only.

## A3.15 Large gap promotion
A1 queue.

## A3.16 Already-resolved strike
Evidence citation.

## A3.17 Parent closeout status
Only if all deferrals terminal.

## A3.18 Sweep report
Permanent ledger.

## A3.19 Build
Green.

## A3.20 Verify-fast
Green.

---

# APPENDIX D — B1 PLAN 27 EXECUTION CARDS

## B1.01 Prerequisite gate
26A/26B terminal first.

## B1.02 Full contract extraction
27A/27B/27C exact.

## B1.03 Current test count
Measure; do not trust vintage.

## B1.04 Existing journey inventory
Replay/save/scenarios.

## B1.05 Historical gap subtraction
Credit earlier waves.

## B1.06 27A target list
Artifact/bootstrap fidelity.

## B1.07 Authority-backed fixture
Use current loader/root.

## B1.08 No production test fixture
Remove hidden synthetic fixture if prohibited.

## B1.09 27B anti-pattern scan
Candidate generation only.

## B1.10 Constructor-only target
Upgrade behavior assertion.

## B1.11 Does-not-throw target
Upgrade typed/state assertion.

## B1.12 Callback target
Assert invocation/payload.

## B1.13 Save target
Restore and compare.

## B1.14 Event target
Assert once/payload.

## B1.15 Fake console target
Replace with current adapter/authority pattern.

## B1.16 Focused file run
Alone.

## B1.17 Adjacent run
Neighborhood.

## B1.18 27C journey inventory
Exact plan-named journeys.

## B1.19 Journey bootstrap
Real composition.

## B1.20 Journey intermediate oracle
State.

## B1.21 Journey save midpoint
If required.

## B1.22 Journey terminal oracle
Authoritative state.

## B1.23 Journey fingerprint
Deterministic.

## B1.24 N-repeat
Zero variance.

## B1.25 Comprehensive save battery
Green.

## B1.26 Fidelity metric
Plan-defined.

## B1.27 Claim discipline
No Plan 24 paths.

## B1.28 Implementation log
C1[5]-style.

## B1.29 Census update
C1[6] terminal.

## B1.30 Ledger update
Package row.

## B1.31 Build
Green.

## B1.32 Verify-fast
Green.

---

# APPENDIX E — B2 ONE-TRUTH EXECUTION CARDS

## B2.01 Full C1[7] read
All clauses.

## B2.02 Census row consume
Use A1 evidence.

## B2.03 Wave 3 D3 credit
No reexecution.

## B2.04 Wave 4 B1 credit
No reexecution.

## B2.05 Wave 5 A2 credit
No reexecution.

## B2.06 Wave 10 A1 credit
No duplicate census.

## B2.07 Clause remainder table
Exact.

## B2.08 Canon registry audit
If in scope.

## B2.09 Capability source proof
Per row.

## B2.10 Overstatement correction
Truthful status.

## B2.11 Understatement correction
Truthful status.

## B2.12 Single-ledger check
If plan requires.

## B2.13 CI enforcement gap
Add cheap check only if named.

## B2.14 Historical snapshot preservation
No rewrites.

## B2.15 Generated docs
Input-first.

## B2.16 Focused governance tests
Green.

## B2.17 Implementation log
Reconciliation evidence.

## B2.18 Census update
Chain moves.

## B2.19 Docs index
Green.

## B2.20 Build
Green.

## B2.21 Verify-fast
Green.

---

# APPENDIX F — CORPUS CENSUS REVIEW QUESTIONS

For each plan:
- Does the file actually exist?
- What is its exact source baseline?
- What dependencies are hard vs contextual?
- What mandatory order exists?
- Which clauses are already implemented?
- Which prior wave/package did that work?
- Does current source still match the plan’s premise?
- Is any named owner now replaced?
- Is any acceptance gate obsolete?
- Is any current path actively claimed?
- Does the plan require a product decision?
- Is the remainder bounded?
- What downstream plans depend on it?
- What exact evidence supports status?
- What would make the status wrong?

---

# APPENDIX G — PLAN 27 ANTI-PATTERN REVIEW

A test is a candidate for upgrade if its only meaningful assertion is:
- constructor succeeds;
- object non-null;
- method does not throw;
- fake callback wired but result unasserted;
- serialization capture without restore;
- event subscription without trigger;
- UI open without bound model;
- hardcoded ending/state injected instead of owner-driven;
- fake console unrelated to production adapter;
- golden blindly refreshed.

Each candidate requires human/agent contract review before edit. Do not mass-transform.

---

# APPENDIX H — CLAIM TABLE REVIEW QUESTIONS

For every claim:
- Is status current?
- Is owner still active?
- Do claimed paths still require exclusivity?
- Is completion doc present?
- Did acceptance pass?
- Is HANDOFF accepted?
- Are paths overlapping another ACTIVE claim?
- Is a generated artifact/input also shared?
- Does ledger agree?

---

# APPENDIX I — MICRO-DEFERRAL REVIEW QUESTIONS

For every item:
- Was it explicitly recorded?
- Is it still missing?
- Does it have a live consumer?
- Is it content-only?
- Does it require product decision?
- Is code gap truly small?
- Did later wave already seal it?
- Does closing it change parent completion?
- What focused gate proves closure?

---

# APPENDIX J — PART 1 HANDOFF TEMPLATE

## Status
**Task:**<br>
**Terminal state:**<br>
**HEAD:**<br>
**Claims checked:**<br>

## Premise
**Historical blocker:**<br>
**Current evidence:**<br>
**Changed premise:**<br>

## Work
**Files changed:**<br>
**Production behavior changed?:**<br>
**Docs/data/tests changed:**<br>

## Verification
**Focused:**<br>
**Adjacent:**<br>
**Save/replay:**<br>
**Generators:**<br>
**Build:**<br>
**Verify-fast:**<br>

## Truth updates
**Census:**<br>
**Claim table:**<br>
**Integration ledger:**<br>
**Closeout/log:**<br>

## Remaining
**Exact blocker:**<br>
**Dependency:**<br>
**Claim:**<br>
**Decision:**<br>
**Next chain head:**<br>

---

# APPENDIX K — WAVE 10 PART 2 PROMOTION FILTER

Part 2 must be generated from the post-B2 census, not from raw corpus numbering.

For each candidate:
1. Is status OPEN-UNCLAIMED or PARTIALLY-SEALED?
2. Are all dependencies terminal?
3. Is premise current?
4. Is required owner clear?
5. Are paths unclaimed?
6. Are decisions signed?
7. Can package be bounded?
8. Does it unblock downstream DAG?
9. Is acceptance measurable?
10. Is it preferable to another ready chain head by DAG criticality?

Candidate record:

`Corpus key | Title | Remainder | Dependencies | Claims | Decisions | Downstream unlocked | Package size | Acceptance | Recommended rank`.

Part 2 should begin at the highest ready DAG node, not automatically C1[8].

---

# APPENDIX L — FINAL QUALITY BAR

Wave 10 Part 1 is acceptable only if:

- the “~78 files” claim is replaced by an exact mechanical inventory;
- every corpus file has evidence-backed status;
- known overlaps are reconciled clause-by-clause;
- prerequisite unknowns are resolved or explicitly block dependents;
- the stale economy claim no longer blocks completed paths if acceptance is green;
- claim table has no knowingly stale ACTIVE rows left unclassified;
- micro-deferrals are drained/routed rather than rediscovered later;
- Plan 27 execution is fidelity-driven, not count-driven;
- Plan 27 journeys are stable across repeated seeded runs;
- C1[7] credits already-sealed governance work;
- no historical plan is re-executed simply because its corpus file lacked an implementation log;
- census and implementation logs become the source for Part 2 queueing;
- Part 2 can be generated without rereading all 78 plans from scratch.

**End of Wave 10 Part 1 implementation plan.**

# APPENDIX M — CORPUS CENSUS CLASSIFICATION PROTOCOL

This appendix defines exactly how A1 classifies each plan so that two agents given the same repository state should reach the same result.

## M.1 Step 1 — establish plan identity

For each file, record:

- absolute repository-relative path;
- corpus family (`C1`, `C2`, `D1`, `E1`);
- bracket/index identifier;
- exact title;
- exact source-baseline Plan ID(s);
- creation/revision date if present;
- explicit predecessor/successor plan references.

Do not identify a plan by number alone.

### Identity conflict rule

If two documents use the same historical Plan ID but different title/scope:
- treat them as distinct documents;
- add collision note;
- follow corpus path/title as primary identity;
- map both to their historical source lineage.

## M.2 Step 2 — extract executable contract

Extract only enforceable statements:

- dependencies;
- mandatory sub-plan order;
- required owners;
- required data;
- save/persistence contract;
- UI/presentation contract;
- verification gates;
- explicit non-goals;
- deferrals;
- product decisions.

Do not promote descriptive rationale into acceptance if the plan does not treat it as mandatory.

## M.3 Step 3 — verify current premise

For every plan premise:

`Historical premise | Current source query | Current result | Still true? | Consequence`.

Examples:
- “No semantic event layer exists” may be false after Wave 9.
- “No ship/export smoke exists” may be partially false after earlier ship-gate work.
- “No orchestration spine exists” may be partially false after composition-root work.

A false premise does not necessarily mean the entire plan is stale; it may mean the plan is PARTIALLY-SEALED and only later clauses remain.

## M.4 Step 4 — search execution evidence

Evidence priority:

1. current source implementation;
2. focused current tests;
3. implementation log/closeout;
4. claim DONE/HANDED_OFF evidence;
5. integration ledger;
6. prior wave plan assignment;
7. historical prose.

A prior wave assignment without source/test evidence is not enough to mark SEALED.

## M.5 Step 5 — classify clause-by-clause

For each sub-plan clause:

- `SEALED-HERE` — implemented by a package explicitly associated with this corpus plan.
- `SEALED-ELSEWHERE` — implemented through another wave/package.
- `OPEN` — missing.
- `STALE` — premise no longer valid.
- `SUPERSEDED` — newer contract replaces it.
- `DECISION-BLOCKED`.
- `CLAIM-BLOCKED`.
- `DEPENDENCY-BLOCKED`.

Then compute plan-level status from clauses.

## M.6 Step 6 — compute plan-level status

### SEALED
All mandatory clauses terminal and no remainder.

### SEALED-ELSEWHERE
All mandatory clauses terminal, but implementation came from other packages.

### PARTIALLY-SEALED
At least one valid OPEN/BLOCKED clause remains.

### OPEN-UNCLAIMED
No mandatory implementation clause is sealed enough to satisfy plan and dependencies are otherwise clear.

### STALE-PREMISE
Plan’s central reason for existence is invalid and no meaningful current remainder exists.

### SUPERSEDED
A newer plan explicitly or functionally replaces its contract.

## M.7 Step 7 — derive remainder

Remainder wording must be:
- owner-specific;
- path discoverable;
- bounded;
- acceptance-testable.

Bad:
> Finish export smoke.

Good:
> Route the release smoke through the current exported-build launcher, assert artifact boot reaches the canonical composition root, and fail on fallback/mock bootstrap.

## M.8 Step 8 — dependency readiness

For every remainder:
- list hard dependencies;
- map each dependency status;
- mark readiness.

Readiness states:
- READY;
- WAITING-DEPENDENCY;
- WAITING-CLAIM;
- WAITING-DECISION;
- SUPERSEDED;
- NOT-ACTIONABLE.

## M.9 Step 9 — queue scoring

Use a transparent qualitative score:

### Dependency readiness
- 3 = all sealed;
- 2 = one bounded dependency;
- 1 = several unresolved;
- 0 = unknown/cyclic.

### Downstream unlock
- 3 = unblocks many plans;
- 2 = unblocks one major dependent;
- 1 = isolated;
- 0 = superseded.

### Authority clarity
- 3 = existing owner/API clear;
- 2 = owner clear, seam missing;
- 1 = overlapping authority;
- 0 = decision required.

### Package boundedness
- 3 = one package;
- 2 = two-phase;
- 1 = broad;
- 0 = undefined.

Ranking uses the score as an aid, not a replacement for DAG order.

## M.10 Step 10 — reviewer challenge

Before finalizing each classification, ask:

- What evidence would falsify this status?
- Did we check source after the last relevant wave?
- Are we conflating a plan ID collision?
- Is the “remainder” merely a restatement of the plan?
- Is there an active claim?
- Did a newer contract intentionally supersede this?

Only then commit census row.

---

# APPENDIX N — DAG CONSTRUCTION AND VALIDATION

## N.1 Raw edge format

Represent dependency edge as:

`<prerequisite corpus identity> -> <dependent corpus identity> [source line/section]`

If dependency references historical Plan ID:
- resolve to current corpus identity;
- if ambiguous, preserve unresolved marker.

## N.2 Dependency categories

### Hard
Plan explicitly says “depends on”, “requires”, mandatory order, or acceptance impossible without it.

### Soft/context
Plan references system for integration but can execute independently.

### Historical
Dependency was required at plan authoring but current architecture has superseded it.

Only hard current dependencies gate queue readiness.

## N.3 Sub-plan order

Mandatory order such as:
`27A -> 27B -> 27C`

must be represented inside plan node or as subnodes.

Do not mark Plan 27 sealed if 27A/27B pass but 27C is open.

## N.4 Unknown dependency protocol

When header says Plan 26A/26B but no immediate mapping:
1. search corpus;
2. search docs/plans;
3. search integration ledger;
4. search implementation logs;
5. search current source for named capability;
6. classify unresolved only after all.

Unknown remains explicit blocker.

## N.5 Cycle protocol

If DAG cycle appears:
- verify it is not numbering collision;
- distinguish implementation dependency from integration context;
- if real hard cycle, route to foreman/architecture decision;
- do not choose arbitrary break.

## N.6 DAG output forms

Census should contain:
- human-readable Mermaid/ASCII dependency view if repository style permits;
- machine-friendly table;
- ranked ready queue;
- dependency-blocked queue.

## N.7 DAG update after B1/B2

When C1[6]/C1[7] close:
- recompute readiness;
- do not just increment index.

Part 2 may begin at:
- C1[8];
- another C1 plan;
- C2 chain head;
- prerequisite plan;
depending on actual DAG.

---

# APPENDIX O — CLAIM-HYGIENE RECONCILIATION PROTOCOL

## O.1 Claim-table snapshot

Before A2:
- copy current statuses into evidence artifact;
- record commit;
- record currently dirty governance files.

This provides audit before edits.

## O.2 Evidence hierarchy for claim closure

A claim is safe to close when all are true:
1. package has completion/handoff evidence;
2. current focused acceptance passes;
3. no owner reports in-flight changes;
4. linked plan/ledger says complete or equivalent;
5. no unmerged work depends on exclusivity.

## O.3 Economy claim exact closure

For the known stale economy claim:
- read claimed path list;
- verify each path exists/current;
- run cited acceptance;
- verify C1 completion;
- transition status.

Do not alter claim path contents.

## O.4 Sibling stale ACTIVE detection

Candidate:
- ACTIVE for > expected package duration;
- linked closeout exists;
- integration ledger DONE;
- no current commits/handoff pending.

But age alone is not proof.

## O.5 ACTIVE but acceptance-debt

If completion doc exists but current acceptance fails:
- status should remain active or be marked acceptance-debt according to table semantics;
- create repair/acceptance follow-up;
- do not close simply because historical gate passed.

## O.6 HANDOFF ambiguity

HANDED_OFF means:
- implementation owner released paths;
- acceptance may still be pending.

Do not normalize to DONE unless repository convention and evidence permit.

## O.7 Overlap detection matrix

`Claim A | Claim B | Overlapping path | Exact/parent/generated | Risk | Foreman action`.

Generated artifacts matter because two agents can race different inputs to same output.

## O.8 Claim hygiene closeout metric

Report:
- ACTIVE before;
- stale ACTIVE closed;
- acceptance-debt flagged;
- overlaps flagged;
- ACTIVE after;
- rows with missing evidence.

Metric is governance truth, not “number closed”.

---

# APPENDIX P — MICRO-DEFERRAL INVENTORY PROTOCOL

## P.1 Search expressions

Search recorded governance/docs for:
- `deferred`;
- `not-yet-authored`;
- `remaining`;
- `flagged`;
- `quirk`;
- `follow-up`;
- `pending decision`;
- `not implemented` only when inside a completed-package note.

Each match receives parent context before classification.

## P.2 Deduplication

Two docs may refer to same deferral.

Deduplicate by:
- parent plan;
- target owner/content ID;
- same semantic gap.

Keep all source references.

## P.3 Content-authorable classification

Requirements:
- schema/mechanism already exists;
- runtime consumer exists;
- missing work is authored rows only;
- no policy decision.

If any false, not content-only.

## P.4 Small code-gap classification

Requirements:
- current owner exists;
- current contract says exact wiring;
- no new persistence;
- no new data schema;
- no product policy;
- ≤ bounded owner/host seam.

Do not use line count as sole criterion.

## P.5 Decision-needed classification

Any choice affecting:
- pacing;
- balance;
- authority;
- player semantics;
- save model;
must route to decision.

## P.6 Already-resolved classification

Requires:
- current source;
- test;
- closeout/package evidence.

Then update stale note.

## P.7 Radio weather-prediction acceptance

For known content gap:

### Data
- rows match owning schema;
- IDs unique;
- references valid.

### Semantics
- wording reflects forecast uncertainty;
- reliability model not bypassed.

### Runtime
- radio selection/consumer can reach rows.

### Tests
- integrity;
- utilization;
- Radio;
- World/Weather;
- snapshot if applicable.

## P.8 Micro-gap report terminal state

Every inventory row:
- CLOSED-CONTENT;
- CLOSED-WIRING;
- RESOLVED-ELSEWHERE;
- DECISION-REGISTER;
- CORPUS-QUEUE;
- RETIRED.

No unclassified residue.

---

# APPENDIX Q — PLAN 27 FIDELITY-UPGRADE PLAYBOOK

## Q.1 Constructor-only example pattern

### Before
Test constructs system and asserts not null.

### After
- construct via production-like composition;
- perform canonical command/tick;
- assert typed result;
- assert authoritative state/event;
- assert failure/no-op semantics if relevant.

The exact domain behavior comes from current owner contract.

## Q.2 “Does not throw” pattern

A no-throw assertion may remain as secondary guard, but never sole acceptance for meaningful behavior.

Upgrade:
- trigger valid case;
- assert outcome;
- trigger invalid boundary;
- assert typed failure/no mutation.

## Q.3 Inert callback pattern

Before:
- callback passed;
- no assertion.

After:
- record call count;
- verify payload;
- verify ordering;
- verify exactly-once where contract says so.

## Q.4 Fake console/adapter pattern

If test uses fake interface unlike production:
- inspect current adapter;
- either use real adapter with controlled I/O seam;
- or use contract-faithful fake implementing same behavior boundaries.

Do not force production to match an obsolete fake.

## Q.5 Save capture-only pattern

Before:
- capture DTO;
- assert field.

After:
- mutate/advance;
- capture;
- restore into fresh owner;
- compare authoritative state;
- continue one step;
- compare deterministic suffix.

## Q.6 Event-presence pattern

Before:
- subscribe;
- call method;
- assert some event happened.

After:
- trigger exact authority edge;
- assert event type;
- payload;
- count;
- order;
- no duplicate on restore/repeat.

## Q.7 UI route-presence pattern

Before:
- route opens.

After:
- route opens;
- binds current read model;
- command routes;
- refresh on event;
- unbind lifecycle;
- accessibility/focus if appropriate.

## Q.8 Authority-backed fixture criteria

A fixture is high-fidelity when:
- uses canonical loader/catalog;
- uses production composition or supported test composition;
- shares validation semantics;
- avoids hidden “demo only” shortcuts.

## Q.9 Production selftest fixture audit

Flag selftest if it:
- silently constructs fake catalog not used in production;
- uses hard-coded IDs absent from authoritative data;
- injects fake ending/world state that bypasses real owner;
- reports PASS without behavior assertions.

Replacement must remain deterministic and fast.

## Q.10 Fidelity target selection

Choose highest-risk low-fidelity tests.

Risk factors:
- save/persistence;
- campaign continuity;
- economy;
- survivor state;
- world/expedition;
- event routing;
- composition/bootstrap.

Do not waste package on trivial value objects while critical journeys remain shallow.

## Q.11 Journey acceptance design

Each journey has:
- intent;
- owners crossed;
- setup;
- action sequence;
- intermediate invariants;
- save point;
- terminal oracle;
- deterministic fingerprint.

## Q.12 Journey N-repeat protocol

N selected per repository policy/plan.

Record:
`Run | Seed | Duration | Fingerprint | Result`.

Any variance requires investigation.

## Q.13 Flake sources to eliminate

- wall clock;
- unordered collections;
- shared static state;
- non-reset RNG;
- async timing without deterministic await;
- external filesystem residue;
- test-order dependency.

Do not “stabilize” by increasing sleeps.

## Q.14 Golden refresh policy

If fidelity improvement changes golden:
- inspect reason;
- prove new output correct;
- preserve prior artifact for review if policy;
- update with evidence.

No batch accept-all.

## Q.15 Fidelity closeout table

`Target | Anti-pattern | Upgrade | Production owner | Behavior assertion | Save/replay | N-run | Result`.

---

# APPENDIX R — PLAN 27 27A/27B/27C CONTROL SHEETS

## R.1 27A gate

Before 27B:
- Plan 26A/26B dependency terminal;
- artifact/bootstrap fidelity clauses green;
- no unresolved prerequisite.

## R.2 27B gate

Before 27C:
- targeted shallow tests upgraded;
- authority-backed fixtures established;
- no new flaky tests;
- no production API distortion.

## R.3 27C gate

Before Plan 27 closure:
- plan-named campaign journeys implemented;
- seeded repeat stable;
- save battery green;
- runtime evidence metric recorded.

## R.4 Partial closure

If 27A/27B complete but 27C claim-blocked:
- Plan 27 PARTIALLY-SEALED;
- do not mark SEALED;
- exact claimed journey paths/owner recorded.

---

# APPENDIX S — C1[7] RECONCILIATION PLAYBOOK

## S.1 Clause-by-clause subtraction

For each clause:
1. identify intended output;
2. identify prior package;
3. verify current source;
4. verify current gate;
5. mark sealed or remainder.

## S.2 Agent-sync overlap

If Wave 3 already established:
- canonical rule sources;
- sync process;
- integrity tests;
credit it.

Only execute C1[7] additions not covered.

## S.3 Docs-atlas overlap

If Wave 4 created:
- docs index;
- generator/check;
- plan mapping;
credit it.

Verify current gate still green.

## S.4 Skill-doc truth overlap

If Wave 5 established skill truth pass:
- inspect scope;
- compare C1[7] contract;
- execute only missing enforcement.

## S.5 Census overlap

A1 already supplies corpus truth.

Do not build another roadmap truth table inside C1[7].

## S.6 Canon registry audit

If C1[7] requires canonical registry:
- enumerate current registry rows;
- verify implementation status;
- correct drift;
- add structured check if missing.

## S.7 Overstatement severity

Critical:
- claims implemented capability absent at runtime.

High:
- claims wired consumer absent.

Medium:
- stale completion wording.

Low:
- outdated file path.

Prioritize semantic truth.

## S.8 Understatement

Also correct:
- shipped feature still marked planned;
- completed package missing closeout pointer.

“One Truth” means both directions.

## S.9 Enforcement vs convention

If current truth depends solely on humans remembering:
- and C1[7] explicitly requires enforceable governance,
add cheap CI/check.

If no structured source exists, avoid brittle semantic linter; instead improve source-of-truth structure only if plan authorizes.

## S.10 C1[7] terminal log

Log must show:
- clauses;
- sealed elsewhere;
- executed here;
- remainder;
- no-op evidence.

This prevents future wave from treating absence of code diff as absence of completion.

---

# APPENDIX T — STATUS BANNER STANDARD

For stale/superseded historical plan docs:

> **STATUS (YYYY-MM-DD): STALE-PREMISE / SUPERSEDED / SEALED-ELSEWHERE.**<br>
> Current disposition: `<summary>`.<br>
> Evidence: `<closeout/log/census reference>`.<br>
> Historical body retained below for provenance.

Do not rewrite body.

---

# APPENDIX U — IMPLEMENTATION LOG STANDARD FOR CORPUS PLANS

Each newly executed corpus plan gets:

## 1. Plan identity
Corpus file, title, source baseline.

## 2. Prerequisites
Declared vs current status.

## 3. Premise update
Historical vs current.

## 4. Clause matrix
Every mandatory clause.

## 5. Sealed elsewhere
Prior packages credited.

## 6. Work executed
Files/owners.

## 7. Tests
Focused/adjacent.

## 8. Save/replay
If relevant.

## 9. Claims
Paths and handoff.

## 10. Acceptance
Per plan contract.

## 11. Remainder
Exact.

## 12. Census update
Final status and next chain.

---

# APPENDIX V — PART 1 NEGATIVE-CASE REGISTRY

## V.1 A1
- [ ] duplicate corpus index with different plan identity;
- [ ] unresolved dependency reference;
- [ ] hard cycle;
- [ ] plan assigned in old wave but implementation absent;
- [ ] source class exists but acceptance missing;
- [ ] plan premise false;
- [ ] active claim blocks ready plan;
- [ ] decision blocks ready plan.

## V.2 A2
- [ ] completion doc exists but tests fail;
- [ ] ACTIVE claim has unmerged owner work;
- [ ] HANDOFF lacks acceptance;
- [ ] two ACTIVE claims overlap exact path;
- [ ] parent/child claim overlap;
- [ ] generated input/output race.

## V.3 A3
- [ ] same deferral recorded in multiple docs;
- [ ] content row already authored;
- [ ] content mechanism missing;
- [ ] decision needed;
- [ ] small gap actually changes policy;
- [ ] parent cannot close after micro-gap due to larger remainder.

## V.4 B1
- [ ] 26A/26B unknown;
- [ ] historical test count stale;
- [ ] anti-pattern grep false positive;
- [ ] upgraded test exposes production bug;
- [ ] test-order dependency;
- [ ] journey nondeterminism;
- [ ] save restore mismatch;
- [ ] active Plan 24 overlap.

## V.5 B2
- [ ] entire clause already sealed;
- [ ] canon registry source no longer authoritative;
- [ ] duplicate ledger temptation;
- [ ] CI check would be prose heuristic;
- [ ] archived rulebook threatened;
- [ ] remainder larger than package.

---

# APPENDIX W — COMMAND/EVIDENCE LOG TEMPLATE

For every verification command:

**Task:**<br>
**Purpose:**<br>
**Command:**<br>
**HEAD:**<br>
**Working tree notes:**<br>
**Expected:**<br>
**Actual result:**<br>
**Cases:**<br>
**Duration:**<br>
**Warnings:**<br>
**Artifact/log path:**<br>
**Disposition:**<br>

Commands should be copied exactly into implementation logs.

---

# APPENDIX X — REVIEW GATES BEFORE MERGE

## X.1 A1 review
- exact corpus count, not approximate;
- every row status evidence;
- DAG validated;
- prerequisites mapped;
- zero production change;
- queue ranked.

## X.2 A2 review
- stale economy claim acceptance green;
- no active overlap hidden;
- claim table/ledger agree.

## X.3 A3 review
- recorded gaps only;
- all inventory rows terminal/routed;
- content utilized;
- decisions untouched.

## X.4 B1 review
- dependency satisfied;
- 27A→27B→27C respected;
- shallow tests upgraded behaviorally;
- no vanity metrics;
- journeys stable;
- save battery green.

## X.5 B2 review
- prior waves credited;
- only remainder executed;
- no duplicate governance architecture;
- truth checks structured/cheap.

---

# APPENDIX Y — WAVE 10 PART 2 INPUT CONTRACT

Part 1 closeout must output a machine/human-readable queue:

| Rank | Corpus key | Title | Status | Remainder | Hard deps | Claims | Decisions | Downstream | Package recommendation |
|---:|---|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |  |

Part 2 planner must:

1. re-verify top candidates at current `HEAD`;
2. discard any sealed by Part 1/concurrent work;
3. obey dependency order;
4. split broad plans into bounded packages where source plan already defines phases;
5. preserve mandatory sub-plan order;
6. continue implementation-log precedent;
7. update census after every package.

If C1[8] becomes next chain head, use the full richer C1[8] contract, not only Wave 9’s smaller Plan 31 semantic-kind slice.

---

# APPENDIX Z — FINAL NO-FALSE-CLOSURE RULES

Wave 10 Part 1 is **not complete** if:

- any corpus file remains unclassified because it was “too large to inspect”;
- dependency graph contains unresolved references that are omitted from queue logic;
- economy claim stays ACTIVE after green completion evidence with no reason;
- claim is closed despite failing current acceptance;
- radio forecast rows are authored but not consumed;
- a micro-deferral is silently dropped rather than dispositioned;
- Plan 27 is marked complete after only increasing test count;
- new journeys are not repeat-stable;
- save fidelity is not exercised where Plan 27 requires it;
- C1[7] reimplements prior waves instead of citing them;
- a governance check creates a second mutable source of truth;
- Part 2 queue is simply “next file number” rather than post-census DAG readiness.

Wave 10’s purpose is to convert the hidden planning corpus into trustworthy execution state. The success metric is not the number of plans touched; it is whether the repository can answer, for every corpus plan: **what is already true, what is still missing, what blocks it, and what exact package should execute next.**

# APPENDIX AA — CORPUS ROW AUDIT PACKET

Create one audit packet per corpus file. This is the unit of evidence behind the census.

## AA.1 Identity
**Corpus key:**<br>
**File:**<br>
**Title:**<br>
**Source baseline:**<br>
**Revision/date:**<br>

## AA.2 Historical contract
**Dependencies:**<br>
**Mandatory order:**<br>
**Named owners:**<br>
**Acceptance:**<br>
**Non-goals:**<br>
**Deferrals:**<br>

## AA.3 Current repository evidence
**Named classes/files found:**<br>
**Named data found:**<br>
**Named panels/routes found:**<br>
**Named tests/selftests found:**<br>
**Save/persistence evidence:**<br>
**Closeout/implementation log:**<br>
**Claim row:**<br>
**Integration ledger row:**<br>

## AA.4 Clause status
| Clause | Historical requirement | Current truth | Evidence | Status | Remainder |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## AA.5 Dependency state
| Dependency | Resolved identity | Status | Evidence | Blocks this plan? |
|---|---|---|---|---|
|  |  |  |  |  |

## AA.6 Claim state
| Required path | Active claim? | Claim owner | Safe now? | Action |
|---|---|---|---|---|
|  |  |  |  |  |

## AA.7 Final classification
**Plan-level status:**<br>
**Reason:**<br>
**Exact remainder:**<br>
**Queue readiness:**<br>
**Downstream unlock:**<br>
**Next action:**<br>

A reviewer must be able to derive the census row from this packet without rereading the whole repository.

---

# APPENDIX AB — PREREQUISITE-RESOLUTION PROTOCOL

## AB.1 Direct prerequisite
If a plan names another plan/file exactly:
- resolve that file;
- inspect current status;
- use its own implementation log/census row.

## AB.2 Historical plan-ID prerequisite
If only numeric ID is named:
1. search current corpus;
2. search historical docs;
3. map title/scope;
4. detect collisions;
5. resolve to current identity.

## AB.3 Capability prerequisite
If dependency is phrased as capability:
- identify current owner;
- verify capability acceptance;
- record sealing package.

Do not require historical package execution if newer architecture already provides equivalent required capability.

## AB.4 Superseded prerequisite
If prerequisite plan is superseded:
- verify newer authority satisfies dependent plan’s needed contract;
- mark dependency SATISFIED-BY-SUPERSEDING-AUTHORITY.

## AB.5 Partial prerequisite
If dependent requires only one sub-clause:
- verify whether that clause is sealed;
- do not block entire dependent on unrelated remainder unless plan declares full-plan dependency.

## AB.6 Unknown
Unknown remains explicit.
Never convert UNKNOWN to “probably done”.

## AB.7 Plan 26A/26B packet
For B1 specifically capture:
- where 26A/26B docs live;
- artifact/bootstrap contract;
- current implementation;
- current tests;
- status;
- whether Plan 27 requires full or specific sub-clause completion.

## AB.8 Plan 23 packet
For future C1[10]:
- power/heat owner;
- save state;
- player/runtime consumer;
- implementation log;
- tests;
- exact dependency clause.

## AB.9 Plan 36 producer-port packet
Resolve:
- what “producer-port gate” means in current architecture;
- whether a producer interface/port already landed;
- gate/test;
- dependent production systems.

These packets become reusable Wave 10 evidence.

---

# APPENDIX AC — STALE-PREMISE DECISION RULES

A plan receives STALE-PREMISE only when its central “missing state” is contradicted by current source.

Examples:
- plan says no semantic layer; current total semantic authority exists;
- plan says no route; current panel/route/gate exists;
- plan says no composition root; current canonical root exists.

Do **not** mark stale merely because implementation differs from original design.

If current implementation satisfies intent through different architecture:
- SEALED-ELSEWHERE or SUPERSEDED is usually better.

If current implementation partially satisfies:
- PARTIALLY-SEALED.

STALE-PREMISE is for plan assumptions that no longer describe the problem.

---

# APPENDIX AD — CLAIM CONFLICT HANDLING

## AD.1 Exact path overlap
Do not edit.

## AD.2 Parent/child overlap
Treat as overlap unless claim policy explicitly permits.

## AD.3 Generated artifact overlap
If Agent A claims generator input and Agent B claims output:
- conflict;
- output ownership follows generator workflow.

## AD.4 Shared documentation
If implementation log/census is shared:
- serialize edits;
- avoid concurrent manual merges that can lose status rows.

## AD.5 Claim handoff
Required:
- owner releases;
- current diff committed/handed off;
- acceptance state recorded;
- new agent claims path.

## AD.6 Stale claim candidate
Never “steal” path.
A2 must formally transition stale row first.

---

# APPENDIX AE — PLAN 27 JOURNEY ORACLE TEMPLATE

Use for every 27C journey.

## AE.1 Identity
**Journey:**<br>
**Plan clause:**<br>
**Owners crossed:**<br>
**Seed:**<br>
**Expected duration:**<br>

## AE.2 Bootstrap
**Composition root:**<br>
**Catalogs/data:**<br>
**Initial survivor/world state:**<br>
**No synthetic production fixture proof:**<br>

## AE.3 Actions
1.<br>
2.<br>
3.<br>

## AE.4 Intermediate invariants
- after step 1:
- after step 2:
- before save:
- after restore:

## AE.5 Save midpoint
**Captured state:**<br>
**Fresh runtime restore:**<br>
**Parity fields/fingerprint:**<br>

## AE.6 Terminal oracle
**Authoritative expected state:**<br>
**Events expected:**<br>
**Events forbidden:**<br>
**Player-visible result:**<br>

## AE.7 Repeat stability
| Run | Fingerprint | Terminal state | Duration | Result |
|---:|---|---|---:|---|
| 1 |  |  |  |  |
| 2 |  |  |  |  |
| 3 |  |  |  |  |

Any divergence blocks merge.

---

# APPENDIX AF — PLAN 27 FIDELITY SCORING WITHOUT VANITY COVERAGE

The plan forbids global vanity targets. Use a qualitative runtime-evidence scale per upgraded target:

### Level 0 — presence only
Constructor/non-null/no-throw.

### Level 1 — local behavior
Command/result/state asserted.

### Level 2 — authority integration
Real owner/event/data path asserted.

### Level 3 — persistence/continuity
Save/restore or cross-owner journey asserted.

### Level 4 — deterministic campaign evidence
Seeded multi-owner journey with replay/fingerprint.

Report distribution of targeted files before/after.

Do **not** declare repo-wide score target unless original Plan 27 defines one.

Use this scale only as explanatory review tool, not CI metric unless plan explicitly requires it.

---

# APPENDIX AG — “DOES NOT THROW” HUNT PROCEDURE

1. Search test sources for common no-throw constructs.
2. Search constructor-only patterns.
3. Search non-null-only tests.
4. Group matches by subsystem.
5. Exclude legitimate smoke tests whose contract is startup/no-crash.
6. Rank remaining candidates.
7. Manually inspect before modifying.

Output:
`File | Test | Pattern | Legitimate smoke? | Upgrade candidate? | Owner | Risk`.

No automated bulk rewrite.

---

# APPENDIX AH — AUTHORITY-BACKED FIXTURE REQUIREMENTS

A test fixture is authority-backed when:

- IDs come from canonical catalog or validated fixture generated through same schema;
- state is constructed through supported owner APIs;
- composition uses production root or sanctioned test composition;
- time uses deterministic campaign clock;
- RNG uses seeded repository stream;
- persistence uses actual capture/restore contracts;
- events flow through canonical bus/coordinator;
- player surfaces bind actual read models.

A fake may still be valid for external boundary isolation, but it must implement current contract faithfully and not replace the behavior under test.

---

# APPENDIX AI — SAVE BATTERY REVIEW FOR PLAN 27

Before calling save battery green, record:

- current battery files;
- case count;
- schema versions covered;
- corruption cases;
- missing-section cases;
- round-trip cases;
- mid-journey restore cases;
- exactly-once ledger cases;
- generated contract checks.

If known standing failures exist:
- reconcile baseline;
- name them;
- new B1 failures cannot be hidden behind them.

---

# APPENDIX AJ — C1[7] CANON REGISTRY AUDIT PACKET

## AJ.1 Registry identity
**File/source:**<br>
**Generated or hand-authored:**<br>
**Canonical input:**<br>

## AJ.2 Row verification
| Capability | Registry status | Current source | Runtime consumer | Tests | Correct status |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## AJ.3 Drift types
- false positive: marked implemented, absent;
- false negative: shipped, marked planned;
- stale path;
- stale owner;
- stale closeout link;
- duplicate capability row.

## AJ.4 Correction rule
Fix canonical input, regenerate if derived.

## AJ.5 Enforcement
If C1[7] requires check:
- validate structural consistency;
- avoid interpreting free-form prose.

---

# APPENDIX AK — ROADMAP SINGLE-TRUTH AUDIT

Identify all plan-status surfaces:
- integration ledger;
- claim table;
- corpus census;
- known debt;
- completion docs;
- architecture/canon registry;
- generated indexes.

For each:
`Surface | What truth it owns | What it derives | Who writes | How checked`.

No surface should claim ownership of the same mutable status dimension without synchronization rule.

Example:
- claim table owns path exclusivity/state;
- census owns corpus-plan classification;
- integration ledger owns active package lifecycle;
- closeout logs own historical acceptance evidence.

If C1[7] expects “one truth”, interpret as one authoritative owner **per truth dimension**, not one giant document containing everything.

---

# APPENDIX AL — MICRO-DEFERRAL CONTENT ACCEPTANCE PACKET

For each content-only micro-gap:

**Parent package:**<br>
**Recorded deferral:**<br>
**Catalog:**<br>
**Consumer:**<br>
**Schema:**<br>
**Rows before:**<br>
**Rows added:**<br>
**References:**<br>
**Runtime reachability:**<br>
**Integrity result:**<br>
**Utilization result:**<br>
**Owning suite:**<br>
**Snapshot:**<br>
**Parent status after:**<br>

This prevents authored content from being treated as closed when runtime cannot reach it.

---

# APPENDIX AM — PART 1 METRICS THAT MATTER

Do report:
- exact corpus file count;
- classification counts;
- dependency-blocked count;
- ready queue count;
- stale ACTIVE claims corrected;
- acceptance-debt claims;
- micro-deferrals by disposition;
- Plan 27 targets upgraded by fidelity category;
- Plan 27 journey count and repeat stability;
- C1[7] clauses sealed elsewhere vs executed here.

Do not report as success metrics:
- lines changed;
- raw number of tests added;
- percentage of corpus files “touched”;
- amount of documentation generated.

---

# APPENDIX AN — PART 1 MERGE ORDER

Recommended merge order:

1. A1 census.
2. A2 claim hygiene.
3. A3 micro-deferral sweep.
4. B1 Plan 27.
5. B2 One Truth.
6. Part 1 closeout.

After each merge:
- refresh `HEAD`;
- re-run claims check;
- update census readiness.

Do not prepare B2 from stale pre-B1 census.

---

# APPENDIX AO — PART 1 CLOSEOUT TABLE

| Task | Start blocker | Terminal state | Key evidence | Production changed? | Census effect | Next dependency released |
|---|---|---|---|---|---|---|
| A1 | unreconciled corpus |  |  | no | full map |  |
| A2 | stale claims |  |  | no | claims availability |  |
| A3 | micro-deferrals |  |  | maybe bounded | parent statuses |  |
| B1 | C1[6] |  |  | tests/fixtures only unless repair | C1[6] |  |
| B2 | C1[7] remainder |  |  | governance/docs only expected | C1[7] |  |

---

# APPENDIX AP — WAVE 10 PART 2 START CONDITIONS

Part 2 may be authored only after:

- A1 census committed;
- A2 claim table reconciled;
- A3 micro-gap dispositions known;
- B1 terminal or correctly dependency-blocked;
- B2 terminal or correctly queued;
- census reranked at latest HEAD.

Part 2 source must therefore cite:
- current census row(s);
- current claim state;
- current dependencies;
- current remainder.

If Part 1 execution surfaces a different chain head than historical expectation, Part 2 follows the new evidence.

---

# APPENDIX AQ — FINAL IMPLEMENTER CHECKLIST

Before A1:
- [ ] claim docs readable;
- [ ] corpus directory enumerated;
- [ ] wave/closeout sources available.

Before A2:
- [ ] A1 census committed;
- [ ] economy completion evidence gathered.

Before A3:
- [ ] claim table current;
- [ ] micro-gap search sources current.

Before B1:
- [ ] 26A/26B resolved;
- [ ] B1 exact paths unclaimed;
- [ ] current test baseline recorded.

Before B2:
- [ ] C1[7] census row current;
- [ ] prior sealing packages verified;
- [ ] B1 outcome reflected in DAG.

Before Part 1 close:
- [ ] all task logs/handoffs complete;
- [ ] census reranked;
- [ ] claims truthful;
- [ ] docs index green;
- [ ] build green;
- [ ] verify-fast green;
- [ ] Part 2 queue generated from current evidence.

**End of Wave 10 Part 1 implementation-unblocker plan.**

# APPENDIX AR — FINAL AUDIT BEFORE HANDOFF

This final audit is deliberately redundant with no earlier checklist: it verifies that the **state transitions of the planning corpus itself** are correct after all Part 1 edits.

## AR.1 Corpus completeness audit

Re-enumerate `C-integration-plans/` after B2 and compare with the A1 inventory.

Required:
- no newly added plan file omitted;
- no renamed/moved file still referenced by stale census path;
- no duplicate row;
- exact count recorded.

If corpus changed during Part 1, refresh affected packets and DAG.

## AR.2 Status consistency audit

For every plan marked SEALED/SEALED-ELSEWHERE:
- evidence link resolves;
- acceptance still passes or remains covered by current gate;
- no open remainder exists.

For PARTIALLY-SEALED:
- remainder nonempty;
- dependencies explicit.

For STALE/SUPERSEDED:
- banner/disposition present where policy requires.

## AR.3 DAG consistency audit

After B1/B2:
- remove/resolve closed dependency blockers;
- recompute ready nodes;
- verify queue rank;
- ensure no plan remains dependency-blocked on a plan now sealed.

## AR.4 Claim consistency audit

Re-read claim table at close:
- B1/B2 claims handed off;
- stale economy claim terminal;
- no newly stale temporary claim;
- active Plan 24 or successor state reflects actual repository state.

## AR.5 Micro-gap consistency audit

Every A3 inventory item must appear in exactly one terminal/routed class.

No item may exist only in narrative handoff prose.

## AR.6 Plan 27 acceptance audit

Review implementation log against source plan:
- 27A complete;
- 27B complete for current scoped targets;
- 27C complete;
- mandatory order respected;
- prerequisite evidence present;
- no flaky journey;
- fidelity metric/evidence present.

If scope was reduced because of claim/dependency, plan remains PARTIALLY-SEALED.

## AR.7 C1[7] acceptance audit

Review:
- clause matrix complete;
- sealed-elsewhere evidence;
- executed remainder;
- governance checks;
- no duplicated source-of-truth artifact.

## AR.8 Documentation integrity

Run:
- docs index check;
- broken-link/reference checks if repository provides them;
- generated census/DAG check if implemented.

## AR.9 Production-diff audit

A1/A2 should have no production code changes.
A3 production changes only for explicitly bounded code-gap items.
B1 primarily test/fixture changes unless a real bug was separately repaired.
B2 expected docs/governance/checks unless source plan’s verified remainder says otherwise.

Unexpected production diff requires explanation or removal.

## AR.10 Final handoff oracle

A new agent should be able to answer without broad rediscovery:

1. Exactly how many corpus plans exist?
2. Which are fully sealed?
3. Which are partially sealed?
4. Which are stale/superseded?
5. Which are ready?
6. Which are dependency-blocked?
7. Which are claim-blocked?
8. What is the next chain head?
9. Why is it next?
10. What paths can be claimed?
11. What acceptance closes it?

If these answers are not directly available from the census/closeout, Part 1 is not done.

---

# APPENDIX AS — PART 2 PACKAGE SKELETON

For the highest-ranked ready plan after Part 1:

## Identity
**Corpus key:**<br>
**Title:**<br>
**Source baseline:**<br>

## Current premise
**Historical premise:**<br>
**Current evidence:**<br>
**Changed assumptions:**<br>

## Dependencies
| Dependency | Status | Evidence |
|---|---|---|
|  |  |  |

## Current remainder
Clause-level, executable.

## Authority
**Writer:**<br>
**Consumers:**<br>
**Save owner:**<br>
**UI owner:**<br>
**Generators:**<br>

## Claims
Exact paths.

## Phases
Use the source plan’s mandatory order.

## Acceptance
- focused tests;
- save/replay;
- data/integrity;
- UI;
- build;
- verify-fast.

## Stop conditions
- stale premise;
- missing authority;
- active claim;
- unsigned decision;
- dependency not terminal.

This skeleton is the only acceptable input form for Wave 10 Part 2 package generation.

---

# APPENDIX AT — CENSUS CHANGELOG FORMAT

Every census update after a package records:

**Date/commit:**<br>
**Package closed:**<br>
**Previous status:**<br>
**New status:**<br>
**Dependencies released:**<br>
**Queue changes:**<br>
**New blockers discovered:**<br>
**Stale premises corrected:**<br>
**Claims changed:**<br>

This prevents queue churn from becoming opaque.

---

# APPENDIX AU — MAINTENANCE OF THE CORPUS AFTER WAVE 10

Once the corpus is reconciled:

- new plan files must declare dependencies explicitly;
- implementation logs should be created when a corpus plan executes;
- completion must update census/ledger/claim state;
- superseded plans get banners;
- no active claim remains after accepted handoff;
- future waves consume the census rather than rediscovering the corpus.

The census becomes a maintained planning artifact, not a one-time audit dump.

# APPENDIX AV — RELEASE NOTE REQUIREMENT

The Part 1 closeout must include one concise release note stating:

- exact corpus file count discovered;
- count by final status;
- stale claims closed;
- micro-deferrals drained/routed;
- Plan 27 prerequisite disposition;
- Plan 27 final status;
- C1[7] final status;
- next ready DAG node;
- any remaining claim/decision/dependency blocker.

The release note is not a substitute for evidence tables. Its purpose is to provide the foreman and next implementation agent with a compact state summary that cannot drift from the census.

Before publishing it, compare every count and next-node statement against the latest census commit. If any value was copied from the initial Wave 10 ledger rather than recomputed after B1/B2, correct it.

**Final invariant:** the next agent must never need to infer whether a plan is “probably done.” Every corpus node must be terminal, explicitly blocked, or queued with a current executable remainder.
