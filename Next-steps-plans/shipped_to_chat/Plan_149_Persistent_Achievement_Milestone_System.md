# Plan 149 — In-Campaign Achievement & Milestone System

## Current-evidence architecture decision (2026-09-24; supersedes stale instructions below)

This section is the operative integration architecture for **Achievements and milestones**. It is a plan, not an implementation claim. The baseline material below remains a scenario inventory; when it says a missing file is “new,” requests a separate registry, or assumes a host count, this current-evidence section controls. Current source was inspected on 2026-09-24. Recheck it at the start of a claimed implementation package because concurrent integration work may have moved the seams.

**Verified premise:** Core and a 16-row achievements.json catalog exist. The panel evaluates a roster snapshot when refreshed and includes a derived fallback; endgame creates another AchievementSystem for export. A future slice must establish one run-owned evaluation and restore path before deleting fallback or expanding conditions.

**Master authority mapping:** C16 progression, C13 endgame, C17 UI. The master expansion document `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` supplies the Part II premise sweep, evidence labels, anti-duplication firewall, and Lane A prose, Lane B mechanics, Lane D save, and Lane E player-surface questions. Its scene and content candidates are ideas, not evidence that a route currently works. The live source and AGENTS.md take precedence.

**Bounded outcome:** Establish one source fact or player command, one canonical consumer, one durable result, and one truthful player readout for the next unsealed gap. The first phase should close a single representative path. A later content batch, cross-system extension, or balancing pass is a separate path claim. No panel-owned unlock authority or duplicate cross-run store.

### Authority and custody map

| Concern | Custody | Required proof before a change |
|---|---|---|
| Domain rule | `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` | Inspect public API, mutation and capture/restore; amend it only for an observed gap. |
| Producer | `committed campaign facts from their domain owners` | Show that the event follows a committed action and carries stable IDs/day. |
| Host composition | `src/UI/AchievementsPanel.cs and src/Main.Endgame.cs; verify a campaign owner before moving evaluation` | Show setup, restore, bind, dirty/save, reset and lifecycle order in the current tree. |
| Destination effect | `run-local achievement completion and existing meta-progression export` | Call its existing command once; do not mirror its state in this plan. |
| Persistence | `AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner` | Save both source marker and effect custody; prove retry cannot duplicate. |
| Player readout | `AchievementsPanel` | Display provenance, current status, next command and refusal using existing state. |

### Phase gates

0. **Claim and recensus.** Read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, the current domain owner, the specific host route, save registration and catalog loader. Record the exact files a builder may edit. If a current active claim overlaps, wait for its handoff. If source contradicts this page, amend this page before coding.
1. **Domain fact.** Pick one case from the scenario inventory below. Specify input identity, accepted preconditions, typed result, mutation owner, idempotency key and failure code. A UI callback cannot decide domain legality. If an existing owner already supplies the effect, use it and retire the proposed duplicate.
2. **Catalog and prose.** Inspect the actual JSON row shape and consumer. Add only reachable rows with unique IDs and valid references. Write concise diegetic text for a real state and a separate refusal. A copy field is never a source of numeric gameplay rules. The master’s Lane A archetype may suggest tone, but the current loader sets field limits.
3. **Save and deterministic replay.** Identify the exact save DTO and section. Capture after the authoritative mutation, restore before event rebinding, and replay the same input. Use the campaign RNG fork only where the current owner already consumes it; no wall clock, hash-order sampling or process-local dedupe. Treat missing old fields as the documented baseline and reject malformed new values before they enter live state.
4. **Host composition.** Bind the producer to `run-local achievement completion and existing meta-progression export` in the existing host lifetime. Respect setup order, reset, dirty tracking, save orchestration, and unsubscription. If a consumer is optional, specify the withheld effect and a truthful unavailable reason; do not silently report success. Shared `Main` and registry files belong to the named integrator.
5. **Presentation.** Bind `AchievementsPanel` to a read model and command result. Render stable IDs as authored labels only after validating a lookup. Keep keyboard/controller focus, close/back, refresh and disposal correct; expose reasons in words, not color alone. Do not let opening a panel trigger a milestone, trade, or consequence.
6. **Focused acceptance and handoff.** Select the smallest directly relevant test file under `TEST_POLICY.md` when implementation is authorized. Prove fresh path, repeated input, save/restore boundary, invalid reference, headless host route if touched, and UI projection if touched. Record exact commands and results. This document edit does not run implementation tests.

### Code integration framework: current API anchors and proposed placement

**Verified call anchor:** `AchievementSystem.EvaluateRosterSnapshot` currently runs in `AchievementsPanel` and another system instance is constructed for endgame export. The next slice should move the mutation to one campaign owner.

The following C# fragment identifies an existing method and its intended position in the current host. It is a placement guide, not a new authority type or a copy-and-paste patch. Variables and refusal types come from the owning method. A builder must open that source file and reconcile the exact signature in Phase 0.

```csharp
// Proposed host order: committed campaign fact -> one run-owned AchievementSystem
// -> CaptureState -> panel.Bind(read-only result) -> endgame export.
achievementSystem.EvaluateRosterSnapshot(
    simDay, rosterCount, aliveCount, avgHealth, avgDose, avgMorale);
var completedIds = achievementSystem.GetCompletedAchievementIds();
// Do not call EvaluateRosterSnapshot from a panel refresh after the move.
```

**Source contract.** Accept a fact only after the owning command commits. Carry its stable ID, subject, campaign day and authored row ID through the host boundary. Distinguish a query from a mutation: `Evaluate` may mutate counters in some current Core systems, so call sites must be inspected instead of assuming the method name is pure. A panel asks for a read model; a player command asks the existing host session to validate and commit. Treat a missing subject or catalog row as a refusal before changing any destination owner.

**Destination contract.** The destination owner alone changes run-local achievement completion and existing meta-progression export. The host carries a typed fact or uses the owner’s established delegate; it does not copy the destination value into another mutable store. The implementation review must record method name, input ID and before/after destination state. If the destination lacks a safe command, record an authority decision rather than writing its fields directly. A projected outcome is useful only if it can be traced back to the original committed fact.

**Save and retry contract.** Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner after mutation and before reporting a durable result. Where two owners persist separately, the handoff must name save order and recovery for a crash between writes. A repeated fact ID after restore must be a no-op or resume one pending effect, depending on the current owner’s marker semantics. No process-local boolean can stand in for persisted applied identity. The old-save baseline should be documented with one fixture and a stable outcome; corruption should fail without partially mutating an unrelated section.

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

## Integration casebooks: producer, custody, presentation, and failure

These casebooks turn the earlier scenario names into reviewable implementation questions. They are acceptance design, not claims that every feature already exists or that every case needs one test method. Select one bounded case per implementation package and record evidence before promoting it.

### 001. first_week_survivor — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the first_week_survivor result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 002. two_week_endurance — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the two_week_endurance result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 003. month_of_ash — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the month_of_ash result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 004. no_casualties — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the no_casualties result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 005. low_exposure — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the low_exposure result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 006. healthy_cohort — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the healthy_cohort result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 007. perimeter_secured — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the perimeter_secured result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 008. iron_defense — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the iron_defense result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 009. harmonious_bunker — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the harmonious_bunker result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 010. conflict_arbitrator — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the conflict_arbitrator result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 011. wasteland_cartographer — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the wasteland_cartographer result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 012. deep_recon — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the deep_recon result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 013. thriving_caravan — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the thriving_caravan result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 014. resource_resilience — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the resource_resilience result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 015. beacon_of_hope — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the beacon_of_hope result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 016. dignity_preserved — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the dignity_preserved result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 017. old save baseline — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the old save baseline result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 018. once-only unlock — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the once-only unlock result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 019. panel read-only projection — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the panel read-only projection result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 020. cross-run handoff — Producer and temporal boundary

Write the time line from committed campaign facts from their domain owners to the cross-run handoff result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 021. first_week_survivor — Rule and destination handoff

For first_week_survivor, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 022. two_week_endurance — Rule and destination handoff

For two_week_endurance, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 023. month_of_ash — Rule and destination handoff

For month_of_ash, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 024. no_casualties — Rule and destination handoff

For no_casualties, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 025. low_exposure — Rule and destination handoff

For low_exposure, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 026. healthy_cohort — Rule and destination handoff

For healthy_cohort, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 027. perimeter_secured — Rule and destination handoff

For perimeter_secured, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 028. iron_defense — Rule and destination handoff

For iron_defense, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 029. harmonious_bunker — Rule and destination handoff

For harmonious_bunker, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 030. conflict_arbitrator — Rule and destination handoff

For conflict_arbitrator, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 031. wasteland_cartographer — Rule and destination handoff

For wasteland_cartographer, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 032. deep_recon — Rule and destination handoff

For deep_recon, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 033. thriving_caravan — Rule and destination handoff

For thriving_caravan, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 034. resource_resilience — Rule and destination handoff

For resource_resilience, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 035. beacon_of_hope — Rule and destination handoff

For beacon_of_hope, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 036. dignity_preserved — Rule and destination handoff

For dignity_preserved, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 037. old save baseline — Rule and destination handoff

For old save baseline, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 038. once-only unlock — Rule and destination handoff

For once-only unlock, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 039. panel read-only projection — Rule and destination handoff

For panel read-only projection, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 040. cross-run handoff — Rule and destination handoff

For cross-run handoff, let Assets/Ashfall.Core/Achievements/AchievementSystem.cs decide the rule and send only the completed fact to run-local achievement completion and existing meta-progression export. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 041. first_week_survivor — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the first_week_survivor command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 042. two_week_endurance — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the two_week_endurance command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 043. month_of_ash — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the month_of_ash command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 044. no_casualties — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the no_casualties command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 045. low_exposure — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the low_exposure command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 046. healthy_cohort — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the healthy_cohort command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 047. perimeter_secured — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the perimeter_secured command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 048. iron_defense — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the iron_defense command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 049. harmonious_bunker — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the harmonious_bunker command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 050. conflict_arbitrator — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the conflict_arbitrator command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 051. wasteland_cartographer — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the wasteland_cartographer command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 052. deep_recon — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the deep_recon command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 053. thriving_caravan — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the thriving_caravan command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 054. resource_resilience — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the resource_resilience command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 055. beacon_of_hope — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the beacon_of_hope command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 056. dignity_preserved — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the dignity_preserved command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 057. old save baseline — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the old save baseline command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 058. once-only unlock — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the once-only unlock command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 059. panel read-only projection — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the panel read-only projection command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 060. cross-run handoff — Persistence and replay

Capture AchievementState in the current run carrier, with cross-run export through the existing meta-progression owner immediately before the cross-run handoff command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 061. first_week_survivor — Catalog and prose contract

Inspect the current authored row relevant to first_week_survivor and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 062. two_week_endurance — Catalog and prose contract

Inspect the current authored row relevant to two_week_endurance and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 063. month_of_ash — Catalog and prose contract

Inspect the current authored row relevant to month_of_ash and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 064. no_casualties — Catalog and prose contract

Inspect the current authored row relevant to no_casualties and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 065. low_exposure — Catalog and prose contract

Inspect the current authored row relevant to low_exposure and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 066. healthy_cohort — Catalog and prose contract

Inspect the current authored row relevant to healthy_cohort and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 067. perimeter_secured — Catalog and prose contract

Inspect the current authored row relevant to perimeter_secured and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 068. iron_defense — Catalog and prose contract

Inspect the current authored row relevant to iron_defense and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 069. harmonious_bunker — Catalog and prose contract

Inspect the current authored row relevant to harmonious_bunker and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 070. conflict_arbitrator — Catalog and prose contract

Inspect the current authored row relevant to conflict_arbitrator and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 071. wasteland_cartographer — Catalog and prose contract

Inspect the current authored row relevant to wasteland_cartographer and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 072. deep_recon — Catalog and prose contract

Inspect the current authored row relevant to deep_recon and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 073. thriving_caravan — Catalog and prose contract

Inspect the current authored row relevant to thriving_caravan and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 074. resource_resilience — Catalog and prose contract

Inspect the current authored row relevant to resource_resilience and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 075. beacon_of_hope — Catalog and prose contract

Inspect the current authored row relevant to beacon_of_hope and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 076. dignity_preserved — Catalog and prose contract

Inspect the current authored row relevant to dignity_preserved and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 077. old save baseline — Catalog and prose contract

Inspect the current authored row relevant to old save baseline and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 078. once-only unlock — Catalog and prose contract

Inspect the current authored row relevant to once-only unlock and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 079. panel read-only projection — Catalog and prose contract

Inspect the current authored row relevant to panel read-only projection and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 080. cross-run handoff — Catalog and prose contract

Inspect the current authored row relevant to cross-run handoff and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 081. first_week_survivor — Player route and accessibility

Present first_week_survivor through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 082. two_week_endurance — Player route and accessibility

Present two_week_endurance through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 083. month_of_ash — Player route and accessibility

Present month_of_ash through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 084. no_casualties — Player route and accessibility

Present no_casualties through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 085. low_exposure — Player route and accessibility

Present low_exposure through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 086. healthy_cohort — Player route and accessibility

Present healthy_cohort through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 087. perimeter_secured — Player route and accessibility

Present perimeter_secured through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 088. iron_defense — Player route and accessibility

Present iron_defense through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 089. harmonious_bunker — Player route and accessibility

Present harmonious_bunker through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 090. conflict_arbitrator — Player route and accessibility

Present conflict_arbitrator through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 091. wasteland_cartographer — Player route and accessibility

Present wasteland_cartographer through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 092. deep_recon — Player route and accessibility

Present deep_recon through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 093. thriving_caravan — Player route and accessibility

Present thriving_caravan through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 094. resource_resilience — Player route and accessibility

Present resource_resilience through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 095. beacon_of_hope — Player route and accessibility

Present beacon_of_hope through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 096. dignity_preserved — Player route and accessibility

Present dignity_preserved through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 097. old save baseline — Player route and accessibility

Present old save baseline through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 098. once-only unlock — Player route and accessibility

Present once-only unlock through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 099. panel read-only projection — Player route and accessibility

Present panel read-only projection through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 100. cross-run handoff — Player route and accessibility

Present cross-run handoff through AchievementsPanel as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 101. first_week_survivor — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for first_week_survivor. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 102. two_week_endurance — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for two_week_endurance. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 103. month_of_ash — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for month_of_ash. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 104. no_casualties — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for no_casualties. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 105. low_exposure — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for low_exposure. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 106. healthy_cohort — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for healthy_cohort. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 107. perimeter_secured — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for perimeter_secured. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 108. iron_defense — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for iron_defense. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 109. harmonious_bunker — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for harmonious_bunker. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 110. conflict_arbitrator — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for conflict_arbitrator. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 111. wasteland_cartographer — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for wasteland_cartographer. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 112. deep_recon — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for deep_recon. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 113. thriving_caravan — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for thriving_caravan. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 114. resource_resilience — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for resource_resilience. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 115. beacon_of_hope — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for beacon_of_hope. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 116. dignity_preserved — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for dignity_preserved. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 117. old save baseline — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for old save baseline. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 118. once-only unlock — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for once-only unlock. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 119. panel read-only projection — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for panel read-only projection. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 120. cross-run handoff — Failure containment

Use an achievement unlocked by opening a panel rather than by a committed campaign fact as the negative fixture for cross-run handoff. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 121. first_week_survivor — Adjacent-owner collision

Trace each effect claimed by first_week_survivor through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 122. two_week_endurance — Adjacent-owner collision

Trace each effect claimed by two_week_endurance through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 123. month_of_ash — Adjacent-owner collision

Trace each effect claimed by month_of_ash through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 124. no_casualties — Adjacent-owner collision

Trace each effect claimed by no_casualties through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 125. low_exposure — Adjacent-owner collision

Trace each effect claimed by low_exposure through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 126. healthy_cohort — Adjacent-owner collision

Trace each effect claimed by healthy_cohort through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 127. perimeter_secured — Adjacent-owner collision

Trace each effect claimed by perimeter_secured through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 128. iron_defense — Adjacent-owner collision

Trace each effect claimed by iron_defense through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 129. harmonious_bunker — Adjacent-owner collision

Trace each effect claimed by harmonious_bunker through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 130. conflict_arbitrator — Adjacent-owner collision

Trace each effect claimed by conflict_arbitrator through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 131. wasteland_cartographer — Adjacent-owner collision

Trace each effect claimed by wasteland_cartographer through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 132. deep_recon — Adjacent-owner collision

Trace each effect claimed by deep_recon through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 133. thriving_caravan — Adjacent-owner collision

Trace each effect claimed by thriving_caravan through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 134. resource_resilience — Adjacent-owner collision

Trace each effect claimed by resource_resilience through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 135. beacon_of_hope — Adjacent-owner collision

Trace each effect claimed by beacon_of_hope through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 136. dignity_preserved — Adjacent-owner collision

Trace each effect claimed by dignity_preserved through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 137. old save baseline — Adjacent-owner collision

Trace each effect claimed by old save baseline through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 138. once-only unlock — Adjacent-owner collision

Trace each effect claimed by once-only unlock through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 139. panel read-only projection — Adjacent-owner collision

Trace each effect claimed by panel read-only projection through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 140. cross-run handoff — Adjacent-owner collision

Trace each effect claimed by cross-run handoff through the current map of owners. Ask whether the same condition already reaches run-local achievement completion and existing meta-progression export from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 141. first_week_survivor — Day order and deterministic boundary

Pin first_week_survivor to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 142. two_week_endurance — Day order and deterministic boundary

Pin two_week_endurance to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 143. month_of_ash — Day order and deterministic boundary

Pin month_of_ash to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 144. no_casualties — Day order and deterministic boundary

Pin no_casualties to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 145. low_exposure — Day order and deterministic boundary

Pin low_exposure to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 146. healthy_cohort — Day order and deterministic boundary

Pin healthy_cohort to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 147. perimeter_secured — Day order and deterministic boundary

Pin perimeter_secured to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 148. iron_defense — Day order and deterministic boundary

Pin iron_defense to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 149. harmonious_bunker — Day order and deterministic boundary

Pin harmonious_bunker to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 150. conflict_arbitrator — Day order and deterministic boundary

Pin conflict_arbitrator to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 151. wasteland_cartographer — Day order and deterministic boundary

Pin wasteland_cartographer to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 152. deep_recon — Day order and deterministic boundary

Pin deep_recon to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 153. thriving_caravan — Day order and deterministic boundary

Pin thriving_caravan to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 154. resource_resilience — Day order and deterministic boundary

Pin resource_resilience to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 155. beacon_of_hope — Day order and deterministic boundary

Pin beacon_of_hope to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 156. dignity_preserved — Day order and deterministic boundary

Pin dignity_preserved to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 157. old save baseline — Day order and deterministic boundary

Pin old save baseline to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 158. once-only unlock — Day order and deterministic boundary

Pin once-only unlock to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 159. panel read-only projection — Day order and deterministic boundary

Pin panel read-only projection to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 160. cross-run handoff — Day order and deterministic boundary

Pin cross-run handoff to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 161. first_week_survivor — Observation and diagnostics

A focused probe for first_week_survivor should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 162. two_week_endurance — Observation and diagnostics

A focused probe for two_week_endurance should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 163. month_of_ash — Observation and diagnostics

A focused probe for month_of_ash should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 164. no_casualties — Observation and diagnostics

A focused probe for no_casualties should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 165. low_exposure — Observation and diagnostics

A focused probe for low_exposure should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 166. healthy_cohort — Observation and diagnostics

A focused probe for healthy_cohort should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 167. perimeter_secured — Observation and diagnostics

A focused probe for perimeter_secured should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 168. iron_defense — Observation and diagnostics

A focused probe for iron_defense should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 169. harmonious_bunker — Observation and diagnostics

A focused probe for harmonious_bunker should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 170. conflict_arbitrator — Observation and diagnostics

A focused probe for conflict_arbitrator should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 171. wasteland_cartographer — Observation and diagnostics

A focused probe for wasteland_cartographer should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.

### 172. deep_recon — Observation and diagnostics

A focused probe for deep_recon should print or assert the source ID, subject ID, day, result code, destination owner value, save carrier, and the line the player sees. Avoid a broad suite whose green result could mask an unwired feature. Capture one successful path and one refusal in a small fixture. If the host route is touched, use a bounded headless probe and inspect the panel or CLI readout for truthful state. Treat a compile-only pass as structural evidence, not runtime completion.


## Polishing pass and architecture handoff

This revision received a second editorial pass after the architecture and casebooks were assembled. The pass normalizes headings and whitespace, treats the current evidence section as authoritative over older speculative instructions, preserves the older scenario inventory as conditional intent, removes the most consequential false present-tense claims, and checks that every implementation phase has an owner, a save rule, a player route, a failure path, and a focused proof. Casebook prose is deliberately phrased as review work where a consumer or route has not been verified. The live-source recensus remains mandatory before implementation, especially where another active claim is changing a host.

**Closeout:** Plan 149 now has a documented integration architecture and a bounded first-slice method. No production behavior has been changed by this document. Runtime completion requires the claimed implementation package, focused verification, and the handoff described above.
