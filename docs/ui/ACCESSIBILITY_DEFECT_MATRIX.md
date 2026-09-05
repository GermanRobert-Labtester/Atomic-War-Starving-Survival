# ASHFALL — ACCESSIBILITY DEFECT MATRIX (PLAN 80 / TASK B21)

**Audit Target:** Top Dashboard & Overlay Panels (Baseline Assessment)
**Date:** 2026-09-03
**Status:** Pre-Migration Defect Baseline

---

## Panel Walkthrough Matrix

| Panel | Opener | Initial Focus (Baseline) | Tab Order | Arrow Behavior | Esc Handling | Focus Dead Ends | Mouse-Only Actions | Focus Restoration (Baseline) |
|---|---|---|---|---|---|---|---|---|
| **GameDashboardPanel** (Dashboard) | Game Start / Tab / Menu | None (unfocused) | Top header → Nav rail buttons → Action buttons | None (relies on Godot default 2D spatial search) | Opens Menu | Trapped on disabled buttons if any | Some gauge tooltips hover-only | None |
| **StatusPanel** (Status) | Dashboard nav "STATUS" | None (unfocused) | None (scroll container only, no child buttons) | Vertical scroll only | Hardcoded `Key.Escape` in `_UnhandledInput` | ScrollContainer absorbs Tab with no internal stop | None (pure readout) | None (opener lost) |
| **ResearchAtlasPanel** (Research) | Dashboard nav "RESEARCH" | None (unfocused) | Sidebar items → DataGrid headers → Action rows | Arrow navigation inside grids not bound to keyboard selection | Hardcoded `Key.Escape` in `_UnhandledInput` | DataGrid rows mouse-click only (`OnRowSelected` via GUI input) | Selecting nodes, starting research | None (opener lost) |
| **InventoryPanel** (Inventory) | Dashboard nav "INVENTORY" | None (unfocused) | Header close button → Sidebar filter buttons | None inside item rows (static labels/icons) | Hardcoded `Key.Escape` in `_UnhandledInput` | Focus drops off after sidebar filters | Equipping/consuming items from inventory detail | None (opener lost) |
| **CraftingPanel** (Crafting) | Dashboard nav "CRAFTING" | None (unfocused) | Filter buttons → Workshop/Pharma buttons → Craft buttons | Inconsistent between dynamic recipe list and static buttons | Hardcoded `Key.Escape` in `_UnhandledInput` | ScrollContainer bypasses Tab into background | None | None (opener lost) |
| **MedicalPanel** (Medical) | Dashboard nav "MEDICAL" | None (unfocused) | Header close button → Sidebar cohort buttons → Treatment action buttons | Unbound in patient card lists | Hardcoded `Key.Escape` in `_UnhandledInput` | Treatment buttons lose focus after click when list refreshes | Triage priority toggles | None (opener lost) |

---

## Required Remediation Actions

1. **Shared Focus Policy (`AshfallFocusPolicy`)**:
   - Central helper for `OpenWithFocus(root, initial, opener)`
   - `TrapFocus(root)` to prevent Tab from leaking behind modal backdrops into the dashboard
   - `RestoreFocus(opener)` returning focus to the triggering nav button on close
   - `ApplyFocusVisibleStyle(control)` applying a distinct 2px warm/hot focus ring so keyboard users never lose the cursor.
2. **Standardized Close Handling**:
   - Replace raw `Key.Escape` with canonical `AshfallInputActions.IsCloseOrCancel(@event)`.
   - Ensure closing an overlay restores focus to its opener.
3. **DataGrid Keyboard Selection**:
   - Enable Space/Enter and Up/Down arrow selection on `AshfallDataGrid` rows.
4. **Theme Contrast Compliance**:
   - Adjust `Theme.Critical` and `Theme.Dim` to meet WCAG AA (4.5:1) text contrast floor against dark backgrounds.
5. **Accessible Labels**:
   - Provide explicit tooltip or accessibility names for icon buttons and navigation elements.
