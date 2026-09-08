# Year of Ash Content Utilization

The expanded catalog is loaded by `YearOfAshCatalogLoader` and registered by
`YearOfAshHostSession`. The Plan 114 test suite verifies all 15 definitions, all new stage graphs,
canonical faction/item/encounter references, day boundaries, and live choice progression.

The current repository does not expose a dedicated Year of Ash content-utilization command. The
catalog is gameplay-consumed through the Year of Ash host UI and `QuestlineSystem`; door-encounter
IDs are staged in the existing choice result but are not currently consumed by `Main.YearOfAsh`.
That handoff is recorded as deferred rather than counted as live expedition utilization.
