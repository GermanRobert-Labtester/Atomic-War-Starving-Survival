# C2 — Flagship Integration Plan [31]: Romance, Partnership, Family Units, and Generational Social Continuity

> **Deliverable:** `C2_planintegration[31].md`
> **Source scope:** Plan 150 — *Romance & Family Dynamics System*
> **Primary objective:** create a deterministic, consent-based romance and family layer where survivor relationships can progress from attraction through courtship and partnership, families can form around canonical survivor/child lineage, and generational consequences persist through caregiving, grief, duty preference, inherited traits, family memory, and epilogue outcomes—without creating a second relationship, cohort, lineage, grief, or survivor-identity authority.
> **Required execution order:** **150A Foundation/System Contract → 150B Courtship/Family/Generational Content → 150C Integration, Save/CI, Balance, and Narrative Closure**
> **Hard dependencies:** `SurvivorRelationsSystem`, `CohortSystem`, `GenerationalLineageExtension`, `CaregivingSystem`, `MentalHealthCrisisSystem`, `DutyRosterSystem`, Plan 40 authored identity, Plan 41 grief/memory, Plan 44 relation effects/history, Plan 31 semantic events, Plan 36 port-contract discipline, Plan 39 save durability, Plan 55 retention.
> **Scope discipline:** no romance score as a second replacement for canonical affinity, no random or inferred sexual/romantic orientation outside authored compatibility/eligibility data, no partnership without mutual eligibility/consent, no forced “Soulmate” trait mutation unless the trait system explicitly supports it, no copying traits on partner death, no duplicate family/lineage graph, no autonomous child creation outside canonical CohortSystem rules, no romance-only morale/efficiency system, and no UI that exposes hidden compatibility or future relationship outcomes as certainty.

---

# 0. Executive Intent

ASHFALL already has the raw ingredients for long-form survivor bonds:

- pair affinity and relationship history,
- parent/child lineage,
- mentorship and caregiving,
- survivor identity and beliefs,
- grief and death,
- generational continuity,
- household/shelter assignments,
- social events,
- epilogue/completion state.

What it lacks is a coherent **relationship-lifecycle layer** that can express:

```text
attraction
→ courtship
→ partnership
→ family formation
→ parent/child/sibling dynamics
→ grief
→ generational legacy
```

The target architecture is:

```text
authored survivor identity
+ canonical pair relations
+ shared history
+ eligibility/consent
           │
           ▼
       RomanceSystem
           │
           ├─ attraction eligibility
           ├─ courtship progression
           ├─ partnership state
           └─ dissolution / loss
           │
           ▼
        FamilySystem
           │
           ├─ family-unit projection
           ├─ parent/child/sibling roles
           ├─ household/social modifiers
           └─ legacy hooks
           │
           ▼
 canonical downstream owners
```

The strongest player-facing outcome is:

> **Survivors can form believable long-term bonds that emerge from existing identity, affinity, history, and circumstances; those bonds matter when assigning rooms, coping with loss, raising children, supporting relatives, and reaching the epilogue—but the game never turns romance into a separate optimization stat detached from the social simulation.**

---

# 1. Source Diagnosis

The source establishes:

- `SurvivorRelationsSystem` already tracks relationships,
- `CohortSystem` already tracks children and parent IDs,
- `GenerationalLineageExtension` already tracks lineage,
- `bondType` exists but romance/family types are unused,
- caregiving exists,
- romance stages are proposed:
  - attraction,
  - courtship,
  - partnership,
  - bonded,
- compatibility factors are proposed:
  - age,
  - beliefs,
  - personality,
  - background,
- family formation should integrate with children/adoption/lineage,
- 20 romance events and 15 family events are expected,
- old saves must load,
- deterministic seeded triggering is required,
- UI should expose relationships/family trees,
- mental health, duty roster, caregiving, lineage, and relations should all integrate.

The key architectural correction is:

```text
romance stage is additional pair metadata
not a competing replacement for affinity/trust/history
```

and:

```text
family unit is a projection over canonical lineage/household relationships
not a second genealogy database.
```

---

# 2. Program-Level Success Criteria

C2[31] closes only when:

1. Romance eligibility is deterministic and state-backed.
2. Mutual consent/eligibility is required for progression into partnership.
3. Romance state survives save/load.
4. Romance does not replace canonical affinity/trust/relationship history.
5. Family units reference canonical parent/child/sibling links.
6. FamilySystem does not duplicate the lineage graph.
7. Parent-child and sibling dynamics produce explainable relation/history changes.
8. Partnership effects route through canonical housing, duty, morale, work, and grief systems.
9. Death of a partner triggers canonical grief consequences.
10. Breakups produce canonical relation/history consequences.
11. Children/adoption are created only through CohortSystem-approved flows.
12. Trait inheritance uses GenerationalLineageExtension rather than bespoke copying.
13. No trait is “inherited” by a surviving romantic partner on death unless a different canonical legacy mechanic explicitly owns that behavior.
14. Relationship progression cannot be rushed through save/load or repeated UI actions.
15. Romance/family events are data-authored and localized.
16. 20 romance events and 15 family events validate and become runtime-observed.
17. Old saves initialize valid empty romance/family extension state.
18. Headless progression works without opening romance UI.
19. UI exposes known relationship state without showing hidden future compatibility math.
20. Long-run seeded tests prove family growth remains bounded and coherent.

---

# 3. Architectural Invariants

## 3.1 `SurvivorRelationsSystem` remains the pair-state authority

Romance adds:

- stage,
- courtship metadata,
- consent/commitment state.

It does not duplicate:

- affinity,
- trust,
- resentment,
- pair history.

## 3.2 CohortSystem remains child/parent authority

FamilySystem references canonical child/parent IDs.

## 3.3 GenerationalLineageExtension remains inheritance authority

No duplicate inherited-trait list owned by FamilySystem if it can be derived or owned there.

## 3.4 Romance progression uses authored eligibility

Do not infer orientation or romantic eligibility from name, sex, gender, age similarity, or arbitrary defaults unless explicitly authored by project data.

## 3.5 Consent is explicit

Partnership/bonded state requires mutual acceptance/eligibility.

## 3.6 Compatibility is a projection, not destiny

High compatibility increases eligibility/progression opportunity.

It never forces a relationship.

## 3.7 Relationship events are deterministic

All event selection uses `ISeededRng`.

## 3.8 Family benefits are contextual and bounded

No permanent broad “family buff” that bypasses existing morale/work/needs systems.

## 3.9 Death/grief uses canonical grief systems

No romance-owned mental-health state.

## 3.10 Family history is retained under Plan 55

No unbounded event logs.

---

# 4. Dependency Graph

```text
40 authored identity ─────────────┐
44 relations/history ─────────────┤
CaregivingSystem ─────────────────┤
                                 ▼
                          150A RomanceSystem
                                 │
                                 ▼
                           partnership state
                                 │
             ┌───────────────────┼───────────────────┐
             ▼                   ▼                   ▼
          housing             duty/work            grief
                                 │
                                 ▼
                           150B FamilySystem
                                 │
                    ┌────────────┼─────────────┐
                    ▼            ▼             ▼
                CohortSystem  LineageExt.   relations
                                 │
                                 ▼
                              150C
                                 │
                         epilogue / legacy
```

---

# 5. Baseline Capture

Before implementation, record:

- current `RelationshipEntry` fields,
- current bond-type vocabulary,
- canonical affinity/trust/history APIs,
- CohortSystem parent/child model,
- adoption/child-generation APIs if any,
- `GenerationalLineageExtension` fields,
- current inherited-trait behavior,
- CaregivingSystem pair/bond logic,
- grief/mental-health APIs,
- room/housing assignment APIs,
- duty roster APIs,
- existing family/kinship tags,
- survivor identity fields relevant to eligibility.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Capture one multi-generation populated save.

---

# 6. Workstream 150A — Foundation / System Contract

## Goal

Create deterministic romance progression as additional pair metadata over canonical relation state, with explicit eligibility/consent and save-safe lifecycle.

---

# 7. 150A Phase A — Define Romance State Types

Create:

```text
Assets/Ashfall.Core/Survivors/RomanceSystem.cs
```

Supporting DTOs:

```text
RomancePairState
RomanceStage
RomanceEligibilityResult
CourtshipEventInstance
PartnershipState
RomanceResolution
```

Avoid one giant DTO.

---

# 8. 150A Phase B — Stage Vocabulary

Initial stages:

```text
None
Attraction
Courtship
Partnership
Bonded
Ended
```

Use explicit terminal/ended state rather than deleting history.

---

# 9. 150A Phase C — Do Not Duplicate Affinity

Remove/avoid an independent `romanceScore` if it acts like a second relationship meter.

Preferred:

```text
romance progression
= stage + event history + commitment state
```

with stage transitions informed by canonical affinity/trust/history.

If a bounded courtship-progress value is needed, it must be narrow and documented as process progress, not emotional truth.

---

# 10. 150A Phase D — Compatibility Query

Create pure:

```text
RomanceCompatibilityAssessment
```

Inputs:

- authored eligibility,
- identity/belief/profile data,
- pair relation state,
- age/role constraints if authored,
- history,
- current commitments.

Output:

```text
eligible
compatibility band
reason codes
blocking reasons
```

---

# 11. 150A Phase E — Authored Romantic Eligibility

Add/identify canonical survivor metadata for:

```text
romance_eligible
relationship preference/eligibility descriptors
```

only if the project chooses to model them.

Do not infer sensitive identity dimensions from unrelated data.

If no preference data exists, the safe product choice is:

```text
explicitly define generic mutual-eligibility policy
```

without pretending to know orientation.

---

# 12. 150A Phase F — Age/Generation Guardrails

Romance eligibility must reject:

- children/minors,
- parent-child,
- siblings,
- close prohibited kinship,
- incompatible life-stage relations.

Use canonical cohort/lineage data.

No “similar age preferred” rule should override safety/kinship constraints.

---

# 13. 150A Phase G — Existing Partnership Guard

Define relationship model:

```text
monogamous default
```

or another explicit project policy.

Do not accidentally allow multiple bonded partnerships because pair rows are independent.

If future polyamory is intended, it requires explicit design rather than emergent duplicate rows.

---

# 14. 150A Phase H — Attraction Eligibility

Initial trigger uses:

- sufficient canonical affinity/trust,
- eligibility,
- compatible history/context,
- no blocking relation,
- cooldown.

Source threshold `affinity > 50` becomes a data-tunable default.

---

# 15. 150A Phase I — Attraction Is Not Automatic

High affinity:

```text
enables attraction event eligibility
```

It does not force attraction.

Use seeded event selection.

---

# 16. 150A Phase J — Courtship Entry

Attraction may move to courtship only after:

- mutual eligibility,
- positive event,
- no hard block,
- seeded/contextual opportunity.

---

# 17. 150A Phase K — Mutual Consent

Partnership requires explicit pair state:

```text
A accepts
B accepts
```

represented deterministically through event outcome/eligibility.

No one-sided forced transition.

---

# 18. 150A Phase L — Bonded Stage

Source suggests:

```text
75+ for 30 days
```

Replace raw “score” dependence with:

```text
sustained partnership
+ strong canonical relation state
+ no unresolved severe conflict
+ sufficient shared-history duration
```

Thresholds data-authored.

---

# 19. 150A Phase M — Partnership Lifecycle

State includes:

```text
start_day
last_meaningful_interaction_day
commitment_state
cohabitation_preference
shared-history references
dissolution status
```

---

# 20. 150A Phase N — Breakup/Dissolution

Triggers may include:

- sustained low affinity,
- betrayal,
- severe unresolved conflict,
- voluntary separation event.

Resolution writes canonical pair-history reasons.

---

# 21. 150A Phase O — Reunion

Ended partnership may become eligible again only through authored cooldown/history rules.

No immediate breakup/reunion loop.

---

# 22. 150A Phase P — Romance Event Source

Create:

```text
RomanceEventCatalogLoader
```

or integrate into canonical event catalog.

Events reference:

- stage,
- eligibility,
- required contexts,
- relation effects,
- localization keys.

---

# 23. 150A Phase Q — Seed Discipline

Use dedicated `ISeededRng` stream:

```text
romance_events
```

Stable candidate sorting by event ID/pair ID.

---

# 24. 150A Phase R — Tick Cadence

Do not roll every frame/day indiscriminately.

Define:

```text
relationship opportunity cadence
minimum days between courtship events
stage cooldowns
```

---

# 25. 150A Phase S — Romance Save State

Persist:

- pair stage,
- courtship/partnership state,
- cooldowns,
- event IDs already consumed where necessary,
- dissolution/reunion state.

Do not persist static compatibility calculations.

---

# 26. 150A Phase T — Old Save Compatibility

Missing romance section:

```text
valid
→ no active romance pair states
```

Existing parent/child/family data remains untouched.

---

# 27. 150A Phase U — Port Contract

Required integrations:

- relations,
- cohort/lineage,
- grief/mental health,
- housing,
- duty,
- quest/event,
- completion/epilogue.

Missing required sink fails validation.

---

# 28. 150A Phase V — Semantic Events

Candidate event kinds:

```text
romance_attraction_started
courtship_event_resolved
partnership_formed
partnership_ended
bonded_partnership_formed
family_unit_formed
partner_died
family_member_reunited
```

Use Plan 31 vocabulary governance.

---

# 29. 150A Phase W — Diagnostics

Developer-only:

```text
ROMANCE_PAIRS_ELIGIBLE
ROMANCE_PAIRS_ACTIVE
COURTSHIPS_ACTIVE
PARTNERSHIPS_ACTIVE
BONDED_PAIRS
ROMANCE_REQUIRED_PORTS_MISSING
```

---

# 30. 150A Tests

- kinship blocking,
- child/minor blocking,
- deterministic eligibility,
- mutual consent,
- stage progression,
- breakup,
- old-save empty state,
- same-seed event ordering,
- missing port failure,
- save round-trip.

---

# 31. 150A Definition of Done

- [ ] RomanceSystem,
- [ ] typed stages,
- [ ] no duplicate affinity meter,
- [ ] compatibility assessment,
- [ ] authored eligibility policy,
- [ ] kinship/life-stage guards,
- [ ] commitment policy,
- [ ] attraction/courtship/partnership/bonded lifecycle,
- [ ] breakup/reunion,
- [ ] event catalog,
- [ ] seeded cadence,
- [ ] save/old-save,
- [ ] ports,
- [ ] events,
- [ ] diagnostics.

---

# 32. Workstream 150B — Courtship / Family / Generational Content

## Goal

Implement romance events, partnership consequences, family-unit projection, parent/child/sibling dynamics, adoption, and generational storytelling without duplicating cohort/lineage authorities.

---

# 33. 150B Phase A — `FamilySystem`

Create:

```text
Assets/Ashfall.Core/Survivors/FamilySystem.cs
```

Responsibilities:

- derive family units from canonical kinship + partnership links,
- expose household/family read model,
- coordinate family events,
- write family-history references.

It does not own the genealogy graph.

---

# 34. 150B Phase B — Family Unit Model

Prefer:

```text
FamilyUnitView
```

derived from:

- bonded/partner pair(s),
- CohortSystem parents/children,
- lineage siblings/extended relations,
- household assignment.

Only persist family-specific state not derivable elsewhere, such as:

- named family tradition,
- family reputation if truly mechanical,
- event cooldowns.

---

# 35. 150B Phase C — Family ID

Use deterministic family identity.

Possible:

```text
hash/canonical ID from founding pair + formation day
```

Persist if referenced by journals/quests.

---

# 36. 150B Phase D — Parent/Child Dynamics

Parent-child behavior uses:

- lineage IDs,
- relation history,
- caregiving,
- mentorship,
- co-location.

Do not create separate parent-child affinity.

---

# 37. 150B Phase E — Sibling Dynamics

Sibling links derive from shared parentage/lineage.

Events may produce:

- support,
- rivalry,
- grief,
- cooperation.

Use normal relation APIs.

---

# 38. 150B Phase F — Adoption

Adoption must use canonical CohortSystem parent/guardian update path.

Do not merely append child ID to a family list.

---

# 39. 150B Phase G — Child Creation / Birth

Source says bonded partners can “have children.”

This must respect the game’s existing CohortSystem and product boundaries.

If child generation already has a canonical reproduction/birth mechanic:

```text
integrate
```

If not:

```text
do not invent biological simulation here
```

Use adoption/lineage hooks until a dedicated child-generation contract exists.

---

# 40. 150B Phase H — Trait Inheritance

Use `GenerationalLineageExtension`.

No direct copy:

```text
child.traits += parent.traits
```

Define inheritance rules in the canonical lineage system.

---

# 41. 150B Phase I — Reject Partner Trait Inheritance on Death

The source proposes:

```text
bonded partners inherit each other's traits on death
```

Do not implement literally unless trait semantics explicitly support memory/legacy transfer.

Preferred:

- grief,
- keepsake,
- memory,
- memorial,
- legacy flag.

Traits remain identity properties.

---

# 42. 150B Phase J — Partnership Housing

Partners may prefer cohabitation.

Housing authority decides:

- available room,
- conflict,
- capacity.

A preference is not a guaranteed assignment.

---

# 43. 150B Phase K — Mutual Support

Source proposes morale bonuses.

Implement via existing relation/needs/morale modifier stack.

Context:

```text
co-located
healthy relationship
not in severe conflict
```

No unconditional permanent +morale.

---

# 44. 150B Phase L — Task Cooperation

Partners/family members may gain bounded coordination bonuses if:

- same assignment,
- relation band supports it.

Use Plan 44 relation-effect query.

Avoid double-counting relation bonus + romance bonus.

---

# 45. 150B Phase M — Protective Behavior

Combat/conflict protection should consume canonical relations/autonomy/combat system.

Do not create a romance-owned combat interception mechanic unless existing combat ports support it.

---

# 46. 150B Phase N — Partner Death

Route through:

```text
death event
→ grief system
→ pair history
→ family history
→ voice/journal
```

Severity can depend on bonded/partnership stage.

---

# 47. 150B Phase O — Family Resource Efficiency

Source proposes “families share resources more efficiently.”

Do not add generic invisible efficiency multiplier without a real mechanism.

Preferred concrete mechanics:

- caregiving,
- co-housing,
- shared meal scheduling,
- childcare labor,
- mutual support.

If no concrete owner exists, omit broad bonus.

---

# 48. 150B Phase P — Child Safety

Family/guardianship may influence child safety only through existing caregiving/location/duty systems.

No abstract “child safety +20” parallel stat.

---

# 49. 150B Phase Q — Family Reputation

If implemented, define what it means.

Potential:

```text
historical descriptor / public family identity
```

Do not add an opaque reputation meter unless another system consumes it.

---

# 50. 150B Phase R — Family Traditions

Source event “The Tradition” can create:

- journal/legacy record,
- ritual/household tag,
- later epilogue reference.

Keep bounded.

---

# 51. 150B Phase S — 20 Romance Events

Required total:

```text
20
```

Balanced across:

- attraction,
- courtship,
- partnership,
- conflict,
- breakup,
- reunion,
- loss.

---

# 52. 150B Phase T — Romance Event Examples

Source examples:

```text
First Sight
The Date
The Proposal
The Wedding
The Breakup
The Loss
The Reunion
The Rivalry
```

Treat “Wedding” as culturally neutral partnership/bonding ceremony unless the world explicitly supports marriage institutions.

---

# 53. 150B Phase U — Avoid Forced Love Triangle Mechanics

“The Rivalry” can be authored as a rare relation event.

Do not make rivalry an automatic result whenever multiple eligible survivors exist.

---

# 54. 150B Phase V — 15 Family Events

Required total:

```text
15
```

Balanced across:

- parent/child,
- sibling,
- adoption,
- reunion,
- inheritance,
- conflict,
- tradition,
- loss.

---

# 55. 150B Phase W — Family Event Examples

Source:

```text
The Birth
The Graduation
The Reunion
The Inheritance
The Feud
The Tradition
```

Map “birth” only if canonical child-creation system exists.

---

# 56. 150B Phase X — Quest Hooks

Source concepts:

```text
The Matchmaker
The Rival
The Proposal
The Family
The Orphan
The Legacy
```

Use quest runtime.

Do not make Matchmaker override survivor consent.

---

# 57. 150B Phase Y — Courtship Interaction Types

Potential:

- shared meal,
- conversation,
- walk/outing,
- gift,
- shared duty,
- care during illness.

Each requires context and relation consequences.

---

# 58. 150B Phase Z — Gift Mechanics

Gift uses actual inventory transaction.

No free narrative gift.

---

# 59. 150B Phase AA — Shared Meal

Use real meal/event/consumption system if available.

Do not create fake food consumption.

---

# 60. 150B Phase AB — Shared Duty

Uses actual duty/roster assignment.

Can create opportunity for courtship event.

---

# 61. 150B Phase AC — Caregiving-to-Romance

Source allows caregiving to lead to romance.

Guardrail:

```text
caregiving creates opportunity/context
not automatic attraction
```

No exploiting medical dependence.

---

# 62. 150B Phase AD — Romance UI

Prefer integrate into existing Relations panel.

Show:

- known stage,
- partnership status,
- shared history,
- family links,
- known major events.

Do not expose hidden compatibility score.

---

# 63. 150B Phase AE — Family Tree UI

Build from canonical lineage data.

Do not maintain a UI-only family graph.

---

# 64. 150B Phase AF — Tooltips

Survivor tooltip may show:

- partner,
- children,
- guardians,
- siblings,
- family name/ID if relevant.

Only player-known/obvious kinship.

---

# 65. 150B Phase AG — Journal

Log significant events:

- partnership,
- breakup,
- adoption,
- child maturation,
- major family feud,
- death/loss.

Avoid every courtship tick.

---

# 66. 150B Phase AH — Tutorial

First attraction/partnership opportunity explains:

- relationships emerge autonomously,
- player can support/enable context,
- consent/eligibility matter,
- family is optional.

Do not teach hidden formulas.

---

# 67. 150B Phase AI — Localization

All:

- event names,
- choices,
- relationship labels,
- family labels,
- UI text

use localization keys.

---

# 68. 150B Phase AJ — Content Integrity

Validate:

- event IDs,
- stage refs,
- survivor context refs,
- item refs for gifts,
- quest refs,
- localization,
- kinship constraints,
- family-event prerequisites.

---

# 69. 150B Phase AK — Content Utilization

Run 200-day/multi-generation seeded scenarios.

Report:

```text
romance events eligible
courtships started
partnerships formed
breakups
family units
adoptions
family events
dead templates
```

---

# 70. 150B Definition of Done

- [ ] FamilySystem,
- [ ] no duplicate genealogy,
- [ ] adoption through CohortSystem,
- [ ] trait inheritance through lineage authority,
- [ ] no partner-trait copying on death,
- [ ] cohabitation preference,
- [ ] bounded support/work effects,
- [ ] grief integration,
- [ ] 20 romance events,
- [ ] 15 family events,
- [ ] quest hooks,
- [ ] gift/meal/duty interactions use canonical systems,
- [ ] Relations/family-tree UI,
- [ ] journal/tutorial/tooltips,
- [ ] localization,
- [ ] utilization report.

---

# 71. Workstream 150C — Integration / Consequences / Validation

## Goal

Close romance/family consequences through canonical systems and prove consent, lineage integrity, save durability, deterministic progression, and long-run narrative coherence.

---

# 72. 150C Phase A — Relations Integration

Romance stage writes through/alongside `RelationshipEntry`.

Recommended:

```text
bondType = romantic_partner / bonded_partner
```

only if `bondType` is the canonical field.

Do not create separate pair registry if relation system can own it.

---

# 73. 150C Phase B — Pair History Reasons

Write reasons:

```text
attraction_started
courtship_succeeded
partnership_formed
partnership_ended
partner_supported
partner_betrayed
partner_died
reunion
```

No unexplained relation jumps.

---

# 74. 150C Phase C — Cohort Integration

Parent/child/adoption data remains in CohortSystem.

FamilySystem queries it.

---

# 75. 150C Phase D — Lineage Integration

Trait inheritance events call GenerationalLineageExtension.

Assert:

```text
child inherited traits
→ lineage provenance explains source parent
```

---

# 76. 150C Phase E — Caregiving Integration

Caregiving may:

- improve pair history,
- enable courtship event eligibility.

It does not directly set romance stage.

---

# 77. 150C Phase F — MentalHealth Integration

Partner death:

```text
grief severity input
```

to canonical mental-health/grief system.

No duplicate “romance grief” state.

---

# 78. 150C Phase G — Duty Roster Integration

Partners may prefer same shifts.

Preference is advisory/weighted.

Never force an unsafe assignment.

---

# 79. 150C Phase H — Housing Integration

Cohabitation preference goes through housing allocation.

Full shelter can deny cohabitation.

No relationship break merely because no room exists unless event rules say so.

---

# 80. 150C Phase I — Autonomy Integration

If survivor autonomy is active:

- courtship events can originate from autonomous behavior,
- but must respect player schedule/availability and safety.

No romance event teleports survivors away from duties.

---

# 81. 150C Phase J — Family Formation

Family formation occurs when:

- partnership/bond qualifies,
- canonical family/household prerequisites pass,
- event is committed.

No duplicate family on reload.

---

# 82. 150C Phase K — Save/Load Matrix

Test at:

```text
pre-attraction
attraction
courtship
pre-partnership
partnership
bonded
breakup
family formed
adoption
partner death
```

Reload preserves exact state.

---

# 83. 150C Phase L — Save-Scum Prevention

Reload may not reroll:

- attraction pair,
- courtship event,
- acceptance/rejection outcome,
- breakup event.

Persist event identity/outcome where needed.

---

# 84. 150C Phase M — Old Save Compatibility

Old save:

```text
no romance state
```

loads cleanly.

Existing parents/children still produce family-tree projection.

---

# 85. 150C Phase N — Existing Kinship on Migration

If old save already has:

- parents,
- children,
- siblings,

FamilySystem should derive these immediately without fabricating romance.

---

# 86. 150C Phase O — No-Romance Scenario

A campaign with:

- no eligible pairs,
- no interest,
- or low affinity

must remain valid.

Family gameplay can still exist through existing parent/adoption lineage.

---

# 87. 150C Phase P — All-Eligible Stress Scenario

Source asks “all survivors bonded.”

Better test:

```text
many eligible pairs
```

Assert:

- no overlapping invalid partnerships,
- no combinatorial event spam,
- household/family count remains coherent.

---

# 88. 150C Phase Q — Kinship Safety Test

Ensure no romantic relationship between:

- parent/child,
- siblings,
- prohibited close kin.

---

# 89. 150C Phase R — Minor Safety Test

No minor/child participant can enter romance pipeline.

This is a hard invariant.

---

# 90. 150C Phase S — Consent Test

No partnership if one side rejects/is ineligible.

Player/quest cannot override.

---

# 91. 150C Phase T — Family Graph Integrity

No:

- duplicate parent edges,
- cyclic parentage,
- missing child references,
- family tree orphan links.

---

# 92. 150C Phase U — Breakup Consequences

Breakup may affect:

- affinity,
- housing preference,
- duty preference,
- voice/journal,
- family household.

Children/lineage remain intact.

---

# 93. 150C Phase V — Partner Death Consequences

Death:

- relationship becomes historical,
- grief fires once,
- family links remain,
- no dangling active partner state.

---

# 94. 150C Phase W — Epilogue / Completion Record

Record landmark states:

- lasting partnership,
- major breakup,
- surviving family,
- adopted child,
- generational continuation,
- famous family tradition.

Use canonical completion/epilogue path.

---

# 95. 150C Phase X — Voice Integration

Plan 42 voice can reference:

- partner,
- family,
- grief,
- reunion.

Must respect player-known/live relationship state.

---

# 96. 150C Phase Y — UI Accessibility

Relations/family tree:

- keyboard,
- controller if supported,
- text labels,
- non-color relation distinctions,
- scalable layout,
- screen-reader/accessibility names where supported.

---

# 97. 150C Phase Z — Headless Behavior

Romance/family progression must continue without UI.

No panel-driven stage changes.

---

# 98. 150C Phase AA — `--romance-family-selftest`

Required scenarios:

1. eligible pair attraction,
2. ineligible kinship pair rejected,
3. minor rejected,
4. courtship progression,
5. one-sided rejection,
6. partnership,
7. breakup,
8. family projection,
9. adoption,
10. trait inheritance,
11. partner death grief,
12. old save,
13. save/load determinism.

---

# 99. 150C Phase AB — Data Integrity

Validate:

- event templates,
- stage refs,
- quest refs,
- item refs,
- localization,
- eligibility/kinship policies,
- family-event prerequisites.

---

# 100. 150C Phase AC — Deliberate Failure Proof

Break:

- invalid kinship rule,
- duplicate partnership,
- missing lineage ref,
- stale partner state after death.

Assert selftest/gate fails.

---

# 101. 150C Phase AD — 200-Day Social Soak

Record:

```text
eligible pairs
attractions
courtships
partnerships
bonded pairs
breakups
family formations
adoptions
family events
grief events
```

---

# 102. 150C Phase AE — Multi-Generation Soak

Run through:

- child growth/maturation,
- parent death,
- sibling relations,
- new adult partnerships if eligible.

Validate lineage continuity.

---

# 103. 150C Phase AF — Relationship Spam Budget

Define caps:

```text
max courtship events/day
max active courtships
cooldown per pair
```

Prevent simulation from becoming romance-event spam.

---

# 104. 150C Phase AG — Mechanical-Bonus Budget

Audit:

- morale,
- work,
- housing,
- duty,
- caregiving,
- relation effects.

Ensure romance does not double-count the same bond across multiple systems.

---

# 105. 150C Phase AH — Balance Profiles

Run:

```text
family_dense
family_sparse
no_romance
high_conflict
```

Compare:

- morale,
- productivity,
- grief burden,
- housing pressure,
- event frequency.

No family structure should become mandatory meta.

---

# 106. 150C Phase AI — Narrative Fairness Playtest

Evaluate:

```text
Did attraction feel earned?
Did courtship feel too mechanical?
Did rejection feel legitimate?
Did family events emerge from state?
Did grief reflect relationship depth?
```

Use human review for emotional quality.

---

# 107. 150C Phase AJ — Documentation

Create:

```text
docs/systems/ROMANCE_AND_FAMILY.md
```

Include:

- authority boundaries,
- eligibility/consent,
- romance lifecycle,
- family projection,
- lineage integration,
- save contract,
- adding events.

---

# 108. 150C Definition of Done

- [ ] relation/bondType integration,
- [ ] pair-history reasons,
- [ ] CohortSystem integration,
- [ ] lineage inheritance integration,
- [ ] caregiving integration,
- [ ] mental-health/grief integration,
- [ ] duty/housing preferences,
- [ ] save/load lifecycle matrix,
- [ ] save-scum prevention,
- [ ] old-save compatibility,
- [ ] existing family projection,
- [ ] no-romance valid scenario,
- [ ] many-pair stress scenario,
- [ ] kinship safety,
- [ ] minor exclusion,
- [ ] consent enforcement,
- [ ] graph integrity,
- [ ] breakup/death closure,
- [ ] epilogue/legacy,
- [ ] voice,
- [ ] accessibility,
- [ ] headless selftest,
- [ ] deliberate failure proof,
- [ ] 200-day soak,
- [ ] multi-generation soak,
- [ ] event-spam budget,
- [ ] mechanical-bonus audit,
- [ ] balance profiles,
- [ ] narrative playtest,
- [ ] docs.

---

# 109. Integrated Romance / Family Pipeline

```text
authored identity
+ canonical relation state
+ kinship constraints
+ shared history
           │
           ▼
   RomanceCompatibilityAssessment
           │
           ▼
      RomanceSystem
           │
   attraction / courtship /
  partnership / bonded / ended
           │
           ▼
       FamilySystem
           │
           ├─ partnership household
           ├─ Cohort parent/child links
           ├─ lineage siblings
           └─ family history
           │
           ▼
 canonical downstream systems
```

---

# 110. Relationship Authority Contract

Affinity/trust/history remain in `SurvivorRelationsSystem`.

Romance adds stage/commitment metadata only.

---

# 111. Eligibility Contract

Eligibility must pass:

- adulthood,
- kinship safety,
- commitment policy,
- authored romantic eligibility,
- pair relation prerequisites.

---

# 112. Consent Contract

Partnership transition requires mutual acceptance.

No quest/player override.

---

# 113. Compatibility Contract

Compatibility is:

```text
advisory probability/eligibility input
```

not a destiny score shown to the player.

---

# 114. Stage Contract

Stage changes only through committed deterministic events.

No raw threshold auto-flip every tick.

---

# 115. Cohort Contract

Children/parents belong to CohortSystem.

FamilySystem references, never duplicates.

---

# 116. Lineage Contract

Trait inheritance belongs to GenerationalLineageExtension.

Every inherited trait has provenance.

---

# 117. Family Contract

A family unit is a social projection around canonical kinship/partnership links.

Only family-specific traditions/history require separate persistence.

---

# 118. Adoption Contract

Adoption updates canonical guardian/parentage state.

No family-list-only adoption.

---

# 119. Partner Death Contract

Death closes active romantic state and opens grief/history consequences exactly once.

---

# 120. Breakup Contract

Breakup ends partnership, not lineage.

Children remain linked to both parents/guardians.

---

# 121. Housing Contract

Cohabitation is a preference.

Housing authority owns final placement.

---

# 122. Duty Contract

Same-shift preference may influence scheduling.

Safety/fitness/role needs outrank romance.

---

# 123. Mechanical Bonus Contract

Romance/family benefits must be concrete and non-overlapping.

No generic permanent “family efficiency” stat.

---

# 124. Save Contract

Persist:

- romance stage/commitment,
- courtship cooldown/event state,
- family-specific traditions/history references.

Do not persist:

- duplicated affinity,
- duplicated lineage,
- derived compatibility,
- derived family tree.

---

# 125. Old-Save Contract

Old saves gain no fabricated romances.

Existing kinship renders immediately through FamilySystem.

---

# 126. Determinism Contract

Same:

```text
seed
state
player actions
```

→ same relationship event sequence.

---

# 127. Retention Contract

Keep:

- recent courtship/family events,
- landmark partnership/breakup/death/adoption,
- generational legacy.

Roll up low-value old event detail under Plan 55.

---

# 128. UI Contract

UI shows actual known relationship/family state.

No hidden compatibility number or future success probability.

---

# 129. Human-Review Contract

Emotional believability is a playtest quality dimension, not a deterministic CI claim.

---

# 130. Content Acceptance Contract

Romance/family events progress through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ EVENT_PRODUCED
→ CONSEQUENCE_PRODUCED
→ PLAYER_VISIBLE
```

---

# 131. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| romance becomes a second affinity meter | Medium | High | stage metadata over canonical relations |
| eligibility inferred from sensitive attributes | Medium | Critical | explicit authored policy only |
| kinship bugs create invalid pairings | Low–Med | Critical | hard lineage guards |
| child/minor enters romance pipeline | Low | Critical | hard invariant/selftest |
| romance progression feels mechanical | High | Medium | event/state based + human playtest |
| family graph duplicates lineage | Medium | High | projection-only FamilySystem |
| trait inheritance duplicates lineage logic | Medium | High | GenerationalLineageExtension owns |
| partner-death trait copying corrupts identity | Medium | High | reject literal source behavior |
| support bonuses double-count relation effects | Medium | High | mechanical-bonus audit |
| courtship event spam | Medium | Medium | cadence/caps |
| family becomes optimal mandatory meta | Medium | High | sparse/no-romance balance profile |
| save/load rerolls acceptance/rejection | Medium | High | persist event identity/outcome |

---

# 132. Commit Strategy

## 150A — Foundation

### C2[31].1 — baseline + romance/family ADR

### C2[31].2 — romance stages/state DTOs

### C2[31].3 — eligibility/kinship/consent policy

### C2[31].4 — compatibility assessment

### C2[31].5 — attraction/courtship lifecycle

### C2[31].6 — partnership/bonded/breakup lifecycle

### C2[31].7 — save/old-save/seed cadence

### C2[31].8 — ports/events/diagnostics

### Gate: 150A complete

---

## 150B — Content / Family

### C2[31].9 — FamilySystem projection

### C2[31].10 — Cohort/adoption/lineage integration

### C2[31].11 — housing/duty/support mechanics

### C2[31].12 — grief/family-history mechanics

### C2[31].13 — 20 romance events

### C2[31].14 — 15 family events

### C2[31].15 — quest/gift/meal/duty hooks

### C2[31].16 — UI/journal/tutorial/localization

### C2[31].17 — content-utilization report

### Gate: 150B complete

---

## 150C — Closure

### C2[31].18 — relation/bondType integration

### C2[31].19 — save-load/idempotency matrix

### C2[31].20 — kinship/minor/consent hard tests

### C2[31].21 — breakup/death/epilogue closure

### C2[31].22 — selftest + deliberate failure proof

### C2[31].23 — 200-day relationship soak

### C2[31].24 — multi-generation soak

### C2[31].25 — bonus/spam/balance audits

### C2[31].26 — narrative playtest/docs

### Gate: 150C complete

---

# 133. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --romance-family-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
port-contract validation
romance/family content-utilization report
old-save fixture load
same-seed relationship replay
200-day social soak
multi-generation lineage soak
relations/family-tree snapshot-accessibility gate
```

---

# 134. Flagship Definition of Done

## 150A — Foundation

- [ ] RomanceSystem,
- [ ] typed stages,
- [ ] no duplicate romance score/affinity authority,
- [ ] explicit eligibility policy,
- [ ] kinship guards,
- [ ] minor exclusion,
- [ ] mutual consent,
- [ ] compatibility projection,
- [ ] attraction/courtship/partnership/bonded lifecycle,
- [ ] breakup/reunion,
- [ ] deterministic event cadence,
- [ ] save/old-save,
- [ ] ports,
- [ ] semantic events,
- [ ] diagnostics.

## 150B — Content / Family

- [ ] FamilySystem as projection,
- [ ] CohortSystem remains parent/child authority,
- [ ] adoption canonical,
- [ ] lineage inheritance canonical,
- [ ] no partner trait copying on death,
- [ ] housing/duty preferences,
- [ ] bounded support mechanics,
- [ ] grief integration,
- [ ] 20 romance events,
- [ ] 15 family events,
- [ ] quest hooks,
- [ ] gift/meal/duty interactions canonical,
- [ ] Relations/family-tree UI,
- [ ] journal/tutorial/tooltips,
- [ ] localization,
- [ ] utilization report.

## 150C — Integration

- [ ] bondType/relation integration,
- [ ] pair-history reasons,
- [ ] save/load matrix,
- [ ] save-scum prevention,
- [ ] old saves,
- [ ] existing kinship projection,
- [ ] no-romance valid campaign,
- [ ] many-pair stress case,
- [ ] kinship/minor/consent tests,
- [ ] family graph integrity,
- [ ] breakup closure,
- [ ] partner-death closure,
- [ ] epilogue/legacy,
- [ ] voice integration,
- [ ] accessibility,
- [ ] headless,
- [ ] selftest,
- [ ] failure proof,
- [ ] 200-day soak,
- [ ] multi-generation soak,
- [ ] event-spam budget,
- [ ] bonus double-count audit,
- [ ] balance profiles,
- [ ] narrative playtest,
- [ ] docs.

## Global

- [ ] no duplicate relation authority,
- [ ] no duplicate lineage authority,
- [ ] no unsafe eligibility inference,
- [ ] no invalid kinship pair,
- [ ] no minor romantic participation,
- [ ] no forced partnership,
- [ ] no family-required optimal meta,
- [ ] full verification green.

---

# 135. Closure Report Template

```markdown
## C2[31] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- RelationshipEntry fields:
- bondType values:
- Cohort parent/child links:
- Lineage fields:
- Caregiving links:
- Existing romance refs:

### 150A — Foundation
- Romance stages:
- Eligibility policy:
- Kinship guard:
- Consent:
- Compatibility:
- Active courtships:
- Partnerships:
- Save schema:
- Old save:
- Missing ports:
- Result:

### 150B — Family / Content
- Family units:
- Cohort integration:
- Adoption:
- Inheritance:
- Housing:
- Duty:
- Grief:
- Romance events:
- Family events:
- Quests:
- UI:
- Unused content:
- Result:

### 150C — Closure
- Pair-history reasons:
- Save-load rerolls:
- Kinship violations:
- Minor violations:
- Consent violations:
- Graph integrity:
- Breakups:
- Partner-death grief:
- Epilogue inputs:
- 200-day soak:
- Multi-generation soak:
- Bonus double-count findings:
- Narrative playtest:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Romance-family selftest:
- Port contract:
- Old-save fixtures:
- Same-seed replay:
- Content utilization:
- 200-day soak:
- Multi-generation soak:
- Verify fast:

### Final Metrics
- ROMANCE_EVENTS:
- FAMILY_EVENTS:
- ELIGIBLE_PAIRS:
- COURTSHIPS_STARTED:
- PARTNERSHIPS_FORMED:
- BONDED_PAIRS:
- BREAKUPS:
- FAMILY_UNITS:
- ADOPTIONS:
- INVALID_KINSHIP_PAIRINGS:
- MINOR_ELIGIBILITY_VIOLATIONS:
- CONSENT_VIOLATIONS:
- SAVE_REROLL_VIOLATIONS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Romance content:
- Family content:
- Child generation:
- Housing:
- Voice:
- UI:
```

---

# 136. Final Execution Directive

Execute Plan 150 as a **relationship-lifecycle and family-projection layer over the existing survivor relations, cohort, lineage, caregiving, grief, housing, and duty authorities**.

The critical sequence is:

```text
establish explicit eligibility and kinship safety
→ treat compatibility as an opportunity input, not destiny
→ progress attraction/courtship through deterministic events
→ require mutual consent for partnership
→ store romance stage without duplicating affinity
→ derive families from canonical partnership + lineage
→ route adoption/inheritance through Cohort/Lineage
→ route support/grief/housing/duty through existing systems
→ preserve save/load determinism
→ prove long-run multi-generation coherence
```

Do not create a second affinity meter.

Do not infer romantic eligibility from unrelated sensitive attributes.

Do not permit minor/close-kin romance.

Do not let a quest or player override mutual consent.

Do not duplicate genealogy.

Do not copy one partner’s identity traits onto another after death.

The strongest social rule is:

> **Romance extends the meaning of an existing relationship; it does not replace the relationship system that already owns affinity, trust, and history.**

The strongest family rule is:

> **A family is a projection of real partnership, parentage, guardianship, and lineage—not a second family graph that can drift from CohortSystem.**

The strongest narrative rule is:

> **Relationship progression must feel earned through shared state and history, while rejection, breakup, grief, adoption, and generational continuity remain mechanically real and explainable.**

The flagship acceptance scenario is:

> **Start with two eligible adult survivors who already have strong canonical affinity and shared history, plus one ineligible close-relative pair and one minor. Run a seeded courtship sequence, save/load before partnership, confirm one pair forms only after mutual acceptance, then derive their household/family projection without duplicating lineage. Adopt an orphan through CohortSystem, inherit one trait through GenerationalLineageExtension, kill one partner in a controlled test, and verify grief/history/epilogue consequences fire exactly once while the family graph remains valid. The ineligible kinship pair and minor must never enter romance eligibility, and the same seed/actions must reproduce the same progression across reloads.**
