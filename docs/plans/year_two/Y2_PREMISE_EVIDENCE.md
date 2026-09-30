# ASHFALL — Year Two: The Long Thaw (Days 361–720)
# Premise Audit & Reality Evidence (Package P0)

**Date:** 2026-09-30
**Authority:** `.ai/plans/year-two-the-long-thaw-2026-09-29.md` (umbrella §5, card P0)
**Parent Plan:** `.ai/plans/y2-p0-premise-audit-2026-09-29.md` (`STATUS: APPROVED BY USER`)
**Companion Prose:** `docs/expansions/expansion_year_two_the_long_thaw_plan.md` §3
**Auditor Role:** Sweep + Foreman Foundation (Static verification against current repository source, data, and tests)
**Verification Standard:** AGENTS.md Rule 7 (Current Evidence Authority) — zero speculative claims; exact file paths and line numbers verified.

---

## 1. Executive Summary

This audit establishes the empirical ground for **Year Two: The Long Thaw (Days 361–720)** before any code changes are introduced.
All 21 premise facts (**F1–F21**) have been forensically re-verified against the active worktree. All 21 are **CONFIRMED** with exact current line numbers and file paths (including path corrections where prior documentation drifted).
Zero premise contradictions or blocking discrepancies were found. The path is open for Package P1 (Horizon Lift) and subsequent packages.

---

## 2. Premise Evidence Table (F1–F21)

| ID | Finding | Verdict | Current Evidence (Path:Line) | Architectural Consequence for Year Two |
|---|---|---|---|---|
| **F1** | Host ends campaign chapter at Day 360 by construction (`living == 0 \|\| day >= 360` calls `TriggerEnding`; `ChroniclePanel` offers `SEAL CAMPAIGN & FREEZE ARCHIVE`). | **CONFIRMED** | `src/Main.Endgame.cs:222-240`<br>`src/UI/ChroniclePanel.cs:143, 197` | Package P2 (`Play On`) must intercept this trigger to offer Chapter 2 continuation alongside final sealing. |
| **F2** | Sealing runs heavy terminal side effects: terminal `SaveAll`, unified ending rewrites epilogue prose, generational legacy archived, completion history appended (append-only), meta-progression recorded. | **CONFIRMED** | `src/Main.Endgame.cs:72-180` (`OnCampaignSealed`) | Only the *final* seal (at Day 720 or extinction) may execute these side effects. Day 360 `Play On` must bypass terminal completion archiving. |
| **F3** | Host ending context is thin: sets `CurrentDay`, `LivingSurvivors`, `DeceasedSurvivors`, `AverageMorale`, `ExpeditionsCount`, `ForceExtinction`; omits `TruthBroadcasted`, `VassalageAccepted`, `DominantFaction`, `ForceWinterFailure`. Only 3 of 8 authored endings are reachable via host trigger. | **CONFIRMED** | `src/Main.Endgame.cs:228-236`<br>`Assets/Ashfall.Core/Endgame/EndgameSystem.cs:117-151` | The richer projection already exists in `BuildCampaignOutcomeSnapshot()` (`src/Main.Endgame.cs:145`). Year Two's Standing reads this richer snapshot instead of the thin context. |
| **F4** | `YearOfAshTimelineSystem.AdvanceDay` clamps at `EndDay = 360` and ignores later days; its temperature feeds thermal, radon, and ice road systems. | **CONFIRMED** | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs:37, 63-65, 170`<br>`src/YearOfAsh/YearOfAshHostSession.cs:170-175` | **Blocker for Days 361+.** Days 361+ freeze at Day 360 values (+4 °C). Package P1 (`Horizon Lift`) must provide catalog-backed climate data for Days 361–720. |
| **F5** | 360 horizon appears across core systems and data as clamps, defaults, or content windows (`QuestlineSystem.maxDay`, `DoorEncounterSystem.maxDay`, `YearOfAshCatalogLoader.maxDay`, `DoseContentCatalog.maxDay`, `CampaignCalendar`, `SeasonalCelebrationSystem`). | **CONFIRMED** | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs:79`<br>`Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs:79`<br>`Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:116`<br>`Assets/Ashfall.Core/DoseContentCatalog.cs:43`<br>`Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:201-209` | Classified in Section 3 below. Only true runtime clamps require lifting; content windows remain bounded by design. |
| **F6** | Authored content beyond Day 360 is almost non-existent in core content files (11 entries in `events.json` at 365, 8 in `quests_npc_arcs.json` at 365, 5 in `radio.json` at 365, 1 in `nuclear_winter_phases.json` at 361, 1 in `narrative_discovery_manifest.json` at 390; faction war entries use 300-day offset). | **CONFIRMED** | `Assets/StreamingAssets/Data/events.json`<br>`Assets/StreamingAssets/Data/quests_npc_arcs.json`<br>`Assets/StreamingAssets/Data/radio.json`<br>`Assets/StreamingAssets/Data/nuclear_winter_phases.json` | Year Two is an authored greenfield content space (Package P8). It is built via genuine content rather than fake extrapolation. |
| **F7** | Child development stages: Infant <60, Toddler <180, Child <500, Adolescent <720, YoungAdult >=720 days. Canonical age floored at birth day 1. Earliest coming-of-age is Day 721. | **CONFIRMED** | `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs:98-124` | Year Two spans childhood and apprenticeship; the first Rite of Passage falls naturally on the eve of adulthood at Day 720. |
| **F8** | Second-generation milestone ladder exists: `first_words`, `foundational_letters`, `tool_handling`, `field_survey`, `vocational_apprenticeship`, `rite_of_passage`, `succession_readiness`. Ticked by `SecondGenerationMilestoneHostSession.TickAll`. | **CONFIRMED** | `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs:8-18, 54`<br>`src/Host/SecondGenerationMilestoneHostSession.cs:13-60` | The milestone ladder is already operational in Core and host. Package P4 reuses this owner and adds no parallel ladder. |
| **F9** | `ApprenticeshipSystem` and `GenerationalLineageExtension.PerformSuccession(retiree, successor)` both exist and are host-wired via `ApprenticeshipHostSession` and `GenealogyHostSession`. | **CONFIRMED** *(path correction: Core root, not Survivors/)* | `Assets/Ashfall.Core/ApprenticeshipSystem.cs:89`<br>`Assets/Ashfall.Core/GenerationalLineageExtension.cs:64`<br>`src/Host/ApprenticeshipHostSession.cs:1-60`<br>`src/Host/GenealogyHostSession.cs:1-120` | Primitives for mentorship, will execution, and generational succession are fully wired. Package P4 provides the Council governance layer over them. |
| **F10** | Outpost / Waystation / Colony custody is signed and separate: `OutpostSettlementSystem` (4 authored outposts in `outposts.json`, save section `outpost_settlement`), `WaystationNetworkSystem` (14 waystations), `ColonySystem` (player colonies, save section `colony`). | **CONFIRMED** | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:267, 284, 427, 600`<br>`src/Main.OutpostSettlement.cs:20-60`<br>`docs/plans/ORPHAN_SEAL_PRIORITY_W1_BOUNDARIES.md` §2 | Custody boundaries strictly preserved. Thirteen belongs to `OutpostSettlementSystem` as a secondary hearth outpost; `ColonySystem` remains independent. |
| **F11** | Outposts are never attacked in play: `WorldDangerRatingForDay(day)` returns `0` unconditionally, bypassing risk simulation. | **CONFIRMED** | `src/Main.OutpostSettlement.cs:280-287` | Package P6 replaces the stub with a truthful danger source derived from warlord doctrine, weather, and faction friction. |
| **F12** | Outposts are supplied for free daily from central inventory without physical journey or distance cost (`centralRationSupplyProvider` pulls items directly). | **CONFIRMED** | `src/Main.OutpostSettlement.cs:216, 234-246` | Package P6 introduces supply runs via expedition routes to make distance and logistics meaningful. |
| **F13** | `outposts.json` `graph_node_id`s (`node_north_ridge`, `node_rail_junction`, `node_radio_tower`, `node_deep_quarry`) are unreferenced by map catalogs, and integrity validator checks costs only. | **CONFIRMED** | `Assets/StreamingAssets/Data/outposts.json:6, 17, 28, 39`<br>`Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs:33` | Package P5 retro-binds the four outposts to real `locations.json` location nodes. |
| **F14** | Established outposts can be re-established to clear `IsOverrun`, but no dedicated relief journey command with unique cost/risk exists. | **CONFIRMED** | `Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs:196, 221, 377` | Relief must be a dispatched journey through the expedition owner rather than a button reset. |
| **F15** | The Verdict's three endings (Recounts, Held, Lease) carry operational hooks for Year Two; `VerdictEndingEvaluator.IsTempestDecommissioned` is true exactly when the sector recounts. | **CONFIRMED** | `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs:17-41, 65-69` | These three outcomes directly map to Standing A (Accepted as Read), Standing B (The Window), and Standing C (The Quarter), establishing the four quarterly readings. |
| **F16** | The lore already authored the second shelter: Allocation 11 (`loc_overflow_alloc_11`, Nila Brant), Allocation 13 (`loc_overflow_alloc_13`), Pump Hatch (`loc_overflow_pump_hatch`), and The Blank Cellar (`loc_overflow_blank_cellar`, "DO NOT WRITE THE LIVING"). | **CONFIRMED** | `Assets/StreamingAssets/Data/duty_roster_locations.json:125-171` | Thirteen is not invented; it is re-opened in continuity with existing lore. |
| **F16b** | Quest choice for the chart in `duty_roster_quests.json` sets live flags: `mutation_schedule_refused` ("Leave it. Access holds."), `mutation_roster_blank` ("Rewrite. Kindness."), `mutation_schedule_living` ("Copy it for Sole. Completeness opens 13 to a file and closes 11 to you."). | **CONFIRMED** | `Assets/StreamingAssets/Data/duty_roster_quests.json:905, 910, 915` | Year Two reads these existing flags to determine initial access and relationship with Thirteen and Allocation 11. |
| **F17** | Expansions 44, 46, 82, and 12 explored adjacent concepts. Expansion 82's premise ("no save section for Colony") is stale (`colony` is in `SaveSectionRegistry`). | **CONFIRMED** | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:267, 427` | Historical proposals serve as design reference only; signed custody decisions govern. |
| **F18** | Faction branching is massive: 45 authored branches (15 Military, 15 Rebel, 15 Independent), 3 endings each (135 total endings), point-of-no-return flags, and `OnEndingResolved` events managed by `FactionBranchCoordinator`. | **CONFIRMED** | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:68-130`<br>`Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs:45, 180`<br>`Assets/Ashfall.Core/Factions/RebelBranchSystem.cs:39, 163`<br>`Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs:223` | Branch endings serve as rich Year One closure states that feed storyline chapter profiles in Package P1B. |
| **F19** | Reckoning phase days are `const` inside `ReckoningSystem` (`KnowingDay 160`, `CulpableDay 210`, `CountedDay 240`); no other code reads them, and `Poll` compares raw campaign day. | **CONFIRMED** | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs:57-59, 91, 95, 106` | Storyline-specific Reckoning timing requires only a lightweight adapter (`ReckoningClock`) translating campaign day to verdict day without mutating authored data. |
| **F20** | Verdict can be unresolved at Day 360 (`Poll` requires evidence to advance Knowing -> Culpable; `DecideEnding` returns null below Counted). | **CONFIRMED** | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs:94-97`<br>`Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs:53-54` | A campaign can reach the Day 360 Reading without having resolved the Call. This validates **Standing D — The Late Call** as a legitimate fourth standing. |
| **F21** | `UnifiedEndingContext` carries `factionBranchId`, `musterApproachId`, `holdfastEndingId`, `verdictEndingId`; populated in `src/Main.UnifiedEnding.cs`. | **CONFIRMED** | `src/Main.UnifiedEnding.cs:85-100`<br>`Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs:20-60` | Multi-authority ending resolution is already wired. Package P1B can route Year One endings through this structure. |

---

## 3. Horizon 360-Day Census Table

A forensic scan of every 360-day constant and data boundary across the repository yields the following taxonomy:

| File & Location | Constant / Syntax | Classification | Context & Architectural Treatment |
|---|---|---|---|
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs:37, 64, 170` | `public const int EndDay = 360;`<br>`if (day > EndDay) day = EndDay;` | **RUNTIME CLAMP** | **Must lift in P1.** Extends timeline for `day > 360` by delegating to `YearTwoClimateCatalog`. Days <= 360 remain byte-identical. |
| `src/Main.Endgame.cs:222` | `if (living == 0 \|\| day >= 360)` | **RUNTIME CLAMP** | **Must lift in P2.** Gated by chapter threshold: offers `Play On` and `Seal Here` on Day 360; triggers final seal on Day 720. |
| `Assets/Ashfall.Core/Endgame/EndgameSystem.cs:148` | `if (ctx.CurrentDay >= 360) return GetEndingOrDefault("ending_dawn_of_thaw");` | **RUNTIME CLAMP** | **Must lift in P1B / P2.** Replaced by profile-aware ending evaluation when continuing into Chapter 2. |
| `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:201-209` | `if (dayInYear <= 360)`<br>`float endT = (dayInYear - 360) / 5.0f;` | **RUNTIME CLAMP** | **Handled in P1 (DEC-Y2-03).** Reconciles 360-day chapter with 365-day civil calendar; Days 361–365 form the Five Days interlude. |
| `Assets/Ashfall.Core/Events/SeasonalCelebrationSystem.cs:207, 376` | `(Math.Max(1, day) - 1) % 360 + 1`<br>`occurrence / 360` | **RUNTIME CLAMP** | Modulo arithmetic for annual celebrations. Naturally recurs every 360 days in Year Two without modification. |
| `Assets/Ashfall.Core/Spiritual/SpiritualRitualCalendarEngine.cs:63` | `public const int DaysPerYear = 360;` | **RUNTIME CLAMP** | Annual ritual calendar cycle. Modulo cycles handle Year Two automatically. |
| `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs:79` | `public int maxDay = 360;` | **CONTENT WINDOW** | Authored Year One door encounters expire at 360. Year Two introduces dedicated encounters (Package P8). Do not mutate Year One catalog. |
| `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs:79` | `public int maxDay = 360;` | **CONTENT WINDOW** | Year One questlines conclude at 360. Year Two quests enter via their own catalog in P8. |
| `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs:1074` | `maxDay = 360` | **CONTENT WINDOW** | Specific coastal radio operator questline ends at Day 360. Content constraint; leave intact. |
| `Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs:408` | `EndDay = 360` | **CONTENT WINDOW** | Progression curve envelope for initial nuclear winter phase. |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs:116` | `public int maxDay = 360;` | **INERT DEFAULT** | Deserialization default when JSON omits `max_day`. Retain as fallback for legacy data. |
| `Assets/Ashfall.Core/DoseContentCatalog.cs:43` | `public int maxDay = 360;` | **INERT DEFAULT** | Default upper bound for radiation story events. Retain as legacy fallback. |
| `Assets/Ashfall.Core/Performance/WorkloadProfile.cs:71-73` | `Days360 = campaignDays: 360` | **INERT DEFAULT** | Benchmark profile constant for headless stress testing. Independent of gameplay logic. |
| `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs:18, 185`<br>`Assets/Ashfall.Core/World/WeatherSystem.cs:199`<br>`Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs:19, 320` | Angular math `% 360f` | **GEOMETRIC CONSTANT** | Azimuth and bearing angle calculations in degrees (0–360°). Completely unrelated to campaign calendar. |
| `Assets/Ashfall.Core/Settings/UserSettingsCodec.cs:144` | `if (data.MaxFps < 0 \|\| data.MaxFps > 360)` | **SETTING CLAMP** | Monitor refresh rate / framerate ceiling clamp. Unrelated to calendar. |

---

## 4. Named Subsystem Host Owners & Integration Seams

To satisfy AGENTS.md Rule 5 (One authority per concern, extend existing owners), the relevant host owners and files are formally identified:

1. **Verdict Host Session & Seam:**
   - **Core Engine:** `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`
   - **Host Session:** `src/Host/VerdictHostSession.cs` (constructed in `src/Main.Verdict.cs`)
   - **Poll Call Site:** `src/Host/VerdictHostSession.cs:164`: `Reckoning.Poll(day, livingCount, logReadCount, Evidence.Count)`
   - **Save Store:** `src/Host/VerdictSaveStore.cs` (`verdict` save section)
   - **UI Dashboard:** `src/UI/VerdictDashboardPanel.cs` and `src/VerdictPanel.cs`
   - **Seam for P1B:** Wrap the day passed to `Poll` via `ReckoningClock.ToVerdictDay(campaignDay, profile)`.

2. **Survivor Role & Duty Assignment Owner:**
   - **Core Engine:** `Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs`
   - **Host Session:** `src/Host/SurvivorRoleHostSession.cs` (constructed in `src/Main.SurvivorRoles.cs`)
   - **Duty Roster Seam:** `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` via `src/Host/DutyRosterHostSession.cs` (`src/Main.DutyRoster.cs`)
   - **Save Section:** `survivor_roles` (`survivor_roles_save.json`)

3. **Expedition Dispatch Host Seam:**
   - **Host Session:** `src/Host/ExpeditionHostSession.cs`
   - **Dispatch Methods:** `DispatchSortie` (`src/Host/ExpeditionHostSession.cs:980`) and `StartExpedition` (`:600`)
   - **Main Wiring:** `src/Main.Expeditions.cs:231, 642`
   - **Seam for P6:** Supply runs to outposts dispatch through `ExpeditionHostSession.DispatchSortie` carrying dedicated supply manifests.

4. **Registration ("Who Is Written") Authority:**
   - **Core Engine:** `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs`
   - **Host Session:** `src/Host/VoluntaryRegisterHostSession.cs` (wired in `src/Main.VoluntaryRegister.cs`)
   - **Save Section:** `voluntary_register` (`voluntary_register_save.json`)
   - **Alternative/Complementary Read Model:** `StandingRecordEngine` (`Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs`) via `src/Host/StandingRecordHostSession.cs`
   - **Verdict for DEC-Y2-10:** Extend `VoluntaryRegisterSystem` for the binary command ("write / leave unwritten") as it is already the sovereign authority on registered shelter dwellers.

5. **F16b Quest Flag Identifiers:**
   - Location: `Assets/StreamingAssets/Data/duty_roster_quests.json:904-916`
   - `mutation_schedule_refused`: "Leave it. Access holds." (Allocation 11 remains open, Thirteen unfiled).
   - `mutation_roster_blank`: "Rewrite. Kindness. The hatch will not open."
   - `mutation_schedule_living`: "Copy it for Sole. Completeness opens 13 to a file and closes 11 to you." (Thirteen filed in register, Allocation 11 closed).

6. **Outpost Retro-Binding Location Identifiers:**
   - Verified candidate locations from `Assets/StreamingAssets/Data/locations.json`:
     - `outpost_north_watch` (`node_north_ridge`) -> `loc_wind_gap_ridge`
     - `outpost_rail_depot` (`node_rail_junction`) -> `loc_settlement_nine_rails`
     - `outpost_high_peak` (`node_radio_tower`) -> `loc_summit_relay`
     - `outpost_deep_quarry` (`node_deep_quarry`) -> `location_quarry_overlook`
     - `outpost_thirteen` (Thirteen, secondary hearth outpost) -> `loc_overflow_alloc_13` (defined in `duty_roster_locations.json:137`)

7. **Census of Verdict Day-Gate Consumers:**
   - `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs:91, 95, 106, 152, 165` (Phase transitions, `CanAccessPilotTone` day >= 210, `SelectEnding` phase >= Counted).
   - `Assets/Ashfall.Core/Verdict/VerdictAccusationSystem.cs:153` (`_reckoning.Phase < ReckoningPhase.Culpable` accusation gate).
   - `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:52, 55` (`Poll(day, phase)` requires `phase >= ReckoningPhase.Culpable`).
   - `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs:53` (`state.phase < ReckoningPhase.Counted` ending fallback gate).
   - `Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs:305` (`input.VerdictReckoningState.phase >= ReckoningPhase.Counted`).
   - `src/Host/VerdictHostSession.cs:164, 234` (`Reckoning.Poll`, `Reckoning.Phase < ReckoningPhase.Culpable`).
   - `src/UI/VerdictDashboardPanel.cs:41, 50, 58` (UI phase change subscription).
   - `src/VerdictPanel.cs:168-177` (Phase readout rendering).

---

## 5. Draft Storyline Chapter Profiles Table (For P1B Signature)

The user's 2026-09-29 mandate overrules a single universal Reckoning day: each storyline family receives its own timing profile while preserving legacy compatibility.

| Profile ID | Storyline Family | Knowing Day | Culpable Day | Counted Day (The Call) | Reading Day (Chapter Close) | Year One Ending Source | Standing Default |
|---|---|---|---|---|---|---|---|
| `profile_base_v1` | Legacy / Base Holdfast | Day 160 | Day 210 | Day 240 | Day 360 | `legacy_context` (`EndgameSystem.EvaluateEnding`) | Derived from Verdict flags (A/B/C) |
| `profile_military` | Military Faction Branches | Day 145 | Day 195 | Day 225 | Day 360 | `faction_branch` (Active Military ending) | Military Command Modifier |
| `profile_rebel` | Rebel Faction Branches | Day 170 | Day 220 | Day 250 | Day 360 | `faction_branch` (Active Rebel ending) | Insurgency Autonomy Modifier |
| `profile_independent` | Independent Faction Branches | Day 160 | Day 210 | Day 240 | Day 360 | `faction_branch` (Active Independent ending) | Free Trade League Modifier |
| `profile_muster` | Muster / Frontier Expedition | Day 180 | Day 240 | Day 270 | Day 360 | `muster_approach` | Frontier Expedition Modifier |
| `profile_standing_d` | Unresolved Verdict / Deferred Call | Day 160 | Day 210 | Day 390 (Year 2) | Day 360 | `verdict_late_call` | **Standing D — The Late Call** |

---

## 6. Proposed Ownership Claims for P1, P1B, and P2

Per `WORKTREE_OWNERSHIP.md` and AGENTS.md Rule 6, the exact file boundaries for subsequent implementation packages are designated:

### Package P1 — Horizon Lift
- **Claim ID:** `claim-year-two-p1-horizon-lift-2026-09-30`
- **Core Files:**
  - `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs`
  - `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs` (additive)
  - `Assets/Ashfall.Core/YearOfAsh/YearTwoClimateCatalog.cs` (new loader)
- **Data Files:**
  - `Assets/StreamingAssets/Data/year_two_climate.json` (new authored 4-quarter climate data)
- **Host Files:**
  - `src/YearOfAsh/YearOfAshHostSession.cs`
- **Integrator-Shared (`INT`):**
  - `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` (delegation only)
- **Test Target:**
  - `Ashfall.Core.Tests/YearOfAsh/YearTwoHorizonTests.cs` (new)

### Package P1B — Storyline Chapter Profiles
- **Claim ID:** `claim-year-two-p1b-chapter-profiles-2026-09-30`
- **Core Files:**
  - `Assets/Ashfall.Core/Endgame/ChapterProfileCatalog.cs` (new)
  - `Assets/Ashfall.Core/Endgame/ChapterProfileResolver.cs` (new, pure)
  - `Assets/Ashfall.Core/Verdict/ReckoningClock.cs` (new adapter)
  - `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` (additive profile thresholds)
  - `Assets/Ashfall.Core/Endgame/EndgameSystem.cs` (profile id in state, additive)
- **Data Files:**
  - `Assets/StreamingAssets/Data/chapter_profiles.json` (new)
- **Host Files:**
  - `src/Main.UnifiedEnding.cs` (co-sign with integrator)
  - `src/Host/VerdictHostSession.cs`
- **Test Target:**
  - `Ashfall.Core.Tests/Endgame/ChapterProfileTests.cs` (new)
  - `Ashfall.Core.Tests/Verdict/ReckoningClockTests.cs` (new)

### Package P2 — Play On
- **Claim ID:** `claim-year-two-p2-play-on-2026-09-30`
- **Core Files:**
  - `Assets/Ashfall.Core/Endgame/EndgameSystem.cs` (`ContinueChapter`, threshold evaluation)
  - `Assets/Ashfall.Core/Endgame/EndgameSaveState.cs` (schema v2, `chapters[]`)
- **Host Files:**
  - `src/Host/EndgameHostSession.cs`
  - `src/Host/EndgameSaveStore.cs`
  - `src/Main.Endgame.cs` (`INT` co-sign)
  - `src/UI/ChroniclePanel.cs` (`PLAY ON` and `SEAL HERE` buttons)
- **Data Files:**
  - `Assets/StreamingAssets/Data/year_two_chapter.json` (new)
- **Test Target:**
  - `Ashfall.Core.Tests/Endgame/YearTwoPlayOnTests.cs` (new)

---

## 7. Audit Acceptance Sign-Off

The premise audit is complete. All 21 evidence premises are confirmed, the 360-day census is categorized, host seams and owners are located, and storyline profiles are drafted.
Package P0 is certified ready for formal sealing.
