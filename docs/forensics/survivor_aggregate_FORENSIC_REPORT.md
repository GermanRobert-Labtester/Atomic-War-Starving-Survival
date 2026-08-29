# 1. Target

Establish one canonical survivor identity and lifecycle model for the entire ASHFALL campaign, with domain-owned mutable state components, while preserving all existing gameplay mechanics, determinism, persistence, and host wiring.

# 2. Executive Finding

The repository currently has **no canonical survivor aggregate**. Survivor identity and lifecycle state are split across at least **nine independent string-keyed authorities**, each deciding locally whether a survivor exists, is alive, or is dead. Domain components (needs, radiation, medical, social, expeditions, duty roster, fate) each maintain their own survivor dictionaries/lists with no referential-integrity enforcement. A `SurvivorFateSystem` exists as a death-cascade authority, but it is reactive (first death report wins) rather than a proactive lifecycle owner.

**Bottom line:** The codebase compiles and runs, but it has multiple independent survivor truths that can and do diverge.

# 3. Evidence Summary

## 3.1 Independent Survivor Identity Authorities

| Authority | Location | Key Type | Storage | Lifecycle Fields |
|-----------|----------|----------|---------|------------------|
| `SurvivorRosterSystem` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs:61` | `string` | `Dictionary<string, SurvivorRosterEntry> _byId` | `isAlive`, `deathReason` |
| `NeedsSystem` | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:70` | `string` (in `SurvivorNeedsState.Id`) | `List<SurvivorNeedsState> _survivors` | `IsAlive`, `IsDead` |
| `RadiationSystem` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:93` | `string` (in `SurvivorRadState.Id`) | `List<SurvivorRadState> _survivors` + `Dictionary<string, Dosimeter> _dosimeters` | `IsAlive` |
| `SurvivorsHostSession` | `src/Host/SurvivorsHostSession.cs:41` | `string` | `List<SurvivorNeedsState> RosterState` + `Dictionary<string, RadSurvivorWrapper> _radStates` | none (mirrors Needs/Radiation) |
| `SurvivorFateSystem` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:89` | `string` | `Dictionary<string, SurvivorFateEvent> _byId` | death record (first report wins) |
| `ExpeditionSystem` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:175` | `string` | `Dictionary<string, ExpeditionState> _active` | none |
| `DutyRosterSystem` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs:86` | `string` | `Dictionary<string, DutyRosterRow> _byId` | none |
| `DutyRosterAssignmentEngine` | `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs:41` | `string` | `Dictionary<string, string> _assignmentByRole` | none |
| `MedicalWardSystem` | `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs:18` | `string` (in `MedicalAdmissionRecord.PatientId`) | `List<MedicalAdmissionRecord>` in `MedicalWardState` | `MedicalAdmissionStatus` (Active/Discharged/Deceased) |
| `MemorialSystem` | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:19` | `string` (in `MemorialEntry.SurvivorId`) | `List<MemorialEntry>` in `MemorialState` | none (post-death record) |

**Evidence:** Each system above independently accepts a `string` survivor ID, creates its own state record, and never validates that the ID resolves to a canonical survivor aggregate.

## 3.2 Survivor Definition vs Campaign Entity

- **Definitions** are loaded from `survivors.json` via `SurvivorCatalogLoader` into `SurvivorDefinition` objects (`Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs:11`).
- **Campaign entities** are created by `SurvivorRosterSystem.Join(definitionId, day)` (`SurvivorCatalog.cs:88`), which uses the `definitionId` as the `survivorId`.
- **Starting state** is loaded from `starting_survivors.json` via `SurvivorStartingStateLoader` (`Assets/Ashfall.Core/Survivors/SurvivorStartingStateLoader.cs`).
- **Current 1:1 mapping:** The codebase assumes one campaign instance per authored survivor (definition ID = campaign entity ID). `SurvivorRosterEntry.definitionId` and `survivorId` are always the same value.

## 3.3 Lifecycle State Fragmentation

Lifecycle is expressed through overlapping booleans across multiple types:

| State | `SurvivorRosterEntry` | `SurvivorNeedsState` | `SurvivorRadState` | `MedicalAdmissionRecord` |
|--------|----------------------|---------------------|-------------------|-------------------------|
| Alive | `isAlive = true` | `IsAlive = true`, `IsDead = false` | `IsAlive = true` | `Status = Active` |
| Dead | `isAlive = false` | `IsAlive = false`, `IsDead = true` | `IsAlive = false` | `Status = Deceased` |
| Missing | not expressed | not expressed | not expressed | not expressed |
| Away/Deployed | not expressed | not expressed | not expressed | not expressed |
| Memorialized | not expressed | not expressed | not expressed | not expressed |

**Evidence:** No explicit lifecycle state machine exists. Death is the only formally tracked transition, and even that is tracked inconsistently (roster booleans, needs booleans, radiation booleans, fate records, medical status).

## 3.4 Death Cascade (`SurvivorFateSystem`)

`SurvivorFateSystem.ReportDeath()` (`SurvivorFateSystem.cs:140`) is the closest thing to a lifecycle authority. It:
1. Marks roster entry dead
2. Forces needs state dead
3. Clears assignments (duty roster, caregiving, medical ward, expedition)
4. Applies leadership stress + grief morale
5. Resolves final wishes
6. Records memorial
7. Records journal entry
8. Updates consequence ledger

**Critical gap:** `ReportDeath` is reactive — any system can report death, and the cascade runs once (first report wins). There is no central authority that *decides* death; systems independently detect it (needs health ≤ 0, radiation sickness, combat, disease, etc.) and report it.

## 3.5 Host-Side Radiation Wrapper (`RadSurvivorWrapper`)

`SurvivorsHostSession` (`src/Host/SurvivorsHostSession.cs:80`) defines:
```csharp
private sealed class RadSurvivorWrapper : SurvivorRadState { }
```

This wrapper:
- Inherits from `SurvivorRadState` (Core)
- Is stored in `Dictionary<string, RadSurvivorWrapper> _radStates`
- Acts as the identity authority for radiation state in the Godot host
- Is created in `AddSurvivor()` and restored in `RestoreSave()`

**Evidence:** The host wrapper duplicates Core radiation state ownership. `RadiationSystem` operates on `SurvivorRadState` objects, but the host creates wrapper instances and stores them in its own dictionary. The wrapper's existence implies survivor existence in the host, independent of any canonical aggregate.

## 3.6 Demo Survivor Seeding

`SurvivorsHostSession.SeedDemoRoster()` (`src/Host/SurvivorsHostSession.cs:146`) seeds three hardcoded survivors:
- `survivor_dr_sarah_chen`
- `survivor_gunner_mikhail`
- `elena_vasquez`

`LoadStartingRoster()` (`src/Host/SurvivorsHostSession.cs:118`) first tries `starting_survivors.json`, then falls back to `SeedDemoRoster()`. This means demo survivors appear in production if the starting file is missing.

## 3.7 Survivor ID Comparison Semantics

| System | Comparer | Normalization |
|--------|----------|---------------|
| `SurvivorRosterSystem._byId` | default (`Ordinal`) | none |
| `NeedsSystem._survivors` | `List<T>.Contains` + `String.Equals(..., Ordinal)` | none |
| `RadiationSystem._survivors` | `List<T>.Contains` | none |
| `RadiationSystem._dosimeters` | default (`Ordinal`) | none |
| `SurvivorFateSystem._byId` | `StringComparer.Ordinal` | none |
| `SurvivorSocialCoordinator._beliefs` | `StringComparer.Ordinal` | none |
| `DutyRosterChartEngine._byId` | default (`Ordinal`) | none |
| `DutyRosterAssignmentEngine._assignmentByRole` | default (`Ordinal`) | none |
| `SurvivorLetterCatalog._byId` | `StringComparer.OrdinalIgnoreCase` | case-insensitive (outlier) |

**Evidence:** Most systems use ordinal comparison. One system (`SurvivorLetterCatalog`) uses case-insensitive comparison, creating a potential normalization inconsistency.

## 3.8 Save/Load Survivor Slices

Survivor state is persisted across multiple save sections:

| Save Slice | Location | Format |
|-----------|----------|--------|
| `SurvivorRosterState` | `SurvivorCatalog.cs:43` | `List<SurvivorRosterEntry>` inside `SurvivorRosterState` |
| `SurvivorsSaveState` | `src/Host/SurvivorsSaveStore.cs` | `List<SurvivorSliceState>` (needs + radiation combined) |
| `SurvivorFateSaveState` | `SurvivorFateSystem.cs` | `List<SurvivorFateEvent>` |
| `MedicalWardState` | `MedicalWardSystem.cs` | `List<MedicalAdmissionRecord>` + `List<MedicalProcedureRecord>` |
| `MemorialState` | `MemorialSystem.cs` | `List<MemorialEntry>` |
| `SurvivorSocialSaveState` | `SurvivorSocialCoordinator.cs` | composite of 5 sub-states |
| `DutyRosterSystemState` | `DutyRosterSystem.cs` | `DutyRosterRow` list + assignments |
| `ExpeditionSystem` save | `ExpeditionSystem.cs` | `List<ExpeditionState>` |

**Evidence:** No versioned migration exists for survivor slices. Legacy saves may contain contradictory state (e.g., roster says alive, fate record says dead, medical says deceased).

# 4. Architecture Placement

## Current Architecture (Actual)

```
SurvivorDefinition (catalog, immutable)
    ↓ Join()
SurvivorRosterEntry (roster, mutable lifecycle)
    ↓ parallel, uncoordinated
SurvivorNeedsState (needs domain)
SurvivorRadState (radiation domain)
CombatTraumaSurvivorState (combat domain)
RespiratorySurvivorState (medical domain)
GuiltSurvivorState (psych domain)
FinalWishSurvivorState (final wishes domain)
RationConflictSurvivorState (social domain)
FlashbackSurvivorState (psych domain)
TradeSpecialtySurvivorState (skills domain)
SkillProgressionState (skills domain)
ExpeditionState (expedition domain)
DutyRosterRow (assignment domain)
MedicalAdmissionRecord (medical domain)
MemorialEntry (memorial domain)
SurvivorFateEvent (fate domain)
```

**Key observation:** Every domain independently creates its own survivor state record keyed by `string`. There is no central authority that validates "does this survivor ID exist?" before allowing domain state creation.

## Target Architecture (Task 132)

```
SurvivorId (value object, canonical identity)
    ↓ resolves to
SurvivorAggregate (minimal: SurvivorId + definition reference + lifecycle state)
    ↓ referenced by
Domain Components (each keyed by SurvivorId):
  - NeedsComponent (SurvivorNeedsState)
  - RadiationComponent (SurvivorRadState)
  - MedicalComponent (MedicalAdmissionRecord)
  - PsychologicalComponent (GuiltSurvivorState, FlashbackSurvivorState)
  - SocialComponent (TraumaBond, RationConflict, IdeologicalFriction)
  - SkillsComponent (SkillProgressionState, TradeSpecialtySurvivorState)
  - EquipmentComponent (EquipmentInstance bindings)
  - AssignmentComponent (DutyRosterRow, DutyRosterAssignmentEntry)
  - ExpeditionComponent (ExpeditionState)
```

# 5. Current Implementation

## 5.1 SurvivorRosterSystem

**File:** `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs:55`

```csharp
public class SurvivorRosterSystem
{
    private readonly Dictionary<string, SurvivorRosterEntry> _byId;
    private readonly List<SurvivorDefinition> _catalog;

    public bool Join(string definitionId, int day);
    public bool Die(string survivorId, string reason);
    public SurvivorRosterEntry? Find(string survivorId);
    public int LivingCount { get; }
}
```

**Responsibilities:**
- Loads definitions from `survivors.json`
- Tracks roster entries (definition instantiation)
- Lifecycle: Join / Die
- Events: `OnSurvivorJoined`, `OnSurvivorDied`, `OnStateChanged`

**Gaps:**
- No explicit lifecycle states beyond alive/dead
- No Missing/Away/Memorialized states
- No assignment/expedition/location ownership
- `Die()` only sets `isAlive = false` and `deathReason`; does not clear assignments or expedition participation
- No referential integrity with domain components

## 5.2 NeedsSystem

**File:** `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:70`

```csharp
public class NeedsSystem
{
    private readonly List<SurvivorNeedsState> _survivors;

    public void Register(SurvivorNeedsState survivor);
    public void Unregister(SurvivorNeedsState survivor);
    public SurvivorNeedsState? Get(string id);
    public void Tick(float gameHours);
    public void ForceDeath(SurvivorNeedsState survivor);
}
```

**Responsibilities:**
- Per-survivor need decay (hunger, thirst, fatigue, warmth, morale, health, hygiene)
- Critical need consequences
- Death evaluation at Health ≤ 0

**Gaps:**
- Owns `IsAlive`/`IsDead` independently from roster
- No validation that registered survivor exists in roster
- `ForceDeath` can fire without roster coordination

## 5.3 RadiationSystem

**File:** `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:93`

```csharp
public class RadiationSystem
{
    private readonly List<SurvivorRadState> _survivors;
    private readonly Dictionary<string, Dosimeter> _dosimeters;

    public void Register(SurvivorRadState survivor);
    public void Unregister(SurvivorRadState survivor);
    public void Tick(float gameHours);
    public void Expose(SurvivorRadState survivor, float radsPerHour, float hours);
}
```

**Responsibilities:**
- Dose accumulation
- Status tracking (acute/chronic/resistance)
- Iodine/anti-rad administration

**Gaps:**
- Owns `IsAlive` independently
- Dosimeter dictionary keyed by string ID, separate from survivor state
- No validation that registered survivor exists in roster

## 5.4 SurvivorFateSystem

**File:** `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:89`

```csharp
public sealed class SurvivorFateSystem
{
    private readonly Dictionary<string, SurvivorFateEvent> _byId;

    public SurvivorFateEvent ReportDeath(SurvivorFateEvent fate);
    public int ReconcileFromRoster();
}
```

**Responsibilities:**
- Single idempotent death record per survivor
- Cascade: roster → needs → assignments → medical → expedition → social → final wishes → memorial → journal → flags

**Gaps:**
- Reactive (first death report wins) rather than authoritative
- No prevention of contradictory state before cascade
- No prevention of duplicate/parallel death paths after cascade

## 5.5 MedicalWardSystem

**File:** `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs:18`

```csharp
public sealed class MedicalWardSystem
{
    private readonly MedicalWardState _state;

    public MedicalWardAdmissionResult Admit(string patientId, string bedId, int day);
    public MedicalWardAdmissionResult Discharge(string patientId, int day);
}
```

**Responsibilities:**
- Bed management
- Admission/discharge/procedure tracking

**Gaps:**
- No validation that `patientId` exists in canonical survivor store
- Admission status (Active/Discharged/Deceased) duplicates lifecycle state

## 5.6 MemorialSystem

**File:** `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:19`

```csharp
public sealed class MemorialSystem
{
    private readonly MemorialState _state;

    public MemorialEntry Memorialize(MemorialInput input);
}
```

**Responsibilities:**
- Idempotent memorial record creation
- Stores cause, day, final wish status, epitaph, heirloom

**Gaps:**
- No validation that `SurvivorId` exists in canonical store
- No enforcement that memorialization only occurs after death

# 6. Runtime Wiring

## 6.1 Host Session

`SurvivorsHostSession` (`src/Host/SurvivorsHostSession.cs`) is the Godot host's thin wrapper:
- Owns `NeedsSystem` and `RadiationSystem`
- Owns `List<SurvivorNeedsState> RosterState` (duplicate of needs state)
- Owns `Dictionary<string, RadSurvivorWrapper> _radStates` (host radiation wrapper)
- Wires `Needs.OnDied` → `SurvivorFateSystem.ReportDeath()`
- Wires `Radiation.OnStatusGained` → audio alerts
- `AddSurvivor()` creates roster entry, needs state, and radiation wrapper atomically (within the method, but not across domains)

**Evidence:** The host session is the closest thing to a composition root for survivors, but it does not enforce canonical identity — it just creates parallel lists/dictionaries.

## 6.2 Composition Root

`CampaignServices` (`Assets/Ashfall.Core/Campaign/CampaignServices.cs:26`) references `SurvivorsHostSession`, but the Core composition root (`GameBootstrap`) constructs systems independently. There is no central survivor store that all systems must resolve through.

# 7. Data Flow

## New Game Flow

1. `SurvivorsHostSession.LoadCatalog()` loads `survivors.json` into `SurvivorRosterSystem`
2. `SurvivorsHostSession.LoadStartingRoster()` loads `starting_survivors.json` or seeds demo roster
3. For each starting survivor: `Roster.RegisterDefinition()` + `Roster.Join()` + `Needs.Register()` + `Radiation.Register()` + host wrapper creation
4. No canonical store validates that all three (roster, needs, radiation) agree on survivor existence

## Death Flow

1. `NeedsSystem` or `RadiationSystem` detects death condition
2. Fires `OnDied` event
3. `SurvivorsHostSession` receives event, calls `SurvivorFateSystem.ReportDeath()`
4. `ReportDeath` runs cascade: roster → needs → assignments → medical → expedition → social → final wishes → memorial → journal → flags
5. **Gap:** If death is detected by combat or expedition system directly, it may bypass the needs/radiation death path and call `ReportDeath` directly, creating a parallel authority

## Expedition Flow

1. `ExpeditionSystem.Start()` accepts `string survivorId`
2. Creates `ExpeditionState` keyed by survivorId
3. **Gap:** No validation that survivor exists in roster or is alive
4. On return/completion: no automatic lifecycle reconciliation

## Assignment Flow

1. `DutyRosterAssignmentEngine.Assign()` accepts `string survivorId`
2. Looks up `DutyRosterRow` via `_getRow(survivorId)`
3. **Gap:** No validation that survivor exists in canonical roster
4. On death: `SurvivorFateSystem` calls `RemoveAssignmentsFor()`, but only if the fate cascade is triggered

# 8. State Ownership

| Domain | Owns | Does Not Own |
|--------|------|-------------|
| `SurvivorRosterSystem` | roster ledger, definition catalog, alive/death | needs, radiation, medical, social, skills, equipment |
| `NeedsSystem` | need values, death detection | identity, roster membership, assignments |
| `RadiationSystem` | dose, status, dosimeter | identity, roster membership |
| `SurvivorFateSystem` | death records, cascade coordination | identity, needs values, radiation values |
| `MedicalWardSystem` | bed assignments, procedures | patient identity validation |
| `MemorialSystem` | memorial entries | identity validation |
| `ExpeditionSystem` | expedition state | survivor lifecycle |
| `DutyRosterSystem` | duty rows, assignments | survivor lifecycle |

**Critical finding:** No system owns "does this survivor ID exist in this campaign?" That question is answered implicitly by whether a domain-specific dictionary/list contains the ID.

# 9. Save/Load

## 9.1 Save Format

Survivor state is persisted across **8+ independent save sections** with no central migration authority:

- `SurvivorRosterState` → `SurvivorRosterSystem.CaptureState()`
- `SurvivorsSaveState` → `SurvivorsHostSession.CaptureSave()` (combined needs + radiation)
- `SurvivorFateSaveState` → `SurvivorFateSystem.CaptureState()`
- `MedicalWardState` → `MedicalWardSystem.CaptureState()`
- `MemorialState` → `MemorialSystem.CaptureState()`
- `SurvivorSocialSaveState` → `SurvivorSocialCoordinator.CaptureState()`
- `DutyRosterSystemState` → `DutyRosterSystem.CaptureState()`
- `ExpeditionSystem` → `ExpeditionSystem.CaptureState()`

## 9.2 Load Order

There is **no documented load order** for survivor slices. `Main.cs` orchestrates `SetupXxx`/`RestoreXxx` calls, but the order varies and there is no validation that loaded slices are mutually consistent.

## 9.3 Legacy Migration

- `SurvivorFateSystem.ReconcileFromRoster()` synthesizes fate records for dead roster entries with no fate record
- No migration exists for conflicting legacy state (roster alive ≠ fate dead ≠ needs dead)

# 10. Determinism

## 10.1 Iteration Order

| System | Iteration Order | Deterministic? |
|--------|----------------|----------------|
| `SurvivorRosterSystem` | `_state.entries` list order + `_byId` dictionary | `CaptureState` sorts by `survivorId` ordinal; `RestoreState` preserves insertion order |
| `NeedsSystem` | `_survivors` list order | insertion order (registration order) |
| `RadiationSystem` | `_survivors` list order | insertion order |
| `SurvivorFateSystem` | `_state.fates` sorted by day then survivorId | Yes (explicit sort in `CaptureState`) |
| `ExpeditionSystem` | `_active` dictionary | insertion order |

**Gap:** `NeedsSystem` and `RadiationSystem` iterate in registration order, which may vary if registration order differs between runs.

## 10.2 ID Equality

Most systems use `StringComparison.Ordinal` or default dictionary comparers (ordinal). One outlier:
- `SurvivorLetterCatalog` uses `StringComparer.OrdinalIgnoreCase`

**Risk:** If any survivor ID differs only by case, `SurvivorLetterCatalog` would treat them as the same survivor while other systems treat them as different.

# 11. UI/Player Feedback

UI panels consume survivor state through host sessions:
- `SurvivorInspectionHostSession` provides read-model projections
- `ExpeditionPanel` binds through `SurvivorsHostSession`
- `SaveLoadPanel` persists `SurvivorsSaveState`

**Gap:** UI derives from parallel host lists, not from a canonical aggregate. If domain states diverge, UI may display inconsistent information.

# 12. Tests & Verification

## 12.1 Existing Tests

| Test File | Coverage |
|-----------|----------|
| `Ashfall.Core.Tests/SurvivorRosterSystemTests.cs` | Roster join, die, find, living count |
| `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` | Needs decay, death, radiation exposure, dose |
| `Ashfall.Core.Tests/SurvivorFateSystemTests.cs` | Death cascade, idempotency, reconciliation |
| `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs` | Memorialization |
| `Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs` | Admission, discharge, procedures |
| `Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs` | Social systems wiring |

## 12.2 Test Gaps

- No test validates that a `SurvivorNeedsState` cannot be registered for an unknown roster ID
- No test validates that `MedicalWardSystem` cannot admit a non-existent patient
- No test validates that `ExpeditionSystem` cannot start an expedition for a dead survivor
- No test validates deterministic iteration order across all survivor systems
- No test validates that save/load produces mutually consistent survivor slices
- No test validates that `RadSurvivorWrapper` cannot outlive its roster entry

# 13. Duplicates / Legacy / Forks

## 13.1 Duplicate Identity Authorities

| Primary | Duplicate | Relationship |
|---------|-----------|--------------|
| `SurvivorRosterSystem._byId` | `SurvivorsHostSession.RosterState` (list) | Host duplicates roster state as a list |
| `RadiationSystem._survivors` | `SurvivorsHostSession._radStates` | Host creates wrapper dictionary mirroring Core radiation list |
| `SurvivorNeedsState.IsAlive` | `SurvivorRosterEntry.isAlive` | Both independently track alive/dead |
| `SurvivorRadState.IsAlive` | `SurvivorRosterEntry.isAlive` | Both independently track alive/dead |
| `MedicalAdmissionStatus.Deceased` | `SurvivorRosterEntry.isAlive = false` | Both independently track death |

## 13.2 Legacy Artifacts

- `SurvivorRosterState` class name suggests a "state" object that owns survivor truth — it does, but only for the roster domain
- `RadSurvivorWrapper` is a host-side subclass of `SurvivorRadState` that exists solely to provide identity storage in the host
- `SurvivorsSaveState` combines needs + radiation into one save slice, hiding the domain separation

# 14. Existing Extension Seams

## 14.1 Interfaces

- `ISurvivorAuthor` (used by `JournalSystem`)
- `ISeededRng` (deterministic RNG)
- `IFileIO`, `IJsonSerializer` (persistence)

## 14.2 Events

- `SurvivorRosterSystem.OnSurvivorJoined`, `OnSurvivorDied`, `OnStateChanged`
- `NeedsSystem.OnNeedChanged`, `OnNeedCritical`, `OnDied`
- `RadiationSystem.OnDoseChanged`, `OnStatusGained`, `OnStatusLost`
- `SurvivorFateSystem.OnSurvivorFate`, `OnLastSurvivorDied`
- `MemorialSystem.OnMemorialized`

## 14.3 Save Patterns

- `CaptureState()`/`RestoreState()` pattern is established
- `SaveStore<T>` service exists
- `CampaignEnvelopeBuilder` exists for atomic save composition

## 14.4 Composition Root

- `CampaignServices` aggregates host sessions
- `GameBootstrap` constructs Core systems
- `Main.Application` dispatches headless selftests

# 15. Functional Equivalents

| Concept | Current Equivalent | Gap |
|---------|-------------------|-----|
| Canonical survivor ID | `SurvivorRosterEntry.survivorId` (string) | Not a value object; no validation |
| Survivor aggregate | None | Must be created |
| Lifecycle state | `SurvivorRosterEntry.isAlive` (bool) | No explicit state machine |
| Component store | Per-domain dictionaries | No central registry |
| Deterministic iteration | `CaptureState` sorts roster; others use insertion order | Inconsistent |
| Atomic lifecycle transaction | `SurvivorFateSystem.ReportDeath()` | Only for death; no join/deploy/return |

# 16. Confirmed Gaps

1. **No canonical survivor aggregate** — multiple independent string-keyed authorities
2. **No lifecycle state machine** — only alive/dead booleans, no Missing/Away/Memorialized
3. **No referential integrity** — domain components accept any string ID
4. **No atomic join/leave/deploy/return** — only death has a cascade
5. **Host radiation wrapper duplicates Core authority** — `RadSurvivorWrapper` in `SurvivorsHostSession`
6. **Demo survivor seeding in production path** — `SeedDemoRoster()` falls back when `starting_survivors.json` is missing
7. **No save migration for survivor slices** — legacy saves may contain contradictory state
8. **Inconsistent comparison semantics** — `SurvivorLetterCatalog` uses case-insensitive comparison
9. **No deterministic iteration guarantee** — needs/radiation iterate in registration order
10. **No validation that expedition participants exist** — `ExpeditionSystem.Start()` accepts any string

# 17. Risks

## CRITICAL

1. **State divergence:** Multiple independent authorities can create contradictory survivor state (roster alive, needs dead, expedition active, medical admitted)
2. **No migration path:** Existing saves contain scattered survivor slices with no conflict-resolution rules
3. **Host wrapper authority:** `RadSurvivorWrapper` in `SurvivorsHostSession` implies survivor existence independently of Core roster

## HIGH

4. **No lifecycle atomicity:** Join/leave/deploy/return operations span multiple domains with no transaction safety
5. **Determinism risk:** Registration-order iteration in needs/radiation may vary
6. **Case sensitivity outlier:** `SurvivorLetterCatalog` uses case-insensitive comparison, potentially creating duplicate identities

## MEDIUM

7. **Demo seeding in production:** Missing `starting_survivors.json` silently falls back to hardcoded demo survivors
8. **UI projection drift:** UI reads from parallel host lists, not canonical aggregate
9. **Test coverage gaps:** No referential-integrity or atomicity tests

# 18. Constraints for Planning

1. **Preserve all existing gameplay mechanics** — needs decay, radiation progression, death cascade, medical admissions, expeditions, duty roster, social systems
2. **Preserve determinism** — same seed must produce identical simulation
3. **Preserve save compatibility** — existing save format must migrate or remain loadable
4. **Preserve host wiring** — `SurvivorsHostSession` is the Godot integration point
5. **Do not create god object** — aggregate must remain minimal; domain state stays in domain components
6. **Do not introduce new event architecture** — reuse existing events
7. **Incremental cutover** — migrate one domain at a time with dual-run parity
8. **No global string replacement** — convert only confirmed identifier strings to `SurvivorId`

# 19. Evidence Index

| Evidence | File | Line/Member |
|----------|------|-------------|
| Roster system | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` | `SurvivorRosterSystem` class |
| Needs system | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | `NeedsSystem` class |
| Radiation system | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | `RadiationSystem` class |
| Host session | `src/Host/SurvivorsHostSession.cs` | `SurvivorsHostSession` class |
| Rad wrapper | `src/Host/SurvivorsHostSession.cs` | `RadSurvivorWrapper` class |
| Fate system | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` | `SurvivorFateSystem` class |
| Medical ward | `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` | `MedicalWardSystem` class |
| Memorial system | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | `MemorialSystem` class |
| Social coordinator | `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` | `SurvivorSocialCoordinator` class |
| Expedition system | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | `ExpeditionSystem` class |
| Duty roster | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` | `DutyRosterSystem` class |
| Starting loader | `Assets/Ashfall.Core/Survivors/SurvivorStartingStateLoader.cs` | `SurvivorStartingStateLoader` class |
| Demo seeding | `src/Host/SurvivorsHostSession.cs` | `SeedDemoRoster()` method |
| Case-insensitive comparison | `Assets/Ashfall.Core/Narrative/SurvivorLetterCatalog.cs` | `StringComparer.OrdinalIgnoreCase` |
| Save store | `src/Host/SurvivorsSaveStore.cs` | `SurvivorsSaveStore` class |

# 20. Confidence & Unknowns

## High Confidence

- Current architecture has no canonical survivor aggregate
- Multiple independent string-keyed survivor authorities exist
- `RadSurvivorWrapper` is a host-side identity authority
- `SurvivorFateSystem` is reactive, not authoritative
- Demo survivor seeding exists in production path
- No explicit lifecycle state machine exists

## Unknowns (Require Further Investigation)

1. **Exact load order** — `Main.cs` orchestrates survivor slice restoration, but the precise order and validation steps are not documented
2. **Task #140/#141/#148 outputs** — these dependency tasks may have already introduced partial canonical infrastructure not yet identified
3. **Equipment binding semantics** — `WeaponEquipmentBridge` accepts `ownerId` strings, but the exact binding rules and validation are not fully traced
4. **Expedition-location ownership** — whether location belongs to aggregate, expedition, or a separate location component is not decided
5. **Psychological system scope** — `GuiltInsomniaSystem`, `SomaticFlashbackSystem`, `PsychologicalContaminationSystem` exist but their exact survivor-referencing patterns are not fully catalogued
6. **Legacy save volume** — how many existing saves contain contradictory survivor state is unknown

---

- **Report generated:** 2026-08-29
- **Analyst:** ashfall-analyze skill (read-only forensic pass)
- **Status:** Phase 0 reconnaissance complete. No code modified.
- **Corrections:** see `docs/architecture/survivor_identity_inventory.json`
  (`forensic_report_corrections`) — four claims in this report were
  refuted during Task #132 P0 implementation.
