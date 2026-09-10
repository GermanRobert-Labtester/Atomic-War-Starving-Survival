# Plan 149 baseline

Date: 2026-09-09

The authored source is `Assets/StreamingAssets/Data/narrative/bureaucratic_documents_expansion.json`. It is a schema-version 1 object with collection id `bureaucratic_documents_expansion` and 27 records. The earlier acceptance note called this batch DATA_ONLY; that was accurate for the pre-activation repository state, but it did not describe the current runtime after this plan.

The source contains 14 `doc_type` values, days 1–62, and transcripts of 596–1,520 characters. The source is presentation content. No transcript field is interpreted as an inventory, medicine, population, power, staffing, room, ration, quest or infrastructure mutation.

The runtime now loads the source through `BureaucraticDocumentCatalogLoader`, joins it with the separate `bureaucratic_document_runtime_map.json`, and exposes the typed catalog through the existing Journal Events tab. Discovery writes only `KnowledgeKeys.BureaucraticDocument(doc_id)` through `JournalSystem`.

The existing `JournalSave` / `JournalSaveStore` is the persistence authority. It already stores a sorted knowledge-key set inside the campaign journal section and uses the existing checksummed campaign envelope. No document transcript, duplicate resource value, location object, character object or document-specific save section was added.

The audit found no proven stable room/location IDs for the human-readable `location` strings, and no exact canonical survivor ID is required by the document reader. Names remain display prose unless a separate continuity system proves an identity. The runtime map therefore leaves `location_id` empty for all 27 records.

The selected presentation surface is `JournalBookUI` / `JournalCodex`, which already supports locked and unlocked Events rows and long scrollable bodies. Existing routes discover records by explicit producer context when the corresponding surface opens: archive desk, medical office, shelter records and duty roster. Discovery is day-gated by the authored `posted_day` only after a producer is invoked; there is no global `posted_day <= currentDay` unlock loop.

## Baseline checks

| Check | Result |
|---|---|
| Authored document records | 27 |
| Runtime-map records | 27 |
| ID parity | PASS; no missing or extra mappings |
| Duplicate authored IDs | 0 |
| Duplicate runtime-map IDs | 0 |
| Document types | 14 |
| Posted-day range | 1–62 |
| Proven stable `location_id` mappings | 0 |
| Unsafe records in the shipped runtime map | 0 |
| Simulation effect adapters | 0 by design |
| Existing discovery/save authority reused | JournalSystem / JournalSave |
| Existing reader reused | JournalCodex / JournalBookUI Events tab |

## Reconciliation decisions

- The 27-record current JSON wins over the older 15-record prose descriptions.
- The static numeric claims are retained as authored history or authored notices and are visibly labelled by truth class.
- Requisitions, inspections, maintenance chits and rosters are `template_compatible_record` because their structure could support a future live report. The shipped records remain archival snapshots; no live projection is implemented in this plan.
- No document read creates or changes game state beyond the idempotent journal knowledge key.
- An unresolved mapping, malformed catalog, missing required field or unknown truth class fails closed or becomes `UnsafeUnresolved`; it cannot be discovered as an executable record.
