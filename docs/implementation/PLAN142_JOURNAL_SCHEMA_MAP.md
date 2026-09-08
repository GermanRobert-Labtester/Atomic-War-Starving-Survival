# Plan 142 Journal Schema Map

## Canonical runtime contract

`Ashfall.Core.Journal.JournalEntry` remains the only persisted runtime DTO.
The adapter maps authored source records into the existing raw-entry
insertion path rather than introducing another runtime entry type.

| Authored field | Canonical field | Rule |
|---|---|---|
| `id` | `JournalEntry.Id` | stable authored ID; generated IDs remain unchanged for existing producers |
| `bodyText` or `text` | `Text` | verbatim body; no paraphrase |
| `knowledge_key` | `KnowledgeKey` | canonical batches use it; ambient records use `id` as their stable producer key |
| `author` or `author_id` | `AuthorName` / `AuthorId` | exact survivor IDs resolve to the survivor; unresolved names retain display text and blank `AuthorId` |
| `day` | `Day` | clamp through the existing runtime rule, with invalid source rows rejected by the loader |
| `hour` | `Hour` | canonical batch hour only; ambient is `-1` |
| `timestamp` | `Timestamp` | canonical source timestamp is validated against day/hour; ambient renders `Day N` |
| `title` | adapter metadata | retained by the read-only catalog; not forced into persisted runtime state |
| `type` | adapter metadata | retained by the read-only catalog |
| `tags` | adapter metadata | retained by the read-only catalog |

## Source adapters

### Ambient schema

`id/title/bodyText/day/type/author/tags` is normalized to a canonical authored
record with:

- `KnowledgeKey = id`;
- `Hour = -1`;
- no invented time-of-day;
- stable `Id = id`;
- title/type/tags retained only in the catalog metadata.

### Canonical-shaped schema

`id/text/timestamp/author_name/author_id/knowledge_key/day/hour` is validated
and passed through without rewriting authored body or timestamp text.

## Non-Plan-142 sources

`faction_war_journal.json` stays owned by `FactionWarContentCatalog`. Its
location, voice, and war-day semantics are not collapsed into generic journal
records by this plan. `journal_voice_prose.json` remains the generated
trait-voice catalog.
