# Plan 180 — Skill Certification & Tiers — Owner-Safe Progression
> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome
Define a read-only tier projection over current discipline XP first; promote a durable certification only
after one named capability gate and certification custody are approved.

**Current state:** PARTIAL/DECISION-GATED: 160 authored skills, action XP, one expert discipline,
dormancy/reactivation, a daily host tick, save through apprenticeship and SkillMatrixPanel are live. No
skill-certification system or catalog was found. The September premise that skills are merely bare floats
and invisible is stale.

**Non-goals:** No replacement XP ledger, skill tree, universal bonus multiplier, new role registry,
speculative exam RNG, certification save store, or mass-created certificate catalog before a real consumer
and owner are signed.

**First deliverable:** a current tier-threshold decision and read-only SkillMatrix projection over existing
XP. A durable credential requires a separately signed consumer, custody and save contract.

## 2. Authority and evidence status
The current or candidate domain authority is `SkillProgressionSystem owns action XP, earned/dormant skill
IDs and bonus synchronization; certification authority is undecided`. Source and adjacent paths inspected
for this revision (some are candidate consumers rather than active bindings):
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`, `src/Main.CampaignServices.cs`,
`src/Main.ShelterSocial.cs`, `src/Host/ApprenticeshipSaveStore.cs`, `src/UI/SkillMatrixPanel.cs`, and
`Assets/StreamingAssets/Data/skills.json`. The focused test starting point is
`Ashfall.Core.Tests/SkillProgressionSystemTests.cs`. Persistence boundary under audit: `apprenticeship
section carries skillProgression; no certification section exists`. Paths are evidence pointers, not advance
claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-16/C16
progression and DM-9/C9 survivor skills identify the current skills/apprenticeship owners. The master treats
active XP authority and content utilization as preconditions; tier labels alone are presentation. Its Part
II requires live premise checks, bounded subject scope, explicit evidence labels, and a duplication
firewall. The relevant deep maps and lane matrices guide coverage; they do not override newer code. The
master compilation itself warns against padding and stale repository assumptions. This plan therefore
records concrete contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall
- **Evidence or explicit premise:** SkillProgressionSystem has six discipline IDs, GetXp, OnXpGained, earned
  skill IDs, dormancy and reactivation; it is not a general 0-100 skill-level system.
- **Evidence or explicit premise:** Main.EnsureSharedSkillProgression loads skills.json and
  TickSharedSkillProgression drives daily dormancy for living survivors.
- **Evidence or explicit premise:** The skill progression state is restored and captured inside the
  apprenticeship section; no independent certification section was found.
- **Evidence or explicit premise:** SkillMatrixPanel already binds the shared progression instance and
  exposes current skill and practice information.
- **Evidence or explicit premise:** The historical plan requests eight certificates, six specializations,
  exams, roles and new bonuses without a canonical consumer or signed custody; those are conditional
  proposals.
- **Evidence or explicit premise:** A `trade_certifications` prose family in trade_texts.json is not a
  gameplay certification catalog.

### Skill authority and tier dossier

The old plan treats skill value as a 0–100 float, but current progression stores hidden action XP by one of
six disciplines and awards discrete skills from `skills.json`. Its one-expert-discipline constraint and
14-day dormancy are gameplay rules. A tier mapping must be defined in XP units, not copied from the old
0–20/20–40 ranges. The first slice can compute a read-only tier label from `GetXp`, show it in the already
bound SkillMatrixPanel, and prove that XP, skill ownership, dormancy and bonuses remain unchanged.

Certification is a separate claim. A qualification must answer what concrete command currently refuses
without it, who examines, how prerequisites are checked, whether a dormant skill suspends the credential,
and where its durable record lives. If the only outcome is a badge, keep it as a derived milestone rather
than a new mutable store. If a clinical, crafting or duty owner needs an actual gate, that owner must
specify the query contract and failure code. Any new state should be attached to an approved existing
survivor/skill save owner or receive an explicit section decision; the apprenticeship section already
carries the progression state.

The historical proposals for eight certificates, six specializations, universal multipliers, quests and
random exams are not current requirements. Each is a separate content/consumer package after the first
signed gate. Do not restore the retired Unity GameBootstrap path.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/skills.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.CampaignServices.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `apprenticeship section carries skillProgression; no certification section exists` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/SkillMatrixPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Proposed pure projection over the existing XP owner; thresholds require approval.
float xp = skillProgression.GetXp(survivorId, disciplineId);
SkillTierView tier = ProjectTier(disciplineId, xp, approvedThresholds);
// A durable certificate is a separate signed command only if an existing
// duty, clinical, crafting, or expedition owner needs that qualification.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration
Inspect `src/Host/ApprenticeshipSaveStore.cs` and the registry for the named concern; absence of a section
is a decision gate, not permission to invent one. Write down exact DTO version, constructor baseline,
capture point, restore point, and dirty flag. For a stateless bridge, persist only the source and
destination authorities; do not create a bridge section. If a new mutable field is genuinely required,
version the existing owner DTO and prove migration from the preceding schema. Never equate a panel cache
with campaign state.
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
Inspect the current route to `src/UI/SkillMatrixPanel.cs` and its bind/open/close methods. Expose only
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
| `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/skills.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.CampaignServices.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.ShelterSocial.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/ApprenticeshipSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/SkillMatrixPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback
Use `bash scripts/run_test.sh Ashfall.Core.Tests/SkillProgressionSystemTests.cs` for the first contract
check, then only the directly affected save and host targets identified by Phase 0. Run a Godot headless
probe only if the claimed change affects the runtime host path. A compile result cannot establish live route
reachability. Acceptance requires: (1) one real input; (2) one canonical consumer effect; (3) one durable
restore; (4) repeat delivery without duplicate effect; (5) visible correct state and blocker; (6) no new
authority.
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

### 01. medical discipline XP [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns medical discipline XP. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** GetXp(survivorId,"medical") must be the only XP source for a projected
medical tier; no copied per-panel value.

**Fresh campaign path.** For medical discipline XP, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For medical discipline XP, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For medical discipline XP, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For medical discipline XP, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For medical discipline XP, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For medical discipline XP, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For medical discipline XP, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For medical discipline XP, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 02. crafting discipline XP [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns crafting discipline XP. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** The crafting tier reads current action XP and never changes a
CraftingSystem result until that owner accepts a specific qualification gate.

**Fresh campaign path.** For crafting discipline XP, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For crafting discipline XP, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For crafting discipline XP, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For crafting discipline XP, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For crafting discipline XP, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For crafting discipline XP, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For crafting discipline XP, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For crafting discipline XP, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 03. science discipline XP [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns science discipline XP. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A science tier can be displayed from current XP, but research unlocks
remain with ResearchSystem.

**Fresh campaign path.** For science discipline XP, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For science discipline XP, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For science discipline XP, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For science discipline XP, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For science discipline XP, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For science discipline XP, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For science discipline XP, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For science discipline XP, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 04. combat discipline XP [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns combat discipline XP. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** Combat qualification requires a named TacticalCombat or duty consumer; the
tier projection alone confers no damage multiplier.

**Fresh campaign path.** For combat discipline XP, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For combat discipline XP, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For combat discipline XP, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For combat discipline XP, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For combat discipline XP, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For combat discipline XP, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For combat discipline XP, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For combat discipline XP, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 05. scavenging discipline XP [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns scavenging discipline XP. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** Scavenging XP is current authority; expedition capacity, discovery and loot
retain their existing owners.

**Fresh campaign path.** For scavenging discipline XP, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For scavenging discipline XP, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For scavenging discipline XP, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For scavenging discipline XP, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For scavenging discipline XP, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For scavenging discipline XP, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For scavenging discipline XP, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For scavenging discipline XP, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 06. survival discipline XP [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns survival discipline XP. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A survival tier uses the current discipline key and does not invent a
separate navigation score.

**Fresh campaign path.** For survival discipline XP, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For survival discipline XP, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For survival discipline XP, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For survival discipline XP, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For survival discipline XP, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For survival discipline XP, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For survival discipline XP, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For survival discipline XP, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 07. earned skill versus tier [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns earned skill versus tier. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A granted skill ID and a projected XP tier are separate facts; either may
change without silently changing the other.

**Fresh campaign path.** For earned skill versus tier, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For earned skill versus tier, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For earned skill versus tier, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For earned skill versus tier, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For earned skill versus tier, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For earned skill versus tier, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For earned skill versus tier, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For earned skill versus tier, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 08. dormant earned skill [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns dormant earned skill. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A dormant skill remains in saved progression state; certificate policy must
state whether the qualification remains valid during dormancy.

**Fresh campaign path.** For dormant earned skill, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For dormant earned skill, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For dormant earned skill, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For dormant earned skill, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For dormant earned skill, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For dormant earned skill, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For dormant earned skill, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For dormant earned skill, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 09. reactivation after practice [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns reactivation after practice. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A recorded action reactivates the existing skill through
SkillProgressionSystem; any display label refreshes from that source.

**Fresh campaign path.** For reactivation after practice, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For reactivation after practice, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For reactivation after practice, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For reactivation after practice, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For reactivation after practice, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For reactivation after practice, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For reactivation after practice, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For reactivation after practice, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 10. one expert discipline [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns one expert discipline. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** The existing one-expert-discipline rule must be preserved before proposing
multiple master certifications.

**Fresh campaign path.** For one expert discipline, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For one expert discipline, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For one expert discipline, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For one expert discipline, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For one expert discipline, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For one expert discipline, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For one expert discipline, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For one expert discipline, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 11. threshold boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns threshold boundary. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** At each approved threshold, evaluate XP just below, equal to and above; the
view changes once without a new mutation.

**Fresh campaign path.** For threshold boundary, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For threshold boundary, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For threshold boundary, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For threshold boundary, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For threshold boundary, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For threshold boundary, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For threshold boundary, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For threshold boundary, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 12. old-save XP baseline [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns old-save XP baseline. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** Restore an apprenticeship save with no certification fields and derive
tiers from restored XP without backfilling credentials.

**Fresh campaign path.** For old-save XP baseline, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For old-save XP baseline, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For old-save XP baseline, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For old-save XP baseline, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old-save XP baseline, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old-save XP baseline, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old-save XP baseline, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For old-save XP baseline, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 13. skill matrix view [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns skill matrix view. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** SkillMatrixPanel shows current XP, dormant status and any approved tier
label from one bound shared progression instance.

**Fresh campaign path.** For skill matrix view, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For skill matrix view, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For skill matrix view, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For skill matrix view, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For skill matrix view, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For skill matrix view, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For skill matrix view, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For skill matrix view, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 14. certification command decision [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns certification command decision. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A new certificate command needs a signed owner, exact ID, preconditions,
effect, save carrier and refusal vocabulary before coding.

**Fresh campaign path.** For certification command decision, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For certification command decision, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For certification command decision, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For certification command decision, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For certification command decision, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For certification command decision, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For certification command decision, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For certification command decision, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 15. clinical qualification consumer [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns clinical qualification consumer. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** Advanced treatment remains owned by MedicalPipelineCoordinator; any
certification gate must be queried there and not in UI.

**Fresh campaign path.** For clinical qualification consumer, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For clinical qualification consumer, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For clinical qualification consumer, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For clinical qualification consumer, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For clinical qualification consumer, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For clinical qualification consumer, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For clinical qualification consumer, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For clinical qualification consumer, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 16. craft qualification consumer [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns craft qualification consumer. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A recipe or batch gate belongs to its crafting/foundry owner and may read a
signed certificate, never a panel badge.

**Fresh campaign path.** For craft qualification consumer, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For craft qualification consumer, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For craft qualification consumer, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For craft qualification consumer, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For craft qualification consumer, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For craft qualification consumer, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For craft qualification consumer, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For craft qualification consumer, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 17. duty qualification consumer [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns duty qualification consumer. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** DutyRosterSystem decides assignment legality; a certificate query must give
a truthful refusal or eligibility reason.

**Fresh campaign path.** For duty qualification consumer, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For duty qualification consumer, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For duty qualification consumer, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For duty qualification consumer, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For duty qualification consumer, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For duty qualification consumer, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For duty qualification consumer, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For duty qualification consumer, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 18. exam randomness proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns exam randomness proposal. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** An exam is deferred until its venue, examiner, item cost, cooldown, seeded
RNG stream and persistence are authorized.

**Fresh campaign path.** For exam randomness proposal, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For exam randomness proposal, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For exam randomness proposal, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For exam randomness proposal, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For exam randomness proposal, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For exam randomness proposal, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For exam randomness proposal, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For exam randomness proposal, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 19. specialization proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns specialization proposal. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A specialization cannot mint a skill, bonus or role until the owning
consumer and prerequisite graph are validated.

**Fresh campaign path.** For specialization proposal, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For specialization proposal, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For specialization proposal, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For specialization proposal, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For specialization proposal, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For specialization proposal, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For specialization proposal, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For specialization proposal, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

### 20. certification catalog gate [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns certification catalog gate. Start from
`Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and `Assets/StreamingAssets/Data/skills.json` and
trace any effect through `src/Main.CampaignServices.cs` to its current destination owner. Persistence must
follow this boundary: `apprenticeship section carries skillProgression; no certification section exists`.
Verify the exact source and destination sections; a new section requires an ownership decision. Keep the
source fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this
integration case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A new skill_certifications.json needs ID/reference validation and a
reachable consumer; eight named rows are not an acceptance target.

**Fresh campaign path.** For certification catalog gate, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For certification catalog gate, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For certification catalog gate, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For certification catalog gate, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For certification catalog gate, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For certification catalog gate, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For certification catalog gate, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For certification catalog gate, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` if it covers this contract.
Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner behavior. Record
this card as passed, deferred with an owner, or stale with source evidence; do not leave an ambiguous
“implemented” label.

## 14. Legacy plan reconciliation register
The original 2026-09-01 task list is preserved as intent here in condensed form. Numbered items that are
already delivered or contradicted by signed authority must be marked DELIVERED or RETIRED. Each entry is a
premise question, never an instruction to create a duplicate class or save section. Current code and the
live ownership ledger decide whether it becomes a claim.
- **L01:** Create `SkillCertificationSystem.cs` in `Assets/Ashfall.Core/Survivors/`. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `SkillTier` DTO: `tierId`, `tierName` (novice/competent/proficient/expert/master),
  `skillLevel` (float threshold), `bonusModifier` (1.0-2.0), `unlockedCapabilities` (list), `description`.
  Verify against `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `SkillCertification` DTO: `certId`, `certName`
  (certified_medic/master_engineer/expert_tracker/etc.), `requiredSkill` (skill ID), `requiredTier` (tier
  ID), `requiredExperience` (list of experience requirements), `benefits` (list of unlocks),
  `examDifficulty` (0-100). Verify against `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `SurvivorCertification` DTO: `survivorId`, `certId`, `earnedDay`, `certifyingSurvivorId`
  (who administered exam), `benefits` (active unlocks). Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `SkillSpecialization` DTO: `specId`, `specName`
  (combat_medic/field_engineer/master_tracker/etc.), `parentSkill` (skill ID), `requiredCerts` (list of cert
  IDs), `uniqueAbilities` (list), `description`. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Define `CertificationState` DTO: list of survivor certifications, list of specializations earned,
  certification exam log, skill tier progress per survivor. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define skill tiers (5 levels per skill):. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define skill certifications:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define skill specializations:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define certification exams:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Define certification benefits:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add deterministic seeding: exams use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Wire into `GameBootstrap`: `SetupCertifications`, `TickCertifications`, `SaveCertifications`.
  Verify against `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Create `SkillCertificationCatalogLoader` for certification definitions. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement skill tier tracking:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement certification earning:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement certification benefits:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement specialization:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement certification exams:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement teaching/certifying others:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement skill decay interaction:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Create certification events:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Add certification quest hooks:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Implement certification UI:. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add certification journal: automatic log of certification events. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement certification tutorial: first tier explains system. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add certification tooltips: hover shows requirements and benefits. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Create 8 certification definitions + 6 specialization definitions. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Wire into `SkillProgressionSystem`: tiers based on skill levels. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Connect to `CraftingSystem`: certifications unlock recipes. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Integrate with `ExpeditionSystem`: certifications enable expedition roles. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Connect to `FactionBranchCoordinator`: certifications affect faction roles. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Wire into `DutyRosterSystem`: certifications enable duty assignments. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Connect to `EducationSystem` (Plan 154): certified teachers. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Implement old-save compatibility: existing survivors get tier assignments. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Add deterministic seeding: exams use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Create exploit prevention: exams require prerequisites and cooldown. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Add tests: tier progression, certification exams, specializations, benefits, save round-trip.
  Verify against `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Verify catalog integrity: all certification/specialization IDs resolve. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Test edge cases: no certifications (novice), many certifications (expert survivor). Verify
  against `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition
  must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Verify headless behavior: certifications process correctly without UI. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Add data-integrity-selftest: certifications validate against skill catalogs. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Create `--certification-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract
**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh Ashfall.Core.Tests/SkillProgressionSystemTests.cs` plus targeted
owner save/host checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
