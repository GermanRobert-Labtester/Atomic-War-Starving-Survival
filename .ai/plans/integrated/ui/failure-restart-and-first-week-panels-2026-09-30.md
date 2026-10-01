# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Failure/Restart Path Proof + First-Week Panel Set (Tasks 9 & 10)

> **STATUS: APPROVED BY USER**

User-authorized package (2026-09-30): "Proceed with these next coding tasks!
9. Prove the failure and restart path… 10. Set a first-week panel set…"

## Bounded outcome

1. **Task 9 — failure/restart proof.** A new headless selftest
   (`--failure-restart-selftest`) drives only real production entry points:
   `StartNewGame` → survivor deaths through the `SurvivorFateSystem` pipeline →
   `OnLastSurvivorDied` → `ShowGameOver` (terminal slot seal) → `ReturnToMenu`
   → `StartNewGame` again (no stale state) → Continue after a simulated crash
   (`TryLoadAndRestoreGame`) → a corrupt-`campaign.json` recovery check
   (fail-closed load with live session intact, then `FindRecoverableBackup` /
   `RecoverBackup` restore).
2. **Task 10 — first-week panel set.** The dashboard navigation rail
   (`GameDashboardPanel.BuildNavigationRail`) gains a foregrounded "WEEK ONE"
   section with the 8 panels a player needs in week 1 — overview, inventory,
   greenhouse, survivors, map, journal, save, settings — at the top of the rail,
   visible without scrolling. Every other surface stays reachable in the
   existing scrollable sections (reachable, not foregrounded). Time-to-menu is
   measured with the existing `FrameStartupProfiler`
   (`ASHFALL_STARTUP_PROFILE_PATH`) across three headless boots.

## Non-goals

- No Core gameplay-system changes, no new save sections, no new parallel
  authorities, no panel removal (every existing route stays reachable).
- No full test suite; focused verification only per TEST_POLICY.md.
- No commit (user did not request one).

## Exact files

- `src/Main.UiTests.FailureRestart.cs` (new — selftest body)
- `Assets/Ashfall.Core/HostCliRegistry.cs` (enum member + descriptor)
- `src/Host/HostCli.cs` (enum mirror, arg parse, help line)
- `src/Main.Application.cs` (dispatch case)
- `src/UI/GameDashboardPanel.cs` (nav rail week-one section)
- `.ai/state.md`, this plan, its archive entry, and the
  `WORKTREE_OWNERSHIP.md` claim entry.

## Verification

- `godot --headless --path . --max-fps 15 -- --failure-restart-selftest`
- `godot --headless --path . --max-fps 15 -- --player-panels-uitest`
- `godot --headless --path . --max-fps 15 -- --ui-layout-selftest`
- `dotnet build Ashfall.csproj --no-restore -v:minimal` (0 errors)
- `ASHFALL_STARTUP_PROFILE_PATH=<tmp> godot --headless --path .` ×3
  (time-to-menu evidence)

## Integration result (2026-09-30)

- `--failure-restart-selftest`: 43/43 PASS (game over → menu → new game →
  crash-continue → corrupt-save fail-closed → verified backup recovery, all
  through real production entry points).
- Week-one nav rail foregrounded in `GameDashboardPanel.BuildNavigationRail`;
  all other surfaces remain reachable; `--player-panels-uitest`,
  `--ui-layout-selftest`, `--dashboard-uitest` all PASS.
- Time-to-menu measured with FrameStartupProfiler: 14701 / 23265 / 20275 ms
  across three headless Debug boots (baseline 2026-09-27: 9909/11588 ms).
- Build: 0 errors, 6 pre-existing warnings. No commit, no full suite.
