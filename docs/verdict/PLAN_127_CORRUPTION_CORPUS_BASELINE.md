# Plan 127 Corruption Corpus Baseline

## Before expansion

`verdict_data.json` contained eight opaque corruption strings. They already
covered signal loss, halted sectors, valve/hand absence, tone repetition,
meter loops, census garbling, archive interruption, and held-count
repetition.

## After expansion

The corpus now contains **25 strings**:

| Ordinals | Status | Coverage |
|---:|---|---|
| 0–7 | Preserved | Original Plan 08 machine-corruption lines |
| 8–9 | Added | Timestamped sector garble and census-window corruption |
| 10–12 | Added | Valve failure, repeated signal loss, and count recursion |
| 13–15 | Added | Carrier acknowledgement, blank responsibility register, expired maintenance |
| 16–18 | Added | Archive failure, halted sector, and repeated readout |
| 19–21 | Added | Revised personnel total, absent hand, and tone/silence/tone |
| 22–24 | Added | Operator-record failure, vacant counting house, and denied close request |

All entries remain plain display strings. No parser, placeholder expansion,
recent-entry suppression, or no-repeat rule was introduced.

## Runtime verification

- Loader path: `VerdictCatalogLoader.LoadCorruptionCorpus`.
- Selection: seeded `MachineLogSystem.InsertCorruptionMarker`.
- Persistence: `MachineLogSystemState.entries[*].bodyShort`.
- Ordering policy: append-only to preserve deterministic selection inputs.
