# ASHFALL — Expansion Design Bible

**Title:** ASHFALL: THE DUTY ROSTER  
**Internal id:** `expansion_the_duty_roster`  
**Status:** Design bible for review. No game data has been edited. No C#.  
**All new ids below are PROPOSED** unless marked *existing*.  
**Tone lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.  
**Sister pack:** Expansion 1 is `expansion_the_holdfast`. This pack does **not** reopen District 8 geography. It is the unlisted home.

---

# ANALYSIS PHASE

## 1. Strengths and gaps after The Holdfast

### What Holdfast already spends

- The **allocated world** as a destination: Cluster 7, the Ice Road, the plant, the Office, Reconstruction Order 12-C, the hatch reversed.
- A closed Sector 4 map. Four Powers. Currents, not a fifth Power. `faction_lore.json` stays at six.
- Orphan assets reclaimed: `hydro_barons`, desalination, icebreaker / Migration fades given a coast.
- Four systems that must be **hooked, not rebuilt:** `IceRoadSystem`, `BrineWaterSystem`, `CensusClaimSystem`, `WaystationSystem`.
- Named people who will walk south: Edor Vale waits; Ormund files; Yara marks dark; Leva counts resin; Sela may be claimed.

### What is still a hole in the hole

- **Allocation 12 is still only a discrepancy.** Layer 1 already put a wall chart in the bunker — `ALLOCATION 12 — DUTY ROSTER`, all rows blank — and then never made the chart a political machine. Holdfast's levy names three survivors out of a void. The void is this expansion.
- **Shelter is a HUD, not a district.** Hatch dilemma constants exist (`LetThemInContaminationRadsPerHour = 50`, `ForceDeconContaminationRadsPerHour = 10`, `DenyEntryMoralePenaltyForOtherSurvivors = 20`). Meal contamination, duty notes, sleeping-stack crowding, and "who goes to the hatch" are under-authored as *scenes*.
- **Morale is a number.** NeedsSystem already ticks it. What is missing is a **mark**: a flag, a later sentence, a missing person, a quieter room. Not an alignment meter.
- **Sector 4 questlines that change the world** already exist as hegemony mutations (`Highway9Cleared`, `MedicalSupplyGone`, `TransitTax`, `CultShrineAirIntake`). They do not yet notice who is *home*. A levy column, a brass tin, a caretaker, a name Sole can corroborate — those are roster facts.
- **Holdfast endings assume a community that has not been written.** "The duty roster on the wall has names on it that are not the names that slept there" is a Holdfast slide. This pack is that sentence, playable.

### Weaknesses this pack must not pretend away

- The territorial map is **closed**. No fifth Power. No second coast. No new District 8 sub-region.
- Two faction id namespaces remain a live defect. New people are Currents or named NPCs.
- `Victory_TrueEnding` terraformers, Tessarat, Sector 7G, androids, neuromancers: unused.
- Companions are named survivors, not a combat party. "Bosses" are crises.
- Presentation is 2D management: node graph, location cards, UI Toolkit. The bunker-as-stage is **inspectable wings + events**, not a walkable interior renderer.

## 2. Three concepts

| # | Concept | Why it might be the pack | Why it isn't, or how it is used |
|---|---|---|---|
| A | **THE DUTY ROSTER** | The chart is already in Layer 1. Who is on it changes levy, Ice Road labour, Voss's numbers, Frayne's brass, Sole's completeness, and the hatch reversed escort. Shelter is the other district. | **Proceeding.** |
| B | **THE SECOND WINTER** | A named season that thins the Ice Road, kills steam, and forces home encounters. Holdfast-scale only if it is quest-first. | Used as a **SeasonProfile** (`season_second_winter`) that *presses* the roster, not as the spine. A weather DLC would be cheaper than Holdfast and would not pay off the blank chart. |
| C | **THE HATCH ACCOUNT** | Intercom, visitors, contamination vs morale. Strong scenes. | Too narrow. Becomes the Approach region + radio pack inside A. |

**Not considered as spine:** Dead Hand / automated military belt (Holdfast already rejected; D/9 owns leftover orders). A second Holdfast coast (forbidden).

## 3. Choice

**ASHFALL: THE DUTY ROSTER.**

Holdfast is the allocated world: District 8, forms, brine, Ice Road.  
This pack is the **unlisted home**: Allocation 12 as a political space, Sector 4 nodes as quest stages, the bunker/hatch/duty roster/sleeping-stack as a stage.

The Second Winter is a season that can fall across both packs. It is not a difficulty slider with a title screen.

---

# SECTION 1 — EXPANSION OVERVIEW

| Field | Value |
|---|---|
| **Title** | ASHFALL: THE DUTY ROSTER |
| **id** | `expansion_the_duty_roster` |
| **Hook** | The formula stored a reconstruction pool in a hole. The hole has to decide whether it is a community before someone else finishes the form. |
| **Tagline (UI)** | *The chart was left blank. The ice is not.* |
| **Genre lock** | Same game. 2D survival-**management**. Expeditions are node ticks. Shelter wings are location cards + events. No 3D bunker crawler, no party RPG. |
| **Playtime (new content)** | **12–18 hours** main roster + home crises on a mid-game save; **20–28 hours** completionist (Overflow, Quiet House, Holdfast two-way flags, repeatable watches). |
| **Scale honesty** | Equivalent to Holdfast: 4 region-equivalents, 10 main quests, ~18 side, 6 NPCs, ≤4 systems, a creative pack in the same word-band. Not a walkable overworld. |
| **Progression gate (soft)** | Day **60+**, 6+ living survivors, hatch usable (not Buried/Frozen). Can begin **before** the Ice Road. |
| **Progression gate (story)** | Knowledge key `lore_allocation_wrongness` **or** inspecting the roster wall **or** Edor Vale's census started (`quest_holdfast_the_clerk`, *Holdfast*). |
| **Progression gate (hard ending)** | Roster written, erased, or burned (`quest_roster_ink`) **and** at least one Holdfast levy/12-C/hatch flag **or** Day 200 List Layer 5. The two hatches are two doors into the same occupancy. |
| **Does not require** | Holdfast unlocked. If District 8 is dark, this pack still plays; Edor can still arrive from `loc_weighbridge` (*existing*). If Holdfast **is** live, every main quest reads the flags in Appendix A. |
| **Does not add** | A 7th Codex Power. A 16th unrelated `Victory_*.cs` (optional epilogue slide `victory_the_duty_roster` only). New hatch-dilemma magnitudes (Prompt #26). |

### Thesis (unspoken)

Fairness published in advance is still a selection. A blank chart is also a selection. Kindness does not erase a column. Refusing to write a name does not keep the ice from wanting one.

### One-paragraph pitch

District 8 schedules labour. The people still in the hole have to decide what a community is when the formula, the Office, Voss, Frayne, Sole, and the weather all want a piece of the same fourteen bunks. The world changes because **who is on the roster** changes what Sector 4 can still field and what District 8 can still claim. A caretaker sent north is an outfall shift the plant gets and a surgery Ianov does not. A name left in pencil is a census Edor can complete. A name burned is a hatch escort that arrives with the wrong list. The game does not tell you which of those is better. It shows the wall, the leftover food, and who is not in the room.

### Integration strategy (summary; full matrix in Appendix A)

| Layer | How it attaches |
|---|---|
| **Map** | No `region_holdfast` clone. Four **stage-regions**: Stack, Approach, Unlisted Circuit (*existing* Sector 4 nodes), Overflow (one new sub-geography of authenticated holes). |
| **Travel** | Circuit uses existing `travelHours`. Overflow is 1.5–3.0h from home. Approach is 0. Home still ticks `ShelterDegradationSystem` while anyone is north. |
| **Economy** | No new currency. Roster labour is the scarce good: caretaker-hours, night-watch, hatch-openers, brass that can leave the tin. Hooks `DynamicEconomySystem`, Rebuilders brass, Holdfast resin/iodine/calories. |
| **Lore** | New `world_history` under `ashfall` with `discovery_location_id` = shelter wings / Overflow. Does not rewrite District 8 history. Second paragraphs on **endings** and on Holdfast entries when two-way flags fire. |
| **Factions** | One new Current: `faction_blank_rows` in `currents.json` (or `duty_roster_currents.json`). **Do not** add to `faction_lore.json`. |
| **Holdfast systems** | Hook `CensusClaimSystem` (levy names **are** roster rows), `IceRoadSystem` (window vs home watch), `WaystationSystem` (bunks A1–A4 steal from the Stack), `BrineWaterSystem` (membrane strip vs filtration). |
| **Consequences** | New mutations on `WorldStateConsequenceSystem` (or a parallel apply-API). Reuse `mutation_levy_column` if Holdfast already minted it. Do **not** put Office or Blank Rows in `_hegemony`. |
| **Save** | `exp_duty_roster_unlocked` + `DutyRosterState` blob + morale-mark flags + encounter cooldowns. Old saves load; the wall starts blank until the chart quest. |
| **UI** | Diegetic roster document (not a reputation bar). Shelter-wing inspect. Hatch intercom as radio text. Morale marks appear as **later prose**, not `Morale +2`. |

### What the player is managing at home

The same seven needs. The **weights sit on fourteen bunks**.

| Need | How the roster bites |
|---|---|
| Hunger | Who eats. Leftover portions. Levy absence leaves food that looks like grief. |
| Thirst | Filtration labour. Membrane strip (Holdfast) shortens the home clock. |
| Fatigue | Night watch. Ice Road windows force long hauls on a short stack. |
| Warmth | Steam death in District 8 does not heat Allocation 12. Home heater vs who is away. |
| Radiation | Hatch let-in / force-decon / deny (existing constants). Who sleeps by the intake. |
| Morale | Marks, not sermons. A quieter room. A name not said. |
| Health | Illness in the stack. Quiet House vs dying in the bunk. Caretaker present or north. |
| Shelter | Air-filter wear, crowding, contamination. The hatch is a stage. |

### Second Winter (named season, not the spine)

**id:** `season_second_winter` (*PROPOSED* data profile, not a 4th simulation class)

When the profile is active (seeded year-2 winter, or forced after first Ice Road window if Holdfast live):

- Ice Road windows shorten (8–12 days). Beacons still Yara's.
- Plant steam more likely to trip if membrane already wounded (hook `BrineWaterSystem`; do not retune 48h without a ticket).
- Shelter encounter rate up. Everyone is home more often.
- Quest `quest_roster_window` becomes the season's spine beat.

This is how THE SECOND WINTER is Holdfast-scale without being a difficulty pack: it **authors** nights in the hole while the road is thin.

---

# SECTION 2 — REGION-EQUIVALENTS

Not a second coast. Four stages. Visual DNA unchanged: dry-gouache, ash-grey, concrete, rust, terminal amber. The Stack adds **unfaded rectangles, pencil, numbered footboards, a kettle that has a queue.** Too close. Too used.

Travel banding from the hatch:

| Stage-region | `travelHours` | Danger | Signature hazard |
|---|---|---|---|
| The Stack | 0 | 2–5 | Crowding, argument, contamination, the chart |
| The Approach | 0–0.2 | 3–6 | Hatch dilemma, visitors, ash, Garrison checkpoint if `Mutation_TransitTax` |
| The Unlisted Circuit | existing | existing | Powers who want the same names District 8 wants |
| The Overflow | 1.5–3.0 | 4–7 | Empty authenticators, Blank Rows etiquette, 12-B water that is still working |

---

## 2.1 The Stack — *Allocation 12 Interior*

**id prefix:** `loc_stack_*`  
**Visual:** Bolted fittings that do not match the people. Bunks for eleven, manifest for fourteen. Four unfaded rectangles. A crate of boots, sizes 1–4. The chart.  
**Lore:** Provisioned for fourteen. Occupied by whoever was near an unlocked hatch. The chart was left blank because the assignees had names and these people were not them. Writing a name is how a community starts, and how a levy finds a column.  
**Unique mechanic:** `DutyRosterSystem` — morning row, assignments, absences, pencil vs ink.  
**Who you meet:** Kess Adler; Ansel Duth; whoever is not north.

### POIs (6) — all PROPOSED inspectable wings, not a new overworld

| id | Name | Hook |
|---|---|---|
| `loc_stack_roster_wall` | The Chart | Wall-mounted `ALLOCATION 12 — DUTY ROSTER`. Rows blank until `quest_roster_the_chart`. Pencil tin on a string. |
| `loc_stack_sleeping` | The Sleeping Stack | Numbered footboards. Eleven bolted; three camp-pads. Paper tags if fourteenth claimed. |
| `loc_stack_mess` | The Mess | One table. Portion marks on enamel. A ladle that is also a vote. |
| `loc_stack_filtration` | Filtration Stack | The tin of nameplates (*existing* lore `lore_hz_nameplates`). Filter canisters. Who sleeps nearest the intake. |
| `loc_stack_clinic_alcove` | Clinic Alcove | Not a hospital. Iodine, a bolt of cloth, Ianov's arithmetic if he has visited. Empty if Hadi is north. |
| `loc_stack_airlock` | Inner Airlock | Boots crate. Decon shower that is a bucket. The last door before the hatch scene. |

**Map note:** Indoor nodes. Cluster-density of manners, not Cluster geography. Home `Shelter` simulation remains the runtime; these are **authored inspect + encounter stages**.

---

## 2.2 The Approach — *Hatch Account*

**id prefix:** `loc_approach_*`  
**Visual:** Outer hatch. Ash apron. A folding stool that should not be there. Intercom grille with a cracked button.  
**Lore:** Standby cycle held the hatch unlocked at Hour Zero. Everything since has been a decision about who comes in. Edor will wait here. Census escorts will wait here. Quiet House will knock twice. Pell will take a number as if the ash were a bureau.  
**Unique mechanic:** `ShelterEncounterSystem` hatch-trigger. **Reuses** ExpeditionSystem hatch-dilemma constants. Do not retune without Prompt #26.  
**Who you meet:** Tamsin Rook (intercom); Edor Vale (*Holdfast*); Len Quill; Sergeant Pell (*existing*).

### POIs (4) — PROPOSED

| id | Name | Hook |
|---|---|---|
| `loc_approach_hatch` | Outer Hatch | The stage for Sela (*existing*), 12-C reversed (*Holdfast*), and this pack's visitors. Temperature is a fact. |
| `loc_approach_apron` | Ash Apron | Tracks. Folding marks from a stool. Carrion if `CorpseManagementSystem` left a burial outside. |
| `loc_approach_stool` | The Waiting Stool | Edor's. He will not enter uninvited. The waiting is the pressure. If levy refused, the stool is there for forty days. |
| `loc_approach_decon` | Decon Alcove | Strip, bucket, rag. Force-decon spill = 10 rads/h (*existing*). Let-in = 50. Deny = morale 20 on everyone else. |

**Map note:** Zero travel. Always available unless hatch Buried/Frozen (*existing* weather).

---

## 2.3 The Unlisted Circuit — *Sector 4 nodes as quest stages*

**Not new geography.** Existing ids, new **meaning** when the roster is in play. Recast descriptions on implementation flags; do not duplicate District 8.

| id (*existing*) | As a Duty Roster stage |
|---|---|
| `loc_weighbridge` | Edor's first interview; occupations vs wall names |
| `loc_conscription_office` | Pell's numbers; Voss wants the same three the levy wants |
| `loc_the_allotments` | Frayne's brass vs tin vs Leva; stacked demand |
| `loc_grange_hall` | Delacroix vote if you shelter people the Office or Garrison named |
| `loc_alloc_12b` | Halvard's kit; Sela's water memory; Overflow cousin |
| `location_the_memory_vault` | Sole files living names if corroborated |
| `loc_st_brigids_almshouse` | Quiet House door (*recast overlay*; id stays) |
| `loc_dentists_row` | The missing chair is in your airlock |
| `loc_school_gymnasium` | Wren; what you told a child at the mess |
| `loc_ration_queue_plaza` | Who still queues if your people eat by roster |
| `loc_veterinary_surgery` | Ianov if Hadi is gone |
| `loc_cut_waystation_a` | *Holdfast* — bunks A1–A4 are Stack people with different weather |
| `loc_ice_road_gate` | *Holdfast* — column intercept |
| `loc_cluster_clinic` / `loc_cluster_quad` | *Holdfast* — missing-strip, Sela claim, forty rooms |

When `exp_duty_roster_unlocked`, these nodes gain inspect lines and `threateningBodyText` pairs keyed to roster marks — not new travel graphs.

---

## 2.4 The Overflow — *the only new sub-geography*

**id prefix:** `loc_overflow_*`  
**Visual:** Authenticator lights that still believe in numbers. Stairwells with chalk. Holes Continuity numbered and did not fill.  
**Lore:** Allocation 12 was overflow for Cluster 7. 11 and 13 were overflow for 12. The formula made spare holes the way it made spare people. Some of the spares are occupied. The occupants do not write names.  
**Unique mechanic:** Current access `faction_blank_rows` — granted or withdrawn. You cannot conquer them. You can lose the hiding place.  
**Who you meet:** Nila Brant.

This does **not** reopen Sector 4's Power map. It is a Current practice in authenticated voids, mostly under the Grid/Drown seam, 1.5–3h from home. It is not a coast. It is not District 8.

### POIs (4) — PROPOSED

| id | Name | d | hrs | rads | Hook |
|---|---|--:|--:|--:|---|
| `loc_overflow_alloc_11` | Allocation 11 | 4 | 1.5 | 22 | Hatch authenticates. Occupied. Roster wall **kept blank on purpose**. Nila's. |
| `loc_overflow_alloc_13` | Allocation 13 | 5 | 2.0 | 28 | Authenticator lit. Empty. Dust. A chart with one name, erased to paper-scar. |
| `loc_overflow_pump_hatch` | Pump Hatch | 6 | 2.5 | 36 | A hatch on a service riser that authenticates for nobody. Blank Rows cache. |
| `loc_overflow_blank_cellar` | The Blank Cellar | 5 | 2.0 | 24 | Practice room. Pencils in a jar. A rule on the wall: `DO NOT WRITE THE LIVING`. |

**Existing cousin (not new):** `loc_alloc_12b` — fallback designation, chalk marks fourteen / gap / six. Overflow quests **visit** it; they do not replace Halvard's death.

**Map note:** Hub-and-spoke from home, not a district. Four nodes. Stop.

---

## 2.5 Current: The Blank Rows (*not a 7th Power*)

**id:** `faction_blank_rows` (*PROPOSED*; catalog with Currents, **not** `faction_lore.json`)

| Field | Value |
|---|---|
| Alignment | peaceful, conditional |
| Home | Overflow practice; they cross Grid/Drown |
| Wants | silence, pencil, air filters they will not beg for |
| Offers | a hiding place for a named levy; labour without a return |
| Access rule | Write one of their living names on a census, a roster in ink, or 12-C, and access withdraws. They do not retaliate. Allocation 11's hatch simply does not open. |
| Signature | "If it isn't written, it isn't a pool." |

**They are not the Provisioned.** The Provisioned (*existing Current* `faction_the_provisioned`) built private holes and were correct in advance. Blank Rows occupy **Continuity** holes and refuse the formula's second use of them. Do not merge. Do not run `The Knock` as this Current's story; hook it only as a morale-mark if that encounter already fired.

---

# SECTION 3 — MAIN STORYLINE

## Central conflict

**Two correct occupancies.**

The wall chart was printed for fourteen assignees.  
The people sleeping under it have been a community for five years.  
Reconstruction Order 12-C says unallocated occupants of an authenticated facility are a labour reserve.  
Voss says able bodies are service.  
Frayne says brass is fittings, not names.  
Sole says a record that does not include the living is incomplete.  
Nila says a record that includes the living is how the ice finds them.

The player must decide whether the unlisted are a community, a pool, a hiding place, or a blank.

## Theme (unspoken)

Writing a name is an act. Erasing one is also an act. The weather does not care which, and the Office does.

## Principal NPCs (6)

Holdfast NPCs (Edor, Ormund, Yara, Leva, Mire, Sela) remain **theirs**. They appear here as integration, not as a second cast. Pell, Frayne, Sole, Voss, Wren, Ianov, Ivy: *existing*, Circuit stages.

### 1. `npc_kess_adler` — Kess Adler *(companion)*

- **Where:** `loc_stack_roster_wall`
- **Was:** Records clerk, municipal. Reconstruction Utility Rating **9**. Would not have been allocated. She knows.
- **Wants:** A chart that is true in the morning. Pencil, not ink, until someone orders otherwise.
- **Will not:** Write a name that has not slept here. Falsify a date of birth. Enter a Blank Rows name in ink.
- **Voice:** Quiet, present tense, names as rows. Never "we should." She asks who is on today.
- **Snippet:**
  > "The wall was left blank because the names it wanted did not arrive. If I write yours, that is not the same as being them. It is only the same as being here."

### 2. `npc_hadi_morrow` — Hadi Morrow *(companion)*

- **Where:** `loc_stack_clinic_alcove` / `loc_veterinary_surgery` if sent
- **Was:** Veterinary assistant. Occupation Edor will get wrong by one grade. District 8's missing-strip wants a veterinarian. Ianov wants a pair of hands that will not round.
- **Wants:** To finish the people in the stack before anyone schedules the plant.
- **Will not:** Call himself a doctor. Go north without being told, or stay if a child in the stack is septic and he is on a levy.
- **Voice:** Practical. Doses out loud. Does not use the word *fair*.
- **Snippet:**
  > "I can do this here with iodine and a clean rag. I can do the outfall with a whistle. I cannot do both in the same morning. You have to write which morning it is."

### 3. `npc_tamsin_rook` — Tamsin Rook *(companion)*

- **Where:** `loc_approach_hatch` / intercom / night slate
- **Was:** Harbour night-clerk, unlisted. Score would have been in the thirties if anyone had scored a person who worked when others slept.
- **Wants:** A watch that rotates. The hatch not opened by whoever is least tired.
- **Will not:** Sleep the same bunk two nights if the watch is short. Lie on the intercom about who is outside.
- **Voice:** Distances, times, "say again." Dark and lit as facts about the apron, not morals.
- **Snippet:**
  > "There's a stool in the ash. There's a person on it. I'm not opening until you say. I'm also not pretending the stool isn't there."

### 4. `npc_ansel_duth` — Ansel Duth

- **Where:** `loc_stack_mess` / sleeping stack
- **Was:** Parent. Dependent count that would have helped a score, in a household that was not allocated. One child in the stack (use living child survivor if present; else `npc_duth_child` as a named dependent, not a combatant).
- **Wants:** The child not to be a line item. Food that does not require a speech.
- **Will not:** Ask twice whether you told the truth at the table.
- **Voice:** Short. Questions that are not rhetorical.
- **Snippet:**
  > "If you tell them the boots were for someone else, they will still put them on. They will just know."

### 5. `npc_len_quill` — Len Quill

- **Where:** `loc_approach_apron` then `loc_st_brigids_almshouse`
- **Was:** Quiet House runner. Not a medic.
- **Wants:** A name, and one true thing. Blankets if you have them. No sermon.
- **Will not:** Enter the Stack unless invited. Adjudicate the back room. Take a body without the name.
- **Voice:** Four-word answers when he can. The true thing is written exactly as given.
- **Snippet:**
  > "We make it quiet. I need the name. I need one true thing. I will write it the way you say it."

### 6. `npc_nila_brant` — Nila Brant *(companion, Overflow)*

- **Where:** `loc_overflow_alloc_11`
- **Was:** Lamp-oil clerk, unlisted. Occupies Allocation 11 with three others. Keeps their chart blank.
- **Wants:** Your ink not to include them. A filter if you can spare one without a lecture.
- **Will not:** Hide a person whose name is already in Ormund's return. Open 11 if you wrote them.
- **Voice:** Dry. Rules. No poetry about freedom.
- **Snippet:**
  > "You can sleep here if you are not a pool. The minute you are a pool, this hatch is a wall. I will not explain it twice."

**Sela Renn** remains conditional (*existing*). If present, she is a roster row who remembers 12-B. District 8 may still try to claim her. This pack does not age her into a fighter.

## Story beats (10)

| # | Beat | Day / gate | What happens |
|---|---|---|---|
| 1 | **The chart** | Day 60+ or inspect wall | Kess asks whether the morning row may be written. The first political act in the hole. |
| 2 | **The ladle** | After beat 1 | A meal is short, or a levy absence has left extra. Who eats is a mark. |
| 3 | **The fourteenth** | Hatch visitor | Someone wants a bunk the manifest promised. Guest, runner, or Overflow. |
| 4 | **The caretaker named** | Edor *or* missing-strip *or* Pell | Hadi is a trade the formula discarded. Listing him is a world change. |
| 5 | **The column** | Levy window or Pell's quota | Voss and the Office want the same three bodies. The roster is the disputed document. |
| 6 | **The tin** | Filtration inspect | Nameplates. Frayne. Leva. Ink on the wall vs brass in a tin. |
| 7 | **The quiet** | Illness / rad / old injury | Len at the hatch. Die in the bunk or leave with a tag. |
| 8 | **Completeness** | Vault access | Living names to Sole. Two witnesses. Kess can be one if she wrote them. |
| 9 | **The house in the window** | Ice Road open **or** `season_second_winter` / road dark | Home must hold while labour is north — or everyone is home and the stack is too full. |
| 10 | **Ink** | After 8 or 9 | Pencil, ink, erase, burn. The ending writes world state. The hatch reversed reads it. |

## Branching choices (5)

| id | Choice | Immediate | Long |
|---|---|---|---|
| `roster_write_pencil` | Allow Kess to keep pencil | Erasable truth; Edor can still be wrong | Audit risk; Blank Rows tolerate you |
| `roster_write_ink` | Ink the living | Census completes; community visible | 12-C finds a pool; Nila's hatch dark |
| `roster_hadi_hide` | Keep Hadi off returns | Clinic alcove stays; levy list weaker | Cluster strip stays missing; Ianov keeps a hand; plant may not |
| `roster_hadi_send` | Send or list Hadi | Outfall / clinic / levy | Alcove empty; a surgery that does not happen; Holdfast membrane labour exists |
| `roster_quiet_house` vs `roster_die_in_bunk` | Len's terms or not | Name leaves or stays | Quieter room vs a death everyone ate beside |

Silent branches: brass tin; child-truth; hatch deny/decon/let-in (existing magnitudes); Blank Rows access.

## Endings (4 narrative + 1 quiet)

All write a `world_history` second paragraph discoverable at `loc_stack_roster_wall` or `location_the_memory_vault`. The game does not rank them.

| id | Name | Condition | Slide (house voice) |
|---|---|---|---|
| `ending_roster_ink` | **The Chart Holds** | Ink + at least one levy honour **or** Sole completeness | The wall has names. Some of them sleep in Block C. The pencil tin is empty. Edor's return is current. |
| `ending_roster_pencil` | **Morning Row** | Pencil kept; Hadi not listed in ink; Blank Rows access held | The chart is true until someone erases it. The ice still wants a column. The hole is still a hole. |
| `ending_roster_blank` | **Not a Pool** | Erase or never write; Nila access; 12-C starved of names | Allocation 11 stays a wall. Ormund's file is incomplete. Someone still waits on a stool. |
| `ending_roster_burned` | **The Ash Copy** | Chart burned | Sole cannot complete what isn't written. The rectangles on the corridor are still unfaded. A child asks where the wall writing went. |
| `ending_roster_second_winter` | **The House Held** | `season_second_winter` survived with home watch intact, road thin | Overlay, not exclusive. The window was short. The stove was not. Names on the slate matched the people who came back. |

**TrueEnding terraformer / android / neuromancer content is not used.**

## Lore revelations (what standing there teaches)

1. The blank duty roster was not an oversight. Assignees had names. Unlisted occupants were not supposed to need a chart.
2. 12-C's pool is only as real as a written occupancy. Pencil is a delay. Ink is a levy. Blank is a politics.
3. District 8 and Voss want the **same trades** the formula scored below twenty. Refusing one often feeds the other.
4. Brass nameplates and Cluster playground seats are the same metal. Writing the names back onto a wall is the opposite of selling the tin, and both are silence of different kinds.
5. Sole's completeness and Nila's blankness are the same fear, facing opposite ways.
6. A hatch escort reads whatever you left on the wall. If you left ash, they bring a list from somewhere else.

---

# SECTION 4 — QUEST DESIGN

**This is the heart.** Runtime: `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids` (*existing*). Register `quest_roster_*` at implementation. Types: `expedition`, `shelter`, `faction`, `personal`, `repeatable`.

**World-change bar:** every main quest names `mutation_id`, a visible returning-player change, and a Holdfast read-difference. If a row cannot, it is cut.

Hatch-dilemma magnitudes: **do not retune.** Prompt #26 if anyone tries.

---

## 4.1 Main questline (10)

### `quest_roster_the_chart` — The Blank Chart

| Field | Value |
|---|---|
| **Type** | shelter |
| **Prereqs** | Day 60+ **or** `lore_allocation_wrongness` **or** inspect `loc_stack_roster_wall` |
| **Time** | 25–40 min |
| **Synopsis** | Kess Adler stands under a chart headed for fourteen. The rows are blank. She has a pencil on a string. She will not write the first name unless someone says the wall may be used. |
| **Objectives** | 1. Inspect the chart (print date, fourteen rows, pencil tin). 2. Hear Kess's rule (slept-here only). 3. Choose: pencil morning row / leave blank / ask her to wait for ink. 4. Optional: compare three names to Edor's occupations if `quest_holdfast_the_clerk` started. 5. Tell the Stack at the mess, or don't. |
| **Rewards** | `item_roster_pencil` (*PROPOSED*); Kess companion unlock; `knowledge_key: lore_dr_chart` |
| **Complete mutation** | `mutation_roster_in_use` |
| **Fail / refuse** | Leave blank forty days: `mutation_roster_still_blank`. Kess still erases dust. Edor's guesses stay wrong. |
| **Returning player sees** | Wall description recasts: names in pencil, or a cleaner blank. Codex Layer 1 gains a second sentence. |
| **Holdfast reads** | Edor's census occupations match the wall or remain "wrong by one." Levy naming uses wall trades if written. |

**Choice bodies (ids):** `roster_write_pencil` / `roster_leave_blank` / `roster_wait_ink`.

---

### `quest_roster_who_eats` — The Ladle

| Field | Value |
|---|---|
| **Type** | shelter |
| **Prereqs** | Chart quest resolved (any branch) |
| **Time** | 20–35 min |
| **Synopsis** | The mess is short one portion, or levy-absence has left two extra. Ansel's child is at the table. Kess has drawn a line on the enamel. The ladle is the vote. |
| **Objectives** | 1. Count portions vs heads. 2. Choose who goes without, who takes extra, or split (fatigue). 3. Tell the child the truth, a softer sentence, or send them out. 4. Optional: set a standing protocol Kess will write. |
| **Rewards** | `mark_ration_protocol`; mess inspect recast; no alignment score |
| **Complete mutation** | `mutation_ration_protocol` |
| **Fail** | No choice by meal-end: Utility AI feeds the loudest; `mark_ladle_default`. Ansel remembers. |
| **Returning player sees** | Mess description: marks on enamel; a seat left empty if levy honoured; extra bowl if three are north. |
| **Holdfast reads** | Calorie inflow from honouring the levy hits a **protocol**, not a pile. Cluster guest tickets feel like the same ladle if you winter Block C. |

---

### `quest_roster_fourteenth` — The Fourteenth Bunk

| Field | Value |
|---|---|
| **Type** | shelter / hatch |
| **Prereqs** | Chart quest; hatch not sealed |
| **Time** | 35–55 min |
| **Synopsis** | Someone at the outer hatch wants a bunk the manifest provisioned. Variants by flag: allocated runner south (`enc_allocated_runner` *Holdfast*); Overflow from 13; Sela's adult if `alloc12_letter_only`; Fleet ashore if `ending_holdfast_tender`. Manifest says 14. You may already be 14. |
| **Objectives** | 1. Intercom (Tamsin). 2. Inspect apron/stool. 3. Let in / force decon / deny (**existing** rads/morale constants). 4. If in: paper tag on a footboard or refuse the tag. 5. Optional: send them to Allocation 11 if Blank Rows access. |
| **Rewards** | Bunk state; visitor flag; possible companion-lock |
| **Complete mutation** | `mutation_bunk_claimed` **or** `mutation_hatch_guest_denied` |
| **Fail** | Deny and they die in the ash (quiet, forty days, a card or a work ticket): `mutation_fourteenth_in_ash`. |
| **Returning player sees** | Sleeping stack: paper tag, or an empty pad that everyone walks around. Approach: stool occupied or not. |
| **Holdfast reads** | `quest_office_forty_rooms` / `quest_exp_forty_first`: a Sector 4 occupation already on a footboard. Block C paper tags rhyme. Hatch reversed escort has one more or one fewer bed to offer. |

---

### `quest_roster_caretaker` — Named for the Pool

| Field | Value |
|---|---|
| **Type** | faction / personal |
| **Prereqs** | Hadi alive; **one of:** Edor interview, Cluster missing-strip (`quest_office_missing_strip`), Pell numbers, or membrane crisis needing outfall labour |
| **Time** | 40–70 min |
| **Synopsis** | Hadi Morrow is the trade the rubric scored cheap and District 8 cannot desalinate a child without. Listing him completes a return. Hiding him keeps the alcove. Sending him changes two districts. |
| **Objectives** | 1. Hear the claim (Edor / strip / Pell / Leva). 2. Talk to Hadi (he will not self-name as doctor). 3. List / hide / send. 4. Inform Kess (pencil or refusal). 5. Optional: Ianov at `loc_veterinary_surgery`. |
| **Rewards** | `flag_hadi_listed` / `flag_hadi_hidden` / `flag_hadi_sent`; clinic alcove state |
| **Complete mutation** | `mutation_hadi_status` |
| **Fail** | If sent and he does not return (salt-rash, intercept, window close): `mutation_hadi_never_back`. Alcove empty. A rag still on the hook. |
| **Returning player sees** | Clinic alcove description. Cluster Quad missing-strip filled or still hanging. Ianov's waiting room. |
| **Holdfast reads** | Levy names change. Outfall shift exists or does not. `quest_salt_outfall_limit` / membrane 48h labour pool. Sela clinic claim does not replace a vet; it replaces a child. |

---

### `quest_roster_the_column` — The Column

| Field | Value |
|---|---|
| **Type** | expedition / crisis |
| **Prereqs** | Caretaker resolved **or** Holdfast levy issued **or** Pell quota after `Mutation_TransitTax` |
| **Time** | 70–120 min |
| **Synopsis** | Three bodies. The Office has a levy. Voss has a number. Delacroix has a vote if you hide them. The roster is the document everyone is holding. If Ice Road dark, the column tries the Toll instead. |
| **Objectives** | 1. Compare levy names to wall names. 2. Honour as written / substitute / refuse / hide at Allocation 11. 3. Kit and route (Gate *or* weighbridge). 4. Encounter: `enc_garrison_intercept` (*Holdfast*) and/or Pell. 5. Aftermath at the mess (ladle protocol applies). |
| **Rewards** | Hegemony deltas (*existing* pattern); column flags |
| **Complete mutation** | `mutation_levy_column` (*reuse Holdfast proposed id if minted*) + `mutation_column_intercept` or `mutation_column_hidden` |
| **Fail** | Intercept succeeds: people conscripted, not scheduled. `mutation_column_voss`. Ice Road Gate description: Garrison high-vis over Cutter bone-jackets. |
| **Returning player sees** | Gate / weighbridge patrols. Three empty bunks or three Garrison receipts. Highway 9 tax if Iron Ledger also done (stack, don't merge). |
| **Holdfast reads** | Who is available for levy. Whether Edor is still waiting (if refuse + hide, stool empty — he has nothing to wait for). Whether Voss intercepts a column (yes if this mutation). Hatch reversed escort: Garrison-shaped if `alloc12_terms` or Voss took them. |

---

### `quest_roster_the_tin` — The Tin

| Field | Value |
|---|---|
| **Type** | shelter / faction |
| **Prereqs** | Inspect `loc_stack_filtration`; brass demand from Frayne **or** Leva `quest_salt_brass_seats` **or** playground theft |
| **Time** | 40–65 min |
| **Synopsis** | Fourteen brass nameplates in a tin behind the filtration stack. Everyone who finds them puts them back (*existing*). This time someone is buying. Frayne. Leva. The Quad chains. Writing names on the wall is the other use of the same metal. |
| **Objectives** | 1. Open the tin (or refuse). 2. Count plates vs living heads vs fourteen. 3. Keep / sell north / sell to Works / screw one plate back under a living name. 4. Kess will not comment. Ansel might. |
| **Rewards** | `item_tin_fourteenth` (*Holdfast legendary — reuse if minted*) or `item_nameplate_living` (*PROPOSED*); brass count |
| **Complete mutation** | `mutation_brass_kept` / `mutation_brass_north` / `mutation_brass_frayne` / `mutation_plate_on_wall` |
| **Fail** | Stolen by a visitor during a hatch let-in: `mutation_tin_gone`. Nobody mentions it. The rectangles are still unfaded. |
| **Returning player sees** | Tin lighter or present. One plate on the wall if screwed back. Allotments noticeboard. Quad chains. |
| **Holdfast reads** | Leva's valve seats. Playground price. Membrane brass. Frayne's minutes record a shortage or a delivery. `ach_brass_quiet` / `ach_brass_kept` remain Holdfast achievements; this quest **feeds** them. |

---

### `quest_roster_quiet` — Make It Quiet

| Field | Value |
|---|---|
| **Type** | shelter / personal |
| **Prereqs** | A living survivor at unrecoverable health/rad **or** scripted stack illness after Second Winter / membrane fumes |
| **Time** | 30–50 min |
| **Synopsis** | Len Quill at the apron. Two knocks. Name, and one true thing. The back room is not shown. Dying in the bunk is also a choice. The game does not adjudicate Quiet House. |
| **Objectives** | 1. Hear Len. 2. Choose the name (legal / used / refuse). 3. Supply the true thing from **run-true** options (including a lie the House will write anyway). 4. Invite in / keep at hatch / refuse entirely. 5. Aftermath: bunk stripped or a body in the stack. |
| **Rewards** | Personal effects returned later, tagged; or not |
| **Complete mutation** | `mutation_quieter_room` **or** `mutation_death_in_stack` |
| **Fail** | Refuse and they die at the hatch: `mutation_quiet_on_apron`. Carrion system may notice (*existing*). |
| **Returning player sees** | Sleeping stack: a stripped bunk, or a curtain. St Brigid's: a tag with your sentence. |
| **Holdfast reads** | Hatch reversed: an empty bunk to offer, or a name already gone from occupancy. Sela clinic: if the dead was her bunk-neighbour, her claim scene has a quieter room behind her. Levy: one fewer body. |

---

### `quest_roster_sole` — Say the Name

| Field | Value |
|---|---|
| **Type** | expedition |
| **Prereqs** | Vault / Archivists access (`lore_bs_the_vault_holds` or boat); chart not burned |
| **Time** | 60–90 min |
| **Synopsis** | Sole will not enter a living name on one testimony. Kess can be the second witness if she wrote the roster. Nila will not corroborate a name she is hiding. Completeness vs blankness. |
| **Objectives** | 1. Copy the living list (pencil rubbing or ink copy). 2. Reach `location_the_memory_vault`. 3. Corroborate or fail the rule. 4. Optional: show 12-C if owned (`item_order_12c` *Holdfast*). Sole files, does not sign. 5. Return. Kess asks what was said aloud. |
| **Rewards** | `knowledge_key: lore_dr_living_schedule`; `item_sole_living_copy` (*PROPOSED*) |
| **Complete mutation** | `mutation_schedule_living` **or** `mutation_schedule_refused` |
| **Fail** | One testimony only: names not entered. The rule is the point of the rule. `mutation_uncorroborated`. |
| **Returning player sees** | Vault inspect: living unlisted in a different ink. Roster wall: a check-mark that is not a score. |
| **Holdfast reads** | Ormund's drawer can show occupancy that is current. 12-C in the Drown lists your people or does not. Unifier levy-treaty: Office signs a pool that exists on paper, or cannot. |

---

### `quest_roster_window` — While the Road Is Open

| Field | Value |
|---|---|
| **Type** | shelter / expedition |
| **Prereqs** | First Ice Road window **or** `season_second_winter` **or** Ice Road dark (closed-road variant) |
| **Time** | 90–150 min (the window is the clock) |
| **Synopsis** | If the road is open: haulers leave; the house must hold — watches, meals, hatch for returnees (decon vs morale). If the road is dark / Second Winter: everyone is home; steam may die north; arguments; illness; a visitor who cannot leave. Quest-first, not a weather DLC. |
| **Objectives** | 1. Assign home watch vs waystation vs haul (DutyRoster). 2. Survive N nights of `ShelterEncounterSystem`. 3. Resolve at least one returning-hatch dilemma with **existing** constants. 4. Keep stove / filter / child. 5. Optional: send Tamsin to Waystation A (Holdfast watch quality). |
| **Rewards** | `flag_home_held` / `flag_home_failed`; waystation credit |
| **Complete mutation** | `mutation_home_watch` |
| **Fail** | Filter death, or a returnee denied until morale break, or waystation stove out: `mutation_house_thinned`. A name missing. Repeatable watches locked for a window. |
| **Returning player sees** | Night slate. Filter notches. Waystation A1 empty if Tamsin stayed home. Accident book if a haul went out under-watched. |
| **Holdfast reads** | Repeatable `quest_rep_ice_window_haul` / `quest_rep_steam_watch` succeed or fail from **home labour**. Lamps: Ivy's oil vs Yara's if you stripped the house. Membrane 48h: no outfall bodies if they are on night slate. |

---

### `quest_roster_ink` — Ink

| Field | Value |
|---|---|
| **Type** | shelter / story |
| **Prereqs** | Sole quest done **or** window quest done **or** 12-C live **or** Day 200 claim |
| **Time** | 35–60 min |
| **Synopsis** | Kess will not choose ink. Nila will leave if you ink her people. Edor will complete a return if you ink yours. Burning is a community decision that looks like a kettle accident if you lie about it. The hatch reversed will read whatever remains. |
| **Objectives** | 1. Hear Kess, Nila (if access), Ansel, Tamsin. 2. Choose: ink / keep pencil / erase all / burn. 3. If burn: tell the child the truth or a fire story. 4. Write nothing on the night slate, or write. 5. Wait for the next hatch (Sela and/or Office escort). |
| **Rewards** | Ending flag; history second paragraph; optional `victory_the_duty_roster` slide |
| **Complete mutation** | `mutation_roster_ink` / `mutation_roster_pencil` / `mutation_roster_blank` / `mutation_roster_burned` |
| **Fail** | No choice when escort arrives: they read a blank and bring **their** list. `mutation_roster_read_by_others`. |
| **Returning player sees** | The wall: ink, pencil, scar, ash. Codex. Occupancy. |
| **Holdfast reads** | Hatch reversed escort list. Edor waiting or not. Levy availability forever. `ending_holdfast_schedule` slide's roster sentence becomes this wall, specifically. Foghorn / Block C nameplates if inked people went north. |

---

**Main quest total player time:** ~8–12 hours including needs management and Circuit travel, not including side catalog.

---

## 4.2 Side quests (18)

### Faction / Current (6)

| id | Type | Giver | Location | Hook | Objectives | Rewards / mutation |
|---|---|---|---|---|---|---|
| `quest_roster_pell_numbers` | faction | Sergeant Pell (*existing*) | `loc_conscription_office` | His quota is three. They are your three if the levy named them, or the next three trades on the wall. He will not lie. | 1. Take a number. 2. Hear what happens to decliners. 3. Meet quota with volunteers / refuse / send substitutes. | Garrison trust; `mark_pell_honest`; Voss intercept more/less likely |
| `quest_roster_frayne_minutes` | faction | Ottilie Frayne (*existing*) | `loc_the_allotments` | Minutes: brass fittings. She will not ask if they were nameplates. | 1. Deliver 8 `brass_fittings` (*existing*) or none. 2. Sit the committee. 3. Do not comment. | Works hegemony; water clock; stacks with Leva |
| `quest_roster_grange_vote` | faction | Delacroix (*existing*) | `loc_grange_hall` | A show of hands: shelter people the Office or Garrison named. Your hand is visible. | 1. Attend. 2. Vote. 3. Live with the room. | Militia trust; Lasko-shaped risk; `mark_hand_visible` |
| `quest_roster_ivy_oil` | Current | Ivy Corrigan (*existing*) | Kilometre 19 / home lamps | House lamps vs Ice Road oil. She will not go dark for you. | 1. Carry oil. 2. Do not ask the exception. 3. Receipt. | Lamplighter access held or eleven-day dark (*existing rule*) |
| `quest_roster_blank_access` | Current | Nila Brant | `loc_overflow_alloc_11` | A filter for a hiding place. No please. | 1. Visit 11. 2. Trade filter or refuse. 3. Hear the rule. | `faction_blank_rows` access; `The Knock` not duplicated |
| `quest_roster_missing_strip` | faction | Quad noticeboard *or* Kess copy | home / `loc_cluster_quad` | A living name on District 8's missing trades. | 1. Match. 2. Tell them or don't. 3. If told, retrieval file. | Feeds Holdfast `quest_office_missing_strip`; morale mark |

### Companion (6)

| id | Giver | Hook | Objectives | Mark |
|---|---|---|---|---|
| `quest_roster_kess_pencil` | Kess | Her own DOB is written twice in a salvaged clerk-book. Convoy 12 grammar. | 1. Compare. 2. Do not joke. 3. Let her correct it or leave it. | She will erase one levy name for you if you leave the error — and hate it. Cousin to Edor's DOB quest, **not a copy**: hers is municipal, his is allocated. |
| `quest_roster_hadi_shift` | Hadi | Ianov vs outfall vs alcove. One morning. | 1. Assign the morning. 2. Live with the surgery that waits. | Clinic vs plant vs Verge |
| `quest_roster_tamsin_watch` | Tamsin | Night slate has the same name three times. | 1. Rotate or pay fatigue. 2. Optional: send her to Waystation A. | Waystation quality; hatch intercom tired-voice variant |
| `quest_roster_ansel_truth` | Ansel | The child asks what the boots were for. | 1. Truth / soft / send out. 2. Wren if present hears a version. | `mark_child_truth`; Cluster school if they go north |
| `quest_roster_len_tag` | Len | Effects come back. The true thing is on the tag. Someone in the Stack reads it. | 1. Leave the tag in the alcove or burn it. | Prose variant forever |
| `quest_roster_nila_eleven` | Nila | Allocation 13 has one erased name. She wants it to stay a scar. | 1. Visit 13. 2. Do not rewrite. 3. Optional: copy the scar for Sole (this **breaks** access). | Blank Rows or completeness, not both |

### Exploration (3)

| id | Location | Hook | Objectives | Mutation / loot |
|---|---|---|---|---|
| `quest_roster_chair` | `loc_dentists_row` (*existing*) | The fourth practice is missing a chair. Yours is bolted in the airlock. | 1. Confirm the bolts. 2. Return the chair or leave the hole. 3. Kess will not write DENTIST unless a dentist sleeps here. | `mutation_chair_returned` — Dentists' Row description recasts; Layer 1 dental chair gone |
| `quest_roster_12b_kit` | `loc_alloc_12b` (*existing*) | Halvard's improvised potable. Sela: engineering, not salvage. | 1. Photograph/copy notes. 2. Leave the working kit or take it. 3. If Sela present, she stays or leaves the room. | Feeds `item_halvard_kit_notes` (*Holdfast*). 12-B water still works only if left. |
| `quest_roster_brigid` | `loc_st_brigids_almshouse` (*existing*) | Charts filled to a date, then not. Quiet House overlay. | 1. Walk the ward. 2. Do not enter the back room. 3. Leave blankets or don't. | Recast description; Len's trust; no adjudication |

### Shelter (1) + Repeatable (2)

| id | Type | Hook | Objectives | Loop |
|---|---|---|---|---|
| `quest_roster_boot_crate` | shelter | Crate of boots 1–4, never opened (*existing* Layer 1). A child can wear a pair. They were not packed for them. | 1. Open or leave sealed. 2. If open: fit / keep for "arrivals" / send north to Cluster school. | `mutation_boots_opened`; Cluster forty-rooms rhyme; warmth item if taken |
| `quest_rep_night_slate` | repeatable | Each night Tamsin posts a slate. Assign 1 watch or Utility AI defaults. | 1. Assign. 2. Encounter check. | Fatigue; hatch readiness; Second Winter frequency up |
| `quest_rep_meal_row` | repeatable | Kess copies the ladle protocol. | 1. Confirm portions. 2. Exception for sick / child / levy-return. | Ration protocol reinforcement; no gold |

---

## 4.3 Morale micro-choices catalog (26)

Each is **1–2 sentences of situation**, **2–3 options**, **flag + one sentence of later evidence**. Not `Morale +2`. Not an alignment meter.

| id | Situation | Options | Mark later |
|---|---|---|---|
| `mmc_extra_portion` | One bowl left. Two people looking at it. | Give to child / to the person going to the hatch / leave it on the table until it is cold. | `mark_bowl_cold`: the enamel has a ring nobody scrubs. |
| `mmc_who_hatch` | Expedition returning, glowing. Tamsin on intercom. | Let in / force decon / deny. | Existing contamination/morale numbers. Later: a rag that still ticks, or a stool with two people. |
| `mmc_child_boots` | Child puts on size-2 boots from the crate. | Let them / take them off / say they are borrowed. | `mark_boots_on`: they sleep in them. Cluster school notices northern rubber. |
| `mmc_name_on_wall` | Someone writes a nickname, not a legal name. | Leave it / Kess corrects / erase. | Edor's return has a nickname. Ormund notes irregular. |
| `mmc_tell_child_levy` | Three people packing for thirty days. | Truth (north, forms) / "work trip" / send child to sleeping stack. | `mark_child_levy_story`: they wait at the hatch on day 30 or they don't. |
| `mmc_sela_row` | If Sela present: Kess asks whether she is a row or a guest. | Row / guest / let Sela say. | Clinic claim later uses your word. |
| `mmc_edor_tea` | Edor on the stool. Kettle inside. | Carry a cup out / invite in / ignore. | He will mention the cup in the return, or the silence. |
| `mmc_pell_number` | Ticket machine in the ash (he brought it). | Take a number / refuse the machine / break it. | Conscription office: your number is already on a spike, or the machine is missing a gear. |
| `mmc_night_same_bunk` | Tamsin asked not to sleep the same bunk twice. Only one empty. | Rotate someone else / make her / leave a pad in the airlock. | `mark_tamsin_double`: her intercom is slower. |
| `mmc_filter_who` | Filter failing. Who sleeps by the intake. | Sick / child / volunteer / draw lots (Kess writes the lot). | Intake bunk description; Cult shrine mutation stacks if present. |
| `mmc_true_thing_lie` | Len accepts a lie. | Lie / true / refuse the sentence. | Tag comes back with the lie. Someone who knew them reads it. |
| `mmc_wren_object` | Wren trades for an explanation of the roster pencil. | True / joke / "don't know." | Wren's only version. Cluster homework if she ever sits that school. |
| `mmc_frayne_comment` | A survivor says *fair* in the Allotments. | Silence / agree / shut them down. | Frayne's minutes: a visitor spoke. She does not record the word. |
| `mmc_brass_one_plate` | Screw one plate under a living name. | Do / don't / put it back in the tin after a night. | Wall has a name that catches lamp-light. Nobody mentions the tin. |
| `mmc_empty_bunk_sheet` | Levy honoured. Make the bunk or leave the tangle. | Make it / leave it / let a child keep a sock on the board. | Mess: extra portion protocol. Stack: a sock or a taut blanket. |
| `mmc_intercom_lie` | Visitor asks if Hadi is inside. | Yes / no / "we don't give names." | Retrieval file. Nila hears if you said yes. |
| `mmc_second_helping_hadi` | Hadi skipped a meal to finish a dressing. | Make him eat / leave it / child delivers the bowl. | He will go north fed or not. Outfall shift fatigue. |
| `mmc_alloc13_rewrite` | Player can rewrite the erased name at 13. | Rewrite / leave scar / copy for Sole. | Access lost if written. Vault has a scar-copy. |
| `mmc_waystation_letter` | Paper tag on A3: a home name. | Bring it home / leave it / burn. | Waystation description. Home footboard missing a tag. |
| `mmc_membrane_iodine` | Last iodine: thyroid child vs process water vs outfall. | Home / north / split. | Clinic vs plant vs salt-rash. Holdfast iodine store notices. |
| `mmc_voss_receipt` | Garrison receipt for three names. Pin it by the chart or not. | Pin / hide / burn. | Pell sees it next visit. Kess will not take it down if you pin it. |
| `mmc_quiet_blanket` | Len asks for a blanket. It is the child's. | Give / refuse / cut it in half (both worse). | Tag includes "a half blanket" if you cut. Child sleeps cold. |
| `mmc_burn_story` | If chart burned: child asks. | Fire in the kettle / we did it / silence. | `mark_burn_story`. They repeat it at the Quad if they go. |
| `mmc_sole_aloud` | Sole: say the name while you write it. | Say / whisper / refuse. | Archivists access. Kess asks which you chose. |
| `mmc_deny_forty` | Denied visitor. Forty days. Find a bag in the ash. | Bring in the bag / bury / leave. | Card in the bunker or a buried rectangle of earth the crows know. |
| `mmc_lamp_oil_cup` | Last cup of oil: house lamp vs Ivy's can vs Yara's stick. | Home / Ivy / Yara. | Eleven-day dark in a region, or a dark Cut segment, or a dark mess. |

---

## 4.4 Shelter encounter table (14)

Reuse hatch constants. Do not retune.

| id | Trigger | Beats | Morale mark | Mutation / flag |
|---|---|---|---|---|
| `se_night_slate` | night | Tamsin posts names. Argument if same name twice. Assign or default. | Tired voice on intercom | Watch quality |
| `se_hatch_return` | hatch + expedition AtHatchDilemma | Inspect glow. Let in / decon / deny. | Existing ±morale/rads | Contamination on `Shelter` |
| `se_meal_short` | meal + protocol | Ladle. Child present. | Bowl ring | Ration protocol stress |
| `se_intake_sleep` | radiation / filter tick | Who sleeps by intake. Lots or order. | Cough in the morning | Stack inspect recast |
| `se_levy_absence` | levy active | Empty bunks. Extra food. Sock on a board. | Quieter mess | `mark_three_away` |
| `se_ice_pack` | Ice Road window opens | Who goes. Kit. Tamsin stays or not. | Packing silence | Home labour down |
| `se_edor_stool` | visitor + clerk started | Stool. Form. Optional tea. | Cup or silence | Census progress |
| `se_pell_machine` | visitor + Garrison | Ticket machine in ash. Numbers. | Hand visible to nobody yet | Intercept risk |
| `se_stack_fever` | illness / Second Winter | Two bunks. Hadi present or not. Separate or not. | Fear as procedure | Health; Quiet House offer |
| `se_child_chart` | child + chart in use | Child copies a name wrong. | Nickname mark | Edor irregular |
| `se_tin_again` | inspect filtration after tin quest | Someone put a plate back. Or didn't. | Silence | Brass state confirm |
| `se_intercom_office` | 12-C live | Escort language through grille. Threatening pair. | Temperature named | Hatch reversed progress |
| `se_road_dark_crowd` | Ice Road dark **or** window closed | Too many bodies. Arguments. Visitor who cannot leave. | Crowding | Second Winter content |
| `se_sela_row` | Sela present + Kess morning | Guest vs row. She may speak. | Her sentence | Clinic claim wording |

---

# SECTION 5 — SYSTEMS (max 4)

**Cap:** 3 new plain-C# systems + 1 data SeasonProfile. No LLM. Event-raising. Save-safe. Host-callback injection like `ShelterDegradationSystem`. **Do not rebuild** IceRoad, BrineWater, CensusClaim, Waystation, WorldStateConsequence — **hook** them.

Hatch constants: **untouched**.

Cross-tool QA: roster assignment × needs tick × levy availability is **three coupled variables**. Implementer ≠ reviewer (Prompt #26). Reviewer sees diff + this spec only.

---

## 5.1 `DutyRosterSystem`

**id:** `duty_roster_system`  
**What it is:** The chart as save-safe occupancy. Not a job minigame. A document that other systems read.

**Mechanics:**
- Rows ≤ 14. Fields: `survivorId`, `displayName`, `occupationObserved`, `status` (`home` / `levy` / `waystation` / `quiet` / `missing` / `dead`), `script` (`pencil` / `ink` / `blank`), `lastSleptDay`.
- Morning tick: Kess fills pencil if allowed. Ink never auto-fills.
- Assignments: night watch, mess, hatch opener, intake sleeper, expedition. Utility AI scores if player skips.
- Levy / CensusClaim: named IDs **must** exist as rows if `mutation_roster_in_use` and script ≠ blank. Hide flags omit from copies sent north.
- Events: `OnRosterUpdated`, `OnNameWritten`, `OnNameErased`, `OnRosterBurned`, `OnAssignmentChanged`.

**UI/UX:** Diegetic chart in Lore Codex + wall inspect. No green "community score."

**Balance:** Cannot assign the dead. Cannot write Blank Rows names without breaking access. Fourteen is a hard cap (manifest). Over-occupancy is the fourteenth-bunk quest, not a UI cheat.

**Integration:** `NeedsSystem` (who is home to eat/sleep); `CensusClaimSystem.LevyOrder`; `WaystationSystem` bunks; `WorldStateConsequenceSystem` mutations listed in §4.

---

## 5.2 `ShelterEncounterSystem`

**id:** `shelter_encounter_system`  
**What it is:** The bunker as a stage. Timed, flagged, save-safe scenes. Not a procedural chatterbox.

**Mechanics:**
- Trigger table §4.4. Cooldowns. Seed `_worldSeed + 1208`.
- Hatch-return **bridges** existing `OnHatchDilemmaReady` — does not replace it, does not retune it.
- Visitor queue: one at a time on the stool/apron (Edor, Len, Pell, Office, Overflow).
- Ice Road open/closed and `season_second_winter` change weights, not a new weather sim.
- Events: `OnShelterEncounterStarted`, `OnShelterEncounterResolved`.

**UI/UX:** Event modal + inspect. Intercom lines as radio text. `threateningBodyText` when Office/Garrison trust low.

**Balance:** Max one encounter per night unless crisis (`quest_roster_window`). Do not starve expeditions.

**Integration:** `ExpeditionSystem` hatch phase; `NeedsSystem` morale as **result**, marks as **flags**; Holdfast window calendar.

---

## 5.3 `MoraleMarkSystem`

**id:** `morale_mark_system`  
**What it is:** Small, frequent, diegetic consequences. A flag + a later sentence. Not a second morale meter.

**Mechanics:**
- Store `HashSet<string>` marks + optional `string payload` (the true thing, the lie, the child's version).
- Queries: `HasMark`, `GetPayload`. Prose tables in JSON (`duty_roster_marks.json`).
- NeedsSystem morale deltas may **accompany** a mark (existing event `moraleDelta`) but the mark is the content.
- Events: `OnMarkSet`, `OnMarkCleared` (rare; burning a tag).

**UI/UX:** Player sees evidence in inspect/NPC barks, not a checklist of sins.

**Balance:** Marks do not expire except by authored quest (tag burned). Do not clear on sleep.

**Integration:** `threateningBodyText`; location `description` overlays; Holdfast stage variants; Quiet House tags.

---

## 5.4 SeasonProfile `season_second_winter` (data, not a class)

Consumed by `IceRoadSystem` + `ShelterEncounterSystem` + heater/filter ticks.

| Knob | Effect |
|---|---|
| Window length | 8–12 days |
| Encounter weight | ×1.6 at home |
| Steam trip chance | + if membrane wounded (Holdfast) |
| Travel | Cut still Yara's; dark still dark |

**Unrealistic (do not build):** a new climate sim, snow physics, a separate executable.

---

## Systems explicitly not in this expansion

- No 4th Holdfast travel system.
- No seventh Power in `faction_lore.json`.
- No livestock, no SubBay requirement.
- No FalloutForecast.
- No companion combat AI.

---

# SECTION 6 — CHARACTERS & ENCOUNTERS

## 6.1 Companions (assignable labour, not a party)

| id | Name | AI bias | Will not | If they die / leave |
|---|---|---|---|---|
| `npc_kess_adler` | Kess Adler | Write, erase, refuse ink | Write unslept names; joke DOB | Chart goes to player-only; errors multiply on Edor's return |
| `npc_hadi_morrow` | Hadi Morrow | Dress, refuse title "doctor" | Leave a septic child for a form | Alcove empty; membrane labour missing; Ianov alone |
| `npc_tamsin_rook` | Tamsin Rook | Watch, intercom truth | Lie about the apron | Hatch scenes unnarrated; waystation worse |
| `npc_nila_brant` | Nila Brant | Hide, withdraw | Hide an already-listed name | Allocation 11 dark; no Overflow |

Ansel and Len are **not** expedition companions. Sela is conditional fifth (*existing*).

Utility AI actions (*PROPOSED*): `Action_RosterWrite`, `Action_NightWatch`, `Action_HatchIntercom`, `Action_ClinicAlcove`. Seed `_worldSeed + 1208`.

## 6.2 Encounter variants (10)

Human danger = people in conditions. No fantasy. Combat = existing expedition resolution.

| id | Name | Where | Cost | Notes |
|---|---|---|---|---|
| `enc_stool_clerk` | Clerk on a stool | Approach | Time, names | Edor. Not a fight. |
| `enc_census_escort` | Census escort | Approach / Cut | Ammo or time | *Holdfast* reuse |
| `enc_pell_quota` | Decent conscriptor | Approach / Grid | People | Pell. Honest. Worse. |
| `enc_quiet_knock` | Two knocks | Apron | A name | Len |
| `enc_overflow_dark` | Hatch that will not open | Alloc 11 | Access | After ink betrayal |
| `enc_garrison_intercept` | Northern patrol | Gate / Toll | Hegemony | *Holdfast* reuse |
| `enc_allocated_runner` | Running south | Approach | Bunk | *Holdfast* reuse |
| `enc_stack_argument` | Mess argument | Stack | Morale mark | No HP bar |
| `enc_returnee_glow` | AtHatchDilemma | Hatch | Existing constants | Bridge, don't clone |
| `enc_blank_cache` | Pump hatch | Overflow | Filter, silence | Not Undertow grammar |

## 6.3 Crises (5) — multi-phase, not arenas

| id | Name | Phases | Failure | Success looks like |
|---|---|---|---|---|
| `crisis_the_chart` | Occupancy | Blank → pencil → ink/erase/burn | Escort reads a foreign list | A wall you can live with |
| `crisis_the_ladle` | Portions | Count → child → protocol | Default AI; Ansel's silence | Marks on enamel |
| `crisis_the_column` | Three bodies | Paper → route → intercept → mess | Voss has them | Names still yours or honestly gone |
| `crisis_the_quiet` | One person | Knock → true thing → bunk | Death on apron | A tag or a curtain you chose |
| `crisis_the_window` | House | Assign → nights → hatch return | Thinned house | Slate matches returns |

Ormund / Voss / Pell are not final bosses. Killing Pell is possible, costly, and replaces him with a less decent clerk. Do not make that a win.

---

# SECTION 7 — ITEMS & REWARDS

Existing tools remain canonical. All new item ids **PROPOSED** except noted.

## 7.1 Sets (5)

| Set id | Pieces | Function |
|---|---|---|
| `set_roster_paper` | `item_roster_pencil`, `item_roster_ink_stick`, `item_chart_rubbing`, `item_night_slate` | Quest keys; visible in Stack |
| `set_hatch_account` | `item_intercom_key`, `item_stool_fold`, `item_decon_rag` | Approach; rag may tick |
| `set_overflow` | `item_alloc11_token` (blank disc), `item_erased_scar_copy` | Access; completeness risk |
| `set_quiet_tag` | `item_true_thing_tag`, `item_returned_effects` | Payload is the sentence |
| `set_two_district_labour` | reuse Holdfast census/12-C; `item_levy_copy_home` | Home carbon of a northern form |

## 7.2 Unique objects (8) — one in the world, with a history

| id | Name | Where | What it does | First line |
|---|---|---|---|---|
| `item_roster_pencil` | String Pencil | Chart | Allows morning row | The string is greasy. The point is short. |
| `item_chart_burned_edge` | A Charred Header | After burn | Ending key | `ALLOCATION 12 — DUTY` and then nothing. |
| `item_nameplate_living` | One Plate, Used | Tin quest | Wall catch-light | It has a name it was not cast with. |
| `item_sole_living_copy` | Living Occupancy | Vault | Codex; 12-C overlay | Said aloud. Written. Different ink. |
| `item_duth_boot_left` | Size 2, Left | Crate | Warmth; mark | The pair is broken. The child knows. |
| `item_hadi_rag` | Clinic Rag | Alcove | Present if he is not | Still damp. Nobody boils it. |
| `item_nila_disc` | Unnumbered | Alloc 11 | Access token | It authenticates nothing. That is the point. |
| `item_edor_cup` | Tin Cup, Returned | Stool | If you carried tea | He washed it in the ash. There is grit in the seam. |

Reuse Holdfast legendaries when flags say they exist: `item_order_12c`, `item_tin_fourteenth`, `item_halvard_kit_notes`, `item_playground_seat`.

## 7.3 Achievements (16)

`ach_dr_*`. No kill-counts. No jokes that break tone.

| id | Name | Condition |
|---|---|---|
| `ach_dr_chart` | Morning Row | Write the first name |
| `ach_dr_blank` | Still Blank | Forty days without a name |
| `ach_dr_ladle` | Enamel | Set a ration protocol |
| `ach_dr_fourteenth` | Paper Tag | Claim or deny the fourteenth |
| `ach_dr_hadi_hide` | Not a Pool | Hide Hadi through a levy |
| `ach_dr_hadi_send` | Outfall | Send Hadi; he returns |
| `ach_dr_hadi_gone` | The Rag | He does not |
| `ach_dr_column` | Three | Resolve the column without Voss taking them |
| `ach_dr_tin` | Behind the Stack | Open the tin |
| `ach_dr_quiet` | One True Thing | Complete Quiet House terms |
| `ach_dr_sole` | Aloud | Corroborate living names |
| `ach_dr_window` | Slate | Hold the house through a window |
| `ach_dr_ink` | Ink | Ending ink |
| `ach_dr_burn` | Header | Burn the chart |
| `ach_dr_eleven` | Wall | Keep Blank Rows access to Ink |
| `ach_dr_hatch` | The Account | Finish Ink and a hatch scene in the same week |

## 7.4 Narrative word-count estimate

| Bucket | Words | Notes |
|---|---|---|
| Main quest stage/choice | 10,000 | Full UI text in creative pack |
| Side | 6,500 | 18 × ~350 |
| Morale micro-choices | 2,500 | Diegetic lines |
| Shelter encounters | 5,000 | Playable scenes |
| NPC bibles | 3,000 | 6 × barks + monologue |
| Location cards (new wings) | 2,000 | Stack / Approach / Overflow |
| Endings + radio/intercom | 1,500 | |
| **Creative pack target** | **~22,000–26,000** | Quest-weighted vs Holdfast's location-weighted pack |

---

# SECTION 8 — TECHNICAL IMPLEMENTATION PLAN

## 8.1 Architecture mapping

| Concern | Existing pattern | Duty Roster |
|---|---|---|
| Data | StreamingAssets JSON + DTOs | `duty_roster_quests.json`, `duty_roster_marks.json`, `duty_roster_encounters.json`, append `currents.json` (`faction_blank_rows`), location overlays for stack/approach/overflow |
| Logic | Plain C#, events, save blobs | `DutyRosterSystem`, `ShelterEncounterSystem`, `MoraleMarkSystem` |
| Host | `GameBootstrap` partials | `GameBootstrap.DutyRoster.cs` |
| AI | UtilityAI | New survivor actions; no LLM |
| UI | UITK, Lore Codex, event modal | Chart document; wing inspect; intercom |
| Map | GeneratedMap nodes | Overflow 4 nodes; wings as indoor inspect (may be location ids without travelHours) |
| Lore | `LoreDiscoveryIndex` | `lore_dr_*` |
| Quests | `QuestlineSO.Ids` | Register all `quest_roster_*` |
| Consequences | `WorldStateConsequenceSystem` | New mutations; **do not** add Office/Blank Rows to `_hegemony` |
| Hatch | `ExpeditionSystem` constants | Bridge only |

**Ids namespace:** `loc_stack_*`, `loc_approach_*`, `loc_overflow_*`, `faction_blank_rows`, `npc_*` listed, `quest_roster_*`, `mmc_*`, `se_*`, `mark_*`, `flag_*`, `lore_dr_*`, `mutation_*`, `ending_roster_*`, `season_second_winter`.

## 8.2 Assets (specify only; generate later into `generated_AIassets/`)

Dry-gouache, isolated objects, no readable AI text, no flags, no gore, no fantasy glow.

| Asset | Type | Notes |
|---|---|---|
| Wing cards × ~14 | 2D | Chart, mess enamel, tin, stool, Alloc 11 hatch |
| NPC portraits × 6 | Chest-up, deferred | Kess, Hadi, Tamsin, Ansel, Len, Nila |
| Items × ~20 icons | 64–128 px | Pencil on string, charred header, blank disc |
| Chart UI | UITK | Fourteen rows, pencil/ink states |
| Intercom | Text + optional audio | Hatch grille. Text fallback mandatory |
| **Not in scope** | 3D bunker, full VO, new music album | |

## 8.3 Sprints (4 × 3 weeks)

| Sprint | Goal | Deliverables | Verify |
|---|---|---|---|
| **S1 — Chart & marks** | Wall works | `DutyRosterSystem` + `MoraleMarkSystem`; stack wings; quests chart/ladle; Kess + Ansel; JSON | Save roundtrip; 14-cap; compile PASS |
| **S2 — Hatch account** | Approach works | `ShelterEncounterSystem`; hatch bridge (no retune); fourteenth; Tamsin; Len; Pell encounter | Hatch constants unchanged; compile PASS |
| **S3 — Circuit & Overflow** | World changes | Hadi; column; tin; Nila; 11/13; Sole corroboration; mutations into WorldState | Levy names = rows; Blank Rows withdraw; compile PASS |
| **S4 — Window & ink** | Endings work | `season_second_winter` data; window quest; ink endings; Holdfast two-way flags; 8 side quests | Ending exclusive flags; compile PASS; PlayMode: one window at home |

**QA:** home needs tick while north; filters degrade; no 7th `faction_lore` row; hatch magnitudes logged unchanged.

## 8.4 Risks

| Risk | Mitigation |
|---|---|
| Feels like a morale DLC | World mutations mandatory on all 10 mains |
| Duplicates Holdfast census | Home is occupancy; District 8 is destination. Shared flags, different rooms |
| Duplicates Quiet House story | Do not adjudicate the back room. Len is a runner. St Brigid's overlay only |
| Duplicates The Knock | Filter-at-home is internal; Provisioned stay their Current |
| Duplicates Edor DOB | Kess DOB is municipal clerk-book, not allocated return |
| Hatch retune temptation | Spec forbids. Prompt #26 |
| Overflow becomes a district | Cap 4 nodes |
| Child NPC combat | Dependents are not a party |

## 8.5 QA cases (minimum)

1. Old save → blank chart → pencil → Edor occupations update  
2. Levy honour → three `status=levy` → mess extra → day 30 return hatch dilemma  
3. Hide Hadi → Cluster strip still missing → Nila will hide him → ink of Hadi breaks 11  
4. Voss intercept vs Office levy — same three names, two receipts  
5. Tin sold north → Frayne **and** Leva shortage; wall still blank of plates  
6. Quiet House lie written on tag → Stack reads it  
7. Sole one-witness fail — no entry  
8. Ice Road dark → `se_road_dark_crowd` → window quest closed-road variant  
9. Burn chart → hatch escort brings foreign list  
10. Hatch let-in still +50 rads/h; deny still −20 morale others  
11. Compile + EditMode PASS  

---

# SECTION 9 — PLAYER ENGAGEMENT & RETENTION

## Day-one (post-unlock)

- The wall. A pencil on a string. Kess asking a question that is not "how do you feel."
- First meal after a name is written. The ladle.
- First stool in the ash if Holdfast clerk has started — or Tamsin saying there is a stool even if Edor has not arrived yet (Overflow visitor / Len).

## 3–6 month roadmap (after S4)

| Month | Content | Why they return |
|---|---|---|
| M1 | Remaining sides; intercom pack; more marks | Nights in the hole are the loop |
| M2 | Long Walk visits the Approach (one night only — they will not stay a second) | *Existing* Current; news of both districts |
| M3 | Second Winter as a repeatable SeasonProfile | Calendar, not battle-pass |
| M4–6 | Shareable: chart screenshots (their living names), Quiet House tags (their sentence). No live service | Occupancy is personal |

## Monetization

Same as Holdfast: no iodine microtransaction, no gacha. If paid DLC: one purchase with Holdfast or after.

## Feedback loops

| Loop | Need served |
|---|---|
| Morning row | Identity, levy, census |
| Ladle | Hunger, morale marks |
| Hatch account | Radiation vs morale (existing numbers) |
| Column | Hegemony, Ice Road, Voss |
| Quiet | Health, crowding |
| Window | Fatigue, warmth, Holdfast haul |
| Ink | World state, hatch reversed |

---

# SECTION 10 — LORE CONSISTENCY CHECK

## 10.1 Must not contradict

| Canon | Source | Duty Roster stance |
|---|---|---|
| Sector 4 map closed; no fifth Power | `00_OVERVIEW.md` | Overflow is a Current practice, not a Power |
| Player bunker = Allocation 12, unlisted | `02_THE_LIST.md` | Chart does not make them allocated |
| Sela's card genuine; four hatch branches | `02_THE_LIST.md` | Fourteenth / Ink **modify**, do not replace |
| Sole files, 41.2, not allocated | `02` | She will file living unlisted if corroborated; she will not "fix" herself |
| Quiet House: name + true thing; never adjudicate back room | `05` | Held |
| Lamplighter rule: no exception | `05` | Ivy oil side quest; Tamsin is not a Lamplighter |
| Rebuilders brass | `06` | Tin quest stacks with Leva and playground |
| Hatch dilemma magnitudes | `ExpeditionSystem.cs` | Unchanged |
| Hydro-Barons / District 8 | Holdfast | Sister pack; hook, don't redraw |
| No magic, no real countries/people, no glorified violence | `AGENTS.md` | Held |

## 10.2 Small recasts (justified)

| Item | Change | Why |
|---|---|---|
| `loc_st_brigids_almshouse` description | Quiet House door overlay | Charts already stop at a date. Located knowledge. Id stays. |
| Duty roster Layer 1 | Chart becomes usable | The object was always there. Gameplay is this pack. |
| `loc_dentists_row` | Optional chair-return | Layer 1 payoff authored as a quest, not a retcon of the missing chair. |

**Not retconned:** TrueEnding, Tessarat, 7G, androids, neuromancers, Holdfast geography, Provisioned vs Blank Rows distinction, cannibal barge (Holdfast's problem).

## 10.3 Timeline

| When | Event |
|---|---|
| Exchange−3Y | Bunker Boom; Allocations numbered; 11/12/13 overflow holes |
| Exchange−1M | Quiet Evacuation north; Convoy 12 held on DOB |
| Exchange+0 | Hatch standby; unlisted occupancy; chart left blank |
| Exchange+3D | Nameplates into the tin |
| Exchange+2Y | Halvard dies at 12-B |
| Exchange+4Y | Ice Road regularised (Holdfast); Blank Rows practice named among overflow occupants |
| Exchange+5Y | **Now.** Census. Levy. The chart can no longer stay an unused fitting. |
| Exchange+5Y winter | `season_second_winter` may fall |

## 10.4 Base-game / Holdfast references (use them)

Sela, Sole, Frayne, Voss, Pell, Delacroix, Wren, Ianov, Ivy, Edor, Yara, Leva, Ormund, Nomi, Ostrowski, Quiet House, Archivists, Lamplighters, Provisioned (do not merge), Long Walk, brass tin, boot crate, dental chair, hatch constants, Ice Road, 12-C, Cluster missing-strip, Waystation A, membrane 48h.

## 10.5 Word to the implementer

If a system wants a seventh Codex relationship, a retuned hatch constant, or a walkable 3D stack, **stop and ticket it**. The expansion is a chart, a ladle, a stool, a tin, a quieter room, and a hatch that reads the wall. That is enough.

---

# APPENDIX A — Integration matrix (Holdfast ↔ Exp 2)

## A.1 Holdfast → Duty Roster

| Holdfast flag / state | Duty Roster change |
|---|---|
| `holdfast_levy_honour` | Three rows `status=levy`. `se_levy_absence`. Ladle extra. Fourteenth harder (crowding down, grief up). Tamsin short-handed. |
| `holdfast_levy_substitute` | Kess marks IRREGULAR. Edor trust down. Pell may notice wrong trades. |
| `holdfast_levy_refuse` | `se_edor_stool` forty days. Ice Road may go dark (Holdfast 11-day lamps). Window quest uses closed-road variant. Intercom threatening pair. |
| `holdfast_membrane_sector4` | Iodine/filters/brass short at home. `mmc_membrane_iodine`. Filtration ticks faster. Frayne minutes already hungry. |
| `holdfast_membrane_let_drop` | Office legitimacy crack: escort thinner; Blank Rows more willing to hide; levy prices (labour) change — Cluster desperate, not polite. |
| 12-C live / `item_order_12c` | `se_intercom_office`. Kess refuses ink unless ordered. Ink quest hard-gates. |
| Sela claimed (clinic) | Water memory gone. Boot crate used. `mmc_sela_row` skipped. Quieter if her neighbour died. |
| Sela stays | She is a row. Clinic claim remains pressure. 12-B kit quest locked to "engineering." |
| Waystation staffed | Home watch short. `quest_roster_tamsin_watch` can send her north. |
| Ice Road dark (Yara withdrew) | No haul. Everyone home. `se_road_dark_crowd`. Pell/Voss become the labour threat. |
| `ending_holdfast_tender` | Fourteenth variant: Fleet needs beds. |
| `ending_holdfast_dark_road` | Edor's incomplete return can be finished or buried at the weigh hut. |
| `alloc12_honoured` | +5 adults crowding. Chart overcrowded. Ladle brutal. |
| `alloc12_letter_only` | Sela in; adults may be the fourteenth visitor. Parent marks. |
| `alloc12_refused` | Card in ash. `mmc_deny_forty`. Quiet Name may find it. |
| `alloc12_terms` | Voss doctrine already spoken in the Stack. Pell easier. Ansel says so. |
| `mutation_transit_tax` | Pell/checkpoint on Approach. |
| `mutation_medical_supply_gone` | Hadi's rag is the market. |

## A.2 Duty Roster → Holdfast

| Duty Roster flag / mutation | Holdfast change |
|---|---|
| `flag_hadi_listed` / `_sent` | Levy list includes him. Missing-strip filled. Outfall labour exists. |
| `flag_hadi_hidden` | Levy weaker. Strip hangs. Nila may hide him. Edor occupations stay wrong. |
| `mutation_hadi_never_back` | He is not available for levy. Clinic cannot claim a vet. Frayne notices field-care missing. Rag on hook. |
| `mutation_column_voss` | Column never arrives Cluster. Gate has Garrison. Edor waits for people who are in the Grid. |
| `mutation_column_hidden` | Edor cannot complete. Stool empty (nothing to wait for) **or** still there (he doesn't know). Ice Road may stay lit (Cutters not asked to dark you). |
| `mutation_brass_north` | Leva seats; playground; tin legendary. |
| `mutation_brass_frayne` | Allotments clock; Leva still short. |
| `mutation_brass_kept` / `mutation_plate_on_wall` | Northern brass prices up; wall catches light. |
| `mutation_quieter_room` | Hatch reversed offers an empty bunk. Escort list short one. |
| `mutation_schedule_living` | Ormund's drawer occupancy current. 12-C Drown copy lists your people. Treaty possible. |
| `mutation_roster_ink` | Hatch reversed reads the wall. Schedule Holds slide is specific. Block C plates match. |
| `mutation_roster_pencil` | Audit. Edor current-enough. Nila still talks. |
| `mutation_roster_blank` / `_burned` | Escort brings **their** list. Sole cannot complete. Forty rooms stay theoretical. |
| `flag_home_failed` | Haul/steam watch fail. Accident book. |
| `faction_blank_rows` access lost | No hide for next levy. |
| `mark_child_truth` | Cluster school / Wren versions. |
| Tamsin at waystation | `quest_rep_steam_watch` careful-check bonus. |
| Chair returned | Dentists' Row recast; no Holdfast effect. |
| Boots to Cluster | Forty-rooms / school inspect. |

## A.3 Two-way flag list (10) — parent summary

1. Levy honour/refuse/substitute ↔ empty bunks, ladle, Edor's stool, irregular mark.  
2. Membrane strip/drop ↔ iodine/brass/filter at home, Office tone, hide-willingness.  
3. 12-C live ↔ intercom, ink hard-gate, Kess refusal.  
4. Sela clinic vs stay ↔ row/guest, boots, 12-B kit language, quieter neighbour.  
5. Waystation staffing ↔ Tamsin, home watch, steam watch.  
6. Ice Road dark ↔ closed-road window quest, Pell/Voss instead of levy ice.  
7. Hadi listed/hidden/gone ↔ levy names, outfall, Ianov, never-back.  
8. Voss intercept ↔ Gate description, Edor waiting for the wrong district.  
9. Brass tin/plate ↔ Leva, Frayne, playground, Holdfast achievements.  
10. Ink/pencil/blank/burn ↔ hatch reversed escort list, Sole completeness, Block C nameplates.

---

# APPENDIX B — Proposed id checklist (collision notes)

Verified non-colliding against `locations.json` / `locations_expansion3.json` / `QuestlineSO.Ids` / `currents.json` / `faction_lore.json` / Holdfast proposed ids **at time of writing**. Re-grep before commit.

**Existing reused:** `loc_weighbridge`, `loc_conscription_office`, `loc_the_allotments`, `loc_grange_hall`, `loc_alloc_12b`, `location_the_memory_vault`, `loc_st_brigids_almshouse`, `loc_dentists_row`, `loc_school_gymnasium`, `loc_veterinary_surgery`, `brass_fittings`, `iodine_pills`, hatch constants, Holdfast NPCs/quests/flags listed in Appendix A, `faction_quiet_house`, `faction_the_provisioned`, `npc_sergeant_pell`, `npc_wren`, `npc_sela_renn`.

**New (selected):** `expansion_the_duty_roster`, `faction_blank_rows`, `npc_kess_adler`, `npc_hadi_morrow`, `npc_tamsin_rook`, `npc_ansel_duth`, `npc_len_quill`, `npc_nila_brant`, `loc_stack_roster_wall`, `loc_approach_stool`, `loc_overflow_alloc_11`, `quest_roster_the_chart`, `quest_roster_ink`, `mutation_roster_in_use`, `season_second_winter`, `ending_roster_ink`.

Full lists in §§2–7. Do not mint `loc_alloc_12b` (exists). Do not mint a 7th `faction_lore` row.

---

# APPENDIX C — Next prompt (implementation)

> Implement Sprint 1 of `docs/expansions/expansion_02_the_duty_roster_plan.md`: `DutyRosterSystem` + `MoraleMarkSystem` (plain C#, events, save/load), JSON location overlays for Stack wings (`loc_stack_roster_wall`, mess, sleeping, filtration), quests `quest_roster_the_chart` / `quest_roster_who_eats`, NPCs Kess Adler and Ansel Duth. Reuse hatch constants; do not retune. Do not add a 7th faction to `faction_lore.json`. Register new quest ids in `QuestlineSO.Ids`. Re-grep all new ids. Verify Unity batch compile and EditMode tests. Cross-tool QA: reviewer is not the implementer (Prompt #26) — roster × needs × levy.

---

# APPENDIX D — House-voice samples (shippable; more in the creative pack)

**`loc_stack_roster_wall`**
> A wall chart headed ALLOCATION 12 — DUTY ROSTER. Fourteen rows. The print date is before the Exchange. The pencil hangs on a string that has darkened from hands. Nobody has written a name, or somebody has, and the difference is the whole of the next year.

**`loc_approach_stool`**
> A folding stool in the ash, three metres from the hatch. The feet have sunk and been pulled and sunk again. There is no cup. There will be, if you bring one. The person who waits here does not knock.

**`loc_overflow_alloc_11`**
> The authenticator light is on. The chart inside is blank on purpose. A disc with no number hangs on a nail. If you write a living name in ink, this hatch will still look like a hatch. It will not open.
