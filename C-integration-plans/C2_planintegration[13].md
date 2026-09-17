# C2 — Flagship Integration Plan [13]: Port Contracts, Host Wiring Validation, and Unbound-Effect Closure

> **Deliverable:** `C2_planintegration[13].md`
> **Source:** Plan 36 — *The Port Contract: Unbound Effects Must Fail CI*
> **Wave:** Continuity Wave 5 — *The Human Interface*
> **Required order:** **35A → 36A → 36B → 36C**
> **Purpose:** make every production-required Core integration seam and host collaborator machine-declared, statically gateable, runtime-validatable, session-swap-safe, and measurable until the missing-port debt reaches zero.
> **Dependencies:** 35A delivery ports; 15C panel liveness; 27A/27B fidelity; 28A subsystem manifest; 31A semantic event vocabulary.
> **Hard boundaries:** no DI container, no new orchestration framework, no `Godot.*` in Core, no warning-only handling for missing required production ports, no gate weakening, and no deletion of an unwired seam until intent is classified.

---

# 0. Executive Summary

Across multiple continuity waves ASHFALL has repeatedly produced the same structural defect:

```text
Core behavior exists
→ unit tests prove it
→ host never plugs the integration seam in
→ CI stays green
→ gameplay never reaches the behavior
```

Examples already encountered include grief, sedation, meals, hunter skill, refrigeration, ration consumption, moral-choice resolution, daily world simulation, chain runners, null callbacks, and durability paths.

Plan 36 converts that entire class from an audit finding into an enforceable contract.

The target architecture is:

```text
Core effect / port
      │
      ▼
PortContract declaration
      ├─ owner
      ├─ requirement
      ├─ expected caller
      ├─ effect id
      └─ lifecycle stage
      │
      ▼
generated port manifest
      ├────────────► static CI gate
      └────────────► runtime host expectations
                           │
                           ▼
                     host session setup
                           │
                     bind required ports
                           │
                     IWiringReporter
                           │
                     boot validator
                           │
                ┌──────────┴──────────┐
                ▼                     ▼
              PASS                  FAIL LOUD
                │
         session replacement
                │
         validate everything again
```

C2[13] is complete when an unplugged production effect is not merely detectable by a reviewer; it is structurally unable to pass the project’s fast gates and exported boot smoke.

---

# 1. Source Diagnosis

The source plan identifies:

- 147 public Core methods shaped like integration seams.
- 74 with no caller in `src/`.
- 34 with no host caller, Core caller, or test caller.
- `ApplyGrief` and `ApplySedative` as test-only examples whose behavior appears intended but unreachable.
- `ApplyTreatment` as the opposite case: host-less at the direct surface but live through Core's medical pipeline.
- `CombatHostSession.ValidatePorts()` as an existing successful pattern.
- optimistic state-version validation already present in water-treatment actions.
- prior liveness and fixture-fidelity gates as precedent.

The conclusion is not that every host-less public method is broken.

The conclusion is that **every seam must state what class of seam it is** and the machine must enforce the class.

---

# 2. Program-Level Success Criteria

C2[13] closes only if:

1. Every integration-shaped Core seam is classified.
2. Every production-required host port has one contract authority.
3. Every required declared caller is observable.
4. Every constructed host subsystem can report required, bound, missing, and fallback dependencies.
5. End-of-boot validation returns zero missing required ports.
6. New Game and Load re-run the same validation.
7. Missing required wiring fails headless CI and exported boot smoke.
8. Current missing-port exemptions form a shrink-only ratchet.
9. `ApplyGrief` and `ApplySedative` are either genuinely wired or explicitly removed after intent review.
10. `HOST_MISSING` reaches zero by 36C or every residual item has a named owner/plan and dated exemption.
11. Content utilization distinguishes naming/loading from actual runtime effects.
12. No Core contract type depends on Godot.

---

# 3. Core Architectural Invariants

## 3.1 One contract vocabulary

Use one declaration system for all Core integration seams.

Accepted shapes:

- lightweight attributes, or
- one Core-side generated/declared table.

Do not create per-domain schemes.

## 3.2 Required/optional/test/internal/dead are explicit

No seam remains “unknown.”

## 3.3 Static and runtime halves reinforce each other

Static gate answers:

```text
Is the expected caller present?
```

Runtime validator answers:

```text
Did the campaign instance actually bind it?
```

Both are necessary.

## 3.4 Setup-time binding

Production-required effects bind during subsystem setup, not when:

- a panel opens,
- an event happens,
- a route is first visited.

## 3.5 Session replacement is a second boot

New Game and Load invalidate previous session wiring and must revalidate.

## 3.6 Exemptions are debt

An exemption does not mean “valid architecture.”

It means “known missing integration with an owner.”

## 3.7 No silent fallback

Fallbacks must be named and environment-scoped.

## 3.8 Every player-relevant effect becomes observable

After binding, semantic event routing via Plan 31 must tell downstream systems that the effect occurred where appropriate.

---

# 4. Dependency Graph

```text
35A delivery/effect ports
        │
        ▼
36A classify + declare + gate
        │
        ├─ static contract
        ├─ runtime ValidatePorts
        ├─ ratchet
        └─ grief/sedation pilots
        │
        ▼
36B host wiring contracts
        │
        ├─ declared collaborators
        ├─ IWiringReporter
        ├─ boot table
        ├─ session-swap validation
        ├─ 28A manifest alignment
        └─ stale-command rejection
        │
        ▼
36C long-tail sweep
        ├─ remaining ports
        ├─ unsubscribed events
        ├─ save asymmetry
        ├─ panel binds
        ├─ runtime content evidence
        └─ unread authored fields
```

Hard order:

```text
35A → 36A → 36B → 36C
```

---

# 5. Baseline Capture

Before behavior changes, freeze a full integration baseline.

## 5.1 Core seam inventory

Scan public Core methods matching:

```text
Bind*
Set*
Wire*
Register*
Apply*
Enable*
Configure*
```

Exclude demos/headless examples explicitly.

Capture:

- port/seam name,
- declaring type,
- file:line,
- host callers,
- Core callers,
- test callers,
- owner subsystem,
- provisional classification.

## 5.2 Host wiring inventory

Per host session record:

- subsystem constructed,
- mandatory collaborators,
- optional collaborators,
- callbacks/ports bound,
- current validation method,
- fallback behavior,
- session lifecycle.

## 5.3 Baseline metrics

Publish:

```text
PORT_SEAMS_TOTAL
PORT_SEAMS_NO_SRC_CALLER
PORT_SEAMS_NO_CALLER_ANYWHERE
PORTS_REQUIRED_HOST
PORTS_HOST_MISSING
PORTS_TEST_ONLY
PORTS_LIVE_VIA_CORE
PORTS_DEAD
HOST_SESSIONS
HOST_SESSIONS_WITH_VALIDATION
HOST_REQUIRED_COLLABORATORS_MISSING
HOST_FALLBACKS_ACTIVE
```

Re-measure current repo; do not hard-code the source-plan counts as future thresholds.

## 5.4 Baseline verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

---

# 6. Task 36A — Classify Every Seam, Then Gate the Classes

## Goal

Make “declared and never invoked” a CI-visible contract violation.

---

# 7. 36A.1 — Publish the Triage Table

Create one row per host-less seam:

| Seam | Owner | Host refs | Core refs | Test refs | Class | Evidence | Action |
|---|---|---:|---:|---:|---|---|---|

Classes:

- `LIVE_VIA_CORE`
- `HOST_MISSING`
- `DEAD`
- `TEST_ONLY`

If implementing a richer contract, represent these as requirement + reachability states, but preserve the source semantics.

---

# 8. 36A.2 — Classification Rules

## LIVE_VIA_CORE

Requires a real reachable Core call chain, not merely another textual reference.

## HOST_MISSING

Behavior is designed to ship and requires host wiring.

## DEAD

No intended runtime path remains.

## TEST_ONLY

A behavior is proven only by tests.

For `TEST_ONLY`, make an explicit product decision:

```text
WIRE
or
REMOVE
```

No indefinite limbo.

---

# 9. 36A.3 — Remove Genuine DEAD Seams

For each `DEAD`:

1. verify behavior is genuinely unwanted,
2. remove the seam,
3. remove misleading tests/docs,
4. regenerate catalogs,
5. record rationale.

A public API with no intended caller is architectural noise.

---

# 10. 36A.4 — Introduce the Core Port Contract

Create:

```text
Assets/Ashfall.Core/Ports/PortContract.cs
```

Possible conceptual declaration:

```csharp
[PortContract(
    PortId = "medical.sedation.apply",
    Owner = "medical",
    Requirement = PortRequirement.RequiredHost,
    RequiredCaller = "medical_ward_host",
    EffectId = "sedation_applied",
    LifecycleStage = PortLifecycleStage.Setup)]
```

The exact syntax should follow repo conventions.

---

# 11. 36A.5 — Contract Fields

Minimum useful fields:

```text
PortId
OwnerSubsystemId
Requirement
RequiredCallerId
EffectId
LifecycleStage
OptionalReason
DeprecatedReason
```

Prefer stable subsystem IDs over file names.

Generated reports may include file:line evidence separately.

---

# 12. 36A.6 — Engine-Free Enforcement

Add test/gate proving:

```text
Assets/Ashfall.Core/Ports/**
```

does not reference `Godot.*`.

Allowed:

- attributes,
- plain interfaces,
- strings,
- enums,
- DTOs.

---

# 13. 36A.7 — Generate Port Contract Artifacts

Create:

```text
scripts/ci/generate-port-contract.py
```

Outputs:

```text
docs/architecture/PORT_CONTRACT.md
docs/architecture/port-contract.json
```

Each row:

```text
seam
owner
declared caller
observed caller
requirement
classification
status
evidence
```

Generated files are never hand-edited.

---

# 14. 36A.8 — Register the Fast Gate

Add:

```bash
python3 scripts/ci/generate-port-contract.py --check
```

to the CI manifest.

Fail if:

- required caller missing,
- unclassified seam,
- duplicate PortId,
- owner absent,
- caller ID invalid,
- generated output stale.

---

# 15. 36A.9 — Runtime `ValidatePorts` Contract

Generalize combat’s successful model.

Any host session binding required effect ports must expose or participate in a setup-time check.

Validation returns:

```text
required ports
bound ports
missing ports
```

Development/headless behavior:

```text
missing required port → hard failure
```

Release behavior:

```text
refuse/disable incomplete subsystem + structured diagnostic
```

Never warning-only continuation.

---

# 16. 36A.10 — Unbound-Port Selftest

Add a narrow selftest or extend an existing lifecycle selftest.

Required output:

| Subsystem | Required | Bound | Missing |
|---|---:|---:|---:|

Summary:

```text
PORTS_REQUIRED=N
PORTS_BOUND=N
PORTS_MISSING=0
```

Non-zero if missing.

---

# 17. 36A.11 — Shrink-Only Ratchet

Land the gate against reality using a dated baseline file.

Every exemption contains:

```text
port_id
owner
date
reason
plan_or_issue
```

Allowed:

```text
count decreases
```

Forbidden:

```text
new missing production port
count increases silently
```

A new exemption requires explicit reviewed baseline change.

---

# 18. 36A.12 — Pilot: ApplyGrief

Target causal chain:

```text
survivor death
→ fate/memorial resolution
→ ApplyGrief
→ existing morale/psychological authority
→ semantic state event
→ briefing/journal visibility
```

Requirements:

- no new grief authority if existing systems already own the effect,
- exactly once,
- deterministic,
- persisted.

Tests:

- death with grief path,
- death path not requiring grief,
- save/load,
- event attribution.

---

# 19. 36A.13 — Pilot: ApplySedative

Target chain:

```text
medical chemical action
→ ApplySedative
→ existing calm/fatigue/dependency authority
→ semantic event
→ UI/briefing visibility
```

Do not build a new sedation simulation if the existing medical/needs/chemical systems own it.

Tests:

- valid sedative use,
- no-item/invalid treatment,
- deterministic effect,
- save/load,
- semantic event.

---

# 20. 36A.14 — Deterministic Binding

All required bindings happen during deterministic setup.

No:

```text
if panel opened then BindX
```

No:

```text
bind on first event
```

No:

```text
bind depending on previous UI state
```

---

# 21. 36A.15 — Gate Failure Proof

Create a sandbox fixture that intentionally drops one required binding.

Assert all relevant validation layers fail:

- contract/generator test,
- runtime port validation,
- headless wiring selftest.

A gate that has never been proven to fail is incomplete.

---

# 22. 36A Definition of Done

- [ ] full seam triage published,
- [ ] every seam classified,
- [ ] genuine dead seams removed,
- [ ] test-only seams dispositioned,
- [ ] Core PortContract exists,
- [ ] Core remains engine-free,
- [ ] port manifest generated,
- [ ] fast CI gate active,
- [ ] runtime ValidatePorts active,
- [ ] selftest prints missing count,
- [ ] shrink-only baseline active,
- [ ] ApplyGrief live or explicitly retired,
- [ ] ApplySedative live or explicitly retired,
- [ ] deterministic setup binding,
- [ ] intentional omission test fails.

---

# 23. Task 36B — Make Every Host Session Say What It Needs

## Goal

A campaign with an unplugged required collaborator refuses to call itself healthy.

---

# 24. 36B.1 — Host Contract Model

Create:

```text
src/Host/HostSessionContracts.cs
```

Host-only types may describe:

```text
session id
required collaborators
optional collaborators
required ports
bound ports
missing ports
fallbacks
```

Do not move Core ownership into HostSessionContracts.

---

# 25. 36B.2 — Nullable Dependency Audit

Scan host sessions for:

- constructor optionals,
- nullable fields,
- `= null!`,
- `?.` calls around required authority,
- lazy fallback creation.

Classify each collaborator:

```text
REQUIRED
OPTIONAL
FALLBACK
TEST_ONLY
```

---

# 26. 36B.3 — Requirement Rule

A collaborator is required when the subsystem’s advertised production behavior cannot function correctly without it.

Do not label it optional simply because code currently handles null.

---

# 27. 36B.4 — Introduce `IWiringReporter`

Conceptual interface:

```csharp
public interface IWiringReporter
{
    WiringReport GetWiringReport();
}
```

`WiringReport` includes:

```text
SessionId
RequiredCollaborators
BoundCollaborators
RequiredPorts
BoundPorts
Missing
ActiveFallbacks
```

---

# 28. 36B.5 — End-of-Boot Validation

After all setup:

```text
Main.Application
→ enumerate wiring reporters
→ aggregate one table
→ validate
→ fail headless boot if required missing
```

This is the host’s wiring truth.

---

# 29. 36B.6 — Structured Boot Table

Example:

| Subsystem | Req Collab | Bound Collab | Req Ports | Bound Ports | Missing | Fallbacks |
|---|---:|---:|---:|---:|---:|---:|

Machine summary:

```text
HOST_SESSIONS=N
HOST_WIRING_REQUIRED=N
HOST_WIRING_BOUND=N
HOST_WIRING_MISSING=0
HOST_FALLBACKS_ACTIVE=N
```

---

# 30. 36B.7 — Session-Swap Validation

Re-run after:

- New Game,
- Load,
- any campaign session replacement.

Required order:

```text
teardown old
→ rebuild
→ bind
→ report
→ validate
→ rebind panels
```

Coordinate with Plan 28C lifecycle contract.

---

# 31. 36B.8 — Tie to Plan 28A Manifest

Manifest required authorities and Host wiring report must agree.

Fail on:

```text
manifest says authority required
but host does not report it
```

or:

```text
host claims required authority
but manifest ownership omits it
```

Avoid two divergent requirement lists.

---

# 32. 36B.9 — Named Fallbacks

Every allowed fallback has:

```text
FallbackId
Owner
AllowedEnvironment
Reason
```

Suggested environment classes:

- `DEV_ONLY`
- `TEST_ONLY`
- `HEADLESS_ONLY`
- `PRODUCTION_OPTIONAL`

Unexpected production fallback is a failure.

---

# 33. 36B.10 — Ban Silent Production Fallback

Do not allow a session to silently replace missing real data/authority with seed/default state.

If a production fallback is genuinely part of design, declare it as such.

---

# 34. 36B.11 — Shared Optimistic Concurrency

Generalize the existing:

```text
expectedStateVersion
currentStateVersion
```

pattern for stale-sensitive UI mutation commands.

Flow:

```text
render version V
→ user acts
→ submit expected V
→ compare current
→ execute or reject
```

---

# 35. 36B.12 — High-Risk Action Surfaces

Prioritize:

- inventory,
- treatment,
- expedition preparation,
- trade,
- power,
- duty/roster.

Read-only views do not need artificial versioning.

---

# 36. 36B.13 — Stale Command Result

Return structured result:

```text
success
reason_key
expected_version
current_version
```

Use Plan 25 localization for presentation.

---

# 37. 36B.14 — Metrics and Artifacts

Publish each run:

```text
required ports
bound ports
missing ports
required collaborators
missing collaborators
fallbacks active
```

Write into artifacts and CI status docs.

---

# 38. 36B.15 — Exported Build Validation

Plan 26B smoke must execute the same host wiring validator.

The shipped artifact is unhealthy if:

```text
HOST_WIRING_MISSING != 0
```

---

# 39. 36B Tests

- complete boot,
- missing required port,
- missing required collaborator,
- named fallback,
- illegal production fallback,
- New Game revalidation,
- Load revalidation,
- stale command rejection,
- valid versioned action,
- manifest mismatch,
- exported smoke validation.

---

# 40. 36B Definition of Done

- [ ] HostSessionContracts exists,
- [ ] nullable dependency audit complete,
- [ ] requirements explicit,
- [ ] IWiringReporter implemented,
- [ ] one end-of-boot table,
- [ ] missing required wiring fails headless,
- [ ] New Game revalidates,
- [ ] Load revalidates,
- [ ] Plan 28A cross-check active,
- [ ] fallbacks named,
- [ ] production fallback policy enforced,
- [ ] stale mutation protection shared,
- [ ] high-risk surfaces covered,
- [ ] metrics emitted,
- [ ] exported boot uses validator.

---

# 41. Task 36C — Close the Four-Wave Long Tail

## Goal

Use the machinery to eliminate the remaining integration ghosts systematically.

---

# 42. 36C.1 — Report-Only First

Run the new tooling without fixing anything.

First output:

```text
complete offender table
```

not a code diff.

Sources:

- port contract,
- host wiring report,
- content utilization,
- field utilization,
- event subscription scan,
- save parity scans,
- panel liveness.

---

# 43. 36C.2 — Unified Offender Model

Columns:

| Finding | Owner | Category | Runtime Evidence | Status | Action | Plan |
|---|---|---|---|---|---|---|

Finding types:

- unbound Core port,
- missing collaborator,
- dead API,
- unsubscribed event,
- capture without restore,
- restore without capture,
- save store not registered,
- live panel without authority,
- catalog loaded but no effect,
- authored field unread.

---

# 44. 36C.3 — Group by Owning Wave

Group into:

- decision/story,
- physical systems,
- production/shipping,
- outside world,
- human interface.

Fix within the architecture that owns the behavior.

Avoid one giant miscellaneous wiring patch.

---

# 45. 36C.4 — Resolve Every `HOST_MISSING`

Each receives:

```text
WIRE
DELETE
or
EXEMPT WITH OWNER/PLAN
```

No anonymous TODO.

Final target:

```text
HOST_MISSING = 0
```

unless explicitly scoped follow-up work is accepted.

---

# 46. 36C.5 — Sibling Public API Scan

Expand beyond method-name prefixes.

Detect potential ghosts:

- public methods with only test callers,
- events with no live subscriber,
- callback properties never assigned,
- interfaces implemented but never consumed.

Use heuristics to find candidates, then classify manually/semantically.

---

# 47. 36C.6 — Event Subscription Coverage

For each declared event source:

```text
owner
event
declared consumers
observed subscribers
semantic event mapping
```

No silent event source if player-relevant behavior is expected.

---

# 48. 36C.7 — Capture/Restore Symmetry

Scan for:

- `CaptureState`,
- `RestoreState`,
- `Save*`,
- `Load*`.

Flag:

```text
capture only
restore only
registered store with no owner
owner with no registered store
```

Integrate SaveSectionRegistry and Plan 28A manifest.

---

# 49. 36C.8 — Panel Binding Coverage

Reuse 15C and 28A.

Every live player route requires:

- live campaign authority,
- required port bindings,
- runtime identity validation.

---

# 50. 36C.9 — Runtime Content Utilization

Strengthen content evidence.

Suggested tiers:

```text
DISCOVERED
LOADED
READ
EFFECT_PRODUCED
PLAYER_OBSERVED
```

A catalog referenced by a class name or loader alone does not count as fully consumed.

---

# 51. 36C.10 — Dead Catalog Resolution

For each catalog lacking effects:

- wire it,
- mark intentional codex-only,
- retire it.

Record decision.

---

# 52. 36C.11 — Field Utilization

Merge Wave-1 18C style field utilization into the same reporting pass.

Track:

```text
field authored
field parsed
field read
field changes effect
```

Target zero unexplained authored-but-unread gameplay fields.

---

# 53. 36C.12 — Documentation Truth Pass

Correct:

- implementation registry,
- AGENTS known issues,
- subsystem manifest,
- port contract docs,
- content utilization.

Every changed claim should have source evidence.

---

# 54. 36C.13 — Explicit Closure Metrics

Publish before/after:

```text
HOST_MISSING
UNBOUND_REQUIRED_PORTS
MISSING_COLLABORATORS
UNSUBSCRIBED_REQUIRED_EVENTS
SAVE_ASYMMETRIES
EFFECT_PRODUCED_CATALOGS
UNREAD_GAMEPLAY_FIELDS
```

Hard targets:

```text
HOST_MISSING = 0
UNBOUND_REQUIRED_PORTS = 0
MISSING_COLLABORATORS = 0
```

---

# 55. 36C.14 — Permanent Regression Fixture

Keep a sandbox subsystem/session that deliberately omits a requirement.

Expected:

- static gate fails,
- runtime validator fails,
- headless selftest fails.

This test remains forever.

---

# 56. 36C.15 — Timebox Residual Debt

Every item not closed during the sprint receives:

- named owner,
- plan number,
- dated exemption,
- expected removal condition.

No bare TODO.

---

# 57. 36C Definition of Done

- [ ] report-only baseline generated,
- [ ] unified offender table,
- [ ] findings grouped by owner,
- [ ] HOST_MISSING driven to zero or formally owned,
- [ ] sibling API scan,
- [ ] event subscription audit,
- [ ] save symmetry audit,
- [ ] panel authority audit,
- [ ] runtime content evidence,
- [ ] field utilization merged,
- [ ] documentation corrected,
- [ ] explicit closure metrics published,
- [ ] regression fixture retained,
- [ ] residual debt numbered and owned.

---

# 58. Port Classification Model

Recommended final machine vocabulary:

```text
REQUIRED_HOST
OPTIONAL_HOST
LIVE_VIA_CORE
TEST_ONLY
DEPRECATED
DEAD
```

This can coexist with source-plan categories:

```text
LIVE_VIA_CORE
HOST_MISSING
DEAD
TEST_ONLY
```

where `HOST_MISSING` is computed from a required-host declaration lacking an observed/bound caller.

---

# 59. Port Contract Ownership Matrix

Every port answers:

```text
Who owns it?
What effect does it represent?
Is it required?
Who is expected to bind/call it?
At what lifecycle stage?
How is missing wiring validated?
How is its effect observed?
```

No unanswered field for production-required effects.

---

# 60. Host Wiring Ownership Matrix

Every host session answers:

```text
Which subsystem does it host?
Which collaborators are mandatory?
Which ports does it bind?
Which fallbacks are allowed?
Which lifecycle rebuilds recreate it?
How is it validated after rebuild?
```

---

# 61. Runtime Failure Policy

## Development and CI

Missing production-required binding:

```text
hard fail
```

## Exported smoke

Missing production-required binding:

```text
non-zero exit
```

## Player release

Prefer:

```text
refuse unsafe/incomplete campaign startup
+ structured diagnostic
```

over running a subtly empty simulation.

---

# 62. Named Fallback Policy

Each fallback has:

```text
id
owner
environment
reason
visibility
```

No fallback may become the implicit default merely because a collaborator is null.

---

# 63. Manifest Cross-Check

Plan 28A subsystem metadata and PortContract metadata should cross-validate:

```text
required authority
↔ host collaborator
↔ bound effect ports
```

Avoid duplicating subsystem requirements in two disconnected tables.

---

# 64. Lifecycle Alignment

Plan 28C lifecycle stages should drive port binding expectations:

```text
Construct
→ Wire
→ Initialise
→ Tick
→ Persist
→ Teardown
```

Required ports should normally be complete by the end of `Wire`/setup.

---

# 65. Session Replacement Contract

New Game/Load:

```text
teardown old subscriptions
→ create new authorities
→ wire required ports
→ run host wiring validation
→ restore/rebind panels
→ resume simulation
```

Never reuse old session callbacks.

---

# 66. Semantic Event Alignment

Plan 31 must receive semantic events for player-relevant effects.

Example grief:

```text
effect applied
→ morale changes
→ semantic grief/morale event
→ briefing/journal
```

A wired but invisible effect can still be a continuity defect.

---

# 67. Stale Action Contract

For mutable UI:

```text
panel reads state version
→ user chooses action
→ action includes expected version
→ system compares
→ accept or reject
```

Rejecting stale input must be attributable and localized.

---

# 68. Save Contract Alignment

Stateful effect application must obey:

- correct authority mutation,
- correct save section,
- round-trip,
- session rebind.

No port should mutate a temporary projection that save never sees.

---

# 69. Content-Evidence Alignment

Port wiring reports should cross-reference runtime content evidence.

A catalog-driven effect is only fully integrated when:

```text
data loads
→ system reads it
→ port/effect executes
→ state changes
→ player can observe where designed
```

---

# 70. Metrics Dashboard

Publish:

```text
PORTS_TOTAL
PORTS_REQUIRED_HOST
PORTS_BOUND
PORTS_HOST_MISSING
PORTS_LIVE_VIA_CORE
PORTS_TEST_ONLY
PORTS_DEAD
HOST_SESSIONS
HOST_MISSING_COLLABORATORS
HOST_FALLBACKS_ACTIVE
EVENTS_REQUIRED_UNSUBSCRIBED
SAVE_ASYMMETRIES
CATALOGS_EFFECT_PRODUCED
FIELDS_AUTHORED_UNREAD
```

Ratchet toward zero for defect categories.

---

# 71. Pilot End-to-End Tests

## Grief

```text
death
→ memorial/fate outcome
→ grief port
→ canonical morale/psych state
→ semantic event
→ save/load
```

Assert exactly one application.

## Sedation

```text
valid sedative action
→ sedation port
→ canonical fatigue/calm/dependency state
→ semantic event
→ save/load
```

Assert no effect for invalid action.

---

# 72. Long-Session Wiring Soak

Scenario:

```text
boot
→ New Game
→ validate
→ advance
→ save
→ load
→ validate
→ advance
→ New Game
→ validate
```

Assert:

- zero missing required ports,
- zero stale collaborators,
- zero duplicate subscriptions,
- stable port counts,
- stable event counts.

---

# 73. Exported Artifact Contract

Plan 26B exported smoke should output:

```text
PORTS_HOST_MISSING=0
HOST_WIRING_MISSING=0
```

and fail otherwise.

---

# 74. Failure Modes

## Gate passes but seam is falsely LIVE_VIA_CORE

Require call-chain evidence.

## Gate is noisy so exemptions grow

Reject; baseline may only shrink without explicit reviewed change.

## Missing port logs warning and continues

Change to hard failure.

## Test-only behavior is removed despite intended design

Reclassify and wire instead.

## Boot validates but Load does not

Add lifecycle revalidation.

## Host contract duplicates manifest

Derive or cross-check 28A.

## Production uses headless fallback

Fail environment policy.

## Stale UI mutates newer state

Apply version contract.

## Catalog counted because loader mentions it

Require effect-produced runtime evidence.

---

# 75. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| classification false positive | Medium | Medium | evidence table |
| false LIVE_VIA_CORE | Medium | High | real call-chain proof |
| initial gate noise | High | Medium | dated ratchet |
| exemptions become permanent | Medium | High | zero target + owners |
| hard failures expose many latent gaps | High | Medium | intentional; clear diagnostics |
| duplicate requirement authorities | Medium | High | 28A cross-check |
| stale-version retrofit too broad | Medium | Medium | high-risk surfaces only first |
| wanted behavior deleted | Medium | High | intent review before DEAD |
| Core polluted with engine types | Low | High | architecture test |
| export wiring differs from source tree | Medium | High | 26B smoke |

---

# 76. Commit Strategy

## C2[13].1 — inventory and triage
No behavior change.

## C2[13].2 — Core PortContract model

## C2[13].3 — generator + docs + CI check

## C2[13].4 — runtime ValidatePorts infrastructure

## C2[13].5 — ratchet + selftest

## C2[13].6 — ApplyGrief pilot

## C2[13].7 — ApplySedative pilot

### Gate: 36A complete

## C2[13].8 — HostSessionContracts/IWiringReporter

## C2[13].9 — required-collaborator migration

## C2[13].10 — boot validator

## C2[13].11 — session-swap revalidation

## C2[13].12 — manifest alignment + fallbacks

## C2[13].13 — stale-action version contract

## C2[13].14 — exported boot integration

### Gate: 36B complete

## C2[13].15 — report-only closure baseline

## C2[13].16 — HOST_MISSING batch 1

## C2[13].17 — HOST_MISSING batch 2

## C2[13].18 — event/save/panel sibling sweep

## C2[13].19 — runtime content/field evidence

## C2[13].20 — docs + target-zero closure

## C2[13].21 — permanent regression fixture

### Gate: 36C complete

## C2[13].22 — integrated closure

---

# 77. Verification Checklist

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-port-contract.py --check
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

Also:

```text
exported build boot smoke
intentional unbound-port regression fixture
NewGame→Load→NewGame wiring soak
runtime content-evidence report
```

---

# 78. Flagship Definition of Done

## 36A

- [ ] all seams classified,
- [ ] no phantom DEAD APIs,
- [ ] test-only seams resolved,
- [ ] one Core PortContract vocabulary,
- [ ] generated manifest,
- [ ] fast CI gate,
- [ ] runtime ValidatePorts,
- [ ] selftest count,
- [ ] shrink-only exemptions,
- [ ] grief pilot,
- [ ] sedation pilot,
- [ ] deterministic setup binding,
- [ ] deliberate regression failure test.

## 36B

- [ ] every host session declares requirements,
- [ ] nullable dependencies audited,
- [ ] IWiringReporter,
- [ ] one boot wiring table,
- [ ] zero missing required wiring on healthy boot,
- [ ] session-swap revalidation,
- [ ] manifest agreement,
- [ ] named fallbacks,
- [ ] stale mutation guard,
- [ ] metrics artifact,
- [ ] exported artifact validation.

## 36C

- [ ] report-only baseline,
- [ ] unified offender table,
- [ ] HOST_MISSING=0 or formally owned residuals,
- [ ] unbound required ports=0,
- [ ] missing collaborators=0,
- [ ] sibling API scan,
- [ ] event subscription scan,
- [ ] save symmetry scan,
- [ ] panel authority scan,
- [ ] runtime content evidence,
- [ ] field utilization,
- [ ] docs corrected,
- [ ] permanent regression fixture,
- [ ] no anonymous TODO debt.

## Global

- [ ] no gate weakened,
- [ ] no Godot in Core,
- [ ] no new DI framework,
- [ ] no silent production fallback,
- [ ] all player-relevant effects reportable,
- [ ] full verification green.

---

# 79. Closure Report Template

```markdown
## C2[13] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- PORTS_TOTAL:
- PORTS_NO_SRC_CALLER:
- PORTS_NO_CALLER_ANYWHERE:
- HOST_MISSING:
- TEST_ONLY:
- DEAD:
- LIVE_VIA_CORE:
- HOST_SESSIONS:
- HOST_MISSING_COLLABORATORS:
- FALLBACKS_ACTIVE:

### 36A
- Triage:
- PortContract:
- Generator:
- CI gate:
- Runtime validation:
- Ratchet:
- ApplyGrief:
- ApplySedative:
- Regression proof:
- Result:

### 36B
- Sessions migrated:
- IWiringReporter:
- Boot validator:
- New Game:
- Load:
- Manifest mismatches:
- Named fallbacks:
- Stale-version actions:
- Export smoke:
- Result:

### 36C
- Findings:
- HOST_MISSING before:
- HOST_MISSING after:
- Missing collaborators:
- Dead seams removed:
- Unsubscribed events:
- Save asymmetries:
- Panel gaps:
- EFFECT_PRODUCED catalogs before:
- EFFECT_PRODUCED catalogs after:
- Unread fields before:
- Unread fields after:
- Residual exemptions:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Port-contract check:
- Panel lifecycle:
- Content utilization:
- Verify fast:
- Export smoke:
- Regression fixture:

### Remaining Debt
- Ports:
- Host wiring:
- Events:
- Save:
- Content:
- UI:
```

---

# 80. Final Execution Directive

Execute Plan 36 as a machine-enforced integration-completeness repair.

The critical sequence is:

```text
classify every seam
→ declare intended caller and requirement
→ generate/gate the contract
→ validate bindings at runtime
→ make every host session report wiring
→ validate again after session replacement
→ reject stale mutation commands
→ sweep the remaining ghosts
→ drive missing-port debt toward zero
```

Do not weaken the gate to make the repository appear complete.

Do not accept unit-test coverage as proof that a behavior is reachable in gameplay.

Do not delete an unbound effect before deciding whether the behavior it promises is wanted.

The strongest Core rule is:

> **A production-required port must have a declared owner, a declared caller, an observed binding, and a setup-time validation path.**

The strongest host rule is:

> **Every host session must be able to state exactly what it requires, what it bound, what is missing, and which fallbacks are active.**

The strongest closure rule is:

> **`HOST_MISSING`, unbound required ports, and missing required collaborators are debt counters that may only move toward zero.**

The flagship regression proof is:

> **Remove one required binding in a sandbox fixture. Static contract validation, runtime host validation, and headless CI must all fail before the build can be called healthy.**
