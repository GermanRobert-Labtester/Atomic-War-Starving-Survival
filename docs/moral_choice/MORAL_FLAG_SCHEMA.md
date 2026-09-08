# Moral Flag Schema

## Catalog authority

`Assets/StreamingAssets/Data/moral_choice_flags.json` is a root object:

```json
{
  "schema_version": 1,
  "description": "...",
  "flags": [
    { "id": "flag_example", "display_name": "Example" }
  ]
}
```

Each flag record has exactly the live fields:

- `id`: stable lowercase `flag_` wire identifier.
- `display_name`: player-facing historical label.

The final catalog contains 25 records. No per-flag metadata was added.

## Producer extension

Moral-choice option records now also accept the optional existing-style `set_flag` field:

```json
{
  "label": "...",
  "moral_delta": 0,
  "empathy_delta": 0,
  "set_flag": "flag_shared_rations",
  "outcome_text": "...",
  "epitaph": "..."
}
```

`set_flag` is absent/empty for ordinary options. It is mapped to `MoralChoiceOption.SetFlag` and written only after a resolution is committed. The flag remains a boolean historical fact; it does not become a second score or save section.
