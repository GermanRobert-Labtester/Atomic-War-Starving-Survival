# C2[4] / Plan 20 — Baseline Exposure Audit (Premise Corrections)

> Date: 2026-09-15. Evidence-first read-only sweep completed before any 20A edit,
> per `AGENTS.md` rule 7 (*use current evidence*) and `C2_planintegration[4].md` §5.
> Focused verification at time of audit: Radiation 43/43, EnvironmentalExposureJourneyTests 1/1,
> host build 0 errors, data-integrity 327/327 PASS, bridge PASS, triad drift PASS.

## 1. Plan premises that are STALE (already built in current source)

The C2[4] plan's §1 source-truth defect list was written against an older
repository state. Current evidence:

| Plan §1 premise | Current source reality | Evidence |
|---|---|---|
| Hardcoded survivor-ID zone branch (`2f / 40f`) | **Already removed.** A source-level regression gate exists. | `Ashfall.Core.Tests/Radiation/EnvironmentalExposureMatrixTests.cs::NoSurvivorIdLiteral_ControlsRadiationDose` (passing); no literal branch found in `src/` |
| No explicit survivor position state | **Exists:** `SurvivorExposureLocation` (ShelterInterior / ShelterPerimeter / WastelandOutdoors / Expedition) + `ExposureEnvironmentResolver.SetSurvivorLocation` | `Assets/Ashfall.Core/Radiation/ExposureEnvironment.cs` |
| Position not persisted | **Persisted** in survivor save slices (`locationKind`, `locationId`) and restored | `src/Host/SurvivorsHostSession.cs:497,562-565` |
| Location contamination not consumed | **Data-authored + consumed:** `baseRadsPerHour` (fallback `dangerLevel × 5`) loaded from `locations*.json` via `LoadLocationRadRates` | `ExposureEnvironment.cs` (loader), `crossing_locations.json` rows |
| Weather has no consumed radiation multiplier | **Consumed:** `WeatherRadModifierProvider → WeatherSystem.OutdoorRadModifier` | `src/Main.Survivors.cs:59`, `Assets/Ashfall.Core/World/WeatherSystem.cs:239` |
| Expedition members stay on shelter ambient | **Resolved:** host `SurvivorLocationQuery` maps active expeditions to `Expedition + exp.locationId` | `src/Main.Survivors.cs:82-88` |
| Fallout/anomaly overlays missing | **Wired:** `FalloutContaminationProvider` (fallout plumes) and Plan 176 `AnomalyRadRateProvider` | `src/Main.Survivors.cs:62-76` |
| Exposure-source resolver missing | **Exists:** `ExposureEnvironmentResolver` — pure, identity-free, providers for location/weather/fallout/anomaly | `Assets/Ashfall.Core/Radiation/ExposureEnvironment.cs` |

Existing test coverage already proves: shelter vs outdoors dose divergence,
identity-independence of base rate, catalog-backed expedition rads, seeded
weather scaling without breaching intact shelter (matrix tests, 43/43 green).

## 2. Plan premises that are REAL gaps (revised 20A/20C scope)

### Gap G1 — Weather balance values are hardcoded in Core (20A §11, 20C §35)

`WeatherSystem.OutdoorRadModifier` is a `switch (WeatherKind)` returning Core
constants:

- `FalloutStormOutdoorRadModifier = 150f`
- `BlackRainOutdoorRadModifier = 250f`

This is exactly the shape plan §11.2 forbids (balance values embedded in a
`switch (WeatherKind)`). No weather-effects data file exists
(`weather_hardening_upgrades.json`, `weather_route_gates.json`,
`weather_seasons.json` exist; no effects/multiplier catalog does).
**Work:** data-authored `WeatherEffects` record; every `WeatherKind` gets an
explicit entry (20C §35.1 no-silent-defaults gate).

### Gap G2 — `ShelterRadQuery` never populated (20A §12)

`RadiationSystem` supports `context.ShelterRadQuery(zone)`, but the host
exposure builder (`SurvivorsHostSession.BuildExposure`) constructs contexts via
`ExposureEnvironment.ToExposureContext(...)`, which sets only
`ShelterShielding` subtraction — never the interior-rad query. Shelter
interior base rate is also a hardcoded Core const
(`DefaultShelterInteriorRadRate = 2.0f`), not data-authored.
**Work:** populate the query seam from the (20B) shielding model; data-author
the shelter interior baseline.

### Gap G3 — Weather-gate forced-entry rad dose is silently dropped (§49/§57 class)

`ExpeditionHostSession` raises `OnWeatherGateForced(survivorId, locationId,
WeatherGateBlock)` at two sites, with a documented contract: *"The radiation
owner applies `block.ForceRadDose` to the survivor."* **The event has zero
subscribers** — `force_rad_dose` authored in `weather_route_gates.json` is
never applied. This is a silent-failure defect and a plan-20 acute-dose
producer with no owner.
**Work:** subscribe through the radiation ownership seam, apply as a documented
acute (distinct-from-ambient) dose, with exactly-once semantics and tests.

### Gap G4 — Vestigial expedition camp radiation field (§13.2 audit row)

`ExpeditionSystem` camp state carries `radiationExposure` which is only ever
zeroed (`:711`) and copied on save-transfer (`:1336`) — never incremented.
No double-count risk (confirms the continuous-ambient exactly-once invariant is
already held by `RadiationSystem` alone), but it is dead data that §13.2's
audit table must classify (keep-as-inert or retire).

## 3. Revised execution order (20A remainder)

1. **G3 first** — smallest coherent repair: give `OnWeatherGateForced` a
   consumer honoring its own documented contract (acute dose, not ambient).
2. **G1** — `weather_effects` data authority; `WeatherSystem.OutdoorRadModifier`
   reads from catalog (20A §11 + 20C §35 share one schema shape, per §11.3).
3. **G2** — populate `ShelterRadQuery` (interim interior query OK; 20B replaces
   internals behind the same seam); data-author shelter interior baseline.
4. **G4** — record decision on the dead `radiationExposure` field (no behavior
   change required to close §13.2's exactly-once audit).

Baseline metrics for §5.3 (dose/day curves) will be captured as part of G1/G2
verification; the position-divergence invariants the plan asks to "establish
as broken" are demonstrably already fixed and gated by tests.

## 5. 20A execution outcome (2026-09-15, same session)

- **G3 closed:** `OnWeatherGateForced` now has a consumer in `src/Main.Expeditions.cs` honoring
  its documented contract — `ForceRadDose` from `weather_route_gates.json` is applied as an
  acute discrete dose through the radiation owner (`SurvivorsHostSession.ApplyAcuteRadDose`,
  clamped 0–100 acute scale, never touching the ambient zone-rate providers — no double-count).
  Journaled with a dedupe key per survivor/location/gate. Source-gate + data tests: 4/4.
- **G1 closed:** `weather_effects.json` data authority (all 22 WeatherKind rows, explicit —
  no silent defaults). `WeatherEffectsCatalog` strict loader; `WeatherSystem.OutdoorRadModifier`
  and a new `ForecastRadModifier(kind)` both read the bound table (legacy constants preserved
  byte-identically when unbound). **Balance alignment recorded:** Ashfall outdoor dose is now
  45 rads/h — matching what the legacy forecast projection already promised players (previously
  the forecast said +45 while actual dose added 0 — a §57.3 drift, resolved in favor of the
  player-visible forecast). Integrity validator enforces full coverage + range/dup/unknown rules.
  Catalog tests: 10/10. Data-integrity selftest PASS (327 catalogs).
- **G2 closed (seam level, per plan §12):** `ShelterRadQuery` semantics proven and pinned by
  `ShelterRadQuerySeamTests` (5/5): query takes precedence over the shielding-subtraction
  fallback, exactly-once accumulation, gear protection still subtracts, no negative interiors.
  The host shelter path keeps the byte-identical fallback math until 20B's ShelterShieldingModel
  supplies the interior query — the seam requires no caller changes at that handoff.
- **G4 decision:** `ExpeditionSystem.camp.radiationExposure` is inert (never incremented);
  continuous-ambient exactly-once ownership by RadiationSystem confirmed. Field retirement is a
  20B-era cleanup, not a behavior change.
- **Deferred to next 20A package step:** §16 radiation UI breakdown extension (panel shows
  env fields already; input-breakdown display parity), §17 seeded 60-day balance sweep
  (ashfall dose change should be re-validated by `ashfall-balance-sim`).

### 20A closure (same day, second pass)

- **§16 closed:** canonical `RadiationSystem.ComputeEffectiveRate` extracted (tick + UI share
  one resolver); Core `ExposureBreakdown` read model (position label, zone/base/weather/fallout/
  anomaly/shielding/gear/effective rate/dose/lifetime, compact display line);
  `SurvivorsHostSession.GetExposureBreakdown`; `RadiationDetailPanel` renders the canonical
  line per survivor. Source gates forbid panel-side dose math. 15/15 tests.
- **§17 closed:** `Plan20ARadiationBalanceSweepTests` (9/9) — 60-day deterministic curves for
  intact/degraded shelter, clear/storm surface, low/hot expedition sectors, multi-zone phase
  curve, ordering + paired-run fingerprint. Report: `docs/balance/BALANCE_SIM_radiation_exposure_20A.md`.
  Findings (proposal only, no data edited): F1 surface/expedition rates saturate acute scale in
  hours (pre-existing, upstream of 20A); F2 Ashfall alignment recorded; F3 ceiling attenuation is
  the dominant shelter lever (size 20B contributions against it); F4 intact shelter is exactly 0 —
  20B internal sources should give shelter maintenance a numeric floor.
- **Full verification:** Radiation 72/72; host build 0 errors; data-integrity PASS; bridge PASS;
  panel-bind-lifecycle PASS; ui-a11y PASS; triad drift PASS.

## 4. Pre-existing conditions observed (not owned by this package)

- Working tree carries extensive uncommitted user changes; the
  `verify-fast.sh` whitespace-hygiene gate fails on those pre-existing files
  (e.g. `Assets/Ashfall.Core/DecontaminationSystem.cs:206`) — outside this claim.
- `INTEGRATION_PLANS.md` current batch is the DISTRESS-SIGNALS wave (presented
  for acceptance); C2[4] 20A is claimed here as a new package.
