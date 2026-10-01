# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user 2026-10-01 — "continue with T14, T15, T16 Full scale
> integrate!"; integrated and archived in the same session.)

# T14/T15/T16 — expedition console routing + MapPanel counter truthfulness + l10n verify

> **STATUS: APPROVED BY USER**
> (User-authorized 2026-10-01: "continue with T14, T15, T16 Full scale
> integrate!" — follow-ups to the T11/T12 map-panel repairs from the task
> 14/15 flag list in `.ai/state.md`.)

## Goal

1. **T14:** resolve the foreman-flagged unreachable
   `ExpeditionRadarPanel`/`ExpeditionCampPanel` routing — the last open item
   of the task-14 flag list.
2. **T15:** remove the remaining fabricated numbers/rows in `MapPanel` —
   the four invented fallback location cards, the `Math.Max(total, 8)`
   waypoint floor, and the static 8/4/12/6 "exploration archive" counters
   plus the fabricated "100% Deterministic Seed Verification" row.
3. **T16:** the `strings.csv` EN `discovery.micro_frozen_bus.description`
   divergence (task 16 scope from task 14's handoff).

## Evidence (premise audit, 2026-10-01)

- **T14 — WIRE, not retire.** Both panels are truthful, live surfaces:
  `ExpeditionCampPanel` delegates entirely to `ExpeditionHostSession`
  (CampTick/BreakCamp/GetCampState/ResolveCampEncounter); `ExpeditionRadarPanel`
  renders `_host.Definitions`, active sorties, stamina, cargo, encounters.
  Both routes are registered in `PanelRegistryBootstrap` as
  `PanelGroup.Secondary` with default `PanelMaturity.Live` (player-navigable),
  both have `ConfigureActions` bind/open entries in
  `src/Main.PlayerSurfaces.cs`, and both panels are instantiated in
  `Main.UiPanels.cs`. The ONLY missing piece: no surface ever emits
  `"expedition_radar"`/`"expedition_camp"` (grep across src/ finds zero
  emitters). `OpenPlayerPanel(panelId)` resolves the descriptor and invokes
  `descriptor.Bind()` + `descriptor.Open()`, so one emitter per route is
  sufficient. `SnapshotHarness` also renders the radar panel — deletion
  would break the snapshot lane.
- **T15 — premise LIVE.** `MapPanel` lines: fallback invents 4 location
  cards with fabricated descriptions when no catalog/expedition data is
  bound; overview row clamps waypoints to `Math.Max(totalLocations, 8)`;
  the "SURVEY MEMORY & SITE LAYOUTS" card prints constants
  (`8/4` grids, `12/6` memories) and a fabricated
  "100% Deterministic Seed Verification" row. Real authorities exist:
  `LocationLayoutSystem.State.parents[*].unlockedRoomIds` (mapped grids),
  `LocationMemorySystem.State.visitCounts` (recorded site memories) and
  `.StratumCount` (authored strata).
- **T16 — premise STALE, closed verify-only.** A foreign stream already
  corrected `assets/l10n/strings.csv` and added the permanent gate
  `StringsCsvLocaleGateTests.Catalog_MicroLocationEnglishMatchesAuthoritativeJson`
  (JSON is authoritative). Verified: that gate passes 4/4 and
  `--expedition-panel-uitest` is now **59/59 PASS, 0 FAIL**. Per Rule 7 no
  duplicate fix is made.

## Repair

- **T14** (`src/UI/ExpeditionPanel.cs`, additive hunks in
  `src/Main.UiPanels.cs`): add two deep-link console buttons in the
  expedition prep area — "RADAR SWEEP CONSOLE" and "OVERNIGHT CAMP CONSOLE"
  — raising `OnOpenRadarRequested`/`OnOpenCampConsoleRequested`; wire them
  in `Main.UiPanels.cs` next to the existing panel link subscriptions as
  `OpenPlayerPanel("expedition_radar")` / `OpenPlayerPanel("expedition_camp")`
  (same pattern as `_craftingPanel.OnOpenWorkshopRequested`). No
  PanelRegistryBootstrap or PlayerSurfaces changes (claimed by an ACTIVE
  foreign claim; also not needed — routes and actions already exist).
- **T15** (`src/UI/MapPanel.cs`):
  - Fallback: when no catalog/expedition data is bound, render one truthful
    "UNCHARTED REGION — no cataloged waypoints" card instead of four
    fabricated locations (no INSPECT button without an id).
  - Overview: `{totalLocations} Sector Coordinates` (drop the 8 floor).
  - Exploration archive card: real counts — mapped grids =
    sum of `Layouts.State.parents[*].unlockedRoomIds.Count`; site memories =
    `Memory.State.visitCounts.Count`; replace the fabricated integrity row
    with authored `Memory.StratumCount`. All zero when `_expansions` unbound.

## Invariants

- No Core change; no new catalog, save section, or parallel state.
- Routing decisions stay in the host; ExpeditionPanel only raises events.
- No edits to foreign-ACTIVE-claimed paths (`PanelRegistryBootstrap.cs`,
  `Main.PlayerSurfaces.cs`).
- Deterministic render (authored/state order); no fog leak (unchanged gates).

## Non-goals

- Radar/camp feature work beyond routing (panels already render real data).
- No commit; no full suite; foreign dirty hunks preserved.

## Test plan

- `PanelRouteGateTests`: new fact pinning both console routes are registered
  Secondary AND emitted (`ExpeditionPanel` events) AND wired
  (`OpenPlayerPanel("expedition_radar"/"expedition_camp")`).
- `MapPanelTruthfulnessGateTests`: ban the fabricated fallback/survey
  literals ("filtration stack", "Seed Verification", `Math.Max(totalLocations`,
  …) and pin the real counter sources.
- Runs: both gate files + `PanelSubscriptionHygieneTests` via
  `scripts/run_test.sh`; `dotnet build Ashfall.csproj`; headless
  `--expedition-panel-uitest` (T14 touch + T16 verify, expect 59/59) and
  `--ui-layout-selftest`.

## Definition of done

Gates green, build clean, plan marked FULLY INTEGRATED and archived,
`.ai/state.md` updated, claim released.

## Integration record (2026-10-01)

- Changed files: `src/UI/ExpeditionPanel.cs` (two console deep-link buttons
  + events, additive), `src/Main.UiPanels.cs` (two additive wiring lines:
  `OpenPlayerPanel("expedition_radar")` / `OpenPlayerPanel("expedition_camp")`),
  `src/UI/MapPanel.cs` (truthful "UNCHARTED REGION" fallback card replacing
  four fabricated locations; `{totalLocations}` waypoint count without the
  8 floor; real archive counters from `Layouts.State.parents[*].unlockedRoomIds`,
  `Memory.State.visitCounts`, `Memory.StratumCount`; fabricated "100%
  Deterministic Seed Verification" row removed),
  `Ashfall.Core.Tests/UI/PanelRouteGateTests.cs` (new
  `ExpeditionConsoleRoutes_AreEmittedAndWired` fact),
  `Ashfall.Core.Tests/UI/MapPanelTruthfulnessGateTests.cs` (new
  `MapPanel_LocationFallbackAndArchiveCountersAreReal` fact + banned
  literals).
- T16: no code change — verified the foreign stream's fix
  (`StringsCsvLocaleGateTests` 4/4; `--expedition-panel-uitest` 59/59, 0 FAIL).
- Verification: truthfulness 6/6, route gate 22/22, hygiene 2/2 — all PASS
  via `scripts/run_test.sh`; `dotnet build Ashfall.csproj` 0 errors;
  headless `--expedition-panel-uitest` 59 PASS / 0 FAIL (exercises the
  panel `_Ready` containing the new buttons); `--ui-layout-selftest` PASS;
  `git diff --check` clean.
- Mid-run incident (foreign, resolved): a concurrent stream's in-flight
  `Ashfall.Core.Tests/Combat/RealtimeRestoreHardeningTests.cs` (untracked)
  briefly broke the test-project compile (`BeginFight` overload mismatch
  with foreign-modified `TacticalCombatSystem.*`); the stream landed its
  fix and the final hygiene run passed 2/2. No foreign file was touched.
- With this batch, the ENTIRE task-14/15 foreman flag list is retired. No
  commit; full suite not run; foreign dirty work untouched (additive-only
  edits to foreign-dirty `Main.UiPanels.cs` / `ExpeditionPanel.cs`).
