# UI AUDIT — PRECISION & FUNCTIONALITY REPAIR (2026-09-26)

> **STATUS: APPROVED BY USER** — user directive: "Please do a UI audit with
> immediate precision and functionality repair!"

## Bounded outcome

Audit the ASHFALL UI for accessibility/functional defects and repair the
highest-confidence, bounded finding without racing active claims.

## Finding (repaired)

**F-UI-CONTRAST-CRITICAL — `Theme.Critical` failed WCAG AA body-text contrast.**

- Evidence: `docs/ui/ACCESSIBILITY_REPORT.md` (2026-09-05) already recorded
  Critical `#E63333` failing: 4.41:1 on Surface, 4.12:1 on SurfaceCard,
  3.64:1 on SelectedBg (body-text floor 4.5:1). The `Dim` token was sealed on
  2026-09-25; `Critical` was left open.
- Impact: the token is consumed as a text `font_color` in dozens of panels
  (e.g. `AshfallUiHelpers.MakeCritical`, `GameHudOverlay`, `PharmaLabPanel`,
  `ShelterBarterPanel`, `TradeScreenGodotPanel`, `DoseRegisterSurface`).
- Repair: `Assets/Ashfall.Core/UI/Theme.cs` — `CriticalHex`/`Critical` moved
  from `#E63333` (0.902,0.200,0.200) to `#FF5252` (1.000,0.322,0.322).
  Measured ratios: Ink 6.18, Surface 5.93, SurfaceCard 5.54, HoverBg 5.02,
  SelectedBg 4.89 — all ≥ 4.5:1. Hue preserved (~0°), distinct from Warm/Hot.
- Ratchet: `Ashfall.Core.Tests/UI/ThemeSemanticTokensTests.cs` gains
  `CriticalTextColor_MeetsWcagAaOnEveryConsumedSurface`, which recomputes the
  WCAG ratio on all five consumed surfaces and pins hex↔tuple agreement.

## Files changed

- `Assets/Ashfall.Core/UI/Theme.cs`
- `Ashfall.Core.Tests/UI/ThemeSemanticTokensTests.cs`
- `Ashfall.Core.Tests/Settings/ColorblindColorMapperTests.cs` (Critical pin moved to the sealed value; mapper contract unchanged)
- `.ai/plans/ui-audit-precision-repair-2026-09-26.md` (this plan)

## Non-goals / remaining ranked findings (not touched)

The 2026-09-05 audit's HIGH functional findings remain open and are larger
shared-seam changes requiring their own package/claim:

1. `AshfallDataGrid` rows are mouse-only (no keyboard/controller selection).
2. Dashboard navigation rail can exceed the 1920×1080 canvas without scroll.
3. Overlay detection lists disagree between `Main.GameFlow.AnyOverlayPanelOpen`
   and `Main.PanelLifecycle.CloseAllOverlayPanels`, making Escape unpredictable.
4. Hex/tuple drift in Pale/Surface/SurfaceCard/Warning constants.
5. Label floor 11px (`Theme.FontSizeLabel`) on dense metadata.

## Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/ThemeSemanticTokensTests.cs` → **5/5 passed**
  (includes the new `CriticalTextColor_MeetsWcagAaOnEveryConsumedSurface`).
- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs` → **5/5 passed**.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Settings/ColorblindColorMapperTests.cs` → **14/14 passed**.
- Host build is currently red from a concurrent untracked Plan 217 package
  (`src/Host/HostCli.Genealogy.cs`), which is outside this claim and was not
  touched.
