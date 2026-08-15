# Factions — Powers and Currents

> Target files: a **new** `currents.json` catalog + `NPC_*.cs` classes.
> Deliberately **not** `faction_lore.json` — see *Why not faction_lore.json*.

## The rule

Adding factions to a world that already has four well-drawn ones usually makes
all of them weaker. The fix is that the new ones are not the same *kind* of
thing.

| | **Powers** (4) | **Currents** (14) |
|---|---|---|
| Hold territory | yes | **never** |
| Can be at war | yes | no — they pass through wars |
| Demand tribute | yes | no — they trade one specific thing |
| Player relationship | trust, standing, lockout | access, granted or withdrawn |
| Where they live | a sub-region | a *practice* |

A Power asks *whose side are you on*. A Current asks *do you have what I need,
and did you follow the etiquette*. You cannot conquer a Current, you can only
lose access to it — which is worse, because access is how you survive.

The map from `01_GAZETTEER.md` stays closed. Four powers, five sub-regions.
Currents cross all of it.

## Why not `faction_lore.json`

`FactionLoreCatalogLoader.cs` carries this comment, already written by whoever
built it:

> *"FactionRelationships is a fixed-field DTO (JsonUtility can't deserialize
> into a Dictionary), so it only has slots for these 4 factions. A 5th faction
> id here would have any relationship data referencing it silently dropped."*

The loader logs a warning and drops the data. So a fifth entry in that file is
not a content decision — it's a C# change to the DTO, the loader, and the five
fixed nodes in `FactionRelationshipMap.cs`.

Currents don't need it. They have no `relationships` — that is the entire point
of them — so they belong in their own catalog, matching the architecture that
`NPC_Archivists.cs`, `NPC_SunSeekers.cs` and `NPC_Osteophages.cs` already use:
a standalone state class with its own trust float.

**Three of the eight below already exist in code.** Their lore is supplied
here; their behaviour is already written.

---

# Peaceful Currents

## 1. `faction_archivists` — The Archivists of the Before
**Status: already coded** (`NPC_Archivists.cs`) · Drown · *peaceful*

Monastic order of Bunker-Born who venerate pre-war media as ancestral spirits.
Tithe: photo albums, cassettes. Pays in morale and `item_encrypted_drive`.

Origin, and the reason the order exists at all, is in `02_THE_LIST.md` — one
records clerk who never stopped filing, and a generation of children who
watched her and inferred a religion from observed practice.

> "Say the name aloud while you write it. If you don't say it, you're only
> copying."

**Story — *The Corroboration*.** The Archivists will not accept a name into the
Schedule on one testimony. To register your own dead, you must find a second
person who knew them. For an early-game survivor who came to the shelter alone,
there is no second person, and the entry cannot be made. The order is not being
cruel; the rule is the whole point of the rule. Late-game, if the player has
mapped enough of Sector 4, a corroborating witness can sometimes be *found* —
and that quest is the warmest thing in the game.

## 2. `faction_lamplighters` — The Lamplighters
**Status: new** · all regions · *peaceful, universally protected*

They maintain the route beacons: the Toll's reflector posts, the pilgrim
markers on the switchbacks, the channel lights in the Drown. Salvaged
high-visibility jackets, a fuel can, a ledger of lamp positions and last-lit
dates.

Every faction protects them, because every faction travels. The Garrison issues
them passes. The Warlords waive their toll. The Cult considers them a lesser
but legitimate order. This is not a treaty and has never been written down.

**Wants:** lamp oil, wicks, reflector glass, batteries. Chronically short of
all four.
**Offers:** the lit routes themselves — reduced travel time and a lower ambush
chance on any lamped road — plus the ledger, which is the second-best map in
Sector 4 after Ostrowski's.

**The rule that defines them:** *a beacon is lit for whoever is walking.* They
do not ask who, or why, or which way they are going.

> "I don't light it for you. I light it."

**Story — *The Rule Held*.** The senior Lamplighter is a woman named **Ivy
Corrigan**. Four years ago she lit the north approach on a clear night, as the
ledger required, for a column she could see was armed and could see was not
Garrison. The rule does not have an exception and she did not invent one. They
followed the lamps into the Verge settlement she had been born in.

She still walks that route. She still lights that post. The ledger shows it
lit, every scheduled night, for four years, in the same hand.

The player can offer her an exception — fuel on condition she goes dark for a
night, to trap or evade someone. **She refuses, every time, and the refusal is
not negotiable and never becomes negotiable.** If pressed twice, the
Lamplighters withdraw access permanently and the lamps in your region go out
one at a time over eleven days.

## 3. `faction_quiet_house` — The Quiet House
**Status: new** · The Grid, a single building · *peaceful, unsettling*

They take the dying that nobody else can care for. Radiation cases past
treatment, terminal afflictions, the very old. No payment. No sermon. They ask
for exactly two things: **the person's name, and one true thing about them.**

They are not medics and do not claim to be. They keep people warm, clean, and
not alone.

**Offers:** the removal of an unrecoverable survivor without the morale
collapse of a death in the shelter — and the return, later, of that survivor's
personal effects, catalogued, with the true thing written on the tag.

**Wants:** blankets, ethanol, and the name.

**The unanswered question.** There is a room at the back that no visitor
enters. Asked what happens in it, the answer is always the same four words:

> "We make it quiet."

The game never adjudicates this. It is never revealed, never confirmed, never
denied, and there is no quest that resolves it. Survivors in your shelter will
hold both opinions and argue about it, and the argument is the content.

**Story — *One True Thing*.** The player must supply the true thing themselves,
in their own words, choosing from options that reflect what actually happened
in the run — including the option to lie, which the House accepts without
comment and writes on the tag exactly as given.

## 4. `faction_grain_exchange` — The Grain Exchange
**Status: new, grounded in canon** (`world_history`: *The Grain Exchange*,
Exchange+4Y) · The Verge / The Toll border · *peaceful, fragile*

Not a group of believers — a **clearing house**. Posted rates, a weighed scale,
a chalked board, and four faction representatives who attend because the
alternative is worse. It is the only place all four Powers do business at the
same table.

It has no guards, no charter, and no enforcement mechanism whatsoever. It works
for exactly one reason: **everybody attending is hungry.**

**Offers:** bulk trade at rates far better than any faction will give you
directly, and the only reliable read on what the four Powers are actually short
of this season — which is intelligence, and better than the Garrison's.

> "The board is the board. Argue with the board."

**Story — *The Year Somebody Wasn't Hungry*.** If the player's shelter reaches
genuine food security — surplus for 30 consecutive days with the Verge in
drought — the Exchange offers the player the seat of **setting the board.**

Take it and it works. Rates favour you. It is a straightforwardly good economic
outcome and the game presents it without irony.

The Exchange does not collapse in a dramatic event. It simply has fewer
attendees each season, and the board stops being repainted, and roughly ninety
days later a Verge trader mentions in passing that they don't go up there any
more. Nobody blames the player. Nobody connects it. The `world_history` entry
for *The Grain Exchange* gains a second paragraph, discoverable at
`loc_weighbridge`, written in the same civil-service register as the first, and
it ends with the date it stopped.

---

# Dangerous Currents

## 5. `faction_sun_seekers` — The Sun-Seekers
**Status: already coded** (`NPC_SunSeekers.cs`) · surface, all regions ·
*conditional — trades or raids*

Surface-dwellers who worship the Ozone Scourge as a cleansing light. Trade
**only** during `WeatherKind.FalseSpring` / `SilentSpring`, demanding UV visors
and welder's glass in exchange for solar tech. Raid violently if they detect
that a shelter is hoarding UV protection.

The lore that makes the mechanic land: they are not seeking death. They are
seeking *the sun*, which they have not properly seen in five years, and they
have built an entire theology around the few days a year the sky thins. When it
thins, they come out, and they are joyful, and they will trade you anything.

The rest of the year they are underneath something, waiting, and the waiting is
what has gone wrong with them.

> "It came back. We told you it comes back."

**Story — *False Spring*.** The trade window is genuinely the best in the game
and lasts as long as the weather does. The player will be tempted to stockpile
visors *for* the window. Stockpiling is exactly what triggers the raid check.
The optimal play and the fatal play are the same play, and nothing warns you.

## 6. `faction_osteophages` — The Osteophages
**Status: already coded** (`NPC_Osteophages.cs`) · The Drown fringe ·
*dangerous, transactional, tragic*

The Rust-Eaters. Heavy-metal poisoning and pica sufferers who gnaw copper wire
and rusted pipe. They accept toxic tech trash — and exiled, chelation-starved,
mentally broken survivors — through an airlock, and return purified
`item_copper_wire` and `item_scrap_metal`.

They are the darkest transaction in the game and they are not villains. They
are ill, they are organised, and their process works.

**The detail that does the work:** they return the metal *clean*. Better than
you could refine it. Whatever they are, they are good at this, and you will
keep trading with them.

> *(The Osteophages do not have a signature quote. They do not negotiate
> verbally. There is a chute, and a bell, and a wait.)*

**Story — *What the Airlock Returns*.** A survivor exiled to the Osteophages is
gone. But roughly forty days later, a delivery arrives containing, among the
scrap, one item that belonged to them — cleaned to the same standard as the
metal. No message. The game does not explain whether this is sentiment, or
sorting, or the survivor themselves.

## 7. `faction_the_tally` — The Tally
**Status: new** · The Toll, and wherever a debt is · *dangerous, lawful*

Debt enforcement by written contract. They do not raid, do not extort, and have
never once been known to break an agreement — including agreements that end
with someone's death.

A Tally contract states the debt, the term, the rate, and the forfeit, in
plain language, signed by both parties. They will read it back to you before
you sign. They will read it aloud again, in full, before they enforce it.

They are more frightening than the Warlords because the Warlords can be
bargained with.

**Offers:** genuine credit — the only source of it in Sector 4. Goods now,
against a forfeit later. The rates are fair. The contracts are honest.
**Wants:** the forfeit. On the day. As written.

> "You've heard it. Do you want it read again? Most people want it read
> again."

**Story — *The Collector's Side*.** The player can also **hire** the Tally, to
recover something genuinely owed to them by a faction or a survivor. It works.
It is efficient, lawful, and precisely as written.

Every survivor in the shelter who has ever owed anyone anything watches the
player do it. There is no morale event, no dialogue, and no consequence
flagged. Three of them simply have a new line available in their next personal
quest, and it is about the player.

## 8. `faction_undertow` — The Undertow
**Status: new** · The Drown · *dangerous, deniable*

Wreckers. They salvage the accidents that happen in the Drown, and the
accidents that happen in the Drown happen at a rate that is difficult to
explain by water alone. Channel markers move. Ice is thin in places it was
thick last week. Moorings come loose in still weather.

Nothing has ever been proven. There is nobody to prove it to.

They do not present as a faction, ever. They present as **helpful strangers who
arrive very quickly.**

**Offers:** rescue, salvage recovery, and local knowledge — all real, all
delivered, all at a price agreed after you are already in the water.
**Wants:** the Drown to stay unnavigable to everyone else.

> "Lucky we were close."

**Story — *The Kittiwake Chart*.** This is the questline that interlocks with
`loc_bathymetric_boat` in `03_LOCATIONS.md`. The survey launch's logbook
contains the only accurate chart of the flooding — eleven days of soundings
with timestamps.

Copy it and distribute it, and the Drown becomes navigable for everyone: the
Shallows market grows, `loc_cold_store_atlantic` becomes reachable, the
Archivists stop being isolated, and Sector 4's whole late game opens up.

It also ends the Undertow's business model permanently, and they know the
moment the first copy circulates.

They do not attack the player. They have never attacked anyone. What happens
instead is that expeditions into the Drown start having accidents, at a rate
that is difficult to explain by water alone, and every single time, someone is
lucky enough to be close.

---

## Interlocks

The Currents are written to cross each other, so the world feels like a system
rather than a menu:

| | interacts with | how |
|---|---|---|
| Lamplighters | Undertow | lit channel markers are the Undertow's direct enemy |
| Kittiwake chart | Archivists | distribution ends their isolation and their safety at once |
| Grain Exchange | Tally | the Exchange has no enforcement; the Tally sells exactly that |
| Quiet House | Osteophages | the two ends a dying survivor can be sent to, and the contrast is the argument |
| Sun-Seekers | Lamplighters | the only group that does not want the routes lit at night |
| Tally | all four Powers | credit crosses faction lines; debt is the one thing everyone honours |

---

---

# Second wave — the orphaned badges

`Assets/Resources/Art/Factions/` already contains **seven finished faction
badge artworks with no lore, no id in `faction_lore.json`, and no code
presence**, plus `faction_leader_1.jpg` and `faction_leader_2.jpg` — two leader
portraits for leaders who have never been named.

Somebody generated art for a faction roster that was never written. These are
free factions: the expensive part is already done.

Where a badge fits a Current above, it should simply be adopted as that
Current's art. Where it does not, it names a faction worth having.

| Badge asset | Assignment |
|---|---|
| `faction_badge_free_traders` | **The Grain Exchange** (§4) — adopt as-is |
| `faction_badge_scientific_remnant` | **The Cold Count** — new, below |
| `faction_badge_deserter_coalition` | **The Deserter Coalition** — new, below |
| `faction_badge_doomsday_preppers` | **The Provisioned** — new, below |
| `faction_badge_outcast_nomads` | **The Long Walk** — new, below |
| `faction_badge_scavenger_guild` | **The Scavenger Guild** — new, below |
| `faction_badge_iron_raiders` | **The Iron Raiders** — new, below |
| `faction_leader_1` / `faction_leader_2` | Ivy Corrigan (Lamplighters) and the Tally's reader |

## 9. `faction_cold_count` — The Cold Count
*The Spine · peaceful · badge: `scientific_remnant`*

Four surviving researchers working `loc_low_background_lab` — the shielded salt
chamber that can still measure accurately enough to distinguish fallout
isotopes by origin.

They are not withholding the answer. They have it. They published it, once, on
paper, to an audience of nobody, and filed the copy.

**Wants:** power, shielding, and samples from places nobody sane goes.
**Offers:** accurate rad readings — the difference between a hot zone that is
survivable in four hours and one that is not — plus provenance analysis.

> "It's not a secret. It's a measurement. Nobody came to collect it."

**Story — *Provenance*.** The Cold Count can prove where the warheads came
from. Any faction that learns they can do this immediately wants the result,
and each of the four wants a *different* result. The Count will not falsify it
and does not understand why they keep being asked to.

## 10. `faction_deserter_coalition` — The Deserter Coalition
*The Verge and the treeline · conditional · badge: `deserter_coalition`*

Ties directly to `faction_deserter_asylum`, already in code, and to
`npc_ivor_lasko` in `04_ENCOUNTERS.md`. Garrison deserters who found each other.
Voss's standing order applies to every one of them.

They are decent people who are wanted for a capital offence, which makes them
unpredictable in exactly one way: they cannot ever be seen.

**Wants:** silence, civilian clothing, and papers.
**Offers:** Garrison patrol schedules, weapon maintenance nobody else can do,
and disciplined fighters who cannot be called on in daylight.
**Risk:** sheltering them is the single fastest route to a Garrison lockout.

## 11. `faction_the_provisioned` — The Provisioned
*The Grid and outlying · conditional · badge: `doomsday_preppers`*

Private shelter owners who were never on the Continuity Allocation Schedule and
did not need to be. They built their own, paid for it themselves, and were
proved right.

**They are the spine's counter-argument.** Where the player's community
survived by proximity and luck, the Provisioned survived by having been
correct in advance, and they have had five years to develop opinions about the
difference.

**Wants:** almost nothing. That is what makes them difficult.
**Offers:** pre-war stock in original packaging, and the only working
pre-Exchange technology in Sector 4.

> "Nobody helped us build it. I notice nobody's asking whether we'd like help
> now."

**Story — *The Knock*.** A Provisioned shelter, four people, failing filtration
they cannot repair. They will trade extraordinarily well for one air filter.
They will also not say please, and one of your survivors will want them refused
on principle, and will say so out loud, using the word *fair*.

## 12. `faction_long_walk` — The Long Walk
*everywhere, briefly · peaceful · badge: `outcast_nomads`*

They do not stop. Thirty-odd people moving a continuous circuit of Sector 4 on
a route that takes roughly eleven months, because a moving target is not worth
raiding and because ground recovers if you leave it alone.

They arrive, trade for a day, and go. They will not stay a second night, for
anyone, for any offer.

**Wants:** water, footwear, and news.
**Offers:** goods from regions the player cannot reach yet, and an accurate
account of what is happening everywhere else — the only true Sector-wide
situation report in the game, delivered once, verbally, by someone already
walking away.

> "We'll be back round in about a year. Don't hold anything for us."

## 13. `faction_scavenger_guild` — The Scavenger Guild
*The Grid and Toll · conditional · badge: `scavenger_guild`*

Professional salvage, organised as a trade guild: territories, apprenticeships,
and a rule against stripping a site below the point where it can be worked
again.

They are not sentimental. The rule is economic — a site cut to the frame yields
once. They will trade with anyone and they will blacklist a shelter that
over-strips a claimed site, and the blacklist is honoured across the whole
Guild, permanently.

**Wants:** claim respect, and tools.
**Offers:** the richest salvage routes in Sector 4, and apprenticeship — a
survivor sent to the Guild for 30 days returns with a permanent scavenging
bonus and opinions about the player's methods.

## 14. `faction_iron_raiders` — The Iron Raiders
*The Toll fringe · dangerous · badge: `iron_raiders`*

The genuine article: no code, no contract, no ideology. What the Warlords are
constantly and inaccurately accused of being.

They exist to make the Warlords legible. After an Iron Raider encounter, the
Tollman's posted rates and honest receipts read very differently, and the
player understands why the Toll is tolerated.

**Wants:** what you have.
**Offers:** nothing. There is no trade interaction. They are the only faction in
this document with no `offers[]` entry, and that absence is the design.

> *(no signature quote — they do not announce themselves)*

---

## Implementation notes

**New catalog** `Assets/StreamingAssets/Data/currents.json`, loaded by a
lightweight loader following the `PhantomTriggerCatalogLoader` pattern. Proposed
schema — flat, JsonUtility-safe, no dictionaries:

```
id, display_name, alignment, home_region, is_active, trust,
wants[], offers[], signature_quote, access_rule
```

`alignment` ∈ `peaceful | conditional | dangerous`. No `relationships` field —
by design. Currents have no diplomacy.

**Eleven new `NPC_*.cs` classes**, mirroring the three that exist:
`NPC_Lamplighters`, `NPC_QuietHouse`, `NPC_GrainExchange`, `NPC_Tally`,
`NPC_Undertow`, `NPC_ColdCount`, `NPC_DeserterCoalition`, `NPC_Provisioned`,
`NPC_LongWalk`, `NPC_ScavengerGuild`, `NPC_IronRaiders`.

**Art is already done for seven of them.** The badges in
`Assets/Resources/Art/Factions/` are finished assets — adopting them costs no
generation credits and retires seven orphaned files at the same time. Wiring
them is a `GameAssetService` lookup, following the existing item-icon path.

**Before any of this is built, resolve the namespace split** flagged in
`00_OVERVIEW.md`. Currents sidestep it — they use neither the `faction_lore`
nor the `faction_central_*` namespace — but `faction_rebuilders` is still
tracked for hegemony and invisible to the player, and adding eight more
faction-shaped things on top of an unreconciled split will make it permanent.

**Open question for the owner:** the Rebuilders. They have hegemony tracking, a
`PersonalQuestSystem.Rebuilders.cs`, and trade value on
`Item_BrassFittings` — but no lore entry and no presence. They are either a
fifth Power that was started and abandoned, or a Current that was mis-filed.
I did not decide this one; it changes the map, which is yours.
