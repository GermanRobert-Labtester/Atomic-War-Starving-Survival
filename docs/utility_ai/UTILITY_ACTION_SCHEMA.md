# Utility Action Schema Contract

> **Schema Authority:** `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs` (`UtilityActionDef`, `CurvePoint`) and `Assets/StreamingAssets/Data/utility_actions.json`.

---

## 1. Catalog JSON Schema

```json
{
  "schema_version": 1,
  "actions": [
    {
      "id": "action_example",
      "displayName": "Example Action",
      "description": "Concise description of the action.",
      "basePriority": 0.1,
      "weight": 1.0,
      "isOverrideAction": false,
      "tags": [
        "loud_labor",
        "maintenance"
      ],
      "curvePoints": [
        { "x": 0.0, "y": 0.0 },
        { "x": 1.0, "y": 1.0 }
      ],
      "baseScore": 0.4,
      "fatigueGate": 80.0,
      "skillBonusFactor": 0.2
    }
  ]
}
```

---

## 2. Field Specifications

| Field Name | Type | Required | Default | Description / Valid Range |
|---|---|---|---|---|
| `id` | `string` | **Yes** | `""` | Unique action identifier, snake_case with `action_` prefix |
| `displayName` | `string` | **Yes** | `id` | Human-readable title displayed in UI and status logs |
| `description` | `string` | No | `""` | Diegetic description of the task |
| `basePriority` | `float` | No | `0.1` | Additive base priority added to curved score `[0.0, 1.0]` |
| `weight` | `float` | No | `1.0` | Multiplier applied to `(curvedScore + basePriority)` `[0.5, 3.0]` |
| `isOverrideAction` | `bool` | No | `false` | When true, score is not clamped to 1.0, allowing overrides to dominate |
| `tags` | `string[]` | No | `[]` | Categorical and trait-veto tags (`loud_labor`, `menial_labor`, etc.) |
| `curvePoints` | `CurvePoint[]` | No | `null` | Response curve key points `[{x, y}]`; null/empty evaluates as identity |
| `baseScore` | `float` | No | `0.0` | Baseline score used in `EvaluateRaw(context)` `[0.0, 1.0]` |
| `fatigueGate` | `float` | No | `0.0` | If > 0, returns 0 if `context.Fatigue > fatigueGate` `[0.0, 100.0]` |
| `skillBonusFactor` | `float` | No | `0.0` | Scales `context.CraftingSkill` into bonus raw score `[0.0, 1.0]` |

---

## 3. Deserializer Rules & Fallbacks

- `CatalogLocator.LoadWrappedList<UtilityActionDef>` uses `SystemTextJsonSerializer.Options` (case-insensitive property mapping).
- Missing `tags` initializes to `Array.Empty<string>()`.
- Missing `displayName` falls back to `id`.
- Missing or empty `curvePoints` defaults to `ResponseCurve.Identity` (`x=0,y=0; x=1,y=1`).
