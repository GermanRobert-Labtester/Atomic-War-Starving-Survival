# Plan 110 — Regression Matrix

| Area | Evidence | Result |
| --- | --- | --- |
| JSON parse | `jq empty` | PASS |
| Section count | 3 sections | PASS |
| Band count | 7 bands per section | PASS |
| Pool count | 21 arrays | PASS |
| Final line count | 420 strings | PASS |
| Per-pool count | every pool exactly 20 | PASS |
| Empty values | no null/blank strings | PASS |
| Within-pool duplicates | normalized duplicate check | PASS, 0 |
| Slightly-positive whispers | loaded and selected by runtime | PASS |
| Seeded selection | equal seed produces equal sequence | PASS |
| Internal IDs | no quest/event/band identifiers in lines | PASS |
| Runtime state | no new save/index/history state | PASS |
| Context safety | C2 band-only selection | PASS; event-specific lines deferred |

## Test coverage added

`MoralChoiceGossipExpansionTests` verifies the 21-pool/420-line contract,
slightly-positive whisper reachability, seeded selection determinism across all
bands and sections, and absence of internal context identifiers.

## Existing behavior preserved

- Moral thresholds are unchanged.
- Gossip decay is unchanged.
- Section and band lookup are unchanged except for the newly completed
  `SlightlyPositive` whisper arm.
- No new persistence fields or runtime filters were added.
