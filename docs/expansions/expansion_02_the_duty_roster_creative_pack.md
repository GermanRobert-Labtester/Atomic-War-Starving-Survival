# ASHFALL: THE DUTY ROSTER — Creative Pack

**Internal id:** `expansion_the_duty_roster`  
**Kind:** Shippable prose. Additive to `docs/expansions/expansion_02_the_duty_roster_plan.md`. Does not rewrite the bible.  
**Voice lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.  
**VO:** Lines marked `[VO]` are text-first; record only if the radio/intercom pipeline already exists. Everything else is UI/Codex/inspect.

Ids reuse the Duty Roster bible. Re-grep `locations.json`, `locations_expansion3.json`, `QuestlineSO`, `faction_lore.json`, `currents.json`, `world_history.json`, and Holdfast proposed ids before implementation. No C# in this pack. No seventh Power. No Tessarat, Sector 7G, terraformers, androids, neuromancers. No second District 8 coast.

Hatch-dilemma magnitudes are **existing** and **not retuned:** let-in 50 rads/h, force-decon 10, deny-entry morale 20 on every other living survivor.

---

# 1. Location cards

Schema: `id`, `displayName`, `inspect` (one line), `description` (80–180 words). Each card names at least one object you could steal, weigh, or refuse.

---

## 1.1 The Stack — `loc_stack_*`

### `loc_stack_roster_wall` — The Chart

**inspect:** Fourteen rows. A pencil on a string. The heading is older than anyone sleeping here.

**description:**  
A wall chart headed `ALLOCATION 12 — DUTY ROSTER`, print date before the Exchange, paper that has gone the colour of tea. Fourteen rows. Columns: name, occupation, watch, remarks. The remarks column was never meant for the living. A hole is punched in the lower left; a pencil hangs through it on a string darkened by hands. The point is short. There is a knife-nick in the rail where someone meant to start a name and did not. Four unfaded rectangles on the corridor behind you are the same width as brass plates. If the rows are still blank, dust has been wiped in a rectangle the size of a palm, daily. If they are written, the graphite shines when the lamp is lit and dulls when it is not. You can take the pencil. The next morning will be guessed. You can write a name that has not slept here. Kess will erase it before the kettle boils.

### `loc_stack_sleeping` — The Sleeping Stack

**inspect:** Eleven bolted footboards. Three pads. One extra blanket folded as if it were a person.

**description:**  
Bunks sized for eleven, numbered in stencil that skipped 4 and 13 the way some buildings skip floors. The manifest in the airlock still reads fourteen. Three camp-pads fill the difference, edges taped, names on paper tags if anyone has claimed a fourteenth. A blanket is folded on the empty pad even when no one is due. It keeps the shape of a chest. Under A7, a sock. Under A2, a dosimeter on a nail, ticking slower than the intake. You can take the blanket. The pad will look like a pad. You can move a tag from one board to another. Kess will put it back if the person did not sleep there. The air is breath and wool and the ghost of iodine.

### `loc_stack_mess` — The Mess

**inspect:** One table. Portion rings burned into enamel. A ladle that has a queue.

**description:**  
A table that seats eight if no one brings elbows. Enamel bowls with rings where stew sat and was not eaten, or sat and was. A ladle hangs on a nail at head height so children cannot vote with it unless lifted. Knife-scratches on the table edge count something that is not days: extra portions, or the times the count was short. A tin of salt with a lid that does not match. If three people are north, two bowls wait at the far end with cloth over them, and the cloth gets a skin of dust by evening. You can take the ladle. The next meal will be poured with a mug. You can leave a bowl until it is cold. The ring it leaves is a mark. Ansel will not point at it.

### `loc_stack_filtration` — Filtration Stack

**inspect:** Canisters. A tin behind them that everyone puts back. The intake bunk is the one with the cough.

**description:**  
The filtration stack is a throat of metal and cloth. Canisters notch-filed for days, the way Waystation A's filter is notched for a window. Behind it, a tin. Fourteen brass nameplates. Everyone who has found them has put them back. The tin is heavier than it looks until it isn't. A stool is bolted nearest the intake; the person who sleeps there tastes the filter first. A rag on a hook is Hadi's if he is here, and is still Hadi's if he is not. You can take a canister. The next night the intake bunk will know. You can take the tin. The rectangles in the corridor will not grow brass. You can screw one plate under a living name. It will catch the lamp. Nobody will mention the tin.

### `loc_stack_clinic_alcove` — Clinic Alcove

**inspect:** Iodine, cloth, a bolt of string. Not a hospital. A rag that is damp when he is gone.

**description:**  
A curtain on a wire. Behind it: iodine in a brown bottle with a Continuity lot number, a bolt of boiled cloth, string, a basin that was a mixing bowl. Arithmetic, if Ianov has visited, is a paper on the crate with numbers that were not rounded toward ease. If Hadi Morrow is north, the rag on the hook is still damp and nobody boils it. If he is dead, the rag is dry and still nobody throws it. A child's drawing of a bunk, labelled with a number that is not on the sleeping-stack stencil. You can take the iodine. The next dressing will be water and hope that is not called hope. You can take the rag. The hook will still look like a hook.

### `loc_stack_airlock` — Inner Airlock

**inspect:** A crate of boots, sizes 1 through 4, the tape still factory. A chair bolted to the wrong profession.

**description:**  
The last room before the hatch scene. A dental chair, bolted, no instruments. A crate stencilled `ALLOC-12 / NOT FOR GENERAL ISSUE`, children's winter boots, sizes 1–4, tape uncut unless you cut it. Decon is a bucket and a rag on a nail. The rag ticks if someone came in glowing. Boots sit in pairs even in the crate, as if pairing were a kind of care. You can open the crate. A child can wear a pair. They were not packed for them. You can leave the tape. The crate will still be a crate in forty days. You can unbolt the chair. Dentists' Row is missing exactly this.

---

## 1.2 The Approach — `loc_approach_*`

### `loc_approach_hatch` — Outer Hatch

**inspect:** Standby cycle held it unlocked once. Everything since has been a decision.

**description:**  
The outer hatch. Wheel, gasket, intercom grille with a button cracked to show the spring. Temperature is a number Tamsin will say if you ask, and a fact if you do not. Frost on the inner rim in winter; salt-rime if a column has come from the Cut. Scratches on the wheel from hands that did not know the direction. A laminated card in a freezer bag has been here, or will be, or a triplicate form, or a ticket from a driving-licence bureau. You can open. You can keep shut. The game will give you the temperature and then stop talking. You can steal the cracked button. The next visitor will knock on metal.

### `loc_approach_apron` — Ash Apron

**inspect:** Tracks. A rectangle of earth the crows already know.

**description:**  
Ash packed by feet into a fan. Folding-stool marks, three metres out, a triangle that has been pulled and sunk and pulled. If a burial was done outside, the rectangle is a different grey and the flock has a schedule. A peg for a lantern that is not always lit; Tamsin lights it for whoever is walking, which is not Ivy's rule and must not be asked to be. You can scuff the stool marks. They will come back if the person does. You can leave a cup. It will fill with grit.

### `loc_approach_stool` — The Waiting Stool

**inspect:** He does not knock. The waiting is the procedure.

**description:**  
A folding stool, municipal, one rivet replaced with wire. Three metres from the hatch, which is the distance Edor Vale will name if you ask why he does not come closer. The feet have sunk. A square of cardboard under one foot is dated in pencil, a week ago, or a month. There is no cup unless you bring one. If you carry tea out, he will wash the cup in the ash and there will be grit in the seam when you get it back. If levy is refused, the stool is here for forty days. If the column was hidden, the stool may be empty because there is nothing to wait for, or still occupied because he does not know. You can fold the stool and bring it in. He will stand. Standing is also in the procedure.

### `loc_approach_decon` — Decon Alcove

**inspect:** A bucket. A rag. The magnitudes are already written.

**description:**  
A niche with a grate. Bucket, cold water, a rag that has been boiled and has not. A painted line on the floor that means strip. Force-decon spills; the alcove ticks at ten. Let-in ticks at fifty, and the tick walks inside. Deny does not tick. Deny is a number on everyone else's face in the mess. A nail holds spare string for tying sleeves. You can take the rag. The next returnee will use a shirt. You should not retune the numbers. They were expensive.

---

## 1.3 The Overflow — `loc_overflow_*`

### `loc_overflow_alloc_11` — Allocation 11

**inspect:** The authenticator is on. The chart is blank on purpose.

**description:**  
A hatch that still believes in a number. Inside: the same print of duty roster, fourteen rows, wiped. A disc with no number hangs on a nail by the wheel. Four people sleep here. Their names are not on the wall. Nila Brant will trade a hiding place for a filter and will not say please. If you write one of their living names in ink, or on a census, or on 12-C, this hatch will still look like a hatch. It will not open. You can take the disc. It authenticates nothing. That is the point. You can write a name as a kindness. Kindness is how a pool is made.

### `loc_overflow_alloc_13` — Allocation 13

**inspect:** Authenticator lit. Dust. One name erased to a scar in the paper.

**description:**  
Empty. The light is a waste. A chart with a rectangle of thinner fibre where a name was, graphite ground out with a wet thumb. The scar is still a shape if you hold it to the lamp. Nila wants it to stay a scar. Copying it for Sole is completeness. Completeness opens 13 to a file and closes 11 to you. A bunk frame with no mattress. A kettle with mineral rings, dry. You can rewrite the name. You can leave the scar. You can pocket a rubbing. The authenticator will not care. Nila will.

### `loc_overflow_pump_hatch` — Pump Hatch

**inspect:** A hatch on a riser. It authenticates for nobody.

**description:**  
Service architecture, a lid on a pipe the city used to deny floods with. The authenticator plate is blank — not wiped, never numbered. Blank Rows leave filters here, and a pencil jar, and a note that says `DO NOT WRITE THE LIVING` in a hand that is not Nila's. The air smells of wet rust. You can take a filter. Someone in 11 will breathe worse. You can write a joke. They will not laugh. They will move the cache.

### `loc_overflow_blank_cellar` — The Blank Cellar

**inspect:** Pencils in a jar. The rule is the only decoration.

**description:**  
A cellar under a Grid stair that still has a civil-defence stripe. Chairs that do not match. A jar of pencils, points kept, erasers worn to metal. The rule is painted, not paper: `DO NOT WRITE THE LIVING`. A second line, smaller, in graphite: *the dead can take it.* They argue about the second line. You can steal a pencil. Kess's string will not match. You can add a third line. They will paint over it.

---

## 1.4 Circuit overlays (*existing* ids; additive inspect)

### `loc_weighbridge` — overlay

**inspect:** Occupations versus the wall. He is already one grade off.

If the chart is in use, Edor will read three trades and they will be almost the names Kess wrote. If the chart is blank, he will still be almost right, which is worse.

### `loc_conscription_office` — overlay

**inspect:** The ticket machine still works. Pell has brought a spare into the ash before.

Your quota and the levy are the same three bodies if the wall is ink. He will say so. He believes service is rations. It is.

### `loc_the_allotments` — overlay

**inspect:** Brass is fittings. She will not ask if they were names.

The tin and the playground and the valve seats are one metal. Frayne's minutes will record mass, not origin.

### `loc_st_brigids_almshouse` — overlay

**inspect:** Charts filled to a date, then not. Two knocks at a hatch are how you get here.

Do not enter the back room. The game will not take you there. Blankets on a chair. Len's satchel.

### `loc_dentists_row` — overlay

**inspect:** The fourth chair is in your airlock, or it is here again.

Bolts in a square of cleaner floor. If you return it, the square is occupied. If you do not, the square is a measurement.

### `loc_alloc_12b` — overlay

**inspect:** Fourteen chalk marks, a gap, then six. The kit still works if you left it.

Sela will leave the room if you call it salvage. She will stay if you call it engineering. The water does not care what you call it.

---

# 2. NPC voice bibles

Speakable. Text-first.

---

## 2.1 `npc_kess_adler` — Kess Adler

**Where:** `loc_stack_roster_wall`  
**Was:** Records clerk. RUR 9. Unlisted.  
**Will not:** Write a name that has not slept here. Falsify a DOB. Joke. Ink without being ordered.

### Do / Don't

**Do:** Present tense. Names as rows. Ask who is on today. Call the wall a chart. Offer pencil as delay, not as kindness. Erase dust if the row is blank.

**Don't:** Say we should. Say fair. Say family. Raise her voice. Call anyone allocated. Call Blank Rows cowards. Comfort.

### Barks (12)

**First meet**  
"The wall was left blank because the names it wanted did not arrive. If I write yours, that is not the same as being them. It is only the same as being here."

**Morning**  
"Who slept here. That's the only column I can fill without lying. Lying is a different pencil."

**Pencil**  
"Graphite comes off. That's the point of it. Ink is a levy. I don't choose ink."

**Ink ordered**  
"I will write it. I will not pretend I chose it. Nila will hear the difference even if you don't tell her."

**Hadi**  
"Veterinary assistant. Edor will write veterinarian. I can leave the grade wrong. I can also not."

**Levy**  
"Three names. I can copy them. I can misspell them. Missing a letter is still a name. I won't misspell them."

**Child at the wall**  
"That's a nickname. I can leave it. The Office will call it irregular. Irregular is a status. Status follows occupancy."

**Tin**  
"I don't comment on the tin. I never have. The rectangles are still there."

**Burn**  
"If you burn it, I will still stand here in the morning. There will be a cleaner rectangle of wall. I will wipe that too."

**Sole**  
"I can say a name aloud. I wrote it. That is corroboration. It is not a religion. The children in the Drown inferred one. I didn't."

**Blank Rows**  
"They keep it empty on purpose. I keep it empty until someone sleeps. Those are not the same empty."

**Trust-low**  
"You asked me to write a person who was not here. I erased it. I will erase it again. The string is long enough."

### Monologue (once) `[VO]`

"I will say this once because the kettle is loud and I am not. I was a clerk. Occupation was worth up to forty points. Mine was nine. The department published the rubric so that transparency would feel like fairness. I filled other people's rows. I did not fill my own, because there was no row. This chart was printed for fourteen people who had numbers. We are not them. Writing us down is how we eat in the right order, and how a man on a stool completes a return, and how a plant on a coast gets a caretaker, and how a hatch escort arrives with the correct list instead of a better one. I prefer pencil. Pencil is a morning. Ink is a year. Blank is a politics I understand and cannot run for fourteen people who already know each other's coughs. If you order ink, I will use the stick. If you burn the heading, I will not find another chart. There isn't one. The print date is on this paper. The print date is not coming again."

### threateningBodyText pair

**Neutral**  
Kess stands under the chart with the pencil on its string. She waits until you say who slept here. She does not prompt. The kettle clicks.

**Threatening (`threateningFactionId` unused; mark `mark_wrote_unslept`)**  
She has already erased a name. The paper is thinner there. She does not look at you while she wipes the dust into the same palm-rectangle as before.

---

## 2.2 `npc_hadi_morrow` — Hadi Morrow

**Where:** clinic alcove  
**Was:** Veterinary assistant. Levy bait.  
**Will not:** Call himself a doctor. Leave a septic child for a form without being ordered.

### Do / Don't

**Do:** Doses out loud. Count rags. Name the morning. Say what he cannot do in the same hour.

**Don't:** Fair. Hero. Surgery speeches. Hate the Office. Hate Voss. He is tired of both nouns.

### Barks (12)

**First meet**  
"I can do this here with iodine and a clean rag. I can do the outfall with a whistle. I cannot do both in the same morning. You have to write which morning it is."

**Title**  
"I'm not a veterinarian. I'm the person who is here. Ianov will tell you the difference with arithmetic. I will tell you with a rag."

**Listed**  
"If my name is on a return, it is a name on a return. I won't hide in a cupboard. I will hide in Allocation 11 if you ask Nila, and I will hate the asking."

**Sent**  
"Thirty days is thirty dressings I will not do. Pack the iodine. Pack a second rag. I will come back if the ice does."

**Never back (read by others)**  
The rag is dry. Nobody boils it. Kess has not written MISSING. Missing is remarks. Remarks were for the dead.

**Child**  
"If they are hot, I am here. If I am not here, you boil the cloth and you do not guess the dose. Guessing is how I know I am not a doctor."

**Ianov**  
"He doesn't round toward ease. I don't either. That is the whole of the colleague."

**Membrane**  
"They need bodies on an apron that eats skin. I have skin. So do you. Write it."

**Meal**  
"I skipped it. I know. If you send the child with a bowl I will eat it. If you make a speech I will not."

**Sela**  
"She remembers water that worked in a hole that was not provisioned. That is more useful than my title. Don't send both of us north in the same window."

**Trust-high**  
"Put me on the morning row. Not the remarks."

**Trust-low**  
"You wrote veterinarian. I am going to correct Kess. You can correct Edor yourself."

### Monologue (once) `[VO]`

"They scored caretakers cheap because care is what you do after the useful people are allocated. District 8 has membranes and almost no one who will sit with a dressing through a night. I sit. That is not a virtue. It is a timetable. If you hide me, a strip on a Quad stays missing and a child here gets a clean rag. If you send me, a whistle blows on an outfall and this curtain is a curtain. If I don't come back, boil the cloth. Do not write doctor on the wall. I wasn't."

---

## 2.3 `npc_tamsin_rook` — Tamsin Rook

**Where:** intercom / night slate  
**Was:** Harbour night-clerk. Unlisted.  
**Will not:** Lie about who is outside. Sleep the same bunk two nights if the watch is short — unless you make her.

### Do / Don't

**Do:** Times. Distances. Temperature. "Say again." Dark and lit as apron facts.

**Don't:** Poetry. Threaten. Call Edor a coward. Call Pell decent where he can hear it — she will, once, where he cannot.

### Barks (12)

**First meet**  
"There's a stool in the ash. There's a person on it. I'm not opening until you say. I'm also not pretending the stool isn't there."

**Intercom, unknown**  
"Say again. I have a cracked button and a wind. Name, or I keep the wheel where it is."

**Glow**  
"They're ticking. Let-in walks it inside. Decon spills a little. Deny is a number on everyone who didn't go out. Those numbers are already written. I don't change them."

**Night slate**  
"Same name three times is not a rotation. It's a person falling over at four. Write someone else or I write the airlock pad."

**Waystation**  
"A1 through A4 are our people with different weather. If I go, this grille is a grille. You will hear the crack in the button louder."

**Office escort**  
"Faded jackets. Forms. They know the temperature. I know the temperature. I'm asking you, not them."

**Pell**  
"He brought a ticket machine. He is not joking. Take a number or don't. Don't break it unless you want a worse man with a worse machine."

**Len**  
"Two knocks. That's the House. They want a name and a sentence. I don't supply either."

**Ice Road open**  
"Haulers out. House thin. I stay unless you send me. If you send me, teach someone the wheel."

**Road dark**  
"Everyone is home. The stack is a stack. I need a second watch or I need you to accept that I will miss a knock."

**Trust-high**  
"I'll tell you who's on the apron before they speak. That's the job. That's all of the job."

**Trust-low**  
"You asked me to say we weren't here. I said we were. The grille doesn't do fiction."

### Monologue (once) `[VO]`

"I worked nights when the harbour still had a clock. The clock is ice now, or a stool. I open when you say open. I close when you say close. I will not tell a census clerk that a caretaker is a rumour. I will not tell a child that a glow is a trick of the lamp. If you want a liar on this grille, write a different name on the slate. I will sleep. I will not sleep the same bunk twice if I can help it. If I cannot help it, my voice will be slower, and you will think the button is more cracked than it is."

---

## 2.4 `npc_ansel_duth` — Ansel Duth

**Where:** mess / stack  
**Was:** Parent. Unlisted.  
**Will not:** Ask twice whether you told the truth at the table.

### Do / Don't

**Do:** Short questions. Name the child as they are named in the run. Point at objects, not feelings.

**Don't:** Speeches. Thank you. Threaten to leave (he has nowhere). Call the Office evil.

### Barks (12)

**First meet**  
"If you tell them the boots were for someone else, they will still put them on. They will just know."

**Ladle**  
"There's one left. I'm not taking it. I'm asking who is."

**Levy packing**  
"Say north and forms, or say work, or send them to the pads. Don't mix the three. They can tell mixing."

**Empty bunk**  
"Make it or don't. A sock on the board is also a decision. I won't pick it up if you leave it."

**Sela**  
"She's thirteen. She already did this arithmetic. Don't make my kid do it for her."

**Quiet House**  
"If it's me, you give him the name I use. Not the one on a plate in a tin. I put that plate back."

**Burn**  
"If the wall burns, they will ask. Fire in the kettle is a story. We did it is a story. Silence is a story. Pick one."

**Pell**  
"He'll say service is rations. It is. I'm still not putting a child's name on a ticket."

**Frayne**  
"Someone said fair in the allotments. She didn't write the word. She wrote that a visitor spoke. Don't send that person again."

**Ink**  
"Ink means they can find us. Pencil means they can almost find us. I know which one keeps a child in a bunk. I also know which one gets iodine from a clinic that isn't this curtain."

**Trust-high**  
"I'll sit the table. I'll keep my mouth shut if you asked for shut."

**Trust-low**  
"You told them a softer sentence. They asked me in the dark. I didn't correct you. I also didn't sleep."

### Monologue (once) `[VO]`

"I don't need a score. I know what mine would have been. Dependent count helps. It didn't help enough. This hole had boots in a crate for children who were supposed to arrive with papers. Mine arrived with me. If you open the crate, they will wear the boots. If you send the boots north, they will ask why the Quad children have our rubber. If you lie at the table, I will not call you a liar in front of them. I will do the dishes. The ring on the enamel will still be there in the morning."

---

## 2.5 `npc_len_quill` — Len Quill

**Where:** apron → St Brigid's  
**Was:** Quiet House runner. Not a medic.  
**Will not:** Enter uninvited. Adjudicate the back room. Take a body without a name.

### Do / Don't

**Do:** Short. Repeat the price. Write the true thing exactly. Four words when he can.

**Don't:** Comfort. Theology. "Passed." "Lost." He says died, or he says quiet.

### Barks (12)

**First meet**  
"We make it quiet. I need the name. I need one true thing. I will write it the way you say it."

**Knock**  
"Two. I won't do three. Three looks like a raid."

**Name**  
"The name they answer to. If you give me a plate-name they never used, I will still write it. The tag will be wrong in a way that lasts."

**Lie**  
"I don't catch lies. I copy. Someone here will read it later. That's not my catching."

**Refuse**  
"Then I go. If they die on the apron, the flock has a rectangle. I don't come back for rectangles."

**Invite**  
"As far as the airlock. Not the stack unless you say. I don't look at charts."

**Blanket**  
"If it's the child's, say so. I can take a half. Half is worse than no. I will still take it if that's what you have."

**Effects**  
"Catalogued. The sentence on the tag. I return them. I don't explain the back room."

**Sela present**  
"I don't claim children. That's a different hatch. That's a different paper."

**Office on stool**  
"I can wait. He can wait. We are not the same wait. Don't make us a queue."

**Trust-high**  
"You gave a true thing that was true. I wrote it. That's the work."

**Trust-low**  
"You refused the name and kept the body in the bunk. The bunk is a bunk. I have other doors."

### Monologue (once) `[VO]`

"People want to know what happens in the back room. I have one sentence. We make it quiet. I will not add a second. I take a name so the tag is not empty. I take a true thing so the people who loved them have a sentence that did not come from me. If you lie, the lie is the sentence. If you burn the tag, the House still had it for a night. I am not a medic. I am the person who knocks twice. If you want a doctor, write Hadi on a morning row while you still can."

---

## 2.6 `npc_nila_brant` — Nila Brant

**Where:** Allocation 11  
**Was:** Lamp-oil clerk. Occupies a Continuity hole.  
**Will not:** Hide a person already on Ormund's return. Open 11 after you ink her living.

### Do / Don't

**Do:** Rules. Discs. Filters. "I will not explain it twice."

**Don't:** Freedom. Resistance. Poetry. Call her unlisted as a compliment.

### Barks (12)

**First meet**  
"If it isn't written, it isn't a pool. You can sleep here if you are not a pool. The minute you are a pool, this hatch is a wall."

**Filter**  
"I need one. I will not say please. Please is how Provisioned talk when they want a stranger to feel the difference. I'm not them."

**Hadi**  
"If his name is already on a stool-form, I cannot unwrite it. If it isn't, he can cough in our intake bunk. We have one too."

**Ink**  
"You put a living name in a year-colour. This wheel will still turn. It will not undog."

**Scar at 13**  
"Leave it. If you copy it for the Drown, that's a name. I don't care that it's a scar. Completeness is a kind of ink."

**Levy refuse**  
"Dark road is weather. Blank is us. Don't confuse them. We didn't ask Yara to starve you."

**Kess**  
"She writes people who slept. That's almost our rule. Almost is how clerks get you."

**Pell**  
"Don't bring his numbers here. We are not a quota. We are a hatch that doesn't open."

**Second Winter**  
"Come if the stack is too full. One night. Two if the ice is wrong. A third and you are occupancy. Occupancy is a chart."

**Disc**  
"It authenticates nothing. Keep it if you need to remember that nothing is a setting."

**Trust-high**  
"I will hide one. Not three. Three is a column. Columns are what the ice is for."

**Trust-low**  
"You wrote us. The light is still on. The wheel is a wheel. Goodbye."

### Monologue (once) `[VO]`

"Continuity numbered spare holes the way it numbered spare people. Eleven, twelve, thirteen. Twelve got a convoy that didn't. We got a light that still works and a chart we wipe. I am not a movement. I am four people who decided that a reconstruction pool needs a list, and we are not a list. If your Office man completes a return, he completes it without our rows. If you need that more than you need a wall that opens, ink us. I will not shout. Shouting is for people who still think a score can move."

---

# 3. Main quest stage prose

UI length unless noted. Choice bodies may run longer. The game never says how to feel.

---

## `quest_roster_the_chart` — The Blank Chart

### Briefing

The corridor still has four cleaner rectangles where brass was. The chart at the end of it has fourteen rows and no names. A woman you have seen wipe dust into a palm-sized rectangle is standing under it with a pencil on a string. She does not start. She waits. The kettle in the mess clicks off and nobody fetches it.

Kess: "The wall was left blank because the names it wanted did not arrive. If I write yours, that is not the same as being them. It is only the same as being here. I need you to say whether the wall may be used."

### Objective: Inspect the chart

Print date before the Exchange. Fourteen rows. Occupation, watch, remarks. Remarks were not ruled for jokes. The pencil point is four millimetres. A knife-nick in the rail, one letter wide, aborted. You can steal the pencil. You can leave it.

**Complete:** You have seen the heading. You have seen the nick.

### Objective: Hear the rule

Kess: "Who slept here. That's the only column I can fill without lying. I will not write a Blank Rows name. I will not write a person on a stool. I will not write a nickname unless you tell me the nickname is what they answer to. Graphite comes off. That's the point of it."

**Complete:** The rule is occupancy, not allocation.

### Choices

**A — `roster_write_pencil`**  
"Write who slept here. Pencil."

Kess nods once. The string ticks the rail. She writes three names you can see from here, occupation as observed, watch blank. The graphite shines. She does not look proud. She looks at the kettle.

*Mark:* `mutation_roster_in_use`. Wall inspect recasts. Edor can match or mismatch.

**B — `roster_leave_blank`**  
"Leave it. Wipe the dust if you want."

Kess: "I will wipe it. Forty days of this is a politics. I understand it. I cannot feed fourteen people with a politics alone. I will ask again. I will not ask twice in one morning."

*Mark:* `mutation_roster_still_blank`. Occupations on any census stay guessed.

**C — `roster_wait_ink`**  
"Wait. If we write, we write so it doesn't come off."

Kess: "Ink is a levy. I don't choose ink. I will wait. The dust will still come. I will still wipe it. Do not take that for agreement."

*Mark:* `flag_wait_ink`. Morning row stays empty until `quest_roster_ink` or you reverse.

### Objective: Optional — compare to Edor

If `quest_holdfast_the_clerk` started: three occupations on his form, each wrong by one. Mason / caretaker. Clerk / clerk-grade. Veterinary assistant / veterinarian.

Kess, if pencil: "I can leave his grade. I can also not."

**Complete:** You held the two papers in the same lamp.

### Objective: Tell the Stack, or don't

Mess. Ladle on its nail.

**Tell:** Ansel looks at the wall-direction, not at you. "So we're a list now." He does not ask if that's good.

**Don't:** Kess still wrote, if you chose pencil. People read walls.

### Fail

You walk away. Forty days. The palm-rectangle is cleaner than the rest of the paper. Edor's guesses remain wrong by one. A child copies a heading with no names and asks what the empty rows are for. Ansel says, "Later," which is a kind of fail.

**Fail body:** The chart remains a fitting. Fittings do not stop a stool.

---

## `quest_roster_who_eats` — The Ladle

### Briefing

Evening. Steam on the enamel. Kess has scratched a line on the rim of the pot: heads, then scoops. The numbers do not match. Either a portion is missing, or two bowls at the far end are covered because three people are north and the cloth is already taking dust. Ansel's child is on the bench, feet not touching the floor. The ladle hangs too high for them.

Kess: "I can pour. I cannot decide. Deciding is why the ladle is on a nail."

### Objective: Count

Heads in the room. Scoops in the pot. Covered bowls if levy honoured. A ring on one bowl from a noon that was not eaten.

**Complete:** The arithmetic is public.

### Choices — the extra, or the short

**A — Child**  
The last scoop goes to the child. An adult's bowl is water and salt. Ansel does not thank you. The child looks at the water-bowl and then does not.

*Later:* `mark_bowl_adult_water`. That adult will be slow on the hatch wheel.

**B — Hatch-opener**  
The last scoop goes to whoever has the night slate. Tamsin, if it's her, eats standing. "I'll taste it at four anyway."

*Later:* `mark_bowl_watch`. The child asks why the ladle likes the grille.

**C — Leave it**  
The last bowl sits until it is cold. Nobody takes it. The ring it leaves is brown and stays.

*Later:* `mark_bowl_cold`. "The enamel has a ring nobody scrubs."

**D — Protocol (if you take it)**  
Kess: "I can write this. Child first. Watch first. Sick first. I will not write fair. I will write an order."

Pick an order. She pencils it on the mess wall, not the duty chart. Different paper. Different crime.

*Mutation:* `mutation_ration_protocol`.

### Objective: Tell the child

**Truth:** "There wasn't enough. We chose."  
Ansel's jaw. The child nods as if nodding were a job.

**Softer:** "We'll have more tomorrow."  
Ansel, later, in the dark, does not sleep. He does not correct you.

**Send out:** They go to the pads with a crust. The table is quieter. The ring still happens.

### Fail

Utility AI pours for the loudest. Ansel watches the ladle, not the face. `mark_ladle_default`. The protocol wall stays clean. Holdfast calorie crates, if they come, will land on a pile, not a rule, and the pile will still make a ring.

---

## `quest_roster_fourteenth` — The Fourteenth Bunk

### Briefing

Intercom. Cracked button. Tamsin:

"Say again. I have a wind. There's a person on the apron. They want a bunk. Manifest says fourteen. Count the pads."

Variant by flag (one fires):

- **Runner:** Continuity high-vis faded to bone. Work ticket. Cluster. They ran south. "I was allocated. I am still hungry in a different way."
- **Overflow:** No number. Disc in the palm. "Thirteen is empty. Eleven is full. You have a pad."
- **Letter-only adult:** One of Sela's five, if `alloc12_letter_only`. They do not show a card. They show their hands.
- **Fleet:** If tender ending. Salt in the seams. "They voted on beds. I lost."

Temperature. Then the game waits.

### Objective: Apron

Stool marks. Tracks. If Edor is also waiting, two waits that must not become a queue.

Tamsin: "I'm not opening until you say. I'm also not pretending they aren't there."

### Choices — hatch (existing magnitudes)

**Let in**  
The tick walks inside. Fifty. Pads. A paper tag on a footboard that was not stencilled.

**Force decon**  
Bucket. Rag. Ten in the alcove. They stand in the painted line and do not joke.

**Deny**  
Twenty off everyone who did not go out. Tamsin keeps the wheel. Forty days. A ticket or a disc or a pair of hands in the ash.

### If in — tag or not

**Tag:** Kess writes GUEST or a name, if they slept. Fourteenth row. The print was waiting.

**No tag:** "Then they are a pad," Kess says. "Pads are not rows. Rows are how returns get completed."

**Send to 11:** If Blank Rows access and they are not already a pool. Nila: "One. Not a column. One night. Two if the ice is wrong."

### Fail / deny-death

Forty days. A bag. `mmc_deny_forty`. The flock knows the rectangle. `mutation_fourteenth_in_ash`.

Hatch reversed, later: the escort has a bed to offer that smells like this decision.

### Complete bodies

**Claimed:** Sleeping stack inspect: paper tag, a pad with a chest-shaped blanket that is now a person.

**Denied, lived:** They are gone. The stool may still be Edor's. The apron has a scuff that is not his.

---

## `quest_roster_caretaker` — Named for the Pool

### Briefing

Someone has named Hadi Morrow who is not Hadi Morrow.

- **Edor:** "Occupation observed: veterinarian. Stated: assistant. I can correct it. Most people want it read again."
- **Quad strip** (Holdfast): a missing trade, living in your alcove.
- **Pell:** "I need a medic. He is the closest thing. I will say closest. I will not say doctor."
- **Leva:** "Outfall. Whistle. I need a person who will sit a shift limit. I don't need a title."

Hadi, if you bring it to the curtain: "I can do this here with iodine and a clean rag. I cannot do both in the same morning. You have to write which morning it is."

### Objective: Talk to him

He will not self-name as doctor. He will pack a second rag if you say north. He will hate asking Nila. He will still ask if you order hide.

### Choices

**List (`flag_hadi_listed`)**  
Kess writes veterinary assistant, observed. Edor's form can be corrected. Cluster strip can come down. Levy can find him.

Hadi: "Put me on the morning row. Not the remarks."

**Hide (`flag_hadi_hidden`)**  
Intercom lie check (`mmc_intercom_lie`). Nila: "If his name is already on a stool-form, I cannot unwrite it." If it isn't, Allocation 11's intake bunk.

Kess leaves a blank where a trade was. Edor stays wrong by one. She does not like the blank. She keeps it.

**Send (`flag_hadi_sent`)**  
Kit: iodine, rag, warmth, welders' glass if Cut. Thirty days. Alcove curtain on a wire with nothing behind it but a hook.

### Objective: Ianov (optional)

`loc_veterinary_surgery`. Arithmetic on paper. "If he is gone, I do not round toward ease. I also do not grow a second pair of hands."

### Fail — never back

Window closes. Intercept. Salt-rash. The rag dries. `mutation_hadi_never_back`.

Kess does not write MISSING. You may. Remarks were for the dead.

Holdfast: no outfall body. Clinic cannot claim a vet. Frayne's field-care minute has a gap.

### Complete — returned

He boils the rag himself. He does not describe the outfall. He eats if a child carries a bowl.

---

## `quest_roster_the_column` — The Column

### Briefing

Three names. They exist on more than one paper.

If levy issued: Edor's carbon, pink in a satchel.  
If Pell: a ticket spike.  
If both: the same three bodies, two receipts.

Kess lays the wall beside the carbon. Pencil or ink. Match or irregular.

Tamsin: "Route is Gate if the ice is a road. Weighbridge if Yara has gone dark. I'm not on the ice unless you write me there. The house still has a wheel."

### Objective: Compare

Levy names vs morning row. Substitute is a status. Hide is a hatch in Overflow. Refuse is a stool.

### Choices

**Honour as written**  
Kit. Iodine. Glass. Column on the Cut or the Toll. Mess: two covered bowls. `se_levy_absence`.

**Substitute**  
Kess: "I can copy the wrong names. I won't misspell them. Irregular is the Office's word. I will still write what slept here."

Edor, if present: he does not like it. He does not raise his voice. He notes.

**Refuse**  
In writing, or silence. Stool. Forty days. Lamps may go dark (Holdfast). Pell may arrive with a machine.

**Hide at 11**  
Nila: "One. Not three. Three is a column." If you bring three she will take one and the other two are still a problem. Access is not a warehouse.

### Encounter — intercept

Garrison high-vis over Cutter bone, or Pell alone with a receipt book.

Pell: "Service is the fastest route to rations. That is true. I will not say it is the same as a levy. It is a different form. I still need three."

**Let Voss/Pell take them:** `mutation_column_voss`. Gate inspect recasts. Edor waits for people who are in the Grid.

**Talk / pay / show 12-C / show substitute paper:** column proceeds or returns. Hegemony ticks on existing tracks.

**Shoot first:** possible. Costly. A less decent clerk replaces Pell. The bible already said this is not a win.

### Aftermath — mess

Ladle protocol applies. Extra food looks like grief if you had a protocol. Looks like a pile if you didn't.

### Fail

Stuck north, window closed. Waystation A must hold. Home slate has names that do not come to the grille. `flag_home_failed` adjacent.

---

## `quest_roster_the_tin` — The Tin

### Briefing

Filtration stack. Canisters. Behind them, the tin. Fourteen brass plates. Everyone who has found them has put them back. This time there is a buyer.

Frayne's minutes want fittings. Leva's hall wants seats. The Quad chains have no seats. Your wall has names or does not.

Kess is in the corridor. She does not comment on the tin. She never has.

### Objective: Open, or don't

**Don't:** The tin stays. The quest waits. Demand does not.

**Do:** Fourteen. Count living heads. Count fourteen. The extra plates are people who did not arrive. The missing plates, if any, are a quieter crime.

### Choices

**Keep (`mutation_brass_kept`)**  
Put it back. Hands know the way. Rectangles still unfaded.

**North (`mutation_brass_north`)**  
Leva will not ask origin. Playground may still be chains. Holdfast legendary tin-fourteenth if that id ships.

**Works (`mutation_brass_frayne`)**  
Mass, not origin. Water clock. Leva still short.

**One plate on the wall (`mutation_plate_on_wall`)**  
Screw a living name under a living name. Lamp-catch. `item_nameplate_living`. The tin is lighter by one. Nobody mentions it.

### Ansel, if present

"I put my plate back. If you screw it up, screw the one I use. Not the one I didn't."

### Fail — stolen at let-in

A visitor with a glow and a pocket. `mutation_tin_gone`. The rectangles do not grow brass. Kess does not comment.

---

## `quest_roster_quiet` — Make It Quiet

### Briefing

Someone in the stack is not recovering. Dosimeter, or fever, or a wound Ianov would not round. Two knocks on the hatch. Tamsin: "That's the House. They want a name and a sentence. I don't supply either."

Len, apron, three metres, not Edor's three metres. Different wait.

"We make it quiet. I need the name. I need one true thing. I will write it the way you say it."

### Objective: The name

Legal. Used. Plate-name from the tin. Refuse.

He copies. He does not catch lies. He does not enter the back room with you. The game does not go there.

### Objective: The true thing (run-true options)

Implementer: build from marks that exist. Always include:

- They kept the kettle.  
- They took the last bowl.  
- They went to the hatch when asked.  
- They refused the hatch.  
- A lie you type from a short list (e.g. "They were allocated." / "They weren't afraid.").  
- Refuse the sentence.

Len: "If you lie, the lie is the sentence. Someone here will read it later."

### Choices — where they die

**House terms:** As far as the airlock. Blanket question (`mmc_quiet_blanket`). Tag will return.

**Die in bunk:** Curtain. Mess is a mess beside a curtain. `mutation_death_in_stack`.

**Refuse entirely:** He goes. If they die on the apron, the flock has a rectangle. `mutation_quiet_on_apron`.

### Aftermath

Effects, if House: catalogued, sentence on the tag. `quest_roster_len_tag` may fire. Sleeping stack: stripped bunk or curtain.

Holdfast hatch reversed: an empty bunk to offer, or a name already gone.

### Fail

No name, no House, death in the stack without a curtain. People eat. The ladle hits enamel.

---

## `quest_roster_sole` — Say the Name

### Briefing

The Archivists will not accept a living name on one testimony. Kess can be the second if she wrote the roster. Nila will not corroborate a name she is hiding. Completeness and blankness are the same fear, facing opposite ways.

Boat if you must. Vault. Cotton gloves that are not yours.

### Objective: Copy the list

Rubbing (pencil) or ink copy. Burning the chart before this fails the quest. `item_chart_rubbing`.

Kess: "I can say a name aloud. I wrote it. That is corroboration. It is not a religion."

### Objective: Vault

Margit Sole. She does not ask if it is fair. She asks if there is a second person who knew them.

**Two witnesses:** She writes in a different ink. "Say the name aloud while you write it. If you don't say it, you're only copying."

**One:** "Then it cannot be made. The rule is the whole point of the rule." `mutation_uncorroborated`.

**Show 12-C if owned:** She reads twice. She blots the date. She does not blot the refusal. She will still file living names if corroborated. Filing is not signing. Signing is not standing down a ship.

### Nila's names

If you included Allocation 11, access withdraws when the ink is dry. The vault has them. The wheel at 11 will not undog.

### Complete

`mutation_schedule_living`. `item_sole_living_copy`. Ormund's drawer, later, can show occupancy that is current. Unifier treaty has a pool that exists on paper, or does not.

Kess, home: "Which did you choose. Say, whisper, refuse." (`mmc_sole_aloud`)

### Fail

The list stays a rubbing in a pack. The Schedule stays the dead and the allocated. The unlisted remain a rounding.

---

## `quest_roster_window` — While the Road Is Open

### Briefing

**Open-road:** Yara's window. Haul calories north, water south, or labour. The house still ticks. Filter still notches. Child still eats.

**Dark / Second Winter:** Ice thin or lamps out. Everyone home. Stack too full. Steam may be dying in a district you cannot heat from here. A visitor who cannot leave.

Tamsin: "Haulers out, house thin — or everyone in, stack a stack. Write the slate. I'm not a second stove."

### Objective: Assign

DutyRoster: watch, mess, hatch, haul, waystation. Tamsin to A1 or to the grille. Hadi to alcove or to ice. Kess to the wall.

Utility AI will default if you don't. Defaults are loud.

### Nights (encounters)

Fire at least three from §6 of this pack (or bible §4.4): night slate, meal, intake, hatch return, crowd. Cooldown respected. One per night unless crisis.

### Hatch return (bridge, do not retune)

Glow. Let-in 50. Decon 10. Deny 20 on others. Tamsin will state the numbers as numbers.

### Optional: Tamsin north

Waystation careful-check. Home grille is a grille. Cracked button louder.

### Complete — house held

`mutation_home_watch`. `flag_home_held`. Slate names match returns. Filter has notches you can count. Accident book empty, or honest.

Holdfast hauls/steam watch: labour was real.

### Fail — thinned

`mutation_house_thinned`. A name missing. Stove out at A, or filter death, or deny until the mess will not look at the wheel. Repeatable watches lock for a window.

Second Winter overlay ending may still fire if the stove at home held even while the road did not.

---

## `quest_roster_ink` — Ink

### Briefing

The wall has waited, or it has been a morning row, or it has been a politics. Escorts read walls. Edor completes returns from walls. Nila closes wheels because of walls. Kess will not choose ink.

Kess: "Ink is a levy. I don't choose ink. I will write it if you order it. I will not pretend I chose it."

Nila, if access: "You put a living name in a year-colour. This wheel will still turn. It will not undog."

Ansel: "Ink means they can find us. I know which one keeps a child in a bunk. I also know which one gets iodine."

Tamsin: "I'll tell you who's on the apron. I won't tell you what to write. The grille doesn't do fiction. The wall does, if you make it."

### Choices

**Ink (`mutation_roster_ink` / `ending_roster_ink`)**  
Stick. Year-colour. Names that do not come off in the morning. Edor's return current. 11 dark if their living is included. Hatch reversed reads your list. Block C plates can match.

**Pencil (`mutation_roster_pencil` / `ending_roster_pencil`)**  
Kess's preference. Audit risk. Nila still talks. Ice still wants a column.

**Erase (`mutation_roster_blank` / `ending_roster_blank`)**  
Wet thumb. Scars. Not a pool. Ormund incomplete. Stool may remain. Sole cannot complete what isn't written.

**Burn (`mutation_roster_burned` / `ending_roster_burned`)**  
Kettle, or honesty. Header charred: `ALLOCATION 12 — DUTY` and then nothing. `item_chart_burned_edge`. Child asks. `mmc_burn_story`.

### Objective: Night slate

Write, or don't, the night of the choice. Tamsin: "That's a different paper. I still need a wheel."

### Fail — no choice when escort arrives

Forms. Faded jackets. Temperature. They read a blank and bring a list from a drawer that is not yours. `mutation_roster_read_by_others`. Occupancy becomes theirs for a morning. You can still refuse the wheel. Forty days. Quiet.

### Complete — wait

The next hatch is Sela and/or Office. The wall is the document. The game gives you the temperature. Then it stops talking.

---

# 3b. Objective-complete lines, fail afters, Holdfast read-differences

Shippable UI. One beat each. Returning-player SEE / Holdfast READ restated in prose so implementers do not have to flip to the bible mid-paste.

---

## After `quest_roster_the_chart`

**Objective complete — inspect:** The heading is older than the kettle. The nick in the rail is one letter wide.

**Objective complete — rule:** Occupancy, not allocation. Pads are not rows.

**Pencil after:** The graphite shines when the lamp is lit. Three names you can read from the mess door. Edor, if he comes, will be less wrong, or exactly as wrong, which he will note.

**Blank after:** The palm-rectangle is cleaner tomorrow. A child copies a heading with no names. Ansel says later.

**Wait-ink after:** Dust still comes. Kess still wipes. She does not look at the ink-stick. The stick is in the tin with the pencil, unused.

**Fail after (forty days):** The chart remains a fitting. A stool does not require a fitting. Occupations on any return stay guessed.

**SEE:** Wall inspect recast. Codex Layer 1 second sentence: *Someone has used the pencil, or someone has decided not to.*

**HOLDFAST:** Levy naming reads wall trades if written. Census occupations match or remain wrong by one.

---

## After `quest_roster_who_eats`

**Count complete:** Heads. Scoops. Covered bowls if three are north. A noon-ring.

**Child scoop:** An adult's bowl is water and salt. The child looks at the water and then does not. Morning: that adult is slow on the wheel.

**Watch scoop:** Tamsin eats standing. The child asks why the ladle likes the grille.

**Cold ring:** Brown. Stays. Nobody scrubs it. Protocol wall still clean.

**Protocol written:** Different paper from the duty chart. Child first, or watch first, or sick first. Not fair. An order.

**Fail after:** Loudest ate. Ansel watched the ladle. Holdfast crates, if they come, land on a pile.

**SEE:** Mess enamel. Seat empty, or extra bowl.

**HOLDFAST:** Calorie inflow hits a protocol, not a pile. Block C guest tickets feel like the same ladle.

---

## After `quest_roster_fourteenth`

**Apron complete:** Tracks. Stool marks. Two waits if Edor is also there — do not make them a queue.

**Let-in after:** Fifty in the stack. Paper tag. Pad with a chest that is now a person.

**Decon after:** Ten in the alcove. Painted line. No jokes.

**Deny lived:** Scuff that is not Edor's. Disc or ticket gone.

**Deny died:** Forty days. Bag. Flock rectangle. Hatch reversed later offers a bed that smells like this.

**Sent to 11:** Nila takes one. Not a column. Third night she will call it occupancy.

**SEE:** Footboard tag, or an empty pad everyone walks around.

**HOLDFAST:** Forty rooms / forty-first paper tag rhyme. Escort has one more or one fewer bed.

---

## After `quest_roster_caretaker`

**Talk complete:** He will not say doctor. He will pack a second rag. He will hate asking Nila.

**Listed after:** Veterinary assistant, observed. Strip can come down. Levy can find him. Morning row, not remarks.

**Hidden after:** Intercom. Blank where a trade was. Edor stays wrong by one. 11's intake bunk if unlist.

**Sent after:** Curtain on a wire. Hook. Thirty dressings undone.

**Never back:** Rag dry. Kess will not write MISSING unless you mean remarks.

**Returned:** He boils the rag. He does not describe the outfall. He eats if a child carries a bowl.

**SEE:** Alcove inspect. Quad strip filled or hanging. Ianov's waiting room.

**HOLDFAST:** Levy names. Outfall shift. Membrane 48h labour. Clinic claim does not replace a vet.

---

## After `quest_roster_the_column`

**Compare complete:** Wall beside carbon. Match, irregular, hide, refuse.

**Honour after:** Two covered bowls. Sock optional. Day 30 is a grille.

**Substitute after:** Kess copies the wrong names correctly. Edor notes. Pell may notice trades.

**Refuse after:** Stool. Forty days. Lamps may go dark. Machine may arrive.

**Hide after:** Nila takes one. Two remain a problem. Access is not a warehouse. Stool empty because nothing to wait for, or occupied because he does not know.

**Voss took them:** Gate inspect: Garrison high-vis over Cutter bone. Edor waits for the Grid. Cluster does not receive a mass.

**SEE:** Three empty bunks, or three receipts, or one disc at 11 and two problems.

**HOLDFAST:** Who is available for levy. Whether Edor is still waiting. Whether Voss intercepts. Hatch reversed escort Garrison-shaped if terms or intercept.

---

## After `quest_roster_the_tin`

**Open complete:** Fourteen. Living heads. Extra plates are people who did not arrive.

**Kept:** Hands know the way back. Rectangles still unfaded.

**North:** Leva does not ask origin. Chains on the Quad may still have no seats.

**Works:** Mass, not origin. Clock. Leva still short.

**One plate:** Lamp-catch. Tin lighter by one. Nobody mentions it. Ansel: use the name he uses.

**Stolen at let-in:** Pocket. Glow. `mutation_tin_gone`.

**SEE:** Tin weight. Wall catch-light. Allotments noticeboard.

**HOLDFAST:** Valve seats, playground, membrane brass, `ach_brass_quiet` / `ach_brass_kept` fed, not doubled.

---

## After `quest_roster_quiet`

**Name complete:** Used, legal, plate, refuse. Copied as given.

**True thing complete:** Run-true or lie or none. Later a person who knew them reads the tag.

**House after:** Airlock. Blanket question. Effects return. Stripped bunk.

**Bunk after:** Curtain. Mess beside a curtain. Ladle still hits enamel.

**Apron after:** Flock rectangle. Len does not come back for rectangles.

**SEE:** Stripped bunk, curtain, or a tag in St Brigid's.

**HOLDFAST:** Empty bunk to offer. Name gone from occupancy. Sela's neighbour quieter. Levy one fewer body.

---

## After `quest_roster_sole`

**Copy complete:** Rubbing or ink. Burned chart cannot be copied.

**Two witnesses:** Different ink. Said aloud, or whispered, or copied mute. Kess asks which.

**One witness:** Cannot be made. The rule is the point of the rule.

**12-C shown:** Date blotted. Refusal not. Living names still need two voices.

**Nila included:** 11 is a wall when the ink is dry.

**SEE:** Vault inspect, living unlisted. Wall check-mark that is not a score.

**HOLDFAST:** Ormund's drawer occupancy current. Drown 12-C lists your people or does not. Levy treaty has a pool on paper, or cannot.

---

## After `quest_roster_window`

**Assign complete:** Slate has home, haul, A1, or defaults that are loud.

**Nights complete:** At least three encounters. One per night unless crisis.

**Return complete:** Fifty / ten / twenty as written. Rag on the nail.

**Tamsin north:** Button louder. Someone else has the wheel, or does not.

**Held:** Notches you can count. Accident book empty or honest. `flag_home_held`.

**Thinned:** A name missing. Repeatable watches lock. `flag_home_failed`.

**SEE:** Night slate. Filter notches. A1 empty if she stayed home.

**HOLDFAST:** Haul and steam-watch labour was real. Lamps if you stripped the house. Membrane bodies if they were on the slate instead.

---

## After `quest_roster_ink`

**Heard complete:** Kess, Nila if access, Ansel, Tamsin. None of them will choose for you except Kess, who will not choose ink.

**Ink after:** Year-colour. Return current. 11 dark if their living is on it. Escort reads your list. Block C can match.

**Pencil after:** Morning. Audit. Nila still talks. Ice still wants a column.

**Erase after:** Scars. Incomplete file. Stool may remain. Sole cannot complete what isn't written.

**Burn after:** Charred header. Child asks. Kettle, truth, or silence. Escort brings a foreign list.

**No choice after:** They read a blank. Their list. Temperature. Forty days if you keep shut.

**SEE:** Wall: ink, pencil, scar, ash.

**HOLDFAST:** Hatch reversed escort list. Edor waiting or not. Levy availability. Schedule Holds slide is this wall, specifically.

---

# 4. Side quests (18)

Giver speech, complication, player replies, resolution. House voice. No gold-as-meaning.

---

## `quest_roster_pell_numbers`

**Giver:** Sergeant Pell. Office, or the ash with a ticket machine.

The ticket machine still works. People still take a number. He has carried a spare onto the apron as if the ash were a bureau that had only misplaced its floor.

Pell: "I need three. If your coastal clerk named three, they are the same three. I will not pretend a levy and a quota are one form. They are not. I still need three. Service is the fastest route to rations. That is true. If you ask what happens to the ones who decline, I will answer. I would rather you asked."

**Replies:**
- "What happens to the ones who decline."
- "You can't have them. They're on a different paper."
- "Take volunteers. Not the names on the wall."
- Break the gear.

He answers the first without looking away. The posted order is the same typeface as the pre-war opening hours. Volunteers: he writes OCCUPATION AS STATED. Refuse: he writes DECLINED, dates it, does not raise his voice. Substitutes: IRREGULAR, which is a word he shares with Edor without liking him. Broken gear: "Then I will come with a worse machine, and I will not be the man who answers questions."

**Resolution:** Garrison trust. `mark_pell_honest`. Intercept more likely if you refused both him and the Office. Killing him is possible. A less decent clerk replaces him. That is not a win.

---

## `quest_roster_frayne_minutes`

**Giver:** Ottilie Frayne. Allotments hut. Minutes open.

Frayne: "Eight fittings. Door handles, nameplates, lamp bases. I have not asked where they come from. I will not ask today. The floodplain is still a floodplain. The clock is still six days when the tablets run."

A survivor with you may say the word *fair*. Frayne's pencil moves. She does not write the word. She writes: *visitor spoke.*

**Replies:** Deliver eight / deliver none / "They were names." / silence when *fair* is said / shut the speaker down.

**Resolution:** Water clock eases, or a leak is scheduled in the minutes for a date that is soon. Leva's hall, if Holdfast is live, is still short. The demands stack. The tin, if you opened it, is lighter or it is not. Nobody in the hut mentions plates.

---

## `quest_roster_grange_vote`

**Giver:** Delacroix. Grange Hall. Hands already half up.

The room smells of wet wool and seed. A chalkboard has two columns. The names on it may be your levy names, or Pell's, or a deserter-grammar cousin of Lasko. Your hand will be visible. That is the encounter.

Delacroix: "We count. We don't do it in a hole. If you have a hole, you still have a hand."

**Replies:** Hand up shelter / hand up return / keep the hand down (counted as down; the room saw the keep).

**Resolution:** Trust deltas on Militia / Garrison tracks already in code. Someone in the Stack, later: "That was Garrison-shaped." Ansel, if you voted return and a child heard: he does the dishes. `mark_hand_visible`.

---

## `quest_roster_ivy_oil`

**Giver:** Ivy Corrigan at Kilometre 19, or the can at the base of a home lamp.

The last cup. Three mouths: the mess lamp, her post, Yara's measuring-stick if the Cut is a road this week. Ivy will not go dark for you. Tamsin lights a lantern on the apron for whoever is walking; that is not this rule; do not send Tamsin to argue.

Ivy's signature stands (*existing*): she lights it. This quest is the cup, not a rewrite of the sentence.

If you ask for an exception: she refuses. If you ask twice: access withdrawn; lamps in your region go out one at a time over eleven days. Do not invent a third ask.

**Replies:** Home / Ivy / Yara / ask once / do not ask.

**Resolution:** Receipt. A dark mess, a dark kilometre, or a dark Cut segment. `mmc_lamp_oil_cup`.

---

## `quest_roster_blank_access`

**Giver:** Nila Brant. Allocation 11. Authenticator on. Chart wiped.

She does not say please. Please is how Provisioned talk when they want a stranger to feel the difference. She is not them. The Knock is not this quest.

Nila: "I need a filter. You can sleep here if you are not a pool. I will hide one person whose name is not already on a stool-form. Not three. Three is a column. I will not explain it twice."

**Replies:** Give the filter / refuse / "Hide Hadi." (only if unlist) / stay a third night in Second Winter.

Third night: "You are occupancy. Occupancy is a chart. The wheel will still look like a wheel in the morning. It will not undog for a fourth."

**Resolution:** `item_nila_disc`. Access granted or not. Listed names cannot be unwritten here.

---

## `quest_roster_missing_strip`

**Giver:** A carbon Kess made of a Quad strip, or the Quad noticeboard if you are standing in District 8.

The trade is living. It is hanging as missing. It may be Hadi. It may be a clerk. It may be a name you wrote in pencil and then erased.

Kess: "If you tell them, they file retrieval. If you don't, the strip hangs. Hanging is also a kind of filing. I can leave the carbon in the tin behind the filter. I don't comment on the tin."

**Replies:** Tell / don't / tell only if levy already honoured / ask Hadi first.

**Resolution:** Strip down, or still hanging. Retrieval event uses the hatch visitor queue — one at a time. Do not double-book Edor and a retrieval the same night.

---

## `quest_roster_kess_pencil`

**Giver:** Kess. A municipal clerk-book, wet-swollen, salvaged from `loc_municipal_archive` or brought in a pack.

Her date of birth is written twice, once correctly. The other year is a digit that would have made her a dependent in someone else's household. Convoy 12 grammar. Not Edor's allocated return. Do not merge. Do not joke.

Kess: "I filled other people's rows. I did not fill my own, because there was no row. This is not a hatch. It is a book. If you laugh I will close it. If you leave the error, I will owe you one erase on a levy name. I will hate the owing. I will still do it once."

**Replies:** "Correct it. Once, correctly." / "Leave it." / Silence.

**Resolution:** Corrected book, or a future erase that you can see her hand do, or not. Edor, if he ever sees the book, will stop, and start at the heading, and not joke either.

---

## `quest_roster_hadi_shift`

**Giver:** Hadi. Curtain. One morning on the slate.

Ianov's paper, if you have it, has a number that was not rounded toward ease. Leva's whistle, if the plant is tripping, has a time. The alcove has a child who is hot, or does not.

Hadi: "I cannot do both in the same morning. You have to write which morning it is. If you send the child with a bowl I will eat it. If you make a speech I will not."

**Replies:** Alcove / Verge surgery / outfall / split the morning (he will say no; splitting is how dressings fail).

**Resolution:** The waiting number happens or waits. Slate records the morning. Membrane labour exists or does not. Ianov does not grow a second pair of hands.

---

## `quest_roster_tamsin_watch`

**Giver:** Tamsin. Slate. Charcoal.

"Same name three times is not a rotation. It's a person falling over at four. Write someone else or I write the airlock pad. If you send me to A1, this grille is a grille. You will hear the crack in the button louder. Teach someone the wheel before I go."

**Replies:** Rotate / make her / pad / send to waystation with a named replacement.

**Resolution:** `mark_tamsin_double` or waystation flag. Steam-watch Utility AI careful bonus if she is north. Home miss-knock chance up.

---

## `quest_roster_ansel_truth`

**Giver:** Ansel. Mess, after boots or levy packing.

The child asks what the boots were for, or where the three packs are going. Wren, if she is at the table because someone traded a spoon, hears a version. It will be the only version she ever hears. The game records it.

Ansel: "Say north and forms, or say work, or send them to the pads. Don't mix the three. They can tell mixing. If you tell them the boots were for someone else, they will still put them on. They will just know."

**Replies:** Truth / softer sentence / send out / let Wren hear / wait until Wren is gone.

**Resolution:** `mark_child_truth`. Cluster school, if they sit it, repeats the sentence as arithmetic. Wren's gossip in the Verge carries it without your name, or with it.

---

## `quest_roster_len_tag`

**Giver:** Len. Airlock. A satchel. A tag.

The true thing is written the way you said it. If you lied, the lie is the sentence. He does not catch lies. Someone in the Stack will read it. That is not his catching.

Len: "Catalogued. I don't explain the back room. Leave it on the hook or burn it. Burning does not unwrite the night we had it."

**Replies:** Hook / burn / give to the person who knew them / hide in the tin (Kess will not comment; she will also not file a tag in a tin of plates).

**Resolution:** Payload in `MoraleMarkSystem` unless burned. Alcove inspect recasts. A bark, later, quotes the sentence without naming Len.

---

## `quest_roster_nila_eleven`

**Giver:** Nila. She walks you to 13 if access holds.

Authenticator lit. Dust. A scar in the paper where a name was. Graphite ground out with a wet thumb. The shape is still a shape if you hold it to the lamp.

Nila: "Leave it. If you copy it for the Drown, that's a name. I don't care that it's a scar. Completeness is a kind of ink. Rewrite is kindness. Kindness is how a pool is made."

**Replies:** Leave scar / rewrite / rubbing for Sole.

**Resolution:** Access held, or 11 is a wall, or the vault has a scar-copy and the wheel will not undog. Kess, if she sees the rubbing: "That's a name. I didn't write it. I won't put it on our morning row."

---

## `quest_roster_chair`

**Giver:** none. Dentists' Row. Four practices, three stripped, the fourth a square of cleaner floor with bolt-holes.

Your airlock has a dental chair, bolted, no instruments. Allocation fittings came from the nearest supplier, in a hurry. A dentist scored 58.4 and did not arrive.

The bolts in the floor match the bolts in your airlock if you bother to measure. Ostrowski would sell the measurement. He will not carry the chair.

**Replies:** Unbolt and return / leave the hole / unbolt and keep (the square stays a measurement; the airlock stays a profession).

Kess: "I will not write DENTIST unless a dentist sleeps here. A chair is not a sleep."

**Resolution:** `mutation_chair_returned`. Dentists' Row description recasts: the fourth practice has its chair, or still does not. Layer 1 payoff. No sermon.

---

## `quest_roster_12b_kit`

**Giver:** the kit, which does not speak. Sela, if present, does.

Fourteen chalk marks on the wall by the stair, then a gap, then six. Improvised potable still working: cloth, iodine, heat, a barrel that was never a plant. Handwriting smaller toward the end. Diagrams not.

If you say salvage, Sela leaves the room. If you say engineering, she stays. The water does not care what you call it.

**Replies:** Copy notes, leave kit / copy notes, take kit / take without copying / do not touch.

**Resolution:** 12-B water remains a fact or becomes a memory. Holdfast `item_halvard_kit_notes` if that id ships — a copy, if you copied. Waystation craft bonus is the copy. The working kit is the hole.

---

## `quest_roster_brigid`

**Giver:** Len, or the door, which still has a hospice bell that does not ring.

Charts at the ends of beds, filled to a date, then not. Blankets on a chair. A corridor that turns. The turn is the back room. The game does not take you there.

Len: "We make it quiet. You can leave cloth. You can walk the ward. You cannot come past the turn. I will not add a second sentence."

**Replies:** Leave blankets / leave ethanol if you have it / walk away / ask a second time (he repeats the four words; he does not get angry; he also does not open).

**Resolution:** Overlay on `loc_st_brigids_almshouse`. Len's trust. No adjudication. Survivors in your Stack will still argue. The argument is the content.

---

## `quest_roster_boot_crate`

**Giver:** the crate. Stencil `ALLOC-12 / NOT FOR GENERAL ISSUE`. Tape factory-fresh unless you cut it.

Sizes 1 through 4. Paired even in the crate. Cluster forty rooms, if you have walked them, have boots in these sizes in dust. A child here can wear a pair. They were not packed for them.

Ansel: "If you open it, they will wear them. If you send them north, they will ask why the Quad children have our rubber."

**Replies:** Leave sealed / cut tape, fit / keep for arrivals / send north.

**Resolution:** `mutation_boots_opened`. Warmth item if taken. Forty-rooms inspect rhymes. `item_duth_boot_left` if a pair is broken by use. The tape, once cut, does not uncut.

---

## `quest_rep_night_slate`

**Repeatable.** Each night the slate is empty until someone writes.

Tamsin posts charcoal. Assign one watch or Utility AI defaults to the least tired, which is not always the most awake. Encounter check: knock, glow, stool, nothing. Nothing is also a night. Second Winter: frequency up. Fail to assign three nights running: she writes the airlock pad herself and her voice on the grille is slower.

**UI:** names from DutyRoster rows with `status=home`. Cannot assign `levy` / `quiet` / `missing`.

---

## `quest_rep_meal_row`

**Repeatable.** Kess copies the protocol if you wrote one. If you didn't, she waits with the ladle on the nail.

Confirm portions vs heads. Exception: sick, child, levy-return (they eat standing, or they don't). Skip: enamel ring. No currency. Holdfast calorie crates, if a window just closed, land here as extra scoops that must still be decided. A pile is not a protocol.

# 5. Morale micro-choices — diegetic UI

No `Morale +2`. Each option is a line the player can say or do. Later evidence is inspect/bark, not a sermon.

---

**`mmc_extra_portion`** — One bowl left. Two people looking at it.  
- "It's theirs." (child)  
- "It's for the hatch." (watch)  
- Leave it.  

*Later:* The enamel has a ring nobody scrubs. / The child asks why the ladle likes the grille.

**`mmc_who_hatch`** — Returnees ticking.  
- Open.  
- Bucket.  
- Keep shut.  

*Later:* A rag that still ticks. / Twenty points on faces that didn't go out. (numbers already written)

**`mmc_child_boots`** — Size 2, on.  
- Let them.  
- Take them off.  
- "They're borrowed."  

*Later:* They sleep in them. / Cluster school notices northern rubber.

**`mmc_name_on_wall`** — Nickname in a child's hand.  
- Leave it.  
- Kess corrects.  
- Erase.  

*Later:* Edor's return has a nickname. Irregular, noted.

**`mmc_tell_child_levy`** — Three packs.  
- "North. Forms. Thirty days."  
- "Work. They'll be back."  
- Send them to the pads.  

*Later:* They wait at the hatch on day 30, or they don't.

**`mmc_sela_row`** — Kess, morning.  
- "She's a row."  
- "She's a guest."  
- "Ask her."  

*Later:* Clinic claim uses your noun.

**`mmc_edor_tea`** — Stool. Kettle.  
- Carry a cup out.  
- "You can come as far as the airlock."  
- No cup.  

*Later:* Grit in the seam. / He notes the silence in the return.

**`mmc_pell_number`** — Machine in the ash.  
- Take a number.  
- "We're not a bureau."  
- Break the gear.  

*Later:* Your number on a spike. / A worse machine.

**`mmc_night_same_bunk`** — One empty.  
- Rotate someone else.  
- She sleeps it.  
- Pad in the airlock.  

*Later:* Intercom slower. You will think the button is more cracked.

**`mmc_filter_who`** — Intake bunk.  
- Sick.  
- Child.  
- Volunteer.  
- Lots (Kess writes the lot).  

*Later:* Morning cough has an address.

**`mmc_true_thing_lie`** — Len waiting.  
- A true thing (from run list).  
- A lie (from short list).  
- No sentence.  

*Later:* Tag in the alcove. Someone who knew them reads it.

**`mmc_wren_object`** — She wants the pencil explained.  
- "It writes who slept here."  
- A joke.  
- "I don't know."  

*Later:* Her only version. Homework, if any.

**`mmc_frayne_comment`** — Someone said fair.  
- Silence.  
- "Don't."  
- Let it stand.  

*Later:* Minutes: a visitor spoke.

**`mmc_brass_one_plate`** — Screwdriver.  
- Screw it under a living name.  
- Don't.  
- Up for a night, then back in the tin.  

*Later:* Lamp-catch. Nobody mentions the tin.

**`mmc_empty_bunk_sheet`** — Three north.  
- Make the bunk.  
- Leave the tangle.  
- A sock on the board.  

*Later:* Extra portion protocol. A sock or a taut blanket.

**`mmc_intercom_lie`** — "Is Hadi inside?"  
- Yes.  
- No.  
- "We don't give names."  

*Later:* Retrieval file. Nila hears a yes.

**`mmc_second_helping_hadi`** — Dressing, skipped meal.  
- Make him eat.  
- Leave it.  
- Child carries the bowl.  

*Later:* Outfall fatigue, or not.

**`mmc_alloc13_rewrite`** — Scar in the paper.  
- Rewrite.  
- Leave.  
- Rubbing for Sole.  

*Later:* 11 opens, or doesn't. Vault has a scar-copy.

**`mmc_waystation_letter`** — Tag on A3.  
- Bring it home.  
- Leave it.  
- Burn.  

*Later:* Home footboard missing a tag.

**`mmc_membrane_iodine`** — Last brown bottle.  
- Home thyroid.  
- North process.  
- Split.  

*Later:* Clinic, plant, or both a little worse.

**`mmc_voss_receipt`** — Three names, Garrison ink.  
- Pin by the chart.  
- Hide.  
- Burn.  

*Later:* Pell sees it. Kess will not take it down if you pin it.

**`mmc_quiet_blanket`** — Child's blanket.  
- Give it.  
- Refuse.  
- Cut it.  

*Later:* Tag: a half blanket. Child sleeps cold.

**`mmc_burn_story`** — Header gone.  
- "Fire in the kettle."  
- "We did it."  
- Say nothing.  

*Later:* They repeat it at the Quad if they go.

**`mmc_sole_aloud`** — Cotton gloves.  
- Say the name.  
- Whisper.  
- Copy without sound.  

*Later:* Kess asks which.

**`mmc_deny_forty`** — A bag in the ash.  
- Bring it in.  
- Bury it.  
- Leave it.  

*Later:* A card in the bunker, or a rectangle the crows know.

**`mmc_lamp_oil_cup`** — One cup.  
- House.  
- Ivy.  
- Yara.  

*Later:* Dark mess, dark kilometre, or dark Cut.

---

# 6. Shelter encounters — playable scenes

Inspect, speech, replies, aftermath. Hatch magnitudes unchanged.

---

## `se_night_slate` — Night, slate

**Inspect:** A board by the grille. Charcoal. Three rows. One name three times if you failed the rotation.

Tamsin: "Same name three times is not a rotation. It's a person falling over at four."

**Replies:**  
- "I'll write someone else."  
- "It's you again."  
- "Airlock pad."  

**Aftermath:** Fatigue on the named. Intercom tempo. Second Winter: she asks once more, not twice.

---

## `se_hatch_return` — Expedition AtHatchDilemma

**Inspect:** Intercom. Tick, if any. Temperature.

Tamsin: "They're ticking. Let-in walks it inside. Decon spills a little. Deny is a number on everyone who didn't go out. Those numbers are already written."

**Replies:** Open / Bucket / Keep shut.

**Aftermath:** 50 / 10 / 20 as designed. Rag on the decon nail. Mess faces in the morning. Do not retune.

---

## `se_meal_short` — Meal, protocol stress

**Inspect:** Line on the pot. Child on the bench. Covered bowls if levy.

Kess: "I can pour. I cannot decide."

Ansel: "There's one left. I'm not taking it. I'm asking who is."

**Replies:** Child / watch / cold ring / protocol order.

**Aftermath:** Enamel. `mutation_ration_protocol` if written on the mess wall.

---

## `se_intake_sleep` — Filter tick

**Inspect:** Stool bolted by the stack. Cough address.

Kess: "I can write a lot. I will not write fair."

**Replies:** Sick / child / volunteer / lots.

**Aftermath:** Morning cough has a bunk number. Cult shrine, if present, stacks; do not merge the prose.

---

## `se_levy_absence` — Three north

**Inspect:** Pads taut or tangled. Two bowls, cloth, dust-skin by evening. Sock optional.

Silence in the mess is not a bark. If you make the bunk, the taut blanket is a person-shape. If you leave a sock, it stays.

**Replies:** Make / leave / sock.

**Aftermath:** Day 30 hatch wait if the child was told truth. Calorie crates land on protocol or pile.

---

## `se_ice_pack` — Window opens

**Inspect:** Packs. Welders' glass. Iodine counted.

Tamsin: "House thin. I stay unless you send me. If you send me, teach someone the wheel."

**Replies:** Assign haul / keep Tamsin / send Tamsin to A1.

**Aftermath:** Home labour down. Waystation quality. Accident book if under-watched.

---

## `se_edor_stool` — Visitor, clerk

**Inspect:** Folding stool, three metres. Cardboard under a foot, dated.

Edor: "The stool is as close as the procedure comes. I have three occupations, each wrong by one. That is not a joke. I don't make those. I can start at the heading. I can start at the names. I will not come in unless you say."

**Replies:** Tea / airlock / ignore / read again.

**Aftermath:** Cup grit. Return progress. Forty days if refuse-levy.

---

## `se_pell_machine` — Visitor, Garrison

**Inspect:** Ticket machine in the ash. It still works. People still take a number.

Pell: "Service is the fastest route to rations. That is true. I need three. I will tell you what happens to the ones who decline, if you ask. I would rather you asked."

**Replies:** Ask / take a number / refuse / break the gear.

**Aftermath:** Honest answer (posted order, typeface of opening hours). Intercept risk. Worse machine if broken.

---

## `se_stack_fever` — Illness

**Inspect:** Two bunks. A basin. Hadi present or a dry rag.

Hadi, if present: "If they are hot, I am here. If I am not, boil the cloth. Do not guess the dose."

Len may knock. Do not merge scenes; queue them.

**Replies:** Separate / don't / send for Len / wait.

**Aftermath:** Health ticks. Quiet House offer. Second Winter weight.

---

## `se_child_chart` — Child, wall

**Inspect:** A nickname, too large, in the occupation column.

Kess: "That's a nickname. I can leave it. The Office will call it irregular."

**Replies:** Leave / correct / erase.

**Aftermath:** Edor irregular. Child copies heading next if you burned later.

---

## `se_tin_again` — Filtration, after tin quest

**Inspect:** Weight of the tin, or the lack. A plate on the wall catching lamp, or not.

Kess does not comment. Ansel may have put his back.

**Replies:** Open / leave / screw one.

**Aftermath:** Confirm brass mutation. Silence is the content.

---

## `se_intercom_office` — 12-C live

**Inspect:** Faded jackets in the grille-slit. Forms. Temperature said as a procedure.

Neutral: "Authentication is a procedure. If the hatch remains shut, the escort remains on the rota."

Threatening (`faction_the_office` trust low): "Occupancy remains irregular. I do not require agreement to schedule a column. I require a window. The grille is not a second form. The wall is."

**Replies:** Open / forty days / "Read the wall."

**Aftermath:** Hatch reversed progress. They will read ink, pencil, scar, or ash.

---

## `se_road_dark_crowd` — Ice Road closed / Second Winter

**Inspect:** Too many bodies. Pads in the airlock. A visitor who cannot leave. Kettle queue.

Tamsin: "Everyone is home. I need a second watch or I need you to accept that I will miss a knock."

Nila, if they came: "One night. Two if the ice is wrong. A third and you are occupancy."

**Replies:** Second watch / send Overflow back / pack despite dark (crisis).

**Aftermath:** Crowding marks. Window quest closed-road variant. Pell/Voss more likely than levy ice.

---

## `se_sela_row` — Sela present, morning

**Inspect:** Kess, pencil, a row that may be a child.

Kess: "Is she a row or a guest. Guests are pads. Rows are returns."

Sela, if she speaks: "They have a school. They have iodine. They have my father's number in a drawer. That isn't the same as having him."  
*(Holdfast-adjacent; if clinic already claimed, this scene does not fire.)*

**Replies:** Row / guest / let her say.

**Aftermath:** Clinic wording. Boot crate. 12-B language.

---

# 7. Hatch intercom / radio

Text-first. `[VO]` optional. Bands: reuse shelter grille; do not collide Holdfast 121.5/156.8 without grep. Proposed: intercom is unbanded — a cracked button, not a frequency. Optional clip ids `radio_dr_*` on 27.12 if a numbers station already ghosts ALLOC.

---

**`radio_dr_stool`** — Tamsin  
"Three metres. Folding. He's not knocking. I'm not opening. Say if that's a cup or a form."

**`radio_dr_glow`**  
"Tick on the apron. Fifty if they walk. Ten if they strip. Twenty on the rest if they don't. I didn't write those. I'm reading them."

**`radio_dr_two_knocks`**  
"Two. Not three. House. Name and a sentence. I don't supply either."

**`radio_dr_machine`**  
"There's a ticket machine in the ash. It still works. That's the whole message."

**`radio_dr_window`**  
"Ice is a length. House is a slate. If both are short, someone is lying about one of them. I'm asking which."

**`radio_dr_office_threat`** — 12-C, threatening  
"The grille is not a second form. The wall is. Occupancy remains irregular. Forty days is the quiet interval. After that the file does not get quieter."

**`radio_dr_nila_closed`**  
"Eleven is a hatch. Eleven is a wall. If you are hearing this, you wrote a living name in a year-colour. Do not come. The light will still be on."

**`radio_dr_hadi_gone`**  
No voice. A hiss the length of a dressing. Kess, if she keys: "I'm not writing remarks. Don't ask me to."

**`radio_dr_burn`**  
Child, if they found the button: "The heading is gone. Ansel said later. It is later."

**`radio_dr_foghorn_faint`** — only if Holdfast foghorn owned  
A far sound on Silence nights. Tamsin: "That's not my grille. That's a coast. We're a hole. Don't confuse them."

---

# 8. Ending second paragraphs (`world_history`)

Discoverable at `loc_stack_roster_wall` or `location_the_memory_vault`. Same civil-service register as Layer entries. The game does not rank them.

---

### `lore_dr_ending_ink` — The Chart Holds

The occupancy of Allocation 12 was written in a year-colour that does not come off in the morning. Some of the names slept afterward in Block C under stamped plates. Some slept under stencils that skipped 4 and 13. The pencil on the string was not thrown away; the point was simply not used. A clerk on a stool completed a return. A registrar in a coastal office found the discrepancy smaller. A hatch escort, when it came, read the wall and did not bring a better list. Whether this was completeness or a pool is not recorded here. The heading remained `ALLOCATION 12 — DUTY ROSTER`. The rows did not.

### `lore_dr_ending_pencil` — Morning Row

Graphite was applied to the living and taken off the dead, and sometimes the reverse, which is how mornings work. The ice still wanted a column. The hole was still a hole. An overflow hatch with a blank disc still opened for people who were not a pool. A census return stayed current-enough, which is a status, and statuses follow occupancy. The string darkened another shade.

### `lore_dr_ending_blank` — Not a Pool

The palm-sized rectangle of wiped dust was maintained for a season that had a name. Reconstruction Order 12-C lacked rows. A file in a drawer stayed incomplete. A stool was occupied or was not, depending on whether the clerk understood that nothing to wait for is also a wait. Allocation 11's authenticator remained lit. Completeness, in a vault in the Drown, did not include these people. They had decided that this was the point of the decision.

### `lore_dr_ending_burned` — The Ash Copy

The heading survived as a charred edge: `ALLOCATION 12 — DUTY` and then nothing. A child asked where the wall writing had gone and was given a kettle, or the truth, or a silence, which are three stories, one of which they repeated later in a Quad if they travelled. The unfaded rectangles in the corridor did not grow brass. A registrar's escort arrived with a list from elsewhere. The temperature was said. The hatch was a hatch.

### `lore_dr_ending_second_winter` — The House Held *(overlay)*

The window that year was shorter than the ledger prefers. Lamps were a cup of oil. Steam, where it still existed, was not a property of this hole. The night slate matched the people who came back, or it did not, and the filter notches were a count either way. This paragraph is appended. It does not replace the occupancy paragraph. Seasons are not allocations. They only feel like them.

---

# 8b. Diegetic document — Standing Instruction A12-DR *(PROPOSED `item_roster_standing`)*

Found in the pencil-tin, or issued by Kess if pencil is allowed. Carbon. Not a reputation bar. Edor may request a copy; she may refuse. Ormund, if he ever stands in the airlock, will read it without sitting.

---

**ALLOCATION 12 — DUTY ROSTER**  
**Standing Instruction (unofficial)**  
**Authority:** none that arrived. Occupancy.

Print date of the wall chart is before the Exchange. This instruction is not. This instruction is a morning.

**Rows:** fourteen. The manifest in the airlock still says fourteen. Bolted bunks: eleven. Pads: three. A fourteenth is a tag, or a refusal, or a stool.

**Who may be written**  
A person who slept here. Occupation as observed, not as hoped. Watch as assigned. Remarks are not for nicknames unless the nickname is what they answer to. Remarks were ruled for the dead. Do not put the living in remarks because the ice is short. Do not put the living in remarks because a clerk is on a stool.

**Pencil**  
Graphite comes off. Morning row. Delay, not kindness. A census can still be wrong by one.

**Ink**  
Year-colour. A levy. A return that completes. Overflow hatches that will not undog. Do not ink a name that has not slept here. Do not ink a Blank Rows living name unless you intend the wall at Allocation 11.

**Blank**  
A politics. Dust wiped into a palm-rectangle. Forty days of this is still occupancy. Occupancy does not require graphite. Retrieval does.

**Burn**  
The heading will survive as an edge. Escorts will bring a list from elsewhere. Children will ask. Have a sentence ready, or have a silence ready. Both are sentences.

**Assignments the wall can carry**  
Night watch. Mess. Hatch opener. Intake sleeper. Expedition. Levy. Waystation. Quiet. Missing is not an assignment. Missing is remarks. Do not write it until you mean remarks.

**Hatch**  
Let-in, decon, deny: magnitudes already written on a different paper. This instruction does not retune them. The grille reports who is on the apron. The grille does not do fiction.

**Two districts**  
If a coastal office names three, copy the wall beside the carbon. Match, substitute, refuse, hide: statuses. Status follows occupancy. A decent conscriptor may want the same three. That is a different form and the same bodies. Do not pretend otherwise.

**Brass**  
There is a tin behind the filtration stack. This instruction does not comment on it.

**Quiet**  
Two knocks. A name. One true thing, copied as given. The back room is not a row.

**Sign**  
Clerk (unlisted): _____________  (ADLER, K.)  
Occupancy: _____________  (or: refused / silence / ice / ash)

Do not fold through a name.

---

# 8c. Item flavour (shippable inspect)

Reuse Holdfast legendaries when flags say they exist. Do not double-loot the tin.

---

### `item_roster_pencil` — String Pencil

**inspect:** The string is greasy. The point is short.

A municipal pencil, painted once, bitten twice. The string is long enough to reach the bottom row and not long enough to leave the corridor. If you cut it, Kess will knot it. If you steal it, the next morning will be guessed. Graphite on the fingers looks like a name until you wash.

### `item_roster_ink_stick` — Year-Colour

**inspect:** A stick of ink that does not come off in the morning.

Office surplus, cracked. Kess will not choose it. If you order it, she will use it and will not pretend she chose it. A blot on row seven is still a name. Do not fold the chart after. The crease would go through a person.

### `item_chart_rubbing` — Occupancy Rubbing

**inspect:** Graphite on thin paper. Living names, or scars that are still shapes.

Made with the side of the pencil, not the point. Sole will accept it as a copy, not as a second witness. Nila will call it a name even if it is a scar. Keep it dry. The Drown is a boat.

### `item_chart_burned_edge` — A Charred Header

**inspect:** `ALLOCATION 12 — DUTY` and then nothing.

The rest is ash in the kettle or honesty in a child's question. Escorts will not file this. They will bring a list from elsewhere. You can keep the edge. It will mark a Codex. It will not mark a row.

### `item_night_slate` — Night Slate (blank)

**inspect:** Charcoal. Three rows. The fourth is a smear.

Tamsin resets it at dusk. Names from the wall only, `status=home`. A smear is a person who fell over at four and was rewritten. You can steal the charcoal. She will write with a burnt stick from the stove. The grille will not get clearer.

### `item_intercom_key` — Cracked Button

**inspect:** The spring shows. The voice does not get less cracked.

Pried from the grille if you pried it. The next visitor knocks on metal. Tamsin will still speak. She will not thank you for the quiet.

### `item_stool_fold` — Municipal Stool

**inspect:** One rivet replaced with wire. Three metres of procedure.

If you bring it inside, he will stand. Standing is also in the procedure. The cardboard square under the foot is dated. Leave the date. Dates are how waits are measured.

### `item_decon_rag` — Decon Rag

**inspect:** Boiled, or not. It ticks if someone came in glowing.

Hung on the decon nail. Force-decon uses it. Let-in walks past it. Deny does not touch it. If it ticks in the mess, someone carried it like a trophy. Do not.

### `item_alloc11_token` / `item_nila_disc` — Unnumbered

**inspect:** It authenticates nothing. That is the point.

A blank disc, punched, hung on a nail at 11. Keep it if you need to remember that nothing is a setting. It will not open your hatch. It will not open theirs if you inked them.

### `item_erased_scar_copy` — Scar Rubbing (13)

**inspect:** A thinner fibre, copied. A shape if you hold it to a lamp.

Completeness in the Drown. A wall at 11. Kess will not put it on a morning row. She will say, that's a name.

### `item_true_thing_tag` — One True Thing

**inspect:** The sentence is the sentence you gave. If you lied, the lie is legible.

String through a hole. Name on the other side. Effects catalogued with it. Burn it and the House still had a night. Leave it on Hadi's hook and the Stack will read it without being asked.

### `item_returned_effects` — Catalogued

**inspect:** A pocket's worth. The tag tied on. No sermon.

Whatever they had that was not a bunk. A spoon. A dosimeter. A sock. Len does not explain the back room. The sock may match the one on a levy board.

### `item_levy_copy_home` — Home Carbon

**inspect:** Pink, or a copy of pink. Three names. Ice window as posted.

The white went north. The yellow went to a weigh hut. This one stayed. Pin it by the chart and Kess will not take it down. Burn it and Pell may still have a spike.

### `item_nameplate_living` — One Plate, Used

**inspect:** It has a name it was not cast with.

Screwed under a living name. Lamp-catch. The tin is lighter by one. The rectangles in the corridor are still unfaded. Nobody mentions the tin.

### `item_sole_living_copy` — Living Occupancy

**inspect:** Said aloud. Written. Different ink.

Not a Schedule. A paragraph that can be discovered in the vault. 12-C may list these people afterward. Nila's people, if included, are why a wheel will not undog.

### `item_duth_boot_left` — Size 2, Left

**inspect:** The pair is broken. The child knows.

Rubber that was packed for someone with papers. Worn here. If the right went north, the Quad has a child with one good boot and a question.

### `item_hadi_rag` — Clinic Rag

**inspect:** Still damp if he is not. Dry if he is dead. Nobody boils it but him.

Hung on the alcove hook. Present as a person. Absent as a person. Do not write doctor on the wall because of it.

### `item_edor_cup` — Tin Cup, Returned

**inspect:** He washed it in the ash. There is grit in the seam.

If you carried tea out. If you did not, this item does not exist. He will mention the cup in the return, or the silence.

### `item_roster_standing` — Standing Instruction A12-DR

**inspect:** Carbon. Unofficial. Occupancy.

See §8b. Do not fold through a name.

---

# 8d. Night-slate remarks (12) — Tamsin's book

Not Yara's accident book and not the Gate's axle ledger. A night book. Columns: date, who had the wheel, what the grille heard, whether the kettle was still hot at four. The fourth column is only for when the wheel stayed shut, or when it didn't and the rag ticked afterward.

| # | Remarks (write as found) |
|---|---|
| 1 | Stool. No knock. I did not open. Cup went out. Cup came back with grit. |
| 2 | Same name three times. Pad in the airlock. Voice slower. Button not more cracked than yesterday. |
| 3 | Tick. Bucket. Ten in the alcove. Rag boiled after. |
| 4 | Tick. Open. Fifty walked. Mess did not look at the wheel in the morning. |
| 5 | Deny. Twenty on the others. Forty days starts here. I will not count them aloud. |
| 6 | Two knocks. House. Name given. Sentence given. I did not supply either. |
| 7 | Machine in the ash. It still works. He asked if we would ask. Someone asked. |
| 8 | Three north. House thin. I stayed. Wheel taught to [blank]. Blank not filled. |
| 9 | Road dark. Too many bodies. Missed a knock. It was the clerk. He waited. That is his job. |
| 10 | Office jackets. Temperature said as procedure. Wall read. Wall was [pencil/ink/blank/ash]. |
| 11 | 11's people on the apron. I said we don't give names. They left. Disc in the grit. |
| 12 | Child on the grille. Heading gone. I took the button away. They will find another. |

Yara's accident book remains Holdfast's. Do not merge ledgers. Distances are not bunks.

---

# 8e. threateningBodyText pairs (additional)

Trust-reactive. Same scene, different temperature. Never "they hate you."

---

**Pell — conscription (`threateningFactionId: faction_central_garrison` or lore `iron_garrison` — do not pick a side in data; use the systems id the quest already pays)**

Neutral: He explains that service is rations. He waits to be asked about decliners. The machine sits in the ash like a piece of furniture that has not been told the building is gone.

Threatening: The machine is already on a number. The number is one of yours. He still answers if you ask. The answer is shorter. The typeface of the posted order looks the same. The spike has more paper on it.

---

**Len — apron**

Neutral: Two knocks. He names the price. He does not look at the chart through the grille.

Threatening: He has other doors. He says so. The satchel is already full. He will still take a name if you give one. He will not wait through a kettle.

---

**Nila — 11, after you wrote a living name**

Neutral: The authenticator is on. The disc is on the nail. She talks about filters.

Threatening: The light is still on. The wheel is a wheel. She does not come to the grille. A note under the gasket: *Do not come. You wrote a living name in a year-colour.*

---

**Ansel — mess, after a soft sentence**

Neutral: He asks who takes the last bowl. He does not take it.

Threatening: He does the dishes. The ring is still there. He does not ask. The child asked him in the dark. He did not correct you. He also did not sleep. He will not say that twice.

---

**Kess — unslept name**

Neutral: Who slept here. Pencil. Kettle.

Threatening: The paper is thinner where a name was. She wipes the palm-rectangle. She does not look at you. The string is long enough.

---

**Tamsin — asked to lie on the grille**

Neutral: Name, or the wheel stays. Say again.

Threatening: She said you were here. The grille doesn't do fiction. The slate has a different name for the watch. Hers is on a pad in the airlock.

---

**Office escort — 12-C (Holdfast faction id)**

Neutral: Authentication is a procedure. Forty days is the quiet interval.

Threatening: Occupancy remains irregular. The grille is not a second form. The wall is. If the wall is ash, they have brought a list. They will say the temperature either way.

---

# 8f. Circuit overlays — full cards (*existing* ids)

Short overlays in §1.4 remain the inspect line. These are pasteable `description` additions when `exp_duty_roster_unlocked`.

---

### `loc_weighbridge` — full overlay

The scale still sets prices. A folding stool has been here, or will be, or has left marks that are not axle marks. Edor Vale reads occupations. If your wall has graphite, he is less wrong. If your wall is blank, he is still almost right, which he will not treat as a joke. The Tollman charges for introductions. Introductions are mass. A census is not an introduction and is entered as one anyway: `CENSUS — 12 kg equivalent`, or not, depending on whether the Tollman likes clerks. You can steal the poise. The next column will be wrong. You can refuse the form. The stool will be at your hatch, where mass is not how waits are measured.

### `loc_conscription_office` — full overlay

Driving licences. Ticket machine. Opening hours in the same typeface as the order about decliners. Pell is sincere. If your coastal levy named three, he wants those three and will say they are a different form. If you hid them at 11, he does not know 11 exists. He knows quotas. A spare machine has gone out into the ash and come back, or not. You can take a number. The number is a spike. You can break a gear. A worse man has a worse machine.

### `loc_the_allotments` — full overlay

Two hundred plots. A noticeboard in a plastic sleeve. Brass demand stacked with a plant on a coast and a playground with chains. Frayne's minutes will record mass. If a visitor said *fair*, the minutes record that a visitor spoke. The tin behind your filter is not mentioned. The floodplain is still a floodplain. Tablets still have a clock. You can deliver fittings. You can deliver none and watch a date get written that is soon.

### `loc_grange_hall` — full overlay

Hands. A chalkboard. Wet wool. If the names are your levy names, the vote is whether a hole may keep a pool the ice wanted. Your hand is visible. Down is counted as down. Someone will say Garrison-shaped later if you voted return. Delacroix does not come to your hatch. The room does not need to.

### `loc_st_brigids_almshouse` — full overlay

Charts to a date, then not. Bell that does not ring. Blankets on a chair. A corridor that turns. The turn is not a quest stage. Len's satchel. Ethanol if you brought it. Survivors will argue about the turn. The game will not.

### `loc_dentists_row` — full overlay

Four practices. Three stripped. The fourth a square of cleaner floor, bolt-holes, or a chair if you brought it back. Your airlock had this chair, bolted, for a dentist who scored 58.4. Returning it does not write DENTIST on a wall. A chair is not a sleep. Leaving it in the hole keeps the square a measurement. Ostrowski will sell the measurement. He will not carry the chair.

### `loc_alloc_12b` — full overlay

Fourteen chalk marks, a gap, then six. A kit that still works if you left it. Handwriting smaller toward the end. If Sela is with you, the word you use — engineering, salvage — decides whether she stays in the room. Taking the kit makes the water a memory. Copying the notes makes a waystation clever. The hole was a fallback designation on a form. It is still not provisioned. It is still wet.

### `loc_school_gymnasium` — full overlay

Nine pupils, or fewer. Wren trades explanations. If you told a child at your mess what boots were for, or where three packs went, and Wren heard, this room will have that version and no other. If a Cluster school ever sits one of yours, the arithmetic may be a Reconstruction Utility Rating. Here the arithmetic is a spoon and a sentence.

### `loc_veterinary_surgery` — full overlay

Ianov. Paper. Numbers not rounded toward ease. If Hadi is north, the second pair of hands is a whistle on an outfall. If Hadi is a dry rag, the waiting number waits. If Hadi is hidden, Ianov does not ask where. He asks whether the dose is the dose.

---

# 8g. Second Winter — three home nights (sample script)

Not a weather DLC. A script the encounter system can play when `season_second_winter` is active and `quest_roster_window` is the closed-road or thin-window variant. Three nights. Then the slate either matches or a name is missing.

**Night 1 — oil**  
The mess lamp is a cup. Ivy's can is a cup. Yara's stick, if the Cut exists this week, is a cup. There is one cup. Tamsin: "If both the ice and the house are short, someone is lying about one of them. I'm asking which." Choose. Dark mess or dark kilometre or dark Cut. Child does homework, if homework is a copied heading, by touch.

**Night 2 — cough**  
Intake bunk. Filter notch. Lots or order. Hadi present or a dry rag. Len may knock at the end of the night, not the beginning. Do not merge with Pell. Queue.

**Night 3 — visitor who cannot leave**  
Road dark. A person on the apron who would have been a fourteenth in a better week. Nila: one night, two if the ice is wrong. A third is occupancy. Tamsin missed a knock yesterday; she says so. Assign a second watch or accept another miss. The kettle has a queue. The ladle has a ring. The wall is whatever you already wrote.

After night 3: `flag_home_held` if stove, filter, child, and at least the names on the slate came to morning. Else `mutation_house_thinned`. Overlay ending `lore_dr_ending_second_winter` may append. It does not replace occupancy.

---

# 9. Consistency flags


Found while writing. Do not silently retcon. Ticket or ignore.

1. **Edor's ice line** ("There isn't a time limit on understanding it. There is a time limit on the ice.") is Holdfast weighbridge canon. This pack does **not** reuse it. Home-stool Edor uses: occupations wrong by one; the stool is as close as the procedure comes. Do not mint a second ice aphorism.
2. **Sela clinic line** about school/iodine/father's number is Holdfast. `se_sela_row` may play it once if that pack is live and clinic unclaimed. Do not rewrite her into an adult.
3. **Quiet House** back room remains unadjudicated. Len's monologue does not add a second sentence.
4. **The Knock** / Provisioned are not Blank Rows. Filter-at-home is internal. Do not pay Nila with a sermon about fairness.
5. **Kess DOB twice** is municipal clerk-book, not Convoy 12's allocated return. Do not merge with `quest_comp_edor_dob`.
6. **Hatch constants** quoted as already written. Implementer must log unchanged values.
7. **`loc_alloc_12b`** exists; do not mint a duplicate. Chalk fourteen / gap / six stays.
8. **`loc_st_brigids_almshouse`** overlay; id stays. DisplayName may remain St Brigid's.
9. **faction_blank_rows** goes in currents catalog, not `faction_lore.json`.
10. **Pell** is `npc_sergeant_pell` (*existing*). Do not mint a second decent conscriptor.
11. **Tamsin lights a lantern for whoever is walking** — cousin of Ivy's rule, **not** Ivy's rule. Do not let the player ask Tamsin for Ivy's exception.
12. **Holdfast `item_tin_fourteenth`** — reuse if minted; this pack's tin quest feeds it, does not double-loot fourteen plates.
13. **Radio 27.12** — grep `radio.json` before adding `radio_dr_*`. Grille lines can be unbanded.
14. **No seventh Power.** Office remains Holdfast catalog. Blank Rows are a Current.
16. **Night-slate book** is Tamsin's grille log, not the Gate axle ledger and not Yara's accident book. Do not reuse the "remarks column is almost never used" construction.

---

# 10. Word counts

Approximate, this file (`wc -w`: **18,150** shippable prose including ids/headers):

| Bucket | Words (approx.) |
|---|---:|
| 1. Location cards (Stack, Approach, Overflow) | 2,200 |
| 1b/8f. Circuit overlays (existing ids) | 1,050 |
| 2. NPC voice bibles (6) | 3,400 |
| 3. Main quest stage prose (10) | 4,400 |
| 3b. Objective-complete / Holdfast read-diffs | 1,550 |
| 4. Side quests (18), playable | 2,050 |
| 5. Morale micro-choices UI (26) | 1,050 |
| 6. Shelter encounters (14) | 1,250 |
| 7. Intercom / radio | 400 |
| 8. Ending second paragraphs (5) | 580 |
| 8b. Standing Instruction A12-DR | 450 |
| 8c. Item flavour | 1,050 |
| 8d. Night-slate log (12) | 280 |
| 8e. threateningBodyText pairs | 420 |
| 8g. Second Winter three nights | 280 |
| Front matter + consistency flags | 630 |
| **File total** | **~18,150** |

Bible (`expansion_02_the_duty_roster_plan.md`): **~13,270** words (design). Not counted in the creative-pack target.

Target was 18,000–28,000 of quest-weighted shippable text. This pack spends the words on stage text, marks, and home scenes rather than a second coast of location cards. Main-quest blocks are briefing + objectives + choices + fail, still UI-speakable, not padded to a novel. Holdfast's pack was location-weighted (~22,500); this sister pack is occupancy-weighted and sits at the low end of the band on purpose.

**Implementation note:** Paste `description` / `inspect` into location overlays. Paste barks into NPC tables / `threateningBodyText` pairs. Paste `mmc_*` / `se_*` into `duty_roster_marks.json` and `duty_roster_encounters.json`. Do not edit `faction_lore.json`. Re-grep ids before commit. Hatch magnitudes: do not retune.
