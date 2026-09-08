# Year of Ash Save Contract

No new save schema was introduced. `YearOfAshSave` remains version 5 and persists the existing
`QuestlineSystemState`, including active records, current stage IDs, choice history, day started/
resolved values, completed/failed IDs, and cumulative morale/guilt.

Adding definitions is additive: old active/completed IDs remain stable, and new definitions become
available only through the existing day-window logic. No migration injects expired quests, and no
new quest-specific store or consequence-applied flag was created. Mid-quest and terminal
save/reload behavior remains the responsibility of the existing Year of Ash save path.
