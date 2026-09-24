# Plan 183 — Child Development Stages — Canonical Generational Projection
> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome
Seal truthful stage, education, care and milestone presentation from the canonical GenerationalSystem, and
route any remaining player action through its existing commands and save section.

**Current state:** MOSTLY LIVE: GenerationalSystem and NurseryPanel are hosted, child_development is saved,
and Main.GetCanonicalChildDevelopment exposes a detached Plan 183 projection. ChildDevelopmentSystem also
has standalone mutable RegisterChild/TickDay APIs, but no host instance was found; do not create a second
age clock.

**Non-goals:** No second child roster, birth clock, milestone history, education XP ledger, child labor
rules, or independent ChildDevelopmentSystem save section.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status
The current or candidate domain authority is `GenerationalSystem owns birth, growth, education,
caregiver/teacher links and adulthood; ChildDevelopmentSystem provides a detached stage projection`. Source
and adjacent paths inspected for this revision (some are candidate consumers rather than active bindings):
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs`, `src/Main.Plans178_181.cs`,
`src/Main.CampaignOwners.cs`, `src/Host/GenerationalSaveStore.cs`, `src/UI/NurseryPanel.cs`, and
`Assets/StreamingAssets/Data/development_traits.json`. The focused test starting point is
`Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs`. Persistence boundary under audit:
`child_development section carries GenerationalState; no second child-stage section`. Paths are evidence
pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-9/C9 child
cohorts, care, lineage and apprenticeship plus DM-2 medical school eligibility; the master flags
ChildDevelopmentSystem as a possible overlapping carrier requiring a premise sweep. Its Part II requires
live premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The
relevant deep maps and lane matrices guide coverage; they do not override newer code. The master compilation
itself warns against padding and stale repository assumptions. This plan therefore records concrete
contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall
- **Evidence or explicit premise:** Main.EnsureGenerational loads development_traits.json, restores
  GenerationalSaveStore and binds canonical stage/adulthood journal events.
- **Evidence or explicit premise:** GenerationalSystem.GetCanonicalChildProfile uses
  ChildDevelopmentSystem.ProjectCanonicalChild, with birth day and adulthood flag from the saved
  generational record.
- **Evidence or explicit premise:** SaveSectionRegistry registers child_development and
  child_development_save.json; it stores GenerationalState, not ChildDevelopmentState.
- **Evidence or explicit premise:** NurseryPanel already binds GenerationalSystem and presents child count,
  education and actions.
- **Evidence or explicit premise:** ChildDevelopmentSystem exposes independent
  RegisterChild/TickDay/CaptureState, but no hosted instance was found.
- **Evidence or explicit premise:** Canonical age thresholds in ChildDevelopmentSystem.ResolveStage are 60,
  180, 500 and 720 days; must account for the GenerationalSystem adult flag and stage projection.

### Child-state custody dossier

`GenerationalSystem` is the saved child authority. Its `ChildDevelopment` records hold birth day,
phase/progress, growth tick, care and adulthood information. The differently named `ChildDevelopmentSystem`
has a standalone mutable model but is used in production as a static projection adapter; hosting its
`RegisterChild` and `TickDay` would create two independently advancing children. The plan must treat its
mutable methods as unclaimed, and any new field should first be considered in `GenerationalState` with a
migration rule.

A stage label is not a second clock. Use `GetCanonicalChildProfile(childId, day)` to read a detached view.
The host already journals `OnCanonicalStageAdvanced`; prove day-keyed once-only behavior before changing the
wording or adding effects. Milestones that unlock a real command must be enforced by the destination owner:
school eligibility by the cohort/education owner, work eligibility by duty roster, adult transfer by
survivor/skill owner. Do not infer work permission from an age label.

The first residual slice is one child across a stage threshold, saved before and after the tick, with a
NurseryPanel readout and a single journal milestone. Caregiver and education buttons are later small slices
if current command routes have a verified gap.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/development_traits.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.Plans178_181.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `child_development section carries GenerationalState; no second child-stage section` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/NurseryPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Existing read path: the host returns a detached view of canonical state.
ChildProfile? view = main.GetCanonicalChildDevelopment(childId, day);
// Nursery actions call GenerationalSystem commands through Main; the panel
// never calls ChildDevelopmentSystem.RegisterChild or TickDay.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration
Inspect `src/Host/GenerationalSaveStore.cs` and the registry for the named concern; absence of a section is
a decision gate, not permission to invent one. Write down exact DTO version, constructor baseline, capture
point, restore point, and dirty flag. For a stateless bridge, persist only the source and destination
authorities; do not create a bridge section. If a new mutable field is genuinely required, version the
existing owner DTO and prove migration from the preceding schema. Never equate a panel cache with campaign
state.
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
Inspect the current route to `src/UI/NurseryPanel.cs` and its bind/open/close methods. Expose only commands
backed by an existing Core method or a specifically planned method in the named owner. The panel should show
source state, currently legal action, expected cost, blocker, committed outcome, and uncertainty where the
game cannot know more. Refresh on authoritative state change and after restore; unbind subscriptions on
close/disposal. Preserve keyboard/controller back and readable contrast.

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
| `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/development_traits.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.Plans178_181.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.CampaignOwners.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/GenerationalSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/NurseryPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback
Use `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` for
the first contract check, then only the directly affected save and host targets identified by Phase 0. Run a
Godot headless probe only if the claimed change affects the runtime host path. A compile result cannot
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

### 01. infant stage boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns infant stage boundary. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** At age 0 and 59 days the detached view is Infant; the canonical birth day
remains unchanged.

**Fresh campaign path.** For infant stage boundary, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For infant stage boundary, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For infant stage boundary, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For infant stage boundary, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For infant stage boundary, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For infant stage boundary, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For infant stage boundary, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For infant stage boundary, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 02. toddler stage boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns toddler stage boundary. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** At age 60 the projected stage becomes Toddler once and uses
GenerationalSystem stage notification for journal presentation.

**Fresh campaign path.** For toddler stage boundary, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For toddler stage boundary, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For toddler stage boundary, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For toddler stage boundary, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For toddler stage boundary, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For toddler stage boundary, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For toddler stage boundary, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For toddler stage boundary, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 03. child stage boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns child stage boundary. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** At age 180 the projection becomes Child without adding a second stage field
to a save.

**Fresh campaign path.** For child stage boundary, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For child stage boundary, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For child stage boundary, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For child stage boundary, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For child stage boundary, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For child stage boundary, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For child stage boundary, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For child stage boundary, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 04. adolescent stage boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns adolescent stage boundary. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** At age 500 the projection becomes Adolescent subject to canonical adulthood
state.

**Fresh campaign path.** For adolescent stage boundary, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For adolescent stage boundary, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For adolescent stage boundary, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For adolescent stage boundary, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For adolescent stage boundary, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For adolescent stage boundary, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For adolescent stage boundary, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For adolescent stage boundary, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 05. young adult boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns young adult boundary. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** At age 720 the projection and GenerationalSystem adulthood flag must agree
after tick and reload.

**Fresh campaign path.** For young adult boundary, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For young adult boundary, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For young adult boundary, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For young adult boundary, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For young adult boundary, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For young adult boundary, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For young adult boundary, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For young adult boundary, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 06. birth day source [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns birth day source. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** ResolveCanonicalAgeDays reads birthDay and current campaign day; the panel
cannot infer age from visual stage text.

**Fresh campaign path.** For birth day source, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For birth day source, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For birth day source, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For birth day source, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For birth day source, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For birth day source, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For birth day source, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For birth day source, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 07. canonical child identity [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns canonical child identity. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** The same survivorId keys roster, generational record and detached
ChildProfile; an unknown ID returns null.

**Fresh campaign path.** For canonical child identity, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For canonical child identity, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For canonical child identity, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For canonical child identity, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For canonical child identity, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For canonical child identity, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For canonical child identity, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For canonical child identity, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 08. caregiver assignment [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns caregiver assignment. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** An action uses a GenerationalSystem care command and shows the assigned
caregiver after save/reload.

**Fresh campaign path.** For caregiver assignment, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For caregiver assignment, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For caregiver assignment, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For caregiver assignment, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For caregiver assignment, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For caregiver assignment, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For caregiver assignment, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For caregiver assignment, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 09. education progress [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns education progress. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** School progress reads the existing generational education value;
RecordEducation on the standalone projection stays unused.

**Fresh campaign path.** For education progress, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For education progress, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For education progress, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For education progress, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For education progress, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For education progress, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For education progress, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For education progress, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 10. teacher assignment [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns teacher assignment. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** The Nursery route validates a living adult and education eligibility before
changing the canonical teacher relationship.

**Fresh campaign path.** For teacher assignment, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For teacher assignment, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For teacher assignment, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For teacher assignment, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For teacher assignment, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For teacher assignment, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For teacher assignment, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For teacher assignment, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 11. child ration and care [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns child ration and care. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** Needs and ration owners supply actual care effects; Plan 183 view may show
them without a second counter.

**Fresh campaign path.** For child ration and care, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For child ration and care, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For child ration and care, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For child ration and care, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For child ration and care, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For child ration and care, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For child ration and care, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For child ration and care, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 12. trait registration [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns trait registration. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** All seven development_traits rows validate and register once before
restoring child state.

**Fresh campaign path.** For trait registration, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For trait registration, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For trait registration, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For trait registration, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For trait registration, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For trait registration, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For trait registration, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For trait registration, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 13. malnourished-growth trait [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns malnourished-growth trait. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** The authored trait may affect canonical growth only through
GenerationalSystem; the panel reports observed effect.

**Fresh campaign path.** For malnourished-growth trait, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For malnourished-growth trait, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For malnourished-growth trait, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For malnourished-growth trait, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For malnourished-growth trait, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For malnourished-growth trait, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For malnourished-growth trait, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For malnourished-growth trait, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 14. hypervigilant trait [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns hypervigilant trait. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** A psychological-looking child trait must not create a parallel
mental-health diagnosis.

**Fresh campaign path.** For hypervigilant trait, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For hypervigilant trait, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For hypervigilant trait, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For hypervigilant trait, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For hypervigilant trait, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For hypervigilant trait, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For hypervigilant trait, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For hypervigilant trait, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 15. milestone event identity [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns milestone event identity. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** OnCanonicalStageAdvanced presents one journal fact per actual stage
transition, including after restore.

**Fresh campaign path.** For milestone event identity, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For milestone event identity, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For milestone event identity, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For milestone event identity, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For milestone event identity, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For milestone event identity, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For milestone event identity, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For milestone event identity, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 16. missed-day catch-up [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns missed-day catch-up. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** A multi-day jump computes canonical age and transitions without duplicate
milestones or RNG drift.

**Fresh campaign path.** For missed-day catch-up, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For missed-day catch-up, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For missed-day catch-up, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For missed-day catch-up, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For missed-day catch-up, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For missed-day catch-up, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For missed-day catch-up, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For missed-day catch-up, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 17. clock rewind [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns clock rewind. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** A lower day cannot regress canonical growth or create negative age; the
detached projection remains truthful.

**Fresh campaign path.** For clock rewind, Start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. The card passes only when the
named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For clock rewind, Deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For clock rewind, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For clock rewind, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For clock rewind, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For clock rewind, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For clock rewind, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For clock rewind, Replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines the
allowed range. If this card needs randomness, fork from the existing campaign stream and make the event key
stable. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 18. adulthood transfer [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns adulthood transfer. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** OnAdulthoodReached fires once and adult roster/skill owners receive the
approved transition.

**Fresh campaign path.** For adulthood transfer, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For adulthood transfer, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For adulthood transfer, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For adulthood transfer, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For adulthood transfer, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For adulthood transfer, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For adulthood transfer, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For adulthood transfer, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 19. nursery panel lifecycle [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns nursery panel lifecycle. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** Open, refresh, close and reopen preserve focus/feedback while reading the
same GenerationalSystem instance.

**Fresh campaign path.** For nursery panel lifecycle, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For nursery panel lifecycle, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For nursery panel lifecycle, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For nursery panel lifecycle, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For nursery panel lifecycle, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For nursery panel lifecycle, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For nursery panel lifecycle, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For nursery panel lifecycle, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 20. legacy child save [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns legacy child save. Start from
`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and
`Assets/StreamingAssets/Data/development_traits.json` and trace any effect through
`src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this boundary:
`child_development section carries GenerationalState; no second child-stage section`. Verify the exact
source and destination sections; a new section requires an ownership decision. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify
the current producer and destination API before implementation.

**Feature-specific acceptance:** An old child_development save restores with existing birth/care data and
produces the same projected stage.

**Fresh campaign path.** For legacy child save, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For legacy child save, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For legacy child save, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For legacy child save, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For legacy child save, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For legacy child save, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For legacy child save, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For legacy child save, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register
The original 2026-09-01 task list is preserved as intent here in condensed form. Numbered items that are
already delivered or contradicted by signed authority must be marked DELIVERED or RETIRED. Each entry is a
premise question, never an instruction to create a duplicate class or save section. Current code and the
live ownership ledger decide whether it becomes a claim.
- **L01:** Create `ChildDevelopmentSystem.cs` in `Assets/Ashfall.Core/Cohort/`. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `DevelopmentStage` DTO: `stageId`, `stageName`
  (infant/toddler/child/adolescent/young_adult), `ageRange` (min-max), `capabilities` (list of what they
  can/cannot do), `needs` (special needs for stage), `learningRate` (skill learning multiplier),
  `socialBehavior` (description), `events` (list of stage-specific events). Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `ChildState` DTO: `childId`, `currentStage`, `age` (days), `parentIds` (list),
  `caregiverId` (assigned caregiver), `developmentProgress` (0-100 within stage), `learnedSkills` (list),
  `personality` (emerging traits), `health` (stage-specific health modifiers). Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `DevelopmentEvent` DTO: `eventId`, `childId`, `eventType`
  (first_steps/first_words/reading/walking_talking/adolescent_rebellion/coming_of_age), `day`,
  `description`, `effects` (list). Verify against `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `ChildDevelopmentState` DTO: list of child states, list of development events, stage
  transition log, developmental milestones achieved. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define 5 developmental stages:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define stage capabilities:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define stage-specific needs:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define learning progression:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define developmental events:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Add deterministic seeding: development uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Wire into `GameBootstrap`: `SetupChildDevelopment`, `TickChildDevelopment`,
  `SaveChildDevelopment`. Verify against `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Create `DevelopmentStageCatalogLoader` for stage definitions. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement child development UI: child detail panel showing stage, progress, needs. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement stage progression:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement infant care:. Verify against `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement toddler development:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement child education:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement adolescent phase:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement young adult transition:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement caregiver assignment:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Implement developmental consequences:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Create development events:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Add development quest hooks:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Implement development UI:. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Add development journal: automatic log of development events. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Implement development tutorial: first child birth explains system. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Add development tooltips: hover over stage shows capabilities. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Create 5 stage definitions in data file. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Wire into `CohortSystem`: replaces boolean maturation with stage progression. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to `SurvivorLifecycle`: young adult transitions to adult. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Integrate with `NeedsSystem`: child-specific needs. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Connect to `SkillProgressionSystem`: child skill learning. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Wire into `EducationSystem` (Plan 154): education integration. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Connect to `AgingSystem` (Plan 176): aging integration. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Implement old-save compatibility: existing children get estimated ages/stages. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Add deterministic seeding: development uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Create exploit prevention: development is time-based, can't be rushed. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Add tests: stage progression, care effects, education, transition, save round-trip. Verify
  against `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Verify all stages progress correctly. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Test edge cases: no care (developmental delay), excellent care (accelerated). Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Verify headless behavior: development processes correctly without UI. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Add data-integrity-selftest: stages validate against age/skill catalogs. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L45:** Create `--child-development-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` and the destination owner; disposition must be
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
Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs` plus targeted owner save/host
checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
