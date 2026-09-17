# C2 — Flagship Integration Plan [10]: Autonomous Outside World, World Politics, Consequence Reach, and Neighbour Simulation

> **Deliverable:** `C2_planintegration[10].md`
> **Source scope:** Plan 30 — *The War Runs Without You: An Autonomous Outside World*
> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
> **Primary objective:** make the world outside the shelter advance independently of player input, beginning with faction-war simulation, then routing those changes into existing player-facing systems, and finally extending autonomy to caravans, waystations, coast/maritime actors, wildlife, and world evolution while preserving information scarcity.
> **Required execution order:** **Plan 31 → 30A → 30B → 30C**
> **Hard prerequisite:** Plan 31 semantic event kinds must land before autonomous world changes are allowed into the briefing/journal/radio stream.
> **Registration prerequisite:** Plan 28A subsystem manifest must be available before adding/extending the world-politics day owner so setup/save/flush/day-owner obligations cannot drift.
> **Cross-plan dependencies:** Plan 34A for real economy shock tuning, Plan 32 travel graph for territory-aware routes, Plan 33 intel channels for information propagation.
> **Scope discipline:** no new faction system, no new war mechanic, no new generic world-event framework, no omniscient world map, no new prose hardcoded in C#, and no autonomous change that bypasses semantic event reporting or save/determinism contracts.

---

# 0. Executive Intent

ASHFALL already contains a substantial outside-world simulation vocabulary:

- faction standing,
- territorial control,
- war tension,
- decrees,
- territorial clashes,
- artillery strike accounting,
- faction radio,
- trade price shock concepts,
- expedition risk,
- airlock security,
- caravans,
- world evolution,
- wildlife migration,
- maritime/coast systems,
- waystations.

The central continuity defect is that many of these systems are **reactive only**.

The player acts.
The world responds.

But the world itself does not meaningfully advance unless the player pushes it.

The source identifies the most direct example:

```text
FactionWarSystem.SimulateDailyFriction(day)
```

already exists, is deterministic, is tested, and has no live game-path caller.

This plan turns the outside world from a static backdrop into an autonomous simulation.

The intended chain is:

```text
day advances
→ autonomous world owners advance
→ faction/world facts change
→ semantic events record what happened
→ consequences reach economy/travel/security/radio/map
→ information channels determine what the player actually knows
→ the player responds to a world that did not wait for them
```

The flagship player-facing outcome is:

> **Leave the shelter alone for thirty days and the outside world has changed for reasons independent of you; when you return to its consequences, you can trace what changed, who caused it, how you learned about it, and what it now costs you.**

---

# 1. Source Evidence and Architectural Reading

The source plan identifies a particularly strong case for integration rather than invention:

- `FactionWarSystem` already models meaningful political state,
- `SimulateDailyFriction(day)` exists and is tested,
- `YearOfAshHostSession.TickDay` is live but omits faction-war advancement,
- `FactionWarChainRunner` already has save shape but no host construction/caller,
- world presentation already queries faction-war state,
- player-caused standing changes already land in the system,
- faction radio data is already consumed,
- trade UI already contains faction-war shock concepts,
- the day loop has a natural phase for world politics.

The correct implementation stance is therefore:

```text
activate existing world simulation
→ connect consequences
→ extend autonomy only through existing systems
```

not:

```text
design a new world-events framework
```

---

# 2. Program-Level Success Criteria

C2[10] is complete only when all of the following are true.

## 2.1 Faction politics advance without player input

A no-action campaign still changes:

- standing,
- tension,
- territory,
- dominance,
- decrees,
- clash history.

## 2.2 Political advancement is part of the campaign day loop

No side timer.
No UI-triggered simulation.
No hidden tick.

## 2.3 Autonomous changes are semantically reported

Every meaningful transition maps to Plan 31 event vocabulary.

## 2.4 State survives save/load and catch-up

Loading an old save does not freeze politics or skip elapsed world days.

## 2.5 Consequences reach existing player systems

Political state changes affect:

- prices,
- trade access,
- radio,
- expedition risk/disposition,
- map control,
- airlock pressure,
- population/refugee pressure where appropriate.

## 2.6 The world does not become omniscient

The player learns distant facts through:

- radio,
- caravan,
- scout/intel,
- journal/briefing,
- local observation.

## 2.7 Neighbour systems have autonomous schedules/intentions

Caravans, waystations, coast/maritime actors, wildlife/world evolution do not exist only as player-triggered content.

## 2.8 Autonomy remains deterministic

Same seed + same player actions → same world evolution digest.

## 2.9 Autonomy remains recoverable and balanced

A hostile outside world tightens pressure but does not make campaigns unwinnable by hidden global drift.

---

# 3. Architectural Invariants

## 3.1 Plan 31 event vocabulary first

Do not emit ad-hoc strings such as:

```text
market_ticked
war_changed
faction_update
```

if semantic kinds are not canonicalized.

## 3.2 Plan 28A manifest owns registration

Any new or extended day owner must declare:

- setup,
- save,
- flush policy,
- day phase,
- event sources,
- teardown where needed.

## 3.3 `FactionWarSystem` remains political-state authority

Do not create:

- `WorldPoliticsSystem`,
- duplicate faction tension model,
- panel-local territory state.

## 3.4 Existing consequence systems remain owners

Politics may influence:

- `MarketSystem`,
- `FactionRadioEngine`,
- `ExpeditionSystem`,
- `AirlockSecuritySystem`,
- population systems.

But politics does not replace them.

## 3.5 Consequences are channelled, not duplicated

Examples:

```text
war tension
→ PriceShockKind.FactionWar
```

not:

```text
war tension
→ direct arbitrary item price edits
```

## 3.6 Information is delayed/filtered

Simulation truth and player-known truth are separate.

## 3.7 No prose in mechanics

All new player-facing text uses Plan 25 localization/string layer.

## 3.8 Daily autonomy uses stable ordering

No dictionary-order dependence.

## 3.9 Catch-up is deterministic

Old save world advancement runs the same logical daily steps as live play.

## 3.10 Autonomous world work stays within day-advance budget

Use Plan 26C performance contracts.

---

# 4. Dependency Graph

```text
Plan 31 — semantic event kinds
 │
 ▼
30A — world politics day owner
 │
 ├──────────────► save/catch-up/determinism
 ├──────────────► political semantic events
 │
 ▼
30B — consequences reach player systems
 │
 ├─ economy (34A)
 ├─ radio
 ├─ expedition risk (32)
 ├─ trade stance
 ├─ airlock pressure
 └─ map/territory attribution
 │
 ▼
30C — neighbours act autonomously
 │
 ├─ caravans
 ├─ waystations
 ├─ coast/maritime
 ├─ wildlife
 ├─ location evolution
 └─ information gating (33)
```

Additional structural dependency:

```text
28A manifest
→ registration correctness for world_politics owner
```

---

# 5. Prerequisite Gate

Do not begin implementation unless:

## Plan 31

- canonical semantic world/politics event kinds exist,
- briefing consumer can handle them,
- no-silent-drop policy is active.

## Plan 28A

- subsystem manifest can declare/extensively validate day owner,
- save/flush/day ownership cannot be registered separately by hand.

If prerequisites are absent:

```text
STOP C2[10] implementation at the integration boundary
and complete prerequisite plan first.
```

Do not invent temporary vocabularies or manual registration that must later be migrated.

---

# 6. Baseline Capture

Record:

- commit SHA,
- current faction-war state after 30/90/180 no-action days,
- current day-owner set,
- current event counts,
- current save digest,
- current map/trade/radio behavior,
- current runtime-scale numbers.

## 6.1 Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expansions-selftest
bash scripts/ci/verify-fast.sh
```

Also run:

```text
ashfall-seed-replay
ashfall-balance-sim
```

using repository-canonical invocations.

---

# 7. Workstream 30A — Put the World on the Clock

## 7.1 Objective

Faction-war state advances every day from its own rules and is fully integrated into campaign persistence/event reporting.

---

# 8. 30A Phase A — Read and Pin `SimulateDailyFriction`

Before wiring:

- document every state field it mutates,
- document every event it raises,
- document its RNG dependency,
- document iteration ordering,
- document tension-stage behavior.

Use existing tests as behavioral contract.

Do not alter friction behavior in the same commit that wires it live.

---

# 9. 30A Phase B — Failing No-Action Integration Test

Test:

```text
new campaign
no player political actions
advance N days
```

Current expected failure:

```text
FactionWar state unchanged
```

Post-fix:

```text
war state digest changed
```

Pin a representative seed and expected digest/semantic transitions.

---

# 10. 30A Phase C — `YearOfAshHostSession.TickDay`

Wire faction-war advancement beside existing Year-of-Ash daily work.

Requirements:

- same session RNG infrastructure,
- no `System.Random`,
- no separate timer,
- no UI trigger.

---

# 11. 30A Phase D — `world_politics` Day Owner

Prefer extending an existing owner if phase semantics fit.

Otherwise register:

```text
world_politics
```

through Plan 28A manifest.

Declare:

- owner ID,
- phase,
- setup,
- save section,
- flush policy,
- events,
- catch-up support.

Do not add a manual `_campaignDay.Register(...)` outside manifest-driven orchestration.

---

# 12. 30A Phase E — Day Phase Ordering

Political advancement must occur in an intentional phase relative to:

- weather/world evolution,
- economy,
- expeditions,
- briefing/event aggregation.

Document whether:

```text
politics updates
→ economy reads new state same day
```

or next day.

Do not leave this implicit.

---

# 13. 30A Phase F — `FactionWarChainRunner` Activation

Instantiate the saved-but-unused chain runner.

Connect it to existing encounter/choice resolution idiom.

Do not build a new choice resolver.

Requirements:

- chain state restored,
- resolve choice routes through canonical effects,
- semantic event emitted,
- save persists progression.

---

# 14. 30A Phase G — Decree Source

Decrees must originate from:

- authored faction decree data,
- existing chain outcomes,
- or existing system state.

Do not hardcode decree IDs in day-owner code.

Each decree should define:

- ID,
- eligibility,
- duration/window,
- effects,
- presentation key.

---

# 15. 30A Phase H — Semantic Event Mapping

Map existing Core events to canonical Plan 31 kinds.

Conceptual semantic events:

- `standing_shifted`,
- `tension_stage_changed`,
- `territory_changed`,
- `decree_enacted`,
- `clash_occurred`,
- `dominance_changed`.

Use actual canonical names from Plan 31 if different.

Each event should carry:

- source owner,
- faction IDs,
- old/new values,
- significance,
- related location/territory if applicable.

---

# 16. 30A Phase I — Briefing Rate Limiting

Political systems can produce many changes.

Introduce significance aggregation.

Rules:

- major transitions always visible,
- minor same-type drift can aggregate,
- cap political briefing lines/day,
- raw semantic events remain available to logs/journal if needed.

Do not drop simulation facts silently.

---

# 17. 30A Phase J — Persistence

Ensure war state save contains:

- faction records,
- tension,
- dominant faction,
- decrees,
- artillery/clash state,
- chain-runner state,
- last advanced day.

Add round-trip:

```text
advance 30
save
load
advance 30
```

versus uninterrupted 60-day run.

Digests must match.

---

# 18. 30A Phase K — Old Save Catch-Up

For old saves:

```text
lastAdvancedDay < campaignDay
```

run deterministic daily catch-up.

Requirements:

- same ordering as live days,
- bounded operation,
- no duplicate player-facing event spam from historical days unless designed,
- final world state identical to continuous simulation.

Consider summary aggregation for historical catch-up reporting.

---

# 19. 30A Phase L — Determinism Gate

Run 180-day paired-seed simulation.

Digest should include:

- standings,
- territory,
- tension,
- dominance,
- decrees,
- clash/artillery counters,
- chain state.

Ensure:

- faction iteration ordinal,
- territory iteration ordinal,
- no hash-order influence.

---

# 20. 30A Phase M — Performance

Measure political daily pass separately.

Target:

```text
negligible relative to Plan 26C day budget
```

Record per-owner timing if diagnostics support it.

No per-frame politics.

---

# 21. 30A Tests

- no-action state advances,
- friction by tension stage,
- standing transition,
- tension stage transition,
- territory shift,
- dominance handover,
- decree enactment,
- clash event,
- chain resolution,
- save/load round-trip,
- catch-up equivalence,
- paired-seed digest,
- event rate limit,
- performance budget.

---

# 22. 30A Definition of Done

- [ ] friction live,
- [ ] day owner manifest-declared,
- [ ] phase explicit,
- [ ] chain runner live,
- [ ] decree source data/chain-driven,
- [ ] semantic events mapped,
- [ ] briefing aggregation,
- [ ] save round-trip,
- [ ] old-save catch-up,
- [ ] 180-day replay deterministic,
- [ ] performance within budget,
- [ ] day 90 world differs from untouched baseline.

---

# 23. Workstream 30B — Let the War Reach the Player

## 23.1 Objective

Political state must alter existing gameplay systems without creating a new generic world-events layer.

---

# 24. 30B Phase A — Reach Matrix

Before code, create:

| Political output | Existing consumer | Effect | Magnitude source | Player evidence |
|---|---|---|---|---|
| standing band | trade | refuse/discount/embargo | faction stance data | trade UI/radio |
| tension | market | price shock | 34A tuning | briefing/trade |
| territory | expeditions | risk/disposition | travel graph | map/dispatch |
| dominance | radio/map | weighting/control | faction data | radio/map |
| decree | economy/security | modifier | authored decree | briefing/journal |
| clash | route/airlock | risk/pressure | existing systems | map/radio |

Only implement rows with real existing consumers.

---

# 25. 30B Phase B — Economy Integration

Coordinate with Plan 34A.

Use real tuning bundle.

Political inputs should route through:

- existing price shock kinds,
- existing market modifiers,
- existing faction stance.

Do not set prices directly from `FactionWarSystem`.

---

# 26. 30B Phase C — Price Shock Attribution

When war changes price:

```text
political event
→ shock rule
→ market price effect
→ player-facing reason
```

UI should expose cause such as:

```text
Faction war disrupted convoy supply.
```

via localization keys.

---

# 27. 30B Phase D — Faction Radio

Use existing `FactionRadioEngine`.

Selection weighting/gating can consume:

- standing band,
- tension stage,
- dominant faction,
- territorial events.

Do not author new prose in this plan unless corpus coverage is insufficient and separately approved.

---

# 28. 30B Phase E — Expedition Risk

Use Plan 32 travel graph.

Route control/territory owner affects:

- encounter odds,
- faction disposition,
- route risk.

Reuse existing encounter bridge.

No new war encounter class unless current bridge cannot represent needed parties.

---

# 29. 30B Phase F — Trade Availability

Use live campaign `FactionStanceEngine`.

Examples:

- allied → preferred access,
- neutral → standard,
- hostile → restrictions,
- blood feud → refusal/embargo.

Do not construct a fresh stance engine.

---

# 30. 30B Phase G — Airlock Security Pressure

Connect war tension / nearby hostile control to existing airlock incident pressure.

Use existing sentry/security/breach paths.

Political system supplies risk context.

Airlock system owns actual incident mechanics.

---

# 31. 30B Phase H — Map Overlay

`FactionWarMapWidget` should render:

- current control,
- changed territory,
- cause/related clash/decree.

Click-through should show attributable event history.

Do not expose facts the player does not know if Plan 33 intel gating says they are unknown.

---

# 32. 30B Phase I — Refugee / Census Ripple

Where neighboring conflict creates arrivals:

- route through existing visitor triage,
- census/register systems,
- food/duty/space systems then carry cost.

Do not create a standalone refugee resource ledger.

---

# 33. 30B Phase J — No Silent External Mutation

Every externally caused player-impacting change must have at least one evidence channel:

- briefing,
- journal,
- radio,
- local UI explanation.

This applies to:

- price shifts,
- trade refusal,
- raid pressure,
- route closure,
- visitor surge.

---

# 34. 30B Phase K — Localization/Tone

All new strings use Plan 25.

Use existing faction voice/register metadata where applicable.

No prose literals in C#.

---

# 35. 30B Phase L — Balance Sweep

Sweep:

```text
war tension
× faction stance
× territory proximity
× economy scarcity
```

Track:

- price multipliers,
- trade closures,
- raid incidence,
- expedition risk,
- player resource runway.

Acceptance:

- hostile world creates pressure,
- no single autonomous political spiral guarantees unwinnable state,
- recovery/diplomacy/resource alternatives remain.

---

# 36. 30B Integration Test

Pin a scenario:

```text
day 60 political shift
→ day 65 price changes
→ day 65 radio selection changes
→ day 65 expedition encounter/risk changes
```

Same political cause must be traceable across all three outputs.

---

# 37. 30B Definition of Done

- [ ] reach matrix complete,
- [ ] prices react,
- [ ] price cause visible,
- [ ] radio reacts,
- [ ] expeditions react,
- [ ] trade availability reacts,
- [ ] airlock pressure reacts,
- [ ] map reflects control/change,
- [ ] population ripple uses existing systems,
- [ ] no silent external mutation,
- [ ] strings localized,
- [ ] balance sweep passes,
- [ ] runtime content evidence increases.

---

# 38. Workstream 30C — Neighbours Have Their Own Days

## 38.1 Objective

Extend autonomy beyond faction arithmetic to settlements, caravans, wildlife, coast, waystations, and other outside systems.

---

# 39. 30C Phase A — Autonomous-System Audit

Create table:

| System | Already ticks? | Player-triggered only? | Own state? | Save? | Events? | Missing autonomy |
|---|---:|---:|---:|---:|---:|---|

Audit:

- location evolution,
- wildlife migration,
- landmark degradation,
- caravans,
- waystations,
- deep coast,
- maritime/flotilla,
- weather intelligence.

Only add autonomy where missing.

---

# 40. 30C Phase B — Caravan Intentions

Traveling caravans should have:

- origin,
- destination,
- route,
- reason/intent,
- schedule.

Inputs:

- politics,
- weather,
- graph availability,
- trade demand where existing.

A caravan arrival becomes consequence of world state, not a timer alone.

---

# 41. 30C Phase C — Caravan State

Persist:

- current route,
- origin/destination,
- ETA,
- cargo/intent where already modeled,
- disruption state.

No teleporting after load.

---

# 42. 30C Phase D — Waystations

Tie waystation state into Plan 32 graph.

Political/world events may:

- abandon,
- damage,
- reopen,
- change control.

Waystation descriptions/presentation derive from live state.

---

# 43. 30C Phase E — Wildlife

Existing migration remains authority.

Add bias from:

- contamination,
- weather,
- territorial conflict/human activity

only if physically/gameplay justified.

Do not directly set animal counts from politics.

---

# 44. 30C Phase F — Deep Coast / Flotilla

Maritime/coast systems consume shared political facts:

- blockade,
- tribute,
- control,
- hostile pressure.

Do not make coast simulation a disconnected alternative world model.

---

# 45. 30C Phase G — Information Does Not Teleport

This is a critical game-premise rule.

Simulation state:

```text
truth
```

Player state:

```text
known truth
```

A distant event becomes known only through a channel:

- radio,
- caravan,
- scout,
- encounter,
- intelligence system.

Plan 33 owns intel propagation mechanics.

30C supplies events/facts.

---

# 46. 30C Phase H — Known Happenings View

Create or reuse compact view assembled from known briefing/journal/intel entries.

Shows only known facts such as:

- caravan last reported destination,
- known territory holder,
- known collapse,
- known blockade.

This is not a second world-state authority.

---

# 47. 30C Phase I — Semantic Events

Each autonomous change emits canonical Plan 31 event.

Examples:

- caravan departed,
- caravan diverted,
- waystation abandoned,
- waystation reopened,
- wildlife route shifted,
- blockade formed,
- landmark collapsed.

Use actual canonical names.

---

# 48. 30C Phase J — Catch-Up

Use shared world catch-up mechanism.

For each autonomous subsystem:

- store last advanced day,
- catch up deterministically,
- avoid duplicate historical notification spam,
- preserve final state equivalence.

---

# 49. 30C Phase K — Day Owner Timing

All autonomous owners remain inside documented phase order.

Add per-owner timing to diagnostics where possible.

No hidden scheduled timers.

---

# 50. 30C Phase L — Deterministic Cross-System Ordering

World systems may read each other's outputs.

Document order, for example:

```text
weather
→ politics
→ world evolution
→ caravan routing
→ wildlife
→ economy
→ information propagation
```

Use actual phase model.

Pin digest.

---

# 51. 30C No-Action Test

Core flagship test:

```text
start campaign
take no external/world-affecting player actions
advance 30 days
```

Assert:

- world state changes,
- multiple autonomous systems advance,
- known information changes only via channels,
- briefing/journal receives discoverable reports.

---

# 52. 30C Information-Gating Test

Scenario:

```text
distant clash occurs
no radio/scout/caravan channel
```

Expected:

- simulation state changes,
- player-known state does not.

Then activate channel.

Expected:

- fact becomes known,
- map/journal can surface it.

---

# 53. 30C Persistence Tests

Per autonomous subsystem:

```text
advance
save
load
advance
```

equals uninterrupted run.

Also test loading 40 days behind and catching up.

---

# 54. 30C Performance Budget

Total autonomy work must remain within Plan 26C budgets.

Track per owner:

- time,
- allocations,
- event count.

No unbounded event generation.

---

# 55. 30C Definition of Done

- [ ] autonomous-system audit complete,
- [ ] caravans have intentions,
- [ ] caravan state persists,
- [ ] waystations live in graph,
- [ ] wildlife reacts to justified world inputs,
- [ ] coast/maritime consume shared politics,
- [ ] information gating enforced,
- [ ] known-happenings view uses known data only,
- [ ] all autonomous changes emit semantic events,
- [ ] shared catch-up works,
- [ ] deterministic phase ordering,
- [ ] no-action 30-day run advances world,
- [ ] performance within budget.

---

# 56. Integrated Autonomous World Pipeline

```text
Campaign day
   │
   ▼
Manifest-registered world owners
   │
   ├─ weather/world
   ├─ world_politics
   ├─ world_evolution
   ├─ caravan/settlement actors
   ├─ wildlife/maritime actors
   │
   ▼
Canonical world facts
   │
   ├─ political state
   ├─ route control
   ├─ settlement state
   ├─ migration state
   └─ economic pressure
   │
   ▼
Plan 31 semantic events
   │
   ├─────────────► gameplay consequences
   │               ├─ market
   │               ├─ expedition
   │               ├─ trade
   │               └─ airlock
   │
   └─────────────► information channels
                   ├─ radio
                   ├─ caravan
                   ├─ scout/intel
                   └─ journal/briefing
                          │
                          ▼
                    player-known world
```

---

# 57. Truth vs Knowledge Contract

Maintain two concepts:

## World truth

What actually happened.

Owned by simulation systems.

## Player-known truth

What the player has learned.

Owned/filtered by Plan 33/intel and presentation histories.

Do not let:

- map widget,
- briefing,
- trade UI

read secret truth directly when the premise requires uncertainty.

---

# 58. Event Attribution Contract

Every autonomous event should carry enough data to answer:

```text
What happened?
Who/what caused it?
Where?
When?
Was it player-caused or autonomous?
How significant?
How can the player learn about it?
```

This enables:

- briefing,
- journal,
- radio,
- map history,
- diagnostics.

---

# 59. Player-Causality Classification

Classify event cause:

- PLAYER_DIRECT,
- PLAYER_INDIRECT,
- AUTONOMOUS,
- ENVIRONMENTAL,
- MIXED.

Do not label autonomous faction drift as player action.

---

# 60. Save Contract

Each autonomous subsystem must have:

- state authority,
- save section or parent save authority,
- last advanced day,
- deterministic catch-up.

Plan 28A manifest declares registration.

No standalone ad-hoc save files.

---

# 61. Catch-Up Contract

Shared catch-up should process:

```text
lastAdvancedDay + 1
through
currentCampaignDay
```

with stable order.

For long gaps:

- preserve exact final state,
- optionally aggregate player-facing historical notices,
- do not skip simulation days unless mathematically equivalent and proven.

---

# 62. Performance Contract

Per autonomous owner, record:

- median time/day,
- p95 time/day,
- allocations/day,
- events/day.

Ratchet against Plan 26C budgets.

A 30-day autonomous run must not multiply day-advance cost uncontrollably.

---

# 63. Economy Contract

Politics influences economy through existing tuning/market APIs.

Forbidden:

```text
FactionWarSystem directly writes item price
```

Required:

```text
political state
→ authored shock/tuning rule
→ MarketSystem
```

---

# 64. Expedition Contract

Politics influences:

- route risk,
- encounter faction,
- availability,

through travel graph/expedition estimation/runtime.

No UI-only risk changes.

---

# 65. Radio Contract

Faction radio selects from existing corpus.

Inputs:

- known/actual state depending channel rules,
- faction,
- standing,
- tension,
- recent semantic events.

No new hidden news generator.

---

# 66. Map Contract

Map shows:

- current known control,
- known changes,
- attributable known cause.

Unknown territory remains uncertain/last-known where Plan 33 defines it.

---

# 67. Airlock Contract

Outside political pressure modifies existing incident inputs.

Airlock system remains incident authority.

Do not let `FactionWarSystem` directly spawn breach consequences.

---

# 68. Population Contract

Refugee/visitor pressure uses existing:

- triage,
- census,
- roster,
- food,
- duty,
- shelter capacity.

No parallel “world refugee counter” that bypasses shelter simulation.

---

# 69. Failure Modes and Corrective Actions

## 69.1 World advances but briefing is noise

Cause:

- Plan 31 not landed,
- raw events not semantic,
- no significance aggregation.

Fix:

- enforce prerequisite,
- aggregate.

## 69.2 War affects prices but no cause is visible

Fix:

- event → shock attribution → localized UI reason.

## 69.3 Map shows distant truth instantly

Critical premise violation.

Fix:

- player-known layer / Plan 33 gating.

## 69.4 Caravans still appear on timer only

Fix:

- origin/destination intention tied to live world state.

## 69.5 Old save resumes with frozen politics

Fix:

- lastAdvancedDay + catch-up.

## 69.6 Catch-up produces different state from continuous play

Fix:

- daily exact replay,
- stable ordering.

## 69.7 World politics causes runaway resource collapse

Fix:

- tension balance sweep,
- cap/soften authored multipliers,
- preserve recovery paths.

## 69.8 Autonomous systems double-tick

Fix:

- Plan 28A manifest uniqueness,
- one owner registration.

---

# 70. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| event noise | High | Medium | Plan 31 + significance caps |
| autonomous drift changes balance | High | High | seeded sweeps |
| save baselines shift | High | Medium | expected rebaseline after validation |
| catch-up divergence | Medium | High | uninterrupted-vs-catchup digest |
| world becomes omniscient | Medium | High | Plan 33 knowledge layer |
| cross-owner ordering drift | Medium | High | explicit phase order |
| price shocks too punitive | Medium | High | 34A + balance |
| caravan/world event spam | Medium | Medium | event aggregation |
| performance regressions | Medium | Medium | per-owner timing |
| duplicate day owner | Low–Med | High | 28A manifest gate |
| hardcoded prose creeps in | Medium | Medium | Plan 25 new-string gate |

---

# 71. Commit Strategy

## Commit C2[10].1 — Prerequisite verification + baseline

- Plan 31 presence,
- Plan 28A manifest,
- no-action baseline,
- state digest.

## Commit C2[10].2 — faction friction live

- `TickDay`,
- failing→passing no-action test.

## Commit C2[10].3 — world_politics owner / phase

- manifest registration.

## Commit C2[10].4 — chain runner + decrees

## Commit C2[10].5 — semantic events + rate limiting

## Commit C2[10].6 — save/catch-up/determinism/perf

### Gate: 30A complete

## Commit C2[10].7 — reach matrix + economy

## Commit C2[10].8 — radio + expedition

## Commit C2[10].9 — trade + airlock

## Commit C2[10].10 — map + population ripple

## Commit C2[10].11 — attribution + balance + runtime evidence

### Gate: 30B complete

## Commit C2[10].12 — autonomous-system audit

## Commit C2[10].13 — caravan intentions/state

## Commit C2[10].14 — waystations + travel graph

## Commit C2[10].15 — wildlife + coast/maritime

## Commit C2[10].16 — information gating + known happenings

## Commit C2[10].17 — catch-up + ordering + perf

## Commit C2[10].18 — 30-day no-action flagship test

### Gate: 30C complete

## Commit C2[10].19 — integrated Wave-4 autonomy closure

---

# 72. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --expansions-selftest
bash scripts/ci/verify-fast.sh
```

Also run:

```text
ashfall-seed-replay — 180-day world digest
ashfall-balance-sim — tension sweep
no-action 30-day autonomous run
expedition selftest
content-utilization selftest
```

using canonical repository commands.

---

# 73. Flagship Definition of Done

## Prerequisites

- [ ] Plan 31 event vocabulary live,
- [ ] Plan 28A manifest live,
- [ ] world owner registration manifest-driven.

## 30A

- [ ] daily friction live,
- [ ] world politics phase explicit,
- [ ] chain runner live,
- [ ] decrees sourced from data/chain,
- [ ] semantic transitions,
- [ ] event aggregation,
- [ ] war state saved,
- [ ] lastAdvancedDay saved,
- [ ] catch-up exact,
- [ ] 180-day replay deterministic,
- [ ] performance within budget.

## 30B

- [ ] reach matrix complete,
- [ ] economy reacts,
- [ ] radio reacts,
- [ ] expedition reacts,
- [ ] trade reacts,
- [ ] airlock reacts,
- [ ] map reacts,
- [ ] population ripple integrated,
- [ ] every external change attributable,
- [ ] no prose in C#,
- [ ] balance sweep viable.

## 30C

- [ ] autonomous-set audit,
- [ ] caravans have intentions,
- [ ] waystations dynamic,
- [ ] wildlife world-responsive,
- [ ] coast/maritime politically responsive,
- [ ] no information teleportation,
- [ ] known-happenings view based on knowledge,
- [ ] semantic events for autonomous changes,
- [ ] persistence/catch-up,
- [ ] deterministic phase order,
- [ ] per-owner performance measured,
- [ ] no-action 30-day run proves autonomy.

## Cross-system

- [ ] world truth separate from player knowledge,
- [ ] existing systems remain authorities,
- [ ] no new generic world-event framework,
- [ ] no omniscient map,
- [ ] full verification green.

---

# 74. Closure Report Template

```markdown
## C2[10] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Prerequisites
- Plan 31:
- Plan 28A:
- Plan 32:
- Plan 33:
- Plan 34A:

### Baseline
- Faction war state day 0:
- Day 30 no-action:
- Day 90 no-action:
- Current political event count:
- Day-advance median:
- Save digest:

### 30A — Politics
- Friction wiring:
- Day owner:
- Phase:
- Chain runner:
- Decrees:
- Semantic kinds:
- Briefing cap:
- Save:
- Catch-up:
- 180-day digest:
- Perf:
- Result:

### 30B — Reach
- Economy:
- Radio:
- Expedition:
- Trade:
- Airlock:
- Map:
- Population:
- Attribution:
- Balance:
- Runtime evidence:
- Result:

### 30C — Neighbours
- Autonomous audit:
- Caravans:
- Waystations:
- Wildlife:
- Coast/maritime:
- Intel gating:
- Known happenings:
- Semantic events:
- Catch-up:
- Owner ordering:
- 30-day no-action:
- Perf:
- Result:

### Determinism
- Seed:
- Run A digest:
- Run B digest:
- Catch-up digest:
- Continuous digest:

### Full Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Expansions:
- Seed replay:
- Balance sim:
- 30-day no-action:
- Verify fast:

### Final Metrics
- Political changes/day:
- Briefing political lines/day:
- Autonomous owner count:
- World events/day:
- Known-vs-hidden event counts:
- Price shock range:
- Raid/airlock pressure range:
- Day-advance delta:

### Remaining Debt
- Politics:
- Economy:
- Travel graph:
- Intel:
- Neighbour systems:
- Balance:
```

---

# 75. Final Execution Directive

Implement Plan 30 as a world-autonomy repair.

The critical sequence is:

```text
canonical semantic event language
→ activate faction-war daily simulation
→ register it through the subsystem manifest
→ persist/catch up deterministically
→ route political consequences into existing systems
→ separate simulation truth from player-known truth
→ give neighbouring systems autonomous intentions
→ prove the world changes during zero player input
```

Do not create a new world-event framework.

Do not make the map omniscient.

Do not let autonomous state mutate player-visible systems without a trace.

The strongest simulation rule is:

> **The world advances because its own systems have daily rules, not because the player touched a UI.**

The strongest information rule is:

> **A distant fact may be true without being known; the player learns the outside world through maintained information channels.**

The strongest continuity rule is:

> **Every autonomous change that reaches the player must preserve the chain: cause → semantic event → consequence → evidence.**

The flagship acceptance scenario is simple:

> **Advance thirty days with no outside-world player actions. The political map, caravans, routes, wildlife/settlement state, prices, and known news must no longer match day zero — and every known change must be attributable, deterministic, persisted, and delivered through a legitimate information channel.**
