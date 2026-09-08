# Utility Scoring Contract

Derived from `UtilityActionScorer.Score()` in `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs`.

## Scoring Pipeline

```
1. IsForbiddenByTraits(action, context) → 0 if vetoed
2. EvaluateRaw(context) → rawScore
   - 0 if !IsAlive
   - 0 if fatigueGate > 0 && Fatigue > fatigueGate
   - baseScore + CraftingSkill * skillBonusFactor (clamped 0-1)
3. rawScore ≤ 0 → score = 0 (stop)
4. Curve.Evaluate(rawScore) → curvedScore
5. (curvedScore + basePriority) * weight → score
6. ApplyTraitBiases(score, action, context) → score
7. if IsListless: score -= 0.08
8. if isOverrideAction: return max(0, score)  // no upper clamp
9. return clamp01(score)
```

## Formula

```
score = clamp01(
    bias(
        (curve(baseScore + skill * skillBonus) + basePriority) * weight
    ) - listlessPenalty
)
```

Where:
- `curve(x)` = piecewise-linear interpolation of curvePoints
- `bias(s)` = trait soft multiplier (e.g., Politician × 0.6 on dirty_labor)
- `listlessPenalty` = 0.08 if IsListless, else 0
- Override actions skip the final `clamp01`

## Veto Matrix (Hard, score → 0)

| Trait | Tag | Condition |
|-------|-----|-----------|
| `coward` | `loud_labor` | Always |
| `god_complex` | `menial_labor` | Always |
| `pacifist` | `weapon` | Always |
| `blind` | `gun` | Always |
| `ex_con` | `order` | Always |
| `hitman` | `medical_triage` | Always |
| `hitman` | `farming` | Always |
| `germaphobe` | `medical_triage` | Without hazmat |

## Bias Matrix (Soft, multiplier)

| Trait | Tag | Multiplier |
|-------|-----|------------|
| `politician` | `dirty_labor` | 0.6x |

Bias multiplier clamped to [0.1, 2.0].

## Selection (UtilityAiSystem.SelectAction)

```
For each candidate:
  score = scorer.Score(candidate, context)
  if score > 0: score += rng.NextDouble() * 0.0001  // deterministic noise
  if score > 0 && score > bestScore: best = candidate
Return best (or null if all ≤ 0)
```

- Ties: first-wins over candidate list order
- Candidate list order IS the deterministic contract
- Noise scale: 0.0001 (Unity parity)
- Only positive scores compete