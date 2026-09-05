# Power Room Coverage Matrix

> **Coverage:** Full matrix of all 18 power grid consumer entries in `power_grid.json`.

---

| # | Room ID | Display Name | Draw (W) | Default Priority | Failure Effect ID | Downstream Consumer / Hook |
|---|---|---|---|---|---|---|
| 1 | `room_air_filtration` | Air Filtration | 180 | critical | `fx_filtration_off` | `ShelterEnvironmentSystem` (Air Quality / Radiation Ingress) |
| 2 | `room_clinic` | Clinic | 120 | critical | `fx_clinic_off` | `MedicalSystem` / Triage Bed Availability |
| 3 | `room_water_pump` | Water Pump | 100 | critical | `fx_water_pressure_drop` | Wellhead Lift / Sump Pumping |
| 4 | `room_greenhouse` | Greenhouse | 160 | standard | `fx_grow_lights_off` | `GreenhouseSystem` (Hydroponic Trays & Grow Lamps) |
| 5 | `room_foundry` | Silent Foundry | 220 | low | `fx_foundry_standstill` | Cupola & Casting Floor Operations |
| 6 | `room_lighting_main` | Main Lighting | 80 | low | `fx_lighting_dim` | Concourse Lighting & Morale Baseline |
| 7 | `room_workshop` | General Workshop | 200 | standard | `fx_workshop_unpowered` | `CraftingSystem` (Bench Tools, Lathe, Vises) |
| 8 | `room_kitchen` | Galley Kitchen | 120 | standard | `fx_kitchen_cold` | Canteen Ration Prep & Cook Range |
| 9 | `room_radio_tuner` | Radio Communications Bay | 100 | standard | `fx_radio_static` | `RadioHostSession` / Signal Transceiver Lead |
| 10 | `room_laboratory_research` | Science & Research Lab | 300 | standard | `fx_laboratory_offline` | `ResearchSystem` (Analysis Centrifuge & Archive Decoding) |
| 11 | `room_armory_munitions` | Armory & Munitions Depot | 50 | standard | `fx_armory_lockdown` | Weapon Rack Electronic Blast Locks & Reloading Press |
| 12 | `room_storage_secure` | Reinforced Armored Vault | 80 | standard | `fx_cold_storage_spoilage` | Refrigerated Pharmaceutical & Ration Chiller |
| 13 | `room_common_mess_hall` | Communal Mess Hall | 40 | low | `fx_mess_hall_dark` | Social Gathering & Morale Support |
| 14 | `room_bunks` | Standard Dormitory | 30 | low | `fx_dormitory_cold` | Bunk Ventilation & Rest Quality |
| 15 | `room_water_treatment` | Water Treatment Plant | 180 | critical | `fx_water_contamination` | Reverse Osmosis & UV Decontamination |
| 16 | `room_surveillance` | Perimeter Surveillance | 90 | standard | `fx_surveillance_blind` | Surface Sensor Array & Low-Light Cameras |
| 17 | `room_airlock` | Decontamination Airlock | 110 | critical | `fx_airlock_decon_disabled` | Chemical Decon Sprayers & Outer Blast Hatch Winch |
| 18 | `room_ward_quarantine` | Isolation Quarantine Bay | 70 | critical | `fx_quarantine_breach` | Negative-Pressure Vent Fan & UV Sterilization |
