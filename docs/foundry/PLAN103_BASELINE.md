# Plan 103 — Baseline Reconnaissance & Execution Contract

**Plan:** Plan 103 — Foundry Treaty Consequences Expansion: 6 → 15 Consequence Policies
**Data Authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**Treaty Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Host Runtime Policy:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
**System Coordinator:** `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` / `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Baseline State (Pre-Plan 103)

- **Total Authored Policies:** 6
- **Total Covered Treaties:** 3 (`treaty_brine_pipe_and_iodine_exchange`, `treaty_cluster_labour_schedule`, `treaty_road_iron_charter`)
- **Total Treaties in Authority (`foundry_accords.json`):** 12 (4 District 8 treaties + 8 regional wasteland treaties)
- **Exempt Treaty:** `treaty_the_cluster_charter` (0 policies by design — finale constitution marker, regression-guarded in test suite)
- **Signatory Faction in Baseline:** `faction_silent_foundry` (all 6 rows)
- **Pre-change Test Suite Status:**
  - `dotnet test Ashfall.Core.Tests`: 7,301 tests passing (0 failed).
  - `--data-integrity-selftest`: 0 errors, 215 catalogs clean.
  - `--content-utilization-selftest`: CI gate PASS.
  - `--scene-binding-selftest`: 22/22 passed.
  - `scene-lint.py`: 0 errors across 27 scenes.
  - `--silent-foundry-selftest`: 26/26 passed.

---

## 2. Expansion Objectives & Mathematical Reconciliation

The plan objective is expanding `foundry_treaty_consequences.json` from **6 to 15 consequence policies**.

### Coverage Arithmetic
- Total Target Policies: 15
- Baseline Policies: 6
- New Policies to Add: 9
- Available Non-Exempt Treaties: 11
- Arithmetic Constraint: 15 policies cannot provide 2 outcomes for all 11 treaties ($11 \times 2 = 22$).
- **Canonical Distribution Selected:**
  - 7 treaties have 2 outcome policies ($7 \times 2 = 14$):
    1. `treaty_brine_pipe_and_iodine_exchange` (`met`, `missed`)
    2. `treaty_cluster_labour_schedule` (`met`, `violated`)
    3. `treaty_road_iron_charter` (`met`, `missed`)
    4. `treaty_flotilla_saline_corridor_concordat` (`met`, `missed`)
    5. `treaty_switchback_fuel_and_passage_accord` (`met`, `violated`)
    6. `treaty_deep_coast_aquifer_protection_treaty` (`met`, `violated`)
    7. `treaty_garrison_grain_tithe_compact` (`met`, `violated`)
  - 1 treaty has 1 outcome policy ($1 \times 1 = 1$):
    8. `treaty_scale_suburban_fair_trade_convention` (`met`)
  - Total Policies: $14 + 1 = 15$.
  - 3 remaining treaties staged for follow-on expansion:
    9. `treaty_scrap_salvage_demarcation`
    10. `treaty_roster_border_demilitarization_pact`
    11. `treaty_high_scarp_observatory_sanctuary`

---

## 3. Schema & Dispatch Architecture

1. **DTO Shape:**
   - `schema_version`: integer (`1`)
   - `collection_id`: string (`"foundry_treaty_consequence_policy"`)
   - `policies`: array of policy objects:
     - `treaty_id`: string (exact foreign key to `foundry_accords.json`)
     - `faction_id`: string (exact signatory from `foundry_accords.json`)
     - `outcome`: string (`"met"`, `"missed"`, `"violated"`)
     - `standing_delta`: float (signed trust delta)
     - `reason`: string (institutional prose explaining consequence)
     - `market_modifiers`: list of good demand adjustments (`good_id`, `demand_delta`, `reason`)

2. **Idempotency:**
   - Evaluated by `SilentFoundrySystem.AssessTreatyCompliance(int day)`
   - Checked via `SilentFoundryConsequenceState.IsApplied(treatyId, cycleMarker)`
   - `cycleMarker = day` (assessment day), preventing duplicate application on reload, save-load roundtrip, or repeated same-day ticks.
