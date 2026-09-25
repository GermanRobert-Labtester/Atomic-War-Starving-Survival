# Keyboard / Controller First-Hour Walkthrough (evidence)

Scope: prove the first-hour onboarding journey is completable without a mouse, and record what
is automated versus what still needs a human pass. Source of the journey:
`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs` (`FirstHourOrder`, 7 stages).

## Stage → surface → keyboard path

| # | Stage | Hint route | Surface opened by the hint | Entry control & keys |
|---|---|---|---|---|
| 1 | Water | `water_treatment` | Water treatment panel | Focus granted by `EnsureInitialFocus`; **Tab/arrows** move focus, **Enter/Space** activate, **Esc** closes |
| 2 | Power | `power_grid` | Power grid panel | breaker buttons are standard `Button`s (focusable, FX on focus/hover/press) |
| 3 | Food | `inventory` | Inventory panel | grid rows are keyboard rows (`AshfallDataGrid`), **Enter** uses the ration |
| 4 | Duty | `duty_roster` | Duty roster panel | assign buttons focusable; **Enter/Space** assigns |
| 5 | Dose | `dose_ledger` | Dose ledger / radiation detail | panel opens from a focusable rail/dashboard button |
| 6 | Research | `research` | Research atlas | node buttons focusable; **Enter/Space** starts a node |
| 7 | Expedition | `expeditions` | Expedition board | dispatch button focusable; **Enter/Space** dispatches |

Universal behaviour on every overlay surface:

* `UiMotion.EnsureInitialFocus(panel)` grants initial focus on open (deferred, guarded, no-op in
  headless/suppressed runs) so the first **Tab** lands inside the panel, not on the rail behind it.
* Arrow keys traverse siblings (`AshfallFocusPolicy` explicit neighbours where a panel defines
  them); **Esc** closes through `CloseAllOverlayPanels()` with the animated close path.
* Buttons carry focus feedback (`UiMotion.AttachButtonFx` → ×1.015 focus lift) so keyboard driving
  is visible, not silent.

## Automated evidence (2026-09-25)

| Gate | Command | Result |
|---|---|---|
| Clickability | `godot --headless --path . -- --player-panels-uitest` | `[UiClickability] panels audited=169 skipped=0 buttons=559 inert=0` |
| Focusability | same run | `[UiFocusability] 561 interactive / 0 unreachable / 0 panelsWithNoFocus` |
| Truthfulness | same run | `[UiTruthfulness] blankUnboundPanels=0` |
| Accessibility incl. focus/contrast | `godot --headless --path . -- --ui-accessibility-selftest` | **PASS** (5/5) |
| Motion preference gate | `ReducedMotion` selftest check in the same suite | `ReducedMotion disables AccessibilityPresentation.MotionAllowed` PASS |

## Motion / comfort defaults (first run)

`Assets/Ashfall.Core/Settings/UserSettingsData.cs` defaults: `WindowMode=1` (1920×1080 borderless),
`UiScale=1.0`, `VSync=true`, `MasterVolume=1.0`, `MuteAll=false`, `HighContrast=false`,
`HazardTextLabels=true`, `LargeFonts=false`, `ColorblindMode=None`, `ConfirmEndDay=true`,
**`ReducedMotion=false`** — animation is on by default and the player can turn it off in Settings;
`UiMotion.CanAnimate` and `AudioManager` both read `AccessibilityPresentation.MotionAllowed`, so the
preference is one authority for both presentation layers.

## Limits (honest)

* The automated gates prove **reachability and focus assignment**, not raw key-event delivery
  through the OS/window layer. A human keyboard-only pass over stages 1–7 on a real display is
  still the acceptance step for alpha (tracked in `docs/builds/EXPORT_REPORT.md` follow-ups).
* Mouse-driven-only flows inside a stage (e.g. drag interactions, if any) are out of scope of this
  document; none of the seven stage surfaces requires one today.
