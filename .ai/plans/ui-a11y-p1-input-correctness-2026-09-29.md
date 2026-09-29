# UI A11Y P1 INPUT CORRECTNESS — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "some more UI work!" (2026-09-29), implementing
the P1 fixes from `docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md` (§9 items 1, 2
partial, 6). Ownership: user-authorized integrator implementation, mirroring the
`claim-deep-audit-repair-2026-09-26` convention.

## Bounded outcome

Three input-correctness defects fixed in the Godot host layer; no Core changes,
no data changes, no save-section changes (all three targets are presentation/
input seams only):

1. **Moral-choice modal key collision (1–5):** `OpenMoralChoiceModal` opens over
   an open CombatPanel without the exclusive-open seam, so raw keys 1–5 are
   double-consumed. Fix: call `CloseAllOverlayPanels()` first, matching
   `OpenFireIncidentPanel` and every `OpenPlayerPanel` route
   (`src/Main.GameFlow.cs:366`).
2. **J hotkey unguarded in menu state:** `AshfallInputActions.IsJournal` branch
   in `Main._UnhandledKeyInput` runs `ToggleJournal()` with no
   `_state == GameState.Playing` gate, unlike every sibling hotkey. Fix: gate
   the branch on Playing (menu-state J becomes a no-op, consistent with
   F/H/T/X/E).
3. **Crisis HUD layering:**
   - `EmergencyResponseHud._Input` (pre-GUI phase) closes the HUD even when a
     later-added modal is stacked above it. Fix: demote to
     `_UnhandledKeyInput` with the standard pressed/echo guard, so tree-order
     (later siblings first) decides, and the HUD still wins over the global
     Esc sweep when it is the only visible overlay.
   - Panels opened after `BuildUserInterface` (lazy panels, modals) draw above
     the crisis HUD. Fix: in `CloseAllOverlayPanels`, `MoveToFront()` the HUD
     when visible, so an active crisis alert is never obscured.

## Divergence from the audit's proposed fix (recorded, deliberate)

The audit proposed adding `_crisisHud` to `OverlayPanelCatalog()`. On
revalidation that would *close the HUD on every panel switch* and never
re-open it (open path is gated on `snap.Severity >= Severe && !Visible`,
`src/Main.UiPanels.cs:1424-1429`) — losing an active crisis alert. The
catalog membership is therefore intentionally NOT done; re-raise is used
instead. Also NOT done (scope): wiring the Core-advertised `Shortcut = "1"`
strings (`CrisisPresentationCoordinator.cs:228` etc.) — the HUD has zero
consumers of `Shortcut`; Space already activates the focused ack button via
engine `ui_accept`; adding digit handlers would create a NEW collision with
CombatPanel 1–5 while both are visible. Core field left dormant; follow-up
noted in the audit report.

## Exact files

- `src/Main.UiHandlers.cs` — Fix 1 (1 line)
- `src/Main.Application.cs` — Fix 2 (1 condition)
- `src/UI/EmergencyResponseHud.cs` — Fix 3a (input phase + guard)
- `src/Main.PanelLifecycle.cs` — Fix 3b (re-raise, ~4 lines)
- `Ashfall.Core.Tests/UI/UiA11yP1InputGateTests.cs` — new static source gate
  (pattern: `MainTriadDriftGateTests`), pinning all three fixes against drift.
- Governance: this plan, `WORKTREE_OWNERSHIP.md` (claim row), `.ai/state.md`.

`src/Main.PanelLifecycle.cs` note: listed in the stale 2026-09-25
PFGL-CODEX-LUNA6-OCTET row, but that phase's files shipped
(`src/Main.PfglOctetBoards.cs`, `src/UI/PfglOctetBoardPanels.cs` exist) and the
file has been edited by multiple later integrations; the change is a 4-line
additive block.

## MUST NOT

- No Core changes (CrisisPresentationCoordinator stays untouched).
- No catalog membership change for the crisis HUD (see divergence).
- No renames, formatting, or cleanup beyond the five files above.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Focused: new gate test + adjacent input/UI gates via `bin/run-scoped-tests`.
3. `godot --headless --path . --quit-after 2` boot check (input-path change).
