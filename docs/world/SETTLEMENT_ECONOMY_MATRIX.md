# Settlement Economy & Trade Loop Matrix

## 1. Trade Goods & Needs Balance

| Settlement | Primary Export | Primary Import | Stocked Goods | Needs |
|---|---|---|---|---|
| `settlement_tinkers_notch` | `electronic_scrap` | `clean_water` | `electronic_scrap`, `copper_wire_10m_of_10m`, `battery` | `clean_water`, `medical_kit`, `canned_food` |
| `settlement_ferry_crossing` | `clean_water` | `scrap_metal` | `clean_water`, `cooked_meat`, `fuel` | `scrap_metal`, `mechanical_parts`, `cloth` |
| `settlement_nine_rails` | `mechanical_parts` | `canned_food` | `mechanical_parts`, `scrap_metal`, `fuel` | `canned_food`, `clean_water`, `bandage` |
| `settlement_iron_siding` | `scrap_metal` | `fuel` | `scrap_metal`, `mechanical_parts`, `heavy_industrial_motor` | `fuel`, `antibiotics`, `clean_water` |
| `settlement_fort_karkov` | `heavy_industrial_motor` | `canned_food` | `heavy_industrial_motor`, `scrap_metal`, `mechanical_parts` | `canned_food`, `clean_water`, `medical_kit` |
| `settlement_lock_seven` | `fuel` | `cloth` | `fuel`, `mechanical_parts`, `chemicals` | `cloth`, `water_filter`, `canned_food` |
| `settlement_brine_pans` | `item_crossing_traded_salt` | `scrap_metal` | `item_crossing_traded_salt`, `chemicals`, `clean_water` | `scrap_metal`, `fuel`, `bandage` |
| `settlement_silo_burrow` | `canned_food` | `scrap_metal` | `canned_food`, `cloth`, `clean_water` | `scrap_metal`, `battery`, `medical_kit` |
| `settlement_slate_hollow` | `scrap_metal` | `clean_water` | `scrap_metal`, `chemicals`, `mechanical_parts` | `clean_water`, `canned_food`, `antibiotics` |
| `settlement_pilgrim_hearth` | `medical_kit` | `cloth` | `medical_kit`, `item_stabilization_tea`, `bandage` | `cloth`, `scrap_metal`, `fuel` |
| `settlement_cape_beacon` | `clean_water` | `electronic_scrap` | `clean_water`, `item_fungicide_fogger`, `medical_kit` | `electronic_scrap`, `copper_wire_10m_of_10m`, `battery` |
| `settlement_st_nicholas` | `clean_water` | `cloth` | `clean_water`, `bandage`, `antibiotics` | `cloth`, `canned_food`, `fuel` |

## 2. Complementary Trade Loops
1. **The Salt & Preserves Loop:** `settlement_brine_pans` exports `item_crossing_traded_salt` → `settlement_silo_burrow` uses it for meat/grain curing → exports `canned_food` to `settlement_nine_rails`.
2. **The Electronics & Heavy Machinery Loop:** `settlement_tinkers_notch` exports `electronic_scrap` → `settlement_cape_beacon` needs it for optics → `settlement_iron_siding` exports `scrap_metal` to `settlement_tinkers_notch`.
3. **The Water & Medicine Loop:** `settlement_st_nicholas` & `settlement_pilgrim_hearth` export `clean_water` and `medical_kit` → `settlement_fort_karkov` and `settlement_slate_hollow` purchase them for heavy industrial workers.
