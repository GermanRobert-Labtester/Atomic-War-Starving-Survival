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
