# ASHFALL — Quality Roadmap Batch 110

## Theme: Automated Content Coverage Reporting — Find Unreachable/Orphaned Game Content

**Priority:** MEDIUM<br>
**Risk:** Low-to-Medium — pure analysis tool, no changes to game code or existing data; **correction:** "None" was too strong as a blanket rating — the tool's correctness depends entirely on correctly reverse-engineering undocumented, per-file-inconsistent JSON schemas (mixed camelCase/snake_case, several files not yet verified), so the real risk is "silently produces wrong/misleading results," not zero risk. No data or gameplay code is modified by this work.<br>
**Scope:** `Assets/Ashfall.Core/ContentAnalysis/` (new — confirmed this path does not already exist), `Ashfall.Core.Tests/`, CI pipeline<br>
**Estimated effort:** 4–6 focused sessions. **Correction:** this likely undercounts the real cost of Step 2 (schema reverse-engineering across ~10+ inconsistent file shapes) — treat 4–6 as optimistic and budget for at least one extra session once `questline_master.json`/`factions.json`/NPC data schemas are actually read.

---

## Motivation

**Correction (verified against the real repository):** the figures below in the original draft conflated several distinct counts. Verified counts, by exact file listing:
- **196 files** exist under `Assets/StreamingAssets/Data/narrative/` — but these are lore/flavor-text documents (bestiaries, logbooks, dispatches), **not** encounter definitions. There is no `encounters/` subfolder anywhere in the data tree.
- Files with "encounter" in the name are only **3**: `crossing_encounters.json`, `door_encounters.json`, `narrative_encounters.json` (top-level `Data/`, not `narrative/`).
- **296 total `.json` files** exist under `Assets/StreamingAssets/Data/` (98 top-level + 196 narrative + 1 documents + 1 whitelists). Note `CatalogIntegrityValidator`'s own selftest only walks the 55 top-level gameplay catalogs non-recursively — it does not descend into `narrative/`, `documents/`, or `whitelists/`. Any new tool must decide explicitly which of these three different scopes ("55 catalogs validator touches", "98 top-level JSON files", "296 total JSON files") it means at each point, rather than using one number loosely for all of them.
- ID counts are **approximate, not exact** (grep-based, both methods double-count reused ids to some degree): item_-prefixed ids number in the low hundreds, loc_ similarly, quest_ ~290-390 depending on method, npc_ ~46, event_ ~50-53, radio_ ~150-170, echo_ ~25-45. **`recipe_` and `encounter_` as literal ID prefixes are essentially unused in the real data** — `recipes.json` entries use verb-phrase ids like `craft_bandage`/`purify_water`, not `recipe_*`, despite `recipe_` being a defensive/unused entry in `CatalogIntegrityValidator`'s prefix list. Any plan math based on "500+ IDs" or per-prefix breakdowns must be labeled as approximate and must not assume `recipe_`/`encounter_` prefixes describe real entity counts.

The ASHFALL project contains, more precisely:
- 196 narrative *lore/flavor* JSON files (not encounters)
- 3 files with "encounter" content (`crossing_encounters.json`, `door_encounters.json`, `narrative_encounters.json`), plus additional narrative-adjacent files like `echoes.json`, `radio.json`, `events.json`
- 296 total JSON files under the data authority (98 top-level + 196 narrative + 2 misc)
- An approximate few hundred to low-thousands of authored IDs across items, locations, factions, quests, NPCs, encounters, recipes, events, radio transmissions, echoes — exact count requires running the validator itself, not a plan-time estimate
- Cross-referencing between systems (encounters reference items, quests reference encounters, NPCs reference locations, recipes reference items, events trigger quests) — **the specific field names originally listed here (`available_encounters`, `prerequisite_flags`, `reward_items`, `triggers_encounter`, `trade_items`, `home_location`, `unlock_condition`) do not exist in any of the 296 real JSON files** (verified via exact-string grep across the entire data tree, 0 matches for each). See Step 2 for the real field names to use instead.

Without automated coverage analysis:
- **Orphaned content** exists undetected — authored encounters that no quest or event triggers, items that no recipe produces and no encounter rewards
- **Dead-end paths** — content reachable only through one fragile trigger (if that trigger is refactored, content silently becomes unreachable)
- **Wasted authoring effort** — writers create content that players never see because the trigger chain is broken
- **Regression risk** — refactoring a trigger system may orphan content without any test catching it

**Correction:** the original draft claimed "the existing `CatalogIntegrityValidator` checks that referenced IDs *exist* ... but does NOT check that content is *reachable*" — this part was actually correct. But the plan's own Step framing elsewhere implicitly treated the validator as something to extend/reuse. To be explicit and unambiguous: `CatalogIntegrityValidator.cs` (658 lines, read in full) implements exactly 5 tiers — REGISTRY (flat id→location map, no graph), TIER-1 (every prefixed string must resolve to a registered id), TIER-2 (values under known reference keys must resolve), RANGES (`minDay`/`maxDay` ordering), UNIQUENESS (no duplicate root-level `id` per file). **None of these five tiers build a graph, do BFS/DFS, or check "is this id referenced by anything."** The validator only ever checks reference→definition existence, never the inverse. There is also no other code anywhere in the repo (Core or `src/`) that does content-reachability graph analysis — the only two existing "coverage"-flavored things are `AssetRegistry.RunFullCoverage` (art-asset coverage: does an item have a sprite — wired to `--asset-coverage-report`) and an unrelated map-pathfinding BFS in `WastelandMapSystem.cs` (physical travel between discovered locations, not content-ID references). This plan is therefore genuinely new work, not an extension of existing capability — which is fine, but the plan should say so accurately rather than only gesturing at what the validator lacks.

Content coverage analysis is the complement of integrity validation: integrity ensures "if you reference it, it exists"; coverage ensures "if it exists, something references it."

---

## Step 1 — Define Content Reachability Model

### Goal
Formally define what "reachable" means for each content type, what constitutes a root node, and what edges connect content pieces.

### Implementation
- Create `Assets/Ashfall.Core/ContentAnalysis/ReachabilityModel.cs` with documented definitions:
  - **Root nodes** (always reachable — game start provides these):
    - Starting items (defined in `survivors.json` initial loadouts)
    - Starting location (`loc_bunker` or equivalent starting zone)
    - Day-0 quests (quests with `min_day: 0` or `trigger: "game_start"`)
    - Always-available recipes (no prerequisite)
    - Starting NPCs (present at game start)
  - **Edge types** (how content connects):
    - `produces`: recipe → output item; encounter reward → item
    - `requires`: recipe → input items; quest → prerequisite items/flags
    - `triggers`: event → quest; flag → encounter; day threshold → event
    - `references`: NPC → location; quest → encounter; encounter → NPC
    - `unlocks`: quest completion → new location; flag → recipe availability
    - `contains`: location → encounters available there; zone → locations
  - **Reachable definition**: A content node is reachable if there exists a path from any root node to it through a chain of edges, where each intermediate node is also reachable.
  - **Under-connected definition**: A content node is reachable but has in-degree = 1 (only one path leads to it). If that single path breaks, the content becomes orphaned.
- Document assumptions:
  - Expansions are assumed active (all 4 expansion content is considered)
  - RNG branches are considered reachable (any branch that *can* fire counts)
  - Day-gated content is reachable (day will eventually advance)
  - Player-choice branches are all reachable (player might choose either option)

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles with new file.
- Model documentation is clear enough that another developer could implement the graph builder from it alone.
- Edge types cover all known cross-reference patterns in the data.

### Done when
- [ ] `ReachabilityModel.cs` defines root nodes, edge types, and reachability/under-connected
- [ ] All ID prefix types (`item_`, `loc_`, `quest_`, `encounter_`, `npc_`, `recipe_`, `event_`, etc.) have defined reachability semantics
- [ ] Assumptions documented (expansions active, RNG branches reachable)
- [ ] Compiles cleanly

---

## Step 2 — Implement ContentGraphBuilder

### Goal
Build a tool that parses all JSON catalogs in `Assets/StreamingAssets/Data/` and constructs a directed graph of content ID references.

### Implementation
- Create `Assets/Ashfall.Core/ContentAnalysis/ContentGraphBuilder.cs`:
  - Constructor: `ContentGraphBuilder(IFileIO fileIO, IJsonSerializer serializer, ILog log)`
  - `ContentGraph BuildGraph(string dataRootPath)` — scans all JSON files, extracts IDs and references
  - **Correction: the field names below are the actual verified field names from the real catalog files (sampled directly, not guessed) — the original draft's field names (`available_encounters`, `prerequisite_flags`, `reward_items`, `triggers_encounter`, `trade_items`, `home_location`, `unlock_condition`) do not exist anywhere in the 296 real JSON files (0 matches on exact-string search) and must not be used.** Before writing the real parser, re-verify these against the current file contents at implementation time — the list below is a snapshot, and schema drift is possible.
  - Parsing strategy per file type (verified field names):
    - `items.json`: fields are `id, displayName, description, type, stackMax, weight, radProtection, durability, contamination, hungerRestore, thirstRestore, healthEffect, radCleanse, moraleEffect, isEquipable, equipSlot, tradeValue, empShielded`. There is no `recipe` field on items — the reverse link (item ← recipe) must be derived from `recipes.json`'s `resultItemId`, not from an items.json field.
    - `recipes.json`: fields are `id, recipeName, ingredients, resultItemId, resultAmount, craftingTimeHours, requiredStationId`. `ingredients` create `requires` edges to items; `resultItemId` creates the `produces` edge (not a generic "reward_items" field). Note real recipe ids are verb-phrases (`craft_bandage`, `purify_water`), not `recipe_*`-prefixed — do not filter by `recipe_` prefix when identifying recipe nodes; use the source file instead.
    - `locations.json`: fields are `id, displayName, description, dangerLevel, travelHours, baseRadsPerHour`. **There is no `available_encounters` field.** How a location is connected to the encounters that can occur there needs a separate investigation at implementation time (check `narrative_encounters.json`'s `requiredLocationId` and `crossing_encounters.json`'s `target_location` — see below — as the likely real link direction: encounter → location, not location → encounter list).
    - `narrative_encounters.json`: fields are `id, title, description, category, baseWeight, stealthWeightMultiplier, speedWeightMultiplier, minDangerLevel, requiredLocationId, forceOnArrival, choices`. Uses `camelCase`. `requiredLocationId` creates a `references`/`contains`-style edge to the location.
    - `crossing_encounters.json`: top-level shape is `{encounters, crises}`, not a flat list; encounter entries have `id, name, target_location, description, threat_level, choices`. Uses `snake_case` — inconsistent with `narrative_encounters.json`'s `camelCase`. **The graph builder must handle both naming conventions per-file; do not assume one convention project-wide**, or references in whichever convention isn't handled will be silently missed (a real, verified risk, distinct from the invented-field-name problem).
    - `door_encounters.json`: top-level shape is `{entries}`; entries have `encounterId, visitorName, visitorFaction, description, minDay, maxDay, threatLevel, choices`.
    - `radio.json`: fields are `id, frequency, minDay, maxDay, intelType, confidence, message`. No `unlock_condition` field — gating appears to be via `minDay`/`maxDay` range, not a flag-based unlock condition object.
    - `echoes.json`: fields are `id, title, bodyText, weight, minDay, conditions, choices`. `conditions` is the closest real analog to the invented "unlock_condition" — inspect its actual shape before assuming it is flag-based.
    - `events.json`: fields are `id, title, bodyText, weight, minDay`.
    - `survivors.json`: fields are `id, displayName, profession, bio, baseHealth`. No `trade_items` field was found — NPC/survivor trade linkage needs a separate, explicit investigation rather than an assumed field name.
    - `questline_master.json` (quest data): field names were not fully catalogued in this pass — **before implementing, read this file directly and confirm exact field names for prerequisites/rewards/triggers rather than reusing any assumed name from the original draft (`prerequisite_flags`, `reward_items`, `triggers_encounter` are all unverified and likely wrong given the pattern above).**
    - `factions.json`: not sampled in this verification pass — confirm real field names (the draft's assumed `home_location` is unverified) before implementing.
    - `npcs.json`: not confirmed to exist as a separate file from `survivors.json` in this pass — verify the actual NPC data file name and shape before implementing; do not assume a file called `npcs.json` exists.
  - Handle missing/optional fields gracefully (many entries won't have all fields).
  - Handle **mixed camelCase/snake_case field naming per file** (confirmed real inconsistency, not hypothetical) — the parser needs a per-file-type field map, not a single global field-name table.
  - Use existing reference-key list from `CatalogIntegrityValidator` (`ReferenceKeys`, e.g. `resultItemId`, `requiredLocationId`-style keys — the real list is smaller and different in exact contents than "200+ prefixes" implied; that figure describes `IdPrefixes`, a different, much larger list used for Tier-1 prefix matching, not `ReferenceKeys` used for Tier-2 — verify both lists directly in `CatalogIntegrityValidator.cs` before reusing) to identify edges automatically where possible, supplemented by the per-file field maps above for fields that aren't in `ReferenceKeys`.
- Create `Assets/Ashfall.Core/ContentAnalysis/ContentGraph.cs`:
  - `Dictionary<string, ContentNode> Nodes` — keyed by content ID
  - `List<ContentEdge> Edges` — directed edges with type label
  - `ContentNode`: `Id`, `Type` (item/quest/etc.), `SourceFile`, `IsRoot`
  - `ContentEdge`: `FromId`, `ToId`, `EdgeType` (produces/requires/triggers/etc.)
  - `int NodeCount`, `int EdgeCount` — summary stats

### Verification
- Unit test with minimal synthetic JSON (3 items, 2 recipes, 1 quest) → correct graph.
- Edge count matches expected (every reference creates exactly one edge).
- Handles malformed/partial JSON without crashing (logs warning, skips entry).
- `dotnet test` passes.

### Done when
- [ ] `ContentGraphBuilder.cs` parses all major JSON catalog types
- [ ] `ContentGraph.cs` holds nodes + edges with type information
- [ ] ≥5 unit tests with synthetic data
- [ ] Handles partial/missing fields gracefully
- [ ] Compiles and tests pass

---

## Step 3 — Run Reachability Analysis

### Goal
Implement BFS/DFS from root nodes to mark all reachable content, producing a clear partition of reachable vs. unreachable nodes.

### Implementation
- Create `Assets/Ashfall.Core/ContentAnalysis/ReachabilityAnalyzer.cs`:
  - `ReachabilityResult Analyze(ContentGraph graph)`:
    - Identifies root nodes (per `ReachabilityModel` rules)
    - BFS from all roots simultaneously (multi-source BFS)
    - Marks each visited node as reachable, records the shortest path from nearest root
    - Returns `ReachabilityResult`:
      - `List<ContentNode> ReachableNodes`
      - `List<ContentNode> UnreachableNodes`
      - `Dictionary<string, int> DistanceFromRoot` — hop count from nearest root
      - `Dictionary<string, List<string>> PathFromRoot` — one example path per node (for debugging)
  - Handle cycles gracefully (content that references itself or mutual references — BFS naturally handles this).
  - Performance target: complete analysis in <100ms for the actual graph size. **Correction:** "500+ nodes" was an inherited estimate from the same unverified "500+ IDs" figure corrected in Motivation — grep-based counting of actual id-defining fields (not all prefixed-string occurrences) found roughly 4,015 id-field occurrences across all 296 JSON files by one method, with per-prefix breakdowns in the low hundreds each; this is closer to the real order of magnitude but is still approximate, not authoritative. Regardless of the exact figure, <100ms is a safe target for a graph this size (BFS is linear in nodes+edges) — keep the target but do not cite "500+" as a verified fact in code comments or reports; say "actual data size" or measure it once Step 2 is implemented and record the real number then.
- Create `Assets/Ashfall.Core/ContentAnalysis/ReachabilityResult.cs`:
  - `float CoveragePercentage` → `ReachableNodes.Count / TotalNodes.Count * 100`
  - `IReadOnlyList<ContentNode> GetUnreachableByType(string type)` — filter orphans by content type
  - `string GenerateReport()` — human-readable summary

### Verification
- Unit test: fully connected graph → 100% reachable.
- Unit test: graph with isolated node → that node appears in `UnreachableNodes`.
- Unit test: graph with cycle → all nodes in cycle reachable if any root connects to cycle.
- Unit test: disconnected subgraph → all nodes in subgraph unreachable.
- Performance test: 1000-node random graph completes in <100ms.
- `dotnet test` passes.

### Done when
- [ ] `ReachabilityAnalyzer.cs` implements multi-source BFS
- [ ] `ReachabilityResult.cs` provides coverage stats + filtered views
- [ ] ≥5 unit tests covering connected, disconnected, cyclic graphs
- [ ] Performance acceptable for actual data size
- [ ] Compiles and tests pass

---

## Step 4 — Report Unreachable Content

### Goal
Generate a structured report of all content IDs that exist in the data authority but cannot be reached from normal gameplay.

### Implementation
- Create `Assets/Ashfall.Core/ContentAnalysis/ContentCoverageReporter.cs`:
  - `string GenerateOrphanReport(ReachabilityResult result)` — markdown-formatted report. **The example below is illustrative only** — the node counts (523/498/95.2%), ids (`item_rusty_compass`, `item_broken_radio_tube`, `quest_find_lost_dog`, `npc_hermit_scientist`, `loc_hidden_lab`), and file paths (`narrative/abandoned_train.json`) are all invented placeholders for format demonstration, not real content found in the repository. Do not treat them as a preview of actual results — the real baseline numbers can only come from running the tool once Steps 1–3 are implemented (see Step 6):
    ```
    # Content Coverage Report

    Generated: {date}
    Total content nodes: {N}
    Reachable: {N} ({pct}%)
    Unreachable: {N} ({pct}%)

    ## Unreachable Items ({N})
    - {real_item_id} ({source_file}:{line}) — no recipe produces it, no encounter rewards it
    ...

    ## Unreachable Encounters ({N})
    - {real_encounter_id} ({source_file}) — no location/quest references it
    ...

    ## Unreachable Quests ({N})
    - {real_quest_id} ({source_file}:{line}) — prerequisite flag never set by any event
    ...

    ## Unreachable NPCs ({N})
    - {real_npc_id} ({source_file}:{line}) — its only location is itself unreachable
    ...
    ```
  - For each orphan, include:
    - ID and source file (with line number if available)
    - Reason for unreachability (no incoming edges, or all incoming edges from other unreachable nodes)
    - Suggested fix (add to a recipe/encounter/quest, or mark as intentionally hidden)
- Add `--content-coverage-report` CLI flag to Godot headless runner. **Correction (verified against `src/Host/HostCli.cs`):** this is a well-precedented, small change, not a new mechanism — the existing pattern (used by `--data-integrity-selftest`, `--asset-coverage-report`, etc.) is: add a value to the `HostCliAction` enum, add an `if (Has(args, "--content-coverage-report")) return HostCliAction.ContentCoverageReport;` line inside `HostCli.Parse`, add a help line in `PrintHelp()`, add a dispatch `case` in `src/Main.cs`'s existing switch (~line 279+), and implement `RunContentCoverageReport(string dataDir)` in a `HostCli.*.cs` partial file, following `AssetRegistry.RunFullCoverage`'s "report-only, never fails the run" style as the closest existing precedent for a coverage-flavored flag.
  - Runs the full pipeline (build graph → analyze → report)
  - Outputs report to stdout and optionally to `CONTENT_COVERAGE_REPORT.md`
  - Exit code: 0 if coverage ≥ threshold (default 90%), non-zero otherwise. **Note:** this makes the flag gating (unlike its closest precedent, `--asset-coverage-report`, which is explicitly documented as report-only/never-failing). Decide deliberately whether this flag should ever fail CI on its own, or whether gating belongs solely in the `dotnet test` fact in Step 6 — having both a failing CLI exit code AND a failing test for the same threshold is redundant enforcement and doubles the maintenance surface when the threshold changes.

### Verification
- Unit test: synthetic data with 1 orphan → report contains that orphan with correct reason.
- Integration test: run against actual `Assets/StreamingAssets/Data/` (may find real orphans — that's expected and valuable; do not hardcode an expected count in this test since the real baseline is unknown until the tool is actually run — assert structural properties like "report is non-empty markdown" and "every id in `UnreachableNodes` also appears in the report text" instead).
- Report format is valid markdown.
- CLI flag works: `godot --headless --path . -- --content-coverage-report` produces output — verify by checking process exit code and stdout content, not just "it runs."

### Done when
- [ ] `ContentCoverageReporter.cs` generates structured markdown report
- [ ] Each orphan has ID, source file, reason, and suggested fix
- [ ] CLI flag `--content-coverage-report` wired following the exact `HostCliAction`/`Parse`/`PrintHelp`/`Main.cs` dispatch pattern used by `--asset-coverage-report` (verify this pattern directly in `src/Host/HostCli.cs` before implementing, since it may have changed)
- [ ] Report generated from actual data (real baseline captured and recorded — not the placeholder numbers shown above)
- [ ] Decision made and documented on whether the CLI exit code gates CI, or only the `dotnet test` fact in Step 6 does (avoid redundant enforcement)
- [ ] Tests pass

---

## Step 5 — Report Under-Connected Content

### Goal
Identify content that is technically reachable but fragile — connected to the rest of the game through only a single path.

### Implementation
- Add to `ReachabilityAnalyzer.cs`:
  - `List<ContentNode> GetUnderConnectedNodes(ContentGraph graph, ReachabilityResult result)`:
    - For each reachable node, count in-degree (number of other reachable nodes with edges pointing to it)
    - Nodes with in-degree = 1 are "under-connected" (single point of failure)
    - Exclude root nodes (they are entry points by definition)
    - Exclude nodes whose single incoming edge is from a root (these are direct children of roots, expected to have in-degree 1)
  - `FragilityScore` per node: `1.0 / in_degree` — higher = more fragile
- Add to `ContentCoverageReporter.cs`:
  - Section in report: "## Under-Connected Content (Fragile Paths)"
  - Groups by fragility severity:
    - **Critical (in-degree 1, distance from root > 3):** Deep content hanging by a single thread
    - **Warning (in-degree 1, distance from root ≤ 3):** Near-surface content with single path
  - For each under-connected node: show the single incoming edge and suggest adding a second path
  - Example (**illustrative placeholder ids only — `encounter_vault_door`, `loc_industrial_zone`, `quest_scavenger_guild`, `recipe_medical_basic` do not necessarily exist in the real data and must not be treated as real content to fix**; the real report will use whatever ids the actual graph surfaces):
    ```
    ## Under-Connected Content (Fragile Paths)

    ### Critical (deep content, single path)
    - {real_id} (distance: {N}, only path: {real_path})
      Suggestion: Add as possible encounter/reward elsewhere

    ### Warning (shallow content, single path)
    - {real_id} (distance: {N}, only path: {real_path})
      Suggestion: Add as encounter reward or NPC trade item
    ```

### Verification
- Unit test: node with in-degree 1 (not from root, distance > 3) → appears in Critical.
- Unit test: node with in-degree 3 → not under-connected.
- Unit test: root child with in-degree 1 → excluded (expected pattern).
- Integration test against real data — check results are plausible.
- `dotnet test` passes.

### Done when
- [ ] Under-connected analysis implemented with severity tiers
- [ ] Report includes fragile-path section with suggestions
- [ ] Root children correctly excluded
- [ ] ≥4 unit tests
- [ ] Integration against real data produces plausible results

---

## Step 6 — Add Content Coverage Metric & CI Tracking

### Goal
Establish a tracked metric for content reachability percentage and integrate into CI so regressions are caught.

### Implementation
- Create `Ashfall.Core.Tests/ContentAnalysis/ContentCoverageTests.cs`:
  - `[Fact] ContentCoverage_MeetsMinimumThreshold()`:
    - Runs full pipeline against actual `Assets/StreamingAssets/Data/`
    - Asserts `result.CoveragePercentage >= MINIMUM_COVERAGE`. **Correction:** "start with current baseline, e.g., 88%" was an invented placeholder number — the real starting threshold is unknown until Step 4's report is actually run once against real data (see Step 4's corrected Done-when: "real baseline captured"). Do not hardcode 88%, 90%, or any other number in code or this plan until it has been measured. Set `MINIMUM_COVERAGE` to whatever the first real run reports, not before.
    - Fails if a commit orphans content
  - `[Fact] ContentCoverage_NoNewOrphans()`:
    - Loads a committed orphan baseline file (`content_coverage_baseline.json`)
    - Compares current orphans to baseline
    - Fails if new orphans appear (existing orphans are grandfathered until fixed)
    - Passes if orphans decrease (someone fixed one)
  - `[Fact] UnderConnected_CriticalCount_DoesNotIncrease()`:
    - Tracks count of critical under-connected nodes
    - Fails if count increases
- Create `content_coverage_baseline.json` (committed to repo). **Correction: the numbers below are a schema example only, not real measured data** (real values come from the first actual run, per above):
  ```json
  {
    "schema_version": 1,
    "generated": "{iso_date_of_first_real_run}",
    "total_nodes": "{measured}",
    "reachable_nodes": "{measured}",
    "coverage_percentage": "{measured}",
    "known_orphans": ["{real_id_1}", "{real_id_2}"],
    "critical_under_connected_count": "{measured}"
  }
  ```
- Baseline update workflow:
  - Run `godot --headless --path . -- --content-coverage-report --update-baseline` to regenerate. **Note:** `--update-baseline` is a new sub-flag not yet specified in Step 4's CLI implementation — add it explicitly to Step 4's scope (as a modifier on `--content-coverage-report`) rather than introducing it here for the first time with no implementation step.
  - Commit updated baseline when orphans are intentionally added or fixed
  - PR review should check baseline changes

### Verification
- Deliberately add an orphan item to test data → `NoNewOrphans` test fails.
- Remove the orphan → test passes again.
- Decrease threshold below current coverage → `MeetsMinimumThreshold` passes (sanity).
- Set threshold above current coverage → test fails (proves enforcement works).
- `dotnet test` passes in steady state.

### Done when
- [ ] Coverage threshold test exists and enforces minimum
- [ ] No-new-orphans test exists with committed baseline
- [ ] Under-connected critical count tracked
- [ ] Deliberate orphan introduction triggers test failure
- [ ] Baseline file committed and documented

---

## Step 7 — Write Coverage Validation Tests

### Goal
Ensure the content coverage system itself is correct with dedicated tests, including a "golden test" that deliberately introduces an orphan and verifies detection.

### Implementation
- Create `Ashfall.Core.Tests/ContentAnalysis/ContentGraphBuilderTests.cs`:
  - `BuildsGraphFromMinimalCatalog()` — 3 items + 1 recipe → correct nodes/edges
  - `HandlesEmptyDirectory()` — no crash, empty graph
  - `HandlesMalformedJson()` — logs warning, skips file, continues
  - `ExtractsAllEdgeTypes()` — each edge type (produces, requires, triggers, references, unlocks, contains) tested
  - `NoDuplicateEdges()` — same reference mentioned twice doesn't create duplicate edge
  - `CountsMatchExpected()` — known input → known node/edge counts
- Create `Ashfall.Core.Tests/ContentAnalysis/ReachabilityAnalyzerTests.cs`:
  - `FullyConnectedGraph_100PercentReachable()`
  - `DisconnectedNode_MarkedUnreachable()`
  - `CyclicGraph_AllReachableIfRootConnects()`
  - `CyclicGraph_UnreachableIfNoRootConnects()`
  - `MultipleRoots_UnionReachability()`
  - `DistanceFromRoot_Correct()`
  - `PathFromRoot_ValidPath()`
- Create `Ashfall.Core.Tests/ContentAnalysis/GoldenOrphanTest.cs`:
  - Sets up a small synthetic catalog with one deliberate orphan (`item_test_orphan`)
  - Runs full pipeline → asserts `item_test_orphan` appears in unreachable list
  - Verifies the reason string mentions "no recipe produces it" (or equivalent)
  - Removes orphan → re-runs → asserts 100% coverage
  - This is the definitive proof that the system catches real orphans
- Create `Ashfall.Core.Tests/ContentAnalysis/UnderConnectedTests.cs`:
  - `SingleIncomingEdge_MarkedUnderConnected()`
  - `MultipleIncomingEdges_NotMarked()`
  - `RootChild_Excluded()`
  - `FragilityScore_CalculatedCorrectly()`

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all new tests pass.
- Golden orphan test proves detection works end-to-end.
- No existing tests broken.
- Full verification checklist passes.

### Done when
- [ ] ≥6 `ContentGraphBuilder` unit tests
- [ ] ≥7 `ReachabilityAnalyzer` unit tests
- [ ] Golden orphan test passes (proves detection)
- [ ] ≥4 under-connected analysis tests
- [ ] All tests pass
- [ ] Full verification checklist (5 steps) passes

---

## Summary

| Step | Deliverable | Layer | Risk | Depends on |
|------|-------------|-------|------|------------|
| 1 | `ReachabilityModel.cs` (formal definitions) | Core | None | — |
| 2 | `ContentGraphBuilder.cs` + `ContentGraph.cs` | Core | **Medium** — real field names differ per file and mix camelCase/snake_case; `quests.json`/`factions.json`/NPC data schemas were not fully verified and may not match assumed structure; silent-miss risk if a naming convention isn't handled | Step 1 |
| 3 | `ReachabilityAnalyzer.cs` (BFS + results) | Core | None (pure graph algorithm over Step 2's output; correctness depends on Step 2, not on data-shape risk directly) | Step 2 |
| 4 | `ContentCoverageReporter.cs` + CLI flag | Core/Host | Low — additive, but decide up front whether the CLI exit code or the `dotnet test` fact (or both) enforces the coverage threshold, to avoid duplicated/conflicting enforcement | Step 3 |
| 5 | Under-connected analysis + fragility scoring | Core | None | Steps 3–4 |
| 6 | CI enforcement + baseline tracking | Tests/CI | Low — baseline numbers must come from a real first run, not an assumed starting percentage | Steps 4–5 |
| 7 | 17+ validation tests + golden orphan test | Tests | None | Steps 1–6 |

**Exit criteria:** `dotnet test` enforces that content coverage does not regress below the baseline threshold, new orphans are caught before merge, and under-connected content is surfaced for authoring review. Running `godot --headless --path . -- --content-coverage-report` produces a full human-readable report of content health.

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real ASHFALL codebase (`Assets/StreamingAssets/Data/` — 296 real JSON files sampled directly; `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` read in full, 658 lines; `src/Host/HostCli.cs` and `src/Host/AssetRegistry.cs` inspected for CLI/coverage precedent). Corrections applied in place above; summarized here for traceability.

**Factual errors found and fixed:**
1. **"196 narrative encounter JSON files" was wrong on two counts.** 196 files do exist under `Assets/StreamingAssets/Data/narrative/`, but they are lore/flavor-text documents, not encounters. Files that actually contain encounter content are only 3 (`crossing_encounters.json`, `door_encounters.json`, `narrative_encounters.json`), located at the top level of `Data/`, not in a `narrative/` or `encounters/` subfolder (no such subfolder exists). Fixed in Motivation.
2. **"130+ data catalog JSON files" undercounted the real total** (296 files across `Data/`, `Data/narrative/`, `Data/documents/`, `Data/whitelists/`) while also not distinguishing it from the 55 top-level catalogs `CatalogIntegrityValidator`'s own selftest actually walks (non-recursively, skipping `narrative/`/`documents/`/`whitelists/` entirely). Fixed in Motivation with all three scopes named explicitly.
3. **"500+ IDs" and the per-prefix breakdown were unverified estimates presented as fact**, and in particular **`recipe_` and `encounter_` as ID prefixes are essentially fictional** — real recipe ids are verb-phrases (`craft_bandage`), and there is no `encounter_`-prefixed id scheme in the data at all, despite both being listed in `CatalogIntegrityValidator`'s defensive prefix list. Fixed in Motivation; Step 2 explicitly warns against filtering by these prefixes.
4. **All seven field names the graph-parsing logic in Step 2 was built around were invented and do not exist in any of the 296 real JSON files** (`available_encounters`, `prerequisite_flags`, `reward_items`, `triggers_encounter`, `trade_items`, `home_location`, `unlock_condition` — each verified via exact-string search returning 0 matches across the entire data tree). This was the single most severe defect in the plan: as originally written, Step 2's implementation would compile and run but silently build an empty or near-empty edge set, making every subsequent step's "reachability" analysis meaningless without anyone noticing (Step 2's own Done-when criteria did not require validating against real data, so the bug would have shipped past its own gate). Fixed by replacing every file's assumed schema with the real, verified field names, and flagging the files not sampled in this pass (`questline_master.json`, `factions.json`, NPC data) as needing direct verification before implementation rather than reusing the same invented names.
5. **False/misleading framing that the validator "does NOT check reachability" implied the reader should extend it** — this specific claim was actually true, but the plan's structure elsewhere (Step 2's "use existing reference-key list... 200+ prefixes") conflated `CatalogIntegrityValidator`'s two separate lists (`IdPrefixes`, ~200+ entries, used for Tier-1; `ReferenceKeys`, a smaller, different list, used for Tier-2) as if they were one "200+ prefix reference-key list." Fixed in Step 2 to name both lists correctly and note they serve different tiers.
6. **No other code anywhere in the repo does content-reachability graph analysis** — confirmed by repo-wide search. The two existing "coverage"-flavored precedents are unrelated: `AssetRegistry.RunFullCoverage` (art-asset coverage — does an item have a sprite, wired to `--asset-coverage-report`) and a map-pathfinding BFS in `WastelandMapSystem.cs` (physical travel between locations). This plan is genuinely new work; the plan now says so explicitly instead of only implying it, and correctly identifies `--asset-coverage-report`'s implementation as the CLI-wiring precedent to follow (verified against real `HostCli.cs` structure: `HostCliAction` enum → `Parse` → `PrintHelp` → `Main.cs` dispatch → `Run*` method).

**Scope/process issues found and fixed:**
7. **Mixed camelCase/snake_case field naming across files is real** (`narrative_encounters.json` uses camelCase, `crossing_encounters.json` uses snake_case for equivalent concepts) and was not addressed anywhere in the original plan — a parser written against one convention will silently miss references encoded in the other, producing false-positive orphans. Added as an explicit risk and requirement (per-file field maps, not one global table) in Step 2, and elevated Step 2's risk rating from "None" to "Medium" in the Summary table to reflect this.
8. **Fabricated example data throughout Steps 4–6** (`item_rusty_compass`, `npc_hermit_scientist`, `loc_hidden_lab`, `encounter_vault_door`, coverage numbers like "523 total nodes / 95.2%") was presented in a way that could be mistaken for real findings or a real starting baseline. All such examples are now explicitly labeled as illustrative placeholders, and Step 6's `MINIMUM_COVERAGE` / baseline JSON no longer hardcodes an invented starting percentage (88%) — the plan now requires measuring the real number from the first actual run before setting any threshold.
9. **Missing `--update-baseline` implementation step:** Step 6 referenced a `--content-coverage-report --update-baseline` CLI invocation that Step 4 (which owns the `--content-coverage-report` flag) never specified. Fixed by requiring `--update-baseline` to be added explicitly to Step 4's scope.
10. **Redundant/ambiguous enforcement point:** Step 4 specified a non-zero CLI exit code when coverage falls below a threshold, and Step 6 separately specified a `dotnet test` fact enforcing the same threshold, with no stated relationship between the two. Fixed by requiring an explicit decision (documented in Step 4's Done-when) on which mechanism is the actual gate, to avoid two independently-maintained threshold checks drifting apart.
11. **Missing risk/rollback notes:** the original plan rated every step's risk as "None," including a step (Step 2) whose success entirely depends on correctly guessing undocumented, inconsistent JSON schemas across ~296 files — demonstrably not a zero-risk step. The Summary table's risk column has been corrected step-by-step; this doc has no dedicated per-step "Risk/Rollback" subsections because the tool is genuinely read-only/non-mutating (confirmed: no step writes to `StreamingAssets/Data/` — only Step 6's baseline JSON is new, additive, and can be deleted with zero effect on other systems), so the primary risk category here is "silently wrong output," not "damages existing systems." That distinction is now made explicit in the Summary table and Motivation rather than asserting blanket "None" risk.
12. **Verification vagueness:** "Report format is valid markdown," "results are plausible," and hardcoded expected counts in an integration test against real (unknown) data were not runnable, falsifiable, or safe to hardcode ahead of time. Tightened in Steps 3–4 to assert structural properties (e.g., every unreachable id appears in the generated report text) rather than magic numbers that don't yet exist.

**What was already correct and left unchanged:** the overall reachability model (root nodes, BFS, in-degree fragility scoring) is sound and does not depend on any of the corrected factual errors — it is a generic graph algorithm that works regardless of which real field names feed it, once Step 2 is fixed. The `IFileIO`/`IJsonSerializer`/`ILog` constructor-injection pattern matches real Core port conventions. The claim that `CatalogIntegrityValidator` performs only existence-checking (never reachability) was correct and is the one accurate foundational claim the whole plan correctly builds on.
