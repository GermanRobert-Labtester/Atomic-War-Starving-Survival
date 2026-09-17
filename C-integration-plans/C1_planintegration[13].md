# C1 — Flagship Integration Plan [13]: Governing Together — Leadership, Policy, Consent & Crew Refusal

> **Output:** `C1_planintegration[13].md`
>
> **Source baseline:** Plan 43 — Governing Together: The Shelter Decides
>
> **Wave:** Continuity Wave 6 — *The People In It*
>
> **Depends on:** Plan 40A identity/beliefs; Plan 24A/24B fitness + needs modifier stack; Plan 31 semantic event vocabulary; Plan 38C commitments/deadlines; Plan 41B standing records; Plan 42C voice/grievance presentation; Plan 35C labour; Plan 19A derived ending context; Plan 36A port-contract enforcement.
>
> **Mandatory execution order:** 43A → 43B → 43C.
>
> **Critical stop condition:** 43C may legitimately stop and ship a gate document instead of a mutiny/escalation mechanic if leadership, policy, grievance, standing-record, and labour state are not yet persisted, readable, and causally connected.
>
> **Primary architectural rule:** do not add a political-simulation subsystem. This plan wires existing leadership, ration, schedule, register, arbitration, standing, friction, grievance, fate, needs, and commitment authorities into one governance substrate.
>
> **Primary gameplay rule:** governance must be legible before it becomes punitive. The player must know who leads, what policy is active, who objects, why they object, and what off-ramps exist before refusal or departure can occur.
>
> **Guardrails:** no approval-rating meter; no hidden loyalty stat; no shelter-internal faction simulator; no election/voting UI without a grievance substrate; no cinematic coup/QTE; no new fake console; no modal policy spam; no invented punishment for information the player never received; no second morale channel; no hardcoded internal political blocs.

---

# 0. Mission

ASHFALL already models most of the mechanics required for governance:

- `LeadershipSystem`;
- leader designation;
- step-down;
- leader stress;
- death/injury/crisis stress;
- break risk;
- ration policy;
- curfew;
- emergency override;
- survivor needs;
- ideological friction;
- ration conflict;
- grievance/morale marks;
- voluntary registration;
- census claims;
- arbitration;
- treaties;
- standing records;
- duty roster;
- survivor fate/desertion;
- commitments/deadlines;
- policy-relevant ending inputs.

The continuity failure is not absence of political state.

The failure is that these systems do not form a player-visible decision loop.

The source baseline describes the current shape:

```text
LeadershipSystem
  has DesignateLeader / StepDown / stress / break risk
        │
        └── no real runtime designation or crisis calls

RationPolicy
  choice → grievance → morale
        │
        └── real and working

Curfew / EmergencyOverride
  panel toggles → schedule state
        │
        └── real state, but not a shared policy mechanism

VoluntaryRegister / Census / Arbitration / Treaties
        │
        └── separate slices, no governance surface

IdeologicalFriction
        │
        └── data and simulation, weak/no player voice
```

The target is:

```text
SURVIVOR IDENTITY / BELIEFS
            │
            ▼
      LEADERSHIP SYSTEM
            │
            ├── designation
            ├── stress
            ├── break risk
            └── succession
            │
            ▼
        POLICY SYSTEM
            │
            ├── propose
            ├── decide
            ├── apply
            ├── grievance
            └── record
            │
            ▼
       CREW RESPONSE
            │
            ├── acceptance
            ├── objection
            ├── refusal
            ├── arbitration
            ├── challenge
            └── departure
            │
            ▼
      DUTY / NEEDS / FATE
            │
            ▼
BRIEFING / JOURNAL / STANDING RECORD / ENDING
```

Governance should feel like survival-management, not a politics minigame.

The player makes decisions already present in the shelter:
- who leads;
- how food is rationed;
- whether curfew is imposed;
- whether emergency override is active;
- whether mourning time is granted;
- who is counted;
- who is bound to duty;
- whether a promise is honored;
- whether a grievance is heard;
- whether someone is allowed to leave.

The game then exposes the consequences through the same channels already used for fatigue, morale, duty, faction standing, and fate.

---

# 1. Source-Evidence Interpretation

## 1.1 Leadership is modeled but runtime-absent

The source baseline reports:
- `DesignateLeader`;
- `StepDown`;
- `OnCrisisEvent`;
- `OnSurvivorDied`;
- `OnSurvivorInjured`;
- `Tick`;
- leader-stress/break events;

but no real runtime caller designates a leader or informs the system of crises.

Therefore 43A is primarily wiring, not feature invention.

## 1.2 Leadership already has a valid downstream effect channel

`SurvivorSocialCoordinator` reportedly binds leadership morale effects into the existing needs/morale system.

Therefore:
- use that channel;
- do not add a leadership morale track.

## 1.3 Ration policy is the working reference implementation

The morning ration triage path already has:

```text
player choice
→ policy value
→ social coordinator
→ ration conflict
→ morale
```

43B generalizes this shape.

## 1.4 Curfew and emergency override are real but ad hoc

They currently mutate schedule state directly.

They should be migrated into the same policy mechanism without changing behavior.

## 1.5 Voting/elections/mutiny do not currently exist

That absence is not itself a defect.

43C must only add escalation if the substrate justifies it.

A mutiny mechanic without:
- persisted grievances;
- readable policies;
- real leader;
- labour refusal;
- arbitration;
is theater.

## 1.6 Existing bureaucracy is the right texture

Voluntary registration, census claims, standing records, arbitration, and treaties already provide a grounded governance vocabulary.

This plan should express governance through:
- records;
- duty;
- promises;
- refusal;
- arbitration;
rather than a generic political score.

---

# 2. Non-Negotiable Governance Invariants

## INV-43.1 — One leadership authority

`LeadershipSystem` remains the sole authority for:
- designated leader;
- leader stress;
- break risk;
- step-down.

No UI-local leader state.

## INV-43.2 — Leadership effects flow through existing needs/morale

No second morale channel.

## INV-43.3 — Crisis notifications are exactly-once

A death, injury, brownout, breach, or duty crisis must not increase leader stress twice due to duplicate subscriptions or panel reopen.

## INV-43.4 — Policy is one reusable mechanism

Rations, curfew, emergency override, work rhythm, quarantine, mourning/funeral and future compatible policies use one policy lifecycle.

## INV-43.5 — Policy effects are declarations

Policy data references:
- needs modifiers;
- schedule parameters;
- grievance hooks;
- friction multipliers;
- existing effect channels.

No policy-specific effect arithmetic hidden in panel code.

## INV-43.6 — Policy decisions cost attention

Adopting/changing policy consumes:
- governance/council moment;
- duty/attention;
or equivalent existing scheduling resource.

No zero-cost toggle spam.

## INV-43.7 — Grievance is the feedback channel

Crew objection is represented through existing:
- ration conflict;
- morale marks;
- friction;
- standing/social records.

No hidden approval meter.

## INV-43.8 — Reversal is possible but costly where authored

A policy can be changed.

Flip-flopping may cost:
- trust;
- morale;
- standing;
through authored effects.

## INV-43.9 — Governance history is recorded

Leader changes and policy choices enter:
- standing record;
- journal;
- event history;
- ending context where relevant.

## INV-43.10 — Refusal precedes violence

If 43C lands, escalation begins with:
- complaint;
- refusal;
- work stoppage;
- arbitration.

Violent outcomes, if any, must be late and already supported by existing systems.

## INV-43.11 — Crew response is trigger-based and data-driven

No rung fires without its authored trigger.

## INV-43.12 — Consent loss is readable before consequence

The player must receive escalating warning signals before labour refusal, challenge, or departure.

## INV-43.13 — Desertion is a survivor fate

A survivor leaving the shelter must use the canonical fate path.

No silent roster deletion.

## INV-43.14 — Internal belief coalitions are derived

Do not hardcode shelter factions.

Use:
- belief profiles;
- relationships;
- friction;
- profession/age where authored.

## INV-43.15 — 43C can defer honestly

If substrate readiness is insufficient, shipping a formal gate/defer document is success.

---

# 3. Definition of Done

Plan 43 is complete only when:

- the player can designate and step down a leader from an existing live survivor/roster surface;
- designation uses `LeadershipSystem`;
- leader state is visible in dashboard/survivor detail;
- leader stress is fed by real crisis/death/injury events;
- crisis subscriptions are exactly-once;
- leadership ticks through the existing social day owner;
- stress and break risk reach the existing morale/needs channel;
- belief profile affects leadership-friction interactions through existing ideology data;
- leader death triggers an explicit succession decision;
- fake hardcoded faction-leader text is removed;
- leadership state round-trips;
- leader identity and leadership ending are available to derived epilogue context;
- one `PolicySystem`/thin policy authority exists;
- policy definitions live in data;
- ration policy, curfew, and emergency override are expressed through it;
- policy effects flow through existing needs/schedule/grievance channels;
- policies generate attributable semantic events;
- policy changes cost attention;
- objections/grievances are visible;
- reversal cost works;
- policy decisions enter standing record/journal;
- policy prompts respect the attention/modal budget;
- policy UI is accessible and localized;
- 43C readiness gate is evaluated before escalation work;
- if 43C lands, an authored escalation ladder exists;
- work refusal removes actual labour;
- disputes can use arbitration;
- registers/census have real consent/governance meaning;
- departure/desertion passes through survivor fate;
- leadership challenge uses step-down/designation verbs;
- belief/friction determine grouping rather than hardcoded blocs;
- every escalation rung is traceable;
- no rung fires without authored trigger;
- save/load mid-escalation is deterministic;
- all governance state reaches journal/standing record/ending as designed;
- port-contract, triad, UI access, selftests, and seeded playthrough gates pass.

---

# 4. Phase P0 — Governance Authority Inventory

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
LeadershipSystem production callers
DesignateLeader callers
StepDown callers
OnCrisisEvent callers
OnSurvivorDied callers
OnSurvivorInjured callers
Leadership.Tick callers
leader state save section
ration policy owner
curfew mutation callers
emergency override mutation callers
RationConflictSystem callers
IdeologicalFrictionSystem callers
MoraleMarkSystem consumers
VoluntaryRegisterSystem host callers
CensusClaimSystem host callers
CrossingArbitrationSystem host callers
RegionalTreatySystem host callers
standing record write APIs
desertion/fate APIs
leader display strings
policy-like toggles
```

---

## P0.2 Build governance authority matrix

Create:

`docs/systems/GOVERNANCE_AUTHORITY_MATRIX.md`

Columns:

```text
concept
authority
current runtime caller
persisted?
player-visible?
downstream effect
event
ending relevance
status
```

Rows:
- designated leader;
- leader stress;
- break risk;
- ration policy;
- curfew;
- emergency override;
- quarantine policy if real;
- mourning/funeral policy if real;
- grievance;
- ideological friction;
- registration;
- census claim;
- arbitration;
- treaty obligation;
- labour refusal;
- desertion;
- succession.

---

## P0.3 Reproduce leadership dead path

Add pre-fix test:

```text
campaign starts
→ no leader can be designated through runtime surface
```

and:

```text
hazard event
→ leadership stress unchanged
```

---

## P0.4 Reproduce fake faction-leader UI

Snapshot/string test proving live trade screen displays hardcoded leader rather than authored faction data.

Preserve until fixed.

---

## P0.5 Build policy inventory

Create table:

```text
policy_id/current_setting
owner
current control
current downstream effect
grievance path
save owner
UI surface
migration status
```

Minimum:
- ration policy;
- curfew;
- emergency override.

Add only real compatible policies.

---

# TASK 43A — Leadership Becomes a Live Campaign Authority

# 43A.0 Goal

Make leadership designation, stress, crisis handling, break risk, succession, voice, and historical recording real without creating a new governance system.

---

## 43A.1 Campaign composition

Ensure one campaign-owned `LeadershipSystem` instance is composed through the existing survivor-social authority.

Reference-identity test:

```text
leader shown in UI
== same LeadershipSystem used by social coordinator
== same instance saved/restored
```

---

## 43A.2 Player designation action

Add action on:
- survivor detail;
- roster;
or other existing live survivor surface.

Action:

```text
DesignateLeader(survivorId)
```

through host/application layer.

No new governance panel.

---

## 43A.3 Validation

Designation validates:
- survivor exists;
- alive;
- not incapacitated where rules require;
- not already leader;
- any leadership eligibility requirements from existing data.

Return typed `ActionResult` with reason IDs.

---

## 43A.4 Step-down action

Expose:

```text
StepDown()
```

through the same live surface.

Step-down may be:
- voluntary;
- crisis response;
- challenge resolution.

---

## 43A.5 Designation event

Use Plan 31 semantic event:

```text
leader_designated
```

Payload:
- survivor ID;
- day;
- cause/action;
- previous leader if relevant.

---

## 43A.6 Step-down event

Emit:
- leader stepped down;
- reason;
- successor pending if relevant.

---

## 43A.7 Crisis source inventory

Map real crisis events:
- survivor death;
- survivor injury;
- hazard warning;
- duty vacancy;
- brownout;
- breach;
- major ration conflict;
- obligation failure if appropriate.

Only connect events that existing leadership semantics consider crises.

---

## 43A.8 One crisis adapter

Create a narrow host adapter/coordinator that translates semantic campaign events into leadership calls.

Avoid each panel calling leadership directly.

---

## 43A.9 Exactly-once event identity

Use:
- event ID;
- day + kind + source identity;
or existing event identity.

Do not call `OnCrisisEvent` once from producer and once from briefing.

---

## 43A.10 Death-specific leadership call

If `OnSurvivorDied` carries richer semantics than generic crisis:
- call it once;
- avoid also applying equivalent generic crisis stress unless system expects both.

Document combination.

---

## 43A.11 Injury-specific call

Same rule for `OnSurvivorInjured`.

---

## 43A.12 Daily ticking

Drive:

```text
Leadership.Tick(gameHours)
```

inside `SurvivorSocialCoordinator.TickDay` / existing `survivor_social` day owner.

No new campaign day owner.

---

## 43A.13 Game-hours derivation

Use:
- calendar/day schedule;
- existing day-duration semantics.

No hardcoded separate leadership hours.

---

## 43A.14 Stress visibility

Dashboard/leader detail shows:
- current leader;
- stress;
- break-risk state;
- deaths witnessed;
- major stressors where available.

---

## 43A.15 Text + bar

Stress status must use:
- numerical/word state;
- bar/color only supplemental.

---

## 43A.16 Stress attribution

Where system can expose cause history:
- last major crises;
- deaths;
- overload.

Do not build full event history inside LeadershipSystem if journal already owns history.

---

## 43A.17 Existing morale channel

Leadership stress/break effects use:

```text
Leadership.ApplyMoraleDelta
Leadership.ApplyShelterMoraleDelta
→ NeedsSystem Morale
```

No direct second morale mutation.

---

## 43A.18 Break-risk decision

When break-risk threshold is reached:
- emit warning;
- create decision opportunity;
- do not instantly depose via hidden RNG unless existing system explicitly does.

Options may include:
- keep leader;
- ask step-down;
- reassign duties/rest;
depending supported mechanics.

---

## 43A.19 Rest as real remedy

If reducing leader load is possible:
- use duty/fitness/schedule systems.

No special "leadership recovery potion."

---

## 43A.20 Belief-profile integration

Post-40A:
- leader `belief_profile_id` is real data.

Use `IdeologicalFrictionSystem` to affect:
- synergy;
- conflict;
- grievance likelihood.

---

## 43A.21 No hardcoded belief pairings

All pairing logic comes from existing ideological/friction data.

---

## 43A.22 Leader death

On canonical leader death:
- leadership authority clears/marks vacancy;
- memorial/fate pipeline handles death;
- governance path opens succession decision.

---

## 43A.23 Succession candidate ranking

Use existing meaningful facts where available:
- standing;
- leadership skill;
- role/fitness;
- relationship/trust.

Do not create opaque "leadership score" unless existing system already has one.

---

## 43A.24 Player confirmation

System may recommend candidate.

Player confirms designation.

No automatic permanent successor without choice unless emergency fallback is explicitly authored.

---

## 43A.25 Leadership vacancy state

A vacancy is real:
- visible;
- may affect morale/policy efficacy if existing mechanics support;
- recorded in standing history.

---

## 43A.26 Standing-record succession history

Record:
- leader appointed;
- stepped down;
- died;
- vacancy;
- successor.

Use Plan 41B standing record.

---

## 43A.27 Leader voice

Policy announcement/briefing lines may identify current leader through Plan 42C voice.

Do not create separate narrative engine.

---

## 43A.28 Fake faction-leader repair

Remove:

```text
Leader: Varek (gen 1)
```

from live trade UI.

Bind:
- faction catalog;
- authored character/leader data.

---

## 43A.29 Faction-leader validation

Catalog integrity must verify:
- referenced leader exists;
- correct faction.

If no authored leader:
- show generic localized fallback;
- never invent a hardcoded person.

---

## 43A.30 Epilogue leadership facts

Extend derived epilogue context only if Plan 19's architecture supports additive facts.

Candidate facts:
- final leader ID;
- leadership continuity count;
- leadership ended by death/step-down/challenge.

Do not persist duplicate ending booleans if derivable.

---

## 43A.31 Save round-trip

Persist existing:
- designated leader;
- stress;
- deaths witnessed;
- break state.

After load:
- same leader;
- same read model;
- no duplicate event subscriptions.

---

## 43A.32 Subscription lifecycle test

Open/close relevant panels repeatedly.

Trigger one crisis.

Assert:
- leadership stress increments once.

---

## 43A.33 100-day crisis cadence

Seeded run:
- controlled deaths/injuries/hazards;
- deterministic stress curve;
- no listener multiplication.

---

## 43A.34 Leadership wiring tests

Required:
- appoint valid;
- appoint invalid;
- step-down;
- crisis;
- injury;
- death;
- stress→morale;
- break warning;
- succession;
- save/load;
- faction leader from data.

---

## 43A.35 Documentation

Create:

`docs/systems/LEADERSHIP.md`

Include:
- authority;
- designation flow;
- crisis inputs;
- morale output;
- persistence;
- succession;
- UI;
- events;
- tests.

### 43A DoD

The shelter has a real leader the player chose, who is visibly affected by campaign crises and can leave office through real simulation state.

---

# TASK 43B — One Policy Lifecycle

# 43B.0 Goal

Generalize the already-working ration-policy pattern so governance decisions share one data-driven lifecycle:

```text
propose
→ decide
→ apply
→ grievance
→ report
→ record
```

---

## 43B.1 Create a thin `PolicySystem`

File:

`Assets/Ashfall.Core/Governance/PolicySystem.cs`

Responsibilities:
- catalog;
- active option per scope;
- decision state;
- proposer/decision metadata;
- change cost;
- grievance declarations;
- read model.

It does not own:
- needs;
- schedule;
- ration inventory;
- faction simulation.

---

## 43B.2 Policy catalog

Create:

`policies.json`

Fields:

```text
schema_version
policies[]
  id
  scope
  options[]
  proposer_rules
  decision_method
  attention_cost
  reversal_cost
  effect_declarations
  grievance_rules
  localization_keys
```

---

## 43B.3 Initial scopes

Start only with real behavior:

```text
rations
curfew
emergency_override
```

Potential later compatible:
- work rhythm;
- quarantine;
- mourning/funerals.

Do not populate empty fake policies.

---

## 43B.4 Policy options

Example:

```text
ration.normal
ration.reduced
ration.priority_sick
```

Use actual current `RationPolicy` values.

Do not rename existing semantics casually.

---

## 43B.5 Effect declarations

Policy option may declare references to:
- NeedsModifier source;
- schedule mode;
- ration policy enum/value;
- friction multiplier;
- grievance group.

No embedded arbitrary code expression language.

---

## 43B.6 Effect applier registry

Host maps declared effect types to existing authorities.

Example:

```text
needs_modifier → NeedsModifierStack
schedule_flag → ShelterScheduleSystem
ration_policy → SurvivorSocialCoordinator
```

---

## 43B.7 Policy action transaction

1. validate policy exists;
2. validate option;
3. validate proposer/decision requirements;
4. validate attention cost;
5. preview effects/grievance;
6. apply authoritative state;
7. record policy state;
8. apply attention cost;
9. emit event;
10. create grievance records;
11. update journal/standing record.

---

## 43B.8 Ration policy migration

Wrap existing morning ration triage semantics.

Behavior equivalence test:
- same policy;
- same social/grievance/morale consequence.

No balance change unless explicitly authored.

---

## 43B.9 Curfew migration

Replace direct panel lambda mutation with policy action.

Downstream:
- `ShelterScheduleSystem.curfewActive`;
- fatigue/sleep effect through Plan 24B.

---

## 43B.10 Emergency override migration

Same pattern.

Preserve current schedule/power semantics.

---

## 43B.11 No duplicate state

PolicySystem may track active option metadata.

The actual domain state remains owned by:
- ration/schedule system.

Define which is canonical per field.

Preferred:
- policy decision state is canonical governance history;
- domain system is canonical operational state;
- an invariant keeps them synchronized.

---

## 43B.12 Synchronization assertion

After policy decision:
- PolicySystem current option;
- domain authority state
must agree.

Fail if drift.

---

## 43B.13 Attention cost

Policy adoption consumes one existing resource, such as:
- governance/council duty hours;
- day action budget;
- leader/administrator time.

Choose existing mechanism.

Do not invent "governance points."

---

## 43B.14 Crisis-modal budget

A policy decision cannot forcibly interrupt:
- active crisis modal;
- critical combat/medical flow.

It can:
- queue for briefing/journal;
- remain pending.

---

## 43B.15 Proposer rules

Use existing identity:
- leader;
- role/profession;
- standing;
- commitment counterparty
where appropriate.

No generic council simulation.

---

## 43B.16 Decision method

Initial options may be:

```text
leader_decision
player_decision
required_commitment
```

Do not add votes until 43C substrate proves need.

---

## 43B.17 Grievance declarations

Each option declares objection predicates using existing facts:

- belief profile;
- profession;
- age class;
- ration priority;
- relationship;
- prior policy history.

---

## 43B.18 No hidden opinion score

Grievance output goes to:
- RationConflictSystem-style record;
- MoraleMark;
- IdeologicalFriction;
- standing/social log.

---

## 43B.19 Grievance event

Emit canonical:
- `grievance_raised`.

Payload:
- policy;
- option;
- objector/group;
- reason.

---

## 43B.20 Policy event

Emit:
- `policy_proposed`;
- `policy_adopted`;
- `policy_reversed`;
- `policy_rejected`
as needed by Plan 31 vocabulary.

---

## 43B.21 Grievance voice

Plan 42C can render objections from:
- morale marks;
- grievance reason;
- belief/profession.

No bespoke speech generator here.

---

## 43B.22 Reversal

Player may reverse policy.

Validate:
- minimum duration if authored;
- attention cost;
- reversal/trust penalty.

---

## 43B.23 Reversal price

Effects:
- morale/trust/friction;
using existing channels.

No permanent hard lock.

---

## 43B.24 Standing record

Record:
- policy;
- option;
- proposer/leader;
- day;
- reversal;
- grievance.

---

## 43B.25 Journal history

Same policy decision appears in journal.

Do not derive separate prose logic.

---

## 43B.26 Ending context

Plan 19 may derive:
- how treaty/debt/policy decisions were made;
- policy history aggregates
if endings need them.

Avoid dozens of direct policy booleans.

---

## 43B.27 Commitments integration

Plan 38C may require policies:
- ration delivery priority;
- work quota;
- treaty compliance.

Commitment can request policy action, but does not own policy state.

---

## 43B.28 Leader interaction

Leader identity can:
- propose;
- announce;
- bear stress consequences.

Do not make policy impossible when no leader unless the policy definition requires one.

---

## 43B.29 Policy read model

Expose:

```text
policy id
scope
active option
effective since
proposer
attention cost
current effects
likely objections
reversal cost
available options
```

---

## 43B.30 UI placement

Use existing:
- shelter schedule;
- ration/survivor social;
- dashboard/briefing.

Do not add "Governance Console."

---

## 43B.31 Shared policy control component

If multiple panels need a chooser:
- reuse small UI component/read model;
- no new framework.

---

## 43B.32 Accessibility

Options:
- keyboard selectable;
- textual effect summary;
- grievance preview;
- no color-only support/opposition.

---

## 43B.33 Localization

All policy names/options/effects use Plan 25A text layer.

No inline English in Core.

---

## 43B.34 Save round-trip

Persist:
- governance metadata;
- current policy choices;
- history references if owned here.

After load:
- domain state matches.

---

## 43B.35 Determinism

Same:
- policy;
- beliefs;
- roster;
- prior history
=> same grievance generation.

---

## 43B.36 Equivalence tests

For migrated:
- ration policy;
- curfew;
- emergency override;

assert same operational outcome as pre-migration behavior.

---

## 43B.37 Reversal tests

Apply A → B → A.

Assert:
- final domain state correct;
- reversal cost once;
- history retains changes.

---

## 43B.38 Data-integrity tests

Validate:
- policy IDs;
- option IDs;
- effect declaration types;
- referenced beliefs/professions/age classes;
- localization keys;
- no impossible decision method.

---

## 43B.39 Docs

Create:

`docs/systems/POLICIES.md`

Table:
- policy;
- options;
- effect channels;
- grievance predicates;
- decision method;
- cost;
- UI surface.

### 43B DoD

Policy is one data-driven mechanism with real operational effects, visible objections, reversible decisions, and historical recording.

---

# TASK 43C — Consent, Refusal, Arbitration & the Mutiny Gate

# 43C.0 Goal

Give accumulated grievance a terminal possibility only if the substrate proves it can do so honestly.

The likely first meaningful endpoint is not mutiny.

It is:

> "I'm not working that shift."

---

## 43C.1 Readiness gate

Before coding escalation, generate:

`docs/systems/CREW_CONSENT_READINESS.md`

Check:

```text
leadership persisted?
policy persisted?
grievance persisted?
grievance source attributable?
standing record writable?
duty refusal possible?
fate/desertion path available?
arbitration live?
belief profiles live?
save/load stable?
```

---

## 43C.2 Readiness result

Statuses:

```text
READY
PARTIAL
BLOCKED
```

If any hard prerequisite is `BLOCKED`:
- do not implement ladder;
- list gaps;
- hand off to next wave;
- mark 43C deferred by design.

This satisfies the plan.

---

## 43C.3 Escalation catalog

If ready, create data:

`crew_consent_escalation.json`

Rungs:

```text
quiet_discontent
work_refusal
organized_grievance
leadership_challenge
departure
```

Only add open mutiny/violence if existing mechanics justify it.

---

## 43C.4 Each rung must define

```text
id
trigger
minimum_duration
effect
warning
off_ramps
eligible_participants
cooldown
next_rung
```

---

## 43C.5 Quiet discontent

Inputs:
- repeated grievances;
- ideological friction;
- policy reversal;
- ration inequality;
- leader stress/break;
- unmet commitments.

Output:
- morale mark;
- voiced complaint;
- journal/briefing signal.

No labour loss yet.

---

## 43C.6 Work refusal

A survivor/group refuses a shift.

Effect:
- duty assignment removed/refused;
- producer loses labour;
- event emitted.

This is the first material consent consequence.

---

## 43C.7 No automatic "disloyalty"

Work refusal is tied to:
- authored grievance trigger;
- policy/leader context;
- belief/friction.

No generic low morale threshold alone unless data explicitly defines it.

---

## 43C.8 Organized grievance

Represent through:
- standing/grievance record;
- arbitration request;
- policy challenge.

No new protest meter.

---

## 43C.9 Arbitration

Use `CrossingArbitrationSystem` patterns/capabilities where compatible.

Create adapter for internal dispute:
- claimant;
- leadership/policy;
- stakes;
- decision;
- outcome.

Do not fork another dialogue resolution engine.

---

## 43C.10 Arbitration outcomes

Possible existing channels:
- policy reversal;
- duty release;
- compensation/resource;
- standing/morale change;
- leader stress.

---

## 43C.11 Registration as consent instrument

Use:
- VoluntaryRegisterSystem;
- CensusClaimSystem.

Define what registration means operationally:
- counted in shelter;
- eligible/obligated for duty;
- entitled to rations/records;
- permitted departure status.

Do not imply coercion not already supported.

---

## 43C.12 Consent metadata

If needed, add minimal fields to existing record:
- voluntary;
- claimed;
- duty-bound status;
- departure request.

Avoid new identity subsystem.

---

## 43C.13 Leadership challenge

Uses:
- `StepDown`;
- `DesignateLeader`.

The "challenge" is a decision state.

No combat/QTE.

---

## 43C.14 Challenge off-ramps

Examples:
- leader steps down;
- policy reversed;
- grievance settled;
- claimant leaves;
- arbitration resolved.

Every rung needs at least one nonviolent exit.

---

## 43C.15 Belief-based participation

Use:
- belief profile;
- ideological friction;
- relation affinity.

Do not create internal political parties.

---

## 43C.16 Coalition derivation

Participants may be grouped by:
- shared grievance;
- compatible belief;
- shared shift/profession;
- relation.

All derived, deterministic.

---

## 43C.17 Departure request

A survivor may choose to leave after unresolved escalation.

Player may:
- allow;
- negotiate;
- refuse if existing governance rules support it.

Do not invent imprisonment mechanics.

---

## 43C.18 Desertion/fate

Actual departure passes through `SurvivorFateSystem`.

Record:
- deserted/departed;
- reason;
- day;
- keepsake disposition;
- duty release;
- ration recalculation.

---

## 43C.19 Memorial distinction

Departure is not death.

41A memorial pipeline:
- no funeral;
- maybe keepsake left;
- standing record/fate line;
- grief/social ripple as existing rules allow.

---

## 43C.20 Labour ripple

Departure/refusal immediately affects:
- duty roster;
- production Plan 35C;
- caregiving/expeditions if assigned.

---

## 43C.21 Ration ripple

Departure updates future ration obligation through Plan 22/35.

No retroactive reclaim.

---

## 43C.22 Leader stress ripple

Challenge/refusal may increase leader stress through existing crisis channel.

Exactly once.

---

## 43C.23 Briefing ladder

Every rung emits a semantic event.

Example:
- grievance raised;
- shift refused;
- arbitration opened;
- leadership challenged;
- survivor departed.

---

## 43C.24 Journal sequence

The player can reconstruct:

```text
policy adopted
→ grievance
→ refusal
→ arbitration
→ challenge/departure
```

---

## 43C.25 Standing-record sequence

Standing record stores major transitions.

No history only in transient briefing.

---

## 43C.26 Voice

Plan 42C surfaces:
- objections;
- refusal lines;
- arbitration statements.

Tone:
- tired;
- procedural;
- human.

No villain monologue.

---

## 43C.27 Attention budget

Escalation should not trigger:
- multiple overlapping modals.

Use briefing/journal + queued decision.

---

## 43C.28 Negative trigger test

For every rung:
- construct state without trigger;
- assert no transition.

This is mandatory.

---

## 43C.29 Trigger precedence

If several rungs become eligible same day:
- choose deterministic lowest/current next rung;
- never jump from quiet discontent directly to departure without authored override.

---

## 43C.30 Cooldown/de-escalation

After resolution:
- grievance can decay or resolve;
- escalation does not instantly re-trigger.

Use authored cooldown/off-ramp.

---

## 43C.31 Save mid-escalation

Save at:
- grievance;
- refusal;
- arbitration;
- challenge.

Load:
- same rung;
- no duplicate event;
- same eligible next actions.

---

## 43C.32 Determinism

Same:
- seed;
- policies;
- grievances;
- beliefs;
- standing;
- choices
=> same ladder trajectory.

---

## 43C.33 Seeded reachability run

Use expansion QA playthrough.

Prove:
- at least one refusal path;
- at least one de-escalation path;
- one departure/challenge path if implemented.

---

## 43C.34 No forced mutiny requirement

The plan is not failed if no mutiny/violence ships.

The acceptance question is:

> Can the crew say no in a readable, staged, consequential way?

---

## 43C.35 Docs

Create:

`docs/systems/CREW_CONSENT.md`

Document:
- readiness;
- ladder;
- triggers;
- off-ramps;
- authority;
- persistence;
- events;
- tests.

### 43C DoD

Either the crew can refuse and escalate through a readable, data-driven ladder with real off-ramps, or the plan ships a precise readiness gate explaining why escalation is deferred.

---

# 5. Cross-Task Dependency Graph

```text
40A beliefs / identity
        │
        ▼
43A leadership
        │
        ▼
43B policy
        │
        ▼
43C consent / refusal
```

Support:

```text
24A fitness ───────────► labour/refusal consequences
24B needs stack ───────► morale/fatigue/policy effects
31 events ─────────────► governance visibility
42C voice ─────────────► objections/announcements
41B standing record ───► history
38C commitments ───────► deadline-driven policy pressure
35C labour ────────────► refusal actually costs production
19A ending ────────────► leadership/policy history
36A port contract ─────► sink/binding enforcement
```

---

# 6. Governance State Model

Governance is distributed authority, not one monolith.

```text
LeadershipSystem
  owns leader/stress

PolicySystem
  owns governance decision metadata

Needs/Schedule/Ration
  own operational effects

RationConflict/Friction/MoraleMark
  own objections/social effects

StandingRecord/Journal
  own history

DutyRoster
  owns work

SurvivorFate
  owns departure/death fate

CommitmentSystem
  owns due obligations
```

---

# 7. Leadership Crisis Contract

One event adapter should decide which leadership API to call:

```text
survivor death → OnSurvivorDied
survivor injury → OnSurvivorInjured
other crisis → OnCrisisEvent
```

Do not stack all three for one incident unless intended.

---

# 8. Policy Contract

A policy definition answers:

```text
what decision?
which scope?
who may propose?
how decided?
what existing state changes?
what grievances can arise?
what does reversal cost?
where is it visible?
```

---

# 9. Grievance Contract

A grievance record should identify:

```text
source policy/action
objector survivor/group
reason id
severity
day
resolved?
resolution
```

Use existing grievance/mark system where possible.

---

# 10. Consent Contract

Consent is not a meter.

It is observed through:
- compliance;
- grievance;
- refusal;
- arbitration;
- leadership challenge;
- departure.

---

# 11. Event Vocabulary

Use Plan-31 semantic kinds, such as:

```text
leader_designated
leader_stepped_down
leader_break_risk
policy_adopted
policy_reversed
grievance_raised
shift_refused
arbitration_opened
leadership_challenged
survivor_departed
```

Use generalized registry values if already defined.

---

# 12. Persistence Matrix

| State | Authority | Persist? |
|---|---|---:|
| leader ID | LeadershipSystem/social section | yes |
| leader stress | LeadershipSystem | yes |
| deaths witnessed | LeadershipSystem | yes |
| policy metadata | PolicySystem | yes |
| operational ration state | existing ration/social | yes |
| curfew/override | schedule | yes |
| grievance records | existing social/grievance | yes |
| standing record | StandingRecord | yes |
| consent escalation rung | consent adapter/system if implemented | yes |
| duty refusal | DutyRoster/history | yes |
| desertion fate | SurvivorFate | yes |
| derived ending facts | derived | no |

---

# 13. Governance Transaction Semantics

For a policy decision:

1. validate policy;
2. validate option;
3. validate proposer/leader requirements;
4. validate attention cost;
5. compute effect declarations;
6. preview grievances;
7. apply domain authority changes;
8. persist policy metadata;
9. charge attention;
10. create grievance records;
11. emit events;
12. write history.

No partial invisible state.

---

# 14. Leadership Succession Semantics

When leader leaves office:

```text
clear active leader
→ record reason
→ expose vacancy
→ rank/recommend candidate if possible
→ player confirmation
→ designate
→ journal/standing record
```

No fake immediate successor if player action is available.

---

# 15. Policy Reversal Semantics

Reversal:

```text
current option A
→ choose B
→ apply B
→ reversal cost
→ grievance/trust effect
→ history
```

Later A may be chosen again.

The system remembers flip-flops.

---

# 16. Read Models

## Leadership read model

```text
leader
stress
stress band
break risk
deaths witnessed
current policy count
recent crises
succession status
```

## Policy read model

```text
policy
active option
effects
cost
objections
reversal cost
proposer
effective day
```

## Consent read model

```text
active grievance
rung
participants
trigger
off-ramps
next risk
```

Only expose consent model if 43C ships.

---

# 17. UI Acceptance

## Survivor detail / roster

- appoint/step down;
- leader badge;
- stress/break status.

## Dashboard

- current leader;
- top governance warning;
- active important policy;
- next commitment.

## Schedule/ration surfaces

- use policy action;
- show effects/grievances.

## Standing/journal

- policy/leadership history.

No new governance console.

---

# 18. Accessibility

- leader state text + icon;
- policy option effect text;
- grievance reasons readable;
- keyboard actions;
- no color-only severity/support;
- queued decisions do not trap focus.

---

# 19. Localization

Every:
- policy;
- option;
- grievance;
- leader status;
- refusal;
- arbitration state
uses localization keys.

No inline Core prose.

---

# 20. Tone Guardrails

Governance should be expressed as:
- shifts;
- food;
- records;
- exhaustion;
- practical disagreement.

Avoid:
- campaign-speech rhetoric;
- ideological villain monologues;
- gamified approval slogans.

---

# 21. Failure Injection Matrix

## N43.1 Same crisis delivered twice
Expected: leader stress increments once.

## N43.2 Panel reopens ten times
Expected: no duplicate leadership subscription.

## N43.3 Leader dies
Expected: vacancy + succession path, no stale leader UI.

## N43.4 PolicySystem says curfew on but schedule says off
Expected: synchronization invariant fails.

## N43.5 Policy changes with no attention capacity
Expected: action refused.

## N43.6 Policy option references unknown effect type
Expected: data-integrity fail.

## N43.7 Grievance fires without declared predicate
Expected: test fails.

## N43.8 43C substrate not ready
Expected: escalation code not enabled; readiness doc says BLOCKED.

## N43.9 Shift refusal
Expected: duty removed and production loses labour.

## N43.10 Desertion removes survivor directly
Expected: fate-path gate fails.

## N43.11 Hardcoded faction leader remains
Expected: UI/source gate fails.

## N43.12 Escalation skips rungs
Expected: ladder contract fails.

---

# 22. Determinism Contract

Same:

```text
seed
+ leader
+ crisis sequence
+ policies
+ beliefs
+ grievances
+ commitments
+ player decisions
```

=> same:

```text
leader stress
policy effects
grievances
escalation rung
refusal
succession recommendation
departure outcome
history
```

---

# 23. Balance Guardrails

Leadership stress:
- should matter;
- should not force constant replacement.

Policy grievance:
- should create tradeoffs;
- should not make any policy universally optimal.

Refusal:
- should be rare enough to feel consequential;
- predictable enough to be fair.

---

# 24. Telemetry

Track:

```text
days without leader
leader stress avg/max
break warnings
leadership changes
policies adopted
policy reversals
grievances raised
grievances resolved
shift refusals
arbitrations
leadership challenges
departures
production hours lost to refusal
morale changes from governance
```

Use for seeded 100–200-day soak.

---

# 25. Static / CI Gates

Recommended:

```text
leadership_runtime_wiring
leadership_subscription_identity
policy_catalog_integrity
policy_domain_state_equivalence
policy_effect_port_contract
governance_history_recording
crew_consent_readiness
consent_trigger_integrity
no_fake_faction_leader
no_new_governance_console
```

---

# 26. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
python3 scripts/ci/generate-port-contract.py --check
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also:

```text
ashfall-expansion-qa-playthrough
ashfall-ui-access
ashfall-snapshot-diff
```

Use actual current commands if names differ.

---

# 27. Recommended Commit Breakdown

```text
43A-1 leadership wiring audit + failing tests
43A-2 designation/step-down actions
43A-3 crisis adapter + exact-once subscriptions
43A-4 TickDay/stress/morale integration
43A-5 leader UI/read model
43A-6 succession + standing record
43A-7 faction-leader data binding
43A-8 persistence/ending/docs/tests

43B-1 policy catalog + thin PolicySystem
43B-2 effect applier + transaction
43B-3 ration policy migration
43B-4 curfew/emergency migration
43B-5 grievance/reversal/attention cost
43B-6 journal/standing/events
43B-7 UI/accessibility/localization
43B-8 save/equivalence/integrity tests

43C-1 readiness gate document
43C-2 escalation catalog if READY
43C-3 work refusal
43C-4 arbitration/register integration
43C-5 leadership challenge
43C-6 departure/fate integration
43C-7 history/voice/off-ramps
43C-8 save/determinism/reachability tests
```

If readiness is BLOCKED:
- stop after 43C-1;
- produce exact gap handoff;
- do not create placeholder escalation code.

---

# 28. Risk Register

## R43.1 Leadership stress double-counts

Mitigation:
- semantic-event adapter;
- exact-once identity;
- lifecycle tests.

## R43.2 PolicySystem duplicates domain state

Mitigation:
- explicit authority map;
- synchronization invariants.

## R43.3 Governance becomes another minigame

Mitigation:
- existing surfaces;
- thin policy layer;
- no approval meter.

## R43.4 Grievance becomes hidden punishment

Mitigation:
- reason IDs;
- voice;
- briefing;
- standing record.

## R43.5 43C escalates too quickly

Mitigation:
- refusal first;
- authored ladder;
- off-ramps;
- minimum durations.

## R43.6 Political blocs get hardcoded

Mitigation:
- beliefs/relations/friction derive participants.

## R43.7 Policy modals interrupt critical play

Mitigation:
- attention budget;
- queue to briefing/journal.

## R43.8 Ending context bloats

Mitigation:
- derive aggregates from governance history rather than many booleans.

---

# 29. Acceptance Checklist

## 43A — Leadership

- [ ] one campaign LeadershipSystem authority
- [ ] reference identity verified
- [ ] appoint action on existing live surface
- [ ] designation validation
- [ ] step-down action
- [ ] leader-designated event
- [ ] leader-step-down event
- [ ] crisis-source matrix
- [ ] one crisis adapter
- [ ] exact-once event identity
- [ ] death-specific call correct
- [ ] injury-specific call correct
- [ ] Tick driven by survivor_social owner
- [ ] game-hours source correct
- [ ] stress visible
- [ ] text + bar accessibility
- [ ] stress attribution
- [ ] existing morale channel used
- [ ] break-risk decision
- [ ] rest/workload remedy uses existing systems
- [ ] belief-profile integration
- [ ] no hardcoded belief pairing
- [ ] leader death clears office
- [ ] succession candidate ranking grounded
- [ ] player confirms successor
- [ ] vacancy state visible
- [ ] standing-record history
- [ ] leader voice reuse
- [ ] fake Varek string removed
- [ ] faction leader reads authored data
- [ ] catalog validation
- [ ] epilogue leadership facts derived
- [ ] save round-trip
- [ ] subscription lifecycle test
- [ ] 100-day crisis cadence deterministic
- [ ] leadership wiring tests pass
- [ ] LEADERSHIP.md complete

## 43B — Policy

- [ ] thin PolicySystem created
- [ ] policies.json created
- [ ] initial scopes only real
- [ ] ration options map existing behavior
- [ ] effect declarations data-driven
- [ ] effect applier registry
- [ ] policy transaction
- [ ] ration policy migrated
- [ ] ration equivalence test
- [ ] curfew migrated
- [ ] curfew equivalence test
- [ ] emergency override migrated
- [ ] emergency equivalence test
- [ ] no duplicate operational state
- [ ] synchronization invariant
- [ ] attention cost real
- [ ] crisis-modal budget respected
- [ ] proposer rules data-driven
- [ ] decision method minimal
- [ ] grievance declarations data-driven
- [ ] no approval meter
- [ ] grievance event
- [ ] policy events
- [ ] voice reuse
- [ ] reversal supported
- [ ] reversal price
- [ ] standing record
- [ ] journal history
- [ ] ending integration derived
- [ ] commitments integration
- [ ] leader interaction
- [ ] policy read model
- [ ] no new panel
- [ ] shared control where useful
- [ ] accessibility
- [ ] localization
- [ ] save round-trip
- [ ] determinism
- [ ] migrated-toggle equivalence
- [ ] reversal tests
- [ ] data integrity
- [ ] POLICIES.md complete

## 43C — Consent/refusal

- [ ] CREW_CONSENT_READINESS.md generated
- [ ] readiness status explicit
- [ ] blocked substrate halts escalation work
- [ ] escalation catalog only if READY
- [ ] each rung has trigger/effect/warning/off-ramp
- [ ] quiet discontent real
- [ ] work refusal removes labour
- [ ] no generic hidden disloyalty
- [ ] organized grievance uses existing records
- [ ] arbitration reused
- [ ] arbitration outcomes use existing channels
- [ ] register/census semantics documented
- [ ] no new identity subsystem
- [ ] leadership challenge uses 43A verbs
- [ ] challenge has off-ramps
- [ ] belief-based participation
- [ ] no internal hardcoded parties
- [ ] departure request flow
- [ ] desertion uses SurvivorFate
- [ ] departure distinct from death
- [ ] labour/ration ripple
- [ ] leader stress ripple exact-once
- [ ] briefing ladder
- [ ] journal sequence
- [ ] standing-record sequence
- [ ] voice tone restrained
- [ ] attention budget
- [ ] negative trigger tests
- [ ] rung precedence deterministic
- [ ] cooldown/de-escalation
- [ ] mid-escalation save round-trip
- [ ] deterministic ladder
- [ ] seeded reachability run
- [ ] no forced mutiny requirement
- [ ] CREW_CONSENT.md complete if implemented

---

# 30. Ship / No-Ship Gate

**SHIP** only if:

```text
leadership_authorities == 1
AND runtime_leader_designation == true
AND crisis_events_reach_leadership == true
AND duplicate_leadership_crisis_calls == 0
AND leadership_tick_registered_once == true
AND leadership_uses_existing_morale_channel == true
AND leader_state_roundtrip == pass
AND fake_faction_leader_strings == 0
AND policy_authorities == 1
AND migrated_policy_operational_equivalence == pass
AND policy_effects_use_existing_channels == true
AND policy_grievances_attributable == true
AND policy_attention_cost_real == true
AND policy_history_recorded == true
AND policy_roundtrip == pass
AND (
    crew_consent_readiness == BLOCKED_WITH_DOCUMENTED_GAPS
    OR (
        crew_consent_readiness == READY
        AND refusal_removes_real_labour == true
        AND escalation_rungs_without_trigger == 0
        AND every_rung_has_off_ramp == true
        AND desertion_uses_survivor_fate == true
        AND consent_roundtrip == pass
    )
)
AND governance_port_contract == pass
AND triad_drift_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 31. Implementer Handoff

1. Do 43A before policy work.
2. Use the existing `LeadershipSystem`; do not add a leadership manager.
3. Bind real crisis/death/injury events exactly once.
4. Tick leadership through the existing `survivor_social` day owner.
5. Surface stress before break risk becomes consequential.
6. Use the existing morale/needs channel.
7. Make leader succession a visible player decision.
8. Remove the hardcoded trade-screen faction leader.
9. Build 43B from the working ration-policy pattern.
10. Keep `PolicySystem` thin and declarative.
11. Migrate curfew and emergency override with behavior-equivalence tests.
12. Use needs, schedule, ration conflict, and friction as effect sinks.
13. Charge real attention/duty cost for policy changes.
14. Record leadership and policy history in journal/standing record.
15. Evaluate 43C readiness before writing any escalation code.
16. If not ready, stop and ship the gap document.
17. If ready, implement refusal before challenge or departure.
18. Reuse arbitration/register/fate systems.
19. Derive participants from beliefs/relations, not hardcoded factions.
20. Require a readable warning and an off-ramp at every escalation rung.
21. Close with seeded governance reachability, exact-once subscription tests, and full CI.

---

# 32. Final Outcome

When this plan is complete, governance in ASHFALL stops being a collection of hidden systems and panel toggles.

The player can appoint a leader from the people who actually live in the shelter. That leader carries stress from deaths, injuries, brownouts, breaches, and other real crises. The player can see that strain before it becomes a break risk, and leadership effects move the same morale and fatigue channels the rest of the game uses.

Rationing, curfew, emergency override, and future compatible policies become one mechanism: propose, decide, apply, hear objections, record the consequence. Policy is not a free toggle. It costs attention, can be reversed at a price, and leaves a standing-record trail the ending can read.

If the substrate is ready, the crew can finally say no. They complain first. They may refuse a shift. Production loses real labour. A grievance can enter arbitration. A leader can be challenged through the same step-down/designation verbs already used for normal succession. A survivor who leaves goes through the same fate, ration, duty, keepsake, and historical systems as any other departure.

If the substrate is not ready, the plan stops honestly before inventing theater.

The result is not a political simulator. It is the existing leadership, ration, schedule, belief, grievance, register, arbitration, duty, fate, journal, and ending machinery finally behaving like a shelter whose people can consent, object, and remember who made the decisions.
