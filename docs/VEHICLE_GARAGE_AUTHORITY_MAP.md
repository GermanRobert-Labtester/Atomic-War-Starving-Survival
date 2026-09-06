# Plan 50 — Expedition Overland Vehicle Customization & Maintenance Garage
## Authority & Domain Mapping

**Catalog:** `Assets/StreamingAssets/Data/vehicle_modifications.json` (`schema_version: 1`)
**Core System:** `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`
**Save Store:** `src/Host/VehicleGarageSaveStore.cs` (`vehicle_garage_save.json`, section `vehicle_garage`)
**UI Surface:** `GarageDetailPanel`

### 1. Domain Ownership
- `ExpeditionVehicleSystem`: Owns vehicle acquisition, baseline definitions (`vehicles.json`), active travel fuel, and basic mobility.
- `VehicleGarageSystem`: Owns installed modification slots, modification catalog, component wear (`chassis_stress_permille`, `engine_fouling_permille`, `transmission_wear_permille`), maintenance services, and stranded recovery operations.
- `Inventory`: Authoritative shelter inventory where mechanical parts, scrap, and tools are atomically deducted for modification installs and maintenance jobs.

### 2. Modification Slots
- `cargo`: Flatbeds, auxiliary cargo racks (alters authoritative `cargoCapacity`).
- `protection`: Lead-lined cabs, armored bullbars (reduces crew radiation up to 40%, protects against impact).
- `mobility`: Cleats, reinforced suspensions, track modules (improves traction, reduces terrain breakdown).
- `engine`: Superchargers, fuel blending modules (improves speed multiplier, modifies fuel efficiency).
- `utility`: Winch kits, field repair lockers, stretcher mounts.

### 3. Recovery Operations
When a vehicle reaches critical breakdown or immobilization, it enters `Immobilized` state. A recovery mission is registered through the expedition layer requiring another vehicle or dispatch team rather than instant teleportation.
