# PLAN B67 CLOSEOUT — Radio Signal Cryptanalysis & Triangulation Intercept Grid

**Date:** 2026-09-06 · **Branch:** `feat/asset-pipeline-flagship`
**Scope:** audit-then-extend, per the Wave 0 reconnaissance. The intercept
grid already ships as **Plans 46–49**; B67's audit reconciled it against the
flagship plan and closed the single true gap.

## Audit results — what already exists (no duplicate was created)

| Plan requirement | Status | Evidence |
|---|---|---|
| 16 authored intercepts | ✅ | `radio_intercepts.json` — 16 defs with `frequency_khz`, `band`, `base_signal_strength`, `encryption.scheme/difficulty/required_skill_ids`, `triangulation.required_bearings/revealed_location_id`, `expiry_days` |
| Signal quality model | ✅ | `ShelterRadioStationSystem.Scan` — `effectiveStrength = base × tuningMatch × (1 − weatherNoise)` |
| Cipher progression (67.7) | ✅ | `ProgressDecryption` — difficulty-based rate (250/difficulty), operator-skill-multiplied, permille progress, headless-resolvable (no UI dependency) |
| Bearing accumulation (67.5) | ✅ | `RecordBearing` — distinct-azimuth rule (≥ 20°), required-bearing threshold |
| Triangulation (67.4/67.6) | ✅ | `RecordBearing` → authored `revealed_location_id` at `required_bearings`; deeper continuous-coordinate layer in `SignalTriangulationSystem` (ray intersection, confidence, uncertainty) — **discrete reveal through the authored graph, matching the map topology** |
| Map reveal exactly-once (67.12) | ✅ | `discoveredLocationIds` guard + `Main.Plans46_49.cs` → `_world.WastelandMap.Discover(locationId)` |
| Decoy system (67.10) | ✅ by design | `radio_intercept_spoofed_distress_trap_08` resolves through the normal path to `loc_motel_verity` — a dangerous authored location (`encounterChancePerTick` 0.2, Warlord-enforced). **The destination authority owns ambush risk; radio never resolves combat** — exactly the prescribed delegation |
| Skill integration (67.11) | ✅ | `required_skill_ids` (`skill_signal_ear`, `skill_cold_analysis`) multiply decryption gain — no auto-solve of top-tier content |
| Save (67.16) | ✅ | `RadioSave`/`RadioStationSaveStore`, full capture/restore |
| Audio/UI separation (67.13/67.14) | ✅ | Core emits state/events only; panel + audio are presentation |

## The one true gap — fixed

**`BindWeatherNoiseProvider` was never wired in the host.** Detection ran on
a hardcoded 0.15 default regardless of weather; plan 67.15 requires
canonical weather/solar interference.

**Fix (host wiring only):** `Main.EnsureRadioStation()` now binds a noise
provider over `_world.Weather.Current` (`WeatherNoiseForKind`):
- Clear → 0.05
- Rain/Overcast/Ashfall/BioFog/AlgaeBloom → 0.15
- FalloutStorm/Blizzard/BlackRain/AcidSnow/RadHail/GlassStorm/BloodRain/BlackSnow → 0.35 (the pair the triangulation engine already penalizes hardest)
- EMPStorm/AshLightning → 0.45 (electrical interference)

No radio-only weather state was created; the mapping is host presentation
over the canonical `WeatherSystem`.

## Files changed

| File | Change |
|---|---|
| `src/Main.Plans46_49.cs` | bind weather-noise provider + `WeatherNoiseForKind` mapping (~35 lines) |

## Verification (2026-09-06)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `dotnet test` (full suite) | PASS — 8823/8823 (radio suites included) |
| `--data-integrity-selftest` | PASS — 283 catalogs (16 intercepts validate) |

## Known follow-ups

1. **Cipher-wheel minigame (67.8)** — optional visual layer; Core
   `ProgressDecryption` is already the headless authority. Defer until a
   panel design exists (Google Stitch per project policy).
2. **Solar-specific interference** — covered by EMPStorm/AshLightning kinds;
   no separate solar-cycle authority exists to consume.
3. **Encrypted transmission logs (67.9)** — decoded messages already append
   to `decodedIntelligenceLogs`; dedicated log presentation is UI work.
