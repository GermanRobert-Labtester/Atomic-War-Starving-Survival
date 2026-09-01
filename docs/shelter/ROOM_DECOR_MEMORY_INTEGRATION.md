# Room Decor & Narrative Memory Integration

## 1. Plan 12C Decor Integration
- Stable room IDs (`room_bunker_corridor`, `room_bunks`, `room_kitchen`, etc.) serve as anchors for placing shelter decor items, posters, curtains, and morale objects.

## 2. Plan 29A / W19 Room Memory Integration
- `shelter_room_identities.json` links to the canonical room IDs for discovering room history vignettes (`room_history_seen_*`) and examining pre-war fixtures (`room_fixture_*`).
- Plan 41 preserves the distinction between static catalog definitions (`shelter_rooms.json`), narrative discovery entries (`shelter_room_identities.json`), and campaign runtime assignments (`ShelterAssignmentSave`).
