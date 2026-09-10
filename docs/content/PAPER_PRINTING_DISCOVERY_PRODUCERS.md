# Plan 156 — Discovery Producers

Discovery is explicit and producer-backed. timestamp_relative is authored chronology and is displayed after discovery; it is not a universal currentDay unlock. The shared codex keeps undiscovered records locked until one of these contexts is inspected.

| Producer | Context | Source use | Normal route |
|---|---|---|---|
| room_workshop | Shelter workshop/archive surface | Beater maintenance, felt, sizing, ink and stencil notes | Crafting panel |
| room_workshop_heavy | Heavy workshop surface | Press, beater and damaged sizing records | Crafting panel |
| room_workshop_precision | Precision workshop surface | Mould, finishing and type wear records | Crafting panel |
| room_foundry | Foundry surface | Type-metal wear records | Crafting panel |
| loc_printworks | Recovered printworks inspection | Press, ink, stencil and paper-stock provenance | Map location detail |
| loc_municipal_archive | Municipal archive inspection | Watermark, sizing, buffering and ink records | Map location detail |
| loc_excavation_archive_bunker | Excavation/archive bunker inspection | Mould, press, sizing, pulp, ink and stencil records | Map location detail |
| government_bunker | Existing archive desk provenance | Selected mould, sizing, ink and stencil records | Archive desk |

The active mapping contains all 60 records. Fourteen paper-making records have a second explicit producer context, spanning workshop and location/archive classes, while retaining one canonical projection and one discovery ID per source record. This avoids duplicate codex rows and duplicate journal state.

The first vertical slice is loc_printworks inspection → NarrativeDiscoveryCatalog.GetByProducer → JournalSystem.TryDiscover → JournalCodex. The host call is DiscoverPaperPrintingRecords; it filters the exact Plan 156 source allowlist and applies no other state change.

The prototype UndergroundPrintingPressPanel remains outside this path because its current buttons claim paper, ink, morale and leaflet effects without a typed domain binding. A live generated report/template is deferred until a real current-state paper/printing owner exists.
