# PLAN-DETERMINISM-REPLAY-13 — Appendix A: RNG Stream Registry

**Generated:** 2026-09-21 from `Assets/Ashfall.Core/Random/CampaignRngStream.cs`
(64 registered streams) with consumer counts across `src/**` (host) and
Core (self/other).
**Use:** DR-13A — every rolling system forks a stream from this registry; the
gate flags any `Fork(` literal not present here. Streams with zero host
consumers are either Core-internal or unwired consumers.

## Stream registry

| Constant | Stream id | Host refs | Core refs |
|---|---|---:|---:|
| `Weather` | `weather` | 240 | 100 |
| `Combat` | `combat` | 122 | 61 |
| `Disease` | `disease` | 56 | 59 |
| `Greenhouse` | `greenhouse` | 58 | 32 |
| `Expedition` | `expedition` | 78 | 91 |
| `Narrative` | `narrative` | 156 | 190 |
| `Echo` | `echo` | 24 | 14 |
| `Economy` | `economy` | 172 | 94 |
| `Radio` | `radio` | 206 | 101 |
| `Social` | `social` | 22 | 27 |
| `MoralChoice` | `moral_choice` | 18 | 28 |
| `Shelter` | `shelter` | 369 | 253 |
| `DutyRoster` | `duty_roster` | 11 | 9 |
| `Muster` | `muster` | 148 | 64 |
| `Foundry` | `foundry` | 113 | 84 |
| `Maritime` | `maritime` | 74 | 24 |
| `DeepCoast` | `deep_coast` | 54 | 5 |
| `Psychology` | `psychology` | 24 | 5 |
| `Medical` | `medical` | 232 | 126 |
| `Events` | `events` | 69 | 152 |
| `LowBackgroundMetrology` | `low_background_metrology` | 2 | 1 |
| `InSarDeformation` | `insar_deformation` | 1 | 1 |
| `HydraulicExtrusion` | `hydraulic_extrusion` | 2 | 1 |
| `RunFlatTire` | `runflat_tire` | 2 | 1 |
| `SofcPower` | `sofc_power` | 4 | 1 |
| `SoundRanging` | `sound_ranging` | 4 | 1 |
| `CvdDiamond` | `cvd_diamond` | 4 | 1 |
| `AmphibiousDraisine` | `amphibious_draisine` | 4 | 1 |
| `WorldEvolution` | `world_evolution` | 9 | 1 |
| `AnomalyHazard` | `anomaly_hazard` | 1 | 1 |
| `CupolaFoundry` | `cupola_foundry` | 0 | 2 |
| `VerticalAscent` | `vertical_ascent` | 0 | 1 |
| `AcousticDetection` | `acoustic_detection` | 0 | 1 |
| `MineralChemical` | `mineral_chemical` | 0 | 1 |
| `AgricultureMutation` | `agriculture_mutation` | 1 | 1 |
| `AgriculturePest` | `agriculture_pest` | 1 | 1 |
| `AgricultureBlight` | `agriculture_blight` | 0 | 1 |
| `DefenseTargeting` | `defense_targeting` | 1 | 1 |
| `DefenseCapture` | `defense_capture` | 1 | 1 |
| `DefenseDamage` | `defense_damage` | 0 | 1 |
| `PsychologyArcTrigger` | `psychology_arc_trigger` | 1 | 1 |
| `PsychologyArcBehavior` | `psychology_arc_behavior` | 1 | 1 |
| `PsychologyRecovery` | `psychology_recovery` | 1 | 1 |
| `WildlifePopulation` | `wildlife_population` | 1 | 1 |
| `CompanionAnimal` | `companion_animal` | 1 | 1 |
| `Bionics` | `bionics` | 5 | 2 |
| `WildlifeMigration` | `wildlife_migration` | 1 | 1 |
| `WildlifeApex` | `wildlife_apex` | 1 | 1 |
| `WildlifeTaming` | `wildlife_taming` | 1 | 1 |
| `EbpvdCoating` | `advanced_mfg_ebpvd_coating` | 1 | 1 |
| `MicrofluidicDiagnostics` | `medical_microfluidic_diagnostics` | 1 | 1 |
| `MineClearingFlail` | `route_engineering_mine_flail` | 1 | 1 |
| `RailGrinding` | `route_engineering_rail_grinding` | 1 | 1 |
| `MetrologyCalibrationDrift` | `metrology_calibration_drift` | 1 | 1 |
| `MetrologyMeasurementNoise` | `metrology_measurement_noise` | 0 | 1 |
| `DfSkywaveJitter` | `df_skywave_jitter` | 0 | 1 |
| `DfFalseSignature` | `df_false_signature` | 0 | 1 |
| `AquaponicsDisease` | `aquaponics_disease` | 1 | 1 |
| `AquaponicsFrySurvival` | `aquaponics_fry_survival` | 0 | 1 |
| `BreachOperatorIncident` | `breach_operator_incident` | 0 | 1 |
| `BreachObstacleSecondary` | `breach_obstacle_secondary_effect` | 0 | 1 |
| `BlackMarketStock` | `black_market_stock` | 1 | 1 |
| `BlackMarketBounty` | `black_market_bounty` | 0 | 1 |
| `BlackMarketDebtEvent` | `black_market_debt_event` | 0 | 1 |
