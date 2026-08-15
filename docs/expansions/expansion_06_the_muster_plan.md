# ASHFALL — Expansion Design Bible & Godot-Native Integration Plan

**Title:** ASHFALL: THE MUSTER (THE FIFTEENTH CURRENT & THE VERGE RISING)
**Internal id:** `expansion_06_the_muster`
**Timeline Scope:** Day 180 to Day 360, with epilogue hooks extending past Day 360
**Companion Document:** `expansion_05_the_year_of_ash_plan.md` (environmental phases, door encounters, endgame epilogues) — this document does not restate that content
**Target Engine:** Godot 4.7+ (.NET/C#) Host + `Ashfall.Core` Engine-Agnostic Simulation
**Status:** Complete Design Bible & Master Architectural Specification (Revision 2 — expanded)
**Tone Lock:** Cold, exhausted, human, restrained. Specificity over adjectives. No line tells the player how to feel. No magic, no fantasy, no real countries/wars/people, no glorified violence, no supernatural adjudication. Humor, where it appears, is dry, situational, and character-earned — never played for slapstick.

---

# I. EXECUTIVE SUMMARY & SCOPE BOUNDARY

`expansion_05_the_year_of_ash` covers the environmental and faction-siege spine of Days 180–360: the Deep Freeze, the Total War phase, the Great Thaw, forty door encounters, and five endgame epilogues. It does this well and does not need to be re-litigated here.

This document is the **integration layer** — it takes six factions the game already promises in `currents.json` and never turns on, one leadership discontinuity the shipped Year-of-Ash content already introduced, and one commercial faction the shipped content already treats as real but the lore bible never named, and gives all three a Day-180+ mechanical and narrative payoff. This revision expands every one of those payoffs into a full branching questline and adds a consistent player-agency model across the whole document.

### The Approach system — how this document gives the player multiple ways to shape the story

Every major questline below (Sections II, III, V, VI) forks around a labeled set of **Approaches** — usually two to four. An Approach is not a dialogue-flavor choice. Each one:
- routes execution to a distinct branch of the relevant `NPC_*.cs` state machine,
- applies a distinct set of faction trust deltas,
- unlocks a distinct set of items, locations, or journal entries, and
- resolves to distinct prose, feeding the epilogue matrix in Section XII.

This mirrors a pattern the codebase already uses — `docs/lore/04_ENCOUNTERS.md`'s Ivor Lasko vote (returned / sheltered / abstained) and `QuestlineSystem.cs`'s existing surrender/refuse fork on `quest_garrison_blood_debt` — extended from a single fork per questline to two to four forks, consistently labeled, across every new questline in this document.

A second, complementary layer of player agency comes from a real, already-implemented system this document leans on directly: `Assets/Ashfall.Core/Journal/JournalSystem.cs` composes journal text through `JournalVoice.ComposeFullText(knowledgeKey, bias, day)`, keyed to the authoring survivor's `RiskBiasTrait` (`Paranoid`, `Cautious`, `Realist`, `Reckless`, `Denialist`, `Fatalist`, `Empath`, `Sociopath` — `Assets/Ashfall.Core/Journal/RiskBiasTrait.cs`). Its own doc comment states the design intent exactly: *"Same world state, different felt danger — this is what makes two survivors in the same bunker act differently."* Section III's Harven investigation is built to use this directly — the same discovery, journaled by three different survivors, reads as three different conclusions, and the game never adjudicates which one is correct. That ambiguity is not a gap in this design; it is the design, and it is why Section III has no Approach fork of its own — the fork already exists, one layer down, in whichever survivor the player sends to ask.

### What already exists and is NOT duplicated here

| Already shipped | Where | This document's relationship to it |
|---|---|---|
| 8 of 14 `currents.json` factions have working `NPC_*.cs` state classes (Archivists, Sun-Seekers, Osteophages, Lamplighters, Quiet House, Grain Exchange, Tally, Undertow) | `Assets/_Game/Factions/NPC_*.cs`, wired in `GameBootstrap.Currents.cs` | Section IV gives each a short Day-180+ vignette questline with a light two-way Approach fork, calling their real existing methods. No new class is written for these eight. |
| The Kittiwake Chart questline (`event_kittiwake_chart`, `NPC_Undertow.ChartDistributed()/OfferRescue()`) | `Assets/_Game/Factions/NPC_Undertow.cs`, `Assets/Tests/EditMode/KittiwakeChartEventTests.cs` | Fully implemented. Section IV.7 only adds a wartime timing pressure on top; the mechanic itself is untouched. |
| The Year-of-Ash catalog family (`year_of_ash_items.json`, `_events.json`, `_locations.json`, `_radio.json`, `_survivors.json`, `_quests.json`) — 30 locations, 36 items, 12 quests including `quest_garrison_blood_debt`, `quest_low_background_provenance`, and `quest_final_manifest_muster` | `Assets/StreamingAssets/Data/year_of_ash_*.json` | Reused directly wherever a new beat needs a site, an item, or a questline anchor. Note the name collision risk: `quest_final_manifest_muster` ("The Aurora Departure," the Northern Redoubt evacuation roll-call) is unrelated to **The Muster** as this document uses the term (the Deserter Coalition uprising, Section VI). Both use the word "muster" for unrelated reasons — one is a ship's manifest, one is a call to arms. This document does not rename either; it flags the coincidence so nobody debugs a phantom cross-reference later. |
| `npc_ivor_lasko`, the Day-40 deserter vote at `loc_grange_hall` | `docs/lore/04_ENCOUNTERS.md` | Named as the origin point of the Muster's political history in Section VI. Not rerun or altered. |
| The generic `faction_deserter_asylum` event and `DeserterSystem.cs` hatch-defection mechanic | `Assets/StreamingAssets/Data/events.json` (~L784), `Assets/_Game/Core/DeserterSystem.cs` | Explicitly disambiguated from the Muster in Section VI.0 — different system, different scope, left untouched. |
| `DesertersStandSystem.cs`, a static map-generated massacre-site discovery | `Assets/_Game/Core/DesertersStandSystem.cs` | Also disambiguated in Section VI.0. Unrelated to the Muster; not modified. |
| `JournalSystem.cs` / `JournalVoice.cs` / `RiskBiasTrait.cs` / `KnowledgeBase.cs` | `Assets/Ashfall.Core/Journal/` | Reused as the delivery mechanism for every ambiguous or trait-colored discovery in this document (Section III especially). No new journal system is written. |

### What is genuinely new in this document

1. **`faction_hydro_barons`**, a fifteenth Current, with a full four-Approach questline, "The Rate Card War" (Section II).
2. **The Colonel Harven succession**, investigated through three contradictory witness accounts and the real `JournalVoice` trait system rather than a single dialogue tree (Section III).
3. **Full mechanical activation and full branching questlines for the six Currents that exist only as flavor text today** — Cold Count, Deserter Coalition, Provisioned, Long Walk, Scavenger Guild, Iron Raiders (Section V).
4. **The Muster** — the Day 260+ uprising, expanded to four full player-directed campaign strategies (Section VI).
5. **The Ledger Nobody Signed** — a recurring mystery thread and story item spanning three questlines, resolved (deliberately, ambiguously) in Section VI.5.
6. A substantially expanded roster of new locations, items, and NPCs (Sections VII–IX).
7. A tighter, explicitly sustainable resource economy with a player-chosen Sector Charter (Section X).
8. A massively expanded lore section, including Approach-variant history entries (Section XI).
9. An **epilogue matrix** of eight named Day-360 outcomes, additive to `expansion_05`'s five (Section XII).

---

# II. THE FIFTEENTH CURRENT — COASTAL HYDRO-BARONS

`year_of_ash_locations.json` already contains `loc_hydro_baron_aqueduct_manifold`, `loc_hydro_baron_desal_plant_4`, and `loc_brine_pumping_sluice`. `year_of_ash_quests.json` already contains `quest_hydro_baron_aqueduct_sabotage`. Five door encounters and multiple events already treat this cartel as a real, distinct actor. None of that content is a territorial Power — Sector 4's map stays closed at four Powers per `docs/lore/00_OVERVIEW.md` — and none of it is one of the fourteen Currents in `currents.json` either. This section gives it a formal identity and a full questline.

### `currents.json` — new entry (append)

```json
{
  "id": "faction_hydro_barons",
  "display_name": "The Coastal Hydro-Barons",
  "alignment": "conditional",
  "home_region": "the_coast",
  "is_active": false,
  "trust": 0,
  "wants": ["brass_fittings", "corrosion_inhibitor", "labor_contracts"],
  "offers": ["potable_water_quota", "brine_byproduct_salt", "desalination_access"],
  "signature_quote": "We didn't poison the water. We just decided who drinks it first.",
  "access_rule": "Three desalination plants and one aqueduct manifold, inherited intact from a pre-Exchange utility contractor and never nationalised. They do not sell water. They sell a place in the queue.",
  "badge_asset_id": "faction_badge_hydro_barons"
}
```

`home_region: the_coast` is a new sixth region tag, distinct from the five Sector 4 sub-regions catalogued in `docs/lore/01_GAZETTEER.md`. `faction_badge_hydro_barons` needs a new badge asset — a valve-wheel-and-wave motif is consistent with the existing location iconography.

### Cast

- **Meret Odalen** — queue clerk. The public face of the rate card. Not a villain; extremely, unnervingly consistent about the price of moving up the list. Has never once raised her voice at a customer, which somehow makes the refusals worse.
- **Chief Engineer Praline Yurga** — runs Desalination Unit 4's actual machinery. Knows exactly how close the brine lines are to failing in a hard freeze and has stopped mentioning it to Odalen because the last three times she did, the answer was a memo about queue priorities, not repairs.
- **Collections Agent Dreth Iversen** — works the brine sluice. Polite, patient, and will absolutely walk away from a debtor's door without a word if that debtor stops answering — and note the exact day the visits stop, for later.
- **The rate card itself** — a laminated, six-year-old drought-surcharge pricing sheet from a pre-Exchange contractor referred to in company paperwork only as "the Halloway Concern." Nobody currently working the plants has ever met anyone from Halloway. Nobody has revised a single line on the card since.

### `quest_the_rate_card_war` — "The Queue"

**Stage 1 — The Line (Day 180+, unlocked on first visit to `loc_hydro_baron_aqueduct_manifold`).** Meret Odalen explains the queue with the flat patience of someone who has explained it four thousand times: pay in fittings, coolant, or labor, and your shelter moves up. Pay in nothing, and you drink whatever the catchment gives you. The player leaves with `item_hydro_baron_queue_chit` and a number.

**Stage 2 — The Crack in Unit 4 (Day ~195, gated on Deep Freeze Phase IV).** Chief Engineer Yurga, cornered at the plant during a routine delivery, admits off the record that a hard freeze below -30°C will crack the brine return line permanently, and that she's asked for the parts three times. This is leverage the player did not have before: the Hydro-Barons need something from the sector as badly as the sector needs them.

**Stage 3 — The Approach fork.**

- **Approach A — UNDERCUT.** Ally with the Grain Exchange (Section IV.6) to flood the barter market with cheaper brine-salt substitute, gutting the Hydro-Barons' secondary revenue stream until Odalen has no choice but to renegotiate the rate card downward. Slow, bloodless, and it costs the player nothing but time and Grain Exchange favor. *Outcome:* rate card revised to a flat, published price; `faction_hydro_barons` trust rises steadily; Grain Exchange trust rises sharply; Odalen keeps her job and never once thanks the player for costing her the leverage she'd built her whole career on.
- **Approach B — AUDIT.** Bring Halvard Ness and the Cold Count's instruments (Section V.1) to Unit 4 and test the "clean" quota water directly. It has been within tolerance the entire time — the Cold Count's honesty cuts both ways — but the *safety margin* on the desalination filters has been quietly narrowed for eleven months to hit quota targets. Going public forces immediate reform. *Outcome:* Yurga is promoted to plant administrator over Odalen's objection; pricing becomes transparent and regulated; Cold Count trust rises sharply; a one-time sector-wide `event_the_thin_margin_disclosure` fires, spiking short-term paranoia (a Denialist-voiced journal entry insists the water was always fine; a Paranoid one insists it never was) before settling.
- **Approach C — SEIZE.** Amid Phase V's total war, before Garrison or Ash Sign patrols can, take Desalination Unit 4 by force. Odalen and Yurga's fates depend on whether the player warns them first. Loot value is high and immediate; the queue system is destroyed in the process, triggering a temporary sector-wide water shortfall (`event_the_thirsty_season`, elevated danger rating for two weeks) that the player's own shelter is not exempt from. *Outcome:* the player becomes the plant's new, unaccountable administrator — the rate card doesn't get fairer, it gets renamed. `faction_hydro_barons`'s remaining staff either flee to the Long Walk's circuit (Section V.4, a standing rescue hook) or, if abandoned, don't.
- **Approach D — BROKER.** Bring the Tally (Section IV.7) in to formalize a three-way rotation among Garrison, Rebuilders, and the general population, backed by an enforceable written contract instead of Odalen's memory. Slowest path; requires simultaneous standing with three factions. *Outcome:* the most stable resolution in the game — water stops being a weapon anyone can wield unilaterally. Tally trust rises; the Hydro-Barons lose their pricing monopoly but keep their jobs and, for the first time, a union contract nobody can quietly amend.

**Stage 4 — Resolution.** Whichever Approach resolves, `item_hydro_baron_queue_chit` becomes either a collector's relic (Approach A/D, the queue system it once represented is gone or fixed) or a live currency (Approach C, now minted by the player). Feeds the epilogue matrix, Section XII.

---

# III. COLONEL HARVEN & THE CONTINUITY RECLAMATION DECREE

`QuestlineSystem.cs`'s `quest_garrison_blood_debt` (Days 185–260) and `year_of_ash_quests.json` both name the Iron Garrison's wartime commanding officer as **Colonel Harven**. The base game's faction lore names the Iron Garrison's commander as **Colonel Voss**, and `docs/lore/04_ENCOUNTERS.md` confirms Voss issued the standing deserter-execution order that governed the Day-40 Ivor Lasko vote. Both are real, both are already written, and there is no code fix required to reconcile them — only a placement in the timeline, and, this revision adds, a genuinely unresolved mystery around exactly how that placement happened.

**Canon anchor:** Voss commands the Iron Garrison through Day 239. The Continuity Reclamation Decree — already named in `expansion_05` Section II's Phase V timeline at Day 240 — is the moment the Garrison's central command structure is reorganised for total war. Colonel Harven's name appears on Garrison notices starting Day 240 with no announcement of a change of command.

### `quest_the_unsigned_order` — three witnesses, no verdict

This is not a fork with a "correct" branch. It is three separate, independently discoverable accounts, each triggered by sending a *specific survivor* to ask a *specific contact*, each composed through the real `JournalVoice` system so the same underlying event reads differently depending on who wrote it down. The player is never told which account is true, because nobody in the fiction knows either.

**Witness 1 — The Checkpoint Conscript**, encountered at `loc_garrison_checkpoint_gamma`. A boy barely old enough for the uniform, three drinks past careful, tells whichever survivor is listening that Voss was shot by his own staff for refusing a direct order to fire on a Rebuilder grain convoy. He says this like a confession, then immediately asks the survivor never to repeat it, which they can't, because it's already in the journal.

**Witness 2 — The Quartermaster**, encountered while trading at `loc_garrison_motor_pool`. Calm, bored, unbothered: Voss requested reassignment to the coastal evacuation liaison post at the outset of Phase V and nobody's heard from him since, which the quartermaster considers entirely unremarkable — officers rotate out of the worst postings all the time, and the coast (`loc_maritime_icebreaker_dock`, `loc_continental_convoy_staging_area`) is where the paperwork says he went.

**Witness 3 — The Signals Intercept**, discoverable only if the player has standing with Black Ops (D/9) and Signals Sergeant Anneke Ruhl (`docs/lore/06_REBUILDERS_AND_BLACK_OPS.md`) is willing to share a burst transmission her people flagged and never forwarded up the chain: an unregistered, low-power signal, three weeks after Day 240, using an authentication cipher retired with Voss's old command. It could be him. It could be someone using a code that was never properly decommissioned. Ruhl's own assessment, recorded verbatim in the entry: *"I don't chase ghosts. I log them."*

**Mechanical implementation:** each witness account is delivered via `JournalSystem.TryAddRawEntry(knowledgeKey, text, author, day)`, where `text` is pre-written per witness (not procedurally composed) but the *framing* sentence bracketing it is generated through `JournalVoice.ComposeFullText`, keyed to whichever survivor's `RiskBiasTrait` recorded it — a `Paranoid` author's framing leans toward the assassination account regardless of which witness they actually heard; a `Denialist` author's framing downplays whichever account they heard; a `Fatalist` shrugs at all three equally. This means two players who send different survivors to gather the same three witnesses end up with differently-weighted journals without a single line of new branching logic — the trait system does the work it was already built to do.

No door encounter, radio broadcast, or ending in this document or `expansion_05` ever states which witness was right. Harven's Garrison is measurably harsher than Voss's regardless: the Martial Allocation Authority water levy (Section II) tightens, and Harven is the antagonist whose ultimatum against Ola Vask (`quest_garrison_blood_debt`) is the spark that lights the Muster (Section VI). Whatever happened to Voss, Harven is who the player has to deal with now — which is, itself, the point being made.

---

# IV. DAY 180+ ESCALATION — ACTIVATING THE EIGHT

Each of these eight Currents already has a working `NPC_*.cs` state class with real methods. Nothing below adds a new class. Each now gets a short vignette questline — a hook, a complication, a light two-way Approach fork — gated on `Day >= 180`, that calls into the faction's existing behaviour under wartime pressure.

### 1. Archivists (`faction_archivists`, active) — "What the Vault Won't Lend"
Wartime scarcity means `power` and `deep_samples` (a want shared with Cold Count, Section V.1) start getting rationed at `loc_records_annex`. **Approach — Petition:** make the case in person, slow, costs nothing but time, occasional refusal. **Approach — Requisition:** use Garrison standing (if any) to pull rank on the Archivists' allocation, fast, permanently costs Archivist trust. No new mechanic — an existing access-gate check just evaluates against a harsher scarcity flag.

### 2. Sun-Seekers (`faction_sun_seekers`, active) — "The Short Days"
The existing `TickSunSeekersNightRule` interlock with Lamplighters already checks lamp coverage at night; Deep Freeze shortens usable daylight, so the check simply runs against a harsher `hour` window. **Approach — Ration light with them:** join their existing rationing discipline, small mutual morale gain. **Approach — Outbid them for fuel:** buy lamp oil out from under the Sun-Seekers' own supply chain; short-term gain, standing Sun-Seeker resentment.

### 3. Osteophages (`faction_osteophages`, active) — "A Larger Pool"
Winter mortality (frostbite, exposure) feeds their existing want/offer loop a larger input pool without changing its shape. **Approach — Look away:** let the existing offer economy run as designed. **Approach — Object:** raise it with them directly; they don't apologize, but they do, once, explain why the work matters — a rare moment of the Osteophages breaking their usual silence, delivered as a single new `event_osteophage_explanation` using the existing event schema.

### 4. Lamplighters (`faction_lamplighters`, dormant, has code) — "Doused by Order"
Harven's Phase V martial law (Section III) orders lamps doused near checkpoints. **Approach — Comply:** the existing lamp-state toggle flips dark near Garrison territory, Garrison trust protected. **Approach — Defy:** keep the lamps lit anyway; Sun-Seekers' night rule keeps working at full strength, but Garrison patrols start treating lit windows as a search trigger.

### 5. Quiet House (`faction_quiet_house`, dormant, has code) — "The Cost of a Favor Now"
Harven's checkpoints (Section III) make their existing quiet-favor economy riskier to use. **Approach — Pay the new premium:** a trust-cost multiplier, already exposed by the class, simply gets applied. **Approach — Ask them to stop operating near the Grid entirely:** the Quiet House relocates its favor economy to a lower-traffic region for the rest of the war — safer, slower, fewer favors available per season.

### 6. Grain Exchange (`faction_grain_exchange`, dormant, has code) — "The Floor"
Deep Freeze pushes their existing seasonal decline tick to a harder floor, reading the same `CropSO` water/hr field the player's own hydroponics use (Section X). **Approach — Share surplus:** if the player's own crops are healthy, donate the difference; Grain Exchange trust rises, and this is the exact leverage Section II's Approach A (Undercut) depends on. **Approach — Hold surplus:** keep it for the shelter; safer locally, no Hydro-Baron leverage gained.

### 7. Tally (`faction_the_tally`, dormant, has code) — "The Harven Premium"
Contracts written after Day 240 via `NPCTally.WriteContract()` carry a Harven-era risk premium — a data field, not a new method, reflecting how much harder collection is under total war. **Approach — Accept the premium:** contracts still get written, at worse terms. **Approach — Wait out the war:** no new Tally contracts until Day 320; safer terms later, no access to Section II's Approach D (Broker) until then.

### 8. Undertow (`faction_undertow`, dormant, has code) — "Bad Timing"
The Kittiwake Chart (`ChartDistributed()`, `salvageAccidentRisk` 0.1 → 0.5 on distribution) is complete and untouched. **Approach — Distribute before Day 240:** the accident-risk spike lands in calmer water. **Approach — Distribute during the siege:** the spike lands on top of already-elevated Iron Raiders danger (Section V.6), which the game warns the player about exactly once, in the Undertow's own dry voice, and then never mentions again.

---

# V. THE SIX SILENT CURRENTS — FULL MECHANICAL ACTIVATION & QUESTLINES

These six exist today as complete flavor text in `currents.json` — voice, wants, offers, access rule — and nothing else. This is the actual "new factions" deliverable: six state machines and six full branching questlines that finally let the game speak in the voice `currents.json` already wrote for them.

Every class follows the established pattern (`Assets/_Game/Factions/NPC_Tally.cs`, `NPC_Undertow.cs`, `NPC_TamsinRook.cs`): a serializable state struct, an `OnStateChanged` event, `CaptureState()`/`RestoreState()` for save parity, plain methods for the faction's core verb. All new classes live in `Assets/_Game/Factions/`, zero `UnityEngine` types beyond `[Serializable]`.

## 1. The Cold Count (`faction_cold_count`)

*The Spine · peaceful · wants power, shielding, deep samples · offers accurate rad readings, provenance analysis.*

Four researchers at `loc_low_background_lab` hold the isotopic proof of who fired the first shot. `quest_low_background_provenance` and "The Measured Truth" ending (`expansion_05` Section V.5) already exist; this section is the missing state machine between that quest id and that ending, plus a full prequel questline.

```
Assets/_Game/Factions/NPC_ColdCount.cs
├── NPC_ColdCountState
│   ├── string id = "npc_cold_count"
│   ├── bool isActive
│   ├── int powerSuppliedDays
│   ├── int shieldingDelivered
│   ├── bool provenanceDataComplete
│   ├── bool broadcastSent
│   └── float trust
├── SupplyPower(int days)
├── DeliverShielding(string itemId, int qty)
├── CompleteProvenanceRun()          // requires item_calibrated_mass_spectrometer_tube + powerSuppliedDays >= 30
└── TransmitFindings()               // sets broadcastSent, fires event_measurement_broadcast
```

### `quest_four_names_on_the_roster` — prequel, Day 180+

**Stage 1 — The Duty Roster.** Four researchers, none rotated since the Exchange: Halvard Ness (senior), and three others who get named the moment the player asks — the game does not withhold their names for drama, because the Cold Count doesn't operate on drama. Ness explains, flatly, that they did not evacuate when the order came, because moving the equipment without recalibration would have taken longer than anyone believed the war would last. Five years later, the equipment is still there. So are they.

**Stage 2 — The Approach fork.**
- **Approach A — Sustain them.** `SupplyPower()` and `DeliverShielding()` on a regular schedule; slow, materially costly, and the only path that lets `CompleteProvenanceRun()` succeed before Day 300, which matters for "The Measured Truth" ending's Day-360 window.
- **Approach B — Extract the data and leave.** Trade heavily up front for a partial reading, then stop supplying — Ness will not falsify anything either way, but an incomplete data set is exactly that: incomplete. `TransmitFindings()` can still fire, but the broadcast is caveated, and Harven's Garrison (which does not want the war's cause published at all) treats a caveated broadcast as far less credible — softer consequences, weaker payoff.

**Stage 3 — Transmission.** `TransmitFindings()` broadcasts on 142.850 MHz. This is the literal action "The Measured Truth" ending in `expansion_05` describes; this document supplies the system that makes it a decision instead of a flag.

## 2. The Deserter Coalition (`faction_deserter_coalition`)

Fully specified in Section VI — this is the Muster itself. Not duplicated here.

## 3. The Provisioned (`faction_the_provisioned`)

*The Grid · conditional · wants almost nothing · offers prewar stock, working pre-Exchange technology.*

```
Assets/_Game/Factions/NPC_Provisioned.cs
├── NPC_ProvisionedState
│   ├── bool isActive
│   ├── int respectScore
│   ├── bool haveMadeContact
│   └── List<string> unlockedTradeIds
├── OfferTrade(string proof)
├── RecordUnprompted(string kind)    // fired by OTHER systems when the player helps a third party
│                                     //   with no Provisioned benefit — Grain Exchange famine relief,
│                                     //   a free Long Walk escort, a Scavenger Guild claim honored
└── UnlockCache(string cacheId)      // gated on respectScore, never on goods handed over
```

### `quest_the_second_winter` — Day 190+

**Stage 1 — The Door That Answers.** At `loc_second_winter_homestead`, Quenna Brix opens the hatch, looks the player over for exactly as long as it takes to decide they're not a threat, and closes it again without a word. No dialogue. No trade offer. This is the entire first contact, and it is meant to be unsettling in how competent and unbothered it feels.

**Stage 2 — The Test Nobody Announces.** `respectScore` only rises via `RecordUnprompted()` calls from other systems — the player cannot buy their way in. The quest text makes this explicit the first time a player tries to trade directly: Brix's flat response, relayed secondhand through whoever the player sent, is *"Nobody helped us build it. I notice nobody's asking whether we'd like help now."* — the line already written into `currents.json`'s access rule, delivered in-fiction instead of just sitting in data.

**Stage 3 — Contact, on their terms.** Once `respectScore` crosses threshold, Brix meets the player in person for the first time. The trade offer, when it finally comes, is small on purpose — `item_prewar_diagnostic_scanner` and a handful of near-worthless wants (accurate forecasts, news) — because the point of the Provisioned was never their inventory. There is no Approach fork here; the entire questline is a single, patient Approach — *earn it, don't buy it* — and that singularity is the design, not an oversight.

## 4. The Long Walk (`faction_long_walk`)

*All regions · peaceful · wants water, footwear, news · offers unreachable-region goods, sector-wide situation report.*

```
Assets/_Game/Factions/NPC_LongWalk.cs
├── NPC_LongWalkState
│   ├── bool isActive
│   ├── string currentRegion         // cycles the_grid → the_verge → the_spine → the_toll → the_drown → the_coast → repeat
│   ├── int daysUntilDeparture = 1
│   └── Dictionary<string,float> lastKnownFactionTrust
├── DailyTick()                      // mirrors TravelingCaravanSystem.DailyTick()'s route-advance shape
├── TradeSupplies(water, footwear)
└── RequestSituationReport()         // returns a deliberately stale snapshot
```

### `quest_the_eleven_month_circuit` — Day 185+

**Stage 1 — First Crossing.** Osric Fane's group passes through whichever region the player's shelter sits in. They trade briefly, report what they know (already up to several weeks stale, and they say so unprompted), and leave before the second night, exactly as the access rule promises — the game keeps this promise mechanically, not just narratively, via `daysUntilDeparture`.

**Stage 2 — The Approach fork, next crossing (~60 days later).**
- **Approach A — Escort.** Send survivors to guard a leg of their circuit through a dangerous stretch (e.g., `loc_collapsed_valley_viaduct`). No payment accepted — but it's exactly the kind of unprompted help `NPC_Provisioned.RecordUnprompted()` listens for, and the Long Walk, unprompted in turn, starts sharing fresher intelligence on the next pass.
- **Approach B — Resupply only.** Trade water and footwear for goods and a report, nothing more. Perfectly sustainable, permanently at arm's length. Osric Fane remains cordial and permanently a stranger.

**Stage 3 — The Standing Circuit.** By Day 300 the Long Walk's `lastKnownFactionTrust` snapshot, however stale, is often the only sector-wide picture the player has left once Phase V communications degrade — this document's one deliberate answer to "how does the player know what's happening in regions they haven't personally visited in months."

## 5. The Scavenger Guild (`faction_scavenger_guild`)

*The Grid · conditional · wants claim respect, tools · offers richest salvage routes, apprenticeship.*

```
Assets/_Game/Factions/NPC_ScavengerGuild.cs
├── NPC_ScavengerGuildState
│   ├── bool isActive
│   ├── HashSet<string> claimedSiteIds
│   ├── HashSet<string> blacklistedShelterIds   // no removal method, ever — matches access_rule literally
│   └── float trust
├── ClaimSite(string locationId)
├── RecordOverStrip(string shelterId, string locationId)
└── IsBlacklisted(string shelterId)
```

### `quest_the_second_color_ledger` — Day 190+

**Stage 1 — The Guildhall.** At `loc_scavenger_guildhall`, Brannick Sten shows the player the two-color claim map without being asked, because it's the fastest way to explain the Guild's entire worldview in one gesture: claimed sites in the first color, blacklisted shelters in the second, and the second color is never crossed out.

**Stage 2 — The Approach fork.**
- **Approach A — Apprentice.** Take the Guild's training (`offers: apprenticeship`), get access to `claimedSiteIds` routing information — the richest salvage, first, every time — in exchange for a hard yield cap per site the player agrees to respect.
- **Approach B — Freelance.** Skip the apprenticeship, salvage wherever the player wants, at the standing risk that a single over-stripped claimed site puts the shelter's id into `blacklistedShelterIds` permanently. Higher short-term yield, catastrophic long-term risk, no warning shot.

**Stage 3 — The Permanent Ledger.** If `RecordOverStrip()` ever fires against the player's shelter, this document is explicit: there is no redemption arc written for it, anywhere. That absence is intentional and mirrors the Iron Raiders' "no offers" design below — some factions in this game do not forgive, and the fun is in knowing that going in, not being surprised by it after.

## 6. The Iron Raiders (`faction_iron_raiders`)

*The Toll · dangerous · wants what you have · offers nothing.*

```
Assets/_Game/Factions/NPC_IronRaiders.cs
├── NPC_IronRaidersState
│   ├── bool isActive
│   ├── float aggressionLevel        // reads the same wartime-tension value FactionWarSystem.cs
│   │                                  //   already tracks for the four territorial Powers
│   └── int raidsThisSeason
├── EvaluateRaidChance(float shelterVisibility, float aggressionLevel)
└── ExecuteRaid()                    // combat/loss event only — no dialogue tree by design
```

### `quest_nothing_to_offer` — not a diplomatic questline, Day 200+

There is no Approach fork here in the usual sense, because the Iron Raiders' entire design is the absence of one — `access_rule` is explicit that the absence of any offer *is* the design. What this document adds is player agency of a different kind: **preparation choices**, made before contact rather than during it.

- **Choice — Fortify `loc_iron_raiders_den`'s approach routes** (a defensive infrastructure investment) lowers `shelterVisibility` in `EvaluateRaidChance()`.
- **Choice — Do nothing** leaves `aggressionLevel` as the sole driver, meaning a bad Phase V siege week is also, unpredictably, a bad Iron Raiders week.
- **Choice — Provoke them deliberately** (raiding `loc_iron_raiders_den` first, for loot, before Day 240) is possible and survivable if the player is strong enough by then — the only faction in the roster where the player can choose to strike first with no narrative gate stopping them, and no faction anywhere in the game will think less of them for it, because nobody speaks for the Iron Raiders' feelings. This is deliberately the single most "video-gamey," consequence-light option in the whole document, included on purpose as a pressure release for players who want at least one faction relationship that's just combat.

---

# VI. THE MUSTER — AWAKENING THE DESERTER COALITION

### VI.0 — What this is not

Before any new content: this codebase already has three separate systems that touch "a soldier who ran." They are not the Muster, and the Muster does not replace or duplicate them.

1. **`faction_deserter_asylum`** (`events.json`, Day 20+) — a single generic event where an unnamed Garrison-affiliated deserter offers intel in exchange for shelter. One decision, one survivor tag (`garrison_deserter`), done.
2. **`DeserterSystem.cs` / `DeserterHUD.cs`** — a standing hatch mechanic: any hostile-faction soldier can defect at the door, 30% chance they're a plant, `DeserterCombatBonus = 15f` if kept and legitimate. Runs constantly, not tied to any faction narrative.
3. **`DesertersStandSystem.cs`** — a static, once-per-map environmental discovery describing a past civil-war massacre. Backstory, not an active faction.

The Muster is what happens when the sector's accumulated individual desertions stop being isolated incidents and become a coalition with a location, leadership, and the ability to fight. Mechanically, it is the activation of `faction_deserter_coalition`, dormant since it was written, home-region `the_verge` — precisely where Ivor Lasko hid in Day 40.

### VI.1 — The spark: Ola Vask

`quest_garrison_blood_debt` (Days 185–260) is fully written: Colonel Harven demands the player surrender survivor Ola Vask, on pain of embargo. This document does not alter that questline's existing stages; it gives its refusal branch a consequence beyond the player's own bunker for the first time.

**New hook, appended at the existing quest's refusal outcome:** if the player refuses to surrender Vask and survives the embargo, word reaches other Verge residents hiding the same kind of history — a shelter refused Harven and lived. This is the exact mechanism by which `faction_deserter_coalition` flips from `is_active: false` to `true`.

```
Assets/_Game/Factions/NPC_DeserterCoalition.cs
├── NPC_DeserterCoalitionState
│   ├── bool isActive
│   ├── int membersRallied            // starts at 1 (Vask)
│   ├── bool holdingGroundEstablished
│   ├── float garrisonLockoutRisk
│   ├── string chosenStrategy         // "political" | "military" | "railroad" | "informant" — see VI.4
│   └── float trust
├── RallyMember(string survivorSourceId)   // the ONE integration point with DeserterSystem.cs — routes a
│                                            //   legitimate hatch defection into membersRallied instead of
│                                            //   (or in addition to) becoming an ordinary survivor
├── EstablishHoldingGround(string locationId)
├── SetStrategy(string strategy)
└── ResolveMuster()
```

### VI.2 — Holding ground: reusing `loc_denial_cut_substation`

The Coalition's primary ground reuses the existing `loc_denial_cut_substation` — a reinforced railway culvert and transformer basement, already marked with D/9 civil-defense denial notation. No new primary location is authored for it. `loc_muster_treeline_camp` (Section VIII) exists as an overflow site once `membersRallied` exceeds what one exit can safely evacuate.

### VI.3 — Ola Vask, in her own words

Before the strategy fork, one scene: Vask, given the chance, tells the player plainly why she ran — not heroics, not ideology, a direct order to fire on a checkpoint line of civilians trying to cross into the Grid ahead of a supply cutoff, and a decision made in about four seconds that she has had five months to second-guess in a cellar. *"I'd like to say I thought about it. I didn't. My legs just wouldn't do the other thing."* This is the only characterization Vask gets beyond her existing questline text, and it's deliberately small — this document does not attempt to out-write `quest_garrison_blood_debt`'s own material, only to extend the world it opens onto.

### VI.4 — Four strategies (the Approach fork)

Once `faction_deserter_coalition` is active and holding ground, the player picks a `chosenStrategy` — not a one-time dialogue choice but a standing posture that shapes every subsequent beat through Day 320.

**Approach A — POLITICAL (Amnesty Campaign).** Rather than fight, build a case. Use Section III's ambiguous Harven material, testimony from Lasko-adjacent Verge residents, and — if pursued — Cold Count credibility (Section V.1) to petition for a formal amnesty. Slow, requires sustained multi-faction trust, and can fail outright if Harven's own position weakens too much to negotiate from (a genuine risk: political capital has a shelf life tied to the war's progress, tracked via `FactionWarSystem`'s existing tension value). *If it succeeds:* the Coalition is legally absorbed into a demobilized-conscript status, the first and only path in this document that ends the Coalition's fugitive status entirely rather than defending it.

**Approach B — MILITARY (The Standing Ground).** Arm them. `offers: patrol_schedules, weapon_maintenance, disciplined_fighters` flow back into the player's own defense — a direct combat-strength contribution, mirroring `DeserterCombatBonus` in `DeserterSystem.cs`. Harven treats the Coalition's existence as Open Rebellion regardless of the player's own Garrison standing. Counter-raids escalate through Phase V; survival to Day 320 is a real fight, not a formality.

**Approach C — RAILROAD (Nobody Stays).** Don't defend the ground — empty it. Use the Long Walk's circuit (Section V.4) and, if the Northern Redoubt ending's convoy prep is underway (`expansion_05` Section V.1), route rallied members out of Sector 4 entirely in small groups rather than concentrating them at one defensible site. Lower combat risk, slower, and it means the Coalition as a *standing local faction* never really exists — it becomes a corridor instead of a camp. This Approach deliberately trades the "faction uprising" the player might expect for something quieter and, arguably, more humane: nobody has to win a siege if there's no siege to have.

**Approach D — INFORMANT (The Blood Price).** The dark option, offered without judgment and without a moralizing narrator: report the Coalition's location to Harven in exchange for restored Garrison standing and a fuel/medical resupply. `garrisonLockoutRisk` drops to zero immediately. `membersRallied` — including Vask, if she's still with the player — is lost. No other faction in the sector ever finds out how, specifically, unless the player's own journal entries (again, trait-voiced — a `Sociopath`-authored entry records it as a transaction; an `Empath`-authored one does not get written at all, because that survivor refuses to put it in words) give it away. This Approach exists because the game's own tone lock insists it never tell the player how to feel about a choice, and pretending this option wasn't a real, playable branch would be exactly the kind of thumb-on-the-scale restraint the tone lock argues against.

### VI.5 — The Ledger Nobody Signed (recurring mystery thread)

Across three separate questlines — the Hydro-Barons' collections ledger (Section II, via Dreth Iversen), a Tally contract audit (Section IV.7), and a D/9 denial-cache manifest at `loc_d9_cache_bunker_delta` — the same six-character alphanumeric debt code appears, always marked **PAID**, always against an account nobody currently on staff at any of the three organizations can identify. It predates Harven's command. It may predate the Exchange. Chasing it down (an optional, unmarked thread stitched across all three questlines rather than its own standalone quest id) leads nowhere conclusive: Iversen has no record of who opened the account; the Tally's own archive shows the code was never one of theirs to begin with; the D/9 manifest entry is the oldest of the three by at least two years and is the only one of the three actually signed — with a single initial, no name. This document does not resolve it. It exists to reward the kind of player who reads collections notices and manifest ledgers closely, and to seed a genuine, unforced mystery for a future document to pick up or leave alone, exactly as the game already treats Voss's fate in Section III.

---

# VII. NEW NAMED NPCS

Every name below was checked against `NPC_TamsinRook.cs`, `NPC_DessaVane.cs`, `docs/lore/06_REBUILDERS_AND_BLACK_OPS.md`'s Vane, and `year_of_ash_survivors.json`'s `survivor_corporal_vane` / `survivor_felix_vane` for collisions. None reuse Vane, Rook, Doyle, or Tamsin.

| NPC | Current / Role | Notes |
|---|---|---|
| **Ola Vask** | Deserter Coalition spark | Already named in `quest_garrison_blood_debt`; extended, not renamed, per Section VI. |
| **Halvard Ness** | Cold Count, senior researcher | Delivers `TransmitFindings()`'s broadcast; the one who says the `currents.json` line, *"It's not a secret. It's a measurement."* |
| **Quenna Brix** | The Provisioned, contact point | Never asks first. `RecordUnprompted()` is her whole philosophy. |
| **Osric Fane** | The Long Walk, route-keeper | Tells the player his information is stale, every single time, unprompted. |
| **Brannick Sten** | Scavenger Guild, claims warden | Never explains the second color twice. |
| **Meret Odalen** | Coastal Hydro-Barons, queue clerk | The rate card's public face; the calmest person in this document. |
| **Chief Engineer Praline Yurga** | Coastal Hydro-Barons, plant technician | Knows the brine line will crack before Odalen admits it matters. |
| **Collections Agent Dreth Iversen** | Coastal Hydro-Barons, debt enforcement | Never raises his voice; the silence after he stops visiting is the actual threat. |
| **Ivor Lasko** *(cross-reference, not renamed)* | Historical origin point of Deserter Coalition politics | `docs/lore/04_ENCOUNTERS.md`, Day 40. Not revisited directly. |

---

# VIII. NEW & REUSED LOCATIONS

### Reused (no new location authored)
| Location id | Reused for |
|---|---|
| `loc_denial_cut_substation` | Deserter Coalition primary holding ground (Section VI.2) |
| `loc_low_background_lab` | Cold Count activation (Section V.1) — see `expansion_05` Addendum item 3 on this id's cross-catalog duplication risk |
| `loc_hydro_baron_aqueduct_manifold`, `loc_hydro_baron_desal_plant_4`, `loc_brine_pumping_sluice` | Coastal Hydro-Barons' three plants (Section II) |
| `loc_garrison_checkpoint_gamma`, `loc_garrison_motor_pool` | Harven-era martial law staging (Section III) |
| `loc_geothermal_well_alpha` | Grain Exchange irrigation tension (Section IV.6) |
| `loc_collapsed_valley_viaduct` | Long Walk's most treacherous regular crossing (Section V.4) |
| `loc_grange_hall` | Cited, not revisited — the historical site of the Lasko vote (Section VI.1) |
| `loc_maritime_icebreaker_dock`, `loc_continental_convoy_staging_area` | Witness 2's claimed Voss reassignment posting (Section III) |
| `loc_d9_cache_bunker_delta` | The Ledger Nobody Signed's third ledger entry (Section VI.5) |

### New
```json
[
  {
    "id": "loc_muster_treeline_camp",
    "displayName": "The Treeline Camp",
    "d": 6, "travelHours": 5, "rads": 30,
    "description": "A scatter of lean-tos under dead pine, chosen because the canopy still holds enough ash-snow to break a thermal signature. No fire after dark.",
    "lore": "Overflow ground for the Deserter Coalition once the substation fills past what one exit can evacuate."
  },
  {
    "id": "loc_second_winter_homestead",
    "displayName": "The Second Winter Homestead",
    "d": 3, "travelHours": 4, "rads": 15,
    "description": "A private shelter built into a hillside a decade before the Exchange, its blast door hand-fitted by someone who clearly expected to use it. It has been resupplied every winter since, by nobody the sector can identify.",
    "lore": "The Provisioned's home ground. The approach road suggests nothing. The door suggests thirty years of quiet competence."
  },
  {
    "id": "loc_scavenger_guildhall",
    "displayName": "The Scavenger Guildhall",
    "d": 4, "travelHours": 3, "rads": 20,
    "description": "A repurposed freight depot, its walls papered floor to ceiling with hand-drawn claim maps. Every claimed site is inked in one color; every blacklisted shelter's name is inked in a second color and never crossed out.",
    "lore": "Grid territory, but the Guild answers to no Power. The ledger on the second color is the whole of their law."
  },
  {
    "id": "loc_iron_raiders_den",
    "displayName": "The Cut",
    "d": 9, "travelHours": 6, "rads": 40,
    "description": "A collapsed rail cutting choked with burned-out freight cars, refitted as a den. There is no gate to knock on and no reason to try.",
    "lore": "The Toll's worst-kept and least visited secret. Nobody has ever come back with a description of the inside worth trusting."
  },
  {
    "id": "loc_the_tally_hall",
    "displayName": "The Tally Hall",
    "d": 2, "travelHours": 2, "rads": 12,
    "description": "A converted counting house, its walls lined with ledger boxes instead of shelving. Every contract The Tally has ever written is filed here, dated, and enforced on schedule.",
    "lore": "The Toll. Gives NPC_Tally's existing EnforceDueContracts() a physical home instead of an abstract state check."
  },
  {
    "id": "loc_amnesty_petition_hall",
    "displayName": "The Petition Hall",
    "d": 3, "travelHours": 3, "rads": 18,
    "description": "A converted rail depot waiting room, benches worn smooth, one clerk's window still staffed out of habit more than authority. This is where a case gets made, if a case can be made at all.",
    "lore": "Used only by Section VI.4's Political Approach — the physical site where the amnesty petition is filed and, eventually, answered."
  }
]
```

---

# IX. NEW ITEMS & STORY ITEMS

Naming follows the established `year_of_ash_items.json` convention: verbose, technical, no flourish.

| Item id | Ties to | Notes |
|---|---|---|
| `item_prewar_diagnostic_scanner` | The Provisioned | Available from no other source in the game; a genuine pre-Exchange artifact. |
| `item_deserter_coalition_forged_papers` | The Muster | Fulfills `faction_deserter_coalition`'s literal `wants: papers`; craftable via Quiet House favor (Section IV.5) or a Tally contract (Section IV.7). |
| `item_hydro_baron_queue_chit` | Coastal Hydro-Barons | Stamped brass token, proof of queue position; becomes either a relic or a live currency depending on Section II's Approach chosen. |
| `item_scavenger_guild_claim_marker` | Scavenger Guild | Placed at a site to formally claim it; the mechanical trigger for `ClaimSite()`. |
| `item_long_walk_route_ledger` | Long Walk | A physical copy of `RequestSituationReport()`'s output — re-readable without waiting for the next crossing. |
| `item_cold_count_provenance_seal` | Cold Count | Story item accompanying `TransmitFindings()`'s broadcast; referenced by "The Measured Truth" ending in `expansion_05` Section V.5. |
| `item_unsigned_debt_ledger_page` | The Ledger Nobody Signed | Story item — one of three matching pages, collected across Sections II, IV.7, and VI.5. Holding all three doesn't unlock a resolution; it unlocks a single new journal entry admitting, in the player's own trait-voiced hand, that there isn't one. |
| `item_amnesty_petition_dossier` | The Muster, Political Approach | Compiled testimony and evidence; the physical object Approach A's success or failure is checked against. |
| `item_garrison_manifest_forgery_kit` | The Muster, Informant/Railroad crossover use | A morally neutral tool — usable to forge Garrison transit papers for the Railroad Approach or to fabricate a false manifest entry that covers an Informant's tracks. The game does not gate which use is "allowed." |

No new item duplicates anything in the 36-entry `year_of_ash_items.json` roster; `item_calibrated_mass_spectrometer_tube`, `item_boron_shielding_tile`, and `item_lead_shielded_sample_cask` remain the Cold Count's core material inputs and are reused, not reinvented.

---

# X. SUSTAINABILITY ECONOMY — TIGHTER BUT ACHIEVABLE

The design goal is specific: tighter resource pressure, but a genuine, reachable path to self-sufficiency through trade, crops, and water — never a grind with no floor. Built entirely on real, already-implemented systems.

### The Sector Charter — a third layer of player agency

At any point after Day 200, the player may formally declare a standing economic posture. This is not a dialogue choice; it is a persistent modifier read by every trade-capable Current and Power, and it can be changed later at a cost (a cooldown, not a wall).

- **Self-Sufficiency Charter.** Shelter-internal `CropSO`/`WaterEconomySystem` output gets a small efficiency bonus; all external trade prices (caravans, Hydro-Barons, Grain Exchange) rise slightly. Rewards a player who has genuinely built out hydroponics and purification rather than trading for everything.
- **Open Trade Charter.** External trade prices drop across the board; internal production gets no bonus. Rewards a player leaning on `TravelingCaravanSystem`, the Long Walk, and the Hydro-Barons' queue.
- **Black Market Charter.** Unlocks off-book trade routes with the Scavenger Guild and, riskily, the Iron Raiders' fringes — best prices in the game, but every transaction has a small, compounding chance of tripping a Garrison search flag under Harven's checkpoints (Section III), independent of the player's actual Garrison standing.

### Water
`WaterEconomySystem.cs`'s three-tier model (irradiated → dirty → clean, via catchment and purifier) is untouched. The Hydro-Barons (Section II) become a second clean-water source priced in `item_hydro_baron_queue_chit` position rather than rations — a non-ration currency path that stays open even when the ration economy alone gets harder in Phase IV–V.

### Crops
`CropSO`'s existing fields govern hydroponics without change. Grain Exchange's seasonal decline tick (Section IV.6) reads the same water/hr field the player's own hydroponics use, so a shelter that has kept its own crops alive through the winter is, by the same math, a shelter the Grain Exchange finds easier to trade with — self-sufficiency and faction standing reinforce each other instead of competing for the same clock.

### Trade
`TravelingCaravanSystem`'s route/stay/inventory model is the direct template for `NPC_LongWalk.DailyTick()` (Section V.4), deliberately, so the player's existing mental model extends naturally. No new trade UI required.

### Forecasting
`FalloutForecastSystem`'s sensor-array upgrade path (level 1–3, horizon 4–6 days) is the input both the Provisioned's `almost_nothing` want and the Long Walk's situation reports implicitly reference — a shelter with a level-3 sensor array needs either faction's forecast less, an intentional soft cap: every new faction offers a shortcut around a real cost, never a resource with no other path to it.

---

# XI. MASSIVE LORE EXPANSION — WORLD HISTORY PAST DAY 180

New `world_history.json`-style entries, following the established "located knowledge" pattern (`discovery_location_id` / `discovery_trigger` / `knowledge_key`).

```json
[
  {
    "knowledge_key": "history_continuity_reclamation_decree",
    "discovery_location_id": "loc_garrison_checkpoint_gamma",
    "discovery_trigger": "day_240_reached",
    "text": "The notice board at Checkpoint Gamma is repapered overnight. The old bulletin, signed Voss, is gone. The new one is signed Harven and says nothing about why."
  },
  {
    "knowledge_key": "history_hydro_baron_rate_card_origin",
    "discovery_location_id": "loc_hydro_baron_aqueduct_manifold",
    "discovery_trigger": "first_visit",
    "text": "The rate card predates the Exchange by six years. It was a drought-season surcharge sheet from a contractor called the Halloway Concern. Nobody has revised a single line of it. The apocalypse changed what the water was worth; it did not change how the company decided who paid more."
  },
  {
    "knowledge_key": "history_deserter_coalition_founding",
    "discovery_location_id": "loc_denial_cut_substation",
    "discovery_trigger": "membersRallied_reaches_5",
    "text": "Someone has scratched a tally into the transformer housing — one mark per person who made it here and stayed. The first mark is dated to a fuel embargo that never technically happened, according to Garrison records. It happened."
  },
  {
    "knowledge_key": "history_cold_count_before_the_lab",
    "discovery_location_id": "loc_low_background_lab",
    "discovery_trigger": "provenanceDataComplete",
    "text": "Four names on the duty roster, none of them changed since the Exchange. They did not evacuate when the order came. Recalibration takes longer than anyone believed the war would."
  },
  {
    "knowledge_key": "history_the_provisioned_advance_knowledge",
    "discovery_location_id": "loc_second_winter_homestead",
    "discovery_trigger": "respectScore_threshold",
    "text": "The homestead's log predates the Allocation Schedule by three winters of stocked supply runs. Someone here believed this was coming with enough certainty to spend a decade preparing for it alone, and never once tried to sell that certainty to anyone who might have used it."
  },
  {
    "knowledge_key": "history_checkpoint_conscripts_confession",
    "discovery_location_id": "loc_garrison_checkpoint_gamma",
    "discovery_trigger": "witness_1_heard",
    "text": "He asked whoever was listening not to repeat it. It's already written down. That's not a betrayal, exactly. It's just what a journal is."
  },
  {
    "knowledge_key": "history_quartermasters_paperwork",
    "discovery_location_id": "loc_maritime_icebreaker_dock",
    "discovery_trigger": "witness_2_heard",
    "text": "Reassignment orders, coastal liaison, dated Day 240, signature illegible in the way every signature on this form is illegible. The paperwork is real. That isn't the same as the story being true."
  },
  {
    "knowledge_key": "history_the_intercepted_cipher",
    "discovery_location_id": "loc_d9_cache_bunker_delta",
    "discovery_trigger": "witness_3_heard",
    "text": "Ruhl's note, verbatim, appended to the transcript: 'I don't chase ghosts. I log them.' The cipher is real. Whoever is using it is not required to explain themselves to a filing cabinet."
  },
  {
    "knowledge_key": "history_the_ledger_nobody_signed",
    "discovery_location_id": "loc_the_tally_hall",
    "discovery_trigger": "third_ledger_page_collected",
    "text": "Three pages, one code, marked PAID in three different hands across what has to be years. Nobody currently employed by any of the three ledgers it appears in can say who opened the account. The oldest page is the only one signed — one initial, no name, and the ink has held up better than anything written since."
  }
]
```

---

# XII. THE EPILOGUE MATRIX — EIGHT DAY-360 OUTCOMES

Additive to `expansion_05` Section V's five endgame epilogues. The Day-360 evaluation now also reads `NPC_DeserterCoalition.chosenStrategy`, the Section II Hydro-Baron Approach taken, and whether Section III's mystery was ever pursued, to select among the following. These are not mutually exclusive with `expansion_05`'s five — several combine directly, noted below.

### 1. The Open Muster *(Military Approach, Coalition survives to Day 320)*
The substation held. Not because it was strong — because Harven's Garrison had a siege to run against three other fronts and could not spare the men to finish what a fuel embargo started. By Day 320 the tally scratched into the transformer housing runs past forty names. Nobody calls it a victory.

### 2. The Amnesty *(Political Approach, petition succeeds)*
The demobilization order is one paragraph long and reads like every other piece of Garrison paperwork, which is exactly what makes it real. Ola Vask signs her name to a conscript-status form instead of a wanted notice. Nobody throws a parade. Somewhere, a checkpoint conscript who talked too much once finds out his account of what happened to Voss made it, unattributed, into the case file that helped.

### 3. The Corridor *(Railroad Approach)*
There was never a siege because there was never a camp long enough to besiege. By Day 340 the last of the rallied deserters have moved through the Long Walk's circuit and out past the sector line, in twos and threes, carrying forged papers and not much else. The substation stands empty. Nobody scratches a final tally into the transformer housing, because nobody stayed long enough to think of it as theirs.

### 4. The Blood Price *(Informant Approach)*
The fuel arrives on schedule. The medical resupply arrives on schedule. Harven's notices about the Verge stop mentioning any coalition at all, because there wasn't one, not really, not for long. The player's own journal — if anyone wrote it down — is the only place any of this survives Day 360, in whichever survivor's hand happened to be holding the pen.

### 5. The Rate Card, Revised *(combines with Hydro-Barons Approach A or D)*
Pairs with any of the above. Water stops being a second front in whatever the player's main story became. A quiet, load-bearing footnote rather than its own ending.

### 6. The Administrator *(combines with Hydro-Baron Approach C — Seize)*
Pairs with any of the above, darker inflection. The player's shelter runs Desalination Unit 4 by Day 360, whether or not that was ever the plan going in.

### 7. The Measured Truth, Contested *(Cold Count Approach B used, provenance broadcast caveated)*
A variant on `expansion_05`'s existing "Measured Truth" ending: the broadcast goes out, but incomplete, and the regional commanders who wanted a reason to stop fighting anyway use it as one; the ones who didn't call it propaganda. The war ends the same way either version does — it just ends less cleanly, and the player's own Cold Count Approach (Section V.1) chose that shape months earlier without knowing it would matter this much.

### 8. Unwritten *(Section III never pursued, Muster never triggered)*
Voss's fate is never investigated. `quest_garrison_blood_debt` resolves exactly as `QuestlineSystem.cs` already has it, with no wider consequence. This is the fully valid "engage with none of this document" outcome, and it resolves cleanly into whichever of `expansion_05`'s five original epilogues the player's other choices produced — confirming, again, that nothing in this document is required scaffolding for the base Year-of-Ash experience.

---

# XIII. GODOT-NATIVE IMPLEMENTATION BLUEPRINT

```
Godot Host (Presentation & UI)
├── src/Muster/
│   ├── MusterHostSession.cs           (Coordinator wiring the six new + eight extended Current classes)
│   ├── CurrentsRosterWidget.cs        (Fifteen-Current status panel)
│   ├── ApproachSelectionModal.cs      (Generic Approach-fork UI, reused across Sections II/IV/V/VI —
│   │                                     one widget, driven by a data-defined list of Approach ids/labels/
│   │                                     descriptions per questline, not one bespoke UI per questline)
│   ├── DeserterCoalitionCampWidget.cs (Holding-ground status, membersRallied, chosenStrategy, garrisonLockoutRisk)
│   ├── JournalWitnessPanel.cs         (Renders Section III's three witness entries via the existing
│   │                                     JournalSystem/JournalVoice pipeline — no new text-composition logic)
│   └── MusterSaveStore.cs             (JSON persistence, sibling of YearOfAshSaveStore.cs)
└── Core Simulation (Ashfall.Core / Assets/_Game/Factions — plain C#, zero engine namespaces)
    ├── NPC_ColdCount.cs
    ├── NPC_Provisioned.cs
    ├── NPC_LongWalk.cs
    ├── NPC_ScavengerGuild.cs
    ├── NPC_IronRaiders.cs
    ├── NPC_DeserterCoalition.cs
    └── NPC_HydroBarons.cs             (mirrors NPC_Tally.cs's contract/ledger shape for the queue-chit economy)
```

### The Approach pattern, formalized

Every Approach fork in this document (Sections II, IV, V, VI) is implemented as the same small shape, avoiding one bespoke branching implementation per questline:

```
public enum QuestApproach { A, B, C, D }  // per-questline meaning documented in data, not in the enum

public interface IApproachQuestline
{
    void SelectApproach(QuestApproach approach);
    QuestApproach? SelectedApproach { get; }
    bool IsResolved { get; }
    string ResolveEndingKey();   // returns an ending-text id consumed by the epilogue matrix, Section XII
}
```

`NPC_HydroBarons`, `NPC_DeserterCoalition`, and the six Section V classes all implement this interface where they expose a genuine fork (Section V.3's Provisioned and Section V.6's Iron Raiders deliberately do not, per their own design notes above, and expose `IsResolved` without ever implementing a meaningful `SelectApproach`).

### Catalog changes
- `currents.json`: append `faction_hydro_barons`; flip `is_active` to `true` for the six Section V Currents once wired into `GameBootstrap.Currents.cs`'s `BootCurrents()`.
- `door_encounters.json` / `year_of_ash_radio.json`: one new entry for the Harven succession beat (Section III) plus witness-encounter entries.
- `world_history.json`: nine new entries (Section XI).
- `year_of_ash_items.json` gains the nine Section IX items appended to its existing 36.
- `year_of_ash_quests.json` gains `quest_the_rate_card_war`, `quest_the_unsigned_order`, `quest_four_names_on_the_roster`, `quest_the_second_winter`, `quest_the_eleven_month_circuit`, `quest_the_second_color_ledger`, `quest_nothing_to_offer` — all confirmed non-colliding against the existing 12-entry roster and against every other id in this repository at time of writing.

### GameBootstrap wiring
`GameBootstrap.Currents.cs`'s `BootCurrents()` log line ("Currents booted: 8 state classes initialised") becomes 15 once this document's classes are wired — the fifteenth, `faction_hydro_barons`, new to the roster entirely rather than an activation of an existing dormant entry.

---

# XIV. VERIFICATION PROTOCOL

1. `dotnet test Ashfall.Core.Tests` — new tests for each of the seven new/reused `NPC_*.cs` classes, following the existing `KittiwakeChartEventTests.cs` shape: construct, mutate, `CaptureState()`/`RestoreState()` round-trip, assert. Approach-forking classes additionally need one test per Approach value confirming `ResolveEndingKey()` returns a distinct, stable id.
2. `dotnet build Ashfall.csproj` — 0 errors, 0 warnings, no `UnityEngine`/`Godot` references inside any new `Assets/_Game/Factions/NPC_*.cs` file.
3. Regression check on `DeserterSystem.cs` and `DesertersStandSystem.cs` — confirm neither file requires modification; the Muster's only touch point is the additive `RallyMember()` call (Section VI.1).
4. Journal integration check — Section III's three witness entries must round-trip through `JournalSystem.CaptureState()`/`RestoreState()` and remain correctly attributed to their authoring survivor's `RiskBiasTrait` after a save/load cycle.
5. Cross-catalog id check — no new id introduced by this document collides with an existing id in `locations.json`, `holdfast_locations.json`, `year_of_ash_locations.json`, `year_of_ash_quests.json`, or any `NPC_*.cs`/`survivor_*` roster (spot-checked at authoring time; re-verify before merge, since `year_of_ash_quests.json` may have grown since).
6. Save/Load parity — Coalition, Cold Count, and Hydro-Baron state in particular (each gates at least one epilogue) must round-trip identically between Godot and Unity batch test harnesses.
7. Epilogue matrix check — all eight Section XII outcomes must be independently reachable and independently distinguishable in save data, with no combination silently collapsing into another's text.

---

# XV. NON-DUPLICATION LEDGER & SELF-REVIEW

- **Placeholder scan**: no TBD/TODO left in this document. Every new NPC, item, location, and quest id is concrete and spot-checked against the live repository.
- **Internal consistency**: Section VI.4's four strategies each terminate into a distinct, named epilogue (Section XII) or a clean fallback into existing systems (`DeserterSystem.cs`'s survivor pool, for the Informant/lost-members case) — no dead-end state. Section III's three witnesses are consistent with each other in tone (none is written as obviously the "real" one) even though their content conflicts, which is the intended effect, not an error.
- **Scope check**: this document adds one new Current (Hydro-Barons) with a four-Approach questline, activates six dormant Currents with full questlines, extends eight active ones with light vignettes, and adds a mystery thread, a Sector Charter economic layer, and an eight-outcome epilogue matrix. It does not touch combat balancing, save format versioning, or the four territorial Powers' core diplomacy model.
- **Ambiguity check**: Section III and Section VI.5 are deliberately, permanently unresolved — this is a tone-lock-consistent design choice, not an unfinished one, and both are explicitly labeled as such rather than left to look like oversights.
- **Approach-count audit**: Hydro-Barons (4), Muster (4), Cold Count (2), Long Walk (2), Scavenger Guild (2), Provisioned (1, by design), Iron Raiders (0 diplomatic / 3 preparation choices, by design), the Eight (2 each) — every fork in this document was checked to confirm it produces mechanically distinct outcomes, not reskinned duplicates of the same result.
- **Known open item carried forward**: the `loc_low_background_lab` cross-catalog id collision (three files, same id) remains unresolved at the data-integrity level; this document reuses that id rather than redefining it, and does not worsen it.
