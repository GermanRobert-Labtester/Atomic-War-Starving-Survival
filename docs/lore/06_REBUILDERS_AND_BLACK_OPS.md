# The Two Unwritten Factions

> `faction_rebuilders` and `faction_black_ops` — the two ids that exist in
> shipped code and have no entry in `faction_lore.json`, so the player can be
> killed by one and starved by the other without the game ever naming either.
>
> Target file: `faction_lore.json` (+ one small DTO change, see the end)

## Why these two are written differently

Everything in `03_LOCATIONS.md`, `04_ENCOUNTERS.md` and `05_FACTIONS.md` is
proposal — invented first, converted to data later. These two are the reverse.
They already have mechanical commitments in code that ships today. The lore has
to be written **to** that code, and where the code says something inconvenient,
the code wins.

So before any prose, here is every fact I am not allowed to contradict.

### The Rebuilders — what the code already says

| Fact | Source |
|---|---|
| Hegemony-tracked, initialised at 0 | `WorldStateConsequenceSystem.cs:78` |
| Siding with the Cult costs them −60 | `WorldStateConsequenceSystem.cs:193` |
| They are the sector's **medical supply**; if they refuse to trade, medicine leaves the market | `Mutation_MedicalSupplyGone` |
| They have a **settlement**, and it can run out of **water in 6 days** | `FactionLockoutEngine.cs:39,145` |
| They **broadcast**, and die broadcasting | `"The the_empath intercepts their dying broadcast"` |
| `quest_rebuilders_thirst` pays `water_purification_tablets x3`, Hegemony +50 | `FactionLockoutEngine.cs:176` |
| That quest is **mutually exclusive** with `quest_cult_purity` | `LockQuestline` both ways |
| They pay high for **brass fittings — "door handles, nameplates, lamp bases"** | `ExpansionIXItemCatalog.cs:59` |

### Black Ops — what the code already says

| Fact | Source |
|---|---|
| `displayName = "Black Ops (Ex-Military Rebels)"` | `NPC_BlackOps.cs:11` |
| `isHostileToEveryone = true` | `NPC_BlackOps.cs:12` |
| **Booby traps.** Fail a Perception check ≥14 and you open the fight already Bleeding, 25 damage | `NPC_BlackOps.cs:13-14,34-42` |
| Their ammunition is its own provenance class, `BlackOpsMilitary` | `Item_AmmoTypes.cs:95` |
| Their armour rating is 40 — the heaviest in the loot table | `Item_AmmoTypes.CombatLoot.cs:17` |
| The class is a **dormant ghost**, not Boot/Save wired | `NPC_BlackOps.cs:23` |

Two traps for whoever implements this:

- **`Faction_BlackOps` is a declared constant with zero uses.** It is never
  added to `_hegemony`. That is *correct* and should not be "fixed" — a faction
  hostile to everyone has no hegemony to track. It wants a comment, not an
  initialiser.
- **`RegisterTickRebuildersDaily` has nothing to do with this faction.** It
  fires for Accountant / PennyPincher / Auditor survivors. "Rebuilders" there is
  the name of prompt batch #284–298, which is about survivor archetypes. Do not
  wire it to the faction.

---

## The Rebuilders

`faction_rebuilders` · **The Rebuilders**

They do not call themselves that. They call themselves **the Works**, short for
public works, which is the department three of them used to be paid by. The
Rebuilders is what the Toll started calling them, and it was not meant kindly,
and it stuck the way names do.

### The one idea

Every other faction in Sector 4 is trying to hold something. The Works are
trying to **finish** something. They are the only group whose plans are longer
than their own lives, and they are seventy days from dying of thirst, and both
of those facts have the same cause.

They settled the floodplain because it is the only good soil in the sector.

The soil is good because it floods.

Everything that floods there is contaminated.

They can grow food and they cannot drink. Nobody made that happen to them. They
looked at a map, made the correct agricultural decision, and it was the wrong
decision, and they are still there because moving a working farm is not a thing
you can do in one season.

### The Allotments

`loc_the_allotments` — a pre-war municipal allotment scheme on the seam between
The Verge and The Drown. Two hundred numbered plots, a caretaker's hut, a
noticeboard, and a chain-link fence that was there to stop children stealing
runner beans and now serves.

They kept the numbering. Plot 114 is still plot 114. The waiting list is still
on the noticeboard, in a plastic sleeve, and there are forty-one names on it,
and four of those people are alive and farming there now, and they got their
plots in the order they were on the list.

### Why they have the medicine

They run the sector's only still and the sector's only working autoclave, in a
caretaker's hut with a cast-iron stove. Distilling is a fuel problem, not a
water problem: they can make enough sterile water for a clinic and nothing
remotely like enough for two hundred people to drink.

So the Allotments is where you go when someone is bleeding, and the Allotments
is dying of thirst, and those are not a contradiction. That is the entire
mechanical shape of `Mutation_MedicalSupplyGone` and it needed a reason.

### The brass

This is the part that does the work.

`brass_fittings` — *"Door handles, nameplates, lamp bases. High trade value with
Rebuilders."* — already exists and already pays well, and nowhere does anything
say why.

Brass does not corrode, and you cannot cast a valve seat out of scrap steel on a
charcoal forge. Every tap, stopcock and union in a water system has to be brass
or it is a leak with a schedule. The Works are buying every door handle in
Sector 4 because they are building a pipe network and they have run out of the
only metal that will hold a seal.

They are buying door handles to make taps.

**And the player has fourteen brass nameplates in a tin behind the filtration
stack.** (`lore_hz_nameplates`, `02_THE_LIST.md`.) They belong to people who
never arrived. Everyone who has ever found them has put them back.

The Works will pay well for them. They will not ask where they came from —
they never do, it is a courtesy — and the game will not comment, then or ever.
No morale hook, no flag, no line of dialogue. The tin is in the wall and the
price is on the board and that is the whole encounter.

### Ottilie Frayne

Not a leader. The Works have a **committee**, and it keeps minutes, and Frayne
chairs it because nobody else would.

She was a school caretaker. She knows boilers, gutters, keys, stock rotation,
and how to move forty children somewhere in an orderly line without raising her
voice. It is the most useful set of skills in Sector 4.

The Reconstruction Utility Rating scored caretaking at **11**.

She has never seen her own entry and does not know the number. She has,
however, worked out roughly what happened, because she has noticed which trades
are missing from the surface, and she said so once, at a meeting, and it is in
the minutes:

> "There's no one left who can do water. Not a soul, the whole sector. You don't
> get that by accident. Somebody chose."

She was right, and she does not know how right. **Halvard Renn — Water Engineer,
RUR 31, score 71.8** — scored high enough to be allocated, and went underground,
and died at Allocation 12-B four years ago. (`lore_af_renn_dies`.) The formula
took the sector's water engineers off the surface and put them in holes, and the
farm on the floodplain has been thirsty ever since.

His daughter knocks on the player's hatch on Day 200.

### Voice

Minutes of a meeting. They say *carried*, *noted*, *deferred*. They ask you to
sign things, and they mean it, and the paper goes in a box file. Nobody in
Sector 4 finds this less absurd than they do; they do it anyway, because the
alternative is being the sort of place where nothing is written down.

**Signature quote:**

> "Motion: that we plant the north strip anyway. Carried, four to one. The one
> was me."

### If you let them die

The code already specifies the shape (`Retaliation_RebuildersDeath`): six days,
then the Empath intercepts the dying broadcast, then GriefCascade, and possible
sabotage of the player's water purifier.

Write the broadcast as procedure, not as pleading. They read out the plot
numbers and who was farming them. It takes a long time. It is a **handover
document** — the location of the seed store, the depth of the well, which plots
are alkaline, where the spare autoclave gasket is kept — addressed to whoever
gets there next.

Nobody gets there next.

The sabotage is not revenge. The Empath is a survivor who listened to two
hundred people file a report about their own deaths and then had to go and look
at the player's full water tank.

---

## Black Ops

`faction_black_ops` · **Black Ops (Ex-Military Rebels)**

The display name in `NPC_BlackOps.cs` is what the Toll calls them, and it is
wrong in all three particulars. They are not black — they were never covert.
They are not ops — they have not received an order in five years. They are not
rebels — rebelling would require having stopped.

Their own designation is **D/9**, spoken *dee-nine*, and it stands for Ninth
Denial Detachment, which is a boring name for a boring job.

### The one idea

They were given a continuity task before the Exchange: on the loss of central
command, **deny the sector's fixed infrastructure to any organised force.**
Bridges, pumping stations, the rail cut, the substations. Not hold them. Deny
them. There is a doctrinal difference and it explains everything about how they
fight.

Denial doctrine does not take ground, so they lay traps and leave.

Denial doctrine does not distinguish between a hostile organised force and a
friendly one, because that determination was somebody else's job, at a
headquarters, and the order to make it never came.

So `isHostileToEveryone = true` is not a personality. It is an **unamended
tasking order**, and it is the same machine that ran the Continuity Allocation
Schedule and held Convoy 12 at a checkpoint over a date of birth: people doing
their jobs correctly after the job stopped meaning anything.

D/9 is that idea in its most violent register. It is the same story as Margit
Sole. She kept filing. They kept mining bridges.

There are four of them left.

### The marks

Doctrine requires that friendly obstacles be marked, so that your own people do
not walk into them.

D/9 has never stopped complying.

**Every trap in Sector 4 is marked.** A specific scratch on a guardrail, a stone
moved to a specific side of a culvert, a strip of tape at a specific height.
Nothing hidden. Fully, correctly, uselessly signposted, in a notation that four
people alive can read.

This turns the Perception ≥14 check from a stat wall into located knowledge.
The notation is a real document — an obstacle-marking annex — and it survives in
at least three places: a Garrison field manual, the Ministry's civil-defence
files, and Anneke Ruhl's own head.

> `knowledge_key: lore_denial_marks` ·
> `discovery_location_id: location_ministry_of_truth_bunker` ·
> `discovery_trigger: inspection`

Once the player has it, the check is trivial and the sector reads differently in
retrospect: the scratches were on the guardrails the entire game. Nobody hid
anything from anybody. That is the point.

### Signals Sergeant Anneke Ruhl

The ranking survivor of D/9, and the lowest-ranked of its original complement,
because the officers took the tasks with the shorter fuses.

She holds the detachment's authenticator: a one-time pad that would let her
verify a stand-down order if one ever arrived.

She listens for it. Fixed frequency, fixed time, every night, five years. She
does not expect it. She has said, to the two people who have asked and lived,
that she does not expect it. She listens anyway, because listening is on the
schedule and the schedule is what is left.

She is not waiting to be forgiven. She is waiting to be **told she can stop**,
and the only authority that could tell her stopped existing before the ash fell.

### The stand-down

You cannot ally with D/9. `isHostileToEveryone` is true and stays true.

You can end them, though, and not with ammunition.

Continuity authorities issued stand-down orders. The Office of Continuity was a
continuity authority. **Margit Sole was Records Clerk Grade II, Office of
Continuity**, and she holds the authority codes, because she filed them.

She has been able to sign a valid stand-down for five years.

Nobody has ever asked her, because nobody knew a records department had that
power, because the Office of Continuity was proud of how boring it was.

If the player reaches Layer 4 and thinks to ask, she will write it out. Correct
form, correct codes, her own signature and grade. Ruhl will run it against the
pad. It will verify.

And it is exactly as real as Sela Renn's laminated card in the freezer bag: a
piece of paper that is legally perfect under a state that does not exist. One of
them asks to be let in. One of them asks four people to stop killing. The game
presents both and does not say which kind of true they are.

**What actually changes:** D/9 stands down. That is all.

The traps stay armed. Four people are not going to walk five years of denial
work backwards, and some of the maps were on Corporal Vane, and Vane is at the
bottom of the rail cut. The encounter ends. The hazard does not. Nobody thanks
anybody.

### Voice

Flat, procedural, unhurried. Distances in metres, times on the 24-hour clock,
the passive voice for anything they did. They do not threaten, because a threat
is a negotiation and they are not negotiating.

**Signature quote:**

> "The bridge was denied on the fourteenth. Nobody has rescinded that."

---

## What the two of them are for

Put together, they are the same sentence twice.

The Works are what people build when nobody tells them to. D/9 is what people
keep doing when nobody tells them to stop. One is a farm that will outlive its
farmers. One is four people mining a bridge for a headquarters that is a hole in
the ground.

Frayne noticed the water engineers were missing and said so at a meeting.
Ruhl listens every night for an order that was never sent. Sole files a list
that excluded her. None of the three is a fool, and all three are correct, and
it does not help any of them.

That is the bible's thesis and these two are where it stops being a mood and
starts costing the player water and blood.

---

## Schema-ready entries

Append to `Assets/StreamingAssets/Data/faction_lore.json`. Schema matches the
existing four entries exactly. `relationships` values are restricted to the
three the file already uses — `hostile`, `suspicious`, `neutral`.

Ids use the **systems namespace** (`faction_rebuilders`, `faction_black_ops`)
rather than inventing lore-namespace twins. `00_OVERVIEW.md` flags the existing
`ash_militia` / `faction_upland_militia` split as a live defect; these two have
no lore-side id yet, so matching the code is the one choice that does not create
a second instance of that bug.

```json
{
  "faction_id": "faction_rebuilders",
  "display_name": "The Rebuilders",
  "ideology": "Municipal continuity. Public works, minuted decisions, and plans longer than the people making them",
  "origin_story": "Three surviving employees of a district public works department walked out to the floodplain allotments in the first winter because it was the only good soil left in Sector 4, and began farming it in plot order off the pre-war waiting list. They call themselves the Works. The Rebuilders is what the Toll called them, and it was not meant kindly. The soil is good because the ground floods, and everything that floods there is contaminated, so they can grow food and cannot drink; two hundred people now live seventy days from thirst on the best farmland in the sector. They hold the only still and the only working autoclave for forty kilometres, which makes a caretaker's hut the sector's clinic. Their standing purchase order is brass -- door handles, nameplates, lamp bases -- because you cannot cast a valve seat out of scrap steel and every joint in a water system is brass or it is a leak with a schedule.",
  "key_beliefs": [
    "Write it down or it did not happen and cannot be handed over",
    "The waiting list is the waiting list; plot 114 is still plot 114",
    "A thing worth building takes longer than the person building it",
    "There is no one left in this sector who can do water, and that was chosen, not accidental",
    "We do not ask where the brass came from"
  ],
  "dialogue_style": "Minutes of a meeting. Carried, noted, deferred. Asks you to sign things and means it",
  "signature_quote": "Motion: that we plant the north strip anyway. Carried, four to one. The one was me.",
  "relationships": {
    "iron_garrison": "suspicious",
    "ash_militia": "neutral",
    "cult_of_ash_sign": "hostile",
    "warlords_sector_4": "hostile"
  },
  "tribute_demands": [
    "brass_fittings",
    "water_purification_tablets",
    "clean_water_by_the_barrel"
  ],
  "tech_offerings": [
    "antibiotics",
    "antiseptic",
    "surgical_kit",
    "seeds",
    "sterile_field_surgery"
  ]
}
```

```json
{
  "faction_id": "faction_black_ops",
  "display_name": "Black Ops (Ex-Military Rebels)",
  "ideology": "Denial of fixed infrastructure to any organised force, under a tasking order nobody has rescinded",
  "origin_story": "Ninth Denial Detachment -- D/9 -- was given a continuity task before the Exchange: on loss of central command, deny the sector's bridges, pumping stations, rail cut and substations to any organised force. Not hold them. Deny them. The order to distinguish a hostile organised force from a friendly one was somebody else's job at a headquarters that is now a hole in the ground, so the distinction has never been made and the tasking has never been amended. They do not take ground; they lay charges and leave. Four are left. Doctrine requires friendly obstacles be marked so your own people do not walk into them, and they have never stopped complying, so every trap in Sector 4 is correctly signposted in a notation four living people can read. The Toll calls them Black Ops, which is wrong in all three particulars: they were never covert, they have had no orders in five years, and rebelling would require having stopped.",
  "key_beliefs": [
    "The tasking stands until it is rescinded by an authority that can authenticate",
    "Obstacles are marked. That is not mercy, it is the annex",
    "Ground held is ground you have to keep holding",
    "Everyone organised is, by the clock we are still running on, an occupying force"
  ],
  "dialogue_style": "Flat and procedural. Distances in metres, times on the 24-hour clock, passive voice for anything they did. Does not threaten, because a threat is a negotiation",
  "signature_quote": "The bridge was denied on the fourteenth. Nobody has rescinded that.",
  "relationships": {
    "iron_garrison": "hostile",
    "ash_militia": "hostile",
    "cult_of_ash_sign": "hostile",
    "warlords_sector_4": "hostile"
  },
  "tribute_demands": [],
  "tech_offerings": []
}
```

## Implementation notes

**1. Two loader warnings until the DTO grows.**
`FactionLoreCatalogLoader.cs:60` logs a warning for any `faction_id` outside its
4-name whitelist, and `FactionRelationships` has no field for these two, so
their `relationships` blocks are silently dropped. Six lines fix it — two fields
on `FactionRelationships`, two entries in `KnownRelationshipFactionIds`, two
`AddIfPresent` calls in `ToRelationshipMap`.

Note the asymmetry if you skip it: these two can *point at* the canon four,
but the canon four still have no field to point back. The Cult killing the
Rebuilders is in the code and would remain unsayable in the Codex.

**2. `dialogue_style`, `tribute_demands` and `tech_offerings` are inert.**
They exist on all four shipped entries but are not fields on `FactionLoreEntry`,
so nothing reads them today. Authored here for parity and for whatever reads
them later. `brass_fittings`, `water_purification_tablets`, `antibiotics`,
`antiseptic`, `seeds` and `surgical_kit` are verified item ids;
`clean_water_by_the_barrel` and `sterile_field_surgery` are descriptive slugs in
the same style as the existing `young_recruits_for_service` and
`artillery_support`.

**3. Leave `Faction_BlackOps` uninitialised.** Add a comment saying why, or
somebody will add it to `_hegemony` as a tidiness fix and give a
hostile-to-everyone faction a reputation bar.

**4. New content ids introduced here**, all verified collision-free:
`loc_the_allotments`, `lore_denial_marks`. Characters: Ottilie Frayne,
Signals Sergeant Anneke Ruhl, Corporal Vane.

**5. No audio.** The dying broadcast and Ruhl's nightly listening watch are
both text-and-silence as written. If either is ever voiced it belongs in
`EXTERNAL_AUDIO_REQUIREMENTS.md` under the ElevenLabs pipeline, not here.

## What this deliberately does not do

- **Does not make D/9 alliable.** `isHostileToEveryone` stays true. The
  stand-down ends the encounter; it does not add a friend.
- **Does not disarm the traps.** Standing down is not cleanup.
- **Does not make the Rebuilders saveable in general.** The code gives one
  water quest, locked against the Cult questline. They stay fragile.
- **Does not promote either to a fifth Power.** `00_OVERVIEW.md` closes the
  territorial map at four, and that holds: the Works occupy one uncontested
  floodplain and their hegemony gates a *market*, not ground — their only map
  mutation is `Mutation_MedicalSupplyGone`. D/9 is anti-territorial by doctrine.
- **Does not adjudicate the stand-down order.** Same rule as Sela Renn's card.
  The game presents the paper and stops talking.
