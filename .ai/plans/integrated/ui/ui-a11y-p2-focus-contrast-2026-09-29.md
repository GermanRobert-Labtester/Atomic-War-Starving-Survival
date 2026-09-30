# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y P2 FOCUS RESTORE + CONTRAST HYGIENE — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue with more UI work!" (2026-09-29),
continuing the audit-fix series. Implements P1 item 4 and P2 items 8 and 11
from `docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md`. Follows the
`claim-ui-a11y-p1-input-correctness-2026-09-29` convention.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no fix and no acceptance criterion.
> **MUST NOT** below remains binding.

---

## 0. Framing — Focus Is a Promise

> *"A focus ring that lands in the wrong place is not a styling bug. It is the game losing track of
> where you are standing."*

Keyboard and controller players do not have a mouse cursor. They have **focus**, and focus is the
only thing telling them where they are in a screen full of controls. When panel B opens over panel
A and closes, and focus is restored to a control *inside the panel that just closed*, the player is
left pointing at nothing — pressing a key into a place that no longer exists.

This package fixes that with one restore attempt after the close loop, taken from the topmost
closed panel's recorded opener, with a deterministic fallback to the first focusable control of the
exposed dashboard. Deterministic is the operative word: when the honest answer is unavailable, the
plan picks a *predictable* one rather than a lucky one.

**Tone & register.** Optical, exact, quietly compassionate. The vocabulary is vision and position:
*contrast, ratio, token, focus, opener, restore, deferred*. Prose should read like a lighting
technician who has learned that some of the audience cannot see the difference between two greys.

**The interesting number.** The deselected route icon measured **≈1.9:1** against a 3:1 minimum.
Raised to alpha 0.4 it measures ≈3.8:1 — and selection still reads as dimmer. Two small floats, and
the difference between a control that exists and a control that is a rumour.

**The second layer.** Focus is the cursor that keyboard and controller players carry in their
heads, and losing it is not an inconvenience — it is being told *you are nowhere*. The restore
rule is the plan's compassion made mechanical: return the player to where they were standing, and
when that place no longer exists, choose *predictably* rather than luckily. And 1.9:1 is the
evidence that invisibility is measurable: a control below its contrast line is not hard to see. It
is, for part of its audience, not there at all.

**Texture (second prose pass — commentary only).**

- "Restored to a control inside the panel that just closed" — pointing at nothing, pressing keys into a place that no longer exists.
- "When the honest answer is unavailable, the plan picks a predictable one." Determinism as kindness.
- "Some of the audience cannot see the difference between two greys." The line that justifies every number in the file.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, token, acceptance criterion or
register row changes. The register below is unchanged.)*

- 1.9:1 is not *hard to see*; below its line a control is, for part of the audience, a rumour.
  Measurability is what makes invisibility arguable — and then fixable.
- The deterministic fallback is compassion with its feelings sorted: when the honest answer is
  gone, the plan chooses predictably rather than luckily, and a player can learn a predictable
  mistake.
- Focus is where the player is standing. The restore rule is a promise to never lose the address,
  and to say so plainly when the address no longer exists.

> "Some of the audience cannot see the difference between two greys. Every number in this file is
> written for them."

---

## Bounded outcome

1. **Stacked-panel focus restore (P1.4).** Today `CloseAllOverlayPanels`
   calls `RestoreFocusFromRoot` per panel inside the close loop: with panel B
   opened over panel A, B's opener lives inside A, panels close in catalog
   order, and the synchronous `opener.Visible` check fails (or multiple
   competing deferred `GrabFocus` calls race). Fix:
   - One restore attempt after the loop, taken from the topmost closed
     panel's recorded opener, via a new deferred check
     (`AshfallFocusPolicy.RestoreFocusDeferred`, `IsVisibleInTree`).
   - If that opener lives inside any panel that was just closed (stacked
     case), it cannot receive focus — deterministic fallback: focus the
     first focusable of the exposed dashboard
     (`AshfallFocusPolicy.FocusFirstDeferred`).
   - Journal close keeps its existing `RestoreFocusFromRoot` (opener is on
     the always-present dashboard surface; works today).
   - New `AshfallFocusPolicy` members: `GetRecordedOpener`,
     `RestoreFocusDeferred`, `FocusFirstDeferred`, `IsInsideAny`.
     Existing `RestoreFocus`/`RestoreFocusFromRoot` semantics unchanged
     (other callers: dead-code `ModalManager`, journal path).

2. **Deselected route icon contrast (P2.11).** `GameDashboardPanel.cs:327`
   `Modulate (1,1,1,0.22)` measures ≈1.9:1 — below the 3:1 UI-component
   minimum. Raise alpha to 0.4 (≈3.8:1 on opaque Ink); selection still reads
   as dimmer than the full-strength selected icon.

3. **Alarm/status accents onto tokens (P2.8).** Replace hand-rolled
   primaries with canonical tokens (all pass AA on Ink; removes drift):
   - `Colors.Red` → `DesignTheme.Critical` (6.19:1) and `Colors.White` →
     `DesignTheme.Pale` in GeigerCalibrationPanel (3 sites), SafeCrackModal
     (1), BrineExtractionPanel (3).
   - TriangulationPanel: `Colors.Green` → `Success`, `Colors.Yellow` →
     `Warning`, `Colors.White` → `Pale` (semantic mapping: CONFIRMED /
     Pending / no-data). Pure primaries also broke the restrained-palette
     tone rule.

## Exact files

- `src/UI/AshfallFocusPolicy.cs` — 4 additive static members
- `src/Main.PanelLifecycle.cs` — CloseAllOverlayPanels restore rework
- `src/UI/GameDashboardPanel.cs` — 1 alpha value (file sits in the stale,
  already-shipped PFGL Phase A row, same situation as PanelLifecycle last
  package; change is one literal)
- `src/UI/GeigerCalibrationPanel.cs`, `src/UI/SafeCrackModal.cs`,
  `src/UI/BrineExtractionPanel.cs`, `src/UI/TriangulationPanel.cs` — token
  swaps + `using DesignTheme = Ashfall.Core.UI.Theme;` alias (files had no
  token references before)
- `Ashfall.Core.Tests/UI/UiA11yP2FocusContrastGateTests.cs` — new static gate
- Governance: this plan, `WORKTREE_OWNERSHIP.md` (claim row), `.ai/state.md`

## MUST NOT

- No Core changes; no new color literals beyond the two token swaps' alias.
- No ModalStackController/ModalManager wiring (P1.3 is a separate package).
- No panel-resize/bottom-up cleanup (P2.10 stays out of scope).

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. New gate tests + adjacent UI gates via `bin/run-scoped-tests`.
3. `godot --headless --path . --quit-after 2` boot check.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **MUST NOT** list and its recorded exclusions. Not defects — deliberate
non-changes. Any later pass that raises one must re-measure the contrast and re-check the callers.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately verified) |
|---|---|---|---|
| UI2-OM-1 | `ModalManager` / `ModalStackController` wiring (P1.3). | Explicitly out of scope; deferred with P3 to a separate governance decision. | Governance, not a builder. |
| UI2-OM-2 | Panel resize / bottom-up cleanup (P2.10). | Stays out of this package; later swept into the target-sizes package. | The target-sizes package (shipped). |
| UI2-OM-3 | `ModalManager` is dead code. | Recorded as such and left alone. Whether dead code is removed is not this plan's judgement. | A reachability case, made per member. |
| UI2-OM-4 | Were the hand-rolled primaries chosen deliberately? | Replaced with canonical tokens; the pure primaries **also broke the restrained-palette tone rule**. The original intent is not recorded. | Never — texture by omission. |
| UI2-OM-5 | What other colors are below 3:1? | The plan fixes the sites the audit enumerated (§9.11, §9.8). Absence of a listed site is not proof of compliance elsewhere. | A full contrast sweep with a stated inventory. |
| UI2-OM-6 | Why is `GameDashboardPanel.cs` in a stale, already-shipped ownership row? | Same situation as `PanelLifecycle` last package: the row is stale, the file shipped and was edited later. Change is one literal. | `WORKTREE_OWNERSHIP.md`'s owner. |
