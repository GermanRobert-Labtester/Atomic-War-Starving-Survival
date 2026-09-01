# Plan 40 — Save Contract

## Save Integration
- `LedgerDebtSystemState` is in `ExpansionHubSave` since v1
- New fields (`creditorId`, `templateId`) default to empty string on old saves
- No save schema version bump required (additive fields)

## Round-Trip Behavior
- `CaptureState()` → deep copy → serialize → deserialize → `RestoreState()`
- All fields survive round-trip
- `closedContracts` preserved permanently
- `ledgerTampered` flag preserved

## Old-Save Compatibility
- Pre-Plan-40 saves load safely
- Empty `creditorId`/`templateId` on old contracts is valid
- No migration needed

## Multi-Debt Persistence
- Multiple active contracts persist independently
- Each contract has unique `debtorId`
- Payments apply to intended debt only
- Consequences do not cross-wire
