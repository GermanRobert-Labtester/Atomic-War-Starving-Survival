# Power Grid Authority Map

> **Authority Map:** Grounded ownership boundaries between `PowerGridSystem` and downstream shelter subsystems.

---

## 1. Domain Ownership Table

| Domain / Concept | Authoritative System | Power Grid Seam | Invariant Rule |
|---|---|---|---|
| **Generation Capacity** | `PowerGridSystem` (`GenerationWatts`) | Direct property | Power grid owns generation balance; downstream does not inject arbitrary watts |
| **Battery Storage** | `PowerGridSystem` (`BatteryReserveWh`, `BatteryCapacityWh`) | Direct property | Battery stores surplus energy, discharges during deficit, and triggers brownout when depleted |
| **Fuel Reserve** | `PowerGridSystem` (`FuelUnits`) | Direct property | Fuel burns proportional to active generation (`gen * 24 * 0.001` units/day) |
| **Room Base Draw** | `power_grid.json` (`draw_watts`) | Input definition | Drawn watts are fixed authored properties, not mutable random numbers |
| **Breaker State** | `PowerGridState` (`ClosedBreakers`) | System toggle | Players may manually disconnect circuits to manage load shedding |
| **Circuit Trip State** | `PowerGridState` (`TrippedRooms`) | Overload simulation | Breakers have a 10% chance to trip if brownout exceeds 4 hours in a tick |
| **Priority Setting** | `PowerGridState` (`Priorities`) | Player control | `Disabled` (0), `Low` (1), `Standard` (2), `Critical` (3) |
| **Room Powered State** | `PowerGridSystem.IsRoomPowered(id)` | Query predicate | Returns `true` if breaker closed, untripped, and grid not in brownout |
| **Crafting Operations** | `CraftingSystem` | Gated by `IsRoomPowered("room_workshop")` | Unpowered workshop suspends crafting bench operations without deleting materials |
| **Cooking Operations** | `KitchenSystem` / Food Authority | Gated by `IsRoomPowered("room_kitchen")` | Unpowered kitchen halts hot meal prep; raw rations remain consumable |
| **Research Progress** | `ResearchSystem` | Gated by `IsRoomPowered("room_laboratory_research")` | Unpowered lab halts tech decoding; earned progress remains intact |
| **Crop Cultivation** | `GreenhouseSystem` | Gated by `IsRoomPowered("room_greenhouse")` | Unpowered greenhouse pauses grow lights and pumps; crops do not instantly vanish |
| **Water Pumping & Lift** | `WaterSystem` | Gated by `IsRoomPowered("room_water_pump")` | Unpowered pump halts wellhead extraction; reservoir remains preserved |
| **Water Purification** | `WaterSystem` | Gated by `IsRoomPowered("room_water_treatment")` | Unpowered treatment allows toxic silt ingress into the distribution loop |
| **Air Filtration** | `ShelterEnvironmentSystem` | Gated by `IsRoomPowered("room_air_filtration")` | Unpowered filtration permits external particulate and radiation ingress |
| **Radio Monitoring** | `RadioSystem` | Gated by `IsRoomPowered("room_radio_tuner")` | Unpowered tuner loses signal reception and distress beacon tracking |
| **Perimeter Detection** | Security / Defense Authority | Gated by `IsRoomPowered("room_surveillance")` | Unpowered sensor grid disables early threat warnings |
| **Decontamination** | `AirlockSystem` / Expeditions | Gated by `IsRoomPowered("room_airlock")` | Unpowered airlock suspends chemical decon spray; returning scouts retain fallout |
| **Quarantine Isolation** | `MedicalSystem` | Gated by `IsRoomPowered("room_ward_quarantine")` | Unpowered quarantine loses negative pressure and UV sterilization |

---

## 2. Invariant Boundaries

1. **PowerGrid never owns downstream state:** It does not own inventory, crop timers, medical afflictions, recipes, or radio transcripts.
2. **Downstream never simulates electricity:** Systems query `IsRoomPowered(roomId)` or respond to `OnPowerChanged`.
3. **Idempotent evaluation:** Repeated queries to `IsRoomPowered` do not apply recurring penalties.
