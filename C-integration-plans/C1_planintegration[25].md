# C1 — Flagship Integration Plan [25]: Working Animals & Companion System

> **Output:** `C1_planintegration[25].md`
>
> **Source baseline:** Plan 151 — Working Animals & Companion System
>
> **Primary mission:** introduce living animal companions as durable, named, cared-for campaign actors that can emerge from existing wildlife/trapping content and contribute to shelter defense, expeditions, morale, pest control, scouting, and carrying through current domain authorities.
>
> **Primary architectural rule:** animals are not items once tamed. They require stable identity, persistent state, one companion authority, and explicit adapters into existing shelter, expedition, combat, morale, inventory, wildlife, and grief systems.
>
> **Primary scope rule:** the first shippable slice is intentionally smaller than the source's full ambition. The flagship should establish 3 core companion archetypes first (dog, cat, pack animal/horse) and only add birds, breeding, bloodlines, racing, messenger networks, veterinary specialization, or radiation mutations after the base system is proven reachable, balanced, deterministic, and maintainable.
>
> **Mandatory execution order:** 151A authority audit + living-animal identity/capture contract → 151B taming/care/bond/training → 151C deployment into expeditions, shelter defense, combat, pest control, morale and grief → 151D persistence, aging/health, balance, UI, reachability and CI → 151E optional advanced features only if existing rails support them.
>
> **Critical re-baseline rule:** before creating `AnimalCompanionSystem`, inspect `WildlifeTrappingSystem`, `WildlifeMigrationSystem`, expedition logistics, shelter defense, tactical combat, inventory/food consumption, morale/mental-health, item catalogs, save orchestration, Plan-24 fitness/needs, Plan-35 goods flow, Plan-41 memory/grief, Plan-52 ambience if animal audio is in scope, and Plan-50 asset-manifest rules. New animal logic must call those authorities rather than reproducing them.
>
> **Guardrails:** no second wildlife ecology; no animal-as-inventory-item after taming; no passive global morale aura without a named consumer; no duplicate expedition carry calculation; no direct combat damage system inside companion code; no separate animal disease simulation unless current medical systems can support it cleanly; no breeding/genetics system in the MVP; no per-frame taming/training; no wall-clock/GUID/unseeded RNG; no unlimited animal population; no invisible food drain; no animal grief duplicated across morale/mental-health systems; no pet-shop economy unless existing trade rails support it; no new panel if an existing shelter/roster surface can host companion management cleanly.

---

# 0. Mission

ASHFALL already has wildlife.

It can:
- spawn/migrate;
- be trapped;
- become meat;
- become hides;
- become a hazard.

What it cannot yet do is preserve an animal as a living relationship and operational asset.

The source baseline describes the current topology:

```text
WILD ANIMAL
   │
   ▼
TRAP
   │
   ▼
RESOURCE PROCESSING
   │
   ├── meat
   └── hides
```

There is no durable branch for:

```text
WILD ANIMAL
   │
   ▼
CAPTURED ALIVE
   │
   ▼
TAMING / CARE
   │
   ▼
NAMED COMPANION
   │
   ├── bond
   ├── health
   ├── training
   ├── assignment
   ├── expedition
   ├── shelter defense
   ├── morale / pest control
   └── death / memory
```

That missing branch matters because animals create a qualitatively different survival loop.

A dog is not merely:
- +20% defense.

A useful implementation must answer:
- where did the animal come from?
- who cares for it?
- what does it eat?
- what can it do?
- what happens when it is injured?
- what happens when its bonded survivor dies?
- what happens when the animal itself dies?
- can the player afford to keep it?
- can the animal be deployed away from the shelter?
- what if the expedition returns without it?
- what if the animal flees?
- what if the shelter is starving?

The target architecture is:

```text
WILDLIFE / TRAPPING
       │
       ▼
LivingAnimalCapture
       │
       ▼
AnimalCompanionSystem
       │
       ├── stable identity
       ├── species definition
       ├── domestication state
       ├── health
       ├── care
       ├── bond
       ├── training
       ├── assignments
       └── lifecycle
       │
       ▼
EXISTING DOMAIN AUTHORITIES
       │
       ├────────► Inventory / food consumption
       ├────────► Duty / shelter jobs
       ├────────► ShelterDefense
       ├────────► Expedition logistics
       ├────────► TacticalCombat
       ├────────► Pest/spoilage systems
       ├────────► Morale / relationships
       ├────────► Journal / memory
       └────────► Save / epilogue
```

The companion system owns **the animal**.

It does not own:
- food inventory;
- shelter morale;
- combat resolution;
- expedition routing;
- raid resolution;
- wildlife ecology;
- grief;
- trade.

---

# 1. Source-Evidence Interpretation

## 1.1 Trapping already proves the animal acquisition seam

`WildlifeTrappingSystem` already catches wildlife.

Therefore the correct first integration is:
- add an **alive-capture outcome** to real trap resolution,
not a separate animal-spawn system.

## 1.2 Wildlife migration already owns wild populations

`WildlifeMigrationSystem` remains authority for wild packs and environmental population.

Animal companions must leave that wild-state ownership cleanly when captured/tamed.

## 1.3 Expeditions already own logistics

The source proposes:
- speed;
- carry;
- detection.

These must enter the existing expedition planning/runtime calculations rather than being applied as detached bonuses.

## 1.4 Shelter defense may or may not already exist

The source references Plan 138.

If shelter defense authority is not live:
- dog guard integration becomes a deferred adapter,
not a reason to create defense logic inside Plan 151.

## 1.5 Animal food should use real inventory

Care costs only matter if:
- feed comes from inventory;
- consumption is visible;
- starvation/neglect is deterministic.

No background abstract "feed points."

## 1.6 Animal grief should reuse human relationship/morale systems

The source proposes:
- owner grief if animal dies;
- animal mourning if owner dies.

These are emotional consequences, not a new mental-health engine.

## 1.7 Breeding is a separate complexity tier

Breeding implies:
- sex;
- fertility;
- lineage;
- offspring identity;
- genetics;
- population growth;
- inheritance.

This should not be included in the first integration unless the current repository already has a generic heredity system that can be reused.

## 1.8 Ten species is likely too much for first ship

The source itself recognizes complexity risk.

A stronger flagship plan starts with:
- dog;
- cat;
- horse/pack animal
as one minimal proving set.

---

# 2. Non-Negotiable Animal Invariants

## INV-151.1 — One companion authority

A tamed animal has one canonical owner system.

## INV-151.2 — Stable animal identity

Each companion has a deterministic persistent ID.

## INV-151.3 — Wild and companion states do not overlap

An animal cannot simultaneously exist in:
- wildlife pack;
- trap output;
- companion roster.

## INV-151.4 — Taming consumes campaign time

No instant domestication unless explicitly authored.

## INV-151.5 — Taming outcomes are deterministic under seed

No wall-clock randomness.

## INV-151.6 — Animal care consumes real resources

Food, medicine, and shelter capacity use existing systems.

## INV-151.7 — No invisible upkeep

Daily care requirements are player-readable.

## INV-151.8 — Bond is not morale

Bond is animal↔survivor relationship state.

## INV-151.9 — Training is task capability, not a second skill system

Training determines which animal tasks are available and how effectively the animal contributes.

## INV-151.10 — Deployment has one owner at a time

An animal cannot simultaneously:
- guard shelter;
- accompany expedition;
- perform pest control.

Unless a task is explicitly passive/compatible.

## INV-151.11 — Expedition stats are calculated by expedition authority

Animal system contributes typed modifiers/capabilities.

## INV-151.12 — Combat is resolved by TacticalCombatSystem

Animal companion code never becomes a combat engine.

## INV-151.13 — Shelter defense is resolved by shelter-defense authority

Dog guard contribution is an input.

## INV-151.14 — Morale effects use existing morale/relationship authority

No animal-owned morale score.

## INV-151.15 — Death is permanent unless an existing revive mechanic exists

No respawn.

## INV-151.16 — Death consequences are exactly-once

No repeated grief or memorial entry on load.

## INV-151.17 — Population is bounded

No unlimited animal accumulation.

## INV-151.18 — Old saves default safely

Existing saves receive no companions.

## INV-151.19 — Optional advanced features do not block MVP

Breeding/bloodlines/messenger/racing/mutations are follow-ons.

## INV-151.20 — Every species has an actual gameplay purpose

No flavor-only species added just to hit a count.

---

# 3. Definition of Done

Plan 151 closes only when:

- wildlife/trapping acquisition seam is audited;
- animal identity/lifecycle is explicit;
- alive capture is distinguishable from butcher-resource outcomes;
- companion state is versioned and persistent;
- 3 core companion archetypes are real and fully integrated;
- taming uses real survivor assignment/time/resources;
- taming progress is deterministic;
- training uses a bounded task model;
- bond responds to real care/events;
- care consumes real food/resources;
- neglect has bounded consequences;
- animals can be assigned to exactly one active deployment;
- expedition modifiers use expedition authority;
- shelter defense modifiers use shelter-defense authority if live;
- pest control uses real spoilage/pest system if live;
- morale contributions use morale authority;
- animal injury/death routes through real combat/medical/lifecycle seams;
- animal death can cause owner grief exactly once;
- owner death can affect bonded animal behavior through existing relationship/morale hooks or remains a documented follow-on;
- old saves load with empty animal state;
- save/load round trip passes;
- no duplicate companions appear on restore;
- no infinite animal population exploit exists;
- no invisible food consumption exists;
- no per-frame taming/training occurs;
- animal state is fully headless;
- UI clearly shows companion, health, care, bond, training, deployment and upkeep;
- animal definitions pass data-integrity validation;
- `--animal-companions-selftest` exists or equivalent;
- 30/120/180-day balance runs show animals remain useful but costly;
- 400-year retention/aging policy does not explode state;
- optional advanced species/features are explicitly dispositioned.

---

# 4. Phase P0 — Authority & Lifecycle Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
WildlifeTrappingSystem outcomes
WildlifeMigrationSystem identity model
item IDs for meat/hide/live capture
trap definitions
survivor assignment/duty APIs
ExpeditionSystem logistics APIs
ShelterDefenseSystem APIs
TacticalCombat ally/actor APIs
inventory consume/delivery APIs
spoilage/pest APIs
morale/grief APIs
save orchestration
asset registry / portraits
journal/event APIs
```

## P0.2 Build animal authority matrix

Create:

`docs/animals/ANIMAL_AUTHORITY_MATRIX.md`

Columns:

```text
fact
authority
read API
write API
persisted?
animal system role
status
```

Rows:
- wild population;
- captured alive;
- companion identity;
- care;
- bond;
- training;
- task assignment;
- feed consumption;
- expedition carry;
- expedition speed;
- scouting;
- shelter defense;
- pest control;
- morale;
- combat injury;
- death;
- grief;
- journal history.

## P0.3 Audit trap capture semantics

Determine whether current trap resolution can output:
- item;
- encounter;
- callback;
- custom result.

Choose smallest extension.

## P0.4 Audit animal-like items

Search `items.json` and wildlife data.

Identify:
- carcasses;
- meat;
- hide;
- feed;
- medicine;
- cage traps.

## P0.5 Audit expedition modifier seams

Need exact answer for:
- team speed;
- cargo;
- encounter detection;
- route visibility.

## P0.6 Audit combat ally seam

Can an allied non-survivor actor join combat?

If no:
- expedition dog combat is deferred.

## P0.7 Audit shelter-defense seam

If no live defense authority:
- dog defense bonus deferred.

## P0.8 Baseline proof

Capture current:
- trapped animal always becomes resource;
- no companion roster;
- no living animal save state.

---

# TASK 151A — Living Animal Capture, Identity & Core State

# 151A.0 Goal

Create the smallest durable model that can represent a living captured/tamed animal.

## 151A.1 New namespace

If needed:

`Assets/Ashfall.Core/Animals/`

## 151A.2 Companion state DTO

Suggested:

```text
AnimalCompanion
  id
  species_id
  name
  life_stage
  health
  domestication_state
  bond_owner_id
  bond_value
  training[]
  active_assignment
  created_day
  age_days
  provenance
```

## 151A.3 Avoid overstuffed DTO

Do not add:
- genetics;
- pedigree;
- mutation;
- pregnancy;
- personality traits
in MVP.

## 151A.4 Stable ID

Derive from authoritative capture event:

```text
capture_event_id + species_id
```

or current campaign object ID service.

No GUID.

## 151A.5 Provenance

Track:

```text
capture location
source trap
capture day
wild source pack if known
```

Useful for story/history.

## 151A.6 Species catalog

Create:

`animal_companions.json`

Versioned.

## 151A.7 MVP species

Start with 3 archetypes:

```text
dog
cat
horse_or_pack_animal
```

Whether horse and pack animal split depends on real logistics.

## 151A.8 Species fields

Suggested:

```text
id
display_key
diet
daily_feed
max_health
taming_duration_days
taming_difficulty
trainable_tasks[]
base_capabilities
lifespan_days
adult_age_days
asset_id
```

## 151A.9 No direct percentage magic in C#

Species values are data.

## 151A.10 Capture outcome

Cage/live trap can return:

```text
LiveAnimalCaptureResult
```

rather than butchered inventory.

## 151A.11 Capture choice

If system supports:
- keep alive;
- process immediately;
- release.

Use existing encounter/choice resolver.

## 151A.12 No duplicate carcass

Keeping alive must not also award meat/hides.

## 151A.13 Release

Returns animal to wild state only if wildlife system supports reintroduction.

Otherwise records release event and removes capture.

## 151A.14 Captured-not-yet-tamed state

Need separate lifecycle:

```text
captured
taming
companion
released
escaped
dead
```

## 151A.15 Captive capacity

If shelter has cage/space resource:
- use it.

If not:
- set bounded capture-slot count in data/config as temporary architecture.

Do not allow infinite untamed animals.

## 151A.16 Capture expiry/escape risk

Only if authored.

No silent disappearance.

## 151A.17 Capture food cost

If live-captive care begins immediately:
- real feed consumption.

## 151A.18 Name

Player can name after taming or during capture according to UI.

Name is presentation state, not ID.

## 151A.19 Name validation

Length/localization-safe.

## 151A.20 Owner/bonded survivor

Optional.

An animal can be shelter-owned before one survivor bond is established.

## 151A.21 Health

MVP numeric or tiered state.

Do not create veterinary disease simulation yet.

## 151A.22 Aging

Use campaign days.

No wall clock.

## 151A.23 Death

Lifecycle transition:
- alive → dead
exactly once.

## 151A.24 CaptureState/RestoreState

Versioned.

## 151A.25 Old save

Empty companion/captive collection.

## 151A.26 Restore ordering

After:
- survivor IDs;
- inventory;
- wildlife;
- expedition
as required.

## 151A.27 Referential integrity

Validate:
- species ID;
- owner survivor ID;
- task IDs;
- asset IDs.

## 151A.28 Headless

No UI dependency.

## 151A.29 Generated species matrix

Create:

`docs/animals/ANIMAL_SPECIES_MATRIX.md`

### 151A DoD

A trapped animal can remain alive, acquire a stable identity, persist through save/load, and transition into or out of companion state without duplicating wildlife or inventory state.

---

# TASK 151B — Taming, Care, Bonding & Training

# 151B.0 Goal

Make companionship something the shelter earns and maintains through real time, labor, and resources.

---

# 151B-T — Taming

## 151B.T1 Taming job

Use existing duty/job/assignment framework if possible.

Do not build standalone per-frame progress.

## 151B.T2 Assigned survivor

Taming requires one assigned survivor if species/rule says so.

## 151B.T3 Survivor capability

Use existing:
- skill;
- fitness;
- needs performance
where relevant.

Do not invent `animal_handling_skill` unless a generic skill/tag already supports it.

## 151B.T4 Duration

Use campaign days/hours.

## 151B.T5 Difficulty

Species data.

## 151B.T6 Progress model

Prefer deterministic progress per day:

```text
base progress
× survivor capability
× care quality
```

## 151B.T7 Taming success

At completion:
- deterministic threshold;
or
- seeded final outcome if authored.

## 151B.T8 Seeded RNG

Use only for final uncertainty if design requires.

Not every tick.

## 151B.T9 Failure outcomes

Possible:
- escaped;
- taming setback;
- aggression event.

No forced "must kill" result unless combat/choice system supports.

## 151B.T10 Escape

Removes captured state.

Optionally feeds wildlife system.

## 151B.T11 Aggression

Use encounter/combat authority if real.

## 151B.T12 Taming interruption

If survivor becomes unavailable:
- pause or reassign;
- no hidden reset.

## 151B.T13 Taming save

Mid-progress round trip.

## 151B.T14 No UI timer drift

Progress from Core state.

---

# 151B-C — Care

## 151B.C1 Daily feed

Consumes real inventory.

## 151B.C2 Feed resource types

Species data may use:
- meat;
- grain;
- scraps;
- generic ration category
only if actual items exist.

## 151B.C3 No abstract feed currency

## 151B.C4 Feeding priority

If insufficient food:
- explicit shortage.

## 151B.C5 Auto-feed vs manual

Prefer shelter policy:
- auto-feed if inventory available;
- player can change policy.

Do not require daily micromanagement click.

## 151B.C6 Neglect

Consequences:
- bond drop;
- health risk;
- performance reduction;
- escape risk
bounded and visible.

## 151B.C7 Grooming

Only include if it maps to:
- care labor;
- health maintenance.

If it is pure busywork, defer.

## 151B.C8 Attention/play

Can be:
- optional bond action;
- low-frequency.

No daily click tax.

## 151B.C9 Care duty

If schedule/duty supports:
- animal care slot.

## 151B.C10 Care workload

Scales with animal count.

## 151B.C11 Animal capacity

Shelter supports max active animals via:
- kennel/stable capacity if real;
- otherwise bounded config.

## 151B.C12 Food shortage priority

Animals cannot silently consume human emergency food without policy.

## 151B.C13 Consumption event

Record:
- animal;
- item;
- quantity;
- day.

Use inventory authority.

## 151B.C14 Starvation

Do not instantly kill.

Use progressive health/bond loss.

---

# 151B-B — Bond

## 151B.B1 Bond is pair state

Animal ↔ survivor.

## 151B.B2 Bond owner

Optional primary bonded survivor.

## 151B.B3 Bond growth sources

- care;
- successful training;
- shared expedition;
- rescue event if real.

## 151B.B4 Bond loss sources

- neglect;
- mistreatment if authored;
- prolonged separation;
- failed command
only where real.

## 151B.B5 No random daily decay by default

Use event/state-derived change.

## 151B.B6 Bond cap

0..100 or project standard.

## 151B.B7 Bond performance

Small bounded modifier.

Training remains main capability.

## 151B.B8 Owner death

Animal receives:
- temporary bond/grief state
only if current morale/behavior model supports.

Otherwise record memory/history and defer mechanics.

## 151B.B9 Owner departure

Rebonding may be possible through care.

## 151B.B10 Rebonding

No permanent dead owner pointer.

## 151B.B11 Multiple survivors

Animal can know multiple carers if architecture later supports.

MVP primary bond only.

---

# 151B-R — Training

## 151B.R1 Training tasks

Start minimal:

```text
guard
carry
scout
pest_control
companionship
```

Only species-supported tasks.

## 151B.R2 No task without consumer

Every task must have a real downstream system.

## 151B.R3 Training level

Can be per-task.

Avoid one universal 0–100 if different tasks train separately.

## 151B.R4 Recommended

```text
task_id
proficiency
unlocked
practice_count / progress
```

## 151B.R5 Training job

Uses schedule/job system.

## 151B.R6 Daily practice

Core day tick / job progress.

## 151B.R7 Use-based improvement

Only if current deployment event can report use.

## 151B.R8 No infinite idle training

Requires:
- time;
- feed;
- caregiver.

## 151B.R9 Multi-task penalty

Optional only if training balance needs.

Do not add complexity early.

## 151B.R10 Task performance curve

Species base capability × training proficiency × bond small modifier.

## 151B.R11 Upper bound

No absurd stacking.

## 151B.R12 Training failure

No random catastrophic failure unless authored.

## 151B.R13 Retraining

Can change assignment/task.

## 151B.R14 Save/load

Per-task progress persists.

### 151B DoD

A companion must be earned through time and care, remain costly enough to matter, and gain operational capabilities through bounded training rather than arbitrary passive bonuses.

---

# TASK 151C — Deployment Into Existing Systems

# 151C.0 Goal

Make trained animals useful by contributing to existing shelter, expedition, combat, pest, and morale systems.

---

# 151C-E — Expedition Deployment

## 151C.E1 Expedition companion slot

Audit current expedition roster/cargo model.

Use one companion list/slot model.

## 151C.E2 Animal cannot be in shelter assignment simultaneously

Assignment authority enforces exclusivity.

## 151C.E3 Dog expedition role

Potential:
- encounter detection;
- hazard warning;
- combat aid if supported.

## 151C.E4 Horse/pack role

Potential:
- carry capacity;
- travel speed.

## 151C.E5 No duplicate carry formula

Animal contribution enters current expedition cargo capacity.

## 151C.E6 No duplicate speed formula

Animal contribution enters current team/route travel-time calculation.

## 151C.E7 Feed on expedition

Pack feed requirement:
- packed as real inventory;
- consumed by expedition day/time.

## 151C.E8 Missing feed

Performance/health consequences explicit.

## 151C.E9 Animal injury

If expedition hazard can target companion:
- canonical animal health mutation.

## 151C.E10 Animal death

Remove from expedition and companion roster on return/current state.

## 151C.E11 Animal lost

If expedition system supports missing/lost state:
- persistent.

Otherwise defer.

## 151C.E12 Dispatch preview

Show:
- extra carry;
- speed change;
- feed cost;
- task role.

## 151C.E13 Actual parity

Preview and actual use same calculation.

## 151C.E14 Headless tests

Healthy/underfed/trained/untrained cases.

---

# 151C-D — Shelter Defense

## 151C.D1 Precondition

Only if shelter-defense authority exists.

## 151C.D2 Guard assignment

Dog assigned to guard.

## 151C.D3 Detection contribution

Feeds:
- raid detection;
- warning time;
or existing defense prep metric.

## 151C.D4 Defense contribution

May feed combat/defense score.

No direct "+20%" hardcoded outside defense system.

## 151C.D5 Injury/death

Defense authority reports animal casualty.

## 151C.D6 Multiple dogs

Diminishing returns/cap.

## 151C.D7 No dog wall exploit

Cannot stack 20 dogs for invulnerability.

---

# 151C-P — Pest Control / Spoilage

## 151C.P1 Precondition

Find real pest/spoilage mechanic.

## 151C.P2 Cat role

If pests exist:
- reduce pest pressure through that authority.

## 151C.P3 If no pest system

Do not fabricate hidden spoilage reduction.

Defer cat pest-control gameplay and retain companionship role.

## 151C.P4 Storage interaction

No direct inventory mutation from cat.

## 151C.P5 Bounded effect

Cannot eliminate all spoilage.

---

# 151C-M — Morale / Companionship

## 151C.M1 Morale authority

Use existing needs/morale/social channel.

## 151C.M2 No permanent shelter aura by default

Companion morale benefit should depend on:
- presence;
- care;
- bond;
- relevant survivor.

## 151C.M3 Cat shelter-wide bonus

Source proposes +10.

Treat as candidate, likely too global.

Prefer:
- bounded per-day/interaction relief;
- owner/nearby survivor effect
if system supports.

## 151C.M4 Dog owner bond

Small morale/stress benefit through existing modifier stack.

## 151C.M5 No stacking exploit

Multiple cats/dogs do not produce infinite morale.

## 151C.M6 Morale ceiling

Clamp through existing system.

---

# 151C-C — Tactical Combat

## 151C.C1 Precondition

Confirm allied animal actor support.

## 151C.C2 If unsupported

Defer combat participation.

Do not redesign TacticalCombatSystem inside Plan 151.

## 151C.C3 If supported

Animal actor:
- canonical combat ID;
- species stats from data;
- companion linkage.

## 151C.C4 Combat AI

Use existing AI archetype.

No animal-specific tactical engine unless current AI is data-driven.

## 151C.C5 Commands

No new command UI in MVP unless allied actors already accept commands.

## 151C.C6 Injury/death

Combat result reports animal consequence.

## 151C.C7 Friendly-fire

Handled by combat authority.

## 151C.C8 Owner rescue events

Narrative event only after real combat evidence.

---

# 151C-G — Grief / Memory

## 151C.G1 Animal death event

Emit exactly once.

## 151C.G2 Owner grief

Use existing:
- morale;
- mental health;
- pair-history/memory
through adapter.

## 151C.G3 Shelter grief

Only if animal had broad relationship.

No universal large penalty.

## 151C.G4 Memorial

If Plan 41 memorial system supports non-human memorial entries:
- integrate.

Otherwise journal only.

## 151C.G5 Keepsake

Collar/tag/object only if item/memorial system supports.

## 151C.G6 Animal mourning owner

Use bond history to create:
- temporary behavioral state
only if animal behavior layer supports.

Otherwise journal/narrative.

---

# 151C-W — Wildlife Integration

## 151C.W1 Capture removes animal from wild state

Exactly once.

## 151C.W2 Escape/release

Returns to wildlife only through existing API.

## 151C.W3 No companion duplication from migration

Stable IDs.

## 151C.W4 Species availability

Taming candidates must originate from wildlife that actually exists in region.

### 151C DoD

Animals contribute to expeditions, shelter defense, pest control, morale, combat, and grief only through real domain authorities, with explicit deferral wherever a consumer is not yet live.

---

# TASK 151D — Health, Aging, UI, Persistence, Balance & CI

# 151D.0 Goal

Make companion state durable, readable, bounded, and survivable over long campaigns.

## 151D.1 Health model

MVP:
- health;
- injured state;
- starving/neglected penalties.

No separate animal disease catalog yet.

## 151D.2 Medical treatment

If human medical inventory can logically apply:
- use generic treatment item/effect.

Do not create veterinary medicine subsystem yet.

## 151D.3 Veterinary care follow-on

Document.

## 151D.4 Aging

Use campaign days.

## 151D.5 Lifespan

Species data.

## 151D.6 Life stages

Optional:

```text
young
adult
old
```

Only if gameplay effect exists.

## 151D.7 Natural death

Deterministic by age/health rules.

Do not use hidden random instant death.

## 151D.8 Age-based decline

If used:
- gradual;
- bounded.

## 151D.9 Old animal

May have:
- reduced task performance;
- higher care.

No sudden uselessness.

## 151D.10 UI surface decision

Audit:
- roster;
- shelter;
- companion panel route budget.

Prefer:
- `Animal` tab/detail inside shelter roster
if enough.

New panel only if necessary.

## 151D.11 UI summary

Show:
- name;
- species;
- health;
- bond;
- current owner;
- training;
- assignment;
- daily feed need.

## 151D.12 Taming UI

Show:
- progress;
- assigned survivor;
- estimated days;
- food/care.

## 151D.13 Training UI

Show:
- task;
- proficiency;
- expected effect.

## 151D.14 Deployment UI

Show:
- active task;
- conflicts.

## 151D.15 Expedition UI

Show animal contribution and feed.

## 151D.16 Warning UI

- low feed;
- poor health;
- low bond;
- assignment conflict.

## 151D.17 Accessibility

No color-only health/bond.

## 151D.18 Text scale

Readable.

## 151D.19 Asset manifest

Animal portraits/icons resolve through Plan 50A manifest.

## 151D.20 Audio

Only if Plan 52 supports:
- dog bark;
- cat vocalization;
- hoofstep
with density budget.

Not core acceptance.

## 151D.21 Journal

Significant events:
- tamed;
- named;
- trained first task;
- expedition lost;
- saved owner;
- died.

No daily feed spam.

## 151D.22 Tutorial

Only if tutorial framework exists.

## 151D.23 Old save

Empty animal state.

## 151D.24 Save round-trip matrix

Cases:
1. no animals;
2. captured live;
3. mid-taming;
4. tamed dog;
5. trained cat;
6. expedition horse;
7. injured animal;
8. low bond;
9. dead animal history;
10. many animals.

## 151D.25 Idempotence

Reload cannot:
- duplicate companion;
- duplicate feed;
- duplicate training progress;
- duplicate grief;
- duplicate journal event.

## 151D.26 Determinism

Same state/seed:
- same taming outcome;
- same training progress;
- same care consequences.

## 151D.27 No frame-time

All progression campaign-time.

## 151D.28 Headless

All logic works.

## 151D.29 Data integrity

Validate:
- species IDs;
- task IDs;
- feed items;
- asset IDs;
- min/max values;
- lifecycle enums.

## 151D.30 Selftest

Create:

```text
--animal-companions-selftest
```

## 151D.31 Selftest cases

At least:
- live capture;
- process/release choice;
- taming;
- taming failure;
- training;
- feed;
- neglect;
- assignment exclusivity;
- expedition modifier;
- defense modifier if live;
- morale;
- death/grief;
- old save;
- save/load.

## 151D.32 Population cap test

Attempt many animals.

System remains bounded.

## 151D.33 Food-economy balance

Track:
- animal daily consumption;
- human ration competition;
- companion benefit.

## 151D.34 30-day scenario

One dog + one cat.

## 151D.35 120-day scenario

Several companions.

Track:
- food cost;
- task value;
- deaths;
- bond;
- expedition utility.

## 151D.36 180-day stress

Harsh food shortage.

Assert:
- player can release/reassign animals;
- no unavoidable shelter collapse.

## 151D.37 Animal-hoarding exploit

No unlimited companion stacking.

## 151D.38 Defense exploit

Guard animals diminishing returns.

## 151D.39 Carry exploit

Pack animal capacity bounded by:
- feed;
- expedition slots;
- route/terrain if current system supports.

## 151D.40 Morale exploit

Companions do not max morale passively.

## 151D.41 Taming farming

Live traps cannot produce infinite companions without ecological/capacity cost.

## 151D.42 Performance

Day tick:
- O(active animals).

No full wildlife scan.

## 151D.43 Retention

Dead/released animals:
- compact history;
- not full active state.

## 151D.44 400-year soak

State remains bounded.

## 151D.45 Generated docs

Create:
- `ANIMAL_COMPANION_ARCHITECTURE.md`;
- `ANIMAL_SPECIES_MATRIX.md`;
- `ANIMAL_TASK_MATRIX.md`;
- `ANIMAL_SCOPE_DISPOSITION.md`;
- `ANIMAL_BALANCE_REPORT.md`.

### 151D DoD

Companions remain understandable, costly, deterministic, persistent, bounded, and compatible with long campaigns and existing saves.

---

# TASK 151E — Advanced Features: Explicitly Gated Follow-On

# 151E.0 Goal

Prevent advanced companion ideas from destabilizing the MVP.

## 151E.1 Birds

Only ship if real consumers exist for:
- scouting;
- message carrying.

If message system absent:
- scout-only or defer.

## 151E.2 Breeding

Default:
- DEFER.

Requires separate design for:
- sex;
- fertility;
- pregnancy;
- offspring identity;
- capacity;
- population control.

## 151E.3 Bloodlines/genetics

Default:
- DEFER.

## 151E.4 Offspring training inheritance

Default:
- DEFER.

## 151E.5 Racing

Default:
- DEFER to morale/event content.

## 151E.6 Animal trading

Only if trade system supports companion entities.

Do not serialize animals as ordinary item stacks.

## 151E.7 Veterinary specialization

Only if survivor skill system can support one more domain.

## 151E.8 Radiation mutations

Default:
- REJECT from Plan 151 core.

Would require:
- mutation authority;
- balance;
- content;
- ethical/tone review.

## 151E.9 Famous-animal epilogue

Good follow-on if epilogue accepts named non-survivor history.

## 151E.10 Animal legacy/New Game+

Separate future plan.

### 151E DoD

Advanced systems are dispositioned, not accidentally half-implemented.

---

# 5. Animal Lifecycle Model

```text
WILD
  │
  ▼
CAPTURED_ALIVE
  ├── processed
  ├── released
  ├── escaped
  └── taming
       │
       ▼
   COMPANION
       │
       ├── training
       ├── assignment
       ├── expedition
       ├── injured
       ├── aging
       ├── released/retired
       └── dead
```

One state at a time.

---

# 6. Species Contract

Each species definition must answer:

```text
what does it eat?
how much?
how hard to tame?
what tasks can it learn?
what is its base capability?
how long does it live?
what shelter capacity does it require?
what assets identify it?
```

No species without an operational reason.

---

# 7. Assignment Contract

Active assignment enum/tags:

```text
none
taming
training
guard
expedition
pest_control
companionship
```

Only one exclusive assignment unless a task is explicitly passive.

---

# 8. Taming Contract

Taming uses:

```text
time
+ caregiver capability
+ animal difficulty
+ care quality
+ seeded uncertainty if authored
```

No click-spam.

---

# 9. Care Contract

Daily care may consume:

```text
feed
water if modeled
care labor
medical item if injured
```

No hidden costs.

---

# 10. Bond Contract

Bond:
- affects loyalty/performance slightly;
- drives emotional history.

Training:
- remains primary task effectiveness.

This prevents petting from replacing training.

---

# 11. Training Contract

Per-task proficiency preferred.

Example:

```text
guard 0.75
carry 0.20
```

rather than a universal training score.

---

# 12. Expedition Contract

Animal system supplies modifiers/capabilities.

Expedition owns:
- route;
- duration;
- cargo;
- encounter;
- consumption.

---

# 13. Shelter Defense Contract

If defense system exists:

```text
trained guard dog
→ defense input
→ defense system
→ outcome
```

No direct raid resolution inside companion system.

---

# 14. Pest Control Contract

If pest/spoilage authority exists:

```text
cat assignment
→ pest pressure modifier
→ storage/spoilage authority
```

No direct food creation.

---

# 15. Morale Contract

Companion effects:
- bounded;
- state-driven;
- use existing morale modifier stack.

No shelter-wide permanent additive aura by default.

---

# 16. Combat Contract

If tactical combat supports allied animal actors:

```text
companion deployment
→ combat actor adapter
→ TacticalCombatSystem
→ result
→ animal health/death
```

Otherwise defer.

---

# 17. Grief Contract

Animal death:

```text
death event
→ bonded survivor relationship/morale
→ journal/memory
```

Exactly once.

---

# 18. Wildlife Contract

Wildlife owns:
- populations;
- migration;
- encounter availability.

Companion owns:
- captured/tamed individual.

Transition is explicit.

---

# 19. Inventory Contract

Feed and medicine:
- real items;
- consumed exactly once.

Companion entity:
- never an item stack after taming.

---

# 20. Persistence Matrix

| Fact | Owner |
|---|---|
| wild population | wildlife |
| captured animal | animal companion system |
| species | data catalog |
| companion identity | animal companion system |
| taming progress | animal companion system |
| training | animal companion system |
| bond | animal companion system |
| feed inventory | inventory |
| expedition state | expedition |
| combat state | combat |
| defense state | shelter defense |
| morale | morale/needs |
| grief | social/mental health |
| journal history | journal |

---

# 21. Old-Save Migration

Default:

```text
animal_state:
  schema_version: 1
  captured: []
  companions: []
```

No retroactive pets generated.

---

# 22. Failure Injection Matrix

## N151.1 Live-captured animal also yields meat/hide
Expected: exclusivity test fails.

## N151.2 Same capture event creates two companions
Expected: stable-ID/idempotence fails.

## N151.3 Taming progresses every frame
Expected: time-authority gate fails.

## N151.4 Animal eats food without inventory transaction
Expected: consumption authority fails.

## N151.5 Expedition horse directly changes route time outside expedition authority
Expected: architecture gate fails.

## N151.6 Dog applies raid defense directly
Expected: defense-authority gate fails.

## N151.7 Cat creates food instead of modifying pest pressure
Expected: pest authority fail.

## N151.8 Animal death applies owner grief twice after reload
Expected: idempotence fail.

## N151.9 Twenty dogs stack unlimited defense
Expected: anti-hoarding/defense balance fails.

## N151.10 Horse carry allows infinite loot with no feed/capacity
Expected: expedition balance fails.

## N151.11 Companion stored as ordinary inventory stack
Expected: architecture fail.

## N151.12 Old save crashes due missing animal section
Expected: migration fail.

---

# 23. Determinism Contract

Same:

```text
capture event
+ species data
+ caregiver state
+ campaign day
+ care actions
+ seeded taming RNG
```

must yield same:
- taming progress;
- final taming outcome;
- training progress;
- bond changes;
- health consequences;
- assignment state.

---

# 24. Long-Horizon Metrics

Track:

```text
live captures
taming attempts
taming success/failure
active companions
animals released
animal deaths
daily feed consumption
training proficiency
assignment utilization
expedition benefit
defense benefit
pest-control benefit
morale contribution
owner grief events
state bytes
```

---

# 25. Balance Guardrails

Animals should be:

```text
useful
costly
limited
emotionally meaningful
```

Not:

```text
free buffs
mandatory optimization
infinite population
micromanagement tax
```

---

# 26. Population Guardrails

Use one or more:
- shelter capacity;
- stable/kennel slots;
- food cost;
- assignment/care labor;
- ecological capture rarity.

Do not rely on one arbitrary hard cap if real constraints exist.

---

# 27. Taming Difficulty Guardrails

Early species:
- accessible but not guaranteed.

Rare/high-value species:
- later;
- longer;
- higher cost.

---

# 28. UI Acceptance

## Companion summary
- name;
- species;
- health;
- bond;
- feed;
- task;
- assignment.

## Capture/taming
- current progress;
- caregiver;
- expected duration.

## Training
- per-task proficiency.

## Expedition
- contribution + feed cost.

## Alerts
- hungry;
- injured;
- low bond;
- conflict.

---

# 29. Accessibility

- text + icon;
- no color-only health/bond;
- keyboard management;
- readable tooltips;
- no mandatory drag-only assignment.

---

# 30. Asset / Audio Contract

Visual assets:
- manifest IDs.

Audio:
- Plan-52 density/asset pipeline.

No hardcoded paths.

---

# 31. Content Acceptance

Species/task data:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Every shipped species must reach real effect.

---

# 32. Reachability

For each species:

```text
can appear in wildlife?
can be trapped alive?
can be tamed?
can learn at least one real task?
can player deploy task?
```

Unreachable:
- fix or defer.

---

# 33. Performance Guardrails

Animal day processing:
- O(active captives + companions).

No:
- full wildlife scan;
- full item scan;
- per-frame logic.

---

# 34. CI / Gate Set

Recommended:

```text
animal_species_integrity
animal_capture_exclusivity
animal_identity_idempotence
animal_taming_determinism
animal_training_progress
animal_feed_consumption
animal_assignment_exclusivity
animal_expedition_parity
animal_defense_adapter
animal_grief_idempotence
animal_population_budget
animal_save_matrix
animal_long_horizon
animal_ui_access
```

---

# 35. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --animal-companions-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 36. Recommended Commit Breakdown

```text
151A-1 authority/wildlife/trap audit
151A-2 species schema + companion DTO
151A-3 stable IDs/lifecycle
151A-4 alive-capture branch
151A-5 save/restore
151A-6 capacity/provenance
151A-7 integrity/headless
151A-8 architecture docs

151B-1 taming job
151B-2 taming outcome/seed
151B-3 feed/care policy
151B-4 bond
151B-5 task training
151B-6 assignment model
151B-7 mid-progress save/recovery
151B-8 tests/docs

151C-1 expedition deployment
151C-2 cargo/speed parity
151C-3 shelter-defense adapter if live
151C-4 pest-control adapter if live
151C-5 morale adapter
151C-6 tactical-combat adapter if live
151C-7 grief/memory integration
151C-8 cross-system tests

151D-1 health/aging
151D-2 roster/detail UI
151D-3 taming/training UI
151D-4 old-save/idempotence
151D-5 30-day balance
151D-6 120/180-day economy stress
151D-7 400-year retention
151D-8 CI gates/failure fixtures

151E-1 advanced feature disposition
151E-2 birds only if consumers
151E-3 breeding defer/design
151E-4 trading/vet/epilogue follow-ons
```

---

# 37. Risk Register

## R151.1 Scope explodes into animal-life simulator

Mitigation:
- 3-species MVP;
- advanced feature gate.

## R151.2 Animals become free buffs

Mitigation:
- feed;
- care labor;
- capacity;
- bounded task bonuses.

## R151.3 Animals become micromanagement burden

Mitigation:
- auto-feed policy;
- low-frequency care;
- task assignment, not daily clicks.

## R151.4 Expedition math duplicates

Mitigation:
- typed contribution only;
- preview/runtime parity.

## R151.5 Combat integration becomes huge

Mitigation:
- adapter only if allied actor seam exists.

## R151.6 Grief double-applies

Mitigation:
- one death event;
- source ID;
- idempotence.

## R151.7 Animal population explodes

Mitigation:
- no breeding in MVP;
- capacity + food + rarity.

## R151.8 Old saves break

Mitigation:
- empty default section.

---

# 38. Acceptance Checklist

## P0

- [ ] WildlifeTrappingSystem audited
- [ ] WildlifeMigrationSystem audited
- [ ] live-capture seam identified
- [ ] items/feed/cage-trap IDs audited
- [ ] duty/job APIs audited
- [ ] ExpeditionSystem logistics audited
- [ ] ShelterDefenseSystem audited
- [ ] TacticalCombat allied-actor seam audited
- [ ] inventory consumption audited
- [ ] pest/spoilage authority audited
- [ ] morale/grief authority audited
- [ ] save ordering audited
- [ ] asset manifest rules audited
- [ ] baseline no-companion behavior reproduced
- [ ] animal authority matrix published

## 151A

- [ ] companion namespace only if needed
- [ ] compact companion DTO
- [ ] no genetics/breeding fields
- [ ] stable deterministic ID
- [ ] provenance
- [ ] versioned species catalog
- [ ] 3-species MVP
- [ ] species fields data-driven
- [ ] no hardcoded bonus magic
- [ ] live-capture result
- [ ] keep/process/release choice if supported
- [ ] no duplicate meat/hide
- [ ] captured lifecycle
- [ ] bounded captive capacity
- [ ] live-captive feed if required
- [ ] naming separate from ID
- [ ] optional bond owner
- [ ] health
- [ ] campaign-day aging
- [ ] exactly-once death
- [ ] CaptureState
- [ ] RestoreState
- [ ] old-save default
- [ ] restore ordering
- [ ] reference integrity
- [ ] headless
- [ ] species matrix

## 151B — Taming

- [ ] taming uses existing job/duty framework
- [ ] assigned caregiver
- [ ] caregiver capability grounded
- [ ] campaign-time duration
- [ ] data-backed difficulty
- [ ] deterministic progress
- [ ] seeded final uncertainty only if needed
- [ ] escape/setback/aggression outcomes bounded
- [ ] no forced kill unless encounter supports
- [ ] interruption pause/reassign
- [ ] mid-taming save
- [ ] UI reads Core progress

## 151B — Care

- [ ] daily feed real inventory
- [ ] feed items real
- [ ] no abstract feed currency
- [ ] shortage visible
- [ ] auto-feed policy
- [ ] neglect bounded
- [ ] grooming only if meaningful
- [ ] attention low-frequency
- [ ] care duty if framework supports
- [ ] workload scales
- [ ] animal capacity
- [ ] human-food priority policy
- [ ] exactly-once consumption event
- [ ] progressive starvation

## 151B — Bond

- [ ] bond pair state
- [ ] primary owner optional
- [ ] real bond growth events
- [ ] bounded bond loss
- [ ] no arbitrary daily decay
- [ ] bond modifier small
- [ ] owner death behavior disposition
- [ ] rebonding possible if owner leaves/dies
- [ ] no multi-owner complexity in MVP

## 151B — Training

- [ ] minimal real task list
- [ ] no task without consumer
- [ ] per-task proficiency
- [ ] training job
- [ ] campaign-time practice
- [ ] use-based progression only if event exists
- [ ] no idle infinite training
- [ ] task-performance curve
- [ ] upper bounds
- [ ] save/load proficiency
- [ ] no catastrophic random failure

## 151C — Expedition

- [ ] companion roster/slot integration
- [ ] assignment exclusivity
- [ ] dog role
- [ ] horse/pack role
- [ ] no duplicate carry formula
- [ ] no duplicate speed formula
- [ ] expedition feed packed/consumed
- [ ] missing-feed consequence
- [ ] injury path
- [ ] death path
- [ ] dispatch preview
- [ ] runtime parity
- [ ] headless tests

## 151C — Shelter defense

- [ ] only if defense authority live
- [ ] guard assignment
- [ ] detection contribution
- [ ] defense contribution via authority
- [ ] casualty path
- [ ] diminishing returns
- [ ] no infinite-dog exploit

## 151C — Pest / Morale / Combat / Grief

- [ ] pest authority audited
- [ ] cat pest control only through real system
- [ ] no direct food creation
- [ ] morale authority reused
- [ ] no permanent unbounded morale aura
- [ ] companion stacking bounded
- [ ] combat adapter only if allied actor seam exists
- [ ] no animal combat engine
- [ ] owner grief exactly once
- [ ] memorial integration only if supported
- [ ] wildlife transition exact
- [ ] no migration duplication

## 151D

- [ ] MVP health model
- [ ] no vet disease system
- [ ] campaign-day aging
- [ ] lifespan data
- [ ] age-stage gameplay only if real
- [ ] deterministic natural death
- [ ] gradual age decline
- [ ] UI route decision
- [ ] companion summary
- [ ] taming UI
- [ ] training UI
- [ ] deployment UI
- [ ] expedition contribution UI
- [ ] warning UI
- [ ] accessibility
- [ ] text scale
- [ ] asset manifest
- [ ] audio optional via Plan 52
- [ ] significant-event journal
- [ ] tutorial only if framework exists
- [ ] old-save empty state
- [ ] 10-case save matrix
- [ ] no duplicate feed/training/grief
- [ ] determinism
- [ ] no frame-time
- [ ] headless
- [ ] data integrity
- [ ] animal-companions selftest
- [ ] population cap
- [ ] food economy balance
- [ ] 30-day scenario
- [ ] 120-day scenario
- [ ] 180-day stress
- [ ] anti-hoarding
- [ ] defense exploit
- [ ] carry exploit
- [ ] morale exploit
- [ ] taming farming check
- [ ] O(active animals) performance
- [ ] dead/released retention
- [ ] 400-year soak
- [ ] generated docs

## 151E

- [ ] birds gated on real consumers
- [ ] breeding deferred by default
- [ ] genetics/bloodlines deferred
- [ ] racing deferred
- [ ] animal trading only if entity trade supported
- [ ] vet specialization follow-on
- [ ] radiation mutation rejected from core
- [ ] epilogue follow-on documented

---

# 39. Ship / No-Ship Gate

**SHIP** only if:

```text
animal_companion_authorities == 1
AND duplicate_wild_companion_identity == 0
AND live_capture_also_awards_carcass_resources == false
AND stable_animal_ids == true
AND taming_uses_campaign_time == true
AND unseeded_taming_rng == 0
AND animal_feed_without_inventory_transaction == 0
AND companion_task_without_real_consumer == 0
AND duplicate_expedition_carry_formula == 0
AND duplicate_expedition_speed_formula == 0
AND companion_owned_combat_engine == false
AND companion_owned_shelter_defense_engine == false
AND companion_owned_morale_state == false
AND duplicate_animal_grief_application == 0
AND unlimited_animal_population == false
AND old_save_animal_state == pass
AND animal_save_roundtrip == pass
AND animal_taming_determinism == pass
AND animal_assignment_exclusivity == pass
AND animal_expedition_parity == pass
AND animal_population_budget == pass
AND animal_balance_30_day == pass
AND animal_balance_120_day == pass
AND animal_balance_180_day == pass
AND animal_retention_400_year == pass
AND animal_companions_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 40. Implementer Handoff

1. Audit the live-capture seam before creating any companion class.
2. Keep wildlife populations and named companions as distinct authorities.
3. Give every captured/tamed animal a stable deterministic identity.
4. Start with dog, cat, and one horse/pack archetype.
5. Add no species whose task has no real consumer.
6. Use the duty/job framework for taming and training.
7. Use campaign time, not frame time.
8. Feed animals from real inventory with visible policy.
9. Keep care low-micromanagement: auto-feed plus meaningful exceptions.
10. Make bond an emotional/performance modifier, not the main training system.
11. Use per-task proficiency rather than a vague universal training level if practical.
12. Enforce one active assignment at a time.
13. Feed expedition carry/speed/scouting through the expedition authority.
14. Feed guard dogs through shelter defense only if that system is live.
15. Feed cat pest control through spoilage/pest authority only if it exists.
16. Feed morale through the existing morale stack with bounded contributions.
17. Integrate tactical combat only if allied animal actors already have a clean seam.
18. Make animal death and owner grief exactly-once.
19. Persist companion state, not duplicated domain state.
20. Add old-save and mid-taming/mid-expedition fixtures.
21. Run food-economy and anti-hoarding simulations before tuning benefits upward.
22. Keep breeding, bloodlines, racing, messenger networks, veterinary specialization, trading, and mutations out of the MVP unless their owning rails already exist.
23. Close only when companions feel valuable **because they cost time, food, attention, and risk**, not because they are free permanent buffs.

---

# 41. Final Outcome

When this plan is complete, wildlife stops having only two destinies:

threat or resource.

A live animal can be captured, cared for, tamed, named, trained, deployed, injured, lost, or remembered.

A dog can become useful because it was actually trained for guard or expedition work, not because owning a dog silently adds a global percentage. A horse or pack animal can improve carrying or travel because the expedition system consumes a real companion capability while also consuming the animal's feed. A cat can matter through companionship or pest control only if the relevant morale/storage systems actually exist to receive that contribution.

The player must pay for that value.

Animals eat. They occupy capacity. They require care and training time. Taking one on an expedition means it is not available at the shelter. Underfeeding or neglect has visible consequences. Hoarding a dozen animals is not free optimization.

The emotional layer follows the same discipline. Bond grows from care and shared events. An animal can survive its owner, be rehomed, or remain part of the shelter's history. If a companion dies, grief flows through the same systems that already remember human loss, exactly once.

The companion system therefore does not become a parallel simulation of everything animals touch.

It owns the living animal.

The rest of ASHFALL reacts through the authorities that already own food, travel, defense, combat, morale, memory, and history.

The result is a new survival relationship with real cost, real utility, and real emotional consequence.
