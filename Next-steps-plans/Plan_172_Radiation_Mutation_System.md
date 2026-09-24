# Plan 172 — Radiation Mutation — Exposure and Capability Integration

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Bind one canonical survivor radiation exposure fact to MutationSystem, use deterministic mutation evaluation
at a defined day boundary, then make one real capability or treatment effect observable without duplicating
the dose ledger.

**Current state:** PARTIAL: `MutationSystem`, nine mutation rows, mutation_tree save section,
SetupMutations/SaveMutations, audio event, and MutationTreePanel are live. Search of src found no caller of
AddRadiationExposure or TryMutateSurvivor, so canonical radiation facts do not currently drive the genetic
profile. File comments call this Plan 180, a numbering drift to reconcile.

**Non-goals:** No replacement radiation ledger, no genetics RNG seeded from wall time, no automatic mutation
from merely opening the panel, no new mutation catalog, no presentation-only capability promise, no invented
inheritance until a live lineage consumer is approved.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status

The current or candidate domain authority is `MutationSystem for genetic profile, canonical radiation/dose
owner for exposure facts`. Source and adjacent paths inspected for this revision (some are candidate
consumers rather than active bindings): `Assets/Ashfall.Core/Medical/MutationSystem.cs`,
`src/Main.Plans178_181.cs`, `src/Main.PlayerSurfaces.cs`, `src/Host/MutationSaveStore.cs`,
`src/UI/MutationTreePanel.cs`, and `Assets/StreamingAssets/Data/mutations.json`. The focused test starting
point is `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs`. Persistence boundary
under audit: `mutation_tree section; radiation owner retains dose history`. Paths are evidence pointers, not
advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-2/C2 medical and
radiation plus C9 survivor capacity and C17 host panels: the master requires a single dose owner and proven
downstream consumer. The current source, not historical Plan 172 numbering, controls scope. Its Part II
requires live premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall.
The relevant deep maps and lane matrices guide coverage; they do not override newer code. The master
compilation itself warns against padding and stale repository assumptions. This plan therefore records
concrete contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Evidence or explicit premise:** MutationSystem has AddRadiationExposure, TryMutateSurvivor,
  AdministerRadAway, PerformGeneTherapy, stat/capability/visual projections, and CaptureState/RestoreState.
- **Evidence or explicit premise:** The Core mutation chance uses cumulativeRadDose and geneticInstability;
  eligible nodes are sorted by mutation_id before RNG selection.
- **Evidence or explicit premise:** Main.EnsureMutations loads mutations.json, restores MutationSaveStore,
  and subscribes a journal entry to OnMutationAcquired.
- **Evidence or explicit premise:** SaveSectionRegistry already registers mutation_tree and
  mutation_save.json; a new save section would duplicate existing state.
- **Evidence or explicit premise:** MutationTreePanel is bound through PanelRegistry; its copy currently
  implies severe exposure catalyzes mutation, but no host exposure feed was found in src.
- **Evidence or explicit premise:** The `src/Main.Plans178_181.cs` comment says Plan 180 while the requested
  historical plan is 172; use file and API identities, not numeric label, in claims.

### Radiation and mutation contract dossier

The source dose owner must be identified by an actual committed event or authoritative delta reader. Do not
infer new exposure from a snapshot of the survivor's current dose after load: that double counts historical
radiation. The proposed adapter needs stable event identity or a persisted high-water mark keyed to the
canonical dose owner. It must define what happens when a dose is corrected, an event is replayed, or a
survivor leaves the roster. Genetic cumulative dose is a consequence of exposure events; medical current
dose remains with the medical owner.

`MutationSystem` already owns the tree. The first integration slice should feed one radiation fact, then
evaluate `TryMutateSurvivor` at a specified phase after dose is committed. The current method rolls before
filtering eligible nodes; a balance review may change that, but a bridge must preserve current semantics
unless a focused decision says otherwise. The current `AddRadiationExposure` comment says a spike is above
50 mSv while code checks `dose > 20.0f`; record this discrepancy as an explicit threshold audit and avoid
promising either value in UI until resolved.

`AdministerRadAway` changes the mutation profile but does not itself consume medicine; `PerformGeneTherapy`
removes a named inventory vial. A real treatment route must coordinate the canonical clinical or inventory
command so the item and dose effects commit once. Capability tags and stat modifiers remain inert until a
named consumer queries them. The first consumer should be one specific action, such as low-light expedition
scouting, with a before/after assertion and no parallel survivor stat cache.

The panel shows profiles, counts, and raw IDs. A clinical route should give an eligible survivor, measured
exposure/instability, known active nodes, treatment availability and blocker, and grounded descriptions from
catalog. Presentation must distinguish a true dose reading from genetic history.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/mutations.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Medical/MutationSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.Plans178_181.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `mutation_tree section; radiation owner retains dose history` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/MutationTreePanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Proposed adapter shape; confirm canonical dose event and day phase first.
void OnCanonicalDoseCommitted(string survivorId, float deltaDose, int day, string eventId)
{
    if (WasApplied(eventId)) return;
    mutationSystem.AddRadiationExposure(survivorId, deltaDose, day);
    MarkApplied(eventId); // persist with the receiving mutation profile or existing fact owner
}
// Day owner calls TryMutateSurvivor once per eligible survivor in ordinal ID order.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `src/Host/MutationSaveStore.cs` and the registry for the named concern; absence of a section is a
decision gate, not permission to invent one. Write down exact DTO version, constructor baseline, capture
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

Inspect the current route to `src/UI/MutationTreePanel.cs` and its bind/open/close methods. Expose only
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
| `Assets/Ashfall.Core/Medical/MutationSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/mutations.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.Plans178_181.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.PlayerSurfaces.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/MutationSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/MutationTreePanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` for
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

The following cards are planning checks, not a request to create one test method per card. Each card has one
feature-specific expected outcome; crosscutting checks below it define the evidence needed to accept that
outcome. Select the smallest independent cases that prove the changed contract, save/load, determinism,
lifecycle, and cross-system behavior. “Legacy intent” cards are explicitly conditional: first prove their
premise and owner, then either promote as a separate bounded package or mark them retired. This prevents the
2026-09-01 plan text from resurrecting already delivered or contradictory architecture.

### 01. canonical radiation exposure fact [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns canonical radiation exposure fact. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** One committed radiation event increments
SurvivorMutationProfile.cumulativeRadDose once for that survivor without changing the radiation owner
snapshot.

**Fresh campaign path.** For canonical radiation exposure fact, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For canonical radiation exposure fact, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For canonical radiation exposure fact, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For canonical radiation exposure fact, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For canonical radiation exposure fact, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For canonical radiation exposure fact, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For canonical radiation exposure fact, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For canonical radiation exposure fact, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 02. zero and negative exposure refusal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns zero and negative exposure refusal. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A zero or negative delta must not create a profile or instability; define
whether to reject it at source or receiving adapter.

**Fresh campaign path.** For zero and negative exposure refusal, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For zero and negative exposure refusal, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For zero and negative exposure refusal, Capture the relevant existing section after
the source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For zero and negative exposure refusal, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For zero and negative exposure refusal, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For zero and negative exposure refusal, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For zero and negative exposure refusal, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For zero and negative exposure refusal, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 03. duplicate exposure event [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns duplicate exposure event. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Redeliver the same event ID after restore and prove cumulativeRadDose,
instability, mutation count and journal entry remain unchanged.

**Fresh campaign path.** For duplicate exposure event, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For duplicate exposure event, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For duplicate exposure event, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For duplicate exposure event, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For duplicate exposure event, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For duplicate exposure event, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For duplicate exposure event, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For duplicate exposure event, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 04. cumulative dose versus current dose [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns cumulative dose versus current dose. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Compare current medical dose with lifetime genetic exposure before and
after RadAway; the two concepts must remain distinct.

**Fresh campaign path.** For cumulative dose versus current dose, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For cumulative dose versus current dose, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For cumulative dose versus current dose, Capture the relevant existing section after
the source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For cumulative dose versus current dose, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For cumulative dose versus current dose, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For cumulative dose versus current dose, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For cumulative dose versus current dose, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For cumulative dose versus current dose, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 05. instability spike threshold [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns instability spike threshold. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** At the actual code boundary dose > 20.0f, instability spikes by
(dose-20)*0.25; reconcile the contradictory 50 mSv comment before UI copy.

**Fresh campaign path.** For instability spike threshold, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For instability spike threshold, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For instability spike threshold, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For instability spike threshold, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For instability spike threshold, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For instability spike threshold, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For instability spike threshold, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For instability spike threshold, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 06. low-light adaptation node [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns low-light adaptation node. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Use mutation_low_light_adaptation and prove both catalog eligibility and
one concrete low-light consumer when authorized.

**Fresh campaign path.** For low-light adaptation node, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For low-light adaptation node, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For low-light adaptation node, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For low-light adaptation node, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For low-light adaptation node, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For low-light adaptation node, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For low-light adaptation node, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For low-light adaptation node, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 07. photophobia exclusive path [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns photophobia exclusive path. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Confirm mutation_photophobia exclusivity against the active ID set,
including a reverse exclusion check if catalog rules require symmetry.

**Fresh campaign path.** For photophobia exclusive path, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For photophobia exclusive path, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For photophobia exclusive path, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For photophobia exclusive path, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For photophobia exclusive path, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For photophobia exclusive path, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For photophobia exclusive path, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For photophobia exclusive path, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 08. keratinized skin node [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns keratinized skin node. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Acquire mutation_keratinized_skin only after its authored required_exposure
and parents are satisfied.

**Fresh campaign path.** For keratinized skin node, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For keratinized skin node, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For keratinized skin node, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For keratinized skin node, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For keratinized skin node, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For keratinized skin node, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For keratinized skin node, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For keratinized skin node, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 09. brittle skin exclusion [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns brittle skin exclusion. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** For mutation_brittle_skin, check exclusive_mutation_ids against the
acquired counterpart before the RNG pick.

**Fresh campaign path.** For brittle skin exclusion, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For brittle skin exclusion, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For brittle skin exclusion, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For brittle skin exclusion, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For brittle skin exclusion, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For brittle skin exclusion, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For brittle skin exclusion, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For brittle skin exclusion, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 10. dense bone and brittle bones [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns dense bone and brittle bones. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Dense-bone and brittle-bones branches cannot coexist if either authored
exclusivity relationship forbids the other.

**Fresh campaign path.** For dense bone and brittle bones, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For dense bone and brittle bones, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For dense bone and brittle bones, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For dense bone and brittle bones, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For dense bone and brittle bones, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For dense bone and brittle bones, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For dense bone and brittle bones, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For dense bone and brittle bones, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 11. toxin tolerance capability [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns toxin tolerance capability. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** mutation_toxin_tolerance must affect a named canonical toxin check or
remain a visible inert capability tag.

**Fresh campaign path.** For toxin tolerance capability, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For toxin tolerance capability, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For toxin tolerance capability, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For toxin tolerance capability, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For toxin tolerance capability, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For toxin tolerance capability, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For toxin tolerance capability, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For toxin tolerance capability, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 12. heightened reflex capability [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns heightened reflex capability. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** mutation_heightened_reflex must reach one existing action calculation
before a panel claims a combat advantage.

**Fresh campaign path.** For heightened reflex capability, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For heightened reflex capability, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For heightened reflex capability, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For heightened reflex capability, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For heightened reflex capability, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For heightened reflex capability, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For heightened reflex capability, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For heightened reflex capability, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 13. parent mutation eligibility [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns parent mutation eligibility. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Tiered nodes with parent_mutation_ids stay ineligible until every required
active node is present after restore.

**Fresh campaign path.** For parent mutation eligibility, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For parent mutation eligibility, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For parent mutation eligibility, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For parent mutation eligibility, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For parent mutation eligibility, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For parent mutation eligibility, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For parent mutation eligibility, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For parent mutation eligibility, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 14. one mutation per survivor-day [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns one mutation per survivor-day. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** TryMutateSurvivor returns false on the same survivor-day after a successful
mutation, including after save/reload.

**Fresh campaign path.** For one mutation per survivor-day, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For one mutation per survivor-day, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For one mutation per survivor-day, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For one mutation per survivor-day, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For one mutation per survivor-day, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For one mutation per survivor-day, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For one mutation per survivor-day, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For one mutation per survivor-day, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 15. stable eligible-node ordering [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns stable eligible-node ordering. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** With identical seed and catalog ordering, candidate selection and acquired
ID match even if registration order changes.

**Fresh campaign path.** For stable eligible-node ordering, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For stable eligible-node ordering, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For stable eligible-node ordering, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For stable eligible-node ordering, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For stable eligible-node ordering, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For stable eligible-node ordering, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For stable eligible-node ordering, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For stable eligible-node ordering, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 16. RadAway detox and chemical stress [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns RadAway detox and chemical stress. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** AdministerRadAway modifies the genetic profile under current Core
semantics; canonical medicine cost and dose change require a separate clinical transaction.

**Fresh campaign path.** For RadAway detox and chemical stress, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For RadAway detox and chemical stress, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For RadAway detox and chemical stress, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For RadAway detox and chemical stress, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For RadAway detox and chemical stress, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For RadAway detox and chemical stress, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For RadAway detox and chemical stress, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For RadAway detox and chemical stress, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 17. gene therapy vial settlement [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns gene therapy vial settlement. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** PerformGeneTherapy refuses without gene_therapy_retroviral_vial and
consumes exactly one vial on successful removal.

**Fresh campaign path.** For gene therapy vial settlement, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For gene therapy vial settlement, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For gene therapy vial settlement, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For gene therapy vial settlement, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For gene therapy vial settlement, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For gene therapy vial settlement, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For gene therapy vial settlement, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For gene therapy vial settlement, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 18. mutation stat modifier consumer [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns mutation stat modifier consumer. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** GetStatModifiers requires one verified downstream stat reader; compare the
consumer output before and after mutation removal.

**Fresh campaign path.** For mutation stat modifier consumer, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For mutation stat modifier consumer, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For mutation stat modifier consumer, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For mutation stat modifier consumer, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For mutation stat modifier consumer, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For mutation stat modifier consumer, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For mutation stat modifier consumer, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For mutation stat modifier consumer, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 19. mutation panel and audio event [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns mutation panel and audio event. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** OnMutationAcquired writes one journal fact and one audio event while
MutationTreePanel refreshes from saved profile IDs after reopen.

**Fresh campaign path.** For mutation panel and audio event, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For mutation panel and audio event, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For mutation panel and audio event, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For mutation panel and audio event, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For mutation panel and audio event, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For mutation panel and audio event, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For mutation panel and audio event, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For mutation panel and audio event, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 20. old-save mutation baseline [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns old-save mutation baseline. Start from
`Assets/Ashfall.Core/Medical/MutationSystem.cs` and `Assets/StreamingAssets/Data/mutations.json` and trace
any effect through `src/Main.Plans178_181.cs` to its current destination owner. Persistence must follow this
boundary: `mutation_tree section; radiation owner retains dose history`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A legacy campaign missing mutation_tree restores an empty profile without
backfilling mutations from current medical dose.

**Fresh campaign path.** For old-save mutation baseline, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For old-save mutation baseline, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For old-save mutation baseline, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For old-save mutation baseline, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old-save mutation baseline, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old-save mutation baseline, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old-save mutation baseline, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For old-save mutation baseline, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` if it
covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `MutationSystem.cs` in `Assets/Ashfall.Core/Radiation/`. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `MutationTrait` DTO: `traitId`, `traitName`, `traitType`
  (beneficial/harmful/neutral/mixed), `radiationThreshold` (cumulative dose required), `probability` (0-1,
  chance of manifesting at threshold), `effects` (list of modifiers: health, strength, intelligence,
  appearance, lifespan), `description`, `flavorText`. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `SurvivorMutation` DTO: `survivorId`, `traitId`, `manifestedDay`, `severity` (0-100),
  `progression` (stable/worsening/improving), `visible` bool (affects appearance), `inherited` bool (passed
  from parent). Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `MutationState` DTO: list of survivor mutations, list of manifested traits, mutation pool
  (available traits), mutation seed for determinism. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Define mutation categories:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define mutation mechanics:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define mutation inheritance:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define mutation visibility:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define mutation management:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define mutation effects on gameplay:. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Add deterministic seeding: mutation rolls use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Wire into `GameBootstrap`: `SetupMutations`, `TickMutations`, `SaveMutations`. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Create `MutationTraitCatalogLoader` for trait definitions. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement mutation UI: mutation panel showing survivor mutations. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement mutation threshold tracking:. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement trait manifestation:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement mutation progression:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement mutation inheritance:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement mutation visibility:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement mutation management:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement mutation social effects:. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Create mutation events:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Add mutation quest hooks:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Implement mutation UI:. Verify against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add mutation journal: automatic log of mutation events. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement mutation tutorial: first mutation explains system. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add mutation tooltips: hover over trait shows effects. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Create 20 mutation traits in data file (5 per category). Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Wire into `RadiationSystem`: cumulative dose triggers mutation checks. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Connect to `SurvivorLifecycle`: mutations affect lifespan and reproduction. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Integrate with `DiseaseSystem`: mutations interact with disease pipeline. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Connect to `NeedsSystem`: mutations affect health and morale. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Wire into `FactionBranchCoordinator`: visible mutations affect faction interactions. Verify
  against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Connect to `ResearchSystem`: gene therapy research unlock. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Implement old-save compatibility: existing saves get empty mutation state. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Add deterministic seeding: mutation rolls use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Create exploit prevention: mutations are permanent, management is slow. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Add tests: mutation manifestation, inheritance, progression, management, save round-trip. Verify
  against `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Verify catalog integrity: all trait IDs resolve. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Test edge cases: no mutations (low radiation), many mutations (high radiation). Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Verify headless behavior: mutations process correctly without UI. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Add data-integrity-selftest: mutation traits validate against survivor catalogs. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Create `--mutation-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Medical/MutationSystem.cs` and the destination owner; disposition must be DELIVERED,
  RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh
Ashfall.Core.Tests/Medical/Plan172RadiationMutationIntegrationTests.cs` plus targeted owner save/host checks
selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
