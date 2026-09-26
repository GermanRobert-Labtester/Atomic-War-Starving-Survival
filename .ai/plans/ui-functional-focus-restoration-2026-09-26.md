# UI FUNCTIONAL REPAIR — FOCUS RESTORATION + THERMAL PANEL NULL GUARD (2026-09-26)

> **STATUS: APPROVED BY USER** — user directive: "Please do a UI audit with
> immediate precision and functionality repair!" then "yes!" to the larger
> functional items.

## Premise correction (Rule 7)

The three HIGH functional findings carried from the 2026-09-05 audit are
**already sealed** in current source; re-verification found:

1. `AshfallDataGrid` rows now set `FocusMode.All` + a visible focus style and
   accept `ui_accept` (`BuildRowContainer`, 2026-09-25 comment) — not mouse-only.
2. `GameDashboardPanel.BuildNavigationRail` now contains the rail in a
   `railScroll` `ScrollContainer` (2026-09-25 comment) — the overflow is fixed.
3. `Main.GameFlow.AnyOverlayPanelOpen` and
   `Main.PanelLifecycle.CloseAllOverlayPanels` now share the single
   `OverlayPanelCatalog()` authority (2026-09-25 comments) — no disagreement.

Real, current gaps found by source inspection instead:

## F-UI-FOCUS-OPENER — overlay close never restored focus (functional)

- `AshfallFocusPolicy.RestoreFocusFromRoot` reads the `_ashfall_focus_opener`
  metadata and is called on every overlay dismissal
  (`Main.PanelLifecycle.CloseAllOverlayPanels`, lines 249/260).
- The **only** writer of that metadata is `AshfallFocusPolicy.OpenWithFocus`,
  which has **0 call sites** (`grep` verified). Therefore the metadata is never
  set and focus restoration is a silent no-op: keyboard/controller users lose
  their place after closing any overlay.
- Repair: record the pre-open focus owner in the host's single open seam
  (`Main.PlayerSurfaces.EnsureInitialFocus`, reached by both
  `RegisterOpenMotionRecursive` and `ShowPanelLifecycle`), using a shared
  `AshfallFocusPolicy.FocusOpenerMeta` constant so writer/reader cannot drift.
- Note: `src/Main.PlayerSurfaces.cs` overlaps the ACTIVE PFGL-octet claim; this
  is a 2-line additive edit in `EnsureInitialFocus` (line ~1037), far from the
  octet board region, performed under the user's explicit functional-repair
  authorization. Unrelated dirty Plan 142 clothing wiring is preserved.

## F-UI-THERMAL-NULL — `_Ready` dereferences nullable host (crash risk)

- `ShelterThermalPanel._Ready` iterates `_host.System.State.rooms` with no
  guard, while every other method in the file (`RefreshView`, `ValveFor`,
  `RefreshZoneButtons`) guards `_host != null`. This is the live `CS8602`
  warning and a latent `NullReferenceException` if the panel enters the tree
  before `Bind`.
- Repair: guard the zoning loop in `_Ready` with the file's established idiom.

## Files changed

- `src/UI/AshfallFocusPolicy.cs` (add `FocusOpenerMeta` const; use it in
  `OpenWithFocus` / `RestoreFocusFromRoot`)
- `src/Main.PlayerSurfaces.cs` (`EnsureInitialFocus` records the opener)
- `src/UI/ShelterThermalPanel.cs` (`_Ready` null guard)
- `.ai/plans/ui-functional-focus-restoration-2026-09-26.md` (this plan)

## Non-goals

No new focus/overlay authority; no change to `ModalManager` (it has its own
working restore), no grid/nav-rail rework, no gameplay/save/data changes.

## Verification

- `Ashfall.csproj` build → **0 errors**; touched-file warning `CS8602` cleared
  (4 → 3 warnings, remaining are pre-existing CS0162 in others' files).
- Headless `godot --headless -- --ui-accessibility-selftest` → **5/5 PASS**.
- Headless `godot --headless -- --player-panels-uitest` → **21/21 PASS**.
- `AccessibilitySourceAuditTests` → **6/6** (new writer-side gate).
- `ThemeSemanticTokensTests` → **5/5**.

## Additional context

`WORKTREE_OWNERSHIP.md` is foreman-only, so no claim row was added; the
user-authorized scope and the PFGL-claim overlap on `Main.PlayerSurfaces.cs`
are recorded in `.ai/state.md`.
