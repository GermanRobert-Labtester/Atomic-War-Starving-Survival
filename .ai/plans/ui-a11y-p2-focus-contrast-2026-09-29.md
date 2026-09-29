# UI A11Y P2 FOCUS RESTORE + CONTRAST HYGIENE — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue with more UI work!" (2026-09-29),
continuing the audit-fix series. Implements P1 item 4 and P2 items 8 and 11
from `docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md`. Follows the
`claim-ui-a11y-p1-input-correctness-2026-09-29` convention.

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
