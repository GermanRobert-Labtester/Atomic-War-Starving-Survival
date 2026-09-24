# Plan 198 — Health History & Medical Records — Signed Pipeline Log

## Current evidence and integration architecture — 2026-09-24

This section controls the integration route for **Health history and medical records**. The scenario inventory retained below is historical planning material: a line there that proposes a new system, save section, catalog, or UI route is conditional until Phase 0 proves the premise and a named owner claims the files. This document is a plan and does not modify production behavior.

**VERIFIED current state:** MedicalPipelineCoordinator owns a bounded MedicalRecordLog: at most 64 identifier-only entries, saved inside medical_pipeline and displayed in AfflictionsPanel. HealthHistorySystem and record templates exist in Core/data but have no host route. The signed debt closeout excluded a second history or vaccination authority.

**Master expansion use:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` supplies Lane A/C2 casebook labels and Lane D medical save compatibility. Part II requires a premise sweep, a duplication firewall, explicit evidence labels, and one bounded outcome per package. The master gives ideas for content and integration questions; it cannot overrule live source, signed retirement, or a current path claim.

### Bounded outcome and authority line

The first deliverable is one truthful path from one clinical event emitted by MedicalPipelineCoordinator to MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, saved under `medical_pipeline additive record field` and shown through src/UI/AfflictionsPanel.cs. It must handle one valid event, one refusal, and a replay after restore. The immediate outcome may be an audit and a re-open decision where the current owner already closes the player need. Never persist free-text patient notes, export private records, or duplicate the dose ledger.

| Responsibility | Verified or candidate location | Boundary |
|---|---|
| Current rule owner | `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` | Read the public command and state before modifying behavior. |
| Unhosted or adjacent Core concept | `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` | A class's existence alone does not make it campaign authority. |
| Host/lifecycle | `src/Main.Medical.cs and src/Host/MedicalPipelineSaveStore.cs` | Bind once after restore, save on actual mutation, detach on reset. |
| Authored data | `Assets/StreamingAssets/Data/medical_record_templates.json (presentation candidate only)` | Use only fields read by the current loader; validate IDs and consumer reachability. |
| Save custody | `medical_pipeline additive record field` | No new section without a signed ownership decision and old-save rule. |
| Player presentation | `src/UI/AfflictionsPanel.cs` | A panel reads owner state and sends only supported commands. |

### Dependency-ordered integration phases

**Phase 0 — recensus and claim.** Re-read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, the Core owner, catalog, host, save registry and panel. Record current evidence, a single chosen case, exact file paths, and any signed decision gate. Existing claims are read-only to nonowners. If the source contradicts this section, update the plan first.

**Phase 1 — source-to-owner contract.** Name the committed producer event, stable subject and event IDs, campaign day, destination method, and refusal codes. A query, panel refresh, or forecast may not become the producer. If two Core classes claim the same state, select the live owner and either make the other a pure projection or retire its mutable path.

**Phase 2 — data and prose.** Inspect the actual JSON shape and loaded IDs. Author a small, reachable content slice with an observation, an action label, a consequence and a refusal. Give each line a known cause; do not disclose hidden simulation values. Check catalog references and the present UI consumer before adding rows. Larger flavor catalogs wait for utilization evidence.

**Phase 3 — state and deterministic replay.** Capture the source and destination owner state after one mutation. Restore in a fresh session before attaching event handlers, then deliver the same source ID again. It must apply exactly once. Document the legacy missing-field baseline, invalid record policy, save order across owners, and a recovery path for a crash between two section writes. Use the current seeded RNG stream if and only if the rule needs a roll.

**Phase 4 — host and presentation.** Attach the result to `src/Main.Medical.cs and src/Host/MedicalPipelineSaveStore.cs` in its existing lifetime, then project through `src/UI/AfflictionsPanel.cs`. Show current state, cause, allowed next action, and a plain-language blocker. Refresh on state change; preserve keyboard/controller close and focus; do not let the panel own a hidden ledger. A host selftest is warranted only if this phase touches the runtime route.

**Phase 5 — focused acceptance and handoff.** Start with `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if implementation changes that contract, then select only directly affected save/host/UI checks under `TEST_POLICY.md`. Record actual commands and results, source and destination values, old-save parity, repeated delivery, and a negative case. The handoff names unresolved decisions and rollback boundaries. This documentation pass performs static review only.

### C# placement framework — existing API anchors

The fragment identifies call direction and a real API anchor. It is not a new subsystem, a complete patch, or permission to edit a claimed composition root. Variables come from the owning method; the builder reconciles signatures against source during Phase 0.

```csharp
// Current signed owner: coordinator subscribes its own clinical events.
MedicalRecordLog log = pipeline.Record;
var recent = log.ForSurvivor(survivorId, max: 8);
// AfflictionsPanel renders restrained labels; no HealthHistorySystem
// vaccination or trend effect is implied by this read-only record.
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
Preserve and polish the existing per-patient medical record view, audit source-event coverage and privacy,
and route any desired clinical capability through a new signed medical-owner decision.

**Current state:** SIGNED LIMITED IMPLEMENTATION: MedicalRecordLog already records eight pipeline event
kinds, retains at most 64 identifier-only facts, persists in MedicalPipelineSaveState and appears in
AfflictionsPanel. HealthHistorySystem and seven template rows exist in Core/data but are unhosted; the
signed debt closeout explicitly excluded a second history/vaccination system.

**Non-goals:** No separate HealthHistorySystem save section, free-text patient notes, duplicated dose
ledger, synthetic vaccination immunity, daily trend archive, export route, or automated treatment modifier
without a signed clinical owner.

**First deliverable:** a bounded record-source and panel audit against the signed 64-entry privacy contract.
Any new clinical history authority requires a separate signed decision.

## 2. Authority and evidence status
The current or candidate domain authority is `MedicalPipelineCoordinator.Record is the signed
MedicalRecordLog owner; DoseLedgerSystem retains dose history and treatment systems retain clinical state`.
Source and adjacent paths inspected for this revision (some are candidate consumers rather than active
bindings): `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs`, `src/Main.Medical.cs`,
`src/Main.Inventory.cs`, `src/Host/MedicalPipelineSaveStore.cs`, `src/UI/AfflictionsPanel.cs`, and
`Assets/StreamingAssets/Data/medical_record_templates.json`. The focused test starting point is
`Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs`. Persistence boundary under audit:
`medical_pipeline section contains the additive bounded record field`. Paths are evidence pointers, not
advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: DM-2/C2 medical
pipeline names dose, disease, diagnostics and therapies as separate owners. The master’s live-data rule
cannot override the signed Plan 198 MedicalRecordLog retention and privacy boundary. Its Part II requires
live premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The
relevant deep maps and lane matrices guide coverage; they do not override newer code. The master compilation
itself warns against padding and stale repository assumptions. This plan therefore records concrete
contracts and treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall
- **Evidence or explicit premise:** MedicalPipelineCoordinator internally subscribes its own clinical events
  to MedicalRecordLog, preventing an external bridge from drifting from emit sites.
- **Evidence or explicit premise:** MedicalRecordLog retains 64 oldest-first, campaign-scoped entries with
  day, kind, survivorId and detail ID only; no prose notes.
- **Evidence or explicit premise:** MedicalPipelineSaveState has an additive record field, and old saves
  restore it empty without a new section.
- **Evidence or explicit premise:** AfflictionsPanel already shows recent medical record rows per survivor
  through Pipeline.Record.ForSurvivor.
- **Evidence or explicit premise:** HealthHistorySystem and medical_record_templates.json exist as Core/data
  artifacts but have no src reference; this contradicts the old “ZERO matches” premise yet does not overturn
  the signed authority.
- **Evidence or explicit premise:** DoseLedgerSystem owns radiation dose history; the medical log may point
  to a treatment ID but must not copy or redefine exposure measurements.

### Medical record governance dossier

The signed Plan 198 result is deliberately smaller than the September proposal. `MedicalRecordLog` belongs
to `MedicalPipelineCoordinator`, is bounded to 64 identifier-only events, and is embedded in the existing
`medical_pipeline` save section. It records facts after pipeline events; it does not become a clinical
decision engine. `AfflictionsPanel` already reads it. The unhosted `HealthHistorySystem` has broader
`MedicalRecord`, `VaccinationRecord` and `HealthTrend` DTOs, but its existence is not permission to add a
second record store; the signed debt entry expressly says no HealthHistorySystem/ChronicConditionSystem
host.

The first residual package should audit eight emitted kinds against the event sites and panel labels, check
retention/restore and privacy, and fix only a demonstrated gap. Any request for vaccination immunity,
contraindications, trend forecasts or medication-history effects changes clinical authority. It needs a
foreman decision naming the source medical system, the intervention command, save custody, retention and
privacy policy, and one observable consumer. No generic patient notes or export facility may bypass the
identifier-only rule.

Historical template rows may be useful for presentation only if a validated mapping from event kind to
template ID exists and the panel actually consumes it. A template’s presence does not make its vaccination
or trend mechanics live.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/medical_record_templates.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.Medical.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `medical_pipeline section contains the additive bounded record field` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/AfflictionsPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
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
// Existing clinical read path.
IReadOnlyList<MedicalRecordEntry> recent = pipeline.Record.ForSurvivor(survivorId, 8);
// Record.Append is called from MedicalPipelineCoordinator event subscriptions.
// A panel projects the IDs through approved labels; it never stores notes
// or calculates immunity from an unhosted HealthHistorySystem.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration
Inspect `src/Host/MedicalPipelineSaveStore.cs` and the registry for the named concern; absence of a section
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
Inspect the current route to `src/UI/AfflictionsPanel.cs` and its bind/open/close methods. Expose only
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
| `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/medical_record_templates.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.Medical.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.Inventory.cs` | READ / integrator MODIFY | composition root | high |
| `src/Host/MedicalPipelineSaveStore.cs` | READ / bounded MODIFY | persistence | high |
| `src/UI/AfflictionsPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback
Use `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` for the first
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

### 01. diagnosis suspected [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns diagnosis suspected. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** OnDiagnosisSuspected appends one day/kind/survivor/detail fact and appears
under the correct patient.

**Fresh campaign path.** For diagnosis suspected, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For diagnosis suspected, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For diagnosis suspected, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For diagnosis suspected, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For diagnosis suspected, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For diagnosis suspected, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For diagnosis suspected, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For diagnosis suspected, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 02. diagnosis confirmed [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns diagnosis confirmed. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** OnDiagnosisConfirmed uses the canonical affliction ID rather than a prose
diagnosis note.

**Fresh campaign path.** For diagnosis confirmed, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For diagnosis confirmed, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For diagnosis confirmed, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For diagnosis confirmed, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For diagnosis confirmed, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For diagnosis confirmed, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For diagnosis confirmed, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For diagnosis confirmed, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 03. patient stabilized [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns patient stabilized. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** OnPatientStabilized records a clinical transition once without changing the
patient state in the log.

**Fresh campaign path.** For patient stabilized, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For patient stabilized, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For patient stabilized, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For patient stabilized, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For patient stabilized, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For patient stabilized, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For patient stabilized, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For patient stabilized, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 04. patient recovered [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns patient recovered. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** OnPatientRecovered adds the recovery fact and leaves the source affliction
owner in charge of actual resolution.

**Fresh campaign path.** For patient recovered, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For patient recovered, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For patient recovered, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For patient recovered, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For patient recovered, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For patient recovered, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For patient recovered, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For patient recovered, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 05. treatment scheduled [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns treatment scheduled. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A scheduling fact enters the log only after the medical pipeline accepts
the schedule.

**Fresh campaign path.** For treatment scheduled, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For treatment scheduled, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For treatment scheduled, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For treatment scheduled, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For treatment scheduled, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For treatment scheduled, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For treatment scheduled, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For treatment scheduled, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 06. treatment completed [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns treatment completed. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A completion fact reflects the owning treatment command; inventory costs
and effects are not replayed by the log.

**Fresh campaign path.** For treatment completed, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For treatment completed, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For treatment completed, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For treatment completed, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For treatment completed, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For treatment completed, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For treatment completed, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For treatment completed, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 07. treatment refused [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns treatment refused. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** The refusal entry uses the approved reason ID and does not expose
stigmatizing free text.

**Fresh campaign path.** For treatment refused, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For treatment refused, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For treatment refused, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For treatment refused, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For treatment refused, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For treatment refused, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For treatment refused, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For treatment refused, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 08. protocol executed [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns protocol executed. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A shelter-wide protocol fact can have no survivor ID; patient filters must
not misattribute it.

**Fresh campaign path.** For protocol executed, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For protocol executed, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For protocol executed, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For protocol executed, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For protocol executed, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For protocol executed, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For protocol executed, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For protocol executed, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 09. inventory treatment append [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns inventory treatment append. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** InventoryHostSession.MedicalRecordLog points to the same pipeline log and
does not create a second copy.

**Fresh campaign path.** For inventory treatment append, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For inventory treatment append, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For inventory treatment append, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For inventory treatment append, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For inventory treatment append, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For inventory treatment append, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For inventory treatment append, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For inventory treatment append, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 10. oldest-first eviction [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns oldest-first eviction. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A 65th append removes only the oldest entry while preserving stable order
and the 64-row limit.

**Fresh campaign path.** For oldest-first eviction, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For oldest-first eviction, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For oldest-first eviction, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For oldest-first eviction, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For oldest-first eviction, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For oldest-first eviction, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For oldest-first eviction, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For oldest-first eviction, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 11. per-survivor view [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns per-survivor view. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** ForSurvivor returns newest-first bounded rows only for that exact survivor
ID.

**Fresh campaign path.** For per-survivor view, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For per-survivor view, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For per-survivor view, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For per-survivor view, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For per-survivor view, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For per-survivor view, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For per-survivor view, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For per-survivor view, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 12. old-save baseline [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns old-save baseline. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A medical_pipeline save without record restores an empty log and does not
infer events from active conditions.

**Fresh campaign path.** For old-save baseline, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For old-save baseline, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For old-save baseline, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For old-save baseline, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old-save baseline, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old-save baseline, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old-save baseline, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For old-save baseline, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 13. restore parity [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns restore parity. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Capture and restore preserve day, kind, survivor and detail IDs without
UI-generated prose.

**Fresh campaign path.** For restore parity, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For restore parity, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For restore parity, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For restore parity, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For restore parity, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For restore parity, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For restore parity, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For restore parity, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 14. deceased patient history [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns deceased patient history. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A retained campaign record may still be queried for a deceased survivor
under existing privacy rules.

**Fresh campaign path.** For deceased patient history, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For deceased patient history, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For deceased patient history, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For deceased patient history, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For deceased patient history, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For deceased patient history, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For deceased patient history, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For deceased patient history, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 15. radiation dose boundary [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns radiation dose boundary. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** A clinical record may mention treatment while DoseLedgerSystem remains the
sole dose history authority.

**Fresh campaign path.** For radiation dose boundary, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For radiation dose boundary, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For radiation dose boundary, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For radiation dose boundary, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For radiation dose boundary, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For radiation dose boundary, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For radiation dose boundary, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For radiation dose boundary, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 16. vaccination proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns vaccination proposal. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** HealthHistorySystem.AdministerVaccine cannot be hosted until
disease/immunity owner, item cost and save custody are signed.

**Fresh campaign path.** For vaccination proposal, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For vaccination proposal, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For vaccination proposal, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For vaccination proposal, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For vaccination proposal, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For vaccination proposal, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For vaccination proposal, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For vaccination proposal, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 17. chronic condition proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns chronic condition proposal. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Current chronic flags remain with their medical owner; a record label is
not a second condition state.

**Fresh campaign path.** For chronic condition proposal, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For chronic condition proposal, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For chronic condition proposal, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For chronic condition proposal, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For chronic condition proposal, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For chronic condition proposal, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For chronic condition proposal, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For chronic condition proposal, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 18. trend proposal [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns trend proposal. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Daily trend points are deferred until an authoritative metric source and
bounded retention are approved.

**Fresh campaign path.** For trend proposal, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For trend proposal, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For trend proposal, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For trend proposal, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For trend proposal, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For trend proposal, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For trend proposal, Trace the fact from producer to destination authority. Show
the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For trend proposal, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 19. patient panel wording [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns patient panel wording. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** AfflictionsPanel maps event kinds to restrained labels and shows only the
last permitted rows after reload.

**Fresh campaign path.** For patient panel wording, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For patient panel wording, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For patient panel wording, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For patient panel wording, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For patient panel wording, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For patient panel wording, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For patient panel wording, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For patient panel wording, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 20. medical-record template disposition [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns medical-record template disposition. Start from
`Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
`Assets/StreamingAssets/Data/medical_record_templates.json` and trace any effect through
`src/Main.Medical.cs` to its current destination owner. Persistence must follow this boundary:
`medical_pipeline section contains the additive bounded record field`. Verify the exact source and
destination sections; a new section requires an ownership decision. Keep the source fact distinct from the
consumer mutation. Catalog evidence: No catalog row directly matches this integration case; verify the
current producer and destination API before implementation.

**Feature-specific acceptance:** Seven template rows are currently unhosted; mark each as unused or justify
a current pipeline consumer before expansion.

**Fresh campaign path.** For medical-record template disposition, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For medical-record template disposition, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For medical-record template disposition, Capture the relevant existing section after
the source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For medical-record template disposition, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For medical-record template disposition, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For medical-record template disposition, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For medical-record template disposition, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For medical-record template disposition, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

## 14. Legacy plan reconciliation register
The original 2026-09-01 task list is preserved as intent here in condensed form. Numbered items that are
already delivered or contradicted by signed authority must be marked DELIVERED or RETIRED. Each entry is a
premise question, never an instruction to create a duplicate class or save section. Current code and the
live ownership ledger decide whether it becomes a claim.
- **L01:** Create `HealthHistorySystem.cs` in `Assets/Ashfall.Core/Medical/`. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `MedicalRecord` DTO: `recordId`, `survivorId`, `recordType`
  (illness/injury/treatment/vaccination/radiation_exposure/chronic_condition/checkup), `recordedDay`,
  `description`, `severity` (mild/moderate/severe/critical), `duration` (days), `outcome`
  (resolved/ongoing/chronic/fatal), `treatmentApplied` (list of treatment_ids), `treatingSurvivorId` (medic
  who treated), `notes` (additional details). Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `HealthEvent` DTO: `eventId`, `survivorId`, `eventType`
  (diagnosis/treatment/recovery/relapse/complication/vaccination/checkup), `eventDay`, `description`,
  `relatedCondition` (condition_id if applicable), `outcome` (success/partial/failure), `notes`. Verify
  against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `VaccinationRecord` DTO: `vaccinationId`, `survivorId`, `vaccineType` (disease_id or
  vaccine_id), `administeredDay`, `administeredBySurvivorId`, `immunityLevel` (0-100), `immunityDuration`
  (days), `boosterDue` (day). Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `HealthTrend` DTO: `trendId`, `survivorId`, `healthMetric`
  (overall_health/radiation_dose/immune_strength/chronic_condition_count), `measurementDay`, `value` (0-100
  or specific value), `trend` (improving/stable/declining). Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Define `HealthHistoryState` DTO: list of medical records per survivor, list of health events,
  list of vaccination records, list of health trends, health history settings (auto-record treatments bool,
  show trends bool). Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination
  owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define medical record types (7+ types):. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define health event types (7+ types):. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define medical record retention:. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define health trend tracking:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Define medic integration:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Define vaccination system:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Add deterministic seeding: health events use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Wire into `GameBootstrap`: `SetupHealthHistory`, `TickHealthHistory`, `SaveHealthHistory`. Verify
  against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement medical record creation:. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement health event tracking:. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement vaccination system:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement health trend tracking:. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement medic integration:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement medical UI:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement record search/filter:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Implement record export:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Implement health alerts:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Create health events:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add health quest hooks:. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement health tutorial: first medical record explains system. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add health tooltips: hover over record shows details. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Create medical record templates in data file. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Implement health persistence: records saved with survivor state. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Wire into `MedicalPipelineCoordinator`: medical records created for treatments. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Connect to `DiseaseSystem`: illness records created for diseases. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Integrate with `RadiationSystem`: radiation exposure records created. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Connect to `DoseLedgerSystem`: radiation dose integrated into health trends. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Wire into `CombatTraumaSystem`: injury records created for combat wounds. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Connect to `ChronicConditionSystem` (Plan 193): chronic condition records created. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Implement old-save compatibility: existing saves get empty health history. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Add deterministic seeding: health events use `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Create exploit prevention: health records are automatic, can't be gamed. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Add tests: medical records, health events, vaccinations, trends, medic integration, save
  round-trip. Verify against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner;
  disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Verify all record types work correctly. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Test edge cases: no records (healthy survivor), extensive records (chronically ill). Verify
  against `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Verify headless behavior: health history processes correctly without UI. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Add data-integrity-selftest: health records validate against medical catalogs. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L45:** Create `--health-history-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract
**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` plus
targeted owner save/host checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.

## Detailed implementation and narrative casebook

Each entry below is a reviewable case within the same bounded feature; it is not a separate new system. Choose the smallest case whose premise survives the live recensus. The casebook combines the master’s prose, mechanics, save, and UI lenses while keeping the existing authority map fixed.

### 001. diagnosis suspected — Origin and witness

For **diagnosis suspected**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 002. diagnosis confirmed — Origin and witness

For **diagnosis confirmed**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 003. patient stabilized — Origin and witness

For **patient stabilized**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 004. patient recovered — Origin and witness

For **patient recovered**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 005. treatment scheduled — Origin and witness

For **treatment scheduled**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 006. treatment completed — Origin and witness

For **treatment completed**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 007. treatment refused — Origin and witness

For **treatment refused**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 008. protocol executed — Origin and witness

For **protocol executed**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 009. inventory treatment append — Origin and witness

For **inventory treatment append**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 010. oldest-first eviction — Origin and witness

For **oldest-first eviction**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 011. per-survivor view — Origin and witness

For **per-survivor view**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 012. old-save baseline — Origin and witness

For **old-save baseline**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 013. restore parity — Origin and witness

For **restore parity**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 014. deceased patient history — Origin and witness

For **deceased patient history**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 015. radiation dose boundary — Origin and witness

For **radiation dose boundary**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 016. vaccination proposal — Origin and witness

For **vaccination proposal**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 017. chronic condition proposal — Origin and witness

For **chronic condition proposal**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 018. trend proposal — Origin and witness

For **trend proposal**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 019. patient panel wording — Origin and witness

For **patient panel wording**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 020. medical-record template disposition — Origin and witness

For **medical-record template disposition**, record the actual witnessable source: one clinical event emitted by MedicalPipelineCoordinator. Write a trace that includes the source ID, subject ID, location or context, campaign day, and the moment the producer considers the event committed. Identify which details a survivor could report and which are hidden simulation values. A preview must be labeled as such. Proposed diegetic copy should name an observation rather than announce an unseen outcome. This avoids making the player believe that an authored line has already changed MedicalRecordLog.Append with day, kind, survivor ID and detail ID only.

### 021. diagnosis suspected — Domain decision

For **diagnosis suspected**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 022. diagnosis confirmed — Domain decision

For **diagnosis confirmed**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 023. patient stabilized — Domain decision

For **patient stabilized**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 024. patient recovered — Domain decision

For **patient recovered**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 025. treatment scheduled — Domain decision

For **treatment scheduled**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 026. treatment completed — Domain decision

For **treatment completed**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 027. treatment refused — Domain decision

For **treatment refused**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 028. protocol executed — Domain decision

For **protocol executed**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 029. inventory treatment append — Domain decision

For **inventory treatment append**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 030. oldest-first eviction — Domain decision

For **oldest-first eviction**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 031. per-survivor view — Domain decision

For **per-survivor view**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 032. old-save baseline — Domain decision

For **old-save baseline**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 033. restore parity — Domain decision

For **restore parity**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 034. deceased patient history — Domain decision

For **deceased patient history**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 035. radiation dose boundary — Domain decision

For **radiation dose boundary**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 036. vaccination proposal — Domain decision

For **vaccination proposal**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 037. chronic condition proposal — Domain decision

For **chronic condition proposal**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 038. trend proposal — Domain decision

For **trend proposal**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 039. patient panel wording — Domain decision

For **patient panel wording**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 040. medical-record template disposition — Domain decision

For **medical-record template disposition**, start from `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` and enumerate legal inputs, range checks, unknown IDs, repeated IDs, and the current destination method. State which return value means an accepted mutation and which means a refusal. If `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` appears to own the same fact, document the overlap field by field and choose the established campaign owner. The review note should give an exact before/after value on MedicalRecordLog.Append with day, kind, survivor ID and detail ID only, not merely report that an event handler fired.

### 041. diagnosis suspected — Save boundary

For **diagnosis suspected**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 042. diagnosis confirmed — Save boundary

For **diagnosis confirmed**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 043. patient stabilized — Save boundary

For **patient stabilized**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 044. patient recovered — Save boundary

For **patient recovered**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 045. treatment scheduled — Save boundary

For **treatment scheduled**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 046. treatment completed — Save boundary

For **treatment completed**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 047. treatment refused — Save boundary

For **treatment refused**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 048. protocol executed — Save boundary

For **protocol executed**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 049. inventory treatment append — Save boundary

For **inventory treatment append**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 050. oldest-first eviction — Save boundary

For **oldest-first eviction**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 051. per-survivor view — Save boundary

For **per-survivor view**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 052. old-save baseline — Save boundary

For **old-save baseline**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 053. restore parity — Save boundary

For **restore parity**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 054. deceased patient history — Save boundary

For **deceased patient history**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 055. radiation dose boundary — Save boundary

For **radiation dose boundary**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 056. vaccination proposal — Save boundary

For **vaccination proposal**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 057. chronic condition proposal — Save boundary

For **chronic condition proposal**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 058. trend proposal — Save boundary

For **trend proposal**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 059. patient panel wording — Save boundary

For **patient panel wording**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 060. medical-record template disposition — Save boundary

For **medical-record template disposition**, capture `medical_pipeline additive record field` immediately before and after the accepted result. Restore both snapshots in new objects and compare the source fact, applied marker, destination state, and visible line. Then simulate a save between source commitment and destination handoff. If a pending result is recoverable, name the field that carries its identity; if it is not, hold the implementation at the architecture gate. An old save lacking the proposed field must take a documented baseline and never invent earlier activity.

### 061. diagnosis suspected — Player surface and restrained copy

For **diagnosis suspected**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 062. diagnosis confirmed — Player surface and restrained copy

For **diagnosis confirmed**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 063. patient stabilized — Player surface and restrained copy

For **patient stabilized**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 064. patient recovered — Player surface and restrained copy

For **patient recovered**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 065. treatment scheduled — Player surface and restrained copy

For **treatment scheduled**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 066. treatment completed — Player surface and restrained copy

For **treatment completed**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 067. treatment refused — Player surface and restrained copy

For **treatment refused**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 068. protocol executed — Player surface and restrained copy

For **protocol executed**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 069. inventory treatment append — Player surface and restrained copy

For **inventory treatment append**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 070. oldest-first eviction — Player surface and restrained copy

For **oldest-first eviction**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 071. per-survivor view — Player surface and restrained copy

For **per-survivor view**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 072. old-save baseline — Player surface and restrained copy

For **old-save baseline**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 073. restore parity — Player surface and restrained copy

For **restore parity**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 074. deceased patient history — Player surface and restrained copy

For **deceased patient history**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 075. radiation dose boundary — Player surface and restrained copy

For **radiation dose boundary**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 076. vaccination proposal — Player surface and restrained copy

For **vaccination proposal**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 077. chronic condition proposal — Player surface and restrained copy

For **chronic condition proposal**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 078. trend proposal — Player surface and restrained copy

For **trend proposal**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 079. patient panel wording — Player surface and restrained copy

For **patient panel wording**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 080. medical-record template disposition — Player surface and restrained copy

For **medical-record template disposition**, the view `src/UI/AfflictionsPanel.cs` should answer what changed, why it changed, what remains uncertain, and what the player can do next. Draft four separate strings: observed state, available action, refusal, and aftermath. Each string must fit a current consumer field or remain a clearly marked proposal. Use ordinary language, distinct speaker perspective where diegetic, and no color-only warning. Reopening or refreshing the panel must not commit a new consequence or reveal a hidden quantity.

### 081. diagnosis suspected — Failure and uncertainty

For **diagnosis suspected**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 082. diagnosis confirmed — Failure and uncertainty

For **diagnosis confirmed**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 083. patient stabilized — Failure and uncertainty

For **patient stabilized**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 084. patient recovered — Failure and uncertainty

For **patient recovered**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 085. treatment scheduled — Failure and uncertainty

For **treatment scheduled**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 086. treatment completed — Failure and uncertainty

For **treatment completed**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 087. treatment refused — Failure and uncertainty

For **treatment refused**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 088. protocol executed — Failure and uncertainty

For **protocol executed**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 089. inventory treatment append — Failure and uncertainty

For **inventory treatment append**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 090. oldest-first eviction — Failure and uncertainty

For **oldest-first eviction**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 091. per-survivor view — Failure and uncertainty

For **per-survivor view**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 092. old-save baseline — Failure and uncertainty

For **old-save baseline**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 093. restore parity — Failure and uncertainty

For **restore parity**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 094. deceased patient history — Failure and uncertainty

For **deceased patient history**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 095. radiation dose boundary — Failure and uncertainty

For **radiation dose boundary**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 096. vaccination proposal — Failure and uncertainty

For **vaccination proposal**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 097. chronic condition proposal — Failure and uncertainty

For **chronic condition proposal**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 098. trend proposal — Failure and uncertainty

For **trend proposal**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 099. patient panel wording — Failure and uncertainty

For **patient panel wording**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 100. medical-record template disposition — Failure and uncertainty

For **medical-record template disposition**, use a missing reference, stale report, dead actor, unavailable item, or already-resolved hazard as the negative fixture most relevant to this domain. Reject before debiting an inventory item, changing standing, adding a condition, changing a role, or registering an encounter. Make the reason visible and stable after reload. If a downstream owner is unavailable, keep the source fact with a pending or refused status; never call the route successful because a journal line printed.

### 101. diagnosis suspected — Cross-system ownership

For **diagnosis suspected**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 102. diagnosis confirmed — Cross-system ownership

For **diagnosis confirmed**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 103. patient stabilized — Cross-system ownership

For **patient stabilized**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 104. patient recovered — Cross-system ownership

For **patient recovered**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 105. treatment scheduled — Cross-system ownership

For **treatment scheduled**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 106. treatment completed — Cross-system ownership

For **treatment completed**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 107. treatment refused — Cross-system ownership

For **treatment refused**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 108. protocol executed — Cross-system ownership

For **protocol executed**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 109. inventory treatment append — Cross-system ownership

For **inventory treatment append**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 110. oldest-first eviction — Cross-system ownership

For **oldest-first eviction**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 111. per-survivor view — Cross-system ownership

For **per-survivor view**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 112. old-save baseline — Cross-system ownership

For **old-save baseline**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 113. restore parity — Cross-system ownership

For **restore parity**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 114. deceased patient history — Cross-system ownership

For **deceased patient history**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 115. radiation dose boundary — Cross-system ownership

For **radiation dose boundary**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 116. vaccination proposal — Cross-system ownership

For **vaccination proposal**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 117. chronic condition proposal — Cross-system ownership

For **chronic condition proposal**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 118. trend proposal — Cross-system ownership

For **trend proposal**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 119. patient panel wording — Cross-system ownership

For **patient panel wording**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 120. medical-record template disposition — Cross-system ownership

For **medical-record template disposition**, trace the change through the currently hosted authority rather than the class named in the old plan. Ask whether a sibling system already records the same event or applies the same modifier. Where two consumers genuinely differ, declare order and caps in the owning domain. Where they are aliases, retain one and make the other a read-only view. This case must respect the specific boundary: Never persist free-text patient notes, export private records, or duplicate the dose ledger. The file map for the handoff should identify every shared Main or registry edit as integrator-owned.

### 121. diagnosis suspected — Deterministic day and replay

For **diagnosis suspected**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 122. diagnosis confirmed — Deterministic day and replay

For **diagnosis confirmed**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 123. patient stabilized — Deterministic day and replay

For **patient stabilized**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 124. patient recovered — Deterministic day and replay

For **patient recovered**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 125. treatment scheduled — Deterministic day and replay

For **treatment scheduled**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 126. treatment completed — Deterministic day and replay

For **treatment completed**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 127. treatment refused — Deterministic day and replay

For **treatment refused**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 128. protocol executed — Deterministic day and replay

For **protocol executed**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 129. inventory treatment append — Deterministic day and replay

For **inventory treatment append**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 130. oldest-first eviction — Deterministic day and replay

For **oldest-first eviction**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 131. per-survivor view — Deterministic day and replay

For **per-survivor view**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 132. old-save baseline — Deterministic day and replay

For **old-save baseline**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 133. restore parity — Deterministic day and replay

For **restore parity**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 134. deceased patient history — Deterministic day and replay

For **deceased patient history**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 135. radiation dose boundary — Deterministic day and replay

For **radiation dose boundary**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 136. vaccination proposal — Deterministic day and replay

For **vaccination proposal**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 137. chronic condition proposal — Deterministic day and replay

For **chronic condition proposal**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 138. trend proposal — Deterministic day and replay

For **trend proposal**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 139. patient panel wording — Deterministic day and replay

For **patient panel wording**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 140. medical-record template disposition — Deterministic day and replay

For **medical-record template disposition**, state whether the input belongs to a player command, hourly update, daily phase, or post-resolution event. Sort multi-entity inputs by stable ID before mutation; use a named seeded RNG stream if the current rule rolls. Repeat the same day and same source ID after a fresh restore. The outcome, applied count, save checksum inputs, and visible status should match. If the rule is a pure projection, show that two reads produce no new event, counter, or save dirtiness.

### 141. diagnosis suspected — Acceptance and prose handoff

For **diagnosis suspected**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 142. diagnosis confirmed — Acceptance and prose handoff

For **diagnosis confirmed**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 143. patient stabilized — Acceptance and prose handoff

For **patient stabilized**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 144. patient recovered — Acceptance and prose handoff

For **patient recovered**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 145. treatment scheduled — Acceptance and prose handoff

For **treatment scheduled**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 146. treatment completed — Acceptance and prose handoff

For **treatment completed**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 147. treatment refused — Acceptance and prose handoff

For **treatment refused**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 148. protocol executed — Acceptance and prose handoff

For **protocol executed**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 149. inventory treatment append — Acceptance and prose handoff

For **inventory treatment append**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 150. oldest-first eviction — Acceptance and prose handoff

For **oldest-first eviction**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 151. per-survivor view — Acceptance and prose handoff

For **per-survivor view**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 152. old-save baseline — Acceptance and prose handoff

For **old-save baseline**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 153. restore parity — Acceptance and prose handoff

For **restore parity**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 154. deceased patient history — Acceptance and prose handoff

For **deceased patient history**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 155. radiation dose boundary — Acceptance and prose handoff

For **radiation dose boundary**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 156. vaccination proposal — Acceptance and prose handoff

For **vaccination proposal**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 157. chronic condition proposal — Acceptance and prose handoff

For **chronic condition proposal**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 158. trend proposal — Acceptance and prose handoff

For **trend proposal**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 159. patient panel wording — Acceptance and prose handoff

For **patient panel wording**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 160. medical-record template disposition — Acceptance and prose handoff

For **medical-record template disposition**, package one accepted path and one refusal with exact IDs, file paths, old/new save states, and the owner value the player sees. The prose handoff should include a short cause/effect brief, not only a scene title, so the writer cannot promise a mechanic the code never commits. The focused proof starts at `Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs` only when that fixture actually covers the selected path. Record an explicit stop condition: if identity, custody, or a signed decision is absent, the case remains a proposal.

### 161. diagnosis suspected — implementation decision record

The builder records the current producer signature, stable source ID, exact `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` command or query, current catalog row, `medical_pipeline additive record field` DTO field, and a before/after readout from `src/UI/AfflictionsPanel.cs`. For this specific case, the decision packet must state whether the prior plan's `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` is DELIVERED, RESIDUAL, BLOCKED, or RETIRED. It must also name the refusal produced when the source is absent and the smallest focused check of restore/replay. A route without a destination owner remains an unimplemented proposal. Candidate prose is reviewed against the observed fact and the owner’s result code before it is promoted to JSON.

### 162. diagnosis confirmed — implementation decision record

The builder records the current producer signature, stable source ID, exact `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs and MedicalRecordLog.cs` command or query, current catalog row, `medical_pipeline additive record field` DTO field, and a before/after readout from `src/UI/AfflictionsPanel.cs`. For this specific case, the decision packet must state whether the prior plan's `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` is DELIVERED, RESIDUAL, BLOCKED, or RETIRED. It must also name the refusal produced when the source is absent and the smallest focused check of restore/replay. A route without a destination owner remains an unimplemented proposal. Candidate prose is reviewed against the observed fact and the owner’s result code before it is promoted to JSON.


## Editorial closeout and implementation handoff

This plan has a current-evidence architecture at the top, a preserved historical scenario inventory, a bounded casebook, and a C# placement framework tied to a real API anchor. The historical material is conditional where it conflicts with the current owner or a signed decision. One implementation package selects one case, claims exact paths, proves source → owner → save → UI, and reports the negative and replay paths. Documentation completion is not a claim that the runtime route is integrated.

## Polished candidate prose tied to real state

These lines are editorial candidates, not authored JSON or proof of a live route. Their trigger notes tell an integrator which owner fact must exist before the line can be used. Each candidate should be checked against the actual field length, localization path, and current panel layout before promotion.

### 1. treatment

> Treatment completed today. The record points to the protocol and the survivor; it contains no bedside note.

**Trigger and restraint:** Render an identifier-only MedicalRecordLog entry after the coordinator emits the fact.

### 2. unresolved case

> The ward ledger still lists the case as active. The next review is a clinical task, not a prediction.

**Trigger and restraint:** Do not infer resolution from a panel refresh.

### 3. dose pointer

> The medical record names the visit. The radiation ledger holds the measured exposure.

**Trigger and restraint:** Do not copy dose numbers into a second history authority.

### 4. privacy

> The public board shows that care is available. The patient record stays in the ward register.

**Trigger and restraint:** No free-text export or broad public history view.

### 5. old save

> Older pages have no recent-event list. The current ledger begins with the first new recorded event.

**Trigger and restraint:** An absent additive record field restores empty, without backfilled events.

### Editorial acceptance

A reader should be able to tell observation from confirmed outcome, type description from instance history, and warning from resolution. Remove any line whose source fact or consumer field cannot be named. Preserve the restrained, human tone of the master expansion document; do not use the proposed copy to smuggle in mechanics or mutable state.
