# Phase 9 — UI/Content Honesty Pass (B5–B8) — landed

> Flagship §16.4 + §17: every panel shows only actionable truth, blocked
> reasons name the actual missing thing, research descriptions match landed
> mechanics.

## UI changes (projection only — no new authorities)

| Panel | Change |
|---|---|
| **PowerGridPanel** | Status line now distinguishes **CRITICAL DEFICIT // LIFE SUPPORT UNSERVED** (§8.6 — never silent) from **BROWNOUT // n LOAD(S) SHED (n W)** with real tick numbers; per-room rows show allocation-aware **SERVED/SHED** state (warn-toned shed, muted offline) instead of the global powered read — a critical load shows SERVED during a brownout while optional loads show SHED, matching the Phase 2 allocation |
| **GreenhousePanel** | New **Blight risk** row: the risk *band* (none-prevented / low / elevated / high) plus the current contributors (`contam P · drought ×S · fed −r`) from the Phase 4 `GetBlightRiskProfile` — the exact daily-roll inputs, never a forecast of the roll (§17.3 no false precision); an unfed crop hints that dosing lowers risk |
| **SumpFloodingPanel** | Pump status is now three-state truth: **RUNNING** (toggle + served load), **ENGAGED — LOAD SHED (no grid power)** (warn), **POWER OFF** — the panel can no longer claim RUNNING while the Phase 3 load is shed (`IsPumpEffectivelyPowered` on the host session) |

All changes are read-only projections over landed Core state; the panel-lifecycle
(16 gates) and UI-accessibility (98 controls / 345 labels, zero focus blockers)
selftests pass unchanged.

## §16.4 research-description audit (11 nodes)

| Node | Verdict |
|---|---|
| hydroponics / doser blueprint | OK — nutrient-dosing loop + recipe chain live |
| solar_advanced | OK — battery-bank chain + grid-tie inverter (Phase 2) deliver the promised overnight draw |
| deep_well_hydraulics | OK — Phase 6 build → 40 L/day raw |
| greenhouse_microclimate | OK — Phase 4 winter light compensation (powered room required) |
| fortified_chokepoints | OK — Phase 7 gates barricade + outer gate |
| automated_sentry_doctrine / turret_controller_blueprint | OK — Phase 7 gates both turrets; the chip is a real cost |
| **defensive_tripwire_arrays** | **FIXED** — the description promised early-warning rigs; `def_tripwire_flare_line` (the Plan 203 alert device) was ungated. Now gated on the node; pinned by test |
| **water_condenser_blueprint** | **FLAGGED (documented Phase 6)** — condenser build deferred; the desal-still recipe bug (`resultAmount: 0`) needs the content stream first |
| **iff_transponder_blueprint** | **PARTIAL (documented Phase 7)** — beacon crafts into comms gear; the automated-defense-grid encounter does not exist; bypass consumer deferred, never wired to human raids |

## Files changed

- `src/Host/PowerGridHostSession.cs` (LastTickSummary projection)
- `src/UI/PowerGridPanel.cs` (status line + served/shed room rows)
- `src/UI/GreenhousePanel.cs` (blight-risk contributors row)
- `src/Host/SumpFloodingHostSession.cs` (served-state query)
- `src/UI/SumpFloodingPanel.cs` (three-state pump truth)
- `Assets/StreamingAssets/Data/perimeter_defenses.json` (tripwire gate)
- `Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs` (+1 gate test)
- `docs/plans/flagship_b5_b8/PHASE9_UI_HONESTY.md` (this file)

## Verification

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefensePhase7Tests.cs` | 7/7 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/PerimeterDefensePlan203Tests.cs` | 16/16 PASS |
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | PASS (all gates) |
| `godot --headless --path . -- --ui-accessibility-selftest` | PASS — zero focus blockers, all labels readable |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `godot --headless --path . -- --content-utilization-selftest` | PASS — 0 hard failures |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |
