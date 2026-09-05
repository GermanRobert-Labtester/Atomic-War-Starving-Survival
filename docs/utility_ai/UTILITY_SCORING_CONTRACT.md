# Utility Scoring Contract

> **Mathematical Specification:** Exact scoring pipeline implemented in `Assets/Ashfall.Core/UtilityAI/UtilityActionScorer.cs`.

---

## 1. Step-by-Step Scoring Pipeline

Given an action definition `action` and decision context `context`:

1. **Null & Life Validation:**
   ```csharp
   if (action == null || context == null || !context.IsAlive) return 0f;
   ```

2. **Hard Trait Veto Check:**
   ```csharp
   if (UtilityActionScorer.IsForbiddenByTraits(action, context)) return 0f;
   ```

3. **Fatigue Gate Check:**
   ```csharp
   if (action.fatigueGate > 0f && context.Fatigue > action.fatigueGate) return 0f;
   ```

4. **Raw Baseline & Skill Contribution:**
   ```csharp
   float rawScore = action.baseScore;
   if (action.skillBonusFactor > 0f && context.CraftingSkill > 0f)
       rawScore += context.CraftingSkill * action.skillBonusFactor;
   rawScore = Math.Clamp(rawScore, 0f, 1f);
   if (rawScore <= 0f) return 0f;
   ```

5. **Response Curve Transformation:**
   ```csharp
   float curvedScore = action.Curve.Evaluate(rawScore);
   ```

6. **Base Priority & Weight Multiplier:**
   ```csharp
   float score = (curvedScore + action.basePriority) * action.weight;
   ```

7. **Listless Morale Penalty:**
   ```csharp
   if (context.IsListless)
       score -= UtilityActionScorer.ListlessScorePenalty; // 0.08f
   ```

8. **Override vs. Standard Clamping:**
   ```csharp
   if (action.isOverrideAction)
       return Math.Max(0f, score); // Unclamped > 1.0 allows override dominance

   return Math.Clamp(score, 0f, 1f);
   ```

9. **Deterministic Noise & Tie-Breaking (`UtilityAiSystem.SelectAction`):**
   ```csharp
   if (score > 0f && rng != null)
       score += (float)(rng.NextDouble() * 0.0001d);
   ```
   If two actions have the exact same score without RNG, the first action in candidate list order wins.
