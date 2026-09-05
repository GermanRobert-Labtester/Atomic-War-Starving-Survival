# Plan 85 — Save Compatibility

## Persistence owner

One piece of campaign state was added: the registered-fragment ledger.

- **Field:** `WastelandMapState.RegisteredMapFragments` (`List<string>`, default empty).
- **Persisted by:** the existing `wasteland_map` save section — `WastelandMapSaveStore` → Core `SaveStore<T>` (`SaveStoreHub.Checksummed`, atomic write, checksummed envelope) and the campaign envelope (`campaign.json`, `wasteland_map` section). No new save store, no new section, no triad change.
- **Completion** is derived, never stored. **Reveal** is the pre-existing `Discovered`/`Unlocked` node lists. One persisted truth per fact (§1.1).

## Migration behavior

| Scenario | Behavior |
|---|---|
| Old save (pre-Plan 85, 3- or 6-zone era) loads | `RegisteredMapFragments` missing → deserializes empty; original `Discovered/Completed/Locked/Unlocked` untouched. **No missing-key failure possible** (added field with default, matching the existing DTO style). |
| Old save with partial original-zone progress | Progress values load identically; pinned by `OldSave_OriginalZoneProgress_LoadsAndPreserves_UnderExpandedCatalog`. |
| Old save with an already-revealed original installation (`loc_hidden_relay_bunker` node discovered) | Node stays discovered; zone completion recomputed from fragments + catalog. |
| Expanded catalog vs old save | New zones initialize undiscovered; unknown fragment ids (none exist today) are inert — they cannot complete any zone and are never reinterpreted as owned. |
| New save loaded by an older build | Standard forward-incompatibility of the existing envelope (unknown JSON fields ignored by the serializer); no new unique tolerance rule introduced (§7.6). |
| Restore mechanics | `WastelandMapSystem.RestoreState` mutates state **in place**; the `DamagedMapSystem` bound at session creation keeps reading the same live state. |

## Save-boundary coverage (§85E.3 subset — test-pinned)

Covered: before any fragment; after first fragment; N-1; immediately after final fragment (completion + reveal same call); after reveal (capture/restore round-trip); reload-and-re-register (no double fire); old-save fixture under expanded catalog. Expedition-en-route and cache-open boundaries are owned by the unchanged expedition save flow (`ExpeditionSaveStore`, checksummed).

## Determinism (§85E.4)

- Completion is set-based over stable string ids — catalog order irrelevant (`CatalogOrder_DoesNotAffectProgression`).
- Reveal uses `Discover`/`Unlock`, both idempotent and state-list-backed; no dictionary-iteration or wall-clock dependence.
- All randomness in the loop (fragment rolls, site loot) flows through the existing seeded `ISeededRng`; no reroll-on-reload surface exists because site loot is stateless by design (see loot provenance).
