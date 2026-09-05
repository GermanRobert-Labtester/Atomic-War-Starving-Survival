# Plan 95 — Journal Voice Key Matrix

## 1. Catalog Inventory Summary

The journal voice catalog (`Assets/StreamingAssets/Data/journal_voice_prose.json`) contains **33 situation and lore keys**:
- **5 Baseline Survival Keys**: `high_co2`, `has_seen_radiation`, `has_experienced_storm`, `filter_failing`, `freezing_shelter`.
- **16 Baseline History / Muster Keys**: `history_continuity_reclamation_decree`, `history_hydro_baron_rate_card_origin`, `history_deserter_coalition_founding`, `history_cold_count_before_the_lab`, `history_the_provisioned_advance_knowledge`, `history_checkpoint_conscripts_confession`, `history_quartermasters_paperwork`, `history_the_intercepted_cipher`, `history_the_ledger_nobody_signed`, `history_evacuation_harbor_manifest`, `history_evacuation_offshore_flash`, `history_evacuation_casualty_arrival`, `history_evacuation_quayside_order`, `history_grain_convoy_cargo_manifest`, `history_grain_convoy_ballistic_triage`, `history_grain_convoy_rules_of_engagement`, `history_grain_convoy_third_flank`, `history_foundry_pressure_drop`, `history_foundry_diverted_seals`, `history_foundry_unserved_amendment`, `history_foundry_unwritten_covenant`.
- **12 New Situation Keys (Plan 95)**: Detailed below.

---

## 2. Expanded Situation Keys (Plan 95)

### 1. `low_food`
*Shelter food reserves have fallen below safe thresholds; portions must be curtailed.*
- **default** (17 words): The food stores are running low again. Portions will have to shrink until something comes back in.
- **paranoid** (24 words): The shelves emptied faster than the ledger says they should have. Either the count is wrong, or someone is eating better than the rest of us.
- **cautious** (19 words): We are below a comfortable reserve. Rations need tightening now, before hunger starts making decisions for us.
- **realist** (16 words): There is less food than the shelter needs. We either reduce consumption or bring more in.
- **reckless** (19 words): We have gone hungry before and still worked. Cut the talking, send people out, and fill the shelves.
- **denialist** (23 words): The stores only look bad because everything is spread between too many bins. One decent expedition will make this look foolish.
- **fatalist** (21 words): The shelves keep getting lighter no matter how carefully we count them. Hunger was always going to reach us eventually.

---

### 2. `low_water`
*Safe drinking water supply is approaching depletion.*
- **default** (12 words): Clean water is getting scarce. We are measuring cups now instead of days.
- **paranoid** (18 words): The water count keeps changing between shifts. Someone knows where the missing containers are going.
- **cautious** (21 words): We need to protect what is left and check every source twice. Thirst leaves very little room for correcting mistakes.
- **realist** (18 words): The reserve is below what the shelter needs. Drinking comes first; washing and everything else wait.
- **reckless** (17 words): Sitting here counting bottles will not make more water. Take the cans and find a source.
- **denialist** (19 words): We are not out of water. We are low because everyone started panicking at the same time.
- **fatalist** (19 words): Food lets you bargain with time. Water does not. This shortage will decide for us if we cannot.

---

### 3. `death_of_survivor`
*A survivor has died within the holdfast or on sortie.*
- **default** (22 words): Someone is missing from the shelter now, and every room notices it differently. Their work will be reassigned before anyone is ready.
- **paranoid** (22 words): A death never arrives alone here. Someone missed something, hid something, or decided it was safer not to ask.
- **cautious** (22 words): We need to understand what happened before grief turns into another preventable death. The shelter can mourn and still learn.
- **realist** (20 words): They are dead. Their duties, supplies, and space still have to be accounted for, however ugly that sounds today.
- **reckless** (17 words): Stopping will not bring them back. We carry what they were doing and keep moving.
- **denialist** (21 words): It still feels like they are away on a long shift. I keep expecting the door to open before I remember.
- **fatalist** (20 words): Another bed has become storage. We keep acting surprised when the shelter reminds us what survival costs.

---

### 4. `successful_expedition`
*Scouts have returned from the wasteland with critical resources and intact salvage.*
- **default** (21 words): The expedition came back with enough to matter. For once, the inventory went up before anyone started crossing things out.
- **paranoid** (24 words): They brought back good supplies, almost too clean for the route they described. I want the story checked before I trust the haul.
- **cautious** (20 words): The run paid off. We should store the useful pieces properly before success makes us careless about the next one.
- **realist** (14 words): The expedition returned with a net gain. That buys time, not safety.
- **reckless** (24 words): That is what happens when we stop hiding in the shelter. There is more out there if we move before someone else takes it.
- **denialist** (24 words): See? The shortages were never as bad as everyone said. One proper run and the shelves already look normal again.
- **fatalist** (18 words): We found enough to postpone the next problem. Around here, that counts as a good day.

---

### 5. `failed_expedition`
*Scouts have returned empty-handed, injured, or critically depleted.*
- **default** (17 words): The expedition came back with less than it took out. Nobody is calling the route a mistake yet.
- **paranoid** (22 words): The account does not fit the result. Either the route was worse than reported, or someone out there knew we were coming.
- **cautious** (20 words): We spent supplies and gained almost nothing. The route needs reviewing before another team repeats the same failure.
- **realist** (16 words): The run was a loss. We cannot afford many more expeditions with that return.
- **reckless** (20 words): One bad run is not a reason to close the door. Change the route and send the next team smarter.
- **denialist** (17 words): It was bad luck, nothing more. The same route could pay twice as much tomorrow.
- **fatalist** (24 words): We sent people out because staying here was not enough. They came back proving that leaving is not enough either.

---

### 6. `faction_raid`
*Armed raiders or hostile faction units breached or assaulted shelter defenses.*
- **default** (19 words): They came for what we had and left damage behind with the empty spaces. We will be counting both for days.
- **paranoid** (20 words): They knew where to hit and what to take. Someone has been watching the shelter more closely than we thought.
- **cautious** (21 words): The raid exposed weak points we cannot leave open. Inventory can wait; doors, watch shifts, and escape routes cannot.
- **realist** (22 words): We lost supplies and security at the same time. The next attack will cost more if we repair only one of them.
- **reckless** (19 words): They got inside once. That does not mean they get to leave thinking it was easy.
- **denialist** (20 words): It was a smash-and-grab, not a siege. Patch the damage and stop turning one raid into the end of the shelter.
- **fatalist** (19 words): We built walls because we knew someone would test them. Now we know how long they last.

---

### 7. `disease_outbreak`
*Contagion is spreading through the quarters and bunks.*
- **default** (20 words): Too many people are sick at once for coincidence. Beds, water, and clean hands matter more than arguments now.
- **paranoid** (22 words): Illness spread faster than anyone admitted. I want to know who was sick first and why nobody wrote it down.
- **cautious** (23 words): We need separation, clean supplies, and a proper count of symptoms. Guessing will spread this faster than the disease does.
- **realist** (19 words): This is an outbreak. Reduce contact, protect the healthy, and spend medicine where it changes outcomes.
- **reckless** (20 words): We cannot put the whole shelter in bed. Keep the worst cases down and let everyone else work.
- **denialist** (23 words): People get sick when they are cold and tired. Calling it an outbreak will only make everyone see symptoms they do not have.
- **fatalist** (19 words): Disease likes crowded shelters and tired people. We gave it both long before the first fever.

---

### 8. `power_failure`
*Grid blackout or generator failure in the holdfast.*
- **default** (20 words): The lights are out and every machine sounds louder by being silent. We are back to deciding what can wait.
- **paranoid** (19 words): Power does not just disappear without a reason. I want the breakers checked before anyone blames the grid.
- **cautious** (20 words): We need to isolate the failure and protect what still works. Restarting everything at once would be another mistake.
- **realist** (19 words): The power is down. Heat, pumps, refrigeration, and communications now compete for whatever backup remains.
- **reckless** (20 words): Darkness is not a disaster. Get the generator turning and stop treating every dead bulb like a funeral.
- **denialist** (18 words): The grid has stumbled before. Give it time and half these emergency measures will look unnecessary.
- **fatalist** (18 words): Every machine in the shelter was borrowed time with a wire attached. Tonight the wire ran out.

---

### 9. `new_survivor_arrived`
*A new survivor has been vetted and added to the shelter roster.*
- **default** (24 words): We made room for one more person. Their name is on the roster now, which makes the shelter different whether we notice yet or not.
- **paranoid** (23 words): New people arrive with stories polished by the road. I would rather learn what they left out before we hand them a key.
- **cautious** (24 words): One more survivor means another pair of hands and another set of needs. Give them time, rules, and nothing sensitive until we know them.
- **realist** (20 words): We gained labor, skills, appetite, and risk in one body. The balance will depend on what they can do.
- **reckless** (17 words): Good. We need people willing to work more than we need another empty bunk.
- **denialist** (23 words): Everyone is acting like taking in one person changes the shelter. They will settle in, and things will go back to normal.
- **fatalist** (18 words): We keep adding names because empty beds frighten us. Eventually the roster will thin again.

---

### 10. `severe_cold`
*Deep ambient sub-zero conditions penetrating the shelter envelope.*
- **default** (21 words): The cold has moved indoors despite the walls. Every task takes longer when nobody wants to uncover their hands.
- **paranoid** (24 words): The temperature dropped faster than the forecast said. Either the instruments are failing, or someone gave us numbers they never trusted themselves.
- **cautious** (20 words): Heat loss is winning room by room. Close unused spaces, watch the pipes, and keep the weakest people near warmth.
- **realist** (20 words): The shelter is losing heat faster than we can replace it. Fuel and exposure time are now the same problem.
- **reckless** (22 words): Cold is a reason to move, not freeze in place. Work faster, keep circulation going, and save the complaining for spring.
- **denialist** (17 words): It is a bad cold snap, not a new climate. We have handled worse nights than this.
- **fatalist** (16 words): Winter always finds the weak seams eventually. This time it found ours.

---

### 11. `high_radiation_zone`
*Survivors have traversed or established operations within a high-rad hotspot.*
- **default** (22 words): The meter stayed high long enough that nobody needed reminding to leave. The route may still be useful, but not casually.
- **paranoid** (23 words): The warning markers were too far apart for readings like that. Someone knew the zone was hotter and chose not to mark it.
- **cautious** (20 words): The dose climbed quickly. Next time we need better protection, less time inside, or another route.
- **realist** (20 words): The area is usable only at a cost in dose. That cost needs to be counted like fuel or food.
- **reckless** (22 words): The meter complained, we moved through, and we came back. Keep the exposure short and take what is worth it.
- **denialist** (21 words): The reading was high because the equipment hates that terrain. Nobody felt any different when we crossed it.
- **fatalist** (20 words): Some places keep killing people long after everyone who poisoned them is gone. We crossed one today.

---

### 12. `moral_compromise`
*The shelter authority has enacted a severe ethical compromise to preserve survival.*
- **default** (19 words): We chose the option that kept the shelter moving. Nobody has found a clean way to describe what it cost.
- **paranoid** (21 words): Everyone agreed quickly once the decision became useful. I wonder how many of them had already decided before they asked.
- **cautious** (25 words): We made a choice under pressure and cannot pretend the consequences ended with the decision. We need to remember why we crossed that line.
- **realist** (17 words): The choice solved the immediate problem and created another kind of debt. Both facts are true.
- **reckless** (17 words): We did what the situation demanded. Regret does not feed anyone or undo the decision.
- **denialist** (19 words): People keep calling it a compromise because they need the story to hurt. We made the only workable choice.
- **fatalist** (16 words): Survival keeps asking for pieces of us and calling the exchange temporary. We paid again.
