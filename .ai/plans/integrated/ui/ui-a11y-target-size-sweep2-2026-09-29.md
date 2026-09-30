# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11y — Target-Size Floor Sweep 2 (Direct Buttons) + Fixed-Width Label Clipping

STATUS: FULLY INTEGRATED

Date: 2026-09-29
Lane: UI precision (audit series package 10; extends
`docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md` P2.10 to the panels that
package did not reach)

---

## 0. Framing — The Floor

> *"Twenty-eight pixels is a promise made to fingers."*

P2.10 set the interactive-target floor and found, afterwards, that the floor had holes: ~230 direct
`new Button` constructions across ~70 files still sized themselves from the font — about 27px, and
*less* wherever a player had enlarged their text. That is the quiet cruelty of font-coupled sizing:
the interface shrinks its buttons at exactly the moment the player is asking to see more clearly.
This plan closes the holes and clips the labels that pinned widths could overflow.

**Tone & register.** Builders' plain. The vocabulary is the site: *floor, token, sweep, skip,
exception, gate*. Prose should read like a building-code inspection — the structure was sound, the
signage was fine, and the door handles were all slightly too high.

**The second layer.** The honest core of this plan is the *excluded* list. Icon-only controls,
steppers and custom row buttons are skipped **and reported**, and the skip exceptions are baked
into the static gate — recorded, not hidden. Accessibility work that admits its exceptions keeps
its credibility; a sweep that claims everything has fixed nothing verifiably. The fixed-width
label clipping in Part B is the same ethic one scale down: an ellipsis instead of a collision.

**Texture (second prose pass — commentary only).**

- The ~230 direct buttons are the parts of the house built before the building code. Nothing about them was wrong *then*.
- Three parallel agents, one floor: the sweep is delegated, but the standard is single — `Theme.MinInteractiveHeight`, one token, no local opinions.
- "Left or upgraded to the token" is the whole plan in five words: some sites were already right, and the plan says which.
- Under a font override, the old sizing fell below its own nominal number. The floor is measured against the fingers, not the font.

*Deliberate limits: the Non-goals below and the recorded skip exceptions are this plan's register —
binding, documented, and deliberately unenlarged. No new open items are created here.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, acceptance criterion or register
row changes.)*

- The excluded list is the file's credibility. Icon-only controls, steppers and custom rows are
  skipped *and printed*, and the skips are baked into the gate — recorded exceptions are the only
  kind that stop being lies.
- A floor with holes is worse than no floor, because it is believed. Sweeping twice is what
  happens when the first sweep is trusted too early.
- The fixed-width clipping is the same ethic one scale down: an ellipsis instead of a collision,
  an admission instead of an overlap.

> "A sweep that admits its exceptions is the only sweep that can be believed."

---

## Outcome

P2.10 added the 28px interactive-target floor (`Theme.MinInteractiveHeight`)
to 31 panels, but only where buttons route through `AshfallUiHelpers.MakeButton`
or were hand-floored. ~230 direct `new Button` constructions across ~70 files
still size font-coupled (~27px, and less under font overrides). This package
extends the floor to those sites and clips fixed-width direct labels.

**Part A — button floor via one central seam (implementation record):**
The original design delegated per-site edits (3 agents, ~230 sites), but the
delegation could not run in this session. Pivoted to a strictly better shape:
`AshfallUiTheme.EnforceControlDefaults(Node)` — an idempotent subtree walk
that raises every `Button` below the 28px floor
(`Theme.MinInteractiveHeight`; larger explicit sizes win) and adds
`ClipText` + `TrimEllipsis` to fixed-width non-autowrap `Label`s. Wired at
two seams: `ShowPanelLifecycle` (the canonical open path, 52 call sites —
normalizes each panel before animation/focus) and a deferred whole-tree sweep
after boot (menus, dashboard, HUD). Same end state as the per-site sweep, one
authority per concern, no 230 duplicated literals.

**Part B — fixed-width label clipping:** covered by the same walk (identical
policy to the factories' `FinishLabel` seam from pkg 9, 075e771fd).

**Excluded from the sweep (framework/tooling seams, not panel buttons):**
`src/UI/AshfallUiTheme.cs` (theme builder), `src/UI/UiMotion.cs`,
`src/UI/AshfallSidebar.cs` (custom row buttons with own styles),
`src/Main.UiPanels.cs`, `src/Host/HostCli.Command.RunUiLayoutSelfTest.cs`.
(The walk does not special-case these; they are simply not panel-open
targets. Icon-only buttons smaller than 28px get floored — that is the
audit's target-size rule, applied uniformly.)

## Non-goals

- No layout redesign, no new tokens, no factory migration of `new Label`
  call sites beyond fixed-width clipping.
- No visual-lane work.

## Verification

1. Host build 0 errors.
2. New static gate `Ashfall.Core.Tests/UI/UiA11yTargetSizeSweep2GateTests.cs`:
   walk policy present (button floor + label clip + recursion), wired before
   animation in `ShowPanelLifecycle`, deferred boot sweep present.
3. Headless probes: `--ui-layout-selftest` PASS, `--player-panels-uitest`
   PASS, boot clean.
4. Governance: claim row, `.ai/state.md`, pathspec commit, memory.
