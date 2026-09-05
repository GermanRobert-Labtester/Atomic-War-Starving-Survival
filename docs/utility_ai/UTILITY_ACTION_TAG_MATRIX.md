# Utility Action Tag Matrix

> **Tag Contract:** Interaction matrix between authored action tags and survivor trait vetoes (`UtilityTags` & `UtilityActionScorer.IsForbiddenByTraits`).

---

## 1. Trait Veto Matrix

| Tag Name | Constant | Trait Gate / Veto | Consequence |
|---|---|---|---|
| `loud_labor` | `UtilityTags.TagLoudLabor` | `coward` | Survivor with `coward` trait refuses action (score = 0) |
| `menial_labor` | `UtilityTags.TagMenialLabor` | `god_complex` | Survivor with `god_complex` trait refuses action (score = 0) |
| `weapon` | `UtilityTags.TagWeapon` | `pacifist` | Survivor with `pacifist` trait refuses weapon actions (score = 0) |
| `gun` | `UtilityTags.TagGun` | `blind` | Survivor with `blind` trait refuses gun actions (score = 0) |
| `order` | `UtilityTags.TagOrder` | `ex_con` | Survivor with `ex_con` trait refuses authority/order actions (score = 0) |
| `medical_triage` | `UtilityTags.TagMedicalTriage` | `hitman`, `germaphobe` | `hitman` refuses triage; `germaphobe` refuses triage unless `context.HasHazmat` |
| `farming` | `UtilityTags.TagFarming` | `hitman` | `hitman` refuses agricultural labor (score = 0) |

---

## 2. 20-Action Tag Distribution

| Action ID | Tags Assigned | Active Trait Gates | Category |
|---|---|---|---|
| `action_weigh_goods` | `["loud_labor"]` | `coward` | Administrative |
| `action_read_contract` | `[]` | None | Administrative |
| `action_canvas_support` | `["menial_labor"]` | `god_complex` | Administrative |
| `action_run_vouch` | `[]` | None | Administrative |
| `action_audit_inventory` | `["quiet_labor"]` | None | Administrative |
| `action_file_report` | `["quiet_labor"]` | None | Administrative |
| `action_repair_equipment` | `["loud_labor", "maintenance", "crafting"]` | `coward` | Maintenance |
| `action_inspect_housing` | `["quiet_labor", "maintenance"]` | None | Maintenance |
| `action_treat_wounded` | `["medical_triage", "medical"]` | `hitman`, `germaphobe` (needs hazmat) | Medical |
| `action_seek_treatment` | `["medical"]` | None | Medical |
| `action_cook_food` | `["quiet_labor", "food"]` | None | Food |
| `action_preserve_food` | `["menial_labor", "food"]` | `god_complex` | Food |
| `action_purify_water` | `["loud_labor", "water"]` | `coward` | Water |
| `action_socialize` | `["social"]` | None | Social |
| `action_resolve_conflict` | `["order", "social"]` | `ex_con` | Social |
| `action_train_skill` | `["quiet_labor", "training"]` | None | Training |
| `action_teach_skill` | `["social", "training"]` | None | Training |
| `action_stand_watch` | `["weapon", "security"]` | `pacifist` | Security |
| `action_conduct_research` | `["quiet_labor", "research"]` | None | Research |
| `action_rest` | `["rest"]` | None | Rest |
