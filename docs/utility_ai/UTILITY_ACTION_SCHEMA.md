# Utility Action Schema Contract

Derived from `UtilityActionDef` in `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs`.

## Required Fields

| Field | JSON key | Type | Valid Range | Description |
|-------|----------|------|-------------|-------------|
| id | `"id"` | string | `"action_*"` prefix | Unique action identifier |
| displayName | `"displayName"` | string | non-empty | Human-readable label |
| basePriority | `"basePriority"` | float | ≥ 0 | Additive after curve |
| weight | `"weight"` | float | > 0 | Multiplicative after curve+priority |
| isOverrideAction | `"isOverrideAction"` | bool | true/false | Skip clamp01 if true |
| curvePoints | `"curvePoints"` | array | ≥ 2 points | Piecewise-linear response curve |
| baseScore | `"baseScore"` | float | 0-1 | Static EvaluateRaw baseline |
| fatigueGate | `"fatigueGate"` | float | ≥ 0 | 0=off; raw→0 when fatigue exceeds |
| skillBonusFactor | `"skillBonusFactor"` | float | ≥ 0 | CraftingSkill multiplier |

## Optional Fields

| Field | JSON key | Type | Default | Description |
|-------|----------|------|---------|-------------|
| description | `"description"` | string | `""` | Flavor/behavior text |
| tags | `"tags"` | string[] | `[]` | Tag array for veto/bias |

## CurvePoint Schema

| Field | Type | Description |
|-------|------|-------------|
| `x` | float | rawScore input (0-1) |
| `y` | float | curved output |

- Points are auto-sorted by x ascending at load time
- Clamp to first/last point y at boundaries
- Piecewise-linear interpolation between points
- Empty/null → identity passthrough (x→x)

## Catalog Format

```json
{
  "schema_version": 1,
  "actions": [
    { ... },
    { ... }
  ]
}
```

Loaded via `CatalogLocator.LoadWrappedList<UtilityActionDef>()` — supports both bare array and wrapped object with `schema_version`.

## Validation Rules

1. `id` must be unique, non-empty, and start with `action_`
2. `displayName` must be non-empty
3. `baseScore` must be > 0
4. `weight` must be > 0
5. `basePriority` must be ≥ 0
6. `tags` must be non-null (defaults to `[]` at load)
7. `curvePoints` must be non-null with ≥ 2 entries
8. Unknown fields are silently ignored by deserializer