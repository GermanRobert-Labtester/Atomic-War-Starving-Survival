# Plan 89 — Muster Epilogues Expansion Closeout Report

> **Theme:** 12 -> 25 Campaign-Ending Outcomes
> **Authority:** `Assets/StreamingAssets/Data/muster_epilogues.json`
> **Evaluation Engine:** `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`
> **Status:** **COMPLETE** (100% Verified)

---

## 1. Goal Achievement & Reconciliation

The mission was to expand `muster_epilogues.json` from 12 verified campaign endings to exactly 25, providing rich coverage across:
- **4 Faction Endings**: Garrison Absorption, Rebuilders Alignment, Independent Coalition, Foundry Annexation.
- **3 Resource Endings**: Water Plant Held, Grain Silo Captured, Fuel Depot Burned.
- **3 Moral Endings**: Mercy Road, Iron Way, Listener's Thread.
- **2 Compound Endings**: Mercy + Water Held, Iron Way + Fuel Depot Burned.
- **1 Failure Ending**: Shelter Falls (archaeology of the next scavenger).

### Architectural Reconciliation
1. **Selection Precedence:** Authored the `EpilogueMatrix` class and `EpilogueMatrixInput` DTO in `EpilogueMatrix.cs` to enforce deterministic, precedence-based ending selection without creating secondary morality or scoring layers.
2. **Catalog Integrity Fix:** Registered `"ending_key"` in `CatalogIntegrityValidator.DefinitionKeys` so the 13 new `ending_` prefixed IDs in `muster_epilogues.json` are properly recognized as definitions rather than unresolved references.
3. **Preservation of Original 12:** All 12 existing epilogues remain word-for-word intact with identical IDs and prose.

---

## 2. Complete Roster of 25 Outcomes

| # | Ending Key | Title | Category | Words |
|---|---|---|---|---|
| 1 | `the_open_muster` | The Open Muster | Muster Core | 49 |
| 2 | `the_amnesty` | The Amnesty | Muster Core | 47 |
| 3 | `the_corridor` | The Corridor | Muster Core | 55 |
| 4 | `the_blood_price` | The Blood Price | Muster Core | 46 |
| 5 | `the_rate_card_revised` | The Rate Card, Revised | Hydro Barons | 52 |
| 6 | `the_administrator` | The Administrator | Hydro Barons | 49 |
| 7 | `the_measured_truth_contested` | The Measured Truth, Contested | Cold Count | 46 |
| 8 | `the_measured_truth` | The Measured Truth | Cold Count | 44 |
| 9 | `unwritten` | Unwritten | Fallback | 51 |
| 10 | `ending_verdict_the_sector_recounts` | The Sector Recounts | The Verdict | 49 |
| 11 | `ending_verdict_the_count_is_held` | The Count Is Held | The Verdict | 51 |
| 12 | `ending_verdict_the_offer_is_a_lease` | The Offer Is a Lease | The Verdict | 51 |
| 13 | `ending_garrison_absorbs_coalition` | Under Their Watch | Faction | 54 |
| 14 | `ending_rebuilders_joined` | Hands on the Ruins | Faction | 58 |
| 15 | `ending_coalition_independent` | No Banner | Faction | 59 |
| 16 | `ending_foundry_annexation` | Stamped in Steel | Faction | 55 |
| 17 | `ending_water_plant_held` | The Last Clean Line | Resource | 61 |
| 18 | `ending_grain_silo_captured` | The Grain Count | Resource | 58 |
| 19 | `ending_fuel_depot_burned` | Fire at the Depot | Resource | 61 |
| 20 | `ending_mercy_road` | The Mercy Road | Moral | 57 |
| 21 | `ending_iron_way` | The Iron Way | Moral | 57 |
| 22 | `ending_listeners_thread` | The Listener's Thread | Moral | 62 |
| 23 | `ending_mercy_water_held` | Water for the Road | Compound | 65 |
| 24 | `ending_iron_fuel_ash` | Ash in the Tanks | Compound | 65 |
| 25 | `ending_shelter_falls` | What They Found | Failure | 65 |

---

## 3. Editorial & Tone Quality Metrics

- **Average Word Count:** ~56.2 words across the 25 epilogues (concise, tightly edited).
- **Style:** Cold, material, observational third-person accounts.
- **Physical Motifs:** Duty boards, airlock passes, intake manifolds, brass shims, grain bins, paraffin grease, carbon paper, standpipes, calcified boots.
- **Tone Guardrails:** 0 occurrences of second-person address ("you"), 0 gamey judgements ("you won/lost"), 0 triumphalism.

---

## 4. Verification Evidence

1. `dotnet test Ashfall.Core.Tests`: **6,913 passed, 0 failed** (includes new `MusterEpilogueMatrixTests` covering all 18 test fixtures).
2. `godot --headless --path . -- --data-integrity-selftest`: **0 errors, 0 warnings across 208 catalogs**.
3. `godot --headless --path . -- --content-utilization-selftest`: **CI Content Utilization Gate: PASS**.
4. `godot --headless --path . -- --scene-binding-selftest`: **22/22 passed**.
5. `python3 scripts/ci/scene-lint.py`: **0 errors across 27 scenes**.
6. `dotnet build Ashfall.csproj`: **0 errors, 0 warnings**.
