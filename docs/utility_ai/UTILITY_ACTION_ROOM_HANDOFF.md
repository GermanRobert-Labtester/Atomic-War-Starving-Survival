# Utility Action Room Handoff

> **Room Seams:** Integration between 5 room-linked utility actions and Plan 41 shelter rooms (`shelter_rooms.json`, `ShelterRoomCatalog.cs`).

---

## 1. Five Primary Room-Linked Actions

1. **`action_cook_food` → `room_kitchen` (Galley Kitchen)**
   - *Room Authority:* `ShelterRoomCatalog.cs` defines `room_kitchen` with capacity 2, function `Kitchen`.
   - *Operational Seam:* Action checks `IsRoomPowered("room_kitchen")` (Plan 71 power grid) and that kitchen condition > 0.
   - *Behavior When Room Disabled/Unpowered:* Action becomes ineligible; raw score drops to 0.

2. **`action_repair_equipment` → `room_workshop` (General Workshop)**
   - *Room Authority:* `room_workshop`, capacity 2, required skill `skill_rough_repairs`.
   - *Operational Seam:* Checks `IsRoomPowered("room_workshop")` and tool workstation availability.
   - *Behavior When Room Disabled/Unpowered:* Workbench lathe and vices cannot run; action is suppressed.

3. **`action_conduct_research` → `room_laboratory_research` (Science & Research Lab)**
   - *Room Authority:* `room_laboratory_research`, capacity 2, function `Laboratory`.
   - *Operational Seam:* Checks `IsRoomPowered("room_laboratory_research")` and tech terminal availability.
   - *Behavior When Room Disabled/Unpowered:* Research terminals offline (`fx_laboratory_offline`); action is suppressed.

4. **`action_stand_watch` → `room_airlock` (Decontamination Airlock & Sentry Hatch)**
   - *Room Authority:* `room_airlock`, capacity 2, function `Airlock`.
   - *Operational Seam:* Checks airlock presence and security post availability.
   - *Behavior When Room Disabled/Unpowered:* Sentry monitors switch to manual peephole watch.

5. **`action_purify_water` → `room_water_treatment` (Water Treatment Plant)**
   - *Room Authority:* Canonical water treatment utility facility.
   - *Operational Seam:* Checks `IsRoomPowered("room_water_treatment")` and raw water supply.
   - *Behavior When Room Disabled/Unpowered:* Electric pumps unpowered (`fx_water_contamination`); action is suppressed.
