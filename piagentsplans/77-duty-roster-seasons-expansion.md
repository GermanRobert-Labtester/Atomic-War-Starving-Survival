# Plan 77 — Duty Roster Season Windows and Temporal Workload Projection

> **Rebuild status:** COMPLETE 8-SEASON CATALOG — TEMPORAL INTEGRATION AND BALANCE MAINTENANCE
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

- The current catalog spans day 0 through 365 with eight named windows and explicit encounter weight and steam-trip chance boost. The duty roster host/save owner re-resolves the active season from the current day rather than persisting a duplicate season state.
- The safe route is `duty_roster_seasons.json` → `DutyRosterCatalog` → `GetSeasonForDay` → roster/encounter/steam-trip consumers → existing duty-roster save.
- The plan should focus on boundary correctness, consumer semantics and authored balance rather than adding more seasons or inventing a global weather calendar.

**Bounded outcome:** Retire the 1→8 data-only implementation premise. The current season catalog has 8 contiguous, non-overlapping windows and focused tests prove day selection, boundaries, overflow fallback and save re-resolution. The rebase protects temporal semantics and prevents a second season calendar.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `duty_roster_seasons.json` is present with 8 unique seasons and snake_case window fields.
- `DutyRosterCatalog` selects the highest matching lower-bound window; `DutyRosterHostSession` exposes catalog count and save state; `DutyRosterSeasonCatalogTests` covers exact 8, contiguity, boundaries, overflow and restore.
- The existing season windows are campaign cadence data; weather and chapter systems have separate owners and are not overwritten by this catalog.
- Current tests are executable evidence only after a fresh focused run.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 cadence and fact projection guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 1→8 target with an 8-window current census.
- Document boundary, overflow, negative-day and save re-resolution behavior.
- Trace encounter weight and steam-trip boost to current consumers without duplicating their state.
- Require future season rows to preserve contiguous, non-overlapping campaign coverage.

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
| season definitions and day selection | DutyRosterCatalog | `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs` | Sole season catalog authority. |
| roster state and temporal work projection | DutyRosterSystem | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | Owns roster state, not authored season rows. |
| host projection and save composition | DutyRosterHostSession | `src/Host/DutyRosterHostSession.cs; src/Main.DutyRoster.cs` | Composes current day and save. |
| window and restore proof | Duty roster season tests | `Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs` | Executable current contract. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Duty Roster Season Windows and Temporal Workload Projection
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ DutyRosterCatalog
│   season definitions and day selection
│ DutyRosterSystem
│   roster state and temporal work projection
│ DutyRosterHostSession
│   host projection and save composition
│ Duty roster season tests
│   window and restore proof
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

1. **Preserve current state ownership.** DutyRosterCatalog owns season definitions and day selection: Sole season catalog authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| season definitions and day selection | DutyRosterCatalog | `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs` | Sole season catalog authority. |
| roster state and temporal work projection | DutyRosterSystem | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | Owns roster state, not authored season rows. |
| host projection and save composition | DutyRosterHostSession | `src/Host/DutyRosterHostSession.cs; src/Main.DutyRoster.cs` | Composes current day and save. |
| window and restore proof | Duty roster season tests | `Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs` | Executable current contract. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load season definitions
2. read current campaign day
3. select the matching window by stable lower-bound order
4. apply encounter/steam modifiers through their current consumers
5. project roster/season status
6. capture day/roster state and re-select on restore

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Season rows are immutable definitions; active season is derived from the current day.
- Window boundaries are inclusive and deterministic; no overlapping row is silently selected.
- Overflow after the final window follows the current fallback contract; negative day has no active season.
- Restore stores the day/roster state and re-derives the active season rather than persisting a shadow season.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every day in the supported campaign range resolves to the intended one season.
- Boundary days are deterministic and not off by one.
- A malformed overlap is a validation failure, not a UI warning.
- A restored day and catalog produce the same active season as an uninterrupted run.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `duty_roster_seasons.json` is the sole duty-season authority.
- No duplicate weather or chapter season data is introduced.
- New rows require contiguous coverage, valid ranges and a consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing duty-roster save section.
- No new season save section is justified.
- The current day remains the save authority; active season is recomputed.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Selection is ordered and deterministic.
- No random or wall-clock input affects the active window.
- Save/restore and continuous runs produce identical projections.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- The current day owner advances the roster clock.
- Roster/encounter consumers read the derived season; the catalog emits no gameplay mutation event.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/DutyRosterHostSession.cs
- src/Main.DutyRoster.cs
- src/UI/DutyRosterPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Season names and descriptions should communicate campaign pressure without real-world geopolitical claims.
- The catalog is a cadence tool; narrative events remain in their owning narrative systems.
- A season transition should be legible through existing briefing/roster surfaces.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Overlapping or gapped windows pass unnoticed. | DutyRosterCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | The UI caches a different active season than the host. | DutyRosterSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A save persists a stale season and contradicts the day. | DutyRosterHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A new season changes weather or chapter authority. | Duty roster season tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A boundary day maps to the wrong window. | DutyRosterCatalog | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current season census | Read catalog, selector, host and tests. | 8 contiguous windows are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — boundary matrix | Test 0, each transition, 365, overflow and negative day. | Selection is exact. | No production path until the owning implementation package is separately claimed. |
| 2 — consumer audit | Trace encounter and steam-trip consumers. | No duplicate temporal authority exists. | No production path until the owning implementation package is separately claimed. |
| 3 — save/replay proof | Compare continuous and restored roster projections. | Results match. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/duty_roster_seasons.json | READ ONLY; MODIFY only for a proven calendar gap | 8-row authority |
| Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs | READ ONLY | Selector |
| src/Host/DutyRosterHostSession.cs | READ ONLY | Host projection |
| src/Main.DutyRoster.cs | READ ONLY | Current save/host seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second season calendar. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using an inclusive/exclusive boundary inconsistently. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Persisting derived active-season state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing weather or chapter behavior under a roster plan. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new season count.
- No weather calendar rewrite.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future calendar edits retain the previous valid JSON and boundary tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 8 current windows and their boundaries are documented.
- Derived season semantics are explicit.
- Save/replay and host projection contracts are named.
- No parallel temporal authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 1→8 target with an 8-window current census.
- Document boundary, overflow, negative-day and save re-resolution behavior.
- Trace encounter weight and steam-trip boost to current consumers without duplicating their state.
- Require future season rows to preserve contiguous, non-overlapping campaign coverage.

## MUST NOT DO

- No new season count.
- No weather calendar rewrite.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current season census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: season definitions and day selection → DutyRosterCatalog; roster state and temporal work projection → DutyRosterSystem; host projection and save composition → DutyRosterHostSession; window and restore proof → Duty roster season tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 77.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 77 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by DutyRosterCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 226 lines / 8734 bytes.
- SHA-256: `3ebf7993c0f39727ac172c61f951a2ce249770ec22927b1588d3e71acd625c2d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DutyRosterLocationEntry
public string id;
public string displayName;
public string inspect;
public string description;
public float dangerLevel;
public float travelHours;
public float baseRadsPerHour;
public string region;
public bool overlay_on_unlock;
public bool recast_always;
public class DutyRosterQuestStageEntry
public string id;
public string text;
public class DutyRosterQuestChoiceEntry
public string id;
public string text;
public string set_flag;
public class DutyRosterQuestEntry
public string id;
public string display_name;
public string type;
public string briefing;
public string prereq_quest_id;
public int min_day;
public DutyRosterQuestStageEntry[] stages;
public DutyRosterQuestChoiceEntry[] choices;
public string knowledge_key;
public string target_location_id;
public string complete_mutation;
public string fail_mutation;
public int StageCount => stages != null ? stages.Length : 0;
public class DutyRosterMarkEntry
public string id;
public string later;
public string situation;
public class DutyRosterSeasonEntry
public string id = DutyRosterIds.SeasonSecondWinter;
public int windowMinDays = DutyRosterIds.SecondWinterWindowMinDays;
public int windowMaxDays = DutyRosterIds.SecondWinterWindowMaxDays;
public float encounterWeight = DutyRosterIds.SecondWinterEncounterWeight;
public float steamTripChanceBoost;
public sealed class DutyRosterCatalog
public List<DutyRosterLocationEntry> Locations { get; } = new List<DutyRosterLocationEntry>();
public List<DutyRosterQuestEntry> Quests { get; } = new List<DutyRosterQuestEntry>();
public List<DutyRosterMarkEntry> Marks { get; } = new List<DutyRosterMarkEntry>();
public List<DutyRosterSeasonEntry> Seasons { get; } = new List<DutyRosterSeasonEntry>();
public DutyRosterLocationEntry? GetLocation(string id) {
public DutyRosterQuestEntry? GetQuest(string id) {
public DutyRosterMarkEntry? GetMark(string id) {
public DutyRosterSeasonEntry? GetSeason(string id) {
public DutyRosterSeasonEntry? GetSeasonForDay(int day) {
public sealed class DutyRosterCatalogLoader
public const string LocationsFile = "duty_roster_locations.json";
public const string QuestsFile = "duty_roster_quests.json";
public const string MarksFile = "duty_roster_marks.json";
public const string SeasonsFile = "duty_roster_seasons.json";
public DutyRosterCatalog Load(string dataDirectory) {
public static readonly IReadOnlyDictionary<string, string> SeasonKeyAliases = new Dictionary<string, string> {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1161 lines / 49527 bytes.
- SHA-256: `6223ad74f06bae54742b4bce7ac1d52573173884dd8fef9d3de261cca872c54d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=14; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DutyRosterRow
public string survivorId;
public string displayName;
public string occupationObserved;
public string status;
public string script;
public int lastSleptDay = -1;
public DutyRosterRow Clone() {
public class DutyRosterAssignmentEntry
public string role;
public string survivorId;
public bool fitnessWarningAcknowledged;
public int fitnessWarningDay = -1;
public List<string> fitnessWarningReasons = new List<string>();
public class DutyRosterWatchShift
public string shift_id = string.Empty;
public string post_id = string.Empty;
public string survivorId = string.Empty;
public int day = -1;
public int start_hour = 0;
public int duration_hours = 1;
public int fatigue_before_permille = 0;
public int fatigue_after_permille = 0;
public string fatigue_tier = "Alert";
public bool completed = false;
public class DutyRosterPneumaticMemo
public string memoId;
public string targetRoomId;
public int deliveredDay;
public int expiresDay;
public float shiftEfficiencyBonus;
public class DutyRosterOccupant
public string survivorId;
public string displayName;
public string occupationObserved;
public bool sleptHere;
public class DutyRosterSystemState
public string systemId = DutyRosterIds.SystemId;
public bool expansionUnlocked;
public bool wallInspected;
public string chartScript = DutyRosterIds.ScriptBlank;
public bool kessPencilAllowed;
public bool waitInk;
public bool blankRowsAccess = true;
public bool mutationRosterInUse;
public bool mutationRosterStillBlank;
public bool mutationRosterBurned;
public bool mutationRationProtocol;
public string endingId;
public bool secondWinterActive;
public int seedSalt = DutyRosterIds.SeedUtilityOffset;
public int lastMorningDay = -1;
public int daysLeftBlank;
public int lastBurnDay = -1;
public bool overflowAccess;
public List<string> overflowVisited = new List<string>();
public List<DutyRosterRow> rows = new List<DutyRosterRow>();
public List<DutyRosterAssignmentEntry> assignments = new List<DutyRosterAssignmentEntry>();
public List<DutyRosterWatchShift> watch_shifts = new List<DutyRosterWatchShift>();
public int watch_schema_version = 1;
public List<DutyRosterPneumaticMemo> pneumaticMemos = new List<DutyRosterPneumaticMemo>();
public List<string> hiddenFromNorth = new List<string>();
public List<string> blankRowsLivingNames = new List<string>();
public class DutyRosterSystem
public const string SystemId = DutyRosterIds.SystemId;
public const string ExpansionId = DutyRosterIds.ExpansionId;
public const string FlagExpUnlocked = DutyRosterIds.FlagExpUnlocked;
public const string LocStackRosterWall = DutyRosterIds.LocStackRosterWall;
public const string LocStackSleeping = DutyRosterIds.LocStackSleeping;
public const string LocStackMess = DutyRosterIds.LocStackMess;
public const string LocStackFiltration = DutyRosterIds.LocStackFiltration;
public const string LocStackAirlock = DutyRosterIds.LocStackAirlock;
public const string LocStackClinicAlcove = DutyRosterIds.LocStackClinicAlcove;
public const string QuestTheChart = DutyRosterIds.QuestTheChart;
public const string QuestWhoEats = DutyRosterIds.QuestWhoEats;
public const string QuestFourteenth = DutyRosterIds.QuestFourteenth;
public const string QuestCaretaker = DutyRosterIds.QuestCaretaker;
public const string QuestTheColumn = DutyRosterIds.QuestTheColumn;
public const string QuestTheTin = DutyRosterIds.QuestTheTin;
public const string QuestQuiet = DutyRosterIds.QuestQuiet;
public const string QuestSole = DutyRosterIds.QuestSole;
public const string QuestWindow = DutyRosterIds.QuestWindow;
public const string QuestInk = DutyRosterIds.QuestInk;
public const string NpcKessAdler = DutyRosterIds.NpcKessAdler;
public const string NpcAnselDuth = DutyRosterIds.NpcAnselDuth;
public const string NpcHadiMorrow = DutyRosterIds.NpcHadiMorrow;
public const string NpcTamsinRook = DutyRosterIds.NpcTamsinRook;
public const string NpcLenQuill = DutyRosterIds.NpcLenQuill;
public const string NpcNilaBrant = DutyRosterIds.NpcNilaBrant;
public const string ChoiceWritePencil = DutyRosterIds.ChoiceWritePencil;
public const string ChoiceLeaveBlank = DutyRosterIds.ChoiceLeaveBlank;
public const string ChoiceWaitInk = DutyRosterIds.ChoiceWaitInk;
public const string ChoiceLadleChild = DutyRosterIds.ChoiceLadleChild;
public const string ChoiceLadleHatch = DutyRosterIds.ChoiceLadleHatch;
public const string ChoiceLadleLeave = DutyRosterIds.ChoiceLadleLeave;
public const string ChoiceLadleProtocol = DutyRosterIds.ChoiceLadleProtocol;
public const string ScriptBlank = DutyRosterIds.ScriptBlank;
public const string ScriptPencil = DutyRosterIds.ScriptPencil;
public const string ScriptInk = DutyRosterIds.ScriptInk;
public const string ScriptBurned = DutyRosterIds.ScriptBurned;
public const string StatusHome = DutyRosterIds.StatusHome;
public const string StatusLevy = DutyRosterIds.StatusLevy;
public const string StatusWaystation = DutyRosterIds.StatusWaystation;
public const string StatusQuiet = DutyRosterIds.StatusQuiet;
public const string StatusMissing = DutyRosterIds.StatusMissing;
public const string StatusDead = DutyRosterIds.StatusDead;
public const string RoleNightWatch = DutyRosterIds.RoleNightWatch;
public const string RoleMess = DutyRosterIds.RoleMess;
public const string RoleHatchOpener = DutyRosterIds.RoleHatchOpener;
public const string RoleIntakeSleeper = DutyRosterIds.RoleIntakeSleeper;
public const string RoleExpedition = DutyRosterIds.RoleExpedition;
public const string MutationRosterInUse = DutyRosterIds.MutationRosterInUse;
public const string MutationRosterStillBlank = DutyRosterIds.MutationRosterStillBlank;
public const string MutationRationProtocol = DutyRosterIds.MutationRationProtocol;
public const string MutationRosterBurned = DutyRosterIds.MutationRosterBurned;
public const string MutationRosterInk = DutyRosterIds.MutationRosterInk;
public const string MutationRosterBlank = DutyRosterIds.MutationRosterBlank;
public const string MutationFactionBlankRowsAccess = DutyRosterIds.MutationFactionBlankRowsAccess;
public const string FlagWaitInk = DutyRosterIds.FlagWaitInk;
public const string EndingInk = DutyRosterIds.EndingInk;
public const string EndingPencil = DutyRosterIds.EndingPencil;
public const string EndingBlank = DutyRosterIds.EndingBlank;
public const string EndingBurned = DutyRosterIds.EndingBurned;
public const string EndingSecondWinter = DutyRosterIds.EndingSecondWinter;
public const string SeasonSecondWinter = DutyRosterIds.SeasonSecondWinter;
public const int SecondWinterWindowMinDays = DutyRosterIds.SecondWinterWindowMinDays;
public const int SecondWinterWindowMaxDays = DutyRosterIds.SecondWinterWindowMaxDays;
public const float SecondWinterEncounterWeight = DutyRosterIds.SecondWinterEncounterWeight;
public const int ManifestCap = DutyRosterIds.ManifestCap;
public const int SoftGateDay = DutyRosterIds.SoftGateDay;
public const int StillBlankDays = DutyRosterIds.StillBlankDays;
public const int SeedUtilityOffset = DutyRosterIds.SeedUtilityOffset;
public static readonly string[] StackWingIds = DutyRosterIds.StackWingIds;
public const string LocOverflowAlloc11 = DutyRosterIds.LocOverflowAlloc11;
public const string LocOverflowAlloc13 = DutyRosterIds.LocOverflowAlloc13;
public const string LocOverflowPumpHatch = DutyRosterIds.LocOverflowPumpHatch;
public const string LocOverflowBlankCellar = DutyRosterIds.LocOverflowBlankCellar;
public static readonly string[] OverflowNodeIds = DutyRosterIds.OverflowNodeIds;
public static readonly string[] AssignmentRoles = DutyRosterIds.AssignmentRoles;
public event Action OnRosterUpdated;
public event Action<string> OnNameWritten;
public event Action<string> OnNameErased;
public event Action OnRosterBurned;
public event Action<string, string> OnAssignmentChanged;
public event Action<string, string> OnDutyVacated;
public event Action<DutyRosterSystemState> OnStateChanged;
public DutyRosterSystemState State => _state;
public bool IsUnlocked => _state.expansionUnlocked;
public string ChartScript => _state.chartScript;
public bool BlankRowsAccess => _state.blankRowsAccess;
public bool MutationInUse => _state.mutationRosterInUse;
public int OccupiedRowCount => _state.rows != null ? _state.rows.Count : 0;
public IReadOnlyList<DutyRosterRow> Rows => _state.rows;
public RoleFitnessVerdict? PreviewRoleFitness(string survivorId, string role) {
public CrewConsentVerdict? PreviewCrewConsent(string survivorId, string role) {
public void Initialise(int seedSalt) {
public void Unlock(int day) {
public void NotifyWallInspected() {
public bool CanBeginChart(int day, bool loreAllocationWrongness, bool holdfastClerkStarted) {
public DutyRosterRow GetRow(string survivorId) {
public bool WriteName( string survivorId, string displayName, string occupationObserved, string script, int day,
public bool EraseName(string survivorId) {
public bool BurnChart(int day) {
public void TickMorning(int day, IReadOnlyList<DutyRosterOccupant> occupants) {
public bool ResolveChartChoice(string choiceId, int day) {
public bool ResolveLadleChoice(string choiceId, int day) {
public bool ResolveInkEnding(int day) {
public bool SetStatus(string survivorId, string status) {
public bool SetRowScript(string survivorId, string script) {
public void SetSecondWinterActive(bool active) {
public bool IsSecondWinterActive => _state.secondWinterActive;
public bool Assign(string role, string survivorId) {
public ActionResult AssignWatchShift( string shiftId, string postId, string survivorId, int day, int startHour,
public ActionResult CompleteWatchShift(string shiftId, int fatigueAfterPermille, int? restQualityPermille = null) {
public IReadOnlyList<DutyRosterWatchShift> GetWatchShifts(int? day = null) {
public int GetWatchShiftCount(string postId, int day, bool completedOnly = false) {
public int GetAverageWatchFatigue(string postId, int day) {
public DutyRosterWatchShift? FindWatchShift(string shiftId) {
public Func<string, DutyHourSnapshot>? DutyHourResolver { get; set; }
public DutyHourSnapshot? PreviewDutyHours(string survivorId) => DutyHourResolver?.Invoke(survivorId);
```


# Appendix B.04 — Current Code Architecture: `src/Host/DutyRosterHostSession.cs`

### `src/Host/DutyRosterHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 440 lines / 19490 bytes.
- SHA-256: `3beb81558d909f0dc287c797f1b8f5c188a99ecfdc4c833f470aabc19056f658`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DutyRosterHostSession
public const int DefaultSeed = 908; // roster seed offset: _worldSeed + 1208 style
public DutyRosterSystem Roster { get; }
public MoraleMarkSystem Marks { get; }
public ShelterEncounterSystem Encounters { get; }
public DutyRosterQuestRuntime Quests { get; }
public DutyRosterCatalog Catalog { get; }
public SimClock Clock { get; }
public string LastEvent { get; private set; } = string.Empty;
public int LocationCount => Catalog.Locations.Count;
public int QuestCount => Catalog.Quests.Count;
public int MarkCount => Catalog.Marks.Count;
public int SeasonCount => Catalog.Seasons.Count;
public RoleFitnessVerdict? PreviewRoleFitness(string survivorId, string roleId) => Roster.PreviewRoleFitness(survivorId, roleId);
public DutyHourSnapshot? PreviewDutyHours(string survivorId) => Roster.PreviewDutyHours(survivorId);
public static DutyRosterHostSession Create(string dataDirectory, ILog? log = null, Ashfall.Core.Journal.JournalSystem journal = null!) {
public void Unlock(int day) {
public DutyRosterSave CaptureSave() =>
public void RestoreSave(DutyRosterSave save) =>
public bool SaveState() {
public string StartRosterQuest(string questId) {
public string AdvanceRosterQuest(string questId) {
public string ResolveRosterChoice(string questId, string choiceId) {
public string ActiveQuestProse(string questId) {
public string QuestsLine() {
public string TickDay() {
public void SyncDay(int day) {
public void DrainDayEvents(List<DayStateChangeEvent> target) {
public string TickDay(IReadOnlyList<DutyRosterOccupant> occupants) {
public void SyncHoldfastToDuty( CensusClaimSystem census, IceRoadSystem iceRoad, WaystationSystem waystation, BrineWaterSystem brine, int day)
public DutyRosterHoldfastSnapshot SnapshotForHoldfast() {
public string InspectWall() {
public string ResolveChart(string choiceId) {
public string ResolveInk() {
public string BurnChart() {
public string QueueVisitor(string visitorId) {
public string StartEncounter(string kind) {
public string ActivateSecondWinter() {
public string GrantOverflowAccess() {
public string RegisterOverflowVisit(string nodeId) {
public string BridgeHatchReturn(string survivorId = null!, bool crisis = false) {
public string GrantBlankRowsAccess() {
public string WallLine() {
public string EncountersLine() {
public string MarksLine() {
public string CatalogLine() {
public CommandResult AssignDuty(string role, string survivorId, bool confirmFitnessWarning = false) {
```


# Appendix B.05 — Current Code Architecture: `src/Main.DutyRoster.cs`

### `src/Main.DutyRoster.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 222 lines / 8335 bytes.
- SHA-256: `642d99ce3aa72601d8b4e170646bd56dd44bea8850f2db0400d69c61afabe1e3`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix B.06 — Current Code Architecture: `src/UI/DutyRosterPanel.cs`

### `src/UI/DutyRosterPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 723 lines / 33788 bytes.
- SHA-256: `a4be6bbae22dcfba03eef87efcf0d39e1527f83d69959051357088c220f21e09`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=8; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class DutyRosterPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action? OnAssignmentChanged;
public event Action<string>? OnRoleSelected;
public event Action? OnDetailsRequested;
public bool StatusStripNonEmpty() => !string.IsNullOrEmpty(_host?.LastEvent);
public bool IsBound => _host != null;
public void Bind(DutyRosterHostSession host, SurvivorsHostSession? survivors = null) {
public void Unbind() {
public override void _Ready() {
public void RefreshView() {
internal static List<AshfallDataGrid.Row> BuildFixtureRows() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/duty_roster_seasons.json`

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


# Appendix D.08 — Existing Focused Test Inventory: `Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`

### `Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 245; SHA-256: `f5da3ab28292ef1bc4ef5ae8377d6d08a0dd62bac3caea7c44468778a18e9620`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExact8SeasonsWithValidSchema
Catalog_PreservesSecondWinterIdentityAndValues
Catalog_ContainsAll7NewSeasonsWithCorrectIds
Catalog_ZeroDuplicateIds
Catalog_ContiguousGapFreeAndNoOverlapsAcross365Days
Selection_EveryDayFrom0To365ResolvesExactlyOneSeason
Selection_ExactTransitionBoundaries
Selection_Post365OverflowFallback
Selection_NegativeDayReturnsNull
Selection_LargeDayJumpsResolveCorrectly
SaveRoundTrip_RestoresSimDayAndReResolvesActiveSeasonCleanly
```


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`

### `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 332; SHA-256: `989c97cfb6c166d932224eb0dea10fb1d18a7a5bae992bf14f6d41a6324f4959`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TryLoad_NonExistentFile_ReturnsNullWithoutLoggingError
TryLoad_EmptyOrWhitespaceFile_ReturnsNullWithoutLoggingError
TryLoad_MalformedJson_CatchesException_LogsErrorAndReturnsNull
TryLoad_TruncatedJson_CatchesException_LogsErrorAndReturnsNull
TryLoad_TamperedPayloadChecksumMismatch_CatchesException_LogsErrorAndReturnsNull
TryLoad_MissingChecksum_CatchesException_LogsErrorAndReturnsNull
TryLoad_UnsupportedFutureSaveVersion_CatchesException_LogsErrorAndReturnsNull
TryLoad_InvalidNegativeSaveVersion_CatchesException_LogsErrorAndReturnsNull
TryLoad_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsNull
TrySave_NullState_ReturnsFalseWithoutThrowing
TrySave_FileIoThrowsIOException_CatchesException_LogsErrorAndReturnsFalse
TryRestoreBare_MalformedJson_CatchesException_LogsErrorAndReturnsNull
TryRestoreBare_NullOrWhitespace_ReturnsNullWithoutLoggingError
CaptureBare_NullState_ReturnsEmptyString
RoundTrip_ValidSave_EncodesAndDecodesSuccessfully_IncrementsWriteCount
```


# Appendix E.10 — Supporting Code Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 384 lines / 17537 bytes.
- SHA-256: `7a3bf9432946caed1d300118cabe1b393d45701b46e54c8f5319d69998f0675f`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DutyRosterSave
public const int CurrentSaveVersion = 3;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public DutyRosterSystemState roster = new DutyRosterSystemState();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public DutyRosterQuestState quests = new DutyRosterQuestState();
public string Checksum = string.Empty;
public class DutyRosterOverflowState
public bool access;
public List<string> visitedNodes = new List<string>();
public sealed class DutyRosterSaveV1
public int saveVersion = 1;
public int simDay;
public DutyRosterSystemState roster = new DutyRosterSystemState();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public string Checksum = string.Empty;
public sealed class DutyRosterSaveV2
public int saveVersion = 2;
public int simDay;
public DutyRosterSystemState roster = new DutyRosterSystemState();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public string Checksum = string.Empty;
internal sealed class LegacyDutyRosterSystemStateChecksum
public string systemId = string.Empty;
public bool expansionUnlocked;
public bool wallInspected;
public string chartScript = string.Empty;
public bool kessPencilAllowed;
public bool waitInk;
public bool blankRowsAccess;
public bool mutationRosterInUse;
public bool mutationRosterStillBlank;
public bool mutationRosterBurned;
public bool mutationRationProtocol;
public string endingId = string.Empty;
public bool secondWinterActive;
public int seedSalt;
public int lastMorningDay;
public int daysLeftBlank;
public int lastBurnDay;
public bool overflowAccess;
public List<string> overflowVisited = new List<string>();
public List<DutyRosterRow> rows = new List<DutyRosterRow>();
public List<DutyRosterAssignmentEntry> assignments = new List<DutyRosterAssignmentEntry>();
public List<DutyRosterPneumaticMemo> pneumaticMemos = new List<DutyRosterPneumaticMemo>();
public List<string> hiddenFromNorth = new List<string>();
public List<string> blankRowsLivingNames = new List<string>();
public static LegacyDutyRosterSystemStateChecksum From(DutyRosterSystemState? state) {
internal sealed class LegacyDutyRosterSaveV1Checksum
public int saveVersion;
public int simDay;
public LegacyDutyRosterSystemStateChecksum roster = new LegacyDutyRosterSystemStateChecksum();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public string Checksum = string.Empty;
internal sealed class LegacyDutyRosterSaveV2Checksum
public int saveVersion;
public int simDay;
public LegacyDutyRosterSystemStateChecksum roster = new LegacyDutyRosterSystemStateChecksum();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public string Checksum = string.Empty;
internal sealed class LegacyDutyRosterSaveV3Checksum
public int saveVersion;
public int simDay;
public LegacyDutyRosterSystemStateChecksum roster = new LegacyDutyRosterSystemStateChecksum();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public DutyRosterQuestState quests = new DutyRosterQuestState();
public string Checksum = string.Empty;
public static class DutyRosterSaveCodec
public static DutyRosterSave Capture( DutyRosterSystem roster, MoraleMarkSystem marks, ShelterEncounterSystem encounters, IClock clock, DutyRosterQuestRuntime? quests = null)
public static string Encode(DutyRosterSave save, IJsonSerializer json) {
public static DutyRosterSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( DutyRosterSave save, DutyRosterSystem roster, MoraleMarkSystem marks, ShelterEncounterSystem encounters, IClock clock,
```


# Appendix E.11 — Supporting Code Evidence: `Assets/Ashfall.Core/Campaign/CampaignDaySave.cs`

### `Assets/Ashfall.Core/Campaign/CampaignDaySave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 125 lines / 5541 bytes.
- SHA-256: `41e4178b4b94b7796eeb605865b6f8f0aa93f720733bbe8a7f4af2eefb31bf16`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CampaignDaySave
public const int CurrentSaveVersion = 2;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int lastAdvancedDay = -1;
public int masterSeed = 1986;
public int derivationVersion = 1;
public System.Collections.Generic.Dictionary<string, int> streamPositions = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
public string difficulty_preset_id = string.Empty;
public string Checksum = string.Empty;
public static class CampaignDaySaveCodec
public static CampaignDaySave Encode(CampaignDaySave save, IJsonSerializer json) {
public static string EncodeToString(CampaignDaySave save, IJsonSerializer json) {
public static CampaignDaySave Decode(string jsonText, IJsonSerializer json) {
public int saveVersion;
public int lastAdvancedDay;
public int masterSeed;
public int derivationVersion;
public System.Collections.Generic.Dictionary<string, int> streamPositions = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
public string Checksum = string.Empty;
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`

### `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 393 lines / 15772 bytes.
- SHA-256: `23f329a574d06d8ddda22c32f421f6dcdf6852cea846a86fe2f5628eca4988de`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ShelterEncounterRecord
public string id;
public string kind;      // visitor / hatch / night / meal / crowd / illness
public int dayStarted;
public int dayResolved = -1;
public string payload;
public string visitorId; // e.g. npc_edor_vale, npc_len_quill
public class ShelterEncounterSystemState
public string systemId = ShelterEncounterSystem.SystemId;
public bool expansionUnlocked;
public int seedSalt = ShelterEncounterSystem.SeedOffset;
public int lastEncounterDay = -1;
public int encountersThisNight;
public float encounterWeightMultiplier = 1f;
public int secondWinterActiveSince = -1;
public List<ShelterEncounterRecord> history = new List<ShelterEncounterRecord>();
public List<string> activeVisitorQueue = new List<string>();
public List<string> resolvedIds = new List<string>();
public class ShelterEncounterSystem
public const string SystemId = "shelter_encounter_system";
public const int SeedOffset = 1208;
public const string KindNightSlate = "night_slate";
public const string KindHatchReturn = "hatch_return";
public const string KindMealShort = "meal_short";
public const string KindIntakeSleep = "intake_sleep";
public const string KindLevyAbsence = "levy_absence";
public const string KindIcePack = "ice_pack";
public const string KindEdorStool = "edor_stool";
public const string KindPellMachine = "pell_machine";
public const string KindStackFever = "stack_fever";
public const string KindChildChart = "child_chart";
public const string KindTinAgain = "tin_again";
public const string KindIntercomOffice = "intercom_office";
public const string KindRoadDarkCrowd = "road_dark_crowd";
public const string KindSelaRow = "sela_row";
public const string VisitorEdor = "npc_edor_vale";
public const string VisitorLen = "npc_len_quill";
public const string VisitorPell = "npc_sergeant_pell";
public const string VisitorOffice = "faction_the_office";
public const string VisitorOverflow = "overflow_runner";
public event Action<ShelterEncounterRecord> OnShelterEncounterStarted;
public event Action<ShelterEncounterRecord> OnShelterEncounterResolved;
public event Action<ShelterEncounterSystemState> OnStateChanged;
public ShelterEncounterSystemState State => _state;
public bool IsUnlocked => _state.expansionUnlocked;
public int LastEncounterDay => _state.lastEncounterDay;
public int EncountersThisNight => _state.encountersThisNight;
public IReadOnlyList<string> ActiveVisitorQueue => _visitorQueue;
public void ResetNightCounter(int day) {
public float EncounterWeightMultiplier => _state.encounterWeightMultiplier;
public bool IsSecondWinterActive => _state.secondWinterActiveSince >= 0;
public void SetSecondWinter(float multiplier, int day) {
public void ClearSecondWinter() {
public void Initialise(int seedSalt) {
public void Unlock(int day) {
public bool QueueVisitor(string visitorId, int day) {
public string? PeekVisitor() {
public bool ResolveVisitor(string visitorId) {
public bool StartEncounter(string id, string kind, int day, string? visitorId = null, string? payload = null) {
public bool StartEncounterCrisis(string id, string kind, int day, string? visitorId = null, string? payload = null) {
public bool BridgeHatchReturn(int day, string? survivorId = null, string? payload = null, bool crisis = false) {
public bool ResolveEncounter(string id, int day) {
public bool IsResolved(string id) {
public ShelterEncounterRecord? GetActive(string id) {
public ShelterEncounterSystemState CaptureState() {
public void RestoreState(ShelterEncounterSystemState saved) {
```


# Appendix E.14 — Supporting Code Evidence: `src/Host/CampaignDaySaveStore.cs`

### `src/Host/CampaignDaySaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 2342 bytes.
- SHA-256: `007f06eae3b0a4e4f834cf417a309dda852ac5a44ad52ee2d58f460e24c29e83`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CampaignDaySaveStore
public const string FileName = "campaign_day_save.json";
public const string SectionName = "campaign_day";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCapture(CampaignDaySave state) => s_store.CaptureEncoded(state);
public static CampaignDaySave? TryRestore(string json) => s_store.RestoreEncoded(json);
public static bool TrySave(CampaignDaySave save) => s_store.TrySave(save);
public static CampaignDaySave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(CampaignDaySave save) => s_store.CapturePersisted(save);
```


# Appendix G.15 — Supporting Regression Evidence: `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs`

### `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs`

- Current test declarations: Fact=40, Theory=0, InlineData=0.
- File lines: 899; SHA-256: `b26fda1cfc83c1a39f5a5fca380651331935aca040608b8b62d60d27e1406307`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Ids_StackWingsAndOverflowAreCanonicalSnakeCase
Ids_BlankRowsIsACurrentNotAPower
Catalog_QuestsAndMarksResolve
Save_OldV1MigratesToBlankChartAndClosedOverflow
SoftGate_RequiresDayOrLoreOrInspectOrClerk
Chart_PencilWaitInkEraseBurnAndInvalidTransitions
Chart_InkEndingRequiresPencilScriptAndNotBurned
Morning_UnsleptNameCannotBePenciled
Morning_AbsentAndDeadCannotBeAssigned
Assignments_SurvivorHoldsAtMostOneRole
Assignments_UnknownRoleOrSurvivorRejected
Manifest_FourteenthBunkIsHardCap
BlankRows_WithdrawOnListingAndCanBeRegrantedByPractice
Encounters_OnePerNightAndCrisisOverride
Encounters_VisitorQueueIsSingleFileAndResolvedIdsDedupe
HatchBridge_StagesOneScenePerNightAndNeverTouchesExpeditionNumbers
Determinism_SameSeedSameInputsSameAssignment
Marks_PersistAcrossDaysAndClearOnlyByAuthoredAction
SecondWinter_MultiplierAppliesAndResets
HoldfastToDuty_LevyHonourMarksRowsLevyAndSetsMark
HoldfastToDuty_RefuseQueuesEdorStool
HoldfastToDuty_MembraneAndWaystationSetMarks
HoldfastToDuty_IceRoadDarkSetsMarkAndCrowdEncounter
DutyToHoldfast_SnapshotCarriesChartNorthAndHadiStatus
DutyToHoldfast_LevyValidationFlagsMissingRows
Save_RoundTripsChartMarksEncountersAndOverflow
Save_MissingStateDefaultsAndFutureVersionRejected
Save_ChecksumStableAcrossSerializerRoundTrip
Overflow_ClosedAccessRejectsVisitsAndUnknownNodesNeverBlessed
Quests_StartPrereqMinDayAndCompletion
Quests_ChoiceResolutionIsAuthoredOnly
Quests_SaveRoundTripAndV3Migration
Quests_ChartCompletionPutsRosterInUse
Quests_MarkCompletionsAndUnknownMutations
Quests_CrisisWindowQuestUnlocksExtraEncounters
Quests_AppliedMutationsSurviveSaveRoundTrip
DutyToHoldfast_SnapshotCarriesQuestMutationsAndMarks
Choices_HadiHiddenHidesFromNorthAndSetsMark
Effects_FourteenthClaimedSetsMarkAndIsSaveSafe
MasterSession_RosterWiredAndTicks
```


# Appendix G.16 — Supporting Regression Evidence: `Ashfall.Core.Tests/DutyRosterSaveTests.cs`

### `Ashfall.Core.Tests/DutyRosterSaveTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 230; SHA-256: `0eb651494e6811b00eeba2a192d1bf4326db9bd045fd645d2d7c0bbc1c2f0dc8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RoundTrip_RestoresRosterMarksAndEncounters
Restore_WithQuestsAndOverflowAndAssignments_RestoresAllSubsystems
Restore_NullSave_ThrowsArgumentNullException
Restore_NullOptionalSubsystems_HandledSafely
Restore_SimDayZeroOrNegative_PreservesExistingClockDay
Restore_Idempotent_CanBeCalledRepeatedlyWithoutDuplicatingState
HostSession_RestoreSave_Contract_DelegatesToAllSubsystems
Decode_RejectsTamperedChecksum
Decode_RejectsNewerVersion
```


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/DutyRosterSystemTests.cs`

### `Ashfall.Core.Tests/DutyRosterSystemTests.cs`

- Current test declarations: Fact=32, Theory=0, InlineData=0.
- File lines: 617; SHA-256: `1ab17d40ce3521d39e48f5763da10921b72eab99578dfb33fa18f28baca99608`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
BlankUntilUnlock
ChartGateDayOrInspect
PencilMorningFill
InkNeverAutoFills
FourteenCap
CannotAssignDead
BlankRowsInkWithdrawsAccess
LeaveBlankFortyDays
HiddenOmittedFromNorthCopy
SameSeedSameAssignment
LadleProtocolAndBurn
SaveRoundtrip
PencilRefusesUnsleptName
InkEndingResolves
SecondWinterFlagSurvivesSave
BurnBlocksInkEnding
FlagAndLaterProse
DoesNotClearOnNewDay_OnlyAuthoredClear
SaveRoundtrip
StackWingIdsUniqueSnakeCase
ChartAndLadleQuestsRegistered
SecondWinterSeasonLoaded
FullExpansionCatalogComplete
RosterNpcsPresentInCharactersCatalog
MarksHaveLaterProse
LockedUntilUnlock
OneEncounterPerNight
CrisisAllowsMultiple
VisitorQueueOneAtATime
ResolveOnceOnly
SaveRoundtrip
SecondWinterMultiplierApplies
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/DutyRoster/DutyRosterHostSessionTests.cs`

### `Ashfall.Core.Tests/DutyRoster/DutyRosterHostSessionTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 218; SHA-256: `bdff8e6521f391ef61ecac7c0c4b0dfe46fe1bf0963d7ea50536f62b010b5315`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RestoreSave_RestoresClockRosterMarksAndEncounters
RestoreSave_WithQuestsAndOverflowAndAssignments_RestoresCompleteState
RestoreSave_NullSave_ThrowsArgumentNullException
RestoreSave_Idempotent_CanBeCalledMultipleTimesWithoutDuplication
RestoreSave_SimDayZero_PreservesExistingClockDay
RestoreSave_RoundtripThroughSerializationCodec
```


# Appendix H.19 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| season definitions and day selection | DutyRosterCatalog | roster state and temporal work projection | DutyRosterSystem | Owner emits/reads a typed fact; no mirror state. |
| season definitions and day selection | DutyRosterCatalog | host projection and save composition | DutyRosterHostSession | Owner emits/reads a typed fact; no mirror state. |
| season definitions and day selection | DutyRosterCatalog | window and restore proof | Duty roster season tests | Owner emits/reads a typed fact; no mirror state. |
| roster state and temporal work projection | DutyRosterSystem | season definitions and day selection | DutyRosterCatalog | Owner emits/reads a typed fact; no mirror state. |
| roster state and temporal work projection | DutyRosterSystem | host projection and save composition | DutyRosterHostSession | Owner emits/reads a typed fact; no mirror state. |
| roster state and temporal work projection | DutyRosterSystem | window and restore proof | Duty roster season tests | Owner emits/reads a typed fact; no mirror state. |
| host projection and save composition | DutyRosterHostSession | season definitions and day selection | DutyRosterCatalog | Owner emits/reads a typed fact; no mirror state. |
| host projection and save composition | DutyRosterHostSession | roster state and temporal work projection | DutyRosterSystem | Owner emits/reads a typed fact; no mirror state. |
| host projection and save composition | DutyRosterHostSession | window and restore proof | Duty roster season tests | Owner emits/reads a typed fact; no mirror state. |
| window and restore proof | Duty roster season tests | season definitions and day selection | DutyRosterCatalog | Owner emits/reads a typed fact; no mirror state. |
| window and restore proof | Duty roster season tests | roster state and temporal work projection | DutyRosterSystem | Owner emits/reads a typed fact; no mirror state. |
| window and restore proof | Duty roster season tests | host projection and save composition | DutyRosterHostSession | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 1→8 target with an 8-window current census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Document boundary, overflow, negative-day and save re-resolution behavior. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Trace encounter weight and steam-trip boost to current consumers without duplicating their state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Require future season rows to preserve contiguous, non-overlapping campaign coverage. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.551 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherSystem.cs`

### `Assets/Ashfall.Core/World/WeatherSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 532 lines / 22049 bytes.
- SHA-256: `23309eb8803b1759e8d529078baea0677ee6d24f777601e3023fb1c6c12ea341`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SeasonWindowDef
public string id = string.Empty;
public string displayName = string.Empty;
public int startDay = 0;
public float clearWeight = 1f;
public float rainWeight = 1f;
public float overcastWeight = 1f;
public float ashfallWeight = 1f;
public float falloutStormWeight = 1f;
public float blizzardWeight = 1f;
public float blackRainWeight = 1f;
public class SeasonProfileDef
public string id = "default_winter";
public string displayName = "The Long Winter";
public float weatherCheckIntervalHours = 6f;
public List<SeasonWindowDef> seasons = new List<SeasonWindowDef>();
public class WorldWeatherState
public string systemId = WeatherSystem.SystemId;
public string currentKind = "Clear";
public float totalElapsedHours = 0f;
public float hoursUntilNextCheck = 0f;
public int rollCount = 0;
public bool restrictToNonHazardWeather = false;
public float wind_direction_deg = 0f;
public float wind_speed_kph = 0f;
public class WeatherSystem
public const string SystemId = "world_weather_system";
public const float FalloutStormOutdoorRadModifier = 150f;
public const float BlackRainOutdoorRadModifier = 250f;
public const float BlackRainHazmatMeltMultiplier = 5f;
public const float BlizzardTemperaturePenaltyC = -15f;
public const float FalloutStormTemperaturePenaltyC = -5f;
public const float BlackRainTemperaturePenaltyC = -8f;
public const float BlizzardVisibilityFactor = 0.4f;
public event Action<WeatherKind> OnWeatherChanged;
public event Action<WorldWeatherState> OnStateChanged;
public WorldWeatherState State => _state;
public WeatherKind Current => ParseKind(_state.currentKind);
public void BindWeatherEffects(WeatherEffectsCatalog? catalog) {
public float WindDirectionDeg => _state.wind_direction_deg;
public float WindSpeedKph => _state.wind_speed_kph;
public int Seed => _seed;
public SeasonProfileDef? Profile => _profile;
public void BindProfile(SeasonProfileDef profile, int seed) {
public SeasonWindowDef GetSeasonForDay(int day) {
public void Tick(float gameHours) {
public void ForceWeather(WeatherKind kind) {
public float ForecastRadModifier(WeatherKind kind) {
public bool IsScavengingBlocked(bool hasFullSuit) =>
public float GetTemperaturePenaltyCelsius() {
public float HazmatDegradeMultiplier =>
public static float TemperaturePenaltyForWeather(WeatherKind kind) {
public float TemperaturePenaltyC(WeatherKind kind) {
public float VisibilityModifier(WeatherKind kind) {
public WorldWeatherState CaptureState() {
public void RestoreState(WorldWeatherState saved) {
public void RestrictToNonHazardWeather(bool restrict) {
public List<WeatherForecastEntry> PeekForecast(int daysAhead = 3) {
public class WeatherForecastEntry
public int Day;
public WeatherKind Kind;
public float OutdoorRad;
public float Visibility;
public string Summary = string.Empty;
public float ThermalLoadC;
public float TravelSpeedMultiplier = 1f;
public float TravelEncounterMultiplier = 1f;
public float TrapYieldMultiplier = 1f;
public float CaravanAvailabilityMultiplier = 1f;
public static class WeatherProfileLoader
public const string FileName = "weather_seasons.json";
public static SeasonProfileDef? Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.552 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterHeadlessDemo.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 152 lines / 8353 bytes.
- SHA-256: `d1531704f70acc29eb8a191d5d4d3944c99b4d6708c79833f6561d766a2e7030`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DutyRosterHeadlessDemo
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.553 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherSondeSystem.cs`

### `Assets/Ashfall.Core/World/WeatherSondeSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 691 lines / 31183 bytes.
- SHA-256: `9e662202eb4e737868a9c0b00fda05309187b2987ee2e37453478e3a4a00f274`.
- Architecture signals: seeded references=6; save/restore symbols=2; typed event declarations=17; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SondeTelemetrySample
public int sampleIndex = 0;
public float altitudeKm = 0f;
public float temperatureC = 0f;
public float radiationMsv = 0f;
public float windSpeedKmh = 0f;
public float windDirectionDeg = 0f;
public float humidityPct = 0f;
public bool isLost = false;       // signal lost at this altitude
public class SondeForecastEntry
public int dayOffset = 0;         // 0 = today, 1 = tomorrow, etc.
public string predictedKind = "Clear";
public float confidence = 0f;     // 0..1
public float uncertaintyRadius = 0f; // weather variability
public string hazardTag = "none"; // Plan 71 §6.9: "ashfall" / "fallout_storm" / "none" …
public class WeatherSondeState
public string systemId = WeatherSondeSystem.SystemId;
public string sondeId = string.Empty;
public bool isLaunched = false;
public bool isRecovered = false;
public bool isFailed = false;
public int launchDay = 0;
public float launchHour = 0f;
public int flightDurationTicks = 0;
public int ticksElapsed = 0;
public float batteryLevel = 1.0f;   // 0..1
public float hydrogenLevel = 1.0f;  // 0..1, inflation gas
public float sensorQuality = 1.0f;  // 0..1, degrades with altitude
public float observationQuality = 0f; // 0..1, cumulative quality
public List<SondeTelemetrySample> samples = new List<SondeTelemetrySample>();
public List<SondeForecastEntry> forecast = new List<SondeForecastEntry>();
public string failureReason = string.Empty;
public float currentAltitudeKm = 0f;      // live altitude (ascent or descent)
public bool isBurst = false;              // envelope burst — descent phase
public float burstAltitudeKm = 0f;
public int positionEastingM = 0;          // quantized world meters (Trap H)
public int positionNorthingM = 0;
public float driftEastKm = 0f;            // unquantized drift accumulators (persisted
public float driftNorthKm = 0f;           //  for exact split-run continuation)
public int landingDay = -1;
public int landingEastingM = 0;
public int landingNorthingM = 0;
public float payloadCondition = 0f;       // 0..1 sensor/package state at landing
public bool recoveryTargetSpawned = false;
public int recoveryExpiryDay = -1;
public bool recoveryClaimed = false;
public string payloadId = string.Empty;
public class WeatherSondeSystem
public const string SystemId = "weather_sonde_system";
public const int DefaultFlightDurationTicks = 4;
public const int MaxFlightDurationTicks = 8;
public const float BatteryDrainPerTick = 0.06f;           // ascent drain
public const float BatteryDrainDescentPerTick = 0.03f;      // parachute descent drain
public const float HydrogenDrainPerTick = 0.05f;
public const float SensorDegradationPerKm = 0.02f;
public const float MaxAltitudeKm = 30f;
public const float AltitudePerTickKm = 7.5f;
public const int BaseForecastHorizonDays = 3;
public const int ExtendedForecastHorizonDays = 5;
public const float HighQualityThreshold = 0.7f;
public const float SignalLossChancePerTick = 0.05f;
public const float HoursPerFlightTick = 1f;
public const float MetersPerKm = 1000f;
public const int MaxTelemetrySamples = 64;         // bounded history (§6.16)
public const int RecoveryTargetExpiryDays = 14;    // authored expiration (§6.11)
public const float BurstPayloadDamage = 0.6f;      // payload condition cap after burst
public const float HydrogenCostPerLaunch = 0.5f;
public const float BatteryCostPerLaunch = 0.3f;
public event Action<string> OnLaunchStarted;           // sondeId
public event Action<SondeTelemetrySample> OnTelemetryReceived;
public event Action<SondeTelemetrySample> OnTelemetryLost;
public event Action<string> OnSondeFailed;             // reason
public event Action<string> OnSondeRecovered;          // sondeId
public event Action OnPayloadLanded;                   // Plan 71 §6.11
public event Action<List<SondeForecastEntry>> OnForecastConfidenceChanged;
public event Action<WeatherSondeState> OnStateChanged;
public WeatherSondeState State => _state;
public bool IsLaunched => _state.isLaunched;
public bool IsComplete => _state.isRecovered || _state.isFailed;
public void ApplySoundingCatalog( IReadOnlyList<SoundingAltitudeBandDef>? bands, IReadOnlyList<SoundingPayloadDef>? payloads) {
public void BindRecoveryInventory(Ashfall.Core.Inventory.Inventory? inventory) {
public bool Launch(string sondeId, int day, float hour, float hydrogenAvailable, float batteryAvailable) {
public bool Tick(ISeededRng rng, int day = -1) {
public ActionResult ClaimRecoveryPayload(int currentDay) {
public int GetSampleCount() => _state.samples.Count;
public int GetLostSampleCount() {
public float GetCurrentAltitude() {
public List<SondeForecastEntry> GetForecast() => new List<SondeForecastEntry>(_state.forecast);
public WeatherSondeState CaptureState() {
public void RestoreState(WeatherSondeState saved) {
```


# Appendix Q.554 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix Q.555 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/WeatherStationSystem.cs`

### `Assets/Ashfall.Core/WeatherStationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 339 lines / 14337 bytes.
- SHA-256: `b2e2c915d8f9a33ec3cdbe97518b8bd5e1c7c69468b018a1daa13d2f28ef238d`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=10; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum WeatherStationTier
public sealed class WeatherStationState
public string systemId = WeatherStationSystem.SystemId;
public bool isInstalled;
public bool isCalibrated;
public int installDay = -1;
public int calibrationDay = -1;
public int forecastHorizonDays = 3;
public float accuracy = 0.7f;
public float durability = 100f; // 0 to 100
public bool hasSensorFault;
public string faultReason = string.Empty;
public int lastForecastDay = -1;
public List<ForecastEntry> cachedForecast = new List<ForecastEntry>();
public sealed class ForecastEntry
public int day;
public WeatherKind weather;
public float confidence;
public bool isRouteSafe;
public bool? routeSafety;
public bool? RouteSafety { get => routeSafety; set => routeSafety = value; }
public float temperature;
public string warning = string.Empty;
public string preparationPayoff = string.Empty;
public string atmosphericFlavor = string.Empty;
public sealed class WeatherStationSystem
public const string SystemId = "weather_station";
public WeatherStationState State => _state;
public bool IsOperational => _state.isInstalled && _state.isCalibrated && !_state.hasSensorFault && _state.durability >= 20f;
public int EffectiveHorizonDays => CurrentTier switch
public event Action OnForecastUpdated;
public event Action OnStationStateChanged;
public WeatherGateCatalog? GateCatalog { get; set; }
public ActionResult Install(int day) {
public ActionResult Calibrate(int day) {
public ActionResult Repair(int day, float durabilityAmount) {
public void Degrade(float durabilityAmount) {
public void TriggerSensorFault(string reason) {
public void ClearSensorFault() {
public ActionResult GenerateForecast(int currentDay) => GenerateForecast(currentDay, null);
public ActionResult GenerateForecast(int currentDay, string? routeId) {
public static string GetPreparationPayoff(WeatherKind kind) {
public static string GetAtmosphericFlavor(WeatherKind kind, int day) {
public IReadOnlyList<ForecastEntry> GetForecast() => _state.cachedForecast.AsReadOnly();
public bool IsRouteSafe(int day) {
public bool IsRouteSafe(int day, string? routeId) {
public float GetConfidence(int day) {
public WeatherStationState CaptureState() => CloneState(_state);
public void RestoreState(WeatherStationState saved) {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

### `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 444 lines / 21947 bytes.
- SHA-256: `c5d60671673ea3333c1f484f8cfd293905a42799056208eada0956173951c301`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DeepChainHopSpec
public string HopId { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string RequiredFile { get; set; } = string.Empty;
public string? RequiredLoader { get; set; }
public string[]? RequiredSystems { get; set; }
public string? RequiredSurface { get; set; }
public sealed class DeepChainSpec
public string ChainId { get; set; } = string.Empty;
public string Narrative { get; set; } = string.Empty;
public bool IsHardGate { get; set; }
public DeepChainHopSpec[] Hops { get; set; } = Array.Empty<DeepChainHopSpec>();
public sealed class DeepChainFinding
public string ChainId { get; set; } = string.Empty;
public string HopId { get; set; } = string.Empty;
public string MissingCategory { get; set; } = string.Empty;
public string Details { get; set; } = string.Empty;
public string Severity { get; set; } = "HARD"; // HARD | WARN
public string RecommendedFix { get; set; } = string.Empty;
public sealed class DeepChainReport
public string SchemaVersion { get; set; } = "1.0.0";
public List<DeepChainFinding> Findings { get; set; } = new();
public int ChainsEvaluated { get; set; }
public int HardFailures => Findings.Count(f => f.Severity == "HARD");
public int Warnings => Findings.Count(f => f.Severity == "WARN");
public bool HardGatePassed => HardFailures == 0;
public void Stabilize() =>
public static class ContentDeepChainGate
public static readonly DeepChainSpec ResearchToCraft = new() {
public static readonly DeepChainSpec ExpeditionToUse = new() {
public static readonly DeepChainSpec FactionTreatyBriefing = new() {
public static readonly DeepChainSpec[] WarnTierChains = new[] {
public static IEnumerable<DeepChainSpec> AllChains =>
public static DeepChainReport Evaluate(ContentUtilizationGraph graph) {
```


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterHoldfastBridge.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterHoldfastBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 258 lines / 12284 bytes.
- SHA-256: `45f3fc3da8028dc85c4302b48d694818437309c0db6353af8b47a39e308e49d6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DutyRosterHoldfastSnapshot
public string ChartScript = string.Empty;
public List<DutyRosterRow> NorthRows = new List<DutyRosterRow>();
public List<string> LevyNames = new List<string>();
public bool BlankRowsAccess = true;
public bool OverflowAccess = false;
public string Mutation = string.Empty;
public string HadiStatus = string.Empty;
public List<string> QuestMutations = new List<string>();
public List<string> MarkIds = new List<string>();
public static class DutyRosterHoldfastBridge
public const string MarkThreeAway = "mark_three_away";
public const string MarkLadleDefault = "mark_ladle_default";
public const string MarkEdorStool = "mark_edor_stool";
public const string MarkFilterWho = "mmc_filter_who";
public const string MarkTamsinWatchShort = "mark_tamsin_watch_short";
public const string MarkHouseThinned = "mark_house_thinned";
public const string MarkHadiListed = "mark_hadi_listed";
public const string MarkHadiHidden = "mark_hadi_hidden";
public const string MarkHadiSent = "mark_hadi_sent";
public const string MarkHadiNeverBack = "mark_hadi_never_back";
public const string MarkRosterInk = "mark_roster_ink";
public const string MarkRosterPencil = "mark_roster_pencil";
public const string MarkRosterBlank = "mark_roster_blank";
public const string MarkRosterBurned = "mark_roster_burned";
public const string MarkHomeHeld = "mark_home_held";
public const string MarkUncorroborated = "mark_uncorroborated";
public const string MarkRationProtocol = "mark_ration_protocol";
public const string MarkFourteenthClaimed = "mark_fourteenth_claimed";
public const string MarkFourteenthDenied = "mark_fourteenth_denied";
public const string MarkScheduleLiving = "mark_schedule_living";
public const string MarkColumnVoss = "mark_column_voss";
public const string MarkColumnHidden = "mark_column_hidden";
public const string MarkBrassKept = "mark_brass_kept";
public const string MarkPlateOnWall = "mark_plate_on_wall";
public const int MaxLevyRows = 3;
public static void SyncFromHoldfast( DutyRosterSystem roster, MoraleMarkSystem marks, ShelterEncounterSystem encounters, CensusClaimSystem census, IceRoadSystem iceRoad,
public static DutyRosterHoldfastSnapshot SnapshotForHoldfast(DutyRosterSystem roster) {
public static DutyRosterHoldfastSnapshot SnapshotForHoldfast( DutyRosterSystem roster, DutyRosterQuestRuntime quests, MoraleMarkSystem marks) {
public static List<string> ValidateLevyNamesAgainstRoster(DutyRosterSystem roster, IReadOnlyList<string> censusLevyNames) {
public static void NoteMark(MoraleMarkSystem marks, string id, string payload, int day) {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WorldHeadlessDemo.cs`

### `Assets/Ashfall.Core/World/WorldHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 101 lines / 4040 bytes.
- SHA-256: `a0509788508e8cca91da3ff9b442e7a6490b6b846d8fa4ac4b891327708f0987`.
- Architecture signals: seeded references=0; save/restore symbols=7; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WorldHeadlessDemo
public static HeadlessReport Run(ILog? log = null) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 483 lines / 20748 bytes.
- SHA-256: `a0372f1bf6cea59c8c3de4bba814e219181531b56d7e340a415ad8a01768dd44`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=10; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DutyRosterQuestProgress
public string questId = string.Empty;
public int startedDay = -1;
public int currentStage = -1;
public int completedDay = -1;
public int failedDay = -1;
public bool started;
public bool completed;
public bool failed;
public string chosenChoiceId = string.Empty;
public class DutyRosterQuestState
public string systemId = DutyRosterQuestRuntime.SystemId;
public List<DutyRosterQuestProgress> quests = new List<DutyRosterQuestProgress>();
public List<string> appliedMutations = new List<string>();
public class DutyRosterQuestRuntime
public const string SystemId = "duty_roster_quest_runtime";
public event Action<DutyRosterQuestProgress> OnQuestStarted;
public event Action<DutyRosterQuestProgress> OnQuestStageAdvanced;
public event Action<DutyRosterQuestProgress> OnQuestCompleted;
public event Action<DutyRosterQuestProgress> OnQuestFailed;
public event Action<DutyRosterQuestState> OnStateChanged;
public DutyRosterQuestState State => _state;
public int StartedCount => _byId.Count;
public void BindCatalog(DutyRosterCatalog catalog) {
public DutyRosterQuestProgress? GetProgress(string questId) {
public bool IsStarted(string questId) => GetProgress(questId)?.started == true;
public bool IsComplete(string questId) => GetProgress(questId)?.completed == true;
public bool IsFailed(string questId) => GetProgress(questId)?.failed == true;
public int GetCurrentStage(string questId) => GetProgress(questId)?.currentStage ?? -1;
public List<DutyRosterQuestEntry> GetAvailableQuests(int day) {
public List<DutyRosterQuestEntry> GetActiveQuests() {
public bool StartQuest(string questId, int day) {
public bool AdvanceStage(string questId, int day) {
public bool ResolveChoice(string questId, string choiceId) {
public bool ResolveChoiceWithEffects( string questId, string choiceId, DutyRosterSystem roster, MoraleMarkSystem marks, int day)
public bool FailQuest(string questId, int day) {
public bool HasMutation(string mutationId) {
public IReadOnlyList<string> AppliedMutations => _state.appliedMutations;
public bool IsCrisisQuestActive() {
public void ApplyKnownEffects(DutyRosterSystem roster, MoraleMarkSystem marks, int day, ILog? log = null) {
public DutyRosterQuestState CaptureState() {
public void RestoreState(DutyRosterQuestState saved) {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

### `Assets/Ashfall.Core/ExpansionMasterSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 212 lines / 10386 bytes.
- SHA-256: `568760f21b08597c7cd360ea5f147c07feebc84edac5a7ca2f20addfd7545f0b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionMasterSession
public HoldfastSession Holdfast { get; }
public DutyRosterSystem DutyRoster { get; }
public DutyRosterCatalog DutyRosterData { get; }
public LocationLayoutSystem StandingRecord { get; }
public CrossingSession Crossing { get; }
public SimClock Clock { get; }
public ILog Log { get; }
public SilentFoundrySystem SilentFoundry { get; }
public SilentFoundryCatalog FoundryData { get; }
public DiseaseSystem Disease { get; }
public DiseaseCatalog DiseaseData { get; }
public bool AllExpansionsActive =>
public static ExpansionMasterSession Load(string dataDirectory, int seed =808, ILog? log = null) {
public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant>? homeOccupants = null, IReadOnlyList<string>? diseaseCandidates = null) {
public static HeadlessReport RunAllSelfTests(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

### `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 375 lines / 15683 bytes.
- SHA-256: `3a926de1f7f6e5bb05b4f4bd0a57492ceb92dac2142f13ddd6ca4aa3303529fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseQuarantineCoordinator
public event Action<string, string, int>? OnQuarantineAssigned;
public event Action<string, string, int>? OnQuarantineReleased;
public event Action<int, int, float>? OnDailyBurdenProcessed;
public MedicalWardSystem Ward => _medicalWard;
public DiseaseSystem Disease => _diseaseSystem;
public DutyRosterSystem? Roster => _dutyRoster;
public ContainmentCapability Containment =>
public bool IsIsolated(string survivorId) {
public float GetIsolationQuality(string survivorId) {
public MedicalBed? FindAvailableIsolationBed() {
public QuarantineCommandPreview PreviewAssignIsolation(string survivorId) {
public QuarantineCommandResult ExecuteAssignIsolation(string survivorId, int day) {
public QuarantineCommandPreview PreviewReleaseIsolation(string survivorId) {
public QuarantineCommandResult ExecuteReleaseIsolation(string survivorId, int day) {
public void TickDaily(int day) {
public void Rehydrate() {
public sealed class QuarantineCommandPreview
public bool CanExecute { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string TargetBedId { get; set; } = string.Empty;
public string? ConflictingRole { get; set; }
public float ProjectedIsolationQuality { get; set; } = 1.0f;
public Dictionary<string, int> DailySupplyCost { get; set; } = new Dictionary<string, int>();
public static QuarantineCommandPreview Success(string survivorId, string bedId, string? conflictingRole, float projectedQuality, Dictionary<string, int> costs) =>
public static QuarantineCommandPreview Blocked(string reason, string survivorId) =>
public sealed class QuarantineCommandResult
public bool Success { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string BedId { get; set; } = string.Empty;
public int Day { get; set; }
public static QuarantineCommandResult Ok(string survivorId, string bedId, int day) =>
public static QuarantineCommandResult Fail(string reason, string survivorId, string bedId = "", int day = 0) =>
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterOverflowEngine.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterOverflowEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 91 lines / 3202 bytes.
- SHA-256: `89579c8276c78ddc63d78d0f8d41f5820c1d7cadb46b8cb0a44c5dfda432357c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal class DutyRosterOverflowEngine
public bool Access => _state.overflowAccess;
public IReadOnlyList<string> Visited => _state.overflowVisited;
public void Bind(DutyRosterSystemState state) {
public bool GrantOverflowAccess() {
public bool WithdrawOverflowAccess() {
public bool RegisterOverflowVisit(string nodeId) {
public bool HasVisitedOverflow(string nodeId) {
public DutyRosterOverflowState Capture() {
public void Restore(DutyRosterOverflowState saved) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherGateCatalog.cs`

### `Assets/Ashfall.Core/World/WeatherGateCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 193 lines / 9580 bytes.
- SHA-256: `0c98d6d2a4ea4a13861c1b02144936b177d30145cd44f0915317a707cf2ccd31`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WeatherGateCatalog
public int Count => _gates.Count;
public List<string> Errors { get; } = new List<string>();
public bool IsValid => Errors.Count == 0;
public void Register(WeatherGate gate) {
public void Add(WeatherGate gate) {
public bool TryGet(string gateId, out WeatherGate? gate) {
public WeatherGate? GetById(string id) => TryGet(id, out var g) ? g : null;
public WeatherGate? FindByTargetId(string targetId) {
public WeatherGate? GetByTarget(string target) => FindByTargetId(target);
public IReadOnlyList<WeatherGate> GetAll() {
public IEnumerable<string> AllGateIds => _gates.Keys.OrderBy(k => k, StringComparer.Ordinal);
public static WeatherGateCatalog LoadFromJson(string json, Ashfall.Core.IJsonSerializer? jsonSerializer = null) =>
public static WeatherGateCatalog LoadFromDirectory(string dataDir, Ashfall.Core.IFileIO fileIO, Ashfall.Core.IJsonSerializer? jsonSerializer = null) =>
public sealed class WeatherGateCatalogException : Exception
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherGateRadioHooks.cs`

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


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`

### `Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 355 lines / 15367 bytes.
- SHA-256: `962ab63c433b2bf189343ca17c0807b637189bcc8b1ec4c08d6b82a0725139b5`.
- Architecture signals: seeded references=6; save/restore symbols=10; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WeatherIntelligenceSaveState
public WeatherStationState station = new WeatherStationState();
public OrbitalTelemetryState orbital = new OrbitalTelemetryState();
public SeasonalEventSaveState seasonal = new SeasonalEventSaveState();
public CloudSeedingSaveState? cloudSeeding;
public sealed class WeatherIntelligenceReadModel
public string seasonId = string.Empty;
public string seasonDisplayName = string.Empty;
public bool stationInstalled;
public bool stationCalibrated;
public bool stationOperational;
public WeatherStationTier stationTier;
public string stationTierName = string.Empty;
public float stationAccuracy;
public float stationDurability;
public int forecastHorizonDays;
public int lastForecastDay;
public List<ForecastEntry> forecast = new List<ForecastEntry>();
public bool telemetryActive;
public bool hasPendingImpact;
public int impactDay;
public int warningLeadDays;
public int daysUntilImpact;
public int activeSalvageCount;
public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();
public List<ActiveSeasonalEvent> activeSeasonalEvents = new List<ActiveSeasonalEvent>();
public int routeSafeDays;
public int bestTravelDay;
public float bestTravelConfidence;
public string advisory = string.Empty;
public string? predictedCrisisEventId;
public int predictedCrisisDay;
public float predictedCrisisConfidence;
public string? crisisPreparationAdvice;
public WeatherKind? predictedWeatherKind;
public bool hasPredictedCrisis;
public int daysUntilPredictedCrisis;
public string predictionSource = string.Empty;
public bool isStationCalibrated;
public bool cloudSeedingInstalled;
public bool cloudSeedingOnCooldown;
public int cloudSeedingCooldownDays;
public bool cloudSeedingPartialProtectionActive;
public sealed class WeatherIntelligenceCoordinator
public WeatherStationSystem Station { get; }
public OrbitalHarrowTelemetrySystem Orbital { get; }
public SeasonalEventSystem Seasonal { get; }
public CloudSeedingSystem CloudSeeding { get; }
public event Action? OnIntelligenceChanged;
public void TickDay(int day) {
public WeatherIntelligenceReadModel BuildReadModel() {
public WeatherIntelligenceSaveState CaptureState() {
public void RestoreState(WeatherIntelligenceSaveState? saved) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`

### `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 512 lines / 22340 bytes.
- SHA-256: `01571211f3a35ad9a43c54265c360224ee86db9b792606c92a3b0af36bb36916`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CampaignDayCoordinator
public ICampaignCalendar Calendar { get; }
public ICampaignRngManager Rng { get; private set; }
public event Action<DayAdvancedEventArgs> OnDayAdvanced;
public bool IsAdvancing => _advancing;
public int LastAdvancedDay => _lastAdvancedDay == int.MinValue ? -1 : _lastAdvancedDay;
public void Register(string ownerId, IDayAdvanceOwner owner, int phase = 3) {
public bool Unregister(string ownerId) {
public bool TryBegin(int day) {
public void EndAdvance() {
public DayAdvancedEventArgs? AdvanceTo(int targetDay, IDayAdvancePersistence? persistence = null, bool failClosed = true) {
public DayAdvancedEventArgs? Advance(int day, IDayAdvancePersistence? persistence = null, bool failClosed = true) {
public readonly string OwnerId;
public readonly IDayAdvanceOwner Owner;
public readonly int Phase;
public CampaignDaySave CaptureState() {
public void RestoreState(CampaignDaySave save) {
public interface IDayAdvanceOwner
public interface IPreDaySnapshotRestore
public interface IDayAdvancePersistence
public sealed class DayStateChangeEvent
public string Kind;
public string SourceOwnerId;
public string PrimaryId;
public string SecondaryId;
public float Numeric;
public sealed class DayOwnerReport
public string OwnerId;
public double DurationMs;
public bool Succeeded;
public DayStateChangeEvent[] Events;
public string FailureMessage;
public sealed class DayAdvancedEventArgs
public int Day;
public DayOwnerReport[] OwnerReports;
public int OwnerCount => OwnerReports?.Length ?? 0;
public bool Succeeded => OwnerReports != null && Array.TrueForAll(OwnerReports, static r => r.Succeeded);
public bool HasFailures => !Succeeded;
public IEnumerable<DayStateChangeEvent> AllEvents() {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs`

### `Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 471 lines / 18115 bytes.
- SHA-256: `21b7425651b6756b0737b00118703cd33921fcb9d949645709985448aaf9bf7e`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=3; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ClimateTrend
public sealed class WinterPhaseDef
public string PhaseId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public int StartDay { get; set; } = 1;
public int EndDay { get; set; } = 90;
public float SeverityModifier { get; set; } = 1.0f;
public float TemperatureBaseCelsius { get; set; } = -10.0f;
public float StormFrequencyModifier { get; set; } = 1.0f;
public float RadiationModifier { get; set; } = 1.0f;
public float DaylightPenaltyHours { get; set; } = 1.0f;
public string Description { get; set; } = string.Empty;
public sealed class SeasonalCycleDef
public string SeasonId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public int DurationDays { get; set; } = 30;
public float TemperatureOffsetCelsius { get; set; } = 0.0f;
public float StormChanceMultiplier { get; set; } = 1.0f;
public float BaseDaylightHours { get; set; } = 10.0f;
public float MoraleDailyImpact { get; set; } = 0.0f;
public sealed class ClimateState
public int Day { get; set; }
public string PhaseId { get; set; } = string.Empty;
public string PhaseDisplayName { get; set; } = string.Empty;
public string SeasonId { get; set; } = string.Empty;
public string SeasonDisplayName { get; set; } = string.Empty;
public float GlobalTemperatureCelsius { get; set; }
public float StormSeverityMultiplier { get; set; }
public float RadiationModifier { get; set; }
public float EffectiveDaylightHours { get; set; }
public float DailyMoraleImpact { get; set; }
public float HeatingFuelCostMultiplier { get; set; }
public float ExpeditionHazardMultiplier { get; set; }
public float CropGrowthRateMultiplier { get; set; }
public ClimateTrend Trend { get; set; }
public sealed class NuclearWinterCatalogData
public int SchemaVersion { get; set; } = 1;
public List<WinterPhaseDef> Phases { get; set; } = new List<WinterPhaseDef>();
public List<SeasonalCycleDef> Seasons { get; set; } = new List<SeasonalCycleDef>();
public sealed class NuclearWinterSaveState
public int SchemaVersion { get; set; } = 1;
public int CurrentDay { get; set; } = 1;
public string LastPhaseId { get; set; } = string.Empty;
public string LastSeasonId { get; set; } = string.Empty;
public int ClimateAnomalyCount { get; set; }
public List<string> RecordedEvents { get; set; } = new List<string>();
public sealed class NuclearWinterProgressionSystem
public IReadOnlyList<WinterPhaseDef> Phases => _phases;
public IReadOnlyList<SeasonalCycleDef> Seasons => _seasons;
public IReadOnlyList<string> RecordedEvents => _recordedEvents;
public int CurrentDay => _currentDay;
public int AnomalyCount => _anomalyCount;
public Action<WinterPhaseDef, WinterPhaseDef>? OnPhaseTransitionedSeam { get; set; }
public Action<SeasonalCycleDef, SeasonalCycleDef>? OnSeasonTransitionedSeam { get; set; }
public Action<ClimateState, string>? OnSevereClimateAnomalySeam { get; set; }
public void LoadCatalog(string json) {
public WinterPhaseDef GetPhaseForDay(int day) {
public SeasonalCycleDef GetSeasonForDay(int day) {
public ClimateState EvaluateClimate(int day, ISeededRng? rng = null) {
public ClimateState AdvanceDay(int day, ISeededRng? rng = null) {
public float CalculateHeatingDemand(float baseDemand, int day) {
public float CalculateExpeditionRisk(float baseRisk, int day) {
public float CalculateCropYieldMultiplier(int day, bool hasGreenhouse) {
public NuclearWinterSaveState CaptureState() {
public void RestoreState(NuclearWinterSaveState? state) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ApprenticeshipSystem.cs`

### `Assets/Ashfall.Core/ApprenticeshipSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 495 lines / 21116 bytes.
- SHA-256: `1c8336bbdcdd6d156ca6163857e76bc31ee0960d3348098de2e184fe33536a24`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=17; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MentorshipDef
public string mentorship_id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string discipline { get; set; } = string.Empty;
public string required_room_tag { get; set; } = "workshop";
public float daily_xp_rate { get; set; } = 15f;
public string manual_item_id { get; set; } = string.Empty;
public int manual_transcription_days { get; set; } = 4;
public string legacy_trait_id { get; set; } = "trait_stoic_craftsman";
public float legacy_xp_grant { get; set; } = 150f;
public sealed class MentorshipCatalog
public int schema_version { get; set; } = 1;
public List<MentorshipDef> mentorships { get; set; } = new List<MentorshipDef>();
public sealed class TranscriptionTask
public string taskId = string.Empty;
public string survivorId = string.Empty;
public string mentorshipId = string.Empty;
public int daysWorked;
public int daysRequired = 4;
public bool isComplete;
public sealed class SurvivorWill
public string willId = string.Empty;
public string testatorSurvivorId = string.Empty;
public string primaryBeneficiaryId = string.Empty;
public string fallbackBeneficiaryId = string.Empty;
public List<string> bequeathedItemIds = new List<string>();
public string finalWords = string.Empty;
public bool isExecuted;
public int executionDay = -1;
public sealed class ApprenticeshipState
public string systemId = ApprenticeshipSystem.SystemId;
public List<Apprenticeship> activePairs = new List<Apprenticeship>();
public List<string> completedSkillIds = new List<string>();
public List<TranscriptionTask> transcriptionTasks = new List<TranscriptionTask>();
public List<SurvivorWill> registeredWills = new List<SurvivorWill>();
public List<SurvivorWill> executedWills = new List<SurvivorWill>();
public Dictionary<string, List<string>> legacyTraitsGranted = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public SkillProgressionSaveState skillProgression = new SkillProgressionSaveState();
public sealed class Apprenticeship
public string pairId = string.Empty;
public string mentorId = string.Empty;
public string apprenticeId = string.Empty;
public string targetSkillId = string.Empty;
public string mentorshipId = string.Empty;
public string requiredRoomTag = string.Empty;
public float progressXp;
public float targetXp = 100f;
public int dayStarted = -1;
public bool isComplete;
public bool isCancelled;
public bool isLegacyInherited;
public string milestonePerkId = string.Empty;
public sealed class ApprenticeshipSystem
public const string SystemId = "apprenticeship";
public const string CatalogPath = "apprenticeship_catalog.json";
public ApprenticeshipState State => _state;
public IReadOnlyDictionary<string, MentorshipDef> Catalog => _catalog;
public int MaxConcurrentPairs { get; set; } = 3;
public Func<string, bool>? IsApprenticeEligible { get; set; }
public event Action<Apprenticeship>? OnApprenticeshipCompleted;
public event Action? OnApprenticeshipChanged;
public event Action<string, string>? OnManualTranscribed; // survivorId, manualItemId
public event Action<SurvivorWill, string>? OnWillExecuted; // will, recipientSurvivorId
public event Action<string, string, float>? OnMentorLegacyInherited; // apprenticeId, traitId, xpGranted
public void LoadCatalog(string jsonContent) {
public void RegisterMentorship(MentorshipDef def) {
public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f, string? mentorshipId = null) {
public ActionResult CancelPair(string pairId) {
public ActionResult StartTranscription(string survivorId, string mentorshipId, InventoryContainer? inv = null) {
public ActionResult RegisterWill(SurvivorWill will) {
public ActionResult ExecuteWill(string deceasedSurvivorId, InventoryContainer targetInventory, HashSet<string>? livingSurvivors = null) {
public void NotifyMentorDeath(string deceasedMentorId) {
public void TickDay(int day) {
public List<Apprenticeship> GetActivePairs() => _state.activePairs.FindAll(p => !p.isComplete && !p.isCancelled);
public ApprenticeshipState CaptureState() => CloneState(_state);
public void RestoreState(ApprenticeshipState saved) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterChartEngine.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterChartEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 297 lines / 11698 bytes.
- SHA-256: `61fc0b3cf9ed814eacf512f74f8c72ca72848e955dbd457c957f4b83366d244d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal class DutyRosterChartEngine
public void Bind(DutyRosterSystemState state) {
public DutyRosterRow GetRow(string survivorId) {
public Func<string, bool>? IsCandidateEligible { get; set; }
public bool WriteName( string survivorId, string displayName, string occupationObserved, string script, int day,
public bool EraseName(string survivorId) {
public bool BurnChart(int day) {
public void TickMorning(int day, IReadOnlyList<DutyRosterOccupant> occupants) {
public bool ResolveChartChoice(string choiceId, int day) {
public bool ResolveLadleChoice(string choiceId, int day) {
public bool ResolveInkEnding(int day) {
public bool SetStatus(string survivorId, string status) {
public bool SetRowScript(string survivorId, string script) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ArchiveDeskSystem.cs`

### `Assets/Ashfall.Core/ArchiveDeskSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 204 lines / 8404 bytes.
- SHA-256: `9bde74b2c74012b2c4e4c47cef31fc7e91b2145a1e6c6f1a9b2ff787d8bd91f2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDeskState
public string systemId = ArchiveDeskSystem.SystemId;
public List<TranscriptionJob> queue = new List<TranscriptionJob>();
public List<string> unlockedEvidenceIds = new List<string>();
public int totalTranscriptions;
public sealed class InkMaterialDefinition
public string ink_id = string.Empty;
public string display_name = string.Empty;
public float legibilityScore = 1f;      // 0-1
public float archivalLongevityDays = 365f;
public float fadeRatePerDay = 0.001f;
public string requiredItemId = string.Empty;
public int requiredAmount = 1;
public sealed class TranscriptionJob
public string jobId = string.Empty;
public string evidenceId = string.Empty;
public string archivistId = string.Empty;
public string inkId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public float totalHoursRequired = 4f;
public bool isComplete;
public bool isCancelled;
public float legibilityScore = 1f;
public string journalEntryId = string.Empty;
public sealed class ArchiveDeskSystem
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public RiskBiasTrait RiskBias { get; set; } = RiskBiasTrait.Realist;
public const string SystemId = "archive_desk";
public ArchiveDeskState State => _state;
public IReadOnlyDictionary<string, InkMaterialDefinition> Catalog => _inkCatalog;
public event Action<TranscriptionJob> OnJobCompleted;
public event Action OnArchiveChanged;
public void LoadInkCatalog(List<InkMaterialDefinition> inks) {
public ActionResult QueueTranscription(string evidenceId, string archivistId, string inkId) {
public ActionResult CancelJob(string jobId) {
public void TickDay(int day) {
public List<TranscriptionJob> GetActiveJobs() => _state.queue.FindAll(j => !j.isComplete && !j.isCancelled);
public bool IsEvidenceUnlocked(string evidenceId) => _state.unlockedEvidenceIds.Contains(evidenceId);
public ArchiveDeskState CaptureState() => CloneState(_state);
public void RestoreState(ArchiveDeskState saved) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/AtmosphericCondenserSystem.cs`

### `Assets/Ashfall.Core/AtmosphericCondenserSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 286 lines / 12658 bytes.
- SHA-256: `2db7445b408dcbc2b276064ad99a6e13e20bf012bb0697724826ccac00502f16`.
- Architecture signals: seeded references=0; save/restore symbols=9; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class AtmosphericCondenserState
public string systemId = AtmosphericCondenserSystem.SystemId;
public int schemaVersion = 1;
public bool built;
public bool enabled = true;
public float membraneIntegrity = 100f;
public long totalYieldLiters;
public int lastCondenseDay = -1;
public sealed class AtmosphericCondenserSystem
public const string SystemId = "water_condenser";
public const int CurrentSchemaVersion = 1;
public const string SourceId = "source_atmospheric_condenser";
public const string RequiredKnowledgeId = "knowledge_water_condenser_blueprint";
public const string MembraneItemId = "item_desal_membrane";
public const float BaseYieldLitersPerDay = 15f;
public const float ArrayDrawWatts = 90f;
public const string PowerRoomId = "room_water_condenser";
public const float MembraneWearPerCondensingDay = 0.5f;
public static float HumidityIndexFor(WeatherKind kind) => kind switch
public event Action<AtmosphericCondenserState>? OnStateChanged;
public event Action<AtmosphericCondenserState>? OnMembraneSpent;
public AtmosphericCondenserState State => CaptureState();
public bool IsBuilt => _state.built;
public bool IsEnabled => _state.enabled;
public float MembraneIntegrity => _state.membraneIntegrity;
public long TotalYieldLiters => _state.totalYieldLiters;
public bool TryBuild(bool hasRequiredCapability, out string reason) {
public bool SetEnabled(bool enabled, out string reason) {
public bool ReplaceMembrane(out string reason) {
public void TickDay(int day) {
public AtmosphericCondenserState CaptureState() => new AtmosphericCondenserState
public void RestoreState(AtmosphericCondenserState? saved) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ContractorRosterSystem.cs`

### `Assets/Ashfall.Core/ContractorRosterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 235 lines / 9931 bytes.
- SHA-256: `252364e87221157a754d45b97f6bc8fe00167a23a3646fab5fb4d9d526d531ad`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContractorRosterState
public string systemId = ContractorRosterSystem.SystemId;
public List<Contractor> contractors = new List<Contractor>();
public List<ContractOffer> activeOffers = new List<ContractOffer>();
public sealed class ContractOffer
public string offerId = string.Empty;
public string candidateId = string.Empty;
public string role = string.Empty;
public int initialFee;
public int dailyHazardPay;
public string paymentCurrency = "scrap_metal";
public int termDays;
public float loyalty = 100f;
public ContractStatus status;
public int proposedDay = -1;
public int startDay = -1;
public int expiryDay = -1;
public List<string> requiredSkills = new List<string>();
public List<string> equipmentIds = new List<string>();
public sealed class Contractor
public string contractorId = string.Empty;
public string displayName = string.Empty;
public string role = string.Empty;
public float loyalty = 100f;
public float trust = 50f;
public ContractStatus status;
public int startDay = -1;
public int expiryDay = -1;
public int missedPayments;
public bool isInjured;
public bool isDeceased;
public List<string> skillIds = new List<string>();
public List<string> equipmentIds = new List<string>();
public enum ContractStatus { Available, Active, Expired, Dismissed, Deceased } public sealed class ContractorRosterSystem { public const string SystemId = "contractor_roster"; private ContractorRosterState _state = new ContractorRosterState(); private readonly ISeededRng _rng; private readonly ILog _log; private readonly Inventory.Inventory _inventory; private readonly DutyRosterSystem _roster; private readonly ExpeditionSystem _expedition; private int _currentDay; public ContractorRosterState State => _state; public event Action<Contractor> OnContractorStatusChanged; public event Action<ContractOffer> OnOfferStatusChanged; public event Action OnRosterChanged; public ContractorRosterSystem( ISeededRng rng, Inventory.Inventory inventory, DutyRosterSystem roster, ExpeditionSystem expedition, ILog? log = null) { _rng = rng ?? throw new ArgumentNullException(nameof(rng)); _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory)); _roster = roster ?? throw new ArgumentNullException(nameof(roster)); _expedition = expedition ?? throw new ArgumentNullException(nameof(expedition)); _log = log ?? NullLog.Instance; }
public ActionResult GenerateOffer(string candidateId, string role, List<string> requiredSkills, int initialFee, int dailyPay, int termDays) {
public ActionResult AcceptOffer(string offerId) {
public ActionResult Dismiss(string contractorId) {
public void TickDay(int day) {
public bool IsAvailableForExpedition(string contractorId) {
public ContractorRosterState CaptureState() => CloneState(_state);
public void RestoreState(ContractorRosterState saved) {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/District8DeepCoastSystem.cs`

### `Assets/Ashfall.Core/District8DeepCoastSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 794 lines / 36531 bytes.
- SHA-256: `662b7bfafbacdd4f1a0faab94fdfc8f432835471e7550648225d12e4beb181f0`.
- Architecture signals: seeded references=3; save/restore symbols=3; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DeepCoastStage
public enum DeepCoastAccessDecision
public class District8DeepCoastState
public string systemId = District8DeepCoastSystem.SystemId;
public string expansionKey = District8DeepCoastSystem.ExpansionKey;
public int stage = (int)DeepCoastStage.Sealed;
public int accessDecision = (int)DeepCoastAccessDecision.None;
public int decisionDay = -1;
public bool perimeterSurveyed;
public bool perimeterCleared;
public bool channelCleared;
public bool berthRepaired;
public float structuralIntegrity = 100f; // 100 = sound; <60 forces the shoring bill
public float contaminationLevel;         // 0..1 brine/industrial contamination
public bool fleetLevyActive;             // Fleet-controlled: levy on dock salvage
public bool officeAccessLimited;         // municipal-controlled: access restricted
public bool fleetStoodUp;                // fleet-controlled: the Fleet came ashore
public List<string> narrativeMarkers = new List<string>();
public string activeDockOperationId = string.Empty;
public string activeDockOperationLocationId = string.Empty;
public string dockOperationDiverId = string.Empty;
public int dockOperationStartedDay = -1;
public int lastTickDay = -1;
public int surgeActiveDay = -1;   // day the current surge began (-1 none)
public int surgeLastStormDay = -1; // last day a surge-grade storm hit
public bool AtLeast(DeepCoastStage s) => stage >= (int)s;
public sealed class DeepCoastRouteNode
public string id = string.Empty;
public string displayName = string.Empty;
public float travelHours;   // cumulative from loc_shelf_foghorn
public float dangerLevel;   // 1..10
public float baseRadsPerHour;
public float segmentHours;  // hours from the previous node
public sealed class District8DeepCoastSystem
public const string SystemId = "district8_deep_coast_system";
public const string ExpansionKey = "expansion_district8_deep_coast";
public const string RegionId = "region_district8_deep_coast";
public const string RouteStartId = "loc_shelf_foghorn";
public const string PerimeterBreakwaterId = "loc_shelf_perimeter_breakwater";
public const string ServiceChannelId = "loc_shelf_service_channel";
public const string DeepBerthId = "loc_shelf_deep_berth";
public const string DockId = "loc_maritime_icebreaker_dock"; // existing Year of Ash anchor
public const string DockDiveSiteId = "site_exp09_naval_patrol"; // existing dive site
public const string FactionFleet = "faction_the_fleet";
public const string FactionOffice = "faction_the_office";
public const string ItemScrapMetal = "scrap_metal";
public const string ItemBrassFittings = "brass_fittings";
public const string ItemFuel = "fuel";
public const string ItemCleanWater = "clean_water";
public const string ItemRoResin = "item_ro_resin";
public const string ItemIodine = "iodine_pills";
public const string JournalSurvey = "dc8_survey_perimeter";
public const string JournalStabilize = "dc8_decision_stabilize";
public const string JournalSalvage = "dc8_decision_salvage";
public const string JournalFleet = "dc8_decision_fleet";
public const string JournalMunicipal = "dc8_decision_municipal";
public const string JournalDockOpen = "dc8_dock_open";
public const string JournalBerthOperational = "dc8_berth_operational";
public const string JournalDiveLaunched = "dc8_dive_launched";
public const float BerthRepairMinIntegrity = 60f;
public const float SalvageImmediateIntegrityHit = 25f;
public const float SalvageImmediateContaminationBoost = 0.40f;
public const float FleetLevyFraction = 0.25f; // Fleet takes 25% of dock salvage
public event Action<DeepCoastStage> OnStageAdvanced;
public event Action<DeepCoastAccessDecision> OnDecisionMade;
public event Action<string, string, int> OnSalvageRolled;      // locationId, itemId, qty
public event Action<string> OnNarrativeMarker;                 // marker/journal key
public event Action OnStateChanged;
public District8DeepCoastState State => _state;
public DeepCoastStage Stage => (DeepCoastStage)_state.stage;
public DeepCoastAccessDecision AccessDecision => (DeepCoastAccessDecision)_state.accessDecision;
public bool IsFleetLevyActive => _state.fleetLevyActive;
public bool IsOfficeAccessLimited => _state.officeAccessLimited;
public bool IsFleetStoodUp => _state.fleetStoodUp;
public float StructuralIntegrity => _state.structuralIntegrity;
public float ContaminationLevel => _state.contaminationLevel;
public bool IsDockOperationActive => !string.IsNullOrEmpty(_state.activeDockOperationId);
public IReadOnlyList<DeepCoastRouteNode> Route => _route;
public bool IsNodeAccessible(string nodeId) {
public bool IsDeepCoastNode(string nodeId) =>
public bool CanStartDockOperation =>
public float TravelHours(string nodeId) {
public float DangerLevel(string nodeId) {
public float RadsPerHour(string nodeId) {
public bool SurveyPerimeter(int day) {
public DeepCoastDecisionOutcome? MakeReopeningDecision(DeepCoastAccessDecision decision, int day, ISeededRng rng) {
public bool TryClearPerimeter(int day, Func<IReadOnlyDictionary<string, int>, bool> tryConsumeBill) {
public bool TryClearPerimeter(int day, Func<string, int, bool> tryConsumeItem) {
public bool TryClearServiceChannel(int day, Func<IReadOnlyDictionary<string, int>, bool> tryConsumeBill) {
public bool TryClearServiceChannel(int day, Func<string, int, bool> tryConsumeItem) {
public CommandPreview PreviewRepairDeepBerth(int day, long stateVersion = 0) {
public CommandResult ExecuteRepairDeepBerth(int day, Func<IReadOnlyDictionary<string, int>, bool> tryConsumeBill, long expectedStateVersion = 0, long currentStateVersion = 0) {
public bool TryRepairDeepBerth(int day, Func<IReadOnlyDictionary<string, int>, bool> tryConsumeBill) {
public bool TryRepairDeepBerth(int day, Func<string, int, bool> tryConsumeItem) {
public bool TryStartDockOperation(string operationId, string diverId, int day) {
public bool TryEndDockOperation(bool success, out float levyFraction) {
public string ActiveDockOperationId => _state.activeDockOperationId;
public string ActiveDockOperationDiverId => _state.dockOperationDiverId;
public Dictionary<string, int> NextStepBill() {
public Dictionary<string, int> PerimeterClearBill() {
public Dictionary<string, int> ChannelClearBill() {
public Dictionary<string, int> BerthRepairBill() {
public void TickDaily(int day, WeatherKind weather) {
public const int SurgeRecedeLagDays = 2;           // calm days before the water drops
public const float SurgeDailyContamination = 0.10f;
public const string JournalSurgeBegan = "dc8_surge_began";
public const string JournalSurgeAftermath = "dc8_surge_aftermath";
public bool IsSurgeActive => _state.surgeActiveDay >= 0;
public int SurgeActiveDay => _state.surgeActiveDay;
public bool IsSurgeBlockingDock => _state.surgeActiveDay >= 0;
public static bool IsSurgeGradeWeather(WeatherKind weather) =>
public District8DeepCoastState CaptureState() {
public void RestoreState(District8DeepCoastState saved) {
public sealed class DeepCoastDecisionOutcome
public DeepCoastAccessDecision Decision;
public float FleetTrustDelta;
public float OfficeTrustDelta;
public string NarrativeKey = string.Empty;
public List<SalvageEntry> Salvage = new List<SalvageEntry>();
public sealed class SalvageEntry
public string ItemId = string.Empty;
public int Quantity;
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`

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


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LibraryStudySystem.cs`

### `Assets/Ashfall.Core/LibraryStudySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 353 lines / 15082 bytes.
- SHA-256: `d8ed15df8fd325172f8ad2b9e2ae8629172d95cbf94708b833150fda32039f5f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LibraryStudyState
public string systemId = LibraryStudySystem.SystemId;
public List<StudyJob> activeJobs = new List<StudyJob>();
public List<string> completedManualIds = new List<string>();
public int totalStudyHours;
public sealed class ManualDefinition
public string manual_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string category { get; set; } = string.Empty;       // "technical", "medical", "military", etc.
public int studyHoursRequired { get; set; } = 10;
public float fatiguePerHour { get; set; } = 0.3f;
public float moraleEffect { get; set; } = -0.5f;           // studying is draining
public List<string> skillXpGrants { get; set; } = new List<string>(); // skill_id, xp_amount pairs
public List<string> researchUnlocks { get; set; } = new List<string>();
public List<string> knowledgeUnlocks { get; set; } = new List<string>();
public List<string> prerequisites { get; set; } = new List<string>();
public bool requiresPower { get; set; } = true;
public List<string> lootTableIds { get; set; } = new List<string>();
public List<string> expeditionRewardIds { get; set; } = new List<string>();
public List<string> traderPoolIds { get; set; } = new List<string>();
public string archiveScribingRecipeId { get; set; } = string.Empty;
public List<string> startingOriginIds { get; set; } = new List<string>();
public string originFacility { get; set; } = string.Empty;
public int technicalComplexityTier { get; set; } = 1;
public string schematicSummary { get; set; } = string.Empty;
public sealed class StudyJob
public string jobId = string.Empty;
public string manualId = string.Empty;
public string readerId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public bool isComplete;
public bool isCancelled;
public sealed class LibraryStudySystem
public const string SystemId = "library_study";
public Func<bool>? PowerAvailable { get; set; }
public bool IsManualPowered(string manualId) {
public LibraryStudyState State => _state;
public IReadOnlyDictionary<string, ManualDefinition> Catalog => _catalog;
public event Action<StudyJob> OnJobCompleted;
public event Action OnLibraryChanged;
public bool IsReaderStudying(string survivorId) {
public static string NormalizeDiscipline(string category) {
public float GetComprehensionRate(string readerId, string manualId) {
public float GetEffectiveStudyHours(string readerId, string manualId) {
public float GetEstimatedDays(string readerId, string manualId) {
public void LoadCatalog(List<ManualDefinition> manuals) {
public ActionResult StartStudy(string manualId, string readerId) {
public ActionResult CancelStudy(string jobId) {
public void TickDay(int day) {
public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);
public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);
public LibraryStudyState CaptureState() => CloneState(_state);
public void RestoreState(LibraryStudyState saved) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`

### `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 208 lines / 9043 bytes.
- SHA-256: `d657335e24b67edfcc6456f8b6b17d5056e1b58a045b8082da0f5a8a76415d81`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MentalHealthState
public string systemId = MentalHealthCrisisSystem.SystemId;
public List<CrisisCase> activeCases = new List<CrisisCase>();
public List<CrisisCase> resolvedCases = new List<CrisisCase>();
public int wardCapacity = 2;
public int currentOccupancy;
public sealed class CrisisCase
public string caseId = string.Empty;
public string survivorId = string.Empty;
public CrisisProfile profile;
public CrisisAcuity acuity;
public int dayStarted = -1;
public int dayResolved = -1;
public CrisisStatus status;
public string assignedCaregiverId = string.Empty;
public string intervention = string.Empty;
public float stressInput;
public float recoveryProgress;
public List<string> sideEffects = new List<string>();
public enum CrisisAcuity { Mild, Moderate, Severe, Critical } public enum CrisisStatus { Active, InTreatment, Recovering, Recovered, Chronic }
public enum CrisisProfile { AcuteStress, SomaticFlashback, GuiltInsomnia, ChemicalWithdrawal, IsolationParanoia } public sealed class MentalHealthCrisisSystem { public const string SystemId = "mental_health"; private MentalHealthState _state = new MentalHealthState(); private readonly ISeededRng _rng; private readonly ILog _log; private readonly NeedsSystem _needs; private readonly MedicalWardSystem _medical; private readonly ChemicalDependencySystem _dependency; private readonly DutyRosterSystem _roster; private int _currentDay; public MentalHealthState State => _state; public event Action<CrisisCase> OnCrisisResolved; public event Action OnMentalHealthChanged; public MentalHealthCrisisSystem( ISeededRng rng, NeedsSystem needs, MedicalWardSystem medical, ChemicalDependencySystem dependency, DutyRosterSystem roster, ILog? log = null) { _rng = rng ?? throw new ArgumentNullException(nameof(rng)); _needs = needs ?? throw new ArgumentNullException(nameof(needs)); _medical = medical ?? throw new ArgumentNullException(nameof(medical)); _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency)); _roster = roster ?? throw new ArgumentNullException(nameof(roster)); _log = log ?? NullLog.Instance; }
public ActionResult TriggerCrisis(string survivorId, float stressInput, CrisisProfile profile) {
public ActionResult BeginTreatment(string caseId, string caregiverId, string intervention) {
public const int ChronicThresholdDays = 14;
public void TickDay(int day) {
public bool IsInCrisis(string survivorId) {
public bool IsEligibleForWork(string survivorId) {
public MentalHealthState CaptureState() => CloneState(_state);
public void RestoreState(MentalHealthState saved) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

### `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 403 lines / 14637 bytes.
- SHA-256: `364d311490e3a73f9e9d2d31861f78f70ecb80a4615f9b48ade364cb2330e7a8`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PerformanceCampaignHarness : IDisposable
public CampaignDayCoordinator Coordinator { get; }
public SurvivorRosterSystem Survivors { get; }
public Inventory.Inventory Inventory { get; }
public JournalSystem Journal { get; }
public WeatherSystem Weather { get; }
public ExpeditionSystem Expeditions { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmark { get; }
public int CurrentDay => Coordinator.Calendar is Ashfall.Core.Clock.ISimClock simClock ? simClock.DayIndex : Coordinator.LastAdvancedDay;
public ISeededRng Rng { get; }
public double AdvanceDays(int days) {
public string CaptureSavePayload() {
public double MeasureSaveLatency() {
public double MeasureLoadLatency(string payload) {
public static double MeasureChecksumLatency(string payload) {
public long MeasureRetainedMemoryAfterNewGame() {
public void Dispose() {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public string Id => "perf_author";
public string DisplayName => "Perf";
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 77

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the stale 1→8 target with the current eight-window calendar.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced catalog selection → day owner → roster/consumer → save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass pins inclusive boundaries, overflow fallback and derived-season restore.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
