# C1 — Flagship Integration Plan [3]: Ending Continuity, Generational State & Repository Truth

> **Output:** `C1_planintegration[3].md`
>
> **Source baseline:** Plan 19 — Ending Continuity: The Campaign Must Compute Its Own Ending
>
> **Wave:** Continuity Wave 1 — closing plan
>
> **Dependencies:** 15A/15B for meaningful choices; 18B for real runtime state.
>
> **Priority rule:** Task 19A is a release blocker and must execute first.
>
> **Primary execution order:** 19A → 19C hygiene steps 1–4 → 19B → remainder of 19C
>
> **Scope guardrail:** no new ending branch, no new epilogue prose, no new panel, no new matrix. Connect existing state to existing authored outcomes.

---

## 0. Mission

A long-form survival campaign is only continuous if the ending is a faithful projection of the campaign that preceded it.

The source evidence identifies a critical discontinuity:

- the epilogue accepts real branching inputs;
- gameplay authorities capable of deciding those inputs already exist;
- both player-facing epilogue entry points currently pass literals instead;
- therefore important branches are structurally unreachable and campaigns collapse toward the same ending.

Plan 19 closes three kinds of continuity:

1. **Ending continuity** — the epilogue is derived from saved campaign state.
2. **Generational continuity** — children, memorials, ageing, and legacy affect actual play and ending state.
3. **Knowledge continuity** — repository sidecars, audits, authority docs, and CI truth accurately describe current source.

---

## 1. Release-Blocking Invariants

### INV-19.1 — The ending is a projection, never an input form

Production code must not pass hand-authored booleans into the epilogue.

Required shape:

```text
saved campaign authorities
        ↓
EpilogueContextInputs
        ↓
EpilogueContextFactory
        ↓
EpilogueEvaluationContext
        ↓
EpilogueMatrixRuntime
        ↓
authored narrative
```

### INV-19.2 — One definition for each ending fact

If `VerdictEndingEvaluator` already defines "Tempest decommissioned," reuse it. Do not fork the rule.

### INV-19.3 — Every ending fact is persisted or derivable from persisted state

A fact that exists only in RAM cannot decide a loaded game's ending.

### INV-19.4 — All matrix branches are demonstrably reachable

Reachability is tested from campaign state, not by constructing synthetic context booleans.

### INV-19.5 — Multi-year state has at least one gameplay cost, one future payoff, and ending relevance

For children, the source task deliberately limits implementation to exactly three links:

- mouths to feed;
- schoolable/apprenticeable population;
- `childrenSurvived` ending fact.

### INV-19.6 — Repo truth is gated

Dangling sidecars and stale audit claims may not silently accumulate.

### INV-19.7 — No content expansion before integration closure

Do not add ending prose or branches while existing permutations are unreachable.

---

## 2. Definition of Done

Plan 19 is complete only when:

- both epilogue entry points consume the same derived context;
- no production source contains literal epilogue booleans/death count;
- `EpiloguePanel.Bind` no longer requires a positional boolean bundle;
- each ending fact maps to an authoritative campaign source;
- `totalDeathsRecorded` equals the campaign memorial/death ledger;
- branch-reachability tests cover every conditional path in `EpilogueMatrixRuntime`;
- three seeded 200-day policy runs produce at least three distinct valid endings;
- save → quit → load → game over preserves identical derived context for identical state;
- children affect rations, schooling/apprenticeship eligibility, maturity-to-work, and the ending;
- memorial records preserve meaningful death distinctions;
- dangling `.cs.uid` count is zero under CI gate;
- the duplicate `.claude/worktrees/` tree is ignored repo-wide;
- named stale documentation claims are reconciled;
- a session-continuity CI journey catches both ending hardcoding and throwaway-authority regressions.

---

# PHASE P0 — Evidence Freeze and Safety Preconditions

## P0.1 Record repository state

Before changing files, capture:

```text
commit SHA
branch
git status --porcelain count
current dangling .cs.uid count
current ending bind call sites
current EpilogueEvaluationContext construction sites
current content-utilization RUNTIME count
current exempt_no_source_evidence count
current live-panel count
```

The source baseline references `ccac926e` and a large dirty tree. Do not assume those numbers remain current.

## P0.2 Protect work in progress

Before 19A if the tree is still heavily modified:

1. categorize changes by system;
2. commit coherent finished work in small commits;
3. do not create one "everything" commit;
4. ensure no untracked generated files are accidentally included;
5. preserve the user's current working changes.

This is a prerequisite because endgame integration touches high-centrality host files.

## P0.3 Read the exact ending pipeline

Read completely:

- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`
- epilogue evaluation context definition
- `src/UI/EpiloguePanel.cs`
- `src/Main.GameFlow.cs`
- `src/Main.PlayerSurfaces.cs`
- `src/Host/ExpansionHostSession.cs`
- `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs`
- `Assets/Ashfall.Core/RegionalTreatySystem.cs`
- `Assets/Ashfall.Core/LedgerDebtSystem.cs`
- `Assets/Ashfall.Core/CohortSystem.cs`
- memorial/death ledger code
- `EvidenceLedger`
- `ReckoningSystem`
- `IFlagLedger`
- save registry and sections for all relevant authorities.

## P0.4 Write the ending fact ownership table

Before implementation, publish:

| Epilogue field | Authoritative source | Persisted section | Fallback allowed? | Rule owner |
|---|---|---|---|---|
| `days` | campaign day | campaign/world | no | campaign clock |
| `livingDwellers` | survivor roster | survivor save | no | roster |
| `deathsRecorded` | memorial/death ledger | memorial/survivor state | no | death pipeline |
| `grandTreatySigned` | treaty authority | treaty/flags | sanctioned flag only if needed | treaty |
| `tempestDecommissioned` | Verdict evaluator + Reckoning state | verdict state | no duplicate rule | Verdict |
| `debtLedgersBurned` | debt/ledger authority | debt section | sanctioned flag only if needed | debt |
| `childrenSurvived` | cohort authority | cohort/dose section | no literal | cohort |
| `velSecretExposed` | evidence/reckoning | verdict/evidence | sanctioned flag only if needed | Verdict |

No coding shortcut may bypass this table.

---

# TASK 19A — Derive the Ending from the Campaign

## 19A.0 Severity

This is the highest-priority task in this plan.

Do not begin 19B feature-deepening while production epilogue routes still pass literal values.

## 19A.1 Failing proof first

Add an end-to-end-ish host test:

Campaign A:
- low deaths;
- no treaty;
- Tempest active;
- ledgers intact;
- no surviving children;
- secret hidden.

Campaign B:
- different persisted outcomes.

Call the same production context accessor.

Assert:

```text
contextA != contextB
```

Also prove both current epilogue entry points would have yielded identical literals before the fix. Preserve the test as a regression test after implementation.

## 19A.2 Recreate `EpilogueContextFactory.cs`

Land the missing source file corresponding to the orphaned sidecar only because the integration layer is actually needed.

Recommended Core types:

```csharp
public sealed record EpilogueContextInputs(
    int Days,
    int LivingDwellers,
    int DeathsRecorded,
    bool GrandTreatySigned,
    bool TempestDecommissioned,
    bool DebtLedgersBurned,
    bool ChildrenSurvived,
    bool VelSecretExposed,
    IReadOnlyList<string> SourceIds);
```

And:

```csharp
public static class EpilogueContextFactory
{
    public static EpilogueEvaluationContext Build(EpilogueContextInputs inputs);
}
```

Constraints:

- no Godot types;
- no host/session references in Core DTO;
- validate non-negative counts;
- source IDs are traceability metadata, not rule inputs;
- no RNG;
- pure mapping.

## 19A.3 Build one host-side input collector

Do not let `Main.GameFlow` and `Main.PlayerSurfaces` independently gather facts.

Create one host accessor, e.g.:

```csharp
private EpilogueEvaluationContext BuildCurrentEpilogueContext()
```

or a small projector service.

Its responsibility:

1. read campaign owners;
2. compute each factual input using sanctioned rule owners;
3. build `EpilogueContextInputs`;
4. call Core factory;
5. return context.

## 19A.4 Derive `deathsRecorded`

Authority rules:

- use memorial/death ledger;
- if the death event records memorial entries, count the authoritative persisted record;
- do not count current missing roster slots;
- do not infer deaths from initial roster minus living roster;
- do not use a local session counter unless persisted as the authoritative ledger.

Tests:

- zero deaths → 0;
- one death → 1;
- 51 deaths → >50 branch is reachable;
- save/load preserves exact count;
- memorial count and epilogue count agree.

## 19A.5 Derive `tempestDecommissioned`

Reuse:

```text
VerdictEndingEvaluator.IsTempestDecommissioned(ReckoningState)
```

Do not duplicate state predicates in the factory or host.

Tests:
- state satisfying evaluator → true;
- non-satisfying → false;
- factory result exactly follows evaluator;
- saved Reckoning state reproduces same result after load.

## 19A.6 Derive `grandTreatySigned`

Preferred order:

1. direct `RegionalTreatySystem` state;
2. existing explicit treaty outcome;
3. sanctioned persisted flag if architecture requires it.

If flags are used:
- use a cataloged `flag_...` ID;
- validate via catalog integrity;
- document why direct authority was insufficient.

Do not combine unrelated flags heuristically without a named rule owner.

## 19A.7 Derive `debtLedgersBurned`

Read `LedgerDebtSystem`.

Clarify exactly which persisted state counts as "burned."

If source semantics are ambiguous:
- define one named method on the authority;
- unit-test it;
- have epilogue consume that method.

Do not interpret UI labels.

## 19A.8 Derive `childrenSurvived`

Read `CohortSystem`.

For 19A, implement only the truthful read.

Possible rule:
- one or more cohort children alive at ending;
- or a richer existing Cohort definition if source already supplies it.

Do not pre-implement 19B design here.

## 19A.9 Derive `velSecretExposed`

Use `EvidenceLedger` / `ReckoningSystem`.

Prefer a named query on the authority over scanning arbitrary evidence IDs in the host.

If no named query exists:
- add a narrowly tested Core query;
- do not hardcode UI state.

## 19A.10 `days` and `livingDwellers`

Even if these are already non-hardcoded, fold them into the same projection pipeline.

Tests:
- day count equals campaign clock;
- living count equals live roster;
- counts remain stable through save/load.

## 19A.11 Replace both production bind paths

Replace:

- game-over bind;
- player-facing epilogue route.

Both must call the same projector/accessor.

Required architectural test:

```text
gameOverContext == routedEpilogueContext
```

for the same campaign state.

## 19A.12 Narrow `EpiloguePanel.Bind`

Preferred:

```csharp
public void Bind(EpilogueEvaluationContext context)
```

Avoid this:

```csharp
Bind(days, living, deaths, treaty, tempest, ledgers, children, vel)
```

If compatibility overload is temporarily retained:
- mark it obsolete/internal;
- ensure production never calls it;
- remove after callers migrate.

## 19A.13 Make condition flips visible during play

For each endgame fact, emit or consume an existing day-event vocabulary:

- `treaty_signed`
- `ledger_burned`
- `tempest_decommissioned`
- `child_born`
- `child_lost`
- `secret_exposed`

Requirements:
- event enters existing briefing/journal path;
- event is not a second source of truth;
- event describes state transition already committed to authority;
- repeat loads do not duplicate historical event unless intended.

## 19A.14 Branch reachability suite

Enumerate every conditional branch in `EpilogueMatrixRuntime`.

For each:
1. record predicate;
2. construct persisted authority state that should satisfy it;
3. build context through production projector;
4. evaluate epilogue;
5. assert branch-specific outcome marker/text.

This includes currently unreachable examples such as:
- `!debtLedgersBurned`;
- `totalDeathsRecorded > 50`;
- combinations involving surviving children;
- Tempest not decommissioned.

Do not instantiate `EpilogueEvaluationContext` directly in reachability tests except factory unit tests.

## 19A.15 Retire production-adjacent demo APIs

Review `EvaluateEpilogueDemo(...)`.

Allowed outcomes:
- delete if redundant;
- keep only behind explicit demo/selftest command;
- rename to make synthetic behavior unmistakable.

Forbidden:
- production UI calling a boolean-taking demo API;
- host convenience method with defaults.

## 19A.16 Save continuity

Journey:

```text
play
→ create ending-relevant state
→ save
→ quit/recreate session
→ load
→ BuildCurrentEpilogueContext
→ game over
```

Assert context equals pre-quit derived context.

If an input fails:
- locate missing persistence;
- fix that authority's save section;
- do not cache the epilogue result to hide the gap.

## 19A.17 Deterministic ending replay

Given:
- same seed;
- same initial state;
- same player choices;
- same sequence of day advances;
- same save/load boundaries;

Require:
- identical context;
- identical selected epilogue branch;
- identical ending text.

If text has intentional nondeterminism, it must use campaign RNG deterministically and be covered by replay.

## 19A.18 Three-policy 200-day proof

Use scripted/telemetry playtest for three distinct policies, e.g.:

A. reconciliation / treaty / low casualty;
B. authoritarian or high-casualty / debt unresolved;
C. anti-Tempest evidence exposure with different cohort outcome.

Do not force exact narrative design; use policies supported by current mechanics.

Acceptance:
- each reaches game over;
- derived contexts differ;
- endings differ;
- logs include source IDs and decisive facts.

### 19A DoD

- production epilogue inputs contain no literals for the eight context fields;
- both entry points use one projector;
- every branch is reachable from campaign state;
- three policy runs yield distinct endings;
- death count matches memorial/death authority.

---

# TASK 19C-A — Immediate Hygiene Before Deeper Work

The source plan places sidecar and duplicate-tree cleanup immediately after 19A because stale repo artifacts distort searches used by 19B/19C.

## 19C-A.1 Sweep dangling `.cs.uid`

Generate a table:

```text
uid_path
expected_source_path
source_exists?
moved_source?
references_to_old_symbol
verdict
```

For each of the named candidates, confirm whether the source moved.

Rule:
- delete only true sidecars with no corresponding source;
- never recreate a class just to satisfy a sidecar;
- exception: `EpilogueContextFactory.cs` is intentionally recreated by 19A because the architecture requires it.

## 19C-A.2 Add CI sidecar gate

Add:

```bash
scripts/ci/uid-sidecar-gate.sh
```

Expected behavior:
- scan `src` and `Assets`;
- find `*.cs.uid`;
- compute sibling `.cs`;
- fail with paths for any missing sibling;
- stable output order;
- exit 0 when clean.

Wire into `verify-fast.sh`.

## 19C-A.3 Ignore duplicate agent worktrees

Add repository-shared ignore:

```gitignore
.claude/worktrees/
```

Do not delete the local tree unless separately desired.

Test:
- fresh `git check-ignore`;
- repo-wide grep excludes duplicate tree under normal tooling assumptions;
- CI clone sees the same ignore rule.

## 19C-A.4 Re-run source inventory

After hygiene:
- rerun class/reference greps;
- rerun dangling sidecar count;
- rerun duplicate authority scan;
- store clean baseline for 19B/remaining 19C.

---

# TASK 19B — Cohort, Memorials and Legacy

## 19B.0 Objective

Make multi-year campaign state affect daily survival and ending continuity without creating a sprawling child-simulation subsystem.

The scope is intentionally limited.

Exactly three cohort links:

1. children consume rations;
2. children form school/apprenticeship population and later work eligibility;
3. cohort outcome decides `childrenSurvived`.

Memorials deepen the death-to-legacy path and provide the authoritative ending death count.

## 19B.1 Inventory actual `CohortSystem`

Before design, document current API/state:

```text
child identity
age
birth/entry
maturation
baseline correction
health/survival fields
events
capture/restore DTO
host owner
save section
```

Do not implement features based on summary names if source APIs differ.

## 19B.2 Child ration factor in data

Define a data-driven child ration fraction, e.g. a tuning field.

Rules:
- no hard-coded fraction scattered through needs code;
- adults and children remain explicit categories;
- ration arithmetic deterministic;
- insufficient food impacts existing needs pipeline.

Tests:
- 0 children;
- 1 child;
- N children;
- mixed adults/children;
- boundary rounding;
- starvation/shortage behavior;
- save/load.

## 19B.3 Ration integration

Use existing shelter ration allocation / needs authority.

Forbidden:
- second child-only hunger system;
- UI-only food display without resource consumption.

Required daily loop:

```text
cohort population
→ ration requirement calculation
→ existing inventory/ration allocation
→ existing hunger/needs consequence
```

## 19B.4 School/apprenticeship eligibility

Use existing education/knowledge systems.

Integrate cohort as population input:

```text
age/state
→ eligible for schooling/apprenticeship
→ capacity allocation
→ existing progression
```

Do not create a new progression model if `ApprenticeshipSystem` / library study already owns it.

Tests:
- under-age not work-eligible;
- school-age eligible;
- capacity limit enforced;
- maturation changes eligibility on correct day;
- save/load retains enrollment/progression where existing system supports it.

## 19B.5 Duty-roster maturity gate

Reuse `DutyRosterSystem`.

Rule:
- cohort maturation determines when a child can become a duty candidate;
- duty system still decides assignment/capacity.

Negative tests:
- force under-age ID into roster → rejected;
- mature child → accepted if other requirements met;
- save/load preserves age and resulting eligibility.

## 19B.6 Remove `sv_cohort_demo`

Replace demo literal in live surface with current selected survivor/cohort entity.

If no live selection:
- render empty/unavailable state;
- do not invent a fallback ID.

Move true demo correction flow into explicit selftest/demo command.

Add a source gate for the literal.

## 19B.7 Memorial continuity

For each death:

```text
death event
→ authoritative death record
→ memorial entry
→ circumstance-specific text selection
→ visible memorial growth
→ epilogue death count
```

Use existing:
- `DeathQuality`;
- `MemorialOutcome`;
- epitaph catalogs;
- wall-carving catalogs;
where actually supported by source.

Do not invent a parallel memorial state.

## 19B.8 Circumstance variance

Tests must prove at least several distinct death circumstances do not collapse to identical memorial metadata when the existing system supports differentiation.

Test examples:
- disease;
- combat;
- exposure;
- starvation;
- other currently modeled death qualities.

Do not fabricate categories absent from source.

## 19B.9 Surface passage of time

Reuse an existing status/shelter surface.

Show at least one concise, live marker such as:
- cohort ages;
- generation count;
- memorial wall count/growth.

No new console.

The goal is perceptual continuity, not dashboard expansion.

## 19B.10 Day-event vocabulary

Emit/route:

- `child_born`
- `child_aged`
- `child_lost`
- `generation_advanced`

Requirements:
- event IDs validated;
- journal/briefing history persists as designed;
- no duplicate events after load/replay.

## 19B.11 Persistence audit

Confirm whether cohort state intentionally lives in the dose ledger section.

If yes:
- document coupling;
- add round-trip tests;
- ensure lifecycle owner restores before consumers.

If no:
- register a proper cohort section using repository save conventions.

Do not casually migrate save ownership in the same commit as gameplay wiring without compatibility tests.

## 19B.12 `childrenSurvived` integration

Production epilogue projection from 19A reads the real cohort result.

Tests:
- no children ever existed;
- children existed but none survived;
- one or more survive;
- save/load before ending;
- context result agrees with cohort authority.

## 19B.13 Three-year balance simulation

Run `ashfall-balance-sim` or current equivalent.

Track:
- adults;
- children;
- total food requirement;
- food per capita;
- schooling capacity;
- work-eligible population;
- mortality;
- final `childrenSurvived`.

Success criteria:
- children impose measurable food pressure;
- population growth is not free;
- maturation creates future labor/knowledge value;
- system does not guarantee collapse under reasonable baseline.

Record policy assumptions.

## 19B.14 Update expansion/context atlas

Re-check underconnected state classification after implementation.

Replace stale labels only when runtime read-set proves it.

### 19B DoD

Children:
- eat;
- learn/become apprentices through existing systems;
- become work-eligible only at correct maturity;
- can be lost and memorialized;
- contribute truthfully to ending evaluation.

---

# TASK 19C-B — Repository Truth and Wave-Close Gates

## 19C-B.0 Objective

Make future agents and CI read the same truth that runtime executes.

## 19C-B.1 Reconcile data audit

For each stale/orphan claim in `DATA_GAP_AUDIT.md`:

- verify current loader;
- verify current consumer;
- classify `WIRED`, `DEAD`, `MOVED`, or genuinely `ORPHAN`;
- attach current source path;
- prefer regeneration from `artifacts/content-utilization.json` where feasible.

Do not edit only the named questline row; sweep the document using current output.

## 19C-B.2 Reconcile silence/audio audit

Correct claims contradicted by source, including death-event availability if still current.

Then:
- rerun audio selftest;
- ensure references point to current path/line concept, not brittle exact line numbers where avoidable;
- separate "event exists" from "audio is wired."

## 19C-B.3 Reconcile canon registry

Remove stale defects no longer present.

For each known-issue entry:
- source scan;
- reproduce if possible;
- mark fixed only with evidence;
- update current counts from source.

## 19C-B.4 Refresh `CURRENT_AUTHORITY.md`

Add wave metrics:

```text
EFFECT_PRODUCED count
exempt_no_source_evidence count
live-panel count
epilogue-derived-context gate
dangling-sidecar count
```

List Plans 15–19 and their authority artifacts.

## 19C-B.5 Working-tree discipline

If the tree remains large:
- partition by feature;
- commit in dependency order;
- do not mix generated snapshots with Core logic unless required;
- preserve bisectability.

Suggested Plan 19 commit series:

```text
19A-1 failing ending projection tests
19A-2 EpilogueContextFactory + host projector
19A-3 production bind migration + branch reachability
19A-4 save/replay/200-day acceptance
19C-a sidecar gate + ignore rule
19B-1 cohort ration integration
19B-2 education/duty integration
19B-3 memorial/legacy + ending cohort read
19C-b audit reconciliation
19C-c session-continuity journey + CI wave close
```

## 19C-B.6 Session-continuity journey test

This is a critical cross-plan regression test.

Journey:

1. New game.
2. Take an action that changes a campaign authority.
3. Produce at least one briefing entry.
4. Produce an ending-relevant fact.
5. Save.
6. Quit/reconstruct session.
7. Load.
8. Open the corresponding panel.
9. Assert panel values equal restored campaign authority.
10. Assert briefing history survived.
11. Build epilogue context.
12. Assert ending facts survived.
13. Advance to game over or invoke production endgame route.
14. Assert epilogue reflects restored state.

This should catch:
- Plan 16 throwaway authority;
- Plan 19 hardcoded ending;
- missing persistence;
- stale panel session;
- briefing discontinuity.

## 19C-B.7 CI proof against all-true regression

Create a scripted save/campaign with values deliberately different from the old all-true/zero-death constants.

Load through production code.

Assert:
- derived context matches expected non-default values;
- epilogue selects the expected branch or stable identifying text.

Prefer stable semantic outcome IDs if available over brittle full-paragraph text. If only text exists, pin the smallest stable unique phrase allowed by project testing conventions.

## 19C-B.8 Content-utilization evidence

After new runtime reads:
- rerun content utilization;
- verify runtime evidence increases where expected;
- verify exemptions decrease where expected;
- investigate surprising changes.

Do not force metric movement by adding fake reads.

## 19C-B.9 Wave-close metric table

Record before/after:

| Metric | Before | After |
|---|---:|---:|
| hardcoded epilogue inputs | 2 routes | 0 |
| unreachable epilogue branch predicates due to constants | >0 | 0 |
| dangling `.cs.uid` | baseline | 0 |
| duplicate worktree ignore shared | no | yes |
| cohort gameplay links | underconnected | 3 scoped links |
| session-continuity journey | absent | passing |
| content runtime evidence | baseline | measured after |
| stale audited claims | baseline | 0 known in reconciled docs |

## 19C-B.10 Final wave verification

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/uid-sidecar-gate.sh
bash scripts/ci/verify-fast.sh
```

And:
- three seeded 200-day policies;
- save/load endgame journey.

### 19C DoD

- zero dangling sidecars under gate;
- duplicate worktree ignored repository-wide;
- named audit docs reconciled against current source;
- session-continuity CI journey passes;
- CI fails if epilogue regresses to hardcoded defaults.

---

# 3. Cross-Task Dependency Graph

```text
15A / 15B ──────────────┐
                        │
18B ────────────────────┼──► 19A: derive ending from real campaign state
                        │            │
                        │            ├──► immediate 19C hygiene
                        │            │
                        │            └──► 19B: deepen cohort/memorial legacy
                        │                         │
                        └─────────────────────────┴──► 19C wave-close truth gates
```

Plan-local execution:

```text
19A
 ↓
19C steps 1–4
 ↓
19B
 ↓
19C remainder
```

Wave-level order from source:

```text
15A
→ 16A
→ 15C
→ 16B
→ 19A
→ 16C
→ 17A
→ 17C
→ 17B
→ 18A
→ 18B
→ 19B
→ 18C
→ 19C
```

If only three highest-leverage tasks are possible:
- 15A;
- 19A;
- 16A.

---

# 4. Epilogue Fact Traceability Requirements

Every ending input should be explainable in a debug/task log.

Recommended debug representation:

```text
Epilogue fact: deathsRecorded
value: 17
authority: MemorialLedger
save section: memorial
source ids: [...]
derived at day: 200
```

Do not expose internal debug identifiers to normal player UI unless useful.

For each boolean:
- value;
- authority;
- rule method;
- persisted source;
- last transition event.

This makes ending disputes diagnosable.

---

# 5. Branch Reachability Matrix

Create a test artifact listing every branch in `EpilogueMatrixRuntime`.

Example shape:

| Branch | Predicate | Authority state recipe | Reachable through production projector? | Test |
|---|---|---|---|---|
| high-death Tempest path | `!tempest && deaths > 50` style predicate per source | persisted Reckoning + 51 deaths | yes | named test |
| children settlement path | living ≥ threshold + children survived | cohort + roster | yes | named test |
| burned-ledger child path | burned + children | debt + cohort | yes | named test |
| unburned-ledger path | `!debtLedgersBurned` | debt state | yes | named test |

Use actual predicates from source when implementing.

A branch is not considered reachable if the test directly passes booleans into the context.

---

# 6. Save Compatibility Strategy

Do not version the epilogue context itself as a new persisted section unless architecture requires it.

Preferred:
- persist underlying facts;
- derive context at game over.

For older saves:
- if an authority field did not exist, use existing migration/default semantics;
- document defaults;
- avoid pretending an unavailable historical fact occurred.

If a new flag is required:
- add it to data authority;
- add migration/default;
- integrity validate ID;
- ensure old saves load deterministically.

---

# 7. Failure Injection

### N19.1 No memorial section
Expected: save/load test fails clearly; do not return deaths = 0 silently.

### N19.2 Missing treaty authority
Expected: descriptive missing-authority error in development or sanctioned flag path, not `true`.

### N19.3 Old all-true bind reintroduced
Expected: source/CI test fails.

### N19.4 Direct synthetic context in branch reachability test
Expected: test review/gate disallows it for integration suite.

### N19.5 Under-age duty assignment
Expected: rejected by existing duty system integration.

### N19.6 Child ration coefficient absent
Expected: data validation failure or explicit fallback documented in tuning authority; no magic scattered value.

### N19.7 Dangling UID added
Expected: `uid-sidecar-gate.sh` fails and prints path.

### N19.8 Duplicate worktree appears in grep
Expected: repository ignore/tooling excludes it after checkout.

### N19.9 Loaded save changes ending
Same save, no new action:
Expected: derived context identical before quit and after load.

---

# 8. Determinism and Replay Contract

The following must be stable:

```text
same seed
+ same choices
+ same sequence of day advances
+ same save/load points
= same epilogue context
= same branch
= same ending output
```

If memorial text variants or epilogue text variants use RNG:
- use campaign RNG;
- capture required RNG state;
- test paired replay.

Do not seed from:
- wall clock;
- `GetHashCode`;
- UI open time;
- session object identity.

---

# 9. Performance Guardrails

Ending derivation runs at low frequency, so optimize for correctness and traceability, not micro-performance.

Still require:
- no scanning hundreds of files/JSON at game over;
- authorities already loaded;
- no catalog reparsing;
- no expensive UI tree walk to derive facts;
- no mutation during projection.

`BuildCurrentEpilogueContext()` must be observational.

Cohort daily calculations should:
- scale with cohort size;
- avoid repeated LINQ allocations in hot day-loop code if existing style avoids them;
- reuse existing population/needs aggregations where available.

---

# 10. UI/UX Integration Rules

No new panels.

### Epilogue
- panel receives finalized context;
- panel may display debug provenance only in developer mode;
- normal player narrative remains authored matrix output.

### During play
Use existing:
- daily briefing;
- journal;
- shelter/status surfaces.

Ending-relevant transitions should be legible before game over so the outcome feels earned.

### Multi-year continuity
One visible marker is enough if it clearly communicates:
- ages/generation;
- memorial accumulation;
- long-term change.

Do not create a second campaign dashboard.

---

# 11. Documentation Truth Protocol

For each audit document touched:

1. generate fresh source evidence;
2. update claim;
3. include current path;
4. mark status;
5. avoid retaining false historical defect as active;
6. if useful, preserve historical note explicitly labeled resolved.

Do not rely on exact line numbers as permanent identity when a symbol/path is more stable.

---

# 12. Acceptance Test Catalog

## 19A unit
- factory maps valid inputs;
- negative counts rejected;
- source IDs preserved;
- Tempest rule follows Verdict evaluator.

## 19A integration
- different campaign states → different contexts;
- game-over route == player-route context;
- deaths match memorial;
- treaty/debt/cohort/VEL facts read truthfully;
- all branches reachable;
- save/load context stable;
- replay deterministic.

## 19B
- child ration arithmetic;
- schooling eligibility;
- apprenticeship capacity;
- maturity/work gate;
- live cohort selection replaces demo literal;
- memorial variance;
- cohort ending fact;
- persistence;
- 3-year balance.

## 19C
- UID gate;
- ignore rule;
- data-audit reconciliation;
- silence-audit reconciliation;
- canon-registry reconciliation;
- current-authority metrics;
- session-continuity journey;
- hardcoded-ending CI regression;
- content-utilization evidence.

---

# 13. Final Acceptance Checklist

## Release blocker — 19A
- [ ] failing proof written first
- [ ] `EpilogueContextInputs` exists in Core
- [ ] `EpilogueContextFactory` exists in Core
- [ ] one host projector gathers all facts
- [ ] deaths derive from death/memorial authority
- [ ] Tempest rule reuses Verdict evaluator
- [ ] treaty derives from treaty authority/validated flag
- [ ] debt derives from debt authority
- [ ] children derive from cohort
- [ ] VEL secret derives from evidence/reckoning
- [ ] days/living roster included in same projection
- [ ] both production call sites use same projector
- [ ] panel binds context object
- [ ] ending-fact transitions are visible during play
- [ ] every matrix branch reachable through campaign state
- [ ] demo boolean API retired from production
- [ ] save/load projection identical
- [ ] deterministic replay passes
- [ ] three 200-day policy endings differ

## Legacy — 19B
- [ ] real Cohort API inventory recorded
- [ ] exactly three cohort links implemented
- [ ] child ration fraction data-driven
- [ ] existing ration pipeline consumes child demand
- [ ] schooling/apprenticeship uses cohort eligibility
- [ ] duty roster gates maturity
- [ ] `sv_cohort_demo` absent from production
- [ ] deaths create memorial records with existing circumstance semantics
- [ ] passage-of-time marker surfaced in existing UI
- [ ] child/generation events feed briefing
- [ ] cohort persistence verified
- [ ] `childrenSurvived` derives correctly
- [ ] 3-year balance simulation recorded
- [ ] atlas classification refreshed from evidence

## Repository truth — 19C
- [ ] true dangling sidecars removed
- [ ] `EpilogueContextFactory.cs.uid` now has real sibling
- [ ] UID sidecar CI gate added
- [ ] `.claude/worktrees/` added to `.gitignore`
- [ ] duplicate-tree grep pollution resolved
- [ ] data gap audit reconciled
- [ ] silence audit reconciled
- [ ] canon registry reconciled
- [ ] current authority doc refreshed
- [ ] dirty work grouped into reviewable commits
- [ ] session-continuity journey test passes
- [ ] CI catches old all-true epilogue regression
- [ ] content-utilization evidence updated
- [ ] full wave verification passes

---

# 14. Ship / No-Ship Gate

**SHIP** only if:

```text
hardcoded_epilogue_inputs == 0
AND production_epilogue_projectors == 1
AND unreachable_matrix_branches_due_to_literals == 0
AND memorial_death_count_matches_epilogue == true
AND save_load_context_stable == true
AND three_policy_endings_distinct == true
AND dangling_cs_uid == 0
AND session_continuity_journey == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 15. Implementer Handoff

1. Do 19A first.
2. Do not write more ending content.
3. Build one projection path and delete duplicate derivation.
4. Reuse existing rule owners.
5. Treat every missing persisted fact as a persistence defect, not an excuse for a default.
6. Test branch reachability through real authorities.
7. Clean sidecars/duplicate worktree before deep repo-wide searches.
8. Keep cohort scope to the three explicit links.
9. Reuse existing ration, education, duty, memorial, briefing, and UI systems.
10. Close the wave with a save/load/game-over journey test and fresh audit metrics.

---

# 16. Final Outcome

When this plan is complete, the campaign computes its own ending from the state the player actually produced. Deaths matter because they are recorded, treaties and debt outcomes matter because their authorities are read, Verdict state matters through its canonical evaluator, children matter both during the years and at the end, and the repository's own audits stop teaching future agents false facts.

The 32 authored epilogue permutations remain the content authority. Plan 19 simply makes them reachable and truthful.
