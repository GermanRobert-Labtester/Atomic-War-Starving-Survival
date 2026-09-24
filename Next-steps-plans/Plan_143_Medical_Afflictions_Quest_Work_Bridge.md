# Plan 143 — Medical Afflictions → Quest & Work Bridge

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Bind the catalog and canonical live affliction IDs to the existing duty assignment and quest availability
owners, then surface truthful reasons in the existing medical and work/quest panels.

**Current state:** PARTIAL: the stateless bridge and six-row rule catalog exist with Core tests; no host
reference to AfflictionQuestWorkBridge was found in src at this audit.

**Non-goals:** No new affliction store or bridge save section, no diagnosis from UI labels, no duplicate
duty roster, no event-based quest unlock without a real runtime mapping, no automatic dismissal of
assigned workers without an owner rule.

**First deliverable:** one vertical slice with a real producer fact, one canonical destination owner,
save/reload parity, and a truthful player readout. Later slices are separate claims and must repeat the
premise check.

## 2. Authority and evidence status

The implementation authority is `AfflictionQuestWorkBridge as pure projection over medical state`.
Verified entry points inspected for this revision:
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs`, `src/Host/MedicalHostSession.cs`,
`src/Main.Medical.cs`, `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`,
`src/UI/AfflictionsPanel.cs`, and `Assets/StreamingAssets/Data/affliction_bridge_rules.json`. The focused
test starting point is `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs`. Existing
save ownership is `medical + duty roster + quest runtime`. Paths are evidence pointers, not advance
claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: C2 medical
pipeline, C9 survivor capacity, C10 quests, C1 shelter labor, and C17 presentation; the factory requires
the source affliction owner and destination consumer to be named. Its Part II requires live premise
checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The relevant deep
maps and lane matrices guide coverage; they do not override newer code. The master compilation itself
warns against padding and stale repository assumptions. This plan therefore records concrete contracts
and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Verified or directly observed:** The bridge reads IDs and has no persistent state; medical and quest
  owners retain their existing save sections.
- **Verified or directly observed:** CalculateWorkModifiers multiplies speed and quality, unions
  exclusions, and floors both multipliers at 0.1.
- **Verified or directly observed:** CheckQuestGates currently emits events during a query; avoid
  repeated journal writes from panel refresh.
- **Verified or directly observed:** The rule catalog has six work modifiers and six quest gates;
  validate each ID against the live affliction vocabulary.
- **Verified or directly observed:** AfflictionDutyBridge already exists; prove ownership and composition
  before binding a second duty modifier.
- **Verified or directly observed:** Quest tags in the catalog need a real quest/runtime mapping; an
  unlock tag alone is not an available quest.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public
methods and save snapshots, and write a one-page premise note. If another live owner already mutates the
target concern, use it. If a required write API does not exist, return the proposed consumer hook to the
foreman as an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix


| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/affliction_bridge_rules.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Host/MedicalHostSession.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `medical + duty roster + quest runtime` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/AfflictionsPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Proposed pure-query use after verifying the canonical medical ID reader.
IReadOnlyList<string> activeIds = ReadCanonicalActiveAfflictionIds(survivorId);
WorkModifierResult work = bridge.CalculateWorkModifiers(activeIds);
if (work.IsDutyExcluded(dutyType))
    return DutyRefusal("affliction_excluded", work.ActiveAfflictionIds);
// A quest query must call CheckQuestGates only at the canonical availability seam.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent
must confirm names and signatures against the live tree immediately before coding. Keep
`Assets/Ashfall.Core/` free of Godot and Unity references. If an adapter needs a callback, bind it in the
current host lifetime, unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` and the registry entry for `medical +
duty roster + quest runtime`. Write down exact DTO version, constructor baseline, capture point, restore
point, and dirty flag. For a stateless bridge, persist only the source and destination authorities; do
not create a bridge section. If a new mutable field is genuinely required, version the existing owner DTO
and prove migration from the preceding schema. Never equate a panel cache with campaign state.
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

Inspect the current route to `src/UI/AfflictionsPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and
uncertainty where the game cannot know more. Refresh on authoritative state change and after restore;
unbind subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 9A. First integration slice: a medical ID blocks a real duty assignment


**Premise proved in source:** `AfflictionQuestWorkBridge` is stateless and `affliction_bridge_rules.json`
has six work rows and six quest gates. `src/` has no reference to this bridge. `DutyRosterSystem` already
exposes `EvaluateRoleFitness`, `PreviewRoleFitness`, `PreviewAssign`, `ExecuteAssign` and
`GetEffectiveWorkSpeed`. `MedicalPipelineCoordinator` owns affliction episodes, diagnosis and treatment.
`AfflictionDutyBridge` is another existing medical-to-duty adapter; the Phase 0 audit must decide how its
output composes with the quest/work bridge at the roster's single fitness seam.

**First slice:** Trace `affliction_broken_leg` from the canonical active medical episode set for a
survivor. Bind a pure query that maps only confirmed/current affliction IDs into
`CalculateWorkModifiers`; map `heavy_labour` or `expedition` to a real roster role through the existing
role catalog. Have `DutyRosterSystem.PreviewAssign` and `ExecuteAssign` return the same refusal reason.
The panel shows the medical cause and the roster blocker using one read model. When treatment resolves
the episode, eligibility changes on refresh with no bridge state to migrate.

**Quest slice:** `CheckQuestGates` emits `OnQuestBlocked`/`OnQuestUnlocked` while answering a query. Do
not call it from a frame update or panel draw until emission semantics are separated from a pure
availability query or guarded by the quest owner's durable transition. `QuestRuntimeCoordinator` stores
quest instances and lifecycle; a `quest_tag` in the affliction catalog is not itself a quest instance.
Resolve each `quest_tag` to an authored quest/template or record it as unreachable content. A blocked
quest remains visible with a reason if the canonical quest UI supports it; do not delete or fail the
quest merely because a survivor becomes ill.

**Acceptance:** Broken-leg and healthy control survivors produce different roster previews and assignment
results, with no second medical save section. Save/load of the medical episode and roster assignment
yields the same refusal. A recovered survivor regains eligibility. Two simultaneous afflictions multiply
work speed/quality once each, never duplicate on repeated host binding. Querying a quest panel twice
emits no duplicate event or journal record.

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
| `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Host/MedicalHostSession.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.Medical.cs` | READ / integrator MODIFY | composition root | high |
| `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/AfflictionsPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` for
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

### 01. broken leg [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns broken leg. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: affliction_work_modifiers:
{"affliction_id": "affliction_broken_leg", "description": "Broken leg severely limits movement. Heavy and
mobile duties are impossible.", "excluded_duty_types": ["heavy_labour", "expedition", "patrol",
"construction"], "work_quality_multiplier": 0.9, "work_speed_multiplier": 0.5} | affliction_quest_gates:
{"affliction_id": "affliction_broken_leg", "description": "Cannot undertake expeditions with a broken
leg.", "gate_type": "blocks", "quest_tag": "expedition_quest", "severity": "moderate"}

**Fresh campaign path.** For broken leg, start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. A path that only returns a
DTO without changing the named consumer remains incomplete. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Repeat and idempotency.** For broken leg, deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For broken leg, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For broken leg, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For broken leg, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For broken leg, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For broken leg, trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency
rather than writing state into the producer. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For broken leg, replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the
event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 02. radiation sickness [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns radiation sickness. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: affliction_work_modifiers:
{"affliction_id": "affliction_radiation_sickness", "description": "Radiation sickness impairs cognition
and causes nausea. Food safety duties prohibited.", "excluded_duty_types": ["food_handling",
"water_treatment", "childcare"], "work_quality_multiplier": 0.5, "work_speed_multiplier": 0.7} |
affliction_quest_gates: {"affliction_id": "affliction_radiation_sickness", "description": "Radiation
sickness opens the 'Find Anti-Rad Supplies' quest.", "gate_type": "unlocks", "quest_tag":
"find_anti_rad", "severity": "mild"}

**Fresh campaign path.** For radiation sickness, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For radiation sickness, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For radiation sickness, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For radiation sickness, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For radiation sickness, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For radiation sickness, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For radiation sickness, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For radiation sickness, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 03. combat trauma [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns combat trauma. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: affliction_work_modifiers:
{"affliction_id": "affliction_combat_trauma", "description": "Combat trauma prevents effective
performance in violent or threatening roles.", "excluded_duty_types": ["combat_duty", "guard_duty",
"weapons_training"], "work_quality_multiplier": 0.75, "work_speed_multiplier": 0.8} |
affliction_quest_gates: {"affliction_id": "affliction_combat_trauma", "description": "Combat trauma opens
the 'PTSD Support Group' shelter event.", "gate_type": "unlocks", "quest_tag": "ptsd_support",
"severity": "mild"}

**Fresh campaign path.** For combat trauma, start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. A path that only
returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Repeat and idempotency.** For combat trauma, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For combat trauma, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For combat trauma, replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For combat trauma, exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily.
The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For combat trauma, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For combat trauma, trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a
visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For combat trauma, replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 04. respiratory degeneration [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns respiratory degeneration. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: affliction_work_modifiers:
{"affliction_id": "affliction_respiratory_degeneration", "description": "Respiratory failure makes
outdoor and physical exertion dangerous.", "excluded_duty_types": ["outdoor_duty", "heavy_labour",
"expedition"], "work_quality_multiplier": 0.8, "work_speed_multiplier": 0.6} | affliction_quest_gates:
{"affliction_id": "affliction_respiratory_degeneration", "description": "Respiratory degeneration unlocks
the 'Air Filtration Upgrade' project.", "gate_type": "unlocks", "quest_tag": "air_filtration_upgrade",
"severity": "moderate"}

**Fresh campaign path.** For respiratory degeneration, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For respiratory degeneration, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For respiratory degeneration, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For respiratory degeneration, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For respiratory degeneration, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For respiratory degeneration, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For respiratory degeneration, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For respiratory degeneration, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 05. chemical dependency [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns chemical dependency. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: affliction_work_modifiers:
{"affliction_id": "affliction_chemical_dependency", "description": "Chemical dependency causes
unpredictable behaviour in high-trust roles.", "excluded_duty_types": ["medical_duty", "childcare",
"leadership"], "work_quality_multiplier": 0.7, "work_speed_multiplier": 0.75} | affliction_quest_gates:
{"affliction_id": "affliction_chemical_dependency", "description": "Chemical dependency opens the 'Detox
Programme' quest.", "gate_type": "unlocks", "quest_tag": "detox_program", "severity": "moderate"}

**Fresh campaign path.** For chemical dependency, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For chemical dependency, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For chemical dependency, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For chemical dependency, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For chemical dependency, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For chemical dependency, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For chemical dependency, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For chemical dependency, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 06. broken arm [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns broken arm. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: affliction_work_modifiers:
{"affliction_id": "affliction_broken_arm", "description": "Broken arm prohibits lifting, crafting, and
weapons handling.", "excluded_duty_types": ["heavy_labour", "weapons_training", "construction",
"crafting"], "work_quality_multiplier": 0.65, "work_speed_multiplier": 0.6} | affliction_quest_gates:
{"affliction_id": "affliction_broken_arm", "description": "Cannot participate in combat quests with a
broken arm.", "gate_type": "blocks", "quest_tag": "combat_quest", "severity": "moderate"}

**Fresh campaign path.** For broken arm, start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. A path that only returns a
DTO without changing the named consumer remains incomplete. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Repeat and idempotency.** For broken arm, deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For broken arm, capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For broken arm, replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For broken arm, exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For broken arm, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For broken arm, trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency
rather than writing state into the producer. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For broken arm, replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the
event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 07. heavy labour exclusion [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns heavy labour exclusion. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For heavy labour exclusion, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For heavy labour exclusion, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For heavy labour exclusion, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For heavy labour exclusion, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For heavy labour exclusion, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For heavy labour exclusion, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For heavy labour exclusion, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For heavy labour exclusion, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 08. food handling exclusion [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns food handling exclusion. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For food handling exclusion, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For food handling exclusion, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For food handling exclusion, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For food handling exclusion, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For food handling exclusion, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For food handling exclusion, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For food handling exclusion, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For food handling exclusion, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 09. guard duty exclusion [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns guard duty exclusion. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For guard duty exclusion, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For guard duty exclusion, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For guard duty exclusion, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For guard duty exclusion, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For guard duty exclusion, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For guard duty exclusion, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For guard duty exclusion, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For guard duty exclusion, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 10. expedition exclusion [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns expedition exclusion. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For expedition exclusion, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For expedition exclusion, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For expedition exclusion, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For expedition exclusion, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For expedition exclusion, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For expedition exclusion, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For expedition exclusion, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For expedition exclusion, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 11. crafting exclusion [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns crafting exclusion. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For crafting exclusion, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For crafting exclusion, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For crafting exclusion, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For crafting exclusion, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For crafting exclusion, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For crafting exclusion, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For crafting exclusion, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For crafting exclusion, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 12. medical duty exclusion [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns medical duty exclusion. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For medical duty exclusion, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For medical duty exclusion, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For medical duty exclusion, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For medical duty exclusion, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For medical duty exclusion, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For medical duty exclusion, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For medical duty exclusion, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For medical duty exclusion, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 13. expedition quest block [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns expedition quest block. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For expedition quest block, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For expedition quest block, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For expedition quest block, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For expedition quest block, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For expedition quest block, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For expedition quest block, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For expedition quest block, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For expedition quest block, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 14. combat quest block [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns combat quest block. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For combat quest block, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For combat quest block, deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For combat quest block, capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For combat quest block, replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For combat quest block, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For combat quest block, open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For combat quest block, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For combat quest block, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 15. find anti-rad unlock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns find anti-rad unlock. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For find anti-rad unlock, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For find anti-rad unlock, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For find anti-rad unlock, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For find anti-rad unlock, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For find anti-rad unlock, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For find anti-rad unlock, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For find anti-rad unlock, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For find anti-rad unlock, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 16. PTSD support unlock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns PTSD support unlock. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For PTSD support unlock, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For PTSD support unlock, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For PTSD support unlock, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For PTSD support unlock, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For PTSD support unlock, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For PTSD support unlock, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For PTSD support unlock, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For PTSD support unlock, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 17. detox programme unlock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns detox programme unlock. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For detox programme unlock, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For detox programme unlock, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For detox programme unlock, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For detox programme unlock, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For detox programme unlock, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For detox programme unlock, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For detox programme unlock, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For detox programme unlock, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 18. air filtration unlock [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns air filtration unlock. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For air filtration unlock, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For air filtration unlock, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For air filtration unlock, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For air filtration unlock, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For air filtration unlock, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For air filtration unlock, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For air filtration unlock, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For air filtration unlock, replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 19. multiple afflictions [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns multiple afflictions. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For multiple afflictions, start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. A path that
only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence should
name the actual method, stable ID, source day, destination state field or read model, and exact refusal
code if the operation is unavailable.

**Repeat and idempotency.** For multiple afflictions, deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Save and restore.** For multiple afflictions, capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For multiple afflictions, replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse
that one effect or show a specific unavailable reason. It must not invent a fallback faction, location,
survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For multiple afflictions, exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For multiple afflictions, open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For multiple afflictions, trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For multiple afflictions, replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

### 20. recovery restores eligibility [CATALOG OR CONTRACT CASE — VERIFY EFFECT]

**Source and ownership:** This card concerns recovery restores eligibility. Start from
`Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and
`Assets/StreamingAssets/Data/affliction_bridge_rules.json` and trace any effect through
`src/Host/MedicalHostSession.cs` to its current destination owner. The bridge is stateless: medical
episodes, roster assignments and quest instances remain in their respective existing save owners. Keep
the source fact distinct from the consumer mutation. Catalog evidence: No row matches this case directly;
verify the producer and authored reference before implementation.

**Fresh campaign path.** For recovery restores eligibility, start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. A
path that only returns a DTO without changing the named consumer remains incomplete. Acceptance evidence
should name the actual method, stable ID, source day, destination state field or read model, and exact
refusal code if the operation is unavailable.

**Repeat and idempotency.** For recovery restores eligibility, deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not
solve duplication by a process-local boolean that disappears on reload. Acceptance evidence should name
the actual method, stable ID, source day, destination state field or read model, and exact refusal code
if the operation is unavailable.

**Save and restore.** For recovery restores eligibility, capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to
prove the recovery path has an explicit pending/completed contract. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Invalid reference.** For recovery restores eligibility, replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime
must refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Day/order boundary.** For recovery restores eligibility, exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For recovery restores eligibility, open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For recovery restores eligibility, trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if
the operation is unavailable.

**Determinism and bounds.** For recovery restores eligibility, replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the
owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence;
do not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `AfflictionQuestBridge.cs` in `Assets/Ashfall.Core/Medical/`. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Create `AfflictionWorkBridge.cs` in `Assets/Ashfall.Core/Medical/`. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `AfflictionQuestGate` DTO: `afflictionId`, `questId`, `gateType`
  (blocks/unlocks/modifies), `severity` (mild/moderate/severe), `description`. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `AfflictionWorkModifier` DTO: `afflictionId`, `workSpeedMultiplier` (0.3-1.0),
  `workQualityMultiplier` (0.5-1.0), `excludedDuties` (list of duty types). Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `AfflictionBridgeState` DTO: list of active quest gates, list of active work modifiers.
  Verify against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define quest gate rules:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define work modifier rules:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Create `IAfflictionQuestSink` interface for quest systems to query affliction gates. Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Create `IAfflictionWorkSink` interface for `DutyRosterSystem` to query work modifiers. Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Implement quest gate checking: quest systems check if survivor's afflictions
  block/unlock/modify the quest. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Implement work modifier application: duty roster reads affliction modifiers and applies to
  work output. Verify against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add deterministic calculation: gates/modifiers are pure functions of affliction state (no
  RNG). Verify against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Wire into `GameBootstrap`: `SetupAfflictionBridges`, `SaveAfflictionBridges`. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement quest blocking:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement quest unlocking:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement quest modification:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement work speed modification:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement work quality modification:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement duty exclusions:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Create affliction-aware quest events:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Add affliction quest hooks:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Implement affliction recovery effects:. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Add UI: medical panel shows affliction effects on quests and work. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Create medical journal: automatic log of affliction impacts. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Implement medical tutorial: first affliction explains quest/work impacts. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Add medical tooltips: hover over affliction shows quest/work effects. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Create 15 affliction quest gates and 15 work modifiers in data files. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Wire into `MedicalPipelineCoordinator`: affliction changes trigger bridge updates. Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Connect to quest systems: quest availability checks affliction gates. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Integrate with `DutyRosterSystem`: work output applies affliction modifiers. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to `SomaticFlashbackSystem`: existing work penalty integrated. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Wire into `ExpeditionSystem`: expedition readiness checks afflictions. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Connect to `TacticalCombatSystem`: combat effectiveness checks afflictions. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Implement old-save compatibility: existing saves get empty bridge state. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Add deterministic calculation: gates/modifiers are pure functions of affliction state. Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Create exploit prevention: afflictions have natural progression, can't be reset. Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Add tests: quest gating, work modification, stacking, save round-trip. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Verify catalog integrity: all affliction/quest IDs resolve. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Test edge cases: no afflictions (no gates/modifiers), multiple afflictions (stacking). Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Verify headless behavior: bridges process correctly without UI. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Add data-integrity-selftest: affliction gates/modifiers validate against catalogs. Verify
  against `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Create `--affliction-bridges-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs` and the destination owner; disposition must
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
Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs` plus targeted owner save/host
checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement
the first accepted vertical slice. This plan does not itself alter production code.
