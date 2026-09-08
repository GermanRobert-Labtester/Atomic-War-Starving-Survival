# Year of Ash Choice Schema

The live `QuestChoice` DTO accepts:

`choiceId`, `text`, `nextStageId`, `moraleDelta`, `guiltDelta`, `grantItemId`,
`grantItemQuantity`, `targetFactionId`, `factionStandingDelta`, `unlockEncounterId`,
`conditions`, and `outcomeNarrative`.

`conditions` are represented as `QuestCondition` objects with `conditionTag` and `isBlocker`.
They are preserved as authored data, but `TakeChoice` currently does not evaluate them. Empty string
IDs and zero quantities are used for absent optional rewards/hooks, following the existing catalog
convention. No new choice properties or condition grammar were introduced.
