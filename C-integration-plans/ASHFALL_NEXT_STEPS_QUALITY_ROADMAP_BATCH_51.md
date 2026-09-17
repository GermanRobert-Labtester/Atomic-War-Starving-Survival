# ASHFALL Quality Roadmap — Batch 51
## Theme: Data Authority Hardening — schema_version Rollout & Property Naming

**Priority:** HIGH (only 45 of 296 JSON files under `Assets/StreamingAssets/Data/` have schema_version; only 12 of 98 top-level files)
**Risk:** Low — additive metadata, no behavioral changes
**Prerequisite:** None (can run in parallel with Batches 49-50)

---

## Rationale

The data authority (`Assets/StreamingAssets/Data/`) is the single source of truth for all game content. Verified against the current repository (counts taken directly from the tree, not from prior reporting):
- `Assets/StreamingAssets/Data/` contains **296** `.json` files total: **98** top-level files, **196** files under `narrative/`, **1** under `documents/`, and **1** under `whitelists/`.
- Of the 296 total files, **45** already have a top-level `schema_version` field and **251** do not.
- Of the 98 top-level files specifically, **12** have `schema_version` and **86** do not. (The previously reported "~130+ top-level files" figure was wrong — the real top-level count is 98.)
- Of the 196 narrative files, **31** already have `schema_version` and **165** do not (the earlier "0 of 196" assumption used in downstream planning was wrong — nearly a sixth are already versioned; the remaining gap is 165 files, not 196).
- Property naming is inconsistent: core files use `camelCase` (`items.json`, `locations.json`) while newer files use `snake_case` (`disease_catalog.json`).
- No automated enforcement of the schema_version requirement exists yet.
- Without versioning, save migration and data format changes cannot be safely detected.

This batch adds `schema_version` to all core data files and establishes the naming migration path.

**Risk/rollback note:** every step below is additive JSON metadata plus loader fallback logic — no existing field is renamed or removed, and no save format changes. If a step causes `--data-integrity-selftest` or catalog tests to regress, the fix is to revert that step's JSON diffs and loader-fallback diff as one unit (they were committed together) rather than patching forward; each step should be its own commit specifically so this revert is a single `git revert` per step.

---

## Step 1 — Audit All JSON Files for schema_version

**Goal:** Produce a complete inventory of every JSON file in `Assets/StreamingAssets/Data/` with its current schema_version status and property naming convention.

**Implementation:**
1. Write a script (or use `jq`/`dotnet` tool) that walks `Assets/StreamingAssets/Data/` recursively, including `narrative/`, `documents/`, and `whitelists/`.
2. For each `.json` file, detect:
   - Has `schema_version`? (yes/no, value)
   - Top-level structure: array vs object envelope
   - Property naming: camelCase, snake_case, or mixed
3. Output a CSV/markdown table with one row per file plus a summary block broken out by directory (top-level, `narrative/`, `documents/`, `whitelists/`).

**Verification (concrete, testable):**
- The audit table's row count equals `find Assets/StreamingAssets/Data -name "*.json" | wc -l` (296 at time of writing; re-run the `find` command to get the current true count rather than trusting this number if the tree has changed).
- The audit's "has schema_version" total equals `grep -lR "schema_version" Assets/StreamingAssets/Data --include="*.json" | wc -l` (45 at time of writing).
- The audit's top-level-only subtotal equals `grep -l "schema_version" $(find Assets/StreamingAssets/Data -maxdepth 1 -name "*.json") | wc -l` (12 at time of writing) — this script command doubles as the audit's own top-level correctness check.
- The audit's narrative subtotal equals `grep -lR "schema_version" Assets/StreamingAssets/Data/narrative --include="*.json" | wc -l` (31 at time of writing).

**Done when:** Audit table committed to `docs/data_schema_audit.md`, and the four grep/find commands above, run against the committed audit's source tree state, produce numbers matching the audit's own summary block (self-consistency check, not a fixed hardcoded target — the exact numbers will drift as files are added).

---

## Step 2 — Add schema_version to Core Item/Location/Survivor Files

**Goal:** Add `schema_version: 1` envelope to the 4 most critical bare-array files: `items.json`, `locations.json`, `survivors.json`, `recipes.json`.

**Verified current state:** all four files are confirmed bare top-level JSON arrays with `camelCase` properties (e.g. `items.json` starts `[{"id": "dosimeter", "displayName": ...}]`; `recipes.json` starts `[{"id": "craft_bandage", "recipeName": ..., "resultItemId": ...}]`). None currently have `schema_version`.

**Risk correction:** there is no single canonical `ItemCatalogLoader`/`LocationCatalogLoader`/`RecipeCatalogLoader` class in the codebase today. `items.json` and `locations.json` in particular are read by multiple independent call sites that each parse the bare array directly — confirmed examples include `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs` (`ItemFiles`/`LocationFiles` arrays spanning 6 item files and 9 location files) and `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs` (reads `items.json` directly by path). Before implementation, enumerate every call site that opens these 4 files with a repo-wide search (e.g. `grep -rl "items.json\|locations.json\|survivors.json\|recipes.json" Assets/Ashfall.Core src`) — do not assume a single loader class exists to patch. This step is larger than "update the loader"; it is "update every direct consumer of these 4 files," which may be more than 4 call sites.

**Implementation:**
1. Transform each from bare array to envelope:
   ```json
   {
     "schema_version": 1,
     "collection_id": "items",
     "entries": [ ... existing array ... ]
   }
   ```
2. Update every Core call site enumerated above that consumes these files to handle the new envelope (not just "the loader" — there may be several).
3. Add fallback: if the top-level is still an array (pre-migration save/data), parse it as before.
4. Update `CatalogIntegrityValidator` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`) to check for `schema_version` presence — but only as part of Step 6's dedicated tier; do not duplicate that work here.

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles with 0 errors
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all tests pass, with an explicit assertion that catalog-loading tests touching items/locations/survivors/recipes still return the same entry counts as before the migration (regression guard, not just "pass")
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors
- Add a dedicated unit test per file (4 total) that loads a fixture copy of the file in bare-array form and asserts the fallback path returns identical parsed entries to the new envelope form

**Done when:** all 4 files have `schema_version`, every enumerated call site (not just one loader) handles both the bare-array and envelope formats, the 4 fallback-equivalence tests pass, and `--data-integrity-selftest` reports 0 errors.

---

## Step 3 — Add schema_version to Expansion Data Files

**Goal:** Add `schema_version: 1` to all expansion-specific JSON files that lack it.

**Corrected file inventory (verified against the actual repository tree — the original list named 10 files that do not exist under this project's naming and omitted files that do exist):**

- Holdfast (5 files exist, 5 lack schema_version): `holdfast_factions.json`, `holdfast_flavor.json`, `holdfast_items.json`, `holdfast_locations.json`, `holdfast_quests.json`. (`holdfast_trade_goods.json` does **not exist** — drop from scope.)
- DutyRoster (4 files exist, 4 lack schema_version): `duty_roster_locations.json`, `duty_roster_marks.json`, `duty_roster_quests.json`, `duty_roster_seasons.json`. (`duty_roster_assignments.json` and `duty_roster_events.json` do **not exist** — drop from scope.)
- YearOfAsh (6 files exist, 6 lack schema_version): `year_of_ash_events.json`, `year_of_ash_items.json`, `year_of_ash_locations.json`, `year_of_ash_quests.json`, `year_of_ash_radio.json`, `year_of_ash_survivors.json`. (`year_of_ash_timeline.json`, `year_of_ash_factions.json`, `year_of_ash_questlines.json` do **not exist** — drop from scope.)
- Crossing (5 files exist, 5 lack schema_version): `crossing_encounters.json`, `crossing_factions.json`, `crossing_items.json`, `crossing_locations.json`, `crossing_quests.json`. (`crossing_arbitration.json` does **not exist** — drop from scope.)
- StandingRecord (4 files exist, 4 lack schema_version): `standing_record_factions.json`, `standing_record_layouts.json`, `standing_record_memory.json`, `standing_record_quests.json`. (`standing_record_locations.json` and `standing_record_encounters.json` do **not exist** as data files — note that `Assets/Ashfall.Core/Warlords/WarlordDoctrineCatalog.cs` references `standing_record_locations.json` in its candidate file list, which is itself a latent bug in that loader; do not create the file just to satisfy that reference without confirming with the loader owner first.)
- Muster (2 files exist, 2 lack schema_version): `muster_epilogues.json`, `muster_witnesses.json`. (`muster_factions.json` does **not exist** — drop from scope.)
- Verdict (5 of 6 files still need it): `verdict_items.json`, `verdict_locations.json`, `verdict_npcs.json`, `verdict_questlines.json`, `verdict_radio.json` all lack `schema_version`. Only `verdict_data.json` already has it — the original claim that "Verdict already has schema_version" was true only for this one file out of six; the other five are in scope for this step.
- Foundry (2 of 4 files still need it): `foundry_faction.json` and `foundry_items.json` lack `schema_version`. `foundry_production.json` and `foundry_treaty_consequences.json` already have it and should be skipped. `foundry_accords.json` also already has it. The original claim that "Foundry already has schema_version" was true for 3 of 4 files but wrong for the other 2 — those 2 remain in scope.

For each in-scope file:
1. Wrap in envelope object if bare array; add `schema_version: 1` if object.
2. Update the corresponding catalog loader (verified names: `HoldfastCatalogLoader`, `DutyRosterCatalogLoader`, year-of-ash's loader, `CrossingCatalogLoader`/`CrossingQuestCatalogLoader`, `StandingRecordCatalogLoader`, `WitnessCatalogLoader`, `VerdictCatalogLoader`, `SilentFoundryCatalogLoader`/`SilentFoundryConsequenceCatalogLoader` — confirm the exact class before editing, some expansions split loading across more than one loader class) to handle the envelope.
3. Add fallback for bare format.

**Verification:**
- `dotnet test` — all expansion tests pass, and the total test count is unchanged or increased (never decreased) versus the pre-change baseline
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors

**Done when:** all 26 in-scope expansion data files listed above have `schema_version` (5 Holdfast + 4 DutyRoster + 6 YearOfAsh + 5 Crossing + 4 StandingRecord + 2 Muster) plus the 5 remaining Verdict files and 2 remaining Foundry files — 33 files total — and every corresponding loader has a passing fallback test.

---

## Step 4 — Add schema_version to Economy/Radio/Event Files

**Goal:** Add `schema_version: 1` to remaining data files: `economy_goods.json`, `radio.json`, `echoes.json`, `events.json`, `characters.json`, `dynamic_questlines.json`.

**Verified current state:** all 6 files exist under `Assets/StreamingAssets/Data/` and none currently have `schema_version`.

**Risk correction — loader existence not verified, do not assume:** a repo-wide search of `Assets/Ashfall.Core/` finds **zero** call sites referencing `radio.json`, `echoes.json`, `events.json`, `characters.json`, or `dynamic_questlines.json` by filename. Only `economy_goods.json` has a confirmed engine-agnostic loader (`GoodsCatalogLoader` in `Assets/Ashfall.Core/Economy/GoodsCatalog.cs`). This means:
- Either these 5 files are consumed only by legacy Unity code in `Assets/_Game/` (which per project rule 3/5 must not gain new logic and is being migrated away from, not extended) via a mechanism this search didn't catch (e.g. a config-driven filename rather than a literal string), or
- They are currently orphaned/unconsumed data files.
- **Before starting this step**, run `grep -rn "radio.json\|echoes.json\|events.json\|characters.json\|dynamic_questlines.json" Assets/ src/ --include="*.cs"` yourself to confirm which case applies for each file, since the answer changes what "update the loader" means (nothing to update vs. a legacy Unity call site that should not be touched per the migration-direction rule vs. a genuinely missing Core loader that must be written first).
- If a file turns out to have no Core loader at all, adding `schema_version` is still safe (it's additive JSON metadata with no consumer to break), but "update loaders" and "add fallback" for that file should be dropped from scope — there is nothing to update.

**Implementation:**
1. Same envelope pattern as Steps 2-3.
2. Update `GoodsCatalogLoader` for `economy_goods.json` (confirmed loader). For the other 5 files, follow the loader-existence check above before assuming any code needs to change.
3. Add fallback for bare format only where a real Core loader exists.

**Verification:**
- `dotnet test` — all pass, with no reduction in total test count
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors

**Done when:** all 6 files are versioned, the loader-existence check above has been performed and recorded (which files had real Core loaders vs. none), and any loader that does exist has a passing fallback test.

---

## Step 5 — Add schema_version to Narrative JSON Files (Batch)

**Goal:** Add `schema_version: 1` to narrative JSON files in `Assets/StreamingAssets/Data/narrative/`.

**Corrected count:** the directory contains **196** files (confirmed), but **31 already have `schema_version`** — the remaining gap is **165 files**, not 196. Re-verify this split at execution time with `grep -LR "schema_version" Assets/StreamingAssets/Data/narrative --include="*.json" | wc -l` since files may have been added since this audit.

**Critical scope-creep correction — this is not a single-loader change.** The original plan assumed one consumer, `NarrativeEncounterCatalogLoader`. That class only loads the single top-level `Assets/StreamingAssets/Data/narrative_encounters.json` file (note: no trailing directory — it is a sibling file to the `narrative/` folder, not inside it) and has nothing to do with the 196 files inside `Assets/StreamingAssets/Data/narrative/`.

The actual consumers of the `narrative/` subdirectory are dozens of small, individually-named catalog classes, each hand-written for one or a handful of files — confirmed examples include `BunkerBlueprintCatalog` (`bunker_blueprints_codex.json`), `BunkerContrabandCatalog` (`bunker_contraband_barter.json`), `BunkerCourtCatalog` (`bunker_court_verdicts_codex.json`), `BunkerGraffitiCatalog` (`bunker_graffiti_postings.json`), `BunkerMaintenanceCatalog` (`bunker_maintenance_glitches.json`), `CourierDispatchCatalog` (`courier_dispatches_master.json`), `CulinaryRationCatalog` (`culinary_ration_codex.json`), `DeadHandDirectiveCatalog` (`dead_hand_directives.json`), `RegionalTreatyCatalog` (`regional_treaty_protocols.json`), and `NarrativeBatchCatalog` (`jrnl_templates_cycle_d.json`, via `LoadJournalBatch`). There are at least 71 catalog-shaped test files in `Ashfall.Core.Tests/` targeting this style of one-class-per-file (or small-group-of-files) loader.

Many of the 196 files (confirmed via sampling, e.g. `activated_carbon_adsorption_records.json`) are **not consumed by any Core loader at all today** — they appear to be lore/codex reference data (bare arrays of `id`/`tags`/`prose` records) that may only be read directly by name at specific call sites, or not read at all yet. Treat "does this file have a Core consumer, and if so which class" as an open question per file, not a known fact.

Given this, "batch script adds the field to all 165 files" is safe and low-risk as pure JSON editing, but the follow-on claim "ensure the loader handles both formats" cannot be satisfied by touching one class. This step should be split:

**Implementation:**
1. Write a batch script that processes each of the 165 not-yet-versioned narrative `.json` files:
   - If top-level is array: wrap in `{ "schema_version": 1, "entries": [...] }` (do not hardcode `"encounters"` as the array key name — these files are not encounters; use a generic key like `entries` unless a specific existing loader already expects a specific key name, in which case match that loader's expectation).
   - If top-level is object without schema_version: add the field only, do not restructure.
2. Before running the batch script, produce a per-file map of "consumed by Core loader class X" vs. "no known Core consumer" by cross-referencing every filename in `narrative/` against `grep -rn "narrative\", \"<filename>"` across `Assets/Ashfall.Core/` and `src/`. Commit this map alongside the audit from Step 1.
3. For every file confirmed to have a real Core consumer, update that specific loader class (not `NarrativeEncounterCatalogLoader`) to accept the new envelope with a bare-array fallback, and add or extend that class's existing test file.
4. For files with no confirmed Core consumer, schema_version is still added (additive, no risk), but no loader change is needed or possible — do not fabricate a fallback test for code that doesn't exist.
5. Run `data-integrity-selftest` to confirm no regressions.

**Verification:**
- 165/165 previously-unversioned narrative files now have schema_version (196/196 total files versioned, matching the corrected count)
- `godot --headless --path . -- --data-integrity-selftest` — 0 errors
- Every catalog class identified in the per-file consumer map has its corresponding test file still passing after the format change

**Done when:** 196/196 narrative files have schema_version, the consumer map from sub-step 2 is committed, and every identified real consumer's tests pass — not just "narrative encounter tests," since `NarrativeEncounterCatalogLoader` is not the relevant loader for this directory.

---

## Step 6 — Enforce schema_version in CatalogIntegrityValidator

**Goal:** Add a new validation tier to `CatalogIntegrityValidator` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, confirmed 657 lines as of this writing) that fails if any JSON file in the data authority lacks `schema_version`.

**Naming correction:** the validator's existing five checks are documented in its class header as `REGISTRY`, `TIER-1`, `TIER-2`, `RANGES`, and `UNIQUENESS` — there is no numeric "TIER-3/4/5" naming today (RANGES and UNIQUENESS are named, not numbered). Calling the new check "TIER-6" implies five prior numbered tiers that don't exist. Name the new check consistently with the existing scheme, e.g. `SCHEMA_VERSION` (matching the `RANGES`/`UNIQUENESS` naming style), and update the class's doc comment to append it as a sixth bullet in the existing list rather than renumbering the first two as TIER-1/TIER-2 differently than they already are.

**Implementation:**
1. Add a new `SCHEMA_VERSION` check that walks every `.json` file under `Assets/StreamingAssets/Data/` (recursively, including `narrative/`, `documents/`, `whitelists/`) and requires a top-level `schema_version` integer field.
2. Allowlist: files in `documents/` and `whitelists/` subdirectories are exempt (both confirmed to exist, 1 file each today).
3. Report missing schema_version as an error (not warning).
4. This check can only be enabled after Steps 2–5 land — sequence it last within this batch and gate it behind confirmation that the Step 1 audit shows 0 remaining unversioned files outside the allowlist.

**Verification:**
- `godot --headless --path . -- --data-integrity-selftest` — still 0 errors (all files now versioned)
- Add a dedicated unit test that temporarily removes `schema_version` from a fixture file copy and asserts the validator reports exactly one new error referencing that file's path (not just "the test fails" — assert the specific error message/count)

**Done when:** the `SCHEMA_VERSION` check is enforced, the allowlist covers exactly `documents/` and `whitelists/` and no other exceptions, and the removal-test above passes.

---

## Step 7 — Property Naming Migration Plan (Document Only)

**Goal:** Document the snake_case migration path for the files still using camelCase properties. Do NOT execute the migration in this batch (too risky without coordinated loader changes).

**Verified:** `SystemTextJsonSerializer` (`Assets/Ashfall.Core/HostDefaults.cs`) sets `PropertyNameCaseInsensitive = true` — confirmed, so the claim that loaders already tolerate mixed case on read is accurate. Note this only covers case-insensitivity, not camelCase-vs-snake_case key *spelling* differences (e.g. `displayName` vs `display_name` are different strings, not just different case) — case-insensitivity alone does not make `items.json`'s `displayName` field readable as `display_name`. The migration plan must account for this: renaming a property is not covered by the existing case-insensitive comparer and requires an explicit dual-read (old name OR new name) in the loader, not just relying on `PropertyNameCaseInsensitive`.

**Implementation:**
1. Create `docs/property_naming_migration.md`.
2. List every file with camelCase properties (produce this list from the Step 1 audit's property-naming column rather than guessing a count — do not assert a specific number of files here without re-deriving it from the audit).
3. For each, list the affected property names and their snake_case equivalents.
4. Propose migration order (least-referenced files first, per the consumer-map methodology established in Step 5).
5. Define the migration protocol:
   - Loader reads both old and new property names explicitly (a real code change per loader — `PropertyNameCaseInsensitive` alone is insufficient, per the correction above, since this is a rename, not a case change)
   - Data file updated to snake_case
   - After one release cycle, remove old-name fallback

**Verification:**
- Document reviewed, migration order is safe
- No code changes
- Document explicitly distinguishes "case-insensitive read" (already true today) from "dual property-name read" (not yet true, required for any actual rename)

**Done when:** Migration plan document committed, ready for execution in a future batch, and its property list traces back to the Step 1 audit rather than being independently estimated.

---

## Summary

| Step | Target | Files Affected | Risk |
|------|--------|---------------|------|
| 1 | Audit | 0 (analysis) | None |
| 2 | Core 4 files | items, locations, survivors, recipes + all confirmed direct consumers (may exceed 4 code call sites) | Low |
| 3 | Expansion files | 33 files (26 previously-unversioned + 5 Verdict + 2 Foundry) + corresponding loaders | Low |
| 4 | Economy/radio/events | 6 files; only 1 (`economy_goods.json`) has a confirmed Core loader — verify the other 5 before assuming loader work is needed | Low |
| 5 | Narrative batch | 165 previously-unversioned files (196 total, 31 already versioned) + a per-file consumer map + N distinct loader classes (not 1) | Low–Medium (complexity is in discovery, not the JSON edit itself) |
| 6 | Validator enforcement | 1 file (`CatalogIntegrityValidator.cs`), new check named to match existing `RANGES`/`UNIQUENESS` style rather than "TIER-6" | Low |
| 7 | Naming plan | 0 (document) | None |

**End state:** 100% of the 296 data authority files (98 top-level + 196 narrative + 1 documents + 1 whitelists) have `schema_version`. Future format changes can be detected and migrated safely. Property naming migration is planned but not yet executed.

## Review Notes (Corrected)

This file was adversarially reviewed against the actual repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and corrected in place. Findings:

1. **Wrong top-level file count.** Original said "~130+ top-level JSON files, 12 with schema_version." The real top-level count is **98** (12 with, 86 without). The true grand total across the whole `Data/` tree (top-level + `narrative/` + `documents/` + `whitelists/`) is **296** files, **45** with `schema_version`, **251** without. Fixed throughout the header, rationale, Step 1, and the closing summary.

2. **Wrong narrative file baseline.** Original assumed 0 of 196 narrative files had `schema_version` ("Add schema_version to all 196 narrative JSON files"). Actual: **31 of 196 already have it**; the real gap is **165 files**. Fixed in Step 5 and the summary table.

3. **Step 3's expansion file list named 10 files that do not exist in this repository**, all fabricated or copied from a different naming scheme: `holdfast_trade_goods.json`, `duty_roster_assignments.json`, `duty_roster_events.json`, `year_of_ash_timeline.json`, `year_of_ash_factions.json`, `year_of_ash_questlines.json`, `crossing_arbitration.json`, `standing_record_locations.json`, `standing_record_encounters.json`, `muster_factions.json`. All ten were removed from scope and replaced with the actual file names verified to exist per expansion (Holdfast: 5 files; DutyRoster: 4; YearOfAsh: 6; Crossing: 5; StandingRecord: 4; Muster: 2).

4. **"Verdict already has schema_version" and "Foundry already has schema_version" were both false as blanket claims.** Verdict has 6 data files; only `verdict_data.json` has `schema_version` — the other 5 (`verdict_items.json`, `verdict_locations.json`, `verdict_npcs.json`, `verdict_questlines.json`, `verdict_radio.json`) do not and were incorrectly excluded from scope. Foundry has 4 files; `foundry_production.json`, `foundry_treaty_consequences.json`, and `foundry_accords.json` have it, but `foundry_faction.json` and `foundry_items.json` do not and were incorrectly excluded. Both expansions are now correctly split between "already done" and "in scope" files in Step 3.

5. **Step 5's implementation named the wrong loader class.** The plan said to "ensure `NarrativeEncounterCatalogLoader` handles both formats," but that class only loads the single sibling file `narrative_encounters.json` and has no connection to the `narrative/` subdirectory's 196 files. The actual consumers are dozens of individually-named catalog classes (`BunkerBlueprintCatalog`, `BunkerContrabandCatalog`, `BunkerCourtCatalog`, `CourierDispatchCatalog`, `CulinaryRationCatalog`, `DeadHandDirectiveCatalog`, `RegionalTreatyCatalog`, `NarrativeBatchCatalog`, and more — at least 71 catalog-test files exist for this style of loader), and a large share of the 196 files (confirmed by sampling) have **no Core loader at all today**. This was the most significant scope-creep risk in the plan and is now called out explicitly, with the step restructured to require a per-file consumer map before any loader code is touched.

6. **Step 6's "TIER-6" naming didn't match the real validator.** `CatalogIntegrityValidator.cs` (confirmed 657 lines) documents its five checks as `REGISTRY`, `TIER-1`, `TIER-2`, `RANGES`, `UNIQUENESS` — RANGES and UNIQUENESS are named, not numbered as TIER-4/TIER-5. Renamed the new check to `SCHEMA_VERSION` to match the existing naming convention instead of implying a numbering scheme that doesn't exist.

7. **Tightened vague Done-when criteria** across Steps 1, 2, 4, 6, and 7 to assert specific counts, specific test behaviors (e.g. "assert identical entry counts," "assert exactly one new error referencing the file's path"), or explicit provenance (e.g. "traces back to the Step 1 audit" instead of an independently guessed file count), rather than generic "passes" statements.

8. **Added a rollback/risk note** at the top of the document: each step should be committed independently so a regression can be reverted per-step rather than patched forward, since every step here is additive JSON + loader-fallback code with a clear single-commit revert boundary.

9. **Step 2's risk was understated.** The original implied one canonical loader per file; verified that `items.json`/`locations.json` in particular are consumed by multiple independent call sites (`WarlordDoctrineCatalog`, `DiseaseHeadlessDemo`, and others), so "update the loader" must become "enumerate and update every direct consumer."

10. **Step 4's loader assumption was unverified and mostly wrong.** Of the 6 files in scope, only `economy_goods.json` has a confirmed Core loader (`GoodsCatalogLoader`). No Core call site references `radio.json`, `echoes.json`, `events.json`, `characters.json`, or `dynamic_questlines.json` by filename — these may be legacy-Unity-only, or unconsumed. Added an explicit pre-step verification instruction instead of assuming "the radio loader" and "the events loader" exist as Core classes.
