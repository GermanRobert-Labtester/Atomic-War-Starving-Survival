# Expedition Schema Contract

## 1. DTO Specification

The authoritative DTO schema consumed by `ExpeditionCatalogLoader.cs` and `ExpeditionSystem.cs` is defined as follows:

```json
{
  "schema_version": 1,
  "expeditions": [
    {
      "id": "string",
      "displayName": "string",
      "distanceTicks": 8,
      "dangerLevel": 1,
      "encounterChancePerTick": 0.12,
      "baseStaminaDrainPerHour": 2.0,
      "lootCategories": [
        "item_id_1",
        "item_id_2"
      ]
    }
  ]
}
```

## 2. Field Bounds and Invariants

| Field | Type | Required | Bounds / Constraints | Description |
| :--- | :--- | :--- | :--- | :--- |
| `id` | `string` | Yes | Must match canonical `id` in `locations.json` | Location reference ID. |
| `displayName` | `string` | Yes | Non-empty string | Human-readable site name. |
| `distanceTicks` | `int` | Yes | $\ge 1$, typical $2..22$ | Number of travel ticks each way ($1 \text{ tick} = 0.5 \text{ hr}$). |
| `dangerLevel` | `int` | Yes | $1..10$ | Hazard rating affecting encounter difficulty and loot rolls. |
| `encounterChancePerTick` | `float` | Yes | $[0.05, 0.50]$ | Per-travel-tick roll probability for an encounter. |
| `baseStaminaDrainPerHour` | `float` | Yes | $[1.0, 5.0]$ | Hourly stamina drain applied to expedition members. |
| `lootCategories` | `List<string>` | Yes | Non-empty list of valid `item_id`s | Item IDs eligible for scavenging rolls during looting phase. |

## 3. Parsing and Fallback Rules

1. `ExpeditionCatalogLoader` loads `expeditions.json` first as the primary definition authority.
2. If duplicate IDs appear in secondary catalogs (`locations_expansion3.json`, `locations.json`, etc.), the primary `expeditions.json` entry takes precedence.
3. If `distanceTicks` is omitted or $\le 0$, it is computed as $\text{round}(\text{travelHours} \times 2)$.
4. If `encounterChancePerTick` is omitted, it is clamped to $\text{Clamp}(0.10 + \text{dangerLevel} \times 0.02, 0.05, 0.50)$.
5. If `baseStaminaDrainPerHour` is omitted, it is clamped to $\text{Clamp}(1.5 + \text{dangerLevel} \times 0.25, 1.0, 5.0)$.
