# ASHFALL — Expansion Design Bible

**Title:** ASHFALL: NOBODY'S CHARTER
**Internal id:** `expansion_nobodys_charter`
**Status:** Design bible for review. No game data has been edited. No C#.
**All new ids below are PROPOSED** unless marked *existing*.
**Tone lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.
**Sister packs:** Expansion 1 is `expansion_the_holdfast`. Expansion 2 is `expansion_the_duty_roster`. This pack requires neither — it reads their flags if present (Appendix A) and stands alone if not.

---

# ANALYSIS PHASE

## 1. Strengths and gaps after Holdfast and Duty Roster

### What the first two packs already spend

- **The allocated world** (Holdfast): Cluster 7, the Ice Road, the plant, the Office, Reconstruction Order 12-C, the hatch reversed. Order arriving with paperwork and calling it a levy.
- **The unlisted home** (Duty Roster): the chart, the ladle, the stool, the tin, the quieter room. Order refused to fill in a blank space, and what filling it in costs.
- Both packs are about **published fairness** — a formula, a rubric, a Schedule, a census. Both endings tables say the same thing in different rooms: *a rule that was written down in advance is still a selection.*
- Both packs keep the map closed. Four Powers. No fifth. New people are **Currents** (`currents.json`) or named individuals, never a seventh row in `faction_lore.json`.
- Fourteen Currents now sit in `currents.json` with fully authored `wants` / `offers` / `signature_quote` / `access_rule` fields — most of them (`faction_the_tally`, `faction_undertow`, `faction_grain_exchange`, `faction_scavenger_guild`, `faction_cold_count`, `faction_deserter_coalition`, `faction_iron_raiders`, `faction_the_provisioned`) are `"is_active": false` and have never anchored a quest. That is real, spendable design material and this bible cameos several of them at The Crossing's edges — but does not centre on them. (Considered and set aside below, §4.)

### What is still a hole in the hole

- **Every dispute in this game has an adjudicator.** The Office files. The Garrison enforces. The Rebuilders keep minutes. The Quiet House has a rule that never bends. Sole corroborates. Even the Currents have a fixed, unilateral `access_rule` — you meet their price or you don't, and there is no discussion. **Nothing in ASHFALL has ever modelled a rule that is only as real as who is currently willing to enforce it.** That is a distinct mechanic from a hegemony bar or a trust float, and it is the one this bible is built to hold.
- **"Major faction quests" have so far meant Powers you already know, or Currents you meet once.** No pack has built a faction *from nothing* whose whole reason to exist is that no existing authority reaches it — and no pack has asked the player to actually choose, with lasting consequence, which flavour of authority replaces the vacuum.
- **Branching so far is binary-plus-a-silent-option**: honour/refuse/substitute (Holdfast), ink/pencil/blank/burn (Duty Roster). Nothing formalises a *third, active* betrayal — taking a task, banking the trust it buys, and spending that trust somewhere the asker never agreed to. The user asked for exactly this shape and no existing quest table names it as a repeatable pattern. This bible makes it the spine (§4.0).
- **Shelter-door content is currently owned by Duty Roster's `ShelterEncounterSystem` / `MoraleMarkSystem`**, both PROPOSED, neither yet a second full system elsewhere. This pack hooks them rather than re-inventing a hatch.
- **Highway 9 already exists in code** (`Mutation_Highway9Cleared`, `Quest_IronLedger`, `Location_TollHouse`, `Location_CheckpointKiloMemorial`, `Location_QuarantineMile` — real files, Warlord territory) and nothing has ever used the fact that a road with checkpoints implies a place *without* one.

### Weaknesses this pack must not pretend away

- The territorial map is **closed**. Four Powers. No fifth. This pack's new geography and new factions must not become a fifth Power under a friendlier name — see §5 for the compliance argument.
- Two faction id namespaces (`iron_garrison`/`faction_central_garrison`, etc.) remain a live defect. This pack does not touch it and does not add a namespace of its own beyond one small, self-contained catalog.
- Companions are named survivors on Utility AI, not a combat party. "Bosses" are crises, not arenas.
- `Victory_TrueEnding` terraformers, Tessarat, Sector 7G, androids, neuromancers: unused, as always.
- Hatch-dilemma magnitudes (`LetThemInContaminationRadsPerHour = 50`, `ForceDeconContaminationRadsPerHour = 10`, `DenyEntryMoralePenaltyForOtherSurvivors = 20`) are **untouched**. Prompt #26 if anyone tries.

## 2. Top three opportunities

| # | Opportunity | Why it is the largest lever |
|---|---|---|
| 1 | **A rule with no permanent author.** No system in this game has modelled law as a standing consensus instead of a document or a hierarchy. It is a genuinely new shape, it is cheap (a flag + a backers list, not a physics sim), and it is the literal, mechanical answer to "quests that alter the world depending on how they were completed, failed, or double-crossed" — because a ruling that only holds while backed is a ruling a double-cross can *actually overturn*, on screen, not just in flavour text. |
| 2 | **A gate that is social, not seasonal.** Holdfast gates on weather (Ice Road). Duty Roster gates on nothing (zero travel). A gate that opens because someone vouches for you, and closes because you spent that trust badly, is unclaimed narrative territory and reuses nothing but a flag and an NPC relationship. |
| 3 | **The founding-document mystery as a small, honest "List."** `02_THE_LIST.md`'s whole method — a real document, a story people built on top of it that the document does not actually support — has never been run a second time at a smaller scale. A three-page pre-war weigh-scale compact, inflated by five years of telling, is the same trick played quietly, and it gives this pack its title a literal, findable, disappointing, true answer instead of a rhetorical one. |

## 3. Critical gaps and assumptions

| Gap | Assumption used in this bible |
|---|---|
| No location in the base gazetteer sits outside all four Powers' patrol radius | The Crossing occupies the seam where the Toll (Highway 9, Warlord territory) meets the unclaimed Drown — an old rail/road interchange the Warlords stopped maintaining because the viaduct into it is unsafe for vehicles. Foot traffic only. This is a gap in patrol *capability*, not a gap the lore has to invent from nothing. |
| `faction_lore.json` is at six entries and the Currents DTO has no `relationships` field | The Crossing's three blocs live in a **new** catalog, `crossing_factions.json`, Currents-shaped (`id, display_name, alignment, home_region, wants, offers, signature_quote, access_rule`). Not a seventh Power. Not a Current either, technically — see §5 for why that distinction is written into the fiction, not hand-waved. |
| Fourteen existing Currents already have full `wants`/`offers`/`access_rule` data and zero quests | Cameo only: `faction_deserter_coalition` explains why some Compact refugees can't be named; `faction_scavenger_guild`'s claim-blacklist rule is the model the Scale's stall claims are visibly imitating; `faction_iron_raiders` is the answer to "what fills the vacuum if the player lets The Crossing collapse." None of the three gets a questline of its own in this pack — that is future-pack material, flagged, not spent here. |
| `WorldStateConsequenceSystem` has a fixed `_hegemony` set (four Powers + Rebuilders) | New mutations attach to a **market/route effect**, the same shape as `Mutation_MedicalSupplyGone` and `Mutation_Highway9Cleared` — a price and a travel number, not a new relationship row. |
| How does the player even hear about The Crossing? | Ostrowski is the default first vouch (he already sells maps and knows things he won't source). If Holdfast or Duty Roster are live, alternate vouch paths open (Appendix A). |
| Is there a real founding document? | Yes. Three pages. A 1962-style interstate weigh-scale calibration and toll-revenue-sharing compact between two county highway authorities. It says nothing about courts, votes, or sovereignty. It is completely real and almost completely beside the point, which is the point. |
| Combat model | Same as always: expedition-tick + stance + Utility AI + resource spend. The Underwrite's "muscle" is a cost and a risk, not a boss. |

## 4. Three expansion concepts (brief)

**A. NOBODY'S CHARTER — a self-governed interchange** *(chosen)*
A pre-war customs depot at the Toll/Drown seam, held by none of the four Powers because holding it was never worth the men. Five years of ad-hoc rulings have hardened into three competing shapes of order — procedural, transactional, aspirational — none of them law, all of them acting like it. The player's shelter needs a market no Power prices, or an appeal no Power will hear, and the road in runs through whoever will vouch for them.

**B. THE CURRENTS, NAMED — activate the dormant factions**
Give The Tally, Undertow, Grain Exchange, Cold Count, and Scavenger Guild full major-faction questlines on their existing `wants`/`offers`/`access_rule` data, no new geography. Strong content, genuinely the cheapest path to "major faction quests," and it was this bible's first-choice recommendation before design direction was set. Not built here by explicit choice — reserved, with hooks left in place (§4.2 Compact-aligned side quests, Section 10) for a future pack that wants exactly this shape.

**C. ONE POWER, ONE WAR**
Escalate Rebuilders vs. Warlords, or crack the Cult of the Ash Sign open, into a full internal-conflict questline with a defeat/broker-peace/betray-to-a-rival structure. Strong stakes, but it risks retuning `_hegemony` weights that three other packs already balance against, and it makes an existing Power the centre of attention rather than filling an actual gap. Rejected: this bible's job is to spend what is unclaimed, not re-litigate what four packs' worth of hegemony math already depends on.

## 5. Choice and why

**Proceeding with A — NOBODY'S CHARTER.**

It is the only concept that (1) respects the closed map by making the new authority's *illegitimacy* the plot rather than a loophole, (2) gives "major faction quests" and "world-altering, completable/failable/double-crossable" content a genuinely new mechanical home (a ruling that only holds while backed) rather than reusing hegemony trust as a proxy, (3) needs no new victory path, no new affliction, and no seventh Codex Power, and (4) is small enough — one front, twelve POIs, three blocs, six NPCs — to stay a controlled, finishable pack rather than a second Holdfast.

The compliance argument, stated once and held to for the rest of this document: **the Powers hold ground. The Crossing's three blocs do not hold ground — they hold a ruling, for as long as three people keep backing it, and not one day longer.** That is not a fifth Power in a different shirt. It is the thing a Power is not.

---

# SECTION 1 — EXPANSION OVERVIEW

| Field | Value |
|---|---|
| **Title** | ASHFALL: NOBODY'S CHARTER |
| **id** | `expansion_nobodys_charter` |
| **Hook** | A place none of the four Powers ever thought was worth holding has spent five years governing itself out of habit, favour, and debt. It works. It is not going to keep working by accident forever, and the player just walked into the room where that gets decided. |
| **Tagline (UI, not marketing-speak)** | *Nobody wrote the rules. Everybody's still keeping them. For now.* |
| **Genre lock** | Same game. 2D survival-**management**. Expeditions are node ticks. No 3D interchange, no dialogue-wheel courtroom sim, no co-op. |
| **Playtime (new content)** | **10–15 hours** for the main Crossing arc plus one bloc's full side catalog on a mid-game save; **16–22 hours** completionist (all three blocs, Charter mystery, all repeatables). |
| **Scale honesty** | Smaller than Holdfast on purpose: one front, not four sub-regions. Twelve new POIs, three new factions in one small catalog, six NPCs, 3 new systems (cap held), 10 main quests, 18 side quests, 26 morale micro-choices, 14 shelter/gate encounters. |
| **Progression gate (soft)** | Day **70+**, shelter can field a 2–3 person expedition of 6+ hours, at least one tradeable surplus good (any of: brass, iodine, seed stock, salvage). |
| **Progression gate (story)** | A grievance a Power won't hear: any one of `Mutation_TransitTax`, `Mutation_MedicalSupplyGone`, a Cult tithe demand, a Holdfast levy dispute, or a Duty Roster roster conflict **or** simply Ostrowski trust ≥ 20 and the rumour has been heard twice. |
| **Progression gate (hard ending)** | Day 150+ **and** at least two of the three blocs' main-quest chains resolved (any resolution shape — Complete, Fail, or Double-Cross all count) **and** the Charter found (`quest_crossing_three_dry_pages`). |
| **Does not require** | Holdfast or Duty Roster unlocked. If neither is live, Ostrowski is the sole vouch path and the Approach/hatch content stands alone on *existing* hatch mechanics. If either is live, every main quest reads their flags (Appendix A). |
| **Does not add** | A seventh `faction_lore.json` row. A new `WorldStateConsequenceSystem._hegemony` entry. A 16th unrelated `Victory_*.cs` (optional epilogue flag only, `victory_nobodys_charter`). New hatch-dilemma magnitudes. A fifth Sector 4 Power under any name. |

### One-paragraph pitch

The depot was built to settle an argument between two county highway authorities about who paid for the scale. It settled it. Nobody has settled anything at the Crossing since, and that turns out to be almost the same as settling everything — because the alternative to a government is not chaos, it's five years of people who have to eat tomorrow agreeing, over and over, in public, on who gets to be right today. The Scale keeps the weights honest because a dishonest scale empties the stalls in a season. The Underwrite covers your losses because covering losses is how you get to be owed. The Compact wants to write it all down before it curdles into either of the other two permanently, and they might be right, and the rubric they've drafted has the same shape as the one that decided who got a shelter. Somebody is going to end up holding the ledger. The game will not tell you who should.

### Integration strategy

| Layer | How it attaches |
|---|---|
| **Map** | One new front, `region_crossing` tag, three POI clusters (12 total), reached via `loc_crossing_viaduct_gate` off the existing Toll/Drown seam. No fourth sub-region added to the gazetteer; The Crossing is described as sitting *between* the Toll and the Drown, claimed by neither. |
| **Travel** | 3.5–6.5 hrs from the player's bunker, danger 4–7. Single-front, no seasonal gate. The gate is social: `VouchAccessSystem`. |
| **Economy** | No new currency. The Crossing buys and sells at rates no Power's hegemony math touches — a genuine price-discovery market. Hooks `DynamicEconomySystem`. Debt is tracked in-fiction via `LedgerDebtSystem`, not a second currency. |
| **Lore** | New `world_history` entries under `ashfall`, `discovery_location_id` = Crossing POIs. Resolves (quietly, not triumphantly) the question the title asks: was there ever a real charter. Does not touch The List, the Schedule, or either sister pack's canon. |
| **Factions** | New catalog `crossing_factions.json`: `faction_the_scale`, `faction_the_underwrite`, `faction_the_compact`. **Not** added to `faction_lore.json`. **Not** added to `_hegemony`. |
| **Consequences** | New `WorldStateConsequenceSystem` mutations, market/route-shaped like `Mutation_MedicalSupplyGone` and `Mutation_Highway9Cleared` — never a new relationship row. |
| **Save** | One flag `exp_nobodys_charter_unlocked` + `CrossingArbitrationState` + `LedgerDebtState` + vouch/reputation flags. Old saves load; the viaduct gate is just a location description until the rumour quest starts. |
| **UI** | Lore Codex tab "The Crossing" (or folded into an existing "Currents" tab if one exists at implementation time). Standing rulings shown as a diegetic notice-board list, not a reputation bar. Debt shown as the literal contract text, re-readable on demand. |

### What the player is managing at The Crossing

The same seven needs. The weight that shifts is **trust as a spendable, revocable resource** rather than a passive score.

| Need | How the Crossing bites |
|---|---|
| Hunger / Thirst | Genuinely fair trade rates — the entire reason to make the trip — but only while the Scale is honest and the road stays walkable |
| Fatigue | No shelter of your own here until earned; every trip is a there-and-back or a favour for a bed at the Annex |
| Warmth | No shelter degradation system of its own — home bunker still ticks while you're away, same as every other pack |
| Radiation | Moderate, industrial (rads 18–30 across the front) — this is a working town, not a hot ruin |
| Morale | Watching a ruling you backed get overturned, or backing one you didn't believe in, is the marks catalogue's engine (§4.3) |
| Health | The Lockup and the outfall-adjacent chores at the Scalehouse carry ordinary injury risk, nothing new |
| Shelter | Untouched. This pack does not add a second base to manage — the player's shelter stays the only shelter |

---

# SECTION 2 — THE CROSSING

**The Crossing** (what everyone calls it) / **Interchange 6** (the stencil still on the old scale-house roof) / **the depot** (what the Toll calls it, when the Toll bothers).

Held by: nobody, on purpose, the way the Drown holds nobody. Contested by three blocs who each act like law without being able to produce one. Visual DNA matches the rest of Sector 4 — dry-gouache, ash-grey, concrete, rust, terminal amber — with one addition: **hand-lettering.** Every sign here was repainted by someone with an opinion. No two signs agree on spelling. That is the whole visual thesis: nothing here was issued.

Travel banding from the player's bunker (Grid/Verge seam):

| Cluster | `travelHours` | Danger | Signature detail |
|---|---:|---:|---|
| The Scalehouse Row | 3.5–4.5 | 4–5 | The truck scale, the market, honest weights (for now) |
| The Underwrite's Quarter | 4.5–5.0 | 5–6 | Contracts, collateral, the fire where deals actually get made |
| The Compact's Camp | 4.0–4.5 | 4–5 | The petition tent, the Annex, the founding myth |

**Entry:** `loc_crossing_viaduct_gate` is the only way in. No vehicle has crossed it since the Warlords stopped grading the approach road, four years ago (per Toll patrol logs, not enforced doctrine — nobody rescinded access, nobody renewed it either). First entry requires `VouchAccessSystem` — see §5.2.

---

## 2.1 The Scalehouse Row

**id prefix:** `loc_crossing_*` (this cluster: gate, scalehouse, stallrow, watchtower)
**Visual:** A concrete weighbridge built for trucks that mostly don't exist anymore. A market grown up around it the way grass grows through a parking lot — not planned, just persistent. Hand-lettered stall signs, none of them matching, all of them legible.
**Lore:** Pre-war, this was a state-line weigh station: axle loads, fuel tax stamps, agricultural quota checks. The Scale's authority is not invented — it is the last un-repealed part of a real, boring, pre-war job, performed continuously by whoever inherited the scale-house key.
**Unique mechanic:** `VouchAccessSystem` entry gate at the viaduct. First-weigh ritual at the Scalehouse establishes trade access.
**Who you meet:** Osran Kell; stallholders (backers pool for `CrossingArbitrationSystem`); Watchtower lookouts.

### POIs (4)

| id | Name | d | hrs | rads | Hook |
|---|---|--:|--:|--:|---|
| `loc_crossing_viaduct_gate` | The Viaduct Gate | 5 | 3.5 | 18 | A rail truss over the Drown's edge, planked for foot traffic only. A sign, hand-repainted so many times the paint has texture: NO CHARTER NO GUARD ASK FOR SOMEONE. |
| `loc_crossing_scalehouse` | The Scalehouse | 4 | 4.0 | 20 | The actual truck scale, calibrated by a weight nobody has seen in years (Osran swears it's still true). First-weigh ritual: your goods, and by implication you, get a number. |
| `loc_crossing_stallrow` | Stallrow | 4 | 4.0 | 20 | The market. Two dozen stalls, claim-marked in chalk the way the Scavenger Guild marks a site — an imitation nobody at Stallrow will admit to. |
| `loc_crossing_watchtower` | The Watchtower | 5 | 4.5 | 24 | Pre-war inspection tower, the one building here taller than two storeys. Sightline over all three clusters. The Scale's only real muscle is knowing who's coming. |

**Map note:** Hub-and-spoke off the gate. The most "orderly" cluster — and the one where the Standing ruling table (§5.1) is most often invoked, because Stallrow is where disputes actually happen.

---

## 2.2 The Underwrite's Quarter

**id prefix:** `loc_crossing_underwrite_*`, plus `loc_crossing_the_lockup`, `loc_crossing_granary_pledge`, `loc_crossing_nightfire`
**Visual:** Warmer light than the Scalehouse Row — the Underwrite pays for lamp oil out of interest, and it shows. A hall with a long table. A fire that never quite goes out. A granary with a padlock that is mostly theatre; everyone here already knows what's owed.
**Lore:** When the depot's first winter alone turned out to be survivable but not comfortable, somebody started fronting seed and fuel against a promise, in public, with witnesses, so the promise couldn't quietly stop existing. It worked. It is still working. It has also, unavoidably, become the only insurance anyone here has, which means it is also the only entity here that can genuinely ruin you.
**Unique mechanic:** `LedgerDebtSystem` — contracts read twice, forfeits named up front, never a hidden clause.
**Who you meet:** Dessa Vane; Wyn Sabler (lives at the Annex but her pledge is here); collectors.

### POIs (4)

| id | Name | d | hrs | rads | Hook |
|---|---|--:|--:|--:|---|
| `loc_crossing_underwrite_hall` | The Underwrite Hall | 5 | 4.5 | 22 | A long table, a ledger chained to it (not for security — so it's never "misplaced"). Dessa's seat is the one with the good light. |
| `loc_crossing_the_lockup` | The Lockup | 6 | 5.0 | 26 | Collateral storage. Tools, livestock, once — the story goes — a set of teeth in a jar, never explained, never asked about twice. |
| `loc_crossing_granary_pledge` | The Pledged Granary | 5 | 4.5 | 22 | Wyn Sabler's grain, mortgaged against a debt that's coming due. Visible, countable, exactly as much as it looks like. |
| `loc_crossing_nightfire` | The Nightfire | 6 | 5.0 | 24 | Where deals actually happen, after the Scalehouse closes. No one has ever formally claimed it belongs to the Underwrite. Nobody else sits there either. |

**Map note:** The Underwrite's power is entirely social and financial — no armed compound, no wall. The Lockup is the closest thing to a threat, and it is a threat made of paperwork more than muscle.

---

## 2.3 The Compact's Camp

**id prefix:** `loc_crossing_petition_tent`, `loc_crossing_founders_marker`, `loc_crossing_the_annex`, `loc_crossing_records_room`
**Visual:** Newest structures on the front — a canvas-and-scavenge camp that's trying hard to look permanent before it is. A hand-painted sign that says DRAFT 4 with the 4 still wet some days.
**Lore:** The Compact are mostly people the other two blocs' arrangements have already failed once — a debtor who paid and still lost the season, a Scale claim that got overruled twice in one year, refugees the Deserter Coalition couldn't keep moving forever. They want the thing everyone else here is quietly relieved doesn't exist: a rule that doesn't need anyone's continued goodwill to keep holding.
**Unique mechanic:** none of its own — the Compact's questline runs entirely through `CrossingArbitrationSystem` (trying to make a Standing ruling permanent) and the Charter mystery.
**Who you meet:** Perrin Ashby; Wyn Sabler (residence); Ivo Fenn (records room, technically Compact-adjacent — he answers to no one, but they're the only bloc that visits him without wanting something first).

### POIs (4)

| id | Name | d | hrs | rads | Hook |
|---|---|--:|--:|--:|---|
| `loc_crossing_petition_tent` | The Petition Tent | 4 | 4.0 | 20 | Perrin's drafting table. A charter in progress, rewritten enough times the margins are a second document. |
| `loc_crossing_founders_marker` | The Founders' Marker | 5 | 4.5 | 24 | A plaque, pre-war, corroded past the third line. What it's believed to say and what it says are already two different things before you even find the real document. |
| `loc_crossing_the_annex` | The Annex | 5 | 4.5 | 22 | Refugee housing the Compact runs on favours and Wyn's grain. Warmest room at The Crossing in every sense but temperature. |
| `loc_crossing_records_room` | The Records Room | 6 | 5.5 | 30 | Ivo Fenn's. The depot's actual pre-war filing, mostly intact, entirely unread by anyone with a reason to read it until now. |

**Map note:** The endgame lobe. The Charter is found here (`quest_crossing_three_dry_pages`); what the player does with it is the closest thing this pack has to a final choice.

---

## 2.4 Existing Sector 4 nodes that gain meaning (not new geography)

When `exp_nobodys_charter_unlocked`:

- `loc_weighbridge` — Bram's cousin-trade. He'll compare his weights to the Scalehouse's, unprompted, and be right to within a pound. He will not say how he knows.
- `location_abandoned_convoy_yard` / `loc_diesel_tank_farm` / `loc_recovery_yard` — waypoints on the walk in. Description overlay only: the road to the Crossing runs past here, and someone has left chalk marks matching Stallrow's claim-marks on the fence.
- `loc_conscription_office` — Pell knows the Crossing exists and has decided, in writing, not to know it. If pressed, he'll say a place with no charter isn't a place his quota reaches.
- `loc_low_background_lab` — Cold Count (*existing Current*, cameo only) can date the Founders' Marker's corrosion and, later, verify the Charter's paper stock is genuinely pre-war. One line of dialogue, no questline spent.
- `location_the_memory_vault` — Sole can cross-reference the Charter against Continuity's own records, once found, the same corroboration rule as always. She will not be surprised by what it says. She will be surprised anyone thought it said more.

---

# SECTION 3 — MAIN STORYLINE

## Central conflict

**Three people are each partly right about what The Crossing is, and none of them can prove it, because there was never a document that said.**

Osran Kell says it's a scale-house that grew a market, and a scale-house needs a weighmaster more than it needs a government.
Dessa Vane says it's an economy that would collapse into raiding without someone willing to be owed, and that person doesn't need to be liked, just paid.
Perrin Ashby says none of that is a reason it can't also be a town, with a vote, and a code, and an actual appeal instead of whoever can find three backers fastest.

The player is the only person here with no stake in which of them is right — which means the player is the only person who can spend a season finding out, and the only person whose answer will actually stick, because everyone else has already spent their credibility taking a side.

## Theme (unspoken)

A rule nobody enforces is a suggestion. A rule everybody enforces is a government. This place has spent five years being neither, on purpose, and calling the gap between them freedom. It is freedom, for exactly as long as nobody needs it enforced against them personally.

## Principal NPCs (6)

### 1. `npc_osran_kell` — Weighmaster Osran Kell *(companion)*

- **Where:** `loc_crossing_scalehouse`
- **Was:** State highway-authority scale inspector, pre-war. The only person here whose pre-war job and current job are, functionally, the same job.
- **Wants:** The scale kept honest. Stallrow fed. To not be the government, loudly and often, while doing most of the things a government does.
- **Will not:** Rig a weight. Take a side in the Underwrite/Compact argument on the record. Let anyone call the Scalehouse "his."
- **Voice:** Numbers first, opinions never volunteered. Answers a different question than the one asked, accurately.
- **Snippet:**
  > "I don't run this place. I run a scale. It happens that an honest scale is most of what a place like this needs, and it happens that I'm the one who kept it honest, and I understand why those two facts look like a throne from where you're standing. Get closer. It's a folding chair."

### 2. `npc_dessa_vane` — Dessa Vane *(companion)*

- **Where:** `loc_crossing_underwrite_hall`
- **Was:** Nothing anyone can confirm. She was here by year one. That is the whole of her public biography and she has never corrected it.
- **Wants:** Contracts honoured. A Crossing that still needs the Underwrite next winter. Perrin's charter to fail specifically at the clause that would make lending regulated.
- **Will not:** Lie about a term. Forgive a debt for sentiment on the record (she has, off the record, exactly once — see `quest_crossing_companion_dessa`). Let a forfeit go uncollected in front of witnesses; the collecting is the product.
- **Voice:** Reads terms aloud, twice, unhurried, the same cadence for a debt of ten rounds or a debt of a person's labour.
- **Snippet:**
  > "You've heard it. Do you want it read again? Most people want it read again. Not because they missed something. Because hearing it twice is the last moment it still feels like a choice."

### 3. `npc_perrin_ashby` — Perrin Ashby *(companion)*

- **Where:** `loc_crossing_petition_tent`
- **Was:** Nothing that qualifies as a trade — which is, Osran notes, the closest thing to a joke he tells about anyone.
- **Wants:** A written charter, voted, binding, that survives the people currently enforcing it by habit. To be the person who fixed the thing everyone else profits from leaving broken.
- **Will not:** Write a clause they know is unfair to get it passed faster. Admit the draft's scoring system for "who gets a vote" resembles a rubric anyone here would recognise, until the player makes them look at it.
- **Voice:** Earnest, precise, slightly too fond of the word "finally."
- **Snippet:**
  > "You think I don't know what it looks like. A list, a score, a line that decides who counts. I know exactly what it looks like. I also know the alternative is Dessa's ledger and Osran's folding chair, forever, because neither of them will ever write it down where it can be argued with. Mine can be argued with. That's not nothing."

### 4. `npc_mattis_cray` — Mattis Cray *(companion)*

- **Where:** `loc_crossing_viaduct_gate`, then anywhere
- **Was:** Crossing-born — one of maybe six people here who never lived anywhere else. Runner, fixer, the person who actually walks new arrivals across the truss.
- **Wants:** The Crossing to still be a place you can walk into. To stop being the only vouch of last resort, which he has been for three years because saying no to a person freezing at the gate is a thing he has tried and cannot do again.
- **Will not:** Vouch for someone twice if the first vouch was spent badly. Pick a bloc — genuinely undecided, not performing neutrality.
- **Voice:** Fast, practical, the only person here who talks like the Verge instead of like a committee.
- **Snippet:**
  > "I'll vouch for you. That means if you burn it, it's my name that's ash, not yours — you get to just leave. Think about that before you decide the debt collector deserved it."

### 5. `npc_ivo_fenn` — Ivo Fenn

- **Where:** `loc_crossing_records_room`
- **Was:** Depot filing clerk, pre-war. Genuinely, boringly, the actual job.
- **Wants:** The records kept in order. Nothing else, and he means it — Perrin's petition and Dessa's ledger both irritate him for the same reason: neither is filed correctly.
- **Will not:** Destroy a record, for anyone, for any reason. Tell you what's in the Charter before you find it yourself — not from drama, from procedure: he doesn't summarise files, he produces them.
- **Voice:** Precise, unbothered by five years of collapse, the last civil servant on earth who still believes in his filing system.
- **Snippet:**
  > "People keep asking me what it says. I keep telling them: read it. Not because I'm protecting a secret. Because I have watched three people in a row hear my summary and build a religion on it, and the document itself has never once done that to anyone. Read it."

### 6. `npc_wyn_sabler` — Wyn Sabler

- **Where:** `loc_crossing_the_annex`, pledge at `loc_crossing_granary_pledge`
- **Was:** Farmer, upriver, before a bad season put her in the Underwrite's ledger for the first time. This is her second bad season.
- **Wants:** To keep the granary. Failing that, to not be the example everyone else at the Nightfire tells the story about.
- **Will not:** Ask the player to break the terms for her — she read them twice, same as everyone. She will accept it if the player breaks them anyway.
- **Voice:** Flat, arithmetic, no self-pity performed for an audience that's already decided what it thinks.
- **Snippet:**
  > "I'm not asking you to feel bad about it. I signed. I'd sign again — the alternative that year was starving with my name still my own. I'm telling you what's owed and what happens if it isn't, because Dessa will tell you the same thing and I'd rather you heard it from someone who isn't collecting."

---

## Story beats (10)

| # | Beat | Day / gate | What happens |
|---|---|---|---|
| 1 | **The Vouch** | Progression gate met | Ostrowski (or a sister-pack contact) names the Crossing and refuses to walk there. A name is needed. |
| 2 | **The First Weigh** | Vouch obtained | Mattis walks the player across the viaduct. Osran weighs the goods, and by extension, the shelter's word. |
| 3 | **The Terms** | First weigh done | Dessa offers a genuinely useful contract, read twice, forfeit named plainly. First live look at `LedgerDebtSystem`. |
| 4 | **The Petition** | First weigh done | Perrin asks the player to be an early signatory on the draft charter. First look at the rubric problem. |
| 5 | **The Standing** | Any bloc's opening quest done | The player witnesses or is drawn into a real dispute — a stall claim, a debt dispute, a petition objection — resolved by whoever gets three backers first. `CrossingArbitrationSystem` live. |
| 6 | **The Marker** | Standing witnessed | The Founders' Marker's corroded plaque and mismatched local legends about what founded the Crossing start the Charter mystery in earnest. |
| 7 | **The Forfeit** | Terms quest + Day 90ish | Wyn Sabler's pledge comes due. Major complete/fail/double-cross fork — see `quest_crossing_the_forfeit`. |
| 8 | **The Vote That Isn't** | Petition quest + at least one Standing resolved | Perrin attempts the Compact's first real ratification. Both other blocs interfere, differently. |
| 9 | **Three Dry Pages** | Marker + Records Room access | The actual Charter is found. Player decides what to do with the truth. |
| 10 | **Who Holds the Ledger** | Two of three bloc chains resolved + Charter found | Endgame. Whichever bloc (or none) is left standing becomes The Crossing's de facto law. World-state mutation fires. Ending flag set. |

## Branching choices (5)

| id | Choice | Immediate | Long |
|---|---|---|---|
| `crossing_vouch_spent_well` | Keep Mattis's vouch clean through the opening arc | Full access, standing invitations to all three blocs | Mattis stays a companion option indefinitely; his personal quest opens |
| `crossing_vouch_burned` | Betray whoever vouched for you (Mattis or a sister-pack contact) | Immediate access loss; must find a second, harder vouch | That NPC's trust does not recover in this pack; a colder Crossing for the rest of the playthrough |
| `crossing_forfeit_honoured` / `_defaulted` / `_doublecrossed` | Wyn's granary: help her pay, let it default, or help her flee with the grain before the Lockup can take it | Underwrite trust up / Underwrite trust down, Compact trust up / a genuinely new problem (Dessa now hunts a debtor who "disappeared" with the player's help) | Ripples directly into the endgame mutation table (§4.1, `quest_crossing_the_forfeit`) |
| `crossing_petition_signed_honest` / `_signed_rigged` | Support the Compact's charter as drafted, or quietly help Perrin rig the first vote to pass it | Compact trust up either way, but rigged support is discoverable later (Ivo's records don't lie) | Honest path: fragile but real ratification. Rigged path: the charter passes, and it is exactly as compromised as the rubric it resembles |
| `crossing_charter_revealed` / `_kept_quiet` / `_sold` | The three dry pages: publish the mundane truth, sit on it, or sell the revelation to whichever bloc benefits most from discrediting a rival | Deflates whichever myth needed the Charter most / preserves the status quo / actively weaponises the truth for one bloc against the others | Feeds directly into which ending slide plays (§ Endings) |

**Bloc chain definition** (referenced by the endgame gate above and by `quest_crossing_who_holds_the_ledger`'s prereqs): a bloc's chain counts as *resolved*, in any of the Three Shapes, once its pair of quests is done — Scale: `quest_crossing_first_weigh` + `quest_crossing_scale_integrity` (side); Underwrite: `quest_crossing_the_terms` + `quest_crossing_the_forfeit`; Compact: `quest_crossing_the_petition` + `quest_crossing_the_vote_that_isnt`. Scale's second quest is deliberately a side quest rather than a main-list crisis — procedural honesty doesn't escalate to a crisis the way a forfeit or a contested vote does, and that quiet is the point of the bloc.

## Endings (4 narrative + 1 fade)

All write a `world_history` second paragraph discoverable at `loc_crossing_records_room` or `loc_weighbridge`. The game does not rank them.

| id | Name | Condition | Slide (house voice) |
|---|---|---|---|
| `ending_crossing_scale` | **The Folding Chair** | Scale-aligned resolution dominant; Osran backed through the endgame Standing | The scale is still honest. Osran still says he doesn't run the place. Fewer people believe him than used to. |
| `ending_crossing_underwrite` | **Paid in Full** | Underwrite-aligned resolution dominant; forfeit collected or honoured on Dessa's terms | Everyone eats. Everyone owes. The Nightfire is warmer than it has any right to be, and nobody asks why the light costs what it costs. |
| `ending_crossing_compact` | **Draft Four, Signed** | Compact charter ratified (honest or rigged path both qualify, prose differs) | There is a document now. People argue with it instead of with each other, which Perrin considers a victory and does not examine too closely. |
| `ending_crossing_none` | **No One's** | Player double-crosses or guts all three blocs, or the Standing collapses without a majority resolution | The viaduct is still there. The market isn't. Word travels that a place with no charter now has no scale either, and the kind of people that word attracts do not ask to be vouched in. |
| `ending_crossing_walked` | **Just Passing Through** | Player completes the trade-access arc, avoids the endgame Standing entirely, never picks a side | The Crossing continues exactly as uneasily as it always has. The shelter got what it came for. Nobody there will remember the player's name in a year, which several people there would say is the whole point of the place. |

**TrueEnding terraformer / android / neuromancer content is not used.**

## Lore revelations (what standing there teaches)

1. The Crossing's authority was never granted by anyone who could grant it — it accreted, one honoured debt and one honest weight at a time, and it can un-accrete the same way.
2. The Charter is real, three pages, and is a weigh-scale calibration and toll-revenue-sharing compact between two county highway authorities, dated decades before the Exchange. It establishes nothing about self-governance. Everyone who has ever cited it as their authority was citing a document that never said what they needed it to say.
3. `Mutation_Highway9Cleared` and the Toll's checkpoints are the same doctrine of holding ground the Crossing was never worth applying to — the two are the same road, forty minutes apart, governed by opposite theories of what a road is for.
4. The Scavenger Guild's claim-blacklist rule (*existing Current*) is the model Stallrow's chalk marks are visibly copying, unofficially, because it is the only enforcement mechanism anyone here has ever seen work without a government behind it.
5. Wyn Sabler's forfeit and a Rebuilders brass demand and a Cluster levy (if Holdfast is live) are the same shape of obligation wearing three different institutional faces — a debt is a debt is a debt, whichever bloc is collecting it.
6. A ruling that needs three backers to hold is not weaker than a law. It is a law that has to keep being true to stay a law. That is either the safest kind of government or the least stable kind, and the game will not adjudicate which.

---

# SECTION 4 — QUEST DESIGN

Quest runtime: `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids` (*existing*). New ids registered at implementation. Types: `expedition`, `shelter`, `faction`, `personal`, `repeatable`.

## 4.0 The Three Shapes (structural device, read this before the tables)

Every main quest, and several side quests, resolves into one of three shapes. This is the pack's answer to "quests that alter the whole game world in a meaningful way depending on how the quest was completed or failed, or double crossed":

| Shape | What it means | What it costs | What it's worth |
|---|---|---|---|
| **Complete (Keep Faith)** | Do what was actually asked, honestly, at the real price named up front | Time, goods, sometimes a companion's disapproval | The asking bloc's trust rises; the ruling/contract/vote holds cleanly |
| **Fail / Refuse (Walk Away)** | Don't, or can't. No scheme, no cover story — the ask goes unmet | Whoever needed you absorbs the consequence themselves | No betrayal penalty, but no credit either; the world moves on without your hand in it, which is its own kind of mark |
| **Double-Cross (Sell It Elsewhere)** | Take the job, then spend the trust or access it bought you somewhere the asker never agreed to | The asker's trust breaks, often permanently, and the benefit lands with whoever you actually served | The largest single-quest world-state swing in the pack — and the only shape that can trigger `ending_crossing_none` on its own if stacked |

Every main-quest table row below ends in a **Resolution shapes** sub-table instead of a single "reward" line, in the format `Shape → mutation/flag → what a returning player sees`.

---

## 4.1 Main questline (10)

### `quest_crossing_the_vouch` — A Name at the Gate

| Field | Value |
|---|---|
| **Type** | expedition |
| **Prereqs** | Progression gate met |
| **Time** | 30–50 min |
| **Synopsis** | Ostrowski names the Crossing, sells a rough sketch of the approach, and will not walk there himself. "I sold them a map once. That was the whole transaction. I'd like it to stay that way." |
| **Objectives** | 1. Hear Ostrowski out. 2. Find a name willing to vouch — Ostrowski himself (reluctant, one-time only), or a sister-pack contact if Holdfast/Duty Roster live (Appendix A). 3. Walk the approach road, past the recast waypoints (§2.4). 4. Reach the viaduct. |
| **Rewards** | `item_vouch_token_crossing`; `knowledge_key: lore_nc_the_vouch` |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Vouch secured cleanly. `flag_crossing_vouched_clean`. Mattis meets you at the gate personally. |
| Fail/Refuse | No vouch found this trip. Quest stays open; returning to Ostrowski after a cooldown offers a second, colder chance. |
| Double-Cross | N/A — no one to betray yet. This quest has no double-cross shape; it's the only main quest that doesn't, by design (nothing has been trusted to you yet). |

---

### `quest_crossing_first_weigh` — What the Scale Says

| Field | Value |
|---|---|
| **Type** | expedition |
| **Prereqs** | Vouch obtained |
| **Time** | 30–45 min |
| **Synopsis** | Osran weighs your goods on the depot scale. The number is real. What people infer from it is not his problem. |
| **Objectives** | 1. Present goods for weighing. 2. Answer Osran's questions about the shelter (occupation-style, not a form — no paperwork changes hands). 3. Accept or contest the recorded weight. 4. Receive Stallrow trade access. |
| **Rewards** | Stallrow trade unlocked; `item_calibration_weight` (proof-of-honest-scale, quest key for `quest_crossing_scale_integrity`) |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Accept the true weight, even when it's less favourable than hoped. `mutation_crossing_honest_trader`. Osran remembers; later Standing rulings weight your backing slightly heavier. |
| Fail/Refuse | Contest a true weight for no real reason. Access still granted (Osran doesn't punish suspicion) but the exchange is noted; `mark_crossing_difficult`. |
| Double-Cross | Attempt to bribe Osran to misweigh. He refuses, on the record, in front of Stallrow. `mutation_crossing_bribe_attempted` — trade access still granted, but at a permanently worse rate, and the story reaches Dessa before you do. |

---

### `quest_crossing_the_terms` — Read It Again

| Field | Value |
|---|---|
| **Type** | faction |
| **Prereqs** | First weigh done |
| **Time** | 35–55 min |
| **Synopsis** | Dessa offers real help — seed stock, a covered loss, a favour bank — against a plainly named forfeit. |
| **Objectives** | 1. Hear the offer. 2. Have it read twice (mechanically: the choice UI shows the full contract text both times — no summarised version exists). 3. Sign, negotiate the term, or decline. 4. If signed: `LedgerDebtSystem` opens a `DebtContract`. |
| **Rewards** | Goods or covered loss per contract; `knowledge_key: lore_nc_read_again` |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Sign honestly, pay on schedule later. `mutation_crossing_underwrite_reliable`. Better terms offered on future contracts. |
| Fail/Refuse | Decline the offer entirely. No debt, no benefit. `flag_crossing_underwrite_untested` — Dessa neither trusts nor distrusts you; a blank slate. |
| Double-Cross | Sign, take the goods, then default deliberately and immediately sell the goods to the Compact or take them home without ever intending to pay. `mutation_crossing_underwrite_burned` — the Lockup opens a file on the shelter; future visits carry a collector escort. |

---

### `quest_crossing_the_petition` — Draft Four

| Field | Value |
|---|---|
| **Type** | faction |
| **Prereqs** | First weigh done |
| **Time** | 35–55 min |
| **Synopsis** | Perrin asks the player to be an early signatory on the Compact's draft charter — and, if the player reads closely, to notice its scoring clause for who gets a vote. |
| **Objectives** | 1. Read the draft. 2. Ask about the scoring clause, or don't. 3. Sign as-is, ask for a revision, or refuse to sign. 4. Optional: compare the clause aloud to a Holdfast RUR score or a Duty Roster occupation row, if either pack is live — Perrin has not made that connection yet and it visibly lands. |
| **Rewards** | `knowledge_key: lore_nc_the_rubric_again`; Compact trust |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Sign after a genuine revision pass — Perrin actually fixes the clause. `mutation_crossing_petition_revised`. Slower, better draft. |
| Fail/Refuse | Decline to sign. Petition proceeds without the player's name; Perrin is disappointed, not hostile. `flag_crossing_petition_unsigned`. |
| Double-Cross | Sign, then quietly show the unrevised draft's scoring clause to Osran or Dessa to undermine the Compact's credibility before the vote. `mutation_crossing_petition_leaked` — Perrin's trust breaks hard; the eventual vote (`quest_crossing_the_vote_that_isnt`) starts from a worse position no matter how it's later played. |

---

### `quest_crossing_the_standing` — Three Backers

| Field | Value |
|---|---|
| **Type** | shelter / faction |
| **Prereqs** | Any one opening quest done (weigh, terms, or petition) |
| **Time** | 45–75 min |
| **Synopsis** | A real dispute goes to the Standing — a stall claim, a debt argument, or a petition objection, chosen from whichever threads are live. The player learns `CrossingArbitrationSystem` by using it, not by reading a tutorial. |
| **Objectives** | 1. Hear both sides. 2. Recruit backers from the stallholder pool (favours, trades, or plain persuasion). 3. Call the Standing. 4. Watch the ruling hold, or get immediately contested by a rival backer count. |
| **Rewards** | `knowledge_key: lore_nc_the_standing`; reputation as a backer (used in `quest_crossing_who_holds_the_ledger`) |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Back the side you actually believe is right, win or lose fairly. `mutation_crossing_standing_honest`. Backers remember; easier to recruit next time. |
| Fail/Refuse | Decline to get involved. The dispute resolves without you; someone else's three backers decide it. No mark either way. |
| Double-Cross | Promise backing to both sides privately, then back whichever wins — or bribe backers directly, which several will accept and one (a `faction_deserter_coalition`-sheltering stallholder, cameo) will report to Perrin out of principle. `mutation_crossing_standing_rigged` — the ruling holds, but it is now common knowledge it was bought, which weakens every future ruling you back. |

---

### `quest_crossing_the_marker` — What the Plaque Doesn't Say

| Field | Value |
|---|---|
| **Type** | exploration |
| **Prereqs** | The Standing witnessed |
| **Time** | 30–50 min |
| **Synopsis** | The Founders' Marker is corroded past the third line. Three different people at the Nightfire will tell you three different things it says. None of them has actually read it. |
| **Objectives** | 1. Inspect the marker. 2. Collect the three competing local legends. 3. Optional: bring a Cold Count contact (*existing Current*, cameo) to date the corrosion. 4. Learn the records room exists and that Ivo Fenn keeps it. |
| **Rewards** | `knowledge_key: lore_nc_three_legends`; Records Room access flag |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Report the legends accurately, contradictions intact, to whoever asks. `mutation_crossing_legends_recorded` — no one's myth is disturbed yet. |
| Fail/Refuse | Don't investigate further. The Marker stays a rumour mill; `quest_crossing_three_dry_pages` remains locked until revisited. |
| Double-Cross | Tell one bloc a fabricated version of what the marker "really says," tailored to support their claim to legitimacy. `mutation_crossing_myth_seeded` — a new false legend enters circulation, discoverable and correctable later, at cost, in `quest_crossing_three_dry_pages`. |

---

### `quest_crossing_the_forfeit` — What Wyn Owes

| Field | Value |
|---|---|
| **Type** | faction / personal |
| **Prereqs** | Terms quest done (player need not have signed personally); Day ~90; Wyn's pledge term expires |
| **Time** | 60–100 min |
| **Synopsis** | Wyn Sabler's granary pledge comes due. She will not ask for help. Dessa will collect exactly what the contract says, no more, no less, in front of witnesses. |
| **Objectives** | 1. Learn the terms from Wyn directly (she reads them back herself, unprompted — she remembers every word). 2. Help her raise the grain to pay in full, broker a renegotiation with Dessa, or do neither. 3. If neither: witness or prevent the Lockup's collection. 4. Optional: help her disappear with the pledged grain before collection. |
| **Rewards** | Varies by shape; `knowledge_key: lore_nc_the_forfeit` |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Help Wyn pay in full, honestly, on the original terms. `mutation_crossing_forfeit_honoured`. Underwrite trust up; Wyn's personal trust up; Annex morale up. |
| Fail/Refuse | Do nothing. The Lockup collects the granary on schedule, calmly, without cruelty — which several onlookers find worse than cruelty would have been. `mutation_crossing_forfeit_defaulted`. Compact recruitment among Annex refugees rises sharply. |
| Double-Cross | Help Wyn flee with the pledged grain before collection — technically theft of collateral, dressed as compassion. `mutation_crossing_forfeit_doublecrossed`. Dessa does not forgive this; the Underwrite opens active pursuit of Wyn (a standing, low-grade threat on future Crossing visits) and the player's name is attached to the theft whether or not anyone can prove it. |

---

### `quest_crossing_the_vote_that_isnt` — Draft Four, Called

| Field | Value |
|---|---|
| **Type** | faction / crisis |
| **Prereqs** | Petition quest done; at least one Standing resolved; the Forfeit resolved (any shape) |
| **Time** | 70–110 min |
| **Synopsis** | Perrin calls the Compact's first real ratification vote. Osran won't block it and won't bless it. Dessa treats it as a direct threat to the Underwrite's position and interferes, quietly, through favour-calling rather than force. |
| **Objectives** | 1. Learn the ballot mechanics (essentially a larger-scale Standing — many backers, one binding count). 2. Canvas support, honestly or otherwise. 3. Handle Dessa's interference (a debt-called-in on a key Compact backer, timed to remove their vote). 4. Attend the count. |
| **Rewards** | Sets up `ending_crossing_compact` eligibility; `knowledge_key: lore_nc_the_count` |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete | Help the vote proceed cleanly — cover the called-in debt yourself, or persuade Dessa to delay collection on principle (hard, costly, possible). `mutation_crossing_vote_clean`. |
| Fail/Refuse | Stay out of it entirely. The vote happens or doesn't on its own momentum — usually doesn't, this once. `mutation_crossing_vote_stalled`. Perrin tries again later, weaker. |
| Double-Cross | Quietly help Dessa's interference succeed — point out which backer to target, in exchange for Underwrite favour. `mutation_crossing_vote_sabotaged`. Perrin never learns it was you, on-screen, but Ivo's records (§`quest_crossing_three_dry_pages`) will, if anyone ever looks. |

---

### `quest_crossing_three_dry_pages` — The Charter

| Field | Value |
|---|---|
| **Type** | exploration / story |
| **Prereqs** | Marker quest done; Records Room access |
| **Time** | 60–90 min |
| **Synopsis** | Ivo Fenn will not summarise. He produces the file. Three pages: a weigh-scale calibration and toll-revenue-sharing compact between two county highway authorities, decades old, notarised, utterly mundane. |
| **Objectives** | 1. Request the file from Ivo. 2. Read it (full diegetic text in the creative pack — three short, dry, real pages). 3. Decide: publish it plainly, sit on it, or sell the revelation to whichever bloc benefits most from discrediting a rival's founding claim. 4. Optional: cross-reference at `location_the_memory_vault` (Sole) or `loc_low_background_lab` (Cold Count) for authentication. |
| **Rewards** | `item_charter_three_pages`; `knowledge_key: lore_nc_three_dry_pages` |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete (Revealed) | Publish the truth plainly, to all three blocs at once, in public. `mutation_crossing_charter_revealed`. Nobody's founding myth survives intact; oddly, this calms more arguments than it starts, because it removes the one thing everyone was arguing past each other about. |
| Fail/Refuse (Kept Quiet) | Tell no one. `mutation_crossing_charter_hidden`. The myths continue exactly as before, load-bearing and false, same as they were the day the player arrived. |
| Double-Cross (Sold) | Sell the revelation privately to whichever bloc most benefits from discrediting a rival's claim to legitimacy (typically the Underwrite, against the Compact's founding-myth appeals). `mutation_crossing_charter_weaponised`. That bloc gains a permanent rhetorical weapon; Ivo Fenn, who trusted the player with the file on the explicit understanding it would be read, not sold, withdraws Records Room access for the rest of the playthrough. |

---

### `quest_crossing_who_holds_the_ledger` — Endgame

| Field | Value |
|---|---|
| **Type** | story |
| **Prereqs** | Two of three bloc chains resolved (any shape) + Charter found |
| **Time** | 45–70 min |
| **Synopsis** | A final Standing is called — not over a stall claim or a debt, but over what The Crossing *is*, going forward. Every prior resolution shape across this questline is tallied. |
| **Objectives** | 1. Attend the final Standing. 2. Back a bloc, back none, or attempt to broker a genuine three-way accommodation (rare, hard, requires high trust with all three simultaneously — realistically achieved only on a very clean playthrough). 3. Watch the ruling called. 4. Return home; the world-state mutation fires on the next relevant trade/travel check. |
| **Rewards** | Ending flag; `world_history` second paragraph; optional `victory_nobodys_charter` epilogue slide |

**Resolution shapes**
| Shape | Result |
|---|---|
| Complete (any bloc, honestly backed) | That bloc's ending fires (`ending_crossing_scale` / `_underwrite` / `_compact`). Clean prose, clean mutation. |
| Fail/Refuse (no bloc backed, no prior Standing rigged) | `ending_crossing_walked`. The Crossing continues without the player's name attached to its outcome either way. |
| Double-Cross (prior double-crosses stacked, or actively sabotage the final Standing) | `ending_crossing_none`. The vacuum fills with `faction_iron_raiders` (*existing Current*, cameo-only appearance) within a season — no questline, just a changed, worse location description and a closed market. |

**Main quest total player time:** ~7–10 hours including travel and needs management, not including side content.

---

## 4.2 Side quests (18)

### The Scale (`faction_the_scale`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_crossing_scale_integrity` | Osran Kell | Scalehouse | The scale's reference weight hasn't been checked against an outside standard in years. Osran wants to know, and is afraid to. | 1. Locate a genuine pre-war reference weight (Cold Count lab, cameo, or salvage). 2. Compare. 3. Report true or fudge it for his peace of mind. | Trade rate stability; `mark_crossing_scale_true` or `mark_crossing_scale_mercy` |
| `quest_crossing_watchtower_smuggling` | Watchtower lookout | Watchtower | Goods are moving across the viaduct at night, unweighed. Osran suspects, hasn't proven it. | 1. Watch a night. 2. Identify the smugglers (a Nightfire regular, working for Dessa off the books, or independent). 3. Report, ignore, or join. | Osran trust; or a quiet cut of the smuggling |
| `quest_crossing_stallrow_claim` | A stallholder | Stallrow | Two stalls, one chalk mark, both parties certain. Scavenger Guild rules cited by neither party correctly. | 1. Hear both claims. 2. Check the actual mark history (dates scratched underneath). 3. Rule, or bring it to a full Standing instead. | Backer favour; stallholder trust |

### The Underwrite (`faction_the_underwrite`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_crossing_the_collateral` | Dessa Vane | The Lockup | Collateral in storage doesn't match the ledger — someone's been quietly reclaiming pledged goods without paying. | 1. Audit the Lockup. 2. Identify the thief (a desperate debtor, sympathetically drawn). 3. Report to Dessa, cover the shortfall yourself, or let it go. | Underwrite trust; or a personal debt of your own |
| `quest_crossing_cold_feet` | A Underwrite collector | Nightfire | A collector can't make himself take a family's last blanket over a small debt. Dessa expects results, not feelings. | 1. Hear him out. 2. Cover the debt, talk Dessa into a grace period, or let the collection proceed. | Collector loyalty (future companion-adjacent bark); Underwrite reputation |
| `quest_crossing_provenance` | Dessa Vane | Underwrite Hall | Some collateral in the Lockup is stolen goods, laundered as a pledge. Dessa half-knows and doesn't want to fully know. | 1. Investigate two suspicious items. 2. Trace origin (possible `faction_iron_raiders` or Undertow-adjacent salvage, cameo only). 3. Tell Dessa, quietly return the goods, or say nothing. | Knowledge; possible Underwrite trust hit if exposed |

### The Compact (`faction_the_compact`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_crossing_the_clauses` | Perrin Ashby | Petition Tent | The draft needs an actual dispute-resolution clause — right now it just says "the Standing continues." Perrin wants the player's input. | 1. Discuss options (keep the Standing, replace it with fixed terms, hybrid). 2. Draft the clause with Perrin. 3. See it survive or get cut at the eventual vote. | `knowledge_key: lore_nc_the_clauses`; shapes `quest_crossing_the_vote_that_isnt` framing |
| `quest_crossing_annex_intake` | Annex resident | The Annex | A new arrival wants shelter; the Annex is full; someone already there would have to make room. | 1. Hear both cases. 2. Decide, or refuse to. 3. Live with who's grateful and who isn't. | Annex morale; `mark_crossing_annex_choice` |
| `quest_crossing_the_mole` | Perrin Ashby (unwitting) | Petition Tent | A Compact member is quietly reporting draft details to Dessa. Perrin hasn't noticed. | 1. Identify the mole (a debtor working off a debt with information instead of grain). 2. Expose, confront privately, or use the leak yourself. | Compact trust; leverage over the Underwrite if exposed publicly |

### Companion (4)

| id | Giver | Hook | Objectives | Mark |
|---|---|---|---|---|
| `quest_crossing_companion_osran` | Osran Kell | Why he stayed at a scale-house instead of walking to any of the four Powers' territories, where a man with his skills would have been welcomed. | 1. Ask. 2. Learn he tried, once, in year one — and watched a Power's "welcome" turn into a job he couldn't quit. 3. Decide whether to tell him a Power is recruiting again (if a hegemony hook is live). | He will not leave The Crossing regardless of your answer; asking changes only what he says about it after |
| `quest_crossing_companion_dessa` | Dessa Vane | The one forgiven debt no one is supposed to know about. | 1. Find the discrepancy in the ledger (one contract, closed, no payment recorded). 2. Ask her about it. 3. She tells you, once, why — or doesn't, and the ledger stays a small, permanent mystery. | Dessa companion trust; does not change any bloc-level mechanic — this is character, not systems |
| `quest_crossing_companion_perrin` | Perrin Ashby | Confront the rubric clause personally, not abstractly. | 1. Perrin scores the player's own shelter against the draft's voting-weight formula. 2. React. 3. Perrin either revises the formula on the spot or defends it — player-driven, not scripted. | Feeds `quest_crossing_the_petition`'s revision outcome retroactively if not already resolved |
| `quest_crossing_companion_mattis` | Mattis Cray | What vouching for strangers for three years has actually cost him. | 1. Ask who he's vouched for that burned him. 2. Learn one name — someone now barred from the Crossing entirely because of a debt Mattis is still quietly covering. 3. Offer to help pay it down, or don't. | Mattis's "will not vouch twice" rule gets a face; paying it down is the only way to soften it |

### Exploration (3)

| id | Location | Hook | Objectives | Reward |
|---|---|---|---|---|
| `quest_crossing_managers_office` | Scalehouse upper floor | The pre-war depot manager's office, sealed, un-looted because nobody's ever agreed who it belongs to. | 1. Get consensus to open it (a small Standing of its own). 2. Loot/document. 3. Find a pre-war duty log that explains the weigh-scale dispute the real Charter settled. | `item_duty_log_fragment`; sets up `quest_crossing_three_dry_pages` context |
| `quest_crossing_smugglers_cache` | Under the viaduct | A cache under the truss, older than the Crossing's current arrangement — pre-dates even Mattis. | 1. Find it (dangerous footing, existing expedition hazard resolution). 2. Identify the era (Cold Count cameo, optional). 3. Claim, report to the Scale, or leave it. | Salvage; `knowledge_key: lore_nc_older_than_you_think` |
| `quest_crossing_the_weigh_log` | Scalehouse archive | Decades of pre-war weigh logs show a seasonal pattern — someone was under-declaring loads for years before the war, and the real Charter's calibration clause exists because of it. | 1. Read the logs. 2. Cross-reference with the Charter once found. 3. Understand *why* the document exists — a mundane fraud case, not a founding. | `knowledge_key: lore_nc_the_actual_reason`; recontextualises the whole mystery on a second read |

### Repeatable (2)

| id | Giver | Hook | Objectives | Loop |
|---|---|---|---|---|
| `quest_crossing_weigh_run` | Osran / Stallrow | Standing trade haul: shelter surplus in, genuinely fair-priced goods out. | 1. Fill a manifest. 2. Cross the viaduct (vouch must still be valid). 3. Weigh, trade, return. | `DynamicEconomySystem` prices at true market rate; fatigue cost; the pack's basic economic loop |
| `quest_crossing_nightfire_watch` | Mattis Cray | Sit the Nightfire, listen, report back what's actually being said versus what the blocs claim is being said. | 1. Send a survivor for an evening. 2. Utility AI "listen, don't talk" check. 3. Rumour yield: early warning on the next main-quest beat. | Information; small morale cost (fatigue) |

---

## 4.3 Morale micro-choices catalog (26)

1–2 sentences of situation, 2–3 options, a flag, and one sentence of later evidence. Not `Morale +2`. Split roughly half shelter-door, half on-site, per the request's emphasis on both.

### Shelter-door (14)

| id | Situation | Options | Mark later |
|---|---|---|---|
| `mmc_nc_collector_at_hatch` | An Underwrite collector at the hatch, polite, with a number. | Pay it / argue the number / deny entry (existing hatch constants). | `mark_crossing_collector_seen`: future collectors are shorter with you. |
| `mmc_nc_perrin_recruiter` | A Compact canvasser wants a signature on the petition, at your own door. | Sign / decline / ask them to come back with Perrin himself. | `mark_crossing_canvassed`: affects `quest_crossing_the_vote_that_isnt` starting support. |
| `mmc_nc_refugee_at_hatch` | An Annex-bound refugee, turned around by weather, asks for one night. | Let in (existing rads/morale) / point them to St Brigid's or the Approach / turn away. | `mark_crossing_refugee_turned` or `_sheltered`: Compact hears either way. |
| `mmc_nc_mattis_urgent_vouch` | Mattis at the hatch, out of breath, needs an emergency vouch for someone he half-trusts. | Vouch on his word / ask questions first / refuse. | `mark_crossing_second_hand_vouch`: if it goes wrong, it costs Mattis, not you — a fact the player only learns later. |
| `mmc_nc_osran_delivery` | Osran sends a genuinely fair-value trade offer by runner, no pressure. | Accept / counter / ignore. | `mark_crossing_fair_deal_noted`: Stallrow prices soften slightly on your next visit. |
| `mmc_nc_wyn_letter` | A letter from Wyn, before the forfeit comes due, that reads exactly like someone who already knows the answer. | Reply with help promised / reply honestly that you can't / don't reply. | `mark_crossing_wyn_warned`: shapes her tone in `quest_crossing_the_forfeit`. |
| `mmc_nc_ivo_query` | Ivo Fenn sends a runner with a filing question about your own shelter's founding — for his records, he says. | Answer fully / answer minimally / decline. | `mark_crossing_on_file`: a copy of your answer sits in the Records Room, discoverable by anyone later. |
| `mmc_nc_dessa_terms_reminder` | Dessa's runner reads your own contract terms back to you, unprompted, at the hatch. Exactly as agreed. Nothing more. | Thank them / bristle / say nothing. | `mark_crossing_terms_respected`: no mechanical effect — purely a tone read on how the shelter is talked about at the Nightfire. |
| `mmc_nc_scale_auditor` | An Osran-sent auditor wants to re-weigh your last trade goods, at your hatch, to spot-check the Scalehouse's own honesty. | Allow / delay / refuse. | `mark_crossing_audited`: refusing, even innocently, reads as guilt at Stallrow. |
| `mmc_nc_deserter_coalition_ask` | A Deserter Coalition contact (*existing Current*, cameo) asks you to pass a message to the Compact without using their name. | Pass it / refuse / read it first. | `mark_crossing_courier`: minor Compact trust if passed faithfully. |
| `mmc_nc_iron_raiders_rumor` | A trader mentions Iron Raiders (*existing Current*) have been asking about the viaduct's foot-traffic patterns. | Warn Mattis / warn Osran / say nothing. | `mark_crossing_warning_given`: matters heavily if `ending_crossing_none` is trending. |
| `mmc_nc_child_asks_charter` | A shelter child asks what "nobody's charter" means, having overheard the phrase. | Explain plainly / simplify kindly / say you don't know yet. | `mark_crossing_child_told`: pays off once the real Charter is found. |
| `mmc_nc_lockup_notice` | A notice arrives: someone's forfeit is public record now, posted at the Nightfire, and it isn't yours — but it could be. | Read it / burn it / ignore it. | `mark_crossing_read_the_notice`: small, quiet foreshadowing beat. |
| `mmc_nc_returning_favor` | A stallholder you backed once at a Standing sends a genuinely useful gift, unasked. | Accept / return it / accept and give something back. | `mark_crossing_favor_banked`: cheaper backer recruitment next Standing. |

### On-site (12)

| id | Situation | Options | Mark later |
|---|---|---|---|
| `mmc_nc_scale_thumb` | You could nudge the scale reading in your own favour by a fraction. Nobody would know but Osran, and maybe not even him. | Don't / do it once / do it and confess. | `mark_crossing_thumb_on_scale`: confessing costs nothing mechanically and everything socially, in the good direction. |
| `mmc_nc_backer_bribe` | A backer at the Standing will switch sides for a small, private gift. | Bribe / persuade honestly / withdraw. | `mark_crossing_bought_backer`: quietly logged by Ivo regardless of whether anyone else notices. |
| `mmc_nc_wyn_grain_offer` | You could quietly top up Wyn's granary yourself, before the forfeit, so the count reads different than it is. | Do it secretly / do it openly / don't. | `mark_crossing_grain_topped`: secret version is discoverable later and reads worse than if it had been open. |
| `mmc_nc_ledger_page` | The Underwrite ledger is chained but not locked. A page could be torn out. | Tear it / read it / leave it. | `mark_crossing_ledger_torn`: a debtor whose page vanishes benefits — someone else's debt then gets misattributed to cover the gap. |
| `mmc_nc_annex_bed` | The Annex has one more bed than beds accounted for on the roster. | Report the discrepancy / say nothing / claim it for a companion. | `mark_crossing_extra_bed`: small Compact trust either way, direction depends on choice. |
| `mmc_nc_marker_rubbing` | You could take a rubbing of the Founders' Marker before anyone official does — control the first telling. | Take it and share plainly / take it and embellish / leave it. | `mark_crossing_first_telling`: embellishing seeds a new false legend, same family as the Double-Cross outcome on `quest_crossing_the_marker`. |
| `mmc_nc_watchtower_favor` | The lookout offers to "forget" they saw you carrying more than your declared weight across the viaduct. | Accept / decline and re-declare / report the offer to Osran. | `mark_crossing_underdeclared`: small trade bonus now, real trust cost if ever audited. |
| `mmc_nc_petition_wording` | Perrin asks you to help word one sensitive clause — about who counts as a "resident." | Word it inclusively / word it narrowly / decline to help. | `mark_crossing_clause_worded`: directly shapes Annex refugee eligibility under a ratified charter. |
| `mmc_nc_fenn_offcut` | Ivo Fenn has filed, but not destroyed, records that are personally embarrassing to each of the three bloc leaders. He'd let you see one, once. | Look / decline / ask him to burn it instead. | `mark_crossing_read_the_offcut`: leverage, usable once, against whichever leader's record you read. |
| `mmc_nc_nightfire_rumor_sell` | A rumour you overheard at the Nightfire is worth something to a rival bloc. | Sell it / share it freely / keep it. | `mark_crossing_rumor_sold`: small immediate gain, a Double-Cross-shaped mark even outside a formal quest. |
| `mmc_nc_collateral_return` | You recognise a pledged item in the Lockup as something stolen from a Sector 4 trader you know. | Return it quietly / expose the pledge as stolen / say nothing. | `mark_crossing_collateral_returned`: feeds `quest_crossing_provenance` if not already resolved. |
| `mmc_nc_final_standing_seat` | At the endgame Standing, there's one empty seat among the backers, unclaimed. | Take it yourself / offer it to a companion / leave it empty. | `mark_crossing_seat_taken`: cosmetic in the final slide's wording only — the game notices who was sitting down when the ledger changed hands. |

---

## 4.4 Shelter/gate encounter table (14)

Reuses hatch constants and Duty Roster's `ShelterEncounterSystem` (*hooked, not rebuilt*) where a hatch scene is implied; the viaduct-side encounters use the same trigger/cooldown pattern applied to `loc_crossing_viaduct_gate` instead of the home hatch.

| id | Trigger | Beats | Morale mark | Mutation / flag |
|---|---|---|---|---|
| `se_nc_collector_hatch` | hatch + active `DebtContract` | Collector reads the term, waits, does not threaten. | Existing hatch constants apply if physically admitted | Debt state progression |
| `se_nc_canvasser_hatch` | hatch + petition signed | Perrin's canvasser asks for a repeat endorsement ahead of the vote. | `mark_crossing_canvassed` | Vote support tally |
| `se_nc_refugee_hatch` | hatch + Annex over capacity | A turned-away Annex hopeful, cold, polite, not desperate yet. | Existing rads/morale if admitted | Compact recruitment |
| `se_nc_mattis_favor` | hatch + Mattis trust high | He asks for a favour that isn't about vouching for once — personal. | Companion mark | Sets up `quest_crossing_companion_mattis` |
| `se_nc_gate_challenge` | viaduct + vouch expired or burned | Someone new is holding the gate; the old arrangement doesn't apply. | Access tension | Re-vouch required |
| `se_nc_watchtower_hail` | viaduct approach | The lookout calls down before you're close enough to see them. Routine, unless trust is low. | Threatening variant if Scale trust low | Entry friction |
| `se_nc_nightfire_overheard` | Nightfire, night phase | A conversation not meant for you, about you. | `mark_crossing_overheard` | Early quest-beat warning |
| `se_nc_lockup_procession` | Lockup, forfeit day | A collection happens whether or not you're there to see it. | Witnessed vs. absent prose differs | Forfeit mutation confirms |
| `se_nc_stallrow_dispute` | Stallrow, random | A minor dispute erupts; small-scale Standing without full ceremony. | Backer reputation | Micro-version of §5.1 mechanics |
| `se_nc_records_visit` | Records Room, repeat visits | Ivo notes you're back again and asks, mildly, what you're actually looking for. | Trust-neutral | Charter quest pacing |
| `se_nc_iron_raiders_scout` | viaduct approach, `ending_crossing_none` trending | A scout, watching the gate, not yet acting. | Dread, not danger yet | Foreshadows collapse ending |
| `se_nc_child_at_annex` | Annex, if a shelter child accompanies | A Compact child asks your child what a vote is. | `mark_crossing_child_told` payoff | Cosmetic, cross-references home |
| `se_nc_osran_audit_visit` | Scalehouse, post-`quest_crossing_scale_integrity` | Osran shows you the reference weight himself, once, unprompted, if you helped him find the truth. | Trust payoff | None — pure character beat |
| `se_nc_final_summons` | any Crossing location, endgame prereqs met | A runner from whichever bloc(s) still trust you asks you to attend the final Standing. | N/A | Triggers `quest_crossing_who_holds_the_ledger` |

---

# SECTION 5 — NEW GAMEPLAY SYSTEMS

**Cap: 3 new plain-C# systems.** No LLM. Event-raising. Save-safe. Host-callback injection like `ShelterDegradationSystem`. **Hook, do not rebuild:** `ShelterEncounterSystem` / `MoraleMarkSystem` (Duty Roster), `WorldStateConsequenceSystem`, `DynamicEconomySystem`, `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids`, `ExpeditionSystem` hatch constants.

---

## 5.1 `CrossingArbitrationSystem`

**id:** `crossing_arbitration_system`
**What it is:** "The Standing." A ruling is real for exactly as long as three backers hold it — no more, no less. The mechanical expression of the whole pack's thesis.

**Mechanics:**
- A pool of ~10–14 named stallholder/bloc-adjacent NPCs act as potential backers, each with their own `wants`/`will not` (reusing the companion "will not cannot be bought off" pattern from both prior packs, applied to non-companions for the first time).
- Any dispute (quest-scripted or the repeatable `se_nc_stallrow_dispute`) can be brought to a Standing.
- A ruling needs 3 declared backers to hold. Backers are earned through favours, trades, prior honest rulings, or persuasion — never a flat purchase past a soft cap (some backers refuse a bribe outright and will say so publicly if pushed, which itself becomes a mark).
- A later Standing with a different 3+ backers can overturn an earlier ruling. Nothing is permanently settled — this is fiction, not a bug.
- Rulings are stored as `StandingRuling { topic, backers[], shape }` and read by quest/mutation logic wherever "who currently controls X at the Crossing" matters.
- Events: `OnStandingCalled`, `OnRulingMade`, `OnRulingOverturned`.

**UI/UX:** A diegetic notice-board list at Stallrow — current rulings and their backers, publicly visible, exactly as the fiction says it should be. No hidden meter.

**Balance:** Cannot be permanently captured by money alone — principled backers exist specifically to cap pure-bribery play. No AI arms race; overturns require the player (or scripted beats) to actively call a new Standing, not a background simulation running unattended.

**Integration:** `WorldStateConsequenceSystem` reads the endgame ruling to fire the ending mutation. `NeedsSystem` morale as a *result* of marks (§4.3), never the mechanism itself.

**Unrealistic (do not build):** a full agent-based political simulation, procedurally generated NPC opinions, a voting UI with hundreds of simulated agents.

---

## 5.2 `VouchAccessSystem`

**id:** `vouch_access_system`
**What it is:** The gate. Social, not seasonal — a relationship, not a calendar.

**Mechanics:**
- First entry requires `vouchedBy` = an NPC id willing to stake their own standing (Ostrowski by default; Mattis, or a sister-pack contact, as alternates — Appendix A).
- A vouch can be burned: if the player betrays whoever backed them badly enough (tracked via specific quest/mark flags, not a generic trust number), that NPC's own relationship takes a real, mostly permanent hit, and the player is turned back at the viaduct until a *new* vouch is found.
- After the opening arc (`flag_crossing_vouched_clean` or equivalent), the gate softens: the player's own name becomes sufficient, tracked as informal standing rather than a re-triggerable gate check.
- Mattis exists explicitly as the "last resort" vouch — always available, at real narrative cost (`quest_crossing_companion_mattis`), so the pack never hard-locks the player out entirely.
- Events: `OnVouchGranted`, `OnVouchBurned`, `OnAccessSoftened`.

**UI/UX:** No countdown, no calendar bar. The viaduct's location description and the encounter table (`se_nc_gate_challenge`) carry the state.

**Balance:** Never a full lockout for the rest of the pack — there is always one more vouch available, at increasing narrative (not mechanical) cost.

**Integration:** Reads/writes flags consumed by `quest_crossing_the_vouch` and the branching table (§3). Does not touch `IceRoadSystem` or any Holdfast calendar mechanic.

**Unrealistic (do not build):** a rejected-at-the-door minigame, timed dialogue trees, a reputation number visible to the player as a raw integer.

---

## 5.3 `LedgerDebtSystem`

**id:** `ledger_debt_system`
**What it is:** Debt as a document, not a currency. Read twice, forfeit named up front, never a hidden clause — the mechanical expression of Dessa's own ethic.

**Mechanics:**
- `DebtContract { debtorId, principal, termDays, rate, forfeit }`. The forfeit is always a named, knowable thing (an item, a service-days count, a pledged good) — never abstract.
- Contract text is shown in full, twice, before signing (matches `faction_the_tally`'s existing `access_rule` flavour, implemented here for the first time rather than left as Codex text).
- On term end: paid in full / renegotiated (extends term, adjusts rate, requires a fresh Standing if contested) / defaulted (forfeit triggers, `quest_crossing_the_forfeit`-shaped) / player intervenes on someone else's contract (pay it off, expose fraud in it, or help the debtor default and flee).
- Events: `OnContractSigned`, `OnContractPaid`, `OnContractRenegotiated`, `OnForfeitTriggered`, `OnLedgerTampered`.

**UI/UX:** The literal contract text, re-readable on demand from the player's inventory/Codex — never summarised into a number.

**Balance:** No surprise terms, ever. Every forfeit is knowable before signing. The tension is entirely about whether the player paid attention and whether circumstances changed, not about hidden fine print.

**Integration:** Hooks `DynamicEconomySystem` for goods pricing at signing; `WorldStateConsequenceSystem` for the Underwrite-aligned ending mutation; `CrossingArbitrationSystem` for contested renegotiations.

**Unrealistic (do not build):** a full loan-amortisation simulation, a numeric credit score, interest compounding beyond what a single flat `rate` field can express.

---

## Systems explicitly not in this expansion

- No fourth new system. Three is the cap, same discipline as both sister packs.
- No fifth Sector 4 Power, no seventh `faction_lore.json` row, no new `WorldStateConsequenceSystem._hegemony` entry.
- No second shelter/base to manage. The Crossing has no player-ownable structure.
- No combat AI beyond existing expedition encounter resolution. The Lockup's "muscle" is a cost/risk modifier, not a fight system.
- No new victory-path architecture beyond the optional `victory_nobodys_charter` epilogue flag.
- No livestock, no vehicle depth, no FalloutForecast — none of this pack's content needs them.

---

# SECTION 6 — CHARACTERS & ENCOUNTERS

## 6.1 Companions (4)

Assignable labour and expedition company, not a party. Utility AI bias + an unbuyable "will not," same discipline as both prior packs.

| id | Name | AI bias | Will not | If they die / leave |
|---|---|---|---|---|
| `npc_osran_kell` | Osran Kell | Weigh, verify, refuse to arbitrate | Rig a weight, claim authority he'd have to defend | Scalehouse trade continues at a worse, unverified rate; Stallrow gets nervous |
| `npc_dessa_vane` | Dessa Vane | Contract, collect, refuse to forgive publicly | Waive a forfeit on the record | The Underwrite continues under a harder, less personally-flexible successor |
| `npc_perrin_ashby` | Perrin Ashby | Draft, canvas, refuse to cut corners on wording | Pass a clause they know is unfair to save time | The Compact's draft stalls indefinitely; the Annex loses its clearest advocate |
| `npc_mattis_cray` | Mattis Cray | Vouch, run messages, refuse to pick a bloc | Vouch a second time for someone who burned him once | The viaduct gate gets harder for everyone — he was the last-resort option |

Ivo Fenn and Wyn Sabler are **not** expedition companions — stationary, quest-critical, same status as Ansel/Len in Duty Roster.

Utility AI actions (*PROPOSED*): `Action_WeighGoods`, `Action_ReadContract`, `Action_CanvasSupport`, `Action_RunVouch`. Seed `_worldSeed + 1811`.

## 6.2 Encounter variants (10)

Human danger only — people in conditions, not fantasy threats. "Combat" resolves through existing `ExpeditionSystem` encounter mechanics.

| id | Name | Where | Cost | Notes |
|---|---|---|---|---|
| `enc_nc_collector_visit` | Underwrite collector | hatch, viaduct | Time, goods, or morale on deny | Polite, procedural, exactly as threatening as the contract terms say and no more |
| `enc_nc_backer_pressure` | A backer wants a favour before agreeing to back you | Stallrow, Nightfire | Goods or a future favour owed | Not a fight — a negotiation with real refusal risk |
| `enc_nc_lockup_muscle` | Collateral escort | The Lockup | Risk if resisted; cost if paid | Only turns dangerous if the player tries to take collateral by force |
| `enc_nc_iron_raiders_scout` | Watching the gate | Viaduct approach | Dread; danger only if provoked | Cameo-only; `faction_iron_raiders` does not get a questline here |
| `enc_nc_deserter_passage` | A Deserter Coalition contact needs safe passage through Crossing territory | Viaduct, Annex | Risk of Garrison attention if noticed | Cameo of *existing* Current; sheltering them is the Duty Roster/base-game risk pattern, unchanged |
| `enc_nc_scavenger_dispute` | A Scavenger Guild-blacklisted trader tries to sell at Stallrow anyway | Stallrow | Osran's trust if you vouch for them | Cameo; tests whether the player understands the claim-blacklist rule they've been imitating |
| `enc_nc_grain_exchange_envoy` | A Grain Exchange envoy asks why the Crossing doesn't send a seat to the board | Stallrow | Time; a diplomatic answer required | Cameo; the Exchange treats the Crossing as a rival price-setter, not an ally |
| `enc_nc_sun_seekers_pass` | Sun-Seekers (*existing Current*) passing through during a False Spring window | Viaduct | UV gear check | Pure flavour/trade cameo, no quest attached |
| `enc_nc_forfeit_witness` | A forfeit collection happens in public | The Lockup, Stallrow | Morale (witnessed) | No player action required or possible — a scene, not a choice |
| `enc_nc_standing_ambush` | A rival bloc tries to pack a Standing with last-minute backers | Stallrow | Backer favours spent fast | The only "timed pressure" encounter in the pack, matching existing crisis pacing |

## 6.3 Crises (5) — multi-phase, not arenas

| id | Name | Phases | Failure | Success looks like |
|---|---|---|---|---|
| `crisis_the_forfeit` | Wyn's granary | Notice → Terms read → Raise/Broker/Flee → Collection or not | Public collection, calm and total | Debt honoured or fairly renegotiated |
| `crisis_the_vote` | Draft Four ratification | Call → Canvas → Interference → Count | Vote stalls indefinitely | Clean or contested-but-real ratification |
| `crisis_the_standing_contested` | A ruling gets challenged | Call → Backer recruitment → Rival recruitment → Result | Ruling overturned publicly, backer trust drops | Ruling holds, or is honestly overturned without a rig |
| `crisis_the_charter_found` | Three Dry Pages | Request → Read → Verify → Decide | N/A — this crisis cannot be "failed," only resolved differently | Any of the three resolution shapes, played through fully |
| `crisis_who_holds_the_ledger` | Endgame Standing | Summons → Final backing → Ruling → Aftermath | Collapse (`ending_crossing_none`) | Any of the four non-collapse endings |

Osran, Dessa, and Perrin are not final bosses. None can be "defeated" — only out-argued, out-backed, or, in Dessa's case, refused payment at real cost. Killing any of them (player-caused, possible, costly) does not resolve their bloc; a colder successor takes the seat within a season.

---

# SECTION 7 — ITEMS & REWARDS

Existing tools remain canonical: `dosimeter`, `geiger_counter`, `iodine_pills`, `anti_rad`, `hazmat_suit`, `water_filter`, `air_filter`, `brass_fittings`. All new item ids **PROPOSED**.

## 7.1 Sets (5)

| Set id | Pieces | Function |
|---|---|---|
| `set_crossing_trade` | `item_vouch_token_crossing`, `item_calibration_weight`, `item_trade_manifest_blank` | Access + fair-rate trading keys |
| `set_crossing_paper` | `item_debt_contract_copy`, `item_ledger_page_torn` (*only if taken*), `item_petition_draft_copy` | Quest keys; visible proof of terms |
| `set_crossing_charter` | `item_charter_three_pages`, `item_duty_log_fragment`, `item_marker_rubbing` | The mystery, end to end |
| `set_crossing_standing` | `item_backer_favor_token` ×N, `item_standing_notice_copy` | Arbitration currency (favour, not coin) |
| `set_crossing_annex` | `item_annex_ration_share`, `item_pledge_receipt` | Wyn's arc; Compact humanitarian content |

## 7.2 Legendaries (10) — unique, with a history, no magic

| id | Name | Where | What it does | Flavour (first line) |
|---|---|---|---|---|
| `item_charter_three_pages` | The Actual Charter | Records Room | Codex unlock; resolves the title's question | Page one is a calibration tolerance. It is, somehow, still the most contested document in Sector 4. |
| `item_calibration_weight` | The True Weight | Scalehouse | Proof-of-honest-scale; unlocks `quest_crossing_scale_integrity` | Stamped, dated, and correct to a gram nobody here can currently verify except by trusting it. |
| `item_wyn_receipt_paid` | Paid in Full | Forfeit quest, Complete shape only | Unique flavour item; no mechanical effect | The ink is the same ink as the original contract. She kept the same pen. |
| `item_debt_contract_copy` | Read Twice | Terms quest | Re-readable full contract text, permanently | Every word Dessa said the second time, in her own hand, unchanged from the first. |
| `item_marker_rubbing` | Third Line Missing | Marker quest | Charter-mystery clue; combines with duty log | Charcoal on paper. The corrosion came through anyway. Some things resist being copied out of. |
| `item_duty_log_fragment` | Depot Duty Log, Partial | Manager's Office | Explains *why* the Charter exists; recontextualises the mystery | A dispute about axle weight, in a hand that clearly thought it mattered more than it turned out to. |
| `item_backer_favor_token` | A Favour, Owed | Standing quests | Arbitration currency; spend to recruit a backer | Not money. A memory of a thing you did, redeemable exactly once. |
| `item_iron_raiders_marker` | Left at the Gate | `ending_crossing_none` only | Ending-state flavour item | Nobody claims to have left it. Nobody has to. |
| `item_ivo_filing_stamp` | The Last Stamp | Records Room, Complete-path only | Cosmetic; Codex flavour | Ivo will stamp anything you bring him correctly filed. It means nothing to anyone but him. It means everything to him. |
| `item_annex_child_drawing` | A Vote, Drawn | `mmc_nc_child_asks_charter` payoff | Cosmetic; shelter decoration | A child's picture of people raising hands. Nobody taught her what a vote was. She'd heard the word enough to guess the shape. |

## 7.3 Consumables (new)

| id | Effect |
|---|---|
| `item_trade_manifest_blank` | Required for `quest_crossing_weigh_run`; consumed on use |
| `item_annex_ration_share` | Small hunger relief; Compact goodwill if declined in favour of someone else |
| `item_pledge_receipt` | Proof of a paid-off debt; prevents duplicate collection attempts |
| `item_standing_notice_copy` | Codex-only; no gameplay effect beyond flavour |

## 7.4 Achievements (21)

`ach_nc_*`. No kill-counts. No jokes that break tone.

| id | Name | Condition |
|---|---|---|
| `ach_nc_vouched` | A Name at the Gate | Enter The Crossing for the first time |
| `ach_nc_clean_vouch` | Spent Well | Complete the opening arc without burning a vouch |
| `ach_nc_burned_vouch` | Spent Badly | Burn a vouch and find a second one anyway |
| `ach_nc_true_weight` | Honest Scale | Complete `quest_crossing_first_weigh` honestly |
| `ach_nc_bribe_refused` | Folding Chair | Have a bribe attempt publicly refused by Osran |
| `ach_nc_read_twice` | Read It Again | Sign a Dessa contract and pay it off in full |
| `ach_nc_burned_ledger` | Underwrite Burned | Double-cross a debt contract |
| `ach_nc_rubric_seen` | The Same Shape | Notice and name the petition's rubric problem aloud |
| `ach_nc_clause_fixed` | Revised | Get Perrin to genuinely revise the voting clause |
| `ach_nc_first_standing` | Three Backers | Successfully call a Standing |
| `ach_nc_bought_ruling` | Bought, Not Backed | Win a Standing through bribery |
| `ach_nc_wyn_paid` | Paid in Full | Resolve the Forfeit by honouring the debt |
| `ach_nc_wyn_fled` | Gone With the Grain | Resolve the Forfeit by double-crossing the Underwrite |
| `ach_nc_vote_clean` | Draft Four, Clean | Complete the ratification vote without sabotage on either side |
| `ach_nc_the_charter` | Three Dry Pages | Find and read the real Charter |
| `ach_nc_charter_sold` | Sold It | Sell the Charter revelation for advantage |
| `ach_nc_folding_chair_ending` / `ach_nc_paid_in_full_ending` / `ach_nc_draft_signed_ending` / `ach_nc_no_ones_ending` / `ach_nc_just_passing_ending` | (5 ids) | One per ending (§3 Endings table) |

## 7.5 Narrative word-count estimate

| Bucket | Words | Notes |
|---|---|---|
| Main quest stage/choice text (incl. Three Shapes) | 13,000 | 10 quests × ~1,300, tripled resolution text included |
| Side quests | 9,500 | 18 × ~530 |
| Morale micro-choices | 3,000 | 26 shelter-door + on-site |
| Shelter/gate encounters | 4,500 | 14 playable scenes |
| NPC voice bibles (6 × barks + monologue) | 4,500 | Matches Duty Roster's per-NPC density |
| Location cards (12 new + 5 overlays) | 3,000 | |
| The Charter (in-fiction document text) | 800 | Three short, dry, real pages, written in full |
| Endings + notice-board/ledger flavour | 2,000 | |
| **Creative pack target** | **~40,000–44,000** | Quest-weighted, denser than Duty Roster's 22–26k because every main quest carries three resolution shapes instead of one |

---

# SECTION 8 — TECHNICAL IMPLEMENTATION PLAN

## 8.1 Architecture mapping

| Concern | Existing pattern | Nobody's Charter |
|---|---|---|
| Data | `StreamingAssets/Data/*.json` + JsonUtility-safe DTOs | `crossing_factions.json` (Currents-shaped), `crossing_locations.json` (or append `locations.json`), `crossing_quests.json`, `crossing_marks.json`, world_history append |
| Logic | Plain C# systems, events, save blobs | `CrossingArbitrationSystem`, `VouchAccessSystem`, `LedgerDebtSystem` |
| Host | `GameBootstrap` partials | `GameBootstrap.NobodysCharter.cs` |
| AI | `UtilityAI` + `ActionScorer` | New `SurvivorAction`s, no LLM |
| UI | UI Toolkit, Lore Codex, event modal | Standing notice-board list; contract re-read panel; Crossing Codex tab |
| Map | `GeneratedMap` nodes | `region_crossing` tag, travelHours 3.5–6.5, single front |
| Economy | `DynamicEconomySystem` | True-price hook; no new currency |
| Lore | `LoreDiscoveryIndex` | New `lore_nc_*` knowledge keys |
| Quests | `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids` | Register all `quest_crossing_*` |
| Consequences | `WorldStateConsequenceSystem` | New market/route mutations; **do not** add a Crossing row to `_hegemony` |
| Hatch | `ExpeditionSystem` constants; Duty Roster `ShelterEncounterSystem` | Bridge only, extended to cover the viaduct gate trigger |

**Ids namespace:** `loc_crossing_*`, `faction_the_scale`, `faction_the_underwrite`, `faction_the_compact`, `npc_osran_kell`, `npc_dessa_vane`, `npc_perrin_ashby`, `npc_mattis_cray`, `npc_ivo_fenn`, `npc_wyn_sabler`, `quest_crossing_*`, `mmc_nc_*`, `se_nc_*`, `enc_nc_*`, `crisis_*`, `lore_nc_*`, `mutation_crossing_*`, `ending_crossing_*`, `mark_crossing_*`.

## 8.2 Assets (specify only; generate later into `generated_AIassets/`)

Dry-gouache, isolated objects, no readable AI text, no flags, no gore, no fantasy glow. One new visual note: **hand-lettered signage** — every Crossing sign should read as painted by a different, unofficial hand.

| Asset | Type | Notes |
|---|---|---|
| Location cards × 12 | 2D illustration | Scale, chalk-marked stalls, chained ledger, petition tent |
| Faction badges × 3 | Badge | Scale (a stencilled weight symbol), Underwrite (a ledger corner), Compact (a hand-drawn "4") |
| NPC portraits × 6 | Chest-up, deferred | Osran, Dessa, Perrin, Mattis, Ivo, Wyn |
| Items × ~25 icons | 64–128 px | Contract page, calibration weight, favour token, three dry pages |
| Standing notice-board UI | UITK | Public ruling list |
| Contract re-read panel | UITK | Full text, twice, no summary |
| **Not in scope** | 3D interchange, full VO, new music album (reuse ash ambience) | |

## 8.3 Sprints (4 × 3 weeks)

| Sprint | Goal | Deliverables | Verify |
|---|---|---|---|
| **S1 — The Gate & the Scale** | Entry works | `VouchAccessSystem`; viaduct + Scalehouse + Stallrow POIs; `quest_crossing_the_vouch` / `_first_weigh`; Osran + Mattis; JSON | Vouch burn/grant roundtrip; save-safe; compile PASS |
| **S2 — Paper & Debt** | Underwrite/Compact live | `LedgerDebtSystem`; Underwrite + Compact clusters; `quest_crossing_the_terms` / `_the_petition`; Dessa + Perrin | Contract read-twice UI; forfeit trigger logic; compile PASS |
| **S3 — The Standing** | Arbitration works | `CrossingArbitrationSystem`; `quest_crossing_the_standing` / `_the_forfeit` / `_the_vote_that_isnt`; Wyn; backer pool | Ruling hold/overturn roundtrip; mutations fire correctly; compile PASS |
| **S4 — The Charter & Endings** | Endgame works | `quest_crossing_three_dry_pages` / `_who_holds_the_ledger`; Ivo; all 5 endings; 18 side quests; morale/encounter catalogues | Ending flags exclusive; compile PASS; PlayMode: one full Standing cycle |

**QA (all sprints):** vouch state persists across save/load; no seventh `faction_lore.json` row; no new `_hegemony` entry; hatch magnitudes unchanged.

## 8.4 Risks

| Risk | Mitigation |
|---|---|
| Reads as a fifth Power | Compliance argument (§5) enforced in every quest description: rulings, never ground. Reviewer checklist item. |
| `CrossingArbitrationSystem` becomes a hidden political sim | Hard cap: scripted Standings + one repeatable micro-version (`se_nc_stallrow_dispute`). No background agent loop. |
| Three Shapes feels like three copies of the same quest | Each shape names a *different* NPC/bloc as the one who reads the consequence — never a stat delta alone. Enforced in every main-quest table row. |
| Debt system reads as a monetisation mechanic | No real currency, no purchasable advantage, forfeits always named in-fiction. Explicit in `AGENTS.md` review pass. |
| Charter mystery deflates player excitement (it's "just" three boring pages) | That is the intended payoff, matching The List's own method — Section 10 explicitly defends this as a feature, not a miscalibration. |
| Overlaps `faction_the_tally` too closely | The Tally is name-checked once in flavour (§Section 3, lore revelation 5) and never given a quest here; `LedgerDebtSystem` is Underwrite-only, a new local institution, not a Tally reskin. |
| Cross-tool QA (≥2 coupled variables) | Vouch state × Standing backers × Debt terms is **three** coupled variables. Implementer ≠ reviewer (Prompt #26). Reviewer sees diff + this spec only. |

## 8.5 QA cases (minimum)

1. Old save → Ostrowski rumour → vouch → viaduct opens
2. Vouch burned → re-entry blocked → Mattis last-resort vouch succeeds at cost
3. Contract signed → term expires unpaid → forfeit triggers → Lockup collection scene fires
4. Standing called → 3 backers → ruling holds → later Standing with 3 different backers overturns it cleanly
5. Double-cross a contract → `mutation_crossing_underwrite_burned` → future visits show collector escort
6. Charter found → sold to Underwrite → Ivo access revoked → Records Room description recast
7. Two of three bloc chains resolved + Charter found → endgame Standing available
8. All three blocs double-crossed → `ending_crossing_none` → Iron Raiders cameo location text appears, no combat forced
9. Home needs tick normally while player is at the Crossing (no second shelter simulation spun up)
10. Compile + EditMode PASS before "done"

---

# SECTION 9 — PLAYER ENGAGEMENT & RETENTION

## Day-one (post-unlock)

- Ostrowski's refusal to walk there himself — a rumour that is, unusually for this game, more inviting than most warnings.
- The viaduct sign: NO CHARTER NO GUARD ASK FOR SOMEONE. Read, not explained.
- The first weigh: a number that is just true, in a game where very few numbers about the player's own shelter have ever been neutral.

## 3–6 month roadmap (after S4)

| Month | Content | Why they return |
|---|---|---|
| M1 | Remaining side quests; backer-pool NPC barks; Nightfire rumour pack | The Standing is a repeatable systemic loop — players keep finding new disputes |
| M2 | A Current cameo gets a light quest hook (Scavenger Guild claim dispute expanded, or Deserter Coalition passage becomes recurring) — explicitly seeded, not required | Cross-Current interlock, same retention shape as Holdfast's Long Walk / Duty Roster's Second Winter |
| M3 | Second playthrough incentive: the Charter's *other* reading — Ivo has a second, uncatalogued box, found only if the first was handled with care | Rewards a Complete-shape playthrough with more, not different, content |
| M4–6 | Community: shareable Standing rulings (procedural from *their* backer list), ending second paragraphs. No live service, no canon vote | Occupancy of a ruling is personal, same principle as Duty Roster's roster |

## Feedback loops

| Loop | Need served |
|---|---|
| Weigh-run haul | Genuine fair trade, a rare feeling in this economy |
| The Standing | Agency over a rule, immediately visible, immediately contestable |
| Contract terms | Planning, risk, and the specific tension of having been told exactly what would happen |
| The Charter | Curiosity, then a quiet, deliberately anticlimactic payoff |
| Vouch | Relationship-as-infrastructure — trust with a mechanical floor under it |

## Monetization

Same as both sister packs: no microtransaction, no gacha, no loot boxes. If paid DLC: one purchase, bundled with or after Holdfast/Duty Roster.

---

# SECTION 10 — LORE CONSISTENCY CHECK

## 10.1 Must not contradict

| Canon | Source | Nobody's Charter stance |
|---|---|---|
| Sector 4 map closed; no fifth Power | `00_OVERVIEW.md` | The Crossing's blocs hold rulings, not ground; compliance argument stated in §5 and enforced per-quest |
| Additional factions are Currents, not Powers | `05_FACTIONS.md` | The three new blocs are *not* filed as Currents either — they are explicitly named in-fiction as something smaller and less durable than even a Current, living in their own catalog |
| Fourteen existing Currents, most dormant | `currents.json` | Five cameo (Deserter Coalition, Scavenger Guild, Iron Raiders, Grain Exchange, Sun-Seekers, Cold Count); none gets a questline here — reserved for future packs |
| Hatch-dilemma magnitudes | `ExpeditionSystem.cs` | Unchanged; extended only by trigger-point (viaduct), never by value |
| Highway 9 / Warlord territory | `WorldStateConsequenceSystem.cs`, `Location_TollHouse.cs` | Named as the road forty minutes from the Crossing, governed by the opposite theory of authority; not entered, not retuned |
| No magic, no real countries/people, no glorified violence | `AGENTS.md` | Held |
| The List / Schedule / Continuity canon | `02_THE_LIST.md`, Holdfast | Not touched. The Charter is a wholly separate, smaller, unrelated document — the bible is explicit that this is not a second List |
| Duty Roster's `ShelterEncounterSystem` / `MoraleMarkSystem` | Duty Roster plan | Hooked, not duplicated; viaduct-gate encounters use the same trigger/cooldown shape |

## 10.2 Explicit choices (not retcons — nothing existing is changed)

| Item | Note |
|---|---|
| `Location_TollHouse.cs` / Highway 9 | Referenced only as a geographic anchor ("forty minutes away"). No description, mutation, or value in that file is touched. |
| `faction_the_tally` | Name-checked once as a lore parallel to `LedgerDebtSystem`'s ethic. No shared code, no shared quest, no trust value modified. |
| `faction_scavenger_guild` | Its claim-blacklist rule is cited as the model Stallrow's chalk-marks imitate. Its own data is not modified. |

**Not retconned:** anything in `faction_lore.json`, any existing `Victory_*.cs`, any existing `Affliction_*.cs`, Holdfast or Duty Roster geography, The List, TrueEnding/Tessarat/Sector 7G/android content (all ignored, as always).

## 10.3 Timeline

| When | Event |
|---|---|
| Pre-war (decades before Exchange) | The real Charter signed: a weigh-scale calibration and toll-revenue-sharing compact between two county highway authorities |
| Exchange−? | Interchange 6 operates as an ordinary, minor, forgettable depot |
| Exchange+0 to +1 | The Warlords assess the site, decide it isn't worth holding, stop grading the approach road within a year |
| Exchange+1 | First ad-hoc trades begin; Osran (already the scale's caretaker) keeps weighing out of habit |
| Exchange+2 | First debt covered informally; the practice that becomes the Underwrite begins |
| Exchange+3 | The viaduct becomes foot-only in practice; Mattis (a child at Exchange+0) starts running messages across it |
| Exchange+4 | The Compact forms, mostly from people the other two arrangements have already failed once |
| Exchange+5 | **Now.** Draft Four is on the table. Wyn's second bad season is due. The player arrives. |

## 10.4 Base-game / sister-pack references (use them)

Ostrowski, `location_the_memory_vault` / Sole, `loc_low_background_lab` / Cold Count (*Current*, cameo), `loc_weighbridge`, `loc_conscription_office` / Pell, Highway 9 / Toll House (*existing, unmodified*), `faction_the_tally`, `faction_undertow`, `faction_grain_exchange`, `faction_scavenger_guild`, `faction_deserter_coalition`, `faction_iron_raiders`, `faction_sun_seekers` (all *existing Currents*, cameo only), Duty Roster's `ShelterEncounterSystem` / `MoraleMarkSystem` (hooked), Holdfast's Ice Road / Cluster / Order 12-C (referenced only as alternate vouch/flag sources in Appendix A, never rewritten).

## 10.5 Word to the implementer

If a system wants a fourth new class, a seventh Codex Power, a new `_hegemony` entry, or a retuned hatch constant, **stop and ticket it.** The expansion is a scale, a ledger, a petition, and three dry pages that turn out not to matter as much as everyone needed them to. That is enough.

---

# APPENDIX A — Integration matrix (sister packs ↔ Nobody's Charter)

## A.1 Holdfast / Duty Roster → Nobody's Charter

| Sister-pack flag / state | Nobody's Charter change |
|---|---|
| `holdfast_levy_refuse` or `alloc12_refused` | Alternate vouch path: Edor Vale (Holdfast) knows Mattis from Ice Road hauling seasons and will vouch once, drily, as a professional courtesy between people who wait on stools for a living |
| Duty Roster `flag_hadi_hidden` / Blank Rows access | A Blank Rows contact (*existing Current*) can vouch, on condition the player never writes their name anywhere at the Crossing either — the Charter's ledger becomes a second wall they refuse to be on |
| `Mutation_TransitTax` or `Mutation_MedicalSupplyGone` (base game) | Either qualifies as the "grievance a Power won't hear" story-gate on its own, no additional flag needed |
| Holdfast `holdfast_membrane_sector4` (Sector 4 stripped for District 8) | Wyn's forfeit terms are harsher this playthrough — the Underwrite prices scarcity accurately |
| Duty Roster `mutation_roster_ink` (community formally recognised at home) | Perrin's petition draft cites the roster, unprompted, as proof a written charter can work — small dialogue variant only |

## A.2 Nobody's Charter → Holdfast / Duty Roster

| Nobody's Charter mutation | Sister-pack change |
|---|---|
| `mutation_crossing_charter_revealed` | `location_the_memory_vault` gains a cross-reference entry; Sole's completeness thesis (Duty Roster) gets a small, satisfied dialogue variant — a record that turned out to be exactly as complete as it looked |
| `mutation_crossing_underwrite_burned` | Trade prices for the shelter tighten slightly at every Sector 4 market, not just the Crossing's — word travels about a defaulted debtor |
| `ending_crossing_none` | Ostrowski stops mentioning the Crossing at all in future playthroughs' rumour pool; a closed-market flavour line appears at `loc_weighbridge` |
| `mutation_crossing_forfeit_doublecrossed` | If Duty Roster is live, Nila Brant (Blank Rows) will not vouch for the player afterward — a debtor who "disappeared" is exactly the kind of written trail Blank Rows refuses to be near |
| `ending_crossing_compact` | Perrin's ratified charter becomes a discoverable comparison document at the Cluster Office (Holdfast) if that pack is live — Ormund reads it once, off-screen implied, and files it without comment |

## A.3 Two-way flag list (5) — parent summary

1. Grievance gates (TransitTax / MedicalSupplyGone / levy disputes) ↔ story-gate satisfaction, no new flag required.
2. Vouch source ↔ Edor Vale or a Blank Rows contact as alternates to Ostrowski.
3. Underwrite outcomes ↔ Sector 4-wide trade price texture.
4. Charter revelation ↔ Vault/Sole small dialogue payoff.
5. Collapse ending ↔ Ostrowski's rumour pool closes; `loc_weighbridge` flavour recast.

---

# APPENDIX B — Proposed id checklist (collision notes)

Verified non-colliding against `locations.json`, `currents.json`, `faction_lore.json`, `characters.json`, Holdfast and Duty Roster proposed ids **at time of writing**. Re-grep before implementation.

**Existing reused (unmodified):** `loc_weighbridge`, `loc_conscription_office`, `location_the_memory_vault`, `loc_low_background_lab`, `location_abandoned_convoy_yard`, `loc_diesel_tank_farm`, `loc_recovery_yard`, `faction_the_tally`, `faction_undertow`, `faction_grain_exchange`, `faction_scavenger_guild`, `faction_deserter_coalition`, `faction_iron_raiders`, `faction_sun_seekers`, `faction_cold_count`, `npc_bram_ostrowski`, `npc_sergeant_pell`, hatch constants, `Mutation_Highway9Cleared` (referenced, not modified).

**New (selected):** `expansion_nobodys_charter`, `region_crossing`, `faction_the_scale`, `faction_the_underwrite`, `faction_the_compact`, `npc_osran_kell`, `npc_dessa_vane`, `npc_perrin_ashby`, `npc_mattis_cray`, `npc_ivo_fenn`, `npc_wyn_sabler`, `loc_crossing_viaduct_gate`, `loc_crossing_scalehouse`, `loc_crossing_records_room`, `quest_crossing_the_vouch`, `quest_crossing_who_holds_the_ledger`, `crossing_arbitration_system`, `vouch_access_system`, `ledger_debt_system`, `mutation_crossing_charter_revealed`, `ending_crossing_scale`.

Full lists live in §§2–7. Do not mint a seventh `faction_lore.json` row. Do not add a Crossing entry to `WorldStateConsequenceSystem._hegemony`.

---

# APPENDIX C — Next prompt (implementation)

> Implement Sprint 1 of `docs/expansions/expansion_03_nobodys_charter_plan.md`: `VouchAccessSystem` (plain C#, events, save/load), JSON locations for the Scalehouse Row (`loc_crossing_viaduct_gate`, `loc_crossing_scalehouse`, `loc_crossing_stallrow`, `loc_crossing_watchtower`), quests `quest_crossing_the_vouch` / `_first_weigh`, NPCs Osran Kell and Mattis Cray, new catalog `crossing_factions.json` (`faction_the_scale`, `faction_the_underwrite`, `faction_the_compact` — Currents-shaped DTO, not added to `faction_lore.json`). Register new quest ids in `QuestlineSO.Ids`. Re-grep all new ids for collisions first. Verify Unity batch compile and EditMode tests. Cross-tool QA: reviewer is not the implementer (Prompt #26) — vouch state × future Standing backers × future debt terms is the coupled-variable set to watch even in Sprint 1's scaffolding.

---

# APPENDIX D — House-voice samples (shippable; more in the creative pack)

**`loc_crossing_viaduct_gate`**
> A rail truss over the Drown's edge, planked over for feet instead of axles. The paint on the sign has texture from how many times it's been redone: NO CHARTER NO GUARD ASK FOR SOMEONE. Someone added, smaller, underneath, in different paint: WE MEAN IT.

**`loc_crossing_scalehouse`**
> A truck scale built for loads nobody hauls anymore, kept calibrated for reasons that stopped being obvious around the same time the reasons stopped mattering less. Osran's office has one chair for him and none for you. He'll fetch a second one. He always fetches a second one.

**`loc_crossing_underwrite_hall`**
> A long table, a ledger chained to it — not against theft, Dessa will tell you, unprompted, the first time you ask. Against convenient memory. The fire is always lit. Somebody's interest paid for the wood.

**`item_charter_three_pages`** (inspect line)
> Three pages. A calibration tolerance, a revenue split, two signatures, a notary stamp. It says nothing about a town. It has been asked to mean a town for five years. It has never once agreed.
