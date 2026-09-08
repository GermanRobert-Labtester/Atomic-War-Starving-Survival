# Year of Ash Questline Schema

The live root is:

```json
{"schema_version": 1, "quests": [...]}
```

Each `QuestlineDefinition` uses:

| Field | Live type | Meaning |
|---|---|---|
| `questlineId` | string | Stable questline identity |
| `title` | string | Player-facing title |
| `synopsis` | string | Crisis summary |
| `factionTag` | string | Canonical owning faction ID/tag |
| `minDay` / `maxDay` | int | Inclusive absolute availability window |
| `firstStageId` | string | Entry stage ID |
| `stages` | array | Nested stage definitions |

The final catalog contains exactly 15 definitions. No Plan 114-only fields were added.
