# Weather Gate Interaction with Faction War State (Task F17)

## 1. Overview & Core Mission
In ASHFALL, wartime pressure transforms the strategic meaning of route choke points. While weather gates physically govern passability (a blizzard freezes high mountain cols; biofog chokes lowland marshes), wartime state elevates corridor tension, introduces armed checkpoints, and amplifies physical consequences without replacing the baseline weather rule.

## 2. Passability Authority Invariant
**Weather condition remains the sole passability authority.**
- If a route is closed due to a `Blizzard`, wartime state does not make it "more closed" or alter the weather status.
- If a route is open under `Clear` weather, wartime state does **not** flip the gate to blocked. Instead, it exposes the traveler to armed checkpoints, increased combat patrol encounter weighting, and detour suggestions.

## 3. Schema & Configuration
Wartime modifiers are defined in `weather_route_gates.json` under `war_state_modifier`:
```json
"war_state_modifier": {
  "enabled": true,
  "hostile_only": true,
  "min_tension": 50,
  "severity_multiplier": 1.5,
  "encounter_tag": "warlord_checkpoint",
  "encounter_weight_multiplier": 1.75,
  "force_detour": false
}
```

### Field Definitions:
- `enabled` (`bool`): Master toggle for the wartime modifier on this gate.
- `hostile_only` (`bool`): When true, the modifier only activates if the dominant faction controlling the corridor is hostile (`standing <= -50`).
- `min_tension` (`int`, 0..100): Minimum active war tension required for wartime encounter pressure to materialize.
- `severity_multiplier` (`float`): Multiplier applied to consequences (e.g. forced traversal stamina/rad damage). Capped at 2.0x globally.
- `encounter_tag` (`string`): Encounter category injected into the route's encounter deck (e.g. `warlord_checkpoint`, `warlord_patrol`).
- `encounter_weight_multiplier` (`float`): Weight multiplier applied to matching faction encounters along this corridor.
- `force_detour` (`bool`): When true, flags that caravans or couriers should prefer alternate routes.

## 4. Evaluation Semantics
1. Evaluator inspects `FactionWarSnapshot`:
   - `IsAtWar`: Is an active conflict ongoing?
   - `ActiveWarTension`: Does current tension meet or exceed `min_tension`?
   - `DominantFactionId`: Who holds military dominance over the corridor?
   - `IsDominantFactionHostile`: Is standing below the hostility threshold?
2. If wartime criteria are met:
   - `FactionEncounterTag` is set to `war_state_modifier.encounter_tag`.
   - `FactionEncounterWeightMultiplier` is set to `war_state_modifier.encounter_weight_multiplier`.
   - `warSeverityMultiplier` participates in the unified consequence severity merge (`MergeSeverity`).
   - Reason `war_hostile_tension_{tension}` is appended to `AppliedContextReasons`.
3. If peacetime or criteria not met:
   - Base weather passability and default encounter weights (1.0x) apply.

## 5. Canonical Implementation & Verification
- Domain model: `Assets/Ashfall.Core/World/WeatherGate.cs`
- Pure evaluator: `Assets/Ashfall.Core/World/WeatherGateContextEvaluator.cs`
- Automated test suite: `Ashfall.Core.Tests/World/WeatherGateWarInteractionTests.cs`
