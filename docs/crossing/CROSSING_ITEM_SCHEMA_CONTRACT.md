# Crossing Item Schema Contract

## Local Crossing projection

`CrossingItemEntry` is the DTO loaded by `CrossingCatalogLoader`:

```text
id, displayName, description, type, stackMax, weight, tradeValue,
thirstRestore, hungerRestore, moraleEffect
```

The file root is an object with `schema_version` and an `items` array. Existing casing is preserved.

## Global item authority

The same file is consumed by `ItemCatalogLoader` and registered in the global `ItemCatalog`. Its global DTO also accepts `healthEffect` and other inventory fields. Plan 126 uses the existing `healthEffect` field for Off-Ledger Medicine; no new field or type was added.

`ItemType` parsing is case-insensitive and uses the canonical type set documented in `PLAN126_BASELINE.md`. Item IDs are global `item_*` identifiers, not local-to-Crossing keys.

## Validation contract

- IDs are unique within the file and must not collide with any merged item catalog.
- `stackMax` is positive; `weight` and `tradeValue` are finite and non-negative.
- Direct hunger/thirst/morale effects use the existing inventory scale.
- Health treatment is represented only by the existing global `healthEffect` field.
- No item-specific runtime behavior is encoded in JSON.
