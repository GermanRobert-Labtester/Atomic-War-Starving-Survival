# Utility Override Contract

> **Emergency Precedence:** Semantics and constraints governing `isOverrideAction` in `UtilityActionScorer.cs`.

---

## 1. Override Mechanics

Standard utility actions clamp their final score to `[0.0, 1.0]`.

When `isOverrideAction == true`:
```csharp
if (action.isOverrideAction)
    return Math.Max(0f, score); // Unclamped > 1.0 allows override dominance
```

This allows actions with high weight (e.g. `weight = 2.0` or `5.0`) to produce scores of `1.5`, `2.0`, or higher, instantly preempting any standard action (which is capped at `1.0`).

---

## 2. Strict Constraints

1. **Rarity:** `isOverrideAction = true` is reserved strictly for life-safety emergency interrupts (such as fleeing toxic fire or acute trauma response).
2. **Discretionary Actions Are Never Overrides:** Routine maintenance, cooking, water purification, training, social chat, and research are NEVER marked `isOverrideAction = true`.
3. **Player Command Precedence:** Explicit DutyRoster assignments and direct player commands take precedence over autonomous scheduling unless an acute emergency override is active.
