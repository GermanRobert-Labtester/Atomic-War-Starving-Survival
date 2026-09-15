# Phase 8 — Integrated Cross-System Scenarios + Balance (B5–B8) — landed

> Deterministic Core harnesses (flagship §12 scenarios) over a wired world —
> grid, greenhouse, water treatment, sump, deep well, and perimeter defense
> consuming one another's authoritative outputs with no host present. Same
> discipline as the Plans 210–213 `FlagshipEconomyScenarioTests`.

## Scenario harness (`FlagshipShelterScenarioTests`, 6/6 green)

| Scenario | What it pins |
|---|---|
| **A — normal operation** (30 days) | grid stable + fuel actually burned; well ledger conserves (every yielded liter is raw-pool stock or treatment-processed — the well→treatment→clean loop closes); fed plot grows without dying; treatment runs |
| **B — brownout during a greenhouse cycle** | deterministic priority shedding (low lighting sheds, critical air served, no critical deficit); the dark greenhouse **pauses growth but never insta-kills** the crop (documented severity) |
| **C — flood + treatment emergency** | sump flood raises node contamination; the pump — a Phase 3 registered critical load — is served and drains (power owns availability, sump owns consequences) |
| **D — raid under brownout** | 300W vs 580W demand: the turret's armory circuit sheds at allocation (summary is the allocation-time truth; post-tick overload trips are the grid's own authority), criticals stay served, **no combat authority is touched by power**; D2 pins the full-power served counterpart |
| **G — restore mid-crisis** | restored mid-brownout fires **no Began edge replay**; the well ledger continues (never restarts); blight restores exactly (no re-roll, no cure) |

## 30-day balance soak (three profiles, deterministic seeds)

| Profile | Config | Fuel used | Brownout hrs | Critical days | Well yield | Harvests | Gen condition |
|---|---|---|---|---|---|---|---|
| Early | 500 W, 1 kWh, 2 plots, no well | 360 | 52.3 | **0** | — | 6 | 92% |
| Mid | 800 W, 4 kWh, 4 plots, well | 576 | 15.1 | 0 | 1166 L | 12 | 92% |
| Late | 1400 W, 8 kWh, 6 plots, well | 1000 | 1.0 | 0 | 1166 L | 18 | 92% |

Reading (§19 goals):
- **No death spirals**: even the Early profile records zero critical-deficit days — brownouts force prioritization (52 shed-hours), never life-support loss.
- **No arbitrage**: well yield (≈1166 L/30 d) enters the raw pool and only becomes potable through costed, filter-wearing treatment; harvests scale with plots and water/labor inputs; generator wear accrues on burning days only.
- **Upgrades help visibly**: Mid/Late cut brownout-hours 52→15→1 while all inputs remain nonzero (fuel, wear, nutrients, filter integrity).
- Treatment processing is player-initiated (batches), so the soak's `processed=0` is expected; Scenario A pins the processed path.

## Verification

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Integration/FlagshipShelterScenarioTests.cs` | 6/6 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridPhase5MaintenanceTests.cs` | 9/9 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Water/DeepWellSystemTests.cs` | 10/10 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs` | 6/6 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs` | 13/13 |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

Scratch metrics harness ran and was removed (tables above); permanent coverage = the six scenario tests.

## Files

- `Ashfall.Core.Tests/Integration/FlagshipShelterScenarioTests.cs` (new, 6 scenarios)
- `docs/plans/flagship_b5_b8/PHASE8_SCENARIOS_BALANCE.md` (this file)
