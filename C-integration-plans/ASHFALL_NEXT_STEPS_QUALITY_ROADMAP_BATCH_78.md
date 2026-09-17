# ASHFALL — Quality Roadmap Batch 78

## Theme: Narrative Content Validation — Story Consistency & Dangling Reference Detection

**Priority:** MEDIUM-HIGH
**Risk:** Low-to-Medium — validation only, no content changes, but Steps 2/5/6 carry
architectural risk of being descoped mid-batch once Step 1's real schema findings are known,
and Steps 3/4 carry a demonstrated false-positive risk that requires cross-referencing C# code,
not just JSON.
**Estimated Effort:** 4–6 sessions (likely to run over if Step 1 finds the schema fragmentation
described below, since Steps 2/5/6 then require redesign rather than direct implementation)
**Prerequisites:** Familiarity with `CatalogIntegrityValidator` (**657 lines**, not 603 — verify
before citing; five informal tiers implemented as one procedural static class, not an enum or
plugin system)

---

### Motivation

The project has 196 narrative JSON files (confirmed count) in
`Assets/StreamingAssets/Data/narrative/`. The existing `CatalogIntegrityValidator`
(`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, **657 lines**, not 603 — corrected below)
performs five tiers of structural validation (registry, prefix resolution, reference key
resolution, range ordering, uniqueness) — but none of these tiers understand narrative-specific
semantics.

**Critical correction — the schema this batch was designed against does not exist.** The
original draft assumed a uniform "encounter" schema with `encounter_id`, `choices[]`,
`outcomes[]`, `flags_set`/`flags_clear`, `items_add`/`items_remove`, `next_encounter_id`,
`npc_id`, `location_id`. Direct inspection of real files (`dead_hand_directives.json`,
`night_watch_logbook.json`, `wasteland_expeditions_master.json`, and others) shows **no such
unified schema exists**. The 196 files are structurally heterogeneous per collection:
- `dead_hand_directives.json` uses `directive_id`, `transcript`, `archaeological_notes`, `tags`.
- `night_watch_logbook.json` uses `log_id`, `sentry_id`, `log_entry`, `tactical_action`, `tags`.
- `wasteland_expeditions_master.json` uses `expedition_id`, `zone`, `choices[]` (with
  `choice_id`, `label`, `skill_required`, `outcome_success`, `outcome_risk` as **prose strings**
  embedding effects as text, e.g. `"+800 Fuel"`, not structured, machine-parseable fields).
- None of the sampled files contain `encounter_id`, `flags_set`, `flags_clear`, `items_add`,
  `items_remove`, `next_encounter_id`, or a structured `location_id`/`npc_id` field of the kind
  this batch's schema assumes.

This means Steps 2–6 as originally drafted (encounter chain validation via `next_encounter_id`,
flag coverage via `flags_set`/`flags_read`, cross-catalog references via `npc_id`/`items_add`,
orphan detection via encounter reference graphs, graph export of `next_encounter_id` edges) are
built on a schema that is **aspirational, not real**. Every step below has been rewritten to
either (a) start from Step 1's actual schema audit before committing to specific field names, or
(b) target the narrative-adjacent systems that genuinely do have structured references (quest
JSON, encounter/expedition choice outcomes referencing items/locations by prefix) rather than
inventing a story-graph structure the data doesn't have.

Also corrected: there is no single "characters catalog" or "NPC catalog" file — NPC/character
data is split across `characters.json`, `survivors.json`, and per-expansion files such as
`verdict_npcs.json`. And `items.json`/`locations.json` are the core files among ~10+
expansion-scoped siblings (`holdfast_items.json`, `crossing_locations.json`, etc.) that must
also be included in any cross-catalog lookup set, or false positives will be rampant.

**Flag coverage false-positive risk is real and already partially manifesting.** Flags are
tracked through at least three separate, non-unified mechanisms in `Assets/Ashfall.Core/`:
`IFlagLedger`/`InMemoryFlagLedger` (the primary ledger, ~10 call sites across
`CensusBroadcastScheduler`, `VerdictCensusBroadcast`, `DiveInstanceRunner`,
`OrphanKnockWhitelist`), plus independent, unrelated `HashSet<string>`-based flag stores in
`CrossingQuestSystem` (`setFlags`/`HasFlag()`), `VerdictNpcSystem` (`ContainsFlag()` over a
caller-supplied collection), and `LocationLayoutSystem`/`LocationMemorySystem` (`rt.Flags`/
`_activeFlags`). `CatalogIntegrityValidator.KnownRuntimeIds` already hardcodes a manual
whitelist of `flag_verdict_*` ids specifically because they are set purely by C# code and would
otherwise be flagged as unresolved by the existing Tier-1 check — concrete, existing evidence
that a JSON-only flag scan produces false positives today, before this batch even adds TIER-7.
Step 3 is rewritten accordingly to make cross-referencing all of these C# flag stores mandatory,
not optional.

---

## Step 1 — Audit Narrative JSON Structure (Hard Prerequisite Gate)

**Goal:** Document the actual, heterogeneous schema(s) of narrative JSON files — per-collection
field names, choice/outcome formats (prose vs. structured), and any real cross-reference
fields — to determine which of Steps 2–6 are even buildable, and in what form.

**Correction — this step is now load-bearing, not a formality.** The original draft treated
Step 1 as a quick documentation pass feeding into an already-decided schema for Steps 2–6. In
reality, spot-checking `dead_hand_directives.json`, `night_watch_logbook.json`, and
`wasteland_expeditions_master.json` shows **no shared "encounter" schema exists** — field names
are per-collection (`directive_id` vs. `log_id` vs. `expedition_id`), and outcome effects in at
least the expeditions file are **free-text prose** (`outcome_success: "+800 Fuel"`), not
structured `flags_set`/`items_add` arrays. Step 1 must now produce a definitive verdict on
whether structured cross-references (encounter chains, flag mutations, item/NPC/location refs)
exist anywhere in the 196-file corpus in machine-parseable form, and if so, in which files —
because Steps 2–6 can only be built for the subset (if any) that has structured data. Steps
that target files with only prose effects must be redesigned as prose/regex heuristics (higher
false-positive risk, must be flagged as such) or dropped for those files.

**Implementation:**
- Read a representative sample across collection types, not just 10-15 files — the corpus has
  at least 3 structurally distinct families observed already (directive-log style, sentry-log
  style, expedition-choice style); sample enough files per family to characterize each, and
  enumerate how many *distinct* top-level schemas exist across all 196 files (grep top-level
  key sets per file and diff them, don't assume homogeneity).
- For each family found, document its actual fields, e.g.:
  - Directive/log style: `directive_id`/`log_id`, `timestamp_utc`/`recorded_day`, prose body
    fields (`transcript`/`log_entry`), `tags`. Generally no choices, no outcomes, no
    cross-references beyond `tags`.
  - Expedition/choice style: `expedition_id`, `zone` (free-text, not a `location_id` foreign
    key), `choices[]` with `choice_id`, `label`, `skill_required`, `outcome_success`/
    `outcome_risk` as **prose strings** describing effects, not structured effect objects.
- For each family, explicitly answer: does this family have a `next_encounter_id`-equivalent
  field enabling story-graph chaining? Does it have any field that could resolve against
  `items.json`/`locations.json`/an NPC catalog as a real foreign key (not free text)? Does it
  set or read any `flag_`-prefixed id in structured form?
- Identify every `schema_version` value present (per real-file finding, files consistently have
  `schema_version` + `collection_id` at the root — confirm this holds across all 196, not just
  the sample).
- Note that `npc_id`/`items_add`/`items_remove`/`flags_set`/`flags_clear`/`next_encounter_id`
  as literal field names were **not found** in the initial 3-file sample; confirm across a
  wider sample whether they exist in *any* file family before Steps 2–4 assume they do.
- Document which "catalog" files are the actual authorities for cross-reference validation:
  `characters.json` and `survivors.json` (NPC-ish data; no single "the NPC catalog") plus
  expansion-specific NPC files like `verdict_npcs.json`; `items.json`/`locations.json` plus
  their ~10 expansion-scoped siblings (e.g. `holdfast_items.json`, `crossing_locations.json`).

**Verification:**
- The documented schema covers every distinct top-level field set found across all 196 files,
  not just the sampled subset — verify by running a script that extracts and diffs the
  top-level key set of every file, and confirm the sample's families cover all of them.
- Every cross-reference field that is confirmed to exist in structured (non-prose) form is
  identified and categorized by target catalog; every field that only exists as a prose
  substring is explicitly called out as NOT machine-validatable without an NLP/regex heuristic,
  with the associated false-positive risk documented.

**Done when:** `docs/narrative/NARRATIVE_SCHEMA.md` exists documenting every real structural
family found (not a single hypothetical unified schema), which families (if any) support
structured cross-reference validation, and an explicit go/no-go recommendation for each of
Steps 2–6 based on what the real data supports. **Steps 2–6 must not proceed on the originally
assumed schema — they must be revised in place once Step 1 completes, using its findings.**

---

## Step 2 — Add TIER-6: Encounter/Node Chain Validation (Conditional on Step 1 Findings)

**Goal:** Ensure any chain-linking field that Step 1 confirms actually exists in structured
form (the original draft assumed `next_encounter_id`, which was **not found** in the sampled
files) resolves to a real target node somewhere in the narrative corpus.

**Risk — corrected from original draft:** This step assumed a `next_encounter_id` field exists
across the corpus and chains "encounters" into a story graph. Direct inspection of
`dead_hand_directives.json`, `night_watch_logbook.json`, and `wasteland_expeditions_master.json`
found no such field — the expedition file's `choices[]` have `outcome_success`/`outcome_risk`
as **prose strings**, not a link to another node. **This step must not be implemented until
Step 1 confirms which file family (if any) has a genuine structured chain-link field**, and
under what name. If Step 1 finds no such field exists anywhere in the 196 files, this step
should be descoped entirely rather than implemented against an invented field name that would
trivially "pass" by finding zero matches — a validator that never finds what it's looking for
provides no value and gives false confidence.

**Implementation (once Step 1 confirms a real chain-link field exists):**
- Extend `CatalogIntegrityValidator` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`) or add
  a companion `NarrativeIntegrityValidator` class. Note the existing validator has **no tier
  enum or plugin interface** — the five tiers are informal (a doc comment + inline logic inside
  one large `Walk()`/`Validate()` pair). Adding TIER-6 means adding new static lookup arrays
  (similar to the existing `IdPrefixes`/`ReferenceKeys`/`RangeKeys`) and new inline checks in
  the same procedural style, not slotting into a clean extension point — budget time for this;
  it is additive but not decoupled.
  - **Pass 1 — Registry:** Collect all node-identifying values (using whatever id field Step 1
    confirms — e.g. `expedition_id`, or none, depending on findings) across all 196 files.
  - **Pass 2 — Resolution:** Scan for the confirmed chain-link field. For each value found,
    verify it exists in the registry set.
  - Report violations as errors using the existing `CatalogIntegrityReport.Error(...)` method —
    note the report class already supports a `Warn(...)` method with a separate `Warnings`
    list, but it is currently **never called** anywhere in the validator; if TIER-6 findings
    should be warnings rather than hard errors (recommended for anything not 100%
    confidence-verified against real data), this batch is what finally exercises that unused
    code path — call `Warn()` explicitly, don't add a sixth caller of `Error()` by habit.
- Handle terminal/end nodes (no chain-link field) as valid.
- Handle `null`/empty chain-link values as valid.
- Integrate into the `--data-integrity-selftest` CLI verb (confirmed: this is dispatched from
  `src/Host/HostCli.cs`, a ~326-line file with a ~70-member `HostCliAction` enum and a linear
  `if (Has(args, "--xxx")) return HostCliAction.Yyy;` chain — adding a verb here is a simple,
  low-risk additive change, not a refactor, but note the file is already large).

**Verification:**
```
godot --headless --path . -- --data-integrity-selftest   # Must report 0 TIER-6 errors
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```
- Introduce a deliberately broken chain-link value in a test fixture; verify the validator
  catches it.
- Remove the broken fixture; verify clean pass.
- **This step is skipped entirely (not "done with zero findings") if Step 1 concludes no
  structured chain-link field exists in the corpus.** Record that decision explicitly rather
  than silently omitting the step.

**Done when:** Either (a) TIER-6 validation runs as part of data-integrity-selftest against a
confirmed real field name and passes on the current 196-file corpus, or (b) Step 1's findings
are documented as the reason this step is out of scope for the current data model, with a
recommendation for what schema change (if any) would be needed to make it buildable.

**Rollback:** New validator logic is additive to a static class; if TIER-6 proves unreliable
post-merge, gate it behind a feature flag or comment it out of the `Validate()` call chain
without touching the existing five tiers.

---

## Step 3 — Add TIER-7: Narrative Flag Coverage (High False-Positive Risk — Mandatory C# Cross-Reference)

**Goal:** Detect flags that are set but never checked (dead flags) and flags that are checked
but never set (impossible conditions), ensuring narrative flag logic is complete.

**Confirmed false-positive risk — this is not hypothetical.** Flags in this codebase are
tracked through **at least three separate, non-unified mechanisms**, not just JSON:
1. `IFlagLedger`/`InMemoryFlagLedger` (`Assets/Ashfall.Core/Flags/IFlagLedger.cs`) — the
   primary ledger, with real call sites in `CensusBroadcastScheduler.cs`,
   `VerdictCensusBroadcast.cs`, `DiveInstanceRunner.cs`, `OrphanKnockWhitelist.cs`.
2. `CrossingQuestSystem`'s own independent `HashSet<string> setFlags` + `HasFlag()`/`Add()` —
   entirely separate from `IFlagLedger`.
3. `VerdictNpcSystem.GetAvailable()`'s caller-supplied flag collection + `ContainsFlag()`, and
   `LocationLayoutSystem`/`LocationMemorySystem`'s own local flag `HashSet`s (`rt.Flags`,
   `_activeFlags`).

Concrete proof this already causes false positives today: `CatalogIntegrityValidator`'s
`KnownRuntimeIds` array already hardcodes `flag_verdict_eden_log_recovered`,
`flag_verdict_fuse_world_read`, `flag_verdict_shift_charter_restored`, and others specifically
*because* they are set only by C# code and would otherwise be flagged as unresolved by the
existing Tier-1 prefix-resolution check. **A TIER-7 that scans only JSON for `flags_set`/
`flags_clear`/`requirements.flags` — fields which, per Step 1, may not even exist under those
literal names — would misclassify every flag whose sole producer or consumer is C# logic as
dead or impossible.** Given at least 3 unrelated C# flag stores exist, and the JSON-side field
names are unconfirmed, this step's original false-positive risk was correctly anticipated by
the original draft's motivation section, but the draft did not actually address it in the
implementation — it must.

**Implementation:**
- Do not implement this step until Step 1 confirms which JSON field(s), if any, encode
  `flags_written`/`flags_read` in structured form. If no such structured field exists, this
  tier must be re-scoped to scanning free-text/prose fields for `flag_`-prefixed substrings
  (much higher false-positive risk — flag ids may appear in narrative flavor text without being
  a real read/write) or dropped.
- Collect `flags_written` and `flags_read` from **both** sources, not JSON alone:
  - JSON side: whatever structured fields Step 1 confirms exist.
  - C# side: every call site of `IFlagLedger.Set/Clear/IsSet`, plus `CrossingQuestSystem`'s
    `setFlags.Add()`/`HasFlag()`, plus `VerdictNpcSystem.ContainsFlag()`'s caller-supplied
    collections, plus `LocationLayoutSystem`/`LocationMemorySystem`'s local flag sets. Missing
    any one of these four sources reproduces the exact false-positive failure mode already
    seen in `KnownRuntimeIds`.
  - Also treat every id already present in `CatalogIntegrityValidator.KnownRuntimeIds` as a
    known C#-origin flag, not an orphan — that array is direct evidence of prior false
    positives and must be honored, not rediscovered from scratch.
- Validation rules:
  - **Dead flag (warning, using the report's existing but currently-unused `Warn()` method,
    not `Error()`):** flag in `flags_written` (JSON + C#) but not in `flags_read` (JSON + C#).
  - **Impossible condition (error):** flag in `flags_read` but not in `flags_written`, checked
    against the union of JSON and all four C# sources.
- Report format includes the source of each match (JSON file vs. specific C# call site) so a
  reviewer can immediately tell whether a "dead flag" finding is real or a cross-source miss.

**Verification:**
```
godot --headless --path . -- --data-integrity-selftest   # 0 TIER-7 errors (warnings acceptable)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```
- Test with a fixture that has an orphaned required flag with NO C#-side producer anywhere —
  verify error is raised.
- Test with a flag set only via `IFlagLedger.Set()` in C# and read only in JSON — verify NO
  false positive (this is the critical regression test; the original draft did not include it).
- Test with a flag set only via `CrossingQuestSystem.setFlags`/`VerdictNpcSystem`/
  `LocationLayoutSystem`'s local stores and read in JSON — verify NO false positive for each of
  the three non-`IFlagLedger` mechanisms individually.
- Run once against the full real corpus + full real `Assets/Ashfall.Core/` source and manually
  review every reported "impossible condition" error before treating the count as ground truth
  — given the confirmed existence of `KnownRuntimeIds` as a pre-existing patch for this exact
  problem, expect the first run to surface additional false positives requiring more whitelist
  entries, not a clean pass.

**Done when:** TIER-7 runs in data-integrity-selftest, cross-references JSON with all four
identified C# flag-tracking mechanisms (not JSON alone), reports dead flags as warnings via the
report's existing `Warn()` path, reports impossible conditions as errors only after excluding
everything already covered by `KnownRuntimeIds` or a new equivalent, and a human has reviewed
the first real run's findings rather than assuming a clean pass.

**Rollback:** Ship this tier as warning-only initially (never hard-fail CI on it) until at
least one full review cycle has run against the real corpus, given the demonstrated history of
false positives in this exact area.

---

## Step 4 — Add TIER-8: Cross-Catalog Reference Validation

**Goal:** Ensure all NPC, item, and location references within narrative files (where such
references exist in structured, non-prose form — per Step 1's findings) resolve to actual
definitions in their respective catalog files.

**Correction — "the NPC catalog" and "items.json"/"locations.json" as sole authorities are
both wrong assumptions.**
- There is no single NPC/character catalog file. NPC-ish data is split across
  `characters.json`, `survivors.json`, and per-expansion files such as `verdict_npcs.json`
  (referenced by `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`), plus survivor "field"
  files (`antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`,
  `expansion_survivor_fields.json`, `year_of_ash_survivors.json`). A lookup set built from only
  one of these will produce false "phantom NPC" errors for every valid npc id defined in
  another.
- `items.json` (398,123 bytes) and `locations.json` (79,473 bytes) exist at the expected paths
  but are **not the complete universes** — expansion-scoped siblings exist with the same
  suffix pattern: `black_flotilla_items.json`, `chemical_dependency_items.json`,
  `crossing_items.json`, `dose_items.json`, `foundry_items.json`, `greenhouse_items.json`,
  `holdfast_items.json` (items); `crossing_locations.json`, `deep_lore_locations.json`,
  `dose_locations.json`, `duty_roster_locations.json`, `holdfast_locations.json` (locations). A
  lookup set built only from the two core files will produce false "phantom item"/"phantom
  location" errors for every valid id defined only in an expansion file.

**Implementation:**
- Build lookup sets from **all** authority catalogs, not just the core files:
  - NPCs: union of `npc_id`-shaped values from `characters.json`, `survivors.json`,
    `verdict_npcs.json`, and the survivor-field files listed above. Confirm this list is
    complete by grepping `Assets/StreamingAssets/Data/*.json` for files containing `npc_id` or
    matching the `npc_` id prefix, rather than hardcoding the four/five known today — new
    expansions will add more.
  - Items: union of `item_id` values from `items.json` and every `*_items.json` sibling.
  - Locations: union of `loc_*` values from `locations.json` and every `*_locations.json`
    sibling.
- This mirrors how `CatalogIntegrityValidator`'s existing Tier-1/Tier-2 checks already work —
  they build registries from *all* files matching the relevant `DefinitionKeys`, not just one
  "main" file per category. TIER-8 must follow the same pattern to avoid reintroducing a
  problem the existing tiers already solved correctly.
- Scan narrative files for fields referencing these catalogs — but only for fields Step 1
  confirms exist in structured form. If item/NPC/location references only appear as
  substrings inside prose fields (as seen in the `wasteland_expeditions_master.json` sample,
  where `outcome_success` is a free-text string), a regex-based substring scan is a heuristic,
  not a validation, and must be labeled as such with an explicit false-positive/negative
  disclaimer (e.g. "Fuel" as a word inside prose text is not the same confidence level as a
  dedicated `item_id` field).
- Exclude IDs that match known expansion prefixes for content not yet shipped — configurable
  exclusion list, following the same pattern as the existing `KnownRuntimeIds` escape hatch
  (reuse that array/mechanism rather than inventing a second whitelist system).

**Verification:**
```
godot --headless --path . -- --data-integrity-selftest   # 0 TIER-8 errors
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```
- Test with a fixture referencing a nonexistent item — verify error.
- Test with a fixture referencing an item that only exists in an expansion `*_items.json`
  sibling (not the core `items.json`) — verify NO false positive. This is the critical
  regression test the original draft omitted.
- Test with a valid cross-reference — verify pass.

**Done when:** TIER-8 validates all cross-catalog references in narrative files against the
complete, unioned set of core + expansion catalog files, and passes cleanly on the current
corpus without requiring new entries in the runtime-id whitelist for content that already
exists in an expansion catalog.

**Rollback:** Additive validator logic; disable by removing the TIER-8 call site in
`Validate()` if the false-positive rate proves too high post-merge.

---

## Step 5 — Add Orphan Node Detection (Conditional on Step 2)

**Goal:** Identify narrative nodes that are never referenced by any other node's chain-link
field — potential dead content that players can never reach through normal narrative flow.

**Dependency correction:** This step is directly gated on Step 2, which is itself conditional
on Step 1 confirming a real chain-link field exists. If Step 2 is descoped because no such
field exists in the corpus, this step has no reference graph to operate on and must also be
descoped, or reframed around whatever weaker signal is available (e.g. "files never referenced
by `tags`" is not the same guarantee as "encounters never referenced by `next_encounter_id`"
and must not be presented as equivalent).

**Implementation (only if Step 2 ships):**
- From the node registry (Step 2), identify all node ids.
- From all confirmed chain-link fields (plus any "entry point" lists in world/quest
  definitions — verify these actually exist and under what name before assuming so), build a
  set of "referenced nodes."
- Also check for nodes referenced as quest start points, event triggers, or radio broadcasts —
  confirm these entry-point mechanisms exist in the real data/code before relying on them (e.g.
  check `Assets/Ashfall.Core/Radio/` and quest system code for how entry points are actually
  registered, rather than assuming a JSON list exists).
- Orphan = node that exists but is never a target of any reference and is not a registered
  entry point.
- Report as warnings via the validator's existing (currently unused) `Warn()` path, not
  `Error()` — some may be intentionally standalone (this matches the report class's existing
  `Warnings` list, which nothing currently populates).
- Provide summary statistics: total nodes, referenced nodes, entry-point nodes, orphan nodes
  (list each).

**Verification:**
```
godot --headless --path . -- --data-integrity-selftest   # Orphans reported as warnings
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```
- Test with a fixture containing an orphan — verify it's flagged.
- Test with a fixture where all nodes are reachable — verify no warnings.

**Done when:** Orphan detection runs as part of data-integrity-selftest (if Step 2 shipped) and
reports unreachable nodes as warnings with summary statistics, or is explicitly marked
out-of-scope alongside Step 2 if the underlying chain-link field doesn't exist.

**Rollback:** Additive, warning-only; safe to disable independently of Steps 2–4.

---

## Step 6 — Add Narrative Graph Visualization (Conditional on Steps 2 and 5)

**Goal:** Generate a machine-readable graph (DOT/Mermaid format) of node flow so designers can
visually inspect narrative structure, spot dead ends, and understand branching — **only for
whatever chain-link structure Steps 1/2 actually confirm exists.**

**Dependency correction:** Doubly conditional on Step 2 (chain-link field must exist) and Step 5
(orphan/entry-point classification for node styling). If the real corpus has no cross-file
chaining at all (plausible given the Step 1 findings — directive/log-style files show no
inter-file references), this step reduces to "visualize each collection's internal `choices[]`
branching within a single file," which is a materially smaller and different deliverable than
a 196-node story graph, and must be re-scoped explicitly rather than assumed away.

**Implementation (once Steps 2 and 5 confirm a real graph exists to visualize):**
- Add a CLI verb: `godot --headless --path . -- --narrative-graph-export [format]`
  - Formats: `dot` (Graphviz), `mermaid` (Mermaid markdown).
  - Per the confirmed CLI dispatch pattern in `src/Host/HostCli.cs` (a `HostCliAction` enum +
    linear `if (Has(args, "--xxx")) return HostCliAction.Yyy;` chain), this is a mechanical
    additive change: one enum member, one dispatch line, one help-text line, one handler case.
- Graph structure:
  - **Nodes:** each narrative node (labeled with its real id field + short title/label, per
    Step 1's confirmed schema — do not assume `encounter_id` + `title` exist).
  - **Edges:** each confirmed chain-link (labeled with choice text, truncated).
  - **Node styling:** entry points (green border), terminal nodes with no outgoing edges (red
    border), orphans (dashed border, per Step 5), normal (default border).
  - **Edge styling:** conditional edges (dashed), unconditional (solid) — only if a
    `requirements`-equivalent field is confirmed to exist per Step 1.
- Output to `docs/narrative/encounter_graph.dot` or `.mmd`.
- For large graphs, support per-chain subgraph export.

**Verification:**
- Generated DOT file structurally validates (node/edge counts match expectations). **Do not
  assume Graphviz (`dot -Tsvg`) is installed in the CI/dev environment** — the original draft's
  verification step assumed this without confirming; state explicitly that Graphviz-based
  rendering is an optional manual check, and the required, always-runnable check is a
  structural assertion (e.g. a unit test parsing the DOT file's own syntax) that doesn't depend
  on an external binary being present.
- Generated Mermaid file is checked for basic syntax validity (matching `graph`/`flowchart`
  keyword and balanced node/edge syntax) without requiring an external Mermaid renderer.
- Per-chain export produces a subset of the full graph.

**Done when:** The `--narrative-graph-export` verb works for whatever real chain structure
Steps 1/2/5 confirmed, produces valid DOT/Mermaid output verifiable without external tooling,
and correctly styles entry points, terminals, and orphans — or is explicitly descoped/reduced
in scope if no cross-file chain exists.

**Rollback:** Pure tooling addition; no risk to gameplay data or save compatibility. Remove the
CLI verb and handler if unused.

---

## Step 7 — Write Narrative Integrity Tests

**Goal:** Create xUnit tests that exercise whichever narrative validation tiers actually ship
(6, 7, 8 + orphan detection — some may be descoped per Steps 2/5/6's conditionality) with
deliberately broken fixture data, ensuring the validators catch real issues without false
positives against the real corpus.

**Correction — test list must track what actually ships, and must include the
false-positive regression tests called out in Steps 3 and 4.** The original draft's 9-test
list assumed all of TIER-6/7/8 + orphan detection ship unconditionally. Update the concrete
test list once Steps 2–6 are finalized; do not treat "9+ tests" as a fixed target if some tiers
are descoped — a smaller, honest test suite for what actually ships is better than padding
count with tests for tiers that don't exist.

**Implementation:**
- Create `Ashfall.Core.Tests/NarrativeIntegrityTests.cs`:
  - **Test: BrokenChain** (only if Step 2 ships) — fixture with a chain-link value pointing to
    a nonexistent target. Assert TIER-6 error is raised.
  - **Test: ValidChain** (only if Step 2 ships) — fixture with all links resolving. Assert no
    TIER-6 errors.
  - **Test: ImpossibleFlagCondition** — fixture requiring a flag never set anywhere (JSON or
    any of the four C# flag mechanisms identified in Step 3). Assert TIER-7 error.
  - **Test: DeadFlagWarning** — fixture setting a flag never checked anywhere. Assert TIER-7
    warning (via the report's `Warn()` path).
  - **Test: FlagSetByFlagLedgerReadByJson** (new — critical regression test per Step 3) — flag
    set only via `IFlagLedger.Set()` in C#, read only in JSON requirements. Assert NO false
    positive.
  - **Test: FlagSetByNonLedgerCSharpStore** (new — critical regression test per Step 3) — flag
    set only via `CrossingQuestSystem.setFlags`/`VerdictNpcSystem`/`LocationLayoutSystem`'s
    local stores, read in JSON. Assert NO false positive, for each of the three mechanisms.
  - **Test: PhantomItemReference** — fixture rewarding a nonexistent item (checked against the
    full unioned item catalog set, not just `items.json`). Assert TIER-8 error.
  - **Test: ValidExpansionItemReference** (new — critical regression test per Step 4) — fixture
    referencing an item that exists only in an expansion `*_items.json` sibling. Assert NO
    false positive.
  - **Test: PhantomNpcReference** — fixture referencing a nonexistent NPC (checked against the
    full unioned NPC set: `characters.json` + `survivors.json` + `verdict_npcs.json` + survivor
    field files). Assert TIER-8 error.
  - **Test: PhantomLocationReference** — fixture referencing nonexistent location (full unioned
    location set). Assert TIER-8 error.
  - **Test: OrphanNodeDetection** (only if Step 5 ships) — fixture with unreachable node.
    Assert warning.
  - **Test: FullCorpusClean** — run all shipped tiers against the real 196-file corpus **and**
    the real `Assets/Ashfall.Core/` source for C# flag/reference cross-checks. Assert zero
    hard errors (warnings acceptable). Given the demonstrated pre-existing false-positive
    history (`KnownRuntimeIds`), budget explicit time to triage the first real run's findings —
    do not assume this test passes on the first attempt.
- Use inline JSON fixtures (not file-dependent) for unit tests; the full-corpus test reads from
  `Assets/StreamingAssets/Data/narrative/` and `Assets/Ashfall.Core/` (for C# flag cross-checks).

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All tests pass
```
- Each negative test (broken fixture) must trigger the expected validation error.
- Each new false-positive regression test (flag/item cross-source tests) must NOT trigger a
  finding — these are the tests most likely to fail on first implementation, precisely because
  they test the failure mode this batch's own motivation section identifies as the main risk.
- The full-corpus test must pass (if it doesn't, fix the validator's exclusion/whitelist logic
  first — per the existing `KnownRuntimeIds` precedent — before assuming the *data* is broken;
  distinguish "the validator has a gap" from "the data has a bug" explicitly in the PR).

**Done when:** `NarrativeIntegrityTests.cs` exists with tests covering every tier that actually
shipped (not a fixed "9+" count), including the false-positive regression tests for flags and
cross-catalog references, all passing against the real corpus and real Core source.

**Rollback:** Test-only change; no production risk. If `FullCorpusClean` cannot be made to pass
without excessive whitelisting, that is a signal to reduce a tier's strictness (error → warning)
rather than force a pass by disabling the test.

---

## Summary Table

| Step | Deliverable | Type | Risk | Depends On |
|------|-------------|------|------|------------|
| 1 | `docs/narrative/NARRATIVE_SCHEMA.md` (real, per-family schema + go/no-go for Steps 2–6) | Documentation | None | — |
| 2 | TIER-6 chain validation — **conditional on Step 1 confirming a real chain-link field** | Code (Validator) | Low, or N/A if descoped | Step 1 |
| 3 | TIER-7 flag coverage — **mandatory 4-source C# + JSON cross-reference** | Code (Validator) | Medium — demonstrated false-positive history | Step 1 |
| 4 | TIER-8 cross-catalog references — **must union core + expansion catalog files** | Code (Validator) | Medium — false positives if catalog union is incomplete | Step 1 |
| 5 | Orphan node detection — **conditional on Step 2** | Code (Validator) | Low, or N/A if Step 2 descoped | Step 2 |
| 6 | `--narrative-graph-export` CLI verb — **conditional on Steps 2, 5; re-scoped if no cross-file graph exists** | Code (Tooling) | None | Steps 2, 5 |
| 7 | `NarrativeIntegrityTests.cs`, including false-positive regression tests | Tests | None | Steps 2–5 |

---

## Notes

- Step 1 is pure documentation but is now a **hard gate**, not a formality — it was found during
  review that the schema this batch assumed does not match the real 196-file corpus. All
  subsequent steps depend on its findings and must be revised in place once it completes.
- Steps 2–4 are only "parallel-safe" in the sense that they don't block each other's
  implementation; they are not independent of Step 1, and Steps 3/4 in particular must not be
  implemented against JSON alone (see false-positive corrections above).
- Step 5 depends on Step 2 (uses the node registry and reference graph) — if Step 2 is
  descoped, Step 5 is descoped too.
- Step 6 is a developer tool — it doesn't gate CI but aids content review, and its scope shrinks
  or changes shape depending on what Steps 2/5 actually ship.
- Step 7 must be the final step to test all prior work, and its test list must track what
  actually shipped rather than a fixed count.
- No narrative content is changed in this batch — only validation is added.
- If the full-corpus test (Step 7) reveals actual broken content, those fixes belong in a
  separate content-fix batch, not this validation batch.
- All new validation integrates into the existing `--data-integrity-selftest` CLI verb
  (confirmed: `src/Host/HostCli.cs`, ~326 lines, ~70-member enum + linear if-chain — additive
  and low-risk to extend, but the file is already large; be mindful of further bloating it).
- **Scope-creep guard:** if Step 1 reveals that most of the 196 files have no structured
  cross-references at all (plausible per the sampled evidence), resist expanding this batch
  into "redesign the narrative JSON schema to support story-graph validation" — that would be a
  content/data-model batch, not a validation batch, and is explicitly out of scope here.

## Review Notes (Corrected)

This document was adversarially reviewed against the real codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Summary
of factual errors and structural risks found and fixed:

1. **`CatalogIntegrityValidator.cs` line count was wrong.** Claimed 603 lines; actual file is
   **657 lines** (confirmed via `wc -l`). Corrected in the header and Motivation section.
2. **The proposed narrative schema does not match real data — this is the most severe finding.**
   Sampled real files (`dead_hand_directives.json`, `night_watch_logbook.json`,
   `wasteland_expeditions_master.json`) show **no unified "encounter" schema** with
   `encounter_id`/`choices`/`outcomes`/`flags_set`/`flags_clear`/`items_add`/`items_remove`/
   `next_encounter_id`/`npc_id`/`location_id` as the original draft assumed. Real files use
   per-collection field names (`directive_id`, `log_id`, `expedition_id`, etc.), and at least
   one family (`wasteland_expeditions_master.json`) encodes outcome effects as **free-text
   prose strings** (e.g. `"+800 Fuel"`), not structured, machine-parseable fields. Steps 2, 5,
   and 6 (encounter chains, orphan detection, graph export) were all rewritten to be
   **conditional on Step 1's findings** rather than implemented against the assumed schema, and
   Step 1 itself was upgraded from a formality to a hard prerequisite gate with an explicit
   go/no-go deliverable for each downstream step.
3. **Narrative file count (196) is correct** — verified via `find ... -name "*.json" | wc -l`.
   No change needed to this claim.
4. **The validator has no tier enum or plugin architecture.** The five existing tiers
   (REGISTRY, TIER-1, TIER-2, RANGES, UNIQUENESS) are implemented as one procedural static
   class with a single `Walk()`/`Validate()` pair and static lookup arrays
   (`IdPrefixes`/`DefinitionKeys`/`ReferenceKeys`/`RangeKeys`), not as separate methods or an
   `IValidationTier` interface. Adding TIER-6/7/8 is additive but not decoupled — corrected
   Steps 2–4 to describe the real extension mechanism instead of implying a clean plugin point.
5. **The report's `Warn()` method exists but is never called anywhere in the current
   validator** — every existing finding calls `Error()`. The original draft's warning/error
   distinction for TIER-6/7 orphan and dead-flag findings is achievable, but this batch is what
   finally exercises the unused `Warn()` path; corrected each relevant step to call this out
   explicitly so the implementer doesn't default to `Error()` out of habit, matching existing
   code patterns.
6. **TIER-7 flag coverage false-positive risk is real and already partially manifesting today**
   (the user's specific concern, confirmed). Flags are tracked through at least three separate,
   non-unified C# mechanisms beyond `IFlagLedger` (`CrossingQuestSystem.setFlags`,
   `VerdictNpcSystem.ContainsFlag()`, `LocationLayoutSystem`/`LocationMemorySystem`'s local
   flag sets), and `CatalogIntegrityValidator.KnownRuntimeIds` already hardcodes a manual
   whitelist of `flag_verdict_*` ids specifically to work around this exact false-positive
   mechanism for the existing Tier-1 check. Step 3 was rewritten to make cross-referencing all
   four flag-tracking mechanisms mandatory (not optional, as it was framed as "also check Core
   code" in the original draft), and Step 7's test list now includes explicit false-positive
   regression tests for each mechanism — the original draft had zero tests verifying the
   absence of false positives, only tests verifying true positives.
7. **"The NPC catalog" and "items.json"/"locations.json" as sole authorities were both wrong.**
   No single NPC catalog exists — NPC data spans `characters.json`, `survivors.json`,
   `verdict_npcs.json`, and multiple survivor-field files. `items.json`/`locations.json` are
   the core files among ~10+ expansion-scoped siblings (`holdfast_items.json`,
   `crossing_locations.json`, etc.). TIER-8 (Step 4) was rewritten to union all relevant
   catalog files, following the same pattern the existing Tier-1/Tier-2 checks already use, and
   Step 7 added a regression test for expansion-only items to catch the false positive the
   original draft would have produced.
8. **Step 6's verification assumed Graphviz (`dot -Tsvg`) is installed** without confirming
   this — corrected to make the required verification a structural/syntax check that doesn't
   depend on an external binary, with Graphviz rendering demoted to an optional manual check.
9. **Missing risk/rollback notes throughout** — added explicit rollback guidance per step
   (documentation steps are trivially revertible; validator steps should ship as warnings
   first given the demonstrated false-positive history; tooling steps are removable without
   touching gameplay data).
10. **Underestimated complexity, as the user specifically flagged for TIER-7.** The original
    "4-6 sessions" estimate assumed Steps 2-6 could be implemented directly against the assumed
    schema. With Step 1 now a hard gate that may descope Steps 2/5/6 entirely and substantially
    expand Steps 3/4's scope (four flag-tracking sources, N catalog files instead of 2), the
    estimate is corrected to note this batch will likely run long, and that is an acceptable,
    expected outcome of doing Step 1 honestly rather than skipping it.
