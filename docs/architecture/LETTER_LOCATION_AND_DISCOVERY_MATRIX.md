# LETTER LOCATION & DISCOVERY MATRIX

## 1. Spatial Projection Architecture
Authored `location` strings in `narrative/letters_expansion.json` describe environmental micro-settings (e.g., `"the bedside table, folded under the lamp"`, `"the cold frame, in the dirt"`).

To make these discoverable through existing gameplay interaction loops, each letter is mapped to a canonical shelter room (`room_*`) or surface expedition zone (`loc_*`), with a defined discovery channel.

---

## 2. Canonical Location & Producer Mapping

| Letter ID | Authored Location Description | Canonical Room / Location ID | Discovery Channel | Min Day | Producer Rationale |
|---|---|---|---|---|---|
| `letter_01_to_mother` | the shelter, by the lamp | `room_bunks` | `location_inspection` | 12 | Personal living quarters by dwelling bunk beds. |
| `letter_02_to_son` | the gate house, night | `room_airlock` | `location_inspection` | 28 | Gatehouse airlock sentry post. |
| `letter_03_to_sister` | the mess hall, after the meal | `room_kitchen` | `location_inspection` | 19 | Galley canteen and pantry area. |
| `letter_04_to_lover_returning` | the bedside table, folded under lamp | `room_bunks` | `location_inspection` | 8 | Domestic bunk cubicle. |
| `letter_05_to_lover_gone` | the bedside table, unfolded, lamp cold | `room_bunks` | `location_inspection` | 44 | Domestic bunk cubicle after disappearance. |
| `letter_06_to_grown_daughter` | the cold frame, in the dirt | `room_greenhouse` | `location_inspection` | 33 | Hydroponics and soil cold frame beds. |
| `letter_07_last_letter` | the forward post, in the cap | `room_airlock` | `location_inspection` | 5 | Surface airlock observation post / sentry cupola. |
| `letter_08_confession_theft` | the west locker, inside door | `room_storage_bay` | `location_inspection` | 23 | Secure supply locker in storage gallery. |
| `letter_09_confession_cowardice` | the road, in the ditch | `room_bunker_corridor` | `location_inspection` | 14 | Found on traveler belongings in main concourse. |
| `letter_10_confession_mercy` | the clinic, in the ledger | `room_clinic` | `location_inspection` | 30 | Infirmary desk / medical triage station. |
| `letter_11_to_the_dead_husband` | the cold room, after | `room_clinic` | `location_inspection` | 40 | Clinic morgue / cold preservation bay. |
| `letter_12_to_the_dead_child` | the school, in the book | `room_bunker_corridor` | `location_inspection` | 22 | Improvised classroom corner in corridor alcove. |
| `letter_13_to_whoever_finds_this` | the ditch, by the road | `room_airlock` | `location_inspection` | 50 | Recovered by surface scout team. |
| `letter_14_thank_you_to_scavenger` | the printing works, on desk | `room_workshop` | `location_inspection` | 28 | Workshop drafting desk / tool fabrication bench. |
| `letter_15_child_to_father` | the shelter, in the book | `room_bunks` | `location_inspection` | 16 | Children's sketchpad in residential bunks. |
| `letter_16_to_old_friend` | the mess hall, under bowl | `room_kitchen` | `location_inspection` | 25 | Mess hall dining table beneath soup bowl. |
| `letter_17_to_the_engineer` | the generator room, on logbook | `room_filtration` | `location_inspection` | 24 | Diesel powerhouse / filtration stack logbook desk. |
| `letter_18_the_list_of_names` | the admissions counter, in folder | `room_main` | `location_inspection` | 62 | Main atrium intake desk and records archive. |
| `letter_19_to_the_house` | the door, pinned under stone | `room_airlock` | `location_inspection` | 35 | Brought in by returning scavenger. |
| `letter_20_apology_to_stranger` | the well, on ledge | `room_water_pump` | `location_inspection` | 17 | Deep well intake pump room. |
| `letter_21_to_the_teacher` | the school, on desk | `room_bunker_corridor` | `location_inspection` | 36 | School study table in corridor alcove. |
| `letter_22_to_younger_self` | the cold frame, in the dirt | `room_greenhouse` | `location_inspection` | 100 | Late-campaign greenhouse soil inspection. |
| `letter_23_warning_to_the_next` | the cabin, on table | `room_airlock` | `location_inspection` | 7 | Recon report returned from surface outpost. |
| `letter_24_love_without_the_word` | the bedside table, under watch | `room_bunks` | `location_inspection` | 48 | Bunk bedside table beneath watch. |
| `letter_25_one_sentence` | the hook, by the door | `room_airlock` | `location_inspection` | 3 | Outer blast door entryway coat hook. |

---

## 3. Producer Integration Rules
1. **Inspection Reachability**: When a player clicks or inspects a shelter room, letters mapped to that room whose `min_day <= currentDay` become eligible for discovery through the existing `NarrativeDiscoveryCatalog` workflow.
2. **Zero Involuntary Disruption**: Letters never interrupt gameplay with intrusive modals. They populate the Journal and Codex under "Personal Letters & Unsent Correspondence".
