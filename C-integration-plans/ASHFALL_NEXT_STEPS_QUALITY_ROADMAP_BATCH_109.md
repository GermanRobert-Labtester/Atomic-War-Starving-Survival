# ASHFALL — Quality Roadmap Batch 109

## Theme: Contextual Help System — In-Game Encyclopedia & Tooltip Database

**Priority:** MEDIUM<br>
**Risk:** Low — additive content/UI layer; no changes to existing gameplay systems. **Correction:** the flag-push-notification sub-piece of Step 6 is not low-risk-trivial — it either requires polling (behavior change: discovery is checked periodically, not instantly) or a new `IFlagLedger` change-notification wrapper (new surface area on a Core port used by many systems). Treat that specific sub-piece as Medium risk until scoped.<br>
**Scope:** `Assets/Ashfall.Core/Help/` (new domain), `Assets/StreamingAssets/Data/help/`, `src/UI/`, `Ashfall.Core.Tests/`<br>
**Estimated effort:** 6–8 focused sessions. **Correction:** this excludes Step 3 (authoring 50 tone-correct entries against the project's "cold, exhausted, human, restrained" voice, each cross-checked for `related_ids` connectivity and character limits) — that step alone is comparable in effort to 1–2 of the code sessions, not a quick data-entry task. Revised estimate: 8–11 sessions, or explicitly split Step 3's authoring into its own follow-up batch if session budget is fixed at 6–8.

---

## Motivation

ASHFALL has 82+ interacting systems with domain-specific terminology that no player will intuitively understand:

- **Medical:** chelation, somatic flashback, dose ledger, respiratory degeneration, potassium iodide
- **Radiation:** dosimeter, geiger counter, rad-away, fallout storm, nuclear winter, sky-layer armor
- **Economy:** brine water, ice roads, holdfast trade sessions, cohort system, waystations
- **Social:** ideological friction, coalition camps, voluntary registers, census claims, muster

The existing `KnowledgeBase` (`Assets/Ashfall.Core/Journal/KnowledgeBase.cs`) tracks player-discovered knowledge keys (a `HashSet<string>` of snake_case ids such as `has_seen_radiation` or dynamically-built `item_seen_<id>` strings via `KnowledgeKeys.ItemSeen(...)`) but provides no explanatory text. **Important:** `KnowledgeBase` is a private instance owned by `JournalSystem` (`_knowledge`, exposed read-only via `JournalSystem.Knowledge`) — it is not a standalone top-level system and is not independently save-registered; it is nested inside `JournalSave.Knowledge`. Players currently have no way to look up what a term means, how a system works, or why something is happening to their survivors.

A contextual help system would:
- Show tooltip explanations on hover/tap over game terms
- Provide an encyclopedia of all discovered concepts (progressively unlocked)
- Link related systems together so players can explore connections
- Integrate with the existing `KnowledgeBase` discovery tracking

This is purely additive — it reads from existing systems but doesn't modify their behavior.

---

## Step 1 — Design Help Content Schema

### Goal
Define the JSON schema for help entries in `Assets/StreamingAssets/Data/help/` following the project's data-authority conventions.

### Implementation
- Create `Assets/StreamingAssets/Data/help/help_entries.json` with schema:
```json
{
  "schema_version": 1,
  "entries": [
    {
      "id": "help_radiation_basics",
      "title": "Radiation Exposure",
      "category": "survival",
      "short_description": "Invisible particles that accumulate in the body, causing progressive organ damage.",
      "long_description": "Radiation is measured in millisieverts (mSv). Your survivors absorb dose from fallout zones, contaminated water, irradiated food, and fallout storms. The dose ledger tracks lifetime accumulation. High doses trigger acute radiation syndrome; chronic low doses cause long-term illness.",
      "related_ids": ["help_dosimeter", "help_fallout_storms", "help_chelation", "help_dose_ledger"],
      "discovery_condition": {
        "type": "flag",
        "flag_id": "flag_first_radiation_exposure"
      },
      "tags": ["radiation", "hazard", "medical"],
      "icon_hint": "radiation_trefoil"
    }
  ]
}
```
- Schema rules:
  - `id` uses `help_` prefix (registered in `CatalogIntegrityValidator` prefix list)
  - `short_description` ≤ 120 characters (fits tooltip width)
  - `long_description` ≤ 800 characters (fits encyclopedia panel)
  - `related_ids` must reference other valid `help_*` IDs (Tier-2 validation)
  - `discovery_condition` types: `flag` (flag must be set), `item_seen` (item in inventory at least once), `day_reached` (day counter threshold), `always` (available from start)
  - `category` enum: `survival`, `medical`, `economy`, `social`, `crafting`, `exploration`, `combat`, `shelter`
- Create `Assets/Ashfall.Core/Help/HelpEntryDefinition.cs` — the C# DTO matching the JSON schema.
- Register `help_` prefix in `CatalogIntegrityValidator` so help IDs are validated.

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles with new DTO.
- `godot --headless --path . -- --data-integrity-selftest` passes with new JSON file (no dangling refs yet — `related_ids` point to entries in the same file).
- JSON is valid, snake_case, has `schema_version`.

### Done when
- [ ] `help_entries.json` exists with valid schema and ≥3 example entries
- [ ] `HelpEntryDefinition.cs` DTO matches JSON structure exactly (field-for-field, including `discovery_condition` as a nested object, not a flat string)
- [ ] `help_` prefix registered in `CatalogIntegrityValidator.IdPrefixes` (confirm by reading the actual list at the top of `CatalogIntegrityValidator.cs` before adding — do not assume the list's exact current contents without checking, since the prefix list may have changed since AGENTS.md was last updated)
- [ ] Data integrity selftest (`godot --headless --path . -- --data-integrity-selftest`) passes with 0 errors, run and observed directly — not assumed
- [ ] Rollback: if `help_` prefix registration causes existing catalogs to fail Tier-1 (unlikely but possible if `help_` collides with an existing informal usage), revert the single line in `CatalogIntegrityValidator.cs` and the new JSON file; no other system depends on this yet

---

## Step 2 — Create HelpDatabase in Core

### Goal
Implement the engine-agnostic `HelpDatabase` system that loads entries, tracks discovery state, and resolves tooltip lookups.

### Implementation
- Create `Assets/Ashfall.Core/Help/HelpDatabase.cs`:
  - Constructor: `HelpDatabase(IFileIO fileIO, IJsonSerializer serializer, ILog log)`
  - `void LoadEntries(string path)` — deserializes `help_entries.json`
  - `HelpEntryDefinition? GetEntry(string helpId)` — lookup by ID
  - `IReadOnlyList<HelpEntryDefinition> GetByCategory(string category)` — filtered view
  - `IReadOnlyList<HelpEntryDefinition> Search(string query)` — case-insensitive substring match on title + tags + descriptions
  - `bool IsDiscovered(string helpId)` — checks if discovery condition is met
  - `void MarkDiscovered(string helpId)` — force-unlock (for tutorial/debug)
  - `IReadOnlyList<HelpEntryDefinition> GetDiscoveredEntries()` — all unlocked entries
- Create `Assets/Ashfall.Core/Help/HelpDatabaseState.cs` — serializable DTO for save/load:
  - `HashSet<string> ManuallyDiscoveredIds` (entries force-unlocked outside normal conditions)
  - Implements `CaptureState/RestoreState` pattern
- Discovery evaluation (**corrected API — verified against `Assets/Ashfall.Core/Flags/IFlagLedger.cs` and `Assets/Ashfall.Core/Ports.cs`**):
  - `flag` type: queries `IFlagLedger.IsSet(flagId)` — **not** `HasFlag`; no `HasFlag` method exists anywhere in the codebase. `IFlagLedger` is `bool IsSet(string flagId)`, `void Set(string flagId)`, `void Clear(string flagId)`, plus counter methods.
  - `item_seen` type: queries a new `IInventoryOracle.HasEverSeen(itemId)` port. Confirmed no `IInventoryOracle` exists today (verified via workspace search) — this is genuinely new, not an extension. Note the sibling risk from AGENTS.md Invariant 4: `InMemoryFlagLedger` uses `OrdinalIgnoreCase`; keep `IInventoryOracle`'s id comparisons consistent (ordinal, case-sensitive on snake_case ids) or you inherit the same cross-host drift risk documented for flags.
  - `day_reached` type: queries `IClock.Day >= threshold` — **not** `CurrentDay`. `IClock` (`Assets/Ashfall.Core/Ports.cs`) exposes `int Day { get; }`, `void AdvanceDays(int days)`, `void SetDay(int day)`. (There is a second, unrelated tick-based `ISimClock` with `DayIndex`/`CurrentTick` — do not confuse the two; `HelpDatabase` should take a plain `IClock`.)
  - `always` type: returns true unconditionally

### Verification
- Unit tests with in-memory JSON covering: load, lookup, search, category filter.
- Discovery tests with mocked `IFlagLedger` and `IClock`.
- `CaptureState/RestoreState` round-trip test.
- `dotnet test` passes.

### Done when
- [ ] `HelpDatabase.cs` implements all methods
- [ ] `HelpDatabaseState.cs` supports save/load
- [ ] All port dependencies are constructor-injected (no `new` of engine types)
- [ ] ≥10 unit tests covering core paths

---

## Step 3 — Author 50 Initial Help Entries

### Goal
Populate `help_entries.json` with 50 well-written entries covering the most confusing game concepts across all 8 categories.

### Implementation
- Target distribution (50 entries):
  - `survival` (8): hunger, thirst, fatigue, warmth, morale, radiation, health, hygiene
  - `medical` (8): chelation, dose_ledger, affliction_pipeline, triage, respiratory_degeneration, somatic_flashback, combat_trauma, chemical_dependency
  - `economy` (7): dynamic_pricing, trade_stances, brine_water, ice_roads, cohort_system, caravans, waystations
  - `social` (7): ideological_friction, ration_conflict, leadership, coalition_camps, census_claims, voluntary_registers, trauma_bonding
  - `crafting` (5): recipes, workbench, material_quality, improvised_tools, water_filtration
  - `exploration` (5): expeditions, fallout_zones, visibility, wasteland_cartography, encounters
  - `combat` (5): combat_trauma_system, triage_priority, defensive_positions, hatch_defense, mutated_fauna
  - `shelter` (5): radiation_shielding, air_filtration, blast_doors, greenhouse, duty_roster
- Writing guidelines:
  - Tone: cold, factual, restrained (matches project tone rules)
  - No real countries/wars/people
  - Short descriptions: one sentence, ≤120 chars
  - Long descriptions: 2–4 sentences explaining the mechanic, what affects it, and what the player can do about it
  - Related IDs form a web — players can hop between connected concepts
- All entries have appropriate `discovery_condition` (basics = `always`, advanced = `flag` or `day_reached`)
- **Ordering caveat:** Step 6 (discovery wiring) is the step that identifies which flag ids actually exist and are settable at the right moment. Do not hand-author specific `flag_id` values in this step for the `flag`-typed entries if those flag ids haven't been confirmed to exist in Core/data yet (see Step 6's note that `flag_first_radiation_exposure`/`flag_first_trade` are unverified placeholders). Practical sequencing: author `always`- and `day_reached`-typed entries fully in this step (their conditions need no new discovery), but leave `flag`-typed entries' `discovery_condition.flag_id` as a documented TODO list until Step 6 confirms real flag ids, then backfill. Otherwise Step 3's "Done when — data integrity selftest passes" will pass (integrity checks don't validate that a flag id is ever actually set by gameplay) while the entries are silently unreachable in practice — the exact "orphaned content" failure mode this project's own Batch 110 plan is designed to catch.

### Verification
- `godot --headless --path . -- --data-integrity-selftest` — all 50 `help_*` IDs pass.
- All `related_ids` resolve to other entries in the file (no dangling references).
- No entry exceeds character limits (write a test that validates constraints).
- Tone review: no forbidden content (magic, fantasy, real countries).

### Done when
- [ ] 50 entries in `help_entries.json` across 8 categories
- [ ] All `related_ids` form a connected graph (no isolated entries)
- [ ] Character limits enforced by test
- [ ] Data integrity selftest passes

---

## Step 4 — Implement Contextual Tooltip in UI

### Goal
Create a Godot UI component that shows a tooltip when the player hovers over a recognized game term in any text label.

### Implementation
- Create `src/UI/HelpTooltip/HelpTooltipPanel.tscn` — a small floating panel:
  - `PanelContainer` with `MarginContainer` > `VBoxContainer` > `TitleLabel` + `DescriptionLabel`
  - Follows mouse position with offset
  - Fades in/out (0.15s animation)
  - Max width 320px, wraps text
  - Uses project fonts (BarlowCondensed for title, ShareTechMono for body)
- Create `src/UI/HelpTooltip/HelpTooltipPanel.cs` (Godot node script):
  - `Show(string helpId, Vector2 position)` — queries `HelpDatabase.GetEntry()`, positions panel, displays short description
  - `Hide()` — fades out
  - Auto-hides after 5 seconds of no movement
- Create `src/UI/HelpTooltip/HelpRichTextLabel.cs` — a custom `RichTextLabel` extension:
  - Scans displayed text for known help terms (maintained in a lookup table derived from entry titles)
  - Wraps recognized terms in BBCode `[url=help:help_id]term[/url]`
  - On `meta_hover_started` signal → shows tooltip
  - On `meta_hover_ended` signal → hides tooltip
  - On `meta_clicked` signal → opens encyclopedia panel to that entry
- Term matching uses case-insensitive whole-word matching to avoid false positives.

### Verification
- `dotnet build Ashfall.csproj` — Godot host compiles with new UI scripts.
- Automated test (not "manual test"): write a headless Godot script-level test or an editor-independent unit test around `HelpRichTextLabel`'s term-matching logic in isolation (pure C#, no scene tree needed) that asserts "chelation" in sample text is wrapped in the expected BBCode. Reserve actual mouse-hover/visual confirmation for a human QA pass called out explicitly as manual, not folded into the automated Done-when list.
- Tooltip respects viewport bounds (doesn't render off-screen) — verify via a unit test on the clamping math (position + panel size vs. viewport size), not by eyeballing it.
- Performance: term scanning caches results per text change, not per frame — verify via a test that changes text once and asserts the scan function is invoked exactly once for N subsequent frames/redraws.

### Risk / Rollback
- Risk: `HelpRichTextLabel` performs text scanning against a lookup table built from *all* entry titles — if entry count grows past the 50 planned in Step 3, whole-word scanning cost across every visible label needs a re-check; flag as a follow-up perf test if entry count later exceeds ~200.
- Rollback: `HelpTooltipPanel`/`HelpRichTextLabel` are new, additive scene/script files with no modifications to existing UI scenes in this step — deleting the two new files fully reverts Step 4 with no residual state.

### Done when
- [ ] `HelpTooltipPanel.tscn` + `.cs` exist and render correctly (verified by an automated test on positioning/clamping logic, plus a noted manual QA pass — not manual-only)
- [ ] `HelpRichTextLabel.cs` identifies and links help terms, proven by a unit test on the term-matching function in isolation
- [ ] Tooltip shows on hover with correct content from `HelpDatabase`
- [ ] Click-through opens encyclopedia (Step 5)
- [ ] Godot host compiles cleanly (`dotnet build Ashfall.csproj`, 0 errors, 0 warnings per project convention)
- [ ] Naming check: confirm `HelpTooltipPanel`/`HelpRichTextLabel`/`HelpDatabase` do not collide with the existing "Codex" terminology already used by `JournalSystem.OnCodexUnlocked`/`CodexUnlockCount` (discovery-tracking) and `OralLoreCatalog`/`BunkerBlueprintCatalog` (in-world lore "Codex" catalogs) — do not name any new class or UI panel "Codex" anything, to avoid confusing three unrelated concepts that already share vocabulary

---

## Step 5 — Create Encyclopedia Panel

### Goal
Build a full-screen encyclopedia UI panel where players can browse, search, and read all discovered help entries.

### Implementation
- Create `src/UI/Encyclopedia/EncyclopediaPanel.tscn`:
  - Left sidebar: category buttons (8 categories) + search bar at top
  - Center: scrollable list of entry titles (filtered by selected category or search)
  - Right: detail view showing selected entry's full content
  - Bottom of detail view: "Related" section with clickable links to related entries
  - Undiscovered entries shown as "???" with locked icon (teases existence without revealing content)
  - Entry count display: "12/50 discovered"
- Create `src/UI/Encyclopedia/EncyclopediaPanel.cs`:
  - `Open()` / `Close()` — toggle visibility, pause game while open
  - `OpenToEntry(string helpId)` — navigate directly to a specific entry (used by tooltip click-through)
  - Binds to `HelpDatabase.GetDiscoveredEntries()` for filtering
  - Search queries `HelpDatabase.Search(query)` with 300ms debounce
  - Tracks "new" badge on entries discovered since last encyclopedia open
- Visual design:
  - Dark background (matches bunker UI aesthetic)
  - Amber/green text on dark panels (consistent with existing UI theme)
  - Category icons match the game's existing icon set
  - Responsive to window resize (panels stack on narrow viewport)

### Verification
- `dotnet build Ashfall.csproj` — compiles cleanly.
- Panel opens/closes without errors — assert via a test that toggling `Open()`/`Close()` twice leaves `Visible` state consistent and does not leak signal connections (no duplicate `meta_clicked` handlers after repeated open/close).
- Search returns correct results — assert against a fixed known query on the real (or a fixture) `help_entries.json`, e.g. `Search("radiation")` returns exactly the entries whose title/tags/description contain "radiation", named explicitly rather than "correct results".
- "Related" links navigate between entries — verify `OpenToEntry(relatedId)` is invoked with the correct id when a related-entry link is activated (unit-testable on the click handler in isolation).
- Undiscovered entries are hidden behind "???" (no information leak) — verify by asserting the rendered list for an undiscovered id never contains its real `title`/`short_description` text anywhere in the panel's exposed strings, not just visually.

### Risk / Rollback
- Risk: "pause game while open" (`Open()`) needs to interact correctly with the existing pause/tick system — confirm which mechanism the Godot host already uses to pause simulation (if any) before adding a second, competing pause flag. If no existing pause mechanism exists, decide explicitly whether Encyclopedia pausing is in scope for this batch or deferred.
- Rollback: `EncyclopediaPanel` is additive and gated behind a new UI entry point; if it is not yet wired into any existing menu/HUD flow when this step ends, it has zero blast radius. Do not wire it into the main HUD flow until Step 6 confirms discovery works — reduces risk of shipping a broken panel behind a live keybind.

### Done when
- [ ] `EncyclopediaPanel.tscn` + `.cs` exist with full layout
- [ ] Category filtering works
- [ ] Search with debounce works
- [ ] Related entry navigation works
- [ ] Undiscovered entries properly masked
- [ ] "New" badge appears on freshly discovered entries

---

## Step 6 — Wire Help Discovery to Gameplay

### Goal
Connect the `HelpDatabase` discovery system to actual gameplay events so entries unlock naturally as the player encounters relevant situations.

### Implementation
- In `src/Host/` (or appropriate Godot host session), wire discovery triggers:
  - **Flag-based:** `IFlagLedger` has no change-notification/event mechanism (confirmed: it's a plain `IsSet`/`Set`/`Clear` + counter interface, no `OnFlagSet` event). "Subscribe to flag changes" as originally phrased is not implementable as-is. Realistic approach: poll `IFlagLedger.IsSet(flagId)` for all flag-gated help entries on a cheap interval (e.g. once per day-tick, alongside the day-based check below) rather than event-subscribing. If true push notification is required, that's a separate, larger task: adding a change-notification wrapper around `IFlagLedger` — call this out as an explicit dependency risk, not a one-line wire-up.
  - **Item-based:** Create `IInventoryOracle` port in Core with `bool HasEverSeen(string itemId)`. Implement in Godot host by tracking items that have entered any survivor's inventory. When a new item is first seen, check if any help entries unlock.
  - **Day-based:** On day tick, check if any `day_reached` entries newly qualify.
  - **Always:** Available from game start.
- Create discovery notification:
  - Small toast notification in top-right: "[Book icon] New: {entry title}" — fades after 3 seconds
  - Clickable — opens encyclopedia to that entry
  - Does not interrupt gameplay (non-modal)
- Wire to existing event flow (**flag names below are illustrative placeholders, not verified to exist** — before implementing, grep `Assets/StreamingAssets/Data/` and Core systems for the actual flag ids each trigger sets; do not assume `flag_first_radiation_exposure` / `flag_first_trade` are real until confirmed):
  - First radiation exposure → some existing radiation-related flag → unlocks radiation help entries
  - First trade → some existing trade-related flag → unlocks economy entries
  - Day 7 reached (via `IClock.Day >= 7`) → unlocks "Nuclear Winter" entry
  - First affliction → some existing affliction-related flag → unlocks medical entries
- Save integration: `HelpDatabase.CaptureState()` must be added to the save file via `Main.cs`'s existing `SaveAll` orchestration (per AGENTS.md H7, `Main.cs` follows a `SetupXxx`/`SaveXxx`/`FlushXxxIfDirty` triad per subsystem — this plan must add a triad, not just a `SaveXxx` call, or it repeats the "triad drift" bug pattern AGENTS.md already warns about). Confirm the five save stores that currently lack checksums (`ExpeditionSaveStore`, `MedicalSaveStore`, `NarrativeSaveStore`, `WorldSaveStore`, `JournalSaveStore`) are not the model to copy — a **new** `HelpSaveStore` should ship with a checksummed envelope from day one, not bare state, to avoid adding a 6th store to that known-gaps list.

### Verification
- Unit test: mock `IFlagLedger.IsSet(...)` returning true → `IsDiscovered` returns true for matching entry.
- Unit test: `IClock.Day` advances past threshold → entry discoverable.
- Integration test: `CaptureState` includes discovery state, `RestoreState` preserves it.
- `dotnet test` — all tests pass.
- `dotnet build Ashfall.csproj` — Godot host compiles.

### Done when
- [ ] Flag-based discovery triggers work via `IFlagLedger.IsSet` polling (event-push explicitly deferred — see Implementation notes; do not claim "subscribe to flag changes" as done unless a change-notification wrapper was actually built)
- [ ] Item-based discovery triggers work (with new `IInventoryOracle` port, added to `Ports.cs` alongside existing `IFileIO`/`IJsonSerializer`/`ILog`/`IClock`)
- [ ] Day-based discovery triggers work using `IClock.Day` (not `CurrentDay`)
- [ ] Toast notification appears on discovery
- [ ] Discovery state persists across save/load via a checksummed `HelpSaveStore` envelope (verify with a round-trip test asserting non-empty `Checksum`, matching the pattern in `SaveStoreChecksumSweepTests`)
- [ ] Actual flag ids used for radiation/trade/affliction triggers are confirmed to exist in the data authority or existing Core systems (not invented placeholder names)
- [ ] No existing test regressions (`dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` full suite, not just new tests)

---

## Step 7 — Write Help System Tests

### Goal
Comprehensive test coverage for the help system ensuring correctness, save/load integrity, and content validation.

### Implementation
- Create `Ashfall.Core.Tests/HelpDatabaseTests.cs`:
  - `LoadsEntriesFromJson()` — valid JSON → all entries accessible
  - `GetEntry_UnknownId_ReturnsNull()` — graceful miss
  - `GetByCategory_FiltersCorrectly()` — only matching entries returned
  - `Search_MatchesTitle()` — "radiation" finds radiation entries
  - `Search_MatchesTags()` — tag-based search works
  - `Search_CaseInsensitive()` — "CHELATION" finds "chelation"
  - `Search_NoMatch_ReturnsEmpty()` — no crash on miss
  - `IsDiscovered_FlagCondition_QueriesLedger()` — verifies `IFlagLedger.IsSet` integration (not `HasFlag`)
  - `IsDiscovered_DayCondition_QueriesClock()` — verifies `IClock.Day` integration (not `CurrentDay`)
  - `IsDiscovered_AlwaysCondition_ReturnsTrue()` — always-visible entries
  - `MarkDiscovered_ForcesDiscovery()` — manual unlock works
  - `CaptureState_IncludesManualDiscoveries()` — state captures correctly
  - `RestoreState_RestoresManualDiscoveries()` — state restores correctly
  - `CaptureState_RestoreState_RoundTrip()` — full round-trip
- Create `Ashfall.Core.Tests/HelpContentValidationTests.cs`:
  - `AllEntries_HaveValidIds()` — `help_` prefix enforced
  - `AllEntries_ShortDescriptionWithinLimit()` — ≤120 chars
  - `AllEntries_LongDescriptionWithinLimit()` — ≤800 chars
  - `AllEntries_RelatedIdsResolve()` — no dangling references
  - `AllEntries_HaveAtLeastOneTag()` — searchability guaranteed
  - `AllEntries_ValidCategory()` — enum enforcement
  - `AllEntries_ValidDiscoveryCondition()` — condition type recognized
  - `NoDuplicateIds()` — uniqueness
  - `RelatedIdsFormConnectedGraph()` — no isolated clusters (optional, informational)

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all new tests pass.
- Content validation tests run against actual `help_entries.json` (integration test reading real file).
- No existing tests broken.
- Full verification checklist passes.

### Done when
- [ ] ≥14 unit tests for `HelpDatabase` behavior
- [ ] ≥8 content validation tests for `help_entries.json`
- [ ] All tests pass
- [ ] Save/load round-trip verified
- [ ] Full verification checklist (5 steps) passes

---

## Summary

| Step | Deliverable | Layer | Risk | Depends on |
|------|-------------|-------|------|------------|
| 1 | `help_entries.json` schema + `HelpEntryDefinition.cs` | Core/Data | None | — |
| 2 | `HelpDatabase.cs` + `HelpDatabaseState.cs` | Core | None | Step 1 |
| 3 | 50 authored help entries (flag-typed entries' `flag_id` values backfilled after Step 6, not before) | Data | None | Steps 1–2, partially Step 6 |
| 4 | `HelpTooltipPanel` + `HelpRichTextLabel` (Godot UI) | Host | Low | Steps 1–2 |
| 5 | `EncyclopediaPanel` (full-screen browse/search) | Host | Low | Steps 2–4 |
| 6 | Discovery trigger wiring + toast notification + checksummed `HelpSaveStore` | Host/Core | **Medium** (flag-push mechanism unresolved; real flag ids unverified; new save store must ship checksummed, unlike 5 existing gap stores) | Steps 2–3 |
| 7 | 22+ tests (behavior + content validation) | Tests | None | Steps 1–6 |

**Exit criteria:** Players can hover any game term for a tooltip, open an encyclopedia to browse/search discovered entries, and discovery progresses naturally through gameplay. All help content is validated by automated tests and the data integrity selftest.

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real ASHFALL codebase (`Assets/Ashfall.Core/Journal/JournalSystem.cs`, `KnowledgeBase.cs`, `Ashfall.Core/Flags/IFlagLedger.cs`, `Ashfall.Core/Ports.cs`). Corrections applied in place above; summarized here for traceability.

**Factual errors found and fixed:**
1. **`IFlagLedger.HasFlag(...)` does not exist.** The real method is `IsSet(string flagId)`. Fixed in Step 2 and Step 6.
2. **`IClock.CurrentDay` does not exist.** The real property is `int Day { get; }` on `Ashfall.Core.IClock` (`Ports.cs`). There is a second, unrelated tick-based clock, `ISimClock`, with `DayIndex`/`CurrentTick` — the plan must not conflate the two. Fixed in Step 2 and Step 6.
3. **"KnowledgeBase tracks player-discovered knowledge IDs" was correct**, but the plan implied it as a general-purpose, standalone system. In reality `KnowledgeBase` is a private field owned by `JournalSystem` (`_knowledge`), not independently save-registered — it's nested inside `JournalSave.Knowledge`. The new `HelpDatabase` must not attempt to extend or read `JournalSystem.Knowledge` as if it were a general registry; it needs its own independent discovery state. Fixed in Motivation section.
4. **No `KnowledgeKey` type exists.** Knowledge keys are plain `string`s produced by static helpers in `KnowledgeKeys` (plural). This didn't block the plan (it never referenced the type directly) but is noted so implementers don't go looking for a nonexistent type.
5. **`IInventoryOracle` does not exist yet** — confirmed via workspace-wide search. The plan correctly proposes it as new; this is now explicitly called out as new port surface, not a wrapper around existing capability.
6. **`IFlagLedger` has no change-notification/event mechanism.** Step 6's "subscribe to flag changes" was not implementable as literally written — no `OnFlagSet`-style event exists anywhere on the interface or `InMemoryFlagLedger`. Rewritten to specify polling as the realistic default, with event-push flagged as a separate, larger task.

**Scope/process issues found and fixed:**
7. **Illogical ordering:** Step 3 (author 50 entries with `flag`-typed discovery conditions) was scheduled before Step 6 (the step that determines which flag ids actually exist and fire at the right time). As originally ordered, Step 3's Done-when ("data integrity selftest passes") would pass while flag-typed entries silently reference non-existent or never-set flags — i.e., the plan would ship exactly the "orphaned/unreachable content" failure mode that the companion Batch 110 plan exists to detect. Fixed by splitting Step 3 into always/day_reached entries (safe to author immediately) and flag-typed entries (backfilled after Step 6 confirms real flag ids).
8. **Underestimated effort:** 6–8 sessions did not obviously account for the authoring cost of 50 tone-consistent entries (Step 3) on top of 6 code/UI steps. Revised to 8–11 sessions or an explicit split recommendation.
9. **Missing risk/rollback notes:** the original plan had no risk or rollback discussion anywhere. Added per-step risk/rollback notes (Steps 1, 4, 5, 6) covering: prefix-registration revert path, additive-file blast radius, pause-system collision risk, and the save-store checksum requirement (new stores must not join the 5 existing checksum-gap stores AGENTS.md already tracks).
10. **Vague Done-when / unrunnable verification:** "renders correctly," "opens/closes without errors," "returns correct results," and "manual test: ... hover shows tooltip" were not automatable acceptance criteria. Tightened throughout Steps 1, 4, 5, 6 to name the exact assertion, mock, or test each criterion maps to, and to separate genuinely-manual QA from what should be an automated unit test.
11. **Naming collision risk:** the term "Codex" is already used for two unrelated existing concepts (`JournalSystem.OnCodexUnlocked` discovery-tracking, and `OralLoreCatalog`/`BunkerBlueprintCatalog` in-world lore catalogs). Added an explicit Done-when check in Step 4 to avoid a third, conflicting use of "Codex" in the new Help UI naming.
12. **Save integration under-specified:** "included in the save file via existing `SaveAll` orchestration" glossed over AGENTS.md's own documented triad pattern (`SetupXxx`/`SaveXxx`/`FlushXxxIfDirty`) and its known "triad drift" risk, and glossed over the checksum requirement now mandatory for new save stores. Fixed in Step 6.

**What was already correct and left unchanged:** the `IJsonSerializer`/`IFileIO`/`ILog` constructor-injection pattern in Step 2 matches real port names in `Ports.cs`; `CaptureState/RestoreState` is the correct save pattern; the `help_` prefix / Tier-2 `related_ids` validation approach is consistent with how `CatalogIntegrityValidator` actually works; no existing help/tooltip/encyclopedia system exists to conflict with this new work (verified by search).
