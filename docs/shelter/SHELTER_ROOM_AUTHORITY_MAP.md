# Shelter Room Authority Map

| Concern | Primary Authority | Consumer / Integration |
|---|---|---|
| Room Definitions | `Assets/StreamingAssets/Data/shelter_rooms.json` | `ShelterRoomCatalogLoader` |
| Assignment Rules | `Assets/StreamingAssets/Data/shelter_rooms.json` | `ShelterAssignmentSystem` |
| Occupancy & Assignment | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | UI, Host Session |
| Room Unlocking / Excavation | `Assets/Ashfall.Core/ExcavationSystem.cs` | `roomBlueprintId` in `excavation_sites.json` |
| Machine & Fixture Identity | `Assets/StreamingAssets/Data/shelter_room_identities.json` | Plan 29A narrative inspections |
| Downstream Production | Food, Medical, Workshop, Research systems | Applied assignment rules & modifiers |
| Save / Load State | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSave.cs` | Campaign envelope (`shelter_assignment`) |
