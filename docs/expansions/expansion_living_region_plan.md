# ASHFALL — THE LIVING REGION
### The ground keeps moving · Settlements, refugees and prices shift with the war

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/living-region-2026-09-29.md` (packages, paths, acceptance, decision register)
**Family:** "The world moves without you" — with *The Long Line: Freight*, *The Drowned Coast*, *The Plague Year*. Boundary index: `docs/expansions/expansion_world_moves_without_you_index.md`.
**Tone lock (inherited, unchanged):** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel. No magic, no real countries, wars, people, or copied text.
**Convention:** **LIVE** = confirmed in source/data during this audit · **GAP** = confirmed missing or stubbed · **PROPOSED** = new · **VERIFY** = plausible, must be re-checked at implementation start.

---

> *"The war does not visit the shelter. The war is the weather the shelter is standing in."*
>
> Twelve settlements exist in the data today as fixed points on a map. They are not places yet. A
> place is what a name becomes when something is *happening* to it while you are looking somewhere
> else.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

Today the region is a stage set that is repainted once a day. Wildlife packs migrate, landmarks fall, four goods get dearer when the packs thin, and a faction can be "dominant" — but the twelve settlements the player trades with, prays beside, and fears do not *live*. Brine-Pan Hollow has "50 people" in a catalog and will have 50 people on Day 700. Prices in the Flotilla are cheap for foundry brine because a file says so, not because the foundry is still burning.

The Living Region makes the settlements answer the war. Not with a new simulation — the corpus already has five clocks (weather, wildlife, migration, faction war, embargo). What is missing is the thing that *reads them together and remembers*: a settlement that has been short of water for nine days is **Failing**, not "priced at 1.3×"; a column of people leaving the Toll arrives, eleven days later and a third smaller, at the gate of a camp that was built for sixty.

The player's promise: **you will hear the region change before you see it, see it before you can fix it, and what you do at the gate is remembered by the people who came through it.**

Five rungs — Steady, Strained, Failing, Emptied, Swollen — and notice that *Emptied* is not the
bottom and *Swollen* is not the top. Both are arrivals, not ends. A settlement can be Swollen with
refugees and Strained by the same fact. And there is **no "Thriving"** anywhere in the ladder,
which is the quietest and most unsettling decision in the whole family. Nothing gets better. It
only stops getting worse.

The only way the player learns any of it is hearsay: **Heard / Told / Seen**. Three grades of
not-quite-knowing. The world does not lie to you — it just gets to you late and slightly wrong,
and then, to its credit, corrects itself.

### 1.2 Design pillars

1. **The world does not wait.** State moves on the day tick whether or not the player is looking. Nothing here is generated on player arrival.
2. **News has a grade.** Every fact about a settlement reaches the player as *Heard* (rumour, may be wrong), *Told* (a trader's word, usually right, late) or *Seen* (the player visited). State is one truth; the view is graded. No hidden second simulation.
3. **The war is a cause, not a backdrop.** A settlement's condition explains itself: "short of water", "road cut", "ninety lost to the Toll" — never a bare number.
4. **One authority per concern.** Population lives with migration. Prices move through the market's existing shock seam. Ownership lives with LocationEvolution. This plan adds a *reader* and a small ledger; it does not add a second economy.
5. **Refugees are people at a door.** A wave is a decision the shelter has to make with the food it actually has.
6. **Ship dark.** With no new catalog present, every save and every existing day behaves exactly as today.

### 1.3 What this is not

Not a city-builder. Not a faction-diplomacy rewrite (FactionWarSystem stays the war authority). Not a new map, region set, or routed panel. Not a promise that every settlement can be saved.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | The evolving world (24 sectors, 24 packs, 30 landmarks, 40 location seeds, shelter sector `sector_4_hinterlands`) ticks daily in phase 4 (`world_evolution`). | `Assets/StreamingAssets/Data/world_evolution_seeds.json`; `src/Main.CampaignOwners.cs` `EvolvingWorldDayOwner` (~L2150–2330) | LIVE |
| F2 | Wildlife pressure moves market scarcity for four goods (`canned_food`, `cooked_meat`, `item_smoked_meat`, `clean_water`): +0.02 below 0.6 population ratio, +0.005 below 0.85, −0.005 above 1.2; scaled 0.5×/1.0×/1.5× by whether an active caravan origin supplies the good. | same owner, "Wildlife pressure → market scarcity"; `RegionalSupplyRouter.WorldShortageDemandScale` | LIVE |
| F3 | Seasonal human migration: 4 factions × 4 phases (`deep_winter`, `thaw`, `dry_heat`, `ash_winds`), regions `settlement`, `iron_basin`, `industrial_belt`, `ash_flats`, `deep_coast`, dwell 10 days; persisted regional weights; consequence engine with an exactly-once ledger (`AppliedConsequenceKeys`) exposing demand, labour, territorial-friction and caravan-priority multipliers. | `Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs`, `MigrationConsequenceEngine.cs`; `src/Main.MigrationConsequence.cs` `TickMigrationConsequence`; save sections `human_migration`, `migration_consequence` | LIVE |
| F4 | 12 authored settlements (archetype, governance, population, threat, attitude, economy, faction gate). Static catalog. | `settlements.json`; `Assets/Ashfall.Core/World/SettlementCatalog.cs` | LIVE (data) / **GAP** (no mutable per-settlement state) |
| F5 | Regional price atlas: 24 static entries, permille bounded [500, 2000], scarcity profiles `local_surplus / balanced / imported_scarce`. | `regional_prices.json`; `RegionalPriceAtlas.cs` | LIVE (static) |
| F6 | **Four region vocabularies with no mapping**: settlements use `the_toll / industrial_belt / coastal_shelf / high_scarp / dead_suburbs / the_drown / the_verge / the_cluster`; migration, caravan routes and embargoes use `settlement / iron_basin / industrial_belt / ash_flats / deep_coast`; the price atlas uses `coastal / flotilla / foundry / greenhouse / settlement / traplines`; the map uses 8 `reg_*` ids and the evolving world uses `sector_*` ids. | the JSON files above; `map_regions.json` | **GAP** |
| F7 | War authority: `activeWarTension` 0–100, `dominantFactionId`, per-faction standing and territorial control %, defense-readiness pressures; 38 authored war event chains. | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`; `faction_war_events.json` | LIVE |
| F8 | Dominance → ownership loop only re-owns seeds whose **authored** owner already equals the new dominant faction, skipping ones already owned by it. | `EvolvingWorldDayOwner`, "Faction dominance → location ownership" | **VERIFY** — reads as restoration, not conquest; confirm intent before building on it |
| F9 | 19 faction territories and 5 contested zones are catalogued; `PatrolTerritoryAuthority` has **zero** references under `src/`. | `faction_territory.json`; `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs`; grep of `src/` | **GAP / VERIFY** |
| F10 | Weather-driven embargoes (14 rules) and weather shocks already touch caravans and prices with decay to neutral. | `trade_embargoes.json`; `TradeEmbargoSystem.cs`; `EconomyWeatherShockRules.cs` | LIVE |
| F11 | `settlement_silo_burrow` is an authored **Refugee Camp** (`the_verge`); nothing sends it refugees. | `settlements.json` | GAP |
| F12 | The world already speaks through capped radio intercepts (≤3 wildlife sightings/day) and journal lines. | `EvolvingWorldDayOwner` | LIVE |
| F13 | The market owner exposes one idempotent shock seam used by migration consequences. | `src/Main.MigrationConsequence.cs` (`ApplyMigrationMarketConsequence`) | LIVE |
| F14 | Rumour/market-tell vocabulary exists. | `EconomyMarketRumorRules.cs`; `trade_tell_lines.json` | LIVE (VERIFY reach into settlements) |

### 2.1 The disagreement to resolve first (Rule 6 — systems win)

The four region vocabularies (F6) mean "the Toll is starving" cannot be said in code today. **Systems win:** the canonical vocabulary is the **supply-tag set** already used by migration, caravans and embargoes. The other three are mapped *onto* it by one data file. No vocabulary is deleted.

---

# PART II — THE STORY

## 3. The shape of the thing: the Regional Pulse

The Pulse is a **reader**. Once per day it reads the five existing clocks and derives, for each of the twelve settlements, a **Condition**. It stores only what cannot be re-derived: how long a settlement has sat in its band, how many people it has lost or taken in, and the ledger of refugee waves.

### 3.1 The five ladders

Every settlement is on one of five rungs. A rung changes only after **five consecutive days** beyond the threshold (hysteresis — a bad Tuesday is not a collapse).

| Rung | What it means in play | Trade | Gate |
|---|---|---|---|
| **Steady** | Authored baseline. | Authored prices and stock. | Authored standing gate. |
| **Strained** | One pillar failing (food, safety, road, sickness). | Stock thins; prices climb on the failing category only. | Wary. Wants a reason. |
| **Failing** | Two pillars failing or one for 15+ days. | Buys before it sells. Sells what it should keep. | Turns away outsiders; begins to send people out. |
| **Emptied** | Population under the floor. | Shuttered. A shell with a name. | No one to ask. Salvage rights open. |
| **Swollen** | Took in more than it feeds. | Every price high; labour cheap. | Takes anyone who works. |

*Thriving* is deliberately absent. Nobody in the Year of Ash is thriving; the best rung is *Steady*.

### 3.2 The four pillars

| Pillar | Read from | Fails when |
|---|---|---|
| **Food & water** | scarcity deltas (F2), embargo state (F10) | the settlement's goods run short for its region and no caravan origin buffers them |
| **Safety** | `activeWarTension`, faction control %, settlement `threat_level` | tension high in the settlement's faction orbit; owner flipped |
| **Road** | route availability, embargoes, caravan blockage | the region's routes are blocked for ≥5 days |
| **Health** | *The Plague Year* outbreak watch (if present) | an outbreak is declared in the region |

Health is a **soft input**: absent the Plague Year catalog it reads neutral (ship-dark).

### 3.3 Three grades of news

| Grade | Source | Delay | Reliability |
|---|---|---|---|
| **Heard** | radio, market rumour | 0–2 days | may mislabel the rung by one step |
| **Told** | a caravan or trader who came through | route travel time | correct at the time of leaving |
| **Seen** | the player, or a survivor on a sortie, is there | none | exact |

The Board shows the best grade on hand, with its age. State never changes; only what the player is allowed to know.

## 4. The twelve settlements

Each entry: the rung ladder in one line each, the pillar most likely to break, and one thing the settlement will do that it did not do before.

1. **Brine-Pan Hollow** (Salt Camp, the Toll → `settlement`/`deep_coast`). *Steady:* the pans smoke and the preserve sells. *Strained:* rain in the pans; salt runs thin. *Failing:* boiling stops, salt is hoarded for the sick. *Emptied:* pans crusted, boilers stripped by the next camp. *Swollen:* strangers at the pans, boiling round the clock. **Breaks on:** Road. **New:** stops selling salt to the shelter unless the shelter has sold it something first.
2. **Iron Siding** (Rail Siding Town, industrial belt). Breaks on Safety. **New:** trains a militia when tension rises and does not disband it.
3. **Cape Beacon** (Lighthouse Commune, coastal shelf). Breaks on Health (the coast gets everything first). **New:** closes the lamp — and with it the only reliable sea-mark — when it quarantines. *(Hook: The Drowned Coast.)*
4. **Slate Hollow** (Quarry Enclave, high scarp). Breaks on Food. **New:** trades stone for grain at a rate that tells you how hungry it is.
5. **Pilgrim Hearth** (Monastic Sanctuary, high scarp). Breaks on Safety, slowly. **New:** the only settlement that takes refugees at no price, and the first to run out of everything because of it.
6. **Tinker's Notch** (Free Trader Scrap Market, dead suburbs). Breaks on Road. **New:** the price board reflects the war within two days, both ways.
7. **Ferry Crossing** (Trade Post, the Drown). Breaks on Road/Health. **New:** the ferry stops when the river does; it is where a quarantine is first felt. *(Hooks: Plague Year, Drowned Coast.)*
8. **Nine Rails** (Trade Post, industrial belt). Breaks on Safety. **New:** weighbridge tolls follow whichever faction is dominant.
9. **Fort Karkov** (Faction Stronghold, high scarp). Breaks on Food under siege. **New:** the only one that can be *Steady* while everything around it is *Failing* — and everyone knows why.
10. **Lock Seven** (Faction Stronghold, the Toll). Breaks on Road. **New:** the lock is a valve; which way it is open is a political statement.
11. **Silo Burrow** (Refugee Camp, the Verge). Built to fail. **New:** receives the waves; its rung is the honest measure of the region's war.
12. **St. Nicholas** (Ideological Community, the Cluster). Breaks on Health. **New:** refuses to accept anyone's account of an outbreak and pays for it.

## 5. Refugee waves

A wave is a **fact with a cause**, not random weather.

- **Cause kinds** (authored): *war front moves* (tension crossing 70, or a war-chain outcome), *road cut* (embargo ≥5 days), *hunger* (Failing ≥15 days), *sickness* (outbreak declared, Plague Year), *flooded out* (Drowned Coast).
- **A wave** = `{ cause, from region, to region, size, start, days on the road }`. The size is taken **from the origin's population**, so the origin gets emptier by exactly what the destination gets fuller by. Population is conserved.
- **On the road** the column can be met (an outpost, a waystation, a sortie): a real encounter using the existing expedition/encounter authorities. Nothing new is invented for it.
- **At the shelter's gate**, a wave of size *N* arrives as at most a few **petitions per day** — a family, a foreman with nine, three brothers. Each is a decision: **open** (they join; feed them), **ration** (a place for some), **refuse**, or **redirect** to Silo Burrow (costs a guide and a day). The shelter's stores decide what is possible, not the story.
- **Memory.** Who the shelter turned away is remembered by the settlement they reach, and by the standing record.

### 5.1 Sample lines (voice lock)

- *Radio, Heard:* "Toll road's closed past the second lock. Somebody says eighty. Somebody else says the number keeps going."
- *Trader, Told:* "Left Brine-Pan on the ninth. They were boiling for the sick and nobody else. Take cash, not salt."
- *Gate, Seen:* "Eleven of them. The oldest has a child's coat over her arm and it is not for anyone in the group."
- *Journal:* "Silo Burrow is sleeping four to a bunk. They asked us for nothing. That is worse."

## 6. Prices that remember

No new price authority. When a settlement's rung changes, or a wave lands, the Pulse asks the market owner's **existing idempotent shock seam** for a bounded, decaying nudge on the failing *category* in that region — the same seam migration consequences use today. Nudges are bounded (proposed 800–1400 permille), stack additively before the existing [500, 2000] clamp, decay to exactly neutral, and carry an exactly-once key.

The atlas labels (`local_surplus / balanced / imported_scarce`) are shown *as the player knows them*: the label follows the news grade.

## 7. The war, settled differently in each storyline

The Reckoning is per-storyline (Year Two design). The Living Region reads only the **Chapter Profile's** declared war settlement — one of four closed values — as a baseline for tension and migration multipliers:

| War settlement | Regional feel |
|---|---|
| **Armistice** | tension decays; migration favours the towns; the Board gets quieter, and quiet is unsettling. |
| **Partition** | borders harden; embargoes become permanent-ish; refugees stop crossing. |
| **Siege** | one front stays hot; Fort Karkov steady, everything downwind failing. |
| **Ashes** | no dominant power; every settlement on its own clock; waves come from hunger, not soldiers. |

Legacy profiles read **Siege at current values** — i.e. the game as it is today.

## 8. The seasons of the board

The four migration phases become four reading tempos: **deep winter** (people cluster, food fails first), **thaw** (roads open and lie), **dry heat** (water, sickness), **ash winds** (roads shut, radios full). Each season has a radio-column register and a set of authored rumour lines.

## 9. Content plan (waves)

- **W1 — Ladders:** 12 settlements × 5 rungs of two lines each (radio + trader) = 120 lines; journal lines ×36.
- **W2 — Waves:** 5 cause kinds × 3 registers × 4 seasons of radio lines (60); 24 gate petitions (family, workers, defector, sick, collector).
- **W3 — Consequences:** settlement-remembered lines for opened/rationed/refused/redirected (48); Silo Burrow honesty ladder (10).
- **W4 — Rumours with wrong grades:** 30 lines where Heard is off by one rung, each with a correcting Told line later.

All lines pass the existing content validator and voice lock; no line names a real place or power.

## 10. Player experience in one week

Day 212: radio says the Toll road is cut. The player hears it. Day 214: a salt trader arrives with less salt and a price he is embarrassed by. Day 216: the trader says Brine-Pan is boiling only for its sick. Day 219: three families are at the gate; the shelter has nine days of food for its own. The player rations. Day 224: the player rides out; Brine-Pan is on its knees and someone recognizes the shelter's mark from the gate. Nothing in that week was scripted for the player; every step is the same five clocks, read together.

## 11. Non-goals (restated)

No new save section (state nests in the migration owner, DEC-LR-02). No new panel (DEC-LR-06). No second population, price, or ownership authority. No change to war outcomes. No change to Year One pacing when ship-dark.

## 12. Risks

- **The Pulse invents cause.** Mitigation: every rung change stores its *cause tag* from the input that tripped it; the Board shows the cause.
- **Population conservation drift.** Mitigation: property test — total population weight is constant across waves.
- **Region-map mistakes** silently starve a settlement. Mitigation: integrity validator requires every settlement and every price region to resolve.
- **News grade leak** (view showing truth). Mitigation: presenter tests on the three grades.

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** A place becomes a place when something happens to it while you are looking
elsewhere. The graded news is the plan's epistemology — *Heard, Told, Seen*: three distances from
someone else's suffering, never closed, corrected late and slightly wrong, which is the most human
mechanic in the corpus. The world does not lie to you. It gets to you late.

**What the expansion leaves lying around.**

> "Bulletin, corner: Heard — Strained. Later, in a different pencil: not Strained. Both lines stay."

> "Wave tally: forty arrived. Conserved arithmetic. The names were never on it."

> "Gate petition, folded twice. Four answers possible. The petition has already been read twice."

**Scenes the player may piece together.**

> "A settlement moves Strained → Swollen in one entry and the Board does not comment. The Board's job is rungs, not grief."

> "The *Seen* line is written by someone who walked through it. The *Seen* lines are short."

**Held silences (texture — the register below is unchanged).**

- What the four unmapped vocabularies called their places. One mapping file is added and nothing deleted; the old words survive untranslated and unpronounced.
- Whether the region ever looks back at the shelter. The Pulse's inputs are regional; the shelter is a gate and a market, never an input.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**Why there is no "Thriving" rung.** A design rule and a worldview at the same time. Adding one
would be a tone change, not a feature.

**Where redirected refugees go.** The ledger ends at the gate and *conserves* population without
following anyone. Conservation is the point.

**What the four unmapped region vocabularies were for.** One mapping file is added and **nothing is
deleted**. The old vocabularies survive untranslated.

**Whether *Emptied* is a state or a verdict.** The rung derives from inputs. Whether anyone in the
fiction uses the word as a judgement is not authored.

**Why *Heard* is wrong by exactly one.** The bias is authored as a mechanism, never as a character
flaw in whoever is reporting.

**Whether the Pulse knows about the shelter.** Its inputs are regional. The shelter is a gate and a
market, never an input.
