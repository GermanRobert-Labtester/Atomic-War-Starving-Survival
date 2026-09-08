# Plan 126 Integration Matrix

## Plan 120 / factions

`crossing_factions.json` uses macro tags in `wants` and `offers` (`charter_draft`, `fuel_stores`, `route_intelligence`, `staple_grain`, and similar). It does not accept item IDs. The semantic candidates are documented below, but no unsupported exact-item fields were added.

| Item candidate | Existing faction macro surface | Exact item link |
| --- | --- | --- |
| `item_granary_receipt` | `staple_grain`, `pledged_goods` | deferred; schema is tag-based |
| `item_arbitration_token` | `verification`, `discreet_arbitration` | deferred; schema is tag-based |
| `item_smugglers_ledger` | `route_intelligence`, `unchartered_salvage` | deferred; schema is tag-based |
| `item_charter_draft` | `charter_draft`, `ratification` | deferred; schema is tag-based |

## Plan 115 / encounters

`CrossingChoiceEntry` has a `cost_items` field, and existing data contains item IDs. However, the audit found no live resolver or choice-application path for the Crossing encounter DTO; references appear catalog/presentation-only at present. No new item was inserted into an encounter cost where it could become an unresolvable or accidentally consumable requirement.

Candidate staged uses are `item_quarantine_bands`, `item_arbitration_token`, and `item_smugglers_ledger`/`item_rejection_notice`.

## Plan 116 / loot and cross-expansion circulation

No new location-loot references were added. The global registry makes all fourteen IDs available to any existing system that already consumes global item IDs, but no safe landed loot/trade recipient was proven for Plan 116 during this pass. Map, medicine, chit, and receipt are the preferred future broadly-circulating objects; charter artifacts should remain local until an explicit source/sink exists.

## Plan 105 / trade specialties

Trade specialties use profession `item_patterns`, not exact Crossing item references. No new specialty rows were required. New IDs remain compatible with the existing pattern-based authority without adding a second demand model.
