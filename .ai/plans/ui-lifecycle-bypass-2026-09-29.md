# UI A11y — Route Bypass Panel Opens Through ShowPanelLifecycle + LineEdit Floor

STATUS: APPROVED BY USER

Date: 2026-09-29
Lane: UI correction (audit series package 12)

## Outcome

**Part A — close the lifecycle bypass (36 sites, 29 Main.* partial files).**
Dozens of overlay panels were opened with bare `<panel>.Visible = true`,
skipping everything `ShowPanelLifecycle` provides (its own doc comment says
all panel opens should use it): the pkg-10 `EnforceControlDefaults` walk
(28px button floor, fixed-width label clipping), the open animation, and
`EnsureInitialFocus` — i.e. keyboard focus never landed on those panels at
all, the exact regression the P1 focus work sealed for routed panels. Every
bypass site is a simple `_*Panel` field statement; all are replaced with
`ShowPanelLifecycle(<panel>);` (compound `if (p != null) { p.Visible = true;
p.RefreshView(); }` sites keep their RefreshView call).

Excluded (intentionally, boot-built always-visible roots with their own
focus flows): `_mainMenu`, `_dashboard`, `_gameOver`, `_gameUiContainer`,
`_hudOverlay`, `_feedbackPanel`, `_confirmationModal`, crisis HUD.

**Part B — LineEdit target floor in `EnforceControlDefaults`.**
Single-line inputs are interactive controls under the same 28px audit rule as
buttons; direct `new LineEdit` sites were font-coupled. The walk now floors
`LineEdit.CustomMinimumSize.Y` to `Theme.MinInteractiveHeight` (larger
explicit sizes win). SpinBox benefits automatically via its internal
LineEdit child.

## Non-goals

- No changes to panel content, layout, or close paths; no new tokens.
- No motion/focus redesign — bypass sites simply get the canonical behavior
  every routed panel already has.

## Verification

1. Host build 0 errors.
2. Static gate `Ashfall.Core.Tests/UI/UiLifecycleBypassGateTests.cs`:
   no `Main.*` partial file outside the exclusion list contains
   `.Visible = true;` (bypass class eliminated, and prevented from
   regrowing); LineEdit floor present in the walk.
3. Headless probes: `--ui-layout-selftest`, `--player-panels-uitest`, boot.
4. Governance: claim row, `.ai/state.md`, pathspec commit, memory.
