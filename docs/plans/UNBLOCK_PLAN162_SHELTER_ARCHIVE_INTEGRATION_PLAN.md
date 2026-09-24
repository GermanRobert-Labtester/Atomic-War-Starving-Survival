# UNBLOCK — Plan 162: ShelterArchiveSystem / Shelter History & Archive

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-162-shelter-archive-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Shelter History & Archive system is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `ShelterArchiveSystem` (DEC-337) the live chronicling and history authority
for shelter timeline records, historical milestones, governance decisions, memorial casualty tributes,
and external discoveries while preserving existing single owners (Rule 5):

- `JournalSystem` owns raw chronological narrative journal entries;
  `CampaignChronicleService` owns high-level campaign summaries;
  `ShelterArchiveSystem` owns structured categorical records, discovery logging,
  governance decrees, memorials, keyword indexing, and temporal search.
- `ShelterArchiveSystem` in pure Core manages entries, categories (`archive_categories.json`),
  projections from canonical sources, search indexes, and census statistics (`ShelterArchiveCensus`).
- `ShelterArchiveHostSession` and `ShelterArchiveSaveStore` provide host-level
  lifecycle, persistence (`shelter_archive` section, `shelter_archive_save.json`, section #256),
  and atomic operations.
- `ShelterArchiveDayOwner` in `Main.CampaignOwners.cs` advances daily archive chronology
  and processes timeline milestones during phase-5 daily simulation, emitting `shelter_archive_ticked`.
- `--shelter-archive-selftest` (alias `--archive-system-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs`: Pure domain system, state model, entries, and `ShelterArchiveCensus`.
  - `Assets/StreamingAssets/Data/archive_categories.json`: Authoritative archive category definitions.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `shelter_archive` registered under `shelter` domain with `ExpandedShelterLifecycleGroup` (file: `shelter_archive_save.json`).
- **Godot Host (`src/`):**
  - `src/Host/ShelterArchiveHostSession.cs`: Host session and checksummed save store.
  - `src/Main.ShelterArchive.cs`: Main partial with setup, save, reset, record, milestone, memorial, search, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetEnrolledFlagshipSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `ShelterArchiveDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.ShelterArchive.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan162ArchiveIntegrationTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs`: 8/8 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: 5/5 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --shelter-archive-selftest`: 12/12 PASS
- `godot --headless --path . -- --archive-system-selftest`: 12/12 PASS

## Non-Goals

No duplicate narrative/journal stores. No unseeded randomness.
No Unity dependencies or invocation.
