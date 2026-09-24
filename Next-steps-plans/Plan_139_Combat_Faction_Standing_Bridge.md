# Plan 139 — Combat → Faction Standing Bridge

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Finish one bounded, politically legible aftermath path from a resolved tagged encounter to a visible
standing/incident record and one existing reaction consumer, preserving the exactly-once combat marker.

**Current state:** PARTIAL: bridge, combat save markers, host completion hook, and canonical standing
mutation already exist; reaction, raid, quest, and player-facing incident evidence require source-level
proof.

**Non-goals:** No second standing ledger, no witness RNG or cooldown store without an authored source, no
simultaneous writes to FactionStanceEngine, no invented patrol context, no new combat resolver.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status

The implementation authority is `CombatFactionStandingBridge projecting CombatState into Year of Ash
FactionWar standing`. Verified entry points inspected for this revision:
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs`, `src/Host/CombatHostSession.cs`,
`src/Main.Expeditions.cs`, `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`,
`src/UI/FactionsPanel.cs`, and `Assets/StreamingAssets/Data/faction_combat_thresholds.json`. The focused
test starting point is `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs`.
Existing save ownership is `combat`. Paths are evidence pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: C7 factions and
war, C5 expedition combat, C15 defense, C10 quests, and C17 presentation; the master authority calls for
a single standing owner and live consumer evidence. Its Part II requires live premise checks, bounded
subject scope, explicit evidence labels, and a duplication firewall. The relevant deep maps and lane
matrices guide coverage; they do not override newer code. The master compilation itself warns against
padding and stale repository assumptions. This plan therefore records concrete contracts and treats older
task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Verified or directly observed:** EvaluateConsequences sorts incidents and uses faction-tagged
  CombatState combatants.
- **Verified or directly observed:** ApplyConsequences uses CombatState.AppliedFactionConsequenceIds;
  preserve this marker through combat capture/restore.
- **Verified or directly observed:** ConfigureFactionStanding retries resolved restored encounters;
  verify once-only mutation after a save between resolution and sink binding.
- **Verified or directly observed:** Friendly fire, self-defense, assisted victory, downed, and lethal
  casualties have distinct current formulas.
- **Verified or directly observed:** The faction_combat_thresholds catalog exists, but its consumption in
  EvaluateConsequences must be verified before any balance claim.
- **Verified or directly observed:** FactionConsequenceApplied is a presentation fact after standing
  mutation; callbacks must not mutate standing again.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public
methods and save snapshots, and write a one-page premise note. If another live owner already mutates the
target concern, use it. If a required write API does not exist, return the proposed consumer hook to the
foreman as an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix


| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/faction_combat_thresholds.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Host/CombatHostSession.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `combat` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/FactionsPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
| cross-system effect | `destination subsystem named in each phase` | Call the owner method once; do not copy its mutable state. |

## 5. Data and identity contract

Canonical IDs come from the existing catalogs and runtime facts. Case handling, trimming and comparison
must match the owning system. Proposed new IDs need a schema and reference validator before a producer
can emit them. Read the current JSON fields before extending a row; do not use the old plan’s desired row
count as an acceptance measure. Reject duplicates or conflicting definitions with per-row diagnostics.
Treat display names and prose as presentation, never identity. Preserve ordinal ordering for stable
output and save checksum inputs.
A change to catalog shape requires an old-catalog compatibility rule, one example valid row, one invalid
row, and a reader inventory. New prose must state a real observed consequence; it cannot promise trade,
safety, quest or reward behavior until a consumer reads the corresponding Core fact. Avoid real-world
names or copied narrative.

## 6. C# implementation sketch


```csharp
// Existing seam (signature observed in CombatHostSession).
combat.ConfigureFactionStanding(yearOfAsh.FactionWar.ModifyStanding);
combat.FactionConsequenceApplied += consequence =>
{
    // Host marks its existing save owner dirty and presents the applied fact.
    // Never call ModifyStanding a second time from this callback.
};
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent
must confirm names and signatures against the live tree immediately before coding. Keep
`Assets/Ashfall.Core/` free of Godot and Unity references. If an adapter needs a callback, bind it in the
current host lifetime, unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` and the registry entry for `combat`. Write
down exact DTO version, constructor baseline, capture point, restore point, and dirty flag. For a
stateless bridge, persist only the source and destination authorities; do not create a bridge section. If
a new mutable field is genuinely required, version the existing owner DTO and prove migration from the
preceding schema. Never equate a panel cache with campaign state.
The restore sequence is: catalog validated; source state restored; destination state restored; adapters
bound; pending effects reconciled once; UI bound; day processing resumes. A new campaign starts with an
explicit empty/default state. A legacy save without the field takes that same baseline without inventing
past events. Unknown future schema versions must fail or degrade according to the existing save contract,
with a visible diagnostic. Corrupt single records must not silently reset an entire unrelated section.

## 8. Event, day and failure semantics

One semantic fact should carry stable source ID, campaign day, owning entity ID, content ID if any, and
enough context for the destination owner to decide. The host may route the fact but may not calculate a
competing gameplay rule. Define when the fact is emitted, when a command is committed, and what is
durable before the next callback. A read-only query must not create journal entries or rewards on every
panel refresh.
Use the canonical day owner only for behavior that genuinely changes with time. Preserve the current day
phase ordering and forked seeded RNG contract. No wall-clock seed, hash-order iteration, or System.Random
belongs in deterministic Core. Replay after load must not repeat a completed effect; an effect that was
not committed must remain recoverable. When a destination owner rejects a command, keep the source fact
and show a reason rather than writing a partial substitute effect.

## 9. Player commands and UI

Inspect the current route to `src/UI/FactionsPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and
uncertainty where the game cannot know more. Refresh on authoritative state change and after restore;
unbind subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 9A. First integration slice: a resolved encounter with visible political aftermath


**Premise proved in source:** `CombatHostSession` owns a `CombatFactionStandingBridge`, calls
`EvaluateConsequences` when `OnEncounterEnded` fires, and persists
`CombatState.AppliedFactionConsequenceIds` with the combat encounter. `Main.Expeditions.SetupCombat`
binds `ConfigureFactionStanding(_yearOfAsh.FactionWar.ModifyStanding)` and marks Year of Ash dirty on
`FactionConsequenceApplied`. The faction threshold catalog exists, but repository search found no host
call to `CombatFactionStandingBridge.LoadCatalog`; its thresholds and faction rules must be treated as
authored but unconsumed until a real call is added.

**First slice:** Keep one resolved encounter with a single faction-tagged casualty. Verify
`EvaluateConsequences` returns the expected stable `IncidentId`, `ApplyConsequences` rounds to the
integer value actually sent, and combat state retains both the consequence and applied marker. Extend the
existing faction panel or journal read model to show faction name, incident day, reason, actual integer
delta, and current standing. A UI callback may observe `FactionConsequenceApplied`; it must not call
`ModifyStanding` again. Derive the display from saved combat consequences or an existing canonical
consequence ledger instead of creating an incident store.

**Catalog decision:** Choose one of two explicit dispositions after the Phase 0 audit: (A) bind
`faction_combat_thresholds.json` to the bridge and make its threshold and rule values actually
participate in the pure evaluation formula, including malformed-row rejection and deterministic order; or
(B) mark the catalog informational and remove any plan promise that it balances combat. Do not load JSON
into an unused list and call the feature data-driven. Any new rule must keep `MaxCasualtyPenalty`,
self-defense, assistance and friendly-fire semantics coherent, with one authority for the final standing
delta.

**Acceptance:** Resolve, save, reload and inspect a faction encounter; standing changes once and the same
incident appears in the panel. A restored resolved combat with an unavailable sink remains pending, then
applies once when the sink binds. A repeated end callback does not alter standing. A factionless enemy
produces no political incident. A hostile patrol or raid reaction is a later separately claimed consumer
phase, contingent on its owner accepting a typed incident.

## 10. Dependency-ordered implementation phases

### Phase 0 — Premise and claim


**Action:** Read current source/data; diff the old plan against delivered work; claim exact paths before
an edit. **Gate:** A signed package lists files, owner, non-goals, acceptance and focused command.
**Dependency:** preceding phase accepted. Shared composition roots remain integrator-owned; if a phase
needs them, package the exact seam for the integrator.

### Phase 1 — Core/consumer contract


**Action:** Identify one producer fact and one destination owner. Add the smallest typed query or command
if the owner agrees. **Gate:** A domain unit exercise proves input, refusal, and stable identity.
**Dependency:** preceding phase accepted. Shared composition roots remain integrator-owned; if a phase
needs them, package the exact seam for the integrator.

### Phase 2 — Data validation


**Action:** Extend only the existing catalog as required by Phase 1; validate references and ranges.
**Gate:** One valid and one invalid row produce clear, reproducible results. **Dependency:** preceding
phase accepted. Shared composition roots remain integrator-owned; if a phase needs them, package the
exact seam for the integrator.

### Phase 3 — Save and restore


**Action:** Bind existing section; introduce versioned state only for genuinely new durable facts.
**Gate:** Round-trip, old-save baseline, and replay-after-restore agree. **Dependency:** preceding phase
accepted. Shared composition roots remain integrator-owned; if a phase needs them, package the exact seam
for the integrator.

### Phase 4 — Host composition


**Action:** Bind in the current session at correct setup order, route result to destination owner once.
**Gate:** A focused host-level check observes the real destination effect. **Dependency:** preceding
phase accepted. Shared composition roots remain integrator-owned; if a phase needs them, package the
exact seam for the integrator.

### Phase 5 — UI and narrative


**Action:** Bind existing panel or claimed route to a read model; show action and blocker truthfully.
**Gate:** A headless or bounded runtime check opens/refreshes/closes without a stale claim.
**Dependency:** preceding phase accepted. Shared composition roots remain integrator-owned; if a phase
needs them, package the exact seam for the integrator.

### Phase 6 — Acceptance and handoff


**Action:** Run only directly affected focused test files and necessary runtime probe. **Gate:** Handoff
includes exact commands/results, limitations, and shared paths untouched. **Dependency:** preceding phase
accepted. Shared composition roots remain integrator-owned; if a phase needs them, package the exact seam
for the integrator.

## 11. File impact map


| Path | Action after claim | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Host/CombatHostSession.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.Expeditions.cs` | READ / integrator MODIFY | composition root | high |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/FactionsPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs`
for the first contract check, then only the directly affected save and host targets identified by Phase
0. Run a Godot headless probe only if the claimed change affects the runtime host path. A compile result
cannot establish live route reachability. Acceptance requires: (1) one real input; (2) one canonical
consumer effect; (3) one durable restore; (4) repeat delivery without duplicate effect; (5) visible
correct state and blocker; (6) no new authority.
Keep changes in reviewable phase commits. If a consumer hook fails, revert that phase without replacing
the canonical owner; preserve old saves and catalog compatibility. If a migrated section cannot load,
stop before adding UI or content and give the integrator the exact schema and fixture. Record any
deferred edge with a current evidence pointer and promotion condition rather than claiming it complete.

## 13. Detailed integration acceptance cards

The following cards are planning checks, not a request to create one test method per card. Select the
smallest independent cases that prove the changed contract, save/load, determinism, lifecycle, and
cross-system behavior. “Legacy intent” cards are explicitly conditional: first prove their premise and
owner, then either promote as a separate bounded package or mark them retired. This prevents the
2026-09-01 plan text from resurrecting already delivered or contradictory architecture.

### 01. faction-tagged enemy kill [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns faction-tagged enemy kill. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For faction-tagged enemy kill, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For faction-tagged enemy kill, deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For faction-tagged enemy kill, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For faction-tagged enemy kill, replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime
must refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Day/order boundary.** For faction-tagged enemy kill, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For faction-tagged enemy kill, open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome,
and preserves close/back/focus behavior. A label that announces a benefit absent from the owning system
fails this card. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For faction-tagged enemy kill, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For faction-tagged enemy kill, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 02. downed enemy [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns downed enemy. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For downed enemy, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For downed enemy, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For downed enemy, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For downed enemy, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For downed enemy, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For downed enemy, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For downed enemy, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For downed enemy, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 03. friendly fire [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns friendly fire. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For friendly fire, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For friendly fire, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For friendly fire, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For friendly fire, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For friendly fire, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For friendly fire, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For friendly fire, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For friendly fire, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 04. self-defense [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns self-defense. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For self-defense, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For self-defense, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For self-defense, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For self-defense, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For self-defense, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For self-defense, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For self-defense, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For self-defense, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 05. assisted faction victory [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns assisted faction victory. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For assisted faction victory, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For assisted faction victory, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For assisted faction victory, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For assisted faction victory, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For assisted faction victory, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For assisted faction victory, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For assisted faction victory, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For assisted faction victory, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 06. multi-faction encounter [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns multi-faction encounter. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For multi-faction encounter, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For multi-faction encounter, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For multi-faction encounter, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For multi-faction encounter, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For multi-faction encounter, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For multi-faction encounter, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For multi-faction encounter, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For multi-faction encounter, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 07. factionless combatant [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns factionless combatant. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For factionless combatant, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For factionless combatant, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For factionless combatant, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For factionless combatant, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For factionless combatant, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For factionless combatant, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For factionless combatant, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For factionless combatant, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 08. wilderness patrol [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns wilderness patrol. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For wilderness patrol, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For wilderness patrol, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For wilderness patrol, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For wilderness patrol, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For wilderness patrol, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For wilderness patrol, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For wilderness patrol, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For wilderness patrol, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 09. shelter raid [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns shelter raid. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For shelter raid, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For shelter raid, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For shelter raid, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For shelter raid, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For shelter raid, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For shelter raid, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For shelter raid, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For shelter raid, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 10. expedition battle [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns expedition battle. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For expedition battle, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For expedition battle, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For expedition battle, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For expedition battle, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For expedition battle, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For expedition battle, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For expedition battle, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For expedition battle, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 11. restored resolved encounter [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns restored resolved encounter. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For restored resolved encounter, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For restored resolved encounter, deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not
solve duplication by a process-local boolean that disappears on reload. Acceptance evidence should name
the actual method, stable ID, source day, destination state field or read model, and exact refusal code
if the operation is unavailable.

**Save and restore.** For restored resolved encounter, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For restored resolved encounter, replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime
must refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Day/order boundary.** For restored resolved encounter, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For restored resolved encounter, open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome,
and preserves close/back/focus behavior. A label that announces a benefit absent from the owning system
fails this card. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For restored resolved encounter, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For restored resolved encounter, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 12. pending sink binding [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns pending sink binding. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For pending sink binding, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For pending sink binding, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For pending sink binding, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For pending sink binding, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For pending sink binding, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For pending sink binding, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For pending sink binding, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For pending sink binding, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 13. duplicate combat end [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns duplicate combat end. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For duplicate combat end, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For duplicate combat end, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For duplicate combat end, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For duplicate combat end, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For duplicate combat end, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For duplicate combat end, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For duplicate combat end, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For duplicate combat end, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 14. standing clamp [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns standing clamp. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For standing clamp, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For standing clamp, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For standing clamp, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For standing clamp, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For standing clamp, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For standing clamp, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For standing clamp, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For standing clamp, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 15. host journal record [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns host journal record. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For host journal record, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For host journal record, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For host journal record, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For host journal record, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For host journal record, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For host journal record, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For host journal record, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For host journal record, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 16. faction panel history [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns faction panel history. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For faction panel history, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For faction panel history, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For faction panel history, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For faction panel history, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For faction panel history, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For faction panel history, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For faction panel history, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For faction panel history, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 17. catalog threshold [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns catalog threshold. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For catalog threshold, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For catalog threshold, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For catalog threshold, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For catalog threshold, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For catalog threshold, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For catalog threshold, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For catalog threshold, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For catalog threshold, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 18. faction rule [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns faction rule. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For faction rule, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For faction rule, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For faction rule, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For faction rule, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For faction rule, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For faction rule, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For faction rule, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For faction rule, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 19. quest reaction [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns quest reaction. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For quest reaction, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For quest reaction, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For quest reaction, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For quest reaction, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For quest reaction, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For quest reaction, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For quest reaction, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For quest reaction, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 20. raid response [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns raid response. Start from
`Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
`Assets/StreamingAssets/Data/faction_combat_thresholds.json` and trace any effect through
`src/Host/CombatHostSession.cs` to its current destination owner. The persisted carrier is `combat`
unless the premise audit proves that the source and destination already persist their own halves. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For raid response, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For raid response, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For raid response, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For raid response, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For raid response, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For raid response, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For raid response, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For raid response, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `CombatFactionBridge.cs` in `Assets/Ashfall.Core/Combat/`. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `CombatFactionConsequence` DTO: `factionId`, `standingDelta` (-50 to +50), `reason`
  (killed_allied/killed_enemy/defeated_patrol/assisted_rebellion), `day`, `witnessed` bool, `severity`
  (minor/moderate/major). Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `CombatFactionState` DTO: list of consequences, list of faction reactions, cooldown map
  (faction → last incident day). Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define standing delta rules:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Create `ICombatFactionSink` interface for `TacticalCombatSystem` to report combat outcomes.
  Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Implement consequence calculation: read combatant faction_ids, compare to player's faction
  standing, calculate delta. Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Create cooldown system: same faction can't be incident more than once per 7 days (prevents
  farming). Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Add deterministic seeding: witness detection uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Wire into `GameBootstrap`: `SetupCombatFactionBridge`, `SaveCombatFaction`. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Create `CombatFactionCatalogLoader` for standing delta rules per faction pair. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Implement consequence logging: all combat→faction events recorded for UI/epilogue. Verify
  against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add faction reaction system: factions respond to combat incidents (diplomatic protests,
  bounties, praise). Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Create UI hook: faction panel shows recent combat incidents and standing changes. Verify
  against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement combat outcome reporting:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Create combat context detection:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement witness detection:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Create faction reaction mechanics:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement combat reputation system:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Create combat diplomacy options:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement combat justification system:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Create combat faction events:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Add UI: "Combat Record" panel showing faction combat history and standing impacts. Verify
  against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Create combat journal: automatic log of politically significant combats. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Implement combat faction tutorial: first-time combat with faction explains consequences.
  Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add combat faction tooltips: hover over combatant shows faction and standing impact. Verify
  against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Create 10 combat faction scenarios in data file. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add combat faction interaction with other systems:. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Wire into `TacticalCombatSystem`: combat outcomes reported to bridge. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Connect to `FactionBranchCoordinator`: standing deltas applied. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Integrate with `FactionStanceEngine`: trust values updated. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to `ExpeditionSystem`: expedition combats affect standing. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Wire into `MoralChoiceSystem`: faction killing is moral choice. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Connect to `ShelterDefenseSystem`: defense against factions affects standing. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Implement old-save compatibility: existing saves get empty combat faction state. Verify
  against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Add deterministic seeding: witness detection uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Create exploit prevention: cooldowns prevent standing farming. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Add tests: standing delta calculation, witness detection, cooldown, save round-trip,
  determinism. Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Verify catalog integrity: all faction IDs in combat catalog resolve. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Test edge cases: no faction combatants (no consequence), all faction combatants (major
  consequence). Verify against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Verify headless behavior: bridge processes correctly without UI. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Add data-integrity-selftest: combat faction rules validate against faction catalog. Verify
  against `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Create `--combat-faction-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0
premise audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate
achievement/profile/standing/discovery/animal state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh
Ashfall.Core.Tests/Combat/Plan139CombatFactionBridgeIntegrationTests.cs` plus targeted owner save/host
checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement
the first accepted vertical slice. This plan does not itself alter production code.
