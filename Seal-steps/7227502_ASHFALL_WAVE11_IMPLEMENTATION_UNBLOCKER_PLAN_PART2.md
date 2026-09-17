# ASHFALL — GENERATION WAVE 11 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 2

**Document role:** execution-grade Wave 11 Part 2 continuation, derived from the verified second-half Wave 11 ledger.

**Part 2 task set:**
1. **B3 — C2[12] “Difficulty Authority, Immutable Completion Record, Chronicle/Epilogue Projection”**
2. **B4 — C2[13] “Port Contracts, Host Wiring Validation, Unbound-Effect Closure”**
3. **B5 — C2[14] “Goods Must Arrive” cross-corpus duplicate reconciliation**
4. **C1 — Standing Decision Register recurrence**
5. **C2 — Census refresh and queue maintenance**

**Wave 11 Part 2 purpose:** finish the current chain segment without duplicating already-sealed work, close governance ambiguity that can cause double implementation, drain standing decisions, and hand Wave 12 a refreshed, evidence-backed queue.

**Standing cadence:**<br>
**read plan → re-verify premise → consume census row → resolve dependencies/claims → execute only the remainder → write implementation log → update ledger/census → rerank queue**

---

# 0. GLOBAL EXECUTION CONTRACT

## 0.1 Allowed terminal states

Every task/sub-scope ends as one of:

- **SEALED** — current contract fully satisfied.
- **SEALED-ELSEWHERE** — fully satisfied by prior work, with evidence.
- **PARTIALLY-SEALED** — exact remaining delta is named.
- **DEPENDENCY-BLOCKED** — hard prerequisite remains open.
- **CLAIM-BLOCKED** — required paths are actively owned.
- **DECISION-BLOCKED** — product/governance choice requires foreman verdict.
- **SUPERSEDED** — newer authoritative contract replaces the historical one.
- **STALE-PREMISE** — plan premise no longer describes current source.
- **RETIRED** — explicitly no longer required.
- **ROUTED-REPAIR** — execution reveals a genuine production regression requiring a dedicated repair package.
- **RECONCILED-DUPLICATE** — duplicate corpus row resolved to one authority or unique remainder.

No task may finish with “probably done”, “waiting someday”, “future”, or an unclassified duplicate.

## 0.2 Hard rules

1. Claims before edits.
2. Re-verify each premise at current `HEAD`.
3. Consume the current corpus census row.
4. Follow declared plan order.
5. Earlier sealed work is credited, not reimplemented.
6. A duplicate plan must be reconciled before any duplicate-scoped execution.
7. No second ending authority.
8. No campaign-save coupling for cross-campaign completion history unless the plan explicitly requires it.
9. No provider rewrite in C2[13]; wiring/documentation/validation only unless a real regression is found.
10. Every unbound provider ends as bound, explicitly retired, or explicitly allowed with documented reason.
11. Decision execution stops at signature.
12. Deferred decisions require named conditions and recheck triggers.
13. Census refresh is documentation/queue truth only; zero production change.
14. Focused verification first.
15. Generated docs/indexes are updated through their source process.
16. Implementation logs are mandatory for newly executed corpus work.
17. Census and integration ledger must agree after each package.
18. No Wave 12 plan is generated from stale pre-refresh ordering.
19. No silent plan duplication across C1/C2 corpora.
20. If the queue/register/quarantine drains reach zero, transition to maintenance instead of inventing work.

## 0.3 Required evidence bundle

Each task handoff includes:

- current `HEAD`;
- starting census row;
- plan file(s);
- dependency state;
- claim state;
- clause/remainder matrix;
- sealed-elsewhere evidence;
- files changed;
- authority map;
- save/persistence impact;
- determinism impact;
- focused test commands/results;
- owning suite/selftest result;
- build;
- docs/generator checks;
- implementation log;
- census update;
- integration ledger update;
- final terminal state;
- remaining blocker;
- queue movement.

## 0.4 Abort conditions

Stop if:
- completion record semantics require an unresolved cross-campaign policy not defined by C2[12];
- current epilogue authority differs materially from historical assumptions;
- a provider appears “unbound” only because grep missed current host wiring;
- a provider’s intended host binding is unclear;
- duplicate plan files contain nontrivial divergent scope and authority cannot be determined;
- a signed decision memo’s premise is stale;
- census refresh discovers a production regression that would require code changes;
- a queue-head plan is not actually ready after dependency recheck.

---

# 1. ORDERING AND DEPENDENCIES

## 1.1 Recommended execution order

**B3 → B4 → B5 → C1 → C2**

Rationale:
- B3 closes the current C2[12] chain remainder.
- B4 closes the systematic unbound-provider governance gap.
- B5 reconciles duplicate corpus identity before future queue advancement.
- C1 drains current decision seams after all Wave 11 tasks have had a chance to route new decisions.
- C2 runs last because it must ingest every Wave 10–11 outcome and produce the authoritative Wave 12 queue.

## 1.2 Conditional order

B5 may execute earlier because it is docs/governance-only and prevents accidental double work.

C1 should occur after B3/B4/B5 if any of them route new memos.

C2 must be last.

---

# 2. TASK B3 — C2[12] IMMUTABLE CAMPAIGN COMPLETION RECORD

## 2.1 Objective

Implement the verified-open remainder of C2[12]: an immutable, append-only, tamper-evident campaign completion record derived from the existing epilogue/ending authority and persisted at user level rather than inside an individual campaign save.

The task must preserve:
- epilogue as ending authority;
- difficulty ownership as already assigned elsewhere;
- cross-campaign completion history;
- deterministic derivation;
- observation-only recording;
- compatibility with potential future NG+ without implementing NG+.

## 2.2 Entry gate

Before edits:

- read `C2_planintegration[12].md` fully;
- consume census row;
- verify 19A epilogue projection is still sealed;
- verify `EpilogueContextFactory` or current ending-context owner;
- verify Wave 6 B2 difficulty ownership/status;
- read Wave 8 C3 disposition for Plan 175 / NG+;
- inspect current settings/user-level storage pattern;
- inspect SaveChecksum/checksum discipline;
- inspect archive chronicle/completion UI surfaces.

If completion record already appeared concurrently:
- compare contract and close only missing delta.

## 2.3 Authority boundary

### Epilogue/ending authority owns
- ending reached;
- branch/context;
- ending projection;
- outcome semantics.

### Completion recorder owns
- immutable historical record of a completed campaign;
- append-only persistence;
- tamper-evidence;
- record retrieval.

### Recorder does not own
- ending selection;
- difficulty calculation;
- NG+ unlock logic unless later consumer reads it;
- campaign save state.

## 2.4 Record identity

Define stable campaign completion identity using existing stable fields.

Potential identity sources:
- campaign run ID;
- completion timestamp only if user-level non-deterministic metadata is explicitly allowed;
- ending record ID;
- deterministic completion key.

Do not use mutable display name as sole identity.

If no stable campaign-run ID exists:
- read plan/current save identity conventions;
- do not invent an unreliable hash from arbitrary fields without review.

## 2.5 Record schema

Only plan-required fields.

Likely categories:
- record ID;
- ending/epilogue branch ID;
- completion context summary or canonical stable references;
- difficulty/tier identifier if required;
- key milestones;
- campaign duration/day;
- record integrity checksum/hash linkage;
- previous record hash if hash-chained.

Do not duplicate every campaign save field.

## 2.6 Derivation source

Record derives from deterministic terminal state and `EpilogueContextFactory` output/current equivalent.

Required invariant:
- same campaign terminal state → same gameplay-semantic completion data;
- recorder does not recompute ending independently;
- branch/context parity test.

## 2.7 Append-only semantics

No public/update path mutates prior records.

Allowed operations:
- append new valid completion;
- read/list;
- validate chain/checksum;
- optional migration from old user-store schema.

No edit/delete API unless plan explicitly defines administrative repair.

## 2.8 Tamper-evidence

Use plan/current checksum discipline.

Options only if contract supports:
- per-record checksum;
- hash chain: record N includes hash of N-1;
- store checksum envelope.

The goal is tamper-evident, not cryptographic security against a malicious user unless plan says so.

No false security claims.

## 2.9 User-level store

Use settings-store family/current user-level persistence.

Properties:
- survives campaign deletion/new campaign;
- independent of campaign save;
- empty default for old installation;
- atomic write behavior according to store convention;
- corruption behavior defined.

## 2.10 Cross-campaign persistence

Test:
1. complete campaign A;
2. append record;
3. create/load different campaign B;
4. completion history still contains A;
5. campaign B save does not own/remove A.

## 2.11 Duplicate recording guard

Completion event may be observed more than once due UI/reload.

Use stable completion identity/idempotence rule:
- same completed run cannot append duplicate record;
- separate campaigns with same ending can each append.

Do not rely on UI opening once.

## 2.12 NG+ interaction

Read Plan 175 disposition.

### If Plan 175 promoted
- completion record may expose read API suitable as unlock basis;
- do not implement NG+ state transition here.

### If retired/held
- record remains standalone historical proof.

No dependency on speculative future mode.

## 2.13 Chronicle / epilogue consumers

Current chronicle and ending surface may read completion history if C2[12] requires.

Presentation:
- read-only;
- stable ordering;
- no record mutation;
- no ending recomputation.

No new panel unless plan names one.

## 2.14 Difficulty field

If record stores difficulty:
- read from canonical difficulty authority;
- immutable at completion;
- no new difficulty logic.

If Wave 6 B2 is not landed and field required:
- dependency-block exact field/package rather than invent tier semantics.

## 2.15 Record ordering

Use deterministic/stable ordering.

If wall-clock metadata exists:
- display ordering may use it only if plan permits;
- game-semantic identity must not rely solely on wall clock.

Prefer completion ordinal or append order in user store.

## 2.16 Integrity validation

On load:
- verify checksum/hash chain;
- corrupted record behavior follows plan/current store policy;
- do not silently rewrite corrupted data as valid.

Possible outcomes:
- reject corrupted tail;
- report invalid history;
- preserve prior valid prefix.

Use exact plan.

## 2.17 Migration

Old install:
- no history → empty.

Older completion-store version:
- migrate if such version exists.

No campaign-save migration unless prior implementation already coupled it.

## 2.18 Observation-only determinism

Recording should not change simulation fingerprint.

Test:
- terminal state without recording;
- equivalent terminal state with recorder observing;
- gameplay fingerprint equal;
- user-level history differs only outside campaign simulation.

## 2.19 Focused tests

Minimum:
1. derive record from epilogue context;
2. append first record;
3. append second campaign;
4. no mutation API;
5. duplicate same-run prevented;
6. same ending/different run allowed;
7. checksum valid;
8. tamper invalid;
9. cross-campaign persistence;
10. old install empty;
11. recorder does not change campaign fingerprint;
12. difficulty context matches authority if included.

## 2.20 Endgame suite

Run current endgame/epilogue suite; historical 84/84 is not hard-coded.

No existing ending branch may change.

## 2.21 Implementation log centerpiece

`C2[12] sub-plan | Existing sealed work | Open record requirement | Implemented owner | Store | Tamper proof | Test | Final`.

## 2.22 Census / ledger

After acceptance:
- C2[12] -> SEALED if no remainder;
- release dependents in DAG;
- update integration ledger.

## 2.23 Verification order

1. record schema/derivation tests;
2. user-store tests;
3. checksum/tamper tests;
4. cross-campaign tests;
5. endgame/epilogue suite;
6. chronicle UI if touched;
7. fingerprint observation-only test;
8. build;
9. verify-fast;
10. integrity.

## 2.24 Non-goals

- no second ending authority;
- no NG+ implementation;
- no campaign-save section;
- no difficulty redesign;
- no strong anti-cheat security claims;
- no edit/delete completion UI unless plan requires.

---

# 3. TASK B4 — C2[13] PORT CONTRACTS / HOST WIRING / UNBOUND-EFFECT CLOSURE

## 3.1 Objective

Perform the corpus-wide systematic closure sweep that earlier waves never performed: every optional provider / “unbound means legacy-neutral” effect must have an explicit terminal wiring decision.

Every site becomes:
- **BOUND** — host wiring exists and is tested;
- **INTENTIONALLY-UNBOUND** — documented neutral behavior with reason;
- **RETIRED** — provider/effect intentionally removed;
- **ROUTED-REPAIR** — expected binding is missing due regression.

The task also adds the cheap host-wiring validation gate required by C2[13].

## 3.2 Entry gate

Read C2[13] fully.

Extract:
- port-contract shape;
- provider semantics;
- neutral/unbound rule;
- host validation acceptance;
- standardization expectations.

Then search current source.

## 3.3 Inventory strategy

Search patterns:
- nullable/optional providers;
- `provider == null`;
- `?.Invoke`;
- default delegates;
- injected function providers;
- `Bind*`, `Set*Provider`, `Configure*`;
- legacy-neutral comments;
- unbound fallback.

The search produces candidates only.

Each candidate must be read in context.

## 3.4 Inventory schema

| Provider/effect | Core owner | Provider type | Neutral behavior | Host binding found? | Host path | Intended state | Evidence |
|---|---|---|---|---|---|---|---|

Do not mark “never bound” from grep alone.

## 3.5 Host search

For every candidate:
- search exact property/method;
- search interface/type;
- search construction root;
- search host sessions;
- search tests;
- search generated bindings.

Only after full search classify.

## 3.6 Bound classification

BOUND requires:
- actual host/composition wiring;
- intended provider supplied;
- focused wiring test or equivalent evidence.

A setter call in dead/test-only code is insufficient.

## 3.7 Intentionally-unbound classification

Valid only when:
- owning doc/plan says neutral fallback is intended;
- leaving unbound is safe;
- no expected player-facing effect is missing.

If intentional but undocumented:
- add concise owner documentation;
- inventory allow-list reason.

## 3.8 Retired classification

Use when:
- effect/provider no longer needed;
- current plan/owner confirms retirement.

Remove stale provider only if C2[13] allows and change is bounded. Otherwise document retirement and promote cleanup.

No broad provider rewrite.

## 3.9 Missing host binding

If expected binding is absent:
- locate existing host session/composition root;
- wire provider through standing pattern;
- no new service locator;
- no Core ownership change.

## 3.10 Wiring contract

Host binding should:
- use current authoritative query/command;
- be deterministic;
- preserve legacy-neutral behavior when not bound in old/test contexts;
- not create circular dependency.

## 3.11 Legacy parity

For optional provider:
- unbound fixture reproduces historical behavior;
- bound fixture exhibits intended effect.

This proves additive integration.

## 3.12 Save impact

Provider binding should usually add no save state.

If provider reads persisted owner:
- restore must naturally rebind host after composition.

Test fresh runtime restore, not serialized delegates.

## 3.13 Validation gate

Create cheap deterministic gate over explicit inventory/registry.

For every optional provider:
- state = BOUND / INTENTIONALLY_UNBOUND / RETIRED;
- reason/evidence required for unbound/retired;
- expected bound provider must have host wiring evidence.

The gate prevents ambiguous new providers.

## 3.14 Inventory as source

Prefer structured inventory near tooling/docs.

Avoid fragile prose grep as the only gate.

If code annotation/registry already exists, extend it.

## 3.15 Gate can-fail test

Synthetic/test fixture:
- add unclassified provider entry → gate fails;
- missing reason → fails;
- valid bound/unbound state → passes.

Do not modify production provider just to test gate.

## 3.16 Formal port-contract standardization

If C2[13] requires a standard:
- naming;
- neutral semantics;
- binding ownership;
- documentation fields.

Prefer docs + lint/check.

No rewrite of all existing providers into a new abstraction unless explicitly required.

## 3.17 Per-provider tests

For each newly wired provider:
- unbound legacy-neutral;
- bound behavior measurable;
- host composition supplies it;
- save/restore composition rebinds;
- no duplicate binding.

## 3.18 Domain suites

Run only touched owners.

Examples may include:
- Campaign;
- economy;
- survivor;
- world;
- UI host sessions.

Do not run full suite as substitute.

## 3.19 Closure table

Implementation log centerpiece:

`Provider | Core owner | Historical unbound behavior | Host binding | Final state | Reason | Focused test`.

## 3.20 Census / ledger

C2[13] SEALED only when no ambiguous provider remains.

If some provider requires product decision:
- PARTIALLY-SEALED / DECISION-BLOCKED exact rows.

## 3.21 Verification order

1. inventory check;
2. newly wired provider tests;
3. host composition tests;
4. validation gate can-fail;
5. touched domain suites;
6. build;
7. verify-fast;
8. new closure gate.

## 3.22 Non-goals

- no provider rewrite;
- no service locator;
- no behavior change for already-bound paths;
- no forced binding where neutral is intentional;
- no repo-wide API renaming.

---

# 4. TASK B5 — C2[14] CROSS-CORPUS “GOODS MUST ARRIVE” DUPLICATE RECONCILIATION

## 4.1 Objective

Resolve the governance hazard created by two corpus documents claiming the same “Goods Must Arrive” delivery-chain scope: C1[10] and C2[14]. Determine whether they are true duplicates, versioned siblings, or same-title distinct scopes; establish one authoritative contract; update census/ledger; and run a corpus-wide duplicate-title sweep.

Zero production code changes.

## 4.2 Entry gate

Read both:
- `C1_planintegration[10].md`;
- `C2_planintegration[14].md`.

Capture:
- title;
- source baseline;
- revision date;
- sub-plan order;
- acceptance;
- dependencies;
- non-goals;
- cited predecessor/successor;
- historical references from Wave 10 B5.

## 4.3 Diff methodology

Produce semantic diff:

`Clause | C1[10] | C2[14] | Identical? | Stronger/newer | Unique?`.

Do not rely only on textual diff because wording may differ while semantics same.

## 4.4 Duplicate classes

### TRUE DUPLICATE
Same scope and acceptance.

Action:
- designate authoritative file;
- banner non-authority SUPERSEDED-BY;
- census duplicate column;
- Wave 10 B5 remains owner of implementation.

### REVISIONED SIBLING
Same plan lineage, one has unique improvements.

Action:
- choose authority using revision evidence/current citations;
- merge unique clauses into authority only if governance policy allows;
- banner sibling;
- adjust B5 scope/log if necessary.

### SAME TITLE, DISTINCT SCOPE
Meaningfully separate contract.

Action:
- rename/disambiguate census identity;
- preserve both;
- create unique queue row for C2[14] remainder.

No duplicate implementation.

## 4.5 Authority selection rules

Consider:
1. explicit revision date;
2. richer/current contract;
3. consistency with executed logs;
4. current integration-ledger references;
5. downstream dependency references.

Do not select by corpus letter alone.

## 4.6 Historical integrity

Non-authoritative plan body remains for provenance.

Add header banner:
- status;
- authoritative replacement;
- date;
- reconciliation evidence.

Do not erase history.

## 4.7 Merge rule for unique clauses

If sibling has unique valid clause:
- copy/merge into authoritative plan with provenance note;
- do not rewrite implementation history;
- update implementation package acceptance only if clause was not already satisfied.

If unique clause is stale:
- document why not merged.

## 4.8 Wave 10 B5 scope impact

Read Wave 10 B5 implementation log/assignment.

Classify:
- no scope change;
- acceptance extension required;
- unique separate task required.

Do not automatically reopen sealed B5 unless new authoritative clause is genuinely unmet.

## 4.9 Corpus-wide duplicate sweep

Mechanically extract titles / normalized source baselines for all C1/C2 rows.

Flag:
- exact title matches;
- highly similar normalized titles;
- same source Plan IDs across corpus rows.

The script produces candidates.

Human/agent reconciliation required.

## 4.10 Duplicate inventory schema

| Corpus A | Corpus B | Title/source match | Class | Authority | Disposition | Evidence |
|---|---|---|---|---|---|---|

## 4.11 Census schema extension

Add duplicate/reconciliation column if not already present.

Every duplicate candidate:
- reconciled;
- false-positive documented.

## 4.12 Ledger update

If package scope changed:
- update Wave 10 B5 row/log pointer;
- no production changes.

## 4.13 Docs index

Regenerate/check.

## 4.14 Zero-production-diff gate

Verify `git diff` production paths empty.

Only:
- plan docs;
- census;
- ledger;
- tooling script if needed;
- docs index/generated metadata.

## 4.15 B5 closeout

Deliver:
- semantic diff;
- authority decision;
- banners;
- corpus-wide duplicate table;
- census updates;
- queue correction.

## 4.16 Non-goals

- no delivery-chain implementation;
- no product code;
- no plan-body rewrite beyond authority merge;
- no deleting historical plan files;
- no automatic dedupe by title alone.

---

# 5. TASK C1 — STANDING DECISION REGISTER RECURRENCE

## 5.1 Objective

Run the recurring Wave 10 E1 governance pass after all Wave 11 execution tasks have had a chance to route new decisions. Rebuild decision truth from source documents, obtain verdicts for silent/condition-matured items, execute bounded signed items, queue large signed items, and end with zero unsigned-without-condition decisions.

## 5.2 Sources

Rebuild from:
- decision memos;
- decision register;
- integration ledger;
- claims;
- closeouts;
- KNOWN_DEBT;
- Wave 8–11 task logs.

Do not trust last register snapshot alone.

## 5.3 Known decision families

Include current still-relevant items such as:
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
- new Wave 11 memos/proposals.

Only include items that remain decisions.

## 5.4 Register schema

| Decision ID | Topic | Memo | Owner | Current premise | Verdict | Condition | Recheck | Execution package | Evidence |
|---|---|---|---|---|---|---|---|---|---|

## 5.5 Verdict truth pass

For each:
- signed?;
- declined?;
- deferred?;
- condition matured?;
- executed already?;
- premise stale?;
- superseded?

Do not mark silent if source doc already has verdict.

## 5.6 Foreman session packet

Present concise:
- topic;
- current evidence;
- existing options;
- architecture-safe recommendation if memo already contains one;
- blast radius;
- consequence of no decision.

No new memo authoring unless current premise invalidates old memo and refresh is unavoidable.

## 5.7 Signed bounded execution

Inline only if:
- existing memo exact;
- bounded paths;
- no broad migration;
- claims clear;
- focused verification known.

Each execution follows its memo-specific gates.

## 5.8 Signed large execution

Create/activate package:
- exact scope;
- dependencies;
- claims;
- acceptance;
- queue rank.

Do not partially execute large decision inline.

## 5.9 Declined

Record:
- verdict;
- reason;
- date;
- close blocker in all source-of-truth docs.

No lingering “awaiting”.

## 5.10 Deferred with condition

Must include:
- named condition;
- recheck trigger;
- owner;
- optional target wave/release if governance uses.

“Later” invalid.

## 5.11 Premise-stale memo

If old memo is no longer applicable:
- mark stale/superseded;
- do not ask foreman to choose obsolete options.

If decision still needed with changed facts:
- refresh memo explicitly and record revision.

## 5.12 Cross-doc consistency

Check:
- memo;
- register;
- debt;
- closeout;
- integration ledger;
- census if plan blocked.

No contradictory verdicts.

## 5.13 Success metric

Zero:
- UNSIGNED with no session/owner;
- DEFERRED with no condition;
- SIGNED with no execution/queue disposition;
- DECLINED still listed as open.

## 5.14 Verification

For inline executions:
- memo gates;
- touched suite;
- build;
- verify-fast.

For governance-only:
- docs index;
- consistency check.

## 5.15 Pass record

Append Wave 11 pass:
- decisions reviewed;
- verdicts;
- executions;
- packages;
- deferrals;
- stale/superseded memos.

## 5.16 Non-goals

- no unsigned execution;
- no new product decision by implementer;
- no duplicate detailed memos;
- no indefinite deferral.

---

# 6. TASK C2 — CENSUS REFRESH & QUEUE MAINTENANCE

## 6.1 Objective

Run the final Wave 11 actuality heartbeat: re-verify all Wave 10–11 executed rows, ingest execution-surfaced blockers, rerun duplicate reconciliation, refresh the DAG, measure the three standing drains, pre-verify the Wave 12 queue head, and leave the census as the authoritative next-step source.

Zero production changes.

## 6.2 Input set

Read:
- current corpus census;
- every Wave 10 implementation log;
- every Wave 11 implementation log;
- current claim table;
- decision register;
- quarantine manifest/count;
- duplicate inventory;
- integration ledger.

## 6.3 Re-verify executed rows

For each claimed SEALED row:
- log exists;
- current source still supports;
- focused acceptance still representative;
- no regression note invalidates status.

Spot-check focused gates where cheap.

If status no longer true:
- reopen honestly;
- route regression.

## 6.4 Execution-surfaced blocker collection

Each implementation log may contain:
- divergence;
- deferred surface;
- new missing consumer;
- active claim;
- decision;
- broader migration;
- data/content gap;
- regression.

Collect all.

No blocker may remain buried only in prose.

## 6.5 Blocker classification

`Blocker | Origin log | Type | Owner | Size | Dependency | Decision | Queue action`.

Types:
- MICRO;
- REPAIR;
- PLAN-REMAINDER;
- DECISION;
- CLAIM;
- GOVERNANCE;
- CONTENT;
- MIGRATION.

## 6.6 Small closure rule

Inline within C2 only if:
- documentation/census correction;
- zero production change.

Source says C2 is zero production change.

Any actual code fix becomes a package, not inline.

## 6.7 Large/new blocker

Add queue row:
- exact scope;
- DAG dependencies;
- source log;
- proposed acceptance.

Do not silently attach to next numeric plan.

## 6.8 Duplicate sweep

Run B5 duplicate script against current corpus.

Reconcile any new candidates using same procedure.

Update duplicate column.

## 6.9 DAG refresh

Recalculate:
- sealed prerequisites;
- ready dependents;
- decision blockers;
- claim blockers;
- duplicate blockers.

Queue head changes according to readiness.

## 6.10 Wave 12 head premise-check

Before handoff:
- read top ready plan;
- verify source premise;
- verify owner;
- verify dependencies;
- verify claims.

Wave 12 receives pre-verified head.

## 6.11 Three-drain measurements

Measure:

### Queue drain
Number of remaining actionable plan rows.

### Decision drain
Number of open decisions:
- unsigned;
- deferred-with-condition not matured;
- signed queued.

Report categories.

### Quarantine drain
Remaining quarantined test files.

Do not fabricate zero by retiring unresolved work.

## 6.12 Standing count statement

Publish:

`Queue remaining: N`
`Decision register open: M`
`Quarantine remaining: K`

Include definitions so counts are reproducible.

## 6.13 Census updates

For every Wave 11 row:
- status;
- evidence;
- remainder;
- dependency release;
- duplicate status.

## 6.14 Ledger synchronization

Integration ledger reflects:
- terminal packages;
- newly queued packages;
- active claims.

No disagreement.

## 6.15 Docs index

Regenerate/check.

## 6.16 Zero-production-change gate

Diff must contain no production changes from C2 refresh package.

## 6.17 Wave 12 verdict

If any drain remains:
- Wave 12 exists, with queue head.

If all three drains zero:
- convert to maintenance cadence.

Do not continue series by inertia.

## 6.18 C2 closeout artifact

Create/append:
- refreshed census;
- blocker-generation table;
- duplicate table;
- DAG snapshot;
- three-drain counts;
- pre-verified Wave 12 head.

## 6.19 Non-goals

- no execution;
- no plan rewrite;
- no code fix;
- no stale queue inheritance;
- no arbitrary reprioritization outside DAG/evidence.

---

# 7. CROSS-TASK AUTHORITY MAP

| Task | Authority | New state allowed? | Main risk |
|---|---|---|---|
| B3 | epilogue + user completion store | append-only completion history | second ending authority/campaign coupling |
| B4 | existing provider owners + host composition | inventory metadata only | false unbound classification/provider rewrite |
| B5 | corpus governance | no production state | duplicate implementation |
| C1 | decision register + source memos | governance records | unsigned execution/stale decisions |
| C2 | corpus census/ledger | docs/queue state | stale queue |

---

# 8. SAVE / PERSISTENCE POLICY

## B3
Cross-campaign completion history in user-level store, not campaign save.

## B4
No serialized provider delegates. Rebind during composition.

## B5
No runtime save impact.

## C1
Only memo-specific executions may affect save, under their own contract.

## C2
No runtime save impact.

---

# 9. DETERMINISM POLICY

## B3
Record derives from deterministic epilogue context; observation does not alter simulation.

## B4
Host bindings must not introduce wall-clock/random behavior absent owner contract.

## B5
Docs-only.

## C1
Per memo.

## C2
Docs-only.

---

# 10. IMPLEMENTATION LOG STANDARD

For B3/B4 and any inline C1 execution:

1. plan/decision identity;
2. starting census/register state;
3. dependencies;
4. premise;
5. sealed-elsewhere evidence;
6. executed delta;
7. claims;
8. files;
9. save/determinism;
10. tests;
11. broader gates;
12. terminal state;
13. census/register update;
14. remaining blocker;
15. next queue movement.

---

# 11. COMMIT BOUNDARIES

## B3
1. premise/schema/store design;
2. record derivation + append;
3. checksum/tamper;
4. consumers;
5. tests/log/census.

## B4
1. inventory;
2. bounded wiring per provider/domain;
3. intentionally-unbound documentation;
4. closure gate;
5. log/census.

## B5
1. semantic diff;
2. authority reconciliation;
3. corpus-wide duplicate sweep;
4. banners/census/ledger.

## C1
1. register rebuild;
2. verdict update;
3. bounded executions individually;
4. package routing;
5. pass record.

## C2
1. execution-log blocker collection;
2. duplicate/DAG refresh;
3. counts/head verification;
4. census/ledger/docs index.

---

# 12. ROLLBACK / ROUTING MATRIX

| Task | Finding | Action |
|---|---|---|
| B3 | completion data not derivable from ending context | dependency/design review |
| B3 | user store lacks safe append semantics | extend existing store pattern, not campaign save |
| B3 | tamper check changes ending outcome | rollback; recorder must observe only |
| B4 | provider actually bound under different name | mark BOUND, no code |
| B4 | intended host unknown | decision/owner review |
| B4 | provider binding exposes regression | repair package |
| B5 | files true duplicate | supersede one |
| B5 | unique valid clauses | merge/re-scope |
| C1 | memo premise stale | refresh/mark stale before verdict |
| C1 | signed work large | queue package |
| C2 | sealed row regressed | reopen/repair package |
| C2 | production fix appears “small” | queue package; C2 remains docs-only |

---

# 13. MASTER ACCEPTANCE MATRIX

| Task | Blocker | Mandatory proof |
|---|---|---|
| B3 | immutable completion record missing | append-only cross-campaign store, tamper proof, epilogue parity, observation-only |
| B4 | ambiguous unbound effects | every provider classified, missing bindings wired, closure gate green |
| B5 | duplicate plan scope | authority/reconciliation table, duplicate sweep, zero production diff |
| C1 | recurring decisions | zero unsigned-without-condition, signed work executed/queued |
| C2 | aging queue truth | rows refreshed, blockers routed, DAG reranked, N/M/K counts, Wave 12 head verified |

---

# 14. PART 2 CLOSEOUT

Before Wave 11 closes:

1. C2[12] sub-plan matrix terminal.
2. Completion record derives from ending authority.
3. Completion record persists cross-campaign at user level.
4. Append-only/tamper evidence tests green.
5. C2[13] inventory covers every optional provider pattern in scope.
6. Every provider is bound/unbound-documented/retired/repair-routed.
7. Host-wiring closure gate green.
8. C2[14] duplicate reconciled.
9. Corpus-wide duplicate sweep complete.
10. Decision register rebuilt from source docs.
11. Silent/condition-matured decisions reviewed.
12. Zero unsigned-without-condition.
13. Signed bounded work executed.
14. Signed large work queued.
15. Census refresh includes all Wave 10–11 logs.
16. Surfaced blockers routed.
17. DAG refreshed.
18. Three-drain counts measured.
19. Wave 12 head premise-checked.
20. Zero production changes in B5/C2 governance packages.
21. Build green where production work occurred.
22. Verify-fast green.
23. Docs-index check green.
24. Final Wave 11 release note issued.

---

# 15. PART 2 NON-GOALS

- no second ending authority;
- no NG+ implementation;
- no campaign-save completion ledger;
- no provider abstraction rewrite;
- no broad host refactor;
- no duplicate delivery-chain execution;
- no unsigned decision execution;
- no production changes during census refresh;
- no Wave 12 scheduling by numeric sequence alone.




# APPENDIX A — B3 COMPLETION RECORD CONTROL SHEETS

## A.1 Sub-plan reconciliation

| C2[12] axis | Existing state | Sealed by | Remainder | B3 action |
|---|---|---|---|---|
| difficulty authority |  | Wave 6 B2 / current |  |  |
| epilogue projection |  | 19A |  | none |
| chronicle |  |  |  |  |
| immutable completion record | absent/open |  | full remainder | implement |

## A.2 Record field review

| Field | Source owner | Stable? | Needed? | Persisted | Tamper-covered |
|---|---|---|---|---|---|
| run ID |  |  |  |  |  |
| ending ID |  |  |  |  |  |
| milestones |  |  |  |  |  |
| difficulty |  |  |  |  |  |
| day/duration |  |  |  |  |  |
| prior hash/checksum |  |  |  |  |  |

No field without consumer/contract value.

## A.3 Append-only API review
Allowed:
- AppendCompletion;
- ReadAll/Query;
- Validate.

Flag if any:
- Update;
- Replace;
- Delete;
unless plan explicitly allows administrative repair.

## A.4 Duplicate guard

| Scenario | Same run ID? | Same ending? | Append? |
|---|---|---|---|
| same completion observed twice | yes | yes | no |
| different campaigns same ending | no | yes | yes |
| different campaigns different ending | no | no | yes |

Use current identity model.

## A.5 Tamper chain

For hash chain:
- genesis;
- record hash input;
- previous hash;
- verification.

Do not include unstable serialization order.

Canonical serialization required.

## A.6 Corruption tests
- mutate ending ID;
- mutate milestone;
- break prior hash;
- truncate tail;
- corrupt first record.

Expected behavior from plan/store policy.

## A.7 User-store atomicity
Review current settings-store writing:
- temp + replace?;
- serialization?;
- error handling?;
- backup?.

Reuse.

## A.8 Observation-only proof
Record before/after gameplay fingerprint.
User-store write may occur outside campaign fingerprint.

## A.9 Chronicle consumer
If required:
- stable list;
- no mutation;
- invalid record flagged/omitted per policy;
- no ending recompute.

## A.10 NG+ future seam
Expose read API only if useful.
No unlock implementation.

---

# APPENDIX B — B3 COMPLETION RECORD TEST MATRIX

## B.1 Creation
- [ ] terminal ending triggers append once;
- [ ] context parity with epilogue;
- [ ] difficulty field correct if included.

## B.2 Persistence
- [ ] app restart/store reload;
- [ ] new campaign retains history;
- [ ] campaign save deletion does not remove history.

## B.3 Immutability
- [ ] no mutation command;
- [ ] returned records immutable/read-only according to language pattern.

## B.4 Integrity
- [ ] valid checksum;
- [ ] tamper detected;
- [ ] chain order valid.

## B.5 Idempotence
- [ ] same completion observed twice → one record.

## B.6 Determinism
- [ ] semantic record equal from equivalent terminal state;
- [ ] campaign fingerprint unaffected by recorder.

## B.7 Old environment
- [ ] no file/store → empty history;
- [ ] malformed/corrupt store handled per policy.

---

# APPENDIX C — B4 UNBOUND-PROVIDER INVENTORY PLAYBOOK

## C.1 Search categories

Search code for:
- nullable delegates;
- nullable interfaces;
- provider properties;
- setter injection;
- no-op fallbacks;
- “legacy” comments;
- `if (x is null)`;
- default function returns;
- optional query callbacks.

## C.2 False positives

Exclude after inspection:
- optional diagnostics;
- test hooks;
- truly optional cosmetic providers;
- internal nullable cache;
- lifecycle-null-before-bind with guaranteed bind later.

Document why excluded if ambiguous.

## C.3 Candidate worksheet

**Provider:**<br>
**Core owner:**<br>
**Declared neutral behavior:**<br>
**Intended external authority:**<br>
**Search for binding:**<br>
**Actual binding:**<br>
**Production path reachable?:**<br>
**Final classification:**<br>
**Evidence:**<br>

## C.4 Bound criteria

Bound means:
- production host supplies real provider;
- provider returns authoritative value;
- wiring occurs on every intended composition path.

A dev/test-only binding does not count.

## C.5 Multi-host providers

If multiple hosts:
- verify all required host variants;
- one missing host keeps row partial.

## C.6 Restore host

Fresh runtime restore must recreate bindings through composition.

No delegate persistence.

## C.7 Intentionally-unbound documentation

Must state:
- why optional;
- neutral fallback;
- expected hosts;
- future trigger if any.

No vague “not wired yet” in final state.

## C.8 Retired provider

If no consumer/need:
- mark retired;
- cleanup only if bounded and plan permits;
- otherwise queue cleanup.

---

# APPENDIX D — B4 HOST-WIRING VALIDATION GATE

## D.1 Structured inventory row

`Provider ID | Core path | Expected state | Host path | Reason | Test reference`.

## D.2 Gate checks
- provider exists;
- classification present;
- BOUND has host path;
- UNBOUND has reason;
- RETIRED has evidence;
- no duplicate provider IDs;
- stale row for removed provider fails.

## D.3 Can-fail fixtures
- provider absent classification;
- bound without host;
- unbound without reason;
- stale inventory entry.

## D.4 Gate location
Integrate with standing parity/tooling gates per plan.

Do not add expensive runtime reflection if static inventory suffices.

## D.5 Developer workflow
When adding provider:
1. add provider;
2. decide binding;
3. add host wiring or reason;
4. update inventory;
5. run gate.

---

# APPENDIX E — B5 DUPLICATE RECONCILIATION CONTROL SHEETS

## E.1 Semantic diff

| Contract area | C1[10] | C2[14] | Difference | Authority decision |
|---|---|---|---|---|
| dependencies |  |  |  |  |
| 35A |  |  |  |  |
| 36A |  |  |  |  |
| 35B |  |  |  |  |
| 35C |  |  |  |  |
| acceptance |  |  |  |  |
| non-goals |  |  |  |  |

## E.2 Authority evidence

Record:
- revision dates;
- downstream citations;
- Wave 10 B5 references;
- richer/current contract.

## E.3 Banner template

> **STATUS (YYYY-MM-DD): SUPERSEDED-BY `<authoritative plan>`**<br>
> Reconciled by Wave 11 B5. Historical body retained for provenance.<br>
> See `<reconciliation artifact>`.

## E.4 Corpus duplicate script output

`Normalized title | Source IDs | Corpus files | Candidate type`.

Do not auto-edit from script.

## E.5 False positive examples
Similar title but different:
- phase;
- scope;
- target owner.

Document as NOT-DUPLICATE.

## E.6 Queue correction
If true duplicate:
- remove one executable queue node;
- dependency references point to authority.

If unique remainder:
- keep re-scoped node.

---

# APPENDIX F — C1 DECISION REGISTER REBUILD PACKET

## F.1 Source discovery

For every memo:
- title/ID;
- last revision;
- owner;
- current verdict;
- implementation state.

## F.2 Register states

### SIGNED-PENDING-EXECUTION
Valid verdict; implementation not done.

### SIGNED-EXECUTED
Terminal if acceptance green.

### DECLINED
Terminal.

### DEFERRED-WITH-CONDITION
Open but non-ambiguous.

### UNSIGNED
Needs session.

### STALE
Memo premise invalid.

### SUPERSEDED
Newer decision replaces.

## F.3 Condition maturity

For each deferred:
- parse condition;
- check current repository state;
- if condition met → return to foreman session;
- if not → remain deferred.

## F.4 One-session order

Prioritize:
1. condition-matured;
2. blockers at current DAG head;
3. small high-unlock decisions;
4. lower-priority future decisions.

Do not bury chain-head blockers.

## F.5 Signed bounded execution checklist
- memo exact;
- claims clear;
- dependencies clear;
- one package;
- focused gate;
- update sources after.

## F.6 Signed large package checklist
- package ID;
- paths;
- dependencies;
- acceptance;
- queue rank;
- owner.

## F.7 Decline closure checklist
- reason;
- memo;
- register;
- debt;
- ledger;
- census if blocked plan.

---

# APPENDIX G — C2 CENSUS REFRESH CONTROL SHEETS

## G.1 Row spot-check

| Corpus | Status before | Log exists | Current source proof | Gate spot-check | Status after |
|---|---|---|---|---|---|
|  |  |  |  |  |  |

## G.2 Surfaced blocker intake

| Blocker | Source log | Owner | Type | Size | Dependency | Claim | Decision | Queue action |
|---|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

## G.3 Queue row requirements

New row must include:
- source;
- current premise;
- owner;
- deps;
- acceptance;
- rank rationale.

## G.4 DAG refresh
For each newly sealed node:
- find dependents;
- re-evaluate readiness;
- update rank.

## G.5 Three drains

### Queue
Count actionable OPEN/PARTIAL ready/blocked plan nodes according to census convention.

### Register
Count unresolved decisions by status.

### Quarantine
Count test files still excluded.

Definitions must remain stable between waves.

## G.6 Wave 12 head packet
**Corpus key:**<br>
**Title:**<br>
**Current premise:**<br>
**Dependencies:**<br>
**Claims:**<br>
**Decision:**<br>
**Remainder:**<br>
**Acceptance:**<br>
**Why highest ready node:**<br>

---

# APPENDIX H — CURRENT WAVE 12 PREVIEW RECONCILIATION

The source preview names:

1. **C1[16] Depth Passes — Pouring Dead Content Onto New Rails**
2. **C2[15] Input Reality, Focus Navigation, Controller Parity, Rebinding**
3. C1[17] presented game / Holdfast / map
4. C1[18] repository weight/hygiene
5. C2[16] session durability/release gates

These are not automatically Part 1/Part 2 tasks.

C2 census refresh must:
- verify top readiness;
- reconcile dependencies;
- check claims;
- check duplicate status;
- identify whether newly surfaced blockers outrank them.

---

# APPENDIX I — NEGATIVE-CASE REGISTRY

## I.1 B3
- same completion observed twice;
- two campaigns same ending;
- corrupt tail;
- corrupt middle record;
- user store absent;
- campaign save deleted;
- recorder invoked before terminal state;
- difficulty authority unavailable.

## I.2 B4
- provider bound under alternate symbol;
- provider test-only bound;
- provider intentionally unbound but undocumented;
- bound host missing after restore;
- inventory stale after provider removal;
- validation gate false positive.

## I.3 B5
- same title exact duplicate;
- same title revised sibling;
- same title different scope;
- duplicate source Plan ID with different title;
- downstream dependency points to non-authority.

## I.4 C1
- signed memo already executed;
- declined memo still open in debt;
- deferred condition matured;
- stale premise;
- signed large item mistakenly executed inline.

## I.5 C2
- sealed row missing log;
- log says partial but census says sealed;
- newly surfaced repair omitted;
- duplicate added after prior sweep;
- top queue plan dependency not actually sealed;
- all drains zero.

---

# APPENDIX J — FINAL WAVE 11 RELEASE NOTE TEMPLATE

# Wave 11 Closeout

## B3 — C2[12]
**Status:**<br>
**Completion-record owner:**<br>
**Store:**<br>
**Tamper model:**<br>
**Cross-campaign proof:**<br>
**Epilogue parity:**<br>

## B4 — C2[13]
**Status:**<br>
**Providers inventoried:**<br>
**Bound:**<br>
**Intentionally unbound:**<br>
**Retired:**<br>
**Repair-routed:**<br>
**Closure gate:**<br>

## B5 — C2[14]
**Duplicate class:**<br>
**Authority:**<br>
**Unique clauses:**<br>
**Corpus-wide candidates:**<br>
**Queue correction:**<br>

## Decision Register
**Signed executed:**<br>
**Signed queued:**<br>
**Declined:**<br>
**Deferred with condition:**<br>
**Unsigned:**<br>

## Census / drains
**Queue remaining N:**<br>
**Decision open M:**<br>
**Quarantine K:**<br>

## Wave 12 head
**Plan:**<br>
**Premise:**<br>
**Why ready:**<br>
**Blockers:**<br>

---

# APPENDIX K — FINAL NO-FALSE-CLOSURE RULES

Wave 11 Part 2 is not complete if:

- B3 stores completion history inside campaign save contrary to C2[12];
- B3 recomputes endings instead of deriving from epilogue authority;
- B3 allows record mutation;
- B3 calls checksumming “secure anti-cheat” without such a contract;
- B4 leaves any in-scope provider ambiguous;
- B4 calls a provider unbound based only on grep without host search;
- B4 rewrites provider APIs instead of wiring/documenting;
- B5 leaves C1[10] and C2[14] both executable for same scope;
- B5 performs production delivery work;
- C1 executes unsigned decisions;
- C1 leaves deferred items without conditions;
- C2 changes production code;
- C2 inherits the prior queue without rerunning DAG readiness;
- three-drain counts are not reproducible;
- Wave 12 is generated from the preview rather than refreshed actuality.

**Final invariant:** at Wave 11 close, completion-history ownership is explicit, every optional provider has a binding decision, duplicate plan identity is reconciled, decisions are non-ambiguous, and the next queue head is freshly verified.

# APPENDIX L — B3 COMPLETION RECORD STORAGE CONTRACT

## L.1 Storage-layer selection

Before implementing persistence, identify the current user-level store family used for settings/profile/meta information.

Record:
- owning service/type;
- file/location abstraction;
- serialization format;
- checksum/integrity mechanism;
- atomic-write behavior;
- version/migration policy;
- error-reporting path.

The completion-record store should reuse this family unless C2[12] explicitly names a different owner.

## L.2 Why campaign save is forbidden

The completion record must outlive:
- a single campaign;
- campaign deletion;
- campaign replacement;
- new-game creation.

Therefore:
- do not put history in `CampaignSave`;
- do not make a completion record disappear when a save slot is deleted;
- do not require loading the completed campaign to inspect history.

## L.3 Store schema evolution

If the user-level store already has a root DTO:
- add one additive collection/section there if contract allows.

If separate file is the standing pattern:
- use same version envelope.

Migration rules:
- missing collection => empty;
- unknown future fields follow serializer policy;
- corrupt history follows explicit validation behavior.

## L.4 Canonical serialization for checksums

Tamper-evidence fails if semantically identical records serialize differently.

Canonicalization must define:
- stable field order or canonical serializer behavior;
- stable collection order;
- stable numeric/string encoding;
- no transient/UI fields;
- no locale-formatted values.

## L.5 Hash-chain option

If C2[12] explicitly requires chained history:

`record_hash = H(previous_hash || canonical_record_payload)`

Rules:
- stable genesis value;
- previous hash immutable;
- append computes new hash;
- validation recomputes full chain;
- no mutation path.

Do not invent cryptographic algorithm requirements absent plan/current checksum policy; reuse standing checksum discipline.

## L.6 Per-record checksum option

If plan only requires tamper-evident records:
- each record checksum may suffice;
- no chain semantics invented.

Document exact integrity guarantee.

## L.7 Corruption handling decision table

| Corruption | Detection | Store behavior | UI/read behavior | Repair? |
|---|---|---|---|---|
| invalid checksum |  |  |  |  |
| truncated record |  |  |  |  |
| malformed payload |  |  |  |  |
| broken prior hash |  |  |  |  |
| unknown schema |  |  |  |  |

Use existing user-store policy where available.

## L.8 No silent healing

A corrupted record should not be silently rewritten into a valid record unless current store migration policy explicitly authorizes deterministic repair.

Validation failure must remain observable.

## L.9 Atomic append

If user-level store supports atomic whole-file replacement:
1. load and validate current history;
2. verify duplicate identity;
3. append in memory;
4. compute integrity;
5. write atomically.

No partial tail write.

## L.10 Concurrent append

If app architecture can trigger two completion observations:
- stable duplicate guard;
- one append wins/second becomes no-op;
- no duplicated same-run record.

Do not add complex locking beyond current user-store concurrency policy.

---

# APPENDIX M — B3 COMPLETION RECORD DOMAIN MODEL

## M.1 Record semantic fields

Every field must answer one of:
- identifies the campaign completion;
- records canonical ending context;
- records milestone/difficulty metadata required by C2[12];
- supports integrity;
- supports a named consumer.

Fields with no purpose are excluded.

## M.2 Ending ID

Must come from canonical ending/epilogue branch identity.

No human-readable localized title as authority.

## M.3 Milestones

Only milestones already computed by terminal/epilogue context or named by plan.

Do not re-scan arbitrary campaign history inside the recorder if `EpilogueContextFactory` already provides them.

## M.4 Difficulty

If stored:
- canonical stable difficulty ID;
- not display string;
- captured at completion.

## M.5 Campaign duration

Use canonical campaign day count.

No wall-clock “hours played” unless explicitly part of plan.

## M.6 Optional real-world timestamp

Only if user-level history UI requires and current contract permits.

It must remain metadata, not gameplay-semantic identity.

## M.7 Record version

Include if standing user-store migration policy requires.

## M.8 Immutable type shape

Use repository idiom:
- immutable record/struct/read-only fields;
- collection snapshots copied;
- no mutable reference leak.

Tests should prove returned collections cannot mutate stored history through aliasing.

---

# APPENDIX N — B3 ENDING / RECORD PARITY TESTS

## N.1 Branch parity

For every representative ending branch:
- build deterministic terminal context;
- run epilogue projection;
- create completion record;
- assert ending ID and context fields match exactly.

Do not require 32 separate tests if existing parameterized epilogue suite can supply all branches efficiently; preserve total branch coverage.

## N.2 Milestone parity

For key milestone sets:
- record contains exactly the plan-required milestone identity;
- no omitted named milestone;
- no recorder-only reinterpretation.

## N.3 No second ending logic

Static/code review gate:
- completion recorder may depend on epilogue/context output;
- it must not contain branch-selection conditionals duplicating ending authority.

## N.4 Observation-only side-effect audit

Before recording:
- capture campaign state fingerprint;
- record completion;
- capture fingerprint.

Assert identical.

Only user-level store changes.

## N.5 Repeated view

Opening completion history/chronicle:
- no append;
- no campaign mutation;
- no checksum rewrite;
- no timestamp mutation.

---

# APPENDIX O — B3 CROSS-CAMPAIGN JOURNEY

## O.1 Campaign A
1. create seeded campaign;
2. reach deterministic ending fixture using standing endgame test infrastructure;
3. emit/observe completion;
4. append record;
5. validate history count = prior + 1.

## O.2 App/session restart
- reconstruct user-level store;
- no Campaign A runtime loaded;
- history remains.

## O.3 Campaign B
- create different campaign;
- verify A record visible;
- finish B;
- append B record;
- verify two records.

## O.4 Delete Campaign A save
If test harness supports:
- delete campaign save;
- history remains unchanged.

## O.5 Duplicate event
Replay Campaign B completion notification/observer:
- no third record.

## O.6 Final
Validate integrity chain/checksums and stable ordering.

This is the core acceptance journey for cross-campaign persistence.

---

# APPENDIX P — B3 NG+ COORDINATION TABLE

| Plan 175 status | B3 behavior |
|---|---|
| PROMOTED | expose completion-history query suitable for future unlock; no NG+ implementation |
| HOLD | completion record standalone; note possible future consumer |
| RETIRED | no NG+-specific fields unless independently required |
| UNKNOWN | read Wave 8 disposition; do not speculate |

No completion-record schema should be bloated for hypothetical NG+.

---

# APPENDIX Q — B4 CORPUS-WIDE PROVIDER DISCOVERY PROCEDURE

## Q.1 Search pass 1 — named provider symbols

Search codebase for:
- `Provider`;
- `Resolver`;
- `Lookup`;
- `Query`;
- `Callback`;
- `Func<`;
- optional interface injections;
- explicit `Bind` methods.

Collect only Core/runtime candidates.

## Q.2 Search pass 2 — neutral fallbacks

Patterns:
- `provider is null`;
- `provider == null`;
- nullable invoke;
- default return;
- `legacy`;
- `unbound`;
- `fallback`;
- `optional`.

## Q.3 Search pass 3 — host bindings

For each candidate symbol:
- direct references;
- setter calls;
- constructor injection;
- composition-root registration;
- host-session setup;
- test-only references.

## Q.4 Search pass 4 — documentation

Search owning plan/docs for:
- intentionally unbound;
- future binding;
- legacy neutral;
- optional provider.

## Q.5 Search pass 5 — current tests

Determine whether tests exercise:
- unbound path;
- bound path;
- host composition.

## Q.6 Deduplicate inventory

Same conceptual provider may appear through:
- interface;
- backing field;
- setter.

One inventory row per semantic port.

---

# APPENDIX R — B4 PROVIDER CLASSIFICATION DECISION TREE

## R.1 Is the port reachable in production?
- **No:** determine dead/test-only/retired.
- **Yes:** continue.

## R.2 Does a production host bind it?
- **Yes:** BOUND; prove.
- **No:** continue.

## R.3 Does owner documentation intentionally allow neutral-unbound?
- **Yes:** INTENTIONALLY-UNBOUND; document reason.
- **No:** continue.

## R.4 Does current plan require the effect?
- **Yes:** wire host if owner/seam clear.
- **No / retired:** RETIRED.
- **Ambiguous:** DECISION-BLOCKED.

## R.5 Does binding require new authority?
- **Yes:** stop; architecture package.
- **No:** bounded wiring.

---

# APPENDIX S — B4 HOST-WIRING TEST PATTERN

For each new binding:

## S.1 Unbound fixture
Instantiate Core owner without provider using supported test constructor.
Assert legacy-neutral behavior.

## S.2 Bound fixture
Provide deterministic authoritative provider.
Assert intended behavior differs exactly as contract specifies.

## S.3 Production composition
Instantiate current host/composition root.
Assert provider is bound to real source.

## S.4 Save/restore
Fresh runtime restore:
- composition runs;
- provider rebound;
- behavior matches continuous state.

## S.5 Duplicate binding
If setters allow repeated bind:
- current policy determines whether overwrite/reject;
- test no duplicate event/subscription.

---

# APPENDIX T — B4 PROVIDER INVENTORY EXAMPLE FORMAT

| ID | Core path | Semantic effect | Neutral behavior | Expected host | Actual host | Final state | Reason | Test |
|---|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

The inventory itself becomes the validation source. It should be concise enough to maintain.

---

# APPENDIX U — B4 CLOSURE GATE IMPLEMENTATION OPTIONS

Choose current repository-compatible method.

## U.1 Structured manifest
Best if repo already uses manifests/registries.

Pros:
- deterministic;
- easy CI validation.

## U.2 Code annotation + extractor
Use only if existing tooling supports.

## U.3 Explicit test inventory
Acceptable if small and stable:
- table in test/tool;
- each provider ID classified.

Avoid free-form grep as permanent acceptance because grep is too noisy.

---

# APPENDIX V — B4 PORT CONTRACT DOCUMENTATION STANDARD

Each optional provider contract should state:

- provider purpose;
- authoritative source when bound;
- neutral behavior when unbound;
- expected host/composition location;
- whether unbound is valid production state;
- determinism/save implications.

No full API rewrite required.

---

# APPENDIX W — B5 CORPUS DUPLICATE DETECTION TOOL

## W.1 Inputs
Enumerate all integration plan files.

Extract:
- normalized title;
- source Plan IDs;
- revision/date;
- corpus key.

## W.2 Candidate heuristics
- exact normalized title;
- same source Plan IDs;
- title similarity above conservative threshold if tooling allows;
- identical first scope paragraph.

Heuristics only.

## W.3 Output
CSV/Markdown table of candidates.

No automatic file deletion or supersede status.

## W.4 False-positive control
Every candidate manually classified.

## W.5 Persistent census column
Add:
- duplicate class;
- authoritative plan;
- reconciliation date.

---

# APPENDIX X — B5 AUTHORITY RECONCILIATION PACKET

## X.1 Evidence
**C1[10] revision:**<br>
**C2[14] revision:**<br>
**Wave 10 B5 citation target:**<br>
**Downstream dependencies:**<br>

## X.2 Semantic differences
- dependencies:
- phase order:
- delivery invariant:
- producer-port gate:
- save tests:
- non-goals:

## X.3 Decision
**Class:** TRUE DUPLICATE / REVISIONED SIBLING / DISTINCT<br>
**Authority:**<br>
**Reason:**<br>

## X.4 Unique clauses
If any:
- clause;
- status;
- merge or separate queue.

## X.5 Census effect
Rows after reconciliation.

---

# APPENDIX Y — C1 DECISION REGISTER REVIEW SESSION PACKET

## Y.1 Session preparation

Sort decisions by:
1. current DAG blockers;
2. matured deferrals;
3. bounded/high-value;
4. long-term optional.

## Y.2 One-page item format

**ID / topic**<br>
**Why still open**<br>
**Current premise**<br>
**Options from memo**<br>
**Blast radius**<br>
**Architecture-safe recommendation already recorded**<br>
**Needed verdict**<br>

No 20-page memo review during session unless foreman asks; link full memo.

## Y.3 Verdict capture

Immediately record:
- verdict;
- date;
- signer;
- condition;
- execution path.

Avoid verbal-only decisions.

## Y.4 Post-session reconciliation

Run consistency table across all source docs.

---

# APPENDIX Z — C1 DECISION CONSISTENCY MATRIX

| ID | Memo | Register | Debt | Closeout | Ledger | Census blocker | Consistent |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

A contradiction is itself a governance blocker.

---

# APPENDIX AA — C2 EXECUTION-SURFACED BLOCKER HARVEST

For each Wave 10–11 implementation log, inspect sections:
- remaining blocker;
- deferred;
- divergence;
- claim;
- decision;
- follow-up;
- repair;
- future package.

Create one normalized row.

## AA.1 Deduplication
Same blocker may appear in multiple logs.
Merge source references while keeping one queue identity.

## AA.2 Current re-verification
Before queueing:
- check source;
- check claim;
- check decision;
- check if another task already closed it.

Do not queue stale blockers.

---

# APPENDIX AB — C2 QUEUE RANKING

Rank ready nodes using:

### Hard dependency readiness
Highest priority.

### Downstream unlock
Prefer nodes releasing many dependents.

### Critical product path
Standing product/release blockers outrank optional depth.

### Authority clarity
Bounded owner > ambiguous consolidation.

### Decision readiness
Signed > unsigned.

### Package boundedness
Reviewable package > sprawling migration.

Document rank reason.

No opaque numeric score required unless census already uses one.

---

# APPENDIX AC — C2 THREE-DRAIN COUNT DEFINITIONS

## AC.1 Queue count N
Count corpus/repair packages that remain nonterminal and intentionally tracked.

Exclude:
- sealed;
- superseded duplicates;
- retired.

Include:
- dependency-blocked;
- claim-blocked;
- decision-blocked plans if still actionable later.

Define consistently.

## AC.2 Decision count M
Count current decision items not terminal:
- UNSIGNED;
- DEFERRED-WITH-CONDITION;
- SIGNED-PENDING-EXECUTION if register considers open.

State categories.

## AC.3 Quarantine count K
Count test files currently excluded under quarantine policy.

Do not count permanently retired tests if removed from quarantine manifest.

## AC.4 Zero condition
All:
- N = 0;
- M = 0;
- K = 0

then maintenance cadence.

If one nonzero:
- series continues only for that drain.

---

# APPENDIX AD — C2 WAVE 12 HEAD PRE-VERIFICATION

For highest queue node:

1. read plan;
2. verify file exists;
3. premise-check source;
4. resolve dependencies;
5. check claims;
6. check decisions;
7. check duplicate status;
8. extract exact remainder;
9. identify acceptance;
10. record why ready.

This saves Wave 12 from beginning with an unverified premise.

---

# APPENDIX AE — PART 2 IMPLEMENTATION LOG TEMPLATE

# `<TASK / CORPUS>` IMPLEMENTATION LOG

## Identity
**Plan / decision / governance artifact:**<br>
**Starting status:**<br>

## Dependencies / claims
| Item | State | Evidence |
|---|---|---|
|  |  |  |

## Premise
**Historical:**<br>
**Current:**<br>

## Scope matrix
| Clause/item | Existing state | Executed here | Final state | Evidence |
|---|---|---|---|---|
|  |  |  |  |  |

## Files
-

## Save / determinism
-

## Verification
| Command | Purpose | Result |
|---|---|---|
|  |  |  |

## Governance updates
**Census:**<br>
**Ledger:**<br>
**Register:**<br>
**Claims:**<br>

## Final
**Terminal state:**<br>
**Remaining blocker:**<br>
**Next queue movement:**<br>

---

# APPENDIX AF — MERGE READINESS CHECKLISTS

## AF.1 B3
- [ ] ending authority unchanged;
- [ ] record schema minimal;
- [ ] append-only API;
- [ ] stable duplicate guard;
- [ ] user-level persistence;
- [ ] checksum/tamper validation;
- [ ] cross-campaign journey;
- [ ] observation-only fingerprint;
- [ ] endgame suite;
- [ ] log/census.

## AF.2 B4
- [ ] inventory exhaustive enough for plan;
- [ ] every candidate context-read;
- [ ] every row terminal/classified;
- [ ] newly wired provider tests;
- [ ] unbound reasons;
- [ ] validation gate;
- [ ] gate failure tests;
- [ ] log/census.

## AF.3 B5
- [ ] both plans read;
- [ ] semantic diff;
- [ ] duplicate class;
- [ ] authority selected;
- [ ] unique clauses handled;
- [ ] corpus sweep;
- [ ] banners;
- [ ] census;
- [ ] zero production diff;
- [ ] docs index.

## AF.4 C1
- [ ] register rebuilt;
- [ ] premise truth pass;
- [ ] foreman verdicts;
- [ ] bounded signed items executed;
- [ ] large items queued;
- [ ] declined closed;
- [ ] deferred conditioned;
- [ ] consistency matrix;
- [ ] zero unsigned-without-condition.

## AF.5 C2
- [ ] Wave 10–11 logs harvested;
- [ ] statuses spot-checked;
- [ ] blockers reverified;
- [ ] duplicate sweep rerun;
- [ ] DAG refreshed;
- [ ] N/M/K counts;
- [ ] queue head preverified;
- [ ] zero production diff;
- [ ] docs index.

---

# APPENDIX AG — FINAL WAVE 12 HANDOFF FORMAT

# Wave 12 Input

## Three drains
**Queue N:**<br>
**Decisions M:**<br>
**Quarantine K:**<br>

## Queue head
**Corpus key:**<br>
**Title:**<br>
**Status:**<br>
**Premise:**<br>
**Remainder:**<br>
**Dependencies:**<br>
**Claims:**<br>
**Decision:**<br>
**Duplicate status:**<br>
**Acceptance:**<br>

## Next five
| Rank | Plan | Why ready | Main blocker/risk |
|---:|---|---|---|
| 1 |  |  |  |
| 2 |  |  |  |
| 3 |  |  |  |
| 4 |  |  |  |
| 5 |  |  |  |

## Surfaced blockers
-

## Decision packages
-

## Quarantine trajectory
-

Wave 12 is authored from this artifact.

---

# APPENDIX AH — FINAL QUALITY BAR

Wave 11 Part 2 is flagship-quality only if:

- C2[12] completion history is a real immutable user-level record, not a campaign-save field or second ending calculation;
- C2[13] produces an explicit whole-scope provider closure table and a preventive validation gate;
- C2[14] cannot cause a second executor to rebuild the Wave 10 delivery chain;
- the decision register is reconstructed from source truth and contains no ambiguous open items;
- the census refresh actively re-verifies execution outputs rather than merely changing statuses to SEALED;
- duplicate drift becomes measurable and repeatably detectable;
- the three drains have reproducible definitions;
- Wave 12 receives a premise-checked queue head;
- docs-only governance tasks preserve zero production diff;
- no terminal status hides a remaining claim, decision, duplicate, or acceptance failure.

**The Wave 11 close criterion is operational truth:** one completion-history authority, explicit port-binding decisions, one authoritative delivery-plan contract, non-ambiguous decisions, and a current queue.

# APPENDIX AI — B3 COMPLETION RECORD SECURITY / INTEGRITY BOUNDARY

This plan uses **tamper-evident** in the narrow engineering sense. The completion history detects unauthorized/accidental modifications according to the project’s standing checksum discipline. It does not claim cryptographic secrecy, anti-cheat hardening, server authority, or adversarial resistance beyond the mechanism actually implemented.

## AI.1 Required wording

Documentation should say:
- checksum-validated;
- tamper-evident;
- append-only by application contract.

Avoid:
- unhackable;
- secure against arbitrary malicious users;
- cryptographically trusted,
unless the project separately establishes those guarantees.

## AI.2 Threat cases in scope

- accidental file corruption;
- manual text/JSON edit if checksum invalidates;
- truncated file;
- reordered chained records;
- stale partial write where current store can detect.

## AI.3 Threat cases out of scope

Unless C2[12] explicitly adds them:
- attacker recomputing checksum;
- filesystem compromise;
- process memory manipulation;
- online account trust.

## AI.4 Recovery semantics

Read current store policy before choosing:
- fail whole history;
- accept valid prefix;
- isolate corrupt record;
- offer reset.

The implementation plan does not authorize destructive auto-reset.

---

# APPENDIX AJ — B3 USER-LEVEL STORE MIGRATION PACKET

## AJ.1 Version 0 / absent store

Expected:
- no error;
- empty completion history;
- first append creates canonical storage.

## AJ.2 Current store older schema

If existing profile/settings store has migration chain:
- add completion-history field to latest DTO;
- migration defaults empty.

## AJ.3 Completion-history schema changes later

The initial implementation should make future migration possible:
- version field/envelope if current store uses;
- stable record IDs;
- stable semantic enum/IDs rather than display text.

## AJ.4 Test matrix

| Input store | Expected load | Expected history | Write-back |
|---|---|---|---|
| absent | success | empty | on first append |
| current without field | success | empty | current schema |
| valid with records | success | records | unchanged until append |
| corrupt | policy-defined | policy-defined | no silent healing |

---

# APPENDIX AK — B3 COMPLETION CONSUMER READ MODEL

If C2[12] requires chronicle/completion presentation, expose a read model rather than raw persistence DTO.

Read model may include:
- ending display data resolved from stable ID;
- completion order/date;
- milestone labels;
- difficulty label.

Persistence retains stable IDs only where practical.

## AK.1 Missing current content

If an old record references an ending/milestone removed/renamed:
- use current unknown/missing-reference presentation policy;
- do not mutate historical record.

## AK.2 Sorting

Use append ordinal / canonical completion sequence.

Do not sort by localized ending title.

## AK.3 Invalid record

If checksum invalid:
- do not present as valid completion.
Presentation follows store policy.

---

# APPENDIX AL — B4 INVENTORY COMPLETENESS REVIEW

The closure gate is only valuable if the discovery pass captures the relevant provider pattern family.

## AL.1 Reviewer sampling

After inventory built:
- select several known bound providers from prior waves;
- confirm they appear;
- select known intentionally-unbound pattern;
- confirm it appears/classifies;
- search a second way for provider symbols and compare.

## AL.2 Miss-risk categories

High risk:
- lambda/delegate stored under non-Provider name;
- interface dependency with optional default;
- static host callback;
- generated bindings;
- partial classes hiding setters.

Search these explicitly if C2[13] plan scope includes them.

## AL.3 Inventory boundary

Document exclusions:
- presentation-only callbacks;
- test injection;
- diagnostics hooks;
- external adapter interfaces that are mandatory rather than optional.

A defined boundary is better than pretending “all callbacks in repo” are port contracts.

---

# APPENDIX AM — B4 PROVIDER CLOSURE STATUS SEMANTICS

## AM.1 BOUND

Must mean:
- all required production composition paths bind;
- authoritative source is correct;
- focused behavior test green.

## AM.2 INTENTIONALLY-UNBOUND

Must mean:
- production unbound is valid;
- neutral fallback is a supported contract;
- reason documented.

Not:
- “we have not implemented it yet.”

## AM.3 RETIRED

Must mean:
- effect intentionally no longer participates;
- no expected consumer missing;
- source docs updated.

## AM.4 ROUTED-REPAIR

Means:
- provider should be bound;
- current missing/broken binding is a regression;
- repair package exists.

The C2[13] plan remains partial until repair closes if that provider is mandatory.

## AM.5 DECISION-BLOCKED

Use when product/architecture must choose whether effect belongs in production.

---

# APPENDIX AN — B4 VALIDATION-GATE MAINTENANCE CONTRACT

The gate must remain cheap enough to run routinely.

## AN.1 Inputs
Structured provider inventory + optional static source checks.

## AN.2 Outputs
- provider ID;
- classification failure;
- missing reason;
- missing host reference;
- stale source path.

## AN.3 Non-goals
- dynamic whole-game dependency analysis;
- runtime reflection boot of every subsystem;
- replacing focused wiring tests.

## AN.4 Update rule
When adding a new optional provider:
- inventory must change in same PR/package;
- gate fails otherwise.

This converts C2[13] from one-time sweep into standing closure discipline.

---

# APPENDIX AO — B5 DUPLICATE SEMANTIC-DIFF PROCEDURE

Text diff alone may misclassify plans because formatting/wording can diverge. Compare semantic categories.

## AO.1 Scope
What problem is each plan solving?

## AO.2 Outputs
What artifacts/mechanisms does each require?

## AO.3 Order
What phases/sub-plans exist and in what order?

## AO.4 Dependencies
Are prerequisites identical?

## AO.5 Acceptance
Do they require the same tests/gates?

## AO.6 Non-goals
Do boundaries differ?

## AO.7 Revision markers
Dates/version notes.

Then classify.

## AO.8 Merge provenance

If unique clauses merged:
- annotate origin file;
- preserve sibling body;
- census notes merge date.

No silent historical rewriting.

---

# APPENDIX AP — B5 DEPENDENCY REWRITE RULES

After authority selected, downstream docs may reference the non-authoritative row.

Audit:
- corpus dependency headers;
- census DAG;
- implementation logs;
- ledger.

Update **current planning references** to authoritative identity.

Historical logs may retain original references with reconciliation note.

Do not rewrite old history as though it cited authority originally.

---

# APPENDIX AQ — C1 DECISION REGISTER TRIAGE MODEL

Each unresolved decision gets urgency:

### P0 — current chain blocker
Prevents highest ready DAG node.

### P1 — active subsystem acceptance blocker
Code landed but cannot close.

### P2 — future chain prerequisite
Not immediate but required soon.

### P3 — optional/product polish
No current dependency.

Foreman session starts P0/P1.

Urgency does not decide outcome.

---

# APPENDIX AR — C1 DECISION CONDITION LANGUAGE

A valid deferred condition is concrete.

Good:
- “Recheck after Plan 24 24C is SEALED.”
- “Recheck when survivor-avatar owner exists.”
- “Recheck after C1[16] content-rail utilization report.”

Bad:
- later;
- when ready;
- future wave;
- TBD.

Every condition should be machine/human-checkable from repository state.

---

# APPENDIX AS — C1 SIGNED-PACKAGE CREATION STANDARD

For signed large decisions:

**Decision ID:**<br>
**Chosen option:**<br>
**Source memo:**<br>
**Current premise:**<br>
**Dependencies:**<br>
**Owner:**<br>
**Claim paths:**<br>
**Implementation phases:**<br>
**Save impact:**<br>
**Determinism:**<br>
**Acceptance:**<br>
**Queue rank:**<br>

This package enters census/ledger if it corresponds to a corpus plan remainder.

---

# APPENDIX AT — C2 CENSUS STATUS RECONCILIATION RULES

## AT.1 SEALED row recheck

A SEALED row remains sealed if:
- implementation exists;
- no log says known missing mandatory clause;
- acceptance evidence still structurally valid.

Do not rerun every historical test; spot-check current relevant gates according to policy.

## AT.2 PARTIALLY-SEALED row

Remainder must still be current.
If another wave closed it:
- upgrade to SEALED with evidence.

## AT.3 BLOCKED row

Re-evaluate blocker:
- dependency may have sealed;
- claim may have closed;
- decision may have signed.

Blocked rows are most likely to become ready after a wave.

## AT.4 SUPERSEDED duplicate

Ensure no queue rank remains.

## AT.5 STALE-PREMISE

Check whether newer work changed premise again.
Usually remains non-executable.

---

# APPENDIX AU — C2 EXECUTION-LOG DIVERGENCE HARVEST

Implementation logs should be treated as first-class blocker sources.

Look for:
- “deferred”;
- “remaining”;
- “blocked”;
- “claim”;
- “decision”;
- “follow-up”;
- “not covered”;
- “out of scope”;
- “repair”;
- “migration”.

Each phrase needs context. Do not queue every mention.

## AU.1 A real surfaced blocker
It is:
- current;
- required by contract or newly exposed correctness;
- unsealed.

## AU.2 Not a blocker
- explicit non-goal;
- optional future idea;
- historical note;
- already-resolved follow-up.

---

# APPENDIX AV — C2 QUEUE-NODE ACCEPTANCE QUALITY

A queue row is actionable only if it has:

- precise scope;
- current premise;
- owner;
- dependencies;
- claims;
- decision state;
- acceptance;
- source evidence.

If missing:
- queue-maintenance task resolves metadata before execution.

Do not hand Wave 12 an ambiguous row.

---

# APPENDIX AW — C2 DUPLICATE SWEEP GOVERNANCE

The duplicate sweep should recur when new corpus plans are added.

Standing policy:
- title/source identity checked at census ingestion;
- duplicate candidate reconciled before executable state;
- one authority per duplicated scope.

This prevents another C1[10]/C2[14] case.

---

# APPENDIX AX — C2 WAVE 12 HEAD PREMORTEM

Before calling a node “ready,” ask:

1. Could it be duplicate?
2. Could a prerequisite be only partially sealed?
3. Could an active claim overlap?
4. Could a decision be unsigned?
5. Could current source already satisfy it?
6. Could the plan premise be stale?
7. Could scope be too large for one package?

Record answers.

---

# APPENDIX AY — PART 2 REVIEWER QUESTIONS

## B3
- Is history truly cross-campaign?
- Can any existing API mutate old records?
- Does recorder duplicate ending logic?
- Does tamper evidence match actual guarantee?
- Does completion observation alter gameplay fingerprint?
- Is NG+ kept out of scope?

## B4
- How was inventory completeness checked?
- Did every candidate get host search, not grep-only classification?
- Are intentionally-unbound states truly intentional?
- Does validation gate prevent new ambiguity?
- Did wiring preserve unbound legacy parity?

## B5
- Were plans semantically diffed?
- Why is chosen authority authoritative?
- Were unique clauses preserved?
- Can an executor still accidentally run both?
- Did corpus-wide sweep find more cases?

## C1
- Is every decision state sourced from current docs?
- Which deferred conditions matured?
- Did any stale memo get refreshed rather than blindly signed?
- Are signed large items queued?

## C2
- Which rows changed status and why?
- Which blockers were surfaced by execution?
- Are N/M/K counts reproducible?
- Why is Wave 12 head ready?
- Did refresh itself change zero production code?

---

# APPENDIX AZ — FINAL INTEGRATION HANDOFF

## Wave 11 final state

**B3 C2[12]:**<br>
**B4 C2[13]:**<br>
**B5 C2[14]:**<br>
**Decision register:**<br>
**Census:**<br>

## Quality gates
**Build:**<br>
**Verify-fast:**<br>
**Endgame:**<br>
**Provider closure gate:**<br>
**Docs-index:**<br>

## Three drains
**Queue N:**<br>
**Decision M:**<br>
**Quarantine K:**<br>

## Wave 12
**Head:**<br>
**Premise:**<br>
**Dependencies:**<br>
**Claims:**<br>
**Acceptance:**<br>

## New packages
-

## Reopened repairs
-

No Wave 12 planning should start until this handoff and refreshed census agree.

---

# APPENDIX BA — FINAL RELEASE-BLOCKING CONDITIONS

Do not close Wave 11 if any is true:

- completion record can mutate;
- completion record disappears with campaign save;
- completion recorder changes ending/gameplay state;
- provider inventory contains UNKNOWN rows;
- validation gate allows unclassified provider;
- C1[10]/C2[14] both remain executable duplicates;
- decision register contains unsigned item with no explicit review owner/session state;
- deferred decision lacks condition;
- census status contradicts an implementation log;
- duplicate sweep candidates remain unclassified;
- Wave 12 queue head is not premise-checked;
- B5 or C2 docs-only packages changed production code unexpectedly.

---

# APPENDIX BB — MAINTENANCE-CADENCE TRANSITION

The endgame rule is mechanical.

If after C2 refresh:
- queue N = 0;
- decision M = 0;
- quarantine K = 0;

then:
1. do not create Wave 12;
2. mark gap-sealing series complete;
3. transition to maintenance cadence;
4. retain:
   - periodic census integrity;
   - decision register (empty unless new decisions arise);
   - quarantine policy;
   - release gates;
   - content acceptance;
   - duplicate detection.

If any drain nonzero:
- continue only against measured remainder.

This prevents infinite roadmap generation.

# APPENDIX BC — FINAL EXECUTION ORACLES

## BC.1 B3 completion-record oracle

The task is complete only when all statements are true:

- a completed campaign causes one append operation;
- the appended semantic payload is derived from canonical ending context;
- no code path rewrites prior records;
- the record remains available after a different campaign starts;
- deleting or replacing the campaign save does not delete completion history;
- duplicate observation of the same completed run does not append again;
- integrity validation detects the plan-defined corruption/tamper cases;
- opening history/chronicle does not mutate campaign or record state;
- the campaign simulation fingerprint is unchanged by recording.

A passing UI screenshot is not sufficient evidence.

## BC.2 B4 provider-closure oracle

The task is complete only when:

- every in-scope optional provider appears in the inventory;
- each provider has one explicit final state;
- each BOUND provider has current production host evidence;
- each INTENTIONALLY-UNBOUND provider has a reason and neutral contract;
- each RETIRED provider has retirement evidence;
- each required missing binding has been wired or routed as repair;
- the standing gate fails if a new provider is added without a decision.

“Nothing obvious was found by grep” is not closure.

## BC.3 B5 duplicate oracle

After reconciliation, a new executor reading the census must see exactly one of:
- one authoritative executable “Goods Must Arrive” plan;
- one authoritative plan plus one separately scoped unique remainder.

They must never see two apparently executable copies of the same delivery-chain contract.

## BC.4 C1 register oracle

Every decision row must answer:
- what is the verdict?;
- if deferred, what exact condition?;
- if signed, where is execution?;
- if declined, where is closure?;
- if stale, what superseded premise?;
- when is it rechecked?

No blank/implicit state.

## BC.5 C2 census oracle

A new Wave 12 planner must be able to select the next package using the census alone plus the referenced evidence. If broad repository rediscovery is still necessary to understand the queue head, the refresh is incomplete.

---

# APPENDIX BD — FINAL COMMAND / EVIDENCE LOG

For every command run during Part 2, record:

**Task:**<br>
**Plan clause / governance purpose:**<br>
**Command:**<br>
**Commit / worktree:**<br>
**Expected:**<br>
**Actual:**<br>
**Case/check count:**<br>
**Warnings:**<br>
**Artifact/log path:**<br>
**Disposition:**<br>

Recommended minimum command groups:

### B3
- completion-record focused tests;
- user-store tests;
- tamper/checksum tests;
- endgame/epilogue suite;
- observation fingerprint;
- build;
- verify-fast.

### B4
- provider inventory/gate;
- newly wired provider tests;
- host/composition tests;
- touched domain suites;
- build;
- verify-fast.

### B5
- duplicate detection script;
- docs index;
- zero-production-diff inspection.

### C1
- memo-specific commands for bounded executions;
- build/verify-fast if code changed;
- docs consistency checks.

### C2
- census/duplicate/DAG tooling;
- docs index;
- zero-production-diff inspection.

---

# APPENDIX BE — FINAL MERGE ORDER

Recommended:

1. B3 completion record.
2. B4 provider closure.
3. B5 duplicate reconciliation.
4. C1 decision-register pass.
5. C2 final census refresh.

After every merge:
- refresh `HEAD`;
- refresh claim table;
- update implementation log references;
- avoid carrying stale hashes/paths into subsequent governance work.

C2 must execute from the final merged state.

---

# APPENDIX BF — WAVE 11 PART 2 RELEASE NOTE TEMPLATE

# ASHFALL Wave 11 Part 2 — Closeout

## C2[12] Completion Record
**Status:**<br>
**Store owner:**<br>
**Record count fixture:**<br>
**Append-only proof:**<br>
**Integrity model:**<br>
**Cross-campaign test:**<br>
**Epilogue parity:**<br>
**Fingerprint:**<br>

## C2[13] Provider Closure
**Status:**<br>
**Providers inventoried:**<br>
**Bound:**<br>
**Intentionally unbound:**<br>
**Retired:**<br>
**Repair-routed:**<br>
**Decision-blocked:**<br>
**Gate command/result:**<br>

## C2[14] Duplicate Reconciliation
**Class:**<br>
**Authoritative plan:**<br>
**Sibling disposition:**<br>
**Unique clauses:**<br>
**Other duplicate candidates:**<br>

## Decision Register
**Signed-executed:**<br>
**Signed-queued:**<br>
**Declined:**<br>
**Deferred-with-condition:**<br>
**Stale/superseded:**<br>
**Unsigned-without-condition:** 0 required

## Census Heartbeat
**Queue N:**<br>
**Decision M:**<br>
**Quarantine K:**<br>
**New blockers:**<br>
**Reopened rows:**<br>
**Newly sealed rows:**<br>

## Wave 12
**Pre-verified head:**<br>
**Current premise:**<br>
**Hard dependencies:**<br>
**Claims:**<br>
**Decision state:**<br>
**Acceptance:**<br>

---

# APPENDIX BG — FINAL REVIEWER SIGN-OFF

Reviewer signs:

- [ ] C2[12] does not create a second ending authority.
- [ ] Completion history is cross-campaign/user-level.
- [ ] Completion record is append-only by application API.
- [ ] Integrity/tamper semantics are accurately described.
- [ ] Recorder does not alter campaign fingerprint.
- [ ] C2[13] inventory is systematic rather than anecdotal.
- [ ] No in-scope provider remains UNKNOWN.
- [ ] The provider validation gate can demonstrably fail.
- [ ] C2[14] has one authoritative disposition.
- [ ] Corpus duplicate candidates are all classified.
- [ ] Decision register has zero unsigned-without-condition.
- [ ] Deferred decisions all have concrete conditions.
- [ ] Census was refreshed after all Wave 11 merges.
- [ ] Three-drain counts are reproducible.
- [ ] Wave 12 head was re-verified at final HEAD.
- [ ] Governance-only tasks have zero unintended production diff.

If any box lacks evidence, Wave 11 remains open.

---

# APPENDIX BH — FINAL WAVE 12 TRANSITION RULE

The initial Wave 11 source previews C1[16] and C2[15] as likely next heads. The final decision is made only after C2 refresh.

If C1[16] is ready:
- Wave 12 can begin with the dead-content depth pass only if its target mechanisms/rails are sealed and content rows have real consumers.

If C2[15] outranks it:
- begin with input reality / focus / controller parity only if its prerequisites and claims are clear.

If a newly surfaced repair or dependency gate outranks both:
- Wave 12 begins there.

**There is no entitlement to numeric progression.**

---

# FINAL EXECUTION NOTE

Wave 11 Part 2 is the governance-heavy closure of the wave. Its job is not to maximize code volume. It must make five things unambiguous:

1. **What proves a campaign was completed, across campaigns?**
2. **Which optional providers are actually bound, intentionally neutral, retired, or broken?**
3. **Which “Goods Must Arrive” plan document is authoritative?**
4. **What is the current verdict for every standing decision seam?**
5. **What is the real, dependency-ready next queue head?**

Once those five answers are backed by code/tests/docs and the refreshed census, Wave 11 is closed and Wave 12 can proceed from actuality rather than accumulated planning assumptions.

# APPENDIX BI — FINAL AUDIT PACKET

Before handoff, create one compact audit packet proving that documentation state and implementation state agree.

## BI.1 C2[12]
**Plan status:**<br>
**Record owner:**<br>
**User-level store:**<br>
**Immutability proof:**<br>
**Integrity/tamper proof:**<br>
**Cross-campaign proof:**<br>
**Epilogue parity proof:**<br>
**Observation-only fingerprint:**<br>

## BI.2 C2[13]
**Inventory source:**<br>
**Inventory row count:**<br>
**BOUND count:**<br>
**INTENTIONALLY-UNBOUND count:**<br>
**RETIRED count:**<br>
**ROUTED-REPAIR count:**<br>
**DECISION-BLOCKED count:**<br>
**Unknown count:** must be 0 for full seal<br>
**Gate result:**<br>

## BI.3 C2[14]
**Duplicate class:**<br>
**Authoritative file:**<br>
**Non-authority banner:**<br>
**Unique clauses:**<br>
**Wave 10 B5 scope impact:**<br>
**Corpus-wide duplicate count:**<br>
**Unclassified duplicate count:** must be 0<br>

## BI.4 Decision register
**Open unsigned without condition:** must be 0<br>
**Deferred with condition:**<br>
**Signed pending package:**<br>
**Signed executed:**<br>
**Declined:**<br>
**Stale/superseded:**<br>

## BI.5 Census
**Queue count:**<br>
**Decision count:**<br>
**Quarantine count:**<br>
**Top ready node:**<br>
**Top node premise checked:**<br>
**Top node dependencies checked:**<br>
**Top node claim state checked:**<br>

The audit packet is the final reviewer-facing proof that Wave 11 Part 2 did not merely change labels.

---

# APPENDIX BJ — FINAL GOVERNANCE INVARIANTS

The following invariants become standing project discipline after Wave 11:

1. A campaign completion history record is derived from the canonical ending projection and stored outside campaign saves.
2. Optional providers cannot remain silently ambiguous; each has a maintained binding decision.
3. Corpus plans with duplicate scope are reconciled before execution.
4. Signed decisions have an execution or queue disposition; deferred decisions have explicit conditions.
5. Every execution wave refreshes the corpus census before the next wave is authored.
6. Queue ordering follows dependencies and current evidence, not filename numbering.
7. Governance-only reconciliation tasks preserve production behavior.
8. Historical documents remain available for provenance but cannot masquerade as current executable authority after supersession.
9. The queue, decision register, and quarantine inventory are measurable drains with stable definitions.
10. When those drains are zero, the gap-sealing series ends.

---

# APPENDIX BK — FINAL WAVE 11 PART 2 ACCEPTANCE CHECKLIST

- [ ] C2[12] full plan read.
- [ ] Epilogue and difficulty sealed work credited.
- [ ] Completion-record remainder implemented or explicitly blocked.
- [ ] No second ending authority.
- [ ] User-level persistence proven.
- [ ] Append-only semantics proven.
- [ ] Tamper/checksum validation proven.
- [ ] Cross-campaign journey green.
- [ ] Campaign fingerprint unaffected by recorder.
- [ ] C2[13] full plan read.
- [ ] Optional-provider inventory complete within documented scope.
- [ ] Every provider classified.
- [ ] Newly required host bindings wired through existing composition.
- [ ] Legacy-neutral behavior preserved where valid.
- [ ] Standing validation gate installed and can fail.
- [ ] C2[14] semantic diff complete.
- [ ] One authoritative disposition recorded.
- [ ] Corpus duplicate sweep complete.
- [ ] Zero unintended production changes from B5.
- [ ] Decision register rebuilt from source docs.
- [ ] Condition-matured items presented for verdict.
- [ ] Signed bounded items executed.
- [ ] Signed large items queued.
- [ ] Declined items closed.
- [ ] Deferred items have named conditions.
- [ ] Zero unsigned-without-condition.
- [ ] Census rows reverified.
- [ ] Execution-surfaced blockers harvested.
- [ ] Duplicate sweep rerun during refresh.
- [ ] DAG reranked.
- [ ] Queue N measured.
- [ ] Decision M measured.
- [ ] Quarantine K measured.
- [ ] Wave 12 head preverified.
- [ ] Docs index green.
- [ ] Build/verify-fast green where applicable.
- [ ] Final release note generated from latest state.

**Wave 11 Part 2 is terminal only when every checked item is backed by evidence or the task is explicitly in a non-SEALED terminal state with a named blocker.**
