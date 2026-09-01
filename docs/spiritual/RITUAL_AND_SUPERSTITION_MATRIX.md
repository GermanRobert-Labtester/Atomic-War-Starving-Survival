# Ritual & Superstition Matrix

---

## 1. Eight Emergent Optional Rituals

| ID | Title | Context Trigger | Morale Delta | Cooldown | Purpose |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `ritual_exterior_door_tap` | Two Taps on the Outer Iron | `expedition_departure` | +1.5 | 1d | Hand habit before airlock cycle |
| `ritual_crust_for_the_waste` | The Outside Crumb | `mealtime` | +1.0 | 2d | Symbolic bread crumb left for waste |
| `ritual_birthday_match_flame` | The Birthday Match | `birthday` | +3.0 | 5d | Single match burned in dark dormitory |
| `ritual_departure_plate_touch` | The Roster Plate Touch | `expedition_departure` | +2.0 | 1d | Palm on brass former-crew roster |
| `ritual_return_roll_call` | The Return Count | `return_muster` | +2.5 | 1d | Surnames spoken in order at inner door |
| `ritual_first_clean_sip_pause` | The Wellhead Pause | `water_purified` | +2.0 | 3d | 3-second silence before drinking new water |
| `ritual_empty_seat_meal_silence` | The Bereaved Spoon | `mealtime_after_loss` | +1.5 | 4d | Inverted spoon silence following a death |
| `ritual_generator_casing_knock` | The Generator Wrench Knock | `machine_maintenance` | +1.0 | 2d | 3 taps with brass spanner before startup |

---

## 2. Six Friction Superstitions

| ID | Title | Friction Flag | Operational Collision Condition |
| :--- | :--- | :--- | :--- |
| `superstition_intake_vent_nightmare` | Intake Vent Dread | `friction_vent_sleeping_dispute` | Bunk assigned within 3 paces of intake grate |
| `superstition_lucky_lower_bunk` | The Unbroken Bunk | `friction_lucky_bunk_dispute` | New dweller assigned to Bunk 4 Lower |
| `superstition_hatch_name_taboo` | Unspoken on Departure | `friction_spoken_name_blame` | Scout name spoken during airlock cycling |
| `superstition_night_shift_machine_rest` | The Turbine's Sleep | `friction_night_shift_repair_refusal` | Emergency repair ordered between 03:00–05:00 |
| `superstition_dead_frequency_omen` | The 94.2 Bad Frequency | `friction_radio_frequency_fear` | Radio tuned to 94.2 MHz during storm watch |
| `superstition_hot_lead_token_talisman` | The Hot Lead Charm | `friction_hot_token_confiscation` | Mildly hot lead casing kept in inventory |

---

## 3. Four Folklore-as-Comfort Moments

| ID | Title | Context | Morale Delta | Effect |
| :--- | :--- | :--- | :--- | :--- |
| `folklore_comfort_blackout_freeze` | Red-Light Freeze Rhyme | `power_failure` | +3.5 | Calms nursery panic during sudden blackout |
| `folklore_comfort_intake_tremor` | Deep-Cold Lullaby | `cold_snap` | +3.0 | Dormitory singing settles sub-zero shivering |
| `folklore_comfort_bereaved_child_bunk_mark` | Under-Bunk Mark | `child_bereavement` | +4.0 | Guided under-bunk mark eases orphan grief |
| `folklore_comfort_scout_return_count` | Return Count for Scout | `scout_trauma_return` | +3.5 | Children's roll-call grounds traumatized scout |
