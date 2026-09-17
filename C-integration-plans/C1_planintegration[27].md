# C1 — Flagship Integration Plan [27]: Shelter Governance & Political System

> **Output:** `C1_planintegration[27].md`
>
> **Source baseline:** Plan 159 — Shelter Governance & Political System
>
> **Primary mission:** make the shelter's rules, priorities, representation, justice, and political disagreements explicit enough to affect daily survival while preserving existing authorities for duty, rationing, medicine, trade, morale, relationships, leadership, ideology, autonomy, and consequences.
>
> **Primary architectural rule:** governance does **not** own shelter operations. Governance owns policy intent, law status, decision procedure, legitimacy/representation state, and governance-history. Existing systems continue to own the facts those policies influence.
>
> **Primary continuity rule:** Plan 43 (“Governing Together”) and any existing council/leadership/refusal/autonomy mechanics must be re-audited before creating a new `GovernanceSystem`. If current code already owns decision legitimacy, voting, council membership, or consent, Plan 159 extends that authority instead of establishing a second government.
>
> **Mandatory execution order:** 159A authority/overlap audit → 159B policy/law contract and effect adapters → 159C representation/ideological blocs and decision procedures → 159D justice/dispute-resolution only through real event/evidence/sanction rails → 159E UI, persistence, balance, determinism, reachability and CI → 159F advanced elections/revolts/institutions only when supporting systems are real.
>
> **Critical scope rule:** the source proposes five ideological political factions, propaganda, coercion, imprisonment, exile, elections, revolts, constitutions, education mandates, religion policy, family planning, corruption investigations, and political specialization. These are not automatically core Plan-159 mechanics. Each must either map to a real existing authority or remain an explicit follow-on.
>
> **Guardrails:** no second DutyRoster; no second rationing allocator; no second medical-priority engine; no second shelter-housing authority; no second faction system for external factions; no arbitrary survivor “vote” RNG detached from beliefs and current state; no governance-owned morale; no governance-owned market prices; no governance-owned moral band; no generic prison system unless a real location/availability/incapacitation authority supports it; no direct inventory confiscation without canonical inventory transactions; no “anarchy = random chaos” heartbeat; no governance type that magically applies global multipliers without explicit adapters; no policy that can silently softlock food, medicine, quests, defense, or immigration; no wall-clock/GUID/unseeded randomness; no player-facing bureaucracy for settings that already have a better direct control surface.

---

# 0. Mission

ASHFALL already has pieces of governance.

The source baseline points to:
- `LeadershipSystem`;
- `DutyRosterSystem`;
- `MoralChoiceSystem`;
- `MarketSystem`;
- survivor relationships;
- ideological-friction work;
- survivor autonomy;
- earlier Plan-43 governing-together work.

Yet the source correctly identifies that most shelter decisions are still effectively:

```text
PLAYER DECIDES
    │
    ▼
SYSTEM CHANGES
```

without an explicit model for:

```text
who is authorized to decide?
what rule/policy is currently in force?
who supports it?
who opposes it?
what procedure changes it?
what happens when rules conflict with survivor autonomy?
what record persists when leadership changes?
```

The target architecture is:

```text
SHELTER CONDITIONS / SURVIVOR BELIEFS / LEADERSHIP
          │
          ▼
Governance Authority
          │
          ├── governance mode / decision procedure
          ├── enacted laws
          ├── active policies
          ├── representation/council state
          ├── support/opposition projection
          ├── pending governance decisions
          └── governance history
          │
          ▼
TYPED POLICY ADAPTERS
          │
          ├────────► rationing authority
          ├────────► DutyRosterSystem
          ├────────► housing/room authority
          ├────────► medical triage/ward authority
          ├────────► MarketSystem / trade stance
          ├────────► shelter defense authority
          ├────────► visitor/immigration authority
          ├────────► SurvivorRelationsSystem
          ├────────► MoralChoiceSystem
          ├────────► autonomy/refusal authority
          └────────► event / journal / briefing
```

Governance should answer:

> Which rule is in force, how was it adopted, who accepts or rejects it, and which existing systems should interpret that rule?

Governance should **not** answer:
- how much food exists;
- who actually works a shift;
- how medicine is consumed;
- what a market price is;
- whether a survivor physically fights;
- how morale is calculated.

---

# 1. Re-Baseline Against Existing Governance-Like Work

## 1.1 Plan 43 overlap is mandatory

Before creating `GovernanceSystem.cs`, inspect the implementation resulting from Plan 43 (“Governing Together”).

Determine whether current repository already contains:
- councils;
- votes;
- consent/approval;
- leadership authority;
- group decision state;
- governance history;
- refusal/legitimacy seams.

If yes:
- Plan 159 is an expansion of that system.

If no:
- document why Plan 43 did not create a reusable governance owner.

## 1.2 Leadership is not governance

`LeadershipSystem` may own:
- designated leader;
- leader stress;
- succession.

Governance may read leadership but must not duplicate it.

## 1.3 Ideological friction is not political organization

Plan 148 may supply:
- belief tags;
- disagreement pressure;
- ideological compatibility.

Plan 159 may aggregate these into temporary political blocs/coalitions.

Do not duplicate the underlying beliefs.

## 1.4 Survivor autonomy is not a legislature

Plan 144 may own:
- refusal;
- autonomous behavior;
- self-directed choices.

Governance policy may influence the **conditions** under which refusal happens, but the autonomy system remains authority for actual refusal.

## 1.5 Laws and policies must be separated from operational settings

Example:

```text
LAW:
"Minimum emergency ration guaranteed"

POLICY:
"Food allocation priority: children / sick / equal / workers"

OPERATION:
actual daily ration quantities
```

The first two belong to governance; the third belongs to food/ration systems.

## 1.6 Justice is especially high-risk for duplication

The source proposes:
- crime;
- investigation;
- evidence;
- witness testimony;
- trial;
- sentencing.

Do not create a generic court simulation unless the repository already has:
- incidents/crimes;
- evidence records;
- survivor availability/sanctions.

MVP justice should begin with **dispute/case adjudication over real existing incidents**.

---

# 2. Non-Negotiable Governance Invariants

## INV-159.1 — One governance authority

Decision procedure, enacted law, active policy, and governance-history have one owner.

## INV-159.2 — Existing operational systems keep ownership

Governance publishes policy constraints/adapters; it does not own downstream state.

## INV-159.3 — Leadership remains separate

Leader identity/succession stays in `LeadershipSystem` or current canonical authority.

## INV-159.4 — Ideology remains separate

Political blocs derive from survivor beliefs; they do not own beliefs.

## INV-159.5 — Faction terminology is disambiguated

Internal shelter political blocs must not use IDs/namespaces that can be confused with external world factions.

Prefer:
- `PoliticalBloc`;
- `Caucus`;
- `CouncilGroup`.

## INV-159.6 — No magical global governance multipliers

A law has effects only through typed adapters.

## INV-159.7 — Every policy parameter has one consumer

No authored policy without a live downstream read.

## INV-159.8 — Policy changes are transactional

Enact/repeal cannot leave half-applied state.

## INV-159.9 — Decision procedures are deterministic

Same:
- membership;
- beliefs;
- support;
- rule;
- seed where tie-break uncertainty is authored
=> same outcome.

## INV-159.10 — No random voting detached from state

Voting behavior derives from:
- preferences;
- relationships;
- current shelter conditions;
- explicit uncertainty.

## INV-159.11 — Support is not morale

Policy support/opposition is governance-specific preference state or derivation.

## INV-159.12 — “Stability” is not a duplicate morale/order meter by default

Only create it if ADR proves it cannot be derived from:
- legitimacy;
- refusal;
- ideological conflict;
- violence/disputes;
- policy compliance.

## INV-159.13 — Justice cases originate from real incidents

No random crime generator inside governance.

## INV-159.14 — Verdict does not invent guilt truth

If evidence is uncertain:
- decision is governance judgement;
- incident facts remain historical facts.

## INV-159.15 — Sanctions use existing domain effects

Fine → inventory/economy.
Exile → survivor/world lifecycle.
Community service → duty/commitment.
Restriction → availability.
Imprisonment only if real system exists.

## INV-159.16 — Governance cannot silently remove critical capabilities

Every restrictive law has:
- validation;
- fallback;
- explicit consequences.

## INV-159.17 — Political blocs are bounded

No N×N coalition explosion.

## INV-159.18 — Government history persists across leadership change

Law/policy history survives leader replacement.

## INV-159.19 — UI presents consequences before enactment

Known operational impacts are previewed.

## INV-159.20 — Save/load cannot re-enact policies

Restore reconstructs state without replaying enactment side effects.

---

# 3. Definition of Done

Plan 159 closes only when:

- Plan-43/current governance overlap is audited;
- leadership, ideology, autonomy, duty, rationing, medical, market, housing, visitor/immigration, defense, relationships and moral-choice authorities are mapped;
- one governance owner is selected;
- governance mode/decision procedure is explicit;
- laws and policies are distinct concepts;
- every shipped law/policy has a typed downstream consumer;
- unsupported policy categories are deferred rather than simulated with generic multipliers;
- four governance modes are implemented only if materially distinct and supported; otherwise fewer modes ship honestly;
- internal political groups are called blocs/caucuses rather than external factions;
- bloc membership derives from real ideology/belief state;
- influence/support is bounded and explainable;
- council/vote outcomes derive from state;
- player fiat remains available only where governance mode allows;
- governance change has a canonical transition path;
- justice cases originate from real incidents;
- justice sanctions route through existing authorities;
- imprisonment is absent unless an actual holding/incapacitation system exists;
- laws can be enacted/repealed idempotently;
- policy previews match actual downstream behavior;
- old saves receive a safe default governance mode;
- no policy effect is duplicated on restore;
- no hand-authored “stability” meter duplicates existing social state without ADR;
- governance UI shows mode, active laws, policies, support, pending decisions, and case status;
- 20 laws / 15 policies are authored only after all referenced effect seams exist; otherwise staged definitions are explicitly excluded from gameplay acceptance;
- `--governance-selftest` exists or equivalent;
- 30/120/180-day simulations show governance changes behavior without making direct operations unusable;
- content acceptance/reachability proves enacted definitions actually produce effects;
- all governance IDs, policy parameters, bloc predicates and sanction references validate.

---

# 4. Phase P0 — Governance Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
LeadershipSystem APIs/state
Plan-43 governance/council artifacts
DutyRosterSystem
ration/food allocation authority
housing/room assignment authority
medical triage authority
MarketSystem / trade policy
visitor/refugee/immigration authority
shelter defense authority
MoralChoiceSystem
IdeologicalFrictionSystem
SurvivorRelationsSystem
survivor autonomy/refusal APIs
commitment/deadline system
event/journal history
save sections
```

## P0.2 Build governance authority matrix

Create:

`docs/governance/GOVERNANCE_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
governance may own?
adapter required?
status
```

Rows:
- leader;
- council membership;
- governance mode;
- law;
- policy;
- ration quantity;
- duty assignment;
- room assignment;
- medical priority;
- trade restriction;
- visitor admission;
- defense readiness;
- ideology;
- refusal;
- support/opposition;
- disputes;
- sanctions;
- moral consequence;
- journal history.

## P0.3 Audit Plan 43

Produce explicit disposition:

```text
REUSE
EXTEND
MIGRATE
DELETE DUPLICATE
MISSING
```

for each governance-like seam.

## P0.4 Audit naming collision

External:
- faction.

Internal:
- political bloc/caucus.

Reserve namespaces.

## P0.5 Baseline reproduction

Demonstrate:
- policy-like player choices are currently direct system settings;
- no persistent law/decision-procedure layer.

---

# TASK 159A — Governance Core: Mode, Law, Policy & Decision Contract

# 159A.0 Goal

Create the smallest governance authority that can represent legitimate shelter-wide rules and route them to existing systems.

## 159A.1 Prefer extending current Plan-43 authority

Only create new:

`Assets/Ashfall.Core/Shelter/GovernanceSystem.cs`

if no current canonical owner exists.

## 159A.2 Governance state

Suggested:

```text
schema_version
mode
active_law_ids[]
active_policy_values{}
pending_decisions[]
council_state if needed
history_refs[]
```

Do not store duplicate operational facts.

## 159A.3 Governance modes

Candidate:

```text
leader_rule
council
assembly
informal
```

These may map approximately to source:
- autocracy;
- council;
- democracy;
- anarchy.

Prefer terminology consistent with ASHFALL fiction.

## 159A.4 Mode must change procedure

A mode is valid only if it changes:
- proposer rights;
- approval process;
- timing;
- repeal process;
- representation.

No decorative labels.

## 159A.5 Informal mode

“Anarchy” should not mean random chaos.

It means:
- no formal policy procedure;
- direct existing operational controls;
- no institutional legitimacy modifiers.

## 159A.6 Law DTO

Suggested:

```text
law_id
category
status
enacted_day
repealed_day
decision_id
effect_refs[]
```

Descriptions/localization live in catalog.

## 159A.7 Policy state

Suggested:

```text
policy_id
parameter_values
active
effective_day
decision_id
```

Do not persist `affectedSurvivors[]` if derivable.

## 159A.8 Law vs policy

Law:
- normative/structural rule.

Policy:
- current operational priority/settings.

## 159A.9 Governance policy catalog

`governance_policies.json`

Versioned.

## 159A.10 Law catalog

Prefer separate:
`governance_laws.json`.

Do not overload one file if schemas differ.

## 159A.11 Typed effect adapters

Examples:

```text
ration_policy
duty_policy
medical_priority
admission_policy
trade_policy
defense_policy
housing_policy
```

Only real adapters.

## 159A.12 No generic string→reflection effect engine

Use typed registry.

## 159A.13 Enact transaction

Steps:

1. validate definition;
2. validate decision procedure;
3. validate downstream adapter acceptance;
4. commit governance state;
5. notify adapters;
6. emit event/history;
7. refresh UI.

## 159A.14 Repeal transaction

Same discipline.

## 159A.15 Adapter failure

Enactment fails atomically.

No half-active law.

## 159A.16 Policy conflict validation

Examples:
- two mutually exclusive ration policies;
- two incompatible admission policies.

Catalog declares conflict groups.

## 159A.17 Policy precedence

Avoid hidden precedence.

Use:
- one active policy per policy slot;
or explicit priority.

## 159A.18 Effective day

Some changes:
- immediate;
- next day;
- next schedule cycle.

Definition-specific.

## 159A.19 Cooldown

Only when changing policy too often would create exploit/instability.

Data-driven.

## 159A.20 No arbitrary cooldown for all laws

Major laws may require process delay rather than cooldown.

## 159A.21 Support preview

Governance core may query support projection before decision.

## 159A.22 No support storage if purely derivable

Only persist:
- historical vote;
- pledged bloc position
if needed.

## 159A.23 Decision ID

Stable deterministic ID.

No GUID.

## 159A.24 Governance history

Record:
- proposed;
- enacted/rejected;
- repealed;
- mode changed;
- case adjudicated.

Compact.

## 159A.25 Old save

Default:
- current player-fiat behavior represented as `leader_rule` or `informal`, based on existing Plan-43 semantics.

Do not surprise old saves with immediate voting restrictions.

## 159A.26 Restore

No adapter side-effect replay.

Adapters read active policies after restore.

## 159A.27 Generated contract docs

Create:
- `GOVERNANCE_LAW_MATRIX.md`;
- `GOVERNANCE_POLICY_MATRIX.md`.

### 159A DoD

Governance can persist formal rules and decision procedure while every actual shelter operation remains owned by its existing system.

---

# TASK 159B — Policy Adapters Into Real Shelter Operations

# 159B.0 Goal

Make laws/policies matter by configuring existing systems rather than replacing them.

---

# 159B-R — Resource Allocation / Rationing

## 159B.R1 Audit ration authority

Find actual:
- food;
- water;
- priority;
- emergency ration
system.

## 159B.R2 Policy examples

Only if authority supports:

```text
equal_rations
priority_medical
priority_children_if_children_system_real
priority_workers
emergency_minimum
```

Do not reference children if no child system is live.

## 159B.R3 Governance supplies priority rule

Ration system calculates actual quantity.

## 159B.R4 No food creation

Policy redistributes scarcity.

## 159B.R5 Minimum guarantee

Must fail/scale visibly if resources physically insufficient.

## 159B.R6 Preview

Show projected groups affected.

---

# 159B-W — Work Assignment

## 159B.W1 DutyRoster remains authority

Policy may configure:
- compulsory vs voluntary assignment;
- priority categories;
- maximum shift;
- refusal consequences only through autonomy/commitment systems.

## 159B.W2 No direct roster replacement

## 159B.W3 Survivor autonomy

Mandatory work policy increases:
- refusal/resentment input
only if autonomy system has explicit seam.

## 159B.W4 No random noncompliance inside governance

## 159B.W5 Fitness safety

Governance cannot force medically unfit survivor into invalid duty if fitness authority prohibits it.

---

# 159B-H — Housing

## 159B.H1 Precondition

Only if room/bunk assignment authority exists.

## 159B.H2 Policy

May configure:
- household priority;
- quarantine;
- occupancy priority.

## 159B.H3 No new housing simulator

If no assignment policy seam:
- defer housing laws.

---

# 159B-M — Medical Priority

## 159B.M1 Medical system remains authority

Policy may alter:
- triage priority weights;
- scarce treatment priority.

## 159B.M2 Safety floor

Cannot override:
- contraindications;
- impossible treatment;
- quarantine rules.

## 159B.M3 Moral consequence

A discriminatory/harsh priority may feed MoralChoice only through explicit authored effect.

---

# 159B-I — Immigration / Admission

## 159B.I1 Find visitor/refugee authority

If absent:
- defer formal immigration policy.

## 159B.I2 Possible policy

```text
open
screened
capacity_limited
faction_restricted
closed
```

Only if visitor system supports.

## 159B.I3 Admission decision

Visitor system applies policy.

## 159B.I4 Exceptions

Quest/scripted humanitarian cases may override through explicit authored branch.

---

# 159B-D — Defense Policy

## 159B.D1 Shelter defense authority required

Policy may control:
- militia participation;
- weapon access;
- readiness priority.

## 159B.D2 No defense score in governance

## 159B.D3 Survivor fitness/autonomy

Still enforced.

---

# 159B-E — Economic / Trade Policy

## 159B.E1 MarketSystem remains authority

Policy may configure:
- trade restrictions;
- reserve thresholds;
- export bans;
- rationed categories
if current economy supports.

## 159B.E2 No direct price multipliers unless MarketSystem exposes policy input

## 159B.E3 Black-market interaction

Plan 155 contraband/underground trade may react to restrictions.

Do not duplicate black-market logic.

---

# 159B-S — Social / Education / Religion / Family

## 159B.S1 Default disposition

DEFER unless current systems have real consumers.

## 159B.S2 No fake effect

Do not ship:
- education requirement;
- religious freedom;
- family planning
as generic morale modifiers if corresponding mechanics do not exist.

### 159B DoD

Every shipped law/policy changes a real shelter authority through a narrow adapter, and every unsupported source category is deferred honestly.

---

# TASK 159C — Representation, Political Blocs, Debate & Decision Procedures

# 159C.0 Goal

Give survivors a political voice without duplicating ideology, relationships, leadership, or autonomy.

## 159C.1 Internal group type

Use:
`PoliticalBloc`.

Not `Faction`.

## 159C.2 Bloc DTO

Suggested:

```text
bloc_id
ideology_tags[]
leader_id optional
member_ids[]
influence
agenda_tags[]
formation_day
status
```

## 159C.3 Membership source

Derived from:
- Plan-148 beliefs;
- relationships;
- current grievances;
- survivor priorities.

Avoid arbitrary assignment RNG.

## 159C.4 Dynamic vs persistent membership

Persist membership only if:
- bloc identity/history matters.

Otherwise recompute eligibility and persist explicit join/leave choices.

## 159C.5 Five source ideologies

Do not automatically hardcode:
- authoritarian;
- democratic;
- libertarian;
- collectivist;
- individualist
as mutually exclusive universal identities.

Prefer reusable belief tags already present.

## 159C.6 Bloc formation threshold

Require:
- at least N aligned survivors;
- shared agenda;
- stable membership.

Data-driven.

## 159C.7 No one-person “faction”

Unless named political advocate is intentionally modeled as caucus leader without bloc.

## 159C.8 Influence

Derive from:
- membership;
- leadership;
- relationships;
- credibility/results
using existing metrics.

Avoid generic propaganda points.

## 159C.9 Propaganda

Default:
- DEFER.

If speeches/posters already exist as morale/narrative actions:
- may influence support via current systems.

## 159C.10 Alliances

Coalitions can be a temporary vote support relation.

Do not create permanent diplomatic graph among shelter blocs initially.

## 159C.11 Coercion

Default:
- DEFER to specific authored events/autonomy/violence systems.

No generic intimidation button.

## 159C.12 Agenda

Data tags such as:
- ration equality;
- defense priority;
- open admission;
- strict discipline.

## 159C.13 Support projection

For a proposed law:

```text
survivor preference
+ bloc agenda
+ current shelter condition
+ personal trust/leadership
→ support score
```

## 159C.14 No opaque support magic

Provide reason trace.

## 159C.15 Debate

Debate is:
- pre-decision presentation;
- relationship/argument hooks if existing dialogue supports.

No full debate minigame required.

## 159C.16 Voting

If mode uses vote:
- eligible voter set;
- deterministic support/choice;
- seeded tie/uncertainty only if necessary.

## 159C.17 Secret ballot

Only affects UI/information, not random choice.

## 159C.18 Council

Council membership comes from:
- existing election/selection;
- or authored appointment process.

## 159C.19 Leader rule

Player/leader can enact within authority.

Opposition still affects:
- refusal;
- support;
- future politics
through real systems.

## 159C.20 Assembly

Survivor vote.

## 159C.21 Informal

No formal vote.

Existing direct shelter operations remain.

## 159C.22 Decision timing

Major decisions may take:
- proposal day;
- debate;
- vote next day.

Campaign-time.

## 159C.23 Emergency override

If existing crisis/leadership system supports:
- emergency decree with legitimacy/opposition consequence.

## 159C.24 No indefinite pending queue

Pending decisions have:
- deadline;
- cancellation;
- resolution.

## 159C.25 Bloc memory

Use:
- governance history;
- Plan-147 NPC memory
for named personal reactions.

Do not add separate “faction memory” if unnecessary.

## 159C.26 Bloc dissolution

If membership falls below threshold:
- historical archive;
- no active processing.

## 159C.27 No revolt mechanic in core

High opposition may produce:
- refusal;
- protest event;
- leadership challenge
only if owning systems exist.

Revolt/armed conflict is 159F follow-on.

### 159C DoD

Governance procedures reflect real survivor beliefs and leadership without becoming an independent ideology or social simulation.

---

# TASK 159D — Justice & Dispute Resolution

# 159D.0 Goal

Create a bounded adjudication layer over real incidents instead of a free-standing crime simulator.

## 159D.1 Incident sources

Eligible cases only from real systems:

```text
theft event
violence event
broken commitment
resource misuse
duty refusal if policy defines violation
corruption only if resource/leadership event proves it
```

## 159D.2 No random crime generator in governance

## 159D.3 Case DTO

Suggested:

```text
case_id
source_event_id
case_type
accused_id
affected_ids[]
opened_day
evidence_refs[]
status
verdict
sanction_id
resolved_day
```

## 159D.4 Typo correction

Source uses `innocate`.

Canonical enum:
- `guilty`;
- `not_proven` / `not_guilty`;
- `pending`.

Choose terminology that fits uncertainty.

## 159D.5 Evidence

Reference existing event records.

Do not fabricate witness testimony system if absent.

## 159D.6 Testimony

Only if dialogue/knowledge systems already support witness statements.

## 159D.7 Investigation

MVP may simply expose:
- known evidence;
- unresolved facts.

No detective minigame.

## 159D.8 Adjudicator

Depends on governance mode:
- leader;
- council;
- assembly/jury only if implemented.

## 159D.9 Jury

Default:
- DEFER unless decision/vote infrastructure makes it trivial.

## 159D.10 Verdict is decision state

Does not rewrite source event.

## 159D.11 Sanction definitions

Catalog-driven.

## 159D.12 Fine

Use canonical resource transaction.

## 159D.13 Community service

Use:
- commitment/duty assignment
with finite term if supported.

## 159D.14 Exile

Use survivor lifecycle/world departure authority.

## 159D.15 Imprisonment

Default:
- DEFER unless a canonical detention/unavailability state exists.

## 159D.16 Restitution

Prefer over punishment where:
- inventory/commitment supports.

## 159D.17 Warning/censure

Low-cost sanction:
- governance history;
- relationship effect
if real.

## 159D.18 No generic punishment multiplier

## 159D.19 Justice policy

Law may determine:
- available sanctions;
- burden/threshold;
- who adjudicates.

## 159D.20 No retrospective law

Incident evaluated under:
- law in force at incident day
unless game explicitly allows retroactive rule.

## 159D.21 Double jeopardy/idempotence

One source incident cannot generate repeated resolved cases.

## 159D.22 Appeal

Follow-on unless decision system supports.

## 159D.23 Wrongful verdict

Can create:
- relationship/moral consequence
only if truth/evidence system can establish discrepancy.

## 159D.24 Rehabilitation/deterrence

Do not create generic statistical “crime rate” from justice unless behavior system consumes it.

## 159D.25 Journal

Record major case outcome.

### 159D DoD

Justice resolves real shelter incidents through explicit procedures and canonical sanctions without creating an unrelated crime, prison, or investigation simulator.

---

# TASK 159E — UI, Persistence, Determinism, Balance, Reachability & CI

# 159E.0 Goal

Make governance understandable, durable, bounded, and mechanically honest.

## 159E.1 UI route decision

Audit:
- existing leadership;
- council;
- shelter settings;
- records.

A governance panel is justified only if the feature set cannot fit a current administration/shelter surface.

## 159E.2 Core governance summary

Show:
- governance mode;
- leader/council;
- active laws;
- active policies;
- pending decisions.

## 159E.3 Policy preview

Before enactment show:
- operational systems affected;
- expected support;
- known costs/risks;
- effective date.

## 159E.4 No fake exact prediction

If support/outcome uncertainty exists:
- show band/range/reasons.

## 159E.5 Bloc view

Show:
- name;
- agenda;
- members;
- influence;
- position on current proposal.

## 159E.6 Avoid ideological caricature

Use agenda and concrete policy preferences.

## 159E.7 Justice view

Show:
- case;
- known evidence;
- decision;
- available sanctions.

## 159E.8 History

Timeline:
- enacted;
- repealed;
- votes;
- reforms;
- major cases.

## 159E.9 Journal

Only major governance events.

## 159E.10 Briefing

Pending:
- vote;
- policy change;
- unresolved case;
- major opposition.

## 159E.11 Tutorial

Only current tutorial framework.

## 159E.12 Accessibility

No color-only support/opposition.

## 159E.13 Localization

All law/policy/decision/reason strings keyed.

---

# 159E-P — Persistence

## 159E.P1 Persist new governance facts only

Potential:

```text
mode
active laws
policy values
pending decisions
council/bloc state if non-derivable
justice cases
history refs
processed decision IDs
```

## 159E.P2 Do not persist duplicate operational outputs

No:
- ration allocations;
- work assignments;
- market prices;
- morale totals;
- medical outcomes
inside governance state.

## 159E.P3 Old save

Safe compatibility mode.

## 159E.P4 Mid-vote save

Same:
- proposal;
- voters;
- state;
- due day.

## 159E.P5 Mid-case save

Same evidence/verdict state.

## 159E.P6 Reload idempotence

No duplicate:
- law adapter;
- policy effect;
- moral consequence;
- fine;
- exile;
- community-service commitment.

---

# 159E-D — Determinism

## 159E.D1 Policy support

Same state:
- same projection.

## 159E.D2 Vote

Same:
- voter set;
- beliefs;
- relationships;
- seed
=> same outcome.

## 159E.D3 No wall clock

## 159E.D4 Stable iteration

Sort survivor/bloc/law IDs.

## 159E.D5 Headless

All procedures work.

---

# 159E-B — Balance

## 159E.B1 Governance must not dominate direct survival play

Decision cadence bounded.

## 159E.B2 Decision budget

Target:
- major governance choice infrequently;
- policies persist long enough to matter.

## 159E.B3 No daily vote spam

## 159E.B4 Mode tradeoffs

Avoid generic:
- autocracy bad;
- democracy good.

Tradeoffs must come from:
- decision speed;
- legitimacy/support;
- representation;
- emergency flexibility.

## 159E.B5 Informal mode viable

Player can largely ignore formal governance early.

## 159E.B6 Formalization unlock

Complexity can unlock after:
- population threshold;
- story milestone;
- council formation.

## 159E.B7 No impossible policy stack

Validation.

## 159E.B8 No food death spiral from policy without warnings

Simulate.

## 159E.B9 No permanent work collapse from unpopular policy

Autonomy/refusal remains bounded/recoverable.

## 159E.B10 Justice burden

Cases cannot flood player attention.

Use case budget/priority.

## 159E.B11 Political bloc cap

Bound active blocs.

## 159E.B12 No one survivor forms five blocs

Membership rules clear.

## 159E.B13 Governance change cooldown/process

Prevent instant mode-toggle exploits.

## 159E.B14 Moral farming

Repeated repeal/enact cannot farm moral effects.

## 159E.B15 Trade exploit

Policy toggles cannot arbitrage market prices instantly.

## 159E.B16 Duty exploit

Policy toggles cannot bypass assignment constraints.

---

# 159E-S — Long-Horizon Simulation

## 159E.S1 30-day informal/leader-rule scenario

Track:
- policies;
- opposition;
- decisions.

## 159E.S2 120-day council scenario

Track:
- proposal success;
- bloc diversity;
- policy duration;
- refusal/compliance.

## 159E.S3 180-day high-friction scenario

Assert:
- shelter still operational;
- no infinite case/proposal backlog;
- no governance-state explosion.

## 159E.S4 Mode transition scenario

Leader rule → council/assembly.

## 159E.S5 Leadership death

Governance law/policy history survives.

## 159E.S6 Ideology swing

Bloc membership updates deterministically.

## 159E.S7 No-governance/informal edge

Shelter remains playable.

## 159E.S8 Maximum formalization edge

Many laws/policies do not over-stack effects.

---

# 159E-T — Testing & CI

## 159E.T1 Data integrity

Validate:
- law IDs;
- policy IDs;
- effect adapter IDs;
- policy conflict groups;
- bloc agenda tags;
- sanction IDs;
- localization keys.

## 159E.T2 Selftest

Create:

```text
--governance-selftest
```

## 159E.T3 Selftest scenarios

At least:
1. old save/default governance;
2. enact policy;
3. repeal policy;
4. conflicting policy rejected;
5. council vote;
6. leader-rule enactment;
7. ideological support projection;
8. duty-policy adapter;
9. ration-policy adapter;
10. economic-policy adapter if real;
11. justice case;
12. sanction;
13. save/load;
14. mode transition.

## 159E.T4 Adapter identity tests

Every policy effect has one downstream authority.

## 159E.T5 Source-scan gate

Detect:
- governance-owned duty assignments;
- governance-owned ration inventory;
- governance-owned market prices;
- governance-owned morale;
- duplicate ideology storage;
- generic imprisonment state if unsupported.

## 159E.T6 Content acceptance

Gameplay laws/policies reach:
`EFFECT_PRODUCED`.

## 159E.T7 Reachability

Every shipped definition can:
- be proposed;
- pass/reject through a real procedure;
- affect its real consumer.

## 159E.T8 Failure fixtures

Deliberately:
- invalid adapter;
- conflicting policy;
- invalid sanction;
- unresolved survivor ID.

## 159E.T9 Generated docs

Create:
- `GOVERNANCE_ARCHITECTURE.md`;
- `GOVERNANCE_AUTHORITY_MATRIX.md`;
- `GOVERNANCE_LAW_MATRIX.md`;
- `GOVERNANCE_POLICY_MATRIX.md`;
- `GOVERNANCE_SCOPE_DISPOSITION.md`;
- `GOVERNANCE_BALANCE_REPORT.md`.

### 159E DoD

Governance remains a legible policy/decision layer over existing shelter systems, survives long campaigns, and cannot duplicate or bypass operational authorities.

---

# TASK 159F — Advanced Political Institutions: Explicit Follow-On Gate

# 159F.0 Goal

Keep Plan 159 from becoming an unbounded political simulator before the base layer is proven.

## 159F.1 Elections

Implement only if:
- council/office terms;
- candidate selection;
- voting;
- leadership transition
are already clean.

Otherwise defer.

## 159F.2 Constitutions

MVP may treat “constitution” as:
- a named set of structural laws / mode change.

Do not create document-authoring subsystem.

## 159F.3 Revolts

Default:
- DEFER.

Requires:
- refusal/unrest;
- faction/bloc organization;
- combat/leadership consequences;
- world-state handoff.

## 159F.4 Rebellion suppression

Not part of core governance.

## 159F.5 Purges

Reject as a generic mechanic.

If a narrative choice involves removal of corrupt officials, use:
- specific dismissal/exile/justice rails,
not a reusable purge system.

## 159F.6 Propaganda

Follow-on if:
- speeches/posters/media systems exist.

## 159F.7 Coercion / blackmail

Follow-on through:
- dialogue;
- NPC memory;
- black-market/intel;
- moral-choice
if those rails exist.

## 159F.8 Professional judges/politicians

Only if survivor job/specialization system supports.

## 159F.9 Governance diplomacy

Inter-shelter agreements are a separate external diplomacy plan.

## 159F.10 Legacy

Epilogue can derive:
- governance mode;
- landmark laws;
- major cases;
- leadership transitions.

No separate legacy simulator.

### 159F DoD

Advanced institutions remain explicit, evidence-gated follow-ons rather than accidental complexity hidden inside the governance MVP.

---

# 5. Governance State Model

Conceptual:

```text
INFORMAL / LEADER RULE
        │
        └── formalization
              ▼
        COUNCIL / ASSEMBLY
              │
              ├── proposal
              ├── support projection
              ├── decision
              ├── enactment
              └── repeal/reform
```

Mode changes procedure, not arbitrary bonuses.

---

# 6. Law vs Policy vs Operation Contract

```text
LAW
  durable normative rule

POLICY
  current configuration/priority

OPERATION
  actual domain action/result
```

Example:

```text
Law: emergency ration guarantee
Policy: prioritize sick
Operation: FoodSystem allocates 0.7 ration today
```

---

# 7. Governance Mode Contract

Every mode definition must specify:

```text
who may propose?
who decides?
how long does decision take?
what quorum/approval applies?
who may repeal?
what emergency power exists?
```

No decorative governance modes.

---

# 8. Political Bloc Contract

Political bloc is:
- internal;
- survivor-composed;
- agenda-driven.

External faction is:
- world entity.

Do not share namespaces or APIs casually.

---

# 9. Support Projection Contract

Support is explainable:

```text
belief alignment
+ personal/leader trust
+ current conditions
+ affected-group impact
+ bloc agenda
```

No random popularity roll.

---

# 10. Vote Contract

A vote is a resolution mechanism.

The system records:
- proposal;
- eligible voters;
- votes;
- outcome.

It does not replace survivor ideology/relationship state.

---

# 11. Emergency Governance Contract

If emergency override exists:

```text
crisis state
→ temporary authority expansion
→ explicit duration/review
→ support/legitimacy consequence
```

No permanent emergency mode through bug/sticky flag.

---

# 12. Ration Policy Contract

Governance supplies priority constraints.

Food authority owns:
- stock;
- consumption;
- delivered ration.

---

# 13. Duty Policy Contract

Governance supplies:
- voluntary/mandatory;
- priority rules.

DutyRoster owns:
- assignments;
- fitness;
- scheduling.

---

# 14. Medical Policy Contract

Governance can influence scarce-resource priority.

Medical system owns:
- diagnosis;
- contraindications;
- treatment;
- ward state.

---

# 15. Trade Policy Contract

Governance can supply:
- export restriction;
- reserve floor;
- trade permission.

Market/trade systems own:
- price;
- inventory;
- transaction.

---

# 16. Admission Policy Contract

Governance may set criteria.

Visitor/immigration system owns:
- actual person/party;
- admission execution.

---

# 17. Defense Policy Contract

Governance may set:
- service requirement;
- resource priority.

Defense/combat systems own:
- readiness;
- assignment;
- resolution.

---

# 18. Justice Case Contract

A case must answer:

```text
what real incident created it?
which law applied at that time?
what evidence is known?
who decides?
what verdict was chosen?
which canonical sanction is requested?
```

---

# 19. Sanction Contract

Preferred MVP sanctions:

```text
warning/censure
restitution
fine
community-service commitment
exile
```

Imprisonment only if infrastructure exists.

---

# 20. No Retroactive Law Contract

Default:

```text
incident_day
→ applicable law snapshot/version
```

No punishment for conduct legal when performed unless explicitly authored otherwise.

---

# 21. Stability / Legitimacy ADR

Before adding `stability 0..100`, answer:

```text
What gameplay fact does it represent?
Which current facts cannot derive it?
Who consumes it?
Why is morale/unrest/support insufficient?
```

If no answer:
- do not add stability meter.

---

# 22. Policy Effect Adapter Contract

Each adapter provides:

```text
policy slot
validation
preview
apply/read configuration
effective timing
diagnostics
```

Governance does not mutate downstream private fields.

---

# 23. Persistence Matrix

| Fact | Owner |
|---|---|
| governance mode | governance |
| law status | governance |
| policy value | governance |
| proposal/vote | governance |
| bloc state | governance if persistent |
| ideology | ideological-friction/survivor state |
| leader | leadership |
| duty assignment | DutyRoster |
| food stock/ration result | food/inventory |
| medical result | medical |
| trade price | MarketSystem |
| moral band | MoralChoice |
| survivor relationship | relations |
| refusal | autonomy |
| sanction outcome | owning systems |
| governance history | governance/journal |

---

# 24. Old-Save Migration

Default mode must preserve existing control style.

Recommended:

```text
if Plan-43 formal governance already exists:
    migrate current mode
else:
    leader_rule or informal
```

No sudden election blocking old-save actions.

---

# 25. Idempotence Contract

Stable IDs:

```text
decision_id
law_enactment_id
policy_change_id
case_id
sanction_source_id
```

Reload cannot replay downstream effect.

---

# 26. Failure Injection Matrix

## N159.1 Governance directly changes food inventory
Expected: authority gate fails.

## N159.2 Governance stores duty assignments
Expected: authority gate fails.

## N159.3 Law references adapter with no consumer
Expected: integrity/content-acceptance fail.

## N159.4 Democracy vote uses random coin flip despite deterministic preferences
Expected: decision determinism fails.

## N159.5 Internal bloc stored as external faction ID
Expected: namespace gate fails.

## N159.6 Policy forces medically unfit survivor into duty
Expected: fitness-policy integration fails.

## N159.7 Old save suddenly enters assembly mode and loses direct control
Expected: migration compatibility fails.

## N159.8 Reopening save reapplies ration-policy side effect
Expected: idempotence fails.

## N159.9 Imprisonment implemented without detention/unavailability authority
Expected: scope/architecture gate fails.

## N159.10 Same justice incident generates multiple cases
Expected: case identity fails.

## N159.11 Repeal/enact loop farms moral rewards
Expected: exploit gate fails.

## N159.12 Unsupported education/religion policy applies generic morale multiplier
Expected: real-consumer gate fails.

---

# 27. Determinism Contract

Same:

```text
governance mode
+ proposal
+ survivor beliefs
+ relationships
+ current shelter conditions
+ bloc state
+ policy data
+ seed where uncertainty is authored
```

must produce same:
- support;
- votes;
- decision;
- effective date;
- history;
- sanction selection if procedural.

---

# 28. Long-Horizon Metrics

Track:

```text
laws enacted/repealed
policy duration
proposals accepted/rejected
votes
support distribution
active blocs
bloc membership churn
refusal events tied to policy
governance-mode changes
justice cases
sanctions
policy adapter effects
decision backlog
governance state bytes
```

---

# 29. Balance Guardrails

Governance should create:

```text
meaningful tradeoffs
shared values
institutional memory
political friction
```

It should not create:

```text
daily bureaucracy
dozens of overlapping sliders
random punishment
opaque global buffs/debuffs
```

---

# 30. Decision Cadence Guardrails

Recommended:
- policies persist;
- major laws infrequent;
- urgent decisions event-driven.

No daily legislative maintenance.

---

# 31. Bloc Complexity Guardrails

Cap:
- active blocs;
- coalition relations;
- pending demands.

Prefer 2–3 meaningful blocs over five permanent ideology parties.

---

# 32. Justice Complexity Guardrails

MVP justice:
- real incidents;
- known evidence;
- explicit decision;
- canonical sanction.

No:
- procedural law simulator;
- unlimited testimony graph;
- prison management.

---

# 33. UI Acceptance

## Governance overview
- mode;
- leader/council;
- pending decisions.

## Laws
- active/repealed;
- actual effect consumers.

## Policies
- current values;
- operational preview.

## Blocs
- agenda;
- support;
- membership.

## Justice
- real case;
- evidence;
- available sanctions.

---

# 34. Accessibility

- support/opposition text + icon;
- no color-only political alignment;
- keyboard navigation;
- text-scale safe;
- no tiny legislative tables as sole interface.

---

# 35. Localization

All:
- law names;
- policy descriptions;
- reasons;
- debate text;
- verdict/sanction labels
keyed.

---

# 36. Content Acceptance

Definitions:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

A law/policy that only loads but has no adapter is not gameplay-complete.

---

# 37. Reachability

For every shipped law/policy:

```text
can proposal become available?
can current mode decide it?
can it enact/reject?
does its adapter consume it?
can player observe result?
```

For cases:

```text
can source incident occur?
can case open?
can decision resolve?
can sanction execute?
```

---

# 38. Performance Guardrails

Governance day processing:
- active proposals;
- active blocs;
- current cases only.

No full survivor×law×policy combinatorial scan each frame.

Support projection computed:
- on proposal;
- relevant state change;
not continuously.

---

# 39. CI / Gate Set

Recommended:

```text
governance_authority_single
governance_policy_adapter_integrity
governance_namespace_integrity
governance_decision_determinism
governance_no_duplicate_operational_state
governance_old_save_compat
governance_policy_idempotence
governance_justice_case_identity
governance_content_acceptance
governance_reachability
governance_long_horizon
governance_ui_access
```

---

# 40. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --governance-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 41. Recommended Commit Breakdown

```text
159A-1 Plan-43/leadership/ideology/autonomy authority audit
159A-2 governance terminology/namespace ADR
159A-3 governance state/mode
159A-4 law/policy catalogs
159A-5 typed adapter registry
159A-6 enact/repeal transaction
159A-7 old-save/restore/idempotence
159A-8 generated contracts/docs

159B-1 ration adapter
159B-2 duty adapter
159B-3 medical priority adapter
159B-4 admission adapter if real
159B-5 defense adapter if real
159B-6 market/trade adapter
159B-7 housing/social disposition
159B-8 adapter identity tests

159C-1 political-bloc model
159C-2 ideology-derived membership
159C-3 support projection
159C-4 leader-rule decision path
159C-5 council path
159C-6 assembly/vote path
159C-7 mode transition / emergency process
159C-8 bloc lifecycle/tests/docs

159D-1 real incident→case adapter
159D-2 evidence/reference model
159D-3 adjudication procedure
159D-4 restitution/fine
159D-5 community-service commitment
159D-6 exile adapter
159D-7 unsupported imprisonment/appeal disposition
159D-8 case tests/docs

159E-1 governance UI
159E-2 policy preview/history/briefing
159E-3 deterministic vote/decision tests
159E-4 30-day balance
159E-5 120-day council scenario
159E-6 180-day high-friction soak
159E-7 CI gates/failure fixtures
159E-8 final ship/no-ship report

159F-1 elections/revolt/constitution disposition
159F-2 propaganda/coercion disposition
159F-3 professional offices follow-on
159F-4 epilogue/legacy integration
```

---

# 42. Risk Register

## R159.1 Duplicates Plan 43 governance

Mitigation:
- mandatory overlap audit;
- extend/migrate existing owner.

## R159.2 Governance becomes bureaucracy

Mitigation:
- low decision cadence;
- policy persistence;
- progressive unlock.

## R159.3 Generic policies fake nonexistent systems

Mitigation:
- real-consumer gate;
- defer unsupported categories.

## R159.4 Internal blocs duplicate external faction model

Mitigation:
- separate type/namespace.

## R159.5 Votes feel random

Mitigation:
- belief/relationship/state-based support trace.

## R159.6 Justice balloons into crime simulator

Mitigation:
- real incident sources;
- limited sanctions;
- no prison unless real.

## R159.7 Policies create irreversible survival spiral

Mitigation:
- preview;
- safety floors;
- long-horizon tests.

## R159.8 Mode stereotypes become mechanical caricatures

Mitigation:
- procedural tradeoffs, not ideology-coded global buffs.

---

# 43. Acceptance Checklist

## P0

- [ ] Plan-43 implementation audited
- [ ] LeadershipSystem audited
- [ ] DutyRoster audited
- [ ] rationing authority audited
- [ ] housing authority audited
- [ ] medical triage audited
- [ ] MarketSystem audited
- [ ] visitor/admission authority audited
- [ ] shelter defense audited
- [ ] MoralChoice audited
- [ ] IdeologicalFriction audited
- [ ] SurvivorRelations audited
- [ ] autonomy/refusal audited
- [ ] commitments/deadlines audited
- [ ] journal/history audited
- [ ] save sections audited
- [ ] internal-vs-external faction naming resolved
- [ ] baseline direct-control behavior reproduced
- [ ] governance authority matrix published

## 159A

- [ ] existing Plan-43 owner extended if available
- [ ] governance state stores only governance facts
- [ ] governance modes materially procedural
- [ ] informal mode is not random chaos
- [ ] law DTO minimal
- [ ] policy state minimal
- [ ] law/policy distinction documented
- [ ] versioned law/policy catalogs
- [ ] typed effect adapters
- [ ] no reflection effect engine
- [ ] atomic enact
- [ ] atomic repeal
- [ ] adapter failure rolls back
- [ ] conflict groups
- [ ] one policy per slot/explicit precedence
- [ ] effective timing
- [ ] cooldowns only where justified
- [ ] support preview
- [ ] support not redundantly persisted
- [ ] stable decision IDs
- [ ] compact governance history
- [ ] old-save control preserved
- [ ] restore does not replay effects
- [ ] generated law/policy matrices

## 159B — Resource / Duty / Housing / Medical

- [ ] ration authority unchanged
- [ ] policy only sets priority
- [ ] no food creation
- [ ] insufficient-resource behavior visible
- [ ] ration preview
- [ ] DutyRoster remains owner
- [ ] work policy configures assignment rules only
- [ ] autonomy/refusal reused
- [ ] no random noncompliance
- [ ] fitness cannot be bypassed
- [ ] housing only if authority exists
- [ ] medical triage remains owner
- [ ] medical safety floor
- [ ] MoralChoice adapter only when authored

## 159B — Admission / Defense / Economy / Social

- [ ] visitor authority found before immigration policy
- [ ] admission criteria bounded
- [ ] quest exceptions explicit
- [ ] defense authority required
- [ ] no governance-owned defense score
- [ ] fitness/autonomy still apply
- [ ] MarketSystem owns trade/economy
- [ ] no direct price arithmetic
- [ ] Plan-155 underground interaction reused
- [ ] unsupported education/religion/family policies deferred
- [ ] no generic morale stand-ins

## 159C

- [ ] internal type named PoliticalBloc/Caucus
- [ ] bloc IDs separate namespace
- [ ] membership derives from beliefs
- [ ] no arbitrary RNG membership
- [ ] five source ideologies not blindly hardcoded
- [ ] formation threshold
- [ ] no meaningless one-person blocs
- [ ] influence explainable
- [ ] propaganda deferred unless real
- [ ] coalitions bounded
- [ ] coercion deferred unless real
- [ ] agenda tags
- [ ] support projection
- [ ] reason trace
- [ ] debate lightweight
- [ ] deterministic voting
- [ ] council membership source real
- [ ] leader-rule path
- [ ] assembly path
- [ ] informal path
- [ ] campaign-time decision timing
- [ ] emergency override only if real
- [ ] pending queue bounded
- [ ] bloc memory uses existing history/NPC memory
- [ ] bloc dissolution
- [ ] revolt deferred from core

## 159D

- [ ] justice cases from real incidents
- [ ] no random crime generator
- [ ] case DTO source-attributed
- [ ] verdict terminology corrected
- [ ] evidence references real history
- [ ] witness testimony only if supported
- [ ] no detective minigame
- [ ] adjudicator follows mode
- [ ] jury deferred unless easy/real
- [ ] source event remains immutable
- [ ] sanction catalog
- [ ] fine canonical
- [ ] community service canonical
- [ ] exile canonical
- [ ] imprisonment deferred unless authority exists
- [ ] restitution supported if possible
- [ ] warning/censure bounded
- [ ] justice policy sets procedure/sanctions
- [ ] no retroactive law
- [ ] one case per source incident
- [ ] appeal disposition
- [ ] wrongful-verdict consequence only if evidence truth exists
- [ ] no fake crime-rate deterrence model
- [ ] journal major cases

## 159E — UI

- [ ] route/panel decision justified
- [ ] governance summary
- [ ] policy preview
- [ ] uncertainty represented honestly
- [ ] bloc agenda/support
- [ ] justice evidence/sanctions
- [ ] history
- [ ] journal bounded
- [ ] briefing pending items
- [ ] tutorial only if framework exists
- [ ] accessibility
- [ ] localization

## 159E — Persistence/Determinism

- [ ] only governance facts persisted
- [ ] no duplicate operational output
- [ ] old-save safe mode
- [ ] mid-vote round-trip
- [ ] mid-case round-trip
- [ ] no repeated adapters/sanctions
- [ ] deterministic support
- [ ] deterministic vote
- [ ] no wall-clock/GUID
- [ ] stable iteration
- [ ] headless

## 159E — Balance/Simulation

- [ ] governance cadence bounded
- [ ] no daily vote spam
- [ ] procedural mode tradeoffs
- [ ] informal mode viable
- [ ] complexity unlock gradual
- [ ] policy stack validates
- [ ] ration death-spiral tested
- [ ] work-collapse tested
- [ ] justice case budget
- [ ] bloc cap
- [ ] mode-toggle exploit prevented
- [ ] moral farm prevented
- [ ] market policy arbitrage prevented
- [ ] duty bypass prevented
- [ ] 30-day scenario
- [ ] 120-day scenario
- [ ] 180-day scenario
- [ ] mode transition
- [ ] leadership death continuity
- [ ] ideology swing
- [ ] informal edge
- [ ] maximum formalization edge

## 159E — CI

- [ ] law/policy/adapter integrity
- [ ] namespace integrity
- [ ] governance selftest
- [ ] adapter identity
- [ ] source-scan gate
- [ ] content acceptance
- [ ] reachability
- [ ] failure fixtures
- [ ] generated docs
- [ ] verify-fast

## 159F

- [ ] elections gated on offices/terms/votes
- [ ] constitution represented as structural law set, not editor subsystem
- [ ] revolt deferred by default
- [ ] purge mechanic rejected
- [ ] propaganda follow-on
- [ ] coercion/blackmail through existing rails only
- [ ] professional judges/politicians follow-on
- [ ] inter-shelter governance diplomacy separate
- [ ] epilogue derives from governance history

---

# 44. Ship / No-Ship Gate

**SHIP** only if:

```text
governance_authorities == 1
AND duplicate_plan43_governance_paths == 0
AND leadership_authorities == 1
AND duplicate_ideology_storage == 0
AND internal_external_faction_namespace_collisions == 0
AND laws_without_real_effect_adapter == 0
AND policies_without_real_consumer == 0
AND governance_owned_ration_inventory == false
AND governance_owned_duty_assignments == false
AND governance_owned_market_prices == false
AND governance_owned_medical_outcomes == false
AND governance_owned_morale_state == false
AND random_vote_without_state_basis == false
AND unsupported_imprisonment_state == false
AND duplicate_justice_cases_per_source_incident == 0
AND retroactive_law_enforcement == false
AND old_save_control_model_breakage == false
AND governance_restore_replays_effects == false
AND governance_content_unreachable_without_disposition == 0
AND governance_decision_determinism == pass
AND governance_old_save == pass
AND governance_save_roundtrip == pass
AND governance_policy_idempotence == pass
AND governance_content_acceptance == pass
AND governance_reachability == pass
AND governance_30_day_balance == pass
AND governance_120_day_balance == pass
AND governance_180_day_balance == pass
AND governance_selftest == pass
AND data_integrity_selftest == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 45. Implementer Handoff

1. Audit the actual result of Plan 43 before creating any new governance class.
2. Preserve `LeadershipSystem` as leader authority.
3. Preserve Plan 148 as belief/ideology authority.
4. Preserve survivor autonomy/refusal as behavior authority.
5. Rename internal political groups to blocs/caucuses to avoid external-faction confusion.
6. Separate laws, policies, and actual operations.
7. Add only policies with real downstream consumers.
8. Use typed effect adapters, never generic global multipliers.
9. Keep ration quantities, duty assignments, medical outcomes, prices, and defense state in their existing systems.
10. Make governance modes differ through procedure, not stereotype bonuses.
11. Derive support/votes from beliefs, relationships, and shelter conditions.
12. Keep debates lightweight unless dialogue already supports more.
13. Treat informal governance as legitimate early-game operation, not chaos.
14. Let formalization unlock gradually.
15. Build justice on real incidents only.
16. Prefer restitution/fine/community-service/exile over inventing a prison system.
17. Never retroactively punish actions that were not violations when committed.
18. Persist law/policy history independently of leaders.
19. Ensure old saves preserve the control model players already had.
20. Add deterministic vote, policy-idempotence, case-identity, and authority gates early.
21. Run 30/120/180-day policy/refusal/backlog simulations before expanding content counts.
22. Author 20 laws and 15 policies only after every one has a real adapter and reachability proof.
23. Keep elections, revolts, propaganda, professional political classes, and inter-shelter governance as explicit follow-ons unless their supporting rails already exist.
24. Close only when shelter politics changes how existing systems make decisions without replacing those systems.

---

# 46. Final Outcome

When this plan is complete, the shelter stops being governed purely by invisible player fiat.

There is a persistent answer to:
- who can make a rule;
- what rules currently exist;
- how those rules were adopted;
- who supports them;
- how they can be changed;
- what happened when they were enforced.

A rationing law does not create food. It changes the priority constraints that the real food system uses. A work policy does not assign shifts. It changes the rules the real `DutyRosterSystem` interprets. A medical-priority policy does not cure anybody. It changes how scarce treatment priority is evaluated by the real medical authority.

Survivors can also disagree politically without becoming a second set of factions. Internal blocs emerge from beliefs and shared agendas already represented elsewhere. Their support for a proposal is explainable through ideology, relationships, shelter conditions, and leadership—not a random popularity roll.

Governance modes matter because they change procedure. Leader rule is fast because the leader can decide directly. A council requires representatives. An assembly requires broader approval. Informal governance remains viable when the shelter is still too small to need institutions.

Justice remains similarly grounded. Cases originate from actual incidents. The governance layer decides how those incidents are judged and which existing sanction authority should act. It does not secretly spawn thefts, witnesses, prisons, or punishments that no other system knows about.

Most importantly, the system accumulates institutional memory.

Leaders can die.
Councils can change.
Policies can be repealed.

But the shelter remembers which laws existed, what decisions were made, and what kind of community it tried to become.

The result is not a second grand-strategy game inside ASHFALL.

It is a survival shelter that has finally become a political community.
