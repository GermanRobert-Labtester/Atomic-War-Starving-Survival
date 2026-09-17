# C2 — Flagship Integration Plan [28]: Shelter Defense, Visitor/Refugee Intake, and Contested-Hatch Decision Architecture

> **Deliverable:** `C2_planintegration[28].md`
> **Source scope:** Plan 138 — *Shelter Defense & Visitor/Refugee System*
> **Primary objective:** turn the shelter from an inviolate safe box into a contested boundary by adding deterministic threat generation, defense-readiness projection, attack resolution through existing combat/security systems, and a visitor/refugee intake lifecycle that supports screening, temporary stay, permanent admission, trade, diplomacy, detention, expulsion, and hidden-risk discovery without creating parallel survivor, faction, economy, or secret-agenda authorities.
> **Required execution order:** **138A Foundation/System Contract → 138B Defense & Visitor Mechanics/Content → 138C Integration, Save/CI, Balance, and Player-Facing Closure**
> **Hard dependencies:** `AirlockSecuritySystem`, `DutyRosterSystem`, `TacticalCombatSystem`, faction coordination/standing, survivor aggregate/relations, moral choice, economy/trade, quest runtime, Plan 132 hidden-agenda architecture for visitor spies/secret motives, Plan 31 semantic events, Plan 36 ports, Plan 39 save durability, Plan 55 retention.
> **Scope discipline:** no duplicated combat resolution, no duplicate survivor roster, no defense rating as an independently mutable truth, no “spy” secret model parallel to HiddenAgendaSystem, no visitor admission that bypasses canonical survivor onboarding/needs/housing, no attack loot loss outside inventory transactions, no indefinite visitor/history growth, and no unavoidable shelter attack without authored telegraph/response where the design promises preparation.

---

# 0. Executive Intent

ASHFALL currently treats the hatch as a boundary with interaction but not strategic vulnerability.

The project already has:

- airlock security,
- duty-roster roles including hatch defense,
- tactical combat,
- faction relations,
- door encounters,
- survivor social state,
- moral choice,
- economy/trade,
- hidden-agenda work,
- save/load infrastructure.

What is absent is a coherent **shelter-boundary simulation**.

The intended architecture is:

```text
world/faction/reputation conditions
            │
            ▼
   Threat / Visitor Eligibility
            │
            ▼
Shelter Boundary Coordinator
            │
      ┌─────┴───────┐
      ▼             ▼
 ShelterDefense   VisitorSystem
      │             │
      │             ├─ screening
      │             ├─ admission
      │             ├─ trade/envoy
      │             ├─ temporary stay
      │             ├─ refugee integration
      │             └─ hidden-risk handoff
      │
      ├─ readiness projection
      ├─ telegraph/alarm
      ├─ defend/negotiate/surrender/flee
      └─ aftermath
            │
            ▼
 existing authoritative systems
```

The product-level outcome is:

> **The player must decide who gets through the hatch and how much capacity is reserved for defense, while every consequence—injury, theft, morale, faction standing, recruitment, trade, detention, combat damage, and survivor integration—flows through the systems that already own those facts.**

---

# 1. Source Diagnosis

The source establishes:

- `AirlockSecuritySystem` exists but is not a raid/defense system.
- “Hatch Defense” already appears as a roster duty.
- `DoorEncountersSystem` supports visitor encounters but not defense/integration.
- No shelter attack/siege mechanic currently exists.
- No persistent outsider-arrival/refugee-integration system exists.
- Proposed threats include:
  - raiders,
  - faction forces,
  - desperate survivors,
  - stealth infiltration,
  - siege.
- Proposed visitor archetypes include:
  - refugee,
  - trader,
  - envoy,
  - spy.
- Proposed player actions include:
  - admit,
  - turn away,
  - detain,
  - expel,
  - defend,
  - negotiate,
  - surrender,
  - flee.
- 10 threat templates and 15 visitor profiles are expected.
- Old-save compatibility, deterministic generation, and headless CI are required.

The architectural reading is:

```text
shelter defense should project from real shelter/security/roster/combat state
visitor intake should project into real survivor/faction/economy/social systems
```

not:

```text
one giant ShelterDefenseSystem owns everything.
```

---

# 2. Program-Level Success Criteria

C2[28] closes only when:

1. Shelter threats are generated deterministically from canonical world/faction/reputation state.
2. Defense readiness is derived from fortification, garrison, equipment, alarms, and current shelter condition.
3. No independent mutable “defenseRating truth” drifts from its sources.
4. Attacks are telegraphed where appropriate and have explicit approach/attack phases.
5. Combat resolves through `TacticalCombatSystem`.
6. Negotiation resolves through canonical social/faction/skill systems.
7. Surrender transfers actual items/resources transactionally.
8. Flee/abandon behavior uses canonical campaign/location/session semantics or is explicitly gated if unsupported.
9. Visitor arrivals are deterministic and time-gated.
10. Visitor truth and player knowledge are separate.
11. Visitor screening changes knowledge/evidence, not hidden truth.
12. Spies/hidden visitor agendas reuse Plan 132 concepts or adapters rather than a second secret engine.
13. Temporary visitors consume real shelter resources.
14. Permanent admission creates a canonical survivor only through the standard survivor aggregate/onboarding path.
15. Refugee admission can fail honestly when housing/resources/capacity are insufficient.
16. Traders use canonical trade/economy surfaces.
17. Envoys use canonical faction/diplomacy state.
18. Detention/expulsion uses canonical morale/relations/faction consequences.
19. Turned-away visitors do not automatically become violent; escalation is authored/state-driven.
20. Threat/visitor histories are bounded by retention policy.
21. Save/load cannot reroll attack or visitor identity/outcome.
22. Headless CI proves threat and visitor lifecycles.
23. 10 threat templates and 15 visitor profiles are runtime-observed.
24. Attack frequency remains bounded and strategically meaningful.

---

# 3. Architectural Invariants

## 3.1 Defense readiness is derived

It may be cached for UI, but source facts are:

- fortification,
- garrison,
- equipment,
- alarm/readiness,
- current damage,
- power/security availability.

## 3.2 `ShelterDefenseSystem` owns encounter lifecycle, not all combat facts

It owns:

- threat state,
- approach timing,
- defense decision context,
- idempotency,
- aftermath coordination.

It does not own:

- survivor wounds,
- inventory,
- shelter HP,
- faction standing,
- tactical combat.

## 3.3 `VisitorSystem` owns outsider lifecycle, not survivor identity

It owns:

- arrival,
- current visitor state,
- screening/knowledge,
- departure/admission decision,
- temporary stay.

Once permanently admitted:

```text
visitor → canonical survivor
```

and VisitorSystem stops being identity authority.

## 3.4 Visitor secrets reuse the hidden-information layer

No `hiddenAgenda` free-text blob that becomes a second logic system.

Use:

- visitor-specific secret descriptor,
- or Plan 132-compatible agenda definition/knowledge model.

## 3.5 Admission is a resource decision

Housing, food, medicine, disease risk, work capacity, and social pressure matter.

## 3.6 Every attack outcome is transactional

No narrative-only resource loss or damage.

## 3.7 Every threat/visitor is idempotent

No duplicate arrival, attack, loot loss, faction delta, or admission after reload.

## 3.8 Threat frequency is capped

The shelter cannot become permanently interrupt-driven.

## 3.9 Player knowledge is non-omniscient

Visitor type may be unknown until screening/discovery.

## 3.10 UI is projection

Opening a panel never advances threat/visitor state.

---

# 4. Dependency Graph

```text
AirlockSecurity ───────────────┐
DutyRoster / Hatch Defense ────┤
Fortification/Equipment ───────┤
                               ▼
                      138A ShelterDefense
                               │
                        ┌──────┴──────┐
                        ▼             ▼
                  threat lifecycle  visitor lifecycle
                        │             │
                        ▼             ▼
                TacticalCombat    screening/admission
                        │             │
            ┌───────────┼─────────────┼────────────┐
            ▼           ▼             ▼            ▼
         inventory   factions      survivors     economy
            │           │             │            │
            └───────────┴─────────────┴────────────┘
                               │
                               ▼
                         moral/quest/relations
```

Hidden visitor risk:

```text
VisitorSystem
  → visitor knowledge adapter
  → Plan 132 hidden-agenda/secret model
```

---

# 5. Baseline Capture

Before implementation, record:

- current airlock/security inputs,
- current hatch-defense duty API,
- fortification/durability authority,
- weapon/armor equipment APIs,
- tactical-combat entry points,
- survivor removal/admission APIs,
- shelter capacity/housing APIs,
- faction standing/operation APIs,
- trade/economy APIs,
- moral-choice APIs,
- quest APIs,
- current `door_encounters.json` visitor concepts,
- save sections and panel routes.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture one populated shelter with:

- non-zero fortification,
- hatch defenders,
- weapons,
- visitors absent.

---

# 6. Workstream 138A — Foundation / System Contract

## Goal

Create deterministic threat and visitor lifecycle authorities with clean boundaries to existing combat, survivor, faction, and economy systems.

---

# 7. 138A Phase A — Split Definitions From Runtime Instances

Create types:

```text
ThreatDefinition
ThreatInstance
ShelterDefenseState
VisitorProfileDefinition
VisitorInstance
VisitorState
VisitorKnowledgeState
VisitorScreeningRecord
```

Do not store static authored definition content inside every save instance.

---

# 8. 138A Phase B — Threat Vocabulary

Initial threat types:

```text
raider_assault
faction_raid
desperate_group
stealth_infiltration
siege
```

Use typed enum/stable IDs.

---

# 9. 138A Phase C — Threat Lifecycle

Recommended:

```text
Eligible
Approaching
Detected
Preparing
Engaged
Resolved
Recovering
Closed
```

Not every threat needs every phase.

---

# 10. 138A Phase D — Threat Instance Fields

Persist:

```text
instance_id
definition_id
threat_type
strength
origin/faction_id
approach_day
attack_day
demands
detection_state
selected_player_response
resolution_state
applied_consequence_keys
```

---

# 11. 138A Phase E — Defense Readiness Projection

Create:

```text
ShelterDefenseReadModel
```

with calculated:

```text
fortification contribution
garrison contribution
equipment contribution
alarm/preparedness contribution
current shelter-condition penalty
final readiness band
```

The numeric score is derived.

---

# 12. 138A Phase F — Fortification Source

Use canonical shelter fortification/structure authority.

If source plan proposes levels 0–3, map to the existing fortification model or add a single canonical level field there.

Do not create a duplicate fortification level inside defense state if one already exists.

---

# 13. 138A Phase G — Garrison Source

Use DutyRoster “Hatch Defense” assignments.

Defense system queries the roster.

No separate defense roster.

---

# 14. 138A Phase H — Garrison Fitness

Effective contribution may depend on:

- fitness/readiness,
- fatigue,
- equipment,
- injury,
- skill/profession.

Use canonical survivor systems.

---

# 15. 138A Phase I — Equipment Source

Use equipped weapons/armor/condition.

No abstract “equipment quality” mutable field unless it is a computed read-model result.

---

# 16. 138A Phase J — Alarm/Early Warning

Use:

- airlock sensors/security,
- lookout duties,
- power availability,
- faction intel,
- weather/visibility if relevant.

Detection changes preparation window.

---

# 17. 138A Phase K — Threat Eligibility

Inputs may include:

- faction hostility/standing,
- shelter wealth/reputation,
- world events,
- regional security,
- prior defenses,
- cooldown,
- campaign day.

Use deterministic weighted selection.

---

# 18. 138A Phase L — Threat Frequency Budget

Source risk recommends roughly 1–2 attacks/month.

Treat as initial balance band, not hardcoded law.

Define:

```text
minimum cooldown
maximum rolling-window frequency
threat severity budget
```

---

# 19. 138A Phase M — Threat RNG

Use `ISeededRng`.

Stable stream:

```text
shelter_threats
```

Candidate list sorted by template ID.

Same seed/state → same threat.

---

# 20. 138A Phase N — Attack Resolution Boundary

`ShelterDefenseSystem` produces:

```text
DefenseEncounterContext
```

for TacticalCombat.

Combat system returns:

```text
CombatOutcome
```

Defense system then coordinates aftermath.

Do not duplicate hit/damage calculations.

---

# 21. 138A Phase O — Non-Combat Outcomes

Canonical response vocabulary:

```text
Defend
Negotiate
Surrender
Flee
```

Each response has explicit prerequisite/support status.

If campaign cannot support shelter abandonment/fleeing:

```text
defer/disable with documented prerequisite
```

rather than fake success.

---

# 22. 138A Phase P — Negotiation Contract

Negotiation input:

- faction standing,
- negotiator survivor,
- profession/skill,
- threat demands,
- prior history.

Outcome seeded and state-backed.

---

# 23. 138A Phase Q — Surrender Contract

Surrender consequences:

- actual item transfer,
- faction/reputation effects,
- morale effects.

All transactional.

---

# 24. 138A Phase R — Visitor Archetype Vocabulary

Initial types:

```text
refugee
trader
envoy
spy
```

Player may not know the true type immediately.

---

# 25. 138A Phase S — Visitor Truth vs Player Knowledge

Truth:

```text
actual profile/archetype/affiliation/secret
```

Knowledge:

```text
unknown
self-reported
screened
suspected
verified
```

UI reads knowledge, not truth.

---

# 26. 138A Phase T — Visitor Instance

Persist:

```text
visitor_id
profile_id
arrival_day
planned_departure_day
declared_request
actual_affiliation
knowledge state
temporary-stay state
screening records
admission status
idempotency keys
```

---

# 27. 138A Phase U — Visitor Generation

Inputs:

- world events,
- faction relations,
- shelter reputation,
- regional danger,
- campaign day,
- capacity,
- cooldown.

Use seeded deterministic selection.

---

# 28. 138A Phase V — Visitor Frequency Budget

Source proposes 1–3/week.

Treat as an upper-content target, not automatic spawn frequency.

Define:

```text
arrival opportunity cadence
active visitor cap
group size cap
```

Prevent constant interruption.

---

# 29. 138A Phase W — Visitor Group Model

Support:

- single visitor,
- small refugee group,
- envoy/trader party.

Group membership is explicit.

Permanent admission converts individual members through survivor onboarding.

---

# 30. 138A Phase X — Visitor Screening Model

Screening methods:

```text
interview
background_check
observation
trust_building
medical_screening
```

Each:

- costs time/labor,
- yields evidence/knowledge,
- may have reliability.

---

# 31. 138A Phase Y — Hidden Agenda Integration

For spies/thieves:

- adapt Plan 132 hidden-agenda/knowledge model,
- or define visitor-secret descriptors compatible with its clue/evidence model.

Do not create visitor-only secret logic that diverges.

---

# 32. 138A Phase Z — Admission Status Vocabulary

```text
waiting
temporary
trader_access
detained
admitted_permanent
turned_away
expelled
departed
escaped
```

---

# 33. 138A Phase AA — Save State

Persist only dynamic state.

Static threat/visitor definitions remain in catalogs.

---

# 34. 138A Phase AB — Old Save Compatibility

Missing states:

```text
default defense lifecycle empty
visitor state empty
```

Derived defense readiness recalculates from existing shelter/roster/equipment state.

---

# 35. 138A Phase AC — Port Contract

Required sinks:

```text
combat
inventory
shelter damage
survivor admission/removal
relations
factions
moral choice
economy/trade
quest runtime
housing/needs
disease/quarantine if used
```

Missing required sink fails validation.

---

# 36. 138A Phase AD — Semantic Events

Candidate events:

```text
threat_approaching
shelter_attack_started
shelter_attack_resolved
visitor_arrived
visitor_screening_completed
visitor_admitted
visitor_turned_away
visitor_detained
visitor_secret_discovered
refugee_group_admitted
siege_started
siege_ended
```

Use Plan 31 vocabulary governance.

---

# 37. 138A Phase AE — Retention

Threat/visitor history:

- recent detailed events,
- landmark attacks/admissions,
- summarized older visits.

Use Plan 55.

---

# 38. 138A Tests

- derived readiness,
- deterministic threat selection,
- threat cooldown,
- visitor generation,
- active visitor cap,
- old-save defaults,
- truth-vs-knowledge,
- screening outcome,
- missing port failure,
- save round-trip,
- idempotency.

---

# 39. 138A Definition of Done

- [ ] defense/visitor definition-instance split,
- [ ] threat lifecycle,
- [ ] readiness projection,
- [ ] fortification authority reused,
- [ ] duty-roster garrison reused,
- [ ] equipment authority reused,
- [ ] deterministic threat generation,
- [ ] attack cooldown/budget,
- [ ] TacticalCombat boundary,
- [ ] negotiation/surrender contracts,
- [ ] visitor lifecycle,
- [ ] truth-vs-knowledge,
- [ ] deterministic visitor generation,
- [ ] screening model,
- [ ] hidden-agenda adapter,
- [ ] save state,
- [ ] old-save support,
- [ ] ports,
- [ ] events,
- [ ] retention policy.

---

# 40. Workstream 138B — Defense / Visitor Mechanics & Content

## Goal

Implement 10 threat templates and 15 visitor profiles that create preparation, admission, screening, defense, diplomacy, trade, and integration decisions.

---

# 41. 138B Phase A — `shelter_defense.json`

Create data authority for threat templates.

Fields:

```text
id
threat_type
eligibility
strength band
telegraph/detection rules
demands
combat profile
negotiation profile
aftermath descriptors
cooldown
quest hooks
localization keys
```

---

# 42. 138B Phase B — `refugee_profiles.json`

Create visitor/profile authority.

Fields:

```text
id
true archetype
declared archetype/request
faction
group-size rule
skills/profession
resource burden
health/quarantine flags
secret descriptor
screening clues
admission outcomes
trade/envoy payload
localization keys
```

---

# 43. 138B Phase C — Template Integrity

Validate:

- unique IDs,
- faction refs,
- item refs,
- survivor profession/skill refs,
- quest refs,
- hidden-secret refs,
- localization keys,
- cooldowns,
- group-size ranges,
- threat-strength ranges.

---

# 44. 138B Phase D — Fortification Upgrades

Player preparation action:

```text
resources + labor
→ canonical fortification upgrade
```

Defense view reflects new readiness.

No upgrade state in defense system if shelter structure owns it.

---

# 45. 138B Phase E — Garrison Assignment

Use normal duty assignment.

UI preview:

- assigned defenders,
- readiness impact,
- lost labor elsewhere,
- fatigue/injury warnings.

---

# 46. 138B Phase F — Defensive Positions

Barricades/traps/chokepoints should use existing item/workshop/build systems.

If no canonical buildable-trap support exists:

```text
gate as prerequisite
```

Do not store imaginary defense bonuses disconnected from assets/resources.

---

# 47. 138B Phase G — Raider Assault

Template family:

- supply demand,
- fight,
- intimidation/negotiation.

Outcome routed through combat/inventory.

---

# 48. 138B Phase H — Faction Raid

Eligibility tied to actual faction state.

Possible:

- surrender demand,
- search,
- arrest/hostage demand,
- assault.

No random hostile faction with neutral standing unless world event explains it.

---

# 49. 138B Phase I — Desperate Group

Important fairness rule:

```text
turned away != automatically violent
```

Violence eligibility may depend on:

- starvation,
- desperation,
- group state,
- prior treatment,
- world conditions.

---

# 50. 138B Phase J — Stealth Infiltration

This should integrate with visitor/spies and Plan 132.

Possible path:

```text
visitor admitted
→ secret objective progresses
→ theft/intel clue
→ discovery
```

or external infiltrator threat.

Do not duplicate hidden-secret mechanics.

---

# 51. 138B Phase K — Siege

Treat siege as a multi-day threat state.

Effects may include:

- blocked trade/expeditions,
- morale pressure,
- resource scarcity,
- negotiation windows.

Do not create direct starvation math in defense code.

---

# 52. 138B Phase L — Attack Preparation Window

Detected threat grants:

- fortification work,
- garrison assignment,
- equipment check,
- medical readiness,
- negotiation prep.

Unaware/surprised attack may reduce readiness.

---

# 53. 138B Phase M — Combat Resolution

Tactical combat receives shelter-specific modifiers:

- chokepoint,
- fortification,
- defender count,
- alarm/preparedness.

No separate combat dice.

---

# 54. 138B Phase N — Breach Outcome

Breached means:

- canonical shelter damage,
- inventory loss,
- injuries/captures if supported,
- morale/relations impact.

Every effect applied once.

---

# 55. 138B Phase O — Overrun Outcome

High-severity.

If capture/temporary displacement is not supported by current campaign architecture:

```text
defer exact branch
```

Do not fabricate unsupported persistent states.

---

# 56. 138B Phase P — Aftermath

After attack:

- repairs,
- treatment,
- casualty/memorial,
- morale,
- faction response,
- journal.

Use existing systems.

---

# 57. 138B Phase Q — Refugee Arrival

Groups:

```text
2–5
```

as source baseline.

Before arrival/admission show:

- food burden,
- housing burden,
- health/quarantine risk,
- known skills,
- unknowns.

---

# 58. 138B Phase R — Temporary Shelter

Visitor stay:

```text
1–7 days
```

data-authored.

During stay, actual resources consumed.

No fake “visitor cost” counter.

---

# 59. 138B Phase S — Permanent Admission

Preconditions:

- capacity/housing,
- policy/moral choice,
- group/member eligibility.

Flow:

```text
visitor profile
→ survivor creation/migration adapter
→ canonical survivor aggregate
→ duty/needs/relations registration
```

---

# 60. 138B Phase T — Refugee Integration

After admission:

- assign housing,
- assign work,
- join relations/social systems,
- persist identity,
- remove visitor ownership.

No dual identity.

---

# 61. 138B Phase U — Trader Access

Trader remains outsider.

Trade happens through designated trade interface.

No unnecessary full-shelter access.

---

# 62. 138B Phase V — Envoy

Envoy visit routes into faction/diplomacy event.

Standing changes only through canonical faction system.

---

# 63. 138B Phase W — Spy Visitor

Spy may declare another role.

Screening/observation produces clues.

Actual theft/intel leak uses existing inventory/faction/intel systems.

---

# 64. 138B Phase X — Detention

If detention/prison state exists:

- reuse it.

If not:

- implement minimal canonical detained-visitor state,
- define resource/security/morale consequences,
- do not silently equate to survivor imprisonment.

---

# 65. 138B Phase Y — Visitor Screening

Methods source-inspired:

- interrogation/interview,
- background research,
- observation,
- trust building.

Add medical screening for disease risk if supported.

---

# 66. 138B Phase Z — Screening Fairness

Screening result should show confidence/evidence band.

Never:

```text
Spy: yes/no
```

until confirmed.

---

# 67. 138B Phase AA — Visitor Event Set

Initial source concepts:

```text
The Informant
The Sick Refugee
The Skilled Worker
The Spy
The Dying Stranger
```

Implement as profile/event templates using canonical systems.

---

# 68. 138B Phase AB — Disease/Quarantine

“The Sick Refugee” uses actual disease/quarantine system.

No visitor-only sickness variable.

---

# 69. 138B Phase AC — Skilled Worker

If permanently admitted:

- profession/skills authored in profile,
- survivor aggregate receives them,
- no arbitrary bonus outside survivor systems.

---

# 70. 138B Phase AD — Informant

Information reward uses canonical intel/faction/world-knowledge systems.

---

# 71. 138B Phase AE — Dying Stranger

Medical aid choice uses actual medical inventory/care system.

Moral consequence reflects actual choice/outcome.

---

# 72. 138B Phase AF — Shelter Status UI

Show:

```text
fortification
current garrison
derived readiness band
detected threats
preparation window
current visitors
capacity pressure
```

Do not show hidden threat RNG or visitor truth.

---

# 73. 138B Phase AG — Visitor Log

Show player-known history:

- arrival,
- declared request,
- screening findings,
- admission/turn-away,
- departure,
- discovered secret.

No hidden truth before discovery.

---

# 74. 138B Phase AH — Journal

Significant:

- attacks,
- refugee admissions,
- siege,
- spy discovery,
- major departures.

Avoid logging every screening tick.

---

# 75. 138B Phase AI — Quest Hooks

Source concepts:

```text
The Siege
The Refugee Crisis
The Spy Hunt
The Raid
```

Use canonical quest lifecycle.

---

# 76. 138B Phase AJ — 10 Threat Templates

Balanced across:

- raider,
- faction,
- desperate group,
- infiltration,
- siege.

Avoid 10 reskins.

---

# 77. 138B Phase AK — 15 Visitor Profiles

Balanced across:

- refugee,
- trader,
- envoy,
- spy/hidden risk.

Include varied:

- skills,
- burdens,
- affiliations,
- requests,
- health states.

---

# 78. 138B Phase AL — Content Utilization

100/200-day seeded runs.

Report:

```text
threat templates eligible/triggered/resolved
visitor profiles eligible/arrived/resolved
admissions
turn-aways
trades
envoy outcomes
spy discoveries
```

---

# 79. 138B Phase AM — Dead Content Policy

Any never-reached template:

- fix,
- mark rare,
- remove,
- exempt with reason.

---

# 80. 138B Definition of Done

- [ ] shelter_defense.json,
- [ ] refugee_profiles.json,
- [ ] 10 threat templates,
- [ ] 15 visitor profiles,
- [ ] fortification prep,
- [ ] garrison prep,
- [ ] equipment readiness,
- [ ] alarm/telegraph,
- [ ] raider/faction/desperate/infiltration/siege,
- [ ] combat/noncombat responses,
- [ ] temporary visitors,
- [ ] permanent admission,
- [ ] refugee integration,
- [ ] traders,
- [ ] envoys,
- [ ] spies,
- [ ] screening,
- [ ] detention/expulsion,
- [ ] five visitor-event concepts,
- [ ] status UI,
- [ ] visitor log,
- [ ] journal,
- [ ] quest hooks,
- [ ] utilization report.

---

# 81. Workstream 138C — Integration / Consequences / Validation

## Goal

Prove defense and visitor outcomes use canonical authorities, survive save/load, remain deterministic, and stay strategically bounded rather than becoming interruption spam.

---

# 82. 138C Phase A — TacticalCombat Integration

Defense context supplies:

- defenders,
- equipment,
- shelter modifiers,
- threat participants.

Combat outcome returns canonical casualty/damage results.

---

# 83. 138C Phase B — Faction Integration

Attacks/negotiation/envoys affect standing through faction coordinator.

Use cause IDs.

No direct standing mutation in UI.

---

# 84. 138C Phase C — Relation Integration

Defense cooperation may create bonds/history.

Refugee admission/turn-away/detention may affect survivor morale/relations.

Use existing relation/morale APIs.

---

# 85. 138C Phase D — Moral Choice Integration

Refugee decisions:

- admit,
- refuse,
- detain,
- exploit,
- aid.

Use canonical moral-choice semantics.

Do not assume every refusal is immoral; context matters.

---

# 86. 138C Phase E — Economy Integration

Traders affect market/trade through real systems.

No visitor-specific shadow prices.

---

# 87. 138C Phase F — Duty Roster Integration

Hatch-defense assignments are normal duty assignments.

Attack preparation reads current assignments.

---

# 88. 138C Phase G — Housing/Needs Integration

Admitted refugees immediately become subject to:

- shelter capacity,
- food,
- sleep,
- medical,
- morale.

No grace ghost-survivor state unless explicitly designed.

---

# 89. 138C Phase H — Disease/Contamination Integration

Visitors may require quarantine.

Use canonical disease/contamination systems.

---

# 90. 138C Phase I — Survivor Conversion Test

Permanent visitor admission:

```text
visitor removed from VisitorState
survivor created once
all systems register new survivor
save/load preserves identity
```

No duplicate on reload.

---

# 91. 138C Phase J — Visitor Departure

Temporary visitor departure:

- clears resource burden,
- archives summary,
- cleans screening/detention state,
- applies departure consequences once.

---

# 92. 138C Phase K — Threat Resolution Idempotency

Save/load around:

- approach,
- decision,
- combat,
- aftermath.

No duplicate:

- raid,
- loot loss,
- damage,
- faction delta.

---

# 93. 138C Phase L — Visitor Idempotency

Save/load around:

- arrival,
- screening,
- admission,
- departure.

No duplicate visitor or survivor conversion.

---

# 94. 138C Phase M — Attack Cooldowns

Persist:

- last attack day,
- rolling threat budget,
- per-template cooldown.

No reload farming.

---

# 95. 138C Phase N — Visitor Time Gates

Arrival/departure schedule persists.

No refresh-to-reroll visitor.

---

# 96. 138C Phase O — No-Garrison Edge Case

Source requests defense=0.

Better rule:

```text
garrison contribution = 0
```

Total defense may still include fortification/alarm/passive defenses.

Do not force total rating to zero if real shelter structure still exists.

Test both.

---

# 97. 138C Phase P — All Visitors Turned Away

Long-run scenario:

- visitor system continues functioning,
- no integration,
- reputation/moral consequences where authored,
- not every group becomes hostile.

---

# 98. 138C Phase Q — Full-Capacity Shelter

Admission blocked or warned by real capacity constraint.

Player may still choose emergency overcrowding if canonical shelter systems support it.

---

# 99. 138C Phase R — All Defenders Injured

Readiness reflects actual fitness.

No stale garrison score.

---

# 100. 138C Phase S — Power-Outage Attack

Alarm/security penalties read canonical power.

Attack still resolves.

---

# 101. 138C Phase T — Siege Save/Load

Multi-day siege:

- persists,
- modifiers remain once,
- end/recovery clears temporary effects.

---

# 102. 138C Phase U — Visitor Spy / Plan 132 Integration

Visitor spy clue/discovery state must be compatible with hidden-agenda knowledge architecture.

After permanent admission, if a hidden agenda persists:

```text
visitor secret
→ survivor HiddenAgendaSystem instance
```

No duplicate.

---

# 103. 138C Phase V — Quest Integration

Quest windows:

- raid,
- siege,
- refugee crisis,
- spy hunt.

No duplicate quest unlock after reload.

---

# 104. 138C Phase W — Epilogue / Legacy

Landmark facts:

- shelter overrun,
- famous defense,
- refugee haven,
- mass refusal,
- siege survived,
- spy infiltration.

Write through canonical completion record.

---

# 105. 138C Phase X — UI Accessibility

Shelter Status/Visitor Log:

- keyboard,
- controller if supported,
- text labels,
- no color-only risk,
- explicit capacity/resource warnings.

---

# 106. 138C Phase Y — Attention Budget

Threats/visitors are not constant modals.

Use:

- briefing,
- alert,
- routeable panel.

Cap simultaneous unresolved high-attention shelter-boundary events.

---

# 107. 138C Phase Z — Headless Behavior

All:

- threat generation,
- approach,
- visitor arrivals,
- screening timers,
- departure,
- siege phases

tick without UI.

---

# 108. 138C Phase AA — `--shelter-defense-selftest`

Required scenarios:

1. threat generated deterministically,
2. detected attack with preparation,
3. no-garrison scenario,
4. surrender item transfer,
5. tactical defense resolution,
6. visitor arrival,
7. screening,
8. permanent admission,
9. spy discovery,
10. old save,
11. save/load idempotency,
12. cooldown/time-gate.

---

# 109. 138C Phase AB — Catalog Integrity

Validate:

- threat/profile IDs,
- factions,
- items,
- skills/professions,
- quests,
- secret refs,
- localization keys,
- group-size bounds,
- cooldowns.

---

# 110. 138C Phase AC — Deliberate Failure Proof

Break:

- faction ID,
- survivor-conversion port,
- duplicate aftermath application,
- threat cooldown.

Assert tests fail.

---

# 111. 138C Phase AD — 200-Day Threat/Visitor Soak

Record:

```text
threats eligible
attacks triggered
attacks resolved
visitors arrived
visitors admitted
visitors turned away
visitors detained
spy discoveries
sieges
overruns
```

---

# 112. 138C Phase AE — Balance Profiles

Run:

```text
fortify_heavily
diplomatic
minimal_defense
open_door
closed_door
```

Compare:

- casualties,
- resource cost,
- morale,
- standing,
- population growth,
- attack frequency.

No policy should dominate every axis.

---

# 113. 138C Phase AF — Attack Frequency Guard

Source risk:

```text
~1–2 attacks/month
```

Measure actual rolling frequency and severity.

Tune by world state.

---

# 114. 138C Phase AG — Visitor Burden Guard

Visitor frequency/resource cost must not turn normal play into constant intake administration.

Measure:

- arrivals/week,
- player interactions,
- resource burden,
- panel interruptions.

---

# 115. 138C Phase AH — False-Security Guard

Fortress shelter should reduce risk but not make all diplomacy irrelevant.

Use diminishing returns/rare high-tier threats only if design supports.

---

# 116. 138C Phase AI — Fairness Playtests

Scenarios:

1. warned raider attack,
2. surprise infiltration,
3. refugee group at low capacity,
4. trader/envoy,
5. admitted spy,
6. siege.

Assess:

```text
telegraph
choice clarity
consequence legitimacy
workload
```

---

# 117. 138C Phase AJ — Documentation

Create:

```text
docs/systems/SHELTER_DEFENSE_AND_VISITORS.md
```

Include:

- authority boundaries,
- readiness derivation,
- threat lifecycle,
- visitor lifecycle,
- screening,
- survivor conversion,
- secret integration,
- save contract,
- adding templates/profiles.

---

# 118. 138C Definition of Done

- [ ] TacticalCombat integration,
- [ ] faction integration,
- [ ] relations/morale integration,
- [ ] moral-choice integration,
- [ ] economy integration,
- [ ] duty-roster integration,
- [ ] housing/needs integration,
- [ ] disease/quarantine integration,
- [ ] visitor→survivor conversion,
- [ ] visitor departure cleanup,
- [ ] threat/visitor idempotency,
- [ ] cooldown/time-gate,
- [ ] no-garrison edge case,
- [ ] all-turn-away edge case,
- [ ] full-capacity edge case,
- [ ] power-outage edge case,
- [ ] siege resume,
- [ ] Plan 132 secret handoff,
- [ ] quest/legacy integration,
- [ ] accessibility,
- [ ] attention budget,
- [ ] headless selftest,
- [ ] deliberate failure proof,
- [ ] 200-day soak,
- [ ] strategy profiles,
- [ ] attack-frequency guard,
- [ ] visitor-burden guard,
- [ ] fairness playtests,
- [ ] docs.

---

# 119. Integrated Shelter Boundary Pipeline

```text
world/faction/reputation state
          │
          ├─────────────► threat eligibility
          │
          └─────────────► visitor eligibility
                              │
                 ┌────────────┴────────────┐
                 ▼                         ▼
           ShelterDefense             VisitorSystem
                 │                         │
         derived readiness          truth / knowledge
                 │                         │
       approach / preparation        screen / admit
                 │                         │
                 ▼                         ▼
        combat/noncombat         temp stay / trade /
             resolution          envoy / integration
                 │                         │
                 └────────────┬────────────┘
                              ▼
                  canonical downstream owners
```

---

# 120. Defense Readiness Contract

Readiness is a function:

```text
fortification
+ active defenders
+ defender readiness
+ equipment
+ alarm/preparation
+ shelter condition
```

It is not independently edited.

---

# 121. Threat Truth Contract

Threat strength/type/timing are runtime truth.

Player sees only what detection/intel reveals.

---

# 122. Telegraph Contract

Where an attack is intended to be preparable:

- detection opportunity exists,
- warning lead time is state-backed,
- preparation changes outcome odds.

Stealth infiltration may intentionally reduce telegraph.

---

# 123. Combat Contract

Defense system never reimplements combat.

It supplies context and receives outcome.

---

# 124. Resource-Loss Contract

Surrender/raid theft:

```text
actual inventory transaction
```

No abstract “lost supplies” number.

---

# 125. Visitor Truth vs Knowledge Contract

The system can know:

```text
actual archetype/affiliation/secret
```

The player sees:

```text
declared request + discovered evidence
```

---

# 126. Screening Contract

Screening changes:

```text
knowledge confidence
```

not actual visitor truth.

---

# 127. Permanent Admission Contract

Admission transfers authority:

```text
VisitorSystem
→ survivor aggregate
```

After transfer, VisitorSystem keeps only historical reference.

---

# 128. Hidden-Agenda Contract

Visitor spies/secret motives must be compatible with Plan 132.

No duplicate hidden-motive framework.

---

# 129. Trader Contract

Trader remains an outsider entity.

Market/trade system owns prices and transactions.

---

# 130. Envoy Contract

Faction system owns diplomatic consequences.

---

# 131. Detention Contract

Detention has explicit:

- duration,
- security/resource cost,
- morale/faction consequences,
- release/expel path.

No indefinite forgotten detainee.

---

# 132. Siege Contract

Siege is a timed multi-day threat wrapper.

It may modify access/supply via canonical systems.

It does not own starvation/economy directly.

---

# 133. Save Contract

Persist:

- active threats,
- threat phase/timing,
- applied consequences,
- current visitors,
- visitor knowledge/screening,
- admission/departure state,
- cooldowns.

Static templates remain data.

---

# 134. Old-Save Contract

Missing defense/visitor state:

```text
valid
```

Derived readiness rebuilds from existing shelter state.

---

# 135. Idempotency Contract

No duplicate:

- combat,
- damage,
- theft,
- standing delta,
- visitor,
- survivor conversion.

---

# 136. Retention Contract

Recent visitor/attack detail can roll up.

Preserve:

- landmark siege,
- major overrun,
- refugee-haven identity,
- key spy event,
- completion-record facts.

---

# 137. Frequency Contract

Shelter-boundary systems have rolling cadence limits.

They should add tension, not dominate all daily play.

---

# 138. Attention Contract

At most a small number of unresolved high-attention shelter-boundary events at once.

Use briefing/panel queues rather than modal spam.

---

# 139. Fairness Contract

Defense outcomes should reflect:

```text
preparation
world/faction risk
actual defenders
real equipment
player response
```

not opaque dice alone.

---

# 140. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| defense rating drifts from real state | Medium | High | projection-only read model |
| too many raids interrupt play | High | High | rolling threat budget |
| visitor intake becomes admin spam | High | Medium | arrival cap + batch UI |
| permanent admission duplicates survivor | Medium | Critical | one conversion transaction |
| spy system duplicates Plan 132 | Medium | High | shared hidden-information adapter |
| attack save/load doubles loot loss | Medium | High | idempotency keys |
| overrun requires unsupported capture system | Medium | High | explicit dependency gate |
| flee branch unsupported | Medium | Medium | defer, do not fake |
| turn-away always causes violence | Medium | High | state-driven escalation |
| detention becomes indefinite orphan state | Medium | Medium | duration/exit contract |
| traders bypass economy | Medium | Medium | canonical trade API |
| shelter becomes impossible to defend | Medium | High | balance profiles + telegraph |

---

# 141. Commit Strategy

## 138A — Foundation

### C2[28].1 — baseline + shelter-boundary ADR

### C2[28].2 — threat/visitor definition-instance DTOs

### C2[28].3 — derived defense readiness

### C2[28].4 — deterministic threat generation/cooldowns

### C2[28].5 — attack lifecycle/combat boundary

### C2[28].6 — visitor lifecycle/truth-vs-knowledge

### C2[28].7 — screening/hidden-agenda adapter

### C2[28].8 — save/old-save/idempotency

### C2[28].9 — port contract/events/retention

### Gate: 138A complete

---

## 138B — Mechanics / Content

### C2[28].10 — shelter_defense.json

### C2[28].11 — refugee_profiles.json

### C2[28].12 — fortification/garrison/preparation

### C2[28].13 — raider/faction/desperate threats

### C2[28].14 — infiltration/siege

### C2[28].15 — temporary/permanent visitors

### C2[28].16 — traders/envoys/spies/detention

### C2[28].17 — visitor events/quests/UI

### C2[28].18 — utilization reports

### Gate: 138B complete

---

## 138C — Closure

### C2[28].19 — combat/faction/relations/morality

### C2[28].20 — economy/duty/housing/disease

### C2[28].21 — visitor→survivor conversion

### C2[28].22 — save/load/exploit matrix

### C2[28].23 — siege/power/full-capacity edge cases

### C2[28].24 — Plan 132 secret handoff + quests/legacy

### C2[28].25 — selftest/integrity/failure proof

### C2[28].26 — 200-day soak + strategy profiles

### C2[28].27 — frequency/burden/fairness playtests

### C2[28].28 — docs/release closure

### Gate: 138C complete

---

# 142. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-defense-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
threat/visitor content-utilization report
old-save fixture load
same-seed threat/visitor replay
save/load idempotency matrix
200-day shelter-boundary soak
Shelter Status / Visitor Log snapshot-accessibility gate
```

---

# 143. Flagship Definition of Done

## 138A — Foundation

- [ ] ShelterDefenseSystem,
- [ ] VisitorSystem,
- [ ] typed threat lifecycle,
- [ ] typed visitor lifecycle,
- [ ] derived readiness,
- [ ] canonical fortification/garrison/equipment sources,
- [ ] deterministic generation,
- [ ] attack frequency budget,
- [ ] TacticalCombat boundary,
- [ ] negotiation/surrender contracts,
- [ ] visitor truth-vs-knowledge,
- [ ] screening,
- [ ] Plan 132-compatible visitor secrets,
- [ ] save/old-save,
- [ ] idempotency,
- [ ] ports,
- [ ] semantic events,
- [ ] retention.

## 138B — Mechanics / Content

- [ ] fortification preparation,
- [ ] garrison assignment,
- [ ] alarm/detection,
- [ ] raider assault,
- [ ] faction raid,
- [ ] desperate group,
- [ ] stealth infiltration,
- [ ] siege,
- [ ] defend/negotiate/surrender,
- [ ] flee only if supported,
- [ ] temporary visitors,
- [ ] permanent admission,
- [ ] refugee integration,
- [ ] trader access,
- [ ] envoys,
- [ ] spies,
- [ ] detention/expulsion,
- [ ] visitor events,
- [ ] status UI,
- [ ] visitor log,
- [ ] journal,
- [ ] quest hooks,
- [ ] 10 threats,
- [ ] 15 visitor profiles,
- [ ] utilization.

## 138C — Closure

- [ ] combat integration,
- [ ] faction integration,
- [ ] relation/morale integration,
- [ ] moral-choice integration,
- [ ] economy integration,
- [ ] duty integration,
- [ ] housing/needs integration,
- [ ] disease/quarantine integration,
- [ ] visitor→survivor conversion,
- [ ] departure cleanup,
- [ ] attack/visitor save-load idempotency,
- [ ] cooldown/time-gating,
- [ ] no-garrison edge case,
- [ ] all-turn-away edge case,
- [ ] full-capacity edge case,
- [ ] power-outage edge case,
- [ ] siege resume,
- [ ] hidden-agenda handoff,
- [ ] quest/epilogue integration,
- [ ] accessibility,
- [ ] headless selftest,
- [ ] deliberate failure proof,
- [ ] 200-day soak,
- [ ] strategy profiles,
- [ ] attack-frequency guard,
- [ ] visitor-burden guard,
- [ ] fairness playtests,
- [ ] docs.

## Global

- [ ] no duplicate defense truth,
- [ ] no duplicate survivor identity,
- [ ] no duplicate secret-motive framework,
- [ ] no narrative-only loot/damage,
- [ ] no save/load reroll,
- [ ] no constant attack spam,
- [ ] full verification green.

---

# 144. Closure Report Template

```markdown
## C2[28] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Fortification source:
- Hatch-defense role:
- Combat entry:
- Current visitor encounters:
- Shelter capacity source:
- Faction sink:
- Trade sink:

### 138A — Foundation
- Defense readiness:
- Threat types:
- Threat cadence:
- Visitor archetypes:
- Visitor knowledge states:
- Screening:
- Hidden-agenda adapter:
- Save schema:
- Old save:
- Missing ports:
- Result:

### 138B — Content
- Threat templates:
- Visitor profiles:
- Raider:
- Faction raid:
- Desperate group:
- Infiltration:
- Siege:
- Refugees:
- Traders:
- Envoys:
- Spies:
- Detention:
- UI:
- Quests:
- Unused content:
- Result:

### 138C — Integration
- Combat:
- Factions:
- Relations:
- Morality:
- Economy:
- Duty roster:
- Housing/needs:
- Disease/quarantine:
- Visitor conversion:
- Save/load:
- Idempotency:
- Edge cases:
- Hidden agenda handoff:
- 200-day soak:
- Strategy balance:
- Playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Shelter-defense selftest:
- Port contract:
- Old-save fixtures:
- Same-seed replay:
- Content utilization:
- Verify fast:

### Final Metrics
- THREAT_TEMPLATES:
- VISITOR_PROFILES:
- THREATS_TRIGGERED:
- ATTACKS_RESOLVED:
- VISITORS_ARRIVED:
- PERMANENT_ADMISSIONS:
- VISITORS_TURNED_AWAY:
- SPY_DISCOVERIES:
- SIEGES:
- DUPLICATE_ATTACK_EFFECTS:
- DUPLICATE_SURVIVOR_CONVERSIONS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Defense content:
- Visitor content:
- Flee/capture support:
- Detention:
- Reputation:
- UI:
```

---

# 145. Final Execution Directive

Execute Plan 138 as a **contested-boundary and outsider-intake layer over existing shelter, combat, survivor, faction, economy, and hidden-information authorities**.

The critical sequence is:

```text
derive defense readiness from real shelter/roster/equipment state
→ generate threats deterministically
→ telegraph and prepare
→ resolve through combat/negotiation/surrender
→ generate visitors separately from threats
→ screen without revealing hidden truth
→ support temporary stay/trade/envoy/refugee decisions
→ convert permanent admissions exactly once into canonical survivors
→ reuse Plan 132 for spy/secret arcs
→ persist/cooldown everything
→ prove fairness and bounded frequency in long runs
```

Do not create a second combat system.

Do not create a second survivor roster.

Do not store a mutable defense score that can disagree with actual fortification and garrison state.

Do not let a trader bypass the economy.

Do not let a visitor spy bypass the hidden-agenda evidence model.

Do not let turning away refugees automatically imply violence.

The strongest defense rule is:

> **Shelter defense strength is a projection of the shelter and people the player actually prepared, not an independent stat.**

The strongest visitor rule is:

> **An outsider remains a visitor until the canonical survivor-admission transaction succeeds; after that, survivor systems own them.**

The strongest secrecy rule is:

> **The system may know who a visitor really is, but the player sees only the evidence earned through screening, observation, and events.**

The flagship acceptance scenario is:

> **Start a seeded campaign with moderate fortification, one hatch defender, and limited spare capacity. Detect an approaching raider threat, assign a second defender, resolve the assault through TacticalCombat, and verify inventory/shelter/faction aftermath exactly once across save/load. Later receive a three-person refugee group containing one hidden spy profile, screen them with incomplete evidence, temporarily admit the group, permanently integrate one skilled refugee into the canonical survivor aggregate, discover the spy through state-backed clues, and expel them. Re-run with the same seed and actions: threat timing, visitor arrivals, screening outcomes, combat context, survivor conversion, and consequences must reproduce without duplicate state or omniscient UI.**
