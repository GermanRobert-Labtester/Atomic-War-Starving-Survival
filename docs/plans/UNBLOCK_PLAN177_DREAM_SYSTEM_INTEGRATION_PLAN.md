# UNBLOCK — Plan 177: DreamSystem / Dream & Sleep Event System

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-177-dream-system-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Dream & Sleep Event system is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `DreamSystem` (DEC-338) the live calculation and narrative authority
for nocturnal psychological processing, dream experience generation, rest quality modifiers,
consecutive nightmare tracking, and psychological dream interpretations while preserving existing single owners (Rule 5):

- `NeedsSystem` owns survivor primary Needs (Fatigue, Numbness, Hunger, etc.);
  `SleepAcousticLedger` owns acoustic bunk soundproofing and decibel relief;
  `SleepNarrativeProjection` owns sleep narrative beat projections;
  `DreamSystem` owns dream taxonomy resolution (peaceful, nightmare, memory, prophetic),
  nightmare accumulation loops, traumatic sleep events, rest bonuses/penalties, and therapeutic interpretation.
- `DreamSystem` in pure Core manages templates (`dream_templates.json`),
  seeded sleep dream evaluations, nightmare fatigue debuffs, dream interpretation, and census statistics (`DreamCensus`).
- `DreamHostSession` and `DreamSaveStore` provide host-level
  lifecycle, persistence (`survivor_dreams` section, `survivor_dreams_save.json`, section #257),
  and atomic operations.
- `SurvivorDreamsDayOwner` in `Main.CampaignOwners.cs` processes nocturnal dream cycles
  for sleeping survivors during phase-5 daily simulation, emitting `survivor_dreams_ticked`.
- `--dream-system-selftest` (alias `--dreams-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Survivors/DreamSystem.cs`: Pure domain system, state model, templates, and `DreamCensus`.
  - `Assets/StreamingAssets/Data/dream_templates.json`: Authoritative dream template catalog.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `survivor_dreams` registered under `survivors` domain with `ExpandedShelterLifecycleGroup` (file: `survivor_dreams_save.json`).
- **Godot Host (`src/`):**
  - `src/Host/DreamHostSession.cs`: Host session and checksummed save store.
  - `src/Main.DreamSystem.cs`: Main partial with setup, save, reset, evaluate, interpret, query history, tick, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetEnrolledFlagshipSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `SurvivorDreamsDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.DreamSystem.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan177DreamSleepIntegrationTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: 5/5 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --dream-system-selftest`: 12/12 PASS
- `godot --headless --path . -- --dreams-selftest`: 12/12 PASS

## Non-Goals

No duplicate fatigue or trauma meters. Seeded PRNG only (`SeededRng`).
No Unity dependencies or invocation.
