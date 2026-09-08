# Plan 121: Independent Faction Branch Expansion — Baseline Reconnaissance Report

## 1. Executive Summary
- **Plan:** Plan 121 — Independent Faction Branch Expansion (8 → 15 branches)
- **Target Catalog:** `Assets/StreamingAssets/Data/independent_faction_branch.json`
- **Linux Mirror:** `builds/linux/Assets/StreamingAssets/Data/independent_faction_branch.json`
- **C# Constants & Authority:** `Assets/Ashfall.Core/Factions/IndependentBranchIds.cs`
- **Loader & DTO:** `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs`
- **Runtime Engine:** `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs`
- **Cross-Branch Coordinator:** `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`
- **Initial Catalog Count:** 8 branches, each with 3 endings (24 endings total).
- **Target Catalog Count:** 15 branches, each with 3 endings (45 endings total).

---

## 2. Pre-Expansion Verification Baseline

| Gate / Command | Baseline Metric | Status |
|---|---|---|
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings | PASS |
| `dotnet test Ashfall.Core.Tests --filter IndependentBranch` | 21 passed, 0 failed | PASS |
| `dotnet test Ashfall.Core.Tests --filter FactionBranchCoordinator` | 17 passed, 0 failed | PASS |
| `godot --headless --path . -- --data-integrity-selftest` | 0 findings across 298 catalogs | PASS |
| `godot --headless --path . -- --content-utilization-selftest` | 581 catalogs, CI gate PASS | PASS |
| `godot --headless --path . -- --scene-binding-selftest` | 25/25 passed | PASS |
| `python3 scripts/ci/scene-lint.py` | 30 scenes checked, 0 errors | PASS |
| `dotnet test Ashfall.Core.Tests` | 9,925 passed, 0 failed | PASS |

---

## 3. Schema & DTO Shape Audit

Inspection of `IndependentBranchCatalog.cs`:
- **Root DTO:** `IndependentBranchDataFile`
  - `int schema_version` (default: 1)
  - `string faction_id` ("faction_independent")
  - `List<IndependentBranchEntry> branches`
- **Branch DTO:** `IndependentBranchEntry`
  - `string id` (snake_case, prefix `branch_ind_`)
  - `string display_name` (Human readable title)
  - `string ponr_flag` (snake_case, prefix `flag_branch_ind_`)
  - `string ponr_trigger` (Irreversible commitment narrative prompt)
  - `string entry_band_min` (e.g. "very_evil", "slightly_evil", "neutral", "positive")
  - `string entry_band_max` (e.g. "very_positive", "slightly_positive", "evil")
  - `int? requires_prpf_standing_min` (Optional nullable int gate, used by IND-3)
  - `bool? requires_hostile_to_military` (Optional nullable bool gate, used by IND-4)
  - `bool? requires_hostile_to_rebel` (Optional nullable bool gate, used by IND-4)
  - `List<IndependentBranchEndingEntry> endings` (Exactly 3 endings per branch)
- **Ending DTO:** `IndependentBranchEndingEntry`
  - `string ending_id` (snake_case, prefix `ending_ind_`)
  - `string band_min` (Moral band lower bound)
  - `string band_max` (Moral band upper bound)
  - `string display_name` (Human readable ending title)

---

## 4. Key Architectural Conclusions
1. `IndependentBranchIds.cs` is an **authoritative whitelist & lookup contract** (Case A). Calling `LockPointOfNoReturn()` throws if a branch ID is not mapped in `PonrFlagFor(branchId)`.
2. Seven new branches are added sequentially: `branch_ind_9_hermit` through `branch_ind_15_prophet`, eliminating any numeric collision with the existing `branch_ind_1` through `branch_ind_8`.
3. Every new branch implements an exhaustive, non-overlapping 3-ending partition across all 7 canonical moral bands, guaranteeing zero undefined fallbacks and deterministic first-match resolution.
