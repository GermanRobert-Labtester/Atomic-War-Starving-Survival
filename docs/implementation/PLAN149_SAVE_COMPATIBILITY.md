# Plan 149 save compatibility

Plan 149 reuses the existing Journal knowledge ledger. A discovered document writes exactly one key:

`bureaucratic_document_<doc_id>`

The key is captured inside `JournalSave.Knowledge.DiscoveredKeys`, then inside the existing checksummed journal section and campaign envelope. No document definitions or transcripts are copied into saves.

| Case | Expected behavior |
|---|---|
| Before discovery | No bureaucratic document key exists |
| After discovery | Exactly one key exists and the Events codex row unlocks |
| Repeated discovery | `AlreadyDiscovered`; no second codex increment or event |
| Save immediately after discovery | Journal dirty state is flushed through the existing save path |
| Restore after discovery | Row is reconstructed from current catalog plus the stable key; no unlock event replays |
| Old save before Plan 149 | Loads unchanged with zero new document keys |
| Old late-day save with no document keys | No retroactive discoveries are synthesized; the player must visit a producer |
| Catalog reordered | Keys remain ID-based; row order is deterministic by posted day then doc ID |
| Optional source file missing | Catalog is empty and inert; existing saves still load |
| Runtime map missing or malformed | Records are unsafe/withheld or the load fails closed; no producer can activate them |
| Related target missing | The target edge is omitted with a warning; the source record remains readable |
| Static transcript changed after a save | The saved ID remains a discovery fact; current catalog content is shown with its current provenance |

No migration reconstructs guaranteed story discoveries from `posted_day`. This prevents an old Day 40 save from receiving every historical record or any implied mechanical consequence.
