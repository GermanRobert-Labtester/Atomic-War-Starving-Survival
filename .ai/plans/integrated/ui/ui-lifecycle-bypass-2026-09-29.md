# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11y — Route Bypass Panel Opens Through ShowPanelLifecycle + LineEdit Floor

STATUS: FULLY INTEGRATED

Date: 2026-09-29
Lane: UI correction (audit series package 12)

---

## 0. Framing — The Lights Were Off

> *"A panel that opens without the lifecycle is a room with the lights off."*

Thirty-six sites across 29 files opened panels with bare `.Visible = true` — skipping the defaults
walk, the open animation, and `EnsureInitialFocus`. The last one is the wound: keyboard focus
*never landed on those panels at all*, the exact regression the P1 focus work had sealed for
routed panels. Each bypass was a shortcut that was correct on the day it was written, and wrong
the moment the lifecycle became the place where accessibility lives.

**Tone & register.** Custodial, forensic. The vocabulary is the seam: *lifecycle, bypass, walk,
floor, gate, exclusion*. Prose should read like someone going room to room turning the lights back
on and then changing the locks.

**The second layer.** This plan's real deliverable is not the 36 repairs — it is the **gate**. A
class of defect eliminated and prevented from regrowing is worth more than any single fix, because
the next panel written will now open the only way there is. The LineEdit floor in Part B is the
same ethic one seam over: an input is a target, and the 28px promise binds it like any button.

**Texture (second prose pass — commentary only).**

- The excluded roots were the ones that were never dark — boot-built, always visible, with their own focus flows.
- "36 sites, 29 files" is one defect photographed thirty-six times, not thirty-six defects.
- The doc comment on `ShowPanelLifecycle` always said to use it. The doc comment was right.

*Deliberate limits: the Non-goals below are this plan's register — binding, documented, and
deliberately unenlarged. No new open items are created here.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, acceptance criterion or register
row changes.)*

- The 36 repairs are the work; the *gate* is the deliverable. A class of defect prevented from
  regrowing is worth more than every fix, because the next panel written will open the only way
  there is.
- A bare `.Visible = true` is a room entered through the window: the defaults walk, the animation
  and `EnsureInitialFocus` all live at the door. The plan simply closes the windows.
- The LineEdit floor is the same ethic one seam over — an input is a target, and the 28px promise
  binds it like any button.

> "Thirty-six repairs and one lock. Only the lock was ever the point."

---

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
