# Plan 153 save compatibility

Plan 153 reuses the existing Journal knowledge section. No Plan 153 save section, survivor field, faction standing field, resource snapshot, audio state or memorial entry is added.

## Persisted state

Each first discovery calls `JournalSystem.UnlockNarrativeDiscovered`, which persists `knowledge_narrative_discovered_<discovery_id>` through the existing journal/campaign envelope. The source transcript, producer map and derived title are catalog data and are reloaded from the installed data files. Related links are recomputed from the catalog and discovered keys.

## Required cases

| Case | Result |
|---|---|
| Before discovery | no Plan 153 knowledge keys |
| After one record | exactly that stable discovery key |
| Reopen/rebind UI | no mutation or duplicate unlock |
| Save and restore | the key remains discovered; no adapter is re-executed |
| Old save with no Plan 153 keys | loads unchanged; records remain undiscovered until their producer is explicitly inspected |
| Day 40 old save | no retroactive global unlock; an explicit producer inspection may reveal only its mapped records |
| Catalog reordered | deterministic sort by source catalog, source record ID and discovery ID |
| Source/catalog missing | the loader skips unavailable records; existing save keys remain harmless |
| Source record removed | no mechanical replay; the missing projection is absent and validation reports the source mismatch |
| Epitaph read | no `MemorialSystem` call, survivor death change, grief or memorial creation |

The loader rejects duplicate discovery IDs from the projection, while the data-integrity validator reports duplicate manifest IDs. A future Plan 153 field must not duplicate downstream authority; it belongs in the owning save store.
