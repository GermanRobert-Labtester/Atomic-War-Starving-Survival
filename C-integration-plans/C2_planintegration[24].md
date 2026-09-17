# C2 — Flagship Integration Plan [24]: Retention, Save Corpus Archaeology, and the 400-Year Campaign

> **Deliverable:** `C2_planintegration[24].md`
> **Source scope:** Plan 55 — *The Long Haul: Retention, a Save Corpus, and the 400-Year Campaign*
> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window*
> **Primary objective:** make long campaigns a first-class supported scenario by defining explicit retention policies for every persisted growth vector, salvaging and versioning the project’s real historical save corpus, and proving 200-hour/400-year session stability across save size, load time, node/handler lifecycle, focus safety, memory growth, and resume correctness.
> **Required execution order:** **55A → 55B → 55C**
> **Urgent parallel prerequisite:** **55B.1 salvage of the existing ~7 MB real `holdfast_archive_*` corpus happens immediately, before any cleanup or refactor can destroy the only genuine historical saves.**
> **Dependencies:** Plan 39B soak harness, Plan 48A save compatibility fixtures/version policy, Plan 41C generations, Plan 38 calendar, Plan 31 journal/briefing attribution, Plan 36B bound-port report, Plan 16B/16C UI authority/lifecycle, Plan 37B focus safety, Plan 52A ambience resume, Plan 54B long-form playtest.
> **Scope discipline:** no global one-size-fits-all retention cap, no silent deletion of obligations/promises/ending state, no roll-up that contradicts prior player-visible history, no bulk rewrite of all growing collections in one commit, no personal paths/usernames in committed fixtures, no un-LFS’d save corpus, and no claim that a passing soak alone proves a good mature-campaign experience.

---

# 0. Executive Intent

ASHFALL’s continuity architecture has reached a point where long campaigns are no longer hypothetical.

The project now contains systems intended to operate over:

- months,
- years,
- generations,
- long faction wars,
- location memory,
- memorial history,
- standing records,
- survivor relation histories,
- repeated save/load cycles.

That changes the engineering contract.

A save system that merely parses today is not enough.
A panel that merely opens today is not enough.
A 30-day performance sample is not enough.

The game now needs to answer:

```text
What is this campaign allowed to remember forever?
What gets summarized?
What may be discarded?
How large can a save become?
How long can it take to load?
Which old saves must still load?
Does day 400 cost roughly what day 10 costs?
Does a panel opened 50 times leak nodes or handlers?
Can a day-250 campaign load and resume every live system correctly?
```

The source plan also identifies an unusually urgent asset:

```text
~7 MB of genuine historical ASHFALL saves
```

captured across real development states and currently living outside version control.

Those archives are not clutter.

They are regression archaeology.

They contain edge cases synthetic fixtures rarely reproduce:

- old pre-envelope shapes,
- mid-migration stores,
- empty or unusual rosters,
- large memorial lists,
- historical store combinations,
- real sequences of evolving save schemas.

Therefore C2[24] has two clocks:

```text
Clock A: salvage the historical corpus immediately.
Clock B: then execute 55A → 55B → 55C in order.
```

The target architecture is:

```text
all persisted growing collections
        │
        ▼
RetentionPolicy
        │
        ├─ keep newest K
        ├─ summarize older detail
        ├─ preserve semantic obligations
        └─ never silently truncate
        │
        ▼
RollingLog / collection-specific migration
        │
        ▼
bounded campaign save
        │
        ├──────────────► 400-year soak
        │
        └──────────────► save corpus fixtures
                               │
                               ▼
                       cross-version matrix
                               │
                               ▼
                     resumability/lifecycle tests
                               │
                               ▼
                       mature-campaign playtest
```

The flagship outcome is:

> **A 400-year campaign has a known, bounded persistence shape; every save format ASHFALL has actually written is preserved as a regression fixture; and a mature campaign can be loaded, resumed, navigated, and advanced without memory, node, handler, focus, or save-size drift.**

---

# 1. Source Diagnosis

The source establishes five critical facts.

## 1.1 No real retention policy exists

Persisted collections already grow over time, while the only existing cap-like logic is largely presentation-level.

## 1.2 Long-run saves already contain growth vectors

Examples include:

- meal serving logs,
- decrees,
- journal entries,
- memorial rows,
- census claims,
- pair histories,
- radiation-dose records,
- machine logs,
- location memories,
- generational records.

## 1.3 Long-run perf harnesses exist but do not yet gate growth

The project already has 30/180/360-day workload profiles.
The missing contract is:

```text
growth slope
```

not raw timing alone.

## 1.4 A real historical corpus exists outside the repo

This is the most urgent source-derived action.

The archives must be salvaged before cleanup.

## 1.5 Long-run play is now a product property

Generations, calendar progression, world autonomy, and standing records mean a mature multi-year campaign is part of the intended game identity, not merely a stress test.

The architectural reading is:

```text
retention + compatibility + lifecycle stability
must become one long-haul contract.
```

---

# 2. Program-Level Success Criteria

C2[24] closes only when all of the following are true.

## 2.1 Every persisted growing collection is inventoried

No unexplained unbounded list remains.

## 2.2 Every collection has an explicit retention policy

Possible outcomes:

- keep all,
- keep newest K,
- summarize old entries,
- preserve references but discard prose,
- bounded recent history + permanent roll-up.

## 2.3 Obligations are never silently lost

Promises, deadlines, grief state, ending flags, and other consequence-bearing state remain semantically intact.

## 2.4 Retention roll-ups are deterministic

Same input history produces the same summary.

## 2.5 Retention migrations are schema-aware

Old saves migrate to the new bounded form deliberately.

## 2.6 Oversized legacy saves report summarization

No silent truncation on load.

## 2.7 Save size has a stated ceiling or growth budget

A 400-year run has a known size profile.

## 2.8 Save-size slope is gated

Nightly soak detects unbounded growth.

## 2.9 Historical real saves are committed as sanitized fixtures

The project no longer depends on one developer disk.

## 2.10 Every supported historical save fixture loads

And can continue play.

## 2.11 Structural save fuzz fails safely

Corruption never crashes the app or silently wipes state.

## 2.12 Largest-fixture load time is measured

Cold-start regressions are visible.

## 2.13 50× panel lifecycle cycles restore node and handler baselines

No accumulation.

## 2.14 Session replacement rebinds all live systems

No stale authority or freed-object reference.

## 2.15 Focus never points to freed controls

New Game/Load remain safe.

## 2.16 Memory growth has a ceiling

Managed/native growth remains within a stated slope/budget.

## 2.17 Every corpus fixture is resumable

Not merely parseable.

## 2.18 Failure-injected loads route to documented recovery

No half-loaded shelter state.

## 2.19 Long-run player experience is playtested

A mature day-250-ish campaign remains legible after retention/roll-up.

## 2.20 Release/nightly gates include long-haul durability

The result is permanent CI coverage.

---

# 3. Architectural Invariants

## 3.1 Retention policy is per collection

No universal:

```text
maxEntries = 100
```

applied everywhere.

## 3.2 Semantics outrank storage savings

Never discard state whose absence changes future outcomes.

## 3.3 Display caps and storage caps are separate

A UI showing only 20 entries does not authorize deleting history from persistence.

## 3.4 Roll-ups preserve meaning

A summary may reduce detail, but not contradict facts the player was previously shown.

## 3.5 Retention is deterministic

No time-of-day/random/pruning-order dependence.

## 3.6 Migration is explicit

Retention changes that affect wire shape require versioned save migrations.

## 3.7 Real save corpus is treated as test data

Not as personal backup content.

## 3.8 Fixtures are sanitized

No usernames, absolute home paths, machine-specific secrets, or irrelevant personal data.

## 3.9 Fixtures are attributed

Every committed save fixture states:

- source era/version,
- capture context,
- purpose,
- expected behavior.

## 3.10 Resumability outranks parseability

A save is compatible only if it loads into a valid campaign that can continue.

## 3.11 Long-session correctness includes UI lifecycle

Not just Core state.

## 3.12 A passing soak is not sufficient UX evidence

A mature campaign still requires human playtest.

---

# 4. Dependency Graph

```text
41C generations ───────────────┐
38 calendar ───────────────────┤
31 journals/briefings ─────────┤
                              ▼
                     55A Retention Policy
                              │
                              ▼
                      bounded save shape
                              │
                              ▼
                     55B Save Corpus
                              │
                ┌─────────────┼──────────────┐
                ▼             ▼              ▼
            old saves     fuzz cases    max-size save
                │             │              │
                └─────────────┼──────────────┘
                              ▼
                    55C Long-Session Resume
                              │
          ┌───────────────────┼──────────────────┐
          ▼                   ▼                  ▼
      node/handler        focus/freed       memory/load
       stability           safety            stability
```

Cross-plan inputs:

```text
39B soak harness ─────────► 55A/55C growth assertions
48A version policy ───────► 55B cross-version matrix
36B port report ──────────► 55C rebind completeness
16B/16C lifecycle ────────► 55C panel/session stability
37B focus safety ─────────► 55C freed-object protection
52A ambience ─────────────► 55C loaded-world resume
54B playtests ────────────► 55C mature-game experience
```

Required order:

```text
55A → 55B → 55C
```

Urgent exception:

```text
55B.1 historical corpus salvage
→ execute immediately before all destructive cleanup
```

---

# 5. Immediate Preflight — Salvage the Real Save Corpus

This is not optional housekeeping.

It is a preservation action.

## 5.1 Locate Existing Archives

Source path class:

```text
Godot user-data ASHFALL directory
→ holdfast_archive_*
→ current slot saves
```

Do not hardcode one username/path into scripts.

Use platform/user-data resolution or explicit operator-provided source path.

---

# 5.2 Read-Only First Pass

Before modifying anything:

- enumerate archives,
- hash every file,
- record byte sizes,
- record timestamps,
- record directory names,
- do not open/write with the game yet.

Output:

```text
artifacts/save-corpus-salvage-inventory.json
```

---

# 5.3 Create a Temporary Preservation Copy

Copy the raw archives into a temporary protected working location before sanitization.

This is not necessarily committed.

Purpose:

```text
if sanitization script is wrong
→ original still exists
```

---

# 5.4 Hash Manifest

For each raw source entry:

```text
source_relative_path
sha256
bytes
timestamp
```

No personal absolute path in the committed manifest.

---

# 5.5 Privacy Scan

Scan raw fixtures for:

- absolute paths,
- usernames,
- machine names,
- emails,
- arbitrary local notes,
- telemetry-like personal fields.

Classify any hits before commit.

---

# 5.6 Sanitization Must Preserve Semantics

If a path/user string is structurally irrelevant:

```text
replace with deterministic placeholder
```

If it participates in checksum/wire semantics:

- update expected checksum after sanitized migration,
- document transformation.

Do not randomly edit fields.

---

# 5.7 Salvage Receipt

Create:

```text
docs/saves/SAVE_CORPUS_SALVAGE_RECEIPT.md
```

Record:

- source archive count,
- total bytes,
- earliest/latest date,
- raw hashes,
- sanitization actions,
- preserved fixture candidates.

This receipt protects against future “where did these saves come from?” ambiguity.

---

# 5.8 No Cleanup Until Salvage Complete

Block any maintenance script that would:

```text
rm -rf user data
reset app data
replace save roots
```

until salvage receipt exists.

---

# 6. Baseline Capture

Before retention migration, measure the current save-growth reality.

## 6.1 Persisted Growth Inventory

Search all persisted DTO/state types for:

- `List<T>`,
- arrays whose length grows,
- dictionaries keyed by day/event/entity,
- append-only strings/logs,
- cumulative counters plus detailed rows.

---

# 6.2 Current Save Growth

Run a reference long simulation without new retention.

Measure:

```text
day
campaign.json bytes
store-specific bytes where separately serialized
load time
save time
entry counts by collection
```

Use a shorter baseline if 400-year current run is too expensive, then extrapolate only as a temporary diagnostic—not a final claim.

---

# 6.3 Current Lifecycle Baseline

For each live panel:

```text
node count before open
node count after open
node count after close
handler count before/after
```

Record 5-cycle baseline before 50-cycle gate.

---

# 6.4 Current Memory Baseline

Measure:

- managed heap,
- native/Godot memory if available,
- texture/resource count,
- event subscription counts.

---

# 6.5 Verification Baseline

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/verify-fast.sh
```

Also run the current longest available soak and archive outputs.

---

# 7. Workstream 55A — Decide What a Campaign Is Allowed to Remember

## Goal

Every growing persisted collection has a deliberate retention contract, and the 400-year save has a known size/growth profile.

---

# 8. 55A Phase A — Build the Retention Inventory

This is the core deliverable.

Create:

```text
docs/saves/RETENTION.md
```

Start with a generated/manual-reviewed table:

| Owner | State field | Entry type | Growth trigger | Expected growth/day | Player-visible? | Consequence-bearing? | Current size | Proposed policy |
|---|---|---|---|---:|---:|---:|---:|---|

Include at minimum source-named systems:

- `KitchenNutritionState.servingLog`,
- `DoseLedgerSystem`,
- `JournalSystem`,
- memorial state,
- `CensusClaimSystem`,
- `VoluntaryRegisterSystem`,
- `LocationMemorySystem`,
- `FactionWarSystem` decrees/strike logs,
- `MachineLogSystem`,
- survivor pair histories,
- generation/standing records,
- any new Wave 6–8 logs.

---

# 9. 55A Phase B — Classify Growth by Semantic Importance

Each collection receives a semantic class.

Suggested:

```text
OBLIGATION
OUTCOME_HISTORY
RECENT_DETAIL
AGGREGATABLE_EVENT_LOG
DIAGNOSTIC_ONLY
DERIVABLE_CACHE
```

Rules:

## OBLIGATION

Never silently drop.

Examples:

- deadlines,
- promises,
- unresolved grief consequences,
- ending flags,
- active contracts.

## OUTCOME_HISTORY

May summarize only if meaning remains.

## RECENT_DETAIL

Keep bounded recent entries.

## AGGREGATABLE_EVENT_LOG

Roll up older records.

## DIAGNOSTIC_ONLY

May not belong in campaign save at all.

## DERIVABLE_CACHE

Prefer recomputation over persistence if safe.

---

# 10. 55A Phase C — Define `RetentionPolicy`

Create engine-free Core policy types.

Conceptual:

```csharp
RetentionPolicy
{
    PolicyId,
    RecentEntryLimit,
    PreserveAllReferences,
    SummaryMode,
    LandmarkRule,
    SchemaVersion
}
```

Use typed enums.

Avoid generic callback-heavy reflection magic.

---

# 11. 55A Phase D — Introduce `RollingLog<T>`

Create:

```text
Assets/Ashfall.Core/Records/RollingLog<T>.cs
```

Responsibilities:

- maintain bounded recent detail,
- invoke deterministic summary update,
- expose stable ordered iteration,
- preserve invariants.

Do not force every collection into `RollingLog<T>` if another structure is better.

It is a standard tool, not a universal rewrite mandate.

---

# 12. 55A Phase E — Summary Contract

Every roll-up defines:

```text
input event
→ summary mutation
```

Examples:

### Meal history

Recent detail:

```text
last N meals
```

Summary:

```text
total meals
mean nutrition
worst deficit band
last shortage day
```

### Dose history

Recent detail:

```text
recent exposure episodes
```

Summary:

```text
lifetime dose
max single episode
high-dose episode count
```

### Pair history

Recent detail:

```text
recent pair reasons
```

Summary:

```text
landmark events
net historical contribution
last band transition
```

---

# 13. 55A Phase F — Deterministic Roll-Up Ordering

If events arrive same day:

```text
stable event kind
→ stable source
→ stable ID
```

or existing deterministic event order.

Do not let dictionary iteration change summary bytes.

---

# 14. 55A Phase G — Reversible-in-Meaning Rule

A summary need not reconstruct every prose entry.

It must preserve the facts needed to make future behavior and player interpretation consistent.

Test:

```text
before roll-up
vs
after roll-up
→ future simulation equivalence for affected mechanics
```

where feasible.

---

# 15. 55A Phase H — Separate Display From Storage

Explicitly document:

```text
display_cap != persistence_cap
```

For briefing/journal UI:

- display may show fewer entries,
- storage policy remains independently defined.

Add regression test preventing UI render limit from truncating persisted data.

---

# 16. 55A Phase I — Never-Cap Obligation Matrix

Create a table of state that may not lose consequence.

Include source examples:

- deadlines,
- grief,
- pair history landmarks,
- ending flags.

For each:

```text
retention method
why future behavior needs it
```

Some may use summaries, but consequence-bearing meaning must remain.

---

# 17. 55A Phase J — Collection-by-Collection Migration Order

Do not bulk rewrite.

Recommended sequence:

1. lowest-risk purely historical logs,
2. meal logs,
3. dose ledger,
4. journal history,
5. memorial records,
6. census/register,
7. pair history,
8. faction/standing history,
9. location/generation history.

After each:

- build,
- unit tests,
- save migration test,
- checksum comparison,
- long-run mini-soak.

---

# 18. 55A Phase K — Store Schema Versioning

For every state shape changed by retention:

- bump store schema,
- register migration,
- update compatibility matrix,
- add fixture.

Do not modify save wire shape without Plan 48A discipline.

---

# 19. 55A Phase L — Legacy Oversize Migration

When an old save contains more detail than current policy:

```text
load full old state
→ deterministically summarize
→ preserve recent detail
→ emit migration diagnostic
```

No silent drop.

---

# 20. 55A Phase M — Player-Facing Migration Notice

Use existing save diagnostics UI.

Example semantic outcome:

```text
Historical records were compacted during save upgrade.
No active obligations were removed.
```

Localized.

Do not expose technical implementation details unnecessarily.

---

# 21. 55A Phase N — Save-Size Ceiling

Define one or more budgets.

Examples:

```text
campaign.json size at 10 years
campaign.json size at 100 years
campaign.json size at 400 years
growth slope/year
```

Do not choose threshold until baseline and retained semantics are measured.

---

# 22. 55A Phase O — Load/Save Time Budgets

Long saves need practical budgets.

Record:

```text
save time p50/p95
load time p50/p95
largest-fixture cold load
```

Use credible sample counts from Plan 39/46 discipline.

---

# 23. 55A Phase P — 400-Year Soak Scenario

Extend canonical soak policy to 400 years.

If simulation calendar year length differs from real 365-day concept, use project-defined campaign years.

Scenario must exercise:

- births/generations,
- deaths,
- relations,
- factions,
- journals,
- memorials,
- radiation,
- trade/policies,
- save/load cycles.

---

# 24. 55A Phase Q — Growth Slope Assertions

Primary checks:

```text
save_size_slope
day_advance_cost_slope
memory_slope
collection_length_slope
```

Assert:

```text
bounded by policy
```

rather than only absolute day-400 values.

---

# 25. 55A Phase R — Day 10 vs Day 400 Comparison

Source intent:

```text
day-400 cost ≈ day-10 cost
```

Implement as a tolerance band.

Example:

```text
late_window / early_window <= configured threshold
```

Actual threshold from baseline.

---

# 26. 55A Phase S — Retention Telemetry in Soak

Soak artifact should include:

```text
collection
recent_count
summary_count/value
pruned_count
bytes
```

This makes growth diagnosis attributable.

---

# 27. 55A Phase T — Retention Integrity Selftest

Add focused selftest:

```text
--retention-selftest
```

Outputs:

```text
RETENTION_POLICIES=N
UNPOLICIED_GROWING_COLLECTIONS=0
OBLIGATION_LOSS=0
SAVE_SIZE_CEILING_OK
```

or canonical equivalent.

---

# 28. 55A Tests

- per-policy caps,
- roll-up determinism,
- same-order/different-insertion-order canonical result,
- obligation preservation,
- old-save migration,
- migration diagnostic,
- display/storage separation,
- summary semantic equivalence,
- ceiling breach detection,
- slope breach detection.

---

# 29. 55A Definition of Done

- [ ] every persisted growth vector inventoried,
- [ ] semantic class assigned,
- [ ] per-collection retention policy,
- [ ] RetentionPolicy Core types,
- [ ] RollingLog where appropriate,
- [ ] deterministic summaries,
- [ ] display/storage distinction,
- [ ] obligation matrix,
- [ ] one-at-a-time migrations,
- [ ] schema bumps/migrations,
- [ ] oversized legacy summarization,
- [ ] migration notice,
- [ ] save-size ceiling,
- [ ] load/save time budget,
- [ ] 400-year soak,
- [ ] growth slope gate,
- [ ] retention selftest,
- [ ] RETENTION.md complete.

---

# 30. Workstream 55B — Build the Save Corpus

## Goal

Turn historical and synthetic campaign saves into a permanent compatibility/regression corpus.

---

# 31. 55B Phase A — Finalize Salvage Set

Using the urgent preflight inventory, choose which raw archives become fixtures.

Do not commit all redundant dated copies blindly.

---

# 32. 55B Phase B — Fixture Selection Matrix

Select deliberately:

```text
oldest known envelope
pre-envelope bare state
each save-store version boundary
representative expansion-era campaign
empty-roster edge
large memorial/history case
corrupted-but-recoverable case
maximal 400-year case
```

Where possible include genuine rather than hand-authored fixtures.

---

# 33. 55B Phase C — Fixture Directory

Create:

```text
Ashfall.Core.Tests/Fixtures/Saves/
```

Suggested structure:

```text
MANIFEST.json
legacy/
current/
corrupt/
maximal/
```

Keep naming semantic.

---

# 34. 55B Phase D — Fixture Manifest

Each fixture records:

```text
fixture_id
relative_path
source_era
game_version
save_schema_versions
manifest_version
capture_date
sanitized
sha256
expected_day
expected_survivor_count
expected_sections
expected_digest
purpose
```

No personal path.

---

# 35. 55B Phase E — Corpus Export Script

Create:

```bash
scripts/ci/export-save-corpus.sh
```

Responsibilities:

- accept explicit source user-data root,
- inventory,
- copy candidates,
- sanitize,
- generate manifest,
- refuse unredacted personal data,
- never overwrite committed fixtures without explicit flag.

---

# 36. 55B Phase F — Privacy/Redaction Gate

Apply Plan 46B/48C rules.

Fail fixture commit if content contains:

- home directory path,
- username,
- email,
- machine name,
- unrelated local diagnostics.

---

# 37. 55B Phase G — LFS Policy

Decide and document LFS pattern for save blobs.

Do not commit binary-ish large fixtures first and fix LFS later.

Run:

```bash
git check-attr filter -- <fixture>
git lfs ls-files
```

---

# 38. 55B Phase H — Load-Every-Fixture Gate

Per push:

```text
fixture
→ parse
→ checksum validation
→ expected day
→ expected roster
→ expected sections
→ migrate
→ save current format
→ compare expected canonical digest
```

---

# 39. 55B Phase I — Resave Idempotence

Where volatile metadata exists, compare canonical semantic digest rather than raw bytes.

Document excluded volatile fields.

---

# 40. 55B Phase J — Cross-Version Matrix

For every supported version:

```text
load
→ migrate
→ advance 30 days
→ save
→ reload
```

This proves compatibility beyond parsing.

---

# 41. 55B Phase K — Continue-Play Validation

During 30-day continuation exercise:

- day advance,
- survivor systems,
- factions,
- relations,
- journaling,
- save/load,
- no migration re-trigger loop.

---

# 42. 55B Phase L — Structured Fuzzing

Fuzz mutations should be targeted.

Cases:

- bad checksum,
- truncated JSON,
- missing `State`,
- unknown manifestVersion,
- duplicate section,
- missing section,
- swapped slot root,
- wrong store version,
- oversized historical log,
- unknown ID.

Expected:

```text
clean error
or
documented recovery
```

Never crash/silent wipe.

---

# 43. 55B Phase M — Recoverable-Corrupt Fixtures

Commit at least one fixture where:

- primary is bad,
- backup is good,

to validate player recovery path from Plan 39C/48C.

---

# 44. 55B Phase N — Real vs Synthetic Coverage

Tag each fixture:

```text
real_play
synthetic
handwritten_minimal
fuzz_mutant
```

Report coverage separately.

Do not let synthetic fixtures crowd out real archaeology.

---

# 45. 55B Phase O — Largest-Fixture Budget

Measure:

- cold load time,
- peak memory during load,
- save time after migration,
- post-load node/authority readiness.

Pin budget.

---

# 46. 55B Phase P — Corpus Documentation

Create:

```text
docs/saves/SAVE_CORPUS.md
```

For each fixture:

- what it is,
- what it proves,
- why retained,
- expected compatibility.

---

# 47. 55B Phase Q — Failure-Proof Fixture

Include a deliberately truncated fixture used only to prove:

```text
corpus gate fails
```

Keep it clearly marked as negative-control fixture.

---

# 48. 55B Phase R — CI Tiering

Fast:

```text
load all valid fixtures
migrate supported fixtures
```

Nightly:

```text
fuzz
30-day continuation
largest-fixture timing
```

Release:

```text
full compatibility matrix
recovery fixtures
```

---

# 49. 55B Phase S — Fixture Growth Policy

The corpus itself needs curation.

Do not add every new save.

Add fixture only when it proves a distinct contract:

- new schema boundary,
- bug regression,
- unique real-world edge,
- maximal shape.

---

# 50. 55B Phase T — Release Compatibility Link

Plan 48A compatibility matrix should link fixture IDs directly.

A claimed supported version with no fixture is a release error.

---

# 51. 55B Tests

- manifest schema,
- privacy redaction,
- fixture hash,
- load all,
- migration,
- 30-day continuation,
- backup recovery,
- structured fuzz,
- negative-control gate,
- LFS policy,
- largest-fixture budget.

---

# 52. 55B Definition of Done

- [ ] real archives salvaged,
- [ ] preservation receipt,
- [ ] deliberate fixture set,
- [ ] fixture manifest,
- [ ] export/sanitize script,
- [ ] privacy gate,
- [ ] LFS policy,
- [ ] load-every-fixture gate,
- [ ] cross-version continuation matrix,
- [ ] structural fuzzing,
- [ ] recoverable-corrupt fixture,
- [ ] real-vs-synthetic reporting,
- [ ] largest-fixture budget,
- [ ] SAVE_CORPUS.md,
- [ ] deliberate failure proof,
- [ ] CI tiering,
- [ ] compatibility matrix linked to fixtures.

---

# 53. Workstream 55C — Long-Session Robustness

## Goal

Make a 200-hour/mature campaign a tested lifecycle scenario: resumable, leak-free, focus-safe, memory-bounded, and still legible.

---

# 54. 55C Phase A — Resume Contract

Create:

```text
docs/qa/RESUME_CONTRACT.md
```

After load:

- campaign authorities are current,
- open/reopened panels show loaded state,
- focus belongs to live controls,
- guidance state is correct,
- ambience reflects loaded world,
- event subscriptions point to new session,
- no old session callback survives.

---

# 55. 55C Phase B — Resume Phases

Define ordered lifecycle:

```text
teardown old
→ deserialize
→ construct authorities
→ restore state
→ wire ports
→ validate ports
→ rebind UI
→ restore focus/route
→ resolve ambience
→ resume gameplay
```

Coordinate Plan 28C.

---

# 56. 55C Phase C — 50× Panel Cycle

For every live panel:

```text
open
→ interact minimally
→ close
```

repeat 50 times.

Measure:

- node count,
- signal/handler count,
- allocation delta,
- focus target validity.

---

# 57. 55C Phase D — Baseline Restoration Assertion

After each cycle or batch:

```text
node_count <= baseline + allowed framework noise
handler_count == baseline
```

No monotonic increase.

---

# 58. 55C Phase E — Freed-Control Focus Guard

On:

- panel close,
- New Game,
- Load,
- route replacement,

assert no focus owner references freed Godot object.

Add regression fixture for known crash class.

---

# 59. 55C Phase F — Subscription Bag Enforcement

All live panel/session subscriptions should use canonical disposable/subscription bag.

Scan for direct unmanaged subscriptions in scoped live UI.

---

# 60. 55C Phase G — Session-Swap Rebind Sweep

Run one ordered rebind pass.

Port report must show:

```text
required bound
missing = 0
fallbacks expected only
```

after New Game and Load.

---

# 61. 55C Phase H — Authority Identity Check

For every live panel/read model:

```text
ReferenceEquals(panel authority, current campaign authority)
```

or equivalent stable ownership assertion.

No stale session.

---

# 62. 55C Phase I — Ambience Resume

After loading fixture:

- ambience controller resolves loaded position/weather/power/occupancy,
- old loops are stopped,
- new loops start exactly once.

This validates Plan 52A resume semantics.

---

# 63. 55C Phase J — Guidance Resume

Plan 17B guidance overlay:

- retains/persists correct onboarding state,
- does not reopen unexpectedly,
- routes remain valid.

---

# 64. 55C Phase K — Idle/Blur/Minimize

Test:

- window blur,
- minimize,
- background idle.

Assert:

- no day advancement,
- no duplicate save,
- no duplicated audio layers,
- no event spin.

---

# 65. 55C Phase L — Close-Request Path

Cover:

```text
NotificationWMCloseRequest
```

against active/idle/save states.

No double-save.
No hanging close.

---

# 66. 55C Phase M — Memory Ceiling

Across long soak measure:

- managed heap,
- Godot native memory,
- textures/resources if queryable,
- node count,
- handler count.

Define slope budget.

---

# 67. 55C Phase N — Allocation Attribution

Report top growing allocation owners.

Prioritize known candidates:

- per-tick list creation,
- modifier stacks,
- journal rebuilds,
- panel rebind caches,
- route estimates,
- worn-gear collectors.

Do not optimize blindly.

---

# 68. 55C Phase O — Save Corpus Resumability

For each 55B valid fixture:

```text
load
→ validate
→ advance 3 days
→ exercise one player action
→ save
```

No error.

---

# 69. 55C Phase P — Failure Injection at Load

Cases:

- missing section,
- unknown ID,
- oversized log,
- broken slot root,
- corrupted settings,
- unavailable asset/data pack where recoverable.

Expected:

- documented error/recovery surface,
- no half-loaded campaign,
- safe return to menu/recovery path where appropriate.

---

# 70. 55C Phase Q — Half-Loaded Shelter Guard

Define one atomic session-load readiness flag.

UI/gameplay cannot resume until:

```text
deserialize complete
restore complete
ports bound
validation passed
```

If failure:

```text
new session not exposed
```

---

# 71. 55C Phase R — Shared Long-Haul Harness

Reuse 55A/39B metrics.

Do not create three different soak engines.

One harness produces:

- cost slope,
- save-size slope,
- memory slope,
- node/handler stability,
- resumability metrics.

---

# 72. 55C Phase S — Mature Campaign Fixture

Use a day/year ~250 campaign state.

If exact “day 250” is too early for generational maturity, use a source-defined mature year fixture.

The point is:

```text
heavy accumulated history
```

---

# 73. 55C Phase T — Three-Hour Mature Playtest

Manual/structured session:

- resume mature save,
- inspect history/journal,
- make assignments,
- run expedition,
- handle death/policy,
- save/load,
- inspect roll-ups.

Observe:

- missing context,
- over-summary,
- unreadable history,
- confusing compaction,
- sluggish surfaces.

---

# 74. 55C Phase U — Retention UX Review

A technically correct roll-up can still harm game memory.

Ask during mature playtest:

```text
Did the player lose the ability to understand why this relationship changed?
Can they still inspect meaningful memorial history?
Do old decrees remain understandable?
Does the journal feel amputated?
```

Feed findings back into 55A policy.

---

# 75. 55C Phase V — Long-Session Checklist

Create:

```text
docs/qa/LONG_SESSION_CHECKLIST.md
```

Automated rows:

- soak,
- node/handler,
- focus,
- memory,
- corpus resume,
- load failure recovery.

Manual rows:

- mature campaign legibility,
- retention meaning,
- subjective load/resume confidence.

---

# 76. 55C Phase W — Nightly Gate

Register:

```text
long-session gate
```

Nightly:

- representative long soak,
- panel cycle,
- fixture resume sample.

Full 400-year:

- scheduled/release cadence if too expensive nightly.

---

# 77. 55C Phase X — Release Report Integration

Include:

```text
SAVE_SIZE_MAX
SAVE_SIZE_SLOPE
LOAD_TIME_MAX_FIXTURE
MEMORY_SLOPE
NODE_GROWTH
HANDLER_GROWTH
FIXTURES_RESUMABLE
LONG_SESSION_GATE
```

---

# 78. 55C Tests

- resume contract,
- 50× cycle,
- node baseline,
- handler baseline,
- focus/freed-node guard,
- session rebind,
- authority identity,
- ambience resume,
- idle/minimize,
- close request,
- memory ceiling,
- corpus 3-day resume,
- half-loaded session prevention,
- failure-injection recovery.

---

# 79. 55C Definition of Done

- [ ] resume contract,
- [ ] ordered resume lifecycle,
- [ ] 50× all-live-panel cycle,
- [ ] node baseline stable,
- [ ] handler baseline stable,
- [ ] freed-control focus test,
- [ ] canonical subscription handling,
- [ ] session-swap rebind complete,
- [ ] authority identity correct,
- [ ] ambience resumes,
- [ ] guidance resumes,
- [ ] idle/background safe,
- [ ] close request safe,
- [ ] memory ceiling,
- [ ] allocation attribution,
- [ ] every corpus fixture resumable,
- [ ] failure-injection recovery,
- [ ] no half-loaded shelter,
- [ ] shared harness,
- [ ] mature-campaign fixture,
- [ ] three-hour mature playtest,
- [ ] retention UX review,
- [ ] LONG_SESSION_CHECKLIST,
- [ ] release/nightly metrics.

---

# 80. Integrated Long-Haul Pipeline

```text
persisted collection
      │
      ▼
RetentionPolicy
      │
      ├─ preserve
      ├─ bound detail
      └─ summarize
      │
      ▼
versioned save migration
      │
      ▼
bounded campaign save
      │
      ├──────────► 400-year soak
      │
      └──────────► corpus fixture
                         │
                         ▼
                 load / migrate / continue
                         │
                         ▼
                 session lifecycle restore
                         │
                         ▼
                  panel/audio/focus rebind
                         │
                         ▼
                    mature playtest
```

---

# 81. Retention Policy Contract

Every growing collection answers:

```text
Who owns it?
What causes growth?
Is it future-consequence-bearing?
What detail is permanent?
What may be summarized?
What is the recent-detail window?
What is the summary?
What schema version owns the policy?
```

No unanswered collection.

---

# 82. Obligation Preservation Contract

State that affects future outcomes cannot disappear.

Examples:

```text
active deadline
unresolved grief consequence
ending condition
active promise
standing obligation
critical relation landmark
```

If detail is summarized:

```text
future effect remains identical
```

---

# 83. Roll-Up Contract

A roll-up has:

```text
summary version
input domain
canonical order
update function
serialization shape
migration path
```

No hidden lossy string concatenation.

---

# 84. Display-vs-Storage Contract

UI cap:

```text
how much is shown now
```

Storage cap:

```text
how much detail remains persisted
```

These are separately configured and tested.

---

# 85. Save-Size Contract

Publish a budget such as:

```text
size/year
size/century
size at maximal supported campaign horizon
```

Use measured numbers.

---

# 86. Save Corpus Contract

Every supported save version must map to at least one corpus fixture.

Fixture manifest is the evidence source.

---

# 87. Corpus Privacy Contract

Committed save corpus contains no:

- personal absolute paths,
- usernames,
- email,
- machine-specific secrets,
- unrelated user telemetry.

---

# 88. Fixture Provenance Contract

Every fixture answers:

```text
real or synthetic?
captured when?
what version?
what edge case?
what compatibility promise?
```

---

# 89. Fuzz Contract

Fuzz is structural.

It targets documented failure surfaces.

No meaningless random byte noise as primary strategy.

---

# 90. Resumability Contract

Compatibility means:

```text
parse
→ validate
→ migrate
→ bind
→ advance
→ save
```

Not merely parse.

---

# 91. Half-Loaded Session Contract

If load validation fails:

```text
new campaign session never becomes active
```

No panel binds to partially restored state.

---

# 92. Node Lifecycle Contract

For every live panel:

```text
open/close 50×
→ returns to baseline
```

Any baseline exception is documented and bounded.

---

# 93. Handler Lifecycle Contract

Signal/event subscription count after cycle must equal initial count.

No duplicate listeners.

---

# 94. Focus Contract

No focus owner may reference:

- queued-for-free,
- freed,
- old-session control.

Fallback focus target must be valid.

---

# 95. Memory Contract

Long-run memory is judged by growth slope.

A high but flat steady state may be acceptable.
A low but monotonic leak is not.

---

# 96. Ambience Resume Contract

Loaded acoustic state derives from loaded campaign state.

No stale pre-load audio layer remains.

---

# 97. Mature-Game UX Contract

Retention is acceptable only if mature campaign history remains understandable.

A passing save-size gate does not override human legibility findings.

---

# 98. CI Tier Contract

Fast:

- load all fixtures,
- retention unit tests,
- migration checks.

Nightly:

- fuzz,
- continuation matrix sample,
- panel cycles,
- medium long-run soak.

Release/periodic:

- full 400-year soak,
- maximal fixture budget,
- complete long-session checklist.

---

# 99. Release Report Contract

Release report adds:

```text
RETENTION_UNPOLICIED_COLLECTIONS
SAVE_SIZE_MAX
SAVE_SIZE_SLOPE
CORPUS_FIXTURE_COUNT
CORPUS_REAL_PLAY_FIXTURES
CROSS_VERSION_CONTINUE_FAILURES
MAX_FIXTURE_LOAD_MS
PANEL_NODE_GROWTH
PANEL_HANDLER_GROWTH
MEMORY_GROWTH_SLOPE
RESUME_FAILURES
```

---

# 100. Failure Modes

## Global cap silently removes a promise

Critical.
Restore consequence-bearing state and define per-collection policy.

## Roll-up changes future behavior

Fix summary semantics/migration.

## UI display cap truncates storage

Separate presentation/persistence.

## Legacy save is truncated during load without notice

Fail migration contract.

## Save corpus committed with personal paths

Privacy gate fails.

## Fixture parses but crashes after day advance

Resumability gate fails.

## 400-year save remains bounded but load time explodes

Add/load-time budget investigation.

## Panel node count creeps +1 per cycle

Lifecycle leak.

## Handler count returns wrong after Load

Session rebind/subscription leak.

## Focus points at old panel after load

Freed-object guard fails.

## Soak passes but mature history is incomprehensible

Retention policy fails UX review.

---

# 101. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| salvage corpus lost before commit | Medium | Critical | urgent preservation preflight |
| retention caps remove meaningful history | Medium | Critical | semantic inventory + mature playtest |
| migrations alter checksum unexpectedly | High | Medium | explicit schema versions/digests |
| 400-year soak too expensive | Medium | Medium | tiered cadence + shared harness |
| corpus bloats repo | Medium | Medium | LFS + curated fixtures |
| fixture privacy leak | Low–Med | High | sanitization/redaction gate |
| long logs hide O(n²) paths | Medium | High | slope assertions |
| UI cycle tests expose many leaks | Medium | Medium | pilot then sweep |
| native memory measurement noisy | Medium | Medium | slope + broad tolerance |
| retention summaries become unreadable | Medium | High | player-facing review |
| old saves have malformed edge combinations | High | Medium | exactly why corpus is valuable |

---

# 102. Commit Strategy

## Urgent Preservation Commit

### C2[24].0 — Save corpus salvage receipt

- raw inventory,
- hashes,
- sanitized candidate copy,
- no gameplay change.

This may precede 55A implementation because loss risk is independent.

---

## 55A

### C2[24].1 — retention inventory + semantic classes

### C2[24].2 — RetentionPolicy / RollingLog primitives

### C2[24].3 — low-risk historical-log migration

### C2[24].4 — nutrition/dose migration

### C2[24].5 — journal/memorial migration

### C2[24].6 — census/register/pair-history migration

### C2[24].7 — faction/location/generation migration

### C2[24].8 — legacy oversize migration diagnostics

### C2[24].9 — save-size/load budgets + 400-year slope

### Gate: 55A complete

---

## 55B

### C2[24].10 — fixture manifest + corpus structure

### C2[24].11 — export/sanitize tooling

### C2[24].12 — historical real-save fixtures + LFS

### C2[24].13 — load-every-fixture gate

### C2[24].14 — cross-version continuation matrix

### C2[24].15 — structural fuzz + recovery fixtures

### C2[24].16 — largest-fixture budget + docs

### Gate: 55B complete

---

## 55C

### C2[24].17 — resume contract + lifecycle ordering

### C2[24].18 — 50× panel/node/handler gate

### C2[24].19 — focus/freed-object/session rebind safety

### C2[24].20 — ambience/guidance/idle/close resume

### C2[24].21 — memory/allocation slope instrumentation

### C2[24].22 — corpus resumability + load failure injection

### C2[24].23 — mature-campaign playtest + UX feedback

### C2[24].24 — nightly/release long-session closure

### Gate: 55C complete

---

# 103. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
bash scripts/ci/lfs-health-check.sh
bash scripts/ci/verify-fast.sh
```

Add/run repository-canonical equivalents of:

```text
retention selftest
save-corpus load-all gate
cross-version continue-play gate
save corpus structural fuzz
400-year growth soak
50× live-panel node/handler lifecycle gate
corpus resumability sweep
mature-campaign long-session checklist
```

---

# 104. Flagship Definition of Done

## Urgent Salvage

- [ ] real archive source inventoried,
- [ ] raw hashes captured,
- [ ] preservation copy created,
- [ ] privacy scan completed,
- [ ] salvage receipt committed,
- [ ] no destructive cleanup before preservation.

## 55A — Retention

- [ ] every growing persisted collection inventoried,
- [ ] per-collection semantic class,
- [ ] per-collection policy,
- [ ] Core retention primitives,
- [ ] deterministic roll-ups,
- [ ] display/storage separation,
- [ ] obligations preserved,
- [ ] one-at-a-time migrations,
- [ ] store schema bumps,
- [ ] oversized legacy migration notice,
- [ ] save-size ceiling,
- [ ] load/save budget,
- [ ] 400-year soak,
- [ ] growth-slope assertions,
- [ ] retention selftest,
- [ ] RETENTION.md.

## 55B — Save Corpus

- [ ] historical archives salvaged,
- [ ] fixture set curated,
- [ ] MANIFEST.json,
- [ ] sanitization/export script,
- [ ] no personal paths,
- [ ] LFS policy,
- [ ] load-every-fixture gate,
- [ ] supported-version matrix,
- [ ] 30-day continuation,
- [ ] structural fuzz,
- [ ] recoverable corruption fixture,
- [ ] real-vs-synthetic coverage,
- [ ] maximal fixture,
- [ ] cold-load budget,
- [ ] deliberate gate-failure fixture,
- [ ] SAVE_CORPUS.md.

## 55C — Long Session

- [ ] resume contract,
- [ ] ordered session restore,
- [ ] 50× panel cycles,
- [ ] node baseline restored,
- [ ] handler baseline restored,
- [ ] freed-focus guard,
- [ ] session rebind complete,
- [ ] panel authority identity correct,
- [ ] ambience resume,
- [ ] guidance resume,
- [ ] idle/minimize safe,
- [ ] close-request safe,
- [ ] memory slope bounded,
- [ ] allocations attributable,
- [ ] every corpus fixture resumes ≥3 days,
- [ ] load-failure recovery clean,
- [ ] no half-loaded shelter,
- [ ] one shared soak harness,
- [ ] mature-campaign fixture,
- [ ] 3-hour mature-campaign playtest,
- [ ] retention UX reviewed,
- [ ] LONG_SESSION_CHECKLIST,
- [ ] release/nightly reporting.

## Global

- [ ] no lost obligation/promise/ending fact,
- [ ] no silent truncation,
- [ ] no personal data in fixtures,
- [ ] no un-LFS’d corpus blob,
- [ ] no bulk retention rewrite,
- [ ] no stale session/focus handler,
- [ ] no claim that automated soak alone proves long-game quality,
- [ ] full verification green.

---

# 105. Closure Report Template

```markdown
## C2[24] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Urgent Corpus Salvage
- Source archives:
- Source bytes:
- Earliest archive:
- Latest archive:
- Raw hashes:
- Privacy hits:
- Sanitized candidates:
- Receipt:
- Result:

### 55A — Retention
- Growing collections:
- Policied collections:
- Unpolicied collections:
- Obligation collections:
- Roll-up policies:
- Store schema bumps:
- Legacy migrations:
- Day-10 save bytes:
- Day-400-year-equivalent save bytes:
- Save-size slope:
- Early day cost:
- Late day cost:
- Cost slope:
- Result:

### 55B — Save Corpus
- Fixture count:
- Real-play fixtures:
- Synthetic fixtures:
- Legacy schema versions:
- LFS tracked:
- Privacy failures:
- Load-all:
- Cross-version:
- 30-day continuation:
- Corruption recovery:
- Fuzz:
- Max fixture bytes:
- Max fixture load ms:
- Result:

### 55C — Long Session
- Live panels:
- Panel cycles:
- Node growth:
- Handler growth:
- Freed-focus failures:
- Session-rebind failures:
- Ambience resume:
- Guidance resume:
- Idle/background:
- Close-request:
- Managed-memory slope:
- Native-memory slope:
- Resumable fixtures:
- Load-failure recovery:
- Mature playtest:
- Retention UX findings:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Save/load UI failure:
- Retention:
- Corpus:
- Cross-version:
- Fuzz:
- 400-year soak:
- Panel cycle:
- LFS:
- Verify fast:

### Final Metrics
- RETENTION_GROWING_COLLECTIONS:
- RETENTION_UNPOLICIED:
- OBLIGATION_LOSS:
- SAVE_SIZE_MAX:
- SAVE_SIZE_SLOPE:
- CORPUS_FIXTURES:
- REAL_PLAY_FIXTURES:
- CROSS_VERSION_FAILURES:
- MAX_FIXTURE_LOAD_MS:
- PANEL_NODE_GROWTH:
- PANEL_HANDLER_GROWTH:
- FREED_FOCUS_FAILURES:
- MEMORY_GROWTH_SLOPE:
- RESUME_FAILURES:

### Remaining Debt
- Retention:
- Migration:
- Corpus:
- Lifecycle:
- Memory:
- Mature-game UX:
```

---

# 106. Final Execution Directive

Execute Plan 55 as a **long-haul persistence and lifecycle** repair.

The critical sequence is:

```text
salvage the unique real save archives immediately
→ inventory every growing persisted collection
→ define a per-collection retention policy
→ migrate one collection at a time
→ measure and gate save-size/cost slope
→ commit a sanitized versioned save corpus
→ load/migrate/continue every supported fixture
→ fuzz structural failure modes
→ cycle every live panel 50×
→ prove focus/session/audio rebinding
→ bound memory growth
→ resume mature saves
→ playtest the mature campaign for legibility
```

Do not cap history before understanding what future mechanics depend on it.

Do not delete a promise, obligation, grief consequence, relation landmark, or ending condition to save bytes.

Do not let a real historical save disappear because it lived outside the repo.

Do not call a save compatible because it parses once.

Do not call the long game healthy merely because a headless soak passes.

The strongest retention rule is:

> **Nothing grows without a stated policy, and nothing consequence-bearing disappears without its meaning being preserved.**

The strongest compatibility rule is:

> **Every save format ASHFALL has actually written becomes a named, sanitized fixture that loads, migrates, resumes, and remains part of CI.**

The strongest long-session rule is:

> **A mature campaign must remain bounded, resumable, focus-safe, leak-free, and understandable — both to the machine and to the player.**

The flagship acceptance scenario is:

> **Load the oldest real corpus fixture, migrate it through the current retention policies, continue it for 30 days, save/reload it, then run the same campaign state through repeated panel/session cycles and a long soak. The save must stay within the measured growth ceiling, no obligation may disappear, every live panel must rebind to the current session without node/handler/focus leaks, and the mature campaign must still present enough preserved history for a player to understand what happened and why.**
