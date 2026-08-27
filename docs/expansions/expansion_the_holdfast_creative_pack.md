# ASHFALL: THE HOLDFAST — Creative Pack

**Internal id:** `expansion_the_holdfast`
**Kind:** Shippable prose. Additive to `docs/expansions/expansion_the_holdfast_plan.md`. Does not rewrite the bible.
**Voice lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.
**VO:** Lines marked `[VO]` are text-first; record only if the radio/tannoy pipeline already exists. Everything else is UI/Codex/inspect.

Ids below reuse the Holdfast bible. Re-grep `locations.json`, `locations_expansion3.json`, `QuestlineSO`, `faction_lore.json`, and `world_history.json` before implementation. No C# in this pack. No seventh Power. No Tessarat, Sector 7G, terraformers, androids, neuromancers.

---

# 1. Location cards

Schema for implementers: `id`, `displayName`, `inspect` (one line, first look), `description` (80–180 words, house voice). Each card names at least one object a player could steal, weigh, or refuse to touch.

---

## 1.1 The Cut — `loc_cut_*`

### `loc_ice_road_gate` — The Gate

**inspect:** A painted boom across ice, and a ledger that cares about axles.

**description:**
A boom laid across ice that was a shipping cut. Someone has painted a queue line on the ice. It has been repainted. The paint sits on rime and the rime sits on last winter's paint, so the line is thicker than it is straight. The hut is a shipping container with a stove-pipe and a window cut with a torch. Inside: a mechanical scale, a stamp, the axle ledger. Columns: date, origin, mass, remarks. The remarks column is almost never used, and is used, when it is used, for the dead. A brass weight marked `500 kg` hangs on a hook by the door. It is the calibration piece. Yara will notice if it leaves. The boom's chain is greasy enough to lock a column or hang a lantern. High-vis jackets on the pegs have faded to the colour of bone. Nobody here jokes about favours the way the Tollman does. You can put a crate on the plate and watch the needle. You can also walk around the boom. The ice on that side is not in the ledger.

### `loc_cut_kilometre_19` — Kilometre 19

**inspect:** The last Sector 4 lamp. The post is numbered. The oil can is not.

**description:**
A reflector post in Lamplighter orange, the number stencilled twice because the first stencil ran in the wet. This is where Ivy Corrigan's ledger stops. The lamp is lit on her schedule, not District 8's. A spare wick sits in a tin nailed to the post, and an oil can with a rag stopper sits in the snow at the base, as if someone set it down to argue and did not pick it up. The can is still half full. Ivy will not cross this kilometre. If you take the oil north, her next lighting is short. If you take it south, Yara's book will show a dark hour that was not hers. The ice beyond the post changes colour: Sector 4 ash on one side, salt-white on the other, a dirty seam like a poorly taped join. There is a child's mitten caught on the reflector bracket, frozen open. Nobody has claimed it. You can leave it. You can weigh it. You should not wear it.

### `loc_cut_weigh_hut` — Ice Weigh Hut

**inspect:** Receipts in triplicate. The third copy is the one they keep.

**description:**
A hut on runners, towed onto the ice each window and left when the window closes, which is why the floor is always a little wet. The scale is older than the hut: a beam and sliding poise, stamped with a municipal serial. Favours are entered as mass. A clerk will write `INTRODUCTION — 12 kg equivalent` without looking up. Carbon paper in a drawer, three colours, the pink copy for the traveller, yellow for the Cutters, white for the Office. The white stack is thicker. A spring-balance hangs over the counter for small loads: iodine tins, resin samples, children's boots. The hook is polished from use. You can steal the poise weight. The next column will be wrong by whatever you took, and Yara will know the ice did not do that. A kettle sits on a spirit stove. The tea is salt-tasting and nobody pretends otherwise. On the wall, a printed notice: `WINDOW LENGTH IS NOT NEGOTIABLE. ICE IS.` Someone has underlined the second sentence, twice.

### `loc_cut_dredger_hulk` — Dredger *Moth*

**inspect:** The stack is still smoking. The charts do not match Ostrowski's.

**description:**
A cutter-suction dredger frozen in at a list. The name on the stern is *Moth*, paint over older paint. The stack bleeds a thin geothermal steam that smells of wet iron. Someone lives in the superstructure: a bunk, a primus, a table weighted with sounding-leads and waxed sheets. The sheets contradict the ice-road map Bram sold you. Channel markers on the *Moth*'s chart are fifty metres west of the Kittiwake log, or the Kittiwake is fifty metres east; the ice will not tell you which. A brass sounding-lead, worn smooth, hangs from a nail by the chart table. It is still greasy with river mud from a river that is now a road. The occupant sells copies. Payment is food, not salt. In a locker: a flare pistol with two cartridges, and a tin of fish whose label has been soaked and re-glued. You can take the lead. The next sounding will be guessed. You can take the tin. The occupant will still sell you the wrong chart, politely.

### `loc_cut_brine_pool` — The Open Pool

**inspect:** Ice that never takes. A thermometer nailed to a stake, reading wrong.

**description:**
A black oval in the white. The brine outfall from the plant keeps this water from freezing, or keeps it from freezing the way ice is supposed to. The surface skins, breaks, skins again. A wooden stake at the lip has a glass thermometer bound on with wire. The scale is for air. The mercury sits in a range that means nothing useful and everything you need: too warm for a road, too cold for a swim. A gaff leans against the stake. There is a fish on it, or what used to be a fish, grey and stiff and glittering with salt. Protein. A bad idea. Somebody has cut steaks off it and left the rest as a warning or a larder. You can take the gaff. You can refuse the meat. You can put a gloved hand in the pool; the glove will stiffen and the skin under it will not thank you. Brine dogs use the far bank. Their tracks come to the water and do not go in.

### `loc_cut_waystation_a` — Waystation A

**inspect:** Four bunks, a stove, a filter marked for eleven days.

**description:**
The only legal overnight on the Cut. A prefab box on a gravel pad that was a car park. Inside: four bunks with numbered footboards (`A1`–`A4`), a stove that takes coal or resin-waste, and a filter canister with eleven notches filed into the rim, one per day of a window. The twelfth notch was started and abandoned. A duty slate hangs on a nail: *stoke / filter / sleep / do not walk dark*. Names rotate. One bunk has a paper tag instead of a stamped plate. Yours, if you winter here. The stove has a brass regulator wheel, warm when the pipe is live, stealable, missed immediately. Under A3, a tin of salt-rash salve with two finger-scoops gone. The door bars from inside with a scaffold pole. Home still ticks while you sleep here. The air tastes of salt and old filters. If you take the regulator, the stove will still burn. It will not burn evenly. That is how people wake up cold and do not know why until morning.

### `loc_cut_accident_12` — Accident 12

**inspect:** The ice took a column. The stencils on the crates did not go with it.

**description:**
A hole that froze over wrong: a dish in the road, a pressure-ridge lip, timber and canvas at angles that used to be a sledge-train. The cargo is still readable. Stencil on the nearest crate: `ALLOC-7 / NOT FOR GENERAL ISSUE`. The crate beside it has split. Tins inside, olive, the same stamp. One tin is already open, spooned, frozen at the rim. You can take a closed tin. You can bury the stencil under ice-cuttings so the Cluster never has to see its own ration on the Cut. You can tell Salt, who will want the calories and not the letters. Yara's accident book lives in the weigh hut; this site is the illustration. A leather glove is frozen to a crate handle, fingers inside it or not — do not pull to find out. Ice crows wait on the ridge. They have learned the timetable of windows. A length of axle sticks from the ice like a survey pin. It is still attached to something you cannot lift.

### `loc_cut_south_beacon` — South Beacon

**inspect:** If this lamp is dark, the road is closed, even when the ice is thick.

**description:**
A lattice mast on a caisson, Cutter-maintained, wick and reservoir in a cage at head height. The cage has a padlock. The padlock is decorative; the real lock is Yara knowing who last filled it. A measuring-stick hangs on a lanyard: oil depth in finger-widths. The stick is stained to a line marked `WINDOW`. Below that line the beacon is considered dark whether or not a flame is showing. A spare mantle, tissue-thin, sits in a tobacco tin on the cage floor. Do not pinch it. Do not walk the stretch beyond if the stick is low. Relighting for a trap is the cousin of Ivy's exception, and Yara's withdrawal is the same shape: lamps out over eleven days, access gone. You can steal the oil. The next column will write an accident that has your mass in it, or will not be there to be written.

---

## 1.2 The Saltworks — `loc_salt_*` / existing plant

### `location_abandoned_desalination` — Municipal Desalination 8 *(recast; existing id)*

**inspect:** Occupied. Failing. Named. The word *abandoned* was what Sector 4 could see from the Drown.

**description:**
Concrete intakes, salt-white yards, steam that smells like hot metal and iodine. The RO hall is a nave of pressure vessels, numbered, some blanked with steel plates and warning tags from a year that still used printed tags. Workers wear plant suits that were never hazmat — grey canvas, inner-tube patches at the knees, visors clouded from the inside by breath. A valve wrench, still warm, hangs on a labelled peg: `HALL 2 — DO NOT REMOVE`. People remove it. It comes back. Hydro-Barons were grade 4–7 municipal engineers. The Toll coined the name. They kept the plant because turning it off kills the Cluster in two days, and because nobody issued a stop order. A visitor book by the inner door has names in a plant hand, not an Office hand. Leva's is the most frequent. You can take the wrench. The next isolation will be done with a pipe and a prayer that is not a prayer. The yard salt crunches. It is not snow.

### `loc_salt_membrane_hall` — Membrane Hall 2

**inspect:** Resin drums counted twice a day. The count is always short.

**description:**
The still-working bank. Pressure gauges with hairline cracks, needle-stops painted by someone who got tired of replacing glass. Resin drums along the south wall, stencil `RO-BED / BATCH`, chalk tallies on the rims: morning count, evening count, a third number that is the difference. The difference is never theft. Evaporation, spent-stack growth, a drum that was always light. A sampling dipper hangs on a nail — polypropylene, stained tea-brown. You can steal a cup of virgin resin. The plant will miss it on Tuesday. You can refuse to touch the spent membranes in the drip-tray; they look dry and are not. Fume sits at chest height. Shift whistles blow from the Grade Hut and are not always answered. A child's drawing is taped inside a locker: four rectangles, yellow and green, labelled `QUAD`. The locker owner works the outfall.

### `loc_salt_intake_caisson` — Intake Caisson

**inspect:** Below the ice. A hatch, a ladder, a rebreather that has been used.

**description:**
A concrete cylinder in the estuary ice, lid dogged with four wheel-locks. One wheel is frozen and is left that way; three are enough if you trust the gasket. Inside: a ladder with rungs missing where brine ate the welds, a platform, the intake mouth sucking black water that used to be a shipping channel. A rebreather hangs on a peg, straps patched, scrubber canister dated in grease-pencil, the date two windows ago. You can take it. You can refuse the descent. Without a sub-bay this is a timed expedition: warmth, breath, the knowledge that the plant's thirst is a pipe in the dark. A brass nameplate on the caisson wall still says `MUNICIPAL HYDROGRAPHIC — INTAKE 8-SOUTH`. Someone has scratched `LEVA` under it, small. A dosimeter on a lanyard ticks faster than the Cut. Ice lid groaning is the clock.

### `loc_salt_iodine_store` — Iodine Store

**inspect:** A cage. Two keys. Thyroid medicine and water treatment in the same lock.

**description:**
A mesh cage inside a dry room that is only dry compared with the hall. Drums of iodine crystal, brown bottles of tincture, blister packs of pills with Continuity lot numbers. The Office has the key on a numbered fob in Ormund's drawer. The Salt has a copy they do not admit, hung on a nail behind the Grade Hut minutes cupboard, inside a tea-tin. The cage door has two padlocks in series, which is a kind of honesty. A spring-balance hangs for issuing: mass out, signature, purpose (`PROCESS` / `CLINIC` / `THYROID`). The thyroid column is shorter than process and is watched more carefully. You can take a bottle. The next outfall shift will drink rawer water or the next Cluster child will go without the dose the formulary assumes. A paper tag on the lowest shelf: `NOT FOR GENERAL ISSUE`. The same grammar as Accident 12. The same metal stamp, different cargo.

### `loc_salt_outfall` — Brine Outfall

**inspect:** Where the plant returns what it does not want. The clipboard for shift limits is dry. The people are not.

**description:**
A concrete apron, stained white, sloping to a channel that feeds the Open Pool. Steam here is not the useful kind. Salt-rash cases come from working without a shift limit. The limit exists on paper: a clipboard in a plastic sleeve, hours, names, a red line at six. The sleeve is dry. The names below the line are in the same hand as the names above it. A tub of salve sits on a crate, lid off, grit in the grease. You can take the salve. You can work the unlimited shift and bring the iodine protocol back, or refuse and watch the clipboard stay ceremonial. A wooden scoop for salt-crust is chained to the rail so it will not be stolen. The chain is newer than the scoop. Brine dogs at the far fence. Workers throw them fish from the Pool when the fish are worse than the dogs.

### `loc_salt_grade_hut` — The Grade Hut

**inspect:** Minutes. Same grammar as Ottilie Frayne's Works. Different water.

**description:**
A site office with a table that seats seven and usually seats four. Binders on a shelf: volumes numbered, Volume 12 current, motion tabs sticking out. *Motion: that we keep running. Carried.* A kettle, mugs with plant-stencilled numbers, a wall chart of membrane integrity that is more honest than the Office's steam token ledger. Leva sits with her back to the door. Auditors from the Cluster take meter readings and do not sit. A brass valve-seat, failed, is used as a paperweight on the visitors' book. You can take it; it is already failed, and it is still brass, and Frayne's demand and Leva's demand will both notice a world with one less fitting. The hut smells of iodine and wet wool. A notice: `IN SITU ESSENTIAL — DO NOT REALLOCATE THIS POST`. The date is Exchange+0. Nobody has taken the notice down. Nobody has issued a stop order.

### `loc_salt_cooling_canal` — Cooling Canal

**inspect:** Steam to the Cluster runs along this. The wheel is painted. The paint is a warning.

**description:**
An open channel, lidded in places with pre-cast slabs, carrying waste heat northeast in a pipe beside the water. The pipe is lagged with what used to be mattresses. At the isolation point: a wheel painted red, then white, then red again, so that turning it is a decision you can see from the towpath. Sabotage here is a war crime by local definition and a repair job by yours. A padlock through the wheel has been cut and replaced twice. The current lock is Office. The Salt has bolt-croppers in the Grade Hut and does not mention them. You can refuse to touch the wheel. You can turn it a quarter and feel the pipe knock as pressure changes. A thermometer strapped to the lagging is marked at the temperature below which Block C hits 2°C in a night. The mercury is a conversation.

### `loc_salt_scrap_membranes` — Spent Stack

**inspect:** Failed RO membranes. Toxic to handle. Valuable to people who still believe in recoating.

**description:**
Pallets in the lee of Hall 2, shrink-wrap gone brittle, spiral-wound elements stacked like carpet rolls. Warning pictograms for skin and fume, faded. A recoating jig cobbled from a drum and a pump, unused this window, used last window, yield written on the drum in grease-pencil: `LOW`. You should not handle these without resin gloves. People do. A pair of gloves hangs on the jig, insides powdered, outsides glazed. You can steal a spent element for the recipe. You can refuse to touch the stack and write the drums off, which is the honest Tuesday. Ice crows do not land here. Even they have a limit. A child's mitten — not the one at Kilometre 19 — is pinned under a pallet corner, as if used to stop a roll and then forgotten. Leave it. The salt has made it a sculpture of a hand.

---

## 1.3 The Cluster — `loc_cluster_*`

### `loc_cluster_gatehouse` — Cluster Gatehouse

**inspect:** They will ask for an Allocation number. Twelve is a known discrepancy.

**description:**
A booth and a barrier painted civil-service cream, the cream still the cream. Queue lines on the asphalt, repainted, like Ration Plaza, except the queue here is for authentication and work tickets. A keypad under a plastic hood: Allocation numbers, four digits, a bell that does not ring for unlisted guests. A procedure binder on a chain. Tab: `DISCREPANCY`. The page is worn. "12" opens a file, not a gate. A visitor badge printer still has stock. The badges are paper. Yours would say `GUEST / BLOCK C` or `UNSCHEDULED`. A brass bell on the counter is for the night clerk. You can steal it. The night clerk will knock on the glass instead. High-vis jackets on a peg, Continuity issue, faded. Edor will look at them if he is with you. An allocated runner will look at them differently. A thermometer outside the booth is for the steam-fed district, not the Cut. It reads like a place that survived.

### `loc_cluster_quad` — The Quad

**inspect:** Four cultivars, two of them yellow. Chains. No seats.

**description:**
Civic square. Hydroponic troughs along the south wall: four cultivars, two failing, leaves the colour of old paper. Labels in a plant hand, not an Office hand. A playground with the chains still on the swings and the seats unscrewed. Brass. The chains hang and do not swing. A noticeboard: labour rota, work tickets, a missing-persons strip that is all Sector 4 trades — caretaker, records clerk, veterinarian, ice-cutter, lamp. One of the trades is yours. You can take a yellow leaf. You can take a chain. You can take a seat if one has been left as a warning or a spare; the Office will notice mass leaving the Quad the way the Weigh Hut notices axles. Queue paint underfoot, for work, not bread. Children pass without looking at unlisted guests. Adults look once and file you. The trough water is process, not clean. A tin cup is chained to the irrigation tap. People drink from it anyway.

### `loc_cluster_block_c` — Block C

**inspect:** Guest housing. The nameplates are metal. Yours would be paper.

**description:**
Four storeys, stairwell C, cream paint, numbers that were never allowed to fade. Apartment doors with stamped plates: allocated names, some with a second line for a dependent. Guest rooms at the end of the second-floor corridor have paper tags in the plate slots. The tags curl. Forty apartments in the Cluster are kept for arrivals; three of them are in this block and are dusted. In C-214: children's boots, sizes 1–4, never worn, the same crate grammar as Allocation 12. You can take a pair. Warmth. The corridor will be lighter. You can leave them. A steam radiator ticks if the substation is live. If it is not, the indoor thermometer on the landing is the honest object in the building. A duty roster in a plastic sleeve by the stair: names that sleep here. A blank line. You can write. You can not.

### `loc_cluster_clinic` — Cluster Clinic

**inspect:** A working autoclave. A human formulary. No veterinarian.

**description:**
A ground-floor suite that still smells of ethanol and hot cloth. The autoclave cycles. Someone logs the cycles in a book with a column for failures, which are rare and written in red. Iodine in a locked cabinet, key on a lanyard around the duty nurse's neck. The formulary is bound, pre-war, dosages for a species the Verge has been approximating. Ianov would weep. They will not send a copy south unless the levy is honoured. A dental chair in the corner, bolted, instruments present — the inverse of your bunker. You can steal a vial. You can refuse the claim they will read over Sela if she is with you: dependent of RENN, HALVARD, allocated, not arrived. A poster of a thyroid, hand-coloured, has a child's correction in the margin. The waiting-room chairs are plastic. Two are yellow. The rest match the paint.

### `loc_cluster_school` — Cluster School

**inspect:** Nineteen children. The homework is arithmetic. The arithmetic is a score.

**description:**
A classroom that was always a classroom. Nineteen coats on pegs, not all occupied on a given day. Blackboard: Reconstruction Utility Rating, worked as sums. Occupation points, dependent points, a line at sixty. The teacher does not raise their voice. A dependent is worth points; that is on the worksheet. Wren, if brought, will sit in the back and not speak. A box of chalk, white and yellow. You can take the yellow. The next graph will be white-only. Exercise books in a crate, names on covers, one book with a Sector 4 occupation written in an adult hand on the flyleaf, as if someone were practising a retrieval. You can correct a sum. You can let it stand. The playground is visible through the window: chains, no seats. Recess is indoors when the UV board by the door is on the red nail.

### `loc_cluster_office` — The Office

**inspect:** Continuity civil service that arrived. The Sector 4 Schedule is in a drawer.

**description:**
A room that was designed to look like work and still does. Ormund's desk, blotter, a tray of triplicate forms, a lamp with a working bulb on plant current. The drawer on the right has a key he keeps on his person. Inside: the Sector 4 Schedule, complete. Sole's entry is there. Frayne is not. Halvard Renn is, marked ALLOCATED / NOT ARRIVED / 12-B UNCONFIRMED. A second copy of Reconstruction Order 12-C in a folder with a string tie. You may be shown it. You may not touch it unless he turns it. A cup of water, process, untouched, replaced twice a day by a clerk who is not Edor. A stamp: `DISCREPANCY NOTED`. The pad under it is worn through. The window looks onto the Quad chains. Ormund does not look at them while he talks. He does not need to.

### `loc_cluster_steam_substation` — Steam Substation

**inspect:** If this dies, Block C hits 2°C in a night. The plant can be up. This can still be down.

**description:**
A valve house at the Cluster end of the cooling canal. Gauges, a bypass, a drain cock that drips into a bucket someone empties on a rota. The rota is on the door. Names. If a name is a levy name, the bucket waits. A wall thermometer with a grease-pencil mark at the temperature that means the apartments fail. You can turn the bypass. That is a repair or a war crime, depending on who writes the minutes. A steam token box — wooden, slotted — takes the Cluster's warmth currency-in-kind. Tokens are stamped fibre, not metal. You can steal tokens. You cannot steal heat. Insulation on the main is mattress-lagging, same as the canal, patched with children's coats that were outgrown and not thrown away. The coats still have names in the collars.

---

## 1.4 The Shelf — `loc_shelf_*` / existing coast ids

### `location_frozen_river_barge` — Frozen River Barge *(recast; existing id)*

**inspect:** Dock crew on frozen cargo. They will trade a crate for a way off the ice.

**description:**
A river barge pinned in pack ice that used to be a harbour roadstead. The hold is a larder and a problem. Crates of Continuity stock, some honest, some swollen. The crew have been living on what would freeze and leaving what would not. They are thin. They are not a carnival. A crate marked `BEANS / ALLOC-7` sits on the hatch-coaming as the asking price for passage toward *Hearth-4* or toward the Cut. You can pay. You can refuse and walk the Ridge. A billhook is lashed to the rail, edge dull from ice, not from people. A logbook in the wheelhouse lists days since a boat came, then stops listing and starts listing temperatures. The last temperature is the same as the first. Someone has been winding the clock. The key is on a string around the skipper's neck. You can ask for it. They will say the clock is how they know the foghorn is late.

### `location_crashed_icebreaker_convoy` — Icebreaker Convoy *(recast; existing id)*

**inspect:** Military rolling stock that tried to reach the roadstead. The RTG is a bruise on the ice.

**description:**
Not a submarine. Not a joke. Ice-capable wagons and a locomotive that tried to make the coast when the Cut was not yet a road. Derailed where the ice moved. A cracked RTG in the power car makes a hotspot you can see as a yellow-brown stain in the white, visible before the dosimeter agrees. Tungsten bars in a crate that did not split. Track sections. A map fragment `Victory_Migration` already wants, waxed, the estuary drawn as a summer river. You can take a bar. You can take the fragment. You should not linger. The dose window is a fact. A helmet on a seat, visor down, welders' glass taped in — someone understood the albedo and still sat too long. Do not put it on to check. Ice crows on the couplings. They wait.

### `loc_shelf_hearth4` — Tender *Hearth-4*

**inspect:** Upright. Authenticator light still lit. A hatch that wants a number.

**description:**
A continuity tender, still drawing a little current, still answering on a schedule. The ice has come up to the Plimsoll mark and stopped, as if it were waiting for the same order the people inside are waiting for. The authenticator above the boarding hatch is a small green lamp. It has no reason to still be a lamp. Mire will not open for unauthenticated boarding. The hatch log is a clipboard in a pouch, every refusal entered, dates, reasons (`NO STAND-UP` / `NO NUMBER` / `BLASTING PARTY — DENIED`). There are a lot of refusals. You can steal the clipboard. He will say it again from memory. Explosives to blast the ice remain possible and are a bad idea the Cutters will say out loud. A mug, fused by salt to the lookout rail, has a name on the bottom. Fleet. Not Cluster. The name is still on a bunk inside.

### `loc_shelf_roadstead_crane` — Roadstead Crane

**inspect:** The only heavy lift on the coast. The hook is frozen in the last job.

**description:**
A harbour crane that has not slewed since the ice locked the slew-ring. The hook hangs over a gap where a lighter used to be. A chain binder on the hook has a tag: `HOLD — HEARTH-4 STORES`. The stores were never lifted. Recovery Yard grammar, different district: this is where a vehicle can be pulled rather than scrapped, if you have heat for the ring and people for the winch. A grease-gun on the cab floor, nozzle snapped. You can take the gun. The next thaw will be done with a stick. The cab has a thermos, empty, and a photograph of the Cluster Quad before the seats were unscrewed. The seats are in the photograph. You can take the photograph. The crane does not need it. Someone on *Hearth-4* might.

### `loc_shelf_pressure_ridge` — The Ridge

**inspect:** The walking route to *Hearth-4* when blasting is refused. Warmth and fatigue are the toll.

**description:**
Pressure ice, a white wall, a path flagged with dark-mark stakes Yara left so that even unlisted parties would not guess. The stakes are lath with black cloth. You can pull a stake and the next party will walk the wrong lead. You can leave them. A crate has been used as a rest: `ALLOC-7`, empty, the stencil facing the sky. People have sat on it. The lid is a better seat than the Quad chains. A pair of welders' glass goggles hangs on a stake for the daytime crossing; UV here is a coastal fact. Take them if yours are broken. Leave them if you can see the bruise of the convoy from this height and do not need to look at it longer. The far side drops toward the tender's upright funnel. Distance is hours. Yara measures it in hours. So should you.

### `loc_shelf_foghorn` — Foghorn 8

**inspect:** It sounds whether anyone is coming or not.

**description:**
A shore horn on a concrete plinth, clockwork and compressed air, a timer that has outlived the harbourmaster. The escapement is visible through a cracked inspection plate: brass, ticking, stealable, after which the Cutters lose the coast in fog and something on the water loses the coast as well. A key on a hook inside the plinth door winds the spring. The foghorn key is also an item. If you silence it to avoid attention, Yara's book will show a dark that is not a lamp. A log of soundings-by-ear is pencilled on the door interior: dates, visibility, whether *Hearth-4* answered. The last answer is a dash. You can refuse to touch the timer. You can take it home and hear it faintly on Silence nights, which is not the same as leaving it here, doing the job.

---

## 1.5 Sector 4 nodes that change meaning

When `exp_holdfast_unlocked`. These are recasts layered on existing ids — not new regions.

### `loc_the_shallows_market` — The Shallows *(existing)*

**inspect:** Nine boats. Nobody boards. Nomi will run north once per window, for a price.

**description:**
Nine boats tied into a raft over what used to be a retail park. Trade happens at gunwale height. The etiquette has not changed. What has changed is the chalk on Nomi Fisk's transom: a north arrow and a date range that matches an ice window, erased after each run. She will take a sealed crate to the Gate if etiquette has not been broken and if you pay in something that is not a side. A sounding-lead of her own, not the *Moth*'s, hangs by the tiller. You can try to buy it. She will name a price that is a joke about mass. The Kittiwake chart, if you have copied it, makes her quieter, not warmer. She already knew the estuary had a road in winter. She had not been paid to use it.

### `loc_weighbridge` — The Weighbridge *(existing)*

**inspect:** The Tollman's first office. Edor Vale's first appearance. The scale still works.

**description:**
A truck scale with a working mechanical readout. Prices are still set here. After the sheet, a man in a faded Continuity jacket stands off the plate so he will not be charged as cargo. Census Clerk Grade III. A form in triplicate, already filled with three of your survivors' pre-war occupations, wrong by one each. He offers to read it again. The Tollman's man will charge for the introduction by mass. You can put Edor's satchel on the plate and watch the needle; Edor will permit it, which is a kind of joke he does not enjoy. The satchel contains more forms, a date-of-birth strip, and a thermos of process water he has carried south like a credential. Do not drink it to be polite. He will not be offended. He will note it.

### `loc_toll_house` — The Tollman's Bridge *(existing; expansion3)*

**inspect:** The introduction fee is posted. The destination is not.

**description:**
A river crossing the Warlords have held since the Tollman learned the demolition codes. The near-bank house still takes blood-or-bullets in the old grammar, and now also takes a line item: `INTRO — NORTH CLERK`. The posted rate is honest. The receipt is honest. The joke about favours-as-mass is repeated, and then the clerk from District 8 is pointed out as if he were a crate. You can pay. You can walk around and meet Edor at the weighbridge anyway; the Tollman still wants the fee for having seen you see him. A carbon copy of an introduction receipt is pinned to the board. The destination field says `ESTUARY / SEASONAL`. Someone has written `the Salt` underneath, lower case, as if it were not a place yet.

### `location_ministry_of_truth_bunker` — Ministry of Truth Bunker *(existing)*

**inspect:** A second copy of the Schedule's cover letter, addressed to District 8.

**description:**
The department that ran the formula. Partial manifests, the scoring rubric, the memo about public enquiries. In a correspondence folio that was always here and is only readable now that you have a reason: a cover letter for the Sector 4 Schedule, carbon, addressed `Office of Continuity — Allocation Cluster 7 — Hydrographic District 8`. The letter assumes the Cluster exists. It assumes the Quiet Evacuation went north. It assumes Allocation 12 is a local overflow hole. None of this is new to the paper. It is new to you. You can take the carbon. Ormund already has the ribbon copy. Sole, in the Drown, has been keeping completeness without this address. The folio smells of wet concrete. A rusted staple still holds the pages that were not supposed to travel.

### `loc_the_allotments` — The Allotments *(existing)*

**inspect:** Frayne's brass demand and District 8's brass demand stack. The tin does not comment.

**description:**
Two hundred numbered plots, a caretaker's hut, a noticeboard, a chain-link fence. The waiting list is still in a plastic sleeve. After Holdfast unlock, a second notice shares the board: a Cluster requisition for valve seats, unsigned, in an Office hand, offering process-water credit. Frayne's minutes already wanted brass. The playground seats in District 8 are the same metal as the nameplates in the tin behind your filtration stack. Nobody on this board will say that. You can pull a fitting from a plot tap. You can leave the tin where it is. You can sell plates north. The troughs here are soil. The troughs there are process. Both are thirsty in opposite directions. A child's watering can, green, sits by plot 14. Plot 14 is being farmed by someone on the waiting list who lived.

### `loc_low_background_lab` — Low-Background Laboratory *(existing)*

**inspect:** The counters in here still mean something. District 8 will ask. They do not want the answer.

**description:**
Deep in the salt, behind two airlocks. Shielded with steel salvaged from pre-atomic shipwrecks. After the census, an Office request appears in the log as a visitor slip: fallout provenance, coastal versus basin, a sample jar with a District 8 tag, empty. They will ask the Cold Count to prove what the ice already implies — that the Holdfast's thinner ash is not cleaner air, only different dirt, and that UV is the coastal tax. You can run the count. You can refuse. The empty jar is stealable and useless. A lead brick used as a doorstop has a scratched inventory number. Do not pocket it unless you like doors that do not stay open. The Count will not go north. The jar can.

---

# 2. NPC voice bibles

Speakable. Text-first. `[VO]` = candidate for radio/tannoy/once-per-run monologue only.

---

## 2.1 `npc_cael_ormund` — Registrar-General Cael Ormund

**Where:** `loc_cluster_office`
**Was:** Logistics planner, Office of Continuity. RUR 34, score 62.1, ALLOCATED.
**Will not:** Falsify a score. Raise his voice. Call anyone a thief.

### Voice rules

**Do:** Civil-service present tense. Name the form, the clause, the occupancy. Use "the discrepancy is noted." Put the threat in the next paragraph of the paperwork. Offer completeness as if it were a kindness, without calling it one. Refer to people by occupation and score when he has them. Call the player’s shelter a facility.

**Don't:** Say please. Say or else. Raise volume. Use metaphor. Say *fair* or *unfair*. Say *family*. Joke. Apologise. Call unlisted occupants squatters, thieves, or guests (he says `unallocated occupants` or `the reconstruction pool`).

### Barks

**First meet**
"You are living in a facility that authenticated for fourteen. The fourteen did not arrive. Under Continuity Reconstruction Order 12-C, unallocated occupants of an authenticated facility constitute a labour reserve. I am not collecting you. I am scheduling you."

**Ice Road**
"The window is a property of the ice. Passage is entered in the axle ledger. If you are on the ice without a return, you are still on a return. I have a copy."

**Levy**
"Three names. Thirty days. Occupations as scored, or as observed. The reconstruction pool is not a request. It is a line. I have written it."

**Refuse**
"The discrepancy is noted. Refusal is also a status. Status follows occupancy. The ice will open again. So will this file."

**Membrane**
"Steam is a municipal output. I do not operate valves. I record who does, and whether Cluster indoor temperature remains within the occupancy standard. The standard is written."

**Hatch**
"Authentication is a procedure. Procedures do not wait in the ash. If the hatch remains shut, the escort remains on the rota. Forty days is the quiet interval. After that the file does not get quieter."

**Trust-high**
"Your return is current. That is uncommon. I have marked it. Marking is not a favour. It is an accurate column."

**Trust-low**
"The occupancy of Allocation 12 remains irregular. Delay is also irregular. I do not require your agreement to schedule a column. I require a window."

**Sela present**
"RENN, SELA. Dependent of RENN, HALVARD, water engineer, allocated, not arrived. Cluster Clinic can enter her as a Cluster child. That is the correct filing. She may speak. The filing does not require her to agree. It is easier if she does."

**Substitute noticed**
"The names on the ice are not the names on the levy. Irregular. I will audit. Audits have a second line."

**Frayne absent**
"There is no FRAYNE in the Sector 4 Schedule. Reconstruction Utility Rating eleven is not a score that produces an allocation. I do not correct the Schedule to include people it did not include."

**Sole named**
"SOLE, MARGIT J. Records Clerk II. Not allocated. Her copy in the Drown is not a second Schedule. It is the same Schedule in a different room. Completeness is not execution. I execute."

**Playground brass**
"Mass has left the Quad. Brass is a municipal fitting. If it is in your pack, it is still a municipal fitting. I have noted the mass. I have not called it theft. Theft is a different form."

**Waystation**
"Forward occupancy on the Cut is permitted for the length of a window. It is not a second Allocation. Do not write it as one."

**If the plant is dying**
"Two hundred and eleven indoor occupants. Forty apartments held for arrivals. I am not a technician. I am the person who will write the indoor temperatures if they fall. I would prefer not to write them."

### Monologue (once) `[VO]`

"I will say this once, because repetition is for people who did not hear the form. The Quiet Evacuation was not a rumour. It was a timetable. Cluster 7 authenticated. The formula, in this district, ran to completion. Sector 4 was not abandoned by accident. It was scored. Occupations below twenty were stored as a reconstruction pool because the Cluster cannot desalinate a child, or file a death, or keep a road, with the people the rubric kept. You are living in a hole that was a rounding. Allocation 12 was overflow. Overflow is still a number. I am not angry that you used the stores. Stores are for occupants. I am scheduling the occupants who were always going to be scheduled. If you honour the levy, some of your people will sleep in Block C under stamped plates. If you refuse, I will come south when the ice allows, with the same paper, and a quieter voice, which is the same voice. I do not raise it. Raising it does not make a score move."

### threateningBodyText pair

**Neutral (`bodyText`)**
Ormund stands with the blotter between you. He turns a page with two fingers. He names three of your people by occupation. He says the ice has a length. He does not ask if you understand. He waits until you say whether you do.

**Threatening (`threateningFactionId: faction_the_office`)**
The same blotter. The same two fingers. He does not name occupations. He names the hatch. He says the quiet interval is forty days and is already counting. He does not say what happens on day forty-one. The next form is already in the tray, face down.

---

## 2.2 `npc_edor_vale` — Census Clerk Grade III Edor Vale

**Where:** first `loc_weighbridge`, then the Cut, then the hatch
**Was:** Junior enumerator. Score 60.4 — lowest allocated band. He knows it.
**Will not:** Enter the bunker uninvited. Falsify a date of birth.

### Voice rules

**Do:** Offer to read it again. Be precise about dates of birth, sometimes too precise. Apologise for the ice, not for the form. Say "that's all right" about confusion. Wait in the ash. Name the triplicate colours. Ask permission to wait near the hatch, and then wait even if permission is messy.

**Don't:** Invent scores. Joke about Convoy 12. Raise his voice. Call Ormund cruel. Call the player family. Enter uninvited. Skip a field. Say the form is optional.

### Barks

**First meet**
"Most people want it read again. That's all right. There isn't a time limit on understanding it. There is a time limit on the ice. I can start at the heading."

**Ice Road**
"I don't open the road. I record who crossed. If you want me on the column, I will walk where Yara says is lit. I will not guess."

**Levy**
"I wrote the three names as I was told to write them. If they are the wrong three, I can read the instruction again. I cannot change the instruction. I can wait while you decide."

**Refuse**
"I'll wait. I won't come in. Waiting is in the procedure. It isn't a threat. It is the time the ice is using anyway."

**Membrane**
"I don't count resin. I can count people who can stand a shift. I can read you the indoor standard if that helps. It usually doesn't. I can read it anyway."

**Hatch**
"They're behind me. I asked to speak first. You don't have to open. If you don't, I will still be here in the morning. I brought the pink copy. The white one stays with him."

**Trust-high**
"I corrected a column because you told me the truth about a date. I won't put that in the remarks. Remarks are for the dead. You're not."

**Trust-low**
"The occupations I have are still wrong by one. I did ask. I will ask again. I have to leave the errors if you won't sit with me. Errors travel."

**Wrong occupations**
"I have mason, not caretaker. I have clerk, not clerk-grade. I have vet assistant, not vet. Each one is adjacent. Adjacent is how we miss people. You can correct them. I would like that."

**Sela's card**
"I can copy the number. I won't take the card. Laminated is a kind of proof we don't get often. I will write UNCONFIRMED until someone who can confirm, confirms."

**His own DOB**
"There are two years on my return. I noticed late. I would like to sit down. I would like you not to make a joke. I can read both lines aloud. You can tell me which one to strike."

**Kilometre 19**
"I won't ask the Lamplighter to cross. I can wait on this side with the form. The form doesn't mind which district it is in. I do, a little. That's all right."

**Block C**
"The plates are metal. The tags are paper. I slept under a plate my first year. It didn't make the steam hotter. It made the rota easier to read in the dark."

**If you let him wait at the hatch**
"I'll keep off the step. I'll keep the stove-tin. If someone in your house wants the form read at night, they can knock the tin. I won't knock the hatch."

**If you send him away**
"I'll be at the weighbridge until the window. After that I'll be on the ice. After that I'll be in the Office with an incomplete return. Incomplete is a status. I don't like it. I will still file it."

### Monologue (once) `[VO]`

"I was scored 60.4. The band is allocated. The band is also the first one they look at when a column is light. I know what I am for. I am for names, occupations, dependent counts, and dates of birth written once, correctly. Convoy 12 was held over a year written twice. I have that in a training example. I copied it in school in the Cluster. I did not know I would stand at a hatch that is that number. I am not here to take your people because I want them. I am here because the return is open and I have never closed a Sector 4 file, and because if I don't stand in the ash, someone with a louder voice will, and they will still use my form. I can read it again. I can read it slower. I cannot make the ice thicker. When you tell me to go, I will go as far as the boom. I will not come in unless you say. That is the only rule I have that isn't printed."

### threateningBodyText pair

**Neutral**
Edor stands off the weigh-plate with the pink copy folded against the wind. He asks if you want the heading first or the names. He says most people want it read again. He waits.

**Threatening**
He is still off the plate. He does not offer the heading. He says the white copy has already gone north. He says he can read you what it will sound like when it comes back. He still does not step onto your threshold.

---

## 2.3 `npc_leva_quist` — Shift Lead Leva Quist

**Where:** `loc_salt_grade_hut` / Membrane Hall
**Was:** Municipal RO technician. Never allocated — in situ essential.
**Will not:** Shut the plant to spite Ormund.

### Voice rules

**Do:** Count out loud. Call the allocated "the indoors." Name drums, seats, hours, millimetres of integrity. Dry. Technical. Correct people who say "abandoned." Say "the membranes don't care."

**Don't:** Speechify about fairness. Shut the plant as leverage. Call herself a Hydro-Baron unironically (she will accept the Toll's joke once, not twice). Beg. Whisper. Pretend the count isn't short.

### Barks

**First meet**
"They scored high enough to be continued. The membranes don't care. I need four people on the outfall by morning or the indoors freeze in their numbers. You can put the resin on the table. I can count it while you talk."

**Ice Road**
"I don't walk the Cut unless a drum has to. If a drum has to, I want Yara's lit hours and I want the yellow copy. I don't argue with ice. I argue with people who think steam is a policy."

**Levy**
"If he named plant hands, send plant hands. If he named clerks, I still need plant hands. I will not shut Hall 2 to make his rota prettier. One, two, three — that's a shift. That's not a philosophy."

**Refuse (player refuses levy)**
"Refuse him. Don't refuse the outfall. Those are different refusals. I can use unlisted. I cannot use a speech."

**Membrane**
"Pressure drop. Bank two. Integrity is a number I can show you on a gauge that cracks if you tap it. Forty-eight hours until the substation is a thermometer. I need resin, brass seats, iodine, two bodies who will stand the apron. Count with me. One drum. Two. We're short. We're always short. That isn't theft. That's Tuesday."

**Hatch**
"I won't stand at your hole. I have a plant. If he takes people from you, send them to me, not to a classroom. I will give them a whistle. The whistle is the limit. They will not like the limit. The limit is why they still have skin."

**Trust-high**
"You brought seats and you didn't tell me where from. Good. I didn't ask. Motion: that we keep running. That's Volume 12. You can read it. You can take it if the trip is coming and I can't leave the hall."

**Trust-low**
"Don't tour the hall if you're here to inventory my people for him. The dipper stays on the nail. You don't look like a person who knows what spent feels like. Don't find out on my shift."

**In situ**
"Hour Zero I was on the bank. They filed me essential instead of scoring me. I have opinions about that. The opinions don't turn valves. Four on the apron. Two in the hall. One on the count because the count is short."

**School (quest)**
"The indoors asked me to speak about water. I have never been in that room. If you walk me there I will tell the children the membranes don't care. The Office will hate that. The children can add it up themselves."

**Brass**
"Eight fittings. I don't ask which playground. I don't ask which tin. If you bring none, I schedule a leak. Scheduled leaks are still leaks. The indoors will feel it in a night."

**Spent stack**
"Don't touch that with bare hands. If you believe in recoating, I have a jig and a yield that will disappoint you. If you don't, write it off. Writing off is a kind of running."

**If steam dies**
"I did the forty-eight hour math when I still had hair that wasn't white at the temples. I will not do it as a threat. I will do it as a countdown. Bring me people or don't. The canal wheel is red for a reason."

**Process water**
"You can drink it. You will want salts after. Iodine after that. If you skip the iodine I will still need you on the apron, and I will count you as a problem, not a worker."

### Monologue (once) `[VO]`

"I will count this once so you hear it in order. Intake. Bank. Resin. Iodine. Heat. Outfall. Canal. Substation. Indoor air. That is the plant. The Office thinks the plant is a building that makes the Cluster possible. The plant is a series of numbers that go down. Tuesday the drums are short and it is not a thief. It is steam and a spent stack and a jig that yields low. They allocated water engineers into holes. I was already here, so I did not get a hole, I got a notice that said do not reallocate this post. I have not been reallocated. I have also not been replaced. If you strip Sector 4 to keep my bank up, Frayne will feel it in fittings and the Verge will feel it in thirst, and I will still need the bank up, because two hundred and eleven people have not practised being cold. If you let it drop I will stay in the hall until the gauges stop lying. I will not spite him with a shutdown. Spite is not a pressure rating."

### threateningBodyText pair

**Neutral**
Leva does not get up. She points at the integrity chart with a knuckle. She counts the drums you can see. She tells you which number is the difference. She asks if you are here to work or to watch.

**Threatening (`faction_the_office` low, or levy hostility spilling onto Salt)**
She still does not get up. She tells you the hall is a controlled space. She tells you auditors wait outside. She tells you if you are here with his paper, you can put it on the table and not on her people. The wrench on the peg is warm.

---

## 2.4 `npc_yara_holm` — Cutter Yara Holm

**Where:** `loc_cut_waystation_a`
**Was:** Harbour ice-pilot. Score 44. Unlisted. Hired because the allocated would not go out in year one.
**Will not:** Guide a column onto ice she has marked dark. Blast.

### Voice rules

**Do:** Short sentences. Distances. Hours. "Dark" and "lit" as moral words. "I don't open it for you. I open it." Write accidents in the book. Name kilometres.

**Don't:** Please. Or else. Speeches. Meet Ivy if you can avoid it (if they meet: agree, don't like it). Call blasting brave. Comfort. Explain her feelings.

### Barks

**First meet**
"I don't open it for you. I open it. If it's dark, you wait. If you don't wait, I write the accident in the book and I don't fetch you."

**Ice Road**
"Window is fourteen days this freeze. Lit hours are posted on the boom. UV at midday. Welders' glass or you go snow-blind and walk dark without meaning to."

**Levy**
"If you're putting three on my ice, they walk where it's lit. I don't care whose names they were yesterday. I care if they step where I put a stake."

**Refuse (player refuses Office)**
"Your fight with the indoors is not a lamp. I will still light it. I light it. If he sends a column without me, that is his accident."

**Membrane**
"I don't do resin. I can keep a drum from going through at Accident 12's cousin. That's the job. Don't ask me to hurry a thaw."

**Hatch**
"I won't stand on your step. If they come south on my window I will mark the road. If they come south on a dark I will not mark it. That is not help. That is the book."

**Trust-high**
"You waited. That's rare. I'll put you on the overnight. A3 is the least wet. Don't take the regulator off the stove. I notice mass."

**Trust-low**
"You're walking like a person who thinks thick is the same as lit. It isn't. Stay off my stakes. If I find one in your pack I will not raise my voice. The beacon will be dark."

**Dark lamp**
"South Beacon is down. Road is closed on that stretch. Relight if it's oil. Leave it if it's a trap. I know the difference. Don't test me for Ivy's exception. We don't do exceptions. We do dark and lit."

**No blast**
"Charges on the Shelf are a way to make a hole that does not care who is waiting in it. I will not guide that. Take the Ridge. Hours and cold. Alive."

**Kilometre 19**
"I don't cross. She doesn't cross. You can carry oil south. Bring a receipt. Don't ask us to stand on the same side of the post and like it."

**Accident 12**
"Column went through in year three. Cargo still says ALLOC-7. You can salvage a tin or bury the letters. If you bury them, say so. I write what I see."

**Foghorn**
"If you silence 8, I lose the coast in weather. So does the tender. Don't steal a timer to be quiet. Quiet is how columns vanish."

**Waystation winter**
"Closed window. Stove or you walk brine. I will check the notches on the filter. Eleven. Don't invent a twelfth with a file. I will see the bright metal."

**If you blasted anyway**
"I didn't raise my voice. The beacon is dark. Find your own kilometres."

### Monologue (once) `[VO]`

"Year one the allocated would not go out. Some of them died of that, indoors, which they thought was the safe direction. I was unlisted. Score forty-four. They hired me because a harbour pilot can read ice that is lying. I mark dark where it is wrong, not where I dislike you. Ivy lights for whoever is walking. I open a road that only exists if the freeze is honest. Those are the same spine in two districts and we should not be made to shake hands. If you walk dark I will write your mass in the remarks, which is the column for the dead, and I will not fetch you, because fetching is how I die on a lie. If you blast the Shelf I will take the lamps down the way she would, one a night, eleven days, and you can explain to the indoors why their water sits in drums they cannot move. I don't open it for you. I open it. Wait."

### threateningBodyText pair

**Neutral**
Yara stands by the boom with the axle ledger open to today's date. She looks at your load, not your face. She says whether the next stretch is lit. She waits for you to wait.

**Threatening (`faction_the_cutters` withdrawn / dark-mark)**
The boom is down. The ledger is closed. She is not in the hut. The beacon up-road is dark in daylight. A stake with black cloth leans on the container as if someone had finished explaining.

---

## 2.5 `npc_halden_mire` — Sparks Halden Mire

**Where:** `loc_shelf_hearth4`, then ashore
**Was:** Fleet radioman. Five years of schedule.
**Will not:** Open the hatch for unauthenticated boarding. Treat blasting as a plan.

### Voice rules

**Do:** Radio procedure. "Say again." Dead air as punctuation. Distinguish hearing from stand-up. Ask to see forms. Be interested, not angry, when Sole's paper fails. Count nights on frequency.

**Don't:** Chatter. Swear for colour. Call the player 'friend'. Open for curiosity. Treat D/9 stand-down as ship stand-down. Make speeches about waiting as virtue.

### Barks

**First meet (radio)**
"Schedule, this is Hearth-4. I can hear you. That is not the same as a stand-up. I need a stand-up. Say again your number."

**First meet (ashore)**
"I came down the ladder because you answered on time. Answering on time is not authentication. Say again why you are on my ice."

**Ice Road**
"I don't keep the Cut. I keep a watch. If Yara says dark I believe her. Ice is a kind of static."

**Levy**
"I don't take labour. I take a stand-up or I don't. If the Office wants beds they can say beds. Beds are not a frequency."

**Refuse**
"Say again. … Copy refusal. Logged. Hatch remains shut. I am still on schedule."

**Membrane**
"If the plant dies I will hear the Cluster change. I will not open because they are cold. Cold is not a stand-up. Say again if you want me ashore as a person. That is a different request."

**Hatch (*Hearth-4*)**
"Hatch wants a number. Allocated companion or the lamp stays a lamp. Blasting party is murder in my log. I have a lot of refusals. I can show you. I would rather you didn't make another line."

**Trust-high**
"I will come ashore if you ask me as a person. I will bring the pad. The pad will not get happier. I will still come."

**Trust-low**
"You sound like a blasting party. Say again without the charges. … Nothing heard. Out."

**Sole's form**
"I can look at it. … Negative verify. Same family. Wrong door. I am not angry. I am interested. Land paper does not stand down a ship. Say again if you thought it would."

**Foghorn**
"I navigate by 8 in weather. If 8 is silent I am a hull in a field. Don't take my coast to hide."

**Authenticator**
"Green is not welcome. Green is current. Current is not an order. I have had current for five years."

**Beds**
"If we come off, the Cluster votes on bunks. I don't vote. I log. Logging is how I know we were here."

**If player blasts**
"… All stations. Charges. This is not a stand-up. This is a hole. Out."

**After D/9 comparison**
"Anneke's pad and mine went to school together. They did not graduate together. I would like to see her paper anyway. Curiosity is not authentication."

### Monologue (once) `[VO]`

"I will say this on the hour because that is when I say things. We were told to wait for a stand-up that used the same authentication family as the land pads. We waited. Some went ashore in year two and became indoors. We did not. The lamp is still green. The ice is at the mark. I have logged every refusal so that when the order comes I can show the order that we were consistent. If you have a number that works I will open. If you have a form that worked on a different door I will file that it did not work on this one, and I will not be angry, because anger is not a checksum. If you put charges in the ice I will stop being a watch and start being a casualty report, which I have practised writing for other people. I can hear you. Say again if what you want is a person on the ice. That I can do without a stand-up. The hatch I cannot."

### threateningBodyText pair

**Neutral**
A voice on a fixed frequency, on the hour. It asks for a number. It waits a full second after you speak. It says *say again* if you rushed.

**Threatening (blasting intent known / Fleet watch hostile)**
The same hour. The same frequency. It names charges. It names the log. It does not wait the full second. It goes out while you are still talking.

---

## 2.6 `npc_sela_renn` — Sela Renn *(existing spine; Holdfast overlay)*

**Where:** player shelter if admitted; Cluster Clinic for the claim
**Will not:** Pretend a drawer is a father. Pretend iodine is the same as staying.

### Voice rules

**Do:** Short. Concrete. Age thirteen, not cute. Name school, iodine, numbers, tunnels, kit. Let her choose. Do not ventriloquise adult morals through her.

**Don't:** Make her a mascot. Make her thank the Office. Make her give speeches about humanity. Force her to comfort the player.

### Barks

**First Holdfast meet (if already in shelter)**
"They have a school. They have iodine. They have my father's number in a drawer. That isn't the same as having him."

**Ice Road**
"I walked further than this in the dark. This is marked. I can walk marked. I don't like the chains on the swings. That isn't walking. That's just true."

**Levy**
"If they name me, say it where I can hear. Don't do the quiet adult thing. I already know about lists."

**Refuse**
"If you keep the hatch shut I will not bang on it from inside. I banged on enough doors that didn't have us on them."

**Membrane**
"He built kits for water that wasn't a plant. If they want that, they can say they want that. If they want me because of a number, that's a different wanting."

**Hatch (Office escort)**
"Second time someone has stood out there with paper. I can hold the card. I can not hold it. You decide the door. I decide where I stand after."

**Trust-high (player honoured her people)**
"You let them stay. I remember that when they talk about dependents. Dependents is a word that means me and doesn't mean them."

**Trust-low**
"Don't trade me for clinic keys. I can hear a trade even when you don't say trade."

**Clinic claim**
"They can put me in Block C under his number. I would have a plate, not a tag. I would have a school that teaches the score. I would like the autoclave. I would not like the arithmetic."

**If she stays Cluster**
"Tell the duty roster I'm not on it. Don't leave a blank like I might come back for a shift. I won't."

**If she refuses Cluster**
"I know the tunnel better than their stairwell. I know which kit was his. They can keep the iodine. We can steal some first. I'm not joking. I'm counting."

**Playground**
"The seats are gone. Brass. I know what brass is for. I'm not putting my hand on the chain."

**Edor**
"He offered to read it again. He's the first one who asked if I wanted the heading or the name. I wanted the name. It was spelled right. That doesn't fix anything. It was spelled right."

**Halvard's notes**
"His writing gets smaller. The diagrams don't. If they call it salvage I will leave the room. If they call it engineering I will stay in the room. That's the whole test."

**Teacher**
"Wren can sit in the back. I will sit in the back. We don't have to talk. I don't want to watch her learn the line at sixty."

### Monologue (once) `[VO]`

"I was eight when the card said this room was ours. I was nine in a maintenance level that had no stores. He died when I was nine, which is a sentence I can say without stopping now. The adults who walked me here are not on the list. I know what happens if the list is honoured as written. I am not going to pretend I don't. District 8 has a clinic that can put me under his number and feed me iodine and teach me why a caretaker scored nine. That is a kind of safety. It is also a kind of being filed. I don't hate the clerk. He reads things twice. I hate the drawer. If you let me hear the claim I will choose. If you choose for me I will still live. I will just know you did the adult thing. The duty roster in our hole is blank. Don't write me in a Cluster hand if I'm sleeping here. Don't leave me in your hand if I'm not."

### threateningBodyText pair

Not Office-threatening in the same way; use only if the Clinic/Office claim is pressed against her will.

**Neutral**
Sela stands where she can see the hatch and the roster. She names school, iodine, the drawer. She waits for the sentence that is hers to finish.

**Threatening (claim without her hearing)**
She is already at the Clinic glass. She heard it from a nurse. She asks if the trade was the formulary. She does not stay for the answer if you lie.

---

## 2.7 `npc_cluster_teacher` — Cluster teacher *(unnamed)*

**Where:** `loc_cluster_school`
**Will not:** Stop teaching the rubric. Raise their voice. Call the unlisted *guests* in front of the children (they say `visitors` or `unscheduled`).

### Voice rules

**Do:** Speak like arithmetic hour. Name points, dependents, the line at sixty. Be calm. Allow correction as if it were a working. Address Wren, if present, without forcing her to answer.

**Don't:** Preach. Apologise for the formula. Invent a personal name on the fly (keep unnamed). Mock unlisted trades. Make the player a guest lecturer unless Leva's quest is on.

### Barks

**First meet**
"We are on occupation points. A dependent is worth points. Write it in the margin. That's how the sum works. Visitors may sit. Visitors may not take the yellow chalk. We have a graph after."

**Ice Road**
"Windows are a calendar problem. We do those on Thursdays. If you have come from the Cut, wipe your boots. Salt eats the floor."

**Levy**
"If a parent is on a thirty-day line, the child still adds. The worksheet does not pause. I don't pause it."

**Refuse (player argues the sum)**
"You may correct a working. You may not correct the rubric. The rubric is the lesson. The lesson is arithmetic."

**Membrane**
"If steam fails we take coats. Coats are not a unit in the sum. I will still set homework. Homework is how we know the day."

**Hatch**
"I don't attend authentications. I attend the hour. If a new child arrives with a plate, I enter them. If they arrive with a tag, I enter them. The pegs are the same height."

**Trust-high**
"You sat the hour. You didn't make a speech. The children will remember which working you changed. I will too. I won't put it on the board unless you want it on the board."

**Trust-low**
"Unscheduled adults who argue with the line at sixty are a disruption. Disruption is not a point value. Please wait by the UV board. I said please to the children, not to you. Sit anyway."

**Wren present**
"You may sit at the back. You don't have to speak. If you want to know what a thing was for, ask after the hour. I will answer with the name and the points. You may ignore the points."

**Leva visiting**
"Shift Lead Quist will speak about water. Water is not on today's sheet. You will still be quiet. You may write questions in the margin. Margins are for dependents and questions."

**Missing strip**
"The Quad board is not a lesson. If a name on it is a living visitor, that is an Office matter. I don't fetch. I add."

**Playground**
"Recess is indoors when the board is on the red nail. The chains are not a toy. We don't discuss the seats. We discuss the sums."

**If player corrects a sum**
"Show the working. If your working is consistent I will mark it. Consistency is not the same as the rubric. Both can be on the page. Circle which one you meant."

**If player lets the sum stand**
"Then the sum stands. Pack away. Yellow chalk back in the box. The box is counted."

**After school**
"Nineteen coats. Sixteen today. Three on the rota at the plant or the Cut. I don't call that a tragedy. I call it attendance. Attendance is a number I can file."

### Monologue (once) `[VO]`

"I will not take the hour to explain the war. I take the hour to make them able to add what was done to them, so that nobody can tell them it was a mystery. Occupation up to forty. Dependents. The line at sixty. Halvard Renn would have been a full working on the board. A caretaker is not. That is not a story. That is the sheet. If you tell them the sheet was cruel, they will write *cruel* in the margin and still need to add. If you tell them nothing, they will add anyway. Visitors from Sector 4 keep wanting the lesson to be a referendum. It isn't. It is Tuesday. The yellow cultivar is failing and that is agriculture hour, which is not mine. Mine is the sum. When a child asks why their parent has a plate and a visitor has a tag, I say: plates are allocated, tags are paper, both hang on the same pegs. Then I make them finish the column. Unfinished columns are how convoys wait on a date of birth."

### threateningBodyText pair

**Neutral**
The teacher does not look up from the working until the sum is finished. They point you to a back chair. The UV board is on the green nail. Chalk dust on the floor is yellow and white.

**Threatening (Office hostile; school closed to unlisted)**
The door is on the latch. A note in the same hand as the worksheets: `UNSCHEDULED ADULTS AFTER THE HOUR ONLY.` The yellow chalk is not visible. Attendance has been taken. You are not on it.

---

# 3. Main quest stage prose

UI length: 40–90 words typical, 120 max. Choice bodies honour / substitute / refuse and membrane strip / let-drop are mandatory where the beat has them.

---

## `quest_holdfast_the_sheet` — The Sheet That Shouldn't

**Briefing**
Bram Ostrowski sells you a waxed sheet of the estuary. A road is drawn where summer water should be. He will not say who walked it. He will take calories or a favour-by-mass. The sheet smells of lamp oil and fish glue.

**Objective complete**

1. *Bought / copied `item_map_sheet_ice_road`.* The wax takes a fingerprint and keeps it. Channel markers on the sheet do not match the last Sector 4 lamp you know.
2. *Compared to Kittiwake log (if owned).* The launch's hand is eleven days past the Exchange. Ostrowski's hand is this year. The road exists on only one of them. The ice has not been asked.
3. *Asked a Lamplighter about Kilometre 19.* Ivy confirms the post. She does not confirm the road. She does not cross.
4. *Survived the asking.* No exception was requested. The ledger still shows 19 lit. You have a fragment and a direction that is only a direction in winter.

**Failure / timeout**
The window after you saw the sheet closes without a crossing. Ostrowski will sell the copy again. The wax will be thicker. The road will still not be there in summer.

**Choice bodies** *(sheet handling)*

- **Pay Bram's price:** He wraps the sheet in the same paper he uses for maps that exist in all seasons. He does not wish you luck. He wishes you a freeze.
- **Copy and leave the original:** Your copy smears at Kilometre 19. His does not. He notices. He does not raise the price. He files you as a person who copies.
- **Refuse the sale:** He puts the sheet away. The estuary remains a rumour with a smell. Ivy will still light 19. Yara will still open a road you cannot prove.

---

## `quest_holdfast_the_clerk` — The Return

**Briefing**
A man stands off the weighbridge plate so he will not be charged as cargo. Census Clerk Grade III Edor Vale. A return in triplicate. Three of your people are already written, occupations wrong by one each. He offers to read it twice. The Tollman charges for the introduction.

**Objective complete**

1. *Heard the form.* Heading, occupancy, reconstruction pool, ice window. He did not skip a field. You may ask him to start again.
2. *Confirmed or denied three occupations.* Adjacent errors. Mason / caretaker. Clerk / clerk-grade. Vet assistant / vet. He corrects in the same hand. He thanks you for the truth without using the word.
3. *Hatch wait: allowed or refused.* If allowed, he keeps off the step with a stove-tin. If refused, he goes as far as the boom.
4. *Sela's card (optional).* He copies the number. He does not take the laminate. UNCONFIRMED until someone who can confirm, confirms.

**Failure / timeout**
He files incomplete. Incomplete is a status. The next time you see him the white copy has already gone north. He still offers to read the pink one.

**Choice bodies**

- **Let him wait near the hatch:** He sleeps in the ash with the tin. Survivors who cannot sleep will knock the tin, not the hatch. In the morning the pink copy has dew in the folds.
- **Send him to the boom:** He goes. He does not call it a refusal. Ormund will. The weighbridge keeps his stool for him.
- **Show the card:** He writes the number as if it were a date of birth: once, slowly. He does not look at Sela as a line item while she is in the room. He looks after.

---

## `quest_holdfast_the_window` — When the Cut Takes

**Briefing**
Yara Holm at the Gate. The boom is up for a freeze that has a length. Fourteen days this window. Outfit three: warmth, iodine, food, welders' glass. Lit hours on the board. Dark ice is not a metaphor.

**Objective complete**

1. *Column kitted.* Glass on faces. Iodine in a tin that will freeze shut if you leave it in a pocket. The axle ledger takes your mass.
2. *Waystation A reached.* Four bunks. Filter notches. Stove regulator in place. Home still ticking.
3. *Dark ice not walked (or walked).* If you waited: she marks you rare. If you did not: remarks column, and she does not fetch.
4. *Returned or wintered A4.* The last bunk is wettest. The window does not care which you chose. The ice will.

**Failure / timeout**
Thaw while you are north: brine at minus twenty, or Nomi's boat at a price that is a side. Stuck-north is a status. The waystation must hold.

**Choice bodies**

- **Wait out a dark stretch:** Hours and cold. The beacon stays honest. Yara does not thank you. She puts you on the overnight.
- **Walk marked-dark:** The ice is thick. Thick is not lit. An accident is written whether or not you are in it.
- **Winter the last bunk:** You spend the closed window on A4. Filter at eleven. Do not file a twelfth notch. Home degrades without you.

---

## `quest_holdfast_the_plant` — In Situ Essential

**Briefing**
The desalination plant is not abandoned. It is staffed, failing, named. Leva Quist's minutes are current. Steam visible toward a place with numbers. Resin is a gift or an insult depending on how you set it down.

**Objective complete**

1. *Grade Hut entered.* Volume 12 open. Motion: that we keep running. Carried. A failed valve-seat for a paperweight.
2. *Membrane Hall toured.* Fume at chest height. Tuesday's difference on the drum rims. The dipper stays on the nail unless you steal it.
3. *Resin: delivered or refused.* Delivered: she counts aloud. Refused: she counts anyway. The count is short.
4. *Steam line seen.* Canal wheel painted red-white-red. Cluster on the far end of a pipe. Not a rumour.

**Failure / timeout**
You leave with the Sector 4 word still in your mouth. The plant does not notice. The next auditor does. Trade stays locked.

**Choice bodies**

- **Gift virgin resin:** She does not say thank you. She marks a drum. Salt trade opens like a valve, which is to say: slowly, with a gauge.
- **Refuse the gift:** She needed it anyway. You are a visitor who watched. Watching is not a shift.
- **Ask about abandoned:** "That was your word. Ours is Municipal Desalination 8. The stop order didn't come. We didn't stop."

---

## `quest_holdfast_authentication` — Take a Number

**Briefing**
Cluster Gatehouse. Cream paint. A keypad under plastic. They will ask for an Allocation number. Twelve is a known discrepancy. They have a procedure. Guest housing is Block C. The Quad has chains and no seats.

**Objective complete**

1. *Number stated, or none.* Twelve opens a file. None opens a different tab of the same binder. Both are procedures.
2. *Block C accepted, or Gatehouse floor.* Metal plates versus paper tags versus tiles. Steam or no steam.
3. *Quad walked.* Four cultivars, two yellow. Missing-persons strip of Sector 4 trades. One of them is yours.
4. *Playground brass: left or taken.* Silent. The chain hangs. The Office notes mass if mass leaves.

**Failure / timeout**
You sleep south of the barrier. The discrepancy file opens without you. Edor will bring it to the boom.

**Choice bodies**

- **Give twelve:** The bell does not ring. A clerk says *known discrepancy* as if it were a weather. Badge: `GUEST / BLOCK C` or `UNSCHEDULED`. Both print on paper.
- **Give none:** They do not invent a number for you. The floor of the gatehouse has a blanket that has been washed. The thermometer still reads like survival.
- **Take a seat / fitting:** No alarm. No speech. Later, a mass note. The chain does not swing. You have brass in the pack.
- **Leave the brass:** Children pass. Adults file you. The yellow leaves tick against the trough.

---

## `quest_holdfast_the_drawer` — The Drawer

**Briefing**
Ormund's right-hand drawer. The Sector 4 Schedule, complete. Names you know. Names you buried. He turns pages with two fingers. He does not ask how you feel about a column.

**Objective complete**

1. *Read Sole.* SOLE, MARGIT J. Records Clerk II. RUR 9. Score 41.2. NOT ALLOCATED. He does not offer to fix her.
2. *Read Renn.* RENN, HALVARD — water engineer — allocated — NOT ARRIVED — 12-B UNCONFIRMED. A dependent line that may be Sela if she is yours to name.
3. *Searched Frayne.* Absent. RUR 11 does not produce an allocation. He will not write her in because you asked.
4. *Asked about 12-C (optional).* He has a string-tied folder. He may show it. He may say it is for a later hour.

**Failure / timeout**
He closes the drawer. You have seen that a second Schedule exists. You have not seen who is missing. Codex dump incomplete. He will not reopen for curiosity.

**Choice bodies**

- **Ask him to read Sole aloud:** He does. Completeness is not execution. He executes. He does not raise his voice at her number.
- **Ask him to read Renn aloud:** He does. If Sela is present he does not look at her until the line is finished. Then he does.
- **Ask why Frayne is absent:** "The Schedule is not a petition." He offers no further sentence.
- **Ask for 12-C now:** "That is a different folder. You may see it after you have slept in a numbered building. Occupancy first."

---

## `quest_holdfast_the_levy` — Reconstruction Pool

**Briefing**
Three names. Thirty days. Occupations as scored or as observed. The ice will not wait for a better feeling. Kit for salt and UV if they go. Tell the people who stay. The duty roster will have holes.

**Objective complete**

1. *Named survivors reviewed.* Wrong-by-one occupations corrected or left. Faces. Tools. Who still has skin that will hate brine.
2. *Honour / substitute / refuse.* Flags. No combat. The next paragraph of the form.
3. *If sending: kitted.* Glass, iodine, salve, calories. Axle ledger. Yara's lit hours.
4. *Remaining shelter informed.* Morale as a quiet room, not a speech. A blank on the roster where a name was.

**Failure / timeout**
Silence past the window is refusal by the ice's clock. Edor waits. Lamps-out cousin in eleven days if Cutters are later withdrawn. Beat 9 accelerates.

### Choice bodies (mandatory branch)

**`holdfast_levy_honour`**
You send the three as written. Calories and medicine will come north-to-south on a rate the Office calls regular. The three take Ice Road fatigue and salt-rash risk. Cluster trust ticks up. The people who remain eat easier and sleep worse. One of the three may refuse to return. That refusal will also be filed.

**`holdfast_levy_substitute`**
You send three other people. Ormund notes irregular. Yara respects that the ice got three who would walk lit. Edor does not: the names are wrong, and names are his job. Later, an audit. Possibly a second levy. The roster at home still has holes, just different holes.

**`holdfast_levy_refuse`**
You refuse in writing, or by not writing. No shots. Edor waits in the ash. Ice Road access withdraws after eleven days if the Office asks the Cutters to treat you as dark. Ormund does not say or else. The next form is 12-C. Threatening prose unlocks on Office scenes. The three named people hear that they were named.

---

## `quest_holdfast_the_membrane` — Forty-Eight Hours

**Briefing**
Bank two trips. Cluster steam clock starts. Leva has the forty-eight hour math in her mouth without making it a threat. Resin, brass, iodine, two workers, an outfall shift. Sector 4's thirst and District 8's thirst are one job until they are not.

**Objective complete**

1. *Diagnosed.* Gauge, difference, canal wheel, substation thermometer. Not ideology. Valves.
2. *Gathered.* Resin drums, brass seats, iodine, two bodies who will stand the apron.
3. *Outfall shift.* Health, salt, the clipboard that is ceremonial until you make it not.
4. *Strip / local salvage / let drop.* The indoors will feel it or the Verge will, or both, or neither in time.

**Failure / timeout**
Forty-eight hours without a decision is a decision. Indoor °C interpolates toward outdoor. 211 people enter a cold they have not trained for. Not a cutscene. A week of bad decisions.

### Choice bodies (mandatory branch)

**`holdfast_membrane_sector4`**
You strip what Sector 4 can spare: Rebuilders brass, iodine, filters. The bank holds. Cluster lives. Allotments thirst clock shortens. Frayne's minutes record a shortage without naming you. Medical market shock. The playground chains do not get their seats back. The tin behind your filter, if you raided it, is lighter. Nobody mentions it.

**Local salvage (if offered)**
Spent stack, recoating jig, Salt's hidden iodine key. Yield low. Integrity to forty percent if the apron is stood. Nobody in the Verge goes thirstier today. Tomorrow's Tuesday is still short.

**`holdfast_membrane_let_drop`**
You let steam die. 211 people who have not practised this cold. Office legitimacy cracks. Salt may offer a separate bargain — plant over paper. Unifier path hardens because a treaty wants a room that can still hold heat. Children take coats. Attendance is still taken.

---

## `quest_holdfast_the_second_list` — Order 12-C

**Briefing**
Reconstruction Order 12-C: unlisted occupants of authenticated Allocation 12 are a labour reserve. Published. Nobody in Sector 4 had a copy that survived. Ormund has one. He will come south when the ice allows. You may carry a copy to Sole. She will file it. She will not sign it. Voss will want the pool.

**Objective complete**

1. *Copy obtained.* `item_order_12c`. String tie. Civil-service present tense. No please. No or else.
2. *Sole (optional).* Drown boat. She files. She does not sign. Completeness versus execution, same noun, opposite people.
3. *Voss (optional).* He wants the pool. He will call it conscription and mean it as a compliment to himself. Intercept risk on a levy column.
4. *Hatch prepared.* Roster. Temperature. Whether anyone writes a name. Whether the outer dog is thrown.

**Failure / timeout**
He comes anyway. The copy you did not take still exists in his drawer. Threatening prose is already live if you refused the levy.

**Choice bodies**

- **Carry 12-C to Sole:** She reads it twice, which is not Edor's habit, it is hers. She blots the date. She does not blot the refusal. `item_sole_unsigned`.
- **Show Voss:** He wants names. He will not pay the Office's rate. He will pay in patrols at the Gate. The Cutters will not like the patrols.
- **Show neither:** The paper lives in your pack like a spare filter. It does not clean anything. It is still a paper that moves people.
- **Prepare shut:** Forty days quiet. A receipt in the ash, cousin to a card in a freezer bag. Different district. Same temperature.

---

## `quest_holdfast_the_hatch` — The Claim, Reversed

**Briefing**
Forms at the outer hatch. Escort in faded Continuity jackets. Temperature. The game stops talking. This is not Sela's arrival, or it is Sela's arrival *and* this. Open or keep shut. Authenticate, house, or levy. Or wait forty days. Write on the duty roster, or do not.

**Objective complete**

1. *Open or shut.* The dog on the hatch is a fact. So is the quiet.
2. *If open: authenticate / house / levy.* Numbers, Block C tags, three names. Edor asks to speak first if he is there.
3. *If shut: forty days.* No combat. The escort remains on a rota you cannot see. Then a receipt.
4. *Roster.* A name in a Cluster hand, a name in yours, or a blank that is also a decision.

**Failure / timeout**
Leaving the beat un-resolved through a second window is shut-by-ice. History second paragraph still writes. It writes incomplete.

**Choice bodies**

- **Open, honour terms in part:** Some of yours live numbered in Block C. The bunker is easier to feed. The roster has names that did not sleep there last winter. (`ending_holdfast_schedule` adjacent)
- **Open, 12-C enforced:** Columns both ways. Receipts in triplicate. Nobody is shot. Sela claimed as dependent if present and if she heard it. (`ending_holdfast_reserve` adjacent)
- **Keep shut:** District 8 continues. Forty empty apartments stay empty. Edor's return may be found later, incomplete, in a good hand. (`ending_holdfast_dark_road` adjacent)
- **Write the roster:** Ink on a chart that was always blank. The people who see it in the morning will know which sentence you believed.
- **Write nothing:** The chart stays honest about who this hole was built for. It stays dishonest about who kept it.

---

# 4. Side quest hooks + resolution text

Giver line / completion / one complication. UI length.

---

## Salt — `hydro_barons`

### `quest_salt_resin_count`

**Giver (Leva, Membrane Hall)**
"Tuesday. Short again. Don't look at my people like that. Follow the night shift. Count with me. One. Two. The difference is a drum that was always light, or steam, or a stack that grows. I want which."

**Completion**
Evaporation, not theft. Recoat at low yield or write off. Two virgin drums released because the books now match a physical truth. Recipe `recipe_resin_recoat` in grease-pencil on a jig.

**Complication**
The night shift is an allocated clerk who volunteered to be useful. He skimmed a cup for a child's thyroid and put it in the Clinic, not a pack. Leva will not call it theft. Ormund will want it in remarks. You choose which book gets the cup.

### `quest_salt_outfall_limit`

**Giver (Grade Hut minutes)**
The clipboard says six hours. The names below the line are in the same hand as the names above it. Salt-rash is up. Bring a protocol from Cluster Clinic or from Ianov. Stand one limited shift. The unlimited one is optional and is not.

**Completion**
You stood the apron. Iodine protocol posted in the sleeve. The next names stop below the red line, for a week. Antiseptic. Clinic and Salt disagree about whose iodine it was.

**Complication**
Unlimited shift: more integrity, more skin. A worker asks for your pills and will not rob you if you explain. If you refuse without explaining, they will still not rob you. They will hate the clipboard more. Friction is a number on two rotas.

### `quest_salt_brass_seats`

**Giver (Leva)**
"Eight fittings. I don't ask. If you bring none I schedule a leak. Scheduled is still a leak. The indoors feel it in a night."

**Completion**
Seats in. Steam stable. Nameplate flag silent if the tin got lighter. Works price shock if Frayne's board and this board wanted the same metal in the same week.

**Complication**
A fitting still has Quad paint in the thread. Leva sees it and does not comment. An auditor sees it and comments. Mass note on the Quad. You can return one seat to a chain. The chain will not swing. The leak schedule will.

---

## Office — `faction_the_office`

### `quest_office_missing_strip`

**Giver (Quad noticeboard)**
A strip of Sector 4 trades. Caretaker, clerk, vet, ice-cutter, lamp. One name is a person eating in your shelter tonight. The paper is pinned with a brass tack. The tack is a fitting.

**Completion**
You matched it. You told them, or you didn't. If you told, a retrieval is filed, polite, with a window. Codex. Morale splits along who wanted to be found.

**Complication**
The living name is adjacent-wrong, Edor's kind of wrong. Telling sends a column for the wrong occupation. Not telling leaves a person listed as missing while they wash your dishes. The tack remains. You can steal it. It is brass.

### `quest_office_school_sum`

**Giver (`npc_cluster_teacher`)**
"A dependent is worth points. Sit the hour. Correct a sum or let it stand. If the small one is in the back, remember what you tell her. I will."

**Completion**
You sat. You changed a working or you didn't. No items. Wren truth flag if she was there. Morale split: allocated children vs unlisted adults in your column.

**Complication**
Correcting the sum in front of nineteen children is a referendum the teacher will not allow to be a referendum. They will mark *consistency* and circle the rubric anyway. Leva, if this is her visit day, will say the membranes don't care, which is not on the sheet.

### `quest_office_forty_rooms`

**Giver (Ormund)**
"Forty apartments held for arrivals. Dusted. Walk three. You will find boots. Sizes one through four. You may leave them. You may not call them unclaimed. They are claimed by a timetable."

**Completion**
Three rooms. Dust that is maintained. Boots that mirror your crate. Knowledge `lore_hf_forty_rooms`. Warmth items if taken. Morale if left.

**Complication**
In the third room a paper tag already has a Sector 4 occupation from your shelter, written in a hopeful Office hand. Burn, return, or keep. Keeping is a census threat. Burning smells like the Quad paint.

---

## Cutters — `faction_the_cutters`

### `quest_cut_dark_lamp`

**Giver (Yara)**
"South Beacon is dark in a window. That's Accident 12's cousin. Walk the stretch. Relight if it's oil. Leave dark if it's a mouth. I know the difference when I see your face after."

**Completion**
Relit: road safety, oil gone. Left dark: stretch closed, ice thick, honest. If you relit for a trap, she withdraws. Eleven days of lamps out. Corrigan rule, northern.

**Complication**
Undertow grammar on the dark ice: "lucky we were close." Cargo offered back at a discount that is not a discount. Proving them is a different life. Yara will not testify. She will mark.

### `quest_cut_accident_book`

**Giver (Weigh Hut)**
The book is a column that went through in year three. Cargo `ALLOC-7 / NOT FOR GENERAL ISSUE`. Read it. Salvage a crate or bury the stencil. Tell Cluster or Salt.

**Completion**
Tins, or a buried marking, or both if you are greedy and fast. Faction delta. Yara writes what she sees. If you bury, say so.

**Complication**
Opening a tin in Sector 4 drops morale: the issue is you. Opening it on the Cut is calories. Telling Cluster sends an auditor to a hole in the ice. Telling Salt sends a drum-sled. The glove on the handle should not be pulled.

### `quest_cut_no_blast`

**Giver (Yara, Gate)**
"Someone wants charges for the Shelf. I will not guide that. Ridge is hours and cold. Alive. If you blast anyway I don't raise my voice. The beacon goes dark."

**Completion**
Ridge taken: Icebreaker path without 100 explosives, fatigue bill, Cutter trust. Blasted: `item_yara_dark_mark`, access destroyed, disaster encounter at the tender, Mire's log gains a line.

**Complication**
The Office will pay for a shorter road to *Hearth-4*. Paying is not Yara's yes. If Edor is with you he will offer to read the refusal again. It will not change.

---

## Fleet — `faction_the_fleet`

### `quest_fleet_schedule`

**Giver (Foghorn 8 / radio)**
A voice on a fixed frequency, fixed time. Not D/9's. Confidence in the sixties. Listen three nights. Answer with a number or without. Meet Mire ashore, or don't.

**Completion**
Contact without a ham exploit. Companion unlock if you asked for a person, not a hatch. `Victory_Icebreaker` now has a hull with a watch.

**Complication**
Answering with a fake Allocation number puts a refusal in his log and a note in Ormund's. Answering with twelve puts discrepancy on a ship pad. He will be interested. Interested is not open.

### `quest_fleet_pad`

**Giver (Mire, *Hearth-4*)**
"I need a stand-up. If you have land paper, show it. Say again if you thought the doors were the same."

**Completion**
Sole's form shown: negative verify. Fleet annex in Ministry files: same family, wrong door. Some waits do not end. Mire will still come ashore if asked as a person. No stand-down.

**Complication**
Voss wants the pad. The pad does not conscript a ship. D/9 comparison (Anneke) is allowed as curiosity. Curiosity is not authentication. Mire will log the visit.

### `quest_fleet_boarding`

**Giver (Mire)**
"Boarding without blasting. Hatch wants a number. Inventory the living. Offer Cluster beds or leave us. Beds are a vote I don't vote on."

**Completion**
Allocated companion authenticates, or fail closed. Living counted. Ending_tender progress. Calories cost if they come ashore. Cluster Quad gains coats on pegs.

**Complication**
One of the living is an allocated deserter who ran south last year and ran back. Voss would call them a prize. The Cluster will call them occupancy. Mire will call them a bunk number.

---

## Companions + Sela

### `quest_comp_edor_dob`

**Giver (Edor)**
"There are two years on my return. I noticed late. I would like to sit down. I would like you not to make a joke. Compare it to Convoy 12 if you have that layer. Then tell me which line to strike."

**Completion**
He corrects it, or you leave the error. Loyalty either way, different flavour. If you leave the error he will lie once for you later, omit a name, and hate it. `item_edor_return_self`.

**Complication**
Ormund can see a struck year. A struck year is an irregularity. Leaving the error is a kindness that is also a forged instrument. Edor knows both sentences. He still wants you to pick.

### `quest_comp_leva_ashore`

**Giver (Leva)**
"The indoors asked me to speak about water. I have never been in that room. Walk me. I will tell the truth about membranes. Pack a whistle. Children shouldn't think steam is a story."

**Completion**
She tells the truth. Office unhappy. Children less so. Salt/Office friction. Leva morale up. Yellow chalk used for a pipe diagram that is not on the rubric.

**Complication**
A child asks what score she was. She says she wasn't. The teacher continues the hour. You may answer after. Wren, if present, will remember whichever sentence you pick.

### `quest_comp_yara_south`

**Giver (Yara, Kilometre 19)**
"I don't cross. A lamp is dark on her side. Carry oil. Don't ask Ivy north. Bring a receipt. Two ledgers. That's the whole job."

**Completion**
Oil delivered. Receipt in a Lamplighter hand. Cross-district etiquette. Lamp oil economy tick. `ach_yara_ivy` adjacent. They do not meet. They agree.

**Complication**
Ivy asks who walks the north road. You can say Cutters. You can say District 8. You can say a road that isn't there in summer. She will light 19 anyway. Do not offer her an exception while carrying Yara's oil. Two withdrawals in one week is how a map goes out.

### `quest_comp_sela_clinic`

**Giver (Sela / Clinic)**
They can claim her as Halvard's dependent. Iodine, school, a plate not a tag. Let her hear it. She chooses. If she stays, Allocation 12 loses its water memory.

**Completion**
She stays or she doesn't. Ending modifier. Clinic access if the Office is pleased. Shelter grief if a bunk goes empty without a death. Formulary still gated on levy honour unless you steal a copy, which she may suggest, not joking.

**Complication**
Ormund prefers she stay. The adults who walked with her are still unlisted. Honouring her as written splits that knot again. She already knows. Don't do the quiet adult thing.

---

## Exploration

### `quest_exp_dredger_moth`

**Giver (none / Ostrowski)**
The *Moth*'s charts contradict the sheet. Board. Pay in food. Copy soundings. The stack is still smoking.

**Completion**
Alternate Ice Road spur. Drown navigation bonus. Sounding-lead still greasy. Bram will not admit the other surveyor. The occupant of the *Moth* will sell you both and eat.

**Complication**
The occupant wants a way off the ice more than a second dinner. Taking them south is a mouth on your filter. Leaving them is a chart that will not update when the channel moves.

### `quest_exp_rtg_bruise`

**Giver (none)**
The shine. 85 rads per hour. Tungsten. Tracks. Timed loot. Leave before the dose window. Optional Migration fragment.

**Completion**
`tracks_salvaged`, `tungsten_bar`, map fragment. ARS risk if you stayed to be sure. The helmet with taped glass is not a souvenir.

**Complication**
Sun-Seekers will want the visors. Paying them in glass you need for albedo is a Verge problem on a Shelf clock. The bruise is visible from the Ridge. Visible is not safe.

### `quest_exp_forty_first`

**Giver (none)**
A paper tag in an empty Block C apartment, already written: a Sector 4 occupation from your shelter. Find it. Burn, return to Office, or keep.

**Completion**
Census threat, morale, or a quiet fire in a sink that still has water. The tag curl is the same as yours would be.

**Complication**
Returning it is an admission that the Office guessed right. Burning it is an admission that you saw the guess. Keeping it means a name in your pack that is not a name yet.

---

## Repeatable

### `quest_rep_ice_window_haul`

**Giver (Yara / Office)**
Each freeze: calories north, water and salt south. Three-crate manifest. Lit hours. Weigh in. Fatigue. Dynamic prices.

**Completion**
Cutter credit. Process barrels with a chance to spoil if you take an accident. Home hunger eases. Cluster yellow cultivars get another week.

**Complication**
Warlords smell District 8 traffic (`mutation_ice_road_tax`). The Gate grows a second fee. Yara does not collect it. She will not dark a lamp to dodge it. You wait or you pay.

### `quest_rep_steam_watch`

**Giver (Leva)**
After the membrane is saved: weekly valve walk. One survivor, eight hours. Careful or a leak. Heat credit for the waystation. Salt-rash chance.

**Completion**
Substation rota gains a name that is yours. Tokens in the wooden box. Waystation stove lives through a closed window if you spent them.

**Complication**
Utility AI panic on a watch: a wheel painted red turned because someone was cold, not because someone was told. Repair is a shift. Blame is an auditor.

---

# 5. Radio / foghorn / plant tannoy

Schema matches `radio.json`: `id`, `frequency`, `intelType`, `confidence`, `message`, plus `textFallback` (mandatory — game is text-first). All clips `[VO]` if recorded; otherwise UI ticker + Codex.

Frequencies used here (proposed, non-colliding with 88.5 / 95.4 / 99.0 civilian-emergency-numbers):

| Frequency | Owner |
|---|---|
| 121.5 | Foghorn 8 / Cutters (coast) |
| 156.8 | Tender *Hearth-4* schedule |
| 162.4 | Plant tannoy / Saltworks |
| 164.2 | Office Cluster bulletin |
| 27.12 | Ice Road window (short-range, boom) |

---

### `radio_hf_foghorn_01` — Foghorn 8, on timer
- **frequency:** 121.5
- **intelType:** Civilian
- **confidence:** 0.82
- **message `[VO]`:** "Foghorn 8. Sounding. Visibility not stated. Roadstead unmarked. If you are on the ice, you already know. If you are not, this is not for you."
- **textFallback:** A long tone, then a woman's recorded voice, Cutters' cadence. No please. No or else. The timer clicks after.

### `radio_hf_foghorn_02` — Foghorn 8, silenced
- **frequency:** 121.5
- **intelType:** Emergency
- **confidence:** 0.55
- **message `[VO]`:** "…no sounding. Escapement absent or wound-down. Cutters: treat coast as dark. Tender: treat coast as dark. Repeat: 8 is quiet. Quiet is not weather."
- **textFallback:** Carrier only, then a different voice reading a procedure. Confidence drops because the horn itself is the proof and the horn is missing.

### `radio_hf_hearth_sked_01` — *Hearth-4* on the hour
- **frequency:** 156.8
- **intelType:** NumbersStation
- **confidence:** 0.70
- **message `[VO]`:** "Schedule. Hearth-4. Authenticator green. Ice at the mark. Stand-up: negative. Say again any number. … Nothing heard. Out."
- **textFallback:** Radio procedure. A full second of dead air after "number." Mire. Do not write *lonely* in the UI.

### `radio_hf_hearth_sked_02` — player answered
- **frequency:** 156.8
- **intelType:** NumbersStation
- **confidence:** 0.74
- **message `[VO]`:** "I can hear you. That is not the same as a stand-up. I need a stand-up. If you are a person on the ice, say person. If you are a blasting party, say nothing. Out."
- **textFallback:** Same hour. The dead air is shorter. A clipboard is audible if you are listening for it.

### `radio_hf_hearth_pad` — Sole's paper failed
- **frequency:** 156.8
- **intelType:** Emergency
- **confidence:** 0.66
- **message `[VO]`:** "Land form shown. Negative verify. Same family. Wrong door. Not angry. Interested. Hatch remains shut. Say again if you are asking for a person ashore. That request is different."
- **textFallback:** Logged. The word *interested* is the warmest thing on this frequency.

### `radio_hf_plant_tannoy_01` — shift change
- **frequency:** 162.4
- **intelType:** Civilian
- **confidence:** 0.88
- **message `[VO]`:** "Hall 2. Shift. Four on the outfall. Two on the bank. One on the count. Count is short. That is Tuesday. Whistles mean limit. Limit means skin. Resin gloves on the jig, not in pockets."
- **textFallback:** Leva or a tannoy that learned her lists. Echoes in the nave. No music bed.

### `radio_hf_plant_tannoy_02` — integrity drop
- **frequency:** 162.4
- **intelType:** Emergency
- **confidence:** 0.80
- **message `[VO]`:** "Pressure drop, bank two. Indoor occupants: you will feel this in the pipes before you feel it in the air. Forty-eight hours is a number. Bring seats if you have seats. Do not bring speeches."
- **textFallback:** A klaxon that used to mean evacuation and now means valves. Cluster radios pick it up as weather.

### `radio_hf_plant_tannoy_03` — iodine issue
- **frequency:** 162.4
- **intelType:** Civilian
- **confidence:** 0.77
- **message `[VO]`:** "Iodine store issue: process column first. Thyroid column second. Clinic third. If you are drinking raw, you are not a third. You are a problem on the apron. Salts after. Then pills. Then work."
- **textFallback:** Cage door, two padlocks, a spring-balance zeroing. The tannoy does not name the Salt's copy of the key.

### `radio_hf_office_bulletin_01` — work tickets
- **frequency:** 164.2
- **intelType:** Civilian
- **confidence:** 0.90
- **message `[VO]`:** "Cluster bulletin. Labour rota posted. Guest occupancy Block C is paper tags. Plates are allocated. Both hang on the same pegs. Discrepancy files remain open. Do not queue for bread. Queue for tickets."
- **textFallback:** Ormund does not record these. A clerk who wants to sound like him does. The cream-paint grammar.

### `radio_hf_office_bulletin_02` — levy
- **frequency:** 164.2
- **intelType:** Civilian
- **confidence:** 0.86
- **message `[VO]`:** "Reconstruction pool: three names, thirty days. Occupations as scored or as observed. This is not a request. This is a line. Window length is not negotiable. Ice is."
- **textFallback:** The last two sentences are stolen from the weigh-hut notice. Nobody in the Office considers that theft.

### `radio_hf_office_threat` — 12-C live
- **frequency:** 164.2
- **intelType:** Emergency
- **confidence:** 0.72
- **message `[VO]`:** "Allocation 12 occupancy remains irregular. Quiet interval: forty days from hatch refusal. Escort remains on rota. Completeness is not paused. Execution is scheduled to the next freeze."
- **textFallback:** No raised voice. The threat is the next paragraph. Use as `threateningBodyText` audio twin if VO exists; otherwise this text in the radio panel.

### `radio_hf_cut_window_open` — boom
- **frequency:** 27.12
- **intelType:** Civilian
- **confidence:** 0.84
- **message `[VO]`:** "Gate. Window open. Thickness at threshold. Beacons: lit, except where marked. Axle ledger current. I don't open it for you. I open it. Wait if it's dark."
- **textFallback:** Yara, short-range, clipped by wind. The boom chain is audible.

### `radio_hf_cut_window_close` — thaw
- **frequency:** 27.12
- **intelType:** Emergency
- **confidence:** 0.79
- **message `[VO]`:** "Gate. Window closed. Ice is a lie from Kilometre 19 north. Walk it and I write remarks. Boat is Nomi's problem. I don't fetch. Filter notches: stop at eleven."
- **textFallback:** Same voice. Less wind. The ledger shutting is a prop if you need one.

### `radio_hf_cut_dark` — South Beacon
- **frequency:** 27.12
- **intelType:** Emergency
- **confidence:** 0.68
- **message `[VO]`:** "South Beacon dark. Stretch closed. Thick is not lit. If you relit for a mouth I will know. Eleven days. Find your own kilometres."
- **textFallback:** Withdrawal grammar. Cousin to Lamplighter lamps-out. Do not add a sad violin.

### `radio_hf_shallows_north` — Nomi
- **frequency:** 27.12
- **intelType:** Civilian
- **confidence:** 0.60
- **message `[VO]`:** "Shallows. I run north once this window. Gunwale height. Nobody boards. Pay in a thing that isn't a side. If etiquette is broken I am not at the raft. The Drown closes. That is the whole announcement."
- **textFallback:** Chalk on a transom, described, not shown. Confidence is low because she does not like radios; she likes hulls.

### `radio_hf_numbers_alloc7` — ghost stencil
- **frequency:** 99.0 *(existing numbers band; Holdfast overlay)*
- **intelType:** NumbersStation
- **confidence:** 0.41
- **message `[VO]`:** "ALLOC-7. Not for general issue. Repeat: not for general issue. Coordinates corrupt. Cargo still readable. If you can hear this you are on a road that was a cut."
- **textFallback:** Automated remnant, confidence in the forties. Accident 12's crates are the better source. This is how rumours start in Sector 4.

### `radio_hf_white_fragment` — ending adjacent
- **frequency:** 121.5
- **intelType:** Civilian
- **confidence:** 0.48
- **message `[VO]`:** "Foghorn 8. Sounding. If you are leaving both districts, copy. We do not ask you to come back. Ice at the mark. Authenticator green. Out."
- **textFallback:** For `ending_holdfast_white` / Migration destination. The horn does not plead. It sounds whether anyone is coming or not.

### `radio_hf_school_uv` — Cluster incidental
- **frequency:** 164.2
- **intelType:** Civilian
- **confidence:** 0.91
- **message:** "School hour. UV board red. Recess indoors. Chains are not a toy. Yellow chalk counted. Visitors after the hour."
- **textFallback:** The teacher does not like the tannoy. A clerk runs it anyway. No `[VO]` required; ticker is enough.

---

# 6. Item flavour

First line = inventory tooltip. Inspect = 2–4 sentences. No magic. No glow.

---

## 6.1 Legendaries (`item_*` from bible §7.2)

### `item_schedule_sector4_copy` — The Other Schedule
**first line:** Every name is legible. Including yours, in a column you were not meant to see.
**inspect:** Ribbon copy from Ormund's drawer, or a carbon that travelled. Sole is here. Renn is here. Frayne is not. 12-C is a different folder; this is only the occupancy that was decided in advance. Do not fold it. The crease would go through a score.

### `item_halvard_kit_notes` — Improvised Potable
**first line:** His handwriting gets smaller toward the end. The diagrams do not.
**inspect:** Field notes from Allocation 12-B. Intake, cloth, iodine, heat, a barrel that was never a plant. Water-craft bonus at the waystation if someone can still read a small hand. If Sela is present she will leave the room if you call it salvage. She will stay if you call it engineering.

### `item_sole_unsigned` — Filed, Not Signed
**first line:** She blotted the date. She did not blot the refusal.
**inspect:** 12-C, Drown-stamped, unsigned. D/9 stand-down still works; Fleet pad still does not. Completeness versus execution on one sheet. The blot is ink, not tears. Do not describe it as tears.

### `item_playground_seat` — One Seat
**first line:** The chain is still there. The brass is in your pack.
**inspect:** A swing seat, unscrewed. 1× `brass_fittings` that everyone notices: Quad, Grade Hut, Allotments board, the tin if you know the tin. Children do not ask. Auditors do. You can put it back. The chain will not swing.

### `item_edor_return_self` — Clerk's Own Return
**first line:** The birth year is written twice. Once correctly.
**inspect:** Pink copy. Two years. Convoy 12's training example in a living person. If the error is left, he will omit a name for you once and hate it. If struck, Ormund will see the strike. Keep it off the duty roster. It is not a trophy.

### `item_yara_dark_mark` — Dark Mark
**first line:** She did not raise her voice. The beacon is dark.
**inspect:** A lath with black cloth, or the absence of oil in a cage. Ice Road access destroyed. Thick ice will still be ice. It will not be a road. You cannot talk this back on. Eleven days is for lesser darks. This is a withdrawal.

### `item_leva_minutes_vol12` — Volume 12
**first line:** Motion: that we keep running. Carried.
**inspect:** Binder, tabs, a failed valve-seat that used to paperweight it. Steam-trip warning six hours early if you keep it in the hall or the waystation. The Office would like a copy. The copy would not hear the gauges.

### `item_hearth4_hatch_log` — Hatch Log
**first line:** They logged every refusal. There are a lot of refusals.
**inspect:** Clipboard, pouch, dates, reasons: NO STAND-UP / NO NUMBER / BLASTING PARTY — DENIED. Icebreaker without a hundred explosives if a number authenticates. Stealing it does not empty his memory. He will say it again.

### `item_alloc7_ration_tin` — ALLOC-7 Tin
**first line:** NOT FOR GENERAL ISSUE. The issue is you.
**inspect:** Olive, stencil, frozen rim if opened on the Cut. Food. Morale down if opened in Sector 4, where the stamp is a mirror. Accident 12 still has more. Ice crows know the timetable.

### `item_cluster_formulary` — Human Formulary
**first line:** Dosage for a species the Verge has been approximating.
**inspect:** Bound, pre-war, Clinic-kept. Ianov payoff. Surgery odds. They will not send a copy south unless the levy is honoured. A child's correction on the thyroid plate is in pencil and is correct.

### `item_foghorn_timer` — Foghorn Escapement
**first line:** It sounds whether anyone is coming or not.
**inspect:** Brass clockwork from Foghorn 8. Shelf navigation. If owned, a faint sounding on Silence nights (text + existing radio — not a new album). If taken, Yara loses the coast in fog and so does the tender. Quiet is how columns vanish.

### `item_tin_fourteenth` — The Fourteenth Plate
**first line:** The tin is lighter. Nobody mentions it.
**inspect:** Only if you sold nameplates north. One plate missing from the fourteen behind the filtration stack. District 8 paid more than the Works. Still no comment. `lore_hz_nameplates` remains a tin. This is the dent.

---

## 6.2 Themed sets

### `set_cutter_kit`

**`item_ice_spike_bar`**
**first line:** A bar for ice that is lying.
**inspect:** Harbour steel, worn at the bite. Accident chance down on the Cut if someone who can read dark is holding it. Not a weapon. A question you ask the road.

**`item_beacon_oil`**
**first line:** Finger-widths to the WINDOW line.
**inspect:** Tithe and relight. The measuring-stick in the South Beacon cage is the honest clock. Steal it and the next column writes an accident with your mass.

**`item_cutter_ledger_blank`**
**first line:** Date, origin, mass, remarks.
**inspect:** Remarks are for the dead. A blank book is not hope. It is capacity. Yara will know if you invent a twelfth filter notch in the same hand.

**`item_ice_tyre_set`**
**first line:** Without these, the Ice Road is walking.
**inspect:** Vehicle component. Speed and accident chance. Not a driving game. A crate of rubber that smells like the Recovery Yard and salt.

### `set_salt_shift`

**`item_plant_suit_patched`**
**first line:** Never hazmat. Inner-tube at the knees.
**inspect:** Grey canvas, visor clouded from the inside. Salt-rash down, fatigue up. Degrades faster in UV. The patch is a bicycle tube from a year that still had bicycles.

**`item_resin_gloves`**
**first line:** Insides powdered. Outsides glazed.
**inspect:** Spent stack handling. Bare hands are how Tuesdays get worse. One pair on the jig is communal. Taking it is a shift decision.

**`item_fume_rag`**
**first line:** Wet it. Don't pretend it is a mask.
**inspect:** Chest-height fume in Hall 2. A rag is not a filter. It is the difference between a tour and a shift. Iodine after.

**`item_shift_whistle`**
**first line:** The whistle is the limit. The limit is skin.
**inspect:** Enforces outfall hours if someone blows it. Fatigue up because limits are work. Leva will give you one. Children should not think steam is a story.

### `set_office_paper`

**`item_census_return_blank`**
**first line:** Pink, yellow, white. White stays with them.
**inspect:** Occupancy, occupations, dependents, DOB once. Edor will read it again. A blank in your pack is not anonymity. It is a form that wants names.

**`item_order_12c`**
**first line:** Unlisted occupants of an authenticated facility constitute a labour reserve.
**inspect:** Published. Sector 4's copies died. This one did not. Sole will file and not sign. Voss will want the pool. The ice will carry a column.

**`item_allocation_tag`**
**first line:** Paper. Not a plate.
**inspect:** Block C guest grammar. Curls. Your name in an Office hand, or a Sector 4 occupation they guessed. Morale when visible in the shelter: some people sleep worse near paper that could become metal.

**`item_triplicate_carbon`**
**first line:** The third copy is the one they keep.
**inspect:** Three colours. Introductions by mass. A stolen stack makes the next receipt honest only twice. The Tollman will laugh. Ormund will note.

### `set_cluster_guest`

**`item_work_ticket`**
**first line:** The queue is for this, not bread.
**inspect:** Indoor access. A day of labour in a district that inventories you while you work. Steam if the pipe is live. Yellow cultivars if you are on trough duty.

**`item_steam_token`**
**first line:** Eight hours of waystation warmth, if the substation agrees.
**inspect:** Stamped fibre, not coin. Cluster currency-in-kind. You cannot steal heat. You can steal tokens. The wooden box at the valve house will be light.

**`item_block_c_key`**
**first line:** A key for a door with a paper tag.
**inspect:** Guest housing. The radiator ticks if the canal is honest. Children's boots in C-214 if you have not taken them yet. Home still ticks without you.

### `set_ro_process`

**`item_ro_resin`**
**first line:** Brine becomes process. Process is not clean.
**inspect:** Plant repair. Tuesday's short count. Virgin drums are brown-stencilled and heavy. Heat and iodine still required. District 8 will never make Sector 4 thirst irrelevant.

**`item_ro_resin_spent`**
**first line:** Looks dry. Is not.
**inspect:** Sample from Hall 2. Recoat yield low. Toxic to handle. Valuable to people who still believe. Ice crows will not land on the stack.

**`item_iodine_crystal`**
**first line:** Thyroid and water in the same cage.
**inspect:** Bulk. Process column, then thyroid, then clinic. The Office has a key. The Salt has a tea-tin. Lot numbers are Continuity. So is the stamp NOT FOR GENERAL ISSUE.

**`item_process_barrel`**
**first line:** Transport. Twenty percent spoilage if the ice lies.
**inspect:** Thirst at forty percent if drunk raw. Electrolyte salts after. Haul south and lose some to the Cut. Rebuilders still need tablets. You cannot pipe this to Allocation 12.

### `set_shelf_radio`

**`item_schedule_crystal`**
**first line:** The hour, not the order.
**inspect:** A crystal that keeps *Hearth-4*'s schedule even when the foghorn is stolen. Hearing is not a stand-up. Mire will say so.

**`item_fleet_pad_copy`**
**first line:** It does not authenticate.
**inspect:** Same family as D/9. Wrong door. Show it to Mire and he will be interested. Interest is not a hatch. Voss cannot conscript a ship with it.

**`item_foghorn_key`**
**first line:** Winds the spring. Does not decide who is coming.
**inspect:** Plinth hook. Companion to the escapement. Cutters navigate by sounding. Silence it to hide and something on the water loses the coast as well.

### `set_two_district`

**`item_map_sheet_ice_road`**
**first line:** A road that is not there in summer.
**inspect:** Waxed. Fingerprints kept. Ostrowski will not say who walked it. The *Moth* will sell you a contradiction. Ivy will confirm a post, not a road.

**`item_kittiwake_copy`**
**first line:** The log continues eleven days past the Exchange.
**inspect:** If the chart was copied. Channel markers versus the sheet versus the *Moth*. Nomi goes quieter, not warmer. She already knew. She had not been paid.

**`item_weigh_receipt_hf`**
**first line:** Introduction — twelve kilograms equivalent.
**inspect:** Tollman grammar meeting Office grammar. Honest paper. The destination field may say ESTUARY / SEASONAL. Someone may have written *the Salt* underneath.

---

## 6.3 Consumables (short)

| id | first line |
|---|---|
| `item_salt_rash_salve` | Grit in the grease. Soothes. Does not cure. |
| `item_uv_grease` | Albedo is a tax. This is a delay. |
| `item_electrolyte_salts` | For people who drank the process. |
| `item_welders_glass` *(existing)* | Midday on the Cut. Or snow-blind on dark ice. |

**`item_salt_rash_salve` inspect:** Two finger-scoops gone from the waystation tin. Iodine soothes not cures. The clipboard at the outfall will not thank you.

**`item_uv_grease` inspect:** One expedition of blistering down. Coastal ozone, ice shine. Sun-Seekers will want visors more than grease. Grease is what you have.

**`item_electrolyte_salts` inspect:** Counters process-water drinking. Leva will still count you as a problem if you skip iodine. Salts are not a membrane.

---

# 7. Accident book entries

Yara's book, weigh hut. Remarks column. House voice. Shareable lines. 12 entries.

**AB-01 — Year 1, unlisted, unnamed**
Window 3. Thickness honest. Beacon lit. Column of allocated from Cluster, four persons, first ice. They walked like indoor air. Two returned. Remarks: *would not wait a dark. Dark was a lead, not a mood.* Mass entered as estimate. Estimate is how we lie politely.

**AB-02 — Year 2, Fleet ashore**
A party from the roadstead, six, trying the Cut south. Foghorn 8 late. They navigated by a lamp that was Sector 4's. Ivy's 19. They did not know the seam. One crate of ALLOC-7 left as payment for a sledge they did not get to keep. Remarks: *paid in letters. Letters are not spikes.*

**AB-03 — Accident 12, Year 3**
Column through. Ice did not. Cargo readable: ALLOC-7 / NOT FOR GENERAL ISSUE. Glove on a handle, do not pull. Axle as survey pin. Remarks: *I did not fetch. Fetching is how I die on a lie. Book is the fetch.*

**AB-04 — Brine Pool, undated**
A person fished. Protein. Bad idea. Gaff left as larder or warning. Thermometer still on the air scale. Remarks: *warm water is not kind water. Dogs on the far bank. Dogs have a limit. This person did not.*

**AB-05 — South Beacon, oil**
Reservoir below WINDOW. Stretch treated closed. A column waited. A different column did not. Remarks: *thick is not lit. I wrote the second column in this column.*

**AB-06 — Waystation A, twelfth notch**
Someone filed a bright twelfth on the filter rim. Closed window. CO2 and salt air. One bunk A4. Survived. Remarks: *do not invent days. Ice does not honour files. I see bright metal.*

**AB-07 — Weigh Hut, poise**
Calibration 500 kg missing. Next three receipts light. A child in Block C later had a doorstop that was a weight. Returned. Remarks: *mass left the ice and went indoors. Indoors thought it was a stone. It was a road.*

**AB-08 — Dredger *Moth*, sounding**
Chart sold, channel west of Kittiwake. A sledge took the west. Wet to the axles. No dead. Remarks: *Ostrowski's sheet and the Moth's sheet and the launch's log are three truths. Ice is the fourth. Pay in food. Do not pay in belief.*

**AB-09 — Levy column, intercepted**
Office three, named. Garrison patrol at the Gate smelling District 8. Tollman charged twice. Remarks: *I light for whoever is walking. I do not light for a war. They waited. Waiting is rare. I put them on the overnight.*

**AB-10 — Charges, Shelf**
Blasting party. Ridge refused. Ice opened a hole that did not care who waited in it. Foghorn 8 heard it as weather. *Hearth-4* logged BLASTING PARTY — DENIED and then stopped being a watch. Remarks: *I did not raise my voice. Beacon dark. Find your own kilometres.*

**AB-11 — Edor Vale, incomplete**
Clerk Grade III. Return unfinished. Found in this hut after a closed window, good hand, pink copy wet. Occupations still wrong by one. Remarks: *he waited in the ash like the procedure said. Procedure does not thicken ice. I did not put him in remarks as dead. I put him as incomplete. Incomplete is a status.*

**AB-12 — Unlisted runner, south**
Allocated person walking toward Sector 4, no ticket. Offered a story. I do not take stories. Lit the stretch. Remarks: *Voss would call them a prize. I called them a mass. They waited when I said dark. That is the whole character reference.*

---

# 8. Ending second paragraphs

`world_history` bodies, discoverable later at `loc_cluster_office` or the player's hatch. The game does not rank them. First slides remain as the bible; these are the **second paragraphs** only.

### `ending_holdfast_schedule` — The Schedule Holds
The duty roster on the bunker wall has names on it that are not the names that slept there. Block C has plates where paper was. Process water comes south in barrels that lose a fifth to the Cut, and the Verge still boils what it has. In the Office drawer the discrepancy file is closed with a stamp that does not say *resolved*. It says *entered*. Children in the Cluster school add a working that includes a caretaker. The teacher circles the rubric anyway. The chains on the Quad have not found their seats. Someone has tied a rag to one of them, so that in wind it looks as if a swing were trying.

### `ending_holdfast_reserve` — The Reserve
Receipts in triplicate: pink in a satchel that went south, yellow in a weigh hut, white in a drawer. Nobody was shot. Columns moved. If a dependent was claimed, a clinic autoclave cycled for a child who knew a tunnel better than a stairwell, and a kit of small handwriting stayed in a hole that no longer had its water memory. Margit Sole filed 12-C and did not sign it. Cael Ormund executed it and did not raise his voice. The ice took a column south and a column north on the same window. Yara wrote both masses. She did not write which one was fair.

### `ending_holdfast_dark_road` — The Road Goes Dark
District 8 continues without the unlisted hole. Forty apartments stay dusted. The yellow cultivars fail on their own timetable. In a weigh hut, after a thaw, a census return is found incomplete, in a good hand, occupations still adjacent-wrong, a date of birth written once. The remarks column of the accident book has a line that is not for the dead. It says *incomplete*. Lamps on the Cut go out in the ordinary way, one wick at a time, because windows close. Ivy's 19 stays lit. The seam between ash and salt is still a poorly taped join. Nobody crosses it who does not already know how.

### `ending_holdfast_tender` — Stand-Up
The Fleet stops being a rumour. *Hearth-4*'s lamp is still green; green was current; current is now people on a quay that is a field. The Cluster votes on beds. Some vote with work tickets. Some vote with the indoor thermometer. Mire logs the vote and does not vote. A pad that would not verify a land form still will not. It did not have to. The authenticator wanted a number and a number was found, or a person was asked ashore, which was a different request. Migration and Icebreaker land in a place. The place has a playground with chains. The new coats on the school pegs are damp with salt.

### `ending_holdfast_white` — The White
The snow-crawler leaves both districts. Foghorn 8 sounds once, on timer, and does not ask them to come back. Allocation 12's hatch is a hole with a blank roster and a tin that may be lighter. Cluster 7's discrepancy file stays open until someone stamps it *left*, which is not a status the forms were printed with. A clerk writes it in remarks anyway. The ice road, next freeze, opens for other masses. Yara opens it. She does not open it for them.

---

# 9. Census form — diegetic document

What Edor reads. House voice. Triplicate grammar. UI may paginate; this is the full read-aloud and the Codex image-text. `[VO]` if a single actor records Edor's read; otherwise print.

---

**CONTINUITY ALLOCATION SCHEDULE — SECONDARY RETURN**
**Document:** Reconstruction Occupancy / Unallocated Labour
**Form:** C-12 / R (triplicate)
**Copies:** pink — respondent · yellow — Cutters (if ice used) · white — Office of Continuity, Allocation Cluster 7
**Clerk:** VALE, EDOR — Grade III — score 60.4
**Authority:** Continuity Reconstruction Order 12-C; Cluster Standing Instruction 7-Ice

---

**Heading (he always offers this first)**

Office of Continuity. Hydrographic District 8. Allocation Cluster 7. This is a secondary return for an authenticated facility whose assignees did not arrive. It is not an eviction notice. It is not a sentence. It is a record of occupancy and a schedule of reconstruction labour reserved by a formula that was published. Most people want it read again. That's all right. There isn't a time limit on understanding it. There is a time limit on the ice.

---

**Section A — Facility**

1. Allocation number of facility: ______ (if unknown, write NONE. Do not invent.)
2. Authenticated occupancy (design): 14
3. Present occupancy (living): ______
4. Hatch status at time of interview: standby / dogged / unknown
5. Duty roster: blank / marked / not seen

*Instruction to clerk:* If the respondent says *twelve*, open discrepancy file D-12. Known discrepancy. Do not treat as confirmation of assignees. Assignees of Allocation 12: see Schedule, Sector 4, RENN and associated household — NOT ARRIVED / 12-B UNCONFIRMED.

---

**Section B — Persons**

For each occupant, one line. Occupations as stated, then as observed. Adjacent is how we miss people.

| # | Name | DOB (once) | Occupation (stated) | Occupation (observed) | Listed / Unlisted | Dependents | Remarks |
|---|---|---|---|---|---|---|---|
| 1 |  |  |  |  |  |  |  |
| 2 |  |  |  |  |  |  |  |
| 3 |  |  |  |  |  |  |  |
| … |  |  |  |  |  |  |  |

*Instruction to clerk:* Dates of birth written once, correctly. If two years appear, stop. Read both lines aloud. Do not joke. Convoy 12 is a training example. It is also a hatch. Remarks are for the dead. Do not put the living in remarks because the ice is short.

*Known adjacent errors (pre-filled if interview is south of Kilometre 19):* mason / caretaker; clerk / clerk-grade; veterinary assistant / veterinarian. Correct in the same hand. Thank the respondent for the truth without using the word.

---

**Section C — Reconstruction pool (Order 12-C)**

Unallocated occupants of an authenticated facility constitute a labour reserve. Trades chronically short in District 8: caretakers, records clerks, veterinarians, ice-cutters, lamp-keepers, and other Reconstruction Utility Ratings scored below twenty.

**Levy (first):** three names, thirty days, destination Cluster 7 / Saltworks as assigned. Kit: warmth, iodine, welders' glass. Ice window as posted at the Gate. This is not a request. This is a line.

**Honour** — names as written. Regular.
**Substitute** — names other than written. Irregular. Audit.
**Refuse** — in writing, or by silence past the window. Status follows occupancy. Quiet interval at hatch: forty days. Escort remains on rota.

The clerk does not enter a facility uninvited. The clerk may wait. Waiting is in the procedure. Waiting is not a threat. It is the time the ice is using anyway.

---

**Section D — Dependents and claims**

If a person present holds a laminated allocation card for a scored household, copy the number. Do not take the card. Laminated is a kind of proof we don't get often. Write UNCONFIRMED until someone who can confirm, confirms.

Cluster Clinic may enter a dependent of an allocated water engineer as a Cluster child. That filing is correct on paper. The child may speak. The filing does not require agreement. It is easier if they agree. Easier is not the same as right. Do not write *right*. Write the number.

---

**Section E — Ice**

Window length is not negotiable. Ice is.
If Cutters mark dark, treat as closed even if thickness is honest.
Yellow copy to weigh hut if the Cut is used. Axle mass of column: ______.
Remarks (Cutters): ______

---

**Closing (he reads this even if they stop him)**

I am not collecting you. I am completing a return. If you want the heading again I will start at the heading. If you want the names I will start at the names. If you tell me to go I will go as far as the boom. I will not come in unless you say. That is the only rule I have that isn't printed.

Signature of clerk: _____________  (VALE, E.)
Signature of respondent: _____________  (or: refused / silence / ice)
Stamp: DISCREPANCY NOTED / RETURN CURRENT / INCOMPLETE

White copy to Registrar-General ORMUND, C.
Do not fold through a score.

---

# Appendix — Consistency flags

Found while writing. Do not silently retcon. Ticket or ignore.

1. **`loc_toll_house`** lives in `locations_expansion3.json` (displayName: "The Tollman's Bridge") and in `world_history.json` discovery fields, not in canonical `locations.json`. Recast in this pack assumes the expansion3 id. If implementation merges catalogs, keep one id.
2. **`location_abandoned_desalination`** still describes an abandoned Hydro-Baron ruin. Bible + this pack recast as occupied Municipal Desalination 8. DisplayName should change at implementation; id stays.
3. **`location_frozen_river_barge`** still says "cannibalistic dockworkers." Recast here: cargo-starved crew, billhook dull from ice. Tone fix, not a new place.
4. **`location_crashed_icebreaker_convoy`** still says "derailed military train." Bible: ice-capable rolling stock toward the roadstead; tender *Hearth-4* is the vessel. Both can be true. This pack does not merge them.
5. **`npc_cluster_teacher`** remains unnamed. Do not mint a personal id without a grep. Voice bible uses title only.
6. **`loc_weighbridge` description** in `locations.json` is one line. Holdfast overlay adds Edor; do not delete the Tollman's first office.
7. **Radio bands** 121.5 / 156.8 / 162.4 / 164.2 / 27.12 are proposed. Re-grep `radio.json` before commit. 99.0 overlay clip `radio_hf_numbers_alloc7` shares an existing numbers band on purpose (ghost stencil).
8. **Frayne RUR 11** (bible) vs lore examples of water-engineer scoring: both true — she was not allocated; Leva was in-situ; Halvard was allocated away from plants. Do not "fix" Frayne into the drawer.
9. **Sela age:** bible/lore thirteen at the claim; Exchange+5Y, allocated at eight. Holdfast overlay must not age her into an adult companion-warrior. She is not a combat party member.
10. **`item_welders_glass`** is existing; listed under consumables as a reminder, not a new id.
11. **Ministry "Protocol Zero"** in the current `location_ministry_of_truth_bunker` one-liner is older tone. This pack's overlay uses the Schedule cover letter to District 8 and does not extend Protocol Zero.
12. **No seventh Power.** Office / Cutters / Fleet live in `holdfast_factions.json` (not written here). `hydro_barons` remains the economy id. Do not add rows to `faction_lore.json`.

---

# Word counts

Approximate, by bucket (this file, shippable prose including ids/headers):

| Bucket | Words |
|---|---:|
| 1. Location cards (35 POIs + Sector 4 recasts) | 6,050 |
| 2. NPC voice bibles (7) | 5,940 |
| 3. Main quest stage prose (10) | 2,750 |
| 4. Side quest hooks + resolution (18) | 2,130 |
| 5. Radio / foghorn / plant tannoy (18 clips) | 1,260 |
| 6. Item flavour (legendaries + sets + consumables) | 1,770 |
| 7. Accident book (12) | 590 |
| 8. Ending second paragraphs (5) | 585 |
| 9. Census form C-12/R | 830 |
| Front matter + consistency flags | 410 |
| **File total** | **~22,300** |

Target was 20,000–35,000 of new shippable text beyond Appendix C. This pack expands the three Appendix C samples into full cards and adds the rest of the catalog. Quest blocks are kept to UI length (40–90 words, 120 max) rather than padded to the bible's 54k architecture estimate.

**Implementation note:** Paste `description` / `inspect` into `holdfast_locations.json` (or `locations.json` overlays). Paste barks into NPC tables / `threateningBodyText` pairs. Do not edit `faction_lore.json`. Re-grep ids before commit.

