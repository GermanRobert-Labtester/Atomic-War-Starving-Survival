# PLAN-ORPHAN-SEAL-01 — Appendix AE: Surface Decision Queue

**Generated:** 2026-09-21. Orphans with **neither a candidate host partial nor a
candidate surface route** (Appendices G and N): 23 of 99. These cannot
be wired mechanically — each needs an EP-01 surface decision before a package
can claim it.
**Options per decision:** (a) add a route through the surface owner (needs that
owner's claim), (b) attach to an existing partial by domain even without a name
match (verify the seam first), (c) ship headless-only with a recorded reason,
(d) decide the system is Core-only and register it per Plan 11's rules.
**Rule:** the decision is recorded in the package — not left implicit.

| Authority | File | Decision options |
|---|---|---|
| `CommitmentSystem` | `Commitments/CommitmentSystem.cs` | a / b / c / d — decide at premise check |
| `InternalCommunicationSystem` | `Communication/InternalCommunicationSystem.cs` | a / b / c / d — decide at premise check |
| `CommunicationsSystem` | `Communications/CommunicationsSystem.cs` | a / b / c / d — decide at premise check |
| `ContentOrphanCertificationEngine` | `Content/ContentOrphanCertificationEngine.cs` | a / b / c / d — decide at premise check |
| `CookingSystem` | `Cooking/CookingSystem.cs` | a / b / c / d — decide at premise check |
| `SeasonalCelebrationSystem` | `Events/SeasonalCelebrationSystem.cs` | a / b / c / d — decide at premise check |
| `TerritoryControlSystem` | `Factions/TerritoryControlSystem.cs` | a / b / c / d — decide at premise check |
| `OilseedPressingEngine` | `Farming/OilseedPressingEngine.cs` | a / b / c / d — decide at premise check |
| `SoilReclamationProfileEngine` | `Farming/SoilReclamationProfileEngine.cs` | a / b / c / d — decide at premise check |
| `SecondGenerationMilestoneEngine` | `Generations/SecondGenerationMilestoneEngine.cs` | a / b / c / d — decide at premise check |
| `FoodTypeSystem` | `Kitchen/FoodTypeSystem.cs` | a / b / c / d — decide at premise check |
| `ModSupportSystem` | `Mods/ModDataContract.cs` | a / b / c / d — decide at premise check |
| `SleepAcousticRestEngine` | `Needs/SleepAcousticRestEngine.cs` | a / b / c / d — decide at premise check |
| `CommonTableRationingEngine` | `Nutrition/CommonTableRationingEngine.cs` | a / b / c / d — decide at premise check |
| `PrecisionGlassworksOpticsEngine` | `Optics/PrecisionGlassworksOpticsEngine.cs` | a / b / c / d — decide at premise check |
| `ConfessionSecretSystem` | `Phantoms/ConfessionSecretSystem.cs` | a / b / c / d — decide at premise check |
| `PublicBroadsheetPressEngine` | `Print/PublicBroadsheetPressEngine.cs` | a / b / c / d — decide at premise check |
| `PsychologicalProfileSystem` | `Psychology/PsychologicalProfileSystem.cs` | a / b / c / d — decide at premise check |
| `OutpostSettlementSystem` | `Settlements/OutpostSettlementSystem.cs` | a / b / c / d — decide at premise check |
| `PlayableMetricsAggregationEngine` | `Telemetry/PlayableMetricsAggregationEngine.cs` | a / b / c / d — decide at premise check |
| `VisitorIntegrationSystem` | `Visitors/VisitorIntegrationSystem.cs` | a / b / c / d — decide at premise check |
| `VoiceLineSelectionEngine` | `Voice/VoiceLineSelectionEngine.cs` | a / b / c / d — decide at premise check |
| `CascadeTargetSystem` | `Weather/WeatherGameplayCascadeEngine.cs` | a / b / c / d — decide at premise check |
