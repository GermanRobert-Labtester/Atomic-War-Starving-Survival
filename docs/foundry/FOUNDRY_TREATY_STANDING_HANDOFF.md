# Foundry Treaty Standing Handoff Contract

**Target System:** `Assets/Ashfall.Core/Legacy/FactionStanceEngine.cs` / `StandingRecordHostSession.cs`
**Host Dispatcher:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Standing Modification Mechanism

When a treaty consequence policy is applied:
1. `SilentFoundrySystem.ApplyConsequence(string treatyId, FoundryTreatyOutcome outcome, int day)` extracts `policy.standing_delta`.
2. Clamps the accumulated guild standing:
   ```csharp
   _consequenceState.guildStanding = MathfCompat.Clamp(
       _consequenceState.guildStanding + policy.standing_delta, StandingMin, StandingMax);
   ```
   Where `StandingMin = -100f` and `StandingMax = +100f`.
3. Dispatches the `OnConsequenceApplied` event carrying `FoundryConsequenceRecord`.
4. The host session mirrors the change directly into `FactionStanceEngine`:
   ```csharp
   GuildStanceEngine.ModifyTrust(record.factionId, record.standingDelta);
   ```

---

## 2. Standing Delta Distribution

Across the 15 policies:
- **Positive Deltas (Met):**
  - `+2.0`: `treaty_brine_pipe_and_iodine_exchange`, `treaty_cluster_labour_schedule`
  - `+3.0`: `treaty_road_iron_charter`, `treaty_flotilla_saline_corridor_concordat`, `treaty_deep_coast_aquifer_protection_treaty`, `treaty_scale_suburban_fair_trade_convention`
  - `+4.0`: `treaty_switchback_fuel_and_passage_accord`, `treaty_garrison_grain_tithe_compact`
- **Negative Deltas (Missed):**
  - `-5.0`: `treaty_flotilla_saline_corridor_concordat`
  - `-6.0`: `treaty_brine_pipe_and_iodine_exchange`, `treaty_road_iron_charter`
- **Negative Deltas (Violated):**
  - `-8.0`: `treaty_cluster_labour_schedule`
  - `-10.0`: `treaty_switchback_fuel_and_passage_accord`, `treaty_deep_coast_aquifer_protection_treaty`
  - `-12.0`: `treaty_garrison_grain_tithe_compact`

---

## 3. Stance Engine Thresholds

The resulting standing levels directly modulate trade stance according to canonical `FactionThresholds`:
- `ShareIntel` ($\ge 40.0$): High cooperation, shared wasteland maps.
- `Trade` ($\ge -20.0$): Trade stall open at base rates.
- `Rob` ($< -20.0$): Trade stall closed; extortion / refusal.
- `HostileRaid` ($\le -50.0$): Active armed confrontation.

A single missed treaty cycle ($-5.0$ or $-6.0$) creates tension without breaking the trade threshold ($-20.0$). A severe violation ($-10.0$ to $-12.0$) pushes an already-tenuous relationship directly into the `Rob` refusal tier.
