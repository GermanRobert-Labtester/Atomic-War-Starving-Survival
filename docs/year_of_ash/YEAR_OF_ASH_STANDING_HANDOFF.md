# Year of Ash Standing Handoff

Standing remains owned by the existing faction-war system. The live host path reads
`targetFactionId` and `factionStandingDelta` from `QuestChoiceResult`, then calls
`FactionWar.ModifyStanding`. Plan 114 uses canonical faction IDs only and does not add a standing
ledger, thresholds, or duplicate save state.

The new choices include standing routes for all five Year of Ash blocs. Magnitudes stay within the
existing authored range validated by the Plan 114 tests; repeated choice application is prevented by
`QuestlineSystem` choice history.
