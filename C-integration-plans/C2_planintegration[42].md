# C2 — Flagship Integration Plan [42]: Leadership Succession, Deputy Authority, Challenges, Elections, Legitimacy, and Continuity of Shelter Governance

> **Deliverable:** `C2_planintegration[42].md`
> **Source scope:** Plan 208 — *Leadership Succession & Challenge System*
> **Primary objective:** extend the existing `LeadershipSystem` so shelter leadership survives death, incapacity, resignation, challenge, election, recall, and other legitimate transfer events; add succession planning, deputy/acting leadership, political challenges, election workflows, legitimacy assessment, leadership history, and optional term-limit support; and connect those transitions to existing relations, morality, skill, interpersonal-conflict, governance, death/fate, morale, journal/archive, and UI systems without creating a second leadership authority or a disconnected political simulation.
> **Required execution order:** **208A Leadership Continuity / Transfer Contract → 208B Succession, Deputy, Challenge, Election, Legitimacy & UI → 208C Cross-System Integration, Save/CI, Exploit Control, Political-Stability Balance, and Closure**
> **Hard dependencies:** existing `LeadershipSystem`; `SurvivorRelationsSystem`; `SkillProgressionSystem`; `MoralChoiceSystem`; Plan 159 governance policy where available; Plan 202 `InterpersonalConflictSystem`; Plan 206 death/legacy or the canonical survivor-death/fate event source; morale/social consequence systems; Plan 31 semantic events; Plan 36 port-contract discipline; Plan 39 save durability; Plan 55 retention; Plan 162 archive/legacy for historically important leaders.
> **Scope discipline:** no second `current_leader_id`, no second leader-designation state, no duplicate survivor death truth, no duplicate relationship/support meter, no duplicate leadership skill, no challenge engine that independently resolves physical coups, no election system that ignores the shelter’s configured governance rules, no mutable legitimacy bar that drifts away from its factors, no leadership transfer that applies twice after save/load, no “automatic successor” that can become leader while dead/incapacitated/ineligible, no leaderless soft-lock, and no political-event spam caused by daily random challenge rolls.

---

# 0. Executive Intent

ASHFALL already has a functioning `LeadershipSystem` with:

- a current leader,
- designation state,
- leader stress,
- crisis morale aura,
- step-down cooldown,
- persistence.

What it lacks is **continuity**.

Today the lifecycle is effectively:

```text
survivor becomes leader
→ leader performs leadership role
→ leader dies / becomes invalid
→ leadership disappears
```

The target lifecycle is:

```text
current leader
     │
     ├─ designated successor
     ├─ backup successor
     ├─ deputy / acting authority
     ├─ legitimacy assessment
     └─ governance/election policy
     │
     ▼
leadership continuity coordinator
     │
     ├─ death/incapacity
     ├─ voluntary step-down
     ├─ challenge
     ├─ recall
     ├─ election
     ├─ appointment
     └─ coup result from external conflict owner
     │
     ▼
LeadershipTransfer
     │
     ▼
existing LeadershipSystem current leader
     │
     ├─ morale/social consequences
     ├─ relations consequences
     ├─ journal/history
     └─ archive/legacy
```

The strongest product outcome is:

> **Shelter leadership becomes a durable political institution rather than a single pointer. The player can prepare succession, appoint a deputy, survive a leader’s sudden death without breaking the simulation, face legitimate challenges when authority erodes, hold elections under the shelter’s governance rules, and watch leadership changes become part of the shelter’s political history.**

---

# 1. Source Diagnosis

The source establishes that the existing `LeadershipSystem.cs`:

- is ~288 lines,
- owns `current_leader_id`,
- tracks `is_designated_leader`,
- tracks leader stress accumulation,
- tracks step-down cooldown,
- has a crisis morale aura,
- already supports `CaptureState/RestoreState`,
- but lacks:
  - succession,
  - deputy,
  - leadership challenge,
  - election,
  - transfer on death,
  - term-limit policy,
  - recall,
  - leadership contest continuity.

The source requires:

- `SuccessionPlan`,
- `LeadershipChallenge`,
- `LeadershipElection`,
- `LeadershipTransfer`,
- `DeputyLeader`,
- `LeadershipLegitimacy`,
- extended `LeadershipState`,
- designated successor and backup,
- automatic succession on death/incapacity,
- emergency election if no successor,
- challenge support threshold of 30%,
- election voting,
- deputy succession,
- legitimacy from accession/performance/morality/popular support,
- optional term limits,
- deterministic `ISeededRng`,
- leadership UI,
- 8 leadership events,
- 7 quest/achievement hooks,
- old-save compatibility,
- `--leadership-succession-selftest`.

Three architectural corrections are required.

First:

```text
legitimacyScore
```

should preferably be **derived from factors** rather than a second independently mutated political stat.

Second:

```text
successful challenge
→ election
```

and:

```text
challenge winner becomes leader
```

are both present in the source and conflict unless challenge types have explicit resolution policies.

Therefore:

```text
challenge type / governance rule
→ resolution policy
```

must determine whether the winner immediately transfers leadership or merely forces an election.

Third:

```text
coup
```

must not be resolved as a simple vote if Plan 202/conflict/combat/security systems own physical conflict.

LeadershipSystem records the political claim and consumes the external outcome.

---

# 2. Program-Level Success Criteria

C2[42] closes only when all of the following are true.

1. The existing `LeadershipSystem` remains the only owner of the current shelter leader.
2. Succession data extends the existing leadership state rather than creating `LeadershipSuccessionSystem` with a second leader pointer.
3. A designated successor can be recorded.
4. A backup successor can be recorded.
5. Successor eligibility is validated at the moment of transfer.
6. Deputy appointment exists.
7. Deputy delegated powers are explicit.
8. Deputy/acting-leader authority does not silently equal full permanent leadership.
9. Leader death triggers one deterministic continuity workflow.
10. Leader incapacity triggers an acting/succession workflow according to policy.
11. Voluntary step-down triggers one valid transfer/election workflow.
12. An invalid/dead successor never becomes leader.
13. If no valid successor exists, emergency governance begins.
14. The shelter can enter a temporary vacancy/acting-leader state without crashing.
15. Leadership challenge eligibility is explicit.
16. Challenge supporter threshold defaults to the source’s 30% rule where governance policy permits it.
17. Support is derived from canonical relations/politics rather than a duplicate loyalty meter.
18. Challenge type determines resolution path.
19. Election candidates are eligibility-filtered.
20. Voter eligibility is rules-driven.
21. Election votes are deterministic under the same survivor state and seed.
22. Vote records persist and cannot reroll on save/load.
23. Ties have a deterministic policy.
24. Abstentions are representable if governance rules permit.
25. Turnout is derived from eligible voters and actual ballots.
26. Legitimacy is derived from explicit factors.
27. Accession method affects legitimacy.
28. Competence/performance affects legitimacy only through real performance facts.
29. Morality affects legitimacy through `MoralChoiceSystem` or canonical moral-event history.
30. Popular support derives from canonical relations/community support.
31. Low legitimacy may increase challenge eligibility/pressure but does not automatically spawn daily random challenges.
32. High legitimacy may reduce challenge support but does not make a leader immune.
33. Optional term limits are governed by Plan 159/rules data.
34. Re-election is supported when policy permits.
35. Recall is supported where governance mode allows it.
36. Coup attempts route physical conflict through Plan 202/security/combat as appropriate.
37. Leadership transfers are idempotent.
38. Leadership history preserves prior leaders and transfer causes.
39. Old saves keep the existing current leader.
40. Old saves gain no fabricated succession plan/challenge history.
41. Headless tests can resolve death succession, election, challenge, deputy fallback, and vacancy.
42. All leadership UI reads authoritative runtime state.
43. No leaderless campaign soft-lock occurs.
44. Political instability remains bounded under frequent-challenge stress.
45. `--leadership-succession-selftest` passes.

---

# 3. Architectural Invariants

## 3.1 One current-leader authority

Only existing `LeadershipSystem` may answer:

```text
Who is the current shelter leader?
```

All succession/challenge/election code culminates in one:

```text
ApplyLeadershipTransfer(...)
```

inside or through `LeadershipSystem`.

## 3.2 Governance policy and leadership state are separate

Plan 159 / `leadership_rules.json` may answer:

```text
How may leadership be acquired?
Who may vote?
Are terms enabled?
Can leaders be recalled?
```

`LeadershipSystem` answers:

```text
Who currently holds office?
What transfer occurred?
```

## 3.3 Succession is a policy, not blind assignment

A designated successor is not automatically eligible forever.

Transfer-time validation must check:

- alive,
- present,
- not incapacitated beyond allowed threshold,
- governance eligibility,
- no disqualifying state.

## 3.4 Deputy is not automatically permanent leader

Deputy can be:

```text
acting authority
```

during incapacity/vacancy.

Permanent succession depends on policy.

The source’s “deputy acts as successor if no formal plan” should be data-driven.

## 3.5 Legitimacy is derived

Prefer:

```text
LeadershipLegitimacyAssessment
```

over a mutable independent score.

Persist factor history where necessary, not redundant truth.

## 3.6 Challenges are event-driven

A challenge requires:

- grievance/cause,
- eligibility,
- support,
- governance permission.

No daily random political slot machine.

## 3.7 Elections are deterministic social decisions

Votes derive from:

- relationships,
- candidate competence,
- moral reputation,
- prior leadership performance,
- faction/social identity if canonically modeled,
- campaign commitments if modeled.

Use seeded RNG only for bounded uncertainty/tie behavior, not arbitrary candidate selection.

## 3.8 Coups are conflict outcomes

A coup may begin as a leadership challenge.

Physical seizure belongs to:

- Plan 202 conflict,
- security,
- combat.

LeadershipSystem consumes winner/outcome.

## 3.9 Transfer is atomic

Exactly one current leader after successful permanent transfer.

No intermediate double-leader state.

## 3.10 Vacancy is explicit

If no permanent leader is immediately available:

```text
Vacant / ActingLeadership
```

is a valid state.

Do not leave dangling invalid `current_leader_id`.

---

# 4. Leadership Continuity State Machine

Recommended high-level states:

```text
StableLeadership
ActingLeadership
SuccessionPending
ElectionPending
ChallengePending
TransferPending
Vacant
```

These are governance workflow states, not replacements for current leader.

Example:

```text
leader dies
→ validate designated successor
    ├─ valid → TransferPending → StableLeadership
    └─ invalid
        → validate backup
            ├─ valid → TransferPending → StableLeadership
            └─ invalid
                → validate deputy policy
                    ├─ acting deputy → ActingLeadership + ElectionPending
                    └─ none → Vacant + ElectionPending
```

---

# 5. Existing-System Ownership Map

```text
LeadershipSystem
→ current leader
→ leader stress
→ crisis aura
→ succession/transfer extension

SurvivorRelationsSystem
→ relationship/support facts

SkillProgressionSystem
→ leadership/competence skill

MoralChoiceSystem
→ moral-choice history / ethical legitimacy inputs

Plan 159
→ governance constitution / election/term/recall policy

Plan 202
→ interpersonal conflict / coup escalation

Plan 206 / survivor fate
→ death/legacy event

Morale/social systems
→ population reaction

Plan 162
→ historical leader legacy
```

---

# 6. Workstream 208A — Foundation / Leadership Continuity Contract

## Goal

Extend the existing leadership authority with normalized succession, deputy, transfer, election/challenge metadata, derived legitimacy, deterministic decision contracts, and safe migration.

---

# 7. 208A Phase A — Inspect Existing `LeadershipSystem`

Read end-to-end:

```text
Assets/Ashfall.Core/Survivors/LeadershipSystem.cs
```

Document:

- constructor dependencies,
- current leader setter,
- designation method,
- step-down API,
- leader invalidation behavior,
- stress tick,
- morale aura,
- capture/restore schema,
- tests,
- composition setup.

Do not patch blindly.

---

# 8. 208A Phase B — Preserve Existing Public API

Where possible, keep current calls stable.

Add:

```text
TryDesignateSuccessor(...)
TryAppointDeputy(...)
BeginLeadershipChallenge(...)
BeginElection(...)
ApplyLeadershipTransfer(...)
EvaluateLegitimacy(...)
```

rather than replacing all existing methods.

---

# 9. 208A Phase C — Normalize Source DTOs

The source proposes several large DTOs.

Recommended split:

```text
SuccessionPlan
DeputyAppointment
LeadershipChallengeRecord
LeadershipElectionRecord
LeadershipTransferRecord
LeadershipLegitimacyAssessment
LeadershipContinuityState
LeadershipState
```

---

# 10. 208A Phase D — `SuccessionPlan`

Fields:

```text
plan_id
leader_id
designated_successor_id
backup_successor_id optional
created_day
updated_day
status
policy_snapshot_ref
```

`isActive` can derive from status.

---

# 11. 208A Phase E — Succession Status

Typed:

```text
Active
Superseded
Consumed
Invalidated
Cancelled
```

---

# 12. 208A Phase F — No Self-Succession

Validate:

```text
successor != current leader
backup != current leader
backup != designated successor
```

---

# 13. 208A Phase G — Successor Eligibility

At designation time:

- survivor exists,
- alive,
- roster member,
- governance-eligible if policy requires.

At transfer time:

- revalidate all.

Designation is not a guarantee.

---

# 14. 208A Phase H — `DeputyAppointment`

Fields:

```text
appointment_id
leader_id
deputy_survivor_id
appointed_day
removed_day optional
delegated_authority_ids
status
```

---

# 15. 208A Phase I — Deputy Authority Vocabulary

Examples:

```text
ActDuringIncapacity
EmergencyDecisions
ShelterOperations
SecurityCoordination
DiplomaticRepresentation
ElectionAdministration
```

Only include powers real systems can consume.

---

# 16. 208A Phase J — No Fake Delegated Powers

Do not author:

```text
"full military power"
```

unless a canonical system queries it.

Each delegated power requires a consumer or remains flavor-only and explicitly labeled.

---

# 17. 208A Phase K — Acting Leader

Introduce:

```text
acting_leader_id
```

only if needed and clearly distinct from permanent current leader.

Preferred:

```text
LeadershipContinuityState.ActingLeaderId
```

not a second permanent `current_leader_id`.

---

# 18. 208A Phase L — Acting vs Permanent Effects

Document whether acting leader gets:

- morale aura,
- leadership skill effects,
- governance permissions.

Do not silently grant all permanent-leader benefits.

---

# 19. 208A Phase M — `LeadershipTransferRecord`

Fields:

```text
transfer_id
from_leader_id optional
to_leader_id
transfer_type
transfer_day
reason_code
reason_context_refs
voluntary
contested
source_workflow_id
```

---

# 20. 208A Phase N — Transfer Types

Preserve source:

```text
SuccessionOnDeath
StepDown
ChallengeVictory
ElectionVictory
Appointment
```

Add if needed:

```text
SuccessionOnIncapacity
Recall
CoupOutcome
EmergencyAppointment
```

---

# 21. 208A Phase O — Transfer Atomicity

`ApplyLeadershipTransfer` must:

1. validate current workflow,
2. validate target,
3. create transfer record,
4. change leader once,
5. update old/new leader designation flags,
6. clear/consume relevant succession/deputy state,
7. emit one semantic event,
8. commit consequence idempotency key.

---

# 22. 208A Phase P — No Leader Transfer

`to_leader_id = null` is not a normal transfer.

Use explicit:

```text
EnterVacancy(...)
```

if necessary.

---

# 23. 208A Phase Q — `LeadershipChallengeRecord`

Recommended:

```text
challenge_id
challenger_id
challenged_leader_id
challenge_type
reason_code
reason_context_refs
supporter_snapshot
opponent_snapshot
opened_day
resolution_day
resolution_policy
outcome
election_ref optional
conflict_ref optional
```

---

# 24. 208A Phase R — Challenge Type Vocabulary

Source:

```text
Election
Coup
Contest
Recall
```

Clarify semantics:

```text
ElectionChallenge
Coup
LeadershipContest
RecallPetition
```

---

# 25. 208A Phase S — Challenge Resolution Policy

Possible:

```text
DirectVoteTransfer
ForceElection
ConflictResolution
RecallVote
NegotiatedCompromise
```

Mapped by governance rules/challenge type.

---

# 26. 208A Phase T — Challenge Reason

Do not store only arbitrary prose.

Use:

```text
reason_code
context refs
localized display key
```

Examples:

```text
LowLegitimacy
FailedCrisis
MoralScandal
BrokenPromise
FactionalConflict
TermViolation
SuccessionDispute
```

---

# 27. 208A Phase U — Supporter Snapshot

Source wants supporters/opponents lists.

Persist snapshot at challenge-open time for procedural fairness.

But support can change during campaign period if rules allow.

If dynamic:

- store initial support,
- final vote separately.

Do not conflate support with vote.

---

# 28. 208A Phase V — 30% Support Threshold

Source requires minimum 30% of shelter.

Implement as default config:

```text
challenge_support_threshold = 0.30
```

but define denominator:

```text
eligible political participants
```

not literally every entity in roster if children/incapacitated/non-voters exist.

---

# 29. 208A Phase W — Challenge Eligibility

Candidate challenger must be:

- alive,
- present,
- governance-eligible,
- not incapacitated,
- not current leader,
- not challenge-cooldown blocked,
- have valid reason/support.

---

# 30. 208A Phase X — `LeadershipElectionRecord`

Recommended:

```text
election_id
trigger_type
candidate_ids
eligible_voter_ids
opened_day
campaign_end_day
vote_deadline_day
ballots
result
winner_id optional
turnout
legitimacy_assessment_ref
status
```

---

# 31. 208A Phase Y — Election Trigger Types

```text
LeaderDeathNoSuccessor
LeaderIncapacityPolicy
VoluntaryStepDown
ChallengeForcedElection
Recall
TermEnd
GovernanceReform
Vacancy
```

---

# 32. 208A Phase Z — Candidate Eligibility

Rules may include:

- age/life-stage,
- roster membership,
- leadership skill floor,
- no incapacitation,
- governance eligibility,
- nomination support threshold,
- no active disqualification.

Do not hardcode “any survivor” if governance policy says otherwise.

---

# 33. 208A Phase AA — Voter Eligibility

Source says all survivors vote.

Refine:

```text
all eligible survivors
```

Governance rules define:

- minors,
- incapacitated,
- absent expedition members,
- prisoners,
- temporary visitors.

No hidden arbitrary exclusion.

---

# 34. 208A Phase AB — Ballot Model

Prefer:

```text
voter_id
candidate_id optional
abstained
reason vector snapshot optional for tests/debug
```

Player UI does not need private exact reasons for every vote unless system intentionally reveals them.

---

# 35. 208A Phase AC — Vote Determinism

Vote decision can be deterministic from:

- relationship to candidate,
- candidate legitimacy,
- leadership competence,
- moral reputation,
- crisis performance,
- faction/group affinity if modeled,
- candidate promises if campaign system later exists.

If residual uncertainty exists:

- use `ISeededRng`,
- stable key by election/voter.

---

# 36. 208A Phase AD — No Save-Scum Voting

Commit ballots deterministically once.

Reload before results does not alter votes.

---

# 37. 208A Phase AE — Tie Policy

Data-driven:

```text
Runoff
SeniorCandidate
HighestSupport
DeputyPreference
RandomSeededLot
GovernanceRule
```

Prefer runoff where governance supports it.

---

# 38. 208A Phase AF — Election Turnout

Derived:

```text
ballots_cast / eligible_voters
```

No separately mutated turnout score.

---

# 39. 208A Phase AG — Election Legitimacy

Derived from:

- turnout,
- fairness,
- winning margin,
- accession legality,
- contestation,
- voter eligibility integrity.

Do not reuse general leader legitimacy blindly.

---

# 40. 208A Phase AH — `LeadershipLegitimacyAssessment`

Fields:

```text
leader_id
day
factor_contributions
score
band
reason_codes
```

Prefer computed read model.

Persist historical snapshots only for transfer/history if needed.

---

# 41. 208A Phase AI — Legitimacy Factors

Preserve source:

```text
Elected
Appointed
PopularSupport
Competence
Morality
CrisisPerformance
```

Add:

```text
SuccessionLegality
RecentChallengeOutcome
BrokenGovernanceRule
```

if needed.

---

# 42. 208A Phase AJ — Accession Legitimacy

Examples:

```text
fair election
→ positive

valid designated succession
→ moderate/positive depending governance

emergency appointment
→ neutral/temporary

coup
→ contested/negative unless later consolidated
```

Data, not hardcoded moral judgment.

---

# 43. 208A Phase AK — Popular Support

Do not create a second support ledger.

Derive from:

- survivor relations toward leader,
- morale/community opinion if canonical,
- prior political events.

---

# 44. 208A Phase AL — Competence

Use:

- leadership skill,
- actual crisis outcomes,
- shelter performance attribution only where causally defensible.

Do not make resource abundance automatically the leader’s personal legitimacy.

---

# 45. 208A Phase AM — Morality

Use canonical moral-choice history.

Not all morally harsh decisions are automatically illegitimate in every governance culture.

Map through rules/profile.

---

# 46. 208A Phase AN — Crisis Performance

Use explicit leadership-participation/performance events.

Examples:

- raid response,
- disaster decision,
- famine leadership,
- diplomatic crisis.

---

# 47. 208A Phase AO — Legitimacy Bands

Example:

```text
Contested
Weak
Accepted
Strong
Mandate
```

Thresholds data-driven.

---

# 48. 208A Phase AP — Legitimacy Effects

Source says:

- low legitimacy → challenges more likely, morale penalty,
- high legitimacy → challenges harder, morale bonus.

Refine:

- legitimacy affects challenge support threshold/eligibility weights,
- morale consequences route to morale system,
- no continuous giant passive bonus/penalty without cap.

---

# 49. 208A Phase AQ — No Daily Legitimacy Mutation

Recompute from factors:

```text
on relevant event
or once/day bounded
```

No arbitrary drift.

---

# 50. 208A Phase AR — Term Limits

Source marks optional.

Policy lives in:

```text
leadership_rules.json
```

or Plan 159 governance constitution.

---

# 51. 208A Phase AS — Term Fields

If enabled:

```text
term_start_day
term_length_days
reelection_allowed
max_consecutive_terms optional
```

---

# 52. 208A Phase AT — Term End

At term end:

```text
trigger election workflow
```

not immediate leader removal unless governance says term expires before replacement.

Prefer incumbent remains caretaker until successor installed.

---

# 53. 208A Phase AU — Recall

Recall is a governance-specific challenge.

Requires:

- petition/support threshold,
- eligible vote,
- majority rule,
- transfer/election policy.

---

# 54. 208A Phase AV — Succession Rules Data

Create:

```text
Assets/StreamingAssets/Data/leadership_rules.json
```

Contains:

- succession priority,
- challenge support threshold,
- candidate/voter eligibility,
- term rules,
- recall rules,
- tie rules,
- legitimacy weights,
- transfer policies.

No survivor-specific state.

---

# 55. 208A Phase AW — Governance Profile Integration

If Plan 159 owns constitutions/governance mode:

```text
leadership_rules.json
```

should define defaults and rule fragments.

Plan 159 selects active policy.

Avoid two conflicting governance authorities.

---

# 56. 208A Phase AX — State Extension

Extend `LeadershipState` with:

```text
succession_plan
deputy
active_challenges
active_election
election_history refs/summaries
transfer_history
continuity_state
term_state
schema_version
```

Legitimacy may be derived rather than persisted.

---

# 57. 208A Phase AY — History Retention

Do not retain every ballot forever in main save if long campaigns explode size.

Plan 55 policy:

- active election full ballots,
- recent historical election summary,
- landmark elections detailed,
- old ballots rolled up.

---

# 58. 208A Phase AZ — Capture / Restore Versioning

Migration from current `LeadershipState`:

```text
existing current leader preserved
succession_plan = null
deputy = null
active challenges = none
active election = none
transfer history = empty
term state initialized from current day/policy without retroactive election
```

---

# 59. 208A Phase BA — Old-Save Term Grace

If term limits become enabled after migration:

- do not immediately declare current leader’s term expired from campaign age.

Start term baseline at migration day unless reliable original accession date exists.

---

# 60. 208A Phase BB — Old-Save Legitimacy Bootstrap

Compute legitimacy from current canonical state.

Do not fabricate past election/appointment factor.

Use neutral:

```text
LegacyIncumbent
```

accession factor until next transfer.

---

# 61. 208A Phase BC — Deterministic RNG

Dedicated stream:

```text
leadership
```

Stable keys:

```text
election ID + voter ID
challenge ID + phase
political event ID
```

No wall clock.

---

# 62. 208A Phase BD — Event-Driven Tick Model

Leadership evaluates on:

- leader death/incapacity,
- voluntary step-down,
- challenge petition,
- term boundary,
- election deadline,
- governance change,
- relevant legitimacy factor change.

Daily tick only for:

- term countdown,
- campaign period,
- support reconciliation,
- bounded legitimacy refresh.

---

# 63. 208A Phase BE — Semantic Events

Candidate kinds:

```text
leadership_successor_designated
leadership_deputy_appointed
leadership_deputy_removed
leadership_challenge_opened
leadership_challenge_resolved
leadership_election_opened
leadership_election_resolved
leadership_transfer
leadership_vacancy
leadership_legitimacy_changed
leadership_term_ended
leadership_recall_opened
```

Plan 31 governance.

---

# 64. 208A Phase BF — Port Contract

Mandatory:

- survivor roster/alive state,
- relations,
- leadership skill,
- morality/history,
- morale sink,
- save service.

Conditional:

- Plan 159 governance,
- Plan 202 conflict,
- Plan 206 death/legacy,
- archive,
- achievement/quest hooks.

---

# 65. 208A Phase BG — Diagnostics

Expose:

```text
LEADERSHIP_CURRENT
LEADERSHIP_ACTING
LEADERSHIP_SUCCESSOR
LEADERSHIP_BACKUP
LEADERSHIP_DEPUTY
LEADERSHIP_CHALLENGES_ACTIVE
LEADERSHIP_ELECTION_ACTIVE
LEADERSHIP_LEGITIMACY_BAND
LEADERSHIP_TRANSFERS_TOTAL
LEADERSHIP_REQUIRED_PORTS_MISSING
```

---

# 66. 208A Tests

- migration preserves leader,
- successor designation,
- invalid successor,
- backup,
- deputy,
- acting leader,
- transfer atomicity,
- challenge eligibility,
- 30% threshold,
- election candidate/voter eligibility,
- deterministic vote,
- tie policy,
- legitimacy derivation,
- term migration grace,
- no duplicate current leader.

---

# 67. 208A Definition of Done

- [ ] existing LeadershipSystem extended,
- [ ] one current-leader authority,
- [ ] succession plan,
- [ ] successor + backup,
- [ ] deputy + delegated powers,
- [ ] acting leadership,
- [ ] transfer records,
- [ ] challenge records,
- [ ] election records,
- [ ] voter/candidate eligibility,
- [ ] deterministic ballots,
- [ ] tie policy,
- [ ] derived legitimacy,
- [ ] optional term limits,
- [ ] recall policy,
- [ ] leadership rules data,
- [ ] Plan 159 governance boundary,
- [ ] save migration,
- [ ] old-save grace,
- [ ] events/ports/diagnostics.

---

# 68. Workstream 208B — Succession Planning

## Goal

Make leadership continuity predictable enough to plan but uncertain enough that invalid successors, political opposition, and governance rules still matter.

---

# 69. 208B Phase A — Designate Successor UI

Leadership panel allows:

```text
Designated Successor
Backup Successor
```

with eligibility reasons.

---

# 70. 208B Phase B — Successor Acceptance

Decide whether designation requires survivor consent.

Recommended:

- political office nomination/designation can be recorded,
- survivor can refuse at transfer time if autonomy system supports it.

Do not assume every survivor automatically accepts leadership.

---

# 71. 208B Phase C — Successor Relationship Effects

Designation may affect:

- successor,
- deputy,
- rivals.

Use relation events.

Do not apply blanket jealousy.

---

# 72. 208B Phase D — Successor Visibility

Player-known designation appears.

Secret succession plans only if governance design explicitly supports them.

---

# 73. 208B Phase E — Backup Fallback

On leader death/incapacity:

```text
designated successor valid?
→ yes: continue policy
→ no: backup valid?
```

---

# 74. 208B Phase F — Deputy Fallback

If no plan:

- governance rule may elevate deputy permanently,
- or deputy becomes acting leader while election starts.

Prefer acting model for elective governance.

---

# 75. 208B Phase G — No Successor

If none valid:

```text
EnterVacancy or ActingLeadership
→ EmergencyElection
```

---

# 76. 208B Phase H — Death Transfer

Source:

```text
leader death → successor automatically becomes leader
```

Implement only where active governance policy permits automatic succession.

If elective constitution requires confirmation:

```text
successor becomes acting/candidate
```

---

# 77. 208B Phase I — Incapacitation

Define threshold:

- unconscious,
- severe illness,
- imprisoned,
- missing,
- expedition unavailable?

Temporary absence should not always trigger permanent succession.

---

# 78. 208B Phase J — Temporary Incapacity

Use deputy/acting leader.

Permanent transfer only after:

- death,
- permanent incapacity,
- formal removal,
- policy threshold.

---

# 79. 208B Phase K — Missing Leader

If leader is missing:

- acting deputy,
- timeout/investigation,
- succession only by policy.

Do not immediately declare dead.

---

# 80. 208B Phase L — Step Down

Existing step-down cooldown remains.

Extend:

```text
leader voluntarily resigns
→ successor/appointment/election according to policy
```

---

# 81. 208B Phase M — Step-Down Legitimacy

Voluntary orderly transfer may:

- preserve stability,
- affect outgoing leader reputation.

No universal morale bonus.

---

# 82. 208B Phase N — Succession Dispute

If multiple claims exist:

- designated successor,
- deputy,
- elected favorite,

may create challenge/election.

Do not auto-randomize.

---

# 83. 208B Phase O — Succession History

Record:

- former leader,
- successor,
- reason,
- day,
- contested/voluntary.

---

# 84. Workstream 208B — Deputy Mechanics

## Goal

Make the deputy useful for continuity and delegated authority without becoming a duplicate co-leader.

---

# 85. 208B Phase P — Appointment

Leader chooses eligible survivor.

Potential requirements:

- relationship/trust,
- leadership capability,
- governance policy.

---

# 86. 208B Phase Q — Deputy Powers

Represent as permission flags consumed by real systems.

Example:

```text
CanResolveRoutineLeadershipDecision
CanActDuringLeaderIncapacity
CanRepresentShelter
CanAdministerElection
```

---

# 87. 208B Phase R — Delegation Boundaries

Deputy cannot:

- permanently replace leader,
- override constitution,
- change succession plan without authority,
- bypass player/leader decisions.

---

# 88. 208B Phase S — Deputy Conflict

A deputy can become:

- loyal successor,
- challenger,
- compromise candidate.

Relations/Plan 202 influence.

---

# 89. 208B Phase T — Deputy Removal

Leader can remove subject to governance policy.

Record event.

Potential relation consequence.

---

# 90. 208B Phase U — Deputy Death

Appointment invalidates safely.

No dangling ID.

---

# 91. Workstream 208B — Leadership Challenges

## Goal

Turn low legitimacy, crisis failure, political conflict, or constitutional pressure into explicit challenges with support thresholds and clear resolution types.

---

# 92. 208B Phase V — Challenge Sources

Possible:

- low legitimacy,
- moral scandal,
- crisis failure,
- broken governance rule,
- relationship factionalism,
- term overrun,
- succession dispute,
- Plan 202 conflict.

---

# 93. 208B Phase W — No Random Daily Challenge Spawn

A challenge requires at least one:

```text
political grievance fact
```

plus challenger/support.

---

# 94. 208B Phase X — Challenger Selection

Candidate pool:

- eligible survivors,
- sufficient leadership ambition/traits if modeled,
- supporters,
- conflict history.

If Survivor Autonomy Plan 144 exists:

- autonomous challenger decision can consume it.

---

# 95. 208B Phase Y — Player-Initiated Challenge

If player can initiate on behalf of survivor:

- only eligible survivor,
- enough support.

No arbitrary challenger assignment.

---

# 96. 208B Phase Z — Support Calculation

Support intent can derive from:

```text
relationship to challenger
relationship to leader
leader legitimacy
challenger competence
moral reputation
recent crisis outcomes
governance preference if modeled
```

---

# 97. 208B Phase AA — Support Snapshot vs Vote

Support petition determines whether challenge opens.

Final election/contest may differ.

---

# 98. 208B Phase AB — Opponents

Opponent list is informational/history.

Do not require every non-supporter to be an explicit opponent.

Allow:

- neutral,
- undecided.

---

# 99. 208B Phase AC — Challenge Campaign Period

Optional.

During period:

- events,
- debates,
- persuasion,
- conflict.

Do not create full political campaign subsystem unless Plan 159 supports.

Baseline can be short fixed duration.

---

# 100. 208B Phase AD — Challenge Outcome Types

Preserve:

```text
Pending
ChallengerWins
LeaderWins
Withdrawn
Compromise
```

Also:

```text
EscalatedToElection
EscalatedToConflict
Invalidated
```

if needed.

---

# 101. 208B Phase AE — Election Challenge

Resolution:

```text
petition passes
→ election starts
```

Winner comes from election.

Do not pre-award challenger.

---

# 102. 208B Phase AF — Recall Challenge

Resolution:

```text
recall vote
→ if leader removed
→ replacement policy
```

---

# 103. 208B Phase AG — Contest

Could be:

- leadership debate,
- performance contest,
- governance-specific contest.

Only implement if there is a real mechanic.

Otherwise map to vote/election.

---

# 104. 208B Phase AH — Coup

Political challenge opens.

Physical outcome routes to Plan 202/security/combat.

LeadershipSystem receives:

```text
CoupOutcome
```

then performs transfer if winner valid.

---

# 105. 208B Phase AI — Failed Challenge Consequences

Source says morale/relationship penalty.

Refine:

- challenger may lose standing/relationships with opponents,
- supporters may react,
- leader may retaliate only if authored/autonomy supports.

No automatic global morale punishment.

---

# 106. 208B Phase AJ — Compromise

Possible:

- deputy appointment,
- power sharing,
- early election,
- policy reform,
- challenger withdrawal.

Requires real governance effects to be meaningful.

---

# 107. 208B Phase AK — Challenge Cooldown

Prevent repeated same challenger spam.

Configurable:

```text
challenge_cooldown_days
```

---

# 108. 208B Phase AL — Challenge Reason Deduplication

Same unresolved grievance cannot reopen same challenge repeatedly after save/load.

---

# 109. Workstream 208B — Elections

## Goal

Resolve leadership through understandable, deterministic shelter politics.

---

# 110. 208B Phase AM — Election Lifecycle

```text
Trigger
→ Nomination
→ Campaign/Deliberation
→ BallotCommit
→ Count
→ TieResolution if needed
→ WinnerValidation
→ Transfer
→ Aftermath
```

---

# 111. 208B Phase AN — Nomination

Candidate may:

- self-nominate,
- be nominated by supporters,
- be successor/deputy automatically eligible.

Policy-driven.

---

# 112. 208B Phase AO — Candidate Minimum Support

Source says candidates declare with minimum support.

Use rules field:

```text
candidate_nomination_threshold
```

---

# 113. 208B Phase AP — Candidate Refusal

If survivor autonomy supports:

- candidate can refuse.

Persist refusal.

---

# 114. 208B Phase AQ — Campaign Phase

Baseline inputs:

- candidate competence,
- relations,
- moral reputation,
- current legitimacy context.

Do not add speeches/promises unless content exists.

---

# 115. 208B Phase AR — Voting

All eligible voters evaluate candidates.

Player may control own/player-avatar vote only if design includes a player survivor.

Otherwise votes are survivor-autonomous.

---

# 116. 208B Phase AS — Ballot Privacy

UI should usually show:

- totals,
- turnout,
- maybe declared supporters.

Do not reveal each private vote unless governance design makes ballots public.

History can store ballots for determinism while UI hides them.

---

# 117. 208B Phase AT — Vote Factors

Recommended deterministic scoring:

```text
relationship affinity/support
candidate leadership competence
candidate moral legitimacy
recent crisis performance
candidate-voter conflict
governance/trait affinity if canonical
```

---

# 118. 208B Phase AU — Vote Noise

If needed:

```text
small seeded residual
```

but candidate ranking should be primarily state-driven.

---

# 119. 208B Phase AV — Abstention

Allowed if:

- no acceptable candidate,
- disengagement,
- incapacity,
- governance policy.

---

# 120. 208B Phase AW — Turnout

Low turnout reduces election legitimacy.

---

# 121. 208B Phase AX — Winning Rule

Configurable:

```text
Plurality
Majority
Runoff
RankedChoice future
```

Baseline likely plurality/majority.

Do not implement complex electoral system without need.

---

# 122. 208B Phase AY — Runoff

If enabled:

- top candidates,
- new deterministic ballot event,
- persistent runoff ID.

---

# 123. 208B Phase AZ — Winner Validation

Before transfer:

- winner still alive,
- present/eligible,
- not incapacitated.

If invalid:

- runner-up or new election per policy.

---

# 124. 208B Phase BA — Election History

Store summary:

- trigger,
- candidates,
- turnout,
- result,
- winning margin,
- fairness flags,
- transfer ref.

---

# 125. 208B Phase BB — Fair Election

Define criteria:

- correct eligible electorate,
- no invalid votes,
- no force/compromise if modeled,
- rules honored,
- result applied.

Source “The Democrat” can consume this.

---

# 126. Workstream 208B — Legitimacy

## Goal

Make legitimacy a transparent political assessment grounded in accession, support, competence, morality, and crisis performance.

---

# 127. 208B Phase BC — Legitimacy Formula

Recommended:

```text
accession legitimacy
+ popular support
+ competence
+ moral reputation
+ crisis performance
+ constitutional compliance
+ recent political stability
```

weighted by governance profile.

---

# 128. 208B Phase BD — Factor Range

Each factor normalized:

```text
-1..1
```

or:

```text
0..100
```

Then weighted.

Avoid independently accumulating arbitrary score.

---

# 129. 208B Phase BE — Explainability

UI shows:

```text
Strong mandate
+ fairly elected
+ strong support
+ successful storm response
- controversial rationing decision
```

not just:

```text
Legitimacy: 73
```

---

# 130. 208B Phase BF — Legitimacy Score Display

If numeric score retained for UI:

- show band prominently,
- exact number optionally.

No hidden magic modifiers.

---

# 131. 208B Phase BG — Popular Support Sampling

For large shelter:

- derive from all eligible adults/participants.

Do not use only challenger supporters.

---

# 132. 208B Phase BH — Relationship Bias

Leader’s close friends should not equal entire popular support.

Aggregate across shelter.

---

# 133. 208B Phase BI — Morale Effects

Legitimacy can create small bounded morale context.

Avoid:

```text
+20 morale every day
```

Prefer:

- transition shock,
- mandate stabilization,
- contested-leadership pressure.

---

# 134. 208B Phase BJ — Challenge Pressure

Low legitimacy can lower petition threshold or increase autonomous challenger willingness.

Do not auto-open without grievance.

---

# 135. 208B Phase BK — Performance Recovery

A contested leader can recover legitimacy through:

- successful crisis management,
- moral reconciliation,
- improved support,
- constitutional compliance.

No permanent lock.

---

# 136. 208B Phase BL — Post-Coup Legitimacy

Coup victor starts contested.

Can later consolidate or reform.

No immediate 100 legitimacy because they won conflict.

---

# 137. Workstream 208B — Term Limits / Recall

## Goal

Support optional constitutional continuity without forcing every campaign into elections every 90 days.

---

# 138. 208B Phase BM — Policy Off by Default Unless Governance Says Otherwise

Source says optional.

Do not force term limits globally.

---

# 139. 208B Phase BN — 90-Day Example

Treat source’s 90 days as example/config default for a term-limit profile, not immutable code constant.

---

# 140. 208B Phase BO — Reelection

Incumbent can run if:

- eligible,
- term rules allow.

---

# 141. 208B Phase BP — Consecutive Term Limit

Optional:

```text
max_consecutive_terms
```

If absent:

- unlimited re-election.

---

# 142. 208B Phase BQ — Recall Petition

Requires separate support threshold.

May be higher than challenge threshold.

---

# 143. 208B Phase BR — Recall Vote

Vote is:

```text
retain / remove
```

If remove:

- replacement policy starts.

---

# 144. Workstream 208B — Leadership UI

## Goal

Make continuity, succession, legitimacy, challenges, elections, and history legible without forcing constant political micromanagement.

---

# 145. 208B Phase BS — Leadership Panel

Show:

- current leader,
- acting leader if any,
- legitimacy band/reasons,
- current term if enabled,
- succession summary,
- deputy,
- active political process.

---

# 146. 208B Phase BT — Succession Panel

Show:

- designated successor,
- backup,
- eligibility status,
- policy effect on transfer.

Example:

```text
Designated successor: Mara
If leader dies: becomes Acting Leader pending election
```

This is better than ambiguous “successor.”

---

# 147. 208B Phase BU — Deputy Panel

Show:

- deputy,
- delegated powers,
- acting-status rules,
- appointment/removal.

---

# 148. 208B Phase BV — Challenge Panel

Show:

- challenger,
- reason,
- petition support,
- challenge type,
- resolution path,
- deadline,
- known supporters.

---

# 149. 208B Phase BW — Election Panel

Show:

- trigger,
- candidates,
- campaign/deadline,
- eligible voter count,
- result when complete,
- turnout,
- fairness/legitimacy summary.

---

# 150. 208B Phase BX — History Panel

Timeline:

```text
Leader
Term/tenure
Transfer reason
Election/challenge
Outcome
```

---

# 151. 208B Phase BY — No Hidden Vote Spoilers

Before ballots resolve, do not show exact future vote totals.

Support estimates can be shown with uncertainty if designed.

---

# 152. 208B Phase BZ — Tooltips / Accessibility

Source asks hover.

Plan 184 requires focus/details alternative.

Leader details accessible by:

- hover,
- keyboard focus,
- controller details action.

---

# 153. 208B Phase CA — Large Text

All leadership panels survive 2× text.

---

# 154. 208B Phase CB — No Color-Only Legitimacy

Use label/icon.

---

# 155. 208B Phase CC — Screen Reader Semantics

Leadership summary announces:

```text
current leader
legitimacy band
successor
deputy
active challenge/election
```

---

# 156. 208B Phase CD — Cognitive Load Mode

Reduced view shows:

```text
Leader
Stability
Successor
Current political issue
Next action
```

advanced political factors collapsible.

---

# 157. Workstream 208B — Events / Quest Hooks

## Goal

Preserve source narrative hooks while avoiding duplicate quest/achievement ownership.

---

# 158. 208B Phase CE — Leadership Events

Preserve source eight:

```text
The Succession
The Challenge
The Election
The Deputy
The Step Down
The Coup
The Mandate
The Crisis
```

Emit semantic events.

---

# 159. 208B Phase CF — The Succession

Trigger on valid succession transfer.

---

# 160. 208B Phase CG — The Challenge

Trigger on meaningful leadership challenge opening.

---

# 161. 208B Phase CH — The Election

Trigger on completed election.

---

# 162. 208B Phase CI — The Deputy

Trigger on first/significant deputy appointment.

---

# 163. 208B Phase CJ — The Step Down

Trigger on voluntary leader resignation.

---

# 164. 208B Phase CK — The Coup

Trigger only when external conflict/coup outcome exists.

Do not label ordinary vote as coup.

---

# 165. 208B Phase CL — The Mandate

Trigger on re-election/strong mandate.

---

# 166. 208B Phase CM — The Crisis

Trigger on leadership vacancy/continuity emergency.

---

# 167. 208B Phase CN — Quest/Achievement Hooks

Source:

```text
The Leader
The Kingmaker
The Challenger
The Successor
The Democrat
The Stabilizer
The Reformer
```

If Plan 149 owns achievements:

- register criteria there.

If shared quest runtime owns quests:

- use it.

No second quest/achievement manager.

---

# 168. 208B Phase CO — The Leader

Become shelter leader through any valid transfer type.

---

# 169. 208B Phase CP — The Kingmaker

Source:

```text
elect 3 leaders
```

Count distinct completed election transfers.

---

# 170. 208B Phase CQ — The Challenger

Successfully challenge leader.

If challenge only forces election:

- criterion may require challenger ultimately wins election.

Define explicitly.

---

# 171. 208B Phase CR — The Successor

Become leader through designated/backup/deputy succession.

---

# 172. 208B Phase CS — The Democrat

Hold 5 fair elections.

Use fair-election criteria.

---

# 173. 208B Phase CT — The Stabilizer

Maintain 90+ legitimacy for 100 days.

If legitimacy is derived:

- daily assessment records band/score threshold duration.

No mutable achievement-only meter.

---

# 174. 208B Phase CU — The Reformer

Implement term limits.

Only if governance system supports policy reform.

---

# 175. 208B Phase CV — Tutorial

First leadership transition explains:

- succession,
- deputy,
- legitimacy,
- challenge/election,
- governance rules.

Do not overload first appointment.

---

# 176. 208B Phase CW — Data Localization

All:

- rule names,
- transfer reasons,
- challenge reasons,
- legitimacy factors,
- UI labels

use localization keys.

---

# 177. 208B Definition of Done

- [ ] successor designation,
- [ ] backup,
- [ ] succession transfer,
- [ ] no-successor emergency workflow,
- [ ] incapacity/acting leader,
- [ ] deputy appointment/powers/removal,
- [ ] challenge initiation,
- [ ] support threshold,
- [ ] challenge types/resolution policy,
- [ ] election lifecycle,
- [ ] candidates,
- [ ] voters,
- [ ] deterministic ballots,
- [ ] ties/runoff policy,
- [ ] turnout,
- [ ] derived legitimacy,
- [ ] optional terms,
- [ ] recall,
- [ ] leadership UI,
- [ ] succession/challenge/election/deputy/history panels,
- [ ] source events,
- [ ] source hooks,
- [ ] tutorial,
- [ ] localization,
- [ ] accessibility.

---

# 178. Workstream 208C — Survivor Relations Integration

## Goal

Use real survivor relationships to influence support, voting, challenge coalitions, and aftermath without creating a duplicate political-support ledger.

---

# 179. 208C Phase A — Relationship Inputs

Consume:

- affinity/trust band,
- grievance/conflict,
- family/romance if relevant,
- mentor/protégé,
- recent betrayal/reconciliation.

---

# 180. 208C Phase B — Support Intent

Derive political support snapshot.

Do not persist long-term `political_support` unless governance system explicitly owns it.

---

# 181. 208C Phase C — Candidate Voting

Relationship matters but is not sole factor.

Avoid:

```text
best friend always wins vote
```

Competence/morality/governance matter.

---

# 182. 208C Phase D — Political Fallout

After election/challenge:

- winners/losers,
- supporters/opponents

may get reason-coded relationship consequences.

Bound magnitude.

---

# 183. 208C Phase E — Family Bloc Guard

Do not treat family as automatic vote bloc.

Use actual relationships/traits.

---

# 184. Workstream 208C — Skill Integration

## Goal

Use real leadership competence without creating separate political skill.

---

# 185. 208C Phase F — Leadership Skill

Audit actual skill ID.

Use:

- candidate competence,
- crisis performance modifier,
- governing effectiveness.

---

# 186. 208C Phase G — No Election Skill XP Farm

Repeated candidacy/elections should not grant unlimited leadership XP.

Only meaningful service/events.

---

# 187. 208C Phase H — Deputy Experience

If acting deputy performs real leadership duties:

- canonical leadership XP may be appropriate.

---

# 188. Workstream 208C — Moral Choice Integration

## Goal

Make moral legitimacy reflect real decisions without hardcoding a universal morality meter.

---

# 189. 208C Phase I — Moral Event Inputs

Use reason-coded moral-choice outcomes.

---

# 190. 208C Phase J — Governance Values

Different governance profiles may weight:

- mercy,
- order,
- fairness,
- survival pragmatism

differently if Plan 159 supports ideology/culture.

Avoid simplistic “good choice + legitimacy.”

---

# 191. 208C Phase K — Scandal

A publicly known moral event may affect legitimacy.

Secret choice should not affect public support unless discovered.

Knowledge state matters.

---

# 192. Workstream 208C — Plan 202 Conflict Integration

## Goal

Let leadership politics escalate into interpersonal conflict without embedding conflict/combat resolution inside LeadershipSystem.

---

# 193. 208C Phase L — Challenge Tension

Opening challenge may emit:

```text
leadership_conflict_context
```

to Plan 202.

---

# 194. 208C Phase M — Coup Escalation

Plan 202/security/combat resolves:

- intimidation,
- violence,
- detention,
- takeover.

Leadership consumes:

```text
winner
contested
casualties refs
```

---

# 195. 208C Phase N — No Double Consequences

If Plan 202 already applies injury/relationship consequences:

LeadershipSystem must not duplicate.

---

# 196. Workstream 208C — Death / Legacy Integration

## Goal

Make leader death trigger continuity exactly once and preserve historical significance.

---

# 197. 208C Phase O — Canonical Death Event

Use Plan 206 or core survivor fate event.

Leadership listens:

```text
SurvivorDied(leader_id)
```

---

# 198. 208C Phase P — Death Idempotency

Same death event:

```text
one succession workflow
```

---

# 199. 208C Phase Q — Leader Legacy

Plan 206/162 may record:

- tenure,
- crises,
- transfer/death,
- successor.

Leadership supplies structured history.

---

# 200. 208C Phase R — No Death Ownership

Leadership does not mark leader dead.

---

# 201. Workstream 208C — Governance Integration

## Goal

Use one constitutional/policy authority.

---

# 202. 208C Phase S — Plan 159 Policy Adapter

Define:

```text
ILeadershipGovernancePolicy
```

Queries:

```text
GetSuccessionMode()
CanAppointDeputy()
CanChallenge(...)
GetChallengeThreshold()
GetElectionRules()
GetTermRules()
GetRecallRules()
GetVoterEligibility(...)
GetCandidateEligibility(...)
```

---

# 203. 208C Phase T — Fallback Policy

If Plan 159 unavailable:

- load conservative default from `leadership_rules.json`.

Document migration to governance adapter later.

---

# 204. 208C Phase U — No Duplicate Constitution

Leadership rules should not independently persist current political constitution if Plan 159 already does.

---

# 205. Workstream 208C — Morale Integration

## Goal

Make leadership stability matter without turning legitimacy into a huge passive morale engine.

---

# 206. 208C Phase V — Transition Shock

Leader death/vacancy may create one-time morale shock.

---

# 207. 208C Phase W — Orderly Succession

Can reduce shock.

Do not necessarily create positive morale.

---

# 208. 208C Phase X — Contested Rule

Ongoing contested leadership may apply bounded morale pressure.

---

# 209. 208C Phase Y — Strong Mandate

May apply temporary stabilization.

No infinite daily morale farming.

---

# 210. Workstream 208C — Save / Load / Idempotency

## Goal

Prove every political workflow survives save/load with exactly one result.

---

# 211. 208C Phase Z — Save Matrix

Test:

```text
stable leader + no plan
successor designated
backup designated
deputy active
leader incapacitated
acting leader
leader death received
transfer pending
challenge petition open
challenge campaign
election nomination
ballots committed
tie/runoff
winner selected
transfer applied
vacancy
term ending
recall vote
```

---

# 212. 208C Phase AA — Transfer Idempotency

Stable key:

```text
source workflow ID + from leader + to leader + transfer type
```

Apply once.

---

# 213. 208C Phase AB — Death Succession Reroll

Once successor validation/choice commits:

- reload cannot choose backup/election differently.

---

# 214. 208C Phase AC — Ballot Idempotency

Each:

```text
election ID + voter ID
```

has one ballot.

---

# 215. 208C Phase AD — Tie Idempotency

Runoff/seeded lot persists.

---

# 216. 208C Phase AE — Challenge Idempotency

Same grievance/petition cannot duplicate challenge.

---

# 217. 208C Phase AF — Legitimacy Idempotency

Derived assessment recomputes deterministically.

Historical snapshot does not apply effects twice.

---

# 218. 208C Phase AG — Deputy Appointment Idempotency

Same active appointment not duplicated.

---

# 219. 208C Phase AH — Old Save Round Trip

Existing current leader remains.

No sudden election.

---

# 220. Workstream 208C — Exploit Prevention

## Goal

Prevent transfer, support, legitimacy, election, and achievement farming.

---

# 221. 208C Phase AI — Step-Down / Reappoint Exploit

Cannot:

```text
step down
reappoint
step down
```

to farm events/rewards.

Use existing cooldown + history.

---

# 222. 208C Phase AJ — Successor Cycling

Changing successor repeatedly should not:

- generate morale,
- relations,
- achievements.

Only appointment history if meaningful.

---

# 223. 208C Phase AK — Challenge Threshold Farming

Rapid relation-band oscillation does not repeatedly open petition.

Challenge requires stable support snapshot/cooldown.

---

# 224. 208C Phase AL — Election Reload Farming

Ballots committed once.

---

# 225. 208C Phase AM — Candidate Withdrawal Reroll

Withdrawal is persisted.

Cannot reopen save to restore candidate.

---

# 226. 208C Phase AN — Legitimacy Farming

Player cannot farm legitimacy by:

- repeated trivial moral choices,
- repeated easy duties,
- UI reopen.

Use significant event sources and bounded contributions.

---

# 227. 208C Phase AO — Crisis Performance Farming

Same crisis contributes once.

---

# 228. 208C Phase AP — Term Reset Exploit

Leadership transfer/temporary acting state must not incorrectly reset term unless policy says so.

---

# 229. 208C Phase AQ — Deputy Acting-Term Exploit

Acting deputy should not gain infinite term reset/reelection benefit.

---

# 230. 208C Phase AR — Fair Election Achievement Farm

Same election counted once.

---

# 231. Workstream 208C — Edge Cases

## Goal

Prove continuity under extreme roster and political states.

---

# 232. 208C Phase AS — No Leader

Source explicitly asks to test current behavior.

Expected after this plan:

```text
valid vacancy/acting state
```

No crash.

Emergency leadership workflow if policy requires.

---

# 233. 208C Phase AT — One Eligible Survivor

If one survivor remains and is eligible:

- emergency appointment/acclamation may occur per policy.

Do not launch meaningless election.

---

# 234. 208C Phase AU — Zero Eligible Survivors

Remain vacant.

Leadership-dependent benefits disabled gracefully.

Game continues if possible.

---

# 235. 208C Phase AV — Leader and Successor Die Same Event

Process survivor deaths first.

Then succession validation sees successor invalid.

Backup/deputy/election fallback.

---

# 236. 208C Phase AW — Leader Dies During Election

Election trigger policy:

- current election may continue,
- emergency rules may alter.

Data-driven.

No duplicate election.

---

# 237. 208C Phase AX — Challenger Dies During Challenge

Withdraw/invalidate challenge or continue election without candidate.

---

# 238. 208C Phase AY — Election Winner Dies Before Transfer

Revalidate.

Use runner-up/new election policy.

---

# 239. 208C Phase AZ — Deputy Dies During Incapacity

Acting authority lost.

Enter next fallback.

---

# 240. 208C Phase BA — Leader Recovers From Incapacity

If deputy acting only:

- permanent leader resumes.

If permanent succession already occurred:

- no automatic rollback.

---

# 241. 208C Phase BB — Frequent Challenges

Stress scenario.

Use:

- challenge cooldown,
- legitimacy causes,
- support threshold,
- global political event budget.

No daily politics spam.

---

# 242. 208C Phase BC — Unanimous Support

No challenge despite low numerical legitimacy unless grievance/challenger exists.

---

# 243. 208C Phase BD — High Legitimacy / Serious Grievance

Challenge can still occur.

Legitimacy is not immunity.

---

# 244. 208C Phase BE — Coup With No Combat-Capable Supporters

External conflict owner may resolve as failed/intimidation-only.

Leadership does not fake violent victory.

---

# 245. 208C Phase BF — Governance Rule Changes Mid-Term

Plan 159 must specify grandfathering:

- current term,
- next election,
- recall eligibility.

Leadership consumes policy transition.

---

# 246. Workstream 208C — Determinism

## Goal

Guarantee political outcomes reproduce under the same social state and choices.

---

# 247. 208C Phase BG — Stable Voter Ordering

Sort voters by survivor ID.

---

# 248. 208C Phase BH — Stable Candidate Ordering

Sort candidates by survivor ID before scoring/tie operations.

---

# 249. 208C Phase BI — Same-Seed Election Digest

Digest:

```text
election ID
candidate list
voter list
ballots
winner
turnout
transfer ID
```

---

# 250. 208C Phase BJ — Same Inputs

Same:

```text
seed
survivor roster
relations
skills
moral history
governance rules
player choices
```

→ same election/challenge outcome.

---

# 251. 208C Phase BK — No Collection-Order Randomness

Avoid dictionary iteration determining vote/tie result.

---

# 252. Workstream 208C — Data Integrity

## Goal

Validate rules and political references before runtime.

---

# 253. 208C Phase BL — `leadership_rules.json` Validation

Validate:

- support thresholds 0–1,
- term length > 0,
- tie policy,
- election rule,
- succession mode,
- recall threshold,
- legitimacy weights,
- candidate/voter rules.

---

# 254. 208C Phase BM — Survivor Reference Validation

State restore:

- current leader,
- successor,
- backup,
- deputy,
- challengers,
- candidates,
- voters

must resolve or be migrated/invalidated safely.

---

# 255. 208C Phase BN — No Duplicate Roles

A survivor cannot simultaneously be:

- current leader,
- successor to themselves,
- challenger to themselves.

Deputy + successor may be allowed.

---

# 256. 208C Phase BO — Election Integrity

Validate:

- winner is candidate,
- every ballot voter eligible,
- every candidate valid,
- vote totals match ballots,
- turnout matches.

---

# 257. 208C Phase BP — Challenge Integrity

Validate:

- challenger valid,
- challenged leader matches relevant tenure,
- support threshold met at open,
- resolution references valid election/conflict if used.

---

# 258. 208C Phase BQ — Transfer Integrity

Validate:

- no duplicate ID,
- from leader matches previous state unless emergency vacancy,
- to leader valid,
- transfer type recognized.

---

# 259. 208C Phase BR — Legitimacy Weight Integrity

Weights normalized or bounded.

No missing factor IDs.

---

# 260. Workstream 208C — `--leadership-succession-selftest`

Required scenarios:

1. designate successor,
2. designate backup,
3. invalid successor,
4. deputy appointment,
5. deputy removal,
6. leader death → successor,
7. leader death → backup,
8. leader death → deputy acting,
9. leader death → emergency election,
10. temporary incapacity,
11. leader recovery,
12. voluntary step-down,
13. challenge below 30% rejected,
14. challenge at threshold opens,
15. leader wins challenge,
16. challenger wins direct contest,
17. challenge forces election,
18. recall,
19. coup external outcome adapter,
20. election nomination,
21. deterministic voting,
22. abstention,
23. tie/runoff,
24. winner transfer,
25. legitimacy low/high,
26. term end,
27. re-election,
28. save/load mid-challenge,
29. save/load mid-election,
30. transfer idempotency,
31. old save,
32. no leader,
33. frequent challenges.

---

# 261. Workstream 208C — Deliberate Failure Proof

Break:

- successor = leader,
- missing successor survivor,
- vote from ineligible voter,
- vote for noncandidate,
- turnout mismatch,
- transfer to dead survivor,
- duplicate transfer ID,
- challenge with stale leader ID,
- unsupported tie policy,
- legitimacy weights invalid.

Assert gate fails.

---

# 262. Workstream 208C — Political Stability / Balance

## Goal

Make political continuity meaningful without turning shelter management into constant election administration.

---

# 263. 208C Phase BS — 200-Day Leadership Soak

Profiles:

```text
stable_high_legitimacy
contested_low_legitimacy
frequent_crises
term_limit_governance
succession_heavy
challenge_heavy
```

Record:

```text
transfers
vacancy days
acting-leader days
challenges
elections
recalls
coups
legitimacy bands
morale consequences
```

---

# 264. 208C Phase BT — Stability Budget

Measure:

```text
political transitions / 100 days
challenge notifications / 100 days
election management actions / 100 days
```

No fixed target until evidence.

---

# 265. 208C Phase BU — Leadership Vacancy Budget

Normal campaign should rarely remain leaderless for long unless catastrophe.

Track:

```text
vacancy days
```

---

# 266. 208C Phase BV — Challenge Frequency

Low legitimacy alone should not create challenge every cooldown.

Need:

- grievance,
- challenger,
- support.

---

# 267. 208C Phase BW — Succession Value

Compare:

```text
planned succession
no plan
deputy only
```

Metrics:

- vacancy duration,
- morale shock,
- election burden,
- contestation.

---

# 268. 208C Phase BX — Legitimacy Impact

Legitimacy should matter enough to influence politics but not dominate all vote decisions.

---

# 269. 208C Phase BY — Election Fairness

Candidates with:

- high skill but weak relationships,
- strong relationships but weak competence,
- moral controversy,
- incumbent advantage

should produce varied outcomes.

---

# 270. 208C Phase BZ — Deputy Value

Deputy should reduce continuity risk.

Should not become mandatory optimal strategy with no cost.

Potential costs:

- relationship politics,
- work allocation,
- rival power center.

---

# 271. 208C Phase CA — Term-Limit Burden

90-day example may be too frequent for a 120–180 day campaign.

Test:

```text
60
90
120
180
off
```

Choose profile-specific values.

---

# 272. 208C Phase CB — Recall Balance

Recall should be rare and meaningful.

Not a free leader reroll button.

---

# 273. 208C Phase CC — Coup Risk

Coup requires severe political breakdown.

Not ordinary low legitimacy.

---

# 274. 208C Phase CD — Morale Consequence Budget

Leadership politics should not create massive repeated morale oscillations.

Use event-based bounded effects.

---

# 275. Workstream 208C — Performance

## Goal

Keep political simulation cheap.

---

# 276. 208C Phase CE — Event-Driven Political Evaluation

No per-frame scans.

---

# 277. 208C Phase CF — Election Complexity

Shelter voter count likely small.

Even:

```text
O(voters × candidates)
```

is acceptable.

Still deterministic.

---

# 278. 208C Phase CG — Legitimacy Caching

Recompute on relevant factor changes.

Cache read model for UI.

---

# 279. 208C Phase CH — History Retention

Old ballots can roll up.

Avoid 400-year save bloat.

---

# 280. Workstream 208C — Accessibility

## Goal

Political state must remain legible under Plan 184 and Plan 37.

---

# 281. 208C Phase CI — Keyboard / Controller

All:

- successor selection,
- deputy management,
- challenge vote,
- election screens

fully focusable.

---

# 282. 208C Phase CJ — Large Text

2× text supported.

---

# 283. 208C Phase CK — No Color-Only Politics

Legitimacy/challenge status uses labels/icons.

---

# 284. 208C Phase CL — Screen Reader

Election panel announces:

- candidates,
- voting status,
- deadline,
- turnout after result.

---

# 285. 208C Phase CM — Cognitive Mode

Show:

```text
Current leader
Stability
Successor
Current election/challenge
Recommended next action
```

Advanced factor details collapsed.

---

# 286. Workstream 208C — Journal / Archive / Legacy

## Goal

Make leadership transitions part of settlement history.

---

# 287. 208C Phase CN — Journal

Log:

- succession,
- election,
- challenge,
- step-down,
- coup,
- vacancy,
- mandate.

---

# 288. 208C Phase CO — Plan 162 Archive

Record landmark:

- first election,
- long-serving leader,
- famous succession,
- coup,
- reform,
- crisis leader.

---

# 289. 208C Phase CP — Plan 206 Death Legacy

Leader death entry can include:

- tenure,
- successor,
- unresolved crisis,
- legitimacy/legacy summary.

---

# 290. 208C Phase CQ — No Duplicate Biography Truth

Leadership history provides structured refs.

Archive/biography projects render them.

---

# 291. Workstream 208C — Human Political Playtest

## Goal

Verify politics feels causal, understandable, and worth planning for.

---

# 292. 208C Phase CR — Playtest Questions

```text
Did succession planning feel useful?
Was it clear why a challenge occurred?
Could the player understand why a candidate won?
Did legitimacy feel grounded in real events?
Did the deputy feel useful without becoming a second leader?
Did a leader death create drama without breaking the campaign?
Did elections happen often enough to matter but not become overhead?
```

---

# 293. 208C Phase CS — Surprise Leadership Death Test

Kill/incapacitate leader unexpectedly.

Player should immediately understand:

- who is acting,
- what succession policy says,
- whether election is needed,
- what effects are active.

---

# 294. 208C Phase CT — Contested Election Test

Two strong candidates.

Review:

- vote explainability,
- support shifts,
- legitimacy outcome,
- aftermath.

---

# 295. 208C Phase CU — Low-Legitimacy Recovery Test

Leader survives scandal/crisis.

Can recover through actual performance.

No irreversible hidden penalty.

---

# 296. Documentation

Create:

```text
docs/systems/LEADERSHIP_SUCCESSION.md
```

Include:

- one-leader authority,
- continuity states,
- succession,
- deputy/acting authority,
- challenge resolution,
- election lifecycle,
- legitimacy derivation,
- term/recall policy,
- Plan 159/202/206 integration,
- save/idempotency,
- adding governance rules.

---

# 297. Data Authoring Guide

Create:

```text
docs/content/LEADERSHIP_RULES_AUTHORING.md
```

Checklist:

```text
1. define governance profile
2. define succession mode
3. define candidate/voter eligibility
4. define challenge/recall thresholds
5. define term rules
6. define tie rule
7. define legitimacy weights
8. define acting-leader policy
9. define transfer policy
10. add targeted selftest fixture
```

---

# 298. Integrated Leadership Pipeline

```text
current LeadershipSystem leader
          │
          ├─ succession plan
          ├─ deputy
          ├─ legitimacy
          └─ governance policy
          │
          ▼
leadership event
          │
   ┌──────┼──────────────┬──────────────┐
   ▼      ▼              ▼              ▼
 death  step-down     challenge       term end
   │      │              │              │
   └──────┼──────────────┼──────────────┘
          ▼
 continuity workflow
          │
     ┌────┼─────────────┐
     ▼    ▼             ▼
 successor acting    election
     │      │             │
     └──────┼─────────────┘
            ▼
 LeadershipTransfer
            │
            ▼
 existing LeadershipSystem
            │
            ├─ relations
            ├─ morale
            ├─ journal
            └─ archive/legacy
```

---

# 299. Current-Leader Authority Contract

Only `LeadershipSystem` owns:

```text
current_leader_id
```

No parallel owner.

---

# 300. Governance Contract

Plan 159 or `ILeadershipGovernancePolicy` owns:

- constitutional rules,
- election mode,
- succession mode,
- voter/candidate eligibility,
- terms,
- recall.

---

# 301. Succession Contract

Succession plan nominates candidates.

Transfer requires current eligibility and governance policy.

---

# 302. Deputy Contract

Deputy is a delegated/acting role.

Not a second permanent leader.

---

# 303. Acting-Leader Contract

Acting leader bridges temporary incapacity/vacancy.

Permanent leader remains unchanged unless formal transfer occurs.

---

# 304. Challenge Contract

Challenge requires:

- grievance,
- eligible challenger,
- minimum support,
- resolution policy.

---

# 305. Coup Contract

LeadershipSystem records coup claim/result.

Plan 202/security/combat resolves physical conflict.

---

# 306. Election Contract

Election owns:

- candidates,
- voters,
- ballots,
- result

inside leadership workflow.

Governance policy owns rules.

---

# 307. Vote Contract

Ballots are deterministic and committed once.

---

# 308. Legitimacy Contract

Legitimacy is derived from explicit factors.

It is not a second persistent social truth.

---

# 309. Relations Contract

Support/voting uses canonical relations.

No duplicate political-support meter.

---

# 310. Skill Contract

Leadership competence uses canonical skills.

---

# 311. Morality Contract

Moral legitimacy uses canonical known moral-choice history.

Secret choices do not affect public support until known.

---

# 312. Death Contract

Survivor fate/death owner emits leader death.

Leadership consumes it.

---

# 313. Conflict Contract

Plan 202 owns interpersonal escalation.

Leadership consumes political outcome.

---

# 314. Transfer Contract

A permanent transfer is atomic and idempotent.

---

# 315. Vacancy Contract

No leader is a valid explicit state.

It must never be represented by dangling dead ID.

---

# 316. Term Contract

Term policy is optional/configurable.

The source 90-day example is not a universal constant.

---

# 317. Recall Contract

Recall is a governance-specific removal process.

Not a generic player button.

---

# 318. Save Contract

Persist:

- succession,
- deputy,
- active political workflows,
- election/transfer history,
- term metadata,
- idempotency.

Do not duplicate:

- relations,
- skills,
- death state,
- morale.

---

# 319. Old-Save Contract

Existing leader remains leader.

No succession history fabricated.

---

# 320. Determinism Contract

Same:

```text
seed
roster
relations
skills
known moral history
governance rules
player choices
```

→ same political outcome.

---

# 321. Attention Contract

Political events are triggered by causes.

No daily random challenge spam.

---

# 322. Content Acceptance Contract

Every leadership transition progresses through:

```text
TRIGGER
→ POLICY RESOLVED
→ ELIGIBILITY VALIDATED
→ WORKFLOW COMMITTED
→ VOTE/CONFLICT/SUCCESSION RESOLVED
→ TRANSFER APPLIED
→ CONSEQUENCES DELIVERED
→ HISTORY RECORDED
```

---

# 323. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| creates second current-leader authority | Medium | Critical | extend existing LeadershipSystem only |
| successor becomes invalid/dead | High | High | transfer-time eligibility |
| source rules conflict with Plan 159 | High | High | governance policy adapter |
| challenge winner/election semantics conflict | High | High | explicit resolution policy |
| coup implemented as simple vote | Medium | High | Plan 202/security/combat handoff |
| legitimacy becomes arbitrary bar | High | High | derived factor assessment |
| elections reroll on save/load | Medium | Critical | committed ballots |
| leader death applies succession twice | Medium | Critical | death/transfer idempotency |
| no leader breaks dependent systems | Medium | Critical | vacancy/acting state |
| term limits create election spam | Medium | High | policy-specific tuning |
| relations dominate voting | Medium | Medium | multi-factor voter model |
| hidden moral choices leak into public legitimacy | Medium | High | knowledge-aware morality |
| political events overwhelm management | Medium | High | cause-driven events + cooldowns |
| old saves instantly trigger overdue election | High | High | migration grace |

---

# 324. Commit Strategy

## 208A — Foundation

### C2[42].1 — baseline + leadership authority ADR

### C2[42].2 — succession/deputy/continuity DTOs

### C2[42].3 — transfer atomicity/idempotency

### C2[42].4 — challenge/election records

### C2[42].5 — governance-policy adapter

### C2[42].6 — deterministic voting/tie policy

### C2[42].7 — legitimacy derivation

### C2[42].8 — term/recall rules

### C2[42].9 — save migration/old-save grace

### C2[42].10 — events/ports/diagnostics

### Gate: 208A complete

---

## 208B — Runtime / UI

### C2[42].11 — successor + backup workflows

### C2[42].12 — incapacity/acting leader

### C2[42].13 — deputy powers/removal

### C2[42].14 — challenge eligibility/support

### C2[42].15 — election lifecycle

### C2[42].16 — recall / challenge policies

### C2[42].17 — coup Plan 202 adapter

### C2[42].18 — legitimacy consequences

### C2[42].19 — term limits / re-election

### C2[42].20 — leadership/succession/deputy UI

### C2[42].21 — challenge/election/history UI

### C2[42].22 — events/hooks/tutorial/localization

### Gate: 208B complete

---

## 208C — Integration / Validation

### C2[42].23 — relations integration

### C2[42].24 — skill/morality integration

### C2[42].25 — Plan 202 conflict integration

### C2[42].26 — Plan 206 death/legacy integration

### C2[42].27 — Plan 159 governance integration

### C2[42].28 — morale/journal/archive integration

### C2[42].29 — save-load/idempotency matrix

### C2[42].30 — exploit prevention

### C2[42].31 — edge-case suite

### C2[42].32 — data-integrity rules

### C2[42].33 — `--leadership-succession-selftest`

### C2[42].34 — deliberate failure proof

### C2[42].35 — 200-day leadership soak

### C2[42].36 — challenge/election/term balance

### C2[42].37 — accessibility/performance/retention

### C2[42].38 — human playtest/docs/release closure

### Gate: 208C complete

---

# 325. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --leadership-succession-selftest
```

Also run repository-canonical equivalents of:

```text
leadership single-authority audit
leadership rules integrity
old-save leader-preservation fixture
leader-death succession idempotency test
deterministic election digest replay
challenge threshold/support test
coup/conflict ownership audit
governance-policy overlap audit
200-day political stability soak
leadership UI accessibility/runtime parity
```

---

# 326. `--leadership-succession-selftest` Acceptance Matrix

| Scenario | Expected |
|---|---|
| Successor | valid designation |
| Backup | valid fallback |
| Invalid successor | rejected/skipped |
| Deputy | appointment persisted |
| Deputy acting | temporary authority |
| Leader death | exactly one workflow |
| Death + successor | policy-valid transfer |
| Death + no successor | emergency election/vacancy |
| Incapacity | acting leadership, not automatic death transfer |
| Recovery | permanent leader resumes if no transfer |
| Step down | policy-driven replacement |
| Challenge <30% | rejected |
| Challenge >=30% | opens |
| Election challenge | forces election |
| Direct contest | policy result |
| Recall | removal vote |
| Coup | external conflict result |
| Election | deterministic ballots |
| Tie | configured deterministic resolution |
| Term end | election if enabled |
| Re-election | policy valid |
| Legitimacy | derived |
| Save/load | exact workflow |
| Old save | incumbent preserved |
| No leader | valid vacancy |
| Frequent challenges | bounded |

---

# 327. Flagship Definition of Done — Foundation

- [ ] existing `LeadershipSystem` extended,
- [ ] one current leader authority,
- [ ] succession plan,
- [ ] designated successor,
- [ ] backup successor,
- [ ] deputy appointment,
- [ ] delegated authority,
- [ ] acting leader,
- [ ] continuity states,
- [ ] leadership challenge,
- [ ] election record,
- [ ] transfer record,
- [ ] deterministic votes,
- [ ] derived legitimacy,
- [ ] optional term limits,
- [ ] recall,
- [ ] rules data,
- [ ] governance adapter,
- [ ] save schema/versioning,
- [ ] old-save leader preservation,
- [ ] events/ports/diagnostics.

---

# 328. Flagship Definition of Done — Runtime / UI

- [ ] death succession,
- [ ] incapacity handling,
- [ ] successor fallback,
- [ ] deputy fallback,
- [ ] emergency election,
- [ ] challenge support threshold,
- [ ] challenge outcome policies,
- [ ] election candidates,
- [ ] eligible voters,
- [ ] turnout,
- [ ] tie/runoff,
- [ ] winner transfer,
- [ ] legitimacy factors/effects,
- [ ] term end,
- [ ] re-election,
- [ ] recall,
- [ ] leadership panel,
- [ ] succession panel,
- [ ] deputy panel,
- [ ] challenge panel,
- [ ] election panel,
- [ ] history,
- [ ] events/hooks,
- [ ] tutorial/tooltips,
- [ ] localization,
- [ ] accessibility.

---

# 329. Flagship Definition of Done — Integration / Validation

- [ ] SurvivorRelationsSystem,
- [ ] SkillProgressionSystem,
- [ ] MoralChoiceSystem,
- [ ] Plan 202 InterpersonalConflict,
- [ ] Plan 206 death/legacy,
- [ ] Plan 159 governance,
- [ ] morale,
- [ ] journal/archive,
- [ ] save/load matrix,
- [ ] transfer idempotency,
- [ ] ballot idempotency,
- [ ] challenge idempotency,
- [ ] old-save grace,
- [ ] step-down/successor/challenge/election exploit guards,
- [ ] no-leader/one-survivor/zero-eligible edges,
- [ ] simultaneous death edge,
- [ ] mid-election death,
- [ ] frequent challenge stress,
- [ ] deterministic replay,
- [ ] data integrity,
- [ ] deliberate failure fixtures,
- [ ] selftest,
- [ ] 200-day soak,
- [ ] legitimacy/challenge/term balance,
- [ ] performance,
- [ ] accessibility,
- [ ] retention,
- [ ] playtest,
- [ ] docs.

---

# 330. Global Definition of Done

- [ ] no second current-leader state,
- [ ] no duplicate relationship support ledger,
- [ ] no duplicate leadership skill,
- [ ] no duplicate death state,
- [ ] no duplicate governance constitution,
- [ ] no local physical coup resolver,
- [ ] no invalid/dead successor transfer,
- [ ] no leaderless soft-lock,
- [ ] no election save-scumming,
- [ ] no duplicate transfer effects,
- [ ] no arbitrary legitimacy drift,
- [ ] no hidden morality leak,
- [ ] no political spam,
- [ ] full verification green.

---

# 331. Closure Report Template

```markdown
## C2[42] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Existing current leader API:
- Leadership state schema:
- Step-down behavior:
- Death invalidation behavior:
- Governance policy availability:
- Plan 202 availability:
- Plan 206 death event:
- Leadership skill:
- Moral-choice input:

### 208A — Foundation
- Succession plan:
- Backup:
- Deputy:
- Acting leader:
- Continuity state:
- Challenge model:
- Election model:
- Transfer model:
- Governance adapter:
- Voting model:
- Tie policy:
- Legitimacy model:
- Term/recall policy:
- Save migration:
- Old-save behavior:
- Missing ports:
- Result:

### 208B — Runtime
- Death succession:
- Incapacity:
- Step-down:
- Challenge:
- Election:
- Recall:
- Coup adapter:
- Deputy:
- Legitimacy:
- Terms:
- UI:
- Events/hooks:
- Result:

### 208C — Integration
- Relations:
- Skills:
- Morality:
- Plan 202:
- Plan 206:
- Plan 159:
- Morale:
- Journal/archive:
- Transfer duplicates:
- Ballot rerolls:
- Challenge duplicates:
- Old-save election regressions:
- Result:

### Balance / Soak
- 200-day transfers:
- vacancy days:
- acting days:
- challenges:
- elections:
- recalls:
- coups:
- average legitimacy:
- challenge notifications:
- term-limit burden:
- political instability findings:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Leadership succession selftest:
- Single-authority audit:
- Leadership rules integrity:
- Old-save fixture:
- Same-seed election digest:
- Coup ownership audit:
- 200-day soak:
- Accessibility:
- Result:

### Final Metrics
- LEADERSHIP_TRANSFERS:
- LEADERSHIP_VACANCY_DAYS:
- LEADERSHIP_ACTING_DAYS:
- LEADERSHIP_CHALLENGES:
- LEADERSHIP_ELECTIONS:
- LEADERSHIP_RECALLS:
- LEADERSHIP_COUPS:
- INVALID_SUCCESSOR_ATTEMPTS:
- DUPLICATE_TRANSFER_VIOLATIONS:
- BALLOT_REROLL_VIOLATIONS:
- CURRENT_LEADER_AUTHORITY_VIOLATIONS:
- GOVERNANCE_DUPLICATION_VIOLATIONS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Governance profiles:
- Campaign/debate content:
- Coup depth:
- Political factions:
- Leader biography:
- UI:
```

---

# 332. Final Execution Directive

Execute Plan 208 as a **continuity and political-transition extension of the existing `LeadershipSystem`**, not as a second leadership simulation.

The critical sequence is:

```text
audit existing LeadershipSystem
→ preserve its current-leader authority
→ add succession/deputy/continuity state
→ define governance-policy adapter
→ make transfers atomic and idempotent
→ derive legitimacy from real accession/support/competence/morality/performance
→ trigger challenges only from real political causes
→ resolve elections through deterministic survivor voting
→ route coups through Plan 202/security/combat
→ consume leader death from Plan 206/canonical fate
→ migrate old saves without displacing the incumbent
→ prove continuity under death, incapacity, vacancy, challenge, and term transitions
```

Do not create a second `current_leader_id`.

Do not automatically install an invalid successor.

Do not make legitimacy a free-floating number modified by arbitrary daily ticks.

Do not let election ballots reroll on reload.

Do not model a coup as merely a different name for a normal vote.

Do not let old saves immediately trigger overdue elections because the campaign is already older than a configured term.

The strongest authority rule is:

> **`LeadershipSystem` remains the one authority for who permanently leads the shelter; succession plans, deputies, challenges, elections, legitimacy, and governance policy exist only to determine when and how that authoritative leader changes.**

The strongest continuity rule is:

> **Leader death or incapacity must produce an explicit deterministic continuity state—successor, backup, acting deputy, emergency election, or vacancy—never a dangling dead leader ID and never a silent leadership disappearance.**

The strongest political rule is:

> **Challenges and elections emerge from real relationships, legitimacy factors, governance rules, and survivor decisions; they are not random events spawned simply because a cooldown elapsed.**

The flagship acceptance scenario is:

> **Start with an existing leader, designate a successor and backup, appoint a deputy with acting authority, and configure an elective governance profile. Save. Kill the leader through the canonical survivor-death path. The system must revalidate the designated successor and, under elective policy, place the valid deputy or successor into the configured acting role while an emergency election opens. Generate two eligible candidates and a 30%+ supported challenger, commit deterministic ballots from canonical relations, competence, and known moral history, then save/load before counting. The same ballots, turnout, winner, legitimacy assessment, and transfer ID must reappear. Apply the transfer once, route morale/relationship consequences through canonical sinks, record the previous leader’s death/legacy through Plan 206/162, and verify the current leader pointer changed exactly once. Re-run with the designated successor dead before the leader: backup/deputy/election fallback must activate without creating a second leader authority or duplicate political effects.**
