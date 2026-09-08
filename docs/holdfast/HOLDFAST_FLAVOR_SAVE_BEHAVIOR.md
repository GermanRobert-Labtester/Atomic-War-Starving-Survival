# Holdfast Flavor Save Behavior

## 1. Persistence Model
- **Holdfast Flavor Data:** `holdfast_flavor.json` is a static, read-only content catalog loaded on application startup into `HoldfastFlavorCatalog`.
- **Dispatch Log Entries:** `HoldfastDispatchLog._entries` is an in-memory `List<string>` capped at `MaxEntries = 64`.
- **Trade Save Storage:** `HoldfastTradeSaveStore` persists trade inventory, balances, and player holdings (`HoldfastTradeSaveState`), but does **NOT** serialize rendered dispatch strings or faction voice copies.
- **Save Integrity:** `HoldfastSaveCodec` captures and restores the core survival and trade envelopes. Adding faction entries to `holdfast_flavor.json` causes zero schema divergence or save checksum alterations.

## 2. Compatibility Assessment
- **Old Saves:** Prior saves load seamlessly. When opening the Holdfast Terminal in a restored save, transactions with newly flavored counterparties immediately render their new specialized voice lines instead of falling back to `NeutralFactionVoice`.
- **Save Round-Trip:** Pinned by existing `HoldfastSaveTests`, `BareSaveStoreSealTests`, and `SaveStoreCoverageGateTests`.
