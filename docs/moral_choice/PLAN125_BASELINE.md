# Plan 125 Baseline

Date: 2026-09-08

## Verified repository state

- `moral_choice_flags.json` contained 10 records.
- `MoralChoiceFlagCatalogLoader` deserialized `schema_version`, `description`, and `flags` records with `id` and `display_name`; it did not validate uniqueness or maintain a separate whitelist.
- `MoralChoiceFlagDefinitions` is a plain DTO (`List<MoralFlagDefinition>`), not generated code and not a static ID registry.
- `MoralChoiceState.activeFlags` is the persistent historical set inside the existing `moral_choice` save section.
- `MoralChoiceSystem.SetFlag` is idempotent for the state list; `RestoreState` replays the saved flags into the shared ledger.
- Moral-choice options had no flag producer field. The implementation therefore adds the optional `set_flag` wire field and maps it through the three moral-choice loaders before `Resolve` commits the flag.

## Baseline gates

- `godot --headless --path . -- --data-integrity-selftest`: PASS, 0 errors / 0 warnings across 298 catalogs.
- Initial targeted `dotnet test ... --filter FullyQualifiedName~MoralChoice` was blocked by the sandbox's local test-runner socket permission; the same suite was rerun with approved host permissions after implementation and passed.

## Scope decision

The live echo, point-of-no-return, faction-reaction, and gossip schemas do not accept flag predicates. No unsupported fields were inserted into those catalogs. Their requested Plan 125 consumer mappings are documented as staged handoffs rather than falsely claimed as live integrations.
