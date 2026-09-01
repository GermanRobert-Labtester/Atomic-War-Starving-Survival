# Plan 10 Save & Persistence Compatibility

**Document:** `docs/combat/PLAN10_SAVE_COMPATIBILITY.md`
**Status:** Validated
**Authority:** `SaveStoreHub.cs`, `CampaignEnvelopeBuilder.cs`

---

## 1. Save Compatibility Invariants

1. **Pre-Plan-10 Legacy Save Compatibility:**
   - Pre-Plan-10 saves containing baseline vehicles (`vehicle_utility_quad`, `vehicle_dirt_bike`, `vehicle_cargo_truck`) load cleanly without missing field errors.
   - Newly authored vehicles, weapons, and dive sites integrate into unlocked rosters without overwriting active instances or reinterpreting completed flags.
2. **Deterministic Roundtrips:**
   - Active garage inventories, vehicle repair conditions, and expedition fuel levels survive save/load roundtrips with byte-identical checksum parity.
   - Maritime dive progress (air remaining, search stage, noise accumulation, completed room states) persists deterministically through `MaritimeDiveSave`.
3. **No Shadow State:**
   - All state mutations route strictly through `Assets/Ashfall.Core/` system instances and are serialized through `SaveStore<T>` / `SaveEnvelopeHelper`.
