# D1 Flagship Integration Plan [5]
## Plan 149 — In-Campaign Achievement & Milestone System

> **Purpose:** Replace UI-derived achievement literals with one deterministic, data-driven Core achievement
> authority that observes a single campaign, tracks clear progress, emits durable once-only unlock events,
> persists run-local state, and projects that state into the achievement panel without creating a second
> profile/meta-progression system.
>
> **Authority boundary:** Plan 34 owns difficulty and the immutable campaign-completion record. Plan 149 owns
> achievement definitions, deterministic run-local evaluation, progress facts, unlock state, and the panel
> projection. Plan 175 remains the sole cross-campaign/meta owner. Plan 149 exports completed achievement IDs
> only; it never owns currency, prestige, permanent bonuses, New Game+, account/profile storage, or reward
> idempotency across campaigns.
>
> **Implementation posture:** preservation-first, deterministic, event-aware, save-compatible, catalog-driven,
> headless-testable, localization-ready, and designed to eliminate UI business logic rather than relocate it.
>
> **Primary defect:** `AchievementsPanel.cs` currently derives milestone-style achievements from live survivor
> state and day count. There is no Core achievement definition contract, no canonical catalog, no durable
> run-local unlock ledger, and no once-only achievement event authority. The panel is therefore both renderer
> and hidden game-rule engine.

---

## 1. Problem Statement

The current panel behavior creates a structural architecture defect rather than a cosmetic UI issue. Achievement
meaning is encoded in presentation code, which causes several downstream problems:

- achievement conditions cannot be tested independently of UI;
- names and thresholds can drift from actual game state;
- save/load cannot preserve once-only unlock semantics cleanly;
- event consumers cannot react to unlocks because no Core event exists;
- old campaigns cannot be retroactively evaluated in a controlled way;
- localization has no stable definition authority;
- future meta-progression risks reading UI state or duplicating logic;
- adding new achievements encourages more literals and condition branches in the panel.

Plan 149 therefore converts achievements into a **Core observable contract**:

```text
Game events / registered state snapshots
          ↓
Achievement observation facts
          ↓
AchievementSystem
          ↓
deterministic condition evaluator
          ↓
run-local AchievementState
          ↓
AchievementUnlocked(id, campaignId) exactly once
          ↓
panel projection + journal/telemetry/other run-local listeners
          ↓
read-only completed-id export at campaign completion
          ↓
Plan 175 decides whether any cross-campaign reward exists
```

The critical design rule is that the achievement system recognizes facts; it does not invent a new profile.

---

## 2. Scope Boundaries and Ownership Matrix

### Plan 149 owns

- achievement definition schema;
- catalog loading and validation;
- deterministic condition evaluation;
- event observation and normalized state snapshots;
- run-local progress facts;
- completed achievement IDs for the current campaign;
- once-only unlock event emission;
- save capture/restore for run-local achievement state;
- retroactive evaluation inside the current campaign;
- achievement panel projection;
- optional epilogue tags stored on definitions;
- headless selftests and data-integrity checks;
- read-only completion export for downstream consumers.

### Plan 149 explicitly does not own

- cross-campaign persistence;
- profile storage;
- account-level achievement history;
- currency;
- prestige;
- reward grants;
- permanent unlockables;
- starting bonuses;
- New Game+;
- difficulty ownership;
- immutable campaign-completion record;
- platform achievement APIs unless a later plan explicitly adds an adapter;
- reward copy that promises effects not implemented elsewhere.

### Plan 34 owns

- difficulty contract;
- campaign completion record;
- any canonical completed-run snapshot needed at terminal campaign completion.

### Plan 175 owns

- cross-campaign storage;
- reward/meta-progression rules;
- idempotency of profile-level rewards;
- profile-level completion history;
- any permanent unlocks or account currency;
- interpreting Plan 149's exported completed IDs.

### Boundary invariant

Plan 149 may expose:

```csharp
IReadOnlySet<string> GetCompletedAchievementIds();
```

or an immutable completion export DTO. It must never call a Plan 175 reward API from achievement condition code.

---

## 3. Flagship Success Criteria

The implementation is complete only when all of the following are simultaneously true:

1. `AchievementsPanel.cs` contains no milestone condition logic.
2. Every panel entry is backed by an `AchievementDefinition`.
3. All definitions load from a canonical data catalog.
4. All condition kinds are explicitly registered and versioned.
5. Evaluation is deterministic for the same ordered fact/state input.
6. Unlock events fire exactly once per achievement per campaign.
7. Unlock state survives save/load.
8. Progress shown in UI derives from Core state, never recomputed ad hoc by the panel.
9. Old in-progress campaigns can be retroactively evaluated without duplicate unlock events.
10. No achievement definition contains reward, currency, prestige, or New Game+ fields.
11. Plan 175 receives only completed IDs or a minimal immutable export.
12. Missing/invalid catalog references fail validation.
13. A dedicated achievements selftest proves panel/catalog parity.
14. Headless operation does not depend on UI scenes.
15. No use of `System.Random` exists in condition evaluation.
16. No unversioned reflection scan is used to infer game state.
17. Localization keys, category IDs, event kinds, referenced survivor/faction/quest/item IDs, and epilogue tags
    resolve where applicable.
18. Save migration is schema-versioned.
19. Achievement IDs are stable and never silently repurposed.
20. Condition evaluation remains bounded and does not rescan the entire campaign world every frame.

---

## 4. Repository Reconnaissance Before Editing

Before writing the new system, create `docs/achievements/ACHIEVEMENT_INTEGRATION_AUDIT.md`.

Inspect:

- `AchievementsPanel.cs` and every helper it calls;
- panel labels, threshold constants, day-count rules, survivor-count rules, and category groupings;
- current campaign save DTOs;
- bootstrap/composition root;
- event bus or domain event infrastructure;
- campaign clock/day authority;
- survivor roster/fate state;
- combat outcome events;
- faction/social state;
- expedition discovery/completion state;
- trade/economy counters;
- moral choice system/band;
- quest completion ledger;
- research/crafting/build systems if existing panel literals depend on them;
- localization loader and string key conventions;
- any existing journal notification/toast system;
- any campaign completion export used by Plan 34;
- any early Plan 175 scaffolding that might accidentally duplicate ownership.

For every current panel literal, record:

| Current panel row | Display text source | Rule source | Threshold | State queried | Save-backed? | Migration target |
|---|---|---|---:|---|---:|---|
| Example row | hardcoded | panel | N | day count | yes/no | achievement id |

Do not add new achievements until all current panel semantics are inventoried and migrated.

---

## 5. Canonical File Layout

Recommended new files, adjusted to repository conventions:

### Core

- `Assets/Ashfall.Core/Achievements/AchievementSystem.cs`
- `Assets/Ashfall.Core/Achievements/AchievementDefinition.cs`
- `Assets/Ashfall.Core/Achievements/AchievementState.cs`
- `Assets/Ashfall.Core/Achievements/AchievementCondition.cs`
- `Assets/Ashfall.Core/Achievements/AchievementConditionEvaluator.cs`
- `Assets/Ashfall.Core/Achievements/AchievementProgress.cs`
- `Assets/Ashfall.Core/Achievements/AchievementEvents.cs`
- `Assets/Ashfall.Core/Achievements/AchievementCatalog.cs`
- `Assets/Ashfall.Core/Achievements/AchievementCatalogLoader.cs`
- `Assets/Ashfall.Core/Achievements/AchievementCompletionExport.cs`

### Data

- `Assets/StreamingAssets/Data/achievements.json`

### Host / wiring

- composition-root additions for setup/restore/save/event wiring;
- host selftest registration;
- panel projection adapter or presenter.

### Tests

- `Ashfall.Core.Tests/Achievements/AchievementSystemTests.cs`
- `Ashfall.Core.Tests/Achievements/AchievementConditionEvaluatorTests.cs`
- `Ashfall.Core.Tests/Achievements/AchievementPersistenceTests.cs`
- `Ashfall.Core.Tests/Achievements/AchievementPanelProjectionTests.cs`
- `Ashfall.Core.Tests/Achievements/AchievementMigrationTests.cs`
- `Ashfall.Core.Tests/Achievements/AchievementCatalogValidationTests.cs`

### Documentation

- `docs/achievements/ACHIEVEMENT_INTEGRATION_AUDIT.md`
- `docs/achievements/ACHIEVEMENT_SCHEMA.md`
- `docs/achievements/ACHIEVEMENT_COVERAGE.md`
- `docs/achievements/ACHIEVEMENT_MIGRATION.md`

---

## 6. Achievement Definition Contract

Define a compact data contract.

Illustrative shape:

```csharp
public sealed record AchievementDefinition
{
    public string Id { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public string Category { get; init; }
    public IReadOnlyList<AchievementCondition> Conditions { get; init; }
    public string? EpilogueTag { get; init; }
    public int SortOrder { get; init; }
    public bool HiddenUntilProgress { get; init; }
}
```

### Forbidden fields

The schema must reject fields such as:

- `reward`
- `currency`
- `prestige`
- `profileUnlock`
- `newGamePlus`
- `startingBonus`
- `accountReward`
- `permanentUnlock`

The absence of these fields is an ownership guarantee, not a temporary omission.

### Stable ID policy

Achievement IDs are save-contract IDs. Once shipped, never change semantics under the same ID. If a condition
must fundamentally change, either:
- version the condition compatibly; or
- deprecate the old achievement and create a new stable ID.

Display text may change without changing the ID.

---

## 7. Catalog Schema

Recommended JSON structure:

```json
{
  "schemaVersion": 1,
  "categories": [
    { "id": "survival", "displayKey": "achievement.category.survival" },
    { "id": "combat", "displayKey": "achievement.category.combat" },
    { "id": "social", "displayKey": "achievement.category.social" },
    { "id": "exploration", "displayKey": "achievement.category.exploration" },
    { "id": "economic", "displayKey": "achievement.category.economic" },
    { "id": "moral", "displayKey": "achievement.category.moral" }
  ],
  "achievements": []
}
```

Each condition should be structured rather than textual.

Example:

```json
{
  "id": "survive_30_days",
  "titleKey": "achievement.survive_30_days.title",
  "descriptionKey": "achievement.survive_30_days.description",
  "category": "survival",
  "conditions": [
    {
      "kind": "campaign_day_at_least",
      "value": 30
    }
  ],
  "epilogueTag": "legacy.endured_first_month",
  "sortOrder": 100
}
```

Do not parse arbitrary expression strings at runtime.

---

## 8. Condition Model

Use a finite, explicit condition vocabulary.

Initial condition kinds should be derived from the migrated panel rules and real stable Core facts. Candidate
families include:

### Campaign / survival
- `campaign_day_at_least`
- `survivors_alive_at_least`
- `survivors_alive_at_most`
- `no_deaths_until_day`
- `crisis_survived_count_at_least`

### Combat
- `combat_victory_count_at_least`
- `combat_no_casualty_victory_count_at_least`
- `enemy_type_defeated_count_at_least`
- `raid_survived_count_at_least`

### Social
- `faction_standing_at_least`
- `refugees_admitted_count_at_least`
- `commitments_kept_count_at_least`
- `relationship_fact_present`

### Exploration
- `expedition_completed_count_at_least`
- `discovery_id_completed`
- `unique_locations_discovered_at_least`

### Economic
- `trade_value_lifetime_at_least`
- `crafted_item_count_at_least`
- `resource_stock_at_least`
- `successful_trade_count_at_least`

### Moral
- `moral_band_at_least`
- `moral_choice_id_resolved_as`
- `mercy_choice_count_at_least`

Only implement kinds whose source state is authoritative and inspectable.

---

## 9. Condition Versioning

Each condition kind must have explicit parsing and evaluation semantics.

Recommended normalized contract:

```csharp
public sealed record AchievementCondition
{
    public string Kind { get; init; }
    public int Version { get; init; } = 1;
    public string? TargetId { get; init; }
    public long? Threshold { get; init; }
    public string? ExpectedValue { get; init; }
}
```

Why version condition kinds:
- save migration may preserve progress facts;
- future balance work may change condition implementation;
- catalog changes can otherwise reinterpret historical campaigns silently.

Condition versioning should be rare, but the contract must allow it.

---

## 10. Run-Local Achievement State

Define a state object that can round-trip in campaign saves.

Illustrative shape:

```csharp
public sealed record AchievementState
{
    public int SchemaVersion { get; init; }
    public HashSet<string> CompletedIds { get; init; }
    public Dictionary<string, AchievementProgressState> Progress { get; init; }
    public HashSet<string> EmittedEventIds { get; init; }
    public long LastEvaluatedSequence { get; init; }
}
```

### State meaning

`CompletedIds`
: achievement IDs completed in this campaign.

`Progress`
: normalized progress facts needed to render clear progress or preserve counters that cannot be reconstructed
  cheaply.

`EmittedEventIds`
: idempotency ledger for unlock event emission during this campaign.

`LastEvaluatedSequence`
: optional event-sequence cursor if the event architecture supports durable sequence numbers.

### Important

Do not store a second profile. This object dies with the campaign save.

---

## 11. Unlock Event Contract

Emit:

```csharp
public sealed record AchievementUnlocked(
    string AchievementId,
    string CampaignId,
    string EventId
);
```

Event ID should be deterministic:

```text
achievement:<campaignId>:<achievementId>
```

or equivalent.

### Exactly-once logical semantics

The underlying event bus may technically be at-least-once, but the achievement system must guarantee logical
once-only unlocks by checking state before publishing.

Algorithm:

```text
if completedIds contains id:
    do nothing
else:
    add completed id
    add deterministic event id
    persist/mark dirty
    emit AchievementUnlocked
```

If persistence and event publication are not transactional in the current architecture, define a replay-safe
sequence so restored saves do not duplicate unlock effects.

---

## 12. Event Observation Architecture

Achievement evaluation should not subscribe to every arbitrary object mutation. Build a small observation
contract.

Two input classes:

### Explicit domain events

Examples:
- DayAdvanced
- SurvivorDied
- CombatResolved
- ExpeditionCompleted
- DiscoveryUnlocked
- TradeCompleted
- MoralChoiceResolved
- FactionStandingChanged
- ItemCrafted
- CrisisResolved

### Registered state snapshots

Used for facts that are easier/safer to query from canonical state:
- current day
- survivors alive
- current faction standing
- current resource stock
- current moral band

The system may evaluate on event boundaries using the snapshot provider. It must not run a reflection scan every
frame.

---

## 13. Normalized Achievement Facts

Introduce a normalized fact layer if direct coupling would otherwise explode.

Example:

```csharp
public interface IAchievementFactSource
{
    AchievementFactSnapshot CaptureAchievementFacts();
}
```

Snapshot may contain:

```text
CampaignDay
AliveSurvivorCount
TotalDeaths
CombatVictories
NoCasualtyCombatVictories
ExpeditionsCompleted
UniqueDiscoveries
TradeValueLifetime
CraftedItemsLifetime
RefugeesAdmitted
MoralBand
FactionStandings
CompletedQuestIds
ResolvedMoralChoiceIds
```

Only include facts supported by canonical sources.

This keeps `AchievementConditionEvaluator` pure and unit-testable.

---

## 14. Pure Evaluation Contract

The evaluator should behave like:

```csharp
AchievementEvaluation Evaluate(
    AchievementDefinition definition,
    AchievementFactSnapshot snapshot,
    AchievementState state);
```

It returns:
- completed/not completed;
- current progress;
- target progress;
- optional reason/debug facts.

It must not:
- modify state;
- publish events;
- query UI;
- call RNG;
- inspect arbitrary services;
- read disk.

`AchievementSystem` owns orchestration and state mutation after pure evaluation.

---

## 15. Progress Model

The source goal explicitly asks for clear progress.

Support progress only where semantically meaningful.

Example DTO:

```csharp
public sealed record AchievementProgress
{
    public string AchievementId { get; init; }
    public AchievementProgressKind Kind { get; init; }
    public long Current { get; init; }
    public long Target { get; init; }
    public bool Completed { get; init; }
}
```

Progress kinds:
- binary
- counter
- threshold
- collection-count
- multi-condition

Avoid exposing raw internal counters if they do not map clearly to player expectations.

---

## 16. Multi-Condition Semantics

Definitions may require multiple conditions.

Initial semantics should be explicit:

- default `all` / AND;
- optional `any` / OR only if real achievements need it;
- no nested arbitrary boolean trees in v1 unless existing panel rules require them.

Example:

```json
{
  "conditionMode": "all",
  "conditions": [
    { "kind": "campaign_day_at_least", "threshold": 30 },
    { "kind": "survivors_alive_at_least", "threshold": 5 }
  ]
}
```

The panel can show aggregate progress but the evaluator must retain per-condition debug state for tests.

---

## 17. Migrating Existing Panel Literals

This is the first content task, before inventing new achievements.

### Migration procedure

1. Enumerate every literal row.
2. Give each a stable ID.
3. Move title/description into localization.
4. Move category into catalog.
5. Convert threshold/rule into supported condition kind.
6. Write a parity fixture with the old panel logic.
7. Add it to `achievements.json`.
8. Change panel to consume the projection.
9. Remove the old literal/rule.
10. Search repository for leftover duplicate literals.

### Migration acceptance

For representative old panel states, old and new visible completion status must match unless an intentional
correction is documented.

---

## 18. Panel Projection Contract

The panel must render only a projection provided by Core/application layer.

Example:

```csharp
public sealed record AchievementPanelEntry
{
    public string Id { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public string Category { get; init; }
    public bool Completed { get; init; }
    public AchievementProgress Progress { get; init; }
    public int SortOrder { get; init; }
}
```

The panel may:
- localize keys;
- format numbers;
- render progress bars;
- group categories;
- animate unlock state.

The panel may not:
- decide whether an achievement is complete;
- calculate threshold facts;
- query survivor/day systems directly;
- invent labels outside the catalog.

---

## 19. UI Binding Order

On load:

```text
load catalogs
construct AchievementSystem
restore AchievementState
perform controlled retroactive evaluation if required
build panel projection
bind panel
```

The source explicitly says restore must occur before panel binds. This prevents:
- completion flicker;
- duplicate unlock animations;
- old campaigns showing false incomplete rows before correction.

---

## 20. Retroactive Evaluation for Old In-Progress Campaigns

Old saves may have no `AchievementState`, but canonical campaign facts already imply some achievements should be
complete.

### Migration strategy

1. Detect missing/legacy achievement state.
2. Build current fact snapshot from persisted campaign state.
3. Evaluate all definitions marked `retroactive=true` or all safe snapshot-based definitions.
4. Populate completed IDs.
5. Decide whether migration unlock events should be emitted.

Recommended behavior:
- populate completion state;
- suppress noisy historical unlock popups by default;
- mark emitted event IDs as consumed for retroactively completed achievements;
- optionally write a migration diagnostic.

For event-history-only achievements that cannot be reconstructed, do not fabricate completion.

---

## 21. Retroactivity Classification

Each condition kind should declare one of:

### Fully reconstructable
Can be evaluated from current persisted state.
Examples:
- day threshold;
- current survivors alive;
- current faction standing;
- completed quest ID.

### Reconstructable from persisted aggregate
Requires a durable counter already stored.
Examples:
- lifetime trades;
- combat victories;
- expeditions completed.

### Event-history dependent
Cannot be reconstructed unless event history is persisted.
Examples:
- "win three combats consecutively without injury" if no streak ledger exists.

For v1, prefer reconstructable achievements.

---

## 22. Save Schema and Migration

Add achievement state to the canonical campaign save.

Requirements:
- schema version;
- stable IDs;
- unknown deprecated IDs tolerated but preserved if needed;
- missing state initializes migration path;
- completion cannot regress merely because a definition disappeared from current catalog;
- progress state is compact.

Create `docs/achievements/ACHIEVEMENT_MIGRATION.md` with cases:

| Save case | Expected behavior |
|---|---|
| New save | empty state |
| Old save, no achievement block | retroactive evaluation |
| Save with completed deprecated id | preserve id, hide if catalog removed |
| Save with unknown condition-version progress | migrate or discard progress safely |
| Save after unlock before UI opened | completed remains complete |

---

## 23. Catalog Reload Policy

Treat achievement definitions as startup-loaded data.

Do not hot-reload during a live campaign unless the project already has a safe data-reload protocol.

Reason:
- changing thresholds mid-session can retroactively unlock or relock;
- save semantics become ambiguous;
- event emission can duplicate.

If debug hot reload is supported:
- invalidate projection;
- never remove completed IDs;
- clearly mark as developer-only.

---

## 24. Achievement Categories

The source requests a balanced catalog across:

- survival;
- combat;
- social;
- exploration;
- economic;
- moral.

Define category IDs in data and keep panel grouping data-driven.

Do not create categories with zero actual achievements merely for symmetry.

---

## 25. Survival Achievement Design

Good run-local survival achievements reward meaningful campaign thresholds without owning difficulty.

Examples of condition patterns:
- survive N days;
- keep N survivors alive by day threshold;
- survive first major crisis;
- maintain zero deaths through an early interval;
- recover from a low-stock emergency if a canonical event exists.

Guardrails:
- do not encode difficulty multipliers here;
- do not reward profile currency;
- avoid achievements that incentivize boring stalling unless survival time itself is a meaningful objective.

---

## 26. Combat Achievement Design

Use authoritative combat outcome events.

Potential patterns:
- first combat victory;
- N victories;
- no-casualty victory;
- repel major raid;
- defeat specific enemy archetype if catalog-backed.

Guardrails:
- no UI-derived kill count;
- no event duplication across combat summary screens;
- no requirement based on non-persisted temporary animation state;
- avoid rewards or permanent combat bonuses.

---

## 27. Social Achievement Design

Potential patterns:
- reach standing threshold with a faction;
- admit N refugees;
- resolve a commitment favorably;
- maintain allied relations across multiple factions;
- complete a social questline.

Guardrails:
- faction IDs must resolve;
- standing thresholds use canonical standing scale;
- if standing can drop later, decide whether achievement records first-time threshold crossing or final-state
  condition. Prefer first-time threshold crossing for normal achievements.

---

## 28. Exploration Achievement Design

Potential patterns:
- first expedition completed;
- discover N unique locations;
- discover one specific major location;
- complete a major expedition chain;
- return from hazardous region.

Use canonical discovery IDs and expedition result events.

Do not infer discoveries from UI map visibility unless map state is the actual authority.

---

## 29. Economic Achievement Design

Potential patterns:
- craft N items;
- complete N trades;
- reach lifetime traded-value threshold;
- maintain a specific resource reserve;
- construct a major production upgrade.

Important distinction:
- lifetime counters are preferable for "do X N times";
- current-stock snapshots are appropriate for "hold N resources at once."

Never grant currency from the achievement itself.

---

## 30. Moral Achievement Design

Moral achievements are especially sensitive because they can become prescriptive.

Potential patterns:
- reach a moral band;
- resolve a specific authored choice;
- demonstrate repeated mercy/ruthlessness;
- complete a moral quest arc.

Guardrails:
- titles/descriptions should describe what happened rather than promise mechanical virtue rewards;
- hidden achievements may be appropriate for spoilers;
- moral condition IDs must map to canonical moral-choice outcomes.

---

## 31. Hidden and Spoiler Achievements

If hidden achievements are supported:

Definition fields may include:
- `visibility: visible|hidden_until_progress|hidden_until_complete`.

Panel behavior:
- hidden title/description remain secret until permitted;
- completion still evaluates normally;
- no secret condition data leaked through progress numbers;
- save state remains identical.

Keep visibility a presentation property, not a reward system.

---

## 32. Epilogue Tag Integration

The source allows an optional epilogue tag.

Purpose:
- enable Plan 145/unified ending systems to reference notable campaign accomplishments.

Rules:
- epilogue tag is descriptive metadata;
- it must not alter achievement completion;
- it must not create cross-campaign reward authority;
- tags must resolve against the epilogue/legacy tag catalog if such a catalog exists;
- duplicate tags across achievements should be intentional.

Example:
```text
achievement: survive_100_days
epilogueTag: legacy.long_endurance
```

---

## 33. Plan 34 Integration

Plan 34 owns the immutable campaign-completion record.

At campaign completion:
- Plan 149 freezes/exports completed achievement IDs for that run;
- Plan 34 may include the export in the immutable completion record if its schema calls for it;
- Plan 149 does not own the completion record.

No achievement should require mutating Plan 34's record after finalization without an explicit compatibility
contract.

---

## 34. Plan 175 Handoff

Create a read-only DTO:

```csharp
public sealed record AchievementCompletionExport
{
    public string CampaignId { get; init; }
    public IReadOnlyList<string> CompletedAchievementIds { get; init; }
}
```

Optional:
- achievement catalog revision;
- campaign completion timestamp only if Plan 34 provides it;
- completion digest.

Do not include:
- rewards;
- profile IDs;
- prestige deltas;
- permanent unlocks.

Plan 175 decides:
- whether IDs are new globally;
- whether any reward is due;
- how to persist profile state;
- how to prevent cross-campaign duplicate rewards.

---

## 35. Export Idempotency

The run-local export may be read multiple times.

Requirements:
- immutable ordering, preferably sorted IDs;
- no side effects;
- no reward calls;
- same state -> same export;
- no clearing completed IDs after export.

This makes Plan 175 integration safe.

---

## 36. Achievement System Lifecycle

Recommended lifecycle:

```text
Construct
  -> load catalog
  -> validate catalog
  -> restore state
  -> migrate if required
  -> subscribe to explicit event stream
  -> capture initial fact snapshot
  -> retroactively evaluate safe definitions
  -> become Active
```

During campaign:

```text
domain event
  -> update fact cache / request snapshot
  -> determine impacted achievement definitions
  -> evaluate impacted definitions only
  -> update progress
  -> complete newly satisfied IDs
  -> emit once-only unlock events
  -> mark save dirty
```

---

## 37. Avoid Full-Catalog Re-Evaluation on Every Event

Index definitions by relevant condition kinds.

Example:

```text
DayAdvanced -> campaign_day_at_least, no_deaths_until_day
CombatResolved -> combat_victory_count_at_least, no_casualty...
ExpeditionCompleted -> expedition_completed_count_at_least, discovery...
MoralChoiceResolved -> moral_choice_id_resolved_as, moral_band...
```

The first implementation may evaluate all definitions if the catalog is tiny, but the architecture should expose
an impact index so growth does not create frame-time work.

---

## 38. No Per-Frame Polling

Achievement evaluation should be event-driven or explicitly checkpointed.

Forbidden:
```csharp
_process(delta) => evaluate all achievements
```

Acceptable:
- day advance;
- transaction completion;
- combat resolution;
- expedition return;
- moral choice commit;
- save restore;
- explicit UI refresh requesting existing projection only.

Opening the panel must not cause gameplay evaluation side effects.

---

## 39. Determinism Rules

- no RNG;
- no wall-clock time;
- no unordered dictionary iteration affecting outcome;
- no localization string parsing;
- no UI text parsing;
- no reflection-discovered rule order;
- no engine frame count;
- no hash-order dependence.

Same normalized facts + same catalog revision + same completed state => same new completions and same progress.

---

## 40. Achievement Evaluation Ordering

If multiple achievements complete on one event:

1. determine all newly completed IDs;
2. sort by explicit catalog sort order then stable ID;
3. commit all completion state;
4. emit events in deterministic order.

This avoids platform-dependent toast ordering.

---

## 41. Notification and Toast Integration

Achievement unlock UI is a consumer.

Rules:
- Core emits event;
- notification presenter localizes title;
- notification animation may be skipped in reduced-motion mode;
- opening panel is not required;
- save/load does not replay historical toasts unless intentionally requested;
- notification failure does not roll back completion.

Do not put toast logic inside `AchievementSystem`.

---

## 42. Journal Integration

Optional run-local journal entry can subscribe to `AchievementUnlocked`.

If used:
- one entry per deterministic event ID;
- localized title/description;
- no reward language;
- replay safe;
- no second completion authority.

---

## 43. Localization

Every definition should use keys.

Validate:
- title key exists;
- description key exists;
- category display key exists;
- hidden placeholder key exists if needed;
- progress format key exists for non-standard progress.

Do not place English achievement labels in panel code.

---

## 44. Data Integrity Validation

Extend the standard data-integrity selftest.

Validate:

- unique achievement IDs;
- valid schema version;
- valid category IDs;
- valid condition kinds;
- valid condition versions;
- legal thresholds;
- known target IDs;
- valid localization keys;
- epilogue tags resolve if registry exists;
- no forbidden reward/meta fields;
- no duplicate semantic definition IDs;
- at least one condition per non-placeholder achievement;
- panel projection can be built for every definition.

---

## 45. Dedicated Achievements Self-Test

Add a host verb, preferably:

```text
--achievements-selftest
```

It should prove:

1. catalog loads;
2. every definition validates;
3. every migrated panel entry exists in catalog;
4. panel contains no non-catalog achievement row;
5. condition evaluators exist for every condition kind/version;
6. deterministic fixtures resolve correctly;
7. once-only emission holds;
8. save round-trip preserves completed/progress/event IDs;
9. retroactive old-campaign fixture evaluates safely;
10. Plan 175 export contains IDs only;
11. forbidden reward fields are absent;
12. headless run does not instantiate achievement UI.

---

## 46. Panel Catalog-Backed Proof

Create a test that obtains the panel projection and asserts:

```text
projection entry ids == catalog visible entry ids
```

subject to explicit visibility filters.

Also search/inspect `AchievementsPanel.cs` and fail a source-level guard if practical when old milestone literal
patterns return.

The strongest architectural proof is that the panel constructor receives a projection provider and has no
survivor/day service dependencies.

---

## 47. Unit Test Matrix

### Definition tests
- parse valid definition;
- reject missing ID;
- reject duplicate ID;
- reject forbidden reward field;
- reject bad condition kind;
- reject missing localization key.

### Evaluator tests
- threshold below;
- threshold equal;
- threshold above;
- multi-condition AND;
- optional OR if supported;
- target ID match/mismatch;
- deterministic repeat.

### System tests
- no unlock;
- single unlock;
- multiple simultaneous unlocks;
- no duplicate event;
- completed stays completed if state later falls below threshold;
- deterministic event order.

### Save tests
- empty state;
- partial progress;
- completed IDs;
- emitted IDs;
- migration from missing block.

### Panel tests
- catalog order;
- categories;
- progress;
- hidden achievements;
- completed state;
- no hardcoded extras.

---

## 48. Golden Campaign Achievement Fixtures

Create fixed snapshots representing:

1. Day 1 starting campaign.
2. Day threshold just below first milestone.
3. Exact threshold.
4. Mature campaign with multiple categories complete.
5. Combat-heavy campaign.
6. Social-heavy campaign.
7. Exploration-heavy campaign.
8. Economic-heavy campaign.
9. High-positive moral campaign.
10. High-negative moral campaign if such achievement definitions exist.
11. Old save with no achievement state.
12. Save with several already completed IDs.
13. Save where current facts fell below a previously completed threshold.
14. Sparse campaign with almost no progress.
15. Campaign completion export fixture.

Use stable IDs as assertions, not rendered English strings.

---

## 49. Retroactive Migration Tests

Test:

- fully reconstructable threshold unlocks;
- completed quest-based unlocks;
- faction-standing current-state unlocks;
- non-reconstructable event-history achievement remains incomplete;
- no historical unlock toast spam;
- emitted event IDs populated according to migration policy;
- second reload does not change state.

---

## 50. Property / Fuzz Testing

Generate random valid snapshots and catalogs within known condition kinds.

Properties:

- evaluator never mutates input;
- repeated evaluation is stable;
- completed IDs only grow within one campaign;
- progress never reports completed=false for a completed ID;
- no duplicate unlock event IDs;
- export is sorted/stable;
- invalid target IDs fail validation before runtime;
- all panel rows resolve definition metadata.

---

## 51. Performance Budget

Achievements are low-frequency gameplay logic, but implementation should still be bounded.

Targets:
- catalog parsed once at startup;
- no per-frame scanning;
- event impact index avoids unnecessary definitions;
- progress update allocations kept small;
- panel projection cached/updated on state changes;
- save footprint proportional to achievements, not event history.

Add a benchmark if definitions scale beyond a few hundred.

---

## 52. Observability

Structured diagnostic logs:

```text
AchievementCatalogLoaded count=<n> revision=<hash>
AchievementEvaluation trigger=<event-kind> candidates=<n>
AchievementUnlocked id=<id> campaign=<id>
AchievementStateRestored completed=<n>
AchievementRetroactiveMigration completed=<n> suppressedEvents=<n>
AchievementExportCreated count=<n>
```

Do not log entire save payloads.

---

## 53. Catalog Revision and Digest

Compute a deterministic catalog revision from normalized definition data.

Store revision optionally in achievement state for diagnostics.

Do not invalidate completed achievements solely because the catalog revision changes.

Use revision to:
- detect migrations;
- reproduce bug reports;
- compare old campaign behavior;
- gate unexpected definition drift.

---

## 54. Definition Change Policy

Classify changes:

### Safe presentation change
- title/description wording;
- sort order;
- category display label.

### Potentially semantic change
- threshold;
- condition kind;
- target ID;
- condition mode.

Semantic changes require:
- migration note;
- regression fixtures;
- decision on old in-progress campaigns.

Never silently lower a threshold and cause a wave of unlock events on load without an intentional retroactive
policy.

---

## 55. Achievement Coverage Report

Generate `docs/achievements/ACHIEVEMENT_COVERAGE.md`.

Include:

| Category | Definitions | With progress | Retroactive-safe | Tested |
|---|---:|---:|---:|---:|
| survival | | | | |
| combat | | | | |
| social | | | | |
| exploration | | | | |
| economic | | | | |
| moral | | | | |

Also report:
- condition kinds in use;
- unreferenced condition evaluators;
- definitions never exercised by fixtures;
- panel/catalog parity;
- epilogue tags;
- hidden definitions;
- deprecated IDs.

---

## 56. Initial Catalog Expansion Strategy

After migrating every panel literal, add only a balanced first wave.

Recommended target: 24–36 achievements, not hundreds.

Example balance:
- 5–7 survival;
- 4–6 combat;
- 4–6 social;
- 4–6 exploration;
- 4–6 economic;
- 3–5 moral.

Actual count should follow system maturity. Do not add achievements for features that are not wired or reliable.

---

## 57. Naming and Description Guidelines

Achievement copy should:

- describe observable accomplishments;
- avoid implying permanent rewards;
- avoid spoilers unless hidden;
- avoid fake mechanical bonuses;
- remain concise in panel width;
- use vocabulary consistent with the game;
- avoid moralizing every morally ambiguous choice;
- avoid platform-specific terms unless platform achievements are later integrated.

Examples of bad text:
- "Unlocks +10% starting food forever."
- "Earn 500 prestige."
- "New Game+ perk unlocked."

Those belong to Plan 175 if they ever exist.

---

## 58. Campaign Reset Semantics

Starting a new campaign creates a fresh `AchievementState`.

Completed IDs from prior campaigns do not automatically carry into Plan 149's new run.

If the profile/meta layer wants to display lifetime completion, that is Plan 175.

This avoids the subtle failure where panel state becomes a hidden profile because completed IDs leak across runs.

---

## 59. Campaign Completion Semantics

When the run completes:

1. ensure achievement evaluation is up to date;
2. freeze current run-local achievement state;
3. produce read-only completed-ID export;
4. allow Plan 34 completion record to reference the export if architecturally appropriate;
5. allow Plan 175 to consume it later;
6. do not mutate the run-local state as a reward side effect.

Any "complete the campaign" achievement must resolve before the export snapshot.

---

## 60. Exploit and Duplication Prevention

Run-local exploits to prevent:

- loading the same save and receiving duplicate unlock events;
- opening/closing panel to trigger unlocks;
- repeated event replay generating duplicate completion;
- save rollback causing reward duplication inside Plan 149;
- duplicate achievement IDs in data;
- a condition being evaluated by both old panel code and new Core code.

Important: cross-campaign reward duplication remains Plan 175's responsibility.

---

## 61. Bootstrap Wiring

Use the existing composition-root style.

Expected order:

```text
load achievement catalog
construct condition evaluator registry
construct fact source(s)
construct AchievementSystem
restore AchievementState
wire event subscriptions
perform safe retroactive evaluation
construct projection provider
bind AchievementsPanel
```

Save capture must obtain `AchievementState` from the system.

Avoid direct singleton access from panel code.

---

## 62. Interfaces

Potential interfaces:

```csharp
public interface IAchievementCatalog
{
    IReadOnlyList<AchievementDefinition> Definitions { get; }
}

public interface IAchievementFactSource
{
    AchievementFactSnapshot Capture();
}

public interface IAchievementCompletionExportSource
{
    AchievementCompletionExport CaptureCompletedIds();
}

public interface IAchievementProjectionSource
{
    IReadOnlyList<AchievementPanelEntry> GetPanelEntries();
}
```

Use only as many abstractions as the repository architecture warrants. The goal is clean authority, not interface
inflation.

---

## 63. Implementation Phase A — Audit and Literal Migration

### Tasks

1. Inventory all panel literals and milestone rules.
2. Record exact thresholds and state sources.
3. Create stable IDs for each.
4. Add localization keys.
5. Create initial catalog.
6. Write parity tests.
7. Do not change panel behavior yet.

### Exit criteria

Every existing visible panel achievement has a corresponding definition and parity fixture.

---

## 64. Implementation Phase B — Core Contract

### Tasks

1. Add definition DTO.
2. Add condition DTO.
3. Add catalog loader.
4. Add validator.
5. Add run-local state.
6. Add event DTO.
7. Add pure evaluator registry.
8. Add state capture/restore.
9. Add basic tests.

### Exit criteria

Core can evaluate definitions headlessly against supplied snapshots.

---

## 65. Implementation Phase C — Observation Wiring

### Tasks

1. Identify canonical event sources.
2. Register event-to-condition impact mapping.
3. Implement fact snapshot source.
4. Wire day events.
5. Wire survivor events.
6. Wire combat events.
7. Wire expedition events.
8. Wire social/faction events.
9. Wire economy events.
10. Wire moral events.
11. Add deterministic multi-unlock ordering.
12. Add once-only event tests.

### Exit criteria

Real campaign events can complete achievements without UI involvement.

---

## 66. Implementation Phase D — Persistence and Retroactivity

### Tasks

1. Add achievement state to campaign save.
2. Add schema version.
3. Restore before UI binding.
4. Implement no-block legacy migration.
5. Classify condition retroactivity.
6. Retroactively evaluate safe definitions.
7. Suppress migration toast spam.
8. Add old-save fixtures.
9. Add round-trip tests.

### Exit criteria

Old and new campaigns load with correct stable completion state.

---

## 67. Implementation Phase E — Panel Cutover

### Tasks

1. Add projection provider.
2. Bind panel to projection.
3. Remove all hardcoded labels/rules.
4. Add categories.
5. Add progress rendering.
6. Add hidden-state rendering if needed.
7. Add accessibility.
8. Add snapshot tests.
9. Search source for old literals.

### Exit criteria

The panel knows nothing about achievement business rules.

---

## 68. Implementation Phase F — Balanced Catalog

### Tasks

1. Review migrated catalog category balance.
2. Add only definitions backed by mature systems.
3. Add survival entries.
4. Add combat entries.
5. Add social entries.
6. Add exploration entries.
7. Add economic entries.
8. Add moral entries.
9. Add epilogue tags sparingly.
10. Add fixture coverage.

### Exit criteria

Catalog is broad enough to represent multiple play styles without implying meta rewards.

---

## 69. Implementation Phase G — Plan 175 Handoff

### Tasks

1. Implement immutable completion export.
2. Ensure sorted stable IDs.
3. Add no-side-effect test.
4. Integrate with campaign completion flow.
5. Document that Plan 175 owns cross-campaign idempotency.
6. Add an architectural test if possible that Plan 149 has no dependency on profile/meta namespaces.

### Exit criteria

Plan 175 can consume completed IDs without Plan 149 storing profile state.

---

## 70. Implementation Phase H — CI and Hardening

### Tasks

1. Add achievements selftest.
2. Extend data-integrity validator.
3. Add golden fixtures.
4. Add fuzz tests.
5. Add source/panel parity guard.
6. Generate coverage report.
7. Add catalog revision diagnostics.
8. Add performance sanity test.
9. Document architecture.
10. Run full regression suite.

### Exit criteria

Achievement architecture is proven in headless CI.

---

## 71. Detailed Acceptance Scenarios

### Scenario 1 — Threshold unlock

Given:
- campaign day 29;
- achievement requires day 30.

When day advances to 30:
- evaluator reports completed;
- state adds ID;
- deterministic event ID is emitted once;
- panel projection immediately shows completion;
- save/reload preserves completion.

### Scenario 2 — State falls below threshold later

Given:
- achievement completed when five survivors were alive;
- later two survivors die.

Expected:
- achievement remains completed;
- event is not revoked;
- progress may display completed state rather than 3/5.

### Scenario 3 — Duplicate domain event

Given:
- same combat resolution event is delivered twice.

Expected:
- aggregate fact source or event sequence dedup prevents double counting where necessary;
- achievement emits at most once.

### Scenario 4 — Old campaign migration

Given:
- day 70;
- no achievement save block;
- several reconstructable facts.

Expected:
- compatible achievements populate as completed;
- migration does not show dozens of historical popups;
- reload remains stable.

### Scenario 5 — Panel opened repeatedly

Expected:
- no gameplay evaluation side effects;
- no new unlock;
- projection is stable.

### Scenario 6 — Plan 175 export

Expected:
- contains campaign ID + sorted completed achievement IDs only;
- no reward fields;
- reading export twice is identical.

---

## 72. Data Integrity Edge Cases

Reject definitions with:

- empty IDs;
- whitespace IDs;
- duplicate IDs;
- unknown category;
- unknown condition kind;
- unsupported condition version;
- negative threshold where illegal;
- target ID missing when required;
- target ID present when forbidden;
- invalid localization key;
- invalid epilogue tag;
- zero conditions unless explicitly supported;
- forbidden reward/meta fields.

Warn or fail according to repository policy for:
- unreachable condition thresholds;
- duplicate title keys;
- redundant definitions with identical conditions.

---

## 73. Condition Evaluator Registry

Prefer explicit registration:

```csharp
registry.Register("campaign_day_at_least", version: 1, EvaluateCampaignDayAtLeast);
registry.Register("faction_standing_at_least", version: 1, EvaluateFactionStandingAtLeast);
```

The validator uses the same registry to confirm all catalog kinds are supported.

Do not discover evaluators via unrestricted reflection.

---

## 74. Event Sequencing and Counter Safety

For event-derived lifetime counters:

- consume canonical committed events only;
- avoid counting "attempted" actions unless achievement design says so;
- distinguish combat started vs combat resolved;
- distinguish trade offered vs committed;
- distinguish expedition dispatched vs completed;
- distinguish moral choice surfaced vs resolved.

If an event bus can replay events on restore, use event IDs or persisted aggregate authority to prevent duplicate
counter increments.

---

## 75. Progress Projection Rules

For threshold conditions:
- current = clamped actual;
- target = threshold;
- completed flag independent once unlocked.

For collection conditions:
- current = unique resolved IDs count;
- target = required unique count.

For boolean conditions:
- display binary state, not 0/1 unless UI convention prefers it.

For multi-condition:
- show either sub-condition progress or a clear overall fraction; do not expose confusing internal values.

---

## 76. Accessibility and UX

Achievement panel must support:

- keyboard navigation;
- controller navigation if global input supports it;
- text scaling;
- clear completed/in-progress state beyond color;
- progress readable by text, not only bars;
- reduced-motion unlock notification;
- hidden-achievement semantics that remain understandable;
- category headings with localization;
- no tooltip-only critical information.

Reuse existing design-system accessibility gates.

---

## 77. Failure Handling

### Catalog fails to load
Fail startup/data integrity according to project policy; do not silently run with hardcoded fallbacks.

### One malformed definition
Prefer catalog validation failure in development/CI. Production recovery policy should be explicit.

### Missing target system
If a condition kind's canonical source is unavailable, fail wiring rather than silently report zero forever.

### UI unavailable
Achievements still evaluate headlessly.

### Save contains unknown completed ID
Preserve the ID in state/export where safe; panel may omit it if no current definition exists.

---

## 78. Security and Data Hygiene

Achievement definitions are data, not executable scripts.

- no arbitrary code expressions;
- no reflection paths supplied from JSON;
- no filesystem access in conditions;
- no shell commands;
- no dynamic type activation from catalog strings;
- no personal/profile identifiers needed inside run-local definitions.

---

## 79. Cross-System Dependency Table

| Dependency | Plan 149 reads | Plan 149 writes | Ownership rule |
|---|---|---|---|
| Campaign clock | day facts | none | clock remains owner |
| Survivors | roster/fate facts | none | survivor system remains owner |
| Combat | resolved outcome facts | none | combat remains owner |
| Factions | standings/branch facts | none | faction system remains owner |
| Expeditions | completion/discovery facts | none | expedition remains owner |
| Economy | committed counters/state | none | economy remains owner |
| Moral choice | band/outcome facts | none | moral system remains owner |
| Save system | capture/restore slot | achievement state | campaign-local only |
| UI | projection consumer | none | no business rules |
| Plan 34 | completion boundary | completed-id snapshot/reference | Plan 34 owns record |
| Plan 175 | completed ID export | none | Plan 175 owns profile/meta |

---

## 80. Dependency Direction Guardrails

Allowed:

```text
Achievements -> stable domain fact interfaces
UI -> achievement projection
Plan175 -> achievement completion export
```

Forbidden:

```text
Achievements -> Plan175 reward service
Achievements -> profile database
Achievements -> UI widget text
Achievement evaluator -> engine scene tree
Panel -> survivor/combat/day systems for milestone logic
```

---

## 81. Achievement ID Naming Convention

Use stable snake_case or project-standard IDs, e.g.:

```text
survival_first_week
survival_thirty_days
combat_first_victory
exploration_ten_locations
social_refuge_of_five
economic_trade_network
moral_mercy_remembered
```

Avoid IDs containing mutable threshold text if thresholds may be tuned unless the threshold is part of the
achievement's enduring identity.

---

## 82. Deprecation Policy

If an achievement is removed:

- keep ID recognition in save migration if shipped;
- preserve completed ID in export if Plan 175/profile history may know it;
- remove from active panel only intentionally;
- document deprecation;
- never recycle ID for different semantics.

If renamed:
- change localization key/text, not stable achievement ID, whenever possible.

---

## 83. Source-Level Cleanup

After cutover, search for:

- old hardcoded achievement titles;
- old day threshold literals;
- old survivor-count milestone rules;
- duplicate progress calculations;
- dead panel helpers;
- UI dependencies on campaign systems used only for achievements.

Remove only once tests prove the Core projection replaces behavior.

---

## 84. CI Verification Commands

Baseline:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --achievements-selftest
```

If repository wrappers exist, integrate the new selftest into the appropriate fast/full CI tier.

---

## 85. Definition of Done — Flagship

### Contract
- [ ] `AchievementSystem.cs` exists.
- [ ] `AchievementDefinition` exists.
- [ ] `AchievementState` exists.
- [ ] catalog loader exists.
- [ ] condition evaluator registry exists.
- [ ] no reward/meta fields exist.

### Migration
- [ ] every current panel literal migrated to data.
- [ ] parity tests exist.
- [ ] old panel rules removed.
- [ ] old in-progress campaigns retroactively evaluate safely.

### Runtime
- [ ] explicit events/state snapshots drive evaluation.
- [ ] no RNG.
- [ ] no per-frame scan.
- [ ] unlock events emit exactly once per run.
- [ ] progress is deterministic.
- [ ] state round-trips.

### UI
- [ ] panel renders definitions + state only.
- [ ] no hardcoded labels.
- [ ] no render-time milestone logic.
- [ ] category/progress/localization supported.
- [ ] accessibility passes.

### Validation
- [ ] all IDs resolve.
- [ ] all condition kinds supported.
- [ ] forbidden fields rejected.
- [ ] achievements selftest exists.
- [ ] data-integrity selftest includes catalog.
- [ ] panel/catalog parity proven.
- [ ] fuzz/property tests pass.

### Handoff
- [ ] Plan 175 receives completed IDs only.
- [ ] export is read-only and stable.
- [ ] Plan 149 has no profile/meta dependency.
- [ ] Plan 34 remains completion-record owner.

---

## 86. Immediate Follow-On Task 149-A — Achievement Condition Reachability Auditor

### Goal

Prove that every active achievement condition is realistically reachable under current game rules.

### Substeps

1. Read definitions.
2. Compare thresholds against configured campaign limits.
3. Compare target IDs against catalogs.
4. Check faction standing ranges.
5. Check item/resource caps.
6. Check maximum survivor counts.
7. Check campaign duration bounds.
8. Check event kinds can actually fire.
9. Flag impossible combinations.
10. Generate `ACHIEVEMENT_REACHABILITY.md`.
11. Gate newly unreachable definitions.

---

## 87. Follow-On Task 149-B — Achievement Event Attribution Audit

### Goal

Ensure every achievement completion can be explained by a specific fact/event set.

### Substeps

1. Store debug `supportingFactIds` for evaluation.
2. Add developer inspector.
3. Show definition ID.
4. Show condition evaluations.
5. Show triggering event.
6. Show prior/current progress.
7. Verify unlocks do not appear mysterious.
8. Use inspector in automated fixtures.

---

## 88. Follow-On Task 149-C — Achievement Catalog Authoring Workflow

### Goal

Make new achievements safe to author without touching code when existing condition kinds suffice.

### Substeps

1. Document schema.
2. Add JSON schema if project uses one.
3. Add localization validation.
4. Add preview generator.
5. Add condition reachability check.
6. Add duplicate-semantic warning.
7. Add category balance report.
8. Add fixture template generator.
9. Keep new condition kinds as code-reviewed additions.

---

## 89. Follow-On Task 149-D — Campaign Completion Export Contract Test

### Goal

Protect the Plan 149 → Plan 175 boundary permanently.

### Substeps

1. Add architectural test asserting export DTO fields.
2. Fail if reward/profile fields are added.
3. Assert sorted IDs.
4. Assert no mutation.
5. Assert repeatable capture.
6. Assert campaign ID matches current run.
7. Add compatibility test with Plan 34 completion boundary.

---

## 90. Follow-On Task 149-E — Achievement Balance and Playstyle Coverage Review

### Goal

Prevent the catalog from becoming survival/combat-heavy while social, exploration, economic, or moral play is
underrepresented.

### Substeps

1. Generate category counts.
2. Generate completion-rate data from synthetic fixtures.
3. Identify trivially automatic achievements.
4. Identify near-impossible achievements.
5. Identify duplicate accomplishments.
6. Review whether any achievement incentivizes degenerate play.
7. Tune definitions with migration notes.
8. Keep reward discussion outside Plan 149.

---

## 91. Longer-Term Opportunities Outside This Plan

Potential later integrations, each requiring separate ownership:

- platform achievement adapter;
- profile-wide gallery via Plan 175;
- permanent reward unlocks via Plan 175;
- achievement-based New Game+ modifiers via Plan 175;
- ending-gallery references via Plan 145;
- statistics page using the same facts but not achievement completion state;
- optional telemetry completion-rate analysis.

None should be smuggled into Plan 149's initial implementation.

---

## 92. Final Guardrails

- No achievement logic in UI.
- No UI text used as a condition.
- No `System.Random`.
- No per-frame catalog scan.
- No unversioned reflection condition engine.
- No second campaign profile.
- No cross-campaign storage.
- No currency.
- No prestige.
- No rewards.
- No New Game+ fields.
- No duplicated reward idempotency.
- No relocking a completed run-local achievement.
- No duplicate unlock events after load.
- No silent threshold change without migration policy.
- No unsupported target IDs.
- No achievement label outside localization/catalog authority.
- No panel entry without a catalog definition.
- No catalog definition with missing evaluator.
- No Plan 175 dependency flowing backward into Core achievement evaluation.

When complete, achievements become a clean observer of the campaign rather than a hidden UI rule set. The player
receives understandable progress and durable run-local recognition; the engineering layer gains deterministic
events, tests, migration, and data authority; and future meta-progression receives exactly the narrow input it
needs without contaminating campaign ownership.
