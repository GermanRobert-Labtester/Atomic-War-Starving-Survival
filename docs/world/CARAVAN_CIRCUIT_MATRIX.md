# Caravan Circuit Matrix

**Authority Catalog:** `Assets/StreamingAssets/Data/caravans.json`
**System Coordinator:** `Assets/Ashfall.Core/TravelingCaravanSystem.cs` / `Assets/Ashfall.Core/Economy/CaravanCatalogLoader.cs`

---

## 1. Caravan Circuit Definitions

| Caravan ID | Name | Faction | Origin | Route Itinerary | Stay (Days) | Specialty Goods |
|---|---|---|---|---|---|---|
| `caravan_flotilla_salt_run` | Salt & Saline Flotilla Convoy | `faction_the_fleet` | Deep Coast | `loc_black_flotilla_outpost` → `loc_the_shallows_market` → `loc_lock_gate_four` → `loc_water_station` → `loc_holdfast` | 2 | Clean Water, Water Filters, Diesel Fuel |
| `caravan_verge_grain_convoy` | Verge Agricultural Hauler | `faction_rebuilders` | Ash Flats | `loc_grain_silo` → `loc_forward_roster_camp` → `loc_cut_merchant_caravanserai` → `loc_grange_hall` → `loc_the_allotments` | 2 | Dried Rations, Clean Water, Scrap Wood |
| `caravan_foundry_coal_iron` | Foundry Iron & Coal Column | `faction_silent_foundry` | Industrial Belt | `loc_recovery_yard` → `loc_railway_span_44_alpha` → `loc_weighbridge` → `loc_cut_abandoned_depot` → `loc_holdfast` | 2 | Scrap Metal, Mechanical Scrap, 7.62/.308 Ammo |
| `caravan_free_trader_circuit` | Scale Free-Trader Circuit | `faction_the_scale` | Settlement | `loc_cut_merchant_caravanserai` → `loc_motel_verity` → `loc_shrine_switchback_waystation` → `loc_low_background_lab` → `loc_water_station` | 3 | Medical Kits, Military Radios, Electronic Scrap |

---

## 2. Transit Schedule & Trading Invariants

1. **Deterministic Node Progression:** Caravans advance along their circular route node index upon spending their required `stay_duration_days` at their current stop.
2. **Guards & Defense:** Caravans carry 4–6 guards. In encounters with bandit ambushes or faction blockades, high defense mitigates cargo loss.
3. **Atomic Barter:** Trades execute via `CaravanAtomicTrader` using ration equivalents, ensuring inventory counts never drop below zero or corrupt save states.
