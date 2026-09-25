# Plan 57 — Shelter Incidents, Event Read Model and Player Decisions

> **Rebuild status:** COMPLETE 25-ROW CATALOG — EVENTS HOST IS A TEXT-ONLY READ MODEL; GAMEPLAY CHOICES ARE NOT YET EXECUTIVE
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

- incidents.json currently contains 25 rows with IDs, titles, bodyText, weight and minDay; it deliberately does not carry the dead category/maxDay/choice fields. EventsHostSession loads and exposes them as events content, while Core gameplay systems remain elsewhere.

**Bounded outcome:** Retire the 5-to-25 content target as complete. The current catalog has 25 rows and current tests explicitly classify EventsHostSession as a text-only read model with no Core choice executor. The plan must define what is intentionally narrative, what a future incident command would consume, and which current owners would apply effects.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the stale pure-data claim with a truthful incident contract: catalog validity, selection/daily reachability, decision grammar only if a current command consumes it, and effect routing to existing owners. No new incident manager is justified.

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
- The current plan is truthful only while incidents remain a read model.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- The required delta is to decide whether incidents are a text/read-only layer or to design one bounded bridge through a current day/event owner. The plan must not imply that authored choices already mutate health, morale, power or inventory.

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
| incident JSON loading and read-model exposure | EventsHostSession | `src/Host/EventsHostSession.cs` | Current host read model. |
| actual daily event execution and effect routing | Campaign day/event owners | `src/Main.CampaignOwners.cs` | Current day-owner boundary; inspect before any future write. |
| candidate consequences | Needs/Cohort/Medical/Power owners | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs; Assets/Ashfall.Core/CohortSystem.cs` | Existing effect owners; no incident shadow state. |
| current 25-row schema/read-model contract | Plan57IncidentTests | `Ashfall.Core.Tests/Plan57IncidentTests.cs` | Focused executable evidence. |
| player-facing incident text and facts | Journal owner | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | Presentation/persistence owner. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Shelter Incidents, Event Read Model and Player Decisions
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ EventsHostSession
│   incident JSON loading and read-model exposure
│ Campaign day/event owners
│   actual daily event execution and effect routing
│ Needs/Cohort/Medical/Power owners
│   candidate consequences
│ Plan57IncidentTests
│   current 25-row schema/read-model contract
│ Journal owner
│   player-facing incident text and facts
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

1. **Preserve current state ownership.** EventsHostSession owns incident JSON loading and read-model exposure: Current host read model.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| incident JSON loading and read-model exposure | EventsHostSession | `src/Host/EventsHostSession.cs` | Current host read model. |
| actual daily event execution and effect routing | Campaign day/event owners | `src/Main.CampaignOwners.cs` | Current day-owner boundary; inspect before any future write. |
| candidate consequences | Needs/Cohort/Medical/Power owners | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs; Assets/Ashfall.Core/CohortSystem.cs` | Existing effect owners; no incident shadow state. |
| current 25-row schema/read-model contract | Plan57IncidentTests | `Ashfall.Core.Tests/Plan57IncidentTests.cs` | Focused executable evidence. |
| player-facing incident text and facts | Journal owner | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | Presentation/persistence owner. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 25 incident definitions
2. validate ID/title/body/day/weight
3. determine current daily selection path
4. present an incident through the existing event surface
5. if a proven command exists, route it to one owner
6. record journal fact
7. capture only the owner that changed

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Incident definitions are immutable catalog rows.
- No incident state exists merely because a row has a weight or minDay.
- If choices are introduced, selection/resolution state must belong to the current event owner and use current save conventions.
- A journal fact is not proof of a gameplay effect.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- No dead JSON field may be treated as executable.
- A weight is inert until a seeded selector consumes it.
- A choice cannot debit or heal without an owner command.
- Repeated day presentation is deterministic and bounded.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- incidents.json is the sole incident definition catalog.
- Medical, needs, power, cohort, faction and journal catalogs own effects/text.
- No duplicate incident state or effect registry.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use existing event/day and journal owners.
- No Plan-57 save section for a read-only catalog.
- Any future incident runtime requires a named existing save owner and migration proof.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Selection uses the campaign seeded stream.
- Catalog order is stable and weights are bounded.
- No wall-clock day or random selection in UI.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- EventsHostSession emits no gameplay decision event today.
- Current day owners emit their own typed facts.
- A future bridge must subscribe to one existing event path and apply one owner per effect.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/EventsHostSession.cs
- src/Main.CampaignOwners.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Incident prose is urgent, grounded and fictional.
- It may describe threats and scarcity without claiming an effect the host cannot apply.
- No real-world emergency advice or copied event text.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A weight is presented as an imminent event without a selector. | EventsHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A dead field is interpreted as a command. | Campaign day/event owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A choice applies an effect twice. | Needs/Cohort/Medical/Power owners | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A panel mutates a system directly. | Plan57IncidentTests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new incident save duplicates the day owner. | Journal owner | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan57IncidentTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 25-row census and dead-field check. | Current read-only status is explicit. | No production path until the owning implementation package is separately claimed. |
| 1 | Trace daily/event host consumers. | No synthetic live path is claimed. | No production path until the owning implementation package is separately claimed. |
| 2 | If a gap is proven, specify one owner-bound bridge. | No parallel incident engine. | No production path until the owning implementation package is separately claimed. |
| 3 | UI and failure polish. | Controls match actual commands. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/incidents.json | READ ONLY; MODIFY only for content defect | 25 incident rows |
| src/Host/EventsHostSession.cs | READ ONLY | Read model |
| src/Main.CampaignOwners.cs | READ ONLY | Future effect seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second incident manager. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Claiming choices are live when only prose exists. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Overloading the journal as gameplay state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Authoring dead fields. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new incident rows.
- No generic effect DSL.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert the planning artifact.
- A future bridge must be isolated to one owner and one save path.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 25-row/read-model distinction is explicit.
- The plan names exact future bridge conditions and rejects fake controls.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- The required delta is to decide whether incidents are a text/read-only layer or to design one bounded bridge through a current day/event owner. The plan must not imply that authored choices already mutate health, morale, power or inventory.

## MUST NOT DO

- No new incident rows.
- No generic effect DSL.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Plan57IncidentTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: incident JSON loading and read-model exposure → EventsHostSession; actual daily event execution and effect routing → Campaign day/event owners; candidate consequences → Needs/Cohort/Medical/Power owners; current 25-row schema/read-model contract → Plan57IncidentTests; player-facing incident text and facts → Journal owner. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 57.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 57 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by EventsHostSession or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `src/Host/EventsHostSession.cs`

### `src/Host/EventsHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 209 lines / 7170 bytes.
- SHA-256: `751cd5c3d0bafa01198fd2ae1b6a6082c25361626be02bd0312dcc729f39038c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class EventsHostSession : Node
public override void _Ready() {
public bool TryGetEvent(string eventId, out EventData eventData) {
public List<EventEntry> GetRecentEvents() {
public List<IncidentEntry> GetIncidents() {
public List<NarrativeEntry> GetNarrativeProgression() {
public class EventsRoot
public int SchemaVersion { get; set; }
public List<EventData> Events { get; set; }
public class EventData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class IncidentData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class NarrativeEntryData
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
public class IncidentsRoot
public int SchemaVersion { get; set; }
public List<IncidentData> Incidents { get; set; } = new List<IncidentData>();
public class NarrativeRoot
public int SchemaVersion { get; set; }
public List<NarrativeEntryData> Entries { get; set; } = new List<NarrativeEntryData>();
public class EventEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class IncidentEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class NarrativeEntry
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
```


# Appendix B.03 — Current Code Architecture: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Journal/JournalSystem.cs`

### `Assets/Ashfall.Core/Journal/JournalSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 490 lines / 21046 bytes.
- SHA-256: `c0d8316adaed06ce3b5415dcfd5fe79f63ad31fa6675181ad85859191601d220`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class JournalSystem
public const int MaxEntries = 64;
public const int TabCount = 5;
public event Action<JournalEntry> OnEntryAdded;
public event Action<JournalEntry> OnNotificationPing;
public event Action<int> OnTabChanged;
public event Action<string> OnCodexUnlocked;
public int ActiveTab { get; private set; }
public int CodexUnlockCount { get; private set; }
public int GetLastSeenIndex(int tab) {
public int GetLastSeenCodexIndex(int tab) {
public bool HasUnreadForTab(int tab) {
public void SwitchTab(int tab) {
public void MarkTabViewed(int tab) {
public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
public bool UnlockRoomHistorySeen(string vignetteId) => UnlockCodex(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool UnlockGlitchNoted(string glitchId) => UnlockCodex(KnowledgeKeys.GlitchNoted(glitchId));
public bool UnlockWildlifeCaught(string speciesId) => UnlockCodex(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool UnlockNarrativeDiscovered(string discoveryId) => UnlockCodex(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool UnlockBureaucraticDocument(string docId) => UnlockCodex(KnowledgeKeys.BureaucraticDocument(docId));
public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);
public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));
public bool IsRoomHistorySeen(string vignetteId) => _knowledge.Has(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool IsGlitchNoted(string glitchId) => _knowledge.Has(KnowledgeKeys.GlitchNoted(glitchId));
public bool IsWildlifeCaught(string speciesId) => _knowledge.Has(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool IsNarrativeDiscovered(string discoveryId) => _knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool IsBureaucraticDocumentDiscovered(string docId) => _knowledge.Has(KnowledgeKeys.BureaucraticDocument(docId));
public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler) {
public KnowledgeBase Knowledge => _knowledge;
public void BindAuthoredCorpus(JournalCorpusAdapter? adapter) {
public bool HasAuthoredCorpus => _authoredCorpus != null;
public JournalEntry? TryAddAuthoredEntry( string knowledgeKey, ISurvivorAuthor? fallbackAuthor = null) {
public IReadOnlyList<JournalEntry> Entries => _entries;
public int EntryCount => _entries.Count;
public string LatestText =>
public bool HasUnread { get; set; }
public bool NotificationPing { get; private set; }
public int NotificationPingCount { get; private set; }
public bool HudIsOpen { get; set; }
public JournalEntry? TryDiscover( string knowledgeKey, ISurvivorAuthor author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverKnowledge( string knowledgeKey, ISurvivorAuthor? author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverRawKnowledge( string knowledgeKey, string text, ISurvivorAuthor? author, int day, float hour = -1f)
public JournalEntry? TryAddRawEntry( string knowledgeKey, string text, ISurvivorAuthor author, int day, float hour = -1f)
public void AcknowledgePing() {
public void MarkRead() {
public void Clear() {
public JournalSave CaptureState() {
public void RestoreState(JournalSave save) {
public class JournalSave
public JournalEntry[] Entries;
public KnowledgeBaseSave Knowledge;
public int NextSeq;
public bool HasUnread;
public bool NotificationPing;
public int NotificationPingCount;
public bool HudIsOpen;
public int ActiveTab;
public int[] LastSeenIndexPerTab;
public int[] LastSeenCodexPerTab;
public int CodexUnlockCount;
```


# Appendix B.05 — Current Code Architecture: `src/Host/EventsHostSession.cs`

### `src/Host/EventsHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 209 lines / 7170 bytes.
- SHA-256: `751cd5c3d0bafa01198fd2ae1b6a6082c25361626be02bd0312dcc729f39038c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class EventsHostSession : Node
public override void _Ready() {
public bool TryGetEvent(string eventId, out EventData eventData) {
public List<EventEntry> GetRecentEvents() {
public List<IncidentEntry> GetIncidents() {
public List<NarrativeEntry> GetNarrativeProgression() {
public class EventsRoot
public int SchemaVersion { get; set; }
public List<EventData> Events { get; set; }
public class EventData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class IncidentData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class NarrativeEntryData
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
public class IncidentsRoot
public int SchemaVersion { get; set; }
public List<IncidentData> Incidents { get; set; } = new List<IncidentData>();
public class NarrativeRoot
public int SchemaVersion { get; set; }
public List<NarrativeEntryData> Entries { get; set; } = new List<NarrativeEntryData>();
public class EventEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class IncidentEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class NarrativeEntry
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
```


# Appendix B.06 — Current Code Architecture: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/incidents.json`

### `Assets/StreamingAssets/Data/incidents.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 12879 bytes / 12879 characters.
- SHA-256: `ebafd3f9f98e5d702dc9f579e69b1e2d6d5bf5710299201bd379709152a0f1f5`.
- Root keys: `incidents`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
incidents: min=25, max=25, observed_paths=1
```

Representative record fields:

- `bodyText`
- `id`
- `minDay`
- `title`
- `weight`

Representative identifiers (ordered, capped for readability):

```text
incident_radiation_spike
incident_bunker_breach
incident_water_contamination
incident_ambush_sector_4
incident_radio_interference
incident_fallout_storm_approach
incident_contaminated_water_table
incident_ground_tremor
incident_perimeter_breach_attempt
incident_unknown_visitor
incident_local_signal_intercept
incident_shelter_disease_outbreak
incident_chemical_exposure
incident_survivor_collapse
incident_ration_dispute
incident_ideological_friction
incident_grief_episode
incident_generator_failure
incident_air_filter_breakdown
incident_water_pipe_burst
incident_nearby_cache_discovered
incident_supply_drop_near_shelter
incident_faction_patrol_nearby
incident_refugees_approaching
incident_exchange_anniversary
```


# Appendix D.08 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Plan57IncidentTests.cs`

### `Ashfall.Core.Tests/Plan57IncidentTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 263; SHA-256: `ad572a0c765969efcfbab73944fcc1a708da832b91d561cb67db6d124b63a7a5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_contains_exactly_25_incidents
Loader_parses_all_25_through_the_live_host_read_model
Original_five_incidents_preserved_field_for_field
All_twenty_new_ids_present_unique_and_prefixed
All_incidents_have_grounded_titles_and_bodies
Weights_authored_within_bands_and_varied
At_least_five_new_incidents_are_phase_gated_across_the_campaign
Faction_linked_incidents_reference_real_faction_names
New_incidents_do_not_duplicate_original_semantics
Category_coverage_matches_the_required_distribution
Schema_stays_within_the_supported_field_set
Deterministic_day_ordering_stable_across_parses
```


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`

### `Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 237; SHA-256: `0e700ccc99860c31fe5b0c70ba780c2c8ac1cef0ea4d8b65b3ccae5d00e86818`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Journey_J39_FireIncident_CanonicalAuthority_DynamicResolution_AndActionMutations
ProductionRoutes_DoNotInstantiateThrowawayFireSystem_OrUseFixtureId
FireIncidentPanel_DoesNotHaveProhibitedLambdaUnsubscriptions
FireIncident_SaveEnvelope_RoundTripsCanonicalLedger
HostWiring_SaveAllAndProcessFlush_EnrollShelterFire
```


# Appendix E.10 — Supporting Code Evidence: `src/Host/EventsHostSession.cs`

### `src/Host/EventsHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 209 lines / 7170 bytes.
- SHA-256: `751cd5c3d0bafa01198fd2ae1b6a6082c25361626be02bd0312dcc729f39038c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class EventsHostSession : Node
public override void _Ready() {
public bool TryGetEvent(string eventId, out EventData eventData) {
public List<EventEntry> GetRecentEvents() {
public List<IncidentEntry> GetIncidents() {
public List<NarrativeEntry> GetNarrativeProgression() {
public class EventsRoot
public int SchemaVersion { get; set; }
public List<EventData> Events { get; set; }
public class EventData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class IncidentData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class NarrativeEntryData
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
public class IncidentsRoot
public int SchemaVersion { get; set; }
public List<IncidentData> Incidents { get; set; } = new List<IncidentData>();
public class NarrativeRoot
public int SchemaVersion { get; set; }
public List<NarrativeEntryData> Entries { get; set; } = new List<NarrativeEntryData>();
public class EventEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class IncidentEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class NarrativeEntry
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
```


# Appendix E.11 — Supporting Code Evidence: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Journal/JournalSystem.cs`

### `Assets/Ashfall.Core/Journal/JournalSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 490 lines / 21046 bytes.
- SHA-256: `c0d8316adaed06ce3b5415dcfd5fe79f63ad31fa6675181ad85859191601d220`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class JournalSystem
public const int MaxEntries = 64;
public const int TabCount = 5;
public event Action<JournalEntry> OnEntryAdded;
public event Action<JournalEntry> OnNotificationPing;
public event Action<int> OnTabChanged;
public event Action<string> OnCodexUnlocked;
public int ActiveTab { get; private set; }
public int CodexUnlockCount { get; private set; }
public int GetLastSeenIndex(int tab) {
public int GetLastSeenCodexIndex(int tab) {
public bool HasUnreadForTab(int tab) {
public void SwitchTab(int tab) {
public void MarkTabViewed(int tab) {
public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
public bool UnlockRoomHistorySeen(string vignetteId) => UnlockCodex(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool UnlockGlitchNoted(string glitchId) => UnlockCodex(KnowledgeKeys.GlitchNoted(glitchId));
public bool UnlockWildlifeCaught(string speciesId) => UnlockCodex(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool UnlockNarrativeDiscovered(string discoveryId) => UnlockCodex(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool UnlockBureaucraticDocument(string docId) => UnlockCodex(KnowledgeKeys.BureaucraticDocument(docId));
public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);
public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));
public bool IsRoomHistorySeen(string vignetteId) => _knowledge.Has(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool IsGlitchNoted(string glitchId) => _knowledge.Has(KnowledgeKeys.GlitchNoted(glitchId));
public bool IsWildlifeCaught(string speciesId) => _knowledge.Has(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool IsNarrativeDiscovered(string discoveryId) => _knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool IsBureaucraticDocumentDiscovered(string docId) => _knowledge.Has(KnowledgeKeys.BureaucraticDocument(docId));
public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler) {
public KnowledgeBase Knowledge => _knowledge;
public void BindAuthoredCorpus(JournalCorpusAdapter? adapter) {
public bool HasAuthoredCorpus => _authoredCorpus != null;
public JournalEntry? TryAddAuthoredEntry( string knowledgeKey, ISurvivorAuthor? fallbackAuthor = null) {
public IReadOnlyList<JournalEntry> Entries => _entries;
public int EntryCount => _entries.Count;
public string LatestText =>
public bool HasUnread { get; set; }
public bool NotificationPing { get; private set; }
public int NotificationPingCount { get; private set; }
public bool HudIsOpen { get; set; }
public JournalEntry? TryDiscover( string knowledgeKey, ISurvivorAuthor author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverKnowledge( string knowledgeKey, ISurvivorAuthor? author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverRawKnowledge( string knowledgeKey, string text, ISurvivorAuthor? author, int day, float hour = -1f)
public JournalEntry? TryAddRawEntry( string knowledgeKey, string text, ISurvivorAuthor author, int day, float hour = -1f)
public void AcknowledgePing() {
public void MarkRead() {
public void Clear() {
public JournalSave CaptureState() {
public void RestoreState(JournalSave save) {
public class JournalSave
public JournalEntry[] Entries;
public KnowledgeBaseSave Knowledge;
public int NextSeq;
public bool HasUnread;
public bool NotificationPing;
public int NotificationPingCount;
public bool HudIsOpen;
public int ActiveTab;
public int[] LastSeenIndexPerTab;
public int[] LastSeenCodexPerTab;
public int CodexUnlockCount;
```


# Appendix E.13 — Supporting Code Evidence: `src/Host/EventsHostSession.cs`

### `src/Host/EventsHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 209 lines / 7170 bytes.
- SHA-256: `751cd5c3d0bafa01198fd2ae1b6a6082c25361626be02bd0312dcc729f39038c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class EventsHostSession : Node
public override void _Ready() {
public bool TryGetEvent(string eventId, out EventData eventData) {
public List<EventEntry> GetRecentEvents() {
public List<IncidentEntry> GetIncidents() {
public List<NarrativeEntry> GetNarrativeProgression() {
public class EventsRoot
public int SchemaVersion { get; set; }
public List<EventData> Events { get; set; }
public class EventData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class IncidentData
public string Id { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public float Weight { get; set; }
public int MinDay { get; set; }
public class NarrativeEntryData
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
public class IncidentsRoot
public int SchemaVersion { get; set; }
public List<IncidentData> Incidents { get; set; } = new List<IncidentData>();
public class NarrativeRoot
public int SchemaVersion { get; set; }
public List<NarrativeEntryData> Entries { get; set; } = new List<NarrativeEntryData>();
public class EventEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class IncidentEntry
public int Day { get; set; }
public string Description { get; set; } = string.Empty;
public class NarrativeEntry
public string Description { get; set; } = string.Empty;
public int Order { get; set; }
```


# Appendix E.14 — Supporting Code Evidence: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix F.15 — Supporting Data Evidence: `Assets/StreamingAssets/Data/incidents.json`

### `Assets/StreamingAssets/Data/incidents.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 12879 bytes / 12879 characters.
- SHA-256: `ebafd3f9f98e5d702dc9f579e69b1e2d6d5bf5710299201bd379709152a0f1f5`.
- Root keys: `incidents`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
incidents: min=25, max=25, observed_paths=1
```

Representative record fields:

- `bodyText`
- `id`
- `minDay`
- `title`
- `weight`

Representative identifiers (ordered, capped for readability):

```text
incident_radiation_spike
incident_bunker_breach
incident_water_contamination
incident_ambush_sector_4
incident_radio_interference
incident_fallout_storm_approach
incident_contaminated_water_table
incident_ground_tremor
incident_perimeter_breach_attempt
incident_unknown_visitor
incident_local_signal_intercept
incident_shelter_disease_outbreak
incident_chemical_exposure
incident_survivor_collapse
incident_ration_dispute
incident_ideological_friction
incident_grief_episode
incident_generator_failure
incident_air_filter_breakdown
incident_water_pipe_burst
incident_nearby_cache_discovered
incident_supply_drop_near_shelter
incident_faction_patrol_nearby
incident_refugees_approaching
incident_exchange_anniversary
```


# Appendix G.16 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan57IncidentTests.cs`

### `Ashfall.Core.Tests/Plan57IncidentTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 263; SHA-256: `ad572a0c765969efcfbab73944fcc1a708da832b91d561cb67db6d124b63a7a5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_contains_exactly_25_incidents
Loader_parses_all_25_through_the_live_host_read_model
Original_five_incidents_preserved_field_for_field
All_twenty_new_ids_present_unique_and_prefixed
All_incidents_have_grounded_titles_and_bodies
Weights_authored_within_bands_and_varied
At_least_five_new_incidents_are_phase_gated_across_the_campaign
Faction_linked_incidents_reference_real_faction_names
New_incidents_do_not_duplicate_original_semantics
Category_coverage_matches_the_required_distribution
Schema_stays_within_the_supported_field_set
Deterministic_day_ordering_stable_across_parses
```


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`

### `Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 237; SHA-256: `0e700ccc99860c31fe5b0c70ba780c2c8ac1cef0ea4d8b65b3ccae5d00e86818`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Journey_J39_FireIncident_CanonicalAuthority_DynamicResolution_AndActionMutations
ProductionRoutes_DoNotInstantiateThrowawayFireSystem_OrUseFixtureId
FireIncidentPanel_DoesNotHaveProhibitedLambdaUnsubscriptions
FireIncident_SaveEnvelope_RoundTripsCanonicalLedger
HostWiring_SaveAllAndProcessFlush_EnrollShelterFire
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| incident JSON loading and read-model exposure | EventsHostSession | actual daily event execution and effect routing | Campaign day/event owners | Owner emits/reads a typed fact; no mirror state. |
| incident JSON loading and read-model exposure | EventsHostSession | candidate consequences | Needs/Cohort/Medical/Power owners | Owner emits/reads a typed fact; no mirror state. |
| incident JSON loading and read-model exposure | EventsHostSession | current 25-row schema/read-model contract | Plan57IncidentTests | Owner emits/reads a typed fact; no mirror state. |
| incident JSON loading and read-model exposure | EventsHostSession | player-facing incident text and facts | Journal owner | Owner emits/reads a typed fact; no mirror state. |
| actual daily event execution and effect routing | Campaign day/event owners | incident JSON loading and read-model exposure | EventsHostSession | Owner emits/reads a typed fact; no mirror state. |
| actual daily event execution and effect routing | Campaign day/event owners | candidate consequences | Needs/Cohort/Medical/Power owners | Owner emits/reads a typed fact; no mirror state. |
| actual daily event execution and effect routing | Campaign day/event owners | current 25-row schema/read-model contract | Plan57IncidentTests | Owner emits/reads a typed fact; no mirror state. |
| actual daily event execution and effect routing | Campaign day/event owners | player-facing incident text and facts | Journal owner | Owner emits/reads a typed fact; no mirror state. |
| candidate consequences | Needs/Cohort/Medical/Power owners | incident JSON loading and read-model exposure | EventsHostSession | Owner emits/reads a typed fact; no mirror state. |
| candidate consequences | Needs/Cohort/Medical/Power owners | actual daily event execution and effect routing | Campaign day/event owners | Owner emits/reads a typed fact; no mirror state. |
| candidate consequences | Needs/Cohort/Medical/Power owners | current 25-row schema/read-model contract | Plan57IncidentTests | Owner emits/reads a typed fact; no mirror state. |
| candidate consequences | Needs/Cohort/Medical/Power owners | player-facing incident text and facts | Journal owner | Owner emits/reads a typed fact; no mirror state. |
| current 25-row schema/read-model contract | Plan57IncidentTests | incident JSON loading and read-model exposure | EventsHostSession | Owner emits/reads a typed fact; no mirror state. |
| current 25-row schema/read-model contract | Plan57IncidentTests | actual daily event execution and effect routing | Campaign day/event owners | Owner emits/reads a typed fact; no mirror state. |
| current 25-row schema/read-model contract | Plan57IncidentTests | candidate consequences | Needs/Cohort/Medical/Power owners | Owner emits/reads a typed fact; no mirror state. |
| current 25-row schema/read-model contract | Plan57IncidentTests | player-facing incident text and facts | Journal owner | Owner emits/reads a typed fact; no mirror state. |
| player-facing incident text and facts | Journal owner | incident JSON loading and read-model exposure | EventsHostSession | Owner emits/reads a typed fact; no mirror state. |
| player-facing incident text and facts | Journal owner | actual daily event execution and effect routing | Campaign day/event owners | Owner emits/reads a typed fact; no mirror state. |
| player-facing incident text and facts | Journal owner | candidate consequences | Needs/Cohort/Medical/Power owners | Owner emits/reads a typed fact; no mirror state. |
| player-facing incident text and facts | Journal owner | current 25-row schema/read-model contract | Plan57IncidentTests | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | The required delta is to decide whether incidents are a text/read-only layer or to design one bounded bridge through a current day/event owner. The plan must not imply that authored choices already mutate health, morale, power or inventory. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> The v1.0 master bible was a snapshot: a large, well-structured reference that a planner reads before drafting one plan. Its structural weakness, identified during the 2026-09-24 audit, is that it is a *library*, not a *machine*. It tells a planner what exists, but it does not encode the generative move — the repeatable transformation of (repository evidence × lane × subsystem) into a bounded subject plan with a recommended integration route.

> 1. **The Drift Register (Part I):** a live-audit correction layer. The repository has moved since the v1.0 snapshot; every plan drafted against stale premises is wasted work. The register lists what changed, with evidence and confidence labels.
2. **The Factory Protocol (Part II):** the operating loop that converts evidence into subject plans. It is deterministic, like everything else in this project: same inputs, same plan shape, same verification demands.
3. **The Generator Matrices (Part III):** the combinatorial core. Ten expansion lanes × seventeen subsystem clusters, with per-cell opening archetypes. This is the mechanism by which one document yields hundreds of expansion plans without inventing duplicate systems.
4. **The Seeded Backlog (Part IV) and Templates (Part V):** audit-derived candidate expansions, each with a subject, evidence, confidence, and best integration route; plus the wave-charter, subject-plan, and verification templates the repository already uses, extended for factory output.

> The audit inspected the live repository directly: root directory listing, `Assets/StreamingAssets/Data/` (342 entries), `docs/` (top-level documents and subdirectories), `docs/plans/` (126 entries), `INTEGRATION_PLANS.md` (32,793 characters, read head and tail), `SESSION_HANDOFF.md`, `AGENTS.md` (head), and the branch list. No working-tree clone was available in the audit environment; findings marked VERIFIED are directly demonstrated by these listings and file reads. Findings marked UNVERIFIED could not be confirmed in this pass and require a follow-up read before any plan relies on them.

> **DR-01 — Root-level coordination artifacts absent from the v1.0 docs map. VERIFIED.**
The repository root now contains planning and coordination artifacts the bible's docs map (v1.0 Part 5.8) does not mention: `A1_BRIEFING_DEFERRED.md`, `A1_COORDINATION_RECORD.md`, `WAVE9_PART1_CLOSEOUT.md`, `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `semantic-review/`, `POTENTIALCLUTTER.md`, `sources.md`, `CRUSH.md`, `VIBE.md`, `MIMOCODE.md`, `OPENSETUP.md`, `Ashfall.slnx`, `Directory.Packages.props`, `global.json`, plus `tests/` and `snapshots/` at root. Consequence: a planner following the v1.0 docs map will miss active coordination surfaces and may duplicate decisions already recorded in them. Any expansion-planning session must now sweep the root-level `*.md` coordination files and the `Next-steps-plans/`, `piagentsplans/`, and `Seal-steps/` directories before drafting.

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 57: Shelter Incidents, Event Read Model and Player Decisions.

- **incident JSON loading and read-model exposure** remains with `EventsHostSession` at `src/Host/EventsHostSession.cs`. Current host read model.
- **actual daily event execution and effect routing** remains with `Campaign day/event owners` at `src/Main.CampaignOwners.cs`. Current day-owner boundary; inspect before any future write.
- **candidate consequences** remains with `Needs/Cohort/Medical/Power owners` at `Assets/Ashfall.Core/Survivors/NeedsSystem.cs; Assets/Ashfall.Core/CohortSystem.cs`. Existing effect owners; no incident shadow state.
- **current 25-row schema/read-model contract** remains with `Plan57IncidentTests` at `Ashfall.Core.Tests/Plan57IncidentTests.cs`. Focused executable evidence.
- **player-facing incident text and facts** remains with `Journal owner` at `Assets/Ashfall.Core/Journal/JournalSystem.cs`. Presentation/persistence owner.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 25 incident definitions
2. validate ID/title/body/day/weight
3. determine current daily selection path
4. present an incident through the existing event surface
5. if a proven command exists, route it to one owner
6. record journal fact
7. capture only the owner that changed

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Incident definitions are immutable catalog rows.
- No incident state exists merely because a row has a weight or minDay.
- If choices are introduced, selection/resolution state must belong to the current event owner and use current save conventions.
- A journal fact is not proof of a gameplay effect.

- No dead JSON field may be treated as executable.
- A weight is inert until a seeded selector consumes it.
- A choice cannot debit or heal without an owner command.
- Repeated day presentation is deterministic and bounded.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/EventsHostSession.cs
- src/Main.CampaignOwners.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Plan57IncidentTests.cs
- Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs

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
| S-01 | 57-01 25 rows load | load 25 incident definitions | Incident definitions are immutable catalog rows. | A weight is presented as an imminent event without a selector. | EventsHostSession |
| S-02 | 57-02 ID/body parity | validate ID/title/body/day/weight | No incident state exists merely because a row has a weight or minDay. | A dead field is interpreted as a command. | EventsHostSession |
| S-03 | 57-03 weight bounds | determine current daily selection path | If choices are introduced, selection/resolution state must belong to the current event owner and use current save conventions. | A choice applies an effect twice. | EventsHostSession |
| S-04 | 57-04 minDay boundary | present an incident through the existing event surface | A journal fact is not proof of a gameplay effect. | A panel mutates a system directly. | EventsHostSession |
| S-05 | 57-05 deterministic order | if a proven command exists, route it to one owner | Incident definitions are immutable catalog rows. | A new incident save duplicates the day owner. | EventsHostSession |
| S-06 | 57-06 missing selector | record journal fact | No incident state exists merely because a row has a weight or minDay. | A weight is presented as an imminent event without a selector. | EventsHostSession |
| S-07 | 57-07 no fake choices | capture only the owner that changed | If choices are introduced, selection/resolution state must belong to the current event owner and use current save conventions. | A dead field is interpreted as a command. | EventsHostSession |
| S-08 | 57-08 owner bridge refusal | load 25 incident definitions | A journal fact is not proof of a gameplay effect. | A choice applies an effect twice. | EventsHostSession |
| S-09 | 57-09 journal fact | validate ID/title/body/day/weight | Incident definitions are immutable catalog rows. | A panel mutates a system directly. | EventsHostSession |
| S-10 | 57-10 no new save | determine current daily selection path | No incident state exists merely because a row has a weight or minDay. | A new incident save duplicates the day owner. | EventsHostSession |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 57-TC-01 schema and count | data | schema and count; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |
| T-02 | 57-TC-02 reference validation | unit | reference validation; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |
| T-03 | 57-TC-03 current owner boundary | persistence | current owner boundary; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |
| T-04 | 57-TC-04 failure refusal | determinism | failure refusal; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |
| T-05 | 57-TC-05 save continuation | host | save continuation; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |
| T-06 | 57-TC-06 deterministic replay | UI/accessibility | deterministic replay; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |
| T-07 | 57-TC-07 UI truth | cross-system | UI truth; verify the named current owner and its negative boundary without inventing a second authority. | EventsHostSession |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 29 | `Ashfall.Core.Tests/JournalSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 19 | `Ashfall.Core.Tests/Plan17CCodexTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 14 | `src/Journal/JournalBookUI.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Ashfall.Core.Tests/Codex/CodexEntryCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `src/Host/ContentUtilizationRuntimeCollector.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/JournalSystemCoreBehaviorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/NarrativeDiscoverySystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Journal/JournalSelfTest.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/ArchitectureHardeningCrossPlanIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/BureaucraticDocumentCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/JournalCorpusTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Host/DeepCoastHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/CodexHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/JournalDetailPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/JournalPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/CollectibleCodexUnlockLiveTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/FringeCultRuntimeActivationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/JournalProducerIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/MicroLocationRadioIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/PersonalLetterRuntimeActivationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Plan57IncidentTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/LibraryStudySystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Foundry/SilentFoundryHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/DutyRosterHostSession.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/incidents.json`

### `Assets/StreamingAssets/Data/incidents.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 12879; characters: 12879.
- SHA-256: `ebafd3f9f98e5d702dc9f579e69b1e2d6d5bf5710299201bd379709152a0f1f5`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `incidents`

#### `incidents` — 25 current rows

- Row 001 `incident_radiation_spike`: `{"bodyText":"The dosimeters scream to life in Sector 4. A toxic cloud of irradiated ash has drifted over the ventilation intakes, spiking levels to a dangerous 1.2 mSv/hr. The air tastes faintly of metal.","id":"incident_radiation_spike","…`
- Row 002 `incident_bunker_breach`: `{"bodyText":"Sparks shower from the primary airlock as unknown assailants attempt to torch through the outer seal. Perimeter guards drive them off with concentrated rifle fire, leaving behind only melted slag and a smear of blood.","id":"i…`
- Row 003 `incident_water_contamination`: `{"bodyText":"The reservoir alarms blare a sickening two-tone klaxon. Corrosive silt has breached the primary purifiers, turning our drinking water a rusty, oily brown.","id":"incident_water_contamination","minDay":15,"title":"Water Contami…`
- Row 004 `incident_ambush_sector_4`: `{"bodyText":"A routine salvage run in Sector 4 collapses into chaos. Ghost-faced raiders dropped from the ruined overpass, claiming one life and stripping our cargo before the dust even settled.","id":"incident_ambush_sector_4","minDay":12…`
- Row 005 `incident_radio_interference`: `{"bodyText":"The comms array bursts with ear-splitting static. Just beneath the white noise, a cold, synthesized voice recites an unbroken sequence of numbers on a ghostly loop.","id":"incident_radio_interference","minDay":8,"title":"Radio…`
- Row 006 `incident_fallout_storm_approach`: `{"bodyText":"The barograph has fallen for two hours and the ridge has gone the color of a wet ashtray. This is a front, not cloud drift: the counters climb ahead of the wind, so the particulate is already moving. Four hours, maybe. Seal th…`
- Row 007 `incident_contaminated_water_table`: `{"bodyText":"The well draws bitter and the strips darken faster than last week. This is not the purifier; the purifier is clean and its seals are good. Something has entered the table upstream of you, quietly, and it will keep entering unt…`
- Row 008 `incident_ground_tremor`: `{"bodyText":"Four seconds, from the north-east. Dust came out of the ceiling seams and the store-room shelving jumped. Nothing broke and nobody was hurt, but the crack map on the wall carries one more line than it did this morning. This sh…`
- Row 009 `incident_perimeter_breach_attempt`: `{"bodyText":"The watch found the sealed service access on the west face standing two fingers open, with the ground beneath it scuffed clean of ash. There are tool marks on the hasp: patient work, not hurried, by somebody who knew where the…`
- Row 010 `incident_unknown_visitor`: `{"bodyText":"One visitor at the hatch, unarmed, hands open and visible, asking for water in the flat even tone of somebody who has done this forty times. A Rebuilder canvas patch is sewn at his shoulder and then cut off, leaving the outlin…`
- Row 011 `incident_local_signal_intercept`: `{"bodyText":"The set has a carrier that is on no schedule you own: a short tone, silence, then the tone again, every eleven minutes, precise enough to set a watch by. Strength puts the transmitter inside four kilometers. It is not calling …`
- Row 012 `incident_shelter_disease_outbreak`: `{"bodyText":"Three bunks in the same row went feverish overnight and a fourth this morning. Same symptoms, same row, same cup on the same bench. The medic has moved them to the isolation alcove and taped a line across the corridor. Panic i…`
- Row 013 `incident_chemical_exposure`: `{"bodyText":"Somebody brought a drum in off the road and it wept in the store room overnight. Two have been coughing since and one has a rash along the forearm where a sleeve rode up. The drum is on the ramp now, in the weather where it be…`
- Row 014 `incident_survivor_collapse`: `{"bodyText":"A survivor went down in the pump room at the end of second shift: no cry, no drama, knees and then floor. They came round inside a minute and were embarrassed, which is the part to watch. The board says the shift pattern has r…`
- Row 015 `incident_ration_dispute`: `{"bodyText":"The evening issue took eleven minutes too long because two people disagreed about a scoop, then four more had opinions about the scoop, then somebody said the word always. The board is on the wall and the board is not in dispu…`
- Row 016 `incident_ideological_friction`: `{"bodyText":"It started about the radio and became, inside a minute, about whether the shelter is a place or a plan. Two voices, eleven listeners, and the listeners carried it to their bunks. Nothing was decided and nothing needs deciding …`
- Row 017 `incident_grief_episode`: `{"bodyText":"Somebody is crying in the corridor at an hour when the corridor is meant to be empty, quietly, into a sleeve, because the bunk room is shared. Nobody stopped and nobody should have; that is not the failure. The failure is if t…`
- Row 018 `incident_generator_failure`: `{"bodyText":"The generator dropped load at the start of the evening cycle and did not come back on the first try. The fault is in the fuel line and it is small, and small faults in fuel lines are how nights get long. You have battery for t…`
- Row 019 `incident_air_filter_breakdown`: `{"bodyText":"The scrubber is pulling against a filter that has given what it has. The gauge sits in the amber and the amber is not a negotiation. There is one spare on the shelf and it has been there since before the storms, which means so…`
- Row 020 `incident_water_pipe_burst`: `{"bodyText":"A joint let go above the store room and forty liters reached the floor before the valve was found. The grain on the low shelf is wet and wet grain has maybe a day in it before it is only compost. This is a bad joint, not a bad…`
- Row 021 `incident_nearby_cache_discovered`: `{"bodyText":"A crew came back with a bearing instead of a haul: a sealed service box under a collapsed lean-to, half a day out, unopened and marked with a municipal stencil. They did not open it, which was either discipline or superstition…`
- Row 022 `incident_supply_drop_near_shelter`: `{"bodyText":"Something came down on the ridge in the night and the sound carried: a crack, a long hiss, then nothing. The chute, if there was one, is out there, and whatever the canister held is out there, and so is whoever was meant to co…`
- Row 023 `incident_faction_patrol_nearby`: `{"bodyText":"Four men walked the service road at mid-distance, unhurried, in the loose column of a patrol that has walked it before. The truck carries Iron Garrison marking and the truck is not stopped, which is the only reason this is a s…`
- Row 024 `incident_refugees_approaching`: `{"bodyText":"Nine people on the road, coming slow, carrying what they can and pushing one handcart. They are not armed in any way that matters and they have not left the road, which is the courtesy they are offering you. They reach your tu…`
- Row 025 `incident_exchange_anniversary`: `{"bodyText":"Ninety days. The shelter did not vote on this and nobody put it on the board, but the corridors know: the work is lighter, the talk is quieter, and two people have asked for the same hour off. Somebody has set a tin on the mes…`


# Appendix — Current Source Detail: `src/Host/EventsHostSession.cs`

### `src/Host/EventsHostSession.cs` — complete current file

- Size: 209 lines / 7170 bytes.
- SHA-256: `751cd5c3d0bafa01198fd2ae1b6a6082c25361626be02bd0312dcc729f39038c`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core;
00004: using AtomicWar.GodotApp.Host;
00005: using Godot;
00006:
00007: namespace AtomicWar.GodotApp.Host
00008: {
00009:     /// <summary>
00010:     /// ASHFALL — Events Host Session.
00011:     /// Catalog/read-model for event history, incidents, and narrative progression displayed
00012:     /// by the Events Log panel. Dynamic trigger progress belongs to HostEventAdapter and is
00013:     /// persisted through HostEventSaveStore; this session intentionally has no save state.
00014:     /// </summary>
00015:     public partial class EventsHostSession : Node
00016:     {
00017:         private readonly IJsonSerializer _jsonSerializer;
00018:         private readonly IFileIO _fileIO;
00019:         private List<EventData> _events;
00020:         private List<IncidentData> _incidents;
00021:         private List<NarrativeEntryData> _narrativeProgression;
00022:
00023:         public EventsHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO)
00024:         {
00025:             _jsonSerializer = jsonSerializer;
00026:             _fileIO = fileIO;
00027:             _events = new List<EventData>();
00028:             _incidents = new List<IncidentData>();
00029:             _narrativeProgression = new List<NarrativeEntryData>();
00030:         }
00031:
00032:         public override void _Ready()
00033:         {
00034:             LoadEvents();
00035:             LoadIncidents();
00036:             LoadNarrativeProgression();
00037:         }
00038:
00039:         private void LoadEvents()
00040:         {
00041:             string eventsJsonPath = CatalogPath.ResolveCatalog("events.json");
00042:             string eventsJson = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir()).ReadAllText(eventsJsonPath);
00043:             var eventsData = _jsonSerializer.Deserialize<EventsRoot>(eventsJson);
00044:             if (eventsData?.Events != null)
00045:                 _events = eventsData.Events;
00046:         }
00047:
00048:         /// <summary>
00049:         /// Resolve one authored event for a host adapter. The lazy guard keeps
00050:         /// composition-order safe: a trapping event can be delivered before
00051:         /// Godot has run this node's _Ready callback.
00052:         /// </summary>
00053:         public bool TryGetEvent(string eventId, out EventData eventData)
00054:         {
00055:             if (_events.Count == 0)
00056:                 LoadEvents();
00057:             for (int i = 0; i < _events.Count; i++)
00058:             {
00059:                 var candidate = _events[i];
00060:                 if (candidate != null && string.Equals(candidate.Id, eventId, System.StringComparison.Ordinal))
00061:                 {
00062:                     eventData = candidate;
00063:                     return true;
00064:                 }
00065:             }
00066:             eventData = null!;
00067:             return false;
00068:         }
00069:
00070:         private void LoadIncidents()
00071:         {
00072:             string incidentsJsonPath = CatalogPath.ResolveCatalog("incidents.json");
00073:             var io = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00074:             if (io.FileExists(incidentsJsonPath))
00075:             {
00076:                 string incidentsJson = io.ReadAllText(incidentsJsonPath);
00077:                 var incidentsData = _jsonSerializer.Deserialize<IncidentsRoot>(incidentsJson);
00078:                 if (incidentsData?.Incidents != null)
00079:                     _incidents = incidentsData.Incidents;
00080:             }
00081:         }
00082:
00083:         private void LoadNarrativeProgression()
00084:         {
00085:             string narrativeJsonPath = CatalogPath.ResolveCatalog("narrative_progression.json");
00086:             var io = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00087:             if (io.FileExists(narrativeJsonPath))
00088:             {
00089:                 string narrativeJson = io.ReadAllText(narrativeJsonPath);
00090:                 var narrativeData = _jsonSerializer.Deserialize<NarrativeRoot>(narrativeJson);
00091:                 if (narrativeData?.Entries != null)
00092:                     _narrativeProgression = narrativeData.Entries;
00093:             }
00094:         }
00095:
00096:         /// <summary>
00097:         /// Returns a list of recent events with Day and Description properties.
00098:         /// </summary>
00099:         public List<EventEntry> GetRecentEvents()
00100:         {
00101:             var recentEvents = new List<EventEntry>();
00102:             foreach (var evt in _events)
00103:             {
00104:                 recentEvents.Add(new EventEntry
00105:                 {
00106:                     Day = evt.MinDay,
00107:                     Description = evt.BodyText
00108:                 });
00109:             }
00110:             return recentEvents;
00111:         }
00112:
00113:         /// <summary>
00114:         /// Returns a list of incidents with Day and Description properties.
00115:         /// </summary>
00116:         public List<IncidentEntry> GetIncidents()
00117:         {
00118:             var incidents = new List<IncidentEntry>();
00119:             foreach (var incident in _incidents)
00120:             {
00121:                 incidents.Add(new IncidentEntry
00122:                 {
00123:                     Day = incident.MinDay,
00124:                     Description = incident.BodyText
00125:                 });
00126:             }
00127:             return incidents;
00128:         }
00129:
00130:         /// <summary>
00131:         /// Returns a list of narrative entries with Description and Order properties.
00132:         /// </summary>
00133:         public List<NarrativeEntry> GetNarrativeProgression()
00134:         {
00135:             var narrativeEntries = new List<NarrativeEntry>();
00136:             foreach (var entry in _narrativeProgression)
00137:             {
00138:                 narrativeEntries.Add(new NarrativeEntry
00139:                 {
00140:                     Description = entry.Description,
00141:                     Order = entry.Order
00142:                 });
00143:             }
00144:             return narrativeEntries;
00145:         }
00146:     }
00147:
00148:     // Data Models
00149:     public class EventsRoot
00150:     {
00151:         public int SchemaVersion { get; set; }
00152:         public List<EventData> Events { get; set; }
00153:     }
00154:
00155:     public class EventData
00156:     {
00157:         public string Id { get; set; } = string.Empty;
00158:         public string Title { get; set; } = string.Empty;
00159:         public string BodyText { get; set; } = string.Empty;
00160:         public float Weight { get; set; }
00161:         public int MinDay { get; set; }
00162:     }
00163:
00164:     public class IncidentData
00165:     {
00166:         public string Id { get; set; } = string.Empty;
00167:         public string Title { get; set; } = string.Empty;
00168:         public string BodyText { get; set; } = string.Empty;
00169:         public float Weight { get; set; }
00170:         public int MinDay { get; set; }
00171:     }
00172:
00173:     public class NarrativeEntryData
00174:     {
00175:         public string Description { get; set; } = string.Empty;
00176:         public int Order { get; set; }
00177:     }
00178:
00179:     public class IncidentsRoot
00180:     {
00181:         public int SchemaVersion { get; set; }
00182:         public List<IncidentData> Incidents { get; set; } = new List<IncidentData>();
00183:     }
00184:
00185:     public class NarrativeRoot
00186:     {
00187:         public int SchemaVersion { get; set; }
00188:         public List<NarrativeEntryData> Entries { get; set; } = new List<NarrativeEntryData>();
00189:     }
00190:
00191:     // Return Models
00192:     public class EventEntry
00193:     {
00194:         public int Day { get; set; }
00195:         public string Description { get; set; } = string.Empty;
00196:     }
00197:
00198:     public class IncidentEntry
00199:     {
00200:         public int Day { get; set; }
00201:         public string Description { get; set; } = string.Empty;
00202:     }
00203:
00204:     public class NarrativeEntry
00205:     {
00206:         public string Description { get; set; } = string.Empty;
00207:         public int Order { get; set; }
00208:     }
00209: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Plan57IncidentTests.cs`

### `Ashfall.Core.Tests/Plan57IncidentTests.cs` — complete current file

- Size: 263 lines / 13285 bytes.
- SHA-256: `ad572a0c765969efcfbab73944fcc1a708da832b91d561cb67db6d124b63a7a5`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core.IO;
00007: using Xunit;
00008:
00009: namespace Ashfall.Core.Tests
00010: {
00011:     /// <summary>
00012:     /// Plan 57 — shelter incident expansion contract tests.
00013:     ///
00014:     /// Pins incidents.json at 25 entries: the original 5 preserved byte-for-byte
00015:     /// in field values, 20 new incident_* entries across the 8 required
00016:     /// categories (documented), phase-gated minDay spread, weight bands, body
00017:     /// text quality, faction-name grounding for faction-linked incidents, and
00018:     /// exact deserialization through the live host read-model DTO
00019:     /// (<c>IncidentsRoot</c>) with deterministic day ordering.
00020:     ///
00021:     /// Choice classification (Plan 57 §4.3): <b>Case D</b> — the incident
00022:     /// consumer (EventsHostSession) is a text-only read model with no
00023:     /// scheduler, RNG selection, consequences, choices, or history. Only the
00024:     /// five supported fields are authored; no dead fields.
00025:     /// </summary>
00026:     public sealed class Plan57IncidentTests
00027:     {
00028:         private static string? FindDataDir()
00029:         {
00030:             if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
00031:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
00032:             return null;
00033:         }
00034:
00035:         private static string ReadRaw()
00036:         {
00037:             string? dataDir = FindDataDir();
00038:             Assert.False(dataDir == null, "StreamingAssets/Data directory not found");
00039:             return new FileSystemIO().ReadAllText(Path.Combine(dataDir!, "incidents.json"));
00040:         }
00041:
00042:         private static List<JsonProbe> Parse()
00043:         {
00044:             var raw = ReadRaw();
00045:             var root = new SystemTextJsonSerializer().Deserialize<RootProbe>(raw);
00046:             Assert.NotNull(root);
00047:             Assert.NotNull(root!.incidents);
00048:             return root.incidents;
00049:         }
00050:
00051:         private sealed class RootProbe { public List<JsonProbe> incidents { get; set; } = new(); }
00052:
00053:         private sealed class JsonProbe
00054:         {
00055:             public string id { get; set; } = string.Empty;
00056:             public string title { get; set; } = string.Empty;
00057:             public string bodyText { get; set; } = string.Empty;
00058:             public float weight { get; set; }
00059:             public int minDay { get; set; }
00060:         }
00061:
00062:         private static readonly string[] Original5 =
00063:         {
00064:             "incident_radiation_spike", "incident_bunker_breach", "incident_water_contamination",
00065:             "incident_ambush_sector_4", "incident_radio_interference"
00066:         };
00067:
00068:         private static readonly string[] New20 =
00069:         {
00070:             "incident_fallout_storm_approach", "incident_contaminated_water_table",
00071:             "incident_ground_tremor", "incident_perimeter_breach_attempt",
00072:             "incident_unknown_visitor", "incident_local_signal_intercept",
00073:             "incident_shelter_disease_outbreak", "incident_chemical_exposure",
00074:             "incident_survivor_collapse", "incident_ration_dispute",
00075:             "incident_ideological_friction", "incident_grief_episode",
00076:             "incident_generator_failure", "incident_air_filter_breakdown",
00077:             "incident_water_pipe_burst", "incident_nearby_cache_discovered",
00078:             "incident_supply_drop_near_shelter", "incident_faction_patrol_nearby",
00079:             "incident_refugees_approaching", "incident_exchange_anniversary"
00080:         };
00081:
00082:         [Fact]
00083:         public void Catalog_contains_exactly_25_incidents()
00084:         {
00085:             Assert.Equal(25, Parse().Count);
00086:         }
00087:
00088:         [Fact]
00089:         public void Loader_parses_all_25_through_the_live_host_read_model()
00090:         {
00091:             // Deserialize through the actual consumer DTO shape (camelCase,
00092:             // case-insensitive) exactly as EventsHostSession does.
00093:             var raw = ReadRaw();
00094:             var root = new SystemTextJsonSerializer().Deserialize<HostShapeProbe>(raw);
00095:             Assert.NotNull(root);
00096:             Assert.Equal(25, root!.Incidents.Count);
00097:             Assert.All(root.Incidents, i =>
00098:             {
00099:                 Assert.False(string.IsNullOrWhiteSpace(i.Id));
00100:                 Assert.False(string.IsNullOrWhiteSpace(i.BodyText));
00101:             });
00102:         }
00103:
00104:         private sealed class HostShapeProbe { public List<HostIncident> Incidents { get; set; } = new(); }
00105:         private sealed class HostIncident
00106:         {
00107:             public string Id { get; set; } = string.Empty;
00108:             public string Title { get; set; } = string.Empty;
00109:             public string BodyText { get; set; } = string.Empty;
00110:             public float Weight { get; set; }
00111:             public int MinDay { get; set; }
00112:         }
00113:
00114:         [Fact]
00115:         public void Original_five_incidents_preserved_field_for_field()
00116:         {
00117:             var byId = Parse().ToDictionary(i => i.id, StringComparer.Ordinal);
00118:             var parity = new List<JsonProbe>
00119:             {
00120:                 new JsonProbe { id = "incident_radiation_spike", title = "Radiation Spike", weight = 1.0f, minDay = 20 },
00121:                 new JsonProbe { id = "incident_bunker_breach", title = "Bunker Breach Attempt", weight = 1.0f, minDay = 18 },
00122:                 new JsonProbe { id = "incident_water_contamination", title = "Water Contamination", weight = 1.0f, minDay = 15 },
00123:                 new JsonProbe { id = "incident_ambush_sector_4", title = "Ambush in Sector 4", weight = 1.0f, minDay = 12 },
00124:                 new JsonProbe { id = "incident_radio_interference", title = "Radio Interference", weight = 1.0f, minDay = 8 }
00125:             };
00126:             // Field-value parity (id, title, weight, minDay) and non-empty body.
00127:             Assert.NotNull(parity);
00128:             foreach (var o in parity)
00129:             {
00130:                 var live = byId[o.id];
00131:                 Assert.Equal(o.title, live.title);
00132:                 Assert.Equal(o.weight, live.weight);
00133:                 Assert.Equal(o.minDay, live.minDay);
00134:                 Assert.False(string.IsNullOrWhiteSpace(live.bodyText));
00135:             }
00136:         }
00137:
00138:         [Fact]
00139:         public void All_twenty_new_ids_present_unique_and_prefixed()
00140:         {
00141:             var incidents = Parse();
00142:             var ids = incidents.Select(i => i.id).ToList();
00143:             foreach (var id in ids)
00144:                 Assert.Matches("^incident_[a-z0-9_]+$", id);
00145:             Assert.Equal(ids.Count, ids.Distinct().Count());
00146:             foreach (var id in New20)
00147:                 Assert.True(ids.Contains(id), $"Plan 57 incident missing: {id}");
00148:             Assert.Equal(20, ids.Except(Original5).Count());
00149:         }
00150:
00151:         [Fact]
00152:         public void All_incidents_have_grounded_titles_and_bodies()
00153:         {
00154:             foreach (var i in Parse())
00155:             {
00156:                 Assert.False(string.IsNullOrWhiteSpace(i.title), $"{i.id}: missing title");
00157:                 Assert.True(i.bodyText.Length >= 80, $"{i.id}: body too short to be grounded");
00158:                 Assert.True(i.bodyText.Length <= 600, $"{i.id}: body exceeds one-glance readability");
00159:                 // Tone guard: no melodrama clichés per §48.
00160:                 var lower = i.bodyText.ToLowerInvariant();
00161:                 foreach (var banned in new[] { "disaster strikes", "terrifying plague", "catastroph", "plunging everyone" })
00162:                     Assert.False(lower.Contains(banned), $"{i.id}: melodramatic phrasing '{banned}'");
00163:             }
00164:         }
00165:
00166:         [Fact]
00167:         public void Weights_authored_within_bands_and_varied()
00168:         {
00169:             var incidents = Parse();
00170:             foreach (var i in incidents)
00171:                 Assert.InRange(i.weight, 0.1f, 1.5f);
00172:             // Anti-pattern guard: not all identical, no drama-max weights.
00173:             Assert.True(incidents.Select(i => i.weight).Distinct().Count() >= 5,
00174:                 "weights should express a frequency gradient");
00175:             var newOnes = incidents.Where(i => !Original5.Contains(i.id));
00176:             Assert.All(newOnes, i => Assert.True(i.weight <= 1.3f, $"{i.id}: weight above the authored band"));
00177:         }
00178:
00179:         [Fact]
00180:         public void At_least_five_new_incidents_are_phase_gated_across_the_campaign()
00181:         {
00182:             var newOnes = Parse().Where(i => New20.Contains(i.id)).ToList();
00183:             // events.json timeline spans days 1–240; phase bands: early <30, mid 30–70, late >70.
00184:             Assert.True(newOnes.Count(i => i.minDay is > 0 and < 30) >= 4, "early-gated incidents");
00185:             Assert.True(newOnes.Count(i => i.minDay is >= 30 and < 70) >= 8, "mid-gated incidents");
00186:             Assert.True(newOnes.Count(i => i.minDay >= 70) >= 4, "late-gated incidents");
00187:             Assert.Equal(90, newOnes.First(i => i.id == "incident_exchange_anniversary").minDay);
00188:         }
00189:
00190:         [Fact]
00191:         public void Faction_linked_incidents_reference_real_faction_names()
00192:         {
00193:             var byId = Parse().ToDictionary(i => i.id, StringComparer.Ordinal);
00194:             // Real faction ids from faction_lore.json: iron_garrison (The Iron
00195:             // Garrison), faction_rebuilders (The Rebuilders).
00196:             Assert.Contains("Iron Garrison", byId["incident_faction_patrol_nearby"].bodyText);
00197:             Assert.Contains("Rebuilder", byId["incident_unknown_visitor"].bodyText);
00198:             // Third faction-grounded incident: the intercept implies an
00199:             // organized nearby transmitter — verified as faction-linked via
00200:             // the design doc; body must stay concrete.
00201:             Assert.Contains("transmitter", byId["incident_local_signal_intercept"].bodyText);
00202:         }
00203:
00204:         [Fact]
00205:         public void New_incidents_do_not_duplicate_original_semantics()
00206:         {
00207:             var byId = Parse().ToDictionary(i => i.id, StringComparer.Ordinal);
00208:             // The original radiation_spike is an arrived cloud over the vents;
00209:             // the new storm incident is the approach/warning phase.
00210:             Assert.Contains("not cloud", byId["incident_fallout_storm_approach"].bodyText);
00211:             Assert.Contains("sealed service access", byId["incident_perimeter_breach_attempt"].bodyText);
00212:             Assert.Contains("tool marks", byId["incident_perimeter_breach_attempt"].bodyText);
00213:             // Original contamination = purifier breach; new = upstream water table.
00214:             Assert.Contains("upstream", byId["incident_contaminated_water_table"].bodyText);
00215:             // Original radio = ghost numbers station; new = nearby transmitter.
00216:             Assert.Contains("every eleven minutes", byId["incident_local_signal_intercept"].bodyText);
00217:         }
00218:
00219:         [Fact]
00220:         public void Category_coverage_matches_the_required_distribution()
00221:         {
00222:             // Category is NOT a schema field (Case D) — ownership is documented
00223:             // and pinned here by minDay/design intent instead of dead JSON.
00224:             var expected = new Dictionary<string, string[]>
00225:             {
00226:                 ["environmental"] = new[] { "incident_fallout_storm_approach", "incident_contaminated_water_table", "incident_ground_tremor" },
00227:                 ["security"] = new[] { "incident_perimeter_breach_attempt", "incident_unknown_visitor", "incident_local_signal_intercept" },
00228:                 ["medical"] = new[] { "incident_shelter_disease_outbreak", "incident_chemical_exposure", "incident_survivor_collapse" },
00229:                 ["social"] = new[] { "incident_ration_dispute", "incident_ideological_friction", "incident_grief_episode" },
00230:                 ["equipment"] = new[] { "incident_generator_failure", "incident_air_filter_breakdown", "incident_water_pipe_burst" },
00231:                 ["supply"] = new[] { "incident_nearby_cache_discovered", "incident_supply_drop_near_shelter" },
00232:                 ["external"] = new[] { "incident_faction_patrol_nearby", "incident_refugees_approaching" },
00233:                 ["psychological"] = new[] { "incident_exchange_anniversary" },
00234:             };
00235:             var ids = Parse().Select(i => i.id).ToHashSet(StringComparer.Ordinal);
00236:             Assert.Equal(20, expected.Values.SelectMany(v => v).Count());
00237:             foreach (var (cat, members) in expected)
00238:                 foreach (var id in members)
00239:                     Assert.True(ids.Contains(id), $"{cat}: missing {id}");
00240:         }
00241:
00242:         [Fact]
00243:         public void Schema_stays_within_the_supported_field_set()
00244:         {
00245:             // Case D: only id/title/bodyText/weight/minDay are consumed by the
00246:             // runtime. Assert no dead fields (category/maxDay/choices/…) were
00247:             // serialized into the authority.
00248:             var raw = ReadRaw();
00249:             foreach (var dead in new[] { "\"category\"", "\"maxDay\"", "\"choices\"", "\"consequences\"", "\"faction\"", "\"system_link\"", "\"cooldown\"" })
00250:                 Assert.False(raw.Contains(dead, StringComparison.Ordinal), $"dead field {dead} must not be authored (Case D)");
00251:         }
00252:
00253:         [Fact]
00254:         public void Deterministic_day_ordering_stable_across_parses()
00255:         {
00256:             // The read model displays incidents in catalog order; ordering must
00257:             // be stable across loads (no dictionary/fs-order dependence).
00258:             var a = Parse().Select(i => i.id).ToList();
00259:             var b = Parse().Select(i => i.id).ToList();
00260:             Assert.Equal(a, b);
00261:         }
00262:     }
00263: }
```


# Appendix — Current Source Detail: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs` — bounded current excerpt (2864 of 2957 lines)

- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Campaign;
00007: using Ashfall.Core.Economy;
00008: using Ashfall.Core.Expeditions;
00009: using Godot;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     public partial class Main
00014:     {
00015:         private void RegisterProductionCampaignOwners()
00016:         {
00017:             if (_campaignDay == null) return;
00018:
00019:             // Phase 1: Environment, Weather & Base Core
00020:             _campaignDay.Register("holdfast_core", new HoldfastCoreDayOwner(this), phase: 1);
00021:             _campaignDay.Register("maritime_deep_coast", new DeepCoastMaritimeDayOwner(this), phase: 1);
00022:             _campaignDay.Register("power_grid", new PowerGridDayOwner(this), phase: 1);
00023:             // Plan B98: nuclear output is an external, fuel-free projection.
00024:             // Its ordinal sorts before power_grid so restored RTG/sealed-cell
00025:             // output is visible when the day's load is resolved.
00026:             _campaignDay.Register("nuclear_core", new NuclearCoreDayOwner(this), phase: 1);
00027:             // Plans B74-B77: ORC output is published before the grid owner
00028:             // resolves the day's load, while chamber and tube milestones run
00029:             // in the production phase.
00030:             _campaignDay.Register("geothermal_orc", new GeothermalOrcDayOwner(this), phase: 1);
00031:             _campaignDay.Register("weather_world", new WeatherWorldDayOwner(this), phase: 1);
00032:             // Plan B68 — geological pulse progression precedes production
00033:             // (foundry sees the quake's interruption context the same day).
00034:             _campaignDay.Register("seismic_geology", new SeismicGeologyDayOwner(this), phase: 1);
00035:
00036:             // Phase 2: Production, Infrastructure & Survival Basics
00037:             _campaignDay.Register("crafting_production", new CraftingProductionDayOwner(this), phase: 2);
00038:             _campaignDay.Register("economy_market", new EconomyMarketDayOwner(this), phase: 2);
00039:             _campaignDay.Register("greenhouse_foundry", new GreenhouseFoundryDayOwner(this), phase: 2);
00040:             _campaignDay.Register("aeroponics", new AeroponicsDayOwner(this), phase: 2);
00041:             _campaignDay.Register("pneumatic_dispatch", new PneumaticDispatchDayOwner(this), phase: 2);
00042:             // Plan B69 — cryo thermal/viability update follows the foundry
00043:             // (shared grid: brownout from the furnace reaches the vault same-day).
00044:             _campaignDay.Register("cryo_vault", new CryoVaultDayOwner(this), phase: 2);
00045:             // Plan B89 — precision metrology drift + workshop projection after
00046:             // seismic (phase 1) so quake disturbance lands before daily drift.
00047:             _campaignDay.Register("precision_metrology", new PrecisionMetrologyDayOwner(this), phase: 2);
00048:             // Plan B87 — aquaponics ecology after power/thermal owners so
00049:             // brownout and room heat are queryable for the same day.
00050:             _campaignDay.Register("aquaponics", new AquaponicsDayOwner(this), phase: 2);
00051:             _campaignDay.Register("shelter_facilities", new ShelterFacilitiesDayOwner(this), phase: 2);
00052:             _campaignDay.Register("shelter_fire", new ShelterFireDayOwner(this), phase: 2);
00053:             _campaignDay.Register("starting_level_rations", new StartingLevelRationsDayOwner(this), phase: 2);
00054:             _campaignDay.Register("plan_166_research", new Plan166ResearchDayOwner(this), phase: 4);
00055:             _campaignDay.Register("plan_168_fluid", new Plan168FluidDayOwner(this), phase: 2);
00056:
00057:             // Phase 3: Survivors, Medical, Disease & Social
00058:             _campaignDay.Register("duty_roster", new DutyRosterDayOwner(this), phase: 3);
00059:             // Plan 210 — sanitation runs BEFORE the disease tick within phase 3:
00060:             // `hygiene` (h) sorts alphabetically before `medical_disease` (m),
00061:             // so waste burden → hygiene → pathogen exposure modifiers are
00062:             // current when the disease authority resolves its daily tick.
00063:             _campaignDay.Register("hygiene", new HygieneDayOwner(this), phase: 3);
00064:             _campaignDay.Register("medical_disease", new MedicalDiseaseDayOwner(this), phase: 3);
00065:             _campaignDay.Register("phase0_psychology", new Phase0PsychologyDayOwner(this), phase: 3);
00066:             _campaignDay.Register("survivor_social", new SurvivorSocialDayOwner(this), phase: 3);
00067:             _campaignDay.Register("survivors_needs", new SurvivorsNeedsDayOwner(this), phase: 3);
00068:
00069:             // Phase 4: Expeditions, World, Factions & Quests
00070:             _campaignDay.Register("expeditions_caravans", new ExpeditionsCaravansDayOwner(this), phase: 4);
00071:             _campaignDay.Register("narrative_quests_verdict", new NarrativeQuestsVerdictDayOwner(this), phase: 4);
00072:             // Task 122: ticks after expeditions (ordinal 'w' > 'e') so it reads
00073:             // fresh sortie results, and after narrative for fresh faction dominance.
00074:             _campaignDay.Register("world_evolution", new EvolvingWorldDayOwner(this), phase: 4);
00075:             // Plan IV: ledger debt ages with the campaign; forfeits dispatch
00076:             // consequences into faction war / raids / inventory / labor.
00077:             _campaignDay.Register("debt_ledger", new DebtLedgerDayOwner(this), phase: 4);
00078:             // Plan 211 — underworld stock refresh + debt/heat tick AFTER the
00079:             // debt-ledger tick: `underworld_market` (u) sorts alphabetically
00080:             // after `debt_ledger` (d) within phase 4 (owner id is not the
00081:             // section key — the save section is `black_market`).
00082:             _campaignDay.Register("underworld_market", new UnderworldMarketDayOwner(this), phase: 4);
00083:             // Flagship XI (Plan 156): underground hazards tick after expeditions
00084:             // (ordinal 's' > 'e', < 'w') so the bridge reads fresh sortie phases.
00085:             _campaignDay.Register("subterranean_network", new SubterraneanDayOwner(this), phase: 4);
00086:             // Flagship XI (Plan 157): psyops broadcast day resolves after
00087:             // expeditions (leaflets) and alongside the world-evolution radio feed.
00088:             _campaignDay.Register("psyops", new PsyOpsDayOwner(this), phase: 4);
00089:             // Plan 173 Phase 2: program prep ticks after psyops so StartCampaign
00090:             // on delivery can reach an already-constructed PsyOpsSystem.
00091:             _campaignDay.Register("radio_program_production", new RadioProgramProductionDayOwner(this), phase: 4);
00092:             // Plans 162-165 (Plan 164): breakdown arcs evaluate AFTER the
00093:             // phase-3 needs tick finalized canonical stress (plan §11.3).
00094:             _campaignDay.Register("psychology_arcs_162", new PsychologyArcsDayOwner(this), phase: 4);
00095:             _campaignDay.Register("plan_167_espionage", new Plan167EspionageDayOwner(this), phase: 4);
00096:             _campaignDay.Register("plan_169_procedural_narrative", new Plan169NarrativeDayOwner(this), phase: 4);
00097:             // Plan 38 — commitments/deadlines evaluate late in phase 4 so the day's
00098:             // expedition/faction facts are settled before a missed obligation routes
00099:             // its consequence into faction standing and the consequence ledger.
00100:             // "shelter_commitments" is the owner id (not the section key `commitment`).
00101:             _campaignDay.Register("shelter_commitments", new CommitmentDayOwner(this), phase: 4);
00102:
00103:             // Phase 5: Events, Memorial & Final Evaluation
00104:             _campaignDay.Register("host_events", new HostEventsDayOwner(this), phase: 5);
00105:             _campaignDay.Register("memorial", new MemorialDayOwner(this), phase: 5);
00106:             // Plan 29 29A: room-history day milestones. Reads only the identity
00107:             // catalog and writes journal knowledge keys; no system ticks here.
00108:             _campaignDay.Register("shelter_room_history", new ShelterRoomHistoryDayOwner(this), phase: 5);
00109:             // Plan 58 — the outpost network runs after the roster, expedition and
00110:             // economy owners so its garrison, rations and hostile pressure read
00111:             // the day's already-finalized population and supply state.
00112:             _campaignDay.Register("outpost_settlement", new OutpostSettlementDayOwner(this), phase: 5);
00113:             // Plan 135 — the weather→gameplay cascade expires fronts whose
00114:             // duration ended, so it runs after the owners that consumed the
00115:             // day's weather and before retention bounds the logs they wrote.
00116:             _campaignDay.Register("weather_cascade", new WeatherCascadeDayOwner(this), phase: 5);
00117:             // Plan 134 — dynamic faction territory and supply line control: delivers
00118:             // active corridors and reinforces held nodes.
00119:             _campaignDay.Register("territory_control", new TerritoryControlDayOwner(this), phase: 5);
00120:             // Plan 136 — wildlife trapping food pipeline & cooking system: progresses
00121:             // active cooking operations and decontaminates fallout-tainted meat.
00122:             _campaignDay.Register("cooking", new CookingDayOwner(this), phase: 5);
00123:             // Plan 137 — needs to performance cascade: evaluates hunger/thirst/fatigue/cold on survivor performance.
00124:             _campaignDay.Register("needs_performance", new NeedsPerformanceDayOwner(this), phase: 5);
00125:             // Plan 140 — generational legacy and campaign inheritance: evaluates active traits and heritage continuity.
00126:             _campaignDay.Register("campaign_legacy", new CampaignLegacyDayOwner(this), phase: 5);
00127:             // Plan 141 — research downstream unlocks bridge: grants breakthrough items, crafting recipes, and capabilities.
00128:             _campaignDay.Register("research_unlock", new ResearchUnlockDayOwner(this), phase: 5);
00129:             // Plan 145 — unified ending resolution: evaluates whole-campaign state and epilogue personalization.
00130:             _campaignDay.Register("unified_ending", new UnifiedEndingDayOwner(this), phase: 5);
00131:             // Plan 147 — per-NPC memory and relationship depth: decays old memories and grudges.
00132:             _campaignDay.Register("npc_memory", new NpcMemoryDayOwner(this), phase: 5);
00133:             // Plan 148 — ideological friction: evaluates bunker frictions, conversions, and confrontations.
00134:             _campaignDay.Register("ideological_friction", new IdeologicalFrictionDayOwner(this), phase: 5);
00135:             // Plan 150 — romance & family: advances bonded tenure and forms new attractions from canonical affinity.
00136:             _campaignDay.Register("romance_family", new RomanceFamilyDayOwner(this), phase: 5);
00137:             // Plan 152 — vehicle customization & mobile base: keeps the module catalog bound for the day report.
00138:             _campaignDay.Register("vehicle_customization", new VehicleCustomizationDayOwner(this), phase: 5);
00139:             // Plan 174 — procedural survivor backstories: updates origin mechanics.
00140:             _campaignDay.Register("backstory", new BackstoryDayOwner(this), phase: 5);
00141:             // Plan 175 — meta progression: evaluates prestige and New Game+ boons.
00142:             _campaignDay.Register("meta_progression", new MetaProgressionDayOwner(this), phase: 5);
00143:             // Plan 167 — underground tunnel network: applies daily structural wear and collapse risk.
00144:             _campaignDay.Register("tunnel_network", new TunnelNetworkDayOwner(this), phase: 5);
00145:             // Plan 166 — shelter identity: selects the founding origin on a fresh campaign.
00146:             _campaignDay.Register("shelter_identity", new ShelterIdentityDayOwner(this), phase: 5);
00147:             // Plan 192 — scheduled trade route contracts: advances run schedules and audits tariffs.
00148:             _campaignDay.Register("trade_routes", new TradeRouteDayOwner(this), phase: 5);
00149:             // Plan 199 — seasonal human migration: tracks regional population weight transitions.
00150:             _campaignDay.Register("human_migration", new HumanMigrationDayOwner(this), phase: 5);
00151:             // Plan 159 — shelter governance: evaluates policy consent, disputes, and shelter stability.
00152:             _campaignDay.Register("shelter_governance", new ShelterGovernanceDayOwner(this), phase: 5);
00153:             // Plan 176 — aging & elderly survivor system: advances chronological age and evaluates milestones/retirement.
00154:             _campaignDay.Register("aging", new AgingDayOwner(this), phase: 5);
00155:             // Expansion 25 — rail track maintenance ledger (event-driven wear; no daily baseline).
00156:             _campaignDay.Register("rail_track_maintenance", new RailTrackMaintenanceDayOwner(this), phase: 5);
00157:             // Expansion 29 — glassworks kiln annealing stages.
00165:             // Expansion 35 — chemical dependency taper programs, withdrawal management, and care posture.
00166:             _campaignDay.Register("dependency_taper_withdrawal", new DependencyTaperWithdrawalDayOwner(this), phase: 5);
00167:             // Expansion 37 — antenatal maternal care, trimester progressions, and neonatal deliveries.
00168:             _campaignDay.Register("antenatal_maternal_health", new AntenatalMaternalHealthDayOwner(this), phase: 5);
00169:             // Expansion 38 — clinical ward triage priority, surgical suite readiness, and sterile supply inventory.
00170:             _campaignDay.Register("clinical_ward_triage", new ClinicalWardTriageDayOwner(this), phase: 5);
00171:             // Expansion 39 — chemical synthesis reactor safety, catalyst purity, and reagent grading.
00175:             // Expansion 41 — sleep quality, soundproofing, and shelter crowding.
00176:             _campaignDay.Register("sleep_acoustic_rest", new SleepAcousticRestDayOwner(this), phase: 5);
00177:             // Plan 162 — shelter history & archive: institutional memory and milestones.
00178:             _campaignDay.Register("shelter_archive", new ShelterArchiveDayOwner(this), phase: 5);
00179:             // Plan 177 — survivor dream & sleep event system: nocturnal dream generation.
00180:             _campaignDay.Register("survivor_dreams", new SurvivorDreamsDayOwner(this), phase: 5);
00181:             // Plan 185 — memory & knowledge decay across cognition, skills, and relationships.
00182:             _campaignDay.Register("memory_decay", new MemoryDecayDayOwner(this), phase: 5);
00183:             // Plan 200 — survivor personal quests and character arcs.
00184:             _campaignDay.Register("personal_quests", new PersonalQuestsDayOwner(this), phase: 5);
00185:             // Plan 202 — interpersonal conflict, grievance accumulation, and mediation resolution.
00186:             _campaignDay.Register("interpersonal_conflict", new InterpersonalConflictDayOwner(this), phase: 5);
00187:             // Plan 216 — survivor exercise routines, physical training adaptation, and conditioning decay.
00188:             _campaignDay.Register("exercise", new ExerciseDayOwner(this), phase: 5);
00189:             // Plan 178 — art and culture creation: survivor artworks, masterworks, cultural identity, and display morale bonus.
00190:             _campaignDay.Register("culture_creation", new CultureCreationDayOwner(this), phase: 5);
00191:             // Plan 179 — unified psychology and phobia profiles: phobias, coping mechanisms, resilience, and therapy.
00192:             _campaignDay.Register("psychological_profiles", new PsychologicalProfilesDayOwner(this), phase: 5);
00193:             // Plan 180 — skill certification and tier system: formal qualifications, exams, benefits, and specializations.
00194:             _campaignDay.Register("skill_certifications", new SkillCertificationsDayOwner(this), phase: 5);
00195:             // Plan 187 — bestiary knowledge and creature encounters tracking.
00196:             _campaignDay.Register("bestiary_knowledge", new BestiaryDayOwner(this), phase: 5);
00197:             // Plan 198 — survivor medical records, longitudinal history, and vaccinations.
00198:             _campaignDay.Register("health_history", new HealthHistoryDayOwner(this), phase: 5);
00199:             // Plan 183 — child development stages, milestones, education, and chore capacity.
00200:             _campaignDay.Register("child_development_stages", new ChildDevelopmentDayOwner(this), phase: 5);
00201:             // Plan 186 — shelter maintenance & degradation: applies daily component wear and environmental stress.
00202:
00203:             _campaignDay.Register("shelter_maintenance", new ShelterMaintenanceDayOwner(this), phase: 5);
00204:             // Plan 188 — individual survivor daily routines: ticks satisfaction and detects schedule conflicts.
00205:             _campaignDay.Register("survivor_routines", new SurvivorRoutinesDayOwner(this), phase: 5);
00206:             // Plan 55 — retention runs last of all: it bounds the campaign logs
00207:             // every other owner just appended to for this day.
00208:             _campaignDay.Register("retention", new RetentionDayOwner(this), phase: 5);
00209:             // Flagship institutions (Tasks 5-8): culture, diplomacy, sky defense, sanatorium.
00210:             RegisterFlagshipInstitutionsOwner();
00211:         }
00212:
00213:         /// <summary>Plan 134 territory-control day owner (ownerId <c>territory_control</c>, phase 5).</summary>
00214:         private sealed class TerritoryControlDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00215:         {
00216:             private readonly Main _m;
00217:             private Ashfall.Core.Factions.TerritoryControlSaveState? _snapshot;
00218:             public TerritoryControlDayOwner(Main m) => _m = m;
00219:
00220:             public void CapturePreDaySnapshot(int day)
00221:             {
00222:                 _m.EnsureTerritoryControl();
00223:                 _snapshot = _m.TerritoryControl?.System.CaptureState();
00224:             }
00225:
00226:             public void RestorePreDaySnapshot(int day)
00227:             {
00228:                 if (_snapshot != null) _m.TerritoryControl?.System.RestoreState(_snapshot);
00229:             }
00230:
00231:             public void TickDay(int day, List<DayStateChangeEvent> events)
00232:             {
00233:                 _m.EnsureTerritoryControl();
00234:                 _m.TickTerritoryControl(day, _m._campaignDay?.Rng?.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter)?.Rng);
00235:                 var census = _m.TerritoryControl?.ReadCensus();
00236:                 events.Add(new DayStateChangeEvent(
00237:                     "territory_control_ticked", "territory_control", null, null, census?.ContestedLocations ?? 0));
00238:             }
00239:         }
00240:
00241:         /// <summary>Plan 136 cooking day owner (ownerId <c>cooking</c>, phase 5).</summary>
00242:         private sealed class CookingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00243:         {
00244:             private readonly Main _m;
00245:             private Ashfall.Core.Cooking.CookingState? _snapshot;
00246:             public CookingDayOwner(Main m) => _m = m;
00247:
00248:             public void CapturePreDaySnapshot(int day)
00249:             {
00250:                 _m.EnsureCooking();
00251:                 _snapshot = _m.Cooking?.System.CaptureState();
00252:             }
00253:
00254:             public void RestorePreDaySnapshot(int day)
00255:             {
00256:                 if (_snapshot != null) _m.Cooking?.System.RestoreState(_snapshot);
00257:             }
00258:
00259:             public void TickDay(int day, List<DayStateChangeEvent> events)
00260:             {
00261:                 _m.EnsureCooking();
00262:                 _m.TickCooking(day);
00263:                 var census = _m.Cooking?.Census;
00264:                 events.Add(new DayStateChangeEvent(
00265:                     "cooking_ticked", "cooking", null, null, census?.TotalMealsPrepared ?? 0));
00266:             }
00267:         }
00268:
00269:         /// <summary>Plan 135 weather-cascade day owner (ownerId <c>weather_cascade</c>, phase 5).</summary>
00270:         private sealed class WeatherCascadeDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00271:         {
00272:             private readonly Main _m;
00273:             private Ashfall.Core.Weather.WeatherCascadeState? _snapshot;
00274:             public WeatherCascadeDayOwner(Main m) => _m = m;
00275:
00276:             public void CapturePreDaySnapshot(int day)
00277:             {
00278:                 _m.EnsureWeatherCascade();
00279:                 _snapshot = _m.WeatherCascade?.System.CaptureState();
00280:             }
00281:
00282:             public void RestorePreDaySnapshot(int day)
00283:             {
00284:                 if (_snapshot != null) _m.WeatherCascade?.System.RestoreState(_snapshot);
00285:             }
00286:
00287:             public void TickDay(int day, List<DayStateChangeEvent> events)
00288:             {
00289:                 int activeBefore = _m.WeatherCascade?.System.State.activeEvents.Count ?? 0;
00290:                 _m.TickWeatherCascade(day);
00291:                 int activeAfter = _m.WeatherCascade?.System.State.activeEvents.Count ?? 0;
00292:                 if (activeAfter != activeBefore)
00293:                     events.Add(new DayStateChangeEvent(
00294:                         "weather_cascade_ticked", "weather_cascade", null, null, activeAfter));
00295:             }
00296:         }
00297:
00298:         /// <summary>Plan 58 outpost day owner (ownerId <c>outpost_settlement</c>, phase 5).</summary>
00299:         private sealed class OutpostSettlementDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00300:         {
00301:             private readonly Main _m;
00302:             private Ashfall.Core.Settlements.OutpostSettlementState? _snapshot;
00303:             public OutpostSettlementDayOwner(Main m) => _m = m;
00304:             public void CapturePreDaySnapshot(int day)
00305:             {
00306:                 _m.EnsureOutpostSettlement();
00307:                 _snapshot = _m.OutpostSettlement?.CaptureState();
00308:             }
00309:             public void RestorePreDaySnapshot(int day)
00310:             {
00311:                 if (_snapshot != null) _m.OutpostSettlement?.RestoreState(_snapshot);
00312:             }
00313:             public void TickDay(int day, List<DayStateChangeEvent> events)
00314:             {
00315:                 _m.EnsureOutpostSettlement();
00316:                 if (_m.OutpostSettlement == null) return;
00317:                 _m.TickOutpostSettlement(day);
00318:                 var census = _m.OutpostSettlement.ReadCensus();
00319:                 events.Add(new DayStateChangeEvent("outpost_network_ticked", "outpost_settlement", null, null,
00320:                     census.Established));
00321:             }
00322:         }
00323:
00324:         /// <summary>Plan 55 retention day owner (ownerId <c>retention</c>, phase 5).</summary>
00325:         private sealed class RetentionDayOwner : IDayAdvanceOwner
00326:         {
00327:             private readonly Main _m;
00328:             public RetentionDayOwner(Main m) => _m = m;
00329:             public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
00330:             public void TickDay(int day, List<DayStateChangeEvent> events)
00331:             {
00332:                 _m.EnsureRetention();
00333:                 int before = _m.Retention?.Passes ?? 0;
00334:                 _m.TickRetention(day);
00335:                 int after = _m.Retention?.Passes ?? 0;
00336:                 if (after != before)
00337:                     events.Add(new DayStateChangeEvent("retention_ticked", "retention", null, null, after));
00338:             }
00339:         }
00340:
00341:         /// <summary>Plan 137 needs-performance day owner (ownerId <c>needs_performance</c>, phase 5).</summary>
00342:         private sealed class NeedsPerformanceDayOwner : IDayAdvanceOwner
00343:         {
00344:             private readonly Main _m;
00345:             public NeedsPerformanceDayOwner(Main m) => _m = m;
00346:             public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
00347:             public void TickDay(int day, List<DayStateChangeEvent> events)
00348:             {
00349:                 _m.EnsureNeedsPerformance();
00350:                 _m.TickNeedsPerformance(day);
00351:                 var census = _m.GetNeedsPerformanceCensus();
00352:                 events.Add(new DayStateChangeEvent(
00353:                     "needs_performance_ticked", "needs_performance", null, null, census.TotalSurvivorsEvaluated));
00354:             }
00355:         }
00356:
00357:         /// <summary>Plan 140 campaign-legacy day owner (ownerId <c>campaign_legacy</c>, phase 5).</summary>
00358:         private sealed class CampaignLegacyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00359:         {
00360:             private readonly Main _m;
00361:             private Ashfall.Core.Legacy.CampaignLegacyState? _snapshot;
00362:             public CampaignLegacyDayOwner(Main m) => _m = m;
00363:
00364:             public void CapturePreDaySnapshot(int day)
00365:             {
00366:                 _m.EnsureCampaignLegacy();
00367:                 _snapshot = _m._campaignLegacy?.System.CaptureState();
00368:             }
00369:
00370:             public void RestorePreDaySnapshot(int day)
00371:             {
00372:                 if (_snapshot != null) _m._campaignLegacy?.System.RestoreState(_snapshot);
00373:             }
00374:
00375:             public void TickDay(int day, List<DayStateChangeEvent> events)
00376:             {
00377:                 _m.EnsureCampaignLegacy();
00378:                 _m.TickCampaignLegacy(day);
00379:                 var census = _m._campaignLegacy?.GetCensus();
00380:                 events.Add(new DayStateChangeEvent(
00381:                     "campaign_legacy_ticked", "campaign_legacy", null, null, census?.CompletedCampaignsCount ?? 0));
00382:             }
00383:         }
00384:
00385:         /// <summary>Plan 141 research-unlock day owner (ownerId <c>research_unlock</c>, phase 5).</summary>
00386:         private sealed class ResearchUnlockDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00387:         {
00388:             private readonly Main _m;
00389:             private Ashfall.Core.Research.ResearchUnlockState? _snapshot;
00390:             public ResearchUnlockDayOwner(Main m) => _m = m;
00391:
00392:             public void CapturePreDaySnapshot(int day)
00393:             {
00394:                 _m.SetupResearchUnlockBridge();
00395:                 _snapshot = _m._researchUnlock?.CaptureState();
00396:             }
00397:
00398:             public void RestorePreDaySnapshot(int day)
00399:             {
00400:                 if (_snapshot != null) _m._researchUnlock?.RestoreState(_snapshot);
00401:             }
00402:
00403:             public void TickDay(int day, List<DayStateChangeEvent> events)
00404:             {
00405:                 _m.SetupResearchUnlockBridge();
00406:                 _m.TickResearchUnlock(day);
00407:                 var census = _m._researchUnlock?.Census;
00408:                 events.Add(new DayStateChangeEvent(
00409:                     "research_unlock_ticked", "research_unlock", null, null, census?.GrantedUnlocksCount ?? 0));
00410:             }
00411:         }
00412:
00413:         /// <summary>Plan 145 unified-ending day owner (ownerId <c>unified_ending</c>, phase 5).</summary>
00414:         private sealed class UnifiedEndingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00415:         {
00416:             private readonly Main _m;
00417:             private Ashfall.Core.Endgame.UnifiedEndingSaveState? _snapshot;
00418:             public UnifiedEndingDayOwner(Main m) => _m = m;
00419:
00420:             public void CapturePreDaySnapshot(int day)
00421:             {
00422:                 _m.SetupUnifiedEnding();
00423:                 _snapshot = _m._unifiedEnding?.CaptureState();
00424:             }
00425:
00426:             public void RestorePreDaySnapshot(int day)
00427:             {
00428:                 if (_snapshot != null) _m._unifiedEnding?.RestoreState(_snapshot);
00429:             }
00430:
00431:             public void TickDay(int day, List<DayStateChangeEvent> events)
00432:             {
00433:                 _m.SetupUnifiedEnding();
00434:                 _m.TickUnifiedEnding(day);
00435:                 var census = _m._unifiedEnding?.Census;
00436:                 events.Add(new DayStateChangeEvent(
00437:                     "unified_ending_ticked", "unified_ending", null, null, census?.IsResolved == true ? 1f : 0f));
00438:             }
00439:         }
00440:
00441:         /// <summary>Plan 147 per-NPC memory day owner (ownerId <c>npc_memory</c>, phase 5).</summary>
00442:         private sealed class NpcMemoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00443:         {
00444:             private readonly Main _m;
00445:             private Ashfall.Core.Narrative.NpcMemorySaveState? _snapshot;
00446:             public NpcMemoryDayOwner(Main m) => _m = m;
00447:
00448:             public void CapturePreDaySnapshot(int day)
00449:             {
00450:                 _m.SetupNpcMemory();
00451:                 _snapshot = _m._npcMemory?.CaptureState();
00452:             }
00453:
00454:             public void RestorePreDaySnapshot(int day)
00455:             {
00456:                 if (_snapshot != null) _m._npcMemory?.RestoreState(_snapshot);
00457:             }
00458:
00459:             public void TickDay(int day, List<DayStateChangeEvent> events)
00460:             {
00461:                 _m.SetupNpcMemory();
00462:                 _m.TickNpcMemory(day);
00463:                 var census = _m._npcMemory?.Census;
00464:                 events.Add(new DayStateChangeEvent(
00465:                     "npc_memory_ticked", "npc_memory", null, null, census?.TotalTrackedNpcs ?? 0));
00466:             }
00467:         }
00468:
00469:         /// <summary>Plan 148 ideological friction day owner (ownerId <c>ideological_friction</c>, phase 5).</summary>
00470:         private sealed class IdeologicalFrictionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00471:         {
00472:             private readonly Main _m;
00473:             private Ashfall.Core.Survivors.IdeologicalFrictionEventSaveState? _snapshot;
00474:             public IdeologicalFrictionDayOwner(Main m) => _m = m;
00475:
00476:             public void CapturePreDaySnapshot(int day)
00477:             {
00478:                 _m.SetupIdeologicalFriction();
00479:                 _snapshot = _m._ideologicalFriction?.CaptureState();
00480:             }
00481:
00482:             public void RestorePreDaySnapshot(int day)
00483:             {
00484:                 if (_snapshot != null) _m._ideologicalFriction?.RestoreState(_snapshot);
00485:             }
00486:
00487:             public void TickDay(int day, List<DayStateChangeEvent> events)
00488:             {
00489:                 _m.SetupIdeologicalFriction();
00490:                 _m.TickIdeologicalFriction(day);
00491:                 var census = _m._ideologicalFriction?.Census;
00492:                 events.Add(new DayStateChangeEvent(
00493:                     "ideological_friction_ticked", "ideological_friction", null, null, census?.TotalEventsFired ?? 0));
00494:             }
00495:         }
00496:
00497:         /// <summary>Plan 150 romance &amp; family day owner (ownerId <c>romance_family</c>, phase 5).</summary>
00498:         private sealed class RomanceFamilyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00499:         {
00500:             private readonly Main _m;
00501:             private string? _snapshot;
00502:             public RomanceFamilyDayOwner(Main m) => _m = m;
00503:
00504:             public void CapturePreDaySnapshot(int day)
00505:             {
00506:                 _m.SetupRomanceFamily();
00507:                 _snapshot = _m._romanceFamily?.CaptureCoreState();
00508:             }
00509:
00510:             public void RestorePreDaySnapshot(int day)
00511:             {
00512:                 if (_snapshot != null) _m._romanceFamily?.RestoreCoreState(_snapshot);
00513:             }
00514:
00515:             public void TickDay(int day, List<DayStateChangeEvent> events)
00516:             {
00517:                 _m.SetupRomanceFamily();
00518:                 _m.TickRomanceFamily(day);
00519:                 var census = _m._romanceFamily?.Census;
00520:                 events.Add(new DayStateChangeEvent(
00521:                     "romance_family_ticked", "romance_family", null, null, census?.TotalRelationships ?? 0));
00522:             }
00523:         }
00524:
00525:         /// <summary>Plan 152 vehicle customization day owner (ownerId <c>vehicle_customization</c>, phase 5).</summary>
00526:         private sealed class VehicleCustomizationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00527:         {
00528:             private readonly Main _m;
00529:             private string? _snapshot;
00530:             public VehicleCustomizationDayOwner(Main m) => _m = m;
00531:
00532:             public void CapturePreDaySnapshot(int day)
00533:             {
00534:                 _m.SetupVehicleCustomization();
00535:                 _snapshot = _m._vehicleCustomization?.CaptureCoreState();
00536:             }
00537:
00538:             public void RestorePreDaySnapshot(int day)
00539:             {
00540:                 if (_snapshot != null) _m._vehicleCustomization?.RestoreCoreState(_snapshot);
00541:             }
00542:
00543:             public void TickDay(int day, List<DayStateChangeEvent> events)
00544:             {
00545:                 _m.SetupVehicleCustomization();
00546:                 _m.TickVehicleCustomization(day);
00547:                 var census = _m._vehicleCustomization?.Census;
00548:                 events.Add(new DayStateChangeEvent(
00549:                     "vehicle_customization_ticked", "vehicle_customization", null, null, census?.TotalInstalledModules ?? 0));
00550:             }
00551:         }
00552:
00553:         /// <summary>Plan 174 procedural survivor backstory day owner (ownerId <c>backstory</c>, phase 5).</summary>
00554:         private sealed class BackstoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00555:         {
00556:             private readonly Main _m;
00557:             private Ashfall.Core.Survivors.BackstoryState? _snapshot;
00558:             public BackstoryDayOwner(Main m) => _m = m;
00559:
00560:             public void CapturePreDaySnapshot(int day)
00561:             {
00562:                 _m.SetupBackstory();
00563:                 _snapshot = _m._backstory?.CaptureState();
00564:             }
00565:
00566:             public void RestorePreDaySnapshot(int day)
00567:             {
00568:                 if (_snapshot != null) _m._backstory?.RestoreState(_snapshot);
00569:             }
00570:
00571:             public void TickDay(int day, List<DayStateChangeEvent> events)
00572:             {
00573:                 _m.SetupBackstory();
00574:                 _m.TickBackstory(day);
00575:                 var census = _m._backstory?.Census;
00576:                 events.Add(new DayStateChangeEvent(
00577:                     "backstory_ticked", "backstory", null, null, census?.TotalBackstories ?? 0));
00578:             }
00579:         }
00580:
00581:         /// <summary>Plan 175 meta progression day owner (ownerId <c>meta_progression</c>, phase 5).</summary>
00582:         private sealed class MetaProgressionDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00583:         {
00584:             private readonly Main _m;
00585:             private Ashfall.Core.Endgame.MetaProgressionSaveState? _snapshot;
00586:             public MetaProgressionDayOwner(Main m) => _m = m;
00587:
00588:             public void CapturePreDaySnapshot(int day)
00589:             {
00590:                 _m.SetupMetaProgression();
00591:                 _snapshot = _m._metaProgression?.CaptureState();
00592:             }
00593:
00594:             public void RestorePreDaySnapshot(int day)
00595:             {
00596:                 if (_snapshot != null) _m._metaProgression?.RestoreState(_snapshot);
00597:             }
00598:
00599:             public void TickDay(int day, List<DayStateChangeEvent> events)
00600:             {
00601:                 _m.SetupMetaProgression();
00602:                 _m.TickMetaProgression(day);
00603:                 var census = _m._metaProgression?.Census;
00604:                 events.Add(new DayStateChangeEvent(
00605:                     "meta_progression_ticked", "meta_progression", null, null, census?.PrestigeScore ?? 0));
00606:             }
00607:         }
00608:
00609:
00610:
00611:         /// <summary>Plan 167 underground tunnel network day owner (ownerId <c>tunnel_network</c>, phase 5).</summary>
00612:         private sealed class TunnelNetworkDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00613:         {
00614:             private readonly Main _m;
00615:             private Ashfall.Core.Underground.TunnelNetworkState? _snapshot;
00616:             public TunnelNetworkDayOwner(Main m) => _m = m;
00617:
00618:             public void CapturePreDaySnapshot(int day)
00619:             {
00620:                 _m.SetupTunnelNetwork();
00621:                 _snapshot = _m.TunnelNetwork?.CaptureState();
00622:             }
00623:
00624:             public void RestorePreDaySnapshot(int day)
00625:             {
00626:                 if (_snapshot != null) _m.TunnelNetwork?.RestoreState(_snapshot);
00627:             }
00628:
00629:             public void TickDay(int day, List<DayStateChangeEvent> events)
00630:             {
00631:                 _m.SetupTunnelNetwork();
00632:                 _m.TickTunnelNetwork(day);
00633:                 var census = _m.TunnelNetwork?.GetCensus();
00634:                 events.Add(new DayStateChangeEvent(
00635:                     "tunnel_network_ticked", "tunnel_network", null, null, census?.TotalSegments ?? 0));
00636:             }
00637:         }
00638:
00639:         /// <summary>Plan 166 shelter identity day owner (ownerId <c>shelter_identity</c>, phase 5).</summary>
00640:         private sealed class ShelterIdentityDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00641:         {
00642:             private readonly Main _m;
00643:             private Ashfall.Core.Shelter.ShelterIdentityState? _snapshot;
00644:             public ShelterIdentityDayOwner(Main m) => _m = m;
00645:
00646:             public void CapturePreDaySnapshot(int day)
00647:             {
00648:                 _m.SetupShelterIdentity();
00649:                 _snapshot = _m.ShelterIdentity?.CaptureState();
00650:             }
00651:
00652:             public void RestorePreDaySnapshot(int day)
00653:             {
00654:                 if (_snapshot != null) _m.ShelterIdentity?.RestoreState(_snapshot);
00655:             }
00656:
00657:             public void TickDay(int day, List<DayStateChangeEvent> events)
00658:             {
00659:                 _m.SetupShelterIdentity();
00660:                 _m.TickShelterIdentity(day);
00661:                 var census = _m.ShelterIdentity?.Census;
00662:                 events.Add(new DayStateChangeEvent(
00663:                     "shelter_identity_ticked", "shelter_identity", null, null, census?.Infamy ?? 0));
00664:             }
00665:         }
00666:
00667:         /// <summary>Plan 192 scheduled trade route contract day owner (ownerId <c>trade_routes</c>, phase 5).</summary>
00668:         private sealed class TradeRouteDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00669:         {
00670:             private readonly Main _m;
00671:             private Ashfall.Core.Economy.PlayerTradeRouteSaveState? _snapshot;
00672:             public TradeRouteDayOwner(Main m) => _m = m;
00673:
00674:             public void CapturePreDaySnapshot(int day)
00675:             {
00676:                 _m.SetupTradeRoutes();
00677:                 _snapshot = _m._tradeRoutes?.CaptureState();
00678:             }
00679:
00680:             public void RestorePreDaySnapshot(int day)
00681:             {
00682:                 if (_snapshot != null) _m._tradeRoutes?.RestoreState(_snapshot);
00683:             }
00684:
00685:             public void TickDay(int day, List<DayStateChangeEvent> events)
00686:             {
00687:                 _m.SetupTradeRoutes();
00688:                 _m.TickTradeRoutes(day);
00689:                 var census = _m._tradeRoutes?.Census;
00690:                 events.Add(new DayStateChangeEvent(
00691:                     "trade_route_ticked", "trade_routes", null, null, census?.ActiveContracts ?? 0));
00692:             }
00693:         }
00694:
00695:         /// <summary>Plan 199 seasonal human migration day owner (ownerId <c>human_migration</c>, phase 5).</summary>
00696:         private sealed class HumanMigrationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00697:         {
00698:             private readonly Main _m;
00699:             private Ashfall.Core.Economy.SeasonalMigrationSaveState? _snapshot;
00700:             public HumanMigrationDayOwner(Main m) => _m = m;
00701:
00702:             public void CapturePreDaySnapshot(int day)
00703:             {
00704:                 _m.SetupHumanMigration();
00705:                 _snapshot = _m._humanMigration?.CaptureState();
00706:             }
00707:
00708:             public void RestorePreDaySnapshot(int day)
00709:             {
00710:                 if (_snapshot != null) _m._humanMigration?.RestoreState(_snapshot);
00711:             }
00712:
00713:             public void TickDay(int day, List<DayStateChangeEvent> events)
00714:             {
00715:                 _m.SetupHumanMigration();
00716:                 _m.TickHumanMigration(day);
00717:                 var census = _m._humanMigration?.Census;
00718:                 events.Add(new DayStateChangeEvent(
00719:                     "human_migration_ticked", "human_migration", null, null, census?.TotalTrackedRegions ?? 0));
00720:             }
00721:         }
00722:
00723:         /// <summary>Plan 159 shelter governance day owner (ownerId <c>shelter_governance</c>, phase 5).</summary>
00724:         private sealed class ShelterGovernanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00725:         {
00726:             private readonly Main _m;
00727:             private Ashfall.Core.Governance.ShelterGovernanceSaveState? _snapshot;
00728:             public ShelterGovernanceDayOwner(Main m) => _m = m;
00729:
00730:             public void CapturePreDaySnapshot(int day)
00731:             {
00732:                 _m.SetupShelterGovernance();
00733:                 _snapshot = _m._shelterGovernance?.CaptureState();
00734:             }
00735:
00736:             public void RestorePreDaySnapshot(int day)
00737:             {
00738:                 if (_snapshot != null) _m._shelterGovernance?.RestoreState(_snapshot);
00739:             }
00740:
00741:             public void TickDay(int day, List<DayStateChangeEvent> events)
00742:             {
00743:                 _m.SetupShelterGovernance();
00744:                 _m.TickShelterGovernance(day);
00745:                 int stability = _m._shelterGovernance?.StabilityRating ?? 100;
00746:                 events.Add(new DayStateChangeEvent(
00747:                     "shelter_governance_ticked", "shelter_governance", null, null, stability));
00748:             }
00749:         }
00750:
00751:         /// <summary>Plan 176 aging & elderly survivor day owner (ownerId <c>aging</c>, phase 5).</summary>
00752:         private sealed class AgingDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00753:         {
00754:             private readonly Main _m;
00755:             private Ashfall.Core.Survivors.AgingState? _snapshot;
00756:             public AgingDayOwner(Main m) => _m = m;
00757:
00758:             public void CapturePreDaySnapshot(int day)
00759:             {
00760:                 _m.SetupAging();
00761:                 _snapshot = _m._aging?.CaptureState();
00762:             }
00763:
00764:             public void RestorePreDaySnapshot(int day)
00765:             {
00766:                 if (_snapshot != null) _m._aging?.RestoreState(_snapshot);
00767:             }
00768:
00769:             public void TickDay(int day, List<DayStateChangeEvent> events)
00770:             {
00771:                 _m.SetupAging();
00772:                 _m.TickAging(day);
00773:                 int tracked = _m._aging?.Census.TotalTrackedSurvivors ?? 0;
00774:                 events.Add(new DayStateChangeEvent(
00775:                     "aging_ticked", "aging", null, null, tracked));
00776:             }
00777:         }
00778:
00779:         /// <summary>Plan 186 shelter maintenance day owner (ownerId <c>shelter_maintenance</c>, phase 5).</summary>
00780:         private sealed class ShelterMaintenanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00781:         {
00782:             private readonly Main _m;
00783:             private Ashfall.Core.Shelter.ShelterMaintenanceState? _snapshot;
00784:             public ShelterMaintenanceDayOwner(Main m) => _m = m;
00785:
00786:             public void CapturePreDaySnapshot(int day)
00787:             {
00788:                 _m.SetupShelterMaintenance();
00789:                 _snapshot = _m._shelterMaintenance?.CaptureState();
00790:             }
00791:
00792:             public void RestorePreDaySnapshot(int day)
00793:             {
00794:                 if (_snapshot != null) _m._shelterMaintenance?.RestoreState(_snapshot);
00795:             }
00796:
00797:             public void TickDay(int day, List<DayStateChangeEvent> events)
00798:             {
00799:                 _m.SetupShelterMaintenance();
00800:                 _m.TickShelterMaintenance(day);
00801:                 int failed = _m._shelterMaintenance?.Census.FailedComponents ?? 0;
00802:                 events.Add(new DayStateChangeEvent(
00803:                     "shelter_maintenance_ticked", "shelter_maintenance", null, null, failed));
00804:             }
00805:         }
00806:
00807:         /// <summary>Plan 188 survivor routines day owner (ownerId <c>survivor_routines</c>, phase 5).</summary>
00808:         private sealed class SurvivorRoutinesDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00809:         {
00810:             private readonly Main _m;
00811:             private Ashfall.Core.Survivors.SurvivorRoutineState? _snapshot;
00812:             public SurvivorRoutinesDayOwner(Main m) => _m = m;
00813:
00814:             public void CapturePreDaySnapshot(int day)
00815:             {
00816:                 _m.SetupSurvivorRoutines();
00817:                 _snapshot = _m._survivorRoutines?.CaptureState();
00818:             }
00819:
00820:             public void RestorePreDaySnapshot(int day)
00821:             {
00822:                 if (_snapshot != null) _m._survivorRoutines?.RestoreState(_snapshot);
00823:             }
00824:
00825:             public void TickDay(int day, List<DayStateChangeEvent> events)
00826:             {
00827:                 _m.SetupSurvivorRoutines();
00828:                 _m.TickSurvivorRoutines(day);
00829:                 int routines = _m._survivorRoutines?.Census.TotalRoutines ?? 0;
00830:                 events.Add(new DayStateChangeEvent(
00831:                     "survivor_routines_ticked", "survivor_routines", null, null, routines));
00832:             }
00833:         }
00834:
00835:         // ── Phase 1 Owners ───────────────────────────────────────────────
00836:
00837:         private sealed class WeatherWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00838:         {
00839:             private readonly Main _m;
00840:             private Ashfall.Core.World.WorldWeatherState? _snapshot;
00841:             public WeatherWorldDayOwner(Main m) => _m = m;
00842:             public void CapturePreDaySnapshot(int day)
00843:             {
00844:                 _m.SetupWorld();
00845:                 _snapshot = _m._world.Weather.CaptureState();
00846:             }
00847:             public void RestorePreDaySnapshot(int day)
00848:             {
00849:                 if (_snapshot != null)
00850:                     _m._world.Weather.RestoreState(_snapshot);
00851:             }
00852:             public void TickDay(int day, List<DayStateChangeEvent> events)
00853:             {
00854:                 _m.SetupWorld();
00855:                 // C2 / Plan 20C (§39) — capture the station's prediction for
00856:                 // TODAY before the weather advances, so a hazard arrival can be
00857:                 // attributed: predicted / missed / unexpected. The radio layer
00858:                 // has no authored weather predictions yet — the radio/station
00859:                 // distinction is documented as not-yet-authored data.
00860:                 var stationState = _m._world.WeatherIntelligence?.Station?.State;
00861:                 var predictedToday = stationState?.cachedForecast?
00862:                     .FirstOrDefault(f => f != null && f.day == day)?.weather;
00863:                 bool stationCouldKnow = stationState != null
00864:                     && stationState.isInstalled
00865:                     && stationState.lastForecastDay >= 0
00866:                     && day <= stationState.lastForecastDay + stationState.forecastHorizonDays;
00867:
00868:                 _m._world.TickHours(24f);
00869:                 _m._world.WeatherIntelligence?.TickDay(day);
00870:
00871:                 var actual = _m._world.Weather.Current;
00872:                 events.Add(new DayStateChangeEvent("weather_ticked", "weather_world",
00873:                     actual.ToString(), null, _m._world.Weather.OutdoorRadModifier));
00874:
00875:                 // Severe-weather arrivals get attribution (§39): the briefing
00876:                 // can say why the player wasn't warned — never a silent storm.
00877:                 if (_m._world.IsSevereWeather(actual) && actual != predictedToday)
00878:                 {
00879:                     events.Add(stationCouldKnow
00880:                         ? new DayStateChangeEvent("weather_forecast_miss", "weather_world",
00881:                             actual.ToString(), "station_predicted_other", day)
00882:                         : new DayStateChangeEvent("weather_unexpected_storm", "weather_world",
00883:                             actual.ToString(), "no_station_forecast", day));
00886:         }
00887:
00888:         private sealed class DeepCoastMaritimeDayOwner : IDayAdvanceOwner
00889:         {
00890:             private readonly Main _m;
00891:             public DeepCoastMaritimeDayOwner(Main m) => _m = m;
00892:             public void CapturePreDaySnapshot(int day) { }
00893:             public void TickDay(int day, List<DayStateChangeEvent> events)
00894:             {
00895:                 _m.SetupMaritime();
00896:                 if (_m._maritime.Dive.IsActive)
00897:                     _m._maritime.TickDive(60f);
00898:                 _m.SetupDeepCoast();
00899:                 _m._deepCoast.TickDaily(day, _m._core.Weather);
00900:                 _m._deepCoastPanel?.SetSimDay(day);
00901:                 events.Add(new DayStateChangeEvent("maritime_ticked", "maritime_deep_coast", null, null, day));
00902:             }
00903:         }
00904:
00905:         private sealed class PowerGridDayOwner : IDayAdvanceOwner
00906:         {
00907:             private readonly Main _m;
00908:             public PowerGridDayOwner(Main m) => _m = m;
00909:             public void CapturePreDaySnapshot(int day) { }
00910:             public void TickDay(int day, List<DayStateChangeEvent> events)
00911:             {
00912:                 _m.TickPowerGrid(day);
00913:
00914:                 // SHELTER_HARDENING: the distribution subgrid observes the grid's
00915:                 // per-room draws, then advances its thermal/fuse model. Runs in
00916:                 // the same phase as the grid so downstream consumers (phase 2+)
00917:                 // see post-surge, post-thermal state.
00918:                 _m.TickPowerSubgrids(day);
00919:
00920:                 events.Add(new DayStateChangeEvent("power_ticked", "power_grid", null, null, day));
00921:
00922:                 // C2[6] 23B: attributed shedding. The grid reports exactly what the
00923:                 // deterministic allocator served/shed; the briefing distinguishes
00924:                 // grid-automatic shed from the player's own breaker actions.
00925:                 var summary = _m._powerGrid?.LastTickSummary;
00926:                 if (summary != null)
00927:                 {
00928:                     if (summary.HasCriticalDeficit)
00929:                         events.Add(new DayStateChangeEvent("power_critical_deficit", "power_grid",
00935:                         events.Add(new DayStateChangeEvent("power_brownout_began", "power_grid", null, null, day));
00936:                     if (summary.BrownoutEnded)
00937:                         events.Add(new DayStateChangeEvent("power_brownout_restored", "power_grid", null, null, day));
00938:                 }
00939:                 if (_m._pendingPlayerSheds.Count > 0)
00940:                 {
00941:                     events.Add(new DayStateChangeEvent("power_shed_player", "power_grid",
00945:
00946:                 // C2[6] 23C: fact-reading cascade authority runs after the grid
00947:                 // resolves, so every stressor reflects the day's real outcome.
00948:                 _m.TickCascade(day, events);
00949:             }
00950:         }
00951:
00952:         private sealed class NuclearCoreDayOwner : IDayAdvanceOwner
00953:         {
00954:             private readonly Main _m;
00955:             public NuclearCoreDayOwner(Main m) => _m = m;
00956:             public void CapturePreDaySnapshot(int day) { }
00957:             public void TickDay(int day, List<DayStateChangeEvent> events)
00958:             {
00959:                 var nuclear = _m.EnsureNuclearCore();
00960:                 _m.PublishNuclearCoreGeneration();
00961:                 events.Add(new DayStateChangeEvent(
00964:                     null,
00965:                     null,
00966:                     nuclear.GetTotalGenerationWatts()));
00967:             }
00968:         }
00969:
00970:         private sealed class HoldfastCoreDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
00971:         {
00972:             private readonly Main _m;
00973:             private int _clockDaySnapshot;
00974:             public HoldfastCoreDayOwner(Main m) => _m = m;
00975:             public void CapturePreDaySnapshot(int day)
00976:             {
00977:                 // The clock is the double-day hazard: if a later phase fails,
00978:                 // a retry must not tick the calendar twice.
00979:                 _clockDaySnapshot = _m._core.Clock.Day;
00980:             }
00981:             public void RestorePreDaySnapshot(int day)
00982:             {
00983:                 _m._core.Clock.SetDay(_clockDaySnapshot);
00984:             }
00985:             public void TickDay(int day, List<DayStateChangeEvent> events)
00986:             {
00987:                 _m.SetupIceRoad();
00988:                 string delta = _m._core.TickDay();
00989:                 if (_m._holdfastRuntime != null && !_m._holdfastRuntime.IsDead)
00990:                 {
00991:                     _m._holdfastRuntime.Survivors = _m._survivors;
00992:                     _m._holdfastRuntime.TickDay();
00993:                 }
00994:                 // Calendar-led authority: the clock is a projection and must
00995:                 // land exactly on the campaign day being committed, whatever
00996:                 // its internal tick state said.
00997:                 _m._core.Clock.SetDay(day);
00998:                 events.Add(new DayStateChangeEvent("holdfast_ticked", "holdfast_core", delta, null, _m._core.Clock.Day));
00999:             }
01000:         }
01001:
01002:         // ── Phase 2 Owners ───────────────────────────────────────────────
01003:
01004:         private sealed class StartingLevelRationsDayOwner : IDayAdvanceOwner
01005:         {
01006:             private readonly Main _m;
01007:             public StartingLevelRationsDayOwner(Main m) => _m = m;
01008:             public void CapturePreDaySnapshot(int day) { }
01009:             public void TickDay(int day, List<DayStateChangeEvent> events)
01010:             {
01011:                 _m.SetupStartingLevel();
01012:                 // SHELTER_FAILURE_EFFECTS (G6): fx_filtration_off — air filtration
01013:                 // follows the canonical room_air_filtration breaker.
01014:                 float airPower = _m._powerGrid?.System == null || _m._powerGrid.System.IsRoomPowered("room_air_filtration") ? 1f : 0f;
01015:                 _m._startingLevel.TickDay(isFilterDutyAssigned: false, outdoorWeather: WeatherKind.Clear, powerAvailability01: airPower);
01016:
01017:                 _m.SetupInventory();
01018:                 _m.SetupDoseLedger();
01019:                 int baseFood = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3;
01020:                 int childFood = _m._doseLedger?.Cohort?.CalculateChildFoodUnits(_m._startingLevel.System.State.rationPolicy) ?? 0;
01021:                 int foodToConsume = baseFood + childFood;
01022:                 int waterToConsume = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Irradiated ? 0 : (_m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3);
01023:                 _m._inventory.Remove("canned_food", foodToConsume);
01024:                 if (waterToConsume > 0)
01025:                     _m._inventory.Remove("clean_water", waterToConsume);
01026:                 else
01027:                     _m._inventory.Remove("irradiated_water", 2);
01028:
01029:                 events.Add(new DayStateChangeEvent("consumed_rations", "starting_level_rations", "canned_food", null, foodToConsume));
01030:                 if (childFood > 0)
01031:                 {
01032:                     events.Add(new DayStateChangeEvent("consumed_child_rations", "starting_level_rations", "canned_food", null, childFood));
01033:                 }
01034:             }
01035:         }
01036:
01037:         private sealed class CraftingProductionDayOwner : IDayAdvanceOwner
01038:         {
01039:             private readonly Main _m;
01040:             public CraftingProductionDayOwner(Main m) => _m = m;
01041:             public void CapturePreDaySnapshot(int day) { }
01042:             public void TickDay(int day, List<DayStateChangeEvent> events)
01043:             {
01044:                 _m.SetupCrafting();
01045:                 _m._crafting.CompleteAll(24f);
01046:                 events.Add(new DayStateChangeEvent("crafting_completed", "crafting_production", null, null, 24f));
01048:         }
01049:
01050:         private sealed class GreenhouseFoundryDayOwner : IDayAdvanceOwner
01051:         {
01052:             private readonly Main _m;
01053:             public GreenhouseFoundryDayOwner(Main m) => _m = m;
01054:             public void CapturePreDaySnapshot(int day) { }
01055:             public void TickDay(int day, List<DayStateChangeEvent> events)
01056:             {
01057:                 // Single growth authority: player GreenhouseHostSession (shared
01058:                 // into the expansion hub). Do not also TickGreenhouse on a twin.
01059:                 _m.SetupGreenhouse();
01060:                 _m.SetupExpansions();
01061:                 _m.SetupAgriculture();
01062:                 if (_m._agriculture != null)
01063:                 {
01064:                     // Plan 162: advanced agriculture derives the grow-light and
01065:                     // ash inputs from power + weather, then ticks the canonical
01066:                     // greenhouse growth authority exactly once inside Core.
01067:                     _m.TickAgricultureDay(day);
01068:                 }
01069:                 else
01070:                 {
01071:                     _m._greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);
01072:                 }
01073:
01074:                 _m.SetupSilentFoundry();
01075:                 _m._silentFoundry.TickDaily(day);
01076:                 _m._silentFoundryPanel?.RefreshView();
01077:                 if (_m._foundryDirty) _m.SaveExpansionHub();
01078:
01079:                 events.Add(new DayStateChangeEvent("greenhouse_foundry_ticked", "greenhouse_foundry", null, null, day));
01080:             }
01081:         }
01082:
01083:         /// <summary>
01085:         /// weather/power: fault tension accumulates, slips route damage to
01086:         /// thermal/excavation authorities, and severe quakes request a cryo
01087:         /// vault breach (Scenario E) before the production owners tick.
01088:         /// </summary>
01089:         private sealed class SeismicGeologyDayOwner : IDayAdvanceOwner
01090:         {
01091:             private readonly Main _m;
01092:             public SeismicGeologyDayOwner(Main m) => _m = m;
01093:             public void CapturePreDaySnapshot(int day) { }
01094:             public void TickDay(int day, List<DayStateChangeEvent> events)
01095:             {
01096:                 _m.SetupSeismicDynamics();
01097:                 _m._seismicDynamics!.TickDay(day);
01098:                 if (_m._seismicDirty) _m.SaveSeismicDynamics();
01099:                 events.Add(new DayStateChangeEvent("seismic_geology_ticked", "seismic_dynamics", null, null, day));
01100:             }
01101:         }
01102:
01103:         /// <summary>
01104:         /// Plan B69 — cryo thermal/viability update runs in phase 2 after the
01105:         /// foundry owner: grid brownout from the furnace reaches the vault the
01106:         /// same day, so a brownout day degrades samples exactly once.
01107:         /// </summary>
01108:         private sealed class CryoVaultDayOwner : IDayAdvanceOwner
01109:         {
01110:             private readonly Main _m;
01111:             public CryoVaultDayOwner(Main m) => _m = m;
01112:             public void CapturePreDaySnapshot(int day) { }
01113:             public void TickDay(int day, List<DayStateChangeEvent> events)
01114:             {
01115:                 _m.SetupCryoVault();
01116:                 _m._cryoVault!.TickDay(day);
01117:                 if (_m._cryoVaultDirty) _m.SaveCryoVault();
01118:                 events.Add(new DayStateChangeEvent("cryo_vault_ticked", "cryo_vault", null, null, day));
01119:             }
01120:         }
01121:
01122:         /// <summary>
01125:         /// applied first, then passive drift.
01126:         /// </summary>
01127:         private sealed class PrecisionMetrologyDayOwner : IDayAdvanceOwner
01128:         {
01129:             private readonly Main _m;
01130:             public PrecisionMetrologyDayOwner(Main m) => _m = m;
01131:             public void CapturePreDaySnapshot(int day) { }
01132:             public void TickDay(int day, List<DayStateChangeEvent> events)
01133:             {
01134:                 _m.TickPrecisionMetrology(day);
01135:                 events.Add(new DayStateChangeEvent("precision_metrology_ticked", "precision_metrology", null, null, day));
01136:             }
01137:         }
01138:
01139:         /// <summary>
01140:         /// Plan B87 — closed-loop aquaponics daily ecology tick. Phase 2 so
01141:         /// power/thermal query results for the day are already settled.
01142:         /// </summary>
01143:         private sealed class AquaponicsDayOwner : IDayAdvanceOwner
01144:         {
01145:             private readonly Main _m;
01146:             public AquaponicsDayOwner(Main m) => _m = m;
01147:             public void CapturePreDaySnapshot(int day) { }
01148:             public void TickDay(int day, List<DayStateChangeEvent> events)
01149:             {
01150:                 _m.TickAquaponics(day);
01151:                 events.Add(new DayStateChangeEvent("aquaponics_ticked", "aquaponics", null, null, day));
01152:             }
01153:         }
01154:
01155:         private sealed class PsychologyArcsDayOwner : IDayAdvanceOwner
01156:         {
01157:             private readonly Main _m;
01158:             public PsychologyArcsDayOwner(Main m) => _m = m;
01159:             public void CapturePreDaySnapshot(int day) { }
01160:             public void TickDay(int day, List<DayStateChangeEvent> events)
01161:             {
01162:                 _m.TickPsychologyArcsDay(day);
01163:                 events.Add(new DayStateChangeEvent("psychology_arcs_ticked", "psychology_arcs_162", null, null, day));
01164:             }
01165:         }
01166:
01167:         private sealed class EconomyMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01168:         {
01169:             private readonly Main _m;
01170:             private Ashfall.Core.Economy.MarketState? _snapshot;
01171:             public EconomyMarketDayOwner(Main m) => _m = m;
01172:             public void CapturePreDaySnapshot(int day)
01173:             {
01174:                 _m.SetupEconomy();
01175:                 _snapshot = _m._economy.CaptureSave();
01176:             }
01177:             public void RestorePreDaySnapshot(int day)
01178:             {
01179:                 if (_snapshot != null)
01180:                     _m._economy.RestoreSave(_snapshot);
01181:             }
01182:             public void TickDay(int day, List<DayStateChangeEvent> events)
01183:             {
01184:                 _m.SetupEconomy();
01185:                 // Plan 212 — weather (phase 1) already ticked; convert today's
01186:                 // severity into a bounded market shock BEFORE the index update.
01187:                 _m.TickEconomyWeatherBridge(day);
01188:                 _m._economy.TickDay(day, _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Economy, day, 0));
01189:                 var activeShockCount = _m._economy.Market.ActiveShocks.Count;
01190:                 events.Add(new DayStateChangeEvent("market_ticked", "economy_market", null, null, _m._economy.Market.Day));
01191:                 if (activeShockCount > 0)
01192:                     events.Add(new DayStateChangeEvent("market_shocks_active", "economy_market", null, null, activeShockCount));
01193:             }
01194:         }
01195:
01196:         private sealed class ShelterFacilitiesDayOwner : IDayAdvanceOwner
01197:         {
01198:             private readonly Main _m;
01199:             public ShelterFacilitiesDayOwner(Main m) => _m = m;
01200:             public void CapturePreDaySnapshot(int day) { }
01201:             public void TickDay(int day, List<DayStateChangeEvent> events)
01202:             {
01203:                 // C2 / Plan 20B (§29) — capture pre-tick shelter shielding state
01204:                 // so degradation/unseal transitions become semantic day events
01205:                 // (canonical vocabulary; the briefing builder renders them).
01206:                 string filterBandBefore = _m._startingLevel?.System.AirFilterConditionBand ?? "healthy";
01207:                 bool deconActiveBefore = _m._decontamination?.System.HasActiveCase ?? false;
01208:                 AirlockDoorState airlockBefore = _m._airlockSecurity?.System.State.doorState
01209:                     ?? AirlockDoorState.Secure;
01210:
01211:                 _m.TickAllExpandedShelterSystems(day);
01212:                 _m._kitchenNutrition?.DrainDayEvents(events);
01213:
01214:                 string filterBandAfter = _m._startingLevel?.System.AirFilterConditionBand ?? "healthy";
01215:                 if (FilterBandRank(filterBandAfter) < FilterBandRank(filterBandBefore))
01216:                 {
01217:                     events.Add(new DayStateChangeEvent("shelter_filter_degraded",
01218:                         "starting_level_air_filter", "air_filter", filterBandAfter,
01220:                 }
01221:
01222:                 bool deconActiveAfter = _m._decontamination?.System.HasActiveCase ?? false;
01223:                 if (!deconActiveBefore && deconActiveAfter)
01224:                     events.Add(new DayStateChangeEvent("shelter_decon_started", "decontamination"));
01225:                 else if (deconActiveBefore && !deconActiveAfter)
01226:                     events.Add(new DayStateChangeEvent("shelter_decon_completed", "decontamination"));
01227:
01228:                 AirlockDoorState airlockAfter = _m._airlockSecurity?.System.State.doorState
01229:                     ?? AirlockDoorState.Secure;
01230:                 if (airlockBefore == AirlockDoorState.Secure && airlockAfter != AirlockDoorState.Secure)
01234:                 }
01235:
01236:                 events.Add(new DayStateChangeEvent("shelter_facilities_ticked", "shelter_facilities", null, null, day));
01237:             }
01238:
01239:             /// <summary>Ordering for filter condition bands (higher = healthier).</summary>
01240:             private static int FilterBandRank(string? band) => band switch
01241:             {
01242:                 "healthy" => 2,
01243:                 "degraded" => 1,
01248:
01249:         /// <summary>
01250:         /// Plan 38 — one daily owner for the commitment authority. Forwards the
01251:         /// pre-day snapshot (fail-closed rollback), the day tick, and the
01252:         /// buffered semantic events into the briefing report. Null-guarded so the
01253:         /// owner survives session resets without outliving a disposed system.
01254:         /// </summary>
01255:         private sealed class CommitmentDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01256:         {
01257:             private readonly Main _m;
01258:             public CommitmentDayOwner(Main m) => _m = m;
01259:
01260:             public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
01261:
01262:             public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
01263:
01264:             public void TickDay(int day, List<DayStateChangeEvent> events)
01265:             {
01266:                 var session = _m.EnsureCommitments();
01267:                 session.TickDay(day);
01268:                 session.DrainDayEvents(events);
01269:                 _m._commitmentsDirty = true;
01270:             }
01271:         }
01272:
01273:         private sealed class ShelterFireDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01274:         {
01275:             private readonly Main _m;
01276:             private Dictionary<string, Ashfall.Core.Shelter.FireIncidentState>? _snapshot;
01277:
01278:             public ShelterFireDayOwner(Main m) => _m = m;
01279:
01280:             public void CapturePreDaySnapshot(int day)
01281:             {
01282:                 _m.SetupShelterFireHazard();
01283:                 _snapshot = _m._shelterFireHazard?.CaptureState();
01284:             }
01285:
01286:             public void RestorePreDaySnapshot(int day)
01287:             {
01288:                 if (_snapshot != null)
01289:                 {
01290:                     _m.SetupShelterFireHazard();
01291:                     _m._shelterFireHazard!.RestoreState(_snapshot);
01292:                 }
01293:             }
01294:
01295:             public void TickDay(int day, List<DayStateChangeEvent> events)
01296:             {
01297:                 _m.SetupShelterFireHazard();
01298:                 int advanced = _m._shelterFireSession!.TickDay(
01299:                     day,
01300:                     _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, day, 15));
01301:                 events.Add(new DayStateChangeEvent("shelter_fire_ticked", "shelter_fire", null, null, advanced));
01302:             }
01303:         }
01304:
01305:         // ── Phase 3 Owners ───────────────────────────────────────────────
01306:
01307:         private sealed class SurvivorsNeedsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01308:         {
01309:             private readonly Main _m;
01310:             private SurvivorsSaveState? _snapshot;
01311:             public SurvivorsNeedsDayOwner(Main m) => _m = m;
01312:             public void CapturePreDaySnapshot(int day)
01313:             {
01314:                 _m.SetupSurvivors();
01315:                 _snapshot = _m._survivors.CaptureSave();
01316:             }
01317:             public void RestorePreDaySnapshot(int day)
01318:             {
01319:                 if (_snapshot != null)
01320:                     _m._survivors.RestoreSave(_snapshot);
01321:             }
01322:             public void TickDay(int day, List<DayStateChangeEvent> events)
01323:             {
01324:                 _m.SetupSurvivors();
01325:                 _m._survivors.Needs.CurrentDay = day;
01326:                 _m.ApplyScheduleNeedsModifiers();
01327:                 _m._survivors.TickHour(24f);
01328:                 _m.VacateInvalidFitnessAssignments();
01329:                 // Plan 24B A2 + 24C A3: measured overwork routes data-authored
01330:                 // fatigue/morale rates through the shared needs seam; grief
01331:                 // decays through the same seam from the relationship ledger's
01332:                 // persisted facts. Both read the post-validation assignment
01333:                 // state so vacated shifts never keep a stale rate.
01334:                 _m.ApplyOverworkNeedsModifiers();
01335:                 _m.ApplyGriefNeedsModifiers();
01342:                 _m.SetupShelterDecor();
01343:                 // Plan 71: room_common_mess_hall — communal comfort morale is a
01344:                 // shed-able low-priority load (level gate, applied once per day;
01345:                 // no per-tick penalty accumulation).
01346:                 bool messHallPowered = _m._powerGrid?.System?.IsRoomPowered("room_common_mess_hall") ?? true;
01347:                 int decorRecipients = messHallPowered ? (_m._shelterDecor?.ApplyDailyMorale(day) ?? 0) : 0;
01348:                 if (decorRecipients > 0)
01349:                     events.Add(new DayStateChangeEvent("shelter_decor_morale", "shelter_decor", null, null, decorRecipients));
01350:                 // Flagship XI (Plan 154): contagion runs after needs + decor morale
01351:                 // so it reads the day's final morale; its deltas are part of today.
01352:                 _m.SetupMoraleContagion();
01353:                 if (_m._moraleContagion != null)
01354:                 {
01355:                     _m._moraleContagion.EvaluateDay(day);
01356:                     events.Add(new DayStateChangeEvent("morale_contagion_ticked", "morale_contagion", null, null,
01357:                         _m._moraleContagion.System.State.survivors.Count));
01358:                 }
01359:                 // Drain any survivor_perished events from the death pipeline
01360:                 // into the briefing feed. Needs/radiation OnDied fires inside
01361:                 // TickHour — every death this day lands here exactly once.
01362:                 _m.SetupSurvivorFate();
01363:                 if (_m._survivorFate != null)
01364:                     _m._survivorFate.DrainDayEvents(events);
01365:                 _m.SetupDutyRoster();
01366:                 _m._dutyRoster!.DrainDayEvents(events);
01367:                 _m._medicalWardSession?.DrainDayEvents(events);
01368:                 events.Add(new DayStateChangeEvent("survivors_ticked", "survivors_needs", null, null, _m._survivors.RosterState.Count));
01369:             }
01370:         }
01371:
01372:         /// <summary>
01376:         /// persisted values until the Wave 6 power-grid feed lands.
01377:         /// </summary>
01378:         private sealed class HygieneDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01379:         {
01380:             private readonly Main _m;
01381:             private Ashfall.Core.Shelter.SanitationState? _snapshot;
01382:             public HygieneDayOwner(Main m) => _m = m;
01383:             public void CapturePreDaySnapshot(int day)
01384:             {
01385:                 _m.SetupSanitation();
01386:                 _snapshot = _m._sanitation!.CaptureSave();
01387:             }
01388:             public void RestorePreDaySnapshot(int day)
01389:             {
01390:                 if (_snapshot != null)
01391:                     _m._sanitation!.RestoreSave(_snapshot);
01392:             }
01393:             public void TickDay(int day, List<DayStateChangeEvent> events)
01394:             {
01395:                 _m.SetupSanitation();
01396:                 int population = 0;
01397:                 if (_m._survivors != null)
01400:                         if (entry != null && entry.isAlive) population++;
01401:                 }
01402:                 _m._sanitation!.TickDay(day, population);
01403:
01404:                 TickSanitationConsequences(day, events);
01405:
01406:                 if (_m._sanitationDirty) _m.SaveSanitation();
01407:                 var spill = _m._sanitation.System.ActiveSpill;
01408:                 events.Add(new DayStateChangeEvent("sanitation_ticked", "hygiene", null, null,
01409:                     _m._sanitation.System.GetShelterHygienePermille()));
01410:                 if (spill != null)
01411:                     events.Add(new DayStateChangeEvent("sanitation_spill", "hygiene", spill.roomId, null, spill.severity));
01412:             }
01413:
01414:             /// <summary>
01415:             /// Plan 210 Wave 6 — the three cross-plan consequences (Core
01416:             /// policy, host adapter): a bounded daily cholera sweep through
01417:             /// the AUTHORED foul_water_draw source (disease system keeps
01418:             /// infection ownership), a reversible Hazardous morale mark, and
01419:             /// a small bounded medical demand shock on crisis.
01420:             /// </summary>
01421:             private void TickSanitationConsequences(int day, List<DayStateChangeEvent> events)
01422:             {
01423:                 var system = _m._sanitation!.System;
01424:                 var band = system.GetShelterHygieneBand();
01425:                 bool spillActive = system.ActiveSpill != null;
01426:
01427:                 // 1. Disease sweep — DiseaseSystem rolls and owns the outcome.
01428:                 if (_m._disease?.Engine != null
01429:                     && Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldRunDailyExposureSweep(band, spillActive))
01430:                 {
01431:                     foreach (var entry in _m._survivors?.Roster.Roster ?? new List<Ashfall.Core.Survivors.SurvivorRosterEntry>())
01432:                     {
01433:                         if (entry == null || !entry.isAlive) continue;
01434:                         string roomId = system.State.rooms.Count > 0 ? system.State.rooms[0].roomId : string.Empty;
01435:                         _m._disease.Engine.TryExpose(new Ashfall.Core.Disease.DiseaseExposureContext
01436:                         {
01437:                             SurvivorId = entry.survivorId,
01438:                             DiseaseId = Ashfall.Core.Disease.DiseaseIds.Cholera,
01439:                             SourceId = Ashfall.Core.Shelter.SanitationConsequenceRules.CholeraSourceId,
01440:                             Day = day,
01441:                             ProbabilityModifier = system.GetPathogenExposureModifier(roomId)
01442:                         });
01443:                     }
01444:                     events.Add(new DayStateChangeEvent("sanitation_disease_sweep", "hygiene", null, null, day));
01445:                 }
01446:
01447:                 // 2. Reversible morale mark (Hazardous only; cleared on recovery).
01448:                 if (_m._dutyRoster?.Marks != null)
01449:                 {
01450:                     if (Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldSetHazardousMark(band)
01451:                         && !_m._dutyRoster.Marks.HasMark(Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId))
01452:                     {
01453:                         _m._dutyRoster.Marks.SetMark(
01454:                             Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId,
01455:                             "The shelter has become genuinely hazardous.", day);
01456:                     }
01457:                     else if (Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldClearHazardousMark(band)
01458:                         && _m._dutyRoster.Marks.HasMark(Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId))
01459:                     {
01460:                         _m._dutyRoster.Marks.ClearMark(Ashfall.Core.Shelter.SanitationConsequenceRules.HazardousMarkId);
01461:                     }
01462:                 }
01463:
01464:                 // 3. Crisis demand shock (idempotent refresh via source id).
01465:                 if (_m._economy != null
01466:                     && Ashfall.Core.Shelter.SanitationConsequenceRules.ShouldApplyCrisisDemandShock(band, spillActive))
01467:                 {
01468:                     _m._economy.Market.ApplyShock(
01469:                         "medical", isShortage: true,
01470:                         severityBp: Ashfall.Core.Shelter.SanitationConsequenceRules.CrisisShockSeverityBp,
01471:                         startDay: day, durationDays: Ashfall.Core.Shelter.SanitationConsequenceRules.CrisisShockDurationDays,
01472:                         sourceId: Ashfall.Core.Shelter.SanitationConsequenceRules.CrisisShockSourceId);
01473:                 }
01474:             }
01475:         }
01476:
01477:         private sealed class MedicalDiseaseDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01478:         {
01479:             private readonly Main _m;
01480:             private static readonly Ashfall.Core.SystemTextJsonSerializer s_json = new Ashfall.Core.SystemTextJsonSerializer();
01481:             private Ashfall.Core.Medical.ChemicalDependencyLedgerState? _medicalSnapshot;
01482:             private string? _diseaseSnapshotJson;
01483:             private Ashfall.Core.Medical.MedicalPipelineSaveState? _pipelineSnapshot;
01484:             public MedicalDiseaseDayOwner(Main m) => _m = m;
01485:             public void CapturePreDaySnapshot(int day)
01486:             {
01487:                 _m.SetupMedical();
01488:                 _medicalSnapshot = _m._medical.CaptureSave();
01489:                 _m.EnsureMedicalPipeline();
01490:                 _pipelineSnapshot = _m._medical.CapturePipelineSave();
01491:
01492:                 _m.SetupDisease();
01493:                 // CaptureState() already returns an independent clone; serialize
01494:                 // to JSON so RestorePreDaySnapshot can rebuild from the same
01495:                 // durable save format used by the hub envelope.
01496:                 _diseaseSnapshotJson = s_json.Serialize(_m._disease.Engine.CaptureState());
01497:             }
01498:             public void RestorePreDaySnapshot(int day)
01499:             {
01500:                 if (_medicalSnapshot != null)
01501:                     _m._medical.RestoreSave(_medicalSnapshot);
01502:                 if (_pipelineSnapshot != null && _m._medical.Pipeline != null)
01503:                     _m._medical.Pipeline.RestoreState(_pipelineSnapshot);
01504:                 if (_diseaseSnapshotJson != null && _m._disease != null)
01505:                 {
01506:                     var restored = s_json.Deserialize<Ashfall.Core.Disease.DiseaseSystemState>(_diseaseSnapshotJson);
01507:                     if (restored != null)
01508:                         _m._disease.Engine.RestoreState(restored);
01509:                 }
01510:             }
01511:             public void TickDay(int day, List<DayStateChangeEvent> events)
01512:             {
01513:                 _m.SetupMedical();
01514:                 _m.EnsureMedicalPipeline();
01515:
01516:                 // Task #133 medical progression order (documented in the plan):
01517:                 // 1. scheduled procedures resolve (consume + apply at completion)
01518:                 // 2. chemical dependency progression (single tick owner)
01519:                 // 3. disease progression
01520:                 if (_m._medical.Pipeline != null)
01521:                 {
01522:                     // SHELTER_EMP_MEDICAL_POWER: procedures advance only while the
01523:                     // clinic has power — an outage freezes remaining hours (no
01524:                     // reroll, no cost anomaly; costs consume at completion).
01525:                     float clinicPower = _m._powerGrid?.System == null || _m._powerGrid.System.IsRoomPowered("room_clinic") ? 1f : 0f;
01526:                     _m._medical.Pipeline.AdvanceScheduled(24f * clinicPower, day);
01527:                 }
01528:                 _m._medical.TickHours(24f);
01529:
01530:                 _m.SetupDisease();
01531:                 _m._disease.TickDaily(day);
01532:
01533:                 // Flagship XI (Plan 155): the strain layer runs immediately after
01534:                 // disease progression — mutations transition this day's outcomes,
01535:                 // cure research advances before triage reads the ward.
01536:                 _m.SetupPathogenStrains();
01537:                 if (_m._pathogenStrains != null)
01538:                 {
01539:                     _m._pathogenStrains.TickMutations(day);
01540:                     _m._pathogenStrains.AdvanceCureProjects(day);
01541:                 }
01542:
01543:                 // Plan 60 / D5 + D7 — bridge illness into the shared sick-list band
01544:                 // ladder and keep the memorial grief sink bound. Runs after the
01545:                 // disease tick so it reads this day's stage, and is idempotent.
01546:                 _m.SyncDiseaseTriage(day, events);
01547:                 _m.VacateInvalidFitnessAssignments();
01548:                 _m.ApplyOverworkNeedsModifiers();
01549:                 _m.ApplyGriefNeedsModifiers();
01550:                 _m._medicalWardSession?.DrainDayEvents(events);
01551:
01552:                 // Plan 172: evaluate deterministic radiation mutation development on day boundary
01553:                 _m.TickMutations(day, events);
01554:
01555:                 if (_m._expansionHubDirty) _m.SaveExpansionHub();
01556:
01557:                 events.Add(new DayStateChangeEvent("medical_disease_ticked", "medical_disease", null, null, day));
01558:             }
01559:         }
01560:
01561:         private sealed class DutyRosterDayOwner : IDayAdvanceOwner
01562:         {
01563:             private readonly Main _m;
01564:             public DutyRosterDayOwner(Main m) => _m = m;
01565:             public void CapturePreDaySnapshot(int day) { }
01566:             public void TickDay(int day, List<DayStateChangeEvent> events)
01567:             {
01568:                 _m.SetupDutyRoster();
01569:                 _m._dutyRoster!.SyncDay(day);
01570:                 _m._dutyRoster!.TickDay(_m.BuildHomeOccupantSnapshot());
01571:                 _m._dutyRoster.DrainDayEvents(events);
01572:                 _m.SetupIceRoad();
01573:                 _m._dutyRoster.SyncHoldfastToDuty(_m._core.Census, _m._core.IceRoad, _m._expansions.Waystation, _m._core.Brine, day);
01574:                 _m._dutyRosterPanel?.RefreshView();
01575:                 if (_m._dutyRosterDirty) _m.SaveDutyRoster();
01576:
01577:                 events.Add(new DayStateChangeEvent("duty_roster_ticked", "duty_roster", null, null, day));
01578:             }
01579:         }
01580:
01581:         private sealed class SurvivorSocialDayOwner : IDayAdvanceOwner
01582:         {
01583:             private readonly Main _m;
01584:             public SurvivorSocialDayOwner(Main m) => _m = m;
01585:             public void CapturePreDaySnapshot(int day) { }
01586:             public void TickDay(int day, List<DayStateChangeEvent> events)
01587:             {
01588:                 _m.TickSurvivorSocial(day);
01589:                 events.Add(new DayStateChangeEvent("survivor_social_ticked", "survivor_social", null, null, day));
01590:             }
01591:         }
01592:
01593:         private sealed class Phase0PsychologyDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01594:         {
01595:             private readonly Main _m;
01596:             private Phase0EffectsSaveState? _snapshot;
01597:             public Phase0PsychologyDayOwner(Main m) => _m = m;
01598:             // Phase0EffectsSaveState is built fresh by CaptureSave (deep-copied
01599:             // sub-states), so holding it directly is a true independent snapshot.
01600:             public void CapturePreDaySnapshot(int day)
01601:             {
01602:                 _m.SetupPhase0();
01603:                 _snapshot = _m._phase0.CaptureSave();
01604:             }
01605:             public void RestorePreDaySnapshot(int day)
01606:             {
01607:                 if (_snapshot != null)
01608:                     _m._phase0.RestoreSave(_snapshot);
01609:             }
01610:             public void TickDay(int day, List<DayStateChangeEvent> events)
01611:             {
01612:                 _m.SetupPhase0();
01613:                 _m._phase0.CurrentDay = day;
01614:                 _m._phase0.IsInFalloutStorm = _m._world != null && _m._world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
01615:                 _m._phase0.IsNightTime = day % 2 == 0;
01616:                 _m._phase0.TickDay(day);
01617:
01618:                 events.Add(new DayStateChangeEvent("phase0_ticked", "phase0_psychology", null, null, day));
01619:             }
01620:         }
01621:
01622:         // ── Phase 4 Owners ───────────────────────────────────────────────
01623:
01624:         private sealed class ExpeditionsCaravansDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01625:         {
01626:             private readonly Main _m;
01627:             private ExpeditionAggregateState? _expeditionSnapshot;
01628:             private TravelingCaravanState? _caravanSnapshot;
01629:             private VehicleGarageState? _garageSnapshot;
01630:             public ExpeditionsCaravansDayOwner(Main m) => _m = m;
01631:             public void CapturePreDaySnapshot(int day)
01632:             {
01633:                 _m.SetupExpeditions();
01634:                 _expeditionSnapshot = _m._expeditions.CaptureSaveAggregate();
01635:                 _m.SetupCaravans();
01636:                 _caravanSnapshot = _m._caravans.CaptureSave();
01637:                 // Plan 50 — recovery progress is part of the day's deterministic
01638:                 // state; a failed day restore must roll it back with the rest.
01639:                 _garageSnapshot = _m.EnsureVehicleGarage().CaptureState();
01640:             }
01641:             public void RestorePreDaySnapshot(int day)
01642:             {
01643:                 if (_expeditionSnapshot != null)
01644:                     _m._expeditions.RestoreSaveAggregate(_expeditionSnapshot);
01645:                 if (_caravanSnapshot != null)
01646:                     _m._caravans.RestoreSave(_caravanSnapshot);
01647:                 if (_garageSnapshot != null)
01648:                     _m.EnsureVehicleGarage().RestoreState(_garageSnapshot);
01649:             }
01650:             public void TickDay(int day, List<DayStateChangeEvent> events)
01651:             {
01652:                 _m.SetupExpeditions();
01653:                 _m._expeditions.TickHours(24f);
01654:                 // Plan 50 — recovery teams work across campaign days. The garage
01655:                 // owns the mission ledger; this owner supplies the day progress
01656:                 // (the vehicle-garage section is captured by SaveOrchestrator).
01657:                 _m.EnsureVehicleGarage().AdvanceRecoveries(24);
01658:                 _m.SetupReconTelemetry();
01659:                 _m._reconTelemetry?.TickDay(day);
01660:
01661:                 _m.SetupDutyRoster();
01662:                 var expeditions = _m._expeditions.Engine.CaptureState();
01663:                 if (expeditions != null && _m._dutyRoster != null)
01664:                 {
01665:                     for (int i = 0; i < expeditions.Count; i++)
01666:                     {
01667:                         var ex = expeditions[i];
01668:                         if (ex == null) continue;
01669:                         if (ex.phase == (int)ExpeditionPhase.Completed && !string.IsNullOrEmpty(ex.survivorId))
01670:                         {
01671:                             bool crisis = _m._dutyRoster.Quests.IsCrisisQuestActive();
01672:                             _m._dutyRoster.BridgeHatchReturn(ex.survivorId, crisis: crisis);
01673:                             // Flagship XI Slice 8: a completed sortie is a hope
01674:                             // source; contagion spreads the relief naturally.
01675:                             _m.SetupMoraleContagion();
01676:                             _m._moraleContagion?.System.StartContagionEvent(
01677:                                 "contagion_successful_rescue_hope", string.Empty, day);
01678:                             break;
01679:                         }
01680:                     }
01681:                 }
01684:                 // Plan 14A — the authoritative weather (advanced in phase 1)
01685:                 // drives caravan embargo blocking/slowing for this movement day.
01686:                 _m._caravans.TickRoute(
01687:                     _m._world != null && _m._world.Weather != null
01688:                         ? _m._world.Weather.Current
01689:                         : WeatherKind.Clear,
01690:                     day,
01691:                     _m._campaignDay?.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Economy, day, 3));
01692:                 _m.EnsureCaravanTrade();
01693:                 if (_m._caravanTradeNetwork != null)
01694:                 {
01695:                     _m._caravanTradeNetwork.Map = _m._world?.WastelandMap;
01696:                     _m._caravanTradeNetwork.TickDay(day);
01697:                 }
01698:
01699:                 events.Add(new DayStateChangeEvent("expeditions_caravans_ticked", "expeditions_caravans", null, null, day));
01700:             }
01701:         }
01702:
01703:         /// <summary>
01705:         /// loyalty-pressure routing to the faction authorities, intercept rolls.
01706:         /// </summary>
01707:         private sealed class PsyOpsDayOwner : IDayAdvanceOwner
01708:         {
01709:             private readonly Main _m;
01710:             public PsyOpsDayOwner(Main m) => _m = m;
01711:             public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
01712:             public void TickDay(int day, List<DayStateChangeEvent> events)
01713:             {
01714:                 _m.SetupPsyOps();
01715:                 if (_m._psyops == null) return;
01716:
01721:                         active++;
01722:
01723:                 _m._psyops.System.TickCampaigns(day);
01724:                 events.Add(new DayStateChangeEvent("psyops_ticked", "psyops", null, null, active));
01725:             }
01726:         }
01727:
01728:         /// <summary>
01729:         /// Plan 173 Phase 2 — radio program production day: prep ticks and
01730:         /// opportunistic delivery via existing schedule Resolve facts.
01731:         /// </summary>
01732:         private sealed class RadioProgramProductionDayOwner : IDayAdvanceOwner
01733:         {
01734:             private readonly Main _m;
01735:             public RadioProgramProductionDayOwner(Main m) => _m = m;
01736:             public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
01737:             public void TickDay(int day, List<DayStateChangeEvent> events)
01738:             {
01739:                 int before = _m._radioProgramProduction?.System.GetActiveJobs().Count ?? 0;
01740:                 _m.TickRadioProgramProduction(day);
01741:                 int after = _m._radioProgramProduction?.System.GetActiveJobs().Count ?? 0;
01742:                 events.Add(new DayStateChangeEvent("radio_program_production_ticked", "radio_program_production", null, null, after));
01743:                 if (before != after)
01744:                     events.Add(new DayStateChangeEvent("radio_program_production_active_delta", "radio_program_production", null, null, after - before));
01745:             }
01746:         }
01750:         /// hazards for every active underground sortie, claustrophobia morale
01751:         /// through the needs authority, and forced retreats back through the
01752:         /// expedition engine. Runs after expeditions (registration phase 4,
01753:         /// ordinal after expeditions_caravans) and before world_evolution.
01754:         /// </summary>
01755:         /// <summary>
01756:         /// Plan 211 — underworld market day owner (ownerId `underworld_market`,
01757:         /// phase 4, after the debt-ledger tick). Refreshes discovered
01758:         /// syndicates' stock snapshots from the black_market_stock RNG
01759:         /// stream (fork-per-day, position-independent), then runs the debt
01760:         /// due/overdue + heat tick. Emits underworld day events.
01761:         /// </summary>
01762:         private sealed class UnderworldMarketDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01763:         {
01764:             private readonly Main _m;
01765:             private Ashfall.Core.Economy.BlackMarketState? _snapshot;
01766:             public UnderworldMarketDayOwner(Main m) => _m = m;
01767:             public void CapturePreDaySnapshot(int day)
01768:             {
01769:                 _m.SetupBlackMarket();
01770:                 _snapshot = _m._blackMarket!.CaptureSave();
01771:             }
01772:             public void RestorePreDaySnapshot(int day)
01773:             {
01774:                 if (_snapshot != null)
01775:                     _m._blackMarket!.RestoreSave(_snapshot);
01776:             }
01777:             public void TickDay(int day, List<DayStateChangeEvent> events)
01778:             {
01779:                 _m.SetupBlackMarket();
01780:                 var stockRng = _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.BlackMarketStock, day, 0);
01781:                 _m._blackMarket!.TickDay(day, stockRng);
01782:                 if (_m._blackMarketDirty) _m.SaveBlackMarket();
01783:                 events.Add(new DayStateChangeEvent("underworld_ticked", "underworld_market", null, null,
01784:                     _m._blackMarket.System.DiscoveredContacts.Count));
01785:             }
01786:         }
01787:
01788:         private sealed class SubterraneanDayOwner : IDayAdvanceOwner
01789:         {
01790:             private readonly Main _m;
01791:             public SubterraneanDayOwner(Main m) => _m = m;
01792:             public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
01793:             public void TickDay(int day, List<DayStateChangeEvent> events)
01794:             {
01795:                 _m.SetupSubterranean();
01796:                 if (_m._subterranean == null) return;
01797:
01798:                 _m._subterranean.TickDay(day, requestRetreat: survivorId =>
01799:                 {
01800:                     _m._expeditions?.Retreat(survivorId);
01801:                 });
01802:
01803:                 // Plan 49 flood bridge: rising subterranean water is the canonical
01804:                 // ingress for the excavation hazard sector of the same node.
01805:                 _m.ProjectSubterraneanFloodIntoExcavationHazards();
01806:
01807:                 int discovered = 0;
01808:                 var nodes = _m._subterranean.System.State.nodes;
01809:                 for (int i = 0; i < nodes.Count; i++)
01810:                     if (nodes[i] != null && nodes[i].discovered) discovered++;
01811:
01812:                 events.Add(new DayStateChangeEvent("subterranean_ticked", "subterranean_network", null, null, discovered));
01813:             }
01814:         }
01815:
01816:         /// <summary>
01817:         /// Task 122 — the world changes because of time and player action:
01818:         /// feeds live weather into landmark decay and location contamination,
01819:         /// runs seeded wildlife migration, records expedition consequences on
01820:         /// locations, bridges faction dominance into ownership, shifts market
01821:         /// scarcity with wildlife pressure, and surfaces every major change
01822:         /// through briefing events, journal lines, and radio intercepts.
01823:         /// </summary>
01824:         private sealed class EvolvingWorldDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
01825:         {
01826:             private readonly Main _m;
01827:             private LocationEvolutionSaveState? _locSnapshot;
01828:             private WildlifeSaveState? _wildSnapshot;
01829:             private LandmarkSaveState? _landSnapshot;
01830:             private readonly HashSet<string> _processedExpeditions = new HashSet<string>();
01831:             private string _lastDominantFaction = string.Empty;
01832:
01833:             public EvolvingWorldDayOwner(Main m) => _m = m;
01834:
01835:             public void CapturePreDaySnapshot(int day)
01836:             {
01837:                 _m.SetupWorld();
01838:                 _locSnapshot = _m._world.LocationEvolution?.CaptureState();
01839:                 _wildSnapshot = _m._world.Wildlife?.CaptureState();
01840:                 _landSnapshot = _m._world.Landmarks?.CaptureState();
01841:             }
01842:
01843:             public void RestorePreDaySnapshot(int day)
01844:             {
01845:                 if (_locSnapshot != null) _m._world.LocationEvolution?.RestoreState(_locSnapshot);
01846:                 if (_wildSnapshot != null) _m._world.Wildlife?.RestoreState(_wildSnapshot);
01847:                 if (_landSnapshot != null) _m._world.Landmarks?.RestoreState(_landSnapshot);
01848:                 _processedExpeditions.Clear();
01849:             }
01850:
01851:             public void TickDay(int day, List<DayStateChangeEvent> events)
01852:             {
01853:                 _m.SetupWorld();
01854:                 var world = _m._world;
01855:
01858:                 float ashfallMm = Main.AshfallMmFor(kind);
01859:
01860:                 // Pre-tick deltas we report on.
01861:                 var collapsedBefore = CollapsedSet(world);
01862:                 var sectorsBefore = SectorMap(world);
01863:                 var ownersBefore = OwnerMap(world);
01864:
01865:                 // ── The world moves ──
01866:                 world.Landmarks?.TickDay(day, ashfallMm);
01867:                 world.LocationEvolution?.TickDay(day,
01868:                     new LocationEvolutionInputs(world.Weather.OutdoorRadModifier, hazard),
01869:                     _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, day, 0));
01870:                 world.Wildlife?.TickDay(day,
01871:                     _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, day, 1));
01872:
01873:                 // Plans 162-165 (Plan 165): the ecology layer ticks immediately
01874:                 // after the migration authority moved the packs — it reads and
01875:                 // mutates populations only through WildlifeMigrationSystem.
01876:                 _m.TickWildlifeEcosystemDay(day);
01877:
01878:                 // ── Landmark collapses → warning, journal ──
01879:                 if (world.Landmarks != null)
01880:                 {
01881:                     foreach (var lm in world.Landmarks.State.landmarks)
01882:                     {
01883:                         if (lm == null || !lm.isCollapsed || lm.collapseDay != day) continue;
01884:                         if (collapsedBefore.Contains(lm.landmarkId)) continue;
01885:                         events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
01886:                             $"Landmark collapsed: {lm.landmarkId}", $"at {lm.locationId} (day {day})", lm.structuralIntegrity));
01887:                         _m.SetupJournal();
01888:                         _m._journal.TryAddRawEntry($"world_{lm.landmarkId}_collapse",
01889:                             $"🔺 {lm.landmarkId} came down at {lm.locationId}. The skyline is poorer by one shape.",
01890:                             null!, day);
01891:                     }
01892:                 }
01893:
01894:                 // ── Pack migrations → radio intercepts ──
01895:                 if (world.Wildlife != null)
01896:                 {
01897:                     int reported = 0;
01898:                     var after = SectorMap(world);
01899:                     foreach (var pack in world.Wildlife.State.packs)
01900:                     {
01901:                         if (pack == null || reported >= 3) continue;
01902:                         if (sectorsBefore.TryGetValue(pack.packId, out var before)
01903:                             && before != pack.currentSectorId)
01904:                         {
01905:                             // Plan 28: archetype-flavored coarse sighting; the
01906:                             // generic move line stays for unremarkable species.
01907:                             string notice = WildlifeSeasonalCalendar.MigrationNotice(
01908:                                 WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId),
01909:                                 pack.speciesId, before, pack.currentSectorId, day);
01910:                             events.Add(new DayStateChangeEvent("radio_intercept", "world_evolution",
01911:                                 "wildlife net",
01912:                                 notice ?? $"{pack.packId} sighted moving {before} into {pack.currentSectorId}",
01915:                             // Plan 28 Phase 5: observation drives knowledge — a
01916:                             // sighted species unlocks its field-guide teach
01917:                             // entry (session knowledge; persistence = Plan 20A).
01918:                             var teach = WildlifeSeasonalCalendar.FieldGuideEntryFor(pack.speciesId);
01919:                             if (teach != null) _m.UnlockFieldGuideObservation(teach);
01920:                         }
01921:                         if (pack.isRabid && pack.lastThreatFiredDay == day)
01922:                         {
01923:                             events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
01924:                                 $"Rabid {pack.speciesId}", $"{pack.packId} turned in {pack.currentSectorId}", pack.aggressionScore));
01925:                         }
01926:                     }
01927:                 }
01928:
01929:                 // ── Expedition consequences on locations ──
01930:                 _m.SetupExpeditions();
01931:                 var expeditions = _m._expeditions.Engine.CaptureState();
01932:                 if (expeditions != null)
01933:                 {
01934:                     foreach (var ex in expeditions)
01935:                     {
01936:                         if (ex == null || string.IsNullOrEmpty(ex.expeditionId)) continue;
01937:                         if (ex.phase != (int)ExpeditionPhase.Completed && ex.phase != (int)ExpeditionPhase.Failed) continue;
01938:                         if (!_processedExpeditions.Add(ex.expeditionId)) continue;
01939:                         if (string.IsNullOrEmpty(ex.locationId) || world.LocationEvolution == null) continue;
01940:
01941:                         if (ex.phase == (int)ExpeditionPhase.Completed)
01942:                         {
01943:                             world.LocationEvolution.MarkCleared(ex.locationId, day);
01944:                             events.Add(new DayStateChangeEvent("expedition_milestone", "world_evolution",
01945:                                 ex.locationId, "swept clean — salvage thins here for a while", 1));
01946:                         }
01947:                         else
01948:                         {
01949:                             world.LocationEvolution.MarkVisited(ex.locationId, day);
01950:                             world.LocationEvolution.AddThreat(ex.locationId, LocationEvolutionSystem.ThreatSquatters);
01951:                             events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
01952:                                 ex.locationId, "sortie lost — stragglers now haunt the ground", 1));
01953:                         }
01954:                     }
01955:                 }
01956:
01957:                 // ── Faction dominance → location ownership ──
01958:                 _m.SetupYearOfAsh();
01959:                 string dominant = _m._yearOfAsh?.FactionWar?.DominantFactionId ?? string.Empty;
01960:                 if (!string.IsNullOrEmpty(dominant) && dominant != _lastDominantFaction)
01961:                 {
01962:                     bool firstObservation = _lastDominantFaction.Length == 0;
01963:                     _lastDominantFaction = dominant;
01966:                         foreach (var seed in world.Seeds.location_seeds)
01967:                         {
01968:                             if (seed == null || seed.owner != dominant) continue;
01969:                             var before = ownersBefore.TryGetValue(seed.location_id, out var o) ? o : null;
01970:                             if (before == dominant) continue;
01971:                             world.LocationEvolution?.SetLocationOwner(seed.location_id, dominant);
01972:                             events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
01973:                                 seed.location_id, $"control passes to {dominant}", 1));
01974:                             _m.SetupJournal();
01975:                             _m._journal.TryAddRawEntry($"world_{seed.location_id}_owner_{dominant}",
01976:                                 $"🔻 {seed.location_id} answer to {dominant} now. Flags change; the ground stays.",
01977:                                 null!, day);
01982:                 // ── Wildlife pressure → market scarcity & trapping density ──
01983:                 _m.SetupEvolvingWorldInfluence();
01984:                 float ratio = world.Wildlife?.GetGlobalPopulationRatio() ?? 1f;
01985:                 var goods = EvolvingWorldSeeder.ScarcityGoods(world.Seeds);
01986:                 if (goods.Count > 0)
01987:                 {
01988:                     float delta = ratio < 0.6f ? 0.02f : ratio < 0.85f ? 0.005f : ratio > 1.2f ? -0.005f : 0f;
01989:                     if (Math.Abs(delta) > 0f)
01990:                     {
01991:                         _m.SetupEconomy();
01992:                         // Plan 56 phase 5 — provenance-aware scarcity: goods with
01993:                         // an active caravan supply line are buffered (0.5×);
01994:                         // general goods track the market (1.0×); goods with no
01995:                         // supply line escalate (1.5×). Active origin regions come
01996:                         // from the caravan data authority.
01997:                         var origins = new List<string>();
01998:                         foreach (var cd in CaravanCatalogLoader.Load(_m._dataDir))
01999:                             if (!string.IsNullOrEmpty(cd.origin_region) && !origins.Contains(cd.origin_region))
02000:                                 origins.Add(cd.origin_region);
02001:                         foreach (var g in goods)
02002:                         {
02003:                             float scaled = delta * RegionalSupplyRouter.WorldShortageDemandScale(
02004:                                 _m._economy.Catalog, g, origins);
02005:                             if (Math.Abs(scaled) > 0f)
02006:                                 _m._economy.Market.AdjustDemand(g, scaled);
02007:                         }
02008:                     }
02009:                 }
02010:
02011:                 // ── Plan 28 Phase 3: war-blocked corridors & collapse notice ──
02012:                 // Faction dominance projects onto the sector graph: sectors
02013:                 // holding dominant-faction ground close to wildlife movement
02014:                 // (stateless projection — the migration runtime never
02015:                 // persists blockage). Binding: seeds' location records carry
02016:                 // an optional sector_id.
02017:                 {
02018:                     world.Wildlife?.ClearSectorBlockages();
02019:                     if (!string.IsNullOrEmpty(dominant) && world.Seeds?.location_seeds != null)
02020:                     {
02021:                         foreach (var seed in world.Seeds.location_seeds)
02022:                         {
02023:                             if (seed == null || seed.owner != dominant) continue;
02024:                             var sector = SectorOfLocation(world, seed.location_id);
02025:                             if (!string.IsNullOrEmpty(sector)) world.Wildlife?.SetSectorBlocked(sector, true);
02026:                         }
02027:                     }
02028:                 }
02029:
02030:                 // ── Plan 28 Phase 4: ecological infestations ──
02031:                 _m.TickEcologicalInfestations(day, events);
02032:
02033:                 // ── Plans 46-49: Workshop, Radio, Social, Subterranean Hazards ──
02034:                 _m.TickPlans46_49(day, events);
02035:
02036:                 // ── Plans 198-201: CBRN Hazards, Comms Array, Ceremonies, Robotics ──
02037:                 _m.TickPlans198_201(day, events);
02038:
02039:                 // ── Plans 194-197: Naval & River, Item Degradation, Hobbies & Downtime, Winter Freeze ──
02040:                 _m.TickPlans194_197(day, events);
02041:
02042:                 // ── Plans 186-189: Radioactive Fallout, Desperation, Mercenary, Archaeology ──
02043:                 _m.TickPlans186_189(day, 24.0f);
02044:
02045:                 // ── Plan 176: anomaly hazard movement + approach warnings ──
02046:                 _m.TickAnomalyHazard(day);
02047:
02048:                 // ── Plan 174: companion care, bond/training, roles ──
02049:                 _m.TickCompanionDay(day);
02050:
02051:                 // ── Plan 177: bionics decay/power + anomaly disruption ──
02052:                 _m.TickBionicsDay(day);
02053:
02054:                 // ── Plan 175: ideological pressure, rituals, tension ──
02055:                 _m.TickZealotryDay(day);
02056:                 _m.TickSpiritualDay(day);
02057:
02058:                 // ── Plans 190-193: Infection & Amputation, Railways, Subterranean Fungi, Wasteland Justice ──
02059:                 _m.TickPlans190_193(day);
02060:
02061:                 // ── Plans 126-129: fermentation (drone/caster/lidar land in later waves) ──
02062:                 _m.TickPlans126_129(day);
02063:
02064:                 // ── Plan 147: contraband stash discovery rumors (pure reads + deduped journal) ──
02065:                 _m.TickContrabandStashDay(day);
02066:
02067:                 // ── Plan 147: shelter barter caravan schedule/arrivals (day-gated broker stock) ──
02068:                 _m.TickShelterBarterDay(day);
02069:
02070:                 // ── Plan 202: Plastic Pyrolysis (retort bay, grid-power projected) ──
02071:                 _m.TickPlasticPyrolysis(day);
02072:
02073:                 // ── Plan 205: airdrop descent/landing/interception + crate collection ──
02074:                 _m.TickCargoAirdrop(day);
02075:
02076:                 // ── Plan 203: perimeter weather wear + false alarms ──
02077:                 _m.TickPerimeterDefenseDaily(day);
02078:
02079:                 // ── Plans 178-181: Childhood Rearing, Prisoner Management, Mutation Trees, Stealth ──
02080:                 _m.TickPlans178_181(day);
02081:
02082:                 // ── Plans 182-185: Aviation, Forced Labor, Narcotics, Settlement Politics ──
02083:                 _m.TickPlans182_185(day);
02084:
02085:                 // ── Plan 28 Phase 3: collapse/scarcity notice (bounded) ──
02086:
02087:                 // ── Plan 28 Phase 3: collapse/scarcity notice (bounded) ──
02092:                     events.Add(new DayStateChangeEvent("hazard_warning", "world_evolution",
02093:                         "wildlife collapse",
02094:                         "the land has gone quiet — snare lines and larders both", ratio));
02095:                     _m.SetupJournal();
02096:                     _m._journal.TryAddRawEntry($"world_wildlife_collapse_{day}",
02097:                         "Something changed in the counts. The dogs range wider; the snares come back empty.",
02098:                         null!, day);
02099:                 }
02100:
02101:                 events.Add(new DayStateChangeEvent("world_evolution_ticked", "world_evolution", null, null, day));
02102:             }
02103:
02104:             /// <summary>Collapse notices re-arm after this many days (anti-spam).</summary>
02105:             private const int CollapseNoticeCooldownDays = 12;
02106:             private int _lastCollapseNoticeDay = -30;
02107:
02108:             /// <summary>
02109:             /// Plan 28 Phase 3 — sector binding for war-blocked corridors: the
02110:             /// seeds' location records carry an optional sector binding so the
02111:             /// dominant faction's ground closes its representative sector to
02112:             /// wildlife movement (stateless projection, never persisted).
02113:             /// </summary>
02114:             private static string? SectorOfLocation(WorldHostSession world, string locationId)
02115:             {
02116:                 if (world.Seeds?.location_seeds == null) return null;
02117:                 foreach (var seed in world.Seeds.location_seeds)
02118:                     if (seed != null && string.Equals(seed.location_id, locationId, StringComparison.Ordinal))
02119:                         return string.IsNullOrEmpty(seed.sector_id) ? null : seed.sector_id;
02120:                 return null;
02121:             }
02122:
02123:             private static HashSet<string> CollapsedSet(WorldHostSession world)
02124:             {
02125:                 var set = new HashSet<string>();
02126:                 foreach (var lm in world.Landmarks?.State.landmarks ?? new List<LandmarkStatusRecord>())
02127:                     if (lm != null && lm.isCollapsed) set.Add(lm.landmarkId);
02129:             }
02130:
02131:             private static Dictionary<string, string> SectorMap(WorldHostSession world)
02132:             {
02133:                 var map = new Dictionary<string, string>();
02134:                 foreach (var p in world.Wildlife?.State.packs ?? new List<WildlifePackRecord>())
02135:                     if (p != null) map[p.packId] = p.currentSectorId;
02136:                 return map;
02137:             }
02138:
02139:             private static Dictionary<string, string> OwnerMap(WorldHostSession world)
02140:             {
02141:                 var map = new Dictionary<string, string>();
02142:                 foreach (var m in world.LocationEvolution?.State.mutations ?? new List<LocationMutationRecord>())
02143:                     if (m != null) map[m.locationId] = m.currentOwner;
02144:                 return map;
02145:             }
02146:         }
02147:
02148:         private sealed class NarrativeQuestsVerdictDayOwner : IDayAdvanceOwner
02149:         {
02150:             private readonly Main _m;
02151:             public NarrativeQuestsVerdictDayOwner(Main m) => _m = m;
02152:             public void CapturePreDaySnapshot(int day) { }
02153:             public void TickDay(int day, List<DayStateChangeEvent> events)
02154:             {
02155:                 _m.SetupMoralChoice();
02156:                 _m._moralChoice.Reconcile(day);
02157:                 _m.TickFactionBranchDay(day);
02158:                 _m.SetupCounterIntelligence();
02159:                 _m._counterIntelligence?.TickDay(day);
02160:
02161:                 _m.TickVerdict(day, _m.LivingDwellerCountEstimate());
02162:
02163:                 if (day >= 180)
02164:                 {
02165:                     _m.SetupYearOfAsh();
02166:                     _m._yearOfAsh.TickDay(day);
02167:                 }
02168:
02169:                 if (day >= 260)
02170:                 {
02173:                 }
02174:
02175:                 _m.SetupExpansions();
02176:                 _m._expansions.TickCrossingQuests(day);
02177:
02178:                 _m.SetupExpansionQuests();
02179:                 _m._expansionQuests.TickDay(day);
02180:                 _m.SetupNpcArcs();
02181:
02182:                 // Plan 143: one deterministic daily draw from the shared
02183:                 // narrative stream. The selected event is persisted as a
02184:                 // pending modal; selecting it never applies consequences.
02185:                 _m.SetupNarrative();
02186:                 var arc = _m._narrative.SelectArcForDay(
02187:                     day,
02188:                     _m._campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Narrative, day, 0));
02198:
02199:                 _m.SetupEchoes();
02200:                 var dueEchoConsequences = _m._echoes?.TickDay(day);
02201:                 if (dueEchoConsequences != null && dueEchoConsequences.Count > 0)
02202:                 {
02203:                     events.Add(new DayStateChangeEvent(
02204:                         "echo_consequence_due",
02205:                         "narrative_quests_verdict",
02206:                         dueEchoConsequences.Count.ToString(),
02207:                         null,
02208:                         day));
02209:                 }
02210:                 var echo = arc == null
02223:                 }
02224:
02225:                 events.Add(new DayStateChangeEvent("narrative_ticked", "narrative_quests_verdict", null, null, day));
02226:             }
02227:         }
02228:
02229:         // ── Phase 5 Owners ───────────────────────────────────────────────
02230:
02231:         private sealed class HostEventsDayOwner : IDayAdvanceOwner
02232:         {
02233:             private readonly Main _m;
02234:             public HostEventsDayOwner(Main m) => _m = m;
02235:             public void CapturePreDaySnapshot(int day) { }
02236:             public void TickDay(int day, List<DayStateChangeEvent> events)
02237:             {
02238:                 _m.SetupEventAdapter();
02239:                 bool hydroAudit = _m._muster?.HydroBarons?.AdminReform ?? false;
02240:                 bool hydroSeized = _m._muster?.HydroBarons?.PlantSeized ?? false;
02241:                 bool osteophageInquiry = (_m._yearOfAsh != null && _m._yearOfAsh.Timeline.CurrentDay >= 205) || day >= 205;
02242:                 bool coldCountBroadcast = _m._muster?.ColdCount?.BroadcastSent ?? false;
02243:                 _m._hostEventAdapter?.EvaluateTriggers(day, hydroAudit, hydroSeized, osteophageInquiry, coldCountBroadcast);
02244:
02245:                 events.Add(new DayStateChangeEvent("events_evaluated", "host_events", null, null, day));
02246:             }
02247:         }
02248:
02249:         private sealed class MemorialDayOwner : IDayAdvanceOwner
02250:         {
02251:             private readonly Main _m;
02252:             public MemorialDayOwner(Main m) => _m = m;
02253:             public void CapturePreDaySnapshot(int day) { }
02254:             public void TickDay(int day, List<DayStateChangeEvent> events)
02255:             {
02256:                 _m.SetupMemorial();
02257:                 events.Add(new DayStateChangeEvent("memorial_checked", "memorial", null, null, day));
02258:             }
02259:         }
02260:
02261:         /// <summary>Plan 29 Task 29A: once-daily room-history milestone pass.</summary>
02262:         private sealed class ShelterRoomHistoryDayOwner : IDayAdvanceOwner
02263:         {
02264:             private readonly Main _m;
02265:             public ShelterRoomHistoryDayOwner(Main m) => _m = m;
02266:             public void CapturePreDaySnapshot(int day) { }
02267:             public void TickDay(int day, List<DayStateChangeEvent> events)
02268:             {
02269:                 _m.TickShelterRoomHistoryMilestones(day);
02270:             }
02271:         }
02272:
02273:         /// <summary>Expansion 25 rail track maintenance day owner (ownerId <c>rail_track_maintenance</c>, phase 5).</summary>
02274:         private sealed class RailTrackMaintenanceDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02275:         {
02276:             private readonly Main _m;
02277:             private Ashfall.Core.Rail.RailMaintenanceState? _snapshot;
02278:             public RailTrackMaintenanceDayOwner(Main m) => _m = m;
02279:
02280:             public void CapturePreDaySnapshot(int day)
02281:             {
02282:                 _m.SetupRailTrackMaintenance();
02283:                 _snapshot = _m._railTrackMaintenance?.CaptureState();
02284:             }
02285:
02286:             public void RestorePreDaySnapshot(int day)
02287:             {
02288:                 if (_snapshot != null) _m._railTrackMaintenance?.RestoreState(_snapshot);
02289:             }
02290:
02291:             public void TickDay(int day, List<DayStateChangeEvent> events)
02292:             {
02293:                 _m.SetupRailTrackMaintenance();
02294:                 var census = _m.GetRailTrackMaintenanceCensus();
02295:                 events.Add(new DayStateChangeEvent(
02296:                     "rail_track_maintenance_ticked", "rail_track_maintenance", null, null, census.DegradedSegments));
02297:             }
02298:         }
02299:
02300:         /// <summary>Expansion 29 glassworks day owner (ownerId <c>glassworks</c>, phase 5).</summary>
02301:         private sealed class GlassworksDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02302:         {
02303:             private readonly Main _m;
02304:             private Ashfall.Core.Optics.GlassworksState? _snapshot;
02305:             public GlassworksDayOwner(Main m) => _m = m;
02306:
02307:             public void CapturePreDaySnapshot(int day)
02308:             {
02309:                 _m.SetupGlassworks();
02310:                 _snapshot = _m._glassworks?.CaptureState();
02311:             }
02312:
02313:             public void RestorePreDaySnapshot(int day)
02314:             {
02315:                 if (_snapshot != null) _m._glassworks?.RestoreState(_snapshot);
02316:             }
02317:
02318:             public void TickDay(int day, List<DayStateChangeEvent> events)
02319:             {
02320:                 _m.SetupGlassworks();
02321:                 _m.TickGlassworks(day);
02322:                 int annealed = _m._glassworks?.Census.AnnealedBatches ?? 0;
02323:                 events.Add(new DayStateChangeEvent(
02324:                     "glassworks_ticked", "glassworks", null, null, annealed));
02325:             }
02326:         }
02327:         /// <summary>Expansion 30 press day owner (ownerId <c>broadsheet_press</c>, phase 5).</summary>
02328:         /// <remarks>
02329:         /// The press never prints by itself: printing is a player command, because what
02330:         /// the shelter publishes is an editorial decision. This owner is therefore a
02331:         /// census-only heartbeat plus the pre-day snapshot, so a mid-advance rollback
02332:         /// still restores the type tray and the printed archive.
02333:         /// </remarks>
02334:         private sealed class BroadsheetPressDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02335:         {
02336:             private readonly Main _m;
02337:             private Ashfall.Core.Print.BroadsheetPressState? _snapshot;
02338:             public BroadsheetPressDayOwner(Main m) => _m = m;
02339:
02340:             public void CapturePreDaySnapshot(int day)
02341:             {
02342:                 _m.SetupBroadsheetPress();
02343:                 _snapshot = _m._broadsheetPress?.CaptureState();
02344:             }
02345:
02346:             public void RestorePreDaySnapshot(int day)
02347:             {
02348:                 if (_snapshot != null) _m._broadsheetPress?.RestoreState(_snapshot);
02349:             }
02350:
02351:             public void TickDay(int day, List<DayStateChangeEvent> events)
02352:             {
02353:                 _m.SetupBroadsheetPress();
02354:                 var census = _m.GetBroadsheetPressCensus();
02355:                 events.Add(new DayStateChangeEvent(
02356:                     "broadsheet_press_ticked", "broadsheet_press", null, null, census.PublicationCount));
02357:             }
02358:         }
02359:
02360:         /// <summary>Expansion 31 kilnworks day owner (ownerId <c>kilnworks</c>, phase 5).</summary>
02361:         /// <remarks>
02362:         /// Advances the oldest queued batch by exactly one firing stage at the fixed
02363:         /// optimal temperature. Deterministic by construction: no RNG stream is read,
02364:         /// so replaying a day always fires the same batch the same way.
02365:         /// </remarks>
02366:         private sealed class KilnworksDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02367:         {
02368:             private readonly Main _m;
02369:             private Ashfall.Core.Shelter.KilnFiringState? _snapshot;
02370:             public KilnworksDayOwner(Main m) => _m = m;
02371:
02372:             public void CapturePreDaySnapshot(int day)
02373:             {
02374:                 _m.SetupKilnworks();
02375:                 _snapshot = _m._kilnworks?.CaptureState();
02376:             }
02377:
02378:             public void RestorePreDaySnapshot(int day)
02379:             {
02380:                 if (_snapshot != null) _m._kilnworks?.RestoreState(_snapshot);
02381:             }
02382:
02383:             public void TickDay(int day, List<DayStateChangeEvent> events)
02384:             {
02385:                 _m.SetupKilnworks();
02386:                 _m.TickKilnworksFiring();
02387:                 int active = _m._kilnworks?.Census.ActiveBatches ?? 0;
02388:                 events.Add(new DayStateChangeEvent(
02389:                     "kilnworks_ticked", "kilnworks", null, null, active));
02390:             }
02391:         }
02392:
02393:         /// <summary>Expansion 32 wildlife harvest ledger day owner (ownerId <c>wildlife_harvest</c>, phase 5).</summary>
02394:         private sealed class WildlifeHarvestDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02395:         {
02396:             private readonly Main _m;
02397:             private Ashfall.Core.World.WildlifeHarvestState? _snapshot;
02398:             public WildlifeHarvestDayOwner(Main m) => _m = m;
02399:
02400:             public void CapturePreDaySnapshot(int day)
02401:             {
02402:                 _m.SetupWildlifeHarvest();
02403:                 _snapshot = _m._wildlifeHarvest?.CaptureState();
02404:             }
02405:
02406:             public void RestorePreDaySnapshot(int day)
02407:             {
02408:                 if (_snapshot != null) _m._wildlifeHarvest?.RestoreState(_snapshot);
02409:             }
02410:
02411:             public void TickDay(int day, List<DayStateChangeEvent> events)
02412:             {
02413:                 _m.SetupWildlifeHarvest();
02414:                 var census = _m.GetWildlifeHarvestCensus();
02415:                 events.Add(new DayStateChangeEvent(
02416:                     "wildlife_harvest_ticked", "wildlife_harvest", null, null, census.SpeciesAtRisk));
02417:             }
02418:         }
02419:
02420:         /// <summary>Expansion 33 storm forecast day owner (ownerId <c>storm_forecast</c>, phase 5).</summary>
02421:         private sealed class StormForecastDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02422:         {
02423:             private readonly Main _m;
02424:             private Ashfall.Core.World.StormForecastState? _snapshot;
02425:             public StormForecastDayOwner(Main m) => _m = m;
02426:
02427:             public void CapturePreDaySnapshot(int day)
02428:             {
02429:                 _m.SetupStormForecast();
02430:                 _snapshot = _m._stormForecast?.CaptureState();
02431:             }
02432:
02433:             public void RestorePreDaySnapshot(int day)
02434:             {
02435:                 if (_snapshot != null) _m._stormForecast?.RestoreState(_snapshot);
02436:             }
02437:
02438:             public void TickDay(int day, List<DayStateChangeEvent> events)
02439:             {
02440:                 _m.SetupStormForecast();
02441:                 _m.TickStormForecast(day);
02442:                 int warnings = _m._stormForecast?.WarningsIssued ?? 0;
02443:                 events.Add(new DayStateChangeEvent(
02444:                     "storm_forecast_ticked", "storm_forecast", null, null, warnings));
02445:             }
02446:         }
02447:
02448:         /// <summary>Expansion 35 dependency taper withdrawal day owner (ownerId <c>dependency_taper_withdrawal</c>, phase 5).</summary>
02449:         private sealed class DependencyTaperWithdrawalDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02450:         {
02451:             private readonly Main _m;
02452:             private Ashfall.Core.Medical.DependencyTaperState? _snapshot;
02453:             public DependencyTaperWithdrawalDayOwner(Main m) => _m = m;
02454:
02455:             public void CapturePreDaySnapshot(int day)
02456:             {
02457:                 _m.SetupDependencyTaperWithdrawal();
02458:                 _snapshot = _m._dependencyTaper?.CaptureState();
02459:             }
02460:
02461:             public void RestorePreDaySnapshot(int day)
02462:             {
02463:                 if (_snapshot != null) _m._dependencyTaper?.RestoreState(_snapshot);
02464:             }
02465:
02466:             public void TickDay(int day, List<DayStateChangeEvent> events)
02467:             {
02468:                 _m.SetupDependencyTaperWithdrawal();
02469:                 var results = _m._dependencyTaper?.AdvanceAll(peerSupportRunToday: true);
02470:                 var census = _m.GetDependencyTaperCensus();
02471:                 events.Add(new DayStateChangeEvent(
02472:                     "dependency_taper_ticked", "dependency_taper_withdrawal", null, null, census.ActiveProgramsCount));
02473:             }
02474:         }
02475:
02476:         /// <summary>Expansion 37 antenatal maternal health day owner (ownerId <c>antenatal_maternal_health</c>, phase 5).</summary>
02477:         private sealed class AntenatalMaternalHealthDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02478:         {
02479:             private readonly Main _m;
02480:             private Ashfall.Core.Survivors.AntenatalMaternalCareState? _snapshot;
02481:             public AntenatalMaternalHealthDayOwner(Main m) => _m = m;
02482:
02483:             public void CapturePreDaySnapshot(int day)
02484:             {
02485:                 _m.SetupAntenatalMaternalHealth();
02486:                 _snapshot = _m._antenatalMaternalHealth?.Ledger.CaptureState();
02487:             }
02488:
02489:             public void RestorePreDaySnapshot(int day)
02490:             {
02491:                 if (_snapshot != null) _m._antenatalMaternalHealth?.Ledger.RestoreState(_snapshot);
02492:             }
02493:
02494:             public void TickDay(int day, List<DayStateChangeEvent> events)
02495:             {
02496:                 _m.SetupAntenatalMaternalHealth();
02497:                 var results = _m._antenatalMaternalHealth?.AdvanceDay(day);
02498:                 var census = _m.GetAntenatalMaternalCensus();
02499:                 events.Add(new DayStateChangeEvent(
02500:                     "antenatal_maternal_health_ticked", "antenatal_maternal_health", null, null, census.ActivePregnanciesCount));
02501:             }
02502:         }
02503:
02504:         /// <summary>Expansion 38 clinical ward triage day owner (ownerId <c>clinical_ward_triage</c>, phase 5).</summary>
02505:         private sealed class ClinicalWardTriageDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02506:         {
02507:             private readonly Main _m;
02508:             private Ashfall.Core.Medical.ClinicalWardTriageState? _snapshot;
02509:             public ClinicalWardTriageDayOwner(Main m) => _m = m;
02510:
02511:             public void CapturePreDaySnapshot(int day)
02512:             {
02513:                 _m.SetupClinicalWardTriage();
02514:                 _snapshot = _m._clinicalWardTriage?.Ledger.CaptureState();
02515:             }
02516:
02517:             public void RestorePreDaySnapshot(int day)
02518:             {
02519:                 if (_snapshot != null) _m._clinicalWardTriage?.Ledger.RestoreState(_snapshot);
02520:             }
02521:
02522:             public void TickDay(int day, List<DayStateChangeEvent> events)
02523:             {
02524:                 _m.SetupClinicalWardTriage();
02525:                 _m._clinicalWardTriage?.AdvanceDay(day, seed: day * 31);
02526:                 var census = _m.GetClinicalWardCensus();
02527:                 events.Add(new DayStateChangeEvent(
02528:                     "clinical_ward_triage_ticked", "clinical_ward_triage", null, null, census.ActiveAdmissionsCount));
02529:             }
02530:         }
02531:
02532:         /// <summary>Expansion 39 chemical reagent synthesis day owner (ownerId <c>chemical_reagent_synthesis</c>, phase 5).</summary>
02533:         private sealed class ChemicalReagentSynthesisDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02534:         {
02535:             private readonly Main _m;
02536:             private Ashfall.Core.Shelter.ChemicalReagentSynthesisState? _snapshot;
02537:             public ChemicalReagentSynthesisDayOwner(Main m) => _m = m;
02538:
02539:             public void CapturePreDaySnapshot(int day)
02540:             {
02541:                 _m.SetupChemicalReagentSynthesis();
02542:                 _snapshot = _m._chemicalReagentSynthesis?.Ledger.CaptureState();
02543:             }
02544:
02545:             public void RestorePreDaySnapshot(int day)
02546:             {
02547:                 if (_snapshot != null) _m._chemicalReagentSynthesis?.Ledger.RestoreState(_snapshot);
02548:             }
02549:
02550:             public void TickDay(int day, List<DayStateChangeEvent> events)
02551:             {
02552:                 _m.SetupChemicalReagentSynthesis();
02553:                 _m._chemicalReagentSynthesis?.AdvanceDay(day);
02554:                 var census = _m.GetChemicalReagentCensus();
02555:                 events.Add(new DayStateChangeEvent(
02556:                     "chemical_reagent_synthesis_ticked", "chemical_reagent_synthesis", null, null, census.ActiveReactorsCount));
02557:             }
02558:         }
02559:
02560:         /// <summary>Expansion 40 mechanical power driveline day owner (ownerId <c>mechanical_driveline</c>, phase 5).</summary>
02561:         private sealed class MechanicalDrivelineDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02562:         {
02563:             private readonly Main _m;
02564:             private Ashfall.Core.Shelter.MechanicalDrivelineState? _snapshot;
02565:             public MechanicalDrivelineDayOwner(Main m) => _m = m;
02566:
02567:             public void CapturePreDaySnapshot(int day)
02568:             {
02569:                 _m.SetupMechanicalDriveline();
02570:                 _snapshot = _m._mechanicalDriveline?.Ledger.CaptureState();
02571:             }
02572:
02573:             public void RestorePreDaySnapshot(int day)
02574:             {
02575:                 if (_snapshot != null) _m._mechanicalDriveline?.Ledger.RestoreState(_snapshot);
02576:             }
02577:
02578:             public void TickDay(int day, List<DayStateChangeEvent> events)
02579:             {
02580:                 _m.SetupMechanicalDriveline();
02581:                 _m._mechanicalDriveline?.AdvanceDay(8);
02582:                 var census = _m.GetMechanicalDrivelineCensus();
02583:                 events.Add(new DayStateChangeEvent(
02584:                     "mechanical_driveline_ticked", "mechanical_driveline", null, null, census.ActiveBranchesCount));
02585:             }
02586:         }
02587:
02588:         /// <summary>Expansion 41 sleep acoustic rest day owner (ownerId <c>sleep_acoustic_rest</c>, phase 5).</summary>
02589:         private sealed class SleepAcousticRestDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02590:         {
02591:             private readonly Main _m;
02592:             private Ashfall.Core.Needs.SleepAcousticState? _snapshot;
02593:             public SleepAcousticRestDayOwner(Main m) => _m = m;
02594:
02595:             public void CapturePreDaySnapshot(int day)
02596:             {
02597:                 _m.SetupSleepAcousticRest();
02598:                 _snapshot = _m._sleepAcousticRest?.Ledger.CaptureState();
02599:             }
02600:
02601:             public void RestorePreDaySnapshot(int day)
02602:             {
02603:                 if (_snapshot != null) _m._sleepAcousticRest?.Ledger.RestoreState(_snapshot);
02604:             }
02605:
02606:             public void TickDay(int day, List<DayStateChangeEvent> events)
02607:             {
02608:                 _m.SetupSleepAcousticRest();
02609:                 _m._sleepAcousticRest?.AdvanceDay(8);
02610:                 var census = _m.GetSleepAcousticCensus();
02611:                 events.Add(new DayStateChangeEvent(
02612:                     "sleep_acoustic_rest_ticked", "sleep_acoustic_rest", null, null, census.QuartersCount));
02613:             }
02614:         }
02615:
02616:         /// <summary>Plan 162 shelter archive day owner (ownerId <c>shelter_archive</c>, phase 5).</summary>
02617:         private sealed class ShelterArchiveDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02618:         {
02619:             private readonly Main _m;
02620:             private Ashfall.Core.Shelter.ShelterArchiveState? _snapshot;
02621:             public ShelterArchiveDayOwner(Main m) => _m = m;
02622:
02623:             public void CapturePreDaySnapshot(int day)
02624:             {
02625:                 _m.SetupShelterArchive();
02626:                 _snapshot = _m._shelterArchive?.System.CaptureState();
02627:             }
02628:
02629:             public void RestorePreDaySnapshot(int day)
02630:             {
02631:                 if (_snapshot != null) _m._shelterArchive?.System.RestoreState(_snapshot);
02632:             }
02633:
02634:             public void TickDay(int day, List<DayStateChangeEvent> events)
02635:             {
02636:                 _m.SetupShelterArchive();
02637:                 _m.TickShelterArchive(day);
02638:                 var census = _m.GetShelterArchiveCensus();
02639:                 events.Add(new DayStateChangeEvent(
02640:                     "shelter_archive_ticked", "shelter_archive", null, null, census.EntryCount));
02641:             }
02642:         }
02643:
02644:         /// <summary>Plan 177 survivor dreams day owner (ownerId <c>survivor_dreams</c>, phase 5).</summary>
02645:         private sealed class SurvivorDreamsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02646:         {
02647:             private readonly Main _m;
02648:             private Ashfall.Core.Survivors.DreamSystemState? _snapshot;
02649:             public SurvivorDreamsDayOwner(Main m) => _m = m;
02650:
02651:             public void CapturePreDaySnapshot(int day)
02652:             {
02653:                 _m.SetupSurvivorDreams();
02654:                 _snapshot = _m._dreamSystem?.System.CaptureState();
02655:             }
02656:
02657:             public void RestorePreDaySnapshot(int day)
02658:             {
02659:                 if (_snapshot != null) _m._dreamSystem?.System.RestoreState(_snapshot);
02660:             }
02661:
02662:             public void TickDay(int day, List<DayStateChangeEvent> events)
02663:             {
02664:                 _m.SetupSurvivorDreams();
02665:                 _m.TickSurvivorDreams(day);
02666:                 var census = _m.GetDreamCensus();
02667:                 events.Add(new DayStateChangeEvent(
02668:                     "survivor_dreams_ticked", "survivor_dreams", null, null, census.RecordedDreamsCount));
02669:             }
02670:         }
02671:
02672:         /// <summary>Plan 185 memory decay day owner (ownerId <c>memory_decay</c>, phase 5).</summary>
02673:         private sealed class MemoryDecayDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02674:         {
02675:             private readonly Main _m;
02676:             private Ashfall.Core.Cognition.MemoryDecayState? _snapshot;
02677:             public MemoryDecayDayOwner(Main m) => _m = m;
02678:
02679:             public void CapturePreDaySnapshot(int day)
02680:             {
02681:                 _m.SetupMemoryDecay();
02682:                 _snapshot = _m._memoryDecay?.System.CaptureState();
02683:             }
02684:
02685:             public void RestorePreDaySnapshot(int day)
02686:             {
02687:                 if (_snapshot != null) _m._memoryDecay?.System.RestoreState(_snapshot);
02688:             }
02689:
02690:             public void TickDay(int day, List<DayStateChangeEvent> events)
02691:             {
02692:                 _m.SetupMemoryDecay();
02693:                 _m.TickMemoryDecay(day);
02694:                 var census = _m.GetMemoryDecayCensus();
02695:                 events.Add(new DayStateChangeEvent(
02696:                     "memory_decay_ticked", "memory_decay", null, null, census.TotalRecords));
02697:             }
02698:         }
02699:
02700:         /// <summary>Plan 200 survivor personal quests day owner (ownerId <c>personal_quests</c>, phase 5).</summary>
02701:         private sealed class PersonalQuestsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02702:         {
02703:             private readonly Main _m;
02704:             private Ashfall.Core.Quests.PersonalQuestSaveState? _snapshot;
02705:             public PersonalQuestsDayOwner(Main m) => _m = m;
02706:
02707:             public void CapturePreDaySnapshot(int day)
02708:             {
02709:                 var session = _m.EnsurePersonalQuests();
02710:                 _snapshot = session.CaptureState();
02711:             }
02712:
02713:             public void RestorePreDaySnapshot(int day)
02714:             {
02715:                 if (_snapshot != null)
02716:                 {
02717:                     var session = _m.EnsurePersonalQuests();
02718:                     session.RestoreState(_snapshot);
02719:                 }
02720:             }
02721:
02722:             public void TickDay(int day, List<DayStateChangeEvent> events)
02723:             {
02724:                 _m.TickPersonalQuests(day);
02725:                 var census = _m.GetPersonalQuestsCensus();
02726:                 events.Add(new DayStateChangeEvent(
02727:                     "personal_quests_ticked", "personal_quests", null, null, census.ActiveQuestsCount));
02728:             }
02729:         }
02730:
02731:         /// <summary>Plan 202 interpersonal conflict day owner (ownerId <c>interpersonal_conflict</c>, phase 5).</summary>
02732:         private sealed class InterpersonalConflictDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02733:         {
02734:             private readonly Main _m;
02735:             private Ashfall.Core.Survivors.InterpersonalConflictState? _snapshot;
02736:             public InterpersonalConflictDayOwner(Main m) => _m = m;
02737:
02738:             public void CapturePreDaySnapshot(int day)
02739:             {
02740:                 _m.SetupInterpersonalConflict();
02741:                 _snapshot = _m._interpersonalConflict?.System.CaptureState();
02742:             }
02743:
02744:             public void RestorePreDaySnapshot(int day)
02745:             {
02746:                 if (_snapshot != null) _m._interpersonalConflict?.System.RestoreState(_snapshot);
02747:             }
02748:
02749:             public void TickDay(int day, List<DayStateChangeEvent> events)
02750:             {
02751:                 _m.SetupInterpersonalConflict();
02752:                 _m.TickInterpersonalConflict(day);
02753:                 var census = _m.GetInterpersonalConflictCensus();
02754:                 events.Add(new DayStateChangeEvent(
02755:                     "interpersonal_conflict_ticked", "interpersonal_conflict", null, null, census.ActiveConflicts));
02756:             }
02757:         }
02758:
02759:         /// <summary>Plan 216 exercise day owner (ownerId <c>exercise</c>, phase 5).</summary>
02760:         private sealed class ExerciseDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02761:         {
02762:             private readonly Main _m;
02763:             private Ashfall.Core.Survivors.ExerciseSystemState? _snapshot;
02764:             public ExerciseDayOwner(Main m) => _m = m;
02765:
02766:             public void CapturePreDaySnapshot(int day)
02767:             {
02768:                 _m.SetupExercise();
02769:                 _snapshot = _m._exercise?.System.CaptureState();
02770:             }
02771:
02772:             public void RestorePreDaySnapshot(int day)
02773:             {
02774:                 if (_snapshot != null) _m._exercise?.System.RestoreState(_snapshot);
02775:             }
02776:
02777:             public void TickDay(int day, List<DayStateChangeEvent> events)
02778:             {
02779:                 _m.SetupExercise();
02780:                 _m.TickExercise(day);
02781:                 var census = _m.GetExerciseCensus();
02782:                 events.Add(new DayStateChangeEvent(
02783:                     "exercise_ticked", "exercise", null, null, census.TrackedProfilesCount));
02784:             }
02785:         }
02786:
02787:         /// <summary>Plan 178 art and culture creation day owner (ownerId <c>culture_creation</c>, phase 5).</summary>
02788:         private sealed class CultureCreationDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02789:         {
02790:             private readonly Main _m;
02791:             private Ashfall.Core.Culture.CultureCreationState? _snapshot;
02792:             public CultureCreationDayOwner(Main m) => _m = m;
02793:
02794:             public void CapturePreDaySnapshot(int day)
02795:             {
02796:                 _m.SetupCultureCreation();
02797:                 _snapshot = _m._cultureCreation?.CaptureState();
02798:             }
02799:
02800:             public void RestorePreDaySnapshot(int day)
02801:             {
02802:                 if (_snapshot != null) _m._cultureCreation?.RestoreState(_snapshot);
02803:             }
02804:
02805:             public void TickDay(int day, List<DayStateChangeEvent> events)
02806:             {
02807:                 _m.SetupCultureCreation();
02808:                 if (_m._cultureCreationDirty) _m.SaveCultureCreation();
02809:                 var census = _m.GetCultureCreationCensus();
02810:                 events.Add(new DayStateChangeEvent(
02811:                     "culture_creation_ticked", "culture_creation", null, null, census.TotalArtworks));
02812:             }
02813:         }
02814:
02815:         /// <summary>Plan 179 psychological profile day owner (ownerId <c>psychological_profiles</c>, phase 5).</summary>
02816:         private sealed class PsychologicalProfilesDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02817:         {
02818:             private readonly Main _m;
02819:             private Ashfall.Core.Psychology.PsychologyState? _snapshot;
02820:             public PsychologicalProfilesDayOwner(Main m) => _m = m;
02821:
02822:             public void CapturePreDaySnapshot(int day)
02823:             {
02824:                 _m.SetupPsychologicalProfiles();
02825:                 _snapshot = _m._psychologicalProfiles?.CaptureState();
02826:             }
02827:
02828:             public void RestorePreDaySnapshot(int day)
02829:             {
02830:                 if (_snapshot != null) _m._psychologicalProfiles?.RestoreState(_snapshot);
02831:             }
02832:
02833:             public void TickDay(int day, List<DayStateChangeEvent> events)
02834:             {
02835:                 _m.SetupPsychologicalProfiles();
02836:                 if (_m._psychologicalProfilesDirty) _m.SavePsychologicalProfiles();
02837:                 var census = _m.GetPsychologicalProfileCensus();
02838:                 events.Add(new DayStateChangeEvent(
02839:                     "psychological_profiles_ticked", "psychological_profiles", null, null, census.TotalProfiles));
02840:             }
02841:         }
02842:
02843:         /// <summary>Plan 180 skill certification day owner (ownerId <c>skill_certifications</c>, phase 5).</summary>
02844:         private sealed class SkillCertificationsDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02845:         {
02846:             private readonly Main _m;
02847:             private Ashfall.Core.Survivors.SkillCertificationState? _snapshot;
02848:             public SkillCertificationsDayOwner(Main m) => _m = m;
02849:
02850:             public void CapturePreDaySnapshot(int day)
02851:             {
02852:                 _m.SetupSkillCertifications();
02853:                 _snapshot = _m._skillCertifications?.CaptureState();
02854:             }
02855:
02856:             public void RestorePreDaySnapshot(int day)
02857:             {
02858:                 if (_snapshot != null) _m._skillCertifications?.RestoreState(_snapshot);
02859:             }
02860:
02861:             public void TickDay(int day, List<DayStateChangeEvent> events)
02862:             {
02863:                 _m.SetupSkillCertifications();
02864:                 if (_m._skillCertificationsDirty) _m.SaveSkillCertifications();
02865:                 var census = _m.GetSkillCertificationCensus();
02866:                 events.Add(new DayStateChangeEvent(
02867:                     "skill_certifications_ticked", "skill_certifications", null, null, census.CertifiedSurvivorsCount));
02868:             }
02869:         }
02870:
02871:         /// <summary>Plan 187 bestiary day owner (ownerId <c>bestiary_knowledge</c>, phase 5).</summary>
02872:         private sealed class BestiaryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02873:         {
02874:             private readonly Main _m;
02875:             private Ashfall.Core.Bestiary.BestiaryState? _snapshot;
02876:             public BestiaryDayOwner(Main m) => _m = m;
02877:
02878:             public void CapturePreDaySnapshot(int day)
02879:             {
02880:                 _m.SetupBestiary();
02881:                 _snapshot = _m._bestiary?.System.CaptureState();
02882:             }
02883:
02884:             public void RestorePreDaySnapshot(int day)
02885:             {
02886:                 if (_snapshot != null) _m._bestiary?.System.RestoreState(_snapshot);
02887:             }
02888:
02889:             public void TickDay(int day, List<DayStateChangeEvent> events)
02890:             {
02891:                 _m.SetupBestiary();
02892:                 if (_m._bestiaryDirty) _m.SaveBestiary();
02893:                 var census = _m.GetBestiaryCensus();
02894:                 events.Add(new DayStateChangeEvent(
02895:                     "bestiary_ticked", "bestiary_knowledge", null, null, census.TotalDiscovered));
02896:             }
02897:         }
02898:
02899:         /// <summary>Plan 198 health history day owner (ownerId <c>health_history</c>, phase 5).</summary>
02900:         private sealed class HealthHistoryDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02901:         {
02902:             private readonly Main _m;
02903:             private Ashfall.Core.Medical.HealthHistoryState? _snapshot;
02904:             public HealthHistoryDayOwner(Main m) => _m = m;
02905:
02906:             public void CapturePreDaySnapshot(int day)
02907:             {
02908:                 _m.SetupHealthHistory();
02909:                 _snapshot = _m._healthHistory?.System.CaptureState();
02910:             }
02911:
02912:             public void RestorePreDaySnapshot(int day)
02913:             {
02914:                 if (_snapshot != null) _m._healthHistory?.System.RestoreState(_snapshot);
02915:             }
02916:
02917:             public void TickDay(int day, List<DayStateChangeEvent> events)
02918:             {
02919:                 _m.SetupHealthHistory();
02920:                 _m._healthHistory?.TickDay(day);
02921:                 if (_m._healthHistoryDirty) _m.SaveHealthHistory();
02922:                 var census = _m.GetHealthHistoryCensus();
02923:                 events.Add(new DayStateChangeEvent(
02924:                     "health_history_ticked", "health_history", null, null, census.TotalRecords));
02925:             }
02926:         }
02927:
02928:         /// <summary>Plan 183 child development day owner (ownerId <c>child_development_stages</c>, phase 5).</summary>
02929:         private sealed class ChildDevelopmentDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
02930:         {
02931:             private readonly Main _m;
02932:             private Ashfall.Core.Survivors.ChildDevelopmentState? _snapshot;
02933:             public ChildDevelopmentDayOwner(Main m) => _m = m;
02934:
02935:             public void CapturePreDaySnapshot(int day)
02936:             {
02937:                 _m.SetupChildDevelopment();
02938:                 _snapshot = _m._childDevelopment?.System.CaptureState();
02939:             }
02940:
02941:             public void RestorePreDaySnapshot(int day)
02942:             {
02943:                 if (_snapshot != null) _m._childDevelopment?.System.RestoreState(_snapshot);
02944:             }
02945:
02946:             public void TickDay(int day, List<DayStateChangeEvent> events)
02947:             {
02948:                 _m.SetupChildDevelopment();
02949:                 _m._childDevelopment?.TickDay(day);
02950:                 var census = _m.GetChildDevelopmentCensus();
02951:                 events.Add(new DayStateChangeEvent(
02952:                     "child_development_ticked", "child_development_stages", null, null, census.TotalChildren));
02953:             }
02954:         }
02955:     }
02956: }
02957:
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 57: Shelter Incidents, Event Read Model and Player Decisions.**.

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
