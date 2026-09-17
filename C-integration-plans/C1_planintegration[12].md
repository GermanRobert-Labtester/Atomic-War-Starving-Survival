# C1 — Flagship Integration Plan [12]: Memory That Acts — Memorials, Heirlooms, Place Memory & Generational Continuity

> **Output:** `C1_planintegration[12].md`
>
> **Source baseline:** Plan 41 — Memory That Acts: Heirlooms, Eulogies, and What the Dead Leave Behind
>
> **Wave:** Continuity Wave 6 — *The People In It*
>
> **Depends on:** Plan 40A identity enrichment; Plan 36A port-contract enforcement; Plan 24C survivor journey; Plan 24B needs/modifier stack; Plan 18A echoes chain; Plan 38A calendar; Plan 35B storage/rations; Plan 19A derived ending.
>
> **Mandatory execution order:** 36A → 40A → 41A → 41B → 41C.
>
> **Primary rule:** death, place history, and generational time must all leave state the player can observe, revisit, and mechanically feel. Memory is not flavor text unless it changes or reveals something.
>
> **Guardrails:** no new memorial subsystem; no new grief stat; no procedural survivor-name generation; no romance/family content design here; no memorial-only UI detached from existing authorities; no death without a surfaced witness; no duplicated keepsake creation on reload; no hidden place-memory omniscience; no cohort maturation driven by raw day arithmetic outside the calendar.

---

# 0. Mission

ASHFALL already owns nearly every ingredient needed for a strong memory layer:

- a procedural eulogy engine;
- a 30-entry heirloom registry;
- memorial state;
- epitaph catalogs;
- wall-carving templates;
- confession secrets;
- echoes;
- phantom-memory triggers;
- survivor relations;
- trauma bonds;
- grief sinks;
- shelter decor;
- standing records;
- location memory;
- world evolution;
- child cohort state;
- generational succession;
- final wishes;
- ending inputs for children surviving;
- journal and briefing infrastructure.

The continuity failure is not missing content. It is that these pieces do not form one causal chain.

The target shape is:

```text
SURVIVOR DEATH
      │
      ▼
SurvivorFate / DeathQuality
      │
      ▼
Memorial Pipeline
      │
      ├── eulogy
      ├── epitaph
      ├── keepsake disposition
      ├── grief applied
      ├── carving/funeral choice
      ├── morale / fatigue / friction change
      ├── phantom-memory eligibility
      ├── journal / briefing
      └── standing-record update
```

Places should also remember:

```text
world event / player action
      │
      ▼
LocationMemorySystem
      │
      ├── death
      ├── discovery
      ├── collapse
      ├── carving
      ├── battle / breach
      └── institution record
      │
      ▼
map detail / archive / standing record / future reveal
```

And time should remember:

```text
birth / cohort record
      │
      ▼
calendar-driven maturation
      │
      ▼
child → adolescent → adult → elder
      │
      ├── ration share
      ├── apprenticeship
      ├── duty eligibility
      ├── dose susceptibility
      ├── succession
      └── ending state
```

This plan is not about adding sentiment. It is about making loss, remembrance, and continuity executable state.

---

# 1. Source-Evidence Interpretation

## 1.1 Eulogy engine: existing intent, zero live path

The source reports `ProceduralEulogyEngine` has no runtime references. 41A wires it rather than replacing it.

## 1.2 Heirlooms and carvings: authored but disconnected

The keepsake registry and wall-carving templates already exist. The missing work is ownership, delivery, UI and persistence.

## 1.3 Phantom memory: live system, missing input

`PhantomMemoryHostSession` is already live. The missing `phantom_background` input should come from Plan 40A. No replacement subsystem is justified.

## 1.4 Grief: Core behavior without gameplay caller

`ApplyGrief` is reportedly test-only. This plan creates the host/runtime path and routes consequences into the existing needs/insomnia system.

## 1.5 Location memory: partly live, weakly surfaced

`LocationMemorySystem` already exists. 41B increases visibility, discoverability, archive interaction, and persistence discipline rather than creating another world-history store.

## 1.6 Cohort maturation: API exists, caller missing

`TryMaturation(childId, day)` reportedly has no production caller. 41C attaches it to the real day loop and Plan 38 calendar.

---

# 2. Non-Negotiable Memory Invariants

## INV-41.1 — One memorial pipeline

Death consequences pass through one orchestrated path. Panels do not independently create memorial state, eulogies, keepsakes or grief.

## INV-41.2 — Existing death authority remains canonical

The memorial pipeline consumes `SurvivorFate` / canonical death facts. It never becomes a second death authority.

## INV-41.3 — Eulogy output is deterministic

Persist the state/template identity needed to reproduce an eulogy. Do not reroll prose on reload.

## INV-41.4 — Keepsakes have unique ownership

Each keepsake has one original owner and one current disposition. Reloading or rebinding cannot create a second copy.

## INV-41.5 — Grief uses the existing needs stack

No new grief stat. Grief modifies fatigue/morale/insomnia through sanctioned existing mechanisms.

## INV-41.6 — Grief is attributable

Every grief contribution names the deceased/source and remains inspectable in needs attribution.

## INV-41.7 — Place memory is local knowledge

The world may remember something before the player knows it. Visibility arrives through a believable information channel.

## INV-41.8 — Place records reflect real events

No generated history without an actual simulation event or authored preexisting world record.

## INV-41.9 — Cohort maturation is calendar-driven

Age transitions use Plan 38's canonical calendar. No per-system age arithmetic.

## INV-41.10 — Age classes have real mechanics

If age classes exist, they affect rationing, apprenticeship/duty and other supported systems.

## INV-41.11 — Succession reflects turnover

Generation/chapter changes must come from actual population transitions, not UI/page counters.

## INV-41.12 — Ending reads cohort state

`childrenSurvived` and related facts are derived from the cohort authority.

---

# 3. Definition of Done

Plan 41 is complete only when all of the following are true:

- a memorial-pipeline authority diagram exists;
- every death enters one runtime memorial path;
- `ProceduralEulogyEngine` is production-instantiated;
- eulogy inputs use real identity/death state;
- epitaph data reaches the player;
- heirlooms become distributed, memorialized, pending, or lost exactly once;
- grief reaches real living survivors;
- grief changes later morale/fatigue/fitness;
- wall-carving content reaches decor/memorial surfaces;
- funeral/refusal choices have real costs/consequences where authored;
- phantom-memory triggers receive real `phantom_background` input;
- memorial/journal/briefing/audio fire exactly once;
- save/load preserves memorial, heirloom, grief and record state;
- place memories are inspectable through legitimate channels;
- collapses/deaths/player actions alter place records;
- archive/record-keeping can preserve or lose knowledge;
- census/standing records continue into death/memorial history;
- place-memory growth is bounded;
- cohort maturation is called from production day loop;
- a 500-day journey reaches maturation;
- age classes affect ration/duty/apprenticeship/dose where supported;
- hardcoded live cohort demo IDs are removed;
- births use the canonical cohort ledger;
- generational chapters are simulation-driven;
- child death uses the same memorial pipeline;
- final wishes are wired or explicitly reclassified;
- population growth collides with food/berths/duty/dose limits;
- `childrenSurvived` derives from cohort truth;
- a deterministic long-run generational soak passes;
- content-utilization status is corrected only after actual runtime effects;
- all required build, bridge, integrity, narrative and port gates pass.

---

# 4. Phase P0 — Authority and Content Inventory

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
death event sources
fate record authority
MemorialSystem consumers
ProceduralEulogyEngine references
DwellerHeirloomCatalog consumers
ApplyGrief callers
wall_carving_templates consumers
confession_secrets consumers
echoes consumers
phantom_triggers consumers
phantom_background loader
LocationMemorySystem writers/readers
StandingRecordEngine consumers
CohortSystem callers
TryMaturation callers
GenerationalSuccessionEngine callers
final_wishes consumers
childrenSurvived ending derivation
```

## P0.2 Memorial stage matrix

Create `docs/systems/MEMORIAL_PIPELINE.md` with:

```text
stage
authority
input
output
runtime caller
save owner
event
UI surface
test
status
```

Stages:
1. death observed;
2. fate/death quality;
3. memorial row;
4. eulogy;
5. epitaph;
6. keepsake disposition;
7. grief;
8. decor/carving/funeral;
9. journal/briefing;
10. phantom/place/standing-record consequences.

## P0.3 Memory-content inventory

Create a catalog matrix for:
- heirlooms;
- epitaphs;
- wall carvings;
- confessions;
- echoes;
- phantom triggers;
- final wishes;
- standing-record memory.

Columns:

```text
catalog
entry count
loader
runtime consumer
selection rule
output/effect
content exemption
status
```

## P0.4 Baseline failure tests

Capture before repair:

```text
survivor dies
→ fate record exists
→ no runtime eulogy
```

and:

```text
bonded survivor dies
→ living survivor receives no gameplay grief
```

Keep both as regression tests.

---

# TASK 41A — From Death to Consequence: One Memorial Pipeline

# 41A.0 Goal

When a survivor dies, at least six observable, persisted consequences must follow through one authority-backed pipeline.

## 41A.1 Memorial integration coordinator

Create a host/application orchestrator, e.g. `MemorialIntegrationCoordinator`.

Responsibilities:
- receive canonical death/fate event;
- invoke existing subsystems;
- enforce idempotence;
- return a structured result.

It does not own death truth.

## 41A.2 Stable death-processing key

Use a stable key such as:

```text
survivorId + deathRecordId/day
```

This prevents duplicate memorial processing after reload/rebind/replayed callbacks.

## 41A.3 Processing order

Recommended order:

1. validate canonical death;
2. ensure memorial row;
3. resolve eulogy inputs;
4. resolve epitaph;
5. resolve keepsake state;
6. apply grief once per surviving relation;
7. update decor/carving options;
8. update standing record;
9. emit events;
10. journal/briefing;
11. audio.

Each step must be individually idempotent or safely retryable.

## 41A.4 Instantiate `ProceduralEulogyEngine`

Wire the existing engine into production host composition. Do not rewrite it simply because it was previously unreachable.

## 41A.5 Eulogy input model

Assemble only real inputs:
- survivor identity;
- profession/archetype;
- death quality;
- memorial outcome;
- relationship context;
- recorded actions where already available;
- final wish if active;
- place of death where known.

## 41A.6 Deterministic eulogy

Same death state produces the same selected eulogy/template identity after load.

If RNG exists, seed from the stable death-processing key.

## 41A.7 Persistence choice

Preferred:
- persist template/selection IDs and underlying state;
- render localized prose from them.

Do not persist a second fully rendered prose authority unless immutable wording is an explicit design requirement.

## 41A.8 Epitaph integration

Verify `wasteland_grave_epitaphs.json` reaches the memorial player surface.

Selection uses only authored filters/current death state.

## 41A.9 Epitaph idempotence

Reload cannot choose a different epitaph for the same memorial.

## 41A.10 Heirloom authority

Use Plan 40A's keepsake field plus `DwellerHeirloomCatalog`.

A keepsake record must track:
- definition/item;
- deceased original owner;
- disposition;
- current holder if inherited;
- memorialized/lost/pending status.

## 41A.11 Keepsake dispositions

Supported states:

```text
inherit_to_survivor
place_on_memorial
store_unclaimed
lost_unrecovered
```

Do not expose impossible options.

## 41A.12 Player-controlled disposition

Where multiple valid dispositions exist, the player chooses. No hidden random heir.

## 41A.13 Inventory delivery

If a keepsake becomes an inventory item, use Plan 35's delivery contract.

Preserve ownership/provenance metadata.

## 41A.14 Memorial-wall delivery

Use the existing shelter decor/memorial bridge.

After Plan 40B, identify keepsakes through explicit metadata—not item-ID string parsing.

## 41A.15 Pending keepsake state

If the player has not decided, the shelter UI shows an unclaimed keepsake. Do not auto-delete or auto-assign it.

## 41A.16 Unrecovered-body state

When context says keepsake is inaccessible/lost, no inventory item is created.

## 41A.17 Runtime grief binding

Wire existing `ApplyGrief` / `IGriefSink` into the live memorial path.

## 41A.18 Grief audience

Determine affected living survivors from:
- relation affinity;
- trauma bonds;
- family identity where canonical;
- other existing social connections.

Do not apply identical grief to every survivor.

## 41A.19 Grief magnitude

Use existing relationship/death/memorial rules.

No new universal grief scalar if current systems already define weighting.

## 41A.20 Grief source IDs

Examples:

```text
grief:<deceased_id>
funeral_refused:<deceased_id>
memorial_completed:<deceased_id>
```

These feed Plan 24B attribution.

## 41A.21 Grief → fitness journey

Required integration:

```text
death
→ grief/insomnia
→ fatigue/morale change
→ FitnessVerdict degradation
→ work consequences
```

## 41A.22 Funeral/memorial action

Use existing action/choice infrastructure for:
- funeral;
- burial;
- memorial-wall placement;
- refusal/no ceremony;
where authored.

## 41A.23 Funeral costs

Use Plan 35C labour/material semantics.

No free labor if content says ceremony consumes work time/materials.

## 41A.24 Refusal consequence

UI remains neutral. Social/morale consequences come from actual systems.

## 41A.25 Wall-carving integration

Consume `wall_carving_templates.json` via morale-band/context gating.

## 41A.26 Carving persistence

A chosen carving becomes persistent decor/memory state and can feed existing morale effects.

## 41A.27 Confession catalog decision

Audit `confession_secrets.json`.

If it belongs naturally to final-wish/confession/death content, wire it through the existing choice system. Otherwise, leave it separate and correct its content status. Do not force consumption.

## 41A.28 Echoes integration

Consume `echoes.json` through Plan 18A where conditions match:
- death;
- memorial;
- revisit;
- anniversary;
- min day.

## 41A.29 Phantom background

Ensure Plan 40A makes authored `phantom_background` data available to the live phantom host.

## 41A.30 Phantom trigger context

Trigger from existing `phantom_triggers.json` using real survivor/location/context state.

## 41A.31 No new phantom system

The existing phantom system stays authority.

## 41A.32 Standing record

On death, preserve the person's registered identity and mark the death/memorial relationship in the standing record.

## 41A.33 Journal

Write one canonical death/memorial history entry with IDs, not duplicated freeform prose copies everywhere.

## 41A.34 Briefing

Emit Plan 31 semantic events for:
- survivor death;
- memorial created;
- keepsake claimed;
- funeral/refusal;
- meaningful grief consequence.

## 41A.35 Audio

Reuse existing memorial/death cues. Exactly one cue per semantic transition.

## 41A.36 Memorial surface

Show:
- survivor identity;
- death context;
- eulogy;
- epitaph;
- keepsake state;
- carving/funeral state;
- legacy links where current UI supports them.

## 41A.37 Empty slots as signals

An unclaimed keepsake or unfilled carving slot remains visible as unfinished memory work.

## 41A.38 Persistence ownership

Persist:
- memorial row;
- eulogy/epitaph selection IDs if needed;
- keepsake disposition;
- grief through existing systems;
- carving/decor;
- standing-record state;
- death-processing marker.

Derive rendered eulogy where safe.

## 41A.39 Mid-pipeline save

Save after memorial creation but before keepsake choice.

Load:
- one memorial only;
- pending choice remains;
- grief/effects do not replay twice.

## 41A.40 Heirloom idempotence test

No duplicate item or decor entry after reload/rebind.

## 41A.41 Grief pair idempotence

Each deceased→living relation receives the initial grief application exactly once.

## 41A.42 Tone discipline

Run narrative checks for:
- repeated eulogy patterns;
- contradictions;
- overly inspirational tone;
- mismatch with death facts.

## 41A.43 Port-contract enforcement

Plan 36A must be able to assert required memorial sinks are bound:
- grief;
- item/keepsake;
- decor;
- journal/event;
- eulogy input path.

## 41A.44 End-to-end death journey

```text
survivor dies
→ fate
→ memorial
→ eulogy
→ epitaph
→ heirloom disposition
→ grief
→ fatigue/morale
→ fitness change
→ journal/briefing
→ save/load
```

### 41A DoD

A death changes the shelter in at least six observable, persisted, attributable ways without duplicate effects.

---

# TASK 41B — Time Leaves Marks: Place Memory and Institutional Records

# 41B.0 Goal

Make real world events leave readable scars in locations and make archive/record keeping a genuine information channel.

## 41B.1 Define supported place-memory event types

Only use events existing systems already produce:

```text
death
discovery
collapse
battle/breach
carving/inscription
hazard
abandonment
major salvage
territory change
memorialization
```

## 41B.2 Keep `LocationMemorySystem` canonical

Extend current records/read models if required. Do not create another place-history authority.

## 41B.3 Memory record semantics

A record should carry stable fields such as:

```text
location_id
event_kind
subject_id
secondary_id
day
source
visibility
persistence_class
template_or_text_id
```

Use the current schema where possible.

## 41B.4 Persistence/retention class

If needed, classify memories as:
- temporary;
- long-lived;
- permanent;
- archived.

Do not invent categories without a retention consumer.

## 41B.5 Surface place memory in map detail

`map_detail` should show known history for the inspected node.

## 41B.6 Preserve information-channel rules

A memory becomes player-visible through:
- visit;
- radio;
- scout/expedition report;
- archive;
- standing record.

No global omniscient map reveal.

## 41B.7 Death scar

A survivor/expedition death at a node writes a location-memory record and links back to memorial history where appropriate.

## 41B.8 Landmark degradation scar

A collapse changes current world state and writes a memory of the former landmark.

## 41B.9 Player-action scars

Where real systems exist, actions such as:
- breach;
- burial;
- destruction;
- occupation;
can produce a scar record.

Do not fabricate event types without producers.

## 41B.10 Territory integration

Known scars may appear alongside Plan 30 territory context, but memory does not reveal unknown ownership or events.

## 41B.11 Standing-record memory

Ensure `standing_record_memory.json` reaches a runtime consumer and supports life→death continuity.

## 41B.12 Census → memorial → record test

```text
person registered
→ survives/acts
→ dies
→ memorial created
→ standing record marks death/legacy
```

## 41B.13 ArchiveDesk integration

Use existing archive authority for preservation/cataloging/retrieval as supported.

## 41B.14 Library/manual integration

Use `library_study` and manuals only through existing knowledge/record seams. No new archive points currency.

## 41B.15 Record loss

If current systems model fire/flood/neglect/archive failure, records can become unavailable to the shelter.

## 41B.16 Separate world truth from archive knowledge

Losing paper records must not rewrite actual world history needed by simulation.

## 41B.17 Archivist labour

If archive maintenance is labor-based, use duty roster, fitness and skill.

## 41B.18 Archive events

Plan 31 semantic events for:
- records lost;
- records preserved/restored;
- archive damaged.

## 41B.19 Player-authored memory

Epitaphs and carvings can create persistent shelter/location records.

## 41B.20 Location strata

Surface existing strata/inscription data as historical layers where supported.

## 41B.21 No inline C# narrative

New player text goes through authored/localized content paths.

## 41B.22 Bounded growth

400-year campaigns cannot accumulate unbounded raw event lists.

## 41B.23 Retention policy

Following Plan 39B:
- preserve critical/permanent records;
- roll up repetitive minor history;
- summarize older generations/periods;
- cap transient events.

## 41B.24 Deterministic rollups

The same historical stream produces the same summaries and retention choices.

## 41B.25 Save/load

Place-memory records remain in the existing world-evolution section if that is canonical.

## 41B.26 Scarred/pristine snapshots

Capture a location detail surface in:
- pristine state;
- scarred/history-rich state.

## 41B.27 Write→read tests

For each supported event type:
- write;
- save;
- load;
- reveal through permitted channel;
- inspect/read.

## 41B.28 Collapse test

Landmark collapse must update current map/world state and preserve historical memory.

## 41B.29 Archive-loss test

Player archive loses the record, but world truth remains coherent.

## 41B.30 World QA

Run tile/zone/world-history authority checks.

### 41B DoD

The wasteland keeps receipts of real campaign events, and reading them is itself a gameplay information channel.

---

# TASK 41C — Grow Up: Cohort Maturation and Generational Turnover

# 41C.0 Goal

Children must age into mechanically distinct survivors, and multi-year chapters must reflect real human turnover.

## 41C.1 Audit `CohortSystem`

Map current fields for:
- child ID;
- parents/uncertainty;
- birth day;
- age band/class;
- morality memory;
- maturation;
- survival/death.

## 41C.2 Select the real day-loop owner

Use an existing campaign-day owner:
- survivor social;
- survivors needs;
- cohort/generational owner if present.

No second day loop.

## 41C.3 Call `TryMaturation`

Each relevant day:
- iterate active cohort members;
- resolve age through calendar;
- invoke threshold transition exactly once.

## 41C.4 Calendar-driven age

Use Plan 38's canonical calendar/day context. No `% 365` or duplicate year arithmetic.

## 41C.5 Transition idempotence

Reloading on or after a birthday cannot repeat the same maturation stage.

## 41C.6 Author age classes

Use project terminology; candidate categories:

```text
infant
child
adolescent
adult
elder
```

## 41C.7 Age-class data

Centralize:

```text
age_class
threshold
consumption_share
duty_eligibility
apprentice_eligibility
dose_susceptibility
health modifiers
schooling eligibility
```

Only fields with real consumers.

## 41C.8 Ration share

Plan 22/35 ration allocation reads age class. Dependents are not free from the food economy.

## 41C.9 Shelter load

Cohort members count toward real population pressure:
- rations;
- berths;
- caregiving;
- disease exposure;
as applicable.

## 41C.10 Apprenticeship

Eligible age classes can enter the existing apprenticeship system, which writes the campaign skill authority.

## 41C.11 Duty eligibility

Eligible adults can enter duty roster, still subject to Plan 24 fitness/skill restrictions.

## 41C.12 Education/knowledge

Reuse library/schooling/knowledge systems. No new education stat.

## 41C.13 Radiation susceptibility

If existing dose model supports age sensitivity, feed an age-class factor into the canonical radiation calculation.

## 41C.14 Health/disease effects

Only connect age effects already represented by current medical contracts. No new pediatric subsystem.

## 41C.15 Birth authority

`BookChild(...)` remains the canonical cohort registration path.

## 41C.16 Parallel family-content boundary

Romance/family content may decide when births happen. It must call the cohort authority. This plan does not design romance.

## 41C.17 Parent ambiguity

`guessBand` is intentional uncertainty. UI should present uncertainty honestly rather than treat it as missing data.

## 41C.18 Remove live demo cohort literal

Delete/replace hardcoded IDs such as `sv_cohort_demo` in production UI paths.

## 41C.19 Demo-ID source gate

Add a targeted source scan for known demo/test IDs in production surfaces.

## 41C.20 Dose register real subject

Dose register mutations must apply to the selected real survivor/cohort member, not a literal demo ID.

## 41C.21 Generational succession

Read existing succession semantics before wiring.

Tie chapter advancement to real cohort/generation turnover and death/maturation events as designed.

## 41C.22 No cosmetic chapter progression

A UI navigation/page flip cannot advance the generation.

## 41C.23 Calendar/succession consistency

Plan 38 calendar displays the same chapter/year values the succession authority owns.

## 41C.24 Maturation semantic events

Use Plan 31 kinds/generalized forms for:
- cohort matured;
- apprentice eligibility;
- duty eligibility;
- generation turned.

No daily age heartbeat.

## 41C.25 Child death

Child death uses 41A's pipeline, not a special UI path.

## 41C.26 Tone discipline

Child death content must remain restrained and non-sensational. Differences are authored, not hardcoded UI special cases.

## 41C.27 Final-wishes audit

Inspect `final_wishes.json`.

Wire only if a real existing quest/effect/death flow can consume it. Otherwise reclassify honestly.

## 41C.28 Final-wish lifecycle

If wired:

```text
wish discovered
→ optional task/choice
→ completed/refused/unfulfilled
→ memorial consequence
```

## 41C.29 Population economy

Multi-year telemetry must track:
- births;
- deaths;
- maturation;
- dependents;
- ration demand;
- berth demand;
- duty capacity;
- dose burden.

## 41C.30 Avoid runaway population

Tune with telemetry and real resource constraints rather than arbitrary invisible caps unless existing design specifies them.

## 41C.31 Retention strategy

Detailed records for living/recent generations; summaries for older generations per Plan 39B.

## 41C.32 `childrenSurvived` derivation

Define the exact canonical semantic meaning from cohort state, e.g. survived to campaign end or adulthood as required by Plan 19.

## 41C.33 Ending branch reachability

Test true and false `childrenSurvived` branches and combinations with other ending facts cited by Plan 19.

## 41C.34 500-day maturation journey

Start a child near a threshold and advance through real calendar/day loop.

Assert:
- maturation;
- ration-share change;
- apprenticeship/duty eligibility;
- persisted state.

## 41C.35 400-year soak

Stress repeated:
- births;
- maturation;
- deaths;
- memorials;
- succession;
- retention.

Assert deterministic digest, bounded state size and no ID collisions.

## 41C.36 Boundary tests

Before threshold: no transition.
Threshold day: one transition.
After threshold: no repeat.

## 41C.37 Consumption tests

Age-class ration share reaches the real ration allocation authority.

## 41C.38 Apprenticeship tests

Blocked before eligibility, allowed after, skill written to the same campaign skill authority.

## 41C.39 Succession tests

Generation turnover advances chapter exactly once and survives save/load.

## 41C.40 Birthday save edge

Save immediately before age threshold; load and advance; one maturation only.

### 41C DoD

Children grow up, consume resources, learn, work, suffer, die, are remembered, and can inherit the shelter.

---

# 5. Cross-Task Dependency Graph

```text
36A port contract
      │
      ▼
40A identity enrichment
      │
      ▼
41A death / memorial pipeline
      │
      ├────────► 41B place memory
      │
      └────────► 41C generational memory
```

Supporting:

```text
24C survivor journey ─────► death/admission/fate
24B needs stack ──────────► grief effects
18A echoes ───────────────► memory narrative
38A calendar ─────────────► maturation/year
35B/22B ─────────────────► rations/storage/funeral costs
19A ending ───────────────► childrenSurvived / legacy
30/32 world ──────────────► place memory / territory / reveal
39B retention ────────────► long-run memory bounds
```

---

# 6. Memorial Transaction Contract

```text
if deathProcessed(key):
    return existing result

resolve canonical death/fate
ensure memorial row
resolve eulogy/epitaph selections
resolve/persist keepsake state
apply grief once per relation
update decor/carving availability
update standing record
emit semantic events
journal/briefing/audio
mark processed
```

Partial failures retry only incomplete idempotent steps.

---

# 7. Memorial Output Contract

A memorial record should be able to answer:

```text
who died?
when?
where?
how?
what was remembered?
what was left behind?
who inherited it?
what did the shelter do?
who grieved?
what changed afterwards?
```

Not every field must be visible on one screen.

---

# 8. Grief Contract

Grief is not a standalone stat.

It contributes through existing:
- insomnia;
- fatigue;
- morale;
- social/needs modifiers.

Attribution always identifies the source death/memorial outcome.

---

# 9. Heirloom Contract

A keepsake is:
- unique;
- traceable to the deceased;
- owned/dispositioned;
- deliverable through existing inventory/decor rails;
- never generated twice.

---

# 10. Place-Memory Contract

World truth and player-known history remain separate where the world-information model requires it.

A hidden scar can exist before a player discovers it.

---

# 11. Record-Keeping Contract

The archive is a knowledge channel, not the owner of historical truth.

Records can be lost while history remains.

---

# 12. Cohort Contract

`CohortSystem` owns:
- child registration;
- age/maturation state;
- parent ambiguity;
- survival.

Ration, duty, apprenticeship, dose, succession and ending read it.

---

# 13. Age-Class Integration Matrix

Create `docs/systems/AGE_CLASS_MATRIX.md` with:

```text
age class
ration share
berth/care
apprenticeship
duty
dose
health/disease
memorial
ending
```

Every non-identity cell requires a real consumer and test.

---

# 14. Content-Utilization Policy

For every newly connected memory catalog, require real progression:

```text
DISCOVERED
LOADED
DESERIALIZED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Do not remove exemptions before the final gameplay effect is observable.

---

# 15. Event Vocabulary

Use Plan 31 canonical semantic kinds/generalized equivalents for:

```text
survivor_died
memorial_created
funeral_held
funeral_refused
heirloom_claimed
carving_added
records_lost
location_scarred
cohort_matured
generation_turned
final_wish_completed
```

No heartbeat events.

---

# 16. Save / Persistence Matrix

| State | Authority | Persist? |
|---|---|---:|
| death/fate | SurvivorFate | yes |
| eulogy rendered text | derived preferred | no |
| eulogy/template selection | memorial if needed | yes |
| epitaph selection | memorial | yes |
| heirloom disposition | memorial/inventory/decor | yes |
| grief | needs/insomnia/social | yes |
| carving | shelter decor | yes |
| phantom background | identity | yes |
| phantom trigger state | existing phantom system | yes |
| location memory | world evolution | yes |
| standing record | standing/census | yes |
| cohort age class | CohortSystem | yes |
| parent ambiguity | CohortSystem | yes |
| generational chapter | succession | yes/derived |
| `childrenSurvived` | derived | no |

---

# 17. Failure Injection Matrix

## N41.1 Duplicate death event
Expected: one memorial chain.

## N41.2 Save after keepsake delivery before UI refresh
Expected: no duplicate heirloom.

## N41.3 Grief delivered twice to same relation
Expected: idempotence gate catches it.

## N41.4 Missing grief/decor/item sink
Expected: port contract fails.

## N41.5 Missing phantom background
Expected: integrity/identity test identifies missing authored input.

## N41.6 Hidden place history shown globally
Expected: information-channel test fails.

## N41.7 Archive record destroyed
Expected: player access changes; world truth persists.

## N41.8 Birthday crossed after reload
Expected: one maturation.

## N41.9 Demo cohort ID in live code
Expected: source gate fails.

## N41.10 Maturation without ration/duty consequences
Expected: matrix integration test fails.

## N41.11 Chapter advanced by UI-only action
Expected: succession test fails.

## N41.12 400-year raw history unbounded
Expected: retention/scale test fails.

---

# 18. Determinism Contract

Same:

```text
seed
death state
identity/relations
memorial choices
world events
cohort births
calendar
```

must yield same:

```text
eulogy/template selection
epitaph
grief targets/magnitude
heirloom state
place memories
maturation dates
succession transitions
ending cohort facts
```

---

# 19. UI Acceptance

## Memorial
- identity;
- death context;
- eulogy;
- epitaph;
- keepsake;
- carving/funeral;
- legacy link.

## Shelter decor
- claimed/unclaimed keepsake;
- carving slots.

## Map detail
- known scars;
- memory layers;
- source/channel if useful.

## Cohort/standing record
- age class;
- parent uncertainty;
- maturation;
- role eligibility.

---

# 20. Accessibility

Memory state cannot be color-only.

Keyboard access is required for:
- keepsake disposition;
- memorial/funeral action;
- carving selection;
- map memory detail.

---

# 21. Performance Guardrails

Avoid:
- generating eulogies every frame;
- scanning all world memories on every map hover;
- iterating all historical cohorts every day.

Use indexed active state and deterministic rollups.

---

# 22. Long-Run Retention

Recommended policy:

```text
living + recent dead → detailed records
older generations → standing-record summaries
critical scars/memorials → permanent
repetitive minor history → rollups
```

Retention must be deterministic.

---

# 23. Narrative/Tone Guardrails

Eulogies and memorials should remain:
- restrained;
- concrete;
- non-repetitive;
- consistent with death facts;
- free of generic heroic uplift.

Child-death writing receives the same restraint with stricter review.

---

# 24. CI / Static Gates

Recommended gates:

```text
memorial_port_contract
heirloom_unique_ownership
grief_runtime_binding
memory_content_utilization
no_demo_cohort_ids
cohort_maturation_owner
age_class_matrix_coverage
place_memory_visibility
long_run_retention
```

---

# 25. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

Also run current equivalents of:

```text
ashfall-seed-replay: 400-year generational soak
ashfall-narrative-check
ashfall-narrative-continuity
ashfall-dialog-graph-lint
ashfall-tilemap-world-qa
ashfall-telemetry-playtest
```

---

# 26. Recommended Commit Breakdown

```text
41A-1 memorial authority map + failing runtime tests
41A-2 eulogy host wiring
41A-3 heirloom disposition + delivery/decor
41A-4 runtime grief binding
41A-5 epitaph/carving/funeral
41A-6 phantom/echo/standing-record links
41A-7 persistence/idempotence/audio/journey
41A-8 docs + port gates

41B-1 place-memory event/read model
41B-2 map-detail visibility
41B-3 death/collapse/player scars
41B-4 standing record + archive
41B-5 record loss/institution rules
41B-6 retention/save/snapshots/tests

41C-1 cohort/day-loop wiring
41C-2 age-class data + rations
41C-3 apprenticeship/duty/dose
41C-4 birth ambiguity + demo-ID removal
41C-5 succession wiring
41C-6 child memorial/final wishes
41C-7 population economy + ending
41C-8 500-day/400-year verification
```

---

# 27. Risk Register

## R41.1 Memorial double processing
Mitigation: stable death key + per-step idempotence.

## R41.2 Keepsake duplication
Mitigation: unique ownership record and transactional delivery.

## R41.3 Grief balance too punishing
Mitigation: relationship-weighted values and telemetry.

## R41.4 Memorial prose repetition/melodrama
Mitigation: existing engine + narrative checks.

## R41.5 Hidden-information leaks through place memory
Mitigation: explicit visibility/channel gating.

## R41.6 Archive loss deletes simulation truth
Mitigation: separate world truth from recorded/player-known knowledge.

## R41.7 Cohort growth destabilizes long-game economy
Mitigation: ration/berth/duty pressure and multi-year telemetry.

## R41.8 Chapter system remains cosmetic
Mitigation: transition tests tied to real cohort turnover.

---

# 28. Acceptance Checklist

## 41A — Memorial pipeline

- [ ] memorial pipeline documented
- [ ] canonical death input used
- [ ] stable processing key
- [ ] eulogy engine production-wired
- [ ] real eulogy inputs
- [ ] deterministic eulogy selection
- [ ] persistence policy documented
- [ ] epitaph reaches memorial UI
- [ ] epitaph idempotent
- [ ] heirloom ownership model
- [ ] dispositions implemented
- [ ] player choice where appropriate
- [ ] keepsake delivery uses Plan 35
- [ ] memorial-wall uses explicit metadata
- [ ] pending keepsake visible
- [ ] lost-body state correct
- [ ] runtime grief caller live
- [ ] relation-aware grief audience
- [ ] grief magnitude uses existing rules
- [ ] grief source attribution
- [ ] grief affects fatigue/morale/fitness
- [ ] funeral choices real
- [ ] funeral costs real where authored
- [ ] refusal consequence real
- [ ] wall-carving catalog consumed
- [ ] carving persists
- [ ] confession catalog audited honestly
- [ ] echoes integrated where appropriate
- [ ] phantom background loaded
- [ ] phantom triggers use existing host
- [ ] standing record updated
- [ ] journal/briefing events
- [ ] audio exactly once
- [ ] memorial UI complete
- [ ] empty-slot state visible
- [ ] persistence owners correct
- [ ] mid-pipeline save safe
- [ ] no duplicate heirloom
- [ ] grief pair idempotence
- [ ] tone tests pass
- [ ] port contract passes
- [ ] death→memorial→grief→fitness journey passes

## 41B — Place memory

- [ ] event vocabulary documented
- [ ] existing LocationMemorySystem remains authority
- [ ] memory record semantics clear
- [ ] retention policy defined
- [ ] map-detail visibility
- [ ] information-channel gating
- [ ] death scars
- [ ] collapse scars
- [ ] real player-action scars
- [ ] territory integration
- [ ] standing-record memory live
- [ ] census→memorial→record chain
- [ ] ArchiveDesk integration
- [ ] library/manual reuse only where real
- [ ] record loss from real causes
- [ ] world truth vs archive knowledge separated
- [ ] archivist labour real if required
- [ ] archive events
- [ ] player-authored memory
- [ ] location strata visible
- [ ] no inline prose
- [ ] bounded growth
- [ ] deterministic rollups
- [ ] save round-trip
- [ ] scarred/pristine snapshots
- [ ] write→read tests
- [ ] degradation history test
- [ ] archive-loss test
- [ ] world QA green

## 41C — Generations

- [ ] cohort state audited
- [ ] day-loop owner chosen
- [ ] TryMaturation has production caller
- [ ] calendar-driven age
- [ ] transition idempotence
- [ ] age classes authored
- [ ] age-class matrix
- [ ] ration share integrated
- [ ] dependents count toward resource pressure
- [ ] apprenticeship eligibility
- [ ] duty eligibility
- [ ] knowledge/education reuse
- [ ] dose susceptibility reuse
- [ ] no new pediatric system
- [ ] BookChild remains birth authority
- [ ] family-content boundary documented
- [ ] parent ambiguity legible
- [ ] demo cohort ID removed
- [ ] demo-ID gate
- [ ] dose surface uses real subject
- [ ] succession uses real turnover
- [ ] no cosmetic chapter advance
- [ ] calendar/succession consistency
- [ ] maturation events semantic
- [ ] child death uses memorial pipeline
- [ ] child-death tone checks
- [ ] final wishes audited
- [ ] final-wish path real or reclassified
- [ ] population telemetry
- [ ] runaway population addressed through real constraints
- [ ] long-run retention
- [ ] childrenSurvived derived
- [ ] ending branches reachable
- [ ] 500-day maturation journey
- [ ] 400-year soak deterministic/bounded
- [ ] maturation boundary tests
- [ ] ration-share tests
- [ ] apprenticeship tests
- [ ] succession tests
- [ ] birthday save edge

---

# 29. Ship / No-Ship Gate

**SHIP** only if:

```text
memorial_pipeline_authorities == 1
AND runtime_eulogy_consumers >= 1
AND gameplay_grief_consumers >= 1
AND duplicate_heirlooms_on_reload == 0
AND death_events_without_memorial_witness == 0
AND epitaph_content_player_reachable == true
AND wall_carving_content_player_reachable == true
AND phantom_background_live == true
AND memorial_save_roundtrip == pass
AND grief_affects_existing_needs_stack == true
AND place_memory_authorities == 1
AND hidden_place_memory_global_leaks == 0
AND location_memory_growth_bounded == true
AND TryMaturation_production_callers >= 1
AND live_demo_cohort_ids == 0
AND age_class_nonidentity_cells_tested == 100_percent
AND generational_chapter_is_simulation_driven == true
AND childrenSurvived_is_derived == true
AND generational_400_year_soak == pass
AND content_utilization_selftest == pass
AND narrative_checks == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 30. Implementer Handoff

1. Draw the death-to-memorial pipeline and identify the owner of every stage.
2. Do not create another memorial or grief system.
3. Wire the existing eulogy engine first.
4. Make every memorial step idempotent.
5. Treat keepsakes as unique owned goods.
6. Use Plan 35 delivery rails for inherited items.
7. Bind grief through Plan 24's existing needs/insomnia stack.
8. Keep phantom memory authority intact and provide its missing identity input.
9. Reuse decor, journal, briefing and standing-record surfaces.
10. Let place memory record only real simulation events.
11. Separate world truth from player/archival knowledge.
12. Keep long-world memory bounded.
13. Call `TryMaturation` from the actual day loop and drive it from Plan 38 calendar.
14. Give age classes real effects through existing systems.
15. Remove hardcoded cohort/demo IDs from production.
16. Make succession depend on real population turnover.
17. Derive ending facts from cohort state.
18. Close with a death journey, a scarred-place journey, a 500-day maturation journey and a deterministic 400-year soak.

---

# 31. Final Outcome

When this plan is complete, ASHFALL remembers.

A survivor's death is no longer a terminal boolean. Their fate produces a memorial, an eulogy, an epitaph, a keepsake decision, grief in the people who knew them, a physical mark in the shelter, a journal record, and later consequences in fatigue, morale, fitness, place memory, and the ending.

The wasteland remembers too. A location can carry scars from collapse, death, breach, burial, and occupation. Those scars are not globally omniscient map text; the player learns them by going there, hearing about them, or preserving records. Archives can protect that knowledge and, when neglected or destroyed, lose access to it without rewriting history.

Time also becomes human. Children booked into the cohort ledger actually mature. Their ration share changes. They become apprentices, then workers. Radiation and illness affect them through the same health systems. Some die and are mourned. Some survive long enough to make `childrenSurvived` true because the cohort says so—not because an ending flag was hardcoded.

Generational chapters then mean something: people aged, inherited, died, were remembered, and left the shelter to someone else.

The result is not a new memorial game. It is the existing survivor, social, decor, world-memory, cohort, calendar, ration, duty, journal, and ending systems finally preserving the fact that people were here.
