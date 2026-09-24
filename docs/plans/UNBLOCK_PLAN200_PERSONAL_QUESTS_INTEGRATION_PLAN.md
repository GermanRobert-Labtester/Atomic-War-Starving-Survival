# UNBLOCK — Plan 200: PersonalQuestSystem / Survivor Personal Quests & Character Arcs

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-200-personal-quests-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Survivor Personal Quests & Character Arcs system is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `PersonalQuestSystem` (DEC-341) the live personal quest and character arc authority
for survivors in the shelter, handling trait- and class-based quest triggers, multi-stage progression,
branching moral decisions, rewards, and arc conclusion while preserving single-owner domain boundaries (Rule 5):

- `CrossingQuestSystem` / `DutyRosterQuestSystem` own world and operational assignments.
- `PersonalQuestSystem` owns internal personal motivations, background arcs, trait-driven journeys, and personal resolution states.
- `PersonalQuestSystem` in pure Core manages quest instances, progression through stages, requirements validation (`Assets/StreamingAssets/Data/personal_quests.json`), rewards, and census statistics (`PersonalQuestCensus`).
- `PersonalQuestHostSession` and `PersonalQuestSaveStore` provide host-level lifecycle, persistence (`personal_quests` section, `personal_quests.json`, section #257), and atomic state operations.
- `PersonalQuestDayOwner` in `Main.CampaignOwners.cs` evaluates daily quest conditions and objective updates during phase-5 daily simulation, emitting `personal_quests_ticked`.
- `--personal-quests-selftest` validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`: Pure domain system, state model, quest definitions, stages, choices, and `PersonalQuestCensus`.
  - `Assets/StreamingAssets/Data/personal_quests.json`: Authoritative personal quest catalog.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `personal_quests` registered under `quests` domain with `ExpandedQuestLifecycleGroup` (file: `personal_quests.json`).
- **Godot Host (`src/`):**
  - `src/Host/PersonalQuestHostSession.cs`: Host session managing active quests.
  - `src/Host/PersonalQuestSaveStore.cs`: Checksummed save store.
  - `src/Main.PersonalQuests.cs`: Main partial with setup, save, reset, trigger, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAllDirect()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `PersonalQuestDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/PersonalQuestSelfTest.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.
  - `src/UI/PersonalQuestPanel.cs`: Godot presentation panel for active personal quests.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs`: 4/4 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs`: 10/10 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --personal-quests-selftest`: 12/12 PASS

## Non-Goals

No duplicate campaign quest ledgers. Pure Core remains engine-free. Deterministic simulation.
