# Weather Gate Interaction with Ledger Debt System (Task F19)

## 1. Overview & Core Mission
In ASHFALL, debt enforcement is grounded in physical geography. Creditors expect repayment at designated lockups or trading hubs. When severe weather blocks the only viable caravan route connecting the debtor to the creditor faction, the repayment countdown may pause under strictly bounded rules to prevent unfair defaults while resisting exploit loops.

## 2. Route-Specific Creditor Resolution Invariant
**Debt timing is strictly route-specific.**
There is no global pause: a storm in the far south never pauses a local garrison debt in the north.
The resolution chain is strictly:
```
Debt Instance -> Creditor Faction -> Repayment Route -> Weather Gate -> weather_delay_debt flag -> Blocked Status
```

## 3. Schema & Configuration
Weather gates supporting debt delays carry the `weather_delay_debt` boolean flag in `weather_route_gates.json`:
```json
{
  "id": "gate_seasonal_ice_road",
  "target": "route_08_the_high_voltage_grid_battery_relay",
  "weather_delay_debt": true
}
```
If `weather_delay_debt` is `false` or absent, the route does not qualify for weather-based debt delay, and terms continue counting down regardless of weather conditions.

## 4. Anti-Exploit Bounded Grace Budget
To prevent players from using persistent winter blizzards as an indefinite debt shield:
- **Cumulative Cap:** Each `DebtContract` has an anti-exploit cap of `MaxWeatherGraceDays = 3` days (`LedgerDebtSystem.MaxWeatherGraceDays`).
- **Tracking:** `DebtContract.weatherDelayDaysUsed` tracks consumed grace days.
- **Exhaustion Rule:** Once `weatherDelayDaysUsed >= MaxWeatherGraceDays`, the daily tick resumes decrementing `daysRemaining` even if the route remains impassable.
- **Resumption:** When the route clears before grace is exhausted, `daysRemaining` resumes countdown from its exact paused value without penalty.

## 5. Persistence Contract
- `DebtContract.weatherDelayDaysUsed` and `DebtContract.lastWeatherDelayGateId` are serialized within `LedgerDebtSystemState`.
- Save/load round-trips preserve the consumed grace days exactly, preventing exploit loops via quicksaving/reloading.

## 6. Canonical Implementation & Verification
- Debt system: `Assets/Ashfall.Core/LedgerDebtSystem.cs`
- Route access resolver: `Assets/Ashfall.Core/World/DebtRouteAccessResolver.cs`
- Route context resolver: `Assets/Ashfall.Core/World/RouteGateContextResolver.cs`
- Automated test suite: `Ashfall.Core.Tests/World/WeatherGateDebtInteractionTests.cs`
