# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# UI A11Y P3 NAV SCOPE + TAB TRAP + OVERFLOW PRECISION — 2026-09-29

STATUS: FULLY INTEGRATED
Authorized: user session request "Continue with more UI work especially UI
precision and correction as well as UI functionality!" (2026-09-29). Third
package in the audit-fix series: closes the last open P1 (§9.3, functionality)
and the grid/shell overflow corrections (§6/§9.9, precision).

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no fix and no acceptance criterion.
> **MUST NOT** below remains binding.

---

## 0. Framing — The Cut Name

> *"The label that runs into the next column is a word somebody wrote, cut in half by a grid that
> did not know it was coming."*

Three defects, and the third is the quietly heartbreaking one: grid headers, grid cells and the
dashboard shell title render at **full text width with no clip and no ellipsis**, so a long
localized name bleeds into the neighbouring column or into the close button. The fix is
`ClipText = true` and `TextOverrunBehavior.TrimEllipsis` — three label sites.

An ellipsis is an admission. It says: *there was more here than we could show, and we are telling
you so rather than pretending the name ended.* That is why it matters that the plan fixes the
bleed and not by truncating the source string.

**Tone & register.** Precise, spatial, faintly architectural. The vocabulary is the grid: *scope
root, topmost, trap, wander, clip, overrun, ellipsis*. Prose should read like a sign-maker who
knows that a sign which overlaps its neighbour is worse than a sign which is slightly too small.

**The interesting restraint.** The Tab trap is delivered with a `Main._Input` override rather than
by wiring `ModalManager`/`ModalStackController` — because wiring them would introduce **a second
modal authority**. The dead seam is left dead and referred upward. And two panels are *excluded*
from the trap because they self-handle Tab: combat target cycling and briefing skip. Even a trap
has to know who lives in the room.

**The second layer.** An ellipsis is an admission, and this plan is full of the right kind: *there
was more here than we could show, and we are telling you so rather than pretending the name
ended.* The grid defects are small and the third one is the humane one — a localized name cut in
half is somebody's word, damaged by a layout that never expected it. The plan fixes the bleed and
refuses to truncate the source string, and it leaves the dead modal seam dead and referred upward,
because a second modal authority would be a worse defect than the trap it delivers.

**Texture (second prose pass — commentary only).**

- "A sign which overlaps its neighbour is worse than a sign which is slightly too small." The lane's entire aesthetic in one line.
- "Even a trap has to know who lives in the room." Two panels self-handle Tab and are excluded by name — an exception recorded is an exception owned.
- Three label sites. The plan counts its work the way a locksmith counts doors.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, acceptance criterion or register
row changes. The register below is unchanged.)*

- The plan repairs the window, never the word: the source string stays whole and the *layout*
  learns to admit it has limits. Truncating a name to fit a grid is book-burning in miniature.
- The dead modal seam stays dead and is referred upward. A second modal authority would be a
  worse defect than the trap it delivers — restraint with a reason attached.
- Even a trap knows who lives in the room: two self-handling panels are excluded *by name*, and an
  exception recorded is an exception owned.

> "A sign that overlaps its neighbour is worse than a sign that is slightly too small."

---

## Bounded outcome

1. **Arrow/D-pad navigation scoped to the open overlay (P1.3a).** Today
   `AshfallFocusNavigator.HandleNavInput(this, …)` is scoped to the whole Main
   tree, so arrows can wander into dashboard controls behind an open overlay.
   Fix: pass `TopmostVisibleOverlayPanel() ?? (Control)this` as the scope root
   (`src/Main.Application.cs` nav branch; one-line change + new helper).
2. **Modal Tab trap (P1.3b).** With an overlay open, Tab escapes into
   background chrome because the viewport consumes Tab for engine
   focus-next before the unhandled phase, and the trap seam
   (`AshfallFocusPolicy.TrapFocus` via `ModalManager`) is unwired dead code.
   Fix: a `Main._Input` override in `src/Main.PanelLifecycle.cs` that, only
   while an overlay is open, runs `TrapFocus` on the topmost overlay in the
   input phase and marks the event handled. Exclusions: `CombatPanel` and
   `DailyBriefingModal` self-handle Tab (combat target cycling, briefing
   skip) via `ashfall_next_tab` and keep their own handlers.
   Not done (deliberate): wiring `ModalManager`/`ModalStackController` —
   the focused-topmost helper plus `TrapFocus` covers the live trap without
   introducing a second modal authority; the dead seam is left for a
   separate governance decision.
3. **Overflow precision (§6/§9.9).** Grid header labels, grid cell labels,
   and the `AshfallDashboardShell` H2 title render at full text width with no
   clip or ellipsis, so long localized names bleed into the next column or
   into the close button. Fix: `ClipText = true` +
   `TextOverrunBehavior.TrimEllipsis` on all three label sites
   (`src/UI/AshfallDataGrid.cs` MakeHeaderLabel + MakeCellControl,
   `src/UI/AshfallDashboardShell.cs` `_titleLabel`).

## Exact files

- `src/Main.PanelLifecycle.cs` — `TopmostVisibleOverlayPanel()` helper +
  `_Input` Tab trap
- `src/Main.Application.cs` — nav branch scope root (1 line)
- `src/UI/AshfallDataGrid.cs` — 2 label sites
- `src/UI/AshfallDashboardShell.cs` — 1 label site
- `Ashfall.Core.Tests/UI/UiA11yP3NavOverflowGateTests.cs` — new static gate
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- No Core changes; no `ModalManager`/`ModalStackController` wiring.
- No Tab behavior change when no overlay is open (engine default preserved).
- No font-size token changes (P2.7 stays a separate visual package).

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. New gate tests + adjacent UI gates via `bin/run-scoped-tests`.
3. Headless `--player-panels-uitest` (runtime nav/focus surfaces) if the
   probe exists; otherwise `godot --headless --path . --quit-after 2`.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's **MUST NOT** list and its recorded exclusions. Not defects — deliberate
non-changes. Any later pass that raises one must re-check the trap behaviour and the clip sites.

| # | Open item | Why it is deliberately open | Who may resolve it (later, separately verified) |
|---|---|---|---|
| UI3-OM-1 | `ModalManager` / `ModalStackController` wiring. | Deliberately **not** done — it would introduce a second modal authority. The dead trap seam is left for a separate governance decision. | Foreman/governance, not a builder. |
| UI3-OM-2 | Why does the engine eat Tab before the unhandled phase? | Recorded as the reason the trap needed an `_Input` override. The engine's reasoning is not this plan's business. | Never — a rule of the host. |
| UI3-OM-3 | Why do exactly two panels self-handle Tab? | `CombatPanel` (target cycling) and `DailyBriefingModal` (skip) are excluded. Whether others should be is not surveyed. | A survey of `ashfall_next_tab` handlers. |
| UI3-OM-4 | What were the names that got cut? | `TrimEllipsis` hides overflow; the strings that overflowed are never recorded. Localization length is a known unknown. | The i18n lane, when long-name strings are collected. |
| UI3-OM-5 | Font-size token changes (P2.7). | Explicitly a separate visual package — and later shipped as its own card. | The font-size package (shipped). |
| UI3-OM-6 | Does the nav scope fix cover the controller? | Arrow/D-pad share `HandleNavInput`. Runtime nav/focus surfaces are probed only if the probe exists. | A `--player-panels-uitest` run on real hardware. |
