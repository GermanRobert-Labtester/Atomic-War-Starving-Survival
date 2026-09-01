# Plan 10 — Combat & Expedition Depth Baseline

**Document:** `docs/combat/PLAN10_BASELINE.md`
**Status:** Canonical Baseline
**Authority:** `Assets/StreamingAssets/Data/combat_catalog.json`, `warlord_doctrines.json`, `vehicles.json`, `dive_sites.json`

---

## 1. Baseline Scope & Inventory

Plan 10 consolidates combat bestiary, warlord doctrines, armory, ammunition, expedition vehicles, and deep-coast dive sites into an integrated, balanced depth pass.

### Baseline vs Target Summary

| System / Area | Legacy Baseline | Plan 10 Target | Live Implemented Status |
|---|---|---|---|
| **Authored Combatants** | 0 (generic) | 10 combatants (6 fauna/mutant + 4 human) | **10 combatants** (`combat_catalog.json`) |
| **Warlord Doctrines** | 4 doctrines | 8 doctrines (4 core + 4 expanded) | **8 doctrines** (`warlord_doctrines.json`) |
| **Weapons** | 5 basic firearms | 15+ weapons (improvised, military, relic) | **15 weapons** (`combat_catalog.json`) |
| **Ammunition Loads** | 5 standard calibers | 11+ ammo types/loads | **14 ammo types** (`combat_catalog.json`) |
| **Expedition Vehicles** | 3 starter vehicles | 8 vehicles (specialized logistics) | **8 vehicles** (`vehicles.json`) |
| **Deep-Coast Dive Sites** | 4 starter wrecks | 12 dive sites (tiered hazards/noise) | **12 dive sites** (`dive_sites.json`) |

---

## 2. Authoritative System Links

- **Combat System:** [`Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs)
- **Ballistics & Penetration:** [`Assets/Ashfall.Core/Combat/BallisticsSystem.cs`](../../Assets/Ashfall.Core/Combat/BallisticsSystem.cs)
- **Weapon Condition:** [`Assets/Ashfall.Core/EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs)
- **Warlord Doctrine System:** [`Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs`](../../Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs)
- **Vehicle System:** [`Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`](../../Assets/Ashfall.Core/ExpeditionVehicleSystem.cs)
- **Deep Coast Maritime System:** [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)
