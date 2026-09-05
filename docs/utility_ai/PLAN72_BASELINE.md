# Plan 72 — Utility AI Baseline Reconnaissance

> **Status:** Grounded baseline inspection completed 2026-09-03.
> **Authority:** `Assets/Ashfall.Core/UtilityAI/`, `Assets/StreamingAssets/Data/utility_actions.json`, `src/Host/UtilityAiHostSession.cs`, `src/UtilityAI/UtilityAiPanel.cs`.

---

## 1. Executive Summary

`Assets/StreamingAssets/Data/utility_actions.json` previously contained **6 actions**:
1. `action_weigh_goods` (Base 0.40, Priority 0.1, Weight 1.0, Gate 85, SkillBonus 0.25, `["loud_labor"]`)
2. `action_read_contract` (Base 0.35, Priority 0.1, Weight 1.0, Gate 90, SkillBonus 0.20, `[]`)
3. `action_canvas_support` (Base 0.45, Priority 0.1, Weight 1.0, Gate 80, SkillBonus 0.15, `["menial_labor"]`)
4. `action_run_vouch` (Base 0.30, Priority 0.1, Weight 1.0, Gate 88, SkillBonus 0.10, `[]`)
5. `action_audit_inventory` (Base 0.35, Priority 0.1, Weight 1.0, Gate 80, SkillBonus 0.00, `["quiet_labor"]`)
6. `action_file_report` (Base 0.35, Priority 0.1, Weight 1.0, Gate 80, SkillBonus 0.00, `["quiet_labor"]`)

These 6 actions represent administrative, depot calibration, and companion-bias tasks from the crossing companion systems. They completely lack basic shelter operational actions: maintenance, medical response, cooking, water purification, social interaction, skill training, security watch, scientific research, and fatigue rest.

---

## 2. Core System Architecture

1. **`UtilityActionDef` (`UtilityAction.cs`):** Defines catalog properties loaded from `utility_actions.json`:
   - `id`: unique action identifier (e.g. `action_*`).
   - `displayName`: human-readable UI label.
   - `description`: flavor and mechanic intent.
   - `basePriority`: additive baseline priority (default 0.1).
   - `weight`: multiplicative score multiplier (default 1.0).
   - `isOverrideAction`: bypasses normal clamping (allows scores > 1.0 to guarantee winning).
   - `tags`: string array checked against trait veto matrix (`loud_labor`, `menial_labor`, `weapon`, `gun`, `order`, `medical_triage`, `farming`, etc.).
   - `curvePoints`: response curve keys `[{x, y}, ...]` with linear interpolation and endpoint clamping.
   - `baseScore`: baseline raw score before curve transformation.
   - `fatigueGate`: threshold above which `rawScore` drops to 0.
   - `skillBonusFactor`: multiplier scaling `CraftingSkill` into bonus raw score.

2. **`AIActionContext` (`UtilityAction.cs`):** Per-evaluation snapshot:
   - `SurvivorId`, `IsAlive`, `Fatigue` (0..100), `CraftingSkill` (0..1), `IsListless`, `HasHazmat`, `Traits` (HashSet).

3. **`UtilityActionScorer` (`UtilityActionScorer.cs`):**
   - Evaluates hard trait vetoes (`IsForbiddenByTraits`).
   - Computes `rawScore = action.EvaluateRaw(context)` (gated by life and fatigue).
   - Transforms raw score via `action.Curve.Evaluate(rawScore)`.
   - Calculates `score = (curvedScore + action.basePriority) * action.weight`.
   - Subtracts `ListlessScorePenalty` (0.08) if `IsListless`.
   - Clamps to `[0, 1]` unless `isOverrideAction == true`.

4. **`UtilityAiSystem` (`UtilityAiSystem.cs`):**
   - Evaluates candidate actions using `scorer.Score(candidate, context)`.
   - Adds deterministic noise: `score += (float)(rng.NextDouble() * 0.0001d)`.
   - Selects action with `score > 0` and highest value. First-in-order wins exact ties.
