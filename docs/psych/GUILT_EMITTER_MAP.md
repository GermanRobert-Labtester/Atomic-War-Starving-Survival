# Plan 66 — Guilt Trigger Emitter & Systems Integration Map

**Authority:** `Assets/StreamingAssets/Data/guilt_sources.json` (40 triggers)

---

## 1. 20 New Sources Emitter Mapping

| Choice Pattern | Category | Primary Emitting System | Gameplay Choice / Trigger Event | Downstream Integration |
|---|---|---|---|---|
| `hoard_medicine_while_needed` | Resource | Medical / Inventory | Withholding critical medication from a dying survivor. | `GuiltInsomniaSystem` + `MedicalSystem` |
| `barter_away_needed_food` | Resource | Economy / Settlement | Bartering away subsistence rations for ammo or scrap. | `GuiltInsomniaSystem` + Morale penalty |
| `issue_known_contaminated_supplies` | Resource | Disease / Water / Nutrition | Issuing food or water marked irradiated to survivors. | `GuiltInsomniaSystem` + Contamination |
| `burn_critical_fuel_for_comfort` | Resource | Power / Heating / Facilities | Burning generator fuel for sleeping warmth while hydroponics lines freeze. | `GuiltInsomniaSystem` + Heating system |
| `refuse_refugee_entry` | Shelter | Door Encounters / Recruitment | Cutting the intercom and locking out desperate refugees in toxic ash. | `GuiltInsomniaSystem` + Plan 57 Incidents |
| `expel_survivor_for_efficiency` | Shelter | Shelter Management / Roster | Evicting an unproductive or injured survivor to free up a bunk. | `GuiltInsomniaSystem` + Plan 57 Incidents |
| `hide_cache_from_allies` | Shelter | Inventory / Factions | Concealing high-value supplies behind false panels during group rationing. | `GuiltInsomniaSystem` + Trust loss |
| `abandon_committed_rescue` | Expedition | Expedition / Quests | Aborting an expedition midway after a distress beacon was accepted. | `GuiltInsomniaSystem` + Quest failure |
| `leave_wounded_behind` | Expedition | Expedition / Combat | Abandoning an incapacitated party member during an overland retreat. | `GuiltInsomniaSystem` + Memorial System |
| `retreat_from_rescue` | Expedition | Distress Radio / Signals | Muting or tuning away from an active SOS signal to conserve resources. | `GuiltInsomniaSystem` + Radio System |
| `execute_surrendered_enemy` | Combat | Combat / Prisoner Roster | Executing an unarmed or surrendered hostile enemy in cold blood. | `GuiltInsomniaSystem` + `CombatTraumaSystem` |
| `use_civilians_as_bait` | Combat | Combat / Tactics | Creating a noisy civilian diversion to draw raiders away from a cache. | `GuiltInsomniaSystem` + `CombatTraumaSystem` |
| `kill_former_ally` | Combat | Faction Combat / NPCs | Engaging and killing a named former comrade fighting for a rival faction. | `GuiltInsomniaSystem` + NPC Arcs |
| `betray_faction_trust` | Social | Faction Diplomacy / Treaties | Breaking a signed treaty or stealing from a allied faction enclave. | `GuiltInsomniaSystem` + Faction Standing |
| `inform_on_survivor` | Social | NPC Arcs / Faction Events | Handing over a shelter resident's name to an armed wasteland patrol. | `GuiltInsomniaSystem` + Survivor Grievance |
| `break_final_wish_promise` | Social | Plan 65 Final Wishes | Failing or refusing to carry out a dying survivor's last wish. | `GuiltInsomniaSystem` + Plan 65 failure |
| `withhold_pain_relief` | Medical | Medical / Infirmary | Preserving palliative medication while a terminal patient suffers. | `GuiltInsomniaSystem` + Plan 65 dignity |
| `triage_by_utility` | Medical | Medical / Triage | Sorting treatment order by survivor physical utility rather than medical urgency. | `GuiltInsomniaSystem` + Medical System |
| `take_family_last_supplies` | Scavenging | Scavenging / Micro-locations | Stripping the final food reserves from an inhabited domestic homestead. | `GuiltInsomniaSystem` + Scavenging loot |
| `order_survivor_to_death` | Leadership | Leadership / Emergency Ops | Ordering a survivor into a fatal task (e.g. radioactive pipe repair) for group survival. | `GuiltInsomniaSystem` + Memorial System |

---

## 2. Cross-System Bridge Contracts

1. **Plan 65 (Final Wishes Integration):**
   - `break_final_wish_promise` (0.85 severity) fires when an active terminal prognosis expires without completion or when the player rejects the wish.
   - `withhold_pain_relief` (0.70 severity) fires when palliative medicine is withheld during a `die_with_dignity` wish.
2. **Psychological Contamination / Trauma Integration:**
   - When total insomnia severity exceeds `0.70`, `OnGuiltInsomniaCritical` fires, directly reducing sleep quality by up to 50% and triggering psychological distress events.
   - High-impact choices (`order_survivor_to_death`, `use_civilians_as_bait`, `execute_surrendered_enemy`) immediately push a survivor into critical threshold.
3. **Plan 57 (Shelter Incidents Integration):**
   - 5 historical guilt sources (`refuse_refugee_entry`, `expel_survivor_for_efficiency`, `hide_cache_from_allies`, `leave_wounded_behind`, `break_final_wish_promise`) are documented as condition triggers for future survivor confrontation incidents.
