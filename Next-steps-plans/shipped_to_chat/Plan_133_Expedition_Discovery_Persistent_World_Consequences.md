# Plan 133 — Expedition Discovery → Persistent World Consequences

## Current-evidence architecture decision (2026-09-24; supersedes stale instructions below)

This section is the operative integration architecture for **Expedition discovery consequences**. It is a plan, not an implementation claim. The baseline material below remains a scenario inventory; when it says a missing file is “new,” requests a separate registry, or assumes a host count, this current-evidence section controls. Current source was inspected on 2026-09-24. Recheck it at the start of a claimed implementation package because concurrent integration work may have moved the seams.

**Verified premise:** The discovery system is composed and persisted in the expedition aggregate. Main binds location discovery and applies standing through YearOfAsh.FactionWar.ModifyStanding. Residual map, caravan, quest, and choice consumers require separate premise checks.

**Master authority mapping:** C5 expeditions, C6 geography, C7 factions, C10 quests, C17 host. The master expansion document `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` supplies the Part II premise sweep, evidence labels, anti-duplication firewall, and Lane A prose, Lane B mechanics, Lane D save, and Lane E player-surface questions. Its scene and content candidates are ideas, not evidence that a route currently works. The live source and AGENTS.md take precedence.

**Bounded outcome:** Establish one source fact or player command, one canonical consumer, one durable result, and one truthful player readout for the next unsealed gap. The first phase should close a single representative path. A later content batch, cross-system extension, or balancing pass is a separate path claim. Do not invent passive yields or a second world ledger.

### Authority and custody map

| Concern | Custody | Required proof before a change |
|---|---|---|
| Domain rule | `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | Inspect public API, mutation and capture/restore; amend it only for an observed gap. |
| Producer | `ExpeditionSystem.OnLocationDiscovered` | Show that the event follows a committed action and carries stable IDs/day. |
| Host composition | `src/Host/ExpeditionHostSession.cs and src/Main.Expeditions.cs` | Show setup, restore, bind, dirty/save, reset and lifecycle order in the current tree. |
| Destination effect | `the named map, faction, caravan, or quest owner` | Call its existing command once; do not mirror its state in this plan. |
| Persistence | `expedition aggregate` | Save both source marker and effect custody; prove retry cannot duplicate. |
| Player readout | `ExpeditionPanel and destination/map projection` | Display provenance, current status, next command and refusal using existing state. |

### Phase gates

0. **Claim and recensus.** Read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, the current domain owner, the specific host route, save registration and catalog loader. Record the exact files a builder may edit. If a current active claim overlaps, wait for its handoff. If source contradicts this page, amend this page before coding.
1. **Domain fact.** Pick one case from the scenario inventory below. Specify input identity, accepted preconditions, typed result, mutation owner, idempotency key and failure code. A UI callback cannot decide domain legality. If an existing owner already supplies the effect, use it and retire the proposed duplicate.
2. **Catalog and prose.** Inspect the actual JSON row shape and consumer. Add only reachable rows with unique IDs and valid references. Write concise diegetic text for a real state and a separate refusal. A copy field is never a source of numeric gameplay rules. The master’s Lane A archetype may suggest tone, but the current loader sets field limits.
3. **Save and deterministic replay.** Identify the exact save DTO and section. Capture after the authoritative mutation, restore before event rebinding, and replay the same input. Use the campaign RNG fork only where the current owner already consumes it; no wall clock, hash-order sampling or process-local dedupe. Treat missing old fields as the documented baseline and reject malformed new values before they enter live state.
4. **Host composition.** Bind the producer to `the named map, faction, caravan, or quest owner` in the existing host lifetime. Respect setup order, reset, dirty tracking, save orchestration, and unsubscription. If a consumer is optional, specify the withheld effect and a truthful unavailable reason; do not silently report success. Shared `Main` and registry files belong to the named integrator.
5. **Presentation.** Bind `ExpeditionPanel and destination/map projection` to a read model and command result. Render stable IDs as authored labels only after validating a lookup. Keep keyboard/controller focus, close/back, refresh and disposal correct; expose reasons in words, not color alone. Do not let opening a panel trigger a milestone, trade, or consequence.
6. **Focused acceptance and handoff.** Select the smallest directly relevant test file under `TEST_POLICY.md` when implementation is authorized. Prove fresh path, repeated input, save/restore boundary, invalid reference, headless host route if touched, and UI projection if touched. Record exact commands and results. This document edit does not run implementation tests.

### Code integration framework: current API anchors and proposed placement

**Verified call anchor:** `RegisterExpeditionDiscovery(locationId, _simDay)` is already called from `Main.BindDiscoveryConsequences`; the host records a consequence ledger provenance flag and sends faction deltas to `FactionWar.ModifyStanding`.

The following C# fragment identifies an existing method and its intended position in the current host. It is a placement guide, not a new authority type or a copy-and-paste patch. Variables and refusal types come from the owning method. A builder must open that source file and reconcile the exact signature in Phase 0.

```csharp
_expeditions.Engine.OnLocationDiscovered += locationId =>
    _expeditions.DiscoveryConsequences.RegisterExpeditionDiscovery(locationId, _simDay);
// The existing OnDiscoveryConsequenceApplied binding owns the standing handoff.
// Extend one missing destination consumer only after its command is verified.
```

**Source contract.** Accept a fact only after the owning command commits. Carry its stable ID, subject, campaign day and authored row ID through the host boundary. Distinguish a query from a mutation: `Evaluate` may mutate counters in some current Core systems, so call sites must be inspected instead of assuming the method name is pure. A panel asks for a read model; a player command asks the existing host session to validate and commit. Treat a missing subject or catalog row as a refusal before changing any destination owner.

**Destination contract.** The destination owner alone changes the named map, faction, caravan, or quest owner. The host carries a typed fact or uses the owner’s established delegate; it does not copy the destination value into another mutable store. The implementation review must record method name, input ID and before/after destination state. If the destination lacks a safe command, record an authority decision rather than writing its fields directly. A projected outcome is useful only if it can be traced back to the original committed fact.

**Save and retry contract.** Capture expedition aggregate after mutation and before reporting a durable result. Where two owners persist separately, the handoff must name save order and recovery for a crash between writes. A repeated fact ID after restore must be a no-op or resume one pending effect, depending on the current owner’s marker semantics. No process-local boolean can stand in for persisted applied identity. The old-save baseline should be documented with one fixture and a stable outcome; corruption should fail without partially mutating an unrelated section.

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

Make each authored discovery consequence observable and effective through the existing map, caravan,
faction, economy, and quest owners, with a bounded first slice that closes one real
destination-to-outcome path.

**Current state:** PARTIAL: expedition destination discovery is recorded, outcomes are in the expedition
aggregate, and standing outcomes reach Year of Ash; map, caravan, quest, and player choice consumers need
individual premise checks.

**Non-goals:** No replacement world simulation, no independent discovery save section, no passive
resource income until the inventory/economy owner exposes an approved transaction, no fabricated
discovery tags, no automatic faction rumor simulation.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status

The implementation authority is `DiscoveryConsequenceSystem inside ExpeditionHostSession`. Verified entry
points inspected for this revision: `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`,
`src/Host/ExpeditionHostSession.cs`, `src/Main.Expeditions.cs`,
`Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`, `src/UI/ExpeditionPanel.cs`, and
`Assets/StreamingAssets/Data/discovery_consequences.json`. The focused test starting point is
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs`. Existing save ownership
is `expedition`. Paths are evidence pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: C5 expeditions,
C6 geography, C7 factions, C10 quests, C11 economy, and C17 host presentation; the master factory treats
the live source as higher authority and requires one bounded outcome per phase. Its Part II requires live
premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The relevant
deep maps and lane matrices guide coverage; they do not override newer code. The master compilation
itself warns against padding and stale repository assumptions. This plan therefore records concrete
contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Verified or directly observed:** Destination identification must remain idempotent through
  RegisterExpeditionDiscovery(locationId, day).
- **Verified or directly observed:** Threat-cleared safety bonus is currently computed; prove a canonical
  caravan risk consumer reads it before claiming route safety.
- **Verified or directly observed:** Standing deltas already flow through
  _yearOfAsh.FactionWar.ModifyStanding; do not send the same delta through FactionStanceEngine.
- **Verified or directly observed:** LocationEvolutionSystem owns world-location mutations; the bridge
  must ask it to apply a typed change, never mirror its fields.
- **Verified or directly observed:** Quest availability must use the current quest runtime or flag
  ledger; a discovery title is not proof of a reachable quest.
- **Verified or directly observed:** Concealment and exploitation must be player commands whose legality
  is checked in Core and whose result survives reload.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public
methods and save snapshots, and write a one-page premise note. If another live owner already mutates the
target concern, use it. If a required write API does not exist, return the proposed consumer hook to the
foreman as an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix


| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/discovery_consequences.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Host/ExpeditionHostSession.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `expedition` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/ExpeditionPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Proposed adapter shape; add only after confirming the current host signature.
void ApplyDiscoveryOutcome(ConsequenceOutcome outcome)
{
    if (outcome == null || string.IsNullOrWhiteSpace(outcome.ConsequenceId)) return;
    // The expedition aggregate records provenance. Each consumer owns its mutation.
    // Use one durable applied marker per (outcome id, consumer), then invoke the owner.
}
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent
must confirm names and signatures against the live tree immediately before coding. Keep
`Assets/Ashfall.Core/` free of Godot and Unity references. If an adapter needs a callback, bind it in the
current host lifetime, unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs` and the registry entry for `expedition`.
Write down exact DTO version, constructor baseline, capture point, restore point, and dirty flag. For a
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

Inspect the current route to `src/UI/ExpeditionPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and
uncertainty where the game cannot know more. Refresh on authoritative state change and after restore;
unbind subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 9A. First integration slice: route safety with durable provenance


**Premise proved in source:** `RegisterExpeditionDiscovery` emits a stable
`expedition_discovery_<locationId>` record and an outcome; `ExpeditionHostSession.CaptureSaveAggregate`
carries `discoveryConsequences`; `Main.Expeditions.BindDiscoveryConsequences` routes standing and writes
a consequence-ledger fact. `GetTotalCaravanSafetyBonus` is displayed in the expedition host report, but
the repository search found no caravan risk consumer of that method. The first slice is therefore a real
threat-cleared outcome affecting a single canonical caravan risk calculation, pending the caravan owner's
API audit. Do not claim the existing report text as a gameplay safety effect.

**Core contract:** A cleared-threat event must carry a stable encounter or location fact key.
`RegisterDiscovery` currently generates sequential IDs; repeated external delivery can make duplicate
discoveries. `ExploitDiscovery` and `EscalateDiscovery` create a new `ConsequenceId` on every call.
Before exposing either as a player command, add a domain guard keyed by discovery and transition kind, or
constrain the producer to a proven once-only source. Preserve existing saved sequence numbers and old
records. Define whether a concealed record can be escalated and whether a terminal exploited record can
be concealed; reject illegal transitions with a typed result.

**Consumer contract:** The caravan owner reads a bounded 0..0.50 safety projection at its own
journey-risk boundary. It keeps route, cargo and encounter authority. If it only supports global risk,
the phase may expose advisory text but cannot call route-specific safety integrated. A location-scoped
projection needs an existing route-to-location mapping and a proven route query before it is scheduled.
The world-map owner applies location mutations through its own method; the quest owner accepts only
authored, resolvable quest IDs. No implicit resource extraction or faction rumor is included in this
slice.

**Acceptance:** One threat-cleared fact produces one outcome in `expedition` save, one changed caravan
risk for an affected canonical journey, and a report that quotes the applied bonus. A second delivery and
a reload preserve the same effect exactly once. A different route sees its owner-defined baseline unless
the route mapping explicitly includes the cleared location. A concealed discovery does not silently grant
a safety benefit if the gameplay contract says concealment suppresses public effects. Record that rule in
Core and a focused test before UI exposure.

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
| `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/discovery_consequences.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Host/ExpeditionHostSession.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.Expeditions.cs` | READ / integrator MODIFY | composition root | high |
| `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/ExpeditionPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh
Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` for the first contract
check, then only the directly affected save and host targets identified by Phase 0. Run a Godot headless
probe only if the claimed change affects the runtime host path. A compile result cannot establish live
route reachability. Acceptance requires: (1) one real input; (2) one canonical consumer effect; (3) one
durable restore; (4) repeat delivery without duplicate effect; (5) visible correct state and blocker; (6)
no new authority.
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

### 01. resource deposit [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns resource deposit. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: consequence_types:
{"auto_escalate_days": 14, "caravan_safety_bonus": 0.0, "description_template": "A cache of
{resource_yield} has been located at {location_id}. Exploitation potential is high.", "discovery_type":
"ResourceDeposit", "faction_standing_delta": 0.0, "label": "Resource Deposit", "type_id":
"csq_resource_deposit"}

**Fresh campaign path.** For resource deposit, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For resource deposit, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For resource deposit, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For resource deposit, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For resource deposit, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For resource deposit, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For resource deposit, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For resource deposit, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 02. threat cleared [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns threat cleared. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: consequence_types:
{"auto_escalate_days": 30, "caravan_safety_bonus": 0.15, "description_template": "Hostile presence
neutralised at {location_id}. Caravan lanes through this corridor are now safer.", "discovery_type":
"ThreatCleared", "faction_standing_delta": 3.0, "label": "Threat Cleared", "type_id":
"csq_threat_cleared"}

**Fresh campaign path.** For threat cleared, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For threat cleared, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For threat cleared, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For threat cleared, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For threat cleared, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For threat cleared, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For threat cleared, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For threat cleared, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 03. ruins uncovered [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns ruins uncovered. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: consequence_types:
{"auto_escalate_days": 10, "caravan_safety_bonus": 0.05, "description_template": "Pre-war structure
exposed at {location_id}. Scavengers and archaeologists are taking notice.", "discovery_type":
"RuinsUncovered", "faction_standing_delta": 0.0, "label": "Ruins Uncovered", "type_id":
"csq_ruins_uncovered"}

**Fresh campaign path.** For ruins uncovered, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For ruins uncovered, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For ruins uncovered, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For ruins uncovered, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For ruins uncovered, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For ruins uncovered, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For ruins uncovered, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For ruins uncovered, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 04. faction contact [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns faction contact. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: consequence_types:
{"auto_escalate_days": 21, "caravan_safety_bonus": 0.0, "description_template": "First contact
established with a faction operating out of {location_id}. Diplomatic opportunity open.",
"discovery_type": "FactionContact", "faction_standing_delta": 8.0, "label": "Faction Contact", "type_id":
"csq_faction_contact"}

**Fresh campaign path.** For faction contact, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For faction contact, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For faction contact, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For faction contact, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For faction contact, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For faction contact, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For faction contact, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For faction contact, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 05. strategic location [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns strategic location. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: consequence_types:
{"auto_escalate_days": 60, "caravan_safety_bonus": 0.1, "description_template": "A strategically
significant position has been mapped at {location_id}.", "discovery_type": "StrategicLocation",
"faction_standing_delta": 0.0, "label": "Strategic Location", "type_id": "csq_strategic_location"}

**Fresh campaign path.** For strategic location, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For strategic location, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For strategic location, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For strategic location, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For strategic location, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For strategic location, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For strategic location, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For strategic location, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 06. destination identification [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns destination identification. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For destination identification, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For destination identification, deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For destination identification, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For destination identification, replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime
must refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Day/order boundary.** For destination identification, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For destination identification, open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome,
and preserves close/back/focus behavior. A label that announces a benefit absent from the owning system
fails this card. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For destination identification, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For destination identification, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 07. conceal discovery [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns conceal discovery. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For conceal discovery, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For conceal discovery, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For conceal discovery, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For conceal discovery, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For conceal discovery, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For conceal discovery, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For conceal discovery, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For conceal discovery, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 08. reveal discovery [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns reveal discovery. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For reveal discovery, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For reveal discovery, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For reveal discovery, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For reveal discovery, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For reveal discovery, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For reveal discovery, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For reveal discovery, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For reveal discovery, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 09. exploit discovery [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns exploit discovery. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For exploit discovery, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For exploit discovery, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For exploit discovery, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For exploit discovery, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For exploit discovery, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For exploit discovery, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For exploit discovery, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For exploit discovery, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 10. escalate discovery [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns escalate discovery. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For escalate discovery, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For escalate discovery, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For escalate discovery, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For escalate discovery, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For escalate discovery, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For escalate discovery, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For escalate discovery, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For escalate discovery, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 11. caravan route safety [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns caravan route safety. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For caravan route safety, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For caravan route safety, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For caravan route safety, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For caravan route safety, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For caravan route safety, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For caravan route safety, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For caravan route safety, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For caravan route safety, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 12. faction standing [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns faction standing. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For faction standing, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For faction standing, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For faction standing, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For faction standing, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For faction standing, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For faction standing, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For faction standing, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For faction standing, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 13. location mutation [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns location mutation. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For location mutation, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For location mutation, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For location mutation, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For location mutation, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For location mutation, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For location mutation, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For location mutation, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For location mutation, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 14. quest availability [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns quest availability. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For quest availability, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For quest availability, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For quest availability, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For quest availability, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For quest availability, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For quest availability, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For quest availability, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For quest availability, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 15. economy availability [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns economy availability. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For economy availability, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For economy availability, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For economy availability, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For economy availability, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For economy availability, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For economy availability, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For economy availability, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For economy availability, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 16. journal evidence [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns journal evidence. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For journal evidence, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For journal evidence, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For journal evidence, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For journal evidence, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For journal evidence, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For journal evidence, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For journal evidence, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For journal evidence, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 17. expedition report [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns expedition report. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For expedition report, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For expedition report, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For expedition report, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For expedition report, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For expedition report, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For expedition report, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For expedition report, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For expedition report, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 18. catalog validation [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns catalog validation. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For catalog validation, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For catalog validation, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For catalog validation, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For catalog validation, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For catalog validation, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For catalog validation, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For catalog validation, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For catalog validation, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 19. old-save baseline [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns old-save baseline. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For old-save baseline, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For old-save baseline, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For old-save baseline, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For old-save baseline, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old-save baseline, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old-save baseline, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old-save baseline, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For old-save baseline, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 20. multi-discovery location [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns multi-discovery location. Start from
`Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
`Assets/StreamingAssets/Data/discovery_consequences.json` and trace any effect through
`src/Host/ExpeditionHostSession.cs` to its current destination owner. The persisted carrier is
`expedition` unless the premise audit proves that the source and destination already persist their own
halves. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row matches this
case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For multi-discovery location, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For multi-discovery location, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For multi-discovery location, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For multi-discovery location, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For multi-discovery location, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For multi-discovery location, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For multi-discovery location, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For multi-discovery location, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse
`Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `DiscoveryConsequenceSystem.cs` in `Assets/Ashfall.Core/Expeditions/`. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `Discovery` DTO: `id`, `locationId`, `discoveryType`
  (resource/threat/ruins/faction_contact/strategic), `discoverySubtype`, `discoveredDay`, `exploited`
  bool, `revealed` bool, `consequenceTriggered` bool, `tags`. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `DiscoveryConsequenceState` DTO: list of discoveries, list of triggered consequences,
  faction awareness map. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define discovery types with distinct consequence chains:. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Create `IDiscoverySource` interface for expedition system to report discoveries. Verify
  against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Implement consequence trigger rules: each discovery type has consequence conditions. Verify
  against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Create faction awareness system: factions learn about discoveries over time (via rumor network
  — Plan 131 integration point). Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define consequence escalation: ignored discoveries attract more attention. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Create discovery concealment mechanic: player can hide discoveries (cost: forego benefits,
  avoid faction interest). Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Wire into `LocationEvolutionSystem`: discoveries update location mutation records. Verify
  against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Add deterministic seeding: consequence timing uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Wire into `GameBootstrap`: `SetupDiscoveryConsequences`, `TickConsequences`,
  `SaveConsequences`. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Create `DiscoveryCatalogLoader` for static discovery templates. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement resource deposit discovery:. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement threat cleared discovery:. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement ruins uncovered discovery:. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement faction contact discovery:. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement strategic location discovery:. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Create 20 discovery templates in `discovery_consequences.json`. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement discovery concealment: player can mark discovery as "hidden" (no consequences but no
  benefits). Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Create discovery revelation: player can reveal hidden discoveries later (delayed
  consequences). Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Implement consequence escalation timer: unaddressed discoveries attract more faction
  attention. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Add UI: "Discovery Log" panel showing discoveries and consequence status. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Create expedition report: post-expedition summary of discoveries and immediate choices. Verify
  against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Implement discovery interaction: multiple discoveries at same location compound consequences.
  Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Add discovery decay: some discoveries become irrelevant over time (resource depleted, threat
  returned). Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Create discovery trade: player can sell discovery information to factions for standing. Verify
  against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Wire into `LocationEvolutionSystem`: discoveries update location mutation records (owner,
  threat level, resource status). Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Connect to faction systems: faction awareness of discoveries affects standing and diplomatic
  options. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Integrate with economy: resource discoveries affect local prices and trade availability.
  Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to quest system: discoveries unlock location-specific quests. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Wire into caravan system: cleared threats improve caravan success rates on affected routes.
  Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Implement old-save compatibility: existing saves get empty discovery state, future expeditions
  generate discoveries. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Add deterministic seeding: consequence timing uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Create exploit prevention: discoveries are one-time events, can't be re-triggered by
  save/load. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Add tests: discovery lifecycle (discover → conceal/reveal → consequence), save round-trip,
  determinism. Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Verify catalog integrity: all discovery location IDs resolve to real locations. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Test edge cases: no expeditions (no discoveries), all discoveries concealed (no consequences).
  Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Verify headless behavior: consequences tick correctly without UI. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Add data-integrity-selftest: discovery templates validate against location/faction catalogs.
  Verify against `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Create `--discovery-consequences-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Document consequence architecture for future expansion. Verify against
  `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0
premise audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate
achievement/profile/standing/discovery/animal state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh
Ashfall.Core.Tests/Expeditions/Plan133DiscoveryConsequenceIntegrationTests.cs` plus targeted owner
save/host checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement
the first accepted vertical slice. This plan does not itself alter production code.

## Integration casebooks: producer, custody, presentation, and failure

These casebooks turn the earlier scenario names into reviewable implementation questions. They are acceptance design, not claims that every feature already exists or that every case needs one test method. Select one bounded case per implementation package and record evidence before promoting it.

### 001. resource deposit — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the resource deposit result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 002. threat cleared — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the threat cleared result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 003. ruins uncovered — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the ruins uncovered result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 004. faction contact — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the faction contact result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 005. strategic location — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the strategic location result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 006. destination identification — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the destination identification result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 007. conceal discovery — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the conceal discovery result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 008. reveal discovery — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the reveal discovery result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 009. exploit discovery — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the exploit discovery result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 010. escalate discovery — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the escalate discovery result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 011. caravan route safety — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the caravan route safety result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 012. faction standing — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the faction standing result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 013. location mutation — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the location mutation result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 014. quest availability — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the quest availability result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 015. economy availability — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the economy availability result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 016. journal evidence — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the journal evidence result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 017. expedition report — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the expedition report result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 018. catalog validation — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the catalog validation result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 019. old-save baseline — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the old-save baseline result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 020. multi-discovery location — Producer and temporal boundary

Write the time line from ExpeditionSystem.OnLocationDiscovered to the multi-discovery location result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 021. resource deposit — Rule and destination handoff

For resource deposit, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 022. threat cleared — Rule and destination handoff

For threat cleared, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 023. ruins uncovered — Rule and destination handoff

For ruins uncovered, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 024. faction contact — Rule and destination handoff

For faction contact, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 025. strategic location — Rule and destination handoff

For strategic location, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 026. destination identification — Rule and destination handoff

For destination identification, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 027. conceal discovery — Rule and destination handoff

For conceal discovery, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 028. reveal discovery — Rule and destination handoff

For reveal discovery, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 029. exploit discovery — Rule and destination handoff

For exploit discovery, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 030. escalate discovery — Rule and destination handoff

For escalate discovery, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 031. caravan route safety — Rule and destination handoff

For caravan route safety, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 032. faction standing — Rule and destination handoff

For faction standing, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 033. location mutation — Rule and destination handoff

For location mutation, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 034. quest availability — Rule and destination handoff

For quest availability, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 035. economy availability — Rule and destination handoff

For economy availability, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 036. journal evidence — Rule and destination handoff

For journal evidence, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 037. expedition report — Rule and destination handoff

For expedition report, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 038. catalog validation — Rule and destination handoff

For catalog validation, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 039. old-save baseline — Rule and destination handoff

For old-save baseline, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 040. multi-discovery location — Rule and destination handoff

For multi-discovery location, let Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs decide the rule and send only the completed fact to the named map, faction, caravan, or quest owner. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 041. resource deposit — Persistence and replay

Capture expedition aggregate immediately before the resource deposit command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 042. threat cleared — Persistence and replay

Capture expedition aggregate immediately before the threat cleared command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 043. ruins uncovered — Persistence and replay

Capture expedition aggregate immediately before the ruins uncovered command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 044. faction contact — Persistence and replay

Capture expedition aggregate immediately before the faction contact command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 045. strategic location — Persistence and replay

Capture expedition aggregate immediately before the strategic location command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 046. destination identification — Persistence and replay

Capture expedition aggregate immediately before the destination identification command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 047. conceal discovery — Persistence and replay

Capture expedition aggregate immediately before the conceal discovery command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 048. reveal discovery — Persistence and replay

Capture expedition aggregate immediately before the reveal discovery command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 049. exploit discovery — Persistence and replay

Capture expedition aggregate immediately before the exploit discovery command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 050. escalate discovery — Persistence and replay

Capture expedition aggregate immediately before the escalate discovery command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 051. caravan route safety — Persistence and replay

Capture expedition aggregate immediately before the caravan route safety command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 052. faction standing — Persistence and replay

Capture expedition aggregate immediately before the faction standing command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 053. location mutation — Persistence and replay

Capture expedition aggregate immediately before the location mutation command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 054. quest availability — Persistence and replay

Capture expedition aggregate immediately before the quest availability command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 055. economy availability — Persistence and replay

Capture expedition aggregate immediately before the economy availability command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 056. journal evidence — Persistence and replay

Capture expedition aggregate immediately before the journal evidence command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 057. expedition report — Persistence and replay

Capture expedition aggregate immediately before the expedition report command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 058. catalog validation — Persistence and replay

Capture expedition aggregate immediately before the catalog validation command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 059. old-save baseline — Persistence and replay

Capture expedition aggregate immediately before the old-save baseline command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 060. multi-discovery location — Persistence and replay

Capture expedition aggregate immediately before the multi-discovery location command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 061. resource deposit — Catalog and prose contract

Inspect the current authored row relevant to resource deposit and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 062. threat cleared — Catalog and prose contract

Inspect the current authored row relevant to threat cleared and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 063. ruins uncovered — Catalog and prose contract

Inspect the current authored row relevant to ruins uncovered and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 064. faction contact — Catalog and prose contract

Inspect the current authored row relevant to faction contact and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 065. strategic location — Catalog and prose contract

Inspect the current authored row relevant to strategic location and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 066. destination identification — Catalog and prose contract

Inspect the current authored row relevant to destination identification and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 067. conceal discovery — Catalog and prose contract

Inspect the current authored row relevant to conceal discovery and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 068. reveal discovery — Catalog and prose contract

Inspect the current authored row relevant to reveal discovery and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 069. exploit discovery — Catalog and prose contract

Inspect the current authored row relevant to exploit discovery and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 070. escalate discovery — Catalog and prose contract

Inspect the current authored row relevant to escalate discovery and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 071. caravan route safety — Catalog and prose contract

Inspect the current authored row relevant to caravan route safety and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 072. faction standing — Catalog and prose contract

Inspect the current authored row relevant to faction standing and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 073. location mutation — Catalog and prose contract

Inspect the current authored row relevant to location mutation and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 074. quest availability — Catalog and prose contract

Inspect the current authored row relevant to quest availability and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 075. economy availability — Catalog and prose contract

Inspect the current authored row relevant to economy availability and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 076. journal evidence — Catalog and prose contract

Inspect the current authored row relevant to journal evidence and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 077. expedition report — Catalog and prose contract

Inspect the current authored row relevant to expedition report and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 078. catalog validation — Catalog and prose contract

Inspect the current authored row relevant to catalog validation and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 079. old-save baseline — Catalog and prose contract

Inspect the current authored row relevant to old-save baseline and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 080. multi-discovery location — Catalog and prose contract

Inspect the current authored row relevant to multi-discovery location and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 081. resource deposit — Player route and accessibility

Present resource deposit through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 082. threat cleared — Player route and accessibility

Present threat cleared through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 083. ruins uncovered — Player route and accessibility

Present ruins uncovered through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 084. faction contact — Player route and accessibility

Present faction contact through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 085. strategic location — Player route and accessibility

Present strategic location through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 086. destination identification — Player route and accessibility

Present destination identification through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 087. conceal discovery — Player route and accessibility

Present conceal discovery through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 088. reveal discovery — Player route and accessibility

Present reveal discovery through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 089. exploit discovery — Player route and accessibility

Present exploit discovery through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 090. escalate discovery — Player route and accessibility

Present escalate discovery through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 091. caravan route safety — Player route and accessibility

Present caravan route safety through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 092. faction standing — Player route and accessibility

Present faction standing through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 093. location mutation — Player route and accessibility

Present location mutation through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 094. quest availability — Player route and accessibility

Present quest availability through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 095. economy availability — Player route and accessibility

Present economy availability through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 096. journal evidence — Player route and accessibility

Present journal evidence through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 097. expedition report — Player route and accessibility

Present expedition report through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 098. catalog validation — Player route and accessibility

Present catalog validation through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 099. old-save baseline — Player route and accessibility

Present old-save baseline through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 100. multi-discovery location — Player route and accessibility

Present multi-discovery location through ExpeditionPanel and destination/map projection as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 101. resource deposit — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for resource deposit. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 102. threat cleared — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for threat cleared. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 103. ruins uncovered — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for ruins uncovered. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 104. faction contact — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for faction contact. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 105. strategic location — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for strategic location. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 106. destination identification — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for destination identification. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 107. conceal discovery — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for conceal discovery. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 108. reveal discovery — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for reveal discovery. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 109. exploit discovery — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for exploit discovery. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 110. escalate discovery — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for escalate discovery. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 111. caravan route safety — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for caravan route safety. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 112. faction standing — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for faction standing. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 113. location mutation — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for location mutation. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 114. quest availability — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for quest availability. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 115. economy availability — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for economy availability. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 116. journal evidence — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for journal evidence. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 117. expedition report — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for expedition report. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 118. catalog validation — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for catalog validation. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 119. old-save baseline — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for old-save baseline. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 120. multi-discovery location — Failure containment

Use a discovered location whose authored consequence references an absent destination as the negative fixture for multi-discovery location. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 121. resource deposit — Adjacent-owner collision

Trace each effect claimed by resource deposit through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 122. threat cleared — Adjacent-owner collision

Trace each effect claimed by threat cleared through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 123. ruins uncovered — Adjacent-owner collision

Trace each effect claimed by ruins uncovered through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 124. faction contact — Adjacent-owner collision

Trace each effect claimed by faction contact through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 125. strategic location — Adjacent-owner collision

Trace each effect claimed by strategic location through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 126. destination identification — Adjacent-owner collision

Trace each effect claimed by destination identification through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 127. conceal discovery — Adjacent-owner collision

Trace each effect claimed by conceal discovery through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 128. reveal discovery — Adjacent-owner collision

Trace each effect claimed by reveal discovery through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 129. exploit discovery — Adjacent-owner collision

Trace each effect claimed by exploit discovery through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 130. escalate discovery — Adjacent-owner collision

Trace each effect claimed by escalate discovery through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 131. caravan route safety — Adjacent-owner collision

Trace each effect claimed by caravan route safety through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 132. faction standing — Adjacent-owner collision

Trace each effect claimed by faction standing through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 133. location mutation — Adjacent-owner collision

Trace each effect claimed by location mutation through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 134. quest availability — Adjacent-owner collision

Trace each effect claimed by quest availability through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 135. economy availability — Adjacent-owner collision

Trace each effect claimed by economy availability through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 136. journal evidence — Adjacent-owner collision

Trace each effect claimed by journal evidence through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 137. expedition report — Adjacent-owner collision

Trace each effect claimed by expedition report through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 138. catalog validation — Adjacent-owner collision

Trace each effect claimed by catalog validation through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 139. old-save baseline — Adjacent-owner collision

Trace each effect claimed by old-save baseline through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 140. multi-discovery location — Adjacent-owner collision

Trace each effect claimed by multi-discovery location through the current map of owners. Ask whether the same condition already reaches the named map, faction, caravan, or quest owner from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 141. resource deposit — Day order and deterministic boundary

Pin resource deposit to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 142. threat cleared — Day order and deterministic boundary

Pin threat cleared to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 143. ruins uncovered — Day order and deterministic boundary

Pin ruins uncovered to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 144. faction contact — Day order and deterministic boundary

Pin faction contact to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 145. strategic location — Day order and deterministic boundary

Pin strategic location to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 146. destination identification — Day order and deterministic boundary

Pin destination identification to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 147. conceal discovery — Day order and deterministic boundary

Pin conceal discovery to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 148. reveal discovery — Day order and deterministic boundary

Pin reveal discovery to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 149. exploit discovery — Day order and deterministic boundary

Pin exploit discovery to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 150. escalate discovery — Day order and deterministic boundary

Pin escalate discovery to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 151. caravan route safety — Day order and deterministic boundary

Pin caravan route safety to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 152. faction standing — Day order and deterministic boundary

Pin faction standing to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 153. location mutation — Day order and deterministic boundary

Pin location mutation to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 154. quest availability — Day order and deterministic boundary

Pin quest availability to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 155. economy availability — Day order and deterministic boundary

Pin economy availability to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 156. journal evidence — Day order and deterministic boundary

Pin journal evidence to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 157. expedition report — Day order and deterministic boundary

Pin expedition report to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 158. catalog validation — Day order and deterministic boundary

Pin catalog validation to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 159. old-save baseline — Day order and deterministic boundary

Pin old-save baseline to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 160. multi-discovery location — Day order and deterministic boundary

Pin multi-discovery location to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 161. resource deposit — Observation and diagnostics

A focused probe for resource deposit should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 162. threat cleared — Observation and diagnostics

A focused probe for threat cleared should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 163. ruins uncovered — Observation and diagnostics

A focused probe for ruins uncovered should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 164. faction contact — Observation and diagnostics

A focused probe for faction contact should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 165. strategic location — Observation and diagnostics

A focused probe for strategic location should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.


## Polishing pass and architecture handoff

This revision received a second editorial pass after the architecture and casebooks were assembled. The pass normalizes headings and whitespace, treats the current evidence section as authoritative over older speculative instructions, preserves the older scenario inventory as conditional intent, removes the most consequential false present-tense claims, and checks that every implementation phase has an owner, a save rule, a player route, a failure path, and a focused proof. Casebook prose is deliberately phrased as review work where a consumer or route has not been verified. The live-source recensus remains mandatory before implementation, especially where another active claim is changing a host.

**Closeout:** Plan 133 now has a documented integration architecture and a bounded first-slice method. No production behavior has been changed by this document. Runtime completion requires the claimed implementation package, focused verification, and the handoff described above.
