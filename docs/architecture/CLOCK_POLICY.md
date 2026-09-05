# ASHFALL Clock Governance & Simulation Policy

**Status:** Canonical Architecture Policy
**Applies To:** `Assets/Ashfall.Core`, `src`, save stores, and test suites
**Authority Level:** Invariant 2 (Ports and Adapters) & Invariant 4 (Determinism)

---

## 1. Executive Summary

ASHFALL maintains two distinct simulation clock interfaces with explicit temporal resolutions:
1. **`IClock` (Day-Level):** Monotonic integer campaign calendar days.
2. **`ISimClock` (Sub-Day / Intraday):** High-resolution simulation ticks (60 ticks/hour, 1440 ticks/day) and intraday hours (0–23).

> **ARCHITECTURAL DIRECTIVE: DO NOT MERGE `IClock` AND `ISimClock`.**
> `IClock` and `ISimClock` serve intentionally decoupled simulation horizons. Day-level systems (survival decay, expedition journeys, season shifts, faction standing) must not take dependencies on tick/sub-day state. Sub-day systems (radio broadcasts, hourly machines, broadcast windows) depend on `ISimClock`. `SimClock` implements both interfaces as a composite adapter.

---

## 2. Canonical Contracts

### 2.1 `IClock` (`Assets/Ashfall.Core/Ports.cs`)

```csharp
public interface IClock
{
    int Day { get; }
    void AdvanceDays(int days);
    void SetDay(int day);
}
```
- **Primary Use:** Simulation calendar tracking whole integer days.
- **Constraints:**
  - Day must be monotonically non-decreasing during normal simulation.
  - Values must never derive from `System.DateTime`.
  - Day index starts at 1 for new campaigns (Day 0 reserved for pre-game setup).

### 2.2 `ISimClock` (`Assets/Ashfall.Core/Clock/ISimClock.cs`)

```csharp
public interface ISimClock
{
    long CurrentTick { get; }
    int DayIndex { get; }
    int HourOfDay { get; }
    void AdvanceTicks(long ticks);
    void AdvanceHours(int hours);
    void AdvanceDays(int days);
}
```
- **Canonical Arithmetic Constants:**
  - `TicksPerHour = 60`
  - `TicksPerDay = 1440` (`60 * 24`)
  - `DayIndex = CurrentTick / TicksPerDay`
  - `HourOfDay = (CurrentTick % TicksPerDay) / TicksPerHour`

### 2.3 `IWallClock` (`Assets/Ashfall.Core/IWallClock.cs`)

- Non-deterministic wall-clock interface for diagnostic logs, session banners, and temporary file paths.
- **Strict Boundary:** Wall-clock values must **never** influence gameplay mechanics, RNG seeds, save hashes, or campaign progression.

---

## 3. Double-Advance Protection & Cadence Governance

### 3.1 Daily Double-Advance Invariant
Any system executing state mutations once per simulation day must guard against duplicate calls for the same day index:
```csharp
if (_state.lastTickDay == day) return;
_state.lastTickDay = day;
```
This guarantees that UI re-renders, multiple tick calls, or host scene transitions cannot double-apply daily decay, tribute, or starvation.

### 3.2 Warlord Doctrine Cadence
- `WarlordDoctrineSystem.TickDaily(day, rng, context)` enforces `lastTickDay == day` idempotence.
- Action interval and tribute demands respect configured day gaps (`action_interval_days`, `tribute_interval_days`).

### 3.3 The Verdict Census Cadence
- `VerdictCensusBroadcast` operates on 99.0 MHz.
- **Window Condition:** strictly `_clock.DayIndex % 7 == 0 && _clock.HourOfDay == 3`.
- **Timing:** 4.0s carrier, 1.7s held-breath pause (canon; do not tune), followed by census count and footer.
- **Idempotency:** `_lastWindowDay == _clock.DayIndex` ensures the broadcast triggers at most once per 7-day window.

---

## 4. Consumer Classification

| System / Area | Primary Clock | Rationale |
|---|---|---|
| `NeedsSystem` | `IClock` / Daily tick | Caloric and hydration decay calculated daily |
| `RadiationSystem` | `IClock` / Daily tick | Environmental exposure accumulated per day |
| `WeatherSystem` | `IClock` / Daily tick | Season duration and weather pattern progression |
| `ExpeditionSystem` | `IClock` / Daily tick | Travel steps and encounter days |
| `WarlordDoctrineSystem` | `IClock` / Daily tick | Tribute cycles and tactical operations |
| `SimClock` | Composite (`ISimClock` + `IClock`) | Canonical host time adapter |
| `VerdictCensusBroadcast` | `ISimClock` | Intraday 03:00 broadcast window |
| `VerdictRadioSystem` | `ISimClock` | Real-time audio carrier sequencing |
| `GodotLog` / Diagnostics | `IWallClock` | Diagnostic log timestamps only |
| `SaveLoadHostSession` | `IWallClock` | Save slot manifest metadata (display only) |

---

## 5. Conversion Rules

1. **Host Boundary Only:** Conversion between simulation ticks and days must only occur through `ISimClock` or explicit arithmetic (`ticks / 1440`).
2. **Deterministic Replay:** Given the same initial tick and advance sequence, simulation clocks must yield bit-identical ticks and event timings across save/load cycles.
