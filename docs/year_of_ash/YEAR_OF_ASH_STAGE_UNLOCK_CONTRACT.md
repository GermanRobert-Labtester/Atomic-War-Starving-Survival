# Year of Ash Stage Unlock Contract

The runtime reads `unlockOnDay` into the stage DTO, but the current `QuestlineSystem.TakeChoice`
path does not gate a choice on that value. Plan 114 therefore treats it as schedule metadata and
keeps every new stage unlock day inside its questline availability window. No Core change was made
to introduce a second day gate.

If a future runtime begins enforcing stage unlocks, the authored absolute days are already ordered
along each new forward graph and leave time before the questline `maxDay` for terminal resolution.
