# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI Sidebar Row Collapse Fix + Snapshot Golden Rebaseline — pkg 16 (2026-09-29)

STATUS: APPROVED BY USER
(Batch mandate: "Continue working on remaining UI" — 2026-09-29 session.)

## Finding (root-caused via xvfb snapshot capture, not assumed)

The first golden regen attempt since 2026-09-26 (commit c8c1e453d) exposed a
**live layout regression** the stale goldens could never show: every
`AshfallSidebar` row collapsed to ~stylebox-margins height and its
label/hint text piled up on one line (20 sidebar-bearing panels affected).

Root cause: the P2.5 keyboard-reachability package (02c48fa92) converted
sidebar rows from `PanelContainer` to `new Button { Text = string.Empty }`
with a `MarginContainer → VBox → label + hint` child. **Godot Buttons are
not containers** — a child Control neither contributes to the button's
minimum size nor gets laid out by it. Under PanelContainer the labels
drove row height; under Button the row's minimum height became just the
empty-text stylebox margins. Static gates and the panel-bind uitest never
measured geometry, so it shipped unnoticed.

## Changes

1. `src/UI/AshfallSidebar.cs` `AddRow`: size the row from its content —
   `row.CustomMinimumSize = new Vector2(0, rowMargin.GetCombinedMinimumSize().Y)`
   before `_list.AddChild(row)`. Restores the exact two-line geometry the
   PanelContainer rows had; selected/hover/focus chrome unchanged.
2. `Ashfall.Core.Tests/UI/UiA11ySidebarHoverOverflowGateTests.cs`: new fact
   `SidebarButtonRows_SizeFromTheirContent` — the content-derived floor
   must stay inside `AddRow` (bans regrowth of the container-child-in-
   Button shape without sizing).
3. Snapshot golden rebaseline: `--ui-snapshot-regenerate` under
   `xvfb-run` (font lift 11→12, pkg 11 theme coverage, pkg 13 chrome
   entries, and this fix all landed after the 2026-09-26 goldens), then a
   verification diff run: 32/32 match, 0 drift, 0 fail. Regenerated
   goldens committed in a follow-up `snapshots/`-only commit.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. `scripts/run_test.sh Ashfall.Core.Tests/UI/UiA11ySidebarHoverOverflowGateTests.cs` — 9/9 PASS.
3. Capture inspection: sidebar rows stacked with full chrome + selected
   highlight (inventory capture, before/after crops compared).
4. `--ui-layout-selftest` 0 FAIL; `--player-panels-uitest` 22/22.
5. Two pathspec commits: (fix + gate + plan), (snapshots/ goldens).

## Non-goals

- No audit of other Button-with-container-children sites (grep found this
  to be the only `new Button { Text = empty }` + child subtree site).
- No §3 scrim work here (separate package).
