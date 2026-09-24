# Plan 149 — In-Campaign Achievement & Milestone System

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Host one campaign achievement instance, feed it real facts at authoritative event/snapshot boundaries,
persist its run-local state, bind the panel to that same instance, and hand its completed IDs to Plan 175
at completion.

**Current state:** PARTIAL: Core and 16 achievement definitions are delivered, but the panel binds
without a live AchievementSystem and Main.Endgame creates a fresh instance with placeholder
health/dose/morale; a campaign save section was not found.

**Non-goals:** No profile rewards in AchievementSystem, no duplicate completion history, no panel-side
unlock evaluation, no default fake snapshot values, no cross-run progress in the campaign state.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status

The implementation authority is `AchievementSystem for run-local progress; CrossRunProfileStore for
profile rewards`. Verified entry points inspected for this revision:
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs`, `src/Main.Endgame.cs`, `src/Main.GameFlow.cs`,
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs`, `src/UI/AchievementsPanel.cs`, and
`Assets/StreamingAssets/Data/achievements.json`. The focused test starting point is
`Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs`. No campaign achievement save
section was found; `achievements` is a proposed section for the foreman/integrator to claim. Plan 175
owns the separate cross-run profile. Paths are evidence pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: C16 progression
and meta, C9 survivor facts, C13 epilogue, and C17 panels; the master factory distinguishes run-local
fact ownership from profile rewards and demands verified observable consumers. Its Part II requires live
premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The relevant
deep maps and lane matrices guide coverage; they do not override newer code. The master compilation
itself warns against padding and stale repository assumptions. This plan therefore records concrete
contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Verified or directly observed:** AchievementCatalog.LoadFromJson currently parses the catalog;
  validate row IDs and condition kinds before adding new definitions.
- **Verified or directly observed:** AchievementState holds completed IDs, progress facts, and emitted
  IDs; its capture/restore must be tied to the active campaign save.
- **Verified or directly observed:** AchievementsPanel.Bind accepts AchievementSystem, but current
  Main.GameFlow and Main.PlayerSurfaces pass only survivors and day.
- **Verified or directly observed:** AchievementsPanel.RefreshView calls EvaluateRosterSnapshot if bound;
  move mutation to an authoritative observation boundary.
- **Verified or directly observed:** Main.Endgame currently constructs a new AchievementSystem and
  supplies 100 health, 0 dose, and 50 morale placeholders; replace with the live instance and real facts.
- **Verified or directly observed:** Plan 175 remains the sole profile reward owner; export only
  completed IDs after its own idempotent completion record.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public
methods and save snapshots, and write a one-page premise note. If another live owner already mutates the
target concern, use it. If a required write API does not exist, return the proposed consumer hook to the
foreman as an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix


| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/achievements.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | proposed `src/Main.Achievements.cs`; existing `src/Main.Endgame.cs` | Wire one campaign instance and export its completed IDs on accepted completion. |
| campaign save | proposed `achievements` section | Integrator claims and registers this section, or records an explicit existing-aggregate decision. |
| player view | `src/UI/AchievementsPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Proposed lifecycle sketch; exact host/save APIs must be checked at claim time.
AchievementSystem achievements = new(catalog, campaignId, restoredState);
achievements.OnAchievementUnlockedSeam += (id, run) => PresentUnlock(id, run);
// Day owner submits canonical roster values once; panel calls read methods only.
// Completion owner passes achievements.GetCompletedAchievementIds() to Plan 175.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent
must confirm names and signatures against the live tree immediately before coding. Keep
`Assets/Ashfall.Core/` free of Godot and Unity references. If an adapter needs a callback, bind it in the
current host lifetime, unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `Assets/Ashfall.Core/Achievements/AchievementSystem.cs`; the live registry has no achievements
entry, so claim the proposed section or record an explicit existing-aggregate decision. Write down exact
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

Inspect the current route to `src/UI/AchievementsPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and
uncertainty where the game cannot know more. Refresh on authoritative state change and after restore;
unbind subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 9A. First integration slice: one live campaign achievement authority


**Premise proved in source:** `AchievementSystem` and `achievements.json` already exist; the catalog has
16 definitions. `AchievementsPanel.Bind` accepts an optional system, but both observed binding call sites
pass only survivor session and day. If the optional system were passed, `RefreshView` would call
`EvaluateRosterSnapshot` during rendering. `Main.Endgame` constructs a separate new achievement system
and supplies `100f`, `0f`, `50f` for health, dose and morale. A search of `SaveSectionRegistry` found no
achievement section. These are the gaps, regardless of the older Plan 149 ledger claim that save and
panel were already delivered.

**Proposed save contract for foreman/integrator claim:** Register a single `achievements` campaign
section in `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`; use the existing `AchievementState` schema
and the current SaveStoreHub/checksum pattern through a narrowly scoped
`src/Host/AchievementSaveStore.cs` or the established local naming convention. Add setup/capture/restore
in one `Main.Achievements.cs` partial and join it through `Main.SaveOrchestrator.cs` and
`Main.Application.cs` at the established lifecycle points. The section stores completed IDs, progress
facts and emitted IDs; the cross-run profile remains Plan 175's separate authority. If the foreman
chooses an existing campaign aggregate instead, record that decision and replace this file map before
implementation. There is no valid “existing achievements registry entry” to inspect today.

**Observation contract:** A single live `AchievementSystem` gets canonical campaign identity at setup and
restores before the panel binds. Submit `survive_days` once at the campaign day boundary; submit roster
health/dose/morale from actual survivor and radiation owners, with explicit empty-roster semantics. Hook
raids, disputes, routes, expedition distance, trades, reserves and moral outcomes only at their
authoritative committed events. Do not infer them from strings in the panel or use fabricated completion
values. The panel renders `Catalog.All`, `GetProgressFact` and `GetCompletedAchievementIds` without
calling `SetFact`, `IncrementFact`, `Unlock` or `EvaluateRosterSnapshot`.

**Catalog contract:** Validate unique IDs, supported `condition_type`, finite nonnegative thresholds and
category values. `AchievementCatalog.LoadFromJson` currently uses string scanning rather than a
structural JSON reader; the integration phase should use the repository's strict catalog pipeline or
harden this loader before treating malformed content as safe. Keep old catalog IDs stable.
`OnAchievementUnlockedSeam` fires once per run, and the emitted marker survives reload. Plan 175 receives
`GetCompletedAchievementIds()` only from the live run instance when the completion record is accepted,
with its own profile idempotency check.

**Acceptance:** Starting Day 6 then advancing to Day 7 unlocks `first_week_survivor` once;
opening/closing the panel never mutates progress. Save/reload shows the same completed ID and no second
toast. A real low-dose roster can unlock `low_exposure`; an unknown dose cannot be replaced with zero.
Endgame reads the same ID set shown in the panel. A legacy save without `achievements` starts from the
documented baseline and evaluates current facts only, without retroactively inventing combat/trade
events.

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
| `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/achievements.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.Endgame.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.GameFlow.cs` | READ / integrator MODIFY | composition root | high |
| `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/AchievementsPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` for
the first contract check, then only the directly affected save and host targets identified by Phase 0.
Run a Godot headless probe only if the claimed change affects the runtime host path. A compile result
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

### 01. first_week_survivor [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns first_week_survivor. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "survival", "condition_type": "survive_days", "description": "Survive until Day 7 in the
wasteland.", "epilogue_tag": "tenacious", "id": "first_week_survivor", "name": "First Week Survivor",
"target_threshold": 7.0}

**Fresh campaign path.** For first_week_survivor, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For first_week_survivor, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For first_week_survivor, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For first_week_survivor, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For first_week_survivor, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For first_week_survivor, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For first_week_survivor, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For first_week_survivor, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 02. two_week_endurance [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns two_week_endurance. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "survival", "condition_type": "survive_days", "description": "Survive until Day 14 under
persistent fallout pressure.", "epilogue_tag": "hardened", "id": "two_week_endurance", "name": "Two-Week
Endurance", "target_threshold": 14.0}

**Fresh campaign path.** For two_week_endurance, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For two_week_endurance, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For two_week_endurance, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For two_week_endurance, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For two_week_endurance, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For two_week_endurance, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For two_week_endurance, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For two_week_endurance, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 03. month_of_ash [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns month_of_ash. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "survival", "condition_type": "survive_days", "description": "Survive 30 days in the
holdfast against all odds.", "epilogue_tag": "veteran_shelter", "id": "month_of_ash", "name": "Month of
Ash", "target_threshold": 30.0}

**Fresh campaign path.** For month_of_ash, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For month_of_ash, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For month_of_ash, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For month_of_ash, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For month_of_ash, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For month_of_ash, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For month_of_ash, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For month_of_ash, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 04. no_casualties [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns no_casualties. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "survival", "condition_type": "all_survivors_alive", "description": "Maintain a full living
roster with zero casualties.", "epilogue_tag": "shepherd", "id": "no_casualties", "name": "No
Casualties", "target_threshold": 1.0}

**Fresh campaign path.** For no_casualties, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For no_casualties, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For no_casualties, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For no_casualties, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For no_casualties, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For no_casualties, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For no_casualties, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For no_casualties, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 05. low_exposure [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns low_exposure. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "survival", "condition_type": "max_avg_dose", "description": "Keep average radiation dose
across survivors below 20 mSv.", "epilogue_tag": "clean_blood", "id": "low_exposure", "name": "Low
Exposure", "target_threshold": 20.0}

**Fresh campaign path.** For low_exposure, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For low_exposure, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For low_exposure, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For low_exposure, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For low_exposure, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For low_exposure, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For low_exposure, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For low_exposure, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 06. healthy_cohort [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns healthy_cohort. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "survival", "condition_type": "min_avg_health", "description": "Maintain average survivor
physical health above 80 points.", "epilogue_tag": "robust_community", "id": "healthy_cohort", "name":
"Healthy Cohort", "target_threshold": 80.0}

**Fresh campaign path.** For healthy_cohort, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For healthy_cohort, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For healthy_cohort, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For healthy_cohort, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For healthy_cohort, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For healthy_cohort, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For healthy_cohort, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For healthy_cohort, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 07. perimeter_secured [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns perimeter_secured. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "combat", "condition_type": "raids_repelled", "description": "Successfully repel 3 perimeter
security threats or wasteland raids.", "epilogue_tag": "defenders", "id": "perimeter_secured", "name":
"Perimeter Secured", "target_threshold": 3.0}

**Fresh campaign path.** For perimeter_secured, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For perimeter_secured, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For perimeter_secured, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For perimeter_secured, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For perimeter_secured, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For perimeter_secured, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For perimeter_secured, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For perimeter_secured, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 08. iron_defense [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns iron_defense. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "combat", "condition_type": "flawless_defense", "description": "Win a tactical encounter
with zero survivor trauma or limb injuries.", "epilogue_tag": "impenetrable", "id": "iron_defense",
"name": "Iron Defense", "target_threshold": 1.0}

**Fresh campaign path.** For iron_defense, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For iron_defense, deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For iron_defense, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For iron_defense, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For iron_defense, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For iron_defense, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For iron_defense, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For iron_defense, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 09. harmonious_bunker [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns harmonious_bunker. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "social", "condition_type": "min_avg_morale", "description": "Raise and sustain average
shelter morale at or above 75 points.", "epilogue_tag": "solidarity", "id": "harmonious_bunker", "name":
"Harmonious Bunker", "target_threshold": 75.0}

**Fresh campaign path.** For harmonious_bunker, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For harmonious_bunker, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For harmonious_bunker, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For harmonious_bunker, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For harmonious_bunker, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For harmonious_bunker, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For harmonious_bunker, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For harmonious_bunker, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 10. conflict_arbitrator [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns conflict_arbitrator. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "social", "condition_type": "disputes_resolved", "description": "Mediate and peacefully
resolve 3 interpersonal or ideological disputes.", "epilogue_tag": "peacemaker", "id":
"conflict_arbitrator", "name": "Conflict Arbitrator", "target_threshold": 3.0}

**Fresh campaign path.** For conflict_arbitrator, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For conflict_arbitrator, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For conflict_arbitrator, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For conflict_arbitrator, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For conflict_arbitrator, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For conflict_arbitrator, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For conflict_arbitrator, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For conflict_arbitrator, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 11. wasteland_cartographer [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns wasteland_cartographer. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "exploration", "condition_type": "routes_mapped", "description": "Survey and map 5 distinct
route corridors across the irradiated wastes.", "epilogue_tag": "trailblazer", "id":
"wasteland_cartographer", "name": "Wasteland Cartographer", "target_threshold": 5.0}

**Fresh campaign path.** For wasteland_cartographer, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For wasteland_cartographer, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For wasteland_cartographer, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For wasteland_cartographer, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For wasteland_cartographer, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For wasteland_cartographer, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For wasteland_cartographer, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For wasteland_cartographer, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 12. deep_recon [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns deep_recon. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "exploration", "condition_type": "expedition_distance", "description": "Successfully
complete a long-range expedition beyond 100 kilometers.", "epilogue_tag": "far_voyager", "id":
"deep_recon", "name": "Deep Reconnaissance", "target_threshold": 100.0}

**Fresh campaign path.** For deep_recon, start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. A path that only returns a
DTO without changing the named consumer remains incomplete. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Repeat and idempotency.** For deep_recon, deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For deep_recon, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For deep_recon, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For deep_recon, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For deep_recon, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For deep_recon, trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency
rather than writing state into the producer. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For deep_recon, replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the
event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 13. thriving_caravan [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns thriving_caravan. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "economic", "condition_type": "trades_completed", "description": "Conduct 5 successful
barter trades with regional merchant caravans.", "epilogue_tag": "merchant_hub", "id":
"thriving_caravan", "name": "Honest Commerce", "target_threshold": 5.0}

**Fresh campaign path.** For thriving_caravan, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For thriving_caravan, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For thriving_caravan, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For thriving_caravan, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For thriving_caravan, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For thriving_caravan, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For thriving_caravan, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For thriving_caravan, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 14. resource_resilience [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns resource_resilience. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements:
{"category": "economic", "condition_type": "food_reserves", "description": "Accumulate a reserve of over
100 preserved rations in shelter storage.", "epilogue_tag": "abundant_pantry", "id":
"resource_resilience", "name": "Full Granary", "target_threshold": 100.0}

**Fresh campaign path.** For resource_resilience, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For resource_resilience, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For resource_resilience, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For resource_resilience, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For resource_resilience, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For resource_resilience, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For resource_resilience, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For resource_resilience, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 15. beacon_of_hope [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns beacon_of_hope. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements: {"category": "moral", "condition_type": "merciful_choices", "description": "Offer sanctuary, medical aid, or mercy in 3 critical moral dilemmas.", "epilogue_tag": "righteous_stand", "id": "beacon_of_hope", "name": "Beacon of Hope", "target_threshold": 3.0}

**Fresh campaign path.** For beacon_of_hope, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For beacon_of_hope, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For beacon_of_hope, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For beacon_of_hope, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For beacon_of_hope, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For beacon_of_hope, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For beacon_of_hope, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For beacon_of_hope, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 16. dignity_preserved [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns dignity_preserved. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: achievements: {"category": "moral", "condition_type": "memorials_erected", "description": "Honor fallen survivors with solemn memorialization and inheritance delivery.", "epilogue_tag": "unforgotten", "id": "dignity_preserved", "name": "Dignity Preserved", "target_threshold": 1.0}

**Fresh campaign path.** For dignity_preserved, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For dignity_preserved, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For dignity_preserved, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For dignity_preserved, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For dignity_preserved, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For dignity_preserved, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For dignity_preserved, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For dignity_preserved, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 17. old save baseline [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns old save baseline. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row
matches this case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For old save baseline, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For old save baseline, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For old save baseline, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For old save baseline, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old save baseline, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old save baseline, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old save baseline, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For old save baseline, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 18. once-only unlock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns once-only unlock. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row
matches this case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For once-only unlock, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For once-only unlock, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For once-only unlock, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For once-only unlock, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For once-only unlock, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For once-only unlock, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For once-only unlock, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For once-only unlock, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 19. panel read-only projection [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns panel read-only projection. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row
matches this case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For panel read-only projection, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For panel read-only projection, deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For panel read-only projection, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For panel read-only projection, replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime
must refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Day/order boundary.** For panel read-only projection, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For panel read-only projection, open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome,
and preserves close/back/focus behavior. A label that announces a benefit absent from the owning system
fails this card. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For panel read-only projection, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For panel read-only projection, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 20. cross-run handoff [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns cross-run handoff. Start from
`Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
`Assets/StreamingAssets/Data/achievements.json` and trace any effect through `src/Main.Endgame.cs` to its
current destination owner. The proposed run-local carrier is the `achievements` campaign section; Plan
175 owns the separate cross-run profile. Confirm the chosen section with the integrator before
implementation. Keep the source fact distinct from the consumer mutation. Catalog evidence: No row
matches this case directly; verify the producer and authored reference before implementation.

**Fresh campaign path.** For cross-run handoff, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For cross-run handoff, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For cross-run handoff, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For cross-run handoff, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For cross-run handoff, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For cross-run handoff, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For cross-run handoff, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For cross-run handoff, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and. Verify against
  `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `AchievementDefinition` with an id, localized display keys, category, conditions, and.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define run-local `AchievementState`: completed ids, in-progress facts, and emitted event ids.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Migrate every existing panel literal into data before adding new definitions. Verify against
  `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Load through the Core serializer; validate all referenced ids and condition kinds. Verify
  against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Evaluate pure conditions from explicit game events and registered state snapshots; never use.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Emit `AchievementUnlocked(id, campaignId)` exactly once per run. Verify against
  `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Persist the run-local state in the campaign save and restore it before the panel binds. Verify
  against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Make the panel render catalog definitions plus state only—no hardcoded achievement labels or.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Start with a balanced catalog across survival, combat, social, exploration, economic, and.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** At completion, provide the completed ids to Plan 175 through a read-only export; Plan 175.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Add tests for condition evaluation, once-only emission, save/load, panel projection, and.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add an achievements self-test that proves every panel entry is catalog-backed and every.
  Verify against `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0
premise audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate
achievement/profile/standing/discovery/animal state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh
Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs` plus targeted owner save/host
checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement
the first accepted vertical slice. This plan does not itself alter production code.
