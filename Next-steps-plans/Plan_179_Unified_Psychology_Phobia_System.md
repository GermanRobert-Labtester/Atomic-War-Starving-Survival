# Plan 179 — Psychology & Phobia — Collision Audit and Safe Integration

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Publish a current collision and provenance decision first; then, only if authorized, bind one read-only
psychological projection or one tightly scoped phobia effect to an existing mental-state owner and player
surface.

**Current state:** AUTHORITY BLOCKED: PsychologicalProfileSystem has six phobias, six coping mechanisms,
trauma/exposure/therapy mutations, and CaptureState, but no src reference or dedicated save section was
found. Live arc, mental-health, crisis, sanatorium, traits, and subterranean concerns overlap. The ledger
calls phobia growth STALE/RETIRED or DEFERRED; the numeric Plan 179 in Main refers to PrisonerSystem.

**Non-goals:** No second trauma ledger, duplicate resilience stat, autonomous phobia growth, new therapy
economy, phobia save section, repeated query-trigger events, or rewiring of the live prisoner Plan 179
because of numbering collision.

**First deliverable:** a signed authority/custody disposition and exact path claim. A read-only projection
may proceed after its owners agree; any mutable integration needs a separate approved vertical slice and
save/reload evidence.

## 2. Authority and evidence status

The current or candidate domain authority is `SurvivorMentalHealthSystem and PsychologicalArcSystem are live
mental-state owners; PsychologicalProfileSystem is an unhosted Core proposal with separate mutable state`.
Source and adjacent paths inspected for this revision (some are candidate consumers rather than active
bindings): `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs`,
`src/Host/PsychologyArcHostSession.cs`, `src/Main.CampaignOwners.cs`, `src/Host/PsychologyArcSaveStore.cs`,
`src/UI/PsychologyArcPanel.cs`, and `Assets/StreamingAssets/Data/psychology_profiles.json`. The focused test
starting point is `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs`. Persistence
boundary under audit: `psychological_arcs and survivor_mental_health already exist; profile custody
undecided`. Paths are evidence pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-9/C9 lists live
traits, trauma, mental arcs, therapies, guilt, crises and sanatorium; the master’s one-authority rule and
the repo’s retired phobia-growth finding make this a decision-first plan. Its Part II requires live premise
checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The relevant deep maps
and lane matrices guide coverage; they do not override newer code. The master compilation itself warns
against padding and stale repository assumptions. This plan therefore records concrete contracts and treats
older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Evidence or explicit premise:** PsychologicalProfileSystem.LoadCatalog reads psychology_profiles.json,
  and RecordTraumaEvent can add phobias using ISeededRng.
- **Evidence or explicit premise:** EvaluatePhobiaExposure mutates TotalPhobiasTriggered and emits events
  despite being named Evaluate; panel reads must not call it.
- **Evidence or explicit premise:** ConductTherapySession lowers severity and changes resilience
  independently of existing arc, crisis, therapy, and sanatorium pathways.
- **Evidence or explicit premise:** SaveSectionRegistry already has psychological_arcs,
  survivor_mental_health, and mental_health_crisis, but no psychological_profile section was found.
- **Evidence or explicit premise:** KNOWN_DEBT records the sleep narrative projection as read-only and
  explicitly says no profile/phobia ledger; phobia growth remains deferred to traits/catalog.
- **Evidence or explicit premise:** src/Main.Plans178_181.cs labels Plan 179 prisoner management; numeric
  label alone cannot identify this psychology plan’s runtime host.

### Psychology collision decision dossier

This plan must begin with a table of each mutable field in `PsychologicalProfileSystem` (`resilience`,
phobias, coping mechanisms, therapy count, trauma count) and the equivalent or related field in
`SurvivorMentalHealthSystem`, `PsychologicalArcSystem`, `MentalHealthCrisisSystem`, traits, and sanatorium.
For each field, name the authoritative source, whether the profile is a derived read model, and who may
mutate it. If two systems can independently represent “trauma severity” or “therapy complete,” the foreman
must sign one owner and a migration rule before wiring any host. The September plan text is not that
decision.

`EvaluatePhobiaExposure` currently increments `TotalPhobiasTriggered` and emits events. It is therefore a
command despite its query name; calling it from panel refresh would grow a counter and potentially apply
effects repeatedly. A safe projection should read only the catalog and existing owner snapshots. A signed
exposure command would need `(survivorId, contextId, day, exposureId)` identity, a durable once-only marker
in the chosen owner, and a destination effect with reversal or expiry.

`RecordTraumaEvent` can probabilistically develop a phobia using seeded RNG, but governance has marked
phobia growth deferred/stale. Do not route trauma events into it by default. If later promoted, start from
one canonical trauma source, one catalog `developed_from_traumas` relationship, a stable seed fork, old-save
migration, and a clear transition rule. No source may silently backfill historical phobias from current
stress values.

The first safe implementation outcome may be a purely read-only panel composition showing current mental
health, arc, crisis and treatment state with provenance. Any new phobia behavior is separately gated. A
player-facing label must distinguish an observed concern from a diagnosis and must never imply a therapy
action that cannot reach the current clinical owner.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/psychology_profiles.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Host/PsychologyArcHostSession.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `psychological_arcs and survivor_mental_health already exist; profile custody undecided` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/PsychologyArcPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Safe default: projection reads existing owner snapshots and returns a value.
PsychologyReadModel view = ProjectPsychology(mentalHealth, arcs, traits);
// An exposure query must not mutate counters or emit OnPhobiaTriggered.
// If a phobia effect is later signed, the destination owner receives one
// stable, context-keyed fact and owns its reversible consequence.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `src/Host/PsychologyArcSaveStore.cs` and the registry for the named concern; absence of a section is
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

Inspect the current route to `src/UI/PsychologyArcPanel.cs` and its bind/open/close methods. Expose only
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
| `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/psychology_profiles.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Host/PsychologyArcHostSession.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.CampaignOwners.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/PsychologyArcSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/PsychologyArcPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` for
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

### 01. claustrophobia in enclosed space [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns claustrophobia in enclosed space. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** phobia_claustrophobia requires a signed mapping from enclosed_space to a
real subterranean context; current trait-based claustrophobia remains owner.

**Fresh campaign path.** For claustrophobia in enclosed space, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For claustrophobia in enclosed space, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For claustrophobia in enclosed space, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For claustrophobia in enclosed space, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For claustrophobia in enclosed space, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For claustrophobia in enclosed space, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For claustrophobia in enclosed space, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For claustrophobia in enclosed space, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 02. nyctophobia in darkness [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns nyctophobia in darkness. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** phobia_nyctophobia cannot fire on panel refresh; darkness_exposure needs
one authoritative context event.

**Fresh campaign path.** For nyctophobia in darkness, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For nyctophobia in darkness, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For nyctophobia in darkness, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For nyctophobia in darkness, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For nyctophobia in darkness, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For nyctophobia in darkness, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For nyctophobia in darkness, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For nyctophobia in darkness, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 03. blood phobia near injury [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns blood phobia near injury. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** phobia_blood_phobia may observe a clinical injury fact but must not replace
crisis or trauma severity storage.

**Fresh campaign path.** For blood phobia near injury, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For blood phobia near injury, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For blood phobia near injury, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For blood phobia near injury, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For blood phobia near injury, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For blood phobia near injury, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For blood phobia near injury, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For blood phobia near injury, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 04. radiation phobia at detection [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns radiation phobia at detection. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** phobia_radiation_phobia must distinguish detection from actual dose and
avoid modifying the radiation ledger.

**Fresh campaign path.** For radiation phobia at detection, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For radiation phobia at detection, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For radiation phobia at detection, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For radiation phobia at detection, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For radiation phobia at detection, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For radiation phobia at detection, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For radiation phobia at detection, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For radiation phobia at detection, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 05. social phobia at gatherings [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns social phobia at gatherings. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** phobia_social_phobia needs a real group_gathering source; do not infer
exposure from a generic social screen.

**Fresh campaign path.** For social phobia at gatherings, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For social phobia at gatherings, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For social phobia at gatherings, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For social phobia at gatherings, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For social phobia at gatherings, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For social phobia at gatherings, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For social phobia at gatherings, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For social phobia at gatherings, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 06. thalassophobia near deep water [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns thalassophobia near deep water. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** phobia_thalassophobia uses deep_water_exposure only if a maritime action
emits a stable context fact.

**Fresh campaign path.** For thalassophobia near deep water, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For thalassophobia near deep water, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For thalassophobia near deep water, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For thalassophobia near deep water, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For thalassophobia near deep water, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For thalassophobia near deep water, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For thalassophobia near deep water, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For thalassophobia near deep water, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 07. meditation coping [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns meditation coping. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** cope_meditation must be owned by a real treatment/recreation command; a
catalog row alone is not a learned coping state.

**Fresh campaign path.** For meditation coping, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For meditation coping, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For meditation coping, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For meditation coping, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For meditation coping, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For meditation coping, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For meditation coping, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For meditation coping, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 08. exercise coping [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns exercise coping. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** cope_exercise must compose with the live ExerciseSystem rather than
awarding a second resilience bonus independently.

**Fresh campaign path.** For exercise coping, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For exercise coping, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For exercise coping, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For exercise coping, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For exercise coping, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For exercise coping, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For exercise coping, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For exercise coping, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 09. socializing coping [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns socializing coping. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** cope_socializing must read current relationships/morale and avoid
duplicating their persistent change.

**Fresh campaign path.** For socializing coping, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For socializing coping, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For socializing coping, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For socializing coping, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For socializing coping, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For socializing coping, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For socializing coping, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For socializing coping, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 10. creative-work coping [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns creative-work coping. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** cope_creative_work depends on an approved culture creation route; do not
reward an unhosted artwork record.

**Fresh campaign path.** For creative-work coping, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For creative-work coping, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For creative-work coping, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For creative-work coping, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For creative-work coping, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For creative-work coping, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For creative-work coping, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For creative-work coping, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 11. substance-use coping [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns substance-use coping. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** cope_substance_use requires the canonical dependency/consumable owner and a
signed side-effect rule.

**Fresh campaign path.** For substance-use coping, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For substance-use coping, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For substance-use coping, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For substance-use coping, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For substance-use coping, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For substance-use coping, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For substance-use coping, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For substance-use coping, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 12. denial coping [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns denial coping. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** cope_denial may be a narrative tag until a reversible clinical effect and
owner are named.

**Fresh campaign path.** For denial coping, Start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. The card passes only when the
named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For denial coping, Deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For denial coping, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For denial coping, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For denial coping, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For denial coping, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For denial coping, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For denial coping, Replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines the
allowed range. If this card needs randomness, fork from the existing campaign stream and make the event key
stable. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 13. canonical trauma provenance [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns canonical trauma provenance. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** One canonical trauma event must map to exactly one provenance ID;
autonomous RecordTraumaEvent growth remains deferred.

**Fresh campaign path.** For canonical trauma provenance, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For canonical trauma provenance, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For canonical trauma provenance, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For canonical trauma provenance, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For canonical trauma provenance, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For canonical trauma provenance, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For canonical trauma provenance, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For canonical trauma provenance, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 14. trait and phobia identity collision [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns trait and phobia identity collision. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** Trait-based claustrophobia and profile phobia cannot independently penalize
the same underground action.

**Fresh campaign path.** For trait and phobia identity collision, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For trait and phobia identity collision, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For trait and phobia identity collision, Capture the relevant existing section after
the source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For trait and phobia identity collision, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For trait and phobia identity collision, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For trait and phobia identity collision, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For trait and phobia identity collision, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For trait and phobia identity collision, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 15. therapy owner choice [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns therapy owner choice. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** ConductTherapySession must not run beside an existing arc/sanatorium
therapy for the same session without a signed owner decision.

**Fresh campaign path.** For therapy owner choice, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For therapy owner choice, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For therapy owner choice, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For therapy owner choice, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For therapy owner choice, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For therapy owner choice, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For therapy owner choice, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For therapy owner choice, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 16. resilience projection ownership [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns resilience projection ownership. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** GetProfileResilienceScore is a projection; decide whether the source
mental-health resilience exists before exposing another number.

**Fresh campaign path.** For resilience projection ownership, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For resilience projection ownership, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For resilience projection ownership, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For resilience projection ownership, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For resilience projection ownership, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For resilience projection ownership, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For resilience projection ownership, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For resilience projection ownership, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 17. subterranean context trigger [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns subterranean context trigger. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** A subterranean context must invoke a command at most once per action and
preserve result identity across reload if approved.

**Fresh campaign path.** For subterranean context trigger, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For subterranean context trigger, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For subterranean context trigger, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For subterranean context trigger, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For subterranean context trigger, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For subterranean context trigger, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For subterranean context trigger, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For subterranean context trigger, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 18. work or expedition effect [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns work or expedition effect. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** Any work/expedition restriction must be applied by duty or expedition
owner, with recovery/unmanaged state restoring eligibility.

**Fresh campaign path.** For work or expedition effect, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For work or expedition effect, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For work or expedition effect, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For work or expedition effect, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For work or expedition effect, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For work or expedition effect, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For work or expedition effect, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For work or expedition effect, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 19. psychology panel read-only view [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns psychology panel read-only view. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** PsychologyArcPanel remains read-only until it has a canonical provenance
model; calling EvaluatePhobiaExposure there is forbidden.

**Fresh campaign path.** For psychology panel read-only view, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For psychology panel read-only view, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For psychology panel read-only view, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For psychology panel read-only view, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For psychology panel read-only view, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For psychology panel read-only view, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For psychology panel read-only view, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For psychology panel read-only view, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

### 20. phobia growth retirement gate [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns phobia growth retirement gate. Start from
`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and
`Assets/StreamingAssets/Data/psychology_profiles.json` and trace any effect through
`src/Host/PsychologyArcHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `psychological_arcs and survivor_mental_health already exist; profile custody undecided`. Verify
the exact source and destination sections; a new section requires an ownership decision. Keep the source
fact distinct from the consumer mutation. Catalog evidence: No catalog row directly matches this integration
case; verify the current producer and destination API before implementation.

**Feature-specific acceptance:** KNOWN_DEBT marks phobia growth deferred/stale; promotion requires current
evidence, exact owner decision and foreman signature.

**Fresh campaign path.** For phobia growth retirement gate, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For phobia growth retirement gate, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For phobia growth retirement gate, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For phobia growth retirement gate, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For phobia growth retirement gate, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For phobia growth retirement gate, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For phobia growth retirement gate, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For phobia growth retirement gate, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` if
it covers this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or
cross-owner behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do
not leave an ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `PsychologicalProfileSystem.cs` in `Assets/Ashfall.Core/Psychology/`. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `PsychologicalProfile` DTO: `survivorId`, `personalityTraits` (list of trait modifiers
  from experiences), `phobias` (list of developed phobias), `copingMechanisms` (list of learned coping
  strategies), `resilience` (0-100, overall psychological resilience), `vulnerabilities` (list of trigger
  sensitivities), `recoveryArc` (current stage of recovery if in therapy). Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `Phobia` DTO: `phobiaId`, `phobiaName`
  (claustrophobia/acrophobia/nyctophobia/thalassophobia/social_phobia/blood_phobia/radiation_phobia),
  `triggerCondition` (what triggers it), `severity` (0-100), `effects` (list: work penalty, avoidance
  behavior, morale penalty), `developedFrom` (trauma event that caused it). Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `CopingMechanism` DTO: `mechanismId`, `mechanismName`
  (meditation/exercise/socializing/creative_work/substance_use/denial), `effectiveness` (0-100),
  `sideEffects` (list of negative effects), `learnedFrom` (therapy/experience/peer). Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `PsychologyState` DTO: list of survivor profiles, phobia development log, therapy sessions
  held, personality evolution history. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define phobia types:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define phobia development:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define phobia effects:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define coping mechanisms:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define personality evolution:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Define therapy system:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add deterministic seeding: psychology uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Wire into `GameBootstrap`: `SetupPsychology`, `TickPsychology`, `SavePsychology`. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Implement psychology UI: psychological profile panel per survivor. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement psychological profiles:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement phobia development:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement phobia management:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement coping mechanisms:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement personality evolution:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement therapy:. Verify against `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement cross-system integration:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Create psychology events:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Add psychology quest hooks:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Implement psychology UI:. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add psychology journal: automatic log of psychology events. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement psychology tutorial: first phobia explains system. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add psychology tooltips: hover over trait shows history. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Create 7 phobia definitions in data file. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Create 6 coping mechanism definitions. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Wire into all 6 trauma systems: report events to profile. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to `NeedsSystem`: psychology affects morale. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Integrate with `SurvivorRelationsSystem`: psychology affects relationships. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Connect to `SkillProgressionSystem`: therapy skill. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Wire into `DreamSystem` (Plan 177): dreams process trauma. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Connect to `AgingSystem` (Plan 176): resilience changes with age. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Implement old-save compatibility: existing survivors get default profiles. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Add deterministic seeding: psychology uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Create exploit prevention: therapy requires time and trust. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Add tests: profile integration, phobia development, coping, therapy, save round-trip. Verify
  against `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Verify catalog integrity: all phobia/coping IDs resolve. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Test edge cases: no trauma (healthy profile), extensive trauma (complex profile). Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Verify headless behavior: psychology processes correctly without UI. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Add data-integrity-selftest: phobias validate against trauma event catalogs. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L45:** Create `--psychology-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh
Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs` plus targeted owner save/host
checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
