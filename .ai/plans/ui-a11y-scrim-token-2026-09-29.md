# UI A11Y §2d PANEL-SCRIM TOKEN CONSOLIDATION — 2026-09-29

STATUS: APPROVED BY USER
Authorized: user session request "Continue doing more UI correction and UI
precision work!" (2026-09-29). Seventh package in the audit-fix series:
audit §2d, "the single largest palette-hygiene win in the tree."

## Bounded outcome

The ~58 hand-rolled near-grey panel scrims (`new Color(0.02–0.07, …, 0.85–0.96)`)
that re-derive the panel background by hand are replaced with one authority:

1. **Core token:** `Theme.InkPanelStrong = (0.035f, 0.043f, 0.047f, 0.92f)` —
   same hue as `Ink`/`InkPanel`, alpha 0.92 = the observed modal/dense-panel
   tier in the drifted literals (16+8+5+… sites cluster at 0.88–0.96; 0.92 is
   the mode). Documented as the dense-panel/modal scrim tier; `InkPanel`
   (0.86) remains the lighter panel tier.
2. **Host accessor:** `AshfallUiHelpers.PanelScrim()` returns
   `ToColor(DesignTheme.InkPanelStrong)` — same namespace as every src/UI
   panel, so no using changes are needed.
3. **Sweep:** all 57 assignment sites matching
   `new Color(0.0[2-7]f, 0.0[2-7]f, 0.0[2-7]f, 0.(8[5-9]|9[0-6])f)` in
   `src/UI/*.cs` become `AshfallUiHelpers.PanelScrim()` (or the equivalent
   inline where the literal feeds a stylebox/ColorRect background).

**Deliberately excluded (semantic or separate concern, kept as-is):**
- Tinted scrims that the near-grey regex does not match: EmergencyResponseHud
  crisis red, ExpeditionPanel amber banner, BlackProjectsArchivePanel red card.
- Sub-0.85 stack-dependent scrims: MapDetailPanel (0.74 — §3 follow-up),
  MainMenuPanel (0.55) and GameOverPanel (0.80) carousel overlays,
  UiBackgroundCarousel.
- Token-derived raw-alpha composites (§2c lower tier): sidebar rows, meters,
  DataGrid state tints — those derive from named tokens already.

Visual effect: grey-channel jitter (0.02–0.07) and alpha jitter (0.85–0.96)
normalize to one value; all panels sit over opaque Ink so Pale-text contrast
(≈14:1) is unaffected; per-panel look changes are imperceptible except the
0.85/0.88 tier becomes 0.07 more opaque (slightly better text isolation).

## Exact files

- `Assets/Ashfall.Core/UI/Theme.cs` (1 token + doc), `src/UI/AshfallUiHelpers.cs`
  (1 accessor), ~58 src/UI panel files (mechanical literal replacement),
  `Ashfall.Core.Tests/UI/UiA11yScrimTokenGateTests.cs` (new gate)
- Governance: this plan, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`

## MUST NOT

- Touch tinted/sub-0.85/carousel overlays, Core beyond the one token, any
  layout or sizing; no new usings (same-namespace accessor).

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Gate test: regex finds zero remaining near-grey scrim literals in
   src/UI outside the exclusion allowlist.
3. `--ui-layout-selftest` + `--player-panels-uitest` headless.
