# Phantom Memory Runtime Contract

## Authority

- Data: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Core engine: `Assets/Ashfall.Core/PhantomMemoryEngine.cs`
- DTO: `Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs`
- Production host: `src/Host/PhantomMemoryHostSession.cs`
- Production composition: `src/Main.Phase0.cs`
- Persistence: `src/Host/PhantomMemorySaveStore.cs`

## Catalog shape

The root is:

```json
{
  "schema_version": 1,
  "items": [
    {
      "background_id": "former_soldier",
      "triggers": [
        {
          "trigger_id": "...",
          "item_category": "military",
          "item_id": "dog_tags",
          "motivation_chance": 0.2,
          "description": "...",
          "motivation_text": "...",
          "breakdown_text": "..."
        }
      ]
    }
  ]
}
```

`PhantomTriggerRuleJson` also supports `affinity_background`,
`affinity_trait`, `lore_only`, `morale_payload`, `guilt_payload`,
`gating_flag`, and `repeatable`. Missing JSON booleans and numbers deserialize
to their C# defaults. The Plan 111 additions use only the existing fields and
omit `repeatable`, retaining the production detailed-loader default of
one-shot behavior.

## Background lookup

`PhantomMemoryHostSession.ProjectSurvivors` resolves a survivor in this order:

1. an `ExpansionEnrichmentCatalog` row's `phantom_background_id`;
2. the production profession mapper;
3. `generic`.

The mapper is case-insensitive and uses ordered substring checks. Enrichment
therefore has authority over profession inference. Plan 111 binds the existing
`expansion_survivor_fields.json` catalog during `Main.SetupPhantom`; it does
not create a new registry or save field.

`PhantomMemoryEngine` then looks up the exact background key using a
case-insensitive dictionary. If that background has no matching item rule, it
tries `generic`. A matching specific rule does not supplement generic rules.
An unknown background remains safe because it falls through to `generic`.

## Item matching

`GetCategoryFromId` infers one of:

```text
childhood
photograph
correspondence
personal_item
military
medical
work_tool
ordinary_object
generic
```

Rules match when either `item_id` matches exactly (case-insensitive) or
`item_category` matches the inferred category. The engine returns all matching
rules in catalog order, then evaluates the first eligible rule. Plan 111 uses
the existing category vocabulary and existing item IDs only.

## Chance and determinism

The engine uses the injected `ISeededRng`:

```text
adjustedChance = 0.15 × (1 + triggersExperienced × 0.10)
roll = rng.NextFloat()
```

The trigger is suppressed when `roll >= adjustedChance`. Otherwise the same
roll selects motivation when:

```text
roll < motivation_chance × adjustedChance
```

An affinity trait, when authored, adds 0.20 to `motivation_chance`, capped at
1.0. Plan 111 adds no affinity traits and no RNG behavior.

## Repeat and persistence

Catalog rules loaded through `PhantomMemoryHostSession` use detailed rule
registration. Because the new records omit `repeatable`, they are one-shot.
The record stores:

- trigger count;
- motivation boost hours;
- breakdown refusal hours;
- triggered item IDs;
- triggered trigger IDs.

`CaptureState` sorts survivor records by ordinal survivor ID and deep-copies
lists. `RestoreState` rejects duplicate survivor IDs without mutating live
state. `PhantomMemorySaveStore` persists the unchanged checksummed Core state;
Plan 111 adds no save fields and requires no migration.

## Consequences and cross-plan boundaries

Motivation sets the existing eight-hour work-efficiency timer and emits
`OnPhantomMemoryResolved` with a morale delta. Breakdown sets the existing
four-hour refusal timer and emits the same event with morale and guilt
payloads.

The live Godot `Main.Phase0` consumer currently applies the morale delta. It
does not route `guiltDelta` to `GuiltInsomniaSystem`, and no current phantom
contract exposes a skill-bonus, journal, confession, or echo-quest command.
Those integrations remain deferred rather than being invented under Plan 111.

## Text contract

`ResolveTriggerText` replaces the supported `{name}` placeholder and falls
back to `Someone` for an empty display name. Text is authored as raw English
prose. The Plan 111 additions use present-tense witnessed actions and keep
the existing single-line JSON string format.
