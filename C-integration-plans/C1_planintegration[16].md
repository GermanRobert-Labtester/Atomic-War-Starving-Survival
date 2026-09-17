# C1 — Flagship Integration Plan [16]: Depth Passes — Pouring the Dead Content Onto the New Rails

> **Output:** `C1_planintegration[16].md`
>
> **Source baseline:** Plan 49 — Depth Passes: Pouring the Dead Content Onto the New Rails
>
> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer* — **closing plan**
>
> **Depends on:** Plan 45A/45B acceptance ladder and dead-bucket sweep; Plan 40A/40B identity + tags; Plan 31A semantic event kinds; Plan 42A/42B voice delivery and density; Plan 41A/41B memorial + place memory; Plan 15A/18A choice resolution and echoes; Plan 46B reachability metrics; Plan 25A/25C localization/overlays; Plan 29B generated evidence claims.
>
> **Mandatory Wave-7 order:** 45A → 46A → 47A → 46B → 48A → **49A** → 45B → 47B → 48B → **49B** → 46C → 47C → 48C → **49C**.
>
> **Primary architectural rule:** no new gameplay subsystem in this plan. Content may receive a loader, selector, adapter, or host bridge only when that thin layer feeds an existing authority. If a family needs a genuinely new system, it becomes a Wave-8 plan instead of being smuggled into 49A–49C.
>
> **Primary acceptance rule:** every surviving family must satisfy Plan 45's declared terminal stage and publish Plan 46B reachability evidence. A loader alone is not success.
>
> **Primary editorial rule:** do not author replacement content to improve a metric. The task is to connect, deduplicate, repurpose, or archive content that already exists.
>
> **Guardrails:** no second choice resolver; no duplicate questline authority; no inline prose; no warning delivered only through flavor text; no uncounted root-array acceptance; no content exemption removal before runtime evidence; no wave-report claim without generated evidence.

---

# 0. Mission

Waves 1–6 built the rails.

Wave 7 made those rails enforceable:
- Plan 45 acceptance;
- Plan 46 measurement;
- Plan 47 content-pack boundary;
- Plan 48 release evidence.

Plan 49 is the cargo pass.

The source baseline identifies hundreds of authored definitions that exist today but reach no player. They are not one type of dead content. They fall into distinct clusters:

```text
PLACE / ATMOSPHERE
    ↓
188 authored lines

MEDICAL / CLINICAL
    ↓
83 authored texts

MEMORY CORPUS
    ↓
85 authored logs / journal / memorial entries

ENCOUNTER / CHOICE
    ↓
64 authored entries

SMALL RITUAL / COLLECTION / TRADE FAMILIES
    ↓
confessions
final wishes
cassette sets
damaged map zones
wall carvings
trade tells
survivor fields
item tags
...
```

The implementation target is not:

```text
JSON
→ loader
→ green scanner
```

It is:

```text
JSON
→ shared loader pattern
→ existing authority
→ state-conditioned query
→ selected definition
→ live player surface
→ real consequence where gameplay
→ utilization evidence
→ reachability evidence
→ exemption removed
```

The three depth passes therefore have distinct goals:

```text
49A
PLACE & ATMOSPHERE
"the world reads differently because of what is actually true"

49B
CLINICAL KNOWLEDGE & MEMORY
"the ward and archive know things the player actually earned"

49C
ENCOUNTERS, QUESTS, RITUALS & COLLECTION
"authored choices resolve through one authority and the remaining small families each acquire one honest consumer"
```

The plan closes Wave 7 only when the project can say, with generated evidence:

> The content that survived the sweep is reachable, the dead-content bucket remains empty, and the acceptance gate is now load-bearing.

---

# 1. Source-Evidence Interpretation

## 1.1 Place and atmosphere is the largest coherent text cluster

The source maps:
- `environmental_atmosphere_expansion.json`;
- `environmental_texts_expansion_05.json`;

to **188 authored lines**.

These lines belong on existing:
- map detail;
- daily briefing;
- place-linked voice;
- place-memory state.

They do not justify a new world simulation.

## 1.2 Medical text is not one semantic family

`medical_texts.json` mixes likely roles:
- diagnostic knowledge;
- procedural notes;
- diegetic documents.

49B must classify each definition before wiring it.

## 1.3 Memory text should become possessed/discovered history

Audio logs and journal entries should not be ambient strings available everywhere.

They should enter the shelter through:
- salvage;
- exploration;
- archive;
- memorial;
- autopsy/knowledge,
depending authored type.

## 1.4 Encounter content must use the existing resolver

The source explicitly prohibits another choice pipeline.

Existing `ResolveChoice` + effect appliers are the authority.

## 1.5 Questline families are ambiguous today

Several files describe overlapping questline concepts.

49C must publish one ownership table and remove duplicate authorities.

## 1.6 Small root-array families are measurable only after 45A

Before Plan 45's root-array fix, an acceptance report can falsely say "zero definitions."

49C may not accept them until current measurement proves their real counts.

## 1.7 Reachability is part of completion

The source requires per-family reachability from Plan 46B.

A player-facing line that is technically selectable under an impossible predicate remains dead content in practice.

---

# 2. Non-Negotiable Depth-Pass Invariants

## INV-49.1 — No new gameplay system

A thin content selector/adapter may exist.

It must not become a parallel owner of:
- weather;
- world state;
- medicine;
- memorials;
- quests;
- choices;
- collections;
- trade;
- survivor identity.

## INV-49.2 — Existing state causes content selection

Flavor is selected from real:
- place;
- weather;
- season;
- knowledge;
- memory;
- identity;
- stance;
- trust;
- quest state.

Not from arbitrary detached RNG.

## INV-49.3 — RNG breaks ties only

Randomness may choose among equally eligible authored lines.

It may not create the underlying state predicate.

## INV-49.4 — Every selected line is explainable

Developer diagnostics can answer:

```text
why was this definition eligible?
why was this one selected?
```

## INV-49.5 — Flavor never carries sole mechanical warning

If a line implies:
- radiation danger;
- disease risk;
- route closure;
- hostile control;
- critical weather;

the owning gameplay UI/event must state that fact independently.

## INV-49.6 — Knowledge is gated by earned channels

The player does not know:
- clinical facts;
- hidden place history;
- final wishes;
- confessions;
- map fragments
without a legitimate source.

## INV-49.7 — Content uses localization/overlay rails

No new inline C# prose.

## INV-49.8 — Choice content uses one resolver

`ResolveChoice` / canonical effect-applier path remains sole choice execution authority.

## INV-49.9 — Questline ownership is singular

After 49C:
- one canonical questline authority;
- migration note for retired families.

## INV-49.10 — Collection content remains collection content

Cassette/vinyl/map-fragment content is not given fake consumable gameplay just to reach `EFFECT_PRODUCED`.

Use honest family terminal stage.

## INV-49.11 — Acceptance follows family type

Gameplay:
- `EFFECT_PRODUCED`.

Narrative/presentation:
- `SELECTED`.

## INV-49.12 — Every family publishes reachability

No acceptance without a Plan-46B reachability number.

## INV-49.13 — Unreachable definitions are fixed or removed

Impossible predicate combinations do not remain in production as dormant ballast.

## INV-49.14 — Exemptions leave only after proof

A family remains exempt until:
- acceptance gate passes;
- runtime evidence exists;
- reachability report exists.

## INV-49.15 — Wave close uses generated evidence

No manually typed "all content is wired" claim.

---

# 3. Definition of Done

Plan 49 closes only when:

- all target families are re-counted after Plan 45 root-array fixes;
- all target families have declared acceptance type and terminal stage;
- place/atmosphere catalogs load through shared loader conventions;
- atmosphere selection uses real state predicates;
- map detail receives location-specific atmosphere;
- briefing receives a bounded atmosphere line;
- place-linked voice uses the same eligible context;
- hidden/unsurveyed places expose less detail;
- atmosphere text uses localization/overlay rails;
- no atmosphere line is the sole carrier of a mechanical warning;
- contradictory place prose is eliminated;
- atmosphere reachability is measured and below-target families are retuned;
- impossible/unplaceable entries are fixed or removed;
- 49A catalogs reach accepted stages and exemptions are removed;
- medical text is classified by role;
- diagnosis knowledge is earned through real systems;
- autopsy can unlock clinical knowledge where authored;
- ward notes/memorials vary with real death/outcome state;
- journal/audio-log entries enter through real collection/discovery channels;
- archive growth is bounded;
- memory/clinical references agree with identity and place records;
- medical/memory reachability is measured;
- memory and clinical families pass deterministic selection;
- relevant exemptions are removed;
- encounters/arc events/echoes/moral choices use one resolver;
- authored moral-choice routes are live and measurable;
- moral-choice stubs are either adopted or archived/deleted;
- questline families are consolidated behind one authority;
- confessions/grudges feed pair-event/history mechanics;
- final wishes feed the memorial/death pipeline where supported;
- cassette sets use existing collectible/vinyl surfaces;
- damaged map zones use map-fragment reveal;
- trade text uses deterministic stance × trust selection;
- root-array families are counted and accepted honestly;
- every 49C family reaches at least its required stage or is archived;
- Wave-7 before/after evidence is published;
- `docs/content/ACCEPTANCE.md` gains worked examples;
- generated registry is clean;
- narrative continuity/lint passes;
- release gate passes;
- unresolved dead-content bucket remains zero.

---

# 4. Phase P0 — Re-derive the Cargo Manifest

## P0.1 Capture current baseline

Record:

```text
commit SHA
branch
dirty paths
Plan-45 acceptance artifact
Plan-46 reachability artifact
content-utilization artifact
target catalog counts
target definition counts
current stages
current exemptions
candidate consumers
runtime consumers
root-array counts
questline family inventory
generated registry status
```

---

## P0.2 Re-count every target catalog

Do not reuse source counts blindly.

Output:

`docs/content/WAVE7_DEPTH_PASS_MANIFEST.md`

Columns:

```text
cluster
family
file
definition_count
root_shape
family_type
required_stage
current_stage
consumer
exemption
reachability
disposition
```

---

## P0.3 Reconcile Plan 45B state

Some families may already have moved due to 45B.

Mark:
- ALREADY_ACCEPTED;
- ALREADY_ARCHIVED;
- STILL_PENDING.

49 does not rewire what is already complete.

---

## P0.4 Reconcile Plan 46B metric format

Confirm reachability report can answer:

```text
eligible definitions
selected definitions
unique definitions seen
share reachable
share actually seen
median first-seen day
unreachable IDs
```

Use actual supported metrics.

---

## P0.5 Establish no-new-system test

Before task execution, identify every proposed new class.

Classify:

```text
loader
selector
adapter
host bridge
gameplay authority
```

Any new `gameplay authority` blocks 49 and becomes Wave 8.

---

# TASK 49A — Place & Atmosphere: State-Conditioned Wasteland Texture

# 49A.0 Goal

Make the 188-ish authored place/atmosphere lines reachable through real world state:

```text
place
+ weather
+ season
+ memory
+ control
+ knowledge
→ eligible atmosphere
→ deterministic tie-break
→ map / briefing / voice
```

---

## 49A.1 Re-read actual schemas

Inspect both catalogs in full.

Document actual fields:
- ID;
- location/zone/sector;
- weather;
- season;
- min/max day;
- control/territory;
- radiation/dose;
- memory flags;
- text;
- tags;
- any priority/rarity.

Do not design from summarized schema.

---

## 49A.2 Classify each atmosphere definition

Categories:

```text
PLACE_FIXED
WEATHER_CONDITIONAL
SEASON_CONDITIONAL
MEMORY_CONDITIONAL
CONTROL_CONDITIONAL
KNOWLEDGE_CONDITIONAL
MULTI_CONDITION
UNPLACEABLE
```

---

## 49A.3 Loader pattern

If no shared loader can consume current shape, add thin:

`Assets/Ashfall.Core/World/AtmosphereCatalogLoader.cs`

Requirements:
- `SystemTextJsonSerializer`;
- shared read/parse/warn pattern;
- catalog diagnostics;
- no new authority state.

---

## 49A.4 Thin selector, not world authority

If required, create:

`AtmosphereTextSystem` / `AtmosphereSelector`

Responsibilities only:
- index definitions;
- evaluate predicates;
- select one deterministically;
- return selection trace.

It does not mutate world state.

---

## 49A.5 Integrity tiers

Validate references:
- `loc_`;
- `zone_`;
- `sector_`;
- weather keys;
- season IDs;
- territory/control state;
- memory flags;
- tags.

---

## 49A.6 Unknown condition handling

Unknown condition key:
- data-integrity error.

Do not silently ignore predicate.

---

## 49A.7 Selection context

Define immutable input:

```text
location_id
zone/sector
day
season
weather
radiation/dose band
territory/control
known memory flags
survey/knowledge tier
locale
seed/context key
```

Only include real authorities.

---

## 49A.8 Eligibility before RNG

Algorithm:

1. exact place/zone compatibility;
2. required state predicates;
3. knowledge visibility;
4. priority/specificity;
5. tie set;
6. seeded tie-break.

---

## 49A.9 Specificity precedence

A line requiring:
- place + winter + collapse memory

should outrank:
- generic winter line
when both match.

Document deterministic scoring.

---

## 49A.10 No hidden random state

The selector cannot decide:
- "today the depot collapsed."

It may only reflect that collapse if world memory says so.

---

## 49A.11 Selection trace

Developer trace:

```text
selected_id
eligible_ids
rejected:
  id → unmet predicate
selection score
tie-break seed
```

Used by reachability debugging.

---

## 49A.12 Map-detail delivery

Map detail/node inspection shows:
- at most a bounded number of place-text lines;
- relevant known memory;
- atmosphere.

Use existing route.

No new panel.

---

## 49A.13 Knowledge gating

Unsure/unvisited:
- generic/vague wording.

Surveyed/visited:
- more specific eligible entries.

Use Plan 32 knowledge tier.

---

## 49A.14 Hidden-state prohibition

Map detail must not reveal:
- hidden control;
- hidden contamination;
- undiscovered death/scar
through atmosphere alone.

---

## 49A.15 Daily briefing delivery

At most one atmosphere line per configured budget.

It belongs in a low-priority flavor/ambient section.

Critical events outrank it.

---

## 49A.16 Briefing eligibility

Briefing atmosphere should relate to:
- shelter/current relevant place;
- recently visited/active location;
- major world node,
according to existing briefing context.

Do not choose random remote location.

---

## 49A.17 Voice trigger integration

Plan 42A may voice a place-linked line only when:
- same selection context is valid;
- density budget allows.

---

## 49A.18 One definition, multiple surfaces

Avoid selecting three contradictory lines independently.

Where useful:
- selection service returns same eligible family/context;
- each surface applies its own density.

---

## 49A.19 Voice density budget

Use Plan 42B.

No atmosphere voice if:
- critical warning;
- survivor line;
- high-priority event
already consumes budget.

---

## 49A.20 Warning duplication rule

If line says:
- air tastes metallic;
- bridge groans;
- ash cloud hides the road;

the owning mechanical surface must independently show:
- dose hazard;
- route condition;
- weather.

---

## 49A.21 Warning-only-by-flavor test

For each atmosphere definition tagged/parsed as hazard implication:
- require matching mechanical fact in context or forbid warning-like content from being sole signal.

Use manual classification if semantic parsing is unreliable.

---

## 49A.22 Localization keys

Convert text to keyed/overlay-compatible representation per 25A/25C.

Preserve source text via migration.

---

## 49A.23 Pack safety

Atmosphere family should comply with Plan 47 contract only if:
- schema is stable;
- selector supports overlays.

Do not mark stable automatically.

---

## 49A.24 Duplicate prose audit

Compare against:
- codex;
- journal;
- radio;
- world descriptions.

Goal:
- no conflicting facts;
- avoid exact duplicate lines.

---

## 49A.25 Narrative continuity

Run:
- `ashfall-narrative-continuity`.

Treat contradiction as data bug.

---

## 49A.26 Selection determinism

Same context:
- same eligible set;
- same selected ID.

---

## 49A.27 Reachability simulation

Use Plan 46B synthetic player profiles.

At minimum:
- normal 60-day;
- exploration-heavy;
- conservative shelter-first.

---

## 49A.28 Reachability threshold

Source target:
- normal 60-day campaign should reach at least ~10% of atmosphere corpus.

Treat 10% as starting diagnostic target, not a magical quality score.

---

## 49A.29 Low-reachability remediation

If < target:
- inspect impossible predicates;
- overly narrow locations;
- unreachable season/day combination;
- hidden knowledge requirement;
- duplicate near-equivalent definitions.

Retune predicates, not campaign balance, unless actual world bug exists.

---

## 49A.30 Unplaceable definitions

For each `UNPLACEABLE`:
- fix reference/state;
- broaden legitimate condition;
- archive/remove.

No permanent dead row.

---

## 49A.31 Cause explainability test

Every selected line must have at least one traceable real predicate beyond pure RNG unless explicitly generic atmosphere.

---

## 49A.32 Generic lines

Generic entries may be used as fallback.

They should:
- be lower specificity;
- not crowd out authored contextual lines.

---

## 49A.33 Event integration

If atmosphere reflects an actual state transition:
- existing Plan 31 event owns the transition.

Atmosphere selector does not emit duplicate gameplay event merely for displaying text.

---

## 49A.34 Save behavior

No need to persist atmosphere selection unless:
- surface history requires exact selected line.

If journal records it:
- persist selected ID in journal/history, not selector state.

---

## 49A.35 Runtime utilization evidence

For each catalog:
- LOADED;
- REGISTERED;
- QUERIED;
- SELECTED;
- `EFFECT_PRODUCED` only if family classification truly requires gameplay effect.

Likely terminal stage:
- SELECTED for pure atmosphere.

---

## 49A.36 Exemption removal

Remove exemption only after:
- acceptance;
- reachability;
- integrity.

---

## 49A.37 Tests

Required:
- load;
- references;
- state predicates;
- specificity;
- deterministic tie;
- knowledge gating;
- hidden-state leak prevention;
- density budget;
- hazard warning parity;
- reachability report.

---

## 49A.38 Docs

Create/update:

`docs/narrative/ATMOSPHERE.md`

Document:
- source catalogs;
- selection context;
- specificity;
- knowledge;
- surfaces;
- terminal acceptance stage.

### 49A DoD

Place and atmosphere prose becomes state-conditioned, explainable, reachable texture without creating another world or weather authority.

---

# TASK 49B — Clinical Knowledge and the Memory Corpus

# 49B.0 Goal

Make authored medical and memory text enter the game through knowledge, discovery, death, caregiving, and archive channels.

---

## 49B.1 Re-read `medical_texts.json`

Build per-definition role classification:

```text
DIAGNOSIS_HINT
CLINICAL_REFERENCE
AUTOPSY_FINDING
WARD_NOTE
PROCEDURAL_NOTE
DIEGETIC_DOCUMENT
UNCLASSIFIED
```

---

## 49B.2 No one-size-fits-all consumer

The file may remain physically one catalog.

Acceptance metadata must track which role each definition reaches.

---

## 49B.3 Medical loader

Use existing/shared loader pattern.

Do not add a parallel medical knowledge authority.

---

## 49B.4 Diagnosis knowledge integration

`DiagnosisKnowledgeStore` owns known clinical knowledge.

Medical text becomes visible only when relevant knowledge is unlocked.

---

## 49B.5 Knowledge source inventory

Existing legitimate channels may include:
- study/library;
- autopsy;
- treatment experience;
- found document;
- trained survivor skill.

Only wire existing systems.

---

## 49B.6 Autopsy produces knowledge

When authored relation exists:

```text
autopsy
→ finding
→ knowledge unlock
→ medical text available
```

This is a gameplay `EFFECT_PRODUCED` path because knowledge state changes.

---

## 49B.7 No omniscient diagnosis hint

Unknown disease:
- UI cannot quote advanced authored text until knowledge gate satisfied.

---

## 49B.8 Diagnosis presentation

When known:
- caregiving/medical panel can display relevant hints.

No inline fallback that bypasses catalog.

---

## 49B.9 Medical text provenance

UI can distinguish:
- learned by autopsy;
- learned from manual;
- known from survivor expertise
if current knowledge store tracks source.

Do not invent provenance if unsupported.

---

## 49B.10 Ward-note role

Procedural/ward texts attach to:
- admission;
- treatment;
- discharge;
- death;
where authored.

---

## 49B.11 Death-quality selection

Use real:
- `DeathQuality`;
- `MemorialOutcome`;
- condition;
- cause.

Different deaths should not always yield same note.

---

## 49B.12 Autopsy and memorial separation

Clinical finding:
- medical/archive knowledge.

Memorial:
- personal/social memory.

Do not merge tonal channels.

---

## 49B.13 Memory corpus inventory

Re-read:
- `audio_logs_expansion_05.json`;
- `journal_entries_expansion_05.json`;
- `memorials_expansion_05.json`.

Classify:
- found artifact;
- generated/personal journal;
- memorial variation;
- world-history record;
- unreachable.

---

## 49B.14 Audio logs as objects

A log should have:
- source/location;
- discovery day;
- possession/unlock state;
- playback route.

It should not exist merely because file loaded.

---

## 49B.15 Item/collection delivery

If logs are inventory-like:
- use Plan 35 delivery;
- canonical tags;
- collection surface.

If they are archive records:
- use archive authority.

Choose one per family.

---

## 49B.16 Place-linked discovery

A found log should be reachable through:
- scavenge;
- expedition;
- map discovery;
depending authored location.

---

## 49B.17 Journal entries

Distinguish:
- authored historical documents;
- player campaign journal output.

Do not confuse the authored catalog with live journal system entries.

---

## 49B.18 Journal catalog selection

If authored entries are discovered records:
- treat like archive content.

If they are templates for live journal:
- select from real event context.

Document authority.

---

## 49B.19 Memorial corpus integration

41A memorial pipeline selects authored memorial variant from:
- death quality;
- identity;
- funeral;
- place;
- keepsake;
as supported.

---

## 49B.20 Eulogy engine coexistence

Procedural eulogy and authored memorial text have distinct roles.

Example:
- eulogy = generated personal remembrance;
- memorial catalog = authored epitaph/note/ritual variation.

No duplicate prose blocks saying same thing.

---

## 49B.21 Heirloom links

Where memory entries reference heirloom/keepsake:
- resolve through Plan 41A/40B metadata.

No string-ID parsing.

---

## 49B.22 Archive capacity

Plan 39B retention rules apply.

Do not append infinite full text objects for 400 years.

---

## 49B.23 Bounded archive model

Candidate:
- active/recent full entries;
- older entries indexed/rolled into standing record;
- collectible logs retained as owned artifacts.

---

## 49B.24 Identity continuity

Names/relationships/professions in selected text must agree with Plan 40A canonical identity.

---

## 49B.25 Place continuity

Referenced place must resolve through Plan 41B/32.

---

## 49B.26 Date continuity

Authored dates/minDay conditions must be compatible with Plan 38 calendar.

---

## 49B.27 Narrative continuity gate

Run merged corpus through continuity tool.

Failures are data bugs.

---

## 49B.28 Tone gate

Run:
- `ashfall-narrative-check`;
- current writing QA.

Reject:
- heroic stock eulogies;
- repetitive melodrama;
- exposition dump;
- medical text that reads like omniscient UI.

---

## 49B.29 Reachability per family

Synthetic players:
- medically cautious;
- exploration-heavy;
- normal.

Publish:
- unique medical text seen;
- logs found;
- memorial variants selected.

---

## 49B.30 Low-reachability medical rows

Check:
- impossible affliction;
- knowledge source absent;
- autopsy path never fires;
- minDay outside campaign.

Fix or archive.

---

## 49B.31 Low-reachability memory rows

Check:
- location unreachable;
- collection route absent;
- memorial predicate impossible;
- duplicate IDs/families.

---

## 49B.32 100-death memorial determinism test

Seeded batch:
- 100 diverse death states;
- stable selected authored variants;
- no duplication bug;
- bounded archive growth.

---

## 49B.33 Medical acceptance targets

Role-specific:
- diagnosis/knowledge definitions likely need `SELECTED` + knowledge state effect where unlocking changes gameplay;
- pure procedural presentation may terminal at `SELECTED`.

Declare per subgroup.

---

## 49B.34 Memory acceptance targets

Audio/journal/memorial text:
- `SELECTED` if player-visible record is intended terminal state;
- `EFFECT_PRODUCED` only if selection mutates knowledge/morale/etc. via existing authority.

---

## 49B.35 Exemption removal

De-exempt each family only when its subgroup terminal requirements are met.

---

## 49B.36 Registry regeneration

Do not hand-edit.

---

## 49B.37 Tests

Required:
- role classification;
- knowledge gating;
- autopsy unlock;
- no unknown-hint leak;
- memorial death-variant selection;
- found-log discovery;
- archive retention;
- identity/place references;
- deterministic selection;
- reachability report shape.

---

## 49B.38 Docs

Create/update:

`docs/content/CLINICAL_AND_MEMORY_CORPUS.md`

### 49B DoD

The ward learns from real channels, the archive contains things the crew actually found or recorded, and the dead are remembered with authored variation tied to real circumstances.

---

# TASK 49C — Encounter, Quest, Ritual, Trade & Collection Closure

# 49C.0 Goal

Finish the remaining corpus by routing all choice-bearing content through one resolver and giving each small content family one honest, measurable consumer.

---

## 49C.1 Re-read choice-family schemas

Inspect:
- narrative encounters;
- arc events;
- moral-choice stubs;
- narrative questlines;
- dynamic questlines;
- master questline files;
- expansion quest files;
- echoes.

Build field/semantic comparison.

---

## 49C.2 Publish choice-family ownership table

Create:

`docs/content/CHOICE_AND_QUEST_AUTHORITY.md`

Columns:

```text
family
canonical file
loader
runtime owner
resolver
save owner
status
supersedes
migration
```

---

## 49C.3 One resolver

All player choices use canonical:

```text
ResolveChoice
→ effect applier
→ result
→ semantic event
→ save
```

Use current host/system seams.

---

## 49C.4 No second effect vocabulary

Effects use existing:
- item/consume;
- needs modifier;
- flags;
- quest state;
- location reveal;
- relationship;
- commitment;
- faction/standing.

No `NarrativeEffectV2`.

---

## 49C.5 Encounter bridge

`DoorEncounterSystem` / `ExpeditionEncounterBridge` should feed canonical resolver.

---

## 49C.6 Arc events

Arc events:
- trigger from actual campaign state;
- choices resolve same path.

---

## 49C.7 Echoes

Use Plan 18A already-live chain.

49C should not recreate echo loader/resolver if complete.

---

## 49C.8 Moral-choice authored quests

Wire behind 15A live player route.

Measure:
- choices per session;
- unique choice reachability;
- branch diversity.

---

## 49C.9 215-choice reconciliation

Recount current authored moral-choice total.

Do not assume source number unchanged.

---

## 49C.10 Choice density

Avoid flooding:
- cap encounter/choice frequency;
- prioritize unresolved/important chains;
- preserve agency.

---

## 49C.11 Moral-choice stubs decision

For `moral_choice_quest_stubs.json`:
- adopt into canonical quest authority;
or:
- archive/delete.

No permanent stub production catalog.

---

## 49C.12 Questline family comparison

Compare:
- schema;
- IDs;
- save references;
- runtime consumers;
- overlapping entries.

---

## 49C.13 Canonical questline file/family

Choose one authority.

May be:
- `questline_master`;
- generated/merged family;
- existing runtime canonical source.

Decision follows current source.

---

## 49C.14 Duplicate questline retirement

Retired files:
- archive;
- migration note;
- no runtime discovery.

---

## 49C.15 Save migration risk

Quest IDs referenced by saves must remain stable.

Do not rename IDs casually during file consolidation.

---

## 49C.16 Questline migration tests

Old save/reference:
- resolves canonical quest after merge.

---

## 49C.17 Confession secrets

Root-array count must be real first.

Use Plan 44B pair-event/social history seam.

---

## 49C.18 Confession effect

A confession may:
- reveal history;
- shift relationship band;
- alter forgiveness/grudge state.

Only if existing pair-event system supports.

---

## 49C.19 Confession selection

Eligibility based on:
- relationship;
- guilt/grudge;
- history;
- day/context.

No random secret with no actor state.

---

## 49C.20 Guilt sources

If `guilt_sources.json` is needed by confessions:
- wire as metadata/source vocabulary.

Do not create a new guilt stat if existing system uses marks/events.

---

## 49C.21 Final wishes

Use Plan 41C memorial/death path.

---

## 49C.22 Final-wish discovery

A wish may become known through:
- survivor disclosure;
- relationship;
- illness;
- event.

Do not reveal every survivor's wish globally.

---

## 49C.23 Final-wish effect

If completed:
- memorial/morale/relationship/standing effect via existing authorities.

If not mechanically supported:
- terminal at selected/recorded narrative state.

---

## 49C.24 Cassette sets

Root-array counted.

Use existing:
- vinyl/collection;
- hidden cache;
- archive/collection surface.

---

## 49C.25 Collectible contract

A cassette set is accepted when:
- discoverable;
- collectible;
- visible in collection;
- selected/playable if audio exists.

Do not invent consumable stat bonus merely to claim an effect.

---

## 49C.26 Damaged map zones

Use Plan 32C fragment-reveal path.

Definition selection:
- discovery;
- damaged-map item/fragment;
- world knowledge.

---

## 49C.27 Map-zone effect

If map fragment reveals/updates world knowledge:
- gameplay family reaches `EFFECT_PRODUCED`.

---

## 49C.28 Wall-carving templates

If not already completed by 41A:
- use memorial/decor pipeline.

No separate wall-carving system.

---

## 49C.29 Trade text

Use existing `TradeTellEngine`.

Selection key:

```text
stance
× trust band
× context
× seeded tie-break
```

---

## 49C.30 Trade-text deterministic test

Same trade context:
- same selected tell.

---

## 49C.31 Trade text never overrides mechanics

It may hint at:
- stance;
- trust;
- negotiation state.

Actual numbers/status remain visible elsewhere.

---

## 49C.32 Survivor field families

If `expansion_survivor_fields` still pending:
- Plan 40A identity authority consumes them.

49C only closes acceptance if 40A already wired.

Do not duplicate identity loading.

---

## 49C.33 Item tag families

If `expansion_item_tags` already accepted by 40B:
- mark ALREADY_ACCEPTED.

Do not touch.

---

## 49C.34 Root-array acceptance sweep

For every root-array family:
- real count;
- family type;
- current stage;
- selected/effect counts;
- reachability.

---

## 49C.35 Small-family one-consumer rule

Each surviving small family must have:
- one named canonical consumer;
- one selection/effect test;
- one reachability metric.

---

## 49C.36 No fake multi-consumer complexity

Do not connect one family to three systems just to appear important.

One honest consumer is enough.

---

## 49C.37 Reachability reports

Publish per family:
- eligible;
- seen;
- unique seen;
- unreachable;
- normal-session rate.

---

## 49C.38 Encounter reachability

Synthetic profiles:
- expedition-heavy;
- shelter-heavy;
- balanced.

No family should require one pathological playstyle unless authored.

---

## 49C.39 Quest reachability

Track:
- started;
- branches offered;
- choices resolved;
- completion;
- permanently unreachable IDs.

---

## 49C.40 Ritual/collection reachability

Track:
- discovered;
- selected;
- collected;
- displayed.

---

## 49C.41 Delete impossible definitions

Unreachable due to bad refs/impossible conditions:
- fix;
- archive/delete.

---

## 49C.42 Acceptance sweep

Every listed catalog:
- accepted at required stage;
- or archived with reason.

No lingering `exempt_no_source_evidence`.

---

## 49C.43 Wave-7 evidence table

Publish into:
- `docs/roadmap/WAVE_LEDGER.md`;
- Wave-7 index.

Columns:

```text
family
definitions before
stage before
definitions after
accepted stage
reachability
exemption removed
archived
runtime consumer
```

---

## 49C.44 Wave-level metrics

Publish:

```text
dead bucket before
dead bucket after
exemptions before/after
SELECTED before/after
EFFECT_PRODUCED before/after
root-array defs recovered
duplicate quest families retired
dead Core consumers removed/fixed
```

---

## 49C.45 Worked examples for Plan 45

Use 49A, 49B, 49C as real acceptance examples:

- atmosphere = narrative SELECTED;
- autopsy knowledge = gameplay EFFECT_PRODUCED;
- choice encounter = gameplay EFFECT_PRODUCED;
- cassette = collection SELECTED.

---

## 49C.46 Release evidence

Plan 48 release report includes Wave-7 final content evidence.

---

## 49C.47 Generated registry

Run `generate-catalog-registry.py --check`.

---

## 49C.48 Narrative gates

Run:
- narrative check;
- continuity;
- dialog graph lint.

---

## 49C.49 Release gate

Run Plan 48B/48C release gate as current.

---

## 49C.50 Final dead-bucket assertion

Required final statement:

```text
unresolved content families: 0
```

Any temporary exemption:
- separately counted;
- owner + due date;
- not classified as unresolved hidden bucket.

### 49C DoD

The encounter, ritual, trade, and collection corpora are playable or honestly player-reachable, quest authority is singular, and Wave 7 closes with generated proof that content debt cannot silently regrow.

---

# 5. Cross-Task Dependency Graph

```text
45A / 45B
acceptance + bucket
      │
      ▼
46B reachability
      │
      ▼
49A place atmosphere
      │
      ▼
49B clinical / memory
      │
      ▼
49C choices / collections
```

Supporting:

```text
40A identity ─────────────► medical/memory/survivor fields
40B tags ─────────────────► items/collections
31A kinds ────────────────► choice/event visibility
42A/42B voice ────────────► place-linked delivery
41A memorial ─────────────► memorial/final wishes
41B place record ─────────► atmosphere/history
15A choice route ─────────► moral-choice UI
18A echo chain ───────────► echoes
25A/25C text rails ───────► every prose family
29B evidence claims ──────► Wave-close claims
47 content-pack contract ─► stable overlay safety
48 release evidence ──────► Wave-close release proof
```

---

# 6. Thin-Adapter Rule

A class added in Plan 49 must pass this test:

```text
Does it own new gameplay state?
```

If YES:
- stop;
- move to Wave 8.

Allowed:
- parser;
- index;
- selector;
- adapter;
- presenter bridge;
- diagnostics.

Not allowed:
- new medical progression system;
- new quest state owner;
- new place-memory owner;
- new collection economy;
- new relationship/guilt system.

---

# 7. Content Selection Trace Contract

Every selector used in this plan should support developer trace:

```text
family
context
eligible IDs
rejected IDs + reasons
specificity/priority
selected ID
tie-break seed
```

This is essential for reachability debugging.

---

# 8. Reachability Contract

Acceptance proves a definition *can* be selected.

Reachability proves a representative campaign *does* expose it.

Both are required.

---

# 9. Reachability Tiers

Recommended reporting:

```text
UNREACHABLE
RARE
LOW
NORMAL
COMMON
```

Backed by measured rates.

Do not treat all rare content as a bug.

Rare content needs intentional predicates.

---

# 10. Unreachable Definition Workflow

For each unreachable ID:

1. inspect references;
2. inspect predicates;
3. inspect route/consumer;
4. inspect min/max day;
5. inspect required world state;
6. fix if bug;
7. archive if obsolete;
8. retain only if intentionally rare but actually reachable under a valid profile.

---

# 11. Narrative vs Gameplay Acceptance

## Narrative/presentation

Examples:
- atmosphere;
- memorial text;
- trade tell;
- cassette metadata.

Terminal:
`SELECTED`.

## Gameplay

Examples:
- autopsy knowledge unlock;
- map fragment reveal;
- moral-choice quest;
- confession relationship change.

Terminal:
`EFFECT_PRODUCED`.

---

# 12. Content Provenance

For selected line/definition, diagnostics should identify:

```text
catalog family
definition ID
base/pack source if Plan 47 active
consumer
selection context
```

Useful for bug reports and continuity.

---

# 13. Localization Contract

All player-facing text:
- stable localization key;
- locale overlay;
- no C# literal.

Content ID and text key remain distinct where architecture requires.

---

# 14. Knowledge Gating Contract

Knowledge channels:

```text
visit
survey
autopsy
study
found document
relationship disclosure
map fragment
radio/briefing
```

A definition may declare one or more existing gates.

No global omniscient lookup.

---

# 15. Choice Resolution Contract

Canonical path:

```text
definition selected
→ UI presents choices
→ player chooses
→ ResolveChoice
→ effect applier
→ state mutation
→ event
→ save
→ journal/history
```

No family-specific direct switch in UI.

---

# 16. Questline Authority Contract

After consolidation:

```text
one runtime questline registry
one loader/merge path
one save identity
many authored definitions
```

File families may remain physically separate only if they share one registry and have explicit roles.

No overlapping authority.

---

# 17. Archive Contract

When content is removed:
- preserve source in archive when authored prose has value;
- mark reason;
- exclude from production discovery;
- no acceptance obligation.

---

# 18. Bounded-History Contract

Long-running:
- journal;
- memorial;
- archive;
- place history
use Plan 39B retention.

49B cannot make retention worse.

---

# 19. Density Budgets

## Briefing
Atmosphere:
- max one low-priority line.

## Voice
Atmosphere:
- subject to Plan 42B density.

## Map detail
- bounded selected lines.

## Journal
- bounded live list; historical rollups.

---

# 20. Warning Parity

For every flavor line with hazard semantics, map to mechanical owner:

| Flavor implication | Mechanical owner |
|---|---|
| radiation | dose/radiation panel |
| disease | medical/contamination |
| weather | forecast/weather |
| route danger | map/route |
| hostile control | territory/faction |
| structural risk | encounter/world hazard |

Flavor may reinforce, never replace.

---

# 21. Medical Knowledge State Machine

```text
UNKNOWN
→ SUSPECTED
→ KNOWN
→ MASTERED
```

Only use existing states if already present.

If store uses simpler model, do not add this state machine.

This section is a conceptual mapping, not an implementation demand.

---

# 22. Memory Corpus State Model

Possible statuses:

```text
undiscovered
discovered
owned/archived
reviewed
```

Use actual existing collection/journal states.

Do not add unnecessary state.

---

# 23. Collection Family Contract

Collection content succeeds when:
- obtainable;
- represented in player collection/archive;
- inspectable/playable if applicable.

It does not require arbitrary stat effects.

---

# 24. Trade Tell Contract

Trade prose can reflect:
- stance;
- trust;
- scarcity context.

It cannot independently change trade numbers.

---

# 25. Confession Contract

A confession requires:
- two real actors;
- relevant history;
- disclosure condition;
- pair-event consequence if gameplay.

No generic global confession pool detached from survivor identity.

---

# 26. Final-Wish Contract

A final wish requires:
- known survivor;
- disclosure/trigger;
- memorial/death integration;
- optional commitment/action if existing system supports.

No random postmortem assignment.

---

# 27. Root-Array Closure

Every root-array family must show:

```text
definition_count > 0
OR genuinely empty file
```

Zero due to parser blind spot is forbidden.

---

# 28. Content-Utilization Expectations

At Wave close:

```text
target families:
- no DISCOVERED-only survivors
- no fake candidate-consumer acceptance
- no expired exemption
- no root-array zero-count blind spot
```

---

# 29. Wave-7 Evidence Artifact

Create/generated:

`artifacts/wave7-content-depth-evidence.json`

Suggested shape:

```text
wave
commit
families[]
  id
  definitions_before
  definitions_after
  stage_before
  stage_after
  reachability
  selected_count
  effect_count
  exemptions_removed
  archived_count
unresolved_count
```

---

# 30. Failure Injection Matrix

## N49.1 Atmosphere line selected for wrong place
Expected: selector test fails.

## N49.2 Hidden hazard revealed only by flavor
Expected: warning-parity test fails.

## N49.3 Unsurveyed place gets deep memory line
Expected: knowledge-gate test fails.

## N49.4 Medical hint shown before knowledge unlock
Expected: clinical gate fails.

## N49.5 Autopsy produces no knowledge despite authored mapping
Expected: effect test fails.

## N49.6 Memorial catalog always picks same entry
Expected: context/variation test flags poor distribution if predicates should vary.

## N49.7 Quest family uses second resolver
Expected: static authority gate fails.

## N49.8 Quest consolidation renames save-referenced ID
Expected: migration/save test fails.

## N49.9 Root-array family reports zero defs
Expected: 45A gate fails.

## N49.10 Cassette receives fake gameplay stat bonus solely for effect metric
Expected: family classification review rejects it.

## N49.11 Family accepted but reachability zero
Expected: Wave-49 close gate fails.

## N49.12 Exemption removed before runtime evidence
Expected: acceptance gate fails.

---

# 31. Test Pyramid

## Tier 1 — Data
- schema;
- references;
- IDs;
- root-array count.

## Tier 2 — Selector
- predicates;
- specificity;
- deterministic choice.

## Tier 3 — Authority integration
- knowledge;
- memorial;
- choice effect;
- pair event;
- collection.

## Tier 4 — UI
- map detail;
- caregiving;
- journal;
- trade;
- choice route.

## Tier 5 — Reachability
- synthetic campaigns;
- per-family unique exposure.

## Tier 6 — Long-run
- archive retention;
- 100-death memory;
- save migration.

---

# 32. CI / Gate Set

Use existing:
- Plan 45 content acceptance;
- Plan 46 reachability;
- Plan 29 generated claims;
- narrative gates;
- release gate.

Add no new broad gate unless required to express a unique invariant.

Possible narrow gates:
- `choice_authority_single`;
- `flavor_warning_parity`;
- `questline_authority`.

---

# 33. Verification Commands

Run per task and at Wave close:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/content-acceptance-gate.sh
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/generate-catalog-registry.py --check
bash scripts/ci/verify-fast.sh
```

Also:

```text
Plan-46B synthetic reachability report
ashfall-narrative-check
ashfall-narrative-continuity
ashfall-dialog-graph-lint
Plan-48 release gate
```

Use exact current command names if different.

---

# 34. Recommended Commit Breakdown

```text
49A-1 atmosphere schema audit + manifest
49A-2 shared loader / thin selector
49A-3 state predicates + trace
49A-4 map-detail integration
49A-5 briefing/voice integration
49A-6 localization/overlay migration
49A-7 reachability / unreachable cleanup
49A-8 acceptance / exemption removal / docs

49B-1 medical role classification
49B-2 diagnosis knowledge integration
49B-3 autopsy unlock path
49B-4 ward/death-quality notes
49B-5 audio-log/journal discovery
49B-6 memorial corpus integration
49B-7 retention/continuity/tone
49B-8 reachability / acceptance / de-exemption

49C-1 choice-family authority table
49C-2 resolver unification
49C-3 moral-choice quest route
49C-4 questline consolidation/migration
49C-5 confessions/final wishes
49C-6 cassette/map/wall-carving collections
49C-7 trade tells/root-array closure
49C-8 reachability / Wave-7 evidence / release close
```

---

# 35. Risk Register

## R49.1 Atmosphere overwhelms information

Mitigation:
- strict density;
- low-priority briefing;
- voice budget.

## R49.2 Flavor leaks hidden state

Mitigation:
- knowledge gating;
- predicate visibility tests.

## R49.3 Medical knowledge changes difficulty

Mitigation:
- preserve mechanics;
- text unlock informs rather than directly buffs unless authored;
- balance telemetry.

## R49.4 Archive growth explodes

Mitigation:
- Plan 39B retention;
- bounded history.

## R49.5 Quest merge breaks saves

Mitigation:
- stable IDs;
- migration tests;
- one authority table.

## R49.6 Reachability tuning makes rare content common

Mitigation:
- report distributions;
- preserve intentional rarity.

## R49.7 Metrics drive fake wiring

Mitigation:
- honest family terminal stages;
- no new gameplay effect for presentation families.

## R49.8 Thin selectors become accidental systems

Mitigation:
- no owned gameplay state;
- class inventory review.

---

# 36. Acceptance Checklist

## P0

- [ ] current utilization artifact regenerated
- [ ] current reachability artifact available
- [ ] Wave-49 manifest generated
- [ ] root-array counts verified
- [ ] already-complete families excluded
- [ ] proposed new classes classified
- [ ] no new gameplay authority hidden in task

## 49A

- [ ] actual atmosphere schemas re-read
- [ ] every atmosphere definition classified
- [ ] shared loader pattern used
- [ ] selector remains stateless/thin
- [ ] reference integrity tiers
- [ ] unknown conditions fail
- [ ] immutable selection context
- [ ] eligibility before RNG
- [ ] specificity precedence
- [ ] no random state invention
- [ ] selection trace
- [ ] map detail integration
- [ ] knowledge gating
- [ ] no hidden-state leak
- [ ] briefing bounded
- [ ] briefing context relevant
- [ ] voice integration
- [ ] cross-surface consistency
- [ ] voice density
- [ ] warning parity
- [ ] warning-only-by-flavor test
- [ ] localization/overlay migration
- [ ] pack contract reviewed if applicable
- [ ] duplicate prose audit
- [ ] narrative continuity
- [ ] deterministic selection
- [ ] synthetic reachability
- [ ] threshold assessed
- [ ] low-reachability remediation
- [ ] unplaceable entries fixed/archived
- [ ] cause explainability
- [ ] generic fallback lower priority
- [ ] no duplicate semantic event
- [ ] persistence behavior explicit
- [ ] utilization stages recorded
- [ ] exemptions removed only after proof
- [ ] tests green
- [ ] ATMOSPHERE.md updated

## 49B

- [ ] medical schema re-read
- [ ] medical definitions role-classified
- [ ] shared medical loader used
- [ ] DiagnosisKnowledgeStore remains authority
- [ ] legitimate knowledge sources identified
- [ ] autopsy knowledge path
- [ ] no omniscient hint
- [ ] diagnosis presentation gated
- [ ] provenance used only if supported
- [ ] ward-note integration
- [ ] death-quality variation
- [ ] clinical/personal memory separated
- [ ] memory corpus re-read
- [ ] audio logs become discoverable objects/records
- [ ] item/archive ownership decision
- [ ] place-linked discovery
- [ ] journal authored/live distinction
- [ ] journal selection authority
- [ ] memorial corpus integrated
- [ ] eulogy/memorial roles distinct
- [ ] heirloom references canonical
- [ ] archive bounded
- [ ] identity continuity
- [ ] place continuity
- [ ] date continuity
- [ ] narrative continuity gate
- [ ] tone gate
- [ ] reachability per family
- [ ] low-reachability medical rows fixed
- [ ] low-reachability memory rows fixed
- [ ] 100-death determinism
- [ ] subgroup acceptance targets declared
- [ ] memory acceptance targets declared
- [ ] exemptions removed after proof
- [ ] registry regenerated
- [ ] tests green
- [ ] clinical/memory docs updated

## 49C

- [ ] all choice-family schemas re-read
- [ ] ownership table published
- [ ] one ResolveChoice path
- [ ] no new effect vocabulary
- [ ] encounter bridge uses resolver
- [ ] arc events use resolver
- [ ] echoes reused from 18A
- [ ] moral-choice route live
- [ ] authored choice total re-counted
- [ ] choice density bounded
- [ ] moral-choice stubs adopted/archived
- [ ] questline families compared
- [ ] canonical quest authority chosen
- [ ] duplicate quest families retired
- [ ] save IDs preserved
- [ ] quest migration tests
- [ ] confessions counted
- [ ] confessions use pair-event history
- [ ] confessions state-gated
- [ ] guilt sources reuse existing social facts
- [ ] final wishes use memorial path
- [ ] final wishes discovered legitimately
- [ ] final-wish effects use existing authority
- [ ] cassette sets counted
- [ ] cassette collection consumer live
- [ ] collection terminal stage honest
- [ ] damaged map zones use reveal path
- [ ] map-zone effect produces world knowledge
- [ ] wall carving reuses 41A
- [ ] trade text uses TradeTellEngine
- [ ] trade selection deterministic
- [ ] trade text not mechanical authority
- [ ] survivor fields defer to/confirm 40A
- [ ] item tags confirm 40B
- [ ] every root-array family measured
- [ ] every small family has one consumer
- [ ] no fake consumer proliferation
- [ ] reachability reports
- [ ] encounter reachability
- [ ] quest reachability
- [ ] ritual/collection reachability
- [ ] impossible definitions fixed/archived
- [ ] all listed catalogs accepted/archived
- [ ] Wave-7 evidence table
- [ ] Wave-level metrics
- [ ] Plan-45 worked examples
- [ ] release evidence
- [ ] registry clean
- [ ] narrative gates green
- [ ] release gate green
- [ ] unresolved bucket = 0

---

# 37. Ship / No-Ship Gate

**SHIP** only if:

```text
new_gameplay_authorities_introduced_by_plan49 == 0
AND target_families_without_declared_terminal_stage == 0
AND atmosphere_unplaceable_production_defs == 0
AND atmosphere_hidden_state_leaks == 0
AND atmosphere_warning_only_by_flavor == 0
AND atmosphere_reachability_reported == true
AND medical_omniscient_text_leaks == 0
AND clinical_knowledge_has_real_unlock_paths == true
AND memory_corpus_reachability_reported == true
AND archive_growth_bounded == true
AND choice_resolvers == 1
AND duplicate_questline_authorities == 0
AND save_referenced_quest_ids_preserved == true
AND root_array_count_blind_spots == 0
AND surviving_small_families_without_consumer == 0
AND target_families_without_reachability_metric == 0
AND exemptions_removed_without_runtime_evidence == 0
AND unresolved_dead_content_families == 0
AND content_acceptance_gate == pass
AND content_utilization_selftest == pass
AND narrative_checks == pass
AND catalog_registry_check == pass
AND release_gate == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 38. Implementer Handoff

1. Regenerate the current content-utilization and reachability evidence first.
2. Re-count root-array families before accepting any of them.
3. Treat this as a content-consumption pass, not a system-design pass.
4. If a family needs a new gameplay authority, stop and create Wave-8 work.
5. Build selectors from real state predicates; use RNG only for ties.
6. Add developer selection traces so low reachability can be debugged.
7. Never let flavor be the only warning for a mechanical risk.
8. Keep hidden place/medical/social knowledge gated behind real channels.
9. Route all prose through localization/overlay rails.
10. Make clinical texts part of diagnosis/autopsy/study—not omniscient UI.
11. Make logs, journals, and memorial records discoverable/owned history.
12. Preserve long-run retention limits.
13. Route every choice through the existing resolver/effect applier.
14. Consolidate questline ownership without renaming save-stable IDs unnecessarily.
15. Give each small ritual/collection family one honest consumer.
16. Do not manufacture gameplay effects for collection/presentation content.
17. Measure reachability per family.
18. Fix or archive impossible definitions.
19. Remove exemptions only after acceptance + runtime + reachability proof.
20. Publish Wave-7 before/after evidence from generated artifacts.
21. Close only when the dead-content bucket stays empty because the gate—not a cleanup document—keeps it empty.

---

# 39. Final Outcome

When this plan is complete, the authored backlog stops being invisible cargo.

Places read differently because the weather, season, territory, radiation state, survey knowledge, and history of that place are actually different. The same depot does not receive a random “atmosphere roll”; its text is selected from the facts the world already owns. Flavor adds texture without becoming the only place a hazard is communicated.

The ward becomes knowledgeable rather than omniscient. Clinical text is unlocked through study, autopsy, care, and experience. The archive contains logs and records the crew actually found or produced. Memorials vary because deaths varied. Long campaigns keep history without storing an unbounded wall of prose.

The encounter corpus becomes playable through one resolver. Moral choices mutate the same effect authorities as the rest of the game. Questline families stop competing for ownership. Confessions alter real pair history. Final wishes reach the memorial pipeline. Cassettes remain collectibles. Damaged maps reveal real world knowledge. Trade tells come from stance and trust rather than a dead JSON file.

Every surviving family then has three kinds of proof:

```text
acceptance
+ runtime utilization
+ reachability
```

And Wave 7 closes with a generated evidence table, not a promise.

The result is not more content.

It is the authored content ASHFALL already paid for finally arriving at the player through the systems that were built to carry it.
