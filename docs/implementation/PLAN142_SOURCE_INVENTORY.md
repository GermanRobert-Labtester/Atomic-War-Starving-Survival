# Plan 142 Source Inventory

## Runtime and persistence

| Owner | Evidence | Responsibility |
|---|---|---|
| `JournalSystem` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` | insertion, deduplication, cap, unread state, events |
| `JournalEntry` / `JournalSave` | `Assets/Ashfall.Core/Journal/JournalEntry.cs`, `JournalSystem.cs` | canonical persisted shape |
| `JournalSaveStore` | `src/Journal/JournalSaveStore.cs` | checksummed `"journal"` campaign section |
| `Main.SetupJournal` | `src/Main.Narrative.cs` | one system construction, restore/seed, UI binding |
| `JournalBookUI` / `JournalPanel` | `src/Journal/JournalBookUI.cs`, `src/UI/JournalPanel.cs` | presentation of canonical entries |

## Loader and source ownership

| File | Loader before Plan 142 | Plan 142 treatment |
|---|---|---|
| `journal_entries_expansion_05.json` | none | ambient adapter input |
| `narrative/journals_expansion.json` | none | ambient adapter input |
| `narrative/journal_entries_batch_1.json` | none | canonical adapter input |
| `narrative/journal_entries_batch_2.json` | none | canonical adapter input |
| `narrative/journal_entries_batch_3.json` | none | canonical adapter input |
| `faction_war_journal.json` | `FactionWarContentCatalogLoader` | remain separate |
| `journal_voice_prose.json` | `JournalVoiceProseCatalogLoader` | remain generated prose |

## Current producer seams

Existing producers already call `JournalSystem` through `TryAddRawEntry`,
`TryDiscoverRawKnowledge`, or `TryDiscoverKnowledge`. They include radio
triangulation, host events, moral choices, autopsy, expedition consequences,
library study, duty roster, shelter systems, faction actions, and later
expansion callbacks.

The map host now also binds `WastelandMapSystem.OnNodeDiscovered` to the
existing journal authority. It uses the authored-key seam for matching
location records and adds no location effect or codex claim.

Plan 142 does not add a parallel event bus, scheduler, archive state machine,
or boot-time loop. The authored corpus is loaded as immutable catalog data and
is consumed only when an existing producer submits a matching key.
