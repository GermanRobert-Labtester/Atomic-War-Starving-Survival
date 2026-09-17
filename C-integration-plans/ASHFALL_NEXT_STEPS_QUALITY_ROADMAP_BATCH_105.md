# ASHFALL — Quality Roadmap Batch 105

## Theme: Responsive Layout System — Resolution Independence for UI Panels

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM-HIGH |
| **Risk** | Medium — touches layout code across 83 UI panels (independently re-verified: `grep -rl 'class \w*Panel\s*:\s*Control' src/UI/` → 83 files; `find src/UI -maxdepth 1 -name '*Panel.cs'` → 83 files, consistent) |
| **Category** | UI Infrastructure / Accessibility |
| **Blocked by** | None (additive to existing panel system) |
| **Blocks** | Steam Deck support, ultrawide marketing, accessibility compliance |
| **Estimated scope** | ~2 weeks (audit + framework + 5 critical panels + tests) — **underestimated, see Review Notes**: the discovery that `Theme.cs` is shared Core code (not Godot-only), the test-project split correction (Step 6 tests cannot live in `Ashfall.Core.Tests` as drafted), and the AGENTS.md Cross-Tool QA Rule (different-reviewer requirement for ≥2 new coupled variables, which the tier+scale-factor system triggers) all add real time not accounted for in the original estimate. Budget ~3 weeks. |

---

## Problem Statement

There are **83** UI panels (`class *Panel : Control`, verified via `grep -rl "class \w*Panel\s*:\s*Control" src/UI/`), built programmatically in C# targeting 1920x1080. No tiered/breakpoint-based responsive layout system exists. Players on non-target resolutions — 1366x768 (most common laptop), 2560x1440, 3840x2160, ultrawide (2560x1080, 3440x1440), and Steam Deck (1280x800) — may encounter:

- Clipped text and truncated labels
- Overlapping controls on smaller screens
- Wasted whitespace on larger screens
- Font sizes that are too small at high DPI or too large at low DPI
- Panels that overflow their allocated dashboard region

**Correction to original premise:** the codebase does NOT bypass Godot's Container system in favor of absolute positioning. A codebase-wide scan found only **1** file in `src/UI/` assigning `.Position = new Vector2(...)` directly (`UiBackgroundCarousel.cs`, not a panel), while **90** files use `VBoxContainer`/`HBoxContainer`/`GridContainer`/`MarginContainer` and **89** files use `SetAnchorsAndOffsetsPreset`/`AnchorPreset`/`LayoutPreset`. Panels are already built on `AshfallDashboardShell` (a `PanelContainer` wrapping a `VBoxContainer`, `src/UI/AshfallDashboardShell.cs`), `AshfallSidebar`, `AshfallStatusRail`, and `AshfallDataGrid` — a consistent shared shell/component system, not ad-hoc pixel layout.

The real gap is narrower than originally stated: panels use **fixed-pixel `CustomMinimumSize`** values (89 files in `src/UI/` set `CustomMinimumSize = new Vector2(...)` with hardcoded literals) and the shell/components have no notion of viewport tier — so at non-1920x1080 resolutions, Containers will reflow content correctly, but minimum sizes, font sizes, and spacing constants will not scale up or down. This is a "no tiered scaling" problem layered on an already-container-based layout, not a "no layout system" problem.

---

## Architecture Decision

**Approach:** Create an `AshfallResponsiveContainer` (custom Godot Control node in `src/UI/`) that adapts its child layout based on available viewport width. Use a breakpoint system (not per-pixel calculations) to maintain designer intent at each tier. All spacing/sizing values flow through the **existing** `Ashfall.Core.UI.Theme` token class (see corrected Step 2 below), scaled by a resolution-dependent multiplier.

**CORRECTED — a design-token class already exists and this plan's central premise was false.** `Assets/Ashfall.Core/UI/Theme.cs` (`public static class Theme`, namespace `Ashfall.Core.UI`) is a real, engine-agnostic token class with zero `UnityEngine`/`Godot` references — spacing (`SpacingXs`..`SpacingXl`, `HudEdge`), typography (`FontSizeH1`..`FontSizeLabel`), corner radii, colors, and panel sizing (`PanelMinWidthNarrow`, `PanelMaxWidth`, `TradePanelMinWidth`, etc.). It is imported into panels via `using DesignTheme = Ashfall.Core.UI.Theme;` and is already in active use in at least 10+ `src/UI/*.cs` files, including two of the five panels this batch names for migration (`InventoryPanel.cs`, `SurvivorsPanel.cs`) and the shared shell components (`AshfallDataGrid.cs`, `AshfallMetricCard.cs`). "This batch must create `AshfallDesignTokens` from scratch" is factually wrong — do not create a duplicate/parallel token class. See Review Notes for the corrected Step 2 scope.

**Not chosen:**
- Godot's built-in `stretch_mode = canvas_items` alone — does not handle layout reflow (just scales everything uniformly, making text unreadable at extremes).
- Per-panel responsive overrides — does not scale to 85 panels; need a system.
- CSS-like media queries — Godot has no native equivalent; our breakpoint system fulfills this role.

---

## Steps

### Step 1: Audit Current Layout Patterns

**Goal:** Since the bulk of panels already use Containers/anchors (verified: 90/97 files in `src/UI/` use Container nodes, 89/97 use anchor presets, only 1 non-panel file uses absolute `Position`), the audit's real purpose is finding which of the 83 panels have hardcoded `CustomMinimumSize`/font-size literals that won't scale, not finding "absolute positioning" panels — there are effectively none of those to find.

**Implementation:**
- Write a diagnostic script (C# in `src/Tools/`, run via a new `--layout-audit` CLI verb registered through the existing `HostCli.cs` `Has(args, ...)` dispatch pattern — do NOT invent a new CLI framework for this one verb) that scans all `src/UI/*.cs` files for:
  - `CustomMinimumSize = new Vector2(literal, literal)` with hardcoded pixel values (89 files currently match this — confirmed via `grep -rl "CustomMinimumSize\s*=\s*new Vector2" src/UI/`)
  - Direct `Position = new Vector2(x, y)` assignments (confirmed: 1 match, `UiBackgroundCarousel.cs`, not a panel — expect this bucket to be near-empty)
  - Direct `Size = new Vector2(w, h)` assignments (re-verified: 2 matches, not 0 as an earlier draft of this doc claimed — `src/UI/ShelterPanel.cs:240` and `src/UI/SnapshotOrchestrator.cs:133`. Both are `SubViewport.Size = new Vector2I(...)` — a render-target pixel dimension, not a `Control.Size` layout override — so they are correctly out of scope for this audit, but the audit script's regex must be precise enough to exclude `SubViewport`/`Vector2I` render-target sizing rather than assuming the bucket is literally empty)
  - Font size overrides (`AddThemeFontSizeOverride`) with literal pixel values, since no central token/theme constant exists yet for font sizing
  - Use of `AnchorPreset` / `SetAnchorsAndOffsetsPreset` / Container nodes as the (already dominant) good pattern, for contrast in the report
- Categorize each panel into: RESPONSIVE (containers/anchors, no hardcoded min-size), NEEDS-SCALING (containers/anchors present but hardcoded `CustomMinimumSize`/font sizes that won't tier), or ABSOLUTE (direct Position/Size assignment — expected to be ~0 panels based on the pre-scan)
- Produce a report: `docs/layout_audit_results.md` with per-panel classification

**Verification:**
```bash
dotnet build Ashfall.csproj   # Audit tool compiles
godot --headless --path . -- --layout-audit   # Produces report at docs/layout_audit_results.md
```

**Done when:**
- Every one of the 83 panels is classified (RESPONSIVE / NEEDS-SCALING / ABSOLUTE) with the classification count stated explicitly in the report header (e.g. "83 panels: 2 RESPONSIVE, 79 NEEDS-SCALING, 2 ABSOLUTE")
- Report identifies the top 10 files with the most `CustomMinimumSize`/font-size literals (the real migration burden, not "absolute positioning")
- Report quantifies percentages per category and the report file is committed to `docs/`

**Risk & Rollback:** Low risk — read-only static analysis, produces a doc file, touches no runtime code. Rollback: delete the report and the audit tool/verb; no game behavior is affected either way.

---

### Step 2: Define Responsive Breakpoints and Scale Tiers

**Goal:** Establish resolution breakpoints, scale factors, and layout rules that all panels will follow.

**Implementation:**
- Create `src/UI/Layout/ResponsiveBreakpoints.cs`:
  ```csharp
  namespace AtomicWar.GodotApp.UI.Layout;

  public enum LayoutTier
  {
      Compact,    // 1280x720 to 1599x899 (Steam Deck, old laptops)
      Standard,   // 1600x900 to 2159x1215 (target 1920x1080)
      Expanded,   // 2160x1216 to 3839x2159 (1440p, ultrawide)
      Large       // 3840x2160+ (4K)
  }

  public static class ResponsiveBreakpoints
  {
      public static LayoutTier GetTier(Vector2I viewportSize) { ... }
      public static float GetScaleFactor(LayoutTier tier) { ... }
      public static int GetColumnCount(LayoutTier tier, int baseColumns) { ... }
  }
  ```
- Define scale factors:
  - Compact: 0.75x (smaller fonts, tighter spacing, stacked layouts)
  - Standard: 1.0x (baseline — current design)
  - Expanded: 1.15x (more breathing room, side-by-side where appropriate)
  - Large: 1.5x (4K readable without squinting)
- Define layout rules per tier:
  - Compact: single-column stacking, abbreviated labels, collapsible sections
  - Standard: designed two-column layout
  - Expanded: three-column where applicable, show more data inline
  - Large: same as Expanded but with increased spacing/font
- **DO NOT create `AshfallDesignTokens.cs`.** The token class already exists at `Assets/Ashfall.Core/UI/Theme.cs` (`Ashfall.Core.UI.Theme`, aliased in Godot host files as `DesignTheme`) and is already referenced by `AshfallDashboardShell`, `AshfallSidebar`(indirectly via `AshfallUiHelpers`), `AshfallDataGrid`, `AshfallMetricCard`, and multiple panels. The correct scope for this step is: (1) add a **new, separate** tier-scaling layer — e.g. `src/UI/Layout/ResponsiveScale.cs` — that reads `Theme.SpacingSm` etc. as the Standard-tier baseline and exposes `Scaled(int baseValue, LayoutTier tier)`, rather than replacing or duplicating `Theme`; (2) because `Theme.cs` lives in `Assets/Ashfall.Core/` it is shared cross-host truth per AGENTS.md — any edit to it is a Core change subject to Invariant 1 (no engine coupling) and must not be conflated with the Godot-only responsive-container work. Treat "extend Theme with tier-lookup helpers" (Core, engine-agnostic) as a clearly separate sub-task/commit from "build the Godot Control that consumes them" (`src/UI/Layout/`), since the AGENTS.md Cross-Tool QA Rule requires a different reviewer for any system introducing ≥2 new coupled variables, and this step introduces both a tier enum and a scale-factor table.

**Verification:**
```bash
dotnet build Ashfall.csproj   # New files compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Responsive"
```

**Done when:**
- `ResponsiveBreakpoints` returns correct tier for all target resolutions, verified by unit tests covering the boundary pixel on each side of every breakpoint (e.g. 1599x899 → Compact, 1600x900 → Standard)
- Scale factors and column counts are defined for all four tiers
- `ResponsiveScale` (new, does NOT duplicate `Theme`) reads `Ashfall.Core.UI.Theme` constants as the Standard-tier baseline (no visual change at 1920x1080) and is referenced by at least `AshfallDashboardShell`, `AshfallSidebar`, and `AshfallStatusRail`
- Confirmed unchanged: `Ashfall.Core.UI.Theme` itself is not forked, renamed, or shadowed by a second token class — a single source of truth for base values remains
- Scaled values can be queried at any tier (`Theme` value * scale factor)

**Risk & Rollback:** Medium risk — this step now touches `Assets/Ashfall.Core/UI/Theme.cs`, which is Core (cross-host shared truth per AGENTS.md Invariant 6/1), not Godot-only code as originally assumed. **Re-verified during this review:** `grep -rl "Ashfall.Core.UI.Theme\|DesignTheme" Assets/_Game/` returns zero matches — the Unity legacy tree has no current consumer of `Theme.cs`, so there is no live cross-host consumer to break today. This lowers immediate risk but does not eliminate the constraint: `Assets/_Game/` is read-only-unless-explicitly-requested per AGENTS.md, not deleted, and any future Unity `IJsonSerializer`/save-adapter work (C1 in AGENTS.md Known Issues) could add a consumer later. Treat the rule as "no existing Unity consumer to break, but still don't fork or rename `Theme.cs` members" rather than "safe to freely edit." Rollback: keep the new `ResponsiveScale` class as a pure additive wrapper with zero edits to `Theme.cs` itself wherever possible, so rollback is "delete the wrapper file"; if `Theme.cs` must gain new members, add them as new constants only (never modify existing constant values), and land that as its own revertible commit separate from the Godot container/tier logic. Verify with a visual smoke pass (`--ui-layout-selftest`, already exists per `HostCliAction.UiLayoutSelfTest`) before merging.

---

### Step 3: Create AshfallResponsiveContainer

**Goal:** Build a reusable container node that automatically adjusts its child layout based on the current viewport tier.

**Implementation:**
- Create `src/UI/Layout/AshfallResponsiveContainer.cs`:
  ```csharp
  namespace AtomicWar.GodotApp.UI.Layout;

  public partial class AshfallResponsiveContainer : Container
  {
      [Export] public int CompactColumns { get; set; } = 1;
      [Export] public int StandardColumns { get; set; } = 2;
      [Export] public int ExpandedColumns { get; set; } = 3;
      [Export] public bool StackOnCompact { get; set; } = true;

      private LayoutTier _currentTier;

      public override void _Ready() { ... }
      public override void _Notification(int what) { ... }

      private void RelayoutChildren() { ... }
  }
  ```
- Behavior:
  - On `NOTIFICATION_RESIZED` or viewport size change, recalculate `LayoutTier`
  - Rearrange children into the column count specified for the current tier
  - Apply scaled spacing from `ResponsiveScale` (wrapping `Ashfall.Core.UI.Theme` — see corrected Step 2)
  - If `StackOnCompact` is true and tier is Compact, force single-column vertical stack
  - Emit a `LayoutTierChanged` signal so panels can react (hide optional elements, abbreviate labels)
- Create `src/UI/Layout/ResponsiveMarginContainer.cs` — wraps any child with tier-appropriate margins
- Create `src/UI/Layout/ResponsiveFontScaler.cs` — applies scale factor to all Label/RichTextLabel descendants

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --responsive-container-selftest
```

**Done when:**
- `AshfallResponsiveContainer` relays out children correctly at all 4 tiers, verified by an automated test that asserts child bounding boxes at each tier (not just visual inspection)
- Font scaler adjusts all descendant labels
- Container handles dynamic child addition/removal without leaking `LayoutTierChanged` signal subscriptions (verify with a repeated add/remove loop under `--responsive-container-selftest` and check subscriber count does not grow)
- No regressions in existing panel rendering at 1920x1080 — confirmed by running the existing `--ui-layout-selftest` (`HostCliAction.UiLayoutSelfTest`, already present in `HostCli.cs`) before and after this step with no new failures

**Risk & Rollback:** Medium risk — this is new code with no existing callers, so it is additive and safe to merge without touching any panel. Rollback: delete the 3 new files; nothing else references them until Step 4. `--responsive-container-selftest` is a new CLI verb — add it via `HostCli.cs`'s existing `Has(args, ...)` pattern, consistent with all other selftest verbs, not a new framework.

---

### Step 4: Migrate Critical Panels to Responsive Layout

**Goal:** Convert the 5 most-used panels (Survivors, Inventory, Dashboard, Expeditions, Map) from fixed-pixel `CustomMinimumSize`/font values to responsive, tiered layout.

**Implementation:**
- For each panel:
  1. Replace hardcoded `CustomMinimumSize`/font-size literals with `ResponsiveScale`-wrapped `Theme` lookups scaled by tier (there is no absolute `Position`/`Size` to replace in these panels — verified none of the 5 target panels below assign `Control.Position` directly; note `ShelterPanel.cs` does assign `SubViewport.Size = new Vector2I(...)` for its interior 3D viewport render target, which is unrelated to Control layout and is correctly out of scope for this migration)
  2. Wrap content sections in `AshfallResponsiveContainer` with appropriate column counts
  3. Replace hardcoded spacing with tier-scaled `Theme` values (per corrected Step 2)
  4. Add `StackOnCompact` behavior for information-dense sections
  5. Test at Compact, Standard, Expanded, Large viewports
- Migration order (by player usage frequency) — **corrected to the actual class names in `src/UI/`** (the original draft used invented names that do not exist in the codebase):
  1. **`SurvivorsPanel`** (`src/UI/SurvivorsPanel.cs`) — most viewed, has survivor list + detail split
  2. **`InventoryPanel`** (`src/UI/InventoryPanel.cs`) — grid layout, needs column reflow
  3. **`GameDashboardPanel`** (`src/UI/GameDashboardPanel.cs`) — multiple stat widgets, natural grid
  4. **`ExpeditionPanel`** (`src/UI/ExpeditionPanel.cs`) — complex layout with map + party + supplies
  5. **`MapPanel`** (`src/UI/MapPanel.cs`) — viewport-sized, needs safe margins at all resolutions
- Before starting implementation, confirm player-usage-frequency ordering against actual telemetry/playtesting notes if any exist; this batch assumes the stated order without a cited source — if no usage data exists, state that explicitly and treat the order as a reasonable guess, not a measured fact
- For each panel, preserve the 1920x1080 appearance exactly (regression baseline)
- Document migration pattern in `docs/responsive_migration_guide.md` for the remaining 78 panels (83 total − 5 migrated in this batch)

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --ui-panel-selftest   # Existing panel tests still pass
godot --headless --path . -- --responsive-panel-check   # New: test panels at 3 resolutions
```

**Done when:**
- All 5 critical panels render correctly at 1280x720, 1920x1080, 2560x1440, and 3840x2160
- No text clipping, no overlap, no overflow at any tested resolution — defined concretely as: no child `Control.Size` component exceeds its parent's available `Size`, and no two sibling `Control` bounding rects intersect (this is the same assertion Step 6 automates; Step 4's manual check and Step 6's automated test must agree on the same definition)
- 1920x1080 appearance is unchanged from before migration — verified via the existing `--ui-panel-selftest` verb plus a manual screenshot comparison (note: this repo has no automated screenshot-diff tooling today; "pixel-identical" is not achievable without building one, so treat this as a manual QA checklist item, not an automated gate)
- Migration guide exists for the remaining 78 panels, and states the audit classification (from Step 1) for each of them

**Risk & Rollback:** Medium-High risk — this step touches 5 live, shipped panels that players use most (per the plan's own usage-frequency ordering). A layout regression here is player-visible. Rollback: migrate and merge one panel per commit (not all 5 in one commit), so any single panel's regression can be reverted independently without losing the other 4. Keep the pre-migration panel file diffable against its git history for a fast revert.

---

### Step 5: Add Global UI Scale Factor

**Goal:** Let players manually override the automatic scaling with a user preference (accessibility: some players need larger UI regardless of resolution).

**Implementation:**
- Add `ui_scale_factor` to player settings (persisted in user config, not save file):
  ```csharp
  // Range: 0.5 to 2.0, default 1.0
  // Multiplies on top of the tier-based scale factor
  public float UiScaleFactor { get; set; } = 1.0f;
  ```
- Create `src/UI/Layout/UiScaleManager.cs`:
  - Reads `ui_scale_factor` from settings on startup
  - Exposes `EffectiveScale = TierScale * UiScaleFactor`
  - Notifies all `ResponsiveFontScaler` and `AshfallResponsiveContainer` instances on change
  - Clamps effective scale so UI never exceeds viewport bounds (prevent infinite growth)
- Add to settings panel:
  - Slider: "UI Scale" (50% to 200%, step 10%)
  - Live preview: changes apply immediately without restart
  - Reset button: returns to 100% (auto-scaling only)
- Ensure the scale factor persists across sessions (write to Godot `user://` config)
- Scale factor does NOT affect game simulation — purely presentation

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --ui-scale-selftest   # Test at 0.5, 1.0, 1.5, 2.0
```

**Done when:**
- Players can set UI scale from 50% to 200%
- Setting persists across game restarts
- Extreme scales (0.5x, 2.0x) do not cause overlaps or push content off-screen
- Scale applies to all text, spacing, and container sizing uniformly
- No impact on game simulation or saves — verify concretely by running an existing save selftest (e.g. `--holdfast-save-selftest`) before and after setting a non-default `ui_scale_factor`, confirming the save checksum is unaffected, since `ui_scale_factor` must live in user preferences (`user://` config), never in a `CaptureState()` DTO

**Risk & Rollback:** Low risk if `ui_scale_factor` stays out of save DTOs as designed; medium risk if a future change wires it into `CaptureState()`, which would violate AGENTS.md's cross-host save invariant (a presentation-only value leaking into a payload shared by both hosts). Rollback: `UiScaleManager` and the settings slider are additive; deleting them reverts to tier-only scaling with no save-data migration needed.

---

### Step 6: Add Resolution Test Suite

**Goal:** Automated tests that instantiate panels at multiple resolutions and verify no visual breakage.

**Implementation:**
- **Correction — test project placement:** `Ashfall.Core.Tests` targets `net9.0` and has **no Godot package reference** (verified: `Ashfall.Core.Tests.csproj` references only `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio` — no `Godot.NET.Sdk`). This is intentional per AGENTS.md: the Core test suite must run without Godot. Any test that instantiates a Godot `Control`/`Container` (as `ResponsiveLayoutTests.cs` below must, to check real bounding boxes) **cannot live in `Ashfall.Core.Tests`** and will fail to compile there. These tests belong in the Godot host project (`Ashfall.csproj`, `net8.0`, `Godot.NET.Sdk/4.7.1`) as a `--responsive-layout-selftest`-style headless CLI verb (consistent with how `--ui-layout-selftest` already works), or in a dedicated `src/Tests/` Godot-side test file. Only pure-math tests with no Godot type dependency (e.g. `ResponsiveBreakpoints.GetTier` boundary tests from Step 2, which take a plain `Vector2I`-free tier/int input) belong in `Ashfall.Core.Tests`.
- Create `Ashfall.Core.Tests/UI/ResponsiveLayoutTests.cs` — **relocate to a Godot-host test path per the correction above** (e.g. `src/Tests/ResponsiveLayoutSelfTest.cs`, run via a new CLI verb, not `dotnet test`):
  - Test helper: `SimulateViewportSize(int width, int height)` — sets the container available space
  - For each critical panel, test at 4 resolutions:
    - 1280x720 (Compact minimum)
    - 1920x1080 (Standard target)
    - 2560x1440 (Expanded)
    - 3840x2160 (Large)
  - Assertions per panel per resolution:
    - No child control extends beyond parent bounds (no overflow)
    - No two sibling controls overlap (bounding box intersection check)
    - All Label nodes have `Size.X >= MinimumSize.X` (no crushed text)
    - Total content height does not exceed scrollable area (or scroll is enabled)
- Create Godot-side integration test (`src/Tests/ResponsiveIntegrationTest.cs`):
  - Actually resize the viewport and screenshot each panel
  - Compare to baseline screenshots (optional, for human review)
- Test at ultrawide aspect ratios: 2560x1080, 3440x1440 (21:9)
- Test at portrait-ish ratios: 1080x1920 (unlikely but defensive)

**Verification:**
```bash
dotnet build Ashfall.csproj   # Godot-host test file compiles (not dotnet test — see placement correction above)
godot --headless --path . -- --responsive-layout-selftest   # New verb; runs the bounding-box assertions
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "ResponsiveBreakpoints"   # Pure-math boundary tests only
```

**Done when:**
- Tests cover all 5 migrated panels at 4+ resolutions
- Zero overlap/overflow failures at any tested resolution
- Tests catch real regressions (verify by intentionally breaking a panel's layout)
- Ultrawide and non-standard aspect ratios do not crash or produce unusable layout

**Risk & Rollback:** Low risk — this step is purely additive test/verification code with no runtime gameplay or UI-shipping changes. The only real risk is the test-placement error described above (Godot-typed tests silently failing to compile if left in `Ashfall.Core.Tests`) — mitigated by relocating them to the Godot host as specified. Rollback: delete the new CLI verb and Godot-side test file; no production code depends on this step.

---

### Step 7: Layout Compliance Test — Prevent Regression

**Goal:** A CI-gated test that scans all panel source files and flags absolute pixel constants that bypass the responsive system.

**Implementation:**
- Create `Ashfall.Core.Tests/UI/LayoutComplianceTests.cs`:
  - **Correction: there is no `src/UI/Panels/` directory.** All 83 panel files live flat in `src/UI/` (e.g. `src/UI/SurvivorsPanel.cs`, not `src/UI/Panels/SurvivorsPanel.cs`). Scan `src/UI/*.cs` files matching `class \w*Panel\s*:\s*Control` (the same pattern used to derive the 83 count in Step 1), not a nonexistent `Panels/` subfolder, via reflection or source text analysis
  - Flag violations:
    - `new Vector2(literal, literal)` assigned to `Position` (absolute positioning)
    - `Size = new Vector2(literal, literal)` outside of `MinimumSize` context — **exclude `SubViewport.Size = new Vector2I(...)` render-target assignments** (real, legitimate usage found at `src/UI/ShelterPanel.cs:240` and `src/UI/SnapshotOrchestrator.cs:133`; these are not Control layout and must not be flagged as violations)
    - Hardcoded font size integers not sourced from `Ashfall.Core.UI.Theme` (there is no `AshfallDesignTokens` — see corrected Architecture Decision/Step 2 above)
    - `CustomMinimumSize` with values > 400px (suspiciously large fixed sizes) — note `Theme.PanelMaxWidth = 420` and `Theme.TradePanelMaxWidth = 720` already exceed this threshold and are intentional; the allowlist below must cover known `Theme` constants, not just ad-hoc panel exceptions
  - Note this test is source-text analysis (reading `.cs` files as strings/regex, not reflecting over compiled Godot types), so it is valid to keep in `Ashfall.Core.Tests` (net9.0, no Godot dependency needed) — unlike Step 6's `ResponsiveLayoutTests.cs`, which does require Godot types and must live in the Godot host per that step's correction
  - Allowlist for justified exceptions (e.g., the map viewport has a fixed minimum)
  - Report: file, line, violation type, suggested fix
- Create a Roslyn analyzer (optional, future) that provides IDE-time warnings for the same patterns
- Add to CI: the compliance test runs as part of `dotnet test` and fails the build if new violations are introduced
- Grandfather existing violations: baseline count is captured, test fails only if count increases
- Provide a migration helper: `--layout-compliance-report` CLI verb that lists all current violations sorted by panel

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "LayoutCompliance"
godot --headless --path . -- --layout-compliance-report
```

**Done when:**
- Compliance test identifies all absolute-positioning patterns in panel code
- Baseline violation count is captured (existing panels are grandfathered)
- Any NEW absolute positioning in a panel file fails CI
- The 5 migrated panels have zero compliance violations
- Report lists remaining violations for future migration batches

**Risk & Rollback:** Low risk — read-only static analysis producing a report and a CI gate; touches no runtime/gameplay code. The one real risk is false positives against the legitimate `SubViewport.Size`/`Theme` constant patterns noted above; if the regex is too broad it will flag correct code and block unrelated PRs. Rollback: the compliance test and `--layout-compliance-report` verb are additive and can be deleted or the CI gate disabled without affecting any panel's actual behavior.

---

## Summary Table

| Step | Title | Risk | Depends On | Output |
|------|-------|------|-----------|--------|
| 1 | Audit Current Layout Patterns | Low | None | `docs/layout_audit_results.md` |
| 2 | Define Responsive Breakpoints | Medium (touches Core `Theme.cs`, see Step 2 correction) | Step 1 | `ResponsiveBreakpoints.cs`, `ResponsiveScale.cs` |
| 3 | Create AshfallResponsiveContainer | Medium | Step 2 | `AshfallResponsiveContainer.cs`, `ResponsiveMarginContainer.cs`, `ResponsiveFontScaler.cs` |
| 4 | Migrate Critical Panels | Medium-High (player-visible) | Step 3 | 5 panels converted, migration guide |
| 5 | Add Global UI Scale Factor | Low | Step 3 | `UiScaleManager.cs`, settings integration |
| 6 | Add Resolution Test Suite | Low | Step 4 | Godot-host `ResponsiveLayoutSelfTest` (relocated per Step 6 correction — not `Ashfall.Core.Tests`) |
| 7 | Layout Compliance Test | Low | Step 4 | `LayoutComplianceTests.cs`, CI gate |

---

## Exit Criteria

- [ ] All 5 critical panels render correctly at 1280x720, 1920x1080, 2560x1440, 3840x2160
- [ ] UI scale slider works from 50% to 200% with immediate preview
- [ ] Zero layout compliance violations in migrated panels
- [ ] Resolution test suite passes at all 4 target resolutions
- [ ] No regressions: existing 1920x1080 appearance unchanged
- [ ] `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- [ ] `dotnet test` — all responsive/layout tests pass (pure-math + compliance tests only; see Step 6 correction on test placement)
- [ ] Migration guide documented for remaining 78 panels (83 total − 5 migrated in this batch)

---

## Future Work (Not in This Batch)

- Migrate remaining 78 panels in batches of 10-15
- Roslyn analyzer for IDE-time layout compliance warnings
- Ultrawide-specific layouts (show extra sidebar on 21:9)
- Steam Deck verified layout preset
- Touch-friendly hit targets for controller/handheld input
- Dynamic font loading (BarlowCondensed light/regular/bold variants per tier)


---

## Review Notes (Corrected)

Adversarial review performed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Every number and file reference below was independently re-verified with `grep`/`find`, not taken from the plan's own prior self-corrections.

### Factual errors found and fixed

1. **False core premise — a design-token class already exists.** The plan's Architecture Decision and Step 2 claimed "there is currently no `AshfallDesignTokens` class or equivalent in the codebase; this batch must create it." This is wrong. `Assets/Ashfall.Core/UI/Theme.cs` defines `public static class Theme` (namespace `Ashfall.Core.UI`) with spacing, typography, radius, color, and panel-sizing constants — engine-agnostic, zero `UnityEngine`/`Godot` references, exactly matching Invariant 1. It is imported today via `using DesignTheme = Ashfall.Core.UI.Theme;` in at least 10 `src/UI/*.cs` files, including two of the five panels this batch names for migration (`InventoryPanel.cs`, `SurvivorsPanel.cs`) and shared components `AshfallDataGrid.cs` / `AshfallMetricCard.cs`. Fixed throughout: replaced all `AshfallDesignTokens` (new class) references with a corrected design that wraps the existing `Theme` class instead of duplicating it, and flagged the Core-vs-Godot-host boundary implications (`Theme.cs` is cross-host shared code by construction, per Invariant 1/6, even though this review's own `grep -rl "Ashfall.Core.UI.Theme\|DesignTheme" Assets/_Game/` found zero current Unity-side consumers — see the updated Step 2 Risk & Rollback for the re-verified, non-hedged version of this claim).

2. **Panel count (83) — verified accurate, but the corroborating method was incomplete.** Confirmed independently: `find src/UI -maxdepth 1 -name "*Panel.cs"` → 83, and `grep -rl 'class \w*Panel\s*:\s*Control' src/UI/` → 83, and both lists agree file-for-file. The plan's "83" figure is correct. No change needed to the count itself; tightened the Risk field to cite both independent methods.

3. **`Position = new Vector2` count (1 file, non-panel) — verified accurate.** Confirmed: exactly one match, `src/UI/UiBackgroundCarousel.cs`, and it is not a `*Panel.cs` file. No change needed.

4. **`Size = new Vector2` count — factually wrong ("0 matches" claimed, 2 actual matches).** The plan stated "confirmed: 0 matches currently — expect empty" for direct `Size = new Vector2(w, h)` assignments. Re-running the grep found **2** matches: `src/UI/ShelterPanel.cs:240` and `src/UI/SnapshotOrchestrator.cs:133`. Both are `SubViewport.Size = new Vector2I(...)` (a 3D render-target pixel dimension), not `Control.Size` layout overrides, so the *substantive* conclusion ("no Control-layout absolute sizing") still holds — but the audit tooling and compliance test in Steps 1 and 7 must explicitly exclude `SubViewport`/`Vector2I` assignments rather than assuming the bucket is empty, or they will produce false positives on day one. Fixed in Steps 1 and 7.

5. **Nonexistent directory referenced.** Step 7 told the compliance test to scan `src/UI/Panels/*.cs`. That directory does not exist — all 83 panels are flat directly under `src/UI/`. Fixed to scan `src/UI/*.cs` filtered by the panel-class regex, consistent with how Step 1 derives its count.

6. **Test-project placement error — would not compile.** Step 6's `Ashfall.Core.Tests/UI/ResponsiveLayoutTests.cs` needs to instantiate real Godot `Control`/`Container` nodes to check bounding boxes. `Ashfall.Core.Tests.csproj` targets `net9.0` and has no `Godot.NET.Sdk` reference (verified by reading the `.csproj` — only `Microsoft.NET.Test.Sdk`/`xunit` packages) — this is intentional per AGENTS.md ("the `Ashfall.Core` test suite must run WITHOUT Unity/Godot"). A test file that references `Godot.Control` in that project will fail to compile, not just fail at runtime. Fixed: relocated the Control-instantiating tests to the Godot host as a new `--responsive-layout-selftest` CLI verb (matching the existing `--ui-layout-selftest` pattern), and left only pure-math tests (breakpoint boundaries) in `Ashfall.Core.Tests`. Step 7's `LayoutComplianceTests.cs` is fine to stay in `Ashfall.Core.Tests` because it is source-text/regex analysis, not Godot-type reflection — this distinction is now called out explicitly so a future implementer doesn't move it unnecessarily.

7. **Missing Risk & Rollback sections.** Steps 6 and 7 had no Risk & Rollback subsection, unlike every other step in the document (an internal inconsistency that looks like the plan was drafted, then partially reviewed, then abandoned mid-edit). Added both.

8. **Arithmetic/wording inconsistency.** Step 4 correctly computed "78 panels remaining" (83 − 5), but the Exit Criteria and Future Work sections at the bottom said "~80" and "80" respectively. Fixed both to 78 for internal consistency.

9. **Scope estimate did not account for the corrections above.** The original "~2 weeks" estimate predates the discovery that Step 2 touches cross-host Core code and that Step 6 needs an entirely different test-hosting approach than drafted. Revised to ~3 weeks and flagged in the summary table.

### Attacks that did not find a defect (confirmed sound)

- The "Not chosen" alternatives (uniform `stretch_mode`, per-panel overrides, CSS media queries) are reasonable rejections with real justifications, not strawmen.
- The 5 named migration-target panels (`SurvivorsPanel`, `InventoryPanel`, `GameDashboardPanel`, `ExpeditionPanel`, `MapPanel`) all exist at the stated paths under `src/UI/`.
- The shared shell classes named (`AshfallDashboardShell`, `AshfallSidebar`, `AshfallStatusRail`, `AshfallDataGrid`, `AshfallUiHelpers`) all exist in `src/UI/`.
- `--ui-layout-selftest` (`HostCliAction.UiLayoutSelfTest`) is real and already registered in `src/Host/HostCli.cs`, so Step 2/4's reliance on it as a pre-existing regression check is valid.
- Godot 4.7 target is consistent with `project.godot` (`config/features=PackedStringArray("4.7", "C#", "Compatibility")`) and AGENTS.md.
- The player-usage-frequency migration ordering in Step 4 is honestly flagged by the plan itself as an unverified guess rather than measured data — this is good practice, not a defect, and was left as-is.
- Step 4's manual-vs-automated overlap/overflow definition consistency check (matching Step 6's automated assertion) is a genuinely useful piece of rigor already present in the plan; left unchanged.

### Independent second-pass adversarial re-verification (this review)

This document arrived with an existing "Review Notes (Corrected)" section claiming prior verification. Per the review mandate, none of those prior claims were taken on trust — every cited number and file path was re-derived from scratch against the live repo at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` before this pass made any further edit. Results:

- `find src/UI -maxdepth 1 -name '*Panel.cs' | wc -l` → 83, and `grep -rl 'class \w*Panel\s*:\s*Control' src/UI/ | wc -l` → 83, and a `diff` of the two file lists is empty (identical sets). The 83 count is confirmed exact by two independent methods with no divergence.
- `grep -rl 'Position\s*=\s*new Vector2(' src/UI/` → exactly 1 file, `src/UI/UiBackgroundCarousel.cs` (not a panel). Confirmed.
- `grep -rn 'Size\s*=\s*new Vector2I(' src/UI/` → exactly 2 matches, at the exact cited locations `src/UI/ShelterPanel.cs:240` and `src/UI/SnapshotOrchestrator.cs:133`, both `SubViewport.Size` render-target assignments. Confirmed byte-for-byte against the file contents.
- `CustomMinimumSize = new Vector2(` → 89 files; Container node usage (`VBoxContainer|HBoxContainer|GridContainer|MarginContainer`) → 90 files; anchor-preset usage → 89 files. All three match the plan's cited figures exactly.
- `Assets/Ashfall.Core/UI/Theme.cs` exists, is `namespace Ashfall.Core.UI`, `public static class Theme`, contains a header comment stating zero UnityEngine/Godot references, and its constants (`HudEdge=24`, `SpacingXs=4`, `SpacingXl=24`, `FontSizeH1=28`, `FontSizeLabel=10`, `PanelMaxWidth=420`, `PanelMinWidthNarrow=260`, `PanelMaxWidthWide=520`, `TradePanelMinWidth=560`, `TradePanelMaxWidth=720`, `EconomyPanelMaxWidth=500`) match every specific value cited in this plan's Step 7, including the two values (420, 720) that intentionally exceed the plan's proposed 400px "suspicious size" compliance threshold. Confirmed.
- `grep -rl 'using DesignTheme = Ashfall.Core.UI.Theme' src/UI/ | wc -l` → 37 files (the plan conservatively says "at least 10"). Confirmed, and the true number is higher than stated, which if anything strengthens the plan's point rather than weakening it.
- `grep -rl 'Ashfall.Core.UI.Theme\|DesignTheme' Assets/_Game/` → 0 matches. This review upgrades the plan's earlier "unverified" hedge on Unity-side `Theme.cs` consumption to a verified "zero, as of this commit" — see the corrected Step 2 Risk & Rollback and Review Notes item 1 above, both updated by this pass.
- All 5 named panels (`SurvivorsPanel.cs`, `InventoryPanel.cs`, `GameDashboardPanel.cs`, `ExpeditionPanel.cs`, `MapPanel.cs`) and all 5 named shared shell classes (`AshfallDashboardShell`, `AshfallSidebar`, `AshfallStatusRail`, `AshfallDataGrid`, `AshfallMetricCard`, plus `AshfallUiHelpers`) exist at exactly one file each under `src/UI/`. Confirmed.
- `project.godot`'s `config/features=PackedStringArray("4.7", "C#", "Compatibility")` matches the plan's Godot-4.7 assumption. Confirmed.

No new factual defects were found beyond the single hedge corrected above (Unity `Theme.cs` consumption, now stated as verified-zero rather than unverified). The plan's own prior self-corrections hold up under independent re-derivation; this pass found no case where the prior "Review Notes" text itself was inaccurate.
