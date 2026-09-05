# Power Schedule Compatibility

> **Schedule Integration:** Analysis of Plan 70 schedules and dynamic electrical load modulation.

---

## 1. Current State: Static Base Draw

- In the current live repository, `PowerGridRoom` specifies a static `DrawWatts` in `power_grid.json`.
- `ComputeTotalDraw()` calculates total active demand as the sum of all untripped, closed-breaker rooms whose priority is not `Disabled`.
- There is currently no time-of-day dynamic draw or cron-based demand modifier in Core `PowerGridSystem`.

---

## 2. Plan 70 Compatibility Contract

- Plan 71 **strictly maintains** the static draw model as designed, adhering to non-negotiable constraint §1.10.
- No parallel real-time scheduler or time-of-day simulation is introduced in Core.
- All simulation timing uses campaign days and ticks (`SimDay`), with zero dependency on wall-clock time (`DateTime.Now` or `DateTime.UtcNow`).

---

## 3. Future Extension Seams (Explicitly Deferred to Plan 70)

When Plan 70 schedule authority is fully integrated, schedule-dependent draw can be modeled through:
1. `IWorkShiftDemandModifier`: Dynamic multiplier adjusting `DrawWatts` during scheduled night rest (e.g. lowering workshop/kitchen draw while slightly raising bunk lighting).
2. `IEmergencyLockdownModifier`: Emergency siren mode prioritizing life-support circuits and forcing non-essential breakers open.
3. These remain follow-on opportunities and are deferred until schedule systems are authoritative.
