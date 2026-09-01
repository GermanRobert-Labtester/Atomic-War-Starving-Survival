# Faction War Spine Audit (Plan 25 · 25C.1)

> Verified 2026-09-01. Plan 25 surrounds this spine; it never rewrites it.

## 1. Simulation layer — `FactionWarSystem`

`Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` (203 lines). State `FactionWarSystemState` (L18–25): `factions: List<FactionStandingRecord>` (factionId, `standing` −100..+100, `territorialControlPercent`, `isHostile` ≤−50, `isAllied` ≥+50), `activeWarTension` 0–100, `dominantFactionId`, `enactedDecrees`, `totalArtilleryStrikesLogged`.

- **No war-active/ended flag, no participant pairing, no outcome enum, no weariness.** "Phases" live in `YearOfAshTimelineSystem.cs:6-10` (`Phase4_DeepFreeze` 180–240, `Phase5_FactionSiege` 241–300, `Phase6_TheGreatThaw` 301–360); content-side `band` strings in JSON (`cold_war`, `open_conflict`, `the_offensive`, `culmination`) are data-only.
- API: `GetStanding` (52), `ModifyStanding` (58, clamps, flips hostile/allied), `EnactDecree` (77, +15 tension), `SimulateDailyFriction(day)` (87 — **no-op ≤ day 240**, then +1 tension/day, territorial clash every 15 days).
- Events: `OnFactionStandingChanged`, `OnDecreeEnacted`, `OnTerritorialClashOccurred`. Zero RNG (pure day-modulo arithmetic).
- Default factions (L109–135): `faction_central_garrison`, `faction_rebuilders`, `faction_black_ops`, `faction_ash_sign`, `faction_hydro_barons`, `faction_forward_roster`. **Note: only Hydro Barons overlaps the four Muster factions** — the war's belligerents are mostly distinct from the Muster gathering's factions.
- Save: inside `year_of_ash` envelope v4 (`YearOfAshSave.factionWar` + `factionWarChainRunner`), `src/YearOfAsh/YearOfAshHostSession.cs:19,144-151,260-272`, `src/Main.YearOfAsh.cs:84`.

## 2. Content chain — `FactionWarChainRunner`

`Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` (476 lines). `SystemId = "faction_war_chain_runner"`.

- **Trigger grammar (closed set, L20–108):** `PlayerVisitedTrigger`, `ChainResolvedTrigger`, `DayOffsetTrigger(offset, params stageIds)`, `AndTrigger`, `AlwaysTrigger`. **No flag/standing/tension/treaty trigger node exists** (Plan 25 adds exactly one: `FlagTrigger`).
- `FactionWarTriggerTable.ByStageId` (L119–198) maps **every stageId explicitly** — class doc mandates manual extension; `FactionWarChainRunnerTests.TriggerTable_HasAnExplicitEntryForEveryStageInTheCatalog` fails on any new stage without an entry.
- `TickDay(day)` (321) surfaces stages gated only by `minDay` (`currentDay < stage.minDay → null`, L305); auto-advances zero-choice stages. `ResolveChoice` (349) applies `moraleDelta` and follows `leadsToStageId` (empty = chain resolved). Events: `OnStageSurfaced`, `OnStageResolved`, `OnChainResolved`.
- Save: `FactionWarChainRunnerState` (schemaVersion 1: chains, visitedLocations, cumulativeMoraleDelta) inside the same `year_of_ash` v4 envelope.

## 3. Authored content — `faction_war_events.json`

`Assets/StreamingAssets/Data/faction_war_events.json` (1224 lines, `schema_version: 1`). 22 chains / 45 stages. Stage DTO: `{stageId, minDay, triggerCondition (authored prose only), title, bodyText, choices[]}`; choice: `{choiceId, text, moraleDelta, leadsToStageId}`. **No maxDay, no preconditions, no flag production, no standing effects anywhere.**

All chains (band, earliest minDay):

| Band | Chains (day) |
|---|---|
| cold_war | `evt_d480_grain_tally_dispute` (480), `evt_d485_checkpoint_notice_war` (485), `evt_d488_manifest_holdup` (488), `evt_d491_toll_hike` (491), `evt_d495_the_clean_strike` (495) |
| open_conflict | `evt_d503_conscription_lists` (503), `evt_d509_border_clash_span44` (509), `evt_d517_almshouse_shelling` (515), `evt_d522_switchback_toll` (522), `evt_d524_market_price_spike` (524) |
| the_offensive | `evt_d533_garrison_offensive_grain_silo` (533), `evt_d541_evacuation_window_plaza` (541), `evt_d545_ration_plaza_strike` (545), `evt_d552_rebuilders_fracture` (552), `evt_d558_ln74_signal_intercept` (558) |
| culmination | `evt_d565_hydro_leverage_break` (565), `evt_d570_forward_roster_first_action` (570), `evt_d578_shrine_strike_anomaly` (576), `evt_d583_d9_reassessment` (583), **`evt_d588_ceasefire_by_exhaustion` (588)**, `evt_d600_theory_surfaces` (600), `evt_d605_post_ceasefire_forward_roster` (605) |

**Pre-war and war-weariness categories do not exist** (earliest band is day 480; zero `weariness` strings repo-wide). The chain fires in extended campaigns (calendar has no day cap; day 360 is the epilogue-matrix view, not a gate).

## 4. What Plan 25 adds around the spine (no rewrites)

1. 6 pre-war escalation chains (E-P1..P6, authored ~minDay 200–300) — gated on Plan 25 grievance flags via the new `FlagTrigger`; they precede the existing friction ramp (day 240) narratively and feed tension context.
2. 6 mid-war chains (E-W1..W6) — gated via **existing** `ChainResolvedTrigger`/`DayOffsetTrigger` on real battle stages (e.g. refugees after `evt_d509_*` resolves); no new trigger types needed here.
3. 4 war-weariness chains (E-R1..R4, culmination band, minDay 565–588) — produce `flag_peace_*` pressure flags consumed by the Muster path evaluator and epilogue hooks; they do NOT end the war (the authored ceasefire at 588 remains the owner).
4. Optional stage/choice fields: `requires_flag`, `produces_flag`, `standing_delta` (choice) — additive DTO fields, applied through existing APIs.
5. Every new stageId gets an explicit `FactionWarTriggerTable` entry (totality test stays green).

## 5. War→Muster linkage (today: none)

`MusterSystem` triggers day-only (`MusterOpeningDay = 260`, `MusterSystem.cs:222`); zero `FactionWar*` references in `Assets/Ashfall.Core/Muster/`. Plan 25's `MusterPathEvaluator` is the **only** sanctioned bridge: it reads war/treaty/standing/flag state as inputs and writes one derived field (`MusterState.musterPath`); it never mutates war state.
