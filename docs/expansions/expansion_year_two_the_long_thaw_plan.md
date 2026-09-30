# ASHFALL — YEAR TWO: THE LONG THAW
### Days 361–720 · Play on after the Reckoning · Generations · The Outposts Network

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/year-two-the-long-thaw-2026-09-29.md` (packages, paths, acceptance, decision register)
**Tone lock (inherited, unchanged):** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel. No magic, no supernatural adjudication, no real countries/wars/people, no glorified violence. Humor is dry, situational, character-earned.
**Convention:** **LIVE** = confirmed in source/data during this audit · **GAP** = confirmed missing or stubbed · **PROPOSED** = new · **VERIFY** = plausible but must be re-checked at implementation start (CLAUDE.md Rule 7).

---

## 0. How to read this document

Part I (§1–§3) is the argument: what Year Two is for, what the code and canon actually say today, and where they disagree. Part II (§4–§8) is the story: the calendar of Year Two, then one section per pillar, then the braid that ties them together. Part III (§9–§12) is the prose bank, the cast, the endings, and the system/data map. Part IV (§13–§15) covers save, determinism, UI, risk, and decisions.

Nothing here restores retired architecture, adds a parallel owner, or asks Core to know Godot. Every stateful proposal names the existing authority it extends. Where an existing authority is missing, stubbed, or contradicted by another, the document says so in plain words and asks for a signature instead of improvising (Rule 10).

---

> *"The war ended the way a fever ends. Not with a morning. With a long, ambiguous week in which
> nobody could tell you whether the sweating had stopped or merely moved."*
>
> Day 360 is a door and there are only two things to do with a door. **SEAL HERE** is not a failure
> state — it is the game keeping its first promise, bit-identically, forever. **PLAY ON** is the
> other thing, and it is not an epilogue. It is a decision to be present for a *thaw*.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

Year One asked whether the people in the Stack could stay alive long enough to be counted. The Reckoning answered: they were counted. The count was read aloud at a weighbridge, or left ringing on a dead band, or converted into a lease with a quarterly invoice. None of those endings say what happens on the morning after.

**Year Two is the morning after.** Being counted was the first year. Being counted on is the second.

Three things change when the game refuses to end at the Reckoning:

1. **The ending becomes a beginning with a standing.** Whatever the count became — accepted, held, or leased — is a live condition of the world for 360 more days, not an epilogue paragraph. (Pillar I, *Play On*.)
2. **The people become the point.** Children who arrived as line items grow into apprentices; apprentices take a mentor's keys; the founders, who have been carrying everything, get a year of being replaceable. (Pillar II, *Generations*.)
3. **The shelter gets a horizon.** For a year the whole story has been about one door. Year Two is about a second door, and the road between them, and who walks it in the dark. (Pillar III, *The Outposts Network*.)

A thaw is the least dramatic and most demanding thing a world can do to the people living in it.
Year Two is a year of paperwork, apprenticeship, distance and weather: children become apprentices,
apprentices become the people who decide things, a second shelter acquires bunks and a real
location, a road acquires cost, and Standing — A, B, C, and the strange fourth grade called **D,
The Late Call** — keeps a quarterly account of what the shelter is now understood to be.

Three things are deliberately held back and must stay held: what the Reckoning *was* in the
mechanics of the world rather than the story of the shelter; what Standing D is waiting for; and
what happens on the night after the last line the chapter writes.

### 1.2 The one sentence each pillar answers

| Pillar | The question it puts to the player |
|---|---|
| I · Play On | *What do you owe the number you were given?* |
| II · Generations | *Who do you trust with the keys, and what will you let them get wrong?* |
| III · Outposts Network | *How far can you stand from the door and still call it home?* |

### 1.3 What Year Two is not

- **Not a new game.** It is Chapter Two of the same campaign, on the same save, with the same roster, the same inventory, the same clocks.
- **Not a second colony sim.** The second shelter is a place the existing outpost authority learns to hold; it does not ship its own power, water, food, or population ledger.
- **Not a dynasty game.** Nobody is "the heir" by birth. Succession is earned by hours of shared work, and it can fail, and failing is written down.
- **Not a punishment for reaching Day 360.** Difficulty in Year Two is *consequence*, not inflation: the winter is milder than Year One's worst, but you are farther from your stores.
- **Not an AI story.** The Tempest is a dead-hand bureaucracy. It has instructions, a clock, and a mandate. It never wants anything. (Verdict bible §1.5 — held verbatim.)
- **Not a rewrite of the Verdict.** The Verdict owns the count. Year Two lets the count be *lived in*; it does not reopen what was resolved.

### 1.4 The title

*The Long Thaw* is the honest name for a season that does not resolve. Ice does not leave on a date; it leaves as a set of small concessions — a gutter runs at noon, a road holds for one day and not the next, a child who could not stand in the autumn walks to the airlock in the spring. Year Two is written in concessions.

---

## 2. Canon reconciliation — what the sources actually say

The project's canon and code disagree about several load-bearing facts. Rule 6 applies: **systems win** unless the foreman overrides, and every conflict is logged rather than silently resolved.

| # | Question | World bible / prose canon | Current code / data | Resolution used in this plan |
|---|---|---|---|---|
| C1 | When is the Reckoning? | "Machine Reckoning at Day 360" (`docs/ashfall-master-world-bible-and-expansion-authority.md` L214, L226) | `ReckoningSystem`: Knowing 160, Culpable 210, **Counted 240**; verdict may be chosen from Day 240 (`SelectEnding` requires `phase >= Counted`) | Two moments, one word. **The Call** (Day 240, the count is presented) is the Reckoning in code. **The Reading** (Day 360, the campaign chapter closes) is the Reckoning in the bible. This plan calls Day 360 *The Reading* and never moves the Call. |
| C2 | How long is a year? | 360-day "Year of Ash" arc (bible L220; `YearOfAshTimelineSystem.EndDay = 360`; endings gate `min_days_survived 360`) | `CampaignCalendar.DaysPerYear = 365`; days 361–365 already authored as a glide "into next year's First Thaw" | **Chapters are 360 days; the civil year is 365.** Chapter Two is Days 361–720. Days 361–365 are **The Five Days** (§4.1). The civil New Year (Day 366, First Thaw) falls inside Q1. Day 720 is dayInYear 355 — a seam the plan uses on purpose (§4.6). |
| C3 | What is the Great Thaw? | "Days 301–360: black mud, radon, final broadcasts" | Timeline ends at 360 at +4 °C; calendar baseline for dayInYear 181–360 replays −45 °C Deep Ash for any later year | Year Two needs its own climate (§4.5). Replaying Year One's deepest cold is rejected by this plan; freezing at +4 °C forever is also rejected. |
| C4 | What is "Year Two"? | `endings.json` → `ending_dawn_of_thaw` is titled *The Spring of Year Two* | Only `ending_dawn_of_thaw` and `ending_wasteland_sanctuary` are ever reachable from the host trigger (§3, F3) | Year Two is not an ending. The title survives as the name of the *Standing* players most often carry in. |
| C5 | How old are people? | — | Adults age **30 days per year** (`life_stages.json`, `survivor_life_stages.json`); children reach `YoungAdult` at **age 720 days** (`ChildDevelopmentSystem.ResolveStage`); `GenerationalSuccessionEngine` ages dwellers **1 year per 365 days** and retires at 65 | Three clocks. Year Two adopts **AgingSystem** (adults) and **ChildDevelopment** (children) as the only age authorities and treats `GenerationalSuccessionEngine`'s `inGameAgeYears` as non-authoritative for any Year Two decision (§6.9). |

### 2.1 Storyline override — Reckoning timing and Year One endings are per storyline (user, 2026-09-29)

The first draft of this document treated the Reckoning as one day (240) and Year One's close as one day (360), with one ending-selection rule for every campaign. **That is overruled.** The game has many storylines — the base Holdfast/Verdict path, the Muster, and the 45 authored faction branches (15 Military, 15 Rebel, 15 Independent) — and each may reach its Reckoning on a different day and close Year One with a different ending, chosen by that storyline's own logic.

What this changes, precisely:

1. **The Reckoning day is a property of the storyline, not a constant.** The three phase days (Knowing / Culpable / Counted; today 160 / 210 / 240) and the Reading (today 360) come from a **Chapter Profile** selected by the campaign's storyline. The default profile reproduces today's numbers exactly.
2. **Year One's ending selection is storyline-aware.** The thin host context (F3) is retired for new campaigns: the ending comes from what the storyline actually resolved — a faction branch's own ending row, the Verdict ending, a Holdfast ending, or the base rule. **Saves already in progress keep the legacy profile** so their Day-360 behavior does not change under them.
3. **The Verdict keeps its authored day gates.** Rather than rewriting the day gates in verdict content, one adapter at the Verdict host boundary translates campaign day into *verdict day* (`verdictDay = campaignDay − profile offset`). The content stays authored in its own days; only the owner's input clock shifts.
4. **Year Two is chapter-relative.** Readings fall at *chapter-open day + 90k*, not at fixed Days 450/540/630/720. The Day-based tables in this document are the **default profile** (open day 360); other profiles shift them. World climate stays on the absolute calendar (weather does not care which faction you joined).
5. **Some storylines have their Reckoning *inside* Year Two.** A profile may defer the Call past the chapter close. That is Standing D (§5.4).

**Constraint that survives the override (F7):** a raised child cannot come of age before absolute Day 721. A profile whose Year Two ends earlier than Day 720 simply cannot end on the first Rite; the plan requires each profile to declare which finale it uses (§5.7).

**Canon extension requested (needs a signature, DEC-Y2-01):** the maintained-but-nonexistent room (§7.3). It extends existing lore; it contradicts none of it.

---

## 3. Verified current reality (audit 2026-09-29)

Each row was confirmed in source or data during this session. Line numbers can drift; **re-verify at package start.**

| ID | Finding | Evidence | Consequence for Year Two |
|---|---|---|---|
| **F1** | The campaign chapter ends on the host at Day 360 by construction. `CheckAndTriggerEndgame(day)` fires when `living == 0 \|\| day >= 360`, calls `TriggerEnding`, and the panel then offers "SEAL CAMPAIGN & FREEZE ARCHIVE". | `src/Main.Endgame.cs` ~L210–240; `src/UI/ChroniclePanel.cs` L190–214 | Play On must change this trigger; it cannot be a content-only feature. |
| **F2** | Sealing has heavy terminal side effects: terminal `SaveAll`, unified-ending resolution rewrites the epilogue prose, generational legacy archived, completion history appended (append-only, per run identity), meta-progression recorded. | `src/Main.Endgame.cs` `OnCampaignSealed` L72–190 | Only the *final* seal may run these. Year One's close must not append a completion record or freeze the archive. |
| **F3** | The host's ending context is **thin**: it sets `CurrentDay`, `LivingSurvivors`, `DeceasedSurvivors`, `AverageMorale`, `ExpeditionsCount`, `ForceExtinction` — and never `TruthBroadcasted`, `VassalageAccepted`, `DominantFaction`, or `ForceWinterFailure`. Of the eight authored endings, the host trigger can only ever select `ending_silent_tombs`, `ending_wasteland_sanctuary` (≥15 living, morale ≥75) or `ending_dawn_of_thaw`. | `src/Main.Endgame.cs` L224–232 vs `EndgameSystem.EvaluateEnding` | The richer projection already exists: `BuildCampaignOutcomeSnapshot()` (INV-19.1) feeds `UnifiedEndingResolver` at seal time. Year Two's Standing must read *that*, not the thin context. |
| **F4** | `YearOfAshTimelineSystem.AdvanceDay` clamps at `EndDay = 360` and ignores earlier days. Its temperature feeds `_deepFreeze.TickDailyThermal`, `_radon.TickDailyRadon`, and `_iceRoad.TickDay`. | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` L60–65; `src/YearOfAsh/YearOfAshHostSession.cs` L170–175 | **Blocker.** After Day 360 the shelter's thermal, radon, and ice-road inputs freeze at Day-360 values. Nothing in Year Two is honest until this is lifted. |
| **F5** | Other systems carry the same 360 horizon as a data default or constant: `QuestlineSystem.maxDay`, `YearOfAshCatalogLoader.maxDay`, `DoorEncounterSystem.maxDay`, `DoseContentCatalog.maxDay`, `CaravanTradeRouteCatalog.season_end_day`, faction-war content windows. | `grep` over `Assets/Ashfall.Core` (this session) | A horizon census is Package 0 work, not an assumption. |
| **F6** | Authored content beyond Day 360 is almost nonexistent. A scan of numeric `min_day / max_day / start_day / end_day` keys in `Assets/StreamingAssets/Data/**` finds only: `events.json` (11 entries at 365), `quests_npc_arcs.json` (8 at 365), `radio.json` (5 at 365), `narrative_discovery_manifest.json` (1 at 390), `nuclear_winter_phases.json` (1 at 361). The faction-war files' Day-480+ values are *authored* days mapped to playable Day 180+ by a 300-day offset (world bible L220) — not Year Two. | this session's scan | Year Two is a **greenfield** content space. It cannot be "unlocked"; it must be written. |
| **F7** | Child growth is already tuned to a ~720-day childhood: Infant <60, Toddler <180, Child <500, Adolescent <720, YoungAdult ≥720 days of age. Canonical age is floored at birth day 1. | `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` L98–124 | The **earliest possible coming-of-age is Day 721** — one day after Year Two ends. Year Two is the whole of a childhood; the first Rite of Passage belongs to its final hour. |
| **F8** | Second-generation milestone gates exist: `first_words` (Toddler), `foundational_letters` (Child, edu ≥10), `tool_handling` (Child, ≥25), `field_survey` (Adolescent, ≥45), `vocational_apprenticeship` (Adolescent, ≥60), `rite_of_passage` (YoungAdult, ≥80), `succession_readiness` (YoungAdult, ≥95). Host session `SecondGenerationMilestoneHostSession.TickAll` records them through the ChildDevelopment owner. | `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs`; `src/Host/SecondGenerationMilestoneHostSession.cs` | The apprentice-to-successor ladder is **already authored as rules**; what is missing is story, stakes, and a moment where it changes who holds a key. |
| **F9** | `ApprenticeshipSystem` (mentor/apprentice pairs, target skill XP, `NotifyMentorDeath`, wills, `ExecuteWill`) and `GenerationalLineageExtension.PerformSuccession(retiree, successor)` (retire + generation bump + family event) both exist and are host-wired (`ApprenticeshipHostSession`, `GenealogyHostSession`). | files as named | Succession has primitives. It has no *council*, no role transfer, no consequence. |
| **F10** | The outpost network is real but thin. `OutpostSettlementSystem` (4 authored outposts in `outposts.json`; establish, garrison, supply, daily consume, overrun, abandon) is host-wired with its own save section `outpost_settlement` and a UI route through `ShelterOperationsPanel` ("ESTABLISH", garrison, relieve). `WaystationNetworkSystem` (14 authored waystations with keepers, filters, watch, 7-day resupply) is host-wired. `ColonySystem` (player-founded colonies, buildings, supply lines) has a `colony` section. | `SaveSectionRegistry`; `src/Main.OutpostSettlement.cs`; `docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` §2 | The **custody decision is signed**: Outposts = authored positions *and secondary holdfast positions*; Colony = player-founded; Waystation = holdfast S2 waystation; SettlementCatalog/TerritoryControl = world catalog. **This plan honours all four and merges none.** |
| **F11** | **Outposts are never attacked.** `Main.WorldDangerRatingForDay(day)` returns `0` unconditionally; `TickOutpostSettlement` returns before drawing any RNG when danger ≤ 0. `SimulateRisk` is real code that the running game never exercises. | `src/Main.OutpostSettlement.cs` L280–287, L220–222 | "Supply and defend" has no *defend* today. Package 6 must replace the stub with a truthful pressure source. |
| **F12** | **Outposts are supplied for free.** `TickDay` calls `centralRationSupplyProvider`, which pulls rations from the canonical inventory every day with no travel, no convoy, no distance. | `src/Main.OutpostSettlement.cs` L216, L233–250 | Distance has no cost. The Long Thaw is a story about distance. Package 6 must make supply a journey. |
| **F13** | `outposts.json` `graph_node_id` values (`node_north_ridge`, `node_rail_junction`, `node_radio_tower`, `node_deep_quarry`) are referenced only by `outposts.json` and one test; no other data or source file defines those node ids, and the integrity validator checks outpost *costs*, not nodes. | `grep` this session; `CatalogIntegrityValidator.ValidateOutpostCostCatalog` | Outposts float free of the travel graph (Plan 32). They must be bound to real locations before they can be *walked to*. |
| **F14** | Established outposts can be re-established to clear `IsOverrun` (`EstablishOutpost` resets it), but there is no *relief* command with its own cost or risk. | `OutpostSettlementSystem` L196, L221 | Relief must be a journey, not a button. |
| **F15** | The Verdict's three endings each carry an operational hook that Year Two can inhabit: **Recounts** (count read at the Grain Exchange weighbridge, "accepted as read"), **Held** (carrier tone continues; `CENSUS WINDOW: OPEN`), **Lease** (quarterly maintenance, a reading per season, a census every 1,827 days, an invoice delivered "to a door that opens, because the door is counted"). `VerdictEndingEvaluator.IsTempestDecommissioned` is true exactly when the sector recounts. | `verdict_data.json` `endings`; `VerdictEndingEvaluator.cs` | The three Standings write themselves. The **lease's quarterly cadence gives Year Two four dated readings: Days 450, 540, 630, 720.** |
| **F16** | The lore already contains the second shelter. `duty_roster_locations.json` defines **Allocation 11** (Nila Brant; four unregistered sleepers; "Eleven has no ledger, only a habit"), **Allocation 13** ("the room the Grid says does not exist… someone has been keeping its airlock greased and its light fitting replaced, quietly, for years"), the **Pump Hatch**, and **The Blank Cellar** (rule painted on the wall: DO NOT WRITE THE LIVING). `codex_entries.json` / `standing_record_layouts.json` define **Allocation 12-B** (subway maintenance level; improvised potable water; chalk marks fourteen, a gap, six). `BUNKER_ORIGIN_CONTINUITY.md` fixes the Holdfast as **the Stack, Allocation 12**, with 11 and 13 as neighbouring narrative space. | data + docs as named | Year Two does not invent a second shelter. **It re-opens Thirteen.** |
| **F16b** | Existing quest text already stages the moral pivot Year Two inherits: `duty_roster_quests.json` offers "Copy it for Sole. Completeness opens 13 to a file and closes 11 to you." and "Leave the scar, rewrite the name, or pocket a rubbing for Sole." | `duty_roster_quests.json` ~L898–914 | Whatever the player chose there is a **live flag Year Two must read** (VERIFY the flag ids), not overwrite. |
| **F17** | Prior proposals cover parts of this ground but are documentation-only and unapproved: Expansion 44 *The Outpost* (Wave 7), Expansion 82 *The Far Hearth* (Wave 17), Expansion 46 *The Long Change* (Wave 7), Expansion 12 *The Second Generation* (partially implemented as F8/F9). Expansion 82's central premise — "no direct Godot reference to ColonySystem, no save-section key" — is **stale**: `SaveSectionRegistry` now carries `colony` (ORPHAN-SEAL-W1). | `docs/expansions/wave7`, `wave17`; registry | This plan **consumes** 44/82 as design references. It does not supersede them and does not implement them wholesale. Where they conflict with signed custody, custody wins. |

### 3.0 Findings added after the storyline override (audit 2026-09-29, second pass)

| ID | Finding | Evidence | Consequence |
|---|---|---|---|
| **F18** | Branching is real and large: `FactionBranchCoordinator` (`Assets/Ashfall.Core/Factions/`) owns strictly mutually exclusive commitment across `MilitaryBranchSystem`, `RebelBranchSystem`, `IndependentBranchSystem` plus durable `PrpfStandingSystem`. Data: `military_faction_branch.json`, `rebel_faction_branch.json`, `independent_faction_branch.json` — **15 branches each, 3 endings each (135 branch endings)**, each branch with a point-of-no-return flag (`flag_branch_<family>_<n>_ponr`) and an `OnEndingResolved` event. | data + `FactionBranchCoordinator.cs` L68–130 | Branch endings already exist as facts. Year One's close can read them; nothing new needs authoring to *have* a branch ending. Each needs a Year Two **Standing modifier** and Chronicle line (§5.7). |
| **F19** | The Reckoning phase days are `const` values inside `ReckoningSystem` (`KnowingDay 160`, `CulpableDay 210`, `CountedDay 240`); no other source file reads them (verified by grep), and `Poll(day, …)` compares the raw campaign day. | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` | Per-storyline timing is a small change at one owner: optional thresholds/offset, defaulting to today's constants. Verdict *content* day gates are a separate census item (P0). |
| **F20** | The Verdict can be **unresolved at Day 360.** `Poll` requires `evidenceGate` (any enrolled evidence) to move Knowing → Culpable, and Culpable → Counted needs that first; with zero evidence read the phase stays Knowing forever. `VerdictEndingEvaluator.DecideEnding` returns `null` below Counted. | `ReckoningSystem.Poll` L84–108; `VerdictEndingEvaluator.cs` L47–52 | My first draft assumed a resolved count. A campaign can reach the Reading never having been counted. That is a fourth Standing, and a legitimate one (§5.4 D). |
| **F21** | Endings are already multi-authority: `UnifiedEndingContext` carries `factionBranchId`, `musterApproachId`, `holdfastEndingId`, `verdictEndingId`; `Main.UnifiedEnding.cs` populates `factionBranchId` from `_factionBranch.Coordinator.ActiveBranchId`. The unified resolver runs only at seal. | `UnifiedEndingResolver.cs`; `src/Main.UnifiedEnding.cs` L95 | The richer projection exists; Year One's *ending selection* (not just its prose) can adopt it under a profile. |

### 3.1 What the audit means in one paragraph

The rules for growing up exist. The primitives for handing over exist. The containers for a second door exist. The Verdict already wrote three different mornings-after. What does not exist is (a) permission to continue past Day 360, (b) an honest world for Days 361–720, (c) any hostile pressure or any real distance between the doors, and (d) 360 days of writing. Year Two is therefore a **horizon lift, a governance-safe chapter mechanism, and a large body of prose** — not a large body of new mechanics. That is the reason the plan can be integration-ready without asking anyone to build a second simulation.

---

# PART II — THE STORY

## 4. The shape of Year Two

### 4.1 The Five Days (Days 361–365)

The calendar already has a five-day glide from +4 °C toward −2 °C. The plan spends it as an interlude rather than a season.

**What The Five Days are:** the time between the Reading and the New Year in which nothing is scheduled by the machine and everything is scheduled by grief. Funerals that were postponed. A ledger reconciled by hand. The first quiet count of who is actually in the room. No quest marker. No radio program. One prompt: **the Standing Sheet** (§5.3), a single-page reading of what Year One left.

**Why it matters:** it is the only place in the game where the player is asked to *look*. Every later choice reads that page.

### 4.2 Quarters

Chapter Two is four quarters of 90 days, counted from the Reading. Each has a season, a pressure, a set piece, and a **reading** — the machine's, the sector's, or nobody's, depending on Standing.

| Quarter | Days | Calendar seasons crossed (civil year) | Working title | Governing pressure | Set piece |
|---|---|---|---|---|---|
| **Q1** | 361–450 | glide → First Thaw → Ash Settling → Deep Freeze | **The Concessions** | *Aftermath.* Black mud on the roads, radon rising off thawing ground, funerals, the Standing Sheet. | The first quarterly Reading (Day 450). |
| **Q2** | 451–540 | Spring Storms → Dry Ash → First Fallout | **The Second Door** | *Expansion.* The road is open for the first time in a year. Sites, keys, the founding of Thirteen. | Opening the airlock on Thirteen. |
| **Q3** | 541–630 | False Spring → Deep Ash | **The Short Summer** | *Stretch.* Heat, bloom, harvest, and the first real cold snap arriving *after* people spread out. Raids test the far watch. | The night the relay goes dark (§7.7). |
| **Q4** | 631–720 | Long Winter → Black Rain Season | **The Handing Over** | *Consequence.* Stores tested at two doors. Succession Council. The final reading. | Day 720: the first Rite of Passage; the Year Two Chronicle. |

### 4.3 The quarterly Reading

A *reading* is a dated moment on Days 450 / 540 / 630 / 720 at which the world takes a measurement of the shelter. It is one mechanism with three voices:

- **Accepted-as-Read (Recounts):** a human reading. Market-day, at the weighbridge, by a registrar with a scale. No machine. The silence where the carrier tone used to be is the thing people react to.
- **The Window (Held):** an unrequested reading. The tone runs on its own; the machine's register counts what it counts. Nobody is asked to file anything. What the readings say is what the player did not decide.
- **The Invoice (Lease):** a filed reading. The player must file a quarterly maintenance/reading return; the machine's registers gate services on it (§5.6).

**The reading is a read model plus one filed act, not a new simulation.** It projects existing facts — roster, deaths, births, dose, evidence — and, under Lease only, records "filed / unfiled" in the Verdict save section (§13).

### 4.4 What the player feels, quarter by quarter

- **Q1:** *We are still here, and the paperwork agrees.* Relief with a residue.
- **Q2:** *We could be somewhere else as well.* Ambition with a cost tag.
- **Q3:** *We are too spread out.* The first time the map is bigger than the crew.
- **Q4:** *Someone else is holding the key.* The first time the founders are not the only answer.

### 4.5 Climate — the Thaw Years

**GAP (F4, F5):** no authoritative post-360 climate exists. **PROPOSED:** a catalog-driven extension of the existing timeline owner (not a second timeline), `year_two_climate.json`, with four phases matching the quarters:

| Phase | Character | Numbers are targets for tuning, not canon |
|---|---|---|
| Q1 — *Thaw Mud* | Rising from about −2 °C; black runoff; radon high and *slowly falling* as ground opens | ash opacity falling; radon peak early, then decline |
| Q2 — *Green Fringe* | Above freezing most days; storms; first green | the only quarter where the ice road is *not* the constraint |
| Q3 — *Short Summer, Late Snap* | Warm spell, then a cold snap that kills a bloom | thermal stress spike with warm-season complacency |
| Q4 — *Second Winter* | Colder than Year One's thaw, warmer than Year One's Deep Ash; long | peak −25 °C, not −45 °C; duration, not depth, is the threat |

**Design principle:** the world does not resume the Year of Ash and does not forgive it. The winters *change character*: Year One was depth; Year Two is duration and distance. (World-bible idiom: "second winter people" — those hardened by the Year of Ash. Year Two is the first year that phrase has a second winter to be about.)

**Reconciliation required (Package 1):** two temperature authorities currently exist — the timeline (feeds thermal/radon/ice road) and the calendar baseline (`CalculateBaselineAmbientTemperature`, feeds the calendar read model). For Days 361+ they must agree by construction: the timeline reads the catalog; the calendar's baseline for `year ≥ 2` delegates to the same catalog or is documented read-model-only.

### 4.6 Why Day 720 lands where it does

Two facts converge: the earliest child can come of age on Day 721 (F7), and Day 720 is dayInYear 355 — ten days shy of the civil New Year. The plan uses both. **The Year Two Chronicle is written on Day 720 with the first Rite of Passage a single tick away.** The final scene is not a ceremony. It is the night before one.

---

## 5. PILLAR I — PLAY ON

### 5.1 The Reading (Day 360 in the default profile; per storyline otherwise)

*Timing is set by the campaign's Chapter Profile (§2.1, §5.7). "Day 360" below means "the profile's Reading day". Close rules a profile may use: a fixed day (the default, 360), or **on a storyline's ending resolving plus a settle window**, with a floor and ceiling day so a branch can neither close before its Year One story has run nor never close.*

**Today (F1, F2):** the game shows an epilogue and offers a seal.

**Year Two:** the same trigger, the same epilogue *text*, a different verb. At Day 360 the endgame authority produces the **Year One Chronicle** — the existing epilogue report — and offers exactly two choices:

- **PLAY ON** (default): commits Year One to the record and opens Chapter Two. Nothing is sealed, frozen, or archived. The campaign day advances from 361.
- **SEAL HERE** (retained): the legacy behavior, unchanged — the campaign ends with Year One's ending. Completion history, meta-progression, and legacy archive run exactly as today (F2).

**Terminal endings stay terminal.** Extinction (`living == 0`) and Frozen Silence end the campaign with no offer to play on — nobody is left to continue. `ending_exodus_to_sea` is *not* terminal in Year Two; it becomes a Standing modifier (§5.4).

**The presentation rule:** the Reading is a screen the player already knows how to use (the Chronicle panel). It gains a button. It does not become a cinematic, a modal trap, or a new route (UI rule: extend the current route).

### 5.2 What "Play On" must not do

- It must not append a completion record at Year One's close (F2 — append-only, per run identity; a Year One record would make a played-on campaign look complete twice).
- It must not re-run `ResolveUnifiedEnding` twice on the same authority state.
- It must not alter the verdict resolution flags (`countPresented / countHeld / offerIsLease`). Those are the *Standing's* source; they are read, never rewritten.
- It must not make Day 360 unreachable by ordinary play (the trigger is still Day 360; only its consequence changes).

### 5.3 The Standing Sheet

A single read-only page shown on Day 361 and reachable afterwards from the Chronicle panel. It is a **projection**, never an authority.

**Contents (all from existing owners via `BuildCampaignOutcomeSnapshot()`):**

1. **The count** — which of the three verdict endings, and what was decided by the player versus by evidence sufficiency (`VerdictEndingEvaluator.DecideEnding` derives Recounts when `evidence ≥ 4`, otherwise Held).
2. **The house** — living, dead, children alive (`childrenSurvived`), average morale, expeditions completed.
3. **The debt** — `debtLedgersBurned` and `ledgerTampered`; who the shelter owes.
4. **The neighbours** — factions and treaties (`grandTreatySigned`, dominant faction, Muster approach if resolved).
5. **The names** — the fallen (memorial), the elders (retirement-eligible by AgingSystem), the children by stage.
6. **One sentence** — chosen by table from the intersection of the above. It is the only authored prose on the page.

**Why a Sheet and not a cutscene:** the player has spent 360 days assembling this state; the game's job is to read it back accurately and then step aside.

### 5.4 The Standing — four ways the morning arrives

The Standing is **derived, not chosen**, from the verdict resolution already on the save. Each Standing has four fixed elements: an *opening state*, a *recurring instrument*, a *signature problem*, and *a door* — a lever the player can pull to change what the Standing costs.

#### Standing A — ACCEPTED AS READ *(Sector Recounts)*
`countPresented = true`; `VerdictEndingEvaluator.IsTempestDecommissioned` is true; the carrier is expected to be silent (VERIFY how the radio owner represents a decommissioned tone).

- **Opening state:** the number exists in public. Traders quote it. It is on a chalk board at the weighbridge and nobody argues with the chalk, only with what it is for.
- **Instrument:** the **Market Reading** — held on market days, at the Grain Exchange weighbridge, by a human. The player attends or sends someone. What the registrar asks is *who is under whose roof*.
- **Signature problem — *The number is a hook.*** Everyone with a claim uses the count: the Garrison for muster rolls, the Hydro-Barons for household debt, the Cult for tithe. **Every child written into the sector's register is fed and is callable.** The player decides how many of their children to write, and the answer is different for every child.
- **The door:** the **Unwritten Clause** — a negotiated carve-out for children under a stage threshold, purchased with standing from the factions that would most like to use them. It is never free.
- **Voice:** slightly ashamed of relief. The radio, which used to have a tone under it, now has silence under it, and people leave it on anyway.

#### Standing B — THE WINDOW *(The Count Is Held)*
`countHeld = true` (or derived Held), the carrier keeps running, `CENSUS WINDOW: OPEN`.

- **Opening state:** nothing was presented; the machine did not stop. Its register still counts. The player is now inside a question nobody is asking out loud.
- **Instrument:** the **Open Reading** — the machine's own unrequested quarterly tally of the shelter, surfaced through the radio the way the carrier already is. It reports; it demands nothing.
- **Signature problem — *The second count differs from the first, and the difference is the children.*** The count taken at the Call listed persons having custody of persons. Year Two adds names the first count did not contain (births, arrivals) and removes names it did (the dead). The Day-720 reading is a **diff**, and the player can read it coming for a year.
- **The door:** **Late Presentation** — the count can still be presented in Year Two (`SelectEnding` has no upper day bound), but presenting a year-old count with a year of changes inside it is a different act than presenting it at Day 240. It resolves into Standing A's world on new terms (DEC-Y2-05 — *default: no late presentation in v1; the Window closes by silence at Day 720*).
- **Voice:** patient. It is the only Standing in which the machine never speaks, and it is the loudest.
- **Held + high deaths (`EpilogueMatrixRuntime` resolving to `TempestSterilization`: no signed treaty branch, Tempest not decommissioned, deaths > 50):** in the epilogue matrix this is the drones-sweep ending. **In Year Two it is not a scripted apocalypse.** It is a *soft threat* — a long-running advisory that restricts one route family until the count is presented or the window is closed by silence at Day 720 (DEC-Y2-04 — *default: epilogue-only; no live sweep in v1*).

#### Standing C — THE QUARTER *(The Offer Is a Lease)*
`offerIsLease = true`.

- **Opening state:** the count converted. The machine now has a schedule with the shelter's name in it.
- **Instrument:** the **Invoice** — Days 450 / 540 / 630 / 720. Each invoice is a *maintenance return*: a dosimetry reading, a dwelling census, a maintenance visit to a Tempest node, or a reading of the relay mast. Filing is a **player act**; not filing is also an act, and the machine records it without judgment.
- **Signature problem — *A door that is not counted does not open.*** The lease gates services on filed returns: the relay mast (which already draws current from the Tempest's own source — "there was never a mystery; there was a lease"), the carrier band, access to maintenance corridors. **A dwelling that is not enrolled is not served.** This is where Thirteen becomes a decision (§7.4).
- **The door:** the **Buy-Down** — completing all four returns matures the lease toward the next census (Day 360 + 1,827), reducing the cadence. Defaulting once does not end it; it lowers service tier. There is no clause that ends it.
- **Voice:** administrative. The invoice is on a machine that "stopped printing anything else in Year One". It is the only piece of paper in Sector 4 that always arrives.

#### Standing D — THE LATE CALL *(the count has not been taken)*
Reckoning phase below Counted at the Reading (F20), or a profile that defers the Call into Year Two.

- **Opening state:** the tone may have been heard; nothing was presented; nothing was refused. Year One ended without the machine's answer, and the sector is doing arithmetic on a number it does not have.
- **Instrument:** the **Approach** — the Reckoning continues on its own clock *through* the Reading. The Knowing/Culpable/Counted days shift by the profile's offset; the Call fires at the profile's `late_call_day` (default: chapter-open + 45).
- **Signature problem — *Everyone is preparing for a number that has not been read.*** Rumor, hoarding, and a quiet market in guesses. The player's evidence decisions (read machine logs or not) still matter: they decide, on the Call, whether the count is honored or held — exactly as before, but inside a year that has children in it.
- **The door:** the evidence gate. A profile may **waive** the enrolled-evidence requirement after a set day (`waive_evidence_gate_after_day`) so the Call cannot be avoided by never reading; the count is then Held by default, not silently skipped.
- **Voice:** anticipatory, a little embarrassed to be waiting. Standing D resolves into A, B, or C on the Call; from that moment the quarterly instruments follow the resolved Standing and the readings before it are recorded as *Approach* readings.

#### Modifiers (read from existing state, not new state)

| Existing input | What it does to the Standing (opening delta only) |
|---|---|
| `EpilogueMatrixRuntime` regional fate | Colours the Sheet's neighbours line and sets the *sector mood* for radio and encounters (Commonwealth / Garrison law / Warlords / Reconciled). |
| Demographic outcome (Thriving / Hardened / Ghost) | Sets how many hands Year Two begins with; **Ghost Shelter (<3 living)** enters Year Two as a *rebuilding* start with reduced set pieces. |
| Moral standing (Forgiven / Indentured / Ruthless) | Sets Q1 encounter tone and which debt-holders call first. |
| `ending_exodus_to_sea` (Flotilla-dominant) | The Flotilla has taken **a chosen part of the roster** aboard as a friendly port; the Stack keeps its door. Those who sailed are a live faction contact, not a deletion. |
| `ending_iron_hegemony` (Garrison-dominant) | The Garrison's forward outposts border the second-door sites; site selection carries a political cost. |
| `ending_warlord_tribute` (vassalage) | Year Two begins with the tribute schedule already running; outposts double as collection points. |
| `ending_wasteland_sanctuary` | More people at the door; the Nursery starts fuller. |

### 5.7 Chapter Profiles — one storyline, one clock, one Year One ending

A **Chapter Profile** is authored data (`chapter_profiles.json`, PROPOSED) that answers, for a storyline: *when is the Reckoning, when does Year One close, what ending closes it, what does that ending mean for Year Two, and how does Year Two end.* It is selected once, persisted by id in the `endgame` section, and never changes mid-campaign.

**Profile fields (PROPOSED, all validated):**

| Field | Meaning | Default profile (`profile_base_v1`) |
|---|---|---|
| `id`, `family` | `base`, `military`, `rebel`, `independent`, `muster`, … and optional per-branch override ids (`branch_mil_4_martyr`) | `base` |
| `verdict_offset_days` | shifts the Verdict owner's input clock (§2.1.3) | 0 → Knowing 160 / Culpable 210 / Counted 240 |
| `waive_evidence_gate_after_day` | Standing D safety (§5.4) | none (today's behavior) |
| `late_call_day` | where the Call falls when deferred into Year Two | none |
| `close_rule` | `{ "day": 360 }` or `{ "on": "storyline_ending_resolved", "settle_days": N, "floor_day": F, "ceiling_day": C }` | `{ "day": 360 }` |
| `year_one_ending_source` | `legacy_context` \| `verdict` \| `faction_branch` \| `holdfast` \| `unified` | `legacy_context` (today's thin context, byte-compatible) |
| `standing_source` | which resolved fact seeds the Standing | `verdict` |
| `standing_modifier_default` and per-ending overrides | how a branch ending colors Year Two (§5.4 modifiers) | none |
| `chapter_two_length` / `chapter_two_end_day` | default 360 days / absolute Day 720 | 360 / 720 |
| `finale` | `first_rite_eve` (requires end ≥ Day 720), `council_close`, `network_close` | `first_rite_eve` |

**Selection rule:** on campaign start (and on migration of an existing save), the profile is chosen as: explicit storyline commitment → that storyline's family profile; else base. An existing save without a profile id loads as `profile_base_v1` (**legacy**), so no in-progress campaign changes behavior at Day 360.

**Branch endings as Year One endings.** For a faction branch, `year_one_ending_source = faction_branch` makes the branch's resolved ending row (for example `ending_mil_4a_saint_of_wasteland`, `ending_rebel_8a_new_republic`, `ending_ind_9c_no_smoke`) the Year One ending id, with its display name on the Chronicle and its band (a/b/c) available to the Standing modifier. There are **135** such endings; data validation requires each to have either a per-ending modifier or a family default so none is orphaned. The first release may ship family defaults only and add overrides as content waves arrive.

**Worked examples (illustrative, not final):**

| Storyline | Reckoning | Year One closes | Year One ending | Year Two shape |
|---|---|---|---|---|
| Base / Verdict path (today) | Call Day 240 | Day 360 fixed | Legacy context (unchanged for old saves); new games use the unified source | Standing A/B/C, default finale |
| The Martyr (`branch_mil_4_martyr`) | Call earlier — the cause makes the count urgent (offset −30 → Call ~210) | On ending resolved + 10 days, floor 300 | `ending_mil_4x_*` | A grief-heavy Year One aftermath; the Council's first act is deciding who inherits the martyr's post |
| The Hermit (`branch_ind_9_hermit`) | Call **late** (offset +45; Standing D possible) | Ceiling Day 420 | `ending_ind_9x_*` | Fewer people, one door — Thirteen is the *only* other room; the network is small by design |
| The Revolution (`branch_rebel_8_revolution`) | Base timing | On ending resolved + 14 days, floor 320 | `ending_rebel_8x_*` | A public register is the storyline itself; registration (§6.6) is the central pressure |

*Numbers are illustrative; each profile's real values are authored and signed in Package 1B.*

**Design rule:** a profile may move *when* and *by what ending*, but never let two systems disagree about the day. The single translation (`campaign day → verdict day`) lives at the Verdict host boundary; readings, quarters, and the finale are computed from the profile's chapter-open day by one helper; the world calendar stays absolute.

### 5.5 Endings in Year Two

- **Year One ending** = the Standing (a *state*, not a terminus).
- **Year Two ending** = the Chronicle written on Day 720 (§10), a permutation of *what the generations became* × *what the network became*, coloured by Standing.
- **Sealing** happens once, at the end of Chapter Two, or earlier if the player chooses **SEAL HERE** at either Reading or the campaign hits a terminal condition.

### 5.6 The lease as a mechanism (Standing C detail)

**PROPOSED, bounded:** the lease is a *service gate list* keyed to filed returns, held in the Verdict save section (§13). It never creates a resource, never spawns enemies, never lowers a stat directly. The three service tiers are:

| Tier | Trigger | Effect |
|---|---|---|
| **Full** | All returns filed on time | Relay mast at rated current; carrier and maintenance corridors available; no penalty text. |
| **Lapsed** | One return unfiled | Relay range at enrolled doors reduced; corridor access reads as *unscheduled*. |
| **Arrears** | Two or more unfiled | Relay dims further; the invoice changes wording and keeps arriving. |

There is no "the machine attacks you." The machine has instructions, and the instructions have a next line.

---

## 6. PILLAR II — GENERATIONS

### 6.1 The premise

The game already has children (F7), a ladder of milestones (F8), mentorship (F9), and succession primitives (F9). It has no *occasion*. Year Two gives the ladder a year in which each rung is a story, and gives the succession primitive a room to happen in.

### 6.2 A child's Year Two, by numbers (from F7/F8)

Given a child born on day *b* (canonical age = Day − *b*):

| Stage | Age (days) | Reached on Day | Milestones available |
|---|---|---|---|
| Infant | 0–59 | *b* | — |
| Toddler | 60–179 | *b* + 60 | `first_words` |
| Child | 180–499 | *b* + 180 | `foundational_letters` (edu ≥10), `tool_handling` (≥25) |
| Adolescent | 500–719 | *b* + 500 | `field_survey` (≥45), `vocational_apprenticeship` (≥60) |
| Young Adult | ≥720 | *b* + 720 | `rite_of_passage` (≥80), `succession_readiness` (≥95) |

**Consequences for the shape of Year Two**

- A child born on Day 1 reaches **Adolescent on Day 501** — Q2's opening — and can hold a `vocational_apprenticeship` before the end of Q3.
- A child born on Day 200 reaches Adolescent on Day 700: an **apprentice in the last three weeks** of the chapter.
- A child born after about Day 220 will **not** reach Adolescent inside Year Two. They are Year Two's toddlers and small children, not its apprentices — and Year Two should say so kindly.
- **No child can reach Young Adult before Day 721** (canonical age is floored at birth day 1). Formal succession by a raised child therefore *cannot* happen inside the chapter.

**This is the design's most important constraint, and it is a gift.** Year Two cannot hand the keys to a child. It can hand them to an apprentice **acting** in a mentor's place, on trial, with the child's own consent and the mentor's; and it ends *the night before* the first formal Rite.

### 6.3 Three ranks, not two

| Rank | Who | What they may hold | How they get there |
|---|---|---|---|
| **Apprentice** | Adolescent-stage child, `vocational_apprenticeship` milestone, an active mentor pair | Assist with a duty; hold a *second key* (cannot lock, cannot unlock alone) | The pairing is made by a mentor and consented to by the child. |
| **Acting successor** | An apprentice whose mentor is retired, dead, or absent, ratified by the Council **for one quarter at a time** | Hold the duty under review. Mistakes are recorded. Stakes are real. | Council vote at a quarterly Reading; renewed or lapsed each quarter. |
| **Ratified successor** | A Young Adult with `rite_of_passage` and `succession_readiness` | The role, permanently | Only from Day 721; the Year Two Chronicle *names* who will stand for it. |

The **adult** side of succession *does* happen inside Year Two: founders who reach retirement age under AgingSystem (age 65, 30 days per year — a founder who joined on Day 0 at age 41 or older reaches it by Day 720; at 53 or older, by Day 360) may step down, and their duties pass to **acting successors** drawn from apprentices *and* from younger adults who have been under a mentor. This is the "successor" story Year Two can tell in full.

### 6.4 The roles that can be inherited

**PROPOSED role list (VERIFY against `SurvivorRoleSystem` and the existing duty roster):**

1. **Keeper of the Stores** (inventory and rations).
2. **The Physician / Clinic** (medical, triage, the sick list).
3. **The Watch** (perimeter, early warning, defense).
4. **The Engineer of Water and Air** (filtration, the well, the Pump Hatch).
5. **The Registrar** (the ledger, the Standing Record, the census).
6. **The Radio** (the mast, the schedule, the long silence).
7. **A Waystation Keeper** (one per staffed waystation — the named keepers already authored).

Each role ties to an existing owner (role assignment, duty roster, skills). **No new role ledger is created;** the Council writes a *designation* into the ledgers that already exist.

### 6.5 The Council

**The Succession Council** convenes on each quarterly Reading (Days 450 / 540 / 630 / 720) and is a **command surface over existing commands**, not a simulation. It presents, for each role with a pending mentor change:

- the *candidate* (apprentice or adult),
- the *evidence* — hours of shared work (from `ApprenticeshipSystem`), milestone state (from ChildDevelopment), education score, and one line from the mentor,
- the *risk* — what changes if the candidate errs,
- three verbs: **Ratify for the quarter**, **Extend the mentorship**, **Decline** (and say why).

**No hidden rolls.** Success is derived from the record: education, hours, mentor kinship, the trust the crew has in the candidate. A candidate who fails does so for a reason the player could have read.

### 6.6 Do Not Write the Living — registration as the generation's defining choice

**The line already exists.** The Blank Cellar's rule — DO NOT WRITE THE LIVING — and Nila Brant's "Three is a column" are Year Two's moral thesis, ready-made.

Across all three Standings, **a person written into a register is legible to everything that keeps registers.** The plan uses the existing Voluntary Register, Standing Record, census, and genealogy systems (VERIFY which is the right owner for "who is written") to make one decision recur:

> *Do you write this child down?*

| If written | If unwritten |
|---|---|
| Rationed by the register's owner; eligible for its protections (Standing A: Market Reading entitlement; Standing C: enrolled dwelling counts). | Fed from the shelter's own stores; invisible to any list. |
| Callable: muster rolls (A), the lease census (C), and the second count (B) all include them. | Safe from being called. Also unrecorded when they die, and unrecorded when they come of age. |
| The Rite of Passage is *witnessed on the record*. | The Rite happens, and only the household knows. |

**The plan does not judge either answer.** The Year Two Chronicle records both, and the epilogue for a generation raised off-register is a different, and not worse, paragraph.

### 6.7 Elders

Elders are not just successors' predecessors; they are the last living carriers of pre-war know-how and of Year One. Three beats:

1. **The Last Lesson.** A retiring founder can teach one skill to one apprentice as a *final wish* (the existing `FinalWishSystem` is the natural owner — VERIFY). The lesson is a single scene, not a stat.
2. **The Handover.** The founder gives up a physical object — a key, a stamp, a handset, a logbook — through the existing heirloom/personal-belongings path (`HeirloomSystem`, `PersonalBelongingsSystem`). The object is the succession, made visible.
3. **The Long Watch.** A retired elder can be posted to a quiet station (a waystation, or Thirteen's first night), where *light duty* still matters. Retirement is a change of post, not a removal.

### 6.8 Losses

Generations without loss is a nursery. Year Two includes:

- **A child dies.** Rare, never random-cruel: it follows from a state the player could have read (a Toddler in a cold room, a fever on a short-stocked shelf). It routes through existing memorial, grief, and Reckoning rite-trace (W10 — rites are *not* evidence).
- **A mentor dies mid-pairing.** `ApprenticeshipSystem.NotifyMentorDeath` exists; the story treatment is that the apprentice is now *acting* whether ready or not (rank 2), on a **one-quarter provisional**.
- **An apprentice declines.** Consent is real. A child who says "not that" is not overridden; the role stays open and the Council writes that down.

### 6.9 Clocks (VERIFY, Package 4)

To keep one authority per concern, the following is fixed for Year Two:

| Concern | Authority | Notes |
|---|---|---|
| Adult age, retirement eligibility | `AgingSystem` (30 d/yr; retire ≥ 65) | |
| Child stage and milestones | `ChildDevelopmentSystem` / `GenerationalSystem` canonical child | Age floor at birth day 1 preserved |
| Lineage, family names, succession facts | `GenerationalLineageExtension` + `GenealogyBridge` | `PerformSuccession` records the fact |
| Mentor/apprentice pairs, wills | `ApprenticeshipSystem` | |
| `GenerationalSuccessionEngine.inGameAgeYears` (365 d/yr) | **Non-authoritative.** Not read by any Year Two decision. | Recorded as a debt item, not fixed here. |

### 6.10 What the successors change

A generation that inherits *is* not a copy. The plan lets the **acting successor's own aptitudes** (from `SecondGenerationMilestoneEngine.CalculateAptitudeModifiers`) shift the role's *style*: a Watch held by a child raised on radio traffic listens differently; a Physician raised in a cold clinic prizes stove-time over instruments. The effect is prose and small multipliers in the role's existing owner — never a new stat block.

---

## 7. PILLAR III — THE OUTPOSTS NETWORK

### 7.1 The premise

A network of three kinds of place, each with its own signed owner, joined by one thing the game has never had: **distance that costs something**.

| Place | Owner (signed custody, F10) | Role in Year Two |
|---|---|---|
| **The Stack** (Allocation 12) | Holdfast core owners | The home door. |
| **Waystations** — 14 authored, each with a named keeper | `WaystationNetworkSystem` | The **Road**: places that keep travel possible. They are *kept*, not owned. |
| **Outposts** — 4 authored + Year Two additions | `OutpostSettlementSystem` | *Positions*: garrisoned, supplied, defended. |
| **The Second Shelter — Thirteen** | `OutpostSettlementSystem` (secondary position) | The **second door**: the only place that can be a home. |
| Player-founded colonies | `ColonySystem` (separate) | *Not used by this plan.* Remains as-is (§7.9). |

**Rule (signed, unchanged):** no second population, food, inventory, or settlement ledger is created. The Network is a **read model and a command facade** over the four owners, plus one behavior — a supply run — that is an existing expedition.

### 7.2 The map

Lore anchors (LIVE, F16), all in or beside the region `the_overflow`:

- **Allocation 12 — the Stack.** Home.
- **Allocation 11.** Nila Brant. Four people who are not on any chart. No ledger, only a habit. She trades a hiding place for a filter and will not say please.
- **Allocation 12-B.** Subway maintenance level. Improvised potable water, still working. Halvard Renn (water engineer, "NOT ARRIVED — 12-B UNCONFIRMED" on the allocation form). Sela (a dependent line "that may be Sela if she is yours to name").
- **Allocation 13.** *The room the Grid says does not exist.*
- **The Pump Hatch.** A pit the Grid stopped certifying.
- **The Blank Cellar.** Pencils in a jar. DO NOT WRITE THE LIVING.

Outposts already authored (F10, F13): **North Watch** (`node_north_ridge`), **Rail Depot Waystation** (`node_rail_junction`), **High Peak Relay Tower** (`node_radio_tower`), **Deep Quarry Camp** (`node_deep_quarry`) — currently un-bound to any real location.

**PROPOSED:** every Year Two position is bound to a real location id that the travel graph and the map actually contain, and the four existing outposts are bound retroactively (Package 5). A network you cannot walk is a decoration.

### 7.3 THIRTEEN — the second shelter

**The story (canon extension, DEC-Y2-01):** Thirteen is the room the Grid says does not exist, and it is the only room in the overflow that has *never lapsed*. Someone has kept its airlock greased and its lamp fitting replaced, quietly, for years. Year Two answers who:

> **There was never a keeper. There was a schedule.**

A maintenance contract, filed under a department that no longer exists, for a room a different department deleted. The contract's next line is quarterly, and nobody cancelled it. The grease is the machine's. The room's *nonexistence* is the reason it was never looted.

This is the same shape as the Verdict's own line about the relay mast ("There was never a mystery. There was a lease.") and it is **deliberately not** a miracle, a ghost, or a benefactor. It is bureaucracy doing exactly what it was told, for a war that is over, and it means the second shelter is real, works, and **is listed in a register the shelter did not know it could read.**

**What Thirteen is, mechanically:**

- A **secondary position** in `outposts.json` with a new `tier` value (`hearth`) and **more bunks than any current outpost** (12 vs. the current maximum of 8).
- Established by the existing `EstablishOutpost` path (cost from canonical inventory; garrison from canonical roster).
- **Provisioned by delivery**, not by a private ledger: rations, fuel, filters, and water arrive through *supply runs* (§7.5) and are consumed by the outpost owner's own daily tick.
- Its **nursery slot** is a caregiver assignment through the existing ChildDevelopment owner — children can be raised at Thirteen.

**What Thirteen is not:** a second holdfast simulation with its own power grid, its own greenhouse, its own dose ledger. Its "readiness" is four inventory-backed conditions (heat, water, air, rations), read from the items it holds.

### 7.4 Thirteen under the Standings

| Standing | What Thirteen means |
|---|---|
| **A · Accepted as Read** | A place that *can be written or not*. Registering its people makes them legible to the Garrison's rolls and to the Cult's tithe. Leaving them unwritten keeps them safe and off every benefit. |
| **B · The Window** | A room the machine's register may or may not contain. The Open Reading's diff at Day 720 includes whoever slept there. |
| **C · The Quarter** | *Enrolled or unenrolled.* A dwelling that is not counted does not open — no relay, no corridor, no invoice. An enrolled Thirteen is served and billed; an unenrolled Thirteen is safe from the Tempest and unreachable by it. **The bureaucracy that kept the grease on the airlock has a line for it;** enrolling means reading that line aloud. |

**The pre-existing flag (F16b):** if the player copied the scar for Sole earlier ("completeness opens 13 to a file and closes 11 to you"), Thirteen is *already filed* and Nila's Eleven is closed to the player. If the player left the scar, Thirteen is still unfiled and Eleven is open. **Year Two reads that flag and opens with the corresponding truth**; it never resolves it for the player.

### 7.5 The Road — supply as a journey

**Today (F12):** outposts are fed for free every day.
**Year Two:** an outpost's rations, fuel, and filters arrive by **supply run**.

- A **supply run** is a dispatched party (existing expedition owner) carrying a **manifest** from canonical inventory. It has a destination, a duration derived from the graph, and the usual encounter exposure. On arrival it calls the outpost owner's existing supply command.
- The outpost's daily tick consumes only its **own reserve**. When the reserve empties, it starves (existing behavior: condition falls, `IsStarving`, seam events).
- **Legacy compatibility:** the original four outposts retain their current *tether* supply mode unless the foreman signs a change (DEC-Y2-06). New Year Two positions use *convoy* mode.
- **Waystations as legs.** A waystation with a watch assigned and a healthy filter shortens the leg, reduces encounter exposure, and gives the party a place to rest. A waystation with a dead filter and no watch does neither. The existing decay (filter −1.5 per day; condition floor) is the pressure.
- **Why not ColonySystem's supply lines?** Two supply concepts is one too many. Authored outposts are the signed owner of their own reserve; a `SupplyLine` in ColonySystem is for player-founded colonies and stays that way.

### 7.6 The Watch — defense that means something

**Today (F11):** hostile pressure is a stub returning 0; no outpost has ever been attacked in play.

**Year Two:** pressure comes from **named sources that already exist** and are read, not invented:

| Source | What it contributes |
|---|---|
| Warlord doctrine (`WarlordDoctrineSystem`, `warlords_sector_4`) | Raid *pressure* by season and doctrine (VERIFY the owner exposes a readable danger signal). |
| Faction war/territory state | Border friction near Garrison-held nodes. |
| Route hazard from the travel graph / weather | Cold snaps and floods raise danger for *travel* and for *exposed positions*. |
| The Standing | Iron Hegemony: Garrison patrols near outposts. Warlord tribute: collection rounds. |

**Defense inputs the player controls (all existing):** garrison size (`+10` effective defense each), position condition (permille), authored defense rating, the **radio relay** (`RadioRelayRange` — positions with a working relay get a *warning day* before a raid, isolated ones do not), and a **relief** party (F14 — a journey, not a button).

**Consequences:** an overrun outpost keeps its existing meaning (cut off until relieved); the story adds *who was in it*. Survivors garrisoned there are not deleted; they are **cut off**, with a countdown to the point where their absence reads as loss.

### 7.7 The Night the Relay Goes Dark (Q3 set piece)

The relay mast at High Peak is what lets the far watches speak to the Stack. Under Standing C it is also the lease's leash. In Q3 it goes dark on one of three causes, *decided by state, not dice*:

- **Lapsed lease** (C): an unfiled return dims it.
- **Cold snap** (any): the late-summer snap that follows the bloom freezes the mast's feed.
- **Sabotage-by-neglect** (any): the relay's watch was never staffed.

The scene is one night. The player has the Watch's radio, a list of three positions, and a rule: **no position can be reached before dawn.** What the player does is *who they call first, and who they do not*.

### 7.8 Keepers

The 14 existing waystations each have a **named keeper** and a `local_problem`. Year Two's Network treats each as a *person with an open problem*, not a stat block:

- **Warden Kessel** (soot on the filters) · **Deacon Vane** (scree and chalk) · **Foreman Taggart** (the sinking abutment) · **Mistress Corvo** (sand in the well) · **Diver Renn** (the welded sluice) · **Weigher Orlov** (the garrison's cut) · **Lamptender Orris Vail** (pitted reflector) · **Registrar Odie Vant** (two marks on one claim) · **Assayer Mira Vos** (the low assay) · **Counter Ilse Marr** (short of shielding lead) · **Sergeant-Marshal Ada Kesk** (a patrol at dusk) · **Clerk Edor Vale** (two consignments, one receipt) · **Walker Kaspar Drej** (the washed landing) · **Runner Tev Alderney** (four beds).

**Year Two adds one line per keeper — the same problem, one year older** — as the seed of a small arc, written in Wave content (Package 8). A keeper whose problem the player helped in Year One remembers it; one whose problem they ignored has *not fixed it*, and has adapted in a way that costs the player something *specific* (a higher price for filters, a closed spring, a bed given away).

### 7.9 Relationship to ColonySystem and Expansions 44/82

**ColonySystem stays separate.** Player-founded colonies (with their five colony types and six building types) remain the signed owner of *player-founded colonies*. The Long Thaw's network is authored (outposts) and kept (waystations); it is not colonial. If the foreman later wants player-founded colonies as a Year Three option, nothing here forecloses it.

**Expansion 44 (The Outpost) and 82 (The Far Hearth)** are consulted for **vocabulary** (charter, founding party, depot, rotation, recall) and **rejected as authority**: 82's premise is stale (F17), and 44 proposes a `OutpostHostSession` that would duplicate the signed `OutpostSettlementHostSession`. Anything either proposes that this plan needs is re-derived here against current custody.

---

## 8. The braid — how the three pillars hold each other up

The pillars are not three features. They are one story told three ways.

| Thread | Pillar I (count) | Pillar II (people) | Pillar III (places) |
|---|---|---|---|
| **The child in the room.** A child, Toddler by Q2, is written or unwritten. | Standing decides what *written* costs. | The child's stage sets what they can do. | Thirteen's nursery slot is where they grow up. |
| **The old founder.** Reaches retirement in Q3. | The lease invoice requires a signature only they hold. | They hand a key to an acting successor. | They take the Long Watch at a waystation. |
| **The door that is counted.** | The Standing's instrument. | The registrar who does the counting. | Thirteen enrolled or not. |
| **The winter.** | The Window's diff arrives in the cold. | A mentor dies in Q4; the apprentice acts. | The far watch's stores are tested at two doors at once. |

**Design rule:** any Year Two set piece should touch at least *two* pillars. A raid on a waystation should be about *who is in it* and *who had the key*; a Council vote should be about *what else they have been asked to hold*; a reading should be about *who is standing at the door when it is taken*.

---

# PART III — PROSE, PEOPLE, AND ENDINGS

## 9. Prose bank

Sample texts for tone lock and for writers. **All are illustrative first drafts; none override authored canon.** Each is deliberately quiet: the emotion sits in the object.

### 9.1 The Reading (Day 360)

> The Chronicle has a button it did not have yesterday. It says PLAY ON, and under it, smaller, SEAL HERE. The first is already highlighted. Whoever wrote the screen did not want to be blamed for either.

### 9.2 The Five Days (Day 361 flavor lines; random-draw pool)

> The stove is warm in the mess. Nobody is eating. Nobody has said what the number means, and nobody is pretending they will.
>
> Someone has written *361* on the roster in pencil, above the row for a person who is no longer in the row. It has been left.
>
> The spring water runs at noon and stops by three. That is not a promise. Write it down as a fact.

### 9.3 The Standing Sheet — sentence table (excerpt; chosen by intersection)

| Intersection | The sentence |
|---|---|
| A · Thriving · Forgiven | *The number is on a chalk board and it is yours; the children in it are not.* |
| A · Hardened · Indentured | *You were counted, and the count was used to bill you.* |
| B · Thriving · Any | *The tone is still running. It has not asked you anything. It is keeping a place.* |
| B · Ghost · Any | *The window is open. There are fewer of you in it than there were.* |
| C · Any · Any | *The invoice will arrive on the 450th day. It will be correct.* |

### 9.4 Market Reading (Standing A, Day 450)

> The registrar sets her scale down and does not look up. "Who is under your roof."
>
> "Fourteen."
>
> "That is a number. Who."
>
> You read the fourteen names. She writes eleven. She looks at you. She writes a twelfth. She does not write the other two, and she does not tell you that she noticed.

### 9.5 The Invoice (Standing C, Day 450)

> QUARTERLY MAINTENANCE RETURN — ALLOCATION 12
> READING DUE: 450. FILED: —.
> Dosimetry, three points. Dwelling census, this address. Relay mast, current at rated tap. Return by hand or by carrier.
> This form is not optional. It does not say what happens next. The next line on the schedule does.

*(The form states obligations and never intent — consistent with the Verdict's "the machine has instructions, and the instructions have a next line.")*

### 9.6 The Open Reading (Standing B, Day 450)

> 99.0. One second on, one second off.
> CENSUS WINDOW: OPEN.
> Not one listener has acknowledged.
> Persons at this address: four more than at the last reading. It does not say which four.

### 9.7 Thirteen — the airlock (Q2)

> The wheel turns. It should not. Somebody has put grease on it that is not candle-wax, not lamp-oil — a light, clean, pale grease with a batch number stamped in the tin lid you find on the sill.
>
> The batch number is from before.
>
> The lamp fitting is new. It is new because the old one is not here; it has been replaced, more than once. There is a little pile of dead ones behind the door, tied in a bundle with wire, as if somebody meant to send them back.

### 9.8 The Council (Q4)

> "She has been on the stores for a quarter," the mentor says. "She has hands. She does not have the voice for it yet. She counts under her breath."
>
> "Under her breath is how I count," says the eldest.
>
> "Then ratify her."
>
> "For a quarter."
>
> Nobody writes the word *only*. It does not need writing.

### 9.9 An outpost keeper's log (Q3)

> Filter at 41. Stove lit. Nine people. One of them is asleep on the floor of the wrong room. The relay has given us nothing since noon. I have not lit the second lamp. If they can see it, they can see it. If they cannot, I would rather not have used the oil.

### 9.10 The night before (Day 720)

> On the roster, in pencil, a new name. It is not yet a name in ink. It will be tomorrow, and the person who wrote it has gone to sleep with the pencil still in her hand.

---

## 10. Endings — the Year Two Chronicle (Day 720)

### 10.1 Structure

The Year Two ending is a **permutation of two outcomes**, coloured by Standing, written on Day 720 (or earlier by terminal condition or an explicit **SEAL HERE**).

**Generations outcome** (from the Council's ledger, ChildDevelopment, ApprenticeshipSystem):

- **`heirs_named`** — at least one acting successor was ratified for at least one quarter and *held*; at least one child has an approaching Rite.
- **`heirs_hurried`** — successors exist but at least one lapsed, or a role was held by an acting successor who was never ratified.
- **`heirs_none`** — no one holds any role but its founder.

**Network outcome** (from OutpostSettlement, Waystation, Journey state):

- **`two_doors_holding`** — Thirteen established and inhabited; every position under watch; no unrelieved overrun.
- **`two_doors_thin`** — Thirteen established; at least one position starving, overrun, or abandoned.
- **`one_door`** — Thirteen never established or abandoned.

### 10.2 The nine titles (working; tone-checked)

| | `two_doors_holding` | `two_doors_thin` | `one_door` |
|---|---|---|---|
| **`heirs_named`** | **The Handing Over** | **What They Carried** | **The Narrow House** |
| **`heirs_hurried`** | **The Night Before** | **A Quarter at a Time** | **The Stack, Alone** |
| **`heirs_none`** | **The Long Table** | **The Last Watch** | **The Sealed Count** |

Each has a **Standing paragraph** (A/B/C) — one line naming what the count became — and a **memorial** paragraph drawn from the existing memorial owner (the fallen of Year Two, by name).

### 10.3 What carries forward

Year Two's Chronicle is **the last thing that seals**. At the seal, *all* existing terminal effects run once (F2): completion history, meta-progression, legacy archive, unified-ending resolver. Cross-run inheritance (`CampaignLegacy`, Plan 140 New Game+) receives Year Two's generational facts as **legacy traits** — the first place the game's inheritance system has something real to inherit.

### 10.4 Chapter Three

Year Two ends *the night before the first Rite*. That is deliberate. It leaves a door open the plan does not walk through: **Chapter Three** (Days 721+), in which the first ratified successors exist. Nothing in this plan builds that chapter. Everything in it leaves the state that chapter would need.

---

## 11. Cast and voice

### 11.1 Existing anchors (LIVE; VERIFY ids and current states at implementation)

| Person | Source | Role in Year Two |
|---|---|---|
| **Nila Brant** — Allocation 11 | `duty_roster_locations.json`, `duty_roster_quests.json` | Keeps four unwritten people; the argument against registration. "Three is a column." |
| **Sela** — a dependent line of Halvard Renn | `holdfast_quests.json`, `duty_roster_quests.json` | The natural first *apprentice*: "engineering, not salvage." Do not turn her into a mascot. |
| **Halvard Renn** — water engineer, 12-B | `holdfast_items.json`, `holdfast_quests.json` | The improvised potable; a mentor whose absence is the mentorship. |
| **Margit Sole** — registrar | `duty_roster_quests.json` | The other pole of *do you write the living*. She is never a villain. |
| **Ansel Duth** — unlisted parent, "one child in the stack" | `characters.json` | "child_not_line_item" is his want. The first Year Two child with a face. |
| **The 14 waystation keepers** | `waystations.json` | §7.8 |
| **The Tempest** | Verdict bible | Never speaks with preference. |
| **The player's roster** | Runtime | **Role-referenced only.** The default starting cohort is one profile of several; nothing in this plan may depend on named starters. |

### 11.2 Proposed cast (`y2_` ids, all new, all small)

| Id | Role | Purpose |
|---|---|---|
| `y2_council_clerk` | A survivor who keeps the Council's minutes | Voice for the *record*; never votes. |
| `y2_thirteens_first_child` | A child born at Thirteen, if any | The first person whose whole life is *on the other side of the road*. Not scripted; a slot filled from real births. |
| `y2_relay_watch` | The relay's long-shift operator | The Q3 night's other voice. |
| `y2_registrar_apprentice` | An apprentice to Sole | The person who has to decide whether to write the living, *after* Sole. |

### 11.3 Voice rules (carried, not changed)

- **The machine never wants.** Its lines read as forms, tones, and counts.
- **The registrar never sneers.** She writes what she is given.
- **The keepers are tired, funny, and specific.** They do not explain their problems; they mention them.
- **The children are not wise.** They are children. They say true things by accident and wrong things on purpose.
- **Idioms** (world bible L1506): "the glass counts you," "spring arithmetic," "the register remembers," "second winter people," "walk the meter line," "signal-fast." Coin sparingly; attach to a speaker; never explain.

---

## 12. Systems map — authority, data, hosting

### 12.1 What is reused (LIVE)

| Concern | Authority (extend, don't replace) | Host seam |
|---|---|---|
| Ending + chapter close | `EndgameSystem`, `EndgameHostSession`, `EndgameSaveStore` | `Main.Endgame.cs`, `ChroniclePanel` |
| The count, lease, evidence | `ReckoningSystem`, `VerdictEndingEvaluator`, Verdict save section | `Main` Verdict hosts |
| Outcome projection | `CampaignOutcomeEvaluator.Evaluate` / `BuildCampaignOutcomeSnapshot()` | `Main.Endgame.cs` |
| Epilogue text | `UnifiedEndingResolver`, `EpilogueMatrixRuntime` | seal path |
| Climate | `YearOfAshTimelineSystem` (extended), `CampaignCalendar` | `YearOfAshHostSession` |
| Children | `ChildDevelopmentSystem`, `GenerationalSystem`, `SecondGenerationMilestoneEngine` | `ChildDevelopmentHostSession`, `SecondGenerationMilestoneHostSession` |
| Apprentices | `ApprenticeshipSystem`, `ApprenticeshipCurriculumEngine` | `ApprenticeshipHostSession`, `ApprenticeshipPanel` |
| Succession facts | `GenerationalLineageExtension`, `GenealogyBridge` | `GenealogyHostSession` |
| Adult age | `AgingSystem`, `SurvivorAgingProgressionEngine` | `AgingHostSession` |
| Outposts | `OutpostSettlementSystem` | `OutpostSettlementHostSession`, `ShelterOperationsPanel` |
| Waystations | `WaystationNetworkSystem` | `WaystationHostSession`, `WaystationNetworkPanel` |
| Expeditions (supply runs) | `ExpeditionSystem` | existing dispatch host |
| Grief, rites | `MemorialSystem`, Reckoning rite trace | existing |
| Heirlooms | `HeirloomSystem`, `PersonalBelongingsSystem` | existing |

### 12.2 What is new (PROPOSED — **state**)

Only three pieces of new state, each nested in an existing section:

1. **Chapter record** — `endgame` section, additive schema bump: `chapterIndex`, `chapters[]` (per-chapter ending id, day, report). *No new section.*
2. **Quarterly reading ledger** — Verdict section, additive: `readings[]` (`quarter`, `day`, `filed`, `tier`). *No new section.*
3. **Council ledger** — inside `apprenticeship` or `genealogy` (owner chosen in Package 4): `designations[]` (`role`, `candidate`, `quarter`, `status`). *No new section.*

Everything else is a **projection** (Standing Sheet, Network view, Year Two Chronicle inputs).

### 12.3 What is new (PROPOSED — **data**, under `Assets/StreamingAssets/Data/`)

| File | Purpose |
|---|---|
| `year_two_chapter.json` | Chapter constants: quarter boundaries, reading days, Standing definitions and sentence tables. |
| `year_two_climate.json` | Timeline phases for Days 361–720. |
| `year_two_endings.json` | The nine permutations and their Standing paragraphs. |
| `year_two_quests.json` | Quest chains (Q1–Q4) using the existing quest loader with `minDay ≥ 361`. |
| `year_two_radio.json` | Radio scripts for readings and network events. |
| `narrative_encounters_year_two.json` | Encounters for roads, waystations, and Thirteen. |
| `outposts.json` (extended) | New positions incl. Thirteen; new `tier`, `supply_mode`, `radio_warning_days` fields. |
| `apprenticeship_catalog.json` (extended) | New mentorship defs; **no** parallel catalog. |

All files: schema-valid, snake_case, through `CatalogIntegrityValidator`. **Presence in JSON is not gameplay reachability** — every catalog gets a consumer and an integration test.

### 12.4 Architecture rules this plan obeys

1. Core (`Assets/Ashfall.Core/`) stays engine-free.
2. JSON is authoritative for authored data.
3. No `System.Random` in deterministic Core; seeded streams forked from the campaign RNG (`CampaignStreamIds`).
4. Every stateful change has a capture/restore path and a legacy-load default.
5. Panels expose commands and state; they own no gameplay.
6. Core events state facts; host adapters apply effects.

---

# PART IV — SAVE, SAFETY, RISK

## 13. Save, determinism, UI, accessibility

**Save.** Three additive nested structures (§12.2). Legacy envelopes (no chapter, no readings, no council) must load as *Chapter One, Dormant Standing, empty ledgers* and behave exactly as today; a save from before this change must play out to Day 360 and offer PLAY ON as a normal player would.

**Determinism.** No wall-clock, no hash-order iteration. Raid pressure, supply-run encounters, and any candidate-success score use `CampaignStreamIds`-forked seeded streams keyed by day and slot. The quarterly reading is a **pure function of persisted state**; it draws no RNG.

**UI.** No new routed panel is required for v1 of any package. The Chronicle panel gains a button and a second page; the Shelter Operations board gains a network tab; the Apprenticeship panel gains a Council page. New panel *registration* touches shared seams (`PanelRegistryBootstrap`, `Main.UiPanels`, `PlayerSurfaceManifest`) and needs an integrator — it is out of every builder package by default.

**Accessibility.** Keyboard/controller close and back preserved; focus restoration on the Council and Chronicle; no color-only state on Network health; contrast at or above the existing AA ratchet; text scaling honored; the Standing Sheet is plain text a screen reader can read in order.

**Tone.** Every new string passes the tone lock. No graphic harm to children — ever. Child loss is *implied by an empty bed and a changed roster line*, never depicted.

## 14. Risk register

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R1 | Play On regresses legacy sealing (F2 side effects run twice, or never). | Med | **High** | Chapter-aware seal guarded by a single `ChapterClosed` fact; run terminal effects only at final seal; regression test that a SEAL HERE at Day 360 is byte-for-byte the legacy path. |
| R2 | Horizon lift breaks Year One behavior (thermal, radon, ice road). | Med | High | Days ≤360 must be **bit-identical** to today; the catalog only applies for `day > 360`. |
| R3 | A 360-day content gap ships as an empty year. | **High** | High | Content waves are separate packages; the mechanism ships dark behind a flag until at least Q1 + Q2 content exists. |
| R4 | Supply runs make outposts unplayable. | Med | Med | Tether mode preserved for legacy outposts (DEC-Y2-06); tuning knob in data. |
| R5 | Real hostile pressure kills Thirteen in the first week. | Med | High | Grace window in data; radio warning day; relief journey; first raid cannot precede a documented tutorial signal. |
| R6 | Three Standings triple content cost. | High | Med | The Standings share 80% of Year Two; only the *instrument*, *problem*, and *door* differ. |
| R7 | Age/clock confusion produces impossible states (a "retired" 30-year-old). | Med | Med | §6.9 fixes authorities; a read-only consistency check in Package 4. |
| R8 | The canon extension (§7.3) collides with quests already authored around Thirteen. | Low | Med | DEC-Y2-01 signature; VERIFY flag ids; feature is additive text. |
| R9 | Scope creep into a full dynasty or colony sim. | Med | High | Explicit non-goals; each package has a small acceptance list. |
| R10 | Player confusion: "did the game end?" | Med | Med | The Reading screen states plainly: PLAY ON opens Chapter Two; SEAL HERE ends the campaign. |

## 15. Decisions needed (full register in the integration plan)

| ID | Decision | Default recommended |
|---|---|---|
| DEC-Y2-01 | Canon extension: Thirteen is maintained by a stale schedule. | **Approve.** |
| DEC-Y2-02 | **REVISED by user 2026-09-29:** the Reckoning day and the Reading day are **per storyline** (Chapter Profiles, §5.7). Default profile keeps 240/360. Chapter = 360 days vs. civil year 365 and the Five Days interlude stand for the default profile. | **User-decided. Adopted.** |
| DEC-Y2-03 | Year Two climate: catalog-extended timeline (not replay, not freeze). | **Approve.** |
| DEC-Y2-04 | Held + deaths > 50: live "sweep advisory" or epilogue-only. | **Epilogue-only in v1.** |
| DEC-Y2-05 | Late Presentation of a held count. | **No in v1.** |
| DEC-Y2-06 | Legacy four outposts stay on tether supply. | **Yes.** |
| DEC-Y2-07 | Where the Council ledger lives (`apprenticeship` vs `genealogy`). | Decide in Package 4's premise audit. |
| DEC-Y2-08 | New Chronicle page vs. new routed panel. | **Page on the existing Chronicle panel.** |
| DEC-Y2-09 | **REVERSED by user 2026-09-29:** Year One's ending selection **may and should differ per storyline** (F3 retired for new campaigns). Saves already in progress load as the legacy profile and stay byte-compatible. | **User-decided. Adopted.** |
| DEC-Y2-12 | Chapter Profile catalog (`chapter_profiles.json`): fields, selection rule, legacy migration, family defaults for the 135 branch endings. | Approve as §5.7; values signed in Package 1B. |
| DEC-Y2-13 | Verdict timing via one boundary adapter (`verdictDay = campaignDay − offset`) instead of rewriting authored verdict day gates. | Approve. |
| DEC-Y2-14 | Standing D (The Late Call) and the evidence-gate waiver. | Approve; default waiver day authored per profile. |
| DEC-Y2-10 | Which existing owner records "who is written" (Standing Record, Voluntary Register, census). | Decide in Package 0's audit. |
| DEC-Y2-11 | `ending_exodus_to_sea` at Day 360: Standing modifier (a chosen part of the roster sails as a friendly port), not terminal, not a deletion. | **Approve.** |

---

## Appendix A — Quick reference: dates

| Day | Event |
|---|---|
| 240 | The Call (verdict resolvable) — **unchanged** |
| 360 | The Reading — Year One Chronicle; **PLAY ON / SEAL HERE** |
| 361–365 | The Five Days — Standing Sheet |
| 366 | Civil New Year (First Thaw) — inside Q1 |
| 450 | Reading Q1 · Council |
| 540 | Reading Q2 · Council |
| 630 | Reading Q3 · Council |
| 720 | Reading Q4 · Council · **Year Two Chronicle** · the night before the first Rite |
| 721 | *Earliest possible coming of age* — Chapter Three's first morning |

## Appendix B — Files read for this audit

`Assets/Ashfall.Core/Verdict/{ReckoningSystem,VerdictEndingEvaluator}.cs` · `Assets/Ashfall.Core/Endgame/{EndgameSystem,EpilogueMatrixRuntime,UnifiedEndingResolver}.cs` · `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` · `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` · `Assets/Ashfall.Core/Survivors/{ChildDevelopmentSystem,GenerationalSystem}.cs` · `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs` · `Assets/Ashfall.Core/{ApprenticeshipSystem,GenerationalLineageExtension}.cs` · `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` · `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs` · `Assets/Ashfall.Core/Waystation/WaystationNetworkSystem.cs` · `src/Main.{Endgame,OutpostSettlement,CampaignOwners,Holdfast}.cs` · `src/Host/{OutpostSettlementHostSession,SecondGenerationMilestoneHostSession,ShelterOperationsHostSession}.cs` · `src/YearOfAsh/YearOfAshHostSession.cs` · `src/UI/ChroniclePanel.cs` · `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` · `Assets/StreamingAssets/Data/{outposts,waystations,endings,verdict_data,life_stages,survivor_life_stages,duty_roster_locations,duty_roster_quests,codex_entries,characters,holdfast_quests}.json` · `docs/endgame/ENDGAME_V1.md` · `docs/expansions/expansion_08_the_verdict_plan.md` · `docs/expansions/wave7/expansion_44_the_outpost_plan.md` · `docs/expansions/wave17/expansion_82_the_far_hearth_plan.md` · `docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` · `docs/shelter/BUNKER_ORIGIN_CONTINUITY.md` · `docs/ashfall-master-world-bible-and-expansion-authority.md` · `docs/plans/PLAN_25_FACTION_ECOLOGY_INTEGRATION_PLAN.md` (Muster/war timing).

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** Year One asked whether the shelter could be kept alive. Year Two asks what
the shelter agrees to *be* — and the program's answer is entirely structural: bunks bound to a
place, readings that disagree, apprentices who are not rushed, a road that costs days, and a last
page that describes instead of judging. The Long Thaw is the least dramatic thing a world can do
to the people in it, and that is why it needs a whole program. The bit-identical legacy promise
licenses all of it: a game that keeps its first promise earns the right to ask its players for a
second year.

**What the expansion leaves lying around.**

> "Calendar page, Day 361. The weather was authored before the day existed; the almanac has no staff."

> "Reading sheet: four voices, one quarter. The sheet is dated. The voices are not named."

> "Twelve bunks and an address. The ledger says Allocation 13. The table says something else."

**Scenes the player may piece together.**

> "The apprentice repaired the pump without asking. It is written in the book, because in ten years the book is what there will be."

> "Day 720. One seal. A paragraph read aloud in a room where nobody flinches — and no second seal, and no appeal."

**Held silences (texture — the register below is unchanged).**

- What the shelter agrees to be. The program's question is structural and its answer is deliberately left to the people at the table.
- Who keeps the quarterly custom after the program stops describing it. Cadence becomes custom in the fiction; the continuation is unauthored and stays so.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**What Standing D — The Late Call — is waiting for.** It is a real grade that resolves into A/B/C.
Until it resolves it is vocabulary the shelter is living inside.

**What happens the night after the last line.** The program ends at Day 720 with a single seal. No
Chapter Three. The silence after the last line is the point.

**What the first Rite of Passage would have been.** The chapter stops **the night before** it, by
design. Writing the rite would spend the program's best silence.

**Is the second shelter a return or a departure.** Twelve bunks bound to a real location and stopped
there. Custody of meaning is not asserted.

**Why four quarterly readings in four voices.** Four voices, never reconciled. The disagreement is
the instrument.

**What the Reckoning was.** A boundary the game crosses and a day the player chose. Its meaning in
the world's mechanics, as opposed to the shelter's story, is never authored.
