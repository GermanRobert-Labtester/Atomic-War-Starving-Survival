# Existing 6-Action Audit

## Action Inventory

### 1. `action_weigh_goods` — "Weigh Goods"
- **Category:** Companion labor (loud_labor)
- **baseScore:** 0.40
- **fatigueGate:** 85.0
- **skillBonusFactor:** 0.25
- **Tags:** `loud_labor`
- **Curve:** Identity
- **Role:** Companion flavor — Osran Kell bias
- **Vetoes:** Coward

### 2. `action_read_contract` — "Read Contract"
- **Category:** Companion (quiet)
- **baseScore:** 0.35
- **fatigueGate:** 90.0
- **skillBonusFactor:** 0.20
- **Tags:** —
- **Curve:** Identity
- **Role:** Companion flavor — The Tally bias
- **Vetoes:** None

### 3. `action_canvas_support` — "Canvas Support"
- **Category:** Companion labor (menial_labor)
- **baseScore:** 0.45
- **fatigueGate:** 80.0
- **skillBonusFactor:** 0.15
- **Tags:** `menial_labor`
- **Curve:** Identity
- **Role:** Companion flavor — Amnesty campaign bias
- **Vetoes:** GodComplex

### 4. `action_run_vouch` — "Run Vouch"
- **Category:** Companion (quiet)
- **baseScore:** 0.30
- **fatigueGate:** 88.0
- **skillBonusFactor:** 0.10
- **Tags:** —
- **Curve:** Identity
- **Role:** Companion flavor — Standing record bias
- **Vetoes:** None

### 5. `action_audit_inventory` — "Audit Inventory"
- **Category:** Shelter labor (quiet_labor)
- **baseScore:** 0.35
- **fatigueGate:** 80.0
- **skillBonusFactor:** 0.0
- **Tags:** `quiet_labor`
- **Curve:** Identity
- **Role:** Shelter maintenance flavor
- **Vetoes:** None

### 6. `action_file_report` — "File Report"
- **Category:** Shelter labor (quiet_labor)
- **baseScore:** 0.35
- **fatigueGate:** 80.0
- **skillBonusFactor:** 0.0
- **Tags:** `quiet_labor`
- **Curve:** Identity
- **Role:** Shelter maintenance flavor
- **Vetoes:** None

## Category Distribution

| Category | Count | Actions |
|----------|-------|---------|
| Companion flavor | 4 | weigh_goods, read_contract, canvas_support, run_vouch |
| Shelter labor | 2 | audit_inventory, file_report |

## Duplicate Check

None of the 6 existing actions duplicate the requested Plan 72 actions:
- No `action_rest`, `action_sleep` exists
- No `action_cook_food`, `action_treat_wounded`, `action_repair_equipment`, etc. exists
- The existing actions are companion-flavor, not general shelter autonomy

## Preservation

All 6 existing actions are preserved byte-for-byte. No field values changed. No IDs renamed. No descriptions altered. No tags removed.

## Stability Test

`UtilityAiTests.Catalog_LoadsFourCrossingActionsWithBoundFields` asserts exact count of 6. `UtilityAiExpandedCatalogTests.Catalog_PreservesOriginal6ActionsByteAndFieldParity` asserts exact field values for the first 6 actions after expansion.