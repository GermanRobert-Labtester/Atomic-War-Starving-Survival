# Plan 142 Baseline

Status: reconnaissance complete; implementation pending.

## Runtime authority

`Ashfall.Core.Journal.JournalSystem` and `JournalEntry` are the only live
journal state machine and entry contract.

`JournalEntry` currently persists:

- `Id`
- `Text`
- `Timestamp`
- `AuthorName`
- `AuthorId`
- `KnowledgeKey`
- `Day`
- `Hour`

`JournalSystem` inserts newest-first, deduplicates through `KnowledgeBase`,
caps the visible journal at 64 entries, and emits insertion events only for
new entries. `RestoreState` rebuilds state without mutation events.

## Source census

| Source | Records | Shape | Current status |
|---|---:|---|---|
| `journal_entries_expansion_05.json` | 28 | ambient `id/title/bodyText/day/type/author/tags` | optional, no runtime loader |
| `narrative/journals_expansion.json` | 40 | same ambient shape | codex-only, no runtime loader |
| `narrative/journal_entries_batch_1.json` | 18 | canonical-shaped raw entries | codex-only, no runtime producer |
| `narrative/journal_entries_batch_2.json` | 15 | canonical-shaped raw entries | codex-only, no runtime producer |
| `narrative/journal_entries_batch_3.json` | 88 | canonical-shaped raw entries | codex-only, no runtime producer |
| **Plan 142 ambient/canonical corpus** | **189** | two source schemas | not activated |
| `faction_war_journal.json` | 26 | faction-war-specific `authorName/body/day/locationId/voice` | separate `FactionWarContentCatalog` |
| `journal_voice_prose.json` | 39 keys | generated trait prose variants | consumed by `JournalVoice`, not authored entries |

The faction-war journal and voice-prose files remain outside the Plan 142
generic corpus. They have distinct owners and are not silently re-ingested.

## Duplicate census

Across the 189 Plan 142 records:

- exact IDs: 189 unique, 0 duplicate groups;
- canonical `knowledge_key` values: 121 unique, 0 duplicate groups;
- normalized title/body fingerprints: 189 unique, 0 duplicate groups;
- ambient IDs overlap canonical batch IDs: 0;
- ambient IDs overlap canonical batch knowledge keys: 0.

The two ambient files are not mirrors of each other at record level. The
narrative file mirrors the Expansion-05 schema, not its records.

## Author census

- Expansion-05: 28/28 author strings exactly match canonical survivor IDs:
  `elena_vasquez` and `marcus_olejnik`.
- `narrative/journals_expansion.json`: 0/40 author strings exactly match the
  survivor registry. Its eight role/name strings are preserved as external
  authored display names, not guessed survivor identities.
- Canonical-shaped batches: 121/121 `author_id` values exactly match the
  survivor registry.

## Time census

- Ambient records provide day only, range Day 1–370.
- Canonical-shaped records provide day/hour and an explicit timestamp.
- All 121 canonical timestamps equal the deterministic
  `JournalVoice.FormatTimestamp(day, hour)` result.
- No source contains a timezone, wall-clock, or random timestamp.

## Existing save and UI evidence

The `journal` campaign section is owned by `JournalSaveStore` and persists
the complete `JournalSave`, including entries, knowledge, sequence, unread
state, notification state, and tab state. The campaign envelope uses the
existing `"journal"` registry key. Plan 142 adds no save section and no second
journal state machine.

`JournalBookUI` and `JournalPanel` already render canonical body, author, and
timestamp fields. Ambient title/type/tags are not fields on `JournalEntry`;
they remain adapter metadata unless a later UI contract explicitly projects
them.

## Baseline verification

Command:

```text
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Journal" --no-restore
```

Result: **PASS, 110/110**.

## Baseline decision

Use an explicit ambient-to-canonical adapter and a producer-bound activation
API. Loading a catalog must not flood a fresh journal or fabricate a discovery
event. Existing producers call the one `JournalSystem`; the adapter supplies
authored body, stable authored ID, safe author identity, and source timestamp
when a producer's knowledge key matches an authored record.
