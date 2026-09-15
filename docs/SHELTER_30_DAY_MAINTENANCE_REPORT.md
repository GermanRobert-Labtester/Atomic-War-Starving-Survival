# Shelter 30-day maintenance report

Deterministic Core characterization in
`StartingLevelMaintenance30DayTests`.

## Ordinary powered baseline

- initial filter integrity: `100%`;
- degradation: `5` points/day;
- first actionable warning: day `12` (`<50%` after the tick);
- ignored failure: day `21`;
- competent path: service at critical band on days `12`, `17`, `22`, and `27`;
- service parts consumed: `4` `scrap_mechanical`;
- remaining service parts from an initial `8`: `4`;
- day-30 integrity: `55%`, not failed.

This is maintainable but not free: ignoring the warning reaches failure, while
the competent path spends real inventory four times in thirty days.

## Research characterization

With the shared `knowledge_air_filtration` capability present, the owner uses
`2/3` of the base degradation rate. The 20-day unresearched reference reaches
failure; the researched 30-day reference reaches the same endpoint, showing
the intended approximately 1.5× lifespan under controlled conditions. The
capability is queried, not duplicated into shelter save state.

The thermal and room-shielding systems remain under their existing authorities;
this pass does not create a second thermal model or a free global efficiency
multiplier.
