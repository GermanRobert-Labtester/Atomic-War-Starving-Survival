# ASHFALL — Audit Issues 11–25 Closeout

**Branch:** `feat/asset-pipeline-flagship`
**Date:** 2026-09-05
**Scope:** Local post-PR #36 audit findings 11–25 (Critical/High/Medium). Uncommitted with issues 1–10 wave.

---

## Disposition Matrix

| # | Severity | Finding | Disposition |
|---|---|---|---|
| **11** | H | `EvaluateGrandTreaty` treated any ratified regional treaty as grand treaty | **FIXED** — only grand/constitution treaty ids, ratification flags, or explicit override qualify. Ordinary `ratified > 0` now returns false with trace. |
| **12** | H | UI gate tests quarantined (`AccessibilitySourceAudit`, `PanelSubscriptionHygiene`) | **FIXED** — Compile Remove lifted; `WeatherForecastPanel` uses named `_onWeatherChanged` for subscription symmetry. `PlayerSurfaceLiveness` already live. |
| **13** | H | ~19 stub panels `Bind(object?)` | **SHELVED** — concurrent closeout marked prototypes `PanelMaturity.Prototype` / `!IsPlayerNavigable` (29 entries). |
| **14** | H | Greenhouse Plan-22 UI gaps | **PARTIAL** — GAP-1 seed picker, GAP-3 supply strip, GAP-6 water split, GAP-7 readiness live. GAP-2/4/5/8 blocked: Amend/Sterilize/maintenance APIs removed from Core; documented in `GreenhousePanel` comments. |
| **15** | H | 8 catalog loaders allowlisted unwired | **REDUCED** — Atmosphere/Environmental/DebtTemplate/Collectible now production-wired; allowlist retains DynamicQuestline + designed-dormant (SkyLayerArmor, Spiritual, HoldfastNpc). |
| **16** | M | LoaderWiring gate only scanned `LoadAndRegister` | **FIXED** — gate accepts `LoadAndRegister` **or** `Load(...)`; `FormerlyAllowlistedLoadFeeders_AreProductionWired` pins DebtTemplate/Collectible/Atmosphere/Environmental. |
| **17** | M | Fire panel fake brigade IDs `sv_a`/`sv_b` | **FIXED** (prior closeout) — live roster only via `RosterWorkerProvider`. |
| **18** | M | `ShelterFireHostSession` unused by player-surface bind | **FIXED** — `fire_incident` / `OpenFireIncidentPanel` bind `_shelterFireSession`. |
| **19** | M | Arc fires single-zone / no adjacency | **FIXED** — `VentilationSystem.BuildArcFireZones` builds source + `vent_duct_main` (+ sibling electrostatic) adjacency graph. |
| **20** | M | `FalloutContaminationProvider` never set on host | **FIXED** — `FalloutSystem.GetLocationContamination` + `SetupSurvivors` / `BindFalloutContaminationProvider` wiring. |
| **21** | M | Epilogue `Bind(snapshot)` then re-eval in `RefreshView` | **FIXED** — `RefreshView` prefers bound snapshot classifications/prose. |
| **22** | M | PanelRegistryBootstrap deps wrong for fire/epilogue | **FIXED** — `epilogue` → expansions/survivors/verdict/regional_treaty/muster; `fire_incident` → survivors/shelter_fire. |
| **23** | M | `OpenMoralChoiceModal(null)` silent no-op | **FIXED** — status-label feedback + `GD.Print` when catalog empty. |
| **24** | M | PRPF standing stub (no daily influence) | **FIXED** — `PrpfStandingSystem.TickDay` (joined +1 alignment / opposed −1 standing / no-op pre-commit) via `FactionBranchCoordinator` + campaign day owner. |
| **25** | M | Atmosphere/Environmental catalogs no consumer | **FIXED** — `WorldHostSession` `LoadAndRegister` + `FlavorTextForLocation`; `ExpeditionPanel` consumes flavor in world-state line. |

---

## Tests Added / Extended

- `CampaignOutcomeEvaluatorTests.GrandTreatySigned_WhenOnlyOrdinaryRegionalTreatyRatified_IsFalse`
- `FalloutSystemTests.GetLocationContamination_*` (sum + seal attenuation)
- `PrpfStandingSystemTests.TickDay_*` (no-op / joined / opposed)
- `ElectrostaticFiltrationEngineTests.ArcFault_*` asserts `vent_duct_main` adjacency
- `LoaderWiringGateTests` rewrite + formerly-allowlisted feeder facts

---

## Reviewer follow-ups (post Block)

- `FireIncidentPanel` active path re-enables `_brigadeButton` after empty/offline disable.
- `WeatherForecastPanel` `_ExitTree` unsubscribes named weather handler.
- Grand-treaty flag path narrowed to grand/constitution flags only (+ regression test).
- `PrpfStandingSystem.TickDay` day-dedupe via `lastTickedDay` (+ idempotency test).

## Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS (0 errors)
dotnet test  --filter CampaignOutcome|Fallout|Prpf|LoaderWiring|Electrostatic|Accessibility|PanelSubscription|PlayerSurfaceLiveness
                                            # PASS 59/59
dotnet build Ashfall.csproj                 # PASS (0 errors; 2 pre-existing obsolete warnings)
godot --headless -- --data-integrity-selftest  # PASS
godot --headless -- --bridge-selftest          # PASS
```

---

## Known Remaining (out of 11–25 scope)

- Greenhouse GAP-2/4/5/8 need Core Amend/Sterilize/maintenance API restoration before UI can close.
- Designed-dormant loaders (SkyLayerArmor, Spiritual, HoldfastNpc, DynamicQuestline) remain allowlisted by design.
- Fire bind wiring still duplicated across PlayerSurfaces + OpenFireIncidentPanel (MEDIUM nit).
- Issues 26+ of the local audit remain open.
