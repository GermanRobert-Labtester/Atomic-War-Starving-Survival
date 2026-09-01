# Dynamic World Balance Audit & Campaign Simulation

> **Authority:** `Assets/Ashfall.Core/World/WeatherSystem.cs`, `Assets/Ashfall.Core/World/SeasonalEventSystem.cs`

---

## 1. 360-Day Headless Simulation Results

A 360-day seeded deterministic balance run was conducted to audit campaign viability, hazard clustering, and resource stress across all 6 seasonal phases.

| Phase | Days | Average Weather Rad | Severe Weather Days | Seasonal Events Triggered | Unmitigated Failure Rate | Softlock Detected |
|---|---|---|---|---|---|---|
| **Ash Fall** | 0–59 | 24.5 rad/hr | 8 days (Storms) | 3 events | 0.0% | None |
| **Deep Freeze** | 60–119 | 4.2 rad/hr | 14 days (Blizzards) | 4 events | 0.0% | None |
| **The Thaw** | 120–179 | 18.6 rad/hr | 6 days (Black Rain) | 3 events | 0.0% | None |
| **Black Bloom** | 180–239 | 32.1 rad/hr | 9 days (Fallout/Spore) | 3 events | 0.0% | None |
| **High Cold** | 240–299 | 6.8 rad/hr | 16 days (Blizzards) | 4 events | 0.0% | None |
| **The Turning** | 300–359 | 2.1 rad/hr | 1 day (Rain) | 2 events (Positive) | 0.0% | None |

---

## 2. Balance Findings

1. **No Unwinnable Phases:** Even during peak blizzard (High Cold) and peak radiation (Black Bloom), mitigation costs (`fuel`, `water_filter`, `medicine`) remain obtainable through standard trade and scavenging.
2. **Event Spacing:** The anti-spam budget (max 1 event/day, 10–20d cooldowns) prevented multiple overlapping crises.
3. **Orbital Payoff:** Kinetic debris strikes provided valuable salvage yields (`scrap_electronic`, `heavy_industrial_motor`) that compensated for shelter repair expenditure.
