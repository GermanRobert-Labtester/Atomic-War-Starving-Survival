# C1 — Flagship Integration Plan [33]: Memory, Knowledge & Skill Decay — Reinforcement, Recall & Canonical Cognitive State

> **Output:** `C1_planintegration[33].md`
>
> **Source baseline:** Plan 185 — Memory & Knowledge Decay System
>
> **Primary mission:** add gradual, deterministic loss of unused knowledge and capability while preserving the existing authorities for skills, certifications, NPC memory, relationships, journal/history, education, survivor health, stress, needs, aging, and cognition.
>
> **Primary architectural rule:** there is no single universal “memory” gameplay object unless the repository already proves one is canonical. Skills, factual knowledge, autobiographical/event memory, relationship memory, procedural competence, and written records are different domains with different owners, persistence semantics, consequences, and reinforcement sources.
>
> **Primary correction to the source plan:** `SkillProgressionSystem` already has dormancy and Plan 180 protects certified skills. `PhantomMemoryEngine` already stores memory-like state. `SurvivorRelationsSystem` already owns relationship state/history. The flagship therefore does **not** create a second master list of every survivor memory and then mirror skill, relationship, journal, and event data into it. Instead, `MemoryDecaySystem` is a **decay/reinforcement coordinator and projection layer** over domain-owned records.
>
> **Mandatory execution order:** 185A authority/semantic audit → 185B canonical decay contracts by memory domain → 185C reinforcement/review/practice adapters → 185D recall/clarity and relearning → 185E journal/preservation and relationship/NPC-memory integration → 185F age/stress/health/nutrition/sleep modifiers → 185G UI, persistence, determinism, balance, long-horizon tests and CI → 185H advanced memory traits/legacy/trading only after the base system is stable.
>
> **Critical re-baseline rule:** before creating `MemoryDecaySystem.cs`, inspect `SkillProgressionSystem`, `PhantomMemoryEngine`, Plan-147 per-NPC memory, `SurvivorRelationsSystem`, journal/history, Plan-154 education, Plan-176 aging, Plan-180 certification, needs/sleep/nutrition/stress, affliction/cognitive status, survivor lifecycle, save orchestration, and any current memory IDs/event retention rules. Extend real owners instead of copying their state.
>
> **Guardrails:** no second skill level; no second relationship score; no second journal; no duplicate NPC memory store; no decay-owned survivor health; no arbitrary “mental decline” from age; no random forgetting roll when deterministic decay suffices; no deleting historical events from the archive because a survivor forgot them; no written journal record becoming factually corrupted because personal recall faded; no forgotten skill becoming literally unusable unless the canonical skill system supports capability loss below a real threshold; no daily event spam; no per-frame memory scan; no `Guid.NewGuid`; no wall clock; no unseeded RNG; no direct mutation of faction standing or quest state from forgetting; no relationship decay just because no conversation occurred unless the real relationship model distinguishes memory from relationship bond; no certification making a skill permanently immortal unless Plan 180 explicitly says so.

---

# 0. Mission

ASHFALL already contains several kinds of “memory,” but they are currently disconnected.

The source baseline identifies:

- `SkillProgressionSystem.cs` with a 14-day dormancy concept;
- Plan 180 certification that can halt or strongly reduce skill decay;
- `PhantomMemoryEngine.cs` storing memories that never fade;
- `SurvivorRelationsSystem.cs` storing relationship-related state/history;
- journal/history systems that preserve events;
- no broad forgetting/knowledge-decay system.

The current high-level behavior is therefore:

```text
SKILL
  └── may go dormant

KNOWLEDGE
  └── remains perfect forever

EVENT MEMORY
  └── remains perfectly clear forever

RELATIONSHIP HISTORY
  └── never loses recall fidelity

WRITTEN JOURNAL
  └── durable record
```

The target architecture is not:

```text
MemoryDecaySystem
  ├── copies every skill
  ├── copies every relationship
  ├── copies every event
  ├── copies every journal entry
  └── owns all decay
```

The target is:

```text
CANONICAL DOMAIN STATE
        │
        ├── SkillProgressionSystem
        ├── Knowledge/Education authority
        ├── PhantomMemory / NPC-memory authority
        ├── SurvivorRelationsSystem
        ├── Journal / Archive
        └── Certifications
        │
        ▼
MemoryDecayCoordinator
        │
        ├── decay policy lookup
        ├── reinforcement routing
        ├── recall/clarity projection
        ├── threshold transition events
        ├── relearning acceleration metadata
        └── diagnostics / UI read model
        │
        ▼
DOMAIN-SPECIFIC CONSEQUENCES
        │
        ├────────► skill dormancy/proficiency
        ├────────► knowledge availability
        ├────────► event recall fidelity
        ├────────► relationship recollection
        ├────────► education/review
        └────────► journal preservation
```

The system should answer:

> What cognitive or capability state has not been reinforced, how much has recall/proficiency degraded according to its domain rules, what can strengthen it again, and what observable consequence follows?

It should not answer:

> What is the survivor's skill rank?
> What is their relationship score?
> What happened historically?
> What does the journal say?
> What is faction standing?
> What quest is active?

Those remain canonical systems.

---

# 1. Source-Evidence Interpretation

## 1.1 Skill dormancy already exists

The source says:

```text
14 unused days → dormant
```

Therefore Plan 185 must first determine:

- whether dormancy is already the intended skill-decay mechanic;
- whether deeper proficiency decay is needed;
- whether dormant is only an availability/performance state;
- how reactivation currently works.

Do not build a parallel skill-strength system without this audit.

## 1.2 `BunkerSkillDecayStopped` already hints at a decay seam

The source found one Core reference:

```text
BunkerSkillDecayStopped
```

That means the repository may already contain:
- a callback/event for stopping decay;
- a certification/habitat effect;
- a partially implemented contract.

Reuse it if canonical.

## 1.3 `PhantomMemoryEngine` should not be blindly replaced

It already stores memories.

Plan 185 must classify:
- autobiographical memory;
- hallucination/phantom memory;
- trauma memory;
- narrative memory.

If it is not the canonical general memory store, do not force all memory types into it.

## 1.4 Relationship bond and memory of a relationship are distinct

A survivor can:
- still care deeply for someone;
- forget details of an old event.

Do not reduce relationship score merely because an event memory blurred.

## 1.5 Journal truth and personal recall are distinct

A written record can preserve:
- factual details externally.

The survivor may still:
- personally remember the event poorly.

Therefore:

```text
journal preservation
≠ personal memory immunity
```

unless a review action reinforces it.

## 1.6 “Memory clarity affects skill check bonuses” is too broad

Skill checks should continue to use:
- canonical skill/proficiency.

Event-memory clarity should not arbitrarily alter every skill check.

Only a specific knowledge/recall check should consume memory clarity.

## 1.7 Deterministic decay may not need RNG

The source proposes `ISeededRng`.

If decay is a deterministic daily curve:
- no RNG is required.

Use seeded RNG only for explicitly stochastic:
- recall failure;
- interference;
- probabilistic retrieval
if a real mechanic needs it.

---

# 2. Non-Negotiable Cognitive Decay Invariants

## INV-185.1 — One owner per memory domain

Skills, knowledge, event memories, relationships, and written records keep their canonical owners.

## INV-185.2 — Decay coordinator stores only new decay-specific facts

No mirrored full domain state.

## INV-185.3 — Historical truth never decays

The world/event archive remains true even if a survivor forgets.

## INV-185.4 — Personal recall may decay

Memory fidelity can change per survivor.

## INV-185.5 — Relationship bond does not automatically decay with memory clarity

## INV-185.6 — Skill dormancy and skill degradation must reconcile

No duplicate “dormant” and “forgotten” semantics without ADR.

## INV-185.7 — Certified skill semantics come from Plan 180

## INV-185.8 — Reinforcement comes from real actions

Practice, review, reminders, teaching, and experiences must have source events.

## INV-185.9 — No free reinforcement heartbeat

## INV-185.10 — Daily decay is deterministic unless a domain explicitly uses seeded stochastic recall

## INV-185.11 — Threshold transitions are exactly once

Fading/fragmentary/forgotten events cannot repeat daily.

## INV-185.12 — Forgotten does not always mean deleted

The domain decides:
- inaccessible;
- dormant;
- low-confidence;
- archived;
- relearnable.

## INV-185.13 — Relearning uses canonical education/skill systems

## INV-185.14 — Journal review can reinforce recall

Writing itself preserves the external record; review reinforces survivor memory.

## INV-185.15 — Teaching reinforces teacher only through real teaching activity

## INV-185.16 — Age is not a universal decay penalty

Any Plan-176 age contribution must be narrow, bounded, and evidence-backed.

## INV-185.17 — Stress, health, nutrition, sleep are domain inputs, not copied state

## INV-185.18 — No memory-decay effect directly writes faction, quest, market, or inventory state

## INV-185.19 — Old saves do not retroactively forget the entire campaign on first load

## INV-185.20 — Headless decay produces identical results for identical state/time

---

# 3. Definition of Done

Plan 185 closes only when:

- all memory-like authorities are inventoried;
- SkillProgression dormancy semantics are documented;
- Plan-180 certification interaction is documented;
- PhantomMemory ownership is documented;
- Plan-147 NPC-memory ownership is documented;
- relationship state vs relationship-event memory is separated;
- journal/archive truth vs personal recall is separated;
- one decay coordinator exists if needed;
- five source categories are either mapped to real domains or explicitly narrowed;
- no duplicate skill state exists;
- no duplicate relationship state exists;
- no duplicate journal/history exists;
- decay curves are data-driven and versioned;
- decay is processed on campaign-day boundaries or other bounded events;
- reinforcement has explicit source events;
- skill practice integrates with SkillProgression;
- knowledge review integrates with education/knowledge authority;
- journal review can reinforce event recall;
- reminders/related events use explicit semantic links;
- teaching reinforces through real training actions;
- threshold transitions are idempotent;
- relearning uses canonical skill/education pathways;
- certified skills behave exactly as Plan 180 intends;
- dormant skills do not double-decay unless explicitly designed;
- age/stress/health/nutrition/sleep modifiers have one owner each;
- no random forgetting is introduced merely for flavor;
- old saves get safe neutral decay metadata;
- no retroactive mass forgetting occurs on first migration;
- save/load preserves decay anchors and thresholds exactly;
- `--memory-decay-selftest` exists or equivalent;
- 30/120/180/400-day simulations prove decay is meaningful but maintainable;
- UI shows actionable at-risk knowledge rather than thousands of trivial memory rows;
- content acceptance proves every decay rule has a consumer and reinforcement path.

---

# 4. Phase P0 — Memory/Skill/Knowledge Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
SkillProgressionSystem fields/APIs
skill dormancy threshold
skill use events
skill reactivation rules
BunkerSkillDecayStopped owner/consumers
Plan-180 SkillCertificationSystem
PhantomMemoryEngine fields/APIs
Plan-147 NPC memory state
SurvivorRelationsSystem relationship/history fields
journal/history APIs
Plan-154 education/knowledge APIs
Plan-176 age/cognition inputs
stress/mental-health authority
needs/nutrition/sleep authority
affliction/cognitive authority
campaign calendar/day authority
save sections/order
```

## P0.2 Build authority matrix

Create:

`docs/cognition/MEMORY_DECAY_AUTHORITY_MATRIX.md`

Columns:

```text
memory domain
canonical owner
current strength/proficiency field
last-use/reinforcement field
persistence
decay role
status
```

Rows:
- skill proficiency;
- skill dormancy;
- certification;
- factual knowledge;
- procedural proficiency;
- autobiographical event memory;
- trauma/phantom memory;
- NPC personal memory;
- relationship bond;
- relationship-event recollection;
- journal record;
- archive record;
- quest/world knowledge.

## P0.3 Create semantic ADR

`docs/architecture/ADR_MEMORY_DECAY_DOMAINS.md`

Answer:

```text
What counts as skill?
What counts as knowledge?
What counts as event memory?
What counts as relationship memory?
What is procedural memory?
What is an external record?
What can actually be forgotten?
What remains historical truth?
```

## P0.4 Skill decay ADR

`ADR_SKILL_DORMANCY_VS_DECAY.md`

Decide:
- dormancy only;
- dormancy + proficiency loss;
- thresholded access;
- certification interaction.

## P0.5 Journal preservation ADR

`ADR_JOURNAL_VS_PERSONAL_RECALL.md`

Define:
- record;
- recall;
- review.

## P0.6 Baseline proof

Demonstrate:
- skill dormancy behavior;
- PhantomMemory persistence;
- relationship persistence;
- journal permanence;
- lack of broader forgetting.

---

# TASK 185A — Domain-Specific Decay Contract

# 185A.0 Goal

Define decay as domain policy rather than a universal memory list.

## 185A.1 Proposed coordinator

If needed:

`Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs`

Owns:
- decay policy registry;
- per-record decay anchor only when owner cannot store it;
- threshold crossing history;
- reinforcement provenance;
- diagnostics.

## 185A.2 Do not persist full mirrored `Memory` DTOs by default

Source proposes:

```text
Memory
  content
  strength
  clarity
  lastReinforced
```

This is acceptable only for genuine event-memory records that have no existing owner.

It is not acceptable as a mirror of every skill/relationship.

## 185A.3 Decay type registry

`Assets/StreamingAssets/Data/decay_rates.json`

Versioned.

## 185A.4 Decay policy DTO

Suggested:

```text
policy_id
domain
base_curve
reinforcement_profile
floor
thresholds[]
modifier_refs[]
forgetting_semantics
relearning_profile
```

## 185A.5 Curves

Prefer:
- linear;
- exponential;
- piecewise
with explicit deterministic formula.

## 185A.6 No arbitrary daily subtraction in many call sites

One policy evaluator.

## 185A.7 Five source categories

Map carefully:

### Skill
Canonical SkillProgression.

### Knowledge
Education/knowledge authority.

### Event
Memory/NPC memory.

### Relationship
Usually event recollection only, not relationship bond.

### Procedural
Likely skill subtype/profile.

## 185A.8 Procedural may not need separate storage

Could be:
- skill decay profile.

## 185A.9 Physical vs mental skills

Only if skill metadata already classifies them.

## 185A.10 Decay floor

Meaning depends on domain.

Example:
- certified skill floor;
- event memory minimum gist;
- relationship memory persistent milestone.

## 185A.11 “0 = forgotten”

Not universal.

For skill:
- may mean dormant/basic familiarity.

For event:
- personal recall inaccessible.

For written knowledge:
- external record remains.

## 185A.12 Thresholds

Candidate:

```text
clear
vague
fragmentary
inaccessible
```

Prefer semantic labels over raw 50/25 universal thresholds.

## 185A.13 Source 50/25/0 values

Treat as tuning candidates.

## 185A.14 Threshold profile per domain

## 185A.15 No daily “decay logged” event spam

Store:
- anchor;
- current computed value;
- threshold transitions.

## 185A.16 Lazy evaluation

Possible optimization:
- compute current strength from last reinforcement day when queried;
- process threshold crossings on day boundary.

## 185A.17 Domain adapter interface

Suggested:

```text
IMemoryDecayDomainAdapter
  EnumerateDecayRecords()
  GetAnchor()
  ApplyThresholdTransition()
  ApplyReinforcement()
  ResolveForgetting()
```

## 185A.18 No generic reflection

## 185A.19 Stable record ID

Use canonical:
- skill ID + survivor ID;
- knowledge ID + survivor ID;
- event memory ID.

## 185A.20 No GUID

## 185A.21 Content field

Do not persist free-form “what is remembered” if canonical IDs/text already exist.

## 185A.22 Read model

Can render human-friendly description from owner.

## 185A.23 Data integrity

Policy domain must have:
- registered adapter;
- valid curve;
- valid bounds.

### 185A DoD

Decay becomes a typed policy over canonical cognitive/capability records rather than a shadow database of the survivor's entire mind.

---

# TASK 185B — Skill Dormancy, Practice, Certification & Relearning

# 185B.0 Goal

Extend the existing skill system without creating a second skill-strength authority.

## 185B.1 Audit current skill fields

Identify:
- level;
- XP;
- proficiency;
- last used day;
- dormant bool/state;
- reactivation.

## 185B.2 Preserve canonical skill level

Do not duplicate into memory strength.

## 185B.3 Choose decay semantic

Options:

### Option A — Dormancy only
Unused skill becomes dormant; practice reactivates.

### Option B — Dormancy + bounded proficiency erosion
Canonical skill proficiency falls slowly after dormancy.

### Option C — Retained level + temporary performance familiarity penalty
No permanent level loss.

Choose one.

## 185B.4 Recommended default

Prefer:
- dormancy + bounded familiarity/proficiency penalty;
- do not erase hard-earned rank rapidly.

## 185B.5 14-day dormancy

Keep current threshold unless re-tuning has evidence.

## 185B.6 Practice event

Skill use emits:

```text
skill_practiced
survivor_id
skill_id
day
context
```

## 185B.7 Reinforcement

SkillProgression consumes practice.

MemoryDecayCoordinator only routes/records threshold provenance.

## 185B.8 Certified skill

Plan 180 owns certification effect.

Source says:
- 0.1× decay.

Treat as Plan-180 policy value, not hardcoded here.

## 185B.9 `BunkerSkillDecayStopped`

Reconcile with certification.

Do not create:
- “0.1× certification”
and
- “stopped”
at the same time accidentally.

## 185B.10 Dormant decay

Source proposes 0.5×.

This may be backwards semantically.

If dormant means “already unused,” why slower?

Audit before keeping.

## 185B.11 Skill floor

Do not decay below:
- baseline competence;
- certification floor;
- trait floor
unless skill system explicitly supports relearning from zero.

## 185B.12 Relearning

Canonical skill training path.

## 185B.13 Relearning acceleration

Can derive from:
- prior peak proficiency;
- certification;
- retained familiarity.

Persist peak only if SkillProgression does not already.

## 185B.14 No free XP grant

Relearning still requires:
- practice;
- education;
- work.

## 185B.15 Partial memory

Could mean:
- familiarity modifier.

## 185B.16 UI

Show:
- active;
- rusty/dormant;
- retained certification;
- practice recommendation.

## 185B.17 No “skill forgotten, cannot use” unless skill design supports hard gate

Prefer:
- reduced effectiveness;
- slower checks;
- need for refresher.

## 185B.18 Tests

- active skill;
- 14-day dormancy;
- certified;
- bunker protection;
- practice;
- relearning;
- save/load;
- no duplicate skill state.

### 185B DoD

Skill decay is reconciled with existing dormancy/certification semantics and remains owned by `SkillProgressionSystem`.

---

# TASK 185C — Knowledge Decay & Review

# 185C.0 Goal

Make explicit learned knowledge maintainable through review/teaching without inventing a generic trivia database.

## 185C.1 Audit knowledge authority

Plan 154 may have:
- curriculum;
- learned subjects;
- education levels;
- knowledge flags.

## 185C.2 Only decay explicit gameplay knowledge

Examples:
- medical procedure knowledge;
- radio code knowledge;
- recipe familiarity;
- map/intel facts
only if these are modeled as survivor-owned knowledge.

## 185C.3 Do not decay global player UI knowledge

The player does not forget controls or tutorial information because a survivor does.

## 185C.4 Knowledge record

Canonical ID:

```text
survivor_id + knowledge_id
```

## 185C.5 Strength

If knowledge owner lacks proficiency/retention, a decay-specific retention value may be justified.

## 185C.6 Retention scale

0–100 or normalized 0–1.

Use one canonical value.

## 185C.7 Review

Real action:
- reading;
- class;
- training;
- journal/manual study.

## 185C.8 Source material

Requires real:
- book;
- journal;
- instructor;
- workstation
where appropriate.

## 185C.9 Review time

Real campaign time/duty.

## 185C.10 Teaching

Teacher reinforces knowledge by:
- actual education action.

## 185C.11 Student learning

Plan 154 owns acquisition.

## 185C.12 Knowledge floor

Some basic knowledge may never fully disappear.

Data-driven.

## 185C.13 Critical knowledge

Could decay more slowly if:
- repeatedly used;
- well practiced.

## 185C.14 Forgotten knowledge

Means:
- cannot supply knowledge-dependent bonus/option.

It does not delete:
- external manual;
- journal;
- world truth.

## 185C.15 Reacquisition

Review/training.

## 185C.16 Relearning faster

Possible through retained familiarity.

## 185C.17 No direct quest failure

If a quest option requires knowledge and survivor loses it:
- prerequisite evaluates false.

Quest runtime owns outcome.

## 185C.18 Tests

- learn;
- neglect;
- vague;
- inaccessible;
- review;
- teach;
- reacquire;
- save/load.

### 185C DoD

Learned knowledge can fade and be reviewed through the education/knowledge system without turning the entire player-facing knowledge base into a decaying survivor property.

---

# TASK 185D — Event Memory, Recall & Clarity

# 185D.0 Goal

Allow autobiographical memory fidelity to fade while preserving objective history.

## 185D.1 Audit PhantomMemoryEngine

Classify:
- memory IDs;
- subject;
- event source;
- emotional content;
- truth/provenance.

## 185D.2 Audit Plan 147 memory

NPC memory likely already stores:
- event;
- valence;
- salience;
- target;
- relationship effect.

## 185D.3 Choose canonical event-memory owner

Do not duplicate.

## 185D.4 Event memory fields

If current owner lacks decay:

```text
last_reinforced_day
retention_anchor
clarity
access_state
```

Minimum needed.

## 185D.5 Clarity

Clarity is useful for:
- recall detail;
- dialogue reference;
- confidence.

It should not become a universal stat bonus.

## 185D.6 Strength vs clarity

Avoid redundant dual 0–100 values unless both have distinct consumers.

Preferred:
- retention strength drives derived clarity band.

## 185D.7 High clarity

Can recall:
- actors;
- place;
- broad outcome;
- details.

## 185D.8 Vague

Can recall:
- gist;
- emotional tone.

## 185D.9 Fragmentary

Can recall:
- isolated detail;
- uncertain attribution.

## 185D.10 Inaccessible

No spontaneous personal recall.

## 185D.11 Historical record remains

Archive/journal unaffected.

## 185D.12 Reminder

Related event can reinforce only if semantic relation exists.

## 185D.13 Similar experience

Can trigger:
- memory resurfacing;
- reinforcement
if current memory system supports associations.

## 185D.14 No arbitrary similarity string matching

Use tags/event links.

## 185D.15 Emotional salience

Strong trauma/bond events may decay slower.

If memory system already stores salience:
- use it.

## 185D.16 Trauma memory

Do not assume traumatic memories decay normally.

Potential:
- persistent/intrusive.

Psychological systems own trauma consequence.

## 185D.17 Phantom memories

If false/phantom memory exists:
- decay policy can differ.

Do not turn decay into truth correction.

## 185D.18 Recall check

Only if gameplay has explicit:
- remember clue;
- identify person;
- recount event.

## 185D.19 Recall determinism

Could be deterministic threshold-based.

Seeded uncertainty only if needed.

## 185D.20 “Unreliable journal entries”

Reject by default.

Journal is an external record.
Personal retelling can be unreliable.

## 185D.21 Tests

- salient memory;
- ordinary memory;
- trauma memory;
- reminder;
- review;
- journal record stays exact;
- inaccessible recall.

### 185D DoD

Survivors can forget details of lived events without rewriting objective history, corrupting the journal, or duplicating the NPC-memory system.

---

# TASK 185E — Relationship Memory vs Relationship Bond

# 185E.0 Goal

Prevent cognitive decay from silently becoming a second social-relations decay engine.

## 185E.1 Audit SurvivorRelationsSystem

Separate:
- bond/trust score;
- event memories;
- grudge;
- affection;
- commitments.

## 185E.2 Relationship bond remains canonical

## 185E.3 Memory of relationship events may decay

Example:
- exact words of an old argument;
- specific favor;
- date/place.

## 185E.4 Deep bond does not disappear because detail faded

## 185E.5 Interaction reinforcement

Current conversation/cooperation can reinforce:
- relationship-related memory salience;
- bond through relations system.

These are separate effects.

## 185E.6 “Strong relationships resist decay”

Interpret as:
- salient relationship memories decay slower.

Not:
- relationship score gets immunity.

## 185E.7 Grudges

If grudge system explicitly depends on remembered event:
- fading memory may reduce the **memory-driven modifier**.

Do not mutate generic relation score unless relations authority defines it.

## 185E.8 Reconciliation

Can:
- change bond;
- reframe memory;
through existing relation/NPC-memory systems.

## 185E.9 Dead/missing person

Relationship memories can persist.

## 185E.10 Memorial reminder

May reinforce memories of deceased.

## 185E.11 No heartbeat decay

No:
`relationship -= 1/day without interaction`.

Unless SurvivorRelations already intentionally models that separately.

## 185E.12 Tests

- strong bond, faded event detail;
- grudge memory;
- interaction;
- deceased survivor;
- save/load.

### 185E DoD

Relationship-memory fidelity can change without duplicating or destabilizing the canonical relationship model.

---

# TASK 185F — Journal, External Records & Preservation

# 185F.0 Goal

Make written records useful without conflating storage of truth with personal memory strength.

## 185F.1 Journal owns entries

## 185F.2 Archive owns historical record

## 185F.3 Writing event down

Creates:
- durable external record.

It does **not** automatically freeze personal memory forever.

## 185F.4 Review action

Reading the record can reinforce:
- event memory;
- factual knowledge.

## 185F.5 Source plan “journal preserves event memories”

Reframe:

```text
journal preserves information externally
review preserves/reinforces internal recall
```

## 185F.6 Journal entry fidelity

Should reflect:
- what was known/recorded at writing time.

Not current fading memory unless journal is explicitly written later from memory.

## 185F.7 Memory-written-later case

If survivor writes from recollection:
- clarity may affect certainty/detail.

This is follow-on unless journal authorship mechanics exist.

## 185F.8 Reminder links

Journal entries can reference:
- event IDs;
- people;
- locations.

Review uses these links.

## 185F.9 No free daily reinforcement

A journal sitting in storage does nothing.

## 185F.10 Manuals

Knowledge preservation can also use:
- books/manuals
if current item/education system supports.

## 185F.11 Teaching notes

Same.

## 185F.12 Data integrity

Every preservable knowledge/memory link must resolve.

## 185F.13 UI

From memory detail:
- “Review journal record”
if available.

## 185F.14 Tests

- recorded event;
- memory fades;
- journal stays exact;
- review strengthens recall;
- missing/destroyed record if itemized.

### 185F DoD

External records preserve facts while deliberate review reinforces internal memory, creating strategic value without making the journal a magical permanent-memory aura.

---

# TASK 185G — Age, Stress, Health, Nutrition & Sleep Modifiers

# 185G.0 Goal

Compose cognitive-decay pressure from real survivor state without stereotypes or duplicate health logic.

## 185G.1 Age

Plan 176 correction applies.

Do not hardcode:
- “elderly forget faster.”

Audit whether Plan 176 exposes:
- cognitive capacity;
- memory consolidation contribution.

## 185G.2 If age modifier exists

Use:
- bounded curve;
- domain-specific;
- not universal.

## 185G.3 Stress

Use canonical stress/mental-health state.

High stress may affect:
- consolidation;
- recall;
if modeled.

## 185G.4 No direct stress copy

## 185G.5 Health

Use relevant:
- neurological;
- fatigue;
- illness
state only.

“Good health slows memory decay” should not be a generic health bonus unless justified.

## 185G.6 Nutrition

Use needs/nutrition data.

Severe malnutrition can influence cognition if existing medical/needs rails expose it.

## 185G.7 Sleep

Fatigue/sleep quality may affect:
- consolidation.

## 185G.8 No extra sleep system

## 185G.9 Afflictions

Some afflictions may modify:
- memory;
- cognition.

Use Plan 143/medical adapters.

## 185G.10 Medication

Only if medical system exposes cognition effect.

## 185G.11 Trauma

Trauma can:
- preserve intrusive memories;
- impair recall
depending on existing mental-health semantics.

Do not flatten to one multiplier.

## 185G.12 Modifier composition

Suggested:

```text
base decay
× certification/practice protection
× cognitive condition factor
× stress/sleep factor
× domain salience factor
```

Bounded.

## 185G.13 No multiplicative explosion

Clamp.

## 185G.14 Diagnostics

Show source contributions.

## 185G.15 UI wording

Actionable:
- “Severe fatigue is making review less effective.”

Avoid:
- “Old age: +50% forgetting.”

## 185G.16 Tests

- healthy rested;
- stressed;
- exhausted;
- malnourished;
- cognitive affliction;
- older healthy;
- younger impaired.

### 185G DoD

Cognitive decay responds to real survivor condition through bounded adapters without inventing a parallel age, stress, nutrition, sleep, or health model.

---

# TASK 185H — Reinforcement & Retrieval Pipeline

# 185H.0 Goal

Make memory maintenance arise from normal gameplay actions rather than repetitive maintenance chores.

## 185H.1 Reinforcement source types

Candidate:

```text
practice
review
teaching
related_experience
explicit_reminder
conversation
journal_review
```

Only real actions.

## 185H.2 Reinforcement DTO

Do not persist every trivial event forever.

Use:
- source event ID;
- record ID;
- day;
- amount/profile;
- semantic type.

Compact history.

## 185H.3 Practice

Skills.

## 185H.4 Review

Knowledge/event recall.

## 185H.5 Teaching

Knowledge/skill.

## 185H.6 Reminder

Event memory.

## 185H.7 Similar experience

Only with semantic tags.

## 185H.8 Conversation

Relationship/event memory if dialogue system exposes it.

## 185H.9 Reinforcement cap

Cannot exceed domain maximum.

## 185H.10 Diminishing same-day reinforcement

Prevents spam.

## 185H.11 Repeated trivial action exploit

One easy action should not fully restore a high-level skill.

## 185H.12 Practice quality

Can depend on:
- actual task difficulty;
- duration;
- success
if SkillProgression supports.

## 185H.13 Review quality

Can depend on:
- source quality;
- teacher skill;
- time.

## 185H.14 No AFK reinforcement

## 185H.15 Reinforcement queue

None required.

Apply on semantic event.

## 185H.16 Consolidation delay

Optional:
- sleep/day boundary
only if current skill/education systems support.

Default:
- immediate bounded reinforcement.

## 185H.17 Threshold recovery

Crossing from:
- fragmentary → vague;
- vague → clear
can emit one semantic event.

## 185H.18 No every-use journal spam

## 185H.19 Reinforcement diagnostics

Track:
- source;
- amount;
- resulting state.

### 185H DoD

Practice, review, teaching, reminders, and relevant experiences reinforce the correct domain records through semantic events and cannot be farmed through trivial repetition.

---

# TASK 185I — Forgetting, Recall Failure & Relearning

# 185I.0 Goal

Make decay consequential without turning forgetting into permanent content deletion.

## 185I.1 Threshold states

Suggested common vocabulary:

```text
clear
vague
fragmentary
inaccessible
```

Domain may override.

## 185I.2 “Forgotten” UI term

Can represent:
- inaccessible without prompt/relearning.

## 185I.3 Do not physically delete canonical records at 0

Prefer:
- inactive;
- inaccessible;
- retained historical provenance.

## 185I.4 Why retain?

Needed for:
- relearning acceleration;
- archive;
- continuity;
- memory resurfacing.

## 185I.5 Skill relearning

Canonical training.

## 185I.6 Knowledge relearning

Education/review.

## 185I.7 Event memory resurfacing

Reminder/similar event.

## 185I.8 Relationship recollection

Conversation/reminder.

## 185I.9 Prior familiarity bonus

Bounded.

## 185I.10 Never stronger than prior peak without new learning

## 185I.11 Rediscovery event

Semantic milestone.

## 185I.12 No quest generation requirement

Plan 171 may later consume:
- memory_faded;
- knowledge_inaccessible;
- skill_reactivated
events.

Memory system does not invent quests.

## 185I.13 Recall checks

If a dialogue/quest asks:
- “Do you remember X?”

Use:
- current recall state.

## 185I.14 No retroactive world mutation

Forgetting a location does not delete the map if world-map knowledge is a separate player/community record.

## 185I.15 Community knowledge vs personal knowledge

Audit whether some knowledge is shared shelter knowledge.

Do not force per-survivor forgetting onto community records.

## 185I.16 Critical operation safety

If forgetting could make:
- medical;
- power;
- shelter operation
impossible,
ensure:
- manuals;
- other survivors;
- retraining
provide recoverability.

## 185I.17 Tests

- inaccessible skill familiarity;
- reacquire;
- event resurfacing;
- community knowledge unaffected;
- no record deletion.

### 185I DoD

Forgetting reduces access and fidelity but preserves enough provenance for coherent relearning, resurfacing, and historical continuity.

---

# TASK 185J — UI, Persistence, Determinism, Balance & CI

# 185J.0 Goal

Make decay visible and actionable without turning ASHFALL into a memory-maintenance spreadsheet.

---

# 185J-U — UI

## 185J.U1 Do not show every trivial memory by default

Source proposes:
- “all memories.”

That could be overwhelming.

Prefer:
- at-risk;
- important;
- filtered categories.

## 185J.U2 Survivor detail integration

Show:
- rusty skills;
- at-risk knowledge;
- important fading memories.

## 185J.U3 Dedicated memory panel

Only if the repository already has enough explicit memory content to justify it.

Otherwise:
- survivor tabs.

## 185J.U4 Strength/clarity

Use semantic bands first.

Exact percentage:
- optional advanced tooltip.

## 185J.U5 Reinforcement actions

Show available:
- practice;
- review;
- teach;
- journal.

## 185J.U6 Risk explanation

Why fading:
- unused;
- stress;
- sleep;
- condition.

## 185J.U7 No warning spam

Only:
- important memory;
- threshold crossing;
- critical skill/knowledge.

## 185J.U8 “Fading warning”

Not every 50% crossing if content is trivial.

Importance filter.

## 185J.U9 Forgotten history

Archive/diagnostic; not necessarily daily UI.

## 185J.U10 Tutorial

First meaningful fade event.

## 185J.U11 Tooltips

State + source + recovery.

## 185J.U12 Accessibility

No color-only clarity.

## 185J.U13 Text scale

Long memory descriptions wrap.

## 185J.U14 Neutral language

Avoid implying real-world cognitive disease from ordinary forgetting.

---

# 185J-P — Persistence

## 185J.P1 Persist minimal decay-specific anchors

Potential:

```text
record_id
last_reinforced_day
retention_anchor
processed_thresholds
prior_peak optional
```

only where canonical owner does not.

## 185J.P2 Do not persist duplicate content strings

## 185J.P3 Do not persist duplicate skill level

## 185J.P4 Do not persist duplicate relation score

## 185J.P5 Forgetting event history

Bounded semantic events.

## 185J.P6 Reinforcement history

Compact/bounded.

## 185J.P7 Old save

Critical rule:

```text
existing records start at current canonical strength/clear state
last_reinforced_day = migration day or domain-specific safe anchor
```

This prevents first-load mass decay.

## 185J.P8 Do not pretend they have been decaying since campaign day 1 retroactively

unless a migration explicitly reconstructs trustworthy last-use history.

## 185J.P9 Skill last-use history

If SkillProgression already stores it:
- preserve.

## 185J.P10 Event memory age

If memory created earlier but has no reinforcement history:
- choose safe migration policy.

## 185J.P11 Save/load

Same computed retention.

## 185J.P12 Schema versioning

Policy changes must not silently reinterpret existing retention state without migration.

---

# 185J-D — Determinism

## 185J.D1 Decay

Pure from:
- policy;
- anchor;
- elapsed campaign days;
- modifiers.

## 185J.D2 Reinforcement

Deterministic from event/profile.

## 185J.D3 Threshold events

Deterministic.

## 185J.D4 Recall uncertainty

Seed only if implemented.

## 185J.D5 No wall clock

## 185J.D6 Stable iteration

Sort record IDs.

---

# 185J-B — Balance

## 185J.B1 Decay should create maintenance choices, not chores

## 185J.B2 Critical skill cadence

Players should not need to practice every skill every few days.

## 185J.B3 Long-term dormancy

Meaningful after weeks/months.

## 185J.B4 Certified skills

Near-stable if Plan 180 intends.

## 185J.B5 Knowledge

Review cadence reasonable.

## 185J.B6 Event memory

Most flavor memories can fade without player intervention.

## 185J.B7 Important event memory

Strong salience / journal records.

## 185J.B8 Relationship memories

Slow decay.

## 185J.B9 Procedural

Slowest if retained as distinct profile.

## 185J.B10 Relearning

Faster than first learning, but still costs time.

## 185J.B11 No total cognitive collapse

A survivor neglected for a long period should not lose every useful capability simultaneously.

## 185J.B12 Skills used in normal work automatically reinforce

This is crucial.

## 185J.B13 Teaching should count as reinforcement

## 185J.B14 Routine interaction can maintain salient relationship memories

## 185J.B15 High stress/fatigue modifiers bounded

## 185J.B16 Age modifier bounded

## 185J.B17 No punishment for playing efficiently

Common task use should naturally maintain common skills.

---

# 185J-S — Long-Horizon Simulation

## 185J.S1 30-day normal play

Assert:
- only unused records begin fading;
- active work skills stable.

## 185J.S2 120-day mixed roster

Track:
- dormant skills;
- knowledge review;
- event-memory bands;
- reinforcement load.

## 185J.S3 180-day campaign

Track:
- relearning;
- certification protection;
- relationship-memory persistence.

## 185J.S4 400-day soak

Track:
- memory record count;
- bounded history;
- no performance collapse;
- archive continuity.

## 185J.S5 Constant reinforcement

No decay.

## 185J.S6 Total neglect

Gradual, not sudden.

## 185J.S7 Certified expert

Retains capability per Plan 180.

## 185J.S8 Stressed/exhausted survivor

Bounded faster decay.

## 185J.S9 Journal-heavy survivor

External record survives; reviewed memory improves.

## 185J.S10 Old-save migration

No immediate fade storm.

---

# 185J-T — Testing & CI

## 185J.T1 Data integrity

Validate:
- policy IDs;
- domains;
- curves;
- bounds;
- reinforcement profiles;
- adapter IDs;
- localization.

## 185J.T2 Selftest

Create:

```text
--memory-decay-selftest
```

## 185J.T3 Selftest scenarios

At least:
1. active skill no dormancy;
2. dormant skill;
3. certified skill;
4. skill practice;
5. learned knowledge decay;
6. knowledge review;
7. event memory clarity;
8. journal external record preservation;
9. relationship event memory;
10. strong bond unaffected by recall fade;
11. relearning;
12. age/stress/sleep modifier;
13. old save;
14. save/load;
15. threshold idempotence;
16. headless.

## 185J.T4 Source-scan authority gate

Detect:
- duplicate skill level in memory-decay state;
- duplicate relationship score;
- duplicate journal entry;
- direct quest/faction/inventory mutation;
- per-frame scanning;
- unseeded random forgetting.

## 185J.T5 Content acceptance

Every decay policy:
- adapter registered;
- record reachable;
- reinforcement path exists.

## 185J.T6 Reachability

Every state:
- clear;
- vague;
- fragmentary;
- inaccessible;
- reinforced/relearned
has deterministic fixture.

## 185J.T7 Performance

Day processing:
- O(records eligible for decay) or indexed subset.

No full archive scan.

## 185J.T8 History retention

Threshold/reinforcement logs bounded.

## 185J.T9 Generated docs

Create:
- `MEMORY_DECAY_ARCHITECTURE.md`;
- `MEMORY_DECAY_AUTHORITY_MATRIX.md`;
- `MEMORY_DECAY_POLICY_MATRIX.md`;
- `SKILL_DORMANCY_DECAY_MATRIX.md`;
- `MEMORY_REINFORCEMENT_MATRIX.md`;
- `MEMORY_MIGRATION_MATRIX.md`;
- `MEMORY_BALANCE_REPORT.md`;
- `ADR_MEMORY_DECAY_DOMAINS.md`;
- `ADR_SKILL_DORMANCY_VS_DECAY.md`;
- `ADR_JOURNAL_VS_PERSONAL_RECALL.md`.

### 185J DoD

Memory decay is deterministic, bounded, understandable, maintainable through normal gameplay, and incapable of duplicating skill, relationship, journal, or historical truth.

---

# TASK 185K — Advanced Memory Traits, Legacy & Memory Exchange: Explicit Follow-On

# 185K.0 Goal

Keep advanced cognition features from bloating the base decay system.

## 185K.1 Photographic memory trait

Only if:
- trait system supports cognitive modifier;
- balance is clear.

Potential:
- slower event/knowledge decay.

Not immunity by default.

## 185K.2 Forgetful trait

Avoid stigmatizing language if it maps to a real cognitive condition.

Prefer:
- retention variation
through trait data.

## 185K.3 Memory legacy

Important memories can be preserved in:
- archive;
- journal;
- memorial.

No separate legacy database.

## 185K.4 Memory quests

Plan 171 can consume:
- memory-fade events.

Dynamic quest runtime owns them.

## 185K.5 Memory flashes

Use PhantomMemory/mental-health systems.

## 185K.6 Déjà vu

Narrative follow-on.

## 185K.7 Memory trading

Default:
- DEFER.

Requires:
- teaching;
- testimony;
- information transfer.

Not literal memory-object trading unless fiction supports it.

## 185K.8 Shared institutional knowledge

Potential:
- manuals;
- archives;
- training curriculum.

Use education/archive systems.

## 185K.9 Memory implants / sci-fi tech

Out of baseline unless ASHFALL fiction explicitly supports.

### 185K DoD

Advanced memory features remain layered on canonical cognition, information, education, journal, and quest systems rather than distorting the foundational decay model.

---

# 5. Cognitive Domain State Model

```text
CANONICAL RECORD
      │
      ├── skill
      ├── knowledge
      ├── event memory
      └── relationship-event memory
      │
      ▼
DECAY POLICY
      │
      ├── elapsed time
      ├── salience
      ├── certification
      ├── condition modifiers
      └── reinforcement history
      │
      ▼
RETENTION / RECALL STATE
      │
      ├── clear
      ├── vague
      ├── fragmentary
      └── inaccessible
      │
      ▼
REINFORCEMENT / RELEARNING
      │
      └── canonical domain action
```

---

# 6. Domain Ownership Contract

## Skills
`SkillProgressionSystem`.

## Knowledge
Education/knowledge authority.

## Event memories
PhantomMemory / Plan-147 memory authority.

## Relationship bond
`SurvivorRelationsSystem`.

## External record
Journal/archive.

MemoryDecay coordinates policy and thresholds.

---

# 7. Historical Truth Contract

A survivor forgetting:

```text
Faction X attacked us on Day 40
```

does not make the event disappear.

Archive/history still knows it happened.

---

# 8. Journal Truth Contract

Journal preserves:
- what was recorded.

It does not automatically guarantee:
- personal recall.

Review is the bridge.

---

# 9. Skill Dormancy Contract

Dormancy should be reconciled with any deeper decay.

Preferred hierarchy:

```text
active
→ rusty
→ dormant
→ deeply dormant / relearning needed
```

if the skill system supports it.

No separate memory-strength ladder.

---

# 10. Certification Contract

Certification is:
- durable proof/mastery support.

Plan 180 owns exact protection.

MemoryDecay reads the certified status/profile.

---

# 11. Knowledge Contract

Knowledge forgetting affects:
- access to knowledge-dependent options/checks.

Not:
- global world truth;
- player UI;
- archived documents.

---

# 12. Event Memory Contract

Event-memory clarity affects:
- recollection;
- dialogue detail;
- personal interpretation.

Not:
- event outcome history.

---

# 13. Relationship Memory Contract

Relationship event recollection may fade.

Relationship bond remains a separate social fact.

---

# 14. Procedural Memory Contract

Prefer representing as:
- skill decay profile;
not duplicate memory domain
unless current systems distinguish it meaningfully.

---

# 15. Reinforcement Contract

Reinforcement requires:
- semantic source action/event.

No passive “once per day maintain memory.”

---

# 16. Practice Contract

Using a skill naturally maintains it.

This prevents routine gameplay from becoming maintenance micromanagement.

---

# 17. Review Contract

Review consumes:
- time;
- educational/journal/manual resource
where appropriate.

---

# 18. Teaching Contract

Teaching can reinforce:
- teacher;
- student
through education/skill systems.

No free duplicate reward.

---

# 19. Reminder Contract

Reminder must reference:
- related event/person/location/tag.

No string similarity.

---

# 20. Relearning Contract

Prior familiarity may accelerate recovery.

The canonical learning system still owns:
- XP;
- proficiency;
- unlock.

---

# 21. Clarity Contract

Clarity should preferably be derived from retention.

Avoid two independent bars unless distinct mechanics consume both.

---

# 22. Forgetting Event Contract

Log only semantic transitions:

```text
became vague
became fragmentary
became inaccessible
recovered clarity
relearned
```

No daily loss event.

---

# 23. Age Contract

Plan 176 remains authority for:
- age.

Memory decay may consume a bounded cognition-related contribution only if that plan exposes it.

---

# 24. Stress Contract

Mental-health authority owns stress.

MemoryDecay reads it.

---

# 25. Nutrition / Sleep Contract

Needs system owns:
- hunger;
- fatigue;
- sleep.

MemoryDecay may read cognition-relevant projections.

---

# 26. Affliction Contract

Medical/affliction system owns:
- cognitive impairment.

MemoryDecay consumes a typed modifier.

---

# 27. Community Knowledge Contract

Some knowledge belongs to the shelter/community, not one survivor.

Do not apply per-survivor forgetting to:
- shared map;
- archive;
- public manual;
- faction ledger
unless explicitly modeled.

---

# 28. Quest Contract

Memory events are predicates/triggers only.

Quest runtime owns:
- availability;
- completion;
- consequence.

---

# 29. Dynamic Quest Contract

Plan 171 can generate:
- refresher;
- rediscovery;
- missing-knowledge
opportunities from memory events.

MemoryDecay does not generate them itself.

---

# 30. Persistence Matrix

| Fact | Owner |
|---|---|
| skill level | SkillProgression |
| skill dormant state | SkillProgression |
| certification | Plan 180 |
| knowledge acquired | education/knowledge |
| memory record | PhantomMemory/NPC memory |
| relationship bond | SurvivorRelations |
| journal entry | Journal |
| archive truth | Archive |
| decay policy | decay data |
| last reinforcement | owner or decay coordinator |
| threshold history | decay coordinator |
| relearning peak/familiarity | owner or decay coordinator |
| stress | mental-health |
| sleep/nutrition | needs |
| age | Plan 176 |

---

# 31. Old-Save Migration

Safe default:

```text
do not retroactively simulate decay from campaign start
```

Migration options:

1. preserve existing skill last-use if already stored;
2. preserve existing dormancy state;
3. initialize missing decay anchors at migration day;
4. initialize event-memory retention to current full/owner state;
5. keep journal/archive untouched;
6. write migration version.

---

# 32. Policy Versioning Contract

Changing decay rates in a future patch can alter active campaigns.

Decide:

### Option A
New policy applies from patch day onward.

### Option B
Campaign snapshots policy version.

Recommended:
- persist policy version/anchor;
- migrate explicitly.

No silent huge retroactive drop.

---

# 33. Exactly-Once Threshold Identity

Stable IDs:

```text
decay:<record_id>:vague
decay:<record_id>:fragmentary
decay:<record_id>:inaccessible
recovery:<record_id>:<band>
```

No duplicates on load.

---

# 34. Failure Injection Matrix

## N185.1 MemoryDecayState stores duplicate skill level
Expected: authority gate fails.

## N185.2 Relationship score decreases daily because memory wasn't reinforced
Expected: relationship-semantics gate fails.

## N185.3 Journal entry loses text because survivor forgot event
Expected: journal/history authority gate fails.

## N185.4 Certified skill still applies two separate decay-protection modifiers
Expected: certification double-count gate fails.

## N185.5 Dormant skill decays through both SkillProgression and MemoryDecay
Expected: skill-authority gate fails.

## N185.6 Event memory at 0 is physically deleted and cannot be resurfaced
Expected: continuity/relearning gate fails.

## N185.7 Old save immediately generates hundreds of fade events
Expected: migration gate fails.

## N185.8 Skill use does not reinforce because practice event missing
Expected: reinforcement reachability fails.

## N185.9 Memory decays every frame
Expected: performance/source-scan gate fails.

## N185.10 Random forgetting uses unseeded RNG
Expected: determinism gate fails.

## N185.11 Older survivor receives universal +100% decay despite no cognition issue
Expected: Plan-176 integration gate fails.

## N185.12 Review of journal magically changes historical record
Expected: external-record gate fails.

---

# 35. Determinism Contract

Same:

```text
campaign day
+ record anchor
+ decay policy version
+ certification
+ skill/knowledge state
+ condition modifiers
+ reinforcement events
```

must produce same:
- retention;
- clarity band;
- threshold events;
- relearning modifier.

Seeded RNG only if a specific recall mechanic intentionally needs uncertainty.

---

# 36. Long-Horizon Metrics

Track:

```text
skills active
skills dormant
skills deeply dormant
knowledge clear/vague/inaccessible
event-memory clarity distribution
relationship-event memories
reinforcements/day
reviews/day
relearning actions
certification protection
threshold events
old-save migration events
state bytes
day-processing time
```

---

# 37. Balance Guardrails

Memory decay should reward:
- natural use;
- teaching;
- review;
- record keeping;
- specialization.

It should not require:
- rotating every survivor through every skill every week;
- manually clicking “review” on dozens of memories.

---

# 38. Skill Decay Guardrails

Commonly used skills:
- self-maintain through work.

Rare emergency skills:
- benefit from drills/certification.

This creates useful gameplay without chores.

---

# 39. Knowledge Decay Guardrails

Important institutional knowledge should have:
- manuals;
- teaching;
- multiple knowledgeable survivors
as preservation strategies.

---

# 40. Event-Memory Guardrails

Most event detail can fade with low gameplay stakes.

Only salient memories should:
- trigger warnings;
- affect dialogue/choices.

---

# 41. Relationship Guardrails

Memory decay must not simulate:
- relationship neglect.

That belongs to relations/social systems if desired.

---

# 42. UI Acceptance

## Survivor skills
- rusty/dormant state;
- practice path.

## Knowledge
- at-risk;
- review source.

## Event memory
- important recall only.

## History
- external record separate.

---

# 43. Accessibility

- no color-only clarity bands;
- no mandatory memory-maintenance micro-grid;
- keyboard navigation;
- readable tooltips;
- clear recovery actions.

---

# 44. Localization

Memory state names:
- localization keys.

Do not put free-form gameplay logic into localized descriptions.

---

# 45. Content Acceptance

Decay policy ladder:

```text
DISCOVERED
LOADED
REGISTERED
DOMAIN_ADAPTER_FOUND
RECORD_REACHABLE
DECAY_EVALUATED
REINFORCEMENT_REACHABLE
CONSEQUENCE_PRODUCED
```

No policy without both:
- decay consumer;
- maintenance/relearning path.

---

# 46. Reachability

For each memory domain:
- deterministic fixture reaches fade;
- deterministic fixture reaches reinforcement;
- deterministic fixture reaches relearning.

No dead decay profile.

---

# 47. Performance Guardrails

- no per-frame processing;
- campaign-day/event updates;
- indexed active decay records;
- lazy formula where possible;
- no full journal/archive scans.

---

# 48. CI / Gate Set

Recommended:

```text
memory_decay_authority_matrix
memory_decay_no_duplicate_skill_state
memory_decay_no_duplicate_relationship_state
memory_decay_no_duplicate_journal_state
memory_decay_skill_dormancy_reconciled
memory_decay_certification_single
memory_decay_reinforcement_reachability
memory_decay_external_record_integrity
memory_decay_threshold_idempotence
memory_decay_old_save
memory_decay_determinism
memory_decay_long_horizon
memory_decay_ui_access
```

---

# 49. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --memory-decay-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 50. Recommended Commit Breakdown

```text
185A-1 memory/skill/knowledge authority audit
185A-2 memory-domain ADR
185A-3 skill dormancy-vs-decay ADR
185A-4 journal-vs-recall ADR
185A-5 decay policy schema/loader
185A-6 domain adapter registry
185A-7 threshold/idempotence contract
185A-8 docs/tests

185B-1 SkillProgression integration
185B-2 14-day dormancy reconciliation
185B-3 practice reinforcement
185B-4 Plan-180 certification integration
185B-5 BunkerSkillDecayStopped reconciliation
185B-6 relearning familiarity
185B-7 no-double-count tests
185B-8 skill decay docs

185C-1 knowledge authority audit
185C-2 knowledge retention state
185C-3 review action
185C-4 teaching reinforcement
185C-5 forgetting/reacquisition
185C-6 critical-knowledge recoverability
185C-7 tests
185C-8 docs

185D-1 PhantomMemory/Plan-147 ownership audit
185D-2 event retention/clarity
185D-3 salience/reminder links
185D-4 trauma/phantom disposition
185D-5 recall access
185D-6 external-history protection
185D-7 tests
185D-8 docs

185E-1 relation-memory separation
185E-2 interaction reinforcement
185E-3 grudge/event-memory adapter
185E-4 deceased-person memories
185E-5 no-bond-decay regression tests
185E-6 docs

185F-1 journal external-record contract
185F-2 journal review action
185F-3 manual/education source integration
185F-4 reminder links
185F-5 preservation tests
185F-6 docs

185G-1 Plan-176 age adapter audit
185G-2 stress/mental-health adapter
185G-3 nutrition/sleep adapter
185G-4 affliction/cognitive adapter
185G-5 modifier clamp/diagnostics
185G-6 anti-stereotype tests
185G-7 docs

185H-1 reinforcement event registry
185H-2 practice/review/teaching
185H-3 reminder/experience
185H-4 diminishing reinforcement
185H-5 recovery thresholds
185H-6 anti-farm tests
185H-7 docs

185I-1 inaccessible state semantics
185I-2 relearning
185I-3 resurfacing
185I-4 community-vs-personal knowledge
185I-5 quest predicate hooks
185I-6 tests/docs

185J-1 UI integration
185J-2 minimal persistence/old-save migration
185J-3 deterministic fingerprint
185J-4 30-day simulation
185J-5 120/180/400-day simulations
185J-6 CI gates/failure fixtures/perf
185J-7 generated reports
185J-8 final ship/no-ship report

185K-1 advanced trait/legacy/trading disposition
```

---

# 51. Risk Register

## R185.1 Creates shadow copies of skills/relationships

Mitigation:
- domain adapters;
- authority matrix;
- source-scan gates.

## R185.2 Memory decay becomes chore system

Mitigation:
- natural-use reinforcement;
- slow curves;
- warning prioritization.

## R185.3 Journal and objective history become unreliable

Mitigation:
- explicit external-record contract.

## R185.4 Skill dormancy double-counts with new decay

Mitigation:
- dedicated ADR and one canonical skill decay path.

## R185.5 Certification protection double-applies

Mitigation:
- Plan-180 owns certification semantics.

## R185.6 Old saves immediately forget everything

Mitigation:
- migration-day anchors;
- no retroactive decay reconstruction without evidence.

## R185.7 Age modifier stereotypes elders

Mitigation:
- Plan-176 bounded cognition adapter only.

## R185.8 Relationship memory turns into relationship decay

Mitigation:
- bond/event-memory separation.

## R185.9 Event logs explode

Mitigation:
- threshold-only semantic logging.

## R185.10 Per-day scanning becomes expensive

Mitigation:
- lazy/indexed evaluation.

---

# 52. Acceptance Checklist

## P0

- [ ] SkillProgressionSystem audited
- [ ] dormancy semantics documented
- [ ] last-use fields audited
- [ ] reactivation audited
- [ ] BunkerSkillDecayStopped audited
- [ ] Plan 180 certification audited
- [ ] PhantomMemoryEngine audited
- [ ] Plan 147 NPC memory audited
- [ ] SurvivorRelationsSystem audited
- [ ] journal/history audited
- [ ] Plan 154 education audited
- [ ] Plan 176 aging/cognition audited
- [ ] stress/mental-health audited
- [ ] needs/nutrition/sleep audited
- [ ] affliction/cognition audited
- [ ] campaign day authority audited
- [ ] save ordering audited
- [ ] memory authority matrix published
- [ ] memory-domain ADR
- [ ] skill dormancy-vs-decay ADR
- [ ] journal-vs-recall ADR
- [ ] baseline persistence captured

## 185A

- [ ] coordinator ownership narrow
- [ ] no universal mirrored memory list
- [ ] versioned decay policy catalog
- [ ] typed policy DTO
- [ ] deterministic curve
- [ ] five source categories mapped honestly
- [ ] procedural memory duplication avoided
- [ ] physical/mental skill tags only if real
- [ ] domain-specific floor semantics
- [ ] zero/forgotten semantics per domain
- [ ] threshold bands data-driven
- [ ] source 50/25/0 treated as tuning
- [ ] no daily decay log spam
- [ ] lazy evaluation considered
- [ ] typed adapter interface
- [ ] no reflection
- [ ] stable record IDs
- [ ] no GUID
- [ ] content not duplicated
- [ ] immutable/read-only projection
- [ ] data integrity

## 185B — Skills

- [ ] canonical skill fields preserved
- [ ] no duplicate skill strength
- [ ] dormancy-vs-proficiency semantic selected
- [ ] 14-day threshold intentionally preserved/re-tuned
- [ ] practice semantic event
- [ ] SkillProgression owns reinforcement
- [ ] Plan-180 certification owns protection
- [ ] BunkerSkillDecayStopped reconciled
- [ ] dormant decay semantics reviewed
- [ ] skill floor
- [ ] canonical relearning
- [ ] familiarity acceleration bounded
- [ ] no free XP
- [ ] UI rusty/dormant
- [ ] hard “cannot use” only if intentional
- [ ] regression tests

## 185C — Knowledge

- [ ] knowledge authority found
- [ ] only explicit gameplay knowledge decays
- [ ] player UI/tutorial knowledge excluded
- [ ] stable knowledge IDs
- [ ] retention field only if needed
- [ ] review action
- [ ] real review source/time
- [ ] teaching integration
- [ ] student learning canonical
- [ ] knowledge floor
- [ ] critical knowledge profiles
- [ ] inaccessible knowledge semantics
- [ ] external manuals remain
- [ ] relearning canonical
- [ ] prior familiarity bounded
- [ ] no direct quest failure
- [ ] tests

## 185D — Event Memory

- [ ] PhantomMemory ownership classified
- [ ] Plan-147 ownership classified
- [ ] one event-memory owner
- [ ] minimal decay fields
- [ ] clarity has real consumers
- [ ] redundant strength/clarity avoided
- [ ] clear/vague/fragmentary/inaccessible semantics
- [ ] historical record preserved
- [ ] reminder uses semantic links
- [ ] similar experience uses tags
- [ ] salience integration
- [ ] trauma memory special handling
- [ ] phantom memory special handling
- [ ] recall check only if real
- [ ] deterministic/seeded recall policy
- [ ] unreliable-journal idea rejected by default
- [ ] tests

## 185E — Relationships

- [ ] relationship bond/history separated
- [ ] bond stays in SurvivorRelations
- [ ] event recollection may fade
- [ ] deep bond not auto-decayed
- [ ] interaction reinforcement semantics
- [ ] strong relationships slow relevant-memory decay only
- [ ] grudge memory adapter explicit
- [ ] reconciliation owned by relations
- [ ] deceased-person memories persist
- [ ] memorial reminder if supported
- [ ] no heartbeat relation decay
- [ ] tests

## 185F — Journal

- [ ] journal owns entries
- [ ] archive owns historical truth
- [ ] writing creates external record
- [ ] writing does not magically freeze personal recall
- [ ] review reinforces
- [ ] source Plan-185 journal preservation semantics corrected
- [ ] entry fidelity based on writing time
- [ ] later-from-memory writing deferred unless real
- [ ] semantic links
- [ ] no passive daily reinforcement
- [ ] manuals/teaching notes if real
- [ ] link integrity
- [ ] memory-detail review action
- [ ] tests

## 185G — Modifiers

- [ ] age uses Plan-176 seam
- [ ] no “elderly forget faster” hardcode
- [ ] bounded age curve if real
- [ ] stress authority reused
- [ ] no copied stress
- [ ] health effects narrow
- [ ] nutrition needs reused
- [ ] sleep/fatigue reused
- [ ] no new sleep system
- [ ] cognitive affliction adapter
- [ ] medication only if real
- [ ] trauma effects nuanced
- [ ] modifier composition bounded
- [ ] no multiplicative explosion
- [ ] diagnostics
- [ ] neutral/actionable UI language
- [ ] anti-stereotype tests

## 185H — Reinforcement

- [ ] source types registered
- [ ] compact provenance
- [ ] practice
- [ ] review
- [ ] teaching
- [ ] reminder
- [ ] related experience
- [ ] conversation if real
- [ ] maximum cap
- [ ] same-day diminishing returns
- [ ] trivial-use exploit prevented
- [ ] practice quality if supported
- [ ] review quality if supported
- [ ] no AFK reinforcement
- [ ] no unnecessary queue
- [ ] consolidation policy explicit
- [ ] recovery threshold events
- [ ] no use-spam journaling
- [ ] diagnostics

## 185I — Forgetting/Relearning

- [ ] common state vocabulary
- [ ] “forgotten” means inaccessible where appropriate
- [ ] canonical records not physically deleted
- [ ] provenance retained
- [ ] skill relearning canonical
- [ ] knowledge relearning canonical
- [ ] event resurfacing
- [ ] relationship recollection
- [ ] bounded prior familiarity
- [ ] prior peak respected
- [ ] rediscovery semantic event
- [ ] no internal quest generation
- [ ] Plan 171 hook only
- [ ] recall predicates
- [ ] map/world knowledge not accidentally deleted
- [ ] community vs personal knowledge audited
- [ ] critical operations recoverable
- [ ] tests

## 185J — UI

- [ ] no “all memories” overload by default
- [ ] at-risk/important filters
- [ ] survivor detail integration
- [ ] dedicated panel justified if created
- [ ] semantic clarity bands
- [ ] exact % optional
- [ ] actionable reinforcement options
- [ ] decay reasons
- [ ] warnings priority-filtered
- [ ] no trivial 50% warning spam
- [ ] forgetting history bounded
- [ ] first meaningful fade tutorial
- [ ] tooltips
- [ ] accessibility
- [ ] text scale
- [ ] neutral language

## 185J — Persistence

- [ ] minimal anchors only
- [ ] no duplicated content strings
- [ ] no duplicate skill level
- [ ] no duplicate relation score
- [ ] bounded forgetting history
- [ ] bounded reinforcement history
- [ ] old save anchored safely
- [ ] no retroactive day-1 decay
- [ ] existing skill last-use preserved
- [ ] event-memory migration safe
- [ ] save/load exact
- [ ] policy version migration

## 185J — Determinism/Balance

- [ ] decay pure
- [ ] reinforcement deterministic
- [ ] threshold deterministic
- [ ] recall RNG seeded only if needed
- [ ] no wall clock
- [ ] stable iteration
- [ ] maintenance not chore-heavy
- [ ] common skills self-maintain
- [ ] emergency skills benefit from drills
- [ ] knowledge review cadence reasonable
- [ ] flavor event memories low-stakes
- [ ] relationship memory slow
- [ ] procedural slowest if distinct
- [ ] relearning faster but costly
- [ ] no total cognitive collapse
- [ ] teaching reinforces
- [ ] routine interaction maintains relevant memory
- [ ] stress/fatigue bounded
- [ ] age bounded
- [ ] efficient play not punished

## 185J — Simulation

- [ ] 30-day normal play
- [ ] 120-day mixed roster
- [ ] 180-day campaign
- [ ] 400-day soak
- [ ] constant reinforcement
- [ ] total neglect
- [ ] certified expert
- [ ] stressed/exhausted survivor
- [ ] journal-heavy survivor
- [ ] old-save migration
- [ ] state size bounded
- [ ] event volume bounded

## 185J — CI

- [ ] policy integrity
- [ ] adapter integrity
- [ ] no duplicate skill state
- [ ] no duplicate relationship state
- [ ] no duplicate journal state
- [ ] skill dormancy reconciled
- [ ] certification single-application gate
- [ ] reinforcement reachability
- [ ] external-record integrity
- [ ] threshold idempotence
- [ ] old-save gate
- [ ] deterministic fingerprint
- [ ] performance benchmark
- [ ] content acceptance
- [ ] state reachability
- [ ] generated docs
- [ ] verify-fast

## 185K

- [ ] photographic-memory trait gated
- [ ] no stigmatizing “forgetful” shortcut
- [ ] memory legacy uses archive/journal
- [ ] memory quests use Plan 171
- [ ] memory flashes use PhantomMemory/mental-health
- [ ] déjà vu narrative follow-on
- [ ] memory trading deferred
- [ ] institutional knowledge uses education/archive
- [ ] sci-fi memory tech out unless fiction supports

---

# 53. Ship / No-Ship Gate

**SHIP** only if:

```text
memory_domain_authorities_documented == true
AND duplicate_skill_level_state_in_memory_decay == 0
AND duplicate_relationship_score_state_in_memory_decay == 0
AND duplicate_journal_or_archive_state_in_memory_decay == 0
AND skill_dormancy_decay_double_application == 0
AND certification_protection_double_application == 0
AND event_forgetting_rewrites_historical_truth == false
AND relationship_memory_decay_mutates_bond_without_relation_rule == false
AND old_save_retroactive_mass_decay == false
AND daily_decay_event_spam == 0
AND per_frame_decay_processing == 0
AND unseeded_forgetting_rng == 0
AND direct_memory_decay_owned_quest_state == false
AND direct_memory_decay_owned_faction_state == false
AND direct_memory_decay_owned_inventory_state == false
AND universal_age_forgetting_penalty_without_plan176_seam == false
AND unreachable_decay_policies == 0
AND decay_policies_without_reinforcement_or_relearning_path == 0
AND memory_decay_old_save == pass
AND memory_decay_save_roundtrip == pass
AND memory_decay_threshold_idempotence == pass
AND memory_decay_skill_integration == pass
AND memory_decay_certification_integration == pass
AND memory_decay_journal_integrity == pass
AND memory_decay_relationship_integrity == pass
AND memory_decay_relearning == pass
AND memory_decay_determinism == pass
AND memory_decay_30_day_balance == pass
AND memory_decay_120_day_balance == pass
AND memory_decay_180_day_balance == pass
AND memory_decay_400_day_long_horizon == pass
AND memory_decay_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 54. Implementer Handoff

1. Start by proving what `SkillProgressionSystem`, `PhantomMemoryEngine`, Plan 147, SurvivorRelations, Journal, Education, and Plan 180 already own.
2. Do not create one giant universal `Memory` list that mirrors all those systems.
3. Build domain decay policies and narrow adapters.
4. Reconcile skill dormancy before adding any deeper skill erosion.
5. Let Plan 180 own certification protection.
6. Treat procedural memory as a skill profile unless a distinct owner already exists.
7. Separate personal recall from objective historical truth.
8. Keep journal/archive entries durable even when survivor recall fades.
9. Make journal **review** reinforce memory; do not make a stored journal entry a passive permanent-memory aura.
10. Separate relationship-event recollection from relationship bond.
11. Use threshold-only semantic events rather than logging daily decay.
12. Prefer deterministic curves; use seeded RNG only for an explicit recall mechanic.
13. Make normal skill use count as practice automatically.
14. Make teaching and review real actions that consume real time/resources where appropriate.
15. Preserve enough provenance for relearning and resurfacing rather than deleting records at zero strength.
16. Let relearning run through the existing skill/education systems.
17. Feed age, stress, nutrition, sleep, and cognitive afflictions through bounded existing-system adapters.
18. Do not hardcode older survivors as universally more forgetful.
19. Migrate old saves safely by anchoring missing decay metadata at migration time unless trustworthy historical usage exists.
20. Do not retroactively simulate months of forgetting on first load.
21. Keep UI focused on important/at-risk skills and knowledge, not every trivial memory.
22. Add 30/120/180/400-day simulations to tune decay so it creates strategy rather than chores.
23. Close only when forgetting is gradual, reversible where appropriate, externally recoverable through records/teaching, and impossible to confuse with loss of objective world truth.

---

# 55. Final Outcome

When this plan is complete, ASHFALL gains cognitive continuity without turning every survivor into a spreadsheet of fading bars.

Skills that are used regularly stay alive because the survivor is actually practicing them. Skills that sit unused for long periods can become rusty or dormant through the same `SkillProgressionSystem` that already owns proficiency. Certification protects expertise through Plan 180 instead of a second protection rule.

Knowledge behaves differently.

A survivor can forget a procedure or factual subject if they never review or use it. Manuals, teachers, education, and journals can help recover that knowledge because they are external sources. Relearning is faster when familiarity remains, but it still requires a real learning action.

Event memories behave differently again.

A survivor may gradually lose exact details of an old encounter while still remembering its emotional gist. A highly salient event can remain vivid longer. Trauma can follow its own psychological rules. A reminder can bring a memory back into focus.

None of that rewrites history.

The archive still knows what happened.
The journal still contains what was written.
The world state still contains the actual consequence.

Personal recall and objective history are separate facts.

Relationships also stay coherent. Forgetting the precise details of an old argument does not automatically erase affection, loyalty, or trust. The relationship system remains responsible for those bonds. Memory decay only affects the survivor's recollection of specific events when that distinction matters.

The system also avoids arbitrary randomness. Most decay is a deterministic function of time since reinforcement, domain policy, salience, certification, and real survivor condition. Stress, fatigue, nutrition, aging, and cognitive illness contribute only through the systems that already represent them.

Most importantly, maintenance happens naturally.

Work reinforces work skills.
Teaching reinforces knowledge.
Reviewing a journal reinforces recollection.
Conversation can refresh shared history.
Training can recover dormant capability.

The player is not asked to click “maintain memory” on fifty entries every week.

The result is a cognition layer where survivors can forget, relearn, misremember, preserve, and pass on knowledge without duplicating ASHFALL's skill, relationship, journal, education, or historical systems.

People no longer remember everything perfectly forever.

But the community can fight forgetting with practice, teaching, records, and institutional memory.
