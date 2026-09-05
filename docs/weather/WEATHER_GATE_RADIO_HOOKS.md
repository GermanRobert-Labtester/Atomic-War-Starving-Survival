# Weather-Gate Radio Hooks (F11)

How weather-gate state transitions become diegetic faction-radio broadcasts.

## Data flow

```text
WeatherSystem.OnWeatherChanged(old, new)
        │
        ▼
WeatherGateEvaluator.CompareWeatherStates(old, new)
        │   pure diff — catalog order, then ordinal gate id
        ▼
WeatherGateTransition[]  ──►  WeatherGateRadioHooks
        │                       rising-edge detection
        │                       one-shot consumption
        ▼
faction_radio_corpus.json  "broadcasts"  (type: "weather_report")
```

## Transition definition

A `WeatherGateTransition` is meaningful when a gate's open state differs
between the previous and current weather. `CompareWeatherStates` is pure:
the same `(previous, current)` pair and the same catalog always produce the
same transition set, in catalog order with ordinal gate-id tie-break. No
`System.Random`, wall-clock, or dictionary iteration order participates.

## World flags and trigger state

Two distinct states per gate, never conflated:

| State | Where it lives | Cleared by |
|---|---|---|
| **World condition** — "the frozen lake is open" | `WeatherGateRadioHooks.WorldConditionOpen` | gate closure (Blizzard → Clear) |
| **Broadcast trigger** — "the opening was announced" | `ConsumedTriggers` (one-shot set) | radio consumption only |

Radio consumption clears **only the trigger**, never the underlying world
condition. A persistent "lake is open" flag is therefore never used as the
sole delivery mechanism for a one-shot broadcast.

## One-shot semantics

Each trigger id fires at most once per transition. A Blizzard lasting
multiple ticks does not re-enqueue the same broadcast: repeated ticks with
no state change produce no trigger, and `Unsubscribe` drops the pending
queue while keeping consumed-trigger state — so a save/load cycle cannot
replay an already-heard broadcast.

## Positive vs negative gates

- **Negative (blocked_weather):** "Blocked — blizzard conditions make the
  exposed pass impassable." Danger closure. Never phrased as waiting.
- **Positive (required_weather):** "Available — frozen crossing open" when
  satisfied; "Requires sustained deep cold to freeze the lake surface" when
  not. The enabling condition is absent, not hostile.

## Privacy boundary

Radio copy names geography only — highland routes, the lake crossing,
lowland basins, the open wasteland. It never contains `route_XX` or
`gate_XX` ids, exact undiscovered coordinates, or hidden location names.

## Determinism guarantees

Same previous weather + same new weather + same catalog ⇒ identical
transition set, identical trigger ids, identical trigger ordering. The
hooks introduce no randomness; `ISeededRng` remains the only random source
in the simulation and is never touched here.

## Save/load behaviour

On `Subscribe` after a load, pending triggers are dropped and consumed
triggers are kept. Broadcasts are never replayed after load.

## Adding a future gate broadcast

1. Add a `weather_report` entry to `faction_radio_corpus.json` `broadcasts`
   with a unique `id`, `faction_id`, `frequency_mhz`, `callsign`, `title`,
   `message`, `signal_strength`, `day_min`/`day_max`, `scheduled`,
   `intel_refs`, `intel_tags`.
2. If the broadcast reacts to a gate transition, no code change is needed —
   the corpus is the authority for radio content.
3. If the broadcast needs a new trigger category, extend
   `WeatherGateRadioTriggerIds` and the mapping in
   `WeatherGateRadioHooks.TriggerIdFor`, then add a corpus entry. Do not
   spread string literals across `WeatherSystem`, radio UI, and tests.
