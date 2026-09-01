# Plan 41 Save Compatibility & Migration Contract

## 1. Save Structure
- `ShelterAssignmentSave` maintains `saveVersion = 1` and embeds `Rooms` (`ShelterRoomSave`) and `State` (`ShelterAssignmentState`).
- Checksumming via `SaveChecksum.Compute` guarantees byte-level tamper detection.

## 2. Backward & Forward Compatibility
- Saves produced before `shelter_rooms.json` was externalized continue to load seamlessly via fallback defaults in `ShelterAssignmentHostSession.CreateDefault`.
- Restored rooms preserve their runtime survivor assignments without data loss.
- Incompatible future save versions are rejected with explicit diagnostics.
