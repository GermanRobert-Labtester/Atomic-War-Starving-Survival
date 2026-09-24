# UNBLOCK — Plan 216: ExerciseSystem / Survivor Exercise & Physical Training

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-216-exercise-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Survivor Exercise & Physical Training system is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `ExerciseSystem` (DEC-343) the live physical fitness, workout routine execution,
and conditioning authority for shelter survivors while preserving single-owner domain boundaries (Rule 5):

- `HealthSystem` owns injury, disease, and raw HP/vitality state.
- `ExerciseSystem` owns physical fitness attributes (strength, endurance, agility, fatigue), routine assignments (`Assets/StreamingAssets/Data/exercise_routines.json`), training sessions, injury prevention modifiers, and fitness decay.
- `ExerciseSystem` in pure Core manages fitness profiles, routine validation, workout calculations, recovery, and census statistics (`ExerciseCensus`).
- `ExerciseHostSession` and `ExerciseSaveStore` provide host-level lifecycle, persistence (`exercise` section, `exercise_save.json`, section #260), and atomic state operations.
- `ExerciseDayOwner` in `Main.CampaignOwners.cs` advances daily fatigue recovery, fitness decay, and routine execution during phase-5 daily simulation, emitting `exercise_ticked`.
- `--exercise-selftest` validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Survivors/ExerciseSystem.cs`: Pure domain system, state model, fitness profiles, workout routines, and `ExerciseCensus`.
  - `Assets/StreamingAssets/Data/exercise_routines.json`: Authoritative exercise routines catalog.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `exercise` registered under `survivors` domain with `ExpandedSurvivorLifecycleGroup` (file: `exercise_save.json`).
- **Godot Host (`src/`):**
  - `src/Host/ExerciseHostSession.cs`: Host session and checksummed save store.
  - `src/Main.Exercise.cs`: Main partial with setup, save, reset, perform workout, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAllDirect()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `ExerciseDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.Exercise.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan216ExerciseIntegrationTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/ExerciseSystemTests.cs`: 7/7 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --exercise-selftest`: 12/12 PASS

## Non-Goals

No duplicate health damage/vitality systems. Pure Core remains engine-free. Deterministic simulation.
