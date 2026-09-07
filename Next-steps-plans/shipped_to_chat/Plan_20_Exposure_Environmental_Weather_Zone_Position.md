# Plan 20 — Exposure Is Environmental: Weather, Zone, and Position Drive Dose

> **Wave:** Continuity Wave 2 — *The Bunker Machine* (Plans 20–24)
> **Predecessor:** Wave 1 (Plans 15–19) — see `Wave1_Continuity_Audit_INDEX.md`.
> **Depends on:** nothing to start; 17A's `DayEventKinds` vocabulary is consumed by 20C.
>
> **Theme:** in a game about surviving a nuclear exchange, the radiation dose a survivor
> accumulates is decided by **a hardcoded ternary on one survivor's id**. Weather states,
> sector contamination, fallout storms, indoor vs outdoor position, decontamination, and
> ventilation do not change the dose rate. Everything needed to fix it already exists in Core;
> the host simply never feeds it in.

---

## Evidence Inventory (re-verified @ `ccac926e`)

### The single set site for zone radiation in the entire repository

| Fact | Evidence |
|---|---|
| `ZoneRadLevel` is assigned in exactly one place, from a literal | `grep -rn "\.ZoneRadLevel" src/ Assets/Ashfall.Core` → one writer: `src/Host/SurvivorsHostSession.cs:246` |
| The value is a survivor-id ternary | `src/Host/SurvivorsHostSession.cs:240` — `float zone = state.Id == "survivor_gunner_mikhail" ? 40f : 2f;` with the comment `// Mikhail is outside in the zone; others are in the shelter` |
| Shelter shielding *is* derived — but only from one input | `:241–244` — `2f * Shelter.GetWeakestCeilingAttenuation()`; air filtration, ventilation, decontamination, and airlock state never enter it |
| The Core math is already general and testable | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:209 ComputeEffectiveAmbient(zone, shielding)`, `:214 ComputeExposurePerHour(zone, gear, shielding)`, and a **second, better** path at `:175–180`: `context.ShelterRadQuery(zone)` → interior rad query — which the host never populates |
| Dose feeds a lot, so the input is high-leverage | `Expose(...)` → `OnDoseChanged`, `Dosimeter.Record`, ARS phase progression, sick list, triage, `DoseLedgerSystem`, and the epilogue-facing survivor fate chain |
| Weather has 22 states and no radiation coupling | `WeatherKind` enum (22 values, `docs/audio/SILENCE_AUDIT.md` §4.1) drives audio and forecast text; nothing in `WeatherSystem` exposes an outdoor-rad multiplier and no C# reads one (`grep -niE "OutdoorRad|radiationLevel" Assets/Ashfall.Core/Weather*` → 0) |
| Locations carry no consumed contamination field | `grep -rn "RadLevel\|rad_level" --include=*.cs` → only `RadiationSystem`'s own DTO property; `locations.json` contamination is not read into exposure |
| Expeditions deploy people into those locations | `ExpeditionSystem` travels `locationId`s and reports risk (`src/Main.Expeditions.cs:177–180` starts combat at `state.locationId`), but the deployed survivor's *ambient dose* is still the `2f` shelter literal while they are 40 km away in a hot sector |
| Indoor/outdoor is not modelled at all | `SurvivorNeedsState` / `RadSurvivorWrapper` carry needs and dose, no position field; "where is this survivor right now" exists only as roster-panel text |

**Interpretation:** this is not a missing system. `ExposureContext` already has three inputs
(zone, shielding, worn gear) plus an injectable interior-rad query. Two of the three are fed
constants. Wave 1 made the *story* consequences of actions real; Wave 2 makes the *physical*
consequences real, starting with the one the title is about.

---

## Task 20A — Position: every survivor is somewhere, and somewhere has a dose rate

**Goal:** replace the `2f / 40f` literal with a derived, per-survivor exposure source: where the
survivor is, what that place's contamination is, and whether they are indoors or outdoors.

**Files:** `src/Host/SurvivorsHostSession.cs:235–249`, new
`Assets/Ashfall.Core/Radiation/ExposureSourceResolver.cs`, `Assets/Ashfall.Core/WeatherSystem.cs`
(read), `Assets/Ashfall.Core/World/*` (zone/sector read models), `ExpeditionSystem`,
`locations.json` / `world_history.json` / sector catalogs, `SaveSectionRegistry`.

### Substeps

1. **Write the failing test first**: assert that two campaigns differing only in a survivor's
   position (shelter vs a hot sector) produce different lifetime doses after the same number of
   hours. It must fail today with the identical value for both.
2. **Add an explicit position fact to Core** — `SurvivorLocation { kind: Shelter | Surface |
   Expedition | Node, locationId, sectorId }` — set by the systems that already know it:
   expedition dispatch/return (`ExpeditionSystem` phases), and a default `Shelter` for everyone
   else. Do not infer position from roster strings.
3. **Author `ExposureSourceResolver` in Core** (engine-agnostic): given position + world state,
   return the ambient zone rate. This is the single place the "why is my dose this number"
   question gets answered, so it must be a pure function of inputs.
4. **Read contamination from the data authority**, not from code: pick the existing
   sector/location contamination values in `locations.json` / sector catalogs and add the field to
   the catalog DTO + loader. If the field does not exist yet, add it to the JSON first (snake_case,
   `schema_version` respected) — Invariant 6 says JSON is the authority.
5. **Wire the weather modifier** as a multiplier supplied by the weather read model (storm =
   resuspended fallout raises outdoor rate; calm/clear lowers it). Keep the multiplier table in
   data, not in a switch.
6. **Populate `ShelterRadQuery`** (`RadiationSystem.cs:175–180`) for shelter-indoor survivors from
   the shelter's own interior conditions, so the better existing code path finally executes.
7. **Expedition members** must resolve to their current node's rate while deployed. Cross-check
   against whatever dose expeditions already add per travel tick so exposure is not counted twice
   (this is the #1 regression risk of the task — see step 12).
8. **Delete the literal branch** at `src/Host/SurvivorsHostSession.cs:240–244` and replace with
   `_exposureResolver.Resolve(state.Id, day, weather, position)`. If `survivor_gunner_mikhail`
   must start outside for scripted-opening reasons, express that as authored starting state, not
   as an id comparison.
9. **Determinism**: resolver is a pure function plus `ISeededRng` for any stochastic component
   forked through `CampaignStreamIds` (as `Main.ShelterBatch3.cs:106` already does).
10. **Persist position** in the survivor section so a reload reproduces yesterday's dose curve.
11. **Surface the inputs** (this is the continuity part): the radiation detail panel and dosimeter
    pen read-out must show the *breakdown* — ambient zone, weather modifier, shielding, gear — not
    just the resulting number. Reuse `radiation_detail` and `radiation_history` routes; no new
    console.
12. **Balance gate**: run `ashfall-balance-sim` / a seeded 60-day sweep before and after. A shelter
    with intact shielding must stay survivable; a storm outdoors must be dangerous but not
    instant-death. Record dose-per-day curves in the task log.
13. **Tests**: resolver unit tests per position kind, weather multiplier table test, no-double-count
    integration test, save round-trip, paired-seed replay.
14. **Run the five-step verification checklist** + `bash scripts/ci/triad-drift-gate.sh`.

**DoD:** dose rate is a function of where you are and what the sky is doing; the breakdown is
legible in the UI.

---

## Task 20B — Shielding: the shelter's condition decides your dose

**Goal:** make every shielding input real — air filtration, ventilation, decontamination,
radon migration, hatch/airlock state, wall/ceiling integrity — so "maintain the bunker" and
"don't die of radiation" stop being two unrelated games.

**Files:** `src/Host/StartingLevelHostSession.cs`, `Assets/Ashfall.Core/StartingLevel/*`,
`DecontaminationSystem.cs`, `AirlockSecuritySystem.cs`, `AirlockSecurityHostSession.cs`,
`BasalRadonMigration` / `SumpFloodingSystem` / `ShelterThermalSystem` (read), new
`Assets/Ashfall.Core/Radiation/ShelterShieldingModel.cs`, `assets/.../shelter` data JSON.

### Substeps

1. **Inventory every shielding-capable input already simulated**: ceiling/wall attenuation
   (`GetWeakestCeilingAttenuation`), HEPA/air-filter state (`item_air_filter_hepa` exists in
   starting supplies), ventilation (`StartingLevelSystem` atmosphere session already drives the
   ventilation audio loop), decontamination cycles, airlock seal state, radon migration.
2. **Author `ShelterShieldingModel` in Core**: an additive/multiplicative composition with
   documented precedence, returning (a) a `ShelterRadQuery` delegate for indoor survivors and
   (b) an `ShelterShielding` fallback. One model, both consumers — no second arithmetic path.
3. **Make filter consumption real**: filters clog on a deterministic curve from dust/dose
   exposure; a clogged filter measurably raises indoor dose; replacing one is felt the same day.
   The `shelter_air_filter` hazard alert already exists (`AutopsyHostSession`,
   `ShelterAudioController`) — tie the alert to the *numeric* threshold that triggers it, not to a
   separate flag.
4. **Decontamination** must temporarily reduce interior contamination for a data-defined window,
   with diminishing returns on repeat cycles (consumes water + consumables already in the catalog).
5. **Airlock/vent coupling**: an unsealed hatch or stalled ventilation raises indoor dose during
   bad weather specifically — this is the moment where two systems become one situation.
6. **Radon migration** and **sump flooding** must feed the same model (both are already
   simulated; both currently end their influence inside their own panel).
7. **Degradation must be visible where it is acted on**: the shelter panel shows the composite
   shielding value plus its weakest contributor, reusing the existing
   "weakest ceiling" phrasing so the language stays consistent.
8. **Emit day events** (`shielding_degraded`, `filter_replaced`, `decon_cycle`, `hatch_unsealed`)
   into 17A's feed so the briefing can say *why* last night's dose climbed.
9. **Data-author the curves** in a new `shelter_shielding.json` (snake_case, `schema_version`,
   referenced ids gated by `CatalogIntegrityValidator`) rather than embedding constants.
10. **Tests**: composition precedence, clog curve, decon window, unsealed-hatch-during-storm case,
    save round-trip for filter/decon state, and one end-to-end test that replacing a filter lowers
    next day's dose.
11. **Balance**: sweep storm frequency × filter supply so scarcity is real but never a death
    spiral the player cannot recover from; record the curves.
12. **Snapshot**: cover the updated shelter/radiation panels per
    `docs/ui/SNAPSHOT_FIXTURE_POLICY.md`.
13. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** the bunker's physical maintenance is the single biggest lever on survivor dose.

---

## Task 20C — Storms you can act on: weather consequences reach decisions

**Goal:** the 22 authored weather states stop being decoration and audio. Each significant state
must change at least one decision the player can make that day, and the forecast must be the tool
that lets them prepare.

**Files:** `Assets/Ashfall.Core/WeatherSystem.cs`, `World/WeatherIntelligenceCoordinator.cs`,
`ExpeditionSystem` (risk/estimates), `StartingLevelSystem`, `ShelterThermalSystem`,
`WildlifeTrappingSystem`, `TravelingCaravanSystem`, `src/UI/WeatherForecastPanel.cs`,
`src/UI/MapPanel.cs`, weather data JSON, `src/Audio/AudioEventBridge.cs`.

### Substeps

1. **Enumerate the 22 `WeatherKind` values** and, in a table, record today's *actual* consumers of
   each (mostly: text + 14 audio cues). Rows with zero mechanical effect are the work list.
2. **Define a `WeatherEffects` data-authored record per state**: outdoor dose multiplier, travel
   speed/risk modifier, trap yield modifier, caravan availability, thermal load, visibility,
   forecast reliability. Put it in the weather catalog, not in a `switch`.
3. **Expedition dispatch reads it**: `ExpeditionSystem.Estimate` already returns ticks/fuel/capacity
   and risk (`#101`); add the weather term so the dispatch screen shows the estimate *for tomorrow*
   and the player can choose to wait. This is the cheapest real decision in the game.
4. **Forecast reliability** (`WeatherStationSystem` accuracy 0.7–0.9, calibration, 3-day horizon —
   all implemented, and `weather_forecast` + `weather_sonde` + `geiger_calibration` routes exist)
   must be *shown* next to the forecast, so a poorly calibrated station visibly misleads.
5. **Make being wrong costly but recoverable**: a storm missed raises dose/risk; it must never be a
   pure loss that removes a survivor outright on day 12.
6. **Surface warnings** through the radio and the briefing (17A): a storm the radio predicted and
   the station missed is a different player experience from a random hit — reuse
   `weather_alert` and the existing broadcast machinery rather than a new notification system.
7. **Trapping and foraging yields** read the same table (wildlife system already exists; the trap
   bait/quarry catalogs exist).
8. **Caravan availability** reads the same table — do not re-implement embargo logic here; if
   Plan 14A's `TradeEmbargoSystem` lands later it subsumes this row, so keep the data shape
   compatible with it.
9. **Thermal coupling**: cold states increase heating/fuel draw already simulated by
   `ShelterThermalSystem`; make the fuel pressure visible in the power/warmth surfaces.
10. **Audio parity**: the 8 states still without a transition cue
    (`SILENCE_AUDIT.md` §4.1: Ashfall, AcidSnow, BioFog, BlackSnow, BloodRain, EMPStorm,
    GlassStorm, RadHail, …) get mapped to existing beds by family — no new production batch here.
11. **Tests**: one test per state asserting at least one mechanical effect is non-identity, plus a
    table-driven test that every `WeatherKind` appears in the effects catalog (no silent default).
12. **Docs**: regenerate `docs/cli/HOST_CLI_COMMAND_CATALOG.md` if a forecast probe verb is added,
    and update the atlas so weather is no longer listed as text-only.
13. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** no weather state is purely cosmetic; the forecast is a decision tool the player can
trust, mistrust, or repair.

---

## Cross-Task Dependencies

```
20A (position + zone) ──► 20B (shielding composes on top of A's ambient)
        │                        │
        └────────┬───────────────┴──► 20C (weather multiplies both)
                 │
   feeds ◄───────┴── 17A event vocabulary (attribution) and 17C geiger intensity
   constrains ────── Plan 21 (gear protection only matters once dose is environmental)
```

**Execution order:** 20A → 20B → 20C. 20A must land first: composing shielding and weather onto a
constant zone rate would just produce a different constant. Wave-1 dependency: do **17A before or
alongside 20B/20C**, otherwise the new causality exists in the simulation and still cannot be
explained to the player.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/triad-drift-gate.sh
7. ashfall-balance-sim: 60-day seeded dose curves, before/after
8. ashfall-seed-replay: paired same-seed exposure curves identical
9. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 20A | 1 new + 1 | 1 | 1 (read) | 2 | 8–11 | Medium | **HIGH — double-counted dose; gate on step 12** |
| 20B | 1 new + 3 read | 2 | 1 new | 1 | 9–12 | Medium | MEDIUM |
| 20C | 1 modified | 1 | 1 modified | 2 | 10–14 (table-driven) | Medium | MEDIUM (balance-wide) |

**Guardrails:** no new radiation system, no new exposure formula (`ComputeExposurePerHour` is
already the formula), no new panel, no new audio family, no new weather state. This plan removes
literals and connects producers that already exist.
