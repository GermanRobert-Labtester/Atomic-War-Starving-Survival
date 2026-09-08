# Feedback Message Producer Matrix

## 1. Overview

This matrix maps authoritative ASHFALL domain outcomes to message keys in `feedback_messages.json`. A feedback key must **never** be triggered arbitrarily or used to infer gameplay logic. Every mapped key has an authoritative source system, a verified trigger condition, and exact typed arguments.

---

## 2. Wave A — Critical Survival Clarity (Thresholds & Transitions)

Warnings are emitted strictly on **state transition** (e.g., crossing from normal to critical, or detecting a new outbreak), never polled and repeated every tick without change.

| Key | Category | Severity | Domain Authority | Trigger / Transition | Arguments | Dedupe Key |
|---|---|---|---|---|---|---|
| `food_critical` | `resource_warning` | `critical` | `HoldfastRuntimeSession` / `NeedsSystem` | Hunger crosses >= 90% (Starvation threshold) | `{0}`: hunger percent | `survival_food_critical` |
| `food_low` | `resource_warning` | `warning` | `HoldfastRuntimeSession` / `NeedsSystem` | Hunger crosses >= 70% | `{0}`: hunger percent | `survival_food_low` |
| `dehydration_imminent` | `health_warning` | `critical` | `HoldfastRuntimeSession` / `NeedsSystem` | Thirst crosses >= 90% (Dehydration threshold) | `{0}`: survivor name | `survival_dehydration_imminent` |
| `water_low` | `resource_warning` | `warning` | `HoldfastRuntimeSession` / `NeedsSystem` | Thirst crosses >= 70% | `{0}`: thirst percent | `survival_water_low` |
| `power_critical` | `warning` | `warning` | `PowerGridSystem` | Grid enters brownout or blackout state | none | `power_grid_critical` |
| `high_radiation` | `warning` | `warning` | `HoldfastRuntimeSession` / `RadiationSystem` | Radiation dose rate crosses >= 50 mSv | none | `radiation_high_rate` |
| `injury_critical` | `health_warning` | `critical` | `HoldfastRuntimeSession` / `SurvivorsHostSession` | Health drops <= 25 HP | `{0}`: survivor name | `health_injury_critical` |
| `survivor_lost` | `failure` | `error` | `DiseaseHostSession` / `HoldfastRuntimeSession` | Survivor death event confirmed | `{0}`: survivor name | `survivor_death_{id}` |
| `disease_outbreak` | `warning` | `warning` | `DiseaseEngine` (`OnOutbreakDeclared`) | Outbreak declared for disease | none | `disease_outbreak_{name}` |
| `disease_alert` | `alert` | `critical` | `DiseaseEngine` (`OnInfection`) | New survivor infected | none | `disease_alert_{survivor}` |
| `storm_approaching` | `warning` | `warning` | `WeatherSystem` / `WeatherHostSession` | Storm forecast detected | `{0}`: hours remaining | `weather_storm_{id}` |

---

## 3. Wave B — Action Results (Committed Outcomes)

Emitted only **after** the transaction/action completes authoritatively with a success or failure result.

| Key | Category | Severity | Domain Authority | Trigger / Result | Arguments | Dedupe Key |
|---|---|---|---|---|---|---|
| `trade_success` | `success` | `success` | `HoldfastTradeSession` / `TradeSystem` | Trade committed (`TradeExecutionResult.Success`) | `{0}`: received, `{1}`: paid | `trade_exec_{timestamp}` |
| `trade_failed` | `failure` | `error` | `HoldfastTradeSession` / `TradeSystem` | Trade rejected (funds, stock, or terms) | `{0}`: partner name | `trade_fail_{timestamp}` |
| `medical_treatment_success` | `success` | `success` | `DiseaseHostSession` / `MedicalSystem` | Treatment succeeded (`OnTreatmentSuccess`) | `{0}`: patient name | `treatment_success_{id}` |
| `medical_failure` | `failure` | `error` | `DiseaseHostSession` / `MedicalSystem` | Treatment failed (`OnTreatmentFailed`) | `{0}`: patient name | `treatment_fail_{id}` |
| `expedition_success` | `success` | `success` | `ExpeditionSystem` / `ReconTelemetrySystem` | Expedition returned safely with loot | `{0}`: resources summary | `expedition_return_{id}` |
| `expedition_failed` | `failure` | `error` | `ExpeditionSystem` / `ReconTelemetrySystem` | Expedition lost or mission aborted | none | `expedition_fail_{id}` |
| `technology_unlocked` | `success` | `success` | `ResearchSystem` | Tech research completed (`OnTechCompleted`) | `{0}`: tech name | `tech_unlock_{id}` |
| `save_success` | `system` | `info` | `SaveLoadHostSession` / `CampaignSaveCoordinator` | Save write committed successfully | none | `save_success` |
| `save_failed` | `system` | `error` | `SaveLoadHostSession` / `CampaignSaveCoordinator` | Save write threw or failed verification | none | `save_failure` |
| `load_success` | `system` | `info` | `SaveLoadHostSession` | Save file loaded cleanly | none | `load_success` |
| `load_failed` | `system` | `error` | `SaveLoadHostSession` | Save envelope corrupted or invalid | none | `load_failure` |

---

## 4. Wave C — World, Production & Faction Progression

| Key | Category | Severity | Domain Authority | Trigger / Result | Arguments | Dedupe Key |
|---|---|---|---|---|---|---|
| `alliance_formed` | `success` | `success` | `FactionSystem` / `FactionActionBoard` | Faction standing reaches Allied status | `{0}`: faction name | `faction_alliance_{id}` |
| `alliance_broken` | `failure` | `error` | `FactionSystem` | Faction alliance severed | `{0}`: faction name | `faction_broken_{id}` |
| `reputation_gained` | `reward` | `success` | `FactionSystem` | Standing increased after quest or barter | `{0}`: delta, `{1}`: faction | `rep_gain_{id}` |
| `reputation_lost` | `penalty` | `warning` | `FactionSystem` | Standing decreased | `{0}`: delta, `{1}`: faction | `rep_loss_{id}` |
| `bunker_upgraded` | `success` | `success` | `SilentFoundryHostSession` / `Crafting` | Production cast completed | `{0}`: capacity/feature | `foundry_cast_{id}` |
| `bunker_damaged` | `failure` | `error` | `SilentFoundryHostSession` | Cast failed / incident | none | `foundry_fail_{id}` |
| `radiation_storm_passed` | `world_state` | `info` | `WeatherSystem` | Severe fallout/storm subsides | none | `weather_storm_passed` |

---

## 5. Confirmation Templates (Gated Actions)

Confirmation messages wrap commands that already exist in Core systems. The modal UI only gates invocation—it never alters domain eligibility.

| Key | Action Target | Target Command | Revalidation Guard |
|---|---|---|---|
| `delete_survivor` | Exile survivor | `SurvivorsHostSession.Exile(id)` | Survivor must still exist and be alive |
| `abandon_quest` | Abandon active quest | `HoldfastQuestSystem.Abandon(id)` | Quest must still be active |
| `use_medicine` | Administer rare cure | `MedicalSystem.ApplyTreatment(...)` | Medicine item and patient must still be present |

---

## 6. Deferred / Dormant Keys

Keys without an active domain producer in the current game slice remain defined in the catalog as dormant assets. They are not removed from the JSON catalog (preserving schema stability), but are documented as dormant until their systems are connected:
- `network_error`: single-player offline architecture; strictly dormant.
- `update_available`, `update_failed`: package management / engine updater; dormant.
- `bunker_closing`: automated vault door breach/seal; dormant.
- `train_ambushed`: railway expansion; dormant until rail schedule loop is integrated.
