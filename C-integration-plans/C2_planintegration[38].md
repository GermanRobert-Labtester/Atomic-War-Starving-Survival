# C2 — Flagship Integration Plan [38]: Dreams, Sleep Events, Trauma-Linked Nightmares, Memory Processing, and Survivor Inner Life

> **Deliverable:** `C2_planintegration[38].md`
> **Source scope:** Plan 177 — *Dream & Sleep Event System*
> **Primary objective:** create a deterministic dream/sleep-event layer in which survivor sleep can produce peaceful dreams, nightmares, memory dreams, surreal dreams, and rare prophetic/foreshadowing dreams; make those events meaningfully reflect recent experience, trauma, morale, grief, shelter conditions, and known future risks; and route all resulting sleep, fatigue, morale, trauma, mental-health, memory, and moral-choice consequences through the canonical systems that already own those facts.
> **Required execution order:** **177A Foundation/System Contract → 177B Dream/Nightmare/Sleep/Interpretation Content → 177C Cross-System Integration, Save/CI, Exploit Control, Balance, and Closure**
> **Hard dependencies:** `GuiltInsomniaSystem`, `CombatTraumaSystem`, `SomaticFlashbackSystem`, `MentalHealthCrisisSystem`, `NeedsSystem`, `MoralChoiceSystem`, survivor memory/history when Plan 147 is available, Plan 31 semantic event vocabulary, Plan 36 port-contract discipline, Plan 39 save durability, Plan 55 retention.
> **Scope discipline:** no second fatigue meter, no second insomnia authority, no duplicate trauma/PTSD diagnosis state, no dream-local morale ledger, no dream-local survivor memory graph, no UI-driven dream processing, no guaranteed “nightmares heal trauma” rule, no magical future-state mutation from prophetic dreams, no unseeded dream rerolls, no dream-frequency farming through repeated sleep toggles, and no dream journal that silently duplicates the entire personal journal system.

---

# 0. Executive Intent

ASHFALL already models several parts of waking psychological pressure:

- guilt-driven insomnia,
- combat trauma,
- somatic flashbacks,
- mental-health crises,
- morale,
- fatigue/rest,
- survivor memory/history,
- moral-choice consequences.

But sleep itself remains mechanically thin.

Current shape:

```text
survivor becomes tired
→ survivor sleeps
→ fatigue/rest changes
```

Target shape:

```text
survivor enters real sleep cycle
        │
        ├─ shelter conditions
        ├─ stress / morale
        ├─ trauma / grief
        ├─ recent canonical memories
        ├─ insomnia pressure
        └─ authoritative risk/forecast context
        │
        ▼
      DreamSystem
        │
        ├─ no remembered dream
        ├─ peaceful dream
        ├─ nightmare
        ├─ memory dream
        ├─ surreal dream
        └─ prophetic/foreshadowing dream
        │
        ▼
   DreamOutcomeRecord
        │
        ├────────► sleep quality / rest modifier
        ├────────► morale consequence
        ├────────► trauma/mental-health hook
        ├────────► memory/grief processing hook
        ├────────► insight/known-risk hint
        └────────► interpretation opportunity
```

The system should create **inner life**, not a second psychology engine.

The strongest product outcome is:

> **A survivor’s dreams feel causally connected to what they have endured, feared, loved, lost, and learned. Nightmares make poor sleep and trauma visible, peaceful dreams can reinforce recovery, memory dreams revisit real history, surreal dreams add character texture, and rare prophetic dreams foreshadow risks the simulation already knows about—without dreams becoming a magical spoiler system or a free source of buffs.**

---

# 1. Source Diagnosis

The source establishes:

- no `DreamSystem` currently exists,
- `GuiltInsomniaSystem.cs` handles sleep disruption but not dream content,
- `CombatTraumaSystem.cs` owns combat trauma,
- `SomaticFlashbackSystem.cs` owns flashback behavior,
- `MentalHealthCrisisSystem.cs` owns mental-health crisis handling,
- `MoralChoiceIds.cs` already references `ComfortNightmare`,
- five dream types are required:
  - peaceful,
  - nightmare,
  - prophetic,
  - memory,
  - surreal,
- dream triggers include:
  - trauma,
  - morale,
  - recent combat,
  - recent death,
  - random/surreal eligibility,
- dream effects include:
  - morale changes,
  - rest/sleep-quality changes,
  - trauma processing,
  - insight,
  - emotional processing,
  - creativity/confusion,
- dream interpretation is required,
- 30 templates are required:
  - 6 per dream type,
- old saves, deterministic `ISeededRng`, headless behavior, UI, quests, events, and `--dream-selftest` are required.

Three source statements need architectural hardening.

First:

```text
nightmare → trauma reduction
```

should not be treated as a guaranteed clinical truth.

Prefer:

```text
nightmare
→ trauma-processing opportunity/context
→ canonical mental-health system decides bounded effect
```

Some nightmares may reduce unresolved processing pressure; others may worsen sleep or symptoms.

Second:

```text
prophetic dream → warns of future raids/storms
```

must not invent future simulation truth.

Prefer:

```text
authoritative scheduled/forecastable risk
→ dream may foreshadow an existing risk
```

Third:

```text
dream journal
```

should be a filtered dream-history surface, not a second journal authority.

---

# 2. Program-Level Success Criteria

C2[38] closes only when all of the following are true.

1. `DreamSystem.cs` exists and has versioned capture/restore support.
2. Exactly five source dream types are implemented.
3. `dream_templates.json` contains 30 valid templates, six per type.
4. Dreams are generated only from real sleep cycles.
5. Dream selection is deterministic under `ISeededRng`.
6. Reopening UI cannot create or reroll a dream.
7. Save/load cannot reroll a committed dream.
8. Dream triggers consume canonical trauma/morale/recent-event facts.
9. Sleep quality is calculated once through a clear owner/seam.
10. `NeedsSystem` remains canonical owner of fatigue/rest.
11. `GuiltInsomniaSystem` remains canonical insomnia owner.
12. `CombatTraumaSystem` remains canonical combat-trauma owner.
13. `MentalHealthCrisisSystem` remains canonical mental-health owner.
14. `SomaticFlashbackSystem` remains canonical flashback owner.
15. DreamSystem never stores a duplicate PTSD diagnosis.
16. Nightmare effects are bounded and routed through canonical psychological systems.
17. Peaceful dreams do not become guaranteed daily morale farms.
18. Memory dreams reference real memories/events where available.
19. Prophetic dreams only foreshadow authoritative future/forecastable risks.
20. Surreal dreams do not permanently mutate cognition with a dream-local stat.
21. Interpretation is a real, idempotent action.
22. Interpretation does not rewrite the original dream.
23. Uninterpretable dreams remain valid.
24. Dream effects are explainable by reason codes.
25. Dream history is retention-aware.
26. Old saves load with empty dream-specific history.
27. No-sleep/no-dream edge cases are valid.
28. Active-dreamer/high-frequency scenarios remain bounded.
29. Headless simulation produces dreams without UI.
30. `--dream-selftest` validates generation, effects, interpretation, save/load, and integration.

---

# 3. Architectural Invariants

## 3.1 Sleep/fatigue authority remains outside DreamSystem

DreamSystem consumes:

```text
sleep cycle started
sleep cycle ended
sleep context
```

and produces:

```text
sleep-quality modifier / dream outcome
```

`NeedsSystem` applies final fatigue/rest.

## 3.2 Insomnia remains `GuiltInsomniaSystem` truth

DreamSystem can contribute:

- poor-sleep context,
- nightmare history,
- sleep disruption signal.

It does not own insomnia state.

## 3.3 Trauma remains canonical

DreamSystem never writes arbitrary:

```text
trauma -= 10
```

It sends a bounded processing/reaction intent to the trauma/mental-health owner.

## 3.4 Dreams are not memories

A dream may reference memories.

It does not become a second memory graph.

## 3.5 Dream history is immutable once experienced

Interpretation may append:

- interpretation state,
- meaning tags,
- follow-up reference.

It does not change what dream occurred.

## 3.6 Prophecy is foreshadowing, not omniscience

The dream may reference only:

- already scheduled deterministic events,
- already forecastable weather,
- known rising threat indicators,
- unresolved canonical quest/event risks.

## 3.7 No dream effect is applied twice

Stable dream/effect IDs.

## 3.8 No UI tick ownership

Dreams process headlessly during canonical sleep.

## 3.9 Templates are static data

Do not persist template definitions in every save.

## 3.10 Dream frequency is derived/configured, not a drifting arbitrary counter

Persist only actual history/cooldown needed to enforce cadence.

---

# 4. Dependency Graph

```text
sleep cycle / rest context
         │
         ▼
     DreamSystem
         │
   ┌─────┼──────────────┬───────────────┐
   ▼     ▼              ▼               ▼
trauma morale       recent events   shelter/sleep context
   │     │              │               │
   └─────┴──────────────┴───────────────┘
         │
         ▼
template eligibility
         │
         ▼
seeded dream selection
         │
         ▼
DreamOutcomeRecord
         │
    ┌────┼────────┬─────────┬────────────┐
    ▼    ▼        ▼         ▼            ▼
 Needs Insomnia MentalHealth Memory    MoralChoice
```

---

# 5. Baseline Capture

Before implementation, inspect and record:

- `GuiltInsomniaSystem` sleep-disruption ownership,
- `NeedsSystem` fatigue/rest/sleep APIs,
- `CombatTraumaSystem` trauma query/write APIs,
- `SomaticFlashbackSystem` processing/trigger APIs,
- `MentalHealthCrisisSystem` crisis/modifier APIs,
- `MoralChoiceSystem` nightmare quest integration,
- `ComfortNightmare` usage,
- survivor event/memory/history sources,
- shelter warmth/safety/noise/sleep-condition APIs,
- campaign event scheduler/forecast APIs for prophetic foreshadowing,
- save-section registration,
- current journal/UI patterns.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Capture baseline fixtures:

```text
low trauma / high morale
high trauma / low morale
recent combat
recent death
insomnia
good shelter sleep
poor shelter sleep
```

---

# 6. Workstream 177A — Foundation / System Contract

## Goal

Create one deterministic dream-event authority that sits on top of the existing sleep and psychology systems without duplicating them.

---

# 7. 177A Phase A — Create `DreamSystem`

Path:

```text
Assets/Ashfall.Core/Survivors/DreamSystem.cs
```

Responsibilities:

- listen to canonical sleep cycles,
- build dream eligibility context,
- select zero/one dream event for the configured sleep window,
- resolve survivor-specific content parameters,
- commit dream outcome,
- emit bounded effect intents,
- manage interpretation state,
- expose dream-history read models,
- capture/restore dream-specific state.

---

# 8. 177A Phase B — Definition vs Runtime Split

Static:

```text
DreamTemplate
DreamTriggerDefinition
DreamEffectDefinition
DreamInterpretationDefinition
```

Runtime:

```text
DreamOccurrence
SleepEventRecord
DreamEffectApplication
DreamInterpretationRecord
DreamState
```

---

# 9. 177A Phase C — `DreamTemplate`

Recommended fields:

```text
template_id
dream_type
rarity
trigger_expression
required_memory_tags
required_event_kinds
forbidden_conditions
content_key
content_parameters
effect_profile_ids
interpretation_profile_id
cooldown_group
weight
```

Do not store rendered prose as the only data authority.

---

# 10. 177A Phase D — `DreamOccurrence`

Recommended fields:

```text
dream_id
survivor_id
template_id
dream_day
sleep_cycle_id
dream_type
resolved_context_refs
remembered
effect_application_ids
interpretation_state
```

Avoid persisting full duplicate trauma/morale values.

Historical snapshot of trigger bands may be stored only for explainability.

---

# 11. 177A Phase E — `SleepEventRecord`

Source event types:

```text
RestfulSleep
Insomnia
Nightmare
Dream
SleepTalking
Sleepwalking
```

Do not let DreamSystem own every sleep event if some are already owned elsewhere.

Recommended split:

- insomnia remains `GuiltInsomniaSystem`,
- DreamSystem may record an external insomnia event reference,
- sleep talking/sleepwalking require actual content/effect support or remain deferred.

---

# 12. 177A Phase F — Scope Sleep Talking / Sleepwalking Carefully

Source DTO names them but does not otherwise define their gameplay.

Do not implement shallow placeholders merely to fill enum values.

Options:

1. include as future-supported event types with no authored templates yet, or
2. add minimal bounded templates after integration owner audit.

Do not let them block the five required dream types.

---

# 13. 177A Phase G — `DreamState`

Persist:

```text
dream occurrences
interpretation records
per-survivor last-dream day/cooldown
effect idempotency keys
optional recent sleep-event references
schema version
retention summaries
```

Do not persist:

```text
trauma processed total as independent truth
dream frequency as mutable gameplay truth
fatigue
morale
insomnia
```

---

# 14. 177A Phase H — Dream Type Vocabulary

Exactly:

```text
Peaceful
Nightmare
Prophetic
Memory
Surreal
```

Stable IDs.

---

# 15. 177A Phase I — Dream Rarity

Source:

```text
Common
Uncommon
Rare
```

Use rarity as one selection-weight dimension.

It is not enough by itself to make a dream eligible.

---

# 16. 177A Phase J — Dream Trigger Context

Build immutable context per sleep cycle:

```text
survivor_id
sleep_cycle_id
day
trauma band
morale band
stress
recent combat refs
recent death/grief refs
recent moral-choice refs
recent positive-event refs
shelter sleep conditions
insomnia context
authoritative forecast/risk refs
memory candidates
```

---

# 17. 177A Phase K — No Live State Mutation During Eligibility Scan

Eligibility reads state.

Mutation occurs only after one dream is committed.

---

# 18. 177A Phase L — High Trauma Trigger

High trauma raises eligibility/weight for nightmare templates.

Do not guarantee nightmare every sleep.

---

# 19. 177A Phase M — Low Morale Trigger

Low morale may:

- raise nightmare/memory weights,
- lower peaceful-dream weights.

Use bounded weighting.

---

# 20. 177A Phase N — Recent Combat Trigger

Recent canonical combat event enables combat-nightmare/memory templates.

No string scanning of journal prose.

---

# 21. 177A Phase O — Recent Death Trigger

Recent canonical survivor death/grief event enables grief-memory/nightmare templates.

---

# 22. 177A Phase P — High Morale Trigger

High morale raises peaceful/positive-memory eligibility.

---

# 23. 177A Phase Q — Surreal Eligibility

Surreal dreams may require fewer state predicates.

Still enforce:

- cadence,
- rarity,
- no competing stronger state-driven template if policy prefers meaningful dreams.

---

# 24. 177A Phase R — Prophetic Eligibility

Prophetic templates require an authoritative foreshadowing source.

Examples:

```text
storm scheduled/forecastable
raid threat already scheduled or above known threshold
known expedition hazard not yet revealed in exact detail
resource-location clue already present in information system
```

No free prediction of uncommitted RNG.

---

# 25. 177A Phase S — Future-Event Reference Contract

A prophetic dream stores:

```text
foreshadow_source_id
```

not a copied event implementation.

If source event is later cancelled/changed by authoritative gameplay:

- dream remains historical,
- interpretation can remain symbolic,
- no forced event resurrection.

---

# 26. 177A Phase T — Sleep Quality Ownership Decision

The source proposes a sleep-quality system.

Before implementation, inspect whether sleep quality already exists.

Preferred architecture:

```text
SleepQualityAssessment
```

derived from:

- warmth,
- safety,
- stress,
- trauma,
- insomnia,
- shelter conditions,
- dream/nightmare outcome.

One designated sleep/rest owner applies it.

---

# 27. 177A Phase U — `SleepQualityAssessment`

Fields:

```text
base_quality
environment_modifier
stress_modifier
insomnia_modifier
dream_modifier
final_quality
reason_codes
```

Do not persist as a second long-lived stat unless current sleep system requires it.

---

# 28. 177A Phase V — Quality Range

Bound:

```text
0..100
```

Example display bands:

```text
Awful
Poor
Fair
Good
Restful
```

Exact thresholds data/config.

---

# 29. 177A Phase W — Good Sleep

Good sleep contributes a rest-efficiency multiplier to `NeedsSystem`.

DreamSystem does not restore fatigue directly.

---

# 30. 177A Phase X — Poor Sleep / Insomnia

Poor sleep may:

- reduce rest efficiency,
- contribute to insomnia context.

`GuiltInsomniaSystem` owns final insomnia state.

---

# 31. 177A Phase Y — Nightmare Sleep Penalty

Nightmare outcome contributes:

```text
dream_modifier < 0
```

to sleep-quality assessment.

No duplicate fatigue subtraction.

---

# 32. 177A Phase Z — Peaceful Sleep Bonus

Peaceful dream may contribute small positive sleep modifier.

Cap so it cannot produce unlimited rest beyond canonical rest ceiling.

---

# 33. 177A Phase AA — Deterministic RNG Stream

Use:

```text
dreams
```

or a named substream.

Seed key:

```text
campaign seed
sleep cycle ID
survivor ID
dream selection phase
```

---

# 34. 177A Phase AB — Stable Candidate Ordering

Sort by:

```text
template_id
```

before weighted selection.

---

# 35. 177A Phase AC — Dream Frequency Budget

Do not generate a remembered dream every sleep.

Define:

- base remembered-dream probability,
- state modifiers,
- per-survivor cooldown,
- max remembered dreams per N days.

---

# 36. 177A Phase AD — Dream vs Remembered Dream

A survivor may dream but not remember it.

For gameplay simplicity:

```text
no recorded dream
```

can represent unremembered ordinary dreaming.

Do not generate invisible records endlessly.

---

# 37. 177A Phase AE — Active Dreamer Profile

If a trait or survivor identity supports vivid dreaming:

- increase remembered-dream frequency within cap.

No universal 100% rate.

---

# 38. 177A Phase AF — Dream Content Resolution

Use:

```text
localization key
+ survivor/event/memory parameters
```

Avoid embedding large resolved English strings in save.

---

# 39. 177A Phase AG — Survivor-Specific Parameters

Possible:

- survivor name,
- deceased survivor name,
- location,
- recent event,
- faction,
- object/keepsake.

Only reference facts the survivor plausibly knows/remembers.

---

# 40. 177A Phase AH — Memory Candidate Interface

Create:

```text
IDreamMemorySource
```

to retrieve stable memory/event references.

If Plan 147 memory is unavailable:

- use canonical recent-event/history sources,
- do not build a duplicate memory store here.

---

# 41. 177A Phase AI — Dream Effects as Intents

Effects:

```text
MoraleEffectIntent
SleepQualityEffect
TraumaProcessingIntent
MentalHealthEffectIntent
InsightHook
Creativity/ConfusionEffectIntent
```

Owning systems apply durable state.

---

# 42. 177A Phase AJ — Nightmare Trauma-Processing Contract

Replace:

```text
nightmare = guaranteed trauma reduction
```

with:

```text
nightmare may create trauma-processing intent
```

Outcome depends on:

- trauma system policy,
- severity,
- recurrence,
- support/interpretation,
- current mental state.

Possible result:

- small relief,
- no change,
- temporary worsening.

---

# 43. 177A Phase AK — Frequent Nightmares / Plan 179

Frequent nightmares can emit:

```text
nightmare_frequency_signal
```

Plan 179 may consume this for PTSD-related evaluation.

DreamSystem does not diagnose PTSD.

---

# 44. 177A Phase AL — Peaceful Effect Contract

Possible:

- small morale intent,
- sleep-quality bonus,
- positive-memory reinforcement.

Bound daily.

---

# 45. 177A Phase AM — Memory Dream Effect Contract

Memory dreams may:

- trigger grief-processing context,
- small positive/negative morale response,
- relation/memory reflection.

No direct deletion of grief.

---

# 46. 177A Phase AN — Surreal Effect Contract

Source:

```text
creativity boost
confusion
```

Use canonical modifier system if creativity/clarity exists.

If not:

- keep as narrative flavor + small existing stat modifier,
- do not create two new permanent stats solely for dreams.

---

# 47. 177A Phase AO — Prophetic Insight Contract

Insight is:

```text
hint / warning / confidence-limited clue
```

No direct event prevention or spawn.

---

# 48. 177A Phase AP — Insight Visibility

Player sees:

- symbolic clue,
- interpreted hint,
- uncertainty.

Do not show exact hidden schedule timestamp unless world fiction supports precise prophecy.

---

# 49. 177A Phase AQ — Interpretation State

Typed:

```text
Unreviewed
Reviewed
Interpreted
Uninterpretable
```

---

# 50. 177A Phase AR — Interpretation Action

Player can interpret only:

- remembered dream,
- not already interpreted,
- if template allows.

Optional resource/person requirement only if future therapy system exists.

Baseline should not require a separate profession.

---

# 51. 177A Phase AS — Interpretation Meaning

Interpretation can reveal:

- trigger context,
- symbolic theme,
- linked memory/risk,
- possible processing explanation.

It should not reveal hidden exact mental-health numbers unless normal UI already does.

---

# 52. 177A Phase AT — Interpretation Morale

Source says interpretation provides morale bonus.

Make conditional/bounded.

Understanding a nightmare may help; interpreting a disturbing dream may also be neutral.

Avoid universal click-to-buff.

---

# 53. 177A Phase AU — Interpretation Idempotency

Apply once.

Save/load preserves.

---

# 54. 177A Phase AV — `DreamTemplateCatalogLoader`

Path:

```text
Assets/Ashfall.Core/Survivors/DreamTemplateCatalogLoader.cs
```

or canonical data-loader location.

---

# 55. 177A Phase AW — `dream_templates.json`

Path:

```text
Assets/StreamingAssets/Data/dream_templates.json
```

Exactly 30 templates:

```text
6 Peaceful
6 Nightmare
6 Prophetic
6 Memory
6 Surreal
```

---

# 56. 177A Phase AX — Template Integrity

Validate:

- unique ID,
- valid dream type,
- rarity,
- trigger refs,
- effect profiles,
- localization key,
- memory/event refs,
- interpretation profile.

---

# 57. 177A Phase AY — `GameBootstrap` Integration

Source names:

```text
SetupDreams
TickDreams
SaveDreams
```

Follow repository’s current composition-root conventions.

If Plan 28 removed direct ad hoc setup methods:

- register through canonical descriptor/composition system,
- do not reintroduce architecture drift just to match old naming.

---

# 58. 177A Phase AZ — Tick Contract

Do not tick dream selection every frame/day independent of sleep.

Trigger on canonical:

```text
sleep cycle completion / sleep-resolution event
```

---

# 59. 177A Phase BA — Save Versioning

Version:

```text
DreamState schema
```

Migrate deterministically.

---

# 60. 177A Phase BB — Old Save Compatibility

Missing dream section:

```text
valid
→ empty dream history
→ no interpretation state
→ no dream cooldown history
```

Avoid immediate burst of catch-up dreams.

---

# 61. 177A Phase BC — Migration Grace

On first load of old save:

- dream cadence begins from current day,
- no retroactive dream generation.

---

# 62. 177A Phase BD — Semantic Events

Candidate kinds:

```text
dream_experienced
nightmare_experienced
peaceful_dream_experienced
prophetic_dream_experienced
memory_dream_experienced
surreal_dream_experienced
dream_interpreted
restful_sleep_resolved
```

Use Plan 31 governance.

---

# 63. 177A Phase BE — Port Contract

Mandatory:

- sleep-cycle source,
- Needs/rest sink,
- morale sink,
- trauma/mental-health sink,
- save service.

Conditional:

- memory source,
- MoralChoice hook,
- future Plan 179 PTSD signal,
- archive/journal.

---

# 64. 177A Phase BF — Diagnostics

Expose:

```text
DREAM_TEMPLATES_TOTAL
DREAMS_EXPERIENCED
NIGHTMARES_EXPERIENCED
PROPHECY_DREAMS_EXPERIENCED
DREAMS_INTERPRETED
DREAM_EFFECT_DUPLICATES
DREAM_REQUIRED_PORTS_MISSING
```

---

# 65. 177A Tests

- template count/type distribution,
- deterministic selection,
- no-dream outcome,
- cooldown,
- sleep-only triggering,
- high-trauma weight,
- recent-combat trigger,
- recent-death trigger,
- prophetic-source requirement,
- interpretation idempotency,
- old-save empty state,
- no duplicate psychology state.

---

# 66. 177A Definition of Done

- [ ] `DreamSystem.cs`,
- [ ] static/runtime split,
- [ ] five dream types,
- [ ] rarity,
- [ ] sleep-context builder,
- [ ] deterministic RNG,
- [ ] dream-frequency budget,
- [ ] sleep-quality assessment seam,
- [ ] effect-intent architecture,
- [ ] safe nightmare processing,
- [ ] prophetic-source contract,
- [ ] memory-source adapter,
- [ ] interpretation,
- [ ] catalog loader,
- [ ] 30-template data authority,
- [ ] composition-root integration,
- [ ] save/versioning,
- [ ] old-save grace,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 67. Workstream 177B — Dream / Nightmare / Sleep / Interpretation Content

## Goal

Author and implement the five dream families, make them meaningfully survivor-specific, build interpretation and dream-history UI, and create events/quests without turning the system into repetitive notification spam.

---

# 68. 177B Phase A — Dream Generation Pipeline

For each completed eligible sleep cycle:

```text
build context
→ decide whether a remembered dream occurs
→ gather eligible templates
→ weight by current state
→ seeded select
→ resolve context
→ commit dream
→ emit effect intents
→ record history
```

Exactly once.

---

# 69. 177B Phase B — No Eligible Template

If no template matches:

```text
no remembered dream
```

This is valid.

Do not fall back to an unrelated surreal dream every time.

---

# 70. 177B Phase C — Peaceful Dream Family

Core themes:

- safety,
- warmth,
- recovered normality,
- loved ones alive,
- food abundance,
- pre-war ordinary life,
- successful shelter future.

Keep setting-consistent.

---

# 71. 177B Phase D — Peaceful Trigger Profile

Weight increases with:

- high morale,
- low/moderate trauma,
- recent positive social event,
- safety/warmth,
- strong relationships.

---

# 72. 177B Phase E — Peaceful Effects

Bounded:

- positive morale intent,
- small sleep-quality bonus,
- possible positive memory reinforcement.

No permanent trait from ordinary peaceful dream.

---

# 73. 177B Phase F — Peaceful Template 1: Warm Kitchen

Context:

- shelter/household memory,
- food security.

Use generic safe-memory references where possible.

---

# 74. 177B Phase G — Peaceful Template 2: Summer Before Fallout

Context:

- pre-war ordinary life.

Avoid asserting personal biography not authored for survivor.

Use generic sensory motifs unless background data supports specifics.

---

# 75. 177B Phase H — Peaceful Template 3: Everyone at the Table

Requires:

- meaningful social/family network.

Do not name absent/dead people incorrectly.

---

# 76. 177B Phase I — Peaceful Template 4: Green Again

Can symbolize:

- recovery,
- agriculture,
- environmental hope.

No literal future promise.

---

# 77. 177B Phase J — Peaceful Template 5: Safe Night

Context:

- recent danger ended,
- shelter security restored.

---

# 78. 177B Phase K — Peaceful Template 6: Familiar Voice

Can use living loved-one/friend reference.

If none available:

- template not eligible.

---

# 79. 177B Phase L — Nightmare Family

Nightmares should feel linked to unresolved pressure.

Themes:

- combat,
- death,
- guilt,
- radiation,
- starvation,
- entrapment.

Avoid generic horror disconnected from state when stronger context exists.

---

# 80. 177B Phase M — Nightmare Trigger Profile

Weight increases with:

- high trauma,
- low morale,
- guilt,
- recent combat,
- recent death,
- mental-health crisis,
- poor shelter sleep conditions.

---

# 81. 177B Phase N — Nightmare Effects

Possible:

- sleep-quality penalty,
- morale penalty,
- trauma-processing intent,
- flashback-pressure signal,
- Plan 179 nightmare-frequency signal.

No guaranteed healing.

---

# 82. 177B Phase O — Nightmare Template 1: The Corridor

Requires:

- combat/raid/evacuation memory.

---

# 83. 177B Phase P — Nightmare Template 2: The Door That Would Not Open

Requires:

- guilt/failed rescue/locked-out survivor context.

---

# 84. 177B Phase Q — Nightmare Template 3: Geiger in the Dark

Requires:

- radiation exposure or radiation fear context.

---

# 85. 177B Phase R — Nightmare Template 4: Empty Bowls

Requires:

- starvation/food crisis memory.

---

# 86. 177B Phase S — Nightmare Template 5: The Name on the Wall

Requires:

- recent death/memorial/grief context.

---

# 87. 177B Phase T — Nightmare Template 6: Buried Alive

Requires:

- collapse/tunnel/disaster/entrapment context if available.

Fallback eligibility should not fabricate event history.

---

# 88. 177B Phase U — Memory Dream Family

Memory dreams replay or transform known past events.

They are the strongest case for integration with Plan 147.

---

# 89. 177B Phase V — Memory Trigger Sources

Candidate memories:

- major moral choice,
- partner/family moment,
- survivor death,
- first arrival,
- major victory,
- loss,
- celebration.

---

# 90. 177B Phase W — Memory Selection

Use:

- recency,
- emotional salience,
- unresolved grief/guilt,
- relationship importance.

Do not select arbitrary low-value event if a high-salience unresolved event exists.

---

# 91. 177B Phase X — Memory Effects

Possible:

- grief-processing intent,
- guilt-processing intent,
- positive morale from joyful memory,
- negative morale from painful memory,
- relation-memory reinforcement.

---

# 92. 177B Phase Y — Memory Template 1: The Last Conversation

Requires:

- deceased relationship memory.

---

# 93. 177B Phase Z — Memory Template 2: Before the Sirens

Requires:

- pre-war/background memory only if authored.

Otherwise use generic social memory.

---

# 94. 177B Phase AA — Memory Template 3: The Choice

Requires:

- significant moral-choice event.

---

# 95. 177B Phase AB — Memory Template 4: The Return

Requires:

- expedition reunion/survival event.

---

# 96. 177B Phase AC — Memory Template 5: The Celebration

Requires:

- canonical positive community event.

---

# 97. 177B Phase AD — Memory Template 6: The Empty Bed

Requires:

- grief/loss context.

---

# 98. 177B Phase AE — Surreal Dream Family

Purpose:

- character texture,
- creative disorientation,
- low-stakes variety.

Should still be setting-compatible.

---

# 99. 177B Phase AF — Surreal Trigger Profile

Eligible when:

- no dominant trauma/memory trigger,
- dream cadence permits,
- surreal rarity weight wins.

May also be influenced by:

- fatigue,
- unusual food/medication effects if canonical.

Do not invent pharmacology.

---

# 100. 177B Phase AG — Surreal Effects

Use only existing canonical modifiers.

Possible:

- small creativity/work ideation modifier,
- short confusion/focus modifier.

If no such system:

```text
narrative only
```

is acceptable.

---

# 101. 177B Phase AH — Surreal Template 1: Snow Indoors

---

# 102. 177B Phase AI — Surreal Template 2: Talking Radio

Can reference real shelter radio identity without implying sentience.

---

# 103. 177B Phase AJ — Surreal Template 3: Endless Stairwell

---

# 104. 177B Phase AK — Surreal Template 4: Purple Sun

Fits nuclear surrealism.

---

# 105. 177B Phase AL — Surreal Template 5: The Room Behind the Wall

Should not automatically reveal a real secret location.

---

# 106. 177B Phase AM — Surreal Template 6: Everyone Wears Another Face

Use generic identities, avoid false relationships.

---

# 107. 177B Phase AN — Prophetic Dream Family

Treat as:

```text
foreshadowing / intuition / subconscious pattern recognition
```

unless supernatural canon is explicitly established.

---

# 108. 177B Phase AO — Prophetic Source Categories

Allowed:

- forecast weather,
- scheduled raid/threat,
- known structural risk,
- unresolved faction hostility,
- partially known resource/location clue,
- disaster warning signal.

---

# 109. 177B Phase AP — Prophetic Quality

Dream may encode:

- vague symbol,
- medium-confidence hint,
- clearer interpreted warning.

Never provide more precision than source knowledge/prophecy policy permits.

---

# 110. 177B Phase AQ — Prophetic Template 1: Black Snow

Requires:

- authoritative severe weather/fallout risk.

---

# 111. 177B Phase AR — Prophetic Template 2: Knocking at the Gate

Requires:

- visitor/raid/contact risk already scheduled or thresholded.

---

# 112. 177B Phase AS — Prophetic Template 3: The Broken Wire

Requires:

- power/communications failure risk.

---

# 113. 177B Phase AT — Prophetic Template 4: Water Under the Door

Requires:

- flood/water ingress risk.

---

# 114. 177B Phase AU — Prophetic Template 5: A Red Mark on the Map

Requires:

- existing location/hazard clue.

No new location creation.

---

# 115. 177B Phase AV — Prophetic Template 6: The Empty Shelf

Requires:

- forecastable supply shortage / consumption trajectory.

Use canonical economy/inventory projection if available.

---

# 116. 177B Phase AW — 30-Template Coverage Matrix

Generate:

| Template | Type | Rarity | Trigger domains | Memory/event requirement | Effects | Interpretation | Runtime observed |
|---|---|---|---|---|---|---|---:|

Exactly six per type.

---

# 117. 177B Phase AX — Sleep Quality Inputs

Environmental:

- warmth,
- safety,
- noise if modeled,
- crowding if modeled,
- illness/pain if canonical.

Psychological:

- trauma,
- morale,
- guilt,
- insomnia.

Dream outcome:

- peaceful,
- nightmare,
- neutral.

---

# 118. 177B Phase AY — Avoid Double Penalties

If insomnia already reduces sleep quality:

- DreamSystem reads that modifier once.

Do not apply:

```text
insomnia penalty
+ another identical “stress penalty”
```

for same cause without reason.

---

# 119. 177B Phase AZ — Rest Bonus Mapping

Map final sleep quality to `NeedsSystem` rest efficiency.

Example:

```text
0–20 → severe rest loss
21–40 → poor
41–60 → partial
61–80 → good
81–100 → full/capped bonus
```

Exact values balance-tested.

---

# 120. 177B Phase BA — No Over-Rest

Sleep quality cannot restore more fatigue than canonical sleep/rest maximum unless another system explicitly allows bonus rest.

---

# 121. 177B Phase BB — Interpretation UI Flow

```text
Dream Journal
→ select dream
→ read detail
→ review known context
→ Interpret
→ reveal meaning
→ apply bounded interpretation consequence once
```

---

# 122. 177B Phase BC — Interpretation Reason Codes

Examples:

```text
linked_to_recent_combat
linked_to_grief
linked_to_guilt
symbolizes_shelter_safety
foreshadows_known_storm
too_surreal_to_interpret
```

---

# 123. 177B Phase BD — Interpretation Without Omniscience

Interpretation can reveal:

```text
"This may reflect your fear after the raid."
```

not:

```text
"Trauma exactly decreased by 7."
```

unless player UI normally exposes exact numbers.

---

# 124. 177B Phase BE — Uninterpretable Dreams

Some surreal templates:

```text
interpretation_allowed = false
```

Still valid history.

---

# 125. 177B Phase BF — Reinterpretation

Baseline:

```text
one interpretation
```

No repeated morale clicks.

Follow-on therapy plan may revisit later.

---

# 126. 177B Phase BG — Dream Event: “The Dream”

Generic first/meaningful dream event.

Avoid firing for every routine dream.

---

# 127. 177B Phase BH — “The Nightmare”

Trigger on significant nightmare.

---

# 128. 177B Phase BI — “The Prophecy”

Trigger only for rare prophetic dream.

---

# 129. 177B Phase BJ — “The Memory”

Trigger for emotionally salient memory dream.

---

# 130. 177B Phase BK — “The Surreal”

Trigger only for notable surreal dream, not every one.

---

# 131. 177B Phase BL — “The Interpretation”

First/significant interpretation milestone.

---

# 132. 177B Phase BM — “The Rest”

Significant perfect/restful sleep achievement.

---

# 133. 177B Phase BN — Quest Hook: “The Dreamer”

Experience 10 remembered dreams.

Count canonical dream history.

---

# 134. 177B Phase BO — Quest Hook: “The Nightmare”

Source says survive nightmare trauma processing.

Define objective in terms of:

- nightmare experienced,
- survivor remains active,
- canonical processing outcome resolved.

No guaranteed trauma reduction needed.

---

# 135. 177B Phase BP — Quest Hook: “The Prophet”

Receive one valid prophetic dream.

---

# 136. 177B Phase BQ — Quest Hook: “The Interpreter”

Interpret 5 eligible dreams.

---

# 137. 177B Phase BR — Quest Hook: “The Rest”

Achieve configured top sleep-quality band.

---

# 138. 177B Phase BS — Quest Hook: “The Processing”

Resolve a defined number of trauma-processing dream hooks through canonical trauma system.

---

# 139. 177B Phase BT — Quest Hook: “The Insight”

Gain one dream-linked canonical insight.

---

# 140. 177B Phase BU — Dream Journal Architecture

Prefer:

```text
DreamHistoryPanel
```

or a Dream filter inside journal.

Do not create a second generic JournalSystem.

---

# 141. 177B Phase BV — Dream Journal List

Show:

- survivor,
- day,
- dream type,
- remembered/interpreted status,
- short title.

---

# 142. 177B Phase BW — Dream Detail

Show:

- localized narrative,
- type,
- effects already applied,
- interpretation state,
- linked memory/event if player-known.

---

# 143. 177B Phase BX — Sleep Quality Display

Show latest/current sleep-quality assessment in survivor sleep/needs surface.

Do not persist UI-only duplicate.

---

# 144. 177B Phase BY — Dream Notification

Notification:

```text
Survivor remembered a dream
```

with severity/importance filtering.

Do not interrupt player for every common dream.

---

# 145. 177B Phase BZ — Dream Tutorial

First remembered dream:

Explain:

- dreams reflect survivor state,
- some affect rest/morale,
- interpretation is optional,
- not every sleep produces a remembered dream.

---

# 146. 177B Phase CA — Tooltips

Dream type/effect tooltip uses read model.

No hidden future-event spoiler.

---

# 147. 177B Phase CB — Localization

Automatic dream content uses localization templates.

Survivor/event parameters localize correctly.

---

# 148. 177B Phase CC — Content Tone Standard

Dream prose should be:

- concise,
- sensory,
- survivor-specific where supported,
- ambiguous enough to feel dreamlike,
- not omniscient exposition.

---

# 149. 177B Phase CD — Avoid Diagnosis Language in Dream Text

Do not write:

```text
"This dream proves PTSD."
```

Prefer:

```text
"The same corridor keeps returning."
```

Diagnosis belongs to mental-health systems.

---

# 150. 177B Phase CE — Avoid Fake Biography

Templates may not invent:

- spouse,
- child,
- hometown,
- profession,
- religion,
- past event

unless survivor data supports it.

---

# 151. 177B Phase CF — Memory-Safe Substitution

If optional context parameter missing:

- choose another eligible template,
- or use neutral version.

Do not substitute false relationship.

---

# 152. 177B Phase CG — Dream Frequency UX

Target:

- enough dreams to build character,
- not every night,
- nightmares noticeable under trauma,
- prophetic rare.

Balance through soak.

---

# 153. 177B Phase CH — Template Utilization

Run representative survivor profiles.

Report:

```text
templates loaded
templates eligible
templates selected
never eligible
never selected
```

---

# 154. 177B Phase CI — Dead Template Policy

Never-observed template:

- fix trigger,
- mark intentionally rare,
- remove,
- exempt with reason.

---

# 155. 177B Definition of Done

- [ ] dream generation pipeline,
- [ ] 6 peaceful templates,
- [ ] 6 nightmare templates,
- [ ] 6 prophetic templates,
- [ ] 6 memory templates,
- [ ] 6 surreal templates,
- [ ] meaningful trigger conditions,
- [ ] survivor-specific parameterization,
- [ ] safe nightmare processing,
- [ ] prophetic source gating,
- [ ] sleep-quality assessment,
- [ ] interpretation,
- [ ] 7 dream events,
- [ ] 7 quest hooks,
- [ ] dream-history UI,
- [ ] sleep-quality display,
- [ ] notifications,
- [ ] tutorial/tooltips,
- [ ] localization,
- [ ] content-tone guardrails,
- [ ] utilization report.

---

# 156. Workstream 177C — Cross-System Integration, Save/CI, Exploit Control, Balance, and Closure

## Goal

Prove dreams are produced by real sleep, affect only canonical downstream systems, remain deterministic across save/load, avoid psychological double-counting, and provide meaningful narrative value without becoming a farmable buff/debuff machine.

---

# 157. 177C Phase A — `GuiltInsomniaSystem` Integration

DreamSystem consumes insomnia context.

Possible flow:

```text
GuiltInsomniaSystem
→ insomnia severity / sleep disruption modifier
→ SleepQualityAssessment
```

Nightmare history may feed back as one input if explicitly supported.

Avoid circular double application.

---

# 158. 177C Phase B — Insomnia Circularity Audit

Potential loop:

```text
insomnia → bad sleep → nightmare → worse sleep → insomnia
```

Bound feedback.

Do not let one nightmare spiral permanently without recovery.

---

# 159. 177C Phase C — `CombatTraumaSystem` Integration

Nightmare eligibility reads trauma.

Nightmare processing emits canonical processing intent.

No local trauma counter.

---

# 160. 177C Phase D — Trauma Effect Idempotency

Each dream effect application has stable ID.

Same nightmare cannot process trauma twice after reload.

---

# 161. 177C Phase E — `MentalHealthCrisisSystem` Integration

Dream effects may emit:

- distress,
- stabilization,
- sleep disruption,
- recurring nightmare signal.

MentalHealth system owns final crisis state.

---

# 162. 177C Phase F — `SomaticFlashbackSystem` Integration

Dream processing can affect flashback pressure only through explicit API.

Do not directly toggle flashback state.

---

# 163. 177C Phase G — `NeedsSystem` Integration

Final sleep quality modifies rest/fatigue.

Test:

```text
same sleep duration
different sleep quality
→ different fatigue recovery
```

---

# 164. 177C Phase H — No Duplicate Rest Application

DreamSystem never calls both:

```text
Needs.AddRest()
```

and:

```text
fatigue -= X
```

through two paths.

One sink.

---

# 165. 177C Phase I — `MoralChoiceSystem` Integration

`ComfortNightmare` quest/reference must point to real nightmare state.

No duplicate nightmare quest state.

---

# 166. 177C Phase J — Plan 147 Memory Integration

If available:

- memory dreams choose from canonical per-NPC memories.

If unavailable:

- use event-history adapter.

No hard dependency that blocks plan if memory system lands later.

---

# 167. 177C Phase K — Plan 179 PTSD Integration

Future optional port:

```text
nightmare-frequency signal
```

No diagnosis or treatment logic here.

---

# 168. 177C Phase L — Morale Integration

Dream effects emit canonical morale reason IDs.

No dream morale ledger.

---

# 169. 177C Phase M — Creativity/Confusion Integration

If canonical work/mental modifiers exist:

- use them.

If not:

- omit mechanical effect and retain narrative flavor.

Do not create permanent new stats just to satisfy template wording.

---

# 170. 177C Phase N — Prophetic Weather Integration

Storm/fallout warning dream can only reference:

- already forecastable/scheduled weather.

If forecast source changes:

- dream remains historical but does not force old forecast.

---

# 171. 177C Phase O — Prophetic Raid Integration

Only if raid/threat system exposes authoritative future-risk object.

Otherwise:

```text
disable raid-prophecy templates
```

rather than invent schedule data.

---

# 172. 177C Phase P — Prophetic Resource Insight

Only if a canonical clue/intel/location-knowledge source exists.

Dream can upgrade:

```text
hint confidence
```

through information system.

Do not create a resource node.

---

# 173. 177C Phase Q — Save/Load Lifecycle Matrix

Save at:

```text
before sleep
sleep started
dream selected
effects pending
dream committed
interpretation pending
interpreted
```

Reload exact state.

---

# 174. 177C Phase R — Dream Selection Idempotency

Once `sleep_cycle_id` has selected/committed a dream:

- same cycle cannot select another.

---

# 175. 177C Phase S — Effect Idempotency

Every effect:

```text
dream_id + effect_profile_id
```

applies once.

---

# 176. 177C Phase T — Interpretation Idempotency

Every interpretation:

```text
dream_id + interpretation_profile_id
```

applies once.

---

# 177. 177C Phase U — Save-Scum Prevention

Reload cannot reroll:

- whether dream occurred,
- type,
- template,
- linked memory,
- prophecy source,
- effect outcome,
- interpretation outcome if already committed.

---

# 178. 177C Phase V — Sleep Toggle Exploit

Prevent:

```text
sleep 5 minutes
wake
sleep again
→ repeated dream rolls
```

Dream eligibility requires canonical qualifying sleep cycle/minimum sleep duration.

---

# 179. 177C Phase W — Dream Farming Exploit

Repeated rest beyond fatigue needs should not grant:

- extra dreams,
- morale,
- trauma processing,
- quest counts

without valid sleep cycle/cooldown.

---

# 180. 177C Phase X — Interpretation Farming Exploit

Interpret once.

No repeated morale.

---

# 181. 177C Phase Y — Prophecy Farming Exploit

Prophetic dream:

- rare,
- tied to authoritative source,
- source can only produce bounded dream count,
- repeated sleep cannot reveal progressively exact spoilers for same event unless authored.

---

# 182. 177C Phase Z — Nightmare Farming Exploit

Player cannot intentionally loop nightmares for trauma reduction.

Because:

- trauma processing not guaranteed,
- cadence/cooldown,
- sleep cost,
- canonical mental-health resolution.

---

# 183. 177C Phase AA — No Sleep Edge Case

Survivor never enters qualifying sleep.

Expected:

```text
zero dreams
```

No background dream generation.

---

# 184. 177C Phase AB — No Dreams Edge Case

A survivor may sleep repeatedly with no remembered dream due to eligibility/cadence.

Valid.

---

# 185. 177C Phase AC — Active Dreamer Edge Case

High-vividness/high-trigger survivor.

Ensure:

- frequency cap,
- history retention,
- no event spam.

---

# 186. 177C Phase AD — High Trauma Edge Case

Nightmares more common.

But still:

- not every night,
- no unbounded morale collapse,
- recovery paths remain.

---

# 187. 177C Phase AE — High Morale Edge Case

Peaceful dreams more likely.

Avoid permanent positive feedback loop:

```text
high morale → peaceful → higher morale forever
```

Use caps/diminishing returns.

---

# 188. 177C Phase AF — Recent Death Edge Case

Grief dream selects real deceased survivor.

If multiple:

- deterministic weighted selection.

---

# 189. 177C Phase AG — No Memory Source Edge Case

Memory dream templates requiring memory become ineligible.

System still functions.

---

# 190. 177C Phase AH — No Authoritative Prophecy Source

Prophetic templates become ineligible.

Do not fabricate prophecy.

---

# 191. 177C Phase AI — Old Save Compatibility

Existing save gets:

```text
empty DreamState
```

No migration error.

No catch-up dreams.

---

# 192. 177C Phase AJ — History Retention

Plan 55:

Keep:

- recent dreams,
- rare prophetic dreams,
- landmark nightmares,
- major memory dreams,
- interpreted dreams with quest/history value.

Roll up:

- low-value common dreams after long horizon.

---

# 193. 177C Phase AK — Dream History Size Budget

Measure:

```text
100 days
1 year
10 years
400 years
```

with active dreamers.

Avoid unbounded resolved narrative text.

---

# 194. 177C Phase AL — `--dream-selftest`

Required source command.

Scenarios:

1. peaceful dream,
2. high-trauma nightmare,
3. recent-combat nightmare,
4. recent-death memory dream,
5. prophetic dream with valid source,
6. prophecy unavailable without source,
7. surreal dream,
8. no-dream sleep,
9. sleep-quality calculation,
10. fatigue integration,
11. interpretation,
12. uninterpretable dream,
13. save/load,
14. duplicate-effect replay,
15. old save,
16. no-sleep case,
17. active-dreamer cap.

---

# 195. 177C Phase AM — Data Integrity

Validate:

- exactly 30 templates,
- exactly 6/type,
- unique IDs,
- valid rarity,
- trigger refs,
- effect refs,
- localization keys,
- prophecy-source categories,
- memory/event requirements.

---

# 196. 177C Phase AN — Trauma/Morale Catalog Validation

Source explicitly requires template validation against trauma/morale catalogs.

If there is no formal catalog:

- validate trigger vocabulary against canonical band IDs/constants.

Do not invent duplicate catalog solely for the test.

---

# 197. 177C Phase AO — Deliberate Failure Proof

Break:

- duplicate template ID,
- missing localization,
- invalid trauma band,
- prophetic template without source policy,
- duplicate effect application,
- invalid interpretation profile.

Assert gate/selftest fails.

---

# 198. 177C Phase AP — Same-Seed Replay

Same:

```text
campaign seed
survivor state
sleep cycles
events
player interpretation choices
```

→ same:

```text
dream IDs
templates
memory refs
prophecy refs
effects
interpretation outcomes
dream-history digest
```

---

# 199. 177C Phase AQ — 100-Day Dream Soak

Profiles:

```text
low_trauma_high_morale
high_trauma_low_morale
combat_veteran
recently_bereaved
stable_average
active_dreamer
```

Record:

```text
sleep cycles
remembered dreams
nightmares
peaceful dreams
memory dreams
prophetic dreams
surreal dreams
mean sleep quality
morale effects
processing intents
```

---

# 200. 177C Phase AR — Frequency Balance

Target qualitative distribution:

- no dream is common,
- peaceful/memory/nightmare state-driven,
- surreal occasional,
- prophetic rare.

Do not hardcode percentages before soak evidence.

---

# 201. 177C Phase AS — Nightmare Balance

Measure:

- nightmare frequency,
- sleep-quality loss,
- morale cost,
- processing outcomes,
- insomnia correlation.

Ensure nightmares are meaningful but not a permanent death spiral.

---

# 202. 177C Phase AT — Peaceful Balance

Measure:

- morale gain,
- sleep gain,
- frequency.

Prevent self-reinforcing max morale.

---

# 203. 177C Phase AU — Prophecy Balance

Measure:

- frequency,
- source validity,
- warning usefulness,
- spoiler precision.

Target:

```text
rare + ambiguous + useful
```

not perfect future knowledge.

---

# 204. 177C Phase AV — Memory-Dream Balance

Measure:

- repeated use of same memory,
- grief-event clustering,
- positive/negative balance.

Use cooldown per source memory if needed.

---

# 205. 177C Phase AW — Surreal Balance

Ensure surreal content adds variety without crowding out state-linked dreams.

---

# 206. 177C Phase AX — Interpretation Value Test

Compare:

- ignore dreams,
- interpret selected dreams,
- interpret everything.

Interpret-all should not become dominant required optimization.

---

# 207. 177C Phase AY — UI Runtime Parity

Displayed:

- dream type,
- day,
- interpreted state,
- effects,
- sleep quality

must match runtime.

---

# 208. 177C Phase AZ — Hidden-State Safety

Dream UI must not reveal:

- exact future raid schedule,
- exact hidden resource IDs,
- unobserved psychological diagnosis,
- non-player-known memory facts.

---

# 209. 177C Phase BA — Accessibility

Dream journal:

- keyboard/controller,
- scalable text,
- type labels beyond color,
- clear interpretation button state,
- no audio-only narrative.

---

# 210. 177C Phase BB — Headless Behavior

Dream generation, effects, save/load, interpretation state, and cooldowns work without UI.

Interpretation itself is player action and may be invoked via command/test adapter.

---

# 211. 177C Phase BC — Non-Interference Test

With dream mechanical effects disabled in a test mode:

- dream recording alone must not alter unrelated simulation state.

With effects enabled:

- only declared canonical sink changes occur.

---

# 212. 177C Phase BD — Port Ownership Audit

Search DreamSystem for direct ownership/mutation of:

```text
fatigue field
insomnia state
PTSD diagnosis
trauma store
mental health store
journal store
```

Expected:

```text
no unauthorized ownership
```

---

# 213. 177C Phase BE — Performance Budget

Dream processing occurs per qualifying sleep cycle, not per frame.

30 templates are tiny.

Expected cost:

```text
O(number of templates)
```

per sleep event.

Document allocations.

---

# 214. 177C Phase BF — Archive / Legacy

Plan 162 may record:

- famous prophetic dream,
- recurring nightmare landmark,
- culturally significant dream.

Do not archive ordinary dreams automatically.

---

# 215. 177C Phase BG — Human Narrative Playtest

Review:

```text
Do dreams feel connected to survivor history?
Do nightmares feel specific rather than generic?
Do prophetic dreams feel suggestive rather than spoilers?
Does interpretation add meaning instead of button-click buffs?
Are dream notifications sparse enough?
```

---

# 216. 177C Phase BH — Documentation

Create:

```text
docs/systems/DREAMS_AND_SLEEP_EVENTS.md
```

Include:

- ownership boundaries,
- dream types,
- trigger context,
- template schema,
- sleep-quality seam,
- trauma-processing policy,
- prophetic-source policy,
- interpretation,
- save/retention,
- adding templates.

---

# 217. 177C Definition of Done

- [ ] `GuiltInsomniaSystem` integration,
- [ ] `CombatTraumaSystem` integration,
- [ ] `MentalHealthCrisisSystem` integration,
- [ ] `SomaticFlashbackSystem` integration,
- [ ] `NeedsSystem` integration,
- [ ] `MoralChoiceSystem` integration,
- [ ] optional Plan 147 memory integration,
- [ ] optional Plan 179 signal,
- [ ] morale integration,
- [ ] prophetic-source integration,
- [ ] save/load lifecycle matrix,
- [ ] dream/effect/interpretation idempotency,
- [ ] sleep-toggle anti-farm,
- [ ] dream/prophecy/nightmare farm prevention,
- [ ] no-sleep/no-dream/high-trauma/high-morale edges,
- [ ] old-save compatibility,
- [ ] retention/size budget,
- [ ] `--dream-selftest`,
- [ ] template/catalog validation,
- [ ] deliberate failure proof,
- [ ] same-seed replay,
- [ ] 100-day soak,
- [ ] frequency/nightmare/peaceful/prophecy balance,
- [ ] interpretation value,
- [ ] UI/runtime parity,
- [ ] hidden-state safety,
- [ ] accessibility,
- [ ] headless,
- [ ] non-interference,
- [ ] ownership audit,
- [ ] performance budget,
- [ ] archive/legacy,
- [ ] playtest,
- [ ] docs.

---

# 218. Integrated Dream Pipeline

```text
canonical sleep cycle
        │
        ▼
DreamContext
        │
        ├─ trauma/morale
        ├─ insomnia
        ├─ recent combat
        ├─ recent death
        ├─ memory/event refs
        ├─ shelter conditions
        └─ authoritative future-risk refs
        │
        ▼
template eligibility + seeded selection
        │
        ▼
DreamOccurrence
        │
        ├─ remembered?
        ├─ type
        ├─ context refs
        └─ effect intents
        │
        ▼
canonical sinks
        │
   ┌────┼───────────┬───────────┬────────────┐
   ▼    ▼           ▼           ▼            ▼
 Needs Insomnia  Trauma/MH   Memory/Grief  Morale
        │
        ▼
DreamHistory / Interpretation / Quest hooks
```

---

# 219. Sleep Authority Contract

DreamSystem does not own:

```text
sleep duration
fatigue
rest capacity
```

It contributes a sleep-quality modifier.

---

# 220. Insomnia Authority Contract

`GuiltInsomniaSystem` remains the insomnia owner.

DreamSystem consumes and emits bounded contextual signals.

---

# 221. Trauma Authority Contract

`CombatTraumaSystem` / mental-health owners remain authoritative.

DreamSystem never stores “trauma processed total” as a competing state.

---

# 222. Flashback Authority Contract

`SomaticFlashbackSystem` owns flashback state.

Dreams may influence it only through explicit sink.

---

# 223. Mental-Health Authority Contract

Dreams can contribute to mental-health context.

They never diagnose disorders.

---

# 224. Memory Authority Contract

Dreams reference canonical memories/events.

They do not create a second survivor-memory graph.

---

# 225. Dream-History Contract

Dream history stores what was experienced.

It does not own the facts dreamed about.

---

# 226. Prophecy Contract

A prophetic dream may only foreshadow:

```text
authoritative scheduled/forecastable/known-risk state
```

It never creates future events.

---

# 227. Interpretation Contract

Interpretation reveals meaning/context and may trigger one bounded consequence.

It never rewrites the dream or source event.

---

# 228. Peaceful-Dream Contract

Peaceful dream benefits are bounded and do not exceed canonical rest/morale caps.

---

# 229. Nightmare Contract

Nightmare:

```text
may produce processing
may produce distress
always affects sleep according to template
```

No guaranteed trauma cure.

---

# 230. Surreal-Dream Contract

If creativity/confusion has no canonical modifier authority:

```text
keep surreal effects narrative-only
```

rather than inventing new permanent stats.

---

# 231. Sleep-Quality Contract

Sleep quality is one assessment used by Needs/rest.

No duplicate persistent sleep-quality truth unless the existing sleep system requires it.

---

# 232. Save Contract

Persist:

- dream occurrences,
- interpretation state,
- cooldowns,
- effect idempotency,
- minimal in-progress sleep-cycle reference if needed.

Do not persist:

- fatigue,
- insomnia,
- trauma,
- morale,
- template catalog,
- future event copies.

---

# 233. Old-Save Contract

Old saves get empty dream history and start dream cadence from load day.

No retroactive dreams.

---

# 234. Determinism Contract

Same:

```text
seed
sleep-cycle ID
survivor state
event history
player choices
```

→ same dream outcome.

---

# 235. Retention Contract

Keep landmark/rare/interpreted dreams.

Roll up repetitive low-value common dreams under Plan 55.

---

# 236. UI Contract

Dream UI shows only:

- experienced dreams,
- current known interpretation,
- legitimate sleep-quality explanation,
- player-known context.

---

# 237. Content Acceptance Contract

Dream templates progress through:

```text
AUTHORED
→ LOADS
→ ELIGIBLE
→ SELECTED
→ EFFECT INTENTS PRODUCED
→ CANONICAL CONSEQUENCES APPLIED
→ PLAYER VISIBLE
```

---

# 238. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| DreamSystem duplicates fatigue/sleep | Medium | High | sleep-quality seam only |
| nightmares become guaranteed trauma cure | High | High | processing-intent policy |
| prophecy leaks exact future state | Medium | High | authoritative-source + ambiguity contract |
| dreams reroll on save/load | Medium | High | sleep-cycle/dream IDs |
| dreams become farmable morale buffs | High | High | frequency/cooldown/caps |
| high trauma creates permanent nightmare spiral | Medium | High | bounded feedback/recovery |
| dream journal duplicates personal journal | Medium | Medium | filtered history/read model |
| templates invent survivor biography | Medium | High | strict context requirements |
| surreal effects require fake new stats | Medium | Medium | narrative-only fallback |
| Plan 179 PTSD gets prematurely duplicated | Medium | High | signal only |
| common dreams flood notifications/save size | High | Medium | attention/retention budgets |
| dream UI leaks hidden prophecy/diagnosis | Medium | High | hidden-state safety tests |

---

# 239. Commit Strategy

## 177A — Foundation

### C2[38].1 — baseline + dream/sleep authority ADR

### C2[38].2 — DreamTemplate / DreamOccurrence / state DTOs

### C2[38].3 — sleep-context and quality assessment seam

### C2[38].4 — deterministic dream selection / cooldowns

### C2[38].5 — effect-intent architecture

### C2[38].6 — nightmare-processing safety contract

### C2[38].7 — prophecy-source / memory-source adapters

### C2[38].8 — interpretation lifecycle

### C2[38].9 — dream_templates.json + loader

### C2[38].10 — save/old-save/idempotency

### C2[38].11 — composition/events/ports/diagnostics

### Gate: 177A complete

---

## 177B — Dream Content / UI

### C2[38].12 — 6 peaceful templates

### C2[38].13 — 6 nightmare templates

### C2[38].14 — 6 memory templates

### C2[38].15 — 6 surreal templates

### C2[38].16 — 6 prophetic templates

### C2[38].17 — sleep-quality / rest presentation

### C2[38].18 — interpretation UI

### C2[38].19 — dream history panel/journal filter

### C2[38].20 — 7 dream events

### C2[38].21 — 7 quest hooks

### C2[38].22 — tutorial/tooltips/localization

### C2[38].23 — content-utilization / narrative-tone audit

### Gate: 177B complete

---

## 177C — Integration / Validation

### C2[38].24 — GuiltInsomnia / Needs integration

### C2[38].25 — CombatTrauma / MentalHealth integration

### C2[38].26 — SomaticFlashback / MoralChoice integration

### C2[38].27 — memory / prophecy-source optional integrations

### C2[38].28 — save-load/idempotency matrix

### C2[38].29 — dream/interpretation/prophecy anti-farm suite

### C2[38].30 — edge cases

### C2[38].31 — `--dream-selftest`

### C2[38].32 — data integrity + deliberate failure proof

### C2[38].33 — same-seed replay

### C2[38].34 — 100-day dream soak

### C2[38].35 — frequency/psychology balance profiles

### C2[38].36 — hidden-state/accessibility/non-interference audits

### C2[38].37 — retention/performance/docs/playtest

### Gate: 177C complete

---

# 240. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --dream-selftest
```

Also run repository-canonical equivalents of:

```text
port-contract validation
dream-template 30/5x6 integrity check
old-save fixture load
same-seed dream replay
sleep-cycle idempotency test
nightmare-processing duplication test
prophecy-source authorization test
dream-history retention/size test
dream-journal hidden-state/accessibility check
```

---

# 241. Flagship Definition of Done

## 177A — Foundation

- [ ] `DreamSystem.cs`,
- [ ] `DreamTemplate`,
- [ ] `DreamOccurrence`,
- [ ] sleep-event references,
- [ ] versioned DreamState,
- [ ] 5 dream types,
- [ ] deterministic triggers,
- [ ] dream frequency/cooldown,
- [ ] sleep-quality seam,
- [ ] effect intents,
- [ ] safe nightmare-processing contract,
- [ ] authoritative prophetic-source contract,
- [ ] memory-source adapter,
- [ ] interpretation,
- [ ] catalog loader,
- [ ] 30 templates / 6 each,
- [ ] composition-root integration,
- [ ] old-save handling,
- [ ] events,
- [ ] ports,
- [ ] diagnostics.

## 177B — Dreams / UI / Content

- [ ] peaceful dreams,
- [ ] nightmares,
- [ ] prophetic dreams,
- [ ] memory dreams,
- [ ] surreal dreams,
- [ ] survivor-specific content,
- [ ] no invented biography,
- [ ] sleep-quality display,
- [ ] interpretation action,
- [ ] uninterpretable cases,
- [ ] 7 events,
- [ ] 7 quest hooks,
- [ ] dream history UI,
- [ ] notifications,
- [ ] tutorial,
- [ ] tooltips,
- [ ] localization,
- [ ] content-utilization,
- [ ] narrative-quality review.

## 177C — Integration / Validation

- [ ] GuiltInsomniaSystem,
- [ ] CombatTraumaSystem,
- [ ] MentalHealthCrisisSystem,
- [ ] SomaticFlashbackSystem,
- [ ] NeedsSystem,
- [ ] MoralChoiceSystem,
- [ ] optional Plan 147 memory,
- [ ] future Plan 179 signal,
- [ ] save/load lifecycle,
- [ ] dream selection idempotency,
- [ ] effect idempotency,
- [ ] interpretation idempotency,
- [ ] sleep-toggle exploit blocked,
- [ ] dream farm blocked,
- [ ] prophecy farm blocked,
- [ ] nightmare farm blocked,
- [ ] no-sleep/no-dream edge cases,
- [ ] active-dreamer cap,
- [ ] high-trauma/high-morale balance,
- [ ] old-save compatibility,
- [ ] retention,
- [ ] `--dream-selftest`,
- [ ] data-integrity gate,
- [ ] failure proof,
- [ ] same-seed replay,
- [ ] 100-day soak,
- [ ] psychology/frequency balance,
- [ ] UI/runtime parity,
- [ ] hidden-state safety,
- [ ] accessibility,
- [ ] headless,
- [ ] non-interference,
- [ ] ownership audit,
- [ ] performance budget,
- [ ] archive/legacy,
- [ ] docs.

## Global

- [ ] no duplicate fatigue authority,
- [ ] no duplicate insomnia authority,
- [ ] no duplicate trauma/PTSD authority,
- [ ] no duplicate memory graph,
- [ ] no guaranteed nightmare cure,
- [ ] no magical uncommitted prophecy,
- [ ] no dream reroll exploit,
- [ ] no click-to-buff interpretation exploit,
- [ ] no UI-driven dream generation,
- [ ] full verification green.

---

# 242. Closure Report Template

```markdown
## C2[38] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Sleep/rest owner:
- Insomnia owner:
- Trauma owner:
- Flashback owner:
- Mental-health owner:
- MoralChoice nightmare reference:
- Memory source:
- Prophecy/forecast sources:

### 177A — Foundation
- DreamSystem:
- Dream types:
- Template count:
- Per-type count:
- Sleep context:
- Sleep-quality seam:
- Frequency policy:
- RNG stream:
- Effect intents:
- Nightmare-processing policy:
- Prophecy-source policy:
- Interpretation:
- Save schema:
- Old-save behavior:
- Missing ports:
- Result:

### 177B — Content
- Peaceful:
- Nightmares:
- Prophetic:
- Memory:
- Surreal:
- Events:
- Quest hooks:
- Dream journal:
- Sleep-quality UI:
- Interpretation UI:
- Unused templates:
- Narrative-quality findings:
- Result:

### 177C — Integration
- GuiltInsomnia:
- CombatTrauma:
- MentalHealth:
- SomaticFlashback:
- Needs:
- MoralChoice:
- Memory:
- PTSD signal:
- Save-load rerolls:
- Duplicate effects:
- Interpretation duplicates:
- Sleep-toggle exploit:
- Dream farm:
- Prophecy farm:
- High-trauma profile:
- High-morale profile:
- 100-day soak:
- Hidden-state leaks:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Dream selftest:
- Port contract:
- 30-template integrity:
- Old-save fixtures:
- Same-seed replay:
- Retention-size test:
- UI/accessibility:
- Result:

### Final Metrics
- DREAM_TEMPLATES:
- PEACEFUL_TEMPLATES:
- NIGHTMARE_TEMPLATES:
- PROPHETIC_TEMPLATES:
- MEMORY_TEMPLATES:
- SURREAL_TEMPLATES:
- SLEEP_CYCLES:
- DREAMS_EXPERIENCED:
- NIGHTMARES:
- PROPHECIES:
- INTERPRETATIONS:
- DREAM_EFFECT_DUPLICATES:
- DREAM_REROLL_VIOLATIONS:
- PROPHECY_SOURCE_VIOLATIONS:
- HIDDEN_STATE_LEAKS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Dream content:
- Memory integration:
- Therapy:
- PTSD integration:
- Audio/voice:
- UI:
```

---

# 243. Final Execution Directive

Execute Plan 177 as a **sleep-triggered narrative/psychological projection over the existing rest, insomnia, trauma, mental-health, memory, and moral-choice systems**.

The critical sequence is:

```text
consume a real completed sleep cycle
→ build survivor-specific context from canonical state
→ decide whether a remembered dream occurs
→ deterministically select one eligible template
→ resolve content from real memory/event references
→ commit dream history exactly once
→ emit bounded effect intents to canonical owners
→ calculate one sleep-quality contribution
→ allow optional interpretation exactly once
→ persist dream history/cooldowns/idempotency only
→ validate long-run frequency and psychological balance
```

Do not create a second fatigue system.

Do not create a second insomnia system.

Do not diagnose PTSD inside DreamSystem.

Do not make nightmares guaranteed trauma cures.

Do not let prophetic dreams invent future events that the simulation has not committed or made forecastable.

Do not reroll dreams by reloading or repeatedly toggling sleep.

The strongest authority rule is:

> **DreamSystem owns which remembered dream occurred and what it symbolically references; fatigue, insomnia, trauma, mental health, memory, morale, and future events remain owned by their canonical systems.**

The strongest psychological rule is:

> **A nightmare is evidence of unresolved pressure and may participate in processing, but it is not automatically therapeutic; its final psychological consequence must be resolved by the system that owns trauma and mental health.**

The strongest prophecy rule is:

> **A prophetic dream can foreshadow an authoritative risk the game already knows exists, but it cannot create hidden future truth merely to justify a dream.**

The flagship acceptance scenario is:

> **Take three survivors into the same seeded night: one stable survivor with high morale, one combat survivor with high trauma and a recent firefight, and one bereaved survivor with a recent canonical death memory. Let all three complete qualifying sleep cycles. The stable survivor may receive a peaceful or no remembered dream; the combat survivor should have elevated nightmare eligibility; the bereaved survivor should have elevated memory-dream eligibility. Save/load after dream selection but before effect resolution and prove each dream ID/template/context remains identical. Then schedule an authoritative severe storm and run a later sleep cycle for a prophecy-eligible survivor: any prophetic dream may foreshadow only that real storm risk and must not expose unsupported hidden facts. Interpret one dream, reload, and verify the interpretation consequence does not duplicate. Final fatigue, insomnia, trauma, mental-health, and morale changes must appear only through their canonical owners, while DreamSystem retains only the dream history and interpretation record.**
