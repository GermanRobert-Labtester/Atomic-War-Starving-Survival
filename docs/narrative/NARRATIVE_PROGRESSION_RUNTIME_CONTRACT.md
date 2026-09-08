# Narrative Progression Runtime Contract

Verified 2026-09 from active source.

## Consumer

**`src/Host/EventsHostSession.cs`** (`EventsHostSession`, a Godot `Node`) is the sole runtime consumer of `narrative_progression.json`.

- Loads `res://Assets/StreamingAssets/Data/narrative_progression.json` in `_Ready()` via `LoadNarrativeProgression()`.
- Deserializes into `NarrativeRoot { SchemaVersion, Entries: List<NarrativeEntryData> }`.
- `NarrativeEntryData` has exactly two properties: `Description` (string), `Order` (int).
- Exposes `GetNarrativeProgression()` → `List<NarrativeEntry>` (same two fields).

## UI Consumer

**`src/UI/EventsLogPanel.cs`** renders every entry as a static Label, sorted by `Order`, on every panel refresh. **All entries are always displayed** — there is no "current chapter" filter.

**`src/UI/EventDetailPanel.cs`** also reads `GetNarrativeProgression()`.

## Trigger Model

**Model E — hardcoded progression / display-only metadata.**

- There are **no trigger fields** in the schema: no `trigger_day`, no `trigger_flag`, no `phase`, no `world_state_changes`, no `on_enter`.
- There is no chapter-advancement runtime: no day-threshold evaluator, no flag condition, no story-event hook, no transition event, no callback.
- "Current chapter" does not exist as runtime state. The Complete/Active/Pending status embedded in each description string is **static authored text**, not runtime state.
- Chapter status prefixes (e.g. "Chapter 3 Active:") are baked into the description strings and displayed verbatim.

## Save Model

**No save state.** `EventsHostSession` "intentionally has no save state" (per its own doc comment). Nothing about progression is persisted; nothing is derived after load. The `HostEventSaveStore` mentioned in the session doc comment belongs to dynamic trigger progress (HostEventAdapter), not to chapters.

## Transition Events

**None.** Chapter "transitions" do not exist as a runtime concept. There is nothing for incidents (Plan 57), territory (Plan 44), or seasons (19C) to subscribe to at the progression layer.

## Localization

Raw English strings. Titles are embedded in the description text (`"Chapter N <Status>: Title — text"`), not separate fields or keys.

## Catalog Integrity

`CatalogIntegrityRules.cs` / `CatalogIntegrityValidator.cs` contain no special-case rules for `narrative_progression.json`. It is validated as a generic root-object catalog (requires top-level `schema_version`). Entries have no `id` fields, so no definition-ID validation applies.

## Content Utilization

`ContentUtilizationScanner.cs` maps the file (by filename) to `NarrativeEncounterSystem` / `NarrativePanel` consumers. The mapping is file-level; record count does not affect scanner behavior.

## Conclusion for Plan 74

Completion mode: **COMPLETE — narrative spine only.** The catalog is display/order metadata. Ten new chapter entries are safe pure data. No trigger, phase, or effect fields may be added (they would be silently ignored dead data requiring Core changes to become live). Cross-system transition effects remain owned by existing/future incident, territory, season, economy, guilt, warlord, rebuilding, and epilogue integrations.