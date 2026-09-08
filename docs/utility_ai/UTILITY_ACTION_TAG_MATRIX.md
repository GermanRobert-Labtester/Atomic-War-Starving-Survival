# Utility Action Tag Matrix

## Recognized Tags (runtime consumers)

| Tag | Constant | Consumer | Effect |
|-----|----------|----------|--------|
| `loud_labor` | `UtilityTags.TagLoudLabor` | `IsForbiddenByTraits` | Coward vetoes |
| `menial_labor` | `UtilityTags.TagMenialLabor` | `IsForbiddenByTraits` | GodComplex vetoes |
| `dirty_labor` | `UtilityTags.TagDirtyLabor` | `ApplyTraitBiases` | Politician 0.6x |
| `weapon` | `UtilityTags.TagWeapon` | `IsForbiddenByTraits` | Pacifist vetoes |
| `gun` | `UtilityTags.TagGun` | `IsForbiddenByTraits` | Blind vetoes |
| `order` | `UtilityTags.TagOrder` | `IsForbiddenByTraits` | ExCon vetoes |
| `medical_triage` | `UtilityTags.TagMedicalTriage` | `IsForbiddenByTraits` | Hitman vetoes; Germaphobe vetoes w/o hazmat |
| `farming` | `UtilityTags.TagFarming` | `IsForbiddenByTraits` | Hitman vetoes |
| `medical` | `UtilityTags.TagMedical` | (none) | Informational / display only |
| `quiet_labor` | (none) | (none) | Informational — used in existing actions |

## Informational Tags (no runtime consumer)

These tags have no veto/bias behavior but are valid in the catalog:
- `quiet_labor` — used by existing `action_audit_inventory`, `action_file_report`

## Tag Usage in Plan 72 (20-action catalog)

| Action ID | Tags | Tag Effect |
|-----------|------|------------|
| `action_weigh_goods` | `loud_labor` | Coward veto |
| `action_read_contract` | — | — |
| `action_canvas_support` | `menial_labor` | GodComplex veto |
| `action_run_vouch` | — | — |
| `action_audit_inventory` | `quiet_labor` | — |
| `action_file_report` | `quiet_labor` | — |
| `action_repair_equipment` | `loud_labor` | Coward veto |
| `action_inspect_housing` | `quiet_labor` | — |
| `action_treat_wounded` | `medical_triage` | Hitman veto; Germaphobe gate |
| `action_seek_treatment` | `medical` | — |
| `action_cook_food` | `quiet_labor` | — |
| `action_preserve_food` | `menial_labor` | GodComplex veto |
| `action_purify_water` | `loud_labor` | Coward veto |
| `action_socialize` | — | — |
| `action_resolve_conflict` | `order` | ExCon veto |
| `action_train_skill` | `quiet_labor` | — |
| `action_teach_skill` | `quiet_labor` | — |
| `action_stand_watch` | `weapon` | Pacifist veto |
| `action_conduct_research` | `quiet_labor` | — |
| `action_rest` | — | — |

## Design Notes

- Tags are primarily for the trait veto/bias matrix
- `quiet_labor` is informational (no consumer) but used for categorization
- `medical` is informational (no consumer) but used for categorization
- Three actions have no tags — they're universally available to all traits
- The veto matrix is intentionally sparse — only 8 pairs in the matrix