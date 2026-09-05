# Weather Gate Context Resolution Architecture (Tasks F17–F20)

## 1. Executive Architecture Summary
The Weather Gate Cross-System Interaction Layer unifies four previously isolated systems into a single deterministic evaluation pipeline:
1. **Faction War (F17)**: Adds wartime tension, checkpoint/patrol encounter weighting, and hazard severity.
2. **Territory Control (F18)**: Governs shelter waypoint availability and infrastructure maintenance.
3. **Ledger Debt (F19)**: Resolves creditor repayment route blockage and applies anti-exploit grace periods.
4. **Seasonal Events (F20)**: Imposes compound hazard modifiers under active environmental crises.

```
                    ┌─────────────────────────┐
                    │ Current Weather State   │
                    └───────────┬─────────────┘
                                │
                                ▼
                    ┌─────────────────────────┐
                    │ Base Gate Passability   │  (Fail-Closed Authority)
                    └───────────┬─────────────┘
                                │
        ┌───────────────────────┼───────────────────────┐
        ▼                       ▼                       ▼
┌───────────────┐       ┌───────────────┐       ┌───────────────┐
│  Faction War  │       │   Territory   │       │   Seasonal    │
│   Modifier    │       │   Modifier    │       │   Modifier    │
└───────┬───────┘       └───────┬───────┘       └───────┬───────┘
        │                       │                       │
        └───────────────────────┼───────────────────────┘
                                │
                                ▼
                    ┌─────────────────────────┐
                    │ Precedence Severity     │  min(2.0, max(1.0, war, terr, seas))
                    │ Merge & Shelter Check   │
                    └───────────┬─────────────┘
                                │
                                ▼
                    ┌─────────────────────────┐
                    │ Debt Route Accessibility│  Route-specific pause predicate
                    └───────────┬─────────────┘
                                │
                                ▼
                    ┌─────────────────────────┐
                    │ Immutable Result & Trace│  Deterministic reason list & trace string
                    └─────────────────────────┘
```

## 2. Invariants & Rules
1. **Weather Passability Authority:**
   Weather conditions exclusively decide whether a gate is open, closed, or blocked. Contextual systems cannot close an open gate.
2. **Non-Multiplicative Harmful Modifiers:**
   Harmful severity multipliers never multiply across categories:
   `effectiveSeverity = min(2.0, max(1.0, war, territory, seasonal))`.
3. **Independent Encounter Axis:**
   Encounter weighting (`FactionEncounterWeightMultiplier`) and category tagging (`FactionEncounterTag`) exist on a separate output axis and do not affect physical consequence severity.
4. **Route-Specific Debt Evaluation:**
   Debts are checked only against the routes leading to their specific creditor faction.
5. **Anti-Exploit Grace Cap:**
   Weather delay is capped at `MaxWeatherGraceDays = 3` consecutive days.
6. **Pure Determinism:**
   Zero random rolls in the evaluation pipeline. Given identical snapshots, output trace and numbers are byte-identical.

## 3. Class Index
| Class / Struct | Role | Location |
|---|---|---|
| `WeatherGate` | Core domain entity with modifier schemas | `Assets/Ashfall.Core/World/WeatherGate.cs` |
| `WeatherGateEvaluationContext` | Pure input context containing system snapshots | `Assets/Ashfall.Core/World/WeatherGateEvaluationContext.cs` |
| `WeatherGateContextResult` | Enriched immutable evaluation result | `Assets/Ashfall.Core/World/WeatherGateContextModifier.cs` |
| `WeatherGateContextEvaluator` | Pure evaluation engine with modifier merging | `Assets/Ashfall.Core/World/WeatherGateContextEvaluator.cs` |
| `RouteGateContextResolver` | Authoritative corridor context resolver | `Assets/Ashfall.Core/World/RouteGateContextResolver.cs` |
| `DebtRouteAccessResolver` | Creditor route blockage evaluation logic | `Assets/Ashfall.Core/World/DebtRouteAccessResolver.cs` |
| `LedgerDebtSystem` | Debt state with grace cap and timer pause | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

## 4. Verification Suites
- `Ashfall.Core.Tests/World/WeatherGateWarInteractionTests.cs` (F17)
- `Ashfall.Core.Tests/World/WeatherGateTerritoryInteractionTests.cs` (F18)
- `Ashfall.Core.Tests/World/WeatherGateDebtInteractionTests.cs` (F19)
- `Ashfall.Core.Tests/World/WeatherGateSeasonalInteractionTests.cs` (F20)
- `Ashfall.Core.Tests/World/WeatherGateCrossSystemIntegrationTests.cs` (Scenarios A–H)
