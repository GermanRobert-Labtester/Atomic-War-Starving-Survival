# Plan 29 — Room History Matrix (Task 29A §29A.7–29A.9)

> Generated from `Assets/StreamingAssets/Data/shelter_room_identities.json` and
> cross-checked against `BUNKER_ORIGIN_CONTINUITY.md`.
> **20 of 20 vignettes authored, all reachable through a wired or milestone host trigger.**

## 1. Vignette roster

| Vignette id | Room | Era | Unlock | Words | Trigger seam |
|---|---|---|---:|---|---|
| `room_history_the_first_filter_change` | `room_filtration` | pre-war maintenance | `inspect_room` | 197 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_a_frame_stayed` | `room_bunks` | early shelter occupancy | `inspect_room` | 198 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_four_pale_rectangles` | `room_bunker_corridor` | early shelter occupancy | `day_milestone` (day 8) | 204 | day owner `shelter_room_history` → `Main.TickShelterRoomHistoryMilestones` |
| `room_history_the_count_came_short` | `room_kitchen` | crisis conversion | `inspect_room` | 199 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_the_basin_that_was_a_mixing_bowl` | `room_clinic` | crisis conversion | `day_milestone` (day 20) | 211 | day owner `shelter_room_history` → `Main.TickShelterRoomHistoryMilestones` |
| `room_history_a_chair_from_the_row` | `room_airlock` | crisis conversion | `inspect_room` | 203 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_the_discrepancy` | `room_storage_bay` | crisis conversion | `inspect_room` | 203 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_the_second_blower` | `room_filtration` | crisis conversion | `repair_performed` | 210 | repair action → `Main.HandleShelterRoomRepairPerformed` |
| `room_history_bench_markings` | `room_bunker_corridor` | early occupancy | `inspect_room` | 125 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_shelf_unit_d` | `room_storage_bay` | crisis conversion | `repair_performed` | 118 | repair action → `Main.HandleShelterRoomRepairPerformed` |
| `room_history_bunk_three` | `room_bunks` | early occupancy | `day_milestone` (day 5) | 126 | day owner `shelter_room_history` → `Main.TickShelterRoomHistoryMilestones` |
| `room_history_can_opener_dent` | `room_kitchen` | current campaign | `inspect_room` | 128 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_suture_pack` | `room_clinic` | pre-war maintenance | `inspect_room` | 109 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_lathe_true` | `room_workshop` | pre-war maintenance | `inspect_room` | 115 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_tuner_warm` | `room_radio_tuner` | early occupancy | `inspect_room` | 107 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_cupola_breath` | `room_foundry` | crisis conversion | `repair_performed` | 101 | repair action → `Main.HandleShelterRoomRepairPerformed` |
| `room_history_soil_window` | `room_greenhouse` | current campaign | `day_milestone` (day 12) | 116 | day owner `shelter_room_history` → `Main.TickShelterRoomHistoryMilestones` |
| `room_history_generator_footings` | `room_main` | pre-war maintenance | `inspect_room` | 108 | shelter room click → `Main.HandleShelterRoomSelected` |
| `room_history_boiler_jacket` | `room_main` | early occupancy | `repair_performed` | 107 | repair action → `Main.HandleShelterRoomRepairPerformed` |
| `room_history_filter_cartridge` | `room_filtration` | current campaign | `inspect_room` | 120 | shelter room click → `Main.HandleShelterRoomSelected` |

## 2. Room coverage

- `room_bunker_corridor` (Central Corridor): 2 vignettes, 4 fixtures
- `room_storage_bay` (Storage Bay): 2 vignettes, 4 fixtures
- `room_bunks` (Bunk Living): 2 vignettes, 4 fixtures
- `room_kitchen` (Galley Kitchen): 2 vignettes, 4 fixtures
- `room_clinic` (Medical Ward): 2 vignettes, 4 fixtures
- `room_workshop` (Workshop): 1 vignettes, 3 fixtures
- `room_filtration` (Filtration Stack): 3 vignettes, 6 fixtures
- `room_airlock` (Airlock Hatch): 1 vignettes, 5 fixtures
- `room_radio_tuner` (Tuner Station): 1 vignettes, 3 fixtures
- `room_foundry` (Silent Foundry): 1 vignettes, 4 fixtures
- `room_greenhouse` (Greenhouse): 1 vignettes, 3 fixtures
- `room_main` (Main Vault): 2 vignettes, 5 fixtures
