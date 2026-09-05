# Weather Gate Interaction with Seasonal Events (Task F20)

## 1. Overview & Core Mission
Severe seasonal phenomena (e.g. ash filter clogs, cold snaps, pipe bursts) compound the dangers of bad weather. When a survivor attempts to traverse a blocked gate during a compound seasonal hazard, physical consequences (stamina loss, frostbite, toxic inhalation) increase.

## 2. Passability Authority Invariant
**Seasonal events do not independently block open routes.**
Compound severity modifiers only apply when the underlying weather gate is **blocked** by weather. Under clear weather, a seasonal event may create shelter or survival challenges, but it does not close the route unless governed by a dedicated route-lock system.

## 3. Schema & Configuration
Compound event modifiers are defined in `weather_route_gates.json` under `compound_event_modifier`:
```json
"compound_event_modifier": {
  "event_season_cold_snap": 1.5,
  "event_season_freeze_pipe_burst": 1.25
}
```

### Key Semantics:
- Keys must match valid seasonal event IDs from `seasonal_events.json`.
- Values must be `>= 1.0`.

## 4. Evaluation Semantics & Precedence
1. **Highest-Only Rule:** If multiple active seasonal events match a gate's `compound_event_modifier` map, only the **highest** multiplier applies. Modifiers never compound multiplicatively with each other (e.g. `1.5x` and `1.25x` yields `1.5x`, not `1.875x`).
2. **Deterministic Selection:** Active event keys are evaluated in ordinal order; the highest value wins.
3. **Cross-System Merge Precedence (Rule 2.3):**
   ```csharp
   effectiveSeverity = min(2.0, max(1.0, warSeverity, territorySeverity, seasonalSeverity))
   ```
   A `1.5x` seasonal modifier and a `1.5x` wartime modifier do **not** multiply to `2.25x` or add to `3.0x`; the maximum harmful modifier is `1.5x`.
4. **Global Cap:** All consequence multipliers are strictly clamped at `2.0x`.

## 5. Canonical Implementation & Verification
- Domain model: `Assets/Ashfall.Core/World/WeatherGate.cs`
- Pure evaluator: `Assets/Ashfall.Core/World/WeatherGateContextEvaluator.cs`
- Automated test suite: `Ashfall.Core.Tests/World/WeatherGateSeasonalInteractionTests.cs`
