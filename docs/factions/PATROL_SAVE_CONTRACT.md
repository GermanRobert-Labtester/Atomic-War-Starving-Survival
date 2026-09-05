# Flagship Integration — Patrol Save Contract (PATROL-INT-F1-F4)

## Persistence: travel_encounters Section

Patrol encounters persist via `TravelEncounterSaveStore` registered in `SaveSectionRegistry`:
- **Section Key:** `travel_encounters`
- **File Name:** `travel_encounters_save.json`
- **State DTO:** `TravelEncounterState`
  - `chain_stages`: dictionary mapping chain IDs to current integer stage.
  - `encounter_available_day`: dictionary mapping cooldown group / encounter IDs to expiry sim day.
- **Store Architecture:** Thin static façade delegating to Core `SaveStore<TravelEncounterState>` via `SaveStoreHub.Checksummed`.
- **Integrity:** SHA-256 reflection-based `SaveChecksum` envelope with atomic write (temp + rename) and legacy bare-state fallback.

## Save Triad & Host Orchestration
- **Setup:** `Main.SetupTravelEncounters()` restores active cooldowns from disk.
- **Save:** `Main.SaveTravelEncounters()` captures dirty payload for the campaign envelope.
- **Flush:** `Main.FlushTravelEncountersIfDirty()` debounces in-flight mutations.
- **Reset:** `Main.Lifecycle.cs` safely resets `_travelEncounters` in reverse dependency order.

## Round-Trip Contract
- Active cooldowns and chain stages survive campaign save/load intact.
- Expired cooldowns naturally allow re-selection on subsequent sim days.
- Save tampered checksums are rejected by the checksum guard.
