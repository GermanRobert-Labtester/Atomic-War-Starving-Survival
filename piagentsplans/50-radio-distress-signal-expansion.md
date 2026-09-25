# Plan 50 — Distress Signal Assessment, Triage and Rescue Decision Runtime

> **Rebuild status:** SUBSTANTIALLY INTEGRATED TRIAGE/RESCUE RUNTIME — RESIDUAL REACHABILITY AND SAFETY AUDIT
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-2`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round2-2026-09-25`
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

- The original boundary was sound—Plan 50 should not own signal rows or create a second mission manager—but the proposed assessment layer is now substantially represented by `SignalAuthenticityEvaluator`, `DistressRescueMissionManager`, `DistressDestinationResolver`, `SignalTrustLedger` and `DistressFollowUpScheduler`.
- The current player decision is a staged rescue lifecycle: heard, identified, dispatched, reached and terminal rescued/failed/ambush. Authenticity and staleness are persisted observations, while trust is a separate campaign ledger with bounded consequences.
- The rebase focuses on truthful UI projection, exactly-once consequences, legacy restore and the boundary between the signal catalog, mission owner, expedition owner and follow-up scheduler. It does not infer hidden outcomes or duplicate mission state in a triage object.

**Bounded outcome:** Do not create `DistressAssessment`, `rescue_triage_rules.json` or a second rescue generator as the old plan proposed. Current Core already owns signal authenticity, trust, destination resolution, rescue stages, deadlines, follow-ups and persistence. The correct next package is a focused audit of the existing route and its player-facing decision surface.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `RadioDistressSystem` owns signal lifecycle; `DistressRescueMissionManager` owns mission stages and deadlines; `SignalTrustLedger` owns trust consequences; `DistressFollowUpScheduler` owns authored follow-up timing.
- The two distress signal catalogs are present with schema version 1 and 25 + 23 broadcast rows; the rescue manager registers authored missions and the radio host binds the systems together.
- `RadioPanel` projects rescue stage, deadline and authenticity analysis, while `RadioHostSession` captures/restores the radio save state including mission, trust and follow-up state.
- The current implementation is beyond the old assessment/triage proposal. A future change must prove player-visible choices and consequences without creating a parallel mission lifecycle.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 cultural/archive fact projection principles.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Retire the proposed standalone assessment/triage rules catalog unless a current owner cannot express a required policy.
- Document the exact signal → authenticity → destination → mission → trust → follow-up flow and its single authority for each field.
- Add a UI/host reachability audit for identified, dispatched, expired, ignored, ambushed and rescued states.
- Preserve exactly-once rewards, trust transitions, ignore consequences and follow-up firing across save/restore.

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
| signal definitions and active/resolved signal state | RadioDistressSystem | `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs` | Owns signal lifecycle and catalog binding. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` | Sole mission owner; no second rescue generator. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | `Assets/Ashfall.Core/Radio/SignalAuthenticityEvaluator.cs` | Observation only; it does not dispatch. |
| canonical destination and route association | DistressDestinationResolver | `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs` | Resolves signal destinations against current route/location authority. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs` | Owns exactly-once trust deltas and save state. |
| composition, save capture/restore and event projection | RadioHostSession | `src/Host/RadioHostSession.cs; src/UI/RadioPanel.cs` | Host owns wiring and presentation projection. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Distress Signal Assessment, Triage and Rescue Decision Runtime
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ RadioDistressSystem
│   signal definitions and active/resolved signal state
│ DistressRescueMissionManager
│   rescue stages, deadlines, dispatch association and rewards
│ SignalAuthenticityEvaluator
│   deterministic authenticity/staleness classification
│ DistressDestinationResolver
│   canonical destination and route association
│ SignalTrustLedger
│   answer/ignore/rescue trust consequences
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

1. **Preserve current state ownership.** RadioDistressSystem owns signal definitions and active/resolved signal state: Owns signal lifecycle and catalog binding.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| signal definitions and active/resolved signal state | RadioDistressSystem | `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs` | Owns signal lifecycle and catalog binding. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs` | Sole mission owner; no second rescue generator. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | `Assets/Ashfall.Core/Radio/SignalAuthenticityEvaluator.cs` | Observation only; it does not dispatch. |
| canonical destination and route association | DistressDestinationResolver | `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs` | Resolves signal destinations against current route/location authority. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs` | Owns exactly-once trust deltas and save state. |
| composition, save capture/restore and event projection | RadioHostSession | `src/Host/RadioHostSession.cs; src/UI/RadioPanel.cs` | Host owns wiring and presentation projection. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. Load signal catalog through the current radio owner
2. intercept and triangulate a signal
3. evaluate authenticity/staleness exactly once when observed
4. resolve a canonical destination
5. create or associate one existing rescue mission
6. dispatch through expedition authority
7. resolve terminal outcome and claim idempotent rewards
8. record trust/follow-up consequences and project truthful UI

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Signal state and mission state are separate: a discovered signal can have no mission, an active mission, or a terminal result.
- Authenticity assessment is persisted once checked and does not reroll on repeated display.
- Trust and ignore consequences are exactly-once per signal and survive legacy missing fields.
- Follow-up entries are scheduled once per authored parent/trigger and expire safely when removed from the catalog.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A signal cannot create two active missions for the same destination/association.
- Unknown or unresolved destination fails closed with a named reason and no mission mutation.
- Ignore/answer/rescue deltas are bounded and cannot double-fire across tick, reload or repeated panel refresh.
- The UI must show known analysis and deadline state without revealing hidden outcome rolls.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Plan 107 signal catalogs remain the sole signal-row authority.
- No new frequency, coordinate, authenticity or rescue outcome rows are added by this plan.
- Any authored mission/follow-up change must resolve against current signal, expedition and faction IDs.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing radio save state, mission save state, trust ledger and follow-up scheduler fields.
- No new save section is justified by this residual audit.
- Legacy missing mission/trust/follow-up fields restore neutral and do not replay historical rewards.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Authenticity checks consume the injected seeded RNG and never reroll on refresh.
- Mission deadline, tie-break and arrival association are ordinal-stable.
- Two identical seeded runs produce identical stage, reward and follow-up traces.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Signal intercepted/triangulated/expired/resolved facts come from `RadioDistressSystem`.
- Mission stage changes and ignore consequences come from `DistressRescueMissionManager`.
- Follow-up firing is an authored fact; the host presents it without changing mission authority.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/RadioHostSession.cs
- src/Host/RadioSaveStore.cs
- src/UI/RadioPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Signal fragments and follow-ups are authored data, not generated hidden truth.
- The triage experience should communicate uncertainty and cost without spoiling the outcome.
- Consequences should be specific and ethically legible, consistent with the project tone rules.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A panel creates a mission or changes trust directly. | RadioDistressSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A repeated dispatch duplicates rewards or trust. | DistressRescueMissionManager | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A restored mission loses its expedition association and resolves the wrong destination. | SignalAuthenticityEvaluator | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A follow-up fires twice or fires for an undiscovered signal. | DistressDestinationResolver | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | The UI labels an unverified signal as genuine. | SignalTrustLedger | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioDistressSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressRescueMissionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/SignalTrustTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — owner census | Read current signal, mission, trust, destination, follow-up and host code. | No old proposed owner remains unclassified. | No production path until the owning implementation package is separately claimed. |
| 1 — lifecycle proof | Trace all mission stages and exactly-once consequences. | A single replay trace covers each terminal branch. | No production path until the owning implementation package is separately claimed. |
| 2 — host projection | Audit the radio panel and host save route. | Every state has truthful visible feedback and restore. | No production path until the owning implementation package is separately claimed. |
| 3 — residual decision | Implement only a proven reachability or safety gap. | No parallel triage authority is introduced. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs | READ ONLY; MODIFY only for a proven mission bug | Sole rescue owner |
| Assets/Ashfall.Core/Radio/SignalAuthenticityEvaluator.cs | READ ONLY | Assessment owner |
| src/Host/RadioHostSession.cs | READ ONLY unless host route gap is proven | Composition/save |
| src/UI/RadioPanel.cs | READ ONLY unless projection gap is proven | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second mission or signal state store. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Leaking hidden authenticity/outcome data. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing trust semantics while fixing UI. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating old triage plan names as current APIs. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new distress catalog.
- No new rescue mission manager.
- No standalone assessment DTO without owner evidence.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the planning file.
- Future Core changes require focused mission, persistence and replay targets plus the prior save fixture.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current owner chain is explicit.
- No second triage authority is proposed.
- All stage/trust/follow-up exactly-once contracts are named.
- Focused commands cover lifecycle, persistence and replay.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Retire the proposed standalone assessment/triage rules catalog unless a current owner cannot express a required policy.
- Document the exact signal → authenticity → destination → mission → trust → follow-up flow and its single authority for each field.
- Add a UI/host reachability audit for identified, dispatched, expired, ignored, ambushed and rescued states.
- Preserve exactly-once rewards, trust transitions, ignore consequences and follow-up firing across save/restore.

## MUST NOT DO

- No new distress catalog.
- No new rescue mission manager.
- No standalone assessment DTO without owner evidence.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RadioDistressSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressRescueMissionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/SignalTrustTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — owner census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: signal definitions and active/resolved signal state → RadioDistressSystem; rescue stages, deadlines, dispatch association and rewards → DistressRescueMissionManager; deterministic authenticity/staleness classification → SignalAuthenticityEvaluator; canonical destination and route association → DistressDestinationResolver; answer/ignore/rescue trust consequences → SignalTrustLedger; composition, save capture/restore and event projection → RadioHostSession. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 50.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 50 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by RadioDistressSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs`

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


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Radio/DistressRescueMissionManager.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Radio/SignalAuthenticityEvaluator.cs`

### `Assets/Ashfall.Core/Radio/SignalAuthenticityEvaluator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 157 lines / 6736 bytes.
- SHA-256: `46b5075c30e5e883f32a2d929f631e3648a9998a5e64fbc2f8cde14f4a22649e`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SignalAuthenticityCategory
public sealed class SignalAuthenticityCheckResult
public bool CheckPerformed { get; set; }
public bool ThreatDetected { get; set; }
public bool StalenessDetected { get; set; }
public SignalAuthenticityCategory Assessment { get; set; } = SignalAuthenticityCategory.Unknown;
public string SkillId { get; set; } = string.Empty;
public int SkillBonus { get; set; }
public int Difficulty { get; set; }
public int DetectionChance { get; set; }
public int Roll { get; set; }
public static class SignalAuthenticityEvaluator
public const int DeceptionDifficulty = 65;
public const string SkillSignalEar = "skill_signal_ear";
public const string SkillWatchful = "skill_watchful";
public const string SkillColdAnalysis = "skill_cold_analysis";
public const int BonusSignalEar = 15;
public const int BonusWatchful = 10;
public const int BonusColdAnalysis = 8;
public static SignalAuthenticityCheckResult Evaluate( DistressSignalDefinition def, int daysTraced, string survivorId, Func<string, string, bool>? hasSkill, ISeededRng rng)
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs`

### `Assets/Ashfall.Core/Radio/DistressDestinationResolver.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 417 lines / 26793 bytes.
- SHA-256: `871b3c08b74bfca2aa20bb386de1b504602cfe005e63aa7985c41209a5104635`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DistressDestinationResolution
public bool IsValid { get; set; }
public string RawLocationId { get; set; } = string.Empty;
public string DestinationId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public int DistanceTicks { get; set; }
public int DangerLevel { get; set; }
public string ScavengingTableId { get; set; } = string.Empty;
public List<string> LootCategories { get; set; } = new List<string>();
public string ResolutionMode { get; set; } = string.Empty; // Direct, Aliased, SignalThematic, Fallback
public string? ErrorCode { get; set; }
public sealed class CanonicalDestinationInfo
public string Id { get; }
public string DisplayName { get; }
public int DistanceTicks { get; }
public int DangerLevel { get; }
public string ScavengingTableId { get; }
public IReadOnlyList<string> LootCategories { get; }
public sealed class DistressDestinationResolver
public const int SchemaVersion = 1;
public static DistressDestinationResolver Default => s_defaultInstance.Value;
public int TotalCanonicalDestinations => _canonical.Count;
public int TotalAliases => _aliasMap.Count;
public bool IsCanonicalDestination(string destinationId) {
public CanonicalDestinationInfo? GetCanonicalInfo(string destinationId) {
public IReadOnlyCollection<CanonicalDestinationInfo> GetAllCanonical() => _canonical.Values;
public DistressDestinationResolution Resolve(string? rawLocationId) {
public DistressDestinationResolution ResolveSignal(DistressSignalDefinition? signal) {
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


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/Radio/DistressFollowUpScheduler.cs`

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


# Appendix B.09 — Current Code Architecture: `src/Host/RadioSaveStore.cs`

### `src/Host/RadioSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 66 lines / 3015 bytes.
- SHA-256: `03481859898a30a00c9b272bb830c1831ee13b4538a871d9230e3c09715dbee9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class RadioSaveStore
public const string FileName = "radio_save.json";
public const string SectionName = "radio";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(RadioSaveState state) => s_store.CaptureBare(state);
public static RadioSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(RadioSaveState state) => s_store.CaptureBare(state);
public static RadioSaveState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(RadioSaveState state, string pathOverride = null!) =>
public static RadioSaveState? TryLoad(string pathOverride = null!) =>
public static string TryCapturePersisted(RadioSaveState state) => s_store.CapturePersisted(state);
```


# Appendix B.10 — Current Code Architecture: `src/UI/RadioPanel.cs`

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


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/radio_distress_signals.json`

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


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/radio_distress_signals_expansion.json`

### `Assets/StreamingAssets/Data/radio_distress_signals_expansion.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 47132 bytes / 47038 characters.
- SHA-256: `0b6e343919fd9658f1ec8643968e6d927b79da2fe820861766b627653aba1937`.
- Root keys: `radio_broadcasts`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
radio_broadcasts: min=23, max=23, observed_paths=1
radio_broadcasts[].message_fragments: min=4, max=5, observed_paths=2
```

Representative record fields:

- `audio_cue`
- `authenticity`
- `days_to_trace`
- `follow_up_signals`
- `frequency_id`
- `frequency_mhz`
- `message_fragments`
- `moral_choice_id`
- `npc_id`
- `outcome_type`
- `resolve_quest_id`
- `revealed_items`
- `revealed_location`
- `source_name`


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RadioDistressSystemTests.cs`

### `Ashfall.Core.Tests/Radio/RadioDistressSystemTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 107; SHA-256: `6e3fb5b799e769a9c1a68d889d4d9676675a4fc832275673d42f3f3443777544`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DistressLifecycle_Intercept_Triangulate_Dispatch_Resolve_WorksCleanly
Distress_TickingDaily_DecrementsCountdown_AndExpiresOverdueSignal
Distress_CaptureAndRestoreState_PreservesActiveAndResolvedSignals
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/DistressRescueMissionTests.cs`

### `Ashfall.Core.Tests/Radio/DistressRescueMissionTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 220; SHA-256: `c75ac769dcabbbea2d2f89dc47475dab9722dd60dca151ca16a8c78027bb9273`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AllRescueMissions_AreRegisteredWithCorrectCorrelation
StagedProgression_MonotonicallyAdvancesToRescued
DeadlineMath_FailsMissionWhenArrivingPastExpiry
RaiderTrap_TransitionsToAmbushThenSurvived
IdempotentRewards_CannotBeClaimedTwice
DailyTick_ExpiresUnreachedMissionsPastDeadline
SaveLoad_PreservesStagesAndClaimedReceipts
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/RescueSignalRuntimePersistenceTests.cs`

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


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/SignalTrustTests.cs`

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


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs`

### `Ashfall.Core.Tests/Radio/DistressFollowUpPopulationReplayTests.cs`

- Current test declarations: Fact=6, Theory=1, InlineData=0.
- File lines: 532; SHA-256: `eb2606a9f90233d15b668e766d9913053422b881066d70bb782d627b2b3c6009`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PopulationCensus_MatchesThePostRemediationAuthoredTable
EveryAuthoredFollowUp_ReplaysThroughV6ExactlyOnce
ConsequenceExpiry_AppliesOnceAndStillSchedulesTheAuthoredFollowUp
UndiscoveredPopulationSample_RemainsSilentAndUnscheduled
PopulationReplay_IsStableAcrossTwoRuns
SameDayPopulationFiresInOrdinalParentOrder
RemovedFollowUpIdsInV6PendingStateExpireWithoutPresentation
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/AcousticDirectionFindingCatalog.cs`

### `Assets/Ashfall.Core/Radio/AcousticDirectionFindingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 109 lines / 5203 bytes.
- SHA-256: `548604cd1ff0097e48ad33245f082deafa635e6d1a2cf2dea8a16d82faa1bb02`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class AcousticArrayProfile
public string id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public string sensor_class = string.Empty;
public string baseline_class = "medium"; // short | medium | long
public float noise_tolerance; // 0..1 normalized
public List<string> signal_bands = new List<string>();
public float base_detection_range_km;
public float confidence_gain; // per observation, normalized
public string warning_window_class = "window_medium";
public float power_demand_w;
public float maintenance_wear;
public List<string> install_item_ids = new List<string>();
public List<string> repair_item_ids = new List<string>();
public string dampening_item_id = string.Empty;
public List<string> tags = new List<string>();
public bool Validate(out string error) {
public sealed class AcousticSignalBandDef
public string id = string.Empty;
public string display_name = string.Empty;
public bool is_hostile;
public sealed class AcousticWarningWindowDef
public string id = string.Empty;
public string display_name = string.Empty;
public int tier = 1;
public sealed class AcousticDirectionFindingCatalogDto
public int schema_version = 1;
public List<AcousticArrayProfile> arrays = new List<AcousticArrayProfile>();
public List<AcousticSignalBandDef> signal_bands = new List<AcousticSignalBandDef>();
public List<AcousticWarningWindowDef> warning_windows = new List<AcousticWarningWindowDef>();
public sealed class AcousticDirectionFindingCatalog
public IReadOnlyDictionary<string, AcousticArrayProfile> Arrays => _arrays;
public IReadOnlyDictionary<string, AcousticSignalBandDef> Bands => _bands;
public IReadOnlyDictionary<string, AcousticWarningWindowDef> WarningWindows => _windows;
public AcousticArrayProfile? GetArray(string arrayId) =>
public bool IsBandHostile(string bandId) =>
public AcousticWarningWindowDef? GetWindow(string windowId) =>
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/RadioSave.cs`

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


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs`

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


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`

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


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Radio/DirectionFindingCatalog.cs`

### `Assets/Ashfall.Core/Radio/DirectionFindingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 247 lines / 11955 bytes.
- SHA-256: `960b45f69b8b0f3e5984cfa780e97b597e280b21889f4bcdffc4d9c5e514dc36`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DirectionFindingArrayDef
public string array_id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public string baseline_class = "short"; // short | medium | long
public string station_id = string.Empty;
public float baseline_x_km;
public float baseline_y_km;
public List<string> band_ids = new List<string>();
public float base_bearing_error_deg = 5f;
public float skywave_uncertainty_mult = 1.4f;
public float power_demand_w;
public int calibration_interval_days = 7;
public List<string> install_item_ids = new List<string>();
public List<string> tags = new List<string>();
public bool Validate(out string error) {
public sealed class DirectionFindingBandDef
public string band_id = string.Empty;
public string display_name = string.Empty;
public float min_mhz;
public float max_mhz;
public sealed class DirectionFindingSkywaveSeasonDef
public string season_id = string.Empty;
public List<string> weather_tags = new List<string>();
public float uncertainty_mult = 1f;
public float confidence_mult = 1f;
public float false_signature_chance;
public sealed class DirectionFindingFingerprintDef
public string fingerprint_id = string.Empty;
public string signal_id = string.Empty;
public string display_name = string.Empty;
public string mapped_location_id = string.Empty;
public float identity_confidence = 0.5f;
public List<string> tags = new List<string>();
public sealed class DirectionFindingCatalogDto
public int schema_version = 1;
public List<DirectionFindingArrayDef> arrays = new List<DirectionFindingArrayDef>();
public List<DirectionFindingBandDef> bands = new List<DirectionFindingBandDef>();
public List<DirectionFindingSkywaveSeasonDef> skywave_seasons = new List<DirectionFindingSkywaveSeasonDef>();
public List<DirectionFindingFingerprintDef> fingerprints = new List<DirectionFindingFingerprintDef>();
public sealed class DirectionFindingCatalog
public IReadOnlyDictionary<string, DirectionFindingArrayDef> Arrays => _arrays;
public IReadOnlyDictionary<string, DirectionFindingBandDef> Bands => _bands;
public IReadOnlyList<DirectionFindingSkywaveSeasonDef> SkywaveSeasons => _seasons;
public IReadOnlyDictionary<string, DirectionFindingFingerprintDef> FingerprintsBySignal => _fingerprintsBySignal;
public DirectionFindingArrayDef? GetArray(string arrayId) =>
public DirectionFindingArrayDef? GetArrayByStation(string stationId) {
public DirectionFindingFingerprintDef? GetFingerprintForSignal(string signalId) =>
public DirectionFindingSkywaveSeasonDef? ResolveSkywave(string weatherCondition) {
public static class DirectionFindingCatalogLoader
public const string DefaultFileName = "direction_finding_catalog.json";
public static DirectionFindingCatalogDto Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static DirectionFindingCatalog Build(DirectionFindingCatalogDto dto) {
public static void Validate(DirectionFindingCatalogDto dto) {
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`

### `Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`

- Current test declarations: Fact=20, Theory=0, InlineData=0.
- File lines: 396; SHA-256: `3329957b141f36045545374367496fd8d2cf91d0384b458225c57e38cac2e61b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
B1_001_HardcodedVsJson_Parity_ZeroMismatch
B1_002_DuplicateStation_Rejected
B1_003_InvalidFrequency_Rejected
B1_004_MissingCatalog_Throws_NoSilentFallback
B1_005_AllProductionConstructors_LoadJson
B1_006_CurrentSchedule_Deterministic
B1_007_NextSlot_Stable
B1_008_ResearchDoesNotGateSchedules
B1_009_EquipmentGatesTuningCapability
B1_010_SignalReasons_ComposeDeterministically
B1_011_VinylBrownout_GivesNoMorale
B1_012_RetryDoesNotDoubleRecord
B1_013_OldRadioSave_Loads
B1_014_UnknownStationOverride_Retained
B1_015_FactionContent_ComesThroughDataPath
B1_016_RestoreEmitsNoTransitionEvent
B1_017_StationSlots_Cover24Hours
B1_018_SignalStrength_ReportsCorrectQualityBands
B1_019_CoreSourceGate_ZeroHardcodedStationDefs
B1_020_RadioCatalogSelftest_ChecksPass
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/SignalTriangulationSystemTests.cs`

### `Ashfall.Core.Tests/SignalTriangulationSystemTests.cs`

- Current test declarations: Fact=19, Theory=0, InlineData=0.
- File lines: 293; SHA-256: `970c520c5069bef22a24dec51047b7d55581f58d0e14835e692833178946e0bf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RecordObservation_AcceptsValidInput
RecordObservation_RejectsNull
RecordObservation_RejectsInvalidBearing
RecordObservation_RejectsInvalidError
RecordObservation_RaisesOnObservationRecorded
Triangulate_ReturnsNullWithTooFewObservations
Triangulate_ReturnsCandidateWithEnoughObservations
Triangulate_ConfidenceIncreasesWithMoreObservations
Triangulate_UncertaintyDecreasesWithMoreObservations
Triangulate_DiscoveryRequiresMinObservationsAndConfidence
Triangulate_RaisesOnLocationRevealed
Triangulate_HighNoiseReducesConfidence
SameObservations_SameCandidate
CaptureRestore_RoundTrips
CaptureState_StableChecksum
DiscoveredLocations_SurviveSaveLoad
GetObservationCount_ReturnsCorrectCount
GetCandidate_ReturnsNullForUnknownSignal
IsLocationDiscovered_ReturnsFalseForUnknown
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`

### `Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 139; SHA-256: `8a3df262d6f4fe50519e209619dc9d46609e1ac21354aecbb487d04eef0d2e20`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RadioStationCatalog_JsonMatchesHardcodedDefaults_ExactParity
RadioStationCatalog_LoadFromDataDirectory_Succeeds
RadioStationCatalog_FindStationAtFrequency_ResolvesNearFrequencies
RadioStationCatalog_StateOverrides_PersistAndRestore
RadioStationCatalog_NoHardcodedStationDefaultsInCore_AuthorityGate
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/Radio/DistressStageResolverTests.cs`

### `Ashfall.Core.Tests/Radio/DistressStageResolverTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 450; SHA-256: `8f27c97a9f9f03ebfe6087b9ed12d7d57cd075203b24ba690dc94f7f835b29b5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SignalProgressesThroughAllStages
SignalClarityIncreasesOverTime
SignalTextChangesAtEachStage
OutcomeHintsRevealAtExpectedStages
MultiStageProgressionIsDeterministic
NoFragmentSignalReturnsLegacyFallbackSentinel
StageSelectionMatchesLegacyConsumersForAllAuthoredSignals
AllAuthoredSignalsLoadWithMultiStageFragments
PrimaryAuthorityWinsForCrossFileDuplicateIds
OutcomeHintFieldBindsFromJson
Validator_AcceptsWellFormedStages
Validator_RejectsDuplicateStageDay
Validator_RejectsDescendingStageDay
Validator_RejectsDecreasingClarity
Validator_RejectsEmptyStageText
Validator_RejectsMissingFragmentsAndOutOfRangeClarity
Validator_RealCatalogsPassWithPrimaryWinsWarningsOnly
```


# Appendix H.27 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| signal definitions and active/resolved signal state | RadioDistressSystem | rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| signal definitions and active/resolved signal state | RadioDistressSystem | deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | Owner emits/reads a typed fact; no mirror state. |
| signal definitions and active/resolved signal state | RadioDistressSystem | canonical destination and route association | DistressDestinationResolver | Owner emits/reads a typed fact; no mirror state. |
| signal definitions and active/resolved signal state | RadioDistressSystem | answer/ignore/rescue trust consequences | SignalTrustLedger | Owner emits/reads a typed fact; no mirror state. |
| signal definitions and active/resolved signal state | RadioDistressSystem | composition, save capture/restore and event projection | RadioHostSession | Owner emits/reads a typed fact; no mirror state. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | signal definitions and active/resolved signal state | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | Owner emits/reads a typed fact; no mirror state. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | canonical destination and route association | DistressDestinationResolver | Owner emits/reads a typed fact; no mirror state. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | answer/ignore/rescue trust consequences | SignalTrustLedger | Owner emits/reads a typed fact; no mirror state. |
| rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | composition, save capture/restore and event projection | RadioHostSession | Owner emits/reads a typed fact; no mirror state. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | signal definitions and active/resolved signal state | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | canonical destination and route association | DistressDestinationResolver | Owner emits/reads a typed fact; no mirror state. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | answer/ignore/rescue trust consequences | SignalTrustLedger | Owner emits/reads a typed fact; no mirror state. |
| deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | composition, save capture/restore and event projection | RadioHostSession | Owner emits/reads a typed fact; no mirror state. |
| canonical destination and route association | DistressDestinationResolver | signal definitions and active/resolved signal state | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| canonical destination and route association | DistressDestinationResolver | rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| canonical destination and route association | DistressDestinationResolver | deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | Owner emits/reads a typed fact; no mirror state. |
| canonical destination and route association | DistressDestinationResolver | answer/ignore/rescue trust consequences | SignalTrustLedger | Owner emits/reads a typed fact; no mirror state. |
| canonical destination and route association | DistressDestinationResolver | composition, save capture/restore and event projection | RadioHostSession | Owner emits/reads a typed fact; no mirror state. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | signal definitions and active/resolved signal state | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | Owner emits/reads a typed fact; no mirror state. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | canonical destination and route association | DistressDestinationResolver | Owner emits/reads a typed fact; no mirror state. |
| answer/ignore/rescue trust consequences | SignalTrustLedger | composition, save capture/restore and event projection | RadioHostSession | Owner emits/reads a typed fact; no mirror state. |
| composition, save capture/restore and event projection | RadioHostSession | signal definitions and active/resolved signal state | RadioDistressSystem | Owner emits/reads a typed fact; no mirror state. |
| composition, save capture/restore and event projection | RadioHostSession | rescue stages, deadlines, dispatch association and rewards | DistressRescueMissionManager | Owner emits/reads a typed fact; no mirror state. |
| composition, save capture/restore and event projection | RadioHostSession | deterministic authenticity/staleness classification | SignalAuthenticityEvaluator | Owner emits/reads a typed fact; no mirror state. |
| composition, save capture/restore and event projection | RadioHostSession | canonical destination and route association | DistressDestinationResolver | Owner emits/reads a typed fact; no mirror state. |
| composition, save capture/restore and event projection | RadioHostSession | answer/ignore/rescue trust consequences | SignalTrustLedger | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Retire the proposed standalone assessment/triage rules catalog unless a current owner cannot express a required policy. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Document the exact signal → authenticity → destination → mission → trust → follow-up flow and its single authority for each field. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Add a UI/host reachability audit for identified, dispatched, expired, ignored, ambushed and rescued states. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve exactly-once rewards, trust transitions, ignore consequences and follow-up firing across save/restore. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`

### `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 693 lines / 29574 bytes.
- SHA-256: `f3944b75e7ba43a711159f11c8f5d8fa6099f20e240db2d9e2d089be768457d6`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=16; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class RadioObservation
public string signalId = string.Empty;
public string stationId = string.Empty;
public int day = 0;
public float hour = 0f;
public float bearingDegrees = 0f;      // 0-360, clockwise from north
public float errorDegrees = 5f;        // ± uncertainty in bearing
public float signalStrength = 0f;      // 0..1
public float noiseLevel = 0f;          // 0..1
public float frequencyMhz = 0f;
public string weatherCondition = "Clear";
public float operatorSkill = 0.5f;     // 0..1
public float polarizationFade = 0f;
public class TriangulationCandidate
public string locationId = string.Empty;
public string displayName = string.Empty;
public float estimatedX = 0f;
public float estimatedY = 0f;
public float uncertaintyRadiusKm = 0f;
public float confidence = 0f;          // 0..1 location confidence
public float identityConfidence = 0f;
public int observationCount = 0;
public bool isFalseSignature = false;
public class StationBaselineEntry
public string stationId = string.Empty;
public float xKm = 0f;
public float yKm = 0f;
public string arrayId = string.Empty;
public class TriangulationState
public string systemId = SignalTriangulationSystem.SystemId;
public List<RadioObservation> observations = new List<RadioObservation>();
public List<TriangulationCandidate> candidates = new List<TriangulationCandidate>();
public List<string> discoveredLocationIds = new List<string>();
public List<StationBaselineEntry> stationBaselines = new List<StationBaselineEntry>();
public string activeSignalId = string.Empty;
public int lastCalibrationDay = 0;
public class SignalTriangulationSystem
public const string SystemId = "signal_triangulation_system";
public const int MinObservationsForHypothesis = 2;
public const int MinObservationsForDiscovery = 3;
public const float ConfidenceThreshold = 0.7f;
public const float BaseUncertaintyKm = 50f;
public const float ObservationUncertaintyReduction = 0.4f;
public const float WeatherNoisePenalty = 0.15f;
public const float MaxBearingErrorDegrees = 15f;
public const float SkywaveDefaultUncertaintyMult = 1.55f;
public const float PolarizationUncertaintyMult = 1.25f;
public event Action<string> OnFrequencyLocked;
public event Action<RadioObservation> OnObservationRecorded;
public event Action<string> OnAntennaCalibrationChanged;
public event Action<TriangulationCandidate> OnCandidateChanged;
public event Action<string> OnTriangulationCompleted;
public event Action<string> OnTriangulationFailed;
public event Action<string> OnLocationRevealed;
public event Action<TriangulationState> OnStateChanged;
public TriangulationState State => _state;
public IReadOnlyList<RadioObservation> Observations => _state.observations;
public IReadOnlyList<TriangulationCandidate> Candidates => _state.candidates;
public IReadOnlyList<string> DiscoveredLocations => _state.discoveredLocationIds;
public IReadOnlyDictionary<string, StationBaselineEntry> StationBaselines => _baselines;
public DirectionFindingCatalog? Catalog => _catalog;
public void LoadCatalog(DirectionFindingCatalog catalog) {
public void LoadCatalog(DirectionFindingCatalogDto dto) {
public void RegisterStationBaseline(string stationId, float xKm, float yKm, string arrayId = "") {
public bool TryGetStationBaseline(string stationId, out StationBaselineEntry entry) {
public void NotifyAntennaCalibration(string stationId, int day = 0) {
public bool RecordObservation(RadioObservation obs) => RecordObservation(obs, rng: null);
public bool RecordObservation(RadioObservation obs, ISeededRng? rng) {
public TriangulationCandidate? Triangulate(string signalId, ISeededRng rng) {
public bool IsLocationDiscovered(string locationId) =>
public TriangulationCandidate? GetCandidate(string signalId) {
public int GetObservationCount(string signalId) {
public TriangulationState CaptureState() {
public void RestoreState(TriangulationState saved) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs`

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


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioTuner.cs`

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


# Appendix Q.563 — Additional Current Architecture Evidence: `src/Main.UiTests.Economy.cs`

### `src/Main.UiTests.Economy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 223 lines / 10953 bytes.
- SHA-256: `234ba8b7ccc3d2591fc75917c57dd2cd872c3ece14767694802119b070aa0774`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RescuedArcProjection.cs`

### `Assets/Ashfall.Core/Radio/RescuedArcProjection.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 152 lines / 5779 bytes.
- SHA-256: `cc461720f207995ef0eda907ac5e3c7aa0b76df3517f727fefe503ef655bcc42`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RescuedArcPhase
public sealed class RescuedArcProjection
public string SignalId { get; }
public string QuestId { get; }
public string DestinationId { get; }
public RescuedArcPhase Phase { get; }
public int RecoveryDaysLeft { get; }
public bool IsTerminal { get; }
public string StatusSummary { get; }
public string JournalKey { get; }
public static RescuedArcProjection Project(DistressRescueMission mission, int currentDay, int recoveryDaysTotal = 5) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/Host/RadioCatalogSelfTest.cs`

### `src/Host/RadioCatalogSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 192 lines / 7703 bytes.
- SHA-256: `0bad438e3afceb561d428a032cb82900e486fc211458cf313ca558e68fd8ae26`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class RadioCatalogSelfTest
public static int Run(string dataDirectory) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

### `src/Host/PanelBindLifecycleSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1371 lines / 70877 bytes.
- SHA-256: `a96e666a51d3760bb8e972a3acf6ab4fed7a74328402409cdbe372d614a9fe1c`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class PanelBindLifecycleSelfTest
public static int Run(string dataDirectory = "") {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

### `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2073 lines / 130366 bytes.
- SHA-256: `7588dbb7ed053936964371ce06c49160f772cb9fffd2e7d519884ab430f044c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContentUtilizationScanner
public static bool IsNarrativeSubdirectoryFile(string relativePath) {
public static bool IsAuthoritativeCatalog(string fileName) {
public ContentUtilizationGraph Scan() {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs`

### `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 238 lines / 9176 bytes.
- SHA-256: `2b9ebe46736f7a205d8dd9dad22c54447a536959865091c41b321d931e074a07`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NumbersStationCipherEntry
public string Id { get; set; } = string.Empty;
public string StationNickname { get; set; } = string.Empty;
public float TransmissionFrequencyKhz { get; set; }
public string ModulationMode { get; set; } = string.Empty;
public int ChimeIntervalSeconds { get; set; }
public string TimestampRelative { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public string Prose { get; set; } = string.Empty;
public sealed class SeismicFaultAlarmEntry
public string Id { get; set; } = string.Empty;
public string StationId { get; set; } = string.Empty;
public float RichterMagnitude { get; set; }
public float DepthKm { get; set; }
public string AlertTier { get; set; } = string.Empty;
public string TimestampRelative { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public string Prose { get; set; } = string.Empty;
public sealed class EmpSnifferLogEntry
public string Id { get; set; } = string.Empty;
public string DetectorId { get; set; } = string.Empty;
public float E1FieldStrengthVoltsPerMeter { get; set; }
public float RiseTimeNanoseconds { get; set; }
public string PulseClassification { get; set; } = string.Empty;
public string TimestampRelative { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public string Prose { get; set; } = string.Empty;
public sealed class BunkerWiretapEntry
public string Id { get; set; } = string.Empty;
public string InterceptChannel { get; set; } = string.Empty;
public string TargetFaction { get; set; } = string.Empty;
public float AudioClarityScore { get; set; }
public string SpeakerIdentities { get; set; } = string.Empty;
public string TimestampRelative { get; set; } = string.Empty;
public List<string> Tags { get; set; } = new List<string>();
public string Prose { get; set; } = string.Empty;
public sealed class SignalIntelligenceCatalog
public IReadOnlyList<NumbersStationCipherEntry> CipherEntries => _cipherEntries;
public IReadOnlyList<SeismicFaultAlarmEntry> SeismicEntries => _seismicEntries;
public IReadOnlyList<EmpSnifferLogEntry> EmpEntries => _empEntries;
public IReadOnlyList<BunkerWiretapEntry> WiretapEntries => _wiretapEntries;
public int TotalCount => _cipherEntries.Count + _seismicEntries.Count + _empEntries.Count + _wiretapEntries.Count;
public static SignalIntelligenceCatalog LoadFromDirectory(string directoryPath) {
public NumbersStationCipherEntry? GetCipher(string id) {
public SeismicFaultAlarmEntry? GetSeismic(string id) {
public EmpSnifferLogEntry? GetEmp(string id) {
public BunkerWiretapEntry? GetWiretap(string id) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/DistressAudioCueResolver.cs`

### `Assets/Ashfall.Core/Radio/DistressAudioCueResolver.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 59 lines / 2611 bytes.
- SHA-256: `df4b20365fe5bf3b85558f5d25cdc435c8c8b4386f904713473f2173b2894fb6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DistressAudioCueResolver
public static string Resolve(DistressSignalDefinition? signal, int stageIndex) {
public static string ResolveForDay(DistressSignalDefinition? signal, int campaignDay) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`

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


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`

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


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/DistressStageResolver.cs`

### `Assets/Ashfall.Core/Radio/DistressStageResolver.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 92 lines / 4415 bytes.
- SHA-256: `8a6a6e8f59ec96648f790f35b222a911847d1f462db099a29d2862252239c4a2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct DistressStageView
public int StageIndex { get; }
public DistressMessageFragment Fragment { get; }
public static class DistressStageResolver
public static DistressStageView? Resolve(DistressSignalDefinition? signal, int campaignDay) {
public static int ResolveStageIndex(DistressSignalDefinition? signal, int campaignDay) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherGateRadioHooks.cs`

### `Assets/Ashfall.Core/World/WeatherGateRadioHooks.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 206 lines / 8955 bytes.
- SHA-256: `dd76cb94bb6640066d55bc0c7af9303cdb8158af913dab8affc9db33b9ef1ee9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WeatherGateRadioTriggerIds
public const string HighlandClosure = "weather_gate.highland_closure";
public const string ThawOpening = "weather_gate.thaw_opening";
public const string LowlandFlood = "weather_gate.lowland_flood";
public const string WastelandFallout = "weather_gate.wasteland_fallout";
public const string BasinFogWarning = "weather_gate.basin_fog_warning";
public sealed class WeatherGateRadioHooks
public IReadOnlyDictionary<string, bool> WorldConditionOpen => _worldConditionOpen;
public IReadOnlyCollection<string> ConsumedTriggers => _consumedTriggers;
public int PendingCount => _pending.Count;
public void Subscribe(WeatherSystem weather) {
public void Unsubscribe(WeatherSystem weather) {
public void ResetForTest() {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Main.Narrative.cs`

### `src/Main.Narrative.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 728 lines / 29845 bytes.
- SHA-256: `b588f3d83df8088676183b79152ffa57c62a3b5d30ed8c7cb73ae0a4b7ece863`.
- Architecture signals: seeded references=0; save/restore symbols=6; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioSignalLog.cs`

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


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RescueDispatchPreflight.cs`

### `Assets/Ashfall.Core/Radio/RescueDispatchPreflight.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 41 lines / 1691 bytes.
- SHA-256: `21025a84433af7542398f42d2de0fee1c466121c80e74e8c21dac6b3db057265`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RescueDispatchRecommendation
public sealed class RescueDispatchPreflight
public string SignalId { get; set; } = string.Empty;
public string QuestId { get; set; } = string.Empty;
public bool Analyzed { get; set; }
public SignalAuthenticityCategory Assessment { get; set; }
public bool ThreatDetected { get; set; }
public bool SenderAlive { get; set; }
public bool Expired { get; set; }
public RescueDispatchRecommendation Recommendation { get; set; }
public string Note { get; set; } = string.Empty;
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/HostCli.PlansB86_B89.cs`

### `src/Host/HostCli.PlansB86_B89.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 632 lines / 33331 bytes.
- SHA-256: `32b5161d84d9cb8b979ded5af52617d4988a72b0450c733b5c59524fb0f09dfc`.
- Architecture signals: seeded references=19; save/restore symbols=9; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunPrecisionMetrologySelfTest(string dataDirectory) {
public static int RunDirectionFindingSelfTest(string dataDirectory) {
public static int RunAquaponicsSelfTest(string dataDirectory) {
public static int RunCombatBreachingSelfTest(string dataDirectory) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

### `src/Host/HostCli.PanelTests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 4479 lines / 250824 bytes.
- SHA-256: `23b1c0498d6a8cea2b2f69b49d342aed1d444425b6bee4d6c0b144f195e78443`.
- Architecture signals: seeded references=15; save/restore symbols=102; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=3; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=12.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunYearOfAshSaveSelfTest(string dataDirectory) {
public static int RunDutyRosterSaveSelfTest(string dataDirectory) {
public static int RunExpansionHubSaveSelfTest(string dataDirectory) {
public static int RunExpeditionSelfTest() {
public static int RunBridgeSelfTest() {
public static int RunPowerGridCatalogSelfTest() {
public static int RunExpeditionEncounterBridgeSelfTest() {
public static int RunMedicalSelfTest() {
public static int RunNarrativeSelfTest() {
public static int RunOralLoreSelfTest(string dataDirectory) {
public static int RunSurvivorsSelfTest() {
public static int RunWorldSelfTest() {
public static int RunEconomySelfTest(string dataDirectory) {
public static int RunUtilityAiSelfTest(string dataDirectory) {
public static int RunDoseLedgerSelfTest(string dataDirectory) {
public static int RunBlackFlotillaSelfTest(string dataDirectory) {
public static int RunRadioSelfTest() {
public static int RunHoldfastBriefing(string dataDirectory) {
public static int RunIceRoadTickDemo(string dataDirectory) {
public static int RunHoldfastSaveSelfTest(string dataDirectory) {
public static int RunStandaloneSystemsSelfTest() {
public static int RunPhase0SelfTest() {
public static int RunCaravanSelfTest() {
public static int RunAssetRegistrySelfTest(string dataDirectory) {
public static int RunAssetCoverageReport(string dataDirectory) {
public static int RunDay1PlayableSelfTest(string dataDirectory) {
public static int RunDay1ToDay2MilestoneSelfTest(string dataDirectory) {
public static int RunUiLayoutSelfTest(string dataDirectory) {
public static int RunSettingsSelfTest(string dataDirectory) {
public static int RunPlayableShellSelfTest(string dataDirectory) {
public static int RunShelterHazardLoopSelfTest(string dataDirectory) {
public static int RunShelterOperationsSelfTest(string dataDirectory) {
public static string SnapshotGoldenRoot() {
public static string SnapshotCaptureRoot() {
internal sealed class PanelTestFaultyFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => throw new System.IO.IOException("Simulated I/O disk error");
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
internal sealed class PanelTestCorruptJsonFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => "{ not valid json syntax !!!";
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Main.NarrativeQuestlines.cs`

### `src/Main.NarrativeQuestlines.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 300 lines / 12648 bytes.
- SHA-256: `48bd492b00b71c40972f967c96d743eab71596efa6c182f1f425f68a161c7f66`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public string LastNarrativeArcEvent { get; private set; } = string.Empty;
public NarrativeQuestlineHostSession? NarrativeQuestlines => _narrativeQuestlines;
public bool BeginSurvivorArc(string survivorId) {
public bool DeliverSurvivorArcObjective(string survivorId, string itemId) {
public bool ChooseSurvivorArcBranch(string survivorId, string branchId) {
public IReadOnlyList<NarrativeQuestlineDef> NarrativeArcDefinitions() {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Audio/AudioSelfTest.cs`

### `src/Audio/AudioSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1464 lines / 86135 bytes.
- SHA-256: `6f4f36f995ff641502b021ecc7c4d5fe92a6e5695cb6a6d80dc67cacb94b9537`.
- Architecture signals: seeded references=19; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class AudioSelfTest
public static int Run() {
internal sealed class TestShelterOperationsAudioProvider : IShelterOperationsAudioProvider
public ShelterWorkshopSystem? AudioWorkshop { get; set; }
public Ashfall.Core.Radio.ShelterRadioStationSystem? AudioRadioStation { get; set; }
public ShelterSocialDynamicsSystem? AudioSocialDynamics { get; set; }
public ExcavationHazardSystem? AudioExcavationHazards { get; set; }
internal sealed class TestExpansionAudioProvider : IExpansionAudioProvider
public Ashfall.Core.Survivors.DesperationSystem? AudioDesperation { get; set; }
public Ashfall.Core.Medical.MutationSystem? AudioMutation { get; set; }
public Ashfall.Core.Combat.ChemWarfareSystem? AudioChemWarfare { get; set; }
public Ashfall.Core.Expeditions.RailwaySystem? AudioRailway { get; set; }
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

### `src/Main.UiPanels.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1755 lines / 86737 bytes.
- SHA-256: `4c6b58cef5ed68a8ccfa1e2a51ca6da94a201322a745aa4c3b7993fc385782a1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Feedback.IFeedbackService FeedbackService => _feedbackService;
public FeedbackPanel FeedbackPanel => _feedbackPanel;
public ConfirmationModal ConfirmationModal => _confirmationModal;
public void PromptConfirmation(string title, string message, Action onConfirm, Action? onCancel = null) {
```


# Appendix Q.582 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`

### `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 330 lines / 14462 bytes.
- SHA-256: `dece5f6b24892465862a40dbbd5d85491f0d43da5c52f6a5529bda4f381a5546`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ReconTelemetrySystem
public const string SystemId = "recon_telemetry";
public ReconTelemetryState State => _state;
public IReadOnlyDictionary<string, ReconProbeDef> Platforms => _platforms;
public float GetRadioRangeMultiplier() {
public float GetRouteSpeedMultiplier(string routeId) {
public event Action<string>? OnReconLaunched;            // missionId
public event Action<string>? OnSurveyCompleted;         // missionId
public event Action<string, string>? OnPlatformLost;    // platformId, reason
public event Action<string>? OnPlatformRecovered;       // platformId
public event Action<string>? OnFalloutForecastGenerated;// forecastId
public event Action<string>? OnRouteScouted;            // routeId
public void RegisterPlatform(ReconProbeDef def) {
public void LoadCatalog(ReconTelemetryCatalog? catalog) {
public ActiveReconMissionState? GetMission(string missionId) {
public bool IsPlatformLaunched(string platformId) {
public LaunchResult LaunchMission(string platformId, string targetSectorId) {
public ActionResult RecoverPlatform(string missionId) {
public SurveyResult SurveySectors(string missionId, List<string> sectorIds) {
public ActionResult GenerateForecast(string platformId) {
public ActionResult ScoutRoute(string routeId, string missionId) {
public void TickDay(int day) {
public ReconTelemetryState CaptureState() => CloneState(_state);
public void RestoreState(ReconTelemetryState saved) {
public sealed class LaunchResult
public bool IsSuccess { get; }
public bool IsBlocked => !IsSuccess && FailureCode != null;
public string? FailureCode { get; }
public string MessageKey { get; }
public string MissionId { get; }
public static LaunchResult Failed(string code, string key) => new LaunchResult(false, code, key, string.Empty);
public static LaunchResult Blocked(string code, string key) => new LaunchResult(false, code, key, string.Empty);
public static LaunchResult Success(string missionId) => new LaunchResult(true, null, string.Empty, missionId);
public sealed class SurveyResult
public bool IsSuccess { get; }
public string FailureCode { get; }
public string MessageKey { get; }
public List<string> SurveyedSectors { get; }
public static SurveyResult Failed(string code, string key) => new SurveyResult(false, code, key, new List<string>());
public static SurveyResult Success(List<string> sectors) => new SurveyResult(true, null, string.Empty, sectors);
```


# Appendix Q.583 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs`

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


# Appendix Q.584 — Additional Current Architecture Evidence: `src/Main.UiTests.PlayerPanels.cs`

### `src/Main.UiTests.PlayerPanels.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 236 lines / 10980 bytes.
- SHA-256: `d452fcebe3e50f0eeb0804046534750abec83d9794cd5bcece126922745e9d62`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.585 — Additional Current Architecture Evidence: `src/Main.UiHandlers.cs`

### `src/Main.UiHandlers.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 321 lines / 12444 bytes.
- SHA-256: `4cb12e7e01ef0776faf5e38403afd5a5b7274e01d72d0534c89bfac2bf69a252`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void OpenSettingsPanel() => _settingsPanel?.Open();
public void OpenCraftingPanel() {
public void OpenRadioPanel() {
public void OpenMedicalPanel() {
public void OpenPhase0Panel() {
public void OpenDutyRosterPanel() {
public void OpenExpeditionPanel() => _expeditionPanel?.Open();
public void OpenWeatherPanel() {
public void OpenWeatherForecastPanel() {
public void OpenWeatherHistoryPanel() {
public void OpenQuestsPanel() {
public void OpenJournalPanel() {
public void OpenFactionsPanel() {
public void OpenShelterPanel() {
public void OpenCombatPanel() {
public void OpenMapPanel() {
public void OpenMapDetailPanel(string locationId) {
public void OpenFactionDetailPanel(string factionId) {
public void OpenQuestDetailPanel(string questId) {
public void OpenMoralChoiceModal(string? questId = null) {
public void OpenFireIncidentPanel(string? incidentId = null) {
public void OpenSaveLoadPanel() => _saveLoadPanel?.Open();
public void OpenCrossingQuestPanel() {
public void OnExitGameClicked() {
```


# Appendix Q.586 — Additional Current Architecture Evidence: `src/Audio/ShelterOperationsAudioBridge.cs`

### `src/Audio/ShelterOperationsAudioBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 368 lines / 13504 bytes.
- SHA-256: `5f03f03b2eff927efb8b4886f4dec3d0edd0c28853e27c8a8fc41d9100941569`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IShelterOperationsAudioProvider
public sealed class ShelterOperationsAudioBridge : IDisposable
public void BindAll( ShelterWorkshopSystem? workshop = null, ShelterRadioStationSystem? radio = null, ShelterSocialDynamicsSystem? social = null, ExcavationHazardSystem? excavation = null) {
public void SubscribeAll(IShelterOperationsAudioProvider? provider) {
public void BindWorkshop(ShelterWorkshopSystem? workshop) {
public void BindRadio(ShelterRadioStationSystem? radio) {
public void BindSocial(ShelterSocialDynamicsSystem? social) {
public void BindExcavation(ExcavationHazardSystem? excavation) {
public void NotifyRadioFrequencyChanged(float frequencyKhz, float minKhz = 3000f, float maxKhz = 30000f) {
public void UpdateMessHallOccupancy(int activeOccupants) {
public void NotifyMethaneWarning(string sectorId, int ppm) {
public void NotifyBulkheadToggled(string sectorId, bool sealedBulkhead) {
public void NotifyPumpStateChanged(string sectorId, bool running) {
public void Dispose() {
```


# Appendix Q.587 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

### `src/Main.PlayerSurfaces.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1141 lines / 66230 bytes.
- SHA-256: `359ab8fa9162114b544f2a9629d5ea490a6cf866323880035b63ab47ab8c9056`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.588 — Additional Current Architecture Evidence: `src/UI/TriangulationPanel.cs`

### `src/UI/TriangulationPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 233 lines / 8962 bytes.
- SHA-256: `09b3141f69c0a707aef71f4f15d53a56331512f0863f3d4c4806d9562dafd232`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class TriangulationPanel : Control
public event Action? OnClose;
public event Action<string>? OnLocationDiscovered;
public bool IsBound => _radioHost != null;
public int RefreshCount { get; private set; }
public void Bind(RadioHostSession radioHost, string signalId = "") {
public void Open() {
public override void _Ready() {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.589 — Additional Current Architecture Evidence: `src/Main.GameFlow.cs`

### `src/Main.GameFlow.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 950 lines / 43281 bytes.
- SHA-256: `2fdc11c80e9e23418cf05a6d8dbd621c66c8d6aba34503e7cde474922c403685`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=2; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.590 — Additional Current Architecture Evidence: `src/Main.Plans46_49.cs`

### `src/Main.Plans46_49.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 474 lines / 19620 bytes.
- SHA-256: `312d6602edf690061111377cd4798a738c47e6c5c344ed258c88e91167262bb8`.
- Architecture signals: seeded references=4; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ShelterWorkshopSystem EnsureShelterWorkshop() {
public ShelterRadioStationSystem EnsureRadioStation() {
public ShelterSocialDynamicsSystem EnsureShelterSocialDynamics() {
public ExcavationHazardSystem EnsureExcavationHazards() {
public DynamicQuestlineSystem EnsureDynamicQuests() {
```


# Appendix Q.591 — Additional Current Architecture Evidence: `src/UI/RadioIntelligencePanel.cs`

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


# Appendix Q.592 — Additional Current Architecture Evidence: `src/Main.Economy.cs`

### `src/Main.Economy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 366 lines / 15389 bytes.
- SHA-256: `2043c87b9cfaed583952235f93761567e656a645d675090b74cf213fe2e6c1ef`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=0; textual Godot mentions=11; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 50

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass rejected the fictional standalone assessment/rules layer.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass separated signal, authenticity, mission, trust and follow-up owners.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass requires exactly-once trust, mission, reward and follow-up consequences.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
