# UI A11Y P3 NAV SCOPE + TAB TRAP + OVERFLOW PRECISION — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue with more UI work especially UI
precision and correction as well as UI functionality!" (2026-09-29). Third
package in the audit-fix series: closes the last open P1 (§9.3, functionality)
and the grid/shell overflow corrections (§6/§9.9, precision).

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
