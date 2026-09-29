# UI A11Y P2.5 + P3 SIDEBAR KEYBOARD ACCESS, HOVER FEEDBACK, OVERFLOW PRECISION — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue doing more UI correction and UI
precision work!" (2026-09-29). Sixth package in the audit-fix series: the one
remaining unimplemented P2 item (§9.5 functionality) plus the P3 hover
feedback gaps (§7/§9.13) and the remaining fixed-width overflow sites (§6).

## Bounded outcome

1. **Sidebar nav rows become keyboard-reachable (P2.5).** Rows are
   PanelContainer+Label with mouse-only `GuiInput` (`AshfallSidebar.cs:167-174`)
   — unreachable by keyboard/controller and invisible to the focus navigator
   (`FindFocusableControls` collects Button-family only). Fix: convert the row
   container to a flat `Button` with per-row styleboxes mirroring today's
   look (normal Ink 0.40, hover Warm 0.12, pressed Warm 0.20, focus =
   the shared `MakeFocusVisibleStyleBox` ring), `Pressed` → `Select(item.Id)`.
   This yields keyboard activation (ui_accept), navigator/Tab-trap
   eligibility, and hover feedback in one move. `SetRowHighlight` mutates the
   per-row `normal` stylebox instead of `panel`.
2. **Grid row hover feedback (P3 §7).** Selectable `AshfallDataGrid` rows get
   `MouseEntered`/`MouseExited`: a Warm 0.10 hover fill while not selected,
   restored via the existing `ApplyRowStyle`/selection path on exit. Selection
   styling stays owned by `RefreshRowHighlights`.
   Not done (engine-limited, noted): per-item ItemList hover (no hovered-item
   stylebox in Godot), SpinBox arrow theming, RichTextLabel link hover.
3. **Remaining fixed-width overflow labels (precision §6).** `ClipText` +
   `TextOverrunBehavior.TrimEllipsis` on: TradeScreenGodotPanel item-name
   labels (100/120/140px), AshfallMetricCard `_valueLbl` (mono value on
   110-180px cards), SurvivorsPanel survivor display name (140px),
   GameDashboardPanel gauge row names (74px). Same correction pattern as the
   P3 grid/shell package.

## Exact files

- `src/UI/AshfallSidebar.cs` — AddRow row type + styleboxes + Pressed;
  SetRowHighlight selector
- `src/UI/AshfallDataGrid.cs` — hover handlers in the selectable-row block
- `src/Economy/TradeScreenGodotPanel.cs` — 3 label sites
- `src/UI/AshfallMetricCard.cs`, `src/UI/SurvivorsPanel.cs`,
  `src/UI/GameDashboardPanel.cs` — 1 label site each
- `Ashfall.Core.Tests/UI/UiA11ySidebarHoverOverflowGateTests.cs` — new gate
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- No Core changes; no ItemList/SpinBox/RichTextLabel theming (engine-limited,
  recorded); no layout/size changes beyond clip behavior; no data/save paths.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Gate tests via `scripts/run_test.sh` + adjacent UI gates.
3. `--ui-layout-selftest` + `--player-panels-uitest` headless runtime probes.
