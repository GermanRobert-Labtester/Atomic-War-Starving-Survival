# Typed action-result surfacing matrix

Core owns stable failure codes; host/UI owns player-facing wording.

| Domain | Representative typed codes | Projection surface |
|---|---|---|
| Expedition | `missing_fuel`, `vehicle_unready`, `vehicle.refuel_failed`, `vehicle.repair_failed`, `missing_kit` | garage/expedition session and expedition panel |
| Shelter | `maintenance_dependencies_missing`, `research_required`, `maintenance_not_needed`, `missing_maintenance_item` | starting-level HUD and shelter maintenance surface |
| Medical | `missing_medicine`, `research_required`, `patient_unavailable`, `unknown_procedure`, `stale_preview`, `reservation_failed`, `treatment_rejected`, `clinic_no_power` | medical panel, journal, and medical host session |
| Economy | existing trade-session typed rejection/stance/fairness results; plain `MarketSystem` returns `NaN` for an unknown good | economy/trade presenters |

Every new resource-backed path validates before mutation. Shelter service,
scheduled medical reservation/consumption, expedition launch, and existing
trade transactions use the owning inventory/ledger authority. The UI does not
derive a capability from a string or calculate a replacement price.
