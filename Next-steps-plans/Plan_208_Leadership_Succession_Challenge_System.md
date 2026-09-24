# Plan 208 — Leadership Succession & Challenges — Live Governance Owner
> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome
Seal one policy or succession player journey through the existing social leadership owner, with real catalog
loading, lawful command route, saved result and governance/morale projection.

**Current state:** CORE AND SOCIAL HOST LIVE: DEC-195 signed five policy templates, successor/deputy
designation, elections, challenges, crisis morale and capture/restore. SurvivorSocialCoordinator owns a
LeadershipSystem, saves it inside survivor_social and exposes designation/challenge commands. Current src
search found no load of leadership_policies.json, so policy catalog consumption and player route require
premise checks.

**Non-goals:** No new leadership system, succession save section, second approval or morale ledger,
automatic policy election without a player/owner command, or duplicate survivor designation in panels.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status
The current or candidate domain authority is `SurvivorSocialCoordinator.Leadership is the campaign
LeadershipSystem authority; ShelterGovernanceHostSession consumes it for stability`. Source and adjacent
paths inspected for this revision (some are candidate consumers rather than active bindings):
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`, `src/Main.ShelterSocial.cs`,
`src/Main.ShelterGovernance.cs`, `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`,
`src/UI/SurvivorRelationsPanel.cs`, and `Assets/StreamingAssets/Data/leadership_policies.json`. The focused
test starting point is `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`.
Persistence boundary under audit: `survivor_social section contains LeadershipSaveState`. Paths are evidence
pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-9/C9 names
leadership as survivor interiority and DM-1 shelter governance; DEC-195 is the newer signed implementation
authority. The master’s faction/standing maps do not authorize a separate legitimacy currency. Its Part II
requires live premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall.
The relevant deep maps and lane matrices guide coverage; they do not override newer code. The master
compilation itself warns against padding and stale repository assumptions. This plan therefore records
concrete contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall
- **Evidence or explicit premise:** DEC-195 marks Plan 208 sealed as a pure domain system with five policies
  and save/restore.
- **Evidence or explicit premise:** SurvivorSocialCoordinator constructs one LeadershipSystem, routes morale
  to NeedsSystem, exposes leadership commands, and captures/restores LeadershipSaveState.
- **Evidence or explicit premise:** SaveSectionRegistry already has survivor_social; a separate leadership
  save section would duplicate it.
- **Evidence or explicit premise:** LeadershipSystem.LoadCatalog/SetPolicy exist, but no src load of
  leadership_policies.json was found in this audit; confirm before claiming policy effects live.
- **Evidence or explicit premise:** ShelterGovernanceHostSession receives the same LeadershipSystem through
  Main.ShelterGovernance, so stability must read this owner.
- **Evidence or explicit premise:** SurvivorRelationsPanel has social read-model leadership fields; command
  reachability and policy controls need a route audit.

### Leadership custody and policy dossier

The old Plan 208 described a missing system, but `LeadershipSystem` now implements the signed DEC-195 rules.
`SurvivorSocialCoordinator` constructs it, binds Needs morale sinks, advances it daily, forwards leader
death/injury/crisis facts, and embeds its state in `survivor_social`. `ShelterGovernanceHostSession` reads
that same instance. A “new” succession system or save section would create two leaders. The first
integration action is to verify the active campaign setup path and the existing social command route, then
add the smallest missing binding.

The five `leadership_policies.json` rows exist and `LeadershipSystem.LoadCatalog` exists, but a source
search did not find a host call loading that file. Current `SetPolicy` accepts an arbitrary ID when the
catalog dictionary is empty, so the binding and strict validation must precede a player-facing selector.
This is a concrete candidate gap. The implementing package should load and validate the catalog before
restoring a saved `ActivePolicyId`, define behavior for a removed policy ID, and show the policy’s actual
rule in the panel. It must not assume five distinct modes are already live just because the Core catalog is
present.

A reviewable residual journey is: designate a successor → leader death → canonical promotion → governance
read-model update → save/reload → no duplicate morale or challenge effects. Policy selection and challenge
UI are separate bounded slices. Any player policy command must use `LeadershipSystem.SetPolicy` through the
social host and show precise refusal; no state mutation in `PoliticsUI` or `SurvivorRelationsPanel`.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/leadership_policies.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.ShelterSocial.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `survivor_social section contains LeadershipSaveState` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/SurvivorRelationsPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
| cross-system effect | `destination subsystem named in each phase` | Call the owner method once; do not copy its mutable state. |

## 5. Data and identity contract
Canonical IDs come from the existing catalogs and runtime facts. Case handling, trimming and comparison must
match the owning system. Proposed new IDs need a schema and reference validator before a producer can emit
them. Read the current JSON fields before extending a row; do not use the old plan’s desired row count as an
acceptance measure. Reject duplicates or conflicting definitions with per-row diagnostics. Treat display
names and prose as presentation, never identity. Preserve ordinal ordering for stable output and save
checksum inputs.
A change to catalog shape requires an old-catalog compatibility rule, one example valid row, one invalid
row, and a reader inventory. New prose must state a real observed consequence; it cannot promise trade,
safety, quest or reward behavior until a consumer reads the corresponding Core fact. Avoid real-world names
or copied narrative.

## 6. C# implementation sketch

```csharp
// Existing social authority; exact host route needs verification.
LeadershipSystem leadership = socialCoordinator.Leadership;
leadership.LoadCatalog(policyJson); // once before restoring saved policy ID
bool accepted = socialCoordinator.DesignateSuccessor(candidateId);
// survivor_social save captures leadership through coordinator.CaptureState().
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration
Inspect `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` and the registry for the named concern;
absence of a section is a decision gate, not permission to invent one. Write down exact DTO version,
constructor baseline, capture point, restore point, and dirty flag. For a stateless bridge, persist only the
source and destination authorities; do not create a bridge section. If a new mutable field is genuinely
required, version the existing owner DTO and prove migration from the preceding schema. Never equate a panel
cache with campaign state.
The restore sequence is: catalog validated; source state restored; destination state restored; adapters
bound; pending effects reconciled once; UI bound; day processing resumes. A new campaign starts with an
explicit empty/default state. A legacy save without the field takes that same baseline without inventing
past events. Unknown future schema versions must fail or degrade according to the existing save contract,
with a visible diagnostic. Corrupt single records must not silently reset an entire unrelated section.

## 8. Event, day and failure semantics
One semantic fact should carry stable source ID, campaign day, owning entity ID, content ID if any, and
enough context for the destination owner to decide. The host may route the fact but may not calculate a
competing gameplay rule. Define when the fact is emitted, when a command is committed, and what is durable
before the next callback. A read-only query must not create journal entries or rewards on every panel
refresh.
Use the canonical day owner only for behavior that genuinely changes with time. Preserve the current day
phase ordering and forked seeded RNG contract. No wall-clock seed, hash-order iteration, or System.Random
belongs in deterministic Core. Replay after load must not repeat a completed effect; an effect that was not
committed must remain recoverable. When a destination owner rejects a command, keep the source fact and show
a reason rather than writing a partial substitute effect.

## 9. Player commands and UI
Inspect the current route to `src/UI/SurvivorRelationsPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and uncertainty
where the game cannot know more. Refresh on authoritative state change and after restore; unbind
subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 10. Dependency-ordered implementation phases

### Phase 0 — Premise and claim

**Action:** Read current source/data; diff the old plan against delivered work; claim exact paths before an
edit. **Gate:** A signed package lists files, owner, non-goals, acceptance and focused command.
**Dependency:** Phase 0 requires a claim; later phases require the preceding gate. Shared composition roots
remain integrator-owned; if a phase needs them, package the exact seam for the integrator.

### Phase 1 — Core/consumer contract

**Action:** Identify one producer fact and one destination owner. Add the smallest typed query or command if
the owner agrees. **Gate:** A domain unit exercise proves input, refusal, and stable identity.
**Dependency:** Phase 0 requires a claim; later phases require the preceding gate. Shared composition roots
remain integrator-owned; if a phase needs them, package the exact seam for the integrator.

### Phase 2 — Data validation

**Action:** Extend only the existing catalog as required by Phase 1; validate references and ranges.
**Gate:** One valid and one invalid row produce clear, reproducible results. **Dependency:** Phase 0
requires a claim; later phases require the preceding gate. Shared composition roots remain integrator-owned;
if a phase needs them, package the exact seam for the integrator.

### Phase 3 — Save and restore

**Action:** Bind existing section; introduce versioned state only for genuinely new durable facts. **Gate:**
Round-trip, old-save baseline, and replay-after-restore agree. **Dependency:** Phase 0 requires a claim;
later phases require the preceding gate. Shared composition roots remain integrator-owned; if a phase needs
them, package the exact seam for the integrator.

### Phase 4 — Host composition

**Action:** Bind in the current session at correct setup order, route result to destination owner once.
**Gate:** A focused host-level check observes the real destination effect. **Dependency:** Phase 0 requires
a claim; later phases require the preceding gate. Shared composition roots remain integrator-owned; if a
phase needs them, package the exact seam for the integrator.

### Phase 5 — UI and narrative

**Action:** Bind existing panel or claimed route to a read model; show action and blocker truthfully.
**Gate:** A headless or bounded runtime check opens/refreshes/closes without a stale claim. **Dependency:**
Phase 0 requires a claim; later phases require the preceding gate. Shared composition roots remain
integrator-owned; if a phase needs them, package the exact seam for the integrator.

### Phase 6 — Acceptance and handoff

**Action:** Run only directly affected focused test files and necessary runtime probe. **Gate:** Handoff
includes exact commands/results, limitations, and shared paths untouched. **Dependency:** Phase 0 requires a
claim; later phases require the preceding gate. Shared composition roots remain integrator-owned; if a phase
needs them, package the exact seam for the integrator.

## 11. File impact map

| Path | Action after claim | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/leadership_policies.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.ShelterSocial.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.ShelterGovernance.cs` | READ / integrator MODIFY | composition root | high |
| `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/SurvivorRelationsPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback
Use `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
for the first contract check, then only the directly affected save and host targets identified by Phase 0.
Run a Godot headless probe only if the claimed change affects the runtime host path. A compile result cannot
establish live route reachability. Acceptance requires: (1) one real input; (2) one canonical consumer
effect; (3) one durable restore; (4) repeat delivery without duplicate effect; (5) visible correct state and
blocker; (6) no new authority.
Keep changes in reviewable phase commits. If a consumer hook fails, revert that phase without replacing the
canonical owner; preserve old saves and catalog compatibility. If a migrated section cannot load, stop
before adding UI or content and give the integrator the exact schema and fixture. Record any deferred edge
with a current evidence pointer and promotion condition rather than claiming it complete.

## 13. Detailed integration acceptance cards
The following cards are planning checks, not a request to create one test method per card. Each card names a
feature-specific outcome and the crosscutting evidence needed to accept it. Select the smallest independent
cases that prove the changed contract, save/load, determinism, lifecycle, and cross-system behavior. “Legacy
intent” cards are explicitly conditional: first prove their premise and owner, then either promote as a
separate bounded package or mark them retired. This prevents the 2026-09-01 plan text from resurrecting
already delivered or contradictory architecture.

### 01. meritocratic policy row [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns meritocratic policy row. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** policy_meritocratic_appointment resolves after catalog load and survives
the survivor_social round trip.

**Fresh campaign path.** For meritocratic policy row, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For meritocratic policy row, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For meritocratic policy row, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For meritocratic policy row, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For meritocratic policy row, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For meritocratic policy row, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For meritocratic policy row, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For meritocratic policy row, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 02. democratic election policy [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns democratic election policy. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** policy_democratic_election must gate election legality through
LeadershipSystem, not a panel-only button.

**Fresh campaign path.** For democratic election policy, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For democratic election policy, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For democratic election policy, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For democratic election policy, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For democratic election policy, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For democratic election policy, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For democratic election policy, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For democratic election policy, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 03. hereditary succession policy [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns hereditary succession policy. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** policy_hereditary_succession needs a canonical lineage candidate query
before it can choose an heir.

**Fresh campaign path.** For hereditary succession policy, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For hereditary succession policy, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For hereditary succession policy, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For hereditary succession policy, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For hereditary succession policy, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For hereditary succession policy, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For hereditary succession policy, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For hereditary succession policy, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 04. elder council policy [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns elder council policy. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** policy_elder_council may change rule selection only through the loaded
policy definition and signed command.

**Fresh campaign path.** For elder council policy, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For elder council policy, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For elder council policy, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For elder council policy, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For elder council policy, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For elder council policy, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For elder council policy, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For elder council policy, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 05. martial challenge policy [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns martial challenge policy. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** policy_martial_challenge must not bypass the existing challenge threshold,
grievance and resolution rules.

**Fresh campaign path.** For martial challenge policy, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For martial challenge policy, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For martial challenge policy, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For martial challenge policy, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For martial challenge policy, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For martial challenge policy, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For martial challenge policy, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For martial challenge policy, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 06. designate leader [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns designate leader. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** A valid living candidate becomes CurrentLeaderId once; NeedsSystem receives
only the attributed morale effect.

**Fresh campaign path.** For designate leader, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For designate leader, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For designate leader, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For designate leader, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For designate leader, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For designate leader, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For designate leader, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For designate leader, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 07. leader step-down [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns leader step-down. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** StepDown respects current cooldown and clears leadership state without
inventing an election result.

**Fresh campaign path.** For leader step-down, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For leader step-down, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For leader step-down, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For leader step-down, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For leader step-down, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For leader step-down, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For leader step-down, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For leader step-down, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 08. designated successor [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns designated successor. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** DesignateSuccessor stores one valid ID and rejects leader/dead/unknown
candidates under current Core rules.

**Fresh campaign path.** For designated successor, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For designated successor, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For designated successor, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For designated successor, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For designated successor, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For designated successor, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For designated successor, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For designated successor, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 09. deputy appointment [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns deputy appointment. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** AppointDeputy stores a distinct backup and exposes it in the social read
model after reload.

**Fresh campaign path.** For deputy appointment, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For deputy appointment, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For deputy appointment, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For deputy appointment, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For deputy appointment, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For deputy appointment, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For deputy appointment, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For deputy appointment, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 10. leader death [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns leader death. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** OnSurvivorDied promotes the approved successor or deputy once and cancels
obsolete challenges.

**Fresh campaign path.** For leader death, Start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. The card passes only when the
named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For leader death, Deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For leader death, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For leader death, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For leader death, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For leader death, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For leader death, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For leader death, Replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines the
allowed range. If this card needs randomness, fork from the existing campaign stream and make the event key
stable. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 11. leader incapacity [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns leader incapacity. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** OnSurvivorInjured follows current stress/continuity rules; do not interpret
every injury as removal.

**Fresh campaign path.** For leader incapacity, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For leader incapacity, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For leader incapacity, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For leader incapacity, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For leader incapacity, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For leader incapacity, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For leader incapacity, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For leader incapacity, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 12. open challenge [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns open challenge. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** InitiateChallenge records a stable challenge ID and reason without
duplicating another active grievance.

**Fresh campaign path.** For open challenge, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For open challenge, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For open challenge, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For open challenge, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For open challenge, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For open challenge, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For open challenge, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For open challenge, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 13. challenge victory [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns challenge victory. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** ResolveChallenge(challengeId,true) changes leader once, resolves siblings
and persists a coherent outcome.

**Fresh campaign path.** For challenge victory, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For challenge victory, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For challenge victory, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For challenge victory, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For challenge victory, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For challenge victory, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For challenge victory, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For challenge victory, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 14. challenge loss [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns challenge loss. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** ResolveChallenge(challengeId,false) records loss without changing
CurrentLeaderId.

**Fresh campaign path.** For challenge loss, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For challenge loss, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For challenge loss, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For challenge loss, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For challenge loss, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For challenge loss, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For challenge loss, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For challenge loss, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 15. crisis morale effect [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns crisis morale effect. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** OnCrisisEvent applies the active policy modifier through the coordinator
Needs sink once per accepted event.

**Fresh campaign path.** For crisis morale effect, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For crisis morale effect, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For crisis morale effect, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For crisis morale effect, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For crisis morale effect, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For crisis morale effect, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For crisis morale effect, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For crisis morale effect, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 16. daily stress decay [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns daily stress decay. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** SurvivorSocialCoordinator.Tick calls Leadership.Tick with the current
interval; restore does not replay elapsed decay.

**Fresh campaign path.** For daily stress decay, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For daily stress decay, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For daily stress decay, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For daily stress decay, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For daily stress decay, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For daily stress decay, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For daily stress decay, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For daily stress decay, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 17. policy catalog load [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns policy catalog load. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** Five policy IDs load before state restore; invalid or duplicate IDs fail
validation rather than becoming silent defaults.

**Fresh campaign path.** For policy catalog load, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For policy catalog load, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For policy catalog load, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For policy catalog load, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For policy catalog load, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For policy catalog load, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For policy catalog load, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For policy catalog load, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 18. governance stability consumer [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns governance stability consumer. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** ShelterGovernanceHostSession reads the same LeadershipSystem instance for
approval/stability projections.

**Fresh campaign path.** For governance stability consumer, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For governance stability consumer, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For governance stability consumer, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For governance stability consumer, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For governance stability consumer, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For governance stability consumer, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For governance stability consumer, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For governance stability consumer, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 19. relations panel truth [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns relations panel truth. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** SurvivorRelationsPanel shows leader, successor, deputy, challenge state and
real refusal feedback from the coordinator.

**Fresh campaign path.** For relations panel truth, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For relations panel truth, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For relations panel truth, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For relations panel truth, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For relations panel truth, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For relations panel truth, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For relations panel truth, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For relations panel truth, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 20. legacy social save [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns legacy social save. Start from
`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
`Assets/StreamingAssets/Data/leadership_policies.json` and trace any effect through
`src/Main.ShelterSocial.cs` to its current destination owner. Persistence must follow this boundary:
`survivor_social section contains LeadershipSaveState`. Verify the exact source and destination sections; a
new section requires an ownership decision. Keep the source fact distinct from the consumer mutation.
Catalog evidence: No catalog row directly matches this integration case; verify the current producer and
destination API before implementation.

**Feature-specific acceptance:** An old survivor_social save with no new policy data restores the documented
default and preserves leader identity.

**Fresh campaign path.** For legacy social save, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For legacy social save, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For legacy social save, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For legacy social save, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For legacy social save, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For legacy social save, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For legacy social save, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For legacy social save, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`
if it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register
The original 2026-09-01 task list is preserved as intent here in condensed form. Numbered items that are
already delivered or contradicted by signed authority must be marked DELIVERED or RETIRED. Each entry is a
premise question, never an instruction to create a duplicate class or save section. Current code and the
live ownership ledger decide whether it becomes a claim.
- **L01:** Extend `LeadershipSystem.cs` with succession DTOs. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `SuccessionPlan` DTO: `planId`, `leaderId`, `designatedSuccessor` (survivor_id),
  `backupSuccessor` (survivor_id), `createdDay`, `updatedDay`, `isActive` bool. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `LeadershipChallenge` DTO: `challengeId`, `challengerId` (survivor_id),
  `challengedLeaderId`, `challengeType` (election/coup/contest/recall), `reason` (description), `supporters`
  (list of survivor_ids), `opponents` (list of survivor_ids), `challengeDay`, `resolutionDay` (-1 if
  unresolved), `outcome` (pending/challenger_wins/leader_wins/withdrawn/compromise), `voteResults` (dict of
  survivor_id → vote). Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `LeadershipElection` DTO: `electionId`, `candidates` (list of survivor_ids), `voters`
  (list of eligible survivor_ids), `electionDay`, `voteDeadline` (day), `results` (dict of candidate_id →
  vote_count), `winner` (survivor_id), `turnout` (0-100), `legitimacy` (0-100). Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `LeadershipTransfer` DTO: `transferId`, `fromLeaderId`, `toLeaderId`, `transferType`
  (succession_on_death/step_down/challenge_victory/election_victory/appointment), `transferDay`, `reason`,
  `isVoluntary` bool, `isContested` bool. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Define `DeputyLeader` DTO: `deputyId`, `leaderId`, `deputySurvivorId`, `appointedDay`, `powers`
  (list of delegated authorities), `isActive` bool. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define `LeadershipLegitimacy` DTO: `legitimacyId`, `leaderId`, `legitimacyScore` (0-100),
  `factors` (list of legitimacy modifiers:
  elected/appointed/popular_support/competence/morality/crisis_performance), `lastUpdatedDay`. Verify
  against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Extend `LeadershipState` DTO: add succession plan, active challenges, election history, transfer
  history, deputy leader, legitimacy score. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Implement `CaptureState/RestoreState` extension with schema versioning. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define succession mechanics:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define leadership challenge mechanics:. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Define election mechanics:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Define deputy mechanics:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Define legitimacy mechanics:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Define term limits (optional):. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Add deterministic seeding: leadership events use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Wire into `GameBootstrap`: extend `SetupLeadership`, `TickLeadership`, `SaveLeadership`. Verify
  against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement succession planning:. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement leadership challenges:. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement elections:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement deputy system:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement legitimacy tracking:. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Implement term limits (optional):. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Implement leadership UI:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Create leadership events:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add leadership quest hooks:. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement leadership tutorial: first leadership transition explains system. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add leadership tooltips: hover over leader shows legitimacy, term. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Create leadership rules in data file. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Implement leadership persistence: succession/challenges/elections saved. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Integrate with `SurvivorRelationsSystem`: relationships affect challenge support. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Wire into `LeadershipSystem`: extend existing system. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Connect to `SurvivorRelationsSystem`: relationships affect support. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Integrate with `MoralChoiceSystem`: morality affects legitimacy. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Connect to `SkillProgressionSystem`: leadership skill affects performance. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Wire into `InterpersonalConflictSystem` (Plan 202): challenges can trigger conflicts. Verify
  against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Connect to `DeathLegacySystem` (Plan 206): leader death triggers succession. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Implement old-save compatibility: existing saves get no succession plan, current leader retains
  position. Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Add deterministic seeding: leadership events use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Create exploit prevention: legitimacy is performance-based, can't be gamed. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Add tests: succession, challenges, elections, deputy, legitimacy, save round-trip. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Verify all leadership transitions work correctly. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Test edge cases: no leader (current behavior), frequent challenges (political instability).
  Verify against `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Verify headless behavior: leadership processes correctly without UI. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L45:** Add data-integrity-selftest: leadership validates against survivor catalogs. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L46:** Create `--leadership-succession-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract
**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh
Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs` plus targeted owner save/host
checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
