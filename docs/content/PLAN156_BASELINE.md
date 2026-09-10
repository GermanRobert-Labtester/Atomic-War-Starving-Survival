# Plan 156 — Baseline and Runtime Gap

The repository contains two typed, engine-agnostic catalogs under Assets/Ashfall.Core/Narrative/:

- PaperMakingCatalog, loading four source files with 8 Hollander, 8 deckle, 7 screw-press and 7 sizing records.
- PaperPrintingCatalog, loading four source files with 8 rag-pulp, 8 ink, 7 typographic and 7 stencil records.

Each catalog therefore loads 30 records, 60 total. Before this activation there were no production callers for either loader; references were limited to the typed definitions and their catalog tests. The existing UndergroundPrintingPressPanel is a hardcoded prototype with no normal player route and is not a valid consequence authority.

The existing safe seam is NarrativeDiscoveryCatalog → JournalSystem → JournalCodex. Plan 156 adds an exact eight-file PaperPrintSourceAdapter, preserves both source catalogs in JournalCatalogs, and adds manifest producer mappings. No source JSON prose or measurement is converted into a crafting, inventory, research, faction, economy, authenticity or propaganda effect.

The active manifest contains 60 Plan 156 projections. It retains one projection per source record and uses explicit producer_ids for alternate contexts on 14 paper-making records. timestamp_relative values remain authored chronology and do not globally unlock a record.

Canonical producer contexts proven during baseline audit are room_workshop, room_workshop_heavy, room_workshop_precision, room_foundry, loc_printworks, loc_municipal_archive, loc_excavation_archive_bunker and government_bunker.
