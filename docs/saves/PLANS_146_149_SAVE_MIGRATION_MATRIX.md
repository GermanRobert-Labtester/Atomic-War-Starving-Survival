# Plans 146–149: Save Migration & Persistence Matrix

**Document ID:** AF-146-149-SAVE-MATRIX
**Status:** Approved & Verified
**Authority:** `SaveStore<T>`, `SaveStoreHub`, `SaveChecksum`, `SaveSectionRegistry`

---

## 1. Registered Save Sections

All 5 state domains introduced in Plans 146–149 are registered in `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` and managed through `SaveStoreHub`:

| Section Key | Store Class | Host Session | File Name | Envelope Pattern |
|---|---|---|---|---|
| `route_infrastructure` | `RouteInfrastructureSaveStore` | Host / World | `route_infrastructure.json` | `{ "State": { ... }, "Checksum": "..." }` |
| `ebpvd_coating` | `EbPvdCoatingSaveStore` | `EbPvdCoatingHostSession` | `ebpvd_coating.json` | `{ "State": { ... }, "Checksum": "..." }` |
| `mine_clearing_flail` | `MineClearingFlailSaveStore` | `MineClearingFlailHostSession` | `mine_clearing_flail.json` | `{ "State": { ... }, "Checksum": "..." }` |
| `microfluidic_diagnostic` | `MicrofluidicDiagnosticSaveStore` | `MicrofluidicDiagnosticHostSession` | `microfluidic_diagnostic.json` | `{ "State": { ... }, "Checksum": "..." }` |
| `rail_grinding` | `RailGrindingSaveStore` | `RailGrindingHostSession` | `rail_grinding.json` | `{ "State": { ... }, "Checksum": "..." }` |

---

## 2. Schema Versions & Backwards Compatibility

### Version Policy
- Every state DTO begins at `SchemaVersion = 1`.
- Any legacy bare-state payload or legacy save envelope missing a section will gracefully deserialize using default constructor states (`allowLegacyBareState: true`).
- Malformed payloads or modified fields with invalid checksums are rejected via `SaveStore<T>` checksum verification, raising warnings in `GodotLog` rather than crashing the session.

### State Migration Table

| System | V1 State Properties | Future V2 Extension Path | Default On Missing |
|---|---|---|---|
| `RouteInfrastructureState` | `Segments` (Dictionary of `RouteSegmentInfrastructureRecord`), `Day` | Multi-lane width parameters, bridge structural load ratings | Empty catalog dictionary, unblocked baseline |
| `EbPvdCoatingState` | `MachineCondition01`, `FilamentHours`, `VacuumPumpHours`, `ChamberShieldingCondition01`, `ActiveJob`, `CompletedRecords`, `MaintenanceFlags` | Dual electron gun configurations, plasma etching stage | New pristine machine (100% condition, 0 hours) |
| `MineClearingFlailState` | `Condition01`, `DrumRpm`, `HydraulicPressureBar`, `ChainLinksRemaining`, `BlastShieldIntegrity01`, `TotalClearedDistanceKm`, `ActiveBreach`, `MaintenanceFlags` | Magnetic anomaly pre-sensors, explosive reactive armor mount | Mounted intact flail (40 links, 100% shield) |
| `MicrofluidicDiagnosticState` | `MachineCondition01`, `MasterMoldCondition01`, `CartridgesManufactured`, `ActiveManufacturingJob`, `ActiveRuns`, `CompletedResults`, `MaintenanceFlags` | High-throughput 96-well immunoassay reader, digital droplet PCR | Pristine reader (100% condition, empty run queue) |
| `RailGrindingEngineState` | `Condition01`, `MotorRpm`, `DownforceBar`, `StoneDiameterMm`, `WaterSuppressorCondition01`, `ActiveJob`, `MaintenanceFlags` | Multi-stone gang grinder draisine, ultrasonic rail inspection | Pristine grinding module (250mm stones, 100% water tank) |

---

## 3. Atomic File I/O & Checksum Integrity

1. **Atomic Writes:** All stores write to a temporary file (`<name>.json.tmp`) and atomically replace the target file via `IFileIO`.
2. **Deterministic Checksums:** `SaveChecksum.Compute(state)` reflection-based hashing guarantees cross-host and cross-platform integrity:
   - Floating-point fields normalized to invariant culture `G9` format.
   - Null and empty collections normalized to avoid false hash drift.
   - Property names evaluated in ordinal sort order.
3. **Automated Verification:** Verified by `Ashfall.Core.Tests/SaveStoreCoverageGateTests.cs` and `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs`.
