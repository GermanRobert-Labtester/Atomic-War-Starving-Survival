# ASHFALL — Combat UI Lane-2 Defects & Fit Audit (Plan 62 / B3)

**Document ID:** DOC-UI-COMBAT-LANE2-DEFECTS
**Date:** 2026-09-05
**Author:** Antigravity
**Target Panel:** `src/UI/CombatPanel.cs`
**Reference Snapshot:** `snapshots/combat_hud_default.png`

---

## 1. Overview

An audit of the Tactical Combat UI (`src/UI/CombatPanel.cs` and `src/UI/CombatDetailPanel.cs`) revealed layout, responsiveness, and component primitive inconsistencies compared to the standardized Phase-12 / Lane-1 design system (`AshfallDataGrid`, `AshfallDashboardShell`, `AshfallUiHelpers`, and semantic tokens).

Combatant rosters and weapon states were authored using unstructured `RichTextLabel` strings, leading to horizontal text drift, wrapping artifacts, and illegibility during multi-combatant engagements.

---

## 2. Lane-2 Defect Table

| Defect ID | Panel / Node Path | Primitive Involved | Visual Symptom | Root Cause | Fix Scope |
|---|---|---|---|---|---|
| **L2-CMB-001** | `CombatPanel / Scroll / VBox / _combatants` | `RichTextLabel` (820×130) | Horizontal text drift across rows; column data (Name, Lane, HP, Armor, Weapon, Jam status) does not align vertically across combatants. | Monolithic text string appending instead of tabular cells; variable-width survivor and enemy names cause column shifts. | Replace with `AshfallDataGrid` with 8 structured columns, explicit `MinWidth` floors, and semantic status coloring. |
| **L2-CMB-002** | `CombatPanel / Scroll / VBox / _weapons` | `RichTextLabel` (820×90) | Bullet-point text rendering of armory condition and jam chance; lacks header labels and status clarity. | String interpolation into raw RichTextLabel without layout constraints or token states. | Replace with `AshfallDataGrid` with 6 structured columns (`Weapon`, `Condition`, `Jam %`, `Status`, `Repair Cost`, `Ammo`). |
| **L2-CMB-003** | `CombatPanel / Scroll / VBox / Action Buttons` | `Button` | Player has no visible indication why an action button (e.g. `FIRE`, `SUPPRESS`, `CLEAR JAM`) failed or is unavailable until after clicking. | No preflight check binding button disabled state or tooltip reason strings. | Add preflight status checking to action buttons with descriptive tooltips (`Jammed`, `No Ammo`, `Not Suppression Capable`, `Encounter Resolved`). |
| **L2-CMB-004** | `CombatPanel / Keyboard Navigation` | `_UnhandledInput` | Only `Escape` key is bound; keyboard-only players cannot cycle targets, switch stances, or execute combat commands without mouse. | Input handler only captured `Key.Escape`. | Add keyboard cycling for targets (`Tab` / `Shift+Tab`), numbers 1–5 for primary actions, and space/enter to confirm. |
| **L2-CMB-005** | `CombatDetailPanel / TacticsData` | `VBoxContainer` | Stance trade-offs rendered as unformatted single-line labels without comparison metrics. | Static string formatting without tabular alignment. | Format stance trade-offs with explicit column metrics and current-stance highlighting. |

---

## 3. DataGrid Column Specifications

### 3.1 Combatants Grid (`_combatantsGrid`)

- **Container:** `AshfallDataGrid`
- **Minimum Dimensions:** Width 820px, Height 140px
- **Columns:**
  1. `Mark` (Width: 30px, Align: Center) — `▶` (player) or `●` (hostile)
  2. `Combatant` (Width: 160px, Align: Left) — Display name
  3. `Lane` (Width: 70px, Align: Center) — Left / Center / Right
  4. `Health` (Width: 100px, Align: Right) — HP / Max HP (Critical when downed)
  5. `Cover` (Width: 70px, Align: Right) — Cover rating %
  6. `Armor` (Width: 70px, Align: Right) — Armor rating %
  7. `Status` (Width: 120px, Align: Left) — OK / DOWNED (turns) / PINNED / LAST STAND
  8. `Weapon` (Width: 180px, Align: Left) — Weapon name + condition % + [JAM]

### 3.2 Weapons Grid (`_weaponsGrid`)

- **Container:** `AshfallDataGrid`
- **Minimum Dimensions:** Width 820px, Height 100px
- **Columns:**
  1. `Weapon` (Width: 180px, Align: Left) — Weapon display name
  2. `Condition` (Width: 90px, Align: Right) — Condition % (Critical if <30%)
  3. `Jam Risk` (Width: 80px, Align: Right) — Jam chance %
  4. `Status` (Width: 110px, Align: Center) — Ready / JAMMED
  5. `Repair` (Width: 100px, Align: Right) — Scrap cost
  6. `Ammo` (Width: 80px, Align: Right) — Ammo count

---

## 4. Architectural Invariants

1. **Presentation-Only:** The `AshfallDataGrid` instances are pure views. They format strings and set semantic cell states (`Normal`, `Positive`, `Warning`, `Critical`, `Muted`). They never compute or mutate combat simulation state.
2. **Deterministic Refresh:** The grids re-render from `CombatHostSession.Snapshot()` on every `StateChanged` event.
3. **No Component Forking:** Grids reuse the existing `AshfallDataGrid` class from `src/UI/AshfallDataGrid.cs` without introducing any combat-specific widget variants.
