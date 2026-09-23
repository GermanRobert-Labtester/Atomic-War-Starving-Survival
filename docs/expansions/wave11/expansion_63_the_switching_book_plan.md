# ASHFALL — Expansion 63 Design Bible
# THE SWITCHING BOOK
## Current Is Not a Promise

**Wave:** 11 — The Work Between Storms  
**Expansion:** 63  
**Document status:** Content-first design bible; proposal only, not an integration claim or authorization.  
**Target length:** approximately 120,000 characters, including authored in-world material.  
**Primary site:** Electrical Maintenance Exchange, Metro Service Ring  
**Anchor:** loc_electrical_maintenance_exchange  
**Narrative mode:** expedition, utility records, conflicting witness accounts, public explanation.

---

## 0. Purpose and reading contract

The Electrical Maintenance Exchange is a place full of equipment that looks like it should still work. Switchgear consoles, copper cable, batteries, transformers and relay racks offer the familiar promise of salvage. The player is sent there for something specific or because the map identifies it as a supply site. The story begins when a paper switching schedule says the lower service line was open, while the visible isolator is tagged shut. Both records have dates. Neither explains the gap.

**The Switching Book** is a content-first expansion about the human meaning of a changed power order. It does not add a regional electricity simulation, new grid-routing interface, substation repair mechanic or external customer ledger. The player investigates a closed industrial room, compares a dispatcher’s schedule with the field crew’s notes, and decides how to record an event that affected people who can no longer provide a complete account. The current power system remains the Holdfast’s authority. This location's old switchgear is historical evidence and salvage, not a second grid.

The story follows Eda Venn, a proposed line worker whose name first appears in Expansion 62 as “A. Venn.” She worked the north service route with Runa Dey and the forestry crew. The route was meant to keep one clinic room, one communications relay and a winter crossing available during a period of severe weather. The routing plan could not keep all three powered. Someone opened a feeder that was supposed to remain isolated. Someone later closed it. The surviving book says the choice was deliberate but does not say who gave the order. A field note says the line went dark before the crew received the correction. The player learns that the record is not a hidden confession; it is an incomplete account of a decision made with incomplete information.

The package gives the player a practical decision: whether to carry a verified switching schedule back to the Holdfast, whether to report a worker by name when the evidence only supports a shift role, and whether to publish a regional explanation that will be repeated by people who did not witness the event. The material reward is secondary. The player may recover a battery or cable only where a current item and expedition consumer permits it. The story should remain useful if the player takes nothing.

### Writing position

Electricity is made legible through sound, heat, dust, labels and the sequence of a hand operating a lever. Do not turn the switchgear into a mysterious machine or a minigame. The control labels are plain and partly faded. One lever has a wear mark from a glove thumb. A paper strip catches under a cabinet foot. The radio operator says “the carrier is gone,” not “the station is silent.” The player is allowed to understand the physical task without being asked to learn real high-voltage procedures. No live electrical action is a safe player puzzle.

The authored material in this bible is intended to be adapted into current quest, location, radio, journal, encounter or narrative-document forms after a live source/data audit. This is not a request to create every proposed text as a separate catalog row. The package must not ship more words than the existing runtime can reach responsibly.

---

## 1. Expansion thesis

**The Switching Book** adds a playable record dispute around a small utility decision. It gives narrative weight to an existing electrical destination and makes a previous expedition story consequential without turning infrastructure into an engineering feature list.

The player investigates a failed winter service line. Three needs competed for power: a clinic’s warm room, a relay used to coordinate the route, and an exchange feeder supplying a station with a manual transfer capability. The emergency schedule contains a priority order. The field book contains an actual switching sequence. A later report treats that sequence as a breach of procedure. A return note says the operator followed an oral instruction that never reached the ledger.

The player cannot prove every part of the story. The outcome is therefore not “solve who was right.” It is “decide what the Holdfast can honestly say and what it can responsibly repeat.” The player can share a confirmed sequence, keep the operator’s name out of a public report, leave the disputed page sealed at the exchange, or use the schedule to plan a present-day expedition without repeating the old route’s mistake.

### Experience added

1. **Reading a switching order as a work record.** The player compares planned loads with actual lever positions and finds that a sequence can be correct even if its date is wrong.
2. **Distinguishing instruction from receipt.** A notice can be issued without reaching the person who must act. The story makes that gap physical: a carbon copy remained clipped behind a door after the line was already changed.
3. **Choosing how precise to be in public.** The player can name a shift, a station, a time window or an individual. Each choice can carry a different burden of proof.
4. **Using old information without treating it as current authority.** The recovered book may help the Holdfast prepare, but it is not a modern operations manual and cannot drive the current power grid.

### Signature image

The central image is a switching book held open by a small battery so that the pages do not close in the draft. The battery is no longer charged. The book is. A paper clip has rusted through the cover and left one red crescent beside the notation “lower line—hold.” When the player lifts the battery, the page moves in the wind. The notation stays where it was.

### What the player risks

The expedition takes time and exposes the party to a hazardous site. The report may affect how a living contact speaks about the old utility crew. The player may bring home useful material at the expense of leaving the evidence at the exchange. A public account may become a rumor that outruns its caveats. The game should show these stakes before the player chooses; it should not hide a major consequence behind a single ambiguous line.

---

## 2. Current evidence and canon fit

### 2.1 Current local evidence checked for this design

- Assets/StreamingAssets/Data/locations.json defines loc_electrical_maintenance_exchange as a primary substation switching vault and line-maintenance dispatch terminal. It names switchgear consoles, high-voltage transformers, copper cabling and relays; the row gives dangerLevel 5, travelHours 2.5 and baseRadsPerHour 25.
- Assets/StreamingAssets/Data/wasteland_map_v1.json lists the destination in the Metro Service Ring. Existing route edges connect it to the Broadcast Bunker Echo; the destination is a map node within the current route graph.
- Assets/StreamingAssets/Data/expeditions.json assigns the location distanceTicks 15, dangerLevel 5, encounterChancePerTick 0.155 and stamina drain 2.75. Its existing scavenging table is table_loot_power_substation, and listed loot categories include battery_pack, item_battery_reconditioned and copper_wire_10m_of_10m.
- Assets/StreamingAssets/Data/damaged_map_zones.json defines the Metro Service Ring and identifies the Electrical Maintenance Exchange as a traction-power junction, distinct from the existing road-side electrical substation. The zone has route fragments and a station reference.
- docs/cartography/PLAN85_COMPLETION_REPORT.md, docs/cartography/PLAN85_BALANCE_MATRIX.md and docs/cartography/INSTALLATION_LOOT_PROVENANCE.md describe the destination, route and loot provenance.
- Assets/Ashfall.Core contains a current shelter power-grid owner. Its present authority applies to the Holdfast grid. This expansion must not infer a regional grid simulator from the existence of the substation location.
- Wave 11 Expansion 62 proposes a North Service Packet and identifies a cross-reference between the forestry cache and a utility route request. That draft is not an implementation fact and remains optional.

The source evidence confirms a setting and a supply profile. It does not confirm that the player can operate this substation, restore its output or affect regional power. The design explicitly excludes those claims.

### 2.2 Existing systems to reuse

- Wasteland map discovery and authored route knowledge.
- Expedition dispatch, travel, party state, hazards, return and loot.
- Weather and current visibility/route hazards.
- Holdfast power and load-shedding state as a present-day contrast, never as the old exchange’s simulation.
- Radio, journal, rumor or standing-record routes only where the current host has an observable consumer.
- Current inventory and trade/reputation owners for actual items or contacts.
- Existing quest/encounter branching and save/restore semantics.

### 2.3 Classification

- **Content:** new location narrative, quest stages, field documents, oral accounts and callbacks.
- **Data only:** possible if current catalogs can express the stages and persistent outcome.
- **Data + wiring:** possible only for one proven route from an existing expedition or report outcome to a journal/radio consumer.
- **Core extension:** not part of the design outcome.
- **Foundational:** rejected; no regional power or switching authority is needed.

### 2.4 Collision cautions

Power restoration, load shedding, grid priority and equipment maintenance already exist as gameplay domains. This story is about an old decision, not an extension of the Holdfast’s operational grid. Another “save the clinic before the generator dies” quest would duplicate common survival pressure. The distinctive question is evidence and transmission: what does a correct report say when a correct schedule was not received in time?

---

## 3. Story world: the closed room and the missing receipt

The exchange served a rail traction branch, a maintenance dispatch desk and a small transfer bay. Before the Exchange, the utility made seasonal preparations with the forestry store and a materials laboratory. The packet was divided among three sites because each had a different job. No single office held the full plan.

The clinic route was not the only load on the line. It was simply the one load whose absence could be described by a person waiting in a cold room. The relay required enough current to carry a signal; the traction side required more energy than the maintenance crew could keep on the reserve circuit. The switching schedule placed the relay first, the clinic second and the traction branch third. During a weather event, the order changed twice.

Eda Venn, a line worker, was operating the manual transfer. Elen Voss, a dispatcher, sent a correction over a weak carrier. Beren Vale, a field worker, held at the lower crossing with a spool of line and a portable test set. Dema Rusk, a clinic runner, waited in a room whose radiator had cooled but whose blanket warmer still had power. Yori Pell, an apprentice, recorded the switch positions after the operator left.

Their actions can be reconstructed in broad outline, but not with complete certainty. The relay lost the first correction. Eda opened the clinic feeder. The station report later described that action as “unapproved,” because the approval copy did not arrive until after the switch moved. Elen’s shift note says she requested the change. Beren’s field note says the power returned for one short interval, then stopped. Dema’s note says the room warmed. The timing of each account overlaps but does not align perfectly. No document proves whether the warming interval came from the line, stored thermal mass, or a local battery.

The player does not decide whether the clinic “survived because of the switch.” That claim is beyond the evidence. The player decides how to preserve the smaller truth: a worker changed a line during a communication gap; the room received heat; the written order was late; the route did not remain powered.

### Tone and realism rules

- No supernatural electrical event, sentient machine, glowing anomaly or unexplained carrier.
- No clean conspiracy and no singular villain. Misdated copies, shift changes, storm delay and limited capacity explain the evidence.
- No glamorous high-voltage interaction. The player reads labels and records, not live wires.
- No miracle battery that powers a whole settlement.
- No simplistic “one lever saves everyone” branch.
- No real utility company, transit network, hospital, agency or person.
- A blank field remains blank. Do not fill it with an omniscient author note.

---

## 4. Narrative spine: “The Lower Feeder”

### Stage 1 — The route request

A map fragment or existing contact mentions a sealed switching book at the Electrical Maintenance Exchange. The Holdfast needs a clear report on the lower service route, not a working power station. The player chooses whether to dispatch now, wait for a better forecast, or leave the route unverified. The content does not create a deadline. A player under current shelter pressure may wait.

### Stage 2 — The door that says HOLD

At the exchange, the player finds a steel door with a paper strip trapped under its lower edge. The label says HOLD — LOWER FEEDER. The strip is dated later than the switch log but earlier than the station closure. The player can inspect the door exterior, enter through the maintenance bay if the route is clear, or withdraw. No live lock puzzle is added.

### Stage 3 — Two schedules

The dispatch desk holds the planned switching order. The field cabinet holds the actual switch position card. The two lists disagree on one interval. A radio note says “correction sent” but carries no acknowledgement. The player can copy both, take one page, or leave the record in place.

### Stage 4 — Elen's shift note

A note in the desk drawer says Elen requested the lower feeder be opened for a short interval. A signed report says the request was unapproved. The player has enough evidence to know that one record is incomplete, but not enough to know which person entered the time incorrectly.

### Stage 5 — Beren at the crossing

A field report from the lower crossing describes a cable spool, a cold test set and a dead carrier. It says the line was “warm for a count of fifty,” then “dark again before return.” The note does not say who counted or whether the interval was an exact fifty seconds. A later copy expands “warm” to “powered,” which is a stronger claim than the original text supports.

### Stage 6 — Dema's clinic receipt

The clinic copy lists a blanket warmer and one room radiator as loads. A margin says “felt heat at the west wall.” There is no medical outcome or patient count. Dema's note describes the room, the people who waited and the fact that she did not know whether the switch was opened for them. The player can read the note as testimony, not proof of causation.

### Stage 7 — The present report

Back at the Holdfast, the player decides whether to publish a confirmed sequence, preserve the names privately, withhold the report, or write a provisional summary. The immediate outcome affects the available conversation and the journal language. Any standing/reputation result must use an existing owner and be proven during promotion.

### Stage 8 — The receipt returned

On a later visit or callback, a single receipt is found in a pocket on the inside of the control-room door. It is addressed to “whoever reads this.” The receipt says the exchange crew received the winter kit, but does not list the clinic. The player sees that the emergency work crossed departments without one shared record. The quest can close without answering whether the clinic load was approved.

### Stage 9 — Use without authority

The player may bring the current schedule back for reference. The Holdfast can learn what labels meant and how the old route was documented. The schedule is stamped “historical—verify before use.” It cannot operate or optimize the Holdfast power grid. This is the final content consequence: the player has useful knowledge and an explicit limit.

---

## 5. Main quest scenes A–C

### Scene A — Board briefing: “Not a working station”

**Quartermaster:** “The map points to a power exchange.”

**Player:** “Can it restore a line?”

**Quartermaster:** “The map does not say that. The expedition list says batteries and cable. If you find a schedule, bring the schedule. If you find a live switch, do not touch it.”

**Route survivor:** “The old rail feeder ran through the lower ring. That's why the exchange is there. It may not have fed anything after the stations closed.”

**Player choices:**

1. “We go for the records and whatever listed supplies are actually safe to take.”
2. “Wait for a clearer forecast.”
3. “Send no one. A map mark is not a reason to spend a crew.”
4. “Ask the radio operator whether the frequency is still in the log.”

If the player asks about radio, the operator responds with evidence, not a miracle clue: “The old station is on the map. I can tune the receiver. I cannot tell whether anyone is transmitting from there.”

### Scene B — Outer door: “HOLD”

**Inspection text:**

“The switch-room door is closed. A thick painted word says HOLD. The paint has cracked across the letters and the lower edge is polished from something being pushed beneath it. A copper-colored stain runs down the latch plate. It is old enough to have dried black.”

A note is caught beneath the door:

> HOLD lower feeder until receipt. Do not transfer on voice alone after shift change. If the carrier is poor, send a runner to the dispatch board. Initial the copy when you have read it.

The player may compare the note date to the map fragment, check the door from the side, or leave. The door can be opened only through an existing site/expedition interaction if one exists; this plan does not design a lockpicking feature. If no such interaction exists, the note is read outside and the story's next clue sits in an already reachable maintenance bay.

**Optional character line:**

“That word is a warning. It is not a direction to try the handle.”

### Scene C — The planned order

The dispatch desk holds a laminated card. The card is intact but its corners are clouded by moisture. A typed header lists load names; a hand has written “line current observed” beside the first three slots.

> SHIFT 2 — WINTER SERVICE ORDER  
> A. relay / receive test  
> B. clinic west room  
> C. lower traction branch  
> D. exchange bay lights  
> Reserve: one transfer attempt, only after field return  
> Correction: copy to station, clinic and cache route  
> Acknowledgement: enter initials and time

The acknowledgement columns are empty. A penciled “A.V.” appears beside the correction, then has been erased. The card may have been touched by Eda Venn, but the initials alone do not prove that.

**Player choices:**

- Copy the planned order and leave the card in place.
- Take the card if its physical item/quest consumer supports that action.
- Photograph or journal the list through a current record path.
- Ignore it and search for the field switch record.

**Quartermaster line if present:**

“It is a schedule. It tells us what they meant to do. We need the mark that tells us what they did.”

## 6. Main quest scenes D–H

### Scene D — The field card: “Position at departure”

The field cabinet contains a card clipped to a spring clip. Its surface bears four switch positions and a note that someone changed the order after the ink dried. The player can read the visible face without touching the cabinet internals.

> LOWER FEEDER: OPEN  
> TRANSFER: HOLD  
> RELAY: TEST / NO ACK  
> WEST ROOM: CURRENT NOT VERIFIED  
> TIME: 19 / ? / 24  
> POSITION CHECKED BY: [blank]

The left side of the card has a thumb-shaped smear. A white line in the paper shows that a strip was removed. Beneath the smear, a pencil note says “do not leave this at the switch.” The card is not proof that the lower feeder remained open. It is one observation from one moment.

**Player choices:**

- Record the observed positions and leave the card in the clip.
- Carry the card to compare with the desk schedule.
- Stop and return if the party is unsafe or the site hazard is worsening.
- Search for the missing strip without moving the cabinet controls.

If the player searches, the strip is found stuck to the underside of a metal cover. It reads “transfer only after receipt.” A later note says “receipt never arrived.” The paper was not hidden. It slipped under the cover during a hurried closure.

**Optional dialogue:**

**Route survivor:** “The lever position is a fact. It isn't a full shift.”

**Player:** “Can you tell when it changed?”

**Route survivor:** “Not from this mark. The dust has moved since.”

The text should stop there. It is not a forensic puzzle with an answer waiting behind a skill check.

### Scene E — Elen's shift note: “I sent it twice”

A drawer at the dispatch desk contains a folded note beneath a blank receipt form.

> I sent the correction at 18:42 and again after the carrier dropped. The first report was read back as “hold lower.” I said “open lower for one transfer.” I do not know which word crossed the gap.
>
> The field team had the old schedule. I had the revised schedule. The courier had the clinic copy. No one had all three at once.
>
> At 19:10 the board showed no acknowledgement from the lower crossing. The line was already carrying current by then. I entered “unapproved” because I had no receipt. I should have written “not acknowledged.” I did not.
>
> The shift after me had to close the feeder. The close order went out by runner. I do not know whether it arrived before the crew returned.
>
> — E.V.

The player can compare “unapproved” with “not acknowledged.” That correction is the story's key. It changes the claim from a judgment about permission to a fact about communication. It does not establish that the feeder should have been opened.

**Player options:**

1. Copy the note as written, including the correction.
2. Copy only the confirmed time range and missing acknowledgement.
3. Take the paper, if a current item/story inventory can represent it.
4. Leave it and record the source location.

### Scene F — Clinic receipt: “West wall”

The clinic copy is not in the clinic. It was folded into the inside cover of the switching book, where someone used it as a divider. Its header lists two room loads: one radiator and one blanket warmer. The receipt is otherwise simple.

> 19:?? — WEST ROOM  
> Radiator: felt at west wall, no gauge reading  
> Blanket warmer: light at plug, no steady current confirmed  
> Patients present: not entered  
> Runner: D. Rusk  
> Received by exchange: no initials

A second line is written in a different hand:

> Don't count warmth by the switch. The wall held some from before. We used it until the room was ready.

This is not an electrical diagnosis. It is a witness note from a person in the room. The story should never claim that the substation kept a patient alive or that the lack of current caused a death. No patient list appears. No victim is invented to increase the stakes.

**Dema's later account:**

“The west room was cold. The radiator was cold when I came in. I went to the exchange because the switch board had a light on. When I came back, the wall did not feel as cold. I wrote that because it was true. I did not write that the line saved anyone. I did not know what the line was carrying before I got there.”

### Scene G — Beren's crossing log: “Fifty”

A weatherproof pouch at the lower crossing contains a field log. The page is warped but readable.

> We waited on the east side with the spool under the canvas. Carrier failed in gusts. At 19:08 the wire sounded live in the test set. I counted fifty after the needle moved. That could be fifty seconds. It could be fifty counts while the needle held. The west room note came in later.
>
> I sent “line good enough to move” and got no read-back. We moved the spool by hand. The cable was not connected when we left.
>
> The morning copy says we moved under power. We did not. We moved after the line went quiet.

The player's journal summary should preserve the ambiguity around the count. A more technical survivor may explain that an uncalibrated test set and a spoken count are not a duration measurement. No tutorial on electrical testing is needed.

### Scene H — The present report

At the Holdfast, the player can now write one of four report forms:

**Confirmed sequence:**

> The dispatch schedule placed the relay first and the west room second. A field card later records the lower feeder open, but it has no timestamp or checker. Elen Voss reports that she sent a correction and had no acknowledgement. The clinic receipt reports warmth at the west wall without a gauge reading. Beren Vale says the line was no longer live when the spool moved. The records do not prove which source of heat warmed the room.

**Shift-level account:**

> During the winter shift, the lower feeder was opened before a receipt was logged. A correction was sent but may not have been read. A later field record shows the route closed. The records do not identify the person who moved the lever.

**Public short version:**

> The exchange's winter order changed during a communications gap. The clinic room reported warmth, but no surviving record proves which load caused it. The lower route did not remain powered.

**Withheld account:**

The player keeps the documents or leaves them at the exchange, depending on the current journal and inventory owners. The report board receives no public version. The game should not interpret silence as guilt.

The player can also choose “I need to check the dates first.” This defers the report without closing the quest. The interface should say exactly what evidence remains unchecked.

---

## 7. Side quests and optional story threads

### 7.1 “The Receipt That Stayed Blank”

A small receipt is clipped to the desk, filled out for a winter kit but unsigned by the receiving station. The player can compare it to the cache issue card from Expansion 62. It lists blanket, rope and one battery. The quantities are partly smeared.

The question is not whether a transaction occurred. The question is whether “received” meant the load reached the exchange, the clinic, or the lower crossing. A dispatcher used the form as a receipt; a field worker used it as a packing list. Both uses are plausible.

**Player actions:**

- Copy the receipt as unresolved.
- Match its legible item to an existing inventory/quest item if available.
- Return it to the clip so another reader sees the blank line.
- Carry it to the Holdfast and risk separating it from its context.

**Closing text:**

“The empty signature box is clean. No pen line crosses it. The paper has been handled often enough that the corners are dark, but no one filled the one space designed to say who received the load.”

### 7.2 “The Read-Back”

The radio log contains a fragment with two possible readings. One is “hold lower.” One is “open lower.” The carrier noise masks the first consonant. A player can ask the radio operator to listen to the surviving recording, but the operator cannot restore missing frequencies.

**Operator:** “I can make the word sound like either one.”

**Player:** “Which did they hear?”

**Operator:** “The person on the other end is the only one who could answer. The tape cannot.”

The story rewards the player who preserves ambiguity. It does not reward speech-recognition skill or a new decoding minigame.

### 7.3 “A Proper Label”

A switch label has been replaced with a handwritten phrase: “clinic west room.” The old typed label beneath it reads “auxiliary load.” The field worker's change made the purpose legible but also attached a human expectation to the circuit. The player can leave it, copy it, or remove the loose label for preservation. The next visit reflects only a supported quest flag.

**Maintenance note:**

> Labels should name the load, not the person who asked for it. If the room changes use, replace the card. Do not leave the old name because it is easier to remember.

### 7.4 “The Warm Wall”

The player can visit an old clinic room reachable through existing map content. It is empty. A patch of insulation on the west wall differs from the rest of the plaster. The radiator has no intact gauge. A small scratch line records three dates and then stops.

The scene offers no corpse, medical outcome or miracle. A survivor may say the room was used as a waiting area. The player can take the thermal blanket if valid and present; otherwise the room is a place observation. No extra disease or medical treatment is introduced.

### 7.5 “The Apprentice's Copy”

Yori Pell's pocket notebook lists lever positions as letters rather than full names. A margin says, “If I write the station's name, the next person will think I knew who was there.” The player can locate a key on the notebook's facing page, but the key only decodes the old shorthand. It does not reveal an identity.

**Notebook fragment:**

> A — test only  
> B — room / one interval  
> C — closed again  
> D — no receipt  
> I did not see who gave the order. I saw a hand on the board after the relay failed. I wrote “operator” because that is what we were.

### 7.6 “The Three Station Copies”

The same schedule appears at three sites: the exchange, the clinic route and the forestry cache. Each copy has a different correction. The player can compare them, but there is no “collect all copies” achievement and no required completion bar. The conflict comes from the copies being intended for different jobs.

- Exchange copy: load order and receipt requirement.
- Clinic copy: room names and a note about warmth.
- Cache copy: route and equipment list.

The player may leave the copies where they are. If one is taken, a later visit shows a blank clip, not an inexplicable replacement page.

### 7.7 “A Voice on the Test Frequency”

The receiver catches the station test tone from the existing radio catalog or an authored recording, if the current cue/audio path supports it. The player may listen to the tone and compare it with a written timestamp. There is no live survivor at the other end and no hidden broadcast schedule.

The short transcript reads:

> **Exchange:** Test one.  
> **Lower crossing:** Heard.  
> **Exchange:** Read back: station open.  
> **Lower crossing:** No. Route open.  
> **Exchange:** Corrected. Route open. Station still held.  
> **Carrier:** [cut]

The distinction between station and route is a mundane but consequential correction. It prevents a later listener from treating an open road as an energized building.

### 7.8 “The Page Under the Battery”

The player lifts the dead battery from the switching book. Beneath it is a note from Elen to an unnamed colleague: “Keep the page flat. The fold runs through the correction.” The note explains why the page was weighted, not why the feeder changed. A low-detail player may never discover it.

### 7.9 “The Equipment Receipt”

A return slip says the winter kit reached the exchange with one battery uncharged, a coil of rope shortened, and one blanket used. The player may compare that description to the cache location. If they do, the game can acknowledge a matching object, but it must not say the same exact blanket or coil traveled between sites unless the texts establish that physical provenance.

### 7.10 “The Modern Switchboard”

A present-day Holdfast worker sees the schedule and asks whether its labels could help reorganize the shelter's circuit priorities. The player can answer that it is a historical record, not an operating diagram. This is a narrative reminder of the boundary; it should not start a new power-grid feature branch.

**Worker:** “It has a load order.”

**Player:** “It has a load order from a station we cannot verify.”

**Worker:** “It could still be useful.”

**Player:** “As a question. Not as an instruction.”

---

## 8. People in the switching book

All names are proposed content. A current repository-wide ID and proper-name census is required before implementation. The named roles do not automatically create recruitable survivors or faction members.

### 8.1 Eda Venn — line worker

**Surface:** The initials A.V. on the cache roster and several field cards. The full name appears only when the player finds a signed maintenance note.

**Practical function:** Manually checked route marks, carried cable and used the old switching order.

**Contradiction:** Eda signed for the lower route but did not claim the route remained powered. Her notes are more careful than the public report written afterward.

**Material history:** A pencil with one flattened side, a repaired glove thumb, a piece of green marking tape stuck to the inside of a coat pocket.

**Pressure response:** She gives a time window and corrects people who round it to a precise time.

**Hidden dimension:** Eda changed the lower feeder after the carrier failed. She believed the correction had been received because the relay gave a response; she later learned that the read-back referred to the route, not the station.

**Arc:** Across all three Wave 11 plans, Eda moves from a set of initials into a person whose work can be described without turning her into the cause of every outcome. Her final note asks that the report say “opened during the gap,” not “opened against orders.”

**Voice:** “The line was open long enough to be a line. That is not the same as saying the room was warm because of it.”

### 8.2 Elen Voss — dispatcher

**Surface:** Author of one schedule correction and several incomplete radio logs.

**Practical function:** Maintained load order, issued changes and recorded acknowledgements.

**Contradiction:** Elen's first report uses “unapproved,” a judgment that exceeds the evidence she had. Her later correction is not a full apology; it is a precise change in language.

**Material history:** Carbon copies folded in the same direction, a red pencil used only for times, and an erased “received” box.

**Pressure response:** She repeats the recipient and the channel before repeating the instruction.

**Hidden dimension:** She believed the correction had been read back; she did not verify whether the field crew understood which “lower” she meant.

**Voice:** “I sent it twice. That does not mean it arrived twice. It might not mean it arrived at all.”

### 8.3 Beren Vale — field worker

**Surface:** Author of the lower-crossing report.

**Practical function:** Carried a test set and line spool on foot through a weather window.

**Contradiction:** Beren says “fifty” but later refuses to call it fifty seconds. Their first report was concise; their later copy is more qualified.

**Material history:** Test-set strap patched with canvas; a steel pencil holder tied to the pouch; one route card that was never folded.

**Pressure response:** Beren uses landmarks rather than clock time when equipment or clocks disagree.

**Hidden dimension:** They moved the spool after the line went quiet, although the summary later described it as a live transfer. The summary may have been shorthand, not deliberate falsification.

**Voice:** “We moved it after. If the form says under power, the form is ahead of us.”

### 8.4 Dema Rusk — clinic runner

**Surface:** The author of the “west wall” receipt.

**Practical function:** Carried blanket and supply slips between a clinic room and the exchange.

**Contradiction:** Dema reports that the room warmed and resists turning the observation into a claim about the electrical line.

**Material history:** A small ruler used to measure blanket hems; a pin cushion made from a torn glove; two receipt copies with different times.

**Pressure response:** She describes what was felt before what was assumed.

**Hidden dimension:** She was responsible for the waiting room's comfort but never knew which person authorized the feeder change.

**Voice:** “I did not need to know which circuit warmed the wall. I needed to know if the room would stay warm. I still do not know the first thing.”

### 8.5 Yori Pell — apprentice recorder

**Surface:** The person who copied the final switch positions after the operator left.

**Practical function:** Made a legible copy under poor light, then carried it to the exchange board.

**Contradiction:** Yori wanted to be exact but wrote the wrong word once because the station glossary used “lower” for both a feeder and a route.

**Material history:** A notebook with the last three pages removed; a pencil sharpener made from folded metal; a line of tiny punched marks on the cover.

**Pressure response:** Yori becomes silent when asked to name the operator, then explains why the label “operator” was chosen.

**Hidden dimension:** They feared that a name would be mistaken for authorization. They preserved the role because they could prove the role.

**Voice:** “I knew who was at the board. I did not know who told them to move.”

## 9. Location writing and safe interaction

### 9.1 Metro Service Ring approach

“The transit cut runs beneath a low concrete canopy. A painted arrow points toward a platform that no longer has a roof. The exchange is behind a service wall to the north, reached by a narrow stair that has been swept once and then left to dust. The map gives distance. The floor gives no promise about what waits at the bottom.”

The route text should use the existing map's terrain, weather and hazard representation. No new cave-in dice or electrical exposure stat is introduced. If the party's condition is poor, a member may ask to pause before the descent; the player may turn back.

### 9.2 Service stair

“The stairwell smells of cold metal and old water. A handrail is wrapped with cloth at the lowest landing. The cloth is not a safety sign; it is a grip patch tied by someone who expected gloves. The lower step has a small dent where a tool case was set down. There is no fresh footprint inside the dust line.”

**Inspect response:** The player can follow the cloth wrap, look at the dent, or continue toward the dispatch desk. The inspection supplies context but does not gate access.

### 9.3 Dispatch desk

“The desk faces the switch-room door. A shallow groove runs along its edge where paper was repeatedly pulled toward the operator. The schedule is pinned under a metal clip. Beside it, a pencil rests in a groove worn by its own point. There is enough light to read the header. The lower columns need a lamp or a careful angle.”

The player's flashlight or light source is only referenced if the current inventory and presentation support it. Otherwise the room text describes available light without requiring an item.

### 9.4 Switching room exterior

“The door is thick enough to keep sound inside. The label says HOLD. The paint is chipped where a hand rested on the word. A line of dust on the sill is unbroken except at one corner, where something thin passed underneath. The door has not been forced.”

The location does not invite the player to operate switches. The player can read the room's labels through the glass, reach a side archive or leave. A live switch is treated as hazardous scenery, not as an interactable lever.

### 9.5 Exchange bay

“Copper cable hangs from the upper rack in loops that have been tied with different cord. One loop is clean enough to have been moved after the last wall wash. A battery sits under the switching book on the desk, placed there to hold the pages flat. The battery terminals are filmed with white residue. A chalk line on the floor marks where a trolley stopped.”

If the loot system provides actual batteries or cable, the expedition can discover them through the existing table. The prose must not create a hidden batch or guarantee a safe battery. The physical description does not identify contamination or charge state without catalog support.

### 9.6 Return visit: open archive cabinet

If the player copied the schedule, a later visit shows the clip empty and a square of cleaner dust beneath it. If the player left the card, it remains in place unless an authored event has a real state to change it. If state persistence cannot be guaranteed, use one-time text and do not claim the room changed.

### 9.7 Return visit: carrier strip

The carrier strip is found in the same location if it was left. Its corner has lifted in damp air. A later note is visible beneath it: “Read-back not receipt.” This is a physical callback to the player's earlier choice to look under the cabinet, not a supernatural message that appears by itself.

---

## 10. Encounter library

### Encounter 1 — A hum behind the wall

A low vibration travels through the wall, then stops. It is a loose cover plate moved by wind through the service corridor. The player can inspect the plate, listen from farther away, or leave. A character who calls it a live transformer is corrected by a second character: “The line is dead. The wall is not.” The wording must reflect actual site state; if current content does not establish the line is dead, use “We cannot verify the line.”

### Encounter 2 — The paper that pulls itself loose

A draft catches the lower corner of a map. The player can weigh it down with an existing object, pin it under the dead battery, or take it. The scene gives no code or hidden cipher. The short practical exchange says more about the room than the paper does.

### Encounter 3 — The service trolley track

Parallel marks on the floor stop at a door and restart on the other side. One track is shallower than the other. The player can infer that the trolley was unloaded or lifted. No tracks lead to an unlisted secret vault. The physical clue supports the idea that equipment moved while the line was under maintenance.

### Encounter 4 — A cabinet held shut by twine

The cabinet door is tied with a piece of ordinary cord. The knot is easy to undo. Inside is a set of empty label sleeves and one sleeve for “temporary load.” The player can take or leave the sleeves if an item identity exists; otherwise the discoverable content is the label and its context.

### Encounter 5 — Dust on the selector

A panel bears a single clean arc where a gloved hand moved it. The player cannot determine the switch position from dust alone. A character says, “It tells us it moved. It does not tell us who moved it, or when.” The story actively rejects overinterpretation.

### Encounter 6 — A missing cover screw

The player finds a cover screw in a tray beside the cabinet. It has a nick across its head and no matching empty hole is visible from the safe inspection point. It may belong to the cabinet, a test box or a trolley. A worker advises leaving small hardware where the next maintainer can find it.

### Encounter 7 — Water above the floor mark

A damp band crosses the wall higher than the current puddle. It records an earlier leak. The player may check the route or avoid the lower room. No toxic flood is presumed. The damp has already dried; it can still make paper brittle and metal stained.

### Encounter 8 — The delayed tone

A brief tone comes through the receiver after the player keys the current radio system, if that action is available. It is a local test tone, not a person. The operator can compare it with a written frequency range but cannot identify the sender. If the radio system cannot represent the event, show the tone as a recorded device artifact rather than a live intercept.

### Encounter 9 — The door from the other side

The outer service door moves a fraction under pressure from the stairwell. The player can hold it, wedge it with a safe object or retreat through the open route. The scene consumes time only according to current expedition rules. No one is trapped in the room by a timed prompt.

### Encounter 10 — A hand-written warning

A warning says, “DO NOT RESET THE TRANSFER.” It was written on the outside of a panel and has been crossed through. The correction reads, “Do not reset until logged.” This is not an invitation to try it. The player can copy the wording and ask why it changed. The text shows procedural caution, not hidden access.

### Encounter 11 — The operator's stool

The stool has one shorter leg and a strip of folded card beneath it. The card bears the date of the second shift. It might have been used as a shim. A player can take it and discover the date belongs to the clinic receipt, or leave the stool balanced. The physical repair is small and practical.

### Encounter 12 — The cold battery

A battery sits under the book. Its label is partly legible. Current item rules determine whether it can be taken, tested or used. If the player cannot verify its state, the text says so. No battery powers the exchange, opens a hidden door or starts an abandoned engine.

### Encounter 13 — Carrier lost on the stair

A radio call from the Holdfast is audible at the top landing and absent at the lower door. The party can move back up to respond, relay a short message from the landing or proceed without acknowledging it. The scene does not create a new long-range signal rule; it represents one location-specific gap only if the existing radio presentation supports it.

### Encounter 14 — The name on the clipboard

A clipboard lists shift roles and contains an erased name. The paper fibers show where the ink was removed, but no letters remain. The player can record that a name was erased. The game must not reveal the name through a convenient reverse-reading trick.

### Encounter 15 — The warm room report

A returning expedition meets a traveler who says they heard that the clinic stayed warm because of the exchange. The player can share the exact record, offer the shorter version, or say they do not know. The traveler can still hold their own belief. This encounter makes information transmission a social choice without introducing a rumor-score system.

---

## 11. Diegetic document packet

### 11.1 Dispatch card, planned order

> METRO SERVICE — WINTER RESERVE  
> Shift 2. Hold lower feeder until receipt.  
> First: test relay carrier.  
> Second: west room / auxiliary heat.  
> Third: lower crossing / one transfer only.  
> Fourth: exchange bay.  
> If the relay fails, do not assume the field team has heard the correction. Send a runner to the board.  
> Initial each copy after reading.  
> Do not enter “received” when you mean “sent.”

A red line beside “runner” says “weather exception: route closed after dark.” The note does not identify who issued that exception.

### 11.2 Actual-position card

> 18:?? — relay: test only  
> 19:?? — lower: open  
> west room: not verified  
> transfer: hold  
> close order: sent by runner  
> acknowledgement: none
>
> **Do not copy this as the schedule.**

The line “actual-position card” is a file label chosen by the player or the current content owner. The card itself is titled only “Positions.”

### 11.3 Elen's correction

> I used “unapproved” because there was no receipt. That was not what I knew. I knew the acknowledgement box was empty. I did not know whether the correction had reached the lower crossing by another route.
>
> If this is copied, keep both lines. The first one says what I wrote. The second one says what I can prove now.
>
> Elen Voss

### 11.4 Beren's field note

> The test set needle moved. We counted fifty. No one called it seconds. We waited for the line to go quiet before lifting the spool. I sent a route message and did not hear it back. The next copy says the crew moved under power. That is not my wording.
>
> We left the trolley at the east door. It could not clear the turn with the battery on it.

### 11.5 Dema's room receipt

> West room received: one blanket, one dry towel, water cup.  
> Radiator: no gauge.  
> Wall: less cold by the time I returned.  
> People: no count taken.  
> No patient name written.  
> Heat source not known.
>
> If someone asks whether the exchange worked, say the room was warmer. If they ask who it warmed, do not guess.

### 11.6 Yori's notebook

> I copied the letter C from the field card. At the dispatch desk, C was lower traction. At the crossing, C was the lower route. I used the station glossary. Beren used the route card. Neither of us meant to change the order.
>
> I wrote “operator” because that is the only word I can prove. It is not a name. It is not permission. It is a job someone was doing.

### 11.7 Station maintenance list

> Replace cover label at lower panel.  
> Clean water from lower sill.  
> Return test box to dry shelf.  
> Confirm route card with north cache.  
> Do not leave a switch position written on the public board without a date.  
> Do not write “safe” where the test only says “quiet.”

The final line connects to the game's wider principle of careful reporting without becoming a slogan. A different author may have written it; no signature remains.

### 11.8 Old receipt, full text

> Received from North service cache:  
> one rope coil, used but dry  
> one blanket, washed  
> one battery, charge unknown  
> one paper route copy  
> Return location: exchange desk  
> Recipient: [blank]  
> Date: [partly lost]
>
> The phrase “charge unknown” is crossed through once, then restored in darker pencil. This may be a correction by someone who did not want uncertainty erased.

### 11.9 Public bulletin, first draft

> During the winter shift, the lower feeder was opened before the acknowledgement was entered. A clinic room reported warmth. The surviving schedule does not show who moved the switch. The route did not remain powered.
>
> Do not use this bulletin to plan a modern connection. It records an event, not an operating procedure.

### 11.10 Public bulletin, altered copy

> The operator diverted power from the station to save the clinic. The line was shut down after the rescue.

This shortened copy is a deliberate overstatement. The player's report can compare it with the evidence and correct it. It is not the official canon and must never be presented as a reliable narrator's conclusion.

### 11.11 Handwritten correction

> No rescue is named in the original pages. No patient count survives. We do not know that power was diverted from the station. We know the lower feeder was open. We know a room reported warmth. We know the line was later closed.
>
> Keep the short copy as evidence of what people are now saying. Do not file it as what happened.

### 11.12 Closing note on the book

> The battery no longer holds the page down. The book stays open because its spine has been bent that way. Put it flat before you leave. The air moves under the door. A loose page will not wait for a reader.

## 12. Radio and spoken fragments

The exchange uses radio as a physical communication tool with limits. The scripts below may be presented as recordings, transcripts or live checks only where the current radio system can support them. They never reveal an exact historic truth by being heard clearly.

### 12.1 Short station test

> **Elen:** Exchange to lower. Send a station check.  
> **Beren:** Lower route reads. Station still held.  
> **Elen:** Repeat: route reads, station held.  
> **Beren:** Correct.  
> **Elen:** Correction sent.  
> **Carrier:** [drops]

### 12.2 The delayed read-back

> **Eda:** Lower feeder open for one transfer.  
> **Elen:** I hear “lower open.” Which lower?  
> **Eda:** Feeder. Not the road.  
> **Elen:** Copy feeder.  
> **Beren:** I heard “road.”  
> **Elen:** Repeat.  
> **Carrier:** [sustained static]

The player should encounter this only after seeing the physical route and feeder labels; otherwise it is noise without a frame.

### 12.3 Current operator's comment

> I can hear the test tone from the exchange. It is not a voice. The old receiver is cycling through a local check, or it is catching another device on the same band. We don't know which. I can show you the frequency on the dial. I cannot tell you who pressed the key.

### 12.4 A present-day message to the Holdfast

> The exchange is reachable from the lower stair. The records are dry enough to read. No switch was moved. We found a schedule, a field card and a room receipt. The west room was reported warmer. We cannot verify the heat source. The lower feeder was not left in service.

This message is intentionally not exciting. It provides useful information, keeps the team safe and separates evidence from inference.

### 12.5 Rumor at a trade stop

> They say the old operator took the whole station dark to warm a room. Maybe. The report on our board does not say that. It says the lower feeder was open and the room felt warmer. People need a story with a hand on the switch. The paper gives them a hand and no name.

The line recognizes rumor without confirming the rumor network's content as true. A current information/rumor owner is required if this becomes a reactive conversation.

### 12.6 Silence after publication

If the player publishes the short version, a later radio conversation may receive no answer. Silence is not proof that a person is angry, dead or listening. The game should avoid an automatic sound cue implying judgment. The transcript can simply read “no acknowledgement entered.”

---

## 13. Outcomes and persistent consequences

### 13.1 Confirmed account published

**Immediate:** The Holdfast has a careful report describing the sequence, the unacknowledged correction, and the limits of the clinic receipt. The report can be used in later dialogue if an existing record or rumor consumer supports it.

**Cost:** People who preferred a simpler account may say the report avoids naming responsibility. That response is not proof the player was wrong. The player has chosen accuracy over rhetorical certainty.

**Later text:** “The published copy is shorter than the evidence packet. It still carries the words ‘cause not verified.’ No one has crossed them out.”

### 13.2 Shift-level account published

**Immediate:** The player records what the shift did but withholds individual names. The group can cite a sequence without turning the worker into a scapegoat.

**Cost:** The account does not satisfy a contact who wants to know which person moved the feeder. They may ask the player to keep looking or may refuse to endorse the report.

**Later text:** A replacement copy arrives with “operator” written where the name line would be. It is not a corrected identity. It is a role that the documents support.

### 13.3 Public short version published

**Immediate:** The player makes a concise statement that can travel farther than the full evidence. Its caveat remains in the same paragraph. If an existing radio or rumor surface shortens it later, the design should show which caveat was lost.

**Cost:** Some readers remember only that the clinic room felt warm. The player can correct that claim once, if the current content route permits a second conversation. The story does not loop infinitely until the player agrees to a particular phrasing.

### 13.4 Report withheld

**Immediate:** The player keeps or leaves the pages and makes no public statement. The Holdfast does not gain a record benefit. It also does not make a public claim from incomplete evidence.

**Cost:** A later conversation may refer to the rumor without a counter-record. Withholding is not framed as cowardice; it is a choice to keep uncertainty private.

### 13.5 Historical schedule used as reference

The player may bring home a copy for historical comparison. A later shelter mechanic or scene may display it next to the current power board only as an example of an old notation system. It cannot modify load priority or generate electrical capacity. A character's line states, “We can learn how they wrote a change. We cannot assume their circuits are ours.”

### 13.6 Evidence left at the exchange

If the player leaves the pages in place, the final passage says exactly that. A later visit may confirm whether the pages remain only if that state is persisted by an existing quest flag. Otherwise, the report says “left at the exchange on the date of return.” It does not promise the papers survived.

### 13.7 Failure and refusal

- An expedition can retreat before entering the switch room.
- A party may copy no documents and still return safely.
- The player may refuse to publish any version.
- The player may read the short rumor and decide not to correct it.
- The player may take salvage and leave the story incomplete; the journal should then say the evidence packet is partial.
- A failed radio attempt does not kill a witness or close the quest.
- No ending requires the player to identify Eda Venn as the operator. Her connection becomes likely only after the cache roster and a signed note are compared.

---

## 14. Optional side conversations at the Holdfast

### 14.1 The electrician and the schedule

**Electrician:** “This is a transfer order.”

**Player:** “Can we use it?”

**Electrician:** “We can read it. We can compare labels. We cannot connect this to our board. It was written for another station.”

**Player:** “Could the priority order help?”

**Electrician:** “The names might. The loads won't. The clinic room and our ward are not the same circuit.”

This conversation is a natural boundary check, not a tutorial that introduces a new feature.

### 14.2 The quartermaster and the receipt

**Quartermaster:** “The battery is listed as unknown charge.”

**Player:** “It is under the book.”

**Quartermaster:** “That says where it was. It does not say it works.”

**Player:** “You want it?”

**Quartermaster:** “I want it tested if testing is safe. I do not want the report to say it ran a room.”

### 14.3 The radio operator and the read-back

**Operator:** “The carrier could have taken one word and lost another.”

**Player:** “Could someone have heard the wrong order?”

**Operator:** “Yes.”

**Player:** “Can you prove they did?”

**Operator:** “No. We can prove they asked for a read-back. The answer is not on the tape.”

### 14.4 The survivor who wants a name

A survivor asks whether the player found Eda. The player has found only initials. The response options are “the record points to her,” “the record does not identify a person,” or “I need to compare one more page.” The survivor can disagree, but the game does not mark a relationship loss solely because the player refuses to overstate evidence.

### 14.5 The survivor who knew the clinic

The survivor says the west room was cold during one visit and warm during another. They cannot place either visit on the same date as the exchange log. The testimony makes the room real but does not close the circuit question.

---

## 15. Wave 11 connection: The North Service Packet

The shared packet connects Expansion 62's North Woods cache, Expansion 63's switching book and Expansion 64's materials test. Each packet fragment is independently readable and has its own local purpose.

| Site | Physical record | Local meaning | Cross-plan clue |
|---|---|---|---|
| Forestry Emergency Store | route request, rope tag, winter-kit receipt | a person could reach a dry room if the crossing stayed open | a kit was meant for the lower route |
| Electrical Maintenance Exchange | switching schedule, field card, clinic receipt | a lower feeder was opened during a communications gap | the route was not the same as the station load |
| Materials Research Sublevel | cold-test sheet, repair clip sample, rejection note | a material passed one test and failed another | the route's cable sleeve could not be certified for extended cold use |

No master copy names a project or person in charge. The player may infer that a few practical staff shared information through paper and radio. That is all. The packet is not a covert network, faction, machine archive or chain of hidden quest requirements.

Expansion 62's cache note says the lower path was open for one delivery. Expansion 63 clarifies that “path open” and “station open” were distinct phrases. Expansion 64 explains why one cable assembly was considered a short-use repair. The player learns how the wording and material constraints interacted, but still cannot prove which person saw which notice in time.

### Optional shared callback

After all three plans, the player may be offered a single journal synthesis:

> The North Service Packet was never a complete operating order. The cache held the route, the exchange held the switching schedule, and the lab held the test sheet. Each record describes a different limit. The documents do not identify one decision-maker or one complete recipient list. They do show that the people involved tried to keep a path, a room and a signal available with materials and time that were already running short.

If all three content flags cannot be read by the existing journal system without a new aggregator, the callback should be dropped. Three complete standalone endings are more important than a central index.

## 16. Object-led story sequence

The player can find these objects in a different order. Each changes the interpretation of a previous one without replacing it.

### 16.1 The red pencil

A red pencil sits in the desk groove, its eraser worn flat. The color matches the correction on the schedule but not the handwriting on the field card. It is not proof that Elen made the correction. A character can say, “The office used red pencil for times. So did everyone who borrowed one.”

### 16.2 The paper clip stain

A rust mark crosses the phrase “received by station.” The paper was clipped under a cabinet lip and got wet from a leak. The stain obscures the signature box, not the date. The player should not be able to infer intentional erasure from ordinary corrosion.

### 16.3 The glove thumb on HOLD

The switch-room label is polished at its lower edge. A thumb in a work glove may have rested there while someone waited. The mark cannot tell whether the person was about to open the feeder or close it. The room was used as a place to stop and read.

### 16.4 Two kinds of copy

The dispatch form has a clean, aligned copy and a hand-written field copy. The clean one is easier to read but lacks one correction. The field copy is complete enough to show the correction but carries no receipt. A player can take either or record both. The content does not designate one as the “correct” document.

### 16.5 The battery beneath the book

The battery's weight has kept the page open, but its bottom has marked the paper with a dark crescent. The book was being read when the battery was placed there. The player cannot know whether this was before the exchange closed or years later when a scavenger inspected it.

### 16.6 The trolley stop

A painted bar on the floor marks a trolley position. A wheel stopped beyond the bar, leaving a short arc in the dust. The line may have been loaded or empty. The player sees that equipment moved through the room, not what it carried.

### 16.7 The clinic room gauge

The gauge face is cracked. Its needle is missing. The last date on the inspection sticker is legible. The player cannot use the gauge to determine the old room temperature. This empty instrument is more useful than an invented reading because it explains why the receipt uses touch rather than a number.

### 16.8 The dispatch board square

A blank rectangle on the board is slightly cleaner than the surrounding paint. A schedule was likely pinned there, then removed. The paper's outline does not reveal its contents. It gives the player a reason to ask why the surviving page is not the only copy.

### 16.9 The station glossary

A laminated glossary defines “lower” as “the secondary feeder below the exchange floor.” A handwritten margin adds “route below crossing.” The margin is in another hand. The two uses of the same word are a real source of confusion, not a clever code.

### 16.10 The unopened test set

A test set remains sealed in its case. It has a service tag and no current calibration stamp. The player should not open it or use it as a puzzle device. The tag says it was sent for repair the month before the shift. This explains why the crossing note relies on an informal count without proving the line state.

### 16.11 The cold floor patch

The floor is stained where a radiator line once leaked. A rectangular section was replaced with concrete of a different color. The patch is dry. It indicates maintenance happened; it does not indicate when or who performed it.

### 16.12 The handrail wrap

A strip of cloth is tied to the lowest stair rail. One end has a square knot and the other a loop. A person could remove it without cutting. The cloth resembles the cache's dry-gear fabric but is too common to establish provenance. The player may notice a repeated knot style; this creates a connection, not a proof of shared author.

### 16.13 The station cup

An enamel cup sits on a shelf. The handle has been repaired with wire and the underside is marked “night.” This may refer to a shift, a shelf or the cup's use. A later room receipt says a cup of water was present at the clinic. It is not the same cup unless an actual matching mark appears.

### 16.14 The missing acknowledgement line

Three rows on the schedule contain a place for an acknowledgement time. Two are blank, one says “sent.” None says “read.” This is the story's most important interface distinction. It should be stated once in clear prose and then shown in later documents, not repeated in every scene.

---

## 17. Secondary encounters

### 17.1 A traveler at the metro stair

A traveler waits above the stair and asks whether the exchange is safe. The player can say it is dangerous, say the route is known but the room is not, offer a map fragment, or ask why they are traveling. The traveler is carrying a bundle of clothes and a folded receipt. They are not a mechanic and do not need to enter the site. The player can help them avoid the lower level without handing over the story's documents.

### 17.2 A contact asks for a circuit diagram

A settlement contact asks whether the player found a diagram that could restore their lights. The player can say no, explain that the schedule is historical, or share a simple load-order note. If the contact asks for a copy, the player can refuse because it is unsafe to present an old station schedule as current instructions. The refusal does not close the relationship.

### 17.3 Weather closes the upper stair

A dusting of frozen rain makes the outside steps slick. The party can wait, use a current route/gear option or retreat. The scene names the condition and cost before choice. No one falls automatically because the plot needs a setback.

### 17.4 The wrong label

A switch cabinet is labeled with a route name, but the schedule uses the station name. The player can record the mismatch. The mechanic is not to switch the panel; it is to avoid confusing a place with a circuit. A character can say, “The words look alike from this side of the door.”

### 17.5 The old voice memo

A clipped voice note says, “Check the lower before the room.” It may be an instruction for inspection order or load order. A later schedule distinguishes the two. The player's journal can mark the voice memo as ambiguous. There is no secret second meaning.

### 17.6 The courier at the Holdfast

A courier asks whether the route is open. The player can answer “the path was passable when we left,” “the feeder was not in service,” or “I have no current report.” The distinction gives the player a chance to use precise language. The courier's response depends on their own destination, not on a hidden moral score.

### 17.7 A light in the wrong window

At distance, a reflection in a broken panel looks like a powered status lamp. On approach, it is a piece of glass catching the party's light. The scene is not a jump scare and does not imply sentience. A survivor can laugh once and then continue working.

### 17.8 A voice waits for confirmation

At a radio check, one survivor keeps repeating “copy?” after the carrier drops. Another reminds them that no answer is not an acknowledgement. This exchange mirrors the old documents without making the characters narrate the theme. The player can respond once or continue.

---

## 18. Dialogue packet: readings and disagreements

### 18.1 Reading the schedule

**Quartermaster:** “It says the relay comes first.”

**Electrician:** “The relay test comes first. The line itself is not listed as a load.”

**Quartermaster:** “Is that different?”

**Electrician:** “It is different enough that the person reading the board should know it.”

**Player:** “Can we tell which one they meant?”

**Electrician:** “We can tell which words are on the page. We can ask the page to do no more.”

### 18.2 Reading the field card

**Route survivor:** “Open, hold, test. These are positions.”

**Player:** “And the time?”

**Route survivor:** “Question marks. Someone had a clock that did not agree with the desk, or someone copied this after the shift.”

**Player:** “Could be both?”

**Route survivor:** “Could be. We don't need to choose between them to say what we found.”

### 18.3 Reading the clinic receipt

**Medical worker:** “It says the west wall felt warmer.”

**Player:** “Would that tell you the room was safe?”

**Medical worker:** “No.”

**Player:** “Would it tell you the line worked?”

**Medical worker:** “No.”

**Player:** “What does it tell you?”

**Medical worker:** “Someone noticed a change and wrote it down. That is not nothing.”

### 18.4 The accusation

**Contact:** “Eda took power from the station.”

**Player:** “The papers show the lower feeder open. They don't show what was drawing current.”

**Contact:** “She still moved it.”

**Player:** “The report says an operator moved it during a gap. It doesn't say who gave the order.”

**Contact:** “You are protecting her.”

**Player:** “I am protecting the difference between the record and the story.”

The contact can remain unconvinced. The player is not awarded a persuasion victory.

### 18.5 The simple story

**Survivor:** “People will remember that a room got warm.”

**Player:** “They should.”

**Survivor:** “They will say it was the switch.”

**Player:** “They might.”

**Survivor:** “Would you correct them?”

**Player:** “I would tell them what we found.”

**Survivor:** “Then they will remember the room and forget the words after it.”

This exchange invites the player to decide what kind of report can travel, without telling them to abandon accurate records.

### 18.6 A refusal to name

**Player:** “The initials could be Eda's.”

**Quartermaster:** “Could be.”

**Player:** “Do you want me to write her name?”

**Quartermaster:** “I want you to write what you can defend if she is standing in the room.”

**Player:** “She isn't.”

**Quartermaster:** “That is why the question matters.”

### 18.7 The electrician disagrees with the quartermaster

**Electrician:** “We should not post this schedule beside our board.”

**Quartermaster:** “No one suggested that.”

**Electrician:** “Someone will.”

**Quartermaster:** “Then we write the warning clearly.”

**Electrician:** “A warning gets folded.”

**Quartermaster:** “So does the schedule.”

The exchange is not a debate about whether records matter. Both characters are concerned that the paper will be used as an instruction.

### 18.8 The player declines to decide

**Contact:** “So what happened?”

**Player:** “A feeder opened. A correction was sent. No receipt survives.”

**Contact:** “That isn't an answer.”

**Player:** “It is the answer we have.”

**Contact:** “Then keep looking.”

The player may leave the conversation there. No hidden evidence is created to satisfy the contact.

---

## 19. Ending passages

### 19.1 Careful public report

> You posted the confirmed sequence and kept the unconfirmed cause in the same paragraph. The report says a room felt warmer. It says no gauge reading survives. It says the lower feeder was later closed. The names remain in the evidence packet but not on the public notice.
>
> Someone has underlined “not verified” once. The line is still legible.

### 19.2 Role-based report

> You filed the shift under “operator,” not under a person's name. The record identifies the work and leaves the decision-maker unresolved. The report is less satisfying to read and harder to misuse as an accusation.
>
> It may still be misread. No paper can prevent that.

### 19.3 Withheld report

> You left the public board blank. The old schedule remains in its folder, and the player-held copy remains incomplete. No one at the Holdfast can cite it as an official account.
>
> The room at the exchange did not change when you chose silence. The conversation did.

### 19.4 Short version travels

> The short account says the west room warmed during the winter shift. It also says the cause is unknown. At the trade stop, someone repeats the first sentence and forgets the second. You correct it once. The next speaker is already looking at the road.
>
> The correction is written in your journal. You cannot make every listener keep it.

### 19.5 Schedule kept for reference

> You carried a copy of the switching order home. The electrician placed it beside the current board, read the header and moved it to the archive shelf. No lever moved. The labels might help someone understand the old station. They will not tell the Holdfast what to power tomorrow.

### 19.6 Evidence left at the station

> You put the pages back under the dead battery. The book stays open to the field card. You cannot know whether another person will read it, but you know where you left it.
>
> The room remains unpowered. The record remains readable.

---
## 20. Supplemental records and public copies

### 20.1 The wrong-side carbon

> COPY 2 — clinic route  
> Keep west room warm if available.  
> Keep lower feeder held until receipt.  
> Do not wait at the exchange if the route is closed.  
> If the copy is unreadable, ask the dispatcher.  
>
> The words “keep lower feeder held” are on the carbon copy, but the top page has “open lower feeder” written above them. The copy may have been made before or after the correction. No timestamp accompanies the change.

### 20.2 Shift change card

> **Outgoing:** relay test incomplete; lower crossing no read-back; west room requested.  
> **Incoming:** hold station, verify route, do not inherit a switch position from the verbal summary.  
> **Note:** There is a second station using “lower.” Ask which one.

### 20.3 Runner's route slip

> Left exchange at 18:55. Took upper stair. West road under drift. Returned to lower crossing at 19:23. No acknowledgement from the clinic copy. Reached cache after dark. The door was shut. Paper left inside the dry sleeve.

The route slip is unsigned. Its times may come from a clock that disagreed with the exchange. It establishes only that someone carried a paper copy along a difficult route.

### 20.4 Unsent notice

> We have changed the winter order. The lower feeder is held until the field team returns. The clinic has one battery for the blanket warmer. The station lights are off. Do not move the transfer until you have read this notice.
>
> The notice has no “sent” mark. It is folded beneath a radio battery. No author is identified.

### 20.5 Maintenance record

> Switch handle clean. Contact surface not inspected. Cabinet dry. Door seal torn at lower corner. Replace seal before next wet season. The control-room wall holds moisture behind the west conduit. Do not use a warm wall as evidence that the radiator line is active.

### 20.6 Clinic room fragment

> We moved the bed away from the exterior wall. The west side felt warmer, but the blanket was already under the patient. The runner brought the receipt later. Nobody asked whether the radiator had come on. We needed the room and had a room.

No patient name, outcome or diagnosis is included. This avoids making a medical person a faceless prop for an electrical story.

### 20.7 Eda's pocket note

> I opened lower after the second test. I heard the relay once. I did not hear Elen say “hold.” The field copy says one transfer. The desk copy says no transfer. I wrote the position when I left. If I have the order wrong, the next copy will say so.

### 20.8 Elen's private addendum

> I am not writing that Eda disobeyed. I am writing that I did not have a receipt. I wanted the feeder shut because I could not see the field. The crew wanted the room warm because they could see it. Both statements can be true. The line did not have capacity for every instruction at once.

### 20.9 The station's public summary

> Lower route and lower feeder are distinct. The route may be traversable while the feeder is isolated. No public notice should say “station open” without a station status check. During the winter shift, the carrier did not provide a complete read-back.

### 20.10 The repeated shorthand

> L/O = lower open?  
> L/H = lower held?  
> R/O = route open  
> S/H = station held  
>
> This shorthand was stopped because the slash mark disappeared in carbon copies. Write the whole word.

### 20.11 The file cover

> WINTER SERVICE — INTERNAL  
> Keep the dispatch order, actual-position card and clinic receipt together.  
> Do not sort by department.  
> If one page is missing, write “incomplete file.”  
> Do not reconstruct it from memory.

### 20.12 The last file note

> This copy is not the master. We do not know where the master was sent. The file is useful if it tells you what it is. It is dangerous if you make it tell you more.

---

## 21. Evidence order and replay structure

The same underlying event can be encountered from different starting points. The facts do not change by run; the order in which the player encounters them does.

### Route A — schedule first

The player reads the planned order before the field card. The schedule seems authoritative until the lower feeder is found open with no checker mark. The player then has to revise their first interpretation. This route teaches that a plan is not a record of an action.

### Route B — field card first

The player sees an open feeder position before knowing the planned order. They may infer that the operator chose a diversion. The desk schedule later makes that inference less certain. This route makes the player feel the pull of a confident story and then qualify it.

### Route C — clinic receipt first

The player sees a warm-room account and may suppose the electrical line caused the change. The dead gauge and the field card show that the heat source cannot be confirmed. This route begins with a human observation and teaches its limit without dismissing it.

### Route D — rumor first

A contact repeats the simplified claim that the station was sacrificed to save the clinic. The player can believe, challenge or defer. The documents later support part of the claim and weaken part of it. The rumor is not automatically false; its certainty is.

### Route E — withdraw before the switch room

The player may leave with a route report and only the exterior notice. Later play can reopen the story. Nothing at the Holdfast asserts a definitive conclusion. This keeps survival management ahead of optional lore.

### Replay value

Replay differences come from evidence order, party voice and public-report choice, not randomized identities or hidden endings. Eda's action remains a switch opened during a communication gap. Elen's correction remains a distinction between “unapproved” and “not acknowledged.” The clinic receipt remains an observation without a causal measurement. A replay can change which document arrives first, but cannot turn a blank field into a name.

---

## 22. Location voice and sound notes

### 22.1 Sound palette

- A dry paper edge moving under a door.
- One relay click recorded on a handheld device; no repeating heartbeat rhythm.
- Vent wind crossing a loose cover plate.
- A boot sole brushing grit off the stair.
- The room's acoustic softness after the outer door shuts.
- A short local test tone that stops on its own.

The sound should not imply that the station is operating. Do not use an active generator loop, energized transformer hum or rising electrical whine unless the current location state and audio registry support it. Silence is an acceptable result.

### 22.2 Weather exterior

Rain ticks against a metal canopy. Frozen runoff leaves a narrow line on the north stair. Snow can hide one landing without burying the whole entrance. The player sees the existing weather state through ordinary changes in visibility and ground condition; the expansion does not define new weather effects.

### 22.3 Light

The exchange is not lit by a mysterious emergency bulb. The party's current light source, daylight through a stairwell, or a safe fixed work lamp can make the documents readable. If no item-based lighting system exists, the room uses authored ambient light and does not pretend the player has a usable lantern.

### 22.4 Touch

The switch-room door has a cold metal handle. The paper below it is dry on one edge and soft on the other. The battery's casing has a white film. The player is told not to touch exposed electrical parts. There is no tactile puzzle that rewards unsafe behavior.

### 22.5 Smell

The room smells of damp insulation, paper dust and a faint mineral tang from the floor drain. It does not smell like ozone unless source evidence supports an electrical event. This keeps the scene materially grounded.

---
## 23. Cross-system interactions and implementation boundaries

| Current owner | Content connection | Player-facing result | Do not add |
|---|---|---|---|
| Wasteland map | The exchange lies in the Metro Service Ring with authored route edges | A real destination and approach decision | A second map or route graph |
| Expeditions | Time, party, supplies, return and current risk | The investigation has a cost and may be deferred | A custom substation mission scheduler |
| Weather | Visibility and route conditions for the present trip | Safer, delayed or abandoned visit | New weather states or hazard formula |
| Inventory | Existing battery/cable item categories through current loot data | Salvage exists only if catalog and loot result say so | A fictional battery that powers a region |
| Holdfast power | Present-day contrast for a character conversation | The player understands the schedule is historical | Rewiring the current grid with a found paper |
| Radio | Existing station/frequency route for a local test or transcript | Communication can fail in a legible way | New station network or trust store |
| Journal / records | Conflicting sources can be summarized | Evidence survives the expedition if the owner supports it | A new testimony archive |
| Rumor / reputation | A simplified report may travel if current consumer supports it | Dialogue can reflect the player's wording | A rumor score or a new faction standing |
| Quest/encounter data | Branches, visit order and a single report choice | The player can resolve or defer the account | Parallel custom quest authority |

### 23.1 Content implementation sequence

1. Re-read the location, map, expedition, loot, power, radio, journal, rumor and quest consumers at the implementation date.
2. Confirm the site can be visited and the current expedition can carry the planned interaction without adding an unrelated feature.
3. Run a data census for “Electrical Maintenance Exchange,” Metro Service Ring, winter-service records and the proposed names. The static references found during drafting are not exhaustive.
4. Decide whether the five-scene spine fits an existing quest catalog. If it does not, claim one narrow route and catalog path before editing.
5. Author the sequence, validate it in the current data-integrity gate, and prove that every proposed ID has a live consumer.
6. Add the reports and side stories in a second tranche only after the first story is reachable.
7. Test any persistence needed by the report choice using the current save owner. Do not create a regional-power save section.
8. Record limitations plainly. A working build is not proof that a player can reach the evidence or observe the outcome.

### 23.2 Explicit non-goals

- No electrical grid expansion or multi-site power distribution.
- No player-operated high-voltage switches, wire-cutting puzzle or live test set.
- No new battery chemistry, storage model, power generation, load class or utility currency.
- No hidden machine, sentient transmitter, paranormal signal or remote voice.
- No new clinic system, patient ledger, hospital population or death consequence.
- No new faction, utility guild, technician profession or settlement.
- No full regional event log or generalized correspondence system.
- No new branch of the Holdfast power UI.
- No conversion of a historical schedule into current electrical instructions.

### 23.3 Data-only viability

The story is likely expressible as content if current data supports multi-stage quest branching, map-triggered encounters and a journal outcome. The later rumor callback is optional. If that callback requires a new cross-system event or save owner, remove it. The expansion's core is one location, a report and a human choice. It does not depend on adding a new technical feature.

---

## 24. Faction, social and personal response

No faction owns the exchange by design. If current territory data assigns the site to a faction, a promotion audit must let that assignment stand and determine whether existing territory controls already alter encounter or access. This plan does not create a utility faction or repaint the map to make the story convenient.

### The Holdfast electrician

They want the schedule for its labels and refuse to use its load order. They have a practical reason: an old feeder order belongs to a different station and a different set of loads. They may ask the player to leave the original copy in the archive rather than pin it beside the current board.

### The Quartermaster

They want to know whether the battery is valid and whether cable can be used. They do not care which report sounds best. Their immediate concern is that an uncertain item not be counted as reliable stock.

### The radio operator

They understand that a carrier can lose a word. Their skill gives them the vocabulary to describe the gap, but it does not let them recover missing audio. They may be frustrated by people who treat the recording as a clear order.

### The medical worker

They care that Dema recorded the room's warmth, because someone noticed the conditions of waiting. They will not infer a patient outcome from an electrical schedule. Their disagreement with a public rumor is grounded in professional restraint, not emotional detachment.

### An outside contact

A contact may want the concise story because it gives people a reason to remember the exchange. The player can share the short version with its caveat or refuse to repeat it. The contact may remain unconvinced that the caveat matters. No branch converts disagreement into villainy.

### Eda's name

A later generation may remember Eda by name if the player published the name-based report. If the player preserved only the role, the public account says “operator.” Neither choice changes what the historical documents prove. The player's responsibility lies in how they record evidence, not in changing the past.

---

## 25. Creative attack pass

### “Is this another power feature plan?”

It becomes one if the design adds external load simulation, a restoration panel, cable network state or new grid priority. Those elements are explicitly outside scope. The story uses current power management as a contrast, not an implementation target.

### “Is this a moral dilemma with a technical costume?”

It becomes one if the player is asked to choose the clinic over the station without knowing the costs, or if the story names a patient death to force a preferred answer. The actual decision is how to report incomplete evidence. The immediate choices are factual, social and practical.

### “Is the truth too vague?”

The present truth is clear: a lower feeder was open; a correction was sent; there is no acknowledgement; a room reported warmth; the line was later closed. What remains uncertain is who received which order, which load caused the warmth and who moved the switch.

### “Is the player being asked to be a forensic expert?”

No. They compare words and dates in documents. They are not asked to diagnose a circuit, operate a panel or identify a person's handwriting with certainty.

### “Does the story depend on luck?”

No. All critical evidence is authored. Expedition risk may affect access or cost through current systems, but it does not randomize the historical record.

### “Could another story use this location more simply?”

Yes: a conventional battery salvage run would be simpler but would add little narrative value. This story is justified only if the documents, witness voices and report choice remain strong enough to be worth the player's time.

### “Is Eda made a hero?”

No. She made a switch change. She may have believed the read-back. She may have misunderstood the order. The surviving evidence cannot fully determine what she knew. The story preserves her work and its limit rather than declaring her heroic or guilty.

---

## 26. Failure, refusal and accessibility

- A player can leave at the outer door with no penalty beyond the actual expedition cost.
- No scene requires understanding electrical terminology. A short inline glossary can clarify feeder, route, receipt and read-back using plain language.
- All crucial information is conveyed in text; sound is supplementary.
- Documents should fit the existing readable text width and support keyboard/controller navigation if shown in a panel.
- Choice wording should state whether the player is publishing, copying, carrying or leaving the source document.
- The game should never require a timed response while a document is open.
- A player can resume after saving without replaying every conversation; use current quest persistence only.
- A low-contrast paper texture must not carry information alone. Dates, names and crossed-out lines remain available as text.
- If the user setting reduces motion, the room's lighting and sound have no required strobe or shake.
- If a route is inaccessible because of current party or weather state, the player can postpone. The story does not silently expire.

---

## 27. Acceptance criteria for a future promotion

1. The player reaches the exchange via current map and expedition owners.
2. The site presents the existing hazard and loot profile honestly.
3. The player can find at least the planned order, one field record and one clinic account without operating live equipment.
4. The report choice distinguishes confirmed facts, inferences and unknowns.
5. The public version cannot call a person guilty or claim the clinic's warmth came from a specific circuit without evidence.
6. Any item reward comes from current item/loot data and a real inventory path.
7. Any journal, rumor, relationship or faction consequence has an existing named owner and an observable consumer.
8. The completion state survives save/reload only through the current quest/save owner; no new save authority is added.
9. Content-integrity and content-utilization checks report no invalid or orphaned records.
10. The player can decline, withdraw or withhold publication and still complete the expedition loop.
11. The final interface never presents the old switching schedule as a current operating manual.
12. Expansion 63 can ship by itself if Wave 11's other plans are delayed.

**Recommended first content slice:** outer door, planned order, field card, clinic receipt, and the careful public report. This small slice proves the narrative form and establishes that the story works without new mechanics. Add voice logs, rumored versions and return callbacks only when the data consumer can present them.

## 28. The night of the switch: a source-by-source reconstruction

The player can lay the papers in a sequence, but the game should not present one omniscient transcript. Each card below names its source and the limit of its clock. The sequence helps the player understand what may have happened without filling gaps.

### 18:42 — dispatch note

> Correction sent to lower route: one transfer allowed after relay test. Hold station lighting. West room load may be placed second if the lower route is clear. Read back the full station name. Do not accept “lower open” as confirmation.

The note was written at the desk. It does not prove the field team received it.

### 18:49 — carrier log

> Tone received. First word lost. Repeat requested.

The time belongs to the desk clock. The receiver's internal clock was last checked two weeks earlier.

### 18:55 — runner slip

> Leaving with clinic copy and west-room blanket. Taking upper stair. No vehicle. Return by lower crossing only if marked open.

The runner's departure time was written after they returned. It may be rounded.

### 19:03 — field card

> Relay test incomplete. Lower feeder held. Route passable at east marker.

The card has a scratched line through “held.” The player cannot tell whether the correction was made before or after the next entry.

### 19:08 — Beren's note

> Needle moved. Counted fifty. No read-back. Spool remains on trolley.

The note does not say what the test set measured. It records a visible movement and a spoken count.

### 19:10 — Elen's desk entry

> Lower status not acknowledged. Request close order after one transfer. Send by runner if carrier fails.

The phrase “after one transfer” may have been added later. The ink is darker than the original entry.

### 19:14 — clinic receipt

> West wall less cold. Blanket warmer light at plug. No gauge. No patient count.

The time is penciled in a margin that was already folded. The receipt may have been completed later.

### 19:20 — station note

> Lower feeder open. Transfer held. Relay down.

The note does not name the operator or identify what current was flowing. It may be a status snapshot rather than a command.

### 19:23 — runner return

> Copy delivered. Door shut. No answer from station.

The time is visible, but the route sequence is not. “Door” may mean the clinic door or the exchange door. A character can point out the ambiguity without solving it.

### Next morning — shift report

> Lower feeder isolated. No acknowledgement attached. Clinic room available. Return equipment to exchange.

This is the last entry in the file. “Clinic room available” does not state whether the room was used, warmed by the feeder, or warmed by another source.

### What the player can conclude

- The dispatch desk issued a correction.
- At least one carrier message was incomplete.
- A lower feeder was recorded open at some point.
- A clinic room reported a change in warmth.
- The feeder was later isolated.
- The record does not confirm the operator, current draw, patient outcome or exact duration.

### What the player cannot conclude

- That one person knowingly disobeyed an order.
- That power from the station was diverted to the clinic.
- That a specific patient lived or died because of the switch.
- That the lower route was electrically powered when the spool moved.
- That “fifty” was a measured time.
- That all documents refer to the same clock or the same meaning of “lower.”

The narrative should let players make their own causal theory. The official report remains narrower than the theory. A strong story can leave interpretation open while keeping the observable facts clear.

---

## 29. Public hearing scene: words read aloud

If the current quest/meeting surface can host this scene, the player may choose to read one public version aloud. The scene is a small room after work, not a courtroom. People bring practical objections. The player is allowed to end the conversation.

### Version A — confirmed sequence

**Player reads:** “A lower feeder was recorded open. A correction was sent, but no acknowledgement survives. The west room reported warmth without a gauge reading. The records do not establish which load caused it.”

**Electrician:** “That is accurate.”

**Contact:** “It sounds like no one did anything.”

**Player:** “It says what they did. It does not say why the room warmed.”

**Quartermaster:** “Then keep those two lines together.”

### Version B — role-based account

**Player reads:** “During the winter shift, an operator moved the lower feeder during a communications gap. The surviving file does not show who issued the order or who acknowledged it.”

**Survivor:** “There was a person at the switch.”

**Player:** “The paper proves a role. It does not prove a name.”

**Survivor:** “Would you say that if you knew the name?”

**Player:** “I would say what I knew.”

The survivor may accept the answer, remain unconvinced, or leave. No outcome is framed as a reputation failure.

### Version C — withheld report

**Contact:** “Are you going to put anything on the board?”

**Player:** “Not yet.”

**Contact:** “Then the short story will stand.”

**Player:** “It may.”

**Contact:** “You can stop it.”

**Player:** “I can add another story. I cannot stop everyone from telling the first one.”

The conversation ends without a public report. The journal retains the player's reason only if a current note/quest owner supports it.

### Version D — player declines the reading

The player can choose “Let the electrician read the technical note” or “Do not read this aloud.” The electrician reads only the schedule header and the warning. They say, “This is not an instruction for our grid.” The room moves on to the next work topic.

The player does not have to perform as a public speaker to resolve the story.

---

## 30. Additional callback lines

These callback lines should appear only when the player has reached the relevant evidence. They are not a universal endgame summary.

- **After finding the field card:** “The lower was open once. The time is not clear.”
- **After finding Elen's correction:** “She changed ‘unapproved’ to ‘not acknowledged.’ That is a change in the record, not a confession.”
- **After finding the clinic receipt:** “The west wall was warmer. The paper does not say why.”
- **After hearing the short rumor:** “The story has a switch and a room. It has no receipt.”
- **After returning the battery:** “The book lies flat now. Nothing on the board has moved.”
- **After withholding publication:** “The Holdfast has no public account. It still has your notes.”
- **After the public notice:** “Someone quoted the first sentence and missed the second.”
- **After all North Service Packet plans:** “The three sites agree that a route was planned. They do not agree on one complete order.”
- **After the player refuses to name Eda:** “The report uses ‘operator.’ The name remains in the source pages.”
- **After the player names Eda:** “The report names Eda Venn and shows why the evidence points to her. It also says the order and read-back are uncertain.”

### Callback conditions

Each line is conditioned on an exact evidence or report state. If the current quest flags cannot support that distinction, use a single neutral summary rather than a line that assumes the player found a document. Do not bind a line to a day number unless the game already has a meaningful, stable time gate for this story.

---

## 31. Final design position

The Switching Book is not about making the player operate an electrical exchange. It is about reading a work order that arrived late, a report that used a stronger word than its evidence supported, and a room whose warmth was observed but not measured. The player goes there with a practical expedition need and leaves with a record they can either publish or keep.

The most important action is not pulling a lever. It is choosing whether “unapproved” should remain on the page after the author of that word has corrected it to “not acknowledged.” That change does not prove that the switch was safe or right. It gives the next reader a truer sentence.

---

## 32. Source and drafting note

The location and route facts in this bible were checked directly against the current workspace files named in Section 2. Older context documents were treated as indexes rather than current proof. The existing data confirms the destination, route family, danger and listed loot categories; it does not confirm a regional grid simulation or a current player action on the old switchgear. Every proposed item reward, report callback, radio delivery and social effect requires a fresh consumer audit before implementation.

The proper names in the story are draft identities. A search for the selected names was clean in the local data and lore corpus at drafting time, but the final integration pass must repeat a case-insensitive proper-name and content-ID census. The string “A. Venn” in the Wave 11 cache draft is a proposed clue, not current canon. If either identity conflicts with a current survivor or historical record, revise the new draft rather than overwrite the established material.

**Character-count target:** approximately 120,000 Unicode characters including spaces, line breaks and headings. The text is authored narrative content and design context; no section is intended as filler.

## 33. Six spoken accounts

These accounts can be heard through existing character dialogue or represented as interview notes. They are not all available in every playthrough. None is a complete explanation.

### Account 1 — The electrician

“I have seen the board. I have seen what a line can carry. I have not seen the night itself. If you ask whether Eda opened the lower feeder, the card says it was open. If you ask whether she had permission, the receipt is blank. If you ask whether the room warmed because of it, the clinic note says no gauge. There is a difference between knowing the switch moved and knowing what the movement did.”

### Account 2 — The route survivor

“I was at the crossing after the carrier went bad. I did not see the exchange. We had the spool. We had one route card. We did not have the schedule. The line was quiet by the time we moved the cable. I wrote that because I wanted the next person to know what we did. The later report says we moved under power. We did not.”

### Account 3 — The quartermaster

“People want the count because a count can be replaced. If the battery is good, we know what we have. If the cable is intact, we know what we can use. A name is harder. A name does not tell me the charge. But if we leave the name out, people may say we hid the person. I want the report to tell the truth it can hold.”

### Account 4 — The clinic worker

“The west wall was warmer when I came back. I did not measure it. I had no thermometer. The room was in use before the exchange called. I wrote what I could feel because the next worker asked whether the radiator had been on. I do not know whether it was. I know where the blanket was. I know the window had been covered. I know the room stayed usable.”

### Account 5 — The radio operator

“I hear the gap in the recording. I can play it until the speaker wears out. The missing word remains missing. I can tell you what a carrier sounds like when it drops. I cannot tell you which order a person heard after that. A repeat request is not the same as a read-back. A read-back is not the same as a receipt. The paper has three boxes because the work had three steps.”

### Account 6 — A listener at the trade stop

“The short story came to us first. One person said the station went dark. Another said the clinic was saved. I believed it because both sounded possible. Then the Holdfast report came. It had too many conditions for a good story. I remember them now anyway. There is a feeder and a route. They were different things. The person who told me the first story may not have known that.”

## 34. Site-state observations

### Before any document is read

The player hears a low scrape as the outer door settles. They see the HOLD label, the dispatch desk and the battery under the book. The text does not name a past event. It says only that the room was built to coordinate work and is now unused.

### After the planned order is read

The word “correction” becomes noticeable on the paper strip. A character can ask whether the field team received it. The player knows the planned order but not the action.

### After the field card is read

The room feels more active because the player has seen an actual position. The location text refers to the unmarked time field. No lever animation plays.

### After the clinic receipt is read

The player may notice the west-wall stain and broken gauge. If that object is not accessible in the current scene, the receipt stands on its own. The game must not teleport the player to a clue they did not inspect.

### After a report is published

The exchange is unchanged. The player has changed the public record at the Holdfast, not the old station. A later visit can show dust on the clip and the page still under the battery. If a state change is not persisted, the callback should be a journal reflection, not a false visual update.

## 35. Draft content inventory for promotion

A content author can use this list to plan the smallest reachable version. Counts are ceilings, not quotas. An item can be removed when the current catalog or runtime cannot present it cleanly.

| Content group | Proposed maximum | Essential? | Promotion test |
|---|---:|:---:|---|
| Main quest stages | 9 | yes, but may compress to 5 | player can reach/report/resolve |
| Named historic voices | 5 | no | proper-name collision and current dialogue route |
| Public report variants | 4 | yes | branch state survives reload |
| Documents | 18 | no | each record has a discoverable place and source |
| Short encounters | 15 | no | current encounter catalog can select them |
| Radio fragments | 6 | no | current radio path, cue IDs and reachability |
| Site inspection texts | 12 | no | location renderer supports them |
| Cross-wave synthesis | 1 | no | all three flags have a current consumer |
| New gameplay systems | 0 | n/a | reject if required |

The first tranche can ship with five scenes, four public report variants, and a maximum of six documents. If these are strong, the player will understand the site. The rest deepens the story but does not justify inventing more infrastructure.

## 36. Story-specific quality gate

Before promotion, ask a writer unfamiliar with the design to read only the in-world documents and answer these questions:

1. What was the planned order?
2. What was recorded as an actual position?
3. What was reported at the clinic?
4. What did the communication record fail to prove?
5. Which word did Elen later correct?
6. Can the reader distinguish a route from a feeder?
7. Does the reader know that the station's old schedule is not a current operating instruction?
8. Can the reader leave with a reasonable theory that differs from the official report?

If the reader cannot answer the first six, revise for clarity. If they believe the game has secretly confirmed one cause or one person's guilt, revise the source labeling. If they infer that the old board can operate the Holdfast, strengthen the warning and remove any UI affordance that suggests a present-day connection.

The emotional line should remain modest. A room was warmer. A form was incomplete. The next person corrected one word. That is enough to remember.

## 37. Field-note packet: the present expedition

These short notes let an expedition party express a reading without claiming an expert verdict. The party's composition determines which lines can appear; do not have every survivor speak in every scene.

### The route note

> The transit stair is dry from the top to the landing. Water sits at the lower turn. The wall is marked with “north” in two hands. The marks disagree. The map places the exchange beyond the service door. We did not test the lower door. Return route remains the upper stair.

### The party note

> One member asked to leave before the switching room. The request was reasonable. The party had already spent time on the descent and the weather at the top had worsened. We left the route card in place and returned with the map corrected to “site reached, records not read.”

This text is available only if a retreat branch occurs and the party's actual state supports the description.

### The inventory note

> Battery casing present. Charge not known. Cable loop present. End condition not checked. Neither item is entered as reliable stock. The scavenging record may separately show what the party actually carried home.

### The journal handoff

> Evidence copied: planned order, field position card, clinic receipt.  \n> Evidence not found: acknowledgement receipt, full shift roster.  \n> Cause of reported warmth: unknown.  \n> Modern use of schedule: not approved.

### The optional personal note

> I wanted the page to say who moved the feeder. It says “operator.” I wanted the room note to say what warmed it. It says “less cold.” I wanted the two clocks to agree. They do not. I have written those three wants in the margin so the next reader does not mistake them for facts.

## 38. Rules for naming and attribution

The public report may name an individual only if the final content records why the name is supported. The draft evidence uses “A. Venn” on a roster and “E.V.” on a correction. The player can reasonably associate those marks with Eda Venn, but the association is still an inference until a signed note or current source confirms it.

Even when the player chooses the name-based report, the report must preserve the uncertainty of authority and receipt. A name does not answer who issued the instruction. A hand at the switch does not establish that a complete order reached the operator. If the name is omitted, the player is not accused of hiding it; if included, the player is not rewarded with a clean moral resolution.

The clinic account names Dema Rusk only because the receipt identifies the runner. It does not identify patients. The report must not invent patients, their diagnoses, or an outcome. If a current canon character is later discovered to have occupied the west room, the new plan must be reconciled against that evidence rather than overwrite it.

## 39. Optional final conversation with Eda's record

After the player returns and files a report, a note may be found tucked inside the Holdfast archive envelope:

> You have my name from the roster. You have my position from the card. You do not have my instruction. If you write that I opened the feeder, write that you know because the card says open. If you write that I chose the room, write that the room was warm after. Do not write that I knew what the line carried. I did not know.  
> — Eda

This note is not a confession. It is an author's final boundary on what her record can support. It should appear only if the content state makes it plausible that Eda's papers reached the archive. Otherwise it remains a proposed optional line and is omitted.

**Alternative if her papers never reached the Holdfast:**

> The folder is empty where a note might have been. There is no clean outline or staple shadow to say a page was removed. The file contains the papers the player brought back, and no more.

The empty folder does not imply hidden content. It is simply empty.

## 40. Accessibility and presentation copy

Every distinction that matters is represented in words, not only in color, sound or page layout:

- Planned order versus actual position uses explicit headings.
- “Sent,” “acknowledged,” and “received” are written as separate labels.
- Crossed-out language is read aloud by a screen-reader-friendly text line.
- Incomplete dates display question marks in the text and a plain-language explanation.
- Ambient radio noise is captioned as “[carrier static]” rather than left as audio-only information.
- Any portrait or location illustration is optional and does not encode which person is truthful.
- No flashing electrical arc, sudden camera shake or forced dark screen carries a clue.
- Report choices name their disclosure level: public, role-only, private, withheld.
- The player can back out of a document without losing a timed expedition decision.

The prose itself should remain concise at the point of interaction. This bible contains a large authoring corpus so future implementation can select content intentionally. A player should never be forced to read all 120,000 characters to complete Expansion 63.

## 41. Closing quality position

The story closes when the player has decided what account to leave, not when every question is answered. A reliable plan names what was supposed to happen. A field card names one position. A clinic worker records warmth. A radio operator describes the lost word. Each source does a different job. The player makes a public sentence from their overlap.

The sentence should be honest enough that a later reader can disagree with the interpretation without disputing the record. The exchange stays cold and still. The Holdfast carries forward a better distinction between a route and a feeder, a sent order and a received one, a name and a proven action. That is the lasting content reward.

**End of Expansion 63 design bible.**

## 21. Additional records: the final pages

The following pieces broaden the archive without introducing a second mystery. They are all consequences of the same incomplete correction: people kept making local decisions while the written state lagged behind them.

### 21.1 Folded notice on the stair rail

The paper has been cut from a larger sheet. Two rusted staples remain at its top edge. The exposed side contains three lines in pencil and a fourth line written after the paper was folded.

> If the board says WEST, read the date first.
>
> This copy is not an instruction to switch.
>
> It records what we told the next crew.
>
> —M.

The player can recognize the hand as Mara Kett’s after finding her witness card, but the note does not need that recognition. The fold crosses the word “date,” making it look like “gate” until the page is flattened. That small misreading echoes the location’s central problem: an apparently legible mark can survive after its context has gone.

### 21.2 Duplicate sheet with a clean surface

This page looks like a prepared duplicate. No coffee stain, boot mark, or glove transfer interrupts the boxes. The destination field says “Clinic West Wall.” The state field is blank. A neat pencil line occupies the acknowledgment field, but the line is not a signature.

A player who assumes it is a completed sign-off will later find the same sheet under a tray with the words “copy only” on its reverse. A player who inspects the reverse at once can still keep the sheet as evidence. Neither reading alters power. It changes the report the expedition writes and the language available in the final scene.

### 21.3 Message on the inside of a tool case

> Eli:
>
> I found the clip you wanted. It holds the paper flat if the wind comes through the west door. It does not make the numbers true.
>
> I am leaving it on your chair. I am not staying for another handoff.
>
> J.

No one comments on whether the clip was actually collected. It is a tiny human message adjacent to the larger records, evidence that the staff were trying to make a safer procedure from whatever they could carry. Its last sentence belongs to a personal departure, not a secret plot.

### 21.4 Inventory correction, written in the margin

The official list says the red tag roll contains forty labels. The marginal note says:

> Counted thirty-seven. Three used to label the pages we could not reconcile.

This is not a hidden puzzle with three missing doors. The note makes the inventory less trustworthy in one narrow way and more honest in another. If the player chooses to cite the quantity, a careful report calls it a later estimate. If they cite the reason, the report can explain why the packet is incomplete.

### 21.5 The unaddressed envelope

The envelope contains a single blank acknowledgement card. A pencilled name, “M. Kett,” has been rubbed until only the first initial remains. The flap was never sealed. It is not addressed to the clinic, the rail crew, or the depot. It might have been kept ready for a person who never returned.

The scene does not resolve the envelope’s recipient. If the player asks Mara’s name during the final reading, she answers, “You have the part I signed.” This closes the immediate question with a boundary rather than a new quest.

## 22. Two versions of a truthful ending

The resolution should respond to the player’s reporting choice, not assign a moral score. Both versions preserve uncertainty and avoid a mechanical reward for “correct” interpretation.

### 22.1 The careful account

If the player separates dated copies from acknowledged instructions, the community receives a short account with three columns: “confirmed,” “reported,” and “not established.” The reading scene is quiet. People ask who will keep the clean copy and whether the clinic report can be repeated. The player can decline to attribute the west-wall note to a person.

Mara says: “That is enough to pass on. It gives the next crew somewhere to start, and it does not tell them what to touch.”

The final log records that the switching packet has been archived with its conflicting sheets intact. It does not record a new grid state or grant access to a fictional route.

### 22.2 The abbreviated account

If the player prefers not to publish uncertain details, the community receives only the chronology and a note that the west-wall report remains unverified. This is a valid ending. It protects a private name and keeps a rumor from becoming an order. It also means the next reader sees less of the reason the pages were preserved.

Mara says: “You can leave the name off. Leave the blank field too.”

The log records a restricted archive entry and identifies the missing acknowledgment as an unresolved source gap. No survivor is punished or embarrassed for having withheld a name.

### 22.3 If the player conflates copy and instruction

The report can still be corrected during the reading. A character points to the date on the folded notice. The player is offered a plain correction: “This page shows what was copied, not what was confirmed.” Accepting the correction revises the archive label. Refusing it leaves the player’s account visibly contested, but still does not trigger a live switch or make the shelter lose power.

This is the only ending variation that foregrounds a mistake. It is designed to teach source handling, not to shame the player.

## 23. Content integration boundaries

The packet belongs to the existing location, encounter, expedition, journal, and dialogue systems. Its authored material can be represented as ordinary content entries after a later promotion review. This plan does not ask for a generic grid dashboard, a feeder model, a command interface, new electrical equipment, a real-time switching minigame, or a simulated clinic circuit.

The following are explicitly outside this proposal:

- changing the present power ledger, production, consumption, or outage behavior;
- adding a new save section or a second record ledger;
- making a dialogue choice operate a physical breaker;
- treating the expedition’s observatory loot-table reference as a claim that this site is an observatory;
- granting a permanent bonus to power generation, repair, or expedition safety;
- converting the dated packet into a route-wide quest chain;
- exposing a new player-authored switching order as authoritative data.

A promotion pass should map each scene to current narrative content contracts and current flag ownership. It should verify names, IDs, and all proposed conditions against active catalogs at that time. If the existing journal cannot distinguish “found copy” from “acknowledged instruction,” the plan should use prose and a single discovery flag rather than expanding the journal schema without a named decision.

## 24. Acceptance read

The story is ready for a future content pass when a player can explain four things in their own words:

1. The packet contains copies from different moments.
2. A correction being written is not the same as that correction being received.
3. The clinic west-wall report is useful testimony but not proof of a present circuit state.
4. A responsible account can preserve disagreement rather than smooth it into a single clean instruction.

The writing should leave the player with one practical habit: check who wrote a page, when it was written, and whether the next person acknowledged it. That habit is the story’s outcome. The world remains dangerous, the clinic remains dependent on the existing power system, and the packet remains a record of people trying to communicate under pressure.

## 25. Draft status

This is a content proposal for review, not a signed implementation package. Site and system references are grounded in the current repository snapshot and must be rechecked before promotion. A future owner should claim the exact content paths, resolve any duplicate names or flags, validate the dialogue against the existing UI length and accessibility rules, and test the actual discovery route. If any source record conflicts with this draft, the source record wins and the prose should be revised.

The planned scale comes from the number of authored scenes and records, not from repeated feature requirements. Every proposed piece either changes how the player reads the switching packet, gives a witness a distinct reason for speaking, or makes the final account more specific. Remove any item that fails that test during implementation.
