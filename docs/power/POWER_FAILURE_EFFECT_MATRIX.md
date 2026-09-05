# Power Failure Effect Matrix

> **Failure Effects:** Comprehensive catalog of failure effects, downstream consumers, and consequences for all 18 rooms.

---

| Room ID | Failure Effect ID | Effect Type | Owning Authority | Primary Consequence When Unpowered | Recovery Behavior When Restored |
|---|---|---|---|---|---|
| `room_air_filtration` | `fx_filtration_off` | Level Gate | `ShelterEnvironmentSystem` | Blower motor halts; particulate & radioactive fallout ingress increases | Ventilation resumes; scrubbers re-engage |
| `room_clinic` | `fx_clinic_off` | Level Gate | `MedicalSystem` | Powered medical equipment & diagnostic monitors offline; surgical treatments paused | Diagnostic lamps & surgical benches re-engage |
| `room_water_pump` | `fx_water_pressure_drop` | Level Gate | `WaterSystem` | Wellhead lift pump stops; water loop pressure drops to zero | Wellhead pumping resumes |
| `room_greenhouse` | `fx_grow_lights_off` | Level Gate | `GreenhouseSystem` | Sodium grow lamps & irrigation pumps unpowered; growth paused | Lamps strike; irrigation resumes |
| `room_foundry` | `fx_foundry_standstill` | Level Gate | Foundry Subsystem | Cupola blower & crane hoists freeze; metal casting paused | Tuyere blowers restart |
| `room_lighting_main` | `fx_lighting_dim` | Level Gate | Presentation / Morale | Arterial concourse goes dark; emergency chemical glowstrips active | Corridors re-illuminate |
| `room_workshop` | `fx_workshop_unpowered` | Level Gate | `CraftingSystem` | Lathe, drill presses, and fabrication stations unpowered; work paused | Powered tools resume immediately without material loss |
| `room_kitchen` | `fx_kitchen_cold` | Level Gate | Kitchen / Food | Electric stoves & range exhaust unpowered; hot meal cooking paused | Stoves reignite; cooked ration prep resumes |
| `room_radio_tuner` | `fx_radio_static` | Level Gate | `RadioHostSession` | Transceiver dead; frequency scanning and distress beacon decoding offline | Antenna preamp active; signal decoding resumes |
| `room_laboratory_research` | `fx_laboratory_offline` | Level Gate | `ResearchSystem` | Centrifuges & archive readers shut down; tech progression paused | Research devices power on; existing decoding preserved |
| `room_armory_munitions` | `fx_armory_lockdown` | Level Gate | Security / Armory | Electronic blast locks default to emergency secure; reloading press offline | Locks clear; armory maintenance resumes |
| `room_storage_secure` | `fx_cold_storage_spoilage` | Delayed Timer | Inventory / Storage | Chiller compressors fail; ambient temperature rises; spoilage grace period starts | Compressors restart; grace timer resets |
| `room_common_mess_hall` | `fx_mess_hall_dark` | Level Gate | Social / Morale | Mess hall lighting offline; evening gatherings disrupted | Overhead lights restore |
| `room_bunks` | `fx_dormitory_cold` | Level Gate | Dormitory / Rest | Bunk ventilation & lighting offline; rest recovery quality reduced | Bunk lighting & air circulation restore |
| `room_water_treatment` | `fx_water_contamination` | Level Gate | `WaterSystem` | Reverse osmosis & UV purifiers halt; untreated water bypasses filters | Purification restores clean water delivery |
| `room_surveillance` | `fx_surveillance_blind` | Level Gate | Perimeter Security | Exterior camera monitors & motion sensors go black; detection bonus lost | Sensor telemetry reconnects |
| `room_airlock` | `fx_airlock_decon_disabled` | Level Gate | `AirlockSystem` | Chemical decon showers & blast hatch winches unpowered; outer decon disabled | Decon spray pumps & hatch winches re-energize |
| `room_ward_quarantine` | `fx_quarantine_breach` | Level Gate | `MedicalSystem` | Negative-pressure air exhaust & UV sterilization shut down | Containment exhaust & UV cycle re-engage |
