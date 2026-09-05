# Plan 81 — Final Regression & Closeout Report

> **Plan 81 — Dose Locations Expansion (3 → 12+ Dose-Ledger Locations)**
> **Radiation Cartography Pillar:** Exposure now possesses geographic reality across shelter, surface threshold, external travel corridors, high-risk expedition perimeters, and faction checkpoints.

---

## 1. Regression & Gate Results

```text
Plan 81 — Final Regression

Build:
- dotnet build Ashfall.csproj: PASS (0 errors, 0 warnings)
- dotnet build Ashfall.Core.Tests.csproj: PASS (0 errors, 2 informational warnings)

Tests:
- dotnet test Ashfall.Core.Tests: PASS (6,782 passed, 0 failed, 0 skipped)
- dose/radiation-specific tests: 75 passed (incl. 11 new tests in Plan81DoseLocationsExpansionTests)

Data integrity:
- godot --headless --path . -- --data-integrity-selftest: PASS (0 findings across 208 catalogs, 10,727 IDs authored, 3,878 reuses)
- godot --headless --path . -- --content-utilization-selftest: PASS (CI gate PASS)
- godot --headless --path . -- --scene-binding-selftest: PASS (22/22 production scenes passed)
- python3 scripts/ci/scene-lint.py: PASS (27 production scenes checked, 0 errors)

Baseline:
- locations before: 5 bunker locations (3 original Expansion 07 + 2 Plan 27 administrative rooms)
- locations added: 9 new locations across 4 missing sectors
- locations after: 14 total locations (100% backward compatible, >= 12 target satisfied)

Schema:
- ID: string (loc_* canonical format, unique)
- displayName: string (human-readable title)
- sector: string (bunker, surface, expedition, external, faction)
- riskLevel: int (0 to 8 authoring abstraction)
- radiationUsv: float (0.01 to 80.0 uSv/h physical exposure rate)
- description: string (1-3 diegetic sentences explaining physical cause)

Radiation semantics:
- stored unit: mSv in DoseLedgerSystem (nominalMsv, bookedMsv, cumulativeMsv)
- input unit: uSv/h (microsieverts per hour) in dose_locations.json
- time base: hours (0.5h per travel/action tick)
- accumulation: (radiationUsv * dwellHours / 1000) * modifiers
- rounding: float math with deterministic clamping
- modifiers: pre-exposure anti-rad (0.5x), personal shielding factor, post-exposure chelation (0.6x), flux ambiguity

Existing bunker locations:
- loc_the_dose_room: 0.02 uSv/h | risk 0 | preserved verbatim
- loc_the_calibration_bench: 0.02 uSv/h | risk 0 | preserved verbatim
- loc_the_childrens_baseline_board: 0.02 uSv/h | risk 0 | preserved verbatim
- loc_the_register_hall: 0.02 uSv/h | risk 0 | preserved verbatim
- loc_the_screening_station: 0.04 uSv/h | risk 0 | preserved verbatim
- save compatibility: 100% verified; old saves load without migration

New sector distribution:
- bunker: 5 locations (35.7%)
- surface: 3 locations (21.4%)
- expedition: 3 locations (21.4%)
- external: 2 locations (14.3%)
- faction: 1 location (7.1%)

New locations:
- shelter exterior approach: loc_shelter_exterior_approach (0.85 uSv/h, risk 1, surface)
- observation post: loc_surface_observation_post (1.75 uSv/h, risk 2, surface)
- contaminated water access: loc_contaminated_water_access (3.50 uSv/h, risk 3, surface)
- irradiated forest edge: loc_irradiated_forest_edge (18.50 uSv/h, risk 5, expedition)
- ruined hospital grounds: loc_ruined_hospital_grounds (28.00 uSv/h, risk 6, expedition)
- military depot perimeter: loc_military_depot_perimeter (45.00 uSv/h, risk 7, expedition)
- frozen wetland crossing: loc_frozen_wetland_crossing (6.20 uSv/h, risk 4, external)
- burned woodland ridge: loc_burned_woodland_ridge (8.40 uSv/h, risk 4, external)
- garrison checkpoint exterior: loc_garrison_checkpoint_gamma_exterior (4.10 uSv/h, risk 3, faction)

Numerical calibration:
- minimum: 0.02 uSv/h (deep shelter rooms)
- maximum: 45.00 uSv/h (military depot crater hardstand)
- risk/rate outliers: zero; higher risk correlates with higher exposure rate
- duration-benchmark issues: verified safe for short sorties; chronic accumulation drives progression
- extreme-value issues: zero NaN, infinite, or negative values

Expedition cross-reference:
- destination 1: loc_irradiated_forest_edge (natural fallout sink)
- destination 2: loc_ruined_hospital_grounds (maps to abandoned_hospital in expeditions.json)
- destination 3: loc_military_depot_perimeter (maps to loc_ordnance_shoulder in expeditions.json)
- invalid refs: 0 invalid references

Exposure flow:
- bunker -> surface: clean step-up from 0.02 to 0.85 uSv/h immediately upon exiting airlock
- external route: sustained 6.20-8.40 uSv/h burden across long travel hours
- expedition arrival: enters acute hot zone (18.5-45.0 uSv/h) during search/scavenge dwell
- expedition departure: ceases destination rate; resumes travel corridor rate
- faction checkpoint: moderate 4.10 uSv/h baseline decoupled from combat hostility

Weather:
- baseline: static in dose_locations.json
- fallout modifier: dynamic weather systems multiply baseline at runtime (impervious for bunker)
- reset after weather: multiplier returns to 1.0x; catalog untouched
- double-counting: verified distinct travel vs. destination vs. weather application

Radiation health:
- ledger handoff: booked readings append to survivor readingsHistory with source = locationId
- health handoff: RadiationSystem applies biological degradation to survivors
- gear/shielding: personal shielding factor attenuates incident dose
- provenance: dosimeter history preserves exact location name and ID

Save:
- old save: loads cleanly
- new surface: booked readings persist with new location IDs
- new expedition: high-dose readings persist and round-trip exactly
- mid-exposure: state captures without lost or double-counted dose
- location exit: departure correctly stops exposure accumulation
- repeated reload: bit-identical serialization

Determinism:
- dose trace: identical inputs + SeededRng produce identical booked readings
- modifier ordering: pre-exposure -> shielding -> post-exposure chelation -> flux ambiguity
- precision/rounding: float stability verified over repeated tick accumulation

Content utilization:
- unreachable locations: 0
- duplicate identities: 0
- staged integrations: all 14 locations consumed by DoseContentCatalog and rendered by DoseRegisterSurface

UI/accessibility:
- sector display: rendered in Dose Ledger UI
- risk display: numeric 0-8 scale supported without color-only reliance
- radiation unit: truthful uSv/h display
- description: concise 1-3 sentences readable without truncation

Exported build:
- catalog packaged: builds/linux/Assets/StreamingAssets/Data/dose_locations.json synced
- lookup: case-sensitive path verified
- accumulation: verified identical to editor/dev execution
- save/reload: clean round-trip

Manual acceptance: PASS
```
