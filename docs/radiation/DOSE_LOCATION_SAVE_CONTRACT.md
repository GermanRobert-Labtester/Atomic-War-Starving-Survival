# Dose Location Save Contract

> **Persistence Contract:** Verification of save/load compatibility, schema stability, and state preservation for `dose_locations.json`.

---

## 1. Zero Save Schema Modification

- **Data-Only Expansion:** Expanding `dose_locations.json` requires **no changes** to `DoseLedgerSave`, `DoseLedgerSaveCodec`, or the campaign save envelope.
- **Persistence Architecture:**
  - `DoseContentCatalog` is loaded statelessly from JSON at boot time by `DoseContentCatalogLoader.Load()`.
  - The runtime save file (`DoseLedgerSave`) persists dynamic ledger records:
    - Survivor entries (`DoseEntry`)
    - Assigned dosimeter tags (`assignedDosimeterTag`)
    - Cumulative millisieverts (`cumulativeMsv`)
    - Reading history records (`DoseReading`), where `source` stores the string location ID.

---

## 2. Backward & Forward Compatibility

| Scenario | Behavior | Result |
|---|---|:---:|
| **Old Save Loaded** | Save containing readings from only the 5 original bunker rooms is loaded. | **PASS** — Old entries resolve properly; catalog expansion has no negative effect. |
| **New Location Visited** | Survivor visits `loc_military_depot_perimeter`; reading booked with `source = "loc_military_depot_perimeter"`. | **PASS** — String ID stored verbatim in `DoseReading.source`. |
| **Save & Reload Round-Trip** | State serialized to JSON and deserialized. | **PASS** — Verified by `Plan81DoseLocationsExpansionTests.DoseLedgerStateCaptureAndRestoreRoundTripPreservesLocationAttribution`. |
| **Mid-Exposure Save** | Saving during an active multi-tick exposure sequence. | **PASS** — Completed readings are persisted; incomplete tick accumulator resumes without double-booking. |
