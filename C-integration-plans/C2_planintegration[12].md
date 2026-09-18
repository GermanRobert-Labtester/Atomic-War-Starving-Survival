# C2 — Flagship Integration Plan [12]: Difficulty Authority, Immutable Campaign Completion Record, and Chronicle/Epilogue Projection

> **Deliverable:** `C2_planintegration[12].md`
> **Source scope:** Plan 34 — *The Long Arc: Difficulty & Campaign Record*
> **Primary objective:** make difficulty a first-class, data-driven campaign setting and produce one deterministic, immutable campaign-completion record that becomes the sole factual input for chronicle/epilogue projection and the read-only handoff to future meta progression.
> **Execution order:** **34A → 34B → 34C**
> **Scope boundary:** Plan 149 owns achievements and achievement-panel projection. Plan 175 owns cross-campaign player profile, unlock economy, prestige, and New Game+. C2[12] must not create or partially implement those systems.
> **Non-goals:** no `AchievementSystem`, no `achievements.json`, no meta wallet, no prestige currency, no cross-campaign reward calculations, no New Game+ modifier engine, no second campaign summary, no second cross-campaign save store.
> **Core principle:** difficulty modifies existing authoritative systems; completion records capture already-persisted facts; chronicle and epilogue render those facts without recomputing mutable live state.

> **Wave 11 B3 authority revision (2026-09-18, user-authorized):** C2[12]'s
> completion-history slice may persist an append-only, checksum-validated history
> in a named **user-level completion-history store**. It remains observation-only:
> records derive from the canonical ending/epilogue context, do not calculate
> endings, rewards, unlocks, or New Game+, and never become a campaign-save
> section. This narrowly supersedes the historical cross-campaign-store
> prohibition in §1.3/§3.9; Plan 175 still owns profile, reward, prestige, and
> New Game+ semantics.

---

# 0. Executive Intent

ASHFALL needs a stable answer to two end-to-end campaign questions:

```text
What rules did this campaign run under?
What actually happened in this campaign?
```

Today, those concepts risk being fragmented across:

- per-system save sections,
- difficulty-like hidden/default tuning values,
- ending/epilogue consumers,
- survivor/death records,
- faction standing,
- future achievement evaluation,
- future meta-progression logic.

C2[12] establishes two narrow authorities.

First:

```text
DifficultyPreset
```

is selected once at campaign creation and persists as immutable campaign configuration.

Second:

```text
CampaignCompletionRecord
```

is constructed exactly once when the campaign reaches a terminal outcome, using facts already persisted by existing systems.

The record is:

- deterministic,
- presentation-free,
- checksummed,
- immutable,
- save-compatible,
- readable after completion,
- safe to hand to Plan 175.

The final architecture should be:

```text
difficulty_presets.json
        │
        ▼
DifficultyCatalog / DifficultyPreset
        │
        ▼
CampaignSettings
        │
        ├─ needs modifiers
        ├─ weather-pressure modifiers
        ├─ economy modifiers
        ├─ combat modifiers
        └─ save-rule modifiers
        │
        ▼
existing authoritative systems
        │
        ▼
campaign state
        │
        ▼
terminal campaign transition
        │
        ▼
CampaignCompletionRecordBuilder
        │
        ├─ final day
        ├─ difficulty id
        ├─ survivors/deaths
        ├─ ending id
        ├─ faction standing bands
        └─ selected terminal subsystem facts
        │
        ▼
checksummed immutable completion record
        │
        ├──────────────► chronicle
        ├──────────────► epilogue
        └──────────────► Plan 175 read-only handoff
```

The flagship outcome is:

> **A completed campaign has one frozen factual record of how it ended and under which difficulty rules it was played; every ending/chronicle consumer reads that same record, while achievements and meta rewards remain separate concerns.**

---

# 1. Scope Boundary Contract

This plan must preserve clean ownership boundaries.

## 1.1 Plan 34 owns

- difficulty preset definitions,
- selected campaign difficulty,
- difficulty persistence,
- completion record DTO,
- completion record builder,
- completion record persistence/checksum,
- chronicle projection,
- epilogue projection,
- read-only handoff interface.

## 1.2 Plan 149 owns

- achievement catalog,
- achievement criteria,
- in-campaign achievement evaluation,
- achievement unlock state,
- achievement panel projection.

Plan 34 may expose completion facts that Plan 149 or later systems can read, but it must not evaluate achievements.

## 1.3 Plan 175 owns

- separate player meta profile,
- unlock economy,
- prestige,
- cross-campaign rewards,
- New Game+ progression,
- cross-campaign save store.

Plan 34 may hand Plan 175 a completion record after epilogue resolution.

It may not:

- grant currency,
- mutate profile state,
- unlock content,
- persist a meta profile.

## 1.4 Enforcement

Add architecture tests/docs that fail if Plan 34 introduces references to:

```text
AchievementSystem
achievements.json
MetaProfile
MetaWallet
Prestige
NewGamePlusStore
```

or repository-equivalent future types, except through an explicitly sanctioned read-only interface declared by the owning plan.

---

# 2. Program-Level Success Criteria

C2[12] is complete only when all of these are true.

## 2.1 Difficulty is explicit

Every campaign has one difficulty preset ID.

## 2.2 Difficulty is data-authored

Preset modifiers live in `difficulty_presets.json`.

## 2.3 Difficulty does not fork systems

Existing systems consume bounded modifiers.

No easy/normal/hard variants of core systems.

## 2.4 Difficulty is immutable for a campaign

Once campaign setup is committed, the selected preset cannot silently change.

## 2.5 Completion record is produced exactly once

A terminal campaign transition captures the final record.

## 2.6 Completion record contains only owned facts

No duplicate achievement evaluation.
No reconstructed shadow flag ledger.

## 2.7 Completion record is deterministic

Same terminal campaign state produces byte-stable equivalent record output.

## 2.8 Completion record is immutable

Post-completion live-system mutations cannot change it.

## 2.9 Completion record is checksummed

Its persisted representation participates in campaign integrity.

## 2.10 Epilogue/chronicle are projections

They render the record; they do not build their own parallel summary.

## 2.11 Old completed saves remain valid

A conservative record can be synthesized without changing the original ending.

## 2.12 Plan 175 handoff is read-only

No reward calculation occurs inside this plan.

---

# 3. Architectural Invariants

## 3.1 One difficulty authority

Use one Core contract plus one data catalog.

Do not create:

- `CombatDifficulty`,
- `EconomyDifficulty`,
- `WeatherDifficulty`,
- independent difficulty sliders owned by each system

unless a future plan explicitly adds a composed custom mode.

## 3.2 Presets modify existing knobs

A difficulty preset references bounded modifiers over existing authority inputs.

It does not implement behavior.

## 3.3 Completion record contains stable IDs

No presentation text.

Use:

- `ending_id`,
- `difficulty_id`,
- survivor IDs,
- faction IDs,
- standing-band IDs,
- terminal-state IDs.

## 3.4 Completion facts are captured, not recalculated

If a system already owns a terminal fact, read it.

Do not infer it from current data after completion.

## 3.5 Record building is pure

Given a frozen campaign fact snapshot:

```text
builder(input) → CampaignCompletionRecord
```

with no mutation and no RNG.

## 3.6 Record construction occurs once

No rebuilding every time epilogue opens.

## 3.7 Presentation is localized downstream

Chronicle/epilogue map IDs/facts to Plan 25 keys.

## 3.8 Save compatibility is explicit

New record/preset fields use versioned save discipline.

## 3.9 Completion record is not meta state

It remains part of completed campaign data.

## 3.10 Plan 175 gets a projection/handoff, not ownership

The campaign owns the completion record permanently.

---

# 4. Execution Order

Required sequence:

```text
34A — completion record
  ↓
34B — difficulty authority
  ↓
34C — chronicle/epilogue projection
```

34A first establishes the stable campaign-outcome fact model.

34B then adds difficulty as a fact captured into that record.

34C can safely render only once the record contract is complete.

---

# 5. Baseline Capture

Before implementation, inventory current campaign-ending and tuning ownership.

## 5.1 Ending inventory

Document:

- terminal campaign state owner,
- ending ID owner,
- final day owner,
- survivor/death authority,
- faction standing authority,
- existing epilogue inputs,
- existing chronicle/history inputs,
- current completed-save behavior.

## 5.2 Difficulty/tuning inventory

Find current values controlling:

- needs pressure,
- weather pressure,
- economy,
- combat,
- save restrictions/rules.

For every value record:

| Tuning value | Current owner | Current default | Data-authored? | Difficulty candidate? |
|---|---|---:|---:|---:|

Do not move tuning ownership merely to centralize it.

## 5.3 Verification baseline

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Also capture known completed-save fixtures if available.

---

# 6. Workstream 34A — Deterministic Campaign Completion Record

## 6.1 Objective

Create one immutable factual record of a completed run using only already-authoritative campaign facts.

---

# 7. 34A Phase A — Define the DTO

Create an engine-free Core DTO.

Conceptual shape:

```csharp
CampaignCompletionRecord
{
    schema_version
    campaign_id?
    completed_day
    difficulty_preset_id
    ending_id
    survivors[]
    deaths[]
    faction_outcomes[]
    terminal_system_states[]
    completion_checksum?
}
```

Use repository conventions.

## 7.1 Avoid presentation text

Do not store:

- "You survived the winter."
- survivor display names,
- faction prose,
- localized descriptions.

Store IDs/facts.

---

# 8. 34A Phase B — Field Selection Rule

Every field must satisfy:

```text
already owned by an existing persistent system
AND
needed for ending/chronicle/meta handoff
AND
stable enough to become completion history
```

Reject fields that are:

- ephemeral UI state,
- derived achievements,
- current localization text,
- debug metrics,
- duplicate raw save payloads.

---

# 9. 34A Phase C — Survivor Outcome Model

Record enough stable survivor facts to describe the run.

Potential fields:

- survivor ID,
- alive/dead,
- death day,
- death/fate ID,
- final role/status if already canonical.

Do not copy entire survivor save objects.

---

# 10. 34A Phase D — Faction Outcome Model

Record:

- faction ID,
- final standing band,
- optional final dominance/control status where already authoritative.

Prefer standing-band IDs over arbitrary presentation strings.

Do not re-run faction logic.

---

# 11. 34A Phase E — Terminal System Facts

Define a narrow registry/list of system terminal summaries.

Examples may include:

- shelter terminal state ID,
- power outcome ID,
- final war/dominance state,
- ending-relevant world state.

Each terminal fact must have an owner.

Avoid:

```text
Dictionary<string, object>
```

with unbounded arbitrary payloads.

Prefer typed/stable records.

---

# 12. 34A Phase F — Pure Builder Input

Create an explicit builder input snapshot.

Conceptual:

```csharp
CampaignCompletionFacts
```

This is assembled from authorities at terminal transition.

Builder:

```csharp
CampaignCompletionRecord Build(CampaignCompletionFacts facts)
```

No host calls inside builder.

---

# 13. 34A Phase G — Terminal Transition Ownership

Identify one canonical campaign-end transition.

At that exact transition:

1. freeze end-state facts,
2. build record,
3. validate,
4. persist,
5. checksum,
6. mark immutable.

Do not generate from epilogue UI open.

---

# 14. 34A Phase H — Exactly-Once Guard

If campaign completion is triggered twice:

```text
existing valid completion record
→ return/read existing record
```

Do not overwrite unless explicit migration/recovery mode.

Add test.

---

# 15. 34A Phase I — Immutability

After record creation:

- expose read-only object,
- no mutation methods,
- persisted record treated as append/finalized state.

If DTO serializer requires setters, enforce immutability at domain layer.

---

# 16. 34A Phase J — Checksum Integration

Integrate record bytes into campaign checksum/save integrity.

Pin serialization order.

Rules:

- stable property order,
- stable list ordering,
- invariant culture,
- no dictionary-order dependence.

---

# 17. 34A Phase K — Byte Stability

Create tests:

```text
same facts
→ same serialized bytes
```

across repeated runs.

Also:

```text
fact order variations that are semantically unordered
→ canonical sorted record
```

where appropriate.

---

# 18. 34A Phase L — Save Section

Prefer one canonical completion section or existing campaign-terminal section.

Do not create a cross-campaign store.

Document:

- schema version,
- capture,
- restore,
- immutability behavior.

---

# 19. 34A Phase M — Old Completed Save Migration

For an old completed campaign lacking a record:

- preserve existing ending ID,
- preserve final day,
- preserve known survivor outcomes,
- preserve known faction bands where available,
- use conservative defaults for missing optional facts.

Do not reinterpret the ending using current rules.

---

# 20. 34A Phase N — Read-Only Handoff Interface

Create a narrow interface such as:

```csharp
ICampaignCompletionRecordSource
{
    CampaignCompletionRecord? CompletionRecord { get; }
}
```

or:

```csharp
ICompletedCampaignReadModel
```

Plan 175 may consume it later.

No callback to mutate meta state in Plan 34.

---

# 21. 34A Phase O — Completion Record Validation

Validate:

- final day >= 0,
- known difficulty ID or legacy/default marker,
- ending ID valid where registry exists,
- survivor IDs valid,
- no duplicate survivor outcome entries,
- faction IDs valid,
- standing bands valid,
- stable ordering.

---

# 22. 34A Tests

- minimal record,
- normal record,
- terminal/extreme record,
- pure builder,
- exactly once,
- immutability,
- save round-trip,
- byte stability,
- checksum changes when fact changes,
- canonical sorting,
- old completed save migration,
- read-only handoff.

---

# 23. 34A Definition of Done

- [ ] Core DTO exists,
- [ ] no presentation text,
- [ ] field ownership documented,
- [ ] pure builder,
- [ ] terminal capture seam,
- [ ] exactly-once guard,
- [ ] immutable read model,
- [ ] checksummed persistence,
- [ ] byte-stability tests,
- [ ] old-save conservative migration,
- [ ] read-only handoff interface,
- [ ] no achievement/meta logic.

---

# 24. Workstream 34B — Difficulty as a Real Data-Driven Axis

## 24.1 Objective

Select one immutable preset at campaign creation and apply bounded modifiers through existing system authorities.

---

# 25. 34B Phase A — Tuning Ownership Inventory

For every proposed modifier, identify the existing system that owns the actual value.

Example categories:

```text
needs
weather pressure
economy
combat
save rules
```

Create:

| Difficulty dimension | Existing authority | Existing knob | Modifier type | Range |
|---|---|---|---|---|

No orphan modifier is allowed.

---

# 26. 34B Phase B — Difficulty Contract

Create an engine-free Core record.

Conceptual:

```csharp
DifficultyPreset
{
    id
    display_key
    description_key?
    needs
    weather
    economy
    combat
    save_rules
}
```

Prefer nested typed modifier records over generic maps.

---

# 27. 34B Phase C — `difficulty_presets.json`

Author:

```text
Assets/StreamingAssets/Data/difficulty_presets.json
```

or canonical data path.

Each preset:

- stable ID,
- localization keys,
- bounded modifiers,
- schema version.

Do not store English display names directly if Plan 25 key layer is available.

---

# 28. 34B Phase D — Preset Count and Naming

Choose deliberate preset set.

Examples conceptually:

- story/easy,
- standard,
- harsh,
- severe.

Do not use these exact names unless project tone approves them.

Avoid excessive preset proliferation.

---

# 29. 34B Phase E — Bounded Modifier Semantics

Every modifier declares valid range.

Examples:

```text
needs_rate_multiplier: 0.5–2.0
weather_pressure_multiplier: 0.5–2.0
economy_scarcity_multiplier: 0.5–2.0
combat_damage_multiplier: 0.5–2.0
save_rule_id: enum-like
```

Actual ranges must reflect system balance.

---

# 30. 34B Phase F — No System Forks

Forbidden:

```text
if difficulty == hard:
    call HardCombatSystem
```

Required:

```text
CombatSystem consumes campaign difficulty modifier
```

Same for economy/weather/needs.

---

# 31. 34B Phase G — Immutable Campaign Setting

Add selected preset ID to campaign settings.

Selection occurs:

```text
new campaign setup
→ choose preset
→ validate
→ freeze
```

Do not allow silent mid-run changes.

If debug/test overrides exist, isolate them.

---

# 32. 34B Phase H — Application Seam

At setup, resolve preset once.

Pass required immutable modifier view to authorities.

Prefer:

- immutable settings object,
- typed modifier interfaces.

Avoid repeated JSON lookups per tick.

---

# 33. 34B Phase I — Needs Integration

Map difficulty needs modifiers into existing needs authority.

Do not duplicate decay rates.

Test:

```text
same seed/actions
different preset
→ expected needs delta only
```

---

# 34. 34B Phase J — Weather Pressure Integration

Apply modifiers to existing weather-pressure/tuning seams.

Do not alter weather RNG in a way that makes difficulty non-deterministic.

A preset may change:

- severity weighting,
- frequency multipliers,
- pressure thresholds

only through existing weather authority.

---

# 35. 34B Phase K — Economy Integration

Apply to existing economy/scarcity tuning.

Do not create a second market formula.

---

# 36. 34B Phase L — Combat Integration

Apply bounded combat modifiers to existing combat calculations.

Avoid changing encounter identity/order simply because difficulty changes unless explicitly designed.

Keep RNG determinism per preset.

---

# 37. 34B Phase M — Save Rules

If difficulty modifies save behavior:

- model as explicit save-rule enum/contract,
- apply through existing save system,
- surface clearly before campaign starts.

Do not hide punitive save rules.

---

# 38. 34B Phase N — Data Validation

Extend integrity validation.

Reject:

- unknown preset IDs,
- duplicate IDs,
- missing display keys,
- out-of-range modifiers,
- unsupported modifier dimensions,
- invalid save rule IDs.

---

# 39. 34B Phase O — Status/Metadata

Surface active difficulty in:

- campaign status,
- save metadata,
- version/debug diagnostics where helpful,
- completion record.

Use ID + localized label.

---

# 40. 34B Phase P — Save/Load

Persist selected preset ID.

On load:

- resolve same preset,
- validate catalog compatibility,
- never silently substitute another preset.

For old saves:

- assign explicit legacy/default preset ID.

---

# 41. 34B Phase Q — Plan 175 Boundary

Plan 175 may later say:

```text
start New Game+ using preset X
```

Plan 34 validates/selects that existing preset.

Plan 175 must not inject a second modifier stack.

Document this interface.

---

# 42. 34B Determinism Tests

For each preset:

```text
same preset + same seed + same actions
→ identical state/checksum
```

Different presets may produce different state, but remain deterministic.

---

# 43. 34B Balance Validation

Run representative seeded campaigns per preset.

Track:

- survival pressure,
- resource runway,
- combat lethality,
- weather exposure,
- economy scarcity.

Ensure presets are distinct but coherent.

---

# 44. 34B Definition of Done

- [ ] tuning inventory complete,
- [ ] Core difficulty contract,
- [ ] JSON preset catalog,
- [ ] bounded modifiers,
- [ ] immutable selection,
- [ ] existing systems consume modifiers,
- [ ] no system forks,
- [ ] save rules explicit,
- [ ] validation,
- [ ] save round-trip,
- [ ] status metadata,
- [ ] completion record includes preset,
- [ ] deterministic per-preset replay,
- [ ] Plan 175 boundary documented.

---

# 45. Workstream 34C — Chronicle and Epilogue Projection

## 45.1 Objective

Render one immutable record into localized post-campaign presentation without recomputing live campaign state.

---

# 46. 34C Phase A — Projection Model

Create a presentation-layer projection.

Input:

```text
CampaignCompletionRecord
```

Output:

```text
localized chronicle entries
epilogue sections
```

Do not add new domain facts during projection.

---

# 47. 34C Phase B — Localization Keys

Map stable record facts to Plan 25 keys.

Examples conceptually:

```text
epilogue.ending.<ending_id>
chronicle.survivor.alive
chronicle.survivor.dead
chronicle.faction.band
chronicle.difficulty
```

Do not build sentences inline.

---

# 48. 34C Phase C — Chronicle Structure

Recommended restrained sections:

1. campaign span,
2. difficulty,
3. survivors,
4. deaths/fates,
5. ending,
6. major faction outcomes,
7. selected terminal-system facts.

Keep it factual and restrained.

---

# 49. 34C Phase D — Epilogue Projection

Epilogue may use authored prose mapped from:

- ending ID,
- major facts,
- survivor outcomes.

But it must read the frozen record.

No querying mutable campaign systems.

---

# 50. 34C Phase E — Post-Completion Viewing

After campaign completion:

- player can reopen chronicle,
- player can reopen epilogue summary,
- values remain identical.

Even if live campaign object remains in memory, projection uses record only.

---

# 51. 34C Phase F — No Recompute

Add test:

1. complete campaign,
2. record created,
3. mutate a live system in test/debug fixture,
4. reopen chronicle.

Expected:

```text
projection unchanged
```

This pins immutability.

---

# 52. 34C Phase G — Plan 175 Handoff Timing

Only after epilogue resolution:

```text
Plan 175 may read record
```

Plan 34 does not call:

- reward grant,
- profile mutation,
- unlock calculation.

Handoff should be explicit and read-only.

---

# 53. 34C Phase H — Minimal Record Rendering

Support sparse/legacy records.

If optional facts unavailable:

- omit section,
- use neutral fallback,
- never invent facts.

---

# 54. 34C Phase I — Normal Record Rendering

Fixture includes:

- several survivors,
- deaths,
- faction bands,
- standard ending,
- difficulty.

Verify ordering and localization.

---

# 55. 34C Phase J — Terminal/Extreme Record Rendering

Fixture includes:

- maximum deaths or terminal failure,
- severe faction outcomes,
- extreme terminal system states.

Verify UI remains readable.

---

# 56. 34C Phase K — Stable Ordering

Sort chronicle items deterministically:

- explicit section order,
- survivor order,
- faction order.

Do not depend on dictionary iteration.

---

# 57. 34C Phase L — Accessibility and Layout

Ensure:

- no color-only outcomes,
- long localized strings wrap,
- keyboard navigation,
- screen-reader labels where existing architecture supports them.

Use Plan 25 pseudo-locale.

---

# 58. 34C Phase M — Snapshot/Headless Tests

Create three fixtures:

- minimal,
- normal,
- terminal.

Test:

- headless projection output,
- UI snapshot where applicable,
- pseudo-locale rendering.

---

# 59. 34C Definition of Done

- [ ] one projection input,
- [ ] localized mapping,
- [ ] chronicle factual structure,
- [ ] epilogue uses record only,
- [ ] post-completion viewing stable,
- [ ] live-state mutation cannot change view,
- [ ] Plan 175 handoff read-only,
- [ ] sparse records render safely,
- [ ] deterministic ordering,
- [ ] accessibility/layout tests,
- [ ] minimal/normal/terminal fixtures pass.

---

# 60. Integrated Campaign-End Pipeline

```text
New Campaign
    │
    ▼
Difficulty preset selection
    │
    ▼
immutable CampaignSettings
    │
    ▼
existing systems consume bounded modifiers
    │
    ▼
campaign runs
    │
    ▼
terminal campaign transition
    │
    ▼
freeze authoritative facts
    │
    ▼
CampaignCompletionRecordBuilder
    │
    ▼
immutable + checksummed completion record
    │
    ├─ save
    ├─ chronicle projection
    ├─ epilogue projection
    └─ Plan 175 read-only handoff
```

---

# 61. Fact Ownership Matrix

At implementation time publish:

| Completion fact | Authority |
|---|---|
| final day | campaign/day authority |
| difficulty ID | CampaignSettings |
| ending ID | ending/terminal authority |
| survivor alive/dead | survivor/fate authority |
| death/fate ID | SurvivorFateSystem or canonical owner |
| faction standing band | FactionStanceEngine / war authority |
| terminal shelter state | canonical shelter authority |
| terminal world state | canonical world authority |

No fact may be captured from UI text.

---

# 62. Completion Record Serialization Contract

Define:

- schema version,
- field order,
- list ordering,
- ID encoding,
- invariant number formatting.

Pin serialized bytes in tests.

---

# 63. Completion Checksum Contract

Clarify whether:

- record is inside existing campaign checksum envelope,
- record has its own digest included in parent checksum,
- both.

Prefer existing save-integrity pattern.

Do not invent a parallel integrity mechanism.

---

# 64. Legacy Completion Record Policy

For completed saves predating Plan 34:

```text
record_source = legacy_migration
```

if metadata field is useful.

Use conservative values.

Never:

- recalculate historical difficulty from current tuning,
- change ending,
- invent achievements,
- invent deaths.

---

# 65. Difficulty Compatibility Policy

If a preset definition changes in a later game version:

Campaign save must preserve enough identity to reproduce/understand the run.

Options:

- preset ID + schema/catalog version,
- preset ID + frozen normalized modifier snapshot.

Choose deliberately.

Recommended:

```text
preset ID
+ difficulty schema version
+ normalized modifier snapshot if presets are allowed to rebalance post-release
```

This prevents old campaigns from silently changing difficulty semantics after patching.

---

# 66. Difficulty Application Contract

Every modifier must answer:

```text
Which authority consumes this?
When is it read?
Is it immutable?
What is the neutral value?
What is the valid range?
```

No unowned modifier.

---

# 67. Neutrality Contract

Define neutral preset semantics:

```text
1.0 multiplier
or
canonical default enum
```

Use this for:

- legacy saves,
- tests,
- baseline comparisons.

---

# 68. Save-Metadata Contract

Save selection UI should expose:

- campaign day,
- difficulty label,
- completion status,
- ending ID/label if completed.

Use existing save metadata surface.

No second save browser.

---

# 69. Read-Only Handoff Contract

Plan 175 handoff must expose:

```text
CampaignCompletionRecord
```

or a read-only interface.

It may not expose mutable campaign systems.

This makes cross-campaign reward calculations reproducible and prevents Plan 175 from scraping live save state.

---

# 70. Achievement Boundary Gate

Add static/architecture check:

C2[12] implementation must not contain:

```text
achievement evaluation
achievement unlock mutation
achievement catalog
```

If achievements need final-run facts, Plan 149 reads the same authorities/record under its own ownership.

---

# 71. Meta Boundary Gate

Add check/documented review:

No Plan 34 file may:

- create a meta save store,
- persist cross-campaign currency,
- unlock NG+ content,
- grant prestige.

---

# 72. Determinism Program

Run paired campaigns.

## Same difficulty

```text
same seed
same actions
same difficulty
→ same completion record bytes
```

## Different difficulty

```text
same seed/actions
different preset
→ deterministic but expectedly different campaign evolution
```

The builder itself has no RNG.

---

# 73. Performance Considerations

Difficulty resolution:

- once at campaign setup,
- typed immutable object,
- no JSON parsing per tick.

Completion builder:

- runs once,
- small data volume.

Chronicle:

- record-sized projection,
- no live-system scans.

Performance should be negligible.

---

# 74. Failure Modes and Corrective Actions

## 74.1 Completion record rebuilt every epilogue open

Fix:

- persist once,
- project existing record.

## 74.2 Record contains localized strings

Fix:

- replace with IDs/keys.

## 74.3 Record evaluates achievements

Scope violation.

Move to Plan 149.

## 74.4 Completion grants meta currency

Scope violation.

Move to Plan 175.

## 74.5 Difficulty creates alternate systems

Fix:

- modifier consumed by existing authority.

## 74.6 Preset changes after load because JSON was rebalanced

Fix:

- preserve preset schema/modifier snapshot policy.

## 74.7 Old save ending changes during migration

Critical.

Fix:

- conservative migration using stored ending.

## 74.8 Chronicle changes after live-state mutation

Critical.

Fix:

- projection reads record only.

## 74.9 Unknown preset silently becomes default

Fix:

- explicit validation/error or legacy migration path.

---

# 75. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| scope creep into achievements | Medium | High | architecture boundary gate |
| scope creep into meta progression | Medium | High | read-only handoff |
| difficulty modifiers duplicate tuning | Medium | High | ownership inventory |
| presets rebalance old campaigns | Medium | High | schema/snapshot policy |
| completion record too large | Low–Med | Medium | narrow fact-selection rule |
| completion record misses epilogue fact | Medium | Medium | consumer inventory before DTO freeze |
| old-save migration changes outcome | Low–Med | Critical | preserve stored ending/facts |
| nondeterministic list ordering | Medium | Medium | canonical sort |
| localized text stored in record | Low | Medium | ID-only tests |
| chronicle becomes second ledger | Medium | High | record-only projection |

---

# 76. Commit Strategy

## Commit C2[12].1 — Baseline + ownership inventory

- end-state authorities,
- difficulty tuning authorities,
- scope boundary docs.

## Commit C2[12].2 — completion DTO + builder input

## Commit C2[12].3 — terminal capture seam

## Commit C2[12].4 — persistence/checksum/immutability

## Commit C2[12].5 — legacy completed-save migration

## Commit C2[12].6 — read-only handoff

### Gate: 34A complete

## Commit C2[12].7 — difficulty contract + JSON schema

## Commit C2[12].8 — catalog loader + validation

## Commit C2[12].9 — immutable campaign settings

## Commit C2[12].10 — needs/weather integration

## Commit C2[12].11 — economy/combat/save-rule integration

## Commit C2[12].12 — metadata/save/determinism/balance

### Gate: 34B complete

## Commit C2[12].13 — chronicle projection

## Commit C2[12].14 — epilogue projection

## Commit C2[12].15 — post-completion stable viewing

## Commit C2[12].16 — Plan 175 read-only handoff timing

## Commit C2[12].17 — minimal/normal/terminal UI + headless tests

### Gate: 34C complete

## Commit C2[12].18 — integrated long-arc closure

---

# 77. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Recommended additional verification:

```text
paired-seed completion-record byte stability
difficulty preset integrity tests
legacy completed-save migration fixtures
pseudo-locale epilogue render
save checksum contract tests
```

---

# 78. Flagship Definition of Done

## 34A — Completion Record

- [ ] small Core DTO,
- [ ] stable IDs only,
- [ ] owned facts only,
- [ ] pure builder,
- [ ] exactly-once terminal capture,
- [ ] immutable record,
- [ ] campaign checksum integration,
- [ ] byte-stability tests,
- [ ] save round-trip,
- [ ] old completed save migration,
- [ ] read-only Plan 175 handoff,
- [ ] no achievement logic,
- [ ] no meta state.

## 34B — Difficulty

- [ ] tuning ownership inventory,
- [ ] `difficulty_presets.json`,
- [ ] Core difficulty contract,
- [ ] bounded modifiers,
- [ ] immutable campaign selection,
- [ ] existing systems consume modifiers,
- [ ] no system forks,
- [ ] save rules explicit,
- [ ] validation,
- [ ] save metadata,
- [ ] completion record includes difficulty,
- [ ] deterministic per-preset runs,
- [ ] old-save default policy,
- [ ] Plan 175 can select existing preset only.

## 34C — Chronicle/Epilogue

- [ ] record-only projection,
- [ ] localization keys,
- [ ] no live-state recompute,
- [ ] post-completion reopen stable,
- [ ] Plan 175 handoff occurs after epilogue,
- [ ] no reward grants,
- [ ] minimal fixture,
- [ ] normal fixture,
- [ ] terminal fixture,
- [ ] deterministic ordering,
- [ ] accessibility/layout pass.

## Scope Boundary

- [ ] no `AchievementSystem`,
- [ ] no `achievements.json`,
- [ ] no meta wallet,
- [ ] no cross-campaign save store,
- [ ] no prestige logic,
- [ ] no NG+ modifier engine,
- [ ] no duplicate campaign summary.

---

# 79. Closure Report Template

```markdown
## C2[12] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Scope Boundary
- Achievement references:
- Meta-profile references:
- Cross-campaign store changes:
- NG+ logic added:
- Boundary result:

### 34A — Completion Record
- DTO:
- Fact owners:
- Builder:
- Terminal capture:
- Exactly-once:
- Serialization:
- Checksum:
- Save section:
- Legacy migration:
- Byte stability:
- Handoff:
- Result:

### 34B — Difficulty
- Preset catalog:
- Preset IDs:
- Difficulty schema:
- Needs modifier:
- Weather modifier:
- Economy modifier:
- Combat modifier:
- Save rule:
- Campaign setting:
- Validation:
- Save metadata:
- Determinism:
- Balance:
- Result:

### 34C — Chronicle/Epilogue
- Chronicle projection:
- Epilogue projection:
- Localization:
- Record-only verification:
- Post-completion reopen:
- Minimal fixture:
- Normal fixture:
- Terminal fixture:
- Plan 175 handoff:
- Result:

### Full Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Completion byte-stability:
- Difficulty integrity:
- Legacy migration:
- Pseudo-locale:

### Final Metrics
- Completion record bytes:
- Completion fields:
- Difficulty presets:
- Invalid modifiers:
- Legacy records synthesized:
- Duplicate campaign summaries:
- Achievement/meta boundary violations:

### Remaining Debt
- Difficulty balance:
- Epilogue content:
- Plan 149 handoff:
- Plan 175 handoff:
```

---

# 80. Final Execution Directive

Execute Plan 34 as a **campaign identity and completion-record** plan, not as an achievement or meta-progression plan.

The critical sequence is:

```text
identify already-owned terminal facts
→ define one immutable completion record
→ capture it exactly once
→ checksum and persist it
→ make difficulty a data-driven immutable campaign setting
→ record the difficulty in the completion record
→ render chronicle/epilogue from that record only
→ expose a read-only record to Plan 175
```

Do not let the completion record become a dump of the whole save.

Do not let difficulty become a parallel simulation layer.

Do not let the epilogue become a second campaign-summary authority.

Do not grant anything cross-campaign here.

The strongest completion rule is:

> **A completed campaign has exactly one immutable factual record, produced once from already-authoritative persisted facts.**

The strongest difficulty rule is:

> **Difficulty selects bounded modifiers over existing systems; it never forks those systems.**

The strongest scope rule is:

> **Plan 34 records what happened. Plan 149 decides achievements. Plan 175 decides what the player earns next.**
