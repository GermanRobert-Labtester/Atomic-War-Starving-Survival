# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user 2026-10-01 — tasks 14 + 15 of the playability checklist; integrated and archived in the same session.)

## Goal

1. **Task 14 — expedition/map loop UI audit:** verify in the rendered UI that the
   player can pick a destination, see risk, dispatch, and get results back, and
   that a fogged node gives clear feedback instead of a silent or wrong refusal.
2. **Task 15 — "why did this fail" layer:** check craft, cook, assign duty, and
   travel refusals against `docs/ACTION_RESULT_SURFACING_MATRIX.md` and make
   sure every common refusal reaches the screen as readable text.

## Non-goals

- No Core changes; no new architecture, catalog, or save section.
- No fix for the pre-existing `strings.csv` localization divergence (task 16 scope).
- No changes to the unreachable `ExpeditionRadarPanel`/`ExpeditionCampPanel`
  routing (foreman architecture decision, flagged below).
- No commit; full suite not run.

## Evidence (premise audit, read-only)

- Plan 32 fog gating is live at Core/host level: `ExpeditionHostSession.GetBlockReason`
  returns six distinct reasons incl. `"Unmapped — no route knowledge"` for
  `MapFogState.Unknown`; `DispatchSortie` returns typed `CommandResult`
  (`unmapped`, `crossing_closed`, `route_blocked`, `vehicle_unready`, …).
- Gaps found: `ExpeditionPanel` rendered every block reason as the hardcoded
  `[CROSSING GATE CLOSED — no vouch]`, discarded the `DispatchSortie` result
  (silent no-op), and its estimate line always described the hardcoded default
  target (`_selectedTargetId` write-never). `MapPanel` rendered fog-Unknown
  sectors as `DISCOVERED` with full hazard numbers. Craft `Start` results were
  discarded in both craft panels; `CraftingPanel._craftSubmitting` reset only on
  success, bricking the button after a runtime refusal. Loot return printed to
  console only. Cook (kitchen) and duty roster already surfaced typed codes.

## Changes (files)

- `src/UI/ExpeditionPanel.cs` — per-card block reason from `GetBlockReason`
  (fog/clue/map/weather gates now named), `TooltipText` on disabled dispatch
  buttons, new `_dispatchStatusLabel` rendering `DISPATCH REFUSED — <prose>`
  from `FormatDispatchRefusal(result.FailureCode)`, per-card `ESTIMATE` button
  wiring `_selectedTargetId` so the risk estimate describes the selected card.
- `src/UI/MapPanel.cs` — Plan 32 fog gate honored in the location list: a node
  that exists on the graph, is undiscovered, and is `MapFogState.Unknown` now
  renders `[UNCHARTED] · no survey data — dispatch refused until this sector is
  mapped` instead of `DISCOVERED` with fabricated hazard numbers.
- `src/UI/CraftingPanel.cs` — craft result captured; on refusal the debounce is
  released and a `REFUSED — <prose>` line (shared `FormatCraftRefusal`) appears
  on the card; success path unchanged.
- `src/UI/SurvivalWorkstationPanel.cs` — `START SELECTED` result captured;
  refusal rendered in a new `_startStatus` label via the shared formatter.
- `src/Main.UiPanels.cs` — `OnLootDeposited` now also emits a success toast
  through the existing `FeedbackPanel.ShowToast` pipeline (console log kept).
- `docs/ACTION_RESULT_SURFACING_MATRIX.md` — added Craft, Cook (kitchen), and
  Duty Roster rows with their representative typed codes and projection
  surfaces; Expedition row extended with the dispatch codes now surfaced.

## Verification (commands and results)

- `dotnet build Ashfall.csproj --no-restore -v:minimal` — 0 errors / 6
  pre-existing warnings in untouched `src/Host/HostCli.*`.
- `--expedition-panel-uitest` — 58 PASS / 1 FAIL; the single failure
  (`longest description text preserved without truncation`) is pre-existing at
  HEAD: `strings.csv` EN `discovery.micro_frozen_bus.description` is missing the
  JSON's trailing clause. Not caused by this package; flagged for task 16.
- `--player-panels-uitest` — PASS 22/22.
- `--ui-layout-selftest` — PASS.
- `--expedition-selftest` — PASS 43/43.
- `git diff --check` on all changed files — clean.

## Limitations / flagged for foreman

- Pre-existing red: `--expedition-panel-uitest` localization divergence above
  (task 16 scope; string-freeze policy governs the fix).
- `ExpeditionRadarPanel` and `ExpeditionCampPanel` are registered but have no
  navigation path into them; `LongWalkExpeditionPanel` is a dead shell behind a
  redirect. Routing them is an unmade architecture decision.
- `MapPanel` still shows static fabricated route/integrity rows and
  `MapDetailPanel` fabricates hazard rows for unknown sectors — display-only,
  left untouched (would need a live-route projection decision).
- `WorkshopPanel`/`PharmaLabPanel` also discard `ActionResult`s (outside the four
  audited families); same one-line pattern applies if the foreman wants them.
- `MapPanel.Bind` double-subscribe leak (`OnMarkersChanged` not unsubscribed) —
  pre-existing, out of scope, flagged.
