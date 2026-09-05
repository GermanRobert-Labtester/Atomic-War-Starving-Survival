# Weather Gate Interaction with Territory Control (Task F18)

## 1. Overview & Core Mission
A route through well-maintained, garrisoned territory offers shelters, emergency cairns, and cleared road markers. Conversely, an unclaimed or lawless wasteland stretch multiplies the perils of winter blizzards and acidic biofogs. Task F18 integrates territory control states (`Controlled`, `Contested`, `Unclaimed`) into weather gate evaluation.

## 2. Passability Authority Invariant
**Territory state never flips an open weather gate to blocked.**
Clear weather remains passable regardless of whether territory is controlled, contested, or unclaimed. Instead, territory control governs:
- Availability of shelter waypoints (`ShelterAvailable`);
- Escalation or mitigation of physical forced traversal consequences (`ConsequenceSeverityMultiplier`).

## 3. Schema & Configuration
Territory modifiers are defined in `weather_route_gates.json` under `territory_modifier`:
```json
"territory_modifier": {
  "controlled": {
    "severity_multiplier": 0.75,
    "shelter_available": true
  },
  "contested": {
    "severity_multiplier": 1.0,
    "shelter_available": false
  },
  "unclaimed": {
    "severity_multiplier": 1.5,
    "shelter_available": false
  }
}
```

### Territory Control States:
| State | Severity Multiplier | Shelter Waypoint | Notes |
|---|---|---|---|
| `Controlled` | 0.75x | `true` | Maintained roads, functional storm shelters, marked cairns |
| `Contested` | 1.0x | `false` | Disputed zone; shelters stripped or booby-trapped; baseline severity |
| `Unclaimed` | 1.5x | `false` | Wild wasteland; destroyed roadbed; extreme exposure risks |

## 4. Evaluation Semantics
1. Evaluator inspects `TerritorySnapshot`:
   - `State`: Current control ladder tier (`Controlled`, `Contested`, `Unclaimed`).
   - `ControllerFactionId`: Faction holding sway over the corridor.
2. If `territory_modifier` is configured on the gate:
   - Matches snapshot state to corresponding definition (`controlled`, `contested`, `unclaimed`).
   - Sets `ShelterAvailable = stateMod.shelter_available`.
   - Passes `territorySeverity` into `WeatherGateContextEvaluator.MergeSeverity`.
   - Emits reason string: `territory_controlled`, `territory_contested`, or `territory_unclaimed`.
3. If no territory modifier is defined:
   - Defaults to `ShelterAvailable = false`, `severity = 1.0f`.

## 5. Live State Reflection & Cache Hygiene
`WeatherGateContextEvaluator` is a pure function. It does not cache prior evaluations. When territory flips from `Controlled` to `Contested` following a regional battle, the very next evaluation tick immediately updates `ShelterAvailable` and consequence scaling without requiring manual invalidation.

## 6. Canonical Implementation & Verification
- Domain model: `Assets/Ashfall.Core/World/WeatherGate.cs`
- Pure evaluator: `Assets/Ashfall.Core/World/WeatherGateContextEvaluator.cs`
- Automated test suite: `Ashfall.Core.Tests/World/WeatherGateTerritoryInteractionTests.cs`
