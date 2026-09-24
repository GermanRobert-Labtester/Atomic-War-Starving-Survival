# UNBLOCK — Plan 185: MemoryDecaySystem / Memory & Knowledge Decay

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-185-memory-decay-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Memory & Knowledge Decay system is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `MemoryDecaySystem` (DEC-340) the live cognition authority for survivor knowledge decay,
memory reinforcement, and cognitive retention while preserving single-owner domain boundaries (Rule 5):

- `SurvivorRelationsState` / `NpcMemoryHostSession` own social opinions and interaction impressions.
- `MemoryDecaySystem` owns factual and cognitive memories across domains (Survival, Crafting, Medical, Wasteland, Lore, Personal), clarity degradation, forgetting events, and deliberate cognitive reinforcement.
- `MemoryDecaySystem` in pure Core manages memory records, decay rates from `Assets/StreamingAssets/Data/memory_decay_rates.json`, clarification threshold queries, reinforcement effects, and census statistics (`MemoryDecayCensus`).
- `MemoryDecayHostSession` and `MemoryDecaySaveStore` provide host-level lifecycle, persistence (`memory_decay` section, `memory_decay_save.json`, section #258), and atomic state operations.
- `MemoryDecayDayOwner` in `Main.CampaignOwners.cs` advances daily memory decay during phase-5 daily simulation, emitting `memory_decay_ticked`.
- `--memory-decay-selftest` validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs`: Pure domain system, state model, records, clarity calculations, and `MemoryDecayCensus`.
  - `Assets/StreamingAssets/Data/memory_decay_rates.json`: Authoritative decay rate definitions.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `memory_decay` registered under `survivors` domain with `ExpandedSurvivorLifecycleGroup` (file: `memory_decay_save.json`).
- **Godot Host (`src/`):**
  - `src/Host/MemoryDecayHostSession.cs`: Host session and checksummed save store.
  - `src/Main.MemoryDecay.cs`: Main partial with setup, save, reset, record, reinforce, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAllDirect()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `MemoryDecayDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.MemoryDecay.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Cognition/Plan185MemoryDecayIntegrationTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs`: 9/9 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --memory-decay-selftest`: 12/12 PASS

## Non-Goals

No duplicate social/relationship memory stores. Pure Core remains engine-free. Deterministic simulation.
