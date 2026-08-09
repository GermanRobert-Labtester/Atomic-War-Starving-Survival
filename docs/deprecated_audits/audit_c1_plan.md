========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# C-1 Remediation Plan — Per-system decision matrix

| # | System | File | State? | Has Tick? | Decision | Rationale |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | ExcavationSystem | Shelter/ExcavationSystem.cs | yes | no | **WIRE** | Player-facing mechanic; add `ShovelExcavateActionSO` |
| 2 | HiddenStorageSystem | Shelter/HiddenStorageSystem.cs | yes | no | **WIRE** | Player-facing; bind to MentalBreakComfort (already similar) |
| 3 | CeilingCollapseSystem | Shelter/CeilingCollapseSystem.cs | yes | no | **WIRE** | Event-driven from explosion/storm; add tick per day |
| 4 | TunnelingSystem | Shelter/TunnelingSystem.cs | yes | no | **WIRE** | Player-facing; add `TunnelActionSO` |
| 5 | MaterialShieldingSystem | Shelter/MaterialShieldingSystem.cs | yes | no | **WIRE** | Player-facing upgrade; add `UpgradeShieldingActionSO` |
| 6 | AirlockSystem | Shelter/AirlockSystem.cs | yes | no | **WIRE** | Player-facing; bind to scavenger return path |
| 7 | ClothingDegradationSystem | Survivors/ClothingDegradationSystem.cs | n/a (stateless) | yes | **WIRE** | Wire Tick to AtmosphericSystem humidity |
| 8 | CompostSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Player-facing; add `CompostWasteActionSO` + tick |
| 9 | ScrapWeaponSystem | Core/SimulationSystems.cs | n/a (stateless) | n/a | **JUSTIFY** | Pure function: `TryFireWeapon`. No state. |
| 10 | SterilizationSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Player-facing; add `BoilToolsActionSO` |
| 11 | ChelationSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Player-facing; add `BeginChelationActionSO` + tick |
| 12 | WindTurbineSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Player-facing; add `BuildTurbineActionSO` + tick |
| 13 | AntibioticResistanceSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Hook into MedicalSystem.TreatmentItemConsumed |
| 14 | InternalHaulingSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Player-facing; add `HaulLootActionSO` + tick |
| 15 | WeaponMaintenanceSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Hook into HatchDefense / Combat use |
| 16 | RoomAestheticsSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Add tick to apply morale effect |
| 17 | HamRadioSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Add `BuildHamRadioActionSO` + tick |
| 18 | TriageBoardSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Add `SetTriageActionSO` |
| 19 | PolypharmacySystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Hook into MedicalSystem |
| 20 | EscapeHatchSystem | Shelter/EscapeHatchSystem.cs | yes | no | **WIRE** | Player-facing endgame; add `ExcavateEscapeHatchActionSO` |
| 21 | LocationQuestSystem | Core/LocationQuestSystem.cs | yes | no | **WIRE** | Hook into ExpeditionSystem on arrival |
| 22 | ResilienceSystem | Core/SimulationSystems.cs | yes | no | **WIRE** | Hook into NeedsSystem on trauma events |
