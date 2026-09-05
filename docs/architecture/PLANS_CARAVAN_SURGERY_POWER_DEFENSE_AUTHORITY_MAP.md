# ASHFALL Authority Reconnaissance Map
## Long-Range Caravans, Advanced Surgery, Power Sub-Grids & Perimeter Defense (Plans 85–88)

**Document:** `docs/architecture/PLANS_CARAVAN_SURGERY_POWER_DEFENSE_AUTHORITY_MAP.md`
**Scope:** Pre-implementation architectural audit across Economy, Medical, Power, Defense, and Persistence domains.

---

## 1. Economy & Caravan Trade Authority

| Dimension | Repository Authority / Finding |
|---|---|
| **Existing Class** | `DynamicEconomySystem`, `MarketSystem`, `CaravanAtomicTrader`, `Inventory`, `InventoryTransaction` |
| **Owning Namespace** | `Ashfall.Core.Economy`, `Ashfall.Core.Inventory` |
| **Save Owner** | `SaveStoreHub` via `SaveStore<T>`, registered under `caravans` / `market` in `SaveSectionRegistry` |
| **Event Owner** | Direct typed C# events (`Action<CaravanCommittedTrade>`, etc.) |
| **Host Bridge** | `TradeScreenPresenter`, `CaravanBarterLedgerPanel` |
| **Intended Integration API** | `Inventory.TryExecuteTransaction(InventoryTransaction)`, `DynamicEconomySystem.GetMultiplier(...)`, `FactionStanceEngine` |
| **Authority Pattern** | **Composes**: `CaravanTradeNetworkSystem` composes `Inventory` for atomic multi-resource barter and queries `DynamicEconomySystem` / `HardcoreEconomyTuning` for baseline pricing without modifying core inventory internals. |

### Domain Details:
- **Canonical item valuation**: Evaluated via `ItemCatalog.Find(id).tradeValue` modified by regional crisis factors and supply/demand multipliers.
- **Atomic barter**: `InventoryTransaction` executes atomic deductions and additions with full rollback on validation failure.
- **Faction IDs**: `faction_the_compact`, `faction_the_scale`, `faction_black_flotilla`.
- **Favored Barter Status**: Unlocks after 5 profitable transactions with a given faction, granting a 15% tariff reduction.

---

## 2. Medical & Advanced Surgery Authority

| Dimension | Repository Authority / Finding |
|---|---|
| **Existing Class** | `MedicalWardSystem`, `AmputationSystem`, `NeedsSystem`, `RadiationSystem`, `SurvivorFateSystem`, `EquipmentConditionSystem` |
| **Owning Namespace** | `Ashfall.Core.Medical`, `Ashfall.Core.Survivors`, `Ashfall.Core.Equipment` |
| **Save Owner** | `MedicalSaveStore`, `SurvivorFateSystem`, `SaveStoreHub` |
| **Event Owner** | `OnWardChanged`, `OnAmputationComplete`, `OnSurvivorDied` |
| **Host Bridge** | `MedicalPanel`, `AmputationTriagePanel` |
| **Intended Integration API** | `AmputationSystem.State.survivorLimbs`, `RadiationSystem.DoseLedger`, `EquipmentConditionSystem.Register(...)` |
| **Authority Pattern** | **Composes**: `AdvancedSurgicalWardSystem` orchestrates surgical procedures, consumables, anesthesia levels, and cleanliness, delegating patient trauma clearance, limb status, and radiation dose purging to existing `AmputationSystem` and `RadiationSystem`. |

### Domain Details:
- **Limb and Amputation State**: `LimbId` (LeftArm, RightArm, LeftLeg, RightLeg), `LimbCondition` (Intact, Wounded, Infected, Gangrenous, Amputated, Prosthetic).
- **Radiation Unit Mapping**: Core `RadiationSystem` tracks doses in rads / exposure units. Cellular rad scrub purges 150 mSv (equivalent to 15 rads under the canonical 10:1 mSv:rad conversion ratio).
- **Prosthetic Registration**: Successful prosthetic fittings update `LimbState.condition = LimbCondition.Prosthetic` and register wear in `EquipmentConditionSystem`.

---

## 3. Power Distribution & Sub-Grids Authority

| Dimension | Repository Authority / Finding |
|---|---|
| **Existing Class** | `PowerGridSystem`, `PowerGridState`, `PowerGridRoom` |
| **Owning Namespace** | `Ashfall.Core.Shelter` |
| **Save Owner** | `PowerGridSave` via `SaveStoreHub.FromCodec` under `power_grid` |
| **Event Owner** | `PowerGridSystem.OnPowerChanged`, `OnTickSummary` |
| **Host Bridge** | `PowerGridHostSession`, `StatusPanel` |
| **Intended Integration API** | `PowerGridSystem.GenerationWatts`, `TotalDrawWatts`, `NetWatts`, room load allocations |
| **Authority Pattern** | **Extends & Composes**: `PowerDistributionSubgridSystem` takes `PowerGridSystem` as the primary generator bus input, then models subterranean distribution nodes, transformer thermal stress, capacitor burst buffering, and branch circuit breakers. |

### Domain Details:
- **Canonical Units**: Watts (W) and Watt-hours (Wh).
- **Industrial Capacitor Bank**: Provides up to 2,000 W momentary burst buffering to prevent breaker trips during heavy machinery spins (e.g. lathe, deep pumps).
- **Thermal Stress & Blowout**: Sustained branch loads >90% increase transformer core temperature, degrading dielectric transformer oil and triggering arc-flash events if unmitigated.
- **Maintenance**: Restores transformer health and oil condition via atomic consumption of `machine_oil` ×1 and `electrical_wire` ×2.

---

## 4. Perimeter Defense & Surface Fortifications Authority

| Dimension | Repository Authority / Finding |
|---|---|
| **Existing Class** | `TacticalCombatSystem`, `EquipmentConditionSystem`, `AirlockSecuritySystem`, `Inventory` |
| **Owning Namespace** | `Ashfall.Core.Combat`, `Ashfall.Core.Equipment`, `Ashfall.Core`, `Ashfall.Core.Inventory` |
| **Save Owner** | `TacticalCombatSystem.Persistence`, `AirlockSecurityState` |
| **Event Owner** | `AirlockSecuritySystem.OnIncidentResolved`, combat events |
| **Host Bridge** | `AirlockSecurityPanel`, `CombatHudOverlay` |
| **Intended Integration API** | `Inventory.TryExecuteTransaction`, `EquipmentConditionSystem.DegradeCondition`, `PowerDistributionSubgridSystem.IsNodePowered` |
| **Authority Pattern** | **Composes**: `PerimeterDefenseSystem` manages surface emplacements, ammo reserves, barrel wear, and tripwire flares. It intercepts surface raider probes before they reach the shelter airlock, passing unresolved breaches to `TacticalCombatSystem` and `AirlockSecuritySystem`. |

### Domain Details:
- **Construction & Ammo**: Barbed wire, revetments, and automated sentry turrets (9mm, 5.56mm) consume materials atomically from `Inventory`.
- **Barrel Wear**: Sentry barrels degrade 2% per 50 rounds fired, increasing jam probability when condition drops below 50%.
- **Tripwire Flares**: Consumes flare items to grant +30% targeting accuracy at night and eliminate stealth infiltration attempts.
- **Power Integration**: Automated turrets require delivered power from the `PowerDistributionSubgridSystem` to fire; unpowered nodes freeze automated defenses.

---

## 5. Cross-System Interaction Matrix

```text
[CaravanTradeNetworkSystem]
       │
       ├─ Supplies: anesthetic_ether, sterile_gauze, chemical_filter ──────► [AdvancedSurgicalWardSystem]
       ├─ Supplies: machine_oil, electrical_wire, copper_fuse ─────────────► [PowerDistributionSubgridSystem]
       └─ Supplies: ammo_9x19, ammo_556, scrap_metal ──────────────────────► [PerimeterDefenseSystem]

[PowerDistributionSubgridSystem]
       │
       ├─ Delivers: Operating theater lighting, autoclave, rad filter ─────► [AdvancedSurgicalWardSystem]
       └─ Delivers: Sentry turret tracking and automated firing ──────────► [PerimeterDefenseSystem]

[PerimeterDefenseSystem]
       │
       └─ Softens/repels raids; casualties requiring surgery ──────────────► [AdvancedSurgicalWardSystem]
```
