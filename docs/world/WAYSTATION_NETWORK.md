# Waystation Infrastructure Network

**Authority Catalog:** `Assets/StreamingAssets/Data/waystations.json`
**Core Coordinator:** `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs`

---

## 1. Network Overview

The waystation network provides forward shelter, atmospheric filter replenishment, resupply, and sentry staging across the six macro-regions of the ASHFALL wasteland.

```
[ High Scarp: The Switchback Waystation ]
                  ▲
                  │
[ Dead Suburbs: Verity Motel ] ◄──► [ The Holdfast ] ◄──► [ Industrial Belt: Waystation A (The Cut) ]
                  │                       │                           │
                  ▼                       ▼                           ▼
[ Deep Coast: Lock Gate Four ] ◄──────────┴──────────► [ Ash Flats: Verge Silo Waystation ]
                                                       [ Industrial Belt: Span 44 Rail ]
```

---

## 2. Waystation Profiles

| Waystation ID | Name | Node ID | Region | Keeper | Specialty | Services |
|---|---|---|---|---|---|---|
| `waystation_alpha_cut` | Waystation A — The Cut | `loc_cut_abandoned_depot` | `industrial_belt` | Warden Kessel | Industrial Tools & Fasteners | trade, staging, rest, filter_recharge |
| `waystation_switchback` | The Switchback Waystation | `loc_shrine_switchback_waystation` | `high_scarp` | Deacon Vane | Cold-Weather Fuel & Thermal Liners | trade, staging, rest, blessing |
| `waystation_span44` | Span 44 Rail Waystation | `loc_railway_span_44_alpha` | `industrial_belt` | Foreman Taggart | Railway Iron & Pneumatic Parts | trade, staging, repair |
| `waystation_verity` | Verity Motel Staging Post | `loc_motel_verity` | `dead_suburbs` | Mistress Corvo | Medical Supplies & Clean Water | trade, staging, rest, intelligence |
| `waystation_coast_lock` | Lock Gate Four Maritime Staging | `loc_lock_gate_four` | `deep_coast` | Diver Renn | Saline Iodine & Marine Salvage | trade, staging, saline_wash |
| `waystation_grain_verge` | Verge Silo Waystation | `loc_grain_silo` | `ash_flats` | Weigher Orlov | Preserved Grain & Honey Comb | trade, staging, grain_exchange |

---

## 3. Runtime Lifecycle & Maintenance Rules

1. **Daily Filter Decay:** Filters degrade by 1.5% daily under ambient particulate exposure. If a filter drops to 0%, structural condition decays rapidly (-3.0%/day).
2. **Maintenance Actions:** Delivering clean water and filter membrane executes `RepairFilter()`, returning filter health to 100% and restoring 15% structural integrity.
3. **Survivor Watch Staffing:** Assigning survivors to waystation watch duties stabilizes structural condition (+0.5%/day) and secures transit safety along adjacent routes.
4. **Periodic Stock Replenishment:** Waystation inventory refreshes deterministically every 7 days from the catalog specialty lists.
