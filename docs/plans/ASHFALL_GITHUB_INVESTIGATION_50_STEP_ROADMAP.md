# ASHFALL — Comprehensive GitHub Codebase Investigation & 50-Step High-Value Roadmap

**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`  
**Target branch reviewed:** `main`  
**Primary objective:** Identify unresolved issues, unimplemented/unwired systems, persistence risks, CI gaps, runtime placeholders, architecture debt, migration debt, and produce an execution-oriented roadmap of **20 high-value tasks / exactly 50 next steps**.

---

# 1. Executive Summary

ASHFALL does **not** currently need a full architectural reset. The codebase already contains several strong engineering foundations:

- engine-agnostic Core direction;
- explicit simulation ports;
- deterministic clock/RNG concepts;
- broad save/load infrastructure;
- extensive headless/self-test coverage;
- data-driven catalog architecture;
- Godot host migration work;
- fail-loud compatibility concepts;
- large regression-test surface;
- substantial content/system breadth.

The dominant remaining problem is now **integration correctness**.

The highest-value debt is concentrated in:

1. **GitHub/local persistence divergence**
2. **remaining mutable save-snapshot aliasing**
3. **clock boundary correctness**
4. **CI that does not enforce the full available verification surface**
5. **systems that are implemented/constructed but not actually reachable during gameplay**
6. **runtime UI panels backed by fixture/placeholder data**
7. **oversized `Main` orchestration**
8. **oversized concrete `SaveSystem` coordination**
9. **Unity legacy authority still compiled into the Godot application**
10. **generated-state tooling with stale/historical assumptions**
11. **warning/nullability suppression masking signal**
12. **lack of packaged-release proof**
13. **missing long-campaign persistence/balance instrumentation**
14. **near-ready gameplay systems that should only be implemented after integration debt is reduced**

The recommended strategy is:

> **Correctness first → reachability second → orchestration/persistence decomposition third → migration reduction fourth → new gameplay afterward.**

---

# 2. Current Repository Assessment

## 2.1 Current GitHub baseline

The reviewed GitHub `main` includes the merge that restored Core/build authority and clarified the Godot-vs-Unity compatibility policy.

The repository appears to be moving toward:

- `Assets/Ashfall.Core` as the engine-agnostic gameplay authority;
- `src/` as Godot host/presentation/composition;
- `Assets/_Game` as a shrinking compatibility/legacy surface.

This direction should be preserved.

---

# 3. Highest-Risk Findings

## 3.1 GitHub `main` is behind locally verified persistence hardening

A locally verified persistence-hardening pass previously introduced snapshot-isolation fixes and tests, but the corresponding commit is not present on the reviewed GitHub `main`.

This is important because the GitHub branch still contains direct mutable snapshot returns.

Examples include patterns such as:

```csharp
public SomeState CaptureState() => _state;
```

and:

```csharp
public void RestoreState(SomeState saved)
{
    _state = saved;
}
```

When `SomeState` contains mutable lists, dictionaries, nested reference objects, or collections, this creates aliasing risk.

### Impact

A captured "save snapshot" may continue sharing references with live runtime state.

That can cause:

- save corruption;
- non-deterministic tests;
- live mutations changing previously captured snapshots;
- snapshot mutations changing live state;
- restore state retaining ownership of external mutable objects;
- difficult-to-reproduce mid-game save bugs.

### Priority

**P0**

---

## 3.2 `TimeSystem` can violate its own hourly event contract

The current time model allows `MaxGameHoursPerStep` to exceed one hour.

However, the advancing logic compares only the previous integer hour with the final integer hour of a substep.

Therefore, if a substep crosses multiple hour boundaries, only one hourly event may be emitted.

A separate restore issue allows an `hourAccumulator` value of exactly `24`, despite `CurrentHour` being documented as `0..23`.

### Impact

Systems driven by hourly callbacks can silently under-run.

This can affect:

- hunger;
- thirst;
- fatigue;
- radiation;
- diseases;
- jobs;
- shelters;
- hazards;
- economy;
- scheduled events;
- AI;
- long-duration fast-forward.

### Priority

**P0**

---

## 3.3 Canonical CI tolerates Godot import failure

The canonical Godot gate attempts:

```bash
godot --headless --path . --import
```

but does not fail the pipeline when this command fails.

Instead, the script continues to later tests.

### Impact

A supposedly green clean-checkout CI run may still have failed to import assets correctly.

Later tests may detect some failures, but not necessarily every editor/importer failure.

### Priority

**P0**

---

## 3.4 The available self-test surface is far larger than the merge gate

The Godot host exposes dozens of headless verification actions covering areas including:

- assets;
- bridge compatibility;
- data integrity;
- disease;
- expansions;
- radio;
- survivors;
- world;
- economy;
- utility AI;
- RNG wiring;
- expeditions;
- medical;
- narrative;
- save paths;
- day transitions;
- playable shell;
- shelter hazards;
- shelter operations;
- audio;
- UI;
- warlords;
- maritime systems;
- combat;
- settings.

Yet the canonical merge gate runs only a small subset.

### Impact

The codebase contains regression protection that is not regularly used.

That creates false confidence: tests exist, but changes can still merge without exercising them.

### Priority

**P1**

---

## 3.5 `HostCli` registration is manually duplicated

The current CLI model combines:

- a large enum;
- a long ordered parser;
- manually maintained aliases;
- manually maintained help output;
- separate dispatch behavior.

### Risks

- duplicate aliases;
- unreachable commands;
- stale help text;
- parser ordering problems;
- test commands silently falling through;
- interactive mode triggered accidentally;
- difficult maintenance.

### Priority

**P1**

---

## 3.6 `Main` remains a Godot orchestration god-object

`Main` is still responsible for too many host concerns:

- session ownership;
- panel ownership;
- navigation;
- command/self-test lifecycle;
- diagnostics;
- save dirty-state;
- save flushing;
- setup sequencing;
- teardown;
- UI composition;
- lifecycle transitions.

The problem is not that `Main` is a composition root.

The problem is the number of policies implemented inside the composition root.

### Priority

**P1**

---

## 3.7 `SaveSystem` still has extreme concrete dependency fan-out

Persistence currently knows about an enormous number of concrete systems.

While an `ISaveable`/registry direction exists, the central coordinator still owns a very broad set of fields and setters.

### Risks

- new persistent systems amplify changes;
- restore order becomes difficult to prove;
- partial restore failures can leave a mixed live session;
- migrations become increasingly difficult;
- persistent-system omission risk grows;
- testing becomes expensive.

### Priority

**P1**

---

# 4. Implemented But Unwired / Unreachable Systems

One of the most important investigation results is that ASHFALL has relatively few obvious TODO/FIXME stubs, but a meaningful amount of:

> **"implemented, instantiated, tested — but not actually reachable in production gameplay."**

This is more dangerous than a TODO because the code appears finished.

---

## 4.1 Siege systems

`GameBootstrap.BatchSystems.cs` constructs multiple siege systems.

Examples include:

- artillery;
- biowarfare;
- blockade;
- hostage shield;
- night raid;
- sappers;
- smoke out;
- vehicle ram.

Some are ticked.

However, at least some activation methods appear only in their implementation/test surfaces rather than a production gameplay trigger.

Examples include:

```text
StartBlockade(...)
CutPower(...)
```

### Why this matters

A constructed/ticked/saveable system is not necessarily implemented gameplay.

A system should only be considered integrated when there is a complete path:

```text
Player/world condition
→ activation authority
→ active state
→ tick/update
→ player feedback/UI
→ save/load
→ terminal outcome
```

### Priority

**P1**

---

# 5. Runtime UI Placeholder Debt

## 5.1 Save/Load panel

The current save/load runtime panel still behaves like a mock.

It contains hard-coded example slots and buttons that only log messages.

This is a very high-value fix because save/load is core player-facing infrastructure.

The real panel needs:

- actual slot enumeration;
- save metadata;
- current day;
- survivor count;
- save version;
- corruption state;
- modification time;
- save size;
- Save action;
- Load action;
- Delete action;
- confirmation;
- error handling;
- unsupported-version messaging.

### Priority

**P1**

---

## 5.2 Economy overlay

The economy panel currently renders placeholder stock/trade/economic data.

Its `Bind(...)` path is not connected to a real typed host/session model.

### Priority

**P1**

---

## 5.3 Journal panel

The journal panel contains hard-coded example:

- logs;
- survivor notes;
- story chapters.

A runtime journal should instead consume:

- actual journal entries;
- narrative progression;
- known NPC information;
- discovered lore;
- quest-state changes;
- radio discoveries;
- shelter events.

Snapshot harnesses may still use fixtures, but production should not.

### Priority

**P1**

---

# 6. Legacy / Migration Authority Debt

The Godot aggregate still compiles:

```text
Assets/_Game/**/*.cs
```

in addition to:

```text
src/**/*.cs
Assets/Ashfall.Core/**/*.cs
```

This means migrated and legacy authorities remain compiled together.

### Problems

- duplicate ownership;
- dead Unity-era code can break Godot;
- compatibility shims keep expanding;
- migration completion does not reduce compile surface;
- contributors cannot easily determine the true authority.

### Desired model

Every feature/domain should be classified as exactly one of:

```text
CORE_AUTHORITY
GODOT_HOST_PRESENTATION
LEGACY_COMPATIBILITY_REQUIRED
RETIRED_QUARANTINED
```

### Priority

**P1**

---

# 7. Warning / Nullability Policy

The Godot project broadly suppresses many nullability warnings.

Examples include categories such as:

```text
CS8600
CS8601
CS8602
CS8603
CS8604
CS8618
CS8625
```

This is understandable during migration, but a broad suppression policy can hide newly introduced faults.

### Recommended model

Use a **ratchet**:

1. record baseline warnings;
2. prevent warning count from increasing;
3. require touched/new Core code to be warning-clean;
4. enable nullable incrementally;
5. isolate serializer-specific warnings locally;
6. eventually use warnings-as-errors for clean areas.

### Priority

**P2**

---

# 8. Generated Visual / Data State

The generated-art pipeline still contains historical assumptions.

The production manifest test documents fixed historical counts and asserts that certain categories must be non-zero.

Generated state should be validated relationally instead.

For example:

```text
PENDING + SKIP_REFERENCE_ONLY == TOTAL
```

without requiring both categories to always contain rows.

The current human-readable `WIRING_MATRIX.md` is also empty while the machine-readable JSON exists.

### Desired architecture

One command:

```bash
python3 tools/regenerate_visual_state.py --write
```

and:

```bash
python3 tools/regenerate_visual_state.py --check
```

should reproduce every committed derived artifact deterministically.

### Priority

**P1/P2**

---

# 9. 20 High-Value Tasks / Exactly 50 Next Steps

---

## TASK 1 — P0
# Reconcile GitHub `main` with locally verified hardening

### Step 1
Diff the local snapshot-hardening work against the current GitHub `main`.

Classify every hunk as:

```text
ALREADY_SUPERSEDED
STILL_REQUIRED
CONFLICTING
UNRELATED
```

Do not blindly cherry-pick.

### Step 2
Create a fresh branch from current `main` and port only applicable persistence fixes and associated tests.

Do not import unrelated dirty-tree content.

### Step 3
Run:

```bash
dotnet build Ashfall.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
./scripts/ci/godot-asset-gate.sh
```

Establish this reconciled state as the new baseline.

### Exit criteria

- GitHub contains all still-valid persistence fixes.
- No unrelated local modifications enter the branch.
- Core and Godot verification is green.

---

## TASK 2 — P0
# Eliminate all remaining mutable save-snapshot aliases

### Step 4
Inventory every instance of:

```text
CaptureState() => _state
Capture() => _state
return _state
_state = saved
```

where mutable state is involved.

Scan:

```text
Assets/Ashfall.Core
Assets/_Game
src
```

### Step 5
Fix active persistence participants first.

Start with systems that:

- are registered with `SaveSystem`;
- participate in the Godot host;
- contain lists/dictionaries/reference DTOs;
- are used in mid/late-game systems.

### Step 6
Create generic mutation-isolation tests:

```text
capture
→ mutate live
→ captured snapshot unchanged

capture
→ mutate captured snapshot
→ live state unchanged

restore
→ mutate input object
→ restored live state unchanged
```

### Exit criteria

No persistent participant returns a live mutable object as its save snapshot.

---

## TASK 3 — P0
# Repair TimeSystem boundary correctness

### Step 7
Replace substep-dependent hourly dispatch with explicit boundary traversal.

A 5.5-hour jump must emit all crossed hour boundaries even if:

```text
MaxGameHoursPerStep = 8
```

### Step 8
Canonicalize restored clock state.

Normalize:

```text
hour == 24
hour > 24
hour < 0
NaN
Infinity
invalid day
```

into deterministic valid state.

### Step 9
Add tests for:

- 5.5-hour advance;
- 24-hour exact transition;
- multiple days;
- high step budget;
- fast-forward;
- malformed save values;
- restore followed by deterministic advancement.

### Exit criteria

Clock semantics are independent of tuning step size.

---

## TASK 4 — P0
# Make canonical CI a genuine release-quality gate

### Step 10
Make Godot import failure fatal.

Replace:

```text
warning → continue
```

with:

```text
failure → CI failure
```

unless a narrowly defined benign Godot case is explicitly detected.

### Step 11
Add a clean-checkout/fresh-cache CI lane.

It should verify without a developer `.godot` cache:

```text
restore
build
import
asset validation
headless gameplay gates
```

### Step 12
Verify GitHub branch/ruleset protection manually as repository administrator.

Require canonical ASHFALL checks before merge.

If admin bypass is retained, document an incident/follow-up policy.

### Exit criteria

A deliberately broken Core/Godot/asset PR cannot merge through the normal workflow.

---

## TASK 5 — P1
# Convert HostCli into a declarative command registry

### Step 13
Create one command descriptor model containing:

```text
Action
CanonicalFlag
Aliases
Description
Category
Handler
CI Tier
InteractiveAllowed
```

### Step 14
Generate parsing and help text from the same registry.

Add tests proving:

- alias uniqueness;
- handler existence;
- command reachability;
- help completeness;
- unknown test flags fail loudly.

### Step 15
Define two automated test tiers.

#### Required PR tier

Include at minimum:

```text
data integrity
bridge
asset registry
RNG wiring
save/tamper
Day 1 → Day 2
playable shell
survivors
world
economy
expansion aggregate
```

#### Nightly tier

Run the complete stable non-interactive HostCli matrix.

### Exit criteria

There is one source of truth for CLI verification.

---

## TASK 6 — P1
# Decompose `Main` safely

### Step 16
Extract CLI/self-test lifecycle into:

```text
HostCliRunner
```

Preserve behavior exactly.

### Step 17
Extract:

```text
SaveFlushCoordinator
DiagnosticsCoordinator
```

Centralize:

- dirty markers;
- coalescing;
- explicit flush;
- quit flush;
- diagnostic refresh timing.

### Step 18
Extract:

```text
UiCompositionRouter
GameSessionCoordinator
```

Then create an ownership matrix:

| Host Session | Setup Owner | Tick Owner | Save Owner | UI Owner | Dispose Owner |
|---|---|---|---|---|---|

### Exit criteria

`Main` remains a composition root but no longer contains most orchestration policy.

---

## TASK 7 — P1
# Complete the versioned transactional persistence registry

### Step 19
Create a participant descriptor such as:

```text
StableId
SchemaVersion
Capture()
Decode()
Validate()
Migrate()
Apply()
Required
```

Reject duplicate IDs immediately.

### Step 20
Implement true two-phase restore.

#### Phase A

```text
read
checksum
decode
migrate
validate
detached snapshot
```

for every participant.

#### Phase B

Only if **all required participants pass**:

```text
apply to live systems
```

### Step 21
Migrate one low-risk system first.

Add tests for:

- old save version;
- current save;
- corrupt participant;
- invalid schema;
- duplicate ID;
- failed migration;
- all-or-nothing restore.

Then migrate the rest incrementally.

### Exit criteria

Adding a participant no longer requires expanding central concrete field ownership indefinitely.

---

## TASK 8 — P1
# Perform full production reachability audit

### Step 22
Generate a machine-readable integration matrix for all constructed systems:

| System | Constructed | Activated | Production Caller | Tick Path | Save | UI | Terminal State | E2E Test |
|---|---:|---:|---|---:|---:|---:|---:|---:|

### Step 23
Resolve all siege-family systems first.

For each system:

```text
game condition
→ encounter/event authority
→ activation
→ live effects
→ UI/player feedback
→ resolution
```

Do not keep test-only activation as "implemented."

### Step 24
Add end-to-end production-route tests.

A system is integrated only if a test can:

```text
trigger through production authority
→ advance
→ observe effect
→ save
→ reload
→ continue
→ resolve
```

### Exit criteria

"Constructed" and "Implemented" are no longer treated as equivalent.

---

## TASK 9 — P1
# Replace production UI fixture state with live typed data

### Step 25
Implement real Save/Load UI first.

Required operations:

```text
EnumerateSlots
GetMetadata
Save
Load
Delete
ConfirmDelete
DetectCorruption
ShowVersion
ShowTimestamp
ShowSaveSize
```

### Step 26
Replace no-op `Bind(object)` patterns with typed contracts.

Examples:

```text
IEconomyViewModel
IJournalViewModel
ISaveLoadViewModel
```

Use explicit event subscription and disposal.

### Step 27
Separate fixture mode from runtime mode.

Snapshot/UI tests may inject fixture view models.

Production runtime should never silently show fake economy, journal, or save data.

### Exit criteria

Every player-facing panel labeled as functional displays actual runtime state.

---

## TASK 10 — P1
# Shrink the Unity compatibility compile surface

### Step 28
Create:

```text
migration_ownership.json
```

Classify every relevant file/domain:

```text
CORE_AUTHORITY
GODOT_HOST
UNITY_COMPAT_REQUIRED
RETIRED
```

### Step 29
Replace blanket:

```xml
<Compile Include="Assets/_Game/**/*.cs" />
```

with explicit compatibility inclusion/exclusion.

Start with fully migrated domains.

### Step 30
Add CI checks for duplicate gameplay authority.

A migrated Core system should not simultaneously regain an active legacy implementation.

### Exit criteria

The Godot executable compiles only intentional compatibility code.

---

## TASK 11 — P1
# Create one atomic generated-state pipeline

### Step 31
Create:

```bash
python3 tools/regenerate_visual_state.py --write
```

that runs all relevant generators in dependency order.

### Step 32
Create:

```bash
python3 tools/regenerate_visual_state.py --check
```

which generates into temporary output and compares against committed state.

Add it to CI.

### Exit criteria

Partial generation cannot silently leave stale dependent artifacts.

---

## TASK 12 — P1
# Remove historical assumptions from art-pipeline tests

### Step 33
Replace historical fixed-count logic with relational invariants.

Allow legitimate states such as:

```text
SKIP_REFERENCE_ONLY == 0
```

when current source data produces that result.

### Step 34
Treat:

```text
docs/visual/WIRING_MATRIX.json
```

as machine authority.

Regenerate or remove empty/outdated human reports instead of maintaining contradictory copies.

### Exit criteria

Generator output can change legitimately without forcing manual count edits.

---

## TASK 13 — P2
# Harden SaveChecksum schema coverage

### Step 35
Reflect serialized save DTO members in tests.

Fail if a serializable member is not covered by checksum canonicalization.

### Step 36
Add golden checksum vectors for:

```text
small campaign save
mid-game save
late-game save
large collection save
legacy save
```

Explicitly canonicalize or forbid unordered save collections.

### Exit criteria

Save-integrity coverage evolves safely with schema changes.

---

## TASK 14 — P2
# Introduce warning/nullability ratchet

### Step 37
Create a warning baseline by:

```text
project
domain
warning ID
file
```

Fail CI on net-new warnings in changed production code unless explicitly justified.

### Step 38
Enable nullable incrementally in Core and modern Godot code.

Move unavoidable compatibility suppressions to the smallest practical scope.

### Exit criteria

Warning signal improves over time without requiring an unrealistic flag-day cleanup.

---

## TASK 15 — P2
# Measure long-campaign persistence scalability

### Step 39
Instrument:

```text
capture duration
capture duration per participant
checksum duration
save byte size
restore decode duration
migration duration
validation duration
apply duration
largest collections
allocations
```

### Step 40
Run save snapshots around:

```text
Day 1
Day 30
Day 100
Day 180
```

Detect unbounded histories, logs, completed queues, journal structures, and similar persistent growth.

### Exit criteria

Persistence optimization decisions become evidence-based.

---

## TASK 16 — P2
# Expand balance telemetry into realistic survival scenarios

### Step 41
Add deterministic scenarios:

#### A — Sustainable

```text
food every 24h
water every 24h
```

#### B — Scarce

```text
food every 48h
water every 24h
```

#### C — Severe

```text
food every 48h
water every 36–48h
```

Use real mechanics.

### Step 42
Run:

```text
7 days
14 days
30 days
```

across multiple seeds.

Record:

```text
hunger
thirst
fatigue
warmth
morale
health
radiation
food
water
power
market cost
resource shortfall
```

Tune only if evidence demonstrates a systemic problem.

### Exit criteria

Balance changes are driven by reproducible simulation evidence.

---

## TASK 17 — P2
# Close real runtime asset wiring gaps

### Step 43
Regenerate the wiring matrix from current catalogs/runtime resolution.

Classify unresolved entries by:

```text
CRITICAL_RUNTIME
NORMAL_RUNTIME
OPTIONAL
REFERENCE_ONLY
DEBUG
DEPRECATED
```

### Step 44
Extend asset verification beyond:

```text
file exists
.import exists
```

to:

```text
production path actually resolves asset
```

### Exit criteria

Critical player-visible paths cannot resolve to missing art while CI remains green.

---

## TASK 18 — P2
# Reduce dependency and repository-policy ambiguity

### Step 45
Inventory every network/service dependency.

For each record:

```text
Package
Host
Runtime Active?
Purpose
Network Behavior
Telemetry?
Data Sent
Privacy Impact
Removal Condition
```

Include Unity services and Sentry.

### Step 46
Move historical audits/generated process material away from the active repository root where practical.

Every quarantine or deprecated tree should have:

```text
owner
reason
removal condition
retirement issue/date
```

### Exit criteria

Dependency and repository policy become explicit rather than archaeological.

---

## TASK 19 — P2
# Prove the real Linux shipping artifact

### Step 47
Create a release job that:

```text
installs exact Godot export templates
builds Linux/X11 release
records executable hash
records PCK hash
```

### Step 48
Inspect the resulting PCK.

Prove that required JSON/data resources are actually packaged.

Then launch the exported binary using a safe smoke mode.

### Exit criteria

Release confidence is based on the packaged game—not only the editor/project build.

---

## TASK 20 — P3
# Implement the highest-value near-ready gameplay slices

Only begin this task after the major correctness/reachability work above.

### Step 49
Implement **Caravan Route Dispatch**.

Requirements:

```text
route model
crew assignment
dispatch
travel state
cancel/failure rules
arrival
resource result
save/load
host integration
UI
headless lifecycle test
```

### Step 50
Implement one focused medical/trade vertical slice.

Recommended first option:

```text
Quarantine Bio-Ward
```

because existing medical/respiratory infrastructure should allow higher reuse.

A feature is not considered finished until it has:

```text
Core authority
runtime activation
persistence
Godot host integration
UI
deterministic test
```

### Exit criteria

New features follow the improved integration standard rather than adding more partial systems.

---

# 10. Recommended Execution Order

The highest-value implementation order is:

```text
Task 1  — GitHub/local reconciliation
Task 2  — snapshot isolation
Task 3  — clock correctness
Task 4  — canonical CI hardening
Task 5  — declarative CLI verification
Task 8  — implemented-but-unwired audit
Task 9  — live runtime UI
Task 6  — Main decomposition
Task 7  — transactional persistence registry
Task 10 — shrink legacy compile surface
Task 11 — atomic generator pipeline
Task 12 — generated-state invariants
Task 13 — checksum schema guards
Task 14 — warning ratchet
Task 15 — save scalability
Task 16 — sustainable balance telemetry
Task 17 — asset runtime proof
Task 18 — dependency/repository policy
Task 19 — packaged Linux release proof
Task 20 — new gameplay vertical slices
```

---

# 11. Recommended First Implementation Cycle

For the next serious engineering cycle, pause broad new content expansion and focus on:

## Phase A — Correctness

1. Reconcile GitHub/local hardening.
2. Eliminate mutable snapshots.
3. Repair `TimeSystem`.
4. Make Godot import fatal.
5. Expand required regression gates.

## Phase B — Reachability

6. Build full system reachability matrix.
7. Repair siege activation.
8. Identify other constructed-but-dead systems.
9. Require production-route E2E tests.

## Phase C — Player-facing infrastructure

10. Implement real Save/Load panel.
11. Wire Economy panel.
12. Wire Journal panel.
13. Remove runtime fixture fallback.

## Phase D — Architecture

14. Decompose `Main`.
15. Complete transactional participant registry.
16. Reduce `_Game` compile ownership.

---

# 12. Definition of "Implemented"

From this point forward, a system should **not** be considered implemented merely because a class exists.

A feature should satisfy all applicable layers:

```text
[ ] Core/domain authority exists
[ ] Data/catalog definition exists
[ ] Construction exists
[ ] Production activation path exists
[ ] Runtime tick/update exists
[ ] Player-visible feedback exists
[ ] Save/load exists
[ ] Snapshot is detached
[ ] Restore is validated
[ ] Host integration exists
[ ] UI integration exists where applicable
[ ] Headless deterministic test exists
[ ] Production-route E2E test exists
[ ] CI exercises critical behavior
[ ] Terminal/resolution state exists
```

This checklist should become a feature-merge standard.

---

# 13. Suggested Severity Policy

## P0 — Stop new feature expansion

Use for:

- corrupted saves;
- non-buildable canonical branch;
- global clock errors;
- partial restore;
- CI falsely green;
- packaged build impossible.

## P1 — Schedule immediately

Use for:

- implemented-but-unreachable systems;
- `Main` orchestration debt;
- persistence fan-out;
- production fixture UI;
- duplicate authority;
- test coverage not executed.

## P2 — Hardening

Use for:

- warnings;
- generated-state reproducibility;
- performance measurement;
- dependency reduction;
- long-campaign soak testing;
- asset completeness.

## P3 — Expansion

Use for:

- new gameplay vertical slices;
- optional visual enhancements;
- additional content;
- feature breadth.

---

# 14. Repository Architecture Target

A healthy target architecture should resemble:

```text
Assets/Ashfall.Core/
    Engine-independent gameplay authority
    Deterministic simulation
    Save DTOs
    Domain logic
    Ports

src/
    Godot composition
    UI
    navigation
    host sessions
    persistence adapters
    platform integration

Assets/_Game/
    Explicitly bounded compatibility only
    Shrinking over time

Assets/StreamingAssets/Data/
    Authored machine-readable gameplay content

tools/
    Deterministic generators
    validation
    auditing
    migration tooling

Ashfall.Core.Tests/
    Core correctness
    persistence
    determinism
    schema
    regression

scripts/ci/
    Canonical build/import/runtime gates
```

---

# 15. Long-Term End State

ASHFALL should reach the following state:

- `main` is always buildable;
- canonical checks are mandatory;
- Core is the single gameplay authority for migrated domains;
- Godot owns presentation/composition rather than gameplay rules;
- Unity code is explicit compatibility, not implicit authority;
- save participants are versioned;
- save restore is transactional;
- snapshots are detached;
- clock events are boundary-correct;
- all critical systems have a production activation path;
- runtime UI displays live game state;
- generated data/art state is reproducible;
- warnings decrease rather than accumulate;
- every headless command is registered declaratively;
- merge and nightly CI together cover the verification matrix;
- long campaigns are profiled and soak-tested;
- Linux packaged builds are actually inspected/launched;
- new features are accepted only after full vertical integration.

---

# 16. Highest-Leverage Principle

The most valuable work now is not writing another 100 systems.

It is ensuring that the systems already present are:

```text
reachable
correct
persistent
deterministic
visible
testable
packaged
```

The largest current risk class is:

> **implemented-looking code that is not completely integrated.**

The corresponding engineering principle should be:

> **No system is "done" until its production path, persistence path, host path, player-facing path, and regression path are all proven.**

---

# 17. Final Recommended Priority Stack

## Immediate

1. GitHub/local persistence reconciliation
2. save snapshot isolation
3. `TimeSystem`
4. Godot import CI
5. required regression gates
6. production reachability audit
7. Save/Load runtime UI

## Next

8. Economy/Journal live bindings
9. `Main` decomposition
10. transactional save registry
11. legacy compile reduction
12. generated-state pipeline
13. checksum guards

## Hardening

14. warning ratchet
15. long-campaign save soak
16. fed/hydrated balance telemetry
17. runtime asset proof
18. dependency cleanup
19. Linux packaged release proof

## Expansion

20. Caravan Route Dispatch
21. Quarantine Bio-Ward or one comparable focused vertical slice

---

# 18. Conclusion

ASHFALL already has substantial depth and an unusually broad systems/test surface.

The next quality jump will not come primarily from adding more code.

It will come from reducing the gap between:

```text
code exists
```

and:

```text
the player can reach it,
it behaves correctly,
it survives save/load,
it is deterministic,
its UI reflects real state,
CI proves it,
and the exported game contains it.
```

That is the highest-value direction for the next development phase.
