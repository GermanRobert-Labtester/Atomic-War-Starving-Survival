# Plan 187 — Bestiary UI — Wildlife Knowledge and Encounter Provenance
> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome
Audit one encounter or observation producer through the wildlife knowledge owner to the existing
BestiaryPanel, then add only a missing provenance/read-model field or truthful UI affordance.

**Current state:** LIVE PANEL: BestiaryPanel is instantiated, routed, bound to WildlifeEcosystemHostSession
and migration provider. The historical BestiarySystem has discovery/kill/sighting state but no src consumer;
reusing it as a second tracker would conflict with the live ecosystem. INTEGRATION_PLANS removed Plan 187
from the partial queue.

**Non-goals:** No second bestiary discovery ledger, unbounded sighting log, duplicate kill count,
panel-owned population estimate, fabricated encounter event, or unconditional reveal of hidden wildlife
data.

**First deliverable:** trace one existing wildlife observation into a saved knowledge change and the live
BestiaryPanel; document the unhosted BestiarySystem disposition before any new feed.

## 2. Authority and evidence status
The current or candidate domain authority is `WildlifeEcosystemSystem owns production species knowledge,
sightings/ecology, extinction and taming; BestiarySystem is an unhosted historical Core ledger`. Source and
adjacent paths inspected for this revision (some are candidate consumers rather than active bindings):
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`, `src/Host/WildlifeEcosystemHostSession.cs`,
`src/Main.Plans162_165.cs`, `src/Host/WildlifeEcosystemSaveStore.cs`, `src/UI/BestiaryPanel.cs`, and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json`. The focused test starting point is
`Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`. Persistence boundary under audit:
`wildlife_ecosystem section; no BestiarySystem save section`. Paths are evidence pointers, not advance
claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-14/C14 ecology
and A-29 natural-history content identify the live wildlife catalog; C17 UI requires knowledge-gated
presentation. The master asks for prose continuation through existing loader rather than a duplicate runtime
tracker. Its Part II requires live premise checks, bounded subject scope, explicit evidence labels, and a
duplication firewall. The relevant deep maps and lane matrices guide coverage; they do not override newer
code. The master compilation itself warns against padding and stale repository assumptions. This plan
therefore records concrete contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall
- **Evidence or explicit premise:** BestiaryPanel is live and binds WildlifeEcosystemHostSession plus a
  WildlifeMigrationSystem provider.
- **Evidence or explicit premise:** The panel gates species knowledge, ecology status, apex activity and
  taming through the ecosystem owner; it does not bind BestiarySystem.
- **Evidence or explicit premise:** SaveSectionRegistry registers wildlife_ecosystem; no BestiarySystem
  section or host reference was found.
- **Evidence or explicit premise:** BestiarySystem.RecordKill internally calls RecordEncounter, which would
  double count if naively fed an encounter and kill event pair.
- **Evidence or explicit premise:** The narrative bestiary catalog contains 24 creature_id rows; production
  ecosystem species IDs require an explicit crosswalk before they can be treated as the same identity.
- **Evidence or explicit premise:** The master’s A-29 is a data/prose continuation with no save impact,
  distinct from a new encounter tracker.

### Bestiary identity and feed dossier

The requested Plan 187 panel exists, but it is the Plan 165 production `BestiaryPanel`. It binds
`WildlifeEcosystemHostSession` and a migration provider, and the `wildlife_ecosystem` section stores its
facts. The separate `BestiarySystem` Core class has its own `BestiaryState`, encounter count, kill count,
sighting list and note keys, yet has no production host reference. A new encounter bridge into that class
would fork wildlife knowledge. Phase 0 must explicitly disposition the unused class and compare its concepts
to the ecosystem state; do not assume it is the route because it carries the plan number.

`wasteland_wildlife_bestiary.json` uses 24 `creature_id` values. Check the live `wildlife_ecosystem` catalog
ID vocabulary before attaching one to an ecosystem species. A name match is insufficient. If there is no
stable crosswalk, prose can remain a catalog continuation under A-29, while runtime encounter effects remain
with their existing species IDs.

The first useful slice is a real observation or tame event → canonical knowledge change → saved ecosystem
state → changed row in the existing panel. If encounter provenance is absent, name its source (expedition,
combat, trapping, migration) and its stable event key, then ask the wildlife owner for a bounded
append/query API. A decorative sighting list with no source is not integrated.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Host/WildlifeEcosystemHostSession.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `wildlife_ecosystem section; no BestiarySystem save section` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/BestiaryPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Existing panel binding is the production path.
bestiaryPanel.Bind(wildlifeEcosystemSession, () => world?.Wildlife);
// A confirmed encounter invokes the existing wildlife knowledge command.
// RefreshView then reads KnowledgeLevel(speciesId); it never increments
// BestiarySystem.RecordEncounter in parallel.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration
Inspect `src/Host/WildlifeEcosystemSaveStore.cs` and the registry for the named concern; absence of a
section is a decision gate, not permission to invent one. Write down exact DTO version, constructor
baseline, capture point, restore point, and dirty flag. For a stateless bridge, persist only the source and
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
Inspect the current route to `src/UI/BestiaryPanel.cs` and its bind/open/close methods. Expose only commands
backed by an existing Core method or a specifically planned method in the named owner. The panel should show
source state, currently legal action, expected cost, blocker, committed outcome, and uncertainty where the
game cannot know more. Refresh on authoritative state change and after restore; unbind subscriptions on
close/disposal. Preserve keyboard/controller back and readable contrast.

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
| `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Host/WildlifeEcosystemHostSession.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.Plans162_165.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/WildlifeEcosystemSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/BestiaryPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback
Use `bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` for the first
contract check, then only the directly affected save and host targets identified by Phase 0. Run a Godot
headless probe only if the claimed change affects the runtime host path. A compile result cannot establish
live route reachability. Acceptance requires: (1) one real input; (2) one canonical consumer effect; (3) one
durable restore; (4) repeat delivery without duplicate effect; (5) visible correct state and blocker; (6) no
new authority.
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

### 01. first observed species [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns first observed species. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** One canonical observation raises KnowledgeLevel for the species through
WildlifeEcosystemSystem and changes exactly one panel row.

**Fresh campaign path.** For first observed species, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For first observed species, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For first observed species, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For first observed species, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For first observed species, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For first observed species, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For first observed species, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For first observed species, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 02. known but undocumented species [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns known but undocumented species. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** The panel shows only knowledge permitted by the current level; exact
population and hidden behavior remain masked.

**Fresh campaign path.** For known but undocumented species, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For known but undocumented species, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For known but undocumented species, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For known but undocumented species, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For known but undocumented species, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For known but undocumented species, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For known but undocumented species, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For known but undocumented species, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 03. documented species [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns documented species. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A documented row may reveal the authored ecology fields allowed by the
current knowledge policy.

**Fresh campaign path.** For documented species, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For documented species, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For documented species, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For documented species, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For documented species, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For documented species, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For documented species, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For documented species, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 04. unrecognized creature ID [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns unrecognized creature ID. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** An encounter ID absent from the live ecosystem catalog is diagnosed, not
inserted as a ghost discovery.

**Fresh campaign path.** For unrecognized creature ID, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For unrecognized creature ID, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For unrecognized creature ID, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For unrecognized creature ID, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For unrecognized creature ID, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For unrecognized creature ID, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For unrecognized creature ID, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For unrecognized creature ID, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 05. creature-to-species crosswalk [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns creature-to-species crosswalk. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Map a narrative creature_id to a real ecosystem species ID before
connecting prose or encounter events.

**Fresh campaign path.** For creature-to-species crosswalk, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For creature-to-species crosswalk, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For creature-to-species crosswalk, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For creature-to-species crosswalk, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For creature-to-species crosswalk, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For creature-to-species crosswalk, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For creature-to-species crosswalk, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For creature-to-species crosswalk, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 06. local extinction [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns local extinction. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A sector extinction fact changes the ecosystem state and panel status
without erasing the species catalog.

**Fresh campaign path.** For local extinction, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For local extinction, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For local extinction, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For local extinction, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For local extinction, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For local extinction, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For local extinction, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For local extinction, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 07. apex activity [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns apex activity. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A canonical apex activity changes the status rail; opening the panel cannot
create activity.

**Fresh campaign path.** For apex activity, Start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. The card passes only when the
named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For apex activity, Deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For apex activity, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For apex activity, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For apex activity, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For apex activity, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For apex activity, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For apex activity, Replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines the
allowed range. If this card needs randomness, fork from the existing campaign stream and make the event key
stable. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 08. migration context [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns migration context. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** The provider supplies current migration status as a read-only context,
never a second population count.

**Fresh campaign path.** For migration context, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For migration context, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For migration context, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For migration context, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For migration context, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For migration context, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For migration context, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For migration context, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 09. taming action [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns taming action. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** TAME routes through WildlifeEcosystemHostSession authority transfer and
displays the resulting domestic animal once.

**Fresh campaign path.** For taming action, Start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. The card passes only when the
named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For taming action, Deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For taming action, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For taming action, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For taming action, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For taming action, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For taming action, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For taming action, Replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines the
allowed range. If this card needs randomness, fork from the existing campaign stream and make the event key
stable. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 10. failed taming [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns failed taming. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A rejected TAME preserves population, domestic record and displayed count
while showing the real refusal.

**Fresh campaign path.** For failed taming, Start a new seeded campaign with the smallest legal source fact.
Invoke the current owner through its normal host entry point, then inspect the projected outcome at the
intended consumer. State the exact identifier and day in the focused fixture. The card passes only when the
named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For failed taming, Deliver the same source fact twice, including a retry after a
UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For failed taming, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For failed taming, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For failed taming, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For failed taming, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For failed taming, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For failed taming, Replay identical seed, content and ordered facts in two fresh
runs; compare the existing section state and consumer read model. Clamp only at the owner that defines the
allowed range. If this card needs randomness, fork from the existing campaign stream and make the event key
stable. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 11. sighting provenance [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns sighting provenance. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** One verified producer fact may appear in a bounded view only if its source
ID/day/location are available from the existing owner.

**Fresh campaign path.** For sighting provenance, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For sighting provenance, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For sighting provenance, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For sighting provenance, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For sighting provenance, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For sighting provenance, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For sighting provenance, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For sighting provenance, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 12. encounter count proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns encounter count proposal. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Do not store a new encounter count unless a specific gameplay consumer or
signed presentation requirement justifies custody.

**Fresh campaign path.** For encounter count proposal, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For encounter count proposal, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For encounter count proposal, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For encounter count proposal, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For encounter count proposal, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For encounter count proposal, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For encounter count proposal, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For encounter count proposal, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 13. kill fact proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns kill fact proposal. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A combat kill must carry creature identity and once-only battle ID; do not
count RecordEncounter and RecordKill separately.

**Fresh campaign path.** For kill fact proposal, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For kill fact proposal, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For kill fact proposal, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For kill fact proposal, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For kill fact proposal, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For kill fact proposal, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For kill fact proposal, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For kill fact proposal, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 14. butcher fact proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns butcher fact proposal. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Butchery remains with inventory/harvest owner; Bestiary shows only a
verified derived fact.

**Fresh campaign path.** For butcher fact proposal, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For butcher fact proposal, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For butcher fact proposal, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For butcher fact proposal, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For butcher fact proposal, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For butcher fact proposal, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For butcher fact proposal, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For butcher fact proposal, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 15. note unlock tier [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns note unlock tier. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Authored natural-history text appears only when the current knowledge gate
permits that entry.

**Fresh campaign path.** For note unlock tier, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For note unlock tier, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For note unlock tier, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For note unlock tier, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For note unlock tier, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For note unlock tier, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For note unlock tier, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For note unlock tier, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 16. catalog entry thinness [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns catalog entry thinness. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** For one of 24 creature rows, audit sighting/specimen prose fields before
authoring a continuation.

**Fresh campaign path.** For catalog entry thinness, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For catalog entry thinness, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For catalog entry thinness, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For catalog entry thinness, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For catalog entry thinness, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For catalog entry thinness, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For catalog entry thinness, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For catalog entry thinness, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 17. panel selection [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns panel selection. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Changing rows preserves selected species ID and refreshes details without
disclosing masked fields.

**Fresh campaign path.** For panel selection, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For panel selection, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For panel selection, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For panel selection, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For panel selection, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For panel selection, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For panel selection, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For panel selection, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 18. panel close and rebind [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns panel close and rebind. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Unsubscribe StateChanged when replacing the host and preserve
close/back/focus behavior.

**Fresh campaign path.** For panel close and rebind, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For panel close and rebind, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For panel close and rebind, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For panel close and rebind, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For panel close and rebind, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For panel close and rebind, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For panel close and rebind, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For panel close and rebind, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 19. save/load discovery [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns save/load discovery. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Restore wildlife_ecosystem into a fresh host and compare knowledge levels
and panel rows.

**Fresh campaign path.** For save/load discovery, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For save/load discovery, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For save/load discovery, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For save/load discovery, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For save/load discovery, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For save/load discovery, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For save/load discovery, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For save/load discovery, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 20. historical BestiarySystem disposition [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns historical BestiarySystem disposition. Start from
`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
`Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` and trace any effect through
`src/Host/WildlifeEcosystemHostSession.cs` to its current destination owner. Persistence must follow this
boundary: `wildlife_ecosystem section; no BestiarySystem save section`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Mark its independent state DELIVERED, RETIRED or explicitly migrated before
any host binding; default is no second ledger.

**Fresh campaign path.** For historical BestiarySystem disposition, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For historical BestiarySystem disposition, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For historical BestiarySystem disposition, Capture the relevant existing section after
the source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For historical BestiarySystem disposition, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For historical BestiarySystem disposition, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For historical BestiarySystem disposition, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For historical BestiarySystem disposition, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For historical BestiarySystem disposition, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if it covers
this contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

## 14. Legacy plan reconciliation register
The original 2026-09-01 task list is preserved as intent here in condensed form. Numbered items that are
already delivered or contradicted by signed authority must be marked DELIVERED or RETIRED. Each entry is a
premise question, never an instruction to create a duplicate class or save section. Current code and the
live ownership ledger decide whether it becomes a claim.
- **L01:** Create `BestiarySystem.cs` in `Assets/Ashfall.Core/Bestiary/`. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `CreatureDiscovery` DTO: `creatureId`, `discoveredDay`, `encounterCount` (times seen),
  `killCount` (times killed), `butcherCount` (times butchered), `firstEncounterLocation` (location_id),
  `lastEncounterDay`, `discoveryContext` (first encounter description: "spotted during expedition",
  "attacked shelter", "found dead", etc.). Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `CreatureSighting` DTO: `sightingId`, `creatureId`, `day`, `locationId`,
  `witnessSurvivorId`, `sightingType` (spotted/attacked/fleeing/dead/track_found), `distance`
  (close/medium/far), `behavior` (hunting/resting/migrating/feeding/aggressive). Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `CreatureBehaviorNote` DTO: `noteId`, `creatureId`, `noteType`
  (habitat/diet/behavior/weakness/strength/danger), `unlockThreshold` (encounter count required to unlock),
  `noteText` (unlocked narrative text). Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `BestiaryState` DTO: list of creature discoveries, list of sightings (recent 50), list of
  unlocked behavior notes, bestiary completion percentage, total creatures discovered count. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define discovery mechanics:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define creature categories (from existing 24 creatures in catalog):. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define sighting tracking:. Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define behavior note unlock system:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define bestiary UI:. Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Add deterministic seeding: encounter generation uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Wire into `GameBootstrap`: `SetupBestiary`, `TickBestiary` (process sightings), `SaveBestiary`.
  Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Create bestiary panel UI in `src/UI/BestiaryPanel.cs`. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Integrate with `JournalSystem`: creature discoveries unlock journal entries. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement discovery system:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement encounter tracking:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement kill/butcher tracking:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement behavior note unlocks:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement sighting generation:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement sighting log:. Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement bestiary completion:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Implement creature categories:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Implement creature detail view:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Create discovery events:. Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add discovery quest hooks:. Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement bestiary UI panel:. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add bestiary journal: automatic log of discovery events. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Implement discovery tutorial: first creature encounter explains system. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Add discovery tooltips: hover over creature shows encounter count, note progress. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Wire into `WastelandBestiaryCatalog`: creature data loaded. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to `ExpeditionSystem`: sightings generated during expeditions. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Integrate with `CombatSystem`: kills tracked. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Connect to `JournalSystem`: discoveries unlock journal entries. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Wire into `LocationEvolutionSystem`: creature habitats affect location evolution. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Connect to `WeatherSystem`: weather affects sighting probability. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Implement old-save compatibility: existing saves get empty bestiary (all creatures unknown).
  Verify against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Add deterministic seeding: sightings use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Create exploit prevention: sightings are time/location-based, can't be farmed. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Add tests: discovery, encounter tracking, note unlocks, sightings, save round-trip. Verify
  against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Verify all 24 creatures can be discovered. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Test edge cases: no encounters (empty bestiary), many encounters (all notes unlocked). Verify
  against `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Verify headless behavior: bestiary processes correctly without UI. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Add data-integrity-selftest: bestiary validates against creature catalog. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L45:** Create `--bestiary-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract
**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`
plus targeted owner save/host checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
