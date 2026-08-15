# ASHFALL: THE VERDICT — MASTER CREATIVE PACK

**Internal id:** `expansion_08_the_verdict`
**Kind:** Shippable prose corpus + narrative resolution. Companion to `docs/expansions/expansion_08_the_verdict_plan.md`.
**Voice lock:** Cold, exhausted, human, restrained. Specificity over adjectives. No line tells the player how to feel. No magic. No chosen one. No evil machines. Dry, situational, character-earned humor only.
**VO:** Lines marked `[VO]` are text-first; record only if the radio/tannoy pipeline exists. Everything else is UI/Codex/inspect.

---

## PART I — THE FOURTEEN DOOR ENCOUNTERS

Eight batch beats below; six more in Section 9.3 of the plan's batch cards. Schema: existing `door_encounters.json` (encounterId, visitorName, visitorFaction, description, minDay, maxDay, threatLevel, choices[] with choiceId/text/requiredTrait/requiredItemId/requiredItemQuantity/baseMoraleDelta/baseGuiltDelta/targetFaction/factionStandingDelta/outcomeDescription). Hours 14–24. No "mysterious stranger" more than once a season.

### ENC-VD-001 — The Tape Seller
**Time window:** midnight, snow ticking against the hatch like fine sand.
**Atmospheric setup:** Three in the morning, a knock with a rhythm that is practiced and unhurried: three, three, one. Through the peephole, a woman in a boiled-wool coat with a tape reel on a leather strap. Her breath is steady. She has obviously been here before, to other doors.
**Visitor:** Salt-and-pepper hair, a wedding ring worn through to a wafer. She carries one tape reel — festooned, leader sticking out — and nothing else. No bag. No food. The coat's cuffs are mended in a different thread on both arms, like the mends were done for size, not wear.
**Stated want:** "I've a reel. It's a census window, recorded off the array. I'm not selling it for food. I'm asking if your shelter keeps a radio, and if you've heard the count."
**True want:** To know whether anybody else on the ridge knows the machines are still counting. The reel is real; it is also bait for conversation, and she will not push.
**Concealed:** She is from the low-background lab. The Cold Count's fourth researcher (unnamed in the ledger) visits doors in winter to check what people believe.
**Choices:**
- **Admit and trade** — shelter gives a meal; she gives the reel and stays to drink the tea slowly. `choice_verdict_tape_tea`: morale +4, `evidence` enrollment on `evidence_fuse_linen` if not already read, flag `verdict_gossip_tape_seller`. Her parting words: "The count is open. That's all it is. Open."
- **Take the reel, don't open the door** — passed through the gunport. `choice_verdict_tape_chute`: guilt +2 (small; the game does not dramatize a chute). The reel joins the inventory unlistened.
- **Decline and close** — `choice_verdict_tape_refuse`: morale −2 (a survivor mutters "that's the second person asking about the radio"). No enrollment. The seller's footsteps are gone by the time the peephole clears.
**Chain:** The reel is one of three items that can be traded to the Archivists (§8.2, `evidence_eden_log` adjacency). If declined, a `verdict_gossip_tape_seller` flag records that the ridge now talks about the count in one more room.
**Moral weight:** 3 — it costs nothing to listen, and the listening collects.

### ENC-VD-002 — The Relay Repairer
**Time window:** First watch, wind from the north, the sort that makes metal sing.
**Atmospheric setup:** A knock at dusk thin and flat, with a pause, then one more, like somebody remembering a knock and doing it from memory. Through the peephole: a figure in a Garrison-issue parka that has been cut down and re-hemmed, tool pouch at the belt, one hand bare to the cold holding an antenna segment.
**Visitor:** Mid-thirties, face weathered the way street surfaces are weathered. No standard-issue boots — his are civilian, resoled with tire rubber. He straightens when the peephole opens, and he *does not raise his hands* — a man who works where people are watched.
**Stated want:** "Spare wire. Two metres, insulated preferred. I'd trade the antenna for it."
**True want:** To fix the relay-mast hut's 03:40–04:10 window without anyone knowing a repairer visits the mast at all. The antenna is genuinely usable.
**Concealed:** He is a deserter from the Garrison signal corps (he would say "left signal"), keeping the mast's *maintenance leger* true because the leger is the only honest record he has left. The name on the pouch is not his.
**Choices:**
- **Trade the wire** — antenna swap, `choice_verdict_relay_wire`: +2 salvage, and the mast's maintenance window ticks true (which `MachineLogSystem` notices two days later as a `maintenance` entry with no logged handshake). Flag `verdict_relay_serviced`.
- **Trade and take his name** — he gives "Calloway," which is not his name, and both of them know it isn't, and neither remarks on it. `choice_verdict_relay_name`: the same +2 salvage, plus a `verdict_nod_to_the_bolted_on` memory that the next door encounter mentioning the relay checks.
- **Refuse** — `choice_verdict_relay_refuse`: no trade; the mast's window next week reads *unmet*, and the relay's voice drops off the radio for three days (a sound thing, not a quest thing). The peephole confirms he walks north, in the dark, without a lamp — his choice, not the player's.
**Chain:** The serviced window is read by `quest_verdict_the_warm_range` Stage 2's cable mapping. Refusal's quiet consequence is the three-day radio silence, which a Radio-savvy survivor may eventually ascribe to the weather. The game never connects them for the player.

### ENC-VD-003 — The Census Clerk
**Time window:** Late evening, the count's carrier tone faint under the roof.
**Atmospheric setup:** A knock, a pause long enough to read a page, then a knock. Through the peephole, a woman with a cloth satchel pressed to her chest like a child, wearing a civilian winter coat that is too large and an expression that is trying to be official.
**Visitor:** Selya Saltmarsh — the Verge census clerk (paper side, not machine). She lives to keep lists. The satchel is her whole archive: partial ledgers, a pencil stub, a sharpener she guards.
**Stated want:** "Your shelter's name. And the number of persons presently within. For the county ledger." She says "county" like the county might still exist.
**True want:** For the count to exist on paper too, in case the machines are wrong. She does not know the machines are right — nobody has told her, and the pack does not tell her.
**Concealed:** Her ledger's last entry is dated Year One. She has been carrying the notion of a census for four winters.
**Choices:**
- **Give the names** — `choice_verdict_clerk_names`: she writes them in her ledger with the pencil stub, thanks the shelter, and leaves; morale +2; the machine's `census_draft` evidence enrolling later cites the matching numbers, and the game shows the two ledgers agreeing on the same run (a small, uncanny recursion).
- **Give a wrong count** — `choice_verdict_clerk_wrong`: she writes it down without comment, and the lie is now in *two* ledgers — hers and, later, the machine's running census. Nothing punishes this. The recursion is the message.
- **Refuse** — `choice_verdict_clerk_refuse`: she accepts the refusal the way an official would — "Then it shan't be counted." The door closes. The carrier still runs.
**Chain:** Her ledger travels to `quest_verdict_eden_grabs`'s archive routing and to the Archivists' corroboration rule (a second witness to a census). The wrong-count branch is never flagged — the pack's rule for lies in records is that the record remembers, and the game does not remind you.

### ENC-VD-004 — The Sound Engineer
**Time window:** First watch, a lull in the wind, the kind of silence that makes people speak quietly.
**Atmospheric setup:** No knock at first — a *scratch* at the hatch's base, like something dragged. Then a knock, patient, from a person's height. Woman, mid-forties, carrying a folded steel tape measure with a carpenter's pencil tucked behind an ear.
**Visitor:** Decline-pattern engineer — the fuse world's maintenance lineage. She measures things that do not need measuring and logs what she measures on her own self. The tape measure is the only tool she has; the pencil is the only pen.
**Stated want:** "I need to check the hatch's sediment seal. It's on the register." The register is in her head.
**True want:** To be let in long enough to measure the shelter's airlock depth, because the machine's meter for this shelter's airlock reads one number, and her measure reads another, and one of them is wrong.
**Concealed:** She is the very thing the pack's machinery describes: a volunteer performing a scheduled reading. It is not her job. There is no job. She is doing it because she has the tool and the pencil.
**Choices:**
- **Let her measure** — `choice_verdict_engineer_admit`: she measures the seal, says "tolerable," and leaves; +1 locale; the airlock's machine entry next week reads `calibrated` (a readout change no UI explains). Flag `verdict_seal_calibrated`.
- **Ask her what she works for** — `choice_verdict_engineer_ask`: the question visibly unsettles her — not with fear, with *surprise* that anyone asked. "The register," she says at last, and the shelter records the phrase as a datum that means nothing and everything.
- **Refuse and watch** — `choice_verdict_engineer_refuse`: she leaves a chalk mark at the hatch's base anyway, a small register glyph, and the mark is the loudest thing in the game that season.
**Chain:** A calibrated airlock reads differently at the Reckoning Call — the shelter's own persons appear in the machine's census *in the right order* (admitted = measured = counted). The chalk mark is a diegetic `verdict_seal_measured` flag with no other effect.

### ENC-VD-005 — The Salt Gatherer
**Time window:** Dawn, the hour machines traditionally came on shift.
**Atmospheric setup:** A knock with frost in its rhythm — stiff, deliberate. Through the peephole, an old man with a grain-sack over one shoulder, an iron spike in its mouth, salt-bright. His eyes are wet not from grief but from wind, and he blinks carefully, the way sailors do.
**Visitor:** A Drown salt-gatherer, one of the free-held brine flats. He collects the salt that the Vent Shaft's discharge lays down on the north flats — salt was never his trade before, but the brine is clean where the sluice runs, and the flats are free.
**Stated want:** "Trade? Salt for a loaf. Or the same for a word."
**True want:** The word — whether the machines still run the vent. The salt is real; his is the one civilian livelihood in Sector 4 that depends on the Tempest's exhaust, and he has never once worried about what that means.
**Concealed:** Nothing. He is exactly what he appears to be: a man with a sack of salt who needs to know if the machinery will keep being sick into the flats.
**Choices:**
- **Trade salt for bread** — `choice_verdict_salt_bread`: +3 food, morale +2; he eats the bread with the door half-shut, talking with his mouth full in the old way, and the mention that the vent "still runs warm" is how the player learns the machine is *alive-ish* through a third party.
- **Trade salt for a word** — `choice_verdict_salt_word`: he asks one question — "Does it still run warm?" — and the answer the player gives him is recorded by the game as a lie, a truth, or a deflection, and the flats' readouts (via `MachineLogSystem`) read *as told* either way.
- **Refuse** — `choice_verdict_salt_refuse`: he nods, hoists the sack, and walks the dark flats path back; the Salt flats' salt continues anyway, because the machine doesn't consult the trade.
**Chain:** The vent's warmth is micro-thread 2's subject (§5.5) — the one outbound call from the comm array's bleed was to this man's daughter, and the pack never connects the two for the player, and it is the silence between those two beats that is the whole micro-thread.

### ENC-VD-006 — The Soil Sampler
**Time window:** Afternoon, thaw-light, the sort of grey that precedes a storm that never comes.
**Atmospheric setup:** A knock at the bunker hatch from a person seen approaching over the last rise, observed the whole way: deliberate, no flanking, boots foreign to the region's mud. She stops a polite two meters from the hatch and waits to be spoken to.
**Visitor:** A Verge soil-sampler in Militia-adjacent clothing, carrying a canvas of glass vials clinking at her belt. She takes eleven standardized cores a season for the Militia's agronomy table.
**Stated want:** "I need a measurement of your shelter's runoff. The Verge's table isn't complete."
**True want:** The runoff number *specifically*, because the Militia's table has been quietly showing the north-field contamination rising at a rate that matches nothing they fertilized, and she is here with a vial and a question she does not want to ask out loud.
**Concealed:** The vials are not for the Militia. They are for the Cold Count — she trades eleven cores for an accurate histogram of the Verge's water table, which the Count's lab reads as a series of baselines and the Militia would read as heresy.
**Choices:**
- **Give the runoff sample** — `choice_verdict_soil_give`: she labels it with the shelter's position and leaves; the Cold Count's baseline for the Verge gains a second reference point (a `provenance`-shaped enrichment, not a quest). Flag `verdict_soil_sampled`.
- **Trade for a story** — `choice_verdict_soil_story`: she tells the shelter about the eleven cores — why eleven, how long the table has run, that the table is the only map the Militia keeps of the land it defends — and the player can give or withhold the sample after. The trade is the sample; the story is hers to give freely.
- **Refuse** — `choice_verdict_soil_refuse`: she says "then it's an incomplete table," and leaves; the Militia's agronomy table reads `incomplete` on the next season's harvest roll — a +2% variance no UI ever labels.
**Chain:** The second reference point is consumed by micro-thread 3 (§5.5) if the player later carries the Vessel's Cell readings to the Cold Count — the Verge baseline and the anomaly reading together produce a genuine, quiet anomaly the Count logs without comment.

### ENC-VD-007 — The Tape Exchanger (Cult, 1×/season cap)
**Time window:** Night, but dry night, clear cold, stars out — the sort of night the Cult counts.
**Atmospheric setup:** Three slow knocks. Through the peephole, a figure in the Cult's soot-dyed habit standing exactly where the light from the hatch makes them visible and no farther. In their hands, not a weapon: a tape reel, held face-out, the way one offers a relic.
**Visitor:** The Cult of the Ash Sign's sound-keeper. Their order's liturgy is read from dosimeters; this one has begun to keep a *different* reading — tapes off the array — and their beliefs about the meters are evolving without any doctrine to anchor it.
**Stated want:** "A trade. This reel — the array's reading from the month of the Exchange — for your shelter's oldest radiograph or a copy of your rad-reading log."
**True want:** To possess a *human* reading of the same period, to compare the machine's memory with the memory of the burned. They are assembling a second scripture.
**Concealed:** The reel genuinely is the array's reading from Exchange-month. The trade is sincere. What is concealed is not the relic but the question: whether the heat that burned the world is the same warmth that kept the meters running through it.
**Choices:**
- **Trade the radiograph** — `choice_verdict_cult_radiograph`: morale +2, the reel joins the archive; the Cult's sound-keeper leaves with a copy of the shelter's rad-reading log, and the recorder notes, unprompted, that the log's early pages contain no mention of the machines at all — which the shelter's survivors find either reassuring or eerie, and the text lets them.
- **Copy the log, decline the reel** — `choice_verdict_cult_log`: the keeper receives the copy but the reel stays; the keeper is visibly, quietly *satisfied* — they did not come expecting the relic to be taken. It was the trade that mattered. No flag; the satisfaction is the moment.
- **Refuse** — `choice_verdict_cult_refuse`: the keeper holds the reel a moment longer, then nods — the way liturgy is nodded — and fades back into the clear dark. The net season, the shrine dosimeter's daily reading (canon site) reads *normal*, and the pack does not connect it.
**Chain:** The reel is `evidence_eden_log`-adjacent material (§8.2); traded, it becomes a second witness for the Archivists' corroboration rule when the player later asks about Eden Vale's broadcasts. The Cult's own evolving scripture is micro-thread 3's third leg — the Vessel's Cell anomaly, the Verge baseline, and this reel, three readings of the same heat, none of them adjudicated.

### ENC-VD-008 — The Clock Parasite
**Time window:** The worst hour — between 03:00 and 04:00, the hour old shift-rules called the graveyard for a reason.
**Atmospheric setup:** A knock with no rhythm at all: two quick, a scratch, a tap. Through the peephole, a woman with windburn and a wire bundle strapped to her back like a child's empty schoolbag. She is breathing hard, and her boots are wet, and there is no snow on them.
**Visitor:** A scavenger who has been *following the schedule* — she has spent three nights reading the relay mast's maintenance window from the treeline, waiting to see who serviced it, and tonight she walked the mast's cable run east, the long way, because the short way crosses the Twelve-Gauge Array, and the array's plates told her the ordnance is gone but the pattern is not.
**Stated want:** "Shelter? Just the hour. I'll be gone before the window closes."
**True want:** To say, in a human room, within the hour, that she has found the cable run and the fuse world's door and the swept path, and did not go in, and the telling is urgent in a way she cannot name.
**Concealed:** The wire bundle is the mast's antenna segment — she took it from the relay's hut during a maintenance window *that the previous repairer serviced*, and the game does not let the player know her theft broke the ritual until the next morning, when the relay's voice drops off the radio at 03:40 and comes back at 04:10, exactly as scheduled, no one servicing it.
**Choices:**
- **Shelter her** — `choice_verdict_clock_shelter`: she eats, tells the cable-run story in fragments and stops at the swept path, and leaves before the hour is up. The relay's silence the next morning stays unexplained unless the player connects it. Morale +1; flag `verdict_clock_knows`.
- **Ask her what she took** — `choice_verdict_clock_ask`: she looks at the bundle like she had forgotten it was there. "The window's short," she says. The game records the theft with no judgment and no flag — the antenna is ten grams of story that will be forgotten in a winter.
- **Turn her out** — `choice_verdict_clock_turn`: the door is not opened; she is gone by morning into the treeline, and the mast's window the next night is serviced by nobody, and the fix — or lack of fix — is never narrated.
**Chain:** If sheltered, `verdict_clock_knows` grants the player a rumor-in-advance of the fuse world (§5.1 Stage 2's dialogue line: *"There's a swept path in a basement that ends at a tape door. Nobody swept it. It sweeps itself."*). The relay theft quietly reverses ENC-VD-002's quiet repair, and the pack offers no resolution — two volunteers, one coherence, zero verdict.

---

## PART II — THE RECKONING CALL TAPES

`[VO]` — broadcast corpus for the resolution. First two are the machine's own (tape; Maro Veen; NWS register). Third is Eden Vale's (rock-bleed; her own voice; slate-calm).

**CALL-01 — The Count Is Open (30 seconds):**
> "This is the Office of Censuses. The count is open. All persons having custody of persons must present them. The count is open. Off-count is a penalty assessed against the holder. This message will repeat."

**CALL-02 — The Count Is Taken (data burst):**
> Numbers. Three columns: District, Ward, Persons. The machine's own register, presented to nobody, on the array's clock. The burst repeats three times, then stops. No voice. The voice was never the point.

**CALL-03 — Eden Was Here (11 months of tube-bleed, one day's worth):**
> "Still here. Static's thinning. That's not good news, that's a storm on the way. If anyone's reading, the array's drawing again. I don't know what it's drawing for. I don't think it draws for us." *(A long pause, the sound of a mug set down.)* "Hello, if anyone's reading. This is Eden Vale, Kilometre 19, Sector 4, and I am very tired, and the array is drawing, and I am going to keep sending until somebody tells me what the drawing is for."

---

## PART III — THE WORD-LADDER (WORLD-HISTORY CORPUS)

`world_history.json` shape. Six layers, each a physical findable.

**L1 — The First Geophone Pit** (`loc_geophone_pit_1`):
> The array reads the ground the way the papers said it never did: even, patient, and uninterested in everything above it but the count. Tempest Site 01. The plate is riveted, not welded — a maintenance choice, five years old, by a hand that knew the difference.

**L2 — The Linen Codes** (`loc_network_fuse_bunker`):
> The shift charters frame the readout cabinets the way offices frame licenses: task, rate, hand, shift, linen code. Twenty-nine charters. The machine kept the codes after the staff stopped keeping the shifts. The codes are the only language both the machine and the humans ever agreed on.

**L3 — The Standard** (`loc_network_fuse_bunker`):
> The charter's full text (Section 2.4 of the plan). The sentence that does the work: *"nothing in this Standard shall be construed to require the presentation to be read."* A civil-service semicolon killed more sentences than any warhead ever did.

**L4 — Hold Pending Count** (`location_the_dead_hand_core`):
> The UXO field register reads *held*, not *live*. The fields were never awake. They were held — held Pending Count, precisely as the Pause Doctrine requires, since a war the count never ended. The Dead Hand Core did not invent the hold. The Standard provided for it, the way the Standard provides for everything.

**L5 — The Reckoning Call** (`loc_comm_array`):
> The census carrier is a pure data tone on a derelict band. It is the machine's last human-coded artifact: the words are thirty years old, the voice is dead, and the schedule is still walking.

**L6 — The Count** (`loc_archive_tape_silo`):
> The count is presented. It names the shelter's persons, by name, in the machine's register: fourteen, then fifteen, then the hand that wrote the line. A machine does not reason. It counts. This is the count.

---

## PART IV — ITEM FLAVOR CORPUS (15)

Schema per Section 8.1/9.5. Category, tier, lore_flavor, mechanical_effects, downstream_quest_trigger, faction_affinity, rarity, emotional_weight.

1. **`evidence_geophone_hymn`** — *The Verge's farming signature, unlabeled.* — "Under the Allotments, the array keeps time like a metronome that farms: a tap at ploughing, a tap at harvest, a tap at the well-house door. The machine reads the ground and hears a farm. It has never been to the farm." — *Rare · Tragic.*

2. **`evidence_twelve_gauge_steel`** — *The fired-plate ordnance log.* — "Twelve stations, twelve plates, twelve sets of hands in the log. The last entry is Year One. The plates were kept legible after that by a hand that did not log — a hand with a pencil stub and an opinion about the count." — *Uncommon · Mundane.*

3. **`evidence_fuse_linen`** — *The Standard's linen.* — "The linen is coded to the same alphabet as the cabinets' charters. It is the nearest thing the machine has to a constitution, and it is written in the tense of a department that fully expected to be read." — *Rare · Ironic.*

4. **`evidence_census_draft`** — *A paper clerk's partial ledger.* — "Four-column ledger, soft pencil that has been sharpened with a knife to the last useful inch. The shelter names are in the second column. The count column is blank, and has been since Year One, which is when Selya Saltmarsh ran out of households to sight." — *Uncommon · Tragic.*

5. **`evidence_mailroom_tape`** — *A carbon-copy censusing rota from Year One.* — "The rota is in triplicate, as the Annex requires. It lists a mailroom, a census clerk, and a sorting route with a footnote in the clerk's hand: 'Persons shall not be counted twice. Persons shall not be counted once.' The footnotes do not say which clause won." — *Rare · Ironic.*

6. **`evidence_uxo_register`** — *The hold register, read.* — "The register's last entry is five years old. It is unsigned — the Annex does not require a signature; it requires a register — and it reads, in full: 'Held. Pending count. Re-audit at census interval.' The interval came due, and the register does not know it has been due." — *Unique · Mundane.*

7. **`evidence_call_calibration`** — *The calibration burst.* — "A pure tone, three seconds, looped. It calibrates nothing; it is the alignment tone the array plays before its own voice, and its only trick is that it plays on every sub-band at once, which is how the sector learns — over the course of a single night — that a department is speaking again." — *Uncommon · Disturbing.*

8. **`evidence_call_plain`** — *The plain burst.* — "The word 'census' in the machine's register, in plaintext, once, at 03:40, on the hour the old maintenance window opened. The sector's radios — the ones still listening — got a department memo, five years late, and did not know what to do with it." — *Rare · Ironic.*

9. **`evidence_reels_matter`** — *The archive's own accounting.* — "The archive keeps a written count of itself: 2,016 reels, of which 1,831 matter. The 'mattering' category is the machine's own — a count that cannot be falsified and has never once been audited by a human, because nobody has ever asked it what it counted." — *Rare · Disturbing.*

10. **`evidence_valve_s36`** — *The valve read per §36.* — "The valve outside the vent shaft reads 'per §36' — a clause the maintenance file cites to a supervisor's log that contains exactly one entry: Shift 36, six names, one missing. The missing hand is the one that has been turning the valve since." — *Unique · Tragic.*

11. **`evidence_eden_log`** — *Eleven months of tube-bleed.* — "Eleven months of Eden Vale's broadcasts, logged by the array the way it logs everything that comes down the wire — including, it is clear, the silence after her last transmission, which the machine's register records as 'no traffic,' which is itself a reading, and the pack does not adjudicate it." — *Unique · Hopeful.*

12. **`item_archive_tape_silo_key`** — *The tape-silo key.* — "A key the size of a hand, brass, with the tape-silo's number cast into the bow. It is worn at only one edge — the edge that fits the lectern's slot — and it is the only key in the fuse world that has a lock still turning freely." — *Quest/story · Mundane.*

13. **`item_fuse_world_shift_charter`** — *Shift 36's completion.* — "The Year-One sign-in ledger, open to Shift 36: six names, five hands, the sixth left blank, and the completion annotation in the margin — a single check mark, in a hand that has never logged anything else in the county's files. The pack records the completion as fact." — *Quest/story · Hopeful.*

14. **`evidence_veen_your_people`** — *The count, presented.* — "The count names the shelter's persons, one by one, in the machine's register — fourteen, then fifteen, then the hand that wrote them down — and the game does not read it aloud to the player, because the player is the hand, and the count is their own." — *Rare · Tragic.*

15. **`item_verdict_salt_flat_sample`** — *The salt-gatherer's first trade.* — "White, coarse, honest salt from the north flats, traded for bread. It is the first civilian good the machine's exhaust has ever paid for, and neither the salt nor the machine knows it." — *Consumable · Hopeful.*

---

## PART V — GRAFFITI & WALL TEXT (11)

House-voice; scored into the fuse world, the array plates, and the relay hut. Progression across visits encouraged.

1. A tally, chalk: *1111111111111*, and beneath, in a different hand: *the count is these too.*
2. *THE OFFICE OF CENSUSES IS OPEN* — painted, neat, then crossed out, then painted over in a matching hand.
3. A petition in pencil: *SIGN IF YOU WERE COUNTED —* the sheet below is blank, and the signature of the petition's author is the only mark on it.
4. *DO NOT ENTER* — stencilled; beneath, in fresh chalk: *the door is not locked.*
5. A child's drawing: a house, a door, a stick figure, and a second stick figure *inside* the house, labeled with an X.
6. *the clock reads 03:40* — scrawled at eye height, with the time carefully circled, and twice underlined, and never explained.
7. A page torn from a maintenance file, pinned: *3.14 Claimed by no one. Expense: none.*
8. *We counted you first* — in the Cult's soot-dyed hand, high enough that only a tall person or a ladder-keeper wrote it.
9. A route map of the cable run, drawn from memory, labeled *the long way*, with a single X at the fuse door.
10. *The meter is the meter.* — four words, chalk, at the array's base plate, in the hand that keeps the plates legible (Selya's).
11. One word, carved: *PENDING*. No more. The carving is old, the word is not.

---

## PART VI — BARKS (40)

Address-match against the Dose's bark shape: `(context, target_band, mood)` → line. Mapped to the census's four states.

**The carrier is on (out-of-shelter):**
- *"That tone again. Somebody's still broadcasting a heartbeat nobody ordered."*
- *"It's not speech. It's a schedule. I've lived around schedules."*
- *"Radio's got a new tenant. Tin voice, iron patience."*
- *"Turn it off? It's a dead band. It turns itself on."*

**The carrier is on (in-shelter):**
- *"Can anyone else hear that? No? Then it's for you."*
- *"I counted us at dinner. Fourteen. The tone counts us twice."*
- *"It's like a clock in a room no one uses. Here all day."*
- *"The last time I heard a sound like that it meant a list."*

**A reading is enrolled:**
- *"It wrote it down. I read it. Now it's in the book twice."*
- *"Somebody's ledger, not mine. But it had my name's shape."*
- *"A meter and a pencil agree on our numbers. That's new."*

**The count is open (post-Call):**
- *"It's not asking anything. That's the part I don't like."*
- *"The machines kept the calendar. The calendar came due."*
- *"Nobody here is on the count — we *are* the people who present persons. I wrote our names, and the machine wrote the count, and both are true."*
- *"A census needs a hand. It has mine, whether I like it or not."*

**Cold quiet post-Call:**
- *"It's quiet now. The tone's one level softer. I think it's finished."*
- *"The machine says fourteen. The mirror says fourteen. The mirror believes it."*
- *"We are the number. That's all anybody ever was, on paper."*

**The Cult's single line (in-shelter, faithful survivor):**
- *"The fire burned away the world's lies. It did not burn away the meters. Those we were told were dead. They are not dead."*

---

## PART VII — ENDINGS CORPUS (3 vignettes, 100–150 words)

### The Sector Recounts (`ending_verdict_the_sector_recounts`)
The count is read aloud at the Grain Exchange weighbridge by a trader who does not look up from the scale. It takes three minutes. The sector spends the next twelve weeks arguing about what the number means — the Garrison audits it, the Militia quotes it, the Cult calls it scripture, the Toll prices it — and agrees on none of it. The machine's register closes the way registers close: a date, a check mark, a ruling hand. The last line is the count, presented, in the machine's hand, followed by the two words the sector will argue about forever: *accepted as read.*

### The Count Is Held (`ending_verdict_the_count_is_held`)
Nobody presents the count. The machine does not notice, because noticing is not in the Standard. The carrier tone continues on the dead band — one second on, one second off — and the players' radio screen reads, forever after, *CENSUS WINDOW: OPEN*, a deskbound, patient, entirely correct chronicler of the fact that the sector declined to be read. Nothing happens. The nothing is the ending. The last image is the tone: one second on, one second off, on a band nobody is listening to, keeping a count nobody asked it to keep, keeping it anyway.

### The Offer Is a Lease (`ending_verdict_the_offer_is_a_lease`)
The count converts into a lease: quarterly maintenance, a reading per season, a census every 1,827 days, enforceable by the machine's own registers. The sector discovers it has a landlord. The landlord does not care about the sector — it cares about the lease, and the lease is ironclad, because it was written by a department that never met anybody it contracted with. On paper, everything is in order. On paper, everything has always been in order. The last image: a quarterly invoice, printed on a machine that stopped printing anything else in Year One, delivered to a door that opens, because the door is counted.

---

## PART VIII — JOURNAL & VOICE-COLORED READINGS

`JournalSystem`/`JournalVoice` integration — same carrier, three survivors:

- **Paranoid:** *"The carrier is a count of us. I have been counting us since the first morning, and I am the only one who keeps the same number as the machine. The others think I am idle. I am not idle."*
- **Denialist:** *"Somebody's got a generator on the ridge and a tin ear. Three days of it, and the first night I asked it to stop, and it didn't, which proves it's a machine, which proves it's not watching."*
- **Fatalist:** *"The tone counts. We are counted either way. The only question is which ledger they find us in, and that has been decided before us."*
- **Empath:** *(This survivor does not write the entry. The page is blank, and the blank is the report.)*

---

## PART IX — THE CULT'S WITNESS (the full limestone exchange)

At `loc_ash_sign_shrine`, after the fuse world is read:
> **Keeper:** "The fire burned away the world's lies. It did not burn away the meters."
> **Pilgrim (young):** "The meters survived the fire?"
> **Keeper:** "The meters *kept* the fire. There is a doctrine growing in the archives about this. It is not mine, and it is not yours yet. It is a reading, and readings are taken."
> *(The exchange is one of three the codex can cite; the other two were rejected during writing — the first made the keeper too certain, the second made them too fearful. The kept version is the one that ends on the word "reading.")*

---

## APPENDIX — DOWNSTREAM TASK CARDS (the 10 batch cards, map-verified)

1. Evidence corpus — 12 items, §8.1 skeleton + the fingerprint (§11.1 item sample).
2. Door encounters — 8 beats, existing schema, hours 14–24.
3. Radio corpus — 12 signals, `faction_war_radio.json` shape.
4. World-history ladder — 6 beats, `world_history.json` shape.
5. Interviewee vignettes — 6 human reactions to the count (Verge farmer, Toll clerk, Cult novice, Drown rower, Machine-kid, the salt gatherer), house register.
6. Graffiti & wall text — 11 beats (Part V), 7 more from the batch.
7. Barks — 40, mapped to the Dose bark shape (Part VI).
8. Ending vignettes — 3 core (Part VII) + 2 alternates.
9. Journal readings — 3 per RiskBiasTrait core (Part VIII) + 4 more.
10. The Cult's witness — the kept exchange (Part IX) + the 2 rejected drafts with rejection notes, per temperature discipline.
