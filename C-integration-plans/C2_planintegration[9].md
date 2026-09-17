# C2 — Flagship Integration Plan [9]: Orchestration Spine, Declarative Subsystem Manifest, Behaviour-Preserving Decomposition, and Lifecycle Contracts

> **Deliverable:** `C2_planintegration[9].md`
> **Source scope:** Plan 28 — *The Orchestration Spine: Registration You Cannot Forget*
> **Wave:** Continuity Wave 3 — *Ship It Intact*
> **Primary objective:** replace human-memory orchestration with one declarative subsystem manifest that drives setup, save, flush, day-owner, panel, event, and authority registration; then split large host files only along those declared ownership seams; finally make subsystem lifecycle stages explicit, deterministic, teardown-safe, and session-rebind-safe.
> **Execution order:** **28A → 28B → 28C**
> **Hard sequencing rule:** complete 28A before any large file split. 28B without the manifest only redistributes implicit knowledge.
> **Tree-stability rule:** do not begin 28B while overlapping Wave-1/Wave-2 work is editing the same files; mechanical moves require a quiet tree.
> **Dependencies:** Wave 1 Plan 15C liveness metadata and Wave 2 semantic day-event vocabulary.
> **Downstream integration:** Plan 27 runtime identity checks, Plan 26 diagnostics, Plan 17/24 event routing, Plan 16 authority/subscription correctness.
> **Scope discipline:** no DI-container rewrite, no new orchestration framework, no behaviour change in 28B commits, no duplicate save-section authority, no subsystem split that leaves one ownership unit scattered across unrelated files.

---

# 0. Executive Intent

ASHFALL already has a sophisticated composition root, but its orchestration contract is distributed across many mechanisms:

- dozens of `Setup*` methods,
- dozens of `Save*` methods,
- a smaller set of `Flush*` methods,
- `SaveSectionRegistry`,
- day-owner registration,
- panel route registration,
- panel authority bindings,
- event subscriptions,
- save-store façades,
- lifecycle rebuilds on New Game/Load.

The source plan identifies the structural defect:

```text
a subsystem is "alive" only if a contributor remembers every registration site
```

That failure class already explains multiple continuity defects from earlier waves:

- system constructed but never saved,
- save section exists but runtime never ticks,
- event exists but no bridge subscribes,
- panel exists but binds a fresh authority,
- action exists but no caller reaches it,
- host session survives old campaign lifecycle,
- save store persists nothing useful,
- a subsystem has setup but no teardown.

The correct repair is not more grep-based gates.

It is one declaration.

The final composition model should be:

```text
SubsystemDescriptor
    │
    ├─ lifecycle group
    ├─ setup
    ├─ save section
    ├─ save method
    ├─ optional flush
    ├─ day owner + phase
    ├─ panel routes
    ├─ required authorities
    ├─ event sources
    ├─ persistence expectations
    └─ teardown requirements
    │
    ▼
SubsystemRegistration
    ├─ deterministic construction/setup order
    ├─ save capture orchestration
    ├─ flush orchestration
    ├─ day-owner registration
    ├─ panel binding validation
    ├─ event subscription derivation
    └─ lifecycle teardown/rebind
    │
    ▼
Generated manifest docs + CI completeness gates
```

The flagship outcome is:

> **A subsystem cannot be partially registered. Adding a new system without declaring its setup, save, tick, surface, event, authority, and lifecycle obligations becomes a compile/test/CI failure instead of a future continuity bug.**

---

# 1. Source Evidence and Architectural Reading

The source plan establishes the following conditions:

- orchestration is already split across many `Main*.cs` partials,
- documentation still describes an outdated monolithic `Main.cs`,
- setup/save/flush counts disagree across docs and code,
- current triad gate covers only part of the real lifecycle,
- save sections, day owners, panels, and stores each have different registration mechanisms,
- `SaveSectionRegistry` proves declarative authority works,
- previous bugs arose from missing registration/wiring rather than missing subsystem logic,
- current composition ordering is implicit and partly lazy,
- large-file size is not itself the root issue.

The correct interpretation is:

```text
file size is a symptom;
undeclared ownership is the cause.
```

Therefore:

```text
28A defines ownership
28B moves code along ownership
28C formalizes lifecycle
```

---

# 2. Program-Level Success Criteria

C2[9] is complete only if all of the following are true.

## 2.1 One subsystem declaration exists

Every live subsystem has exactly one manifest descriptor.

## 2.2 Save authority is not duplicated

Manifest references `SaveSectionRegistry`.

It does not redefine section names, schema versions, aliases, or file names.

## 2.3 Day ownership is declarative

Owner ID and phase are explicit manifest data.

## 2.4 Player surfaces are linked to subsystem ownership

Panel routes, liveness, maturity, and required authorities are discoverable from the manifest.

## 2.5 Event sources are declared

Audio/briefing/journal consumers can derive subscriptions from subsystem declarations.

## 2.6 Setup/save/flush parity is enforceable

A subsystem declares whether each stage applies.

Optional stages are explicit rather than absent-by-accident.

## 2.7 Ordering is deterministic

Construction/setup/day/lifecycle order is stable across runs.

## 2.8 File decomposition is behavior-neutral

Large-file splits produce no state, save, snapshot, event, or performance change.

## 2.9 Lifecycle is explicit

Construct, wire, initialize, tick, persist, teardown semantics are declared and testable.

## 2.10 Session replacement cannot leave stale subsystem or panel references

New Game and Load perform ordered teardown/rebind over the manifest.

---

# 3. Architectural Invariants

## 3.1 One declaration per subsystem

No subsystem should require matching edits across unrelated manually maintained lists.

## 3.2 `SaveSectionRegistry` remains save metadata authority

Manifest references:

```text
section key
```

not:

```text
duplicate schema version/name/file mapping
```

## 3.3 Existing owner IDs are preserved

Do not mint replacement owner IDs for day events.

Promote current strings to constants/typed values.

## 3.4 Manifest is descriptive and executable

It must support both:

- generated documentation/gates,
- host orchestration.

A docs-only manifest does not solve the problem.

## 3.5 Public wrappers may survive migration

Keep `SetupXxx`, `SaveXxx`, `FlushXxx` wrappers temporarily if other code calls them.

But orchestration order should be driven centrally.

## 3.6 Optional flush is explicit

No forced one-size-fits-all parity.

Subsystems declare:

```text
flush required
or
flush not applicable + reason
```

## 3.7 Panel authority requirements are declarative

A route knows which campaign authority it must bind.

This converts fresh-instance binding from a grep smell into a manifest contract violation.

## 3.8 Event source ownership is declarative

Events are emitted by declared subsystem authorities.

Subscriptions are derived or validated from those declarations.

## 3.9 28B commits are mechanical only

No logic edits, no renames, no signature changes, no opportunistic cleanup.

## 3.10 Lifecycle teardown is symmetric

Any subsystem subscribing to external events must declare and execute teardown.

---

# 4. Dependency Graph

```text
15C — panel liveness metadata
 │
 └──────────────► 28A manifest panel fields

Wave-2 day-event vocabulary
 │
 └──────────────► 28A event-source / day-owner declarations

28A — subsystem manifest
 │
 ├──────────────► generated ownership docs
 ├──────────────► setup/save/flush orchestration
 ├──────────────► day-owner registration
 ├──────────────► panel authority validation
 ├──────────────► event subscription derivation
 │
 ▼
28B — split files along declared ownership
 │
 ▼
28C — lifecycle stages / teardown / rebind
 │
 ├──────────────► Plan 27 runtime identity checks
 ├──────────────► Plan 26 diagnostics
 ├──────────────► Plan 16 authority/subscription invariants
 └──────────────► Plan 17/24 event publication
```

Required execution:

```text
28A → 28B → 28C
```

---

# 5. Baseline Capture

Before modifying orchestration, capture:

- commit SHA,
- test count,
- golden save digests,
- snapshot hashes,
- runtime-scale metrics,
- architecture map,
- current setup/save/flush/day/panel/store counts.

## 5.1 Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --runtime-scale-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/generate-architecture-map.sh --check
bash scripts/ci/verify-fast.sh
```

Capture current exported-build smoke from Plan 26 if available.

## 5.2 Count inventory

Measure:

```text
Setup* methods
Save* methods
Flush* methods
SaveSectionRegistry entries
_campaignDay.Register calls
PanelRegistry routes
SaveStore facades
event emitters
AudioEventBridge subscriptions
briefing producers
journal/event consumers
```

Historical source counts are useful baselines but re-measure current branch.

---

# 6. Workstream 28A — One Subsystem Manifest

## 6.1 Objective

Declare each subsystem once and derive/validate the rest.

---

# 7. 28A Phase A — Design `SubsystemDescriptor`

Design on paper first.

Conceptual record:

```csharp
SubsystemDescriptor
{
    Id,
    LifecycleGroup,
    SetupMethod,
    SaveSection?,
    SaveMethod?,
    FlushMethod?,
    DayOwner?,
    DayPhase?,
    PanelRoutes[],
    EventSources[],
    RequiredAuthorities[],
    RequiresTeardown,
    Notes
}
```

Use typed fields rather than raw strings where practical.

---

# 8. 28A Phase B — Manifest Identity

Add:

```text
Assets/Ashfall.Core/Composition/SubsystemManifest.cs
```

or equivalent engine-free Core type.

Define stable subsystem IDs.

Avoid deriving IDs from class names automatically if those names may change.

---

# 9. 28A Phase C — Reuse SaveSectionRegistry

Manifest should reference canonical save-section key.

Validation:

```text
descriptor.SaveSection != null
→ key exists in SaveSectionRegistry
```

Do not repeat:

- version,
- file name,
- alias,
- lifecycle group if registry already owns it.

Where lifecycle group belongs to subsystem rather than section, define ownership clearly.

---

# 10. 28A Phase D — Day Owner Constants

Promote current owner IDs from literals into one stable vocabulary.

Examples conceptually:

```text
survivors_needs
power_grid
weather_world
```

Do not rename without migration because these IDs may appear in:

- `DayStateChangeEvent.SourceOwnerId`,
- briefing,
- diagnostics,
- saved/debug data.

---

# 11. 28A Phase E — Panel Route Declarations

Each subsystem descriptor lists:

- panel route IDs,
- maturity/liveness,
- required authorities.

Integrate Plan 16A/15C metadata.

A manifest query should answer:

```text
which live surfaces expose this subsystem?
```

and:

```text
what authority identity should each route bind?
```

---

# 12. 28A Phase F — Event Source Declarations

Declare events emitted by each subsystem.

Potential metadata:

- event type,
- semantic category,
- audio-interest,
- briefing-interest,
- journal-interest.

Avoid over-modeling subscriber details if event consumers can discover based on interfaces/tags.

The key requirement is to eliminate undeclared emitters.

---

# 13. 28A Phase G — Required Authorities

For each panel/host action, declare required campaign authority types/IDs.

Validation should make this impossible:

```text
panel route says it needs FactionStanceEngine
but bind lambda constructs a new one
```

Plan 16B becomes an invariant.

---

# 14. 28A Phase H — Generated Manifest Artifacts

Create:

```text
scripts/ci/generate-subsystem-manifest.py
```

Outputs:

```text
docs/architecture/SUBSYSTEM_MANIFEST.md
docs/architecture/subsystem-manifest.json
```

Generated fields should include:

- subsystem ID,
- owner,
- lifecycle group,
- setup,
- save section,
- save method,
- flush policy,
- day owner/phase,
- panel routes,
- required authorities,
- event sources,
- teardown requirement.

---

# 15. 28A Phase I — CI Manifest Check

Register:

```bash
python3 scripts/ci/generate-subsystem-manifest.py --check
```

in CI manifest.

Fail when:

- generated docs stale,
- subsystem missing,
- duplicate subsystem ID,
- duplicate owner registration,
- undeclared save section,
- undeclared route/owner/store mapping.

---

# 16. 28A Phase J — Host `SubsystemRegistration`

Create:

```text
src/Host/SubsystemRegistration.cs
```

Responsibilities:

- deterministic setup,
- save orchestration,
- flush orchestration,
- day-owner registration,
- authority mapping,
- event subscription setup,
- teardown order.

Do not turn this into a service locator/DI container.

---

# 17. 28A Phase K — Setup Migration

Move orchestration call-list authority into `SubsystemRegistration`.

Existing `SetupXxx` wrappers remain callable.

But application bootstrap should iterate manifest/registration rather than maintaining a separate manual sequence.

---

# 18. 28A Phase L — Save Capture Migration

For descriptors with save sections:

```text
manifest descriptor
→ canonical SaveXxx wrapper or serializer callback
```

Validate:

- exactly one capture path,
- section exists,
- store delegation satisfies SaveStoreHub contract.

---

# 19. 28A Phase M — Flush Policy

For every subsystem:

```text
FlushRequired = true/false
Reason
```

If required:

- method exists,
- registration invokes it.

If not required:

- explicit reason such as immediate persistence or statelessness.

This eliminates ambiguity from count mismatches.

---

# 20. 28A Phase N — Deterministic Ordering

Define:

```text
LifecycleGroup
→ DayPhase / SetupPhase
→ ordinal subsystem ID
```

or another explicit stable ordering.

Document it.

Add digest test for resulting ordered subsystem list.

---

# 21. 28A Phase O — Eliminate Lazy Setup in Bind Lambdas

Panel binding must not do:

```text
if null then SetupXxx()
```

Instead:

```text
RequireAuthority<T>()
```

or manifest registration asserts authority already constructed.

This converts half-initialized panel binding into a loud lifecycle error.

---

# 22. 28A Phase P — Three-Subsystem Pilot

Before mass migration, select:

- small,
- medium,
- messy.

Source examples:

- vinyl morale,
- power grid,
- phase0 psychology.

For each:

1. declare descriptor,
2. route setup,
3. route save,
4. route day-owner,
5. route panel requirements,
6. route event sources,
7. compare golden save,
8. compare events,
9. compare runtime behavior.

Do not batch until pilot proves ordering parity.

---

# 23. 28A Completeness Gates

Test that each current mechanism is represented exactly once.

Enumerate:

- `_campaignDay.Register`,
- `SaveStore`,
- `Setup*`,
- `Save*`,
- `Flush*`,
- player routes,
- event sources.

Fail on:

- orphan,
- duplicate,
- undeclared item.

---

# 24. 28A Intentional-Omission Test

Create a fixture subsystem deliberately missing one required declaration.

Assert CI/test fails.

Examples:

- save section missing manifest row,
- day owner absent,
- route authority undeclared,
- event source omitted.

This proves the gate catches the bug class.

---

# 25. 28A Definition of Done

- [ ] descriptor shape documented,
- [ ] engine-free manifest exists,
- [ ] save sections referenced, not duplicated,
- [ ] day-owner IDs canonicalized,
- [ ] panel routes declared,
- [ ] required authorities declared,
- [ ] event sources declared,
- [ ] generated docs/JSON,
- [ ] CI `--check` registered,
- [ ] host consumes manifest,
- [ ] deterministic ordering,
- [ ] flush policy explicit,
- [ ] lazy bind-time setup removed from migrated subsystems,
- [ ] three-subsystem pilot byte-for-byte equivalent,
- [ ] completeness tests green,
- [ ] omission test fails correctly.

---

# 26. Workstream 28B — Behaviour-Preserving Decomposition

## 26.1 Objective

Split large files only where the manifest already defines ownership seams.

---

# 27. 28B Phase A — Freeze Behaviour Baseline

Before any move, pin:

- golden save digests,
- snapshot hashes,
- test count,
- runtime-scale output,
- export smoke result.

No decomposition begins without a green baseline.

---

# 28. 28B Phase B — Risk-Ordered Split Sequence

Do not order purely by size.

Preferred sequence:

1. test-only mega-file,
2. UI partial,
3. host sessions with clear ownership seams,
4. other large files only where ownership is unambiguous.

---

# 29. 28B Phase C — `HostCli.PanelTests.cs`

Split by stable command/verb domain.

Potential partials:

- Campaign,
- Diagnostics,
- Expansion,
- Persistence,
- UI.

If dangling `.cs.uid` sidecars indicate intended files, reconcile them explicitly.

Do not infer ownership solely from sidecars; confirm code semantics.

---

# 30. 28B Phase D — `Main.UiPanels.cs`

Split by subsystem/domain.

Examples:

- survivors,
- expeditions,
- medical,
- economy,
- narrative,
- holdfast,
- maritime,
- verdict.

Naming should align with manifest subsystem ownership.

---

# 31. 28B Phase E — Host Session Splits

Only split:

- `SaveLoadHostSession`,
- `Phase0HostSession`,
- `AssetRegistry`,
- other large files

where multiple manifest ownership units are genuinely mixed.

A large file with one responsibility is acceptable.

---

# 32. 28B Mechanical-Move Rule

Each decomposition commit may contain only:

- code relocation,
- using adjustment required by compiler,
- namespace/partial placement required for compile.

Forbidden in same commit:

- rename,
- logic fix,
- API signature change,
- formatting sweep,
- dead-code cleanup,
- behavior change.

---

# 33. 28B Per-Move Verification

After each file move:

```bash
dotnet build
dotnet test
affected selftests
architecture map check
```

Also verify diff is recognizable as move/rename where Git permits.

One bad move should be one revert.

---

# 34. 28B Compile Inclusion Check

Confirm new partial files are included by existing project glob.

Add a test/gate if needed to catch:

```text
.cs.uid exists
but .cs missing
```

or:

```text
.cs exists
but compile item excludes it
```

---

# 35. 28B Architecture Map Regeneration

Regenerate:

```bash
scripts/ci/generate-architecture-map.sh --check
```

and code index/catalog docs.

Documentation should match actual ownership/files.

---

# 36. 28B Size Advisory

Add advisory report for large `src/**` files.

No hard gate initially.

Exclude:

- generated files,
- catalogs,
- intentionally monolithic data blobs.

Purpose:

```text
visibility, not formatting politics
```

---

# 37. 28B Performance Neutrality

At end of decomposition, compare:

- day-advance median/p95,
- allocations,
- exported boot time if tracked.

Expected:

```text
no meaningful regression
```

Any regression becomes a separate issue.

Do not “fix” it inside decomposition commit.

---

# 38. 28B Documentation Truth Pass

Update:

- `AGENTS.md` H7,
- architecture registry,
- ownership docs.

Replace outdated “single huge Main.cs” claims with actual structure:

```text
partial composition root
+ manifest-driven orchestration
```

---

# 39. 28B Definition of Done

- [ ] baseline frozen,
- [ ] splits ordered by risk,
- [ ] files split only along manifest ownership,
- [ ] test mega-file decomposed,
- [ ] UI mega-file decomposed,
- [ ] host sessions split only where justified,
- [ ] commits mechanical only,
- [ ] per-move tests green,
- [ ] compile inclusion verified,
- [ ] architecture map regenerated,
- [ ] size advisory added,
- [ ] perf neutral,
- [ ] docs updated.

---

# 40. Workstream 28C — Explicit Lifecycle Contracts

## 40.1 Objective

Make subsystem states impossible to be half-alive.

---

# 41. 28C Phase A — Define Lifecycle Stages

Create engine-free lifecycle vocabulary:

```text
Construct
Wire
Initialise
TickOwner?
Persist
Teardown
```

Use interfaces/records as appropriate.

Do not force every subsystem to implement every method if a stage is inapplicable.

But applicability must be explicit.

---

# 42. 28C Phase B — Current Lifecycle Inventory

For every manifest subsystem, record:

| Subsystem | Construct | Wire | Init | Tick | Persist | Teardown | Lazy setup? | Subscriptions? |
|---|---:|---:|---:|---:|---:|---:|---:|---:|

This inventory identifies incomplete lifecycles.

---

# 43. 28C Phase C — Separate Construction From Wiring

Construction:

```text
create object
```

Wiring:

```text
inject/bind dependencies
subscribe events
```

Initialization:

```text
load/prepare runtime state
```

Do not hide all three inside `SetupXxx`.

Thin wrappers may call staged lifecycle methods during migration.

---

# 44. 28C Phase D — Ban Lazy Construction During Panel Bind

Bind lambdas may only:

- obtain existing authority,
- subscribe,
- render.

They may not construct missing campaign systems.

Missing authority:

- development → throw descriptive error,
- release → log and refuse binding/route.

---

# 45. 28C Phase E — Mandatory Teardown Declaration

Any subsystem that:

- subscribes,
- owns disposable resources,
- holds session-scoped callbacks

must declare teardown.

Manifest validation checks:

```text
event subscriptions > 0
→ teardown required
```

or explicit stateless exception.

---

# 46. 28C Phase F — Session Replacement Protocol

Define ordered sequence for:

```text
New Game
Load
Session replacement
```

Suggested conceptual flow:

```text
freeze UI actions
→ unbind panels
→ teardown subsystem subscriptions
→ dispose old sessions
→ construct new subsystem set
→ wire
→ restore
→ initialize
→ register day owners
→ rebind panels
→ resume UI
```

Use actual host architecture.

---

# 47. 28C Phase G — Manifest-Driven Panel Rebind

All open player surfaces rebind via manifest route metadata.

Do not maintain a separate hand-written “panels to refresh after load” list.

---

# 48. 28C Phase H — Day Phase Declaration

Every day owner descriptor has explicit phase.

No accidental default phase.

Add gate:

```text
manifest day owner
→ phase specified
```

Document intended phase ordering.

---

# 49. 28C Phase I — Fail Fast on Missing Stage

Registration validates lifecycle completeness before gameplay begins.

Examples:

- save section declared but no capture,
- event source declared but authority unavailable,
- day owner declared but no owner instance,
- teardown required but absent.

Development:

```text
throw descriptive composition error
```

Release:

```text
log subsystem failure
disable/refuse incomplete subsystem
```

Do not silently continue as if alive.

---

# 50. 28C Phase J — Manifest-Wide Selftests

Extend existing lifecycle probes to walk manifest.

Examples:

- panel bind lifecycle,
- save/load UI failure selftest,
- authority identity selftest,
- teardown scan.

Avoid handwritten coverage limited to remembered subsystems.

---

# 51. 28C Phase K — Lifecycle Determinism

Construction/wiring ordering may affect:

- RNG fork order,
- event ordering,
- save output.

Create digest:

```text
ordered lifecycle sequence
```

Run twice with same commit/config.

Assert identical.

---

# 52. 28C Phase L — Teardown Completeness Scan

For subsystem event subscriptions:

- every subscription has corresponding unsubscription/disposable token,
- same delegate identity,
- teardown called on session replacement.

Integrate Plan 16C conventions.

---

# 53. 28C Phase M — New Game / Load Soak

Scenario:

```text
New Game
→ advance
→ Load
→ advance
→ New Game
→ repeat
```

Assert:

- no stale authority references,
- no duplicate handlers,
- no node growth,
- no duplicated day owners,
- no duplicate save capture,
- no duplicate event publication.

---

# 54. 28C Phase N — Lifecycle Performance

Measure:

- registration time,
- session rebind time,
- teardown time.

Ensure negligible relative to Plan 26C budgets.

No per-frame manifest work.

Manifest is orchestration metadata, not a hot-loop dependency.

---

# 55. 28C Documentation

Create:

```text
docs/architecture/LIFECYCLE.md
```

Include:

- stage definitions,
- idempotency rules,
- session replacement diagram,
- manifest fields,
- failure semantics,
- teardown convention,
- day-phase ordering.

Link from:

```text
docs/CURRENT_AUTHORITY.md
```

---

# 56. 28C Definition of Done

- [ ] lifecycle vocabulary exists,
- [ ] every subsystem lifecycle inventoried,
- [ ] construct/wire/init separated where needed,
- [ ] no lazy panel construction,
- [ ] teardown declared,
- [ ] session replacement protocol centralized,
- [ ] panel rebind manifest-driven,
- [ ] day phase explicit,
- [ ] missing stage fails loudly,
- [ ] lifecycle selftests walk manifest,
- [ ] ordering digest deterministic,
- [ ] teardown scan green,
- [ ] NewGame→Load→NewGame soak green,
- [ ] lifecycle perf negligible,
- [ ] docs published.

---

# 57. Integrated Orchestration Pipeline

```text
SubsystemManifest
      │
      ├─ save section ref
      ├─ lifecycle group
      ├─ setup/wire/init
      ├─ day owner + phase
      ├─ panel routes
      ├─ authority requirements
      ├─ events
      ├─ persist
      ├─ flush policy
      └─ teardown
      │
      ▼
SubsystemRegistration
      │
      ├─ deterministic construct
      ├─ deterministic wire
      ├─ initialize
      ├─ register day owners
      ├─ bind event consumers
      ├─ expose panel authorities
      ├─ save
      ├─ flush
      └─ teardown
      │
      ▼
Generated docs / CI completeness gates
```

---

# 58. Ownership Contract

For every subsystem, manifest must answer:

```text
Who owns it?
Who constructs it?
Who wires it?
Who initializes it?
Does it tick?
At what phase?
Does it persist?
Which save section?
Does it flush?
Which panels expose it?
Which authorities do those panels require?
Which events does it emit?
Who tears it down?
```

If one question cannot be answered, subsystem registration is incomplete.

---

# 59. Save Contract

Manifest descriptors with persistence must satisfy:

```text
SaveSectionRegistry key exists
AND
capture path exists
AND
restore path exists
AND
store delegation correct
```

Do not derive schema version from manifest.

---

# 60. Event Contract

For each declared event source:

- emitter authority exists,
- event type is stable,
- optional consumers known/derivable,
- semantic source owner ID aligns with day-owner ID where appropriate.

No “event class exists but nobody subscribes” without an explicit reason.

---

# 61. Panel Contract

For each live route:

```text
manifest subsystem
→ panel route
→ maturity/liveness
→ required authority
→ live campaign instance
```

Reference identity tests can be generated from this metadata.

---

# 62. Day Owner Contract

For every day owner:

- stable owner ID,
- explicit phase,
- one registration,
- declared source subsystem,
- deterministic ordering.

No default phase by accident.

---

# 63. Flush Contract

Subsystem descriptor declares:

```text
FlushPolicy:
- Required
- Immediate
- Stateless
- NotApplicable
```

or equivalent.

Gate checks the chosen policy.

---

# 64. File Ownership Contract

After 28B:

```text
one resulting file/domain cluster
→ one manifest ownership unit
```

Avoid:

- two files both acting as primary owner for same subsystem,
- one file mixing unrelated manifest subsystems unless justified shared infrastructure.

---

# 65. Failure Modes and Corrective Actions

## 65.1 Manifest duplicates save metadata

Fix:

- reference SaveSectionRegistry only.

## 65.2 Manifest becomes docs-only

Fix:

- host registration must consume it.

## 65.3 Registration order changes saves

Fix:

- deterministic ordering,
- golden digest comparison.

## 65.4 Large split includes logic fixes

Reject/revert.

Split separately from behavior changes.

## 65.5 Lifecycle interface forces meaningless methods

Fix:

- explicit applicability metadata,
- optional stages with reason.

## 65.6 Event subscriptions double after Load

Fix:

- teardown + manifest-driven rebind.

## 65.7 Day owner registered twice

Fix:

- one manifest descriptor,
- central registration,
- uniqueness gate.

## 65.8 Panel bind constructs authority

Fix:

- required-authority manifest contract,
- fail-fast.

## 65.9 Docs drift again

Fix:

- generated manifest/docs with `--check`.

---

# 66. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| ordering change breaks determinism | Medium | High | stable sort + digest |
| save orchestration drift | Medium | High | golden saves + registry refs |
| manifest duplicates truth | Medium | High | strict authority boundaries |
| generator becomes second authority | Low–Med | Medium | generated from executable manifest |
| 28B mixed with fixes | Medium | High | mechanical-only commit policy |
| missing compile partial | Low–Med | Medium | compile inclusion check |
| lifecycle teardown incomplete | Medium | High | subscription scan + soak |
| session rebind duplicates authority | Medium | High | manifest-wide identity tests |
| overbuilt orchestration framework | Medium | Medium | one table + one registration file |
| manifest hot-path use | Low | Low–Med | startup/rebind only |
| stale docs | High | Medium | `--check` gates |

---

# 67. Commit Strategy

## Commit C2[9].1 — Baseline + ownership inventory

- counts,
- triad discrepancies,
- event/panel/store inventory,
- digests.

## Commit C2[9].2 — `SubsystemDescriptor` + manifest seed

- no orchestration behavior change.

## Commit C2[9].3 — day owner constants + save-section references

## Commit C2[9].4 — panel authority/event metadata

## Commit C2[9].5 — generator + docs + CI gate

## Commit C2[9].6 — `SubsystemRegistration`

- deterministic setup/save/flush/day registration.

## Commit C2[9].7 — three-subsystem pilot

- byte-for-byte/golden parity.

## Commit C2[9].8 — full completeness migration

### Gate: 28A complete

## Commit C2[9].9 — HostCli test decomposition

## Commit C2[9].10 — Main.UiPanels decomposition

## Commit C2[9].11 — justified host-session decomposition

## Commit C2[9].12 — architecture docs/index + size advisory

### Gate: 28B complete

## Commit C2[9].13 — lifecycle stage vocabulary

## Commit C2[9].14 — construct/wire/init separation

## Commit C2[9].15 — teardown + session replacement protocol

## Commit C2[9].16 — manifest-wide rebind/day-phase enforcement

## Commit C2[9].17 — lifecycle selftests + determinism digest

## Commit C2[9].18 — NewGame/Load soak + perf closure

### Gate: 28C complete

## Commit C2[9].19 — integrated orchestration closure

---

# 68. Verification Checklist

Run per task and final closure:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-subsystem-manifest.py --check
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/generate-architecture-map.sh --check
godot --headless --path . -- --runtime-scale-selftest
bash scripts/ci/verify-fast.sh
```

Also verify:

```text
golden save digests unchanged
snapshots unchanged for 28B
exported boot smoke
session lifecycle selftests
```

---

# 69. Flagship Definition of Done

## 28A — Manifest

- [ ] one manifest type,
- [ ] one descriptor per subsystem,
- [ ] SaveSectionRegistry referenced,
- [ ] day-owner IDs stable,
- [ ] explicit phases,
- [ ] panel routes declared,
- [ ] authority requirements declared,
- [ ] events declared,
- [ ] flush policy explicit,
- [ ] deterministic order,
- [ ] generator outputs docs/JSON,
- [ ] CI manifest check,
- [ ] host registration consumes manifest,
- [ ] three-subsystem pilot equivalent,
- [ ] full completeness gate green,
- [ ] omission test proves failure.

## 28B — Decomposition

- [ ] baseline pinned,
- [ ] quiet tree,
- [ ] split by ownership,
- [ ] mechanical-only commits,
- [ ] test mega-file split,
- [ ] UI mega-file split,
- [ ] host splits justified,
- [ ] compile inclusion green,
- [ ] architecture map current,
- [ ] size advisory,
- [ ] runtime perf neutral,
- [ ] golden saves unchanged,
- [ ] snapshots unchanged,
- [ ] docs truthful.

## 28C — Lifecycle

- [ ] lifecycle stages explicit,
- [ ] per-subsystem inventory,
- [ ] no lazy bind-time construction,
- [ ] teardown declared,
- [ ] session replacement centralized,
- [ ] panel rebind manifest-driven,
- [ ] day phase explicit,
- [ ] incomplete lifecycle fails loudly,
- [ ] selftests manifest-wide,
- [ ] lifecycle digest deterministic,
- [ ] teardown completeness green,
- [ ] repeated NewGame/Load soak green,
- [ ] rebind cost within budget,
- [ ] lifecycle docs published.

## Cross-wave

- [ ] Plan 15C consumes panel manifest fields,
- [ ] Plan 16B/16C violations become manifest/lifecycle failures,
- [ ] Plan 17/24 event sources derive from declarations,
- [ ] Plan 26 diagnostics can report subsystem state,
- [ ] Plan 27 runtime identity checks consume authority metadata.

---

# 70. Closure Report Template

```markdown
## C2[9] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline
- Setup count:
- Save count:
- Flush count:
- Save sections:
- Day owners:
- Panel routes:
- Save stores:
- Event sources:
- Test count:
- Golden digest:
- Runtime-scale:

### 28A — Manifest
- Descriptor fields:
- Manifest subsystem count:
- Save registry refs:
- Day owner constants:
- Panel route coverage:
- Authority coverage:
- Event coverage:
- Flush policies:
- Registration order:
- Generator:
- CI gate:
- Pilot subsystems:
- Omission test:
- Result:

### 28B — Decomposition
- HostCli split:
- Main.UiPanels split:
- Host sessions split:
- Files moved:
- Logic edits in split commits:
- Compile inclusion:
- Architecture map:
- Golden digests:
- Snapshots:
- Perf delta:
- Result:

### 28C — Lifecycle
- Lifecycle stages:
- Subsystems with teardown:
- Lazy setups removed:
- Session replacement:
- Panel rebind:
- Default day phases remaining:
- Lifecycle selftests:
- Ordering digest:
- NewGame/Load soak:
- Stale refs:
- Duplicate subscriptions:
- Rebind perf:
- Result:

### Full Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Manifest check:
- Triad gate:
- Architecture map:
- Runtime scale:
- Golden saves:
- Snapshots:
- Verify fast:
- Export smoke:

### Final Metrics
- Undeclared subsystems:
- Orphan save sections:
- Duplicate day owners:
- Undeclared panel routes:
- Undeclared events:
- Missing teardown:
- Default phases:
- Registration order drift:
- Perf delta:

### Remaining Debt
- Manifest:
- File ownership:
- Lifecycle:
- Diagnostics:
- Runtime identity:
```

---

# 71. Final Execution Directive

Execute Plan 28 as an orchestration-truth repair.

The critical sequence is:

```text
declare every subsystem once
→ make registration consume the declaration
→ generate and gate the ownership view
→ prove parity on representative systems
→ migrate the rest
→ only then split files along ownership
→ then make lifecycle stages explicit
→ then make teardown/rebind deterministic
```

Do not use 28B as a cleanup excuse.

The most important anti-pattern to remove is:

```text
"remember to also add it to..."
```

After C2[9], that phrase should disappear from subsystem integration.

The strongest architectural rule is:

> **A subsystem is declared once; setup, save, flush, day ownership, panel exposure, authority requirements, event sources, and lifecycle obligations are derived or validated from that declaration.**

The strongest decomposition rule is:

> **28B commits move code only. If behavior changes, it is not a decomposition commit.**

The strongest lifecycle rule is:

> **A subsystem may be either fully alive or explicitly unavailable; half-initialized, half-subscribed, or half-persisted states must fail loudly.**
