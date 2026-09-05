# Plan 41 ↔ Plan 71 Room Reconciliation

> **Reconciliation:** Mapping between Plan 41 shelter room authority (`shelter_rooms.json`, `ShelterRoomCatalog.cs`) and Plan 71 power grid consumers (`power_grid.json`).

---

## 1. Reconciliation Matrix

| Planning Candidate ID | Landed Plan 41 Room ID | Display Name | Function Category | Power Profile ID | Reconciliation Status |
|---|---|---|---|---|---|
| `room_air_filtration` | `room_filtration` / `room_filtration_stack` | Air Filtration | FiltrationStack | `room_air_filtration` | **Matched** (canonical service ID preserved) |
| `room_clinic` | `room_clinic` | Field Clinic | MedicalBay | `room_clinic` | **Exact match** |
| `room_water_pump` | (utility wellhead service) | Water Pump | Utility Lift | `room_water_pump` | **Matched** (canonical lift service preserved) |
| `room_greenhouse` | `room_greenhouse_shelter` | Greenhouse | Greenhouse | `room_greenhouse` | **Matched** (baseline power ID preserved) |
| `room_foundry` | `room_foundry` | Silent Foundry | Heavy Industry | `room_foundry` | **Exact match** |
| `room_lighting_main` | (concourse lighting bus) | Main Lighting | Lighting Bus | `room_lighting_main` | **Matched** (canonical service ID preserved) |
| `room_workshop` | `room_workshop` | General Workshop | Workshop | `room_workshop` | **Exact match** |
| `room_kitchen` | `room_kitchen` | Galley Kitchen | Kitchen | `room_kitchen` | **Exact match** |
| `room_radio_room` | `room_radio_tuner` | Radio Communications Bay | RadioRoom | `room_radio_tuner` | **Mapped to landed ID** (`room_radio_tuner`) |
| `room_laboratory` | `room_laboratory_research` | Science & Research Lab | Laboratory | `room_laboratory_research` | **Mapped to landed ID** (`room_laboratory_research`) |
| `room_armory` | `room_armory_munitions` | Armory & Munitions Depot | Armory | `room_armory_munitions` | **Mapped to landed ID** (`room_armory_munitions`) |
| `room_storage_cold` | `room_storage_secure` | Reinforced Armored Vault | Secure Storage | `room_storage_secure` | **Mapped to landed ID** (`room_storage_secure`) |
| `room_common_area` | `room_common_mess_hall` | Communal Mess Hall | CommonArea | `room_common_mess_hall` | **Mapped to landed ID** (`room_common_mess_hall`) |
| `room_dormitory` | `room_bunks` | Standard Dormitory | Dormitory | `room_bunks` | **Mapped to landed ID** (`room_bunks`) |
| `room_water_treatment` | (water purification service) | Water Treatment Plant | Life Support | `room_water_treatment` | **Authored as canonical service** |
| `room_surveillance` | (perimeter sensor array) | Perimeter Surveillance | Security Array | `room_surveillance` | **Authored as canonical service** |
| `room_airlock` | `room_airlock` | Decontamination Airlock | Airlock | `room_airlock` | **Exact match** |
| `room_generator_room` | `room_ward_quarantine` | Isolation Quarantine Bay | MedicalBay | `room_ward_quarantine` | **Replaced** (0W generator rejected; quarantine mapped) |

---

## 2. Rationale for ID Mappings

1. **`room_radio_tuner`:** Plan 41 authoritatively registered `room_radio_tuner` in `shelter_rooms.json`, `ShelterRoomCatalog.cs`, and `StartingLevelSystem`. Using `room_radio_room` would create a phantom alias.
2. **`room_laboratory_research`:** Canonical ID established in Plan 41.
3. **`room_armory_munitions`:** Established in Plan 41 for weapons storage and ammo pressing.
4. **`room_common_mess_hall` & `room_bunks`:** Match spatial and assignment rosters.
5. **`room_storage_secure`:** High-tier powered vault with environmental and security systems.
6. **`room_ward_quarantine`:** Negative-pressure quarantine bay providing critical medical containment.
