# ASHFALL — Expansion 62 Design Bible
# THE CACHE GRID
## A Season's Worth of Rope

**Wave:** 11 — The Work Between Storms  
**Expansion:** 62  
**Document status:** Content-first design bible; proposal only, not an integration claim or authorization.  
**Target length:** approximately 120,000 characters, including authored in-world material.  
**Primary site:** Forestry Emergency Store, North Woods  
**Anchor:** loc_forestry_emergency_store  
**Content authority:** Assets/StreamingAssets/Data/  
**Narrative mode:** expedition investigation, resource custody, route knowledge, small-community memory.

---

## 0. Purpose and reading contract

This document proposes a playable story package, not a new forestry simulator. The player enters the North Woods because the map promises fuel, rope and insulated boots. The place initially looks like a familiar salvage stop: a cache sunk into a slope, a compartment grid, a forestry form, a logging spur, and the words “two crews, one season” on an unsigned copy. The story begins when the quantities on that form do not agree with the goods on the shelves, and the discrepancy has a human explanation that no surviving record has written down in one place.

The expansion asks what an emergency reserve is for after the emergency plan has become impossible to follow. The written inventory names equipment for two fire crews. The site holds fewer tools, extra blankets, a child's mittens, and a jar of stove bolts. The missing items did not vanish into a convenient cache of loot. They went out on separate days, to separate people, under pressures that were never meant to become a precedent. The player can discover this through travel notes, use marks, conversations and the route itself. The final choice is practical: leave the stock sealed as a reliable reserve, release it to the people who now depend on the spur, or divide it under a posted and imperfect rule.

The writing is intended to carry the expansion. Field cards, a damaged cache form, a returned rope, two versions of the same oral account, and the dialogue around issuing one pair of boots should make the player understand the old crew before any character explains it. The gameplay remains inside existing owners: expedition dispatch and risk; map and route knowledge; inventory; weather; shelter needs; current hunting and wildlife content; survivor relationships and reputation where a current binding exists. No second cache ledger, stockpile authority, forest ownership system, route scheduler, or weather model is proposed.

### Reading key

- **Live evidence** is a current file or catalog fact verified during this draft.
- **Proposed content** is new story, dialogue, text, or content identity to be authored only after a duplicate and identifier pass.
- **Recheck before promotion** means the consuming source path must be examined again when this design is picked up. A prior plan, heading, test name, or static catalog row does not prove present reachability.
- **Playable prose** means text that changes what the player knows, risks, refuses, promises, or does. It includes environmental descriptions, not only dialogue.

The content package should be read in order once, then used as a reference. Sections 1–12 establish the story. Sections 13–22 provide the authored packet: quest scenes, branches, field documents, encounters, callbacks, implementation boundaries, and a collision audit. The large sample corpus is deliberate. If the package is accepted, authors should adapt its content into the existing catalog grammar rather than copy this markdown into a runtime text field.

---

## 1. Expansion thesis

**The Cache Grid** adds a grounded, replayable expedition story about emergency stock and the difference between a rule that protects a community and a rule that protects a form. It takes a lightly authored destination with a strong physical premise and turns it into a small region the player can read in layers. The cache is not a treasure room. It is the surviving edge of a work system: marked trees, foot bridges, a stump line, a timber sled, and stores placed where a crew could reach them after the road failed.

The central pressure is not “take supplies or save people.” The player receives no single clean count of who is waiting beyond the forest. Instead, the route notes contain partial evidence: one pair of boots is too small for either listed crew member; a rope has been returned dry but cut into shorter lengths; the stove-bolt jar has a newer lid; a receipt records meal flour delivered to a house that does not exist on the map. The player can carry out a confident but wrong interpretation, return to ask a survivor who worked the ridge, or spend another expedition day following the cache-grid marks.

Three experiences are added:

1. **Reading a work landscape.** A player learns to distinguish forestry marks, storm damage, human repair and deliberate route marking through objects and short observations. The tree line is not a puzzle lock. It is a record written at walking pace.
2. **Making a public promise with finite stock.** The player chooses what to do with a reserve whose intended users are gone, partially known, or represented only by marks. Existing inventory and reputation owners carry the consequences if their current seams support them; the design does not invent hidden stock or invisible trust.
3. **Returning to a place that has changed because of a prior decision.** A second or third visit shows missing stock, replacement labels, a path kept open or allowed to close, and another character's interpretation of the player's posted rule.

The signature image is a coil of rope hung from a nail at shoulder height. The first length is good. The second is cut and spliced. The third is wet, frozen into a flat loop, and tagged with a date. Someone returned all three because the original request said “return if serviceable.” The person who returned them was not named. The rope has been more carefully accounted for than the people who used it.

### Player promise

The player can leave the North Woods with something useful, but usefulness is not measured only in carried weight. One route may become safer to read and still cost more travel. A cache can be opened for a specific need and still make later rescue harder. A posted rule can be fair on paper and callous in practice. The game should never shame the player for prioritizing the Holdfast's immediate survival. It should show what that choice cost and preserve the reason it was made.

### Experience boundaries

This is not a forest-building expansion. It does not add a timber economy, tree-felling loop, wildfire simulation, winter travel subsystem, or settlement. It does not claim that the cache can restore an old forestry network. It does not turn the North Woods into a faction capital. The story is about one reserve and the small network of choices around it.

---

## 2. Canon fit and current evidence

### 2.1 Direct evidence in the checked-out data

The following evidence is present in the current workspace as of the drafting pass:

- Assets/StreamingAssets/Data/locations.json defines loc_forestry_emergency_store as a backcountry ranger supply depot and fire-watch cache in evergreen foothills. The text names axes, chainsaws, safety harnesses and winter rations in galvanized drums. It assigns danger 4, travelHours 2.0 and baseRadsPerHour 18.
- Assets/StreamingAssets/Data/wasteland_map_v1.json contains the destination and its map edges. It connects the store with loc_diesel_tank_farm and loc_cut_abandoned_depot; the route records carry terrain and hazard data owned by the map catalog.
- Assets/StreamingAssets/Data/expeditions.json describes the destination with a 16-tick distance, danger 3, encounter chance 0.125, stamina drain 2.45, the existing forestry-compound scavenging table, and fuel canister, rope, and insulated boots as loot categories.
- Assets/StreamingAssets/Data/damaged_map_zones.json identifies the North Woods zone. Three fragments are named: a Firebreak Seven compartment map, a logging-spur survey, and an emergency cache grid. The cache form says “two crews, one season.” The revealed list includes fuel, rope, and insulated boots.
- docs/cartography/PLAN85_COMPLETION_REPORT.md places the North Woods among the damaged-map destinations; docs/cartography/PLAN85_BALANCE_MATRIX.md and docs/cartography/INSTALLATION_LOOT_PROVENANCE.md document the existing destination/loot mapping.

These rows provide a real starting seam. They do not prove that a particular branch is reachable, that a proposed item is present, or that every route edge has a live consumer. Those points must be checked at promotion.

### 2.2 Existing systems this story can reuse

- Expedition dispatch, return, risk and carried supplies.
- Wasteland map discovery and authored route knowledge.
- Weather forecasts and current travel hazards.
- Inventory and existing equipment items.
- Shelter needs, work assignments and ration choices.
- Current wildlife, hunting and trapping content where existing rules permit it.
- Survivor relationships, moral choices, reputation, journal and chronicle surfaces where current content routes already exist.

The story may mention a system only when the player can observe or affect it through a current owner. For example, a line about “the pass being worse in crosswind” must agree with authored weather and route data if it creates a travel expectation. A narrative statement that the Holdfast issued rope should correspond to an item or an existing resource abstraction. No text should promise a machine function that the shipped runtime does not perform.

### 2.3 Extension classification

- **Story/content:** extension of existing locations, maps, expeditions, quests, encounters and diegetic records.
- **Runtime:** DATA ONLY if current quest and location consumers can express the sequence; DATA + WIRING only for the smallest proven route or callback seam.
- **Core extension:** out of scope unless a promotion audit identifies a specific existing API gap and the foreman separately accepts it.
- **Foundational architecture:** explicitly rejected. The story does not need a new cache manager, region simulation or forestry authority.

### 2.4 Canon link to the rest of Wave 11

The three Wave 11 plans share a small paper trail called the **North Service Packet**. It is not a new organization or a hidden master conspiracy. It is the ordinary set of forms produced when forestry, electric utility and materials staff prepared for a long winter: a cache grid; a switching order; a cold-weather material test. Each plan is playable by itself. The shared packet adds one late callback: a handwritten request to keep the north service path open for one clinic delivery, then a later correction saying the request was cancelled after the clinic moved its patients.

Expansion 62 introduces the cache-side copy of the request. Expansion 63 places the utility copy at the Electrical Maintenance Exchange. Expansion 64 locates the test sheet at the Materials Research Sublevel. Each copy is incomplete in a different way. None names a saboteur. The conflict comes from mismatched dates, the loss of the radio carrier, and the fact that the person who signed the last form was no longer at the station when the route closed.

---

## 3. Thematic design and content rules

### Theme: a reserve is a promise made to an unknown day

The cache was stocked for an emergency that did not arrive on the schedule printed in the binder. The fire crews never used the stores for the reason they were purchased. Later workers used some of them for ordinary injuries, broken bridge planks, wet socks, a stove that would not draw, and one trip made after the road crew stopped answering. The reserve did not fail because the foreman was evil or the forms were foolish. The reserve worked in some ways and failed in others because no one could keep the plan current while the weather, staff and roads changed.

### Writing rules

- Show the history in scuffs, repairs, handwriting, folds, frost lines, inventory marks and the order in which objects were left.
- Keep dialogue concrete. Characters may disagree about a count without delivering speeches about society.
- Do not use a child as an automatic moral trump card. The small mittens matter because they are tagged with a date and repaired at the thumb, not because a character tells the player how to feel.
- Do not frame the player as a judge of dead forestry workers. The player is making a present decision with incomplete evidence.
- Distinguish fear from deception. A person who withholds a route detail may be protecting a living traveler, not carrying out a grand plot.
- Do not make the wilderness monstrous. Cold, terrain, wind, damp rope, bad visibility and fatigue are enough.
- Use no real national agencies, real logging companies, real historical disasters or copied fiction.
- Use no supernatural explanation. The “voices” in the fire lookout are wind through a loose louver, if that source is eventually checked; otherwise leave the sound undescribed rather than suggesting a haunting.

### Authored-content share

The expansion is designed around three recurring writing forms: the compact field label, the practical conversation, and the longer account written after someone has had time to decide what not to say. Each form has a distinct voice and a different reliability profile. The field label is precise but old. The conversation is current but partial. The long account has memory and motive. The player should have to compare them.

The authored corpus later in this bible contains proposed quest dialogue, encounter outcomes, cache cards, two letters, a radio check, a crew log, a winter ration notice, and a player-facing closing record. These are usable writing drafts, not decorative placeholders. Promotion should preserve their words where they fit the current data schema and revise only where a live consumer or continuity check requires it.

---

## 4. New design space: the cache as a readable place

A visit should unfold through a sequence of small recognitions rather than a checklist of containers.

### First arrival

From the logging spur, the store is difficult to see until the player notices the slope was cut in a straight line. The roof is not a roof at first glance. It is a low plane of old snow, pine duff and branches laid crosswise. One galvanized vent has been replaced with stovepipe. The replacement is too wide and carries a streak of soot down its leeward side. No smoke has risen from it in years, but the soot stain is recent enough to be darker than the lichen.

At the entrance, three notches in the door frame sit at different heights. The top two are similar. The third is shallow and has been cut with a knife rather than an axe. The marks are not a child-growth chart. They are the only surviving record that a worker returned to check the door after the crew roster was closed.

### Interior progression

The store has four visible spaces, each described through its former job and its later use:

1. **Entry bench.** Mud, boot nails, a strip of black tape on the wall, two stamped dates, and the practical instructions for keeping wet gear off a sleeping person.
2. **Tool wall.** Empty outlines from removed tools; a chain saw bar pinned with its correct file angle; a replacement wrench that does not match the bolt heads on the pump.
3. **Dry shelf.** Rations, rope, mittens, and small repair parts. The shelf carries two counting systems: a printed form and graphite marks on the end grain.
4. **Fire-lookout stair.** A route to a view over the timber line. The stair has been repaired from the inside with narrow board, suggesting it was used after the main road closed. The top landing looks outward, but the handwritten bearing is on the underside where rain could not reach it.

The player can inspect most objects without spending supplies. Reaching a low crawlspace, following the ridge marks in poor weather, or carrying a recovered item back to compare against the shelter's records may involve an expedition cost. The story should not create a fake “inspect” skill. It should honor existing survivor skills and expedition rules if they already govern the relevant action.

### Place-state presentation

The site can be presented in three authored states using existing quest flags or route state if current infrastructure permits:

- **Unopened:** cache grid legible; contents mostly in expected places; side vent appears undisturbed.
- **Accounted:** one drawer opened and its contents counted; missing-item marks exposed; a second visit can place new notes or a corrected count.
- **Posted:** the player's reserve rule is left at the entrance; later visitors can read it, accept it, alter it, or leave the paper face-down in rain.

These are narrative states, not a new simulation. If existing quest flags cannot express the required state, redesign the callbacks to use existing content triggers or leave the change as text in the journal. Do not add a location mutation system merely to animate one notice.

---

## 5. Main narrative spine: “Two Crews, One Season”

### Premise

A Holdfast expedition reaches the North Woods to recover a field kit and discovers that the store has been used as a quiet waystation. The map's cache form lists two crews and one season. The actual count includes equipment issued outside the roster and some stock returned after use. There are no complete signatures. The player must determine whether the spare items were stolen, privately borrowed, or issued under a separate emergency order.

### Stage 1 — “The Scored Line”

The player receives a damaged map fragment from the North Woods zone. It shows Firebreak Seven, a bearing, and a penciled note: “cache side.” The expedition briefing names the known loot classes but does not reveal the waystation. The player chooses a route and party using current expedition rules. If the weather or roster makes the long spur unreasonable, the player can delay; there is no artificial deadline that silently kills everyone off screen.

**Arrival line:** “The map says north. The trees say east. Someone cut the turn mark at knee height, below the snow line on a year when the snow stayed.”

At the first marker, the party finds a split sapling with a strip of cloth under the bark. The cloth is not a secret code. It is an old survey ribbon turned inward to keep its color from fading. A second ribbon hangs lower on the same tree. The lower knot is newer and tied by someone who had gloves on.

**Player actions:** follow the marked spur; stop and take a dose/condition check under current expedition rules; or turn back with partial knowledge. The turn-back route still yields the map clue and does not mark the player as cowardly.

### Stage 2 — “The Form”

At the store, the expedition finds the posted inventory. The printed form says two crews, one season. The pencil total says one crew, six nights. The last row has no item name, only “returned / cut / still good.”

A field label on the shelf reads:

> ROPE, 30 m. Take a coil with two sound ends. Leave the short lengths for handline repairs. If wet, hang indoors. Do not pack frozen rope under canvas. It will be cold when you need it and worse when you pull.

The player can count visible equipment, compare it to the fragment, inspect tool outlines, or leave with a partial report. The missing supplies are not all recoverable. Their absence matters because each carries a mark: two sets of boot nails, a rope end with a white thread splice, and a blanket patched with the same canvas used on the cache door.

### Stage 3 — “The Lower Mark”

Following the shallow knife notch on the door frame reveals the older inspection route, which ends at a windbreak constructed from deadfall. This path exposes the first contradiction: a crew log says no one used the cache after the seasonal close, but a tin cup at the windbreak has a newer repair and one of the forestry marks has been cut through the frost-distorted bark. The party also finds a dry boot wrapped in an oilskin bag, with the name removed from its tongue by repeated wear rather than a blade.

The player can take the cup, leave it in place, photograph or record its markings through an existing journal surface, or ask a survivor with forestry or repair knowledge to interpret it if the current roster supports that query. The cup is not a quest key. The story remains completable if it is missed.

### Stage 4 — “A Name in the Margin”

On return to the Holdfast, a survivor recognizes the phrase “cache side” from an old forestry aid card. They remember that Runa Dey, a storekeeper, allowed travelers to shelter in the entry bay during one bad season. They do not know whether she issued tools or simply stopped asking what people carried away. Their account is limited by what they saw: boots drying by the stove, not the names of the people who wore them.

The player can share the full count, report the cache as looted, or hold the report until another expedition. A report of theft is accepted by an existing journal/quest owner only if that route is supported; this proposal does not create a new accusation ledger. The choice controls which follow-up conversation is available and whether the team carries a request to check the lower ridge.

### Stage 5 — “The Return Length”

A second expedition may be dispatched to follow the logging spur toward the abandoned pull-off. A coil of rope is found there, dry and neatly hung under a crosspiece. Its frayed end has been cut square. The tag reads “short lengths retained / long length returned / paid from no account.” The writing matches the store's lower inventory marks, but the tag is signed only with two initials: R.D.

The place also contains evidence that another traveler stayed there: a biscuit wrapper folded into a weather shield, two boot prints preserved in a soot layer, and a small square of cloth sewn over a torn coat elbow. There is no corpse and no miraculous cache. The visitor moved on or was moved by someone else.

### Stage 6 — “The Issuing Rule”

The player finds a later notice, handwritten by Runa Dey after the crew roster ended. It states that the cache may be used by anyone who writes the date, takes only what can be carried safely, and leaves a note about what remains. The notice adds: “Do not count a person as a unit of stock. Do count their boots if you expect them back.”

The wording is severe, practical and incomplete. It grants access but does not promise a replenishment. The player has to decide whether to copy the rule, replace it with a Holdfast reserve policy, or leave no rule at all. The decision should be a real choice about accountability, not a morality meter disguised as one.

### Stage 7 — “One Season More”

A final visit or a later callback reveals what happened to the notice. If the player left Runa's rule, a new entry has been added in another hand: “We took the blanket. Returned it clean. Could not return the heat.” If the player posted a Holdfast rule, the paper has been folded under the bench to stay dry. A reader has written “who decides when the emergency is over?” If no rule was posted, the pencil and form remain side by side; the visible count is now one less, but no one knows whether it left for shelter or accident.

The quest resolves when the player takes an action: release, reserve, split, document, or leave. Each choice has a known immediate cost and an uncertain regional meaning. No ending declares the player morally correct.

### Main spine outcomes

- **Reserve:** keep the cache intact for future expeditions; the Holdfast retains a reliable supply, but the lower route remains an unsupported rumor.
- **Release:** bring useful stock back and/or authorize current users to draw from the cache; the immediate shelter benefits, but future teams must plan around reduced availability.
- **Split:** post a quantity-limited issue rule and leave the remaining inventory; this requires enough confirmed inventory data to avoid inventing stock.
- **Record only:** leave every object in place and carry a full account home. This may improve knowledge or credibility through an existing record route but does not produce loot.
- **Walk away:** preserve the site and spend no further travel. This is a valid ending if the Holdfast cannot afford another expedition.

---

## 6. Questline content packet, scenes A–C

The following scenes are intended as near-final authored material. Branch lines are written in a restrained voice and can be shortened to fit the eventual dialogue catalog. Any mechanical rewards named in square brackets are implementation notes, not literal runtime text.

### Scene A — Before dispatch: “A map that folds wrong”

**Trigger:** first player interaction with the North Woods map fragment or the relevant expedition board.  
**Participants:** Quartermaster, player, optional survivor with route or forestry knowledge.  
**Room facts:** The map fragment is damp; a square of it has been cut out rather than torn. Three route marks are visible; only one is legible at ordinary distance.

**Quartermaster:** “The list says rope and boots. The route note says the store was stocked for two crews. If that means two teams of four, we have no business taking half the wall.”

**Player:** “And if it means a pair of crews stopped there?”

**Quartermaster:** “Then we have no business pretending we can count the people from the forms.”

**Optional route hand:** “Firebreak Seven bends where the slope breaks. The map bends where somebody corrected the map. One of those turns is wrong.”

**Player choices:**

1. “Pack for the marked route. We can turn back if the weather closes.”
2. “Take the short spur first. Leave the lower cache for another day.”
3. “We need the rope more than the story. Keep the party light.”
4. “No dispatch until we know who can read these marks.”

**Outcome text:**

- Choice 1: The party carries the field kit, one warm layer per member and a spare length of rope. The board shows a weather note beside the route. The expedition cost is governed by the current dispatch model.
- Choice 2: The party approaches from the familiar trail. They can reach the store but may not encounter the later spur unless they return.
- Choice 3: The party has room for fewer supplies. A survivor may comment on the weight but does not refuse by scripted fiat.
- Choice 4: The map stays where it is. A subsequent day does not expire the story. The player can return after resolving another need.

**If a forestry-skilled survivor is present:**

“Don't use the ribbons as a count,” they say. “Three colors can mean three crews. It can also mean one crew in spring, one in rain, one after they changed the order. Ask the bark before you ask the color.”

**If no such survivor is present:**

The Quartermaster turns the paper sideways, compares its fold marks with the route lines, then sets it down. “No. I don't know. We will write that we don't know.”

### Scene B — Entry bench: “Dry where a person sat”

The door gives after the party clears an accumulation of needles and ice from its lower hinge. Inside, a bench runs along the right wall. The left half is dry. The right half has a dark crescent where a wet coat once pressed against the boards. A shallow groove has been cut into the bench edge to hold a tin cup so it cannot roll when someone sleeps sitting up.

**First inspection text:**

“The instructions are written at eye level for a standing worker: KEEP WET GEAR OFF THE BEDDING. Below them, in a smaller hand: ‘If you have only one blanket, put it under the sleeping person. It dries slower on top.’ The second line is older than the marker above it. Someone repainted the instruction and kept the correction.”

**Optional inspection:** The player can examine the cup groove, boot scrape, coat stain, and a row of dates. Each supplies a short journal observation. None is required to obtain the ordinary cache supplies.

**Quartermaster, if on the expedition:** “Somebody slept here more than once.”

**Player:** “The log says the store was closed.”

**Quartermaster:** “The log says the issue was closed. The bench says somebody stayed.”

**Player options:**

- “Record both statements.” The journal carries the disagreement without resolving it.
- “The inventory is what matters today.” The party proceeds. No moral penalty.
- “Look for the lower route before we take anything.” This directs the party to the door-frame notch.

**Ambient observation if the player waits:**

A soft tick comes from the back wall. It is not a clock. A roof fastener has loosened and moves against a washer when the wind lifts the tarp above the entrance. If the player follows the sound, the fastener's underside bears a forestry store stamp. This is an optional physical detail that supports the site's repair history.

### Scene C — Tool wall: “The outlines”

The tool wall is a board with pale silhouettes where tools hung. Most outlines correspond to tools named in the printed list. Two do not. One outline is narrow and long; the other is shaped like a hand saw with a missing tooth. The wall inventory has a pencil note beside them: “issued for bridge / returned minus sleeve.”

**Inspection copy:**

“Dust remains in the empty tool shapes, except at the lower hooks. Those hooks were used after the wall was washed. A square of clean wood outlines a folding saw. A thin black stripe is all that remains of its sleeve. Below it, in carpenter's pencil, someone has written ‘bridge rail, east bend.’ The line has been erased and written again, carefully enough to score the grain.”

**Player actions:**

- Take only tools whose presence and ownership are clear.
- Leave the wall undisturbed and copy the missing-tool notes.
- Remove a serviceable tool for the expedition, recording the issue in the field log.

The player is never asked to decide whether a lost saw constitutes theft. The next scene reveals that it was used on a bridge but not whether the bridge carried a medical delivery or a family leaving the ridge.

**If the player pockets the folding saw:** The inventory outcome must follow a real item row and current loot/quest consumer. If the item is not represented by a current valid item, the proposal instead leaves the saw as a story object and offers only the existing tool categories. No new item is presumed by this bible.

**Closing line:**

“The tool rack has room for every name on the printed form. It does not have a place for the people who borrowed from it afterward.”

## 7. Questline content packet, scenes D–G

### Scene D — The lower route: “What the pencil did not say”

The lower route begins behind a stand of spruce. The main trail is wide enough for a hand sled. The side track is not. It follows a drainage cut, crosses a narrow shelf, and rises through brush where a person has tied old survey ribbon with the color turned inward. The map's bearing is correct only if the party begins from the bent stump rather than the surviving post.

**Arrival text:**

“The cut is not a road. It is a line people chose to keep open because the other line was worse. The branches are trimmed on the inside of the turn. On the outside, they have grown back across the path. If you follow the fresh-looking opening, you will meet a deadfall and lose an hour climbing around it. If you follow the old knife marks, the route narrows but stays level.”

**Route decision:**

1. **Follow the older marks.** The party spends more time moving slowly, but reaches the windbreak with its carried equipment intact if current travel rules allow it.
2. **Take the open gap.** It looks easier and is not. The party meets a blocked descent or a weather exposure check. The failure is recoverable; the group can retreat or cut around.
3. **Mark the junction and return.** The expedition preserves its current resources. The journal notes an unresolved route; a later trip may use it.

No option is a hidden correct answer. The old marks may be hard to see in poor visibility; a good forecast can make the distinction legible, while a storm can make retreat sensible. The content should consume only known travel conditions and should not create a new map skill.

**At the windbreak:**

A crosspiece holds an old coil of rope under a sheet of roofing tin. The rope is not complete. It has been cut into two useful lengths and one too-short handline. A tag is folded around the longer piece. Its writing is neat but has no date.

> LONG LENGTH: returned, dry.  
> SHORT LENGTHS: left in the lower box.  
> ONE PERSON CROSSED HERE WITH A BAD ANKLE.  
> NO NAME TAKEN.  
> IF THEY COME BACK, ASK FOR THE ROPE.

The tag has a second line, written later with a different pencil: “No one came back for it. We did.”

**Player options:**

- Carry the rope back, if its item condition and inventory capacity support the action.
- Leave the rope and copy the tag. The later return can show whether another group used it.
- Cut the rope further to make a handline for the present expedition. This is a real resource trade if a compatible inventory item exists; otherwise present it as a fixed authored action with no hidden quantity.
- Leave the tag in place. The story remains available through the map and other records.

**Character line, if a practical survivor is present:**

“Rope gets returned when the person who needed it thinks they might need it again. That is not a loan book. It is a weather report about the next person.”

### Scene E — The first account: Runa Dey's kitchen note

The Holdfast receives a packet from a surviving route contact after the player has brought back the inventory count. The contact does not appear because the story needs a convenient witness. The note is found through the existing radio, journal, quest or caravan route that the promotion audit confirms. If no live delivery surface can carry it, the note can be discovered at the store on a later visit instead.

**Note, written on the back of a ration tally:**

> They want to know who took the boots. I can tell them which shelf was empty and what size was left. I cannot tell them who wore the pair. The list says two crews. It does not say two crews were there at the same time. I signed for both stores because the superintendent was on the river road and the radio had gone quiet. I signed for the rope because somebody had to sign. Later I stopped asking for a name if the person could tell me where they were going.
>
> One night we had a man in the entry bay with no dry socks. He was not on a crew. He kept the door open with his boot because he thought it would be rude to take the peg. In the morning the boot was gone. The peg was still there.
>
> I put two pairs of small mittens on the shelf. Nobody on the crew had hands that size. I didn't ask the question out loud because there was no useful answer.
>
> If the store is still there, leave the door open far enough for a person to see the bench. Do not leave it open far enough for snow to fill the room.
>
> — Runa

The note is not a confession to theft. It is one person's account of a practice that began with proper inventory and ended with undocumented help. The player can mark it as testimony, private correspondence, or a work note through an existing journal distinction if the consumer supports that label. If not, preserve its source in the text and make no new record type.

### Scene F — The rule: “A person is not a unit of stock”

The player returns to the store with enough evidence to post a policy. This scene should arrive only after the player has seen at least two independent traces: the inventory discrepancy and the lower-route note, the kitchen account, or another field record. A single clue should not unlock certainty.

**Quartermaster:** “You want a rule that will survive the next storm.”

**Player:** “I want one somebody can follow.”

**Quartermaster:** “Those are not always the same thing.”

The old printed issue form has four columns: item, quantity, crew, return date. The new notice can use the same paper and ask for a date, a destination and the condition of any borrowed tool. It cannot force someone to report their name. The player is offered three posted forms and a blank option:

**Form A — Sealed reserve**

> EMERGENCY STOCK. Do not remove without a planned work order. If the door is open, close it before leaving. Record damage. Return tools to the marked outline.

**Form B — Open issue, limited quantity**

> TAKE WHAT YOU CAN USE SAFELY. Write what remains. If you cannot write, leave a mark another person can read. Do not take the last warm layer from someone sleeping here.

**Form C — Work-share reserve**

> TOOLS MAY BE BORROWED FOR A JOB. Leave the destination and the return place. If the tool cannot return, leave the reason. Food and dry clothing are for immediate use. There is no payment box.

**Blank notice**

The player writes a short text of their own from a constrained option set: “leave a record,” “leave the door safe,” “take only needed stock,” or “no rule posted.” This is not a free-form text box. It is an authored choice with a concise title and one sentence.

The story must not attach “good,” “bad,” “merciful” or “selfish” labels to these options. Each one makes a different kind of future legible. The Holdfast may need the reserve later. A traveler may need the blanket tonight. A tool may have been borrowed for a job no one can verify. A rule cannot solve all three cases.

### Scene G — The next reader

On the first later visit after a policy is posted, the player sees a response. Which response appears depends on the posted rule and the actual path taken, not on a hidden morality score.

**If Form A was posted:**

The stock is still present. A narrow strip of paper has been slid under the door. It says: “We did not take anything. We waited until daylight. The boy's feet were blue before they warmed. We left before the door opened.” There is no signature. The player does not learn whether the note writer was honest; the physical evidence shows a pair of small footprints stopping outside the entry bay.

**If Form B was posted:**

One ration tin has been opened. Its lid is cleanly folded and placed beneath the ledger. The next line reads: “Two people. One night. Tin still sealed inside.” A later pencil addition says: “This count is wrong if you mean meals. It is right if you mean tins.”

**If Form C was posted:**

A length of rope has returned with a splice at the midpoint. The note reads: “We needed the long length. The bridge needed the short one. Both came home.” The player can take the returned rope or leave it for the next crew.

**If no notice was posted:**

The old list has been moved under the bench. A line of graphite crosses out “two crews.” Underneath, someone has written “whoever is on the hill.” The amount of stock is unchanged unless the player's prior expedition actually removed an item.

The player may read, remove the note, leave a reply or take no action. The last state is a valid response. The expansion should reward attention with context, not force the player into an ending button.

---

## 8. Side quests and short arcs

Each side story is small enough to miss and substantial enough to change how a later object reads. The quest text must not promise a new mechanical system. Where a branch calls for a reward, use current items, knowledge or a current reputation/quest effect only after a live-consumer audit.

### 8.1 “The Missing File”

**Hook:** The tool wall has a file-shaped gap, but the printed inventory lists no file. A forestry-skilled survivor says a file is more important than an axe when the saw is already dull. The player can look for it among the cache bins or leave the tool wall intact.

**Find:** A flat file is wrapped in a flour sack behind the boot shelf. One side has been used; the other is clean. The sack carries a meal count from a separate household, not the crew ledger.

**Interpretations:** It may have been stored there to sharpen a neighbor's saw; it may have been returned after the crew's file went missing; it may be the only file and therefore never belonged to the formal cache. The player cannot know which.

**Choices:** carry the file home; leave it with a note; use it to restore a current expedition tool if that is a valid action; or report the mismatch. The immediate item need should compete with the site remaining usable. If carried home, the next visit leaves the empty flour sack in the same place, so the absence is visible.

**Closing document:**

> FILE RETURNED? The old line is still there. I have put a question mark after it because nobody remembers whose file this was. The new edge is good. Keep it dry. A clean tool lasts longer than an argument.

### 8.2 “One Pair, Both Feet”

**Hook:** The boot shelf has one insulated boot, size unclear, with a patch over the left heel. Its mate is missing. A travel note says a small traveler crossed the ridge in snow, but the note is too damaged to tell whether the traveler wore one boot or carried the spare.

**Search:** The lower route includes a shallow drainage pipe where the wind has pressed needles into the opening. The other boot is inside, dried stiff and placed heel-first. The sole has one forestry nail and a worn patch made from a feed sack.

**Choice:** take both boots if found together? They are not together. The player can retrieve the lone boot, search for the mate or leave the pair's history unresolved. A practical outcome is to return the boot to the shelf beside a paper label that says “mate not present,” rather than force an item completion.

**Dialogue at the Holdfast:**

“Could be a spare.”

“Could be a person who lost one.”

“Could be both.”

“What do we write?”

“Write what we saw. One boot. No mate. Don't write the rest for them.”

The branch models factual restraint. It does not turn incomplete evidence into a moral lesson the character announces.

### 8.3 “The Stove Bolts”

A glass jar contains seven bolts, three washers and two small nuts. The lid is newer than the jar. The bolts match no visible stove in the store. The party can take them as salvage, test them against a shelter repair, or leave them as an unidentified repair set.

If a current repair action consumes them, the narrative must say exactly what was repaired and what remained unknown. If the host has no such item or recipe, keep the jar as a fixed site object. Do not invent a “forestry bolt” item only to complete a story beat.

**Label inside the lid:**

> Don't use these on the stove. They fit the sled runner bracket. Stove bolts are in the red tin. If the red tin is gone, leave the stove alone until someone brings the right pitch.

The red tin is absent. The notice shows that the cache's contents had local knowledge attached; a sack of bolts is not a generic material pile.

### 8.4 “The Dry Blanket”

One blanket has been washed, folded and placed above the rations. Its center is thin. Its edges have been resewn with mismatched thread. The player can issue it to the expedition or leave it as shelter stock. The choice is affected by the party's current warmth equipment and travel conditions, not by a scripted guilt prompt.

**Optional dialogue:**

“We could take it.”

“We could.”

“It's already folded.”

“Someone folded it after they washed it.”

“Would they rather we left it?”

“I don't know. That is why we count it.”

### 8.5 “The Quiet Pull-Off”

The logging-spur map marks a pull-off below the bluffs. The player may reach it on the first or later trip. A wide skid mark ends before the pull-off, and the ground beyond it contains parallel scratches from a hand sled. The story does not provide a vehicle chase or new tracking mechanic. It provides a short account: people used the pull-off for a job they could not transport by road.

At the pull-off, the player finds a pair of broken sled runners. One is tagged “bridge rail.” The other is tagged “shelter door.” These labels do not match a single delivery. The player can infer that wood and equipment were carried in both directions. The next scene in Expansion 63 may identify a date on the metal staple joining the runners to an electrical dispatch form.

### 8.6 “The Cut End”

A rope end was cut square and singed. It was not cut to escape a trap; there is no trap. It was cut to remove a damaged length and keep the rest serviceable. A repairer in the Holdfast recognizes the splice but cannot identify the person who tied it. The player may ask them to replace the splice, if the current crafting contract allows it, or preserve it.

**Repairer:** “This knot was meant to be undone with cold fingers.”

**Player:** “Can you do it?”

**Repairer:** “I can do it with warm hands. I don't know if I can do it in gloves.”

If a crafted replacement is allowed, the old splice remains an inspection clue. If not, the dialogue itself carries the skill distinction; no recipe is added.

### 8.7 “The Date That Came Twice”

Two separate forms use the same date, one for a cache inspection and one for a storm closure. The second form has a different paper stock and may have been copied later. The player can compare pencil pressure, not handwriting expertise. The point is not a forensic minigame; the date was likely entered after the fact by someone with a reason to reconstruct events.

**Memo, reverse side:**

> I put the date on the issue card because I knew the date I returned. I cannot certify when the rope left. If you file the card under the day I saw the empty peg, you will be saying more than I can support.

This can connect to Expansion 63's switching book without forcing the player to prove a timeline.

### 8.8 “A Mark for the Next Crew”

A later visitor has left a small, ordinary direction mark at the fork. The player can add a second mark indicating the cache entrance, the safer route or a seasonal closure. The mark should use the existing map/journal affordance if available. It is never a free paint system. If no persistent map annotation exists, resolve the choice through a report text and a later-visit observation.

The authored choices are: “entrance visible from lower spur”; “lower spur unsafe in thaw”; “route closed until bridge repair”; or “no mark.” Each label gives information without promising that weather is predictable.

### 8.9 “The Ration Count”

The cache ledger counts food tins, not meals. A later visitor wrote a calculation in the margin: “Two people, four nights, six tins; one tin saved for the walk down.” The player can compare this to the current expedition's own ration rules. If the values are incompatible, the narrative should present the ledger as historical rather than claim it is an optimal current plan.

**Ledger margin:**

> Don't count the tin they carry out. Count what can be opened at the place. A sealed tin in a pack is a promise to the next hill, not dinner tonight.

### 8.10 “No Signature”

The last page of the cache book offers a blank line for the recipient's name. It is clean except for pressure marks from writing on the page above it. A name can almost be read but should not be reconstructed by the player. The person who issued the store may have protected a visitor by not recording the name; the page may simply have been left blank.

The player can write the Holdfast's name, their own name, a route name or no name. If the player adds a name, the subsequent callback reflects that choice without declaring it an official claim. A blank line remains a possible and complete answer.

---

## 9. Survivor and contact dossiers

All names below are proposed. A catalog-ID and proper-name collision audit is required before implementation. They are written as people in the story, not automatic recruitable survivors. Only a later content audit may decide whether any becomes a roster recruit, a contact, a recorded voice or a historical name.

### 9.1 Runa Dey — cache steward

- **Surface identity:** The name on the late cache notice and several pencil totals.
- **Practical function:** Former storekeeper, trained in issuing, checking and repairing field equipment.
- **Contradiction:** She kept more accurate item counts after the formal crew roster ended, but stopped writing down names.
- **Material history:** Knife cut on the lower door frame; repaired cup groove; a coat hook with a nail bent backward so wet sleeves would not tear.
- **Pressure response:** Under time pressure, she gives one instruction at a time. When challenged, she repeats the date rather than defend the decision.
- **Relationship hooks:** Her old work overlaps with the utility crews and the winter relief route. No faction owns her.
- **Hidden dimension:** She signed for stock that she then issued outside the official team, but she does not know who later used every item.
- **Mechanical hooks:** Cache state, route knowledge, inventory issue text, optional record/relationship callbacks. No new “trust Runa” stat.
- **Arc potential:** The player can discover a second note in which Runa corrects her own earlier count. Her arc is a movement from “I will not name the recipient” to “I will name what I can prove.” The story should not convert her into a confessor who explains the entire past.

**Voice sample:**

“I wrote the count while the shelf was lit. I wrote the name after the person had gone. Those are different jobs. I was better at the first one.”

**Dialogue sample, if confronted with a theft accusation:**

“Then call it theft. Use the word if it helps you file the report. I will not say I saw a hand take the boots. I saw the space where the boots had been. I saw a person come in wet and leave dry. There are days when those facts are enough to keep someone alive and not enough to satisfy a form.”

### 9.2 Tovin Marr — route runner

- **Surface identity:** A name mentioned in one delivery shorthand, possibly the person who carried the lower-spur note.
- **Practical function:** Runner and map copier; knew which forestry ribbons belonged to survey crews and which were improvised by travelers.
- **Contradiction:** He kept route notes but left one crossing unmarked because people were using it after dark.
- **Material history:** A pencil shortened on both ends; graphite under a fingernail; a map folded so the dangerous section sat inside the crease.
- **Pressure response:** He jokes when a route is easier to see than to walk. He stops joking when asked who is missing.
- **Hidden dimension:** He was not sure whether he had led people to safety or only led them to a warmer place to wait.
- **Arc potential:** A later voice clip can reveal that he returned to the route after the closure to remove a misleading marker, not to erase evidence.

**Voice sample:**

“Here is the road. Here is where the road stops pretending. After that, the map is a suggestion with handwriting on it.”

### 9.3 Seli Orr — winter traveler

- **Surface identity:** An unnamed user of the cache, represented by boot patches, meal notes and a blanket repair.
- **Practical function:** No defined profession is needed; she traveled with a child and carried stove fittings between two shelters.
- **Contradiction:** She left the rope and blanket more carefully than she left her own name.
- **Material history:** Feed-sack heel patch, small mittens, a cup with a split rim crimped closed using wire.
- **Pressure response:** In the only surviving conversation, she asks how long a route takes before asking whether it is safe.
- **Hidden dimension:** It is unclear whether the child was her own, a niece, or a person she had taken responsibility for. The expansion does not resolve this because no source can.
- **Arc potential:** A future callback can identify the mittens as belonging to a child now living at a trade stop only if a current, separate source supports that relation. The default ending leaves identity unknown.

**Voice sample:**

“Keep the smaller pair. The other one can be made bigger with a thumb seam. A foot cannot be made warmer by telling it to wait.”

### 9.4 Halden Pike — forestry foreman

- **Surface identity:** The initials on the “two crews, one season” cache form.
- **Practical function:** Responsible for procurement and seasonal maintenance before the exchange.
- **Contradiction:** His forms are clear and legible, but the cache he specified did not include enough dry space for a traveler.
- **Material history:** A ruler line corrected three times; a grease pencil mark at the bottom of the crew roster; a handwritten request for smaller boots that was never approved.
- **Pressure response:** He writes in columns. His radio speech is full of incomplete numbers because he expects the listener to know the map.
- **Hidden dimension:** He did not oppose opening the cache to others. He believed a posted issue rule would protect both the people and the stock; the rule arrived after the first undocumented issue.
- **Arc potential:** His memo may be read first as bureaucratic obstruction, then later as an attempt to make emergency help repeatable.

### 9.5 Eda Venn — line worker at the ridge route

Eda appears briefly in the cache story and receives her full arc in Expansion 63. The plan must recheck her name and any matching role in existing content before promotion.

- **Surface identity:** A utility worker whose initial appears on a return-route note.
- **Practical function:** Field electrician; familiar with weatherproof cable and manual switching schedules.
- **Contradiction:** She asked the forestry crew to leave a light, then later wrote that the light should not be treated as a promise of rescue.
- **Material history:** One splice length wrapped in oilcloth; a pencil tally for “boots borrowed / cable returned.”
- **Pressure response:** She gives ranges, not guarantees.
- **Hidden dimension:** Her choice to keep the exchange powered had a downstream cost that she did not learn until after her radio line failed.
- **Arc potential:** Expansion 63 can follow her work without making her the sole cause of a regional failure.

---
## 10. Encounter library: ordinary pressure, different solutions

These encounters are authored variants for the route and cache. They are not a new random-event system. Each should be attached to a current encounter or expedition content surface only after a consumer check. Every encounter offers observation, withdrawal or negotiation where the physical situation permits it. Combat is not the default answer.

### Encounter 1 — The branch across the mark

A spruce has fallen across the old trail. It is not a deliberate barricade. The trunk has split at the root; the broken top rests on a second tree and hides the low route mark. A rushed party can climb over, but the harness may catch on the dead branches. A careful party clears only enough brush to expose the marker. The player may turn around if weather is worsening. The success line is not “the route is safe”; it is “the route is visible for one more bend.”

### Encounter 2 — Wind at the lookout stair

The lookout structure is standing, but its outer landing flexes. The player hears a repeated wood knock and can mistake it for a signal. A close inspection shows a loose handrail striking the post. The party can watch from the lower window, brace the rail if current repair actions support it, or leave. If the player leaves, the scene records the direction of the wind and nothing more. It never plays a ghost voice.

### Encounter 3 — The frozen latch

A latch has frozen shut around a strip of canvas. Forcing it may tear the canvas and bend the pin. Warmth, dry cloth and patient pressure are available approaches; the party can also walk away. The scene is a small test of expedition supplies. No timer suddenly seals the location. If the player uses a tool, the description names what was used and whether it was returned.

### Encounter 4 — The unmarked sled runner

A short runner protrudes from snow at the logging pull-off. It may belong to a sled, a bridge repair, or a dragged equipment box. The wood bears two separate nail patterns and no complete name. The player can excavate it, mark the site, or leave it. A practical survivor says, “If we take the wood, the marks go with it.” That statement creates a real information cost only if the journal already has a place for the observation; otherwise it remains a described tradeoff without a fake system penalty.

### Encounter 5 — Ration tin in the snow

One sealed tin is wedged beneath a root. Its label is intact; the manufacture date is legible; the contents and safety status follow the real item catalog. The tin is not magically preserved. A cracked seam or unknown provenance should be shown and handled by current item rules. The player can take it, leave it, or open it now if the valid item consumer supports use. If opened, the tin makes a small, ordinary meal and leaves a clean lid that can be used as a note surface.

### Encounter 6 — A traveler asks for the map

A person stands beyond the visible end of the spur. They do not approach the party. They ask to look at the route map, then say they need the low crossing before night. Their pack has no visible weapon and one dry sleeve. The player may share the map, describe the route, ask where they are going, offer an escort only if the current party and quest path can support it, or decline. The traveler can leave without threat. If the player shares a route, the later note on the cache board may report only that someone passed; it does not award hidden trust.

### Encounter 7 — Marks under fresh snow

The trail carries two sets of marks. One is a boot tread with a missing heel edge. The other is a hand-sled drag. The snow is too new to tell whether they traveled together. A tracker can follow them only if current skill checks support that; otherwise the player reads the direction and the uncertainty. The marks reach a fork and disappear under wind-blown powder. The encounter ends with a decision to continue or return, not a sudden discovery of a body.

### Encounter 8 — The returned rope

A length of rope hangs in a tree, tagged with a date but no name. It is dry, cut square and spliced. The player can inspect the knot, take the rope, leave it for the next traveler or add a written issue note. The knot's material description must be consistent with actual rope items. If no rope condition is represented, the text can report “usable at a glance” without inventing durability percentages.

### Encounter 9 — The stove that will not draw

At a trail shelter, the stove chimney is blocked by a bird nest made of dry grass and cord. There are no birds present and no hatching eggs. The player may clear the flue, leave the shelter unused, or use another heat source that already exists. A small note below the stove says “clean out before first burn.” If the player does not clean it, the text does not force a fire hazard unless the current fire system is actually invoked.

### Encounter 10 — The open cache door

The door is open one hand's width. The weather is dry. A nail is wedged in the frame so the latch will not catch. The player may close it, leave it open for a visible entrance, or check whether the hinge is bound. On a later visit, the door state reflects only a supported quest flag or an authored callback. If the runtime cannot persist that state, the encounter is one-time text and no promise is made that the door remains open.

### Encounter 11 — Soot on the vent

The vent's soot streak is recent relative to the lichens, but the exact date cannot be known. A character can point out that soot travels with wind and that the mark does not establish who lit the stove. The player may record “vent used after closure” or “soot present, date unknown.” The difference matters for a later accusation branch. Neither entry is privileged as the player's intelligence score.

### Encounter 12 — Cold hands at the fork

One party member's hands are slow after a long stop. The prompt should use the current survivor condition and gear state if those are available. The player can shorten the route, share a warm layer, or keep moving. Do not add a cold-injury system to justify the scene. The story can show ordinary fatigue through dialogue and the cost in current needs/travel data.

---

## 11. Location and return-visit writing

### 11.1 Forestry Emergency Store — approach description

“From the last marked trail post, the cache is not visible. The slope has been cut into a line too straight to be an animal path. The roof sits under branches laid with their cut ends inward. Rainwater has found one corner and made a dark track through the needles. At the doorway, an old fire-service mark is almost hidden beneath a newer notch. The new mark points down.”

### 11.2 Entry bench — inspected twice

**First visit:**

“The bench is dry on the left and stained dark on the right. A tin cup fits into a groove at the far end. The wall says to keep wet gear off bedding. A second hand has added: ‘If you have one blanket, put it underneath.’ The first instruction is painted. The correction is scratched into the wood.”

**After finding the lower route:**

“The bench has not moved. The shallow cup groove now looks less like a place for a worker's mug and more like a way to keep the cup from falling when someone slept sitting up. The paint still says ‘crew issue.’ The pencil line underneath still says nothing about a crew.”

### 11.3 Fire-lookout stair — view from inside

“The glass is cracked at the upper left, but the opening has been stopped with a square of clear bottle glass and putty. From the stair, the timber line bends around a low shoulder. A survey mark should be visible there in late light. In the morning it is hidden by the ridge. Someone wrote ‘after four’ on the sill and crossed it out, then wrote ‘when the wind drops.’”

### 11.4 Lower windbreak — after a posted rule

“If a policy was left, the paper is folded behind the tin where rain cannot reach it. If none was left, the blank inventory page has been turned to a clean side. Either way, the crosspiece remains. Its middle has a pale stripe where rope rested. The forest has not kept the count for you.”

### 11.5 Logging pull-off

“The slope opens into a small clearing no wider than a wagon. Old wheel ruts remain under moss, but a narrow drag mark crosses them. The pull-off was built for work vehicles. The drag line was made by something that could not drive. A stub of pencil is pushed into a split in the post. Its point has been used down to the wood.”

---

## 12. Environmental story props and inspectable text

The following props are content pieces, each with a proposed reveal order. No prop should be necessary to understand the central choice. They reward careful return visits and create a richer physical record.

### Prop A — The door-frame tally

Three marks at different heights. A maintenance date has been written beside the upper mark, not beside the lower one. The lowest mark is a shallow knife cut. If inspected before the inventory, it reads as unexplained damage. After reading the cache form, it looks like a later measurement of who could reach the latch. The game never confirms who made it.

### Prop B — The dry-gear instruction

The painted instruction is formal and signed “H.P.” A graphite correction says “not over the bedding.” On the back of the board is a newer note: “If the floor is wet, use the sled tarp. Don't use the clean blanket.” It is a hierarchy of practical choices, not a moral code.

### Prop C — The black tape line

A stripe of black tape marks the height where snow once reached the interior door. The tape is rough at the edge and has been drawn over twice. A person wrote the date of a storm beside it, then replaced the date with a tide-like wave mark because the calendar had stopped. The player cannot map it to a global day without another source.

### Prop D — The misfit wrench

The wrench has a flattened jaw. It fits neither the visible pump nor the door hinge. It has no catalog ID until a current item audit proves otherwise. The accompanying label reads: “Returned from east rail. Jaw opened under load. Do not use for the brace pins.” This line can point forward to the Electrical Maintenance Exchange without claiming the same wrench is present there.

### Prop E — The unopened meal

A ration packet is sealed in a waxed envelope with its outside marked “for the walk down.” The back reads “not opened; person had food.” The packet's actual safety and inventory identity must be checked before it can be taken as an item. The story point is that a reserve can be left unused because someone carried enough already.

### Prop F — The changed label

The original shelf label says “crew blankets.” The word “crew” has been scraped away and replaced with “dry blankets.” The remaining fibers hold a faint line of ink. If the player reads the surface at an angle, the text offers no hidden message; it only shows that the category changed.

### Prop G — The cut-sleeve saw

An empty leather tool sleeve hangs behind the wall. The seam is intact but the buckle is gone. The sleeve may have stayed because the tool was returned without it, or because the sleeve belonged to a different saw. The player can take it only if the item catalog contains a valid item; otherwise it stays as a fixed clue.

### Prop H — The replacement nail

A bright nail holds an old instruction board. Its head has a notch cut across it. The same notch appears on a brace in the utility exchange (Expansion 63). The shared notch indicates a worker's repair habit, not a secret society. It lets the player connect two sites by craft and handwriting rather than by an exposition dialogue.

### Prop I — The missing date

An inventory column has a date written then scraped away. The paper is thin enough to show pressure from the removed numerals, but not enough to read them. A nearby storm log supplies a window of possible dates. The player may record a range but cannot recover certainty.

### Prop J — The small mitten

The mitten is patched at the thumb and has a cord through its cuff. Its pair is absent. The outer label reads “repair before issue.” No name or age is given. It should not trigger a sentimental music cue. The soundscape remains wind, fabric and the latch settling into its place.

## 13. Diegetic document packet: the North Woods file

These texts are proposed as separate discoverable pieces, not one lore dump. The table below gives a possible order. A player who finds only two should still understand the practical stakes. A player who finds all of them should be able to reconstruct that the cache was used by more people than the roster named, while remaining uncertain about the identity of at least one traveler.

| Piece | Likely location | Voice | What it confirms | What it cannot confirm |
|---|---|---|---|---|
| Cache issue card | Tool wall | formal, short | intended crew count and item classes | who later used the stock |
| Firebreak legend | map fragment | field shorthand | route marks and one changed spur | why it changed |
| Stove check | entry bench | practical | warmth instructions | who slept there |
| Rope return tag | lower windbreak | anonymous | a long coil came back | who carried it out |
| Runa's kitchen note | discovered later | guarded, first person | she issued items outside the roster | the recipients' names |
| Radio check | exchange packet | clipped technical speech | a request to keep a path open | whether it was received |
| Meal count | ration shelf | arithmetic | the store supported at least one stop | how many people ate |
| Blank signature line | ledger | no voice | the record system had room for names | whether anyone was asked |

### 13.1 Cache issue card, first page

> NORTH WOODS FIELD CACHE — SEASONAL ISSUE  
> Site: Firebreak Seven / lower spur  
> Use: two forestry crews, one season  
> Storekeeper: H. Pike  
> Check interval: before first frost; after severe weather; on return from lower bridge  
>
> Items to account for: line rope, hand axes, saw files, harness straps, stove fuel, dry blankets, winter ration tins.  
>
> Leave serviceable tools on their marked hooks. Keep wet rope open to dry. Do not wrap frozen line under canvas. Replace spoiled food only with an item carrying a legible date.  
>
> If a crew borrows an item for a non-forestry job, enter destination under “work.” If the recipient is not assigned to a crew, enter the destination and the return date. Do not leave the recipient column blank unless the person has refused their name or the line is unsafe to keep.

The last sentence is written in narrower type. The ledger's signature page is blank, which leaves room for several interpretations.

### 13.2 Firebreak legend, grease pencil copy

> F7 / north line: do not follow the wide cut in low cloud.  
> Old survey ribbon: white, turned in; visible from the ground.  
> Crew route: red square, visible from the lookout.  
> Cache route: one knife notch, below shoulder height.  
> Do not paint the cache mark. Snow hides paint and people follow it after closure.  
> Lower crossing: passable when the stones show; do not send a loaded sled after rain.  
> If the bridge is gone, go uphill before you go east.

The reverse side has a sketch of a bent spruce and a correction: “tree fell since this was copied.” This is evidence that map knowledge aged, not that the map was a deliberate trap.

### 13.3 Stove check, inside cover

> CLEAN THE FLUE BEFORE THE FIRST FIRE.  
> Check the roof gap from the outside. If there is no daylight at the upper bend, do not light.  
> Dry wet gloves at the side rail, not on the stove door.  
> Keep the sleeping blanket off the bench if a coat is still dripping.  
> If one blanket is all there is, put it under the person. The person will dry it.

A graphite line under the final instruction reads: “Whoever wrote that was right.” It is a later response, not a signature.

### 13.4 Crew count, folded twice

> First frost count: 2 rope coils, 6 ration tins, 4 blankets, 2 harnesses, 3 hand axes, 1 file.  
> Second count: one rope coil moved to lower bridge; five tins; one blanket airing; harness straps replaced.  
> Close count: form copy lost; no return from east crew; second crew reported by radio, not in person.  
> Addendum: put small boots under dry shelf. Not crew issue. Keep the shelf clear.

No year is written. No one marks whether the second crew reported from the same season or a later one.

### 13.5 Runa's letter to the office

> You asked whether the store can be shut until the road crew returns. I can put the bar across the door. I cannot make the ridge stop being the ridge.
>
> There are two people using the lower crossing. I have not asked which house they sleep in. They have brought the rope back twice. The first time it was wet. The second time it was clean. Both times they left the short lengths on the shelf.
>
> If you want me to stop issuing rations, send a replacement storekeeper who can be here before dark. I am not saying the rule is wrong. I am saying the rule will be read by someone standing in the snow with an empty tin.
>
> I have added the stock I can confirm to the form. I have not added the stock I suspect. There is a difference between keeping a reserve and pretending it has not been used.
>
> Runa Dey

The letter is not delivered or mailed. It is folded into the original copy and may have been written to make herself answerable to an office that no longer responds.

### 13.6 Radio check, crossed out on the copy

> **Eda:** North exchange to cache. Do you read?  
> **Runa:** Read enough. Wind is turning the mast.  
> **Eda:** We have a path request for the lower crossing. One delivery before dark.  
> **Runa:** How many?  
> **Eda:** The form says one. The load says two.  
> **Runa:** That is not a count.  
> **Eda:** No. It is what they brought me.  
> **Runa:** I can open the bench room. I cannot promise the road.  
> **Eda:** Don't promise the road. Tell them where the warm room is.  
> **Runa:** Copy.  
> **Carrier:** [lost]

A later pencil note: “No answer recorded at exchange. Check day and station log.” The radio text is incomplete by design. It should not establish who received the message.

### 13.7 Traveler's note, no greeting

> Took the left branch after the bridge. The marked path was under water at the toe. Slept in the lower room. Used one blanket and left two tins. The rope was stiff in the morning but sound after I opened the coil. I cut the bad foot off and hung it under the roof.
>
> The small boots are not mine. I put them back where the door hand can reach them.
>
> If somebody comes later, the crossing is easier in the morning. The light stays behind the ridge after noon.

The writer's grammar uses “foot” for the damaged end of rope. A reader may initially take the sentence literally; an optional inspection of the rope clarifies it. Do not use the line to suggest gore or violence.

### 13.8 Fire watch log, partial week

> Day not known. Sleet until noon. Lower vent clear. Smoke went east and low.  
> Day after: no smoke; wind held the door against the jamb.  
> Next entry: one kettle, no fire. Cup warm.  
> Next: road noise after first dark. No lights.  
> Next: moved the ribbon to the lower fork. Old mark not visible under new snow.  
> Next: someone took the red scarf from the line. It was mine. It was wet. I hope it helped.

The log never identifies the author. It could be one of the listed crew or a later user. The player has no requirement to decide.

### 13.9 Office correction, unsigned

> The reserve was not authorized for household use. The storekeeper was instructed to report all non-crew issues. The lower route was not included in the approved maintenance plan. No assistance request was received before closure.
>
> Correction entered after review: the lower route was included on map copy 4. This office did not receive map copy 4. No assistance request can be confirmed either way.
>
> Do not destroy the first line. It records what we believed at the time.

This document adds no villain. It shows a bureaucracy correcting an overconfident claim while preserving its original record.

### 13.10 Closing note, whichever ending

> I moved the form to the dry shelf. Whoever comes after can read it without standing in the doorway. I did not change the count. I changed the line that says “crew.” It now says “issue.”
>
> That is not the same as opening the store. It is not the same as closing it. It is a word I can defend if someone asks.

This can be a possible final text if the player chooses the split reserve. It should not appear on other branches.

---

## 14. Radio and oral account fragments

Radio content should be short, physically plausible and grounded in the existing radio system. No new distress mission is required. If a radio event is not available to carry these lines, use the same text as a paper transcript or static journal fragment. The scripts should not create a new trust ledger or claim that the player has opened a long-distance network.

### 14.1 Maintenance test, 22 seconds

> **Eda:** North station, send tone.  
> **Runa:** Tone sent. Wind on the upper guy.  
> **Eda:** I have the pulse. Leave the line where it is.  
> **Runa:** It is not a line. It is a rope around a post.  
> **Eda:** I know. I am checking the carrier.  
> **Runa:** Then say that next time.  
> **Eda:** I will.  
> **Carrier:** [sustained hiss]

The exchange is a test, not an emergency. The joke is not a character's entire personality; it establishes a relationship through a practical correction.

### 14.2 Weather closure bulletin

> North trail is closed above the lower crossing. Closure is for visibility and load, not for snow depth. The old mark is still on the tree; do not use it as a current opening notice. The cache door will remain latched unless someone is inside. No delivery is scheduled. If your route needs the ridge, take the west spur and report what you find. The report matters even if the road does not open.

### 14.3 Oral account, version one

> The store was for the crews. That is what it said. Runa let us sit inside because the wind was worse than the rule. We left the food. We left the rope. We left the place the way we found it, except for the blanket. I remember the blanket because it smelled like wet bark. I do not remember which of us took it.

### 14.4 Oral account, version two

> We took a blanket. Nobody told us to. Nobody stopped us. The paper said crew issue and we were not crew. I carried it back later. It was dry when I left it. I cannot tell you whether the same blanket came back. There were four of them then. There was one when I returned. You can see why the count is not an answer.

The two versions may come from different witnesses. They should not be tagged “truth” and “lie.” Their disagreement gives the player a reason to avoid turning a partial count into an accusation.

### 14.5 Listening text after the choice

- **If the player issued the store:** “The carrier opens for half a second. A voice gives a route, not a name: ‘Lower crossing, morning. Rope returned. No one is waiting at the bench.’”
- **If the player sealed the store:** “The receiver catches the station tone once, then settles. No voice follows. The player cannot know whether the tone was deliberate or interference.”
- **If the player left no rule:** “A paper rustle comes through the receiver because the microphone is open somewhere. Then the carrier drops. Nothing can be identified.”

No branch may pretend that radio static contains words the system cannot decode.

---

## 15. Encounter branches as authored scenes

### 15.1 The traveler at the spruce

**Setup:** A lone traveler stands at the far side of a shallow gully. They ask whether the lower crossing is open. They carry a bundle under a tarp and keep one hand on its knot.

**Traveler:** “I don't need an escort. I need the part of the map that says if the water comes up after noon.”

**Player choices:**

1. “The map is old. I can tell you what we saw this morning.” The traveler accepts the uncertainty and asks for the route note.
2. “No crossing today.” The traveler asks whether that means closed or unknown. The player can clarify.
3. “What's in the bundle?” The traveler says, “Things that break if I set them down.” The answer is not hostile; the party can decline further inquiry.
4. “We can carry it together.” The traveler refuses or accepts depending on carrying capacity and party, but the refusal is not a betrayal flag.

If helped, the group may later find a new route note or a returned item. If not, the traveler may still arrive in a different later branch. Do not make their survival depend on one unseen kindness check unless the existing encounter model already represents that outcome clearly.

### 15.2 The storm room

The party reaches the cache as sleet begins. The room is dry except for the entry bench. The door can be shut only if the hinge is cleared. A survivor asks to leave it open so the next person can see the interior light; another says the storm will bury the threshold. The player chooses whether to spend time on the hinge, leave a visible marker outside, or withdraw.

**First survivor:** “If they come up the spur, they need to know there is a room.”

**Second survivor:** “If we leave the door open, the room stops being a room.”

**First:** “Then leave the lamp where it can be seen.”

**Second:** “There is no lamp.”

The conversation leads to an authored solution: tie a strip of cloth at eye level, leave the door latched, and mark the approach. This is not a new light feature. The player may also choose no action. The most useful line comes after the choice: “We did what the weather lets us do.”

### 15.3 The false theft report

The player returns with a missing-item count. A board member says the store was robbed and proposes denying further access. The player may present the rope tag, the account, the observed cup or only the bare count. The board can dismiss each piece for a different reason, not because it is evil: the tag has no signature; the cup's repair date is unknown; the account is oral; the count lists expected rather than proven inventory.

**Board member:** “I can sign for what leaves this shelter. I cannot sign for what left that cache.”

**Player:** “Then write that.”

**Board member:** “I can write it. I cannot promise it will be the only line anyone reads.”

The scene may change an existing reputation or quest consequence only if a current owner supports that action. Otherwise its result is a journal record and an altered set of dialogue lines.

### 15.4 The return in thaw

When weather shifts and the player visits again, the lower trail has a shallow washout. Water has exposed the foot of one survey post. The writing beneath the old paint reads “not a bridge.” The party can take the longer safe route, use existing route/vehicle rules if they apply, or leave. This is a weather callback, not a new erosion mechanic. The text states only what is visible on that visit.

### 15.5 The shared stove

Two travelers are already in the entry room. One has placed a cup near the stove; the other is holding a wet glove over it. They ask for the room, not supplies. The player may wait outside, ask them to leave, share the bench, or offer the route back to the Holdfast. Their immediate response follows the player's words and current faction/relationship context if supported. No combat is forced. If asked to leave, they pack and go; a later note does not secretly punish the player for that choice.

**Traveler one:** “We were told there was a bench.”

**Traveler two:** “We were told nothing. We found it.”

**Player:** “The room is small.”

**Traveler one:** “So is the storm.”

The line should be delivered plainly, without a music swell or camera focus unless the scene system already uses those affordances.

---
## 16. Additional authored records: fourteen small pieces

These are proposed in-world texts that can be found separately. Each is deliberately shorter than a quest scene. The player should be able to read one, put it away, and return to play. The order can vary. The phrases do not repeat the same emotional beat: they cover stock, weather, procedure, repair and uncertainty.

### 16.1 Tool count, dry shelf

> File: one, edge fair.  
> Rope: two coils at start. One coil split. One long length returned after lower bridge.  
> Harness: two straps dried near stove. Do not use if stitching is white.  
> Blanket: four on first count. Three on this count. One on the bench.  
> Boots: pair, size small. Keep below the dry cloth.  
> This is the count I saw. The count I signed is on the other page.

### 16.2 Ridge weather note

> Wind went over the lookout, not through it. The mast is still standing. Do not climb to test the wire. The bird line is gone from the west post. If the ridge is white before noon, stay below the second marker. If it is white after noon, the old path may already be under drift.

### 16.3 Bent nail envelope

> These nails came from the bridge rail. They are bent because the board was wet when we pulled them. Keep the usable ones. The rest can hold a note under the shelf. Do not put them in the stove tin; they are not stove nails.

### 16.4 Quiet inventory correction

> The ledger says five tins. There are four. I remember five at the last count and four when I returned. I did not see anyone eat the fifth. I am not correcting the number until the next person checks. If there is no next person, the number stays wrong.

### 16.5 Route sketch caption

> Low crossing: stones under water, not missing. Look upstream before you step. The water has changed the sound of the channel. A loud channel does not tell you how deep it is. A quiet channel may be covered with ice.

### 16.6 Lantern wick wrapper

> Kept dry in the tool drawer. Not issued. There is no lamp in this room. Ask at the exchange if they still have glass. Do not leave a flame unattended in the cache. No one is here to watch it when the wind changes.

This is a simple Wave 10 callback to the Wick without claiming a specific Wave 10 item is installed or available in the current build.

### 16.7 Crew roster, torn lower edge

> H. Pike — foreman  
> R. Dey — store  
> T. Marr — route  
> A. Venn — wire / lower spur  
> second crew: copy at exchange  
> The lower edge is torn where the second crew's names would have been. It is not clear whether the paper was torn off intentionally or caught on a staple.

### 16.8 Message for the next storekeeper

> If the person who replaces me asks whether I opened the cache, tell them I used the door. If they ask whether I issued the stock, tell them to read the ledger. If the ledger is gone, tell them there was a bench and the bench was dry. That is not a complete answer, but it is the one I can make without guessing.

### 16.9 Marking pencil

> The pencil is worn flat along one side. Its point breaks if pushed hard. Sharpen it with the file, not the knife. The map paper is thin. If you need to change a route, draw the old line through once and write why beside it. A crossed line is not a cleared line.

### 16.10 Receipt with no payer

> One dry blanket, clean  
> Two ration tins, unopened  
> Rope returned, end cut  
> Payment: none  
> Balance: none  
> The payer line is blank. The receipt was pinned beneath a note that says “paid by the next person.” No one has written what the next person owes.

### 16.11 Door inspection after wind

> West hinge holds. Lower pin is loose. Door closes if lifted by the handle. Do not kick it closed. The shelf behind the door contains a square of dry cloth for the hinge. If used, hang it back before you leave.

### 16.12 Story told at the stove

> There was a woodpecker that hit the same sign until the sign fell down. The ranger put it back with a nail. The bird hit the nail. The ranger put the sign lower. The bird hit the sign again. The children said the bird was trying to read. The ranger said no, it was trying to get through the bark. The children said the bird did not know there was a sign. The ranger said neither did the storm.

This is an optional children's story told by a survivor with no claim to folklore authority. It should not be added to the game's existing folklore catalog without a continuity check.

### 16.13 Last logged service visit

> Left the cache before light. The road was not passable for the vehicle. The crew carried one tool each and two rations between them. No fire. The lower door opened from the inside. We left the latch up so the next person could tell it had been checked. No answer on the exchange frequency.

### 16.14 Margin note on the roster

> “Second crew” might mean the crew assigned to the second shift, not a second set of names. I cannot confirm. The office copy uses “team.” The cache copy uses “crew.” No one corrected the word when it mattered.

---

## 17. Persistent consequences and alternate resolutions

The design separates what the player knows from what the player does. A story can resolve while uncertainty remains. Consequences are expressed through existing game state only where a current consumer exists; otherwise they are visible in authored text and a journal summary.

### 17.1 Reserve outcome

**Immediate:** The player records the cache count and leaves the usable items in place. The Holdfast keeps its current expedition supplies and does not receive extra stock from this story. The cache remains an emergency location.

**Later text:** A traveler can read the notice and decide to wait or continue. The player sees their own written policy quoted accurately. If a later visitor leaves an item, the cache shows the return; if no return can be supported by the quest state, the plan must not imply it occurred.

**Cost:** The player may miss a resource needed now. The story should not conceal that the team could have taken it. This is not a “good ending” awarded for restraint.

### 17.2 Release outcome

**Immediate:** The player takes a quantity of existing, valid items under actual inventory rules, or explicitly authorizes open issue if a live quest mechanism supports it. The plan does not claim that every missing shelf item has been restored to inventory.

**Later text:** The cache shelf is visibly emptier on return only if a quest state can represent that. Otherwise the callback speaks from the journal: “The Holdfast drew one blanket and two tins from the North Woods store.” A paper copy of the rule remains at the site.

**Cost:** Future expeditions lose the assurance of that stock. The player can know the quantity drawn, not the number of unknown visitors affected.

### 17.3 Split outcome

**Immediate:** The player carries away enough supply to meet a current need and leaves an issue rule for the rest. The quantity must be grounded in current item and inventory definitions. The player is not offered an impossible fractional item.

**Later text:** A new reader adds a note that challenges one column of the count. The player can leave it, remove it or correct the wording. The posted limit does not stop someone from taking supplies; the story never describes a paper rule as a lock.

### 17.4 Record-only outcome

**Immediate:** The player spends travel and receives no material reward. A detailed report distinguishes confirmed, inferred and unknown facts. A current journal/chronicle owner may store it if that content route is proven.

**Later text:** If the Holdfast holds a public meeting, the report can be quoted without converting uncertainty into an accusation. The player may have more credible evidence for the electrical exchange chapter.

### 17.5 Withdraw outcome

**Immediate:** The party leaves because of weather, condition, roster or player choice. The expedition is not scripted to fail. Partial route knowledge is retained only through an existing map or quest owner.

**Later text:** The map board shows that the route remains unverified. If a later dispatch revisits it, the first scene recognizes the earlier retreat: “The old mark is still there. You left it because the wind had covered the next one.”

### 17.6 Failure outcomes

- If an expedition member is injured or lost under current rules, the story uses that actual result and does not add a second scripted tragedy to underline it.
- If the player takes a wrong route, the consequence is a measured delay, resource burn or retreat that matches current travel rules.
- If the player removes an item needed by an unrecognized traveler, the game does not later reveal a hidden death solely to shame the choice. It may reveal that the traveler went another way, or leave the outcome unknown.
- If the player records theft, another source can challenge that conclusion, but only with evidence; the game does not secretly mark the player as dishonest.
- If the player takes nothing and later needs the same supply, the narrative may acknowledge the tradeoff without saying they should have predicted the future.

### 17.7 No total closure

The final text should resolve the cache's current policy and the Holdfast's count. It should not identify every visitor, decide whether the lower route was always safe, or reveal a single authoritative account of the entire season. The player closes a present decision. They do not complete a historical court case.

---

## 18. Cross-system interactions and authored pacing

| Existing domain | Content connection | Player-visible output | Boundary |
|---|---|---|---|
| Expedition | Approach, travel time, party composition, supplies, retreat | A journey with a real opportunity cost | No duplicate expedition scheduler or danger roll |
| Wasteland map | Firebreak marks, route fragments, stale directions | Better-informed route choice | No second map or custom route authority |
| Weather | Visibility, wind, wet gear and safe return windows | Delay, alternate route or retreat | No new weather simulation or forecast claim |
| Inventory | Existing rope, fuel, clothing, food or tool rows | Carry/leave/use tradeoff | No item without current catalog and consumer |
| Shelter needs | Cold, fatigue, food and labor already tracked | The party's present capacity shapes choice | No new need or injury meter |
| Survivor relations | Prior work, shared assignment and response to disagreement | Dialogue variants and relationship callbacks | No story-local affinity value |
| Reputation / faction standing | A report may influence an existing community contact | Changed availability only if current owner supports it | No new faction or reputation ledger |
| Journal / chronicle | Confirmed facts and uncertainty can be recorded | A readable account survives the trip | No second archive or free-text save |
| Fire / audio | Stove, wind, hinge and branches support place texture | Quiet, practical sound cue or text | No new ambient manager; no spooky implication |

### Pacing windows

- **Early campaign:** one fragment or a short expedition brief introduces the place without requiring the player to divert from urgent shelter needs.
- **First visit:** one to three observations and a resource decision. The player should not spend a whole play session reading.
- **Middle campaign:** a later note or witness offers a contradictory account after the player has seen the physical marks.
- **Late campaign:** the posted rule or unposted absence returns as a regional memory. The callback should depend on a player action, not an arbitrary day count.
- **Long campaign:** if the world reaches a season where the route is relevant, the cache may matter as a remembered location. Do not force a new threat just to reactivate it.

### Replayability

The story can vary by whether the player has a forestry-skilled survivor, what weather is present, whether the team is already short on warmth or food, whether the player uses the upper or lower route, which documents they inspect, and which policy they post. The underlying truth remains stable: no source contains a complete roster, the cache was used beyond the formal crew plan, and no one can identify every recipient. Replay difference should come from point of view and evidence order, not random rewriting of history.

---

## 19. Faction and social response

No new faction is introduced. Existing faction identities should react only if the North Woods lies inside their current territory and if a current faction event/standing consumer already handles location decisions. The plan must not assign the cache to a new faction because it needs a gatekeeper.

### Holdfast internal response

- The Quartermaster cares about known quantity and replacement cost.
- A route-capable survivor cares about whether the lower mark can be read under snow.
- A medical worker cares about whether a traveler can reach shelter before exposure worsens; they do not ask for supplies they cannot use.
- A survivor with previous scarcity or displacement may react to the sealed-reserve rule, but the line is tailored to their established voice, not a generic trauma response.
- A community speaker may object to an unsigned issue policy because they fear responsibility will land on the Holdfast. Their concern can be reasonable.

### External response

If an existing trade or outpost contact learns of the cache, they may ask for its route information or offer to leave a replacement blanket. This is not a new market transaction unless the current economy and route owners can express it. An external contact may also refuse the information because the route passes through unsafe land. Their refusal is a meaningful limit, not a scripted villain turn.

### Regional memory

The strongest social consequence is the language other people use later: “the store with the dry bench,” “the store they sealed,” “the store with the paper rule.” These phrases may be used in a current radio, journal or encounter catalog after a content-utilization check. They should not become invented reputation tiers.

---

## 20. Implementation classification and non-goals

### Content-first promotion order

1. Recheck the current map, expedition, quest, journal, item and encounter consumers.
2. Run a proper-name and ID collision audit across all current authored data.
3. Select a content form the existing loader consumes. Do not create a new catalog just because this document has a useful table.
4. Author the minimum set of scenes that proves the story is playable: dispatch, first visit, contradictory evidence, policy choice, callback.
5. Add optional records and encounter variants in separate increments if the first slice is reachable and useful.
6. Validate all references through the current data-integrity/content-utilization pipeline.
7. Claim exact paths in the live ownership ledger before any data or source edits. Keep all currently claimed Wave 31 paths untouched.

### Non-goals

- No cache inventory or cache depletion system.
- No forest, wildfire, tree-growth, forestry labor or land-claim simulation.
- No generalized route-signing or player-painting interface.
- No new weather states, winter injury rules or visibility dice.
- No new journal, testimony, settlement, faction, outpost, trader or survivor roster.
- No free-form text entry or author-generated player names.
- No new loot table merely to support a prose prop.
- No migration of old Unity-era content or data authority.
- No mass rewrite of the existing map or old forestry documents.

### Data-only viability

The major story is potentially data-only if existing quest/encounter content supports repeat visits, branch flags and journal output. The continuity callback may require DATA + WIRING if it needs one missing but narrow route from a current expedition result to the existing journal. That seam must be demonstrated from source. If it does not exist, deliver a self-contained location story with no cross-wave callback rather than invent an event bus or new save store.

---

## 21. Collision audit and creative attack

### Duplication risks

- **Another scavenging location:** avoid this by making the player's core task interpret and publish a supply rule; loot is secondary.
- **Another lost-child mystery:** the mittens remain an ambiguous object, not a child-recovery quest.
- **Another morally pure ration story:** every outcome carries a cost; the narrative never treats sharing as free.
- **Another hidden faction cache:** the cache belongs to a documented forestry store, not a new armed group.
- **Another “radio voice from the past”:** the radio is a practical test or transcript, never a supernatural carrier.
- **Another wilderness horror episode:** danger comes from known travel conditions; there is no creature, curse or stalking villain.
- **Another family heirloom quest:** the items are ordinary equipment with use marks, not a unique relic that resolves grief.
- **Another procedural bureaucracy satire:** forms sometimes fail, but the people writing them also correct their own mistakes.

### Questions the finished content must survive

- Can the player finish after only one visit?
- Does the story still work if Runa's note is never found?
- Can a player leave without condemning a survivor or a whole faction?
- Are quantities and rewards real, present and consumable in the current build?
- Do map marks affect route planning only through a current owner?
- Does an ending make an honest account of what the player knows?
- Is the player free to prioritize an immediate shelter need?
- Does the story remain interesting if the player revisits during a different weather state?
- Does any dialogue say what an environmental object already shows?
- Is there a reason for every recurring prop to be where it is?

### Repair pass

If the story begins to feel like a mystery, cut the false villain and add a direct work record. If it begins to feel like a moral test, make the resource cost visible before the choice. If it begins to feel like a loot run, remove reward-first text and make the lower route reveal a different account. If it begins to feel too vague, clarify the present-day issue and leave only the historical identity uncertain. If it begins to feel too resolved, remove one explanatory document and let the player's record remain provisional.

---

## 22. Acceptance criteria for the design bible

A content implementation proposal derived from this plan is ready for a future wave only if all of the following can be demonstrated against current source and data:

1. The player can discover, visit and leave the North Woods site through an existing map/expedition route.
2. The core choice can be expressed with existing quest or encounter data, or with one narrowly accepted wiring seam.
3. Every item, route, character, faction, journal effect and reward resolves to a current authority; unsupported elements are removed rather than simulated in prose.
4. The final account separates confirmed evidence from inference and preserves the reason for the player's choice.
5. At least one meaningful later callback reflects the posted/absent policy without requiring a new state system.
6. The authored content passes current catalog integrity and content-utilization gates, with no new orphan IDs.
7. A new playthrough can reach a different evidence order and resolution without randomizing the underlying facts.
8. Proper names and identifiers are checked for collisions, and the three-plan North Service Packet remains optional across all Wave 11 plans.

**Strongest recommendation:** implement the five-scene spine first as a content-only vertical slice: dispatch, cache arrival, lower-route clue, policy choice, and later reader. Add the optional records only after those five scenes have real consumers. The writing should remain legible if the optional corpus never ships.

---

## 23. Cross-wave handoff notes

Expansion 62 gives Wave 11 its human scale: a cache, a bench, and a rule. Expansion 63 will look at the electrical exchange that wrote the route request. Expansion 64 will examine the material test that affected whether a cable repair could survive cold. Neither later plan may retcon Runa's actions, reveal every traveler, or make the cache's contents a secret reward for a separate feature chain.

The shared North Service Packet has three physical copies and no master copy. Its date marks are compatible but not complete. The player can make a practical choice at any of the three sites without visiting the other two. If all three are played, the player can infer that a relief route was attempted, a switching schedule changed, and a material was limited for winter use. The player still cannot know which person saw which warning or whether a later request reached its intended reader.

The series should end with an ordinary maintenance action, not a new regional governance mechanic. A person may clear the threshold, copy a route note, replace a worn cord, or leave the bench dry. That is the scale of the promise.

---

## 24. Source note

The evidence summarized in Section 2 was checked directly against current workspace files during drafting. The August 2026 canon registry and context atlas were treated as navigation aids, not as current proof. A later integrator must re-read the named source and data files at the time of implementation. The known presence of unrelated dirty changes and active claims means this plan does not edit shared governance, source, tests or gameplay data.

**End of Expansion 62 design bible.**
## 25. Extended branch scene: the Holdfast notice board

This scene is not a tribunal. It takes place beside the expedition board after the party returns with whatever evidence it actually recovered. The player may arrive with a full packet, one note, or only the original map fragment. The room contains two occupied benches, a list of work assignments, and a pot of water cooling on the stove. Nobody stands at a lectern. The people in the conversation are trying to decide how the Holdfast should describe a place it does not own.

### 25.1 Opening state: report with a complete packet

**Quartermaster:** “I have the count. I have the tag. I have the note. I still don't have a name for the person who took the long rope.”

**Route survivor:** “Maybe the name isn't the part we need.”

**Quartermaster:** “It is the part that makes the report look complete.”

**Player:** “Then it should look incomplete.”

The quartermaster looks at the page again and rubs one corner flat. “That is a harder form to file.”

**Player choices:**

- “File the count and leave the recipient unknown.” The report distinguishes visible stock, inferred issue and unknown identity.
- “Call the issue unauthorized.” The report can preserve that the formal roster did not include the recipient, but must not claim theft without evidence.
- “Do not file it yet.” The evidence stays with the player until a later scene, if a current journal owner supports that choice.
- “Ask who needs the cache next.” The conversation shifts from history to present route planning, using current expedition and shelter needs.

### 25.2 Opening state: report with one clue

**Quartermaster:** “You saw the empty shelf.”

**Player:** “I saw the shelf. I saw one tag. I do not know the count before the tag.”

**Quartermaster:** “Most people would put the count on the form.”

**Player:** “Most forms are not standing in the room.”

The quartermaster may answer: “The room is why I want the form.” That line makes their caution legible without turning them into an antagonist.

### 25.3 Opening state: no report

A survivor notices the blank return line. They do not ask where the player went. They ask whether the weather held.

**Player:** “Long enough.”

**Survivor:** “Long enough to get there?”

**Player:** “Long enough to come back.”

The player may tell the full account later. The game does not force disclosure as a reward gate.

### 25.4 The notice-writing conversation

**Quartermaster:** “If we leave a rule, it should say what to do when the person cannot leave a name.”

**Route survivor:** “It should say what to do when the person has no time to write.”

**Medical worker:** “It should not ask someone to inventory a blanket while they are shaking.”

**Player:** “Then the paper cannot be the whole rule.”

**Quartermaster:** “No. It can only make the next person less likely to guess.”

This line is the design's ethical center. It does not tell the player that records are useless. It defines their limit.

### 25.5 Public wording variants

**If the player chooses sealed reserve:**

> North Woods stock is for planned work and emergency travel. Check the weather and route before dispatch. If you are already inside the store, keep the door clear and leave a count. The notice cannot authorize an unsafe crossing.

**If the player chooses open issue:**

> The cache may be used by travelers. Take what you can carry and use safely. Leave a note if you can. If you cannot write, leave the objects dry and the door shut. The next count may not match this one.

**If the player chooses a work-share reserve:**

> Tools and rope may be borrowed for a named repair or route. Leave the work site and what remains. Food and dry gear may be used for immediate need. Do not promise that an item will return before you have it in hand.

**If the player writes no notice:**

The player sees a blank line and an empty pencil groove. The camera does not linger. A later visit can reveal that another person wrote something; it is not guaranteed.

### 25.6 Response after the wording is posted

**A worker who agrees:** “Now the rule says what it cannot promise.”

**A worker who objects:** “Someone still has to decide what counts as immediate.”

**The player:** “Yes.”

**Worker:** “Then write who decides.”

**Player:** “The person who is there.”

**Worker:** “That is not a name.”

**Player:** “It is the only name the rule can always reach.”

The exchange has no triumphant response. If the game supports a journal entry, it records that the player posted a policy and that the group disagreed about its limit.

---

## 26. Additional voice and interaction lines

These lines are designed as short responses to the same environment. They are not generic barks to be repeated on every expedition. Use a small number per scene, conditioned by the current witness and what the player inspected.

### Quartermaster

- “That tin is counted. The meal inside it isn't.”
- “You can have a clean shelf and a wrong number.”
- “I can replace the rope. I cannot replace the path it carried someone across.”
- “If we take the blanket, write the take before the reason. Reasons change. The empty shelf stays.”
- “I don't want a perfect ledger. I want a ledger that knows what it doesn't know.”
- “We can close the door without closing the place.”
- “A reserve no one can reach is a number. A reserve everyone can empty is a memory. We need a middle word.”
- “The last tin is not always the last meal. It depends who can carry it.”

### Route survivor

- “The ribbon points inward because whoever tied it was looking from the path.”
- “This mark was made with a knife, not a forestry stamp.”
- “The cut is old. The snow made it look new.”
- “I would take the ridge in daylight. I would not take it with this wind.”
- “Someone used the post as a handhold. See the polished side?”
- “The trail is open. That does not mean the crossing is.”
- “If the tree is down, follow the mark on the next tree. Do not make a new line through the brush.”
- “We don't need the whole route. We need the next safe place to stop.”

### Runa Dey, from a note or later recording

- “A store can be shut. The weather does not read the notice.”
- “I stopped writing names when people started asking whether a name was required to be cold.”
- “I should have entered the blankets. I did enter the blankets. I did not enter who had them.”
- “The count is a tool. Keep it sharp. Don't use it as a handle.”
- “If the new keeper wants the key, tell them where I hung it. If they want my reason, tell them to walk the route once.”
- “I didn't leave the door open. I left it possible to open.”
- “No, the mittens aren't evidence of a child. They are evidence that I put mittens there.”
- “I remember the cup. I don't remember the face.”

### Tovin Marr

- “The map has a fold right over the water. It was stored by someone who knew where the wet part was.”
- “You can walk the wide trail faster. You cannot walk it through the tree.”
- “I marked the turn twice. The second mark is not a correction. It is for people coming back.”
- “A boot print tells you where the boot went. It doesn't tell you why.”
- “The station was quiet. The ridge was louder.”
- “I took the sign down because people followed it after the crossing changed.”
- “If someone says the path was obvious, ask which day they mean.”
- “The pencil still works. It just does not like being asked to tell the same story twice.”

### Seli Orr

- “The blanket dried by morning. I left it folded because I could not carry the wet one.”
- “The boots were there. I used them. I put them back. If that is not borrowing, I don't know the word.”
- “I did not see who put the mittens there.”
- “The road was not open. It was passable for us.”
- “If you want the exact number, count the lids. If you want the meals, ask a person who is still alive.”
- “I didn't keep the name because I did not know whether the person wanted it kept.”
- “The stove drew badly. We did not light it.”
- “I slept on the bench. The bench was dry. That is what I remember.”

### Ambient interaction notes

- A character who has not seen the lower route should not speak as if they know where it ends.
- A character who is exhausted can ask to stop before giving an interpretation. Do not use a single voice line to diagnose them.
- A survivor's profession informs vocabulary, but no one becomes a walking glossary.
- If a line refers to an item the player already removed, the actor comments on the empty outline rather than repeats the object description.
- If the player skipped a document, dialogue says “we have one account” or “we have the count,” not “we know what happened.”

---

## 27. Full closing passage variants

The ending should be short enough to leave space for play, but specific enough to remember the player's action. The following passages are complete candidate journal entries. Only one is shown per ending, and no passage calls itself a moral verdict.

### 27.1 Closing passage: reserve kept

> The store is still there. The shelves are still counted. You left the rope, the blanket and the tins where they were, and wrote that the count is incomplete. The paper does not name the person who used the lower route. It does not say that the door should stay closed in every kind of weather. It does say what the Holdfast saw and what it did not.
>
> On the way back, the ridge marker was visible from below. You could not see who had tied the second ribbon. You could see where it pointed.

### 27.2 Closing passage: stock released

> The Holdfast carried the selected supplies out of the North Woods store. The list records what left, where it went and why. It does not replace the old count. A blank remains beside the line for the next crew.
>
> The cache is lighter. The party is warmer. Both facts belong in the report.

### 27.3 Closing passage: split rule left

> You left a notice above the bench and placed the pencil where a seated person could reach it. The notice says the store can be used. It also says not to promise a return date for an item before it is in hand.
>
> The old list still reads “two crews, one season.” Below it, the new page reads “issue.” Someone will have to decide what that word means when the weather changes.

### 27.4 Closing passage: report withheld

> You brought the count home in your head and left the paper where it was. No one at the Holdfast can file the lower route as safe. No one has filed it as closed. The next expedition can begin from the same uncertainty you did.
>
> You know the bench is dry. For now, that is not enough to make a plan.

### 27.5 Closing passage: expedition turned back

> The party left the North Woods before reaching the cache. The route note remains folded across Firebreak Seven. You can see where the old mark disappears under the ridge.
>
> The report records the reason for turning back. It does not call the trip a failure. The route is still there on the map, waiting for a day when the party can afford to ask it another question.

### 27.6 Closing passage: no notice posted

> You left no new rule. The old forms remain: one for crews, one for stock, one with a blank recipient line. The cache has not become less useful because you did not decide what it should mean.
>
> At the door, you checked the latch twice. The second time, it held.

---

## 28. Final design position

The Cache Grid is strongest when it treats every object as evidence with a limit. Rope tells the player that a route was used; it does not identify everyone who used it. The blank signature line shows that a system had room for a name; it does not show that anyone felt safe giving one. A dry bench proves that someone prepared a place to sit; it does not prove who arrived. The player works with these limits while managing the very real cost of a return trip and the Holdfast's current needs.

The package can provide a full, satisfying story without one new gameplay subsystem. Its new value comes from the way the authored material asks the player to travel, inspect, compare, decide, and return. That is the intended measure of expansion 62: more consequence and memory from the systems and places already in the game.

## 29. Three dispatch briefs for different campaign conditions

These briefs let the same authored story enter play without pretending every campaign has the same immediate need. They are alternate framing text, not separate questlines.

### 29.1 The Holdfast is short on warm gear

> The insulated boots in the North Woods list are not a guaranteed pair. The expedition board says the store may hold a field kit, but the route crosses exposed ground and the weather report is not clear enough to promise a safe return window.
>
> The Quartermaster has not asked the party to bring everything back. They have asked for a count. If the boots are sound and the route allows it, bring them. If the cache is being used by someone else, write that down. The next team should know what the shelf looked like today.

### 29.2 The Holdfast has enough supplies

> The Holdfast can afford a return trip. That does not make the trip free. The party will spend time, food and energy on a place that may have nothing the shelter needs today.
>
> The map fragment is still incomplete. If the cache is as described, the expedition can improve the route account and come back with no cargo. The board will still need a report. “Nothing worth taking” is not the same as “nothing found.”

### 29.3 The party is already committed elsewhere

> The North Woods route remains available, but the current party has another task. Do not force a dialogue that asks them to abandon it for the cache.
>
> The fragment can be pinned to the board and marked “unverified.” The storm note can be copied. A different survivor may lead the trip later. No one has to pretend that the cache will vanish because the player waited.

## 30. Content integrity note

Every quoted item in this design bible has a source role and a proposed point of discovery. Before any of it becomes runtime text, the author should check whether the same line, name, object or historical claim already appears in the full current data tree. The sample wording is original for this proposal, but thematic overlap is possible in a corpus of this size. If an exact line conflicts, preserve the scene's function and rewrite the line; do not preserve a phrase just because it appears in this file.

The name Runa Dey is a proposed authored identity. The name check performed during this draft found no exact match in the current data and game-master document search. That search is not a substitute for the repository's eventual identifier validator or a case-insensitive proper-name census. The plan intentionally leaves Seli Orr's kinship, the small boot's wearer, the number of travelers, and the final fate of the second crew unresolved. Those are not gaps to be patched with a late explanatory note.

The cache's original inventory can only be represented by its existing textual/map evidence. No numerical stock total is specified for runtime because the current location's loot table and the expedition loot consumer must be checked before promotion. If the design requires a specific quantity to make the split outcome function, it should use the current data's actual item count or revise the choice. It must not grant supplies through narrative text while leaving inventory unchanged.

**Character-count target for this design bible:** approximately 120,000 Unicode characters including headings and whitespace. The authored prose is part of the design payload; it is not padding and should be reviewed as narrative content.


A complete reading is not a required player action. The map, the bench, and the issue choice carry the central story. The optional papers make the history wider, but no player should need to collect every scrap to be treated as attentive. Keep the first visit short, place the clearest contradiction where a tired party can see it, and let a later return reward curiosity without delaying the next day of shelter management. If the player records only one fact, let it be this: someone left the bench dry for the next person.

The site should remain useful after the quest ends. Later expeditions can still use its route and supplies under the current map and loot rules. What changes is the player's understanding of who may have used the place before them, and what a fair count can honestly say.

The next visit begins with what the previous one chose to leave.
