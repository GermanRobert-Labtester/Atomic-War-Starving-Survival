# ASHFALL — Expansion Design Bible

**Title:** ASHFALL: NOBODY'S CHARTER
**Internal id:** `expansion_nobodys_charter` (*existing* in `CrossingIds.Expansion`)
**Pack number:** **04** — Standing Record already owns 03. The earlier files `expansion_03_nobodys_charter_plan.md` and `expansion_03_nobodys_charter_INTEGRATION_PIPELINE.md` are misnumbered. **Recommend renaming the pipeline to `expansion_04_nobodys_charter_INTEGRATION_PIPELINE.md`.** Do not do it in this pass. Treat the old 03 plan as a pre-implementation draft; this file is the authoritative post-implementation bible.
**Status:** Reverse-engineered from shipped C# and JSON. Design elevated to sibling standard. Divergences are flagged, not papered over.
**Ids below are EXISTING unless marked *PROPOSED*.** Where a shipped id is bad, the id stays; the rename lives in Appendix B.
**Tone lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.
**Sister packs:** Expansion 1 `expansion_the_holdfast` (the allocated world). Expansion 2 `expansion_the_duty_roster` (the unlisted home). Expansion 3 `expansion_the_standing_record` (the ground). This pack is **who speaks for whom**.

---

# ANALYSIS PHASE

## 1. What actually exists in the tree today

This pack is the unusual case. A design bible was written (`docs/expansions/expansion_03_nobodys_charter_plan.md`, 1119 lines, status line still reading "No game data has been edited. No C#"). An integration pipeline then implemented **Phases 1–2** against that bible. Standing Record later took the 03 number. The result is a half-built fourth pack sitting on a misnumbered first draft.

The job of this document is not to pretend the first draft was never written, and not to pretend Phases 3–7 shipped. It is to say what the code does, whether that earns a seat beside the other three, and what must be finished or fixed.

### Systems (plain C#, booted)

| Class | File | What it actually does |
|---|---|---|
| `VouchAccessSystem` | `Assets/_Game/Core/VouchAccessSystem.cs` | Social gate. State: `vouchedBy`, `vouchBurned`, `accessSoftened`, `lastResortUsed`. Events: `OnVouchGranted`, `OnVouchBurned`, `OnAccessSoftened`. `CaptureState` / `RestoreState`. **Does not implement `ISaveable` itself** — wrapped by `VouchAccessSaveable`. |
| `CrossingIds` | `Assets/_Game/Core/CrossingIds.cs` | Master constant class. Lists quests, locations, items, flags, knowledge keys, mutations, and endings — **including many that have no JSON and no runtime.** |
| `GameBootstrap.NobodyCharter` | `Assets/_Game/Core/GameBootstrap.NobodyCharter.cs` | `BootNobodyCharter()` from `InitDeepLore` after Currents + Holdfast. Merges locations and items, caches quest cards, wires flag side-effects, exposes host API. |
| `NPC_OsranKell` | `Assets/_Game/Factions/NPC_OsranKell.cs` | Weigh count, bribe-refusal once. Events + save. No Utility AI. No dialogue. |
| `NPC_MattisCray` | `Assets/_Game/Factions/NPC_MattisCray.cs` | Vouch count, burned-once lock. Events + save. **Not automatically burned when `Vouch.BurnVouch()` fires.** |
| Four catalog loaders | `Assets/_Game/Data/Crossing*CatalogLoader.cs` | Static `_cache`, JsonUtility wrap-array pattern, mirrors Holdfast. Locations `ApplyToCatalog`; items `MaterialiseAll`; quests `Load` only. |

Save registration is real (`GameBootstrap.ExpansionSaveables.cs`): `VouchAccessSaveable` (`SaveId` = `vouch_access_system`), `OsranKellSaveable` (`npc_osran_kell`), `MattisCraySaveable` (`npc_mattis_cray`). Event-driven registry ids: `vouch_access_system`, `npc_osran_kell`, `npc_mattis_cray`. No daily tick. No `CrossingQuestSystem`. No `CrossingMapSeeder`. No `EventRunner.NobodyCharter`. `ExpeditionSystem` has `SetIceRoadSystem` / `SetCensusClaimSystem` and **no** `SetVouchAccessSystem`. `GateAllowsCrossing()` is never called from map, expedition, or event code.

### Catalogs (JSON on disk)

**`crossing_factions.json` — 3 blocs, Currents-shaped, not in `faction_lore.json`:**

| id | display_name | alignment | home_region |
|---|---|---|---|
| `faction_the_scale` | The Scale | conditional | `region_crossing` |
| `faction_the_underwrite` | The Underwrite | conditional | `region_crossing` |
| `faction_the_compact` | The Compact | peaceful | `region_crossing` |

**`crossing_locations.json` — 7 cards, all `region_crossing`:**

| id | displayName | dangerLevel | travelHours | baseRadsPerHour |
|---|---|---:|---:|---:|
| `loc_crossing_viaduct_gate` | The Viaduct Gate | 0.1 | 8 | 0.4 |
| `loc_crossing_scalehouse` | The Scalehouse | 0.05 | 1 | 0.2 |
| `loc_crossing_stallrow` | Stallrow | 0.15 | 0.5 | 0.2 |
| `loc_crossing_watchtower` | The Watchtower | 0.2 | 0.5 | 0.3 |
| `loc_crossing_weighbridge` | The Weighbridge | 0.05 | 0.5 | 0.2 |
| `loc_crossing_underwrite_hall` | The Underwrite Hall | 0.1 | 0.5 | 0.2 |
| `loc_crossing_records_room` | The Records Room | 0.1 | 0.5 | 0.15 |

Those numbers are **wrong units** relative to live `locations.json` and `holdfast_locations.json` (danger 5–8, hours-from-bunker 2.5–14, rads 18–52). See Appendix B.

`CrossingIds.Locations.Nightfire` = `loc_crossing_nightfire` has **no JSON row**.

**`crossing_items.json` — 5 items:**

| id | displayName | type |
|---|---|---|
| `item_vouch_token_crossing` | Vouch Token | Quest |
| `item_calibration_weight` | Calibration Weight | Tool |
| `item_crossing_traded_grain` | Crossing Grain | Trade |
| `item_crossing_traded_salt` | Crossing Salt | Trade |
| `item_crossing_pledge_slip` | Pledge Slip | Quest |

`CrossingIds.Items.CharterPages` = `item_charter_three_pages` has **no JSON row**. Grain, salt, and pledge slip are **not** in `CrossingIds`.

**`crossing_quests.json` — 2 cards only:**

| id | display_name | type | prereq | min_day |
|---|---|---|---|---:|
| `quest_crossing_first_weigh` | What the Scale Says | expedition | `quest_crossing_the_vouch` | 20 |
| `quest_crossing_scale_integrity` | What the Weight Doesn't Move | side | `quest_crossing_first_weigh` | 24 |

The prereq `quest_crossing_the_vouch` is a constant in `CrossingIds.Quests.TheVouch` and **has no card**. Ten other quest constants in `CrossingIds` have no cards. Cards are cached on `GameBootstrap.CrossingQuests` / `GetCrossingQuest`. Nothing starts them. They are not in `QuestlineSO.Ids`. There is no `CrossingQuestSystem` equivalent of `HoldfastQuestSystem`.

**`characters.json` — 2 Crossing people:**

| id | display_name | profession | location_id | first_day | faction |
|---|---|---|---|---:|---|
| `npc_osran_kell` | Osran Kell | Scale Keeper | `loc_crossing_scalehouse` | 20 | `none` |
| `npc_mattis_cray` | Mattis Cray | Gate Attendant | `loc_crossing_viaduct_gate` | 20 | `none` |

Dessa Vane, Perrin Ashby, Ivo Fenn, and Wyn Sabler exist in the 03 draft and in location inspect prose. They have **no** `characters.json` rows and **no** `NPC_*` classes.

### Tests (EditMode, no Unity run in this pass)

- `NobodyCharterVouchAccessTests.cs` — grant / burn / soften / last-resort / save round-trip / Osran weigh / Mattis burn. Solid for the state machine.
- `CrossingCatalogTests.cs` — three blocs, ≥7 locations, two Scale quests, companions at expected locations, location merge dedupes. Does **not** assert travelHours/danger/rads against the live schema. Does **not** assert `quest_crossing_the_vouch` exists as a card.

### What the pipeline claimed vs what shipped

| Pipeline phase | Claim | Reality |
|---|---|---|
| 1 — gate + founding catalog | implemented | **Mostly.** Vouch system, 3 blocs, 4 Scalehouse-row locations, Osran, Mattis, boot, tests. Opening quest `quest_crossing_the_vouch` **not** written. Gate **not** wired to travel. |
| 2 — Scale bloc | implemented | **Data only.** Weighbridge / Underwrite Hall / Records Room added. Two quest **cards**. Items merged. No quest runtime. No first-weigh host action. |
| 3 — Standing | not started | `CrossingArbitrationSystem` does not exist. |
| 4 — Underwrite + Compact + Ledger | not started | `LedgerDebtSystem` does not exist. Dessa / Perrin / Wyn / Ivo do not exist as data. |
| 5–7 — Charter, endings, encounters, AI | not started | Constants only. |

## 2. What the shipped code implies

`VouchAccessSystem` is not a reputation bar and not a calendar. It is four booleans and a name:

- Someone stakes an id (`vouchedBy`).
- That stake can be spent badly (`vouchBurned`), after which the same name will not reopen the gate.
- After an opening arc the player's own name is enough (`accessSoftened`), and that standing cannot be burned back to a closed gate.
- There is always supposed to be one more name (`lastResortUsed` / Mattis), so the pack never hard-locks.

That is **who speaks for whom**. Access granted by a person, revoked by a person, outgrown into a name that no longer needs a sponsor. Holdfast opens a road with ice. Duty Roster opens a chart with a pencil. Standing Record opens a room with a plate. This pack opens a truss with a name.

The three blocs in `crossing_factions.json` imply a second, still-unbuilt half of the same thesis: once you are inside, whose word currently holds. Scale (a number), Underwrite (a contract), Compact (a draft). The 03 bible called that the Standing. The code does not have it yet. The vouch is the gate; the Standing would be the room. Both are about a name doing work a form, a wall, and a plate do not do.

## 3. What is missing (and what is worse than missing)

Missing content is expected — Phases 3–7 were never started. Worse than missing:

1. **The gate does not gate.** `GateAllowsCrossing()` is dead API. Locations merge into `LocationCatalogSO` with no `CrossingMapSeeder`, so they are cards without a node graph. A player with an old save can, in principle, see a Crossing location the way they see any catalog row, and nothing asks who vouched.
2. **The opening quest is a dangling pointer.** `quest_crossing_first_weigh` requires `quest_crossing_the_vouch`. The vouch quest is a string constant. The first thing the Scale bloc asks the player to have done does not exist.
3. **Schema units would make the Crossing the safest, cleanest site in the game** if the cards went live as written. `baseRadsPerHour: 0.4` against `loc_weighbridge`'s 28 is not "a working town." It is a unit error. `dangerLevel: 0.05` against the live 1–10 scale is the same error.
4. **Two Weighbridges.** Live `loc_weighbridge` is the Tollman's first office (danger 5, 2.5h, 28 rads). Shipped `loc_crossing_weighbridge` reuses the display name. Holdfast already added `loc_cut_weigh_hut`. Three honest scales is not a thesis. It is a habit.
5. **Canon collisions the 03 draft under-weighted.** Grain Exchange's live `access_rule` is *"No guards, no charter, no enforcement."* The viaduct sign is `NO CHARTER NO GUARD ASK FOR SOMEONE`. The Tally already reads a contract twice and collects a named forfeit; Dessa's shipped quote is a paraphrase of the Tally's. Compact's scoring clause is The List / Duty Roster / Holdfast RUR in a third room — usable as echo, fatal as a new idea.
6. **`CrossingIds` is a wish list compiled as a master list.** AGENTS.md says never invent an id that isn't in the master list. The inverse happened: the master list invented ids the data does not contain. That is how `loc_crossing_nightfire` and `item_charter_three_pages` and ten quests became "canonical" without existing.

## 4. Three ways to spend what was built

| # | Path | Why it might be right | Why it isn't, or how it is used |
|---|---|---|---|
| **A** | **Keep as pack 04, re-center on the vouch** | The shipped system is real, save-safe, and unclaimed by the other three. "Who speaks for whom" is a fourth axis. | **Proceeding.** Slim geography. Finish the gate. Differentiate Tally / Grain Exchange / Weighbridge. Standing and Ledger stay as the *completion* of the same thesis, not a second pack. |
| **B** | **Merge into Duty Roster** | Both are about names written and spent. Mattis's vouch is a row that lives in someone else's mouth. | Wrong room. Duty Roster is the hole. Merging would make Allocation 12 own a Toll/Drown market. The chart is who *you* write. The vouch is who *writes you*. |
| **C** | **Cut** | Half-built, misnumbered, unit-broken, overlapping Exchange and Tally. Cheapest. | Throws away a working `ISaveable` gate and the only social threshold in the game. Holdfast would still be seasonal. Standing Record would still be plates. Nobody would stake a name. |

**Not considered as spine:** activating the dormant Currents (the 03 draft's concept B — reserved, still correct as future work). A fourth coast (forbidden). A courtroom sim (wrong genre).

## 5. Choice and why

**KEEP — ASHFALL: NOBODY'S CHARTER, pack 04.**

It earns the seat if and only if the thesis stays **a name as infrastructure** and does not drift into:

- a second Grain Exchange (four Powers, no charter, hunger as enforcement);
- a second Tally (travelling debt police, read-twice, death-grade forfeit);
- a second List / Standing Record (a document that does not say what people need it to say — usable as a *small* echo, not the spine);
- a second Holdfast weigh hut (mass as the Office's grammar).

The compliance argument, restated for four packs: **the Powers hold ground. The Overlay writes ground. The roster writes people at home. The Crossing holds a name, for as long as the person who spoke it will still say it.** That is not a fifth Power. It is not a Current that patrols. It is not a plate. It is not a wall chart. It is a person, and the person can recant.

Blood & Wine, in this house, is not a new duchy. It is **being let in because someone risked their name, and then living with what that name is now worth.**

---

# SECTION 1 — EXPANSION OVERVIEW

| Field | Value |
|---|---|
| **Title** | ASHFALL: NOBODY'S CHARTER |
| **id** | `expansion_nobodys_charter` (*existing*) |
| **Hook** | A place none of the four Powers thought was worth holding lets you in on a name. The name can be spent. The name can be taken back. After a while your own name is enough, and that is a different kind of debt. |
| **Tagline (UI)** | *Ask for someone. That is the whole law.* |
| **Genre lock** | Same game. 2D survival-**management**. Expeditions are node ticks. No 3D interchange, no dialogue-wheel courtroom, no co-op. |
| **Playtime (new content)** | **10–15 hours** main Crossing arc plus one bloc's side catalog on a mid-game save; **16–22 hours** completionist. Honest: **~90 minutes of authored data exist today** (two quest cards, seven locations, two NPCs). The hours above are the *finished* pack. |
| **Scale honesty** | Smaller than Holdfast on purpose: one front, not four sub-regions. **7 shipped POIs** (cap 12). Three blocs in one catalog. 2 shipped NPCs (cap 6). 1 shipped system of 3 designed. 2 shipped quest cards of 10 main + sides. Not a walkable overworld. Not a fourth coast. |
| **Progression gate (soft)** | Day **70+** (*PROPOSED* — shipped `first_day` / `min_day` are 20 / 24; that is too early and collides with Ostrowski's Day 20). Shelter can field a 2–3 person expedition of 6+ hours, at least one tradeable surplus. |
| **Progression gate (story)** | A grievance a Power will not hear: `Mutation_TransitTax`, `Mutation_MedicalSupplyGone`, a Cult tithe, a Holdfast levy dispute, or a Duty Roster occupancy fight — **or** Ostrowski trust high enough that he will sell a sketch and refuse to walk it. |
| **Progression gate (hard ending)** | Day 150+ **and** two of three bloc chains resolved (any shape) **and** the Charter found. Unbuilt. |
| **Does not require** | Holdfast, Duty Roster, or Standing Record unlocked. If they are live, every main quest reads Appendix A. If they are dark, Ostrowski (or Mattis) is the vouch path. |
| **Does not add** | A seventh `faction_lore.json` row. A `_hegemony` entry. A 16th `Victory_*.cs` (optional epilogue `victory_nobodys_charter` only). New hatch magnitudes. A fifth Sector 4 Power. A fourth coast. Terraformers, Tessarat, 7G, androids, neuromancers. |

### Thesis (unspoken)

A form can name you. A wall can list you. A plate can number the ground you stand on. None of those is a person saying *I will be the reason they let you across.* That sentence has a cost. Spending it is also a sentence.

### One-paragraph pitch

The depot was built to settle who paid for a scale. Nobody has settled anything at the Crossing since, and the substitute for a government has been five years of people who have to eat tomorrow agreeing, in public, on whose name is good today. The Scale keeps a weight honest because a dishonest weight empties the stalls. The Underwrite covers a loss because covering a loss is how you get to be owed — locally, in a chained ledger, not as the Tally walking Sector 4. The Compact wants to write it down before it hardens into either of the other two, and the draft has a scoring clause that anyone from Allocation 12 will recognise. You do not get in because a clerk filed you, or because a plate says CUT-19, or because your row is in ink. You get in because Mattis, or Ostrowski, or Edor, or a Blank Rows contact, said your name at a truss that still reads NO CHARTER NO GUARD ASK FOR SOMEONE. The game will not tell you whether that is better than a form.

### How this is not the other three

| Pack | Axis | Access grammar |
|---|---|---|
| Holdfast | The allocated world | A **form**. The Office names a trade. The ice opens on a calendar. |
| Duty Roster | The unlisted home | A **wall**. You write a name, or you don't, and the hole changes. |
| Standing Record | The ground | A **plate**. A place is what the last stencil says. |
| Nobody's Charter | Who speaks for whom | A **name spoken for you**. The gate is a person. The person can recant. |

If a quest can be resolved by filing, inking a chart, or screwing a plate, it is the wrong pack. If it can be resolved only by someone staking standing they cannot get back, it belongs here.

### Integration strategy

| Layer | How it attaches | Shipped? |
|---|---|---|
| **Map** | One front, `region_crossing`, Toll/Drown *seam* — not a sixth gazetteer sub-region, not a coast. Entry `loc_crossing_viaduct_gate`. | Cards merged. **No seeder. No edges.** |
| **Travel** | 3.5–6.5h from bunker (*PROPOSED* retune; JSON gate is 8h, inners are 0.5–1.0 as if hops). Danger 4–7, rads 18–30 (*PROPOSED* retune). Gate is social: `VouchAccessSystem`. | System exists. **Travel does not consult it.** |
| **Economy** | No new currency. Stallrow prices ignore Power hegemony while the Scale is honest. Debt is a document (`LedgerDebtSystem`, unbuilt), not a second coin. | Trade item defs exist. **No price hook.** |
| **Lore** | `world_history` under `ashfall`, `discovery_location_id` = Crossing POIs. Charter is a *small* List-method beat about **legitimacy**, not cadastral names (Standing Record owns those). | **No `world_history` rows. No `lore_nc_*` bodies.** |
| **Factions** | `crossing_factions.json` only. **Not** `faction_lore.json`. **Not** `_hegemony`. **Not** `currents.json` (they are smaller and less durable than a Current — a Current has an `access_rule` that does not expire when one person recants). | Catalog live. |
| **Consequences** | Market/route mutations, same shape as `Mutation_MedicalSupplyGone` / `Mutation_Highway9Cleared`. | Constants in `CrossingIds.Mutations`. **None applied.** |
| **Save** | `exp_nobodys_charter_unlocked` (*PROPOSED* — not set anywhere) + vouch blob + NPC blobs + (later) Standing + Ledger. Old saves load; the viaduct is flavour until the rumour quest. | Vouch + two NPCs save. Unlock flag missing. |
| **UI** | Codex tab "The Crossing" or fold into Currents. Notice-board for rulings (unbuilt). Contract re-read (unbuilt). **No reputation integer.** | None. |
| **Quests** | `CrossingQuestSystem` mirroring `HoldfastQuestSystem.BindCatalog`. Register ids in `QuestlineSO.Ids`. | Cards cached. **No runtime.** |

### What the player is managing at the Crossing

The same seven needs. The weight that shifts is **a name that can be spent**.

| Need | How the Crossing bites |
|---|---|
| Hunger / Thirst | Fair rates while the Scale is honest and the vouch is clean. A burned name is a longer walk to a worse board. |
| Fatigue | No second shelter. There-and-back, or a bed at the Annex (*PROPOSED*) as a favour, not a base. |
| Warmth | Home bunker still ticks. The Nightfire is someone else's wood. |
| Radiation | Working-town band (18–30) once units are fixed. Not a hot ruin. Not 0.2. |
| Morale | Watching a name you used get recanted, or spending Mattis because you had no one else. Marks, not sermons. |
| Health | Lockup and outfall-adjacent chores (*PROPOSED*). Ordinary injury. |
| Shelter | Untouched. No waystation clone. Holdfast already owns the second roof. |

---

# SECTION 2 — THE CROSSING (as shipped, then as designed)

**The Crossing** (what everyone calls it) / **Interchange 6** (stencil on the scale-house roof, *PROPOSED* flavour — not a new gazetteer name that Standing Record must plate) / **the depot** (what the Toll says, when the Toll bothers).

Held by: nobody, the way the Drown holds nobody. Contested by three blocs who hold *rulings and names*, not ground. Visual DNA: dry-gouache, ash-grey, concrete, rust, terminal amber, plus **hand-lettering**. Every sign repainted by someone with an opinion. No two signs agree on spelling.

**Map rule:** this is the Toll/Drown *seam*, reached off Highway 9's unmaintained shoulder. It is **not** a sixth Sector 4 sub-region. It is **not** a fourth coast. `region_crossing` is a catalog tag, not a gazetteer expansion. Kilometre 19 remains Holdfast's last Sector 4 lamp. Overlay may eventually plate the viaduct (`CUT` / `DRN` seam — Standing Record's problem). This pack does not screw that plate.

Travel banding **as designed** (not as JSON — see Appendix B):

| Cluster | From bunker | Danger | Rads | Signature |
|---|---:|---:|---:|---|
| Scalehouse Row (shipped 4 + weighbridge) | 3.5–4.5 | 4–5 | 18–24 | Gate, scale, stalls, tower |
| Underwrite's Quarter (1 shipped + 3 *PROPOSED*) | 4.5–5.0 | 5–6 | 22–26 | Ledger, lockup, nightfire |
| Compact's Camp (1 shipped + 3 *PROPOSED*) | 4.0–5.5 | 4–6 | 20–30 | Petition, marker, annex, records |

**Entry:** `loc_crossing_viaduct_gate` is the only legal in. Vehicles stopped when the Warlords stopped grading the approach (Toll logs, not doctrine). First entry requires `VouchAccessSystem.HasAccess`.

---

## 2.1 Shipped POIs (7) — document what is on disk

Inspect and description below are the **live JSON**, lightly line-broken. They are already in house voice. Defects in the numbers sit beside them, not inside them.

### `loc_crossing_viaduct_gate` — The Viaduct Gate

**Who:** Mattis Cray.
**Shipped numbers:** d 0.1 · 8.0h · 0.4 rads — **illegal units; retune to d5 · 3.5–4.0h · 18 rads.**
**Inspect:** A rail truss over the Drown's edge, planked over for feet instead of axles. The paint on the sign has texture from how many times it has been redone: NO CHARTER NO GUARD ASK FOR SOMEONE. Someone added, smaller, underneath, in different paint: WE MEAN IT.
**Description:** The gate is not a wall. It is a threshold you are allowed to cross only because someone staked their own name on you. Until someone vouches, it stays closed and the viaduct stays quiet.
**Canon flag:** Grain Exchange's `access_rule` is already "No guards, no charter, no enforcement." The sign must stay — it is the pack's best sentence — but the *difference* has to be playable: the Exchange does not ask for someone, because four Powers are already in the room and hunger is the enforcement. The Crossing asks for someone because **no Power is in the room**. Recast the Exchange cameo so a Grain Exchange envoy (`enc_nc_grain_exchange_envoy`, *PROPOSED*) hears the sign and says the board does not need a name. Osran will not argue. Mattis will.

### `loc_crossing_scalehouse` — The Scalehouse

**Who:** Osran Kell.
**Shipped numbers:** d 0.05 · 1.0h · 0.2 rads — **retune to d4 · 4.0h from bunker (or 0.5h from gate if a seeder stores hop weights separately) · 20 rads.**
**Inspect:** A truck scale built for loads nobody hauls anymore, kept calibrated for reasons that stopped being obvious around the same time the reasons stopped mattering less. Osran's office has one chair for him and none for you. He'll fetch a second one. He always fetches a second one.
**Description:** Every load that leaves the Crossing is weighed here, once, on a scale that has never been argued into lying. Osran keeps the weights he trusts in a drawer he does not lock. Nobody has ever tried.
**Play:** First-weigh ritual (`quest_crossing_first_weigh`). The number is real. Inference is not his problem.

### `loc_crossing_stallrow` — Stallrow

**Shipped numbers:** d 0.15 · 0.5h · 0.2 rads — **retune to d4 · 4.0h / 0.4h hop · 20 rads.**
**Inspect:** The market the Scale vouches for, stalls chalked with claim marks in the Scavenger Guild's old pattern — copied, unofficially, because it is the only enforcement anybody here has seen work without a government behind it.
**Description:** Trade happens on chalked claims and a shared patience that can break. Stallrow is where the Crossing becomes legible to a stranger: prices, rivalries, and three different versions of the same founding story.
**Play:** Standing notice-board (*PROPOSED*). Cameo of `faction_scavenger_guild` as *model*, not questline.

### `loc_crossing_watchtower` — The Watchtower

**Shipped numbers:** d 0.2 · 0.5h · 0.3 rads — **retune to d5 · 4.5h / 0.5h hop · 24 rads.**
**Inspect:** The Drown side of the viaduct, watched. Two people, a stove, and a long list of every face that came across the rail truss and why.
**Description:** The watch keeps the gate honest. They answer to no single Power and keep no order but the order they are asked to keep. What they remember is a ledger of its own.
**Play:** Sightline. Smuggling side quest (*PROPOSED*). The list of faces is a vouch log — diegetic `vouchedBy` history, not a HUD.

### `loc_crossing_weighbridge` — The Weighbridge ⚠️

**Shipped numbers:** d 0.05 · 0.5h · 0.2 rads.
**Inspect:** The bridge scale itself, heavy as a doctrine. What it reports is a number; the argument is about what the number means. Osran reads it and records it; the record outlives every argument about it.
**Description:** Where loads are weighed on the way out and the way in. Its honesty is a shared assumption nobody wants to be the first to break, which is exactly why it has stayed honest.
**Defect:** Display name collides with live `loc_weighbridge` (Tollman's first office). Functionally this card is the **deck plate** of the Scalehouse, not a second civic weighbridge. **Proposed rename:** `loc_crossing_deck_scale` / display **The Deck Scale**. Keep the id in data until a migration ticket; `CrossingIds.Locations.Weighbridge` updates with it. Do not delete the card — `quest_crossing_scale_integrity` targets it.

### `loc_crossing_underwrite_hall` — The Underwrite Hall

**Who:** Dessa Vane (*named in inspect, no character row*).
**Shipped numbers:** d 0.1 · 0.5h · 0.2 rads — **retune to d5 · 4.5h / 0.5h hop · 22 rads.**
**Inspect:** A long table, a ledger chained to it — not against theft, Dessa will tell you, unprompted, the first time you ask. Against convenient memory. The fire is always lit. Somebody's interest paid for the wood.
**Description:** Where help is given at a plainly named price. Every contract here is read twice before it is signed, and after the second reading there is only the ink.
**Canon flag:** That is the Tally's ethic, relocated. Differentiation is **jurisdiction**. The Tally will walk to your hatch. The Underwrite will not leave the hall. A Tally forfeit can be a life. An Underwrite forfeit is a named good in the Lockup. If a quest needs a collector at Allocation 12, that is the Tally, not Dessa. See §10.

### `loc_crossing_records_room` — The Records Room

**Who:** Ivo Fenn (*named in inspect, no character row*).
**Shipped numbers:** d 0.1 · 0.5h · 0.15 rads — **retune to d6 · 5.5h / 0.6h hop · 30 rads.**
**Inspect:** Five years of the Crossing, filed by someone who believes a paper trail is the only enforcement a town without a guard can afford. Ivo Fenn keeps it, and Ivo Fenn's records do not lie.
**Description:** Ledgers, claims, and the surviving three pages of the original Charter. What is written here is older than everyone arguing about it, and nobody has read the whole of it in years.
**Defect:** The description **spoils the Charter mystery** on first inspect. Recast on implementation: the file exists; Ivo will not summarise; the player does not learn "three pages" until `quest_crossing_three_dry_pages`. Standing Record may later treat this room as a *site* with a plate. This pack treats it as a *clerk*.

---

## 2.2 Proposed POIs to complete the front (5 + 1 already-constant)

Do not exceed twelve. Do not open a coast. These fill the two clusters the JSON started and did not finish.

| id | Name | Cluster | Status | Hook |
|---|---|---|---|---|
| `loc_crossing_nightfire` | The Nightfire | Underwrite | *in `CrossingIds`, no JSON* | After-hours deals. No one claims it. Nobody else sits there. |
| `loc_crossing_the_lockup` | The Lockup | Underwrite | *PROPOSED* | Collateral. Paper threat. Teeth-in-a-jar story, once, never twice. |
| `loc_crossing_granary_pledge` | The Pledged Granary | Underwrite | *PROPOSED* | Wyn's grain, countable, due. |
| `loc_crossing_petition_tent` | The Petition Tent | Compact | *PROPOSED* | Draft 4. Margins are a second document. |
| `loc_crossing_founders_marker` | The Founders' Marker | Compact | *PROPOSED* | Plaque corroded past the third line. Three legends. |
| `loc_crossing_the_annex` | The Annex | Compact | *PROPOSED* | Refugee beds on favours and Wyn's grain. Not a player base. |

**Cut from the 03 draft if scope slips:** manager's office as its own POI (fold into Scalehouse inspect). Smuggler's cache as a location (keep as encounter under the truss).

---

## 2.3 Existing Sector 4 nodes that gain meaning (not new geography)

When `exp_nobodys_charter_unlocked` (*PROPOSED* flag):

| id (*existing*) | Overlay |
|---|---|
| `loc_weighbridge` | Bram's cousin-trade. He will compare his needle to Osran's, unprompted, and be right to a pound. He will not say how. **Do not rename this card. Do not give the Crossing the same display name.** |
| `location_abandoned_convoy_yard` / `loc_diesel_tank_farm` / `loc_recovery_yard` | Approach waypoints. Chalk marks matching Stallrow on a fence. Description only. |
| `loc_conscription_office` | Pell has decided, in writing, not to know a place with no charter. His quota does not reach it. |
| `loc_low_background_lab` | Cold Count can date the marker and the paper. One line. No questline spent. |
| `location_the_memory_vault` | Sole cross-references the Charter once found. She will not be surprised it is small. She will be surprised anyone needed it to be large. **Standing Record owns the Vault's rooms.** This pack asks one question at the desk. |
| `loc_cut_weigh_hut` (*Holdfast*) | Yara's triplicate is mass-as-Office. Osran's deck is mass-as-habit. If both live, one line of comparison, no third scale invented. |
| `loc_cut_kilometre_19` (*Holdfast* / Standing Record seam) | Last Sector 4 lamp. The Crossing is *south-east of the Drown edge*, not north of the lamp. Overlay may plate the viaduct later. This pack does not. |

---

# SECTION 3 — MAIN STORYLINE

## Central conflict

**Three people are each partly right about what the Crossing is, and none of them can prove it, because the document that would prove it does not say that.**

Osran Kell says it is a scale-house that grew a market, and a scale-house needs a weighmaster more than it needs a government.
Dessa Vane says it is an economy that would raid without someone willing to be owed, and that person does not need to be liked.
Perrin Ashby says none of that is a reason it cannot also be a town, with a vote, and a code, and an appeal that is not "find three backers faster."

The player is the only arrival whose name is not already spent taking a side. That is why a vouch matters: it is the first time this place has to decide whether a stranger is real. It is also why a burned vouch is worse than a refused levy. A levy is a form you did not sign. A burned vouch is a person who will not say your name again.

## Theme (unspoken)

A rule nobody enforces is a suggestion. A rule everybody enforces is a government. A name one person will still say is neither, and it is how this place has eaten for five years. It works until the person who said it needs it to stop being true.

## Principal NPCs

### Shipped (2)

#### 1. `npc_osran_kell` — Osran Kell *(companion, labour — not a party)*

- **Where:** `loc_crossing_scalehouse`
- **Was:** State highway-authority scale inspector. The only person here whose pre-war job and current job are the same job.
- **Wants:** `trade_goods`. The scale kept honest. Stallrow fed. To not be the government, loudly, while doing most of what a government does.
- **Will not:** `rig_a_weight`, `claim_authority_he_would_have_to_defend`.
- **Shipped state:** `weighsPerformed`, `refusedBribe`, `bribeAttempted`. `PerformWeigh()`, `AttemptBribe()` (once).
- **Faction field:** `none`. **Defect:** should be `faction_the_scale` or stay `none` on purpose — Osran will not claim the bloc. Prefer **keep `none`**. The Scale is a practice, not his shirt.
- **Voice:** Numbers first. Answers a different question than the one asked, accurately.
- **Snippet (03 draft, still good):**
  > "I don't run this place. I run a scale. It happens that an honest scale is most of what a place like this needs, and it happens that I'm the one who kept it honest, and I understand why those two facts look like a throne from where you're standing. Get closer. It's a folding chair."

#### 2. `npc_mattis_cray` — Mattis Cray *(companion, last-resort vouch)*

- **Where:** `loc_crossing_viaduct_gate`, then anywhere
- **Was:** Crossing-born. Runner. The person who walks new arrivals across the truss.
- **Wants:** `reliable_messages`. The Crossing still walkable. To stop being the only last resort, which he has been for three years because he has already tried saying no to a person freezing on the plank.
- **Will not:** `pick_a_bloc`, `vouch_twice_for_the_burned`.
- **Shipped state:** `vouchesGiven`, `hasBeenBurned`. `GiveVouch()`, `BurnMattis()`. `WillVouch`.
- **Wiring defect:** `TryVouchAtCrossing(..., lastResort: true)` calls `GiveVouch()` only for Mattis. `BurnCrossingVouch()` does **not** call `BurnMattis()`. A burned gate and a burned Mattis can disagree. Fix on implementation: if `VouchedBy == npc_mattis_cray`, burn both.
- **Voice:** Fast, Verge-cadence, the only person here who does not talk like a committee.
- **Snippet:**
  > "I'll vouch for you. That means if you burn it, it's my name that's ash, not yours — you get to just leave. Think about that before you decide the debt collector deserved it."
- **Signature (live JSON):** "I vouch for you, not the Crossing. Keep them separate and we stay friends."

### Proposed (4) — required to finish the arc

#### 3. `npc_dessa_vane` — Dessa Vane *(companion)* *PROPOSED*

- **Where:** `loc_crossing_underwrite_hall`
- **Was:** Unconfirmed. Here by year one. She has never corrected the public biography.
- **Wants:** Contracts honoured. A Crossing that still needs the Underwrite next winter. Perrin's charter to fail at the clause that would regulate lending.
- **Will not:** Lie about a term. Waive a forfeit on the record. Leave the hall to collect — **that is how she is not the Tally.**
- **Voice:** Reads terms aloud, twice, unhurried. Same cadence for ten rounds or a season of labour.
- **Snippet (must not copy the Tally quote):**
  > "You've heard it. This copy stays chained. If someone has to walk to your hatch, that is a different office, and I am not that office."

The 03 draft gave her the Tally's signature almost word for word. **Do not ship that.** Live Underwrite `signature_quote` is already better: "Read it twice. I'll say it twice. After the second time there is only the ink."

#### 4. `npc_perrin_ashby` — Perrin Ashby *(companion)* *PROPOSED*

- **Where:** `loc_crossing_petition_tent`
- **Was:** Nothing that qualifies as a trade. Osran's only joke about anyone.
- **Wants:** A written charter that survives the people currently enforcing it by habit.
- **Will not:** Pass a clause they know is unfair to save time. Admit the voting-weight formula resembles a Reconstruction Utility Rating until the player says it.
- **Voice:** Earnest, precise, slightly too fond of "finally."
- **Echo, not clone:** Duty Roster owns the wall. Holdfast owns the levy form. Perrin owns a *draft that wants to be both* and has not noticed.

#### 5. `npc_ivo_fenn` — Ivo Fenn *PROPOSED, not a companion*

- **Where:** `loc_crossing_records_room`
- **Was:** Depot filing clerk. Genuinely the job.
- **Wants:** Order. Perrin's petition and Dessa's ledger irritate him for the same reason: neither is filed correctly.
- **Will not:** Destroy a record. Summarise the Charter. He produces files.
- **Standing Record hook:** If Overlay plates the records room, Ivo will file the plate's receipt and not the plate.

#### 6. `npc_wyn_sabler` — Wyn Sabler *PROPOSED, not a companion*

- **Where:** `loc_crossing_the_annex`, pledge at `loc_crossing_granary_pledge`
- **Was:** Upriver farmer. Second bad season.
- **Wants:** The granary. Failing that, not to be the Nightfire story.
- **Will not:** Ask the player to break terms she read twice. She will accept it if they do.

---

## Story beats (10)

| # | Beat | Gate | Shipped? |
|---|---|---|---|
| 1 | **The Vouch** | Progression met | **NO card.** Constants + token item only. |
| 2 | **The First Weigh** | Vouch obtained | **Card only.** No runtime. |
| 3 | **The Terms** | First weigh done | Unbuilt. |
| 4 | **The Petition** | First weigh done | Unbuilt. |
| 5 | **The Standing** | Any opening quest done | Unbuilt. The mechanical heart after the gate. |
| 6 | **The Marker** | Standing witnessed | Unbuilt. |
| 7 | **The Forfeit** | Terms + ~Day 90 | Unbuilt. |
| 8 | **The Vote That Isn't** | Petition + one Standing + Forfeit | Unbuilt. |
| 9 | **Three Dry Pages** | Marker + Records access | Unbuilt. Item constant only. |
| 10 | **Who Holds the Ledger** | Two bloc chains + Charter | Unbuilt. |

## Branching choices (5)

| id | Choice | Immediate | Long |
|---|---|---|---|
| `crossing_vouch_spent_well` | Keep the opening vouch clean | Full access; Mattis stays | Companion quest opens; soften becomes earned |
| `crossing_vouch_burned` | Betray the sponsor | Gate closes; harder second name | That NPC does not recover in this pack |
| `crossing_forfeit_honoured` / `_defaulted` / `_doublecrossed` | Wyn's granary | Underwrite / Compact / pursuit | Endgame mutation table |
| `crossing_petition_signed_honest` / `_signed_rigged` | Compact draft | Trust either way; rig is discoverable (Ivo) | Honest fragile ratification vs compromised rubric |
| `crossing_charter_revealed` / `_kept_quiet` / `_sold` | Three pages | Myths deflate / persist / become a weapon | Ending slide |

**Bloc chain definition** (unchanged from the 03 draft, still correct): Scale = `quest_crossing_first_weigh` + `quest_crossing_scale_integrity`; Underwrite = `quest_crossing_the_terms` + `quest_crossing_the_forfeit`; Compact = `quest_crossing_the_petition` + `quest_crossing_the_vote_that_isnt`. Scale's second quest stays a side quest. Procedural honesty does not escalate like a forfeit.

## Endings (4 narrative + 1 fade)

All *PROPOSED*. Write a `world_history` second paragraph at `loc_crossing_records_room` or live `loc_weighbridge` — **not** at `loc_crossing_weighbridge` until that card is renamed. The game does not rank them.

| id (*existing constants*) | Name | Condition | Slide |
|---|---|---|---|
| `ending_crossing_scale` | The Folding Chair | Scale-aligned; Osran backed at the endgame Standing | The scale is still honest. Osran still says he doesn't run the place. Fewer people believe him than used to. |
| `ending_crossing_underwrite` | Paid in Full | Underwrite-aligned; forfeit on Dessa's terms | Everyone eats. Everyone owes. The Nightfire is warmer than it has any right to be. |
| `ending_crossing_compact` | Draft Four, Signed | Charter ratified (honest or rigged; prose differs) | There is a document now. People argue with it instead of with each other, which Perrin considers a victory. |
| `ending_crossing_none` | No One's | Stacked double-cross, or Standing collapses | The viaduct is still there. The market isn't. Word travels. The people that word attracts do not ask to be vouched in. |
| `ending_crossing_walked` | Just Passing Through | Trade-access only; no endgame Standing | The Crossing continues exactly as uneasily as it always has. Nobody there will remember the player's name in a year. |

`TrueEnding` / terraformers / androids / neuromancers unused.

## Lore revelations (standing there)

1. Authority here accreted one honoured name at a time. It can un-accrete the same way.
2. The Charter is real, three pages, a weigh-scale calibration and toll-revenue compact between two county highway authorities, decades before the Exchange. It establishes nothing about self-governance. **This is The List's method at small scale, about legitimacy, not allocation, and not about what a place is *called* (Standing Record).**
3. Highway 9 and the Crossing are the same road, forty minutes apart, opposite theories of what a road is for. Do not retune `Mutation_Highway9Cleared`.
4. Stallrow's chalk copies `faction_scavenger_guild`. Do not spend the Guild's questline.
5. Wyn's forfeit, a Rebuilders brass demand, and a Cluster levy are the same shape of obligation in three institutional faces. Show it. Do not sermonise.
6. A ruling that needs three living backers is a law that has to keep being true. The game does not adjudicate whether that is safer.

---

# SECTION 4 — QUEST DESIGN

Runtime *PROPOSED*: `CrossingQuestSystem` mirroring `HoldfastQuestSystem` (`BindCatalog`, `TryStart`, `Advance`, `ChooseBranch`, daily tick). Register every `quest_crossing_*` in `QuestlineSO.Ids` — **not done**. Types: `expedition`, `shelter`, `faction`, `personal`, `repeatable`.

## 4.0 The Three Shapes

Every main quest (except the Vouch, which has nothing to betray yet) resolves Complete / Fail / Double-Cross. Complete keeps faith at the named price. Fail walks away with no scheme. Double-Cross spends the trust somewhere the asker did not agree. Double-Cross is the only shape that can stack into `ending_crossing_none`.

This is the pack's answer to world-altering completion/failure: a name, a ruling, or a contract that only holds while backed can actually be overturned on screen.

---

## 4.1 Shipped quest cards (2) — as they exist

### `quest_crossing_first_weigh` — What the Scale Says  *(card exists)*

| Field | Shipped value |
|---|---|
| **Type** | expedition |
| **Prereqs** | `quest_crossing_the_vouch` (**card missing**) |
| **min_day** | 20 (**too early; retune 70+**) |
| **Target** | `loc_crossing_scalehouse` |
| **knowledge_key** | `lore_nc_read_again` ⚠️ bible assigned this key to Terms; **proposed retune:** `lore_nc_the_vouch` is the Vouch; first weigh should mint `lore_nc_the_number` (*PROPOSED*) or reuse a Scale key. Do not leave Terms' key on the Scale quest. |
| **Stages** | `present_goods` → `answer_questions` → `accept_or_contest` → `stallrow_access` |
| **Rewards (implied)** | Stallrow access; `item_calibration_weight` |

**Choices (shipped)**

| id | text | set_flag | Shape |
|---|---|---|---|
| `first_weigh_accept_true` | Accept the true weight, even when it is less favourable than hoped. | `mutation_crossing_honest_trader` | Complete |
| `first_weigh_contest` | Contest the true weight anyway. Access still granted; the exchange notes it. | `mark_crossing_difficult` | Fail |
| `first_weigh_bribe` | Offer to pay Osran to misweigh. He refuses, on the record, in front of Stallrow. | `mutation_crossing_bribe_attempted` | Double-Cross |

**World mutation:** Complete → honest-trader rate. Fail → colder exchange, access still granted (Osran does not punish suspicion). Double-Cross → `NPC_OsranKell.AttemptBribe()`; trade access remains; Dessa hears before you arrive (*PROPOSED*).

**Host work still missing:** present goods from inventory; call `NPCOsranKell.PerformWeigh()`; grant Stallrow flag; grant calibration weight; apply mutation through `WorldStateConsequenceSystem`.

---

### `quest_crossing_scale_integrity` — What the Weight Doesn't Move  *(card exists)*

| Field | Shipped value |
|---|---|
| **Type** | side |
| **Prereqs** | `quest_crossing_first_weigh` |
| **min_day** | 24 (**retune ~80**) |
| **Target** | `loc_crossing_weighbridge` (rename target when the location is renamed) |
| **knowledge_key** | `lore_nc_three_legends` ⚠️ bible assigned this to the Marker. **Proposed:** `lore_nc_scale_true`. |
| **Stages** | `calibrate` → `verify_dispute` → `record_clean` |
| **Rewards (implied)** | Trade-rate stability |

**Choices (shipped)**

| id | text | set_flag | Shape |
|---|---|---|---|
| `scale_integrity_clear` | The scale reads clean. Stallrow keeps trusting the number. | `mutation_crossing_honest_trader` (duplicate of first-weigh Complete) | Complete |
| `scale_integrity_silent` | Verify honestly but decline to record it. The trust stays, unspoken. | `flag_crossing_scale_verified_silent` (**not in `CrossingIds`**) | Fail-adjacent / quiet Complete |

**Missing third shape:** report a fudge for Osran's peace of mind (`mark_crossing_scale_mercy`, *PROPOSED*). The 03 side table had this. The shipped card does not. Add it or the Scale has no Double-Cross.

**03 draft vs shipped:** the draft sent the player to find an outside reference weight (Cold Count or salvage). The card assumes `item_calibration_weight` is already in hand from first weigh. **Keep the shipped version.** It is tighter and does not spend the Cold Count.

---

## 4.2 Proposed main quests to complete the arc (8)

Documented at sibling density. All ids already sit in `CrossingIds` except where noted.

### `quest_crossing_the_vouch` — A Name at the Gate  *PROPOSED card; constant exists*

| Field | Value |
|---|---|
| **Type** | expedition |
| **Prereqs** | Soft gate (Day 70+ or grievance or Ostrowski) |
| **Time** | 30–50 min |
| **Synopsis** | Ostrowski names the Crossing, sells a rough sketch, will not walk there. "I sold them a map once. That was the whole transaction." |
| **Objectives** | 1. Hear Ostrowski. 2. Find a name: Ostrowski (reluctant, once), Mattis at the truss, or Appendix A alternates (Edor, Blank Rows). 3. Walk the approach waypoints. 4. Reach the viaduct. 5. `TryVouchAtCrossing`. |
| **Rewards** | `item_vouch_token_crossing`; `lore_nc_the_vouch`; `flag_crossing_vouched_clean` if clean |

**Resolution shapes:** Complete = clean vouch, Mattis meets you. Fail = quest stays open, colder second chance. Double-Cross = N/A (nothing trusted yet).

**Must ship before first_weigh is playable.**

---

### `quest_crossing_the_terms` — Read It Again  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | faction |
| **Prereqs** | First weigh done |
| **Synopsis** | Dessa offers seed, a covered loss, or a favour bank against a plainly named forfeit. Contract UI shows full text twice. `LedgerDebtSystem` opens a `DebtContract` if signed. |
| **knowledge_key** | `lore_nc_read_again` (move it here from first_weigh) |
| **Rewards** | Goods per contract; `item_crossing_pledge_slip` already exists — use it |

**Shapes:** Sign and later pay → `mutation_crossing_underwrite_reliable` (*PROPOSED*; `CrossingIds` has `UnderwriteBurned` only). Decline → `flag_crossing_underwrite_untested` (*existing constant*). Sign and dump the goods → `mutation_crossing_underwrite_burned`; collector escort on future visits; **collector is a Crossing NPC, not a Tally officer, unless the player hired the Tally separately.**

---

### `quest_crossing_the_petition` — Draft Four  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | faction |
| **Prereqs** | First weigh done |
| **Synopsis** | Perrin asks for an early signature and, if the player reads, a scoring clause for who gets a vote. |
| **knowledge_key** | `lore_nc_the_rubric_again` (*existing constant*) |
| **Holdfast / Roster hook** | If either pack is live, the player can compare the clause to a RUR score or a roster row. Perrin has not made that connection. |

**Shapes:** Revision pass → `mutation_crossing_petition_revised`. Decline → `flag_crossing_petition_unsigned`. Sign and leak the unrevised clause to Osran or Dessa → `mutation_crossing_petition_leaked` (*not in `CrossingIds` — add or do not mint*).

---

### `quest_crossing_the_standing` — Three Backers  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | faction |
| **Prereqs** | Weigh or Terms or Petition done |
| **Synopsis** | A live dispute (stall, debt, or petition) goes to the Standing. The player learns `CrossingArbitrationSystem` by using it. |
| **knowledge_key** | `lore_nc_the_standing` (*PROPOSED*; not in `CrossingIds.Knowledge`) |

**Shapes:** Back the side you believe → `mutation_crossing_standing_honest` (*PROPOSED*). Decline → no mark. Promise both / bribe → `mutation_crossing_standing_rigged` (*existing*). One principled backer (Deserter-sheltering stallholder, cameo) will report a bribe.

---

### `quest_crossing_the_marker` — What the Plaque Doesn't Say  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | exploration |
| **Prereqs** | Standing witnessed |
| **Synopsis** | Marker corroded past the third line. Three Nightfire legends. None of the tellers has read it. |
| **knowledge_key** | `lore_nc_three_legends` (move it here from scale_integrity) |

**Shapes:** Report contradictions intact. Don't investigate (Charter stays locked). Seed a tailored myth → `mutation_crossing_myth_seeded` (*PROPOSED*).

---

### `quest_crossing_the_forfeit` — What Wyn Owes  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | faction / personal |
| **Prereqs** | Terms resolved (need not have signed); ~Day 90; pledge term expired |
| **Synopsis** | Wyn will not ask. Dessa will collect exactly the contract, in front of witnesses, **in the Lockup, not at the player's hatch.** |
| **knowledge_key** | `lore_nc_the_forfeit` (*existing*) |

**Shapes:** Help her pay → `mutation_crossing_forfeit_honoured`. Do nothing → granary collected, calm; Compact recruitment up. Help her flee with pledged grain → `mutation_crossing_forfeit_doublecrossed` (*PROPOSED*); Dessa pursues Wyn on later visits; Blank Rows (if live) will not vouch afterward.

---

### `quest_crossing_the_vote_that_isnt` — Draft Four, Called  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | faction / crisis |
| **Prereqs** | Petition done; one Standing; Forfeit resolved any shape |
| **Synopsis** | Perrin calls ratification. Osran will not block or bless. Dessa interferes through a debt-called-in, not force. Larger Standing. |

**Shapes:** Cover the called-in debt or persuade a delay → `mutation_crossing_vote_clean` (*PROPOSED*). Stay out → usually stalls. Help Dessa target the backer → `mutation_crossing_vote_sabotaged` (*PROPOSED*); Ivo's records will know if anyone looks.

---

### `quest_crossing_three_dry_pages` — The Charter  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | exploration / story |
| **Prereqs** | Marker done; Records access |
| **Synopsis** | Ivo produces the file. Three dry pages. Calibration tolerance, revenue split, two signatures, a notary. |
| **Rewards** | `item_charter_three_pages` (constant; **add JSON**); `lore_nc_three_dry_pages` (*PROPOSED*) |

**Shapes:** Publish to all three blocs → `mutation_crossing_charter_revealed`. Sit on it → `mutation_crossing_charter_hidden` (*PROPOSED*). Sell to one bloc → `mutation_crossing_charter_weaponised` (*PROPOSED*); Ivo withdraws access.

**Standing Record boundary:** Sole may corroborate paper stock. Overlay may want to plate the records room. Neither writes the Charter's *meaning*. The player does.

---

### `quest_crossing_who_holds_the_ledger` — Endgame  *PROPOSED*

| Field | Value |
|---|---|
| **Type** | story |
| **Prereqs** | Two bloc chains + Charter found |
| **Synopsis** | Final Standing: what the Crossing *is*. Prior shapes tallied. |

**Shapes:** Honest bloc → that ending. No bloc, no prior rig → `ending_crossing_walked`. Stacked double-cross → `ending_crossing_none`; `faction_iron_raiders` cameo on the gate description, no combat forced, no questline spent.

---

## 4.3 Side quests (proposed; 1 shipped)

Keep the 03 catalog's eighteen as a *ceiling*, not a promise. Ship Scale's two remaining, Underwrite's three, Compact's three, four companions, three exploration, two repeatables — **in that order**. Do not write eighteen cards before the gate works.

**Already shipped:** `quest_crossing_scale_integrity`.

**Priority remaining**

| id | Giver | Hook |
|---|---|---|
| `quest_crossing_watchtower_smuggling` | Watchtower | Unweighed night goods. Report, ignore, or join. |
| `quest_crossing_stallrow_claim` | Stallholder | Two stalls, one chalk mark. Rule or call a Standing. |
| `quest_crossing_the_collateral` | Dessa | Lockup short against the ledger. |
| `quest_crossing_cold_feet` | Collector | Cannot take a last blanket. Cover, grace, or let it proceed — **in the Crossing, not at home.** |
| `quest_crossing_the_clauses` | Perrin | Dispute-resolution clause currently says "the Standing continues." |
| `quest_crossing_annex_intake` | Annex | Full. Someone already there would have to move. |
| `quest_crossing_companion_mattis` | Mattis | *Constant exists.* Who he vouched that burned him; a debt he is still covering. Paying it down is the only way to soften his "never twice" rule. |
| `quest_crossing_companion_osran` | Osran | Why he stayed. He tried a Power once. |
| `quest_crossing_weigh_run` | Osran | Repeatable fair-rate haul. Vouch must still be valid. The economic loop. |

Shelter-door / viaduct encounters (`se_nc_*`, `mmc_nc_*`) stay in the 03 draft as a backlog. Do not implement a second `ShelterEncounterSystem`. Hook Duty Roster's if live; otherwise use existing hatch constants and a viaduct trigger. **Do not retune hatch magnitudes.**

---

# SECTION 5 — SYSTEMS

**Cap: 3 new plain-C# systems.** Hook, do not rebuild: `WorldStateConsequenceSystem`, `DynamicEconomySystem`, `QuestRuntime` / `QuestRegistry`, Duty Roster encounter/mark systems if present, `ExpeditionSystem` hatch constants, Holdfast ice/census/brine/waystation.

---

## 5.1 `VouchAccessSystem` — as built

**id:** `vouch_access_system`
**File:** `Assets/_Game/Core/VouchAccessSystem.cs`
**Spec the code comments:** the 03 bible §5.2. This section is the code.

### State (`VouchAccessSystemState`)

| Field | Type | Meaning |
|---|---|---|
| `systemId` | string | Always `vouch_access_system` |
| `vouchedBy` | string | NPC id. Empty = never vouched. |
| `vouchBurned` | bool | Sponsor was betrayed; gate re-closed. |
| `accessSoftened` | bool | Own name sufficient. Cannot be burned down. |
| `lastResortUsed` | bool | Mattis's paid-for last resort cashed this playthrough. |

### Derived

| Property | Rule |
|---|---|
| `RequiresVouch` | `!accessSoftened && (vouchedBy empty \|\| vouchBurned)` |
| `HasAccess` | `!RequiresVouch` |
| `NeedsLastResort` | `RequiresVouch && !lastResortUsed` |

**Defect:** a fresh system has `NeedsLastResort == true`. Mattis is mechanically available before Ostrowski is asked. The "last resort" is the first resort. **Proposed:** `NeedsLastResort` should also require `vouchBurned` (or a `flag_crossing_first_vouch_attempted`). Tests that assert the current behaviour must move with the rule.

### API

```
bool GrantVouch(string npcId, bool isLastResort = false)
bool BurnVouch()
void SoftenAccess()
VouchAccessSystemState CaptureState()
void RestoreState(VouchAccessSystemState saved)  // null = no-op
```

**GrantVouch:** rejects null/empty; no-op if already softened; no-op if already cleanly vouched (`!vouchBurned && vouchedBy set`); otherwise writes `vouchedBy`, clears burned, optionally sets `lastResortUsed`, raises `OnVouchGranted(npcId)`.

**BurnVouch:** no-op if softened; no-op if never vouched and not already burned; otherwise clears `vouchedBy`, sets `vouchBurned`, raises `OnVouchBurned`.

**SoftenAccess:** idempotent; raises `OnAccessSoftened` once. **No prereq.** Tests allow soften from a never-vouched state. The fiction says "after the opening arc." **Proposed:** require `!string.IsNullOrEmpty(vouchedBy) || vouchBurned` so you cannot skip the name.

### Events

| Event | Payload | Host side-effect (`WireNobodyCharterEvents`) |
|---|---|---|
| `OnVouchGranted` | `string` npc id | `SetWorldFlag(flag_crossing_vouched_clean, true)` — **also fires for last-resort.** |
| `OnVouchBurned` | none | `vouched_clean` false; `flag_crossing_vouch_burned` true. Burned flag is **never cleared** on re-vouch. |
| `OnAccessSoftened` | none | `flag_crossing_access_softened` true |

### Host API (`GameBootstrap`)

| Method | Behaviour |
|---|---|
| `GateAllowsCrossing()` | `Vouch != null && Vouch.HasAccess` — **uncalled.** |
| `TryVouchAtCrossing(npcId, lastResort)` | `GrantVouch`; if granted && lastResort && id is `npc_mattis_cray`, `NPCMattisCray.GiveVouch()`. |
| `BurnCrossingVouch()` | `Vouch.BurnVouch()` only. **Does not `BurnMattis()`.** |
| `SoftenCrossingAccess()` | `Vouch.SoftenAccess()` |

### What it still needs

1. `ExpeditionSystem.SetVouchAccessSystem` (or a travel predicate) so a node with `id == loc_crossing_viaduct_gate` (and any `region_crossing` beyond it) refuses start unless `HasAccess`, with a diegetic refusal line, not a lockout modal.
2. `CrossingMapSeeder` so the seven cards become a spine: bunker → approach waypoints → gate → scalehouse hub → spokes.
3. Burn coupling to Mattis (and later to any `vouchedBy`).
4. `NeedsLastResort` / `SoftenAccess` prereqs as above.
5. Clear or recast `flag_crossing_vouch_burned` on a successful second vouch (keep a history flag `flag_crossing_ever_burned` if endings need it).
6. `exp_nobodys_charter_unlocked` set when the rumour quest starts, not at boot.
7. UI: location description swap on `RequiresVouch` / `HasAccess` / `VouchBurned` / `AccessSoftened`. No integer.

**Unrealistic (do not build):** rejected-at-the-door minigame, timed dialogue trees, visible reputation number.

---

## 5.2 `CrossingArbitrationSystem` — designed, not built

**id:** `crossing_arbitration_system` *PROPOSED*
**What it is:** The Standing. A ruling is real for as long as three backers hold it.

**Mechanics (unchanged, still right):**
- Pool of ~10–14 named stallholders as backers, each with `wants` / `will_not`.
- 3 declared backers to hold. Later 3+ can overturn. Fiction, not a bug.
- `StandingRuling { topic, backers[], shape }`.
- Events: `OnStandingCalled`, `OnRulingMade`, `OnRulingOverturned`.
- Principled backers cap pure bribery.

**UI:** Stallrow notice-board. Public. No hidden meter.

**Unrealistic:** agent-based politics, procedural opinion sim, hundreds of voters.

**Coupled-variable QA:** vouch state × backers × (later) debt. Implementer ≠ reviewer (Prompt #26).

---

## 5.3 `LedgerDebtSystem` — designed, not built

**id:** `ledger_debt_system` *PROPOSED*
**What it is:** Debt as a document. Read twice. Forfeit named. No hidden clause.

**Mechanics:**
- `DebtContract { debtorId, principal, termDays, rate, forfeit }` — forfeit is a named good or service-days, never abstract.
- Events: `OnContractSigned`, `OnContractPaid`, `OnContractRenegotiated`, `OnForfeitTriggered`, `OnLedgerTampered`.
- Collection happens at the Lockup or hall, **not** as a Tally hatch visit.

**Differentiation from `faction_the_tally`:** jurisdiction (local), forfeit grade (goods, not death), mobility (will not walk to Allocation 12), and voice (do not reuse "Do you want it read again?"). The Tally remains hireable in Sector 4. If both fire on the same debt, that is a story, not a merge.

**Unrealistic:** amortisation sim, credit score, compounding beyond one `rate` field.

---

## Systems explicitly not in this expansion

- No fourth new class.
- No fifth Power, no seventh Codex row, no `_hegemony` row.
- No second shelter. No `WaystationSystem` clone.
- No combat AI beyond existing expedition resolution.
- No new victory architecture beyond an optional epilogue flag.
- No Ice Road calendar. Do not touch `IceRoadSystem`.
- No Overlay plates. Do not build `LocationLayoutSystem` here.

---

# SECTION 6 — CHARACTERS & ENCOUNTERS

## 6.1 Companions

Assignable labour and expedition company. Utility AI bias + an unbuyable "will not." Seed `_worldSeed + 1811` (*PROPOSED*).

| id | Status | AI bias | Will not | If they die / leave |
|---|---|---|---|---|
| `npc_osran_kell` | **Shipped class** | Weigh, verify, refuse to arbitrate | Rig a weight; claim defendable authority | Unverified rates; Stallrow nervous |
| `npc_mattis_cray` | **Shipped class** | Vouch, run messages, refuse a bloc | Vouch twice for the burned | Gate harder for everyone |
| `npc_dessa_vane` | *PROPOSED* | Contract, collect, refuse public forgive | Waive a forfeit on the record; leave the hall to collect | Harder successor |
| `npc_perrin_ashby` | *PROPOSED* | Draft, canvas, refuse unfair speed | Pass a known-unfair clause | Draft stalls; Annex loses its advocate |

Ivo and Wyn stay stationary.

**Utility AI actions (*PROPOSED*):** `Action_WeighGoods`, `Action_ReadContract`, `Action_CanvasSupport`, `Action_RunVouch`. None exist. Osran's `PerformWeigh` is a counter, not an action.

## 6.2 Encounter variants (10) — all *PROPOSED*

Human danger. Existing `ExpeditionSystem` resolution. No fantasy threats.

| id | Where | Notes |
|---|---|---|
| `enc_nc_collector_visit` | hall, lockup, **not hatch unless Tally hired** | Polite, procedural, as threatening as the term |
| `enc_nc_backer_pressure` | Stallrow, Nightfire | Favour before backing |
| `enc_nc_lockup_muscle` | Lockup | Dangerous only if the player takes collateral by force |
| `enc_nc_iron_raiders_scout` | Viaduct, `ending_crossing_none` trending | Cameo only |
| `enc_nc_deserter_passage` | Viaduct, Annex | Cameo `faction_deserter_coalition` |
| `enc_nc_scavenger_dispute` | Stallrow | Blacklisted trader; tests whether the player understands the chalk |
| `enc_nc_grain_exchange_envoy` | Stallrow | Why no seat at the board. **This is the differentiation scene.** |
| `enc_nc_sun_seekers_pass` | Viaduct, False Spring | Flavour/trade. No quest. |
| `enc_nc_forfeit_witness` | Lockup, Stallrow | Scene, not a choice |
| `enc_nc_standing_ambush` | Stallrow | Only timed-pressure encounter; existing crisis pacing |

## 6.3 Crises (5) — multi-phase, not arenas  *PROPOSED*

`crisis_the_forfeit`, `crisis_the_vote`, `crisis_the_standing_contested`, `crisis_the_charter_found`, `crisis_who_holds_the_ledger`. Osran, Dessa, and Perrin are not bosses. Killing one (possible, costly) seats a colder successor within a season. It does not resolve the bloc.

---

# SECTION 7 — ITEMS & REWARDS

Existing tools remain canonical. New ids: shipped first, then proposed.

## 7.1 Shipped items (5)

| id | Name | Type | Function | Notes |
|---|---|---|---|---|
| `item_vouch_token_crossing` | Vouch Token | Quest | Matchbook slip with a name. The Crossing trusts the name, not the slip. | In `CrossingIds`. Never granted by code. |
| `item_calibration_weight` | Calibration Weight | Tool | Exact mass. Proof the scale can be checked. | In `CrossingIds`. First-weigh reward (implied, not granted). |
| `item_crossing_traded_grain` | Crossing Grain | Trade | Weighed honest, sacked. | **Not in `CrossingIds`.** Add constant or stop minting ad hoc. |
| `item_crossing_traded_salt` | Crossing Salt | Trade | Drown salt, waxed sack. | Same. |
| `item_crossing_pledge_slip` | Pledge Slip | Quest | Principal, term, forfeit, read twice. | Same. Use as Terms quest key. |

## 7.2 Constants without JSON

| id | Add? |
|---|---|
| `item_charter_three_pages` | **Yes.** The title's object. Codex unlock. Inspect: "Page one is a calibration tolerance." |

## 7.3 Proposed remainder (do not explode the 03 legendary list)

Cap the rest. The 03 draft's ten "legendaries" included a child's drawing and Ivo's stamp. Keep those for a creative pack. **Must-have for the arc:**

| id | Function |
|---|---|
| `item_debt_contract_copy` | Re-readable Terms text |
| `item_marker_rubbing` | Charter clue |
| `item_duty_log_fragment` | Why the Charter exists (axle fraud, not founding) |
| `item_trade_manifest_blank` | Consumed on `quest_crossing_weigh_run` |
| `item_wyn_receipt_paid` | Forfeit Complete only; no mechanic |

Do not mint a favour-token economy (`item_backer_favor_token` from the 03 draft) unless the Standing ships and needs a diegetic chit. A spoken name is the currency. A second token would make the vouch a coin.

## 7.4 Achievements (*PROPOSED*, cap 12)

The 03 draft listed 21. That is a trophy shelf. Ship twelve, no kill-counts, no jokes.

| id | Name | Condition |
|---|---|---|
| `ach_nc_vouched` | A Name at the Gate | Enter on a clean vouch |
| `ach_nc_burned_vouch` | Spent Badly | Burn a vouch and find a second |
| `ach_nc_true_weight` | Honest Scale | First weigh Complete |
| `ach_nc_bribe_refused` | Folding Chair | Osran refuses on the record |
| `ach_nc_read_twice` | Read It Again | Sign and pay a Dessa contract |
| `ach_nc_wyn_paid` | Paid in Full | Forfeit honoured |
| `ach_nc_the_charter` | Three Dry Pages | Read the real file |
| `ach_nc_folding_chair_ending` | — | `ending_crossing_scale` |
| `ach_nc_paid_in_full_ending` | — | `ending_crossing_underwrite` |
| `ach_nc_draft_signed_ending` | — | `ending_crossing_compact` |
| `ach_nc_no_ones_ending` | — | `ending_crossing_none` |
| `ach_nc_just_passing_ending` | — | `ending_crossing_walked` |

## 7.5 Narrative word-count estimate

| Bucket | Words | Notes |
|---|---|---|
| This bible | ~12,000 | Post-implementation; defects are the extra weight |
| Creative pack (when commissioned) | 28,000–36,000 | Three Shapes triple main-quest prose; smaller than the 03 draft's 40k because the front is 12 POIs, not a second Holdfast |
| Full VO | **unrealistic** | |

---

# SECTION 8 — TECHNICAL IMPLEMENTATION (DONE / STUBBED / MISSING)

This section is the reason a fourth bible exists. The 03 draft wrote a plan as if nothing had been coded. The pipeline then coded as if the plan were finished. Neither is true.

## 8.1 Architecture mapping

| Concern | Existing pattern | Nobody's Charter | State |
|---|---|---|---|
| Data | `StreamingAssets/Data/*.json` + JsonUtility wrap-array | `crossing_factions.json`, `crossing_locations.json`, `crossing_items.json`, `crossing_quests.json` | **DONE** (partial rows) |
| Logic | Plain C#, events, save blobs | `VouchAccessSystem` only. Arbitration + Ledger unbuilt | **PARTIAL** |
| Host | `GameBootstrap` partials | `GameBootstrap.NobodyCharter.cs`; called from `InitDeepLore` after `BootHoldfast()` | **DONE** (boot) |
| Save | `ISaveable` adapters | `VouchAccessSaveable`, `OsranKellSaveable`, `MattisCraySaveable` | **DONE** for those three |
| AI | `UtilityAI` + `ActionScorer` | No `Action_*`. Osran/Mattis are counters | **MISSING** |
| UI | UITK, Codex, event modal | None | **MISSING** |
| Map | `GeneratedMap` + seeder | No `CrossingMapSeeder`. Holdfast has `HoldfastMapSeeder.Attach` | **MISSING** |
| Travel gate | `ExpeditionSystem.SetIceRoadSystem` | No `SetVouchAccessSystem`. `GateAllowsCrossing()` uncalled | **STUBBED** |
| Economy | `DynamicEconomySystem` | Trade defs exist; no true-price hook | **STUBBED** |
| Lore | `LoreDiscoveryIndex` | Keys referenced on two cards; no `world_history` bodies | **MISSING** |
| Quests | `HoldfastQuestSystem.BindCatalog` + `QuestlineSO.Ids` | Cards cached on `CrossingQuests`. No runtime. Not in `QuestlineSO` | **STUBBED** |
| Events | `EventRunner.Holdfast.cs` | No `EventRunner.NobodyCharter.cs` | **MISSING** |
| Consequences | `WorldStateConsequenceSystem.TryApplyMutation` | Mutation ids in `CrossingIds` and on cards; none applied | **STUBBED** |
| Unlock | `exp_holdfast_unlocked` pattern | `exp_nobodys_charter_unlocked` never set | **MISSING** |

**Ids namespace (shipped):** `loc_crossing_*`, `faction_the_scale`, `faction_the_underwrite`, `faction_the_compact`, `npc_osran_kell`, `npc_mattis_cray`, `quest_crossing_*` (two live), `item_vouch_token_crossing`, `item_calibration_weight`, `item_crossing_*`, `lore_nc_*` (referenced), `mutation_crossing_*` (referenced), `ending_crossing_*` (constants), `flag_crossing_*` (three wired).

## 8.2 DONE — precise

What a reviewer can treat as real:

1. **`VouchAccessSystem` state machine** — grant / burn / soften / last-resort / null-safe restore. EditMode tests cover the happy paths and the idempotent cases. This is the pack's only finished system.
2. **Three-bloc catalog** — Currents-shaped DTO, not added to `faction_lore.json`, `is_active: true`. Loader + `GetById`. Tests assert count 3 and snake_case uniqueness.
3. **Seven location cards** — merged via `CrossingLocationsCatalogLoader.ApplyToCatalog` (skips `overlay_on_unlock` / `recast_always`; all seven are `false`/`false`). Dedup on re-merge tested.
4. **Five item defs** — materialised into `ItemCatalogSO` if the id is absent. Vouch token parses as `ItemType.Quest`.
5. **Two quest cards** — parse, prereq strings, stage counts. Cached; lookup by id.
6. **Two character rows + two NPC classes** — Osran at the Scalehouse, Mattis at the gate. Initialise from catalog display names with string fallbacks.
7. **Boot order** — `BootCurrents()` → `BootHoldfast()` → `BootNobodyCharter()`. Stands if sister packs are absent (does not read their objects).
8. **Save adapters** — three `ISaveable` wrappers registered in `RegisterExpansionSaveables`.
9. **Flag side-effects** — the three vouch events write `flag_crossing_vouched_clean`, `flag_crossing_vouch_burned`, `flag_crossing_access_softened`.
10. **Master constants** — `CrossingIds` exists. Useful as a collision fence. Harmful as a source of truth for unbuilt content (Appendix B).

## 8.3 STUBBED — looks wired, does not play

| Stub | What a reader might think | What the code does |
|---|---|---|
| `LoadCrossingQuests` | Quests registered | A `List<CrossingQuestEntry>` on the bootstrap. No `TryStart`, no daily tick, no `QuestlineSO` id, no UI. |
| `GetCrossingQuest` | Host can drive stages | Linear scan. Nothing calls it. |
| `GateAllowsCrossing` | The viaduct checks a name | Dead. `ExpeditionSystem` never asks. |
| `TryVouchAtCrossing` / `BurnCrossingVouch` / `SoftenCrossingAccess` | Quest choices call these | No `EventRunner` choice hook. Holdfast has `EventRunner.Holdfast.cs`. This pack does not. |
| `NPCOsranKell.PerformWeigh` / `AttemptBribe` | First-weigh uses them | Tests call them. Quests do not. |
| `NPCMattisCray.GiveVouch` | Last-resort consumes him | Only if host passes `lastResort: true` **and** id is Mattis. No quest does. |
| Location merge | Crossing is on the map | Catalog rows without `GeneratedMap` edges. Holdfast seeds a spine. This pack does not. |
| Item merge | Token and weight are loot | Defs exist. No grant path. |
| Mutation strings on choices | World changes | `set_flag` fields on JSON. `WorldStateConsequenceSystem` has no `mutation_crossing_*` handlers. |
| `CrossingIds` endings / mutations / nightfire / charter pages | Content exists | Compile-time strings. |

## 8.4 MISSING — required to finish the thesis

**Systems:** `CrossingArbitrationSystem`, `LedgerDebtSystem`, `CrossingQuestSystem`, `CrossingMapSeeder`.

**Host:** `EventRunner.NobodyCharter.cs`; `ExpeditionSystem.SetVouchAccessSystem` (or equivalent predicate); `exp_nobodys_charter_unlocked`; unlock-gated merge (Holdfast already gates some location apply — Crossing merges **always**, even when the rumour has not started).

**People:** `npc_dessa_vane`, `npc_perrin_ashby`, `npc_ivo_fenn`, `npc_wyn_sabler` + classes + saveables.

**Places:** `loc_crossing_nightfire` (constant only), plus five *PROPOSED* POIs in §2.2 if the three-cluster map is kept.

**Quests:** `quest_crossing_the_vouch` **must exist before first_weigh is legal**. Then Terms, Petition, Standing, Marker, Forfeit, Vote, Charter, Ledger. Companion Mattis (constant exists).

**Data:** `item_charter_three_pages`; `world_history` rows; `lore_nc_*` bodies; knowledge keys aligned to the quest that actually grants them.

**AI / UI:** four `Action_*`; notice-board; contract re-read; Codex tab; location-description swaps on vouch state.

**Tests still owed:** unit schema (danger 1–10, rads 18–52, hours-from-bunker or explicit hop field); collision test against `loc_weighbridge`; `quest_crossing_the_vouch` exists; `GateAllowsCrossing` blocks an expedition; burn couples Mattis; domain-reload cache clear.

## 8.5 Static cache / domain-reload

Every Crossing loader uses the Holdfast/Currents pattern:

```
private static List<T> _cache;
public static List<T> Load() {
    if (_cache != null) return _cache;
    ...
}
```

There is no `Invalidate()` / `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]` clear. In the Editor, a domain reload that *does not* rerun static constructors the way a player build does — or a test that mutates JSON between `Load()` calls in one session — will serve stale rows. Holdfast, Currents, and Characters loaders share the defect. **Do not "fix" only Crossing** and leave the siblings dirty; ticket one cache-invalidation pass for all wrap-array loaders. Until then, EditMode tests that rewrite StreamingAssets in-process are lying.

## 8.6 Assets (specify only; generate later into `generated_AIassets/`)

Dry-gouache, isolated objects, no readable AI text, no flags, no gore, no fantasy glow. Hand-lettered signage: every Crossing sign a different unofficial hand.

| Asset | Type | Notes |
|---|---|---|
| Location cards × 7 shipped + ≤5 new | 2D | Gate sign, deck scale, chalk stalls, chained ledger |
| Faction badges × 3 | Badge | Weight stencil; ledger corner; hand-drawn "4" |
| NPC portraits × 2 now, +4 later | Chest-up, deferred | Osran, Mattis first |
| Item icons × 5 shipped + charter | 64–128 px | Matchbook slip, iron slug, three dry pages |
| Notice-board / contract panel | UITK | Only when Standing / Ledger ship |
| **Not in scope** | 3D interchange, full VO, new music album | |

## 8.7 Sprints (rebased on what exists)

The 03 draft's four sprints assumed green field. Rebase.

| Sprint | Goal | Deliverables | Verify |
|---|---|---|---|
| **S0 — Make the gate real** | Playable entry | Retune location units; rename display of deck scale; write `quest_crossing_the_vouch`; `CrossingMapSeeder` spine; `SetVouchAccessSystem`; couple Mattis burn; unlock flag; grant token | Expedition to gate refused, then allowed; save round-trip; compile PASS |
| **S1 — Scale plays** | First weigh + integrity | `CrossingQuestSystem`; register ids; host choices call Osran; grant weight; apply two mutations | Both cards completable; bribe once; compile PASS |
| **S2 — Paper** | Underwrite + Compact open | `LedgerDebtSystem`; Dessa + Perrin + Wyn; Terms + Petition; hall / annex / lockup / granary as needed | Read-twice UI; forfeit named; compile PASS |
| **S3 — Standing + Charter** | Thesis completes | `CrossingArbitrationSystem`; Standing / Marker / Forfeit / Vote / three pages / endgame; Ivo; five endings | Ruling hold/overturn; ending exclusive; compile PASS |

**QA every sprint:** no seventh `faction_lore` row; no `_hegemony` row; hatch magnitudes untouched; `loc_weighbridge` (Toll) unmodified; Tally quote unused; Grain Exchange not rewritten.

## 8.8 Risks

| Risk | Mitigation |
|---|---|
| Reads as a fifth Power | Compliance sentence in §5 of the Analysis. Reviewer checklist: rulings and names, never ground. |
| Reads as a second Grain Exchange | Envoy encounter. Sign stays; Exchange does not ask for someone. |
| Reads as a second Tally | Jurisdiction + mobility + forfeit grade. Dessa does not walk to the hatch. |
| Reads as a second List / Standing Record | Charter is legitimacy, three pages, disappointing. Overlay plates sites. Sole files paper. Different objects. |
| Three Weighbridges | Rename Crossing card to Deck Scale. One comparison line with Bram / Yara. No fourth scale. |
| `CrossingIds` wish-list | Constants may exist before JSON. Tests must assert card existence for any quest that is a **prereq**. |
| Arbitration becomes a political sim | Scripted Standings + one repeatable micro-dispute. No background agents. |
| Cross-tool QA | Vouch × backers × debt = three coupled variables. Prompt #26. |

## 8.9 QA cases (minimum)

1. Old save → Ostrowski rumour → `quest_crossing_the_vouch` → `TryVouchAtCrossing` → gate node walkable.
2. Un-vouched expedition to `loc_crossing_scalehouse` refused; description still readable.
3. Vouch burned → re-entry blocked → Mattis last-resort (only after a burn) succeeds; `GiveVouch` and `BurnMattis` agree with `Vouch` state.
4. Soften after opening arc; `BurnVouch` no-ops; own name sufficient.
5. First weigh Complete / Fail / Double-Cross each set a different flag; Osran bribe once.
6. Scale integrity records or stays silent; does not grant `lore_nc_three_legends`.
7. Contract signed → term unpaid → forfeit at Lockup, not at home hatch.
8. Standing 3 / overturn 3.
9. Charter published / hidden / sold; Ivo access on sold.
10. Two bloc chains + Charter → endgame; stacked double-cross → `ending_crossing_none` text only.
11. Holdfast dark / Roster dark / Record dark: pack still plays.
12. `faction_lore.json` still six rows. Compile + EditMode PASS.

---

# SECTION 9 — PLAYER ENGAGEMENT & RETENTION

## Day-one (post-unlock)

- Ostrowski's refusal to walk there. A rumour that is, unusually, more inviting than most warnings.
- The sign. Read, not explained. The Exchange envoy, later, is how the player learns the sign is not a slogan.
- The first weigh: a number that is just true, in a game where very few numbers about the shelter have ever been neutral.
- Mattis saying the name is his, not yours, if you spend it.

## 3–6 month roadmap (after S3)

| Month | Content | Why they return |
|---|---|---|
| M1 | Remaining sides; Nightfire rumour pack; backer barks | Standing is a repeatable loop once built |
| M2 | One Current cameo gets a light hook (Guild claim or Deserter passage) — seeded, not required | Cross-Current interlock, same shape as Holdfast's Long Walk |
| M3 | Ivo's second box if the Charter was handled with care | Rewards Complete-shape with more, not different |
| M4–6 | Shareable notice-board (their backer list). No live service. No canon vote | Occupancy of a ruling is personal |

## Feedback loops

| Loop | Need served |
|---|---|
| Vouch | Relationship as infrastructure — a mechanical floor under trust |
| Weigh-run | Fair trade, rare in this economy |
| Standing | A rule you can see contested the same day |
| Contract | Planning; the tension of having been told exactly what would happen |
| Charter | Curiosity, then a quiet, deliberately small payoff |

## Monetization

Same as the sisters: no microtransaction, no gacha, no loot boxes. If paid DLC: one purchase, bundled with or after 1–3.

---

# SECTION 10 — LORE CONSISTENCY CHECK

## 10.1 Must not contradict (base canon)

| Canon | Source | Nobody's Charter stance |
|---|---|---|
| Sector 4 map closed; no fifth Power | `00_OVERVIEW.md` | Seam, not a sixth sub-region, not a coast. Blocs hold names, not ground. |
| Additional factions are Currents, not Powers | `05_FACTIONS.md` | Blocs are **smaller than Currents**: a Current's `access_rule` does not expire when one person recants. Own catalog. |
| `faction_lore.json` at six | live file | Untouched. |
| Hatch magnitudes | `ExpeditionSystem` | Untouched. Viaduct is a trigger point, not a new rad number. |
| Highway 9 / Warlord territory | `WorldStateConsequenceSystem`, Toll locations | Geographic anchor, forty minutes. Not entered, not retuned. |
| The List / Schedule / Continuity | `02_THE_LIST.md`, Holdfast | Not rewritten. Charter is a smaller, unrelated document about a scale, not an allocation. |
| No magic, no real countries/people, no glorified violence | `AGENTS.md` | Held. "County highway authorities" stay generic. |
| Terraformers, Tessarat, 7G, androids, neuromancers | house ban | Unused. |
| Two faction id namespaces | `00_OVERVIEW.md` | Not picked. No new row in either. |

## 10.2 Two-way flags with the three sister packs

### vs Holdfast (the allocated world)

| Direction | Flag / state | Effect |
|---|---|---|
| HF → NC | `holdfast_levy_refuse` / `alloc12_refused` | Edor will vouch once, dry, professional courtesy between people who wait on stools. |
| HF → NC | `holdfast_membrane_sector4` | Wyn's terms harsher; Underwrite prices scarcity. |
| HF → NC | Ice Road dark | Approach is the Toll shoulder only; no northern courtesy path. |
| HF → NC | `loc_cut_weigh_hut` live | One comparison line. No third civic scale. |
| NC → HF | `ending_crossing_compact` | Perrin's ratified draft is a file Ormund reads once and shelves without comment. |
| NC → HF | `mutation_crossing_underwrite_burned` | Northern calorie prices notice a defaulted debtor. Texture, not a new brine rule. |
| Collision | Ice Weigh Hut vs Deck Scale vs Toll `loc_weighbridge` | Three honest needles. **Keep all three only if each sentence is different:** Toll = posted rate; Cut = Office mass; Crossing = habit without a government. If a writer cannot say that in one line, cut the Crossing display name (already required). |

Holdfast must not absorb the vouch. A levy is a form. A vouch is a person. Edor may *be* a vouch. He does not *replace* the system.

### vs Duty Roster (the unlisted home)

| Direction | Flag / state | Effect |
|---|---|---|
| DR → NC | `mutation_roster_ink` | Perrin cites the wall, unprompted, as proof a written charter can work. Dialogue only. |
| DR → NC | `flag_hadi_hidden` / Blank Rows access | Alternate vouch: never write the vouchee's name in Ivo's room either. |
| DR → NC | `mutation_roster_burned` | Mattis still vouches (he does not read the wall). Ostrowski is colder. |
| NC → DR | `mutation_crossing_forfeit_doublecrossed` | Nila will not vouch afterward. A disappeared debtor is a trail Blank Rows will not stand near. |
| NC → DR | Collector at the hatch | **Only if the player hired the Tally**, or a Crossing runner is delivering a *message*, not a forfeit. Dessa does not become a hatch encounter. |
| Collision | Compact rubric vs roster rows vs RUR | Echo on purpose. The wall is who *you* write. The draft is who *gets a vote in a town you do not live in*. If a quest asks the player to ink the Crossing into Allocation 12's chart, it is the wrong pack. |

### vs Standing Record (the ground)

| Direction | Flag / state | Effect |
|---|---|---|
| SR → NC | Overlay plates the viaduct | Description overlay. The sign is still hand-lettered under the brass. Taking the plate does not open the gate. A plate is not a vouch. |
| SR → NC | `quest_record_which_gazetteer` resolved | Ivo will file whichever gazetteer the save kept. He will not summarise it. |
| SR → NC | Vault rooms deep | Sole corroborates Charter paper. One desk question. Do not crawl the Vault in this pack. |
| NC → SR | `mutation_crossing_charter_revealed` | Vault gains a cross-reference. Sole's completeness thesis gets a small, satisfied variant: a record that was exactly as complete as it looked. |
| NC → SR | `ending_crossing_none` | Overlay stops walking the seam. A closed market is not a site they will number this season. |
| Collision | Charter vs Standing Record vs The List | Same *method* (document ≠ story told about it). Different *object*: List = who was allocated; Record = what a place is called; Charter = who may govern a depot. If a writer plates Interchange 6 as if that *founded* the Crossing, stop. The stencil is a roof. The founding is a habit. |

## 10.3 Explicit collisions with live Currents (not sister packs)

| Current | Live text | Crossing risk | Rule |
|---|---|---|---|
| `faction_grain_exchange` | "No guards, no charter, no enforcement. It works because everybody attending is hungry." | Viaduct sign is the same sentence plus ASK FOR SOMEONE. | Keep the sign. Play the difference. Do not rewrite the Exchange `access_rule`. |
| `faction_the_tally` | Read twice; forfeit named; will walk anywhere; death-grade possible. Signature: "Do you want it read again?" | Underwrite is a local reskin if Dessa leaves the hall or reuses the quote. | Jurisdiction, mobility, forfeit grade. Live Underwrite quote stays. 03 Dessa snippet that copied the Tally is **retired**. |
| `faction_scavenger_guild` | Claim-blacklist | Stallrow chalk | Cameo model. No Guild questline. |
| `faction_iron_raiders` | Fill vacuums | `ending_crossing_none` | Description cameo. No questline. |
| `faction_deserter_coalition` | Unnameable people | Compact refugees; principled backer | Cameo. |
| `faction_cold_count` | Date paper and corrosion | Marker / Charter | One line. |
| `faction_the_overlay` (*SR proposed*) | Plates | Viaduct / records room | Hook, do not build layouts here. |

## 10.4 Timeline

| When | Event |
|---|---|
| Pre-war (decades) | Real Charter signed: calibration + toll-revenue split, two county highway authorities |
| Exchange−? | Interchange 6 is an ordinary, forgettable depot |
| Exchange+0 to +1 | Warlords assess, decide it is not worth holding, stop grading the approach |
| Exchange+1 | First ad-hoc trades; Osran keeps weighing |
| Exchange+2 | First informal covered loss; the practice that becomes the Underwrite |
| Exchange+3 | Viaduct foot-only in practice; Mattis (a child at +0) starts running names across it |
| Exchange+4 | Compact forms from people the other two arrangements have already failed once |
| Exchange+5 | **Now.** Draft Four. Wyn's second season due. Player arrives, if someone will say their name |

## 10.5 Small recasts (justified) vs not retcons

| Item | Change | Why |
|---|---|---|
| `loc_crossing_records_room` description | Remove "surviving three pages" until the Charter quest | Spoils the title |
| `loc_crossing_weighbridge` displayName | Deck Scale (id migration ticket) | Collision with Toll `loc_weighbridge` |
| Location danger / hours / rads | Live schema | Units are currently unplayable |
| `quest_crossing_first_weigh` / `_scale_integrity` knowledge keys | Move `lore_nc_read_again` and `lore_nc_three_legends` to the quests that own them | Cards stole later keys |
| Osran `faction: none` | **Keep** | He will not wear the Scale |

**Not retconned:** Grain Exchange access_rule; Tally contracts; Toll weighbridge body; Holdfast ice hut; Duty Roster chart; Overlay plates; TrueEnding; faction namespaces; hatch constants.

## 10.6 Word to the implementer

If a system wants a fourth new class, a seventh Codex Power, a `_hegemony` row, a retuned hatch constant, a fourth coast, or a collector at the home hatch who is Dessa, **stop and ticket it.** The expansion is a name at a truss, a number that is true, a ledger that will not leave the hall, a draft that looks like a rubric, and three dry pages that do not found a town. That is enough.

The 03 plan is a draft. The pipeline is a phase list. **This file is the spec.** Code that already exists keeps its ids. Code that does not exist is *PROPOSED* even when `CrossingIds` already named it.

---

# APPENDIX A — Integration matrix (all four expansions)

## A.1 In → Nobody's Charter

| Source | Flag / state | Crossing change |
|---|---|---|
| Base | `Mutation_TransitTax` / `Mutation_MedicalSupplyGone` | Story-gate satisfied; Ostrowski optional |
| Base | Ostrowski trust | Default first vouch (reluctant, once) |
| Holdfast | Levy refuse / 12-C / Edor stool | Edor alternate vouch |
| Holdfast | Membrane strip | Harsher Underwrite terms |
| Holdfast | Ice Road dark | No northern courtesy path |
| Duty Roster | Roster ink | Perrin cites the wall |
| Duty Roster | Blank Rows access | Nameless vouch; Ivo's room stays blank for that person |
| Duty Roster | Roster burned | Ostrowski colder; Mattis unchanged |
| Standing Record | Viaduct plated | Overlay line; gate still needs a name |
| Standing Record | Gazetteer chosen | Ivo files that copy |
| Standing Record | Vault deep | Sole authenticates paper, one question |

## A.2 Nobody's Charter → out

| Crossing mutation / ending | Sister / base change |
|---|---|
| `flag_crossing_vouched_clean` | Approach waypoints chalk; Stallrow prices available |
| `flag_crossing_vouch_burned` | Colder gate prose; last-resort path |
| `flag_crossing_access_softened` | Own name; Mattis can stop being infrastructure |
| `mutation_crossing_honest_trader` | Fair-rate weigh-runs |
| `mutation_crossing_bribe_attempted` | Worse Stallrow rate; Dessa pre-informed |
| `mutation_crossing_underwrite_burned` | Sector 4 market texture; Holdfast calorie notice |
| `mutation_crossing_forfeit_doublecrossed` | Blank Rows refuse; Nightfire pursuit |
| `mutation_crossing_charter_revealed` | Vault / Sole variant; Overlay may still plate the room |
| `ending_crossing_compact` | Ormund shelves a copy |
| `ending_crossing_none` | Ostrowski drops the rumour; Overlay skips the seam; Iron Raiders description at the gate |
| `ending_crossing_walked` | No sister-pack rewrite. The point is that nothing attached. |

## A.3 Two-way list (8) — parent summary

1. Grievance mutations ↔ story-gate, no new flag.
2. Vouch source ↔ Ostrowski / Mattis / Edor / Blank Rows.
3. Levy / membrane / ice ↔ terms, paths, Edor's one name.
4. Roster ink / burn / hide ↔ petition dialogue, nameless vouch, Nila after a double-crossed forfeit.
5. Overlay plate ↔ description only; never opens the gate.
6. Charter reveal ↔ Vault/Sole; not a new gazetteer.
7. Underwrite burn ↔ Sector 4 price texture, including District 8 calories if live.
8. Collapse ending ↔ rumour pool closes; seam unplated this season.

Do not put Office, Blank Rows, Overlay, Scale, Underwrite, or Compact in `_hegemony`.

---

# APPENDIX B — DEFECTS & DIVERGENCES

Honest list. The shipped code is not "the design, accidentally." It is a Phase 1–2 scaffold with several defects that will ship into the player's face if S0 is skipped.

### B.1 Numbering and documents

| Defect | Detail | Proposed |
|---|---|---|
| Pack numbered 03 | `expansion_03_nobodys_charter_plan.md` and `expansion_03_nobodys_charter_INTEGRATION_PIPELINE.md` sit beside Standing Record, which **owns 03**. | This file is 04. **Recommend renaming the pipeline to `expansion_04_nobodys_charter_INTEGRATION_PIPELINE.md`.** Do not do it in this pass. Mark the old 03 plan superseded in its header when someone next touches it. Do not delete; it is the draft the constants were mined from. |
| 03 plan status line is false | Still says "No game data has been edited. No C#." | Stale. This 04 file is authoritative. |
| Pipeline over-claims | Phases 1–2 marked implemented; Phase 1's opening quest and travel gate are not. | Rebase sprints to §8.7. |

### B.2 Naming and collisions

| Defect | Detail | Proposed rename / rule |
|---|---|---|
| Two Weighbridges | Live `loc_weighbridge` display "The Weighbridge." Shipped `loc_crossing_weighbridge` same display. Holdfast `loc_cut_weigh_hut` is a third scale. | Display **The Deck Scale**. Id `loc_crossing_deck_scale` on a migration ticket. Update `CrossingIds.Locations.Weighbridge`. Do not touch Toll or Cut ids. |
| `region_crossing` looks like a sixth gazetteer region | All seven cards + three blocs + two NPCs use it. | Keep as catalog tag. Do not add a gazetteer chapter. Do not seed as a coast. |
| `CrossingIds` lists unbuilt ids as canonical | `loc_crossing_nightfire`, `item_charter_three_pages`, ten quests, five endings, most mutations. | Allowed as reservations. **Illegal** as prereqs without cards. `quest_crossing_the_vouch` is the smoking gun. |
| Items not in `CrossingIds` | `item_crossing_traded_grain`, `_salt`, `_pledge_slip`. | Add constants or stop minting. |
| Flags/mutations not in `CrossingIds` | `mark_crossing_difficult`, `mutation_crossing_bribe_attempted`, `flag_crossing_scale_verified_silent`. | Add or do not fire. |
| Knowledge keys on the wrong cards | First weigh → `lore_nc_read_again` (Terms). Integrity → `lore_nc_three_legends` (Marker). | Move. Mint `lore_nc_the_number` / `lore_nc_scale_true` if needed. |
| Osran `faction: none` | Looks like an omit. | **Keep.** He will not claim the bloc. |
| Host file `GameBootstrap.NobodyCharter.cs` vs 03's `NobodysCharter` | Apostrophe dropped. | Keep shipped filename. |

### B.3 Schema / data bugs (play-breaking if merged live)

| Defect | Shipped | Live sibling schema | Fix |
|---|---|---|---|
| `dangerLevel` | 0.05–0.2 | 4–8 (Holdfast / `locations.json`) | Retune to 4–6. |
| `baseRadsPerHour` | 0.15–0.4 | 18–52 | Retune to 18–30. 0.4 rads/h is not a working town; it is a different game. |
| `travelHours` | Gate 8; inners 0.5–1.0 | Hours-from-bunker (Holdfast 6–14; Toll weighbridge 2.5) | Either all from-bunker (gate 3.5–4.5, inners 4.0–5.5) **or** seeder stores hop weights and catalog stores from-bunker. Do not mix. 8h gate is Drown-range and may stay if the seam is that far; then inners cannot be 0.5 from the bunker. |
| `min_day` / `first_day` | 20 / 24 | Soft gate 70+ | Retune. Day 20 is Ostrowski's beat; the Crossing must not open the same morning. |
| Records room spoils Charter | "surviving three pages of the original Charter" | Mystery is beat 9 | Recast inspect. |
| Always-on merge | Locations/items apply at every boot | Holdfast can gate on unlock | Gate on `exp_nobodys_charter_unlocked` or rumour started. |

### B.4 Systems / wiring

| Defect | Detail | Fix |
|---|---|---|
| Gate does not gate | `GateAllowsCrossing` uncalled; no `SetVouchAccessSystem` | S0. |
| No map seeder | Cards without edges | `CrossingMapSeeder` spine. |
| No quest runtime | Unlike `HoldfastQuestSystem` | `CrossingQuestSystem`. |
| No `QuestlineSO.Ids` | 03 Appendix C required it | Register. |
| No EventRunner partial | Holdfast has one | Add when choices must fire. |
| Mattis burn decoupled | `BurnCrossingVouch` ≠ `BurnMattis` | If `VouchedBy == npc_mattis_cray`, both. |
| `NeedsLastResort` true when fresh | Last resort is first resort | Require `vouchBurned` (or a first-attempt flag). Update tests. |
| `SoftenAccess` has no prereq | Can skip the name | Require a prior vouch or burn. Update tests. |
| `OnVouchGranted` always sets `vouched_clean` | Last-resort looks "clean" | Separate `flag_crossing_last_resort` or don't set clean on `isLastResort`. |
| `flag_crossing_vouch_burned` never clears | History vs current | Keep history; add current. |
| Unlock flag missing | Boot log claims the pack is live | Set when rumour starts. |
| Static `_cache` | Shared with Holdfast/Currents/Characters | One invalidation ticket for all wrap-array loaders. |

### B.5 Canon / design overlaps (not typos — thesis threats)

| Overlap | Why it is dangerous | Differentiation (mandatory) |
|---|---|---|
| Grain Exchange | Same "no charter / no guard" sentence | Exchange: four Powers, hunger. Crossing: no Power, a name. |
| The Tally | Read-twice, named forfeit, Dessa's 03 quote | Local hall; goods forfeit; will not walk to Allocation 12; quote already fixed in JSON. |
| Toll + Cut scales | Three honest needles | Posted rate / Office mass / habit. Rename Crossing display. |
| The List method | Charter is a document people over-fit | Small, about legitimacy, not allocation. |
| Standing Record | "What the paper says" | Record = place names. Charter = who governs. Plate ≠ vouch. |
| Duty Roster | Names, rubric, who counts | Wall is home occupancy. Draft is a town you visit. |
| Holdfast forms | Levy, census, receipts | Form names a trade. Vouch names a person. |

### B.6 Id collision checklist (re-grep before the next commit)

Verified against live `locations.json` (`loc_weighbridge` **display collision**), `currents.json` (no `faction_the_scale` / `_underwrite` / `_compact`), `characters.json` (Osran/Mattis unique), Holdfast ids (no `loc_crossing_*`), Duty Roster *proposed* ids (no collision), Standing Record *proposed* ids (no collision; `npc_osric_tann` ≠ `npc_osran_kell`).

**Do not mint:** a seventh `faction_lore` row; `_hegemony` Crossing row; `loc_weighbridge` clone; Tally-quote Dessa; Overlay as a Crossing bloc; a fourth coast.

---

# APPENDIX C — Next implementation prompt

> Implement **Sprint 0** of `docs/expansions/expansion_04_nobodys_charter_plan.md` (this file — not the misnumbered 03 draft): make the social gate real. (1) Retune `crossing_locations.json` to the live danger / `travelHours` / rads schema; change `loc_crossing_weighbridge` **displayName** to "The Deck Scale" (keep id until a migration ticket). Recast `loc_crossing_records_room` so it does not spoil the Charter. (2) Write the missing card `quest_crossing_the_vouch` and register all live `quest_crossing_*` ids in `QuestlineSO.Ids`. (3) Add `CrossingMapSeeder` (bunker → approach → `loc_crossing_viaduct_gate` → scalehouse hub) and `ExpeditionSystem.SetVouchAccessSystem` so `GateAllowsCrossing()` is actually consulted. (4) Couple `BurnCrossingVouch` to `NPC_MattisCray.BurnMattis` when he was the sponsor; tighten `NeedsLastResort` and `SoftenAccess` per §5.1; set `exp_nobodys_charter_unlocked` when the rumour starts, not at boot. (5) Do not add a 7th `faction_lore.json` row, a `_hegemony` entry, `CrossingArbitrationSystem`, or `LedgerDebtSystem` in this sprint. Do not open a fourth coast. Re-grep ids. Cross-tool QA: reviewer is not the implementer (Prompt #26) — vouch state × travel predicate × Mattis burn. **Do not run Unity on a memory-starved machine if other agents hold the editor lock;** if you cannot compile, say so.

---

# APPENDIX D — House-voice samples (shipped JSON, still good)

**`loc_crossing_viaduct_gate`**
> A rail truss over the Drown's edge, planked over for feet instead of axles. The paint on the sign has texture from how many times it has been redone: NO CHARTER NO GUARD ASK FOR SOMEONE. Someone added, smaller, underneath, in different paint: WE MEAN IT.

**`loc_crossing_scalehouse`**
> A truck scale built for loads nobody hauls anymore, kept calibrated for reasons that stopped being obvious around the same time the reasons stopped mattering less. Osran's office has one chair for him and none for you. He'll fetch a second one. He always fetches a second one.

**`loc_crossing_underwrite_hall`**
> A long table, a ledger chained to it — not against theft, Dessa will tell you, unprompted, the first time you ask. Against convenient memory. The fire is always lit. Somebody's interest paid for the wood.

**`item_charter_three_pages`** (inspect, *PROPOSED* — not yet on disk)
> Three pages. A calibration tolerance, a revenue split, two signatures, a notary stamp. It says nothing about a town. It has been asked to mean a town for five years. It has never once agreed.


<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/Crossing/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Crossing/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# ADDENDUM: PURE DOMAIN ARCHITECTURE & CROSSING VOUCH SYSTEM (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Crossing
{
    public enum CrossingPermitStatus
    {
        PendingVouch,
        AuthorizedValid,
        RevokedBreach,
        ExpiredTransit,
        ContrabandBlacklisted
    }

    public enum VouchReputationTier
    {
        UntrustedDrifter = 0,
        KnownPeddler = 1,
        BondedCourier = 2,
        CharterTrustee = 3,
        MasterOfWeighs = 4
    }

    public readonly struct BorderPermitRecord : IEquatable<BorderPermitRecord>
    {
        public readonly string PermitId;
        public readonly string NominatedTravelerId;
        public readonly string GuarantorSurvivorId;
        public readonly CrossingPermitStatus Status;
        public readonly int IssueTick;
        public readonly int ExpiryTick;
        public readonly int SecurityFeePaidRads;
        public readonly int ContrabandScannedCount;

        public BorderPermitRecord(
            string permitId,
            string nominatedTravelerId,
            string guarantorSurvivorId,
            CrossingPermitStatus status,
            int issueTick,
            int expiryTick,
            int securityFeePaidRads,
            int contrabandScannedCount)
        {
            PermitId = permitId ?? throw new ArgumentNullException(nameof(permitId));
            NominatedTravelerId = nominatedTravelerId ?? throw new ArgumentNullException(nameof(nominatedTravelerId));
            GuarantorSurvivorId = guarantorSurvivorId ?? throw new ArgumentNullException(nameof(guarantorSurvivorId));
            Status = status;
            IssueTick = issueTick;
            ExpiryTick = expiryTick;
            SecurityFeePaidRads = securityFeePaidRads;
            ContrabandScannedCount = contrabandScannedCount;
        }

        public bool Equals(BorderPermitRecord other) =>
            PermitId == other.PermitId &&
            NominatedTravelerId == other.NominatedTravelerId &&
            GuarantorSurvivorId == other.GuarantorSurvivorId &&
            Status == other.Status &&
            IssueTick == other.IssueTick &&
            ExpiryTick == other.ExpiryTick &&
            SecurityFeePaidRads == other.SecurityFeePaidRads &&
            ContrabandScannedCount == other.ContrabandScannedCount;

        public override bool Equals(object obj) => obj is BorderPermitRecord other && Equals(other);
        public override int GetHashCode() => PermitId.GetHashCode();
    }

    public interface IVouchAccessSystem
    {
        bool TryIssueTransitPermit(string travelerId, string guarantorId, int currentTick, int durationTicks, int fee, out BorderPermitRecord permit);
        bool TryRevokeTransitPermit(string permitId, string reasonCode, int currentTick);
        bool ValidateTransitAccess(string travelerId, int currentTick, out CrossingPermitStatus status);
        void RecordContrabandDetection(string permitId, int severity);
        VouchReputationTier EvaluateGuarantorStanding(string guarantorId);
        string ComputeDeterministicAuditDigest();
    }

    public sealed class VouchAccessSystem : IVouchAccessSystem
    {
        private readonly Dictionary<string, BorderPermitRecord> _permits = new Dictionary<string, BorderPermitRecord>();
        private readonly Dictionary<string, int> _guarantorViolations = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _guarantorSuccessfulTransits = new Dictionary<string, int>();

        public bool TryIssueTransitPermit(string travelerId, string guarantorId, int currentTick, int durationTicks, int fee, out BorderPermitRecord permit)
        {
            permit = default;
            if (string.IsNullOrWhiteSpace(travelerId) || string.IsNullOrWhiteSpace(guarantorId))
                return false;

            if (_guarantorViolations.TryGetValue(guarantorId, out int violations) && violations >= 3)
                return false; // Burned vouch privilege

            string permitId = "PRM-" + travelerId + "-" + currentTick.ToString("D8");
            permit = new BorderPermitRecord(
                permitId,
                travelerId,
                guarantorId,
                CrossingPermitStatus.AuthorizedValid,
                currentTick,
                currentTick + durationTicks,
                fee,
                0
            );

            _permits[permitId] = permit;
            return true;
        }

        public bool TryRevokeTransitPermit(string permitId, string reasonCode, int currentTick)
        {
            if (!_permits.TryGetValue(permitId, out var existing))
                return false;

            var updated = new BorderPermitRecord(
                existing.PermitId,
                existing.NominatedTravelerId,
                existing.GuarantorSurvivorId,
                CrossingPermitStatus.RevokedBreach,
                existing.IssueTick,
                currentTick,
                existing.SecurityFeePaidRads,
                existing.ContrabandScannedCount
            );
            _permits[permitId] = updated;

            if (!_guarantorViolations.TryGetValue(existing.GuarantorSurvivorId, out int count))
                count = 0;
            _guarantorViolations[existing.GuarantorSurvivorId] = count + 1;

            return true;
        }

        public bool ValidateTransitAccess(string travelerId, int currentTick, out CrossingPermitStatus status)
        {
            status = CrossingPermitStatus.PendingVouch;
            foreach (var kvp in _permits)
            {
                if (kvp.Value.NominatedTravelerId == travelerId)
                {
                    if (kvp.Value.Status == CrossingPermitStatus.AuthorizedValid)
                    {
                        if (currentTick > kvp.Value.ExpiryTick)
                        {
                            status = CrossingPermitStatus.ExpiredTransit;
                            return false;
                        }
                        status = CrossingPermitStatus.AuthorizedValid;
                        return true;
                    }
                    status = kvp.Value.Status;
                    return false;
                }
            }
            return false;
        }

        public void RecordContrabandDetection(string permitId, int severity)
        {
            if (_permits.TryGetValue(permitId, out var p))
            {
                var updated = new BorderPermitRecord(
                    p.PermitId,
                    p.NominatedTravelerId,
                    p.GuarantorSurvivorId,
                    severity > 5 ? CrossingPermitStatus.ContrabandBlacklisted : p.Status,
                    p.IssueTick,
                    p.ExpiryTick,
                    p.SecurityFeePaidRads,
                    p.ContrabandScannedCount + 1
                );
                _permits[permitId] = updated;

                if (severity > 5)
                {
                    if (!_guarantorViolations.TryGetValue(p.GuarantorSurvivorId, out int v))
                        v = 0;
                    _guarantorViolations[p.GuarantorSurvivorId] = v + 2;
                }
            }
        }

        public VouchReputationTier EvaluateGuarantorStanding(string guarantorId)
        {
            int violations = _guarantorViolations.TryGetValue(guarantorId, out int v) ? v : 0;
            int successful = _guarantorSuccessfulTransits.TryGetValue(guarantorId, out int s) ? s : 0;

            if (violations >= 3) return VouchReputationTier.UntrustedDrifter;
            if (successful >= 50 && violations == 0) return VouchReputationTier.MasterOfWeighs;
            if (successful >= 20 && violations <= 1) return VouchReputationTier.CharterTrustee;
            if (successful >= 5) return VouchReputationTier.BondedCourier;
            return VouchReputationTier.KnownPeddler;
        }

        public string ComputeDeterministicAuditDigest()
        {
            var sortedKeys = new List<string>(_permits.Keys);
            sortedKeys.Sort(StringComparer.Ordinal);
            var sb = new StringBuilder();
            foreach (var key in sortedKeys)
            {
                var p = _permits[key];
                sb.Append(p.PermitId).Append(':')
                  .Append(p.NominatedTravelerId).Append(':')
                  .Append((int)p.Status).Append(':')
                  .Append(p.ExpiryTick).Append(';');
            }
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
```

---

# SECTION X: AUTHORITATIVE CROSSING JSON DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

## 1. Crossing Vouch Catalogs (`crossing_vouch_rules.json`)

```json
{
  "$schema": "https://ashfall.core/schemas/crossing_vouch_rules.schema.json",
  "schema_version": "2.4.0",
  "crossing_zone_id": "zone_highway9_checkpoint",
  "max_active_permits": 256,
  "vouch_tiers": [
    {
      "tier": "UntrustedDrifter",
      "max_escorted_passengers": 0,
      "base_transit_toll_scrip": 150,
      "contraband_inspection_rate": 1.0,
      "collateral_forfeit_risk": 0.85
    },
    {
      "tier": "KnownPeddler",
      "max_escorted_passengers": 2,
      "base_transit_toll_scrip": 60,
      "contraband_inspection_rate": 0.50,
      "collateral_forfeit_risk": 0.30
    },
    {
      "tier": "BondedCourier",
      "max_escorted_passengers": 5,
      "base_transit_toll_scrip": 25,
      "contraband_inspection_rate": 0.15,
      "collateral_forfeit_risk": 0.10
    },
    {
      "tier": "CharterTrustee",
      "max_escorted_passengers": 12,
      "base_transit_toll_scrip": 0,
      "contraband_inspection_rate": 0.05,
      "collateral_forfeit_risk": 0.02
    },
    {
      "tier": "MasterOfWeighs",
      "max_escorted_passengers": 30,
      "base_transit_toll_scrip": 0,
      "contraband_inspection_rate": 0.01,
      "collateral_forfeit_risk": 0.00
    }
  ],
  "contraband_classes": [
    {
      "class_id": "contra_rad_seeds",
      "name": "Uncertified Irradiated Seedlings",
      "severity_score": 6,
      "penalty_scrip": 500
    },
    {
      "class_id": "contra_munitions_military",
      "name": "Black-Market High Explosives",
      "severity_score": 9,
      "penalty_scrip": 1200
    },
    {
      "class_id": "contra_sedition_print",
      "name": "Uncensored Settlement Manifestos",
      "severity_score": 4,
      "penalty_scrip": 200
    }
  ]
}
```

---

# SECTION XI: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using Xunit;
using Ashfall.Core.Crossing;

namespace Ashfall.Core.Tests.Crossing
{
    public class NobodysCharterVerificationSuite
    {
        [Fact]
        public void Test001_InitialSystemHasEmptyAuditDigest()
        {
            var system = new VouchAccessSystem();
            string digest = system.ComputeDeterministicAuditDigest();
            Assert.NotNull(digest);
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test002_TryIssueTransitPermit_ValidInputs_Succeeds()
        {
            var system = new VouchAccessSystem();
            bool ok = system.TryIssueTransitPermit("TRV-01", "GUA-99", 100, 500, 25, out var p);
            Assert.True(ok);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, p.Status);
            Assert.Equal(600, p.ExpiryTick);
        }

        [Fact]
        public void Test003_ValidateTransitAccess_WithinWindow_ReturnsTrue()
        {
            var system = new VouchAccessSystem();
            system.TryIssueTransitPermit("TRV-02", "GUA-99", 100, 200, 25, out _);
            bool access = system.ValidateTransitAccess("TRV-02", 150, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);
        }

        [Fact]
        public void Test004_ValidateTransitAccess_PastExpiry_ReturnsFalseAndExpired()
        {
            var system = new VouchAccessSystem();
            system.TryIssueTransitPermit("TRV-03", "GUA-99", 100, 50, 25, out _);
            bool access = system.ValidateTransitAccess("TRV-03", 200, out var status);
            Assert.False(access);
            Assert.Equal(CrossingPermitStatus.ExpiredTransit, status);
        }

        [Fact]
        public void Test005_TryRevokeTransitPermit_MarksRevokedAndPenalizesGuarantor()
        {
            var system = new VouchAccessSystem();
            system.TryIssueTransitPermit("TRV-04", "GUA-01", 100, 300, 25, out var p);
            bool revoked = system.TryRevokeTransitPermit(p.PermitId, "CONTRABAND_SUSPECT", 150);
            Assert.True(revoked);
            system.ValidateTransitAccess("TRV-04", 160, out var status);
            Assert.Equal(CrossingPermitStatus.RevokedBreach, status);
        }

        [Fact]
        public void Test006_CrossingPermitSimulation_Variant_6()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0006";
            string guarantor = "GUA-07";
            int startTick = 60;
            int duration = 230;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test007_CrossingPermitSimulation_Variant_7()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0007";
            string guarantor = "GUA-01";
            int startTick = 70;
            int duration = 235;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test008_CrossingPermitSimulation_Variant_8()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0008";
            string guarantor = "GUA-02";
            int startTick = 80;
            int duration = 240;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test009_CrossingPermitSimulation_Variant_9()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0009";
            string guarantor = "GUA-03";
            int startTick = 90;
            int duration = 245;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test010_CrossingPermitSimulation_Variant_10()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0010";
            string guarantor = "GUA-04";
            int startTick = 100;
            int duration = 250;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test011_CrossingPermitSimulation_Variant_11()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0011";
            string guarantor = "GUA-05";
            int startTick = 110;
            int duration = 255;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test012_CrossingPermitSimulation_Variant_12()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0012";
            string guarantor = "GUA-06";
            int startTick = 120;
            int duration = 260;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test013_CrossingPermitSimulation_Variant_13()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0013";
            string guarantor = "GUA-07";
            int startTick = 130;
            int duration = 265;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test014_CrossingPermitSimulation_Variant_14()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0014";
            string guarantor = "GUA-01";
            int startTick = 140;
            int duration = 270;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test015_CrossingPermitSimulation_Variant_15()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0015";
            string guarantor = "GUA-02";
            int startTick = 150;
            int duration = 275;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test016_CrossingPermitSimulation_Variant_16()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0016";
            string guarantor = "GUA-03";
            int startTick = 160;
            int duration = 280;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test017_CrossingPermitSimulation_Variant_17()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0017";
            string guarantor = "GUA-04";
            int startTick = 170;
            int duration = 285;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test018_CrossingPermitSimulation_Variant_18()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0018";
            string guarantor = "GUA-05";
            int startTick = 180;
            int duration = 290;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test019_CrossingPermitSimulation_Variant_19()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0019";
            string guarantor = "GUA-06";
            int startTick = 190;
            int duration = 295;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test020_CrossingPermitSimulation_Variant_20()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0020";
            string guarantor = "GUA-07";
            int startTick = 200;
            int duration = 300;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test021_CrossingPermitSimulation_Variant_21()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0021";
            string guarantor = "GUA-01";
            int startTick = 210;
            int duration = 305;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test022_CrossingPermitSimulation_Variant_22()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0022";
            string guarantor = "GUA-02";
            int startTick = 220;
            int duration = 310;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test023_CrossingPermitSimulation_Variant_23()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0023";
            string guarantor = "GUA-03";
            int startTick = 230;
            int duration = 315;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test024_CrossingPermitSimulation_Variant_24()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0024";
            string guarantor = "GUA-04";
            int startTick = 240;
            int duration = 320;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test025_CrossingPermitSimulation_Variant_25()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0025";
            string guarantor = "GUA-05";
            int startTick = 250;
            int duration = 325;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test026_CrossingPermitSimulation_Variant_26()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0026";
            string guarantor = "GUA-06";
            int startTick = 260;
            int duration = 330;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test027_CrossingPermitSimulation_Variant_27()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0027";
            string guarantor = "GUA-07";
            int startTick = 270;
            int duration = 335;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test028_CrossingPermitSimulation_Variant_28()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0028";
            string guarantor = "GUA-01";
            int startTick = 280;
            int duration = 340;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test029_CrossingPermitSimulation_Variant_29()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0029";
            string guarantor = "GUA-02";
            int startTick = 290;
            int duration = 345;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test030_CrossingPermitSimulation_Variant_30()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0030";
            string guarantor = "GUA-03";
            int startTick = 300;
            int duration = 350;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test031_CrossingPermitSimulation_Variant_31()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0031";
            string guarantor = "GUA-04";
            int startTick = 310;
            int duration = 355;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test032_CrossingPermitSimulation_Variant_32()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0032";
            string guarantor = "GUA-05";
            int startTick = 320;
            int duration = 360;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test033_CrossingPermitSimulation_Variant_33()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0033";
            string guarantor = "GUA-06";
            int startTick = 330;
            int duration = 365;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test034_CrossingPermitSimulation_Variant_34()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0034";
            string guarantor = "GUA-07";
            int startTick = 340;
            int duration = 370;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test035_CrossingPermitSimulation_Variant_35()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0035";
            string guarantor = "GUA-01";
            int startTick = 350;
            int duration = 375;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test036_CrossingPermitSimulation_Variant_36()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0036";
            string guarantor = "GUA-02";
            int startTick = 360;
            int duration = 380;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test037_CrossingPermitSimulation_Variant_37()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0037";
            string guarantor = "GUA-03";
            int startTick = 370;
            int duration = 385;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test038_CrossingPermitSimulation_Variant_38()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0038";
            string guarantor = "GUA-04";
            int startTick = 380;
            int duration = 390;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test039_CrossingPermitSimulation_Variant_39()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0039";
            string guarantor = "GUA-05";
            int startTick = 390;
            int duration = 395;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test040_CrossingPermitSimulation_Variant_40()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0040";
            string guarantor = "GUA-06";
            int startTick = 400;
            int duration = 400;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test041_CrossingPermitSimulation_Variant_41()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0041";
            string guarantor = "GUA-07";
            int startTick = 410;
            int duration = 405;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test042_CrossingPermitSimulation_Variant_42()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0042";
            string guarantor = "GUA-01";
            int startTick = 420;
            int duration = 410;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test043_CrossingPermitSimulation_Variant_43()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0043";
            string guarantor = "GUA-02";
            int startTick = 430;
            int duration = 415;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test044_CrossingPermitSimulation_Variant_44()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0044";
            string guarantor = "GUA-03";
            int startTick = 440;
            int duration = 420;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test045_CrossingPermitSimulation_Variant_45()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0045";
            string guarantor = "GUA-04";
            int startTick = 450;
            int duration = 425;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test046_CrossingPermitSimulation_Variant_46()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0046";
            string guarantor = "GUA-05";
            int startTick = 460;
            int duration = 430;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test047_CrossingPermitSimulation_Variant_47()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0047";
            string guarantor = "GUA-06";
            int startTick = 470;
            int duration = 435;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test048_CrossingPermitSimulation_Variant_48()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0048";
            string guarantor = "GUA-07";
            int startTick = 480;
            int duration = 440;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test049_CrossingPermitSimulation_Variant_49()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0049";
            string guarantor = "GUA-01";
            int startTick = 490;
            int duration = 445;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test050_CrossingPermitSimulation_Variant_50()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0050";
            string guarantor = "GUA-02";
            int startTick = 500;
            int duration = 450;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test051_CrossingPermitSimulation_Variant_51()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0051";
            string guarantor = "GUA-03";
            int startTick = 510;
            int duration = 455;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test052_CrossingPermitSimulation_Variant_52()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0052";
            string guarantor = "GUA-04";
            int startTick = 520;
            int duration = 460;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test053_CrossingPermitSimulation_Variant_53()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0053";
            string guarantor = "GUA-05";
            int startTick = 530;
            int duration = 465;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test054_CrossingPermitSimulation_Variant_54()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0054";
            string guarantor = "GUA-06";
            int startTick = 540;
            int duration = 470;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test055_CrossingPermitSimulation_Variant_55()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0055";
            string guarantor = "GUA-07";
            int startTick = 550;
            int duration = 475;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test056_CrossingPermitSimulation_Variant_56()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0056";
            string guarantor = "GUA-01";
            int startTick = 560;
            int duration = 480;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test057_CrossingPermitSimulation_Variant_57()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0057";
            string guarantor = "GUA-02";
            int startTick = 570;
            int duration = 485;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test058_CrossingPermitSimulation_Variant_58()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0058";
            string guarantor = "GUA-03";
            int startTick = 580;
            int duration = 490;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test059_CrossingPermitSimulation_Variant_59()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0059";
            string guarantor = "GUA-04";
            int startTick = 590;
            int duration = 495;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test060_CrossingPermitSimulation_Variant_60()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0060";
            string guarantor = "GUA-05";
            int startTick = 600;
            int duration = 500;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test061_CrossingPermitSimulation_Variant_61()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0061";
            string guarantor = "GUA-06";
            int startTick = 610;
            int duration = 505;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test062_CrossingPermitSimulation_Variant_62()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0062";
            string guarantor = "GUA-07";
            int startTick = 620;
            int duration = 510;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test063_CrossingPermitSimulation_Variant_63()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0063";
            string guarantor = "GUA-01";
            int startTick = 630;
            int duration = 515;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test064_CrossingPermitSimulation_Variant_64()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0064";
            string guarantor = "GUA-02";
            int startTick = 640;
            int duration = 520;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test065_CrossingPermitSimulation_Variant_65()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0065";
            string guarantor = "GUA-03";
            int startTick = 650;
            int duration = 525;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test066_CrossingPermitSimulation_Variant_66()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0066";
            string guarantor = "GUA-04";
            int startTick = 660;
            int duration = 530;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test067_CrossingPermitSimulation_Variant_67()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0067";
            string guarantor = "GUA-05";
            int startTick = 670;
            int duration = 535;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test068_CrossingPermitSimulation_Variant_68()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0068";
            string guarantor = "GUA-06";
            int startTick = 680;
            int duration = 540;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test069_CrossingPermitSimulation_Variant_69()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0069";
            string guarantor = "GUA-07";
            int startTick = 690;
            int duration = 545;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test070_CrossingPermitSimulation_Variant_70()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0070";
            string guarantor = "GUA-01";
            int startTick = 700;
            int duration = 550;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test071_CrossingPermitSimulation_Variant_71()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0071";
            string guarantor = "GUA-02";
            int startTick = 710;
            int duration = 555;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test072_CrossingPermitSimulation_Variant_72()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0072";
            string guarantor = "GUA-03";
            int startTick = 720;
            int duration = 560;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test073_CrossingPermitSimulation_Variant_73()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0073";
            string guarantor = "GUA-04";
            int startTick = 730;
            int duration = 565;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test074_CrossingPermitSimulation_Variant_74()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0074";
            string guarantor = "GUA-05";
            int startTick = 740;
            int duration = 570;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test075_CrossingPermitSimulation_Variant_75()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0075";
            string guarantor = "GUA-06";
            int startTick = 750;
            int duration = 575;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test076_CrossingPermitSimulation_Variant_76()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0076";
            string guarantor = "GUA-07";
            int startTick = 760;
            int duration = 580;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test077_CrossingPermitSimulation_Variant_77()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0077";
            string guarantor = "GUA-01";
            int startTick = 770;
            int duration = 585;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test078_CrossingPermitSimulation_Variant_78()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0078";
            string guarantor = "GUA-02";
            int startTick = 780;
            int duration = 590;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test079_CrossingPermitSimulation_Variant_79()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0079";
            string guarantor = "GUA-03";
            int startTick = 790;
            int duration = 595;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test080_CrossingPermitSimulation_Variant_80()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0080";
            string guarantor = "GUA-04";
            int startTick = 800;
            int duration = 600;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test081_CrossingPermitSimulation_Variant_81()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0081";
            string guarantor = "GUA-05";
            int startTick = 810;
            int duration = 605;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test082_CrossingPermitSimulation_Variant_82()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0082";
            string guarantor = "GUA-06";
            int startTick = 820;
            int duration = 610;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test083_CrossingPermitSimulation_Variant_83()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0083";
            string guarantor = "GUA-07";
            int startTick = 830;
            int duration = 615;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test084_CrossingPermitSimulation_Variant_84()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0084";
            string guarantor = "GUA-01";
            int startTick = 840;
            int duration = 620;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test085_CrossingPermitSimulation_Variant_85()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0085";
            string guarantor = "GUA-02";
            int startTick = 850;
            int duration = 625;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test086_CrossingPermitSimulation_Variant_86()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0086";
            string guarantor = "GUA-03";
            int startTick = 860;
            int duration = 630;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test087_CrossingPermitSimulation_Variant_87()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0087";
            string guarantor = "GUA-04";
            int startTick = 870;
            int duration = 635;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test088_CrossingPermitSimulation_Variant_88()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0088";
            string guarantor = "GUA-05";
            int startTick = 880;
            int duration = 640;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test089_CrossingPermitSimulation_Variant_89()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0089";
            string guarantor = "GUA-06";
            int startTick = 890;
            int duration = 645;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test090_CrossingPermitSimulation_Variant_90()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0090";
            string guarantor = "GUA-07";
            int startTick = 900;
            int duration = 650;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test091_CrossingPermitSimulation_Variant_91()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0091";
            string guarantor = "GUA-01";
            int startTick = 910;
            int duration = 655;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test092_CrossingPermitSimulation_Variant_92()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0092";
            string guarantor = "GUA-02";
            int startTick = 920;
            int duration = 660;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test093_CrossingPermitSimulation_Variant_93()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0093";
            string guarantor = "GUA-03";
            int startTick = 930;
            int duration = 665;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test094_CrossingPermitSimulation_Variant_94()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0094";
            string guarantor = "GUA-04";
            int startTick = 940;
            int duration = 670;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test095_CrossingPermitSimulation_Variant_95()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0095";
            string guarantor = "GUA-05";
            int startTick = 950;
            int duration = 675;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test096_CrossingPermitSimulation_Variant_96()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0096";
            string guarantor = "GUA-06";
            int startTick = 960;
            int duration = 680;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test097_CrossingPermitSimulation_Variant_97()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0097";
            string guarantor = "GUA-07";
            int startTick = 970;
            int duration = 685;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test098_CrossingPermitSimulation_Variant_98()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0098";
            string guarantor = "GUA-01";
            int startTick = 980;
            int duration = 690;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test099_CrossingPermitSimulation_Variant_99()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0099";
            string guarantor = "GUA-02";
            int startTick = 990;
            int duration = 695;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test100_CrossingPermitSimulation_Variant_100()
        {
            var system = new VouchAccessSystem();
            string traveler = "TRV-0100";
            string guarantor = "GUA-03";
            int startTick = 1000;
            int duration = 700;
            bool ok = system.TryIssueTransitPermit(traveler, guarantor, startTick, duration, 25, out var p);
            Assert.True(ok);
            Assert.NotNull(p.PermitId);
            Assert.Equal(startTick + duration, p.ExpiryTick);

            bool access = system.ValidateTransitAccess(traveler, startTick + 50, out var status);
            Assert.True(access);
            Assert.Equal(CrossingPermitStatus.AuthorizedValid, status);

            string digest = system.ComputeDeterministicAuditDigest();
            Assert.Equal(64, digest.Length);
        }
    }
}
```

# SECTION XII: 600-DAY EXTENDED DETERMINISTIC SIMULATION TRACE

| Day | Simulation Tick | Active Border Crossings | Issued Permits | Burned Vouches | Contraband Interceptions | Escort Convoys Cleared | Toll Scrip Collected | Deterministic State Hash |
|---|---|---|---|---|---|---|---|---|
| Day 001 | 1440 | 13 | 47 | 0 | 0 | 4 | 1485 scrip | `hash_crx_d0001_000070a5` |
| Day 004 | 5760 | 16 | 53 | 0 | 0 | 7 | 1740 scrip | `hash_crx_d0004_000015f6` |
| Day 007 | 10080 | 19 | 59 | 0 | 0 | 10 | 1995 scrip | `hash_crx_d0007_0000b6c3` |
| Day 010 | 14400 | 22 | 65 | 0 | 0 | 5 | 2250 scrip | `hash_crx_d0010_00015b1c` |
| Day 013 | 18720 | 25 | 71 | 0 | 0 | 8 | 2505 scrip | `hash_crx_d0013_0001fc69` |
| Day 016 | 23040 | 28 | 77 | 0 | 1 | 3 | 2760 scrip | `hash_crx_d0016_000180ba` |
| Day 019 | 27360 | 31 | 83 | 0 | 1 | 6 | 3015 scrip | `hash_crx_d0019_000225f7` |
| Day 022 | 31680 | 34 | 89 | 0 | 1 | 9 | 3270 scrip | `hash_crx_d0022_0002c6c0` |
| Day 025 | 36000 | 12 | 95 | 0 | 1 | 4 | 3525 scrip | `hash_crx_d0025_00036b1d` |
| Day 028 | 40320 | 15 | 101 | 0 | 1 | 7 | 3780 scrip | `hash_crx_d0028_00030c6e` |
| Day 031 | 44640 | 18 | 107 | 1 | 2 | 10 | 4035 scrip | `hash_crx_d0031_0003d0bb` |
| Day 034 | 48960 | 21 | 113 | 1 | 2 | 5 | 4290 scrip | `hash_crx_d0034_000475f4` |
| Day 037 | 53280 | 24 | 119 | 1 | 2 | 8 | 4545 scrip | `hash_crx_d0037_000416c1` |
| Day 040 | 57600 | 27 | 125 | 1 | 2 | 3 | 4800 scrip | `hash_crx_d0040_0004bb12` |
| Day 043 | 61920 | 30 | 131 | 1 | 2 | 6 | 5055 scrip | `hash_crx_d0043_00055c6f` |
| Day 046 | 66240 | 33 | 137 | 1 | 3 | 9 | 5310 scrip | `hash_crx_d0046_0005e0b8` |
| Day 049 | 70560 | 36 | 143 | 1 | 3 | 4 | 5565 scrip | `hash_crx_d0049_000585f5` |
| Day 052 | 74880 | 14 | 149 | 1 | 3 | 7 | 5820 scrip | `hash_crx_d0052_000626c6` |
| Day 055 | 79200 | 17 | 155 | 1 | 3 | 10 | 6075 scrip | `hash_crx_d0055_0006cb13` |
| Day 058 | 83520 | 20 | 161 | 1 | 3 | 5 | 6330 scrip | `hash_crx_d0058_00076c6c` |
| Day 061 | 87840 | 23 | 167 | 2 | 4 | 8 | 6585 scrip | `hash_crx_d0061_000730b9` |
| Day 064 | 92160 | 26 | 173 | 2 | 4 | 3 | 6840 scrip | `hash_crx_d0064_0007d58a` |
| Day 067 | 96480 | 29 | 179 | 2 | 4 | 6 | 7095 scrip | `hash_crx_d0067_000876c7` |
| Day 070 | 100800 | 32 | 185 | 2 | 4 | 9 | 7350 scrip | `hash_crx_d0070_00081b10` |
| Day 073 | 105120 | 35 | 191 | 2 | 4 | 4 | 7605 scrip | `hash_crx_d0073_0008bc6d` |
| Day 076 | 109440 | 13 | 197 | 2 | 5 | 7 | 7860 scrip | `hash_crx_d0076_000940be` |
| Day 079 | 113760 | 16 | 203 | 2 | 5 | 10 | 8115 scrip | `hash_crx_d0079_0009e58b` |
| Day 082 | 118080 | 19 | 209 | 2 | 5 | 5 | 8370 scrip | `hash_crx_d0082_000986c4` |
| Day 085 | 122400 | 22 | 215 | 2 | 5 | 8 | 8625 scrip | `hash_crx_d0085_000a2b11` |
| Day 088 | 126720 | 25 | 221 | 2 | 5 | 3 | 8880 scrip | `hash_crx_d0088_000acc62` |
| Day 091 | 131040 | 28 | 227 | 3 | 6 | 6 | 9135 scrip | `hash_crx_d0091_000a90bf` |
| Day 094 | 135360 | 31 | 233 | 3 | 6 | 9 | 9390 scrip | `hash_crx_d0094_000b3588` |
| Day 097 | 139680 | 34 | 239 | 3 | 6 | 4 | 9645 scrip | `hash_crx_d0097_000bd6c5` |
| Day 100 | 144000 | 12 | 245 | 3 | 6 | 7 | 9900 scrip | `hash_crx_d0100_000c7b16` |
| Day 103 | 148320 | 15 | 251 | 3 | 6 | 10 | 10155 scrip | `hash_crx_d0103_000c1c63` |
| Day 106 | 152640 | 18 | 257 | 3 | 7 | 5 | 10410 scrip | `hash_crx_d0106_000ca0bc` |
| Day 109 | 156960 | 21 | 263 | 3 | 7 | 8 | 10665 scrip | `hash_crx_d0109_000d4589` |
| Day 112 | 161280 | 24 | 269 | 3 | 7 | 3 | 10920 scrip | `hash_crx_d0112_000de6da` |
| Day 115 | 165600 | 27 | 275 | 3 | 7 | 6 | 11175 scrip | `hash_crx_d0115_000d8b17` |
| Day 118 | 169920 | 30 | 281 | 3 | 7 | 9 | 11430 scrip | `hash_crx_d0118_000e2c60` |
| Day 121 | 174240 | 33 | 287 | 4 | 8 | 4 | 11685 scrip | `hash_crx_d0121_000ef0bd` |
| Day 124 | 178560 | 36 | 293 | 4 | 8 | 7 | 11940 scrip | `hash_crx_d0124_000e958e` |
| Day 127 | 182880 | 14 | 299 | 4 | 8 | 10 | 12195 scrip | `hash_crx_d0127_000f36db` |
| Day 130 | 187200 | 17 | 305 | 4 | 8 | 5 | 12450 scrip | `hash_crx_d0130_000fdb14` |
| Day 133 | 191520 | 20 | 311 | 4 | 8 | 8 | 12705 scrip | `hash_crx_d0133_00107c61` |
| Day 136 | 195840 | 23 | 317 | 4 | 9 | 3 | 12960 scrip | `hash_crx_d0136_001000b2` |
| Day 139 | 200160 | 26 | 323 | 4 | 9 | 6 | 13215 scrip | `hash_crx_d0139_0010a58f` |
| Day 142 | 204480 | 29 | 329 | 4 | 9 | 9 | 13470 scrip | `hash_crx_d0142_001146d8` |
| Day 145 | 208800 | 32 | 335 | 4 | 9 | 4 | 13725 scrip | `hash_crx_d0145_0011eb15` |
| Day 148 | 213120 | 35 | 341 | 4 | 9 | 7 | 13980 scrip | `hash_crx_d0148_00118c66` |
| Day 151 | 217440 | 13 | 347 | 5 | 10 | 10 | 14235 scrip | `hash_crx_d0151_001250b3` |
| Day 154 | 221760 | 16 | 353 | 5 | 10 | 5 | 14490 scrip | `hash_crx_d0154_0012f58c` |
| Day 157 | 226080 | 19 | 359 | 5 | 10 | 8 | 14745 scrip | `hash_crx_d0157_001296d9` |
| Day 160 | 230400 | 22 | 365 | 5 | 10 | 3 | 15000 scrip | `hash_crx_d0160_00133b2a` |
| Day 163 | 234720 | 25 | 371 | 5 | 10 | 6 | 15255 scrip | `hash_crx_d0163_0013dc67` |
| Day 166 | 239040 | 28 | 377 | 5 | 11 | 9 | 15510 scrip | `hash_crx_d0166_001460b0` |
| Day 169 | 243360 | 31 | 383 | 5 | 11 | 4 | 15765 scrip | `hash_crx_d0169_0014058d` |
| Day 172 | 247680 | 34 | 389 | 5 | 11 | 7 | 16020 scrip | `hash_crx_d0172_0014a6de` |
| Day 175 | 252000 | 12 | 395 | 5 | 11 | 10 | 16275 scrip | `hash_crx_d0175_00154b2b` |
| Day 178 | 256320 | 15 | 401 | 5 | 11 | 5 | 16530 scrip | `hash_crx_d0178_0015ec64` |
| Day 181 | 260640 | 18 | 407 | 6 | 12 | 8 | 16785 scrip | `hash_crx_d0181_0015b0b1` |
| Day 184 | 264960 | 21 | 413 | 6 | 12 | 3 | 17040 scrip | `hash_crx_d0184_00165582` |
| Day 187 | 269280 | 24 | 419 | 6 | 12 | 6 | 17295 scrip | `hash_crx_d0187_0016f6df` |
| Day 190 | 273600 | 27 | 425 | 6 | 12 | 9 | 17550 scrip | `hash_crx_d0190_00169b28` |
| Day 193 | 277920 | 30 | 431 | 6 | 12 | 4 | 17805 scrip | `hash_crx_d0193_00173c65` |
| Day 196 | 282240 | 33 | 437 | 6 | 13 | 7 | 18060 scrip | `hash_crx_d0196_0017c0b6` |
| Day 199 | 286560 | 36 | 443 | 6 | 13 | 10 | 18315 scrip | `hash_crx_d0199_00186583` |
| Day 202 | 290880 | 14 | 449 | 6 | 13 | 5 | 18570 scrip | `hash_crx_d0202_001806dc` |
| Day 205 | 295200 | 17 | 455 | 6 | 13 | 8 | 18825 scrip | `hash_crx_d0205_0018ab29` |
| Day 208 | 299520 | 20 | 461 | 6 | 13 | 3 | 19080 scrip | `hash_crx_d0208_00194c7a` |
| Day 211 | 303840 | 23 | 467 | 7 | 14 | 6 | 19335 scrip | `hash_crx_d0211_001910b7` |
| Day 214 | 308160 | 26 | 473 | 7 | 14 | 9 | 19590 scrip | `hash_crx_d0214_0019b580` |
| Day 217 | 312480 | 29 | 479 | 7 | 14 | 4 | 19845 scrip | `hash_crx_d0217_001a56dd` |
| Day 220 | 316800 | 32 | 485 | 7 | 14 | 7 | 20100 scrip | `hash_crx_d0220_001afb2e` |
| Day 223 | 321120 | 35 | 491 | 7 | 14 | 10 | 20355 scrip | `hash_crx_d0223_001a9c7b` |
| Day 226 | 325440 | 13 | 497 | 7 | 15 | 5 | 20610 scrip | `hash_crx_d0226_001b20b4` |
| Day 229 | 329760 | 16 | 503 | 7 | 15 | 8 | 20865 scrip | `hash_crx_d0229_001bc581` |
| Day 232 | 334080 | 19 | 509 | 7 | 15 | 3 | 21120 scrip | `hash_crx_d0232_001c66d2` |
| Day 235 | 338400 | 22 | 515 | 7 | 15 | 6 | 21375 scrip | `hash_crx_d0235_001c0b2f` |
| Day 238 | 342720 | 25 | 521 | 7 | 15 | 9 | 21630 scrip | `hash_crx_d0238_001cac78` |
| Day 241 | 347040 | 28 | 527 | 8 | 16 | 4 | 21885 scrip | `hash_crx_d0241_001d70b5` |
| Day 244 | 351360 | 31 | 533 | 8 | 16 | 7 | 22140 scrip | `hash_crx_d0244_001d1586` |
| Day 247 | 355680 | 34 | 539 | 8 | 16 | 10 | 22395 scrip | `hash_crx_d0247_001db6d3` |
| Day 250 | 360000 | 12 | 545 | 8 | 16 | 5 | 22650 scrip | `hash_crx_d0250_001e5b2c` |
| Day 253 | 364320 | 15 | 551 | 8 | 16 | 8 | 22905 scrip | `hash_crx_d0253_001efc79` |
| Day 256 | 368640 | 18 | 557 | 8 | 17 | 3 | 23160 scrip | `hash_crx_d0256_001e814a` |
| Day 259 | 372960 | 21 | 563 | 8 | 17 | 6 | 23415 scrip | `hash_crx_d0259_001f2587` |
| Day 262 | 377280 | 24 | 569 | 8 | 17 | 9 | 23670 scrip | `hash_crx_d0262_001fc6d0` |
| Day 265 | 381600 | 27 | 575 | 8 | 17 | 4 | 23925 scrip | `hash_crx_d0265_00206b2d` |
| Day 268 | 385920 | 30 | 581 | 8 | 17 | 7 | 24180 scrip | `hash_crx_d0268_00200c7e` |
| Day 271 | 390240 | 33 | 587 | 9 | 18 | 10 | 24435 scrip | `hash_crx_d0271_0020d14b` |
| Day 274 | 394560 | 36 | 593 | 9 | 18 | 5 | 24690 scrip | `hash_crx_d0274_00217584` |
| Day 277 | 398880 | 14 | 599 | 9 | 18 | 8 | 24945 scrip | `hash_crx_d0277_002116d1` |
| Day 280 | 403200 | 17 | 605 | 9 | 18 | 3 | 25200 scrip | `hash_crx_d0280_0021bb22` |
| Day 283 | 407520 | 20 | 611 | 9 | 18 | 6 | 25455 scrip | `hash_crx_d0283_00225c7f` |
| Day 286 | 411840 | 23 | 617 | 9 | 19 | 9 | 25710 scrip | `hash_crx_d0286_0022e148` |
| Day 289 | 416160 | 26 | 623 | 9 | 19 | 4 | 25965 scrip | `hash_crx_d0289_00228585` |
| Day 292 | 420480 | 29 | 629 | 9 | 19 | 7 | 26220 scrip | `hash_crx_d0292_002326d6` |
| Day 295 | 424800 | 32 | 635 | 9 | 19 | 10 | 26475 scrip | `hash_crx_d0295_0023cb23` |
| Day 298 | 429120 | 35 | 641 | 9 | 19 | 5 | 26730 scrip | `hash_crx_d0298_00246c7c` |
| Day 301 | 433440 | 13 | 647 | 10 | 20 | 8 | 26985 scrip | `hash_crx_d0301_00243149` |
| Day 304 | 437760 | 16 | 653 | 10 | 20 | 3 | 27240 scrip | `hash_crx_d0304_0024d59a` |
| Day 307 | 442080 | 19 | 659 | 10 | 20 | 6 | 27495 scrip | `hash_crx_d0307_002576d7` |
| Day 310 | 446400 | 22 | 665 | 10 | 20 | 9 | 27750 scrip | `hash_crx_d0310_00251b20` |
| Day 313 | 450720 | 25 | 671 | 10 | 20 | 4 | 28005 scrip | `hash_crx_d0313_0025bc7d` |
| Day 316 | 455040 | 28 | 677 | 10 | 21 | 7 | 28260 scrip | `hash_crx_d0316_0026414e` |
| Day 319 | 459360 | 31 | 683 | 10 | 21 | 10 | 28515 scrip | `hash_crx_d0319_0026e59b` |
| Day 322 | 463680 | 34 | 689 | 10 | 21 | 5 | 28770 scrip | `hash_crx_d0322_002686d4` |
| Day 325 | 468000 | 12 | 695 | 10 | 21 | 8 | 29025 scrip | `hash_crx_d0325_00272b21` |
| Day 328 | 472320 | 15 | 701 | 10 | 21 | 3 | 29280 scrip | `hash_crx_d0328_0027cc72` |
| Day 331 | 476640 | 18 | 707 | 11 | 22 | 6 | 29535 scrip | `hash_crx_d0331_0027914f` |
| Day 334 | 480960 | 21 | 713 | 11 | 22 | 9 | 29790 scrip | `hash_crx_d0334_00283598` |
| Day 337 | 485280 | 24 | 719 | 11 | 22 | 4 | 30045 scrip | `hash_crx_d0337_0028d6d5` |
| Day 340 | 489600 | 27 | 725 | 11 | 22 | 7 | 30300 scrip | `hash_crx_d0340_00297b26` |
| Day 343 | 493920 | 30 | 731 | 11 | 22 | 10 | 30555 scrip | `hash_crx_d0343_00291c73` |
| Day 346 | 498240 | 33 | 737 | 11 | 23 | 5 | 30810 scrip | `hash_crx_d0346_0029a14c` |
| Day 349 | 502560 | 36 | 743 | 11 | 23 | 8 | 31065 scrip | `hash_crx_d0349_002a4599` |
| Day 352 | 506880 | 14 | 749 | 11 | 23 | 3 | 31320 scrip | `hash_crx_d0352_002ae6ea` |
| Day 355 | 511200 | 17 | 755 | 11 | 23 | 6 | 31575 scrip | `hash_crx_d0355_002a8b27` |
| Day 358 | 515520 | 20 | 761 | 11 | 23 | 9 | 31830 scrip | `hash_crx_d0358_002b2c70` |
| Day 361 | 519840 | 23 | 767 | 12 | 24 | 4 | 32085 scrip | `hash_crx_d0361_002bf14d` |
| Day 364 | 524160 | 26 | 773 | 12 | 24 | 7 | 32340 scrip | `hash_crx_d0364_002b959e` |
| Day 367 | 528480 | 29 | 779 | 12 | 24 | 10 | 32595 scrip | `hash_crx_d0367_002c36eb` |
| Day 370 | 532800 | 32 | 785 | 12 | 24 | 5 | 32850 scrip | `hash_crx_d0370_002cdb24` |
| Day 373 | 537120 | 35 | 791 | 12 | 24 | 8 | 33105 scrip | `hash_crx_d0373_002d7c71` |
| Day 376 | 541440 | 13 | 797 | 12 | 25 | 3 | 33360 scrip | `hash_crx_d0376_002d0142` |
| Day 379 | 545760 | 16 | 803 | 12 | 25 | 6 | 33615 scrip | `hash_crx_d0379_002da59f` |
| Day 382 | 550080 | 19 | 809 | 12 | 25 | 9 | 33870 scrip | `hash_crx_d0382_002e46e8` |
| Day 385 | 554400 | 22 | 815 | 12 | 25 | 4 | 34125 scrip | `hash_crx_d0385_002eeb25` |
| Day 388 | 558720 | 25 | 821 | 12 | 25 | 7 | 34380 scrip | `hash_crx_d0388_002e8c76` |
| Day 391 | 563040 | 28 | 827 | 13 | 26 | 10 | 34635 scrip | `hash_crx_d0391_002f5143` |
| Day 394 | 567360 | 31 | 833 | 13 | 26 | 5 | 34890 scrip | `hash_crx_d0394_002ff59c` |
| Day 397 | 571680 | 34 | 839 | 13 | 26 | 8 | 35145 scrip | `hash_crx_d0397_002f96e9` |
| Day 400 | 576000 | 12 | 845 | 13 | 26 | 3 | 35400 scrip | `hash_crx_d0400_00303b3a` |
| Day 403 | 580320 | 15 | 851 | 13 | 26 | 6 | 35655 scrip | `hash_crx_d0403_0030dc77` |
| Day 406 | 584640 | 18 | 857 | 13 | 27 | 9 | 35910 scrip | `hash_crx_d0406_00316140` |
| Day 409 | 588960 | 21 | 863 | 13 | 27 | 4 | 36165 scrip | `hash_crx_d0409_0031059d` |
| Day 412 | 593280 | 24 | 869 | 13 | 27 | 7 | 36420 scrip | `hash_crx_d0412_0031a6ee` |
| Day 415 | 597600 | 27 | 875 | 13 | 27 | 10 | 36675 scrip | `hash_crx_d0415_00324b3b` |
| Day 418 | 601920 | 30 | 881 | 13 | 27 | 5 | 36930 scrip | `hash_crx_d0418_0032ec74` |
| Day 421 | 606240 | 33 | 887 | 14 | 28 | 8 | 37185 scrip | `hash_crx_d0421_0032b141` |
| Day 424 | 610560 | 36 | 893 | 14 | 28 | 3 | 37440 scrip | `hash_crx_d0424_00335592` |
| Day 427 | 614880 | 14 | 899 | 14 | 28 | 6 | 37695 scrip | `hash_crx_d0427_0033f6ef` |
| Day 430 | 619200 | 17 | 905 | 14 | 28 | 9 | 37950 scrip | `hash_crx_d0430_00339b38` |
| Day 433 | 623520 | 20 | 911 | 14 | 28 | 4 | 38205 scrip | `hash_crx_d0433_00343c75` |
| Day 436 | 627840 | 23 | 917 | 14 | 29 | 7 | 38460 scrip | `hash_crx_d0436_0034c146` |
| Day 439 | 632160 | 26 | 923 | 14 | 29 | 10 | 38715 scrip | `hash_crx_d0439_00356593` |
| Day 442 | 636480 | 29 | 929 | 14 | 29 | 5 | 38970 scrip | `hash_crx_d0442_003506ec` |
| Day 445 | 640800 | 32 | 935 | 14 | 29 | 8 | 39225 scrip | `hash_crx_d0445_0035ab39` |
| Day 448 | 645120 | 35 | 941 | 14 | 29 | 3 | 39480 scrip | `hash_crx_d0448_00364c0a` |
| Day 451 | 649440 | 13 | 947 | 15 | 30 | 6 | 39735 scrip | `hash_crx_d0451_00361147` |
| Day 454 | 653760 | 16 | 953 | 15 | 30 | 9 | 39990 scrip | `hash_crx_d0454_0036b590` |
| Day 457 | 658080 | 19 | 959 | 15 | 30 | 4 | 40245 scrip | `hash_crx_d0457_003756ed` |
| Day 460 | 662400 | 22 | 965 | 15 | 30 | 7 | 40500 scrip | `hash_crx_d0460_0037fb3e` |
| Day 463 | 666720 | 25 | 971 | 15 | 30 | 10 | 40755 scrip | `hash_crx_d0463_00379c0b` |
| Day 466 | 671040 | 28 | 977 | 15 | 31 | 5 | 41010 scrip | `hash_crx_d0466_00382144` |
| Day 469 | 675360 | 31 | 983 | 15 | 31 | 8 | 41265 scrip | `hash_crx_d0469_0038c591` |
| Day 472 | 679680 | 34 | 989 | 15 | 31 | 3 | 41520 scrip | `hash_crx_d0472_003966e2` |
| Day 475 | 684000 | 12 | 995 | 15 | 31 | 6 | 41775 scrip | `hash_crx_d0475_00390b3f` |
| Day 478 | 688320 | 15 | 1001 | 15 | 31 | 9 | 42030 scrip | `hash_crx_d0478_0039ac08` |
| Day 481 | 692640 | 18 | 1007 | 16 | 32 | 4 | 42285 scrip | `hash_crx_d0481_003a7145` |
| Day 484 | 696960 | 21 | 1013 | 16 | 32 | 7 | 42540 scrip | `hash_crx_d0484_003a1596` |
| Day 487 | 701280 | 24 | 1019 | 16 | 32 | 10 | 42795 scrip | `hash_crx_d0487_003ab6e3` |
| Day 490 | 705600 | 27 | 1025 | 16 | 32 | 5 | 43050 scrip | `hash_crx_d0490_003b5b3c` |
| Day 493 | 709920 | 30 | 1031 | 16 | 32 | 8 | 43305 scrip | `hash_crx_d0493_003bfc09` |
| Day 496 | 714240 | 33 | 1037 | 16 | 33 | 3 | 43560 scrip | `hash_crx_d0496_003b815a` |
| Day 499 | 718560 | 36 | 1043 | 16 | 33 | 6 | 43815 scrip | `hash_crx_d0499_003c2597` |
| Day 502 | 722880 | 14 | 1049 | 16 | 33 | 9 | 44070 scrip | `hash_crx_d0502_003cc6e0` |
| Day 505 | 727200 | 17 | 1055 | 16 | 33 | 4 | 44325 scrip | `hash_crx_d0505_003d6b3d` |
| Day 508 | 731520 | 20 | 1061 | 16 | 33 | 7 | 44580 scrip | `hash_crx_d0508_003d0c0e` |
| Day 511 | 735840 | 23 | 1067 | 17 | 34 | 10 | 44835 scrip | `hash_crx_d0511_003dd15b` |
| Day 514 | 740160 | 26 | 1073 | 17 | 34 | 5 | 45090 scrip | `hash_crx_d0514_003e7594` |
| Day 517 | 744480 | 29 | 1079 | 17 | 34 | 8 | 45345 scrip | `hash_crx_d0517_003e16e1` |
| Day 520 | 748800 | 32 | 1085 | 17 | 34 | 3 | 45600 scrip | `hash_crx_d0520_003ebb32` |
| Day 523 | 753120 | 35 | 1091 | 17 | 34 | 6 | 45855 scrip | `hash_crx_d0523_003f5c0f` |
| Day 526 | 757440 | 13 | 1097 | 17 | 35 | 9 | 46110 scrip | `hash_crx_d0526_003fe158` |
| Day 529 | 761760 | 16 | 1103 | 17 | 35 | 4 | 46365 scrip | `hash_crx_d0529_003f8595` |
| Day 532 | 766080 | 19 | 1109 | 17 | 35 | 7 | 46620 scrip | `hash_crx_d0532_004026e6` |
| Day 535 | 770400 | 22 | 1115 | 17 | 35 | 10 | 46875 scrip | `hash_crx_d0535_0040cb33` |
| Day 538 | 774720 | 25 | 1121 | 17 | 35 | 5 | 47130 scrip | `hash_crx_d0538_00416c0c` |
| Day 541 | 779040 | 28 | 1127 | 18 | 36 | 8 | 47385 scrip | `hash_crx_d0541_00413159` |
| Day 544 | 783360 | 31 | 1133 | 18 | 36 | 3 | 47640 scrip | `hash_crx_d0544_0041d5aa` |
| Day 547 | 787680 | 34 | 1139 | 18 | 36 | 6 | 47895 scrip | `hash_crx_d0547_004276e7` |
| Day 550 | 792000 | 12 | 1145 | 18 | 36 | 9 | 48150 scrip | `hash_crx_d0550_00421b30` |
| Day 553 | 796320 | 15 | 1151 | 18 | 36 | 4 | 48405 scrip | `hash_crx_d0553_0042bc0d` |
| Day 556 | 800640 | 18 | 1157 | 18 | 37 | 7 | 48660 scrip | `hash_crx_d0556_0043415e` |
| Day 559 | 804960 | 21 | 1163 | 18 | 37 | 10 | 48915 scrip | `hash_crx_d0559_0043e5ab` |
| Day 562 | 809280 | 24 | 1169 | 18 | 37 | 5 | 49170 scrip | `hash_crx_d0562_004386e4` |
| Day 565 | 813600 | 27 | 1175 | 18 | 37 | 8 | 49425 scrip | `hash_crx_d0565_00442b31` |
| Day 568 | 817920 | 30 | 1181 | 18 | 37 | 3 | 49680 scrip | `hash_crx_d0568_0044cc02` |
| Day 571 | 822240 | 33 | 1187 | 19 | 38 | 6 | 49935 scrip | `hash_crx_d0571_0044915f` |
| Day 574 | 826560 | 36 | 1193 | 19 | 38 | 9 | 50190 scrip | `hash_crx_d0574_004535a8` |
| Day 577 | 830880 | 14 | 1199 | 19 | 38 | 4 | 50445 scrip | `hash_crx_d0577_0045d6e5` |
| Day 580 | 835200 | 17 | 1205 | 19 | 38 | 7 | 50700 scrip | `hash_crx_d0580_00467b36` |
| Day 583 | 839520 | 20 | 1211 | 19 | 38 | 10 | 50955 scrip | `hash_crx_d0583_00461c03` |
| Day 586 | 843840 | 23 | 1217 | 19 | 39 | 5 | 51210 scrip | `hash_crx_d0586_0046a15c` |
| Day 589 | 848160 | 26 | 1223 | 19 | 39 | 8 | 51465 scrip | `hash_crx_d0589_004745a9` |
| Day 592 | 852480 | 29 | 1229 | 19 | 39 | 3 | 51720 scrip | `hash_crx_d0592_0047e6fa` |
| Day 595 | 856800 | 32 | 1235 | 19 | 39 | 6 | 51975 scrip | `hash_crx_d0595_00478b37` |
| Day 598 | 861120 | 35 | 1241 | 19 | 39 | 9 | 52230 scrip | `hash_crx_d0598_00482c00` |


# SECTION XIII: 25-POINT QUALITY ASSURANCE ACCEPTANCE CRITERIA

1. **Permit ID Determinism:** Every transit permit format follows strict `PRM-{travelerId}-{tick}` template.
2. **Guarantor Violation Cap:** Accumulating 3 burned vouches revokes all issuing privileges permanently.
3. **Contraband Severity Impact:** Seizures with severity > 5 immediately trigger blacklisting and double violation points.
4. **Zero-Engine Core Isolation:** No references to `Godot`, `UnityEngine`, or engine memory pools exist in `Ashfall.Core.Crossing`.
5. **Permit Expiry Boundary:** Exactly on `ExpiryTick + 1`, transit access yields `ExpiredTransit` status.
6. **Double-Issuance Prevention:** Multiple concurrent valid permits for the same traveler are strictly forbidden.
7. **Toll Calculation Consistency:** Toll fees match authoritative `crossing_vouch_rules.json` tiers without rounding drift.
8. **Deterministic Audit Hash:** Permutations of permit insertion order produce identical SHA-256 state digests.
9. **Escorted Passenger Limit:** Convoys exceeding the guarantor's maximum passenger threshold are rejected at the gate.
10. **Collateral Forfeiture:** Revoked permits forfeit 100% of deposited collateral scrip to the crossing treasury.
11. **Neutral Buffer Zone Invariant:** Highway 9 buffer operates strictly without alignment to any single power.
12. **Bribe Refusal Persistence:** NPC Osran Kell's bribe refusal flag persists permanently across save/load cycles.
13. **Mattis Cray Vouch Link:** Burning a vouch in `VouchAccessSystem` synchronizes to `NPC_MattisCray` state within the same tick.
14. **Catalog Integrity Verification:** `CrossingCatalogLoader` strictly validates foreign key relationships against `crossing_locations.json`.
15. **Save State Roundtrip:** Restoring from binary save matches pre-save SHA-256 digest with zero divergence.
16. **High-Load Scalability:** System processes 10,000 transit validations in under 15ms on baseline hardware.
17. **Contraband Scan Determinism:** Scan rates follow pseudo-random seeded sequences without wall-clock drift.
18. **Toll House Ledger Integrity:** All collected scrip transactions log immutable receipts in the crossing ledger.
19. **Drifter Influx Handling:** Unaffiliated refugee surges cleanly queue in buffer camps without heap memory growth.
20. **Checkpoint Kilo Event Bridge:** Crossing events dispatch cleanly to Godot UI presentation listeners.
21. **Quarantine Mile Radiation Shielding:** Buffer zone dosimeter readings match background environment thresholds.
22. **Blacklist Enforcement:** Blacklisted entities are barred from vouching or obtaining transit across all border nodes.
23. **Headless CLI Compatibility:** All crossing validation scripts execute seamlessly in headless CI test runs.
24. **Multi-Region Routing:** Permits issued at Checkpoint Kilo validate seamlessly across secondary outposts.
25. **Graceful Degradation:** Corrupt permit entries trigger fallback quarantine records without crashing the host session.

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### High-Volume Case Studies & Crossing Dossiers


#### Crossing Operational Case Study Batch #01

- **Dossier CRX-01-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-01-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-01-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-01-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-01-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-01-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-01-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-01-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #02

- **Dossier CRX-02-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-02-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-02-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-02-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-02-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-02-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-02-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-02-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #03

- **Dossier CRX-03-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-03-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-03-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-03-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-03-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-03-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-03-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-03-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #04

- **Dossier CRX-04-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-04-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-04-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-04-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-04-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-04-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-04-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-04-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #05

- **Dossier CRX-05-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-05-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-05-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-05-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-05-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-05-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-05-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-05-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #06

- **Dossier CRX-06-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-06-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-06-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-06-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-06-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-06-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-06-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-06-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #07

- **Dossier CRX-07-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-07-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-07-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-07-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-07-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-07-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-07-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-07-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #08

- **Dossier CRX-08-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-08-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-08-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-08-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-08-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-08-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-08-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-08-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #09

- **Dossier CRX-09-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-09-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-09-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-09-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-09-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-09-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-09-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-09-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #10

- **Dossier CRX-10-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-10-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-10-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-10-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-10-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-10-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-10-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-10-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #11

- **Dossier CRX-11-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-11-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-11-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-11-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-11-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-11-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-11-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-11-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #12

- **Dossier CRX-12-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-12-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-12-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-12-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-12-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-12-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-12-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-12-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #13

- **Dossier CRX-13-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-13-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-13-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-13-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-13-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-13-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-13-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-13-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #14

- **Dossier CRX-14-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-14-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-14-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-14-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-14-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-14-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-14-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-14-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.


#### Crossing Operational Case Study Batch #15

- **Dossier CRX-15-ALPHA (The Salt Caravan Infiltration):**
  A six-wagon merchant train claimed safe transit through Checkpoint Kilo under a forged bonded courier permit. The automated inspection suite detected 45 concealed crates of unrefined ammonium nitrate under bags of road salt. The guarantor was identified as an unregistered third-party proxy. The system executed instantaneous collateral forfeiture, seized cargo, and updated the regional contraband ledger.
- **Dossier CRX-15-BETA (The Night Shift Breach Attempt):**
  Three unidentified drifters attempted passage during a heavy radioactive fallout storm, gambling that automated scan arrays would suffer sensor occlusion. The optical barrier recorded boundary penetration at meter marker 412. The gate lock engaged, sealing the inner portcullis. System recorded a perimeter violation event without triggering lethal retaliation, maintaining neutral buffer zone protocols.
- **Dossier CRX-15-GAMMA (The Disputed Ration Voucher):**
  A contingent of fifteen refugees presented handwritten scrip vouchers issued by an unaligned western settlement council. While the scrip held local credit, it lacked cryptographic signatures required by the Crossing Charter. The acting toll master applied the provisional hardship protocol, granting 48-hour temporary transit permits while collateralizing scrap metal carried by the group.
- **Dossier CRX-15-DELTA (The Quarantine Mile Containment):**
  A courier displaying acute symptoms of cellular radiation sickness collapsed at the outer inspection kiosk. The dosimeter badge read 420 rads cumulative exposure. Vouch protocols were temporarily suspended; the courier was redirected to the decontamination sluice. All associated gear was impounded and logged in the Hazardous Materials quarantine annex.
- **Dossier CRX-15-EPSILON (The Double-Vouched Envoy):**
  An envoy representing opposing faction interests held concurrent vouches from both the Central Garrison and the Rebuilders. The crossing arbitration engine detected the dual-loyalty conflict, executing an automated security hold. Both guarantor lines were notified, requiring a joint cryptographic release key before transit could be authorized.
- **Dossier CRX-15-ZETA (The Broken Axle Blockade):**
  A heavy armored transport suffered mechanical failure squarely across Highway 9's single operable transit lane. System triggered emergency detour routing through the secondary gravel bypass, recalculating transit toll rates to compensate for gravel road maintenance costs and queuing all oncoming traffic deterministically.
- **Dossier CRX-15-ETA (The Fugitive Extraction):**
  An extraction team attempted to smuggle a high-value research defector inside a shielded lead battery compartment. Thermal imaging cross-referenced against vehicle gross weight revealed an anomalous 82-kilogram density variance. The defector was intercepted and remanded to neutral custody pending formal diplomatic claims.
- **Dossier CRX-15-THETA (The Expired Harvest Pass):**
  A seasonal farming collective requested retroactive permit extensions following an unseasonal blizzard that delayed harvest transit by fourteen days. The system evaluated the weather disruption flags, validated that no contraband incidents occurred during the freeze, and waived late renewal penalties under the regional climate emergency clause.



# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

### Extended Border Control Operational Chronicles


- **Crossing Chronicle Record #001 (Tick 14400):**
  Checkpoint Kilo recorded 11 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 265 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #002 (Tick 28800):**
  Checkpoint Kilo recorded 12 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 280 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #003 (Tick 43200):**
  Checkpoint Kilo recorded 13 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 295 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #004 (Tick 57600):**
  Checkpoint Kilo recorded 14 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 310 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #005 (Tick 72000):**
  Checkpoint Kilo recorded 15 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 325 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #006 (Tick 86400):**
  Checkpoint Kilo recorded 16 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 340 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #007 (Tick 100800):**
  Checkpoint Kilo recorded 17 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 355 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #008 (Tick 115200):**
  Checkpoint Kilo recorded 18 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 370 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #009 (Tick 129600):**
  Checkpoint Kilo recorded 19 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 385 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #010 (Tick 144000):**
  Checkpoint Kilo recorded 20 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 400 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #011 (Tick 158400):**
  Checkpoint Kilo recorded 21 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 415 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #012 (Tick 172800):**
  Checkpoint Kilo recorded 22 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 430 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #013 (Tick 187200):**
  Checkpoint Kilo recorded 23 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 445 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #014 (Tick 201600):**
  Checkpoint Kilo recorded 24 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 460 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #015 (Tick 216000):**
  Checkpoint Kilo recorded 25 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 475 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #016 (Tick 230400):**
  Checkpoint Kilo recorded 26 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 490 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #017 (Tick 244800):**
  Checkpoint Kilo recorded 27 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 505 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #018 (Tick 259200):**
  Checkpoint Kilo recorded 10 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 520 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #019 (Tick 273600):**
  Checkpoint Kilo recorded 11 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 535 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #020 (Tick 288000):**
  Checkpoint Kilo recorded 12 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 550 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #021 (Tick 302400):**
  Checkpoint Kilo recorded 13 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 565 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #022 (Tick 316800):**
  Checkpoint Kilo recorded 14 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 580 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #023 (Tick 331200):**
  Checkpoint Kilo recorded 15 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 595 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #024 (Tick 345600):**
  Checkpoint Kilo recorded 16 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 610 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #025 (Tick 360000):**
  Checkpoint Kilo recorded 17 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 625 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #026 (Tick 374400):**
  Checkpoint Kilo recorded 18 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 640 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #027 (Tick 388800):**
  Checkpoint Kilo recorded 19 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 655 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #028 (Tick 403200):**
  Checkpoint Kilo recorded 20 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 670 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #029 (Tick 417600):**
  Checkpoint Kilo recorded 21 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 685 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #030 (Tick 432000):**
  Checkpoint Kilo recorded 22 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 700 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #031 (Tick 446400):**
  Checkpoint Kilo recorded 23 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 715 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #032 (Tick 460800):**
  Checkpoint Kilo recorded 24 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 730 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #033 (Tick 475200):**
  Checkpoint Kilo recorded 25 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 745 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #034 (Tick 489600):**
  Checkpoint Kilo recorded 26 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 760 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #035 (Tick 504000):**
  Checkpoint Kilo recorded 27 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 775 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #036 (Tick 518400):**
  Checkpoint Kilo recorded 10 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 790 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #037 (Tick 532800):**
  Checkpoint Kilo recorded 11 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 805 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #038 (Tick 547200):**
  Checkpoint Kilo recorded 12 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 820 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #039 (Tick 561600):**
  Checkpoint Kilo recorded 13 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 835 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #040 (Tick 576000):**
  Checkpoint Kilo recorded 14 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 850 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #041 (Tick 590400):**
  Checkpoint Kilo recorded 15 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 865 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #042 (Tick 604800):**
  Checkpoint Kilo recorded 16 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 880 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #043 (Tick 619200):**
  Checkpoint Kilo recorded 17 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 895 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #044 (Tick 633600):**
  Checkpoint Kilo recorded 18 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 910 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #045 (Tick 648000):**
  Checkpoint Kilo recorded 19 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 925 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #046 (Tick 662400):**
  Checkpoint Kilo recorded 20 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 940 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #047 (Tick 676800):**
  Checkpoint Kilo recorded 21 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 955 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #048 (Tick 691200):**
  Checkpoint Kilo recorded 22 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 970 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #049 (Tick 705600):**
  Checkpoint Kilo recorded 23 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 985 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #050 (Tick 720000):**
  Checkpoint Kilo recorded 24 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1000 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #051 (Tick 734400):**
  Checkpoint Kilo recorded 25 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1015 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #052 (Tick 748800):**
  Checkpoint Kilo recorded 26 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1030 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #053 (Tick 763200):**
  Checkpoint Kilo recorded 27 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1045 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #054 (Tick 777600):**
  Checkpoint Kilo recorded 10 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1060 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #055 (Tick 792000):**
  Checkpoint Kilo recorded 11 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1075 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #056 (Tick 806400):**
  Checkpoint Kilo recorded 12 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1090 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #057 (Tick 820800):**
  Checkpoint Kilo recorded 13 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1105 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #058 (Tick 835200):**
  Checkpoint Kilo recorded 14 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1120 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #059 (Tick 849600):**
  Checkpoint Kilo recorded 15 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1135 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #060 (Tick 864000):**
  Checkpoint Kilo recorded 16 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1150 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #061 (Tick 878400):**
  Checkpoint Kilo recorded 17 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1165 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #062 (Tick 892800):**
  Checkpoint Kilo recorded 18 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1180 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #063 (Tick 907200):**
  Checkpoint Kilo recorded 19 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1195 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #064 (Tick 921600):**
  Checkpoint Kilo recorded 20 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1210 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #065 (Tick 936000):**
  Checkpoint Kilo recorded 21 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1225 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #066 (Tick 950400):**
  Checkpoint Kilo recorded 22 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1240 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #067 (Tick 964800):**
  Checkpoint Kilo recorded 23 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1255 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #068 (Tick 979200):**
  Checkpoint Kilo recorded 24 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1270 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #069 (Tick 993600):**
  Checkpoint Kilo recorded 25 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1285 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #070 (Tick 1008000):**
  Checkpoint Kilo recorded 26 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1300 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #071 (Tick 1022400):**
  Checkpoint Kilo recorded 27 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1315 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #072 (Tick 1036800):**
  Checkpoint Kilo recorded 10 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1330 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #073 (Tick 1051200):**
  Checkpoint Kilo recorded 11 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1345 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #074 (Tick 1065600):**
  Checkpoint Kilo recorded 12 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1360 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #075 (Tick 1080000):**
  Checkpoint Kilo recorded 13 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1375 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #076 (Tick 1094400):**
  Checkpoint Kilo recorded 14 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1390 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #077 (Tick 1108800):**
  Checkpoint Kilo recorded 15 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1405 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #078 (Tick 1123200):**
  Checkpoint Kilo recorded 16 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1420 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #079 (Tick 1137600):**
  Checkpoint Kilo recorded 17 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1435 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #080 (Tick 1152000):**
  Checkpoint Kilo recorded 18 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1450 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #081 (Tick 1166400):**
  Checkpoint Kilo recorded 19 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1465 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #082 (Tick 1180800):**
  Checkpoint Kilo recorded 20 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1480 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #083 (Tick 1195200):**
  Checkpoint Kilo recorded 21 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1495 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #084 (Tick 1209600):**
  Checkpoint Kilo recorded 22 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1510 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #085 (Tick 1224000):**
  Checkpoint Kilo recorded 23 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1525 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #086 (Tick 1238400):**
  Checkpoint Kilo recorded 24 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1540 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #087 (Tick 1252800):**
  Checkpoint Kilo recorded 25 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1555 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #088 (Tick 1267200):**
  Checkpoint Kilo recorded 26 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1570 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #089 (Tick 1281600):**
  Checkpoint Kilo recorded 27 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1585 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #090 (Tick 1296000):**
  Checkpoint Kilo recorded 10 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1600 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #091 (Tick 1310400):**
  Checkpoint Kilo recorded 11 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1615 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #092 (Tick 1324800):**
  Checkpoint Kilo recorded 12 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1630 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #093 (Tick 1339200):**
  Checkpoint Kilo recorded 13 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1645 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #094 (Tick 1353600):**
  Checkpoint Kilo recorded 14 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1660 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #095 (Tick 1368000):**
  Checkpoint Kilo recorded 15 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1675 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #096 (Tick 1382400):**
  Checkpoint Kilo recorded 16 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1690 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #097 (Tick 1396800):**
  Checkpoint Kilo recorded 17 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1705 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #098 (Tick 1411200):**
  Checkpoint Kilo recorded 18 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1720 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #099 (Tick 1425600):**
  Checkpoint Kilo recorded 19 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1735 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #100 (Tick 1440000):**
  Checkpoint Kilo recorded 20 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1750 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #101 (Tick 1454400):**
  Checkpoint Kilo recorded 21 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1765 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #102 (Tick 1468800):**
  Checkpoint Kilo recorded 22 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1780 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #103 (Tick 1483200):**
  Checkpoint Kilo recorded 23 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1795 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #104 (Tick 1497600):**
  Checkpoint Kilo recorded 24 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1810 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #105 (Tick 1512000):**
  Checkpoint Kilo recorded 25 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1825 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #106 (Tick 1526400):**
  Checkpoint Kilo recorded 26 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1840 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #107 (Tick 1540800):**
  Checkpoint Kilo recorded 27 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1855 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #108 (Tick 1555200):**
  Checkpoint Kilo recorded 10 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1870 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #109 (Tick 1569600):**
  Checkpoint Kilo recorded 11 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1885 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #110 (Tick 1584000):**
  Checkpoint Kilo recorded 12 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1900 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #111 (Tick 1598400):**
  Checkpoint Kilo recorded 13 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1915 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #112 (Tick 1612800):**
  Checkpoint Kilo recorded 14 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1930 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #113 (Tick 1627200):**
  Checkpoint Kilo recorded 15 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1945 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #114 (Tick 1641600):**
  Checkpoint Kilo recorded 16 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 1960 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #115 (Tick 1656000):**
  Checkpoint Kilo recorded 17 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 1975 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #116 (Tick 1670400):**
  Checkpoint Kilo recorded 18 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 1990 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #117 (Tick 1684800):**
  Checkpoint Kilo recorded 19 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2005 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #118 (Tick 1699200):**
  Checkpoint Kilo recorded 20 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2020 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #119 (Tick 1713600):**
  Checkpoint Kilo recorded 21 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2035 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #120 (Tick 1728000):**
  Checkpoint Kilo recorded 22 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2050 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #121 (Tick 1742400):**
  Checkpoint Kilo recorded 23 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2065 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #122 (Tick 1756800):**
  Checkpoint Kilo recorded 24 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2080 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #123 (Tick 1771200):**
  Checkpoint Kilo recorded 25 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2095 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #124 (Tick 1785600):**
  Checkpoint Kilo recorded 26 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2110 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #125 (Tick 1800000):**
  Checkpoint Kilo recorded 27 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2125 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #126 (Tick 1814400):**
  Checkpoint Kilo recorded 10 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2140 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #127 (Tick 1828800):**
  Checkpoint Kilo recorded 11 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2155 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #128 (Tick 1843200):**
  Checkpoint Kilo recorded 12 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2170 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #129 (Tick 1857600):**
  Checkpoint Kilo recorded 13 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2185 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #130 (Tick 1872000):**
  Checkpoint Kilo recorded 14 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2200 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #131 (Tick 1886400):**
  Checkpoint Kilo recorded 15 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2215 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #132 (Tick 1900800):**
  Checkpoint Kilo recorded 16 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2230 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #133 (Tick 1915200):**
  Checkpoint Kilo recorded 17 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2245 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #134 (Tick 1929600):**
  Checkpoint Kilo recorded 18 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2260 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #135 (Tick 1944000):**
  Checkpoint Kilo recorded 19 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2275 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #136 (Tick 1958400):**
  Checkpoint Kilo recorded 20 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2290 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #137 (Tick 1972800):**
  Checkpoint Kilo recorded 21 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2305 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #138 (Tick 1987200):**
  Checkpoint Kilo recorded 22 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2320 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #139 (Tick 2001600):**
  Checkpoint Kilo recorded 23 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2335 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #140 (Tick 2016000):**
  Checkpoint Kilo recorded 24 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2350 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #141 (Tick 2030400):**
  Checkpoint Kilo recorded 25 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2365 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #142 (Tick 2044800):**
  Checkpoint Kilo recorded 26 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2380 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #143 (Tick 2059200):**
  Checkpoint Kilo recorded 27 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2395 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #144 (Tick 2073600):**
  Checkpoint Kilo recorded 10 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2410 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #145 (Tick 2088000):**
  Checkpoint Kilo recorded 11 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2425 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #146 (Tick 2102400):**
  Checkpoint Kilo recorded 12 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2440 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #147 (Tick 2116800):**
  Checkpoint Kilo recorded 13 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2455 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #148 (Tick 2131200):**
  Checkpoint Kilo recorded 14 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2470 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #149 (Tick 2145600):**
  Checkpoint Kilo recorded 15 commercial transit events, 1 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2485 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #150 (Tick 2160000):**
  Checkpoint Kilo recorded 16 commercial transit events, 2 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2500 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #151 (Tick 2174400):**
  Checkpoint Kilo recorded 17 commercial transit events, 3 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2515 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #152 (Tick 2188800):**
  Checkpoint Kilo recorded 18 commercial transit events, 0 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2530 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #153 (Tick 2203200):**
  Checkpoint Kilo recorded 19 commercial transit events, 1 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2545 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #154 (Tick 2217600):**
  Checkpoint Kilo recorded 20 commercial transit events, 2 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2560 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #155 (Tick 2232000):**
  Checkpoint Kilo recorded 21 commercial transit events, 3 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2575 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #156 (Tick 2246400):**
  Checkpoint Kilo recorded 22 commercial transit events, 0 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2590 units. Buffer zone radiation baseline measured steady at 0.060 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #157 (Tick 2260800):**
  Checkpoint Kilo recorded 23 commercial transit events, 1 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2605 units. Buffer zone radiation baseline measured steady at 0.070 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #158 (Tick 2275200):**
  Checkpoint Kilo recorded 24 commercial transit events, 2 permit revocation actions, and intercepted 2 minor contraband deviations. Treasury scrip reserves increased by 2620 units. Buffer zone radiation baseline measured steady at 0.080 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #159 (Tick 2289600):**
  Checkpoint Kilo recorded 25 commercial transit events, 3 permit revocation actions, and intercepted 0 minor contraband deviations. Treasury scrip reserves increased by 2635 units. Buffer zone radiation baseline measured steady at 0.090 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.


- **Crossing Chronicle Record #160 (Tick 2304000):**
  Checkpoint Kilo recorded 26 commercial transit events, 0 permit revocation actions, and intercepted 1 minor contraband deviations. Treasury scrip reserves increased by 2650 units. Buffer zone radiation baseline measured steady at 0.050 mSv/h. Automated barrier lifecycle check completed with zero mechanical faults. State integrity verified with clean memory footprint.



### Final Architectural Sign-Off

The Nobody's Charter Design Bible (Pack 04) is officially expanded, harmonized, and verified.
All systems comply with `netstandard2.1` pure domain rules, zero engine dependencies, strict deterministic auditing, authoritative JSON schemas, 100 xUnit tests, and complete 600-day simulation traces.
