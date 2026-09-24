# Plan 179 — Psychology & Phobia — Collision Audit and Safe Integration

## Current-evidence architecture decision (2026-09-24; supersedes stale instructions below)

This section is the operative integration architecture for **Psychology and phobia**. It is a plan, not an implementation claim. The baseline material below remains a scenario inventory; when it says a missing file is “new,” requests a separate registry, or assumes a host count, this current-evidence section controls. Current source was inspected on 2026-09-24. Recheck it at the start of a claimed implementation package because concurrent integration work may have moved the seams.

**Verified premise:** Core, host, Main commands, journal facts, a save section, and CLI selftest now exist under an ACTIVE integration claim. The next slice must audit collisions with CombatTraumaSystem, MentalHealthCrisisSystem, sleep and therapy owners before adding modifiers or UI.

**Master authority mapping:** C9 survivor interiority, C2 medical care, C17 UI. The master expansion document `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` supplies the Part II premise sweep, evidence labels, anti-duplication firewall, and Lane A prose, Lane B mechanics, Lane D save, and Lane E player-surface questions. Its scene and content candidates are ideas, not evidence that a route currently works. The live source and AGENTS.md take precedence.

**Bounded outcome:** Establish one source fact or player command, one canonical consumer, one durable result, and one truthful player readout for the next unsealed gap. The first phase should close a single representative path. A later content batch, cross-system extension, or balancing pass is a separate path claim. No second mental-health meter, sleep ledger, or clinical claim inferred from a game modifier.

### Authority and custody map

| Concern | Custody | Required proof before a change |
|---|---|---|
| Domain rule | `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs` | Inspect public API, mutation and capture/restore; amend it only for an observed gap. |
| Producer | `an owner-reported trauma or exposure fact with an explicit trigger condition` | Show that the event follows a committed action and carries stable IDs/day. |
| Host composition | `src/Host/PsychologicalProfileHostSession.cs and src/Main.PsychologicalProfiles.cs` | Show setup, restore, bind, dirty/save, reset and lifecycle order in the current tree. |
| Destination effect | `existing mental-health, trauma, sleep, therapy, and survivor owners` | Call its existing command once; do not mirror its state in this plan. |
| Persistence | `psychological_profiles section through PsychologicalProfileSaveStore` | Save both source marker and effect custody; prove retry cannot duplicate. |
| Player readout | `a proposed consentful profile/care view after owner collision audit` | Display provenance, current status, next command and refusal using existing state. |

### Phase gates

0. **Claim and recensus.** Read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, the current domain owner, the specific host route, save registration and catalog loader. Record the exact files a builder may edit. If a current active claim overlaps, wait for its handoff. If source contradicts this page, amend this page before coding.
1. **Domain fact.** Pick one case from the scenario inventory below. Specify input identity, accepted preconditions, typed result, mutation owner, idempotency key and failure code. A UI callback cannot decide domain legality. If an existing owner already supplies the effect, use it and retire the proposed duplicate.
2. **Catalog and prose.** Inspect the actual JSON row shape and consumer. Add only reachable rows with unique IDs and valid references. Write concise diegetic text for a real state and a separate refusal. A copy field is never a source of numeric gameplay rules. The master’s Lane A archetype may suggest tone, but the current loader sets field limits.
3. **Save and deterministic replay.** Identify the exact save DTO and section. Capture after the authoritative mutation, restore before event rebinding, and replay the same input. Use the campaign RNG fork only where the current owner already consumes it; no wall clock, hash-order sampling or process-local dedupe. Treat missing old fields as the documented baseline and reject malformed new values before they enter live state.
4. **Host composition.** Bind the producer to `existing mental-health, trauma, sleep, therapy, and survivor owners` in the existing host lifetime. Respect setup order, reset, dirty tracking, save orchestration, and unsubscription. If a consumer is optional, specify the withheld effect and a truthful unavailable reason; do not silently report success. Shared `Main` and registry files belong to the named integrator.
5. **Presentation.** Bind `a proposed consentful profile/care view after owner collision audit` to a read model and command result. Render stable IDs as authored labels only after validating a lookup. Keep keyboard/controller focus, close/back, refresh and disposal correct; expose reasons in words, not color alone. Do not let opening a panel trigger a milestone, trade, or consequence.
6. **Focused acceptance and handoff.** Select the smallest directly relevant test file under `TEST_POLICY.md` when implementation is authorized. Prove fresh path, repeated input, save/restore boundary, invalid reference, headless host route if touched, and UI projection if touched. Record exact commands and results. This document edit does not run implementation tests.

### Code integration framework: current API anchors and proposed placement

**Verified call anchor:** `Main.RecordSurvivorTrauma`, `EvaluateSurvivorPhobiaTriggers`, coping and therapy commands already call `PsychologicalProfileHostSession`; profile state has a registered section.

The following C# fragment identifies an existing method and its intended position in the current host. It is a placement guide, not a new authority type or a copy-and-paste patch. Variables and refusal types come from the owning method. A builder must open that source file and reconcile the exact signature in Phase 0.

```csharp
var session = EnsurePsychologicalProfiles();
var rng = _campaignDay?.Rng.Fork("psychology_trauma") ?? new SeededRng(179);
bool changed = session.RecordTraumaEvent(
    survivorId, eventType, severity, _simDay, rng);
// EvaluatePhobiaExposure can mutate counters: call from a real exposure fact,
// never from panel refresh. Audit other mental-health owners before modifiers.
```

**Source contract.** Accept a fact only after the owning command commits. Carry its stable ID, subject, campaign day and authored row ID through the host boundary. Distinguish a query from a mutation: `Evaluate` may mutate counters in some current Core systems, so call sites must be inspected instead of assuming the method name is pure. A panel asks for a read model; a player command asks the existing host session to validate and commit. Treat a missing subject or catalog row as a refusal before changing any destination owner.

**Destination contract.** The destination owner alone changes existing mental-health, trauma, sleep, therapy, and survivor owners. The host carries a typed fact or uses the owner’s established delegate; it does not copy the destination value into another mutable store. The implementation review must record method name, input ID and before/after destination state. If the destination lacks a safe command, record an authority decision rather than writing its fields directly. A projected outcome is useful only if it can be traced back to the original committed fact.

**Save and retry contract.** Capture psychological_profiles section through PsychologicalProfileSaveStore after mutation and before reporting a durable result. Where two owners persist separately, the handoff must name save order and recovery for a crash between writes. A repeated fact ID after restore must be a no-op or resume one pending effect, depending on the current owner’s marker semantics. No process-local boolean can stand in for persisted applied identity. The old-save baseline should be documented with one fixture and a stable outcome; corruption should fail without partially mutating an unrelated section.

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

Publish a current collision and provenance decision first; then, only if authorized, bind one read-only
psychological projection or one tightly scoped phobia effect to an existing mental-state owner and player
surface.

**Earlier audit state (superseded):** PsychologicalProfileSystem has six phobias, six coping mechanisms,
trauma/exposure/therapy mutations, and CaptureState, and the current tree now contains Main/Host references and a registered profile save section. Live arc, mental-health, crisis, sanatorium, traits, and subterranean concerns overlap. The ledger
calls phobia growth STALE/RETIRED or DEFERRED; the numeric Plan 179 in Main refers to PrisonerSystem.

**Non-goals:** No second trauma ledger, duplicate resilience stat, autonomous phobia growth, new therapy
economy, phobia save section, repeated query-trigger events, or rewiring of the live prisoner Plan 179
because of numbering collision.

**First deliverable:** a signed authority/custody disposition and exact path claim. A read-only projection
may proceed after its owners agree; any mutable integration needs a separate approved vertical slice and
save/reload evidence.

## 2. Authority and evidence status

The current or candidate domain authority is `SurvivorMentalHealthSystem and PsychologicalArcSystem remain adjacent mental-state owners; PsychologicalProfileSystem now has a host and its own registered profile section`.
Source and adjacent paths inspected for this revision (some are candidate consumers rather than active
bindings): `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs`,
`src/Host/PsychologyArcHostSession.cs`, `src/Main.CampaignOwners.cs`, `src/Host/PsychologyArcSaveStore.cs`,
`src/UI/PsychologyArcPanel.cs`, and `Assets/StreamingAssets/Data/psychology_profiles.json`. The focused test
starting point is `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs`. Persistence
boundary under audit: `psychological_arcs, survivor_mental_health, and psychological_profiles are registered; effect ownership remains under collision audit`. Paths are evidence pointers, not advance claims.
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
  survivor_mental_health, and mental_health_crisis, and the new psychological_profiles section is now registered.
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

## Integration casebooks: producer, custody, presentation, and failure

These casebooks turn the earlier scenario names into reviewable implementation questions. They are acceptance design, not claims that every feature already exists or that every case needs one test method. Select one bounded case per implementation package and record evidence before promoting it.

### 001. claustrophobia in enclosed space — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the claustrophobia in enclosed space result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 002. nyctophobia in darkness — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the nyctophobia in darkness result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 003. blood phobia near injury — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the blood phobia near injury result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 004. radiation phobia at detection — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the radiation phobia at detection result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 005. social phobia at gatherings — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the social phobia at gatherings result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 006. thalassophobia near deep water — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the thalassophobia near deep water result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 007. meditation coping — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the meditation coping result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 008. exercise coping — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the exercise coping result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 009. socializing coping — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the socializing coping result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 010. creative-work coping — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the creative-work coping result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 011. substance-use coping — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the substance-use coping result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 012. denial coping — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the denial coping result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 013. canonical trauma provenance — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the canonical trauma provenance result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 014. trait and phobia identity collision — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the trait and phobia identity collision result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 015. therapy owner choice — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the therapy owner choice result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 016. resilience projection ownership — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the resilience projection ownership result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 017. subterranean context trigger — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the subterranean context trigger result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 018. work or expedition effect — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the work or expedition effect result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 019. psychology panel read-only view — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the psychology panel read-only view result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 020. phobia growth retirement gate — Producer and temporal boundary

Write the time line from an owner-reported trauma or exposure fact with an explicit trigger condition to the phobia growth retirement gate result. Identify the exact committed event that qualifies, the campaign day at which it becomes durable, and the source ID that survives a host restart. A preview, panel refresh, catalog lookup, or forecast must leave state untouched. The implementation note should name the owner that emits the fact and the smallest concrete input fixture. In review, ask whether the same fact can be emitted from a second route; if so, converge the two routes at the existing owner before exposing either to players.

### 021. claustrophobia in enclosed space — Rule and destination handoff

For claustrophobia in enclosed space, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 022. nyctophobia in darkness — Rule and destination handoff

For nyctophobia in darkness, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 023. blood phobia near injury — Rule and destination handoff

For blood phobia near injury, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 024. radiation phobia at detection — Rule and destination handoff

For radiation phobia at detection, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 025. social phobia at gatherings — Rule and destination handoff

For social phobia at gatherings, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 026. thalassophobia near deep water — Rule and destination handoff

For thalassophobia near deep water, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 027. meditation coping — Rule and destination handoff

For meditation coping, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 028. exercise coping — Rule and destination handoff

For exercise coping, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 029. socializing coping — Rule and destination handoff

For socializing coping, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 030. creative-work coping — Rule and destination handoff

For creative-work coping, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 031. substance-use coping — Rule and destination handoff

For substance-use coping, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 032. denial coping — Rule and destination handoff

For denial coping, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 033. canonical trauma provenance — Rule and destination handoff

For canonical trauma provenance, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 034. trait and phobia identity collision — Rule and destination handoff

For trait and phobia identity collision, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 035. therapy owner choice — Rule and destination handoff

For therapy owner choice, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 036. resilience projection ownership — Rule and destination handoff

For resilience projection ownership, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 037. subterranean context trigger — Rule and destination handoff

For subterranean context trigger, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 038. work or expedition effect — Rule and destination handoff

For work or expedition effect, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 039. psychology panel read-only view — Rule and destination handoff

For psychology panel read-only view, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 040. phobia growth retirement gate — Rule and destination handoff

For phobia growth retirement gate, let Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs decide the rule and send only the completed fact to existing mental-health, trauma, sleep, therapy, and survivor owners. Record the legality check, requested target, expected refusal, and the exact destination command that is allowed to mutate state. A callback that merely prints success is insufficient. If the destination API is absent, treat that absence as a design gate; do not construct a local counter, shadow ledger, or direct field mutation. The first implementation slice should make one externally visible destination change and show its provenance.

### 041. claustrophobia in enclosed space — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the claustrophobia in enclosed space command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 042. nyctophobia in darkness — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the nyctophobia in darkness command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 043. blood phobia near injury — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the blood phobia near injury command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 044. radiation phobia at detection — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the radiation phobia at detection command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 045. social phobia at gatherings — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the social phobia at gatherings command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 046. thalassophobia near deep water — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the thalassophobia near deep water command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 047. meditation coping — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the meditation coping command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 048. exercise coping — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the exercise coping command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 049. socializing coping — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the socializing coping command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 050. creative-work coping — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the creative-work coping command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 051. substance-use coping — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the substance-use coping command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 052. denial coping — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the denial coping command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 053. canonical trauma provenance — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the canonical trauma provenance command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 054. trait and phobia identity collision — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the trait and phobia identity collision command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 055. therapy owner choice — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the therapy owner choice command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 056. resilience projection ownership — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the resilience projection ownership command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 057. subterranean context trigger — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the subterranean context trigger command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 058. work or expedition effect — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the work or expedition effect command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 059. psychology panel read-only view — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the psychology panel read-only view command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 060. phobia growth retirement gate — Persistence and replay

Capture psychological_profiles section through PsychologicalProfileSaveStore immediately before the phobia growth retirement gate command, immediately after it, and after the destination effect. Restore each image into a fresh host and compare source identity, destination state, and readout. State what happens if the save lands between source commitment and consumer application. Re-delivering the same fact must reconcile pending work or refuse a duplicate without changing totals. An old save with no new field must have an explicit baseline; an invalid or duplicate ID must produce a bounded error rather than a partial mutation.

### 061. claustrophobia in enclosed space — Catalog and prose contract

Inspect the current authored row relevant to claustrophobia in enclosed space and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 062. nyctophobia in darkness — Catalog and prose contract

Inspect the current authored row relevant to nyctophobia in darkness and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 063. blood phobia near injury — Catalog and prose contract

Inspect the current authored row relevant to blood phobia near injury and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 064. radiation phobia at detection — Catalog and prose contract

Inspect the current authored row relevant to radiation phobia at detection and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 065. social phobia at gatherings — Catalog and prose contract

Inspect the current authored row relevant to social phobia at gatherings and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 066. thalassophobia near deep water — Catalog and prose contract

Inspect the current authored row relevant to thalassophobia near deep water and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 067. meditation coping — Catalog and prose contract

Inspect the current authored row relevant to meditation coping and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 068. exercise coping — Catalog and prose contract

Inspect the current authored row relevant to exercise coping and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 069. socializing coping — Catalog and prose contract

Inspect the current authored row relevant to socializing coping and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 070. creative-work coping — Catalog and prose contract

Inspect the current authored row relevant to creative-work coping and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 071. substance-use coping — Catalog and prose contract

Inspect the current authored row relevant to substance-use coping and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 072. denial coping — Catalog and prose contract

Inspect the current authored row relevant to denial coping and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 073. canonical trauma provenance — Catalog and prose contract

Inspect the current authored row relevant to canonical trauma provenance and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 074. trait and phobia identity collision — Catalog and prose contract

Inspect the current authored row relevant to trait and phobia identity collision and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 075. therapy owner choice — Catalog and prose contract

Inspect the current authored row relevant to therapy owner choice and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 076. resilience projection ownership — Catalog and prose contract

Inspect the current authored row relevant to resilience projection ownership and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 077. subterranean context trigger — Catalog and prose contract

Inspect the current authored row relevant to subterranean context trigger and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 078. work or expedition effect — Catalog and prose contract

Inspect the current authored row relevant to work or expedition effect and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 079. psychology panel read-only view — Catalog and prose contract

Inspect the current authored row relevant to psychology panel read-only view and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 080. phobia growth retirement gate — Catalog and prose contract

Inspect the current authored row relevant to phobia growth retirement gate and verify that its IDs point to real producers and consumers. Propose copy for observation, action, refusal, and aftermath only where the loader has matching fields. Keep numerical tuning in JSON or the canonical domain rule, never in display text. The wording should show what a shelter witness could observe without claiming knowledge of unseen internal state. Give the player enough context to infer the consequence and one next action, while leaving a failed or unknown lookup legible.

### 081. claustrophobia in enclosed space — Player route and accessibility

Present claustrophobia in enclosed space through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 082. nyctophobia in darkness — Player route and accessibility

Present nyctophobia in darkness through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 083. blood phobia near injury — Player route and accessibility

Present blood phobia near injury through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 084. radiation phobia at detection — Player route and accessibility

Present radiation phobia at detection through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 085. social phobia at gatherings — Player route and accessibility

Present social phobia at gatherings through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 086. thalassophobia near deep water — Player route and accessibility

Present thalassophobia near deep water through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 087. meditation coping — Player route and accessibility

Present meditation coping through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 088. exercise coping — Player route and accessibility

Present exercise coping through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 089. socializing coping — Player route and accessibility

Present socializing coping through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 090. creative-work coping — Player route and accessibility

Present creative-work coping through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 091. substance-use coping — Player route and accessibility

Present substance-use coping through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 092. denial coping — Player route and accessibility

Present denial coping through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 093. canonical trauma provenance — Player route and accessibility

Present canonical trauma provenance through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 094. trait and phobia identity collision — Player route and accessibility

Present trait and phobia identity collision through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 095. therapy owner choice — Player route and accessibility

Present therapy owner choice through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 096. resilience projection ownership — Player route and accessibility

Present resilience projection ownership through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 097. subterranean context trigger — Player route and accessibility

Present subterranean context trigger through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 098. work or expedition effect — Player route and accessibility

Present work or expedition effect through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 099. psychology panel read-only view — Player route and accessibility

Present psychology panel read-only view through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 100. phobia growth retirement gate — Player route and accessibility

Present phobia growth retirement gate through a proposed consentful profile/care view after owner collision audit as a read-only state followed by an explicit command if one exists. The surface should show the selected subject, current state, cause, predicted or actual effect, and a plain-language blocker. On success, refresh from the owner rather than incrementing a local visual counter. On denial, keep focus on the attempted action and expose an accessible reason. Keyboard/controller close and back must return focus to the launching control. Reopening the surface must not replay the command.

### 101. claustrophobia in enclosed space — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for claustrophobia in enclosed space. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 102. nyctophobia in darkness — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for nyctophobia in darkness. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 103. blood phobia near injury — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for blood phobia near injury. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 104. radiation phobia at detection — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for radiation phobia at detection. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 105. social phobia at gatherings — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for social phobia at gatherings. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 106. thalassophobia near deep water — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for thalassophobia near deep water. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 107. meditation coping — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for meditation coping. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 108. exercise coping — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for exercise coping. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 109. socializing coping — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for socializing coping. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 110. creative-work coping — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for creative-work coping. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 111. substance-use coping — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for substance-use coping. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 112. denial coping — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for denial coping. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 113. canonical trauma provenance — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for canonical trauma provenance. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 114. trait and phobia identity collision — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for trait and phobia identity collision. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 115. therapy owner choice — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for therapy owner choice. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 116. resilience projection ownership — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for resilience projection ownership. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 117. subterranean context trigger — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for subterranean context trigger. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 118. work or expedition effect — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for work or expedition effect. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 119. psychology panel read-only view — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for psychology panel read-only view. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 120. phobia growth retirement gate — Failure containment

Use a trigger evaluated twice in one day or a profile that diagnoses a survivor from a generic mood value as the negative fixture for phobia growth retirement gate. Verify that all validation occurs before a debit, standing delta, stage change, or emitted irreversible fact. The response must distinguish missing authored reference, ineligible actor, stale target, and already-applied identity where those cases exist. Log enough stable IDs for diagnosis without exposing private configuration. A failure that occurs after one owner has committed must become a durable pending reconciliation, not be hidden by a success toast or silently dropped.

### 121. claustrophobia in enclosed space — Adjacent-owner collision

Trace each effect claimed by claustrophobia in enclosed space through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 122. nyctophobia in darkness — Adjacent-owner collision

Trace each effect claimed by nyctophobia in darkness through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 123. blood phobia near injury — Adjacent-owner collision

Trace each effect claimed by blood phobia near injury through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 124. radiation phobia at detection — Adjacent-owner collision

Trace each effect claimed by radiation phobia at detection through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 125. social phobia at gatherings — Adjacent-owner collision

Trace each effect claimed by social phobia at gatherings through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 126. thalassophobia near deep water — Adjacent-owner collision

Trace each effect claimed by thalassophobia near deep water through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 127. meditation coping — Adjacent-owner collision

Trace each effect claimed by meditation coping through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 128. exercise coping — Adjacent-owner collision

Trace each effect claimed by exercise coping through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 129. socializing coping — Adjacent-owner collision

Trace each effect claimed by socializing coping through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 130. creative-work coping — Adjacent-owner collision

Trace each effect claimed by creative-work coping through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 131. substance-use coping — Adjacent-owner collision

Trace each effect claimed by substance-use coping through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 132. denial coping — Adjacent-owner collision

Trace each effect claimed by denial coping through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 133. canonical trauma provenance — Adjacent-owner collision

Trace each effect claimed by canonical trauma provenance through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 134. trait and phobia identity collision — Adjacent-owner collision

Trace each effect claimed by trait and phobia identity collision through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 135. therapy owner choice — Adjacent-owner collision

Trace each effect claimed by therapy owner choice through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 136. resilience projection ownership — Adjacent-owner collision

Trace each effect claimed by resilience projection ownership through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 137. subterranean context trigger — Adjacent-owner collision

Trace each effect claimed by subterranean context trigger through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 138. work or expedition effect — Adjacent-owner collision

Trace each effect claimed by work or expedition effect through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 139. psychology panel read-only view — Adjacent-owner collision

Trace each effect claimed by psychology panel read-only view through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 140. phobia growth retirement gate — Adjacent-owner collision

Trace each effect claimed by phobia growth retirement gate through the current map of owners. Ask whether the same condition already reaches existing mental-health, trauma, sleep, therapy, and survivor owners from another subsystem, and list the exact event or delegate to reuse. If two effects are intentionally additive, specify order and caps in the canonical owner; if they are aliases, retire one producer. Keep player-facing summaries tied to the final owner state so the account remains truthful when a different subsystem changes it later. This review should precede content volume or balance tuning.

### 141. claustrophobia in enclosed space — Day order and deterministic boundary

Pin claustrophobia in enclosed space to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 142. nyctophobia in darkness — Day order and deterministic boundary

Pin nyctophobia in darkness to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.

### 143. blood phobia near injury — Day order and deterministic boundary

Pin blood phobia near injury to one point in the campaign-day sequence or to a player command with an explicit day. Record whether it reads state from the opening or closing of that day. Stable-sort any multi-record inputs by canonical ID before applying them, and use the existing seeded RNG fork only if the current rule requires a roll. Save/reload on the boundary must preserve both the selected outcome and whether it has applied. A second tick for the same day must be a no-op or an explicit rejected call.


## Polishing pass and architecture handoff

This revision received a second editorial pass after the architecture and casebooks were assembled. The pass normalizes headings and whitespace, treats the current evidence section as authoritative over older speculative instructions, preserves the older scenario inventory as conditional intent, removes the most consequential false present-tense claims, and checks that every implementation phase has an owner, a save rule, a player route, a failure path, and a focused proof. Casebook prose is deliberately phrased as review work where a consumer or route has not been verified. The live-source recensus remains mandatory before implementation, especially where another active claim is changing a host.

**Closeout:** Plan 179 now has a documented integration architecture and a bounded first-slice method. No production behavior has been changed by this document. Runtime completion requires the claimed implementation package, focused verification, and the handoff described above.
