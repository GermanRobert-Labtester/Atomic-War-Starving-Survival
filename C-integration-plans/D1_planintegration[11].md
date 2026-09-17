# D1 Flagship Integration Plan [11]
## Plan 174 — Procedural Survivor Backstories & Origin Mechanics

> **Purpose:** Give every survivor a deterministic, mechanically meaningful pre-campaign history that explains
> who they were, why they possess their skills and traits, what they lost, what they fear, what they still want,
> and which parts of their past may become relevant during the campaign.
>
> **Primary source:** Plan 174 — Procedural Survivor Backstories & Origin Mechanics.
>
> **Core design problem:** ASHFALL currently has survivor stats, traits, relationships, and at least one plain
> `backstory` flavor string, but no authoritative biography system whose generated history affects starting
> capabilities, behavior, relationships, secrets, quest hooks, possessions, stress responses, or later
> revelations.
>
> **Implementation posture:** deterministic, data-driven, authored-fragment based, save-stable, bounded in power,
> migration-safe, compatible with existing survivor aggregates, and integrated through explicit skill, trait,
> relationship, moral, quest, journal, autonomy, hobby, and epilogue contracts rather than duplicating them.
>
> **Critical guardrail:** a backstory is an explanatory origin layer, not a second survivor-stat system. It may
> provide starting modifiers and stable narrative facts, but canonical skills, traits, health, relationships,
> inventory, morale, quests, and survivor fate remain owned by their existing systems.

---

## 1. Source Problem Statement

The source plan identifies a narrow but high-value survivor gap:

- `YearOfAshCatalogLoader.cs` contains a `backstory` string used as flavor only;
- survivor creation produces functional characters, but not mechanically coherent personal histories;
- no procedural backstory generator exists;
- no occupation/life-experience authority exists;
- no hidden/revealed biography state exists;
- no lost-connections system exists;
- no backstory-driven quest or relationship hooks exist.

As a result, a survivor can have excellent medical skill without the world ever establishing whether they were
a doctor, medic, caregiver, plague responder, or self-taught survivor. The stats exist; the person does not.

The flagship architecture therefore becomes:

```text
Survivor creation / legacy survivor migration
                ↓
         BackstorySystem
                ↓
 authored template + weighted compatibility graph
                ↓
 deterministic biography composition
  ├─ occupation
  ├─ 2–4 life experiences
  ├─ defining moment
  ├─ motivation / reason for survival
  ├─ lost connections
  ├─ optional secrets
  └─ authored flavor fragments
                ↓
 starting-effect requests
  ├─ skill authority
  ├─ trait authority
  └─ inventory authority
                ↓
 persistent SurvivorBackstory
                ↓
 later reveal/event hooks
  ├─ trust / relations
  ├─ autonomy / stress
  ├─ moral choices
  ├─ quests
  ├─ journal
  ├─ hobbies
  └─ epilogue
```

The generated biography is a stable campaign fact. It must never be rerolled merely because a save is loaded,
the survivor detail panel is opened, or the catalog order changes.

---

## 2. Flagship Success Criteria

The system is complete only when all of the following are simultaneously true:

1. `BackstorySystem.cs` exists with schema-versioned `CaptureState/RestoreState`.
2. A canonical `backstory_templates.json` catalog exists.
3. Occupations and life experiences are stable data IDs.
4. Backstory generation is deterministic from campaign/survivor inputs.
5. Generation does not use `System.Random`, wall-clock time, or collection iteration order.
6. Existing authored survivors can opt into fixed/partially-authored backstories.
7. Procedural survivors receive backstories at canonical creation time.
8. Old saves receive stable migrated backstories without rewriting existing skills/traits incorrectly.
9. Starting bonuses are applied exactly once.
10. Backstory generation cannot be rerolled by save/load or UI.
11. Skill modifiers route through the canonical skill authority.
12. Trait modifiers route through the canonical trait/personality authority.
13. Starting possessions route through the canonical inventory/grant transaction.
14. Relationship hooks do not create a duplicate relationship score.
15. Secrets have explicit revealed/hidden state.
16. Secret revelation is deterministic/event-driven and cannot fire twice.
17. Lost connections use stable placeholder/person IDs and do not fabricate full NPCs unless a consumer exists.
18. Quest hooks are exported as facts/opportunities; BackstorySystem does not own quest runtime.
19. UI displays only discovered biography information.
20. Hidden information does not leak through tooltips, localization, debug data, or serialization projected to UI.
21. `--backstory-selftest` validates generation, migration, one-time starting effects, revelation, save/load,
    and catalog integrity.
22. Ten occupation templates and twenty life-experience definitions exist only if they map to real skills,
    traits, items, and gameplay systems.
23. Backstory combinations are diverse but coherent; impossible combinations are rejected by compatibility rules.
24. Mechanical power from biography is bounded so survivor recruitment remains meaningful rather than dominated
    by one "best" occupation.
25. Biography remains useful even when no backstory quest ever fires.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/backstories/BACKSTORY_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Survivors/`
- `SurvivorLifecycle.cs`
- canonical survivor aggregate/DTO
- `SurvivorRelationsSystem.cs`
- `SkillProgressionSystem.cs`
- trait/personality storage
- survivor autonomy system from Plan 144 if implemented
- needs/stress/mental-health systems
- moral choice system
- quest runtime
- journal runtime
- inventory grant APIs
- survivor starting loadout code
- survivor recruitment/spawn paths
- save schema and survivor migration
- seeded RNG authority
- current `survivors.json`
- `YearOfAshCatalogLoader.cs`
- any authored `backstory` or biography strings
- hobbies/leisure from Plan 161
- achievements from Plan 149
- unified ending/epilogue from Plan 145
- localization loader
- data integrity validator

For each existing survivor initialization path, record:

| Creation path | Survivor ID source | Skills initialized where? | Traits initialized where? | Inventory granted where? | Backstory hook |
|---|---|---|---|---|---|
| starting roster | | | | | |
| recruited survivor | | | | | |
| quest recruit | | | | | |
| migrated old save | | | | | |

Do not wire generation to only one constructor if survivors can enter the campaign through multiple paths.

---

## 4. Scope Boundary

### In scope

- occupation templates;
- life experiences;
- defining moments;
- motivations/reasons for survival;
- lost connections;
- secrets;
- deterministic generation;
- compatibility rules;
- starting skill/trait/item modifiers;
- biography-driven behavior hints;
- reveal state;
- trust/event-driven revelations;
- backstory quest hooks;
- journal/UI;
- old-save migration;
- CI/selftests.

### Out of scope for first pass

- fully procedural prose generation with an LLM;
- spawning arbitrary connected NPCs without catalog authority;
- a second personality model;
- a second trauma system;
- a second relationship system;
- arbitrary criminality/legal simulation;
- cross-campaign legacy ownership;
- forced exile logic inside backstories;
- large bespoke quest chains for every generated biography;
- unrestricted random starting weapons/items.

Backstories should enrich existing systems, not create seven new shadow systems.

---

## 5. Canonical Data Authority

Create:

`Assets/StreamingAssets/Data/backstory_templates.json`

Recommended root:

```json
{
  "schemaVersion": 1,
  "occupations": [],
  "experiences": [],
  "definingMoments": [],
  "motivations": [],
  "connectionArchetypes": [],
  "secrets": [],
  "compatibilityRules": []
}
```

This keeps fragments authored and generation compositional.

---

## 6. Backstory Template Contract

Recommended:

```csharp
public sealed record BackstoryOccupationDefinition
{
    public string OccupationId { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public int Weight { get; init; }

    public IReadOnlyList<StartingSkillModifier> SkillModifiers { get; init; }
    public IReadOnlyList<StartingTraitModifier> TraitModifiers { get; init; }
    public IReadOnlyList<StartingItemGrant> StartingItems { get; init; }

    public IReadOnlyList<string> PreferredExperienceTags { get; init; }
    public IReadOnlyList<string> ForbiddenExperienceIds { get; init; }
    public IReadOnlyList<string> BehaviorTags { get; init; }
}
```

Do not store rendered occupation names inside save state; store stable IDs.

---

## 7. Life Experience Contract

```csharp
public sealed record LifeExperienceDefinition
{
    public string ExperienceId { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public string CategoryId { get; init; }
    public BackstoryRarity Rarity { get; init; }

    public IReadOnlyList<StartingSkillModifier> SkillModifiers { get; init; }
    public IReadOnlyList<StartingTraitModifier> TraitModifiers { get; init; }
    public IReadOnlyList<string> BehaviorTags { get; init; }
    public IReadOnlyList<string> RequiredTags { get; init; }
    public IReadOnlyList<string> ForbiddenTags { get; init; }
}
```

The category is descriptive; the ID is authoritative.

---

## 8. Survivor Backstory Runtime Record

Recommended persisted record:

```csharp
public sealed record SurvivorBackstory
{
    public int SchemaVersion { get; init; }
    public string SurvivorId { get; init; }

    public string OccupationId { get; init; }
    public IReadOnlyList<string> ExperienceIds { get; init; }
    public string DefiningMomentId { get; init; }
    public string MotivationId { get; init; }

    public IReadOnlyList<BackstoryConnection> LostConnections { get; init; }
    public IReadOnlyList<BackstorySecretState> Secrets { get; init; }

    public IReadOnlySet<string> RevealedFactIds { get; init; }

    public string GenerationProfileId { get; init; }
    public string GenerationDigest { get; init; }
    public bool StartingEffectsApplied { get; init; }
}
```

The save record stores IDs/facts, not generated English prose.

---

## 9. Fixed vs Procedural Survivors

Support three modes:

### Authored
All biography IDs explicitly specified in survivor data.

### Hybrid
Occupation or key experiences authored; remaining slots generated.

### Procedural
All components generated deterministically.

This prevents story-critical survivors from receiving incoherent random biographies.

---

## 10. Stable Generation Seed

Seed from stable data:

```text
campaignSeed
+ survivorStableId
+ backstoryGenerationProfileVersion
```

If survivor IDs are generated dynamically, ensure ID assignment itself is persisted before biography generation.

Never seed from:
- join day alone;
- current timestamp;
- UI order;
- list index that can change across versions.

---

## 11. Generation Digest

Compute a normalized digest from:

- occupation;
- sorted/ordered experiences;
- defining moment;
- motivation;
- connections;
- secrets;
- generation profile/version.

Store for debugging and migration.

Same seed/profile/catalog revision should produce same digest.

---

## 12. Occupation Set

The source proposes:

- doctor;
- soldier;
- farmer;
- engineer;
- teacher;
- merchant;
- pilot;
- artist;
- cleric;
- mechanic.

Treat these as candidate archetypes, not guaranteed final IDs.

Before authoring, verify actual skills/items exist.

Examples requiring audit:
- "education skill";
- "creative skill";
- "spiritual skill";
- "navigation skill";
- "charisma trait";
- "religious item";
- "map";
- "medical kit";
- "weapon".

Do not fabricate missing catalogs merely to satisfy a list.

---

## 13. Occupation Mechanical Budget

Each occupation should have comparable total starting power.

Use a point budget concept for balancing, e.g.:

```text
skill bonus points
+ trait advantages
+ starting item value
- explicit disadvantages
```

This is a tuning tool, not a player-facing score.

Avoid:
- doctor gets top medical + valuable kit + empathy with no meaningful tradeoff;
- soldier gets a rare weapon that dominates recruitment value;
- artist gets mostly flavor and becomes mechanically inferior.

The backstory layer should create identity, not obvious tier lists.

---

## 14. Starting Skill Modifiers

BackstorySystem should request initial modifiers before normal progression begins.

Example event/command:

```csharp
ApplyStartingSkillModifier(
    survivorId,
    skillId,
    delta,
    sourceFactId);
```

Use source fact IDs for idempotency.

Do not edit raw skill dictionaries if a canonical initialization API exists.

---

## 15. Starting Trait Modifiers

Traits may be:

- granted;
- weighted;
- prohibited;
- adjusted only at generation.

Do not dynamically change trait truth every time a backstory fact is revealed.

Revelation changes player knowledge; it should not retroactively rewrite who the survivor was.

---

## 16. Starting Possessions

Starting possessions require careful authority.

Flow:

```text
backstory generated
 -> starting-item grants validated
 -> survivor inventory/loadout transaction
 -> grant IDs recorded
```

No duplicate grants on restore.

If the survivor joins with an existing recruitment loadout, merge under an explicit starting-loadout policy.

---

## 17. Item Power Guardrails

Occupation possessions should usually be:
- common;
- thematic;
- useful but not rare progression skips.

Rare weapons, unique quest items, advanced research tools, or scarce medicine require explicit balance review.

---

## 18. Life Experience Categories

Source categories:

- combat;
- medical;
- leadership;
- survival;
- technical;
- social;
- creative;
- athletic;
- intellectual;
- spiritual.

Use category IDs only if corresponding mechanical hooks exist.

An experience can carry multiple tags:
```text
experience_field_medic:
medical, combat, crisis
```

---

## 19. Experience Count

Source suggests 2–4.

Generation should choose a target count deterministically from profile:

```text
common survivor: 2–3
older/complex biography profile: 3–4
story-critical: authored
```

Do not infer age unless age exists canonically.

---

## 20. Experience Compatibility Graph

Avoid nonsensical combinations through declarative rules.

Examples:
- "career pacifist" may conflict with "decorated combat veteran";
- "lifelong isolated hermit" may conflict with "career elected official";
- two mutually exclusive institutional roles may be forbidden unless a timeline supports both.

Represent:
- required tags;
- forbidden tags;
- preferred tags;
- max duplicates per category.

Keep compatibility simple and auditable.

---

## 21. Weighted Generation Without Game-State Cheating

The source suggests occupation weighted by game-state needs.

Use caution.

If the shelter lacks a doctor and generation strongly favors doctor, recruitment can become a hidden adaptive
difficulty system.

Recommended:
- default biography generation depends only on survivor seed/template profile;
- optional scenario/recruitment tables may deliberately bias occupation availability.

Do not let current player shortages secretly rewrite a survivor's past unless explicitly designed.

---

## 22. Defining Moment Contract

Defining moments are authored fact fragments.

Example categories:
- saved someone;
- failed to save someone;
- abandoned a home;
- survived bombardment;
- betrayed by institution;
- protected family;
- made a terrible compromise;
- escaped captivity.

Each definition contains:
- narrative key;
- behavior tags;
- reveal visibility;
- optional moral/relationship hooks.

Avoid directly embedding huge stat effects.

---

## 23. Motivation / Reason for Survival

Motivation is a persistent narrative/behavior tag.

Examples:
- find family;
- protect others;
- prove worth;
- atone;
- rebuild;
- survive at any cost;
- preserve knowledge;
- keep a promise.

It may influence autonomy preferences or morale reactions through explicit adapters.

Do not create a second morale stat.

---

## 24. Lost Connections Contract

```csharp
public sealed record BackstoryConnection
{
    public string ConnectionId { get; init; }
    public string ArchetypeId { get; init; }
    public string RelationshipKind { get; init; }
    public BackstoryConnectionStatus Status { get; init; }
    public string? LinkedNpcId { get; init; }
    public bool Revealed { get; init; }
}
```

Statuses:
- unknown;
- presumed_alive;
- presumed_dead;
- confirmed_dead;
- reachable;
- reunited;
- unresolved.

Do not invent full NPC records unless linked to a real spawn/quest path.

---

## 25. Connection Identity

A connection needs stable identity even before an NPC exists.

Use:
```text
connection:<survivorId>:<sequence>
```

Name/description can be authored/generated from safe name tables if available.

Do not use user-facing name as the primary ID.

---

## 26. Connection Realization

If a quest later materializes the connection as an NPC:

```text
BackstoryConnection
 -> Quest/NPC factory
 -> linkedNpcId stored
```

BackstorySystem remains owner of the relationship-to-past fact, not the NPC lifecycle.

---

## 27. Reunion Outcomes

Reunion can be:
- positive;
- conflicted;
- grief;
- betrayal;
- unresolved.

Route effects to:
- morale;
- relations;
- quest;
- journal.

Do not directly mutate those systems.

---

## 28. Secrets Contract

```csharp
public sealed record BackstorySecretDefinition
{
    public string SecretId { get; init; }
    public string TitleKey { get; init; }
    public string RevealTextKey { get; init; }
    public BackstorySecretSeverity Severity { get; init; }
    public IReadOnlyList<string> RequiredTags { get; init; }
    public IReadOnlyList<string> ForbiddenTags { get; init; }
    public string RevealProfileId { get; init; }
    public IReadOnlyList<string> ConsequenceHookIds { get; init; }
}
```

Keep consequences explicit and bounded.

---

## 29. Secret Count

Source suggests 0–2.

Recommended:
- most survivors: 0–1;
- rare complex biography: 2;
- story-critical: authored.

Do not make everyone secretly criminal/important; that becomes melodramatic.

---

## 30. Secret Visibility

Secret states:

```text
hidden
hinted
revealed
resolved
```

UI must never receive hidden text keys until allowed.

A serialized save can contain secret IDs, but presentation projection filters them.

---

## 31. Revelation Trigger Model

Triggers can include:

- trust threshold crossed;
- relationship tier;
- day/time together;
- related world event;
- quest trigger;
- survivor crisis;
- conversation event.

Use typed trigger kinds.

Do not scan arbitrary game state with reflection.

---

## 32. Trust-Based Revelation

Trust is an input from relations system.

When threshold crosses:
- evaluate eligible hidden facts;
- select deterministic reveal;
- persist reveal ID;
- emit event once.

Do not continuously poll every frame.

---

## 33. Revelation Pace

Avoid dumping full biography on recruitment day.

Suggested reveal layers:

### Known on arrival
- occupation;
- public experience summary;
- basic motivation if appropriate.

### Learned over time
- defining moment;
- specific connection details;
- nuanced experiences.

### Hidden
- secrets;
- sensitive past;
- unresolved betrayal/crime/heroism.

This gives relationship progression narrative value.

---

## 34. Secret Consequences

Negative secrets may create:
- trust change request;
- moral choice;
- quest;
- leadership concern.

Positive secrets may reveal:
- a contact;
- a hidden but already-supported capability;
- useful information.

Do not suddenly add an unearned new skill level at reveal unless the capability was already mechanically present
but hidden by design.

---

## 35. Hidden Skill Problem

The source suggests "positive secrets: hidden skills."

If a skill was not present mechanically before reveal, adding it later can feel magical.

Safer options:
1. skill existed from start but UI explanation was hidden;
2. reveal unlocks permission/knowledge use through a specific system;
3. reveal creates training/research opportunity.

Prefer option 1 or 3.

---

## 36. Backstory-Driven Behavior

Backstory behavior should be advisory tags consumed by autonomy.

Examples:
- `prefers_medical_duty`;
- `avoids_unnecessary_violence`;
- `protective_of_children`;
- `comfortable_under_fire`;
- `seeks_solitude`;
- `values_books`;
- `fears_confinement`.

Autonomy remains owner of action selection.

---

## 37. Work Preference Integration

Occupation can influence job preference, not hard-lock assignment.

Example:
- former doctor prefers medical duty;
- mechanic prefers workshop repairs;
- teacher prefers education/mentoring.

Player can still assign otherwise unless system rules forbid.

---

## 38. Stress Response Integration

Defining moment may emit modifiers/tags to mental-health system.

Examples:
- bombardment survivor reacts strongly to shelling;
- plague survivor reacts to disease outbreak;
- former medic reacts to preventable death.

Do not create stress points in BackstoryState.

---

## 39. Morale Motivation Integration

Motivation may change context-specific morale effects.

Examples:
- "find family" reacts to connection clue;
- "protect others" reacts to survivor death;
- "rebuild" reacts to shelter upgrade.

Needs/MentalHealth remains final authority.

---

## 40. Relationship Hooks

Backstory can provide compatibility tags:
- shared profession;
- shared experience;
- ideological tension;
- similar loss;
- mentor potential.

Relations system decides bond/affinity.

Do not precompute relationship scores for every survivor pair inside BackstorySystem.

---

## 41. Pairwise Complexity Guardrail

For N survivors, avoid O(N²) biography comparison every tick.

Evaluate relation hooks:
- on survivor arrival;
- on relevant reveal;
- on group event.

Cache only emitted relationship facts, not full pair matrix if unnecessary.

---

## 42. Hobby Integration

Plan 161 can consume biography tags:

- artist -> creative hobby affinity;
- teacher -> reading/storytelling;
- mechanic -> woodworking/repair hobby;
- athlete -> physical hobbies.

Backstory does not own hobby progress.

---

## 43. Education Integration

Plan 154 or education system can consume:
- former teacher;
- academic/intellectual experience;
- vocational experience.

This may affect teaching eligibility or starting knowledge through canonical interfaces.

---

## 44. Romance/Family Integration

Plan 150 can consume:
- prior family/lost spouse facts;
- attachment preferences where authored;
- unresolved connections.

Do not use backstory to hardcode attraction or orientation unless the canonical relationship system owns those
fields.

---

## 45. Moral Choice Integration

Secret/reunion events can request moral decisions.

Examples:
- shelter a connection with a dangerous past;
- forgive betrayal;
- reveal a survivor's secret publicly;
- honor a past promise.

MoralChoiceSystem owns moral weight.

---

## 46. Quest Integration

Backstory exports quest hook facts:

```text
BackstoryQuestHookAvailable(
 survivorId,
 hookType,
 factIds)
```

Quest system decides:
- whether a quest exists;
- prerequisites;
- rewards;
- completion.

This prevents generated biographies from spawning invalid quest IDs.

---

## 47. Quest Hook Categories

Suggested:
- find connection;
- revisit place;
- recover keepsake;
- confront past;
- use prior expertise;
- resolve secret;
- fulfill promise.

Each requires an actual quest template family before activation.

---

## 48. Backstory Quest Uniqueness

"Unique to each survivor" should mean personalized parameters, not 100% bespoke authored quest scripts.

Example:
- same reunion template;
- survivor-specific connection;
- destination;
- relationship;
- outcome flavor.

This scales.

---

## 49. Journal Integration

Journal notable entries:
- survivor arrival summary;
- defining moment revealed;
- connection clue;
- reunion;
- secret reveal;
- arc resolution.

Do not log every static occupation trait as a separate entry.

---

## 50. UI Backstory Projection

Create:

```csharp
public sealed record BackstoryPanelModel
{
    public string SurvivorId { get; init; }
    public string OccupationTitleKey { get; init; }
    public IReadOnlyList<BackstoryFactView> KnownExperiences { get; init; }
    public BackstoryFactView? DefiningMoment { get; init; }
    public BackstoryFactView? Motivation { get; init; }
    public IReadOnlyList<ConnectionView> KnownConnections { get; init; }
    public IReadOnlyList<SecretView> RevealedSecrets { get; init; }
    public int HiddenFactCount { get; init; }
}
```

Hidden facts stay absent, not merely blurred with their actual content in the UI DTO.

---

## 51. Arrival Presentation

On survivor arrival, show:
- name;
- occupation;
- one or two known background facts;
- obvious starting capabilities;
- optional line of authored flavor.

Do not reveal the full generated record immediately.

---

## 52. Mechanical Effect Explainability

Survivor detail should be able to explain:

```text
Medical +2
Source: Former doctor

Repair +1
Source: Field mechanic experience
```

This ties narrative to mechanics and meets the source plan's core value.

---

## 53. Tooltips

Tooltips may show:
- what a revealed experience means;
- which starting modifier it contributed;
- whether it influences behavior.

No hidden secret text.

---

## 54. Localization Architecture

Every authored fragment uses localization keys.

Avoid assembling complex grammar from tiny fragments.

Prefer:
- complete occupation descriptions;
- complete defining moment paragraphs;
- parameterized names/places only where localization-safe.

Procedural composition should combine sections, not word salad.

---

## 55. Flavor Text Composition

Backstory prose can be built from:
- occupation intro;
- one selected experience paragraph;
- defining moment paragraph;
- motivation sentence.

Store selected fragment IDs.

Do not generate arbitrary text at runtime.

---

## 56. Content Tone

Review for:
- plausible pre-war/post-collapse history;
- non-melodramatic secrets;
- diversity of ordinary lives;
- not every survivor being ex-military/doctor;
- age/plausibility consistency if ages exist;
- setting consistency.

A good generator produces teachers, drivers, caregivers, technicians, clerks, farmers, artists, laborers—not only
genre heroes.

---

## 57. Rarity Model

Rarity should shape unusual experiences, not power tier.

Example:
- common: ordinary work history;
- uncommon: specialized training;
- rare: exceptional event.

A rare biography should be narratively unusual, not automatically mechanically superior.

---

## 58. Occupation Weighting

Base weights should reflect world plausibility.

Do not assign equal 10% chance to:
- doctor;
- pilot;
- artist;
- farmer;
- mechanic;

unless lore supports it.

Generate a distribution report.

---

## 59. Distribution Calibration

Create:
`docs/backstories/BACKSTORY_DISTRIBUTION.md`

Simulate at least 100,000 generated biographies offline.

Report:
- occupation frequency;
- experience frequency;
- secret count;
- connection count;
- average skill budget;
- top combination frequencies;
- invalid/retry rate.

Use deterministic test seed set.

---

## 60. Rejection Sampling Guardrail

If compatibility rules cause excessive retries, generation can become biased/slow.

Prefer:
1. filter valid candidates first;
2. weighted choose among valid candidates.

Avoid repeated blind rerolls.

---

## 61. Deterministic Candidate Ordering

Before weighted choice:
- sort candidate IDs stably.

Otherwise data loader order changes biography outcomes.

This is critical.

---

## 62. Catalog Revision Behavior

Existing survivors must not reroll when catalog changes.

Persist selected IDs.

New survivors use new catalog.

If a selected definition is removed:
- migration mapping required;
- or retain deprecated definition compatibility.

Do not silently choose replacement.

---

## 63. Deprecated Biography Definitions

Keep deprecated IDs loadable for old saves where practical.

Mark:
```text
deprecated: true
replacementId: ...
```

Do not include in new generation candidate pool.

---

## 64. Old-Save Migration Strategy

The source says existing survivors get generated backstories.

This is dangerous if generated starting bonuses are applied retroactively.

Correct migration policy:

1. generate a stable biography for existing survivor;
2. mark `StartingEffectsApplied = true`;
3. **do not apply occupation/experience starting skill, trait, or item bonuses**;
4. use biography for future behavior/revelation/quests only;
5. optionally choose templates compatible with existing skills/traits to explain the survivor rather than alter them.

This preserves old-save gameplay.

---

## 65. Constraint-Based Legacy Backstory Generation

For migrated survivors, use current stats as constraints.

Example:
- high medical skill increases probability of medical-related occupation/experience;
- strong technical skill favors mechanic/engineer history;
- existing traits constrain incompatible biography fragments.

Goal:
explain the survivor already in the save.

Do not rewrite them.

---

## 66. Legacy Generation Seed

Use:
- campaign seed;
- stable survivor ID;
- migration profile version.

Persist result immediately.

Reload must not reroll.

---

## 67. Starting Effects Idempotency

Use stable source IDs:

```text
backstory:<survivorId>:occupation:<occupationId>
backstory:<survivorId>:experience:<experienceId>
```

Skill/trait/item systems should reject duplicate application where possible.

Also persist `StartingEffectsApplied`.

---

## 68. Survivor Creation Transaction

Recommended:

```text
allocate survivor ID
 -> create base survivor aggregate
 -> generate/finalize backstory
 -> validate starting modifiers
 -> apply starting skills/traits/items
 -> persist survivor + backstory
 -> emit SurvivorCreated
```

If any step fails, do not leave half-created survivor.

---

## 69. Recruitment Preview

If recruitment UI shows survivors before joining, decide whether backstory is generated:

### At candidate creation
Pros:
- stable preview.
Cons:
- many unused biographies.

### At recruitment commit
Pros:
- less state.
Cons:
- preview cannot show real history.

Recommendation:
candidate has stable ID/seed and preview biography snapshot; committing preserves exactly that biography.

---

## 70. Anti-Reroll Exploit

Prevent:
- reopening recruit screen;
- cancelling/reopening;
- save/reload before recruitment;
- changing UI sort order;
- restarting same day.

Candidate biographies derive from stable candidate IDs and recruitment-generation seed.

---

## 71. No-Backstory Edge Case

If catalog unavailable/disabled in tests:
- survivor may use explicit `unknown_past` fallback;
- no starting bonus;
- no crash.

Production integrity should ensure catalog exists.

---

## 72. Complex Backstory Edge Case

Maximum:
- occupation;
- four experiences;
- defining moment;
- motivation;
- three connections;
- two secrets.

UI and save must remain bounded and readable.

---

## 73. Secret Revelation Idempotency

Stable reveal event:

```text
backstory_reveal:<survivorId>:<factId>
```

Persist in `RevealedFactIds`.

Repeated trust events cannot reveal twice.

---

## 74. Multiple Reveal Eligibility

If several secrets become eligible simultaneously:

- sort by reveal priority;
- then stable ID;
- reveal at most configured number per event/day.

Avoid dumping two secrets and a defining moment at once.

---

## 75. Revelation Cooldown

Optional:
- minimum days between major biography reveals.

This preserves pacing.

Store next-eligible day only if needed.

---

## 76. Trust Loss After Reveal

Revealed fact remains known even if trust later drops.

Never hide it again.

Knowledge is monotonic.

---

## 77. Connection Death Before Reunion

A connection may be confirmed dead by world state/quest.

Update:
- status;
- reveal;
- grief hook.

Do not spawn them later.

---

## 78. Connection Duplicate Prevention

Two survivors could reference the same connection only if deliberately linked.

Default procedural generation creates unique connection IDs.

Future authored family connections can share an NPC ID explicitly.

---

## 79. Family Integration

If Plan 150 introduces family trees:
- BackstoryConnection may link to canonical family relation;
- do not maintain separate spouse/child truth.

Migration should merge only through explicit IDs.

---

## 80. Backstory Event Hooks

Source events:
- The Arrival;
- The Revelation;
- The Reunion;
- The Memory;
- The Past;
- The Secret;
- The Resolution.

Implement as event families or journal/quest hooks, not necessarily seven bespoke systems.

---

## 81. Achievement Integration

Plan 149 may observe:
- recruit diverse occupations;
- reveal biography facts;
- complete reunion;
- resolve backstory arc.

BackstorySystem does not own reward/meta state.

---

## 82. Epilogue Integration

Plan 145 may consume:
- occupation;
- defining moment;
- resolved connection;
- major secret;
- completed backstory arc.

Export stable fact IDs.

Do not author final survivor epilogue prose here.

---

## 83. Archive Integration

If a shelter archive exists, only notable biography milestones belong:
- reunion;
- major secret;
- backstory arc resolution.

Not every survivor occupation needs a global archive entry.

---

## 84. Modding Integration

Plan 165 can eventually expose:
- new occupations;
- experiences;
- defining moments;
- motivations;
- secrets.

Only after schemas and compatibility rules stabilize.

Mods cannot add executable behavior logic; they reference registered effect/reveal hook kinds.

---

## 85. Modded Backstory Save Safety

A save may contain modded biography IDs.

Plan 165's mod fingerprint must classify removal risk.

If mod removed:
- do not reroll biography;
- missing-definition compatibility report;
- preserve IDs structurally where possible.

Do not substitute a new random occupation.

---

## 86. Performance Budget

Generation occurs:
- at survivor/candidate creation;
- during old-save migration.

Runtime reveal checks occur on relevant events.

No per-frame scanning.

Pairwise relationship hook evaluation only on arrival/reveal.

Catalog pre-index by:
- tags;
- category;
- compatibility.

---

## 87. Save Footprint

Per survivor store:
- selected IDs;
- small connection states;
- secret states;
- revealed facts;
- digest.

Do not store all unused candidate templates or rendered prose.

---

## 88. Data Integrity Validation

Extend validator to ensure:

- unique occupation IDs;
- unique experience IDs;
- unique secret/moment/motivation IDs;
- all skill IDs resolve;
- all trait IDs resolve;
- all item IDs resolve;
- all behavior tags are registered if typed;
- compatibility references resolve;
- no impossible starting value ranges;
- localization keys exist;
- rarity values valid;
- no circular replacement/deprecation mapping.

---

## 89. Cross-Reference Validation

For each occupation:
- preferred experiences exist;
- forbidden experiences exist;
- item IDs exist.

For each experience:
- required/forbidden tags valid.

For each secret:
- reveal profile exists;
- consequence hooks registered.

Fail CI on invalid references.

---

## 90. Dedicated `--backstory-selftest`

The selftest should:

1. load all catalogs;
2. generate fixed survivor A;
3. generate same survivor twice and compare digest;
4. generate 100+ deterministic survivors;
5. validate occupation/experience compatibility;
6. apply starting effects once;
7. save/reload;
8. verify no duplicate modifiers/items;
9. trigger trust reveal;
10. verify reveal once;
11. generate connection and resolve status;
12. run old-save migration;
13. verify no retroactive stat/item grant;
14. verify UI projection hides secrets;
15. verify headless operation;
16. exit non-zero on mismatch.

---

## 91. Unit Test Matrix

### Catalog
- valid occupation;
- invalid skill;
- invalid item;
- duplicate ID;
- compatibility missing target;
- invalid localization.

### Generation
- authored;
- hybrid;
- procedural;
- deterministic;
- occupation weighting;
- experience filtering;
- max/min experience count.

### Starting effects
- skills;
- traits;
- possessions;
- once-only;
- legacy migration suppression.

### Reveals
- trust threshold;
- event trigger;
- multiple eligibility;
- cooldown;
- once-only.

### Connections
- unknown;
- reachable;
- confirmed dead;
- reunion;
- duplicate protection.

### Persistence
- new survivor;
- complex biography;
- secret states;
- old-save migration.

---

## 92. Golden Survivor Fixtures

Create fixed examples:

1. Former doctor + plague responder.
2. Former soldier + refugee experience.
3. Farmer + community organizer.
4. Engineer + disaster survivor.
5. Teacher + grief defining moment.
6. Merchant + betrayal secret.
7. Pilot/navigation survivor if skills exist.
8. Artist + cultural hobby integration.
9. Mechanic + mentorship potential.
10. Survivor with no secret.
11. Survivor with two secrets.
12. Survivor with three lost connections.
13. Fully authored survivor.
14. Hybrid survivor.
15. Old-save survivor whose current stats constrain biography.

Assert structural IDs, not English text.

---

## 93. Property / Fuzz Testing

Generate thousands of biographies.

Properties:
- all IDs resolve;
- experience count within bounds;
- no forbidden pair;
- no duplicate experience;
- mechanical budget within configured range;
- same seed stable;
- secret count bounded;
- connection count bounded;
- no duplicate starting grants;
- migrated survivors unchanged mechanically.

---

## 94. Distribution Test

Run deterministic generation over many synthetic survivor IDs.

Assert:
- every enabled occupation reachable;
- no occupation dominates unexpectedly;
- rare experiences remain rare;
- zero impossible/retry failures;
- average mechanical budget balanced.

Treat distribution changes as reviewable baseline changes.

---

## 95. Narrative Coverage Report

Generate:
`docs/backstories/BACKSTORY_COVERAGE.md`

Include:

| Layer | Definitions | Reachable | Used by fixture | Mechanical hook |
|---|---:|---:|---:|---:|
| occupation | | | | |
| experience | | | | |
| defining moment | | | | |
| motivation | | | | |
| secret | | | | |
| connection archetype | | | | |

Flag:
- unreachable definitions;
- definitions with no effect/hook;
- overused fragments;
- missing localization.

---

## 96. Mechanical Budget Report

Generate:
`docs/backstories/BACKSTORY_POWER_BUDGET.md`

For every occupation:
- skill delta sum;
- trait value estimate;
- item value estimate;
- disadvantages;
- net score.

This catches accidental "doctor/soldier always best" design.

---

## 97. Narrative Repetition Audit

With 10 occupations and 20 experiences, combinations can still feel repetitive.

Simulate 100 survivors and export readable biographies.

Review:
- repeated opening sentences;
- repeated defining moments;
- too many dead-family stories;
- too many military histories;
- implausible combinations;
- generic motivations;
- melodrama saturation.

Use authored variety, not uncontrolled randomness.

---

## 98. Backstory UI Accessibility

Support:
- keyboard navigation;
- text scaling;
- revealed/hidden state not color-only;
- clear separation between known fact and rumor/secret;
- screen-reader labels if framework supports;
- no hover-only mechanical explanation.

---

## 99. Arrival Notification Accessibility

New survivor biography summary must:
- be dismissible;
- not auto-scroll too quickly;
- use localized full text;
- avoid revealing hidden facts.

No mandatory cinematic.

---

## 100. Structured Diagnostics

Developer logs:

```text
BackstoryGenerated survivor=<id> occupation=<id> digest=<hash>
BackstoryStartingEffectsApplied survivor=<id>
BackstoryFactRevealed survivor=<id> fact=<id>
BackstoryConnectionUpdated survivor=<id> connection=<id> status=<status>
BackstoryMigrated survivor=<id> mode=legacy_no_starting_effects
```

Do not log hidden secret content in normal player logs.

---

## 101. Implementation Phase A — Audit and Contracts

Tasks:
1. map all survivor creation paths;
2. audit skills/traits/items;
3. audit existing flavor backstory strings;
4. define catalog schema;
5. define persisted state;
6. define generation profiles;
7. loader/validator;
8. baseline tests.

Exit:
catalog loads and DTOs round-trip.

---

## 102. Implementation Phase B — Deterministic Generator

Tasks:
1. stable seed;
2. occupation candidate filtering;
3. weighted selection;
4. experience filtering;
5. defining moment;
6. motivation;
7. connections;
8. secrets;
9. digest;
10. authored/hybrid modes.

Exit:
same survivor always gets same valid biography.

---

## 103. Implementation Phase C — Starting Effects

Tasks:
1. skill modifier sink;
2. trait modifier sink;
3. inventory grant sink;
4. power budget limits;
5. idempotency;
6. survivor creation transaction;
7. tests.

Exit:
new survivors gain mechanically meaningful but bounded origins.

---

## 104. Implementation Phase D — Legacy Migration

Tasks:
1. detect missing backstory;
2. build stat/trait constraints;
3. generate compatible biography;
4. mark starting effects already applied;
5. persist;
6. no notification flood;
7. migration fixtures.

Exit:
old survivors gain explanatory histories without stat changes.

---

## 105. Implementation Phase E — Revelation

Tasks:
1. known/hidden fact layers;
2. trust trigger;
3. event trigger;
4. reveal ordering;
5. idempotency;
6. journal/notification;
7. UI projection.

Exit:
biography unfolds during play.

---

## 106. Implementation Phase F — Connections

Tasks:
1. connection archetypes;
2. status state;
3. reveal;
4. quest/NPC link hook;
5. reunion/grief event;
6. save/load;
7. tests.

Exit:
lost connections are stable hooks, not fabricated NPC chaos.

---

## 107. Implementation Phase G — Behavior/Relations

Tasks:
1. autonomy behavior tags;
2. work preferences;
3. stress-response hooks;
4. motivation hooks;
5. relation compatibility events;
6. hobby/education hooks;
7. tests.

Exit:
past history affects present behavior through existing authorities.

---

## 108. Implementation Phase H — Quest/Moral/Journal

Tasks:
1. quest hook facts;
2. secret moral choice hooks;
3. reunion hooks;
4. resolution facts;
5. journal;
6. achievement facts;
7. epilogue facts.

Exit:
backstories can generate stories without owning those systems.

---

## 109. Implementation Phase I — UI

Tasks:
1. arrival summary;
2. backstory tab;
3. mechanical-source explanation;
4. known experiences;
5. defining moment;
6. connections;
7. revealed secrets;
8. accessibility;
9. snapshots.

Exit:
player can understand both narrative and mechanical origin.

---

## 110. Implementation Phase J — Content Authoring

Tasks:
1. validate ten occupation archetypes against actual skills/items;
2. author occupations;
3. author twenty life experiences;
4. author defining moments;
5. author motivations;
6. author secret pool;
7. author connection archetypes;
8. compatibility rules;
9. localization;
10. coverage fixtures.

Exit:
catalog has genuine breadth, not repeated templates.

---

## 111. Implementation Phase K — CI Hardening

Tasks:
1. selftest;
2. data integrity;
3. distribution test;
4. power budget;
5. fuzz;
6. old-save migration;
7. hidden-data UI test;
8. performance;
9. full regression;
10. docs.

Exit:
generation is stable, bounded, and supportable.

---

## 112. Recommended Occupation Authoring Matrix

Final definitions must use real IDs.

### Doctor
Narrative:
- trained medical professional or comparable caregiver.

Mechanical:
- medical proficiency.
Potential tradeoff:
- less combat familiarity, stress around preventable deaths.

Starting item:
- only if real medical item exists and value is balanced.

### Soldier
Mechanical:
- combat/discipline.
Tradeoff:
- social or trauma-related hooks, not arbitrary stat punishment.

### Farmer
Mechanical:
- agriculture/survival.
Tradeoff:
- lower high-tech familiarity if actual skills support.

### Engineer
Mechanical:
- technical/repair.
Tradeoff:
- not automatically poor socially unless authored trait system supports.

### Teacher
Mechanical:
- teaching/social/intellectual.
Strong integration:
- education/apprenticeship.

### Merchant
Mechanical:
- trade/social.
Strong integration:
- economy/faction.

### Pilot
Only if navigation/aviation history is plausible and relevant.

### Artist
Strong integration:
- hobby/culture.
Mechanical effect should remain useful but bounded.

### Cleric / spiritual leader
Only if spiritual/religious systems/lore support this neutral archetype.
Avoid assuming one real-world religion.

### Mechanic
Mechanical:
- repair/workshop.
Strong integration:
- shelter systems.

---

## 113. Life Experience Authoring Guidance

Each life experience should answer:

1. What happened?
2. Why does it matter mechanically?
3. Which trait/skill/behavior tag reflects it?
4. Which occupations prefer it?
5. Which combinations are forbidden?
6. Is it public or revealable?
7. Can it create a future hook?
8. Is it common/uncommon/rare?

Reject an experience that is pure flavor if the source plan's goal is mechanical relevance—unless it exists
explicitly as a narrative-only diversity fragment and is tagged accordingly.

---

## 114. Trauma Integration Guardrail

The source includes traumas.

Do not define trauma as just another `BackstorySystem` number.

If a trauma/mental-health authority exists:
- emit initial trauma/history fact;
- let that system own symptoms/effects.

If no such system exists:
- use behavior/stress tags;
- avoid pseudo-clinical diagnosis mechanics.

Backstory should not trivialize real mental-health conditions into random debuffs.

---

## 115. Secret Content Safety and Tone

Secrets should be varied:

- lied about occupation;
- abandoned someone;
- stole supplies;
- former faction affiliation;
- concealed technical knowledge;
- unresolved debt/promise;
- protected someone at personal cost.

Avoid making "secret" synonymous with:
- murderer;
- criminal;
- betrayal.

Too many extreme secrets destroy plausibility.

---

## 116. Defining Moment Diversity

Create categories:
- loss;
- rescue;
- escape;
- leadership;
- failure;
- sacrifice;
- discovery;
- moral compromise;
- perseverance.

Cap frequency per category in simulation review.

This avoids everyone having the same dead-family backstory.

---

## 117. Lost Connection Diversity

Connection archetypes:
- spouse/partner;
- child;
- sibling;
- parent;
- friend;
- colleague;
- mentor;
- student;
- unit member;
- neighbor.

Use actual family/relationship systems when available.

Do not assume every survivor has missing family.

---

## 118. Backstory Arc Resolution

A backstory arc can become "resolved" when:
- connection found/confirmed;
- promise fulfilled;
- secret confronted;
- defining location revisited;
- keepsake recovered.

Store stable `resolvedFactIds`.

No need for a universal "backstory complete" bar.

---

## 119. Survivor Death With Unrevealed Backstory

If survivor dies:
- hidden facts may remain hidden;
- journal/epilogue may reveal only what player knew;
- optional recovered diary/letter is a separate authored event.

Do not automatically expose all secrets at death unless design says so.

---

## 120. Survivor Departure

If survivor leaves:
- backstory remains historical;
- unresolved connection/quest hooks may cancel or transform;
- no further reveal ticks.

Quest system owns cancellation.

---

## 121. Recruitment Decision Value

Backstory makes recruitment more meaningful only if preview is neither:
- completely opaque;
- completely optimized stat spreadsheet.

Show:
- public occupation;
- known experience;
- obvious capabilities;
- some uncertainty.

This creates a person-first decision.

---

## 122. Avoiding Min-Max Backstory Shopping

Source says backstories cannot be chosen.

Also prevent:
- candidate rerolls;
- quick reload before roster generation;
- repeated recruitment screen refresh.

Persistent candidate IDs and seeds solve this.

Do not prevent players from choosing among legitimately different visible candidates.

---

## 123. Campaign Seed Compatibility

If campaigns can be imported/cloned:
- ensure campaign ID/seed behavior is documented.

Two intentionally identical seeded campaigns may produce identical procedural survivor biographies if survivor
IDs match. That can be acceptable for deterministic testing.

Normal runtime IDs should produce variety.

---

## 124. New Game+ Boundary

If Plan 175 later affects survivor recruitment:
- it may alter generation profile inputs;
- BackstorySystem still generates one run-local history.

No profile persistence lives here.

---

## 125. Achievement Boundary

"The Story — collect 10 unique backstories" is an achievement concern.

BackstorySystem exports:
- discovered biography IDs/counts.

Plan 149 owns completion state.

---

## 126. Verification Commands

Baseline:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --backstory-selftest
```

Also run bridge/save migration gates already present in repository.

---

## 127. Definition of Done — Flagship

### Core
- [ ] `BackstorySystem.cs`
- [ ] occupation catalog
- [ ] life-experience catalog
- [ ] defining moments
- [ ] motivations
- [ ] connections
- [ ] secrets
- [ ] schema-versioned state

### Generation
- [ ] authored mode
- [ ] hybrid mode
- [ ] procedural mode
- [ ] deterministic seed
- [ ] stable ordering
- [ ] compatibility filtering
- [ ] bounded counts
- [ ] generation digest

### Mechanics
- [ ] starting skill modifiers
- [ ] trait modifiers
- [ ] starting possessions
- [ ] idempotency
- [ ] behavior tags
- [ ] work preference hooks
- [ ] stress/morale hooks
- [ ] relation hooks

### Revelation
- [ ] known/hidden facts
- [ ] trust-based reveal
- [ ] event reveal
- [ ] secret state
- [ ] once-only reveal
- [ ] journal/UI

### Connections
- [ ] stable connection IDs
- [ ] reveal states
- [ ] death/reunion status
- [ ] quest/NPC hook
- [ ] no duplicate NPC authority

### Migration
- [ ] old survivors get biographies
- [ ] no retroactive starting bonuses
- [ ] generation constrained by existing stats
- [ ] stable save/load

### Validation
- [ ] ten occupations if repository supports them
- [ ] twenty experiences
- [ ] all refs resolve
- [ ] selftest
- [ ] fuzz
- [ ] distribution report
- [ ] power budget
- [ ] headless

---

## 128. Follow-On Task 174-A — Backstory Reachability & Distribution Auditor

Goal:
prove all authored biography fragments are reachable and not grossly over/underrepresented.

Substeps:
1. generate large deterministic corpus;
2. count each occupation;
3. count experiences;
4. count defining moments;
5. count secrets;
6. check invalid combinations;
7. compare mechanical budget;
8. publish report;
9. CI drift threshold.

---

## 129. Follow-On Task 174-B — Backstory Quest Template Pack

Goal:
turn stable hooks into reusable personalized quest templates.

Families:
- reunion;
- keepsake recovery;
- confront past;
- fulfill promise;
- return to location;
- protect former colleague.

Quest runtime remains owner.

---

## 130. Follow-On Task 174-C — Shared-History Links

Goal:
allow two survivors to procedurally/authoredly share a prior institution, location, or connection.

Requires:
- stable shared history ID;
- controlled pair generation;
- relation hooks;
- no O(N²) scan.

This can create strong emergent stories later.

---

## 131. Follow-On Task 174-D — Backstory Location Memory

Goal:
connect biography to world locations.

Examples:
- former workplace;
- hometown;
- evacuation route;
- battlefield;
- hospital.

Requires real location IDs and quest/map consumers.

---

## 132. Follow-On Task 174-E — Backstory Legacy Integration

Goal:
let Plan 145 remember resolved survivor origins.

Export:
- defining occupation;
- resolved connection;
- major secret;
- fulfilled motivation.

No cross-campaign storage.

---

## 133. Follow-On Task 174-F — Authoring & Preview Tool

Goal:
let writers preview generated biographies.

Tool should:
- validate JSON;
- generate sample survivors;
- show mechanical budget;
- show compatibility failures;
- show full/revealed projection;
- export corpus for prose review.

This is preferable to editing blindly.

---

## 134. Final Guardrails

- No `System.Random`.
- No wall-clock biography generation.
- No reroll on UI open.
- No reroll on save/load.
- No catalog-order-dependent generation.
- No duplicate skill/trait/inventory authority.
- No retroactive old-save bonuses.
- No arbitrary NPC spawning from lost connections.
- No hidden secret leakage.
- No secret reveal twice.
- No arbitrary quest creation without QuestSystem.
- No unrestricted live-generated prose.
- No survivor-pair O(N²) scan every tick.
- No occupation power tier dominating recruitment.
- No rare biography = automatically stronger survivor.
- No unsupported skill/item IDs invented to fill ten occupations.
- No all-survivors-have-dark-secret melodrama.
- No backstory profile/meta persistence.
- No deletion/reroll if a mod definition disappears.
- No starting-item grant duplication.

When complete, Plan 174 should make a survivor's mechanics and narrative reinforce each other. A high medical
skill will have a believable origin; a stress response can trace back to an authored defining moment; a hobby
or work preference can make sense in light of prior life; a hidden secret can emerge through trust; and a lost
connection can become a quest without BackstorySystem taking ownership of quests, NPCs, relationships, morale,
skills, or inventory.

The final proof is not merely that every survivor has more text. It is that the generated history is stable,
mechanically legible, narratively coherent, bounded, revealable over time, and safely integrated into the
existing survivor architecture.
