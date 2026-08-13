# Encounters

> Target files: `events.json`, `echoes.json`, `survivors.json`
>
> Two kinds. **Character encounters** are people who recur, remember you, and
> want something. **Situational encounters** are things that happen to you.

---

# Part I — Character encounters

Canon already has four faction figureheads: **Colonel Voss** (`iron_garrison`),
**Delacroix** (`ash_militia`), **The Vessel** (`cult_of_ash_sign`), **The
Tollman** (`warlords_sector_4`). They are institutions with faces. The people
below are the opposite — faces without institutions, which is what makes them
usable more than once.

Design rule for all of them: **each wants something the player can actually
give, and none of them will ask twice.**

### `npc_bram_ostrowski` — the mapmaker
*The Toll · unaffiliated · from Day 20*

Walks the corridor selling maps he surveys himself, on waxed paper, priced by
the sheet. Accurate to a degree that makes people uncomfortable about how he
obtained the detail.

**Wants:** corrections. He pays for accurate reports of what the player found
and where, and he pays more when the news is bad.
**Offers:** reduced travel time and pre-warning of hazards on any route he has
sold you.
**Will not:** carry a message, take a passenger, or say who else bought the
same sheet.

> "I don't sell where people are. I sell where *things* are. People move.
> That's their business and it's not on the paper."

### `npc_sergeant_pell` — the decent conscriptor
*The Grid · `iron_garrison` · from Day 15*

Runs intake at `loc_conscription_office`. Polite, thorough, and entirely
sincere when he explains that service is the fastest route to rations. He
believes it, because it is true.

**Wants:** to hit his numbers without lying to anyone.
**Offers:** genuine Garrison protection, real medical access, and a straight
answer every time.
**Breaks on:** being asked what happens to the ones who decline. He answers
honestly. That is the encounter.

Pell is the expansion's argument that the Garrison is not evil, which makes the
Garrison much worse.

### `npc_doctor_ianov` — the veterinarian
*The Verge · `ash_militia` · from Day 10*

Large-animal vet at `loc_veterinary_surgery`, performing human medicine because
he is the closest thing available and everyone including him knows it.

**Wants:** a human formulary. Any pre-war dosage reference for humans, at
almost any price.
**Offers:** surgery, at odds he will state out loud beforehand, accurately.
**Detail:** he does the arithmetic on paper in front of the patient every
single time, and he has never once rounded in the direction that would be
easier.

### `npc_wren` — the child who trades
*The Verge · unaffiliated · from Day 25*

Eleven. Attends the nine-pupil school at `loc_school_gymnasium`. Trades small
found objects — a lighter, a spoon, a key — at prices that are always slightly
too generous to the player.

**Wants:** to know what things were for. Will trade a genuinely valuable item
for a straight explanation of a pre-war object.
**Offers:** unpredictable minor salvage, and the Verge's actual gossip, which
is better intelligence than the Militia's.
**The weight:** Wren was born the year of the Exchange. Every explanation the
player gives is the first and only version they will ever hear, and the game
records which ones the player told and whether they were true.

### `npc_kestrel` — the ascending
*The Spine · `cult_of_ash_sign` · from Day 60*

Walking the switchbacks in stages. Visibly in the second phase of acute
radiation sickness, lucid, unhurried, and entirely at peace.

**Wants:** nothing. Declines medical aid, politely, every time it is offered,
and does not argue.
**Offers:** safe passage through Cult ground; hot-zone routes nobody else
knows; and, late, her boots.
**Design note:** she must never be presented as deluded *or* as correct. She is
dying of a choice she made freely and is not frightened. The player's survivors
react; the narration does not.

### `npc_nomi_fisk` — the Shallows trader
*The Drown · unaffiliated · from Day 90*

Runs a nine-metre launch at `loc_the_shallows_market`. Trades with all four
factions on the same afternoon without difficulty.

**Wants:** cargo that does not require her to pick a side.
**Offers:** the only water transport in the Drown; access to
`loc_cold_store_atlantic` and `loc_records_annex`.
**The etiquette:** nobody boards another hull. She explains this once. If the
player's crew violates it, she does not retaliate — she simply is not at the
Shallows again, and the Drown closes.

### `npc_ivor_lasko` — the deserter
*The Verge · ex-`iron_garrison` · Day 40+, one-shot chain*

Hiding in a Verge outbuilding. The Militia is holding a vote on whether to
return him. Voss's standing order is unambiguous and publicly posted.

**The encounter:** the player gets a vote, as a resident. Not a dialogue
choice — an actual show of hands at `loc_grange_hall`, counted, with the
player's hand visible to everyone in the room.

**Branches:** returned (Garrison trust up, Militia trust down, Lasko shot,
event fires eleven days later and is very short) · sheltered (inverse, plus a
standing Garrison search risk on the Verge) · abstained (both factions note it;
Delacroix says nothing, which is worse).

### `npc_the_cartwright_sisters` — the press
*The Verge · `ash_militia` · from Day 35*

Run `loc_cider_press`. Ada speaks and negotiates. Ruth has not spoken since
Hour Zero and does the work.

**Wants:** apples, fuel for the boiler, and no questions about Ruth.
**Offers:** ethanol — Sector 4's only anaesthetic outside Garrison stores.
**Late-game:** if the player has supplied them steadily for 100 days, Ruth
hands over a written note rather than speaking. It is four words long and it is
not about herself.

### `npc_registrar_margit_sole` — the record
*The Drown · `faction_archivists` · Day 150+*

See `02_THE_LIST.md`. The spine's keystone character.

### `npc_sela_renn` — the claim
*Player shelter · Day 200+*

See `02_THE_LIST.md`.

---

# Part II — Situational encounters

## II-a. Trust-reactive scenes

This is `events.json`'s most underused feature: `threateningBodyText` +
`threateningFactionId` + `threateningTrustBelow`. **The scene does not change.
The temperature does.** The player learns their reputation by reading tone,
never by reading a number.

### `event_checkpoint_papers`
`threateningFactionId: iron_garrison` · `threateningTrustBelow: 30` · minDay 12

**bodyText:**
> The corporal at the barrier checks the manifest against the crate count,
> finds them equal, and waves you through without looking up. Behind him
> somebody is frying something in a mess tin and arguing about it.

**threateningBodyText:**
> The corporal at the barrier checks the manifest against the crate count,
> finds them equal, and does not move. He reads it again. Behind him the
> frying has stopped. He asks you to state your shelter designation, which is
> printed on the manifest, in his hand.

Same checkpoint. Same count. Same result — you pass either way. Nothing
mechanical differs. It simply becomes clear that you are now a thing being
handled.

### `event_grange_welcome`
`threateningFactionId: ash_militia` · `threateningTrustBelow: 35` · minDay 20

**bodyText:**
> Somebody takes your coat. Somebody else is already pouring. Three people ask
> after your survivors by name and one of them gets a name wrong and is
> corrected by the other two.

**threateningBodyText:**
> Somebody takes your coat and hangs it by the door rather than the stove. The
> conversation does not stop when you enter, which you notice, because it did
> not use to continue.

### `event_toll_price`
`threateningFactionId: warlords_sector_4` · `threateningTrustBelow: 25` · minDay 18

**bodyText:**
> The Tollman's man quotes the posted rate, takes it, writes a receipt, and
> gives you the receipt. The transaction is complete and slightly friendly.

**threateningBodyText:**
> The Tollman's man quotes the posted rate. Then he quotes it again, with a
> figure attached that is not on the board, and explains — without threat and
> without apology — that the board is for people whose passage is routine.

### `event_shrine_reading`
`threateningFactionId: cult_of_ash_sign` · `threateningTrustBelow: 30` · minDay 45

**bodyText:**
> The reading is taken at eye height and spoken aloud. Someone offers you
> water. It is the same water they are drinking.

**threateningBodyText:**
> The reading is taken at eye height and spoken aloud. Someone offers you
> water. It is not from the same jug, and the person who hands it to you
> watches you hold it.

Nothing happens. The water is fine. It is always fine.

---

## II-b. Echoes — environmental vignettes

Matching `echoes.json`: a found thing, described exactly, with choices that
cost. No line explains the significance.

### `echo_the_nameplates`
> A tin behind the filtration stack, the kind that held boiled sweets.
> Fourteen brass nameplates inside, each with two screw holes and a
> surname. None of the surnames belong to anyone here. Somebody took them down
> in the first week and could not make themselves throw them away, and neither
> could anyone since.

*Choices:* mount them in the corridor where they were (morale −8, permanent
flag `nameplates_hung`) · leave the tin where it is (no change) · melt them for
brass (+brass, morale −20, and the flag is checked at the arrival in
`02_THE_LIST.md`)

### `echo_unopened_boots`
> A crate in the deep stores, banded, never opened. Children's winter boots,
> sizes 1 through 4, eight pairs. The packing note lists a delivery date three
> days after the Exchange and a recipient reference: ALLOC-12/DEP.

*DEP is short for dependents.* The echo does not say so.

### `echo_the_frying_pan`
> A cast-iron pan on a cold stove in an empty Grid kitchen, with a meal in it,
> burned to carbon and then to dust, and a second place set at the table with
> the cutlery squared.

### `echo_school_register`
> The register at the gymnasium school, current, nine names. Behind it in the
> same drawer, the register from before, four hundred and six names, with a
> pencil line drawn through some of them and not others, and the drawing
> stopped partway down page three.

### `echo_dosimeter_pilgrim`
> A dosimeter on the switchbacks, hung on a marker post at eye height,
> reading. Beside it a pair of boots, placed together, and a folded coat, and
> nothing else at all.

### `echo_the_receipt`
> A Warlord receipt in a dead man's inside pocket, waxed paper, entirely
> legible. Passage for one, paid in full, dated yesterday. The bridge is four
> kilometres behind him and he was walking away from it.

### `echo_answering_service`
> A pre-war medical answering service, solar, still cycling. Forty-one
> messages. Most are appointment cancellations. Number 39 is a man
> apologising, at length and very calmly, for missing a scan, and promising to
> reschedule as soon as things settle down.

### `echo_the_second_chalk_count`
> Chalk on the wall at the stair head: fourteen marks, ruled through in groups
> of five. Then a gap of about a metre. Then six marks, in the same hand, much
> later, much less steady.

Found at `loc_alloc_12b`. Requires no explanation and receives none.

---

## II-c. Hazards and pressure

Short, mechanical, no moral content. These exist so the moral ones land.

| id | Where | Shape |
|---|---|---|
| `event_gallery_settle` | `loc_avalanche_gallery` | The shed groans. Continue, or lose the day and go around. |
| `event_paint_stick_gap` | `loc_ordnance_shoulder` | The marking stops. Beyond it is unsurveyed and looks identical. |
| `event_hull_knock` | `loc_the_shallows_market` | Something under the boat. Almost certainly debris. |
| `event_ice_creak` | `loc_cold_store_atlantic` | You are standing on the roof of the thing you came to open. |
| `event_relay_current` | `loc_radio_relay_mast` | The hut is drawing power. The door is not locked. |
| `event_standby_cycle` | player shelter | The outer hatch reports standby, briefly, for the first time in five years. |
| `event_pump_prime` | `loc_pump_station_nine` | One pump turns over. Then stops. It can be done. |
| `event_low_background_null` | `loc_low_background_lab` | The counter reads clean. It has never read clean. Check the sample or check the instrument. |

`event_standby_cycle` is the spine's alarm clock: it fires once, around Day
190, means nothing mechanically, and is the shelter's outer hatch doing exactly
what it did on the afternoon everyone walked in.

---

## Conversion checklist

| Content | Target | Count |
|---|---|---|
| Region gazetteer | *(reference only)* | 5 |
| New locations | `locations.json` | 40 |
| World history beats | `world_history.json` | 16 |
| Character encounters | `survivors.json` + `events.json` | 10 |
| Trust-reactive events | `events.json` | 4 full + pattern |
| Echoes | `echoes.json` | 8 |
| Hazard events | `events.json` | 8 |

**Not yet done and required before conversion:**
- `faction_archivists` needs a `faction_lore.json` entry, or an explicit
  decision to keep the Archivists out of the faction system entirely (they have
  no territory, no tribute, and no `relationships` — the schema fits badly, and
  that is probably the correct signal)
- Sela Renn's arrival needs a flag-gated trigger; no existing system fires a
  one-shot narrative event on a day threshold *and* a knowledge gate
- `echo_the_nameplates` writes a flag read much later by the arrival — confirm
  `RequiredFlagId` supports the reverse lookup
