# Combat & Expedition Authority Map

**Document:** `docs/combat/COMBAT_AUTHORITY_MAP.md`
**Status:** Active Canonical Architecture
**Single Source of Truth:** `Assets/Ashfall.Core/` (Engine-Agnostic C#)

---

## 1. Authority Map

| State / Rule | Authoritative Owner | Plan 10 Role |
|---|---|---|
| Lane / Stance / Action State | [`TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs) | Combat turn logic, lane positioning, move resolution |
| Penetration / Ricochet | [`BallisticsSystem.cs`](../../Assets/Ashfall.Core/Combat/BallisticsSystem.cs) | Material armor interaction, energy retention, ricochet math |
| Weapon Wear / Fouling / Jams | [`EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs) | Condition degradation per shot, fouling, jam rolls |
| Enemy Authored Parameters | `combat_catalog.json` | 10 authored archetypes (6 fauna/mutant, 4 human) |
| Doctrine / Warlord Response | [`WarlordDoctrineSystem.cs`](../../Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs) | 8 strategic doctrines, response actions, transitions |
| Faction Standing / Non-Combat Exits | [`FactionSystem.cs`](../../Assets/Ashfall.Core/FactionSystem.cs) | Surrender thresholds, bribery, morale, retreat checks |
| Inventory & Ammo Consumption | [`InventorySystem.cs`](../../Assets/Ashfall.Core/Inventory/InventorySystem.cs) | Ammo cartridge tracking, loadout validation |
| Weapon & Ammo Crafting | [`CraftingSystem.cs`](../../Assets/Ashfall.Core/Crafting/CraftingSystem.cs) | Improvised weapon construction, custom hand-loads |
| Expedition Vehicles & Garage | [`ExpeditionVehicleSystem.cs`](../../Assets/Ashfall.Core/ExpeditionVehicleSystem.cs) | 8 vehicle chassis, speed, fuel, breakdown chance |
| Deep Coast Diving & Noise | [`MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs) | 12 dive sites, oxygen budget, noise floor, search hazard |
| Save Persistence | `SaveStoreHub.cs` / `CampaignEnvelopeBuilder.cs` | Atomic envelope roundtrip for combat, garage, and maritime |
