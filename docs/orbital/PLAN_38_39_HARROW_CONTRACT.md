# Plan 38 / Plan 39 Harrow Integration Contract

## 1. Authority Division

```text
Telemetry authority:       Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs
Threat authority:          Assets/StreamingAssets/Data/orbital_harrow_events.json
Armor authority:           Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs
Armor catalog authority:   Assets/StreamingAssets/Data/sky_layer_armor_catalog.json
Consequence authority:     Downstream shelter/power/salvage systems + OrbitalImpactReport
Delayed-effect authority:  OrbitalSalvageOpportunity (7-day decay window) + Power disruption cascade
Save authority:            OrbitalTelemetryState + SkyArmorSaveState
Catalog paths:             Assets/StreamingAssets/Data/orbital_harrow_events.json
                           Assets/StreamingAssets/Data/sky_layer_armor_catalog.json
Cross-reference key:       eventId (e.g. event_orbital_kinetic_early_track)
```

## 2. Shared Data Model (Model A/B Reconciled)

- **Plan 38** owns the physical defense layer (`SkyLayerArmorSystem`, `CeilingCellArmor`, `sky_layer_armor_catalog.json` with 6 configurations, absorption thresholds, and repair costs).
- **Plan 39** owns the early warning telemetry layer (`OrbitalHarrowTelemetrySystem`, `OrbitalEventDef`, `orbital_harrow_events.json` with 12 events, lead-times, signal types, false-positive resolution, and radio hooks).
- **Strike Resolution Boundary**: When `TickDay()` reaches `nextImpactDay`, telemetry calculates per-cell energy (halved if braced) and delegates physical penetration math to `SkyLayerArmorSystem.EvaluateKineticImpact()`.
- **Consequence Dispatch**: If breached, `TotalPenetrationDamage` cascades to power grid disruption (`damage * 2.5f`). Salvage opportunities spawn with a 7-day expiration timer, and hidden excavation sites are unlocked deterministically.
