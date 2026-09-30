# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11y — Theme Coverage for Remaining Control Types + Snapshot Parity

STATUS: FULLY INTEGRATED

Date: 2026-09-29
Lane: UI precision/correction (audit series package 11; follows the
class-defaults theme introduced by the 2026-09-25 UI/UX audit and extended by
the 2026-09-29 a11y series)

---

## 0. Framing — A Daytime Theme, Trespassing

> *"Godot's default theme is a daytime theme. This game is played at night."*

The class-chain fallback solved *text* — OptionButton inherits Button's typeface quietly enough —
but it never solved *art*. Blue fills, default check marks, default arrows: the engine's own
handwriting showing through the game's, glaring on ink panels like a lamp left on in a dark room.
About 200 direct constructions carry the trespass.

**Tone & register.** Housekeeping with a conscience. The vocabulary is the theme: *class default,
fallback, stylebox, icon, gap, override*. Prose should read like someone going room to room turning
off the wrong lights — not redesigning the house, just letting it be the colour it decided to be.

**The second layer.** Theme coverage is translation work: every control left on engine defaults is
a small untranslated sentence inside an otherwise deliberate language. The plan fills *gaps*, and
is careful to say per-node overrides still win — the theme is a floor, not a law. And Part B is the
plan's honesty: snapshots that capture without the defaults walk are *photographs that flatter*,
and the golden must not be prettier than the game.

**Texture (second prose pass — commentary only).**

- "Glaring on ink panels" — a daytime theme trespassing at night. The whole defect in four words.
- ScrollContainer's panel is empty already — the one control the engine got right by doing nothing.
- Flat icons, existing tokens, no new tokens: the vocabulary is already sufficient; the plan only teaches it to the stragglers.

*Deliberate limits: the Non-goals below are this plan's register — binding, documented, and
deliberately unenlarged. No new open items are created here.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, acceptance criterion or register
row changes.)*

- Theme coverage is translation work: every control left on engine defaults is a small untranslated
  sentence inside an otherwise deliberate language — the engine's handwriting showing through the
  game's.
- The theme is a floor, not a law: per-node overrides still win, and the plan is careful to say so.
  A design system that cannot be overruled is a different kind of trespass.
- The snapshot rule is the plan's honesty test: a golden taken without the defaults walk is a
  photograph that flatters.

> "The golden must not be prettier than the game."

---

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
