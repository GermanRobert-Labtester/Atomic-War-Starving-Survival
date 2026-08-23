# ASHFALL — Expansion Proposal 11: THE LONG LINE
## Master Story, Lore, Location & Questline Creative Pack

**Proposed internal id:** `expansion_11_the_long_line`  
**Status:** creative proposal only — **not canonical until explicitly approved and added to `ExpansionSuite`**  
**Campaign window:** Day 120+; designed to remain useful through the late campaign  
**Primary geography:** existing Sector 4 only — The Grid, The Verge, The Spine, The Toll, The Drown  
**Primary theme:** communication as infrastructure, ration, evidence, intimacy, and power  
**Voice lock:** cold, exhausted, human, restrained. Specificity over adjectives. No supernatural explanation. No real countries, wars, or public figures. No glorified violence.  
**Design rule:** this expansion adds **no fifth territorial Power** and **no new victory path**. It introduces one non-territorial Current and a physical communications network that crosses the existing map.

---

# 1. One-sentence pitch

A buried civil-defense telephone trunk begins ringing inside Allocation 12, and restoring it forces the shelter to decide not merely **who may speak across Sector 4**, but who is allowed to interrupt everyone else when only one line remains.

---

# 2. Why this belongs in ASHFALL

ASHFALL already has radio, factions, expeditions, weather, medicine, bureaucracy, evidence, route control, archives, trade, and a strong history of making infrastructure political without making it magical.

**The Long Line** does not compete with radio. Radio is broadcast: one voice can reach anyone who can hear it. The Long Line is the opposite. It is point-to-point, physical, fragile, private until somebody lifts another receiver, and limited by how many copper pairs still work.

A radio asks whether a signal gets through.

A telephone asks **who is connected to whom, who is waiting, and who has the right to break in.**

The expansion turns old civil infrastructure into a campaign-scale narrative system:

- buried lead-sheathed cable;
- glass battery cells and 48-volt exchange power;
- manual patch boards;
- repeater huts;
- flood-alarm circuits;
- weather telemetry lines;
- rail dispatch boards;
- party lines in the agricultural belt;
- Continuity priority relays;
- call ledgers;
- abandoned household numbers;
- one trunk that reaches beyond the playable map without opening a new region.

The world gets larger because another human voice answers, not because a new map opens.

---

# 3. Canon fit and hard boundaries

This pack is written against the existing lore model:

- Sector 4 remains the playable geography.
- The four territorial Powers remain the four territorial Powers.
- The Drown remains unclaimed.
- New organizations must behave like **Currents** rather than territorial factions unless code architecture is deliberately changed.
- Located knowledge remains the preferred lore delivery method.
- Trust-reactive prose remains more important than visible reputation numbers.
- The Continuity Allocation Schedule and Allocation 12 may be referenced, but this expansion must still function if the player has not completed every beat of *The List*.
- The line may reach beyond Sector 4 by voice, but the far end is not a travel destination and not a new world map.
- Existing expansion outcomes are read as optional flags; they are never rewritten.

The current canonical expansion registry contains 01–10. This proposal intentionally does **not** modify that registry, tests, save schema, or gameplay code.

---

# 4. Creative thesis — “The busy hour”

Telephone engineers once used the phrase **busy hour** for the part of the day when the network carried its maximum load.

After the Exchange, every hour is a busy hour and almost none of the network exists.

The dramatic engine is simple:

1. The player repairs physical circuits.
2. Repair creates access.
3. Access creates demand.
4. Demand creates queues.
5. Queues require rules.
6. Rules become politics the first time two emergencies happen at once.

The expansion must never solve this with a clean morality meter. A single mother in labour and seventeen people at a flood pump cannot be reduced to a red choice and a blue choice. The player is shown the board, the circuits that are free, the calls that are waiting, and what is known at that moment.

Then the player chooses.

The consequences arrive later and are described as events, not judgments.

---

# 5. The central premise

## The ring

Day 120 or later, after the shelter has stable enough power to notice small loads, a bell rings behind a bolted service panel in Allocation 12.

Not an alarm.

A telephone bell.

It rings once, stops, then rings twice more about forty seconds later.

Behind the panel is a dust-grey field handset mounted sideways because the cabinet was never finished. The paper tag tied to its cord reads:

> `ALLOC 12 / EXT 3 / PRIORITY C / TEST BEFORE OCCUPANCY`

Nobody in the shelter has heard it before because the local exchange has been dead since Hour Zero.

Somebody, somewhere, has just put battery back on the pair.

## What actually happened

Before the Exchange, Sector 4 was served by a municipal telephone exchange and a civil-emergency trunk running east-west under the city, under the bridge approaches, through the highland repeater corridor, and down toward the reclamation works.

The system was designed with emergency preemption.

Priority was physical. A key in the exchange could seize working trunks for Continuity, military command, hospitals, utilities, and allocated shelters in that order. During the final warning, the mechanism did exactly what it had been built to do.

Thousands of civilian calls were disconnected so that higher-priority lines would remain available.

The higher-priority people mostly never called.

The switchboard records this without comment.

The player is not solving a conspiracy. The player is restoring a machine that worked as designed.

---

# 6. The new Current — `faction_linekeepers`
## The Linekeepers

**Type:** Current, never a territorial Power  
**Territory:** none  
**Practice:** maintaining working wire between people who would otherwise be cut off  
**Relationship model:** access granted / restricted / withdrawn; not territorial standing  
**What they want:** dry cable paper, paraffin, copper, solder, ceramic arresters, battery acid, glass cells, fuse wire, hand tools  
**What they offer:** line repair, route bulletins, call capacity, technical maps, emergency interconnection

The Linekeepers are not a guild in the old sense. There is no headquarters and no membership card. A Linekeeper is somebody who knows where a pair runs and keeps it dry.

They began as municipal cable jointers, railway signal technicians, hospital operators, and two people who learned by watching because the people who knew were dying.

They do not own the line.

That sentence is the closest thing they have to doctrine.

### Their five rules

1. **Say whether the line is up.** Never claim a dead circuit is working and never claim a working circuit is dead for politics.
2. **Do not repeat a call.** What was said belongs to the two ends unless both ends ask for it to be recorded.
3. **Come off the key after connect.** An operator may hear enough to establish the circuit. Listening after that is listening.
4. **Emergency means danger that can still be changed.** A death already happened is a report, not an emergency call.
5. **A wet joint is dead until tested from both ends.** Hope is not insulation.

### Why everybody tolerates them

The Garrison needs dispatch circuits.

The Militia needs weather and crop warnings.

The Warlords need bridge status and freight notice.

The Cult wants instrument readings from the Spine.

The Archivists want names verified.

The Quiet House wants relatives found before there is no more time.

The Black Flotilla wants ice and tide reports.

The Silent Foundry wants road and load notices.

Nobody agrees on much. Everybody has waited beside a silent receiver.

### What makes them dangerous to offend

They do not sabotage people who mistreat them.

They simply stop volunteering labour.

A line that loses maintenance does not go dark in a dramatic scene. First one call becomes noisy. Then rain gets into a joint. Then the party line develops a hum. Then the repeater takes three attempts to start. Then somebody writes `NO TEST — NO MAN` on a card and ties it to a handle.

Eleven days later, the circuit is gone.

---

# 7. Principal characters

## `npc_mara_ell` — Mara Ell
**Age:** 58  
**Former work:** municipal cable jointer, Outside Plant Section 4  
**Current:** senior Linekeeper

Mara can identify cable gauge by bending it between two fingers. She carries a folding knife with one blade ground blunt for scraping lead sheath and another she never uses.

She does not romanticize the old network. Asked whether it used to connect everyone, she says:

> “Everyone who paid. Everyone whose street still had copper. Everyone the exchange had a record for. That is not everyone.”

She cares about continuity of service because she has spent five years learning the difference between a person being unreachable and a person being dead.

Her central conflict with the player is technical rather than ideological: she will maintain a line she dislikes, but she will not certify a line as neutral if the player has installed a priority seizure key.

## `npc_sen_ard` — Sen Ard
**Age:** 36  
**Former work:** night operator, Regional Hospital switchboard  
**Current:** manual-board operator

Sen remembers voices better than faces. In the hospital, callers rarely gave surnames. They gave ward numbers, symptoms, or the name of somebody who had stopped breathing.

His hands move automatically on a cord board even after years away from one.

He refuses to write call content into the traffic ledger. He writes origin, destination, connect time, clear time, and whether the call completed.

> “The ledger proves a line existed. It does not prove what somebody said through it.”

He becomes the expansion’s clearest advocate for privacy, though he never uses that word.

## `npc_pele_orin` — Pele Orin
**Age:** 19  
**Former work:** none; eleven at the Exchange  
**Current:** runner, pole climber, apprentice cable jointer

Pele has never used a telephone that was connected to another live telephone.

He knows line work from maps, habits, and Mara’s corrections. He is the fastest person in the group through the Toll culverts and the least patient with pre-war labels.

His personal arc is small: at first he calls every old number “dead.” Sen repeatedly corrects him to “not answering.” Late in the expansion, Pele uses the distinction himself.

## `npc_renna_mere` — Renna Mere
**Age:** 43  
**Location:** North Basin Control, beyond the playable map  
**Role:** far-end duty operator, voice only

The player never meets Renna.

The first time Pair 17 reaches her, she does not ask who the player is. She asks for exchange code, battery polarity, and the number printed on the test jack.

North Basin Control is not a kingdom, settlement hub, or promised sequel map. It is four functioning sites sharing one switchboard: a flood gate, a clinic, a machine shop, and a grain store.

Renna’s voice proves that other people are alive farther away than the player can walk.

That is all the proof the game gives.

> “We are not sending anybody. You are too far. Keep the pair dry and we can still be useful to each other.”

## `npc_ivo_kern` — Ivo Kern
**Age:** 51  
**Affiliation:** Iron Garrison signals section  
**Role:** claimant to the old priority system

Kern is not a cartoon officer. He understands the exchange better than most surviving civilians because military doctrine trained him to.

His argument is straightforward: emergency networks only work if somebody can preempt routine traffic. If a bridge is under attack or a water plant is failing, command must be able to seize the line.

He is correct about the engineering problem.

He is less convincing about who counts as command.

## `npc_lida_sey` — Lida Sey
**Age:** 39  
**Affiliation:** Verge cooperative  
**Former work:** maternity nurse; now midwife, clerk, crop-record keeper  
**Role:** advocate for ordinary access

Lida has no philosophical position on public communications. She has a list of nine farms, two clinics, one mill, and the hours each can spare somebody to stand by a phone.

When told the Garrison wants priority access, she says:

> “Fine. Put their emergencies in the queue. Put ours there too.”

Her quest content gives the player the strongest evidence that a network’s value is often mundane: asking whether a road is passable, whether a fever is spreading, whether a spare gasket exists three kilometres away.

---

# 8. Proposed systemic layer

This is a creative specification, not implementation, but the story works best if the network has a small visible state.

### `line_integrity`
0–100. Represents tested, dry, powered physical network condition.

Low integrity produces noise, dropped calls, uncertain route bulletins, and occasional repair events. High integrity does **not** produce resources; it produces dependable information.

### `circuit_capacity`
How many simultaneous calls the surviving network can carry.

The campaign begins at zero, reaches one, then two. Reaching three should require late optional repairs and meaningful material cost.

The Busy Hour climax is designed around the fact that capacity is still insufficient even after good play.

### `queue_policy`
Temporary operating rule before the final charter.

Examples:
- first requested;
- emergency first;
- shelter priority;
- faction-contract slots;
- operator discretion.

### `privacy_policy`
- operator-connect only;
- traffic metadata retained;
- monitored by player;
- monitored by controlling Power.

This policy changes dialogue, some quests, and whether certain Currents continue using the line.

### `station_access`
Each restored station is a practical capability rather than a collectible percentage.

Examples:
- Verge party line → crop, medical and road calls;
- Spine weather line → earlier forecast;
- Toll repeater → long-distance trunk;
- Drown alarm line → flood warnings;
- hospital board → medical consultation;
- North Basin 17 → distant knowledge and corroboration.

---

# 9. Location atlas — 18 new authored locations

No location below creates new territory. Each belongs to one of the five existing sub-regions or is a sublocation inside an existing site.

## THE GRID

### 1. `loc_sector4_exchange` — Sector 4 Central Exchange
**Region:** The Grid  
**Danger:** 4  
**Travel:** 2.4 h  
**Radiation:** low-moderate

A three-storey brick utility building with no windows on the ground floor. The public entrance still has opening hours painted on the glass. The glass is gone.

Inside, relay racks stand in aisles taller than a person. Dust lies everywhere except on seven test jacks where somebody has recently pushed plugs in and out. A wall clock stopped at 04:17. Another clock on the next floor stopped at 04:23. Neither time is Hour Zero. The batteries kept the building alive after the city did not.

**Inspect:** a directory drawer contains streets that no longer exist and numbers that still do.  
**Quest use:** main arc hub; Mara and Sen first appear here.  
**Loot tone:** relays, fuses, patch cords, test lamps; almost no conventional valuables.

### 2. `loc_exchange_battery_floor` — Forty-Eight Volt Room
**Region:** The Grid  
**Danger:** 6  
**Travel:** same site  
**Hazards:** acid, lead, collapsed ventilation

Rows of glass cells sit on steel racks under a ceiling furred with white corrosion. Each jar is big enough to require two people to lift. The plates have slumped in most of them. Four cells still hold clear liquid to the pencil line.

A rubber apron hangs from a peg. The apron is cracked at the folds but the pencilled inspection dates on the wall continue eleven days after the Exchange.

**Inspect:** `FLOAT 52.8V` is written above the busbar in careful block letters.  
**Quest use:** first exchange restoration.  
**Choice:** salvage cells for the shelter or restore the board; both are useful and mutually exclusive in the short term.

### 3. `loc_main_distribution_frame` — Main Distribution Frame
**Region:** The Grid  
**Danger:** 3  
**Travel:** same site

Thousands of pairs terminate on vertical blocks. Each pair is two thin conductors and a paper label. The labels survived better than the buildings they describe.

One jumper is newer than the others. It runs from `ALLOC-12 / EXT 3` to an unmarked test position and is tied with blue cotton instead of plastic.

Nobody knows who made it.

Later records make the answer mundane: a technician made it three weeks before the Exchange because Allocation 12 failed acceptance testing.

**Lore:** establishes that the player shelter always had a direct circuit.  
**Quest use:** Pair 12 tracing.

### 4. `loc_alloc12_cable_vault` — Allocation 12 Cable Vault
**Region:** Grid/Verge boundary  
**Danger:** 4  
**Travel:** 0.8 h from shelter

A cast-iron street vault under a service road. Meltwater stands ankle deep around a lead-sheathed trunk the width of a wrist.

The splice case is wrapped in black compound and linen tape. Somebody opened it once after the Exchange and closed it badly. The paper insulation inside smells sweet and rotten where it has taken water.

**Quest use:** first physical repair.  
**Item needs:** dry splice paper, paraffin, copper pair, hand pump.  
**Failure texture:** a rushed repair works for days, then begins humming after thaw.

### 5. `loc_hospital_night_switchboard` — Regional Hospital Night Board
**Region:** The Grid  
**Danger:** 6  
**Travel:** inside/adjacent to `loc_regional_hospital`

A small switchboard behind the old admissions desk. Sixteen cord pairs hang from hooks. Three plugs remain inserted.

The traffic slip under the lamp contains only origin numbers and times. The final line reads:

> `04:31 — MATERNITY / OUTSIDE — HELD`  

There is no clear time.

Sen recognizes the handwriting but does not name the operator immediately.

**Quest use:** medical-line restoration; Eleven Minutes lore beat.

### 6. `loc_dead_number_house` — Number 4-2217
**Region:** The Grid  
**Danger:** 2  
**Travel:** 1.6 h

An ordinary house. Roof intact. Kitchen stripped. Telephone on a hallway table beneath a mirror.

Once the exchange is powered, this phone rings if called.

Nobody answers.

The bell works because the line works. That distinction becomes a recurring motif.

**Quest use:** “Number Not Answering” side quest.  
**Loot:** none worth listing. The point is the circuit.

## THE VERGE

### 7. `loc_verge_party_line_house` — South Cooperative Party-Line Hut
**Region:** The Verge  
**Danger:** 2  
**Travel:** 1.9 h

A farm office with six magneto telephones screwed to one plank wall. Each has a different ring code written above it: two short, one long; long-short-long; three short.

The old system let twelve farms share one physical pair. Everyone could listen if they lifted a receiver.

People did.

The cooperative does not remember this as a privacy crisis. They remember it as how you knew whose cow was down before breakfast.

**Quest use:** rural network and privacy contrast.  
**Benefit:** Verge route, crop, and medical bulletin access.

### 8. `loc_irrigation_alarm_shed` — Terrace Water Alarm Shed
**Region:** The Verge  
**Danger:** 3  
**Travel:** 2.3 h

A concrete hut above a valve chamber. Two float switches once rang the exchange if the irrigation header lost pressure.

One float is stuck under a mat of dead roots. The other moves freely and still clicks.

A chalkboard lists valve turns beside first names only. Most names have been rubbed off by sleeves.

**Quest use:** weather/water companion line.  
**Benefit:** earlier drought and freeze warnings if maintained.

### 9. `loc_linekeepers_splice_yard` — The Splice Yard
**Region:** Verge/Toll edge  
**Danger:** 3  
**Travel:** 2.7 h

Not a headquarters. A bus layby with cable reels under tarpaulins, two work benches, a brazier, and a board listing faults.

Each fault is written as a place and symptom:

`BRIDGE EAST — HUM IN WET`  
`HOSPITAL — RINGS / NO SPEECH`  
`17 — NO BATTERY WEST`  

Nobody signs the board.

**Quest use:** Linekeeper access hub.  
**World detail:** if access is withdrawn, the board remains but new chalk stops appearing.

## THE TOLL

### 10. `loc_bridge_cable_gallery` — East Bridge Cable Gallery
**Region:** The Toll  
**Danger:** 6  
**Travel:** 3.5 h

A steel maintenance walkway under the road deck. The river is visible through gaps in the grating. Telephone cable, signalling cable, and dead power conduit share the same brackets.

The Warlords charge toll above. Below, somebody has painted the cable every five metres so smugglers can identify the correct line in darkness.

The paint is not theirs. It predates them.

**Quest use:** route-rights conflict and Pair 17 repair.  
**Hazard:** ice, height, old demolition wiring nearby but clearly marked according to canon denial doctrine.

### 11. `loc_carrier_repeater_17` — Repeater Hut 17
**Region:** The Toll  
**Danger:** 5  
**Travel:** 4.1 h

A roadside concrete box with two doors, one inside the other. The equipment rack carries a bank of carrier filters, a rectifier, and a little inspection stool bolted to the floor.

A thermos sits under the stool. The lid is missing. Somebody has stuffed the opening with cloth to keep ash out.

This hut is the reason the far trunk can work at all.

**Quest use:** first contact with North Basin.  
**Repair needs:** rectifier module, fuse wire, cleaned contacts, stable 48V source.

### 12. `loc_train_dispatch_board` — Rail Dispatch Telephone Board
**Region:** The Toll  
**Danger:** 5  
**Travel:** 3.8 h

The dispatch room overlooks tracks that stop being tracks fifty metres east. The board still shows block sections using small metal flags.

A handset is labelled `BRIDGE`, another `ROUNDHOUSE`, another `CIV DEF EXCH`.

The last dispatcher crossed out no trains after Hour Zero. The timetable continues down the page in printed ink, leaving blank spaces where handwriting should be.

**Quest use:** Warlord and rail cross-content; logistics bulletin endpoint.

## THE SPINE

### 13. `loc_weather_wire_house` — Highland Weather Wire House
**Region:** The Spine  
**Danger:** 6  
**Travel:** 5.6 h

A low stone hut below the ridge instruments. Copper lines enter through porcelain tubes. Inside: a barometer, wet-bulb cradle, chart drums, and a telephone with no dial.

The last paper chart is still wrapped around the drum. The pen drew pressure falling until the ink froze.

**Quest use:** Weather Wire companion arc.  
**Benefit:** forecast lead time and False Spring warnings.

### 14. `loc_observatory_patchbay` — Observatory Instrument Patchbay
**Region:** The Spine  
**Danger:** 7  
**Travel:** tied to existing observatory sites

A rack where instrument circuits were patched onto the civil line during storms. The Cult has labelled several jacks with devotional names in charcoal but has not altered the wiring.

They can operate the anemometer correctly.

They interpret what the wind means differently.

**Quest use:** negotiation with Cult; data sharing without validating doctrine.

### 15. `loc_highland_ground_bed` — Ridge Ground Bed
**Region:** The Spine  
**Danger:** 7  
**Travel:** 6.2 h

Thirty copper rods driven into wet mineral soil, linked by green-black braid. Lightning arresters dump surge energy here before it enters the repeater line.

Most rods are still present because copper underground is harder to steal than copper on a wall.

**Quest use:** permanent reduction in line fault chance; dangerous during electrical weather.

## THE DROWN

### 16. `loc_flooded_exchange_west` — West Exchange Basement
**Region:** The Drown  
**Danger:** 9  
**Travel:** 6.8 h

The ground floor is dry enough to stand in. The cable basement is not.

Rows of distribution blocks descend into black water. When another restored station rings this exchange, tiny lamps below the waterline glow green for a second and go dark.

Methane gathers above the stairwell. No open flame.

**Quest use:** late repair; expands capacity from one to two simultaneous calls.  
**Tone:** extraordinary visual with completely ordinary cause.

### 17. `loc_river_cable_landing` — Armoured River Cable Landing
**Region:** The Drown  
**Danger:** 8  
**Travel:** 7.2 h

A concrete landing chamber where the trunk crosses beneath the river in steel armour. One cable was cut for copper years ago. The second remains because the water came in before the thieves could finish.

A hacksaw lies on the ledge above the flood mark.

The blade is new enough to have teeth.

**Quest use:** optional third circuit / Black Flotilla cross-hook.  
**Hazard:** water, cold, confined space.

### 18. `loc_pump_alarm_room` — Reclamation Pump Alarm Room
**Region:** The Drown  
**Danger:** 8  
**Travel:** 6.4 h

A narrow room above the old flood pumps. The alarm board has red windows for sluice position, motor overload, and sump depth. Most windows are dark.

One still works if the line is powered.

The room becomes one of the Busy Hour endpoints.

**Quest use:** flood emergency call; no treasure required.

---

# 10. The far end — North Basin Control

**Not a map node. Not travelable. Not a new faction.**

North Basin Control is approximately ninety kilometres beyond the last repaired repeater chain. Distance is intentionally imprecise because nobody has a reliable current road measurement.

It connects four sites:

- a flood gate with eleven workers;
- a clinic with three staff;
- a machine shop with six adults and two apprentices;
- a grain store and adjacent settlement using the remaining line as a bulletin service.

The player never sees them.

Their existence should be communicated through practical details:

- Renna asks for tomorrow’s ash opacity because their solar battery bank is weak.
- The machine shop can describe how to lap a valve seat but cannot send the tool.
- Their clinic has a drug name the player has not heard in years, but none left.
- They know a road from before the Exchange that no longer exists on Sector 4 maps.
- They sometimes fail to answer because someone has to leave the switchboard to carry water.

The point is not hope as spectacle.

The point is that the world contains other ordinary rooms with other ordinary shortages.

---

# 11. The buried lore spine — the Priority Table

The Long Line’s historical mystery is not “who caused the war.” It is smaller and more useful.

**Why did the network go silent before many physical lines failed?**

Because it was preempted.

## The Priority Table

The civil network carried five emergency classes:

| Class | Pre-war purpose | Example |
|---|---|---|
| `A0` | national / Continuity seizure | Continuity Office, command circuits |
| `A1` | military and civil-defense command | Garrison predecessors, siren control |
| `B` | hospitals and critical utilities | hospital, flood works, water treatment |
| `C` | allocated shelters and designated reconstruction sites | Allocation 12 |
| `U` | unclassified public service | households, shops, ordinary callers |

A key at Sector 4 Central Exchange could disconnect lower classes to free trunks for higher ones.

At Hour Zero, the key was turned.

This was not hidden.

It was in the manual.

## The Eleven Minutes

A night operator at Regional Hospital kept one unclassified outside line connected for eleven minutes after preemption because the caller was speaking to maternity.

The operator did not save the city.

The operator kept one cord in one jack.

The exchange log records a supervisor notation:

> `U-LINE HELD 11:14 BEYOND SEIZURE — OPERATOR OVERRIDE — REVIEW`

There was no review.

The physical cord is still in the board.

The game never establishes whether the person on the other end survived.

---

# 12. Proposed `world_history` beats

These are written as data-ready lore concepts. Exact schema conversion should use the existing `world_history.json` fields.

## `pre_exchange`

### `lore_line_common_carrier`
**Title:** The Common Carrier Rule  
**Found at:** `loc_sector4_exchange`

The telephone company was required to carry any lawful call for any paying line. Emergency doctrine added exceptions until the exceptions had their own binder.

### `lore_line_forty_eight_volts`
**Title:** Forty-Eight Volts  
**Found at:** `loc_exchange_battery_floor`

Exchange batteries were oversized because people noticed telephone failure sooner than almost any other civic failure. The specification required eight hours. The cells ran for eleven days because most lines stopped drawing current.

### `lore_line_priority_table`
**Title:** Priority Is a Number  
**Found at:** `location_ministry_of_truth_bunker`

The priority table was printed on one page. It was meant to prevent arguments during emergencies by deciding the argument beforehand.

### `lore_line_allocated_extensions`
**Title:** Shelter Extensions  
**Found at:** `loc_main_distribution_frame`

Allocated shelters received direct emergency circuits. Allocation 12 was provisioned as Class C and failed its final voice test three weeks before occupancy.

### `lore_line_last_directory`
**Title:** Directory Correction  
**Found at:** `loc_sector4_exchange`

A clerk corrected three street names after evacuation orders had already begun. The corrected pages were never delivered. The work was completed anyway.

## `hour_zero`

### `lore_line_priority_zero`
**Title:** Seizure  
**Found at:** `loc_sector4_exchange`

The priority key was turned once. Lamps for hundreds of lower-class calls went dark at the same moment. The event recorder marks the action as successful.

### `lore_line_eleven_minutes`
**Title:** Eleven Minutes  
**Found at:** `loc_hospital_night_switchboard`

One outside call remained connected after seizure. The notation says maternity. The clear time is blank.

### `lore_line_calls_waiting`
**Title:** Calls Waiting  
**Found at:** `loc_sector4_exchange`

The final peg count was 184 waiting calls. Peg counts record demand, not content. Nobody wrote what 184 people wanted.

### `lore_line_alloc12_no_answer`
**Title:** Allocation 12 — No Answer  
**Found at:** `loc_main_distribution_frame`

The exchange tested Allocation 12 three times during the warning period. Ring current reached the shelter. Nobody answered because the intended occupants had not arrived.

## `black_sky`

### `lore_line_manual_islands`
**Title:** Islands  
**Found at:** `loc_verge_party_line_house`

For several months, isolated farm loops still worked locally even after the central exchange failed. People learned which neighbours were alive by which ring codes were answered.

### `lore_line_first_cut`
**Title:** The First Cut  
**Found at:** `loc_river_cable_landing`

Copper theft did not begin with greed. The first recorded cut supplied wire for a clinic heater. The second supplied a still. By the third, nobody was keeping records.

### `lore_line_0300`
**Title:** The 0300 Reading  
**Found at:** `loc_weather_wire_house`

A weather observer transmitted a 0300 pressure reading for eighty-three days after the receiving office stopped acknowledging it.

## `ashfall`

### `lore_linekeepers_pair`
**Title:** No One Owns a Pair  
**Found at:** `loc_linekeepers_splice_yard`

The phrase first appears on a fault board after two armed groups both claimed the same cable. The technician repaired it and wrote neither claimant’s name.

### `lore_line_seventeen_answers`
**Title:** Circuit Seventeen  
**Found at:** triggered by first North Basin connection

The first voice from beyond Sector 4 asked for battery polarity before asking for names.

---

# 13. MAIN QUESTLINE — THE LONG LINE

The main arc is twelve quests plus a delayed epilogue. It should feel like repair work accumulating into governance rather than a sequence of boss encounters.

---

## MAIN 01 — `quest_long_line_one_ring`
### “One Ring”

**Trigger:** Day 120+, stable shelter power, storm/thaw or maintenance event  
**Target:** player shelter

A telephone bell rings behind the Allocation 12 service panel.

### Stages
1. Locate the panel by sound.
2. Remove the cover without cutting the cloth-wrapped lead.
3. Read the tag: `ALLOC 12 / EXT 3 / PRIORITY C`.
4. Lift the handset. Hear only line hum and one distant click.
5. Decide whether to leave the circuit connected, isolate it, or test it with shelter power.

### Choices
- **Leave connected:** the line rings again later; no immediate resource cost.
- **Isolate until traced:** safer electrically; Mara later approves the caution.
- **Backfeed a test voltage:** faster clue, minor risk of damaging old protection; can produce an early fault.

### Outcome
Adds `knowledge_key: lore_line_alloc12_extension` and reveals `loc_alloc12_cable_vault`.

### House voice beat
> The handset smells of dust and old rubber. Somebody fitted it for people who never used it. The bell has used itself once.

---

## MAIN 02 — `quest_long_line_pair_twelve`
### “Pair Twelve”

**Target:** `loc_alloc12_cable_vault`

The player traces the shelter drop to the street vault and finds a wet splice.

### Stages
1. Pump standing water below the splice case.
2. Open the lead sheath without cutting neighbouring pairs.
3. Identify Pair 12 by paper tag and test tone.
4. Dry and rewrap the joint.
5. Follow the cable map toward Sector 4 Central Exchange.

### Material tension
Dry paper and paraffin can be spent here or saved for shelter maintenance. A substitute repair works but degrades later.

### Choices
- **Proper splice:** expensive, permanent integrity gain.
- **Field splice:** cheap, immediate access, future fault chance.
- **Take copper instead:** postpones the expansion and yields useful material; the game does not scold the player for being hungry.

### Outcome
Reveals `loc_sector4_exchange` and the Linekeepers’ recent test marks.

---

## MAIN 03 — `quest_long_line_exchange_four`
### “Exchange Four”

**Target:** `loc_sector4_exchange`

The player meets Mara Ell, Sen Ard, and Pele Orin attempting to restore one battery bus.

They rang every labelled shelter circuit in sequence.

Allocation 12 was the first answer.

### Stages
1. Enter the exchange and prove the shelter line is yours by reading the tag code.
2. Inspect the 48V room.
3. Choose how many salvageable cells to dedicate to exchange service.
4. Clean the main frame contacts.
5. Bring one local circuit up.

### Choices
- **Dedicate cells to exchange:** faster network growth; forfeits high-value battery salvage.
- **Split cells:** slower, adequate.
- **Take most cells home:** shelter-first benefit; Linekeepers still cooperate but capacity remains one circuit longer.

### Dialogue hinge
Mara does not ask permission to repair the shelter pair. She asks whether the player wants it listed as **working**.

That is the first moment the expansion makes status itself consequential.

### Outcome
Unlocks Linekeeper access and first local call requests.

---

## MAIN 04 — `quest_long_line_night_board`
### “Night Board”

**Target:** `loc_hospital_night_switchboard`

Sen wants the manual board because the automatic exchange is too damaged for flexible routing.

### Stages
1. Reach the hospital switchboard.
2. Recover cord pairs and a line test set.
3. Find the final traffic slip.
4. Discover the `MATERNITY / OUTSIDE — HELD` entry.
5. Restore the hospital-to-exchange circuit.
6. Complete the first deliberate live call: Verge clinic to shelter medical staff or equivalent available medical endpoint.

### Choice
During the test call, Sen tells the player to come off the monitor key after connect.

- **Come off:** establishes default operator-connect privacy.
- **Keep listening:** reveals harmless medical detail and sets an early monitored-line flag.

No dramatic punishment. Sen notices which lamp is lit.

### Outcome
Unlocks medical consultation calls and `lore_line_eleven_minutes`.

---

## MAIN 05 — `quest_long_line_under_the_bridge`
### “Under the Bridge”

**Target:** `loc_bridge_cable_gallery`

Pair 17 crosses under the Warlords’ bridge. They did not build the cable, but they control the structure holding it.

### Stages
1. Request access below the toll deck.
2. Inspect the split armour joint.
3. Identify old demolition markings so the crew does not disturb them.
4. Negotiate the cost of ongoing maintenance access.
5. Repair the pair or choose a longer flooded reroute.

### Choices
- **Pay a speech toll:** regular food/scrap cost for bridge access; reliable maintenance.
- **Trade information:** Warlords receive bridge/road status bulletins instead of material toll.
- **Reroute through culvert:** no Warlord agreement; higher danger and worse integrity.
- **Invoke Garrison claim:** free short-term access if standing allows; converts a cable dispute into a territorial dispute.

### Outcome
Reveals `loc_carrier_repeater_17`.

---

## MAIN 06 — `quest_long_line_pair_seventeen`
### “Line Clear”

**Target:** `loc_carrier_repeater_17`

The repeater must be powered, aligned, and tested from both ends.

### Stages
1. Restore rectifier output.
2. Replace corroded fuse contacts.
3. Send test tone east.
4. Wait.
5. Hear a return tone.
6. Patch to Sector 4 board.
7. Receive a voice: “North Basin Control. Identify circuit.”

### First conversation
Renna Mere does not deliver exposition. She conducts a line test.

Questions:
- exchange code;
- open-circuit voltage;
- audible noise;
- whether the player can hold the circuit for five minutes tomorrow.

Only after the test does she say:

> “Good. We thought Four was empty.”

### Choices
- **Identify Sector 4 and shelter openly.**
- **Identify only the exchange.**
- **Ask North Basin to identify first.**

All are reasonable. Trust affects how quickly practical information is exchanged later.

### Outcome
Unlocks far-end knowledge exchange. No physical trade route opens.

---

## MAIN 07 — `quest_long_line_weather_wire`
### “The Wire to Weather”

**Target:** `loc_weather_wire_house` and `loc_observatory_patchbay`

North Basin can compare pressure trends if Sector 4 can send a highland reading. The old weather circuit runs through Cult-controlled ground.

### Stages
1. Reach the Weather Wire House.
2. Repair barometer chart drive or recover manual reading procedures.
3. Negotiate access to the observatory patchbay.
4. Take a complete local reading.
5. Send it to North Basin.
6. Receive a crude forecast window in return.

### Choices at the Cult
- **Share raw readings:** Cult receives identical data; interpretation remains theirs.
- **Trade instrument repair:** technical exchange with no doctrinal concession.
- **Sneak patch access:** avoids negotiation; risks future access if discovered.
- **Refuse the site:** expansion continues but weather benefits remain unavailable.

### Outcome
Starts the optional Weather Wire questline and makes forecasts a practical network benefit.

---

## MAIN 08 — `quest_long_line_priority_table`
### “Priority Is a Number”

**Target:** `location_ministry_of_truth_bunker` and/or exchange records

A North Basin operator asks whether Sector 4 still uses Class A seizure because their side has disabled it.

Nobody in the current crew knows what she means.

### Stages
1. Find the emergency operations manual.
2. Read the A0/A1/B/C/U table.
3. Locate Allocation 12 as Class C.
4. Find the event recorder entry for Hour Zero seizure.
5. Find the physical priority key or its duplicate.
6. Compare with the hospital Eleven Minutes record.

### Optional *The List* integration
If the player has discovered the Continuity Allocation Schedule, the same office codes appear in both systems. The expansion confirms that allocation was not merely a bed assignment; it also assigned communications priority.

If the player has not discovered *The List*, the line material foreshadows it without explaining the entire system.

### Choice
Take, copy, seal, or leave the key.

The choice does not resolve ownership yet. It determines who knows the key exists.

---

## MAIN 09 — `quest_long_line_open_board`
### “Open Board”

**Target:** `loc_sector4_exchange`

Once word spreads, everyone wants a circuit.

The board has two working trunks at best and one on a bad day.

### Applicants
Minimum base-game set:
- Verge cooperative;
- Iron Garrison;
- Warlord bridge office;
- Linekeepers;
- Quiet House / medical traffic if available;
- Archivists if discovered;
- one ordinary public window for non-faction calls.

Optional expansion endpoints can be appended without replacing these.

### Task
The player must set a temporary seven-day operating schedule.

### Proposed schedule choices
- **First requested:** simple, inefficient, legible.
- **Emergency first:** requires someone to define emergency.
- **Fixed slots:** each major user gets specific hours.
- **Shelter priority:** player community always preempts.
- **Operator discretion:** Sen/Mara decide in the moment.

### Consequence texture
Nobody immediately declares the policy good or bad. Over the next week:
- queues change;
- missed calls appear;
- some users begin calling at odd hours;
- the Warlords send written notices instead;
- the Verge posts a runner at the phone;
- Garrison operators test preemption codes.

The policy becomes real through behaviour.

---

## MAIN 10 — `quest_long_line_the_key`
### “The Key”

Ivo Kern formally requests custody of the priority key.

His written argument is technically sound: a civil-emergency network without guaranteed preemption can fail during a mass casualty event.

Mara’s response is also technically sound: the previous guaranteed preemption is the reason 184 calls were waiting when the city went dark.

### Stages
1. Hear Kern’s proposal.
2. Hear Mara’s objection.
3. Review actual Busy Hour test capacity.
4. Decide interim custody before the coming weather front.

### Choices
- **Give Kern the original.** Garrison can seize circuits.
- **Keep original; give copy.** Player can counter-seize.
- **Seal it in exchange safe.** Requires two people to open.
- **Break the key.** Physical preemption disabled until rebuilt.
- **Give to Linekeepers.** They will use it only for electrical/network safety, not content priority.

No option is final. MAIN 12 still determines charter, but this choice changes MAIN 11 materially.

---

## MAIN 11 — `quest_long_line_busy_hour`
# “The Busy Hour”

This is the expansion climax.

A pressure fall, river surge, bridge incident, and urban fire occur inside one ninety-minute window. The network is functioning well enough that all four events reach the board.

It is not functioning well enough to connect all four.

### Minimum crisis board

#### Call A — `busy_call_drown_pump`
**Origin:** `loc_pump_alarm_room`  
A surge is arriving. Workers can close a secondary sluice manually if they receive the gate sequence before water reaches the lower room.

**Known:** seventeen people on shift; infrastructure value high; Drown conditions uncertain.

#### Call B — `busy_call_verge_clinic`
**Origin:** Verge party line  
A complicated birth. Lida requests a specific medical consultation and needs a live person, not a written instruction.

**Known:** one patient in immediate danger; shelter medical staff can help if connected.

#### Call C — `busy_call_toll_bridge`
**Origin:** bridge office / Lamplighter relay  
A truck axle has failed across the cable-side lane while a convoy is approaching in low visibility. The road can be closed from the east if the warning reaches the next post.

**Known:** number of travellers uncertain; closing the bridge also interrupts supply movement.

#### Call D — `busy_call_grid_fire`
**Origin:** Grid block line  
A tenement stairwell is burning. The nearest working hydrant is on a manually isolated main. Someone at the water office knows which valve to open.

**Known:** residents present; exact count unknown; Garrison patrol can reach the block if dispatched.

### Mechanics of the scene

- Two circuits are available in the intended balanced state.
- A third may exist if the player completed the hardest Drown cable restoration, but noise makes it unreliable.
- Each call requires time.
- A priority key holder may seize a circuit once, dropping whatever lower-priority call is connected.
- Operators give only information they actually know.
- No timer needs to be real-time; pressure comes from consequences, not interface stress.

### The player decides

Possible actions:
- connect A, B, C, or D;
- ask one caller to hold;
- let Sen choose one line while player chooses the other;
- invoke priority seizure;
- refuse a seizure attempt;
- if three circuits exist, risk the noisy third;
- sacrifice one circuit to keep the network technically stable.

### Consequences

Do not make every unconnected call automatically end in death. That would turn uncertainty into punishment.

Instead each crisis resolves from a probability/state table influenced by prior preparation:
- Pump workers may improvise if earlier flood-alarm maintenance was completed.
- Verge outcome improves if local medical readiness is high even without the call.
- Toll convoy may stop if Lamplighter coverage is strong.
- Grid fire may be contained if Garrison trust/response is high.

The network changes odds and information; it does not become fate.

### After the hour

Sen writes four lines in the traffic ledger.

For each:
- origin;
- connect or not connected;
- time;
- clear time;
- no content.

The player gets outcomes later through messengers, calls, or world-state notes.

Nobody at the board gives a speech.

---

## MAIN 12 — `quest_long_line_charter`
# “Who Gets the Line”

The Busy Hour proves that temporary rules are no longer enough.

The player chooses an operating charter. This is an expansion ending state, **not a game victory path**.

## Charter A — `line_charter_open_board`
### Open Board
Any recognized station may request a call. Queue is chronological except for a narrow life-safety interrupt requiring two operators to agree.

**Benefits:** broad access, strong civilian use, high Linekeeper support.  
**Costs:** player loses guaranteed priority; high queue load; some strategic calls wait.  
**World texture:** people begin carrying written call requests with preferred times.

## Charter B — `line_charter_shelter_priority`
### Allocation 12 Priority
The player shelter keeps first seizure right on one circuit.

**Benefits:** strongest direct survival utility; emergency shelter coordination.  
**Costs:** other users treat the line as player property; some Currents avoid sensitive calls.  
**World texture:** the phone in the command room rings less often because people route around you.

## Charter C — `line_charter_fixed_slots`
### The Timetable
Each Power/Current/community gets allocated windows, published seven days ahead.

**Benefits:** predictable and politically legible.  
**Costs:** unused minutes cannot always be reclaimed; emergencies create constant exception pressure.  
**World texture:** people arrive at phones early and wait for their minute.

## Charter D — `line_charter_garrison_priority`
### Command Net
The Garrison receives A1 seizure authority and maintains guards, batteries, and repair escorts.

**Benefits:** best physical security and power stability.  
**Costs:** monitored/strategic use; Militia and some Currents reduce participation; ordinary calls are lower class again.  
**World texture:** every call begins with an operator identification code.

## Charter E — `line_charter_toll_service`
### Metered Line
Warlords administer bridge/repeater access and charge by completed call or reserved slot.

**Benefits:** stable material funding; fewer frivolous calls; freight coordination improves.  
**Costs:** people without trade goods become effectively mute; the Quiet House cannot always pay.  
**World texture:** a mechanical counter clicks before the first ring.

## Charter F — `line_charter_linekeeper_custody`
### Technical Custody
Linekeepers own no content rights but control switching, repair, and emergency break-in according to their five rules.

**Benefits:** high neutrality and network health; sensitive Currents keep using it.  
**Costs:** the player cannot order surveillance or preferential access; Linekeepers may deny a politically important preemption they judge non-emergency.  
**World texture:** the player must request calls like everyone else.

---

## EPILOGUE — `quest_long_line_aftertone`
### “Aftertone”

Fourteen days later, a short scene reflects the charter without ranking it.

- **Open Board:** a queue card hangs beside the shelter phone. Somebody has written `CALL MOTHER / NORTH BASIN?` and then crossed out the question mark.
- **Shelter Priority:** the handset is in a locked cabinet. The key is on the player’s ring.
- **Fixed Slots:** the wall has a timetable. Somebody has circled 03:20 because it is the only private slot left this week.
- **Garrison Priority:** the phone rings once, stops, then rings twice in the command pattern.
- **Toll Service:** a small counter advances before the bell sounds.
- **Linekeeper Custody:** the shelter phone has moved out of the command room and onto a table near the airlock, where the operator can reach it without asking permission.

North Basin still answers if the physical line remains intact.

---

# 14. COMPANION QUESTLINE A — THE NAMES BETWEEN STATIONS

**Primary systems:** Archivists, located knowledge, *The List* optional integration  
**Tone:** verification, names, distance, the difference between missing and dead

This line becomes available after Pair 17 is restored and the Archivists are known.

## NAMES 01 — `quest_names_dead_number`
### “Not Answering”

The Archivists bring six names from records that end at Sector 4’s boundary. North Basin may have corresponding pages.

The Linekeepers correct the quest title in dialogue: a number is not dead. It is not answering.

**Task:** read six names, dates, and last known locations over the line exactly as written.

**Choice:** allow operator recording or require live transcription only.

**Reward:** first cross-region corroboration entry.

## NAMES 02 — `quest_names_second_voice`
### “Second Voice”

One name belongs to somebody whose death is known only from a single witness. The Archivists require a second source.

North Basin has an elderly pump mechanic who knew the same person under a shortened name.

The player must establish that the two names refer to one individual using occupation, a hand injury, and a workshop location — not sentiment.

**Outcome:** verified record; small Archivist access increase.

## NAMES 03 — `quest_names_six_cards`
### “Six Cards”

North Basin sends six Continuity card numbers by voice. One corresponds to a Sector 4 record marked `UNLOCATED`.

Optional cross-flag: if Sela Renn / Allocation 12 content exists, one card can reference the route that passed 12-B without rewriting Sela’s outcome.

**Choice:** copy all six into local records or only those that can be independently verified.

The Archivists accept only verified entries. Renna is mildly irritated by the delay and then agrees with it.

## NAMES 04 — `quest_names_living_entry`
### “Present Tense”

A person listed in Sector 4 records as presumed dead answers the far-end phone.

No dramatic reveal music.

They ask why their old address is being read aloud.

The Archivists must decide whether a living person belongs in a memorial index. Their rules do not cover it.

**Choices:**
- move the entry to living register;
- leave memorial record with correction note;
- let the person choose how their name is held.

All three create different Archivist dialogue. None affects survival stats heavily.

## NAMES 05 — `quest_names_the_correction`
### “One Letter”

A surname in the Continuity Schedule differs from the far record by one letter. The two documents cannot both be literally correct.

The player searches a physical source in Sector 4 — an employment card, school register, clinic label, or nameplate depending discovered content.

The correct spelling turns out to be the one written by hand, not the printed central record.

**Theme:** records matter because people are real; records do not become real by being official.

## NAMES 06 — `quest_names_read_it_back`
### “Read It Back”

The Archivists and North Basin agree to exchange a verified list of the living, missing, and dead.

The transfer takes hours over a noisy line.

Names are read one at a time and repeated back.

The player chooses how much circuit time to allocate:
- complete exchange over several nights;
- priority names only;
- suspend because other traffic is waiting.

The quest reward is not loot. It adds durable knowledge entries and can later cause rare “corroborating witness found” events for survivor histories.

---

# 15. COMPANION QUESTLINE B — THE WEATHER WIRE

**Primary systems:** weather, expeditions, Sun-Seekers optional, Crop/Verge consequences

The weather system becomes more useful without becoming perfectly predictable. The network provides **lead time**, not certainty.

## WEATHER 01 — `quest_weather_the_drum`
### “The Drum”

Recover and restore the pressure chart drum at `loc_weather_wire_house`.

A previous observer continued replacing paper after nobody acknowledged receipt.

**Choice:** take the preserved charts as lore or reuse the last clean roll for current recording.

## WEATHER 02 — `quest_weather_upper_wind`
### “Upper Wind”

The observatory instruments can provide wind direction above the lower ash layer, but the Cult controls access.

The player obtains a reading by trade, shared repair, or covert access.

The reading improves ash-front timing.

## WEATHER 03 — `quest_weather_0300`
### “The 0300 Reading”

A forecast is only useful if measurements are regular.

For seven days, somebody must take the 0300 reading.

This can integrate with Duty Roster if present or run as a simple labour/stamina cost.

Missing one night does not fail the line. It widens forecast uncertainty.

## WEATHER 04 — `quest_weather_false_spring`
### “Thin Sky”

The combined Sector 4 / North Basin pressure and opacity data predicts a brief thinning of the ash layer.

If Sun-Seekers content is active, they independently identify the same window.

The player chooses whether to publish the forecast to all connected stations, reserve it for shelter expeditions, or share only hazard warnings without precise timing.

**Tradeoff:** information shared broadly saves strangers and also puts more people on the same roads and salvage sites.

## WEATHER 05 — `quest_weather_the_missed_warning`
### “Forecast Error”

Sooner or later, a forecast is wrong.

The network predicted a front six hours late because one highland reading was contaminated by instrument icing.

The expansion must include this quest so the line never becomes omniscient.

Task: trace the error, repair the instrument shelter, and decide how to report uncertainty in future bulletins.

## WEATHER 06 — `quest_weather_publish`
### “The Bulletin”

Final weather-line policy:
- raw data open to all;
- hazard bulletin only;
- shelter-first forecast;
- subscriber/trade access;
- operator discretion.

This policy is subordinate to the main Line Charter but changes travel and faction reactions.

---

# 16. COMPANION QUESTLINE C — A PRIVATE LINE

**Primary systems:** survivors, trust, Tally, Quiet House, privacy policy  
**Core question:** does restoring private speech also restore the right to keep it private?

## PRIVATE 01 — `quest_private_extension_three`
### “Extension Three”

Survivors ask why the only working telephone is in the command area.

The player can install an extension near the common room, near the airlock, or leave the phone controlled.

Placement changes who can request/overhear calls.

## PRIVATE 02 — `quest_private_collect_call`
### “Read It Again”

If the Tally is active, a collector requests a call to someone inside the shelter regarding a valid written debt.

The Tally offers to read the contract aloud over the line.

Choices:
- connect privately;
- require the call be public;
- refuse access;
- settle the debt before connecting.

The Tally does not threaten the operator. They note the refusal as inability to contact.

## PRIVATE 03 — `quest_private_wrong_number`
### “Wrong Number”

A survivor answers a call intended for someone who occupied their pre-war address.

The caller is not a relative. They are an old neighbour trying numbers from memory.

The conversation can end there, or the player can spend line time tracing whether the intended person appears in local records.

The reward is a mundane connection: a street remembered by two people who left it in different directions.

## PRIVATE 04 — `quest_private_listen_in`
### “Come Off the Key”

Someone asks the player to monitor a call.

Possible requester depends on world state:
- Garrison suspects route information is being passed;
- a survivor fears a family member is arranging to leave;
- Warlords suspect tariff evasion;
- the player has a genuine security reason after a prior betrayal.

Choices:
- listen;
- refuse;
- disclose monitoring before connection;
- allow Sen to connect and then physically leave the room.

Listening may reveal useful information. It also changes who trusts the line for later personal quests.

No universal morale penalty. Consequences are relational and specific.

## PRIVATE 05 — `quest_private_five_minutes`
### “Five Minutes”

The Quiet House has a resident with one requested call. A distant relative has been located through North Basin records.

The call is scheduled for five minutes because another station is waiting.

The player may extend it, hold to schedule, or surrender another reserved slot.

The content of the call is never shown word-for-word if the privacy policy is intact.

The player sees only Sen’s ledger:

`QUIET HOUSE → NB17 / CONNECT 21:04 / CLEAR 21:13`

Nine minutes.

Sen does not explain why.

## PRIVATE 06 — `quest_private_no_answer`
### “No Answer”

A standing weekly call to one far station goes unanswered three times.

The player can:
- keep reserving the slot;
- release it;
- ask other far stations for information;
- send an emergency ring despite no known emergency.

Eventually the reason is discovered or remains uncertain based on other network state.

Possible grounded outcomes:
- operator ill, recovered later;
- local battery failure;
- station evacuated after flood;
- no confirmed explanation.

The quest is allowed to end without closure.

---

# 17. Optional maintenance quest chain — COPPER AND GROUND

This is a lower-narrative, higher-exploration chain that gives the location set mechanical weight.

## `quest_copper_dry_joint` — “Dry Joint”
Repair Pair 12 correctly after a temporary splice begins humming during thaw.

## `quest_copper_bridge_gallery` — “Below the Toll”
Replace armour bonding under the bridge without disturbing marked demolition circuits.

## `quest_copper_salt_in_paper` — “Salt in the Paper”
A Drown cable has conductive saltwater in its paper insulation. Decide whether to cut back ten metres of usable copper to reach dry material.

## `quest_copper_ground_test` — “To Ground”
Restore the Highland Ground Bed and verify surge protection before a storm.

## `quest_copper_second_circuit` — “Another Pair”
Recover enough of the West Exchange frame to create a second stable simultaneous call circuit.

Completing all five raises maximum network reliability but never eliminates Busy Hour scarcity.

---

# 18. Twelve side quests / dilemmas

## 1. `quest_line_number_not_answering` — Number 4-2217
A phone rings in an empty house. Verify the circuit, then decide whether to keep allocating maintenance to a number nobody answers.

## 2. `quest_line_party_line` — Two Shorts, One Long
Verge households accuse each other of tying up the shared line. The technical fix is another pair; the social fix is a calling schedule; the cheap fix is telling them to live with it.

## 3. `quest_line_toll_on_speech` — Completed Call
The Warlords propose charging only for completed calls rather than bridge access. It sounds fair until failed calls become free and therefore unmaintained.

## 4. `quest_line_the_bell_in_school` — School Bell
An old school office telephone starts ringing after a cross-connect. Children in a nearby shelter use it to call the same time every day because they were told phones require schedules.

## 5. `quest_line_no_message_taken` — No Message Taken
A caller asks Sen to pass on a personal message. He refuses because the line rule is connect or do not connect; he is not the recipient. The player may overrule him.

## 6. `quest_line_wet_weather` — Hum in Wet
A fault only appears during thaw. Repair requires spending a whole day waiting for rain rather than adventuring.

## 7. `quest_line_call_before_trade` — Before You Send the Cart
A trader wants a confirmation call before sending food. Successfully connecting prevents a wasted caravan but consumes scarce morning capacity.

## 8. `quest_line_the_alarm_that_works` — Alarm Window Three
A flood alarm repeatedly reports high water because its float is stuck. The easiest fix is to disconnect the nuisance circuit. The better fix requires travel into the Drown.

## 9. `quest_line_names_on_the_board` — Initials Only
Some users do not want full names written in the call queue. Decide whether the public board records names, station only, or numbered tokens.

## 10. `quest_line_battery_for_voice` — Fifty-Two Point Eight
The exchange needs intact cells while the shelter also needs batteries. There is no special “communications battery” resource; it is the same lead and acid everyone needs.

## 11. `quest_line_one_more_minute` — Hold the Circuit
A call runs past its slot while another caller waits. The operator asks the player once: cut it or let it run.

## 12. `quest_line_test_every_sunday` — Ring Test
Mara insists every working station perform a weekly ring test even when nobody has anything to say. Skipping tests saves labour until the day a line fails silently.

---

# 19. Recurring encounters once the network exists

These are not full quests. They make the line feel inhabited.

### `enc_line_wrong_station`
A caller reaches the wrong board and apologizes twice because they know every connection costs battery.

### `enc_line_breathing_only`
A connection establishes, nobody speaks, then clears. Could be fault, fear, or wrong number. Never resolved unless tied to another quest.

### `enc_line_child_ring_test`
A child at a Verge station performs the prescribed ring test exactly, including reading the date twice.

### `enc_line_garrison_code_test`
Garrison checks whether old preemption tone still works. Whether the player allows the test matters more than the content.

### `enc_line_archivist_spelling`
Archivists spend six minutes verifying one surname letter.

### `enc_line_quiet_house_request`
Quiet House asks whether a specific station is answering today. They do not say why.

### `enc_line_warlord_freight_notice`
A freight notice tells you a road will be occupied. This is useful intelligence delivered with no threat.

### `enc_line_far_weather`
North Basin reports pressure and says nothing else because there is a queue on their side too.

### `enc_line_silence_after_connect`
Two people asked to be connected and then neither begins. Sen comes off the key.

### `enc_line_clear`
A caller says “clear.” The line goes quiet. The operator pulls the cords. Nothing else happens.

---

# 20. New items and physical props

These are proposed content hooks, not a request to create duplicate item authorities.

| Proposed id | Name | Narrative use |
|---|---|---|
| `item_field_telephone` | Field Telephone | portable line test / expedition communication |
| `item_magneto_generator` | Hand Magneto | ring-current test without exchange power |
| `item_line_test_set` | Lineman’s Test Set | identify pairs, faults, battery polarity |
| `item_cable_splice_sleeve` | Lead Splice Sleeve | permanent wet-joint repair |
| `item_dry_cable_paper` | Dry Cable Paper | restores paper-insulated trunk sections |
| `item_splice_paraffin` | Cable Paraffin | waterproofing and insulation |
| `item_porcelain_arrester` | Porcelain Line Arrester | storm protection |
| `item_48v_rectifier_module` | 48V Rectifier Module | powers repeater/exchange from local source |
| `item_carrier_filter_can` | Carrier Filter Can | long-distance voice circuit repair |
| `item_patch_cord_pair` | Switchboard Cord Pair | manual board capacity |
| `item_exchange_fuse_wire` | Exchange Fuse Wire | low-cost recurring maintenance |
| `item_cable_map_sector4` | Sector 4 Cable Map | reveals technical routes / fault sites |
| `item_priority_seizure_key` | Priority Seizure Key | plot object; custody matters |
| `item_traffic_ledger` | Traffic Ledger | metadata only, not call content |
| `item_ring_code_card` | Party-Line Ring Card | Verge local network prop |

No item should be treated as magical communications loot. The important objects are heavy, ordinary, and often more valuable as scrap than as parts.

---

# 21. Rewards that fit the game

The expansion should reward information and coordination more than raw stat inflation.

## Route bulletins
Connected Toll / Lamplighter stations can reduce uncertainty on selected travel routes: closures, ice, bridge status, convoy occupancy.

## Weather lead time
Restored Weather Wire produces earlier hazard windows, not perfect prediction.

## Remote consultation
Medical, mechanical, or agricultural stations may provide one-time procedural advice. They cannot send absent medicines or tools.

## Corroboration
Archivist and survivor-history quests can use distant witnesses for records previously impossible to verify.

## Call-before-travel
Some expedition or trade tasks can be confirmed before committing travel time, reducing wasted journeys.

## Emergency dispatch
A working line can sometimes bring a local faction, clinic, pump crew, or repair team into an event before the player arrives.

## Costs
- battery power;
- maintenance labour;
- copper and insulation;
- queue conflict;
- political pressure;
- privacy decisions;
- users who become dependent on a service the player may later be unable to sustain.

---

# 22. Cross-expansion hooks

All hooks are optional reads. The Long Line must remain coherent with none of them active.

## Expansion 01 — The Holdfast
The line can carry ice-road opening notices, desalination plant status, and Holdfast administrative traffic. It does not reopen or relocate Holdfast sites.

A call before crossing can confirm whether the current window remains open. Weather can still close it after departure.

## Expansion 02 — The Duty Roster
Adds proposed duties:
- `line_watch`;
- `switchboard_operator`;
- `cable_crew`;
- `weather_reader_0300`.

Fatigue and competence should matter, but no survivor becomes a telecommunications wizard from one assignment.

## Expansion 03 — The Standing Record
Traffic metadata can prove that a call occurred at a time, but not what was said. This is an explicit evidentiary rule.

A monitored call may become stronger evidence at the cost of trust/privacy.

## Expansion 04 — Nobody’s Charter
The line becomes a natural test of civic membership: does access depend on citizenship, shelter contribution, residence, faction, or simple physical connection?

Do not force one answer. Let the Charter content argue with the Line Charter content.

## Expansion 05 — The Year of Ash
Weather and faction-conflict bulletins become valuable. The line can carry ceasefire logistics without becoming a peace machine.

A working line may prevent one patrol from blundering into another. It cannot end a war.

## Expansion 06 — The Muster
Military mobilization creates pressure for dedicated command capacity. Garrison asks for predictable priority rather than generic goodwill.

## Expansion 07 — The Dose / The Vigil
Medical and dosimetry consultation can be relayed. Distant experts can tell the player what a reading means; they cannot reduce the reading.

## Expansion 08 — The Verdict
Call ledgers become evidence about timing and contact. Content is inadmissible unless monitoring/recording was disclosed or the player deliberately violated privacy.

This is fertile material for procedural conflict without turning the phone into a truth machine.

## Expansion 09 — The Black Flotilla
River cable landing, ice reports, tide observations, shore-party check-ins, and distress-call windows integrate naturally.

The line ends at shore infrastructure. It is not underwater radio.

## Expansion 10 — The Silent Foundry
The Foundry can use scheduled dispatch slots for road iron, brine pipe, labour, maintenance, and trade coordination.

Foundry productivity may improve because fewer loads arrive on the wrong day, not because the telephone produces metal.

## Lore spine — The List
Allocation 12’s Class C line confirms that Continuity selected not only shelter occupancy but communications priority.

If Sela/Allocation 12 outcomes exist, use them carefully as optional character echoes. Do not retcon them.

## Currents
- **Archivists:** corroboration and names.
- **Lamplighters:** route bulletin endpoints; universally protected repeater access.
- **Quiet House:** final calls and family tracing.
- **Grain Exchange:** market board call-ins can improve rate freshness.
- **Tally:** lawful remote debt contact creates privacy dilemmas.
- **Sun-Seekers:** False Spring forecast can alter their gathering window.
- **Undertow / other dangerous Currents:** may seek line access without receiving territorial status.

---

# 23. Lore props, echoes, and inspect text

These short pieces are intended for `echoes`, inspect text, journal, or environmental props.

## `echo_line_01` — Test Pencil
> `PAIR 12 — RING OK / SPEECH LOW / RETEST BEFORE OCC.` The second test box is empty.

## `echo_line_02` — Battery Wall
> Inspection dates continue for eleven days. The handwriting gets larger near the end, not worse.

## `echo_line_03` — Queue Pegs
> One hundred eighty-four wooden pegs remain in the waiting board. None has a name.

## `echo_line_04` — Hospital Cord
> The plug is still seated. The cloth cord has a pale clean bend where a hand held it down.

## `echo_line_05` — Party Line Card
> `TWO SHORT — HARK FARM. LONG-SHORT-LONG — MILL.` Someone added `DON’T LISTEN` in pencil. Someone else crossed it out.

## `echo_line_06` — Repeater Thermos
> The thermos is empty. A rag is stuffed where the lid should be. The rag is clean.

## `echo_line_07` — Fault Board
> `RINGS / NO SPEECH` has been underlined twice. The fault is more specific than most surviving maps.

## `echo_line_08` — Priority Manual
> The table fits on one page. Somebody laminated it so it would remain readable during emergencies.

## `echo_line_09` — Directory Drawer
> The directory lists a bakery, a dentist, a flood office, and three shelters on the same page. Only the shelters were assigned priority.

## `echo_line_10` — Ringing House
> The bell sounds too loud in an empty hallway. Dust jumps on the metal gong every time it strikes.

## `echo_line_11` — North Basin Note
> Renna asks you to repeat one number. You do. She says, “That is what I had.” Paper moves at the far end.

## `echo_line_12` — Aftertone
> When a long circuit clears, a faint tone remains for half a second. Sen waits for it before pulling the cord.

---

# 24. Character-specific story beats

## Mara Ell — “Certify Working”
Mara’s trust progression is not about liking the player.

She cares whether the player makes true statements about infrastructure.

High access actions:
- report faults accurately;
- admit temporary repairs are temporary;
- refuse to mark a circuit “safe” without test;
- publish outages even when politically inconvenient.

Access-losing actions:
- claim a line is unavailable to block a caller;
- order Linekeepers to fake a fault;
- conceal a wet splice;
- repeatedly demand eavesdropping.

At maximum trust she gives the player her outside-plant notebook.

It contains no diary.

It contains cable depths, sheath types, and the names of two technicians who are almost certainly dead.

## Sen Ard — “Come Off the Key”
Sen’s arc tests privacy.

If the player repeatedly monitors calls, Sen remains professional but stops handling private-call requests. Another operator takes the board; personal quest frequency falls.

If privacy is preserved, Sen eventually asks for one call of his own.

He gives a number from memory.

It does not answer.

He does not ask to try again that day.

## Pele Orin — “Not Answering”
Pele begins by calling silent endpoints dead.

Across repairs he learns the operator vocabulary:
- open;
- short;
- grounded;
- ringing;
- busy;
- not answering.

Late game, after a far station misses three calls, another survivor says, “They’re gone.”

Pele replies:

> “Not answering.”

No further line is needed.

## Renna Mere — “Too Far”
Renna never offers rescue or migration.

At high trust she gives the player North Basin’s actual headcount only because a supply-planning calculation requires it.

At low trust she gives only station status.

If the line goes down permanently, her final confirmed state remains whatever the last completed call established. No epilogue invents her fate.

---

# 25. Faction reactions to the restored line

These should use trust-reactive prose where possible.

## Iron Garrison
**High trust:** calls it a civil-defense asset under cooperative command.  
**Low trust:** calls it an unsecured strategic circuit and demands inspection.

The technical request is nearly identical. The wording changes.

## Ash Militia / Verge
**High trust:** volunteers operators and publishes farm ring hours.  
**Low trust:** uses runners for sensitive messages and the phone only for weather.

## Warlords
**High trust:** offers bridge access at a stated rate and honours it.  
**Low trust:** still offers access, at a worse rate, because a functioning line increases toll traffic.

## Cult of the Ash Sign
**High trust:** allows instrument data through with ritual labels attached.  
**Low trust:** withholds observatory patch access but still accepts emergency weather warnings.

The game never validates their interpretation of the readings.

---

# 26. The line charter is not a morality ending

Every ending must remain playable and defensible.

### Open Board can fail people
A chronological queue is fair until an emergency arrives after somebody has already waited three hours.

### Shelter Priority can save the player community
A player who has kept twenty people alive for months has a rational reason to preserve guaranteed access.

### Fixed Slots can waste capacity
Predictability has a cost when an empty station owns a quiet hour another station needs.

### Garrison Priority can maintain infrastructure
Armed escorts and fuel matter. So does surveillance.

### Toll Service can finance repairs
A paid service may survive longer than a voluntary one. People without payment still exist.

### Linekeeper Custody can become technocracy
The people who understand the network end up deciding what counts as an emergency, even if they never wanted political authority.

No ending card should call one charter “good,” “bad,” “free,” “authoritarian,” or “optimal.”

Show what the phone does under that rule.

---

# 27. Failure states that create story instead of blocking content

## If the player sells the first batteries
The line remains a one-circuit local service longer. Main quests delay, but salvage was genuinely useful.

## If Linekeeper access is lost
The player can still maintain limited circuits using labour/resources, but faults are more frequent and technical choices are less forgiving.

## If the priority key is destroyed
The network still works. Physical seizure is unavailable. Busy Hour must be managed without it.

## If Pair 17 is never restored
The main arc can conclude as a local Sector 4 network, but North Basin, Names Between Stations, and some weather content remain absent. The player is not forced to discover every miracle of infrastructure.

## If privacy is repeatedly violated
Sensitive personal calls dry up; military/commercial users may increase. The network remains useful and becomes a different kind of place.

## If the Drown second circuit is skipped
Busy Hour is harsher. The player can still complete the story.

---

# 28. Recommended quest flag vocabulary

Proposed only; implementation should reconcile against existing flag authority.

```text
long_line_discovered
long_line_pair12_repaired
long_line_exchange_online
long_line_hospital_board_online
long_line_bridge_access_paid
long_line_bridge_access_bulletin_trade
long_line_pair17_online
long_line_north_basin_contact
long_line_weather_online
long_line_priority_key_found
long_line_priority_key_garrison
long_line_priority_key_sealed
long_line_priority_key_destroyed
long_line_privacy_connect_only
long_line_privacy_monitored
long_line_busy_hour_complete
line_charter_open_board
line_charter_shelter_priority
line_charter_fixed_slots
line_charter_garrison_priority
line_charter_toll_service
line_charter_linekeeper_custody
linekeepers_access_withdrawn
weather_bulletin_open
weather_bulletin_shelter_first
names_exchange_complete
```

Avoid creating parallel aliases for existing faction IDs. The expansion should consume whatever canonical faction-ID reconciliation eventually lands.

---

# 29. Proposed data files if approved for implementation

Do not add all of these merely because this creative pack exists. They are a conversion map.

```text
Assets/StreamingAssets/Data/long_line_locations.json
Assets/StreamingAssets/Data/long_line_quests.json
Assets/StreamingAssets/Data/long_line_stations.json
Assets/StreamingAssets/Data/long_line_calls.json
Assets/StreamingAssets/Data/long_line_items.json
Assets/StreamingAssets/Data/long_line_lore.json
Assets/StreamingAssets/Data/long_line_charters.json
```

Potential Core domain:

```text
Assets/Ashfall.Core/LongLine/
  LongLineSystem.cs
  LongLineState.cs
  LineStationCatalog.cs
  CallQueueSystem.cs
  LineIntegritySystem.cs
  LongLineQuestSystem.cs
  LongLineSaveState.cs
```

Potential host surfaces:

```text
src/Host/LongLineHostSession.cs
src/Host/LongLineSaveStore.cs
src/UI/LongLinePanel.cs
```

Suggested selftest eventually:

```text
--long-line-selftest
```

But the architectural rule from the repo audit still applies: do not widen `Main.cs` blindly. If implemented, this expansion should attach through the existing expansion/session composition pattern or whatever succeeds the current Main decomposition work.

---

# 30. UI concept — one board, not another dashboard

The primary UI should resemble a practical switchboard / traffic board rather than a strategy map.

## Core surfaces

### Circuit lamps
- circuit free;
- ringing;
- connected;
- noisy/faulted;
- down for test.

### Queue cards
Show:
- origin station;
- requested destination;
- request time;
- emergency claim if any;
- no hidden moral score.

### Network map
A simple line diagram across existing regions, not a new territory map.

Nodes light only when tested from both ends.

### Traffic ledger
Origin, destination, connect time, clear time, completion status.

**Never default to transcript logging.**

### Charter card
One concise statement of current access/preemption policy.

The most important UI interaction in Busy Hour is physically deciding which waiting plug gets which available jack.

---

# 31. Audio / presentation language

If VO is available, use it sparingly.

The expansion’s strongest sounds are non-verbal:

- mechanical bell;
- relay chatter;
- hum on a wet line;
- plug click;
- hand magneto whirr;
- faint far-end sidetone;
- pencil on traffic card;
- line-clear click;
- silence after a dropped circuit.

Do not add ghost voices, supernatural number stations, or unexplained prophetic broadcasts. A strange sound should have an electrical or human cause even if the player never discovers which one.

---

# 32. “Do not write it this way” guardrails

- Do **not** make the far-end settlement a promised utopia.
- Do **not** turn Renna into a quest vending machine who knows everything.
- Do **not** make the line secretly controlled by an AI.
- Do **not** reveal that every pre-war call was recorded.
- Do **not** turn phone privacy into a simple morality bar.
- Do **not** create a fifth territorial faction for the Linekeepers.
- Do **not** give perfect weather prediction.
- Do **not** let remote experts conjure missing supplies.
- Do **not** make the Priority Table a secret evil conspiracy. It was ordinary emergency planning.
- Do **not** let the player solve Busy Hour by having repaired everything; scarcity must remain.
- Do **not** invalidate radio. Radio remains better for broadcast and wide-area warnings; the line is better for private, verified, point-to-point exchange.
- Do **not** make a failed call mean the person died.
- Do **not** use “dead line” when the actual state is merely “not answering” unless the speaker is meant to be imprecise.

---

# 33. Why the expansion ends where it does

The Long Line should not end with every station connected.

A complete network would undermine the setting.

It should end when the player has enough working infrastructure that the central problem changes from **can we connect** to **what rules govern connection**.

That is the campaign-scale achievement.

The wires remain old.

The bridge still ices.

The battery plates still shed lead.

North Basin still sometimes fails to answer.

The weather still arrives early.

But the shelter now has a phone that can ring because another person, somewhere else, has decided to ring it.

---

# 34. Closing scene — canonical tone target

Late night. Shelter quiet.

The line is working.

The bell rings.

Who is allowed to answer depends on the charter the player wrote.

Somebody answers.

The text box does not identify the caller immediately.

> “Sector Four?”

A pause.

> “Good. We only needed to know the line was up.”

The circuit clears.

The operator writes the time in the ledger.

Nothing is added to inventory.

The line remains available.
