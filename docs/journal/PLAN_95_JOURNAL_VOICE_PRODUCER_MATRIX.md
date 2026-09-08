# Plan 95 — Journal Voice Producer Matrix

## 1. Audit result

The current journal runtime accepts a caller-supplied string. It does not
translate a food, water, expedition, raid, disease, power, weather,
radiation, roster, death, or guilt state into a journal situation key.

The repository search covered:

- `JournalVoice.ComposeBody` and `JournalSystem.TryDiscover*` callers;
- Core and Godot host string literals;
- data-driven `journalUnlockId` and collectible journal targets;
- the existing `KnowledgeKeys` list and journal tests.

The only matching source literals for the Plan 95 names were unrelated
feedback/UI IDs, tests, documentation, and the prose catalog. Similar names
are not producers.

## 2. Classification

| Key | Status | Exact producer evidence | Overlap / boundary |
|---|---|---|---|
| `low_food` | **DEFERRED** | No journal caller or `journalUnlockId`; `low_food` is a feedback warning ID only. | Food shortage prose is authored, not dispatched. |
| `low_water` | **DEFERRED** | No journal caller or data mapping found. | Distinct from `freezing_shelter`, but currently unrequested by runtime. |
| `death_of_survivor` | **DEFERRED** | No generic survivor-death journal key or caller found. | Do not assume Plan 65/final-wish events supply this key. |
| `successful_expedition` | **DEFERRED** | Expedition journal calls use data-provided `JournalUnlockId`; no exact target exists. | Do not infer success from expedition result code. |
| `failed_expedition` | **DEFERRED** | No exact data target or journal dispatch found. | No casualty/empty-return split was found for this key. |
| `faction_raid` | **DEFERRED** | No exact journal target or call found; `faction_raid_imminent` is feedback only. | Do not invent a faction or raid outcome. |
| `disease_outbreak` | **DEFERRED** | Disease has event IDs and feedback labels, but no journal call with this key. | Event existence is not journal reachability. |
| `power_failure` | **DEFERRED** | Power failure appears in feedback/ritual context, not a journal producer. | Distinct from `freezing_shelter`; no dispatch exists. |
| `new_survivor_arrived` | **DEFERRED** | No exact roster journal target or call found. | Dynamic `survivor_met_<id>` is a different codex key. |
| `severe_cold` | **DEFERRED** | No exact weather journal target or call found. | Keep separate from baseline `freezing_shelter` until a producer defines the boundary. |
| `high_radiation_zone` | **DEFERRED** | No exact expedition/radiation journal target or call found. | Baseline `has_seen_radiation` remains the only matching authored key. |
| `moral_compromise` | **DEFERRED** | Moral-choice/guilt data has journal fields, but no exact `moral_compromise` dispatch. | Do not infer this key from a generic guilt delta. |

## 3. Required downstream integration contract

A future producer task may activate one of these candidates only by using an
existing journal route, for example:

```csharp
journal.TryDiscoverKnowledge(existingSituationKey, author, day, hour);
```

or an already-supported data `journalUnlockId`. That task must prove the
trigger boundary, deduplication frequency, author selection, and save behavior.
It must not silently map a UI feedback ID to a journal key.

Plan 95 intentionally adds no Core dispatch logic, thresholds, event
subscriptions, or new save fields.
```
If the discovery has already been recorded in `KnowledgeBase`, `TryDiscover` returns `null` and takes no action (idempotent deduplication).
