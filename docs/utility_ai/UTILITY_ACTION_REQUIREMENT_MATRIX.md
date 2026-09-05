# Utility Action Requirement Matrix

> **Eligibility Requirements:** Pre-conditions, facility dependencies, and inventory gates for all 20 actions.

---

| Action ID | Room / Facility Dependency | Workstation Requirement | Minimum Resource Input | Survivor Prerequisite |
|---|---|---|---|---|
| `action_weigh_goods` | Storage Bay | Scale Bench | None | Fatigue <= 85 |
| `action_read_contract` | Any quiet area / Bunks | None | None | Fatigue <= 90 |
| `action_canvas_support` | Access Corridor / Mess Hall | None | None | Fatigue <= 80 |
| `action_run_vouch` | Airlock / Gate | Gate intercom | None | Fatigue <= 88 |
| `action_audit_inventory` | Storage Bay | Shelf racks | None | Fatigue <= 80 |
| `action_file_report` | Archive / Radio Tuner | Ledger desk | None | Fatigue <= 80 |
| `action_repair_equipment` | `room_workshop` | Workbench / Vise | Scrap Metal / Parts | Crafting Skill > 0, Fatigue <= 75 |
| `action_inspect_housing` | Corridor / Utility conduits | Structural inspection | None | Fatigue <= 80 |
| `action_treat_wounded` | `room_clinic` | Triage table | Antiseptic / Bandage | Untreated patient exists, Fatigue <= 90 |
| `action_seek_treatment` | `room_clinic` | Clinic triage bed | None | Survivor is wounded/afflicted, Fatigue <= 95 |
| `action_cook_food` | `room_kitchen` | Galley range | Raw rations / water | Edible food target unmet, Fatigue <= 85 |
| `action_preserve_food` | `room_kitchen` / Cold Store | Salting / canning station | Salt / Jars / Produce | Perishables at risk, Fatigue <= 80 |
| `action_purify_water` | `room_water_treatment` | Filter / chemical doser | Raw water / Charcoal | Clean water low, Fatigue <= 85 |
| `action_socialize` | `room_common_mess_hall` | Mess bench | None | Eligible partner available, Fatigue <= 90 |
| `action_resolve_conflict` | Common area / Concourse | None | None | Active survivor friction exists, Fatigue <= 88 |
| `action_train_skill` | Workshop / Library / Firing bay | Practice dummy / books | None | Trainable skill not maxed, Fatigue <= 75 |
| `action_teach_skill` | Workshop / Classroom | Shared workstation | None | Teacher skill > Learner skill, Fatigue <= 80 |
| `action_stand_watch` | `room_airlock` / Surveillance | Sentry camera / watch hatch | Weapon | Fatigue <= 85 |
| `action_conduct_research` | `room_laboratory_research` | Analysis terminal | None | Active uncompleted tech node, Fatigue <= 75 |
| `action_rest` | `room_bunks` | Dormitory bunk | None | None (fatigueGate = 0; fatigue drives urgency) |
