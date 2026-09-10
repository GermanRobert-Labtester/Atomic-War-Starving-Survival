# Plan 160 Save Compatibility

Plan 160 adds no save store and no mutable fields to BoneHornCarvingCatalog. Static source data is reloaded from the packaged data authority.

Discovery is persisted through the existing JournalSystem knowledge ledger using the stable keys narrative_disc_<discovery_id>. Only those IDs participate in Plan 160 state. Full transcripts, source arrays, producer lists and derived summaries are not copied into the save.

Compatibility policy:

- A pre-Plan-160 save loads with zero fabricated bone/horn discoveries unless it already contains one of the stable keys.
- Opening a producer discovers each eligible record once. JournalSystem.UnlockNarrativeDiscovered is the single exact-once owner.
- Saving after discovery and restoring before reopening the producer preserves the key and prevents replay or duplicate journal unlocks.
- Reordering the manifest or catalog source arrays does not change discovery identity; NarrativeDiscoveryCatalog reindexes deterministically by source catalog, source record ID and discovery ID.
- Removing or packaging only part of the optional source corpus fails closed for the missing projection; it does not create an inventory or wildlife fallback.
- Downstream state such as items, animal mortality, crafting, condition, combat, fishing, trade, medical state and research has no Plan 160 save copy and cannot be replayed by restore.

The existing campaign envelope and JournalSaveStore remain the persistence authorities. There is no migration that adopts missed historical records into old late-day saves.
