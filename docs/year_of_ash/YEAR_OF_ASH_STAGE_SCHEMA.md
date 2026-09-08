# Year of Ash Stage Schema

Each stage uses the live `QuestStage` fields:

| Field | Live type | Meaning |
|---|---|---|
| `stageId` | string | Stage identity |
| `title` | string | Stage heading |
| `narrativePrompt` | string | Situation presented to the player |
| `unlockOnDay` | int | Authored day metadata; current choice runtime does not enforce it |
| `isTerminal` | bool | Terminal marker |
| `terminalOutcome` | enum value | `2` = Completed, `3` = Failed in the current JSON enum representation |
| `choices` | array | Choices available from the stage |

Plan 114 stages are forward-only and acyclic. Terminal stages contain empty choice arrays, matching
the current runtime path where the preceding choice enters and resolves the terminal stage.
