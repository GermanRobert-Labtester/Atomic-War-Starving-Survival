# Plan 24 — Radio Schedule, Distress Rescue, and Signal-Intelligence Architecture

> **Rebuild status:** SEALED CORE RUNTIME — CONSERVATION, REACHABILITY, AND NON-REOPENING PLAN
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** the 150k–170k band is a completeness checkpoint, never a reason to add filler. This plan is allowed to trim below the band if the verified architecture is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The current radio catalog has 87 broadcasts, six stations and two authored programs. `RadioScheduleCoordinator` resolves station state, appointment programming and dynamic alerts deterministically.
- The distress runtime already owns staged missions, authenticity, trust, follow-up, salvage, dispatch association, persistence and exactly-once receipts. Its content is sealed by current governance.
- The legitimate remaining work is to verify ordinary broadcasts, faction/verdict/year-of-ash feeds, player programs and number-station content remain distinct, reachable and restore-safe.

**Bounded outcome:** The schedule, station, program, distress mission, authenticity, trust, follow-up, signal log and audio surfaces already exist and the distress content surface is sealed. This plan becomes a preservation and verification plan. It must not add distress scenarios, a second schedule, or a second rescue manager.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Distress runtime and replay/persistence suites are extensive and the content seal is explicit.
- Signal trust availability consumer is retired; trust remains useful only through existing runtime edges.
- Faction radio, verdict radio and year-of-ash radio are separate content families.
- The schedule coordinator injects weather, orbital, disease, route, foundry, treaty and trapping alerts.
- Audio cue and accessibility surfaces use current registries.

**Master-authority sections applied to this rebase:**

- Volumes 2–3 radio seeds
- Volume 36 seal guard
- Volume 49 catalog relationship correction

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Retire the plan as a request to create schedule/rescue architecture; those owners exist.
- Audit frequency/day ownership across ordinary, faction, verdict, Year-of-Ash, distress and player programming.
- Preserve sealed content boundaries and exactly-once runtime tests.
- Improve only truthful missing surface states proven by audits.

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
| ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs` | Sole schedule resolver. |
| station identity/frequency/state | RadioStationCatalog | `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` | Station authority. |
| rescue mission lifecycle and receipts | DistressRescueMissionManager | `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` | Sole rescue mission creator. |
| signal definitions, stages and outcomes | RadioDistressSystem | `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs` | Sealed content surface. |
| tuning, playback, cooldown and cues | Radio host/audio | `src/Host/RadioHostSession.cs; src/Audio/AudioEventBridge.cs` | Presentation and playback authority. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Radio Schedule, Distress Rescue, and Signal-Intelligence Architecture
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ RadioScheduleCoordinator
│   ordinary schedule and dynamic alert precedence
│ RadioStationCatalog
│   station identity/frequency/state
│ DistressRescueMissionManager
│   rescue mission lifecycle and receipts
│ RadioDistressSystem
│   signal definitions, stages and outcomes
│ Radio host/audio
│   tuning, playback, cooldown and cues
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

1. **Preserve current state ownership.** RadioScheduleCoordinator owns ordinary schedule and dynamic alert precedence: Sole schedule resolver.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs` | Sole schedule resolver. |
| station identity/frequency/state | RadioStationCatalog | `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` | Station authority. |
| rescue mission lifecycle and receipts | DistressRescueMissionManager | `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` | Sole rescue mission creator. |
| signal definitions, stages and outcomes | RadioDistressSystem | `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs` | Sealed content surface. |
| tuning, playback, cooldown and cues | Radio host/audio | `src/Host/RadioHostSession.cs; src/Audio/AudioEventBridge.cs` | Presentation and playback authority. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load station/broadcast/program catalogs
2. resolve frequency and station state
3. apply dynamic alert precedence
4. evaluate appointment/broadcast schedule
5. surface typed transmission result
6. for distress, run existing mission lifecycle
7. capture current owner saves

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Schedule state remains transient except station/program state.
- Distress mission/fingerprint/receipts persist in the existing radio save.
- Player programs persist through their current owner.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- No distress signal row is added by ordinary schedule work.
- No two content families claim the same schedule slot without a documented precedence rule.
- Mission resolution is idempotent after reload.
- Audio never drives gameplay.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Maintain current radio, station, program and sealed distress catalogs.
- New ordinary broadcasts require station/frequency/day validity.
- Number-station content must not impersonate distress content.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use current radio save and program owner.
- No new rescue or schedule save section.
- Fingerprint/receipt guards remain mandatory.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Schedule uses day integers and injected RNG.
- Distress replay is byte-identical under same seed/commands.
- No wall-clock radio timing decides state.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Schedule resolution returns a typed result.
- Distress lifecycle emits stage and consequence facts.
- Audio playback is one-way from game facts.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/RadioHostSession.cs
- src/UI/RadioPanel.cs
- src/Audio/AudioEventBridge.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Ordinary, faction, verdict and number-station voices remain distinct.
- Distress authoring remains closed.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A new scheduler shadows RadioScheduleCoordinator. | RadioScheduleCoordinator | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Frequency collisions are unresolved. | RadioStationCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A distress outcome fires twice after restore. | DistressRescueMissionManager | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A new distress scenario bypasses the seal. | RadioDistressSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | An audio cue failure suppresses text and gameplay facts. | Radio host/audio | Focused negative test or static source gate; no broad-suite dependency. |
| F-06 | Availability math is reactivated as a second weighting system. | RadioScheduleCoordinator | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioScheduleCoordinatorTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RescueSignalDeterministicReplayTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressSignalContentUtilizationTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/FactionRadioCorpusTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest`; do not add or alter distress content under this plan.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — sealed-runtime census | Inventory current owners, saves and tests. | No reopened surface. | No production path until the owning implementation package is separately claimed. |
| 1 — schedule collision audit | Map frequency/day/program ownership. | Every ordinary slot is intentional. | No production path until the owning implementation package is separately claimed. |
| 2 — family boundary audit | Separate faction/verdict/Year-of-Ash/distress/number-station content. | No vocabulary spillover. | No production path until the owning implementation package is separately claimed. |
| 3 — player-surface polish | Improve empty/error/audio-fallback states only. | No mechanics change. | No production path until the owning implementation package is separately claimed. |
| 4 — seal regression | Keep replay, receipt and content-seal tests green. | Seal remains durable. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs | READ; MODIFY only through radio integration claim | Schedule owner |
| Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs | READ ONLY; sealed runtime | Rescue owner |
| Assets/StreamingAssets/Data/radio_distress_signals.json | FROZEN | Sealed content |
| src/UI/RadioPanel.cs | READ; MODIFY only for truth/accessibility gap | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Accidentally reopening distress content. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Parallel schedule authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Breaking exactly-once persistence. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Conflating availability and trust. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No distress rows.
- No new mission manager.
- No new schedule authority.
- No audio-to-gameplay edge.

# 23. Rollback and Recovery

- Ordinary schedule/UI changes are isolated.
- Sealed runtime changes are prohibited in this package.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Seal is explicit.
- Current schedule/rescue owners are named.
- Collision and boundary audits are defined.
- No new distress content is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Retire the plan as a request to create schedule/rescue architecture; those owners exist.
- Audit frequency/day ownership across ordinary, faction, verdict, Year-of-Ash, distress and player programming.
- Preserve sealed content boundaries and exactly-once runtime tests.
- Improve only truthful missing surface states proven by audits.

## MUST NOT DO

- No distress rows.
- No new mission manager.
- No new schedule authority.
- No audio-to-gameplay edge.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioScheduleCoordinatorTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RescueSignalDeterministicReplayTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressSignalContentUtilizationTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/FactionRadioCorpusTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest`; do not add or alter distress content under this plan.

## FIRST SAFE IMPLEMENTATION STEP

0 — sealed-runtime census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: ordinary schedule and dynamic alert precedence → RadioScheduleCoordinator; station identity/frequency/state → RadioStationCatalog; rescue mission lifecycle and receipts → DistressRescueMissionManager; signal definitions, stages and outcomes → RadioDistressSystem; tuning, playback, cooldown and cues → Radio host/audio. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 24.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 24 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by RadioScheduleCoordinator or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs`

### `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 426 lines / 20067 bytes.
- SHA-256: `185d7d64597acb2c26381516c3fb4926aaafd2bc225994a0b874ec213e4a1352`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ScheduledBroadcastResult
public bool HasTransmission { get; set; }
public float FrequencyMhz { get; set; }
public string StationId { get; set; } = string.Empty;
public string StationName { get; set; } = string.Empty;
public string SourceName { get; set; } = string.Empty;
public string Headline { get; set; } = string.Empty;
public string Message { get; set; } = string.Empty;
public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
public SourceReliability Reliability { get; set; } = SourceReliability.Official;
public BroadcastPriority Priority { get; set; } = BroadcastPriority.Routine;
public int SignalStrength { get; set; } = 5; // S-units 1..9
public float VuStrength { get; set; } = 0.5f; // 0..1
public string AudioCue { get; set; } = string.Empty;
public bool IsEmergency { get; set; }
public bool IsSilence { get; set; }
public bool IsJammed { get; set; }
public string BroadcastId { get; set; } = string.Empty;
public static ScheduledBroadcastResult StaticDeadAir(float freqMhz, string silenceMsg = "STATIC... [ No carrier detected on frequency. ] ...STATIC") {
public sealed class RadioScheduleCoordinator
public RadioBroadcastCatalog Catalog => _catalog;
public RadioStationCatalog Stations => _stations;
public IReadOnlyList<AppointmentProgramDefinition> AppointmentPrograms => _appointmentPrograms;
public void InjectWeatherAlert(string? alertMessage) => _severeWeatherAlert = alertMessage;
public void InjectOrbitalAlert(string? alertMessage) => _orbitalHarrowAlert = alertMessage;
public void InjectDiseaseAlert(string? alertMessage) => _diseaseOutbreakAlert = alertMessage;
public void InjectRouteAlert(string? alertMessage) => _routeDisruptionAlert = alertMessage;
public void InjectFoundryAlert(string? alertMessage) => _foundryStrikeAlert = alertMessage;
public void InjectTreatyAlert(string? alertMessage) => _treatyAlert = alertMessage;
public void InjectTrappingAlert(string? alertMessage) => _trappingAlert = alertMessage;
public bool HasTrappingAlert => !string.IsNullOrEmpty(_trappingAlert);
public void ClearDynamicAlerts() {
public ScheduledBroadcastResult Resolve(float frequencyMhz, int day, ISeededRng rng, float toleranceMhz = 0.5f) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

### `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 162 lines / 5893 bytes.
- SHA-256: `b2c2eff4efe46878bdcaa00a6c2f059d38c78fe15905c85b639d0a61696b2ac2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioStationCatalog
public const string StationCivilDefense = "station_civil_defense";
public const string StationGarrisonOverlord = "station_garrison_overlord";
public const string StationVitrifiedCrater = "station_vitrified_crater";
public const string StationOpenClassroom = "station_open_classroom";
public const string StationNumbersSigint = "station_numbers_sigint";
public const string StationAutomatedRelay = "station_automated_relay";
public IReadOnlyCollection<RadioStationDefinition> AllStations => _stations.Values;
public void Clear() {
public int LoadFromJson(string json) {
public int LoadFromDataDirectory(string dataDir) {
public void Register(RadioStationDefinition def) {
public RadioStationDefinition? GetStation(string stationId) {
public RadioStationDefinition? FindStationAtFrequency(float frequencyMhz, float toleranceMhz = 0.5f) {
public RadioStationState GetStationState(string stationId) {
public void SetStationState(string stationId, RadioStationState state) {
public void ResetOverrides() {
public Dictionary<string, RadioStationState> ExportOverrides() {
public void ImportOverrides(IDictionary<string, RadioStationState>? overrides) {
public RadioProgramSlot? GetCurrentSlot(string stationId, int campaignDay, int hour) {
public RadioProgramSlot? GetNextSlot(string stationId, int campaignDay, int hour) {
public RadioSignalStrength ComputeSignalStrength(string stationId, RadioReceptionFactors? factors) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs`

### `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 910 lines / 47252 bytes.
- SHA-256: `bfc2cc756f42478aee3acad8bee2d6c61e086e924e8b9dfd5dbce3850d265e4c`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=16; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DistressRescueMissionStage
public sealed class DistressRescueMission
public string QuestId { get; set; } = string.Empty;
public string SignalId { get; set; } = string.Empty;
public string DestinationId { get; set; } = string.Empty;
public string ExpeditionId { get; set; } = string.Empty;
public DistressRescueMissionStage Stage { get; set; } = DistressRescueMissionStage.None;
public int InterceptedDay { get; set; }
public int DaysToTrace { get; set; } = 3;
public int DeadlineDays { get; set; } = 5;
public int ExpiryDay => InterceptedDay + DeadlineDays;
public bool SenderAlive { get; set; } = true;
public int SenderDeathDay { get; set; }
public int SenderSurvivalDays { get; set; }
public bool Expired { get; set; }
public bool IgnoreConsequenceApplied { get; set; }
public int IgnoreConsequenceAppliedDay { get; set; } = -1;
public List<string> IgnoreConsequenceTokens { get; set; } = new List<string>();
public string FactionStandingLossFactionId { get; set; } = string.Empty;
public bool ArrivalResolved { get; set; }
public bool AuthenticityChecked { get; set; }
public int AuthenticityAssessment { get; set; }
public bool AssessmentThreatDetected { get; set; }
public bool AssessmentStalenessDetected { get; set; }
public string AssessmentSkillId { get; set; } = string.Empty;
public bool IsTerminal => Stage == DistressRescueMissionStage.TerminalRescued ||
public bool IsTrap { get; set; }
public string FactionTag { get; set; } = string.Empty;
public List<string> RewardItems { get; set; } = new List<string>();
public int RewardReputation { get; set; }
public bool RewardClaimed { get; set; }
public bool ReputationClaimed { get; set; }
public string OutcomeSummary { get; set; } = string.Empty;
public sealed class DistressMissionSaveState
public int Version { get; set; } = 1;
public List<DistressRescueMission> Missions { get; set; } = new List<DistressRescueMission>();
public List<string> ClaimedReceipts { get; set; } = new List<string>();
public string missionsFingerprint = string.Empty;
public static string ComputeFingerprint(DistressMissionSaveState state) {
public void RefreshFingerprint() => missionsFingerprint = ComputeFingerprint(this);
public sealed class DistressRescueMissionManager
public const string SystemId = "distress_rescue_mission_manager";
public event Action<DistressRescueMission, DistressRescueMissionStage>? OnStageChanged;
public event Action<DistressRescueMission, List<string>, int>? OnRewardsGranted;
public event Action<DistressRescueMission, List<string>, string, int>? OnIgnoreConsequence;
public static readonly string[] KnownConsequenceTokens = { "sender_death", "faction_standing_loss", "faction_ambush" };
public static bool AreConsequenceTokensValid(IEnumerable<string>? tokens) {
public IReadOnlyCollection<DistressRescueMission> AllMissions => _missionsByQuest.Values;
public DistressRescueMission? GetMissionByQuest(string questId) {
public DistressRescueMission? GetMissionBySignal(string signalId) {
public DistressRescueMission? GetActiveMissionByDestination(string destinationId) {
public DistressRescueMission? GetActiveMissionForDispatch(string destinationId) {
public DistressRescueMission? GetMissionForArrival(string destinationId, string expeditionId) {
public bool IsReceiptClaimed(string questId, string signalId) {
public bool RecordSignalHeard(string signalId, int day) {
public SignalAuthenticityCheckResult? RecordAuthenticityCheck( string signalId, string survivorId, int day, int seedBase, Func<string, string, bool>? hasSkill = null)
public bool RecordSignalIdentified(string signalId) {
public bool RecordExpeditionDispatched(string questId, string expeditionId) {
public DistressRescueMissionStage RecordDestinationReached(string questId, int currentDay) {
public bool ResolveAmbushSurvived(string questId, string outcomeNotes = "Ambush repelled.") {
public void TickDaily(int currentDay) {
public RescueDispatchPreflight? GetDispatchPreflight(string signalId) {
public DistressMissionSaveState CaptureState() {
public void RestoreState(DistressMissionSaveState? state) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs`

### `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 824 lines / 36118 bytes.
- SHA-256: `f930dcdc13de2fec6f945800c0412382c27da3817ddacc1b889a2672f948ad2d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DistressSignalStatus
public sealed class DistressMessageFragment
public int Day { get; set; }
public float Clarity { get; set; }
public string Text { get; set; } = string.Empty;
public string OutcomeHint { get; set; } = string.Empty;
public string AudioCue { get; set; } = string.Empty;
public sealed class DistressSignalDefinition
public string FrequencyId { get; set; } = string.Empty;
public string FrequencyMhzStr { get; set; } = "100.0";
public string SourceName { get; set; } = string.Empty;
public string OutcomeTypeStr { get; set; } = "survivor_isolated";
public string OutcomeType => OutcomeTypeStr;
public string Authenticity { get; set; } = string.Empty;
public string ToneRegister { get; set; } = string.Empty;
public int DaysToTrace { get; set; } = 4;
public int? DeadlineDaysSnake { get; set; }
public int? DeadlineDaysCamel { get; set; }
public int? SenderSurvivalDaysSnake { get; set; }
public int? SenderSurvivalDaysCamel { get; set; }
public string? IgnoreConsequenceSnake { get; set; }
public string? IgnoreConsequenceCamel { get; set; }
public string WarningText { get; set; } = string.Empty;
public string RevealedLocation { get; set; } = string.Empty;
public string? LocationReferenceSnake { get; set; }
public string? LocationReferenceCamel { get; set; }
public string RevealedKnowledge { get; set; } = string.Empty;
public int KnowledgePoints { get; set; }
public List<string> RevealedItems { get; set; } = new List<string>();
public List<DistressMessageFragment> MessageFragments { get; set; } = new List<DistressMessageFragment>();
public string AudioCue { get; set; } = string.Empty;
public List<SignalFollowUpDefinition> FollowUpSignals { get; set; } = new List<SignalFollowUpDefinition>();
public string NarrativeId { get; set; } = string.Empty;
public string RecruitSurvivorId { get; set; } = string.Empty;
public string SenderFactionId { get; set; } = string.Empty;
public string DeceptiveFactionId { get; set; } = string.Empty;
public string MoralChoiceId { get; set; } = string.Empty;
public string ReputationFactionId { get; set; } = string.Empty;
public int ReputationDelta { get; set; } = 15;
public string NpcId { get; set; } = string.Empty;
public string ResolveQuestId { get; set; } = string.Empty;
public bool IsTrapOrDeception =>
public bool IsAutomated =>
public bool IsGenuineRescue =>
public bool IsGrimOrMemorial =>
public sealed class ActiveDistressSignal
public string SignalId { get; set; } = string.Empty;
public DistressSignalStatus Status { get; set; } = DistressSignalStatus.Inactive;
public int InterceptedDay { get; set; }
public int DaysRemaining { get; set; }
public float HighestClarity { get; set; }
public bool IsTriangulated { get; set; }
public bool IsDispatched { get; set; }
public bool IsResolved { get; set; }
public string ResolutionSummary { get; set; } = string.Empty;
public bool IsMoralChoiceAvailable { get; set; }
public int MoralChoiceResolutionIndex { get; set; } = -1;
public bool IsIgnored { get; set; }
public sealed class RadioDistressSystem
public const string SystemId = "radio_distress_system";
public event Action<DistressSignalDefinition, ActiveDistressSignal>? OnSignalIntercepted;
public event Action<DistressSignalDefinition, ActiveDistressSignal>? OnSignalTriangulated;
public event Action<DistressSignalDefinition, ActiveDistressSignal>? OnSignalExpired;
public event Action<DistressSignalDefinition, ActiveDistressSignal, string>? OnSignalResolved;
public Func<string, bool>? NpcSignalSuppressionFilter { get; set; }
public IReadOnlyCollection<DistressSignalDefinition> Definitions => _definitions.Values;
public IReadOnlyCollection<ActiveDistressSignal> ActiveSignals => _activeSignals.Values;
public int TotalRegisteredSignals => _definitions.Count;
public void RegisterSignal(DistressSignalDefinition def) {
public DistressSignalDefinition? GetDefinition(string signalId) {
public DistressSignalDefinition? GetByExactFrequency(float freqMhz) {
public ActiveDistressSignal? GetActiveState(string signalId) {
public DistressSignalDefinition? FindSignalAtFrequency(float freqMhz, float toleranceMhz = 0.5f) {
public int LoadFromDataDirectory(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public bool TryTriggerMoralChoice(string signalId, out string moralChoiceId) {
public bool ResolveMoralChoice( string signalId, int choiceIndex, MoralChoiceSystem moral, MoralChoiceQuestDefinition questDef, int day,
public bool CompleteRescue(string signalId, FactionWarSystem? factionWar = null) {
public bool Intercept(string signalId, int day) {
public bool MarkTriangulated(string signalId) {
public bool DispatchExpedition(string signalId) {
public bool Resolve(string signalId, DistressSignalStatus resolutionStatus, string summary) {
public void TickDaily(int currentDay) {
public int LoadFromJson(string json) {
public List<DistressSignalSaveEntry> CaptureState() {
public void RestoreState(List<DistressSignalSaveEntry>? savedEntries) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs`

### `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 178 lines / 8471 bytes.
- SHA-256: `5a3960800e27ccaa6a558316ab7cf313894ec087e3967f8d2f300c9079f3f8db`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SignalTrustPolicy
public const int MinScore = 0;
public const int MaxScore = 100;
public const int NeutralScore = 50;
public const int DeltaAnswered = +2;
public const int DeltaRescueSuccessful = +5;
public const int DeltaIgnored = -2;
public const int DeltaAmbushEncountered = -5;
public static int Clamp(int score) => Math.Clamp(score, MinScore, MaxScore);
public sealed class SignalTrustSaveEntry
public int signalsAnswered;
public int signalsIgnored;
public int trapsFallenFor;
public int rescuesSuccessful;
public int score = SignalTrustPolicy.NeutralScore;
public List<string> answeredSignalIds = new List<string>();
public List<string> ignoredSignalIds = new List<string>();
public List<string> trapSignalIds = new List<string>();
public List<string> rescueSignalIds = new List<string>();
public sealed class SignalTrustLedger
public int SignalsAnswered { get; private set; }
public int SignalsIgnored { get; private set; }
public int TrapsFallenFor { get; private set; }
public int RescuesSuccessful { get; private set; }
public int Score { get; private set; } = SignalTrustPolicy.NeutralScore;
public bool RecordAnswered(string signalId) {
public bool RecordIgnored(string signalId) {
public bool RecordAmbushEncountered(string signalId) {
public bool RecordRescueSuccessful(string signalId) {
public SignalTrustSaveEntry CaptureState() {
public void RestoreState(SignalTrustSaveEntry? entry) {
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioSignalLog.cs`

### `Assets/Ashfall.Core/Radio/RadioSignalLog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 148 lines / 5066 bytes.
- SHA-256: `95555f631b5161673bebdc29660831f1742edf429ca9617564eacea55478dabc`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioSignalLog
public event Action<SignalLogEntry>? OnSignalLogged;
public event Action<string>? OnStationDiscovered;
public IReadOnlyCollection<SignalLogEntry> Entries => _logEntries.Values;
public IReadOnlyCollection<string> DiscoveredStations => _discoveredStations;
public IReadOnlyList<float> Presets => _customPresets;
public bool DiscoverStation(string stationId) {
public bool IsStationDiscovered(string stationId) {
public void AddPreset(float frequencyMhz) {
public void RemovePreset(float frequencyMhz) {
public SignalLogEntry LogIntercept(ScheduledBroadcastResult broadcast, int day) {
public List<SignalLogEntry> CaptureEntries() {
public List<string> CaptureDiscoveredStations() {
public List<float> CapturePresets() {
public void RestoreState(List<SignalLogEntry>? entries, List<string>? discoveredStations, List<float>? presets) {
```


# Appendix B.08 — Current Code Architecture: `src/Host/RadioHostSession.cs`

### `src/Host/RadioHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 785 lines / 37883 bytes.
- SHA-256: `fa3dcecb7e92d69fd292ca9bd06e75d24e57f730ade13ba7b99e7c21fd6b93d8`.
- Architecture signals: seeded references=4; save/restore symbols=16; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioHostSession
public const int DemoSeed = 2026;
public event Action<RadioIntercept, string?>? BroadcastIntercepted;
public FactionRadioEngine Engine { get; }
public SignalTriangulationSystem Triangulation { get; }
public RadioBroadcastCatalog BroadcastCatalog { get; }
public RadioStationCatalog Stations { get; }
public RadioScheduleCoordinator ScheduleCoordinator { get; }
public RadioDistressSystem DistressSystem { get; }
public RadioRecordingSystem RecordingSystem { get; }
public RadioSignalLog SignalLog { get; }
public DistressRescueMissionManager RescueMissions { get; }
public SignalTrustLedger SignalTrust { get; }
public DistressFollowUpScheduler FollowUps { get; }
public ISeededRng Rng { get; }
public IReadOnlyList<RadioIntercept> History => _history;
public int Day { get; private set; }
public float CurrentFrequency { get; private set; }
public RadioIntercept? LastIntercept { get; private set; }
public ScheduledBroadcastResult? LastScheduledBroadcast { get; private set; }
public string LastEvent { get; private set; } = string.Empty;
public Func<string>? WeatherConditionProvider { get; set; }
public Func<WeatherKind>? WeatherKindProvider { get; set; }
public RadioReceiverBand CurrentBand => RadioReceiverPlan.GetBandForFrequencyMhz(CurrentFrequency);
public void SetBand(string bandId) {
public void CycleBand() {
public static RadioHostSession Create(string dataDir, int day = 1, ICampaignRngManager? campaignRng = null) {
public void SetDay(int day) {
public string Listen(float? frequencyMhz = null) {
public string BroadcastBeacon(string customMessage = "Holdfast shelter holding. Awaiting courier contact.") {
public void TuneDelta(float deltaMhz) {
public bool RecordBearingObservation(float bearingDegrees) {
public TriangulationCandidate? TriangulateCurrentSignal() {
public string RecordMarketRumor(string message, int day) {
public string InterceptWarlordWarning(string message, int day) {
public string RecordCulturalBroadcast(string recordId, string genre, string displayName, int day, float signalStrength) {
public bool HasPlayed(RadioIntercept intercept) {
public RadioSaveState CaptureSave() {
public void RestoreSave(RadioSaveState state) {
public string ActiveStationId { get; set; } = "station_alpha";
public void SetActiveStation(string stationId) {
public string RecordObservation(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f, string? stationId = null) {
public string RecordObservationDemo(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f) => RecordObservation(signalId, bearing, signalStrength, noise);
public string TriangulateSignal(string signalId) {
public string TriangulateDemo(string signalId) => TriangulateSignal(signalId);
public string TriangulationStatusLine(string signalId) {
public string StatusLine() {
public RadioProgramSlot? GetCurrentSlot(string stationId, int? hour = null) {
public RadioProgramSlot? GetNextSlot(string stationId, int? hour = null) {
public RadioSignalStrength GetSignalStrength(string stationId, RadioReceptionFactors? factors = null) {
public RadioStationDefinition? GetStationAtCurrentFrequency() {
```


# Appendix B.09 — Current Code Architecture: `src/UI/RadioPanel.cs`

### `src/UI/RadioPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 700 lines / 33756 bytes.
- SHA-256: `38a56568e4a4b51ea77c9054ff60cbd6fab8ea2771bf0803089a6959663b937e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class RadioPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action? OnRadioBroadcastSent;
public bool IsBound => _radioHost != null;
public bool IsProductionBound => _productionHost != null;
public int RenderedSignalCount => _interceptsGrid?.RowCount ?? 0;
public void Bind(RadioHostSession radio) {
public void BindProduction(RadioProgramProductionHostSession production) {
public void Unbind() {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix B.10 — Current Code Architecture: `src/Audio/AudioEventBridge.cs`

### `src/Audio/AudioEventBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 633 lines / 26686 bytes.
- SHA-256: `840935ff49fb41437d3afaf2892e049e4a0f298cc25ae16650b66df0c07139e7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IAudioDomainProvider
public sealed class AudioEventBridge : IDisposable
public void SubscribeAll( RadiationSystem? radiation = null, WeatherSystem? weather = null, TacticalCombatSystem? combat = null, CraftingSystem? crafting = null, ExpeditionSystem? expeditions = null,
public void BindRadiation(RadiationSystem? radiation) {
public void BindWeather(WeatherSystem? weather) {
public void BindCombat(TacticalCombatSystem? combat) {
public void BindCrafting(CraftingSystem? crafting) {
public void BindExpeditions(ExpeditionSystem? expeditions) {
public void BindDisease(DiseaseSystem? disease) {
public void BindSurvivorFate(SurvivorFateSystem? survivorFate) {
public void BindFlashbacks(SomaticFlashbackSystem? flashbacks) {
public void BindEchoes(EchoSystem? echoes) {
public void NotifyGameFlow(string cueId) {
public void Dispose() {
internal bool HasRadiationBinding => _radiation != null;
internal bool HasWeatherBinding => _weather != null;
internal bool HasCombatBinding => _combat != null;
internal bool HasCraftingBinding => _crafting != null;
internal bool HasExpeditionsBinding => _expeditions != null;
internal bool HasDiseaseBinding => _disease != null;
internal bool HasSurvivorFateBinding => _survivorFate != null;
internal bool HasFlashbacksBinding => _flashbacks != null;
internal bool HasEchoesBinding => _echoes != null;
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/radio.json`

### `Assets/StreamingAssets/Data/radio.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 44197 bytes / 44197 characters.
- SHA-256: `2dc373817b8214fec760c82f9ecaf132e3e69a6e90ec8ead16cfaeaa547ae05c`.
- Root keys: `radio_broadcasts`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
radio_broadcasts: min=87, max=87, observed_paths=1
```

Representative record fields:

- `confidence`
- `frequency`
- `id`
- `intelType`
- `maxDay`
- `message`
- `minDay`

Representative identifiers (ordered, capped for readability):

```text
radio_broadcast_01
radio_broadcast_02
radio_broadcast_03
radio_broadcast_04
radio_broadcast_05
radio_broadcast_06
radio_broadcast_07
radio_broadcast_08
radio_broadcast_09
radio_broadcast_10
radio_broadcast_11
radio_broadcast_12
radio_broadcast_13
radio_broadcast_14
radio_broadcast_15
radio_broadcast_16
radio_broadcast_17
radio_broadcast_18
radio_broadcast_19
radio_broadcast_20
radio_broadcast_21
radio_broadcast_22
radio_broadcast_23
radio_broadcast_24
radio_broadcast_25
radio_broadcast_26
radio_broadcast_27
radio_broadcast_28
radio_broadcast_29
radio_broadcast_30
radio_broadcast_31
radio_broadcast_32
radio_broadcast_33
radio_broadcast_34
radio_broadcast_35
radio_broadcast_36
radio_broadcast_37
radio_broadcast_38
radio_broadcast_39
radio_broadcast_40
radio_broadcast_41
radio_broadcast_42
radio_broadcast_43
radio_broadcast_44
radio_broadcast_45
radio_broadcast_46
radio_broadcast_47
radio_broadcast_48
radio_broadcast_49
radio_broadcast_50
radio_broadcast_relay_count
radio_broadcast_winter_ledger
radio_broadcast_last_rotation
radio_broadcast_eco_migration_bulletin
radio_broadcast_eco_fish_run
radio_broadcast_eco_swarm_warning
radio_broadcast_eco_predator_warning
radio_broadcast_distress_trapped_mechanic
radio_broadcast_distress_injured_trader
radio_broadcast_distress_repeating_beacon
radio_broadcast_distress_raider_lure
radio_broadcast_distress_false_flag_settlement
radio_broadcast_distress_encrypted_military
radio_broadcast_distress_child_school
radio_broadcast_distress_military_patrol
radio_broadcast_crater_sermon_glass_mirror
radio_broadcast_crater_liturgy_of_counts
radio_broadcast_crater_sealed_chamber_names
radio_broadcast_crater_sermon_second_sun
radio_broadcast_crater_rim_invitation
radio_broadcast_crater_roll_call_of_dead
radio_broadcast_classroom_reading_lesson
radio_broadcast_classroom_ration_arithmetic
radio_broadcast_classroom_lineman_splice
radio_broadcast_classroom_water_lesson
radio_broadcast_classroom_valley_geography
radio_broadcast_classroom_sign_off_names
radio_broadcast_relay_teletype_ash_front
radio_broadcast_relay_battery_countdown
radio_broadcast_relay_continuity_roll
radio_broadcast_numbers_ninth_night_groups
radio_broadcast_numbers_interval_notice
radio_broadcast_numbers_municipal_codes
radio_broadcast_embargo_alert
radio_broadcast_embargo_clearing
radio_broadcast_kennel_guard
radio_broadcast_kennel_training
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/radio_stations.json`

### `Assets/StreamingAssets/Data/radio_stations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 12840 bytes / 12840 characters.
- SHA-256: `9d02071488c8ee3d1be58e3c4e568ec0d470c449aea726a3067e8499c04dd6b8`.
- Root keys: `radio_stations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
radio_stations: min=6, max=6, observed_paths=1
radio_stations[].equipment_requirements: min=1, max=1, observed_paths=2
radio_stations[].schedule: min=4, max=4, observed_paths=2
radio_stations[].tags: min=3, max=3, observed_paths=2
```

Representative record fields:

- `default_state`
- `display_name`
- `equipment_requirements`
- `frequency_mhz`
- `id`
- `jammed_text`
- `owner_faction_id`
- `persona_voice`
- `reliability`
- `schedule`
- `signal_profile_id`
- `silence_text`
- `station_id`
- `tags`

Representative identifiers (ordered, capped for readability):

```text
station_civil_defense
station_garrison_overlord
station_vitrified_crater
station_open_classroom
station_numbers_sigint
station_automated_relay
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/radio_programs.json`

### `Assets/StreamingAssets/Data/radio_programs.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 1190 bytes / 1190 characters.
- SHA-256: `a4fcb9cc7d82093e572d7ce2be418e34d1f139091dd27e6d4735d29f45e333ac`.
- Root keys: `programs`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
programs: min=2, max=2, observed_paths=1
programs[].required_equipment_item_ids: min=1, max=1, observed_paths=2
```

Representative record fields:

- `description`
- `display_name`
- `genre`
- `id`
- `prep_cost_count`
- `prep_cost_item_id`
- `prep_ticks_required`
- `psyops_campaign_id`
- `required_equipment_item_ids`
- `slot_id`
- `station_id`

Representative identifiers (ordered, capped for readability):

```text
radio_prog_shelter_morning_bulletin
radio_prog_classroom_evening_story
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/radio_distress_signals.json`

### `Assets/StreamingAssets/Data/radio_distress_signals.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 41571 bytes / 41487 characters.
- SHA-256: `2f7ba9f9d5c355f70df4a6cc7e0b1792badaf5e1920aeb35d11d9d42001e41f1`.
- Root keys: `radio_broadcasts`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
radio_broadcasts: min=25, max=25, observed_paths=1
radio_broadcasts[].follow_up_signals: min=1, max=2, observed_paths=2
radio_broadcasts[].message_fragments: min=3, max=4, observed_paths=2
radio_broadcasts[].revealed_items: min=3, max=3, observed_paths=1
```

Representative record fields:

- `audio_cue`
- `authenticity`
- `days_to_trace`
- `deadlineDays`
- `deceptive_faction_id`
- `follow_up_signals`
- `frequency_id`
- `frequency_mhz`
- `ignoreConsequence`
- `knowledge_points`
- `location_reference`
- `message_fragments`
- `moral_choice_id`
- `narrative_id`
- `outcome_type`
- `reputation_delta`
- `revealed_items`
- `revealed_knowledge`
- `revealed_location`
- `senderSurvivalDays`
- `sender_faction_id`
- `source_name`
- `warning_text`


# Appendix C.15 — Catalog Census: `Assets/StreamingAssets/Data/narrative/numbers_station_ciphers.json`

### `Assets/StreamingAssets/Data/narrative/numbers_station_ciphers.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 7924 bytes / 7922 characters.
- SHA-256: `35e43065dfca65373437f6e33f1885e81f0f2eec74e90a0aea6f22270f037f7b`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=11, max=11, observed_paths=1
items[].tags: min=5, max=5, observed_paths=2
```

Representative record fields:

- `chime_interval_seconds`
- `id`
- `modulation_mode`
- `prose`
- `station_nickname`
- `tags`
- `timestamp_relative`
- `transmission_frequency_khz`

Representative identifiers (ordered, capped for readability):

```text
cipher_station_lincolnshire_poacher_echo
cipher_station_swedish_rhapsody_musicbox
cipher_station_magnetic_tape_loop_cherry_ripe
cipher_station_buzzer_uvb_76_marker
cipher_station_backward_music_station_whistle
cipher_station_four_tone_flute_cadence
cipher_station_phonetic_alphabet_drill_sergeant
cipher_station_final_farewell_simplex_loop
cipher_station_relay_count
cipher_station_winter_ledger
cipher_station_last_rotation
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RadioScheduleCoordinatorTests.cs`

### `Ashfall.Core.Tests/Radio/RadioScheduleCoordinatorTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 116; SHA-256: `74721cf80f62e0cf20fc58406868cf2fd4ed4dcac12dc5854a2567c81ca8cde0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Resolve_ReturnsStaticDeadAir_WhenNoStationInTolerance
Resolve_ReturnsSilenceText_WhenStationIsSilent
Resolve_ReturnsJammedText_WhenStationIsJammed
DynamicWeatherAlert_OverridesRoutineBroadcastsImmediately
DynamicOrbitalAlert_InjectsEmergencyWarningOnAutomatedArray
AppointmentPrograms_AreSixCanonicalPrograms
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`

### `Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 190; SHA-256: `873b84fbde84ca0f6e6c00a9658a498f5a16173ada6a531f697cd7b397c817de`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
MissionState_RoundTrips_ThroughRadioSaveState
IgnoreConsequenceState_RoundTrips_ExactlyOnceAfterReload
V3Payload_MigratesToV4_WithNeutralMissionDefaults
LegacyManagerState_MissingNewFields_DeserializesNeutral
ChecksumCoversMissionState_TamperedMissionFailsDecode
```


# Appendix D.18 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RescueSignalDeterministicReplayTests.cs`

### `Ashfall.Core.Tests/Radio/RescueSignalDeterministicReplayTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 182; SHA-256: `37d310890e17b618500a74f3e66fb3b1e23e30eb44c1833d8ee125555ecfc30d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SameDayDeadlineAndSenderDeath_EachResolveExactlyOnce
DetectedTrap_DispatchRemainsPlayersChoice_AmbushPipelineRuns
ContinuousRun_EqualsMidReloadReplay_FieldByField
Fingerprint_TracksRuntimeState_AnalyzedRunsDivergeFromUnanalyzed
MidReloadExpiry_CausesConsequenceExactlyOnce_AtTheSameBoundary
LongRunUndiscoveredSignals_NeverPunished_NeverInitialized
```


# Appendix D.19 — Existing Focused Test Inventory: `Ashfall.Core.Tests/FactionRadioCorpusTests.cs`

### `Ashfall.Core.Tests/FactionRadioCorpusTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 176; SHA-256: `3b28055f62106ba69da337535396f164f3190bf81025a524ac2727e6184437cb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Corpus_GuildChannelResolvesNotFallback
Corpus_LoadsAllThirteenFactions
Corpus_HasAtLeastTwelveChatterLinesPerFaction_AndNoDuplicates
Corpus_ToneLint_NoModernSlangOrAnachronisms
FactionRadioEngine_TuningAndSignalStrength_CalculatesAccurately
FactionRadioEngine_DeterministicRotation_ProvableCrossProcess
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`

### `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 678 lines / 36957 bytes.
- SHA-256: `974aed9a7508a15773166e11c7fc336b5e1874935513b16cf99582742b686737`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioBroadcastCatalog
public IReadOnlyList<UnifiedRadioBroadcast> AllBroadcasts => _allBroadcasts;
public int TotalCount => _allBroadcasts.Count;
public void Register(UnifiedRadioBroadcast b) {
public UnifiedRadioBroadcast? GetById(string broadcastId) {
public List<UnifiedRadioBroadcast> GetByFrequency(float freqMhz, float toleranceMhz = 0.5f) {
public List<UnifiedRadioBroadcast> GetEligibleBroadcasts(float freqMhz, int day, float toleranceMhz = 0.5f) {
public int LoadFromDataDirectory(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public int LoadFactionRadioCorpusJson(string json) {
public void LoadBaseRadioJson(string json) {
public void LoadYearOfAshRadioJson(string json) {
public void LoadVerdictRadioJson(string json) {
public void LoadFactionWarRadioJson(string json) {
public void RegisterAuthoredGapBroadcasts() {
```


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`

### `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 429 lines / 15803 bytes.
- SHA-256: `7f51d2e6e1521e4589ffdf11ee4b62778fc2ebe4f83b638ddee1943f9c41887e`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioEncryptionScheme
public string Scheme { get; set; } = "none";
public int Difficulty { get; set; }
public List<string> RequiredSkillIds { get; set; } = new List<string>();
public sealed class RadioTriangulationData
public int RequiredBearings { get; set; } = 2;
public string RevealedLocationId { get; set; } = string.Empty;
public sealed class RadioInterceptDefinition
public string Id { get; set; } = string.Empty;
public string Callsign { get; set; } = string.Empty;
public int FrequencyKhz { get; set; } = 7000;
public string Band { get; set; } = "hf";
public string SignalClass { get; set; } = "chatter";
public string SourceFactionId { get; set; } = string.Empty;
public float BaseSignalStrength { get; set; } = 0.5f;
public RadioEncryptionScheme Encryption { get; set; } = new RadioEncryptionScheme();
public RadioTriangulationData Triangulation { get; set; } = new RadioTriangulationData();
public int ExpiryDays { get; set; } = 3;
public string Message { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public sealed class RadioInterceptCatalogData
public int SchemaVersion { get; set; } = 1;
public List<RadioInterceptDefinition> Intercepts { get; set; } = new List<RadioInterceptDefinition>();
public sealed class InterceptProgress
public string InterceptId { get; set; } = string.Empty;
public bool Detected { get; set; }
public int SignalLockPermille { get; set; }
public int DecryptProgressPermille { get; set; }
public int BearingsCollected { get; set; }
public List<int> DistinctAzimuths { get; set; } = new List<int>();
public bool Resolved { get; set; }
public bool IsDecrypted { get; set; }
public int DetectedDay { get; set; }
public int? ExpiresOnDay { get; set; }
public bool IsExpired { get; set; }
public sealed class RadioStationStateSave
public string systemId = ShelterRadioStationSystem.SystemId;
public int schemaVersion = 1;
public int tunedFrequencyKhz = 7115;
public string bandId = "hf";
public int antennaAzimuthDegrees = 0;
public bool isOperational = true;
public List<InterceptProgress> intercepts = new List<InterceptProgress>();
public List<string> discoveredLocationIds = new List<string>();
public List<string> decodedIntelligenceLogs = new List<string>();
public int currentDay;
public sealed record RadioScanResult(
public sealed class ShelterRadioStationSystem
public const string SystemId = "radio_station";
public RadioStationStateSave State => _state;
public IReadOnlyDictionary<string, RadioInterceptDefinition> Catalog => _catalog;
public event Action<string>? OnInterceptDetected;
public event Action<string>? OnInterceptDecrypted;
public event Action<string, string>? OnLocationTriangulated; // interceptId, locationId
public event Action<string>? OnDistressExpired;
public event Action<OrbitalWarningEntry>? OnOrbitalWarningRelayed;
public event Action? OnRadioStateChanged;
public void BindSkillProvider(Func<string, float> provider) => _operatorSkillProvider = provider;
public void BindWeatherNoiseProvider(Func<float> provider) => _weatherNoiseProvider = provider;
public void LoadCatalog(RadioInterceptCatalogData? data) {
public void LoadCatalog(string json) {
public void TuneTo(int frequencyKhz, string band = "hf") {
public void SetAntennaAzimuth(int degrees) {
public InterceptProgress GetOrCreateInterceptProgress(string interceptId) {
public RadioScanResult ScanFrequency(int day) {
public int ProgressDecryption(string interceptId, float skillBonus = 1.0f) {
public bool RecordBearing(string interceptId, int azimuthDegrees) {
public void TickDay(int day) {
public OrbitalWarningEntry? CheckOrbitalEarlyWarning(int currentDay) {
public RadioStationStateSave CaptureState() {
public void RestoreState(RadioStationStateSave? saved) {
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/ShelterScheduleSystem.cs`

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


# Appendix E.23 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/RadioSave.cs`

### `Assets/Ashfall.Core/Radio/RadioSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 466 lines / 23405 bytes.
- SHA-256: `c499b9afa3a4c6187e5b23d4685c6bfb654dbd5eb907043cea569fb35be7b137`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class RadioInterceptEntry
public string factionId = string.Empty;
public string callsign = string.Empty;
public float frequencyMhz;
public int kind; // RadioEventKind numeric
public string message = string.Empty;
public int signalStrength;
public int day;
public class DistressSignalSaveEntry
public string signalId = string.Empty;
public int status; // DistressSignalStatus enum numeric
public int interceptedDay;
public int daysRemaining;
public float highestClarity;
public bool isDispatched;
public bool isResolved;
public string resolutionType = string.Empty;
public bool isMoralChoiceAvailable;
public int moralChoiceResolutionIndex = -1;
public bool isIgnored;
public class SignalLogEntry
public string id = string.Empty;
public string title = string.Empty;
public string stationId = string.Empty;
public float frequencyMhz;
public int dayLogged;
public string summary = string.Empty;
public bool isDecoded;
public bool isTriangulated;
public class RecordedCassetteEntry
public string cassetteId = string.Empty;
public string broadcastId = string.Empty;
public string title = string.Empty;
public string transcript = string.Empty;
public int recordedDay;
public float frequencyMhz;
public string sourceName = string.Empty;
public string audioCue = string.Empty;
public class StationStateOverrideEntry
public string stationId = string.Empty;
public int state; // RadioStationState enum numeric
public int overrideUntilDay;
public class RadioSaveState
public int saveVersion = RadioSaveCodec.CurrentSaveVersion;
public int day;
public float currentFrequency;
public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
public List<string> playedBroadcastKeys = new List<string>();
public List<string> discoveredStationIds = new List<string>();
public List<float> customPresets = new List<float>();
public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
public TriangulationState triangulation = new TriangulationState();
public DistressMissionSaveState? rescueMissions;
public SignalTrustSaveEntry? signalTrust;
public SignalFollowUpSaveState? signalFollowUps;
public string Checksum = string.Empty;
public class RadioSaveStateFrozenV5
public int saveVersion = 5;
public int day;
public float currentFrequency;
public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
public List<string> playedBroadcastKeys = new List<string>();
public List<string> discoveredStationIds = new List<string>();
public List<float> customPresets = new List<float>();
public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
public TriangulationState triangulation = new TriangulationState();
public DistressMissionSaveState? rescueMissions;
public SignalTrustSaveEntry? signalTrust;
public string Checksum = string.Empty;
public class RadioSaveStateFrozenV4
public int saveVersion = 4;
public int day;
public float currentFrequency;
public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
public List<string> playedBroadcastKeys = new List<string>();
public List<string> discoveredStationIds = new List<string>();
public List<float> customPresets = new List<float>();
public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
public TriangulationState triangulation = new TriangulationState();
public DistressMissionSaveState? rescueMissions;
public string Checksum = string.Empty;
public class RadioSaveStateFrozenV3
public int saveVersion = 3;
public int day;
public float currentFrequency;
public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
public List<string> playedBroadcastKeys = new List<string>();
public List<string> discoveredStationIds = new List<string>();
public List<float> customPresets = new List<float>();
public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
public TriangulationState triangulation = new TriangulationState();
public string Checksum = string.Empty;
public class RadioSaveStateFrozenV2
public int saveVersion = 2;
public int day;
public float currentFrequency;
public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
public List<string> playedBroadcastKeys = new List<string>();
public List<string> discoveredStationIds = new List<string>();
public List<float> customPresets = new List<float>();
public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
public string Checksum = string.Empty;
public class RadioSaveStateFrozenV1
public int saveVersion = 1;
public int day;
public float currentFrequency;
public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
public List<string> playedBroadcastKeys = new List<string>();
public string Checksum = string.Empty;
public static class RadioSaveCodec
public const int CurrentSaveVersion = 6;
public const int MigrationFromVersion = 1;
public static string Encode(RadioSaveState state, IJsonSerializer json) {
public static bool TryDecode(string json, IJsonSerializer serializer, out RadioSaveState state) {
```


# Appendix E.24 — Supporting Code Evidence: `Assets/Ashfall.Core/Communications/CommunicationsSystem.cs`

### `Assets/Ashfall.Core/Communications/CommunicationsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 560 lines / 19952 bytes.
- SHA-256: `abe250107b1dc8b9fbfdfedb0392dc50fe6c71c0d4522d1f1237c235f62ede40`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=5; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AntennaType
public enum NetworkStatus
public sealed class AntennaDto
public string AntennaId { get; set; } = string.Empty;
public AntennaType Type { get; set; } = AntennaType.BasicWhip;
public string Name { get; set; } = string.Empty;
public double RangeKm { get; set; } = 8.0;
public int Sensitivity { get; set; } = 40; // 0..100
public int MaxChannels { get; set; } = 2;
public int PowerDrawWatts { get; set; } = 25;
public double Condition { get; set; } = 100.0; // 0..100
public bool IsActive { get; set; } = true;
public AntennaDto Clone() {
public sealed class CommunicationsNetworkDto
public string NetworkId { get; set; } = string.Empty;
public string Name { get; set; } = string.Empty;
public double FrequencyMhz { get; set; } = 144.2;
public int EncryptionLevel { get; set; } = 20; // 0..100
public string FactionId { get; set; } = "player_shelter";
public bool IsPlayerOwned { get; set; } = true;
public NetworkStatus Status { get; set; } = NetworkStatus.Active;
public CommunicationsNetworkDto Clone() {
public sealed class InterceptedMessageDto
public string MessageId { get; set; } = string.Empty;
public string SourceFactionId { get; set; } = string.Empty;
public double FrequencyMhz { get; set; }
public int EncryptionLevel { get; set; }
public string RawText { get; set; } = string.Empty;
public string DecodedContent { get; set; } = string.Empty;
public int InterceptDay { get; set; }
public bool IsDecoded { get; set; }
public int IntelligenceValue { get; set; }
public InterceptedMessageDto Clone() {
public sealed class OutgoingBroadcastDto
public string BroadcastId { get; set; } = string.Empty;
public double FrequencyMhz { get; set; }
public string Content { get; set; } = string.Empty;
public int EncryptionLevel { get; set; }
public int BroadcastDay { get; set; }
public double AudienceReachKm { get; set; }
public OutgoingBroadcastDto Clone() {
public sealed class CommunicationsState
public int SchemaVersion { get; set; } = 1;
public int TotalMessagesDecoded { get; set; }
public int TotalBroadcastsSent { get; set; }
public List<AntennaDto> Antennas { get; set; } = new();
public List<CommunicationsNetworkDto> Networks { get; set; } = new();
public List<InterceptedMessageDto> InterceptedMessages { get; set; } = new();
public List<OutgoingBroadcastDto> OutgoingBroadcasts { get; set; } = new();
public sealed class CommunicationsSystem
public int TotalMessagesDecoded { get; private set; }
public int TotalBroadcastsSent { get; private set; }
public Action<AntennaDto>? OnAntennaInstalledSeam { get; set; }
public Action<InterceptedMessageDto>? OnMessageInterceptedSeam { get; set; }
public Action<InterceptedMessageDto>? OnMessageDecodedSeam { get; set; }
public Action<OutgoingBroadcastDto>? OnBroadcastTransmittedSeam { get; set; }
public Action<double, int>? OnFrequencyJammedSeam { get; set; }
public void LoadCatalog(string json) {
public AntennaDto InstallAntenna(string antennaId, AntennaType type, string? name = null) {
public bool RepairAntenna(string antennaId, double amount) {
public void DegradeAntennas(double wearAmount) {
public double GetEffectiveReceptionRangeKm() {
public int GetEffectiveSensitivity() {
public InterceptedMessageDto? InterceptFactionSignal( string factionId, ISeededRng rng, int currentDay) {
public bool DecodeMessage(string messageId, int cryptanalysisSkill, ISeededRng rng) {
public OutgoingBroadcastDto? TransmitBroadcast( double frequencyMhz, string content, int encryptionLevel, int currentDay) {
public void JamFrequency(double frequencyMhz, int durationDays) {
public AntennaDto? GetAntenna(string antennaId) {
public CommunicationsNetworkDto? GetNetwork(string networkId) {
public InterceptedMessageDto? GetMessage(string messageId) {
public IReadOnlyList<AntennaDto> GetAllAntennas() {
public IReadOnlyList<InterceptedMessageDto> GetAllInterceptedMessages() {
public CommunicationsState CaptureState() {
public void RestoreState(CommunicationsState state) {
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Radio/SignalTrustTests.cs`

### `Ashfall.Core.Tests/Radio/SignalTrustTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 397; SHA-256: `141a56a8b6d5a73a4af56ff360312c4691bde8ff08643a50832c892592d15fba`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AnsweringSignalsIncreasesTrust
MerelyTuningOrHearingDoesNotCountAsAnswered
IgnoringSignalsDecreasesTrust
UndiscoveredSignalDoesNotCountAsIgnored
LegacyExpiryWithoutConsequenceAlsoCountsAsIgnored
IgnoringATrapSignalIsNotATrustDeficit
FallingForTrapHasStrongerNegativeEffect
SuccessfulRescueHasStrongestPositiveEffect
LateArrivalCountsAnsweredButNotRescueOrIgnored
NoDoubleCountingOnRepeatedTransitions
ExplicitIgnoreThenDeadlineExpiryCountsOnce
TrapAmbushSurvivalAddsNoSecondTrustEvent
ScoreIsBoundedAndClamped
TrustIsDeterministic
TrustSurvivesSaveLoad
RadioSaveRoundTripsTrustState
RadioSaveV4MigratesToNeutralTrust
TrustAffectsSignalAvailability
AvailabilityModifiersAreBoundedAndMonotonic
CandidateWeightingIsDeterministicAndOrderPreserving
TrustDoesNotChangeAuthenticityClassification
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/FactionRadioBroadcastExpansionTests.cs`

### `Ashfall.Core.Tests/FactionRadioBroadcastExpansionTests.cs`

- Current test declarations: Fact=22, Theory=0, InlineData=0.
- File lines: 634; SHA-256: `5f27d60e82325f6f609cad194275be4960ab070c818e3e334f73a6c5715aee16`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Corpus_HasExactlyThirtyBroadcasts_AndSilenceEventsRetained
Corpus_TenRequiredTypes_ThreeEach
Corpus_IdsUnique_AndNoCollisionWithRadioJson
Corpus_FactionReferencesResolve_ToCorpusFactions
Corpus_AllFactionsAppearAtLeastOnce
Corpus_FrequenciesWithinHostTuningBand_AndSignalValid
Corpus_MessagesConcise_AndToneCompliant
Corpus_IntelRefs_ResolveAgainstLocationRegistry
Corpus_DistressRefs_ResolveAgainstDistressAuthority
Corpus_TelemetryRefs_ResolveAgainstOrbitalHarrowAuthority
Corpus_QuestHooks_ResolveAgainstRuntimeQuestRegistry
Corpus_PatrolTerritoryLinks_AtLeastFive
Catalog_FactionCorpusBroadcastsRegister_ThroughUnifiedLoader
Catalog_AllThirtyBroadcastsRegistered_WithDistinctIds
Catalog_DayGates_ExcludeBroadcastsBeforeMinDay
Schedule_EightRequiredBroadcasts_EligibleThroughCanonicalSchedule
Schedule_ResolveSurfacesDistressBroadcast_AtPriority
Schedule_ResolveSurfacesDeadHandPing_OnAutomatedRelayBand
Schedule_Resolve_IsDeterministicForSameState
Engine_NearChannelBroadcasts_EnterFactionChatterPools
Engine_OffChannelBroadcasts_DoNotEnterChatterPools
Engine_DeterministicSelection_UnderSameSeed
```


# Appendix G.27 — Supporting Regression Evidence: `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`

### `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`

- Current test declarations: Fact=21, Theory=1, InlineData=0.
- File lines: 608; SHA-256: `a633b42fb21d127173f362cbb6523dad7363fe76af21922551de0c27d5a5eb6c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SignalChainsIntoFollowUpAfterInitialContact
RescueSuccessSchedulesFollowUp
FollowUpContentDiffersFromInitial
FollowUpTimingIsDeterministic
FollowUpSurvivesSaveLoad
FollowUpDoesNotFireWhenInitialSignalIgnored
FollowUpDoesNotFireWhenInitialSignalWasTrap
TrapAftermathFollowUpFiresOnAmbushEncountered
NoDuplicateSchedulingOrFiring
FiredFollowUpDoesNotRefireAfterReload
SameDayOrderingIsDeterministic
ResolvedParentCannotRescheduleOldFollowUp
Validator_RejectsUnsupportedTriggerAndNegativeDelay
Validator_RejectsDuplicateFollowUpIdsAcrossCorpus
Validator_AcceptsWellFormedFollowUpsAndRealCatalogsStayClean
Validator_EnforcesDistressFollowUpSemanticRules
FollowUpSignalsBindFromJson
RadioSaveV6RoundTripsFollowUpState
RadioSaveV5MigratesToEmptyFollowUpState
PendingViewProjectionIsDeterministicAndReadOnly
LastFiredProjectionRecordsMostRecentTransmission
StageAndFollowUpSemanticsAreDistinct
```


# Appendix G.28 — Supporting Regression Evidence: `Ashfall.Core.Tests/ShelterSchedulesPlan70CatalogTests.cs`

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


# Appendix H.29 — Supporting Authority Document: `docs/radio/RESCUE_SIGNAL_RUNTIME_AUTHORITY_MAP.md`

### `docs/radio/RESCUE_SIGNAL_RUNTIME_AUTHORITY_MAP.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 178 lines / 18243 bytes.
- SHA-256: `146fe7be288834e81eb03ce320a7a1ac682d955345d8dca646a8ff0014b5f6ea`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.30 — Supporting Authority Document: `docs/radio/DISTRESS_SIGNAL_TASKS_9_12_CLOSEOUT.md`

### `docs/radio/DISTRESS_SIGNAL_TASKS_9_12_CLOSEOUT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 125 lines / 7468 bytes.
- SHA-256: `3783d9ac186103f71a47a6b8d7d498cfbc33c7ec3c4a20fd868af22a415b2aed`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.31 — Supporting Authority Document: `docs/audio/AUDIO_PIPELINE_REPRODUCIBILITY_LEDGER.md`

### `docs/audio/AUDIO_PIPELINE_REPRODUCIBILITY_LEDGER.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 5095 lines / 300108 bytes.
- SHA-256: `c2d199b169d6c81f11d92a3291253ca1e20d425aeb0a2b0ec1c6a52867eb00b8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=42; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AudioAssetCategory
public enum AudioFileFormat
public readonly struct AudioAssetSpecification : IEquatable<AudioAssetSpecification>
public readonly string AssetId;
public readonly string RelativePath;
public readonly AudioAssetCategory Category;
public readonly AudioFileFormat Format;
public readonly float MeasuredLufs;
public readonly float TruePeakDb;
public readonly int DurationMilliseconds;
public readonly string Sha256Checksum;
public bool Equals(AudioAssetSpecification other) {
public override bool Equals(object obj) => obj is AudioAssetSpecification other && Equals(other);
public override int GetHashCode() => (AssetId, RelativePath).GetHashCode();
public sealed class AudioPipelineReproducibilityCoordinator
public int RegisteredAssetCount => _manifest.Count;
public void RegisterAsset(AudioAssetSpecification spec) {
public bool TryGetAsset(string assetId, out AudioAssetSpecification spec) {
public bool ValidateLoudnessCompliance(string assetId, out string failureReason) {
public string ComputeDeterministicChecksum() {
public sealed class AudioPipelineReproducibilityTests
public void Test_AudioPipeline_Reproducibility_Invariant_001() {
public void Test_AudioPipeline_Reproducibility_Invariant_002() {
public void Test_AudioPipeline_Reproducibility_Invariant_003() {
public void Test_AudioPipeline_Reproducibility_Invariant_004() {
public void Test_AudioPipeline_Reproducibility_Invariant_005() {
public void Test_AudioPipeline_Reproducibility_Invariant_006() {
public void Test_AudioPipeline_Reproducibility_Invariant_007() {
public void Test_AudioPipeline_Reproducibility_Invariant_008() {
public void Test_AudioPipeline_Reproducibility_Invariant_009() {
public void Test_AudioPipeline_Reproducibility_Invariant_010() {
public void Test_AudioPipeline_Reproducibility_Invariant_011() {
public void Test_AudioPipeline_Reproducibility_Invariant_012() {
public void Test_AudioPipeline_Reproducibility_Invariant_013() {
public void Test_AudioPipeline_Reproducibility_Invariant_014() {
public void Test_AudioPipeline_Reproducibility_Invariant_015() {
public void Test_AudioPipeline_Reproducibility_Invariant_016() {
public void Test_AudioPipeline_Reproducibility_Invariant_017() {
public void Test_AudioPipeline_Reproducibility_Invariant_018() {
public void Test_AudioPipeline_Reproducibility_Invariant_019() {
public void Test_AudioPipeline_Reproducibility_Invariant_020() {
public void Test_AudioPipeline_Reproducibility_Invariant_021() {
public void Test_AudioPipeline_Reproducibility_Invariant_022() {
public void Test_AudioPipeline_Reproducibility_Invariant_023() {
public void Test_AudioPipeline_Reproducibility_Invariant_024() {
public void Test_AudioPipeline_Reproducibility_Invariant_025() {
public void Test_AudioPipeline_Reproducibility_Invariant_026() {
public void Test_AudioPipeline_Reproducibility_Invariant_027() {
public void Test_AudioPipeline_Reproducibility_Invariant_028() {
public void Test_AudioPipeline_Reproducibility_Invariant_029() {
public void Test_AudioPipeline_Reproducibility_Invariant_030() {
public void Test_AudioPipeline_Reproducibility_Invariant_031() {
public void Test_AudioPipeline_Reproducibility_Invariant_032() {
public void Test_AudioPipeline_Reproducibility_Invariant_033() {
public void Test_AudioPipeline_Reproducibility_Invariant_034() {
public void Test_AudioPipeline_Reproducibility_Invariant_035() {
public void Test_AudioPipeline_Reproducibility_Invariant_036() {
public void Test_AudioPipeline_Reproducibility_Invariant_037() {
public void Test_AudioPipeline_Reproducibility_Invariant_038() {
public void Test_AudioPipeline_Reproducibility_Invariant_039() {
public void Test_AudioPipeline_Reproducibility_Invariant_040() {
public void Test_AudioPipeline_Reproducibility_Invariant_041() {
public void Test_AudioPipeline_Reproducibility_Invariant_042() {
public void Test_AudioPipeline_Reproducibility_Invariant_043() {
public void Test_AudioPipeline_Reproducibility_Invariant_044() {
public void Test_AudioPipeline_Reproducibility_Invariant_045() {
public void Test_AudioPipeline_Reproducibility_Invariant_046() {
public void Test_AudioPipeline_Reproducibility_Invariant_047() {
public void Test_AudioPipeline_Reproducibility_Invariant_048() {
public void Test_AudioPipeline_Reproducibility_Invariant_049() {
public void Test_AudioPipeline_Reproducibility_Invariant_050() {
public void Test_AudioPipeline_Reproducibility_Invariant_051() {
public void Test_AudioPipeline_Reproducibility_Invariant_052() {
public void Test_AudioPipeline_Reproducibility_Invariant_053() {
public void Test_AudioPipeline_Reproducibility_Invariant_054() {
public void Test_AudioPipeline_Reproducibility_Invariant_055() {
public void Test_AudioPipeline_Reproducibility_Invariant_056() {
public void Test_AudioPipeline_Reproducibility_Invariant_057() {
public void Test_AudioPipeline_Reproducibility_Invariant_058() {
public void Test_AudioPipeline_Reproducibility_Invariant_059() {
public void Test_AudioPipeline_Reproducibility_Invariant_060() {
public void Test_AudioPipeline_Reproducibility_Invariant_061() {
public void Test_AudioPipeline_Reproducibility_Invariant_062() {
public void Test_AudioPipeline_Reproducibility_Invariant_063() {
public void Test_AudioPipeline_Reproducibility_Invariant_064() {
public void Test_AudioPipeline_Reproducibility_Invariant_065() {
public void Test_AudioPipeline_Reproducibility_Invariant_066() {
public void Test_AudioPipeline_Reproducibility_Invariant_067() {
public void Test_AudioPipeline_Reproducibility_Invariant_068() {
public void Test_AudioPipeline_Reproducibility_Invariant_069() {
public void Test_AudioPipeline_Reproducibility_Invariant_070() {
public void Test_AudioPipeline_Reproducibility_Invariant_071() {
public void Test_AudioPipeline_Reproducibility_Invariant_072() {
public void Test_AudioPipeline_Reproducibility_Invariant_073() {
public void Test_AudioPipeline_Reproducibility_Invariant_074() {
public void Test_AudioPipeline_Reproducibility_Invariant_075() {
public void Test_AudioPipeline_Reproducibility_Invariant_076() {
public void Test_AudioPipeline_Reproducibility_Invariant_077() {
public void Test_AudioPipeline_Reproducibility_Invariant_078() {
public void Test_AudioPipeline_Reproducibility_Invariant_079() {
public void Test_AudioPipeline_Reproducibility_Invariant_080() {
public void Test_AudioPipeline_Reproducibility_Invariant_081() {
public void Test_AudioPipeline_Reproducibility_Invariant_082() {
public void Test_AudioPipeline_Reproducibility_Invariant_083() {
public void Test_AudioPipeline_Reproducibility_Invariant_084() {
public void Test_AudioPipeline_Reproducibility_Invariant_085() {
public void Test_AudioPipeline_Reproducibility_Invariant_086() {
public void Test_AudioPipeline_Reproducibility_Invariant_087() {
public void Test_AudioPipeline_Reproducibility_Invariant_088() {
public void Test_AudioPipeline_Reproducibility_Invariant_089() {
public void Test_AudioPipeline_Reproducibility_Invariant_090() {
public void Test_AudioPipeline_Reproducibility_Invariant_091() {
public void Test_AudioPipeline_Reproducibility_Invariant_092() {
public void Test_AudioPipeline_Reproducibility_Invariant_093() {
public void Test_AudioPipeline_Reproducibility_Invariant_094() {
public void Test_AudioPipeline_Reproducibility_Invariant_095() {
public void Test_AudioPipeline_Reproducibility_Invariant_096() {
public void Test_AudioPipeline_Reproducibility_Invariant_097() {
public void Test_AudioPipeline_Reproducibility_Invariant_098() {
public void Test_AudioPipeline_Reproducibility_Invariant_099() {
public void Test_AudioPipeline_Reproducibility_Invariant_100() {
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | station identity/frequency/state | RadioStationCatalog | Owner emits/reads a typed fact; no mirror state. |
| ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | rescue mission lifecycle and receipts | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | signal definitions, stages and outcomes | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | tuning, playback, cooldown and cues | Radio host/audio | Owner emits/reads a typed fact; no mirror state. |
| station identity/frequency/state | RadioStationCatalog | ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | Owner emits/reads a typed fact; no mirror state. |
| station identity/frequency/state | RadioStationCatalog | rescue mission lifecycle and receipts | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| station identity/frequency/state | RadioStationCatalog | signal definitions, stages and outcomes | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| station identity/frequency/state | RadioStationCatalog | tuning, playback, cooldown and cues | Radio host/audio | Owner emits/reads a typed fact; no mirror state. |
| rescue mission lifecycle and receipts | DistressRescueMissionManager | ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | Owner emits/reads a typed fact; no mirror state. |
| rescue mission lifecycle and receipts | DistressRescueMissionManager | station identity/frequency/state | RadioStationCatalog | Owner emits/reads a typed fact; no mirror state. |
| rescue mission lifecycle and receipts | DistressRescueMissionManager | signal definitions, stages and outcomes | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| rescue mission lifecycle and receipts | DistressRescueMissionManager | tuning, playback, cooldown and cues | Radio host/audio | Owner emits/reads a typed fact; no mirror state. |
| signal definitions, stages and outcomes | RadioDistressSystem | ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | Owner emits/reads a typed fact; no mirror state. |
| signal definitions, stages and outcomes | RadioDistressSystem | station identity/frequency/state | RadioStationCatalog | Owner emits/reads a typed fact; no mirror state. |
| signal definitions, stages and outcomes | RadioDistressSystem | rescue mission lifecycle and receipts | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| signal definitions, stages and outcomes | RadioDistressSystem | tuning, playback, cooldown and cues | Radio host/audio | Owner emits/reads a typed fact; no mirror state. |
| tuning, playback, cooldown and cues | Radio host/audio | ordinary schedule and dynamic alert precedence | RadioScheduleCoordinator | Owner emits/reads a typed fact; no mirror state. |
| tuning, playback, cooldown and cues | Radio host/audio | station identity/frequency/state | RadioStationCatalog | Owner emits/reads a typed fact; no mirror state. |
| tuning, playback, cooldown and cues | Radio host/audio | rescue mission lifecycle and receipts | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| tuning, playback, cooldown and cues | Radio host/audio | signal definitions, stages and outcomes | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Retire the plan as a request to create schedule/rescue architecture; those owners exist. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Audit frequency/day ownership across ordinary, faction, verdict, Year-of-Ash, distress and player programming. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Preserve sealed content boundaries and exactly-once runtime tests. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Improve only truthful missing surface states proven by audits. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioBroadcastModels.cs`

### `Assets/Ashfall.Core/Radio/RadioBroadcastModels.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 359 lines / 12274 bytes.
- SHA-256: `0f10a7c8612ee252192409dc16ecbb0abcc79533f890c482f443fc563ca5ef20`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum BroadcastGenre
public enum SourceReliability
public enum BroadcastPriority
public enum RadioStationState
public sealed class RadioProgramSlot
public string SlotId { get; set; } = string.Empty;
public int StartHour { get; set; } = 0;
public int EndHour { get; set; } = 23;
public string ProgramType { get; set; } = string.Empty;
public string BroadcastPoolId { get; set; } = string.Empty;
public RadioStationState MinStationState { get; set; } = RadioStationState.Normal;
public int Weight { get; set; } = 10;
public int DayMin { get; set; } = 1;
public int DayMax { get; set; } = 9999;
public bool MatchesTime(int campaignDay, int hour) {
public sealed class RadioReceptionFactors
public float DistanceKm { get; set; } = 0f;
public float WeatherAttenuation01 { get; set; } = 0f;
public bool IsBrownout { get; set; }
public float ReceiverCondition01 { get; set; } = 1.0f;
public bool IsJammed { get; set; }
public bool HasAntennaArray { get; set; }
public bool HasAmplifier { get; set; }
public sealed class RadioSignalStrength
public float RawStrength01 { get; set; } = 1.0f;
public float EffectiveStrength01 { get; set; } = 1.0f;
public string QualityBand { get; set; } = "Optimal"; // Optimal, Good, Degraded, Critical, Unreadable
public List<string> Reasons { get; set; } = new List<string>();
public static RadioSignalStrength Evaluate(float baseStrength01, RadioReceptionFactors? factors) {
public sealed class RadioStationDefinition
public string DisplayName { get; set; } = string.Empty;
public float FrequencyMhz { get; set; } = 88.5f;
public string OwnerFactionId { get; set; } = string.Empty;
public string PersonaVoice { get; set; } = string.Empty;
public SourceReliability Reliability { get; set; } = SourceReliability.Official;
public RadioStationState DefaultState { get; set; } = RadioStationState.Normal;
public string SilenceText { get; set; } = "STATIC... [ Carrier hum steady. No voice detected. ]";
public string JammedText { get; set; } = "STATIC... [ Severe RF interference / heterodyne squeal. ]";
public string SignalProfileId { get; set; } = string.Empty;
public List<string> EquipmentRequirements { get; set; } = new List<string>();
public List<string> Tags { get; set; } = new List<string>();
public List<RadioProgramSlot> Schedule { get; set; } = new List<RadioProgramSlot>();
public RadioProgramSlot? GetCurrentSlot(int campaignDay, int hour) {
public RadioProgramSlot? GetNextSlot(int campaignDay, int hour) {
public sealed class UnifiedRadioBroadcast
public string BroadcastId { get; set; } = string.Empty;
public float FrequencyMhz { get; set; } = 88.5f;
public int DayMin { get; set; } = 1;
public int DayMax { get; set; } = 9999;
public int DayTrigger { get; set; } = 1;
public string StationId { get; set; } = string.Empty;
public string SourceName { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string Message { get; set; } = string.Empty;
public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
public SourceReliability Reliability { get; set; } = SourceReliability.Official;
public BroadcastPriority Priority { get; set; } = BroadcastPriority.Routine;
public int SignalStrength { get; set; } = 5; // S-units 1..9
public bool IsEmergency { get; set; }
public bool IsOneShot { get; set; }
public string AudioCue { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public string DownstreamConsequence { get; set; } = string.Empty;
public sealed class AppointmentProgramDefinition
public string ProgramId { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string StationId { get; set; } = string.Empty;
public float FrequencyMhz { get; set; } = 88.5f;
public int CadenceDays { get; set; } = 1;
public int DayOffset { get; set; } = 0;
public int CadenceWindow { get; set; } = 0;
public BroadcastGenre Genre { get; set; } = BroadcastGenre.CivilianNews;
public BroadcastPriority Priority { get; set; } = BroadcastPriority.Important;
```


# Appendix Q.564 — Additional Current Architecture Evidence: `src/UI/RadioIntelligencePanel.cs`

### `src/UI/RadioIntelligencePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 259 lines / 11526 bytes.
- SHA-256: `d7293f0eac3095a47a53e32128dccaf895175c2f5466bb5893bbefdb20bf05dd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class RadioIntelligencePanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _radio != null;
public void Bind(ShelterRadioStationSystem radio, OrbitalHarrowTelemetrySystem? harrow = null, int currentDay = 0) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() => Unbind();
public void Open() {
public void RefreshView() {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs`

### `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 365 lines / 16893 bytes.
- SHA-256: `848439647bd199a192059bfcd1f4c993db49d03853ae5d30f21e7146226ea3e0`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SignalFollowUpTriggers
public const string Answered = "answered";
public const string RescueSuccess = "rescue_success";
public const string RescueFailed = "rescue_failed";
public const string Expired = "expired";
public const string AmbushEncountered = "ambush_encountered";
public static readonly string[] All = {
public static bool IsValid(string? trigger) {
public sealed class SignalFollowUpDefinition
public string Id { get; set; } = string.Empty;
public string TriggerCondition { get; set; } = string.Empty;
public int DelayDays { get; set; }
public string Text { get; set; } = string.Empty;
public float Clarity { get; set; } = 0.9f;
public string OutcomeHint { get; set; } = string.Empty;
public string AudioCue { get; set; } = string.Empty;
public sealed class PendingSignalFollowUpEntry
public string parentSignalId = string.Empty;
public string followUpId = string.Empty;
public int dueDay;
public sealed class SignalFollowUpSaveState
public int version = 1;
public List<PendingSignalFollowUpEntry> pending = new List<PendingSignalFollowUpEntry>();
public List<string> firedKeys = new List<string>();
public sealed class DistressFollowUpScheduler
public event Action<string, SignalFollowUpDefinition, int>? OnFollowUpFired;
public readonly struct PendingFollowUpView
public string ParentSignalId { get; }
public string FollowUpId { get; }
public int DueDay { get; }
public List<PendingFollowUpView> GetPendingView() {
public int CurrentDay { get; private set; }
public void SetDay(int day) {
public void BindToMissionEvents() {
public int ScheduleForTrigger(string parentSignalId, string trigger) {
public void TickDaily(int currentDay) {
public IReadOnlyCollection<string> PendingKeys => _pending.Keys;
public IReadOnlyCollection<string> FiredKeys => _fired;
public static string Key(string parentSignalId, string followUpId) => $"{parentSignalId}:{followUpId}";
public SignalFollowUpSaveState CaptureState() {
public void RestoreState(SignalFollowUpSaveState? state) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioRecordingSystem.cs`

### `Assets/Ashfall.Core/Radio/RadioRecordingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 106 lines / 4606 bytes.
- SHA-256: `7517b367865804c98714f607869c81f5a69ef80b2d8916a60b94564e9d5ab16d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioRecordingSystem
public const string BlankTapeItemId = "item_blank_magnetic_tape";
public event Action<RecordedCassetteEntry>? OnBroadcastRecorded;
public IReadOnlyCollection<RecordedCassetteEntry> RecordedTapes => _recordedTapes.Values;
public RecordedCassetteEntry? RecordBroadcast(ScheduledBroadcastResult broadcast, int day) {
public RecordedCassetteEntry? ReplayCassette(string cassetteId) {
public int CalculateTradeValue(string cassetteId) {
public List<RecordedCassetteEntry> CaptureState() {
public void RestoreState(List<RecordedCassetteEntry>? savedEntries) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Radio/FactionRadioHudPanel.cs`

### `src/Radio/FactionRadioHudPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 390 lines / 17451 bytes.
- SHA-256: `777fbb06af8ec1f1c72a3223f4bcb3f83b14f88659c9f9afb296dbd5a278a05c`.
- Architecture signals: seeded references=4; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FactionRadioHudPanel : PanelContainer
public float TunedFrequency => _currentFrequency;
public int LogCount => _history.Count;
public bool HasFrameTexture => GetThemeStylebox("panel") != null;
public bool HasFrequencyDial => _sliderFrequency != null;
public bool HasSMeter => _textureSmeter != null;
public bool HasCrtOverlay => _crtOverlay != null;
public bool HasLiveDisplay => _lblCrtLiveText != null && !string.IsNullOrEmpty(_lblCrtLiveText.Text);
public bool HasFactionBadge => _textureFactionBadge?.Texture != null;
public override void _Ready() {
public void BindProvider(IFactionRadioProvider provider, ISeededRng rng, int day = 1) {
public void SetDay(int day) {
public void TuneToFrequency(float freqMhz) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`

### `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 246 lines / 10661 bytes.
- SHA-256: `1b6139a0f0707e684f58718315b0b1191edd1af95db74f20d2dc545718108596`.
- Architecture signals: seeded references=4; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionRadioEngine : IFactionRadioProvider
public int FactionCount => _channels.Count;
public int SilenceEventCount => _silenceEvents.Count;
public void RegisterChannel(FactionRadioChannel channel) {
public void AddSilenceEvent(string silenceText) {
public IReadOnlyList<string> GetAllFactions() => _factionOrder;
public float GetFactionFrequency(string factionId) {
public string GetFactionCallsign(string factionId) {
public string? TryFindFactionAtFrequency(float frequencyMhz, float toleranceMhz = 1.5f) {
public RadioIntercept GetBroadcastAtFrequency(float frequencyMhz, int day, ISeededRng rng) {
public RadioIntercept GetFactionEvent(string factionId, RadioEventKind kind, int day, ISeededRng rng) {
public static FactionRadioEngine LoadFromJson(string json) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/RadioScriptbookCatalog.cs`

### `Assets/Ashfall.Core/Narrative/RadioScriptbookCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 95 lines / 3204 bytes.
- SHA-256: `f9334a7abfe490cd71fb47cf6684dbbbfdaf35e9d6840cf039f41d88938d63e3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioBroadcastEntry
public string broadcast_id;
public float frequency_mhz;
public string station_name;
public string voice_profile;
public int day_trigger;
public string title;
public string transcript;
public string[] tags;
public string audio_cue;
public sealed class RadioScriptbookFile
public int schema_version;
public string collection_id;
public List<RadioBroadcastEntry> broadcasts = new List<RadioBroadcastEntry>();
public sealed class RadioScriptbookCatalog
public IReadOnlyList<RadioBroadcastEntry> AllBroadcasts => _allBroadcasts;
public void Load(string json, IJsonSerializer serializer) {
public RadioBroadcastEntry? GetById(string broadcastId) {
public List<RadioBroadcastEntry> GetByFrequency(float freqMhz, float tolerance = 0.15f) {
public RadioBroadcastEntry? GetActiveBroadcast(float freqMhz, int currentDay, float tolerance = 0.15f) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs`

### `Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 406 lines / 17605 bytes.
- SHA-256: `98b19e30f7cf28ba70a82884c71a1ca1ff77d4db8d5af8cc58dbe7b509da8d84`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RadioProgramJobStatus
public sealed class RadioProgramJob
public string JobId { get; set; } = string.Empty;
public string TemplateId { get; set; } = string.Empty;
public string PresenterId { get; set; } = string.Empty;
public string StationId { get; set; } = string.Empty;
public string SlotId { get; set; } = string.Empty;
public string PsyopsCampaignId { get; set; } = string.Empty;
public int DayStarted { get; set; }
public int PrepTicks { get; set; }
public int PrepTicksRequired { get; set; } = 1;
public int Status { get; set; } = (int)RadioProgramJobStatus.Preparing;
public string LastDeliveryBroadcastId { get; set; } = string.Empty;
public bool PropagandaStarted { get; set; }
public string FailureCode { get; set; } = string.Empty;
public float AudienceMoraleDelta { get; set; }
public string AudienceReachGrade { get; set; } = string.Empty;
public bool OpportunitySpawned { get; set; }
public sealed class RadioProgramFollowUpHook
public string HookId { get; set; } = string.Empty;
public string SourceJobId { get; set; } = string.Empty;
public string TemplateId { get; set; } = string.Empty;
public int CreatedDay { get; set; }
public bool Resolved { get; set; }
public string ResolutionAction { get; set; } = string.Empty;
public int ResolvedDay { get; set; }
public sealed class AudienceResponseResult
public string JobId { get; set; } = string.Empty;
public string TemplateId { get; set; } = string.Empty;
public float MoraleDelta { get; set; }
public string ReachGrade { get; set; } = "Local";
public bool OpportunityCreated { get; set; }
public string TargetFactionId { get; set; } = string.Empty;
public int FactionReputationDelta { get; set; }
public string ResponseSummary { get; set; } = string.Empty;
public sealed class RadioProgramProductionState
public string SystemId { get; set; } = RadioProgramProductionSystem.SystemId;
public int SchemaVersion { get; set; } = 1;
public List<RadioProgramJob> Jobs { get; set; } = new List<RadioProgramJob>();
public List<RadioProgramFollowUpHook> FollowUps { get; set; } = new List<RadioProgramFollowUpHook>();
public int NextJobSeq { get; set; }
public int TotalDelivered { get; set; }
public int TotalCancelled { get; set; }
public sealed class RadioProgramProductionSystem
public const string SystemId = "radio_program_production";
public Func<string, int, bool>? StartPropagandaCampaign { get; set; }
public Func<IReadOnlyList<string>, bool>? HasRequiredEquipment { get; set; }
public Func<string, int, bool>? TryConsumePrepCost { get; set; }
public Func<string, float>? PresenterCapabilityProvider { get; set; }
public Action<float>? ApplyShelterMoraleDelta { get; set; }
public Action<string, int>? ApplyFactionReputationDelta { get; set; }
public event Action<RadioProgramJob>? OnJobReady;
public event Action<RadioProgramJob>? OnJobDelivered;
public event Action<RadioProgramJob>? OnJobCancelled;
public event Action<RadioProgramJob, AudienceResponseResult>? OnAudienceResponseCalculated;
public RadioProgramProductionState State => _state;
public RadioProgramCatalog Catalog => _catalog;
public ActionResult StartPrep(string templateId, string presenterId, int day) {
public ActionResult CancelJob(string jobId) {
public void TickDay(int day) {
public ActionResult TryDeliver(string jobId, ScheduledBroadcastResult delivery, int day) {
public AudienceResponseResult CalculateAudienceResponse(RadioProgramJob job, ScheduledBroadcastResult delivery) {
public ActionResult ResolveFollowUpHook(string hookId, string resolutionAction, int day) {
public List<RadioProgramFollowUpHook> GetUnresolvedFollowUps() =>
public bool SlotExists(string stationId, string slotId) {
public RadioProgramJob? FindJob(string jobId) {
public List<RadioProgramJob> GetActiveJobs() =>
public RadioProgramProductionState CaptureState() {
public void RestoreState(RadioProgramProductionState? saved) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioTuner.cs`

### `Assets/Ashfall.Core/Radio/RadioTuner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 246 lines / 8325 bytes.
- SHA-256: `02ed0287af8a0118b8262fcad89beebbc6f0464a5fd51038a6b90f083e5c188f`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RadioTuner
public event Action<RadioSignalEvent>? OnSignalChanged;
public RadioTunerState State => _state;
public float TunedFrequencyKHz => _state.TunedFrequencyKHz;
public bool IsTunedTo(float frequencyKHz) {
public void TuneTo(float frequencyKHz) {
public void TuneBy(float deltaKHz) {
public SignalLockResult Evaluate(IEnumerable<RadioBroadcast> broadcasts, float staticNoiseFloor, ISeededRng rng) {
public DistressFrequencyTuneResult EvaluateFrequency( float frequencyMhz, RadioDistressSystem distress, float staticNoiseFloor, ISeededRng rng, int day)
public sealed class DistressFrequencyTuneResult
public bool IsLocked;
public DistressSignalDefinition? Signal;
public bool IsGenuineRescue;
public bool IsDeceptive;
public float VuStrength;
public float Noise;
public string DecodedContent = string.Empty;
public float Clarity;
public sealed class RadioBroadcast
public string BroadcastId;
public float FrequencyKHz;
public float SignalStrength;
public float LockThreshold;
public string Headline;
public List<string> TranscriptLines;
public sealed class RadioTunerState
public float TunedFrequencyKHz;
public sealed class SignalLockResult
public bool IsLocked;
public RadioBroadcast Broadcast;
public float VuStrength;
public float Noise;
public string DecodedContent;
public static SignalLockResult NoSignal => new SignalLockResult
public sealed class RadioSignalEvent
public float TunedFrequencyKHz;
public bool IsLocked;
public float VuStrength;
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/SignalTrustAvailability.cs`

### `Assets/Ashfall.Core/Radio/SignalTrustAvailability.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 106 lines / 4891 bytes.
- SHA-256: `c3d5d87185d02908755fa8ca70afdddc2acf937389eb09f6b6e0818c2d1590d0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SignalTrustAvailability
public const int MinPermille = 500;   // 0.5× — bounded, never zero
public const int MaxPermille = 1500;  // 1.5× — bounded, never dominant
public static int GenuineModifierPermille(int score) {
public static int TrapModifierPermille(int score) {
public static int ApplyModifier(int baseWeight, int modifierPermille) {
public readonly struct WeightedCandidate
public string SignalId { get; }
public int BaseWeight { get; }
public bool IsGenuine { get; }
public int FinalWeight { get; }
public static List<WeightedCandidate> ModifyCandidates( IReadOnlyList<(string SignalId, int BaseWeight, bool IsGenuine)> orderedCandidates, int trustScore) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs`

### `Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 122 lines / 4873 bytes.
- SHA-256: `244d1664c34881812ec29dc4cf5fcf1ba8904cce3b31cbec3a1dd9a75a0706fa`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class RadioStationCatalogLoader
public const string StationsFileName = "radio_stations.json";
public const float MinFrequencyMhz = 0.1f;
public const float MaxFrequencyMhz = 1000.0f;
public static int LoadAndRegister( RadioStationCatalog catalog, string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null) {
public static int LoadFromJsonString(RadioStationCatalog catalog, string json, string sourcePath = "<json>") {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`

### `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 746 lines / 38037 bytes.
- SHA-256: `f5e7e24bcf885379f2850f84f24feedd0b3a093485c1e72076ecaf8e40fef2db`.
- Architecture signals: seeded references=0; save/restore symbols=10; typed event declarations=26; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PatientAvailability
public bool Available;
public string ReasonCode = "patient_unknown";
public static PatientAvailability Ok() => new PatientAvailability { Available = true, ReasonCode = "ok" };
public static PatientAvailability Blocked(string reason) => new PatientAvailability { Available = false, ReasonCode = reason };
public sealed class MedicalOperationResult
public bool Success;
public string ReasonCode = string.Empty;
public long StateVersion;
public int ProcedureId = -1;
public DiagnosisStatus DiagnosisAfter = DiagnosisStatus.Unknown;
public static MedicalOperationResult Fail(string reason) =>
public sealed class MedicalPipelineCoordinator
public long StateVersion { get; private set; }
public DiagnosisKnowledgeStore Diagnosis => _diagnosis;
public MedicalReservationLedger Reservations => _reservations;
public MedicalProcedureSchedule Schedule => _schedule;
public MedicalRecordLog Record => _record;
public event Action? StateChanged;
public event Action<string, Survivors.SurvivorId>? OnDiagnosisConfirmed;
public event Action<string, Survivors.SurvivorId>? OnDiagnosisSuspected;
public event Action<string, Survivors.SurvivorId>? OnPatientStabilized;
public event Action<string, Survivors.SurvivorId>? OnPatientRecovered;
public event Action<string, Survivors.SurvivorId>? OnTreatmentScheduled;
public event Action<string, Survivors.SurvivorId>? OnTreatmentCompleted;
public event Action<string, Survivors.SurvivorId, string>? OnTreatmentRefused;
public event Action<string>? OnProtocolExecuted;
public void RegisterHandler(IAfflictionHandler handler) {
public IAfflictionHandler? GetHandler(AfflictionId definition) {
public IReadOnlyCollection<IAfflictionHandler> Handlers => _handlers.Values;
public void RegisterProtocol(IMedicalProtocolHandler protocol) {
public IMedicalProtocolHandler? GetProtocol(string protocolId) {
public IReadOnlyCollection<IMedicalProtocolHandler> Protocols => _protocols.Values;
public CommandPreview PreviewDiagnose(Survivors.SurvivorId survivor, AfflictionId definition, long expectedVersion = 0) {
public MedicalOperationResult ExecuteDiagnose(Survivors.SurvivorId survivor, AfflictionId definition, long expectedVersion = 0) {
public CommandPreview PreviewIdentify(Survivors.SurvivorId survivor, long expectedVersion = 0) {
public MedicalOperationResult ExecuteIdentify(Survivors.SurvivorId survivor, long expectedVersion = 0) {
public void SuspectFromEvidence(Survivors.SurvivorId survivor, AfflictionId definition, int day, string evidenceCode) {
public void ConfirmForLegacySave(Survivors.SurvivorId survivor, AfflictionId definition, int day) {
public CommandPreview PreviewTreatment(Survivors.SurvivorId survivor, string treatmentId, long expectedVersion = 0, AfflictionId? target = null, string? targetItem = null) {
public MedicalOperationResult ExecuteTreatment(Survivors.SurvivorId survivor, string treatmentId, long expectedVersion = 0, AfflictionId? target = null, string? targetItem = null) {
public CommandPreview PreviewProtocol(string protocolId, long expectedVersion = 0) {
public MedicalOperationResult ExecuteProtocol(string protocolId, long expectedVersion = 0) {
public MedicalOperationResult ExecuteCancel(int procedureId, long expectedVersion = 0) {
public IReadOnlyList<MedicalProcedureCompletion> AdvanceScheduled(float hours, int currentDay) {
public void ReconcilePatientDeath(Survivors.SurvivorId survivor, int day) {
public PatientAvailability AvailabilityOf(Survivors.SurvivorId survivor) {
public MedicalPipelineSaveState CaptureState() {
public void RestoreState(MedicalPipelineSaveState? saved) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/CargoAirdropSystem.cs`

### `Assets/Ashfall.Core/World/CargoAirdropSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 452 lines / 20507 bytes.
- SHA-256: `60fee29f757ed41b527f3d58866563aa803cf3eed040d9b1feb97e5d5f8bd2b6`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=14; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class AirdropProfileDef
public string drop_profile_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public List<string> trigger_signal_outcomes { get; set; } = new List<string>();
public string cargo_pool_id { get; set; } = string.Empty;
public float wind_sensitivity { get; set; } = 1f;
public float base_impact_kph { get; set; } = 22f;
public float wind_impact_factor_kph { get; set; } = 0.6f;
public float light_impact_kph { get; set; } = 24f;
public float heavy_impact_kph { get; set; } = 40f;
public int integrity_loss_light_pct { get; set; } = 15;
public int integrity_loss_heavy_pct { get; set; } = 45;
public int beacon_duration_days { get; set; } = 4;
public int initial_interception_risk_bp { get; set; } = 250;
public int interception_rate_per_day_bp { get; set; } = 900;
public List<string> terrain_recovery_tags { get; set; } = new List<string>();
public List<string> tags { get; set; } = new List<string>();
public sealed class AirdropCargoEntryDef
public string item_id { get; set; } = string.Empty;
public int quantity { get; set; }
public float weight_kg_per_unit { get; set; }
public sealed class AirdropCargoPoolDef
public string cargo_pool_id { get; set; } = string.Empty;
public List<AirdropCargoEntryDef> entries { get; set; } = new List<AirdropCargoEntryDef>();
public sealed class CargoAirdropCatalog
public int schema_version { get; set; } = 1;
public int descent_bands { get; set; } = 4;
public int max_active_drops { get; set; } = 3;
public AirdropInterceptionDef interception { get; set; } = new AirdropInterceptionDef();
public List<AirdropProfileDef> drop_profiles { get; set; } = new List<AirdropProfileDef>();
public List<AirdropCargoPoolDef> cargo_pools { get; set; } = new List<AirdropCargoPoolDef>();
public sealed class AirdropInterceptionDef
public int rate_per_day_bp { get; set; } = 900;
public int expired_grace_days { get; set; } = 3;
public sealed class AirdropCrateEntryState
public string item_id { get; set; } = string.Empty;
public int quantity { get; set; }
public float weight_kg_per_unit { get; set; }
public sealed class AirdropEventState
public string event_id { get; set; } = string.Empty;
public string drop_profile_id { get; set; } = string.Empty;
public string source_signal_id { get; set; } = string.Empty;
public string phase { get; set; } = "scheduled";
public int release_day { get; set; }
public int current_band { get; set; }
public int target_x { get; set; }
public int target_y { get; set; }
public float wind_direction_deg { get; set; }
public float wind_speed_kph { get; set; }
public int landing_x { get; set; }
public int landing_y { get; set; }
public int landing_day { get; set; }
public int cargo_integrity_pct { get; set; } = 100;
public bool beacon_active { get; set; }
public int beacon_expires_day { get; set; }
public int interception_progress_bp { get; set; }
public List<AirdropCrateEntryState> crate_contents { get; set; } = new List<AirdropCrateEntryState>();
public List<AirdropCrateEntryState> remaining_contents { get; set; } = new List<AirdropCrateEntryState>();
public sealed class CargoAirdropState
public int schema_version { get; set; } = 1;
public List<AirdropEventState> drops { get; set; } = new List<AirdropEventState>();
public int next_event_number { get; set; } = 1;
public int total_recovered { get; set; }
public int total_intercepted { get; set; }
public int total_expired { get; set; }
public static class AirdropFailures
public const string DropUnavailable = "airdrop.profile_invalid";
public const string RadioContactMissing = "airdrop.signal_missing";
public const string MaxActiveDrops = "airdrop.max_active";
public const string EventNotFound = "airdrop.event_not_found";
public const string WrongPhase = "airdrop.wrong_phase";
public const string BeaconUnavailable = "airdrop.beacon_unavailable";
public sealed class CargoAirdropSystem
public Func<int>? DayProvider { get; set; }
public CargoAirdropState State => _state;
public CargoAirdropCatalog Catalog => _catalog;
public event Action<AirdropEventState>? OnDropScheduled;
public event Action<AirdropEventState>? OnDropLanded;
public event Action<AirdropEventState>? OnDropRecovered;
public event Action<AirdropEventState>? OnDropIntercepted;
public event Action<AirdropEventState>? OnDropExpired;
public event Action<AirdropEventState, string, int>? OnCrateCollected;
public event Action<string>? OnEventRaised;
public void BindCatalog(CargoAirdropCatalog? catalog) {
public AirdropProfileDef? FindProfile(string profileId) {
public ActionResult ScheduleDrop(string profileId, string signalId, int targetX, int targetY) {
public Func<float>? WindDirectionDeg { get; set; }
public Func<float>? WindSpeedKph { get; set; }
public void TickDay(int day) {
public ActionResult ReactivateBeacon(string eventId) {
public AirdropEventState? FindEvent(string eventId) {
public int CollectCrate(string eventId, Func<string, float, int, bool> tryGrantItem) {
public AirdropEventState? FindLandedAt(string locationId) {
public CargoAirdropState CaptureState() {
public void RestoreState(CargoAirdropState? state) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

### `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 451 lines / 20575 bytes.
- SHA-256: `4dfa917553bf8239ff0ed5799cd97fb81954caa3f0f24d32137f4e220bc092ef`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionWarContentCatalog
public IReadOnlyList<FactionWarEventChain> EventChains => _eventChains;
public IReadOnlyList<FactionWarJournalEntry> JournalEntries => _journalEntries;
public IReadOnlyList<FactionWarBroadcast> Broadcasts => _broadcasts;
public IReadOnlyList<FactionWarDialogueSnippet> DialogueSnippets => _dialogueSnippets;
public IReadOnlyList<FactionWarCommunique> Communiques => _communiques;
public IReadOnlyList<FactionWarLocationOverride> LocationOverrides => _locationOverrides;
public int EventChainCount => _eventChains.Count;
public int JournalEntryCount => _journalEntries.Count;
public int BroadcastCount => _broadcasts.Count;
public int DialogueSnippetCount => _dialogueSnippets.Count;
public int CommuniqueCount => _communiques.Count;
public int LocationOverrideCount => _locationOverrides.Count;
public void AddEventChain(FactionWarEventChain chain) => _eventChains.Add(chain);
public void AddJournalEntry(FactionWarJournalEntry entry) => _journalEntries.Add(entry);
public void AddBroadcast(FactionWarBroadcast broadcast) => _broadcasts.Add(broadcast);
public void AddDialogueSnippet(FactionWarDialogueSnippet snippet) => _dialogueSnippets.Add(snippet);
public void AddCommunique(FactionWarCommunique communique) => _communiques.Add(communique);
public void AddLocationOverride(FactionWarLocationOverride entry) => _locationOverrides.Add(entry);
public List<FactionWarEventChain> GetEligibleChains(int day) {
public List<FactionWarJournalEntry> GetJournalForDay(int day) {
public List<FactionWarBroadcast> GetBroadcastsForDay(int day) {
public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day) {
public List<FactionWarCommunique> GetCommuniquesForFaction(string factionId, int day) {
public FactionWarLocationOverride? GetActiveLocationOverride(string locationId, int day) {
public sealed class FactionWarEventChain
public string chainId = string.Empty;
public string band = string.Empty;
public string title = string.Empty;
public List<string> factionsInvolved = new List<string>();
public string locationId = string.Empty;
public List<FactionWarEventStage> stages = new List<FactionWarEventStage>();
public sealed class FactionWarEventStage
public string stageId = string.Empty;
public int minDay;
public string triggerCondition = string.Empty;
public string title = string.Empty;
public string bodyText = string.Empty;
public List<FactionWarEventChoice> choices = new List<FactionWarEventChoice>();
public string requiresFlag = string.Empty;
public string producesFlag = string.Empty;
public sealed class FactionWarEventChoice
public string choiceId = string.Empty;
public string text = string.Empty;
public int moraleDelta;
public string leadsToStageId = string.Empty;
public string requiresFlag = string.Empty;
public string producesFlag = string.Empty;
public string standingFactionId = string.Empty;
public int standingDelta;
public sealed class FactionWarJournalEntry
public string id = string.Empty;
public string authorName = string.Empty;
public int day;
public string locationId = string.Empty;
public string voice = string.Empty;
public string body = string.Empty;
public sealed class FactionWarBroadcast
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public bool isEmergency;
public string audio_cue = string.Empty;
public sealed class FactionWarDialogueSnippet
public string id = string.Empty;
public string locationId = string.Empty;
public int minDay;
public string speakerTag = string.Empty;
public string body = string.Empty;
public sealed class FactionWarCommunique
public string id = string.Empty;
public string eventChainId = string.Empty;
public string factionId = string.Empty;
public int day;
public string title = string.Empty;
public string body = string.Empty;
public string authorNote = string.Empty;
public sealed class FactionWarLocationOverride
public string id = string.Empty;
public string locationId = string.Empty;
public string overrideType = string.Empty;
public int activeFromDay;
public int activeUntilDay;
public string displayName = string.Empty;
public string description = string.Empty;
public sealed class FactionWarEventChainRoot
public int schema_version;
public List<FactionWarEventChain> chains = new List<FactionWarEventChain>();
public sealed class FactionWarJournalRoot
public int schema_version;
public List<FactionWarJournalEntry> entries = new List<FactionWarJournalEntry>();
public sealed class FactionWarBroadcastRoot
public int schema_version;
public List<FactionWarBroadcast> broadcasts = new List<FactionWarBroadcast>();
public sealed class FactionWarDialogueRoot
public int schema_version;
public List<FactionWarDialogueSnippet> snippets = new List<FactionWarDialogueSnippet>();
public sealed class FactionWarCommuniqueRoot
public int schema_version;
public List<FactionWarCommunique> communiques = new List<FactionWarCommunique>();
public sealed class FactionWarLocationOverrideRoot
public int schema_version;
public List<FactionWarLocationOverride> locationOverrides = new List<FactionWarLocationOverride>();
public sealed class FactionWarContentCatalogLoader
public const string EventsFile = "faction_war_events.json";
public const string JournalFile = "faction_war_journal.json";
public const string RadioFile = "faction_war_radio.json";
public const string DialogueFile = "faction_war_dialogue.json";
public const string CommuniquesFile = "faction_war_communiques.json";
public const string LocationOverridesFile = "faction_war_location_overrides.json";
public FactionWarContentCatalog Load(string dataDirectory) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs`

### `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 200 lines / 7165 bytes.
- SHA-256: `7fc44eff7cde5e9b9026c3112ff46cf14c28bdaf97c4db76f2db6bb45f8ee8d8`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PatrolRadioHooksState
public List<string> ConsumedSignals { get; set; } = new();
public List<string> PendingSignals { get; set; } = new();
public sealed class PatrolRadioHooks
public static readonly HashSet<string> RadioCapableFactions = new(StringComparer.OrdinalIgnoreCase) {
public static readonly HashSet<string> NonRadioCapableFactions = new(StringComparer.OrdinalIgnoreCase) {
public IReadOnlyCollection<string> ConsumedSignals => _consumedSignals;
public IReadOnlyList<string> PendingSignals => _pendingSignals.ToList();
public int PendingCount => _pendingSignals.Count;
public static bool IsFactionRadioCapable(string factionId) {
public static bool TryGetRadioSignalForEncounter(string encounterIdOrGroup, out string radioBroadcastId) {
public void Subscribe(TravelEncounterSystem travelSys) {
public void Unsubscribe(TravelEncounterSystem? travelSys = null) {
public bool QueueSignal(string broadcastId) {
public IReadOnlyList<string> TickRadio() {
public PatrolRadioHooksState CaptureState() {
public void RestoreState(PatrolRadioHooksState? state) {
public void ResetForTest() {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/FactionRadioTypes.cs`

### `Assets/Ashfall.Core/Radio/FactionRadioTypes.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 81 lines / 2848 bytes.
- SHA-256: `621f5358e969d7dc78904d690b09f93d24fe21e47946ff6e05233dbba8d7d4dc`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RadioEventKind
public readonly struct RadioIntercept
public string FactionId { get; }
public string Callsign { get; }
public float FrequencyMhz { get; }
public RadioEventKind Kind { get; }
public string Message { get; }
public int SignalStrength { get; } // 1..9 (S-units)
public int Day { get; }
public sealed class FactionRadioChannel
public string FactionId { get; set; } = string.Empty;
public string Callsign { get; set; } = string.Empty;
public float FrequencyMhz { get; set; } = 100.0f;
public List<string> InterceptChatter { get; set; } = new();
public List<string> ParleyResolutions { get; set; } = new();
public List<string> RaidWarnings { get; set; } = new();
public List<string> TradeReactions { get; set; } = new();
public interface IFactionRadioProvider
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
