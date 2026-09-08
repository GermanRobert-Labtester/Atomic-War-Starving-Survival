# Plan 72 — Baseline Reconnaissance

## Core Utility AI (`Assets/Ashfall.Core/UtilityAI/`)

### Files
| File | Purpose |
|------|---------|
| `UtilityAction.cs` | Action DTO, `ResponseCurve`, `AIActionContext`, `UtilityTags` |
| `UtilityActionScorer.cs` | Scoring pipeline, veto matrix, trait biases |
| `UtilityAiSystem.cs` | Selection engine, catalog loader |
| `UtilityAiHeadlessDemo.cs` | Headless verification harness |

### Action Schema (`UtilityActionDef`)
| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `id` | `string` | `""` | Unique `action_*` identifier |
| `displayName` | `string` | `""` | Human-readable label |
| `description` | `string` | `""` | Flavor/behavior text |
| `basePriority` | `float` | `0.1` | Additive to curved score |
| `weight` | `float` | `1.0` | Multiplicative after curve+priority |
| `isOverrideAction` | `bool` | `false` | If true, skips final clamp01 |
| `tags` | `string[]` | `[]` | Tag array for veto/bias matrix |
| `curvePoints` | `CurvePoint[]` | required | Piecewise-linear response curve |
| `baseScore` | `float` | `0` | Static EvaluateRaw baseline |
| `fatigueGate` | `float` | `0` | 0=off; raw→0 when fatigue exceeds |
| `skillBonusFactor` | `float` | `0` | `CraftingSkill * factor` added to raw |

### Scoring Formula

```
rawScore = baseScore + CraftingSkill * skillBonusFactor   // clamped 0-1
         = 0 if !IsAlive or (fatigueGate > 0 && Fatigue > fatigueGate)

curvedScore = ResponseCurve.Evaluate(rawScore)

score = (curvedScore + basePriority) * weight

score = ApplyTraitBiases(score, action, context)  // soft multiplier

if IsListless: score -= 0.08

if isOverrideAction: return max(0, score)  // no upper clamp
else: return clamp01(score)
```

### Curve Points (`ResponseCurve`)
- `x` = rawScore input (0-1)
- `y` = curved output
- Piecewise-linear interpolation
- Points auto-sorted by x ascending
- Clamps to first/last point y at boundaries
- Empty/null → identity passthrough
- Single point → returns that point's y
- Duplicate x → second point wins (sort-stable)

### Known Tags (used in veto/bias matrix)
| Tag | Veto/Bias |
|-----|-----------|
| `loud_labor` | Coward vetoes |
| `menial_labor` | GodComplex vetoes |
| `dirty_labor` | Politician 0.6x bias |
| `weapon` | Pacifist vetoes |
| `gun` | Blind vetoes |
| `order` | ExCon vetoes |
| `medical_triage` | Hitman vetoes; Germaphobe vetoes without hazmat |
| `farming` | Hitman vetoes |
| `medical` | (informational only) |
| `quiet_labor` | (informational only) |

### Selection Engine (`UtilityAiSystem`)
- Scores all candidates, picks highest
- Deterministic seeded noise: `+ rng.NextDouble() * 0.0001`
- Ties: first-wins (candidate list order IS the contract)
- Only positive scores compete (score > 0)
- All-vetoed → returns null
- Stateless: no save state, no cooldown, no commitment

### AIActionContext
| Field | Type | Description |
|-------|------|-------------|
| `SurvivorId` | `string` | Survivor identifier |
| `IsAlive` | `bool` | Dead → score 0 |
| `Fatigue` | `float` | 0-100 scale |
| `CraftingSkill` | `float` | 0-1 scale |
| `IsListless` | `bool` | -0.08 penalty |
| `HasHazmat` | `bool` | Germaphobe gate |
| `Traits` | `HashSet<string>` | Trait flags for vetoes |

**No fields for:** hunger, thirst, health, morale, equipment degradation, room availability, inventory, threats.

## Host Utility AI (`src/UtilityAI/`, `src/Host/`)

### Files
| File | Purpose |
|------|---------|
| `src/Host/UtilityAiHostSession.cs` | Thin host session: loads catalog, demo evaluation |
| `src/UtilityAI/UtilityAiPanel.cs` | Godot UI panel for debug display |
| `src/Main.Survivors.cs` | Wires UtilityAiHostSession into Main |
| `src/Main.UiTests.UtilityAi.cs` | Headless UI smoke test |

### Host Session (`UtilityAiHostSession`)
- Loads catalog via `UtilityActionCatalogLoader.Load()`
- `Engine` (`UtilityAiSystem`) with `OnActionSelected` event → `LastEvent` + `RaiseStateChanged()`
- `EvaluateDemo()` creates context from parameters, selects action
- No save state, no cooldown, no reevaluation cadence

## Existing 6 Actions

| # | ID | baseScore | fatigueGate | skillBonus | Tags |
|---|----|-----------|-------------|------------|------|
| 1 | `action_weigh_goods` | 0.4 | 85 | 0.25 | `loud_labor` |
| 2 | `action_read_contract` | 0.35 | 90 | 0.2 | — |
| 3 | `action_canvas_support` | 0.45 | 80 | 0.15 | `menial_labor` |
| 4 | `action_run_vouch` | 0.3 | 88 | 0.1 | — |
| 5 | `action_audit_inventory` | 0.35 | 80 | 0.0 | `quiet_labor` |
| 6 | `action_file_report` | 0.35 | 80 | 0.0 | `quiet_labor` |

All 6 use identity curves: `[(0,0),(1,1)]`, basePriority 0.1, weight 1.0, no overrides.

These are companion-bias actions — narrative flavor for NPC companions.

## Key Architectural Limitations

1. **No state-driven scoring**: `EvaluateRaw` is `baseScore + skill * factor` only. No hunger, equipment degradation, threat level, or medical urgency inputs.
2. **No action executor**: Core selects an action ID; the host/executor must decide what to do with it.
3. **No commitment/cooldown**: Core is stateless. Reevaluation cadence and action duration are host responsibilities.
4. **No target selection in Core**: The AI picks an action, not a target. Target resolution is executor-owned.
5. **No room/workstation/recipe requirements in schema**: Eligibility checks must be in the executor.
6. **No save state**: Core has nothing to persist. Host may persist current action externally.

## Plan 72 Design Implications

Given the architecture, the 14 new actions:
- Use the same schema (baseScore, fatigueGate, skillBonusFactor, tags, curvePoints)
- baseScore establishes the priority hierarchy
- Tags wire into the existing veto/bias matrix
- fatigueGate prevents exhausted survivors from attempting work
- State sensitivity (checking if food is needed, equipment is degraded, etc.) belongs in the executor, not the action data
- Actions are `action_*` IDs that the executor resolves to subsystem calls