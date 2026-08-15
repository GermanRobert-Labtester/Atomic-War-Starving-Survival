# ASHFALL — Expansion Design Bible

**Title:** ASHFALL: THE STANDING RECORD  
**Internal id:** `expansion_the_standing_record`  
**Files:** this bible + `docs/expansions/expansion_03_the_standing_record_creative_pack.md`  
**Status:** Design bible for review. No game data has been edited. No C#.  
**All new ids below are PROPOSED** unless marked *existing*.  
**Tone lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.  
**Sister packs:** Expansion 1 `expansion_the_holdfast` (District 8). Expansion 2 `expansion_the_duty_roster` (Allocation 12 interior). This pack does **not** reopen the coast or the bunker wings. It is the ground between them.

---

# ANALYSIS PHASE

## 1. What is already deep vs what is still a one-liner

### Strengths the repo actually has

- **Located knowledge is wired.** `LoreDiscoveryIndex` + `world_history.json` `discovery_location_id` / `knowledge_key` means history is found in a place. The List ladder already names Ministry, Transit HQ, bus reversal, 12-B, Memory Vault, Lock Gate Four. The mechanic exists. The *rooms* do not.
- **Holdfast spent the allocated world.** Ice Road, Cluster, plant, 12-C, hatch reversed. Kilometre 19 is the last Sector 4 lamp. District 8 is closed geography for this pack.
- **Duty Roster spent the unlisted home.** Chart, ladle, stool, Overflow 11/13, Quiet House overlay. Stack/Approach/Overflow are closed interiors for this pack.
- **Sector 4 gazetteer is full.** Five sub-regions, four Powers, Currents, ~109 location ids across `locations.json` + `locations_expansion3.json`. Adding fifty more POIs would make the map mush.

### What is still a card with no inside

Live `description` lengths: most gazetteer sites are **12–40 words**. That is an inspect line, not a place.

| Site | Live description | Lore bible already promised | Depth now |
|---|---|---|---|
| `location_ministry_of_truth_bunker` | "Propaganda servers… Protocol Zero" (stale) | Continuity office; formula; registrar stayed | **one-liner + wrong thesis** |
| `location_the_memory_vault` | "server farm… social media" (stale) | Archivists; Schedule fragments; Sole | **one-liner + wrong thesis** |
| `loc_lock_gate_four` | "Still open, exactly as far as it opened." | The Drown is a maintenance failure | **one sentence, no rooms** |
| `loc_alloc_12b` | chalk 14 / gap / 6 | Halvard's kit; fallback designation | **spine without layout** |
| `loc_weighbridge` | truck scale, prices by mass | Edor's first interview; Grain Exchange second paragraph | **card, not a hut** |
| `loc_transit_authority_hq` | grease-pencil convoy slots | Quiet Evacuation routed here | **one room implied** |
| `loc_municipal_archive` | grey brick of paper | Garrison searched for the Schedule (wrong archive) | **no stacks to walk** |
| `loc_the_allotments` | 38 words, strongest Verge card | Frayne; brass; waiting list | **still no plots to stand in** |
| `loc_bridge_seven` | charges under the span | Tollman's codes; D/9 marking grammar | **span without underside** |
| `loc_bus_reversal_loop` | forty-one buses pointing back | unlisted sent home | **circle without a lead bus** |
| `loc_pump_station_nine` | six pumps under water | BilgePumps hook | **no motor to choose** |
| `loc_records_annex` | dusted, heated, a name request | Archivists first contact | **window without rooms** |
| `loc_grange_hall` | lamps, ledger, porch | first council; Lasko vote | **hall without adjacency** |
| `loc_cut_kilometre_19` | *Holdfast proposed* | Ivy's last post; Overlay seam | **not yet a layout** |

**Already deep enough (do not re-author as full layouts):** Holdfast Cut/Salt/Cluster/Shelf; Duty Roster Stack/Approach/Overflow; Dead Hand / drone hive / mortar pit (D/9 owns leftover orders); Tessarat leftovers; Wire-Head camp.

**Stale flavour this pack recasts by standing there, not by patch notes:** Ministry "Protocol Zero"; Vault "social media backup." Same ids. Located knowledge. The word *abandoned* on the desalination plant is Holdfast's recast; this pack does not touch the plant's rooms.

## 2. Top 3 concepts

| # | Concept | Why it might be the pack | Why it isn't, or how it is used |
|---|---|---|---|
| **A** | **THE STANDING RECORD** | History already lives at `discovery_location_id`. A Current is finishing Continuity's unpublished *ground* gazetteer — plates, stencils, lot numbers — so a levy can walk to a painted number. The player writes which gazetteer the save keeps. Plot is a route. | **Proceeding.** |
| **B** | **THE SECOND COAST** | Another estuary, another plant. | Holdfast. Forbidden. |
| **C** | **THE BUNKER CRAWL / DEAD HAND** | Walkable interiors or automated-military setpiece. | Duty Roster owns the hole. D/9 owns leftover orders. Wrong genre (3D/action). Rejected as spine. |

## 3. Critical gaps and assumptions

| Gap | Assumption used in this bible |
|---|---|
| Gazetteer sites have no room graph | Layouts are **authored node cards + adjacency**, ticks like expeditions. Not a walkable renderer. Flag 3D interiors **unrealistic**. |
| Ministry/Vault live text contradicts lore bible | Recast on `exp_standing_record_unlocked`. Ids stay. |
| Who would *re-sign* ground? | Current `faction_the_overlay` in `currents.json`. **Not** a 7th Power. Not The Tally (they count). Not Archivists (they file paper). Not Blank Rows (they refuse to write the living). Overlay writes **on the site**. |
| Ostrowski already sells maps | He sells where *things* are. Overlay sells what a place is *called*. They can share a road and hate each other's paper. |
| Companions / bosses | Named site-keepers. Crises are location mutations, not arenas. |
| Sector 4 map | Closed. No fifth Power. No 7th `faction_lore.json` row. Holdfast/Overflow nodes hook, not clone. |

## 4. Choice and why

**Proceeding with A — THE STANDING RECORD.**

Holdfast is the allocated world. Duty Roster is the unlisted home. This pack is **Sector 4 (and only the Holdfast/Overflow nodes that already exist) as places that remember.** The formula scored people. The Overlay numbers ground. The player's expeditions are how the gazetteer gets a second paragraph — discoverable *at the place*, never from a bunker menu.

Blood & Wine, in this house, is not a new duchy. It is **standing in the room the bible already named**.

---

# SECTION 1 — EXPANSION OVERVIEW

| Field | Value |
|---|---|
| **Title** | ASHFALL: THE STANDING RECORD |
| **id** | `expansion_the_standing_record` |
| **Hook** | The Schedule named households. A second document named ground. Someone is still walking with the plates. |
| **Tagline (UI)** | *History is a site. The gazetteer is whoever last stood there.* |
| **Genre lock** | Same game. 2D survival-**management**. Node graph + location cards. Layouts = room cards + adjacency. **No 3D interiors, no action-RPG dungeon.** |
| **Playtime (new content)** | **12–18 hours** main route on a mid-game save; **20–28 hours** if the player winters site-watches, Holdfast windows, and Duty Roster occupancy into the same gazetteer. |
| **Scale honesty** | Equivalent to Holdfast / Duty Roster: 14 featured sites with layouts (not 40), 10 main location-chain quests, ~18 side, 6 site-keepers, ≤3 systems. Not a walkable overworld. |
| **Progression gate (soft)** | Day **75+**, can field a 2-person expedition of ≥6 hours. Can begin **before** Ice Road. |
| **Progression gate (story)** | Knowledge key `lore_pre_the_formula` **or** `lore_grid_convoy_slots` **or** inspecting Kilometre 19 **or** Ostrowski selling a sheet that has two names for one node. |
| **Progression gate (hard ending)** | Standing Record resolved at the Vault (`quest_record_which_gazetteer`) **and** at least four featured sites mutated. |
| **Does not require** | Holdfast unlocked; Duty Roster chart written. If those packs are live, every main quest reads Appendix A flags. If they are dark, the Overlay still walks Sector 4. |
| **Does not add** | A 7th Codex Power. A 16th unrelated `Victory_*.cs` (optional epilogue slide `victory_the_standing_record` only). New hatch magnitudes. Terraformers, Tessarat, 7G, androids, neuromancers. |

### Thesis (unspoken)

A list of people is not a map. A map is not the ground. Writing a number on a post is an act. Scraping it is also an act. The ice does not care which gazetteer you keep. District 8 does.

### One-paragraph pitch

Continuity's last unpublished job was not the Allocation Schedule. It was the **Standing Record**: a cadastral gazetteer matching every Continuity number to a physical site — lock gates, weighbridges, overflow holes, lamp posts, allotment plots. It was never finished. Five years later a Current of leftover cadastral staff walks Sector 4 with brass plates and stencil paint, finishing the job so a levy, a Garrison quota, or an Office clerk can find a reconstruction-pool site by walking to a painted number. The Archivists keep paper in the Drown. Blank Rows refuse to write the living. The Overlay writes on the ground. The player cannot learn the second paragraph of history from a menu. They have to stand in the room. Completing the chain writes the gazetteer the save will keep.

### Integration strategy

| Layer | How it attaches |
|---|---|
| **Map** | No new region. No fifth Power. Fourteen **featured existing ids** gain layout graphs. Gazetteer remainder gets a one-line overlay when a mutation fires. Kilometre 19 (*Holdfast*) is the seam, not a new Cut. Overflow 11/13 (*Duty Roster*) can be numbered; that is a flag, not a new district. |
| **Travel** | Existing `travelHours`. Ice Road dark changes which Cut-adjacent sites are reachable (*Holdfast*). Levy absence means a Verge site has no caretaker (*Duty Roster*). Layout rooms are indoor ticks at the parent node (`travelHours` 0 from parent). |
| **Economy** | No new currency. Plates are brass. Stencil paint is oil + pigment. Hooks `DynamicEconomySystem`, Rebuilders brass, Holdfast resin/iodine, roster labour for site-watches. |
| **Lore** | New `world_history` under `ashfall` with `discovery_location_id` = featured rooms / parents. Second paragraphs on endings **at the place**. Recast stale Ministry/Vault bodies. |
| **Factions** | One new Current: `faction_the_overlay` in `currents.json`. **Do not** add to `faction_lore.json`. |
| **Holdfast / Roster** | Hook `IceRoadSystem`, `CensusClaimSystem`, `DutyRosterSystem`, `WaystationSystem`, `BrineWaterSystem`. Do **not** rebuild. |
| **Consequences** | New mutations via `WorldStateConsequenceSystem` apply-API (or parallel flags). Do **not** put Overlay in `_hegemony`. |
| **Discovery** | `LoreDiscoveryIndex.EntriesAtLocation` already fires on arrival. Layout inspect can fire additional `knowledge_key`s bound to **room ids** (loader treats room as location id, or parent + suffix). |
| **Save** | `exp_standing_record_unlocked` + `LocationLayoutState` + `LocationMemoryState` + Overlay access. Old saves load; plates appear after the first seam quest. |
| **UI** | Location Detail: room list + adjacency (dark rooms stay dark). Diegetic Record document (not a reputation bar). Codex tab "Standing Record" or fold into existing history. |

### What the player is managing on the ground

The same seven needs. The **weights sit on rooms**.

| Need | How a site bites |
|---|---|
| Hunger | Site-watch labour is a mouth not in the mess. Allotments calories if plots are numbered away from the waiting list. |
| Thirst | Membrane strip (*Holdfast*) changes Allotments water. Pump Nine if condemned vs restored. |
| Fatigue | Room ticks. Dark rooms cost extra hours to open. |
| Warmth | Lock control house vs drowned stair. Vault is heated. Lock leaf is not. |
| Radiation | Drown rooms. 12-B. Vault airlock. Overlay work is often outdoors, UV. |
| Morale | A name on a post that was a lamp. A ledger with cadastral numbers. Marks, not sermons. |
| Health | Fume at Pump Nine. Ice at Km 19. Collapse in Archive stacks. |
| Shelter | Home still ticks. Site-watches steal roster rows (*Duty Roster*). |

---

# SECTION 2 — FEATURED LOCATIONS (14)

**Cap:** 14 full layouts. Do not author 40. Unfeatured gazetteer sites may receive a **one-line overlay** when `LocationMemorySystem` fires (`mutation_record_overlay_line`).

Visual DNA unchanged: dry-gouache, ash-grey, concrete, rust, terminal amber. This pack adds **brass plates with lot numbers, stencil over stencil, grease pencil under glass, rooms that stay dark until an adjacent tick opens them.**

Indoor room ids are **layout nodes**, not `GeneratedMap` travel nodes. Parent `travelHours` / danger / rads stay as live data.

**Spine sites (4)** — standing in them advances the main plot: Ministry, Lock Gate Four, Allocation 12-B, Memory Vault.  
**Route-required (cannot skip):** Kilometre 19, Transit HQ, Weighbridge.  
**The rest** deepen the gazetteer and feed branches (which rooms exist/open).

---

## 2.1 `loc_cut_kilometre_19` — Kilometre 19 *(Holdfast id; seam)*

**Existing or new:** *Holdfast proposed* `loc_cut_kilometre_19`. Do not mint a twin.  
**Region:** Cut / Grid-Verge seam. Last Sector 4 lamp.  
**Visual:** Lamplighter orange post, number stencilled twice, salt-white ice north, ash south. A new brass plate screwed over the stencil.  
**Lore integration:** Ivy's ledger stops here. Overlay's Record *starts* here: Continuity cadastral `CUT-19 / LAMP`. Yara will not cross. Ostrowski's sheet may show both names.  
**Unique mechanic:** Plate vs oil. Taking the plate darkens Overlay labour at Cut-adjacent sites. Taking the oil shorts Ivy's next lighting (*Lamplighter rule*).  
**Map relation:** Hook, not a new Cut. Ice Road dark (*Holdfast*) makes the seam reachable only from Sector 4 south; the plate is still there.

**Layout (4 rooms)**

```
[post] -- [seam ice]
  |
[oil tin]
  |
[plate crate]
```

| Room id | Name | Adjacency | Steal / weigh / refuse |
|---|---|---|---|
| `room_km19_post` | The Post | seam, oil tin | Brass Overlay plate over the stencil |
| `room_km19_seam` | The Seam | post | Child's mitten on the bracket (*Holdfast object — do not duplicate sentence; new: a second mitten, adult, Overlay-issue glove*) |
| `room_km19_oil_tin` | Oil Tin | post | Oil can with rag stopper |
| `room_km19_plate_crate` | Plate Crate | oil tin (unlock after inspect post) | Spare plates stamped `CUT-19` through `CUT-24` |

**Encounter notes:** `enc_site_plate_screwer` (Maren or a junior). `enc_site_ivy_receipt` if oil taken. Aftermath: post description recasts to plate-on, plate-scraped, or both layers visible.

---

## 2.2 `loc_transit_authority_hq` — Transit Authority *(existing)*

**Region:** Grid. d6 · 2.5h · 34 rads.  
**Visual:** Wall-sized route maps under glass. Grease pencil. A trestle with Overlay plates waiting to replace the pencil.  
**Lore:** Quiet Evacuation routed here. Convoy 12: `HELD — DOB QUERY`. Overlay wants printed cadastral times, not a dead clerk's hand.  
**Unique mechanic:** Grease pencil vs overlay print. Copying the pencil is `lore_grid_convoy_slots` (*existing*). Replacing it mutates the map room forever.  
**Map:** Grid hub. Highway 9 cleared (*existing mutation*) shortens the walk.

**Layout (5)**

```
[lobby] -- [map glass] -- [DOB desk]
                |              |
         [overlay bench]  [radio gallery]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_transit_lobby` | Lobby | Number-disc dispenser, empty |
| `room_transit_map_glass` | Map Glass | Grease pencil on a string |
| `room_transit_dob_desk` | DOB Desk | Telephone with no line; DOB blotter |
| `room_transit_overlay_bench` | Overlay Bench | Printed timetable plates |
| `room_transit_radio_gallery` | Radio Gallery | Log of the last order to turn buses around |

**Encounter notes:** Maren's first long scene. Garrison clerk if `Mutation_TransitTax`. Aftermath: maps show lived times, Continuity times, or both (illegible).

---

## 2.3 `loc_municipal_archive` — Municipal Archive *(existing)*

**Region:** Grid.  
**Visual:** Rolling stacks collapsed. Fire-suppression brick below the waist. A cage still dry. Overlay has been filing Continuity plates into municipal drawers that cannot hold them.  
**Lore:** Garrison searched twice for the Schedule. It was never municipal. Overlay does not know that, or does, and files anyway.  
**Unique mechanic:** Wrong stacks. Digging the brick costs hours + collapse check. The Schedule is not here. A **site index** might be — Overlay's field copy.

**Layout (4)**

```
[vestibule] -- [grey brick]
      |              |
[loading dock]  [reading cage]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_archive_vestibule` | Vestibule | Visitor book, last Garrison search dated |
| `room_archive_grey_brick` | Grey Brick | A plate half-sunk in the brick |
| `room_archive_reading_cage` | Reading Cage | Overlay field index (not the Schedule) |
| `room_archive_loading_dock` | Loading Dock | Empty Continuity crate, stencil `RECORD / NOT SCHEDULE` |

**Encounter notes:** Collapse. Overlay junior stuck waist-deep. Aftermath: cage locked, brick dug, or plates left to sink.

---

## 2.4 `location_ministry_of_truth_bunker` — Ministry *(existing; SPINE)*

**Region:** Grid. Live text is stale; recast on unlock.  
**Visual:** Civil-service cream that went grey. Enquiry desk with a bell that still works. Scoring charts. A drawer that once held Sector 4's copy of the Schedule (Ormund has the other; Sole has fragments). The **Standing Record** book: sites, not households. Incomplete.  
**Lore:** Office of Continuity; formula; registrar stayed (`lore_hz_the_registrar_stays`). Ira Vell is that woman, still at the desk, or the junior who inherited the desk when the woman stopped being able to climb the stair. **Do not** invent Protocol Zero.  
**Unique mechanic:** The book. Copying it is not completing it. Completing it requires standing at the sites it lists. Menu inspection of a photocopy does **not** advance the main plot.  
**Map:** Grid. D/9 denial marks in the obstacle annex (`lore_denial_marks`).

**Layout (6)**

```
[stair] -- [enquiry] -- [scoring]
              |            |
        [registrar] -- [obstacle annex]
              |
        [dead phone]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_ministry_stair` | Stair | Authenticator light on a fuse that should have died |
| `room_ministry_enquiry` | Enquiry Desk | Bell; memo *stop answering eligibility* |
| `room_ministry_scoring` | Scoring Floor | Rubric poster; occupation points |
| `room_ministry_registrar` | Registrar | Standing Record (incomplete); Ira |
| `room_ministry_obstacle_annex` | Obstacle Annex | D/9 marking key (paper). Taking it does not teach the four living readers' hands |
| `room_ministry_dead_phone` | Dead Telephone | Blotter: Convoy 12 hold, six hours eleven minutes |

**Encounter notes:** Ira will not raise her voice. Overlay wants the book out of the building. Aftermath: book copied / taken north / left / scraped of site names.

---

## 2.5 `loc_weighbridge` — The Weighbridge *(existing; route-required)*

**Region:** Toll. Edor's first interview (*Holdfast*). Grain Exchange second paragraph (*05_FACTIONS*).  
**Visual:** Mechanical scale. Hut. Receipts by mass. Overlay lot-plates stacked where the calibration weight hangs.  
**Lore:** Favours priced as kilograms. Overlay's plates have no mass. Osric Tann is not the Tollman; he keeps the needle honest.  
**Unique mechanic:** Mass vs lot. A plate on the scale reads as mass. That is a joke the Warlords will repeat until it is policy.

**Layout (4)**

```
[plate] -- [hut] -- [receipts]
              |
        [overlay lots]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_weigh_plate` | The Plate | Calibration weight `500 kg` |
| `room_weigh_hut` | Hut | Spring-balance for iodine tins |
| `room_weigh_receipts` | Receipts | Pink/yellow/white carbons; Edor's unfinished return if levy refused |
| `room_weigh_overlay_lot` | Lot Stack | Plates `TOLL-WB / LOT` |

**Encounter notes:** Osric; Edor if Holdfast clerk started; Tollman does not attend. Aftermath: prices by mass, by lot, or both (columns disagree).

---

## 2.6 `loc_grange_hall` — The Grange Hall *(existing)*

**Region:** Verge.  
**Visual:** Porch weapons sign. Long table. Ledger from week one. Overlay wants cadastral plot numbers in the name column.  
**Lore:** First council. Lasko vote. Delacroix. A show of hands is how the Verge names a place.  
**Unique mechanic:** Vote vs plate. Overlay cannot conquer the hall. They can leave a notice. The ledger is the lived gazetteer.

**Layout (4)**

```
[porch] -- [table] -- [ledger]
              |
          [kitchen]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_grange_porch` | Porch | Sign; a rifle already in the stand |
| `room_grange_table` | Long Table | Oil lamp still warm |
| `room_grange_ledger` | Ledger Desk | First page, 47 names, 12 rifles, 22 crosses |
| `room_grange_kitchen` | Kitchen | Kettle; Overlay notice face-down in a drawer |

**Encounter notes:** Delacroix or a chair. Lasko if vote pending. Aftermath: ledger recast with numbers, names, or a pasted Overlay column nobody uses.

---

## 2.7 `loc_the_allotments` — The Allotments *(existing)*

**Region:** Verge / Rebuilders floodplain.  
**Visual:** Two hundred numbered plots. Caretaker hut. Waiting list in a plastic sleeve. Brass bin. Overlay plates that would make the waiting list "historical."  
**Lore:** Frayne. Brass. Membrane strip (*Holdfast*) shortens water. Levy absence (*Duty Roster*) means Dara Mewn is not on night watch.  
**Unique mechanic:** Plot number vs waiting-list name. Frayne will not ask where brass came from. Overlay brass *is* plates.

**Layout (5)**

```
[gate] -- [hut] -- [noticeboard]
            |           |
      [brass bin]  [waitlist plot]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_allot_gate` | Gate | Chain-link cut and rewired |
| `room_allot_hut` | Caretaker Hut | Minutes; autoclave key on a nail |
| `room_allot_noticeboard` | Noticeboard | Waiting list sleeve |
| `room_allot_plot_waitlist` | A Named Plot | A person farming a number that is not theirs on Overlay paper |
| `room_allot_brass_bin` | Brass Bin | Door handles; a nameplate; an Overlay plate mixed in |

**Encounter notes:** Frayne committee; Dara if home. Aftermath: list current / historical / missing. Water clock if membrane stripped.

---

## 2.8 `loc_bridge_seven` — Bridge Seven *(existing)*

**Region:** Toll.  
**Visual:** Four lanes. Charges taped under the span, visible. Overlay survey flags. D/9 marking grammar on the rail (`lore_denial_marks`).  
**Lore:** Tollman's authority is nobody checking the detonator. Overlay documenting the charges as "friendly obstacles" is how a Record makes a threat into a line item.  
**Unique mechanic:** Look vs check. Looking from the span is allowed. Checking the detonator is the branch that can end the Tollman's joke.

**Layout (4)**

```
[near bank] -- [span] -- [charges]
                  |
           [overlay survey]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_bridge_near` | Near Bank | Toll receipt spike |
| `room_bridge_span` | Span | A stone moved to the marked side of a scupper (D/9) |
| `room_bridge_charges` | Underside | Detonator housing; do not "test" |
| `room_bridge_overlay_survey` | Survey | Clipboard: `FRIENDLY OBSTACLE / TOLL-B7` |

**Encounter notes:** Overlay surveyor. D/9 is not a fight here. Aftermath: charges listed in the Record, scraped, or the detonator disturbed (Tollman policy changes; not a 3D explosion setpiece — a **access mutation**).

---

## 2.9 `loc_bus_reversal_loop` — Bus Reversal Loop *(existing)*

**Region:** Toll.  
**Visual:** Forty-one buses, nose to tail, pointing at the city. Overlay stencil on the lead bus: `EVACUATION COMPLETE`.  
**Lore:** Unlisted sent back. The order was obeyed. Overlay's stencil is a lie that is also a filing.  
**Unique mechanic:** Stencil vs logbook. The lead bus still has a driver's log that contradicts the plate.

**Layout (4)**

```
[circle] -- [lead bus] -- [office]
                |
            [stencil]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_bus_circle` | Turning Circle | A child's suitcase, empty, labelled with a street not a number |
| `room_bus_lead` | Lead Bus | Driver's log; last order |
| `room_bus_office` | Loop Office | Timetable never updated |
| `room_bus_stencil` | Stencil Wall | Overlay paint pot, still wet or long dry |

**Encounter notes:** Overlay painter. Aftermath: stencil complete / scraped / painted over with the street name.

---

## 2.10 `loc_lock_gate_four` — Lock Gate Four *(existing; SPINE)*

**Region:** Drown.  
**Visual:** Gate mid-cycle, frozen at the angle of failure. Control house. Benno Kade lives in it. Overlay plate: `RECLAMATION 4-W CONTROL / COMPLETE`. The Drown is not complete.  
**Lore:** Exchange+3W power loss. Maintenance failure. Overlay cannot make a flood a finished job by signing it. They can make District 8 *treat* it as signed.  
**Unique mechanic:** Weather vs plate. Benno's gauges are the lived Record. Ice Road / Shallows boat changes who can reach the towpath.

**Layout (6)**

```
[towpath] -- [control] -- [gauges]
                |            |
            [Benno]      [leaf]
                |
         [reclaim plate]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_lock_towpath` | Towpath | Mooring ring; Nomi will notice if stolen |
| `room_lock_control` | Control House | Isolation wheel, painted, stuck |
| `room_lock_benno` | Benno's Bunk | A kettle on a spirit stove; a chart of water-rise in pencil |
| `room_lock_gauges` | Gauge Room | Mid-cycle indicator, still true |
| `room_lock_leaf` | Gate Leaf | A wrench frozen to a bolt |
| `room_lock_reclaim_plate` | Reclamation Plate | Overlay plate claiming complete |

**Encounter notes:** Benno. Overlay installation crew. Aftermath: plate up / scraped / gauges copied into the Record as *open, exactly as far as it opened*.

---

## 2.11 `loc_pump_station_nine` — Pump Station Nine *(existing)*

**Region:** Drown.  
**Visual:** Six pumps under three metres. One motor dry by accident of a collapsed floor. Overlay condemned tags.  
**Lore:** Restoring a pump lowers the Drown a measurable amount (*existing bible*). Overlay tagging it condemned keeps the Record tidy and the sublevels shut.  
**Unique mechanic:** Restore vs condemn. Hooks `System_BilgePumps` **if present**; otherwise a location mutation + travelHours delta to Vault/12-B/Annex. Do not invent SubBay as a requirement.

**Layout (5)**

```
[approach] -- [hall] -- [dry motor]
                 |          |
          [condemned]  [switchboard]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_pump_approach` | Boat Approach | Bilge pole |
| `room_pump_hall` | Pump Hall | A name scratched on a pump housing |
| `room_pump_dry_motor` | Dry Motor | Spare belt |
| `room_pump_switchboard` | Switchboard | Fuses; Overlay tag through the handle |
| `room_pump_condemned` | Condemned Cage | Tag bundle `NOT TO BE ENERGISED` |

**Encounter notes:** Rebuilders want the motor. Overlay wants the tag. Aftermath: one pump live / all tagged / hall darker (water up).

---

## 2.12 `loc_alloc_12b` — Allocation 12-B *(existing; SPINE)*

**Region:** Drown / subway maintenance.  
**Visual:** Stencil. No provisioning. Fourteen chalk marks, gap, six. Halvard's improvised potable still working if Duty Roster left it. Overlay wants the stencil refreshed as a finished overflow hole.  
**Lore:** Fallback designation on a form, not a prepared shelter. Sela: engineering, not salvage. Overlay numbering 12-B makes it a pool site for 12-C.  
**Unique mechanic:** Kit vs stencil. Taking the kit kills the water. Refreshing the stencil without the kit is a number on a tomb. Blank Rows will hear if you ink this site (*Duty Roster*).

**Layout (5)**

```
[stair] -- [unprovisioned] -- [kit]
               |                 |
          [stencil]           [water]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_12b_stair` | Stair | Chalk; do not close the gap |
| `room_12b_unprovisioned` | Unprovisioned Hall | Empty bolt-holes where bunks would have been |
| `room_12b_kit` | Halvard's Kit | Notes; working filter jury-rig |
| `room_12b_water` | Water That Works | A cup chained to the pipe |
| `room_12b_stencil` | Stencil | Overlay refresh paint; original faded `12-B` |

**Encounter notes:** Sela if present. Overlay stenciller. Aftermath: kit left/taken; stencil original/refreshed/scraped; chalk gap held.

---

## 2.13 `loc_records_annex` — Records Annex *(existing)*

**Region:** Drown. Boat through a second-storey window.  
**Visual:** Dusted. Heated. Quil Esser. Overlay crate on the landing, unopened, labelled `GROUND COPY`.  
**Lore:** Archivists first contact. Name, and the names of the dead. Overlay wants a ground copy filed *without* saying the site names aloud. Quil will not.  
**Unique mechanic:** Paper vs plate. Filing Overlay plates here is completeness that Sole may accept and Quil may not.

**Layout (4)**

```
[window] -- [dusted] -- [name desk]
                |
         [refused crate]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_annex_window` | Window Entry | Boat-hook scratches |
| `room_annex_dusted` | Dusted Room | Dust-cloth, still damp |
| `room_annex_name_desk` | Name Desk | Witness ledger; two-name rule |
| `room_annex_refused_crate` | Refused Crate | Overlay ground copy, string still tied |

**Encounter notes:** Quil; Sole if Vault already visited. Nomi if etiquette broken. Aftermath: crate accepted / refused / dumped in the Drown.

---

## 2.14 `location_the_memory_vault` — The Memory Vault *(existing; SPINE; end of route)*

**Region:** Drown. Endgame travel. Live text stale; recast.  
**Visual:** Not social media. Dry stacks. Sole's table. A cage for a second copy of history that only exists if someone walked the sites. Overlay plates waiting in the airlock like guests.  
**Lore:** Nine Archivists. Corroboration. Paperwork survives. The Standing Record, completed or refused, is filed **here** or it is not history the Codex will keep.  
**Unique mechanic:** Which gazetteer. The ending writes `world_history` second paragraphs with `discovery_location_id` = this Vault **and** the sites mutated. You cannot finish from the bunker. Pump Nine / Ice Road / Shallows etiquette gate access.

**Layout (6)**

```
[dock] -- [airlock] -- [stacks] -- [Sole's table]
                |                     |
         [second copy]         [standing book]
```

| Room id | Name | Steal / refuse |
|---|---|---|
| `room_vault_dock` | Dock | Mooring; Nomi's etiquette |
| `room_vault_airlock` | Airlock | Overlay plates stacked like hats |
| `room_vault_stacks` | Dry Stacks | A fragment already in lore; do not invent terraformers |
| `room_vault_sole_table` | Sole's Table | Blotter; unsigned 12-C if carried (*Holdfast*) |
| `room_vault_second_copy` | Second Copy Cage | Empty until the route is walked |
| `room_vault_standing_book` | The Book's Place | A space the size of Ira's Record |

**Encounter notes:** Sole. Quil. Maren if Overlay access held. Aftermath: the three (four) endings. Gazetteer recast sector-wide as one-line overlays.

---

## 2.15 Unfeatured overlay (one-line only when mutation fires)

Examples, not full layouts: `loc_conscription_office` (Pell vs occupation plates), `loc_terrace_pumphouse` (lot map on the loophole wall), `loc_ration_queue_plaza` (queue paint numbered), `loc_dentists_row` (chair-return already Duty Roster), `loc_overflow_alloc_11` (Overlay would number it; Nila's hatch dark if they do), `loc_cluster_office` (*Holdfast* — Ormund's drawer gains a Record page if Overlay finishes), `loc_stack_roster_wall` (*Duty Roster* — Kess will not copy site numbers onto people).

---

# SECTION 3 — MAIN STORYLINE

## Central conflict

**Two correct gazetteers.**

Ira Vell's Standing Record lists sites by Continuity number.  
The people who stayed use other names: the Drown, the Weighbridge, Kilometre 19, plot 114, Allocation 12-B.  
Maren Holt is finishing the Record on the ground so a clerk in District 8, or Pell, or a levy escort, can walk to a plate.  
Margit Sole will file a ground copy only if the names are said aloud and corroborated.  
Benno Kade's lock is a weather. A plate that says COMPLETE does not close it.

The player must decide whether Sector 4 is a reconstruction map or a set of rooms that already have names.

The story **is a route.** Skip a spine site and the Vault's second-copy cage stays empty. Photocopying the Ministry book in a menu does not fill it.

## Theme (unspoken)

Naming a place is how you find it, and how someone else finds the people in it. Silence is also a map.

## Principal NPCs (6) — they belong to places

Holdfast / Duty Roster casts remain **theirs**. They appear as integration, not a second starring roster.

### 1. `npc_maren_holt` — Overlay Surveyor Maren Holt *(companion)*

- **Where:** first `room_km19_plate_crate`, then Transit overlay bench, then wherever the Record is unfinished
- **Was:** Municipal cadastral technician. Not allocated. In-situ the way Leva was, except nobody issued a plant order. She stayed on the map.
- **Wants:** The Standing Record finished on the ground. Brass. A walker who will carry plates.
- **Will not:** Screw a plate over a Lamplighter lamp without logging the oil (she will still screw it). Enter Allocation 11 if she knows Nila's rule. Falsify a gauge.
- **Voice:** Distances, lot numbers, "the lived name is a subtitle." Never "please." Never poetry about memory.
- **Snippet:**
  > "The Schedule named households. This names ground. Ground does not argue. People do. I am not here to argue. I am here to finish the post."

### 2. `npc_ira_vell` — Registrar Ira Vell

- **Where:** `room_ministry_registrar`
- **Was:** Filing grade. The woman who stayed, or the junior who did not leave when the senior grades did. Score would not have allocated her. She has the book.
- **Wants:** The Record complete *or* honestly incomplete. She will not pretend a site was visited.
- **Will not:** Give the book to Overlay without a receipt. Raise her voice. Call the player allocated.
- **Voice:** Present tense. "Noted." Pages.
- **Snippet:**
  > "I can copy you the list of sites. That is not the same as you having been there. The book has a column for that. It is empty until the column is not a lie."

### 3. `npc_benno_kade` — Lock-keeper Benno Kade *(companion, Drown)*

- **Where:** `room_lock_benno`
- **Was:** Lock electrician. Unlisted. Has lived in the control house since the gate froze mid-cycle. The Drown's rise is his duty roster.
- **Wants:** The gauges believed. The plate that says COMPLETE taken down. A fuse, sometimes.
- **Will not:** Call the failure a bomb. Guide Overlay to Pump Nine if they have already tagged it condemned.
- **Voice:** Water-heights. Dates. "Open, as far as it opened."
- **Snippet:**
  > "They can screw COMPLETE on the leaf. The leaf is still where it stopped. I sleep in the house that stopped with it."

### 4. `npc_quil_esser` — Annex keeper Quil Esser *(companion)*

- **Where:** `loc_records_annex` / Vault stacks
- **Was:** Born after. Learned filing from watching. Archivist practice, not a sermon.
- **Wants:** Names said aloud. Overlay crate kept string-tied until someone speaks the sites.
- **Will not:** File a plate as a name. Skip corroboration. Adjudicate Quiet House.
- **Voice:** Soft. The two-witness rule. Dusting as punctuation.
- **Snippet:**
  > "If you don't say it, you're only copying. A plate is a copy that does not have a mouth."

### 5. `npc_osric_tann` — Weigh-clerk Osric Tann

- **Where:** `loc_weighbridge`
- **Was:** Municipal scale technician. Toll pays him in food to keep the needle true. He is not the Tollman.
- **Wants:** Mass. Calibration weight on its hook. Overlay lots off the plate.
- **Will not:** Price a favour as a lot number. Lie about the needle for Edor or for Maren.
- **Voice:** Kilograms. Receipts. The joke that stopped being one.
- **Snippet:**
  > "Put the plate on the scale if you want. It will read as mass. That is the only number I will write."

### 6. `npc_dara_mewn` — Plot watch Dara Mewn

- **Where:** `loc_the_allotments` caretaker hut at night
- **Was:** Waiting-list name, alive, farming. Levy-shaped trade (caretaker). If Hadi/levy took the night watch labour, this hut is empty.
- **Wants:** The sleeve current. Brass not taken from the bin if it is fittings. Overlay plates not on the plots she waters.
- **Will not:** Sign a plate that makes the waiting list historical. Comment on nameplates.
- **Voice:** Plot numbers as people. Minutes.
- **Snippet:**
  > "Plot 114 is still 114. The plate they want to put on it has a different 114. I water this one."

**Margit Sole** remains in the Vault. **Ivy, Yara, Edor, Kess, Nila, Frayne, Pell, Ostrowski, Nomi** are integration stages, not recast.

## Story beats (10) — each a place

| # | Beat | Location | What standing there does |
|---|---|---|---|
| 1 | **The plate on the last lamp** | Km 19 | Overlay has begun. Ivy's stencil is under brass. The route cannot start in a menu. |
| 2 | **The pencil under glass** | Transit HQ | Convoy 12's hold is a desk, not a Codex summary. Overlay print waits to replace the hand. |
| 3 | **The wrong stacks** | Municipal Archive | The Schedule is not here. A site index might be. Garrison already looked. |
| 4 | **The book** | Ministry registrar | Ira shows the Standing Record. Incomplete. Column: *visited*. Empty. **Spine.** |
| 5 | **Mass** | Weighbridge | Lot vs kilogram. Edor may be on the stool (*Holdfast*). Osric will not convert. |
| 6 | **Hands** | Grange + Allotments | Lived names. Waiting list. Brass bin. Levy absence empties Dara's hut. |
| 7 | **Friendly obstacle** | Bridge Seven | Overlay lists the charges. Looking is not checking. |
| 8 | **Open, as far as it opened** | Lock Gate Four | COMPLETE plate vs gauges. **Spine.** Shallows/Ice Road gate who is here. |
| 9 | **Fallback** | 12-B (+ Pump Nine branch) | Stencil vs kit. Condemned vs one dry motor. **Spine.** |
| 10 | **Which gazetteer** | Annex → Vault | Second-copy cage. Sole. Maren. Quil. You cannot skip 4/8/9 and fill the cage. |

## Branching choices (5) — rooms exist / open

| id | Choice | Immediate | Rooms / access |
|---|---|---|---|
| `record_plates_stand` | Allow Overlay to finish plates at 3+ spine sites | District 8 / Pell can pathfind by number | `room_vault_airlock` plates admitted; Km 19 post brass-on; 12-B stencil refreshed; Lock COMPLETE stays |
| `record_lived_names` | Write vernacular on posts / scrape plates | Ostrowski's sheets match; Overlay access withdraws | Overlay bench rooms go dark; `room_km19_plate_crate` empty; Grange ledger names only |
| `record_palimpsest` | Leave both layers | Travel times worse; clerks argue | Both room descriptions; Weighbridge columns disagree; Transit maps illegible |
| `record_pump_live` | Energise Pump Nine against Overlay tag | Drown lowers a step; Vault/12-B/Annex easier | `room_pump_condemned` opens as scrap; `room_vault_dock` recast |
| `record_pump_condemned` | Honour the tag | Record tidy; sublevels stay shut | Dry motor room stays dark; Vault travelHours stay 9.0 |

Silent branches: brass bin (Frayne + Overlay plates + tin); Ivy oil vs plate; Nila 11 numbered (access lost); Kess will not copy site numbers onto people; detonator check at Bridge Seven.

## Endings (4) — recast the gazetteer

All write `world_history` second paragraphs discoverable **at the sites**, not only in a menu. The game does not rank them.

| id | Name | Condition | Slide (house voice) |
|---|---|---|---|
| `ending_record_stands` | **The Record Stands** | `record_plates_stand` + Vault files the ground copy | Posts have numbers. Lived names are subtitles in smaller stencil. Edor's next return has sites. The levy can walk. |
| `ending_record_lived` | **The Lived Map** | `record_lived_names` + Sole files spoken names | The plates are in a crate in the Annex, string-tied. Ostrowski's sheet is the one that matches the ground. Overlay is not at Kilometre 19. |
| `ending_record_palimpsest` | **Both Hands** | `record_palimpsest` + book left incomplete-honest | Every featured site shows two names. Clerks lose hours. The cage holds both copies and nobody likes either. |
| `ending_record_scraped` | **Unnumbered** | Plates scraped, nothing written; Blank Rows access held | Sole cannot complete ground. Hatch escort (*Holdfast*) brings a list of places from District 8 that do not match the posts. Benno's gauges are the only honest document in the Drown. |

**TrueEnding terraformer / android / neuromancer content is not used.**

## Lore revelations (what standing there teaches)

1. The Schedule and the Standing Record were sibling jobs. One named people. One named ground. Only the first was famous.
2. Located knowledge (`discovery_location_id`) is the game already telling you this. The expansion spends it as rooms.
3. Overlay, Archivists, Blank Rows, and Kess's pencil are four relationships to writing. They are not four morals.
4. A COMPLETE plate on Lock Gate Four is how a flood becomes a closed file. The water does not file.
5. 12-B as a refreshed stencil is how a fallback hole becomes a labour address for 12-C.
6. You cannot corroborate a site from the bunker. The second copy is empty until the route is walked.

---

# SECTION 4 — QUEST DESIGN

Runtime: `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids` (*existing*). Register `quest_record_*`. Types: `expedition`, `shelter`, `faction`, `personal`, `repeatable`.

**World-change bar:** every main quest names `mutation_id`, a visible returning-player change, and Holdfast **and** Duty Roster read-differences. If a row cannot, it is cut.

**Spatial bar:** objectives are rooms. Completing from the bunker menu is a fail state for mains.

---

## 4.1 Main questline (10)

### `quest_record_the_plate` — The Plate on the Last Lamp

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `loc_cut_kilometre_19` |
| **Rooms** | post, seam, oil tin, plate crate |
| **Prereqs** | Day 75+ **or** Ostrowski two-name sheet **or** Ivy asked about a new plate |
| **Time** | 40–70 min |
| **Synopsis** | The last Sector 4 lamp has a brass plate over the stencil. Maren Holt is finishing a crate of spares. Ivy will not cross. Yara will not come south. The seam is the first room of the pack. |
| **Objectives** | 1. Travel to Km 19 (cannot skip). 2. Inspect post (plate vs stencil). 3. Inspect oil tin (leave / take / split). 4. Open plate crate or refuse. 5. Speak to Maren or to the empty crate. |
| **Rewards** | `item_sr_plate_cut19`; Maren companion unlock; `knowledge_key: lore_sr_seam` |
| **Location mutation** | `mutation_km19_plated` / `_scraped` / `_palimpsest` |
| **Holdfast reads** | Ice Road dark: site still reachable from south. Oil taken: Ivy eleven-day rule. Yara's book shows a dark hour that was not hers if oil went north. |
| **Duty Roster reads** | Site-watch labour if you leave someone at the post. Kess will not write CUT-19 as a person's occupation. |

---

### `quest_record_grease_pencil` — Under Glass

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `loc_transit_authority_hq` |
| **Rooms** | lobby, map glass, DOB desk, overlay bench, radio gallery |
| **Prereqs** | Plate quest resolved (any) |
| **Time** | 50–80 min |
| **Synopsis** | Convoy 12's hold is a blotter and a dead telephone. Overlay printed plates would replace the grease pencil. Copying the pencil discovers `lore_grid_convoy_slots` if not already. Replacing it mutates the map. |
| **Objectives** | 1. Enter lobby. 2. Inspect map glass (copy / leave / allow Overlay print). 3. Sit the DOB desk. 4. Radio gallery: last turn-back order. 5. Choose overlay bench: install / crate / palimpsest. |
| **Rewards** | `item_sr_grease_pencil` or `item_sr_printed_slot`; `lore_sr_held_dob` |
| **Mutation** | `mutation_transit_maps` |
| **Holdfast** | Edor's DOB error rhymes; do not copy his quest. Cluster Schedule still people, not buses. |
| **Duty Roster** | Convoy 12 grammar on Kess's clerk-book. Chart still people. |

---

### `quest_record_wrong_stacks` — Grey Brick

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `loc_municipal_archive` |
| **Rooms** | vestibule, grey brick, reading cage, loading dock |
| **Prereqs** | Transit started |
| **Time** | 45–75 min |
| **Synopsis** | Garrison already searched for the Schedule. Overlay is filing site plates into municipal drawers. The brick may hold a field index. It will not hold households. |
| **Objectives** | 1. Vestibule visitor book. 2. Choose: dig brick / leave / pull the half-sunk plate. 3. Reading cage: copy index or not. 4. Loading dock crate `RECORD / NOT SCHEDULE`. |
| **Rewards** | `item_sr_field_index` (not Schedule); collapse risk |
| **Mutation** | `mutation_archive_dug` / `_sunk` |
| **Holdfast** | Ormund's drawer is still the household Schedule. Index is ground. |
| **Duty Roster** | Completeness vs blankness: Kess vs Nila vs filing plates into brick. |

---

### `quest_record_the_book` — The Visited Column

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `location_ministry_of_truth_bunker` **SPINE** |
| **Rooms** | stair, enquiry, scoring, registrar, obstacle annex, dead phone |
| **Prereqs** | Archive **or** Transit maps resolved |
| **Time** | 60–90 min |
| **Synopsis** | Ira Vell has the Standing Record. The visited column is empty until you have been. Overlay wants the book out. A photocopy in inventory does not fill the Vault cage. |
| **Objectives** | 1. Stair authenticator. 2. Enquiry memo. 3. Scoring floor (occupation points — Frayne absent, clerks 9). 4. Registrar: see the book; do not skip. 5. Obstacle annex (D/9 paper, not a Dead Hand crawl). 6. Dead phone blotter. 7. Choose: copy in place / take / Overlay receipt / refuse. |
| **Rewards** | `item_sr_record_copy` (incomplete); Ira relationship; `lore_sr_standing_record` |
| **Mutation** | `mutation_ministry_recast` (kills Protocol Zero flavour) |
| **Holdfast** | Two Schedules in Cluster drawer remain households. Record is sites. 12-C finds addresses if plates stand. |
| **Duty Roster** | Roster wall is people. Ira will not write your living into the Record. |

---

### `quest_record_mass_or_lot` — The Needle

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `loc_weighbridge` |
| **Rooms** | plate, hut, receipts, overlay lots |
| **Prereqs** | Book seen |
| **Time** | 40–65 min |
| **Synopsis** | Osric Tann prices mass. Overlay lots have no kilograms. Edor may be waiting (*Holdfast*). The Grain Exchange second paragraph still wants this site. |
| **Objectives** | 1. Stand on the scale or put a plate on it. 2. Hut: calibration weight. 3. Receipts: copy / steal white stack / leave. 4. Lot stack: install on the beam / crate / palimpsest column. |
| **Rewards** | `item_sr_weigh_carbon`; Osric trust |
| **Mutation** | `mutation_weigh_lots` / `_mass_only` |
| **Holdfast** | Edor's return sites vs occupations. Ice Road weigh hut is cousin, not copy (different hut, salt). |
| **Duty Roster** | Levy column mass vs named rows. Pell numbers vs lots. |

---

### `quest_record_hands` — Plot 114

| Field | Value |
|---|---|
| **Type** | expedition |
| **Locations** | `loc_grange_hall` then `loc_the_allotments` (chain; both required) |
| **Rooms** | Grange 4 + Allotments 5 |
| **Prereqs** | Weighbridge started **or** Frayne brass demand |
| **Time** | 70–110 min |
| **Synopsis** | The Verge names places by hand and waiting list. Overlay names them by lot. Dara Mewn waters a plot. If levy took the caretaker, the hut is dark and Overlay has already been. |
| **Objectives** | 1. Grange porch (weapons). 2. Ledger: names vs numbers. 3. Kitchen notice. 4. Allotments gate. 5. Noticeboard sleeve. 6. Named plot. 7. Brass bin (plates mixed with fittings). 8. Vote or leave Overlay notice. |
| **Rewards** | `mark_sr_waitlist`; Frayne minutes delta |
| **Mutation** | `mutation_verge_names` |
| **Holdfast** | Membrane strip: irrigation tap weaker; Dara says so. Cluster missing-strip may list caretaker. |
| **Duty Roster** | Hadi/levy absence = hut dark. Brass tin vs bin. Kess pencil vs plot names. |

---

### `quest_record_friendly_obstacle` — Listed Charges

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `loc_bridge_seven` |
| **Rooms** | near, span, charges, overlay survey |
| **Prereqs** | Hands **or** Book |
| **Time** | 50–80 min |
| **Synopsis** | Overlay clipboard: friendly obstacle. D/9 already marked the rail. Looking from the span is a room. Checking the detonator is a branch that mutates Toll access — not an action setpiece. |
| **Objectives** | 1. Near bank. 2. Span: stone/scratch (refuse to move it). 3. Underside: look / refuse / check housing. 4. Survey clipboard: sign / scrape / copy. |
| **Rewards** | `item_sr_obstacle_line`; Toll delta |
| **Mutation** | `mutation_bridge_listed` / `_disturbed` |
| **Holdfast** | Cutters' dark-lamp cousin: a listed charge is not a lit road. Do not blast (*Yara*). |
| **Duty Roster** | Pell/Voss intercept routes if the span is "closed" in the Record. |

---

### `quest_record_the_failure` — As Far As It Opened

| Field | Value |
|---|---|
| **Type** | expedition |
| **Location** | `loc_lock_gate_four` **SPINE** |
| **Rooms** | towpath, control, Benno, gauges, leaf, reclaim plate |
| **Prereqs** | Bridge **or** Book; Drown access (boat / pumps / Ice Road north outlet) |
| **Time** | 70–120 min |
| **Synopsis** | COMPLETE plate vs mid-cycle gauges. Benno lives in the failure. Overlay cannot close a flood with brass. District 8 can *file* it closed. |
| **Objectives** | 1. Towpath (Nomi etiquette if Shallows used). 2. Control wheel. 3. Benno's bunk: copy his rise-chart or not. 4. Gauges. 5. Gate leaf. 6. Reclaim plate: leave / scrape / palimpsest. |
| **Rewards** | Benno companion; `lore_drown_the_failure` if not yet; `item_sr_benno_chart` |
| **Mutation** | `mutation_lock_complete_lie` / `_gauges_filed` / `_plate_down` |
| **Holdfast** | Ice Road dark: Cut-adjacent Drown approach harder. Desalination recast is still Holdfast's; this is the gate that made the estuary. |
| **Duty Roster** | Overflow 11 must not be "completed" as a site if Nila access held. |

---

### `quest_record_fallback` — Fourteen, a Gap, Six

| Field | Value |
|---|---|
| **Type** | expedition |
| **Locations** | `loc_alloc_12b` **SPINE**; optional `loc_pump_station_nine` branch |
| **Rooms** | 12-B 5; Pump 5 if branch |
| **Prereqs** | Lock visited **or** Sela 12-B kit quest (*Duty Roster*) started |
| **Time** | 60–110 min |
| **Synopsis** | Overlay refreshes 12-B as a finished overflow address. The kit is water. The stencil is a levy map. Pump Nine condemned keeps the Vault far; one dry motor is a room that changes travel. |
| **Objectives** | 1. Stair chalk (do not close the gap). 2. Unprovisioned hall. 3. Kit: leave / take / copy notes only. 4. Water cup. 5. Stencil: refresh / scrape / original. 6. Optional: Pump approach → dry motor vs condemned tag (`record_pump_live` / `_condemned`). |
| **Rewards** | `item_halvard_kit_notes` feed (*Holdfast*); pump mutation |
| **Mutation** | `mutation_12b_address` / `_kit_gone` + pump flags |
| **Holdfast** | 12-C can cite 12-B as a pool site if stencilled. Sela clinic claim language. |
| **Duty Roster** | Kit quest. Blank Rows: numbering 12-B is how a hole becomes a pool. Ink of this site threatens 11. |

---

### `quest_record_which_gazetteer` — The Second Copy

| Field | Value |
|---|---|
| **Type** | expedition / story |
| **Locations** | `loc_records_annex` then `location_the_memory_vault` **SPINE** |
| **Rooms** | Annex 4 + Vault 6 |
| **Prereqs** | Ministry book **and** Lock **and** 12-B resolved (any branches). Cage stays empty if skipped. |
| **Time** | 80–130 min |
| **Synopsis** | Quil will not file plates without spoken site names. Sole will not enter ground on one testimony. Maren waits in the airlock with hats of brass. The player writes which gazetteer stands. Not from the bunker. |
| **Objectives** | 1. Window entry. 2. Dusted room. 3. Name desk: say sites or refuse. 4. Refused crate. 5. Vault dock (etiquette). 6. Airlock plates. 7. Stacks. 8. Sole's table. 9. Second-copy cage (filled only if route walked). 10. Book's place: file Record / lived map / both / nothing. |
| **Rewards** | Ending flag; history second paragraphs at **each mutated featured site**; optional `victory_the_standing_record` |
| **Mutation** | `mutation_gazetteer_stands` / `_lived` / `_palimpsest` / `_scraped` |
| **Holdfast** | Ormund drawer gains a Record page or a refusal blot. Hatch reversed escort list of *places*. Levy pathfinding. |
| **Duty Roster** | Chart vs ground. Escort reads wall **and** posts. Burned chart + scraped plates = foreign list of both people and sites. |

---

**Main quest player time:** ~8–12 hours including travel/needs, not including side catalog.

---

## 4.2 Side quests (~18)

### Overlay / Current (4)

| id | Giver | Location | Hook | Objectives | Mutation / reward |
|---|---|---|---|---|---|
| `quest_sr_paint_short` | Maren | Transit bench / Km 19 crate | Stencil pigment is lamp-black and oil. Ivy's oil. Yara's stick. | 1. Source oil (home / Ivy / Yara). 2. Do not ask the Lamplighter exception. 3. Deliver or refuse. | Paint rooms wet or dry; eleven-day dark risk |
| `quest_sr_plate_brass` | Maren | Allotments brass bin / filtration tin | Overlay plates are brass. Frayne's fittings are brass. Same bin. | 1. Count. 2. Separate plates from handles. 3. Deliver to Overlay / Works / tin. | `mutation_brass_plates`; stacks with Holdfast/Roster tin |
| `quest_sr_maren_sheet` | Maren | Ostrowski corridor | Ostrowski sells where things are. Her sheet names them. They have never compared. | 1. Carry both sheets to a featured site. 2. Stand there. 3. Do not make them shake hands. | Travel-time hint; Ostrowski will_not still holds |
| `quest_sr_overlay_withdraw` | Overlay access | any plated site | Scrape three plates with no replacement name. | 1. Scrape. 2. Hear the withdrawal. 3. Caretakers gone. | Overlay rooms dark; Dara/Maren labour gone |

### Site-keepers (4)

| id | Giver | Hook | Objectives | Mark |
|---|---|---|---|---|
| `quest_sr_ira_column` | Ira | She will not fill *visited* from your inventory photocopy. | 1. Return after 3 sites. 2. She writes or refuses. | Book honest |
| `quest_sr_benno_fuse` | Benno | Control house fuse. Overlay COMPLETE does not replace it. | 1. Find a fuse (substation overlay one-liner). 2. Fit it. 3. Gauges brighter, gate still open. | Gauges recast |
| `quest_sr_quil_dust` | Quil | Dusting rota. Overlay crate undusted on purpose. | 1. Dust or refuse the crate. 2. Say one site name. | Annex recast |
| `quest_sr_osric_weight` | Osric | Calibration weight missing (player or Overlay used it as brass). | 1. Return / replace with Overlay plate (needle wrong) / leave wrong. | Weighbridge forever off if plate used |

### Verge / Toll / Grid (4)

| id | Location | Hook | Objectives | Mutation |
|---|---|---|---|---|
| `quest_sr_lasko_number` | Grange | Overlay notice would list Lasko as a lot, not a vote. | 1. Attend if vote pending. 2. Do not let the notice speak. | Hands vs plate |
| `quest_sr_pell_sites` | `loc_conscription_office` *overlay only* | Pell's quota is people. Overlay gives him addresses. | 1. Hear both. 2. Do not merge. | Intercept risk if addresses exist |
| `quest_sr_sent_back_paint` | Bus loop | Stencil wet. A driver-log contradicts. | 1. Read log. 2. Paint / scrape / street name. | Loop recast |
| `quest_sr_terrace_line` | `loc_terrace_pumphouse` *overlay only* | Lot map on the loophole wall. | 1. Inspect. 2. Leave / scrape. | Harvest access one-line |

### Drown / Holdfast-Roster hooks (4)

| id | Location | Hook | Objectives | Mutation |
|---|---|---|---|---|
| `quest_sr_nomi_plates` | Shallows | Overlay boarding with plates. Etiquette. | 1. Hear once. 2. Plates as cargo that picks a side or not. | Nomi present/absent |
| `quest_sr_kittiwake_name` | `loc_bathymetric_boat` *overlay only* | Overlay soundings vs Kittiwake log. | 1. Compare at the boat. 2. Do not "fix" the log. | Drown nav hint |
| `quest_sr_nila_number` | Overflow 11 *Duty Roster* | Overlay would plate ALLOC-11. | 1. Warn Nila or don't. 2. If plated, hatch dark. | Blank Rows access |
| `quest_sr_kess_refuse` | Roster wall *Duty Roster* | Maren asks Kess to copy site numbers onto the chart. | 1. Kess refuses. 2. Player may still write. | Chart irregular |

### Repeatable (2)

| id | Hook | Loop |
|---|---|---|
| `quest_rep_site_watch` | Leave 1 survivor at a featured site 8h. Utility AI: watch, not fight. | Encounter check; room stays "occupied"; Overlay or lived labour |
| `quest_rep_plate_audit` | After palimpsest: walk 3 plated sites, note which layer is winning. | Memory strata tick; no gold |

---

## 4.3 Location encounter catalog (20)

Room-keyed. Aftermath **changes the room**. Combat = existing expedition resolution. No fantasy. No Dead Hand arena.

| id | Room | Trigger | Cost | Aftermath |
|---|---|---|---|---|
| `enc_site_plate_screwer` | `room_km19_post` | first inspect | time, brass choice | plate on / scraped; post recast |
| `enc_site_ivy_oil` | `room_km19_oil_tin` | take oil | Lamplighter rule | tin empty; next lighting short |
| `enc_site_maren_bench` | `room_transit_overlay_bench` | install print | maps | grease vs print |
| `enc_site_dob_ring` | `room_transit_dob_desk` | inspect blotter | morale mark | telephone still dead |
| `enc_site_brick_collapse` | `room_archive_grey_brick` | dig | health, hours | brick open or plate sunk deeper |
| `enc_site_ira_bell` | `room_ministry_enquiry` | ring bell | time | Ira comes or does not (if Overlay took her book) |
| `enc_site_rubric` | `room_ministry_scoring` | read poster | none | Codex; no feeling told |
| `enc_site_needle_joke` | `room_weigh_plate` | plate on scale | receipt | lot-as-mass column |
| `enc_site_edor_stool` | weigh hut | Holdfast clerk | names | occupations vs sites |
| `enc_site_ledger_cross` | `room_grange_ledger` | inspect | none | Overlay column pasted or not |
| `enc_site_dara_dark` | allot hut | levy absence | no caretaker | Overlay already watered with numbers |
| `enc_site_brass_mix` | `room_allot_brass_bin` | search | brass count | plates vs fittings |
| `enc_site_span_look` | `room_bridge_charges` | look | rads, fear-as-procedure | listed / undisturbed |
| `enc_site_detonator` | charges | check | Toll access | `mutation_bridge_disturbed` |
| `enc_site_benno_kettle` | `room_lock_benno` | enter | food if you take the kettle | bunk recast |
| `enc_site_complete_crew` | `room_lock_reclaim_plate` | Overlay install | time, confrontation | plate state |
| `enc_site_dry_motor` | `room_pump_dry_motor` | energise | health, fume | pump live or tagged |
| `enc_site_chalk_gap` | `room_12b_stair` | close the gap | Sela/Nila flags | gap held or filled |
| `enc_site_quil_rule` | `room_annex_name_desk` | file without speaking | access | crate refused harder |
| `enc_site_sole_aloud` | `room_vault_sole_table` | say the site | completeness | cage fills or not |
| `enc_site_watch_night` | any featured, repeatable | site-watch | fatigue | occupied description |
| `enc_site_garrison_address` | conscription overlay | Pell + plates stand | people | intercept uses addresses |

*(20 rows; two extra as optional — implementer may cap at 16 if sprint-sliced. Spec holds 20.)*

---

# SECTION 5 — SYSTEMS (max 3)

**Cap:** 3 new plain-C# systems. No LLM. Event-raising. Save-safe. Host-callback injection. **Do not rebuild** IceRoad, BrineWater, CensusClaim, Waystation, DutyRoster, ShelterEncounter, MoraleMark, WorldStateConsequence, LoreDiscoveryIndex — **hook** them.

Cross-tool QA: layout adjacency × memory recast × Overlay access is **three coupled variables**. Implementer ≠ reviewer (Prompt #26). Reviewer sees diff + this spec only.

---

## 5.1 `LocationLayoutSystem`

**id:** `location_layout_system`  
**What it is:** Room cards + adjacency for featured parents. Not a walker. Not 3D.

**Mechanics:**
- JSON `standing_record_layouts.json`: `parentLocationId`, `rooms[]` with `id`, `displayName`, `adjacent[]`, `unlockRule`, `inspectKey`.
- Expedition at parent: player picks a **lit** room. Tick: inspect / encounter / choice. Adjacent rooms light or stay dark.
- Dark room: visible as a name, not enterable, until rule (inspected neighbour, item, Overlay access, pump live).
- Events: `OnRoomEntered`, `OnRoomUnlocked`, `OnLayoutMutated`.
- Save: per-parent set of unlocked rooms + flags.

**UI/UX:** Location Detail list. ASCII is design-time; runtime is a simple node list + "dark." Diegetic: a hand-drawn adjacency on Overlay paper or Benno's wall.

**Balance:** 3–8 rooms. Average tick 8–15 minutes including needs. Cannot enter all rooms from the parent arrival click.

**Unrealistic:** walkable bunker renderer, first-person rooms, action stealth.

**Integration:** `ExpeditionSystem` at parent; `LoreDiscoveryIndex` on room id **or** parent on first arrival plus room inspect for extra keys.

---

## 5.2 `LocationMemorySystem`

**id:** `location_memory_system`  
**What it is:** Lore strata + recast descriptions. The gazetteer the save keeps.

**Mechanics:**
- Three strata per featured site: `pre` / `after` / `now` (see creative pack).
- Active `now` string selected by mutation flags (plated, scraped, palimpsest, caretaker absent, pump live…).
- Unfeatured sites: optional one-line overlay table.
- `world_history` second paragraphs: `discovery_location_id` = parent or room; trigger `location_explore` or `inspection` of room.
- Events: `OnLocationRecast(string locationId, string stratumId)`.

**UI/UX:** Inspect shows `now`. Codex may show pre/after only after standing there (not after reading a menu dump).

**Balance:** Recasts are sentences, not sermons. No `Morale +2` in the description.

**Integration:** `WorldStateConsequenceSystem.HasMutation`; Holdfast recasts (desalination occupied) remain Holdfast's; this system **adds** Overlay/lived/palimpsest layers, does not overwrite Ice Road.

---

## 5.3 `SiteEncounterSystem`

**id:** `site_encounter_system`  
**What it is:** Encounters keyed to **room**, not generic combat tables.

**Mechanics:**
- Table §4.3. Cooldowns. Seed `_worldSeed + 1808`.
- Trigger: on room enter or on inspect object.
- Aftermath writes layout + memory flags.
- Overlay access: granted/withdrawn like a Current (mirror Blank Rows: no raid, rooms go dark of Overlay labour).
- Events: `OnSiteEncounterStarted`, `OnSiteEncounterResolved`, `OnOverlayAccessChanged`.

**UI/UX:** Existing event modal + `threateningBodyText` if Overlay/Office/Garrison trust low.

**Balance:** Max one site encounter per room visit unless crisis (Pump energise, detonator). Do not starve the route.

**Integration:** `ExpeditionSystem.Encounters`; `CensusClaimSystem` if a plated site becomes a levy address; `DutyRosterSystem` for site-watch assignments; Ice Road node filter for Km 19.

---

## Systems explicitly not in this expansion

- No 4th simulation class (Overlay access lives in SiteEncounter + Current catalog).
- No seventh Power in `faction_lore.json`.
- No 3D interiors. No Dead Hand action pack. No second coast. No bunker-wing clone.
- No hatch retune. No new affliction unless a distinct skin/fume already exists — prefer reskin cause.

---

# SECTION 6 — CHARACTERS AS SITE-KEEPERS & ITEMS AS SITE-KEYS

## 6.1 Companions (assignable labour, not a party)

| id | Name | AI bias | Will not | If they die / leave |
|---|---|---|---|---|
| `npc_maren_holt` | Maren Holt | Plate, log, refuse falsified gauges | Number Alloc 11 if she knows the rule | Overlay juniors slower; plates crooked |
| `npc_benno_kade` | Benno Kade | Gauge, refuse COMPLETE | Call the Drown a crater | Lock house empty; Overlay plate unopposed |
| `npc_quil_esser` | Quil Esser | Dust, say names, refuse plates-as-names | Skip two-witness | Annex undusted; crate easier to sneak |

Ira, Osric, Dara are **not** expedition companions. Sole / Ivy / Edor / Kess remain theirs.

Utility AI (*PROPOSED*): `Action_SiteWatch`, `Action_PlateLog`, `Action_GaugeCopy`. Seed `_worldSeed + 1808`.

## 6.2 Crises (5) — location crises, not bosses

| id | Name | Phases | Failure | Success looks like |
|---|---|---|---|---|
| `crisis_the_plate` | Seam | Plate / oil / crate / Ivy | Eleven-day dark or Overlay withdraw | A post you can live with |
| `crisis_the_book` | Visited column | See / copy / take / lie | Vault cage empty | Honest incomplete or honestly walked |
| `crisis_the_lock` | Failure | Gauges / COMPLETE / Benno | Filed closed while flooding | Chart matches the leaf |
| `crisis_the_fallback` | 12-B | Kit / stencil / pump | Levy address on a tomb | Water and/or an honest number |
| `crisis_the_gazetteer` | Second copy | Speak / file / scrape | Foreign list of places at hatch | A gazetteer the rooms still match |

Killing Maren does not stop Overlay; a junior continues. Killing Ira: the book is still a book. Do not make that a win.

## 6.3 Item sets (site-keys)

Existing tools remain canonical. New ids **PROPOSED**.

| Set | Pieces | Function |
|---|---|---|
| `set_sr_plates` | `item_sr_plate_cut19`, `item_sr_plate_blank`, `item_sr_stencil_pot` | Quest keys; brass mass |
| `set_sr_paper` | `item_sr_record_copy`, `item_sr_field_index`, `item_sr_benno_chart`, `item_sr_grease_pencil` | Incomplete until walked |
| `set_sr_keys` | `item_sr_cage_string`, `item_sr_weigh_carbon`, `item_sr_waitlist_sleeve` | Room unlocks |
| Reuse | `item_order_12c`, `item_halvard_kit_notes`, `item_roster_pencil`, `item_map_sheet_ice_road` | Sister packs |

Unique objects (8):

| id | Name | Where | First line |
|---|---|---|---|
| `item_sr_record_copy` | Standing Record (incomplete) | Ira | The visited column is blank. Inventory does not fill it. |
| `item_sr_plate_cut19` | CUT-19 | Km 19 | The stencil is under it, or it is in your pack and the stencil is cold. |
| `item_sr_benno_chart` | Rise | Lock house | Pencil. Metres. Dates. No COMPLETE. |
| `item_sr_field_index` | Not the Schedule | Archive cage | Sites. Drawers. The Garrison already looked. |
| `item_sr_obstacle_line` | Listed | Bridge survey | Friendly. Visible. Still taped. |
| `item_sr_waitlist_sleeve` | Sleeve | Allotments | Four names alive. The plastic is fogged. |
| `item_sr_cage_string` | String-tied | Annex crate | Quil has not cut it. |
| `item_sr_printed_slot` | Convoy 12, typeset | Overlay bench | HELD is not in the font. |

## 6.4 Achievements (16)

`ach_sr_*`. No kill-counts.

| id | Name | Condition |
|---|---|---|
| `ach_sr_seam` | Last Lamp | Resolve Km 19 plate |
| `ach_sr_glass` | Under Glass | Transit maps choice |
| `ach_sr_brick` | Wrong Stacks | Archive cage |
| `ach_sr_book` | Visited | See Ira's book in the registrar room |
| `ach_sr_needle` | Kilogram | Weighbridge choice |
| `ach_sr_114` | Plot | Allotments sleeve |
| `ach_sr_listed` | Friendly | Bridge clipboard |
| `ach_sr_open` | Mid-cycle | Lock gauges copied |
| `ach_sr_gap` | Six | 12-B chalk gap held |
| `ach_sr_pump` | One Motor | Pump live |
| `ach_sr_tag` | Condemned | Pump tagged |
| `ach_sr_aloud` | Mouth | Say a site at Annex/Vault |
| `ach_sr_stands` | Record | Ending stands |
| `ach_sr_lived` | Lived | Ending lived |
| `ach_sr_both` | Palimpsest | Ending both |
| `ach_sr_none` | Unnumbered | Ending scraped |

---

# SECTION 7 — UNITY PLAN (JSON, NO 3D)

## 7.1 Architecture

| Concern | Existing pattern | Standing Record |
|---|---|---|
| Data | StreamingAssets JSON | `standing_record_layouts.json`, `standing_record_memory.json`, `standing_record_encounters.json`, append `currents.json` (`faction_the_overlay`), location description overlays, `world_history` append |
| Logic | Plain C# | `LocationLayoutSystem`, `LocationMemorySystem`, `SiteEncounterSystem` |
| Host | GameBootstrap partials | `GameBootstrap.StandingRecord.cs` |
| AI | UtilityAI | Site-watch actions; no LLM |
| UI | UITK Location Detail | Room list; dark rooms; Record document |
| Map | GeneratedMap | **No new region.** Parent nodes only. Km 19 if Holdfast shipped |
| Lore | LoreDiscoveryIndex | `lore_sr_*`; room or parent ids |
| Quests | QuestlineSO.Ids | `quest_record_*` |
| Consequences | WorldStateConsequenceSystem | New mutations; **no** Overlay in `_hegemony` |

**Ids namespace:** `room_*`, `faction_the_overlay`, `npc_maren_holt`, `npc_ira_vell`, `npc_benno_kade`, `npc_quil_esser`, `npc_osric_tann`, `npc_dara_mewn`, `quest_record_*`, `quest_sr_*`, `enc_site_*`, `lore_sr_*`, `mutation_*` listed, `ending_record_*`, `item_sr_*`, `ach_sr_*`.

**Do not mint:** `loc_alloc_12b`, `loc_cut_kilometre_19` (Holdfast), Overflow ids, a 7th `faction_lore` row, Tessarat, 7G.

## 7.2 Assets (specify only; generate later into `generated_AIassets/`)

Dry-gouache, isolated objects, no readable AI text, no flags, no gore, no fantasy glow.

| Asset | Notes |
|---|---|
| Room cards × ~66 | Posts, scales, gauges, chalk, brass plates |
| NPC portraits × 6 | Deferred if no UI slot |
| Item icons × ~20 | Plate, incomplete book, rise-chart |
| **Not in scope** | 3D interiors, full VO, new music album |

## 7.3 Sprints (4 × 3 weeks)

| Sprint | Goal | Deliverables | Verify |
|---|---|---|---|
| **S1 — Seam & graph** | Layouts work | `LocationLayoutSystem`; Km 19 + Transit layouts; Maren; quests plate + grease; Overlay current stub | Save roundtrip rooms; compile PASS |
| **S2 — Memory** | Recasts work | `LocationMemorySystem`; Ministry + Archive + Weighbridge; Ira; Osric; recast stale Ministry text | Discovery keys; compile PASS |
| **S3 — Drown spine** | Route works | Lock, 12-B, Pump Nine; Benno; Quil; SiteEncounter table slice | Pump/lock mutations; compile PASS |
| **S4 — Gazetteer** | Endings work | Vault + Annex; Grange/Allotments/Bridge; four endings; Holdfast/Roster flags; 8 sides | Ending exclusive; compile PASS; PlayMode: one route |

## 7.4 Risks

| Risk | Mitigation |
|---|---|
| Feels like a collectathon of POIs | Cap 14 layouts. Route required. Cage empty if skipped |
| 3D dungeon temptation | Spec forbids. Room cards only |
| Duplicates Holdfast census | Sites vs households. Shared flags, different rooms |
| Duplicates Duty Roster chart | Ground vs people. Kess refuses to copy |
| Duplicates The Tally | Tally counts. Overlay names. Do not merge |
| Duplicates Archivists corroboration | Sites said aloud, not only the dead. Quil's crate is ground copy |
| Protocol Zero / social-media Vault leak | Recast on unlock. Do not extend |
| Brass economy triple-stack | Same metal, three buyers, silence |

## 7.5 QA cases (minimum)

1. Old save → Km 19 dark of Overlay → plate quest → rooms unlock by adjacency  
2. Photocopy Record in inventory → Vault cage still empty until Lock + 12-B stood in  
3. Ice Road dark → Km 19 reachable from south; Cut-adjacent Drown harder  
4. Levy honour → Allotments hut dark; Overlay numbers present  
5. Membrane strip → Allotments tap; Dara line  
6. Roster ink of Overlay numbering 11 → Nila hatch dark  
7. Pump live → Vault travelHours down; condemned → unchanged  
8. Scraped plates + burned chart → hatch escort foreign list of people **and** places  
9. Ostrowski sheet vs Maren sheet at one site — both true, disagree  
10. Ministry recast: no Protocol Zero in inspect  
11. Compile + EditMode PASS  

---

# SECTION 8 — RETENTION

## Day-one (post-unlock)

- Kilometre 19. A plate that was not on Ostrowski's last sheet. A lamp that still is.
- First dark room: plate crate not enterable until the post is inspected.
- Ira's visited column, empty, after a photocopy that felt like cheating and was.

## 3–6 month

| Month | Content |
|---|---|
| M1 | Remaining sides; site-watch repeatable; radio/tannoy text at Transit |
| M2 | Long Walk mentions a plated post (one night, *existing* Current) |
| M3 | Palimpsest decay: one layer winning per season profile (data) |
| M4–6 | Shareable: their gazetteer second paragraphs. No live service. No gacha. |

## Feedback loops

| Loop | Need served |
|---|---|
| Route | Located knowledge spent |
| Plate vs name | Identity of *places* |
| Site-watch | Fatigue, roster labour |
| Pump | Drown access, thirst politics |
| Vault cage | Completeness vs blankness vs Overlay |

---

# SECTION 9 — PLAYER-FACING CONTENT BUDGET

See creative pack for shippable prose. Plan-side estimate:

| Bucket | Words |
|---|---|
| This bible | ~12,000 |
| Creative pack target | **18,000–28,000** |
| Full VO | **unrealistic** |

---

# SECTION 10 — LORE CONSISTENCY + TWO-WAY FLAGS

## 10.1 Must not contradict

| Canon | Stance |
|---|---|
| Sector 4 map closed; no fifth Power | Overlay is a Current |
| `faction_lore.json` at 6 | No 7th row |
| Player = Allocation 12, unlisted | Record does not allocate them |
| Sela card; four hatch branches | Modify escort *places*, do not replace |
| Sole files, 41.2, not allocated | She files ground if corroborated; she does not "fix" herself |
| Lamplighter: no exception | Oil vs plate side quest |
| Quiet House: never adjudicate back room | Untouched |
| Rebuilders brass | Bin mixes plates and fittings |
| D/9 marks visible | Obstacle annex + Bridge rail; no Dead Hand crawl |
| Hydro-Barons / Cluster | Sister pack; hook |
| Duty Roster chart | People, not sites |
| No magic, no real countries/people, no glorified violence | Held |
| No terraformers, Tessarat, 7G, androids, neuromancers | Held |

## 10.2 Small recasts (justified)

| Item | Change | Why |
|---|---|---|
| Ministry live description | Continuity office; Record; Ira | Stale Protocol Zero is a different game |
| Vault live description | Archivists; second copy cage | Stale social-media farm is a different game |
| Featured site descriptions | Room-aware `now` stratum | Located knowledge spent |

**Not retconned:** Holdfast geography, Duty Roster wings, barge cannibals (Holdfast's), TrueEnding, faction namespaces (do not pick a side).

## 10.3 Timeline

| When | Event |
|---|---|
| Exchange−4Y | Office of Continuity; Standing Record scoped as sibling to Schedule |
| Exchange−3Y | Bunker Boom; cadastral plates ordered, not all installed |
| Exchange−1M | Quiet Evacuation; Transit grease pencil; buses sent back |
| Exchange+0 | Ministry senior grades leave; Ira stays; Record incomplete |
| Exchange+3W | Lock Gate Four fails; Drown begins |
| Exchange+2Y | Halvard at 12-B; chalk |
| Exchange+4Y | Ice Road (*Holdfast*); Blank Rows practice (*Duty Roster*) |
| Exchange+5Y | **Now.** Overlay walks with leftover plates. Player discrepancy is also a *site* discrepancy. |

## 10.4 Two-way flags with Exp 1 and Exp 2 (10)

1. **Ice Road dark** → Km 19 south-only; Cut-adjacent Drown sites harder; Yara not at the plate.  
2. **Levy honour / absence** → Allotments hut dark (no Dara); Grange labour thin; Overlay numbers arrive first.  
3. **Membrane strip** → Allotments water; brass demand stacks with Overlay plates.  
4. **Roster ink / pencil / blank / burn** → hatch escort list of people **plus** Overlay list of places if plates stand.  
5. **Hadi listed/sent/hidden** → caretaker labour; hut occupied or not.  
6. **12-C live** → plated sites become levy addresses; 12-B stencil is a pool pin.  
7. **Sela clinic vs stay** → 12-B kit language; she leaves the stencil room if called salvage.  
8. **Blank Rows access** → numbering 11/12-B in ink darkens 11.  
9. **Kess pencil** → she refuses site numbers on the chart; player can still write (irregular).  
10. **Ostrowski ice-road sheet** → two names for Km 19; Maren's sheet is the other column.

Additional hooks (not the ten): Waystation site-watch; Nomi etiquette with plates; Pell addresses; Sole unsigned 12-C at Vault table; playground/tin brass vs Overlay plates.

---

# APPENDIX A — Integration matrix (condensed)

| Flag | Standing Record change |
|---|---|
| `ice_road_open` | Km 19 two-sided; Benno may have seen Cutters |
| Ice Road dark | Seam from south; Lock via Shallows or pumps |
| `holdfast_levy_honour` | Dara gone; Overlay at plots |
| `holdfast_membrane_sector4` | Tap; iodine vs pigment oil |
| `mutation_roster_ink` | Escort reads wall + posts |
| `mutation_roster_burned` | Escort brings foreign people **and** Overlay places |
| `faction_blank_rows` lost | No hide; 12-B safer to number |
| `mutation_highway9_cleared` | Transit/Ministry faster |
| `mutation_transit_tax` | Pell/checkpoint on Grid route |
| `ending_holdfast_dark_road` | Edor's incomplete return in weigh receipts room |

Reverse: plated gazetteer → Ormund pathfinding; levy intercept at numbered posts; Cluster missing-strip gains *sites*; Duty Roster fourteenth visitor may be Overlay asking to sleep near a chart.

---

# APPENDIX B — Id checklist (re-grep before commit)

Verified non-colliding against `locations.json` / `locations_expansion3.json` / `QuestlineSO.Ids` / `currents.json` / `faction_lore.json` / Holdfast / Duty Roster proposed ids **at time of writing**.

**Existing reused:** all 14 featured parents; `loc_conscription_office`, `loc_terrace_pumphouse`, `loc_bathymetric_boat`, `loc_the_shallows_market`, Overflow 11, roster wall, Cluster office, `brass_fittings`, discovery keys listed as existing.

**New (selected):** `expansion_the_standing_record`, `faction_the_overlay`, `npc_maren_holt`, `npc_ira_vell`, `npc_benno_kade`, `npc_quil_esser`, `npc_osric_tann`, `npc_dara_mewn`, `room_*` listed in §2, `quest_record_the_plate`, `quest_record_which_gazetteer`, `lore_sr_standing_record`, `mutation_gazetteer_stands`, `ending_record_lived`.

Do not mint a 7th `faction_lore` row. Do not mint `loc_alloc_12b`. Do not use Tessarat / 7G / neuromancer ids.

**The Tally** (`faction_the_tally`) remains a different Current. Do not merge.

---

# APPENDIX C — Next prompt (implementation)

> Implement Sprint 1 of `docs/expansions/expansion_03_the_standing_record_plan.md`: `LocationLayoutSystem` (plain C#, events, save/load), JSON layouts for Kilometre 19 (`loc_cut_kilometre_19` if Holdfast present, else stub parent) and `loc_transit_authority_hq`, quests `quest_record_the_plate` / `quest_record_grease_pencil`, NPC Maren Holt, Current stub `faction_the_overlay` in `currents.json`. Do not add a 7th faction to `faction_lore.json`. Do not build 3D interiors. Register quest ids in `QuestlineSO.Ids`. Re-grep all new ids. Verify Unity batch compile and EditMode tests. Cross-tool QA: reviewer is not the implementer (Prompt #26) — adjacency × recast × Overlay access.

---

# APPENDIX D — House-voice samples (more in the creative pack)

**`room_km19_post`**
> The stencil is still there. The plate is on top of it, four screws, municipal brass, `CUT-19 / LAMP`. The lamp is lit on Ivy's schedule. The plate does not care. You can take the plate. The stencil will be colder. You can leave both. The next clerk will read the brass.

**`room_ministry_registrar`**
> A book the size of a ledger, not a Schedule. Columns: site, cadastral, lived name, visited. The visited column is empty. Ira's pencil is tied to the spine. You can copy the pages. The column will still be empty until you have stood in the rooms it lists.

**`room_lock_gauges`**
> Mid-cycle. The needle is a fact. Someone has written COMPLETE on a plate downstairs. The needle has not been informed.
