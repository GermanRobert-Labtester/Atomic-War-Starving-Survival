# Power Assignment & Output Integration

> **Production Seams:** Wiring between Plan 41 room assignments / production loops and power grid states.

---

## 1. Verified Production Seams (4 Primary Assignment Paths)

### 1. General Workshop Loop (`room_workshop`)
- **Prerequisites:** Survivor assigned with `skill_rough_repairs` / `skill_workshop_sense` + scrap materials.
- **Power Seam:** `PowerGridSystem.IsRoomPowered("room_workshop")`.
- **Powered Behavior:** Workstation is active; fabrication bench and machine tools perform maintenance and crafting tasks.
- **Unpowered Behavior:** Station tools are unpowered; work progress pauses cleanly. No queued items or crafting materials are destroyed.
- **Restoration Behavior:** Work resumes immediately at preserved completion percentage.

### 2. Subterranean Greenhouse Loop (`room_greenhouse`)
- **Prerequisites:** Survivor assigned with `skill_mycology` + seed stocks + water.
- **Power Seam:** `PowerGridSystem.IsRoomPowered("room_greenhouse")`.
- **Powered Behavior:** Sodium grow lamps illuminate crops on timer cycles; irrigation pumps deliver filtered water.
- **Unpowered Behavior:** Grow lights turn off; crop maturation is suspended. Crops do not instantly perish.
- **Restoration Behavior:** Grow cycles resume as lamps re-strike.

### 3. Science & Research Lab Loop (`room_laboratory_research`)
- **Prerequisites:** Survivor assigned with `skill_cold_analysis` + tech archives.
- **Power Seam:** `PowerGridSystem.IsRoomPowered("room_laboratory_research")`.
- **Powered Behavior:** Centrifuges, optical readers, and decoding terminals progress active research node.
- **Unpowered Behavior:** Electronic terminals go dark; active research progress pauses. Completed research tiers remain unlocked.
- **Restoration Behavior:** Terminals reboot; decoding picks up from saved progress.

### 4. Galley Kitchen Loop (`room_kitchen`)
- **Prerequisites:** Survivor assigned with `skill_ration_stretcher` + food supplies.
- **Power Seam:** `PowerGridSystem.IsRoomPowered("room_kitchen")`.
- **Powered Behavior:** Electric stove ranges and exhaust flues prepare hot meals with caloric bonuses and morale lifts.
- **Unpowered Behavior:** Stoves cool; hot meal preparation halts. Basic cold ration distribution continues unhindered.
- **Restoration Behavior:** Cooking resumes with zero lost food inventory.

---

## 2. Gating Protocol

- Downstream systems query `PowerGridSystem.IsRoomPowered(roomId)` at the tick boundary or interaction check.
- Power grid does not directly credit items, research points, or calories.
- Power state functions purely as an operational prerequisite.
