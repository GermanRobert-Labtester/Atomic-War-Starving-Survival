# Crossing Encounter and Crisis Runtime Contract

## Authority

- Catalog data: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Loader and DTOs: `Assets/Ashfall.Core/CrossingCatalog.cs`
- Session: `Assets/Ashfall.Core/CrossingSession.cs`
- Existing political system: `Assets/Ashfall.Core/CrossingArbitrationSystem.cs`
- Existing vouch persistence: the Crossing vouch state inside the expansion save aggregate

## Repository reconciliation

The plan brief described a 10-encounter baseline. The repository currently
contains **14 encounters and 5 crises**. Plan 115 preserves all existing
records and adds 11 encounters and 7 crises, producing the requested final
counts of **25 encounters and 12 crises**. Existing IDs are unchanged.

## Catalog root

The file is a wrapped object:

```json
{
  "schema_version": 1,
  "encounters": [],
  "crises": []
}
```

`CrossingCatalogLoader.LoadEncountersAndCrises` deserializes the wrapper
directly. Null array elements are skipped. The loader does not validate
counts, IDs, locations, threat labels, choice contents, or phase contents.
Duplicate IDs are appended to the catalog; `GetEncounter` and `GetCrisis`
return the first exact ID match.

## Encounter DTO

| Field | Type | Required by DTO | Runtime meaning |
|---|---|---:|---|
| `id` | string | No compiler enforcement | Stable authored identifier. Existing grammar is `enc_nc_*`. |
| `name` | string | No compiler enforcement | Display text. |
| `target_location` | string | No compiler enforcement | Authored location ID. The loader and `CrossingSession` do not resolve it. Plan 115 validates it against `crossing_locations.json`. |
| `description` | string | No compiler enforcement | Display/narrative text. |
| `threat_level` | string | No compiler enforcement | Free-form display label. It is not numeric and has no selection or outcome logic. |
| `choices` | `CrossingChoiceEntry[]` | No compiler enforcement | Authored choice list. The loader preserves it; `CrossingSession` does not select or resolve choices. |

### Choice DTO

| Field | Type | Required by DTO | Runtime meaning |
|---|---|---:|---|
| `text` | string | No compiler enforcement | Display choice text. |
| `cost_items` | string[] | No compiler enforcement | Existing authored item references. The Crossing catalog loader does not consume or deduct them. Plan 115 uses IDs from the authoritative global item catalog or the Crossing item catalog. |
| `result` | string | No compiler enforcement | Display/result prose. It is not an effect key or consequence payload. |

There are no active fields for morale, guilt, standing, flags, location
reveal, resource deltas, next encounter, disease, accord, or treaty outcomes.
Plan 115 adds none.

## Crisis DTO

| Field | Type | Required by DTO | Runtime meaning |
|---|---|---:|---|
| `id` | string | No compiler enforcement | Stable authored identifier. |
| `name` | string | No compiler enforcement | Display text. |
| `phases` | string[] | No compiler enforcement | Ordered authored labels. They are not command tokens in `CrossingSession`. |
| `description` | string | No compiler enforcement | Display/narrative text. |
| `resolution` | string | No compiler enforcement | Resolution prose. It is not a condition expression or action key. |

The seven Plan 115 crises use four causally ordered labels each. The labels
document intended political escalation, but they do not create a new state
machine.

## Session behavior and persistence

`CrossingSession` currently:

- loads the catalog;
- owns `VouchAccessSystem`;
- exposes the vouch gate for `loc_crossing_*` travel;
- grants, burns, or softens vouch access.

It does **not**:

- select encounters;
- resolve encounter choices;
- consume `cost_items`;
- apply `result` text as gameplay;
- select or advance crises;
- interpret phases;
- interpret resolution text;
- persist encounter or crisis IDs/phases/outcomes.

The existing Crossing arbitration system is a separate engine for scripted
Standing/backer disputes. The existing quest system owns Crossing quest
choices and flags. Plan 115 therefore remains a data and narrative pass.
Its crisis resolution prose names existing arbitration, vote, and forfeit
routes without claiming that the new records are wired to those systems.

## External-hook status

- Faction standing: deferred. Encounter/crisis DTOs have no faction or
  standing fields.
- Expedition destination reveal: deferred. No location-reveal field exists.
- Foundry accord/treaty flags: deferred. No flag/effect field exists.
- Disease/quarantine state: deferred. The quarantine crisis remains generic
  and does not invent a disease ID or contagion effect.
- Epilogue history: deferred. No persistent outcome field exists.
