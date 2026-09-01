# Plan 38 — The Year Turns: Seasons, Deadlines, and a Clock With Teeth

> **Wave:** Continuity Wave 5 — *The Human Interface*
> **Depends on:** 20C (weather effects table — seasons must bias it, not replace it), 31
> (transitions must be reportable), 35 (seasonal production/perishability), 30 (the outside world
> has a calendar too).
>
> **Theme:** the game counts days, seasons, chapters, and years — and only the weather generator
> notices. `GetSeasonForDay` has exactly one consumer outside the weather system itself (a panel
> label). The generational chapter/year clock appears only inside status strings
> (`"Chapter {n} · Year {m}"`). The greenhouse reads no season, no temperature, no day-of-year.
> "Ice roads" exist as duty-roster flavour text while the day-advance handler is literally named
> `OnTickIceRoadClicked`. A survival game whose premise is a *nuclear winter* has no winter.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | A season model exists | `Assets/Ashfall.Core/World/WeatherSystem.cs:10–31` — `SeasonWindowDef`, `SeasonProfileDef { id = "default_winter", displayName = "The Long Winter", seasons[] }`; `:83 BindProfile(profile, seed)`, `:89 GetSeasonForDay(int day)` |
| 2 | The authority is tiny but real | `weather_seasons.json`: `schema_version`, `id`, `displayName`, `weatherCheckIntervalHours`, **3** windows (`window_early` "First Thaw" …) each with per-`WeatherKind` weights (`clearWeight`, `rainWeight`, `ashfallWeight`, `falloutStormWeight`, `blizzardWeight`, `blackRainWeight`) |
| 3 | **Only display and weather sampling read seasons** | `GetSeasonForDay` consumers → `src/UI/WeatherPanel.cs:184` plus test/demo/harness constructions (`SevenDayDeterministicSmokeTest.cs:512`, `WorldHeadlessDemo.cs:38`, `PerformanceCampaignHarness.cs:156`). Nothing else |
| 4 | **The year clock is cosmetic** | `Generational.CurrentChapterIndex` / `TotalYearsElapsed` appear only in status strings: `src/Host/ExpansionHostSession.cs:389–404`, `src/UI/ExpansionsHubPanel.cs:384`, `src/UI/CenturySeedPanel.cs:173,175` |
| 5 | **Greenhouse has no season input** | `grep -E "season\|cold\|winter\|dayOfYear\|temperature" Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` → **0 matches**, though `GreenhouseExpansionCatalog.cs:75 LightHoursPerDay` and crop blights (`:46 BlightOutbreak`) exist — the season is exactly what should drive them |
| 6 | Thermal and dose are season-adjacent but not season-aware | `ShelterThermalSystem.cs` models room temperature and pipe wear (`:281 currentTempC < 0f && condition < 50f`); `YearOfAshHostSession.TickDay` already computes `_timeline.AmbientTemperatureCelsius` and passes it to `_deepFreeze.TickDailyThermal` / `_radon.TickDailyRadon` — the ambient-temperature plumbing exists in one expansion only |
| 7 | Ice roads are text | `IceRoad`/`ice_road` matches only in `DutyRoster/*` (`DutyRosterCatalog.cs`, `DutyRosterHoldfastBridge.cs`, `DutyRosterSystem.cs`) and three headless demos — while the day-advance entry point is `src/Main.Holdfast.cs:175 OnTickIceRoadClicked` |
| 8 | One real deadline exists and it works | Warlord tribute: `src/Main.YearOfAsh.cs:54–78` — `SettleWarlordTribute(amount, day, out next)` with a `next` due day, a collector line for paid/short/refused, and `_warlord.TickDaily(day, rng, context)` (`YearOfAshHostSession.cs:175`) with raid assignment downstream — the **model** for all other deadlines |
| 9 | Seasonal content is already authored elsewhere | `duty_roster_seasons.json` is loaded (`DutyRosterCatalog.cs:126 SeasonsFile`) — one subsystem already treats seasons as data; `piagentsplans/77-duty-roster-seasons-expansion` and `83-weather-seasons-expansion` exist as backlog |
| 10 | Wildlife and migration exist to be seasonal | `WildlifeMigrationSystem` is ticked under `world_evolution` (Wave 4's evidence), so a seasonal wildlife signal already has a consumer waiting |
| 11 | Multi-year continuity landed but is unmeasured | cohort maturation (Wave 2's 24B), `GenerationalSuccessionEngine` chapters, memorial accumulation — none of it is gated by a clock the simulation obeys |

**Reading:** nothing here needs a new mechanic. It needs the day counter to stop being a number and
become a **context** that systems already built (thermal, greenhouse, migration, travel, tribute,
preservation) read from one place — and a deadline vocabulary general enough to reuse the warlord
pattern.

---

## Task 38A — One calendar authority: season, chapter, and the year the game is in

**Goal:** a single Core calendar read-model every system may consult, computed from the day, and
seasonal profile data large enough to matter.

**Files:** new `Assets/Ashfall.Core/Calendar/CampaignCalendar.cs` (+ `SeasonReadModel`),
`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs` (`ICampaignCalendar` already exists),
`World/WeatherSystem.cs`, `weather_seasons.json`, `YearOfAsh/YearOfAshTimelineSystem` (ambient
temperature), `src/Host/WorldHostSession.cs`, `src/Main.World.cs`, `SaveSectionRegistry`,
`Ashfall.Core.Tests/CampaignCalendarTests.cs`.

### Substeps

1. **Read the existing calendar interface first** — the coordinator already takes an
   `ICampaignCalendar` (`CampaignDayCoordinator.cs:46 Calendar`), so the seam is there; decide whether
   to extend it or wrap it, and prefer extending so there is still one calendar.
2. **Define the surface**: day, season id + display name, season progress (0..1), days-to-season-end,
   chapter, year-since-exchange, ambient baseline temperature, and a deterministic per-day
   severity roll — one place, pure function of day + seed.
3. **Expand the seasonal authority** from 3 windows to the number the game's length actually needs
   (a 200–400 day campaign with multi-year generational play wants 4 seasons + repeat-with-drift,
   authored; not 3 flat windows), each with weather weights **plus** the mechanical modifiers
   systems will read (day length, ambient temperature band, migration bias, preservation bias).
   Author in JSON — no invented ids outside the master list.
4. **Fuse with Wave 4's `WeatherEffects`** (20C step 2): season biases weather *selection*,
   `WeatherEffects` describes weather *consequences*; one table joins them so a season is never a
   secret global multiplier.
5. **Single source of ambient temperature**: `YearOfAshHostSession` already derives
   `_timeline.AmbientTemperatureCelsius` for deep freeze/radon; promote that to the calendar so
   thermal/power (23) and the base campaign use the same number instead of one expansion having a
   private winter.
6. **Expose it to the day loop** as a value in `DayAdvancedEventArgs` (or a resolved read-model the
   owners pull) so every owner sees the same calendar snapshot for the same day — critical for
   determinism and replay.
7. **Persist** the calendar state in the campaign section, and prove a mid-season save resumes
   correctly (the coordinator's `LastAdvancedDay` catch-up behaviour from Wave 4's 30A step 11).
8. **Display it everywhere it matters**: day counter becomes "Day 143 · Late Thaw · Year 5" in the
   HUD/dashboard/briefing (keyed text per Wave 3's 25A), and the forecast panel shows
   days-to-season-change — preparation is the player-facing payoff of a calendar.
9. **Delete the parallel notion**: reconcile `OnTickIceRoadClicked` (a name promising ice roads but
   doing none, row 7) — rename the handler or make ice roads a real seasonal travel state (38B).
10. **Tests**: pure-function per-day season, boundary days, drift-on-repeat, ambient continuity,
    save/load catch-up, determinism (paired seeds), and a test that no system computes a season from
    `_simDay` by hand (a source-scan for `day % 90`-style arithmetic is the regression lock).
11. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** one authority answers "what time of what year is it", and every system reads the same
answer.

---

## Task 38B — Seasonal consequences: the year actually changes the game

**Goal:** each season moves at least four already-built systems, with the briefing attributing it
(31), so the year turns whether or not the player notices.

**Files:** `GreenhouseSystem.cs` + `GreenhouseExpansionCatalog.cs`, `ShelterThermalSystem.cs`,
`PowerGridSystem.cs` (23), `WildlifeMigrationSystem.cs` + `WildlifeTrappingSystem.cs`,
`ExpeditionSystem` (travel, 32B), `KitchenNutritionSystem.cs` (preservation),
`DiseaseSystem.cs` (vectors), `TravelingCaravanSystem.cs`, calendar read-model, seasonal data.

### Substeps

1. **Write the season matrix first**: rows = season/season-phase, columns = the systems above, cells =
   the modifier and where it's authored. Empty columns get fixed in this task or are marked
   `DECORATIVE` (the Wave 2 rule: no fake dependency invented mid-plan).
2. **Light and growth**: greenhouse `LightHoursPerDay` and crop duration read season day-length;
   blight resistance shifts with season (catalog already authors `BlightResistance`), so winter
   harvests become a lighting/power problem rather than a flat rate.
3. **Heat and fuel**: ambient temperature from 38A feeds `ShelterThermalSystem`, which draws power
   (23A step 10) — winter should be the season the grid is remembered for, and the fuel chain
   (generator + heating + foundry + vehicles) becomes one competition on one screen.
4. **Cold preserves, warmth rots**: seasonal bias on `GetSpoilageDays` (35B) so a cellar in deep
   winter is genuinely better than a cellar in a thaw — provisioning becomes annual strategy.
5. **Migration and hunting**: `WildlifeMigrationSystem` state biases trap sites and quarry
   availability (and, post-35A, delivers real goods) so the larder has a seasonal rhythm.
6. **Mobility**: ice roads, thaw mud, storm frequency — travel modifiers on 32B's route edges.
   Make `ice_road` a real travel state rather than roster prose; a road that opens in winter and
   closes in thaw is the single most legible seasonal fact a player can learn.
7. **Disease vectors**: `DiseaseSystem` has four authored transmission vectors; season-bias two of
   them (water-borne in thaw, respiratory in cold) so illness has a calendar.
8. **Trade rhythm**: caravan arrivals and stock follow season (Wave 4's 30C autonomy + 32B edges), so
   a winter siege of scarcity is felt, not described.
9. **Every effect must be forecastable from the UI**: a season panel line for each modifier actually
   applied, sourced from the same Core function — no hidden multipliers (Wave 2's legibility rule).
10. **Grace and severity**: the harshest season must be survivable with preparation and brutal
    without; verify with `ashfall-balance-sim` sweeping season severity × fuel/food stock, and record
    the curves.
11. **Emit transitions** (`season_changed`, `first_frost`, `thaw_began`, `road_opened`,
    `road_closed`) into 31's vocabulary and the journal, so the year is memorable as events.
12. **Tests**: matrix-driven — one test per (season × system) non-identity cell, plus a four-season
    scripted campaign asserting the year's shape is measurable in food, fuel, dose, illness, and
    travel.
13. **Run the checklist** + `--expedition-selftest` + snapshots of the seasonal panels.

**DoD:** a winter and a thaw are different games, and both are survivable for different reasons.

---

## Task 38C — Deadlines: the warlord pattern generalised

**Goal:** a shared obligation/deadline mechanism (due day, penalty, escalation, remission) so
promises in the fiction become scheduled pressure instead of ambient text.

**Files:** new `Assets/Ashfall.Core/Commitments/CommitmentSystem.cs` (generalising
`WarlordDoctrineSystem`'s tribute cycle), `src/Main.YearOfAsh.cs:54–78` (existing pattern),
`LedgerDebtSystem.cs` (foreclosure), `RegionalTreatySystem.cs` (treaty terms),
`CensusClaimSystem.cs`/`VoluntaryRegisterSystem.cs`, `DutyRosterSystem.cs` (schedules),
`Crossing*/StandingRecord*` obligations, obligations data JSON, `SaveSectionRegistry`, briefing (31),
`EpilogueMatrixRuntime` (19A inputs).

### Substeps

1. **Extract, don't invent**: read `SettleWarlordTribute(amount, day, out next)` +
   `CollectorLine("paid"/"short"/"refused")` + `_warlord.TickDaily(day, rng, context)` and name the
   general shape — `due day, amount/item, settlement, shortfall consequence, escalation ladder,
   remission` — then implement it once.
2. **Author obligations in data** (`commitments.json`): id, type (tribute, debt instalment, treaty
   term, census filing, delivery contract, quota), counterparty faction, due window, amount, and
   per-stage consequence — snake_case ids, `schema_version`, gated by the integrity validator.
3. **One ledger, many creditors**: debts owed to factions, the ledger-debt foreclosure path, and
   treaty terms must all be rows in one commitments view; the player needs one screen that lists
   every promise and its due day.
4. **Warning ladder with real time**: T-minus days reported by the briefing and the guidance overlay
   (17B), with escalating consequences that are *already* modelled (raid assignment, foreclosure,
   stance drop, gate closure) — never a bespoke punishment per obligation type.
5. **Choices under deadline**: partial payment, renegotiation, refusal, delay — resolved through the
   existing choice/effect appliers (Wave 1's 15A/15B) so a deadline is a decision, and each choice
   writes guilt/stance/flag state the ending reads.
6. **Seasonal coupling** (38B): tribute and delivery windows fall in seasons that make them hard —
   the calendar and the ledger are the same design idea, and their intersection is the game's
   difficulty curve.
7. **Counterparty autonomy** (30A): a faction at war with someone else may relax or intensify
   demands; a caravan that never arrives because it was intercepted (32B step 8) makes a delivery
   obligation impossible — consequences the player can trace but not predict.
8. **Persist and migrate**: a `commitments` section through `SaveStoreHub`, checksummed from birth;
   keep the warlord's own state as-is and have the general system read/compose it rather than fork
   it.
9. **Achievable-by-design test**: an automated check that every authored obligation is satisfiable
   given starting stock + production rates for that difficulty preset (Wave 4's 34B) — an
   impossible deadline is a bug, not difficulty.
10. **Emit `obligation_due`, `obligation_met`, `obligation_missed`, `deadline_escalated`** (31) and
    memorialise refusals so the ledger is part of the story the ending tells (19A).
11. **UI**: a "Promises & Debts" surface assembled from the ledger — prefer binding an existing
    console (e.g. `waystation_network` or the shelved `subterranean_debt_ledger`, per Wave 1's 16A)
    to the real authority rather than adding a 31st fake panel.
12. **Tests**: settlement maths per type, escalation ladder, renegotiation effects, seasonal
    interaction, save round-trip mid-window, determinism, and the satisfiability check as a data test.
13. **Run the checklist** + `--expansions-selftest` + `--data-integrity-selftest`.

**DoD:** the game schedules pressure the player can see coming, plan for, and fail on purpose.

---

## Cross-Task Dependencies

```
38A (one calendar) ──► 38B (seasonal consequences) ──► 38C (deadlines land in seasons)
   ▲                          │                             ▲
   │                          ├── 20C WeatherEffects (shared table, no rival)
   └── existing               ├── 23A/23B (watts for heat/light)
     ICampaignCalendar        ├── 35A/35B (goods arrive; perish by season)
                              └── 30A/32B (autonomous world + routes)
                     31A (event kinds) ◄── every transition and deadline reports
```

**Execution order:** 38A → 38B → 38C, and inside Wave 5: **36A (port gate) → 35A → 38A → 37A →
38B → 39A → 38C** — 38B's new wiring is exactly the class of seam Wave 5's 36 exists to prove is
bound.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. ashfall-seed-replay: identical 400-day multi-year curve before/after
7. ashfall-balance-sim: season severity × stock sweep; deadline satisfiability
8. bash scripts/ci/triad-drift-gate.sh                           # new sections registered
9. godot --headless --path . -- --runtime-scale-selftest         # calendar is O(1) per day
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 38A | 1 new + 2 | 3 | 1 expanded | 2 | 8–11 | Medium | MEDIUM (everything reading temperature shifts) |
| 38B | 4–6 reads | 2 | 1 | 2 | 12–16 | Medium–High | **HIGH — balance-wide; sweep before merge** |
| 38C | 1 new + reuse | 3 | 1 new | 1–2 | 10–14 | Medium | MEDIUM |

**Guardrails:** no new calendar system beside `ICampaignCalendar`, no per-system day arithmetic, no
season mechanic that exists only in a panel, no punishment without a warning window, no obligation
that is unsatisfiable by construction, and no invented dependency where a system genuinely doesn't
care about the season (`DECORATIVE` rows stay honest).
