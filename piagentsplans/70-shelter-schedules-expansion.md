# Plan 70 — Shelter Schedules, Circadian Phase and Power-Aware Duty Rhythm

> **Rebuild status:** COMPLETE 12-SCHEDULE CATALOG/HOST LOOP — PHASE, HOUR AND POWER REACHABILITY AUDIT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-3`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round3-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified architecture and current evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The current 12 rows cover standard, emergency, siege, mourning, winter, expedition, scout and other authored rhythms. The source explicitly applies the active schedule across phases, with emergency override and brownout effects.
- The live route is catalog → `ShelterScheduleCatalogLoader` → `ShelterScheduleSystem` → `ShelterScheduleHostSession`/`Main.ShelterInfrastructure` → `ShelterSchedulePanel` → `shelter_schedule` save section and daily/hourly ticks.
- The useful leap forward is proving the hour transition, duty assignment, power demand, emergency override, brownout and restore contracts as one truthful loop, while leaving PowerGrid, Needs and DutyRoster as their existing owners.

**Bounded outcome:** Retire the old 3→12 pure-data brief as a new authoring project. Current `shelter_schedules.json` has 12 rows, the Core system owns phases/curfew/assignments/fatigue/lighting, the host loads and saves it, and the shelter panel projects it. The remaining plan is a phase/hour/power semantics audit, not a second schedule manager or arbitrary schedule growth.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `shelter_schedules.json` is valid JSON with 12 schedule rows and authored day/night/curfew windows, fatigue modifiers, lighting demand, shift pattern and trigger conditions.
- `ShelterScheduleSystem` owns current phase, curfew/emergency state, assignments, recovery modifier and lighting demand; `TickDay` and `TickHour` are the current transition seams.
- `ShelterScheduleHostSession` loads the catalog, delegates commands, ticks day/hour and saves through `ShelterScheduleSaveStore`; `Main.ShelterInfrastructure` constructs and binds the current host/panel.
- Historical DEC-233 tests cover the 12-row catalog and schedule/season integration; current pass status still requires a focused rerun.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C1 Shelter operations cluster: schedule phase, power demand, assignments and rest are separate concerns with one current schedule owner.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 3→12 target with a 12-row current schedule census and trigger/shift-pattern matrix.
- Verify hour wrapping, day/night/curfew/emergency precedence and brownout lighting math against the current source.
- Trace assignments, fatigue recovery and lighting demand into their existing Needs/DutyRoster/PowerGrid consumers without copying their state.
- Audit the current panel’s schedule selection, refusal, close/back, controller and refresh behavior.

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
| authored schedule definitions | ShelterScheduleCatalogLoader | `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs` | Loads the JSON container; it does not decide when a schedule is active. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | Sole schedule authority; accepts PowerGrid as a read-only brownout dependency. |
| brownout/power state | PowerGridSystem | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | Owns grid state; schedule consumes it rather than duplicating it. |
| load/tick/command/save composition | ShelterScheduleHostSession | `src/Host/ShelterScheduleHostSession.cs; src/Host/ShelterScheduleSaveStore.cs` | Thin host boundary. |
| player projection | ShelterSchedulePanel | `src/UI/ShelterSchedulePanel.cs` | Read-only presentation and command binding. |
| catalog, phase, hour and save proof | Schedule focused tests | `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs; Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs; Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Shelter Schedules, Circadian Phase and Power-Aware Duty Rhythm
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ShelterScheduleCatalogLoader
│   authored schedule definitions
│ ShelterScheduleSystem
│   phase, curfew, assignments, modifiers and lighting state
│ PowerGridSystem
│   brownout/power state
│ ShelterScheduleHostSession
│   load/tick/command/save composition
│ ShelterSchedulePanel
│   player projection
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

1. **Preserve current state ownership.** ShelterScheduleCatalogLoader owns authored schedule definitions: Loads the JSON container; it does not decide when a schedule is active.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| authored schedule definitions | ShelterScheduleCatalogLoader | `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs` | Loads the JSON container; it does not decide when a schedule is active. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | Sole schedule authority; accepts PowerGrid as a read-only brownout dependency. |
| brownout/power state | PowerGridSystem | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | Owns grid state; schedule consumes it rather than duplicating it. |
| load/tick/command/save composition | ShelterScheduleHostSession | `src/Host/ShelterScheduleHostSession.cs; src/Host/ShelterScheduleSaveStore.cs` | Thin host boundary. |
| player projection | ShelterSchedulePanel | `src/UI/ShelterSchedulePanel.cs` | Read-only presentation and command binding. |
| catalog, phase, hour and save proof | Schedule focused tests | `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs; Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs; Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load schedule definitions
2. activate a schedule through the current host command
3. feed campaign day/hour into the owner
4. derive phase and curfew/emergency state
5. project lighting/fatigue values to existing consumers
6. assign/unassign beds through the owner
7. capture/restore shelter schedule state and refresh the panel

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Schedule definitions are immutable data; active schedule, phase, curfew/emergency flags, assignments and derived modifiers are owner state.
- Hour transitions are explicit and legacy day-only behavior remains supported when hour is unknown.
- Emergency override is rejected when the active definition disallows it; brownout modifies the published lighting demand after base demand is selected.
- Restore preserves active schedule, phase, assignments and last transition day without silently resetting a campaign.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Unknown schedule IDs return a named failure and leave active state unchanged.
- Emergency override cannot bypass a schedule’s explicit prohibition.
- Brownout cannot double-apply or erase the base lighting demand.
- The same day/hour/grid state produces the same phase, demand and recovery values.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `shelter_schedules.json` is the sole schedule definition authority.
- Do not duplicate shift windows into DutyRoster or PowerGrid catalogs.
- A new row requires a real trigger, bounded hours, a shift pattern and a current consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use `shelter_schedule` and its current store; no new save section.
- Capture/restore includes active ID, phase, curfew/emergency, assignments, modifiers and transition day.
- Legacy missing fields use the current documented neutral defaults.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Schedule lookup uses an ordinal dictionary and fixed day/hour arithmetic; no random selection.
- The host calls hour/day ticks in campaign order, not once per rendered frame.
- Replay compares active ID, phase, assignments, lighting demand and recovery modifier.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Schedule phase and schedule changes emit current owner events.
- Power brownout is an input fact from PowerGrid, not a new schedule authority.
- Panel commands return ActionResult and refresh the current projection.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ShelterScheduleHostSession.cs
- src/Host/ShelterScheduleSaveStore.cs
- src/Main.ShelterInfrastructure.cs
- src/UI/ShelterSchedulePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Schedule descriptions communicate shelter rhythm and pressure without claiming an event the system did not detect.
- A schedule is an operational contract, not a cosmetic wallpaper layer.
- Do not use real-world emergency or labor instructions as mechanics.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A schedule row is displayed but not registered. | ShelterScheduleCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Curfew/emergency precedence differs between Core and panel. | ShelterScheduleSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Brownout demand is applied twice or not at all. | PowerGridSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Assignments are lost on restore. | ShelterScheduleHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A panel changes phase without calling the owner. | ShelterSchedulePanel | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — schedule census | Read 12 rows, loader, system, host, panel and save. | Current schema and owner are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — phase/power proof | Exercise hour wrap, curfew, emergency and brownout boundaries. | One deterministic phase/demand result per input. | No production path until the owning implementation package is separately claimed. |
| 2 — assignments/consumer trace | Trace bed assignment, fatigue and lighting to existing owners. | No duplicate state or hidden consumer. | No production path until the owning implementation package is separately claimed. |
| 3 — accessibility/precision pass | Review panel truthfulness and current save restore. | Residual work is bounded and named. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/shelter_schedules.json | READ ONLY; MODIFY only for a proven trigger/consumer gap | 12-row authority |
| Assets/Ashfall.Core/ShelterScheduleSystem.cs | READ ONLY | Schedule owner |
| src/Host/ShelterScheduleHostSession.cs | READ ONLY | Host seam |
| src/UI/ShelterSchedulePanel.cs | READ ONLY; MODIFY only under a new UI claim | Projection |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second shift/calendar authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing hour semantics while fixing a panel. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating PowerGrid brownout as schedule-owned state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Assuming a trigger string is automatically evaluated by a consumer. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new schedule system.
- No new save section.
- No arbitrary catalog growth.
- No production/data/test/UI changes in this planning package.

# 23. Rollback and Recovery

- Revert the planning document.
- Future Core/host changes retain the current schedule save fixture and focused phase tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 12 current rows and their trigger/power fields are documented.
- Core/host/UI and PowerGrid/Needs/DutyRoster boundaries are explicit.
- Hour, emergency, brownout, assignment and restore obligations are named.
- Focused commands are supplied without fresh-pass claims.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 3→12 target with a 12-row current schedule census and trigger/shift-pattern matrix.
- Verify hour wrapping, day/night/curfew/emergency precedence and brownout lighting math against the current source.
- Trace assignments, fatigue recovery and lighting demand into their existing Needs/DutyRoster/PowerGrid consumers without copying their state.
- Audit the current panel’s schedule selection, refusal, close/back, controller and refresh behavior.

## MUST NOT DO

- No new schedule system.
- No new save section.
- No arbitrary catalog growth.
- No production/data/test/UI changes in this planning package.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — schedule census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: authored schedule definitions → ShelterScheduleCatalogLoader; phase, curfew, assignments, modifiers and lighting state → ShelterScheduleSystem; brownout/power state → PowerGridSystem; load/tick/command/save composition → ShelterScheduleHostSession; player projection → ShelterSchedulePanel; catalog, phase, hour and save proof → Schedule focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 70.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 70 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ShelterScheduleCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/ShelterScheduleSystem.cs`

### `Assets/Ashfall.Core/ShelterScheduleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 376 lines / 14678 bytes.
- SHA-256: `637d5c2359fc965f94a573826f54c346e9b0bdc10d0edd3bd0ed8f64374c7dc0`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterScheduleState
public string systemId = ShelterScheduleSystem.SystemId;
public SchedulePhase currentPhase = SchedulePhase.Day;
public bool curfewActive;
public bool emergencyOverride;
public float fatigueRecoveryModifier = 1f;
public float lightingDemand = 0.5f;
public List<SleepAssignment> assignments = new List<SleepAssignment>();
public int lastTransitionDay = -1;
public string activeScheduleId = "default";
public sealed class ScheduleDefinition
public string schedule_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public float dayStartHour { get; set; } = 6f;
public float dayEndHour { get; set; } = 22f;
public float curfewStartHour { get; set; } = 22f;
public float curfewEndHour { get; set; } = 6f;
public float fatigueRecoveryModifier { get; set; } = 1f;
public float lightingDemandDay { get; set; } = 0.5f;
public float lightingDemandNight { get; set; } = 0.8f;
public float lightingDemandCurfew { get; set; } = 0.3f;
public bool allowEmergencyOverride { get; set; } = true;
public string shiftPattern { get; set; } = "single_shift";
public string triggerCondition { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class SleepAssignment
public string survivorId = string.Empty;
public string bedId = string.Empty;
public bool isAssigned;
public bool isCompliant;
public float restQuality = 1f;
public enum SchedulePhase { Day, Night, Curfew, Emergency } public sealed class ShelterScheduleSystem { public const string SystemId = "shelter_schedule"; private ShelterScheduleState _state = new ShelterScheduleState(); /// <summary>-1 = hour unknown (legacy day-only behaviour).</summary> private int _hourOfDay = -1; private readonly Dictionary<string, ScheduleDefinition> _catalog = new Dictionary<string, ScheduleDefinition>(StringComparer.Ordinal); private readonly ILog _log; private readonly PowerGridSystem _powerGrid; private string _activeScheduleId = "default"; public ShelterScheduleState State => _state; public SchedulePhase CurrentPhase => _state.currentPhase; public bool IsCurfewActive => _state.curfewActive && !_state.emergencyOverride; public bool IsEmergencyOverride => _state.emergencyOverride; public float FatigueRecoveryModifier => _state.fatigueRecoveryModifier; public float LightingDemand => _state.lightingDemand; public string ActiveScheduleId => _activeScheduleId; public IReadOnlyCollection<ScheduleDefinition> GetAllSchedules() => _catalog.Values; public ScheduleDefinition? GetSchedule(string scheduleId) => _catalog.TryGetValue(scheduleId, out var def) ? def : null; public bool TryActivateScheduleByTrigger(string triggerCondition) { if (string.IsNullOrEmpty(triggerCondition)) return false; foreach (var kvp in _catalog) { if (string.Equals(kvp.Value.triggerCondition, triggerCondition, StringComparison.OrdinalIgnoreCase)) { var res = SetSchedule(kvp.Key); return res.IsSuccess; }
public event Action<SchedulePhase> OnPhaseChanged;
public event Action OnScheduleChanged;
public void LoadCatalog(List<ScheduleDefinition> definitions) {
public ActionResult SetSchedule(string scheduleId) {
public ActionResult SetCurfew(bool active) {
public ActionResult SetEmergencyOverride(bool active) {
public ActionResult AssignBed(string survivorId, string bedId) {
public ActionResult UnassignBed(string survivorId) {
public void TickDay(int day) {
public ScheduleDefinition? GetActiveSchedule() {
public bool IsSleepEligible(string survivorId) {
public SchedulePhase PhaseForHour(int hourOfDay) {
public void TickHour(int hourOfDay) {
public ShelterScheduleState CaptureState() {
public void RestoreState(ShelterScheduleState saved) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs`

### `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 51 lines / 1775 bytes.
- SHA-256: `bdd89c824da3d489dce770b3b745df46ae5dcbc503ce1489726c05acfde9e63f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterScheduleCatalogContainer
public List<ScheduleDefinition> schedules = new List<ScheduleDefinition>();
public static class ShelterScheduleCatalogLoader
public const string DefaultFileName = "shelter_schedules.json";
public static List<ScheduleDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( ShelterScheduleSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`

### `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1415 lines / 62138 bytes.
- SHA-256: `01d76096907f04a268dd10572a9d6635286d47742922420c81e6c7863ae39f09`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=20; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PowerGridSystem
public event Action<PowerGridEvent>? OnPowerChanged;
public event Action<PowerGridTickSummary>? OnTickSummary;
public PowerGridState State => _state;
public IReadOnlyList<PowerGridRoom> Rooms => _rooms;
public float BaseGenerationWatts => _state.GenerationWatts;
public IReadOnlyDictionary<string, float> GenerationContributions => _generationContributions;
public float FuelUnits => _state.FuelUnits;
public float BatteryReserveWh => _state.BatteryReserveWh;
public float BatteryCapacityWh => _state.BatteryCapacityWh;
public float TotalDrawWatts => ComputeTotalDraw();
public float NetWatts => GenerationWatts - TotalDrawWatts;
public bool IsBrownout => TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0;
public float SustainableBatteryDischargeWatts => Math.Max(0f, _state.BatteryReserveWh / 24f);
public float AvailableSupplyWatts => GenerationWatts + SustainableBatteryDischargeWatts;
public float DeficitWatts => Math.Max(0f, TotalDrawWatts - AvailableSupplyWatts);
public bool SetGenerationContribution(string sourceId, float watts) {
public bool RemoveGenerationContribution(string sourceId) {
public const string EbPvdInstalledSourceId = "ebpvd_installed";
public const int MaxInstalledCoatedParts = 3;
public const float MaxEbPvdInstalledWatts = 80f;
public const string BatteryBankItemId = "item_battery_reconditioned";
public const float BatteryBankCapacityWh = 1000f;
public const int MaxInstalledBatteryBanks = 4;
public const string GeneratorMaintenanceItemId = "machine_oil";
public const float GeneratorWearPerDay = 0.25f;
public const float GeneratorDegradationThreshold = 50f;
public const float GeneratorMinOutputFactor = 0.5f;
public float GeneratorCondition => _state.GeneratorCondition;
public float GeneratorOutputFactor => _state.GeneratorCondition >= GeneratorDegradationThreshold
public bool PerformGeneratorMaintenance(out string reason) {
public bool TryInstallBatteryBank(out string reason) {
public int InstalledBatteryBankCount => _state.InstalledBatteryBankCount;
public IReadOnlyList<string> InstalledCoatedPartItemIds => _state.InstalledCoatedPartItemIds;
public bool TryInstallCoatedPart(string itemId, out string reason) {
public bool TryUninstallCoatedPart(string itemId, out string reason) {
public void RepublishEbPvdInstalledContribution() {
public static string ResolveCoatedPartFamily(string itemId) {
public static float ResolveCoatedPartWatts(string itemId) {
public PowerGridSnapshot Snapshot() {
public bool IsRoomPowered(string roomId) {
public PowerGridRoomPriority EffectivePriority(string roomId) {
public bool ToggleBreaker(string roomId) {
public bool SetBreaker(string roomId, bool closed) {
public void MarkTripped(string roomId, int day) {
public void ClearTripped(string roomId) => _state.ClearTripped(roomId);
public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);
public bool SetPriority(string roomId, PowerGridRoomPriority priority) {
public bool RegisterLoadRoom(PowerGridRoom room) {
public IReadOnlyList<string> ApplyBrownoutShedPreset() {
public int ApplyCatalogDefaultPriorities() {
public void AddFuel(float units) {
public const float DefaultEmpStormSurgeSeverity = 0.6f;
public const float DefaultSurgeBatteryDrainFraction = 0.15f;
public void ConfigureSurge(float empStormSeverity, float batteryDrainFraction) {
public float EmpStormSeverity => _empStormSurgeSeverity;
public float SurgeBatteryDrain => _surgeBatteryDrainFraction;
public IReadOnlyList<string> ApplySurgeDay(int day, float severity01) {
public PowerGridTickSummary TickDay(int day, ISeededRng tickRng) {
internal const float AllocationEpsilon = 0.01f;
public bool IsRoomServed(string roomId) {
public float ServedWatts;
public bool HasCriticalDeficit;
public List<string> ServedRoomIds = new List<string>();
public List<string> ShedRoomIds = new List<string>();
public PowerGridState CaptureState() => _state.Capture();
public void RestoreState(PowerGridState state) {
public float GetRoomDrawWatts(string roomId) {
public float EffectiveTotalDrawWatts =>
public enum PowerGridRoomPriority
public sealed class PowerGridRoom
public string RoomId;
public string DisplayName;
public float DrawWatts;
public PowerGridRoomPriority DefaultPriority;
public string FailureEffectId; // semantic id the host looks up.
public sealed class PowerGridState
public int SimDay;
public float GenerationWatts;
public float FuelUnits;
public float BatteryReserveWh;
public float BatteryCapacityWh;
public List<string> ClosedBreakers = new List<string>();
public List<string> TrippedRooms = new List<string>();
public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();
public int LastSurgeDay;
public List<string> InstalledCoatedPartItemIds = new List<string>();
public int InstalledBatteryBankCount;
public float GeneratorCondition = 100f;
public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);
public void SetBreaker(string roomId, bool closed) {
public void MarkTripped(string roomId, int day) {
public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);
public PowerGridRoomPriority GetRoomPriority(string roomId) {
public void SetRoomPriority(string roomId, PowerGridRoomPriority priority) {
public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms) {
public PowerGridState Capture() {
public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms) {
internal static float SanitizeNonNegativeFinite(float value) =>
public sealed class RoomPriorityRecord
public string RoomId;
public PowerGridRoomPriority Priority;
public sealed class PowerGridEvent
public PowerGridEventKind Kind;
public string RoomId;
public int Day;
public string Detail;
public float Numeric;
public enum PowerGridEventKind
public sealed class PowerGridTickSummary
public int Day;
public float FuelConsumed;
public float BatteryEndWh;
public float BrownoutHours;
public bool IsBrownout;
public float GenerationWatts;
public float RequestedDrawWatts;
public float ServedWatts;
public float UnservedWatts;
public bool HasCriticalDeficit;
public bool BrownoutBegan;
public bool BrownoutEnded;
public List<string> ServedRoomIds = new List<string>();
public List<string> ShedRoomIds = new List<string>();
public sealed class PowerGridSnapshot
public int Day;
public float GenerationWatts;
public float FuelUnits;
public float BatteryReserveWh;
public float BatteryCapacityWh;
public float TotalDrawWatts;
public float NetWatts;
public bool IsBrownout;
public List<string> RoomIds = new List<string>();
public Dictionary<string, float> GenerationContributions = new Dictionary<string, float>(StringComparer.Ordinal);
```


# Appendix B.05 — Current Code Architecture: `src/Host/ShelterScheduleHostSession.cs`

### `src/Host/ShelterScheduleHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 111 lines / 3763 bytes.
- SHA-256: `ea5a50b5e4328d1907b3407e7dfbb235762887d5db58fc3d3e1075db96f53e2a`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterScheduleHostSession
public ShelterScheduleSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult SetCurfew(bool active) {
public ActionResult SetEmergencyOverride(bool active) {
public ActionResult AssignBed(string survivorId, string bedId) {
public void LoadCatalog(string dataDir) {
public void TickDay(int day) {
public void TickHour(int hourOfDay) {
public override void Save() {
```


# Appendix B.06 — Current Code Architecture: `src/Host/ShelterScheduleSaveStore.cs`

### `src/Host/ShelterScheduleSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2760 bytes.
- SHA-256: `4d4d9e6a62dc0d98c338bff509d0366841635d2cbac52eabbdaca78bd0f4d6aa`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ShelterScheduleSaveStore
public const string FileName = "shelter_schedule_save.json";
public const string SectionName = "shelter_schedule";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(ShelterScheduleState state) => s_store.CaptureBare(state);
public static ShelterScheduleState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ShelterScheduleState state) => s_store.CaptureBare(state);
public static ShelterScheduleState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(ShelterScheduleState state) => s_store.TrySave(state);
public static ShelterScheduleState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ShelterScheduleState state) => s_store.CapturePersisted(state);
```


# Appendix B.07 — Current Code Architecture: `src/Main.ShelterInfrastructure.cs`

### `src/Main.ShelterInfrastructure.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 645 lines / 31304 bytes.
- SHA-256: `5ec59e6a0a93e8e5c67bc7a39c91ff4cca74a518400fd09468938167c23630ea`.
- Architecture signals: seeded references=6; save/restore symbols=16; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Shelter.ShelterFireHazardSystem ShelterFireHazard => GetShelterFireHazardSystem();
public ShelterFireHostSession? ShelterFireSession => _shelterFireSession;
public Ashfall.Core.Narrative.BunkerGraffitiCatalog GetBunkerGraffitiCatalog() {
public Ashfall.Core.Narrative.BunkerCourtCatalog GetBunkerCourtCatalog() {
public Ashfall.Core.Narrative.BunkerMaintenanceCatalog GetBunkerMaintenanceCatalog() {
public Ashfall.Core.Narrative.PersonalLetterCatalog GetPersonalLetterCatalog() {
public Ashfall.Core.Narrative.AbyssalAnomaliesCatalog GetAbyssalAnomaliesCatalog() {
public string BuildMachineTellText(ISeededRng? rng = null) {
public Ashfall.Core.Shelter.ShelterFireHazardSystem GetShelterFireHazardSystem() {
```


# Appendix B.08 — Current Code Architecture: `src/UI/ShelterSchedulePanel.cs`

### `src/UI/ShelterSchedulePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 126 lines / 4447 bytes.
- SHA-256: `53f4022b5ec4be0fa4d17af5476684c27fee47b456bc0c1e8340be2d359f8fcc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ShelterSchedulePanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(ShelterScheduleHostSession session) {
public void Unbind() {
public override void _Ready() {
public void RefreshView() {
public override void _ExitTree() {
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/shelter_schedules.json`

### `Assets/StreamingAssets/Data/shelter_schedules.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 7638 bytes / 7638 characters.
- SHA-256: `521173a5cf43ddc5d00bad85892f7733a1880b45560a9568c8c44ed0ee687f5c`.
- Root keys: `collection_id`, `schedules`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
schedules: min=12, max=12, observed_paths=1
```

Representative record fields:

- `allow_emergency_override`
- `curfew_end_hour`
- `curfew_start_hour`
- `day_end_hour`
- `day_start_hour`
- `description`
- `display_name`
- `fatigue_recovery_modifier`
- `lighting_demand_curfew`
- `lighting_demand_day`
- `lighting_demand_night`
- `schedule_id`
- `shift_pattern`
- `trigger_condition`


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/duty_roster_seasons.json`

### `Assets/StreamingAssets/Data/duty_roster_seasons.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 1449 bytes / 1449 characters.
- SHA-256: `0df2c4aa21c5e59c68299375c3d7ed7925d6bee454bc3ccb822a912d186d9d40`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=8, max=8, observed_paths=1
```

Representative record fields:

- `encounter_weight`
- `id`
- `steam_trip_chance_boost`
- `window_max_days`
- `window_min_days`

Representative identifiers (ordered, capped for readability):

```text
season_first_ashfall
season_second_winter
season_settling
season_spring_thaw
season_faction_pressure
season_first_siege
season_consolidation
season_long_winter
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/power_grid.json`

### `Assets/StreamingAssets/Data/power_grid.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3741 bytes / 3741 characters.
- SHA-256: `8912f95d4f566a7b2c6313daab1333ee153efdf84a7340ff1790384e12296b2e`.
- Root keys: `battery_capacity_wh_default`, `emp_storm_severity`, `fuel_units_default`, `generation_watts_default`, `rooms`, `schema_version`, `surge_battery_drain_fraction`.

Array-path census (minimum, maximum, observed rows):

```text
rooms: min=18, max=18, observed_paths=1
```

Representative record fields:

- `default_priority`
- `display_name`
- `draw_watts`
- `failure_effect_id`
- `id`

Representative identifiers (ordered, capped for readability):

```text
room_air_filtration
room_clinic
room_water_pump
room_greenhouse
room_foundry
room_lighting_main
room_workshop
room_cryo_vault
room_ward_quarantine
room_heating
room_kitchen
room_water_filtration
room_airlock
room_radio_tuner
room_laboratory_research
room_workshop_precision
room_common_mess_hall
room_armory_munitions
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 198; SHA-256: `de38af17fd142fa15629d768172796847c5bbff5065a707f5583b6f6e6ce7f5a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan70_ShelterSchedulesExpansion_EndToEnd
Plan77_DutyRosterSeasonsExpansion_EndToEnd
Plan70_Plan77_CombinedEcosystem_SeasonalScheduleCoordination
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`

### `Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 318; SHA-256: `a0d3c3496cfbe6707e16871e9b136cbf2949133aa968cd286784003b4f62a2db`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsSuccessfully_HasExactCountOf12
Catalog_JsonDocument_HasSchemaVersionAndCollectionId
Catalog_PreservesThreeBaselineSchedules
Catalog_ContainsAllNineNewSchedules
Catalog_AllSchedules_HaveUniqueValidIdsWithPrefix
Catalog_AllSchedules_HaveValidHoursAndModifiers
Catalog_AllSchedules_HaveValidShiftPatternsAndTriggerConditions
System_SetSchedule_SwitchesToAll12Schedules
System_EmergencyOverride_RespectsScheduleAllowFlag
System_TryActivateScheduleByTrigger_ActivatesTargetSchedule
System_SaveAndRestore_PreservesActiveSchedule
System_TickDay_AppliesActiveScheduleModifiers
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

### `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 229; SHA-256: `3b010100ab97a56129db8dbeebd3b43a77c81dfdb5f7dd952193ac7a654fada1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/ShelterScheduleSystem.cs`

### `Assets/Ashfall.Core/ShelterScheduleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 376 lines / 14678 bytes.
- SHA-256: `637d5c2359fc965f94a573826f54c346e9b0bdc10d0edd3bd0ed8f64374c7dc0`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterScheduleState
public string systemId = ShelterScheduleSystem.SystemId;
public SchedulePhase currentPhase = SchedulePhase.Day;
public bool curfewActive;
public bool emergencyOverride;
public float fatigueRecoveryModifier = 1f;
public float lightingDemand = 0.5f;
public List<SleepAssignment> assignments = new List<SleepAssignment>();
public int lastTransitionDay = -1;
public string activeScheduleId = "default";
public sealed class ScheduleDefinition
public string schedule_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public float dayStartHour { get; set; } = 6f;
public float dayEndHour { get; set; } = 22f;
public float curfewStartHour { get; set; } = 22f;
public float curfewEndHour { get; set; } = 6f;
public float fatigueRecoveryModifier { get; set; } = 1f;
public float lightingDemandDay { get; set; } = 0.5f;
public float lightingDemandNight { get; set; } = 0.8f;
public float lightingDemandCurfew { get; set; } = 0.3f;
public bool allowEmergencyOverride { get; set; } = true;
public string shiftPattern { get; set; } = "single_shift";
public string triggerCondition { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class SleepAssignment
public string survivorId = string.Empty;
public string bedId = string.Empty;
public bool isAssigned;
public bool isCompliant;
public float restQuality = 1f;
public enum SchedulePhase { Day, Night, Curfew, Emergency } public sealed class ShelterScheduleSystem { public const string SystemId = "shelter_schedule"; private ShelterScheduleState _state = new ShelterScheduleState(); /// <summary>-1 = hour unknown (legacy day-only behaviour).</summary> private int _hourOfDay = -1; private readonly Dictionary<string, ScheduleDefinition> _catalog = new Dictionary<string, ScheduleDefinition>(StringComparer.Ordinal); private readonly ILog _log; private readonly PowerGridSystem _powerGrid; private string _activeScheduleId = "default"; public ShelterScheduleState State => _state; public SchedulePhase CurrentPhase => _state.currentPhase; public bool IsCurfewActive => _state.curfewActive && !_state.emergencyOverride; public bool IsEmergencyOverride => _state.emergencyOverride; public float FatigueRecoveryModifier => _state.fatigueRecoveryModifier; public float LightingDemand => _state.lightingDemand; public string ActiveScheduleId => _activeScheduleId; public IReadOnlyCollection<ScheduleDefinition> GetAllSchedules() => _catalog.Values; public ScheduleDefinition? GetSchedule(string scheduleId) => _catalog.TryGetValue(scheduleId, out var def) ? def : null; public bool TryActivateScheduleByTrigger(string triggerCondition) { if (string.IsNullOrEmpty(triggerCondition)) return false; foreach (var kvp in _catalog) { if (string.Equals(kvp.Value.triggerCondition, triggerCondition, StringComparison.OrdinalIgnoreCase)) { var res = SetSchedule(kvp.Key); return res.IsSuccess; }
public event Action<SchedulePhase> OnPhaseChanged;
public event Action OnScheduleChanged;
public void LoadCatalog(List<ScheduleDefinition> definitions) {
public ActionResult SetSchedule(string scheduleId) {
public ActionResult SetCurfew(bool active) {
public ActionResult SetEmergencyOverride(bool active) {
public ActionResult AssignBed(string survivorId, string bedId) {
public ActionResult UnassignBed(string survivorId) {
public void TickDay(int day) {
public ScheduleDefinition? GetActiveSchedule() {
public bool IsSleepEligible(string survivorId) {
public SchedulePhase PhaseForHour(int hourOfDay) {
public void TickHour(int hourOfDay) {
public ShelterScheduleState CaptureState() {
public void RestoreState(ShelterScheduleState saved) {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs`

### `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 51 lines / 1775 bytes.
- SHA-256: `bdd89c824da3d489dce770b3b745df46ae5dcbc503ce1489726c05acfde9e63f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterScheduleCatalogContainer
public List<ScheduleDefinition> schedules = new List<ScheduleDefinition>();
public static class ShelterScheduleCatalogLoader
public const string DefaultFileName = "shelter_schedules.json";
public static List<ScheduleDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( ShelterScheduleSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`

### `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1415 lines / 62138 bytes.
- SHA-256: `01d76096907f04a268dd10572a9d6635286d47742922420c81e6c7863ae39f09`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=20; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PowerGridSystem
public event Action<PowerGridEvent>? OnPowerChanged;
public event Action<PowerGridTickSummary>? OnTickSummary;
public PowerGridState State => _state;
public IReadOnlyList<PowerGridRoom> Rooms => _rooms;
public float BaseGenerationWatts => _state.GenerationWatts;
public IReadOnlyDictionary<string, float> GenerationContributions => _generationContributions;
public float FuelUnits => _state.FuelUnits;
public float BatteryReserveWh => _state.BatteryReserveWh;
public float BatteryCapacityWh => _state.BatteryCapacityWh;
public float TotalDrawWatts => ComputeTotalDraw();
public float NetWatts => GenerationWatts - TotalDrawWatts;
public bool IsBrownout => TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0;
public float SustainableBatteryDischargeWatts => Math.Max(0f, _state.BatteryReserveWh / 24f);
public float AvailableSupplyWatts => GenerationWatts + SustainableBatteryDischargeWatts;
public float DeficitWatts => Math.Max(0f, TotalDrawWatts - AvailableSupplyWatts);
public bool SetGenerationContribution(string sourceId, float watts) {
public bool RemoveGenerationContribution(string sourceId) {
public const string EbPvdInstalledSourceId = "ebpvd_installed";
public const int MaxInstalledCoatedParts = 3;
public const float MaxEbPvdInstalledWatts = 80f;
public const string BatteryBankItemId = "item_battery_reconditioned";
public const float BatteryBankCapacityWh = 1000f;
public const int MaxInstalledBatteryBanks = 4;
public const string GeneratorMaintenanceItemId = "machine_oil";
public const float GeneratorWearPerDay = 0.25f;
public const float GeneratorDegradationThreshold = 50f;
public const float GeneratorMinOutputFactor = 0.5f;
public float GeneratorCondition => _state.GeneratorCondition;
public float GeneratorOutputFactor => _state.GeneratorCondition >= GeneratorDegradationThreshold
public bool PerformGeneratorMaintenance(out string reason) {
public bool TryInstallBatteryBank(out string reason) {
public int InstalledBatteryBankCount => _state.InstalledBatteryBankCount;
public IReadOnlyList<string> InstalledCoatedPartItemIds => _state.InstalledCoatedPartItemIds;
public bool TryInstallCoatedPart(string itemId, out string reason) {
public bool TryUninstallCoatedPart(string itemId, out string reason) {
public void RepublishEbPvdInstalledContribution() {
public static string ResolveCoatedPartFamily(string itemId) {
public static float ResolveCoatedPartWatts(string itemId) {
public PowerGridSnapshot Snapshot() {
public bool IsRoomPowered(string roomId) {
public PowerGridRoomPriority EffectivePriority(string roomId) {
public bool ToggleBreaker(string roomId) {
public bool SetBreaker(string roomId, bool closed) {
public void MarkTripped(string roomId, int day) {
public void ClearTripped(string roomId) => _state.ClearTripped(roomId);
public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);
public bool SetPriority(string roomId, PowerGridRoomPriority priority) {
public bool RegisterLoadRoom(PowerGridRoom room) {
public IReadOnlyList<string> ApplyBrownoutShedPreset() {
public int ApplyCatalogDefaultPriorities() {
public void AddFuel(float units) {
public const float DefaultEmpStormSurgeSeverity = 0.6f;
public const float DefaultSurgeBatteryDrainFraction = 0.15f;
public void ConfigureSurge(float empStormSeverity, float batteryDrainFraction) {
public float EmpStormSeverity => _empStormSurgeSeverity;
public float SurgeBatteryDrain => _surgeBatteryDrainFraction;
public IReadOnlyList<string> ApplySurgeDay(int day, float severity01) {
public PowerGridTickSummary TickDay(int day, ISeededRng tickRng) {
internal const float AllocationEpsilon = 0.01f;
public bool IsRoomServed(string roomId) {
public float ServedWatts;
public bool HasCriticalDeficit;
public List<string> ServedRoomIds = new List<string>();
public List<string> ShedRoomIds = new List<string>();
public PowerGridState CaptureState() => _state.Capture();
public void RestoreState(PowerGridState state) {
public float GetRoomDrawWatts(string roomId) {
public float EffectiveTotalDrawWatts =>
public enum PowerGridRoomPriority
public sealed class PowerGridRoom
public string RoomId;
public string DisplayName;
public float DrawWatts;
public PowerGridRoomPriority DefaultPriority;
public string FailureEffectId; // semantic id the host looks up.
public sealed class PowerGridState
public int SimDay;
public float GenerationWatts;
public float FuelUnits;
public float BatteryReserveWh;
public float BatteryCapacityWh;
public List<string> ClosedBreakers = new List<string>();
public List<string> TrippedRooms = new List<string>();
public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();
public int LastSurgeDay;
public List<string> InstalledCoatedPartItemIds = new List<string>();
public int InstalledBatteryBankCount;
public float GeneratorCondition = 100f;
public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);
public void SetBreaker(string roomId, bool closed) {
public void MarkTripped(string roomId, int day) {
public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);
public PowerGridRoomPriority GetRoomPriority(string roomId) {
public void SetRoomPriority(string roomId, PowerGridRoomPriority priority) {
public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms) {
public PowerGridState Capture() {
public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms) {
internal static float SanitizeNonNegativeFinite(float value) =>
public sealed class RoomPriorityRecord
public string RoomId;
public PowerGridRoomPriority Priority;
public sealed class PowerGridEvent
public PowerGridEventKind Kind;
public string RoomId;
public int Day;
public string Detail;
public float Numeric;
public enum PowerGridEventKind
public sealed class PowerGridTickSummary
public int Day;
public float FuelConsumed;
public float BatteryEndWh;
public float BrownoutHours;
public bool IsBrownout;
public float GenerationWatts;
public float RequestedDrawWatts;
public float ServedWatts;
public float UnservedWatts;
public bool HasCriticalDeficit;
public bool BrownoutBegan;
public bool BrownoutEnded;
public List<string> ServedRoomIds = new List<string>();
public List<string> ShedRoomIds = new List<string>();
public sealed class PowerGridSnapshot
public int Day;
public float GenerationWatts;
public float FuelUnits;
public float BatteryReserveWh;
public float BatteryCapacityWh;
public float TotalDrawWatts;
public float NetWatts;
public bool IsBrownout;
public List<string> RoomIds = new List<string>();
public Dictionary<string, float> GenerationContributions = new Dictionary<string, float>(StringComparer.Ordinal);
```


# Appendix E.18 — Supporting Code Evidence: `src/Host/ShelterScheduleHostSession.cs`

### `src/Host/ShelterScheduleHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 111 lines / 3763 bytes.
- SHA-256: `ea5a50b5e4328d1907b3407e7dfbb235762887d5db58fc3d3e1075db96f53e2a`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterScheduleHostSession
public ShelterScheduleSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult SetCurfew(bool active) {
public ActionResult SetEmergencyOverride(bool active) {
public ActionResult AssignBed(string survivorId, string bedId) {
public void LoadCatalog(string dataDir) {
public void TickDay(int day) {
public void TickHour(int hourOfDay) {
public override void Save() {
```


# Appendix E.19 — Supporting Code Evidence: `src/Host/ShelterScheduleSaveStore.cs`

### `src/Host/ShelterScheduleSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2760 bytes.
- SHA-256: `4d4d9e6a62dc0d98c338bff509d0366841635d2cbac52eabbdaca78bd0f4d6aa`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ShelterScheduleSaveStore
public const string FileName = "shelter_schedule_save.json";
public const string SectionName = "shelter_schedule";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(ShelterScheduleState state) => s_store.CaptureBare(state);
public static ShelterScheduleState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ShelterScheduleState state) => s_store.CaptureBare(state);
public static ShelterScheduleState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(ShelterScheduleState state) => s_store.TrySave(state);
public static ShelterScheduleState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ShelterScheduleState state) => s_store.CapturePersisted(state);
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 198; SHA-256: `de38af17fd142fa15629d768172796847c5bbff5065a707f5583b6f6e6ce7f5a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan70_ShelterSchedulesExpansion_EndToEnd
Plan77_DutyRosterSeasonsExpansion_EndToEnd
Plan70_Plan77_CombinedEcosystem_SeasonalScheduleCoordination
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`

### `Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 318; SHA-256: `a0d3c3496cfbe6707e16871e9b136cbf2949133aa968cd286784003b4f62a2db`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsSuccessfully_HasExactCountOf12
Catalog_JsonDocument_HasSchemaVersionAndCollectionId
Catalog_PreservesThreeBaselineSchedules
Catalog_ContainsAllNineNewSchedules
Catalog_AllSchedules_HaveUniqueValidIdsWithPrefix
Catalog_AllSchedules_HaveValidHoursAndModifiers
Catalog_AllSchedules_HaveValidShiftPatternsAndTriggerConditions
System_SetSchedule_SwitchesToAll12Schedules
System_EmergencyOverride_RespectsScheduleAllowFlag
System_TryActivateScheduleByTrigger_ActivatesTargetSchedule
System_SaveAndRestore_PreservesActiveSchedule
System_TickDay_AppliesActiveScheduleModifiers
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

### `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 229; SHA-256: `3b010100ab97a56129db8dbeebd3b43a77c81dfdb5f7dd952193ac7a654fada1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix H.23 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

### `docs/CURRENT_AUTHORITY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 9950 bytes.
- SHA-256: `7dea2c12b4863bfc9a3c2ebb161ba51512ebc06475b762abd5d5d205bef47e5c`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| authored schedule definitions | ShelterScheduleCatalogLoader | phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | Owner emits/reads a typed fact; no mirror state. |
| authored schedule definitions | ShelterScheduleCatalogLoader | brownout/power state | PowerGridSystem | Owner emits/reads a typed fact; no mirror state. |
| authored schedule definitions | ShelterScheduleCatalogLoader | load/tick/command/save composition | ShelterScheduleHostSession | Owner emits/reads a typed fact; no mirror state. |
| authored schedule definitions | ShelterScheduleCatalogLoader | player projection | ShelterSchedulePanel | Owner emits/reads a typed fact; no mirror state. |
| authored schedule definitions | ShelterScheduleCatalogLoader | catalog, phase, hour and save proof | Schedule focused tests | Owner emits/reads a typed fact; no mirror state. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | authored schedule definitions | ShelterScheduleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | brownout/power state | PowerGridSystem | Owner emits/reads a typed fact; no mirror state. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | load/tick/command/save composition | ShelterScheduleHostSession | Owner emits/reads a typed fact; no mirror state. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | player projection | ShelterSchedulePanel | Owner emits/reads a typed fact; no mirror state. |
| phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | catalog, phase, hour and save proof | Schedule focused tests | Owner emits/reads a typed fact; no mirror state. |
| brownout/power state | PowerGridSystem | authored schedule definitions | ShelterScheduleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| brownout/power state | PowerGridSystem | phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | Owner emits/reads a typed fact; no mirror state. |
| brownout/power state | PowerGridSystem | load/tick/command/save composition | ShelterScheduleHostSession | Owner emits/reads a typed fact; no mirror state. |
| brownout/power state | PowerGridSystem | player projection | ShelterSchedulePanel | Owner emits/reads a typed fact; no mirror state. |
| brownout/power state | PowerGridSystem | catalog, phase, hour and save proof | Schedule focused tests | Owner emits/reads a typed fact; no mirror state. |
| load/tick/command/save composition | ShelterScheduleHostSession | authored schedule definitions | ShelterScheduleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| load/tick/command/save composition | ShelterScheduleHostSession | phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | Owner emits/reads a typed fact; no mirror state. |
| load/tick/command/save composition | ShelterScheduleHostSession | brownout/power state | PowerGridSystem | Owner emits/reads a typed fact; no mirror state. |
| load/tick/command/save composition | ShelterScheduleHostSession | player projection | ShelterSchedulePanel | Owner emits/reads a typed fact; no mirror state. |
| load/tick/command/save composition | ShelterScheduleHostSession | catalog, phase, hour and save proof | Schedule focused tests | Owner emits/reads a typed fact; no mirror state. |
| player projection | ShelterSchedulePanel | authored schedule definitions | ShelterScheduleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| player projection | ShelterSchedulePanel | phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | Owner emits/reads a typed fact; no mirror state. |
| player projection | ShelterSchedulePanel | brownout/power state | PowerGridSystem | Owner emits/reads a typed fact; no mirror state. |
| player projection | ShelterSchedulePanel | load/tick/command/save composition | ShelterScheduleHostSession | Owner emits/reads a typed fact; no mirror state. |
| player projection | ShelterSchedulePanel | catalog, phase, hour and save proof | Schedule focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, phase, hour and save proof | Schedule focused tests | authored schedule definitions | ShelterScheduleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog, phase, hour and save proof | Schedule focused tests | phase, curfew, assignments, modifiers and lighting state | ShelterScheduleSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, phase, hour and save proof | Schedule focused tests | brownout/power state | PowerGridSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, phase, hour and save proof | Schedule focused tests | load/tick/command/save composition | ShelterScheduleHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog, phase, hour and save proof | Schedule focused tests | player projection | ShelterSchedulePanel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 3→12 target with a 12-row current schedule census and trigger/shift-pattern matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Verify hour wrapping, day/night/curfew/emergency precedence and brownout lighting math against the current source. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Trace assignments, fatigue recovery and lighting demand into their existing Needs/DutyRoster/PowerGrid consumers without copying their state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Audit the current panel’s schedule selection, refusal, close/back, controller and refresh behavior. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
| C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
| C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
| C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
| C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
| C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
| C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
| C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
| C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
| C10 | Quest state reopening after new discoveries (failure-recovery grammar, v1.0 Part 6.7); moral-choice flag consumers beyond the flag ledger | HIGH CONFIDENCE |
| C11 | Black-market funds/goods legs remain decision-gated (canonical funds authority); merchant restock priority is SEALED by DEC-05 (DR-06) | BLOCKED / SEALED |
| C12 | Year-of-Ash tick-window extensions (180–360) for systems not yet producing winter pressure | PROPOSAL |
| C13 | Reckoning evidence enrollment for systems added since the last endgame wave (19A/19B/19C closed, DR-06) | HIGH CONFIDENCE |
| C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
| C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
| C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
| C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |

> **SB-11 — C2 open-gap package: Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix (Lanes B, F, G).** Evidence: DR-06 not-executed lists. Subject: three bounded follow-ons the ledger itself records as real gaps. Integration route: per existing C2 plan documentation. Confidence: VERIFIED as open; scope per item needs the plan docs.

> ### Subject
Three bounded follow-ons that the integration ledger itself records as real, unexecuted gaps (DR-06): Plan 31 semantic-kind authority; 17C Phase I alert ducking/concurrency and Phase E acquisition sweep; 17B deep test matrix.

> ### What must not change
The 17A-S semantic parity matrix and its gate; the already-built 17B route/visibility and most 17C audio phases (stale per recon — do not rebuild what exists).

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is the shelter’s temporal operating rhythm and its truthful projection into phase, power, rest and duty consumers. The plan expands the current schedule owner’s boundary and verification depth without creating a parallel calendar.

- **authored schedule definitions** remains with `ShelterScheduleCatalogLoader` at `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs`. Loads the JSON container; it does not decide when a schedule is active.
- **phase, curfew, assignments, modifiers and lighting state** remains with `ShelterScheduleSystem` at `Assets/Ashfall.Core/ShelterScheduleSystem.cs`. Sole schedule authority; accepts PowerGrid as a read-only brownout dependency.
- **brownout/power state** remains with `PowerGridSystem` at `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`. Owns grid state; schedule consumes it rather than duplicating it.
- **load/tick/command/save composition** remains with `ShelterScheduleHostSession` at `src/Host/ShelterScheduleHostSession.cs; src/Host/ShelterScheduleSaveStore.cs`. Thin host boundary.
- **player projection** remains with `ShelterSchedulePanel` at `src/UI/ShelterSchedulePanel.cs`. Read-only presentation and command binding.
- **catalog, phase, hour and save proof** remains with `Schedule focused tests` at `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs; Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs; Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load schedule definitions
2. activate a schedule through the current host command
3. feed campaign day/hour into the owner
4. derive phase and curfew/emergency state
5. project lighting/fatigue values to existing consumers
6. assign/unassign beds through the owner
7. capture/restore shelter schedule state and refresh the panel

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Schedule definitions are immutable data; active schedule, phase, curfew/emergency flags, assignments and derived modifiers are owner state.
- Hour transitions are explicit and legacy day-only behavior remains supported when hour is unknown.
- Emergency override is rejected when the active definition disallows it; brownout modifies the published lighting demand after base demand is selected.
- Restore preserves active schedule, phase, assignments and last transition day without silently resetting a campaign.

- Unknown schedule IDs return a named failure and leave active state unchanged.
- Emergency override cannot bypass a schedule’s explicit prohibition.
- Brownout cannot double-apply or erase the base lighting demand.
- The same day/hour/grid state produces the same phase, demand and recovery values.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/ShelterScheduleHostSession.cs
- src/Host/ShelterScheduleSaveStore.cs
- src/Main.ShelterInfrastructure.cs
- src/UI/ShelterSchedulePanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs
- Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs
- Ashfall.Core.Tests/ShelterScheduleSystemTests.cs

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
| S-01 | 70-01 default schedule activation | load schedule definitions | Schedule definitions are immutable data; active schedule, phase, curfew/emergency flags, assignments and derived modifiers are owner state. | A schedule row is displayed but not registered. | ShelterScheduleCatalogLoader |
| S-02 | 70-02 unknown schedule refusal | activate a schedule through the current host command | Hour transitions are explicit and legacy day-only behavior remains supported when hour is unknown. | Curfew/emergency precedence differs between Core and panel. | ShelterScheduleCatalogLoader |
| S-03 | 70-03 hour transition day to night | feed campaign day/hour into the owner | Emergency override is rejected when the active definition disallows it; brownout modifies the published lighting demand after base demand is selected. | Brownout demand is applied twice or not at all. | ShelterScheduleCatalogLoader |
| S-04 | 70-04 curfew boundary and wrap | derive phase and curfew/emergency state | Restore preserves active schedule, phase, assignments and last transition day without silently resetting a campaign. | Assignments are lost on restore. | ShelterScheduleCatalogLoader |
| S-05 | 70-05 emergency override allowed | project lighting/fatigue values to existing consumers | Schedule definitions are immutable data; active schedule, phase, curfew/emergency flags, assignments and derived modifiers are owner state. | A panel changes phase without calling the owner. | ShelterScheduleCatalogLoader |
| S-06 | 70-06 emergency override blocked | assign/unassign beds through the owner | Hour transitions are explicit and legacy day-only behavior remains supported when hour is unknown. | A schedule row is displayed but not registered. | ShelterScheduleCatalogLoader |
| S-07 | 70-07 brownout lighting demand | capture/restore shelter schedule state and refresh the panel | Emergency override is rejected when the active definition disallows it; brownout modifies the published lighting demand after base demand is selected. | Curfew/emergency precedence differs between Core and panel. | ShelterScheduleCatalogLoader |
| S-08 | 70-08 bed assignment/restore | load schedule definitions | Restore preserves active schedule, phase, assignments and last transition day without silently resetting a campaign. | Brownout demand is applied twice or not at all. | ShelterScheduleCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 70-TC-01 catalog row/schema validation | data | catalog row/schema validation; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-02 | 70-TC-02 schedule ID lookup | unit | schedule ID lookup; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-03 | 70-TC-03 trigger activation | persistence | trigger activation; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-04 | 70-TC-04 day/night hour boundaries | determinism | day/night hour boundaries; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-05 | 70-TC-05 curfew precedence | host | curfew precedence; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-06 | 70-TC-06 emergency prohibition | UI/accessibility | emergency prohibition; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-07 | 70-TC-07 brownout multiplier | cross-system | brownout multiplier; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-08 | 70-TC-08 fatigue modifier | data | fatigue modifier; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-09 | 70-TC-09 bed assignment idempotence | unit | bed assignment idempotence; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-10 | 70-TC-10 unassign behavior | persistence | unassign behavior; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-11 | 70-TC-11 day/hour tick order | determinism | day/hour tick order; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-12 | 70-TC-12 save deep copy | host | save deep copy; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-13 | 70-TC-13 legacy restore defaults | UI/accessibility | legacy restore defaults; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-14 | 70-TC-14 panel projection | cross-system | panel projection; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |
| T-15 | 70-TC-15 keyboard/controller focus | data | keyboard/controller focus; verify the current owner and its negative boundary without inventing a second authority. | ShelterScheduleCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 25 | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 24 | `Ashfall.Core.Tests/Shelter/Plan188ScheduleHourConsumerTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 15 | `Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `src/Host/ShelterScheduleHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Shelter/SourceFailureEventTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/PowerGridHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Main.ShelterInfrastructure.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Main.World.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/NewCatalogLoaderTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Shelter/Plan70_77ScheduleSeasonIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Shelter/PowerGridPhase5MaintenanceTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/SumpFloodingSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/Plans74To77HostSessions.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/ShelterScheduleSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Plan194CrisisProducerWireTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Shelter/Phase3WaterIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Shelter/Plan23APowerContinuityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Shelter/Plan23_20PowerExposureIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Shelter/PowerGridPhase2AllocationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Shelter/PowerGridSurgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/VentilationElectrostaticIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Water/DeepWellSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/DeepWellSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Audio/AudioSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Audio/ScarcityAudioController.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/shelter_schedules.json`

### `Assets/StreamingAssets/Data/shelter_schedules.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 7638; characters: 7638.
- SHA-256: `521173a5cf43ddc5d00bad85892f7733a1880b45560a9568c8c44ed0ee687f5c`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `collection_id`, `schedules`

#### `schedules` — 12 current rows

- Row 001 `schedule_standard`: `{"allow_emergency_override":true,"curfew_end_hour":6.0,"curfew_start_hour":22.0,"day_end_hour":22.0,"day_start_hour":6.0,"description":"Standard daily rotation balancing workshop productivity, domestic upkeep, and night curfew rest.","disp…`
- Row 002 `schedule_night_shift`: `{"allow_emergency_override":true,"curfew_end_hour":18.0,"curfew_start_hour":6.0,"day_end_hour":6.0,"day_start_hour":18.0,"description":"Inverted diurnal cycle prioritizing subterranean industry during nocturnal surface cool-down.","display…`
- Row 003 `schedule_curfew_locked`: `{"allow_emergency_override":false,"curfew_end_hour":8.0,"curfew_start_hour":20.0,"day_end_hour":20.0,"day_start_hour":8.0,"description":"Strictly enforced room confinement suppressing social contagion, perimeter exposure, and unauthorized …`
- Row 004 `schedule_emergency_shifts`: `{"allow_emergency_override":true,"curfew_end_hour":6.0,"curfew_start_hour":22.0,"day_end_hour":24.0,"day_start_hour":0.0,"description":"Continuous around-the-clock emergency response with staggered catnaps to avert catastrophic infrastruct…`
- Row 005 `schedule_siege_watch`: `{"allow_emergency_override":false,"curfew_end_hour":6.0,"curfew_start_hour":20.0,"day_end_hour":24.0,"day_start_hour":0.0,"description":"Armed perimeter guard rotations with reinforced airlock security and dark-bunker sound discipline.","d…`
- Row 006 `schedule_winter_hibernation`: `{"allow_emergency_override":true,"curfew_end_hour":8.0,"curfew_start_hour":18.0,"day_end_hour":17.0,"day_start_hour":8.0,"description":"Sub-zero thermal conservation confining dwellers to insulated quarters to minimize generator fuel consu…`
- Row 007 `schedule_mourning`: `{"allow_emergency_override":true,"curfew_end_hour":7.0,"curfew_start_hour":19.0,"day_end_hour":19.0,"day_start_hour":7.0,"description":"Suspended heavy fabrication and subdued low-intensity maintenance following the loss of a shelter dwell…`
- Row 008 `schedule_festival_day`: `{"allow_emergency_override":true,"curfew_end_hour":6.0,"curfew_start_hour":1.0,"day_end_hour":24.0,"day_start_hour":6.0,"description":"Extended communal gathering and recreation lifting morale at the expense of elevated electrical consumpt…`
- Row 009 `schedule_rationing`: `{"allow_emergency_override":true,"curfew_end_hour":8.0,"curfew_start_hour":18.0,"day_end_hour":17.0,"day_start_hour":9.0,"description":"Enforced metabolic deceleration minimizing caloric expenditure during acute nutrition deficit.","displa…`
- Row 010 `schedule_quarantine`: `{"allow_emergency_override":false,"curfew_end_hour":7.0,"curfew_start_hour":21.0,"day_end_hour":21.0,"day_start_hour":7.0,"description":"Hermetic compartmentalization sealing medbay and common corridors into non-overlapping circulation zon…`
- Row 011 `schedule_construction_push`: `{"allow_emergency_override":true,"curfew_end_hour":5.0,"curfew_start_hour":23.0,"day_end_hour":23.0,"day_start_hour":5.0,"description":"Aggressive multi-gang engineering push mobilizing all able-bodied dwellers into the fabrication bays.",…`
- Row 012 `schedule_scout_rotation`: `{"allow_emergency_override":true,"curfew_end_hour":4.0,"curfew_start_hour":22.0,"day_end_hour":22.0,"day_start_hour":4.0,"description":"Pre-dawn staging and staggered logistics synchronization keeping airlocks and scouting parties in conti…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/duty_roster_seasons.json`

### `Assets/StreamingAssets/Data/duty_roster_seasons.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 1449; characters: 1449.
- SHA-256: `0df2c4aa21c5e59c68299375c3d7ed7925d6bee454bc3ccb822a912d186d9d40`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 8 current rows

- Row 001 `season_first_ashfall`: `{"encounter_weight":1.45,"id":"season_first_ashfall","steam_trip_chance_boost":0.01,"window_max_days":7,"window_min_days":0}`
- Row 002 `season_second_winter`: `{"encounter_weight":1.6,"id":"season_second_winter","steam_trip_chance_boost":0.08,"window_max_days":12,"window_min_days":8}`
- Row 003 `season_settling`: `{"encounter_weight":1.0,"id":"season_settling","steam_trip_chance_boost":0.06,"window_max_days":30,"window_min_days":13}`
- Row 004 `season_spring_thaw`: `{"encounter_weight":0.75,"id":"season_spring_thaw","steam_trip_chance_boost":0.12,"window_max_days":60,"window_min_days":31}`
- Row 005 `season_faction_pressure`: `{"encounter_weight":1.3,"id":"season_faction_pressure","steam_trip_chance_boost":0.05,"window_max_days":120,"window_min_days":61}`
- Row 006 `season_first_siege`: `{"encounter_weight":1.75,"id":"season_first_siege","steam_trip_chance_boost":0.03,"window_max_days":180,"window_min_days":121}`
- Row 007 `season_consolidation`: `{"encounter_weight":1.0,"id":"season_consolidation","steam_trip_chance_boost":0.09,"window_max_days":240,"window_min_days":181}`
- Row 008 `season_long_winter`: `{"encounter_weight":1.5,"id":"season_long_winter","steam_trip_chance_boost":0.02,"window_max_days":365,"window_min_days":241}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/power_grid.json`

### `Assets/StreamingAssets/Data/power_grid.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 3741; characters: 3741.
- SHA-256: `8912f95d4f566a7b2c6313daab1333ee153efdf84a7340ff1790384e12296b2e`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `generation_watts_default`, `battery_capacity_wh_default`, `fuel_units_default`, `rooms`, `emp_storm_severity`, `surge_battery_drain_fraction`

#### `rooms` — 18 current rows

- Row 001 `room_air_filtration`: `{"default_priority":"critical","display_name":"Air Filtration","draw_watts":180,"failure_effect_id":"fx_filtration_off","id":"room_air_filtration"}`
- Row 002 `room_clinic`: `{"default_priority":"critical","display_name":"Clinic","draw_watts":120,"failure_effect_id":"fx_clinic_off","id":"room_clinic"}`
- Row 003 `room_water_pump`: `{"default_priority":"critical","display_name":"Water Pump","draw_watts":100,"failure_effect_id":"fx_water_pressure_drop","id":"room_water_pump"}`
- Row 004 `room_greenhouse`: `{"default_priority":"standard","display_name":"Greenhouse","draw_watts":160,"failure_effect_id":"fx_grow_lights_off","id":"room_greenhouse"}`
- Row 005 `room_foundry`: `{"default_priority":"low","display_name":"Silent Foundry","draw_watts":220,"failure_effect_id":"fx_foundry_standstill","id":"room_foundry"}`
- Row 006 `room_lighting_main`: `{"default_priority":"low","display_name":"Main Lighting","draw_watts":80,"failure_effect_id":"fx_lighting_dim","id":"room_lighting_main"}`
- Row 007 `room_workshop`: `{"default_priority":"low","display_name":"Workshop","draw_watts":300,"failure_effect_id":"fx_workshop_offline","id":"room_workshop"}`
- Row 008 `room_cryo_vault`: `{"default_priority":"critical","display_name":"Cryo Vault","draw_watts":280,"failure_effect_id":"fx_cryo_vault_unpowered","id":"room_cryo_vault"}`
- Row 009 `room_ward_quarantine`: `{"default_priority":"critical","display_name":"Quarantine Ward","draw_watts":90,"failure_effect_id":"fx_quarantine_ventilation_off","id":"room_ward_quarantine"}`
- Row 010 `room_heating`: `{"default_priority":"standard","display_name":"Electric Heating","draw_watts":240,"failure_effect_id":"fx_heating_off","id":"room_heating"}`
- Row 011 `room_kitchen`: `{"default_priority":"standard","display_name":"Kitchen","draw_watts":150,"failure_effect_id":"fx_kitchen_off","id":"room_kitchen"}`
- Row 012 `room_water_filtration`: `{"default_priority":"critical","display_name":"Water Filtration","draw_watts":140,"failure_effect_id":"fx_water_filtration_off","id":"room_water_filtration"}`
- Row 013 `room_airlock`: `{"default_priority":"standard","display_name":"Airlock Decontamination","draw_watts":130,"failure_effect_id":"fx_airlock_decon_off","id":"room_airlock"}`
- Row 014 `room_radio_tuner`: `{"default_priority":"standard","display_name":"Radio Room","draw_watts":90,"failure_effect_id":"fx_radio_tuner_off","id":"room_radio_tuner"}`
- Row 015 `room_laboratory_research`: `{"default_priority":"standard","display_name":"Laboratory","draw_watts":260,"failure_effect_id":"fx_laboratory_offline","id":"room_laboratory_research"}`
- Row 016 `room_workshop_precision`: `{"default_priority":"standard","display_name":"Precision Workshop","draw_watts":240,"failure_effect_id":"fx_precision_metrology_off","id":"room_workshop_precision"}`
- Row 017 `room_common_mess_hall`: `{"default_priority":"low","display_name":"Common Mess Hall","draw_watts":70,"failure_effect_id":"fx_common_mess_cold","id":"room_common_mess_hall"}`
- Row 018 `room_armory_munitions`: `{"default_priority":"standard","display_name":"Armory & Munitions","draw_watts":60,"failure_effect_id":"fx_armory_service_off","id":"room_armory_munitions"}`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/ShelterScheduleSystem.cs`

### `Assets/Ashfall.Core/ShelterScheduleSystem.cs` — complete current file

- Size: 376 lines / 14678 bytes.
- SHA-256: `637d5c2359fc965f94a573826f54c346e9b0bdc10d0edd3bd0ed8f64374c7dc0`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text.Json.Serialization;
00005: #pragma warning disable CS8618
00006:
00007: using Ashfall.Core.Shelter;
00008:
00009: namespace Ashfall.Core
00010: {
00011:     [Serializable]
00012:     public sealed class ShelterScheduleState
00013:     {
00014:         public string systemId = ShelterScheduleSystem.SystemId;
00015:         public SchedulePhase currentPhase = SchedulePhase.Day;
00016:         public bool curfewActive;
00017:         public bool emergencyOverride;
00018:         public float fatigueRecoveryModifier = 1f;
00019:         public float lightingDemand = 0.5f;
00020:         public List<SleepAssignment> assignments = new List<SleepAssignment>();
00021:         public int lastTransitionDay = -1;
00022:         public string activeScheduleId = "default";
00023:     }
00024:
00025:     [Serializable]
00026:     public sealed class ScheduleDefinition
00027:     {
00028:         [JsonPropertyName("schedule_id")]
00029:         public string schedule_id { get; set; } = string.Empty;
00030:
00031:         [JsonPropertyName("display_name")]
00032:         public string display_name { get; set; } = string.Empty;
00033:
00034:         [JsonPropertyName("day_start_hour")]
00035:         public float dayStartHour { get; set; } = 6f;
00036:
00037:         [JsonPropertyName("day_end_hour")]
00038:         public float dayEndHour { get; set; } = 22f;
00039:
00040:         [JsonPropertyName("curfew_start_hour")]
00041:         public float curfewStartHour { get; set; } = 22f;
00042:
00043:         [JsonPropertyName("curfew_end_hour")]
00044:         public float curfewEndHour { get; set; } = 6f;
00045:
00046:         [JsonPropertyName("fatigue_recovery_modifier")]
00047:         public float fatigueRecoveryModifier { get; set; } = 1f;
00048:
00049:         [JsonPropertyName("lighting_demand_day")]
00050:         public float lightingDemandDay { get; set; } = 0.5f;
00051:
00052:         [JsonPropertyName("lighting_demand_night")]
00053:         public float lightingDemandNight { get; set; } = 0.8f;
00054:
00055:         [JsonPropertyName("lighting_demand_curfew")]
00056:         public float lightingDemandCurfew { get; set; } = 0.3f;
00057:
00058:         [JsonPropertyName("allow_emergency_override")]
00059:         public bool allowEmergencyOverride { get; set; } = true;
00060:
00061:         [JsonPropertyName("shift_pattern")]
00062:         public string shiftPattern { get; set; } = "single_shift";
00063:
00064:         [JsonPropertyName("trigger_condition")]
00065:         public string triggerCondition { get; set; } = string.Empty;
00066:
00067:         [JsonPropertyName("description")]
00068:         public string description { get; set; } = string.Empty;
00069:     }
00070:
00071:     [Serializable]
00072:     public sealed class SleepAssignment
00073:     {
00074:         public string survivorId = string.Empty;
00075:         public string bedId = string.Empty;
00076:         public bool isAssigned;
00077:         public bool isCompliant;
00078:         public float restQuality = 1f;
00079:     }
00080:
00081:     public enum SchedulePhase { Day, Night, Curfew, Emergency }
00082:
00083:     public sealed class ShelterScheduleSystem
00084:     {
00085:         public const string SystemId = "shelter_schedule";
00086:         private ShelterScheduleState _state = new ShelterScheduleState();
00087:         /// <summary>-1 = hour unknown (legacy day-only behaviour).</summary>
00088:         private int _hourOfDay = -1;
00089:         private readonly Dictionary<string, ScheduleDefinition> _catalog = new Dictionary<string, ScheduleDefinition>(StringComparer.Ordinal);
00090:         private readonly ILog _log;
00091:         private readonly PowerGridSystem _powerGrid;
00092:         private string _activeScheduleId = "default";
00093:
00094:         public ShelterScheduleState State => _state;
00095:         public SchedulePhase CurrentPhase => _state.currentPhase;
00096:         public bool IsCurfewActive => _state.curfewActive && !_state.emergencyOverride;
00097:         public bool IsEmergencyOverride => _state.emergencyOverride;
00098:         public float FatigueRecoveryModifier => _state.fatigueRecoveryModifier;
00099:         public float LightingDemand => _state.lightingDemand;
00100:         public string ActiveScheduleId => _activeScheduleId;
00101:
00102:         public IReadOnlyCollection<ScheduleDefinition> GetAllSchedules() => _catalog.Values;
00103:
00104:         public ScheduleDefinition? GetSchedule(string scheduleId) =>
00105:             _catalog.TryGetValue(scheduleId, out var def) ? def : null;
00106:
00107:         public bool TryActivateScheduleByTrigger(string triggerCondition)
00108:         {
00109:             if (string.IsNullOrEmpty(triggerCondition)) return false;
00110:             foreach (var kvp in _catalog)
00111:             {
00112:                 if (string.Equals(kvp.Value.triggerCondition, triggerCondition, StringComparison.OrdinalIgnoreCase))
00113:                 {
00114:                     var res = SetSchedule(kvp.Key);
00115:                     return res.IsSuccess;
00116:                 }
00117:             }
00118:             return false;
00119:         }
00120:
00121:         public event Action<SchedulePhase> OnPhaseChanged;
00122:         public event Action OnScheduleChanged;
00123:
00124:         public ShelterScheduleSystem(PowerGridSystem powerGrid, ILog? log = null)
00125:         {
00126:             _powerGrid = powerGrid ?? throw new ArgumentNullException(nameof(powerGrid));
00127:             _log = log ?? NullLog.Instance;
00128:             _catalog["default"] = new ScheduleDefinition
00129:             {
00130:                 schedule_id = "default",
00131:                 display_name = "Default Schedule",
00132:                 allowEmergencyOverride = true,
00133:                 fatigueRecoveryModifier = 1f,
00134:                 lightingDemandDay = 0.5f,
00135:                 lightingDemandNight = 0.8f,
00136:                 lightingDemandCurfew = 0.3f
00137:             };
00138:         }
00139:
00140:         public void LoadCatalog(List<ScheduleDefinition> definitions)
00141:         {
00142:             if (definitions == null) return;
00143:             _catalog.Clear();
00144:             foreach (var def in definitions)
00145:                 if (!string.IsNullOrEmpty(def.schedule_id))
00146:                     _catalog[def.schedule_id] = def;
00147:         }
00148:
00149:         public ActionResult SetSchedule(string scheduleId)
00150:         {
00151:             if (!_catalog.TryGetValue(scheduleId, out var def))
00152:                 return ActionResult.Failed("unknown_schedule", "schedule.unknown");
00153:
00154:             _activeScheduleId = scheduleId;
00155:             _state.activeScheduleId = scheduleId;
00156:             _log.Info($"[Schedule] switched to {def.display_name}");
00157:             OnScheduleChanged?.Invoke();
00158:             return ActionResult.Success("schedule.set");
00159:         }
00160:
00161:         public ActionResult SetCurfew(bool active)
00162:         {
00163:             _state.curfewActive = active;
00164:             UpdatePhase();
00165:             OnScheduleChanged?.Invoke();
00166:             return ActionResult.Success("schedule.curfew_set",
00167:                 new Dictionary<string, double> { { "curfew", active ? 1 : 0 } });
00168:         }
00169:
00170:         public ActionResult SetEmergencyOverride(bool active)
00171:         {
00172:             if (!_catalog.TryGetValue(_activeScheduleId, out var def))
00173:                 return ActionResult.Failed("no_schedule", "schedule.no_schedule");
00174:
00175:             if (active && !def.allowEmergencyOverride)
00176:                 return ActionResult.Blocked("not_allowed", "schedule.emergency_not_allowed");
00177:
00178:             _state.emergencyOverride = active;
00179:             UpdatePhase();
00180:             OnScheduleChanged?.Invoke();
00181:             return ActionResult.Success("schedule.emergency_set",
00182:                 new Dictionary<string, double> { { "emergency", active ? 1 : 0 } });
00183:         }
00184:
00185:         public ActionResult AssignBed(string survivorId, string bedId)
00186:         {
00187:             var existing = _state.assignments.Find(a => a.survivorId == survivorId);
00188:             if (existing != null)
00189:             {
00190:                 existing.bedId = bedId;
00191:                 existing.isAssigned = true;
00192:             }
00193:             else
00194:             {
00195:                 _state.assignments.Add(new SleepAssignment
00196:                 {
00197:                     survivorId = survivorId, bedId = bedId, isAssigned = true
00198:                 });
00199:             }
00200:             OnScheduleChanged?.Invoke();
00201:             return ActionResult.Success("schedule.bed_assigned");
00202:         }
00203:
00204:         public ActionResult UnassignBed(string survivorId)
00205:         {
00206:             var existing = _state.assignments.Find(a => a.survivorId == survivorId);
00207:             if (existing != null)
00208:             {
00209:                 existing.isAssigned = false;
00210:                 existing.bedId = string.Empty;
00211:             }
00212:             OnScheduleChanged?.Invoke();
00213:             return ActionResult.Success("schedule.bed_unassigned");
00214:         }
00215:
00216:         public void TickDay(int day)
00217:         {
00218:             if (_state.lastTransitionDay != day)
00219:             {
00220:                 _state.lastTransitionDay = day;
00221:                 UpdatePhase();
00222:             }
00223:
00224:             // Check compliance
00225:             foreach (var assignment in _state.assignments)
00226:             {
00227:                 if (!assignment.isAssigned) continue;
00228:                 assignment.isCompliant = _state.curfewActive;
00229:                 // Rest quality modifier
00230:                 assignment.restQuality = _state.emergencyOverride ? 0.5f : (_state.curfewActive ? 1.2f : 1f);
00231:             }
00232:
00233:             // Fatigue recovery modifier
00234:             if (_catalog.TryGetValue(_activeScheduleId, out var def))
00235:             {
00236:                 // Bug-07: the schedule's modifier applies across all phases;
00237:                 // emergency override is the only thing that overrides it.
00238:                 _state.fatigueRecoveryModifier = _state.emergencyOverride ? 0.5f : def.fatigueRecoveryModifier;
00239:                 _state.lightingDemand = _state.emergencyOverride ? def.lightingDemandCurfew * 0.5f :
00240:                     (_state.curfewActive ? def.lightingDemandCurfew : def.lightingDemandDay);
00241:
00242:                 // Bug-15: brownout halves the lighting demand *after* the
00243:                 // base setting is assigned. Previously this multiplicative
00244:                 // step ran first and was then unconditionally overwritten by
00245:                 // the assignment above, so a brownout had no effect on the
00246:                 // published lightingDemand value.
00247:                 if (_powerGrid.IsBrownout)
00248:                 {
00249:                     _state.lightingDemand *= 0.5f;
00250:                 }
00251:             }
00252:         }
00253:
00254:         public ScheduleDefinition? GetActiveSchedule()
00255:         {
00256:             _catalog.TryGetValue(_activeScheduleId, out var def);
00257:             return def;
00258:         }
00259:
00260:         public bool IsSleepEligible(string survivorId)
00261:         {
00262:             var assignment = _state.assignments.Find(a => a.survivorId == survivorId);
00263:             return assignment != null && assignment.isAssigned;
00264:         }
00265:
00266:         private void UpdatePhase()
00267:         {
00268:             SchedulePhase newPhase;
00269:             if (_state.emergencyOverride)
00270:                 newPhase = SchedulePhase.Emergency;
00271:             else if (_hourOfDay >= 0)
00272:                 // Plan 188 — the schedule owns phase; the hour comes from the
00273:                 // campaign ISimClock. This is what makes Night reachable.
00274:                 newPhase = PhaseForHour(_hourOfDay);
00275:             else if (_state.curfewActive)
00276:                 newPhase = SchedulePhase.Curfew;
00277:             else
00278:                 newPhase = SchedulePhase.Day;
00279:
00280:             if (newPhase != _state.currentPhase)
00281:             {
00282:                 _state.currentPhase = newPhase;
00283:                 OnPhaseChanged?.Invoke(newPhase);
00284:             }
00285:         }
00286:
00287:         /// <summary>
00288:         /// Plan 188 — pure hour→phase mapping from the active schedule's authored
00289:         /// windows. Night is the fallback outside the day and curfew windows, so
00290:         /// the phase is reachable without a second survivor scheduler.
00291:         /// </summary>
00292:         public SchedulePhase PhaseForHour(int hourOfDay)
00293:         {
00294:             if (_state.emergencyOverride) return SchedulePhase.Emergency;
00295:
00296:             int hour = ((hourOfDay % 24) + 24) % 24;
00297:             if (!_catalog.TryGetValue(_activeScheduleId, out var def))
00298:                 def = _catalog.TryGetValue("default", out var fallback) ? fallback : null;
00299:             if (def == null) return hour >= 6 && hour < 22 ? SchedulePhase.Day : SchedulePhase.Night;
00300:
00301:             int dayStart = (int)def.dayStartHour;
00302:             int dayEnd = (int)def.dayEndHour;
00303:             int curfewStart = (int)def.curfewStartHour;
00304:             int curfewEnd = (int)def.curfewEndHour;
00305:
00306:             if (InWindow(hour, curfewStart, curfewEnd)) return SchedulePhase.Curfew;
00307:             if (InWindow(hour, dayStart, dayEnd)) return SchedulePhase.Day;
00308:             return SchedulePhase.Night;
00309:         }
00310:
00311:         /// <summary>
00312:         /// Plan 188 — one schedule read of the campaign hour. Applies the derived
00313:         /// phase and its authored lighting demand (brownout still halves it).
00314:         /// It deliberately does not touch curfew compliance or assignments.
00315:         /// </summary>
00316:         public void TickHour(int hourOfDay)
00317:         {
00318:             _hourOfDay = ((hourOfDay % 24) + 24) % 24;
00319:             ApplyPhase(PhaseForHour(_hourOfDay));
00320:             ApplyLightingForPhase();
00321:         }
00322:
00323:         private void ApplyPhase(SchedulePhase phase)
00324:         {
00325:             if (phase == _state.currentPhase) return;
00326:             _state.currentPhase = phase;
00327:             OnPhaseChanged?.Invoke(phase);
00328:         }
00329:
00330:         private void ApplyLightingForPhase()
00331:         {
00332:             if (!_catalog.TryGetValue(_activeScheduleId, out var def)) return;
00333:             _state.lightingDemand = _state.emergencyOverride
00334:                 ? def.lightingDemandCurfew * 0.5f
00335:                 : _state.currentPhase switch
00336:                 {
00337:                     SchedulePhase.Night => def.lightingDemandNight,
00338:                     SchedulePhase.Curfew => def.lightingDemandCurfew,
00339:                     SchedulePhase.Day => def.lightingDemandDay,
00340:                     _ => def.lightingDemandCurfew
00341:                 };
00342:             if (_powerGrid.IsBrownout)
00343:                 _state.lightingDemand *= 0.5f;
00344:         }
00345:
00346:         /// <summary>Half-open window match; wraps when end is at or before start.</summary>
00347:         private static bool InWindow(int hour, int start, int end)
00348:         {
00349:             if (start == end) return false;
00350:             return start < end
00351:                 ? hour >= start && hour < end
00352:                 : hour >= start || hour < end;
00353:         }
00354:
00355:         public ShelterScheduleState CaptureState()
00356:         {
00357:             _state.activeScheduleId = _activeScheduleId;
00358:             return CloneState(_state);
00359:         }
00360:
00361:         public void RestoreState(ShelterScheduleState saved)
00362:         {
00363:             if (saved == null) return;
00364:             _state = CloneState(saved);
00365:             _activeScheduleId = string.IsNullOrEmpty(_state.activeScheduleId) ? "default" : _state.activeScheduleId;
00366:         }
00367:
00368:         private static ShelterScheduleState CloneState(ShelterScheduleState src)
00369:         {
00370:             if (src == null) return new ShelterScheduleState();
00371:             var s = new SystemTextJsonSerializer();
00372:             var json = s.Serialize(src);
00373:             return s.Deserialize<ShelterScheduleState>(json) ?? new ShelterScheduleState();
00374:         }
00375:     }
00376: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs`

### `Assets/Ashfall.Core/ShelterScheduleCatalogLoader.cs` — complete current file

- Size: 51 lines / 1775 bytes.
- SHA-256: `bdd89c824da3d489dce770b3b745df46ae5dcbc503ce1489726c05acfde9e63f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core
00006: {
00007:     /// <summary>Container shape for shelter_schedules.json (the authority).</summary>
00008:     [Serializable]
00009:     public sealed class ShelterScheduleCatalogContainer
00010:     {
00011:         public List<ScheduleDefinition> schedules = new List<ScheduleDefinition>();
00012:     }
00013:
00014:     /// <summary>
00015:     /// Loads shelter schedule definitions from JSON.
00016:     /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
00017:     /// </summary>
00018:     public static class ShelterScheduleCatalogLoader
00019:     {
00020:         public const string DefaultFileName = "shelter_schedules.json";
00021:
00022:         public static List<ScheduleDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00023:         {
00024:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00025:                 return new List<ScheduleDefinition>();
00026:
00027:             string path = fileIO.Combine(dataDir, DefaultFileName);
00028:             if (!fileIO.FileExists(path))
00029:                 return new List<ScheduleDefinition>();
00030:
00031:             string rawText = fileIO.ReadAllText(path);
00032:             if (string.IsNullOrWhiteSpace(rawText))
00033:                 return new List<ScheduleDefinition>();
00034:
00035:             var container = json.Deserialize<ShelterScheduleCatalogContainer>(rawText);
00036:             return container?.schedules ?? new List<ScheduleDefinition>();
00037:         }
00038:
00039:         public static int LoadAndRegister(
00040:             ShelterScheduleSystem system,
00041:             string dataDir,
00042:             IFileIO fileIO,
00043:             IJsonSerializer json)
00044:         {
00045:             if (system == null) return 0;
00046:             var defs = Load(dataDir, fileIO, json);
00047:             system.LoadCatalog(defs);
00048:             return defs.Count;
00049:         }
00050:     }
00051: }
```


# Appendix — Current Source Detail: `src/Host/ShelterScheduleHostSession.cs`

### `src/Host/ShelterScheduleHostSession.cs` — complete current file

- Size: 111 lines / 3763 bytes.
- SHA-256: `ea5a50b5e4328d1907b3407e7dfbb235762887d5db58fc3d3e1075db96f53e2a`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Shelter;
00007:
00008: namespace AtomicWar.GodotApp
00009: {
00010:     /// <summary>
00011:     /// Host session for ShelterScheduleSystem.
00012:     /// Manages shelter work shifts, sleep assignments, curfews, lighting demand, and emergency overrides.
00013:     /// </summary>
00014:     public sealed class ShelterScheduleHostSession
00015:     : HostSessionBase{
00016:         public ShelterScheduleSystem System { get; }
00017:         public string LastEvent { get; private set; } = string.Empty;
00018:         public ShelterScheduleHostSession(ShelterScheduleSystem system)
00019:         {
00020:             if (system == null)
00021:             {
00022:                 var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
00023:                 var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_main", "Main Vault", 100f) };
00024:                 var grid = new PowerGridSystem(state, rooms, new SeededRng(1986));
00025:                 system = new ShelterScheduleSystem(grid, new GodotLog());
00026:             }
00027:             System = system;
00028:
00029:             System.OnPhaseChanged += phase =>
00030:             {
00031:                 LastEvent = $"[Schedule] Phase changed to {phase}";
00032:                 RaiseStateChanged();
00033:             };
00034:
00035:             System.OnScheduleChanged += () =>
00036:             {
00037:                 RaiseStateChanged();
00038:             };
00039:         }
00040:
00041:         public ActionResult SetCurfew(bool active)
00042:         {
00043:             var res = System.SetCurfew(active);
00044:             if (res.IsSuccess)
00045:             {
00046:                 LastEvent = $"Shelter curfew set to: {(active ? "ACTIVE" : "INACTIVE")}";
00047:                 RaiseStateChanged();
00048:             }
00049:             return res;
00050:         }
00051:
00052:         public ActionResult SetEmergencyOverride(bool active)
00053:         {
00054:             var res = System.SetEmergencyOverride(active);
00055:             if (res.IsSuccess)
00056:             {
00057:                 LastEvent = $"Emergency schedule override set to: {(active ? "ACTIVE" : "OFF")}";
00058:                 RaiseStateChanged();
00059:             }
00060:             return res;
00061:         }
00062:
00063:         public ActionResult AssignBed(string survivorId, string bedId)
00064:         {
00065:             var res = System.AssignBed(survivorId, bedId);
00066:             if (res.IsSuccess)
00067:             {
00068:                 LastEvent = $"Assigned dweller {survivorId} to bunk {bedId}";
00069:                 RaiseStateChanged();
00070:             }
00071:             return res;
00072:         }
00073:
00074:         /// <summary>Load the shelter_schedules.json catalog into the Core system (the authority).</summary>
00075:         public void LoadCatalog(string dataDir)
00076:         {
00077:             if (string.IsNullOrEmpty(dataDir)) return;
00078:             var fileIO = new FileSystemIO();
00079:             var serializer = new SystemTextJsonSerializer();
00080:             int count = ShelterScheduleCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
00081:             if (count > 0)
00082:             {
00083:                 LastEvent = $"Shelter schedule catalog loaded: {count} schedules";
00084:                 RaiseStateChanged();
00085:             }
00086:         }
00087:
00088:         public void TickDay(int day)
00089:         {
00090:             System.TickDay(day);
00091:             RaiseStateChanged();
00092:         }
00093:
00094:         /// <summary>
00095:         /// Plan 188 — feed the campaign hour so the schedule derives Day / Night /
00096:         /// Curfew from its authored windows. Presentation refresh only.
00097:         /// </summary>
00098:         public void TickHour(int hourOfDay)
00099:         {
00100:             System.TickHour(hourOfDay);
00101:             RaiseStateChanged();
00102:         }
00103:
00104:         public override void Save()
00105:         {
00106:             if (!IsDirty) return;
00107:             ShelterScheduleSaveStore.TrySave(System.CaptureState());
00108:             base.Save();
00109:         }
00110:     }
00111: }
```


# Appendix — Current Source Detail: `src/Main.ShelterInfrastructure.cs`

### `src/Main.ShelterInfrastructure.cs` — bounded current excerpt (592 of 645 lines)

- Size: 645 lines / 31304 bytes.
- SHA-256: `5ec59e6a0a93e8e5c67bc7a39c91ff4cca74a518400fd09468938167c23630ea`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Inventory;
00007: using Ashfall.Core.Medical;
00008: using Ashfall.Core.Radiation;
00009: using Ashfall.Core.Shelter;
00010: using Ashfall.Core.StartingLevel;
00011: using Ashfall.Core.Survivors;
00012: using Ashfall.Core.YearOfAsh;
00013: using Ashfall.Core.World;
00014: using Ashfall.Core.Crafting;
00015: using Ashfall.Core.Journal;
00016: using Ashfall.Core.Expeditions;
00017: using Ashfall.Core.Waystation;
00018: using AtomicWar.GodotApp.UI;
00019:
00020: namespace AtomicWar.GodotApp
00021: {
00022:     public partial class Main : Control
00023:     {
00024:         private WaterTreatmentHostSession _waterTreatment = null!;
00025:         private WaterTreatmentPanel _waterTreatmentPanel = null!;
00026:         private AirlockSecurityHostSession _airlockSecurity = null!;
00027:         private AirlockSecurityPanel _airlockSecurityPanel = null!;
00028:         private bool _airlockSecurityDirty;
00029:         private ShelterThermalHostSession _shelterThermal = null!;
00030:         private ShelterThermalPanel _shelterThermalPanel = null!;
00031:         private bool _shelterThermalDirty;
00032:         private WeatherHardeningHostSession _weatherHardening = null!;
00033:         private bool _weatherHardeningDirty;
00034:         private GeothermalAquiferHostSession _geothermalAquifer = null!;
00035:         private bool _geothermalAquiferDirty;
00036:         private Ashfall.Core.VentilationSystem _ventilation = null!; // Plan 29 29B: machine tell readings
00037:         private VentilationHostSession? _ventilationHost;                    // Plan 72 stage console session
00038:         private Ashfall.Core.Shelter.ShelterFireHazardSystem? _stageFireHazard; // Plan 72 arc-fault fire handoff
00039:         private Ashfall.Core.Shelter.ShelterFireHazardSystem? _shelterFireHazard;
00040:         private ShelterFireHostSession? _shelterFireSession;
00041:
00042:         public Ashfall.Core.Shelter.ShelterFireHazardSystem ShelterFireHazard => GetShelterFireHazardSystem();
00043:         public ShelterFireHostSession? ShelterFireSession => _shelterFireSession;
00044:         private ShelterScheduleHostSession _shelterSchedule = null!;
00045:         private ShelterSchedulePanel _shelterSchedulePanel = null!;
00046:         private bool _shelterScheduleDirty;
00047:         private AutopsyHostSession _autopsy = null!;
00048:         private AutopsyReportPanel _autopsyReportPanel = null!;
00049:         private bool _autopsyDirty;
00050:         private WaystationHostSession _waystation = null!;
00051:         private WaystationNetworkPanel _waystationPanel = null!;
00052:         private bool _waystationDirty;
00053:
00054:         // Plan 29 Task 29A — shelter room identity overlay (read-only data projection,
00055:         // loaded once; no condition state, no save section of its own).
00056:         private ShelterRoomIdentityCatalog? _shelterRoomIdentity;
00057:
00058:         /// <summary>Lazy-load the room identity catalog from the data authority. Missing file → empty catalog (overlay, never a dependency).</summary>
00059:         private ShelterRoomIdentityCatalog? GetShelterRoomIdentityCatalog()
00060:         {
00061:             if (_shelterRoomIdentity != null) return _shelterRoomIdentity;
00062:             _shelterRoomIdentity = ShelterRoomIdentityCatalog.Load(
00063:                 new FileSystemIO(), new SystemTextJsonSerializer(), _dataDir);
00064:             return _shelterRoomIdentity;
00065:         }
00066:
00067:         // Plan 29 Task 29B — machine tell catalog (read-only data projection, loaded once).
00068:         private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;
00069:
00070:         private Ashfall.Core.Shelter.ShelterMachineTellCatalog GetMachineTellCatalog()
00071:         {
00072:             if (_machineTellCatalog != null) return _machineTellCatalog;
00073:             _machineTellCatalog = Ashfall.Core.Shelter.ShelterMachineTellCatalog.Load(
00074:                 new FileSystemIO(), new SystemTextJsonSerializer(), _dataDir);
00075:             return _machineTellCatalog;
00076:         }
00077:
00078:         // Plan 145 — bunker graffiti catalog (read-only ambient storytelling projection, loaded once).
00079:         private Ashfall.Core.Narrative.BunkerGraffitiCatalog? _bunkerGraffitiCatalog;
00080:
00081:         public Ashfall.Core.Narrative.BunkerGraffitiCatalog GetBunkerGraffitiCatalog()
00082:         {
00083:             if (_bunkerGraffitiCatalog != null) return _bunkerGraffitiCatalog;
00084:             _bunkerGraffitiCatalog = Ashfall.Core.Narrative.BunkerGraffitiCatalog.LoadFromDirectory(
00085:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00086:             return _bunkerGraffitiCatalog;
00087:         }
00088:
00089:         // Plan 146 — bunker court catalog (read-only historical tribunal records, loaded once).
00090:         private Ashfall.Core.Narrative.BunkerCourtCatalog? _bunkerCourtCatalog;
00091:
00092:         public Ashfall.Core.Narrative.BunkerCourtCatalog GetBunkerCourtCatalog()
00093:         {
00094:             if (_bunkerCourtCatalog != null) return _bunkerCourtCatalog;
00095:             _bunkerCourtCatalog = Ashfall.Core.Narrative.BunkerCourtCatalog.LoadFromDirectory(
00096:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00097:             return _bunkerCourtCatalog;
00098:         }
00099:
00100:         // Plan 148 — bunker maintenance catalog (read-only engineering emergency & glitch records, loaded once).
00101:         private Ashfall.Core.Narrative.BunkerMaintenanceCatalog? _bunkerMaintenanceCatalog;
00102:
00103:         public Ashfall.Core.Narrative.BunkerMaintenanceCatalog GetBunkerMaintenanceCatalog()
00104:         {
00105:             if (_bunkerMaintenanceCatalog != null) return _bunkerMaintenanceCatalog;
00106:             _bunkerMaintenanceCatalog = Ashfall.Core.Narrative.BunkerMaintenanceCatalog.LoadFromDirectory(
00107:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00108:             return _bunkerMaintenanceCatalog;
00109:         }
00110:
00111:         // Plan 150 — personal letter catalog (read-only personal & unsent correspondence, loaded once).
00112:         private Ashfall.Core.Narrative.PersonalLetterCatalog? _personalLetterCatalog;
00113:
00114:         public Ashfall.Core.Narrative.PersonalLetterCatalog GetPersonalLetterCatalog()
00115:         {
00116:             if (_personalLetterCatalog != null) return _personalLetterCatalog;
00117:             _personalLetterCatalog = Ashfall.Core.Narrative.PersonalLetterCatalog.LoadFromDirectory(
00118:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00119:             return _personalLetterCatalog;
00120:         }
00121:
00122:         // Plan 151 — abyssal anomalies science archive catalog (read-only science/anomaly logs, loaded once).
00123:         private Ashfall.Core.Narrative.AbyssalAnomaliesCatalog? _abyssalAnomaliesCatalog;
00124:
00125:         public Ashfall.Core.Narrative.AbyssalAnomaliesCatalog GetAbyssalAnomaliesCatalog()
00126:         {
00127:             if (_abyssalAnomaliesCatalog != null) return _abyssalAnomaliesCatalog;
00128:             _abyssalAnomaliesCatalog = Ashfall.Core.Narrative.AbyssalAnomaliesCatalog.LoadFromDirectory(
00129:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00130:             return _abyssalAnomaliesCatalog;
00131:         }
00132:
00133:         /// <summary>
00134:         /// Plan 29 29A: a shelter room hotspot was clicked — treat it as inspection.
00135:         /// Marks the authoritative Day-1 roster inspection (legacy ids tolerated via
00136:         /// the catalog alias map) and unlocks inspect_room vignettes through the
00137:         /// JournalSystem knowledge key (journal save owns persistence; old saves
00138:         /// simply default locked and unlock on the next inspection).
00139:         /// </summary>
00140:         private void HandleShelterRoomSelected(string roomId)
00141:         {
00142:             if (string.IsNullOrEmpty(roomId)) return;
00143:             var catalog = GetShelterRoomIdentityCatalog();
00144:             string canonical = catalog?.ResolveRoomId(roomId) ?? roomId;
00145:
00146:             if (_startingLevel != null && !_startingLevel.System.InspectRoom(canonical))
00147:             {
00148:                 var aliases = catalog?.GetLegacyAliases(canonical);
00149:                 if (aliases != null)
00150:                 {
00151:                     for (int i = 0; i < aliases.Count; i++)
00152:                         if (_startingLevel.System.InspectRoom(aliases[i])) break;
00154:             }
00155:
00156:             UnlockRoomHistories(catalog,
00157:                 catalog?.GetUnlockableVignettes(canonical,
00158:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
00159:
00160:             // Plans 150/151/153 — real room inspection unlocks producer-bound
00161:             // narrative records (not panel-open dumps).
00162:             SetupJournal();
00163:             DiscoverPersonalLetterRecords(canonical);
00164:             DiscoverAbyssalAnomalyRecords(canonical);
00165:             DiscoverFringeCultRecords(canonical);
00166:             DiscoverPaperPrintingRecords(canonical);
00167:             DiscoverBoneHornRecords(canonical);
00168:         }
00169:
00170:         /// <summary>
00171:         /// Plan 29 29A: a real repair/maintenance action completed in a shelter room
00172:         /// (filter service/replace). Raises the repair_performed unlock path only —
00173:         /// authored vignettes never fire from a decorative interaction.
00174:         /// </summary>
00175:         private void HandleShelterRoomRepairPerformed(string roomId)
00176:         {
00177:             if (string.IsNullOrEmpty(roomId)) return;
00178:             var catalog = GetShelterRoomIdentityCatalog();
00179:             UnlockRoomHistories(catalog,
00180:                 catalog?.GetUnlockableVignettes(catalog.ResolveRoomId(roomId),
00181:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed));
00182:         }
00183:
00184:         /// <summary>
00185:         /// Plan 29 29A: daily milestone pass. Runs once per campaign day from the
00186:         /// day coordinator (never per frame) and unlocks at most the vignettes whose
00187:         /// required day has been reached. Journal keys make it idempotent, so a late
00188:         /// load of an older save catches up once rather than spamming every tick.
00189:         /// </summary>
00190:         private void TickShelterRoomHistoryMilestones(int day)
00191:         {
00192:             var catalog = GetShelterRoomIdentityCatalog();
00193:             if (catalog == null) return;
00194:             UnlockRoomHistories(catalog, catalog.GetDayMilestoneVignettes(day));
00195:
00196:             // Plan 29 29B: daily machine glitch pass — journal one-shots, evaluate continuous.
00197:             TickMachineGlitchEvents(day);
00198:
00199:             // Plan 29 §29B.21: machine tell audio — quirk cues start on threshold
00200:             // crossings and stop on recovery; personality beds sustain.
00201:             TickMachineTellAudio();
00202:         }
00203:
00204:         /// <summary>
00205:         /// Plan 29 §29B.21 consumer side: daily machine tell audio sync. Evaluates
00206:         /// the same readings the text tells use and diffs the fired quirks against
00207:         /// the live audio conditions — newly degraded tells start their ElevenLabs
00208:         /// cue, recovered tells stop it, personality beds stay continuous. The
00209:         /// condition system's already_active guard makes repeated applies no-ops,
00210:         /// so audio fires on threshold transitions (§14), never per frame. No new
00211:         /// state authority: tells re-derive from the owning systems' live condition.
00212:         /// </summary>
00213:         private void TickMachineTellAudio()
00214:         {
00215:             var catalog = GetMachineTellCatalog();
00216:             if (catalog == null || catalog.MachineCount == 0) return;
00217:
00218:             var readings = BuildMachineReadings();
00219:             if (readings == null) return;
00220:
00221:             Ashfall.Core.Shelter.MachineTellAudioSync.Apply(
00222:                 catalog, readings, _audioConditions,
00223:                 cueId => AtomicWar.GodotApp.Audio.AudioCueCatalog.Resolve(cueId)?.Loop ?? false);
00224:         }
00225:
00226:         /// <summary>Apply an unlock batch through the journal (the single persistence authority).</summary>
00227:         private void UnlockRoomHistories(ShelterRoomIdentityCatalog? catalog,
00228:             System.Collections.Generic.IReadOnlyList<RoomHistoryVignette>? vignettes)
00229:         {
00230:             if (catalog == null || vignettes == null || _journal == null) return;
00231:             for (int i = 0; i < vignettes.Count; i++)
00232:             {
00233:                 if (_journal.UnlockRoomHistorySeen(vignettes[i].id))
00234:                     _journalDirty = true;
00237:
00238:         /// <summary>
00239:         /// Plan 29 29B: daily machine glitch pass. Journals one-shot glitches (idempotent
00240:         /// via journal keys) and evaluates continuous glitches for UI surfacing. Old saves
00241:         /// default un-noted and reveal once; continuous events re-fire on their cooldown,
00242:         /// paced by the caller's day bookkeeping.
00243:         /// </summary>
00244:         private void TickMachineGlitchEvents(int day)
00245:         {
00246:             var catalog = GetMachineTellCatalog();
00247:             if (catalog == null || catalog.GlitchEvents.Count == 0 || _journal == null) return;
00248:
00249:             var readings = BuildMachineReadings();
00250:             if (readings == null) return;
00251:
00252:             bool isNoted(string id) => _journal.IsGlitchNoted(id);
00253:             for (int m = 0; m < catalog.MachineCount; m++)
00254:             {
00255:                 string mid = catalog.Machines[m].id;
00256:                 var glitches = catalog.EvaluateGlitchEvents(mid, readings, isNoted);
00257:                 for (int g = 0; g < glitches.Count; g++)
00258:                 {
00259:                     var gl = glitches[g];
00260:                     if (string.Equals(gl.repeat_policy, "once", System.StringComparison.Ordinal))
00261:                     {
00262:                         _journal.UnlockGlitchNoted(gl.id);
00263:                     }
00264:                 }
00266:         }
00267:
00268:         /// <summary>Build MachineConditionReadings from live host systems for tell evaluation.</summary>
00269:         private Ashfall.Core.Shelter.MachineConditionReadings? BuildMachineReadings()
00270:         {
00271:             try
00272:             {
00273:                 return new Ashfall.Core.Shelter.MachineConditionReadings
00274:                 {
00275:                     HepaFilterHealth = (float)Math.Clamp(_startingLevel?.System.State.airFilterHealthPercent ?? 100, 0, 100),
00276:                     HepaRadon = (float)Math.Clamp(_startingLevel?.System.State.radonLevelBqm3 ?? 12, 0, 200),
00277:                     PowerFuelUnits = (float)Math.Clamp(_powerGrid?.System.State.FuelUnits ?? 0, 0, 200),
00278:                     PowerBatteryReserve = _powerGrid != null ? (_powerGrid.System.State.BatteryReserveWh / 4000f * 100f) : 100f,
00279:                     VentilationFilterSaturation = (float)Math.Clamp(_ventilation?.FilterSaturation ?? 0, 0, 100),
00280:                     WaterFilterIntegrity = (float)Math.Clamp(_waterTreatment?.System.FilterIntegrity ?? 100, 0, 100),
00281:                     ThermalBoilerFuel = (float)Math.Clamp(_shelterThermal?.System.BoilerFuelLevel ?? 0, 0, 200),
00282:                     AirlockIncidentActive = _airlockSecurity?.System.HasPendingIncident ?? false,
00283:                     HazardWeather = _world?.Weather.Current is Ashfall.Core.WeatherKind.FalloutStorm or Ashfall.Core.WeatherKind.BlackRain or Ashfall.Core.WeatherKind.Ashfall
00290:         }
00291:
00292:         /// <summary>Build a one-line dashboard tell string from live machine readings (§29B.9–29B.13).</summary>
00293:         public string BuildMachineTellText(ISeededRng? rng = null)
00294:         {
00295:             try
00296:             {
00297:                 var catalog = GetMachineTellCatalog();
00298:                 if (catalog == null || catalog.MachineCount == 0) return string.Empty;
00299:
00300:                 var readings = BuildMachineReadings();
00301:                 if (readings == null) return string.Empty;
00302:
00303:                 var fired = new System.Collections.Generic.List<string>();
00304:                 bool isNoted(string id) => _journal != null && _journal.IsGlitchNoted(id);
00305:                 for (int m = 0; m < catalog.MachineCount; m++)
00306:                 {
00307:                     string mid = catalog.Machines[m].id;
00308:                     string label = catalog.Machines[m].display_name;
00309:                     if (string.IsNullOrWhiteSpace(label))
00310:                     {
00311:                         label = mid;
00312:                         if (label.StartsWith("machine_", StringComparison.Ordinal))
00314:                     }
00315:                     // Shorten to a readable tag: "Main Generator & Battery Bank" → "Generator"
00316:                     if (label.Contains("&", StringComparison.Ordinal))
00317:                         label = label.Split('&')[0].Trim();
00318:                     label = label.Replace("Filtration Stack", "HEPA").Replace("Exhaust Plant", "Ventilation").Replace("Brine Still", "Still").Replace("Shelter ", "").Replace("Airlock Machinery", "Airlock");
00319:                     label = label.ToUpperInvariant();
00320:
00321:                     var quirks = catalog.EvaluateQuirks(mid, readings);
00322:                     for (int q = 0; q < quirks.Count; q++)
00323:                     {
00324:                         var qk = quirks[q];
00325:                         if (string.Equals(qk.kind, "diagnostic", System.StringComparison.Ordinal))
00327:                     }
00328:
00329:                     var glitches = catalog.EvaluateGlitchEvents(mid, readings, isNoted);
00330:                     for (int g = 0; g < glitches.Count; g++)
00331:                     {
00332:                         var gl = glitches[g];
00333:                         fired.Add($"[{label}] {gl.title}");
00334:                         if (string.Equals(gl.repeat_policy, "once", System.StringComparison.Ordinal) && _journal != null)
00335:                             _journal.UnlockGlitchNoted(gl.id);
00336:                     }
00337:                 }
00338:
00351:             if (_waterTreatment != null) return;
00352:             SetupInventory();
00353:             var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
00354:             var wtSys = new WaterTreatmentSystem(new GodotLog());
00355:             wtSys.RestoreState(wtState);
00356:             _waterTreatment = new WaterTreatmentHostSession(wtSys, _inventory);
00357:             // CORE-MECH W3: winter pressure on the water owner. One read-only
00358:             // day → multiplier view of the Year-of-Ash calendar, applied once at
00359:             // the owner's single filter-degradation site (AA.3). Fail-closed to
00360:             // neutral when the calendar cannot be read.
00361:             try
00362:             {
00363:                 var yoaEvents = YearOfAshCatalogLoader.LoadEvents(
00364:                     CatalogPath.ResolveDataDir(),
00365:                     CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir()),
00366:                     new SystemTextJsonSerializer());
00367:                 var pressure = new SeasonalPressureProvider(yoaEvents);
00368:                 wtSys.SeasonalFilterLoadMultiplier = day => pressure.MultiplierFor(day);
00369:             }
00370:             catch (Exception ex)
00371:             {
00372:                 GD.Print("[Ashfall Godot] Seasonal water pressure unavailable: " + ex.Message);
00373:             }
00374:             _waterTreatment.OnTreatmentStarted += () => ObserveSigil("water.treatment_started");
00375:             // B5–B8 expansion (§9.12): unsafe-water exposure → the canonical
00376:             // disease sweep. WaterborneExposureRules owns the dose→disease
00377:             // mapping (Core-pure); DiseaseSystem owns the outcome roll; the
00378:             // roster only supplies the exposed population.
00379:             _waterTreatment.PathogenExposureSink = dose =>
00380:             {
00381:                 if (_disease?.Engine == null
00382:                     || !Ashfall.Core.WaterborneExposureRules.ShouldRunExposureSweep(dose))
00388:                              ?? new List<Ashfall.Core.Survivors.SurvivorRosterEntry>())
00389:                     {
00390:                         if (entry == null || !entry.isAlive) continue;
00391:                         _disease.Engine.TryExpose(new Ashfall.Core.Disease.DiseaseExposureContext
00392:                         {
00393:                             SurvivorId = entry.survivorId,
00394:                             DiseaseId = diseaseId,
00395:                             SourceId = Ashfall.Core.WaterborneExposureRules.SourceId,
00408:         }
00409:
00410:         private void SaveWaterTreatment()
00411:         {
00412:             if (_waterTreatment != null)
00413:                 CaptureSection("water_treatment", WaterTreatmentSaveStore.TryCapturePersisted(_waterTreatment.System.CaptureState()));
00414:         }
00415:
00416:         private void SetupAirlockSecurity()
00417:         {
00418:             if (_airlockSecurity != null) return;
00419:             var asState = AirlockSecuritySaveStore.TryLoad() ?? new AirlockSecurityState();
00420:             var asSys = new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());
00421:             asSys.RestoreState(asState);
00422:             _airlockSecurity = new AirlockSecurityHostSession(asSys);
00423:             if (_airlockSecurityPanel != null && _airlockSecurityPanel.IsInsideTree())
00424:                 RemoveChild(_airlockSecurityPanel);
00425:             _airlockSecurityPanel = new AirlockSecurityPanel();
00426:             _airlockSecurityPanel.Bind(_airlockSecurity);
00429:         }
00430:
00431:         private void SaveAirlockSecurity()
00432:         {
00433:             if (_airlockSecurity != null)
00434:                 CaptureSection("airlock_security", AirlockSecuritySaveStore.TryCapturePersisted(_airlockSecurity.System.CaptureState()));
00435:         }
00436:
00437:         private void SetupShelterThermal()
00438:         {
00439:             if (_shelterThermal != null) return;
00440:             var stState = ShelterThermalSaveStore.TryLoad() ?? new ShelterThermalState();
00441:             var stNeeds = _survivors.Needs;
00442:             var stStarting = _startingLevel.System;
00443:             var stDeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
00444:             var stSys = new ShelterThermalSystem(new SeededRng(1986), stNeeds, stStarting, stDeepFreeze, new GodotLog());
00445:             stSys.RestoreState(stState);
00446:             _shelterThermal = new ShelterThermalHostSession(stSys);
00447:             if (_shelterThermalPanel != null && _shelterThermalPanel.IsInsideTree())
00448:                 RemoveChild(_shelterThermalPanel);
00449:             _shelterThermalPanel = new ShelterThermalPanel();
00450:             _shelterThermalPanel.Bind(_shelterThermal);
00453:         }
00454:
00455:         private void SaveShelterThermal()
00456:         {
00457:             if (_shelterThermal != null)
00458:                 CaptureSection("shelter_thermal", ShelterThermalSaveStore.TryCapturePersisted(_shelterThermal.System.CaptureState()));
00459:         }
00460:
00461:         private void SetupWeatherHardening()
00462:         {
00463:             if (_weatherHardening != null) return;
00464:             var whState = WeatherHardeningSaveStore.TryLoad() ?? new WeatherHardeningState();
00465:             var whSys = new WeatherHardeningSystem(
00466:                 whState,
00467:                 new SeededRng(1999),
00468:                 new GodotLog(),
00472:                 _waterTreatment?.System,
00473:                 _inventory?.Inventory);
00474:             _weatherHardening = new WeatherHardeningHostSession(whSys);
00475:             _weatherHardening.LoadCatalog(_dataDir);
00476:         }
00477:
00478:         private void SaveWeatherHardening()
00479:         {
00480:             if (_weatherHardening != null)
00481:                 CaptureSection("weather_hardening", WeatherHardeningSaveStore.TryCapturePersisted(_weatherHardening.System.CaptureState()));
00482:         }
00483:
00484:         private void SetupGeothermalAquifer()
00485:         {
00486:             if (_geothermalAquifer != null) return;
00487:             var state = GeothermalAquiferSaveStore.TryLoad() ?? new GeothermalAquiferState();
00488:             var system = new GeothermalAquiferSystem(
00489:                 state,
00490:                 new SeededRng(2003),
00491:                 new GodotLog(),
00493:                 _waterTreatment?.System,
00494:                 _inventory?.Inventory);
00495:             _geothermalAquifer = new GeothermalAquiferHostSession(system);
00496:             _geothermalAquifer.LoadCatalog(_dataDir);
00497:             if (_geothermalAquiferPanel != null)
00498:                 _geothermalAquiferPanel.Bind(_geothermalAquifer);
00499:         }
00500:
00501:         private void SaveGeothermalAquifer()
00502:         {
00503:             if (_geothermalAquifer != null)
00504:                 CaptureSection("geothermal_aquifer", GeothermalAquiferSaveStore.TryCapturePersisted(_geothermalAquifer.System.CaptureState()));
00505:         }
00506:
00507:         private void SetupShelterSchedule()
00508:         {
00509:             if (_shelterSchedule != null) return;
00510:             var ssState = ShelterScheduleSaveStore.TryLoad() ?? new ShelterScheduleState();
00511:             var ssPower = _powerGrid.System;
00512:             var ssSys = new ShelterScheduleSystem(ssPower, new GodotLog());
00513:             ssSys.RestoreState(ssState);
00514:             _shelterSchedule = new ShelterScheduleHostSession(ssSys);
00515:             _shelterSchedule.LoadCatalog(_dataDir);
00516:             if (_shelterSchedulePanel != null && _shelterSchedulePanel.IsInsideTree())
00517:                 RemoveChild(_shelterSchedulePanel);
00518:             _shelterSchedulePanel = new ShelterSchedulePanel();
00519:             _shelterSchedulePanel.Bind(_shelterSchedule);
00522:         }
00523:
00524:         private void SaveShelterSchedule()
00525:         {
00526:             if (_shelterSchedule != null)
00527:                 CaptureSection("shelter_schedule", ShelterScheduleSaveStore.TryCapturePersisted(_shelterSchedule.System.CaptureState()));
00528:         }
00529:
00530:         private void SetupAutopsy(ResearchSystem? sharedResearch = null)
00531:         {
00532:             if (_autopsy != null) return;
00533:             sharedResearch ??= _sharedResearch;
00534:             var auState = AutopsySaveStore.TryLoad() ?? new AutopsyState();
00535:             var auInv = _inventory.Inventory;
00536:             var auRad = _survivors.Radiation;
00537:             var auStarting = _startingLevel.System;
00538:             var auVent = new VentilationSystem(auStarting);
00539:             _ventilation = auVent; // Plan 29 29B: expose for machine tell readings
00540:             // Plan 72: electrostatic stage catalog + persistent arc-fire hazard.
00541:             auVent.ApplyElectrostaticCatalog(Ashfall.Core.ElectrostaticFiltrationCatalogLoader.Load(
00542:                 _dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
00543:             SetupShelterFireHazard();
00544:             _ventilationHost = new VentilationHostSession(auVent);
00545:             var auRes = sharedResearch;
00546:             var auMedical = _medicalWard;
00547:             var auSys = new AutopsySystem(new SeededRng(1986), auInv, auRad, auVent, auRes, auMedical, new GodotLog());
00548:             auSys.RestoreState(auState);
00549:             _autopsy = new AutopsyHostSession(auSys);
00550:             _autopsy.LoadCatalog(_dataDir);
00551:             if (_autopsyReportPanel != null && _autopsyReportPanel.IsInsideTree())
00552:                 RemoveChild(_autopsyReportPanel);
00553:             _autopsyReportPanel = new AutopsyReportPanel();
00554:             _autopsyReportPanel.Bind(_autopsy);
00557:         }
00558:
00559:         private void SaveAutopsy()
00560:         {
00561:             if (_autopsy != null)
00562:                 CaptureSection("autopsy", AutopsySaveStore.TryCapturePersisted(_autopsy.System.CaptureState()));
00563:         }
00564:
00565:         private void SetupWaystation()
00566:         {
00567:             if (_waystation != null) return;
00568:             var wsState = WaystationSaveStore.TryLoad() ?? new WaystationSystemState();
00569:             var wsSys = new WaystationSystem();
00570:             wsSys.RestoreState(wsState);
00571:             _waystation = new WaystationHostSession(wsSys);
00572:
00573:             // Plan 56 phase 6 — the multi-node trade-stock network: its 7-day
00574:             // resupply is provenance-aware (locally produced + general stock
00575:             // survive a market shortage; pure imports lapse). The shortage
00576:             // policy reads the live market; the closure is null-safe because
00577:             // the economy session may not be set up yet when it is bound.
00578:             SetupEconomy();
00579:             var network = new WaystationNetworkSystem();
00580:             if (wsState.network != null)
00581:                 network.RestoreState(wsState.network);
00582:             if (_economy?.Catalog != null)
00583:             {
00584:                 _waystation.AttachNetwork(
00585:                     network,
00586:                     _economy.Catalog,
00587:                     () => _economy?.Market.IsSuppliesShort() ?? false);
00588:             }
00589:             if (_waystationPanel != null && _waystationPanel.IsInsideTree())
00590:                 RemoveChild(_waystationPanel);
00591:             _waystationPanel = new WaystationNetworkPanel();
00592:             _waystationPanel.Bind(_waystation);
00593:             _waystationPanel.Visible = false;
00594:             AddChild(_waystationPanel);
00595:         }
00596:
00597:         private void SaveWaystation()
00598:         {
00599:             if (_waystation == null) return;
00600:             var state = _waystation.System.CaptureState();
00601:             if (_waystation.Network != null)
00602:                 state.network = _waystation.Network.CaptureState();
00603:             CaptureSection("waystation", WaystationSaveStore.TryCapturePersisted(state));
00604:         }
00605:
00606:         private bool _shelterFireDirty;
00607:
00608:         private void SetupShelterFireHazard()
00609:         {
00610:             if (_shelterFireHazard != null) return;
00611:             _shelterFireHazard = new Ashfall.Core.Shelter.ShelterFireHazardSystem();
00612:             _stageFireHazard = _shelterFireHazard;
00613:             _shelterFireSession = new ShelterFireHostSession(_shelterFireHazard);
00614:             _shelterFireSession.StateChanged += () => _shelterFireDirty = true;
00615:
00616:             var saved = ShelterFireSaveStore.TryLoad();
00617:             if (saved != null)
00618:                 ShelterFireSaveStore.ApplyToSystem(_shelterFireHazard, saved);
00619:         }
00620:
00621:         private void SaveShelterFire()
00622:         {
00623:             if (_shelterFireHazard == null) return;
00624:             if (CaptureSection(
00625:                     "shelter_fire",
00626:                     ShelterFireSaveStore.TryCapturePersisted(
00627:                         ShelterFireSaveStore.FromSystem(_shelterFireHazard))))
00628:             {
00629:                 _shelterFireDirty = false;
00630:             }
00631:         }
00632:
00633:         private void FlushShelterFireIfDirty()
00634:         {
00635:             if (_shelterFireDirty) SaveShelterFire();
00636:         }
00637:
00638:         public Ashfall.Core.Shelter.ShelterFireHazardSystem GetShelterFireHazardSystem()
00639:         {
00640:             if (_shelterFireHazard == null)
00641:                 SetupShelterFireHazard();
00642:             return _shelterFireHazard!;
00643:         }
00644:     }
00645: }
```


# Appendix — Current Source Detail: `src/UI/ShelterSchedulePanel.cs`

### `src/UI/ShelterSchedulePanel.cs` — complete current file

- Size: 126 lines / 4447 bytes.
- SHA-256: `53f4022b5ec4be0fa4d17af5476684c27fee47b456bc0c1e8340be2d359f8fcc`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Godot;
00004: using Ashfall.Core;
00005: using Ashfall.Core.UI;
00006: using AtomicWar.GodotApp;
00007: using DesignTheme = Ashfall.Core.UI.Theme;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     public partial class ShelterSchedulePanel : Control, IBindablePanel
00012:     {
00013:         public event Action? OnClose;
00014:
00015:         private AshfallDashboardShell _shell = null!;
00016:         private AshfallStatusRail? _statusRail;
00017:         private VBoxContainer _contentStack = null!;
00018:         private Label _detailText = null!;
00019:         private Button _curfewBtn = null!;
00020:         private Button _emergencyBtn = null!;
00021:
00022:         private ShelterScheduleHostSession? _host;
00023:
00024:         public bool IsBound => _host != null;
00025:
00026:         public void Bind(ShelterScheduleHostSession session)
00027:         {
00028:             _host = session;
00029:             if (_host != null)
00030:             {
00031:                 _host.StateChanged += RefreshView;
00032:             }
00033:             RefreshView();
00034:         }
00035:
00036:         public void Unbind()
00037:         {
00038:             if (_host != null)
00039:             {
00040:                 _host.StateChanged -= RefreshView;
00041:                 _host = null;
00042:             }
00043:         }
00044:
00045:
00046:
00047:         public override void _Ready()
00048:         {
00049:             SetAnchorsPreset(LayoutPreset.FullRect);
00050:
00051:             _shell = new AshfallDashboardShell("Shelter Schedule // Shift Assignment", minWidth: 1000, minHeight: 650);
00052:             AddChild(_shell);
00053:
00054:             _statusRail = _shell.SetStatusRail();
00055:             _statusRail.AddCard("phase", "Current Phase", "DAY", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00056:             _statusRail.AddCard("curfew", "Curfew", "INACTIVE", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00057:             _statusRail.AddCard("lighting", "Lighting Load", "50%", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00058:
00059:             _contentStack = new VBoxContainer();
00060:             _contentStack.AddThemeConstantOverride("separation", 12);
00061:             _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00062:             _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
00063:
00064:             _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
00065:             _contentStack.AddChild(_detailText);
00066:
00067:             var buttonRow = AshfallUiHelpers.MakeActionBar(separation: 10);
00068:
00069:             _curfewBtn = AshfallUiHelpers.MakeButton("Toggle Night Curfew", () =>
00070:             {
00071:                 if (_host != null)
00072:                 {
00073:                     bool active = !_host.System.State.curfewActive;
00074:                     _host.SetCurfew(active);
00075:                 }
00076:             });
00077:             _curfewBtn.CustomMinimumSize = new Vector2(180, 36);
00078:             buttonRow.AddChild(_curfewBtn);
00079:
00080:             _emergencyBtn = AshfallUiHelpers.MakeButton("Emergency Override", () =>
00081:             {
00082:                 if (_host != null)
00083:                 {
00084:                     bool active = !_host.System.State.emergencyOverride;
00085:                     _host.SetEmergencyOverride(active);
00086:                 }
00087:             });
00088:             _emergencyBtn.CustomMinimumSize = new Vector2(180, 36);
00089:             buttonRow.AddChild(_emergencyBtn);
00090:
00091:             _contentStack.AddChild(buttonRow);
00092:             _shell.SetContent(_contentStack);
00093:
00094:             _shell.AttachHeaderCloseButton("CLOSE", () =>
00095:             {
00096:                 Visible = false;
00097:                 OnClose?.Invoke();
00098:             });
00099:
00100:             RefreshView();
00101:         }
00102:
00103:         public void RefreshView()
00104:         {
00105:             if (_host == null || _statusRail == null) return;
00106:
00107:             var s = _host.System.State;
00108:             _statusRail.Set("phase", s.currentPhase.ToString().ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
00109:             _statusRail.Set("curfew", s.curfewActive ? "ACTIVE" : "INACTIVE", s.curfewActive ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
00110:             _statusRail.Set("lighting", $"{s.lightingDemand:P0}", AshfallMetricCard.Criticality.Normal);
00111:
00112:             if (_detailText != null)
00113:             {
00114:                 _detailText.Text = $"Shelter Schedule Phase: {s.currentPhase} | Fatigue Recovery Rate: {s.fatigueRecoveryModifier:P0}\n" +
00115:                                    $"Bunk Assignments: {s.assignments.Count} dwellers assigned\n" +
00116:                                    $"Last Event: {_host.LastEvent}";
00117:             }
00118:         }
00119:
00120:         public override void _ExitTree()
00121:         {
00122:             Unbind();
00123:             base._ExitTree();
00124:         }
00125:     }
00126: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs`

### `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` — complete current file

- Size: 229 lines / 9456 bytes.
- SHA-256: `3b010100ab97a56129db8dbeebd3b43a77c81dfdb5f7dd952193ac7a654fada1`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Shelter;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class ShelterScheduleSystemTests
00010:     {
00011:         [Fact] public void SetCurfew_ChangesPhase()
00012:         {
00013:             var s = Create(out _);
00014:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00015:             {
00016:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00017:             });
00018:             s.SetCurfew(true);
00019:             Assert.True(s.IsCurfewActive);
00020:         }
00021:
00022:         [Fact] public void SetCurfew_BackToDay()
00023:         {
00024:             var s = Create(out _);
00025:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00026:             {
00027:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00028:             });
00029:             s.SetCurfew(true);
00030:             s.SetCurfew(false);
00031:             Assert.False(s.IsCurfewActive);
00032:             Assert.Equal(SchedulePhase.Day, s.CurrentPhase);
00033:         }
00034:
00035:         [Fact] public void SetEmergencyOverride_ChangesPhase()
00036:         {
00037:             var s = Create(out _);
00038:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00039:             {
00040:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default", allowEmergencyOverride = true }
00041:             });
00042:             s.SetEmergencyOverride(true);
00043:             Assert.True(s.IsEmergencyOverride);
00044:             Assert.Equal(SchedulePhase.Emergency, s.CurrentPhase);
00045:         }
00046:
00047:         [Fact] public void AssignBed_CreatesAssignment()
00048:         {
00049:             var s = Create(out _);
00050:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00051:             {
00052:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00053:             });
00054:             var r = s.AssignBed("survivor_1", "bed_1");
00055:             Assert.Equal(ActionResult.StatusKind.Success, r.Status);
00056:             Assert.True(s.IsSleepEligible("survivor_1"));
00057:         }
00058:
00059:         [Fact] public void UnassignBed_RemovesAssignment()
00060:         {
00061:             var s = Create(out _);
00062:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00063:             {
00064:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00065:             });
00066:             s.AssignBed("survivor_1", "bed_1");
00067:             s.UnassignBed("survivor_1");
00068:             Assert.False(s.IsSleepEligible("survivor_1"));
00069:         }
00070:
00071:         [Fact] public void TickDay_SetsCompliance()
00072:         {
00073:             var s = Create(out _);
00074:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00075:             {
00076:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00077:             });
00078:             s.AssignBed("survivor_1", "bed_1");
00079:             s.SetCurfew(true);
00080:             s.TickDay(1);
00081:             Assert.True(s.State.assignments[0].isCompliant);
00082:             Assert.True(s.State.assignments[0].restQuality > 1f);
00083:         }
00084:
00085:         [Fact] public void EmergencyOverride_ReducesRecovery()
00086:         {
00087:             var s = Create(out _);
00088:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00089:             {
00090:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default", allowEmergencyOverride = true }
00091:             });
00092:             s.SetCurfew(true);
00093:             s.SetEmergencyOverride(true);
00094:             s.TickDay(1);
00095:             Assert.Equal(0.5f, s.FatigueRecoveryModifier);
00096:         }
00097:
00098:         [Fact] public void TickDay_Brownout_ReducesLighting()
00099:         {
00100:             var s = Create(out _);
00101:             s.TickDay(1);
00102:             Assert.True(s.State.lightingDemand > 0);
00103:         }
00104:
00105:         [Fact] public void SetSchedule_LoadsDefinition()
00106:         {
00107:             var s = Create(out _);
00108:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00109:             {
00110:                 new ScheduleDefinition { schedule_id = "strict", display_name = "Strict Curfew", curfewStartHour = 21f }
00111:             });
00112:             var r = s.SetSchedule("strict");
00113:             Assert.Equal(ActionResult.StatusKind.Success, r.Status);
00114:             var def = s.GetActiveSchedule();
00115:             Assert.Equal(21f, def.curfewStartHour);
00116:         }
00117:
00118:         [Fact] public void TickDay_DayPhase_UsesScheduleFatigueModifier()
00119:         {
00120:             // Bug-07 regression: a schedule with a non-default fatigue recovery
00121:             // modifier must apply that modifier during the day phase, not just
00122:             // the curfew phase. Previously the day branch hardcoded 1f.
00123:             var s = Create(out _);
00124:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00125:             {
00126:                 new ScheduleDefinition
00127:                 {
00128:                     schedule_id = "restful",
00129:                     display_name = "Restful",
00130:                     fatigueRecoveryModifier = 1.3f
00131:                 }
00132:             });
00133:             s.SetSchedule("restful");
00134:             // No curfew, no emergency — pure day phase.
00135:             s.TickDay(1);
00136:             Assert.Equal(1.3f, s.FatigueRecoveryModifier);
00137:         }
00138:
00139:         [Fact] public void TickDay_DayPhase_PropagatesRestlessSchedule()
00140:         {
00141:             // Bug-07 regression #2: a schedule with a SUPPRESSED recovery
00142:             // modifier (e.g. 0.7f) must also propagate during the day phase.
00143:             // Default behavior (1f) is the special case, not the general rule.
00144:             var s = Create(out _);
00145:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00146:             {
00147:                 new ScheduleDefinition
00148:                 {
00149:                     schedule_id = "restless",
00150:                     display_name = "Restless",
00151:                     fatigueRecoveryModifier = 0.7f
00152:                 }
00153:             });
00154:             s.SetSchedule("restless");
00155:             s.TickDay(2);
00156:             Assert.Equal(0.7f, s.FatigueRecoveryModifier);
00157:         }
00158:
00159:         // Bug-15 (brownout) has no dedicated regression test in this batch.
00160:         // Static-evidence rationale: the production fix in
00161:         // ShelterScheduleSystem.TickDay moves the brownout multiplier inside
00162:         // the if/else block so it runs AFTER the lightingDemand assignment,
00163:         // preserving the × 0.5 effect. Writing a deterministic brownout test
00164:         // would require controlling PowerGridSystem into a sustained brownout
00165:         // state, which is a separate upstream design issue (ComputeTotalDraw
00166:         // returns 0 under brownout, causing IsBrownout to flip on the same
00167:         // tick). That is not in scope for this batch.
00168:
00169:         [Fact] public void CaptureRestoreState_PreservesAssignments()
00170:         {
00171:             var s = Create(out _);
00172:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00173:             {
00174:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00175:             });
00176:             s.AssignBed("survivor_1", "bed_1");
00177:             s.SetCurfew(true);
00178:             var state = s.CaptureState();
00179:             Assert.Single(state.assignments);
00180:
00181:             var s2 = Create(out _);
00182:             s2.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00183:             {
00184:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00185:             });
00186:             s2.RestoreState(state);
00187:             Assert.Single(s2.State.assignments);
00188:             Assert.True(s2.IsSleepEligible("survivor_1"));
00189:         }
00190:
00191:         private static ShelterScheduleSystem Create(out PowerGridSystem power)
00192:         {
00193:             var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
00194:             var rooms = new System.Collections.Generic.List<PowerGridRoom>
00195:             {
00196:                 new PowerGridRoom("room_a", "Test Room", 100f)
00197:             };
00198:             power = new PowerGridSystem(state, rooms, new SeededRng(42));
00199:             return new ShelterScheduleSystem(power);
00200:         }
00201:
00202:         // Bug-15 deferred from Batch 2: brownout halves the schedule's daily
00203:         // lighting demand *after* the base assignment, so a brownout-shelter
00204:         // shows 0.25 ('day' baseline 0.5 halved) rather than 0.5.
00205:         [Fact] public void TickDay_Brownout_DoublesLightingDemandHalving()
00206:         {
00207:             var s = Create(out var power);
00208:             // Force a brownout: demand > generation, no battery reserve.
00209:             power.State.GenerationWatts = 50f;
00210:             power.State.BatteryReserveWh = 0f;
00211:             power.State.BatteryCapacityWh = 0f;
00212:
00213:             s.LoadCatalog(new System.Collections.Generic.List<ScheduleDefinition>
00214:             {
00215:                 new ScheduleDefinition { schedule_id = "default", display_name = "Default" }
00216:             });
00217:             float before = s.State.lightingDemand;
00218:             s.TickDay(1);
00219:             float after = s.State.lightingDemand;
00220:
00221:             // Baseline demand in day phase with default definitions is 0.5.
00222:             // Brownout applies ×0.5. Assertuion checks the multiplied value.
00223:             float expectedBase = 0.5f;
00224:             float expectedBrownout = expectedBase * 0.5f;
00225:             Assert.True(Math.Abs(after - expectedBrownout) < 0.001f,
00226:                 $"brownout lighting demand = {after}, expected {expectedBrownout}; pre={before}");
00227:         }
00228:     }
00229: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`

### `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — bounded current excerpt (1207 of 1415 lines)

- Size: 1415 lines / 62138 bytes.
- SHA-256: `01d76096907f04a268dd10572a9d6635286d47742922420c81e6c7863ae39f09`.
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
00009:     /// ASHFALL Power Grid — Core system (item 13).
00010:     ///
00011:     /// Single authority for shelter electrical state. The Core owns the
00012:     /// deterministic math (generation, draw, battery reserve, brownout
00013:     /// effects); the host presents a panel and applies the consequences
00014:     /// to air filtration, water, clinic, greenhouse, foundry, and lighting
00015:     /// via adapters.
00016:     ///
00017:     /// State is captured/restored through <see cref="PowerGridState"/> and
00018:     /// the envelope in <see cref="PowerGridSave"/>. Every mutation emits a
00019:     /// typed <see cref="PowerGridEvent"/> that the host listens to.
00020:     /// </summary>
00021:     public sealed class PowerGridSystem
00022:     {
00023:         private readonly PowerGridState _state;
00024:         private readonly List<PowerGridRoom> _rooms;
00025:         private readonly ISeededRng _rng;
00026:         // Runtime generation sources are projections from their owning systems.
00027:         // They are deliberately not persisted here: the owning save section
00028:         // restores first, then the host republishes the contribution.
00029:         private readonly Dictionary<string, float> _generationContributions =
00030:             new Dictionary<string, float>(StringComparer.Ordinal);
00031:
00032:         // Phase 2 (B5–B8): runtime-only brownout edge bookkeeping. Never saved;
00033:         // RestoreState re-seeds it from the restored state so a reload never
00034:         // replays a brownout-began/ended transition (flagship §14.3).
00035:         private bool _prevTickBrownout;
00036:
00037:         // B5–B8 expansion: runtime-only source-degradation edge latches
00038:         // (flagship §27 — source maintenance/failure events). Each fires once
00039:         // per degradation cycle; RestoreState re-seeds them from the restored
00040:         // state so a reload never replays a warning.
00041:         private bool _generatorWornWarned;
00042:         private bool _fuelStarvedWarned;
00043:
00044:         /// <summary>Raised whenever a room's powered state changes.</summary>
00045:         public event Action<PowerGridEvent>? OnPowerChanged;
00046:
00047:         /// <summary>Raised at end of every tick with the day summary.</summary>
00048:         public event Action<PowerGridTickSummary>? OnTickSummary;
00049:
00050:         public PowerGridSystem(PowerGridState state, IEnumerable<PowerGridRoom> rooms, ISeededRng rng)
00051:         {
00052:             if (state == null) throw new ArgumentNullException(nameof(state));
00053:             if (rooms == null) throw new ArgumentNullException(nameof(rooms));
00054:             _rooms = new List<PowerGridRoom>();
00055:             var roomIds = new HashSet<string>(StringComparer.Ordinal);
00056:             foreach (var room in rooms)
00057:             {
00058:                 if (room == null || string.IsNullOrWhiteSpace(room.RoomId)) continue;
00059:                 string roomId = room.RoomId.Trim();
00060:                 if (!roomIds.Add(roomId)) continue;
00061:                 _rooms.Add(CloneRoom(room, roomId));
00062:             }
00063:             if (_rooms.Count == 0)
00064:                 throw new InvalidOperationException("PowerGridSystem: at least one room required.");
00065:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00066:             _state = new PowerGridState();
00067:             _state.RestoreInto(state, _rooms);
00068:             RepublishEbPvdInstalledContribution();
00069:         }
00070:
00071:         public PowerGridState State => _state;
00072:         public IReadOnlyList<PowerGridRoom> Rooms => _rooms;
00073:
00074:         /// <summary>
00075:         /// Base generator output plus current runtime contributions (kW is
00076:         /// represented as watts at this authority). External contributions are
00077:         /// fuel-free; the base generator remains the only fuel-burning source.
00078:         /// </summary>
00079:         public float GenerationWatts
00080:         {
00081:             get
00082:             {
00083:                 // B5–B8 Phase 5: the base generator's rated output degrades
00084:                 // with condition (factor is 1 at/above the threshold — legacy
00085:                 // parity for a healthy generator). External contributions are
00086:                 // NOT scaled here; their owning systems own their condition.
00087:                 float total = _state.GenerationWatts * GeneratorOutputFactor;
00088:                 foreach (var contribution in _generationContributions.Values)
00089:                     total += Math.Max(0f, contribution);
00090:                 return total;
00091:             }
00092:         }
00093:
00094:         public float BaseGenerationWatts => _state.GenerationWatts;
00095:         public IReadOnlyDictionary<string, float> GenerationContributions => _generationContributions;
00096:         public float FuelUnits => _state.FuelUnits;
00097:         public float BatteryReserveWh => _state.BatteryReserveWh;
00098:         public float BatteryCapacityWh => _state.BatteryCapacityWh;
00099:         public float TotalDrawWatts => ComputeTotalDraw();
00100:         public float NetWatts => GenerationWatts - TotalDrawWatts;
00101:         public bool IsBrownout => TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0;
00102:
00103:         // ---- C2[6] 23A: canonical demand/supply/deficit read model ----------
00104:         //
00105:         // Panels, briefings and the cascade layer must read these instead of
00106:         // re-deriving arithmetic. <see cref="AvailableSupplyWatts"/> is exactly the
00107:         // number the deterministic allocator uses (generation plus the sustainable
00108:         // battery discharge the tick can hold for a day), so a UI forecast can
00109:         // never disagree with the simulation.
00110:
00111:         /// <summary>Battery watts the current reserve can sustain across a 24 h day.</summary>
00112:         public float SustainableBatteryDischargeWatts => Math.Max(0f, _state.BatteryReserveWh / 24f);
00113:
00114:         /// <summary>Generation plus sustainable battery discharge — the served-power ceiling.</summary>
00115:         public float AvailableSupplyWatts => GenerationWatts + SustainableBatteryDischargeWatts;
00116:
00117:         /// <summary>Intent draw above the served-power ceiling (0 when supply covers demand).</summary>
00118:         public float DeficitWatts => Math.Max(0f, TotalDrawWatts - AvailableSupplyWatts);
00119:
00120:         /// <summary>
00121:         /// Estimated hours the current battery reserve lasts against the current
00122:         /// intent draw, using the same generation/draw numbers the daily tick
00123:         /// exchanges. <see cref="float.PositiveInfinity"/> when generation covers
00124:         /// demand. Never negative; never NaN.
00125:         /// </summary>
00126:         public float EstimatedRuntimeHours
00127:         {
00128:             get
00129:             {
00130:                 float drainWatts = TotalDrawWatts - GenerationWatts;
00131:                 if (drainWatts <= 0f) return float.PositiveInfinity;
00132:                 return _state.BatteryReserveWh / drainWatts;
00133:             }
00134:         }
00136:         /// <summary>
00137:         /// Estimated days of fuel left at the base generator's current rated
00138:         /// (condition-scaled) burn, matching the tick's fuel formula exactly.
00139:         /// External fuel-free contributions are excluded by construction.
00140:         /// </summary>
00141:         public float EstimatedFuelRunwayDays
00142:         {
00143:             get
00144:             {
00145:                 float baseWatts = _state.GenerationWatts * GeneratorOutputFactor;
00146:                 float burnPerDay = Math.Max(0.0001f, baseWatts * 24f * 0.001f);
00147:                 return _state.FuelUnits / burnPerDay;
00148:             }
00149:         }
00150:
00151:         /// <summary>
00152:         /// Publish one external generation source. The source ID is stable and
00153:         /// replacing a value is idempotent, so a host can republish after every
00154:         /// campaign tick without accumulating duplicate output.
00155:         /// </summary>
00156:         public bool SetGenerationContribution(string sourceId, float watts)
00157:         {
00158:             if (string.IsNullOrWhiteSpace(sourceId)) return false;
00159:             if (float.IsNaN(watts) || float.IsInfinity(watts)) return false;
00160:             float normalized = Math.Max(0f, watts);
00161:             if (normalized <= 0f)
00162:                 _generationContributions.Remove(sourceId);
00163:             else
00164:                 _generationContributions[sourceId] = normalized;
00165:
00166:             OnPowerChanged?.Invoke(new PowerGridEvent(
00167:                 PowerGridEventKind.GenerationChanged,
00168:                 sourceId,
00169:                 _state.SimDay,
00170:                 normalized > 0f ? "generation_contribution_set" : "generation_contribution_removed",
00171:                 normalized));
00172:             return true;
00173:         }
00174:
00175:         public bool RemoveGenerationContribution(string sourceId)
00176:         {
00177:             if (string.IsNullOrWhiteSpace(sourceId)) return false;
00178:             return SetGenerationContribution(sourceId, 0f);
00179:         }
00180:
00181:         /// <summary>
00182:         /// Plans 146–149 MED: stable runtime contribution id for installed
00183:         /// EB-PVD coated generator parts. Host republishes after restore.
00184:         /// </summary>
00185:         public const string EbPvdInstalledSourceId = "ebpvd_installed";
00186:
00187:         /// <summary>Hard cap on concurrent installed coated parts (blade / combustor / injector).</summary>
00188:         public const int MaxInstalledCoatedParts = 3;
00189:
00190:         /// <summary>Hard cap on total coated contribution watts.</summary>
00191:         public const float MaxEbPvdInstalledWatts = 80f;
00192:
00193:         // ---- B5–B8 Phase 2 (Plan 65): battery bank build chain -----------------
00194:
00195:         /// <summary>Canonical install item for one battery bank. Research gates
00196:         /// the item through the reconditioning recipe chain; research alone
00197:         /// never grants capacity.</summary>
00198:         public const string BatteryBankItemId = "item_battery_reconditioned";
00199:
00200:         /// <summary>Capacity added per installed bank (Wh). Old saves with zero
00201:         /// banks keep their stored capacity unchanged.</summary>
00202:         public const float BatteryBankCapacityWh = 1000f;
00203:
00204:         /// <summary>Hard cap on installed banks (bounded build chain).</summary>
00205:         public const int MaxInstalledBatteryBanks = 4;
00206:
00207:         // ---- B5–B8 Phase 5 (Plan 65): generator condition/maintenance --------
00208:
00209:         /// <summary>Canonical maintenance consumable for the base generator
00210:         /// (same item the subgrid repair and battery service use; produced by
00211:         /// the Fischer-Tropsch lubricant chain). The host consumes it — the
00212:         /// grid never touches inventory.</summary>
00213:         public const string GeneratorMaintenanceItemId = "machine_oil";
00214:
00215:         /// <summary>Condition wear per day while the generator actually burns
00216:         /// fuel. 100 → 0 over 400 burning days; an idle generator does not
00217:         /// wear.</summary>
00218:         public const float GeneratorWearPerDay = 0.25f;
00219:
00220:         /// <summary>Below this condition the generator's rated output begins
00221:         /// to degrade (worn bearings, fouled injectors).</summary>
00222:         public const float GeneratorDegradationThreshold = 50f;
00223:
00224:         /// <summary>Output factor at zero condition (a half-dead engine still
00225:         /// runs at half rating — bounded, never zero while fueled).</summary>
00226:         public const float GeneratorMinOutputFactor = 0.5f;
00227:
00228:         /// <summary>
00229:         /// Current generator condition 0..100. Bounded component state
00230:         /// (flagship §8.8): wear producer (fuel-burning days), maintenance
00231:         /// action with a real item cost, bounded effect, save persistence.
00232:         /// </summary>
00233:         public float GeneratorCondition => _state.GeneratorCondition;
00234:
00235:         /// <summary>Output multiplier the condition applies to the base
00236:         /// generator's rated watts (external contributions are unaffected —
00237:         /// their owning systems own their own condition).</summary>
00238:         public float GeneratorOutputFactor => _state.GeneratorCondition >= GeneratorDegradationThreshold
00239:             ? 1f
00240:             : GeneratorMinOutputFactor
00241:               + (1f - GeneratorMinOutputFactor) * (_state.GeneratorCondition / GeneratorDegradationThreshold);
00242:
00243:         /// <summary>
00244:         /// B5–B8 Phase 5: service the generator back to full condition.
00245:         /// Caller consumes the canonical
00246:         /// <see cref="GeneratorMaintenanceItemId"/> first (coated-part
00247:         /// discipline). A service on an already-healthy generator is blocked —
00248:         /// wasted effort is a blocked action, not a silent success.
00249:         /// </summary>
00250:         public bool PerformGeneratorMaintenance(out string reason)
00251:         {
00252:             reason = string.Empty;
00253:             if (_state.GeneratorCondition >= 100f)
00254:             {
00255:                 reason = "condition_full";
00256:                 return false;
00257:             }
00258:
00259:             _state.GeneratorCondition = 100f;
00260:             _generatorWornWarned = false;
00261:             OnPowerChanged?.Invoke(new PowerGridEvent(
00262:                 PowerGridEventKind.GeneratorMaintained,
00263:                 GeneratorMaintenanceItemId,
00264:                 _state.SimDay,
00265:                 "generator_serviced",
00266:                 GeneratorCondition));
00267:             return true;
00268:         }
00269:
00270:         /// <summary>
00271:         /// Install one battery bank (caller consumes the canonical
00272:         /// <see cref="BatteryBankItemId"/> item first — same discipline as
00273:         /// coated parts). Adds bounded capacity; never touches the current
00274:         /// reserve, so an old save's stored energy is unchanged.
00275:         /// </summary>
00276:         public bool TryInstallBatteryBank(out string reason)
00277:         {
00278:             reason = string.Empty;
00279:             if (_state.InstalledBatteryBankCount >= MaxInstalledBatteryBanks)
00280:             {
00285:             _state.InstalledBatteryBankCount++;
00286:             _state.BatteryCapacityWh += BatteryBankCapacityWh;
00287:             OnPowerChanged?.Invoke(new PowerGridEvent(
00288:                 PowerGridEventKind.BatteryBankInstalled,
00289:                 BatteryBankItemId,
00290:                 _state.SimDay,
00291:                 "battery_bank_installed",
00294:         }
00295:
00296:         public int InstalledBatteryBankCount => _state.InstalledBatteryBankCount;
00297:
00298:         public IReadOnlyList<string> InstalledCoatedPartItemIds => _state.InstalledCoatedPartItemIds;
00299:
00300:         /// <summary>
00301:         /// Install one coated generator part by inventory item id. Requires an
00302:         /// explicit install action — minting a coating never auto-buffs the grid.
00303:         /// One slot per family (blade / combustor / injector).
00304:         /// </summary>
00305:         public bool TryInstallCoatedPart(string itemId, out string reason)
00306:         {
00307:             reason = string.Empty;
00308:             if (string.IsNullOrWhiteSpace(itemId))
00309:             {
00312:             }
00313:
00314:             string family = ResolveCoatedPartFamily(itemId);
00315:             if (string.IsNullOrEmpty(family))
00316:             {
00317:                 reason = "unsupported_coated_part";
00318:                 return false;
00327:                     return false;
00328:                 }
00329:                 if (string.Equals(ResolveCoatedPartFamily(installed[i]), family, StringComparison.Ordinal))
00330:                 {
00331:                     reason = "family_slot_occupied";
00332:                     return false;
00333:                 }
00341:
00342:             installed.Add(itemId);
00343:             RepublishEbPvdInstalledContribution();
00344:             OnPowerChanged?.Invoke(new PowerGridEvent(
00345:                 PowerGridEventKind.GenerationChanged,
00346:                 itemId,
00347:                 _state.SimDay,
00348:                 "coated_part_installed",
00349:                 ResolveCoatedPartWatts(itemId)));
00350:             return true;
00351:         }
00352:
00353:         public bool TryUninstallCoatedPart(string itemId, out string reason)
00354:         {
00355:             reason = string.Empty;
00356:             if (string.IsNullOrWhiteSpace(itemId))
00357:             {
00366:             }
00367:
00368:             RepublishEbPvdInstalledContribution();
00369:             OnPowerChanged?.Invoke(new PowerGridEvent(
00370:                 PowerGridEventKind.GenerationChanged,
00371:                 itemId,
00372:                 _state.SimDay,
00373:                 "coated_part_uninstalled",
00374:                 0f));
00378:         /// <summary>
00379:         /// Idempotent republish of installed coated-part watts into the runtime
00380:         /// contribution map. Call after restore and after install/uninstall.
00381:         /// </summary>
00382:         public void RepublishEbPvdInstalledContribution()
00383:         {
00384:             float watts = 0f;
00385:             var installed = _state.InstalledCoatedPartItemIds;
00386:             for (int i = 0; i < installed.Count; i++)
00387:                 watts += ResolveCoatedPartWatts(installed[i]);
00388:             watts = Math.Clamp(watts, 0f, MaxEbPvdInstalledWatts);
00389:             SetGenerationContribution(EbPvdInstalledSourceId, watts);
00390:         }
00391:
00392:         public static string ResolveCoatedPartFamily(string itemId)
00393:         {
00394:             if (string.Equals(itemId, "item_coated_turbine_blade", StringComparison.Ordinal))
00395:                 return "blade";
00396:             if (string.Equals(itemId, "item_coated_combustor_tile", StringComparison.Ordinal))
00401:         }
00402:
00403:         public static float ResolveCoatedPartWatts(string itemId)
00404:         {
00405:             // Bounded historical-engineering bonuses; never overwrite base GenerationWatts.
00406:             if (string.Equals(itemId, "item_coated_turbine_blade", StringComparison.Ordinal))
00407:                 return 40f;
00408:             if (string.Equals(itemId, "item_coated_combustor_tile", StringComparison.Ordinal))
00409:                 return 35f;
00413:         }
00414:
00415:         public PowerGridSnapshot Snapshot()
00416:         {
00417:             var snapshot = new PowerGridSnapshot
00418:             {
00419:                 Day = _state.SimDay,
00420:                 GenerationWatts = GenerationWatts,
00421:                 FuelUnits = FuelUnits,
00422:                 BatteryReserveWh = BatteryReserveWh,
00423:                 BatteryCapacityWh = BatteryCapacityWh,
00424:                 TotalDrawWatts = TotalDrawWatts,
00427:                 RoomIds = new List<string>(RoomPoweredStates())
00428:             };
00429:             foreach (var contribution in _generationContributions)
00430:                 snapshot.GenerationContributions[contribution.Key] = contribution.Value;
00431:             return snapshot;
00432:         }
00433:
00434:         public bool IsRoomPowered(string roomId)
00435:         {
00436:             if (string.IsNullOrEmpty(roomId)) return false;
00437:             var r = FindRoom(roomId);
00438:             if (r == null) return false;
00441:         }
00442:
00443:         public PowerGridRoomPriority EffectivePriority(string roomId)
00444:         {
00445:             var r = FindRoom(roomId);
00446:             if (r == null) return PowerGridRoomPriority.Disabled;
00447:             // Phase 2 (B5–B8): player override wins; catalog DefaultPriority
00448:             // otherwise. Previously an override-less room always read Standard,
00449:             // silently discarding the catalog's critical/low classification.
00450:             for (int i = 0; i < _state.Priorities.Count; i++)
00451:             {
00452:                 if (_state.Priorities[i].RoomId == roomId)
00453:                     return _state.Priorities[i].Priority;
00460:         /// Idempotent: re-closing an already-closed breaker is a no-op.
00461:         /// </summary>
00462:         public bool ToggleBreaker(string roomId)
00463:         {
00464:             var r = FindRoom(roomId);
00465:             if (r == null) return false;
00466:             bool wasClosed = _state.IsBreakerClosed(roomId);
00467:             _state.SetBreaker(roomId, !wasClosed);
00468:             OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.BreakerToggled,
00469:                 roomId, _state.SimDay, wasClosed ? "closed_to_open" : "open_to_closed"));
00470:             return true;
00471:         }
00472:
00473:         public bool SetBreaker(string roomId, bool closed)
00474:         {
00475:             var r = FindRoom(roomId);
00476:             if (r == null) return false;
00477:             bool wasClosed = _state.IsBreakerClosed(roomId);
00478:             if (wasClosed == closed) return true;
00479:             _state.SetBreaker(roomId, closed);
00480:             OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.BreakerToggled,
00481:                 roomId, _state.SimDay, closed ? "open_to_closed" : "closed_to_open"));
00482:             return true;
00483:         }
00484:
00485:         public void MarkTripped(string roomId, int day)
00486:         {
00487:             _state.MarkTripped(roomId, day);
00488:             OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.Tripped,
00489:                 roomId, day, "manual_trip"));
00490:         }
00491:
00492:         public void ClearTripped(string roomId) => _state.ClearTripped(roomId);
00493:
00494:         public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);
00495:
00496:         public bool SetPriority(string roomId, PowerGridRoomPriority priority)
00497:         {
00498:             var r = FindRoom(roomId);
00499:             if (r == null || !Enum.IsDefined(typeof(PowerGridRoomPriority), priority))
00500:                 return false;
00501:             _state.SetRoomPriority(roomId, priority);
00502:             OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.PriorityChanged,
00503:                 roomId, _state.SimDay, priority.ToString()));
00504:             return true;
00505:         }
00506:
00507:         /// <summary>
00508:         /// B5–B8 Phase 3 (Plan 66): register a dynamic load room — the
00509:         /// power-load subscription contract (§6.3). Consumer systems (sump
00510:         /// pump nodes now, perimeter emplacements later) expose a stable load
00511:         /// id and nominal draw; the grid owns allocation/shedding from that
00512:         /// point on. The consumer reads its served state via
00513:         /// <see cref="IsRoomServed"/> and never mutates generation or
00514:         /// brownout state.
00515:         ///
00516:         /// Idempotent: a RoomId already present (e.g. from the
00517:         /// power_grid.json catalog) is left untouched — catalog rooms take
00518:         /// precedence. Registered loads participate in draw and priority
00519:         /// allocation immediately.
00520:         ///
00521:         /// Dynamic loads are re-registered by their owning system after that
00522:         /// system's save restores (the grid's room list itself is not saved).
00523:         /// </summary>
00524:         public bool RegisterLoadRoom(PowerGridRoom room)
00525:         {
00526:             if (room == null || string.IsNullOrWhiteSpace(room.RoomId)
00527:                 || float.IsNaN(room.DrawWatts)
00528:                 || float.IsInfinity(room.DrawWatts)
00533:             string roomId = room.RoomId.Trim();
00534:             if (FindRoom(roomId) != null) return false;
00535:             var cloned = CloneRoom(room, roomId);
00536:             _rooms.Add(cloned);
00537:             OnPowerChanged?.Invoke(new PowerGridEvent(
00538:                 PowerGridEventKind.LoadRoomRegistered,
00539:                 roomId, _state.SimDay, "load_room_registered", cloned.DrawWatts));
00540:             return true;
00541:         }
00542:
00543:         /// <summary>
00544:         /// B5–B8 expansion (§27): emergency load-shed preset — the
00545:         /// brownout-management shortcut. Demotes every non-Critical load one
00546:         /// tier (Standard → Low, Low stays Low); Critical life-support loads
00547:         /// are never touched. Deterministic, typed (one PriorityChanged event
00548:         /// per changed room), fully reversible by re-applying catalog defaults
00549:         /// via <see cref="ApplyCatalogDefaultPriorities"/> or manual overrides.
00550:         /// Returns the changed room ids (journal/briefing surface).
00551:         /// </summary>
00552:         public IReadOnlyList<string> ApplyBrownoutShedPreset()
00553:         {
00554:             var changed = new List<string>();
00555:             for (int i = 0; i < _rooms.Count; i++)
00556:             {
00557:                 var r = _rooms[i];
00558:                 var current = EffectivePriority(r.RoomId);
00559:                 var target = current switch
00560:                 {
00561:                     PowerGridRoomPriority.Standard => PowerGridRoomPriority.Low,
00562:                     _ => current
00563:                 };
00564:                 if (target != current)
00565:                 {
00566:                     SetPriority(r.RoomId, target);
00567:                     changed.Add(r.RoomId);
00568:                 }
00569:             }
00570:             OnPowerChanged?.Invoke(new PowerGridEvent(
00571:                 PowerGridEventKind.PriorityChanged, "__preset__", _state.SimDay,
00572:                 "brownout_shed_preset", changed.Count));
00573:             return changed;
00574:         }
00575:
00576:         /// <summary>B5–B8 expansion: restore the catalog defaults — clears all
00577:         /// player priority overrides so every room returns to its
00578:         /// power_grid.json classification. Returns the affected room count.</summary>
00579:         public int ApplyCatalogDefaultPriorities()
00580:         {
00581:             int count = _state.Priorities.Count;
00582:             _state.Priorities.Clear();
00583:             if (count > 0)
00584:             {
00585:                 OnPowerChanged?.Invoke(new PowerGridEvent(
00586:                     PowerGridEventKind.PriorityChanged, "__preset__", _state.SimDay,
00587:                     "catalog_defaults_restored", count));
00588:             }
00589:             return count;
00590:         }
00591:
00592:         public void AddFuel(float units)
00593:         {
00594:             if (units <= 0f || float.IsNaN(units) || float.IsInfinity(units)) return;
00595:             double next = (double)_state.FuelUnits + units;
00596:             _state.FuelUnits = (float)Math.Min(next, float.MaxValue);
00597:             OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.FuelAdded, null!,
00598:                 _state.SimDay, "fuel_added", units));
00599:         }
00600:
00601:         /// <summary>EMP storm severity applied on weather onset — catalog-driven
00602:         /// via power_grid.json `emp_storm_severity` (SHELTER_HARDENING); falls
00603:         /// back to this default when the catalog omits the field.</summary>
00604:         public const float DefaultEmpStormSurgeSeverity = 0.6f;
00605:
00606:         /// <summary>Default fraction of battery capacity drained at full surge severity.</summary>
00607:         public const float DefaultSurgeBatteryDrainFraction = 0.15f;
00608:
00609:         private float _empStormSurgeSeverity = DefaultEmpStormSurgeSeverity;
00610:         private float _surgeBatteryDrainFraction = DefaultSurgeBatteryDrainFraction;
00611:
00612:         /// <summary>Catalog-driven surge tuning (0..1 each). Applies from the next surge.</summary>
00613:         public void ConfigureSurge(float empStormSeverity, float batteryDrainFraction)
00614:         {
00615:             _empStormSurgeSeverity = float.IsNaN(empStormSeverity) || float.IsInfinity(empStormSeverity)
00616:                 ? 0f
00617:                 : Math.Clamp(empStormSeverity, 0f, 1f);
00621:         }
00622:
00623:         public float EmpStormSeverity => _empStormSurgeSeverity;
00624:         public float SurgeBatteryDrain => _surgeBatteryDrainFraction;
00625:
00626:         /// <summary>
00627:         /// Apply an external electrical surge (EMP storm onset, orbital impact).
00628:         /// Deterministic: trips the lowest-priority non-tripped circuits first
00629:         /// (priority tier ascending, then RoomId ordinal), draining the battery
00630:         /// proportionally to severity. Critical rooms are exempt below 0.9
00631:         /// severity so a surge can wound the grid without destroying it.
00632:         /// Deduped per day: a second surge event on the same day is a no-op.
00633:         /// The surge day persists via <see cref="PowerGridState.LastSurgeDay"/>.
00634:         /// </summary>
00635:         public IReadOnlyList<string> ApplySurgeDay(int day, float severity01)
00636:         {
00637:             if (float.IsNaN(severity01) || float.IsInfinity(severity01))
00638:                 return Array.Empty<string>();
00639:             float severity = Math.Clamp(severity01, 0f, 1f);
00645:             foreach (var r in _rooms)
00646:             {
00647:                 if (_state.IsRoomTripped(r.RoomId)) continue;
00648:                 // Effective tier: player override wins; catalog default otherwise.
00649:                 bool hasOverride = false;
00650:                 for (int p = 0; p < _state.Priorities.Count; p++)
00651:                 {
00652:                     if (_state.Priorities[p].RoomId == r.RoomId) { hasOverride = true; break; }
00653:                 }
00654:                 var tier = (int)(hasOverride
00655:                     ? _state.GetRoomPriority(r.RoomId)
00656:                     : r.DefaultPriority);
00657:                 if (tier == (int)PowerGridRoomPriority.Disabled) continue;
00658:                 if (tier == (int)PowerGridRoomPriority.Critical && !criticalEligible) continue;
00659:                 candidates.Add((r.RoomId, tier));
00660:             }
00661:             candidates.Sort(static (a, b) =>
00662:             {
00677:             _state.LastSurgeDay = day;
00678:
00679:             OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.SurgeApplied,
00680:                 tripped.Count > 0 ? string.Join(",", tripped) : "none",
00681:                 day, "surge_applied", severity));
00682:             return tripped;
00683:         }
00684:
00685:         /// <summary>
00686:         /// Tick one full day. Deterministic: given the same fuel/battery state
00687:         /// and RNG, the result is identical across hosts and runs.
00688:         /// </summary>
00689:         public PowerGridTickSummary TickDay(int day, ISeededRng tickRng)
00690:         {
00691:             var rng = tickRng ?? _rng;
00692:             _state.SimDay = day;
00693:             float draw = ComputeTotalDraw();
00694:             float gen = GenerationWatts;
00695:             float net = gen - draw; // Wh per hour assumed; full day = 24 units
00696:             float fuelConsumed = 0f;
00697:             float brownoutHours = 0f;
00698:
00699:             // Burn fuel proportional to generation.
00700:             float externalGeneration = 0f;
00701:             foreach (var contribution in _generationContributions.Values)
00702:                 externalGeneration += Math.Max(0f, contribution);
00703:             float fuelNeed = Math.Max(0f, gen - externalGeneration) * 24f * 0.001f;
00704:             if (_state.FuelUnits >= fuelNeed)
00705:             {
00706:                 _state.FuelUnits -= fuelNeed;
00707:                 fuelConsumed = fuelNeed;
00708:             }
00709:             else
00710:             {
00711:                 fuelConsumed = _state.FuelUnits;
00712:                 _state.FuelUnits = 0;
00713:                 gen *= 0.5f; // partial generation when fuel-starved.
00714:             }
00715:
00716:             // B5–B8 expansion: fuel-starvation is a failure edge — the first
00717:             // dry day warns once (runtime latch; service/refuel resets it).
00718:             if (fuelConsumed < fuelNeed && !_fuelStarvedWarned)
00719:             {
00720:                 _fuelStarvedWarned = true;
00721:                 OnPowerChanged?.Invoke(new PowerGridEvent(
00722:                     PowerGridEventKind.FuelStarved, null!, _state.SimDay,
00723:                     "generator_fuel_starved", fuelConsumed));
00724:             }
00725:             else if (fuelConsumed >= fuelNeed)
00726:             {
00727:                 _fuelStarvedWarned = false;
00728:             }
00729:
00730:             // B5–B8 expansion: the generator wears only on burning days. An
00731:             // idle engine does not degrade.
00732:             if (fuelConsumed > 0f)
00733:             {
00734:                 _state.GeneratorCondition = Math.Max(0f,
00735:                     _state.GeneratorCondition - GeneratorWearPerDay);
00736:
00737:                 // Crossing below the degradation threshold is the warn edge —
00738:                 // output is now derated; fires once per wear cycle.
00739:                 if (!_generatorWornWarned &&
00740:                     _state.GeneratorCondition < GeneratorDegradationThreshold)
00741:                 {
00742:                     _generatorWornWarned = true;
00743:                     OnPowerChanged?.Invoke(new PowerGridEvent(
00744:                         PowerGridEventKind.GeneratorWorn, GeneratorMaintenanceItemId,
00745:                         _state.SimDay, "generator_worn", _state.GeneratorCondition));
00746:                 }
00747:             }
00748:
00749:             // Phase 2 (B5–B8): deterministic priority allocation projection.
00750:             // Computed after the fuel-starvation adjustment (that is the real
00751:             // generation this tick) and before the battery exchange (so the
00752:             // discharge capacity reflects the start-of-tick reserve). This is
00753:             // a projection only — the aggregate battery/fuel/brownout-hours
00754:             // math below is untouched and stays byte-parity with legacy ticks.
00755:             float generationUsed = gen;
00756:             float reserveBeforeExchange = _state.BatteryReserveWh;
00757:             var allocation = ComputeAllocation(generationUsed, draw, reserveBeforeExchange);
00758:
00759:             if (net >= 0)
00760:             {
00761:                 float spareWh = net * 24f;
00778:             }
00779:
00780:             // Random load spike (deterministic via injected rng).
00781:             if (rng.NextDouble() < 0.05 && _state.BatteryReserveWh > 0)
00782:             {
00783:                 float spike = (float)(rng.NextDouble() * 30.0);
00784:                 _state.BatteryReserveWh = Math.Max(0, _state.BatteryReserveWh - spike);
00786:             }
00787:
00788:             // Trip breakers that overload for more than 4 hours of brownout.
00789:             if (brownoutHours >= 4f)
00790:             {
00791:                 foreach (var r in _rooms)
00792:                 {
00793:                     if (_state.GetRoomPriority(r.RoomId) == PowerGridRoomPriority.Disabled)
00794:                         continue;
00795:                     if (!_state.IsBreakerClosed(r.RoomId)) continue;
00796:                     if (rng.NextDouble() < 0.10)
00797:                     {
00798:                         _state.MarkTripped(r.RoomId, day);
00799:                         OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.Tripped,
00800:                             r.RoomId, day, "brownout_overload"));
00801:                     }
00802:                 }
00803:             }
00804:
00805:             var summary = new PowerGridTickSummary
00806:             {
00807:                 Day = day,
00808:                 FuelConsumed = fuelConsumed,
00809:                 BatteryEndWh = _state.BatteryReserveWh,
00810:                 BrownoutHours = brownoutHours,
00811:                 IsBrownout = IsBrownout,
00812:                 // Phase 2 (B5–B8) allocation projection + edge transitions.
00813:                 GenerationWatts = generationUsed,
00814:                 RequestedDrawWatts = draw,
00815:                 ServedWatts = allocation.ServedWatts,
00816:                 UnservedWatts = Math.Max(0f, draw - allocation.ServedWatts),
00817:                 HasCriticalDeficit = allocation.HasCriticalDeficit,
00820:             };
00821:             bool brownoutNow = summary.IsBrownout;
00822:             summary.BrownoutBegan = brownoutNow && !_prevTickBrownout;
00823:             summary.BrownoutEnded = !brownoutNow && _prevTickBrownout;
00824:             _prevTickBrownout = brownoutNow;
00825:             OnTickSummary?.Invoke(summary);
00826:             return summary;
00827:         }
00828:
00829:         /// <summary>Tolerance for serving a room within available capacity.</summary>
00830:         internal const float AllocationEpsilon = 0.01f;
00831:
00832:         /// <summary>
00833:         /// Phase 2 (B5–B8): deterministic priority allocation projection.
00834:         ///
00836:         /// ordered by effective priority tier descending, then RoomId ordinal
00837:         /// ascending. Available power is generation (post fuel adjustment)
00838:         /// plus the battery discharge this tick can sustain
00839:         /// (<c>reserve / 24h</c>, only while demand exceeds generation — the
00840:         /// same condition under which the legacy aggregate math drains the
00841:         /// battery). Rooms are served in order while cumulative draw fits;
00842:         /// every remaining room is shed. Consequences:
00843:         ///
00844:         /// - a higher-priority room is never shed while a lower-priority room
00845:         ///   is served (single ordered pass, shed set is always a suffix);
00846:         /// - unserved Critical-tier rooms raise <see cref="HasCriticalDeficit"/>
00847:         ///   — the explicit life-support emergency flag (flagship §8.6);
00848:         /// - no RNG participates in load order.
00849:         ///
00850:         /// The projection does not mutate state; served/shed classification is
00851:         /// derived from exactly the same generation/reserve numbers the legacy
00852:         /// battery math consumes, so both stay consistent by construction.
00853:         ///
00854:         /// Serving is a strict priority prefix: the first room whose load does
00855:         /// not fit is shed together with every room after it, even if a later
00856:         /// smaller room would fit. Real grids shed whole feeders in order — a
00857:         /// predictable suffix beats best-fit scavenging, and it keeps the
00858:         /// flagship invariant exact: no lower-priority load is ever served
00859:         /// while a higher-priority load is unserved. Spare capacity below the
00860:         /// next whole room stays unused (the legacy aggregate battery drain is
00861:         /// untouched parity behavior).
00862:         /// </summary>
00863:         private PowerGridAllocation ComputeAllocation(float generationWatts, float requestedDrawWatts,
00864:             float batteryReserveWh)
00865:         {
00866:             var ordered = new List<(PowerGridRoom Room, int Tier)>(_rooms.Count);
00867:             for (int i = 0; i < _rooms.Count; i++)
00868:             {
00869:                 var r = _rooms[i];
00870:                 if (!_state.IsBreakerClosed(r.RoomId)) continue;
00871:                 if (_state.IsRoomTripped(r.RoomId)) continue;
00872:                 int tier = (int)EffectivePriority(r.RoomId);
00873:                 if (tier == (int)PowerGridRoomPriority.Disabled) continue;
00874:                 ordered.Add((r, tier));
00875:             }
00876:             ordered.Sort(static (a, b) =>
00877:             {
00880:             });
00881:
00882:             float deficit = Math.Max(0f, requestedDrawWatts - generationWatts);
00883:             float batteryDischargeWatts = deficit > 0f
00884:                 ? Math.Min(batteryReserveWh / 24f, deficit)
00885:                 : 0f;
00886:             float availablePower = generationWatts + batteryDischargeWatts;
00887:
00888:             var result = new PowerGridAllocation();
00889:             float cumulative = 0f;
00890:             bool shedding = false;
00893:                 var room = ordered[i].Room;
00894:                 if (!shedding &&
00895:                     cumulative + room.DrawWatts <= availablePower + AllocationEpsilon)
00896:                 {
00897:                     result.ServedRoomIds.Add(room.RoomId);
00898:                     cumulative += room.DrawWatts;
00899:                 }
00900:                 else
00901:                 {
00902:                     // Strict suffix: once one room doesn't fit, everything
00903:                     // after it sheds regardless of size (deterministic, and
00904:                     // never serves a lower-priority room first).
00905:                     shedding = true;
00906:                     result.ShedRoomIds.Add(room.RoomId);
00914:
00915:         /// <summary>
00916:         /// Phase 2 (B5–B8): allocation-aware typed powered query for consumer
00917:         /// systems (greenhouse controlled-environment, sump pump, perimeter
00918:         /// sentries). Unlike <see cref="IsRoomPowered"/> — which treats a
00919:         /// brownout as a global outage — this reports whether the room's load
00920:         /// is actually served under deterministic priority allocation, so
00921:         /// critical loads can remain powered while optional loads shed.
00922:         /// Consumers subscribe to <see cref="OnTickSummary"/> for the tick
00923:         /// projection; this query is for point-in-time reads.
00924:         /// </summary>
00925:         public bool IsRoomServed(string roomId)
00926:         {
00927:             if (string.IsNullOrEmpty(roomId)) return false;
00928:             var allocation = ComputeAllocation(GenerationWatts, TotalDrawWatts, _state.BatteryReserveWh);
00929:             return allocation.ServedRoomIds.Contains(roomId);
00930:         }
00931:
00932:         private sealed class PowerGridAllocation
00933:         {
00934:             public float ServedWatts;
00935:             public bool HasCriticalDeficit;
00936:             public List<string> ServedRoomIds = new List<string>();
00937:             public List<string> ShedRoomIds = new List<string>();
00938:         }
00939:
00940:         public PowerGridState CaptureState() => _state.Capture();
00941:
00942:         public void RestoreState(PowerGridState state)
00943:         {
00944:             if (state == null) throw new ArgumentNullException(nameof(state));
00945:             _state.RestoreInto(state, _rooms);
00946:             // Runtime contribution map is not serialized; republish installed
00947:             // coated-part watts so GenerationWatts matches InstalledCoatedPartItemIds.
00948:             RepublishEbPvdInstalledContribution();
00949:             // Phase 2 (B5–B8): re-seed brownout edge bookkeeping from the
00950:             // restored state so a reload never replays a begin/end transition.
00951:             _prevTickBrownout = IsBrownout;
00952:             // B5–B8 expansion: re-seed source-degradation latches from the
00953:             // restored state (an already-worn restored generator must not
00954:             // replay its warning).
00955:             _generatorWornWarned = _state.GeneratorCondition < GeneratorDegradationThreshold;
00956:             _fuelStarvedWarned = _state.FuelUnits <= 0f;
00957:         }
00958:
00959:         private PowerGridRoom? FindRoom(string roomId)
00964:         }
00965:
00966:         private static PowerGridRoom CloneRoom(PowerGridRoom source, string canonicalRoomId)
00967:         {
00968:             return new PowerGridRoom
00969:             {
00970:                 RoomId = canonicalRoomId,
00971:                 DisplayName = source.DisplayName ?? string.Empty,
00972:                 DrawWatts = PowerGridState.SanitizeNonNegativeFinite(source.DrawWatts),
00973:                 DefaultPriority = Enum.IsDefined(typeof(PowerGridRoomPriority), source.DefaultPriority)
00974:                     ? source.DefaultPriority
00975:                     : PowerGridRoomPriority.Standard,
00976:                 FailureEffectId = source.FailureEffectId ?? string.Empty
00979:
00980:         /// <summary>
00981:         /// Current draw a room presents to the distribution network: its catalog
00982:         /// draw when powered, 0 when tripped/open/disabled (SHELTER_HARDENING —
00983:         /// feeds the subgrid's per-node thermal/fuse model via ApplyRoomLoad).
00984:         /// </summary>
00985:         public float GetRoomDrawWatts(string roomId)
00986:         {
00987:             if (!IsRoomPowered(roomId)) return 0f;
00988:             var r = FindRoom(roomId);
00989:             return r?.DrawWatts ?? 0f;
00996:             // not disabled). The brownout suppression number lives on the new
00997:             // EffectiveTotalDrawWatts property. Splitting intent from effect
00998:             // stops IsBrownout from flipping false the same tick brownout fires:
00999:             // a 0-returning TotalDrawWatts made NetWatts positive, which made
01000:             // the battery charge UP during brownout (opposite of intent).
01001:             float draw = 0f;
01002:             for (int i = 0; i < _rooms.Count; i++)
01003:             {
01004:                 var r = _rooms[i];
01005:                 if (!_state.IsBreakerClosed(r.RoomId)) continue;
01006:                 if (_state.IsRoomTripped(r.RoomId)) continue;
01007:                 var pri = _state.GetRoomPriority(r.RoomId);
01008:                 if (pri == PowerGridRoomPriority.Disabled) continue;
01009:                 draw += r.DrawWatts;
01010:             }
01011:             return draw;
01012:         }
01016:         /// grid is in brownout state, every room drops to 0 effective draw,
01017:         /// which means the unmet-deficit (and thus the brownout-hours
01018:         /// calculation) collapses to whatever the *previous-tick* steady-state
01019:         /// looked like. Returns 0 during brownout to preserve the legacy
01020:         /// TickDay brownout-hours math that downstream code relies on.
01021:         /// </summary>
01022:         public float EffectiveTotalDrawWatts =>
01023:             IsBrownout ? 0f : TotalDrawWatts;
01024:
01025:         private List<string> RoomPoweredStates()
01026:         {
01037:
01038:     /// <summary>Priority used by the grid when total draw exceeds generation.</summary>
01039:     public enum PowerGridRoomPriority
01040:     {
01041:         Disabled = 0,
01042:         Low = 1,
01043:         Standard = 2,
01046:
01047:     [Serializable]
01048:     public sealed class PowerGridRoom
01049:     {
01050:         public string RoomId;
01051:         public string DisplayName;
01052:         public float DrawWatts;
01053:         public PowerGridRoomPriority DefaultPriority;
01054:         public string FailureEffectId; // semantic id the host looks up.
01055:
01056:         public PowerGridRoom() { }
01057:
01058:         public PowerGridRoom(string roomId, string displayName, float drawWatts,
01059:             PowerGridRoomPriority defaultPriority = PowerGridRoomPriority.Standard,
01060: string? failureEffectId = null)
01061:         {
01062:             RoomId = roomId;
01069:
01070:     [Serializable]
01071:     public sealed class PowerGridState
01072:     {
01073:         public int SimDay;
01074:         public float GenerationWatts;
01075:         public float FuelUnits;
01076:         public float BatteryReserveWh;
01077:         public float BatteryCapacityWh;
01078:         public List<string> ClosedBreakers = new List<string>();
01079:         public List<string> TrippedRooms = new List<string>();
01080:         public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();
01081:
01082:         /// <summary>
01083:         /// Day of the last applied surge (EMP/orbital). Optional field: saves
01084:         /// written before the field existed restore as 0 ("no surge yet") per
01085:         /// the repository's optional-field migration tolerance.
01086:         /// </summary>
01087:         public int LastSurgeDay;
01088:
01089:         /// <summary>
01090:         /// Plans 146–149 MED: inventory item ids of coated parts installed into
01091:         /// the generator. Optional — old saves restore empty (no install).
01092:         /// Runtime watts are republished via <see cref="PowerGridSystem.RepublishEbPvdInstalledContribution"/>.
01093:         /// </summary>
01094:         public List<string> InstalledCoatedPartItemIds = new List<string>();
01095:
01096:         /// <summary>
01097:         /// B5–B8 Phase 2: installed battery-bank count (additive — old saves
01098:         /// restore 0 and keep their stored capacity exactly).
01099:         /// </summary>
01100:         public int InstalledBatteryBankCount;
01101:
01102:         /// <summary>
01103:         /// B5–B8 Phase 5: base generator condition 0..100. Field initializer
01104:         /// 100 is the migration contract: legacy saves lacking the field
01105:         /// restore a healthy generator (no retroactive degradation); new saves
01106:         /// persist the value explicitly.
01107:         /// </summary>
01108:         public float GeneratorCondition = 100f;
01109:
01110:         public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
01111:         public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);
01112:
01113:         public void SetBreaker(string roomId, bool closed)
01114:         {
01115:             if (closed) ClosedBreakers.Remove(roomId);
01116:             else if (!ClosedBreakers.Contains(roomId)) ClosedBreakers.Add(roomId);
01117:         }
01118:
01119:         public void MarkTripped(string roomId, int day)
01120:         {
01121:             if (!TrippedRooms.Contains(roomId)) TrippedRooms.Add(roomId);
01122:         }
01123:
01124:         public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);
01125:
01126:         public PowerGridRoomPriority GetRoomPriority(string roomId)
01127:         {
01128:             for (int i = 0; i < Priorities.Count; i++)
01129:                 if (Priorities[i].RoomId == roomId) return Priorities[i].Priority;
01130:             return PowerGridRoomPriority.Standard;
01131:         }
01132:
01133:         public void SetRoomPriority(string roomId, PowerGridRoomPriority priority)
01134:         {
01135:             for (int i = 0; i < Priorities.Count; i++)
01136:             {
01137:                 if (Priorities[i].RoomId == roomId)
01144:         }
01145:
01146:         public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms)
01147:         {
01148:             ClosedBreakers ??= new List<string>();
01149:             TrippedRooms ??= new List<string>();
01150:             Priorities ??= new List<RoomPriorityRecord>();
01151:             InstalledCoatedPartItemIds ??= new List<string>();
01152:
01153:             GenerationWatts = SanitizeNonNegativeFinite(GenerationWatts);
01154:             FuelUnits = SanitizeNonNegativeFinite(FuelUnits);
01155:             BatteryCapacityWh = SanitizeNonNegativeFinite(BatteryCapacityWh);
01156:             BatteryReserveWh = SanitizeNonNegativeFinite(BatteryReserveWh);
01157:             if (BatteryReserveWh > BatteryCapacityWh) BatteryReserveWh = BatteryCapacityWh;
01158:             InstalledBatteryBankCount = Math.Clamp(
01159:                 InstalledBatteryBankCount,
01160:                 0,
01161:                 PowerGridSystem.MaxInstalledBatteryBanks);
01162:             GeneratorCondition = IsFinite(GeneratorCondition)
01163:                 ? Math.Clamp(GeneratorCondition, 0f, 100f)
01164:                 : 0f;
01165:             if (SimDay < 0) SimDay = 0;
01166:             if (LastSurgeDay < 0) LastSurgeDay = 0;
01167:
01176:         }
01177:
01178:         public PowerGridState Capture()
01179:         {
01180:             var copy = new PowerGridState
01181:             {
01182:                 SimDay = Math.Max(0, SimDay),
01183:                 GenerationWatts = SanitizeNonNegativeFinite(GenerationWatts),
01184:                 FuelUnits = SanitizeNonNegativeFinite(FuelUnits),
01185:                 BatteryReserveWh = SanitizeNonNegativeFinite(BatteryReserveWh),
01186:                 BatteryCapacityWh = SanitizeNonNegativeFinite(BatteryCapacityWh),
01187:                 ClosedBreakers = new List<string>(),
01188:                 TrippedRooms = new List<string>(),
01189:                 Priorities = new List<RoomPriorityRecord>(),
01190:                 LastSurgeDay = Math.Max(0, LastSurgeDay),
01193:                     0,
01194:                     PowerGridSystem.MaxInstalledBatteryBanks),
01195:                 GeneratorCondition = IsFinite(GeneratorCondition)
01196:                     ? Math.Clamp(GeneratorCondition, 0f, 100f)
01197:                     : 0f,
01198:                 InstalledCoatedPartItemIds = new List<string>()
01199:             };
01200:             if (copy.BatteryReserveWh > copy.BatteryCapacityWh)
01215:                 foreach (var priority in Priorities)
01216:                 {
01217:                     if (priority == null || string.IsNullOrWhiteSpace(priority.RoomId)) continue;
01218:                     copy.Priorities.Add(new RoomPriorityRecord
01219:                     {
01220:                         RoomId = priority.RoomId,
01221:                         Priority = priority.Priority
01231:         }
01232:
01233:         public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms)
01234:         {
01235:             if (state == null) throw new ArgumentNullException(nameof(state));
01236:             var restored = state.Capture();
01237:             SimDay = restored.SimDay;
01238:             GenerationWatts = restored.GenerationWatts;
01239:             FuelUnits = restored.FuelUnits;
01240:             BatteryReserveWh = restored.BatteryReserveWh;
01241:             BatteryCapacityWh = restored.BatteryCapacityWh;
01242:             LastSurgeDay = restored.LastSurgeDay;
01243:             InstalledBatteryBankCount = restored.InstalledBatteryBankCount;
01244:             GeneratorCondition = restored.GeneratorCondition;
01245:             ClosedBreakers = restored.ClosedBreakers;
01246:             TrippedRooms = restored.TrippedRooms;
01247:             Priorities = restored.Priorities;
01248:             InstalledCoatedPartItemIds = restored.InstalledCoatedPartItemIds;
01249:             NormalizeAndValidate(rooms);
01250:         }
01251:
01252:         internal static float SanitizeNonNegativeFinite(float value) =>
01253:             IsFinite(value) && value > 0f ? value : 0f;
01254:
01255:         private static bool IsFinite(float value) =>
01256:             !float.IsNaN(value) && !float.IsInfinity(value);
01263:                 string roomId = values[i];
01264:                 if (string.IsNullOrWhiteSpace(roomId)
01265:                     || !validIds.Contains(roomId)
01266:                     || !seen.Add(roomId))
01267:                 {
01268:                     values.RemoveAt(i);
01269:                 }
01279:                 if (priority == null
01280:                     || string.IsNullOrWhiteSpace(priority.RoomId)
01281:                     || !validIds.Contains(priority.RoomId)
01282:                     || !seen.Add(priority.RoomId))
01283:                 {
01284:                     Priorities.RemoveAt(i);
01285:                     continue;
01286:                 }
01287:                 if (!Enum.IsDefined(typeof(PowerGridRoomPriority), priority.Priority))
01288:                     priority.Priority = PowerGridRoomPriority.Standard;
01289:             }
01297:             foreach (var itemId in InstalledCoatedPartItemIds)
01298:             {
01299:                 if (string.IsNullOrWhiteSpace(itemId) || !seenItems.Add(itemId)) continue;
01300:                 string family = PowerGridSystem.ResolveCoatedPartFamily(itemId);
01301:                 if (string.IsNullOrEmpty(family) || !seenFamilies.Add(family)) continue;
01302:                 normalized.Add(itemId);
01303:             }
01304:             InstalledCoatedPartItemIds = normalized;
01305:         }
01307:
01308:     [Serializable]
01309:     public sealed class RoomPriorityRecord
01310:     {
01311:         public string RoomId;
01312:         public PowerGridRoomPriority Priority;
01313:     }
01314:
01315:     [Serializable]
01316:     public sealed class PowerGridEvent
01317:     {
01318:         public PowerGridEventKind Kind;
01319:         public string RoomId;
01320:         public int Day;
01321:         public string Detail;
01322:         public float Numeric;
01323:
01324:         public PowerGridEvent() { }
01325:
01326:         public PowerGridEvent(PowerGridEventKind kind, string roomId, int day,
01327: string? detail = null, float numeric = 0f)
01328:         {
01329:             Kind = kind;
01330:             RoomId = roomId ?? string.Empty;
01335:     }
01336:
01337:     public enum PowerGridEventKind
01338:     {
01339:         BreakerToggled,
01340:         PriorityChanged,
01341:         FuelAdded,
01342:         Tripped,
01343:         SurgeApplied,
01344:         GenerationChanged,
01345:         BatteryBankInstalled,
01346:         LoadRoomRegistered,
01347:         GeneratorMaintained,
01348:         GeneratorWorn,
01349:         FuelStarved,
01350:         TickSummary
01351:     }
01352:
01353:     [Serializable]
01354:     public sealed class PowerGridTickSummary
01355:     {
01356:         public int Day;
01357:         public float FuelConsumed;
01358:         public float BatteryEndWh;
01359:         public float BrownoutHours;
01360:         public bool IsBrownout;
01361:
01362:         // ---- Phase 2 (B5–B8) additive fields: allocation projection + edges.
01363:         // Not persisted; consumers read them via OnTickSummary only.
01364:
01365:         /// <summary>Generation actually available this tick (after any fuel
01366:         /// starvation adjustment), including external contributions.</summary>
01367:         public float GenerationWatts;
01368:
01369:         /// <summary>Intent draw of all eligible rooms (same number the legacy
01370:         /// aggregate math uses).</summary>
01371:         public float RequestedDrawWatts;
01372:
01373:         /// <summary>Watts of room load served under deterministic priority
01374:         /// allocation (generation + sustainable battery discharge).</summary>
01375:         public float ServedWatts;
01376:
01377:         /// <summary>RequestedDrawWatts − ServedWatts; the shed load.</summary>
01378:         public float UnservedWatts;
01379:
01380:         /// <summary>True when at least one Critical-tier room is unserved —
01381:         /// the explicit life-support emergency state, distinct from an ordinary
01382:         /// brownout where only lower-priority loads shed.</summary>
01383:         public bool HasCriticalDeficit;
01384:
01385:         /// <summary>Edge: brownout began this tick (false on a restored
01386:         /// campaign that was already in brownout — transitions never replay).</summary>
01387:         public bool BrownoutBegan;
01388:
01389:         /// <summary>Edge: brownout ended this tick.</summary>
01390:         public bool BrownoutEnded;
01391:
01392:         /// <summary>Room IDs served this tick, in deterministic allocation order
01393:         /// (priority tier descending, RoomId ordinal ascending).</summary>
01394:         public List<string> ServedRoomIds = new List<string>();
01395:
01396:         /// <summary>Room IDs shed this tick, in the same deterministic order.</summary>
01397:         public List<string> ShedRoomIds = new List<string>();
01398:     }
01399:
01400:     [Serializable]
01401:     public sealed class PowerGridSnapshot
01402:     {
01403:         public int Day;
01404:         public float GenerationWatts;
01405:         public float FuelUnits;
01406:         public float BatteryReserveWh;
01407:         public float BatteryCapacityWh;
01408:         public float TotalDrawWatts;
01409:         public float NetWatts;
01410:         public bool IsBrownout;
01411:         public List<string> RoomIds = new List<string>();
01412:         public Dictionary<string, float> GenerationContributions =
01413:             new Dictionary<string, float>(StringComparer.Ordinal);
01414:     }
01415: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is the shelter’s temporal operating rhythm and its truthful projection into phase, power, rest and duty consumers. The plan expands the current schedule owner’s boundary and verification depth without creating a parallel calendar.**.

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
