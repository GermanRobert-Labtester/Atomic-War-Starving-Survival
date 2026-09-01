# Plan 19 — Dynamic World Systems: Weather Forecasting, Orbital Harrow & Seasons

> **Theme:** The *systemic* world that changes whether or not the player acts. Weather is
> passive input, orbital telemetry is unexploited, and seasons don't drive content cadence.
> This plan makes the world's *dynamics* a source of anticipatory gameplay.
>
> **Key evidence (verified):** `WeatherSystem.cs` (22 states) + `WeatherStationSystem.cs` +
> `OrbitalHarrowTelemetrySystem.cs` + `WeatherAtmosphereMap.cs` all live; registry §20 flags
> "orbital kinetic telemetry" and "weather forecasting payoff" as underused; sky-armor (10/#10)
> awaits hazard events.

---

## Task 19A — Weather forecasting & storm-cadence payoff

**Goal:** Make the `WeatherStationSystem` actually *predict* — give the player a readable
forecast so weather becomes plannable, and storms become anticipated events (feeds 13C).

**Files:** forecast data / weather tables, read-only `WeatherStationSystem.cs`,
`WeatherSystem.cs`, `WeatherAtmosphereMap.cs`; UI forecast readout in `src/UI/`.

**Substeps:**
1. Read `WeatherStationSystem` to see what it computes today (does it forecast, or only report current?).
2. If forecast exists but isn't surfaced: wire a forecast readout to a HUD/panel. If it doesn't: file a small Core extension (forecast = seeded N-day lookahead on the deterministic weather stream) — do NOT add RNG.
3. Define forecast accuracy tiers (a damaged station gives vague, a repaired one precise) — upgrade path via existing repair mechanics.
4. Surface a 3-day forecast (icon + confidence) on the HUD and a detailed panel.
5. Tie forecast to the 13C crisis events: "fallout storm in 2 days" lets the player prepare (filter stock, recall expeditions).
6. Author 6 forecast-flavor lines per season in the house voice.
7. Ensure forecast is derived from the deterministic weather stream (same seed → same forecast) — no `System.Random`.
8. Data-integrity selftest; weather selftest.
9. xUnit: forecast matches realized weather N days out; accuracy tier affects precision; determinism.
10. Snapshot-diff the forecast HUD; approve golden image.

**Next steps:** cloud-seeding (white space #17) consumes the forecast ("storm in 2 days — fire
the mortar now"); false-forecast events (a broken station misleads).

---

## Task 19B — Orbital Harrow kinetic-strike events

**Goal:** Activate `OrbitalHarrowTelemetrySystem` (decaying-orbit kinetic debris) as a real
hazard: forecastable strikes that test sky armor (white space/registry §20).

**Files:** orbital/strike event data, read-only `OrbitalHarrowTelemetrySystem.cs`,
`SkyLayerArmorSystem.cs`, `PowerGridSystem` (impact damage), `WeatherSystem` (debris vs weather).

**Substeps:**
1. Read `OrbitalHarrowTelemetrySystem` to learn what telemetry it tracks (decay rate, predicted impact window, footprint).
2. Read `SkyLayerArmorSystem` to learn cell-grid penetration/damage so strikes map to roof cells.
3. Author 5 kinetic-strike event templates (small debris shower, single heavy keg, clustered impact, near-miss with shockwave, telemetry-predicted vs. surprise).
4. Wire a predicted-strike warning through the 19A forecast/radio so the player can shore ceiling cells or evacuate a room.
5. Map strike damage to specific roof cells + cascading power/room damage (existing systems).
6. Author the telemetry readout surface (a decaying-orbit plot) in the UI — tense, technical, restrained.
7. Add a post-strike salvage opportunity (debris field on the map = rare alloys; ties to 11A/04 relics).
8. Ensure strike timing is deterministic (seeded) and saved/restored.
9. Data-integrity + sky-armor + orbital selftests.
10. xUnit: strike prediction window, roof-cell damage mapping, salvage spawn, save round-trip; cross-tool QA (couples orbital + armor + power).

**Next steps:** a planetary-defense radar upgrade that narrows the impact window; a strike that
*reveals* a buried site (cracks it open for 11A).

---

## Task 19C — Seasonal & nuclear-winter content cadence

**Goal:** Give the seasonal cycle real content rhythm: each season brings distinct hazards,
resource swings, and events so the calendar is a strategic axis, not wallpaper.

**Files:** seasonal event/weather weighting data, `events.json` family, read-only
`WeatherSystem` (seasonal cycles), `GreenhouseSystem` (growing season), ice-road (thaw).

**Substeps:**
1. Read how `WeatherSystem` models seasons + nuclear winter; list the distinct seasons/phases.
2. Map which existing systems are season-sensitive (greenhouse growing, ice road thaw, deep freeze, radon).
3. Define 4–6 named seasons/phases (Ash Fall, Deep Freeze, Thaw, Black Bloom, High Cold, The Turning) with distinct hazard profiles.
4. Author 3 signature events per season (a thaw flood, a deep-freeze pipe burst, a bloom spore surge) keyed to the seasonal state.
5. Author seasonal resource swings (thaw = water abundant/food scarce; deep freeze = reverse) via existing scarcity tiers.
6. Author 6 seasonal flavor texts + 4 seasonal radio broadcasts (07B VO candidates).
7. Wire the ice-road open/closed state to the thaw season (18A quests respect it).
8. Validate ids; data-integrity selftest; weather selftest.
9. xUnit: seasonal transitions fire events; resource swing applies; ice-road state matches season; determinism.
10. Balance sim: seasonal swings create tension without an unwinnable season; cross-tool QA.

**Next steps:** a "survived a full year" milestone; seasonal epilogue coloring (15A); seasonal
visual filters on the map/shelter (08 art).
