# Plan 198 — Health History & Medical Records — Signed Pipeline Log
> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
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
