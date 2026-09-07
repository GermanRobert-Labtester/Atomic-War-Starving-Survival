# B5–B8 Authority Map (frozen at Phase 0)

> Supersedes §5 of the flagship brief where they differ. Evidence-based
> corrections from `B5_B8_BASELINE_RECONCILIATION.md` are marked **[CORRECTED]**.
> Change this map only with repository evidence and a note in the completion report.

| Domain | Canonical authority | B5/B6/B7/B8 relationship |
|---|---|---|
| Inventory / items | `InventoryHostSession` + item catalog (`items.json`) | all physical inputs/outputs; every cost is a canonical item ID |
| Water inventory items | `clean_water`, `irradiated_water` items ↔ `WaterAuthority` reservoir mass | **[CORRECTED]** mass-conserving draw/pour already pinned by `WaterAuthorityMassBalanceTests` |
| Research unlocks | `ResearchSystem` (`IsManualUnlocked`, blueprints, points) | capability gates; unlocks ≠ built infrastructure |
| Survivor busy / labor | duty roster / assignment authority (audit exact class in Phase 4) | greenhouse work, repairs, defense setup |
| Water treatment | `WaterTreatmentSystem` (`water_treatment` section) | treatment quality/capacity authority; 4 modes, filter integrity |
| Water spendable reservoir | `WaterTreatmentSystem` pools + inventory water items | **[CORRECTED]** live spendable authority is the treatment pools + inventory items + `ConsumeRation`. The quarantined `WaterAuthorityMassBalanceTests` suite (`Ashfall.Core.Tests/Water/WaterAuthorityMassBalanceTests.cs` is `Compile Remove`d — it targets an in-flight `DrawWater`/`IOutputSink` API not yet in Core) is the conservation contract its owning stream will land; until then `WaterRequestContracts` (Phase 1) is the shared consumer seam |
| Brine / reject water | `BrineWaterSystem` | **[CORRECTED]** already a live system; B7 only audits consumer closure |
| Sump / flood state | `SumpFloodingSystem` (`sump_flooding` section) | producer of flood/contamination incidents; pump state + condition live |
| Power generation/loads | `PowerGridSystem` (`power_grid` section) + `power_grid.json` rooms | rooms = named loads with priorities/breakers; named source contributions |
| Nuclear / geothermal generation | `NuclearCoreLifecycleSystem`, `GeothermalOrcSystem`, `GeothermalAquiferSystem` | publish watts into the grid via `SetGenerationContribution` |
| Weather / season | macro/weather authority | read-only inputs to solar, flood, winter, greenhouse |
| Greenhouse crop state | `GreenhouseSystem` (`greenhouse` section) | trays, growth, blight, harvest; tray moisture is state, not a water counter |
| Apiculture | `ApicultureSystem` (embedded in `GreenhouseState.apiculture`) | **[CORRECTED]** already live |
| Kitchen / nutrition | existing kitchen/nutrition authority | consumes harvest items |
| Disease | `DiseaseSystem` (`DiseaseEngine.TryExpose(DiseaseExposureContext)`) | unsafe-water exposure handoff — this exact contract |
| Radiation / dose | radiation/dose authority | irradiated-water/decon exposure handoff |
| Airlock / visitors | `AirlockSecuritySystem` (`airlock_security` section) | **[CORRECTED]** door/visitor incidents only — NOT fortification |
| Perimeter fortification | `PerimeterDefenseSystem` (`perimeter_defense` section, Plan 203) | **[CORRECTED]** sectors, emplacements, turret ammo/power, assault sim — the Plan 67 owner |
| Raid / faction pressure | warlord doctrine + faction pressure systems | why/frequency/severity of raids — untouched by defense |
| Tactical combat | `TacticalCombatSystem` (+ `CombatBreachingEngine`, `EnemyCompositionSelector`) | final combat resolution; defense never resolves combat |
| Journal / briefing | journal + briefing authorities | consequence reporting only |

## Enforcement rules (unchanged from the brief)

1. One authority per domain — no second counters anywhere.
2. Cross-system effects only via typed requests/events/projections
   (`CommandPreview`/`CommandResult`, `DiseaseExposureContext`, `PowerGridTickSummary`,
   `SetGenerationContribution`, `SetIncomingContamination`).
3. No free research multipliers; research gates builds/crops/recipes.
4. Deterministic shedding/allocation; no RNG in load order.
5. UI is projection; preview/commit with explicit blocked reasons.
6. Restores never replay transition events or re-roll RNG.
