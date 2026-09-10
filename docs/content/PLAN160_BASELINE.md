# Plan 160 Baseline

The current data authority contains four typed bone, horn and antler catalogs under Assets/StreamingAssets/Data/narrative. They load through BoneHornCarvingCatalog and contain 8 preparation logs, 8 sawing records, 7 scraping/polishing reports and 7 needle/awl/hook assays: 30 records total.

Before this activation there was no production caller for BoneHornCarvingCatalog, no narrative discovery adapter, and no producer-backed journal route. The source records therefore described authored craft history but did not reach a player-facing surface.

The runtime seam used by this plan is the existing NarrativeDiscoveryCatalog → JournalSystem → JournalCodex path. Plan 160 adds a typed BoneHornSourceAdapter and 30 manifest entries. The adapter reads only the four allowlisted schemas and emits NarrativeDiscoveredRecord values. It has no effect fields and no calls to wildlife, survivor, inventory, crafting, condition, combat, fishing, medical, trade or research authorities.

Forensic findings:

- Source IDs are unique across the four files. The records have no authored tags, timestamps, stable cross-record references or canonical item IDs.
- Animal labels are generic or material labels. None is an exact survivor identity. Dog records are never matched to a living companion; the deer antler source explicitly says old shed in antler_horn_001.
- Saw labels, blank labels, abrasives and finished tool labels do not resolve to exact current item IDs. Related items such as surgical_saw and phonograph_needle remain unrelated context.
- Existing producer contexts are shelter workshop rooms and proven world locations. No carcass-processing or wildlife producer was added.
- No process chain is asserted in the manifest because compatible names are not stable authored foreign keys.

The current combined narrative manifest contains 243 projections after this activation. The Plan 160 portion contributes 30 records and seven producer contexts with records. Discovery state remains in the existing JournalSystem knowledge ledger; the static catalogs remain immutable.

Verification status is recorded in PLAN160_COMPLETION_REPORT.md. Current repository-wide verification may also report unrelated concurrent-stream blockers documented there.
