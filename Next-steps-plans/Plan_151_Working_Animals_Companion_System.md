# Plan 151 — Working Animals & Companion System

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Seal the remaining canonical command and presentation seams around the existing companion authority, with
one first slice for assignment/training/feeding/medical action and its visible result.

**Current state:** MOSTLY DELIVERED: five species, taming adoption, care, save, day tick, pack capacity,
guard, morale and kennel route exist; residual player commands, expedition lifecycle, veterinary
reachability and presentation truth need targeted review.

**Non-goals:** No second AnimalCompanionSystem, no new species authority, no invented dog/cat/horse
catalog, no parallel inventory or morale store, no duplicate animal identity, no UI-owned role bonus
math.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status

The implementation authority is `CompanionAnimalSystem instances over WildlifeEcosystemSystem
species/taming`. Verified entry points inspected for this revision:
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`, `src/Main.Companion.cs`,
`src/Main.FlagshipPanels.cs`, `src/Host/CompanionSaveStore.cs`, `src/UI/KennelPanel.cs`, and
`Assets/StreamingAssets/Data/companion_animals.json`. The focused test starting point is
`Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs`. Existing save ownership is
`companion_animals`. Paths are evidence pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: C14 wildlife and
ecology, C9 survivor morale, C5 expedition carrying, C15 defense, C2 veterinary care, and C17 kennel UI;
one wildlife species/taming owner and one companion instance owner. Its Part II requires live premise
checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The relevant deep
maps and lane matrices guide coverage; they do not override newer code. The master compilation itself
warns against padding and stale repository assumptions. This plan therefore records concrete contracts
and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Verified or directly observed:** Wildlife DomesticAnimalState.animal_id is the companion identity;
  adoption on old saves uses only real domestic records.
- **Verified or directly observed:** CompanionSaveStore and the companion_animals save section already
  exist.
- **Verified or directly observed:** Main.Companion binds canonical inventory food ports and a day-keyed
  sickness RNG fork.
- **Verified or directly observed:** Pack bonus is supplied lazily to the expedition owner; guard and
  morale outputs pass through their owners.
- **Verified or directly observed:** KennelPanel is a read-only status surface; investigate whether
  player commands have a reachable UI route.
- **Verified or directly observed:** The current catalog has five species including Ash Hound, Feral
  Goat, Cotton Hare, Rad-Dog, and Iron Crow.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public
methods and save snapshots, and write a one-page premise note. If another live owner already mutates the
target concern, use it. If a required write API does not exist, return the proposed consumer hook to the
foreman as an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix


| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/companion_animals.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.Companion.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `companion_animals` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/KennelPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Existing identity and capacity seams; host should expose real commands only.
var result = companions.RegisterCompanion(domestic.animal_id, domestic.species_id, day);
expeditions.PackCapacityProvider = survivorId =>
    companions.GetPackCapacityBonusForSurvivor(survivorId);
// Role/training/medical buttons call the existing CompanionAnimalSystem commands.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent
must confirm names and signatures against the live tree immediately before coding. Keep
`Assets/Ashfall.Core/` free of Godot and Unity references. If an adapter needs a callback, bind it in the
current host lifetime, unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `src/Host/CompanionSaveStore.cs` and the registry entry for `companion_animals`. Write down exact
DTO version, constructor baseline, capture point, restore point, and dirty flag. For a stateless bridge,
persist only the source and destination authorities; do not create a bridge section. If a new mutable
field is genuinely required, version the existing owner DTO and prove migration from the preceding
schema. Never equate a panel cache with campaign state.
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

Inspect the current route to `src/UI/KennelPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and
uncertainty where the game cannot know more. Refresh on authoritative state change and after restore;
unbind subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 9A. First integration slice: a real companion command and returned state


**Premise proved in source:** `CompanionAnimalSystem` already implements `RegisterCompanion`, `Assign`,
`Feed`, `TreatSickness`, `TickDay`, `CaptureState` and `RestoreState`. `Main.Companion` loads five
authored profiles, binds inventory food and wildlife knowledge, adopts real tamed animals on old saves,
routes pack capacity, ticks day-keyed sickness and exposes `CompanionTreat`. `CompanionSaveStore` and
`companion_animals` section already exist. `KennelPanel` presents status and its `companion_kennel` route
exists. The original Plan 151 request to create an animal system and ten new dog/cat/horse species is
stale and would collide with the current authority.

**First slice:** Add a reachable kennel action for assigning one live tamed animal to a compatible role
and handler through `CompanionAnimalSystem.Assign`. Preview eligibility from the same Core rules, return
the existing `CompanionAssignResult.ReasonCode`, and refresh the panel from `CompanionState`. The UI must
never set `role` or `assigned_survivor_id` directly. A rejected action leaves state, inventory and role
benefits unchanged. Confirm the current `Main.FlagshipPanels` route and close/back lifecycle before
editing shared panel-registration paths.

**Remaining route audit:** Check whether training, feeding and `CompanionTreat` have player command
callers. A command method existing in Main is not a usable player action. Check expedition
departure/return synchronization of `on_expedition`; the pack query must include only eligible, available
animals. Check that guard rating is consumed by the canonical defense calculation and that morale/grief
flows through NeedsSystem only once. Veterinary treatment must consume a real canonical item through the
medical/inventory owner or return a truthful refusal. Do not add breeding, offspring, rare mutations,
trading or extra species as part of this residual seal.

**Acceptance:** A tamed Feral Goat can be assigned Pack to a live handler, increases that handler's
expedition capacity by the current bounded Core query, remains assigned after save/load, and appears with
the same role in the kennel. An incompatible Cotton Hare Pack assignment fails with `role_incompatible`
or the current precise reason code and yields no cargo bonus. An absent/dead handler, starving animal,
and animal already away on expedition display honest blockers. A treatment action either consumes the
approved item and changes sickness once or refuses without loss.

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
| `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/companion_animals.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.Companion.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.FlagshipPanels.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/CompanionSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/KennelPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` for the first
contract check, then only the directly affected save and host targets identified by Phase 0. Run a Godot
headless probe only if the claimed change affects the runtime host path. A compile result cannot
establish live route reachability. Acceptance requires: (1) one real input; (2) one canonical consumer
effect; (3) one durable restore; (4) repeat delivery without duplicate effect; (5) visible correct state
and blocker; (6) no new authority.
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

### 01. ash hound guard [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns ash hound guard. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: companions: {"base_food_per_day": 2, "bond_rate": 9, "disease_resistance": 4, "display_name": "Ash Hound", "fallback_food_item_ids": ["raw_meat", "cooked_meat", "item_smoked_meat", "item_salted_meat"], "guard_rating": 45, "max_health": 80, "morale_support_bp": 250, "pack_capacity_kg": 0, "preferred_food_tags": ["meat", "raw_meat"], "role_tags": ["guard", "morale"], "species_id": "species_ash_hound", "tags": ["canine", "guard_breed", "nocturnal_senses"], "terrain_tags": ["open_waste", "forest_burn", "highway"], "trainability": 7}

**Fresh campaign path.** For ash hound guard, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For ash hound guard, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For ash hound guard, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For ash hound guard, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For ash hound guard, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For ash hound guard, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For ash hound guard, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For ash hound guard, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 02. feral goat pack [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns feral goat pack. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: companions: {"base_food_per_day": 3, "bond_rate": 6, "disease_resistance": 6, "display_name": "Feral Goat", "fallback_food_item_ids": ["crop_ash_grain", "item_grain_flour", "item_pickled_tubers"], "guard_rating": 5, "max_health": 70, "morale_support_bp": 120, "pack_capacity_kg": 25, "preferred_food_tags": ["forage", "grain"], "role_tags": ["pack", "morale"], "species_id": "species_feral_goat", "tags": ["hoofed", "load_bearing", "hardy"], "terrain_tags": ["highland", "forest_burn", "open_waste"], "trainability": 5}

**Fresh campaign path.** For feral goat pack, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For feral goat pack, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For feral goat pack, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For feral goat pack, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For feral goat pack, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For feral goat pack, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For feral goat pack, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For feral goat pack, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 03. cotton hare morale [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns cotton hare morale. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: companions: {"base_food_per_day": 1, "bond_rate": 10, "disease_resistance": 2, "display_name": "Cotton Hare", "fallback_food_item_ids": ["crop_leafy_green", "item_aeroponic_food_leaf"], "guard_rating": 0, "max_health": 30, "morale_support_bp": 300, "pack_capacity_kg": 0, "preferred_food_tags": ["forage", "greens"], "role_tags": ["morale"], "species_id": "species_cotton_hare", "tags": ["small", "quiet", "den_animal"], "terrain_tags": ["forest_burn", "lowland"], "trainability": 4}

**Fresh campaign path.** For cotton hare morale, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For cotton hare morale, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For cotton hare morale, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For cotton hare morale, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For cotton hare morale, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For cotton hare morale, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For cotton hare morale, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For cotton hare morale, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 04. rad-dog guard [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns rad-dog guard. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: companions: {"base_food_per_day": 1, "bond_rate": 4, "disease_resistance": 8, "display_name": "Rad-Dog", "fallback_food_item_ids": ["raw_meat", "cooked_meat"], "guard_rating": 65, "max_health": 95, "morale_support_bp": 80, "pack_capacity_kg": 15, "preferred_food_tags": ["meat", "scraps"], "role_tags": ["guard", "pack"], "species_id": "species_rad_dog", "tags": ["canine", "heavy_guard", "irradiated_blood"], "terrain_tags": ["open_waste", "highway"], "trainability": 3}

**Fresh campaign path.** For rad-dog guard, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For rad-dog guard, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For rad-dog guard, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For rad-dog guard, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For rad-dog guard, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For rad-dog guard, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For rad-dog guard, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For rad-dog guard, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 05. iron crow alert [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns iron crow alert. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: companions: {"base_food_per_day": 1, "bond_rate": 7, "disease_resistance": 5, "display_name": "Iron Crow", "fallback_food_item_ids": ["crop_ash_grain", "raw_meat"], "guard_rating": 30, "max_health": 35, "morale_support_bp": 200, "pack_capacity_kg": 0, "preferred_food_tags": ["grain", "meat"], "role_tags": ["guard", "morale"], "species_id": "species_iron_crow", "tags": ["avian", "alert_senses", "scout"], "terrain_tags": ["open_waste", "highland"], "trainability": 9}

**Fresh campaign path.** For iron crow alert, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For iron crow alert, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For iron crow alert, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For iron crow alert, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For iron crow alert, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For iron crow alert, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For iron crow alert, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For iron crow alert, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 06. tame adoption [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns tame adoption. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For tame adoption, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For tame adoption, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For tame adoption, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For tame adoption, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For tame adoption, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For tame adoption, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For tame adoption, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For tame adoption, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 07. old-save adoption [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns old-save adoption. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For old-save adoption, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For old-save adoption, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For old-save adoption, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For old-save adoption, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old-save adoption, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old-save adoption, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old-save adoption, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For old-save adoption, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 08. role assignment [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns role assignment. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For role assignment, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For role assignment, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For role assignment, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For role assignment, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For role assignment, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For role assignment, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For role assignment, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For role assignment, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 09. role reassignment [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns role reassignment. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For role reassignment, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For role reassignment, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For role reassignment, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For role reassignment, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For role reassignment, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For role reassignment, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For role reassignment, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For role reassignment, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 10. handler assignment [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns handler assignment. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For handler assignment, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For handler assignment, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For handler assignment, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For handler assignment, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For handler assignment, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For handler assignment, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For handler assignment, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For handler assignment, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 11. training progress [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns training progress. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For training progress, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For training progress, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For training progress, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For training progress, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For training progress, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For training progress, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For training progress, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For training progress, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 12. feeding preferred stock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns feeding preferred stock. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For feeding preferred stock, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For feeding preferred stock, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For feeding preferred stock, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For feeding preferred stock, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For feeding preferred stock, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For feeding preferred stock, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For feeding preferred stock, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For feeding preferred stock, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 13. feeding fallback stock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns feeding fallback stock. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For feeding fallback stock, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For feeding fallback stock, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For feeding fallback stock, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For feeding fallback stock, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For feeding fallback stock, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For feeding fallback stock, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For feeding fallback stock, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For feeding fallback stock, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 14. starvation [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns starvation. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For starvation, start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. A path that only returns a
DTO without changing the named consumer remains incomplete. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Repeat and idempotency.** For starvation, deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For starvation, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For starvation, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For starvation, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For starvation, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For starvation, trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency
rather than writing state into the producer. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For starvation, replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the
event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 15. veterinary sickness [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns veterinary sickness. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For veterinary sickness, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For veterinary sickness, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For veterinary sickness, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For veterinary sickness, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For veterinary sickness, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For veterinary sickness, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For veterinary sickness, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For veterinary sickness, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 16. medical treatment [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns medical treatment. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For medical treatment, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For medical treatment, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For medical treatment, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For medical treatment, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For medical treatment, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For medical treatment, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For medical treatment, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For medical treatment, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 17. expedition deployment [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns expedition deployment. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For expedition deployment, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For expedition deployment, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For expedition deployment, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For expedition deployment, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For expedition deployment, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For expedition deployment, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For expedition deployment, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For expedition deployment, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 18. guard warning [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns guard warning. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For guard warning, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For guard warning, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For guard warning, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For guard warning, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For guard warning, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For guard warning, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For guard warning, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For guard warning, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 19. morale support [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns morale support. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For morale support, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For morale support, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For morale support, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For morale support, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For morale support, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For morale support, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For morale support, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For morale support, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

### 20. companion death grief [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns companion death grief. Start from
`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and
`Assets/StreamingAssets/Data/companion_animals.json` and trace any effect through `src/Main.Companion.cs`
to its current destination owner. The persisted carrier is `companion_animals` unless the premise audit
proves that the source and destination already persist their own halves. Keep the source fact distinct
from the consumer mutation. Catalog evidence: No row matches this case directly; verify the producer and
authored reference before implementation.

**Fresh campaign path.** For companion death grief, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For companion death grief, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For companion death grief, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For companion death grief, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For companion death grief, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For companion death grief, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For companion death grief, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For companion death grief, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave
an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `AnimalCompanionSystem.cs` in `Assets/Ashfall.Core/Animals/`. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `AnimalCompanion` DTO: `id`, `species` (dog/cat/horse/pack_animal/bird), `name`,
  `ownerId` (survivor ID), `trainingLevel` (0-100), `bondStrength` (0-100), `health` (0-100), `tasks`
  (list of trained tasks), `age` (years), `temperament` (docile/aggressive/independent). Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `AnimalCompanionState` DTO: list of animal companions, list of wild animals in shelter
  vicinity. Verify against `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define animal species with distinct capabilities:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Define taming mechanics:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define training mechanics:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define bonding mechanics:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Add deterministic seeding: taming/training outcomes use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Wire into `GameBootstrap`: `SetupAnimalCompanions`, `TickAnimals`, `SaveAnimalCompanions`.
  Verify against `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Create `AnimalCompanionCatalogLoader` for species definitions. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Implement animal health/aging: animals can get sick, age, die. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add animal breeding: compatible animals can produce offspring. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Create UI hook: animal panel showing companions, tasks, bond strength. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement taming from trapping:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement training system:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement deployment:. Verify against `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement animal care:. Verify against `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement animal combat:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Create animal events:. Verify against `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Add animal quest hooks:. Verify against `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement animal inheritance:. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Add UI: animal panel showing all companions, tasks, health, bond. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Create animal journal: automatic log of animal events. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Implement animal tutorial: first taming explains system. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add animal tooltips: hover over animal shows stats and tasks. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Create 10 animal species definitions in data file. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Wire into `WildlifeTrappingSystem`: cage trap produces live animals. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Connect to `ExpeditionSystem`: animals accompany expeditions. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Integrate with `ShelterDefenseSystem` (Plan 138): guard dogs defend shelter. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Connect to `TacticalCombatSystem`: expedition dogs fight in combat. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Wire into `NeedsSystem`: animal care consumes food resources. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Connect to `MentalHealthCrisisSystem`: animal death causes grief. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Implement old-save compatibility: existing saves get empty animal state. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Add deterministic seeding: taming/training use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Create exploit prevention: animals have needs, can't be infinite. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Add tests: taming, training, deployment, combat, save round-trip. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Verify catalog integrity: all animal species IDs resolve. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Test edge cases: no animals (no companions), many animals (resource drain). Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Verify headless behavior: animals process correctly without UI. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Add data-integrity-selftest: animal definitions validate against catalogs. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Create `--animal-companions-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0
premise audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate
achievement/profile/standing/discovery/animal state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs` plus
targeted owner save/host checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement
the first accepted vertical slice. This plan does not itself alter production code.
