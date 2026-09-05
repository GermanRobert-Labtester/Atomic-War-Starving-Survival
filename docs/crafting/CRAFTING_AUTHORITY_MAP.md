# Crafting Authority Map (Plan 55)

Derived from repository truth (`CraftingSystem.cs`, `RecipeCatalogLoader.cs`,
`CraftingHostSession.cs`, `Main.World.cs`, `ResearchSystem.cs`, loaders).
Where this map and code disagree, code wins and this map is corrected.

| State / rule | Authority | Plan 55 use |
|---|---|---|
| Recipe definition | `recipes.json` + `RecipeCatalogLoader` (strict: throws on zero-result sink pattern outside the 6-recipe legacy allowlist) | authored conversion |
| Item identity | `items.json` merged with 9 secondary item files (`greenhouse_items.json`, `foundry_items.json`, …) by `ItemCatalogLoader` | ingredient/output refs |
| Inventory quantity | `Ashfall.Core.Inventory.Inventory` (`ValidateTransaction` / `TryExecuteTransaction` bills) | consume/produce |
| Crafting eligibility | `CraftingSystem.CanCraft` — station operational, craft-result gate, ingredient bill, output capacity | validation |
| Station existence | `CraftingHostSession.SyncStations` / `RemoveStation`; shelter bridge in `Main.World.SyncCraftingStationsFromShelter` (workbench ← room_workshop machine health; stove ← room_kitchen; heater ← room_generator; water_purifier ← room_filtration) | station gate |
| Station wear | `CraftingStation.Degrade/Repair` (5f per craft, inside `CraftingSystem`) | untouched |
| Skill state | `SkillProgressionSystem` (survivor traits); crafting integration = crafter cost/time multipliers only | efficiency only; no recipe gating |
| Research state | `ResearchSystem` + `research_knowledge.json` (56 nodes); completion awards `breakthroughItem` | breakthrough items are rare crafting inputs — the live integration surface |
| Recipe discovery | **None** — catalog membership is discovery | no unlock state authored |
| Cooking outcome | Food items (`ItemType.Food` + `hungerRestore`) consumed by the needs authority | food outputs |
| Water state | Water recipes produce `clean_water` / components; purification infrastructure owns throughput | water outputs via existing items only |
| Medicine effects | Pharma lab (`pharma_recipes.json`, 26 recipes, `pharma_bench`) + `medical_texts.json` treatments (consume `bandage`, `splint`, `antiseptic`, `antibiotics`, `medical_kit`, …) | Plan 55 medicine recipes produce only items with existing treatment consumers |
| Weapon ammo identity | `combat_catalog.json` calibers (`ammo_9x19`, `ammo_556`, `ammo_762`, `ammo_12g`, `ammo_308`, `ammo_357`, `ammo_22lr`, `ammo_762x54r`, `ammo_improvised_*`) | reloaded ammo uses live calibers only |
| Weapon condition | `EquipmentConditionSystem` / `WeaponEquipmentBridge` | untouched — no repair bypass |
| Shelter upgrades | `ShelterRoomCatalog` build costs, `SkyLayerArmorCatalog` material costs, `ShelterWorkshopSystem` overhaul bills — all consume raw items directly | Plan 55 adds no double-charged components |
| Vehicle condition | `ExpeditionVehicleSystem.Repair(vehicleId, amount)` — takes a float; the host consumes **no** items | **no vehicle-component recipes** (no consumer); documented substitution |
| Trade values | `tradeValue` on item definitions | balance audit input |
| Craft-job persistence | `CraftingSystemSave.ActiveCrafts` via `CraftingSaveStore` | round-trip preserved |
| Catalog loading | `RecipeCatalogLoader` via `CraftingHostSession.Create(dataDir,…)` — dev/headless/exported share one path | exported parity |
