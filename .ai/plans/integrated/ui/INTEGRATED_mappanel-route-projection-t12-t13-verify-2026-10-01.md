# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user 2026-10-01 — "T12 and T13 lets go for!"; integrated and archived in the same session.)

# T12 — MapPanel fabricated route rows → canonical projection (+ T13 verify)

> **STATUS: APPROVED BY USER**
> (User-authorized 2026-10-01: "T12 and T13 lets go for!" — follow-ups to the
> T11 map-panel repair from the same line-255 flag list in `.ai/state.md`.)

## Goal

1. **T12:** `MapPanel` "DISCOVERED TRANSIT CORRIDORS" hardcodes four fabricated
   route rows ("Holdfast ↔ Allotments [5 Ticks, Safe]" etc.). Replace them
   with a canonical, fog-gated projection of the authored map routes.
2. **T13:** verify the flagged `WorkshopPanel`/`PharmaLabPanel` discarded
   `ActionResult`s defect — premise audit first, repair only if still present.

## Evidence (premise audit, 2026-10-01)

- **T13 premise is STALE — already fixed by the T10 stream.** Both panels
  now route every typed refusal through the shared
  `src/UI/ActionRefusalText.cs` formatter (`RecordRefusal` /
  `_refusalLabel.Text = ActionRefusalText.Line(...)`); the matrix
  `docs/ACTION_RESULT_SURFACING_MATRIX.md` already carries the Pharma lab
  and Workshop rows with full code lists; and
  `ActionResultSurfacingGateTests.DirectPanels_RenderRefusalsThroughSharedFormatter`
  already pins both files. Per Rule 7 / repair-skill falsification: no repair
  will be duplicated — T13 closes as verify-only (gate run must be green).
- **T12 premise is LIVE.** `MapPanel.RefreshView` still renders the four
  hardcoded rows. Real authority exists: `WastelandMapSystem.Routes`
  (`wasteland_map_v1.json`: 22 nodes, 68 routes, `distanceKm` +
  `weatherHazard`, currently no tags, all `land` domain) with player-facing
  `GetNodeIntel` views. Canonical fog rule already ratified by
  `MapAtlasPanel`: a route is charted only when **both endpoints** are
  fog-known (`GetNodeIntel != null && FogState != Unknown`).

## Repair (T12)

`src/UI/MapPanel.cs` routes card:

- Iterate `map.Routes`; skip routes whose either endpoint is not fog-known
  (same rule as `MapAtlasPanel.ReloadLocations`/status rail).
- Render each charted corridor with endpoint intel display names and real
  metadata: `{DistanceKm:0.0} km · {TravelDomain upper}` + weather hazard
  percentage when > 0 + authored tags uppercased (none today; future-proof).
  Hazardous corridors (flooded or hazard ≥ 0.5) render Warm, others Pale.
- Empty states: no canonical map bound → truthful line; zero charted
  corridors → "None on record — survey adjacent sectors to chart transit
  corridors." No fabricated content remains in the card.

## Invariants

- No Core change; no new catalog, save section, or parallel state.
- Deterministic render order: authored `Routes` order (stable).
- Deep-coast / warlord / active-expedition sub-cards unchanged (already real).
- Charted corridors render only player-known geography (no fog leak).

## Non-goals

- The canonical-baseline fallback location rows in the locations section
  (separate follow-up T16 family).
- No matrix/test changes for T13 (already integrated); no commit; no full
  suite.

## Test plan

- Extend `Ashfall.Core.Tests/UI/MapPanelTruthfulnessGateTests.cs` (T11 gate,
  same owner): ban the distinctive fabricated route literals; pin the
  canonical projection (`GetNodeIntel(route.From)`/`(route.To)` + truthful
  empty state).
- Run: both map gate files + `ActionResultSurfacingGateTests` (T13 verify)
  via `scripts/run_test.sh`; `dotnet build Ashfall.csproj` 0 errors;
  `--ui-layout-selftest` (panel render path).

## Definition of done

Gates green, build clean, plan marked FULLY INTEGRATED and archived,
`.ai/state.md` updated, claim released.

## Integration record (2026-10-01)

- Changed files: `src/UI/MapPanel.cs` (routes card: four fabricated rows
  replaced with the canonical fog-known projection over
  `WastelandMapSystem.Routes` — endpoint names via `GetNodeIntel`, real
  `distanceKm`/`travelDomain`/`weatherHazard`/tags metadata, Warm for
  hazardous corridors, truthful empty states for no-map/none-charted) and
  `Ashfall.Core.Tests/UI/MapPanelTruthfulnessGateTests.cs` (two new facts:
  banned fabricated route literals; canonical projection pin).
- T13: **no code change — premise was already fixed by the T10 stream.**
  Verified in source: both panels route refusals through shared
  `ActionRefusalText`, matrix rows exist,
  `ActionResultSurfacingGateTests.DirectPanels_RenderRefusalsThroughSharedFormatter`
  pins both files and passes 3/3.
- Verification: `MapPanelTruthfulnessGateTests` 5/5, `PanelSubscriptionHygieneTests`
  2/2, `ActionResultSurfacingGateTests` 3/3 — all PASS via
  `scripts/run_test.sh`; `dotnet build Ashfall.csproj` 0 errors (6 warnings =
  documented benign CS0162 set in foreign `src/Host/HostCli.*` files, none
  from this change); headless `--ui-layout-selftest` PASS; `git diff --check`
  clean. No commit; full suite not run; foreign dirty work untouched.
