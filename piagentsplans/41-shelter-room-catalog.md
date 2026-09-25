# Plan 41 — Shelter Room Catalog, Assignment Rules and Capacity Semantics

> **Rebuild status:** COMPLETE 23-ROOM/12-RULE CATALOG — SHELTER ASSIGNMENT CORE AND SAVE ARE PRESENT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-5`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round5-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified current architecture and evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The current catalog includes room definitions, costs, capacities and assignment rules.
- ShelterAssignmentSystem exposes assign/unassign, capacity, same-room and capture/restore methods.
- Main.Sanitation and ShelterAssignmentHostSession load the catalog; the current operations board may project room state.
- The old 20-room target is stale and must not drive more rows.

**Bounded outcome:** Retire the no-catalog premise. shelter_rooms.json now has 23 rooms and 12 assignment rules, ShelterRoomCatalogLoader parses it, ShelterAssignmentSystem owns assignment state, and ShelterAssignmentSave persists it. The plan is now a room reachability/assignment-safety audit, not a new shelter geography system.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old count target with a 23-room/12-rule census and field-to-owner map.
- Trace room instances, assignment, capacity bonuses, construction/upgrade and operations-board presentation.
- Keep room catalog definitions, assignment state, construction state and room identity/history in their current owners.

**Master-authority sections applied to this rebase:**

- Part II factory protocol: establish current reality, reject duplicate authority, and name one safe extension seam before design.
- Part II continuity checklist: data presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III cluster map: preserve the current Core owner and route cross-system effects through typed facts rather than panel copies.
- Part VI Multi-Session Growth Protocol: 250k is an evidence-backed depth target, not a mandate to manufacture prose or row count.
- Live source/data authority outranks this plan; a future audit that contradicts a current declaration returns the package to STALE_PLAN.
- Anti-padding rule: preserve completed work as maintenance scope and spend detail only on proven residual gaps.
- Part II current-reality rule: JSON presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III one-authority rule: extend the existing owner and route typed facts through it; do not create a second ledger, save store, registry, simulation, or panel cache.
- Part VI replay rule: any new randomness must use the existing seeded campaign stream and stable ordinal ordering; no System.Random or wall-clock decision path.
- Part VI save rule: a new mutable field is incomplete until CaptureState, RestoreState, old-save defaults, and checksum migration are specified.
- Part VII UI rule: presentation projects owner state and routes real commands; it never becomes a gameplay authority or a fake operational route.
- Part VIII quality rule: the 150k–170k band is an initial completeness checkpoint; 250k is an evidence-backed depth target, not a ceiling or a reason to pad.
- Live source/data evidence outranks the original plan. If a future audit contradicts a declaration here, the package returns to STALE_PLAN rather than reviving an obsolete API.
- The residual is safe room/assignment reachability, not more room definitions.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

Anything beyond this list is a different package. In particular, this plan does not convert a documentation gap into permission to create a second domain owner.

# 4. Current Evidence and Premise Audit

The current evidence set for this plan is enumerated in the appendices with file hashes, declaration digests, catalog schema/counts and focused test inventories. A source declaration proves an API surface exists; it does not prove a fresh test run or live player reachability. Those claims require the verification steps in this document.

**Evidence classes used here:**

- **VERIFIED CURRENT:** the named path exists and its contents were read during this rebuild.
- **HISTORICAL RECORD:** an archived closeout or old plan says a package once landed; it is useful context but is not current pass evidence.
- **PROPOSAL:** a future seam or file shape that requires a new claim and premise recheck.
- **UNKNOWN:** deliberately unresolved because the present plan does not need to invent an answer.

# 5. Existing Extension Seams

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| room and rule definitions | ShelterRoomCatalogLoader | `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs` | Static catalog owner. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | Sole assignment authority. |
| assignment capture/restore | ShelterAssignmentSave | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` | Current save owner. |
| host load/commands/save adapter | ShelterAssignmentHostSession | `src/Host/ShelterAssignmentHostSession.cs` | Thin host seam. |
| catalog composition and current room route | Main.Sanitation | `src/Main.Sanitation.cs` | Current host integration. |
| catalog and assignment contracts | ShelterRoomCatalogTests | `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Shelter Room Catalog, Assignment Rules and Capacity Semantics
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ShelterRoomCatalogLoader
│   room and rule definitions
│ ShelterAssignmentSystem
│   assignment, capacity and same-room state
│ ShelterAssignmentSave
│   assignment capture/restore
│ ShelterAssignmentHostSession
│   host load/commands/save adapter
│ Main.Sanitation
│   catalog composition and current room route
                │
                ▼
Host projection → existing command → owner mutation → typed fact
                │
                ├─ UI / briefing / journal / audio presentation
                ├─ existing save envelope and checksum
                └─ focused Core / host / headless verification
```

The architecture is deliberately projection-first where a read model is sufficient, owner-extension-first where new mutable facts are required, and data-first only when an existing catalog can express the content. It does not permit a new subsystem merely to make the plan look larger.

## 6.1 Architectural decisions

1. **Preserve current state ownership.** ShelterRoomCatalogLoader owns room and rule definitions: Static catalog owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| room and rule definitions | ShelterRoomCatalogLoader | `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs` | Static catalog owner. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | Sole assignment authority. |
| assignment capture/restore | ShelterAssignmentSave | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` | Current save owner. |
| host load/commands/save adapter | ShelterAssignmentHostSession | `src/Host/ShelterAssignmentHostSession.cs` | Thin host seam. |
| catalog composition and current room route | Main.Sanitation | `src/Main.Sanitation.cs` | Current host integration. |
| catalog and assignment contracts | ShelterRoomCatalogTests | `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 23 rooms and 12 rules
2. validate IDs/capacities/costs
3. read current room instances and assignment state
4. present occupancy and rule outcome
5. route assign/unassign through ShelterAssignmentSystem
6. apply current capacity/construction effects
7. capture ShelterAssignmentSave

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Room definitions and rules are immutable catalog data.
- Assignments, occupancy and bonuses belong to ShelterAssignmentSystem.
- Construction/upgrade and room identity/history remain with their current owners.
- No new room state is stored in the catalog or panel.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Unknown room/survivor fails without state change.
- Capacity is checked by the owner, not UI.
- A rule is a modifier only when its required skill/trait is current.
- A failed assignment emits no success event.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- shelter_rooms.json is the room catalog.
- Construction, identity, sanitation and operations catalogs remain separate.
- No duplicate room/assignment registry.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use ShelterAssignmentSave and current shelter operation saves.
- No Plan-41 save section.
- Old assignments referencing missing rooms normalize through current restore rules.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Assignment order is stable; no RNG is required for ordinary assignment.
- Any deterministic tie-break uses stable room/survivor IDs.
- Capture order is stable for checksum stability.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Assignment emits the current ShelterAssignmentEvent.
- Capacity changes must use the current owner path.
- Host refresh projects current assignment state only.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ShelterAssignmentHostSession.cs
- src/Main.Sanitation.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Room history is restrained and fictional.
- Capacity bonuses must describe the current owner, not invented simulation effects.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Catalog count drives an unnecessary new system. | ShelterRoomCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | UI assigns a survivor despite capacity. | ShelterAssignmentSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A rule is treated as a hard gate without current evidence. | ShelterAssignmentSave | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Assignment and construction state are duplicated. | ShelterAssignmentHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Save restore creates phantom occupants. | Main.Sanitation | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 23-room/12-rule census. | Current catalog is exact. | No production path until the owning implementation package is separately claimed. |
| 1 | Assignment/construction/capacity trace. | One owner per mutable concern. | No production path until the owning implementation package is separately claimed. |
| 2 | Save and failure audit. | No phantom room state. | No production path until the owning implementation package is separately claimed. |
| 3 | UI/accessibility polish. | Current commands and refusals are visible. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/shelter_rooms.json | READ ONLY; MODIFY only for content/reference defect | 23 rooms/12 rules |
| Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs | READ ONLY | Assignment authority |
| src/Host/ShelterAssignmentHostSession.cs | READ ONLY | Host seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel room state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| UI capacity authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Combining room identity with construction state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a save section for catalog data. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new room rows.
- No new assignment manager.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future route/readability changes remain save-compatible.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 23/12 counts and all current owners are documented.
- Capacity, restore and accessibility gates are explicit.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

## MUST NOT DO

- No new room rows.
- No new assignment manager.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: room and rule definitions → ShelterRoomCatalogLoader; assignment, capacity and same-room state → ShelterAssignmentSystem; assignment capture/restore → ShelterAssignmentSave; host load/commands/save adapter → ShelterAssignmentHostSession; catalog composition and current room route → Main.Sanitation; catalog and assignment contracts → ShelterRoomCatalogTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 41.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 41 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ShelterRoomCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`

### `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 535 lines / 30549 bytes.
- SHA-256: `6c1eddd2273749e7ed30febe53863f2197b12a48354bf49f0edbbee7eededfd9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterRoomCostDef
public string item_id { get; set; } = string.Empty;
public int quantity { get; set; } = 1;
public sealed class ShelterRoomDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string function { get; set; } = "General"; // Dormitory, Workshop, MedicalBay, Kitchen, Storage, Greenhouse, RadioRoom, Armory, Laboratory, CommonArea, Airlock, GeneratorRoom, FiltrationStack, Corridor
public int capacity { get; set; } = 2;
public int max_upgrade_level { get; set; } = 3;
public string required_skill_id { get; set; } = string.Empty;
public string workstation_id { get; set; } = string.Empty;
public float base_condition { get; set; } = 100.0f;
public List<ShelterRoomCostDef> build_cost { get; set; } = new List<ShelterRoomCostDef>();
public List<ShelterRoomCostDef> repair_cost { get; set; } = new List<ShelterRoomCostDef>();
public List<string> tags { get; set; } = new List<string>();
public sealed class ShelterAssignmentRuleDef
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string target_room_function { get; set; } = string.Empty;
public string required_skill_id { get; set; } = string.Empty;
public string bonus_type { get; set; } = "efficiency";
public float bonus_magnitude { get; set; } = 0.20f;
public float penalty_magnitude { get; set; } = -0.10f;
public bool is_hard_gate { get; set; } = false;
public sealed class ShelterRoomCatalogContainer
public int schema_version { get; set; } = 1;
public string collection_id { get; set; } = "shelter_rooms";
public List<ShelterRoomDef> rooms { get; set; } = new List<ShelterRoomDef>();
public List<ShelterAssignmentRuleDef> assignment_rules { get; set; } = new List<ShelterAssignmentRuleDef>();
public static class ShelterRoomCatalogLoader
public const string CatalogFileName = "shelter_rooms.json";
public static ShelterRoomCatalogContainer Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static ShelterRoomCatalogContainer GetDefaultCatalog() {
public static List<ShelterRoomDef> GetDefaultRooms() {
public static List<ShelterAssignmentRuleDef> GetDefaultRules() {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 388 lines / 15192 bytes.
- SHA-256: `1f43a02984d0efdce124420391e5efeb2b30feabe8069c77decc97c321f020fa`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=3; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterAssignmentSystem
public event Action<ShelterAssignmentEvent>? OnAssignmentChanged;
public IReadOnlyList<ShelterRoom> Rooms => _rooms;
public ShelterAssignmentState State => _state;
public IReadOnlyList<ShelterAssignment> GetAssignments() => _state.Assignments;
public ShelterAssignment? GetAssignmentForSurvivor(string survivorId) {
public IReadOnlyList<ShelterAssignment> GetAssignmentsForRoom(string roomId) {
public int GetRoomOccupancy(string roomId) {
public bool AreInSameRoom(string? survivorA, string? survivorB) {
public int GetRoomCapacity(string roomId) {
public void ApplyCapacityBonuses(IReadOnlyDictionary<string, int> capacityBonuses) {
public bool CanAssign(string survivorId, string roomId) {
public ShelterAssignmentResult Assign(string survivorId, string roomId, string? workstationId = null, int day = 0) {
public ShelterAssignmentResult Unassign(string survivorId, int day = 0) {
public ShelterAssignmentState CaptureState() => _state.Capture();
public void RestoreState(ShelterAssignmentState state) {
public sealed class ShelterRoom
public string RoomId;
public string DisplayName;
public int Capacity;
public string RequiredSkillId; // optional gating; empty = no requirement
public string WorkstationId; // optional default workstation
public sealed class ShelterAssignment
public string SurvivorId;
public string RoomId;
public string WorkstationId;
public int AssignedDay;
public ShelterAssignmentStatus Status;
public enum ShelterAssignmentStatus
public sealed class ShelterAssignmentState
public List<ShelterAssignment> Assignments = new List<ShelterAssignment>();
public void NormalizeAndValidate(IReadOnlyList<ShelterRoom> rooms) {
public ShelterAssignmentState Capture() {
public void RestoreInto(ShelterAssignmentState state, IReadOnlyList<ShelterRoom> rooms) {
public enum ShelterAssignmentEventKind
public sealed class ShelterAssignmentEvent
public ShelterAssignmentEventKind Kind;
public string SurvivorId;
public string RoomId;
public int Day;
public sealed class ShelterAssignmentResult
public bool Succeeded;
public string ReasonCode;
public ShelterAssignment Assignment;
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`

### `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 79 lines / 3350 bytes.
- SHA-256: `0cf98c21764ca49c6dadf211d2aec0ae053cf186b60b89b8a507deb2a3f42aa9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ShelterAssignmentSave
public const int CurrentSaveVersion = 1;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public List<ShelterRoomSave> Rooms = new List<ShelterRoomSave>();
public ShelterAssignmentState State = new ShelterAssignmentState();
public string Checksum = string.Empty;
public sealed class ShelterRoomSave
public string RoomId;
public string DisplayName;
public int Capacity;
public string RequiredSkillId;
public string WorkstationId;
public static class ShelterAssignmentSaveCodec
public static ShelterAssignmentSave Encode(ShelterAssignmentSave save, IJsonSerializer json) {
public static string EncodeToString(ShelterAssignmentSave save, IJsonSerializer json) {
public static ShelterAssignmentSave Decode(string jsonText, IJsonSerializer json) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/ShelterAssignmentHostSession.cs`

### `src/Host/ShelterAssignmentHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 182 lines / 6888 bytes.
- SHA-256: `0e283e269d5b0be549d9b0077055df77c4ccc131db4fbfd5aa2ea5bbbd6fe646`.
- Architecture signals: seeded references=3; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterAssignmentHostSession
public ShelterAssignmentSystem System { get; private set; }
public static ShelterAssignmentHostSession CreateDefault(ISeededRng rng, string? dataDir = null) {
public bool AreInSameRoom(string? a, string? b) => System?.AreInSameRoom(a, b) ?? false;
public bool TrySave() {
public bool TryLoad() {
public override void Save() {
public static class ShelterAssignmentSaveStore
public const string FileName = "shelter_assignment_save.json";
public const string SectionName = "shelter_assignment";
public static string TryCaptureDirect(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestoreDirect(string json) {
public static string TryCapture(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestore(string json) {
public static string SavePath => SaveSlotRoot.Resolve(FileName);
public static bool TrySave(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryLoad() {
public static string TryCapturePersisted(ShelterAssignmentSave save) => TryCapture(save);
```


# Appendix B.06 — Current Code Architecture: `src/Host/ShelterAssignmentHostSession.cs`

### `src/Host/ShelterAssignmentHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 182 lines / 6888 bytes.
- SHA-256: `0e283e269d5b0be549d9b0077055df77c4ccc131db4fbfd5aa2ea5bbbd6fe646`.
- Architecture signals: seeded references=3; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterAssignmentHostSession
public ShelterAssignmentSystem System { get; private set; }
public static ShelterAssignmentHostSession CreateDefault(ISeededRng rng, string? dataDir = null) {
public bool AreInSameRoom(string? a, string? b) => System?.AreInSameRoom(a, b) ?? false;
public bool TrySave() {
public bool TryLoad() {
public override void Save() {
public static class ShelterAssignmentSaveStore
public const string FileName = "shelter_assignment_save.json";
public const string SectionName = "shelter_assignment";
public static string TryCaptureDirect(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestoreDirect(string json) {
public static string TryCapture(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestore(string json) {
public static string SavePath => SaveSlotRoot.Resolve(FileName);
public static bool TrySave(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryLoad() {
public static string TryCapturePersisted(ShelterAssignmentSave save) => TryCapture(save);
```


# Appendix B.07 — Current Code Architecture: `src/Main.Sanitation.cs`

### `src/Main.Sanitation.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 125 lines / 5202 bytes.
- SHA-256: `29709b443b23f186819429e3ab59ffb953b7a0a9e60f356062a24a973fa9ff51`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public SanitationHostSession EnsureSanitationSession() {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/shelter_rooms.json`

### `Assets/StreamingAssets/Data/shelter_rooms.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20439 bytes / 20439 characters.
- SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`.
- Root keys: `assignment_rules`, `collection_id`, `rooms`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
assignment_rules: min=12, max=12, observed_paths=1
rooms: min=23, max=23, observed_paths=1
rooms[].build_cost: min=1, max=2, observed_paths=2
rooms[].repair_cost: min=1, max=1, observed_paths=2
rooms[].tags: min=2, max=3, observed_paths=2
```

Representative record fields:

- `base_condition`
- `build_cost`
- `capacity`
- `description`
- `display_name`
- `function`
- `id`
- `max_upgrade_level`
- `repair_cost`
- `required_skill_id`
- `tags`
- `workstation_id`

Representative identifiers (ordered, capped for readability):

```text
room_bunker_corridor
room_bunks_crowded
room_bunks
room_quarters_private
room_workshop
room_workshop_heavy
room_workshop_precision
room_clinic
room_ward_clinical
room_ward_quarantine
room_kitchen
room_storage_bay
room_storage_secure
room_greenhouse_shelter
room_radio_tuner
room_armory_munitions
room_laboratory_research
room_common_mess_hall
room_reading_quiet_room
room_airlock
room_generator
room_filtration
room_hope_beacon
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/shelter_room_identities.json`

### `Assets/StreamingAssets/Data/shelter_room_identities.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 54024 bytes / 54024 characters.
- SHA-256: `e294ef0d24b40c42a9fee8c1650ee6cc3778daa5c89a2f69cea6bcce02f3be90`.
- Root keys: `collection_id`, `fixtures`, `rooms`, `schema_version`, `vignettes`.

Array-path census (minimum, maximum, observed rows):

```text
fixtures: min=53, max=53, observed_paths=1
rooms: min=13, max=13, observed_paths=1
rooms[].fixture_ids: min=4, max=4, observed_paths=2
rooms[].legacy_aliases: min=0, max=0, observed_paths=2
vignettes: min=21, max=21, observed_paths=1
```

Representative record fields:

- `current_use`
- `display_name`
- `fixture_ids`
- `former_use`
- `id`
- `inspection_summary`
- `legacy_aliases`
- `one_line_history`

Representative identifiers (ordered, capped for readability):

```text
room_bunker_corridor
room_storage_bay
room_bunks
room_kitchen
room_clinic
room_workshop
room_filtration
room_airlock
room_radio_tuner
room_foundry
room_greenhouse
room_main
room_water_pump
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 166; SHA-256: `5121939d1dbac0de2d70af77bb20e6f8e34687a61d5d0ece18179cb7e4fe12d3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DefaultCatalog_Contains22RoomsAnd12Rules
LoadFromFile_ParsesCorrectly
AllRoomIds_AreUniqueAndValidPrefix
AllRuleIds_AreUniqueAndValidPrefix
AssignmentRules_TargetValidFunctions
ShelterAssignmentSystem_LoadsFromCatalogRooms
DormitoryVariants_ReflectCapacityAndCostTradeoffs
WorkshopVariants_TargetDistinctDisciplines
SaveRoundTrip_WithCatalogRooms_PreservesState
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`

- Current test declarations: Fact=20, Theory=0, InlineData=0.
- File lines: 333; SHA-256: `64251d808239453605848adcddd7bf184ee0d0ffbcd79f7a0ffaa806c453ad98`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Constructor_CapturedStateAndRooms_DoNotAliasAuthority
Capture_AssignmentObject_IsSnapshot
Restore_CapturedState_DoesNotAliasInput
InactiveAssignments_DoNotConsumeRoomCapacityOrOccupancy
Assign_ReactivatesInactiveAssignment
Constructor_NullAssignmentList_FailsClosed
Assign_AddsAssignment
Assign_UnknownRoomFails
Assign_AlreadyAssignedFails
Assign_RoomFullFails
CanAssign_FalseWhenFull
Unassign_RemovesAssignment
Unassign_NotAssignedFails
GetOccupancy_AfterMultipleAssigns
Events_FireOnAssignAndUnassign
CaptureRestore_RoundTrip
Save_RoundTrip_ChecksumStable
Save_TamperedChecksumRejected
Save_EmptyChecksumRejected
DeDuplicates_SurvivorAssignments_OnNormalize
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 272; SHA-256: `f631579fb7a4dbb6372a734563633c1f61efa24ef5b0cb5c2ec9054d54f304f9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AreInSameRoom_NullOrEmptyOrSelf_ReturnsFalse
AreInSameRoom_BothAssignedToSameRoom_ReturnsTrue
AreInSameRoom_DifferentRooms_ReturnsFalse
AreInSameRoom_UnassignedSurvivor_ReturnsFalse
AreInSameRoom_InactiveOrDecommissionedAssignment_ReturnsFalse
Flashback_GroundedWhenCompanionInSameRoom
Flashback_UngroundedWhenCompanionsInDifferentRooms
Flashback_UngroundedWhenCompanionsUnassigned
Flashback_UngroundedWhenCompanionIsDead
Flashback_Reassignment_UpdatesProximityDynamically
Flashback_SaveRestoreRoundtrip_PreservesGroundingBehavior
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`

### `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 535 lines / 30549 bytes.
- SHA-256: `6c1eddd2273749e7ed30febe53863f2197b12a48354bf49f0edbbee7eededfd9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterRoomCostDef
public string item_id { get; set; } = string.Empty;
public int quantity { get; set; } = 1;
public sealed class ShelterRoomDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string function { get; set; } = "General"; // Dormitory, Workshop, MedicalBay, Kitchen, Storage, Greenhouse, RadioRoom, Armory, Laboratory, CommonArea, Airlock, GeneratorRoom, FiltrationStack, Corridor
public int capacity { get; set; } = 2;
public int max_upgrade_level { get; set; } = 3;
public string required_skill_id { get; set; } = string.Empty;
public string workstation_id { get; set; } = string.Empty;
public float base_condition { get; set; } = 100.0f;
public List<ShelterRoomCostDef> build_cost { get; set; } = new List<ShelterRoomCostDef>();
public List<ShelterRoomCostDef> repair_cost { get; set; } = new List<ShelterRoomCostDef>();
public List<string> tags { get; set; } = new List<string>();
public sealed class ShelterAssignmentRuleDef
public string id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string target_room_function { get; set; } = string.Empty;
public string required_skill_id { get; set; } = string.Empty;
public string bonus_type { get; set; } = "efficiency";
public float bonus_magnitude { get; set; } = 0.20f;
public float penalty_magnitude { get; set; } = -0.10f;
public bool is_hard_gate { get; set; } = false;
public sealed class ShelterRoomCatalogContainer
public int schema_version { get; set; } = 1;
public string collection_id { get; set; } = "shelter_rooms";
public List<ShelterRoomDef> rooms { get; set; } = new List<ShelterRoomDef>();
public List<ShelterAssignmentRuleDef> assignment_rules { get; set; } = new List<ShelterAssignmentRuleDef>();
public static class ShelterRoomCatalogLoader
public const string CatalogFileName = "shelter_rooms.json";
public static ShelterRoomCatalogContainer Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static ShelterRoomCatalogContainer GetDefaultCatalog() {
public static List<ShelterRoomDef> GetDefaultRooms() {
public static List<ShelterAssignmentRuleDef> GetDefaultRules() {
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 388 lines / 15192 bytes.
- SHA-256: `1f43a02984d0efdce124420391e5efeb2b30feabe8069c77decc97c321f020fa`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=3; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterAssignmentSystem
public event Action<ShelterAssignmentEvent>? OnAssignmentChanged;
public IReadOnlyList<ShelterRoom> Rooms => _rooms;
public ShelterAssignmentState State => _state;
public IReadOnlyList<ShelterAssignment> GetAssignments() => _state.Assignments;
public ShelterAssignment? GetAssignmentForSurvivor(string survivorId) {
public IReadOnlyList<ShelterAssignment> GetAssignmentsForRoom(string roomId) {
public int GetRoomOccupancy(string roomId) {
public bool AreInSameRoom(string? survivorA, string? survivorB) {
public int GetRoomCapacity(string roomId) {
public void ApplyCapacityBonuses(IReadOnlyDictionary<string, int> capacityBonuses) {
public bool CanAssign(string survivorId, string roomId) {
public ShelterAssignmentResult Assign(string survivorId, string roomId, string? workstationId = null, int day = 0) {
public ShelterAssignmentResult Unassign(string survivorId, int day = 0) {
public ShelterAssignmentState CaptureState() => _state.Capture();
public void RestoreState(ShelterAssignmentState state) {
public sealed class ShelterRoom
public string RoomId;
public string DisplayName;
public int Capacity;
public string RequiredSkillId; // optional gating; empty = no requirement
public string WorkstationId; // optional default workstation
public sealed class ShelterAssignment
public string SurvivorId;
public string RoomId;
public string WorkstationId;
public int AssignedDay;
public ShelterAssignmentStatus Status;
public enum ShelterAssignmentStatus
public sealed class ShelterAssignmentState
public List<ShelterAssignment> Assignments = new List<ShelterAssignment>();
public void NormalizeAndValidate(IReadOnlyList<ShelterRoom> rooms) {
public ShelterAssignmentState Capture() {
public void RestoreInto(ShelterAssignmentState state, IReadOnlyList<ShelterRoom> rooms) {
public enum ShelterAssignmentEventKind
public sealed class ShelterAssignmentEvent
public ShelterAssignmentEventKind Kind;
public string SurvivorId;
public string RoomId;
public int Day;
public sealed class ShelterAssignmentResult
public bool Succeeded;
public string ReasonCode;
public ShelterAssignment Assignment;
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`

### `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 79 lines / 3350 bytes.
- SHA-256: `0cf98c21764ca49c6dadf211d2aec0ae053cf186b60b89b8a507deb2a3f42aa9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ShelterAssignmentSave
public const int CurrentSaveVersion = 1;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public List<ShelterRoomSave> Rooms = new List<ShelterRoomSave>();
public ShelterAssignmentState State = new ShelterAssignmentState();
public string Checksum = string.Empty;
public sealed class ShelterRoomSave
public string RoomId;
public string DisplayName;
public int Capacity;
public string RequiredSkillId;
public string WorkstationId;
public static class ShelterAssignmentSaveCodec
public static ShelterAssignmentSave Encode(ShelterAssignmentSave save, IJsonSerializer json) {
public static string EncodeToString(ShelterAssignmentSave save, IJsonSerializer json) {
public static ShelterAssignmentSave Decode(string jsonText, IJsonSerializer json) {
```


# Appendix E.16 — Supporting Code Evidence: `src/Host/ShelterAssignmentHostSession.cs`

### `src/Host/ShelterAssignmentHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 182 lines / 6888 bytes.
- SHA-256: `0e283e269d5b0be549d9b0077055df77c4ccc131db4fbfd5aa2ea5bbbd6fe646`.
- Architecture signals: seeded references=3; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterAssignmentHostSession
public ShelterAssignmentSystem System { get; private set; }
public static ShelterAssignmentHostSession CreateDefault(ISeededRng rng, string? dataDir = null) {
public bool AreInSameRoom(string? a, string? b) => System?.AreInSameRoom(a, b) ?? false;
public bool TrySave() {
public bool TryLoad() {
public override void Save() {
public static class ShelterAssignmentSaveStore
public const string FileName = "shelter_assignment_save.json";
public const string SectionName = "shelter_assignment";
public static string TryCaptureDirect(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestoreDirect(string json) {
public static string TryCapture(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestore(string json) {
public static string SavePath => SaveSlotRoot.Resolve(FileName);
public static bool TrySave(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryLoad() {
public static string TryCapturePersisted(ShelterAssignmentSave save) => TryCapture(save);
```


# Appendix E.17 — Supporting Code Evidence: `src/Host/ShelterAssignmentHostSession.cs`

### `src/Host/ShelterAssignmentHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 182 lines / 6888 bytes.
- SHA-256: `0e283e269d5b0be549d9b0077055df77c4ccc131db4fbfd5aa2ea5bbbd6fe646`.
- Architecture signals: seeded references=3; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterAssignmentHostSession
public ShelterAssignmentSystem System { get; private set; }
public static ShelterAssignmentHostSession CreateDefault(ISeededRng rng, string? dataDir = null) {
public bool AreInSameRoom(string? a, string? b) => System?.AreInSameRoom(a, b) ?? false;
public bool TrySave() {
public bool TryLoad() {
public override void Save() {
public static class ShelterAssignmentSaveStore
public const string FileName = "shelter_assignment_save.json";
public const string SectionName = "shelter_assignment";
public static string TryCaptureDirect(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestoreDirect(string json) {
public static string TryCapture(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryRestore(string json) {
public static string SavePath => SaveSlotRoot.Resolve(FileName);
public static bool TrySave(ShelterAssignmentSave save) {
public static ShelterAssignmentSave? TryLoad() {
public static string TryCapturePersisted(ShelterAssignmentSave save) => TryCapture(save);
```


# Appendix F.18 — Supporting Data Evidence: `Assets/StreamingAssets/Data/shelter_rooms.json`

### `Assets/StreamingAssets/Data/shelter_rooms.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20439 bytes / 20439 characters.
- SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`.
- Root keys: `assignment_rules`, `collection_id`, `rooms`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
assignment_rules: min=12, max=12, observed_paths=1
rooms: min=23, max=23, observed_paths=1
rooms[].build_cost: min=1, max=2, observed_paths=2
rooms[].repair_cost: min=1, max=1, observed_paths=2
rooms[].tags: min=2, max=3, observed_paths=2
```

Representative record fields:

- `base_condition`
- `build_cost`
- `capacity`
- `description`
- `display_name`
- `function`
- `id`
- `max_upgrade_level`
- `repair_cost`
- `required_skill_id`
- `tags`
- `workstation_id`

Representative identifiers (ordered, capped for readability):

```text
room_bunker_corridor
room_bunks_crowded
room_bunks
room_quarters_private
room_workshop
room_workshop_heavy
room_workshop_precision
room_clinic
room_ward_clinical
room_ward_quarantine
room_kitchen
room_storage_bay
room_storage_secure
room_greenhouse_shelter
room_radio_tuner
room_armory_munitions
room_laboratory_research
room_common_mess_hall
room_reading_quiet_room
room_airlock
room_generator
room_filtration
room_hope_beacon
```


# Appendix F.19 — Supporting Data Evidence: `Assets/StreamingAssets/Data/shelter_room_identities.json`

### `Assets/StreamingAssets/Data/shelter_room_identities.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 54024 bytes / 54024 characters.
- SHA-256: `e294ef0d24b40c42a9fee8c1650ee6cc3778daa5c89a2f69cea6bcce02f3be90`.
- Root keys: `collection_id`, `fixtures`, `rooms`, `schema_version`, `vignettes`.

Array-path census (minimum, maximum, observed rows):

```text
fixtures: min=53, max=53, observed_paths=1
rooms: min=13, max=13, observed_paths=1
rooms[].fixture_ids: min=4, max=4, observed_paths=2
rooms[].legacy_aliases: min=0, max=0, observed_paths=2
vignettes: min=21, max=21, observed_paths=1
```

Representative record fields:

- `current_use`
- `display_name`
- `fixture_ids`
- `former_use`
- `id`
- `inspection_summary`
- `legacy_aliases`
- `one_line_history`

Representative identifiers (ordered, capped for readability):

```text
room_bunker_corridor
room_storage_bay
room_bunks
room_kitchen
room_clinic
room_workshop
room_filtration
room_airlock
room_radio_tuner
room_foundry
room_greenhouse
room_main
room_water_pump
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 166; SHA-256: `5121939d1dbac0de2d70af77bb20e6f8e34687a61d5d0ece18179cb7e4fe12d3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DefaultCatalog_Contains22RoomsAnd12Rules
LoadFromFile_ParsesCorrectly
AllRoomIds_AreUniqueAndValidPrefix
AllRuleIds_AreUniqueAndValidPrefix
AssignmentRules_TargetValidFunctions
ShelterAssignmentSystem_LoadsFromCatalogRooms
DormitoryVariants_ReflectCapacityAndCostTradeoffs
WorkshopVariants_TargetDistinctDisciplines
SaveRoundTrip_WithCatalogRooms_PreservesState
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`

- Current test declarations: Fact=20, Theory=0, InlineData=0.
- File lines: 333; SHA-256: `64251d808239453605848adcddd7bf184ee0d0ffbcd79f7a0ffaa806c453ad98`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Constructor_CapturedStateAndRooms_DoNotAliasAuthority
Capture_AssignmentObject_IsSnapshot
Restore_CapturedState_DoesNotAliasInput
InactiveAssignments_DoNotConsumeRoomCapacityOrOccupancy
Assign_ReactivatesInactiveAssignment
Constructor_NullAssignmentList_FailsClosed
Assign_AddsAssignment
Assign_UnknownRoomFails
Assign_AlreadyAssignedFails
Assign_RoomFullFails
CanAssign_FalseWhenFull
Unassign_RemovesAssignment
Unassign_NotAssignedFails
GetOccupancy_AfterMultipleAssigns
Events_FireOnAssignAndUnassign
CaptureRestore_RoundTrip
Save_RoundTrip_ChecksumStable
Save_TamperedChecksumRejected
Save_EmptyChecksumRejected
DeDuplicates_SurvivorAssignments_OnNormalize
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 272; SHA-256: `f631579fb7a4dbb6372a734563633c1f61efa24ef5b0cb5c2ec9054d54f304f9`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AreInSameRoom_NullOrEmptyOrSelf_ReturnsFalse
AreInSameRoom_BothAssignedToSameRoom_ReturnsTrue
AreInSameRoom_DifferentRooms_ReturnsFalse
AreInSameRoom_UnassignedSurvivor_ReturnsFalse
AreInSameRoom_InactiveOrDecommissionedAssignment_ReturnsFalse
Flashback_GroundedWhenCompanionInSameRoom
Flashback_UngroundedWhenCompanionsInDifferentRooms
Flashback_UngroundedWhenCompanionsUnassigned
Flashback_UngroundedWhenCompanionIsDead
Flashback_Reassignment_UpdatesProximityDynamically
Flashback_SaveRestoreRoundtrip_PreservesGroundingBehavior
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| room and rule definitions | ShelterRoomCatalogLoader | assignment, capacity and same-room state | ShelterAssignmentSystem | Owner emits/reads a typed fact; no mirror state. |
| room and rule definitions | ShelterRoomCatalogLoader | assignment capture/restore | ShelterAssignmentSave | Owner emits/reads a typed fact; no mirror state. |
| room and rule definitions | ShelterRoomCatalogLoader | host load/commands/save adapter | ShelterAssignmentHostSession | Owner emits/reads a typed fact; no mirror state. |
| room and rule definitions | ShelterRoomCatalogLoader | catalog composition and current room route | Main.Sanitation | Owner emits/reads a typed fact; no mirror state. |
| room and rule definitions | ShelterRoomCatalogLoader | catalog and assignment contracts | ShelterRoomCatalogTests | Owner emits/reads a typed fact; no mirror state. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | room and rule definitions | ShelterRoomCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | assignment capture/restore | ShelterAssignmentSave | Owner emits/reads a typed fact; no mirror state. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | host load/commands/save adapter | ShelterAssignmentHostSession | Owner emits/reads a typed fact; no mirror state. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | catalog composition and current room route | Main.Sanitation | Owner emits/reads a typed fact; no mirror state. |
| assignment, capacity and same-room state | ShelterAssignmentSystem | catalog and assignment contracts | ShelterRoomCatalogTests | Owner emits/reads a typed fact; no mirror state. |
| assignment capture/restore | ShelterAssignmentSave | room and rule definitions | ShelterRoomCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| assignment capture/restore | ShelterAssignmentSave | assignment, capacity and same-room state | ShelterAssignmentSystem | Owner emits/reads a typed fact; no mirror state. |
| assignment capture/restore | ShelterAssignmentSave | host load/commands/save adapter | ShelterAssignmentHostSession | Owner emits/reads a typed fact; no mirror state. |
| assignment capture/restore | ShelterAssignmentSave | catalog composition and current room route | Main.Sanitation | Owner emits/reads a typed fact; no mirror state. |
| assignment capture/restore | ShelterAssignmentSave | catalog and assignment contracts | ShelterRoomCatalogTests | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save adapter | ShelterAssignmentHostSession | room and rule definitions | ShelterRoomCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save adapter | ShelterAssignmentHostSession | assignment, capacity and same-room state | ShelterAssignmentSystem | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save adapter | ShelterAssignmentHostSession | assignment capture/restore | ShelterAssignmentSave | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save adapter | ShelterAssignmentHostSession | catalog composition and current room route | Main.Sanitation | Owner emits/reads a typed fact; no mirror state. |
| host load/commands/save adapter | ShelterAssignmentHostSession | catalog and assignment contracts | ShelterRoomCatalogTests | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current room route | Main.Sanitation | room and rule definitions | ShelterRoomCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current room route | Main.Sanitation | assignment, capacity and same-room state | ShelterAssignmentSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current room route | Main.Sanitation | assignment capture/restore | ShelterAssignmentSave | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current room route | Main.Sanitation | host load/commands/save adapter | ShelterAssignmentHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog composition and current room route | Main.Sanitation | catalog and assignment contracts | ShelterRoomCatalogTests | Owner emits/reads a typed fact; no mirror state. |
| catalog and assignment contracts | ShelterRoomCatalogTests | room and rule definitions | ShelterRoomCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog and assignment contracts | ShelterRoomCatalogTests | assignment, capacity and same-room state | ShelterAssignmentSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and assignment contracts | ShelterRoomCatalogTests | assignment capture/restore | ShelterAssignmentSave | Owner emits/reads a typed fact; no mirror state. |
| catalog and assignment contracts | ShelterRoomCatalogTests | host load/commands/save adapter | ShelterAssignmentHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog and assignment contracts | ShelterRoomCatalogTests | catalog composition and current room route | Main.Sanitation | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Current evidence requires a bounded owner/reachability audit; no new authority is implied. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

Every requirement in the objective and delta must resolve to at least one current owner, one negative condition and one future focused verification. A requirement with no owner is removed or returned as `STALE_PLAN`.

# Appendix K — Status and Evidence Labels

| Claim type | Label | Meaning |
| --- | --- | --- |
| Current source | VERIFIED PATH INVENTORY | Appendices hash current files; declarations are read-only. |
| Historical completion | HISTORICAL, NOT FRESH TEST PROOF | Focused tests must be rerun by an implementation/verification package. |
| Proposed types/paths | PROPOSAL ONLY | Never shown as current evidence; requires a new claim. |
| Character count | COMPLETENESS CHECK ONLY | External verifier records it; no padding or repeated boilerplate. |

# Appendix L — Plan Maintenance and Re-Audit Triggers

Re-run the premise sweep when any of the following occurs:

1. A listed Core owner is renamed, split, merged or removed.
2. A listed catalog changes `schema_version`, root shape or consumer.
3. A save section, checksum contract or campaign-day order changes.
4. A listed test is removed, renamed or moved to quarantine.
5. A live ledger marks a surface sealed, retired, accepted or blocked.
6. A generated architecture/save/catalog matrix changes the owner relationship.
7. A new active claim touches any current or proposed path.

The re-audit records only changed evidence. Historical prose is not rewritten merely to appear current, and current evidence is not deleted merely because an old plan disagrees with it.

# Appendix M — Definition of a Safe No-Change Result

A safe no-change result is valid when the current implementation already satisfies the requested behavior. It records: current owner paths, focused tests that exist, any rerun performed by a future verification package, and the precise condition that would justify reopening. A no-change result does not create a placeholder subsystem, a synthetic integration framework, or a test solely to increase counts.

# Appendix N — Final Precision Checklist

- [ ] Every current path in this document exists or is explicitly labeled unavailable.
- [ ] Every proposed path/type is labeled `PROPOSAL` and excluded from current claims.
- [ ] No current owner is duplicated.
- [ ] Core architecture remains engine-free.
- [ ] JSON is described as data authority, not automatic reachability.
- [ ] Save impact names the current owner and migration behavior.
- [ ] Determinism names streams or explicitly states no randomness.
- [ ] UI remains presentation over owner commands.
- [ ] Tests are focused and current commands use `scripts/run_test.sh` policy.
- [ ] Sealed/retired/blocked ledger decisions are respected.
- [ ] No full-suite result is claimed without a dedicated execution window.
- [ ] No Unity dependency or historical architecture is proposed.
- [ ] Character count is not used as evidence of quality.
- [ ] The first implementation step is a premise recheck, not code creation.
- [ ] The implementation handoff can be executed without reinterpreting ownership.

# Appendix O — Handoff Record Template

```text
Package:
Current status rechecked:
Outcome implemented:
Files changed:
Current owner contract used:
Save section/version touched:
Determinism streams touched:
Focused verification commands and results:
Tests reused/added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for independent sweep: yes/no
```


# Appendix — Master authority re-read

# Appendix — Master Authority Re-read Record

The following excerpts were selected from the live master authority by this plan's subject terms. They are planning constraints, not claims that the historical backlog is current.

> **Scale honesty clause.** The requested target for this expansion effort is two million characters. A single authoring pass cannot responsibly produce two million characters of *verified* planning content, and the repository's own constitution (Part 0.4 of v1.0; `AGENTS.md` rules 7–8) forbids manufacturing padded work. v2.0 therefore defines a Multi-Session Growth Protocol (Part VI): the factory is designed to be *appended* session by session, each session adding one or more verified volumes (expanded subsystem deep maps, prose spec libraries, backlog batches), until the corpus reaches the target size organically. The Part VI protocol is the only sanctioned path to the target; bulk generation of unverified prose is a NON-CANON act.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
`Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.

> **DR-09 — Branch and agent sprawl. VERIFIED.**
The repository carries numerous agent- and CI-generated branches (`Zcode_Branch`, `bug_fixing_main`, multiple `chore/*` and `ci-autogen-*` branches) and a wide set of per-tool agent rulebooks at root (`CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GEMINI.md`, `GOOSE.md`, `MIMOCODE.md`, `QWEN.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, `.zcode/`). Consequence: multi-agent discipline (worktree ownership, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`) is not optional; every factory-generated plan must carry an ownership-claim step. No expansion plan may assume it is the only writer.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> **Step 3 — Pull the cell's opening archetype and instantiate it.**
Each matrix cell names an archetype (the *kind* of expansion that cell supports, with its owning seams). The session instantiates the archetype against current evidence: which catalog, which loader, which host session, which save family, which panel. If the archetype's seams no longer exist as described, the cell is stale — record the correction in the Drift Register and pick again.

> **Step 5 — Run the continuity and anti-duplication checklist.**
The v1.0 checklist (Part 13.2) applies in full, plus two factory additions: (a) duplication firewall — prove the candidate does not duplicate any live catalog, system, or `docs/` authority map; (b) unclaimed-content check — if the candidate's content domain appears in `UNCLAIMED_CORPUS_CENSUS.md`, the plan must wire the unclaimed content first or explain why new content outranks it.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 41: Shelter Room Catalog, Assignment Rules and Capacity Semantics.

- **room and rule definitions** remains with `ShelterRoomCatalogLoader` at `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`. Static catalog owner.
- **assignment, capacity and same-room state** remains with `ShelterAssignmentSystem` at `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`. Sole assignment authority.
- **assignment capture/restore** remains with `ShelterAssignmentSave` at `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`. Current save owner.
- **host load/commands/save adapter** remains with `ShelterAssignmentHostSession` at `src/Host/ShelterAssignmentHostSession.cs`. Thin host seam.
- **catalog composition and current room route** remains with `Main.Sanitation` at `src/Main.Sanitation.cs`. Current host integration.
- **catalog and assignment contracts** remains with `ShelterRoomCatalogTests` at `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 23 rooms and 12 rules
2. validate IDs/capacities/costs
3. read current room instances and assignment state
4. present occupancy and rule outcome
5. route assign/unassign through ShelterAssignmentSystem
6. apply current capacity/construction effects
7. capture ShelterAssignmentSave

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Room definitions and rules are immutable catalog data.
- Assignments, occupancy and bonuses belong to ShelterAssignmentSystem.
- Construction/upgrade and room identity/history remain with their current owners.
- No new room state is stored in the catalog or panel.

- Unknown room/survivor fails without state change.
- Capacity is checked by the owner, not UI.
- A rule is a modifier only when its required skill/trait is current.
- A failed assignment emits no success event.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/ShelterAssignmentHostSession.cs
- src/Main.Sanitation.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs
- Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs
- Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs

These commands are intentionally small and named. A planning rebuild does not run them and does not convert historical pass counts into fresh evidence. An implementation package records the actual command, result, fixture and limitation.

## F. Precision questions for the next owner

- Which current method is the single mutation point for each durable fact?
- Which loader and validator prove that every authored row is admitted?
- Which host command makes the feature reachable from the live game?
- Which existing save section carries the fact, and what is its frozen legacy shape?
- Which deterministic stream, ordering rule or no-RNG contract governs repeated execution?
- What visible refusal prevents a player from mistaking a projection for authority?
- What focused test would fail if the owner were bypassed?
- What future file or type is explicitly *not* part of this package?


# Appendix — Scenario matrix

# Appendix — Scenario and Negative-Contract Matrix

| ID | Scenario | Precondition | Expected owner outcome | Negative proof | Owner |
| --- | --- | --- | --- | --- | --- |
| S-01 | 41-01 23 rooms load | load 23 rooms and 12 rules | Room definitions and rules are immutable catalog data. | Catalog count drives an unnecessary new system. | ShelterRoomCatalogLoader |
| S-02 | 41-02 12 rules load | validate IDs/capacities/costs | Assignments, occupancy and bonuses belong to ShelterAssignmentSystem. | UI assigns a survivor despite capacity. | ShelterRoomCatalogLoader |
| S-03 | 41-03 capacity check | read current room instances and assignment state | Construction/upgrade and room identity/history remain with their current owners. | A rule is treated as a hard gate without current evidence. | ShelterRoomCatalogLoader |
| S-04 | 41-04 unknown room refusal | present occupancy and rule outcome | No new room state is stored in the catalog or panel. | Assignment and construction state are duplicated. | ShelterRoomCatalogLoader |
| S-05 | 41-05 duplicate assignment refusal | route assign/unassign through ShelterAssignmentSystem | Room definitions and rules are immutable catalog data. | Save restore creates phantom occupants. | ShelterRoomCatalogLoader |
| S-06 | 41-06 same-room query | apply current capacity/construction effects | Assignments, occupancy and bonuses belong to ShelterAssignmentSystem. | Catalog count drives an unnecessary new system. | ShelterRoomCatalogLoader |
| S-07 | 41-07 save continuation | capture ShelterAssignmentSave | Construction/upgrade and room identity/history remain with their current owners. | UI assigns a survivor despite capacity. | ShelterRoomCatalogLoader |
| S-08 | 41-08 construction interaction | load 23 rooms and 12 rules | No new room state is stored in the catalog or panel. | A rule is treated as a hard gate without current evidence. | ShelterRoomCatalogLoader |
| S-09 | 41-09 UI refresh | validate IDs/capacities/costs | Room definitions and rules are immutable catalog data. | Assignment and construction state are duplicated. | ShelterRoomCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 41-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-02 | 41-TC-02 ID/range validation | unit | ID/range validation; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-03 | 41-TC-03 assignment success | persistence | assignment success; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-04 | 41-TC-04 unknown IDs | determinism | unknown IDs; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-05 | 41-TC-05 capacity boundary | host | capacity boundary; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-06 | 41-TC-06 event once | UI/accessibility | event once; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-07 | 41-TC-07 save round trip | cross-system | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |
| T-08 | 41-TC-08 UI command | data | UI command; verify the named current owner and its negative boundary without inventing a second authority. | ShelterRoomCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 29 | `src/Host/ShelterAssignmentHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 16 | `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/World/HoldfastInteriorView.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/GapTestCoverageTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/ShelterDecorHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.ShelterBatch3.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/ShelterThermalFrostbiteBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/ShelterDecorSnapshotFixture.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Shelter/Plan40_41DebtRoomIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/ShelterThermalSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Survivors/ShelterApprenticeshipAndWillPlan55Tests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/HoldfastPresentationHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/ShelterThermalHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Shelter/ShelterOperationsBoardCoreTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Survivors/Plan24NeedsSourceMigrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Needs/SleepAcousticRestEngine.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.HoldfastPresentation.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/Phase0HostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/ShelterDecorSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/SleepAcousticRestHostSession.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/shelter_rooms.json`

### `Assets/StreamingAssets/Data/shelter_rooms.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 20439; characters: 20439.
- SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `rooms`, `assignment_rules`

#### `rooms` — 23 current rows

- Row 001 `room_bunker_corridor`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4}],"capacity":0,"description":"Access concourse and structural spine connecting bunker sectors, hatchways, and stairwells.","display_name":"Central Access Corridor"…`
- Row 002 `room_bunks_crowded`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":6},{"item_id":"scrap_metal","quantity":4}],"capacity":6,"description":"Triple-tiered pipe bunks packed wall-to-wall for maximum occupancy at the cost of privacy and c…`
- Row 003 `room_bunks`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":4},{"item_id":"scrap_metal","quantity":6}],"capacity":4,"description":"Double-tiered steel bunk frames bolted to concrete slab with personal locker space.","display_n…`
- Row 004 `room_quarters_private`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":8},{"item_id":"cloth","quantity":4}],"capacity":2,"description":"Acoustically insulated private sleeping cubicles providing restorative rest and privacy.","display_na…`
- Row 005 `room_workshop`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"mechanical_parts","quantity":4}],"capacity":2,"description":"Fabrication benches, vise clamps, and hand tools for general repairs and scrap repurposin…`
- Row 006 `room_workshop_heavy`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12},{"item_id":"heavy_industrial_motor","quantity":1}],"capacity":2,"description":"Reinforced flooring, overhead crane hoist, and hydraulic presses for engine overha…`
- Row 007 `room_workshop_precision`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"scrap_electronic","quantity":4}],"capacity":2,"description":"Clean room environment with jeweler loupes, soldering irons, and precision instruments.",…`
- Row 008 `room_clinic`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":4},{"item_id":"cloth","quantity":6}],"capacity":2,"description":"Emergency triage tables, sterile dressings, and disinfectant wash for treating trauma.","display_name…`
- Row 009 `room_ward_clinical`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"chemicals","quantity":4}],"capacity":2,"description":"Sealed operating bay with surgical lighting, oxygen manifold, and surgical instruments.","displa…`
- Row 010 `room_ward_quarantine`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"cloth","quantity":4}],"capacity":2,"description":"Negative-pressure containment cell with ultraviolet sanitization for infectious outbreaks.","display…`
- Row 011 `room_kitchen`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":6},{"item_id":"scrap_wood","quantity":4}],"capacity":2,"description":"Three-kettle stove, butchering block, and ration preparation counter with exhaust flue.","displ…`
- Row 012 `room_storage_bay`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":8}],"capacity":1,"description":"Banded wooden shelving and pallet racks for dry rations, raw scrap, and tools.","display_name":"General Storage Bay","function":"Stora…`
- Row 013 `room_storage_secure`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12}],"capacity":1,"description":"Heavy blast-lock room for secure caching of firearms, ammunition, and rare medical isotopes.","display_name":"Reinforced Armored Vau…`
- Row 014 `room_greenhouse_shelter`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"chemicals","quantity":4}],"capacity":2,"description":"Tiered hydroponic growth trays under full-spectrum sodium lamps fed from filtered water.","displ…`
- Row 015 `room_radio_tuner`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"scrap_electronic","quantity":4}],"capacity":1,"description":"Shortwave tuner rack, antenna lead-in, and signal decoding station for monitoring the was…`
- Row 016 `room_armory_munitions`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":10},{"item_id":"mechanical_parts","quantity":4}],"capacity":1,"description":"Rifle lockers, cleaning solvent basins, and a reloading press for ammunition refits.","d…`
- Row 017 `room_laboratory_research`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"scrap_electronic","quantity":6}],"capacity":2,"description":"Centrifuges, distillation tubes, and analytical charts for cataloging pre-war engineering…`
- Row 018 `room_common_mess_hall`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":10}],"capacity":4,"description":"Long communal benches and notice board where survivors gather for meals, meetings, and morale.","display_name":"Communal Mess Hall","…`
- Row 019 `room_reading_quiet_room`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_wood","quantity":6},{"item_id":"cloth","quantity":2}],"capacity":2,"description":"A quiet reading nook insulated from generator vibration for decompression and technical manual study.…`
- Row 020 `room_airlock`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":10},{"item_id":"mechanical_parts","quantity":4}],"capacity":2,"description":"Double airtight blast hatch with chemical decontam sprayers and dosimeter staging rack."…`
- Row 021 `room_generator`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":12},{"item_id":"fuel","quantity":2}],"capacity":2,"description":"Heavy industrial diesel dynamo providing electrical power across main lighting and pump circuits.","…`
- Row 022 `room_filtration`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":8},{"item_id":"cloth","quantity":4}],"capacity":1,"description":"HEPA filter banks and activated charcoal canisters providing clean breathable air to the shelter.","…`
- Row 023 `room_hope_beacon`: `{"base_condition":100.0,"build_cost":[{"item_id":"scrap_metal","quantity":4},{"item_id":"cloth","quantity":2},{"item_id":"battery","quantity":1}],"capacity":3,"description":"The mess hall's lamplit corner where someone reads the news aloud…`

#### `assignment_rules` — 12 current rows

- Row 001 `rule_medical_field_surgery`: `{"bonus_magnitude":0.25,"bonus_type":"treatment_efficiency","description":"Medically trained survivors accelerate wound recovery and reduce medical supply consumption.","id":"rule_medical_field_surgery","is_hard_gate":false,"name":"Triage …`
- Row 002 `rule_workshop_machinist`: `{"bonus_magnitude":0.2,"bonus_type":"repair_speed","description":"Skilled mechanics improve structural repair speed and minimize part wear.","id":"rule_workshop_machinist","is_hard_gate":false,"name":"Machinist & Maintenance","penalty_magn…`
- Row 003 `rule_workshop_precision`: `{"bonus_magnitude":0.2,"bonus_type":"crafting_yield","description":"Deep workshop sense boosts yield and crafting reliability on high-tier items.","id":"rule_workshop_precision","is_hard_gate":false,"name":"Precision Tooling Focus","penalt…`
- Row 004 `rule_radio_communications`: `{"bonus_magnitude":0.25,"bonus_type":"signal_clarity","description":"Trained signal operators discern faint broadcasts through background ionospheric noise.","id":"rule_radio_communications","is_hard_gate":false,"name":"Radio Signal Proces…`
- Row 005 `rule_kitchen_nutrition`: `{"bonus_magnitude":0.25,"bonus_type":"meal_efficiency","description":"Skilled cooks stretch limited calories without sacrificing survivor nutrition or morale.","id":"rule_kitchen_nutrition","is_hard_gate":false,"name":"Canteen Ration Maste…`
- Row 006 `rule_laboratory_analysis`: `{"bonus_magnitude":0.3,"bonus_type":"research_speed","description":"Disciplined researchers accelerate knowledge decoding from recovered technical archives.","id":"rule_laboratory_analysis","is_hard_gate":false,"name":"Scientific Analysis"…`
- Row 007 `rule_greenhouse_botany`: `{"bonus_magnitude":0.25,"bonus_type":"harvest_yield","description":"Hardy growers optimize nutrient feeds to boost harvest yields in artificial lighting.","id":"rule_greenhouse_botany","is_hard_gate":false,"name":"Hydroponic Cultivation","…`
- Row 008 `rule_generator_maintenance`: `{"bonus_magnitude":0.2,"bonus_type":"fuel_efficiency","description":"Experienced engineers reduce fuel consumption and prevent unexpected grid brownouts.","id":"rule_generator_maintenance","is_hard_gate":false,"name":"Turbine & Power Tunin…`
- Row 009 `rule_armory_service`: `{"bonus_magnitude":0.2,"bonus_type":"weapon_maintenance","description":"Watchful armorers keep firearms cleaned and expedition equipment primed.","id":"rule_armory_service","is_hard_gate":false,"name":"Armory Maintenance","penalty_magnitud…`
- Row 010 `rule_storage_logistics`: `{"bonus_magnitude":0.15,"bonus_type":"handling_speed","description":"Organized quartermasters streamline inventory handling and reduce spoilage.","id":"rule_storage_logistics","is_hard_gate":false,"name":"Logistics Organization","penalty_m…`
- Row 011 `rule_airlock_decontamination`: `{"bonus_magnitude":0.2,"bonus_type":"turnaround_speed","description":"Proper decontamination procedures accelerate expedition turnaround and scrub fallout.","id":"rule_airlock_decontamination","is_hard_gate":false,"name":"Airlock Protocol"…`
- Row 012 `rule_dormitory_caretaker`: `{"bonus_magnitude":0.15,"bonus_type":"rest_quality","description":"Attentive caretakers maintain clean living quarters, improving rest quality and morale.","id":"rule_dormitory_caretaker","is_hard_gate":false,"name":"Shelter Caretaking","p…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/shelter_room_identities.json`

### `Assets/StreamingAssets/Data/shelter_room_identities.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 54024; characters: 54024.
- SHA-256: `e294ef0d24b40c42a9fee8c1650ee6cc3778daa5c89a2f69cea6bcce02f3be90`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `rooms`, `vignettes`, `fixtures`

#### `rooms` — 13 current rows

- Row 001 `room_bunker_corridor`: `{"current_use":"Crisis conversion: the shelter's spine — muster, notices, and every walk between the stacks","display_name":"Central Corridor","fixture_ids":["room_fixture_corridor_chart_rail","room_fixture_corridor_plate_rectangles","room…`
- Row 002 `room_storage_bay`: `{"current_use":"Crisis conversion: ration and supply locker, with the deep stores behind it still only partly opened","display_name":"Storage Bay","fixture_ids":["room_fixture_stores_bin_labels","room_fixture_stores_scale_pin","room_fixtur…`
- Row 003 `room_bunks`: `{"current_use":"Crisis conversion: shared sleeping quarters under an unshielded timber ceiling","display_name":"Bunk Living","fixture_ids":["room_fixture_bunks_stencil_gaps","room_fixture_bunks_spare_socket","room_fixture_bunks_dosimeter_n…`
- Row 004 `room_kitchen`: `{"current_use":"Crisis conversion: one table, one ladle, and the only warm room most people remember","display_name":"Galley Kitchen","fixture_ids":["room_fixture_kitchen_portion_rings","room_fixture_kitchen_ladle_nail","room_fixture_kitch…`
- Row 005 `room_clinic`: `{"current_use":"Crisis conversion: a curtain on a wire, iodine, boiled cloth, and arithmetic","display_name":"Medical Ward","fixture_ids":["room_fixture_clinic_curtain_wire","room_fixture_clinic_basin_rim","room_fixture_clinic_iodine_lot",…`
- Row 006 `room_workshop`: `{"current_use":"Crisis conversion: fabrication bench, repair station, and the shelter's spare-parts memory","display_name":"Workshop","fixture_ids":["room_fixture_workshop_busbar_leg","room_fixture_workshop_tool_shadow","room_fixture_works…`
- Row 007 `room_filtration`: `{"current_use":"Crisis conversion: the HEPA bay and intake leg keeping the whole shelter breathable","display_name":"Filtration Stack","fixture_ids":["room_fixture_filtration_canister_notches","room_fixture_filtration_nameplate_tin","room_…`
- Row 008 `room_airlock`: `{"current_use":"Crisis conversion: the inner airlock; decontamination is a bucket, a rag on a nail, and a decision","display_name":"Airlock Hatch","fixture_ids":["room_fixture_airlock_boot_crate","room_fixture_airlock_bolted_chair","room_f…`
- Row 009 `room_radio_tuner`: `{"current_use":"Crisis conversion: 142.850 MHz watch station, rate cards, and the coast's weather","display_name":"Tuner Station","fixture_ids":["room_fixture_radio_mesh_panel","room_fixture_radio_log_book","room_fixture_radio_load_bulb"],…`
- Row 010 `room_foundry`: `{"current_use":"Post-war reconstruction: pipe, plate, bracket and bearing casting for everything the shelter repairs","display_name":"Silent Foundry","fixture_ids":["room_fixture_foundry_goggle_hook","room_fixture_foundry_sand_beds","room_…`
- Row 011 `room_greenhouse`: `{"current_use":"Crisis conversion: seedling trays, hardy greens, and the only window nobody can look through","display_name":"Greenhouse","fixture_ids":["room_fixture_greenhouse_peat_troughs","room_fixture_greenhouse_ballast_shield","room_…`
- Row 012 `room_main`: `{"current_use":"Generation and DC reserve for the whole electrical bus; the boiler was added when the deep heating manifold was isolated","display_name":"Main Vault","fixture_ids":["room_fixture_main_generator_mount","room_fixture_main_bat…`
- Row 013 `room_water_pump`: `{"current_use":"Still functional: the hand-pump backup draws from the same shaft as the main","display_name":"Water Pump","fixture_ids":["room_fixture_pump_pressure_gauge","room_fixture_pump_leather_cup","room_fixture_pump_flow_ledger","ro…`

#### `vignettes` — 21 current rows

- Row 001 `room_history_the_first_filter_change`: `{"body":"Eleven minutes is what the first crew filed against the intake housing, and there is a notch on the strap to prove it. The manual taped inside the cabinet door allows an hour. The difference between those two numbers is the reason…`
- Row 002 `room_history_a_frame_stayed`: `{"body":"The first death in this shelter was not dramatic enough to be written up. It happened on the watch between midnight and the milk round, in the second tier of the far frame, and the crew's only quarrel was about how to carry it out…`
- Row 003 `room_history_four_pale_rectangles`: `{"body":"There are four rectangles on this wall the colour of new concrete, and they are exactly as wide as a brass plate. The chart outlived the office that printed it. The rows are still the fourteen rows the manifest carries, and the pe…`
- Row 004 `room_history_the_count_came_short`: `{"body":"The count came up short on a Tuesday, which is the kind of detail people keep because it makes the memory portable. Two bowls waited at the far end under cloth, because three people were north on the haul road and the galley had i…`
- Row 005 `room_history_the_basin_that_was_a_mixing_bowl`: `{"body":"The engineering record describes this room with a table, a drain, and a hot wash point. It has none of those things. It has a wire, a curtain, a brown bottle with a lot number ink-stamped on the shoulder, a bolt of cloth boiled un…`
- Row 006 `room_history_a_chair_from_the_row`: `{"body":"The chair came in on its side through the outer hatch and it is still here. It is a dentist's chair, bolted to the floor of an airlock, facing the inner door, with no instruments and no prospect of instruments. The Row is short ex…`
- Row 007 `room_history_the_discrepancy`: `{"body":"A crate stencilled to this allocation's depot, marked not for general issue, carries a packing form with a date three days after the sky went. Eight pairs of children's winter boots, sizes one through four, banded and taped, and n…`
- Row 008 `room_history_the_second_blower`: `{"body":"The second blower did not come from this allocation. It came out of a laundry on the south side, on a hand cart, with its belt cut short and its housing dented where the stairwell was. The plant as drawn has one duty blower and on…`
- Row 009 `room_history_bench_markings`: `{"body":"The bench in the corridor has tally marks cut into the frame with a knife. They are not days. They are not water units. They are something else, because the spacing changes after the third month and the marks after that are shorte…`
- Row 010 `room_history_shelf_unit_d`: `{"body":"Shelf unit D was the last to be filled and the first to be reorganised. The discrepancy is in the spacing: the brackets on the left side are two holes closer together than the right, which means the shelf was cut from a different …`
- Row 011 `room_history_bunk_three`: `{"body":"Bunk three has a notch cut in the frame rail at the height of the mattress spring. The notch is worn smooth, which means it was cut before the spring was installed and then rubbed by fabric for a long time. The person who slept th…`
- Row 012 `room_history_can_opener_dent`: `{"body":"The kitchen can opener has a dent in the handle where it was struck with something hard. The strike mark is on the left side, which means the person who used it was left-handed and frustrated. The dent is old enough to have a pati…`
- Row 013 `room_history_suture_pack`: `{"body":"The basin that was a mixing bowl still has a ring of dried compound around the rim that is not any antiseptic anybody in this crew can name. The suture pack in drawer seven is sealed and the expiry date is before the Exchange. The…`
- Row 014 `room_history_lathe_true`: `{"body":"The lathe was true when it was last measured, and the measurement is written on the tailstock in pencil. The pencil mark is under a layer of grease and can be read only if you wipe the tailstock with your thumb and hold it at an a…`
- Row 015 `room_history_tuner_warm`: `{"body":"The tuner is warm even when the set has been off for hours. The warmth is not from the electronics; the valve line draws nothing when it is cold. The warmth comes from the exhaust leg of the ventilation plant, which runs behind th…`
- Row 016 `room_history_cupola_breath`: `{"body":"The cupola breathes when it cools. The breath is a slow exhalation through the tuyeres and it lasts exactly as long as the walk from the charging floor to the water jacket. The crew timed it once and wrote the number on the blower…`
- Row 017 `room_history_soil_window`: `{"body":"The soil in the greenhouse trays is darker than the soil outside, which is not surprising, but the dark layer is thinner than it should be. The top millimetre is ash and the layer underneath is the original topsoil, and the bounda…`
- Row 018 `room_history_generator_footings`: `{"body":"The generator footings are cast into the slab with four anchor bolts and a shim pack. The shim pack on the left rear footing is thinner than the others by the thickness of a coin, which is why the set vibrates at idle and the penc…`
- Row 019 `room_history_boiler_jacket`: `{"body":"The boiler jacket ticks twice when the fire settles. The ticking is the steel contracting as it cools and it has done that since the canteen's range was converted. The person who made the conversion left a note inside the hatch fr…`
- Row 020 `room_history_filter_cartridge`: `{"body":"The filter cartridge slots have a wear pattern on the sealing face that matches the intake housing's mounting lugs. The pattern is not uniform. The left bank wears faster than the right, which means the intake leg is not balanced.…`
- Row 021 `vignette_water_pump_original_use`: `{"body":"The pump room had a schedule. Morning: draw thirty litres and check the gauge. Midday: top the seal and log the flow rate. Evening: bleed the line and hang the leather cup to dry. The schedule is still chalked on the wall above th…`

#### `fixtures` — 53 current rows

- Row 001 `room_fixture_corridor_chart_rail`: `{"art_visible":true,"detail":"A chart rail in brass, with a pencil hanging through the ring on a string gone dark.","historical_meaning":"Printed before the Exchange for a crew of fourteen; the rail has held every roster since.","id":"room…`
- Row 002 `room_fixture_corridor_plate_rectangles`: `{"art_visible":true,"detail":"Four unfaded rectangles of older paint above the bench, plate-width, in a row.","historical_meaning":"Where the brass name-plates were screwed. The plates are in the tin behind the filtration stack.","id":"roo…`
- Row 003 `room_fixture_corridor_pencil_stub`: `{"art_visible":true,"detail":"A pencil stub on the rail, sharpened at both ends.","historical_meaning":"Sharpened twice so it would not run out mid-watch; the point is always short.","id":"room_fixture_corridor_pencil_stub","inspectable":f…`
- Row 004 `room_fixture_corridor_scrub_line`: `{"art_visible":true,"detail":"A band of bare concrete at head height along the north wall, painted over and coming through.","historical_meaning":"Where the first decon wash-down scrubbed the paint off and the crew repainted without discus…`
- Row 005 `room_fixture_bunks_stencil_gaps`: `{"art_visible":true,"detail":"Bunk numbers stencilled in white; the sequence skips two numbers.","historical_meaning":"Original installer's convention. The reason is not recorded and is not asked about.","id":"room_fixture_bunks_stencil_ga…`
- Row 006 `room_fixture_bunks_spare_socket`: `{"art_visible":true,"detail":"A socket drilled in the runner for a third pad that never arrived.","historical_meaning":"Provisioned for the manifest count and never filled.","id":"room_fixture_bunks_spare_socket","inspectable":false,"renov…`
- Row 007 `room_fixture_bunks_dosimeter_nail`: `{"art_visible":true,"detail":"A nail under a second-tier frame with a dosimeter hanging on a cord.","historical_meaning":"Read at the top of each watch by whoever wakes first; it ticks slower than the intake.","id":"room_fixture_bunks_dosi…`
- Row 008 `room_fixture_bunks_bolt_rings`: `{"art_visible":true,"detail":"Paint circles around each foot, noticeably wider than the bolt heads.","historical_meaning":"The frames were re-bolted once after the slab moved; the old holes are inside the rings.","id":"room_fixture_bunks_b…`
- Row 009 `room_fixture_filtration_canister_notches`: `{"art_visible":true,"detail":"A file notch on every canister strap, grouped in days.","historical_meaning":"The crew's own service log, cut into metal because paper runs out.","id":"room_fixture_filtration_canister_notches","inspectable":f…`
- Row 010 `room_fixture_filtration_nameplate_tin`: `{"art_visible":true,"detail":"A sweets tin behind the canister bank. Heavier than it looks, until it is not.","historical_meaning":"Fourteen brass plates with surnames that belong to nobody here. Everyone who finds it puts it back.","id":"…`
- Row 011 `room_fixture_filtration_intake_stool`: `{"art_visible":true,"detail":"A stool bolted closer to the intake than anything else in the room.","historical_meaning":"Whoever sleeps nearest tastes the filter first. The duty rota has quietly assigned it that way.","id":"room_fixture_fi…`
- Row 012 `room_fixture_filtration_steam_valve`: `{"art_visible":true,"detail":"A bake-out valve with a wire seal and a hand-scribed quarter-mark beside it.","historical_meaning":"The charcoal beds are steamed on a cycle longer than the filters; the quarter-mark is how the room warns you.…`
- Row 013 `room_fixture_filtration_spare_belt`: `{"art_visible":true,"detail":"A spare belt on a hook, clearly the wrong size.","historical_meaning":"Salvaged with the second blower and kept because nothing else has ever fitted either.","id":"room_fixture_filtration_spare_belt","inspecta…`
- Row 014 `room_fixture_filtration_hazmat_hook`: `{"art_visible":true,"detail":"A rubber apron on a hook, two sizes too large for anyone currently here.","historical_meaning":"Issued from the pre-war kit for pre-filter work. The size is a reminder of who the plant was built for.","id":"ro…`
- Row 015 `room_fixture_kitchen_portion_rings`: `{"art_visible":true,"detail":"Concentric burn rings inside three bowls, none of them the size of a full portion.","historical_meaning":"Stew that sat and was eaten, and stew that sat and was not.","id":"room_fixture_kitchen_portion_rings",…`
- Row 016 `room_fixture_kitchen_ladle_nail`: `{"art_visible":true,"detail":"A ladle on a nail set at head height.","historical_meaning":"Placed where children can see it and cannot lift it. The order at the table is negotiated around it.","id":"room_fixture_kitchen_ladle_nail","inspec…`
- Row 017 `room_fixture_kitchen_table_scratches`: `{"art_visible":true,"detail":"Knife marks along the table edge, grouped in fives.","historical_meaning":"A tally of short counts, kept so the number could exist without being spoken.","id":"room_fixture_kitchen_table_scratches","inspectabl…`
- Row 018 `room_fixture_kitchen_flue_damper`: `{"art_visible":true,"detail":"A flue bypass damper welded open, the handle snapped off flush.","historical_meaning":"The range was designed to be vented and was re-pointed when the exhaust shaft was blocked.","id":"room_fixture_kitchen_flu…`
- Row 019 `room_fixture_clinic_curtain_wire`: `{"art_visible":true,"detail":"A curtain on a stretched wire, the hooks polished bright.","historical_meaning":"The partition this shelter actually has, in place of the one the drawings show.","id":"room_fixture_clinic_curtain_wire","inspec…`
- Row 020 `room_fixture_clinic_basin_rim`: `{"art_visible":true,"detail":"A basin with the rim ground thin, and a scuff at the height of a kneeling adult.","historical_meaning":"Repurposed from the stores. It has been used for every dressing since the first week.","id":"room_fixture…`
- Row 021 `room_fixture_clinic_iodine_lot`: `{"art_visible":true,"detail":"A brown bottle with a Continuity lot number stamped on the shoulder.","historical_meaning":"Pre-war stock from the provisioning network, and a clue to how much of this place was meant to be supplied.","id":"ro…`
- Row 022 `room_fixture_clinic_capped_drain`: `{"art_visible":true,"detail":"A floor drain capped with folded cloth and a plate on top.","historical_meaning":"The room was drawn around this drain. Nobody has used it as drawn, and nobody has removed the cap either.","id":"room_fixture_c…`
- Row 023 `room_fixture_workshop_busbar_leg`: `{"art_visible":true,"detail":"Three busbar stubs along the ceiling; one is capped and taped and never re-energised.","historical_meaning":"The shop was fed three-phase. The shelter can only afford to carry one leg under load.","id":"room_f…`
- Row 024 `room_fixture_workshop_tool_shadow`: `{"art_visible":true,"detail":"A painted tool silhouette on the pegboard with no tool and no gap around it.","historical_meaning":"The shadow predates the crew. Nobody has matched a tool to it since.","id":"room_fixture_workshop_tool_shadow…`
- Row 025 `room_fixture_workshop_swarf_grate`: `{"art_visible":true,"detail":"A swarf grate under the lathe position, cut and re-welded twice.","historical_meaning":"Machine work continued here through the plant's own power shortages.","id":"room_fixture_workshop_swarf_grate","inspectab…`
- Row 026 `room_fixture_airlock_boot_crate`: `{"art_visible":true,"detail":"A banded crate stencilled for this allocation's depot, tape uncut.","historical_meaning":"Children's winter boots. The delivery date on the form is after the Exchange.","id":"room_fixture_airlock_boot_crate","…`
- Row 027 `room_fixture_airlock_bolted_chair`: `{"art_visible":true,"detail":"A heavy chair bolted to the airlock floor, facing the inner door.","historical_meaning":"Salvaged from the Row. It stayed because it could not be moved back through the hatch.","id":"room_fixture_airlock_bolte…`
- Row 028 `room_fixture_airlock_nozzles`: `{"art_visible":true,"detail":"Four bare feed stubs in the ceiling arch where decon nozzles were mounted.","historical_meaning":"The valves seized in the cold and were cut out for parts the plant needed more.","id":"room_fixture_airlock_noz…`
- Row 029 `room_fixture_airlock_handprints`: `{"art_visible":true,"detail":"Paint worn to bare metal at head height on the inside of the door.","historical_meaning":"Where people stood while the hatch cycled. The wear is older than the crew.","id":"room_fixture_airlock_handprints","in…`
- Row 030 `room_fixture_airlock_rag_nail`: `{"art_visible":true,"detail":"A rag on a nail by the inner seal, boiled or not, depending on the week.","historical_meaning":"The shelter's entire decontamination instrumentation, in cloth form.","id":"room_fixture_airlock_rag_nail","inspe…`
- Row 031 `room_fixture_radio_mesh_panel`: `{"art_visible":true,"detail":"Copper mesh stapled behind the set where a screened enclosure used to be.","historical_meaning":"A field repair of a Faraday screen that was built into the wall and lost to it.","id":"room_fixture_radio_mesh_p…`
- Row 032 `room_fixture_radio_log_book`: `{"art_visible":true,"detail":"A station log whose call signs predate everyone living here.","historical_meaning":"Continuity traffic, kept going out of habit, which is why nobody starts a new book.","id":"room_fixture_radio_log_book","insp…`
- Row 033 `room_fixture_radio_load_bulb`: `{"art_visible":true,"detail":"A load bulb wired in series, its glass marked with a figure in grease pencil.","historical_meaning":"A power limit somebody wrote on the hardware itself so it survived the loss of the paperwork.","id":"room_fi…`
- Row 034 `room_fixture_greenhouse_peat_troughs`: `{"art_visible":true,"detail":"Timber troughs stepped up the wall; one is rotted through and shored with a prop.","historical_meaning":"Cascading peat beds as designed. The prop is the crew's own amendment.","id":"room_fixture_greenhouse_pe…`
- Row 035 `room_fixture_greenhouse_ballast_shield`: `{"art_visible":true,"detail":"A hand-formed metal shield over a lamp ballast, with a note tucked behind it.","historical_meaning":"The ballast heat was recorded in the plan as a spare radiator for the level above.","id":"room_fixture_green…`
- Row 036 `room_fixture_greenhouse_seed_tins`: `{"art_visible":true,"detail":"A crate of dated seed tins, three of them labelled with varieties nobody recognises.","historical_meaning":"Allocation stock. The labels are depot shorthand, not a language the crew was taught.","id":"room_fix…`
- Row 037 `room_fixture_foundry_goggle_hook`: `{"art_visible":true,"detail":"Cobalt-tinted goggles on a hook, the lenses crazed to milk.","historical_meaning":"Pouring glass requires them. The replacement stock does not exist.","id":"room_fixture_foundry_goggle_hook","inspectable":fals…`
- Row 038 `room_fixture_foundry_sand_beds`: `{"art_visible":true,"detail":"Casting beds raked to a chalk line that no longer matches the floor.","historical_meaning":"The bay was levelled against a datum that has since shifted; the crew rakes to memory now.","id":"room_fixture_foundr…`
- Row 039 `room_fixture_foundry_heat_stain`: `{"art_visible":true,"detail":"A rectangular discolouration on the slab, edges crisp as a struck match.","historical_meaning":"Heat from a proper full heat reads through two floor levels. The plant's siting was chosen around that.","id":"ro…`
- Row 040 `room_fixture_foundry_works_plate`: `{"art_visible":true,"detail":"A riveted builder's plate, its works number filed half-illegible.","historical_meaning":"Deliberate or damage; the crew has never agreed, and the plate still carries the machine's name.","id":"room_fixture_fou…`
- Row 041 `room_fixture_stores_bin_labels`: `{"art_visible":true,"detail":"Steel bins with seal gaskets and label plates in two different print runs.","historical_meaning":"Re-labelled in a hurry during the conversion; the first print is depot type, the second is this crew's.","id":"…`
- Row 042 `room_fixture_stores_scale_pin`: `{"art_visible":true,"detail":"A balance scale hung on a pin, its weight box missing one disc.","historical_meaning":"Everything here is weighed. The missing disc is why two of the counts disagree.","id":"room_fixture_stores_scale_pin","ins…`
- Row 043 `room_fixture_stores_humidity_gauge`: `{"art_visible":true,"detail":"A wall humidity gauge with a red line scribed well below its useful range.","historical_meaning":"The bins were built to a dryness spec the shelter can no longer guarantee.","id":"room_fixture_stores_humidity_…`
- Row 044 `room_fixture_stores_depot_form`: `{"art_visible":true,"detail":"A packing form stapled inside a lid, the date printed three days late.","historical_meaning":"The provisioning network's last paperwork. Kept flat, not displayed.","id":"room_fixture_stores_depot_form","inspec…`
- Row 045 `room_fixture_main_generator_mount`: `{"art_visible":true,"detail":"The generator sits on a cast-iron isolation pad with four hold-down lugs and a vibration cut-out that somebody has already wrapped in tape.","historical_meaning":"Pre-war specification for a three-hand watch; …`
- Row 046 `room_fixture_main_battery_rack`: `{"art_visible":true,"detail":"The battery rack holds eight cells in two rows. The top row is original. The bottom row was added from the allocation's emergency lighting bank.","historical_meaning":"Mixed provenance: pre-war cells with a po…`
- Row 047 `room_fixture_main_inverter_panel`: `{"art_visible":true,"detail":"The inverter panel has a scratch in the paint that spells NOT LOAD in pencil, and a relay that clicks twice at odd hours.","historical_meaning":"The pencil scratch predates the crew. The relay click has been l…`
- Row 048 `room_fixture_main_boiler_hatch`: `{"art_visible":true,"detail":"The boiler hatch is a range hearth door with a new sight-glass fitted and the original brass handle bent to a different angle.","historical_meaning":"Converted from the canteen's coal range when the deep heati…`
- Row 049 `room_fixture_main_isolation_pad`: `{"art_visible":true,"detail":"Four isolation pads, one under each mount. The generator pad is cracked along one edge where the set was dropped during installation.","historical_meaning":"The drop crack is old enough to have a name among th…`
- Row 050 `room_fixture_pump_pressure_gauge`: `{"art_visible":true,"detail":"A brass pressure gauge with the needle resting at zero and a crack across the face.","historical_meaning":"The gauge was last replaced during the Exchange. Its crack appeared on the first day of fallout.","id"…`
- Row 051 `room_fixture_pump_leather_cup`: `{"art_visible":true,"detail":"A leather cup on a hook beside the hand pump, softened by use and stained dark.","historical_meaning":"The leather was cut from a seat in the old depot truck. It still holds water.","id":"room_fixture_pump_lea…`
- Row 052 `room_fixture_pump_flow_ledger`: `{"art_visible":true,"detail":"A wall ledger of flow rates in pencil, the last entry dated three days before the Exchange.","historical_meaning":"The handwriting changes halfway through the page. Two people recorded the well.","id":"room_fi…`
- Row 053 `room_fixture_pump_well_collar`: `{"art_visible":true,"detail":"The well collar is cast iron with a rusted chain and a bucket that has no bottom.","historical_meaning":"The bucket was cut open to use as a strainer. The chain still has the original tag.","id":"room_fixture_…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`

### `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs` — complete current file

- Size: 535 lines / 30549 bytes.
- SHA-256: `6c1eddd2273749e7ed30febe53863f2197b12a48354bf49f0edbbee7eededfd9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.IO;
00005:
00006: namespace Ashfall.Core.Shelter
00007: {
00008:     [Serializable]
00009:     public sealed class ShelterRoomCostDef
00010:     {
00011:         public string item_id { get; set; } = string.Empty;
00012:         public int quantity { get; set; } = 1;
00013:     }
00014:
00015:     /// <summary>
00016:     /// Static authored definition for a shelter room type.
00017:     /// </summary>
00018:     [Serializable]
00019:     public sealed class ShelterRoomDef
00020:     {
00021:         public string id { get; set; } = string.Empty;
00022:         public string display_name { get; set; } = string.Empty;
00023:         public string description { get; set; } = string.Empty;
00024:         public string function { get; set; } = "General"; // Dormitory, Workshop, MedicalBay, Kitchen, Storage, Greenhouse, RadioRoom, Armory, Laboratory, CommonArea, Airlock, GeneratorRoom, FiltrationStack, Corridor
00025:         public int capacity { get; set; } = 2;
00026:         public int max_upgrade_level { get; set; } = 3;
00027:         public string required_skill_id { get; set; } = string.Empty;
00028:         public string workstation_id { get; set; } = string.Empty;
00029:         public float base_condition { get; set; } = 100.0f;
00030:         public List<ShelterRoomCostDef> build_cost { get; set; } = new List<ShelterRoomCostDef>();
00031:         public List<ShelterRoomCostDef> repair_cost { get; set; } = new List<ShelterRoomCostDef>();
00032:         public List<string> tags { get; set; } = new List<string>();
00033:     }
00034:
00035:     /// <summary>
00036:     /// Static authored definition for an assignment rule governing survivor fit.
00037:     /// </summary>
00038:     [Serializable]
00039:     public sealed class ShelterAssignmentRuleDef
00040:     {
00041:         public string id { get; set; } = string.Empty;
00042:         public string name { get; set; } = string.Empty;
00043:         public string description { get; set; } = string.Empty;
00044:         public string target_room_function { get; set; } = string.Empty;
00045:         public string required_skill_id { get; set; } = string.Empty;
00046:         public string bonus_type { get; set; } = "efficiency";
00047:         public float bonus_magnitude { get; set; } = 0.20f;
00048:         public float penalty_magnitude { get; set; } = -0.10f;
00049:         public bool is_hard_gate { get; set; } = false;
00050:     }
00051:
00052:     /// <summary>
00053:     /// Container for shelter_rooms.json catalog authority.
00054:     /// </summary>
00055:     [Serializable]
00056:     public sealed class ShelterRoomCatalogContainer
00057:     {
00058:         public int schema_version { get; set; } = 1;
00059:         public string collection_id { get; set; } = "shelter_rooms";
00060:         public List<ShelterRoomDef> rooms { get; set; } = new List<ShelterRoomDef>();
00061:         public List<ShelterAssignmentRuleDef> assignment_rules { get; set; } = new List<ShelterAssignmentRuleDef>();
00062:     }
00063:
00064:     /// <summary>
00065:     /// Authority loader for shelter room definitions and assignment rules.
00066:     /// </summary>
00067:     public static class ShelterRoomCatalogLoader
00068:     {
00069:         public const string CatalogFileName = "shelter_rooms.json";
00070:
00071:         public static ShelterRoomCatalogContainer Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
00072:         {
00073:             fileIO ??= new FileSystemIO();
00074:             serializer ??= new SystemTextJsonSerializer();
00075:
00076:             if (string.IsNullOrEmpty(dataDir))
00077:                 return GetDefaultCatalog();
00078:
00079:             string fullPath = fileIO.Combine(dataDir, CatalogFileName);
00080:             if (!fileIO.FileExists(fullPath))
00081:                 return GetDefaultCatalog();
00082:
00083:             try
00084:             {
00085:                 string json = fileIO.ReadAllText(fullPath);
00086:                 var container = serializer.Deserialize<ShelterRoomCatalogContainer>(json);
00087:                 if (container != null && container.rooms.Count > 0)
00088:                     return container;
00089:             }
00090:             catch (Exception ex)
00091:             {
00092:                 CatalogDiagnostics.Warn(fullPath, "ShelterRoomCatalogContainer", ex);
00093:             }
00094:
00095:             return GetDefaultCatalog();
00096:         }
00097:
00098:         public static ShelterRoomCatalogContainer GetDefaultCatalog()
00099:         {
00100:             return new ShelterRoomCatalogContainer
00101:             {
00102:                 schema_version = 1,
00103:                 collection_id = "shelter_rooms",
00104:                 rooms = GetDefaultRooms(),
00105:                 assignment_rules = GetDefaultRules()
00106:             };
00107:         }
00108:
00109:         public static List<ShelterRoomDef> GetDefaultRooms()
00110:         {
00111:             return new List<ShelterRoomDef>
00112:             {
00113:                 new ShelterRoomDef
00114:                 {
00115:                     id = "room_bunker_corridor",
00116:                     display_name = "Central Access Corridor",
00117:                     description = "Access concourse and structural spine connecting bunker sectors, hatchways, and stairwells.",
00118:                     function = "Corridor",
00119:                     capacity = 0,
00120:                     max_upgrade_level = 3,
00121:                     tags = new List<string> { "spine", "circulation" },
00122:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } },
00123:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 1 } }
00124:                 },
00125:                 new ShelterRoomDef
00126:                 {
00127:                     id = "room_bunks_crowded",
00128:                     display_name = "Crowded Bunkhouse",
00129:                     description = "Triple-tiered pipe bunks packed wall-to-wall for maximum occupancy at the cost of privacy and comfort.",
00130:                     function = "Dormitory",
00131:                     capacity = 6,
00132:                     max_upgrade_level = 2,
00133:                     tags = new List<string> { "residential", "high_density" },
00134:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 6 }, new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } },
00135:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
00136:                 },
00137:                 new ShelterRoomDef
00138:                 {
00139:                     id = "room_bunks",
00140:                     display_name = "Standard Dormitory",
00141:                     description = "Double-tiered steel bunk frames bolted to concrete slab with personal locker space.",
00142:                     function = "Dormitory",
00143:                     capacity = 4,
00144:                     max_upgrade_level = 3,
00145:                     tags = new List<string> { "residential", "standard" },
00146:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 4 }, new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 } },
00147:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 2 } }
00148:                 },
00149:                 new ShelterRoomDef
00150:                 {
00151:                     id = "room_quarters_private",
00152:                     display_name = "Partitioned Quarters",
00153:                     description = "Acoustically insulated private sleeping cubicles providing restorative rest and privacy.",
00154:                     function = "Dormitory",
00155:                     capacity = 2,
00156:                     max_upgrade_level = 3,
00157:                     tags = new List<string> { "residential", "comfort" },
00158:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 8 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 4 } },
00159:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 1 } }
00160:                 },
00161:                 new ShelterRoomDef
00162:                 {
00163:                     id = "room_workshop",
00164:                     display_name = "General Workshop",
00165:                     description = "Fabrication benches, vise clamps, and hand tools for general repairs and scrap repurposing.",
00166:                     function = "Workshop",
00167:                     capacity = 2,
00168:                     max_upgrade_level = 3,
00169:                     required_skill_id = "skill_rough_repairs",
00170:                     tags = new List<string> { "crafting", "repair" },
00171:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "mechanical_parts", quantity = 4 } },
00172:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
00173:                 },
00174:                 new ShelterRoomDef
00175:                 {
00176:                     id = "room_workshop_heavy",
00177:                     display_name = "Heavy Machinery Workshop",
00178:                     description = "Reinforced flooring, overhead crane hoist, and hydraulic presses for engine overhauls.",
00179:                     function = "Workshop",
00180:                     capacity = 2,
00181:                     max_upgrade_level = 3,
00182:                     required_skill_id = "skill_workshop_sense",
00183:                     tags = new List<string> { "crafting", "heavy_industrial" },
00184:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 12 }, new ShelterRoomCostDef { item_id = "heavy_industrial_motor", quantity = 1 } },
00185:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } }
00186:                 },
00187:                 new ShelterRoomDef
00188:                 {
00189:                     id = "room_workshop_precision",
00190:                     display_name = "Precision Tooling Bench",
00191:                     description = "Clean room environment with jeweler loupes, soldering irons, and precision instruments.",
00192:                     function = "Workshop",
00193:                     capacity = 2,
00194:                     max_upgrade_level = 3,
00195:                     required_skill_id = "skill_workshop_sense",
00196:                     tags = new List<string> { "crafting", "precision" },
00197:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 }, new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 4 } },
00198:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 2 } }
00199:                 },
00200:                 new ShelterRoomDef
00201:                 {
00202:                     id = "room_clinic",
00203:                     display_name = "Field Clinic",
00204:                     description = "Emergency triage tables, sterile dressings, and disinfectant wash for treating trauma.",
00205:                     function = "MedicalBay",
00206:                     capacity = 2,
00207:                     max_upgrade_level = 3,
00208:                     required_skill_id = "skill_field_dressing",
00209:                     tags = new List<string> { "medical", "triage" },
00210:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 4 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 6 } },
00211:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "cloth", quantity = 2 } }
00212:                 },
00213:                 new ShelterRoomDef
00214:                 {
00215:                     id = "room_ward_clinical",
00216:                     display_name = "Clinical Surgical Ward",
00217:                     description = "Sealed operating bay with surgical lighting, oxygen manifold, and surgical instruments.",
00218:                     function = "MedicalBay",
00219:                     capacity = 2,
00220:                     max_upgrade_level = 3,
00221:                     required_skill_id = "skill_steady_hands",
00222:                     tags = new List<string> { "medical", "surgery" },
00223:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "chemicals", quantity = 4 } },
00224:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "chemicals", quantity = 2 } }
00225:                 },
00226:                 new ShelterRoomDef
00227:                 {
00228:                     id = "room_ward_quarantine",
00229:                     display_name = "Isolation Quarantine Bay",
00230:                     description = "Negative-pressure containment cell with ultraviolet sanitization for infectious outbreaks.",
00231:                     function = "MedicalBay",
00232:                     capacity = 2,
00233:                     max_upgrade_level = 3,
00234:                     required_skill_id = "skill_field_dressing",
00235:                     tags = new List<string> { "medical", "quarantine" },
00236:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 4 } },
00237:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 2 } }
00238:                 },
00239:                 new ShelterRoomDef
00240:                 {
00241:                     id = "room_kitchen",
00242:                     display_name = "Galley Kitchen",
00243:                     description = "Three-kettle stove, butchering block, and ration preparation counter with exhaust flue.",
00244:                     function = "Kitchen",
00245:                     capacity = 2,
00246:                     max_upgrade_level = 3,
00247:                     required_skill_id = "skill_ration_stretcher",
00248:                     tags = new List<string> { "nutrition", "canteen" },
00249:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 6 }, new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 4 } },
00250:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 2 } }
00251:                 },
00252:                 new ShelterRoomDef
00253:                 {
00254:                     id = "room_storage_bay",
00255:                     display_name = "General Storage Bay",
00256:                     description = "Banded wooden shelving and pallet racks for dry rations, raw scrap, and tools.",
00257:                     function = "Storage",
00258:                     capacity = 1,
00259:                     max_upgrade_level = 3,
00260:                     required_skill_id = "skill_quartermaster",
00261:                     tags = new List<string> { "logistics", "general" },
00262:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 8 } },
00263:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
00264:                 },
00265:                 new ShelterRoomDef
00266:                 {
00267:                     id = "room_storage_secure",
00268:                     display_name = "Reinforced Armored Vault",
00269:                     description = "Heavy blast-lock room for secure caching of firearms, ammunition, and rare medical isotopes.",
00270:                     function = "Storage",
00271:                     capacity = 1,
00272:                     max_upgrade_level = 3,
00273:                     required_skill_id = "skill_watchful",
00274:                     tags = new List<string> { "logistics", "secure" },
00275:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 12 } },
00276:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
00277:                 },
00278:                 new ShelterRoomDef
00279:                 {
00280:                     id = "room_greenhouse_shelter",
00281:                     display_name = "Subterranean Greenhouse",
00282:                     description = "Tiered hydroponic growth trays under full-spectrum sodium lamps fed from filtered water.",
00283:                     function = "Greenhouse",
00284:                     capacity = 2,
00285:                     max_upgrade_level = 3,
00286:                     required_skill_id = "skill_mycology",
00287:                     tags = new List<string> { "agriculture", "hydroponics" },
00288:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "chemicals", quantity = 4 } },
00289:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "chemicals", quantity = 2 } }
00290:                 },
00291:                 new ShelterRoomDef
00292:                 {
00293:                     id = "room_radio_tuner",
00294:                     display_name = "Radio Communications Bay",
00295:                     description = "Shortwave tuner rack, antenna lead-in, and signal decoding station for monitoring the wasteland.",
00296:                     function = "RadioRoom",
00297:                     capacity = 1,
00298:                     max_upgrade_level = 3,
00299:                     required_skill_id = "skill_signal_ear",
00300:                     tags = new List<string> { "communications", "intel" },
00301:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 }, new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 4 } },
00302:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 2 } }
00303:                 },
00304:                 new ShelterRoomDef
00305:                 {
00306:                     id = "room_armory_munitions",
00307:                     display_name = "Armory & Munitions Depot",
00308:                     description = "Rifle lockers, cleaning solvent basins, and a reloading press for ammunition refits.",
00309:                     function = "Armory",
00310:                     capacity = 1,
00311:                     max_upgrade_level = 3,
00312:                     required_skill_id = "skill_watchful",
00313:                     tags = new List<string> { "combat", "defense" },
00314:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 10 }, new ShelterRoomCostDef { item_id = "mechanical_parts", quantity = 4 } },
00315:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
00316:                 },
00317:                 new ShelterRoomDef
00318:                 {
00319:                     id = "room_laboratory_research",
00320:                     display_name = "Science & Research Lab",
00321:                     description = "Centrifuges, distillation tubes, and analytical charts for cataloging pre-war engineering archives.",
00322:                     function = "Laboratory",
00323:                     capacity = 2,
00324:                     max_upgrade_level = 3,
00325:                     required_skill_id = "skill_cold_analysis",
00326:                     tags = new List<string> { "research", "science" },
00327:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 6 } },
00328:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_electronic", quantity = 2 } }
00329:                 },
00330:                 new ShelterRoomDef
00331:                 {
00332:                     id = "room_common_mess_hall",
00333:                     display_name = "Communal Mess Hall",
00334:                     description = "Long communal benches and notice board where survivors gather for meals, meetings, and morale.",
00335:                     function = "CommonArea",
00336:                     capacity = 4,
00337:                     max_upgrade_level = 3,
00338:                     tags = new List<string> { "social", "morale" },
00339:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 10 } },
00340:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
00341:                 },
00342:                 new ShelterRoomDef
00343:                 {
00344:                     id = "room_reading_quiet_room",
00345:                     display_name = "Quiet Archive & Study",
00346:                     description = "A quiet reading nook insulated from generator vibration for decompression and technical manual study.",
00347:                     function = "CommonArea",
00348:                     capacity = 2,
00349:                     max_upgrade_level = 2,
00350:                     tags = new List<string> { "social", "study" },
00351:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 6 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 2 } },
00352:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_wood", quantity = 2 } }
00353:                 },
00354:                 new ShelterRoomDef
00355:                 {
00356:                     id = "room_airlock",
00357:                     display_name = "Decontamination Airlock",
00358:                     description = "Double airtight blast hatch with chemical decontam sprayers and dosimeter staging rack.",
00359:                     function = "Airlock",
00360:                     capacity = 2,
00361:                     max_upgrade_level = 3,
00362:                     required_skill_id = "skill_field_dressing",
00363:                     tags = new List<string> { "perimeter", "expedition" },
00364:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 10 }, new ShelterRoomCostDef { item_id = "mechanical_parts", quantity = 4 } },
00365:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 3 } }
00366:                 },
00367:                 new ShelterRoomDef
00368:                 {
00369:                     id = "room_generator",
00370:                     display_name = "Primary Diesel Generator",
00371:                     description = "Heavy industrial diesel dynamo providing electrical power across main lighting and pump circuits.",
00372:                     function = "GeneratorRoom",
00373:                     capacity = 2,
00374:                     max_upgrade_level = 3,
00375:                     required_skill_id = "skill_rough_repairs",
00376:                     tags = new List<string> { "power", "infrastructure" },
00377:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 12 }, new ShelterRoomCostDef { item_id = "fuel", quantity = 2 } },
00378:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 4 } }
00379:                 },
00380:                 new ShelterRoomDef
00381:                 {
00382:                     id = "room_filtration",
00383:                     display_name = "Filtration & Scrubber Stack",
00384:                     description = "HEPA filter banks and activated charcoal canisters providing clean breathable air to the shelter.",
00385:                     function = "FiltrationStack",
00386:                     capacity = 1,
00387:                     max_upgrade_level = 3,
00388:                     required_skill_id = "skill_rough_repairs",
00389:                     tags = new List<string> { "life_support", "infrastructure" },
00390:                     build_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "scrap_metal", quantity = 8 }, new ShelterRoomCostDef { item_id = "cloth", quantity = 4 } },
00391:                     repair_cost = new List<ShelterRoomCostDef> { new ShelterRoomCostDef { item_id = "cloth", quantity = 2 } }
00392:                 }
00393:             };
00394:         }
00395:
00396:         public static List<ShelterAssignmentRuleDef> GetDefaultRules()
00397:         {
00398:             return new List<ShelterAssignmentRuleDef>
00399:             {
00400:                 new ShelterAssignmentRuleDef
00401:                 {
00402:                     id = "rule_medical_field_surgery",
00403:                     name = "Triage & Field Medicine",
00404:                     description = "Medically trained survivors accelerate wound recovery and reduce medical supply consumption.",
00405:                     target_room_function = "MedicalBay",
00406:                     required_skill_id = "skill_field_dressing",
00407:                     bonus_type = "treatment_efficiency",
00408:                     bonus_magnitude = 0.25f,
00409:                     penalty_magnitude = -0.10f
00410:                 },
00411:                 new ShelterAssignmentRuleDef
00412:                 {
00413:                     id = "rule_workshop_machinist",
00414:                     name = "Machinist & Maintenance",
00415:                     description = "Skilled mechanics improve structural repair speed and minimize part wear.",
00416:                     target_room_function = "Workshop",
00417:                     required_skill_id = "skill_rough_repairs",
00418:                     bonus_type = "repair_speed",
00419:                     bonus_magnitude = 0.20f,
00420:                     penalty_magnitude = -0.05f
00421:                 },
00422:                 new ShelterAssignmentRuleDef
00423:                 {
00424:                     id = "rule_workshop_precision",
00425:                     name = "Precision Tooling Focus",
00426:                     description = "Deep workshop sense boosts yield and crafting reliability on high-tier items.",
00427:                     target_room_function = "Workshop",
00428:                     required_skill_id = "skill_workshop_sense",
00429:                     bonus_type = "crafting_yield",
00430:                     bonus_magnitude = 0.20f,
00431:                     penalty_magnitude = -0.05f
00432:                 },
00433:                 new ShelterAssignmentRuleDef
00434:                 {
00435:                     id = "rule_radio_communications",
00436:                     name = "Radio Signal Processing",
00437:                     description = "Trained signal operators discern faint broadcasts through background ionospheric noise.",
00438:                     target_room_function = "RadioRoom",
00439:                     required_skill_id = "skill_signal_ear",
00440:                     bonus_type = "signal_clarity",
00441:                     bonus_magnitude = 0.25f,
00442:                     penalty_magnitude = -0.10f
00443:                 },
00444:                 new ShelterAssignmentRuleDef
00445:                 {
00446:                     id = "rule_kitchen_nutrition",
00447:                     name = "Canteen Ration Mastery",
00448:                     description = "Skilled cooks stretch limited calories without sacrificing survivor nutrition or morale.",
00449:                     target_room_function = "Kitchen",
00450:                     required_skill_id = "skill_ration_stretcher",
00451:                     bonus_type = "meal_efficiency",
00452:                     bonus_magnitude = 0.25f,
00453:                     penalty_magnitude = -0.10f
00454:                 },
00455:                 new ShelterAssignmentRuleDef
00456:                 {
00457:                     id = "rule_laboratory_analysis",
00458:                     name = "Scientific Analysis",
00459:                     description = "Disciplined researchers accelerate knowledge decoding from recovered technical archives.",
00460:                     target_room_function = "Laboratory",
00461:                     required_skill_id = "skill_cold_analysis",
00462:                     bonus_type = "research_speed",
00463:                     bonus_magnitude = 0.30f,
00464:                     penalty_magnitude = -0.10f
00465:                 },
00466:                 new ShelterAssignmentRuleDef
00467:                 {
00468:                     id = "rule_greenhouse_botany",
00469:                     name = "Hydroponic Cultivation",
00470:                     description = "Hardy growers optimize nutrient feeds to boost harvest yields in artificial lighting.",
00471:                     target_room_function = "Greenhouse",
00472:                     required_skill_id = "skill_mycology",
00473:                     bonus_type = "harvest_yield",
00474:                     bonus_magnitude = 0.25f,
00475:                     penalty_magnitude = -0.05f
00476:                 },
00477:                 new ShelterAssignmentRuleDef
00478:                 {
00479:                     id = "rule_generator_maintenance",
00480:                     name = "Turbine & Power Tuning",
00481:                     description = "Experienced engineers reduce fuel consumption and prevent unexpected grid brownouts.",
00482:                     target_room_function = "GeneratorRoom",
00483:                     required_skill_id = "skill_rough_repairs",
00484:                     bonus_type = "fuel_efficiency",
00485:                     bonus_magnitude = 0.20f,
00486:                     penalty_magnitude = -0.10f
00487:                 },
00488:                 new ShelterAssignmentRuleDef
00489:                 {
00490:                     id = "rule_armory_service",
00491:                     name = "Armory Maintenance",
00492:                     description = "Watchful armorers keep firearms cleaned and expedition equipment primed.",
00493:                     target_room_function = "Armory",
00494:                     required_skill_id = "skill_watchful",
00495:                     bonus_type = "weapon_maintenance",
00496:                     bonus_magnitude = 0.20f,
00497:                     penalty_magnitude = -0.05f
00498:                 },
00499:                 new ShelterAssignmentRuleDef
00500:                 {
00501:                     id = "rule_storage_logistics",
00502:                     name = "Logistics Organization",
00503:                     description = "Organized quartermasters streamline inventory handling and reduce spoilage.",
00504:                     target_room_function = "Storage",
00505:                     required_skill_id = "skill_quartermaster",
00506:                     bonus_type = "handling_speed",
00507:                     bonus_magnitude = 0.15f,
00508:                     penalty_magnitude = -0.05f
00509:                 },
00510:                 new ShelterAssignmentRuleDef
00511:                 {
00512:                     id = "rule_airlock_decontamination",
00513:                     name = "Airlock Protocol",
00514:                     description = "Proper decontamination procedures accelerate expedition turnaround and scrub fallout.",
00515:                     target_room_function = "Airlock",
00516:                     required_skill_id = "skill_field_dressing",
00517:                     bonus_type = "turnaround_speed",
00518:                     bonus_magnitude = 0.20f,
00519:                     penalty_magnitude = -0.05f
00520:                 },
00521:                 new ShelterAssignmentRuleDef
00522:                 {
00523:                     id = "rule_dormitory_caretaker",
00524:                     name = "Shelter Caretaking",
00525:                     description = "Attentive caretakers maintain clean living quarters, improving rest quality and morale.",
00526:                     target_room_function = "Dormitory",
00527:                     required_skill_id = "skill_hard_living",
00528:                     bonus_type = "rest_quality",
00529:                     bonus_magnitude = 0.15f,
00530:                     penalty_magnitude = 0.0f
00531:                 }
00532:             };
00533:         }
00534:     }
00535: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` — complete current file

- Size: 388 lines / 15192 bytes.
- SHA-256: `1f43a02984d0efdce124420391e5efeb2b30feabe8069c77decc97c321f020fa`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Shelter
00007: {
00008:     /// <summary>
00009:     /// ASHFALL Shelter Assignment System (item 3).
00010:     ///
00011:     /// Tracks survivor-to-room/workstation assignments, room capacity,
00012:     /// eligibility, and assignment status. The Core owns the deterministic
00013:     /// assignment logic; the host reuses the existing HoldfastInteriorView,
00014:     /// RoomHotspotView, and SurvivorActorView to render and interact.
00015:     /// </summary>
00016:     public sealed class ShelterAssignmentSystem
00017:     {
00018:         private readonly ShelterAssignmentState _state;
00019:         private readonly List<ShelterRoom> _rooms;
00020:         private readonly Dictionary<string, int> _baseCapacities;
00021:
00022:         public event Action<ShelterAssignmentEvent>? OnAssignmentChanged;
00023:
00024:         public ShelterAssignmentSystem(ShelterAssignmentState state,
00025:             IEnumerable<ShelterRoom> rooms, ISeededRng rng)
00026:         {
00027:             if (state == null) throw new ArgumentNullException(nameof(state));
00028:             if (rooms == null) throw new ArgumentNullException(nameof(rooms));
00029:             _ = rng ?? throw new ArgumentNullException(nameof(rng));
00030:
00031:             _rooms = new List<ShelterRoom>();
00032:             _baseCapacities = new Dictionary<string, int>(StringComparer.Ordinal);
00033:             var roomIds = new HashSet<string>(StringComparer.Ordinal);
00034:             foreach (var room in rooms)
00035:             {
00036:                 if (room == null || string.IsNullOrWhiteSpace(room.RoomId)) continue;
00037:                 string roomId = room.RoomId.Trim();
00038:                 if (!roomIds.Add(roomId)) continue;
00039:                 _rooms.Add(CloneRoom(room, roomId));
00040:                 _baseCapacities[roomId] = Math.Max(0, room.Capacity);
00041:             }
00042:             if (_rooms.Count == 0)
00043:                 throw new InvalidOperationException("ShelterAssignmentSystem: at least one room required.");
00044:
00045:             _state = new ShelterAssignmentState();
00046:             _state.RestoreInto(state, _rooms);
00047:         }
00048:
00049:         public IReadOnlyList<ShelterRoom> Rooms => _rooms;
00050:         public ShelterAssignmentState State => _state;
00051:
00052:         public IReadOnlyList<ShelterAssignment> GetAssignments() => _state.Assignments;
00053:
00054:         public ShelterAssignment? GetAssignmentForSurvivor(string survivorId)
00055:         {
00056:             if (string.IsNullOrEmpty(survivorId)) return null;
00057:             for (int i = 0; i < _state.Assignments.Count; i++)
00058:             {
00059:                 var assignment = _state.Assignments[i];
00060:                 if (assignment != null
00061:                     && string.Equals(assignment.SurvivorId, survivorId, StringComparison.Ordinal))
00062:                     return assignment;
00063:             }
00064:             return null;
00065:         }
00066:
00067:         public IReadOnlyList<ShelterAssignment> GetAssignmentsForRoom(string roomId)
00068:         {
00069:             var list = new List<ShelterAssignment>();
00070:             if (string.IsNullOrEmpty(roomId)) return list;
00071:             for (int i = 0; i < _state.Assignments.Count; i++)
00072:             {
00073:                 var assignment = _state.Assignments[i];
00074:                 if (assignment != null
00075:                     && assignment.Status == ShelterAssignmentStatus.Active
00076:                     && string.Equals(assignment.RoomId, roomId, StringComparison.Ordinal))
00077:                     list.Add(assignment);
00078:             }
00079:             return list;
00080:         }
00081:
00082:         public int GetRoomOccupancy(string roomId)
00083:         {
00084:             int n = 0;
00085:             for (int i = 0; i < _state.Assignments.Count; i++)
00086:             {
00087:                 var assignment = _state.Assignments[i];
00088:                 if (assignment != null
00089:                     && assignment.Status == ShelterAssignmentStatus.Active
00090:                     && string.Equals(assignment.RoomId, roomId, StringComparison.Ordinal))
00091:                     n++;
00092:             }
00093:             return n;
00094:         }
00095:
00096:         /// <summary>
00097:         /// Null-safe ordinal check: returns true if two distinct survivors are actively assigned to the same shelter room.
00098:         /// </summary>
00099:         public bool AreInSameRoom(string? survivorA, string? survivorB)
00100:         {
00101:             if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB)) return false;
00102:             if (string.Equals(survivorA, survivorB, StringComparison.Ordinal)) return false;
00103:
00104:             var assignA = GetAssignmentForSurvivor(survivorA);
00105:             var assignB = GetAssignmentForSurvivor(survivorB);
00106:
00107:             if (assignA == null || assignB == null) return false;
00108:             if (assignA.Status != ShelterAssignmentStatus.Active || assignB.Status != ShelterAssignmentStatus.Active) return false;
00109:             if (string.IsNullOrEmpty(assignA.RoomId) || string.IsNullOrEmpty(assignB.RoomId)) return false;
00110:
00111:             return string.Equals(assignA.RoomId, assignB.RoomId, StringComparison.Ordinal);
00112:         }
00113:
00114:         public int GetRoomCapacity(string roomId)
00115:         {
00116:             for (int i = 0; i < _rooms.Count; i++)
00117:                 if (_rooms[i].RoomId == roomId) return _rooms[i].Capacity;
00118:             return 0;
00119:         }
00120:
00121:         /// <summary>
00122:         /// Reprojects authored room capacities plus completed construction bonuses.
00123:         /// The supplied map is derived from ShelterExpansionSystem's completed
00124:         /// rooms; repeated calls replace, rather than stack, those bonuses.
00125:         /// </summary>
00126:         public void ApplyCapacityBonuses(IReadOnlyDictionary<string, int> capacityBonuses)
00127:         {
00128:             foreach (var room in _rooms)
00129:             {
00130:                 _baseCapacities.TryGetValue(room.RoomId, out int baseCapacity);
00131:                 int bonus = 0;
00132:                 if (capacityBonuses != null)
00133:                     capacityBonuses.TryGetValue(room.RoomId, out bonus);
00134:                 room.Capacity = Math.Max(0, baseCapacity + Math.Max(0, bonus));
00135:             }
00136:         }
00137:
00138:         public bool CanAssign(string survivorId, string roomId)
00139:         {
00140:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(roomId))
00141:                 return false;
00142:             var room = FindRoom(roomId);
00143:             if (room == null) return false;
00144:             if (GetRoomOccupancy(roomId) >= room.Capacity) return false;
00145:             var existing = GetAssignmentForSurvivor(survivorId);
00146:             if (existing?.Status == ShelterAssignmentStatus.Active) return false;
00147:             return true;
00148:         }
00149:
00150:         public ShelterAssignmentResult Assign(string survivorId, string roomId,
00151: string? workstationId = null, int day = 0)
00152:         {
00153:             if (string.IsNullOrEmpty(survivorId))
00154:                 return new ShelterAssignmentResult(false, "missing_survivor_id", null!);
00155:             if (string.IsNullOrEmpty(roomId))
00156:                 return new ShelterAssignmentResult(false, "missing_room_id", null!);
00157:             var room = FindRoom(roomId);
00158:             if (room == null)
00159:                 return new ShelterAssignmentResult(false, "unknown_room", null!);
00160:             var existing = GetAssignmentForSurvivor(survivorId);
00161:             if (existing?.Status == ShelterAssignmentStatus.Active)
00162:                 return new ShelterAssignmentResult(false, "already_assigned", null!);
00163:             if (GetRoomOccupancy(roomId) >= room.Capacity)
00164:                 return new ShelterAssignmentResult(false, "room_full", null!);
00165:
00166:             ShelterAssignment assignment;
00167:             if (existing != null)
00168:             {
00169:                 existing.RoomId = roomId;
00170:                 existing.WorkstationId = workstationId ?? string.Empty;
00171:                 existing.AssignedDay = day;
00172:                 existing.Status = ShelterAssignmentStatus.Active;
00173:                 assignment = existing;
00174:             }
00175:             else
00176:             {
00177:                 assignment = new ShelterAssignment
00178:                 {
00179:                     SurvivorId = survivorId,
00180:                     RoomId = roomId,
00181:                     WorkstationId = workstationId ?? string.Empty,
00182:                     AssignedDay = day,
00183:                     Status = ShelterAssignmentStatus.Active
00184:                 };
00185:                 _state.Assignments.Add(assignment);
00186:             }
00187:             OnAssignmentChanged?.Invoke(new ShelterAssignmentEvent(
00188:                 ShelterAssignmentEventKind.Assigned, survivorId, roomId, day));
00189:             return new ShelterAssignmentResult(true, "ok", assignment);
00190:         }
00191:
00192:         public ShelterAssignmentResult Unassign(string survivorId, int day = 0)
00193:         {
00194:             if (string.IsNullOrEmpty(survivorId))
00195:                 return new ShelterAssignmentResult(false, "missing_survivor_id", null!);
00196:             for (int i = 0; i < _state.Assignments.Count; i++)
00197:             {
00198:                 if (_state.Assignments[i] != null
00199:                     && string.Equals(_state.Assignments[i].SurvivorId, survivorId, StringComparison.Ordinal))
00200:                 {
00201:                     string roomId = _state.Assignments[i].RoomId;
00202:                     _state.Assignments.RemoveAt(i);
00203:                     OnAssignmentChanged?.Invoke(new ShelterAssignmentEvent(
00204:                         ShelterAssignmentEventKind.Unassigned, survivorId, roomId, day));
00205:                     return new ShelterAssignmentResult(true, "ok", null!);
00206:                 }
00207:             }
00208:             return new ShelterAssignmentResult(false, "not_assigned", null!);
00209:         }
00210:
00211:         public ShelterAssignmentState CaptureState() => _state.Capture();
00212:
00213:         public void RestoreState(ShelterAssignmentState state)
00214:         {
00215:             if (state == null) throw new ArgumentNullException(nameof(state));
00216:             _state.RestoreInto(state, _rooms);
00217:         }
00218:
00219:         private ShelterRoom? FindRoom(string roomId)
00220:         {
00221:             for (int i = 0; i < _rooms.Count; i++)
00222:                 if (string.Equals(_rooms[i].RoomId, roomId, StringComparison.Ordinal)) return _rooms[i];
00223:             return null;
00224:         }
00225:
00226:         private static ShelterRoom CloneRoom(ShelterRoom source, string roomId)
00227:         {
00228:             return new ShelterRoom
00229:             {
00230:                 RoomId = roomId,
00231:                 DisplayName = source.DisplayName ?? string.Empty,
00232:                 Capacity = Math.Max(0, source.Capacity),
00233:                 RequiredSkillId = source.RequiredSkillId ?? string.Empty,
00234:                 WorkstationId = source.WorkstationId ?? string.Empty
00235:             };
00236:         }
00237:     }
00238:
00239:     [Serializable]
00240:     public sealed class ShelterRoom
00241:     {
00242:         public string RoomId;
00243:         public string DisplayName;
00244:         public int Capacity;
00245:         public string RequiredSkillId; // optional gating; empty = no requirement
00246:         public string WorkstationId; // optional default workstation
00247:
00248:         public ShelterRoom() { }
00249:
00250:         public ShelterRoom(string roomId, string displayName, int capacity,
00251: string? requiredSkillId = null, string? workstationId = null)
00252:         {
00253:             RoomId = roomId;
00254:             DisplayName = displayName;
00255:             Capacity = capacity;
00256:             RequiredSkillId = requiredSkillId;
00257:             WorkstationId = workstationId;
00258:         }
00259:     }
00260:
00261:     [Serializable]
00262:     public sealed class ShelterAssignment
00263:     {
00264:         public string SurvivorId;
00265:         public string RoomId;
00266:         public string WorkstationId;
00267:         public int AssignedDay;
00268:         public ShelterAssignmentStatus Status;
00269:
00270:         public ShelterAssignment() { }
00271:     }
00272:
00273:     public enum ShelterAssignmentStatus
00274:     {
00275:         Active = 0,
00276:         OnLeave = 1,
00277:         Decommissioned = 2
00278:     }
00279:
00280:     [Serializable]
00281:     public sealed class ShelterAssignmentState
00282:     {
00283:         public List<ShelterAssignment> Assignments = new List<ShelterAssignment>();
00284:
00285:         public void NormalizeAndValidate(IReadOnlyList<ShelterRoom> rooms)
00286:         {
00287:             Assignments ??= new List<ShelterAssignment>();
00288:             var validIds = new HashSet<string>(StringComparer.Ordinal);
00289:             for (int i = 0; i < rooms.Count; i++) validIds.Add(rooms[i].RoomId);
00290:
00291:             // De-duplicate survivor ids (last write wins).
00292:             var seen = new HashSet<string>(StringComparer.Ordinal);
00293:             for (int i = Assignments.Count - 1; i >= 0; i--)
00294:             {
00295:                 var assignment = Assignments[i];
00296:                 if (assignment == null || string.IsNullOrWhiteSpace(assignment.SurvivorId) ||
00297:                     string.IsNullOrWhiteSpace(assignment.RoomId) ||
00298:                     !validIds.Contains(assignment.RoomId))
00299:                 {
00300:                     Assignments.RemoveAt(i);
00301:                     continue;
00302:                 }
00303:                 if (!seen.Add(assignment.SurvivorId))
00304:                 {
00305:                     Assignments.RemoveAt(i);
00306:                     continue;
00307:                 }
00308:                 if (!Enum.IsDefined(typeof(ShelterAssignmentStatus), assignment.Status))
00309:                     assignment.Status = ShelterAssignmentStatus.Decommissioned;
00310:                 assignment.SurvivorId = assignment.SurvivorId.Trim();
00311:                 assignment.RoomId = assignment.RoomId.Trim();
00312:                 assignment.WorkstationId ??= string.Empty;
00313:             }
00314:         }
00315:
00316:         public ShelterAssignmentState Capture()
00317:         {
00318:             var copy = new ShelterAssignmentState
00319:             {
00320:                 Assignments = new List<ShelterAssignment>()
00321:             };
00322:             if (Assignments == null) return copy;
00323:             foreach (var assignment in Assignments)
00324:             {
00325:                 if (assignment == null) continue;
00326:                 copy.Assignments.Add(new ShelterAssignment
00327:                 {
00328:                     SurvivorId = assignment.SurvivorId,
00329:                     RoomId = assignment.RoomId,
00330:                     WorkstationId = assignment.WorkstationId,
00331:                     AssignedDay = assignment.AssignedDay,
00332:                     Status = assignment.Status
00333:                 });
00334:             }
00335:             return copy;
00336:         }
00337:
00338:         public void RestoreInto(ShelterAssignmentState state, IReadOnlyList<ShelterRoom> rooms)
00339:         {
00340:             Assignments = state?.Capture().Assignments ?? new List<ShelterAssignment>();
00341:             NormalizeAndValidate(rooms);
00342:         }
00343:     }
00344:
00345:     public enum ShelterAssignmentEventKind
00346:     {
00347:         Assigned,
00348:         Unassigned
00349:     }
00350:
00351:     [Serializable]
00352:     public sealed class ShelterAssignmentEvent
00353:     {
00354:         public ShelterAssignmentEventKind Kind;
00355:         public string SurvivorId;
00356:         public string RoomId;
00357:         public int Day;
00358:
00359:         public ShelterAssignmentEvent() { }
00360:
00361:         public ShelterAssignmentEvent(ShelterAssignmentEventKind kind,
00362:             string survivorId, string roomId, int day)
00363:         {
00364:             Kind = kind;
00365:             SurvivorId = survivorId ?? string.Empty;
00366:             RoomId = roomId ?? string.Empty;
00367:             Day = day;
00368:         }
00369:     }
00370:
00371:     [Serializable]
00372:     public sealed class ShelterAssignmentResult
00373:     {
00374:         public bool Succeeded;
00375:         public string ReasonCode;
00376:         public ShelterAssignment Assignment;
00377:
00378:         public ShelterAssignmentResult() { }
00379:
00380:         public ShelterAssignmentResult(bool succeeded, string reasonCode,
00381:             ShelterAssignment assignment)
00382:         {
00383:             Succeeded = succeeded;
00384:             ReasonCode = reasonCode ?? string.Empty;
00385:             Assignment = assignment;
00386:         }
00387:     }
00388: }
```


# Appendix — Current Source Detail: `src/Host/ShelterAssignmentHostSession.cs`

### `src/Host/ShelterAssignmentHostSession.cs` — complete current file

- Size: 182 lines / 6888 bytes.
- SHA-256: `0e283e269d5b0be549d9b0077055df77c4ccc131db4fbfd5aa2ea5bbbd6fe646`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Shelter;
00006: using Godot;
00007:
00008: namespace AtomicWar.GodotApp
00009: {
00010:     /// <summary>
00011:     /// Shelter Assignment host session (item 3).
00012:     ///
00013:     /// Thin Godot-side glue: holds the Core ShelterAssignmentSystem, loads
00014:     /// and saves through ShelterAssignmentSaveStore, exposes the room list
00015:     /// to the existing HoldfastInteriorView/RoomHotspotView, and registers
00016:     /// with the Campaign Day Coordinator so per-day cleanup (decommissioned
00017:     /// assignments) runs at the right seam.
00018:     /// </summary>
00019:     public sealed class ShelterAssignmentHostSession
00020:     : HostSessionBase{
00021:         public ShelterAssignmentSystem System { get; private set; }
00022:
00023:         private readonly ISeededRng _rng;
00024:
00025:         public static ShelterAssignmentHostSession CreateDefault(ISeededRng rng, string? dataDir = null)
00026:         {
00027:             var catalog = ShelterRoomCatalogLoader.Load(dataDir ?? string.Empty);
00028:             var rooms = new List<ShelterRoom>();
00029:             if (catalog?.rooms != null)
00030:             {
00031:                 foreach (var def in catalog.rooms)
00032:                 {
00033:                     rooms.Add(new ShelterRoom(def.id, def.display_name, def.capacity, def.required_skill_id, def.workstation_id));
00034:                 }
00035:             }
00036:             if (rooms.Count == 0)
00037:             {
00038:                 rooms.Add(new ShelterRoom("room_bunker_corridor", "Central Access Corridor", 0));
00039:                 rooms.Add(new ShelterRoom("room_bunks", "Bunks", 4));
00040:                 rooms.Add(new ShelterRoom("room_kitchen", "Kitchen", 2, "skill_cooking"));
00041:                 rooms.Add(new ShelterRoom("room_clinic", "Clinic", 2, "skill_medic"));
00042:                 rooms.Add(new ShelterRoom("room_workshop", "Workshop", 2, "skill_crafting"));
00043:                 rooms.Add(new ShelterRoom("room_filtration", "Filtration Stack", 1, "skill_technician"));
00044:             }
00045:             return new ShelterAssignmentHostSession(rooms, new ShelterAssignmentState(), rng);
00046:         }
00047:
00048:         public ShelterAssignmentHostSession(List<ShelterRoom> rooms,
00049:             ShelterAssignmentState state, ISeededRng rng)
00050:         {
00051:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00052:             System = new ShelterAssignmentSystem(state, rooms, rng);
00053:             System.OnAssignmentChanged += _ => RaiseStateChanged();
00054:         }
00055:
00056:         public bool AreInSameRoom(string? a, string? b) => System?.AreInSameRoom(a, b) ?? false;
00057:
00058:         public bool TrySave()
00059:         {
00060:             var save = new ShelterAssignmentSave
00061:             {
00062:                 simDay = 0,
00063:                 Rooms = new List<ShelterRoomSave>(),
00064:                 State = System.CaptureState()
00065:             };
00066:             foreach (var r in System.Rooms)
00067:                 save.Rooms.Add(new ShelterRoomSave
00068:                 {
00069:                     RoomId = r.RoomId,
00070:                     DisplayName = r.DisplayName,
00071:                     Capacity = r.Capacity,
00072:                     RequiredSkillId = r.RequiredSkillId,
00073:                     WorkstationId = r.WorkstationId
00074:                 });
00075:             return ShelterAssignmentSaveStore.TrySave(save);
00076:         }
00077:
00078:         public bool TryLoad()
00079:         {
00080:             var loaded = ShelterAssignmentSaveStore.TryLoad();
00081:             if (loaded == null) return false;
00082:             System.State.RestoreInto(loaded.State, System.Rooms);
00083:             return true;
00084:         }
00085:
00086:         public override void Save()
00087:         {
00088:             if (!IsDirty) return;
00089:             TrySave();
00090:             base.Save();
00091:         }
00092:     }
00093:
00094:     /// <summary>
00095:     /// Save store for ShelterAssignmentSave (mirrors the other expansion stores).
00096:     /// </summary>
00097:     public static class ShelterAssignmentSaveStore
00098:     {
00099:         public const string FileName = "shelter_assignment_save.json";
00100:         public const string SectionName = "shelter_assignment";
00101:
00102:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
00103:         public static string TryCaptureDirect(ShelterAssignmentSave save)
00104:         {
00105:             return TryCapture(save);
00106:         }
00107:
00108:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
00109:         public static ShelterAssignmentSave? TryRestoreDirect(string json)
00110:         {
00111:             return TryRestore(json);
00112:         }
00113:
00114:         /// <summary>Capture state to JSON without writing to disk.</summary>
00115:         public static string TryCapture(ShelterAssignmentSave save)
00116:         {
00117:             try
00118:             {
00119:                 if (save == null) return string.Empty;
00120:                 return ShelterAssignmentSaveCodec.EncodeToString(save, s_json);
00121:             }
00122:             catch (Exception e)
00123:             {
00124:                 s_log.Error("[ShelterAssignmentSaveStore] capture failed: " + e.Message);
00125:                 return string.Empty;
00126:             }
00127:         }
00128:
00129:         /// <summary>Restore state from JSON without reading from disk.</summary>
00130:         public static ShelterAssignmentSave? TryRestore(string json)
00131:         {
00132:             try
00133:             {
00134:                 if (string.IsNullOrWhiteSpace(json)) return null;
00135:                 return ShelterAssignmentSaveCodec.Decode(json, s_json);
00136:             }
00137:             catch (Exception e)
00138:             {
00139:                 s_log.Error("[ShelterAssignmentSaveStore] restore failed: " + e.Message);
00140:                 return null;
00141:             }
00142:         }
00143:
00144:         private static readonly IFileIO s_files = new FileSystemIO();
00145:         private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
00146:         private static readonly ILog s_log = new GodotLog();
00147:
00148:         public static string SavePath => SaveSlotRoot.Resolve(FileName);
00149:
00150:         public static bool TrySave(ShelterAssignmentSave save)
00151:         {
00152:             if (save == null) return false;
00153:             try
00154:             {
00155:                 s_files.WriteAllText(SavePath, ShelterAssignmentSaveCodec.EncodeToString(save, s_json));
00156:                 return true;
00157:             }
00158:             catch (Exception e)
00159:             {
00160:                 s_log.Error("[ShelterAssignmentSaveStore] save failed: " + e.Message);
00161:                 return false;
00162:             }
00163:         }
00164:
00165:         public static ShelterAssignmentSave? TryLoad()
00166:         {
00167:             try
00168:             {
00169:                 if (!s_files.FileExists(SavePath)) return null;
00170:                 return ShelterAssignmentSaveCodec.Decode(s_files.ReadAllText(SavePath), s_json);
00171:             }
00172:             catch (Exception e)
00173:             {
00174:                 s_log.Error("[ShelterAssignmentSaveStore] load failed: " + e.Message);
00175:                 return null;
00176:             }
00177:         }
00178:
00179:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
00180:         public static string TryCapturePersisted(ShelterAssignmentSave save) => TryCapture(save);
00181:     }
00182: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs`

### `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` — complete current file

- Size: 79 lines / 3350 bytes.
- SHA-256: `0cf98c21764ca49c6dadf211d2aec0ae053cf186b60b89b8a507deb2a3f42aa9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core.Shelter;
00006:
00007: namespace Ashfall.Core.Shelter
00008: {
00009:     /// <summary>Checksummed save envelope for shelter assignments.</summary>
00010:     [Serializable]
00011:     public class ShelterAssignmentSave
00012:     {
00013:         public const int CurrentSaveVersion = 1;
00014:         public const int MigrationFromVersion = 1;
00015:
00016:         public int saveVersion = CurrentSaveVersion;
00017:         public int simDay;
00018:         public List<ShelterRoomSave> Rooms = new List<ShelterRoomSave>();
00019:         public ShelterAssignmentState State = new ShelterAssignmentState();
00020:         public string Checksum = string.Empty;
00021:     }
00022:
00023:     [Serializable]
00024:     public sealed class ShelterRoomSave
00025:     {
00026:         public string RoomId;
00027:         public string DisplayName;
00028:         public int Capacity;
00029:         public string RequiredSkillId;
00030:         public string WorkstationId;
00031:     }
00032:
00033:     public static class ShelterAssignmentSaveCodec
00034:     {
00035:         public static ShelterAssignmentSave Encode(ShelterAssignmentSave save, IJsonSerializer json)
00036:         {
00037:             if (save == null) throw new ArgumentNullException(nameof(save));
00038:             if (save.saveVersion > ShelterAssignmentSave.CurrentSaveVersion)
00039:                 throw new InvalidOperationException(
00040:                     "ShelterAssignmentSave: refusing to encode a saveVersion newer than supported.");
00041:             save.Checksum = SaveChecksum.Compute(save);
00042:             return save;
00043:         }
00044:
00045:         public static string EncodeToString(ShelterAssignmentSave save, IJsonSerializer json)
00046:         {
00047:             Encode(save, json);
00048:             return json.Serialize(save);
00049:         }
00050:
00051:         public static ShelterAssignmentSave Decode(string jsonText, IJsonSerializer json)
00052:         {
00053:             if (string.IsNullOrWhiteSpace(jsonText))
00054:                 throw new InvalidOperationException("ShelterAssignmentSave: empty save payload.");
00055:             ShelterAssignmentSave save;
00056:             try { save = json.Deserialize<ShelterAssignmentSave>(jsonText!); }
00057:             catch (Exception e)
00058:             {
00059:                 throw new InvalidOperationException(
00060:                     "ShelterAssignmentSave: malformed save payload: " + e.Message, e);
00061:             }
00062:             if (save == null)
00063:                 throw new InvalidOperationException("ShelterAssignmentSave: empty save payload.");
00064:             if (save.saveVersion > ShelterAssignmentSave.CurrentSaveVersion)
00065:                 throw new InvalidOperationException(
00066:                     "ShelterAssignmentSave: saveVersion " + save.saveVersion + " is newer than supported.");
00067:             if (save.saveVersion < ShelterAssignmentSave.MigrationFromVersion)
00068:                 throw new InvalidOperationException("ShelterAssignmentSave: invalid saveVersion.");
00069:             if (string.IsNullOrEmpty(save.Checksum))
00070:                 throw new InvalidOperationException(
00071:                     "ShelterAssignmentSave: save carries no checksum (truncated or tampered file).");
00072:             string actual = SaveChecksum.Compute(save);
00073:             if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
00074:                 throw new InvalidOperationException(
00075:                     "ShelterAssignmentSave: checksum mismatch (corrupt or foreign save).");
00076:             return save;
00077:         }
00078:     }
00079: }
```


# Appendix — Current Source Detail: `src/Main.Sanitation.cs`

### `src/Main.Sanitation.cs` — complete current file

- Size: 125 lines / 5202 bytes.
- SHA-256: `29709b443b23f186819429e3ab59ffb953b7a0a9e60f356062a24a973fa9ff51`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Main Partial : Plan 210 — Waste Management & Sanitation host wire
00004: // Subsystems   : facility catalog, room registration (tag→role), daily
00005: //                population feed, compost→inventory delivery, save section.
00006: // ============================================================================
00007: using System;
00008: using System.Linq;
00009: using Godot;
00010: using Ashfall.Core;
00011: using Ashfall.Core.Shelter;
00012:
00013: namespace AtomicWar.GodotApp
00014: {
00015:     public partial class Main
00016:     {
00017:         private SanitationHostSession? _sanitation;
00018:         private bool _sanitationDirty;
00019:
00020:         public SanitationHostSession EnsureSanitationSession()
00021:         {
00022:             SetupSanitation();
00023:             return _sanitation!;
00024:         }
00025:
00026:         private void SetupSanitation()
00027:         {
00028:             if (_sanitation != null)
00029:             {
00030:                 // Plan 210 follow-up — feed the live power grid every visit;
00031:                 // the provider is evaluated at facility-tick time, so late
00032:                 // grid composition is picked up without re-binding.
00033:                 if (_sanitation.System.RoomPowerProvider == null)
00034:                     _sanitation.System.RoomPowerProvider = roomId =>
00035:                         _powerGrid?.System != null && _powerGrid.System.IsRoomPowered(roomId);
00036:                 return;
00037:             }
00038:
00039:             _sanitation = SanitationHostSession.Create(_dataDir);
00040:             _sanitation.System.RoomPowerProvider = roomId =>
00041:                 _powerGrid?.System != null && _powerGrid.System.IsRoomPowered(roomId);
00042:             _sanitation.StateChanged += () =>
00043:             {
00044:                 _sanitationDirty = true;
00045:                 _economyPanel?.RefreshView();
00046:                 if (_state == GameState.Playing) UpdateHud();
00047:             };
00048:
00049:             // Register the authored rooms once (EnsureRoom is idempotent).
00050:             // Role mapping follows the canonical shelter_rooms.json tags.
00051:             var rooms = ShelterRoomCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00052:             foreach (var room in rooms.rooms)
00053:             {
00054:                 if (room == null || string.IsNullOrEmpty(room.id)) continue;
00055:                 _sanitation.System.EnsureRoom(room.id, RoleForRoom(room));
00056:             }
00057:
00058:             // Compost output lands in the canonical inventory authority.
00059:             _sanitation.System.OnCompostReady += DeliverCompostOutput;
00060:
00061:             var saved = SanitationSaveStore.TryLoad();
00062:             if (saved != null)
00063:             {
00064:                 _sanitation.System.RestoreState(saved);
00065:                 _sanitationDirty = false; // restore just raised state-change events
00066:                 GD.Print("[Ashfall Godot] Sanitation state restored.");
00067:             }
00068:         }
00069:
00070:         private static RoomWasteRole RoleForRoom(ShelterRoomDef room)
00071:         {
00072:             var tags = room.tags ?? new System.Collections.Generic.List<string>();
00073:             if (tags.Contains("residential")) return RoomWasteRole.Residential;
00074:             if (tags.Contains("canteen") || tags.Contains("nutrition")) return RoomWasteRole.FoodPrep;
00075:             if (tags.Contains("heavy_industrial") || tags.Contains("crafting") || tags.Contains("power")) return RoomWasteRole.Industrial;
00076:             if (tags.Contains("medical") || tags.Contains("triage") || tags.Contains("surgery") || tags.Contains("quarantine")) return RoomWasteRole.Medical;
00077:             return RoomWasteRole.Other;
00078:         }
00079:
00080:         private void DeliverCompostOutput(CompostBatchState batch)
00081:         {
00082:             if (batch == null || string.IsNullOrEmpty(batch.outputItemId) || batch.outputUnits <= 0f) return;
00083:             var inv = _inventory?.Inventory;
00084:             if (inv == null) return;
00085:             int units = (int)Math.Floor(batch.outputUnits);
00086:             if (units <= 0) return;
00087:             if (inv.AddById(batch.outputItemId, units))
00088:             {
00089:                 GD.Print($"[Ashfall Godot] Compost delivered: {units} × {batch.outputItemId}");
00090:             }
00091:         }
00092:
00093:         private void SaveSanitation()
00094:         {
00095:             if (_sanitation == null) return;
00096:             CaptureSection("sanitation", SanitationSaveStore.TryCapturePersisted(_sanitation.CaptureSave()));
00097:         }
00098:
00099:         private void FlushSanitationIfDirty()
00100:         {
00101:             if (_sanitationDirty) SaveSanitation();
00102:         }
00103:
00104:         // ── Panels (Plan 210 Phase 9): created hidden; opened via the
00105:         //    expanded-panel route. Presentation only.
00106:         private UI.SanitationPanel? _sanitationPanel;
00107:
00108:         private void EnsureSanitationPanel()
00109:         {
00110:             if (_sanitation == null) return;
00111:             if (_sanitationPanel != null) return;
00112:             _sanitationPanel = new UI.SanitationPanel();
00113:             _sanitationPanel.Bind(_sanitation);
00114:             _sanitationPanel.Visible = false;
00115:             AddChild(_sanitationPanel);
00116:         }
00117:
00118:         private void OpenSanitationPanel()
00119:         {
00120:             SetupSanitation();
00121:             EnsureSanitationPanel();
00122:             if (_sanitationPanel != null) { _sanitationPanel.Visible = true; _sanitationPanel.RefreshView(); }
00123:         }
00124:     }
00125: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs` — complete current file

- Size: 166 lines / 6444 bytes.
- SHA-256: `5121939d1dbac0de2d70af77bb20e6f8e34687a61d5d0ece18179cb7e4fe12d3`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Shelter;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests.Shelter
00010: {
00011:     public class ShelterRoomCatalogTests
00012:     {
00013:         private static string GetDataPath()
00014:         {
00015:             return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "StreamingAssets", "Data");
00016:         }
00017:
00018:         [Fact]
00019:         public void DefaultCatalog_Contains22RoomsAnd12Rules()
00020:         {
00021:             var catalog = ShelterRoomCatalogLoader.GetDefaultCatalog();
00022:             Assert.NotNull(catalog);
00023:             Assert.Equal(1, catalog.schema_version);
00024:             Assert.True(catalog.rooms.Count >= 20, $"Expected at least 20 rooms, got {catalog.rooms.Count}");
00025:             Assert.Equal(12, catalog.assignment_rules.Count);
00026:         }
00027:
00028:         [Fact]
00029:         public void LoadFromFile_ParsesCorrectly()
00030:         {
00031:             string dataDir = GetDataPath();
00032:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00033:             Assert.NotNull(catalog);
00034:             Assert.True(catalog.rooms.Count >= 20);
00035:             Assert.Equal(12, catalog.assignment_rules.Count);
00036:
00037:             var corridor = catalog.rooms.FirstOrDefault(r => r.id == "room_bunker_corridor");
00038:             Assert.NotNull(corridor);
00039:             Assert.Equal(0, corridor.capacity);
00040:             Assert.Equal("Corridor", corridor.function);
00041:
00042:             var kitchen = catalog.rooms.FirstOrDefault(r => r.id == "room_kitchen");
00043:             Assert.NotNull(kitchen);
00044:             Assert.Equal("Kitchen", kitchen.function);
00045:             Assert.Equal(2, kitchen.capacity);
00046:         }
00047:
00048:         [Fact]
00049:         public void AllRoomIds_AreUniqueAndValidPrefix()
00050:         {
00051:             string dataDir = GetDataPath();
00052:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00053:
00054:             var ids = catalog.rooms.Select(r => r.id).ToList();
00055:             var distinctIds = ids.Distinct().ToList();
00056:             Assert.Equal(ids.Count, distinctIds.Count);
00057:
00058:             foreach (var id in ids)
00059:             {
00060:                 Assert.StartsWith("room_", id);
00061:             }
00062:         }
00063:
00064:         [Fact]
00065:         public void AllRuleIds_AreUniqueAndValidPrefix()
00066:         {
00067:             string dataDir = GetDataPath();
00068:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00069:
00070:             var ids = catalog.assignment_rules.Select(r => r.id).ToList();
00071:             var distinctIds = ids.Distinct().ToList();
00072:             Assert.Equal(ids.Count, distinctIds.Count);
00073:
00074:             foreach (var id in ids)
00075:             {
00076:                 Assert.StartsWith("rule_", id);
00077:                 Assert.False(string.IsNullOrWhiteSpace(id));
00078:             }
00079:         }
00080:
00081:         [Fact]
00082:         public void AssignmentRules_TargetValidFunctions()
00083:         {
00084:             string dataDir = GetDataPath();
00085:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00086:
00087:             var roomFunctions = catalog.rooms.Select(r => r.function).Distinct().ToHashSet();
00088:
00089:             foreach (var rule in catalog.assignment_rules)
00090:             {
00091:                 Assert.Contains(rule.target_room_function, roomFunctions);
00092:                 Assert.True(rule.bonus_magnitude > 0);
00093:             }
00094:         }
00095:
00096:         [Fact]
00097:         public void ShelterAssignmentSystem_LoadsFromCatalogRooms()
00098:         {
00099:             string dataDir = GetDataPath();
00100:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00101:
00102:             var rooms = catalog.rooms.Select(r => new ShelterRoom(r.id, r.display_name, r.capacity, r.required_skill_id, r.workstation_id)).ToList();
00103:             var rng = new SeededRng(42);
00104:             var system = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
00105:
00106:             Assert.Equal(catalog.rooms.Count, system.Rooms.Count);
00107:
00108:             // Assign a survivor to the kitchen
00109:             var result = system.Assign("chef_elena", "room_kitchen", day: 1);
00110:             Assert.True(result.Succeeded);
00111:             Assert.Equal(1, system.GetRoomOccupancy("room_kitchen"));
00112:         }
00113:
00114:         [Fact]
00115:         public void DormitoryVariants_ReflectCapacityAndCostTradeoffs()
00116:         {
00117:             string dataDir = GetDataPath();
00118:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00119:
00120:             var crowded = catalog.rooms.First(r => r.id == "room_bunks_crowded");
00121:             var standard = catalog.rooms.First(r => r.id == "room_bunks");
00122:             var privateQ = catalog.rooms.First(r => r.id == "room_quarters_private");
00123:
00124:             Assert.True(crowded.capacity > standard.capacity);
00125:             Assert.True(standard.capacity > privateQ.capacity);
00126:         }
00127:
00128:         [Fact]
00129:         public void WorkshopVariants_TargetDistinctDisciplines()
00130:         {
00131:             string dataDir = GetDataPath();
00132:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00133:
00134:             var general = catalog.rooms.First(r => r.id == "room_workshop");
00135:             var heavy = catalog.rooms.First(r => r.id == "room_workshop_heavy");
00136:             var precision = catalog.rooms.First(r => r.id == "room_workshop_precision");
00137:
00138:             Assert.Contains("repair", general.tags);
00139:             Assert.Contains("heavy_industrial", heavy.tags);
00140:             Assert.Contains("precision", precision.tags);
00141:         }
00142:
00143:         [Fact]
00144:         public void SaveRoundTrip_WithCatalogRooms_PreservesState()
00145:         {
00146:             string dataDir = GetDataPath();
00147:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00148:             var rooms = catalog.rooms.Select(r => new ShelterRoom(r.id, r.display_name, r.capacity, r.required_skill_id, r.workstation_id)).ToList();
00149:
00150:             var sys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(100));
00151:             sys.Assign("survivor_1", "room_bunks", day: 2);
00152:             sys.Assign("survivor_2", "room_kitchen", day: 2);
00153:
00154:             var state = sys.CaptureState();
00155:             Assert.Equal(2, state.Assignments.Count);
00156:
00157:             var newSys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(101));
00158:             newSys.RestoreState(state);
00159:
00160:             Assert.Equal(2, newSys.GetAssignments().Count);
00161:             Assert.True(newSys.AreInSameRoom("survivor_1", "survivor_1") == false);
00162:             Assert.Equal("room_bunks", newSys.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00163:             Assert.Equal("room_kitchen", newSys.GetAssignmentForSurvivor("survivor_2")?.RoomId);
00164:         }
00165:     }
00166: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs` — complete current file

- Size: 333 lines / 12526 bytes.
- SHA-256: `64251d808239453605848adcddd7bf184ee0d0ffbcd79f7a0ffaa806c453ad98`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Shelter;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests.Shelter
00009: {
00010:     public class ShelterAssignmentSystemTests
00011:     {
00012:         private static ShelterAssignmentSystem MakeGrid(ISeededRng rng = null)
00013:         {
00014:             rng ??= new SeededRng(7);
00015:             var rooms = new List<ShelterRoom>
00016:             {
00017:                 new ShelterRoom("room_bunks", "Bunks", 4),
00018:                 new ShelterRoom("room_kitchen", "Kitchen", 2, "skill_cooking"),
00019:                 new ShelterRoom("room_clinic", "Clinic", 2, "skill_medic"),
00020:                 new ShelterRoom("room_workshop", "Workshop", 2, "skill_crafting")
00021:             };
00022:             return new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
00023:         }
00024:
00025:         [Fact]
00026:         public void Constructor_CapturedStateAndRooms_DoNotAliasAuthority()
00027:         {
00028:             var state = new ShelterAssignmentState
00029:             {
00030:                 Assignments = new List<ShelterAssignment>
00031:                 {
00032:                     new ShelterAssignment
00033:                     {
00034:                         SurvivorId = "survivor_1",
00035:                         RoomId = "room_bunks",
00036:                         Status = ShelterAssignmentStatus.Active
00037:                     }
00038:                 }
00039:             };
00040:             var rooms = new List<ShelterRoom>
00041:             {
00042:                 new ShelterRoom("room_bunks", "Bunks", 4)
00043:             };
00044:
00045:             var system = new ShelterAssignmentSystem(state, rooms, new SeededRng(1));
00046:             state.Assignments.Clear();
00047:             rooms[0].Capacity = 0;
00048:
00049:             Assert.Equal(4, system.GetRoomCapacity("room_bunks"));
00050:             Assert.Equal("room_bunks", system.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00051:         }
00052:
00053:         [Fact]
00054:         public void Capture_AssignmentObject_IsSnapshot()
00055:         {
00056:             var system = MakeGrid();
00057:             system.Assign("survivor_1", "room_bunks");
00058:
00059:             var snapshot = system.CaptureState();
00060:             snapshot.Assignments[0].RoomId = "tampered";
00061:
00062:             Assert.Equal("room_bunks", system.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00063:         }
00064:
00065:         [Fact]
00066:         public void Restore_CapturedState_DoesNotAliasInput()
00067:         {
00068:             var source = MakeGrid();
00069:             source.Assign("survivor_1", "room_bunks");
00070:             var saved = source.CaptureState();
00071:             var restored = MakeGrid();
00072:
00073:             restored.RestoreState(saved);
00074:             saved.Assignments[0].RoomId = "tampered";
00075:             saved.Assignments[0].Status = ShelterAssignmentStatus.Decommissioned;
00076:
00077:             Assert.Equal("room_bunks", restored.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00078:             Assert.Equal(ShelterAssignmentStatus.Active, restored.GetAssignmentForSurvivor("survivor_1")?.Status);
00079:         }
00080:
00081:         [Fact]
00082:         public void InactiveAssignments_DoNotConsumeRoomCapacityOrOccupancy()
00083:         {
00084:             var state = new ShelterAssignmentState
00085:             {
00086:                 Assignments = new List<ShelterAssignment>
00087:                 {
00088:                     new ShelterAssignment
00089:                     {
00090:                         SurvivorId = "survivor_1",
00091:                         RoomId = "room_bunks",
00092:                         Status = ShelterAssignmentStatus.OnLeave
00093:                     }
00094:                 }
00095:             };
00096:             var system = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00097:             {
00098:                 new ShelterRoom("room_bunks", "Bunks", 1)
00099:             }, new SeededRng(1));
00100:
00101:             Assert.Equal(0, system.GetRoomOccupancy("room_bunks"));
00102:             Assert.Empty(system.GetAssignmentsForRoom("room_bunks"));
00103:             Assert.True(system.CanAssign("survivor_2", "room_bunks"));
00104:             Assert.True(system.Assign("survivor_2", "room_bunks").Succeeded);
00105:             Assert.Equal(1, system.GetRoomOccupancy("room_bunks"));
00106:         }
00107:
00108:         [Fact]
00109:         public void Assign_ReactivatesInactiveAssignment()
00110:         {
00111:             var state = new ShelterAssignmentState
00112:             {
00113:                 Assignments = new List<ShelterAssignment>
00114:                 {
00115:                     new ShelterAssignment
00116:                     {
00117:                         SurvivorId = "survivor_1",
00118:                         RoomId = "room_bunks",
00119:                         Status = ShelterAssignmentStatus.Decommissioned
00120:                     }
00121:                 }
00122:             };
00123:             var system = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00124:             {
00125:                 new ShelterRoom("room_bunks", "Bunks", 1),
00126:                 new ShelterRoom("room_clinic", "Clinic", 1)
00127:             }, new SeededRng(1));
00128:
00129:             var result = system.Assign("survivor_1", "room_clinic", day: 9);
00130:
00131:             Assert.True(result.Succeeded);
00132:             Assert.Single(system.GetAssignments());
00133:             Assert.Equal("room_clinic", result.Assignment?.RoomId);
00134:             Assert.Equal(ShelterAssignmentStatus.Active, result.Assignment?.Status);
00135:         }
00136:
00137:         [Fact]
00138:         public void Constructor_NullAssignmentList_FailsClosed()
00139:         {
00140:             var state = new ShelterAssignmentState { Assignments = null! };
00141:
00142:             var system = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00143:             {
00144:                 new ShelterRoom("room_bunks", "Bunks", 1)
00145:             }, new SeededRng(1));
00146:
00147:             Assert.Empty(system.GetAssignments());
00148:             Assert.True(system.Assign("survivor_1", "room_bunks").Succeeded);
00149:         }
00150:
00151:         [Fact]
00152:         public void Assign_AddsAssignment()
00153:         {
00154:             var sys = MakeGrid();
00155:             var result = sys.Assign("elena_vasquez", "room_bunks", day: 5);
00156:             Assert.True(result.Succeeded);
00157:             Assert.NotNull(result.Assignment);
00158:             Assert.Single(sys.GetAssignments());
00159:         }
00160:
00161:         [Fact]
00162:         public void Assign_UnknownRoomFails()
00163:         {
00164:             var sys = MakeGrid();
00165:             var result = sys.Assign("elena_vasquez", "room_does_not_exist");
00166:             Assert.False(result.Succeeded);
00167:             Assert.Equal("unknown_room", result.ReasonCode);
00168:         }
00169:
00170:         [Fact]
00171:         public void Assign_AlreadyAssignedFails()
00172:         {
00173:             var sys = MakeGrid();
00174:             sys.Assign("elena_vasquez", "room_bunks");
00175:             var result = sys.Assign("elena_vasquez", "room_kitchen");
00176:             Assert.False(result.Succeeded);
00177:             Assert.Equal("already_assigned", result.ReasonCode);
00178:         }
00179:
00180:         [Fact]
00181:         public void Assign_RoomFullFails()
00182:         {
00183:             var sys = MakeGrid();
00184:             sys.Assign("s1", "room_bunks");
00185:             sys.Assign("s2", "room_bunks");
00186:             sys.Assign("s3", "room_bunks");
00187:             sys.Assign("s4", "room_bunks");
00188:             var result = sys.Assign("s5", "room_bunks");
00189:             Assert.False(result.Succeeded);
00190:             Assert.Equal("room_full", result.ReasonCode);
00191:         }
00192:
00193:         [Fact]
00194:         public void CanAssign_FalseWhenFull()
00195:         {
00196:             var sys = MakeGrid();
00197:             sys.Assign("s1", "room_bunks");
00198:             sys.Assign("s2", "room_bunks");
00199:             Assert.True(sys.CanAssign("s3", "room_bunks"));
00200:             sys.Assign("s3", "room_bunks");
00201:             sys.Assign("s4", "room_bunks");
00202:             Assert.False(sys.CanAssign("s5", "room_bunks"));
00203:         }
00204:
00205:         [Fact]
00206:         public void Unassign_RemovesAssignment()
00207:         {
00208:             var sys = MakeGrid();
00209:             sys.Assign("elena_vasquez", "room_bunks");
00210:             var result = sys.Unassign("elena_vasquez", day: 6);
00211:             Assert.True(result.Succeeded);
00212:             Assert.Empty(sys.GetAssignments());
00213:         }
00214:
00215:         [Fact]
00216:         public void Unassign_NotAssignedFails()
00217:         {
00218:             var sys = MakeGrid();
00219:             var result = sys.Unassign("ghost");
00220:             Assert.False(result.Succeeded);
00221:             Assert.Equal("not_assigned", result.ReasonCode);
00222:         }
00223:
00224:         [Fact]
00225:         public void GetOccupancy_AfterMultipleAssigns()
00226:         {
00227:             var sys = MakeGrid();
00228:             sys.Assign("s1", "room_bunks");
00229:             sys.Assign("s2", "room_bunks");
00230:             Assert.Equal(2, sys.GetRoomOccupancy("room_bunks"));
00231:             Assert.Equal(0, sys.GetRoomOccupancy("room_kitchen"));
00232:         }
00233:
00234:         [Fact]
00235:         public void Events_FireOnAssignAndUnassign()
00236:         {
00237:             var sys = MakeGrid();
00238:             var fired = new List<ShelterAssignmentEvent>();
00239:             sys.OnAssignmentChanged += e => fired.Add(e);
00240:             sys.Assign("elena_vasquez", "room_bunks", day: 3);
00241:             sys.Unassign("elena_vasquez", day: 4);
00242:             Assert.Equal(2, fired.Count);
00243:             Assert.Equal(ShelterAssignmentEventKind.Assigned, fired[0].Kind);
00244:             Assert.Equal(ShelterAssignmentEventKind.Unassigned, fired[1].Kind);
00245:         }
00246:
00247:         [Fact]
00248:         public void CaptureRestore_RoundTrip()
00249:         {
00250:             var sys = MakeGrid();
00251:             sys.Assign("elena_vasquez", "room_bunks");
00252:             sys.Assign("marcus_olejnik", "room_clinic");
00253:             var save = sys.CaptureState();
00254:             var fresh = MakeGrid();
00255:             fresh.RestoreState(save);
00256:             Assert.Equal(2, fresh.GetAssignments().Count);
00257:             Assert.NotNull(fresh.GetAssignmentForSurvivor("elena_vasquez"));
00258:             Assert.NotNull(fresh.GetAssignmentForSurvivor("marcus_olejnik"));
00259:         }
00260:
00261:         [Fact]
00262:         public void Save_RoundTrip_ChecksumStable()
00263:         {
00264:             var sys = MakeGrid();
00265:             sys.Assign("elena_vasquez", "room_bunks");
00266:             var save = new ShelterAssignmentSave
00267:             {
00268:                 simDay = 1,
00269:                 Rooms = new List<ShelterRoomSave>
00270:                 {
00271:                     new ShelterRoomSave { RoomId = "room_bunks", DisplayName = "Bunks", Capacity = 4 }
00272:                 },
00273:                 State = sys.CaptureState()
00274:             };
00275:             var json = new SystemTextJsonSerializer();
00276:             string text = ShelterAssignmentSaveCodec.EncodeToString(save, json);
00277:             var loaded = ShelterAssignmentSaveCodec.Decode(text, json);
00278:             Assert.Equal(save.Checksum, loaded.Checksum);
00279:             Assert.Single(loaded.State.Assignments);
00280:         }
00281:
00282:         [Fact]
00283:         public void Save_TamperedChecksumRejected()
00284:         {
00285:             var json = new SystemTextJsonSerializer();
00286:             var save = new ShelterAssignmentSave
00287:             {
00288:                 simDay = 1,
00289:                 Rooms = new List<ShelterRoomSave>
00290:                 {
00291:                     new ShelterRoomSave { RoomId = "x", DisplayName = "y", Capacity = 1 }
00292:                 },
00293:                 State = new ShelterAssignmentState()
00294:             };
00295:             string text = ShelterAssignmentSaveCodec.EncodeToString(save, json);
00296:             int idx = text.IndexOf("simDay", StringComparison.Ordinal);
00297:             char[] arr = text.ToCharArray();
00298:             arr[idx + 8] = arr[idx + 8] == '1' ? '9' : '1';
00299:             string tampered = new string(arr);
00300:             Assert.Throws<InvalidOperationException>(() => ShelterAssignmentSaveCodec.Decode(tampered, json));
00301:         }
00302:
00303:         [Fact]
00304:         public void Save_EmptyChecksumRejected()
00305:         {
00306:             var json = new SystemTextJsonSerializer();
00307:             var save = new ShelterAssignmentSave { simDay = 1, Checksum = string.Empty };
00308:             string text = json.Serialize(save);
00309:             Assert.Throws<InvalidOperationException>(() => ShelterAssignmentSaveCodec.Decode(text, json));
00310:         }
00311:
00312:         [Fact]
00313:         public void DeDuplicates_SurvivorAssignments_OnNormalize()
00314:         {
00315:             var state = new ShelterAssignmentState
00316:             {
00317:                 Assignments = new List<ShelterAssignment>
00318:                 {
00319:                     new ShelterAssignment { SurvivorId = "elena_vasquez", RoomId = "room_bunks" },
00320:                     new ShelterAssignment { SurvivorId = "elena_vasquez", RoomId = "room_kitchen" }
00321:                 }
00322:             };
00323:             var sys = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00324:             {
00325:                 new ShelterRoom("room_bunks", "Bunks", 4),
00326:                 new ShelterRoom("room_kitchen", "Kitchen", 2)
00327:             }, new SeededRng(1));
00328:             // Last-write-wins: the kitchen assignment should survive.
00329:             Assert.Single(sys.GetAssignments());
00330:             Assert.Equal("room_kitchen", sys.GetAssignmentForSurvivor("elena_vasquez")?.RoomId);
00331:         }
00332:     }
00333: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentProximityTests.cs` — complete current file

- Size: 272 lines / 10836 bytes.
- SHA-256: `f631579fb7a4dbb6372a734563633c1f61efa24ef5b0cb5c2ec9054d54f304f9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Shelter;
00006: using Ashfall.Core.Survivors;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     public class ShelterAssignmentProximityTests
00012:     {
00013:         private static ShelterAssignmentSystem CreateSystem(out List<ShelterRoom> rooms)
00014:         {
00015:             rooms = new List<ShelterRoom>
00016:             {
00017:                 new ShelterRoom("room_bunks", "Bunks", 4),
00018:                 new ShelterRoom("room_kitchen", "Kitchen", 2),
00019:                 new ShelterRoom("room_clinic", "Clinic", 2),
00020:                 new ShelterRoom("room_workshop", "Workshop", 2)
00021:             };
00022:             return new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(42));
00023:         }
00024:
00025:         [Fact]
00026:         public void AreInSameRoom_NullOrEmptyOrSelf_ReturnsFalse()
00027:         {
00028:             var system = CreateSystem(out _);
00029:             system.Assign("survivor_a", "room_bunks");
00030:             system.Assign("survivor_b", "room_bunks");
00031:
00032:             Assert.False(system.AreInSameRoom(null, "survivor_b"));
00033:             Assert.False(system.AreInSameRoom("survivor_a", null));
00034:             Assert.False(system.AreInSameRoom("", "survivor_b"));
00035:             Assert.False(system.AreInSameRoom("survivor_a", ""));
00036:             Assert.False(system.AreInSameRoom(null, null));
00037:             Assert.False(system.AreInSameRoom("survivor_a", "survivor_a")); // Self is not a companion
00038:         }
00039:
00040:         [Fact]
00041:         public void AreInSameRoom_BothAssignedToSameRoom_ReturnsTrue()
00042:         {
00043:             var system = CreateSystem(out _);
00044:             system.Assign("survivor_a", "room_bunks");
00045:             system.Assign("survivor_b", "room_bunks");
00046:
00047:             Assert.True(system.AreInSameRoom("survivor_a", "survivor_b"));
00048:             Assert.True(system.AreInSameRoom("survivor_b", "survivor_a"));
00049:         }
00050:
00051:         [Fact]
00052:         public void AreInSameRoom_DifferentRooms_ReturnsFalse()
00053:         {
00054:             var system = CreateSystem(out _);
00055:             system.Assign("survivor_a", "room_bunks");
00056:             system.Assign("survivor_b", "room_kitchen");
00057:
00058:             Assert.False(system.AreInSameRoom("survivor_a", "survivor_b"));
00059:             Assert.False(system.AreInSameRoom("survivor_b", "survivor_a"));
00060:         }
00061:
00062:         [Fact]
00063:         public void AreInSameRoom_UnassignedSurvivor_ReturnsFalse()
00064:         {
00065:             var system = CreateSystem(out _);
00066:             system.Assign("survivor_a", "room_bunks");
00067:             // survivor_unassigned has no assignment
00068:
00069:             Assert.False(system.AreInSameRoom("survivor_a", "survivor_unassigned"));
00070:             Assert.False(system.AreInSameRoom("survivor_unassigned", "survivor_a"));
00071:             Assert.False(system.AreInSameRoom("survivor_unassigned_1", "survivor_unassigned_2"));
00072:         }
00073:
00074:         [Fact]
00075:         public void AreInSameRoom_InactiveOrDecommissionedAssignment_ReturnsFalse()
00076:         {
00077:             var state = new ShelterAssignmentState
00078:             {
00079:                 Assignments = new List<ShelterAssignment>
00080:                 {
00081:                     new ShelterAssignment
00082:                     {
00083:                         SurvivorId = "survivor_a",
00084:                         RoomId = "room_bunks",
00085:                         Status = ShelterAssignmentStatus.Active
00086:                     },
00087:                     new ShelterAssignment
00088:                     {
00089:                         SurvivorId = "survivor_b",
00090:                         RoomId = "room_bunks",
00091:                         Status = ShelterAssignmentStatus.Decommissioned // Inactive
00092:                     }
00093:                 }
00094:             };
00095:             var rooms = new List<ShelterRoom>
00096:             {
00097:                 new ShelterRoom("room_bunks", "Bunks", 4)
00098:             };
00099:             var system = new ShelterAssignmentSystem(state, rooms, new SeededRng(42));
00100:
00101:             Assert.False(system.AreInSameRoom("survivor_a", "survivor_b"));
00102:         }
00103:
00104:         [Fact]
00105:         public void Flashback_GroundedWhenCompanionInSameRoom()
00106:         {
00107:             var system = CreateSystem(out _);
00108:             system.Assign("survivor_a", "room_bunks");
00109:             system.Assign("survivor_b", "room_bunks");
00110:
00111:             string? groundedBy = null;
00112:             var flashback = new SomaticFlashbackSystem
00113:             {
00114:                 Rng = new SeededRng(10),
00115:                 GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
00116:                 IsCompanionInSameRoom = system.AreInSameRoom
00117:             };
00118:             flashback.OnFlashbackGrounded += (sv, orig, reduced) => groundedBy = sv;
00119:
00120:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00121:             flashback.OnAudioEvent("siren", 10f);
00122:
00123:             Assert.True(flashback.HasActiveFlashback("survivor_a"));
00124:             Assert.Equal("survivor_a", groundedBy);
00125:             Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
00126:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00127:         }
00128:
00129:         [Fact]
00130:         public void Flashback_UngroundedWhenCompanionsInDifferentRooms()
00131:         {
00132:             var system = CreateSystem(out _);
00133:             system.Assign("survivor_a", "room_bunks");
00134:             system.Assign("survivor_b", "room_kitchen"); // Apart
00135:
00136:             bool groundedFired = false;
00137:             var flashback = new SomaticFlashbackSystem
00138:             {
00139:                 Rng = new SeededRng(10),
00140:                 GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
00141:                 IsCompanionInSameRoom = system.AreInSameRoom
00142:             };
00143:             flashback.OnFlashbackGrounded += (sv, orig, reduced) => groundedFired = true;
00144:
00145:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00146:             flashback.OnAudioEvent("siren", 10f);
00147:
00148:             Assert.True(flashback.HasActiveFlashback("survivor_a"));
00149:             Assert.False(groundedFired);
00150:             Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
00151:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00152:         }
00153:
00154:         [Fact]
00155:         public void Flashback_UngroundedWhenCompanionsUnassigned()
00156:         {
00157:             var system = CreateSystem(out _);
00158:             system.Assign("survivor_a", "room_bunks");
00159:             // survivor_b unassigned
00160:
00161:             var flashback = new SomaticFlashbackSystem
00162:             {
00163:                 Rng = new SeededRng(10),
00164:                 GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
00165:                 IsCompanionInSameRoom = system.AreInSameRoom
00166:             };
00167:
00168:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00169:             flashback.OnAudioEvent("siren", 10f);
00170:
00171:             Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
00172:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00173:         }
00174:
00175:         [Fact]
00176:         public void Flashback_UngroundedWhenCompanionIsDead()
00177:         {
00178:             var system = CreateSystem(out _);
00179:             system.Assign("survivor_a", "room_bunks");
00180:             system.Assign("survivor_b", "room_bunks");
00181:
00182:             var flashback = new SomaticFlashbackSystem
00183:             {
00184:                 Rng = new SeededRng(10),
00185:                 // survivor_b is deceased (only survivor_a alive)
00186:                 GetAliveSurvivorIds = () => new[] { "survivor_a" },
00187:                 IsCompanionInSameRoom = system.AreInSameRoom
00188:             };
00189:
00190:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00191:             flashback.OnAudioEvent("siren", 10f);
00192:
00193:             // Cannot be grounded by a dead companion
00194:             Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
00195:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00196:         }
00197:
00198:         [Fact]
00199:         public void Flashback_Reassignment_UpdatesProximityDynamically()
00200:         {
00201:             var system = CreateSystem(out _);
00202:             system.Assign("survivor_a", "room_bunks");
00203:             system.Assign("survivor_b", "room_kitchen"); // Initially apart
00204:
00205:             var flashback = new SomaticFlashbackSystem
00206:             {
00207:                 Rng = new SeededRng(10),
00208:                 GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b" },
00209:                 IsCompanionInSameRoom = system.AreInSameRoom
00210:             };
00211:
00212:             // 1. Apart: ungrounded
00213:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00214:             flashback.OnAudioEvent("siren", 10f);
00215:             Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
00216:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00217:
00218:             // Clear flashback
00219:             flashback.Tick("survivor_a", 24f);
00220:
00221:             // 2. Reassign survivor_b to room_bunks (together)
00222:             system.Unassign("survivor_b");
00223:             system.Assign("survivor_b", "room_bunks");
00224:
00225:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00226:             flashback.OnAudioEvent("siren", 10f);
00227:             Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
00228:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00229:
00230:             // Clear flashback
00231:             flashback.Tick("survivor_a", 24f);
00232:
00233:             // 3. Unassign survivor_b: apart again
00234:             system.Unassign("survivor_b");
00235:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00236:             flashback.OnAudioEvent("siren", 10f);
00237:             Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
00238:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00239:         }
00240:
00241:         [Fact]
00242:         public void Flashback_SaveRestoreRoundtrip_PreservesGroundingBehavior()
00243:         {
00244:             var system = CreateSystem(out var rooms);
00245:             system.Assign("survivor_a", "room_bunks");
00246:             system.Assign("survivor_b", "room_bunks");
00247:             system.Assign("survivor_c", "room_kitchen");
00248:
00249:             var capturedState = system.CaptureState();
00250:
00251:             // Restore into a fresh system instance
00252:             var restoredSystem = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(99));
00253:             restoredSystem.State.RestoreInto(capturedState, rooms);
00254:
00255:             var flashback = new SomaticFlashbackSystem
00256:             {
00257:                 Rng = new SeededRng(10),
00258:                 GetAliveSurvivorIds = () => new[] { "survivor_a", "survivor_b", "survivor_c" },
00259:                 IsCompanionInSameRoom = restoredSystem.AreInSameRoom
00260:             };
00261:
00262:             Assert.True(restoredSystem.AreInSameRoom("survivor_a", "survivor_b"));
00263:             Assert.False(restoredSystem.AreInSameRoom("survivor_a", "survivor_c"));
00264:
00265:             flashback.IncreaseSusceptibility("survivor_a", 1f);
00266:             flashback.OnAudioEvent("siren", 10f);
00267:
00268:             Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
00269:                 flashback.GetWorkEfficiencyPenalty("survivor_a"));
00270:         }
00271:     }
00272: }
```


# Appendix — Focused Current Evidence: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs` — complete current file

- Size: 166 lines / 6444 bytes.
- SHA-256: `5121939d1dbac0de2d70af77bb20e6f8e34687a61d5d0ece18179cb7e4fe12d3`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.IO;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Shelter;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests.Shelter
00010: {
00011:     public class ShelterRoomCatalogTests
00012:     {
00013:         private static string GetDataPath()
00014:         {
00015:             return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "StreamingAssets", "Data");
00016:         }
00017:
00018:         [Fact]
00019:         public void DefaultCatalog_Contains22RoomsAnd12Rules()
00020:         {
00021:             var catalog = ShelterRoomCatalogLoader.GetDefaultCatalog();
00022:             Assert.NotNull(catalog);
00023:             Assert.Equal(1, catalog.schema_version);
00024:             Assert.True(catalog.rooms.Count >= 20, $"Expected at least 20 rooms, got {catalog.rooms.Count}");
00025:             Assert.Equal(12, catalog.assignment_rules.Count);
00026:         }
00027:
00028:         [Fact]
00029:         public void LoadFromFile_ParsesCorrectly()
00030:         {
00031:             string dataDir = GetDataPath();
00032:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00033:             Assert.NotNull(catalog);
00034:             Assert.True(catalog.rooms.Count >= 20);
00035:             Assert.Equal(12, catalog.assignment_rules.Count);
00036:
00037:             var corridor = catalog.rooms.FirstOrDefault(r => r.id == "room_bunker_corridor");
00038:             Assert.NotNull(corridor);
00039:             Assert.Equal(0, corridor.capacity);
00040:             Assert.Equal("Corridor", corridor.function);
00041:
00042:             var kitchen = catalog.rooms.FirstOrDefault(r => r.id == "room_kitchen");
00043:             Assert.NotNull(kitchen);
00044:             Assert.Equal("Kitchen", kitchen.function);
00045:             Assert.Equal(2, kitchen.capacity);
00046:         }
00047:
00048:         [Fact]
00049:         public void AllRoomIds_AreUniqueAndValidPrefix()
00050:         {
00051:             string dataDir = GetDataPath();
00052:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00053:
00054:             var ids = catalog.rooms.Select(r => r.id).ToList();
00055:             var distinctIds = ids.Distinct().ToList();
00056:             Assert.Equal(ids.Count, distinctIds.Count);
00057:
00058:             foreach (var id in ids)
00059:             {
00060:                 Assert.StartsWith("room_", id);
00061:             }
00062:         }
00063:
00064:         [Fact]
00065:         public void AllRuleIds_AreUniqueAndValidPrefix()
00066:         {
00067:             string dataDir = GetDataPath();
00068:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00069:
00070:             var ids = catalog.assignment_rules.Select(r => r.id).ToList();
00071:             var distinctIds = ids.Distinct().ToList();
00072:             Assert.Equal(ids.Count, distinctIds.Count);
00073:
00074:             foreach (var id in ids)
00075:             {
00076:                 Assert.StartsWith("rule_", id);
00077:                 Assert.False(string.IsNullOrWhiteSpace(id));
00078:             }
00079:         }
00080:
00081:         [Fact]
00082:         public void AssignmentRules_TargetValidFunctions()
00083:         {
00084:             string dataDir = GetDataPath();
00085:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00086:
00087:             var roomFunctions = catalog.rooms.Select(r => r.function).Distinct().ToHashSet();
00088:
00089:             foreach (var rule in catalog.assignment_rules)
00090:             {
00091:                 Assert.Contains(rule.target_room_function, roomFunctions);
00092:                 Assert.True(rule.bonus_magnitude > 0);
00093:             }
00094:         }
00095:
00096:         [Fact]
00097:         public void ShelterAssignmentSystem_LoadsFromCatalogRooms()
00098:         {
00099:             string dataDir = GetDataPath();
00100:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00101:
00102:             var rooms = catalog.rooms.Select(r => new ShelterRoom(r.id, r.display_name, r.capacity, r.required_skill_id, r.workstation_id)).ToList();
00103:             var rng = new SeededRng(42);
00104:             var system = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
00105:
00106:             Assert.Equal(catalog.rooms.Count, system.Rooms.Count);
00107:
00108:             // Assign a survivor to the kitchen
00109:             var result = system.Assign("chef_elena", "room_kitchen", day: 1);
00110:             Assert.True(result.Succeeded);
00111:             Assert.Equal(1, system.GetRoomOccupancy("room_kitchen"));
00112:         }
00113:
00114:         [Fact]
00115:         public void DormitoryVariants_ReflectCapacityAndCostTradeoffs()
00116:         {
00117:             string dataDir = GetDataPath();
00118:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00119:
00120:             var crowded = catalog.rooms.First(r => r.id == "room_bunks_crowded");
00121:             var standard = catalog.rooms.First(r => r.id == "room_bunks");
00122:             var privateQ = catalog.rooms.First(r => r.id == "room_quarters_private");
00123:
00124:             Assert.True(crowded.capacity > standard.capacity);
00125:             Assert.True(standard.capacity > privateQ.capacity);
00126:         }
00127:
00128:         [Fact]
00129:         public void WorkshopVariants_TargetDistinctDisciplines()
00130:         {
00131:             string dataDir = GetDataPath();
00132:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00133:
00134:             var general = catalog.rooms.First(r => r.id == "room_workshop");
00135:             var heavy = catalog.rooms.First(r => r.id == "room_workshop_heavy");
00136:             var precision = catalog.rooms.First(r => r.id == "room_workshop_precision");
00137:
00138:             Assert.Contains("repair", general.tags);
00139:             Assert.Contains("heavy_industrial", heavy.tags);
00140:             Assert.Contains("precision", precision.tags);
00141:         }
00142:
00143:         [Fact]
00144:         public void SaveRoundTrip_WithCatalogRooms_PreservesState()
00145:         {
00146:             string dataDir = GetDataPath();
00147:             var catalog = ShelterRoomCatalogLoader.Load(dataDir);
00148:             var rooms = catalog.rooms.Select(r => new ShelterRoom(r.id, r.display_name, r.capacity, r.required_skill_id, r.workstation_id)).ToList();
00149:
00150:             var sys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(100));
00151:             sys.Assign("survivor_1", "room_bunks", day: 2);
00152:             sys.Assign("survivor_2", "room_kitchen", day: 2);
00153:
00154:             var state = sys.CaptureState();
00155:             Assert.Equal(2, state.Assignments.Count);
00156:
00157:             var newSys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, new SeededRng(101));
00158:             newSys.RestoreState(state);
00159:
00160:             Assert.Equal(2, newSys.GetAssignments().Count);
00161:             Assert.True(newSys.AreInSameRoom("survivor_1", "survivor_1") == false);
00162:             Assert.Equal("room_bunks", newSys.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00163:             Assert.Equal("room_kitchen", newSys.GetAssignmentForSurvivor("survivor_2")?.RoomId);
00164:         }
00165:     }
00166: }
```


# Appendix — Focused Current Evidence: `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`

### `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs` — complete current file

- Size: 333 lines / 12526 bytes.
- SHA-256: `64251d808239453605848adcddd7bf184ee0d0ffbcd79f7a0ffaa806c453ad98`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core;
00005: using Ashfall.Core.Shelter;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests.Shelter
00009: {
00010:     public class ShelterAssignmentSystemTests
00011:     {
00012:         private static ShelterAssignmentSystem MakeGrid(ISeededRng rng = null)
00013:         {
00014:             rng ??= new SeededRng(7);
00015:             var rooms = new List<ShelterRoom>
00016:             {
00017:                 new ShelterRoom("room_bunks", "Bunks", 4),
00018:                 new ShelterRoom("room_kitchen", "Kitchen", 2, "skill_cooking"),
00019:                 new ShelterRoom("room_clinic", "Clinic", 2, "skill_medic"),
00020:                 new ShelterRoom("room_workshop", "Workshop", 2, "skill_crafting")
00021:             };
00022:             return new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
00023:         }
00024:
00025:         [Fact]
00026:         public void Constructor_CapturedStateAndRooms_DoNotAliasAuthority()
00027:         {
00028:             var state = new ShelterAssignmentState
00029:             {
00030:                 Assignments = new List<ShelterAssignment>
00031:                 {
00032:                     new ShelterAssignment
00033:                     {
00034:                         SurvivorId = "survivor_1",
00035:                         RoomId = "room_bunks",
00036:                         Status = ShelterAssignmentStatus.Active
00037:                     }
00038:                 }
00039:             };
00040:             var rooms = new List<ShelterRoom>
00041:             {
00042:                 new ShelterRoom("room_bunks", "Bunks", 4)
00043:             };
00044:
00045:             var system = new ShelterAssignmentSystem(state, rooms, new SeededRng(1));
00046:             state.Assignments.Clear();
00047:             rooms[0].Capacity = 0;
00048:
00049:             Assert.Equal(4, system.GetRoomCapacity("room_bunks"));
00050:             Assert.Equal("room_bunks", system.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00051:         }
00052:
00053:         [Fact]
00054:         public void Capture_AssignmentObject_IsSnapshot()
00055:         {
00056:             var system = MakeGrid();
00057:             system.Assign("survivor_1", "room_bunks");
00058:
00059:             var snapshot = system.CaptureState();
00060:             snapshot.Assignments[0].RoomId = "tampered";
00061:
00062:             Assert.Equal("room_bunks", system.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00063:         }
00064:
00065:         [Fact]
00066:         public void Restore_CapturedState_DoesNotAliasInput()
00067:         {
00068:             var source = MakeGrid();
00069:             source.Assign("survivor_1", "room_bunks");
00070:             var saved = source.CaptureState();
00071:             var restored = MakeGrid();
00072:
00073:             restored.RestoreState(saved);
00074:             saved.Assignments[0].RoomId = "tampered";
00075:             saved.Assignments[0].Status = ShelterAssignmentStatus.Decommissioned;
00076:
00077:             Assert.Equal("room_bunks", restored.GetAssignmentForSurvivor("survivor_1")?.RoomId);
00078:             Assert.Equal(ShelterAssignmentStatus.Active, restored.GetAssignmentForSurvivor("survivor_1")?.Status);
00079:         }
00080:
00081:         [Fact]
00082:         public void InactiveAssignments_DoNotConsumeRoomCapacityOrOccupancy()
00083:         {
00084:             var state = new ShelterAssignmentState
00085:             {
00086:                 Assignments = new List<ShelterAssignment>
00087:                 {
00088:                     new ShelterAssignment
00089:                     {
00090:                         SurvivorId = "survivor_1",
00091:                         RoomId = "room_bunks",
00092:                         Status = ShelterAssignmentStatus.OnLeave
00093:                     }
00094:                 }
00095:             };
00096:             var system = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00097:             {
00098:                 new ShelterRoom("room_bunks", "Bunks", 1)
00099:             }, new SeededRng(1));
00100:
00101:             Assert.Equal(0, system.GetRoomOccupancy("room_bunks"));
00102:             Assert.Empty(system.GetAssignmentsForRoom("room_bunks"));
00103:             Assert.True(system.CanAssign("survivor_2", "room_bunks"));
00104:             Assert.True(system.Assign("survivor_2", "room_bunks").Succeeded);
00105:             Assert.Equal(1, system.GetRoomOccupancy("room_bunks"));
00106:         }
00107:
00108:         [Fact]
00109:         public void Assign_ReactivatesInactiveAssignment()
00110:         {
00111:             var state = new ShelterAssignmentState
00112:             {
00113:                 Assignments = new List<ShelterAssignment>
00114:                 {
00115:                     new ShelterAssignment
00116:                     {
00117:                         SurvivorId = "survivor_1",
00118:                         RoomId = "room_bunks",
00119:                         Status = ShelterAssignmentStatus.Decommissioned
00120:                     }
00121:                 }
00122:             };
00123:             var system = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00124:             {
00125:                 new ShelterRoom("room_bunks", "Bunks", 1),
00126:                 new ShelterRoom("room_clinic", "Clinic", 1)
00127:             }, new SeededRng(1));
00128:
00129:             var result = system.Assign("survivor_1", "room_clinic", day: 9);
00130:
00131:             Assert.True(result.Succeeded);
00132:             Assert.Single(system.GetAssignments());
00133:             Assert.Equal("room_clinic", result.Assignment?.RoomId);
00134:             Assert.Equal(ShelterAssignmentStatus.Active, result.Assignment?.Status);
00135:         }
00136:
00137:         [Fact]
00138:         public void Constructor_NullAssignmentList_FailsClosed()
00139:         {
00140:             var state = new ShelterAssignmentState { Assignments = null! };
00141:
00142:             var system = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00143:             {
00144:                 new ShelterRoom("room_bunks", "Bunks", 1)
00145:             }, new SeededRng(1));
00146:
00147:             Assert.Empty(system.GetAssignments());
00148:             Assert.True(system.Assign("survivor_1", "room_bunks").Succeeded);
00149:         }
00150:
00151:         [Fact]
00152:         public void Assign_AddsAssignment()
00153:         {
00154:             var sys = MakeGrid();
00155:             var result = sys.Assign("elena_vasquez", "room_bunks", day: 5);
00156:             Assert.True(result.Succeeded);
00157:             Assert.NotNull(result.Assignment);
00158:             Assert.Single(sys.GetAssignments());
00159:         }
00160:
00161:         [Fact]
00162:         public void Assign_UnknownRoomFails()
00163:         {
00164:             var sys = MakeGrid();
00165:             var result = sys.Assign("elena_vasquez", "room_does_not_exist");
00166:             Assert.False(result.Succeeded);
00167:             Assert.Equal("unknown_room", result.ReasonCode);
00168:         }
00169:
00170:         [Fact]
00171:         public void Assign_AlreadyAssignedFails()
00172:         {
00173:             var sys = MakeGrid();
00174:             sys.Assign("elena_vasquez", "room_bunks");
00175:             var result = sys.Assign("elena_vasquez", "room_kitchen");
00176:             Assert.False(result.Succeeded);
00177:             Assert.Equal("already_assigned", result.ReasonCode);
00178:         }
00179:
00180:         [Fact]
00181:         public void Assign_RoomFullFails()
00182:         {
00183:             var sys = MakeGrid();
00184:             sys.Assign("s1", "room_bunks");
00185:             sys.Assign("s2", "room_bunks");
00186:             sys.Assign("s3", "room_bunks");
00187:             sys.Assign("s4", "room_bunks");
00188:             var result = sys.Assign("s5", "room_bunks");
00189:             Assert.False(result.Succeeded);
00190:             Assert.Equal("room_full", result.ReasonCode);
00191:         }
00192:
00193:         [Fact]
00194:         public void CanAssign_FalseWhenFull()
00195:         {
00196:             var sys = MakeGrid();
00197:             sys.Assign("s1", "room_bunks");
00198:             sys.Assign("s2", "room_bunks");
00199:             Assert.True(sys.CanAssign("s3", "room_bunks"));
00200:             sys.Assign("s3", "room_bunks");
00201:             sys.Assign("s4", "room_bunks");
00202:             Assert.False(sys.CanAssign("s5", "room_bunks"));
00203:         }
00204:
00205:         [Fact]
00206:         public void Unassign_RemovesAssignment()
00207:         {
00208:             var sys = MakeGrid();
00209:             sys.Assign("elena_vasquez", "room_bunks");
00210:             var result = sys.Unassign("elena_vasquez", day: 6);
00211:             Assert.True(result.Succeeded);
00212:             Assert.Empty(sys.GetAssignments());
00213:         }
00214:
00215:         [Fact]
00216:         public void Unassign_NotAssignedFails()
00217:         {
00218:             var sys = MakeGrid();
00219:             var result = sys.Unassign("ghost");
00220:             Assert.False(result.Succeeded);
00221:             Assert.Equal("not_assigned", result.ReasonCode);
00222:         }
00223:
00224:         [Fact]
00225:         public void GetOccupancy_AfterMultipleAssigns()
00226:         {
00227:             var sys = MakeGrid();
00228:             sys.Assign("s1", "room_bunks");
00229:             sys.Assign("s2", "room_bunks");
00230:             Assert.Equal(2, sys.GetRoomOccupancy("room_bunks"));
00231:             Assert.Equal(0, sys.GetRoomOccupancy("room_kitchen"));
00232:         }
00233:
00234:         [Fact]
00235:         public void Events_FireOnAssignAndUnassign()
00236:         {
00237:             var sys = MakeGrid();
00238:             var fired = new List<ShelterAssignmentEvent>();
00239:             sys.OnAssignmentChanged += e => fired.Add(e);
00240:             sys.Assign("elena_vasquez", "room_bunks", day: 3);
00241:             sys.Unassign("elena_vasquez", day: 4);
00242:             Assert.Equal(2, fired.Count);
00243:             Assert.Equal(ShelterAssignmentEventKind.Assigned, fired[0].Kind);
00244:             Assert.Equal(ShelterAssignmentEventKind.Unassigned, fired[1].Kind);
00245:         }
00246:
00247:         [Fact]
00248:         public void CaptureRestore_RoundTrip()
00249:         {
00250:             var sys = MakeGrid();
00251:             sys.Assign("elena_vasquez", "room_bunks");
00252:             sys.Assign("marcus_olejnik", "room_clinic");
00253:             var save = sys.CaptureState();
00254:             var fresh = MakeGrid();
00255:             fresh.RestoreState(save);
00256:             Assert.Equal(2, fresh.GetAssignments().Count);
00257:             Assert.NotNull(fresh.GetAssignmentForSurvivor("elena_vasquez"));
00258:             Assert.NotNull(fresh.GetAssignmentForSurvivor("marcus_olejnik"));
00259:         }
00260:
00261:         [Fact]
00262:         public void Save_RoundTrip_ChecksumStable()
00263:         {
00264:             var sys = MakeGrid();
00265:             sys.Assign("elena_vasquez", "room_bunks");
00266:             var save = new ShelterAssignmentSave
00267:             {
00268:                 simDay = 1,
00269:                 Rooms = new List<ShelterRoomSave>
00270:                 {
00271:                     new ShelterRoomSave { RoomId = "room_bunks", DisplayName = "Bunks", Capacity = 4 }
00272:                 },
00273:                 State = sys.CaptureState()
00274:             };
00275:             var json = new SystemTextJsonSerializer();
00276:             string text = ShelterAssignmentSaveCodec.EncodeToString(save, json);
00277:             var loaded = ShelterAssignmentSaveCodec.Decode(text, json);
00278:             Assert.Equal(save.Checksum, loaded.Checksum);
00279:             Assert.Single(loaded.State.Assignments);
00280:         }
00281:
00282:         [Fact]
00283:         public void Save_TamperedChecksumRejected()
00284:         {
00285:             var json = new SystemTextJsonSerializer();
00286:             var save = new ShelterAssignmentSave
00287:             {
00288:                 simDay = 1,
00289:                 Rooms = new List<ShelterRoomSave>
00290:                 {
00291:                     new ShelterRoomSave { RoomId = "x", DisplayName = "y", Capacity = 1 }
00292:                 },
00293:                 State = new ShelterAssignmentState()
00294:             };
00295:             string text = ShelterAssignmentSaveCodec.EncodeToString(save, json);
00296:             int idx = text.IndexOf("simDay", StringComparison.Ordinal);
00297:             char[] arr = text.ToCharArray();
00298:             arr[idx + 8] = arr[idx + 8] == '1' ? '9' : '1';
00299:             string tampered = new string(arr);
00300:             Assert.Throws<InvalidOperationException>(() => ShelterAssignmentSaveCodec.Decode(tampered, json));
00301:         }
00302:
00303:         [Fact]
00304:         public void Save_EmptyChecksumRejected()
00305:         {
00306:             var json = new SystemTextJsonSerializer();
00307:             var save = new ShelterAssignmentSave { simDay = 1, Checksum = string.Empty };
00308:             string text = json.Serialize(save);
00309:             Assert.Throws<InvalidOperationException>(() => ShelterAssignmentSaveCodec.Decode(text, json));
00310:         }
00311:
00312:         [Fact]
00313:         public void DeDuplicates_SurvivorAssignments_OnNormalize()
00314:         {
00315:             var state = new ShelterAssignmentState
00316:             {
00317:                 Assignments = new List<ShelterAssignment>
00318:                 {
00319:                     new ShelterAssignment { SurvivorId = "elena_vasquez", RoomId = "room_bunks" },
00320:                     new ShelterAssignment { SurvivorId = "elena_vasquez", RoomId = "room_kitchen" }
00321:                 }
00322:             };
00323:             var sys = new ShelterAssignmentSystem(state, new List<ShelterRoom>
00324:             {
00325:                 new ShelterRoom("room_bunks", "Bunks", 4),
00326:                 new ShelterRoom("room_kitchen", "Kitchen", 2)
00327:             }, new SeededRng(1));
00328:             // Last-write-wins: the kitchen assignment should survive.
00329:             Assert.Single(sys.GetAssignments());
00330:             Assert.Equal("room_kitchen", sys.GetAssignmentForSurvivor("elena_vasquez")?.RoomId);
00331:         }
00332:     }
00333: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 41: Shelter Room Catalog, Assignment Rules and Capacity Semantics.**.

The first polish also checks continuity against the master authority: the plan is one bounded outcome, uses an existing owner seam, names a data authority, and does not widen into unrelated economy, UI, save or content work. Any apparently attractive addition that lacks a current owner is recorded as out of scope rather than smuggled into the architecture.

# Appendix — Polishing Pass 2: Integration and Code Architecture

This pass turns the evidence into an executable route. It distinguishes Core domain rules, host composition, Godot presentation, current save envelopes, deterministic streams, typed events and focused tests. The route is deliberately extend-first. A future builder may add a field, catalog row, read model or host adapter only after claiming the exact path and proving that the existing owner can accept it.

The second pass also reviews the handoff from data to player experience. A row that cannot be reached from a command is an orphan; a command that updates a shadow field is a split authority; a panel that recomputes a result is a presentation bug; a save that restores a display but not the owner is a persistence bug. Each failure is given a focused negative obligation.

# Appendix — Final Precision and Reaccuracy Pass

Before handoff, re-read every current path, hash, catalog count, public declaration, test declaration and save owner named above. Correct stale terminology, remove fictional type names, replace old section pins with current owner names, and downgrade any unsupported pass claim to historical evidence. Re-run the structural verifier after this pass. The final artifact should let a builder execute the first safe step without reinterpreting ownership.

**Precision result:** current implementation claims are separated from future proposals; content is not counted as reachability; Core remains engine-free; UI remains a projection; save and determinism are explicit; and every residual gap has a named verification route. If a future source audit contradicts this record, the source wins and the plan returns `STALE_PLAN` for re-audit.

# Appendix — Quality Assurance Pass Record

This record is part of the planning artifact, not a fresh runtime test result.

## Pass A — premise and content
- Current catalog rows, source owners, historical closeout and remaining residual are separated.
- The old baseline count is not presented as the current count.
- No copied, real-world, fabricated or unowned content is proposed.

## Pass B — integration architecture
- Data → loader → Core owner → host command → UI/event → save → replay is named.
- Existing save sections and codecs are identified; no parallel section is invented.
- Deterministic ordering, no-RNG cases, seeded streams and legacy defaults are explicit.

## Pass C — precision and handoff
- Every referenced current path is hash-pinned in the evidence appendices.
- Proposed future seams are labeled as proposals and excluded from current claims.
- Focused test commands, failure responses, rollback and non-goals are included.
