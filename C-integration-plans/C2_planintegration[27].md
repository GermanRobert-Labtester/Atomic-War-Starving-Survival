# C2 — Flagship Integration Plan [27]: Weather as a Deep Gameplay Driver, Forecast-to-Decision Pressure, and Cross-System Cascade Integrity

> **Deliverable:** `C2_planintegration[27].md`
> **Source scope:** Plan 135 — *Weather → Deep Gameplay Cascade*
> **Primary objective:** transform weather from a passive forecast/UI value into a deterministic campaign driver whose state projects into shelter integrity, thermal load, ventilation, flooding, expeditions, factions, economy, mental health, quest availability, location access, and long-term campaign memory—without creating parallel state authorities inside a new weather subsystem.
> **Required execution order:** **135A Foundation/System Contract → 135B Cascade Content & Player Decisions → 135C Cross-System Integration, Recovery, Balance, and Validation**
> **Hard dependencies:** canonical weather/forecast system; Plan 20 weather/exposure work; Plan 23 shelter power; Plan 32 world topology/travel; Plan 30 faction autonomy; Plan 31 semantic events; Plan 34 completion/epilogue record; Plan 39 save durability; Plan 46 balance evidence; Plan 52 ambience/weather presentation.
> **Scope discipline:** no duplicated shelter temperature, route-access, faction-standing, price, radiation, mental-health, or quest state inside `WeatherCascadeSystem`; no “weather effect” that only exists as prose; no unbounded weather history; no one-frame polling; no hidden unavoidable catastrophe without forecast/clue opportunity where the design promises preparedness; and no weather-specific subsystem bypassing canonical effect owners.

---

# 0. Executive Intent

ASHFALL already knows what the weather is.

What it does not yet consistently do is make the rest of the game care.

The current conceptual shape is too shallow:

```text
WeatherSystem
→ current weather kind / severity
→ forecast UI
```

The intended shape is:

```text
canonical weather state
        │
        ▼
WeatherCascadeSystem
        │
        ├─ shelter pressure
        ├─ expedition/travel modifiers
        ├─ faction-operation modifiers
        ├─ economy/scarcity modifiers
        ├─ mental-health pressure
        ├─ location accessibility
        └─ weather-triggered opportunities
        │
        ▼
existing authoritative systems
        │
        ▼
player-visible choices and consequences
```

The new system is not a second simulation.

It is a **cross-system weather projection layer**.

The strongest product outcome is:

> **A forecast matters because the player can prepare before the storm, decide whether to travel anyway, stockpile before scarcity, reinforce weak shelter sections, or exploit unusual faction/weather conditions—and those choices resolve through the same systems that already own shelter, travel, prices, health, relations, and world state.**

---

# 1. Source Diagnosis

The source establishes:

- `WeatherSystem` and `WeatherStationSystem` already exist,
- deterministic seeded weather work already exists,
- the forecast layer has accuracy/horizon,
- weather kinds are authored,
- only a small seasonal-data footprint exists,
- weather is not deeply connected to shelter/faction/economy/mental-health/location systems,
- the plan proposes a `WeatherCascadeSystem`,
- source weather effects cover shelter, expeditions, factions, economy, mental health, and locations,
- player preparation is explicitly part of the design,
- old-save compatibility and deterministic replay are required,
- a weather-alert UI and weather journal are required,
- 15 authored weather-effect templates are expected.

The critical architectural reading is:

```text
weather owns weather
cascade owns projection
downstream systems own consequences
```

---

# 2. Program-Level Success Criteria

C2[27] closes only when:

1. Weather cascade state is deterministic and persistent where persistence is necessary.
2. Every effect is data-authored or derived from canonical config.
3. Shelter weather effects route through existing thermal/damage/flooding/ventilation systems.
4. Expedition weather effects route through existing travel/route/risk systems.
5. Faction weather effects route through existing faction autonomy/standing/operation systems.
6. Economy weather effects route through canonical trade/price/scarcity systems.
7. Mental-health effects route through canonical crisis/morale/needs systems.
8. Location effects route through canonical topology/accessibility/discovery/contamination state.
9. Forecast accuracy materially changes player preparation quality.
10. The same weather event never applies the same consequence twice after save/load.
11. Weather effects have explicit start, active, recovery, and end semantics.
12. Severe weather can be mitigated by preparation where design says it should.
13. Harsh-weather risk is meaningful without becoming constant unavoidable punishment.
14. All effect targets resolve in data integrity checks.
15. Old saves initialize a valid empty/current cascade state.
16. Headless progression works without opening weather UI.
17. Weather UI shows predictions, current effects, preparation, and uncertainty without exposing hidden RNG.
18. Weather history is retained under explicit retention policy.
19. 15 effect templates are authored and runtime-observed.
20. A long seeded soak proves cascade stability, determinism, and balanced frequency.

---

# 3. Architectural Invariants

## 3.1 Weather remains the source of truth

`WeatherCascadeSystem` may never invent current weather.

## 3.2 Forecast and actual weather are distinct

The player may know a forecast estimate while the simulation knows exact current/future weather.

## 3.3 Cascade state is projection, not duplication

Examples:

```text
temperature impact → ShelterThermalSystem
flood impact → SumpFloodingSystem
radiation stress → Ventilation/Radiation
route closure → travel/topology
price change → economy/trade
```

## 3.4 Every effect has a canonical owner

No arbitrary `WeatherEffectState` field replacing real system state.

## 3.5 All random decisions are seeded

`ISeededRng` only.

## 3.6 Effects are idempotent

Save/load and repeated ticks cannot double-apply damage/price/standing consequences.

## 3.7 Effects have recovery semantics

Temporary closures/price shocks/stressors must end or decay.

## 3.8 Player information is honest but incomplete

Forecast confidence/accuracy can be imperfect.
UI never reveals future exact outcomes unless design explicitly grants certainty.

## 3.9 Severe weather has an attention budget

Do not fire six simultaneous full-strength effects with no prioritization.

## 3.10 Weather history is bounded

Use Plan 55 retention discipline.

---

# 4. Dependency Graph

```text
WeatherSystem / WeatherStation
           │
           ▼
      135A Cascade Core
           │
     ┌─────┼─────┬────────┬────────┬─────────┐
     ▼     ▼     ▼        ▼        ▼         ▼
 shelter travel factions economy mental   locations
     │     │     │        │        │         │
     └─────┴─────┴────────┴────────┴─────────┘
                         │
                         ▼
                    135B Content
                         │
                         ▼
                    135C Closure
```

Cross-plan:

```text
20 weather/exposure ─────► weather/rad effects
23 power ────────────────► heating/ventilation capacity
30 factions ─────────────► operation cadence
31 events ───────────────► semantic attribution
32 topology ─────────────► route/location access
34 completion record ────► extreme-weather legacy
39 save durability ──────► persistence/idempotency
46 balance ──────────────► harsh-weather tuning
52 ambience ─────────────► presentation only
```

---

# 5. Baseline Capture

Before implementation, record:

- all current weather kinds,
- weather-station forecast fields,
- weather RNG ownership,
- current shelter thermal inputs,
- sump flooding triggers,
- ventilation/radiation inputs,
- route-blocking APIs,
- expedition risk APIs,
- faction simulation cadence,
- economy price/scarcity APIs,
- mental-health crisis inputs,
- location accessibility authority,
- quest trigger APIs,
- save-section registration,
- current weather UI routes.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture one deterministic weather-seed baseline.

---

# 6. Workstream 135A — Foundation / System Contract

## Goal

Create one deterministic weather-cascade coordinator that turns canonical weather events into typed effect intents routed to existing systems.

---

# 7. 135A Phase A — Define Weather Event vs Weather Effect

Split runtime and authored data.

Recommended types:

```text
WeatherEventDefinition
WeatherEventInstance
WeatherEffectDefinition
WeatherCascadeState
WeatherEffectApplication
WeatherRecoveryState
```

Do not pack everything into one DTO.

---

# 8. 135A Phase B — `WeatherEventInstance`

Runtime fields:

```text
event_id
weather_kind
severity
start_day
end_day
affected_regions
seed/stream identity
phase
applied_effect_ids
recovery_effect_ids
```

---

# 9. 135A Phase C — Effect Target Vocabulary

Typed enum:

```text
Shelter
Expedition
Faction
Economy
MentalHealth
Location
Quest
```

Potential future targets require explicit schema/version update.

---

# 10. 135A Phase D — Effect Type Vocabulary

Typed categories:

```text
Damage
Delay
PriceModifier
BehaviorModifier
Accessibility
Exposure
Morale
Risk
Availability
Discovery
Recovery
```

Avoid arbitrary string effects.

---

# 11. 135A Phase E — Definition vs Application

Definition:

```text
what could happen
```

Application:

```text
this exact event applied this exact effect once
```

This separation is necessary for idempotency.

---

# 12. 135A Phase F — `WeatherCascadeSystem`

Create:

```text
Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs
```

Responsibilities:

- receive canonical weather transitions,
- resolve applicable effect definitions,
- produce deterministic effect applications,
- track active/recovery phases,
- prevent duplicate applications,
- expose read model,
- capture/restore minimal state.

It does not mutate downstream systems directly if port abstractions already exist.

---

# 13. 135A Phase G — `IWeatherCascadeSource`

Weather system publishes:

```text
WeatherStarted
WeatherChanged
WeatherEnded
ForecastUpdated
```

or canonical equivalent.

Cascade subscribes.

No per-frame weather polling.

---

# 14. 135A Phase H — Region Targeting

Every event has affected regions.

Resolution rules:

```text
global
region
location-specific
route-specific
```

Use canonical topology IDs.

---

# 15. 135A Phase I — Severity Bands

Keep raw severity if needed for calculation.

Player/system-facing band:

```text
Mild
Moderate
Severe
Extreme
```

Thresholds data-authored.

---

# 16. 135A Phase J — Weather Effect Rules Data

Create:

```text
Assets/StreamingAssets/Data/weather_effects.json
```

Each row:

```text
id
weather_kind / weather_family
severity_min/max
target
effect_type
magnitude/range
duration
region_scope
mitigation_tags
recovery_policy
localization keys
```

---

# 17. 135A Phase K — Effect Resolver

Input:

```text
event + region + current authoritative state
```

Output:

```text
eligible effect applications
```

Sorted deterministically by effect ID.

---

# 18. 135A Phase L — Seed Discipline

Use dedicated RNG stream.

Stable inputs:

```text
campaign seed
weather event id
effect definition id
target id
```

Do not perturb unrelated simulation RNG.

---

# 19. 135A Phase M — Idempotency Keys

Each application key:

```text
weather_event_id
effect_definition_id
target_id
phase
```

Persist applied keys where needed.

---

# 20. 135A Phase N — Event Phases

Recommended:

```text
Forecast
Approaching
Active
Recovering
Ended
```

Not every weather kind needs all phases.

---

# 21. 135A Phase O — Recovery Model

Temporary effects must declare:

```text
instant_revert
linear_decay
timed_unlock
requires_repair
persistent_world_change
```

This prevents forgotten route closures or price spikes.

---

# 22. 135A Phase P — Save State

Persist only dynamic cascade state:

- active event identities,
- phase/timing,
- applied-effect keys,
- recovery state,
- persistent extreme-event legacy references.

Do not persist static template definitions.

---

# 23. 135A Phase Q — Old Save Compatibility

Missing cascade section:

```text
valid
→ initialize empty/current
```

Do not retroactively apply historical weather effects from days before load.

---

# 24. 135A Phase R — Legacy Grace Policy

If a legacy save loads during severe current weather, decide:

- apply current active weather effects immediately,
- or allow one controlled grace tick.

Document/test.

Avoid surprise damage due solely to adding the feature.

---

# 25. 135A Phase S — Shelter Port Contract

Declare sinks:

```text
thermal pressure
structural damage
flooding input
filtration/radiation stress
```

Each sink binds to real authority.

---

# 26. 135A Phase T — Expedition Port Contract

Declare:

```text
route accessibility
travel risk
stamina modifier
vehicle breakdown modifier
```

Use existing expedition/travel systems.

---

# 27. 135A Phase U — Faction Port Contract

Declare weather-facing faction inputs:

```text
operation tempo modifier
patrol frequency modifier
raid cover modifier
relief opportunity
```

No direct faction-state duplicate.

---

# 28. 135A Phase V — Economy Port Contract

Declare:

```text
supply modifier
price modifier
availability modifier
```

Economy authority owns final prices.

---

# 29. 135A Phase W — Mental-Health Port Contract

Declare:

```text
weather stress
darkness/cold burden
shelter-damage morale
positive-weather morale
```

Mental-health/morale systems own outcomes.

---

# 30. 135A Phase X — Location Port Contract

Declare:

```text
temporary inaccessible
contamination pressure
infrastructure damage
discovery opportunity
```

Topology/location state owns result.

---

# 31. 135A Phase Y — Semantic Events

Use Plan 31 events for:

- severe weather approaching,
- weather impact applied,
- route closed,
- shelter damaged,
- relief opportunity,
- weather recovery,
- extreme event remembered.

No raw prose in events.

---

# 32. 135A Phase Z — Diagnostics

Developer report:

```text
WEATHER_EVENTS_ACTIVE
WEATHER_EFFECTS_ELIGIBLE
WEATHER_EFFECTS_APPLIED
WEATHER_EFFECTS_RECOVERING
WEATHER_REQUIRED_PORTS_MISSING
```

---

# 33. 135A Tests

- event parsing,
- severity bands,
- deterministic effect resolution,
- region targeting,
- idempotency,
- recovery,
- old-save initialization,
- port binding,
- headless ticking,
- no duplicate application after reload.

---

# 34. 135A Definition of Done

- [ ] WeatherCascadeSystem,
- [ ] definition/instance split,
- [ ] typed targets/effects,
- [ ] weather_effects.json schema,
- [ ] deterministic resolver,
- [ ] event phases,
- [ ] recovery policy,
- [ ] save state/versioning,
- [ ] old-save path,
- [ ] shelter ports,
- [ ] expedition ports,
- [ ] faction ports,
- [ ] economy ports,
- [ ] mental-health ports,
- [ ] location ports,
- [ ] semantic events,
- [ ] diagnostics,
- [ ] idempotency tests.

---

# 35. Workstream 135B — Cascade Content & Player Decisions

## Goal

Implement 15 deep weather-effect templates that create preparation, timing, mitigation, and exploitation decisions rather than passive penalties.

---

# 36. 135B Phase A — 15-Template Content Budget

Start with exactly:

```text
15 weather effect templates
```

balanced across:

- shelter,
- travel,
- faction,
- economy,
- mental health,
- location.

No uncontrolled expansion before utilization proof.

---

# 37. 135B Phase B — Shelter Storm Damage

Condition:

```text
severe storm
+ weak fortification
```

Effect:

```text
structural damage risk
```

Mitigation:

```text
reinforcement
maintenance readiness
power/repair resources
```

Use canonical shelter integrity.

---

# 38. 135B Phase C — Pre-Storm Reinforcement

Expose preparation action before impact.

Cost:

- resources,
- labor,
- time.

Outcome:

- reduced/zero damage probability.

Forecast quality determines lead time/uncertainty.

---

# 39. 135B Phase D — Extreme Cold

Effect:

```text
higher heat demand
```

Thermal/power/fuel systems calculate actual burden.

Do not directly subtract fuel in weather code.

---

# 40. 135B Phase E — Flooding

Heavy precipitation:

```text
→ sump load/input
```

Sump system decides:

- pumped successfully,
- overflow,
- water damage,
- contamination.

---

# 41. 135B Phase F — Radiation Storm

Weather projects:

```text
external radiation pressure
+ filtration stress
```

Radiation/ventilation systems own:

- indoor dose,
- filter effectiveness,
- survivor exposure.

---

# 42. 135B Phase G — Expedition Blocking

Severe conditions may:

```text
close route
or
mark high-risk
```

Do not conflate closure with warning.

---

# 43. 135B Phase H — Attempt-Anyway Option

Where fiction/system permits:

```text
wait
or
attempt
```

Attempt uses actual expedition risk/stamina/vehicle systems.

UI shows risk estimate derived from same runtime calculation.

---

# 44. 135B Phase I — Sudden Weather Change

Expedition caught mid-route:

```text
return
push through
shelter in place
reroute
```

only where canonical travel state supports it.

Do not fake mid-route branch if expedition runtime cannot pause/reroute.

---

# 45. 135B Phase J — Vehicle Risk

Weather modifies vehicle breakdown probability through `ExpeditionVehicleSystem`.

No weather-owned vehicle durability.

---

# 46. 135B Phase K — Faction Operation Pause

Military/structured factions may reduce operation tempo in severe weather.

Use faction doctrine/config, not hardcoded faction names where possible.

---

# 47. 135B Phase L — Storm-Cover Aggression

Rebel/irregular factions may gain cover.

This modifies existing raid/encounter opportunity.

Do not guarantee a raid merely because weather is severe.

---

# 48. 135B Phase M — Relief Opportunity

Independent/humanitarian factions may offer:

- shelter,
- trade,
- convoy,
- aid.

Use quest/trade/faction systems.

---

# 49. 135B Phase N — Patrol Reduction

Harsh weather may reduce patrol frequency.

Travel system consumes the modified faction patrol state.

Do not duplicate route-safety calculation.

---

# 50. 135B Phase O — Economy Scarcity

Weather produces a supply shock descriptor.

Economy maps to price/availability.

Avoid hardcoding “+50%” directly in C#.

Initial source values can seed data defaults:

```text
fuel +50
clothing +30
food +40
medicine +20
anti-rad +100
battery +50
```

but should live in data and be balance-tested.

---

# 51. 135B Phase P — Forecast Stockpiling

Player can prepare before price shock.

This is emergent from:

```text
forecast
+ inventory
+ trade
```

No special “stockpile mode” required.

---

# 52. 135B Phase Q — Seasonal Affective Pressure

Extended cold/darkness produces stress input to mental-health system.

Do not add an unsupported medical diagnosis if existing system lacks that diagnosis vocabulary.

If the source term is not modeled canonically, map to:

```text
weather_stress / low_light burden
```

and let diagnosis/treatment plans own medical naming.

---

# 53. 135B Phase R — Storm Stress

Stressors:

- prolonged severe weather,
- damage events,
- confinement.

MentalHealthCrisisSystem owns crisis probability/effects.

---

# 54. 135B Phase S — Positive Weather

Rare good weather may provide morale/restoration bonus.

Use same state-backed morale channel.

Avoid making “beautiful weather” a free global happiness pulse every clear day.

---

# 55. 135B Phase T — Cabin Fever

Extended confinement can increase ideological friction.

Use duration + occupancy/context.

Do not apply each day unboundedly.

---

# 56. 135B Phase U — Location Flooding

Temporary inaccessible flag/state with recovery date.

Topology owns accessibility.

---

# 57. 135B Phase V — Outdoor Contamination

Radiation weather increases location exposure/contamination through canonical radiation/environment state.

---

# 58. 135B Phase W — Infrastructure Damage

Weather can reduce location loot/infrastructure quality.

Apply through canonical location state/damage/depletion model.

No weather-specific loot scalar stored separately.

---

# 59. 135B Phase X — Erosion Cache Reveal

If hidden cache data exists:

```text
weather effect
→ reveal/discover location/object
```

Do not generate arbitrary free loot.

---

# 60. 135B Phase Y — Weather-Gated Quests

Initial source concepts:

```text
survive_the_storm
storm_chaser
weather_prophet
relief_convoy
```

Use canonical quest runtime.

---

# 61. 135B Phase Z — Quest Availability Windows

Weather quest declares:

```text
weather conditions
region
start/expiry
prerequisites
```

Save/load preserves window.

No farming by reloading.

---

# 62. 135B Phase AA — Forecast Improvement

Source proposes a weather-prediction mini-game.

Treat carefully.

If existing WeatherStation upgrade/knowledge system supports it:

```text
forecast skill / station upgrade
→ improved accuracy/horizon
```

If not, defer bespoke mini-game rather than creating a disconnected mechanic.

---

# 63. 135B Phase AB — Weather Shelter Events

Examples:

- bonding during storm,
- cabin fever,
- hidden shelter section revealed by damage.

Route through existing relations/discovery/event systems.

No parallel social event engine.

---

# 64. 135B Phase AC — Weather Alert UI

Use existing panel/briefing architecture.

Show:

```text
current event
forecast confidence
severity band
affected regions
predicted effect categories
preparation actions
active mitigation
recovery status
```

No hidden exact RNG.

---

# 65. 135B Phase AD — Preparation Recommendations

UI can explain:

```text
weak shelter sections
low fuel
route closure risk
filter condition
```

using canonical read models.

Warn, do not automatically act.

---

# 66. 135B Phase AE — Weather Journal

Significant events only.

Record:

- event,
- duration,
- affected region,
- major consequence,
- preparation/result.

Use journal authority.

---

# 67. 135B Phase AF — Weather Legacy

Extreme weather events can write landmark completion-record facts.

No full history copied to epilogue state.

---

# 68. 135B Phase AG — Recovery Content

After event:

- routes reopen,
- price shock decays,
- stress decays,
- infrastructure may require repair,
- contamination may persist if actual environment system says so.

Not every effect auto-reverts.

---

# 69. 135B Phase AH — Template Coverage Matrix

Generate:

| Effect template | Weather | Severity | Target | Mitigation | Recovery | Runtime observed |
|---|---|---:|---|---|---|---:|

---

# 70. 135B Phase AI — Utilization Soak

Seeded 100/200-day simulation.

Report:

```text
templates eligible
templates applied
mitigated
unmitigated
recovered
never reached
```

---

# 71. 135B Phase AJ — Dead Template Policy

Never-observed template:

- fix trigger,
- mark rare,
- change seed set,
- remove,
- exempt with reason.

No silent dead data.

---

# 72. 135B Definition of Done

- [ ] 15 authored templates,
- [ ] shelter damage,
- [ ] cold/heat-load pressure,
- [ ] flooding,
- [ ] radiation storm,
- [ ] expedition blocking/risk,
- [ ] vehicle risk,
- [ ] faction tempo changes,
- [ ] relief opportunities,
- [ ] price/scarcity modifiers,
- [ ] mental-health pressure,
- [ ] cabin fever,
- [ ] location closure/contamination/damage,
- [ ] cache reveal where supported,
- [ ] weather-gated quests,
- [ ] weather alert UI,
- [ ] journal,
- [ ] legacy event,
- [ ] recovery semantics,
- [ ] utilization report,
- [ ] no dead template without disposition.

---

# 73. Workstream 135C — Integration / Consequences / Validation

## Goal

Prove every cascade writes through the canonical owner, remains deterministic and save-safe, recovers correctly, and does not overwhelm the player or economy.

---

# 74. 135C Phase A — Thermal Integration

Weather affects thermal demand through `ShelterThermalSystem`.

Assert:

```text
same weather
+ different insulation/power/fuel state
→ different thermal outcome
```

This proves canonical mitigation.

---

# 75. 135C Phase B — Ventilation Integration

Radiation storm feeds filtration pressure.

Assert:

- functional filters reduce indoor impact,
- degraded filters increase risk,
- weather system itself does not own dose.

---

# 76. 135C Phase C — Sump Flooding Integration

Heavy precipitation feeds real sump load.

Assert:

- operational sump mitigates,
- overwhelmed sump produces existing damage/contamination path.

---

# 77. 135C Phase D — Mental Health Integration

Weather stress enters `MentalHealthCrisisSystem` via declared input.

No direct mutation of diagnosis state.

---

# 78. 135C Phase E — Expedition Integration

Departure/route estimate reads current weather effect.

UI estimate and runtime must agree.

---

# 79. 135C Phase F — Route Recovery

When event ends:

- temporary closure clears,
- ongoing physical damage may remain.

Differentiate:

```text
weather blockade ended
vs
bridge washed out
```

---

# 80. 135C Phase G — Faction Integration

Weather modifier affects faction operation cadence.

Verify:

- military pause/reduction,
- irregular cover bonus,
- patrol reduction,
- relief event opportunity,

according to authored doctrine.

---

# 81. 135C Phase H — Economy Integration

Economy applies supply/price modifier once.

At recovery:

```text
modifier decays/removes
```

No permanent cumulative inflation unless separate economy system owns it.

---

# 82. 135C Phase I — Quest Integration

Weather-gated quests:

- appear in valid window,
- expire correctly,
- do not duplicate on reload,
- preserve accepted state after weather changes where design says so.

---

# 83. 135C Phase J — Location Integration

Temporary inaccessibility uses canonical location/route authority.

No hidden second “weather locked” flag consulted only by weather UI.

---

# 84. 135C Phase K — Save/Load Round Trip

Test at:

```text
forecast
approaching
active
recovering
ended
```

Reload preserves:

- event ID,
- phase,
- effect applications,
- mitigation state,
- recovery timers.

---

# 85. 135C Phase L — No Double Application

Reload during active severe storm:

```text
does not apply shelter damage twice
does not duplicate price modifier
does not double faction slowdown
```

---

# 86. 135C Phase M — Old Save Compatibility

Old save:

```text
empty cascade section
```

loads cleanly.

Current weather state may initialize from canonical weather system according to grace policy.

---

# 87. 135C Phase N — Exploit Prevention

Weather events cannot be farmed through:

- reload,
- cancel/restart expedition,
- repeated UI open,
- repeated forecast refresh.

Persist event/application IDs.

---

# 88. 135C Phase O — Perpetual Good Weather Edge Case

Run long soak with benign weather.

Expected:

- no harmful cascade,
- no stale effect state,
- no forced drama fallback.

---

# 89. 135C Phase P — Perpetual Severe Weather Edge Case

Run stress test.

Goal is not “game remains easy.”

Check:

- caps/mitigation,
- no runaway repeated damage every tick,
- price modifier bounded,
- faction simulation still progresses,
- player not spammed with duplicate warnings.

---

# 90. 135C Phase Q — Effect Stacking Rules

When multiple weather events/effects overlap:

Define:

```text
additive
multiplicative
max-only
exclusive
```

per effect category.

No accidental exponential multipliers.

---

# 91. 135C Phase R — Player Pressure Budget

In one severe event, limit simultaneous high-attention consequences.

Example:

```text
1 primary crisis
2 secondary pressures
other effects represented passively
```

Tune via playtest.

---

# 92. 135C Phase S — Forecast Fairness

Compare outcomes at:

- high forecast accuracy,
- low forecast accuracy,
- no forecast.

Measure:

- preparation success,
- damage avoided,
- expedition cancellations,
- stockpile advantage.

Forecast should matter without becoming omniscient.

---

# 93. 135C Phase T — Economy Abuse Tests

Check stockpile arbitrage.

Prevent trivial loop:

```text
forecast
buy guaranteed cheap
sell guaranteed high
repeat
```

Possible mitigations:

- transaction spread,
- limited availability,
- imperfect forecast,
- decay,
- storage cost.

Use economy authority, not weather hack.

---

# 94. 135C Phase U — Expedition Abuse Tests

Do not allow:

```text
start/cancel route repeatedly
→ reroll weather risk
```

Risk draw/state deterministic for event/route context.

---

# 95. 135C Phase V — Faction Abuse Tests

Weather cover should not allow infinite faction farming.

Encounter/reward systems enforce own limits.

---

# 96. 135C Phase W — UI Attention / Briefing

Major weather effects:

- one consolidated alert,
- briefing summary,
- detailed weather panel.

Avoid one modal per downstream effect.

---

# 97. 135C Phase X — Accessibility

Weather alert:

- text severity,
- icons,
- no color-only risk,
- keyboard/controller navigation,
- localized effect explanations.

---

# 98. 135C Phase Y — Audio/Visual Parity

Plan 52 ambience may reflect weather.

But mechanical consequences must be readable without audio.

Plan 51 weather visual changes may reinforce current state.

No duplicated authority.

---

# 99. 135C Phase Z — Retention

Weather history:

- recent detailed events,
- landmark extreme events,
- bounded summaries.

Use Plan 55 retention policy.

---

# 100. 135C Phase AA — `--weather-cascade-selftest`

Required scenarios:

1. storm → shelter pressure,
2. cold → thermal demand,
3. rain → sump load,
4. radiation storm → filtration stress,
5. severe weather → expedition modifier,
6. faction tempo modifier,
7. economy shock/recovery,
8. mental-health stress,
9. location closure/reopen,
10. save/load idempotency,
11. old save,
12. good-weather no-op,
13. severe-weather bounded pressure.

---

# 101. 135C Phase AB — Content Integrity

Validate:

- weather kinds,
- target systems,
- locations/regions,
- quest IDs,
- item/category IDs,
- localization keys,
- effect stacking modes,
- recovery policies.

---

# 102. 135C Phase AC — Deliberate Failure Proof

Break:

- target ID,
- missing port,
- duplicate application key,
- recovery rule.

Assert gate/selftest fails.

---

# 103. 135C Phase AD — 200-Day Cascade Soak

Record:

```text
weather events
effects applied
effects mitigated
shelter damage
route blocks
price shocks
faction modifiers
mental-health effects
location closures
recovery completion
```

---

# 104. 135C Phase AE — Same-Seed Replay

Same seed + same player policy:

```text
same weather event sequence
same effect application IDs
same recovery sequence
same final digest
```

---

# 105. 135C Phase AF — Balance Profiles

Run at least:

```text
prepared
reactive
reckless
forecast-blind
```

Compare:

- survival,
- resource cost,
- shelter damage,
- expedition losses,
- price burden.

Prepared play should have measurable advantage but not immunity.

---

# 106. 135C Phase AG — Long Winter Scenario

Test extended severe/cold sequence.

Assert:

- price shocks do not stack forever,
- morale/mental-health pressure bounded,
- heat/fuel systems remain authoritative,
- recovery happens when season/weather changes.

---

# 107. 135C Phase AH — Playtest

Scenarios:

1. prepare for storm,
2. travel despite warning,
3. exploit reduced patrols,
4. manage cold snap scarcity,
5. recover from flooded location.

Evaluate whether weather feels:

```text
strategic
telegraphed
consequential
not arbitrary
```

---

# 108. 135C Phase AI — Documentation

Create:

```text
docs/systems/WEATHER_CASCADE.md
```

Include:

- authority boundaries,
- effect types,
- ports,
- stacking,
- recovery,
- forecast information model,
- save contract,
- adding templates.

---

# 109. 135C Definition of Done

- [ ] thermal integration,
- [ ] ventilation integration,
- [ ] sump integration,
- [ ] mental-health integration,
- [ ] expedition integration,
- [ ] faction integration,
- [ ] economy integration,
- [ ] quest integration,
- [ ] location integration,
- [ ] save/load at all phases,
- [ ] no duplicate effects,
- [ ] old-save support,
- [ ] anti-farming,
- [ ] good-weather no-op,
- [ ] severe-weather bounded,
- [ ] stacking rules,
- [ ] pressure budget,
- [ ] forecast fairness,
- [ ] exploit tests,
- [ ] accessibility,
- [ ] audio/visual parity,
- [ ] retention,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] 200-day soak,
- [ ] same-seed replay,
- [ ] balance profiles,
- [ ] long-winter scenario,
- [ ] playtest,
- [ ] docs.

---

# 110. Integrated Weather Cascade Pipeline

```text
WeatherSystem
   │
   ├─ kind
   ├─ severity
   ├─ region
   └─ timing
   │
   ▼
WeatherCascadeSystem
   │
   ├─ effect eligibility
   ├─ seeded application
   ├─ idempotency
   └─ recovery scheduling
   │
   ▼
typed downstream intents
   │
   ├────────► ShelterThermal / Integrity / Sump / Ventilation
   ├────────► Travel / Expeditions / Vehicles
   ├────────► Faction operations
   ├────────► Economy / Trade
   ├────────► Mental health / Morale
   ├────────► Location / Topology / Radiation
   └────────► Quest runtime
   │
   ▼
semantic events + player-visible forecast/alerts
```

---

# 111. Weather Authority Contract

Weather cascade may read:

- current weather,
- forecast,
- severity,
- timing,
- regions.

It may not author or overwrite canonical weather.

---

# 112. Forecast Contract

Forecast state is player knowledge.

Exact future weather remains simulation truth.

UI exposes:

```text
confidence
range
lead time
predicted effect categories
```

not hidden random outcomes.

---

# 113. Shelter Contract

Weather requests/applications feed canonical shelter systems.

No weather-owned:

```text
temperature
flood level
radiation dose
structural HP
```

---

# 114. Travel Contract

Weather affects:

- accessibility,
- risk,
- stamina,
- vehicles.

Travel authority owns route state and traversal.

---

# 115. Faction Contract

Weather modifies operation opportunity.

Faction system still owns:

- control,
- standing,
- raids,
- patrols,
- doctrine.

---

# 116. Economy Contract

Weather supplies modifiers.

Economy calculates final prices/availability.

---

# 117. Mental Health Contract

Weather supplies stressors.

Mental-health system decides crisis/diagnosis/state.

---

# 118. Location Contract

Weather supplies temporary/permanent effect intents.

World topology/location authority stores actual accessibility/infrastructure state.

---

# 119. Quest Contract

Weather creates availability conditions/cause IDs.

Quest runtime owns lifecycle.

---

# 120. Recovery Contract

Every non-permanent effect declares exactly how it ends.

No stranded modifiers.

---

# 121. Idempotency Contract

An application can occur once per:

```text
event
effect
target
phase
```

---

# 122. Save Contract

Persist:

- active weather-cascade event applications,
- recovery timers,
- idempotency keys,
- persistent weather landmarks.

Static templates remain data.

---

# 123. Old-Save Contract

Missing cascade section is valid.

No forced migration damage.

---

# 124. Stacking Contract

Every effect category declares:

```text
stacking mode
cap
priority
```

---

# 125. Fairness Contract

Where severe weather is meant to be preparable:

- forecast exists,
- mitigation is possible,
- cost is understandable,
- failure is not fully hidden.

---

# 126. Anti-Farming Contract

Reload/UI refresh cannot recreate:

- quest rewards,
- price spikes,
- cache reveals,
- damage rolls.

---

# 127. Retention Contract

Recent weather detail may roll up.

Preserve:

- extreme landmark events,
- ending/completion impacts,
- persistent location damage,
- unresolved recovery.

---

# 128. Presentation Contract

Weather alert UI is a projection.

It does not tick or apply weather effects.

---

# 129. Audio Contract

Plan 52 audio reflects actual weather state.

Audio never becomes gameplay authority.

---

# 130. Balance Contract

Weather should create:

```text
preparation value
timing tradeoffs
regional differences
emergent consequence
```

not constant unavoidable punishment.

---

# 131. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| cascade duplicates downstream state | Medium | High | strict authority/port boundaries |
| too many effects fire at once | High | High | pressure budget + stacking |
| forecasts become omniscient | Medium | Medium | confidence/uncertainty |
| poor forecast feels unfair | Medium | High | minimum telegraph rules |
| price shocks create arbitrage exploit | Medium | High | economy abuse tests |
| severe weather soft-locks travel | Medium | High | alternate routes/wait/recovery |
| old save gets immediate damage | Medium | High | legacy grace policy |
| recovery modifier never clears | Medium | High | explicit recovery contract |
| weather history grows forever | Medium | Medium | retention |
| event save/load doubles damage | Medium | High | idempotency keys |
| faction modifiers stack endlessly | Medium | High | scoped timed modifiers |
| weather quest farming | Medium | Medium | event IDs/cooldowns |

---

# 132. Commit Strategy

## 135A — Foundation

### C2[27].1 — baseline + weather cascade ADR

### C2[27].2 — event/effect/state DTOs

### C2[27].3 — weather_effects.json schema

### C2[27].4 — deterministic effect resolver

### C2[27].5 — phase/recovery/idempotency

### C2[27].6 — save/old-save support

### C2[27].7 — shelter/expedition ports

### C2[27].8 — faction/economy/mental/location ports

### C2[27].9 — semantic events + diagnostics

### Gate: 135A complete

---

## 135B — Content

### C2[27].10 — shelter/cold/flood/rad templates

### C2[27].11 — expedition/vehicle templates

### C2[27].12 — faction operation templates

### C2[27].13 — economy templates

### C2[27].14 — mental-health templates

### C2[27].15 — location templates

### C2[27].16 — weather quests + alert UI

### C2[27].17 — journal/legacy/recovery/utilization

### Gate: 135B complete

---

## 135C — Closure

### C2[27].18 — canonical downstream integrations

### C2[27].19 — save/load/idempotency matrix

### C2[27].20 — exploit/stacking/pressure-budget tests

### C2[27].21 — forecast fairness + economy abuse

### C2[27].22 — content integrity/selftest

### C2[27].23 — 200-day soak + same-seed replay

### C2[27].24 — prepared/reactive/reckless balance profiles

### C2[27].25 — long-winter scenario + playtest

### C2[27].26 — docs/release closure

### Gate: 135C complete

---

# 133. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --weather-cascade-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
weather template content-utilization report
same-seed weather replay
old-save fixture load
200-day cascade soak
long-winter stress scenario
forecast fairness/balance sweep
weather-alert snapshot/accessibility check
```

---

# 134. Flagship Definition of Done

## 135A — Foundation

- [ ] WeatherCascadeSystem,
- [ ] typed event/effect contracts,
- [ ] weather_effects.json,
- [ ] deterministic resolver,
- [ ] region targeting,
- [ ] phases,
- [ ] recovery,
- [ ] save state/versioning,
- [ ] old-save support,
- [ ] idempotency,
- [ ] all required downstream ports,
- [ ] semantic events,
- [ ] diagnostics,
- [ ] headless tick.

## 135B — Content

- [ ] 15 effect templates,
- [ ] shelter damage,
- [ ] cold/heat demand,
- [ ] flooding,
- [ ] radiation/filtration stress,
- [ ] expedition blocking/risk,
- [ ] vehicle weather risk,
- [ ] faction operation changes,
- [ ] economy scarcity/prices,
- [ ] mental-health pressure,
- [ ] location accessibility/damage,
- [ ] weather quests,
- [ ] weather alert UI,
- [ ] journal,
- [ ] extreme-event legacy,
- [ ] recovery,
- [ ] utilization report.

## 135C — Integration

- [ ] thermal canonical path,
- [ ] ventilation canonical path,
- [ ] sump canonical path,
- [ ] mental-health canonical path,
- [ ] expedition canonical path,
- [ ] faction canonical path,
- [ ] economy canonical path,
- [ ] quest canonical path,
- [ ] location canonical path,
- [ ] save/load phase matrix,
- [ ] no duplicate effects,
- [ ] anti-farming,
- [ ] good-weather no-op,
- [ ] severe-weather bounded,
- [ ] stacking/caps,
- [ ] forecast fairness,
- [ ] economy/expedition/faction exploit tests,
- [ ] accessibility,
- [ ] retention,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] 200-day soak,
- [ ] same-seed replay,
- [ ] strategy profiles,
- [ ] long-winter test,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no duplicated weather-owned downstream facts,
- [ ] no per-frame cascade polling,
- [ ] no silent permanent modifiers,
- [ ] no unavoidable untelegraphed severe-weather punishment where preparation is promised,
- [ ] no save/load double application,
- [ ] full verification green.

---

# 135. Closure Report Template

```markdown
## C2[27] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Weather kinds:
- Forecast horizon:
- Forecast accuracy:
- Existing cascade consumers:
- Existing weather-gated routes:
- Weather effect templates before:

### 135A — Foundation
- WeatherCascadeSystem:
- Event/effect schema:
- Seed stream:
- Region targeting:
- Phase/recovery:
- Save schema:
- Old save:
- Shelter ports:
- Expedition ports:
- Faction ports:
- Economy ports:
- Mental-health ports:
- Location ports:
- Missing required ports:
- Result:

### 135B — Content
- Templates total:
- Shelter templates:
- Expedition templates:
- Faction templates:
- Economy templates:
- Mental templates:
- Location templates:
- Quests:
- Alert UI:
- Journal:
- Extreme legacy:
- Unused templates:
- Result:

### 135C — Integration
- Thermal:
- Ventilation:
- Sump:
- Expedition:
- Faction:
- Economy:
- Mental health:
- Location:
- Quest:
- Save/load idempotency:
- Anti-farm:
- Good-weather soak:
- Severe-weather soak:
- Forecast fairness:
- Strategy profiles:
- Long winter:
- Playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Weather cascade selftest:
- Port contract:
- Content utilization:
- Old-save fixtures:
- Same-seed replay:
- 200-day soak:
- Verify fast:

### Final Metrics
- WEATHER_EFFECT_TEMPLATES:
- WEATHER_EFFECTS_ELIGIBLE:
- WEATHER_EFFECTS_APPLIED:
- WEATHER_EFFECTS_MITIGATED:
- WEATHER_EFFECTS_RECOVERED:
- DUPLICATE_EFFECT_APPLICATIONS:
- ROUTE_BLOCK_EVENTS:
- SHELTER_DAMAGE_EVENTS:
- PRICE_SHOCK_EVENTS:
- FACTION_OPERATION_MODIFIERS:
- MENTAL_HEALTH_WEATHER_EVENTS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Weather content:
- Forecasting:
- Travel:
- Factions:
- Economy:
- Mental health:
- UI:
```

---

# 136. Final Execution Directive

Execute Plan 135 as a **cross-system weather projection layer**, not as a second world simulation.

The critical sequence is:

```text
bind to the canonical weather source
→ resolve deterministic effect intents
→ route those intents into the systems that already own shelter/travel/factions/economy/health/locations
→ expose forecasts and preparation choices
→ persist effect application/recovery safely
→ prevent reload farming and duplicate consequences
→ balance severe weather across prepared/reactive/reckless play
→ prove recovery and same-seed behavior in long soaks
```

Do not store duplicate temperatures, prices, faction state, route state, radiation, or morale inside the weather cascade.

Do not make a route “weather blocked” only in weather UI while travel still ignores it.

Do not let price modifiers survive after the event by accident.

Do not let reload reapply storm damage.

Do not let forecast certainty erase strategic uncertainty.

The strongest authority rule is:

> **Weather owns weather; downstream systems own consequences; the cascade only connects them.**

The strongest fairness rule is:

> **Severe weather should punish poor preparation more than bad luck: the player needs forecasted risk, mitigation options, and understandable recovery.**

The strongest integration rule is:

> **Every weather effect must be observable in the real owning system and must disappear, decay, or persist according to an explicit recovery contract.**

The flagship acceptance scenario is:

> **Generate one seeded severe storm affecting the shelter region and an expedition route. Let the forecast arrive early enough to reinforce one weak shelter section and stock heating fuel, then deliberately dispatch an expedition anyway. Verify the storm raises thermal demand, pressures the sump/filtration path, modifies the route/risk estimate, changes faction/economy opportunity where applicable, and records a weather journal entry. Save/load mid-storm and prove no effect applies twice. When the storm ends, temporary route/price modifiers must recover while any real structural/location damage remains under its canonical owner. Re-run with the same seed and reproduce the same event/effect sequence.**
