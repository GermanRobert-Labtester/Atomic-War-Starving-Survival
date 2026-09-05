# Plan 95 — Journal Voice Producer Matrix

## 1. Producer Integration Classification

Every situation key in Plan 95 is classified according to how the current runtime interacts with it:
- **ACTIVE**: Systems currently have direct event hooks, feedback messages, or telemetry points that can trip this discovery.
- **MAPPABLE**: Core or host state directly tracks the underlying condition; mapping requires connecting an existing event or threshold check to `_journal.TryDiscover(...)` without writing new engine logic.
- **DEFERRED**: Conceptually planned for downstream expansions (e.g., Plan 65/66); catalog entry exists so that downstream systems can immediately call `TryDiscover` without schema modifications.

---

## 2. Situation Key Producer Table

| Key | Status | Producer Subsystem | Trigger Condition | Distinguishing Boundary |
|---|---|---|---|---|
| `low_food` | **MAPPABLE** | `NeedsSystem` / `HoldfastRuntimeSession` | Food stockpile falls below rationing threshold (`< 15%`) | Focuses on shelter food bins & portion shrinkage. |
| `low_water` | **MAPPABLE** | `NeedsSystem` / `WaterRecyclingSystem` | Clean water reserves drop below critical reserve (`< 10%`) | Focuses on thirst, measurement by cups, and physical survival limit. |
| `death_of_survivor` | **ACTIVE** | `SurvivorLifecycle` / `FinalWishSystem` (Plan 65) | Survivor health reaches 0 or mortal affliction concludes | Records loss of a specific shelter member, reassigning their duties. |
| `successful_expedition` | **ACTIVE** | `ExpeditionSystem` / `ExpeditionHostSession` | Expedition returns with cargo count > 0 and 0 casualties | Records net gain, temporary relief, and logistical expansion. |
| `failed_expedition` | **ACTIVE** | `ExpeditionSystem` / `ExpeditionHostSession` | Expedition returns with severe damage, empty cargo, or lost scouts | Records wasted supplies, tactical failure, and danger of leaving. |
| `faction_raid` | **MAPPABLE** | `ShelterDefenseSystem` / `RaidEvent` | Hostile faction attack event resolves against shelter | Records breach, structural damage, stolen supplies, and compromised safety. |
| `disease_outbreak` | **MAPPABLE** | `DiseaseSystem` / `MedicalHostSession` | 3+ survivors concurrently infected with communicable illness | Records contagion spreading through bunks, need for quarantine. |
| `power_failure` | **MAPPABLE** | `PowerGridSystem` / `ShelterEnvironment` | Generator fuel empty or breaker trip causing blackout | Records sudden machine silence, cold pumps, and battery prioritization. |
| `new_survivor_arrived` | **ACTIVE** | `SurvivorRoster` / `ShelterRecruitment` | New survivor admitted through airlock or quest recruitment | Records adding a new body, changing social dynamics and resource balances. |
| `severe_cold` | **MAPPABLE** | `WeatherSystem` / `AtmosphereSystem` | Weather season transitions to deep blizzard / sub-zero cold snap | Macro-weather environmental cold penetrating walls (distinct from localized room heater failure). |
| `high_radiation_zone` | **ACTIVE** | `ExpeditionSystem` / `RadiationSystem` | Expedition node rad level exceeds 50 mSv/hr | Traversing geographic fallout hotspots (distinct from personal dose tick). |
| `moral_compromise` | **MAPPABLE** | `GuiltSourceSystem` (Plan 66) / `MoralChoiceSystem` | Execution of taboo action, triage sacrifice, or harsh eviction | Records ethical injury and the psychological cost of hard survival choices. |

---

## 3. Boundary Disambiguation

### `severe_cold` vs `freezing_shelter`
- `freezing_shelter` (Baseline): Represents internal shelter failure where indoor living temperatures drop below zero due to faulty heating/ventilation systems.
- `severe_cold` (Plan 95): Represents external wasteland climate conditions — a deep planetary winter or supercell freeze that beats against the exterior walls and resists heating efforts.

### `high_radiation_zone` vs `has_seen_radiation`
- `has_seen_radiation` (Baseline Tutorial): First-time personal discovery that radiation is present in the world (first encounter with dosimeter ticking).
- `high_radiation_zone` (Plan 95): Geographic terrain classification for high-rad expedition sectors where passage demands heavy shielding or imposes severe time limits.

---

## 4. Host Invocation Pattern

To record a situation discovery in host systems without coupling or engine dependencies:

```csharp
// Pure engine-agnostic invocation via JournalSystem:
if (isFirstOccurrence)
{
    journal.TryDiscover(
        knowledgeKey: "low_food",
        author: primaryAuthor,
        day: simClock.CurrentDay,
        hour: simClock.CurrentHour);
}
```
If the discovery has already been recorded in `KnowledgeBase`, `TryDiscover` returns `null` and takes no action (idempotent deduplication).
