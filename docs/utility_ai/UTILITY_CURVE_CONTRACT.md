# Utility Curve Point Contract

Derived from `ResponseCurve` in `Assets/Ashfall.Core/UtilityAI/UtilityAction.cs`.

## Data Shape

```json
"curvePoints": [
  { "x": 0.0, "y": 0.0 },
  { "x": 1.0, "y": 1.0 }
]
```

## X-Axis: rawScore

`x` = rawScore output from `EvaluateRaw()`, which is:

```
rawScore = baseScore + CraftingSkill * skillBonusFactor
```

clamped to [0, 1].

This means the x-axis is **not** a direct world-state value (like hunger or equipment condition). It is the static priority + skill bonus. The curve shapes how this static value maps to the final utility.

## Y-Axis: curved output

`y` = curved multiplier fed into `(curved + basePriority) * weight`.

## Interpolation

Piecewise-linear between consecutive points.

- Points are sorted by x ascending at construction time
- x ≤ first.x → first.y
- x ≥ last.x → last.y
- Between points: `t = (x - a.x) / (b.x - a.x)`, `result = a.y + (b.y - a.y) * t`
- Zero-span segment (b.x ≈ a.x) → b.y

## Special Cases

| Case | Behavior |
|------|----------|
| null/empty | Identity: `f(x) = x` |
| Single point | Returns that point's y for all x |
| Duplicate x | Second point wins (sort-stable) |
| Unsorted points | Auto-sorted at construction |

## Design Implications for Plan 72

Since x is the rawScore (0-1 static priority), the curve primarily serves to:
1. Shape the response at different rawScore levels (e.g., low-priority actions get minimal curve boost)
2. Allow non-linear mapping of the static priority

For the existing 6 actions, all curves are identity: `[(0,0),(1,1)]`. This means the curved output equals the rawScore directly.

For most Plan 72 actions, identity curves are appropriate since the priority hierarchy is established through baseScore values. Non-identity curves could be used to:
- Make an action score very low until a rawScore threshold is crossed
- Give diminishing returns at high rawScore
- Create a sharp activation threshold

But non-identity curves are optional and should only be used when the static baseScore + skill model needs shaping.