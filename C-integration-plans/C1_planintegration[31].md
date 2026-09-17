# C1 — Flagship Integration Plan [31]: Survivor Aging, Functional Capacity, Retirement, Mentorship & Generational Continuity

> **Output:** `C1_planintegration[31].md`
>
> **Source baseline:** Plan 176 — Aging & Elderly Survivor System
>
> **Primary mission:** make survivors age in campaign time, preserve birth/recruitment chronology, expose life-stage context, and integrate age-related capability, retirement, care needs, mentorship, mortality, family continuity, governance participation, memorials, and archive history through the systems that already own those facts.
>
> **Primary architectural rule:** chronological age is a survivor lifecycle fact. **Functional capacity is not age itself.** Work, combat, expedition readiness, cognition, health, care requirements, and mortality must remain governed by the existing fitness, needs, affliction, medical, skill, caregiving, autonomy, and mortality authorities.
>
> **Primary modeling correction:** the source proposes life-stage-wide `physicalModifier`, `mentalModifier`, and `wisdomModifier` values. Those are too coarse as a gameplay truth. The flagship instead treats life stage as a readable demographic classification and allows age to contribute bounded, data-driven modifiers only where a real downstream capability model explicitly consumes them.
>
> **Primary persistence correction:** old saves must **not** receive guessed “estimated ages” unless a reliable source exists. Migration uses canonical authored age/date-of-birth data where available; otherwise it preserves an explicit unknown/legacy age state or a safe deterministic fallback documented in an ADR.
>
> **Mandatory execution order:** 176A survivor chronology/lifecycle audit → 176B canonical age representation + life-stage projection → 176C functional-capacity composition → 176D retirement/care/mentorship → 176E mortality/family/archive/seasonal continuity → 176F UI, persistence, determinism, balance and CI → 176G advanced demographics/traits/ceremonies only after the base model is proven.
>
> **Critical re-baseline rule:** before creating `AgingSystem`, inspect `SurvivorLifecycle`, survivor definition fields, Plan-24 fitness, Plan-137 needs→performance, Plan-143 affliction capability, `CaregivingSystem`, `SkillProgressionSystem`, Plan-140 `GenerationalSuccessionEngine`, Plan-150 family/romance, Plan-159 governance, Plan-162 archive, Plan-170 seasonal events, memorial/death-quality systems, survivor relations, and save contracts. Extend the real chronology/lifecycle owner rather than creating parallel age truth.
>
> **Guardrails:** no second survivor lifecycle; no duplicated age list if survivor state can own birth/recruitment chronology; no universal “elderly = 0.5 work”; no universal “young adult = peak physical”; no age-based mental debuff without a real cognitive model; no “wisdom” stat added only for age unless the skill/decision system already has a compatible capability; no age-based death lottery detached from canonical health/mortality; no retirement that silently makes survivors unavailable without autonomy/assignment rules; no passive mentorship aura; no per-frame aging; no wall-clock age; no GUID/unseeded RNG; no forced birthdays every 30 campaign days unless the campaign time scale deliberately defines that mapping; no arbitrary morale bonus for every birthday; no old-save age guessing from appearance/name; no new elder-care health engine inside aging; no duplicate grief or memorial effects; no age-based discrimination baked into policy defaults.

---

# 0. Mission

ASHFALL already models survivor life and death, but not the passage between them.

The source baseline identifies the current break:

```text
SURVIVOR RECRUITED AT AGE 25
         │
         ├── Day 1
         ├── Day 100
         ├── Day 365
         └── still mechanically age 25
```

Meanwhile:
- `SurvivorLifecycle` owns birth/death-like lifecycle facts;
- `CaregivingSystem` already refers to elderly care;
- `SkillProgressionSystem` owns learning;
- `GenerationalSuccessionEngine` has aging/retirement skeletons but `AdvanceTime()` is reportedly unwired;
- family, memorial, archive, governance, fitness, needs, medical, and relationship systems already exist or are planned.

The missing layer is therefore not “add old-person penalties.”

It is:

```text
CANONICAL CAMPAIGN CHRONOLOGY
           │
           ▼
SURVIVOR AGE / BIRTH CHRONOLOGY
           │
           ├── exact age
           ├── life stage
           └── milestone transitions
           │
           ▼
AGE CAPABILITY CONTRIBUTIONS
           │
           ├────────► fitness / duty eligibility
           ├────────► Plan-137 performance composition
           ├────────► expedition readiness
           ├────────► combat capability
           ├────────► learning / mentorship
           └────────► caregiving need projection
           │
           ▼
LIFE-COURSE DECISIONS
           │
           ├── retirement
           ├── reduced-duty choice
           ├── mentorship
           ├── care
           ├── leadership continuity
           └── family/generational roles
           │
           ▼
LIFECYCLE OUTCOMES
           │
           ├── illness/frailty
           ├── natural death where supported
           ├── memorial
           ├── grief
           └── archive/legacy
```

Age should matter.

But it should matter because it changes **context and probabilities/capabilities through real systems**, not because the game stamps a simplistic debuff onto everyone over 60.

---

# 1. Source-Evidence Interpretation

## 1.1 Aging is genuinely unwired

The source reports:
- no `AgingSystem`;
- no meaningful elderly/old-age implementation;
- `GenerationalSuccessionEngine.AdvanceTime()` exists but is not called.

That makes chronology integration a legitimate gap.

## 1.2 `SurvivorLifecycle` should probably remain age authority

If survivors already have:
- birth date;
- recruitment age;
- birth/recruitment event;
then a separate persisted `AgingState` list of ages is likely duplicate state.

Preferred model:

```text
birth_day / birth_date-like campaign field
+ campaign day
→ current chronological age
```

or:

```text
age_at_recruitment
+ recruitment_day
+ elapsed campaign years
→ current age
```

## 1.3 “1 year every 30 days” is a game-time design choice, not a default fact

This mapping dramatically affects campaign meaning.

If the campaign lasts 120–180 days, 30 days/year means:
- 4–6 years pass.

That may be intentional.

But it must be reconciled with:
- seasons;
- children/generations;
- pregnancies/family;
- archive dates;
- long-term succession.

The time-scale ADR must be explicit.

## 1.4 Life-stage modifiers should not replace fitness/medical systems

A 65-year-old healthy survivor and a 45-year-old severely ill survivor should not be flattened into age bands.

Age contributes;
fitness/health decide actual capability.

## 1.5 Wisdom is not automatically a universal stat

If leadership, teaching, decision-making, or skill checks already expose:
- experience;
- knowledge;
- proficiency;
then age can influence those rails.

Do not create a generic `wisdomModifier` with no owner.

## 1.6 Retirement is a social/work status, not a biological stage

A survivor can:
- be elderly and still work;
- be middle-aged and retire early;
- reduce duties without formal retirement.

Retirement should be explicit state/choice.

## 1.7 Elder care should reuse `CaregivingSystem`

Age can contribute to **care need**.

Caregiving remains the owner of:
- caregiver assignment;
- care delivery;
- care load.

## 1.8 Old-age death must integrate with mortality/health

Age should modify mortality risk only if the canonical death system supports it.

Do not add:
- `if age > 70 roll death`.

That would bypass:
- health;
- disease;
- treatment;
- palliative care;
- death quality;
- memorials.

---

# 2. Non-Negotiable Aging Invariants

## INV-176.1 — One chronology authority

Campaign time has one owner.

## INV-176.2 — One survivor age authority

Age is stored/derived once.

## INV-176.3 — Age is monotonic

Chronological age cannot decrease.

## INV-176.4 — Life stage is derived

Do not persist stage if it can be calculated from age + stage catalog.

## INV-176.5 — Retirement is explicit

Retirement is not automatically identical to crossing an age threshold.

## INV-176.6 — Functional capacity is not chronological age

Age contributes to capability; it does not directly override fitness/medical state.

## INV-176.7 — No universal mental decline

Only real cognitive/skill systems may consume age-related cognitive effects.

## INV-176.8 — No universal wisdom stat without authority

## INV-176.9 — Care need is derived from real state

Age alone does not automatically make someone dependent.

## INV-176.10 — Caregiving owns care delivery

## INV-176.11 — Mentorship uses real skill-transfer authority

No passive global bonus.

## INV-176.12 — Retirement uses real duty/autonomy systems

## INV-176.13 — Natural death uses canonical mortality

## INV-176.14 — Death consequences are exactly once

Grief, memorial, archive, succession.

## INV-176.15 — Birthdays/milestones are semantic events

No daily event spam.

## INV-176.16 — Time scale is globally consistent

Aging year length must align with:
- seasons;
- family/children;
- archive chronology;
- succession.

## INV-176.17 — Old-save age migration is evidence-based

No speculative age guessing.

## INV-176.18 — Long-lived survivors remain possible

Age increases risk/cost, not deterministic arbitrary removal at 70.

## INV-176.19 — No age discrimination hidden in system defaults

Policies may treat age categories only through explicit authored governance rules.

## INV-176.20 — Headless aging is deterministic

---

# 3. Definition of Done

Plan 176 closes only when:

- canonical campaign chronology is identified;
- current survivor age/birth/recruitment fields are audited;
- `GenerationalSuccessionEngine.AdvanceTime()` is either wired into the canonical day/year progression or retired as duplicate;
- one age representation exists;
- life-stage thresholds are data-driven and derived;
- campaign-days-per-year mapping is explicitly decided;
- leap/partial-year semantics are deterministic;
- age transitions occur exactly once;
- work/expedition/combat effects enter existing fitness/performance composition exactly once;
- no life-stage-wide hidden multipliers bypass Plan 24/137/143;
- learning/mentorship integrates with `SkillProgressionSystem`;
- retirement is optional and explicit unless another system mandates incapacity;
- retired survivors can still contribute through configured low-load roles;
- elderly care integrates with `CaregivingSystem`;
- care need is based on health/frailty/capability, not age alone;
- age-related mortality uses canonical health/death pipeline;
- palliative/death-quality/memorial/grief systems remain canonical;
- family relationships can represent parent/grandparent generations where current family system supports it;
- archive/history records milestone transitions and deaths;
- seasonal/birthday timing is coherent;
- old saves migrate without invented ages;
- unknown-age survivors remain playable and UI-safe if needed;
- save/load preserves chronology exactly;
- no stage transition replays on reload;
- no duplicate birthday/milestone events;
- no wall-clock or frame-time age progression;
- `--aging-selftest` exists or equivalent;
- 30/120/180/400-year-equivalent simulations prove age distribution, care load, retirement, mentorship, and mortality remain bounded;
- UI clearly distinguishes age, life stage, health, capacity, care need, and retirement state.

---

# 4. Phase P0 — Chronology, Lifecycle & Capability Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
campaign day/time authority
season/year mapping
SurvivorLifecycle fields/APIs
survivors.json age/birth fields
recruitment DTOs
birth mechanics
death/mortality APIs
Plan-24 FitnessVerdict
Plan-137 performance projection
Plan-143 affliction capability
CaregivingSystem
SkillProgressionSystem
GenerationalSuccessionEngine
Plan-150 family system
Plan-159 governance
Plan-162 archive
Plan-170 seasonal events
memorial/death-quality/grief authorities
save sections/order
```

## P0.2 Build aging authority matrix

Create:

`docs/survivors/AGING_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
aging role
status
```

Rows:
- campaign day;
- campaign year;
- birth date/day;
- age at recruitment;
- current age;
- life stage;
- fitness;
- work performance;
- combat performance;
- expedition readiness;
- health;
- care need;
- caregiver assignment;
- learning;
- mentorship;
- retirement;
- mortality;
- memorial;
- grief;
- family generation;
- archive event.

## P0.3 Time-scale ADR

Create:

`docs/architecture/ADR_CAMPAIGN_TIME_AND_AGING.md`

Answer:

```text
How many campaign days equal one in-world year?
How does this relate to seasons?
Do children age on the same scale?
Does pregnancy/family timing share the same scale?
How do archive dates display?
What happens in 120–180-day campaigns?
What happens in long simulations?
```

## P0.4 Age-storage ADR

Create:

`docs/architecture/ADR_SURVIVOR_AGE_REPRESENTATION.md`

Candidate models:

### A. Birth-day model

```text
birth_campaign_day
→ exact age
```

Best for born-in-shelter survivors.

### B. Recruitment anchor model

```text
age_at_recruitment
recruitment_day
→ current age
```

Best when authored recruits only have starting age.

### C. Hybrid

Use:
- birth anchor if known;
- recruitment anchor otherwise.

Recommended if needed.

## P0.5 Audit `GenerationalSuccessionEngine`

Determine:
- duplicate age state?
- retirement state?
- generation counters?
- `AdvanceTime()` ownership?

Disposition:
- REUSE;
- ADAPT;
- MIGRATE;
- DELETE DUPLICATE.

## P0.6 Baseline proof

Demonstrate:
- survivor age does not advance today;
- elderly-care references have no age producer;
- generational advance is unwired.

---

# TASK 176A — Canonical Age & Life-Stage Projection

# 176A.0 Goal

Create deterministic survivor chronology and derived life-stage context.

## 176A.1 Prefer extending `SurvivorLifecycle`

Only create `AgingSystem` if:
- lifecycle cannot cleanly own chronology;
- and its boundaries remain narrow.

## 176A.2 Canonical age model

Recommended read model:

```text
SurvivorAgeSnapshot
  survivor_id
  chronological_years
  age_days_remainder
  life_stage_id
  next_milestone_day
  age_known
```

## 176A.3 Persist anchor, not ticking age

Prefer:

```text
age_at_anchor
anchor_day
```

or:
- birth day.

Current age is derived.

## 176A.4 No daily `age++`

Increment only when crossing year boundary in derived chronology.

## 176A.5 Partial years

Keep enough precision to avoid drift.

## 176A.6 Days-per-year

Config/data, but globally controlled by time-scale ADR.

## 176A.7 Life stage catalog

Create:

`Assets/StreamingAssets/Data/life_stages.json`

Versioned.

## 176A.8 Life-stage DTO

Suggested:

```text
id
min_age
max_age optional
display_key
description_key
capability_profile_id optional
milestone_tags[]
```

Do not put giant universal modifier sets directly in stage DTO unless they map to real capability inputs.

## 176A.9 Source stage ranges

Treat as tuning candidates:

```text
young_adult 18–30
prime 31–45
middle_age 46–60
elderly 61+
```

Audit against game fiction/time scale.

## 176A.10 Avoid culturally arbitrary hard truth

Stage labels are gameplay abstractions.

Do not imply real-world medical certainty.

## 176A.11 Life-stage transition

Emit once when derived stage changes.

## 176A.12 Birthday event

Emit when integer age advances, if birthday events are retained.

## 176A.13 Milestone events

Data-driven.

Do not hardcode only:
- 30;
- 40;
- 50;
- 60
unless design wants them.

## 176A.14 Birthday frequency

If one in-world year is very short:
- yearly celebration may become notification spam.

Use event-budget policy.

## 176A.15 Celebration vs birthday fact

Birthday fact:
- lifecycle.

Celebration:
- optional event/action.

Do not automatically grant morale every year.

## 176A.16 Age unknown

Old/imported survivor can have:

```text
age_known = false
estimated_stage optional
```

only if safe.

## 176A.17 Unknown-age UI

Display:
- “Age unknown”
or authored approximate category.

## 176A.18 Do not fabricate exact integer

## 176A.19 Recruitment age

Authored survivor age becomes anchor.

## 176A.20 Birth

Born survivor gets exact chronology.

## 176A.21 Adult-only campaigns

If no child system actually ships:
- age still works for adults.

## 176A.22 Data integrity

Validate:
- no overlapping stage ranges;
- no gaps;
- one open-ended terminal stage.

## 176A.23 Generated matrix

Create:

`docs/survivors/LIFE_STAGE_MATRIX.md`

## 176A.24 Unit tests

- exact boundary;
- partial year;
- stage transition;
- unknown age;
- born survivor;
- recruited survivor;
- save/load.

### 176A DoD

Every survivor has a stable, deterministic chronology representation where age is monotonic and life stage is a derived, data-driven read model rather than a second stored truth.

---

# TASK 176B — Functional Capacity Composition

# 176B.0 Goal

Make age affect capability only through existing performance/fitness systems.

## 176B.1 Audit Plan 24 / 137 / 143 final state

Confirm:
- hard fitness exclusions;
- work multiplier composition;
- expedition readiness;
- combat capability;
- affliction contributions.

## 176B.2 Age capability contribution

Preferred interface:

```text
IAgeCapabilitySource
```

or extension to canonical performance projection.

## 176B.3 No direct work mutation

No:
`workSpeed *= stage.physicalModifier`
inside `AgingSystem`.

## 176B.4 No direct combat mutation

## 176B.5 No direct expedition mutation

## 176B.6 Capability dimensions

Only real dimensions.

Possible:
- strenuous work capacity;
- recovery/stamina;
- expedition endurance;
- combat physical performance;
- learning rate;
- mentorship eligibility.

## 176B.7 Chronological age vs frailty

Age contribution should be modest.

Health/afflictions/frailty carry larger direct impairment.

## 176B.8 Frailty

Before adding a stored frailty score, audit:
- health;
- afflictions;
- fitness;
- needs;
- mobility.

Default:
- derive from existing state + age contribution.

## 176B.9 No age-only duty ban

An elderly survivor is not automatically barred from:
- medical;
- workshop;
- leadership;
- teaching;
- guard
unless actual capability/fitness says so.

## 176B.10 Strenuous-duty rule

If duty taxonomy supports physical-demand tags:
- age capability contributes to fitness evaluation.

## 176B.11 Work output

Age contribution feeds final Plan-137 composition once.

## 176B.12 Expedition

Same.

## 176B.13 Combat

Same if combat dimension exists.

## 176B.14 Recovery

If health recovery system has an age factor:
- one canonical contribution.

## 176B.15 Mental capability

Default:
- no universal decline.

Only implement where:
- cognitive state authority exists;
- age contribution is justified.

## 176B.16 Learning

Age may alter learning rate only if:
- SkillProgressionSystem supports modifiers.

Do not assume older = worse learner universally.

## 176B.17 Experience

Actual accumulated skill remains primary.

## 176B.18 “Wisdom”

Prefer deriving from:
- skill;
- experience;
- leadership history;
- mentorship history.

If a `wisdom` concept exists, adapt.

## 176B.19 Decision making

No magic age-based decision bonus unless decision system has a consumer.

## 176B.20 Capability profile data

If needed:

```text
age_capability_profiles.json
```

Fields:
- stage/age curve;
- domain contribution;
- floors/ceilings.

## 176B.21 Continuous curves vs stage steps

Prefer smooth/banded curves where abrupt birthday cliffs feel artificial.

## 176B.22 Boundary smoothing

Crossing 60 should not suddenly reduce output by 40%.

## 176B.23 Multi-factor composition

Age + needs + affliction + trauma:
- compose exactly once.

## 176B.24 Floor

No ordinary age contribution reduces work near zero while survivor remains fit.

## 176B.25 Reason trace

UI can say:
- “Reduced endurance”
not:
- “Old: -35%”.

## 176B.26 Unit tests

- young healthy;
- older healthy;
- older afflicted;
- middle-aged highly skilled;
- stage boundary;
- no double-count.

### 176B DoD

Age contributes bounded context to existing fitness/performance calculations without becoming a parallel capability or health system.

---

# TASK 176C — Retirement, Reduced Duty & Autonomy

# 176C.0 Goal

Make retirement a survivor/work-governance state with meaningful choices rather than an automatic age penalty.

## 176C.1 Retirement state

Suggested:

```text
active_worker
reduced_duty
retired
```

Only if current duty/availability model needs explicit status.

## 176C.2 Retirement owner

Audit:
- survivor lifecycle;
- duty roster;
- autonomy;
- governance.

Choose one canonical owner.

## 176C.3 Retirement eligibility

Candidate inputs:
- age/stage;
- health;
- work history;
- survivor preference;
- governance policy.

Do not use age alone.

## 176C.4 Source default age 60

Treat as data/tuning candidate.

## 176C.5 Optional retirement

Player/survivor/governance choice according to autonomy model.

## 176C.6 Survivor preference

If autonomy exists:
- survivor may request retirement/reduced duty.

## 176C.7 No forced player control contradiction

Respect Plan 144 autonomy.

## 176C.8 Reduced duty

Important intermediate option.

Examples:
- fewer shifts;
- lighter duties;
- mentorship.

## 176C.9 Retired survivor remains resident

Consumes:
- normal real resources according to needs.

No special abstract retirement consumption.

## 176C.10 No retirement resource modifier

Their consumption comes from:
- needs;
- health;
- food.

## 176C.11 Retired work

May still:
- teach;
- advise;
- craft lightly;
- garden;
where actual duty capacity permits.

## 176C.12 No blanket “cannot work”

## 176C.13 Re-entry

Retired survivor may temporarily resume work if:
- autonomy/governance allows;
- fitness allows.

## 176C.14 Retirement event

Semantic.

## 176C.15 Ceremony

Optional event/action.

No automatic morale bonus by default.

## 176C.16 Governance

Plan 159 may support:
- retirement policy;
- elder-care entitlement;
- work exemptions.

Governance configures eligibility/rules;
does not own survivor health.

## 176C.17 Anti-exploit

Player cannot retire/unretire repeatedly to:
- farm morale;
- reset duty penalties;
- trigger events.

## 176C.18 Save/load

State persists once.

## 176C.19 Death while retired

Normal lifecycle.

## 176C.20 UI

Show:
- work status;
- why;
- eligible lighter roles.

### 176C DoD

Retirement becomes a legible, reversible where appropriate, autonomy-aware work status that preserves survivor usefulness without forcing age stereotypes.

---

# TASK 176D — Elder Care & Caregiving Integration

# 176D.0 Goal

Make care need emerge from real health/capability state and route actual care through `CaregivingSystem`.

## 176D.1 Care need projection

Suggested dimensions:
- mobility assistance;
- medication support;
- supervision;
- comfort.

Only if CaregivingSystem supports equivalent categories.

## 176D.2 Age is one risk factor

Not a binary threshold.

## 176D.3 Health/affliction importance

Use:
- chronic affliction;
- injury;
- frailty/capability;
- palliative state.

## 176D.4 No universal elderly care requirement

Healthy older survivor may need none.

## 176D.5 Younger disabled survivor

May need care too.

Do not make “elderly care” architecture age-exclusive.

## 176D.6 Prefer generic care-needs model

If current CaregivingSystem already supports:
- childcare;
- elderly;
- comfort,
consider expanding to:
- `CareRecipientNeeds`.

## 176D.7 Caregiver assignment

Existing system.

## 176D.8 Care labor

Real time/duty.

## 176D.9 Care quality

Existing caregiver capability/relationship if supported.

## 176D.10 Care effects

Route through:
- health;
- comfort/morale;
- adherence
as existing systems support.

## 176D.11 No direct health regeneration from aging

## 176D.12 Lack of care

Can worsen:
- unmet need;
- stress;
- health risk
through canonical systems.

No arbitrary old-age death.

## 176D.13 Care burden

Track shelter care-hours.

## 176D.14 Prioritization

Governance/medical/duty systems may influence.

## 176D.15 Care relationship

Plan 147/relations can record meaningful caregiver bonds if already supported.

## 176D.16 Palliative overlap

Plan 60 palliative care remains authority for end-of-life care.

## 176D.17 No duplicate comfort-care system

## 176D.18 Save/load

Care assignments owned by CaregivingSystem.

Age system stores none.

## 176D.19 Tests

- healthy elder no care;
- frail elder care;
- younger disabled care;
- caregiver unavailable;
- palliative overlap.

### 176D DoD

Age can contribute to care need, but caregiving remains a generic capability/health-driven system rather than an age-triggered penalty engine.

---

# TASK 176E — Mentorship, Skill Transfer & Inter-Generational Relationships

# 176E.0 Goal

Turn accumulated experience into a useful late-life role through the existing skill and relationship authorities.

## 176E.1 Mentorship owner

Prefer:
- SkillProgressionSystem;
- existing education/training system from Plan 154.

Aging only supplies:
- eligibility/context.

## 176E.2 Mentor eligibility

Use:
- skill threshold;
- experience;
- willingness;
- availability.

Age alone is insufficient.

## 176E.3 Younger mentor allowed

A highly skilled younger survivor may mentor.

Do not hardcode mentorship exclusively to elders.

## 176E.4 Elder advantage

Age/history may increase:
- mentorship efficiency
only through data-backed capability contribution.

## 176E.5 Apprentice eligibility

Real survivor.

## 176E.6 Pair assignment

Use existing training/education/duty pairing.

## 176E.7 No passive mentorship aura

Learning accelerates only when:
- actual mentorship/training activity occurs.

## 176E.8 Knowledge transfer

Canonical skill XP/proficiency.

## 176E.9 No “wisdom transfer” stat unless it exists

## 176E.10 Skill decay

If current system has decay:
- mentorship may reduce it.

If not:
- do not invent decay just to make mentorship useful.

## 176E.11 Mentorship history

Record:
- mentor;
- apprentice;
- skill;
- milestone.

## 176E.12 Relationship effect

May strengthen bond through `SurvivorRelationsSystem`.

## 176E.13 No guaranteed relationship gain every tick

Use semantic milestones.

## 176E.14 Mentor death

Apprentice grief uses:
- existing relation/grief authority.

## 176E.15 Knowledge legacy

If archive/legacy supports:
- record mastered teaching lineage.

No separate legacy stat.

## 176E.16 Multiple apprentices

Bound by:
- mentor time;
- training capacity.

## 176E.17 Multiple mentors

Possible but no XP stacking exploit.

## 176E.18 Retirement synergy

Retired/reduced-duty survivor can spend time teaching.

## 176E.19 Tests

- skilled elder mentor;
- unskilled elder not mentor;
- skilled younger mentor;
- apprentice XP;
- reload;
- mentor death.

### 176E DoD

Late-life usefulness comes from real accumulated skill and deliberate mentorship activity, not a generic “wisdom aura.”

---

# TASK 176F — Mortality, Milestones, Family, Archive & Seasonal Continuity

# 176F.0 Goal

Make aging visible across the whole life course while preserving canonical death, grief, family, and history systems.

---

# 176F-M — Mortality

## 176F.M1 Mortality authority audit

Find actual:
- death condition;
- health depletion;
- disease mortality;
- palliative outcome.

## 176F.M2 Age-related mortality contribution

Only through existing mortality-risk pipeline.

## 176F.M3 No direct age roll

Forbidden:

```text
if age >= 70:
    rng death
```

## 176F.M4 Baseline mortality curve

If mortality authority supports risk:
- age contributes hazard/risk band.

## 176F.M5 Health interaction

Healthy older survivor:
- lower risk than severely ill peer.

## 176F.M6 Care interaction

Care may reduce risk indirectly through health/needs.

## 176F.M7 No immortality guarantee

Age can increase baseline vulnerability.

## 176F.M8 Natural causes classification

If death-cause taxonomy supports:
- `natural_causes`;
- `age_related_decline`.

Do not create duplicate death reason if generic health failure suffices.

## 176F.M9 Peaceful death

Death quality/palliative authority decides.

Age alone does not guarantee peaceful death.

## 176F.M10 Memorial

Existing memorial system.

## 176F.M11 Grief

Existing relations/morale system.

## 176F.M12 Exactly once

Source death event identity.

---

# 176F-B — Birthdays & Milestones

## 176F.B1 Birthday fact

Lifecycle event.

## 176F.B2 Celebration

Optional event.

## 176F.B3 Seasonal integration

If Plan 170 owns calendar observances:
- birthday can schedule through calendar/event layer.

## 176F.B4 No forced celebration every year

Resource scarcity may cause:
- quiet recognition;
- no celebration.

## 176F.B5 Milestones

Authored:
- 30;
- 40;
- 50;
- 60
only if meaningful.

## 176F.B6 Milestone effects

Default:
- journal/history only.

Do not automatically award stat bonuses.

---

# 176F-F — Family / Generations

## 176F.F1 Plan 150 reuse

Family authority owns:
- parent;
- child;
- partner;
- kinship.

## 176F.F2 Age validation

Relationship timing should remain plausible if birth chronology is known.

## 176F.F3 Grandparent

Derived kinship.

## 176F.F4 Three generations

Quest/achievement hook only if actual family relations support.

## 176F.F5 No synthetic “generation number” if family graph can derive it

## 176F.F6 Born survivors

Age normally.

## 176F.F7 Coming-of-age

Only if child/adolescent systems exist.

Otherwise defer.

---

# 176F-G — Generational Succession Engine

## 176F.G1 Reconcile `AdvanceTime()`

Do not call a second clock.

Adapt engine to:
- subscribe to canonical age/year transition events.

## 176F.G2 Retirement skeleton

Migrate to canonical retirement state.

## 176F.G3 Legacy

Succession reads:
- death;
- mentorship;
- archive;
- leadership.

No duplicate age processing.

---

# 176F-A — Archive

## 176F.A1 Plan 162 archive

Record significant:
- life-stage milestone;
- retirement;
- mentorship milestone;
- natural death;
- multi-generation event.

## 176F.A2 Do not log every age query

## 176F.A3 Name/identity continuity

Use survivor stable IDs.

## 176F.A4 Age at event

Archive may store:
- age-at-event snapshot for historical rendering.

## 176F.A5 Epilogue

Can mention:
- years lived;
- mentor legacy;
- retirement;
- family generations.

### 176F DoD

Aging culminates in coherent lifecycle, family, memorial, and archive history without bypassing health, mortality, grief, or succession authorities.

---

# TASK 176G — UI, Persistence, Determinism, Balance & CI

# 176G.0 Goal

Make age understandable, save-safe, fair, deterministic, and compatible with both ordinary and long-horizon campaigns.

---

# 176G-U — UI

## 176G.U1 Survivor detail

Show separately:

```text
Age
Life stage
Health
Functional capacity
Care need
Work status
Mentorship
```

Do not collapse them into one “elderly” panel.

## 176G.U2 Age tooltip

Explain:
- chronological age;
- stage;
- next milestone.

## 176G.U3 Capability tooltip

Explain actual modifiers/reasons:
- age contribution;
- afflictions;
- needs;
- retirement.

## 176G.U4 Retirement management

Prefer existing roster/duty surface.

New retirement panel only if necessary.

## 176G.U5 Caregiving UI

Reuse existing caregiving panel.

## 176G.U6 Mentorship UI

Reuse training/education UI.

## 176G.U7 Age list

Optional roster sort/filter.

No separate screen required.

## 176G.U8 Unknown age

Readable.

## 176G.U9 Birthday/milestone notification

Bounded.

## 176G.U10 Death

Normal memorial/death UI.

## 176G.U11 Accessibility

No color-only stage/capacity.

## 176G.U12 Text scale

Age/care tooltips wrap.

## 176G.U13 Language

Avoid demeaning age labels.

Use neutral:
- older adult;
- elder
only where fiction supports.

---

# 176G-P — Persistence

## 176G.P1 Persist anchors, not duplicates

Potential:

```text
age_at_anchor
anchor_day
birth_day if known
age_known
retirement_state if aging owns it
processed_milestone_ids
```

## 176G.P2 Do not persist derived life stage

## 176G.P3 Do not persist final work/combat modifiers

## 176G.P4 Do not persist caregiving assignment

If CaregivingSystem owns it.

## 176G.P5 Do not persist skill effects

## 176G.P6 Old-save migration classes

### Known authored age
Use it.

### Recruitment-age field exists
Anchor it at migration/load epoch according to save chronology.

### No age evidence
Use:
- unknown/legacy state;
or a documented deterministic fallback.

## 176G.P7 No visual/name-based age guessing

## 176G.P8 No random age assignment on migration

## 176G.P9 Stage events

Processed milestone IDs prevent re-emission.

## 176G.P10 Birthday event identity

Stable:
`survivor_id + age`.

## 176G.P11 Save/load

Same exact age/remainder.

## 176G.P12 Time-scale migration

If days-per-year changes between versions:
- do not silently reinterpret old ages.

Persist:
- chronology schema/time-scale version
as needed.

---

# 176G-D — Determinism

## 176G.D1 Chronological age

Pure from anchor + campaign day.

## 176G.D2 Stage

Pure.

## 176G.D3 Retirement eligibility

Pure unless survivor choice uses seeded autonomy.

## 176G.D4 Care need

Pure from canonical health/capability state.

## 176G.D5 Mentorship XP

Canonical deterministic/seeded system.

## 176G.D6 Mortality

If stochastic:
- canonical seeded mortality authority.

## 176G.D7 No wall clock

## 176G.D8 Stable iteration

Sort survivor IDs.

---

# 176G-B — Balance

## 176G.B1 Age should create tradeoff, not punishment

Older survivors may have:
- lower strenuous capacity;
- greater accumulated skill;
- mentorship;
- leadership continuity;
- social/family value.

## 176G.B2 No blanket productive-age optimum

Avoid making everyone >60 strictly worse.

## 176G.B3 Skill can offset physical decline in suitable roles

## 176G.B4 Role diversity

Older survivors should remain strong candidates for:
- teaching;
- medicine;
- negotiation;
- administration;
- crafting
if their skills support.

## 176G.B5 Care burden

Should be meaningful but not automatic by age.

## 176G.B6 Retirement value

Creates:
- mentoring;
- reduced overload;
- recovery/leisure
if those rails exist.

## 176G.B7 Workforce cliff

No mass sudden retirement at same threshold.

## 176G.B8 Death curve

No deterministic wipe at age 70–80.

## 176G.B9 Birthday spam

Bound.

## 176G.B10 Mentorship exploit

No infinite XP from idle retirees.

## 176G.B11 Retirement exploit

No retire/unretire event farming.

## 176G.B12 Care exploit

No arbitrary health farming through care if no care need.

---

# 176G-S — Long-Horizon Simulation

## 176G.S1 30-day ordinary campaign

Verify:
- partial aging;
- no excessive events.

## 176G.S2 120-day campaign

Track:
- age transitions;
- retirements;
- care needs;
- mentorship.

## 176G.S3 180-day campaign

Track:
- workforce age mix;
- productivity;
- mortality;
- elder value.

## 176G.S4 400-day long campaign

Track:
- multi-generation possibility;
- event volume;
- archive volume;
- state size.

## 176G.S5 Young shelter

No elderly:
- no errors;
- no empty-panel noise.

## 176G.S6 Older shelter

Many elders:
- care system bounded;
- workforce still playable.

## 176G.S7 High-skill elders

Verify mentorship/low-strain usefulness.

## 176G.S8 Poor-health middle-aged survivor

Capability lower than healthy elder when canonical health warrants.

This is a key anti-stereotype test.

## 176G.S9 Retirement-heavy shelter

No unavoidable collapse.

## 176G.S10 Family three-generation scenario

Only if family/birth systems support.

---

# 176G-T — Testing & CI

## 176G.T1 Data integrity

Validate:
- life stage IDs;
- ranges;
- capability profiles;
- retirement rules;
- localization.

## 176G.T2 Selftest

Create:

```text
--aging-selftest
```

## 176G.T3 Selftest scenarios

At least:
1. known-age recruit;
2. born survivor;
3. unknown-age old save;
4. partial-year progression;
5. birthday;
6. life-stage transition;
7. age-performance contribution;
8. retirement eligibility;
9. retire/reduced-duty;
10. care need;
11. mentorship;
12. natural mortality adapter;
13. memorial/grief idempotence;
14. save/load;
15. headless.

## 176G.T4 Source-scan authority gate

Detect:
- second campaign clock;
- duplicate final work modifier;
- direct combat mutation;
- direct caregiver assignment in aging;
- direct health regen;
- direct death RNG;
- guessed migration age.

## 176G.T5 Content acceptance

Life-stage/capability data:
- LOADED;
- QUERIED;
- EFFECT_PRODUCED where applicable.

## 176G.T6 Reachability

Every life stage:
- reachable in a deterministic fixture.

Retirement:
- reachable.

Care:
- reachable through real need.

Mentorship:
- reachable.

## 176G.T7 Deterministic fingerprint

Fixed survivor chronology fixture:
- same age/stage/events.

## 176G.T8 Performance

Age query:
- O(1).

Day transition:
- O(active survivors).

No per-frame work.

## 176G.T9 Generated docs

Create:
- `AGING_ARCHITECTURE.md`;
- `AGING_AUTHORITY_MATRIX.md`;
- `LIFE_STAGE_MATRIX.md`;
- `AGE_CAPABILITY_MATRIX.md`;
- `RETIREMENT_CARE_MENTORSHIP_MATRIX.md`;
- `AGING_MIGRATION_MATRIX.md`;
- `AGING_BALANCE_REPORT.md`;
- `ADR_CAMPAIGN_TIME_AND_AGING.md`;
- `ADR_SURVIVOR_AGE_REPRESENTATION.md`.

### 176G DoD

Age becomes a stable part of survivor identity and long-term planning while gameplay capacity, care, learning, mortality, and emotional consequences continue to be decided by their proper systems.

---

# TASK 176H — Advanced Age-Specific Traits, Ceremonies & Demographics: Explicit Follow-On

# 176H.0 Goal

Keep the foundational aging model clean before adding demographic strategy layers.

## 176H.1 Age-specific traits

Default:
- DEFER.

A trait should represent:
- personality/history/capability,
not simply duplicate age.

## 176H.2 “Youth energy”

Avoid as a generic trait if age performance already models endurance.

## 176H.3 “Elder wisdom”

Avoid if skill/history/mentorship already model experience.

## 176H.4 Coming-of-age ceremony

Only if child/adolescent life stages are real.

## 176H.5 Retirement parties

Narrative event follow-on.

## 176H.6 Elder council

Use Plan 159 governance if:
- age-based representation is explicitly chosen.

No separate council.

## 176H.7 Age demographics screen

Could be:
- roster histogram/read model.

No new simulation state.

## 176H.8 Dependency ratio

Derived metric:
- working capacity vs care dependents.

Potential UI/analytics follow-on.

## 176H.9 Pensions

No currency pension system unless economy/governance design requires.

## 176H.10 Elder housing

Only if room/housing system has accessibility/support needs.

## 176H.11 Funeral traditions

Use memorial/culture systems.

## 176H.12 Legacy/famous elder

Plan 162/archive/epilogue derives it.

### 176H DoD

Advanced demographics remain derived/read-model or explicit follow-on features rather than polluting the base survivor chronology contract.

---

# 5. Chronology State Model

```text
SURVIVOR ENTERS WORLD
      │
      ├── born in campaign
      │      └── exact birth anchor
      │
      └── recruited
             └── authored/known age anchor
                    │
                    ▼
             CAMPAIGN TIME ADVANCES
                    │
                    ▼
             CURRENT AGE DERIVED
                    │
                    ├── birthday
                    ├── life-stage transition
                    ├── retirement eligibility
                    └── mortality/capability contribution
```

No second ticking clock.

---

# 6. Age Representation Contract

Preferred:

```text
Age = anchor_age + elapsed_campaign_time / days_per_year
```

or exact birth anchor.

The stored fact should be the minimum needed to reconstruct age deterministically.

---

# 7. Life Stage Contract

Life stage is for:
- presentation;
- coarse rule grouping;
- milestone selection.

It is not:
- a complete physical/mental profile.

---

# 8. Functional Capacity Contract

Actual capability should reflect:

```text
base survivor
× skill
× needs
× affliction
× age contribution
× trauma/social
→ canonical final performance
```

Exact composition follows Plan 137 authority.

---

# 9. Age vs Health Contract

Chronological age:
- one input.

Health:
- separate authority.

An older healthy survivor can outperform a younger sick/injured one.

---

# 10. Age vs Skill Contract

Age does not automatically equal skill.

Accumulated experience:
- SkillProgressionSystem.

Age may make long experience possible, but does not grant knowledge from nowhere.

---

# 11. Wisdom Contract

Default definition:

```text
wisdom = useful accumulated knowledge/history expressed through existing skills, leadership and mentorship
```

Not a free hidden age stat.

---

# 12. Retirement Contract

Retirement means:
- work-status choice;
- lower expected labor burden;
- possible mentorship/care/social role.

It does not mean:
- removal from survivor roster.

---

# 13. Reduced Duty Contract

This is critical for graceful aging:

```text
full duty
→ reduced duty
→ retired
```

with possible movement back when circumstances change.

---

# 14. Care Need Contract

Care need answers:

```text
What assistance does this survivor currently require?
```

not:

```text
Are they older than 60?
```

---

# 15. Mentorship Contract

Mentorship is:
- intentional skill-transfer activity;
- requires expert mentor + apprentice + time.

No passive aura.

---

# 16. Birthday Contract

A birthday is a chronological event.

A celebration is:
- player/community action;
- optional.

Do not bind a guaranteed morale effect to chronology.

---

# 17. Milestone Contract

Milestones are:
- authored historical/event hooks.

They do not automatically modify stats.

---

# 18. Mortality Contract

Age may increase baseline risk.

Canonical health/mortality pipeline still decides:
- actual death;
- cause;
- quality.

---

# 19. Palliative Contract

Plan 60 remains authority for:
- comfort;
- vigil;
- final wishes;
- death quality.

Aging only contributes context.

---

# 20. Memorial Contract

Memorial system owns:
- memorial outcome;
- grief.

Aging provides:
- age at death;
- retirement/mentor history.

---

# 21. Family Contract

Family graph owns:
- parent;
- child;
- partner;
- grandparent derivation.

Aging validates chronology.

---

# 22. Governance Contract

Plan 159 may define:
- retirement rights;
- elder-care policy;
- age-based exemptions;
- representation.

Governance does not mutate age.

---

# 23. Archive Contract

Plan 162 records:
- birthdays selectively;
- stage milestones;
- retirement;
- mentorship;
- death.

Aging does not create second archive.

---

# 24. Seasonal Calendar Contract

If birthdays use calendar dates:
- Plan 170/calendar authority owns calendar mapping.

If only campaign-year transitions exist:
- use those.

No duplicate date system.

---

# 25. Old-Save Migration Contract

Migration priority:

```text
1. exact birth chronology
2. authored current age
3. recruitment-age anchor
4. deterministic documented legacy fallback
5. unknown age
```

Never infer from:
- portrait;
- name;
- role;
- skill.

---

# 26. Persistence Matrix

| Fact | Owner |
|---|---|
| campaign time | time/calendar |
| birth anchor | survivor lifecycle |
| recruitment age anchor | survivor lifecycle |
| current age | derived |
| life stage | derived |
| fitness | fitness |
| performance | Plan 137 composition |
| affliction | medical |
| care need | caregiving/derived capability |
| caregiver assignment | CaregivingSystem |
| retirement | survivor/duty/autonomy canonical owner |
| skill | SkillProgression |
| mentorship relation/activity | skill/training |
| death | mortality/lifecycle |
| grief | relations/morale |
| memorial | memorial |
| family | family |
| archive event | archive |

---

# 27. Time-Scale Compatibility Matrix

Audit:

```text
seasons per year
days per season
days per year
pregnancy duration
child development
education duration
retirement horizon
campaign length
archive date display
```

All must tell the same fictional clock story.

---

# 28. Exactly-Once Milestone Identity

Stable IDs:

```text
birthday:<survivor_id>:<age>
stage:<survivor_id>:<stage_id>
retirement:<survivor_id>:<retirement_sequence>
```

No duplicate journal/event on reload.

---

# 29. Failure Injection Matrix

## N176.1 Survivor age stored separately in `AgingState` and lifecycle
Expected: authority gate fails.

## N176.2 `GenerationalSuccessionEngine.AdvanceTime()` runs a second age clock
Expected: chronology gate fails.

## N176.3 Crossing age 61 directly multiplies all work by 0.5
Expected: capability-authority/boundary test fails.

## N176.4 Healthy elder automatically requires caregiving
Expected: care-need semantics test fails.

## N176.5 Younger disabled survivor cannot receive same care type
Expected: generic-care model test fails.

## N176.6 Retirement silently removes survivor from all duties
Expected: retirement/autonomy integration fails.

## N176.7 Elder passively grants XP to all younger survivors
Expected: mentorship-authority gate fails.

## N176.8 Age 70 rolls death directly inside AgingSystem
Expected: mortality-authority gate fails.

## N176.9 Reload repeats 60th birthday / retirement ceremony
Expected: milestone idempotence fails.

## N176.10 Old save assigns random age
Expected: migration determinism fails.

## N176.11 Changing days-per-year reinterprets old survivor age silently
Expected: chronology-version gate fails.

## N176.12 Birthday every 30 days floods journal
Expected: event-budget test fails.

---

# 30. Determinism Contract

Same:

```text
campaign chronology
+ survivor age anchor
+ life-stage data
+ capability profile data
+ survivor health/needs/skills
+ retirement/care/mentorship state
+ canonical mortality seed/state
```

must produce same:
- age;
- stage;
- milestone transitions;
- capability contribution;
- retirement eligibility;
- care need;
- mentorship eligibility;
- mortality contribution.

---

# 31. Long-Horizon Metrics

Track:

```text
age distribution
stage distribution
birthday events
stage transitions
retirement requests
retired survivors
reduced-duty survivors
care recipients
care hours
mentor/apprentice pairs
skill XP transferred
natural deaths
age at death
oldest survivor
work contribution by age band
health contribution by age band
archive event volume
state bytes
```

---

# 32. Balance Guardrails

The design succeeds when:

```text
time creates meaningful demographic change
older survivors remain valuable
care has cost but not stereotype
retirement has purpose
mentorship matters
health remains more important than raw age for capability
```

It fails when:

```text
aging is simply a death clock
all survivors become useless at 60
birthday spam dominates UI
retirement is a punishment
elder care is automatic tax
```

---

# 33. Aging Rate Guardrails

Do not ship `30 days = 1 year` until simulation shows it fits:
- campaign length;
- seasons;
- generational goals.

Possible profiles:
- compressed campaign chronology;
- long-campaign chronology.

One canonical profile per campaign mode.

---

# 34. Life-Stage Boundary Guardrails

Prefer smooth capability curves.

If stage-based:
- small deltas;
- no cliffs.

Stage transition is primarily narrative/readability.

---

# 35. Mortality Balance Guardrails

Age-related mortality should:
- increase gradually;
- interact with health;
- remain seeded/deterministic;
- not guarantee death at a fixed threshold.

---

# 36. Retirement Balance Guardrails

Retirement should create strategic choice:

```text
lose some labor
gain availability for mentorship/recovery/light duty
reduce overload
preserve skilled survivor longer
```

where supported.

---

# 37. Caregiving Balance Guardrails

Many elders:
- increase care pressure only if health/capability warrants.

No flat “one caregiver per elder.”

---

# 38. Mentorship Balance Guardrails

Mentorship:
- costs time;
- cannot exceed training capacity;
- cannot infinitely stack;
- should preserve institutional knowledge across generations.

---

# 39. UI Acceptance

## Survivor detail
Age is visible but not presented as destiny.

## Duty
Shows actual capacity reasons.

## Retirement
Shows options and consequences.

## Care
Shows real need category.

## Mentorship
Shows skill and apprentice.

## Memorial/archive
Shows age/life history.

---

# 40. Accessibility

- no age-stage color-only encoding;
- no tiny demographic charts as sole explanation;
- keyboard sorting/filtering;
- text scale;
- neutral language.

---

# 41. Localization

Life-stage labels:
- localization keys.

Player-facing wording avoids literal real-world medical claims.

---

# 42. Content Acceptance

Life-stage/capability data:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
EFFECT_PRODUCED
```

Milestone/event definitions:
- reachable.

---

# 43. Reachability

Every life stage:
- deterministic synthetic survivor fixture.

Retirement:
- eligible fixture.

Care:
- need fixture.

Mentorship:
- skilled mentor + apprentice fixture.

Natural mortality:
- canonical high-risk fixture.

---

# 44. Performance Guardrails

Age query:
- O(1).

Day boundary:
- O(active survivors).

Milestones:
- check upcoming boundary, not every historical year.

No per-frame age tick.

---

# 45. CI / Gate Set

Recommended:

```text
aging_chronology_single
aging_age_authority_single
aging_life_stage_integrity
aging_no_duplicate_performance
aging_care_authority
aging_mentorship_authority
aging_mortality_authority
aging_milestone_idempotence
aging_old_save_migration
aging_time_scale_compatibility
aging_determinism
aging_long_horizon
aging_ui_access
```

---

# 46. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --aging-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 47. Recommended Commit Breakdown

```text
176A-1 chronology/lifecycle/succession audit
176A-2 campaign-time-and-aging ADR
176A-3 age-representation ADR
176A-4 survivor chronology anchor
176A-5 life-stage catalog/projection
176A-6 birthdays/milestones/idempotence
176A-7 old-save migration
176A-8 docs/selftests

176B-1 Plan-24/137/143 overlap audit
176B-2 age capability source
176B-3 strenuous-work contribution
176B-4 expedition contribution
176B-5 combat contribution if supported
176B-6 learning/recovery capability audit
176B-7 no-double-count tests
176B-8 capability matrix/docs

176C-1 retirement authority decision
176C-2 eligibility/request flow
176C-3 reduced-duty state
176C-4 retire/unretire behavior
176C-5 autonomy/governance integration
176C-6 retirement UI
176C-7 anti-event-farm tests
176C-8 retirement docs

176D-1 generic care-needs audit
176D-2 age/health care-need projection
176D-3 CaregivingSystem adapter
176D-4 palliative overlap guard
176D-5 caregiver relation events
176D-6 care-load metrics
176D-7 care tests
176D-8 docs

176E-1 skill/education mentorship audit
176E-2 mentor eligibility
176E-3 pair assignment
176E-4 skill transfer
176E-5 relationship/history milestones
176E-6 retirement mentorship
176E-7 anti-XP-stacking tests
176E-8 docs

176F-1 mortality/death pipeline audit
176F-2 age mortality contribution
176F-3 death-quality/palliative integration
176F-4 memorial/grief idempotence
176F-5 family chronology validation
176F-6 succession-engine migration
176F-7 archive/seasonal integration
176F-8 lifecycle tests

176G-1 survivor-detail/roster UI
176G-2 unknown-age/accessibility
176G-3 persistence/time-scale versioning
176G-4 deterministic fingerprints
176G-5 30/120-day balance
176G-6 180/400-day demographic soaks
176G-7 CI/failure fixtures/perf
176G-8 final ship/no-ship report

176H-1 advanced demographic/ceremony disposition
```

---

# 48. Risk Register

## R176.1 Aging duplicates survivor lifecycle

Mitigation:
- age authority ADR;
- persist anchors once.

## R176.2 Life stages become stereotypes

Mitigation:
- stage primarily descriptive;
- capability modest and composed with health/skill.

## R176.3 Age penalties double Plan 137/143

Mitigation:
- one performance-source contribution;
- no direct domain writes.

## R176.4 Elder care becomes automatic age tax

Mitigation:
- care need based on real capability/health.

## R176.5 “Wisdom” creates fake universal stat

Mitigation:
- skills/history/mentorship first.

## R176.6 Old-age death bypasses medicine/palliative

Mitigation:
- canonical mortality adapter only.

## R176.7 30-day years distort campaign fiction

Mitigation:
- mandatory time-scale ADR/simulation.

## R176.8 Old saves get nonsense ages

Mitigation:
- evidence hierarchy;
- unknown state permitted.

## R176.9 Birthday/milestone event spam

Mitigation:
- event budget;
- optional celebrations.

## R176.10 Retirement destroys workforce

Mitigation:
- reduced-duty;
- optional retirement;
- long-horizon balance.

---

# 49. Acceptance Checklist

## P0

- [ ] campaign chronology authority audited
- [ ] season/year mapping audited
- [ ] SurvivorLifecycle audited
- [ ] survivors.json age/birth fields audited
- [ ] recruitment fields audited
- [ ] birth mechanics audited
- [ ] death/mortality audited
- [ ] Plan 24 fitness audited
- [ ] Plan 137 performance audited
- [ ] Plan 143 affliction capability audited
- [ ] CaregivingSystem audited
- [ ] SkillProgressionSystem audited
- [ ] GenerationalSuccessionEngine audited
- [ ] Plan 150 family audited
- [ ] Plan 159 governance audited
- [ ] Plan 162 archive audited
- [ ] Plan 170 season/calendar audited
- [ ] palliative/memorial/grief audited
- [ ] save ordering audited
- [ ] aging authority matrix published
- [ ] campaign-time ADR published
- [ ] age representation ADR published
- [ ] baseline ageless behavior reproduced

## 176A

- [ ] lifecycle extended before new duplicate system
- [ ] canonical age snapshot
- [ ] anchor-based persistence
- [ ] no daily mutable age truth
- [ ] partial-year precision
- [ ] one days-per-year authority
- [ ] versioned life-stage catalog
- [ ] stage DTO minimal
- [ ] source ranges treated as tuning
- [ ] stage is gameplay abstraction
- [ ] exactly-once stage transition
- [ ] birthday transition
- [ ] milestone data-driven
- [ ] birthday event budget
- [ ] celebration separate from birthday
- [ ] unknown age supported
- [ ] no fabricated exact age
- [ ] recruit anchor
- [ ] born-survivor anchor
- [ ] adult-only safe
- [ ] stage ranges no gap/overlap
- [ ] generated stage matrix
- [ ] boundary tests
- [ ] save/load tests

## 176B

- [ ] Plan 24/137/143 final composition verified
- [ ] age capability is one contribution
- [ ] no direct work mutation
- [ ] no direct combat mutation
- [ ] no direct expedition mutation
- [ ] only real capability dimensions
- [ ] health more important than raw age where appropriate
- [ ] frailty duplication audit
- [ ] no age-only duty ban
- [ ] strenuous-duty tags real
- [ ] work contribution once
- [ ] expedition contribution once
- [ ] combat contribution once
- [ ] recovery contribution only if real
- [ ] no universal mental decline
- [ ] learning modifier only if real
- [ ] accumulated skill remains primary
- [ ] no generic wisdom stat
- [ ] no magic decision bonus
- [ ] age capability profiles data-driven
- [ ] smooth/bounded transitions
- [ ] no age-threshold cliff
- [ ] age/needs/affliction/trauma compose once
- [ ] floor
- [ ] reason trace
- [ ] combination tests

## 176C

- [ ] retirement owner selected
- [ ] active/reduced/retired semantics
- [ ] eligibility uses more than age
- [ ] source age 60 treated as tuning
- [ ] retirement optional/autonomy-aware
- [ ] survivor can request retirement if autonomy supports
- [ ] reduced duty
- [ ] retired survivor stays resident
- [ ] no abstract consumption modifier
- [ ] light roles allowed by capability
- [ ] no blanket work ban
- [ ] re-entry policy
- [ ] retirement semantic event
- [ ] ceremony optional
- [ ] governance policy integration if real
- [ ] retire/unretire anti-farm
- [ ] persistence
- [ ] death while retired normal
- [ ] UI shows roles/reasons

## 176D

- [ ] care-need projection
- [ ] only supported need dimensions
- [ ] age one factor only
- [ ] health/affliction integrated
- [ ] no universal elder care requirement
- [ ] younger disabled survivors supported
- [ ] generic care model preferred
- [ ] caregiver assignment remains CaregivingSystem
- [ ] real care labor
- [ ] care quality uses existing factors
- [ ] health/morale effects canonical
- [ ] no direct health regeneration
- [ ] unmet-care consequence canonical
- [ ] care burden metrics
- [ ] governance/medical prioritization only via adapters
- [ ] relationship hooks if real
- [ ] Plan 60 palliative remains authority
- [ ] no duplicate comfort system
- [ ] care assignment not duplicated in aging
- [ ] care tests

## 176E

- [ ] mentorship owner is skill/education
- [ ] age only supplies eligibility/context
- [ ] mentor requires actual skill
- [ ] younger skilled mentors allowed
- [ ] bounded elder advantage if any
- [ ] real apprentice
- [ ] existing pair assignment
- [ ] no passive mentorship aura
- [ ] canonical skill XP
- [ ] no fake wisdom transfer
- [ ] skill decay not invented
- [ ] mentorship history
- [ ] relationship semantic milestones
- [ ] no relationship gain every tick
- [ ] mentor-death grief canonical
- [ ] archive/legacy reuse
- [ ] apprentice count bounded
- [ ] multiple-mentor stacking prevented
- [ ] retirement synergy
- [ ] mentorship tests

## 176F — Mortality

- [ ] canonical mortality authority identified
- [ ] age contribution routed there
- [ ] no direct age death roll
- [ ] mortality curve if supported
- [ ] health interaction
- [ ] care indirect interaction
- [ ] no fixed death threshold
- [ ] death cause taxonomy reused
- [ ] peaceful death owned by death-quality/palliative
- [ ] memorial canonical
- [ ] grief canonical
- [ ] exactly-once death consequence

## 176F — Birthday/Family/Succession/Archive

- [ ] birthday fact separate from celebration
- [ ] calendar/season authority reused
- [ ] no mandatory yearly celebration
- [ ] milestones authored
- [ ] milestone effects not stat bonuses by default
- [ ] Plan 150 family reused
- [ ] chronology validates family relationships
- [ ] grandparents derived
- [ ] three-generation hook only if real
- [ ] no unnecessary generation-number state
- [ ] born survivors age
- [ ] coming-of-age only if child system exists
- [ ] `GenerationalSuccessionEngine.AdvanceTime()` reconciled
- [ ] no second clock
- [ ] retirement skeleton migrated
- [ ] legacy reads canonical events
- [ ] Plan 162 archive used
- [ ] archive age-at-event
- [ ] epilogue age/mentor/family history

## 176G — UI

- [ ] age/life stage separate from health/capacity
- [ ] age tooltip
- [ ] capability reason tooltip
- [ ] retirement uses roster/duty if possible
- [ ] care UI reused
- [ ] mentorship UI reused
- [ ] age sorting/filter
- [ ] unknown age
- [ ] bounded notifications
- [ ] normal death UI
- [ ] accessibility
- [ ] neutral language

## 176G — Persistence

- [ ] anchors persisted
- [ ] no derived stage persisted
- [ ] no final performance modifier persisted
- [ ] care assignment not duplicated
- [ ] skill state not duplicated
- [ ] authored-age migration
- [ ] recruitment-age migration
- [ ] unknown-age fallback
- [ ] no visual/name age inference
- [ ] no random migration age
- [ ] milestone IDs
- [ ] stable birthday ID
- [ ] exact age/remainder round trip
- [ ] time-scale schema/version safety

## 176G — Determinism/Balance

- [ ] pure age derivation
- [ ] pure stage
- [ ] retirement eligibility deterministic
- [ ] care need deterministic
- [ ] mentorship through canonical skill system
- [ ] mortality through seeded canonical authority
- [ ] no wall clock
- [ ] stable survivor iteration
- [ ] older survivors retain value
- [ ] no blanket productive-age optimum
- [ ] skill offsets physical decline in suitable roles
- [ ] role diversity preserved
- [ ] care burden not automatic by age
- [ ] retirement has value
- [ ] no workforce cliff
- [ ] no fixed age death wipe
- [ ] birthday spam bounded
- [ ] mentorship exploit prevented
- [ ] retirement event farm prevented
- [ ] care health farm prevented

## 176G — Simulations

- [ ] 30-day ordinary campaign
- [ ] 120-day campaign
- [ ] 180-day campaign
- [ ] 400-day long campaign
- [ ] young shelter
- [ ] older shelter
- [ ] high-skill elder scenario
- [ ] unhealthy younger vs healthy elder anti-stereotype scenario
- [ ] retirement-heavy shelter
- [ ] three-generation scenario if supported

## 176G — CI

- [ ] life-stage integrity
- [ ] capability-profile integrity
- [ ] retirement-rule integrity
- [ ] localization
- [ ] aging selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] reachability
- [ ] deterministic fingerprint
- [ ] O(1) age query
- [ ] O(active survivors) day processing
- [ ] generated docs
- [ ] verify-fast

## 176H

- [ ] age-specific traits deferred unless non-duplicate
- [ ] youth-energy duplicate avoided
- [ ] elder-wisdom duplicate avoided
- [ ] coming-of-age gated on child system
- [ ] retirement party narrative follow-on
- [ ] elder council uses governance
- [ ] demographic chart derived only
- [ ] dependency ratio derived
- [ ] pensions out unless economy/governance requires
- [ ] elder housing gated on real accessibility mechanics
- [ ] funeral traditions use memorial/culture
- [ ] elder legacy derives from archive

---

# 50. Ship / No-Ship Gate

**SHIP** only if:

```text
campaign_chronology_authorities == 1
AND survivor_age_authorities == 1
AND duplicate_generational_age_clocks == 0
AND persisted_derived_life_stage_state == 0
AND direct_age_owned_work_multiplier_paths == 0
AND direct_age_owned_combat_multiplier_paths == 0
AND direct_age_owned_expedition_multiplier_paths == 0
AND generic_age_owned_health_state == false
AND generic_age_owned_care_assignment == false
AND passive_global_mentorship_bonus == false
AND direct_age_death_rng == false
AND old_save_random_age_assignment == false
AND old_save_visual_age_inference == false
AND duplicate_birthday_or_stage_events == 0
AND retirement_event_farming == false
AND retire_unretire_bypasses_duty_or_autonomy == false
AND fixed_age_workforce_cliff == false
AND healthy_elder_auto_care_requirement == false
AND time_scale_inconsistent_with_seasons_family_archive == false
AND aging_old_save == pass
AND aging_save_roundtrip == pass
AND aging_milestone_idempotence == pass
AND aging_performance_composition == pass
AND aging_care_integration == pass
AND aging_mentorship_integration == pass
AND aging_mortality_integration == pass
AND aging_determinism == pass
AND aging_30_day_balance == pass
AND aging_120_day_balance == pass
AND aging_180_day_balance == pass
AND aging_400_day_long_horizon == pass
AND aging_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 51. Implementer Handoff

1. Audit the actual survivor lifecycle and campaign clock before adding an `AgingSystem`.
2. Pick one age representation and persist the smallest chronology anchor needed to derive current age.
3. Reconcile `GenerationalSuccessionEngine.AdvanceTime()` so it cannot become a second clock.
4. Decide the in-world days-per-year mapping in an ADR before implementing birthday thresholds.
5. Make life stage a derived demographic/readability layer.
6. Do not encode a universal physical/mental/wisdom truth into stage data.
7. Feed age as one bounded contribution into the same Plan-24/137/143 capability path already used for fitness, needs, and afflictions.
8. Never ban an older survivor from a role solely because of chronological age.
9. Let actual skill, health, afflictions, fitness, and workload dominate capability where appropriate.
10. Treat retirement as a work/autonomy status, not a biological transition.
11. Add reduced duty as the important middle ground between full work and retirement.
12. Keep retired survivors in the same needs/resource economy.
13. Make elder care a specialization of generic care need, not “age > 60 = needs caregiver.”
14. Keep caregiver assignment and care delivery inside `CaregivingSystem`.
15. Make mentorship depend on actual expertise, time, and training activity.
16. Allow skilled younger mentors; let age/history be context rather than a hard gate.
17. Route any age-related mortality through the canonical health/death pipeline.
18. Preserve Plan-60 palliative care, memorial, grief, and death-quality ownership.
19. Use Plan-150 family chronology and Plan-162 archive for multi-generation history.
20. Do not guess old-save ages. Prefer known authored age, recruitment anchors, deterministic legacy fallback, or explicit unknown state.
21. Give birthday/stage/retirement events stable IDs so reload cannot replay them.
22. Test the anti-stereotype case explicitly: a healthy skilled elder should be able to outperform a younger badly ill survivor when the canonical systems say so.
23. Run 30/120/180/400-day demographic simulations before tuning aging rates or mortality upward.
24. Close only when age creates temporal depth and changing social roles without becoming a simplistic debuff clock.

---

# 52. Final Outcome

When this plan is complete, survivors finally live through time rather than merely existing inside it.

A survivor recruited in their twenties will actually grow older as campaign years pass. A survivor born in the shelter will share the same canonical chronology. Life-stage labels can change as that chronology advances, and important milestones can enter the journal or archive exactly once.

But age does not become destiny.

An older survivor does not automatically become weak, confused, or dependent. Their real capability remains the product of health, needs, afflictions, accumulated skills, fitness, experience, and current workload. Age contributes to that picture without replacing it.

That distinction matters mechanically.

A healthy, highly skilled older mechanic can remain one of the shelter's most valuable people. They may take fewer strenuous shifts while spending more time teaching apprentices. A younger survivor with a severe injury may require more care and have lower work capacity. The game therefore models **functional reality**, not age stereotypes.

Retirement also becomes a meaningful social role instead of a forced removal. Survivors can move from full duty to reduced duty or retirement, continue contributing in appropriate tasks, mentor others, and remain part of shelter life. If survivor autonomy or governance already gives them a voice, retirement decisions can reflect that rather than being purely a player toggle.

Care works the same way. Older people may become more likely to need assistance, but the actual need comes from health and capability. `CaregivingSystem` remains responsible for delivering that care. Palliative care remains responsible for end-of-life comfort. Mortality remains responsible for death.

Mentorship gives time a positive dimension. Experience accumulated across years can be passed through the real skill system. Knowledge survives because one survivor spent time teaching another, not because every elder emits a passive “wisdom” aura.

Family and archive systems gain continuity as well. Parents become grandparents. Mentors outlive or predecease apprentices. Retirement, milestones, natural decline, death, grief, and memorials become part of the shelter's recorded history.

The most important result is that campaign time finally has demographic consequences.

The shelter is not staffed by a permanently frozen roster.

People arrive.
They learn.
They age.
They adapt.
They teach.
They need care.
They retire.
They die.

And the community has to survive not only the wasteland, but the passage of generations.
