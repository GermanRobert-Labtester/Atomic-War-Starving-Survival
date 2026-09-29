# UI A11y — Theme Coverage for Remaining Control Types + Snapshot Parity

STATUS: APPROVED BY USER

Date: 2026-09-29
Lane: UI precision/correction (audit series package 11; follows the
class-defaults theme introduced by the 2026-09-25 UI/UX audit and extended by
the 2026-09-29 a11y series)

## Outcome

`AshfallUiTheme` installs ASHFALL class defaults for Button, LineEdit,
TextEdit, ItemList, PopupMenu, Slider, and ScrollBar. Text of the remaining
control types inherits those entries through Godot's class-chain fallback
(OptionButton→Button, SpinBox→LineEdit), but their **type-specific styleboxes
and icons** still resolve to Godot's light default theme — blue fills, default
check art, default arrows — glaring on ink panels. ~200 direct constructions
are affected: OptionButton (50), ProgressBar (16), SpinBox (13),
RichTextLabel (6), TabContainer (2), plus CheckBox/CheckButton (1).

**Part A — theme coverage extensions in `src/UI/AshfallUiTheme.Build()`:**
- `ProgressBar`: `background` = ink flat + Line border, `fill` = Warm flat;
  `font_color` Pale, `font_background_color` Muted, font size body.
- `CheckBox` / `CheckButton`: flat `checked`/`unchecked` icons (Hot fill +
  Ink border vs Ink fill + Line border) — replaces Godot default check art.
- `OptionButton`: flat `arrow` icon (Hot) — replaces default arrow art.
- `SpinBox`: flat `updown` icon (Warm fill, Line border).
- `TabContainer` / `TabBar`: `panel` ink flat, `tab_selected` =
  Warm/Hot pressed-style flat, `tab_unselected` = ink flat + Line border;
  `font_selected_color` Pale, `font_unselected_color` Muted, body size,
  Barlow SemiBold font.
- `RichTextLabel`: `default_color` Pale, `normal_font` Barlow Regular,
  `bold_font` Barlow SemiBold, `mono_font` Share Tech Mono, body sizes.

All values derive from the existing Core tokens; no new tokens. Per-node
overrides in panels still win (theme only fills gaps).

**Part B — snapshot parity in `src/UI/SnapshotOrchestrator.cs`:**
The snapshot host already installs the theme (`InstallOn`, line ~196) but not
the control-defaults walk from pkg 10 — captures could diverge from live UI
(min-size floors / label clipping). Add
`AshfallUiTheme.EnforceControlDefaults(root)` beside it.

## Non-goals

- No layout redesign; no per-site edits; no new tokens.
- ScrollContainer (137 sites) untouched: its default panel is empty already.
- No visual-lane work (snapshot golden regen remains the visual lane's job).

## Verification

1. Host build 0 errors.
2. New static gate `Ashfall.Core.Tests/UI/UiThemeCoverageGateTests.cs`:
   theme Build() covers each new control type (key item strings present),
   snapshot orchestrator runs the defaults walk.
3. Headless probes: `--ui-layout-selftest`, `--player-panels-uitest`, boot.
4. Governance: claim row, `.ai/state.md`, pathspec commit, memory.
