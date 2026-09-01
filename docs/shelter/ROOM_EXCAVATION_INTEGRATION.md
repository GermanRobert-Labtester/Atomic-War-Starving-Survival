# Room Excavation Integration Lifecycle

## 1. Lifecycle Sequence
1. **Catalog Definition**: Room types and recipes defined in `Assets/StreamingAssets/Data/shelter_rooms.json`.
2. **Excavation Discovery**: Unlocking a site in `Assets/StreamingAssets/Data/excavation_sites.json` associates a `roomBlueprintId` (e.g. `room_greenhouse_shelter`, `room_laboratory_research`).
3. **Excavation Progress**: Workers assigned to the site via `ExcavationSystem.AssignWorkers`.
4. **Completion & Room Creation**: Upon reaching required progress, the blueprint unlocks and instantiates the `ShelterRoom` into `ShelterAssignmentSystem`.
5. **Staffing & Downstream Output**: Survivors are assigned to the new room, triggering relevant `ShelterAssignmentRuleDef` bonuses for shelter production.
