# ECOLOGICAL_EVENT_MATRIX.md — Plan 28 Task 28J

**Runtime:** the existing `DayStateChangeEvent` stream from `EvolvingWorldDayOwner.TickDay`
(no new event runtime — §1.9 discipline). Events are projected from live migration state;
an event never fires for a population that is not present.

## Migration event pack (6)

| # | Event | Trigger (live state) | Surface | Cap/cooldown |
|---|---|---|---|---|
| 1 | Herd movement | HerdGrazer pack changes sector | `radio_intercept` ("grazing herd sighted leaving X for Y") | 3 reports/day, one per pack-day |
| 2 | Sounder movement | Sounder pack moves | `radio_intercept` ("boar sign heavy on the X–Y line") | same cap |
| 3 | Fish run | CoastalRunner pack moves within water | `radio_intercept` ("fish running the water at X") | same cap |
| 4 | Bird passage | PassageFlock pack moves | `radio_intercept` ("passage birds moving X toward Y") | same cap |
| 5 | Vermin surge | BurrowSwarm pack moves | `radio_intercept` ("burrower sign spreading out of X") | same cap |
| 6 | Moth front | SwarmBlight pack moves | `radio_intercept` ("insect front drifting from X toward Y") | same cap |
| + | Rabid turn | `isRabid && lastThreatFiredDay == day` | `hazard_warning` | once per pack (day-stamped) |

## Anti-spam contract (Task 28AX, live)

- Migration notices fire **only when a pack's sector changed that day** (sector-map diff).
- Hard cap: 3 `radio_intercept` wildlife reports per day (`reported >= 3` guard).
- Resident species get the plain move line, not a dramatic notice (`MigrationNotice` → null).
- Notices never contain population numbers (`MigrationNotice_IsPlausible_...` pins this).

## Deferred event content (needs content passes, not new runtime)

- Dedicated broadcast bulletins (radio.json entries keyed to migration windows) — Plan 24.
- Map sighting markers — Plan 16 UI pass (ECOLOGY_MAP_VISIBILITY.md).
- Disruption event pack (28O) — needs war/corridor state authority (28N design first).
