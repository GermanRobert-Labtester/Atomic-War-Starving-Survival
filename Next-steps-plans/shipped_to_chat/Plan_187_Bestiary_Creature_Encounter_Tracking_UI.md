# Plan 187 — Bestiary UI — Wildlife Knowledge and Encounter Provenance

## Current evidence and integration architecture — 2026-09-24

This section controls the integration route for **Bestiary and encounter tracking**. The scenario inventory retained below is historical planning material: a line there that proposes a new system, save section, catalog, or UI route is conditional until Phase 0 proves the premise and a named owner claims the files. This document is a plan and does not modify production behavior.

**VERIFIED current state:** The production BestiaryPanel already binds WildlifeEcosystemHostSession and a wildlife migration provider. The separate BestiarySystem is Core-only at this audit; its encounter, kill, and note state must not become a parallel wildlife knowledge ledger.

**Master expansion use:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` supplies Lane A/C14 natural-history field reports and Lane E/C17 knowledge-gated presentation. Part II requires a premise sweep, a duplication firewall, explicit evidence labels, and one bounded outcome per package. The master gives ideas for content and integration questions; it cannot overrule live source, signed retirement, or a current path claim.

### Bounded outcome and authority line

The first deliverable is one truthful path from a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID to the WildlifeEcosystemSystem knowledge level and its existing saved state, saved under `wildlife_ecosystem` and shown through src/UI/BestiaryPanel.cs. It must handle one valid event, one refusal, and a replay after restore. The immediate outcome may be an audit and a re-open decision where the current owner already closes the player need. A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter.

| Responsibility | Verified or candidate location | Boundary |
|---|---|
| Current rule owner | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | Read the public command and state before modifying behavior. |
| Unhosted or adjacent Core concept | `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` | A class's existence alone does not make it campaign authority. |
| Host/lifecycle | `src/Main.Plans162_165.cs and src/Host/WildlifeEcosystemHostSession.cs` | Bind once after restore, save on actual mutation, detach on reset. |
| Authored data | `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json` | Use only fields read by the current loader; validate IDs and consumer reachability. |
| Save custody | `wildlife_ecosystem` | No new section without a signed ownership decision and old-save rule. |
| Player presentation | `src/UI/BestiaryPanel.cs` | A panel reads owner state and sends only supported commands. |

### Dependency-ordered integration phases

**Phase 0 — recensus and claim.** Re-read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, the Core owner, catalog, host, save registry and panel. Record current evidence, a single chosen case, exact file paths, and any signed decision gate. Existing claims are read-only to nonowners. If the source contradicts this section, update the plan first.

**Phase 1 — source-to-owner contract.** Name the committed producer event, stable subject and event IDs, campaign day, destination method, and refusal codes. A query, panel refresh, or forecast may not become the producer. If two Core classes claim the same state, select the live owner and either make the other a pure projection or retire its mutable path.

**Phase 2 — data and prose.** Inspect the actual JSON shape and loaded IDs. Author a small, reachable content slice with an observation, an action label, a consequence and a refusal. Give each line a known cause; do not disclose hidden simulation values. Check catalog references and the present UI consumer before adding rows. Larger flavor catalogs wait for utilization evidence.

**Phase 3 — state and deterministic replay.** Capture the source and destination owner state after one mutation. Restore in a fresh session before attaching event handlers, then deliver the same source ID again. It must apply exactly once. Document the legacy missing-field baseline, invalid record policy, save order across owners, and a recovery path for a crash between two section writes. Use the current seeded RNG stream if and only if the rule needs a roll.

**Phase 4 — host and presentation.** Attach the result to `src/Main.Plans162_165.cs and src/Host/WildlifeEcosystemHostSession.cs` in its existing lifetime, then project through `src/UI/BestiaryPanel.cs`. Show current state, cause, allowed next action, and a plain-language blocker. Refresh on state change; preserve keyboard/controller close and focus; do not let the panel own a hidden ledger. A host selftest is warranted only if this phase touches the runtime route.

**Phase 5 — focused acceptance and handoff.** Start with `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` if implementation changes that contract, then select only directly affected save/host/UI checks under `TEST_POLICY.md`. Record actual commands and results, source and destination values, old-save parity, repeated delivery, and a negative case. The handoff names unresolved decisions and rollback boundaries. This documentation pass performs static review only.

### C# placement framework — existing API anchors

The fragment identifies call direction and a real API anchor. It is not a new subsystem, a complete patch, or permission to edit a claimed composition root. Variables come from the owning method; the builder reconciles signatures against source during Phase 0.

```csharp
_bestiaryPanel.Bind(_wildlifeEcosystem, () => _world?.Wildlife);
// Proposed encounter-provenance extension: translate a real source fact to
// the existing ecosystem species ID, then refresh this already bound panel.
// Do not instantiate BestiarySystem as a second mutable campaign ledger.
```

**Command contract.** A command enters the current host with a stable subject ID and day. Validate the actor and authored reference before an irreversible mutation. Return the owner’s accepted result or a specific refusal. Only the owner named above changes the destination value. The host may translate a typed fact, mark its existing section dirty, and publish a read model; it must not calculate a competing rule.

**Capture contract.** The record that prevents repeat effects must live with the mutation owner or a signed transaction carrier. A process-local boolean is not persistence. A restore reconstitutes Core state, then host subscriptions, then UI bindings. If a copied scene can apply only after another section loads, describe the pending state and recovery instead of assuming all saves are atomic.

**Projection contract.** The player view shows a label, source, day, current consequence, available command, and refusal from the owner. It may format values and choose restrained diegetic wording, but cannot infer a hidden diagnosis, species count, item property, potable-water status, treaty standing, or completed treatment from incomplete evidence. The panel's read path should be safe to call repeatedly and after save/load.

**Focused proof specimen.** A small test or host probe should start from one real catalog row and one producer fact, assert the authoritative destination field before and after, capture and restore state, replay the same ID, and assert no duplicate effect. Another input should be invalid at the exact boundary this plan touches. When UI is in scope, verify read-only refresh, visible refusal, and focus return; a build-only result is structural evidence rather than runtime reachability.

### Architecture completion rule

Planning architecture is complete when every candidate effect has a producer, rule owner, destination owner, host attachment, save carrier, duplicate guard, player readout, negative case, and focused proof. Runtime integration remains open until a claimed package implements and verifies one such path. The legacy sections below are an inventory of cases to choose from, not a requirement to implement them all at once.


## Historical scenario inventory and prior prose candidates

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
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

## Detailed implementation and narrative casebook

Each entry below is a reviewable case within the same bounded feature; it is not a separate new system. Choose the smallest case whose premise survives the live recensus. The casebook combines the master’s prose, mechanics, save, and UI lenses while keeping the existing authority map fixed.

### 001. first observed species — Origin and witness

For **first observed species**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 002. known but undocumented species — Origin and witness

For **known but undocumented species**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 003. documented species — Origin and witness

For **documented species**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 004. unrecognized creature ID — Origin and witness

For **unrecognized creature ID**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 005. creature-to-species crosswalk — Origin and witness

For **creature-to-species crosswalk**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 006. local extinction — Origin and witness

For **local extinction**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 007. apex activity — Origin and witness

For **apex activity**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 008. migration context — Origin and witness

For **migration context**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 009. taming action — Origin and witness

For **taming action**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 010. failed taming — Origin and witness

For **failed taming**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 011. sighting provenance — Origin and witness

For **sighting provenance**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 012. encounter count proposal — Origin and witness

For **encounter count proposal**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 013. kill fact proposal — Origin and witness

For **kill fact proposal**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 014. butcher fact proposal — Origin and witness

For **butcher fact proposal**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 015. note unlock tier — Origin and witness

For **note unlock tier**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 016. catalog entry thinness — Origin and witness

For **catalog entry thinness**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 017. panel selection — Origin and witness

For **panel selection**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 018. panel close and rebind — Origin and witness

For **panel close and rebind**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 019. save/load discovery — Origin and witness

For **save/load discovery**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 020. historical BestiarySystem disposition — Origin and witness

For **historical BestiarySystem disposition**, record the actual witnessable source: a committed ecosystem observation, tame, hunt, or expedition encounter with a stable species ID. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed the WildlifeEcosystemSystem knowledge level and its existing saved state.

### 021. first observed species — Domain decision

For **first observed species**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 022. known but undocumented species — Domain decision

For **known but undocumented species**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 023. documented species — Domain decision

For **documented species**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 024. unrecognized creature ID — Domain decision

For **unrecognized creature ID**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 025. creature-to-species crosswalk — Domain decision

For **creature-to-species crosswalk**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 026. local extinction — Domain decision

For **local extinction**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 027. apex activity — Domain decision

For **apex activity**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 028. migration context — Domain decision

For **migration context**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 029. taming action — Domain decision

For **taming action**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 030. failed taming — Domain decision

For **failed taming**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 031. sighting provenance — Domain decision

For **sighting provenance**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 032. encounter count proposal — Domain decision

For **encounter count proposal**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 033. kill fact proposal — Domain decision

For **kill fact proposal**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 034. butcher fact proposal — Domain decision

For **butcher fact proposal**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 035. note unlock tier — Domain decision

For **note unlock tier**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 036. catalog entry thinness — Domain decision

For **catalog entry thinness**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 037. panel selection — Domain decision

For **panel selection**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 038. panel close and rebind — Domain decision

For **panel close and rebind**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 039. save/load discovery — Domain decision

For **save/load discovery**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 040. historical BestiarySystem disposition — Domain decision

For **historical BestiarySystem disposition**, start from `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on the WildlifeEcosystemSystem knowledge level and its existing saved state, not merely report that an event handler fired.

### 041. first observed species — Save boundary

For **first observed species**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 042. known but undocumented species — Save boundary

For **known but undocumented species**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 043. documented species — Save boundary

For **documented species**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 044. unrecognized creature ID — Save boundary

For **unrecognized creature ID**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 045. creature-to-species crosswalk — Save boundary

For **creature-to-species crosswalk**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 046. local extinction — Save boundary

For **local extinction**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 047. apex activity — Save boundary

For **apex activity**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 048. migration context — Save boundary

For **migration context**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 049. taming action — Save boundary

For **taming action**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 050. failed taming — Save boundary

For **failed taming**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 051. sighting provenance — Save boundary

For **sighting provenance**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 052. encounter count proposal — Save boundary

For **encounter count proposal**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 053. kill fact proposal — Save boundary

For **kill fact proposal**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 054. butcher fact proposal — Save boundary

For **butcher fact proposal**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 055. note unlock tier — Save boundary

For **note unlock tier**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 056. catalog entry thinness — Save boundary

For **catalog entry thinness**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 057. panel selection — Save boundary

For **panel selection**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 058. panel close and rebind — Save boundary

For **panel close and rebind**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 059. save/load discovery — Save boundary

For **save/load discovery**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 060. historical BestiarySystem disposition — Save boundary

For **historical BestiarySystem disposition**, capture `wildlife_ecosystem` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 061. first observed species — Player surface and restrained copy

For **first observed species**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 062. known but undocumented species — Player surface and restrained copy

For **known but undocumented species**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 063. documented species — Player surface and restrained copy

For **documented species**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 064. unrecognized creature ID — Player surface and restrained copy

For **unrecognized creature ID**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 065. creature-to-species crosswalk — Player surface and restrained copy

For **creature-to-species crosswalk**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 066. local extinction — Player surface and restrained copy

For **local extinction**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 067. apex activity — Player surface and restrained copy

For **apex activity**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 068. migration context — Player surface and restrained copy

For **migration context**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 069. taming action — Player surface and restrained copy

For **taming action**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 070. failed taming — Player surface and restrained copy

For **failed taming**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 071. sighting provenance — Player surface and restrained copy

For **sighting provenance**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 072. encounter count proposal — Player surface and restrained copy

For **encounter count proposal**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 073. kill fact proposal — Player surface and restrained copy

For **kill fact proposal**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 074. butcher fact proposal — Player surface and restrained copy

For **butcher fact proposal**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 075. note unlock tier — Player surface and restrained copy

For **note unlock tier**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 076. catalog entry thinness — Player surface and restrained copy

For **catalog entry thinness**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 077. panel selection — Player surface and restrained copy

For **panel selection**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 078. panel close and rebind — Player surface and restrained copy

For **panel close and rebind**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 079. save/load discovery — Player surface and restrained copy

For **save/load discovery**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 080. historical BestiarySystem disposition — Player surface and restrained copy

For **historical BestiarySystem disposition**, the view `src/UI/BestiaryPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 081. first observed species — Failure and uncertainty

For **first observed species**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 082. known but undocumented species — Failure and uncertainty

For **known but undocumented species**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 083. documented species — Failure and uncertainty

For **documented species**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 084. unrecognized creature ID — Failure and uncertainty

For **unrecognized creature ID**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 085. creature-to-species crosswalk — Failure and uncertainty

For **creature-to-species crosswalk**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 086. local extinction — Failure and uncertainty

For **local extinction**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 087. apex activity — Failure and uncertainty

For **apex activity**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 088. migration context — Failure and uncertainty

For **migration context**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 089. taming action — Failure and uncertainty

For **taming action**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 090. failed taming — Failure and uncertainty

For **failed taming**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 091. sighting provenance — Failure and uncertainty

For **sighting provenance**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 092. encounter count proposal — Failure and uncertainty

For **encounter count proposal**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 093. kill fact proposal — Failure and uncertainty

For **kill fact proposal**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 094. butcher fact proposal — Failure and uncertainty

For **butcher fact proposal**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 095. note unlock tier — Failure and uncertainty

For **note unlock tier**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 096. catalog entry thinness — Failure and uncertainty

For **catalog entry thinness**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 097. panel selection — Failure and uncertainty

For **panel selection**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 098. panel close and rebind — Failure and uncertainty

For **panel close and rebind**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 099. save/load discovery — Failure and uncertainty

For **save/load discovery**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 100. historical BestiarySystem disposition — Failure and uncertainty

For **historical BestiarySystem disposition**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 101. first observed species — Cross-system ownership

For **first observed species**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 102. known but undocumented species — Cross-system ownership

For **known but undocumented species**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 103. documented species — Cross-system ownership

For **documented species**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 104. unrecognized creature ID — Cross-system ownership

For **unrecognized creature ID**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 105. creature-to-species crosswalk — Cross-system ownership

For **creature-to-species crosswalk**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 106. local extinction — Cross-system ownership

For **local extinction**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 107. apex activity — Cross-system ownership

For **apex activity**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 108. migration context — Cross-system ownership

For **migration context**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 109. taming action — Cross-system ownership

For **taming action**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 110. failed taming — Cross-system ownership

For **failed taming**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 111. sighting provenance — Cross-system ownership

For **sighting provenance**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 112. encounter count proposal — Cross-system ownership

For **encounter count proposal**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 113. kill fact proposal — Cross-system ownership

For **kill fact proposal**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 114. butcher fact proposal — Cross-system ownership

For **butcher fact proposal**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 115. note unlock tier — Cross-system ownership

For **note unlock tier**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 116. catalog entry thinness — Cross-system ownership

For **catalog entry thinness**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 117. panel selection — Cross-system ownership

For **panel selection**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 118. panel close and rebind — Cross-system ownership

For **panel close and rebind**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 119. save/load discovery — Cross-system ownership

For **save/load discovery**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 120. historical BestiarySystem disposition — Cross-system ownership

For **historical BestiarySystem disposition**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: A bestiary entry may describe an observed species but may not reveal hidden population or invent an encounter. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 121. first observed species — Deterministic day and replay

For **first observed species**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 122. known but undocumented species — Deterministic day and replay

For **known but undocumented species**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 123. documented species — Deterministic day and replay

For **documented species**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 124. unrecognized creature ID — Deterministic day and replay

For **unrecognized creature ID**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 125. creature-to-species crosswalk — Deterministic day and replay

For **creature-to-species crosswalk**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 126. local extinction — Deterministic day and replay

For **local extinction**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 127. apex activity — Deterministic day and replay

For **apex activity**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 128. migration context — Deterministic day and replay

For **migration context**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 129. taming action — Deterministic day and replay

For **taming action**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 130. failed taming — Deterministic day and replay

For **failed taming**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 131. sighting provenance — Deterministic day and replay

For **sighting provenance**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 132. encounter count proposal — Deterministic day and replay

For **encounter count proposal**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 133. kill fact proposal — Deterministic day and replay

For **kill fact proposal**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 134. butcher fact proposal — Deterministic day and replay

For **butcher fact proposal**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 135. note unlock tier — Deterministic day and replay

For **note unlock tier**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 136. catalog entry thinness — Deterministic day and replay

For **catalog entry thinness**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 137. panel selection — Deterministic day and replay

For **panel selection**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 138. panel close and rebind — Deterministic day and replay

For **panel close and rebind**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 139. save/load discovery — Deterministic day and replay

For **save/load discovery**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 140. historical BestiarySystem disposition — Deterministic day and replay

For **historical BestiarySystem disposition**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 141. first observed species — Acceptance and prose handoff

For **first observed species**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 142. known but undocumented species — Acceptance and prose handoff

For **known but undocumented species**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 143. documented species — Acceptance and prose handoff

For **documented species**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 144. unrecognized creature ID — Acceptance and prose handoff

For **unrecognized creature ID**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 145. creature-to-species crosswalk — Acceptance and prose handoff

For **creature-to-species crosswalk**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 146. local extinction — Acceptance and prose handoff

For **local extinction**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 147. apex activity — Acceptance and prose handoff

For **apex activity**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 148. migration context — Acceptance and prose handoff

For **migration context**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 149. taming action — Acceptance and prose handoff

For **taming action**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 150. failed taming — Acceptance and prose handoff

For **failed taming**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 151. sighting provenance — Acceptance and prose handoff

For **sighting provenance**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 152. encounter count proposal — Acceptance and prose handoff

For **encounter count proposal**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 153. kill fact proposal — Acceptance and prose handoff

For **kill fact proposal**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 154. butcher fact proposal — Acceptance and prose handoff

For **butcher fact proposal**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 155. note unlock tier — Acceptance and prose handoff

For **note unlock tier**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 156. catalog entry thinness — Acceptance and prose handoff

For **catalog entry thinness**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 157. panel selection — Acceptance and prose handoff

For **panel selection**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 158. panel close and rebind — Acceptance and prose handoff

For **panel close and rebind**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 159. save/load discovery — Acceptance and prose handoff

For **save/load discovery**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 160. historical BestiarySystem disposition — Acceptance and prose handoff

For **historical BestiarySystem disposition**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 161. first observed species — implementation decision record

The builder records the current producer signature, stable source ID, exact `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` command or query, current catalog row, `wildlife_ecosystem` DTO field, and a before/after readout from `src/UI/BestiaryPanel.cs`. For this specific case, the decision packet must state whether the prior plan's `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` is DELIVERED, RESIDUAL, BLOCKED, or RETIRED. It must also name the refusal produced when the source is absent and the smallest focused check of restore/replay. A route without a destination owner remains an unimplemented proposal. Candidate prose is reviewed against the observed fact and the owner’s result code before it is promoted to JSON.


## Editorial closeout and implementation handoff

This plan has a current-evidence architecture at the top, a preserved historical scenario inventory, a bounded casebook, and a C# placement framework tied to a real API anchor. The historical material is conditional where it conflicts with the current owner or a signed decision. One implementation package selects one case, claims exact paths, proves source → owner → save → UI, and reports the negative and replay paths. Documentation completion is not a claim that the runtime route is integrated.

## Polished candidate prose tied to real state

These lines are editorial candidates, not authored JSON or proof of a live route. Their trigger notes tell an integrator which owner fact must exist before the line can be used. Each candidate should be checked against the actual field length, localization path, and current panel layout before promotion.

### 1. first observation

> Two sets of tracks cross the wet ash. One is fresh enough to hold its edges; the other may belong to yesterday.

**Trigger and restraint:** Show as a field observation only after the ecosystem owner records the species at the appropriate knowledge level.

### 2. unverified account

> The patrol heard something moving beyond the culvert. No one saw enough to name it.

**Trigger and restraint:** Keep the species unidentified until a real producer supplies a stable species ID.

### 3. documented behavior

> The same animal returned to the grain spill at dusk. Three witnesses agree on the route it took.

**Trigger and restraint:** Behavior copy requires the current knowledge gate, not an encounter count invented for the panel.

### 4. tame record

> The animal took food from a gloved hand and followed the handler back to the pens.

**Trigger and restraint:** Read the ecosystem tame event and animal ID; do not create a second population.

### 5. local absence

> The old path is quiet this week. The traps are empty, and there are no new prints.

**Trigger and restraint:** Use the ecosystem extinction or migration state; do not pronounce a species globally extinct.

### Editorial acceptance

A reader should be able to tell observation from confirmed outcome, type description from instance history, and warning from resolution. Remove any line whose source fact or consumer field cannot be named. Preserve the restrained, human tone of the master expansion document; do not use the proposed copy to smuggle in mechanics or mutable state.
