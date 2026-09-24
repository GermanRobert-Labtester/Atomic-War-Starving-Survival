# Plan 183 — Child Development Stages — Canonical Generational Projection

## Current-evidence architecture decision (2026-09-24; supersedes stale instructions below)

This section is the operative integration architecture for **Child development stages**. It is a plan, not an implementation claim. The baseline material below remains a scenario inventory; when it says a missing file is “new,” requests a separate registry, or assumes a host count, this current-evidence section controls. Current source was inspected on 2026-09-24. Recheck it at the start of a claimed implementation package because concurrent integration work may have moved the seams.

**Verified premise:** GenerationalSystem resolves canonical age/stage through ChildDevelopmentSystem, Main saves its state in child_development, and NurseryPanel binds the generational owner. Plan 183 remains a projection/extension concern, not an independent child ledger.

**Master authority mapping:** C9 survivor lineage, C2 child health, C17 UI. The master expansion document `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` supplies the Part II premise sweep, evidence labels, anti-duplication firewall, and Lane A prose, Lane B mechanics, Lane D save, and Lane E player-surface questions. Its scene and content candidates are ideas, not evidence that a route currently works. The live source and AGENTS.md take precedence.

**Bounded outcome:** Establish one source fact or player command, one canonical consumer, one durable result, and one truthful player readout for the next unsealed gap. The first phase should close a single representative path. A later content batch, cross-system extension, or balancing pass is a separate path claim. No duplicate birth, age, custody, or lifecycle authority.

### Authority and custody map

| Concern | Custody | Required proof before a change |
|---|---|---|
| Domain rule | `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs` | Inspect public API, mutation and capture/restore; amend it only for an observed gap. |
| Producer | `a living child record with canonical birth day and caregiver context` | Show that the event follows a committed action and carries stable IDs/day. |
| Host composition | `src/Main.Plans178_181.cs` | Show setup, restore, bind, dirty/save, reset and lifecycle order in the current tree. |
| Destination effect | `GenerationalSystem birth, care, age, and growth owner` | Call its existing command once; do not mirror its state in this plan. |
| Persistence | `child_development section carrying GenerationalSystem state` | Save both source marker and effect custody; prove retry cannot duplicate. |
| Player readout | `NurseryPanel and canonical child projection` | Display provenance, current status, next command and refusal using existing state. |

### Phase gates

0. **Claim and recensus.** Read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, the current domain owner, the specific host route, save registration and catalog loader. Record the exact files a builder may edit. If a current active claim overlaps, wait for its handoff. If source contradicts this page, amend this page before coding.
1. **Domain fact.** Pick one case from the scenario inventory below. Specify input identity, accepted preconditions, typed result, mutation owner, idempotency key and failure code. A UI callback cannot decide domain legality. If an existing owner already supplies the effect, use it and retire the proposed duplicate.
2. **Catalog and prose.** Inspect the actual JSON row shape and consumer. Add only reachable rows with unique IDs and valid references. Write concise diegetic text for a real state and a separate refusal. A copy field is never a source of numeric gameplay rules. The master’s Lane A archetype may suggest tone, but the current loader sets field limits.
3. **Save and deterministic replay.** Identify the exact save DTO and section. Capture after the authoritative mutation, restore before event rebinding, and replay the same input. Use the campaign RNG fork only where the current owner already consumes it; no wall clock, hash-order sampling or process-local dedupe. Treat missing old fields as the documented baseline and reject malformed new values before they enter live state.
4. **Host composition.** Bind the producer to `GenerationalSystem birth, care, age, and growth owner` in the existing host lifetime. Respect setup order, reset, dirty tracking, save orchestration, and unsubscription. If a consumer is optional, specify the withheld effect and a truthful unavailable reason; do not silently report success. Shared `Main` and registry files belong to the named integrator.
5. **Presentation.** Bind `NurseryPanel and canonical child projection` to a read model and command result. Render stable IDs as authored labels only after validating a lookup. Keep keyboard/controller focus, close/back, refresh and disposal correct; expose reasons in words, not color alone. Do not let opening a panel trigger a milestone, trade, or consequence.
6. **Focused acceptance and handoff.** Select the smallest directly relevant test file under `TEST_POLICY.md` when implementation is authorized. Prove fresh path, repeated input, save/restore boundary, invalid reference, headless host route if touched, and UI projection if touched. Record exact commands and results. This document edit does not run implementation tests.

### Code integration framework: current API anchors and proposed placement

**Verified call anchor:** `GenerationalSystem.GrowthTick` owns age transitions and `GetCanonicalChildProfile` projects stage; Main saves generational state in `child_development`.

The following C# fragment identifies an existing method and its intended position in the current host. It is a placement guide, not a new authority type or a copy-and-paste patch. Variables and refusal types come from the owning method. A builder must open that source file and reconcile the exact signature in Phase 0.

```csharp
EnsureGenerational().GrowthTick(currentDay);
ChildProfile? projected = EnsureGenerational().GetCanonicalChildProfile(
    childId, currentDay);
// Bind NurseryPanel to GenerationalSystem, which owns birth and care.
// No separate ChildDevelopmentSystem state should be saved by this view.
```

**Source contract.** Accept a fact only after the owning command commits. Carry its stable ID, subject, campaign day and authored row ID through the host boundary. Distinguish a query from a mutation: `Evaluate` may mutate counters in some current Core systems, so call sites must be inspected instead of assuming the method name is pure. A panel asks for a read model; a player command asks the existing host session to validate and commit. Treat a missing subject or catalog row as a refusal before changing any destination owner.

**Destination contract.** The destination owner alone changes GenerationalSystem birth, care, age, and growth owner. The host carries a typed fact or uses the owner’s established delegate; it does not copy the destination value into another mutable store. The implementation review must record method name, input ID and before/after destination state. If the destination lacks a safe command, record an authority decision rather than writing its fields directly. A projected outcome is useful only if it can be traced back to the original committed fact.

**Save and retry contract.** Capture child_development section carrying GenerationalSystem state after mutation and before reporting a durable result. Where two owners persist separately, the handoff must name save order and recovery for a crash between writes. A repeated fact ID after restore must be a no-op or resume one pending effect, depending on the current owner’s marker semantics. No process-local boolean can stand in for persisted applied identity. The old-save baseline should be documented with one fixture and a stable outcome; corruption should fail without partially mutating an unrelated section.

**Host lifecycle contract.** Reconstruct Core state first, then attach the producer subscription once, then expose a player surface. Reset detaches handlers and clears only transient references. Dirty tracking must follow actual state change; calling a query must not mark a section dirty unless the inspected Core method genuinely updates counters. The shared `Main` and save registry remain integrator-owned paths. The review note should include setup, save, reset and direct/indirect caller locations.

**Focused implementation specimen.** Arrange a valid source ID and a real catalog row; invoke the command or event through the current host; assert destination state and readout; capture and restore into a new session; replay the same ID; assert there is still one effect. In a second fixture, substitute the invalid reference described in the relevant casebook and assert a specific refusal with no resource, standing, stage, or wallet change. This is the minimum evidence for one vertical slice, not a request to generate a separate test for every card.

**Content handoff.** Author a short observation, a command label, a refusal, and a consequence line only in fields actually consumed by the current UI or event renderer. Use existing localization and accessibility conventions. A passage must not announce an unlock, treatment, route, or economic result before its owner confirms the state. The narrative writer receives stable IDs and the exact before/after facts, so a line of prose can be reviewed against an implementation trace.

### Integration architecture closeout criteria

The *planning* architecture is finished when each proposed effect names its producer, rule owner, destination owner, host attachment, save carrier, player readout, duplicate guard, negative path, and focused proof. Runtime integration is finished only when those paths are implemented and the relevant focused checks pass. This distinction applies to every scenario below. The handoff must name any decision gate and must not upgrade a proposal to “shipped” because a Core class or catalog row exists.

## Earlier scenario inventory and intent (subject to the current-evidence architecture above)

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
  RegisterChild/TickDay/CaptureState; the current Main host uses GenerationalSystem for canonical child state.
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

## Integration casebooks: producer, custody, presentation, and failure

These casebooks turn the earlier scenario names into reviewable implementation questions. They are acceptance design, not claims that every feature already exists or that every case needs one test method. Select one bounded case per implementation package and record evidence before promoting it.

### 001. infant stage boundary — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the infant stage boundary result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 002. toddler stage boundary — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the toddler stage boundary result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 003. child stage boundary — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the child stage boundary result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 004. adolescent stage boundary — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the adolescent stage boundary result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 005. young adult boundary — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the young adult boundary result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 006. birth day source — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the birth day source result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 007. canonical child identity — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the canonical child identity result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 008. caregiver assignment — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the caregiver assignment result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 009. education progress — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the education progress result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 010. teacher assignment — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the teacher assignment result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 011. child ration and care — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the child ration and care result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 012. trait registration — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the trait registration result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 013. malnourished-growth trait — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the malnourished-growth trait result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 014. hypervigilant trait — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the hypervigilant trait result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 015. milestone event identity — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the milestone event identity result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 016. missed-day catch-up — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the missed-day catch-up result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 017. clock rewind — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the clock rewind result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 018. adulthood transfer — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the adulthood transfer result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 019. nursery panel lifecycle — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the nursery panel lifecycle result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 020. legacy child save — Producer and temporal boundary

Write the time line from a living child record with canonical birth day and caregiver context to the legacy child save result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 021. infant stage boundary — Rule and destination handoff

For infant stage boundary, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 022. toddler stage boundary — Rule and destination handoff

For toddler stage boundary, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 023. child stage boundary — Rule and destination handoff

For child stage boundary, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 024. adolescent stage boundary — Rule and destination handoff

For adolescent stage boundary, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 025. young adult boundary — Rule and destination handoff

For young adult boundary, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 026. birth day source — Rule and destination handoff

For birth day source, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 027. canonical child identity — Rule and destination handoff

For canonical child identity, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 028. caregiver assignment — Rule and destination handoff

For caregiver assignment, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 029. education progress — Rule and destination handoff

For education progress, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 030. teacher assignment — Rule and destination handoff

For teacher assignment, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 031. child ration and care — Rule and destination handoff

For child ration and care, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 032. trait registration — Rule and destination handoff

For trait registration, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 033. malnourished-growth trait — Rule and destination handoff

For malnourished-growth trait, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 034. hypervigilant trait — Rule and destination handoff

For hypervigilant trait, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 035. milestone event identity — Rule and destination handoff

For milestone event identity, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 036. missed-day catch-up — Rule and destination handoff

For missed-day catch-up, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 037. clock rewind — Rule and destination handoff

For clock rewind, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 038. adulthood transfer — Rule and destination handoff

For adulthood transfer, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 039. nursery panel lifecycle — Rule and destination handoff

For nursery panel lifecycle, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 040. legacy child save — Rule and destination handoff

For legacy child save, let Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs and GenerationalSystem.cs decide the rule and send only the completed fact to GenerationalSystem birth, care, age, and growth owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 041. infant stage boundary — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the infant stage boundary command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 042. toddler stage boundary — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the toddler stage boundary command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 043. child stage boundary — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the child stage boundary command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 044. adolescent stage boundary — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the adolescent stage boundary command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 045. young adult boundary — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the young adult boundary command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 046. birth day source — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the birth day source command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 047. canonical child identity — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the canonical child identity command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 048. caregiver assignment — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the caregiver assignment command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 049. education progress — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the education progress command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 050. teacher assignment — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the teacher assignment command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 051. child ration and care — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the child ration and care command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 052. trait registration — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the trait registration command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 053. malnourished-growth trait — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the malnourished-growth trait command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 054. hypervigilant trait — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the hypervigilant trait command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 055. milestone event identity — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the milestone event identity command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 056. missed-day catch-up — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the missed-day catch-up command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 057. clock rewind — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the clock rewind command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 058. adulthood transfer — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the adulthood transfer command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 059. nursery panel lifecycle — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the nursery panel lifecycle command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 060. legacy child save — Persistence and replay

Capture child_development section carrying GenerationalSystem state immediately before the legacy child save command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 061. infant stage boundary — Catalog and prose contract

Inspect the current authored row relevant to infant stage boundary and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 062. toddler stage boundary — Catalog and prose contract

Inspect the current authored row relevant to toddler stage boundary and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 063. child stage boundary — Catalog and prose contract

Inspect the current authored row relevant to child stage boundary and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 064. adolescent stage boundary — Catalog and prose contract

Inspect the current authored row relevant to adolescent stage boundary and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 065. young adult boundary — Catalog and prose contract

Inspect the current authored row relevant to young adult boundary and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 066. birth day source — Catalog and prose contract

Inspect the current authored row relevant to birth day source and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 067. canonical child identity — Catalog and prose contract

Inspect the current authored row relevant to canonical child identity and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 068. caregiver assignment — Catalog and prose contract

Inspect the current authored row relevant to caregiver assignment and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 069. education progress — Catalog and prose contract

Inspect the current authored row relevant to education progress and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 070. teacher assignment — Catalog and prose contract

Inspect the current authored row relevant to teacher assignment and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 071. child ration and care — Catalog and prose contract

Inspect the current authored row relevant to child ration and care and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 072. trait registration — Catalog and prose contract

Inspect the current authored row relevant to trait registration and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 073. malnourished-growth trait — Catalog and prose contract

Inspect the current authored row relevant to malnourished-growth trait and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 074. hypervigilant trait — Catalog and prose contract

Inspect the current authored row relevant to hypervigilant trait and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 075. milestone event identity — Catalog and prose contract

Inspect the current authored row relevant to milestone event identity and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 076. missed-day catch-up — Catalog and prose contract

Inspect the current authored row relevant to missed-day catch-up and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 077. clock rewind — Catalog and prose contract

Inspect the current authored row relevant to clock rewind and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 078. adulthood transfer — Catalog and prose contract

Inspect the current authored row relevant to adulthood transfer and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 079. nursery panel lifecycle — Catalog and prose contract

Inspect the current authored row relevant to nursery panel lifecycle and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 080. legacy child save — Catalog and prose contract

Inspect the current authored row relevant to legacy child save and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 081. infant stage boundary — Player route and accessibility

Present infant stage boundary through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 082. toddler stage boundary — Player route and accessibility

Present toddler stage boundary through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 083. child stage boundary — Player route and accessibility

Present child stage boundary through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 084. adolescent stage boundary — Player route and accessibility

Present adolescent stage boundary through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 085. young adult boundary — Player route and accessibility

Present young adult boundary through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 086. birth day source — Player route and accessibility

Present birth day source through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 087. canonical child identity — Player route and accessibility

Present canonical child identity through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 088. caregiver assignment — Player route and accessibility

Present caregiver assignment through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 089. education progress — Player route and accessibility

Present education progress through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 090. teacher assignment — Player route and accessibility

Present teacher assignment through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 091. child ration and care — Player route and accessibility

Present child ration and care through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 092. trait registration — Player route and accessibility

Present trait registration through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 093. malnourished-growth trait — Player route and accessibility

Present malnourished-growth trait through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 094. hypervigilant trait — Player route and accessibility

Present hypervigilant trait through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 095. milestone event identity — Player route and accessibility

Present milestone event identity through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 096. missed-day catch-up — Player route and accessibility

Present missed-day catch-up through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 097. clock rewind — Player route and accessibility

Present clock rewind through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 098. adulthood transfer — Player route and accessibility

Present adulthood transfer through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 099. nursery panel lifecycle — Player route and accessibility

Present nursery panel lifecycle through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 100. legacy child save — Player route and accessibility

Present legacy child save through NurseryPanel and canonical child projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 101. infant stage boundary — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for infant stage boundary. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 102. toddler stage boundary — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for toddler stage boundary. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 103. child stage boundary — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for child stage boundary. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 104. adolescent stage boundary — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for adolescent stage boundary. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 105. young adult boundary — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for young adult boundary. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 106. birth day source — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for birth day source. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 107. canonical child identity — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for canonical child identity. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 108. caregiver assignment — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for caregiver assignment. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 109. education progress — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for education progress. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 110. teacher assignment — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for teacher assignment. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 111. child ration and care — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for child ration and care. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 112. trait registration — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for trait registration. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 113. malnourished-growth trait — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for malnourished-growth trait. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 114. hypervigilant trait — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for hypervigilant trait. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 115. milestone event identity — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for milestone event identity. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 116. missed-day catch-up — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for missed-day catch-up. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 117. clock rewind — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for clock rewind. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 118. adulthood transfer — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for adulthood transfer. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 119. nursery panel lifecycle — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for nursery panel lifecycle. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 120. legacy child save — Failure containment

Use a child crossing a stage boundary twice after restore or a panel showing independent age as the negative fixture for legacy child save. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 121. infant stage boundary — Adjacent-owner collision

Trace each effect claimed by infant stage boundary through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 122. toddler stage boundary — Adjacent-owner collision

Trace each effect claimed by toddler stage boundary through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 123. child stage boundary — Adjacent-owner collision

Trace each effect claimed by child stage boundary through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 124. adolescent stage boundary — Adjacent-owner collision

Trace each effect claimed by adolescent stage boundary through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 125. young adult boundary — Adjacent-owner collision

Trace each effect claimed by young adult boundary through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 126. birth day source — Adjacent-owner collision

Trace each effect claimed by birth day source through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 127. canonical child identity — Adjacent-owner collision

Trace each effect claimed by canonical child identity through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 128. caregiver assignment — Adjacent-owner collision

Trace each effect claimed by caregiver assignment through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 129. education progress — Adjacent-owner collision

Trace each effect claimed by education progress through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 130. teacher assignment — Adjacent-owner collision

Trace each effect claimed by teacher assignment through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 131. child ration and care — Adjacent-owner collision

Trace each effect claimed by child ration and care through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 132. trait registration — Adjacent-owner collision

Trace each effect claimed by trait registration through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 133. malnourished-growth trait — Adjacent-owner collision

Trace each effect claimed by malnourished-growth trait through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 134. hypervigilant trait — Adjacent-owner collision

Trace each effect claimed by hypervigilant trait through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 135. milestone event identity — Adjacent-owner collision

Trace each effect claimed by milestone event identity through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 136. missed-day catch-up — Adjacent-owner collision

Trace each effect claimed by missed-day catch-up through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 137. clock rewind — Adjacent-owner collision

Trace each effect claimed by clock rewind through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 138. adulthood transfer — Adjacent-owner collision

Trace each effect claimed by adulthood transfer through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 139. nursery panel lifecycle — Adjacent-owner collision

Trace each effect claimed by nursery panel lifecycle through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 140. legacy child save — Adjacent-owner collision

Trace each effect claimed by legacy child save through the current map of owners. Ask whether the same condition already reaches GenerationalSystem birth, care, age, and growth owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 141. infant stage boundary — Day order and deterministic boundary

Pin infant stage boundary to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 142. toddler stage boundary — Day order and deterministic boundary

Pin toddler stage boundary to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 143. child stage boundary — Day order and deterministic boundary

Pin child stage boundary to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 144. adolescent stage boundary — Day order and deterministic boundary

Pin adolescent stage boundary to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 145. young adult boundary — Day order and deterministic boundary

Pin young adult boundary to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 146. birth day source — Day order and deterministic boundary

Pin birth day source to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 147. canonical child identity — Day order and deterministic boundary

Pin canonical child identity to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 148. caregiver assignment — Day order and deterministic boundary

Pin caregiver assignment to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 149. education progress — Day order and deterministic boundary

Pin education progress to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 150. teacher assignment — Day order and deterministic boundary

Pin teacher assignment to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 151. child ration and care — Day order and deterministic boundary

Pin child ration and care to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 152. trait registration — Day order and deterministic boundary

Pin trait registration to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 153. malnourished-growth trait — Day order and deterministic boundary

Pin malnourished-growth trait to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.


## Polishing pass and architecture handoff

This revision received a second editorial pass after the architecture and casebooks were assembled. The pass normalizes headings and whitespace, treats the current evidence section as authoritative over older speculative instructions, preserves the older scenario inventory as conditional intent, removes the most consequential false present-tense claims, and checks that every implementation phase has an owner, a save rule, a player route, a failure path, and a focused proof. Casebook prose is deliberately phrased as review work where a consumer or route has not been verified. The live-source recensus remains mandatory before implementation, especially where another active claim is changing a host.

**Closeout:** Plan 183 now has a documented integration architecture and a bounded first-slice method. No production behavior has been changed by this document. Runtime completion requires the claimed implementation package, focused verification, and the handoff described above.
