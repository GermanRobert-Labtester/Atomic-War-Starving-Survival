# D1 Flagship Integration Plan [4]
## Plan 145 — Unified Ending Resolution & Epilogue Personalization

> **Purpose:** Convert ASHFALL's fragmented late-game ending logic into one deterministic, inspectable,
> save-compatible, data-driven endgame resolution pipeline that reflects the player's actual campaign.
>
> **Primary source plan:** Plan 145 — Unified Ending Resolution & Epilogue Personalization.
>
> **Execution posture:** integration-first, preservation-first, deterministic, catalog-validated,
> headless-testable, migration-safe, and suitable for implementation by a coding agent with repository access.
>
> **Core problem:** the project currently owns several endgame systems that can each describe a different
> ending, but there is no authoritative resolver that decides how they compose. Holdfast endings, Muster
> epilogues, the epilogue matrix, faction branches, moral state, survivor outcomes, expedition discoveries,
> shelter development, and Verdict outcomes can all exist simultaneously without a single mechanism that
> turns them into one coherent campaign conclusion.
>
> **Flagship objective:** create a canonical endgame authority that consumes campaign state once, resolves
> every ending category once, generates a stable personalized epilogue once, persists that result, and exposes
> the same result to UI, journal, export, achievements, New Game+ hooks, telemetry, and regression tests.
>
> **Non-goal:** this plan does not replace authored ending content with unconstrained runtime text generation.
> All player-visible prose remains authored, templated, localized, deterministic, and validated against real
> IDs. Procedural composition means deterministic selection and slotting of authored fragments, never live LLM
> generation in a shipped build.

---

## 1. Source Problem Statement and Repository Evidence

The source plan identifies three independent ending authorities:

1. `Assets/Ashfall.Core/HoldfastEndings.cs` with five armed Holdfast endings:
   `Schedule`, `Reserve`, `DarkRoad`, `Tender`, and `White`.
2. `Assets/StreamingAssets/Data/muster_epilogues.json` with twelve Muster epilogue variants.
3. `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`, which evaluates a broader context matrix but
   currently leaves at least two evaluation fields unused by branching logic.

The result is not merely duplicate code. It is an authority problem. Multiple systems can answer the same
question — "what ending did this campaign produce?" — while each sees only a subset of the campaign. That
creates five classes of defects:

- **Contradictory outcome defect:** one subsystem can imply political consolidation while another outputs prose
  consistent with fragmentation.
- **Lost-choice defect:** the player may have made hundreds of decisions, yet the output may only reflect a few
  aggregate flags.
- **Unconsumed-state defect:** context fields can be captured but never influence the result, creating false
  confidence that the epilogue is personalized.
- **replay instability defect:** if individual systems resolve independently or late, save/load order can change
  what gets displayed or recorded.
- **maintenance divergence defect:** every new faction branch, survivor fate, endgame doctrine, or campaign
  event must be manually wired into several ending systems, increasing drift.

The source plan also states that the current ending text is effectively a set of fixed paragraphs that do not
sufficiently reference faction branches, Muster approach, survivor fates, moral choices, expeditions, or shelter
development. The flagship integration therefore treats personalization as a state-attribution problem first and
a prose problem second.

### Core integration principle

The final epilogue must be a **function of a frozen campaign snapshot**, not a loose collection of late reads
against mutable runtime systems.

Conceptually:

```text
FinalCampaignSnapshot
    -> UnifiedEndingResolver
        -> EndingFacts
            -> PersonalizedEpilogueGenerator
                -> UnifiedEndingResult
                    -> UI
                    -> Journal
                    -> Save
                    -> Export
                    -> Achievements / Legacy consumers
```

Every stage must be deterministic, inspectable, and independently testable.

---

## 2. Flagship Success Criteria

The work is complete only when all of the following are true simultaneously:

- There is exactly one authoritative endgame resolution entry point.
- Existing Holdfast, Muster, matrix, faction, moral, survivor, expedition, shelter, and Verdict systems become
  **inputs** to that authority rather than competing ending authorities.
- The resolver consumes a frozen `UnifiedEndingContext`.
- Every context field is either intentionally used or explicitly documented as non-authoritative.
- The resolver emits structured ending facts before prose generation.
- Prose generation consumes only the structured facts and authored template catalogs.
- Identical campaign state produces byte-stable result identifiers and stable ordered fragments.
- Ending resolution is one-shot and persisted; reopening the ending does not re-resolve mutable state.
- Old saves load and can derive a compatible result without corrupting historical state.
- Headless execution can resolve an ending without loading UI scenes or media.
- All referenced survivor IDs, quest IDs, faction IDs, ending IDs, art keys, audio keys, localization keys, and
  template IDs resolve through validation.
- The system supports extinction/all-dead, no-faction, incomplete-Muster, no-Verdict, and other sparse end states.
- The player can see which campaign facts drove the ending.
- The epilogue journal stores the resolved result rather than regenerating it later.
- The CI suite contains a dedicated `--unified-ending-selftest`.
- Save round-trip preserves the exact resolved outcome.
- No runtime RNG is used in ending selection or prose order.
- No prose template can silently reference a choice that did not occur.
- No ending category is allowed to override another category without an explicit precedence rule.

---

## 3. Scope and Explicit Boundaries

### In scope

- Canonical endgame context DTOs.
- Campaign snapshot collection.
- Unified resolution categories.
- Category precedence and contradiction handling.
- Personalized prose composition.
- Survivor-specific epilogues.
- Expedition and shelter retrospectives.
- Campaign-length framing.
- Verdict/judicial closure.
- Save-state versioning and migration.
- One-shot resolution semantics.
- UI model and presentation contracts.
- Journal persistence.
- Replay of already-resolved endings.
- Ending summary export.
- Audio/art hooks as keyed data.
- Data integrity and localization validation.
- Regression fixtures covering representative campaign outcomes.
- CI and release gates.
- Transitional adapters for existing ending systems.

### Out of scope for the first implementation pass

- New Game+ mechanics themselves.
- Achievement rewards themselves.
- A full cinematic editor.
- Dynamic runtime natural-language generation.
- Rewriting all existing ending prose before the authority layer works.
- Rebalancing all faction or moral systems.
- Reauthoring every survivor biography.
- Adding dozens of new world-ending branches not supported by existing gameplay.
- Multiplayer/shared ending logic.

The implementation should first unify authority and preserve behavior. Personalization depth is layered on top
once determinism and compatibility are locked.

---

## 4. Repository Reconnaissance Before Editing

Before creating new code, perform a focused repository inspection and record results in
`docs/endgame/ENDING_INTEGRATION_AUDIT.md`.

Inspect at minimum:

- `Assets/Ashfall.Core/HoldfastEndings.cs`
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`
- `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`
- `Assets/StreamingAssets/Data/muster_epilogues.json`
- `Assets/StreamingAssets/Data/epilogue_chronicle.json`
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`
- `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs`
- any survivor fate, memorial, death, retirement, succession, relationship, and legacy systems
- quest completion ledgers
- expedition discovery ledgers
- shelter upgrade/state ledgers
- campaign day/clock authority
- save capture/restore contracts
- journal event contracts
- ending UI scenes, presenters, DTOs, and navigation
- localization catalogs
- audio cue registries
- asset/provenance registries

For each source, capture:

| Question | Required answer |
|---|---|
| Who owns the state? | Exact class/data authority |
| Is it persisted? | Save key/schema |
| Is it deterministic? | yes/no + cause |
| Is it already exposed as a DTO? | type name |
| Can it be absent? | default/nullable behavior |
| Can it change after the ending trigger? | mutation path |
| Does an existing ending consumer read it? | consumer list |
| Does a catalog validate its IDs? | validator/selftest |
| Does UI read the domain directly? | coupling risk |
| Is there legacy save migration? | migration path |

Do not assume file names named in this plan still match the repository. Where a class or catalog has moved,
record the canonical replacement and adapt the implementation plan to the actual repository.

---

## 5. Canonical Endgame Domain Model

Create a domain model under `Assets/Ashfall.Core/Endgame/` that separates **raw campaign facts** from
**resolved ending facts**.

### 5.1 `UnifiedEndingContext`

Recommended fields:

```csharp
public sealed record UnifiedEndingContext
{
    public int SchemaVersion { get; init; }
    public string CampaignId { get; init; }
    public long TotalDaysSurvived { get; init; }

    public string? FactionBranchId { get; init; }
    public string? MusterApproachId { get; init; }
    public MoralChoiceBand MoralChoiceBand { get; init; }
    public string? HoldfastEndingId { get; init; }
    public string? VerdictEndingId { get; init; }

    public IReadOnlyList<SurvivorEndingFact> SurvivorFates { get; init; }
    public IReadOnlyList<string> MajorQuestCompletions { get; init; }
    public IReadOnlyList<ExpeditionEndingFact> ExpeditionDiscoveries { get; init; }
    public IReadOnlyList<ShelterEndingFact> ShelterUpgrades { get; init; }
    public IReadOnlyDictionary<string, int> FactionStandings { get; init; }

    public IReadOnlyList<MoralEndingFact> KeyMoralDecisions { get; init; }
    public IReadOnlyList<RelationshipEndingFact> RelationshipFacts { get; init; }
    public IReadOnlyList<MemorialEndingFact> MemorialFacts { get; init; }

    public bool VelSecretExposed { get; init; }
    public string ContentRevision { get; init; }
}
```

Important: use the repository's actual types and naming conventions. The above is a contract target, not a
copy-paste mandate.

### 5.2 `EndingFacts`

Create an intermediate deterministic structure:

```csharp
public sealed record EndingFacts
{
    public EndingPoliticalFact Political { get; init; }
    public EndingSocialFact Social { get; init; }
    public EndingMoralFact Moral { get; init; }
    public EndingJudicialFact Judicial { get; init; }
    public IReadOnlyList<EndingPersonalFact> Personal { get; init; }
    public IReadOnlyList<EndingWorldFact> World { get; init; }
    public IReadOnlyList<string> LegacyTraitIds { get; init; }
}
```

This layer is essential. It allows testing "what the campaign means" independently from "how prose sounds."

### 5.3 `UnifiedEndingResult`

Recommended persisted result:

```csharp
public sealed record UnifiedEndingResult
{
    public int SchemaVersion { get; init; }
    public string ResolutionId { get; init; }
    public string CampaignId { get; init; }
    public string EndingPrimaryId { get; init; }
    public string EndingTitleKey { get; init; }

    public EndingFacts Facts { get; init; }
    public IReadOnlyList<EpilogueSection> Sections { get; init; }
    public IReadOnlyList<SurvivorEpilogue> SurvivorEpilogues { get; init; }
    public WorldStateSummary WorldStateSummary { get; init; }
    public IReadOnlyList<string> LegacyTraitIds { get; init; }

    public string ContextDigest { get; init; }
    public string TemplateCatalogRevision { get; init; }
}
```

Persist stable IDs and localization/template keys. Avoid persisting only rendered English prose if localization
can change; if historical verbatim replay is a requirement, store both the structural result and the rendered
snapshot.

---

## 6. Ending Trigger and Snapshot Freeze

### Goal

Prevent "ending drift" where mutable campaign state changes between resolution, display, save, replay, or export.

### Implementation

Create a single endgame trigger service or adapt the existing canonical trigger:

```text
EndingTriggerRequested
    -> validate campaign can end
    -> freeze campaign-ending snapshot
    -> compute digest
    -> resolve categories
    -> generate result
    -> persist result
    -> emit EndingResolved(resultId)
    -> UI consumes persisted result
```

### Rules

1. The trigger may be a day threshold, final quest action, evacuation decision, surrender, extinction, or other
   repository-supported end condition.
2. Trigger detection and result resolution must be separate.
3. The snapshot is collected exactly once per successful ending resolution.
4. Any final quest transaction that changes ending facts must commit before snapshot capture.
5. UI must never call `Resolve()` directly.
6. Reloading an ending screen must read the stored `UnifiedEndingResult`.
7. "Replay ending" replays presentation only; it does not recompute campaign facts.
8. Debug tooling may force recomputation only behind an explicit developer verb and must never affect normal saves.
9. The one-shot latch must be persisted.
10. Failed resolution must not partially write the result.

### Transactional pattern

Use repository-appropriate transaction or staged commit semantics:

```text
collect context
validate context
resolve facts
validate facts
compose epilogue
validate result
persist result atomically
publish event
```

If any stage fails, leave the campaign unresolved and emit a diagnostic rather than writing a partial ending.

---

## 7. Category Resolution and Precedence

The source plan proposes five categories: Political, Social, Personal, Moral, and Judicial. Keep them orthogonal
where possible.

### 7.1 Political category

Inputs:
- Holdfast ending
- faction branch
- final controlling faction or authority
- campaign-wide governance state if available

Precedence policy:

1. A valid armed Holdfast ending is the strongest political signal.
2. Faction branch modifies interpretation but should not silently replace Holdfast.
3. If no Holdfast ending is armed, faction branch becomes primary.
4. If neither exists, resolve to a neutral/independent/fractured fallback supported by data.

Do not hardcode prose here. Emit stable fact IDs such as:

```text
political.holdfast.schedule
political.holdfast.dark_road
political.faction.independent
political.fallback.fractured
```

### 7.2 Social category

Inputs:
- faction standings
- refugee/visitor acceptance outcomes
- Muster approach
- commitments kept/broken
- community cohesion if available

Resolve:
- primary social outcome
- notable allies
- notable enemies
- one or more community descriptors

### 7.3 Moral category

Inputs:
- aggregate moral band
- key authored moral choices
- mercy/ruthlessness markers
- sacrifice/exploitation markers

Important: aggregate moral band must not erase specific memorable decisions. Use the band for framing and
specific choices for evidence.

### 7.4 Judicial category

Inputs:
- Verdict ending
- related judicial/reckoning state

The category can be absent. Absence must be represented explicitly rather than mapped to a fake "neutral" result.

### 7.5 Personal category

Inputs:
- survivor fates
- relationships
- memorial/final-wish state
- mentorship/succession
- leadership contribution
- notable quest involvement

Personal output is a ranked list rather than one category token.

### Contradiction detection

Add validation rules that flag impossible or suspicious combinations, for example:

- a survivor marked both deceased and retired
- a faction-specific Holdfast ending with a mutually exclusive faction branch
- a Verdict outcome present when the Verdict system never activated
- a Muster approach not present in the campaign's resolved quest state

Contradictions should fail selftests and log diagnostics in normal runtime. Where legacy saves can contain
contradictory historical states, define deterministic migration precedence.

---

## 8. Personalization Architecture: Facts Before Prose

`PersonalizedEpilogueGenerator` should not query live game systems. It receives only:

- `EndingFacts`
- stable campaign metadata
- an authored template catalog
- localization services
- optional presentation metadata

### Proposed flow

```text
EndingFacts
 -> choose section plan
 -> choose one primary template per category
 -> choose bounded supporting fragments
 -> rank survivor epilogues
 -> insert campaign-specific references
 -> validate no duplicate semantic claims
 -> produce ordered EpilogueSection list
```

### Section order

Recommended default:

1. Opening / campaign-length frame
2. Political settlement
3. Social/community outcome
4. Judicial/Verdict outcome if present
5. Major world consequences
6. Survivor epilogues
7. Moral reflection
8. Closing legacy statement

The order should be data-driven so future endings can intentionally reorder sections.

### No live generation

Avoid runtime LLM or arbitrary grammar synthesis. Reasons:

- localization consistency
- deterministic save replay
- rating/content control
- testability
- offline behavior
- platform compliance
- authored tone preservation

Use authored templates with tokens referencing validated facts.

---

## 9. Prose Template Data Authority

Create a data authority such as:

`Assets/StreamingAssets/Data/endgame_epilogue_templates.json`

or use the repository's established catalog location.

### Suggested schema

```json
{
  "schemaVersion": 1,
  "templates": [
    {
      "id": "epilogue.political.schedule.military",
      "category": "political",
      "priority": 100,
      "conditions": {
        "holdfastEndingId": "Schedule",
        "factionBranchId": "Military"
      },
      "textKey": "epilogue.political.schedule.military.body",
      "titleKey": "epilogue.political.schedule.military.title",
      "tags": ["holdfast", "military", "schedule"]
    }
  ]
}
```

### Template requirements

Every template must have:

- stable ID
- category
- explicit condition set
- localization key(s)
- priority/specificity
- deterministic tie-break key
- optional art cue key
- optional audio cue key
- optional chronicle slide family
- optional exclusion tags
- optional required fact IDs

### Selection algorithm

1. Filter by category.
2. Filter by all required conditions.
3. Reject templates whose exclusions match.
4. Sort by specificity descending.
5. Sort by priority descending.
6. Sort by stable ID ascending as final deterministic tie-break.
7. Pick exactly one primary template unless the category supports a bounded additive set.

Never depend on JSON insertion order.

### Initial authoring target

The source asks for 50 personalized prose templates. Implement in two phases:

- Phase A: 20 core templates covering major branch combinations.
- Phase B: expand to 50 after the resolver and coverage report reveal real gaps.

Do not invent combinations for states the game cannot produce. The catalog should be coverage-driven.

---

## 10. Survivor Epilogue Selection and Ranking

The source plan calls for individualized survivor endings. This can explode combinatorially unless the system
uses bounded ranking.

### Candidate scoring

Rank survivor candidates with deterministic rules such as:

- +100 mandatory story-critical survivor
- +80 shelter leader or successor
- +60 participant in final quest
- +50 survivor with major moral event
- +40 survivor with unique relationship arc
- +30 survivor with memorial/final-wish event
- +20 highest contribution/skill milestone
- +10 long-tenure survivor
- deterministic tie-break by survivor ID

Do not use random selection.

### Output budget

Suggested:
- 3–6 detailed survivor epilogues
- one compact rollup for remaining survivors
- all deceased named in memorial rollup if player-facing design requires it

The source requirement "for each notable survivor" should mean each survivor satisfying the repository-defined
notability predicate, not all 129+ survivors receiving equally long bespoke prose.

### Fate-specific branches

For each notable survivor:

**Alive**
- reference role, major event, relationship, or long-term outcome
- avoid inventing occupations unsupported by traits/state

**Deceased**
- reference actual death, memorial, final wish, legacy object, or remembered event where available
- never claim a memorial if none occurred

**Retired**
- reference mentorship, succession, departure, or role transition only when facts support it

### Survivor epilogue DTO

```csharp
public sealed record SurvivorEpilogue
{
    public string SurvivorId { get; init; }
    public SurvivorFate Fate { get; init; }
    public string TemplateId { get; init; }
    public IReadOnlyList<string> SupportingFactIds { get; init; }
    public string PortraitKey { get; init; }
}
```

Persist the selected template/fact IDs for stable replay.

---

## 11. Expedition, Quest, and Shelter Retrospective Integration

### Expeditions

Do not mention every discovery. Introduce `ExpeditionEndingFact` with:

- discovery ID
- location ID
- importance tier
- success/failure/loss outcome
- named survivor involvement if available
- world consequence flag

Rank:
1. discoveries that changed a system/campaign state
2. locations tied to final branch
3. major losses
4. unique authored discoveries
5. ordinary loot runs excluded

### Major quests

Create a curated `ending_relevance` or equivalent data flag rather than assuming every completed quest deserves
epilogue text.

Suggested tiers:

- `critical`: must be eligible
- `major`: eligible when category-relevant
- `minor`: not directly narrated
- `ambient`: excluded

### Shelter upgrades

Mention only upgrades with durable identity or systemic importance.

Examples:
- greenhouse
- radiation shielding
- air filtration
- power infrastructure
- memorial space
- medical capability

Avoid generic prose such as "you upgraded the shelter many times" if the exact upgrades are known.

### Defense and crisis events

If a canonical event ledger exists, allow a small number of high-salience references:
- largest raid survived
- catastrophic fire/contamination
- famine
- mass casualty event
- evacuation
- major recovery

Keep the selector bounded so prose remains coherent.

---

## 12. Campaign-Length Framing

The source suggests:
- short `<180 days`
- medium `180–365`
- long `>365 days`

Before hardcoding those thresholds, audit existing campaign design lengths. If the project normally runs
120–180 days, a `>365` branch may be unreachable or developer-only.

Implement the framing thresholds in data:

```json
{
  "campaignLengthBands": [
    { "id": "short", "maxInclusive": 179 },
    { "id": "standard", "minInclusive": 180, "maxInclusive": 365 },
    { "id": "long", "minInclusive": 366 }
  ]
}
```

Then validate reachability against configured campaign lengths.

Ensure `totalDaysSurvived`, which the source specifically calls out as currently unused in some matrix logic,
actually influences either framing, a category fact, or an explicit documented output.

---

## 13. `velSecretExposed` and Previously Unused Context Fields

The source plan notes that `totalDaysSurvived` and `velSecretExposed` exist in the evaluation context but are not
read by current branching logic.

Treat that as a broader integration smell.

### Audit rule

For every `UnifiedEndingContext` field, produce a generated coverage table:

| Context field | Collected from | Used by resolver? | Used by prose? | Tested? |
|---|---|---:|---:|---:|
| `TotalDaysSurvived` | Campaign clock | yes | yes | yes |
| `VelSecretExposed` | canonical secret state | yes/no intentionally | yes/no | yes |
| ... | ... | ... | ... | ... |

A field that is collected but unused must be:
- removed,
- explicitly reserved with rationale, or
- wired into a meaningful result.

No silent dead fields.

---

## 14. Legacy Adapters for Existing Ending Systems

The safest migration path is adapter-first.

### Holdfast adapter

Create an adapter that exposes:

```text
GetArmedHoldfastEnding() -> stable ending id or none
```

Do not initially rewrite Holdfast internals.

### Muster adapter

Expose the resolved Muster approach and any canonical variant ID. Existing Muster prose becomes input content
or a legacy compatibility source, not the final authority.

### Epilogue matrix adapter

Run the existing matrix in shadow mode during transition:
- record old result
- record new facts/result
- compare expected equivalences
- do not display both

### Verdict adapter

Expose a stable judicial outcome ID.

### Faction adapter

Expose the final branch and standings from the canonical coordinator.

### Moral adapter

Expose the band plus selected key decisions.

### Survivor adapter

Expose final fate ledger.

### Deprecation plan

1. Build adapters.
2. Introduce unified resolver.
3. Run old and new in parallel in tests.
4. Add compatibility fixtures.
5. Switch UI to unified result.
6. Switch journal/export.
7. Remove dead duplicated resolution paths only after coverage proves parity or intentionally documented change.

---

## 15. Save/Load Contract and Schema Versioning

### Persisted fields

At minimum save:

- whether ending has resolved
- `UnifiedEndingResult`
- context digest
- resolver schema version
- template catalog revision
- optional rendered-localization snapshot if historical verbatim replay is required

### One-shot semantics

```text
if save.EndingResolved:
    return save.UnifiedEndingResult
else:
    resolve -> validate -> atomically save -> return
```

### Capture/restore responsibilities

`UnifiedEndingResolver` should not persist every upstream system's state. It persists its own result and any
minimal metadata required for stable replay.

### Migration cases

Support:

1. Old save, campaign not ended.
   - no result exists
   - unified system initializes empty
   - normal ending later uses current resolver

2. Old save, campaign ended under legacy system.
   - derive context from persisted historical state where possible
   - map legacy ending IDs to unified categories
   - mark result as migrated
   - do not invent missing facts

3. Old save with incomplete survivor history.
   - use only known final fates
   - produce reduced personalization rather than fabricated details

4. Save with deprecated template ID.
   - result should still replay from persisted structural output or frozen rendered copy

5. Save with contradictory legacy flags.
   - apply documented deterministic precedence
   - emit migration diagnostic

### Migration audit artifact

Create:
`docs/endgame/ENDING_SAVE_MIGRATION_MATRIX.md`

---

## 16. Old-Save Retroactive Resolution

The source says existing saves should get a default unified ending. Improve that requirement: do not default away
available historical information.

Retroactive algorithm:

1. Read legacy ending state.
2. Read final faction branch if persisted.
3. Read moral band if persisted.
4. Read survivor final roster if persisted.
5. Read Verdict/Muster state if persisted.
6. Build a partial `UnifiedEndingContext`.
7. Mark unknown fields explicitly.
8. Resolve only categories with sufficient evidence.
9. Use compatibility templates for missing-data categories.
10. Persist a `migratedFromLegacy=true` marker.

The goal is graceful degradation, not false precision.

---

## 17. Determinism and Digesting

### Context digest

Serialize the normalized `UnifiedEndingContext` with:
- sorted dictionaries
- stable list ordering where semantic order is irrelevant
- explicit null/default handling
- schema version

Hash to a digest stored with the result.

### Determinism requirements

Same normalized context + same resolver/catalog revision must produce:

- same primary ending ID
- same category fact IDs
- same selected template IDs
- same survivor ordering
- same section ordering
- same legacy trait IDs
- same context digest

Rendered text may differ by locale but must map to the same structural result.

### Test

Run each golden context 100 times and assert structural equality.

---

## 18. Ending Resolution ID and Replay Safety

Generate a stable resolution identity:

```text
resolutionId = hash(
  campaignId,
  contextDigest,
  resolverSchemaVersion,
  templateCatalogRevision
)
```

This is useful for:
- duplicate event suppression
- replay
- journal linking
- export
- achievements
- telemetry
- support diagnostics

Never use a timestamp or RNG as the sole identity.

---

## 19. UI Integration

### UI should receive a presentation DTO

Do not let the ending scene query campaign systems.

Suggested model:

```csharp
public sealed record EndingPresentationModel
{
    public string Title;
    public IReadOnlyList<EndingPresentationSection> Sections;
    public IReadOnlyList<SurvivorCardModel> SurvivorCards;
    public IReadOnlyList<EndingMomentModel> KeyMoments;
    public IReadOnlyList<string> LegacyTraitIds;
    public string ReplayId;
}
```

### Required views

- title/primary outcome
- category sections
- survivor cards
- key moments
- world-state summary
- legacy trait summary
- replay button
- journal link
- export summary action if supported

### Navigation

- keyboard-only
- controller-compatible if controller support exists
- scroll position preserved while opening survivor details
- text scaling supported
- reduced-motion mode
- captions/subtitles for any voice/audio narration
- no auto-advance that prevents reading

### Error fallback

If art/audio is missing, the ending must remain fully readable and complete in text.

---

## 20. Ending Journal Integration

The journal should store a reference to the resolved ending plus the structural sections.

Requirements:

- One journal record per resolution ID.
- Reopening the journal never re-runs ending logic.
- The journal record includes resolved category IDs and selected survivor epilogue IDs.
- If localized text is rendered dynamically, use current locale from stable keys.
- If historical wording must remain immutable across patches, store a rendered snapshot as well.
- Exported journal text clearly distinguishes primary ending, survivor epilogues, and legacy summary.
- Save migration does not duplicate entries.

---

## 21. Ending Replay

Replay means re-present the existing result.

Replay must not:
- alter save state
- fire rewards twice
- fire achievements twice
- append duplicate journal entries
- recompute survivor fates
- use updated live faction state
- select different templates after a patch

If a patch changes presentation assets, it is acceptable for the same structural ending to render with updated
art/audio unless the project promises historical media fidelity.

---

## 22. Ending Summary Export

The source requests a shareable ending summary. Implement as a safe, explicit export.

Possible output:
- Markdown or plain text
- campaign identifier that is non-sensitive
- primary ending title
- campaign duration
- category summaries
- notable survivor fates
- legacy traits
- optional build/version

Do not include:
- absolute local save paths
- account identifiers
- telemetry IDs
- private machine information

Add a deterministic export test.

---

## 23. Audio Integration

The source requests ending music/audio cues. Keep audio subordinate to structural ending facts.

### Data keys

Each primary/category template may specify:
- `musicCue`
- `ambientCue`
- `stingerCue`
- `survivorThemeCue` where such themes truly exist

### Rules

- missing cue does not fail ending resolution
- invalid cue fails data integrity
- replay uses stored structural ending and resolves cue keys through current catalog
- music selection is deterministic
- no per-survivor custom theme requirement unless assets actually exist

### Initial implementation

Start with major ending-family cues rather than dozens of bespoke tracks:
- political consolidation
- bleak/fractured
- tender/hopeful
- judicial closure
- extinction

Expand only after asset inventory confirms coverage.

---

## 24. Art Integration

The source requests unique ending illustrations, survivor portraits, and location art. Use keyed references and
asset validation.

### Art slots

- primary ending illustration
- optional category illustration
- survivor portrait
- one or two key-moment images
- optional expedition location image

### Guardrails

- no placeholder art may ship as final without explicit placeholder flag
- no ending resolution may depend on art availability
- every art key must resolve through asset registry
- portrait fallback is deterministic
- key-moment art only appears when its supporting event fact exists
- marketing/mockup art must not be reused as an in-game ending asset unless intentionally registered

---

## 25. Localization Architecture

All player-facing ending text must use localization keys.

Validate:
- every template key exists
- every locale has required critical ending keys
- token placeholders match across locales
- no raw survivor display name is mistaken for a localization key
- grammar-sensitive substitutions are minimized

Prefer templates that insert proper names and stable nouns rather than assembling complex sentence fragments
whose grammar breaks in inflected languages.

Add a localization coverage report specifically for endgame content.

---

## 26. Template Token System

If templates use tokens, keep the token vocabulary small and typed.

Example:

```text
{survivorName}
{factionName}
{locationName}
{daysSurvived}
{questTitle}
```

Rules:
- no arbitrary reflective property access
- each token has a typed resolver
- unresolved token is a validation error
- tokens cannot execute logic
- conditional logic belongs in template selection, not inline text
- token formatting is locale-aware

---

## 27. Moral Personalization Without Flattening

The source requires both moral band and specific choices.

Implement two levels:

### Framing fact
`moral.band.very_positive`, `moral.band.negative`, etc.

### Evidence facts
Examples derived from actual decisions:
- shared final ration
- executed prisoner
- admitted refugees
- abandoned injured expedition member
- kept/broke commitment
- spared rival

The prose generator may choose:
- one framing template
- up to two evidence fragments

Do not narrate moral judgment using choices that were never made.

---

## 28. Faction Personalization

Faction standings and faction branch should not be conflated.

Possible final facts:
- `faction.branch.military`
- `faction.branch.rebel`
- `faction.ally.hydro_barons`
- `faction.enemy.<id>`
- `faction.neutral.<id>`

The social epilogue can then mention high-salience relationships.

Selection rule:
- top allied faction
- top hostile faction
- branch-defining faction
- avoid listing every standing

All faction IDs must resolve through the canonical faction catalog.

---

## 29. Muster Integration

Muster approach is an input to social/political interpretation, not a separate full ending authority.

Tasks:

1. Identify where Muster outcome is canonically persisted.
2. Map all existing approach IDs.
3. Preserve existing authored Muster prose where useful by converting it into templates or references.
4. Ensure impossible combinations are rejected.
5. Add fixture for each existing Muster approach.
6. Confirm a campaign that never enters Muster resolves cleanly with `none`.

---

## 30. Holdfast Integration

Tasks:

1. Audit arming semantics for the five known Holdfast endings.
2. Determine whether multiple endings can be armed simultaneously.
3. If yes, define deterministic priority or treat as invalid.
4. Preserve legacy outcome IDs.
5. Map each Holdfast ending to political fact(s).
6. Add one fixture per Holdfast ending.
7. Add at least one fixture where Holdfast is absent.
8. Ensure UI no longer reads Holdfast directly once migration is complete.

---

## 31. Epilogue Matrix Integration

The matrix remains useful as a compatibility oracle during migration.

Tasks:

1. Enumerate all current matrix inputs.
2. Enumerate all current matrix branch outputs.
3. Generate a permutation test report.
4. Identify fields unused by branching.
5. Decide whether each unused field belongs in unified resolution.
6. Run old matrix and new resolver across equivalent contexts.
7. Document intentional semantic differences.
8. Deprecate the matrix only after all meaningful behaviors are represented.

Do not immediately delete a system that currently encodes undocumented business logic.

---

## 32. Verdict Integration

Treat Verdict as the Judicial category.

Tasks:

- expose final Verdict outcome ID
- preserve no-Verdict state
- map Recount/Held/Lease or actual repository IDs
- validate timing: Verdict must commit before ending snapshot if it influences epilogue
- include one judicial section when present
- do not force judicial prose into campaigns that never reached Verdict

---

## 33. Extinction and Sparse-State Endings

The source explicitly names the all-survivors-dead edge case.

Create dedicated support for:

- all survivors dead
- one survivor remains
- no faction branch
- no Muster approach
- no Verdict
- no major expeditions
- no shelter upgrades beyond baseline
- campaign ended unusually early
- migrated save missing historical details

Sparse endings are not errors if the campaign can legitimately produce them.

For extinction:
- political/social/personal categories may collapse into a dedicated primary extinction ending
- resolver must still produce a valid result
- UI must not expect survivor cards
- journal must remain valid

---

## 34. Legacy Traits

The source includes `legacyTraits`.

Keep them structural and deterministic.

Possible sources:
- ending category
- moral band
- faction branch
- notable achievements
- survivor continuity
- shelter specialization

Do not implement New Game+ consumption here. Produce validated IDs that downstream legacy systems can consume.

Example:
```text
legacy.disciplined_state
legacy.merciful_founders
legacy.wasteland_diplomats
legacy.last_survivor
```

Each trait must have:
- ID
- description/localization
- condition
- source fact(s)
- validation

---

## 35. Achievements and Rewards Isolation

If achievements are later tied to endings:

- emit one `EndingResolved` event containing stable result ID and fact IDs
- achievement system subscribes
- event is de-duplicated by resolution ID
- replay does not fire again
- migration policy states whether old saves retroactively unlock achievements

Do not make the resolver know achievement IDs.

---

## 36. Telemetry / Playtest Instrumentation

If the project's opt-in telemetry system exists, emit non-sensitive structural data:

- primary ending ID
- category fact IDs
- campaign length band
- count of survivor epilogues
- resolution schema version

Avoid sending free-form survivor names or journal prose unless explicitly required and consented.

Use the data to detect:
- unreachable ending templates
- dominant ending branches
- missing personalization coverage
- extinction frequency
- template fallbacks

---

## 37. Observability and Diagnostics

Add structured logs around resolution:

```text
EndingResolutionStarted campaign=<id>
EndingContextCaptured digest=<digest>
EndingCategoryResolved category=Political fact=<id>
EndingTemplateSelected section=<id> template=<id>
EndingResolutionCommitted resolution=<id>
```

On failure:
- category
- missing ID
- template candidate count
- contradiction reason
- schema version

Do not log full private save payloads.

---

## 38. Data Integrity Validation

Extend the existing `--data-integrity-selftest`.

Validate:

- every ending template has unique ID
- every localization key resolves
- every condition field is recognized
- every referenced faction ID resolves
- every survivor-specific template points to a real survivor only when intentionally hard-bound
- every quest ID resolves
- every expedition/location ID resolves
- every art/audio key resolves or is explicitly optional
- all template tokens are valid
- campaign-length bands do not overlap
- precedence tables contain no duplicate rank
- legacy ID mappings are complete
- fallback templates exist for every category

---

## 39. Dedicated `--unified-ending-selftest`

Implement a headless CI verb.

It should:

1. Load all ending catalogs.
2. Build representative fixed contexts.
3. Resolve them.
4. Verify stable result IDs.
5. Verify expected category facts.
6. Verify selected template IDs.
7. Verify no unresolved tokens.
8. Verify survivor ordering.
9. Capture and restore result.
10. Verify replay returns identical result.
11. Verify old-save fixture migration.
12. Verify extinction fixture.
13. Verify no-faction fixture.
14. Verify no-Verdict fixture.
15. Verify all-template coverage report.
16. Exit non-zero on any failure.

---

## 40. Golden Ending Fixtures

Create a compact corpus under test data, e.g.:

`Ashfall.Core.Tests/Fixtures/Endgame/`

Minimum golden contexts:

1. Military + Schedule + positive moral + several survivors alive.
2. Rebel + DarkRoad + negative moral.
3. Independent + Tender + mixed standings.
4. PRPF + Reserve + Verdict present.
5. White ending.
6. No Holdfast + no faction.
7. All survivors dead.
8. One survivor alive.
9. Muster-only strong social outcome.
10. Verdict absent.
11. Very long campaign.
12. Very short campaign.
13. Migrated legacy save.
14. Contradictory legacy state.
15. Template fallback path.
16. Every Muster approach at least once across corpus.
17. Every Holdfast ending at least once.
18. Every moral band at least once.

Use stable IDs, not English prose, as primary assertions.

---

## 41. Unit Test Matrix

### Resolver tests

- context -> political fact
- context -> social fact
- context -> moral fact
- context -> judicial fact
- survivor ranking
- contradiction rejection
- fallback behavior
- deterministic ordering

### Template selector tests

- specificity beats generic
- priority tie-break
- stable-ID final tie-break
- exclusions
- missing fallback fails validation
- condition mismatch rejects template

### Persistence tests

- capture/restore
- schema migration
- one-shot latch
- result replay
- no duplicate journal entry

### UI model tests

- no survivors
- many survivors
- missing optional art
- long text
- reduced motion
- keyboard navigation

### Export tests

- stable summary
- no private fields

---

## 42. Property and Fuzz Testing

Generate valid randomized contexts from catalog IDs.

Properties:

- resolution never throws for valid context
- all output IDs resolve
- same input twice => same result
- survivor output has no duplicates
- each category emits at most one primary fact
- no template token remains unresolved
- one-shot replay is stable
- invalid contradictory contexts fail with explicit reason rather than arbitrary resolution

Keep random seeds fixed in CI and optionally sweep additional seeds nightly.

---

## 43. Performance Budget

Endgame resolution is infrequent, but unbounded template scanning can still become pathological.

Targets:

- context collection: bounded by final-state ledgers, no full save serialization if avoidable
- template matching: pre-index by category and common condition keys
- no repeated disk parsing during one resolution
- no scene loading in domain resolution
- no large allocations proportional to total authored narrative corpus

Add a benchmark if the template catalog becomes large.

---

## 44. Content Coverage Report

Generate:
`docs/endgame/EPILOGUE_COVERAGE.md`

Include:

| Dimension | Values | Covered by template? | Covered by fixture? |
|---|---|---:|---:|
| Holdfast | 5 | yes/no | yes/no |
| Muster | 8 or actual | yes/no | yes/no |
| Moral bands | actual | yes/no | yes/no |
| Verdict | actual | yes/no | yes/no |
| Faction branches | actual | yes/no | yes/no |
| Campaign length | bands | yes/no | yes/no |

Also report:
- templates never selected by any fixture
- fact IDs with no prose coverage
- templates with zero reachable contexts
- fallback usage count in fixture corpus

This prevents "50 templates" from becoming a vanity count.

---

## 45. Prose Quality Review Pass

After the structural system is green:

1. Export all golden endings to Markdown.
2. Review for repetition.
3. Check contradictory tone across adjacent sections.
4. Check repeated opening phrases.
5. Check pronoun/name ambiguity.
6. Check whether player actions are attributed accurately.
7. Check whether moral judgment overstates the facts.
8. Check whether survivor paragraphs invent unsupported futures.
9. Check punctuation and localization token boundaries.
10. Review endings in actual UI at target text size.

Use authored revision, not algorithmic randomness, to solve repetitiveness.

---

## 46. Ending Composition Coherence Rules

Because independently selected templates can collide, add coherence metadata.

Examples:

- tone: hopeful / bleak / mixed / austere
- temporal frame: immediate / years-later
- governance noun: bunker / settlement / fleet / garrison
- forbidden adjacent tags
- closing strength: terminal / transitional

The composer can reject obviously incompatible combinations and choose the next-ranked template.

Keep this ruleset simple. It should resolve real conflicts, not become a second narrative engine.

---

## 47. UI Snapshot and Regression Coverage

Capture representative ending screens:

- standard multi-section ending
- extinction ending
- long survivor list
- no-survivor-card case
- high text scale
- localized long-string locale if supported
- reduced motion
- missing optional art fallback

Snapshots should assert layout, not pixel-lock dynamic names unless repository snapshot policy already supports it.

---

## 48. Accessibility Requirements

Ending content is a major narrative payoff and must remain accessible.

Requirements:
- keyboard-only navigation
- controller navigation where supported
- text scaling
- no critical information only in color
- reduced-motion mode
- manual pause/advance
- captions for spoken narration
- skip/continue controls
- replay from journal or ending gallery if implemented
- screen-reader-adjacent labels where the UI framework supports them

Add these checks to the same accessibility authority used elsewhere rather than creating one-off ending rules.

---

## 49. Failure Handling

### Missing template

Use validated generic fallback for that category and log diagnostic. CI should catch before release.

### Missing optional media

Render text without media.

### Invalid required ID

Fail resolution before commit and surface developer diagnostic; production UI may show a safe generic error and
allow retry after reload if appropriate.

### Save write failure

Do not mark ending resolved.

### Localization failure

Fallback according to project localization policy, but preserve structural result.

---

## 50. Security and Data Hygiene

The ending system handles large campaign state but should only consume whitelisted data.

- no reflection over arbitrary save objects
- no executable expressions in templates
- no file paths in player-facing export
- no user-entered text injected without escaping
- no arbitrary rich-text tags from data unless sanitized
- no telemetry of raw prose by default

---

## 51. Implementation Phase A — Reconnaissance and Contracts

### Tasks

1. Audit all ending systems and save contracts.
2. Document actual canonical IDs.
3. Create `UnifiedEndingContext`.
4. Create `EndingFacts`.
5. Create `UnifiedEndingResult`.
6. Create adapters for Holdfast, Muster, Matrix, Faction, Moral, Survivor, Verdict.
7. Add pure resolver skeleton.
8. Add deterministic digest.
9. Add empty save state with schema version.
10. Add unit tests for DTO equality and digest stability.

### Exit criteria

No UI changes yet. Resolver can collect a context in tests and produce structured placeholder facts deterministically.

---

## 52. Implementation Phase B — Political/Social/Moral/Judicial Resolution

### Tasks

1. Implement precedence tables.
2. Implement political resolution.
3. Implement social resolution.
4. Implement moral resolution.
5. Implement judicial resolution.
6. Add contradiction detection.
7. Add fallbacks.
8. Add matrix shadow comparison.
9. Add one golden fixture per major branch.
10. Generate coverage report.

### Exit criteria

All non-personal categories resolve structurally without prose.

---

## 53. Implementation Phase C — Survivor and World Facts

### Tasks

1. Implement survivor fate adapter.
2. Implement notability scoring.
3. Implement relationship/memorial support where canonical data exists.
4. Implement expedition fact selector.
5. Implement major quest selector.
6. Implement shelter upgrade selector.
7. Implement campaign-length band.
8. Add extinction support.
9. Add sparse-state support.
10. Add deterministic ordering tests.

### Exit criteria

`EndingFacts` fully represents the campaign conclusion before any prose composition.

---

## 54. Implementation Phase D — Template Catalog and Composition

### Tasks

1. Create schema.
2. Author 20 core templates using existing prose where possible.
3. Implement validator.
4. Implement deterministic selector.
5. Implement token resolver.
6. Implement section composer.
7. Add generic fallbacks.
8. Add survivor templates.
9. Add 10+ golden prose-selection tests.
10. Generate template reachability report.

### Exit criteria

Every golden context produces coherent localized template IDs and no unresolved tokens.

---

## 55. Implementation Phase E — Persistence, Migration, Journal

### Tasks

1. Persist unified result.
2. Add one-shot latch.
3. Add capture/restore.
4. Add legacy save migration.
5. Create migration matrix.
6. Wire journal.
7. Add replay.
8. Add duplicate suppression.
9. Add save round-trip tests.
10. Add migrated-save fixtures.

### Exit criteria

Resolved ending survives reload exactly and never re-rolls.

---

## 56. Implementation Phase F — UI, Art, Audio, Export

### Tasks

1. Create presentation DTO.
2. Wire ending screen to unified result.
3. Add survivor cards.
4. Add category sections.
5. Add key moments.
6. Add optional audio cues.
7. Add optional art keys.
8. Add accessibility.
9. Add ending summary export.
10. Add UI snapshots.

### Exit criteria

The player sees only the unified result; no UI component queries legacy ending systems directly.

---

## 57. Implementation Phase G — CI, Coverage, Cleanup

### Tasks

1. Implement `--unified-ending-selftest`.
2. Extend data integrity.
3. Add golden fixtures.
4. Add fuzz tests.
5. Add coverage report.
6. Add claims/unused-field audit.
7. Run shadow comparison against legacy matrix.
8. Remove dead duplicate UI paths.
9. Deprecate legacy resolver entry points.
10. Document final architecture.

### Exit criteria

CI proves determinism, coverage, migration, and catalog validity.

---

## 58. Exact File Plan

Expected new or modified files, adjusted to repository reality:

### Core

- `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs`
- `Assets/Ashfall.Core/Endgame/UnifiedEndingContext.cs`
- `Assets/Ashfall.Core/Endgame/UnifiedEndingResult.cs`
- `Assets/Ashfall.Core/Endgame/EndingFacts.cs`
- `Assets/Ashfall.Core/Endgame/PersonalizedEpilogueGenerator.cs`
- `Assets/Ashfall.Core/Endgame/EndingContextCollector.cs`
- `Assets/Ashfall.Core/Endgame/EndingTemplateCatalog.cs`
- `Assets/Ashfall.Core/Endgame/EndingTemplateSelector.cs`

### Adapters

- `.../Endgame/Adapters/HoldfastEndingAdapter.cs`
- `.../Endgame/Adapters/MusterEndingAdapter.cs`
- `.../Endgame/Adapters/EpilogueMatrixAdapter.cs`
- `.../Endgame/Adapters/FactionEndingAdapter.cs`
- `.../Endgame/Adapters/MoralEndingAdapter.cs`
- `.../Endgame/Adapters/SurvivorEndingAdapter.cs`
- `.../Endgame/Adapters/VerdictEndingAdapter.cs`

### Data

- `Assets/StreamingAssets/Data/endgame_epilogue_templates.json`
- `Assets/StreamingAssets/Data/endgame_resolution_rules.json`
- localization entries
- optional audio/art cue mappings

### Tests

- `Ashfall.Core.Tests/Endgame/UnifiedEndingResolverTests.cs`
- `Ashfall.Core.Tests/Endgame/EndingTemplateSelectorTests.cs`
- `Ashfall.Core.Tests/Endgame/EndingMigrationTests.cs`
- `Ashfall.Core.Tests/Endgame/UnifiedEndingPersistenceTests.cs`
- `Ashfall.Core.Tests/Endgame/UnifiedEndingFuzzTests.cs`
- fixture JSON under test data

### Docs

- `docs/endgame/ENDING_INTEGRATION_AUDIT.md`
- `docs/endgame/ENDING_SAVE_MIGRATION_MATRIX.md`
- `docs/endgame/EPILOGUE_COVERAGE.md`
- `docs/endgame/ENDING_ARCHITECTURE.md`

---

## 59. Bootstrap / Composition Root Wiring

Use the repository's composition-root conventions.

Target sequence:

```text
load catalogs
construct upstream systems
construct ending adapters
construct ending context collector
construct unified ending resolver
construct epilogue generator
restore persisted ending state
wire ending trigger
wire journal/presentation sinks
```

Do not create circular dependencies where the resolver owns systems that also depend on endgame output.

Prefer interfaces:
- `IEndingContextSource`
- `IEndingResultStore`
- `IEndingResolvedSink`
- `IEpilogueTemplateCatalog`

Avoid service locator access from prose generation.

---

## 60. Event Contract

Recommended event:

```csharp
public sealed record EndingResolvedEvent(
    string ResolutionId,
    string PrimaryEndingId,
    IReadOnlyList<string> FactIds
);
```

Consumers:
- UI presenter
- journal
- telemetry
- achievements
- legacy/New Game+ handoff

The event fires only after persistence succeeds.

---

## 61. Compatibility With Existing Chronicle Builder

`EpilogueChronicleBuilder` should become a presentation consumer.

Instead of deciding what happened, it receives `UnifiedEndingResult` and maps sections/facts to chronicle slides.

Tasks:
1. Audit current 5-slide schema.
2. Map existing slides to unified facts.
3. Preserve existing slide order where appropriate.
4. Replace placeholder-only assumptions.
5. Ensure chronicle can omit a slide if category absent.
6. Add snapshot tests.

---

## 62. 50-Template Expansion Strategy

Do not author 50 arbitrary variants at once.

Coverage-driven batches:

### Batch 1 — 20 core
- all Holdfast endings
- all major faction branches
- broad moral bands
- Verdict outcomes
- extinction
- generic survivor alive/dead/retired

### Batch 2 — 15 combination refinements
- faction + Holdfast
- Muster + social
- faction standing nuances
- campaign length
- key shelter/expedition outcomes

### Batch 3 — 15 survivor/world refinements
- leadership
- memorial
- mentorship
- unique quest arcs
- high-salience discoveries

After each batch, regenerate coverage and remove unreachable templates.

---

## 63. Narrative Conflict Examples to Test

Create explicit tests for semantic collisions:

- hopeful political ending + extinction personal state
- high moral band + one notorious ruthless act
- faction alliance + low standing with same faction
- survivor praised as leader but marked deceased before final act
- Muster amnesty + punitive social template
- Verdict "Held" + prose describing release
- long campaign + short-campaign opening
- greenhouse legacy prose when greenhouse never built

The resolver should either compose a coherent nuanced result or reject invalid data. It should never silently lie.

---

## 64. Player Attribution Requirement

For each major epilogue section, track `SupportingFactIds`.

The UI does not have to expose raw IDs, but this enables:
- debugging
- playtest review
- support
- future "why this ending?" developer view
- claims that every sentence traces to campaign state

Add a developer-only ending inspector:
```text
Section: Political
Template: epilogue.political.schedule.military
Facts:
 - holdfast.Schedule
 - faction.Military
```

---

## 65. Content Authoring Guidelines

Authors should write templates that:

- reference concrete choices where possible
- avoid overclaiming unseen consequences
- preserve ASHFALL tone
- avoid repeating the player's name excessively
- avoid universal statements contradicted by other categories
- leave room for neighboring sections
- use stable nouns from localization catalogs
- avoid hardcoding dates unless facts provide them
- avoid promising New Game+ effects not implemented

---

## 66. Release Migration Strategy

Release in controlled stages:

1. Introduce new system behind feature flag or internal branch.
2. Run legacy ending plus new ending in tests.
3. Compare golden campaign saves.
4. Fix gaps.
5. Switch ending UI to new result.
6. Keep legacy compatibility adapter for one release window if needed.
7. Remove or de-authorize legacy UI paths.
8. Retain ID migration maps permanently if old saves depend on them.

---

## 67. Regression Gate Rules

Release gate fails if:

- any endgame catalog validation fails
- `--unified-ending-selftest` fails
- any golden fixture changes without approved baseline update
- a context field becomes unused without explicit annotation
- a template becomes unreachable unexpectedly
- a save migration fixture fails
- ending UI cannot render extinction/no-survivor state
- localization keys are missing for critical ending sections

---

## 68. Suggested Verification Commands

Use actual repository commands, with the source-plan baseline:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --unified-ending-selftest
```

If the repository uses a different engine CLI or wrapper at implementation time, update this section in the
final execution record rather than blindly preserving stale commands.

---

## 69. Definition of Done — Flagship Version

### Architecture
- [ ] One authoritative ending resolver exists.
- [ ] Legacy ending systems are adapters/inputs, not competing final authorities.
- [ ] Structured facts are separated from prose.
- [ ] Resolver is deterministic and headless.

### State
- [ ] Context capture is frozen.
- [ ] Result persists with schema version.
- [ ] Replay reads persisted result.
- [ ] One-shot resolution enforced.
- [ ] Old saves migrate safely.

### Personalization
- [ ] Political category reflects Holdfast/faction state.
- [ ] Social category reflects Muster/standing/community state.
- [ ] Moral category reflects band plus specific choices.
- [ ] Judicial category reflects Verdict when present.
- [ ] Personal category reflects ranked survivor fates.
- [ ] Major expeditions/quests/upgrades can appear when relevant.
- [ ] Campaign length affects framing.
- [ ] Previously dead context fields are used or removed intentionally.

### Content
- [ ] At least 20 core templates ship before expansion.
- [ ] 50-template target reached only with reachability evidence.
- [ ] All template IDs/localization keys validate.
- [ ] No runtime LLM generation.
- [ ] Fallbacks exist.

### Presentation
- [ ] Ending screen uses unified result.
- [ ] Journal stores unified result.
- [ ] Replay works.
- [ ] Export works if enabled.
- [ ] Optional art/audio cannot break ending.
- [ ] Accessibility requirements pass.

### Tests
- [ ] Unit tests.
- [ ] Golden contexts.
- [ ] Save migration fixtures.
- [ ] Fuzz/property tests.
- [ ] UI snapshots.
- [ ] `--unified-ending-selftest`.
- [ ] data-integrity checks.
- [ ] deterministic digest checks.

### Documentation
- [ ] architecture audit
- [ ] migration matrix
- [ ] coverage report
- [ ] authoring guidelines
- [ ] final implementation notes

---

## 70. Next Substeps Immediately After Plan 145

These are integration-derived tasks that should follow once the unified ending system is green.

### Task 145-D — Ending Coverage and Reachability Auditor

Create a generated report that enumerates all reachable major ending combinations from current catalogs and
compares them against authored template coverage.

Substeps:
1. Enumerate branch domains.
2. Exclude impossible combinations using resolver constraints.
3. Run template selection for each representative state.
4. Count fallback usage.
5. Flag unreachable templates.
6. Flag reachable facts with no specific prose.
7. Publish `EPILOGUE_COVERAGE.md`.
8. Gate regressions.

### Task 145-E — Legacy Save Ending Corpus

Create a fixture corpus from representative historical save schemas.

Substeps:
1. Collect anonymized fixture saves.
2. Version them.
3. Migrate each.
4. Resolve endings headlessly.
5. Assert no exceptions.
6. Assert known legacy IDs map correctly.
7. Add to CI nightly if expensive.

### Task 145-F — Epilogue Attribution Inspector

Create developer tooling showing each rendered section's supporting facts.

Substeps:
1. Add inspector DTO.
2. Show template ID.
3. Show supporting fact IDs.
4. Show source system.
5. Show selection priority.
6. Add no-player-build guard if developer-only.

### Task 145-G — Ending Content Authoring Pipeline

Provide validation/editor tooling for writers.

Substeps:
1. schema validation
2. localization key validation
3. preview against fixture contexts
4. template reachability check
5. duplicate semantic tag warning
6. export all endings for review

### Task 145-H — Ending Gallery / Replay Authority

If an ending gallery is desired, build it on `UnifiedEndingResult` identities.

Substeps:
1. store unlocked structural ending IDs
2. no spoiler leakage before unlock
3. replay only resolved or intentionally unlocked canonical examples
4. never re-resolve past campaigns
5. support localization changes safely

---

## 71. Longer-Term Follow-On Opportunities

### Ending achievements

Attach to stable fact IDs and de-duplicate by resolution ID.

### Ending art gallery

Unlock registered art assets from actually achieved endings.

### Ending soundtrack album

Unlock cue keys associated with achieved endings; keep entitlement logic outside resolver.

### New Game+ legacy

Consume `legacyTraitIds`; do not back-wire New Game+ mechanics into epilogue resolution.

### Cross-campaign chronicle

Aggregate final structured facts across campaigns, enabling a historical archive without parsing prose.

### Playtest-driven prose refinement

Use human sessions to identify:
- generic-feeling endings
- missing attribution
- repeated phrasing
- confusing category transitions
- survivors players expected to see but did not

Improve authored coverage, not runtime randomness.

---

## 72. Execution Checklist for an Implementation Agent

### Pass 0 — Read only
- [ ] inspect all named files
- [ ] locate actual save and bootstrap contracts
- [ ] list existing ending IDs
- [ ] identify every UI consumer
- [ ] record baseline tests

### Pass 1 — Contracts
- [ ] add context/result/fact DTOs
- [ ] add adapters
- [ ] add pure resolver skeleton
- [ ] add digest

### Pass 2 — Structural resolution
- [ ] political
- [ ] social
- [ ] moral
- [ ] judicial
- [ ] personal
- [ ] world facts
- [ ] contradiction rules

### Pass 3 — Persistence
- [ ] capture/restore
- [ ] one-shot latch
- [ ] migration
- [ ] legacy fixtures

### Pass 4 — Prose
- [ ] catalog
- [ ] selector
- [ ] tokens
- [ ] 20 core templates
- [ ] coverage report

### Pass 5 — Presentation
- [ ] UI
- [ ] journal
- [ ] replay
- [ ] export
- [ ] art/audio hooks
- [ ] accessibility

### Pass 6 — Hardening
- [ ] selftest
- [ ] data integrity
- [ ] fuzz
- [ ] snapshots
- [ ] baseline comparisons
- [ ] docs

### Pass 7 — Expansion
- [ ] grow to 50 templates based on uncovered reachable states
- [ ] prose review
- [ ] remove deprecated duplicate ending paths

---

## 73. Final Guardrails

- No competing ending authority after cutover.
- No RNG in resolution or template selection.
- No live reads from UI into campaign systems.
- No regenerated ending on replay.
- No unvalidated ID references.
- No missing-data fabrication in migrated saves.
- No prose that names a choice without a supporting fact.
- No runtime LLM-generated epilogue.
- No all-survivors-dead crash.
- No ending rewards triggered twice.
- No save marked resolved before the result is committed.
- No "50 templates" target achieved through unreachable filler.
- No silent unused context fields.
- No art/audio requirement that can block text completion.
- No legacy resolver deletion until compatibility behavior is understood.

The unified ending should be treated as the final compression of the campaign's state: one canonical, deterministic,
inspectable artifact that answers what happened, why it happened, who survived, what the shelter became, what the
player's choices meant, and what legacy the campaign leaves behind.

When this plan is complete, the ending is no longer twelve generic paragraphs at the edge of several disconnected
systems. It becomes a tested endgame product surface whose claims are traceable to the actual campaign the player
just finished.
