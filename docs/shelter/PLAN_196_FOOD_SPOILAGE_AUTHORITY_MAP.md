# Plan 196 — Food type / spoilage authority map

**Status:** SEALED — map ACCEPTED + type/temp seam implemented 2026-09-12.  
**Package:** `DEBT-196-FOOD-SPOILAGE-BOUNDARY` (map RETIRED) + `DEBT-196-FOOD-TYPE-TEMP-SEAM` (RETIRED)  
**Batches:** `BATCH-2026-09-12-DEBT-196-FOOD-BOUNDARY` + `BATCH-2026-09-12-DEBT-196-FOOD-SEAM` (both closed)  
**Rebase source:** `FOOD-SPOILAGE-OWNERSHIP-BOUNDARY` in `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`  
**Date:** 2026-09-12  
**Sign-off:** Approve all five §3 defaults (FoodPreservation sole stored cohorts; Kitchen prepared meals only; food-type gate; host-fed storage temp from `room_storage_bay`; no new ledger / weather copy / save merge).

---

## 1. Premise (current evidence)

Two Core systems spoil food on separate ledgers and save sections:

| System | Path | Save section | What it spoils |
|---|---|---|---|
| `FoodPreservationSystem` | `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs` | `food_preservation` | Stored `FoodBatchCohort` rows (`FoodItemId`, `TierId`, `FreshnessPercent`) |
| `KitchenNutritionSystem` | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | `kitchen_nutrition` | Prepared `PantryItem` portions (`spoilageTimer`, `portionCount`) |

Catalog `food_preservation.json` declares per-tier `allowed_food_types`, loaded into `PreservationTierDef.allowed_food_types`, but **`AddCohort` never reads that list**. Cohorts have no temperature field. Cryogenic decay only reacts to host-fed `SetPowerStatus` / `UnpoweredDays`.

Kitchen keeps `cellarTempC`, `hasCellar`, and `hasRefrigeration`, but `GetSpoilageDays` uses only the booleans (14 / 5 / 2 days) and **ignores the numeric temperature**. Host production callers for `SetCellar` / `SetRefrigeration` / `ServeMeal` are absent (panel + tick exist; meal pipeline is unwired).

`ShelterThermalSystem` already owns per-room `currentTempC`, including `room_storage_bay` and `room_kitchen`. Nothing binds those values into either food system.

Item catalog (`items.json`) marks food with `type: "Food"` only. There is **no `food_type` field**. Tier `allowed_food_types` mixes item-shaped ids (`raw_meat`, `dried_rations`) with type labels (`vegetable`, `tuber`).

Plans 126–129 already treat `FoodPreservationSystem` as the curing/preservative consumer and forbid fermentation from mutating spoilage or cohorts (`docs/shelter/PLANS_126_129_OWNERSHIP_DECISIONS.md`).

---

## 2. Ownership table (signed)

| Concern | Authority | Boundary |
|---|---|---|
| Stored perishable cohorts (bulk storage, curing output, tier shelf life, power outage on cryo) | **`FoodPreservationSystem`** | Sole owner of inventory-backed stored spoilage. One cohort ledger; no second bulk freshness store. |
| Prepared meal portions (prep jobs → pantry → serve) | **`KitchenNutritionSystem`** | Owns cooked/prepared portion lifecycle only. Must not become a parallel bulk-storage spoilage authority. |
| Food-type eligibility for a preservation tier | **`food_preservation.json` `allowed_food_types`** + item→type map | Catalog remains data authority. `AddCohort` must validate; invalid type → blocked result. |
| Stable food-type input | **Item catalog / explicit map** (not a new inventory) | Resolve `FoodItemId` → food type string that matches tier lists. Prefer extending item data or a small explicit map owned with the catalog — not a shadow inventory. |
| Storage temperature input | **Host-projected from `ShelterThermalSystem` room temp** | Thermal remains thermal-owned. Host feeds a bounded °C (or equivalent band) into preservation tick; Core does not copy weather or invent a second thermal sim. |
| Kitchen cellar / refrigeration flags | **`KitchenNutritionSystem` presentation of prep storage** | Keep for prepared pantry only until a later package decides whether kitchen flags become thermal projections. Numeric `cellarTempC` must not claim room-thermal authority. |
| Power for cryogenic tiers | Existing host power projection → `SetPowerStatus` | Unchanged; power is not owned by food systems. |
| Inventory quantities | Existing `Inventory` | Cohorts and curing bills consume/add through inventory; no parallel warehouse. |
| Persistence | Existing sections `food_preservation` + `kitchen_nutrition` | Do not merge save sections in this package. Follow-on implement may add fields inside the preservation section only. |

---

## 3. Signed defaults

1. **`FoodPreservationSystem` is the sole stored-cohort / bulk-perishable authority.**  
2. **`KitchenNutritionSystem` is prepared-meal portions only** (prep → pantry → serve); it must not gain bulk cohort semantics.  
3. **Food type** comes from an item→type resolution that matches `allowed_food_types`; `AddCohort` enforces the list.  
4. **Storage temperature** is host-fed from the storage room’s `ShelterThermalSystem.currentTempC` (default room: `room_storage_bay`); no weather copy into Core; no parallel thermal ledger.  
5. **Non-goals for the implement package:** new food ledger, new inventory store, merging the two save sections, weather-as-storage-temp, UI theater without Core contract, or rewriting kitchen meal serving as part of the seam seal.

---

## 4. Contract sketch (implement package only after sign-off)

These are design targets for a later claimed implement package — **not authorized by this map alone**.

### 4.1 Food-type gate on `AddCohort`

- Resolve `foodItemId` → food type (map or catalog field).  
- Load tier; if `allowed_food_types` is non-empty and type not listed → `ActionResult.Blocked`.  
- Empty list may mean “unrestricted” or “deny-all”; implement package must pick one and pin it in tests (recommend: empty = unrestricted for backward compatibility with current free-form tests).

### 4.2 Temperature seam into preservation tick

- Host, once per day (alongside existing power status), projects storage-room °C into Core via a narrow setter (e.g. `SetStorageTemperatureC(float)` or tick argument).  
- Decay uses tier shelf life modulated by temperature bands (exact curve deferred to implement package; must stay deterministic).  
- Cryogenic outage path remains power-owned; temperature does not replace power.

### 4.3 Kitchen boundary

- Leave kitchen pantry spoilage as prepared-portion decay.  
- Do not route bulk storage spoilage through kitchen.  
- Optional later package: drive `hasCellar` / `hasRefrigeration` / `cellarTempC` from thermal/room facts instead of free-floating flags — out of scope until signed.

---

## 5. Exact paths (this map package)

| Path | Role |
|---|---|
| `docs/shelter/PLAN_196_FOOD_SPOILAGE_AUTHORITY_MAP.md` | This contract |
| `KNOWN_DEBT.md` | `DEBT-196-FOOD-SPOILAGE-BOUNDARY` |
| `INTEGRATION_PLANS.md` | Active batch row |
| `WORKTREE_OWNERSHIP.md` | `claim-debt-196-food-boundary-2026-09-12` |

**Read-only evidence (not claimed for edit in this package):**

- `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`
- `Assets/Ashfall.Core/Shelter/FoodPreservationCatalog.cs`
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs`
- `Assets/Ashfall.Core/ShelterThermalSystem.cs`
- `Assets/StreamingAssets/Data/food_preservation.json`
- `Assets/StreamingAssets/Data/items.json`
- `src/Main.Plans62_65.cs` (food preservation setup/tick/save)
- `src/Main.ShelterBatch3.cs` / `src/Host/KitchenNutritionHostSession.cs`
- `docs/shelter/PLANS_126_129_OWNERSHIP_DECISIONS.md`
- `docs/foreman/PLAN_170_199_FORENSIC_AUDIT.md`
- `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`

---

## 6. Non-goals (this package and immediate follow-on)

- Creating a third food/spoilage system or ledger.  
- Copying outdoor weather into Core as storage temperature.  
- Merging `kitchen_nutrition` and `food_preservation` save sections.  
- Wiring `ServeMeal` / kitchen UI theater as part of the boundary seal.  
- Changing fermentation / Plans 126–129 preservative bridge.  
- Inventing item IDs without catalog integrity.

---

## 7. Acceptance for this map package

- [x] Dual-ledger gap documented with source paths.  
- [x] Sole stored-cohort owner named.  
- [x] Prepared-meal owner named.  
- [x] Food-type input and temperature seam named.  
- [x] **User/foreman signed §3 defaults (2026-09-12).**  
- [x] Map ACCEPTED; implement claim opened as `DEBT-196-FOOD-TYPE-TEMP-SEAM`.

---

## 8. Implement package result (`DEBT-196-FOOD-TYPE-TEMP-SEAM`)

**Sealed 2026-09-12.**

| Deliverable | Evidence |
|---|---|
| Item→type map | `food_preservation.json` `food_type_by_item_id`; `FoodPreservationCatalog.ResolveFoodType` |
| `AddCohort` gate | Blocks when tier `allowed_food_types` non-empty and type missing; empty list = unrestricted |
| Storage temp | `SetStorageTemperatureC`; bands ≤5→1.5×, ≤15→1.0×, else 0.5× shelf life |
| Host projection | `TickPlans62To65` reads `room_storage_bay.currentTempC` (default 10°C if thermal absent) |
| Curing allow-list align | salt/smoke/ferment tiers include `dried_rations` (and ferment `raw_meat`) so curing outputs pass the gate |
| Kitchen | Untouched (prepared-meal only) |

**Verify:** `Plan196FoodTypeTempSeamTests` 8/8; `FoodPreservationSystemTests` 8/8; `BioFermentationEngineTests` 20/20; `dotnet build Ashfall.csproj` 0/0.

**Still out of scope:** Kitchen `ServeMeal` / cellar flag thermalization, save-section merge, weather-as-storage-temp, inventory redesign.
