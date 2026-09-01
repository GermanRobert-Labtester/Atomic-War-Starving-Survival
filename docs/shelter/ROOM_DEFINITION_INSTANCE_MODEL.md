# Room Definition vs. Instance Model

## 1. Architectural Choice: Model A (Type-Based Catalog)
- **Authoritative Catalog (`shelter_rooms.json`)**: Contains immutable type definitions (`ShelterRoomDef`), base capacities, max upgrade ceilings, skill requirements, default workstations, and build/repair recipes.
- **Runtime Instances**: Created during campaign start or excavated through `ExcavationSystem`. Each instance is tracked by its stable `RoomId` and holds mutable occupancy state.
- **Save State (`ShelterAssignmentSave`)**: Persists runtime room instances (`ShelterRoomSave`) and survivor assignments (`ShelterAssignmentState`), maintaining checksum integrity across sessions without duplicating static catalog fields.
