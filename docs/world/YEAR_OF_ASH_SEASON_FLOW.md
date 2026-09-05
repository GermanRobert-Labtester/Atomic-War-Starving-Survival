# Year of Ash Season Flow

## Current authoritative flow

```text
simulation day [180..360]
        ↓
YearOfAshTimelineSystem
        ├─ phase
        ├─ temperature / ash / radon / thermal stress
        └─ phase one-shot notices
        ↓
DeepFreezeSystem / RadonSystem / FactionWarSystem / quest systems
        ↓
YearOfAshSaveCodec
```

The timeline owns the day-to-phase mapping:

- 180–240: Deep Freeze
- 241–300: Faction Siege
- 301–360: Great Thaw

The day is authoritative. Restore derives the phase and environmental
parameters from the clamped day rather than trusting an inconsistent serialized
phase. Repeated or out-of-order day advances are ignored, which keeps replay
and save/load from re-firing phase transitions.

## Cross-system boundaries

- Deep Freeze owns intake icing and thermal balance.
- Radon owns fissures, scrubber wear, and dose.
- Faction War owns standing, tension, and artillery simulation.
- Ice Road owns Holdfast seasonal travel windows.
- The Year of Ash timeline does not shadow Ice Road state.

## Honest integration status

There is not yet a dedicated Year of Ash storm-window catalog or a shared
economy modifier owned by this timeline. Those additions require an authored
data schema and an explicit consumer mapping to World Weather, Ice Road, and
trade. Until then, the UI must display the existing timeline parameters and
must not claim that a storm window or ice-road state is controlled by this
system.
