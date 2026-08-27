# ASHFALL: THE STANDING RECORD — Creative Pack

**Internal id:** `expansion_the_standing_record`
**Kind:** Shippable prose. Additive to `docs/expansions/expansion_03_the_standing_record_plan.md`. Does not rewrite the bible.
**Voice lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.
**VO:** Lines marked `[VO]` are text-first; record only if the radio/tannoy pipeline already exists. Everything else is UI/Codex/inspect.

Ids reuse the Standing Record bible. Re-grep `locations.json`, `locations_expansion3.json`, `QuestlineSO`, `faction_lore.json`, `currents.json`, `world_history.json`, Holdfast, and Duty Roster proposed ids before implementation. No C#. No seventh Power. No Tessarat, Sector 7G, terraformers, androids, neuromancers. No second District 8 coast. No walkable 3D interiors.

Layouts are **node ticks**: enter room → inspect / encounter / choice → adjacent rooms unlock or stay dark. Each room names one object to steal, weigh, or refuse.

Do not copy Holdfast or Duty Roster sentences. New objects.

---

# 1. Full layout cards

Schema: `id`, `displayName`, `inspect` (one line), `description` (80–180 words). Parent travel stats stay on the existing location id.

---

## 1.1 Kilometre 19 — `loc_cut_kilometre_19` *(Holdfast parent; seam)*

### `room_km19_post` — The Post

**inspect:** Ivy's stencil. Overlay's plate. Four screws, one of them the wrong metal.

**description:**
The reflector post is still Lamplighter orange, the kilometre stencilled twice because the first pass ran. Over the second stencil, a brass plate, municipal, four screws, stamped `CUT-19 / LAMP`. Three screws match. The fourth is steel, bright, a field repair. The lamp is lit on Ivy's schedule. The plate does not mention oil. A spirit-level leans against the base, bubble still between the lines, as if Maren set it down to argue with a post that was already vertical. You can take the plate. The stencil underneath is colder than the brass and will read again in a day if nobody screws it back. You can take the steel screw and leave the brass ones. The next Overlay junior will know someone cared about the mismatch and not about the name. You can refuse both. The lamp will still be Ivy's. The number will still be theirs.

### `room_km19_seam` — The Seam

**inspect:** Ash on one side of a survey nail. Salt-white on the other. The nail is new.

**description:**
The ice changes colour on a line that is not quite straight. Sector 4 ash packed into the south grain; District 8 rime on the north, thinner, meaner in the light. A survey nail has been driven at the join, tag wired on: `SEAM / DO NOT TREAT AS ONE DISTRICT`. The tag is Overlay stock. The nail is a cut-down ice-spike, still with mill-scale. An Overlay-issue work glove, adult, left-hand, is frozen open on the far side of the nail, fingers toward the Cut. It is not the child's mitten on the reflector — that object is still there if Holdfast shipped it — this is a different hand, a different job. You can pull the nail. The seam will still be a seam. The next sheet Ostrowski sells will be missing a point he did not survey. You can weigh the glove. It has a lining of lampblack. You can leave it. The ice will take the shape either way.

### `room_km19_oil_tin` — The Oil Cache

**inspect:** Ivy's wick tin. Beside it, a pigment pot with a thumbprint in lampblack.

**description:**
Nailed to the post, a tobacco tin of spare wick, tissue-thin. In the snow at the base, not the oil can from last winter's argument — a smaller pot, screw-lid, Overlay stencil `PIGMENT / LAMPBLACK / CUT`. The lid is stiff. Inside: lampblack mixed with a finger of oil that smells like Ivy's reservoir. A rag stopper, used, sits in the snow between the two containers as if someone meant to return the oil to the lamp and returned it to the paint instead. You can take the pigment. The next plate will be stamped without black in the letter-cuts and will not read at a distance. You can pour the oil back into Ivy's lighting. Maren will log a short. You can refuse both tins. The lamp will still need a lighting that is not this pack's to author.

### `room_km19_plate_crate` — Plate Crate

**inspect:** Spares. Tissue between plates. The numbering runs past the post you are standing at.

**description:**
A crate on runners, the kind the Cutters tow and forget. Lid off. Brass plates in a stack, tissue paper between each, stamped `CUT-19` through `CUT-24`. 19 is missing if you already took it from the post, or present as a duplicate if you did not. A packing list in a civil-service hand: kilometre, lived name (blank), date screwed, oil noted (blank). A screwdriver with a worn brass ferrule lies on the tissue. You can take a spare. Somewhere a post that is not this one will go unnumbered, or will be numbered twice. You can take the screwdriver. The next plate will be seated with a knife. You can close the lid and leave the crate for Maren. Dark until the post has been inspected. The crate does not unlock from the bunker menu.

---

## 1.2 Transit Authority — `loc_transit_authority_hq`

### `room_transit_lobby` — Lobby

**inspect:** A disc dispenser with nothing left to dispense. Queue paint on a floor that no longer has a queue.

**description:**
Civic linoleum, peeled to the mastic in a path from the doors to the inner glass. A ticket-disc dispenser, municipal, hopper empty, the last disc jammed edge-on in the slot: a blank. Someone tried to stamp it and the die was already gone. A clock over the inner doors is stopped at a time that matches no convoy slot under the glass. The cloak-rail still has one hanger, wire, twisted into a hook that will not hold a coat. You can take the blank disc. It authenticates nothing and Overlay will try to stamp it anyway. You can leave it jammed. The next person through will think the machine still works. The inner doors are dark until you walk to them. There is no tannoy. There is a sign where the tannoy was: `LISTEN FOR YOUR SLOT`. The slots are grease.

### `room_transit_map_glass` — Map Glass

**inspect:** Wall-sized routes. Grease pencil. A trestle of printed plates waiting to become the truth.

**description:**
Glass over paper over wall. The published timetable is pre-Exchange and wrong. The grease pencil is the document: convoy slots, arrows, a circle twice around Convoy 12, and beside it, same hand, `HELD — DOB QUERY`. The pencil hangs on a dirty string. Overlay has set a trestle at the foot of the map, printed plates in a font that does not include HELD. Maren's packing list would call this `install`. You can copy the grease. That is the lore the bible already promised, found by standing here. You can take the pencil. The next correction will be a fingernail. You can let Overlay screw plates over the glass. The hand will still be under there, paler. You can refuse all three. The map will go on meaning two things, which is already its job.

### `room_transit_dob_desk` — DOB Desk

**inspect:** A telephone with the cord cut clean. A blotter that did the holding.

**description:**
A duty desk facing the map, so the officer could see the slot while they failed to reach the department. The telephone is present. The cord has been cut with one motion, both ends still on the blotter, as if the cut were the last procedure. The blotter: Convoy 12, six hours eleven minutes, a child's birth year written twice, once crossed through in the same pencil as the map. A rubber stamp `FOLLOWING PROCEDURE` with the pad dried to a scab. You can take the stamp. It will not ink. You can take the blotter. Ira's Record has a site column that will want this desk listed as `TRANSIT / DOB`. You can refuse to sit. The chair remembers a weight anyway; the seat is shiny. Adjacent: radio gallery unlocks after the blotter is inspected.

### `room_transit_overlay_bench` — Overlay Bench

**inspect:** Typeset slots. A font without a word for held.

**description:**
A folding bench, Overlay-issue, clamps, a tray of screws that match Kilometre 19's brass three-of-four. Printed plates: convoy numbers, departure times that correspond to a timetable Continuity filed and nobody drove. Convoy 12's plate has a time and no HELD. A small bottle of solvent for lifting grease. The cap is off. The smell is in the map room whether you open this bench or not, once the door is lit. You can install a plate. You can crate them. You can solvent a corner of the glass as a test; the grease smears into a weather. Maren will log whatever you do as `field`. You can steal the solvent. The next install will be dry and crooked. Dark until map glass inspected.

### `room_transit_radio_gallery` — Radio Gallery

**inspect:** The last order to turn the buses around, still on a spindle.

**description:**
A narrow gallery behind the maps, headsets on hooks, a spindle of message flimsies. The top flimsy is the turn-back: unlisted traffic to reverse at the loop, city-bound, signed with a grade that had already gone north. The paper is oil-spotted. A headset has foam missing on the left cup; whoever wore it listened longer than the foam lasted. You can take the flimsy. The bus loop will still point at the city. You can leave it on the spindle. Overlay will typeset it as `EVACUATION COMPLETE` if you let them reach the loop first. You can wear the headset. It is dead air. `[VO]` If recorded: six seconds of hiss, then nothing. Text fallback: the gallery is quiet enough to hear the map-room clock not ticking.

---

## 1.3 Municipal Archive — `loc_municipal_archive`

### `room_archive_vestibule` — Vestibule

**inspect:** A visitor book. Two Garrison searches. An Overlay third line, unfinished.

**description:**
Glass doors, one pane starred. A counter with a visitor book chained to a brass rail. Columns: date, unit, purpose. Two entries in a Garrison hand: `SEARCH SCHEDULE` and, months later, `SEARCH SCHEDULE (REPEAT)`. Both purposes are the same and both results are nothing, written in the remarks as `NOT MUNICIPAL`. A third line has been started in Overlay pencil: `FILE RECORD` — no date, no name. A bottle of drying-sand for wet ink, fused into a lump. You can take the book. The chain will still be there. You can finish the third line. Maren will not thank you; she will treat it as done. You can refuse the remarks column. The Garrison already wrote the useful sentence.

### `room_archive_grey_brick` — Grey Brick

**inspect:** Fire suppression made a geology of paper. A plate is sinking in it like a fossil.

**description:**
Rolling stacks, most off their rails. Below waist height the discharged suppression has set the paper into a grey mass you can knock and hear. An Overlay plate is half-sunk in the mass, only `REC-` still legible, the rest a bruise. A mason's chisel leans nearby, Overlay-issue, edge rolled from trying to be an archive tool. You can dig. Hours. Collapse risk. You may free a field index, or you may sink the plate deeper. You can pull the plate now and leave the brick. The index, if it exists, stays in the geology. You can refuse. The Garrison looked twice and went back to shooting deserters, which at least happens at a desk. Adjacent reading cage stays dark until you choose to dig or to walk around to the dock.

### `room_archive_reading_cage` — Reading Cage

**inspect:** Dry. A field index. Not the Schedule. The Schedule was never municipal.

**description:**
Wire cage, desk, a lamp with a working battery Overlay brought and will want back. On the desk: a ring-bound field index, sites not households, Continuity cadastral in the left column, lived names mostly blank, a third column `DRAWER` that refers to municipal numbers that drowned in the brick. Ira's book is the sibling of this; this is the cheaper copy. A rubber thimble, used. You can copy the index. You can take it. Ira will know the difference when the visited column still does not match. You can refuse to call it the Schedule. Pell, if he hears you have a book of addresses, will not care what you call it. Dark until brick resolved or dock walked.

### `room_archive_loading_dock` — Loading Dock

**inspect:** A crate stencilled RECORD / NOT SCHEDULE. The not is in a different hand.

**description:**
Roller door jammed a metre up. Weather has been in. A Continuity crate, empty, stencil `RECORD / NOT SCHEDULE` — the `NOT SCHEDULE` is brush, not stencil, as if someone had to say it after the crate was already marked. Straw. A broken tension-strap. You can take the strap-buckle (brass). Frayne and Maren will both notice a world with one less fitting. You can sit in the crate. It is sized for plates, not people. You can close the roller by hand a further handspan. The dock will be darker. Overlay uses this door when they do not want the vestibule book.

---

## 1.4 Ministry — `location_ministry_of_truth_bunker` *(SPINE)*

*Recast parent inspect when expansion unlocks: not propaganda servers. A civil-service hole that kept working.*

### `room_ministry_stair` — Stair

**inspect:** An authenticator light on a fuse that should have died. Cream paint that was never allowed to fade, and did anyway.

**description:**
A stair that was designed to look like work. Civil-service cream, scuffed to the primer on the nosings. At the landing, an authenticator plate, Allocation-family, light still on, fuse-box beside it with a paper tag `DO NOT REPLACE — STANDBY`. Someone replaced it anyway; the fuse is newer than the tag. A brass handrail, unscrewed at the top and left hanging on the bottom bolts, as if a nameplate crew started and were called to a different job. You can take the hanging rail. It is fittings. You can steal the fuse. The light will die and the stair will still be a stair. You can refuse the authenticator. It will not open a hatch. It only means this building thought it was still a department.

### `room_ministry_enquiry` — Enquiry Desk

**inspect:** A bell that still works. A memo telling staff to stop answering.

**description:**
A counter with a bell, nickel, dented. It rings. Behind the counter: a memo in a plastic sleeve, `PENDING CLARIFICATION — NO FURTHER ELIGIBILITY ENQUIRIES`. The clarification never came. A stack of blank enquiry forms, yellowed, the question *shelter number* still printed. A pencil tin with no pencils, only shavings. You can ring. Ira will come from the registrar if she is here and the book is still in the building. You can take the bell. The next enquiry will be a knock on wood. You can take a form and write a number. The form will not become a hatch. Overlay wants this desk listed as closed. It is closed. The bell disagrees.

### `room_ministry_scoring` — Scoring Floor

**inspect:** Occupation points on the wall. A water engineer is thirty-one. A records clerk is nine.

**description:**
Charts thumb-tacked, then pinned, then left. Reconstruction Utility Rating, worked as a poster because transparency was the same thing as fairness. Occupation up to forty. Water engineer 31. Paediatric nurse 28. Records clerk 9. A dependent line. A sixty. Someone has written, very small, in the margin of the clerk line, `STILL HERE`. The hand matches the visitor book at the archive or does not; you would have to carry both. A wooden pointer, classroom, used to reach the top of the chart. You can take the pointer. The next reading will be a finger. You can take the clerk-line scrap. Ira will notice a hole. You can refuse to do the arithmetic. The arithmetic was already done, on four million people, without you.

### `room_ministry_registrar` — Registrar

**inspect:** The Standing Record. Sites, not households. The visited column is empty.

**description:**
Ira Vell's desk. A book the size of a ledger, bound in the same cloth as municipal minute-books. Columns: site, cadastral, lived name, visited. Lived names are sparse. Visited is empty, or has ticks only where you have already stood in this pack's rooms — never ticks from a photocopy in a rucksack. A pencil is tied to the spine with the same dirty string grammar as Transit. A cup of water, process or melt, replaced and not drunk. The household Schedule is not in the right-hand drawer; a lighter square in the dust says it was. District 8 has a copy. Sole has fragments. This book's job was ground. You can copy pages. The visited column will not fill. You can take the book. Ira will write a receipt. Overlay will call it `field complete` and be wrong. You can refuse to touch it. She will still show you the empty column, which is the point of showing it.

### `room_ministry_obstacle_annex` — Obstacle Annex

**inspect:** Denial doctrine in a binder. Friendly obstacles, marked. Not a crawl.

**description:**
A side room, maps of Sector 4 with symbols that match D/9's rail-scratches and culvert stones. A binder: Obstacle-Marking Annex. Nothing hidden. Fully, correctly, uselessly signposted. Four living people can read the hand; this binder is the printed cousin, which is not the same as the hand. A strip of tape at a marked height on the doorframe, as a teaching aid. You can take the binder. Anneke Ruhl would not call that a stand-down. You can take the tape. The next teaching will be guessed. You can refuse to treat this as a dungeon. It is a filing of how not to walk into your own charges. Bridge Seven's underside is the other room of this sentence.

### `room_ministry_dead_phone` — Dead Telephone

**inspect:** The hold, in hours and minutes, in a hand that was following procedure.

**description:**
A smaller desk behind Ira's, the overflow enquiry that became Convoy 12. Telephone dead, cord intact here — the cut one is at Transit. Blotter: six hours eleven minutes, a birth year twice, a note `DEPT EVACUATED — CONTINUE HOLD`. A teacup ring that has been wiped and left because wiping did not remove it. You can take the blotter. Edor's own return, if Holdfast live, will rhyme and must not be joked about. You can leave it. Overlay will typeset the hold as a completed slot. You can weigh the teacup. It is empty. Adjacent from registrar only.

---

## 1.5 Weighbridge — `loc_weighbridge`

### `room_weigh_plate` — The Plate

**inspect:** A mechanical readout that still believes in kilograms. Overlay lots stacked where the calibration weight should hang.

**description:**
The truck scale, iron, a needle in a glass that has hairline cracks and a stop painted by someone tired of replacing it. The calibration weight marked `500 kg` hangs on a hook, or does not, if Overlay borrowed brass. The plate of the scale is scored with tyre-tracks and one human boot-print, heel toward the hut, as if someone stood there to be priced. You can stand on it. Osric will write a mass. You can put an Overlay lot-plate on it. It will also be a mass. That is the joke the Warlords repeat. You can take the calibration weight. The next column will be wrong by five hundred kilograms and Osric will know the ice did not do that. You can refuse to treat a lot number as a weight. The needle has no opinion.

### `room_weigh_hut` — Hut

**inspect:** Spring-balance for small loads. A kettle. Osric's back to the door.

**description:**
A hut that has always been here, unlike the Ice Weigh Hut on runners (*Holdfast* — different building, different salt). Beam scale inside for loads the truck plate will not take: iodine tins, resin samples, children's boots, Overlay plates. Osric Tann sits with his back to the door. A kettle, tea that tastes of the river whether or not the river is ice. A notice: `FAVOURS ARE MASS`. Underlined once, not twice. You can steal the spring-balance poise. Small loads will be guessed. You can take the kettle. He will still price you. You can refuse to talk about District 8's triplicate colours; his carbons are older and only two.

### `room_weigh_receipts` — Receipts

**inspect:** Carbons. If Edor waited forty days, his unfinished return is in the white stack.

**description:**
A drawer: pink for the traveller, a yellow that the Tollman theoretically keeps, white that has become the archive because nobody collects it. If Holdfast levy was refused, Edor's incomplete return is here, in a good hand, occupations wrong by one. If the column was hidden, the return may be blank of names and still have a date. A spike for paid receipts, transfixed. You can take a white carbon. Osric will not chase you; the needle is his job. You can bury Edor's return under the stack. You can give it back to him if he is on a stool at your hatch. You can refuse to open the drawer. The prices will still have been mass.

### `room_weigh_overlay_lot` — Lot Stack

**inspect:** Plates that want to replace the needle with a number that does not weigh.

**description:**
A crate in the lee of the hut, `TOLL-WB / LOT` and a run of numbers that do not match Osric's serial on the beam. Tissue paper. A packing list with `install on readout glass` as a line item. You can install. The needle will still move behind the brass. Palimpsest. You can crate them back. You can steal a plate as brass. Frayne, Leva, Overlay — same metal. Dark until the scale plate is inspected.

---

## 1.6 Grange Hall — `loc_grange_hall`

### `room_grange_porch` — Porch

**inspect:** Weapons in the stand. A sign that is still a request.

**description:**
A porch with a hand-lettered sign asking visitors to leave weapons here. The sign has been relettered; the nail holes are a history of wording. A rifle already in the stand, tag on the trigger-guard with a name that has a cross in the ledger inside. Oil lamp unlit; daylight is enough. You can leave a weapon. You can take the tagged rifle. The ledger will still have the cross. You can refuse the sign and walk in armed. The Verge will notice with a show of hands later, not now. Overlay has not plated the sign. They plated a notice for the kitchen drawer instead.

### `room_grange_table` — Long Table

**inspect:** Oil lamps. A table that has hosted votes and meals and not distinguished.

**description:**
A long table, scarred, benches both sides. Oil lamps with glass still intact, unusual. Attendance is a ledger, not a feeling. A jug of water, Verge-clean, tin cups chained so they will not leave with Overlay. You can take a cup if you unchain it. The next visitor will drink from their palm. You can sit in Delacroix's place; it is not marked, and that is how you know it. You can refuse to speak. A vote can still happen around a silent person. The lamps smell of the cider press whether or not the Cartwrights have been this week.

### `room_grange_ledger` — Ledger Desk

**inspect:** Forty-seven names. Twelve rifles. Twenty-two crosses in a later hand. Overlay wants a number column.

**description:**
The first page is week one. 47 names, 12 rifles in the margin. Twenty-two crosses, later, different ink. A blank Overlay column has been ruled in pencil down the right edge, cadastral lots, not yet filled, or filled if Dara's hut was dark and they came. You can erase the column. You can fill it from Maren's index. You can refuse to let Lasko become a lot if his vote is pending. You can steal the pen. The next cross will be pencil. The lived gazetteer of the Verge is this book. Ira's Record would call it informal. It has outlived the department.

### `room_grange_kitchen` — Kitchen

**inspect:** A kettle. Overlay's notice face-down in a drawer, as if a vote had already happened.

**description:**
A back kitchen, enamel, a kettle that has a queue. In a drawer: Overlay notice `SITES TO BE NUMBERED FOR RECONSTRUCTION ACCESS`, face-down, a boot-print on the back. Someone stood on it. You can put it on the table. You can burn it in the stove. You can leave it face-down. You can take the kettle. The next meeting will be dry. Dark until the ledger is inspected; they keep the notice away from the first page on purpose.

---

## 1.7 The Allotments — `loc_the_allotments`

### `room_allot_gate` — Gate

**inspect:** Chain-link cut and rewired. The waiting list is visible through the mesh if you stand where the postman would have.

**description:**
A municipal gate, chain-link, cut once at hip height and rewired with brass bell-wire that Frayne would rather see in a valve. A padlock that is ceremonial; the cut is the door. Through the mesh: numbered plots, a hut, a noticeboard in a plastic sleeve. Overlay plate on the gatepost, or a clean rectangle of less-weathered wire where a plate was. You can take the bell-wire. A leak somewhere gets a schedule. You can screw a plate. You can refuse the padlock's theatre and use the cut. Dara Mewn, if she is not on levy, will see you from the hut before you see her.

### `room_allot_hut` — Caretaker Hut

**inspect:** Minutes. An autoclave key. If the levy took the caretaker, the stove is cold and the numbers are already on the plots.

**description:**
A hut that is also the sector's clinic by accident of still and autoclave. Minutes on a nail: *Motion: that we plant the north strip anyway.* If Dara is here, the kettle is on and the key to the autoclave is on a string around a hook, not her neck. If she is north, the stove is ash-cold, a cup unwashed, and Overlay stakes are visible through the window in a grid that ignores the waiting list. You can take the key. The next surgery waits on Ianov's arithmetic without sterile. You can take the minutes. Frayne will write them again. You can refuse to sit in the caretaker chair. It will still be a chair with a depression.

### `room_allot_noticeboard` — Noticeboard

**inspect:** The waiting list in a plastic sleeve. Four of the names are alive and farming here.

**description:**
A board, municipal, notices layered. The sleeve is fogged. The waiting list is still the waiting list; plot 114 is still 114. Four names have ticks in a plant hand, not Overlay. A newer Overlay print would stamp `HISTORICAL` across the sleeve if you let it. You can take the sleeve. The list will yellow faster. You can copy the four ticks. Sole will not accept them as dead. You can refuse to call the list historical. Frayne will not thank you. She will note that the board was not replaced.

### `room_allot_plot_waitlist` — A Named Plot

**inspect:** Someone is farming a number Overlay says belongs to a different 114.

**description:**
A strip of floodplain, good soil because it floods, water you cannot drink. Stakes with municipal numbers. A person — Dara, or another living waitlistee — working a row. Overlay's plate, if present, has a 114 that is not this 114; cadastral and waiting-list diverged before the Exchange and nobody issued a clarification. A watering can, patched, iodine-stained if membrane was stripped south. You can take the can. The row will wait. You can pull the Overlay stake. You can leave both numbers in the dirt. The plants do not read.

### `room_allot_brass_bin` — Brass Bin

**inspect:** Door handles. A nameplate. An Overlay plate. She will not ask which.

**description:**
A bin behind the hut, labelled `FITTINGS` in a Works hand. Contents: door handles, a lamp base, one brass nameplate with the name filed off, and, mixed in, an Overlay site-plate with the stamp still proud. Same metal. You can sort them. You can take eight fittings for Frayne or Leva and leave the plate. You can take the plate as brass and let a site go unnumbered. You can put a nameplate from your tin in and say nothing. The bin does not comment. Dark until the hut is inspected.

---

## 1.8 Bridge Seven — `loc_bridge_seven`

### `room_bridge_near` — Near Bank

**inspect:** A spike of receipts. The span is intact. The underside is a different room.

**description:**
Toll-side approach, four lanes narrowing to a booth that is not always staffed because the charges staff it. A spike of receipts, transfixed, rust and paper. A stone on the marked side of a scupper — D/9 grammar, do not move it. You can take a receipt. It will say mass or bullets depending on the week. You can move the stone. That is not mercy. You can refuse the booth and walk the span. The Tollman does not need to be present for the authority to work.

### `room_bridge_span` — Span

**inspect:** Four lanes over the gorge. Wind. A scratch on the guardrail at the specified height.

**description:**
Open. The river below is Drown-coloured even this far east. Guardrail scratch, tape-height, textbook annex. Overlay survey flags flutter on the downstream side, clipboards in polythene. You can take a flag. The survey will be guessed. You can stand in the middle and not look under. That is allowed. You can refuse to photograph. Ostrowski already sold the span as a line. The charges are not on his sheet as a joke; they are on it as a fact he will not test.

### `room_bridge_charges` — Underside

**inspect:** Taped. Visible. The detonator housing has a dust of five years. Looking is a room. Checking is a branch.

**description:**
Catwalk, official, still there because somebody maintained a way to see the threat. Charges taped to the underside, demolition-family, D/9-adjacent without being D/9's job to keep sticky. Detonator housing, wire, a lock that is rusted shut or Overlay-oiled. Dust on the housing is the honest document: nobody has checked. You can look. You can refuse. You can check — open, test continuity with a meter if you brought one. That mutates Toll access. It is not an explosion setpiece. It is a sentence the Tollman has to write. You can steal a length of unused tape. The next charge will sit on old adhesive.

### `room_bridge_overlay_survey` — Survey

**inspect:** Clipboard: FRIENDLY OBSTACLE / TOLL-B7. Signing it makes a threat into a line item.

**description:**
A folding stool on the downstream walk, clipboard in a plastic sleeve, Overlay hand: `FRIENDLY OBSTACLE / TOLL-B7 / MARKED`. A box ticked `VISIBLE FROM SPAN`. A box unticked `DETONATOR VERIFIED`. You can tick it without verifying. You can scrape the sheet. You can copy it into Ira's Record as lived name `Bridge Seven`. You can steal the clipboard. The next survey will be memory. Dark until span inspected.

---

## 1.9 Bus Reversal Loop — `loc_bus_reversal_loop`

### `room_bus_circle` — Turning Circle

**inspect:** Forty-one buses, nose to tail, city-bound. A suitcase with a street on it, not a number.

**description:**
Tarmac, painted reversal arrows faded to ghosts. Buses packed as if the order were still being obeyed. Between two wheels: a child's suitcase, empty, a luggage-label with a street name in a household hand, no Allocation number. You can take the suitcase. It will not fill. You can leave it. Overlay will plate the circle `EVAC COMPLETE` without opening luggage. You can weigh the label. It is a lived name. Ira's lived-name column would take it if you carried it to her, and she would still want the site visited.

### `room_bus_lead` — Lead Bus

**inspect:** The driver's log contradicts the stencil they want for the bow.

**description:**
First bus, door wedged. Driver's log on the dash, last pages: the turn-back order received, obeyed, a note `UNLISTED ABOARD — NO SLOTS`. A thermos, freeze-split. A ticket punch. You can take the log. The stencil on the bow, if Overlay has been, will still say complete. You can take the punch. It still bites. You can refuse to sit in the driver's seat. The seat is adjusted for someone taller than the Overlay junior who paints.

### `room_bus_office` — Loop Office

**inspect:** A timetable never updated. Coffee rings on the hour that did not come.

**description:**
A kiosk at the circle's edge. Timetable under glass, last pencil 05:40, a coffee ring on 06:00. A lost-property tin with a glove that is not Overlay's and not Ivy's. You can take the tin. You can leave the timetable. Overlay will not replace it; they replace names, not hours. You can steal the glass. The pencil will weather.

### `room_bus_stencil` — Stencil Wall

**inspect:** Paint pot. EVACUATION COMPLETE in a font that was never a driver's.

**description:**
The lead bus's bow, or a practice wall Overlay used first: `EVACUATION COMPLETE`, typeset-looking because it was a stencil, not a hand. Paint pot, still wet if they are mid-job, skinned if they are not. You can paint. You can scrape. You can stencil the street from the suitcase instead. You can steal the pot. Pigment at Km 19 will go short. Dark until lead bus inspected.

---

## 1.10 Lock Gate Four — `loc_lock_gate_four` *(SPINE)*

### `room_lock_towpath` — Towpath

**inspect:** A mooring ring polished by rope. Nomi will notice if it leaves.

**description:**
Stone edge, Drown-water at a height Benno can tell you by month. Mooring ring, iron, the inner face polished by rope. Overlay stakes along the path, or pulled. A life-ring with the municipal name of a lock that still has a name even when Overlay wants `RECLAMATION 4-W`. You can take the ring. Nomi's launch will have to use a bollard. You can take the life-ring. It has never been thrown. You can refuse to call the water a completed reclamation from the path. You cannot see the leaf angle until the next room.

### `room_lock_control` — Control House

**inspect:** Isolation wheel painted, stuck. The house stopped when the gate did.

**description:**
A brick house, windows salt-white. Isolation wheel, painted red then white then red, frozen mid-throw. A fuse-box with one empty socket — Benno's quest. Tannoy dead. A duty slate last written Exchange+3W, `MID-CYCLE — POWER`. You can throw your weight on the wheel. It will not complete the cycle. You can steal a paint flake. You can refuse to treat the wheel as Overlay's COMPLETE. The house smells of kettle and wet wool if Benno is alive in it.

### `room_lock_benno` — Benno's Bunk

**inspect:** A kettle. A pencil chart of rise. A man who sleeps in a failure.

**description:**
A bunk built into the control house, not a municipal fitting. Kettle on a spirit stove. On the wall, graph paper: water-rise, metres, dates, five years, no title, no COMPLETE. A pair of boots, lock-issue, resoled with tyre. You can take the kettle. He will still have the chart. You can copy the chart. Ira's visited column can take a tick if you bring it and you stood here. You can steal the boots. He has a second pair that are worse. You can refuse to wake him if he is asleep. The gauges tick louder than his breathing.

### `room_lock_gauges` — Gauge Room

**inspect:** Mid-cycle. The needle is a fact. The plate downstairs has not been informed.

**description:**
Gauges, analogue, a mid-cycle mark painted by the last electrician, which was Benno or the man before. The needle sits on the paint. Glass cracked, reading still true. A logbook of readings in the same hand as the wall chart, more numbers. You can tap the glass. The needle does not perform. You can take the logbook. You can refuse Overlay's request to photograph the gauge with the COMPLETE plate in frame. Adjacent leaf unlocks from here.

### `room_lock_leaf` — Gate Leaf

**inspect:** Open, exactly as far as it opened. A wrench frozen to a bolt.

**description:**
The gate itself, steel, jammed at the angle of the power cut. Walkway with a missing railing. A wrench frozen to a bolt, handle toward the water. You can try the wrench. Warmth and time. It may come. The gate will not. You can leave it. You can refuse to walk the leaf in wind. The Drown is not a bomb crater. This room is why.

### `room_lock_reclaim_plate` — Reclamation Plate

**inspect:** COMPLETE. The water has not read the stamp.

**description:**
Overlay plate on the landward face of the leaf, large, `RECLAMATION 4-W CONTROL / COMPLETE`. Four screws, all brass, all matching — they did this job carefully. A packing receipt dated this window or last. You can scrape. You can leave it. You can palimpsest: paint `OPEN` under COMPLETE in a smaller hand. You can steal the plate as brass. The leaf will still be mid-cycle. Dark until gauges inspected; Benno will not let you number a thing you have not measured.

---

## 1.11 Pump Station Nine — `loc_pump_station_nine`

### `room_pump_approach` — Boat Approach

**inspect:** A bilge pole with marks that are Benno's grammar, not Overlay's.

**description:**
Water to the lintel. A boat-hook scratch on brick. Bilge pole standing in a drum, notches for depth, the same pencil logic as the lock chart. You can take the pole. The next sounding will be guessed. You can refuse to step off until Nomi's etiquette is done, if she brought you. Overlay tags start at the inner door, fluorescent, `CONDEMNED`.

### `room_pump_hall` — Pump Hall

**inspect:** Six pumps. Water. A name scratched on a housing that is not a cadastral number.

**description:**
A nave of pumps, most drowned. One name scratched on a housing, a lock-electrician or a Rebuilder, lived. Overlay fluorescent tags on the drowned five. Walkways with missing mesh. You can copy the scratched name. Quil would say it aloud. You can refuse to energise anything from here; the dry motor is another room. You can steal a tag. The Record will be missing a condemnation.

### `room_pump_dry_motor` — Dry Motor

**inspect:** Collapsed floor, accident, one motor in air. A belt that is still a belt.

**description:**
A collapse made a dry island. One motor, rusted but not drowned, belt cracked, a spare belt on a peg like a ritual. Switchgear within reach if you step the gap. You can fit the spare. You can take the spare and leave the motor a sculpture. You can refuse Rebuilders who want this more than Overlay wants the tag. Energising is the branch. Fume, health, a measurable drop elsewhere if the bible's pumps hook exists; if not, a mutation and a travelHours delta. Not a minigame of spinning 3D rotors.

### `room_pump_switchboard` — Switchboard

**inspect:** Fuses. An Overlay tag through the handle, string-tied like Quil's crate.

**description:**
A board that still has labels in a municipal hand. Main handle, Overlay tag through it, string, `NOT TO BE ENERGISED`. You can cut the string. You can leave it. You can steal fuses for Benno's empty socket. You can refuse to be the person who makes the Drown tidier on paper. Dark until dry motor seen.

### `room_pump_condemned` — Condemned Cage

**inspect:** A bundle of tags for pumps that have not been looked at, only numbered.

**description:**
A mesh cage of spare tags, a stamp `CONDEMNED`, a pad still wet. Overlay efficiency: tag first, wade later. You can stamp a tag for the dry motor without seeing it — if you do this before the motor room, the motor room stays dark. That is the trap. You can dump the bundle in the water. You can steal the stamp. The next condemnation will be handwriting.

---

## 1.12 Allocation 12-B — `loc_alloc_12b` *(SPINE)*

### `room_12b_stair` — Stair

**inspect:** Fourteen chalk marks, a gap, then six. Overlay would like the gap closed. It is not a filing error.

**description:**
Subway maintenance stair, stencil `ALLOCATION 12-B` faded. Chalk: fourteen marks, a gap, six. The gap is the story. A tin of chalk on the step, used. You can close the gap. Sela, if present, will leave the stair. Nila will hear if you treat overflow as a complete number. You can add a mark that is not a person. You can refuse and copy the gap for Sole. You can steal the chalk. The next count will be charcoal.

### `room_12b_unprovisioned` — Unprovisioned Hall

**inspect:** Bolt-holes where bunks would have been. A form's leftover, not a shelter.

**description:**
A hall Continuity numbered and did not stock. Bolt-holes in a grid for eleven bunks that never arrived, or arrived at Allocation 12 instead. A crate stencil `NOT FOR GENERAL ISSUE` with no crate. Dust. You can count the holes. You can refuse to call this provisioned. Overlay's refresh stencil in the next room would like you to. You can steal a loose bolt. It is not brass. Frayne will not want it.

### `room_12b_kit` — Halvard's Kit

**inspect:** Improvised potable. The diagrams do not get smaller. If Duty Roster left it, the water still works.

**description:**
A maintenance alcove. Filter jury-rig, hose, charcoal, a notebook whose handwriting gets smaller toward the end and whose diagrams do not. Sela will stay if you call it engineering. She will leave if you call it salvage. You can copy the notes (`item_halvard_kit_notes` if Holdfast minted). You can take the working kit. The water room dies. You can refuse to move a clamp. Overlay will stencil around it as if it were Continuity issue. It is not.

### `room_12b_water` — Water That Works

**inspect:** A cup chained to a pipe. Potable because a man stayed. Not because a number was assigned.

**description:**
The pipe Halvard made honest. Cup on a chain, municipal, the chain newer than the cup. Water that does not taste like the Drown if the kit is present. If the kit is gone, the cup is still there and the water is a decision. You can drink. You can fill a bottle. You can refuse to list this as Overlay `ALLOC-12B / WATER POINT` because that is how a levy finds a hole. You can steal the cup. The chain will hang.

### `room_12b_stencil` — Stencil

**inspect:** Original faded 12-B. Overlay refresh paint. A finished overflow is a labour address.

**description:**
The wall Continuity stencilled. Overlay pot, brighter paint, the same letters, the implied completeness of a refresh. You can refresh. 12-C can cite an address. You can scrape both layers. Blank Rows will not thank you; they will remain possible. You can palimpsest the gap from the stair onto this wall. You can steal the pot. Km 19 pigment goes short again. Dark until stair inspected.

---

## 1.13 Records Annex — `loc_records_annex`

### `room_annex_window` — Window Entry

**inspect:** Second storey. Boat-hook scars. Dry above the waterline.

**description:**
A window that is a door. Sill worn by keels and knees. Hook scars in the plaster. Inside, a mat that was a curtain. You can steal the curtain-mat. The next arrival will wet the dusted room. You can refuse to board anyone else's hull in sight of this window. Nomi's etiquette holds. Overlay crates do not get a second explanation.

### `room_annex_dusted` — Dusted Room

**inspect:** A cloth still damp. Heat. Someone has been keeping paper alive.

**description:**
Warm. Dusted. A cloth in a basin, wrung, not dry. Shelves of fragments the Vault will want. A heater on plant logic that is not District 8 steam — a different mercy, local. You can take the cloth. The next week the dust will be a sentence. You can steal a fragment. Quil will know the gap. You can refuse to wipe Overlay's crate. They left it undusted on purpose.

### `room_annex_name_desk` — Name Desk

**inspect:** Two witnesses. Say the site. A plate is a copy that does not have a mouth.

**description:**
Quil Esser's desk. Witness ledger, the Archivists' rule applied to ground: a site enters the second copy if spoken and corroborated. A pencil. A cup. You can say Lock Gate Four, or 12-B, or Kilometre 19. You can refuse and file a plate. She will not cut the crate string. You can steal the ledger. Completeness elsewhere will suffer. You can lie a site you have not stood in. The rule will still be the rule when Sole looks.

### `room_annex_refused_crate` — Refused Crate

**inspect:** GROUND COPY. String still tied. Quil has not cut it.

**description:**
On the landing, Overlay crate, `GROUND COPY`, string, a seal of wax that is just candle. Dust on the top, none on the sides — it was moved. You can cut the string. You can dump it in the Drown. You can carry it to the Vault airlock as hats of brass. You can refuse. The crate is a room that stays a problem. Dark until the name desk has been used or refused.

---

## 1.14 Memory Vault — `location_the_memory_vault` *(SPINE)*

*Recast parent: not a social-media farm. Dry stacks. A cage for a second copy of history that only exists if someone walked.*

### `room_vault_dock` — Dock

**inspect:** Mooring. Etiquette. If Pump Nine lives, the water is a step lower and the scum line tells you.

**description:**
A dock that was a loading bay. Mooring, a scum line on the wall. If the dry motor was energised, the line is a handspan above the water and there is a wet band the colour of old tea. If not, you step down. Nomi will not board another hull. Overlay plates in the next room do not get to pick a side on the dock. You can steal a mooring wedge. You can refuse to help Overlay unload. You can measure the scum with Benno's grammar.

### `room_vault_airlock` — Airlock

**inspect:** Overlay plates stacked like hats. Guests who have not been invited to speak.

**description:**
Decon that is a grate and a rag, cousin to your hatch, not a retune of its numbers. Against the inner wall: Overlay plates, stacked, tissue, waiting. Maren will stand here if access held. You can admit the plates. You can leave them as hats. You can steal tissue for wounds. You can refuse to let a plate through without a spoken name. The inner stacks stay dark until the airlock is resolved.

### `room_vault_stacks` — Dry Stacks

**inspect:** Fragments. The paperwork survives. No off-world thesis. No Protocol Zero.

**description:**
Racked paper, dry, the reason this hole was worth a boat. Schedule fragments, letters, a scoring rubric that matches the Ministry poster. Nine people, or fewer if years have taken them. No servers humming a social network. A cart with a squeak Quil oils and Sole does not. You can take a fragment you already have as Codex. You can refuse to invent a machine that explains the war. You can steal the oil can. The squeak will return.

### `room_vault_sole_table` — Sole's Table

**inspect:** A blotter. She will file. She will not sign a ship, a hatch, or a plate that was not said.

**description:**
Margit Sole's table. Blotter, a tray of fragments, 12-C unsigned if you carried it (*Holdfast*). She will not enter a site on one testimony. She will not raise her voice. A cup of water, replaced. You can say the names of rooms you stood in. You can refuse. You can show a photocopy of Ira's book. She will look at the visited column and wait. You can steal a paperclip. She will use a pin.

### `room_vault_second_copy` — Second Copy Cage

**inspect:** Empty until the route is walked. Inventory does not fill it.

**description:**
A wire cage, a desk inside, a book-size space. If you skipped Lock, 12-B, or the Ministry registrar, the space is empty and a card says `NOT YET A COPY`. If you walked them, the cage holds whatever you chose to file: Record, lived names, both, nothing. You can put Ira's book here. You can put Benno's chart. You can put nothing and lock it. You can steal the padlock. The cage will still be a cage. This room does not light from a bunker menu.

### `room_vault_standing_book` — The Book's Place

**inspect:** A space the size of Ira's Record. The gazetteer the save will keep.

**description:**
A lectern, empty, dusted in a rectangle. Ending files here as well as in the cage: a copy Sole will let the Codex treat as history, with `discovery_location_id` bound to this room and to the sites mutated. You can leave the lectern empty. Unnumbered. You can set down a palimpsest that nobody likes. You can refuse to let Maren stand at it. The game will not tell you which is kinder. It will recast the posts.

---

# 2. Location lore strata

Each featured site: **pre-Exchange / after / now**. `now` is selected by mutation. Codex shows pre/after only after standing in at least one room.

### Kilometre 19
**Pre:** A kilometre post on a shipping cut, numbered for dredgers, not lamps.
**After:** Lamplighters took the post. Ivy's ledger stops. The Cut begins to pretend it is a road.
**Now (plated):** Brass over stencil. Clerks can find a lamp without asking a Lamplighter.
**Now (scraped):** Stencil only. Overlay crate empty or stolen.
**Now (palimpsest):** Both readable. Ostrowski's sheet has two columns.

### Transit Authority
**Pre:** Timetables that corresponded to buses.
**After:** Grease pencil became the Quiet Evacuation. Convoy 12 held. Unlisted turned around.
**Now:** Maps are a hand, a font, or both. The telephone stays cut.

### Municipal Archive
**Pre:** Civil records. Not Continuity.
**After:** Fire suppression. Garrison searches. `NOT MUNICIPAL`.
**Now:** A field index in a cage, or a plate in geology, or both sinking.

### Ministry
**Pre:** Office of Continuity. Proud of being boring. Standing Record scoped, plates ordered.
**After:** Senior grades left. Ira stayed. Household Schedule went to whoever could carry it. Record stayed because it was heavier and less famous.
**Now:** Recast from Protocol Zero. A book with an empty visited column, or a receipt where the book was.

### Weighbridge
**Pre:** Axle weights. Municipal serial.
**After:** Favours became mass. The Tollman's first office.
**Now:** Needle, lots, or two columns that disagree. Edor's paper may sleep in the whites.

### Grange Hall
**Pre:** Agricultural society. Oil lamps.
**After:** First council. 47 names. Crosses later.
**Now:** Ledger with or without a cadastral column. Kitchen notice face-down or on the table.

### Allotments
**Pre:** Municipal plots, waiting list, floodplain.
**After:** Works. Brass order. Seventy days from thirst.
**Now:** Sleeve current or `HISTORICAL`. Hut warm or levy-cold. Bin mixed.

### Bridge Seven
**Pre:** Four lanes, demolition points designed in.
**After:** Tollman. Charges visible. Authority as untested fact.
**Now:** Listed as friendly, scraped, or disturbed. Dust on the housing is the tell.

### Bus Reversal Loop
**Pre:** A turning circle at the edge of an evacuation route.
**After:** Forty-one buses obeyed. Unlisted sent home.
**Now:** COMPLETE stencil, street name, or scraped metal. Logbook vs bow.

### Lock Gate Four
**Pre:** A lock that kept a river off land for ninety years.
**After:** Power lost mid-cycle. Drown begins. Benno stays.
**Now:** COMPLETE lie, gauges filed, or plate down. The leaf does not move.

### Pump Station Nine
**Pre:** Six pumps. Drainage as policy.
**After:** Under the water they were built to move.
**Now:** One motor live, all tagged, or hall darker.

### Allocation 12-B
**Pre:** A fallback line on a form.
**After:** Convoy 12. Chalk. Halvard. Two years of water.
**Now:** Kit present or gone. Stencil original, refreshed, or scraped. Gap held or filled.

### Records Annex
**Pre:** A second-storey office.
**After:** Boat door. Archivists. Dusting.
**Now:** Crate tied, cut, or in the Drown. Names said or not.

### Memory Vault
**Pre:** Continuity dry storage. Not a social network.
**After:** Sole. Fragments. Corroboration.
**Now:** Cage filled with a gazetteer, or empty, or holding two things that argue.

---

# 3. Encounter scene prose

Keyed to rooms. Aftermath changes the room. Not generic combat. `[VO]` text-first.

---

### `enc_site_plate_screwer` — `room_km19_post`

Maren Holt, or a junior with the same screwdriver. They do not block the lamp. They do not ask permission. The fourth screw is already steel.

> "The stencil stays. The plate goes on. If you take the plate, log it. If you don't log it, the next post will be numbered twice and this one will be a rumour."

**If you wait:** they finish the fourth screw. Post recast: brass-on.
**If you take the plate mid-job:** they do not chase. Crate later shows 19 missing. Overlay access ticks down one.
**If you help with a matching brass screw:** they note `field assistance`. Ivy still will not cross.

---

### `enc_site_ivy_oil` — `room_km19_oil_tin`

You take oil, or pigment-with-oil, or both. No Lamplighter is here. The rule still is.

Later, if Ivy is asked: she will not make an exception. She will confirm the post exists. The lighting that was due will be short. Eleven days is the cousin, not a new number.

Room recast: tin empty, rag in the snow, lamp still Ivy's.

---

### `enc_site_maren_bench` — `room_transit_overlay_bench`

Solvent cap off. Printed Convoy 12 without HELD.

> "The hand under the glass is a clerk's. The clerk is not coming back. The slot still has to be readable from the floor."

Install: maps recast, grease paler. Crate: bench empty, glass unchanged. Solvent test: a smear like weather; palimpsest flag. Steal solvent: next Overlay job at the loop is dry-crooked.

---

### `enc_site_dob_ring` — `room_transit_dob_desk`

Sitting is optional. The blotter is not. If you joke about the doubled year, Edor (if Holdfast live) will hear it later and the joke will be in his return as `irregular tone`. The telephone will not ring. `[VO]` optional: a dry click when you lift the handset, then nothing.

Room recast: chair moved; cord ends still on the blotter.

---

### `enc_site_brick_collapse` — `room_archive_grey_brick`

Digging is hours. The mass sounds like packed ash when struck. A stack rail may go. Health if it does. You free the plate, or the index, or neither.

If an Overlay junior is waist-deep: they will take a rope. They will not leave the plate. Afterward the brick shows a hole the size of a ledger, or a plate deeper than when you arrived.

---

### `enc_site_ira_bell` — `room_ministry_enquiry`

The bell rings. If Ira is in the building and the book is too, she comes. If Overlay took the book on a receipt, she comes anyway and the registrar room is a lighter square of dust.

> "If you are here about eligibility, the memo is on the sleeve. If you are here about ground, that is a different book. It is not this desk."

Steal the bell: later knocks. She will answer knocks slower.

---

### `enc_site_rubric` — `room_ministry_scoring`

No encounter person required. The poster is the encounter. Taking the clerk-line scrap leaves a rectangle of cleaner paper. Ira will put a blank strip there and write 9 again.

---

### `enc_site_needle_joke` — `room_weigh_plate`

Osric does not come out for the joke. He writes what the needle says. An Overlay plate on the scale is a mass. A person is a mass. A favour is a mass.

Receipt afterward: `PLATE / kg` or `PERSON / kg`. Room recast: Overlay lots on the hook if the calibration weight left.

---

### `enc_site_edor_stool` — weigh hut *(Holdfast flag)*

Edor Vale, if the clerk quest started, may be here instead of your apron. He has occupations. Osric has kilograms. Maren has lots. Three documents. None of them will convert for the others.

> "Most people want it read again. That's all right. There isn't a time limit on understanding it. There is a time limit on the ice, and this is not the ice."

He offers to read the form twice. He will not step on the scale unless you ask, and if you ask he will, and Osric will write him down.

---

### `enc_site_ledger_cross` — `room_grange_ledger`

A chair, or Delacroix if the week is a council week. Overlay column ruled or filled. Filling it during a Lasko vote is how a person becomes a lot in front of hands.

If you erase: graphite dust on the crosses. The crosses stay. Room recast: column gone, or present, or present and unused.

---

### `enc_site_dara_dark` — allot hut *(levy absence)*

Stove cold. Cup. Overlay stakes already in the window-grid. No Dara. A note: `WATERED — NUMBERS`. The noticeboard sleeve may already say HISTORICAL in a stamp that is still tacky.

If levy not honoured and Dara is here, this encounter does not fire. She offers tea that tastes of iodine if the membrane was stripped.

---

### `enc_site_brass_mix` — `room_allot_brass_bin`

Sorting is a quiet job. Handles, lamp base, filed nameplate, Overlay plate. Frayne will take eight fittings and not ask. Maren will take plates and not ask. You can give each what they want from the same bin. You can give both the same piece. The bin does not catch fire. It gets lighter.

---

### `enc_site_span_look` — `room_bridge_charges`

Looking: dust on the housing, tape, gorge. No check. Clipboard later can tick VISIBLE.

A gust. The catwalk speaks. You can go back to the span without opening anything. Room stays undisturbed. That is a valid aftermath.

---

### `enc_site_detonator` — charges, check branch

Meter, or a tap, or oil on the lock. The Tollman is not here. The authority is. Aftermath is access: a listed span, a closed span, a price that is no longer a joke. Not a cutscene of fire. A receipt at the near bank that says `SPAN UNCERTAIN`.

---

### `enc_site_benno_kettle` — `room_lock_benno`

If you take the kettle, Benno will still talk. He will talk about the chart. The bunk recasts: a ring on the stove where the kettle was, a second tin cup that was the spare.

> "They can screw COMPLETE on the leaf. The leaf is still where it stopped. I sleep in the house that stopped with it."

If you copy the chart without asking: he nods. He has been waiting for someone to treat metres as a document.

---

### `enc_site_complete_crew` — `room_lock_reclaim_plate`

Overlay install, four matching screws. They want a photograph with the gauges. Benno will not pose. If you hold the plate while they screw, access ticks up. If you unscrew as they pack: they log `field reversal` and do not fight. The leaf does not move.

---

### `enc_site_dry_motor` — `room_pump_dry_motor`

Rebuilders, if hegemony allows a face, want the belt on. Overlay wants the tag on the handle. Energise: fume, a cough, a drop you will see later as a scum line at the Vault. Fail: belt snaps, spare gone. Refuse: motor remains a sculpture. Room recast accordingly. No spinning-rotor minigame.

---

### `enc_site_chalk_gap` — `room_12b_stair`

Closing the gap is a stroke. If Sela is present she goes back to the boat, or to the unprovisioned hall, and does not speak in the stencil room. If you copy the gap onto paper, Sole can be shown a shape that is not a number. Overlay junior will offer to "tidy." Tidying is filling.

---

### `enc_site_quil_rule` — `room_annex_name_desk`

Filing a plate without speaking:

> "If you don't say it, you're only copying. A plate is a copy that does not have a mouth."

String on the crate stays. Access to the Vault stacks still possible; the cage will not take the plate as a name. Say the site: she writes it, asks who else stood there. One testimony is not enough. Benno, Ira, you — combinations.

---

### `enc_site_sole_aloud` — `room_vault_sole_table`

She waits. You say rooms. She does not nod as if you had done a kindness. She writes. If you whisper, she asks you to say it again. If you refuse, the cage stays a card `NOT YET A COPY` even if the book is in your pack.

12-C on the table, if carried: she files, does not sign. Mire's pad still will not light. Different annex.

---

### `enc_site_watch_night` — repeatable, any featured parent

A survivor assigned. Utility AI: stoke if there is a stove, not fight. Encounter check: Overlay junior, a Garrison address-taker if plates stand, or nothing but wind. Aftermath: room inspect gains `occupied` — a cup, a notch on a filter, a name on a slate that is not Kess's chart.

---

### `enc_site_garrison_address` — conscription overlay

Pell, honest. Overlay index in his other hand, or not.

> He will hit his numbers without lying. Addresses make the numbers easier. He will say so.

If plates stand, intercept risk up on the route to those parents. If plates scraped, he has people again, not places. The ticket machine still works.

---

# 4. Main quest stage texts

Objectives are spatial. Menu photocopies do not advance visited.

---

## `quest_record_the_plate` — The Plate on the Last Lamp

**Stage 0 — offer (Ostrowski or Ivy or a rumour of brass on a lamp)**
Ostrowski will sell a sheet that has two names for one post, or he will not, and you will hear it from a Lamplighter who will not cross. Either way the post is not in your bunker.

**Stage 1 — arrive**
Kilometre 19. The lamp is lit or it is not, on Ivy's schedule. The plate is a fact on the stencil.

**Stage 2 — post**
Inspect. Four screws, one steel. Spirit-level. Take / leave / palimpsest.

**Stage 3 — oil**
Wick tin. Pigment pot. Oil that smells like a lighting.

**Stage 4 — crate (unlock)**
Spares CUT-19 through CUT-24. Screwdriver. Packing list with oil blank.

**Stage 5 — Maren**
If present: the snippet about ground not arguing. If crate only: a note `FIELD — RETURNING`. Companion unlock if you did not steal the whole stack.

**Complete:** mutation on the post. Codex `lore_sr_seam`.
**Refuse all objects:** post still plated if Maren finished; you have stood there; visited can tick.

**Choice bodies**
`sr_km19_take_plate` — The stencil is colder.
`sr_km19_leave` — Brass and orange.
`sr_km19_oil_north` — Yara's hour.
`sr_km19_oil_ivy` — The lighting that was due.

---

## `quest_record_grease_pencil` — Under Glass

**Stage 0**
Maren: the map has to be readable from the floor. Ira, if already met: Transit is a site in the book without a tick.

**Stage 1 — lobby**
Blank disc jammed. Clock stopped. Sign: LISTEN FOR YOUR SLOT.

**Stage 2 — map glass**
HELD — DOB QUERY. Pencil on a string. Trestle of print.

**Stage 3 — DOB desk**
Cord cut. Blotter. Stamp FOLLOWING PROCEDURE, dry.

**Stage 4 — radio gallery (unlock)**
Turn-back flimsy. Headset. Dead air.

**Stage 5 — bench**
Install / crate / solvent / steal.

**Complete:** `mutation_transit_maps`. `lore_grid_convoy_slots` if not already, because you stood here.
**Photocopy later in bunker:** does not tick Ira's visited.

**Choice bodies**
`sr_transit_keep_hand` — Grease.
`sr_transit_print` — Font without HELD.
`sr_transit_both` — Weather on the glass.

---

## `quest_record_wrong_stacks` — Grey Brick

**Stage 0**
Garrison searched twice. Overlay is filing anyway.

**Stage 1 — vestibule**
Visitor book. Third line unfinished: FILE RECORD.

**Stage 2 — brick or dock**
Dig / walk around. Collapse if you dig badly.

**Stage 3 — cage or empty**
Field index, not Schedule. Rubber thimble.

**Stage 4 — loading dock**
RECORD / NOT SCHEDULE in two hands. Brass buckle.

**Complete:** `mutation_archive_dug` or `_sunk`. `item_sr_field_index` or not.
**Pell hears "book of addresses":** intercept flavour, not a new Power.

---

## `quest_record_the_book` — The Visited Column

**Stage 0**
The Ministry is not humming propaganda. It is cream paint and a memo.

**Stage 1 — stair**
Authenticator on a replaced fuse. Handrail hanging.

**Stage 2 — enquiry**
Bell. Memo. Forms with *shelter number*.

**Stage 3 — scoring**
31. 28. 9. STILL HERE in the margin.

**Stage 4 — registrar (required)**
Ira. The book. Empty visited. Copy / take with receipt / Overlay / refuse.

**Stage 5 — annex / phone (optional same expedition)**
D/9 binder. Convoy hold blotter.

**Complete:** `mutation_ministry_recast`. Parent description loses Protocol Zero. `item_sr_record_copy` if copied. **Visited column does not fill from the copy.**

**Ira, if you show her the copy later without new rooms:**
> "You have the list of sites. That is not the same as you having been there."

---

## `quest_record_mass_or_lot` — The Needle

**Stage 0**
Osric does not attend ceasefires. He attends the needle.

**Stage 1 — plate**
Stand, or put brass on, or take 500 kg.

**Stage 2 — hut**
Spring-balance. Tea. FAVOURS ARE MASS, underlined once.

**Stage 3 — receipts**
Whites. Edor's paper if the flag says so.

**Stage 4 — lot stack (unlock)**
Install on glass / crate / steal as fittings.

**Complete:** `mutation_weigh_lots` or `_mass_only`. Grain Exchange second paragraph still wants this parent.

**Osric if lots installed:**
> "Put the plate on the scale if you want. It will read as mass. That is the only number I will write."

---

## `quest_record_hands` — Plot 114

**Stage 0**
Two parents. Skipping the Grange makes the Allotments a bin without a vote.

**Stage 1 — porch**
Sign. Tagged rifle. Hands later if you walk in armed.

**Stage 2 — table / ledger**
47, 12, 22 crosses. Overlay column.

**Stage 3 — kitchen**
Notice face-down.

**Stage 4 — allotments gate**
Cut, rewired with bell-wire.

**Stage 5 — hut**
Dara or cold stove. Autoclave key.

**Stage 6 — board / plot / bin**
Sleeve. Two 114s. Mixed brass.

**Complete:** `mutation_verge_names`. `mark_sr_waitlist`. Frayne minutes record mass of fittings, not origin.

**Dara, if present:**
> "Plot 114 is still 114. The plate they want to put on it has a different 114. I water this one."

**Dara, if absent:** the note WATERED — NUMBERS. Overlay has been.

---

## `quest_record_friendly_obstacle` — Listed Charges

**Stage 0**
Ira's annex and this underside are one sentence. Dead Hand is not this pack.

**Stage 1 — near bank**
Receipt spike. D/9 stone. Do not move it unless you mean the annex.

**Stage 2 — span**
Scratch at height. Flags.

**Stage 3 — underside**
Look / refuse / check.

**Stage 4 — clipboard**
VISIBLE ticked. DETONATOR VERIFIED blank unless you lied or checked.

**Complete:** `mutation_bridge_listed` or `_disturbed`. Yara, if asked about blasting: she will not. This is not her ice.

---

## `quest_record_the_failure` — As Far As It Opened

**Stage 0**
Access: Shallows, pumps, or a Cut that is lit. Ice Road dark changes the walk, not the leaf.

**Stage 1 — towpath**
Mooring ring. Life-ring. Overlay stakes.

**Stage 2 — control**
Wheel stuck. Fuse empty. Slate MID-CYCLE — POWER.

**Stage 3 — Benno**
Kettle. Chart. Companion if you copy metres.

**Stage 4 — gauges / leaf**
Needle on paint. Wrench frozen.

**Stage 5 — COMPLETE**
Leave / scrape / OPEN under / steal brass.

**Complete:** lock mutation. `lore_drown_the_failure` if not yet. Benno's chart as item.

**Cannot complete by reading the gazetteer one-liner.** The one-liner is twelve words. This is six rooms.

---

## `quest_record_fallback` — Fourteen, a Gap, Six

**Stage 0**
Sela's kit language if she is here. Overlay refresh paint if she is not.

**Stage 1 — stair**
Chalk. Gap. Tin.

**Stage 2 — hall**
Bolt-holes. No bunks.

**Stage 3 — kit / water**
Engineering or salvage. Cup on a chain.

**Stage 4 — stencil**
Refresh / scrape / palimpsest the gap.

**Stage 5 — optional Pump Nine**
Approach, hall, dry motor vs condemned cage. Tag-first makes the motor room stay dark.

**Complete:** `mutation_12b_address` and/or kit flags and/or pump flags. 12-C can cite an address only if refreshed.

---

## `quest_record_which_gazetteer` — The Second Copy

**Gate:** Ministry registrar stood in, Lock stood in, 12-B stood in. Else cage card: NOT YET A COPY.

**Stage 1 — annex window**
Etiquette. Mat.

**Stage 2 — dusted / desk**
Say sites. Corroborate. Crate string.

**Stage 3 — Vault dock**
Scum line as pump evidence.

**Stage 4 — airlock**
Hats of brass. Admit / refuse.

**Stage 5 — stacks / Sole**
No social media. Say again if whispered.

**Stage 6 — cage / lectern**
File Record / lived / both / nothing.

**Complete:** ending flag. Second paragraphs at **each mutated featured parent**, `discovery_trigger: location_explore`. Hatch reversed (*Holdfast*) and roster ink (*Duty Roster*) read this.

**Fail by menu:** carrying all items home and opening Codex. Cage empty. Ira's column empty of your ticks.

---

# 5. Side quest stage texts

Shorter. Still a place.

### `quest_sr_paint_short`
Maren needs lampblack and oil. Ivy's reservoir. Yara's stick. Home lamp. Do not ask the exception. Deliver: stencil rooms wet. Refuse: plates unreadable at distance. Mutation: pigment pots empty at Km 19 and the loop.

### `quest_sr_plate_brass`
Bin behind Frayne's hut. Sort plates from handles. Eight fittings. One proud stamp. Deliver Overlay / Works / tin behind your filter. Silence in all three offices.

### `quest_sr_maren_sheet`
Ostrowski's waxed paper. Maren's cadastral. Stand at one featured parent with both. They will not shake hands. Travel-time hint. His will_not still holds: he does not name buyers.

### `quest_sr_overlay_withdraw`
Scrape three plates, write nothing. Overlay rooms go dark of labour. Dara's hut, if she was a junior watch for them, empties even if levy did not take her. Maren: "Ground does not argue. I can stop walking it."

### `quest_sr_ira_column`
Return after three featured parents. She ticks visited only for rooms you name that match her sites. Photocopy ignored. Reward: honest book.

### `quest_sr_benno_fuse`
Empty socket. Fuse from `loc_substation_yard` overlay one-liner, or Overlay crate. Fit: gauges brighter, leaf still mid-cycle. COMPLETE still a lie if the plate is up.

### `quest_sr_quil_dust`
Rota. Crate undusted on purpose. Dust it and she will still not cut string. Refuse and the dust is a sentence on Overlay. Say one site while dusting.

### `quest_sr_osric_weight`
500 kg missing. Return it, or hang an Overlay plate, or leave the needle wrong. If plate-as-weight: all receipts off by a plate. He will write it. He will not forgive it.

### `quest_sr_lasko_number`
If Lasko's vote is pending, Overlay notice would list him as a lot. Attend. Do not let the notice speak. Hands. He does not beg.

### `quest_sr_pell_sites`
Conscription office overlay. Ticket machine. Addresses in his other hand if plates stand. Hear both. Do not merge quota with cadastral. He answers what happens to decliners. That is still the encounter.

### `quest_sr_sent_back_paint`
Loop. Log vs stencil. Paint / scrape / street from the suitcase. Room recast.

### `quest_sr_terrace_line`
`loc_terrace_pumphouse` one-liner overlay. Lot map on the loophole wall. Inspect. Leave or scrape. Harvest access sentence changes. No full layout.

### `quest_sr_nomi_plates`
Shallows. Overlay cargo that picks a side. She explains hull etiquette once. If you board with plates after being told: she is not there again. Drown closes a door, not a Power war.

### `quest_sr_kittiwake_name`
`loc_bathymetric_boat` overlay. Overlay soundings vs eleven days of flooding in a log. Do not "fix" the log. Drown nav hint. Ostrowski would sell this and call it things, not names.

### `quest_sr_nila_number`
Overflow 11. Warn Nila that Overlay would plate ALLOC-11. If plated anyway: hatch looks like a hatch. It will not open. Blank Rows access gone. Do not run The Knock.

### `quest_sr_kess_refuse`
Roster wall. Maren asks Kess to copy site numbers onto people. Kess:

> "The wall was left blank because the names it wanted did not arrive. Sites are not a morning row. I will not write CUT-19 as a person."

Player may still write. Irregular. Edor may copy it. Nila may hear.

### `quest_rep_site_watch`
Assign one survivor eight hours at a featured parent. Cup, notch, slate. Encounter check. Repeatable. Roster labour (*Duty Roster*).

### `quest_rep_plate_audit`
After palimpsest: walk three plated sites. Note which layer is winning. Memory tick. No gold. Ira likes this more than Overlay does.

---

# 6. NPC voice bibles (site-keepers)

Speakable. Text-first. Do / Don't. Barks. Will not.

---

## 6.1 `npc_maren_holt` — Maren Holt

**Where:** Km 19 crate, then Transit bench, then unfinished posts.
**Was:** Municipal cadastral technician. Unlisted. In-situ without a plant order.
**Will not:** Falsify a gauge. Number Alloc 11 if she knows Nila's rule. Shake Ostrowski's hand.

### Do / Don't

**Do:** Distances. Lot numbers. "Lived name is a subtitle." Log oil even when she still screws the plate. Call the Record a job.
**Don't:** Please. Poetry about memory. Call Benno a squatter. Call Ira allocated. Raise her voice. Joke DOB.

### Barks

**First (Km 19)**
"The Schedule named households. This names ground. Ground does not argue. People do. I am not here to argue. I am here to finish the post."

**Oil**
"If you take the oil, log it. I will still screw the plate. The lighting is not my ledger. The number is."

**Transit**
"The hand under the glass is a clerk's. The clerk is not coming back. The slot still has to be readable from the floor."

**Ira's book**
"I can carry it. I cannot tick visited for you. That column is walking."

**Lock COMPLETE**
"COMPLETE is a filing. I did not measure the leaf. Benno did. I can still plate what Continuity ordered plated."

**12-B**
"A fallback on a form is still a site. A levy that cannot find a hole is a levy that knocks on the wrong hatch."

**Withdraw**
"Ground does not argue. I can stop walking it."

**Kess**
"I asked her to copy numbers. She said sites are not a morning row. She is not wrong about the chart. She is wrong about the posts."

**Ostrowski**
"He sells where things are. I sell what they are called. We can stand on the same ice and not be in the same document."

**If you steal the crate**
"19 through 24 will be rumours. I will not chase you. I will number what is left."

### Threatening pair (`threateningFactionId` Overlay access low)

**Normal:** "Log it."
**Low:** "If you scrape without a name, the next caretaker will not be me. The post will still be a post."

---

## 6.2 `npc_ira_vell` — Ira Vell

**Where:** Ministry registrar. Rarely enquiry if the bell rings.
**Was:** Filing grade. Stayed.
**Will not:** Fill visited from inventory. Give the book without a receipt. Call the player allocated. Raise her voice.

### Do / Don't

**Do:** Present tense. Noted. Pages. Show the empty column.
**Don't:** Eligibility speeches. Protocol Zero. Comfort. Call Overlay thieves. They have a job. So does she.

### Barks

**First**
"I can copy you the list of sites. That is not the same as you having been there. The book has a column for that. It is empty until the column is not a lie."

**Bell**
"If you are here about eligibility, the memo is on the sleeve. If you are here about ground, that is a different book."

**Scoring**
"Nine points. I am aware. The poster is still up because taking it down would be a feeling."

**Receipt**
"If Overlay takes the book, I write that they took it. If you take it, I write that you took it. The building is not empty of the job."

**Photocopy**
"You have the list of sites. That is not the same as you having been there."

**Three sites later**
"Say which rooms. I will tick what matches. I will not tick the Archive because you wanted it to be the Schedule."

**Vault**
"If Sole files a ground copy, I would like the visited column to agree. If it does not, file that it does not."

**Dead phone**
"Six hours eleven minutes. Following procedure. The procedure outlived the department it was telephoning."

---

## 6.3 `npc_benno_kade` — Benno Kade

**Where:** Lock control house.
**Was:** Lock electrician. Unlisted.
**Will not:** Call the Drown a crater. Pose with COMPLETE. Guide Overlay to Pump Nine if already tagged.

### Do / Don't

**Do:** Water-heights. Dates. Open, as far as it opened. Offer the chart.
**Don't:** Hero. Squatter-as-insult. Bomb. Please.

### Barks

**First**
"They can screw COMPLETE on the leaf. The leaf is still where it stopped. I sleep in the house that stopped with it."

**Gauges**
"The needle is on the paint. I put the paint where the power died. That is the document."

**Kettle taken**
"There is a tin cup. The chart is on the wall. The water is still coming."

**Fuse**
"Brighter gauges. Same leaf. I am not selling you a closed lock."

**Pump Nine**
"If they tagged it without wading, they tagged a rumour. I will not walk a rumour into a live board."

**Nomi**
"She will not board my house. It is not a hull. The etiquette is still the etiquette."

**If COMPLETE stays**
"District 8 can file it. The Drown cannot read."

---

## 6.4 `npc_quil_esser` — Quil Esser

**Where:** Annex, then Vault stacks.
**Was:** Born after. Learned by watching.
**Will not:** File a plate as a name. Skip two-witness. Adjudicate Quiet House. Enter Overlay's crate as a kindness.

### Do / Don't

**Do:** Soft. The rule. Dusting as punctuation. Ask who else stood there.
**Don't:** Sermon. Ancestral spirits as flavour-text dump. Call Maren evil.

### Barks

**First**
"If you don't say it, you're only copying. A plate is a copy that does not have a mouth."

**Dust**
"I dust paper. I do not dust their crate. That is not a slight. It is a rota."

**One witness**
"The rule is the whole point of the rule. Benno is a mouth. Ira is a mouth. A photocopy is not."

**Sole**
"She will ask you to say it again if you whisper. I would too. The stacks are quiet. That is not the same as a whisper being a name."

**Dump crate in Drown**
"Then it is a site in the water. I will not dive for it. I will note that it was refused."

**Kess**
"She writes people. I write what people say about the dead, and now about rooms. We are not the same pencil."

---

## 6.5 `npc_osric_tann` — Osric Tann

**Where:** Weighbridge hut.
**Was:** Municipal scale technician.
**Will not:** Price a favour as a lot. Lie about the needle for Edor or Maren. Attend the Tollman's jokes as if they were still jokes.

### Do / Don't

**Do:** Kilograms. Receipts. Back to the door. Underline once.
**Don't:** Blood or bullets as his line — that is the Tollman. Osric is the needle.

### Barks

**First**
"Put the plate on the scale if you want. It will read as mass. That is the only number I will write."

**Weight stolen**
"The next column will be wrong by five hundred kilograms. The ice did not do that."

**Edor**
"He has occupations. I have a needle. We can share the hut. We cannot share a document."

**Lots on glass**
"The needle still moves. If they want a number that does not weigh, they can write it on the receipt themselves. I will not."

**Calibration plate hung as weight**
"I will write it. I will not convert it back. That is not forgiveness."

---

## 6.6 `npc_dara_mewn` — Dara Mewn

**Where:** Allotments hut at night, if not levied.
**Was:** Waiting-list name, alive.
**Will not:** Sign HISTORICAL. Comment on nameplates. Leave the sleeve in rain.

### Do / Don't

**Do:** Plot numbers as people. Minutes. Iodine if the tap is worse.
**Don't:** Fair. Family. Please.

### Barks

**First**
"Plot 114 is still 114. The plate they want to put on it has a different 114. I water this one."

**Levy going**
"If I am north, they will water with numbers. The plants will not die of that. The list might."

**Brass bin**
"I do not ask where it came from. I ask whether it is a fitting or a name. Today it is both."

**Membrane**
"The tap is weaker. I am not a water engineer. There isn't one. I am a watering can."

**Absent (note)**
`WATERED — NUMBERS`

---

# 7. Ending gazetteer paragraphs

Discoverable **at the place**. `discovery_trigger: location_explore`. Second paragraphs. House voice. The game does not rank them.

---

## `ending_record_stands` — The Record Stands

**Kilometre 19**
The plate is on. The stencil is a subtitle, smaller, still orange if you kneel. Clerks from the Cut, when the ice allows, find the lamp by number. Ivy's lighting is still Ivy's. The oil column in Overlay's packing list is finally filled.

**Lock Gate Four**
COMPLETE faces the towpath. Benno's chart is still on the wall. District 8 files the Drown as a closed reclamation. The needle has not been informed. The leaf has not moved.

**Allocation 12-B**
The stencil is bright. A levy escort can walk to it. The chalk gap is still a gap if you left it; the address is not. The cup on the chain is Overlay-tagged `WATER POINT` or it is gone with the kit.

**Memory Vault**
The lectern holds Ira's book with ticks that match plates. Sole has a ground copy. The airlock is empty of hats. Maren is not waiting. The Codex lists sites the way it lists households: as if the walking were finished.

**Slide**
Posts have numbers. Lived names are subtitles. The levy can walk. The rooms are still rooms.

---

## `ending_record_lived` — The Lived Map

**Kilometre 19**
The plate is in the Annex crate, string-tied, or in your pack as brass. The stencil is the name. Ostrowski's sheet matches the ground. Overlay is not at the post. The pigment pot is dry.

**Lock Gate Four**
The leaf is still mid-cycle. The plate is down. Benno's metres are the document Sole ticked. Nobody in District 8 can file COMPLETE without lying in a way Ira would not sign.

**Allocation 12-B**
Faded original. Gap held. Kit engineering if Sela had her way. Not a labour pin. Blank Rows still have a grammar.

**Memory Vault**
Lived names said aloud. Plates in a crate on the landing. Quil cut nothing. Maren stopped walking. The cage holds a list of what people call the rooms.

**Slide**
The plates are string-tied. Ostrowski's sheet matches. Overlay is not at Kilometre 19.

---

## `ending_record_palimpsest` — Both Hands

**Every featured parent (one-line overlay plus room `now`)**
Two names. Clerks lose hours. Weighbridge columns disagree. Transit glass is weather. Grange ledger has a column nobody uses and will not erase. The Vault cage holds both copies. Nobody likes either. Travel times worse by a fraction that is not a new system, only fatigue.

**Slide**
Every featured site shows two names. The cage holds both. The needle still moves.

---

## `ending_record_scraped` — Unnumbered

**Kilometre 19**
Bare stencil, or bare metal if both layers went. No crate. No Maren. A survey nail still in the seam, tag gone.

**Lock Gate Four**
Gauges. Benno, if alive. No COMPLETE. Sole cannot complete ground. Hatch escort (*Holdfast*) arrives with a list of places from District 8 that do not match the posts. Roster burned (*Duty Roster*) makes the list of people foreign too.

**Vault**
Lectern empty. Cage card: NOT A COPY. Quil dusts around a rectangle.

**Slide**
Sole cannot complete ground. Benno's gauges are the honest document in the Drown. The escort has a list. The posts do not.

---

## Remaining featured sites — ending overlays

Shippable second sentences for parents not fully expanded above. Bind `discovery_location_id` to the parent. One paragraph per ending.

### Transit Authority
**Stands:** Printed plates on the glass. HELD is a rumour under a font. The telephone stays cut.
**Lived:** Grease pencil only. Overlay trestle gone. Convoy 12 still held if you kneel.
**Palimpsest:** Solvent weather. Clerks read from the floor and argue.
**Scraped:** Glass bare of both. The published pre-war timetable is the only layer, and it is wrong.

### Municipal Archive
**Stands:** Field index in the cage, plates in drawers that can hold them. Vestibule third line dated.
**Lived:** Index copied into Ira by walking, not filing. Brick undisturbed except the hole you made or did not.
**Palimpsest:** A plate still half-sunk. Cage dry. Dock crate empty.
**Scraped:** Visitor book remarks still NOT MUNICIPAL. Overlay third line erased.

### Ministry
**Stands:** Record gone north or ticked from plates. Enquiry bell quiet. Scoring poster up.
**Lived:** Book in the registrar. Visited ticked from mouths. Overlay receipt refused.
**Palimpsest:** Copy in the Vault, original here, columns disagree.
**Scraped:** Dust rectangle in the drawer. Ira still at the desk with a blank book the size of the old one, pages uncut.

### Weighbridge
**Stands:** Lots on the readout glass. Osric still writes kilograms on the carbon. Two numbers travel with every load.
**Lived:** Needle only. Lot crate in the Annex. Edor's whites are occupations, not sites.
**Palimpsest:** Calibration weight and a plate on the same hook on alternate days.
**Scraped:** Hook empty if you stole 500 kg. Columns wrong. He wrote that the ice did not do it.

### Grange Hall
**Stands:** Cadastral column filled. Crosses still later-hand. Votes happen under numbers.
**Lived:** Column erased. 47, 12, 22. Kitchen notice ash.
**Palimpsest:** Column present, unused, graphite on the crosses.
**Scraped:** First page cleaner. Someone rubbed too hard. The names are thinner.

### Allotments
**Stands:** Sleeve stamped HISTORICAL. Plots plated. Dara waters numbers if she returned.
**Lived:** Sleeve current. Four ticks. Bin fittings only.
**Palimpsest:** Two 114s in the dirt. Plants do not read.
**Scraped:** Gatepost a clean rectangle of wire. Waiting list in rain if the sleeve left.

### Bridge Seven
**Stands:** FRIENDLY OBSTACLE in the Record. Dust still on the housing unless you checked.
**Lived:** Clipboard scraped. Span is Bridge Seven. Stone unmoved.
**Palimpsest:** Flag and scratch both. Clerks ask which is the name.
**Scraped:** Survey stool gone. Charges still taped. Authority still untested, or SPAN UNCERTAIN if you checked.

### Bus Reversal Loop
**Stands:** EVACUATION COMPLETE on the bow. Logbook in a crate. Unlisted still pointed at the city under the paint.
**Lived:** Street name from the suitcase. Forty-one buses still city-bound.
**Palimpsest:** COMPLETE and the street. Drivers would have hated both.
**Scraped:** Bare metal. Paint pot stolen. Circle still a circle.

### Pump Station Nine
**Stands:** Tags on five, dry motor tagged too if you let the cage win. Vault scum line unchanged.
**Lived:** One motor live if you energised; scratched name said aloud; tags in the water.
**Palimpsest:** Motor live, COMPLETE-grammar tags still on the drowned five.
**Scraped:** Stamp stolen. Hall as you found it. TravelHours to the Vault unchanged.

### Records Annex
**Stands:** Ground copy filed. String cut. Dusted crate outline on the landing.
**Lived:** Crate string-tied or in the Drown. Names said. Cloth damp.
**Palimpsest:** Copy filed and crate still on the landing. Quil hates the squeak of the cart more.
**Scraped:** Landing empty. Desk ledger thinner.

---

## Now-state inspect lines (mutation table)

One-line `inspect` plus 80–120 word `description` overlays for implementers. Swap `now` without rewriting the whole card.

### Km 19 — plated
**inspect:** Brass on orange. The lamp is still Ivy's.
The plate sits flush except the steel screw. Packing list oil column filled if you logged, blank if you did not. Spirit-level gone if Maren moved on. Crate lighter by one.

### Km 19 — scraped
**inspect:** Stencil. Four holes. A brighter square of paint.
Screw-holes in a rectangle. The stencil reads again. Pigment in the letter-cuts if you used lampblack to hide the holes and failed. Overlay crate empty or showing 19 present as a spare that has no post.

### Km 19 — palimpsest
**inspect:** You can read both if you kneel.
Brass and stencil. Ostrowski's next sheet will have two columns whether he wants them or not. The survey nail still says DO NOT TREAT AS ONE DISTRICT.

### Transit — print installed
**inspect:** A font from the floor. HELD is under it.
Plates screwed to the frame, not through the glass if Maren was careful, through if a junior did it. Grease a shadow. Solvent smell faded to a memory of maps.

### Transit — hand kept
**inspect:** Pencil on a string. Trestle gone.
The circle twice around 12. Bench marks on the linoleum. Printed plates in a crate at the dock of the Annex or still here if you refused to carry them.

### Archive — dug
**inspect:** A hole the size of a ledger. Grey dust on everything above the waist.
Cage lamp working until Overlay wants the battery. Index gone or copied. Vestibule third line dated if you finished it.

### Archive — sunk
**inspect:** The plate is deeper. The brick sounds the same.
Chisel edge more rolled. Garrison remarks still the useful sentence. Dock crate has straw in the weather.

### Ministry — book present
**inspect:** Visited empty, or ticked for rooms you named that matched.
Ira's pencil shorter. Authenticator fuse still the new one. Enquiry memo still pending clarification.

### Ministry — book receipted out
**inspect:** A lighter square of dust. A receipt in Ira's hand.
She will show you the receipt instead of the column. Scoring poster still up. Dead phone blotter still six hours eleven.

### Weighbridge — lots on glass
**inspect:** Needle behind brass. Carbons with two numbers.
Osric's back to the door. Calibration weight on the hook or not. Edor's whites still occupations.

### Weighbridge — mass only
**inspect:** The joke is still mass. The crate is not here.
FAVOURS ARE MASS, underlined once. Spring-balance honest. Overlay lots string-tied elsewhere.

### Grange — column filled
**inspect:** Lots in the right margin. Crosses in the later hand.
Kitchen notice on the table or burned. Porch sign still a request. Lasko a lot or a vote depending on the week you came.

### Grange — column erased
**inspect:** Graphite on the crosses. Names still names.
Drawer empty. Kettle queued. First page a little thinner.

### Allotments — historical
**inspect:** HISTORICAL on the sleeve, tacky or cracked.
Hut warm or cold. Plots staked in Overlay grid. Bin lighter. Tap weaker if membrane stripped.

### Allotments — list current
**inspect:** Four ticks. Plot 114 is this 114.
Bell-wire still in the cut. Autoclave key on the hook if Dara is home. Brass sorted or not.

### Bridge — listed
**inspect:** VISIBLE ticked. Housing dusted or undisturbed.
Clipboard gone to Ira or still in polythene. Stone unmoved. Near-bank spike rustier.

### Bridge — disturbed
**inspect:** SPAN UNCERTAIN on a receipt.
Lock oiled or broken. Toll price no longer a joke. Catwalk still there. Charges still taped. No fire.

### Bus loop — complete
**inspect:** Typeset COMPLETE. Logbook not on the dash.
Suitcase still between wheels unless you took it. Office 05:40 still pencilled. Paint skin on the pot.

### Bus loop — street
**inspect:** A street where a completion was.
Bow scraped and restencilled in a household hand. Forty-one noses still city-bound. Ticket punch still bites.

### Lock — COMPLETE up
**inspect:** The towpath can read it. The needle cannot.
Four matching screws. Benno's chart still metres. Fuse empty or fitted. Wrench frozen.

### Lock — plate down
**inspect:** Leaf mid-cycle. Holes in the landward face.
Packing receipt in the control house stove or in your pack. Life-ring still unthrown. Mooring polished.

### Pump — live
**inspect:** One motor in air, belt new or spare gone. Scum later at the Vault.
Tags on the drowned five or in the water. Switchboard string cut. Name on the housing still a name.

### Pump — condemned
**inspect:** Handle tagged. Dry island unvisited if you stamped first.
Cage lighter. Hall darker if you did not energise. TravelHours unchanged.

### 12-B — refreshed
**inspect:** Bright letters. A levy can walk here.
Gap held or filled. Cup tagged WATER POINT or missing. Kit engineering in the alcove or gone south.

### 12-B — original
**inspect:** Faded 12-B. Fourteen, a gap, six.
Paint pot stolen or dry. Bolt-holes. Water honest if the clamps were not moved.

### Annex — string cut
**inspect:** Landing outline. Wax crumbs.
Ledger heavier. Cloth damp. Window mat wetter from the extra crate-trip.

### Annex — refused
**inspect:** GROUND COPY. String. Dust on the top.
Quil's rota unchanged. Sole has not seen the plates. Boat-hook scars the same.

### Vault — cage filled
**inspect:** A book-size fact. Lectern matching or arguing.
Airlock empty or still hatted. Sole's blotter dated. Scum line as evidence of Pump Nine.

### Vault — cage empty
**inspect:** NOT YET A COPY, or NOT A COPY.
You skipped a spine room, or you scraped, or you filed nothing. Stacks still dry. Cart still squeaks.

---

## Overlay Current catalog card *(for `currents.json`; not `faction_lore.json`)*

```
id: faction_the_overlay
display_name: The Overlay
alignment: peaceful, conditional
home_region: all_regions (practice: cadastral walking)
trust: 0
wants: [brass_fittings, item_sr_stencil_pot, lamp_oil]
offers: [cadastral_keys, travel_correction_on_named_sites]
signature_quote: "The Schedule named households. The Record names ground. Ground does not argue."
access_rule: Scrape three plates without writing a lived name or a Continuity number, and Overlay labour withdraws. They do not raid. Rooms go dark of juniors. Posts stay posts.
```

Do not merge with `faction_the_tally`. The Tally counts. Overlay names ground. Do not merge with Archivists. Do not merge with Blank Rows.

---

## Additional NPC barks (trust / sister packs)

**Maren, Ice Road dark**
"I can plate a post from the ash side. I cannot make Yara cross. The seam is still a seam."

**Maren, levy honoured**
"The Allotments were numbered when I arrived. The hut was cold. That is not a victory. That is a window."

**Ira, 12-C on the desk**
"That order names people. This book names ground. If you make them the same document, say so out loud. I will note that you said it."

**Benno, Shallows**
"If Nomi brought you, the ring is hers to notice. If you walked the Cut, the ring is still a ring."

**Quil, roster burned**
"People without a wall and sites without a plate. Sole will not like the week. I will still ask for two mouths."

**Osric, Edor forty days**
"His paper is in the whites. I did not weigh it. It has a mass anyway."

**Dara, Hadi gone**
"The alcove in your hole is not my hut. If the caretaker labour went north, I water. If it went nowhere, the numbers arrive first."

**Maren, Kess refused**
"She is correct about the chart. I am still walking."

**Ira, Vault photocopy**
"Noted. The column is not a lie yet. It is also not true yet."

**Benno, detonator checked at Seven**
"That is a different lock. I do not keep that needle. I keep this one."

---

## Threatening body pairs (site prose)

Use `threateningBodyText` when Overlay access low or Office/Garrison trust low. Same room, different sentence.

| Room | Normal | Threatening |
|---|---|---|
| Km 19 post | The plate is a job. | The plate is how a clerk finds you. |
| Transit lobby | The hopper is empty. | Your number is already a site. |
| Weigh plate | The needle writes mass. | The needle writes you as mass. |
| Allotments gate | The cut is the door. | The plate on the post is an address. |
| Lock COMPLETE | Filing. | District 8 can send a column to a closed file. |
| 12-B stencil | Letters. | A pool pin. |
| Vault airlock | Hats of brass. | Guests who have already named your hatch. |
| Conscription overlay | Pell hits numbers. | Pell hits addresses. |

---

## Two-way scene prose (ten flags, shippable)

Not systems. What the player reads when a sister-pack flag is live in a Standing Record room.

### 1. Ice Road dark — `room_km19_seam`
The salt-white side is a wall of weather. Nobody has come from Yara's book this window. The survey nail's tag is rimed. You can still plate the post from the ash. You cannot make the Cut a road. Benno, if you reach him, has not heard a foghorn that was meant for you.

### 2. Levy honour — `room_allot_hut`
The stove is cold in a way that is not weather. A cup. Overlay stakes already in the window, a grid that ignores the sleeve. WATERED — NUMBERS on a scrap. Dara's boots are not under the bunk. The autoclave key is on the hook because she would not take it north. Frayne's minutes will record a shortage of caretaker-hours without naming a levy.

### 3. Membrane strip — `room_allot_plot_waitlist`
The watering can is iodine-stained. Process water, not clean. The row still comes. Dara, if present, does not call herself an engineer. There isn't one. Overlay's 114 does not drink. Yours might. Pigment oil and thyroid pills are the same expedition's argument in a different room.

### 4. Roster ink — Vault airlock + hatch later
Plates stacked like hats, and in your pack a rubbing of a wall of names. The escort that comes for 12-C can match a person to a post. Sole will file both if you said them. Kess will not have copied CUT-19 onto a row unless you made her irregular.

### 5. Roster burned — same rooms
The lectern waits for a gazetteer. The chart at home is a charred header. Overlay still has addresses. The escort brings a list of people from District 8 and a list of places from Maren's last packing sheet. Neither matches what you sleep under.

### 6. 12-C live — `room_12b_stencil`
Bright letters. Reconstruction pool, a fallback hole. The cup on the chain is a water point on a form. Sela, if she stayed, will not call the stencil her father. If she was claimed as a dependent, the kit room is quieter and the address is still useful to a clerk.

### 7. Sela stay vs clinic — `room_12b_kit`
Engineering: she remains. Salvage: she is on the stair, not in the alcove. The diagrams do not get smaller. Overlay will stencil around the clamps as Continuity issue either way. They are not.

### 8. Blank Rows — numbering 12-B or 11
If you refresh 12-B and ink a living name from 11 onto a census, Allocation 11's hatch still looks like a hatch. It will not open. Nila does not explain it twice. Maren, if she knew the rule, would not have screwed ALLOC-11. If she did not know, the plate is a mistake that behaves like policy.

### 9. Kess refuse — `loc_stack_roster_wall` overlay
Maren asked. Kess said sites are not a morning row. If you write CUT-19 anyway, the graphite shines like a lot number. Edor may copy it. Ansel will ask what the wall says about a lamp. Tamsin will not light the apron by cadastral.

### 10. Ostrowski's two-name sheet — Km 19 first offer
He sells where things are. The sheet has Kilometre 19 and CUT-19 in two columns, or a blot where he would not choose. He will not name Maren. He will not take a passenger to the post. Standing there is the correction. People move. The post does not.

---

## Radio / tannoy (text-first)

`[VO]` only if the pipeline exists. Otherwise UI.

**Transit gallery hiss** — six seconds, then nothing. Caption: the clock is still stopped.

**Ministry enquiry** — no tannoy. The sign where it was: LISTEN FOR YOUR SLOT, which was always the other building.

**Lock house** — dead tannoy. Benno does not repair it. COMPLETE would have liked a voice.

**Pump hall** — if energised: a contactor clack, once, then the sound of water moving a distance that is not this room. Caption: a drop you will measure at the Vault.

**Vault stacks** — no public address. Quil oils a cart. That is the loudest scheduled thing.

---

## Stealable / weighable / refusable object index

Implementer checklist. One object per room. Do not drop these from JSON inspect.

| Room | Object | If taken | If refused |
|---|---|---|---|
| `room_km19_post` | Brass plate CUT-19 / steel fourth screw | Stencil colder; Overlay short one post | Brass-on; lamp still Ivy's |
| `room_km19_seam` | Survey nail and tag | Ostrowski missing a point | Seam still a seam |
| `room_km19_oil_tin` | Pigment pot / oil | Lighting short or plates unreadable | Lamp due as scheduled |
| `room_km19_plate_crate` | Spare plate or screwdriver | A kilometre unnumbered or seated with a knife | Maren continues |
| `room_transit_lobby` | Blank disc in the hopper | Overlay will try to stamp it | Next person thinks the machine works |
| `room_transit_map_glass` | Grease pencil | Next correction is a fingernail | Hand remains |
| `room_transit_dob_desk` | Dry stamp FOLLOWING PROCEDURE | It will not ink | Procedure still on the blotter |
| `room_transit_overlay_bench` | Solvent | Loop job dry-crooked | Install possible |
| `room_transit_radio_gallery` | Turn-back flimsy | Loop still points at the city | Overlay typesets COMPLETE first |
| `room_archive_vestibule` | Visitor book | Chain empty | Garrison remarks still useful |
| `room_archive_grey_brick` | Half-sunk plate / chisel | Index stays in geology or hole opens | Brick as found |
| `room_archive_reading_cage` | Field index | Ira knows a copy walked | Cage still not the Schedule |
| `room_archive_loading_dock` | Brass strap-buckle | Frayne and Maren both notice | Crate still empty |
| `room_ministry_stair` | Fuse or hanging rail | Light dies / fittings | Authenticator still a belief |
| `room_ministry_enquiry` | Bell | Knocks | Ira still comes |
| `room_ministry_scoring` | Classroom pointer / clerk-line scrap | Finger; Ira patches a 9 | Poster intact |
| `room_ministry_registrar` | Standing Record | Receipt; visited still walking | Column shown empty |
| `room_ministry_obstacle_annex` | Binder / teaching tape | Printed cousin of a hand | Annex still filing |
| `room_ministry_dead_phone` | Hold blotter | Edor rhyme | Overlay typesets a completed slot |
| `room_weigh_plate` | 500 kg weight | Columns wrong | Needle honest |
| `room_weigh_hut` | Spring-balance poise / kettle | Small loads guessed | Osric still prices |
| `room_weigh_receipts` | White carbon / Edor's return | He may want it | Prices still mass |
| `room_weigh_overlay_lot` | Lot plate as brass | Site unnumbered | Needle vs lot |
| `room_grange_porch` | Tagged rifle | Ledger still has the cross | Sign still a request |
| `room_grange_table` | Unchained cup | Palms | Water still Verge-clean |
| `room_grange_ledger` | Pen | Next cross is pencil | Lived gazetteer holds |
| `room_grange_kitchen` | Kettle / notice | Meeting dry | Face-down remains a vote |
| `room_allot_gate` | Bell-wire | A leak gets a schedule | Cut still the door |
| `room_allot_hut` | Autoclave key / minutes | Surgery waits | Frayne writes them again |
| `room_allot_noticeboard` | Sleeve | List yellows | Four ticks remain |
| `room_allot_plot_waitlist` | Watering can | Row waits | Two 114s in dirt |
| `room_allot_brass_bin` | Overlay plate vs handle | Same metal, three buyers | Bin lighter either way |
| `room_bridge_near` | Receipt / D/9 stone | Mass or bullets; annex broken | Authority still works |
| `room_bridge_span` | Survey flag | Survey guessed | Ostrowski already sold the line |
| `room_bridge_charges` | Unused tape | Next charge on old adhesive | Looking still allowed |
| `room_bridge_overlay_survey` | Clipboard | Next survey is memory | FRIENDLY still a line item |
| `room_bus_circle` | Street-labelled suitcase | It will not fill | Lived name for Ira |
| `room_bus_lead` | Driver's log / punch | Stencil still lies | Punch still bites |
| `room_bus_office` | Lost-property tin / glass | Pencil weathers | 05:40 remains |
| `room_bus_stencil` | Paint pot | Km 19 pigment short | COMPLETE vs street |
| `room_lock_towpath` | Mooring ring / life-ring | Nomi notices | Unthrown |
| `room_lock_control` | Paint flake | Wheel still stuck | COMPLETE still a lie |
| `room_lock_benno` | Kettle / boots / chart copy | Tin cup; worse pair | Metres still a document |
| `room_lock_gauges` | Logbook | Needle still true | Photograph refused |
| `room_lock_leaf` | Frozen wrench | Gate still mid-cycle | Drown not a crater |
| `room_lock_reclaim_plate` | COMPLETE plate | Brass; holes in the face | Leaf unmoved |
| `room_pump_approach` | Bilge pole | Next sounding guessed | Benno's grammar stays |
| `room_pump_hall` | Condemnation tag | Record missing a tidy lie | Scratched name remains |
| `room_pump_dry_motor` | Spare belt | Sculpture or live | Rebuilders vs Overlay |
| `room_pump_switchboard` | Fuses / string | Benno's socket; handle free | Tag theatre |
| `room_pump_condemned` | Stamp | Handwriting next | Motor room may stay dark |
| `room_12b_stair` | Chalk tin | Next count charcoal | Gap held |
| `room_12b_unprovisioned` | Loose bolt | Not brass | Hall still unstocked |
| `room_12b_kit` | Notes / working kit | Water dies if taken | Engineering vs salvage |
| `room_12b_water` | Chained cup | Chain hangs | Levy map if tagged |
| `room_12b_stencil` | Refresh pot | Pigment short | Address or tomb |
| `room_annex_window` | Curtain-mat | Next arrival wets dust | Etiquette holds |
| `room_annex_dusted` | Cloth / fragment | Dust a sentence; Quil knows | Crate still undusted |
| `room_annex_name_desk` | Witness ledger | Completeness suffers | Mouths still required |
| `room_annex_refused_crate` | String / crate | Hats in the airlock | Problem remains a room |
| `room_vault_dock` | Mooring wedge | Unload harder | Scum line still evidence |
| `room_vault_airlock` | Tissue / plates | Wounds; hats admitted | Stacks stay dark |
| `room_vault_stacks` | Oil can / fragment | Squeak returns | No off-world thesis |
| `room_vault_sole_table` | Paperclip | She uses a pin | Files, does not sign |
| `room_vault_second_copy` | Padlock | Cage still a cage | Empty if spine skipped |
| `room_vault_standing_book` | Nothing but a rectangle | Lectern empty is an ending | Gazetteer the save keeps |

---

# 8. Word counts and consistency flags

## 8.1 Counts (this pack)

`wc -w` at time of writing:

| File | Words |
|---|---|
| `expansion_03_the_standing_record_plan.md` | **12,522** |
| `expansion_03_the_standing_record_creative_pack.md` | **18,043** |
| **Combined** | **30,565** |

Pack sits at the floor of the 18,000–28,000 band (Duty Roster pack 18,150; Holdfast pack 22,489). Density is room cards + mutation overlays + object index, not duplicated sermons.

| Bucket (approx.) | Words |
|---|---|
| Layout cards (§1) | ~8,600 |
| Strata + recast inspects | ~3,200 |
| Encounter prose (§3) | ~2,400 |
| Main + side quest stages | ~4,200 |
| NPC bibles + extra barks | ~2,400 |
| Endings + two-way scenes | ~2,400 |
| Object index + flags | ~1,800 |

## 8.2 Consistency flags

| Flag | Hold |
|---|---|
| No 3D interiors | Layouts are room cards + adjacency ticks |
| No 7th `faction_lore` row | Overlay in `currents.json` |
| No fifth Power | Sector 4 map closed |
| No Holdfast Cut clone | Km 19 is a seam, four rooms |
| No Duty Roster wing clone | Overflow 11 is a flag, not a layout pack |
| No Dead Hand spine | Obstacle annex + Bridge look; no arena |
| No Protocol Zero / social-media Vault | Recast on unlock |
| No terraformers, Tessarat, 7G, androids, neuromancers | Unused |
| No copied Holdfast/Roster sentences | New objects (steel screw, survey nail, pigment pot, COMPLETE plate, HISTORICAL stamp) |
| Hatch magnitudes | Untouched |
| The Tally | Not merged with Overlay |
| Archivists | Paper and mouths; Overlay is plates |
| Blank Rows | Refuse living names; Overlay numbers ground |
| Kess | Refuses site numbers on people |
| Brass | Bin / tin / playground / plates — same metal, silence |
| Located knowledge | Second paragraphs at the place |
| Main plot | Cannot complete from bunker menu; Vault cage empty if spine skipped |
| Companions | Named site-keepers, not a combat party |
| Bosses | Location crises |

## 8.3 Two-way examples (prose, not code)

- Ice Road dark: you reach Km 19 from the ash side; Benno has not seen Cutters this window; Shallows etiquette matters more.
- Levy absence: Allotments hut cold; WATERED — NUMBERS; Grange kitchen notice already on the table.
- Membrane strip: Dara's tap; pigment oil vs iodine; brass seats and plates in one bin.
- Roster burn + scraped plates: escort with foreign people and foreign places.
- 12-C + refreshed 12-B: a pool pin on a fallback hole.
- Nila + plated 11: hatch dark.
- Kess + Maren: irregular if you write CUT-19 on a row.
- Ostrowski + Maren: two sheets, one post, no handshake.
- Edor at the weigh hut: occupations vs kilograms vs lots.
- Sole + unsigned 12-C at the Vault table: files, does not sign; plates still need mouths.

---

# 9. Next prompt

> Implement Sprint 1 of `docs/expansions/expansion_03_the_standing_record_plan.md` using room inspect text from this pack for `room_km19_*` and `room_transit_*`. `LocationLayoutSystem` + JSON layouts + `quest_record_the_plate` / `quest_record_grease_pencil` + `npc_maren_holt` + `faction_the_overlay` in `currents.json`. No 7th Power. No 3D. Re-grep ids. Unity batch compile + EditMode. Cross-tool QA (Prompt #26): adjacency × recast × Overlay access.

