# ASHFALL — Expansion Design Bible & Godot-Native Integration Plan

**Title:** ASHFALL: THE MUSTER (THE FIFTEENTH CURRENT & THE VERGE RISING)
**Internal id:** `expansion_06_the_muster`
**Timeline Scope:** Day 180 to Day 360, with epilogue hooks extending past Day 360
**Companion Document:** `expansion_05_the_year_of_ash_plan.md` (environmental phases, door encounters, endgame epilogues) — this document does not restate that content
**Target Engine:** Godot 4.7+ (.NET/C#) Host + `Ashfall.Core` Engine-Agnostic Simulation
**Status:** Complete Design Bible & Master Architectural Specification
**Tone Lock:** Cold, exhausted, human, restrained. Specificity over adjectives. No line tells the player how to feel. No magic, no fantasy, no real countries/wars/people, no glorified violence, no supernatural adjudication.

---

# I. EXECUTIVE SUMMARY & SCOPE BOUNDARY

`expansion_05_the_year_of_ash` covers the environmental and faction-siege spine of Days 180–360: the Deep Freeze, the Total War phase, the Great Thaw, forty door encounters, and five endgame epilogues. It does this well and does not need to be re-litigated here.

This document is not a second copy of that spine. It is the **integration layer** — the layer that takes six factions the game already promises in `currents.json` and never turns on, one leadership discontinuity the shipped Year-of-Ash content already introduced, and one commercial faction the shipped content already treats as real but the lore bible never named, and gives all three a Day-180+ mechanical and narrative payoff. Everything genuinely new in this document exists to close a gap that was already visible in committed or already-authored data — not to invent a parallel game.

### What already exists and is NOT duplicated here

| Already shipped | Where | This document's relationship to it |
|---|---|---|
| 8 of 14 `currents.json` factions have working `NPC_*.cs` state classes (Archivists, Sun-Seekers, Osteophages, Lamplighters, Quiet House, Grain Exchange, Tally, Undertow) | `Assets/_Game/Factions/NPC_*.cs`, wired in `GameBootstrap.Currents.cs` | Section IV gives each a short Day-180+ escalation hook that calls their real existing methods. No new class is written for these eight. |
| The Kittiwake Chart questline (`event_kittiwake_chart`, `NPC_Undertow.ChartDistributed()/OfferRescue()`) | `Assets/_Game/Factions/NPC_Undertow.cs`, `Assets/Tests/EditMode/KittiwakeChartEventTests.cs` | Fully implemented. Section IV.7 only adds a wartime timing pressure on top; the mechanic itself is untouched. |
| The Year-of-Ash catalog family (`year_of_ash_items.json`, `_events.json`, `_locations.json`, `_radio.json`, `_survivors.json`, `_quests.json`) — 30 locations, 36 items, 12 quests including `quest_garrison_blood_debt` and `quest_low_background_provenance` | `Assets/StreamingAssets/Data/year_of_ash_*.json` | Reused directly wherever a new beat needs a site, an item, or a questline anchor. Section VIII lists every reuse explicitly rather than inventing parallel content. |
| `npc_ivor_lasko`, the Day-40 deserter vote at `loc_grange_hall` | `docs/lore/04_ENCOUNTERS.md` | Named as the origin point of the Muster's political history in Section VI. Not rerun or altered. |
| The generic `faction_deserter_asylum` event and `DeserterSystem.cs` hatch-defection mechanic | `Assets/StreamingAssets/Data/events.json` (~L784), `Assets/_Game/Core/DeserterSystem.cs` | Explicitly disambiguated from the Muster in Section VI.0 — different system, different scope, left untouched. |
| `DesertersStandSystem.cs`, a static map-generated massacre-site discovery | `Assets/_Game/Core/DesertersStandSystem.cs` | Also disambiguated in Section VI.0. Unrelated to the Muster; not modified. |

### What is genuinely new in this document

1. **`faction_hydro_barons`**, a fifteenth Current, formally defined to match content the shipped Year-of-Ash catalogs already treat as real (Section II).
2. **The Colonel Harven succession** — a Day 240 leadership-transition beat that reconciles the Year-of-Ash quest data's "Colonel Harven" against the base game's "Colonel Voss" without touching either implementation (Section III).
3. **Full mechanical activation for the six Currents that exist only as flavor text today** — Cold Count, Deserter Coalition, Provisioned, Long Walk, Scavenger Guild, Iron Raiders (Section V).
4. **The Muster** — the Day 260+ uprising, built as the activation story for the already-defined-but-dormant `faction_deserter_coalition`, synthesizing four pre-existing scattered deserter threads into one coalition with a location, a leadership roster, and a war outcome (Section VI).
5. A small set of genuinely new locations, items, and NPCs that fill real gaps rather than reproducing existing ones (Sections VII–IX).
6. A tighter, explicitly sustainable resource economy layered on the real `WaterEconomySystem`, `CropSO`, `TravelingCaravanSystem`, and `FalloutForecastSystem` (Section X).
7. A sixth Day-360 epilogue, additive to `expansion_05`'s five (Section XII).

---

# II. THE FIFTEENTH CURRENT — COASTAL HYDRO-BARONS

`year_of_ash_locations.json` already contains `loc_hydro_baron_aqueduct_manifold`, `loc_hydro_baron_desal_plant_4`, and `loc_brine_pumping_sluice`. `year_of_ash_quests.json` already contains `quest_hydro_baron_aqueduct_sabotage`. Five door encounters and multiple events already treat this cartel as a real, distinct actor. None of that content is a territorial Power — Sector 4's map stays closed at four Powers per `docs/lore/00_OVERVIEW.md` — and none of it is one of the fourteen Currents in `currents.json` either. It has been operating in the game's shipped data with no formal faction identity. This section gives it one, using the exact schema every other Current already uses.

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

`home_region: the_coast` is a new sixth region tag, distinct from the five Sector 4 sub-regions (The Grid, The Verge, The Spine, The Toll, The Drown) catalogued in `docs/lore/01_GAZETTEER.md`. `year_of_ash_locations.json` already implies this geography ("Sector 4, Sector 8, and the Northern Coast" — see its own catalog overview in `expansion_05_the_year_of_ash_plan.md` Section IV.3); `the_coast` names it formally so `HoldfastMapSeeder.cs`-style region tagging has a real value to assign these three locations.

`faction_badge_hydro_barons` does not exist yet. Unlike the fourteen Currents, which each map to an already-drawn but unused badge under `Assets/Resources/Art/Factions/` per the "orphaned badge" table in `docs/lore/05_FACTIONS.md`, this is the one Current in this document that needs a new badge asset commissioned or adapted from existing art (a valve-wheel-and-wave motif is consistent with the desalination/aqueduct iconography already implied by the location names).

### Lore

The Hydro-Barons are not villains and not allies. They are a monopoly that was never dismantled because dismantling it would have meant nobody's taps worked for the six months it took to figure out who should run it instead. Three plants, one manifold, a payroll of maybe forty people who know how reverse osmosis membranes actually fail, and a ledger that has never once balanced in a shelter's favor. Their rate card is public and unapologetic: labor and parts move you up the queue, complaints do not.

**Interlocks:**
- **Iron Garrison** — the Martial Allocation Authority (established in `expansion_05` Section II Phase V) leans on the Hydro-Barons for bulk water to supply the siege lines. When Garrison trust is High, Hydro-Baron quota prices drop 15%; when Garrison is in Open Rebellion against the player (see Section VI), the Hydro-Barons cut the player's queue position to punish anyone the Garrison has blacklisted.
- **The Rebuilders** — the Allotments' glasshouse irrigation (`loc_allotment_glasshouse_complex`, per `expansion_05` Section IV.3) draws on the same aqueduct manifold. A standing tension: every acre the Rebuilders reclaim is water the Hydro-Barons did not sell.
- **Grain Exchange** (`faction_grain_exchange`, Section IV.4 below) — undercuts the Hydro-Barons' brine-salt byproduct on the barter market whenever a harvest is good, which the Hydro-Barons resent openly and do nothing about, because the Grain Exchange doesn't own a desalination plant.

---

# III. COLONEL HARVEN & THE CONTINUITY RECLAMATION DECREE

`QuestlineSystem.cs`'s `quest_garrison_blood_debt` (Days 185–260) and `year_of_ash_quests.json` both name the Iron Garrison's wartime commanding officer as **Colonel Harven**. The base game's faction lore names the Iron Garrison's commander as **Colonel Voss**, and `docs/lore/04_ENCOUNTERS.md` confirms Voss issued the standing deserter-execution order that governed the Day-40 Ivor Lasko vote. Both are real, both are already written, and there is no code fix required to reconcile them — only a placement in the timeline.

**Canon:** Voss commands the Iron Garrison through Day 239. The Continuity Reclamation Decree — already named in `expansion_05` Section II's Phase V timeline at Day 240 — is the moment the Garrison's central command structure is reorganised for total war. Voss does not survive the reorganisation in a form the record is precise about; the Garrison's own communications are deliberately vague, and the player never gets a clean answer. Colonel Harven's name appears on Garrison notices starting Day 240 with no announcement of a change of command, which is itself the point: this is a faction that does not explain itself to civilians.

**Mechanical hook (new, small):** a single door encounter or radio broadcast fired at Day 240, gated on Iron Garrison relationship existing at all, that a player who has been trading with the Garrison notices without being told outright — a supply manifest stamped with an unfamiliar signature, a checkpoint guard who flinches at "the Colonel" and doesn't correct which one. This does not require a new system; it is a single entry appended to `door_encounters.json` or `year_of_ash_radio.json`, using the existing schema. No existing Voss- or Harven-referencing content is altered.

Harven's Garrison is measurably harsher than Voss's: the Martial Allocation Authority water levy (Section II) tightens, and Harven is the antagonist whose ultimatum against Ola Vask (`quest_garrison_blood_debt`) is the spark that lights the Muster (Section VI).

---

# IV. DAY 180+ ESCALATION — ACTIVATING THE EIGHT

Each of these eight Currents already has a working `NPC_*.cs` state class with real methods. Nothing below adds a new class. Each hook is a single new trigger point, gated on `Day >= 180`, that calls into the faction's existing behaviour under wartime pressure. This is the "tighter resource management" the player feels: every faction that used to be a side conversation now has a reason to matter more once the war and the winter both bite.

### 1. Archivists (`faction_archivists`, active)
Wartime hook: `power` and `deep_samples` (shared want vocabulary with Cold Count, Section V.1) become scarce enough that the Archivists start rationing access to `loc_records_annex`. No new mechanic — an existing access-gate check simply starts evaluating true more often as global scarcity flags rise.

### 2. Sun-Seekers (`faction_sun_seekers`, active)
The existing `TickSunSeekersNightRule` interlock with Lamplighters (`GameBootstrap.Currents.cs`) already checks lamp coverage at night. Deep Freeze (`expansion_05` Phase IV, -25°C to -45°C) shortens usable daylight; the same tick just runs against a harsher `hour` window, no new code, only a data-driven seasonal daylight table already implied by `WeatherSystem`.

### 3. Osteophages (`faction_osteophages`, active)
Winter bone/marrow scarcity — feed the existing want/offer loop from the Deep Freeze's increased mortality rate (frostbite, exposure). No new class; the input to their existing offer calculation just has a larger pool to draw from during Phase IV–V.

### 4. Lamplighters (`faction_lamplighters`, dormant, has code)
Wartime blackout discipline: Iron Garrison's Phase V martial law (Section III) orders lamps doused near checkpoints. `NPC_Lamplighters`'s existing lamp-state toggle gets a new wartime-forced-dark flag, read by the Sun-Seekers interlock above.

### 5. Quiet House (`faction_quiet_house`, dormant, has code)
The Grid's home-region alignment with Iron Garrison martial law makes their existing quiet-favor economy riskier to use once Harven's checkpoints (Section III) go up. No mechanic change; a trust-cost multiplier keyed to Garrison relationship state, which the class already exposes.

### 6. Grain Exchange (`faction_grain_exchange`, dormant, has code)
Their existing seasonal decline tick gets a harder floor during the Deep Freeze — hydroponic yield drops per `CropSO`'s real water/hr and growth-hour fields (Section X), which the Grain Exchange's barter offer already reads from.

### 7. Tally (`faction_the_tally`, dormant, has code)
`NPCTally.WriteContract()` / `EnforceDueContracts()` already model debt. Wartime hook: contracts written after Day 240 carry a Harven-era risk premium (a data field, not a new method) reflecting how much harder collection is under total war.

### 8. Undertow (`faction_undertow`, dormant, has code)
The Kittiwake Chart (`ChartDistributed()`, `salvageAccidentRisk` 0.1 → 0.5 on distribution) is complete. The only new content: distributing the chart during Phase V, when Iron Raiders activity (Section V.6) is already elevated, means the accident-risk spike lands on top of raider danger instead of calm water — a **timing decision**, not a new mechanic. The design note for this document's questline text: warn the player, once, that distributing the chart during the siege is a worse trade than distributing it before Day 240.

---

# V. THE SIX SILENT CURRENTS — FULL MECHANICAL ACTIVATION

These six exist today as complete flavor text in `currents.json` — voice, wants, offers, access rule — and nothing else. No `NPC_*.cs` class, no `is_active: true`, no tick registration in `GameBootstrap.Currents.cs`. This is the actual "new factions" deliverable: six state machines that finally let the game speak in the voice `currents.json` already wrote for them.

Every class below follows the established pattern (`Assets/_Game/Factions/NPC_Tally.cs`, `NPC_Undertow.cs`, `NPC_TamsinRook.cs`): a serializable state struct, an `OnStateChanged` event, `CaptureState()`/`RestoreState()` for save parity, plain methods for the faction's core verb. All new classes live in `Assets/_Game/Factions/`, zero `UnityEngine` types beyond `[Serializable]`, following the same dual-engine boundary as their siblings.

## 1. The Cold Count (`faction_cold_count`)

*The Spine · peaceful · wants power, shielding, deep samples · offers accurate rad readings, provenance analysis.*

Four researchers at `loc_low_background_lab` (already in both `docs/lore/03_LOCATIONS.md` and `year_of_ash_locations.json`) hold the isotopic proof of who fired the first shot, and the already-defined `quest_low_background_provenance` / "The Measured Truth" ending (`expansion_05` Section V.5) depend on this faction actually existing as a system, not just an ending condition. This class is the missing link between that quest id and that ending.

```
Assets/_Game/Factions/NPC_ColdCount.cs
├── NPC_ColdCountState
│   ├── string id = "npc_cold_count"
│   ├── bool isActive
│   ├── int powerSuppliedDays        // consecutive days the player has kept the lab powered
│   ├── int shieldingDelivered       // item_boron_shielding_tile / item_lead_shielded_sample_cask units
│   ├── bool provenanceDataComplete  // gates quest_low_background_provenance completion
│   ├── bool broadcastSent           // 142.850 MHz transmission, gates Measured Truth ending
│   └── float trust
├── SupplyPower(int days)            // increments powerSuppliedDays, resets on gap
├── DeliverShielding(string itemId, int qty)
├── CompleteProvenanceRun()          // requires item_calibrated_mass_spectrometer_tube + powerSuppliedDays >= 30
└── TransmitFindings()               // sets broadcastSent, fires event_measurement_broadcast (new, Section IX)
```

Access rule stays exactly as written: they will not falsify the data for the player, in either direction. `CompleteProvenanceRun()` cannot fail toward a flattering result — only toward "not enough data yet."

## 2. The Deserter Coalition (`faction_deserter_coalition`)

Fully specified in Section VI — this is the Muster itself. Not duplicated here.

## 3. The Provisioned (`faction_the_provisioned`)

*The Grid · conditional · wants almost nothing · offers prewar stock, working pre-Exchange technology.*

A private shelter that predates the Continuity Allocation Schedule. They are not hostile and not warm; they are the sector's only source of items no catalog anywhere in this codebase currently defines — genuine pre-war technology, not scavenged and refurbished ordnance.

```
Assets/_Game/Factions/NPC_Provisioned.cs
├── NPC_ProvisionedState
│   ├── bool isActive
│   ├── int respectScore             // earned only through non-transactional acts, see below
│   ├── bool haveMadeContact
│   └── List<string> unlockedTradeIds
├── OfferTrade(string proof)         // "almost nothing" — a small set of near-worthless wants:
│                                     //   accurate weather forecasts (FalloutForecastSystem output),
│                                     //   news the Long Walk (below) would also carry, nothing material
├── RecordUnprompted(string kind)    // player helped a third party with no Provisioned benefit; raises respectScore
└── UnlockCache(string cacheId)      // gated on respectScore threshold, not on goods handed over
```

Design intent: this is the one Current in the whole roster whose economy cannot be bought into. `respectScore` only rises from `RecordUnprompted` calls fired by other systems (a Grain Exchange famine relief action taken with no Provisioned involvement, a Long Walk caravan escorted for free) — the Provisioned are watching the sector's behaviour, not its inventory. New item: `item_prewar_diagnostic_scanner` (Section IX), available nowhere else.

## 4. The Long Walk (`faction_long_walk`)

*All regions · peaceful · wants water, footwear, news · offers unreachable-region goods, sector-wide situation report.*

Thirty-odd people on an eleven-month circuit of Sector 4. They are the game's only mobile information source that is not a `TravelingCaravanSystem` merchant — they don't sell, they report, and they leave.

```
Assets/_Game/Factions/NPC_LongWalk.cs
├── NPC_LongWalkState
│   ├── bool isActive
│   ├── string currentRegion         // cycles the_grid → the_verge → the_spine → the_toll → the_drown → the_coast → repeat
│   ├── int daysUntilDeparture = 1   // they never stay a second night — hard-coded rule from access_rule text
│   └── Dictionary<string,float> lastKnownFactionTrust  // per-region trust snapshot, up to ~11 months stale
├── DailyTick()                      // reuses the exact route/waypoint-advance pattern already proven in
│                                     //   TravelingCaravanSystem.DailyTick() — same shape, six-node loop
├── TradeSupplies(water, footwear)   // simple barter, priced like TravelingCaravanSystem.TryBuyItem
└── RequestSituationReport()         // returns lastKnownFactionTrust snapshot — deliberately stale information,
                                      //   which is the point: the Long Walk's news is honest but old
```

`DailyTick()` deliberately mirrors `TravelingCaravanSystem.DailyTick()`'s route-advance shape rather than inventing a new pattern — this is the kind of proven internal precedent the codebase already establishes.

## 5. The Scavenger Guild (`faction_scavenger_guild`)

*The Grid · conditional · wants claim respect, tools · offers richest salvage routes, apprenticeship.*

```
Assets/_Game/Factions/NPC_ScavengerGuild.cs
├── NPC_ScavengerGuildState
│   ├── bool isActive
│   ├── HashSet<string> claimedSiteIds
│   ├── HashSet<string> blacklistedShelterIds   // permanent once entered — matches access_rule exactly
│   └── float trust
├── ClaimSite(string locationId)      // marks a site as Guild-claimed; player strip-mining a claimed site
│                                      //   past its yield threshold is what triggers blacklist, not theft itself
├── RecordOverStrip(string shelterId, string locationId)  // called from the loot/salvage system when a
│                                      //   claimed site's yield hits zero from a single shelter's hauls
└── IsBlacklisted(string shelterId)   // permanent — the class enforces this by never removing an id once added
```

Design intent honors the access rule literally: `blacklistedShelterIds` has no removal method. This is the one Current whose punishment the player cannot buy, apologize, or grind their way out of — matching "the blacklist is honoured across the whole Guild, permanently."

## 6. The Iron Raiders (`faction_iron_raiders`)

*The Toll · dangerous · wants what you have · offers nothing.*

```
Assets/_Game/Factions/NPC_IronRaiders.cs
├── NPC_IronRaidersState
│   ├── bool isActive
│   ├── float aggressionLevel        // rises with sector-wide desperation (Deep Freeze, Phase V tension)
│   └── int raidsThisSeason
├── EvaluateRaidChance(float shelterVisibility, float aggressionLevel)  // pure function, no negotiation branch
└── ExecuteRaid()                    // combat/loss event only — there is no dialogue tree here by design
```

`access_rule` is explicit that the absence of any offer *is* the design. This class exists purely to give the Iron Raiders a real presence in the danger-rating math that `HoldfastMapSeeder.cs` and the location `d[anger]` fields already use, rather than being flavor text with no game effect. Their `aggressionLevel` reads directly from the same wartime-tension value `FactionWarSystem.cs` already tracks for the four territorial Powers (Section II of `expansion_05`), so raising the siege stakes there also raises Iron Raiders danger — one shared input, no parallel tension system invented.

---

# VI. THE MUSTER — AWAKENING THE DESERTER COALITION

### VI.0 — What this is not

Before any new content: this codebase already has three separate systems that touch "a soldier who ran." They are not the Muster, and the Muster does not replace or duplicate them.

1. **`faction_deserter_asylum`** (`events.json`, Day 20+) — a single generic event where an unnamed Garrison-affiliated deserter offers intel in exchange for shelter. One decision, one survivor tag (`garrison_deserter`), done.
2. **`DeserterSystem.cs` / `DeserterHUD.cs`** — a standing hatch mechanic: any hostile-faction soldier can defect at the door, 30% chance they're a plant, `DeserterCombatBonus = 15f` if kept and legitimate. This runs constantly across the whole game, not tied to any faction narrative.
3. **`DesertersStandSystem.cs`** — a static, once-per-map environmental discovery describing a past civil-war massacre. Backstory, not an active faction.

The Muster is none of these. It is what happens when the sector's accumulated individual desertions — the Lasko vote, the asylum-seekers `DeserterSystem` has been quietly processing all game, and the specific crisis below — stop being isolated incidents and become a coalition with a location, leadership, and the ability to fight. Mechanically, it is the activation of `faction_deserter_coalition`, which has sat `is_active: false` in `currents.json` since it was written, home-region `the_verge` — precisely where Ivor Lasko hid in Day 40.

### VI.1 — The spark: Ola Vask

`quest_garrison_blood_debt` (`QuestlineSystem.cs`, Days 185–260) is already fully written: Colonel Harven (Section III) demands the player surrender survivor Ola Vask, a former Garrison conscript sheltered in the bunker, on pain of a fuel and medical embargo. This document does not alter that questline's text or branches. It gives its refusal branch a *consequence beyond the player's own bunker* for the first time.

**New hook, appended at the existing quest's refusal outcome:** if the player refuses to surrender Vask and survives the embargo Harven threatens, word reaches other Verge residents hiding the same kind of history — a shelter refused Harven and lived. This is the exact mechanism by which `faction_deserter_coalition` flips from `is_active: false` to `true`. No changes to `quest_garrison_blood_debt`'s existing stages; a single new trigger reads its completion flag.

### VI.2 — Formation

```
Assets/_Game/Factions/NPC_DeserterCoalition.cs
├── NPC_DeserterCoalitionState
│   ├── bool isActive                 // flips true on quest_garrison_blood_debt refusal-branch completion
│   ├── int membersRallied            // starts at 1 (Vask), grows via RallyMember()
│   ├── bool holdingGroundEstablished // see VI.3
│   ├── float garrisonLockoutRisk     // "sheltering them is the single fastest route to a Garrison lockout" —
│   │                                  //   this is the literal access_rule text made numeric
│   └── float trust
├── RallyMember(string survivorSourceId)   // called from DeserterSystem.cs's existing defection resolution
│                                            //   when a legitimate (non-plant) defector is kept post-activation —
│                                            //   this is the ONE integration point with the existing system,
│                                            //   not a duplicate of it
├── EstablishHoldingGround(string locationId)
├── OfferOr: patrol_schedules, weapon_maintenance, disciplined_fighters   // exactly the offers already
│                                                                          //   written in currents.json
└── ResolveMuster(bool sheltered, bool armed)  // Section VI.4
```

`RallyMember` is the single point of contact with `DeserterSystem.cs`: once the Coalition is active, a legitimate hatch defection can optionally be routed into `membersRallied` instead of (or in addition to) becoming an ordinary survivor. `DeserterSystem.cs` itself is not modified — this is an additive call the Coalition class makes into data `DeserterSystem` already produces.

### VI.3 — Holding ground: reusing `loc_denial_cut_substation`

The Coalition needs a defensible location. `year_of_ash_locations.json` already contains `loc_denial_cut_substation` — a reinforced railway culvert and transformer basement, marked with D/9 civil-defense denial notation. This document does not introduce a new location for the Coalition's camp; it reuses this one, which is both narratively apt (a hidden, hard-to-approach site) and mechanically apt (it is already flagged as dangerous to casually clear, which is exactly the deterrent a hideout needs). `EstablishHoldingGround("loc_denial_cut_substation")` is the intended call. If a design pass later wants a *second*, smaller overflow camp, `loc_muster_treeline_camp` (Section VIII) is provided — but the primary ground is the existing site.

### VI.4 — The Rising (Day 260–320) and resolution

Once the Coalition holds ground with `membersRallied >= 5`, Iron Garrison (under Harven, Section III) treats their existence as an act of Open Rebellion regardless of the player's own relationship with the Garrison — the Coalition's war is its own, not automatically the player's. Three branches:

1. **Sheltered & Armed** — the player supplies `offers: patrol_schedules, weapon_maintenance, disciplined_fighters` back into their own defense (a direct combat-strength contribution, mirroring how `DeserterCombatBonus` already works in `DeserterSystem.cs`) and the Coalition survives Harven's counter-raids through to Day 320, becoming a standing regional presence. Feeds the sixth epilogue (Section XII).
2. **Sheltered, Unarmed** — the player houses members but doesn't materially arm them; the Coalition is broken up by a Garrison raid on `loc_denial_cut_substation` by Day 300. `membersRallied` scatters back into the generic survivor pool `DeserterSystem.cs` already draws from — a clean, lossless fallback to existing systems rather than a dead end.
3. **Not Sheltered** — the Coalition never reaches `membersRallied >= 5`; Ola Vask's case remains an isolated one-bunker story exactly as `quest_garrison_blood_debt` already resolves it today. This is the "do nothing new" branch and it is fully supported: nothing breaks if a player never engages with this document's content at all.

---

# VII. NEW NAMED NPCS

Kept deliberately small — this document's weight is in systems, not headcount, and every name below was checked against `NPC_TamsinRook.cs`, `NPC_DessaVane.cs`, `docs/lore/06_REBUILDERS_AND_BLACK_OPS.md`'s Vane, and `year_of_ash_survivors.json`'s `survivor_corporal_vane` / `survivor_felix_vane` for collisions. None of the names below reuse Vane, Rook, Doyle, or Tamsin.

| NPC | Current / Role | Notes |
|---|---|---|
| **Ola Vask** | Deserter Coalition spark | Already named and voiced in `quest_garrison_blood_debt`; not renamed, only extended per Section VI. |
| **Halvard Ness** | Cold Count, senior of the four researchers | Delivers `TransmitFindings()`'s broadcast text; the one who says the line already written in `currents.json`: *"It's not a secret. It's a measurement."* |
| **Quenna Brix** | The Provisioned, contact point | Never asks the player for anything on first contact — per `NPC_Provisioned.RecordUnprompted`, she is evaluating, not negotiating. |
| **Osric Fane** | The Long Walk, route-keeper | Delivers `RequestSituationReport()`; explicitly tells the player the information is old, every time, per the Current's access rule. |
| **Brannick Sten** | Scavenger Guild, claims warden | The one who enters a shelter's id into `blacklistedShelterIds` — never apologizes for it, never explains twice. |
| **Meret Odalen** | Coastal Hydro-Barons, queue clerk | The public face of the rate card; not a villain, just extremely consistent about the price of moving up the list. |

---

# VIII. NEW & REUSED LOCATIONS

Every location this document's mechanics need was checked first against `docs/lore/03_LOCATIONS.md`'s 40 entries and `year_of_ash_locations.json`'s 30 entries. Five genuinely new locations remain; everything else reuses an existing id.

### Reused (no new location authored)
| Location id | Reused for |
|---|---|
| `loc_denial_cut_substation` | Deserter Coalition holding ground (Section VI.3) |
| `loc_low_background_lab` | Cold Count activation (Section V.1) — see `expansion_05` Addendum item 3 on this id's cross-catalog duplication risk |
| `loc_hydro_baron_aqueduct_manifold`, `loc_hydro_baron_desal_plant_4`, `loc_brine_pumping_sluice` | Coastal Hydro-Barons' three plants (Section II) |
| `loc_garrison_checkpoint_gamma`, `loc_garrison_motor_pool` | Harven-era martial law staging (Section III) |
| `loc_geothermal_well_alpha` | Grain Exchange irrigation tension (Section IV.6) |
| `loc_collapsed_valley_viaduct` | Long Walk's most treacherous regular crossing (Section V.4) |
| `loc_grange_hall` | Cited, not revisited — the historical site of the Lasko vote (Section VI.1) |

### New

```json
[
  {
    "id": "loc_muster_treeline_camp",
    "displayName": "The Treeline Camp",
    "d": 6,
    "travelHours": 5,
    "rads": 30,
    "description": "A scatter of lean-tos under dead pine, chosen because the canopy still holds enough ash-snow to break a thermal signature. No fire after dark.",
    "lore": "Overflow ground for the Deserter Coalition once the substation fills past what one exit can evacuate."
  },
  {
    "id": "loc_second_winter_homestead",
    "displayName": "The Second Winter Homestead",
    "d": 3,
    "travelHours": 4,
    "rads": 15,
    "description": "A private shelter built into a hillside a decade before the Exchange, its blast door hand-fitted by someone who clearly expected to use it. It has been resupplied every winter since — by nobody the sector can identify.",
    "lore": "The Provisioned's home ground. Nothing about the approach road suggests thirty years of quiet competence; the door does."
  },
  {
    "id": "loc_scavenger_guildhall",
    "displayName": "The Scavenger Guildhall",
    "d": 4,
    "travelHours": 3,
    "rads": 20,
    "description": "A repurposed freight depot, its walls papered floor to ceiling with hand-drawn claim maps. Every claimed site is inked in one color; every blacklisted shelter's name is inked in a second color and never crossed out.",
    "lore": "Grid territory, but the Guild answers to no Power. The ledger on the second color is the whole of their law."
  },
  {
    "id": "loc_iron_raiders_den",
    "displayName": "The Cut", 
    "d": 9,
    "travelHours": 6,
    "rads": 40,
    "description": "A collapsed rail cutting choked with burned-out freight cars, refitted as a den. There is no gate to knock on and no reason to try.",
    "lore": "The Toll's worst-kept and least visited secret. Nobody has ever come back with a description of the inside worth trusting."
  },
  {
    "id": "loc_the_tally_hall",
    "displayName": "The Tally Hall",
    "d": 2,
    "travelHours": 2,
    "rads": 12,
    "description": "A converted counting house, its walls lined with ledger boxes instead of shelving. Every contract The Tally has ever written is filed here, dated, and enforced on schedule.",
    "lore": "The Toll. Gives NPC_Tally's existing EnforceDueContracts() a physical home instead of an abstract state check."
  }
]
```

`d` (danger, 1–10) and `rads` (10–85) both stay inside the ranges established by `docs/lore/03_LOCATIONS.md`'s schema and this document's memory of prior review feedback on the same. `loc_the_tally_hall` is the only "new" location that exists purely to give an already-fully-coded Current (Tally, Section IV.7) a place, not new mechanics — deliberately minimal.

---

# IX. NEW ITEMS & STORY ITEMS

Naming follows the established `year_of_ash_items.json` convention: verbose, technical, no flourish.

| Item id | Ties to | Notes |
|---|---|---|
| `item_prewar_diagnostic_scanner` | The Provisioned | Available from no other source in the game; a genuine pre-Exchange artifact, not refurbished salvage. |
| `item_deserter_coalition_forged_papers` | The Muster | `wants: papers` from `faction_deserter_coalition`'s own data — this item is the literal fulfillment of that want, craftable via Quiet House favor-trading (Section IV.5) or Tally contract (Section IV.7). |
| `item_hydro_baron_queue_chit` | Coastal Hydro-Barons | A stamped brass token; physical proof of position in the water queue, tradeable, forgeable at a narrative-only risk (Harven's checkpoints, Section III, do check these). |
| `item_scavenger_guild_claim_marker` | Scavenger Guild | Placed at a site to formally claim it; the mechanical trigger for `ClaimSite()`. |
| `item_long_walk_route_ledger` | Long Walk | A physical copy of `RequestSituationReport()`'s output — lets the player re-read a stale report without waiting for the Long Walk's next pass. |
| `item_cold_count_provenance_seal` | Cold Count | Story item: the physical artifact that accompanies `TransmitFindings()`'s broadcast, referenced but not consumed by "The Measured Truth" ending in `expansion_05` Section V.5. |

No new item duplicates anything in the 36-entry `year_of_ash_items.json` roster; `item_calibrated_mass_spectrometer_tube`, `item_boron_shielding_tile`, and `item_lead_shielded_sample_cask` (already shipped) remain the Cold Count's core material inputs and are reused, not reinvented.

---

# X. SUSTAINABILITY ECONOMY — TIGHTER BUT ACHIEVABLE

The user ask behind this section is specific: tighter resource pressure, but a genuine, reachable path to self-sufficiency through trade, crops, and water — not a grind with no floor. This is built entirely on real, already-implemented systems; no parallel economy is introduced.

### Water
`WaterEconomySystem.cs`'s three-tier model (irradiated → dirty → clean, via catchment and purifier) is untouched. What changes at Day 180+: the Coastal Hydro-Barons (Section II) become a *second* clean-water source, priced in `item_hydro_baron_queue_chit` position rather than rations — meaning a shelter that has run out of ration surplus but has built trust or delivered `brass_fittings`/`corrosion_inhibitor` can still buy water. This is the "tighter but sustainable" lever: the ration economy alone gets harder in Phase IV–V (per `expansion_05`'s +40% caloric demand), but a second, non-ration currency path stays open.

### Crops
`CropSO`'s existing fields (growth hours, water/hr, calorie yield, contamination yield, toxic strain flag) govern hydroponics without change. The Deep Freeze's heat-loss equation (`expansion_05` Section II) already threatens grow-light power draw; this document's only addition is that Grain Exchange's seasonal decline tick (Section IV.6) reads the same `CropSO` water/hr field the player's own hydroponics use, so a shelter that has kept its own crops alive through the winter is, by the same math, a shelter the Grain Exchange has an easier time trading with — self-sufficiency and faction standing reinforce each other instead of competing for the same clock.

### Trade
`TravelingCaravanSystem`'s route/stay/inventory model is the direct template for `NPC_LongWalk.DailyTick()` (Section V.4) — deliberately, so the player's mental model of "a caravan arrives, trades, leaves" extends naturally to a faction that is not a caravan. No new trade UI is required; the existing caravan trade panel can drive both.

### Forecasting
`FalloutForecastSystem`'s sensor-array upgrade path (level 1–3, horizon 4–6 days) is the input both the Provisioned's `almost_nothing` want (Section V.3) and the Long Walk's situation reports (Section V.4) implicitly reference — a shelter with a level-3 sensor array has less need of either faction's forecast, which is an intentional soft-cap on how much this document's new content matters to a shelter that has already invested in its own infrastructure. That is the sustainability design in one sentence: every new faction offers a shortcut around a real cost, never a resource with no other path to it.

---

# XI. MASSIVE LORE EXPANSION — WORLD HISTORY PAST DAY 180

New `world_history.json`-style entries, following the established "located knowledge" pattern (`discovery_location_id` / `discovery_trigger` / `knowledge_key`) documented across `docs/lore/*.md`.

```json
[
  {
    "knowledge_key": "history_continuity_reclamation_decree",
    "discovery_location_id": "loc_garrison_checkpoint_gamma",
    "discovery_trigger": "day_240_reached",
    "text": "The notice board at Checkpoint Gamma is repapered overnight. The old bulletin, signed Voss, is gone. The new one is signed Harven and says nothing about why. Nobody at the checkpoint corrects you if you ask for 'the Colonel' by the wrong name. Nobody confirms it either."
  },
  {
    "knowledge_key": "history_hydro_baron_rate_card_origin",
    "discovery_location_id": "loc_hydro_baron_aqueduct_manifold",
    "discovery_trigger": "first_visit",
    "text": "The rate card predates the Exchange by six years. It was a utility contractor's pricing sheet for drought-season surcharges. Nobody has revised a single line of it. The apocalypse changed what the water was worth; it did not change how the company decided who paid more."
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
    "text": "Four names on the duty roster, none of them changed since the Exchange. They did not evacuate when the order came. They stayed because the equipment could not be moved without recalibration, and recalibration takes longer than anyone believed the war would."
  },
  {
    "knowledge_key": "history_the_provisioned_advance_knowledge",
    "discovery_location_id": "loc_second_winter_homestead",
    "discovery_trigger": "respectScore_threshold",
    "text": "The homestead's log predates the Allocation Schedule by three winters of stocked supply runs. Someone here believed this was coming with enough certainty to spend a decade preparing for it alone, and never once tried to sell that certainty to anyone who might have used it."
  }
]
```

---

# XII. THE SIXTH EPILOGUE — THE OPEN MUSTER

Additive to `expansion_05` Section V's five endgame epilogues; the Day 360 evaluation gains a sixth branch.

**Requirements**: The Muster resolved to "Sheltered & Armed" (Section VI.4, branch 1), `faction_deserter_coalition` standing at holding-ground-established through Day 320, no Garrison surrender of Ola Vask at any point.

**Ending Prose:** The substation held. Not because it was strong — because Harven's Garrison had a siege to run against three other fronts and could not spare the men to finish what a fuel embargo started. By Day 320, the tally scratched into the transformer housing runs past forty names. Nobody calls it a victory. Ottilie Frayne's ledger at the Allotments notes, without comment, that the Coalition's patrol schedules cut Rebuilder losses on the supply road by a third. Harven's notices stop mentioning them by Day 340, which is its own kind of admission. You did not win the war. You made one corner of it slightly less willing to keep going.

---

# XIII. GODOT-NATIVE IMPLEMENTATION BLUEPRINT

```
Godot Host (Presentation & UI)
├── src/Muster/
│   ├── MusterHostSession.cs          (Coordinator wiring the six new + eight extended Current classes)
│   ├── CurrentsRosterWidget.cs       (Fifteen-Current status panel — extends whatever renders currents.json today)
│   ├── DeserterCoalitionCampWidget.cs (Holding-ground status, membersRallied, garrisonLockoutRisk)
│   └── MusterSaveStore.cs            (JSON persistence, siblings YearOfAshSaveStore.cs)
└── Core Simulation (Ashfall.Core / Assets/_Game/Factions — plain C#, zero engine namespaces)
    ├── NPC_ColdCount.cs
    ├── NPC_Provisioned.cs
    ├── NPC_LongWalk.cs
    ├── NPC_ScavengerGuild.cs
    ├── NPC_IronRaiders.cs
    ├── NPC_DeserterCoalition.cs
    └── NPC_HydroBarons.cs             (mirrors NPC_Tally.cs's contract/ledger shape for the queue-chit economy)
```

### Catalog changes
- `currents.json`: append `faction_hydro_barons` (Section II); flip `is_active` to `true` for the six Section V Currents once their classes are wired into `GameBootstrap.Currents.cs`'s `BootCurrents()`, following the exact registration pattern already used for the existing eight (`RegisterPerSubstep`, `RegisterDaily`, `RegisterEventDriven`).
- `door_encounters.json` / `year_of_ash_radio.json`: one new entry for the Harven succession beat (Section III).
- `world_history.json`: five new entries (Section XI).
- New file `Assets/StreamingAssets/Data/faction_hydro_barons_locations.json` is **not** created — the three relevant locations already live in `year_of_ash_locations.json` and stay there.
- `year_of_ash_items.json` gains the six Section IX items appended to its existing 36.

### GameBootstrap wiring
`GameBootstrap.Currents.cs`'s `BootCurrents()` log line ("Currents booted: 8 state classes initialised") becomes 15 once this document's classes are wired — the fifteenth being `faction_hydro_barons`, which is new to the roster entirely rather than an activation of an existing dormant entry.

---

# XIV. VERIFICATION PROTOCOL

1. `dotnet test Ashfall.Core.Tests` — new tests for each of the seven new/reused `NPC_*.cs` classes (six activated Currents + Hydro-Barons), following the existing `KittiwakeChartEventTests.cs` shape: construct, mutate, `CaptureState()`/`RestoreState()` round-trip, assert.
2. `dotnet build Ashfall.csproj` — 0 errors, 0 warnings, no `UnityEngine` or `Godot` references inside any new `Assets/_Game/Factions/NPC_*.cs` file.
3. Regression check on `DeserterSystem.cs` and `DesertersStandSystem.cs` — confirm neither file requires modification; the Muster's only touch point is the additive `RallyMember()` call described in Section VI.2.
4. Cross-catalog id check — confirm no new id introduced by this document collides with an existing id in `locations.json`, `holdfast_locations.json`, or `year_of_ash_locations.json` (the `loc_low_background_lab` triple-definition flagged in `expansion_05`'s Addendum is a pre-existing condition this document does not worsen, since it reuses that id rather than redefining it).
5. Save/Load parity — Coalition and Cold Count state in particular (both gate an epilogue) must round-trip identically between Godot and Unity batch test harnesses.

---

# XV. NON-DUPLICATION LEDGER & SELF-REVIEW

- **Placeholder scan**: no TBD/TODO left in this document. Every new NPC, item, and location has a concrete id.
- **Internal consistency**: Section VI.4's three branches all terminate cleanly into either a new epilogue (Section XII) or an existing system (`DeserterSystem.cs`'s survivor pool) — no dead-end state.
- **Scope check**: this document adds one new Current (Hydro-Barons), activates six dormant ones, extends eight active ones with single-hook additions, and adds five locations, six items, six named NPCs, and one epilogue. It does not touch combat, save format versioning, or the four territorial Powers' core diplomacy model.
- **Ambiguity check**: Section III deliberately leaves Voss's fate unresolved ("the record is precise about" nothing) — this is an intentional, tone-locked ambiguity matching the house voice, not an unresolved design question.
- **Known open item carried forward**: the `loc_low_background_lab` cross-catalog id collision (three files, same id) remains unresolved at the data-integrity level; both this document and `expansion_05`'s Addendum flag it for a future code/data pass rather than silently working around it.
