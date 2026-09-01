# Plan 41 Shelter Room Baseline

## 1. System Inventory
- `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` (Core assignment logic, room capacity, occupancy, validation)
- `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` (Save codec and checksummed envelope)
- `src/Host/ShelterAssignmentHostSession.cs` (Godot host session)
- `Assets/Ashfall.Core/ExcavationSystem.cs` (Excavation and room blueprint unlocking)
- `Assets/StreamingAssets/Data/shelter_rooms.json` (New data authority for 22 room definitions & 12 assignment rules)
- `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs` (DTOs and loader)

## 2. Model Decision: Model A (Catalog Defines Room Types)
- `shelter_rooms.json` defines static authored room templates (`ShelterRoomDef`) and assignment rules (`ShelterAssignmentRuleDef`).
- Runtime instances and assignments are maintained by `ShelterAssignmentSystem` and persisted in `ShelterAssignmentSave`.
