# Plan 107 — Radio Distress Signals Closeout

**Status:** RECONCILED / AUDIT COMPLETE
**Final policy:** Preserve the landed 25-signal Plan 50 authority; do not
truncate or add a duplicate 5 → 20 batch.

## Final catalog state

| Layer | Records | Unique IDs | Role |
|---|---:|---:|---|
| `radio_distress_signals.json` | 25 | 25 | Primary Plan 50 authority |
| `radio_distress_signals_expansion.json` | 16 | 16 | Later NPC-arc and expansion layer |
| Shared IDs | — | 5 | Primary wins in production composition |
| JSON union | 41 rows | 36 | Complete data-backed signal set |
| Built-in compatibility-only IDs | 8 definitions | 4 unique | Sparse-fixture fallback only |
| Composed runtime | — | 40 | 36 JSON IDs + 4 fallback IDs |

## Runtime authority

- `RadioDistressSystem` owns signal definitions, lifecycle state, and save
  markers.
- `RadioTuner` and `RadioPropagation` select the day-appropriate fragment and
  clarity.
- `DistressDestinationResolver` owns canonical expedition destination
  resolution.
- `DistressRescueMissionManager` owns the five authored rescue-mission
  workflows.
- `RadioHostSession` loads both JSON layers without allowing expansion
  duplicates to override primary definitions.
- Existing faction, moral-choice, expedition, and save systems remain
  authoritative. No second radio, quest, expedition, reward, or persistence
  runtime was introduced.

## Cross-plan audit

- **Plan 73:** three faction-corpus broadcasts reference distress IDs. Those
  IDs resolve against the combined JSON authority.
- **Plan 76:** all signal destinations resolve through the existing canonical
  expedition resolver. No signal adds a second expedition definition or
  bypasses route constraints.
- **Plan 82:** no direct Verdict-site reference field is present in the live
  signal schema. No unsupported Verdict hook was invented.
- **Plan 84:** no direct distress reference is present in the live witness
  catalog. Testimony corroboration remains a documented follow-up, not a
  fabricated data link.

## Verification added

`RadioDistressLayerContractTests` proves:

- primary count 25;
- expansion count 16;
- exactly five shared IDs;
- 36 unique JSON IDs;
- primary definitions win duplicate IDs;
- unique NPC-arc expansion records remain loaded;
- every layered signal has contiguous fragments with strictly increasing
  clarity.

## Remaining limitations

The live UI exposes general radio intercept history and tuner controls, but it
does not render a dedicated distress-fragment transcript or a 36-signal
catalog browser. The existing Core and host lifecycle tests prove the
intercept, moral-choice, rescue, trap, persistence, and deterministic replay
paths for the implemented rescue subset. Expanding those UI surfaces or adding
new Verdict/testimony hooks is separate work.
