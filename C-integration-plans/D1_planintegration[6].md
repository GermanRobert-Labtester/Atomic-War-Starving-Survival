# D1 Flagship Integration Plan [6]
## Plan 153 — Faction Espionage & Sabotage

> **Purpose:** Add a deterministic, save-compatible covert-operations layer to ASHFALL's faction game without
> replacing the existing diplomacy, trade, expedition, moral-choice, survivor, or combat authorities.
>
> **Primary source:** Plan 153 — Faction Espionage & Sabotage.
>
> **Core design problem:** the faction stack currently supports overt relations—standing, trust, trade, branch
> choice, and conflict—but no covert action authority exists. Plan 153 introduces the missing "shadow war"
> layer: infiltration, intelligence theft, sabotage, propaganda, and other covert outcomes that alter what the
> player knows and what factions can do, while preserving deterministic simulation and explicit consequences.
>
> **Implementation posture:** data-driven, deterministic, bounded, transparent to tests, opaque only to the
> player where design requires uncertainty, and integrated through explicit contracts rather than direct
> mutation of unrelated systems.
>
> **Critical guardrail:** espionage must not become a universal bypass around faction progression. Operations
> consume time, expose scarce survivors, increase suspicion, can produce false or incomplete intelligence, and
> can worsen diplomatic/moral state. Covert play is an alternative strategic layer, not a cheat code.

---

## 1. Source Problem Statement

The source plan establishes the missing layer clearly:

- `FactionBranchCoordinator.cs` coordinates faction branches and standing;
- `FactionStanceEngine.cs` tracks faction trust/trade stance;
- `HoldfastTradeSession.cs` or equivalent controls faction-gated trade;
- the expedition layer provides travel/field execution;
- the moral-choice layer records ethical consequences.

But none of these systems owns:
- covert operations;
- infiltration;
- clandestine intelligence;
- faction suspicion;
- compromised agents;
- sabotage;
- disinformation;
- covert-operation discovery.

That gap creates a binary faction model: the player can cooperate, trade, threaten, or fight, but cannot learn,
influence, undermine, or manipulate from the shadows.

The flagship integration therefore adds an explicit covert-action domain:

```text
Faction state + survivor capabilities + operation catalog
                ↓
        EspionageSystem
                ↓
   operation scheduling / ticking
                ↓
 deterministic resolution via seeded RNG
                ↓
 intelligence / sabotage / compromise / suspicion
                ↓
 explicit downstream adapters
   ├─ faction standing/trust
   ├─ expedition options
   ├─ moral consequences
   ├─ survivor fate/relations
   ├─ combat when discovered
   └─ UI/journal/quest hooks
```

No downstream system should need to understand espionage internals.

---

## 2. Flagship Success Criteria

The system is complete only when all of the following are true:

1. `EspionageSystem.cs` is a Core-domain authority with schema-versioned capture/restore.
2. Covert operations are defined in data rather than hardcoded UI.
3. Operation resolution is deterministic under fixed seeds.
4. Faction suspicion is persisted and faction-scoped.
5. A survivor cannot be simultaneously assigned to incompatible duties unless an existing scheduling authority
   explicitly allows it.
6. Intelligence reports have source, type, acquisition day, confidence/accuracy semantics, and decode state.
7. Intelligence does not directly reveal hidden truth unless the report has sufficient authority/accuracy.
8. Successful sabotage changes real downstream capabilities through explicit adapters.
9. Failed or exposed operations can produce real diplomatic and survivor consequences.
10. The UI only projects Core operation/intelligence state.
11. Old saves initialize safely with empty espionage state.
12. All operation/faction/survivor/intelligence target IDs validate.
13. Exploit prevention blocks operation spam.
14. Suspicion makes repeated targeting progressively more dangerous.
15. Headless simulation produces the same result as UI-driven play.
16. `--espionage-selftest` validates deterministic outcomes, catalogs, save round-trip, and cross-system effects.
17. The first implementation does not silently create a second faction-standing authority.
18. Morality changes are delegated to the moral system rather than stored as a second moral score.
19. Agent death/capture is delegated to the canonical survivor/fate systems.
20. Covert actions remain strategically meaningful without making overt diplomacy obsolete.

---

## 3. Repository Reconnaissance Before Editing

Create `docs/espionage/ESPIONAGE_INTEGRATION_AUDIT.md` before implementation.

Inspect at minimum:

- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs`
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`
- `Assets/StreamingAssets/Data/faction_lore.json`
- survivor skill/trait systems
- survivor duty/assignment systems
- survivor capture/death/retirement systems
- survivor relationship systems
- tactical combat handoff
- quest/event systems
- save-state schema
- campaign clock/day tick
- seeded RNG authority
- localization/catalog loader conventions
- panel/UI architecture
- journal notification/logging
- faction leader/key-NPC data, if any
- faction production, trade, security, and territory systems, if present

For each candidate integration point, record:

| Question | Required answer |
|---|---|
| Canonical owner? | exact system/type |
| Stable ID available? | yes/no + format |
| Persisted? | save key |
| Event emitted? | event type |
| Can espionage write directly? | preferably no; adapter/sink |
| Deterministic? | yes/no |
| Can old saves lack state? | migration behavior |
| Catalog validated? | validator/selftest |
| UI currently reads directly? | coupling risk |

Do not assume the source plan's example file names still represent the current repository exactly. Use actual
repository authority names during implementation.

---

## 4. Scope Boundaries

### In scope

- operation catalog;
- active operation lifecycle;
- assigned agent;
- operation duration;
- deterministic success/exposure resolution;
- faction suspicion;
- intelligence report acquisition;
- decoding pipeline;
- report accuracy/confidence;
- sabotage effect intents;
- overt standing/trust consequences after discovery;
- moral consequence requests;
- survivor compromise/capture/death handoffs;
- expedition-linked covert operations;
- UI projection;
- journal/events;
- save migration;
- CI and integrity validation.

### Explicitly out of scope for first cut

- autonomous enemy spy simulation against the player;
- full counter-intelligence system;
- complex double-agent trees;
- a separate covert economy/currency;
- a new standalone stealth-combat minigame;
- fully dynamic faction leader succession unless already supported;
- procedural political simulation not present elsewhere;
- live generative narrative;
- profile/meta progression;
- permanent cross-campaign espionage perks.

Those can follow after the base authority is stable.

---

## 5. Operation Taxonomy

The source proposes five operation types:

1. **Infiltrate**
2. **Steal**
3. **Sabotage**
4. **Assassinate**
5. **Propaganda**

Treat these as stable operation categories, not necessarily one-to-one runtime classes.

Recommended enum/string IDs:

```text
infiltrate
steal_intelligence
sabotage
assassinate
propaganda
```

Data templates then specialize them.

Example:
- `steal_trade_routes`
- `steal_radio_codes`
- `sabotage_supply_depot`
- `sabotage_power_exchange`
- `propaganda_discredit_leadership`

This avoids hardcoding every mission in C#.

---

## 6. Canonical Domain Types

Recommended Core DTOs:

```csharp
public sealed record EspionageOperationDefinition
{
    public string OperationId { get; init; }
    public string OperationType { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }
    public int DurationDays { get; init; }
    public EspionageRiskLevel RiskLevel { get; init; }
    public int BaseDifficulty { get; init; }
    public IReadOnlyList<string> RequiredTags { get; init; }
    public IReadOnlyList<string> AllowedTargetKinds { get; init; }
    public string OutcomeProfileId { get; init; }
}
```

```csharp
public sealed record ActiveEspionageOperation
{
    public string InstanceId { get; init; }
    public string DefinitionId { get; init; }
    public string TargetFactionId { get; init; }
    public string AgentSurvivorId { get; init; }
    public int StartDay { get; init; }
    public int ResolveDay { get; init; }
    public EspionageOperationStatus Status { get; init; }
    public int Seed { get; init; }
    public string? TargetId { get; init; }
}
```

```csharp
public sealed record IntelligenceReport
{
    public string ReportId { get; init; }
    public string SourceFactionId { get; init; }
    public string IntelligenceType { get; init; }
    public int Value { get; init; }
    public int Accuracy { get; init; }
    public int DayObtained { get; init; }
    public IntelligenceDecodeState DecodeState { get; init; }
    public string? SubjectId { get; init; }
    public IReadOnlyList<string> FactKeys { get; init; }
}
```

```csharp
public sealed record EspionageState
{
    public int SchemaVersion { get; init; }
    public IReadOnlyList<ActiveEspionageOperation> ActiveOperations { get; init; }
    public IReadOnlyList<IntelligenceReport> Reports { get; init; }
    public IReadOnlySet<string> CompromisedAgentIds { get; init; }
    public IReadOnlyDictionary<string, int> FactionSuspicion { get; init; }
    public IReadOnlyDictionary<string, int> OperationCooldownUntilDay { get; init; }
}
```

Use actual repository conventions and mutable/immutable patterns.

---

## 7. State Ownership Rules

`EspionageSystem` owns:

- operation instances;
- operation state machine;
- suspicion values;
- intelligence reports;
- decode state;
- cooldown state;
- compromise markers specific to espionage;
- deterministic resolution metadata.

It does **not** own:

- faction standing;
- faction trust;
- faction hostility;
- survivor health;
- survivor alive/dead status;
- moral score/band;
- expedition route state;
- combat outcome;
- faction leader existence;
- faction production authority.

Those changes are requested through explicit sinks/adapters.

---

## 8. Operation Lifecycle State Machine

Use an explicit lifecycle:

```text
planned
  ↓ start validated
active
  ├─ cancelled
  ├─ succeeded
  ├─ failed
  └─ compromised
```

Optional later states:
- captured;
- extraction_pending;
- decoding_pending.

Do not overload one `status` field with unrelated survivor-fate meanings.

### Start validation

Before an operation becomes active:
- definition exists;
- target faction exists;
- target is allowed;
- agent exists and is eligible;
- agent is not unavailable;
- cooldown passed;
- faction not prohibited by scenario;
- required intel/resources exist if applicable;
- operation slot limit allows it;
- duration resolves deterministically;
- seed assigned.

Start should be transactional: either all reservation/assignment effects commit, or none do.

---

## 9. Deterministic RNG Contract

Use the repository's canonical `ISeededRng`.

Never:
- use `System.Random`;
- seed from wall-clock time;
- seed from UI frame count;
- seed from unordered collection hashes.

Recommended seed derivation:

```text
campaignSeed
+ operationInstanceId
+ operationDefinitionId
+ targetFactionId
+ resolveDay
```

Hash through the project's stable seed utility.

The same operation in the same persisted state must resolve identically after save/load.

---

## 10. Success, Exposure, and Consequence Rolls

Do not collapse everything into one "success chance."

Model at least:

1. **mission success**
2. **discovery/exposure**
3. **agent consequence if exposed**
4. **intelligence quality**, where applicable

This permits outcomes like:
- success + undetected;
- success + detected;
- failure + escape;
- failure + compromised;
- failure + captured.

Example deterministic resolution flow:

```text
calculate successScore
roll success
calculate exposureScore
roll exposure
if exposed:
    calculate agentConsequence
if intelligence awarded:
    roll/derive quality band
```

All rolls consume the operation's deterministic RNG stream in a fixed documented order.

---

## 11. Success Formula

Avoid opaque percentages stored as final truth in data.

Prefer:

```text
successScore =
    base operation competence
  + agent relevant skill
  + supporting intelligence bonus
  + preparation bonus
  - operation difficulty
  - faction security
  - suspicion penalty
  - injury/fatigue penalty
```

Clamp to configured bounds.

The UI may display a rounded player-facing estimate. The Core stores the actual resolved parameters.

### Important

If existing survivor skills do not include stealth/intelligence/disguise, do not invent a parallel skill system
inside espionage. Map to existing traits/skills first or create a separate follow-on plan.

---

## 12. Risk Levels

Data-driven risk levels:

```text
low
medium
high
extreme
```

Risk affects:
- exposure chance;
- capture/death severity band;
- suspicion gain;
- moral weight where relevant;
- cooldown;
- expected value.

Do not encode fixed effects directly in UI copy.

---

## 13. Faction Security

Espionage difficulty requires a faction security concept.

Before inventing a new stored security score, audit whether faction systems already expose:
- hostility;
- military strength;
- territory control;
- alertness;
- security doctrine;
- population;
- trust;
- recent attack state.

Recommended approach:
derive a normalized `FactionSecuritySnapshot` from canonical sources.

Example:
```text
baseSecurity
+ suspicion security modifier
+ wartime modifier
+ leadership modifier
+ recent espionage modifier
```

If no upstream security source exists, store only the minimum new data needed and document ownership.

---

## 14. Suspicion Mechanics

Suspicion is faction-specific and run-local.

Range:
```text
0..100
```

Suggested bands:
- 0–24: normal
- 25–49: watchful
- 50–74: alert
- 75–89: hostile countermeasures
- 90–100: crisis / near-certain exposure

Exact thresholds belong in data.

Suspicion should influence:
- exposure chance;
- operation availability;
- cooldown;
- faction security;
- diplomatic consequences on discovery.

Suspicion itself should not automatically replace faction trust/standing. It is a covert-awareness state.

---

## 15. Suspicion Gain

Suspicion gain should derive from:
- operation type;
- operation risk;
- target sensitivity;
- prior operations;
- success/failure;
- exposure;
- cover-up outcome.

Undetected success may still generate a small suspicion increase if design wants factions to notice patterns.
Compromised operations should create major increases.

All changes must be deterministic and logged.

---

## 16. Suspicion Decay

The source proposes decay over time.

Implement through day ticks, not per-frame timers.

Example data:
```json
{
  "suspicionDecay": {
    "basePerDay": 1,
    "minimumAfterConfirmedEspionage": 20,
    "wartimeMultiplier": 0.5
  }
}
```

Confirmed exposure may establish a temporary or permanent floor.

Avoid a system where the player can spam operations, wait a few days, and fully erase consequences.

---

## 17. Operation Cooldowns and Anti-Spam

Add explicit anti-spam constraints:

- global concurrent operation cap;
- per-faction active operation cap;
- per-operation-definition cooldown;
- per-agent recovery/cooldown;
- suspicion soft cap;
- suspicion hard lock for certain operations;
- resource/preparation requirement where existing systems support it.

Keep these values data-driven.

---

## 18. Agent Eligibility

Use actual survivor data.

Eligibility can consider:
- alive/available;
- not already dispatched;
- not hospitalized/critically injured;
- required skill tags;
- relationship/faction history if canonical;
- compromise status.

Do not create a parallel `agent roster`; assigned agents are ordinary survivors temporarily used by espionage.

---

## 19. Agent Assignment Authority

Audit existing survivor duty/expedition scheduling.

The espionage system should call the canonical reservation/assignment API.

Required behavior:
- assigning an agent makes them unavailable for incompatible tasks;
- save/load preserves assignment;
- operation cancellation releases assignment;
- resolution releases or transitions assignment;
- captured agents remain unavailable through survivor/fate authority.

No survivor should duplicate into both an expedition and a covert operation unless explicitly supported.

---

## 20. Agent Experience

The source proposes agent experience/reputation.

Do not introduce a new experience tree in the first pass unless a survivor skill progression authority already
exists.

Preferred:
- emit `EspionageOperationResolved` with contribution facts;
- survivor skill/experience system decides whether/how skill increases.

Espionage may keep operation statistics for UI, but not a second character progression system.

---

## 21. Agent Compromise

Compromise is not identical to death or capture.

Possible espionage-local marker:
- cover identity exposed;
- faction recognizes this survivor;
- future operations against that faction penalized or blocked.

Canonical survivor fate remains external.

Model:
```text
CompromisedAgent(agentId, factionId, severity)
```

Persist per-faction compromise if the game benefits from it; avoid a global compromise flag if discovery should
be faction-specific.

---

## 22. Agent Capture

If capture mechanics already exist, hand off.

If not:
- do not build a giant prisoner system inside EspionageSystem;
- create a minimal `AgentCaptured` outcome contract;
- follow with a dedicated prisoner/recovery plan.

The source's "ransom or leave" behavior should only ship when there is an actual consequence/quest path.

---

## 23. Agent Death

Espionage requests death through canonical survivor fate authority.

Then downstream systems can handle:
- morale;
- relationships;
- memorials;
- epilogue;
- succession.

Never mutate the survivor roster directly from espionage if the repository has a death pipeline.

---

## 24. Intelligence Report Taxonomy

Source types:
- military;
- economic;
- political;
- technical.

Expand only if repository systems provide real consumers.

Each report should have:
- source faction;
- subject;
- value;
- accuracy/confidence;
- obtained day;
- decode state;
- expiration/staleness policy if needed;
- fact keys or typed payload reference.

Avoid storing arbitrary prose as the only intelligence authority.

---

## 25. Raw vs Decoded Intelligence

States:

```text
raw
decoding
decoded
discredited
expired
```

Raw intel may show:
- source;
- category;
- rough value;
- decode requirement.

Decoded intel reveals structured facts and narrative text.

Decoding can require:
- survivor assignment;
- time;
- skill;
- equipment/research if already wired.

Do not make decoding another full minigame in v1.

---

## 26. Intelligence Accuracy Semantics

The source says reports can be false.

Avoid displaying an exact "accuracy 73%" unless the player has a mechanic that justifies knowing that number.

Core may store an accuracy/confidence value, while UI shows bands:
- unverified;
- questionable;
- credible;
- highly credible;
- confirmed.

Truth generation must be deterministic.

False intelligence must not corrupt canonical world state. It changes what the player believes/what options are
presented, not what actually exists.

---

## 27. Intelligence Truth Model

Separate:

```text
WorldTruthFact
IntelligenceClaim
Confidence
```

An intelligence report references claims.

Example:
```text
Claim: faction_X_trade_route = route_A
Truth: route_B
Accuracy: low
```

Consumers must decide whether they are reading the report or canonical truth.

This is essential to avoid accidentally revealing truth simply because the report exists in Core.

---

## 28. Intelligence Staleness

Some intel should age.

Possible per-type staleness:
- military plans: short;
- trade route: medium;
- technical blueprint: permanent after decoded;
- political disposition: medium.

Add `validUntilDay` only when useful.

Do not delete expired reports automatically if journal/history needs them; mark stale.

---

## 29. Infiltration Operations

Goal:
place an agent in a persistent covert position.

Source suggests 30–90 days. Make duration template-driven.

Mechanics:
- initial infiltration roll;
- persistent exposure checks at defined checkpoints, not daily if too noisy;
- periodic report generation;
- extraction/cancel action;
- increased suspicion over time;
- compromise risk.

Use deterministic checkpoint seeds derived from operation instance + checkpoint index.

---

## 30. Theft Operations

Target typed intelligence or assets.

Examples:
- technical blueprint;
- trade route;
- military plan;
- code/cipher;
- political correspondence.

Outcomes:
- report acquired;
- optional item/research unlock handoff if another plan owns it;
- detection;
- compromise.

Do not directly unlock crafting/research unless the canonical research unlock bridge owns that consequence.
Espionage should emit a stolen-knowledge fact to that system.

---

## 31. Sabotage Operations

Sabotage must affect real systems.

Create typed sabotage intents:

```text
trade_efficiency_penalty
production_penalty
security_penalty
supply_loss
route_disruption
communications_disruption
```

Each downstream system may accept or reject supported effects.

Effects need:
- target;
- magnitude;
- duration;
- source operation ID;
- stacking policy.

Do not store opaque "faction effectiveness -20%" if no system consumes it.

---

## 32. Sabotage Effect Registry

Define a registry of supported effect kinds.

Validation rule:
every sabotage template's effect kind must have a registered sink.

Example:
```text
sabotage_trade_route
 -> IFactionTradeDisruptionSink
```

If no sink exists, the operation template must not ship merely because prose describes sabotage.

---

## 33. Assassination Operations

The source includes assassination as extreme risk.

Implementation must first audit whether:
- factions have stable key NPC/leader IDs;
- leader death is supported;
- leadership succession exists;
- quests reference those leaders safely.

If not, assassination should be deferred or limited to targets with fully supported fate handling.

Never fake "leader eliminated" by writing only an espionage log while the faction continues unchanged.

---

## 34. Assassination Consequence Contract

If supported:

```text
Espionage assassination success
 -> Character/FactionLeader fate authority
 -> Faction leadership transition
 -> faction standing/trust reaction
 -> moral consequence
 -> quest/event hooks
 -> possible combat/security escalation
```

Espionage does not own faction succession.

---

## 35. Propaganda Operations

Propaganda should manipulate social perception through explicit faction systems.

Possible effects:
- target faction's standing with another faction;
- temporary trust modifier;
- public opinion/rumor facts if those systems exist;
- diplomatic event triggers.

If faction-to-faction standing is not modeled, do not invent hidden values inside espionage. Use player-facing
standing/trust effects or defer broader inter-faction manipulation.

---

## 36. False-Flag Operations

The source includes a quest hook for framing another faction.

Treat false flag as follow-on unless:
- attribution of attacks/sabotage exists;
- factions can react to believed perpetrators separately from truth.

A safe v1 can record a `suspectedPerpetratorFactionId` on a covert incident only if a consumer exists.

---

## 37. Cover-Up Operations

"Cover-Up" can be a response action after exposure.

Possible effects:
- reduce suspicion increase;
- convert compromised -> failed-undetected;
- protect agent identity;
- consume resources/intelligence.

Avoid allowing full consequence erasure. Confirmed major sabotage should leave some residue.

---

## 38. Double-Agent Hook

Do not fully implement autonomous double agents in the first pass.

Prepare contracts:
- `AgentCompromised`
- `AgentTurnRisk`
- `IntelligenceReportDisinformationSource`

Future counter-espionage can consume them.

The initial system should support false intelligence without requiring a full betrayal simulation.

---

## 39. Faction Standing Integration

Espionage should request standing changes only when consequences become overt or otherwise socially meaningful.

Examples:
- confirmed espionage exposure;
- captured agent identified;
- assassination attributed;
- ally discovered being spied on.

Undetected operations should generally not reduce overt standing unless design intentionally models unexplained
suspicion through stance.

---

## 40. Faction Trust / Stance Integration

`FactionStanceEngine` can consume:
- suspicion band;
- confirmed espionage incidents;
- temporary distrust modifiers.

Do not write its internal fields directly.

Preferred adapter:
```csharp
IFactionEspionageConsequenceSink.Apply(EspionageFactionConsequence consequence)
```

---

## 41. Moral Choice Integration

Espionage can produce moral facts for:
- spying on allies;
- assassination;
- blackmail;
- disinformation;
- sacrificing captured agents;
- framing another faction.

The moral system remains authority.

Do not hardcode moral-band deltas in espionage if moral-choice data already owns weighting.
Emit a typed moral event/choice context.

---

## 42. Survivor Relationship Integration

Possible effects:
- partner/friend objects to risky mission;
- morale after capture/death;
- trust change after abandonment;
- admiration after successful extraction.

Only wire when the survivor relation system exposes a supported event contract.

Do not create relationship scores inside espionage.

---

## 43. Expedition Integration

Covert operations may use the expedition layer in two ways:

1. covert action is an expedition subtype;
2. expedition unlocks/executes a covert opportunity.

Audit which architecture fits existing code.

Do not duplicate travel time, hazard, route, inventory, or injury simulation if expedition already owns them.

---

## 44. Tactical Combat Integration

If an operation is discovered and combat occurs:

```text
EspionageSystem emits covert encounter request
 -> TacticalCombatSystem resolves combat
 -> survivor/faction consequences return
 -> EspionageSystem records operation outcome
```

Espionage must not implement a second combat simulator.

---

## 45. Quest Hooks

The source proposes:

- The Spy Game
- The Mole Hunt
- The Defector
- The Intelligence War
- The False Flag

Treat these as quest integration hooks, not hard requirements to author all quests before the system works.

Expose events:
- operation started;
- infiltration established;
- intelligence acquired;
- operation exposed;
- agent captured;
- high suspicion reached;
- report decoded.

Quest runtime can subscribe.

---

## 46. Journal Integration

The journal may record:

- operation launched;
- major report acquired;
- agent compromised;
- agent lost;
- major sabotage outcome;
- decoded strategic intelligence.

Do not spam one entry per daily infiltration tick.

Entries should reference stable operation/report IDs.

---

## 47. Tutorial / Guidance

First-use guidance should explain:

- operations take time;
- shown success/risk is an estimate;
- suspicion persists;
- agents can be lost;
- intelligence may be unreliable;
- covert actions can damage diplomacy/morality.

Avoid bespoke tutorial popups if the project has a canonical guidance overlay.

---

## 48. UI Projection

Create a presentation DTO.

Example:

```csharp
public sealed record EspionagePanelModel
{
    public IReadOnlyList<EspionageOperationCard> ActiveOperations { get; init; }
    public IReadOnlyList<IntelligenceReportCard> Reports { get; init; }
    public IReadOnlyList<FactionSuspicionCard> Suspicion { get; init; }
    public IReadOnlyList<AvailableOperationCard> AvailableOperations { get; init; }
}
```

The UI may format:
- duration;
- risk label;
- estimated success band;
- suspicion band;
- decode progress.

The UI may not roll outcomes or recalculate operation logic independently.

---

## 49. Player-Facing Risk Communication

Avoid false precision if the game does not justify exact probabilities.

Possible display:
- favorable;
- uncertain;
- dangerous;
- desperate.

Or show exact percentages only if the design wants transparent simulation.

If false intelligence/security uncertainty exists, displayed estimate can be derived from known information while
Core actual roll uses real security.

---

## 50. Operation Template Data Authority

Create:
`Assets/StreamingAssets/Data/espionage_operations.json`

Target 15 templates after system support is proven.

Illustrative groups:

### Infiltration
- basic observer
- trade network mole
- military staff infiltration

### Theft
- trade routes
- technical blueprint
- radio codes
- military plans

### Sabotage
- supply depot
- trade route
- communications
- production line

### Propaganda
- discredit leadership
- sow inter-faction distrust

### Extreme
- supported assassination target
- high-risk extraction/cover-up

Do not pad to 15 with templates whose effects have no downstream consumers.

---

## 51. Operation Definition Validation

Validate:
- unique IDs;
- valid operation type;
- valid risk level;
- positive duration;
- legal difficulty;
- known required skill/tag IDs;
- supported target kinds;
- supported outcome profile;
- supported sabotage effect kinds;
- localization keys;
- no missing faction constraints;
- no unsupported assassination target rules.

---

## 52. Intelligence Catalog / Payload Validation

If intelligence fact types are cataloged, validate:
- type ID;
- subject type;
- target catalog;
- decode profile;
- staleness profile;
- allowed accuracy range;
- consumer exists for strategic effect if one is promised.

No report should claim a gameplay benefit with no consumer.

---

## 53. Save Schema

Persist:
- active operations;
- operation seeds;
- start/resolve days;
- status;
- reports;
- decode state;
- suspicion;
- compromised-agent espionage markers;
- cooldowns;
- any queued deterministic consequence IDs.

Do not persist derived UI estimates.

---

## 54. Old-Save Migration

Source requires empty state.

Migration behavior:
- missing espionage block -> schema v1 empty state;
- suspicion zero for all factions;
- no active operations;
- no reports;
- no compromised agents;
- cooldowns empty.

Do not retroactively invent operations from old diplomacy state.

Add migration fixture.

---

## 55. Save/Load Mid-Operation

Required test:

1. start operation;
2. advance partially;
3. save;
4. reload;
5. continue;
6. resolve.

Expected:
- same resolve day;
- same operation seed;
- same final outcome;
- same suspicion changes;
- no duplicate events.

---

## 56. One-Time Resolution Semantics

Each operation instance resolves once.

Persist a stable instance ID.

If the day tick repeats after load:
- already resolved operation must not resolve again;
- consequences must be idempotent.

Use deterministic consequence/event IDs:
```text
espionage:<campaign>:<operationInstance>:resolved
```

---

## 57. Consequence Transaction

Operation resolution should stage effects:

```text
compute result
validate downstream consequence targets
commit espionage-local state
dispatch typed consequence commands/events
record idempotency
```

If downstream systems cannot participate transactionally, make all consequence messages idempotent by stable
operation outcome ID.

---

## 58. Intelligence Decoding State Machine

```text
raw
  ↓ assign decoder
decoding
  ├─ cancelled
  └─ decoded
```

If decoding can fail:
- define deterministic failure;
- avoid endless rerolls.

Decoder assignment should use canonical survivor availability rules.

---

## 59. Decode Progress

Progress should be day/task based, not UI timer based.

Persist:
- start day;
- completion day;
- assigned decoder;
- report ID.

UI derives progress.

---

## 60. Exploit Prevention

Guard against:

- cancelling/restarting to reroll seed;
- save-scumming that changes operation seed;
- assigning same survivor to multiple operations;
- spam against one faction while suspicion is ignored;
- repeated report decoding for duplicate benefit;
- repeated sabotage stacking beyond intended cap;
- assassination rerolls;
- duplicate operation instance IDs;
- reopening UI to trigger resolution.

Assign operation seed when operation starts and persist it.

---

## 61. Balance Model

Espionage power should be bounded by four costs:

1. **time**
2. **agent opportunity cost**
3. **suspicion**
4. **consequence severity**

Optionally:
5. resources/preparation;
6. prerequisite intelligence.

Do not rely only on low success percentages, which often encourage save-scumming.

---

## 62. Player Strategy Loop

Healthy loop:

```text
gather low-risk intel
 -> identify vulnerability
 -> decide whether benefit justifies suspicion
 -> assign scarce survivor
 -> wait / manage exposure
 -> receive uncertain information or effect
 -> react to consequences
 -> cooldown / suspicion recovery
```

This creates strategy rather than "click sabotage every faction."

---

## 63. Suspicion and Diplomacy Tension

Suspicion should create interesting intermediate states.

Example:
- faction remains publicly friendly;
- trade still possible;
- prices/availability may worsen if stance engine supports it;
- faction security rises;
- specific covert operations lock;
- exposed ally espionage can cause major trust collapse.

This is richer than instant hostility at arbitrary thresholds.

---

## 64. Intelligence Strategic Consumers

Possible consumers only if they exist:

- expedition: reveal safer route;
- trade: reveal high-value demand;
- research: expose blueprint target;
- combat: reveal enemy strength;
- faction: reveal impending hostility;
- world events: reveal planned raid;
- radio: decode secure channel.

Each benefit should be a typed integration, not generic "intelligence value."

---

## 65. Research Integration

If stolen technical intelligence should unlock research:

```text
Espionage report decoded
 -> ResearchUnlockBridge / research intake
```

Plan 153 does not directly mutate recipe/research unlocks.

This keeps Plan 141's research authority intact.

---

## 66. Achievement Integration

If Plan 149 is implemented, espionage may emit facts such as:
- first successful covert operation;
- decode high-value report;
- extract compromised agent.

Plan 153 does not own achievement state or profile rewards.

---

## 67. Epilogue Integration

If Plan 145 is implemented, expose stable endgame facts:

- major intelligence coup;
- famous sabotage;
- exposed ally betrayal;
- lost agent;
- assassination outcome.

Do not write epilogue prose inside espionage.

---

## 68. Telemetry / Playtest Instrumentation

If opt-in telemetry exists, useful metrics:

- operation starts by type;
- completion rate;
- exposure rate;
- suspicion at start;
- agent loss rate;
- report decode rate;
- false-intel encounter rate;
- cancellation rate.

Use to tune risk and identify operations nobody uses.

Do not record private free-form notes.

---

## 69. Structured Diagnostics

Logs:

```text
EspionageOperationStarted instance=<id> type=<id> faction=<id> resolveDay=<n>
EspionageOperationResolved instance=<id> result=<status> exposed=<bool>
FactionSuspicionChanged faction=<id> from=<n> to=<n> reason=<id>
IntelligenceReportCreated report=<id> type=<id>
IntelligenceDecoded report=<id>
AgentCompromised survivor=<id> faction=<id>
```

Do not log hidden truth to normal player logs if it would spoil false-intelligence mechanics.

---

## 70. Data Integrity Self-Test

Extend `--data-integrity-selftest` to validate:

- every operation definition;
- every faction constraint;
- every target ID;
- every operation outcome profile;
- every intelligence type;
- every sabotage effect sink;
- every localization key;
- every risk profile;
- every suspicion band;
- every cooldown profile;
- every referenced survivor tag/skill;
- every assassination target rule if enabled.

---

## 71. Dedicated `--espionage-selftest`

Implement a headless verb that:

1. loads catalogs;
2. creates deterministic faction/agent fixtures;
3. starts each supported operation type;
4. saves/reloads mid-operation;
5. resolves with fixed seeds;
6. verifies expected deterministic result;
7. verifies suspicion;
8. verifies report generation;
9. decodes a report;
10. verifies once-only resolution;
11. verifies invalid target rejection;
12. verifies max-suspicion restrictions;
13. verifies old-save empty migration;
14. verifies no UI dependency;
15. exits non-zero on any mismatch.

---

## 72. Unit Test Matrix

### Catalog
- valid template;
- duplicate ID;
- unsupported operation type;
- missing localization;
- unsupported effect sink;
- illegal duration.

### Start validation
- missing faction;
- missing survivor;
- unavailable survivor;
- cooldown active;
- suspicion too high;
- allowed start.

### Determinism
- same seed same outcome;
- save/load same outcome;
- cancel/restart cannot reroll same instance;
- different instance deterministic but distinct stream.

### Suspicion
- gain;
- decay;
- floor after confirmed exposure;
- clamping 0..100.

### Intelligence
- raw report;
- decode;
- false/low-confidence claim;
- stale report;
- duplicate decode prevention.

### Consequences
- standing event;
- moral event;
- sabotage sink;
- capture/death handoff;
- combat handoff.

### Persistence
- empty;
- active;
- reports;
- compromise;
- cooldown.

---

## 73. Golden Scenario Fixtures

Create at least:

1. Low-risk infiltration success, undetected.
2. Infiltration success, later compromised.
3. Theft success with high-value report.
4. Theft failure with escape.
5. Sabotage success, undetected.
6. Sabotage success, exposed.
7. Propaganda success.
8. High-suspicion operation blocked.
9. Ally espionage discovered with diplomatic/moral consequences.
10. Agent captured.
11. Agent killed through canonical fate handoff.
12. Intelligence decoded.
13. False intelligence produced.
14. Mid-operation save/reload.
15. Old save with no espionage state.
16. No-op campaign where espionage never used.

---

## 74. Property / Fuzz Testing

Generate valid operation fixtures.

Properties:
- suspicion always 0..100;
- operation resolves at most once;
- deterministic same-state resolution;
- active operation has valid agent/faction;
- report IDs unique;
- decode never produces duplicate strategic effect;
- no survivor assigned to incompatible operations;
- effect target resolves;
- no invalid status transition.

---

## 75. Performance Budget

Espionage is day/event-driven.

Requirements:
- catalog parsed once;
- no per-frame polling;
- active-operation ticking O(active operations);
- faction suspicion decay O(factions) on day tick;
- report list bounded/archivable if campaign very long;
- no full survivor scan where indexed availability exists.

A campaign with zero espionage should impose negligible cost.

---

## 76. UI Accessibility

Panel should support:
- keyboard navigation;
- controller if supported;
- text scale;
- risk represented by text/icon, not color alone;
- suspicion represented numerically or with labeled bands;
- operation consequences readable before confirmation;
- reduced-motion animations;
- no tooltip-only critical risk information.

---

## 77. Operation Confirmation UX

Before starting, show:

- operation;
- target;
- agent;
- duration;
- expected risk;
- expected suspicion change band;
- major possible consequences;
- whether ally relations can be harmed.

Do not reveal hidden exact outcomes.

---

## 78. Failure Handling

### Missing downstream sink
Operation definition fails validation; do not silently "succeed" with prose only.

### Agent disappears mid-operation
Resolve according to canonical survivor state:
- dead -> operation abort/fail;
- incapacitated -> abort/compromise depending design;
- removed from campaign -> explicit migration/failure.

### Target faction removed
Cancel or resolve using documented fallback; never dereference invalid target.

### Catalog revision during live game
Use startup-loaded definitions; do not mutate active operations silently.

---

## 79. Security / Data Safety

Operation data is declarative.

Disallow:
- arbitrary scripts;
- reflection paths;
- shell commands;
- dynamic code execution;
- file paths as effects;
- unvalidated target IDs.

Typed effect registries only.

---

## 80. Implementation Phase A — Audit and Contracts

Tasks:
1. audit faction/survivor/RNG/save/event architecture;
2. create integration audit;
3. create operation definition DTO;
4. create active operation DTO;
5. create intelligence report DTO;
6. create state DTO;
7. create catalog loader;
8. create validator;
9. add schema versioning;
10. add empty migration fixture.

Exit:
Core types compile and catalog can load headlessly.

---

## 81. Implementation Phase B — Deterministic Operation Engine

Tasks:
1. start validation;
2. agent reservation;
3. deterministic seed assignment;
4. day-based ticking;
5. operation state machine;
6. success roll;
7. exposure roll;
8. outcome record;
9. once-only resolution ID;
10. save/reload tests.

Exit:
fixed-seed operations resolve deterministically without downstream effects.

---

## 82. Implementation Phase C — Suspicion

Tasks:
1. faction suspicion state;
2. gain profiles;
3. decay;
4. thresholds;
5. security modifier;
6. operation restrictions;
7. UI projection;
8. tests.

Exit:
repeated covert activity creates persistent, bounded faction awareness.

---

## 83. Implementation Phase D — Intelligence

Tasks:
1. report creation;
2. report IDs;
3. raw/decoded state;
4. decoding assignment;
5. accuracy/confidence;
6. false-intelligence support;
7. staleness;
8. strategic consumer hooks;
9. tests.

Exit:
the game has a typed intelligence asset, not just narrative logs.

---

## 84. Implementation Phase E — Infiltrate / Steal / Sabotage / Propaganda

Tasks:
1. implement infiltration profile;
2. periodic infiltration checkpoints;
3. implement theft profile;
4. implement sabotage effect registry;
5. wire supported sabotage sinks;
6. implement propaganda supported effects;
7. author first operation templates;
8. add golden fixtures.

Exit:
four core covert families produce real gameplay consequences.

---

## 85. Implementation Phase F — Assassination Gate

Before enabling:
1. verify target leader IDs;
2. verify fate authority;
3. verify succession;
4. verify quest safety;
5. verify faction state remains coherent.

If any prerequisite fails, keep assassination templates disabled and document follow-on work.

This is preferable to shipping a hollow "leader killed" message.

---

## 86. Implementation Phase G — Cross-System Consequences

Wire:
- faction standing;
- stance/trust;
- moral events;
- survivor fate;
- relationship events;
- expedition;
- tactical combat;
- research stolen-knowledge bridge;
- quest hooks.

Every integration receives stable operation outcome IDs for idempotency.

---

## 87. Implementation Phase H — UI / Journal / Guidance

Tasks:
1. panel projection;
2. operation list;
3. suspicion;
4. reports;
5. decode view;
6. operation confirmation;
7. once-only notifications;
8. journal hooks;
9. first-use guidance;
10. accessibility.

Exit:
UI is a renderer/controller over Core contracts.

---

## 88. Implementation Phase I — 15-Template Catalog

Author templates only after consumers exist.

Coverage:
- 3 infiltration;
- 4 theft;
- 4 sabotage;
- 2 propaganda;
- up to 2 extreme/assassination/cover-up entries depending support.

Validate reachability and consequence sinks.

Exit:
15 templates if and only if all are meaningful and executable.

---

## 89. Implementation Phase J — CI Hardening

Tasks:
1. `--espionage-selftest`;
2. data-integrity extension;
3. migration tests;
4. fuzz tests;
5. performance sanity;
6. source docs;
7. coverage report;
8. unresolved target report;
9. full regression.

Exit:
headless CI proves deterministic covert simulation.

---

## 90. Coverage Report

Generate:
`docs/espionage/ESPIONAGE_COVERAGE.md`

Include:

| Operation Type | Templates | Downstream effect sinks | Fixtures | Enabled |
|---|---:|---:|---:|---:|
| infiltrate | | | | |
| steal | | | | |
| sabotage | | | | |
| propaganda | | | | |
| assassinate | | | | |

Also:
- factions targetable;
- factions blocked;
- suspicion profile coverage;
- intelligence types;
- effect kinds;
- templates with no real consumer;
- operation types never selected by tests.

---

## 91. Risk Register

### Risk: system overwhelms player
Mitigation:
- start with four core operation families;
- bounded concurrent slots;
- clear risk bands;
- progressive unlocks if design supports.

### Risk: espionage trivializes diplomacy
Mitigation:
- suspicion;
- opportunity cost;
- failure consequences;
- incomplete/false intelligence;
- cooldowns.

### Risk: save-scumming
Mitigation:
- persisted operation seed;
- start-time seed assignment;
- consequence idempotency.

### Risk: fake sabotage
Mitigation:
- operation templates require real effect sinks.

### Risk: assassination breaks quests
Mitigation:
- gate behind verified leader/fate/succession support.

### Risk: duplicate state authority
Mitigation:
- espionage emits typed consequences; faction/moral/survivor systems remain owners.

---

## 92. Definition of Done — Flagship

### Core
- [ ] `EspionageSystem.cs`
- [ ] operation definitions
- [ ] active operation state
- [ ] intelligence reports
- [ ] suspicion
- [ ] schema-versioned capture/restore
- [ ] deterministic RNG

### Operations
- [ ] infiltrate
- [ ] steal
- [ ] sabotage
- [ ] propaganda
- [ ] assassination only if prerequisites supported
- [ ] cooldowns
- [ ] anti-reroll seeds
- [ ] once-only resolution

### Intelligence
- [ ] raw reports
- [ ] decoding
- [ ] confidence/accuracy
- [ ] false information
- [ ] strategic consumer hooks
- [ ] no truth leakage

### Agents
- [ ] availability validation
- [ ] assignment reservation
- [ ] compromise
- [ ] capture/death handoff
- [ ] release on completion/cancel
- [ ] save round-trip

### Factions
- [ ] suspicion per faction
- [ ] standing/trust consequence sinks
- [ ] high-suspicion restrictions
- [ ] old saves start clean

### UI
- [ ] active operations
- [ ] available operations
- [ ] intelligence reports
- [ ] suspicion
- [ ] risk communication
- [ ] guidance
- [ ] accessibility

### Validation
- [ ] catalog integrity
- [ ] all target IDs resolve
- [ ] all effect kinds have consumers
- [ ] selftest
- [ ] fuzz tests
- [ ] mid-operation save/load
- [ ] no UI dependency

---

## 93. Follow-On Task 153-A — Counter-Espionage

Goal:
allow the shelter to detect hostile spies using the same suspicion/intelligence architecture.

Substeps:
1. define hostile infiltration facts;
2. create detection authority;
3. use survivor/security capabilities;
4. create mole-hunt events;
5. create false-positive risk;
6. integrate faction attribution;
7. add save state;
8. add selftests.

Do not implement until player espionage state contracts are stable.

---

## 94. Follow-On Task 153-B — Double Agents

Goal:
support turned operatives without creating contradictory survivor authority.

Substeps:
1. define allegiance/cover fact contract;
2. define intelligence poisoning;
3. define turn detection;
4. define extraction/defection;
5. integrate relationship/moral consequences;
6. deterministic seeded behavior;
7. epilogue tags;
8. tests.

---

## 95. Follow-On Task 153-C — Intelligence Trading

Goal:
allow decoded reports to become tradeable strategic assets.

Substeps:
1. define report trade eligibility;
2. define duplicate/consumed semantics;
3. integrate economy/trade;
4. prevent infinite resale exploits;
5. integrate faction trust;
6. add UI;
7. add tests.

Espionage owns the report; economy owns transaction value.

---

## 96. Follow-On Task 153-D — Spymaster Specialization

Goal:
allow survivor progression systems to recognize sustained covert expertise.

Substeps:
1. emit contribution facts;
2. let canonical skill system assign progression;
3. add traits if architecture supports;
4. add operation bonuses;
5. keep progression outside EspionageState where possible;
6. test save/load and availability.

---

## 97. Follow-On Task 153-E — Espionage Legacy / Epilogue

Goal:
surface historically significant covert operations in Plan 145's ending system.

Substeps:
1. define notable-operation criteria;
2. export stable fact IDs;
3. add epilogue tags;
4. author templates;
5. prevent routine operation spam in ending prose;
6. add golden ending fixtures.

---

## 98. Final Execution Checklist for Coding Agent

### Pass 0 — Read
- [ ] inspect faction/trust/expedition/moral/survivor/RNG/save architecture
- [ ] inspect catalogs
- [ ] inspect bootstrap
- [ ] record baseline tests

### Pass 1 — Contract
- [ ] DTOs
- [ ] catalog
- [ ] loader
- [ ] validator
- [ ] save state

### Pass 2 — Engine
- [ ] start validation
- [ ] assignment
- [ ] tick
- [ ] deterministic resolution
- [ ] idempotency

### Pass 3 — Suspicion
- [ ] gain
- [ ] decay
- [ ] bands
- [ ] security influence

### Pass 4 — Intelligence
- [ ] reports
- [ ] decode
- [ ] accuracy
- [ ] false claims
- [ ] staleness

### Pass 5 — Operations
- [ ] infiltrate
- [ ] steal
- [ ] sabotage
- [ ] propaganda
- [ ] assassination gate

### Pass 6 — Integration
- [ ] faction
- [ ] stance
- [ ] moral
- [ ] survivor
- [ ] expedition
- [ ] combat
- [ ] quests
- [ ] research hooks

### Pass 7 — Presentation
- [ ] panel
- [ ] journal
- [ ] guidance
- [ ] accessibility

### Pass 8 — Hardening
- [ ] save/load
- [ ] old save
- [ ] selftest
- [ ] fuzz
- [ ] catalog coverage
- [ ] full regression

---

## 99. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --espionage-selftest
```

Also run the repository's bridge/integration fast gates if they exist.

A successful build alone is insufficient. The system must prove deterministic mid-operation save/reload,
idempotent consequences, valid operation targets, and headless operation.

---

## 100. Final Guardrails

- No `System.Random`.
- No wall-clock seeding.
- No reroll by cancel/restart.
- No per-frame espionage scan.
- No duplicate survivor assignment.
- No fake sabotage effect with no consumer.
- No faction-standing duplicate authority.
- No moral-score duplicate authority.
- No direct survivor deletion.
- No truth leakage from false intelligence.
- No assassination of unsupported quest-critical leaders.
- No operation definition with unresolved target IDs.
- No report benefit that exists only in tooltip prose.
- No suspicion value outside 0..100.
- No unlimited spam against one faction.
- No UI business logic for outcome resolution.
- No save/load change in deterministic result.
- No duplicate resolution event.
- No old-save crash.
- No 15-template vanity target filled with dead operations.

When complete, Plan 153 adds a real covert strategic layer rather than a cosmetic panel. The player can choose
between overt diplomacy and covert leverage; factions can become wary without instantly becoming enemies;
intelligence can be useful without being perfectly trustworthy; survivors become meaningful strategic assets
whose capture or loss matters; and every covert consequence routes into the existing systems that already own
the world state.
