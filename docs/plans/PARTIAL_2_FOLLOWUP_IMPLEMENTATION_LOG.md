# Partial-plan follow-up implementation log — Plans 185 and 162

Date: 2026-09-19
Authority: direct user request following `PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md`

## Bounded outcome

Implement the next two ranked partials only:

1. Plan 185 / C1[33] — Memory Decay.
2. Plan 162 / C2[34] — Shelter Archive.

These are bounded read-model integrations. They do not claim that either
plan is flagship-complete.

## Authority decisions

- Skill progression remains the owner of skill XP, dormancy, active skills,
  and its existing apprenticeship save section.
- Journal remains the owner of journal entries and its existing journal save
  section.
- Memorial remains the owner of death records and its existing memorial save
  section.
- Memory Decay now projects typed canonical facts without storing or mutating
  a second memory ledger.
- Shelter Archive now projects a deterministic searchable index without
  appending parallel archive records or adding a save section.

## Files changed

- `Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs`
- `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs`
- `Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs`
- `Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs`
- `src/Main.Plans162_185.cs`

## Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs` — **9/9 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs` — **8/8 passed**.
- `dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore` — **0 warnings, 0 errors**.
- `dotnet build Ashfall.csproj --no-restore` — **0 warnings, 0 errors**.

## Remaining scope

Plan 185 still needs source-specific mutation/reinforcement routes and any
player-facing surface; those belong to skill, journal, relation, and other
canonical owners. Plan 162 still needs an explicitly owned archive search
route/panel if the product requires one. The remaining ranked partials are
intentionally only placeholders in
`docs/plans/PARTIAL_REMAINING_PLACEHOLDER_2026-09-19.md`.

No live foreman ledgers, debt registers, generated rulebooks, JSON catalogs,
or unrelated dirty changes were changed by this package.
