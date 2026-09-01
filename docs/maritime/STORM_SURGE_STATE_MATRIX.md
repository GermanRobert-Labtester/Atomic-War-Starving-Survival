# Storm Surge State Matrix & Coastal World-State Contract (Plan 23 / Task 23C)

## Single producer path (non-negotiable)

```
WeatherSystem (FalloutStorm / BlackRain)
      → District8DeepCoastSystem.TickDaily(day, weather)     ← the ONLY surge producer
          → surge state (surgeActiveDay / surgeLastStormDay)
          → IsSurgeActive (dock/berth gate)
          → narrative markers (dc8_surge_began / dc8_surge_aftermath)
                → world_evolution_events.json aftermath entries
                (day + required_flag → WastelandMapSystem node lock / danger)
```

No second weather engine, no maritime event scheduler, no second map-evolution owner.
`District8DeepCoastSystem.TickDaily` is already driven by the live host
(`Main.CampaignOwners` → `_deepCoast.TickDaily(day, _core.Weather)`), so the surge is a
derived consequence of the weather the campaign already rolls.

## Storm surge state matrix

| State | Owner | Derived/persisted | Semantics |
|---|---|---|---|
| Surge active | `District8DeepCoastSystem` | persisted (surgeActiveDay) | begins on first surge-grade storm day; contamination +0.10/day while it runs |
| Surge end | derived | — | recedes after `SurgeRecedeLagDays` (2) consecutive calm days after the last storm day |
| Berth suspension | `IsSurgeBlockingDock` | derived from surge state | `CanStartDockOperation` false while active; `TryStartDockOperation` gated by `CanStartDockOperation` |
| Aftermath markers | narrative flag authority | persisted (deep-coast markers) | `dc8_surge_began` on start, `dc8_surge_aftermath` once per surge end |
| Map aftermath | `WorldEvolutionEngine` | persisted (triggered-event ids) | flag-gated events keyed on `dc8_surge_aftermath` (verified producer) |

## Three authored surge crisis patterns

1. **Harbor Overrun** (`event_evolution_surge_harbor_overrun`, aftermath flag):
   the surge floods the dock approach — locks the service-channel node while the
   water is up; brine contamination rises; recession restores access.
2. **Stranded crew (adapted, no new expedition runtime)** — crisis 2 uses the live
   deep-coast dock-operation reference: an active dock operation during surge onset is
   the strand state; the rescue decision is "end the dive early / send the crane".
   Represented through existing expedition/dock state; no stranding runtime invented.
3. **Wreck shift / new exposure** (`event_evolution_surge_wreck_shift`, aftermath flag):
   surge movement exposes the picket-craft wreck state (map presentation through the
   existing world-evolution machinery) and seeds a Flotilla broadcast hook.

Boundedness: surges require surge-grade weather (already rare in season windows),
last at most (storm days + 2), never fire twice for one storm (day-guarded tick), and
recede automatically. Frequency is bounded by the weather system's own storm weights —
audited in Task 23E.

## Old saves

`surgeActiveDay = -1`, `surgeLastStormDay = -1` defaults → no surge, no fabricated
history; dock gates unchanged. Active-surge state round-trips through the existing
deep-coast capture/restore (fields additive to `District8DeepCoastState`, HoldfastSave
envelope). `RestoreState(null)` yields the sealed, surge-free route.
