# Duty Season Modifier Semantics

> **Modifier Mechanics:** Mathematical behavior, clamping, consumer paths, and composition formulas for `encounterWeight` and `steamTripChanceBoost`.

---

## 1. `encounterWeight` Semantics

- **Consumer:** `ShelterEncounterSystem` (`Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs`).
- **Nature:** Multiplicative factor scaling shelter-internal encounter frequency.
- **Formula:**
  ```csharp
  _state.encounterWeightMultiplier = season.encounterWeight;
  ```
- **Clamp Behavior:** If `<= 0f`, defaults to `1.0f`. Clamped to reasonable bounds `[0.5, 2.5]`.
- **Application:** Evaluated when rolling nocturnal shelter visitor/incident queues. Does not directly spawn raids or incidents; it modulates the density and likelihood of internal survivor encounters.
- **Replacement Semantics:** When advancing from Season A to Season B, Season B's `encounterWeight` overwrites Season A's multiplier. It does NOT accumulate.

---

## 2. `steamTripChanceBoost` Semantics

- **Consumer:** Infrastructure / Steam Heating System (`BrineWaterSystem.cs`).
- **Nature:** Additive probability boost to steam-plant emergency trips during adverse seasonal conditions.
- **Formula:**
  ```csharp
  effectiveTripChance = Math.Clamp(baseTripChance + season.steamTripChanceBoost, 0f, 1f);
  ```
- **Range:** `0.0f` to `0.15f` (0% to +15% additional trip risk).
- **Physical Meaning:** In deep winter (`season_long_winter`) and sudden freeze/thaw (`season_second_winter`, `season_spring_thaw`), thermal shock and frozen condensation lines increase mechanical failure risks for the central heating boiler and brine filters.
- **Replacement Semantics:** Replaced cleanly upon season transition; never stacks across multiple days.
