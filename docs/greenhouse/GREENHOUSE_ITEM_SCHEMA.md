# GREENHOUSE ITEM SCHEMA (Plan 91)

Authoritative schema for `Assets/StreamingAssets/Data/greenhouse_items.json`
after Plan 91. Source of truth: `ItemJsonDto` in
`Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` + `ItemTypes.cs`.

## File shape

```json
{
  "schema_version": 1,
  "items": [
    {
      "id": "item_greenhouse_trowel",
      "displayName": "Planting Trowel",
      "description": "1–2 grounded sentences.",
      "type": "Tool",
      "stackMax": 1,
      "weight": 0.4,
      "tradeValue": 6
    }
  ]
}
```

## Field contract used by Plan 91 additions

| Field | Type | Contract |
|---|---|---|
| `id` | string | snake_case, `item_greenhouse_` prefix for non-seed greenhouse supplies. Must be globally unique across all 10 item files (Model A registry). |
| `displayName` | string | 1–3 words, concrete, no raw IDs in UI. |
| `description` | string | 1–2 sentences: what it physically is + what it is for. No numeric effect claims without a live consumer. |
| `type` | string | Exact `ItemType` value. Used by Plan 91: `Tool`, `Material`, `Filter`. (`Food`, `Device`, `Medical`, `Quest`, `ContaminatedFood` appear in baseline entries.) |
| `stackMax` | int ≥ 1 | Physicality: hand tools 1; bulky components/kits 2–6; loose dry goods 8–15. |
| `weight` | float | Kilograms within the game's carry abstraction (baseline spans 0.05–8.0). |
| `tradeValue` | float | Caps/barter points; baseline spans 1–80. |

Fields the Plan 91 additions deliberately omit (no live consumer → no data):
`hungerRestore`, `thirstRestore`, `healthEffect`, `moraleEffect`,
`contamination`, `radProtection`, `durability`, `empShielded`,
`isEquipable`, `scrapValue`, `repairRecipe`. Baseline food/crop entries keep
their own effect fields; supplies gain none (plan §1.11, §49, §50).

## Naming policy (plan §7)

- `item_seed_*` — true seeds/propagules (existing convention; unchanged).
- `crop_*` — harvested yields (existing convention; unchanged).
- `item_greenhouse_*` — greenhouse tools, supplies, repair materials (Plan 91).
- Generic gear already in `items.json` (filters, plastics, hoses, seed
  packets, copper wire) is **referenced by ID**, never duplicated here.
