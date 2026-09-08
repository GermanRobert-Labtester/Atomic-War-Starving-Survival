# Utility Override Action Contract

Derived from `UtilityActionScorer.Score()` and `UtilityAiSystem.SelectAction()` in Core.

## Override Semantics

When `isOverrideAction = true`:
1. The action is scored normally through the pipeline
2. The final `clamp01` is **skipped** — the score can exceed 1.0
3. This allows override actions to dominate even when normal actions reach 1.0

## When Override Wins

Since override actions skip the upper clamp, an override with weight > 1 can produce scores > 1.0, beating any normal action clamped at 1.0.

Example:
- Normal action: baseScore 0.9, weight 1.0 → (0.9 + 0.1) × 1.0 = 1.0 (clamped)
- Override: baseScore 0.3, weight 4.0 → (0.3 + 0.1) × 4.0 = 1.6 (unclamped)
- Override wins: 1.6 > 1.0

## Override vs Override

Multiple override actions compete normally — highest score wins, with deterministic noise tie-breaking.

## Veto Precedence

Vetoes (hard, score → 0) apply **before** the override check. A coward vetoes `loud_labor` even if it's an override.

## Existing Override Actions

**None.** All 6 existing actions have `isOverrideAction: false`.

## Plan 72 Design Rules

1. Override is reserved for states where normal scheduling should stop
2. Appropriate candidates: flee danger, emergency medical response, fire response (if executor exists)
3. Inappropriate: maintenance, training, socializing, research, cooking, cleaning
4. Only mark override if the runtime executor supports emergency interruption

## Plan 72 Decision

Given the current Utility AI is companion-bias (not general shelter autonomy), and no executor supports emergency interruption via the Utility AI pipeline, **no new override actions are added in Plan 72**.

All 20 actions remain `isOverrideAction: false`.