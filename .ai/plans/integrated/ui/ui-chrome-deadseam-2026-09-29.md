# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI Chrome Precision + Dead-Seam Deletion — pkg 13 (2026-09-29)

STATUS: APPROVED BY USER
(Batch mandate: "Continue with a larger batch of doing more UI correction and
UI precision work" — 2026-09-29 session; continuation of the a11y series,
pkgs 1–12 complete through 7fb37c2f9.)

## Evidence (verified in source 2026-09-29)

1. `src/UI/AshfallUiTheme.cs` `Build()` covers Button/LineEdit/ItemList/
   Slider/ScrollBar/ProgressBar/CheckBox/OptionButton/SpinBox/TabContainer/
   TabBar/RichTextLabel/PopupMenu — but has **zero** entries for
   `TooltipPanel`/`TooltipLabel`, `HSeparator`/`VSeparator`, and the base
   `Label` type. Consequences: tooltips render Godot's light grey default
   bubble on ink panels; separator lines render default light grey; all ~359
   direct `new Label` sites that bypass the factory seam render Godot's
   default typeface/size/white color instead of the brand font.
2. `src/UI/ModalManager.cs` (134 lines) is unwired dead code: the audit
   (docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §5) recorded it as a dead
   seam, and its only reference is a smoke check in
   `src/Audio/AudioSelfTest.cs:1087-1089`. Pkg 12 (7fb37c2f9) routed every
   panel open through `ShowPanelLifecycle` with a regrowth-banning gate, and
   the P3 package wired `AshfallFocusPolicy.TrapFocus` into `Main._Input` —
   `ModalManager`/`ModalStackController` are superseded as the open/focus
   authority. Adopting them would create a parallel open-path authority,
   which AGENTS.md rule 5 forbids; deletion is the governed resolution of
   the long-standing adopt-or-delete flag.
3. `Assets/Ashfall.Core/UI/ModalStackController.cs` contains the Core
   `IModalPanel` contract (LIVE: extended by `src/UI/IModalPanel.cs`,
   implemented by `ConfirmationModal`, `MoralChoiceModal`, registered by
   `Main.PlayerSurfaces.cs:948`) and the `ModalStackController<,>` state
   machine (DEAD: only consumer is the deleted `ModalManager`; its test
   `Ashfall.Core.Tests/UI/ModalStackControllerTests.cs` tests only that dead
   class).
4. `AshfallFocusNavigator.TickStickRepeat` (src/UI/AshfallFocusNavigator.cs:110)
   has zero callers — also dead (audit §5b dead-code finding).

## Changes

### Part A — theme chrome coverage (src/UI/AshfallUiTheme.cs, inside Build())

- `TooltipPanel`: flat ink 0.97 panel with `line` border (tooltips stop
  rendering Godot's light default bubble).
- `TooltipLabel`: `font_color` Pale, `font_size` FontSizeBody, Barlow
  regular via `SetFontIfAvailable`.
- `HSeparator` / `VSeparator`: flat `lineSoft` `separator` stylebox.
- Base `Label`: `font` (Barlow regular), `font_color` Pale,
  `font_size` FontSizeBody — brand typeface for every factory-bypassing
  `new Label` site; factories keep their per-label overrides.

### Part B — dead-seam deletion

- Delete `src/UI/ModalManager.cs`.
- Split `Assets/Ashfall.Core/UI/ModalStackController.cs`: keep the
  `IModalPanel` contract as `Assets/Ashfall.Core/UI/IModalPanel.cs`; delete
  the `ModalStackController<,>` class and the file.
- Delete `Ashfall.Core.Tests/UI/ModalStackControllerTests.cs` (tests only
  the deleted class).
- `src/Audio/AudioSelfTest.cs`: remove the ModalManager instantiation smoke
  check.

### Part C — gates

New `Ashfall.Core.Tests/UI/UiChromeDeadSeamGateTests.cs`:
- Theme rows for TooltipPanel/TooltipLabel/HSeparator/VSeparator/Label
  (must live inside `Build()`), pattern: UiThemeCoverageGateTests.
- Repo-scan facts: no `ModalManager`/`ModalStackController` references under
  `src/` or `Assets/Ashfall.Core/` (bans regrowth), and
  `TickStickRepeat` deleted.

## Non-goals

- No adopt/rewire of a modal stack (forbidden parallel authority).
- No per-site conversion of the ~359 `new Label` sites (theme default now
  covers their font/color; per-site FinishLabel adoption stays an open
  incremental item).
- No snapshot-golden regen (visual lane, unchanged open item).
- No changes to panel content, save/state, or Core gameplay.

## Verification

1. `dotnet build` host solution — 0 errors.
2. `scripts/run_test.sh Ashfall.Core.Tests/UI/UiChromeDeadSeamGateTests.cs` — PASS.
3. Focused Core tests for touched areas (AudioSelfTest-adjacent none; theme
   gates) — PASS.
4. Headless probes: `--ui-layout-selftest`, `--player-panels-uitest`,
   audio selftest (ModalManager check removed), boot.
5. Pathspec commit; plan header FULLY INTEGRATED; archive to
   `.ai/plans/integrated/ui/`; claim row; `.ai/state.md`; memory.
