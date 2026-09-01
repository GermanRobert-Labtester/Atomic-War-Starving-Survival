# Production Regression & Verification Matrix

This document defines the 16 end-to-end integration and regression scenarios executed to prove the coherence of ASHFALL's industrial world.

---

## 1. Regression Scenarios

1. **Scrap → Foundry Tooling → Workshop Maintenance**: Smelt `foundry_prod_replacement_die` from scrap; apply to restore machine tool wear.
2. **Structural Casting → Sky-Armor Repair**: Pour `foundry_prod_roof_armor_plate` in Band 4; apply to repair overhead sky armor.
3. **Treaty Labor Block**: High-tier treaty product (`foundry_prod_brine_pipe`) is blocked when treaty standing or stoker conditions are violated.
4. **Labor Dispute → Strike → Production Halt → Resolution**: Trigger `dispute_stoker_walkout`; verify heat preparation halt; resolve by allocating water; verify resumption.
5. **Duty Roster Staffing**: Reassign workers from maintenance to foundry; verify heat duration reduction.
6. **Crop Lifecycle**: Plant `item_seed_hardy_tuber`; verify progression through stages 1 → 2 → 3; harvest `crop_hardy_tuber`.
7. **Seasonal Growth Variance**: Plant `crop_grain` in summer vs winter; verify summer bonus and winter penalty.
8. **Blight & Counterplay**: Trigger blight on mature crop; apply `item_blight_treatment`; verify recovery to clean yield.
9. **Apiculture Yield**: Maintain hive with healthy queen; verify honey and wax buffer accumulation; extract `item_honey_pot`.
10. **Salt Extraction & Processing**: Assign workers to salt mine vein; extract halite; grade into `item_preservation_salt`.
11. **Preservation Conversion**: Cook fresh tubers and salt into `food_pickled_tubers`; verify shelf-life extension from 10 to 45 days.
12. **Preserved Surplus Trade**: Export `food_canned_grain_stew` and `item_trade_salt_sack` to regional caravan; verify fair return value.
13. **Save During Active Heat**: Save while cupola is in molten stage; reload; verify heat timer and charge contents persist.
14. **Save During Crop Growth**: Save while plot is in Stage 2 (Growing); reload; verify exact growth hours and hydration persist.
15. **Save During Active Strike**: Save with active `strike_stage_slowdown`; reload; verify slowdown penalty remains active.
16. **Determinism Verification**: Replay paired 100-day production schedules with identical seed; verify byte-identical output records.
