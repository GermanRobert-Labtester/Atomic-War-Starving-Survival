# ASHFALL — Quality Roadmap Batch 72

## Theme: Mod Support Architecture — Plugin Interface for Community Extensions

| Field | Value |
|-------|-------|
| **Priority** | LOW-MEDIUM |
| **Risk** | **HIGH — not Medium.** This is a new plugin trust boundary that accepts third-party file content, deserializes it, and merges it into the runtime catalog and save format. It has architectural blast radius across the data-integrity gate, the save/load contract, and every catalog loader. See the Risk section below and the corrected Summary Table for why this needed re-rating. |
| **Depends on** | Stable `CatalogIntegrityValidator`, mature data authority, `TuningConfig` (Batch 71) — **and, newly identified in this review, an extension to the `IFileIO` port**, which currently has no directory-listing method (see Step 3 correction). Mod discovery cannot be implemented "purely in Core, no engine coupling" against the *existing* port surface; the port itself needs a new method first. |
| **Blocks** | Community content, workshop integration, extensible narrative, replayable custom scenarios |
| **Estimated scope** | 7 steps across Core + Godot host — **underestimated; see Risk section. Realistically 9–11 steps** once the `IFileIO` extension, a security/sandboxing pass, and a save-migration compatibility step are accounted for. |

---

## Motivation

ASHFALL's architecture is already mod-friendly by design, and the following claims were
verified against the real codebase for this review:

- **Data authority is JSON** (Invariant 6) — confirmed; items, locations, recipes, events,
  encounters are all data-driven under `Assets/StreamingAssets/Data/`.
- **`CatalogIntegrityValidator`** (confirmed at `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`)
  validates every ID reference via a real 5-tier system (REGISTRY, TIER-1 prefix resolution,
  TIER-2 reference-key resolution, RANGES, UNIQUENESS) — this part of the motivation is accurate.
  **However**, the actual `IdPrefixes` array has **239 entries**, not "200+" as a loose
  approximation (technically consistent with "200+" but worth citing the real number), and more
  importantly: **the validator is a single-pass, whole-directory scanner** (`Validate(string
  dataDirectory, IFileIO files)` walks every `*.json` file in one directory via
  `Directory.GetFiles`, called directly — bypassing `IFileIO`, which has no listing method).
  It has **no concept of "which file/mod introduced this ID"** beyond the JSON file name already
  being part of the error path string. Attributing an error to a *mod* (Step 5's design) requires
  either (a) running the validator once per mod's isolated file set and diffing, or (b) adding a
  provenance-tracking layer the validator does not have today. This is a real, non-trivial gap —
  see Step 5's correction.
- **ID-prefix system** (239 confirmed prefixes: `item_`, `loc_`, `event_`, `recipe_`, etc.)
  provides natural namespacing — accurate.
- **CatalogLoaders already discover and merge multiple JSON files from disk** — accurate in the
  narrow sense that individual loaders (e.g. `YearOfAshCatalogLoader`) read multiple named files,
  but none of them do *directory discovery* (glob/scan an unknown folder for unknown files) —
  every existing loader reads a fixed, known filename. `ModLoader.DiscoverAndLoad(modsRootPath)`
  needs to scan an *unknown* directory tree for *unknown* subfolders, which is a new capability,
  not an extension of an existing pattern.
- **Godot supports `.pck` resource packs** — correctly scoped as host-layer/out-of-scope in the
  Future Work section; not evaluated further here since it isn't part of this batch's deliverables.

What's missing is a formal contract: how mods declare themselves, how they're loaded in
deterministic order, how conflicts are detected and resolved, and how saves record which mods
were active. This batch establishes that contract **and, per the corrected risk rating, must also
establish a security/trust boundary — mods are third-party code-adjacent content, and even
"just JSON" mods can carry a real attack surface (path traversal via `data_additions` relative
paths, resource exhaustion via unbounded dependency graphs, disk-fill via unbounded catalog
sizes) that the original draft does not mention anywhere.**

---

## Step 1 — Design Mod Manifest Format (`mod_manifest.json`)

### Goal
Define the canonical `mod_manifest.json` schema that every mod must include at its root, enabling discovery, dependency resolution, and conflict detection.

### Implementation
- Schema definition (documented in `docs/modding/manifest_schema.md`):
  ```json
  {
    "schema_version": 1,
    "mod_id": "string (snake_case, globally unique, prefix: mod_)",
    "display_name": "string (human-readable)",
    "version": "string (semver: major.minor.patch)",
    "min_game_version": "string (semver)",
    "max_game_version": "string (semver, optional)",
    "authors": ["string"],
    "description": "string",
    "dependencies": [
      { "mod_id": "string", "min_version": "string", "optional": false }
    ],
    "conflicts": ["mod_id strings that are incompatible"],
    "data_additions": ["relative paths to new catalog JSON files"],
    "data_overrides": [
      { "target_file": "string", "target_id": "string", "override_fields": {} }
    ],
    "load_priority": 0
  }
  ```
- Mod IDs must follow the `mod_` prefix convention and pass `CatalogIntegrityValidator` prefix rules.
- `data_additions` — paths to new JSON catalogs (new items, events, locations) merged after core.
- `data_overrides` — targeted field-level overrides of existing definitions (explicit, auditable).
- `load_priority` — integer; higher loads later (can override earlier mods); ties broken alphabetically by `mod_id`.

### Verification
- Schema document is complete and internally consistent.
- Example mod manifests (3 examples: item-addition mod, event-addition mod, override mod) validate against schema.
- Manifest schema is compatible with `SystemTextJsonSerializer` deserialization — verify concretely by deserializing each of the 3 example manifests through `SystemTextJsonSerializer.Deserialize<ModManifest>(json)` in a throwaway test/spike and confirming no exception and all fields populate, not just "compatible" as an assertion.

### Done when
- `docs/modding/manifest_schema.md` committed with full field documentation.
- Three example manifests in `docs/modding/examples/` parse without error — mechanically checkable: each example round-trips through `SystemTextJsonSerializer` with zero deserialization exceptions and every documented field present in the deserialized object.
- **Corrected from "schema review confirms no ambiguity"** (unfalsifiable): the schema doc explicitly answers, in writing, these five concrete questions, since "no ambiguity" cannot be checked mechanically: (1) what happens if two mods declare the same `mod_id`? (2) is `mod_id` case-sensitive? (3) what characters are legal in `mod_id` beyond snake_case? (4) what happens if `load_priority` ties and `mod_id`s are also equal (can't happen if `mod_id` is unique, but state that explicitly)? (5) is `data_overrides.override_fields` allowed to add fields not present in the target definition, or only overwrite existing ones?

---

## Step 2 — Design Mod Loading Order and Conflict Resolution

### Goal
Define the deterministic algorithm that discovers mods, resolves dependencies, detects conflicts, and produces a final ordered load list — documented and reviewable before implementation.

### Implementation
- Document the algorithm in `docs/modding/load_order.md`:
  1. **Discovery**: scan mod directories (configurable root, default `mods/`). Each subfolder with a valid `mod_manifest.json` is a candidate.
  2. **Validation**: parse each manifest; reject any with `schema_version` > supported, missing required fields, or invalid `mod_id` format.
  3. **Dependency resolution** (topological sort):
     - Build dependency graph from `dependencies` arrays.
     - Detect cycles → reject all mods in the cycle with clear error.
     - Detect missing required dependencies → reject dependent mod with error listing what's missing.
     - Optional dependencies: load if present, skip gracefully if absent.
  4. **Conflict detection**: if mod A lists mod B in `conflicts` (or vice versa), reject the lower-priority mod (or both if same priority) with error.
  5. **Final ordering**: topological order respecting `load_priority` as tiebreaker within each dependency tier.
  6. **Override merging**: `data_overrides` apply in load order — last writer wins per field (logged).
  7. **Output**: ordered `List<ModManifest>` ready for catalog merge.
- Define error reporting format: structured list of `{ mod_id, error_type, message }`.
- Define determinism guarantee: same set of mods always produces the same load order (no filesystem-order dependence).

### Verification
- Algorithm document reviewed for edge cases: circular deps, diamond deps, conflicting overrides.
- Worked examples in document cover: 0 mods, 1 mod, 3 mods with chain dependency, 2 conflicting mods, missing dependency.
- Determinism guarantee is explicitly stated and testable — concretely: the doc states that load order is a pure function of (mod manifests, dependency edges, load_priority, mod_id) with no dependence on filesystem enumeration order, OS, or wall-clock time, and this claim is checked in Step 7 by a test that shuffles the input discovery order and asserts identical output order.

### Done when
- `docs/modding/load_order.md` committed with algorithm, examples, and error taxonomy.
- **Corrected from "peer review confirms no ambiguity" (unfalsifiable):** the doc explicitly answers these edge cases in writing, each with a worked example: (1) a mod conflicts with itself (self-reference in `conflicts`) — documented as either "ignored" or "rejected," pick one; (2) `load_priority` values are negative — is that legal?; (3) a mod depends on a mod that is present but was rejected during validation (e.g. for a corrupt manifest) — does the dependent mod get rejected too, or does it see the dependency as "missing"?; (4) two mods both list `load_priority: 0` and neither depends on the other — confirmed alphabetical-by-`mod_id` tiebreak, with a worked example showing the exact resulting order for 3 same-priority mods.
- Algorithm can be implemented directly from the document (no design decisions deferred) — verified by having the Step 3 implementer flag, in the Step 3 PR description, any point where they had to make a decision the document didn't specify; zero such flags is the passing condition, not a subjective read-through.

---

## Step 3 — Implement `ModLoader` in Core (Discover, Validate, Merge Catalogs)

### Goal
Implement `Assets/Ashfall.Core/Modding/ModLoader.cs` — the engine-agnostic mod discovery and loading pipeline as designed in Steps 1–2.

### Implementation
- Namespace: `Ashfall.Core.Modding`.
- Classes:
  - `ModManifest` — deserialization target for `mod_manifest.json`.
  - `ModLoadResult` — success/failure with ordered manifest list + error list.
  - `ModLoader` — stateless, receives `IFileIO`, `IJsonSerializer`, `ILog` via constructor.
  - `ModDependencyResolver` — topological sort + conflict detection (pure function).
- `ModLoader.DiscoverAndLoad(string modsRootPath)` returns `ModLoadResult`. **Ordering correction:**
  this step implements *discovery, manifest parsing, dependency resolution, and load ordering
  only.* It must NOT implement catalog merging (additions/overrides) yet — that requires the
  permission model from Step 4, which doesn't exist until after this step. The original draft's
  "Catalog merge: after loading, returns merged catalog entries (additions appended, overrides
  applied)" bullet is removed from this step and moved to Step 4, where it belongs alongside the
  permission checks that must gate it. Merging data before the sandbox exists means there is a
  window where a mod could silently overwrite core data with no guard — defer merge entirely.
- All string comparisons use `StringComparer.Ordinal` (determinism, Invariant 4).
- No `Godot.*`, `UnityEngine.*`, `JsonUtility` references (Invariant 1).
- Log each step: discovery count, validation failures, load order.

### Correction — `IFileIO` does not support directory discovery
Verified against `Assets/Ashfall.Core/Ports.cs`: the `IFileIO` interface exposes only
`DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, and `Combine`. **There is no
method to enumerate subdirectories or list files matching a pattern.** `ModLoader.
DiscoverAndLoad(modsRootPath)` as specified requires scanning `mods/` for an unknown, variable
set of subfolders — this is not achievable through the current port surface.

Note that `CatalogIntegrityValidator` has this exact same need today and solves it by calling
`System.IO.Directory.GetFiles` directly, bypassing `IFileIO` entirely (confirmed at
`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, `Validate()` method) — i.e., the existing
codebase already has an un-ported direct-`System.IO` dependency in Core for this exact use case.
This batch must not repeat that shortcut for new code, or the plan's own "no direct `System.IO`"
verification bullet becomes unenforceable and self-contradicted by precedent.

**Required fix, added as a new prerequisite sub-step:**
- Extend `IFileIO` with `IReadOnlyList<string> ListDirectories(string path)` and
  `IReadOnlyList<string> ListFiles(string path, string searchPattern)`.
- Implement both on every existing `IFileIO` adapter (Godot's default core adapter at minimum;
  confirm whether a distinct Unity adapter exists before assuming none needs updating — AGENTS.md
  states the Unity `IFileIO` adapter is **MISSING** entirely, so this is Godot-only for now).
- This is an **interface change to a port already consumed by other code** — audit all existing
  `IFileIO` implementations and call sites before merging, since adding interface members breaks
  any implementer that doesn't get the new methods added. This should be its own reviewable
  commit, separate from `ModLoader` itself, so a regression is isolated to the port change.

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- Unit tests (in Step 7) will exercise the loader; at this step, verify compilation and API surface.
- Static analysis: 0 engine references in `Assets/Ashfall.Core/Modding/`.
- **New:** confirm every existing `IFileIO` implementer still compiles after the interface
  extension (a mechanical build-break check, not optional — this is a breaking interface change).

### Done when
- `IFileIO` extended with directory-listing methods; all existing implementers updated and building.
- `ModLoader.cs`, `ModManifest.cs`, `ModLoadResult.cs`, `ModDependencyResolver.cs` compile in Core.
- API matches the design documents from Steps 1–2 exactly.
- No engine coupling; uses only ports (`IFileIO`, `IJsonSerializer`, `ILog`).

### Risk / Rollback
Risk: **Medium-High** — this step changes a shared interface (`IFileIO`) consumed by every host
and every catalog loader. A mistake in the new methods' semantics (e.g. relative vs. absolute
path handling, trailing-slash inconsistency, symlink behavior differences between Godot's
`res://`/`user://` virtual filesystem and a plain OS path) could silently break unrelated,
already-shipped file access elsewhere in Core.
- **Rollback plan:** land the `IFileIO` extension as its own commit before any `ModLoader` code
  exists, so it can be reverted independently. Add characterization tests for the new methods
  against the existing Godot adapter *before* `ModLoader` depends on them.

---

## Step 4 — Add Mod Sandbox (ADD vs OVERRIDE Permissions) and Catalog Merge

### Goal
Implement a permission model that lets mods add new content freely but requires an explicit `"override": true` flag to modify existing core definitions — preventing accidental overwrites. Implement the actual catalog merge (deferred from Step 3) gated behind this permission model.

### Implementation
- Extend `ModManifest` with a `permissions` field:
  ```json
  "permissions": {
    "can_add_definitions": true,
    "can_override_definitions": false,
    "can_add_catalogs": true
  }
  ```
- Default permissions: can add, cannot override (safe default).
- Catalog merge (moved here from Step 3, now correctly gated): after `ModLoader` resolves load
  order, apply `data_additions` and `data_overrides` per mod, in order, subject to:
  - If a mod's `data_additions` introduces an ID that already exists in core → reject with error unless `can_override_definitions: true` AND the ID is listed in `data_overrides`.
  - If a mod's `data_overrides` targets a core ID but `can_override_definitions` is false → reject with error.
  - New IDs from mods must use the `mod_` prefix OR the standard prefix with a mod-namespace suffix (e.g., `item_mod_survpack_bandage`).
- **New — path-traversal guard (security gap identified in this review):** `data_additions` and
  `data_overrides.target_file` are relative paths supplied by untrusted third-party manifest
  content. Before resolving any such path through `IFileIO`, validate that the resolved absolute
  path stays within the mod's own declared root directory — reject any path containing `..`
  segments, absolute-path prefixes, or symlink escapes. Without this check, a malicious manifest
  could declare `"data_additions": ["../../../../StreamingAssets/Data/items.json"]` and overwrite
  arbitrary game data or, depending on host file permissions, arbitrary files on disk. This is not
  a hypothetical for a mod system — path traversal via archive/manifest content is one of the most
  common real-world modding-framework vulnerabilities. Add `ModSandboxValidator.ValidatePathIsWithinModRoot(string modRoot, string relativePath)` as a mandatory gate before any file read tied to mod content.
- Add `ModSandboxValidator` class that checks permission compliance before merge is applied.
- Log all permission checks at `ILog.Info` level for auditability.

### Verification
- Unit test: mod with `can_override_definitions: false` trying to override → rejected with clear error.
- Unit test: mod with `can_override_definitions: true` + valid `data_overrides` → accepted.
- Unit test: mod adding new ID with proper prefix → accepted without override permission.
- Unit test: mod adding ID that collides with core without override permission → rejected.
- **New:** unit test: mod manifest with a `data_additions` path containing `../` → rejected before any file read is attempted, with a clear "path traversal rejected" error (not a generic file-not-found).
- **New:** unit test: mod manifest with an absolute path (e.g. `/etc/passwd` or `C:\Windows\...`) in `data_additions` → rejected.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass.

### Done when
- `ModSandboxValidator.cs` implemented and tested, including the path-traversal guard.
- Default-deny for overrides prevents accidental core data corruption.
- Catalog merge only ever runs after permission + path-safety checks pass — no code path merges mod data unconditionally.
- Permission model is documented in `docs/modding/permissions.md`, and the documentation explicitly states the path-traversal threat model and mitigation.

### Risk / Rollback
Risk: **High** — this is the step where untrusted third-party content first touches the runtime
catalog and, if the path-traversal guard is wrong or incomplete, the filesystem outside the mod's
own directory. This is the single highest-risk step in the batch and deserves isolated review
independent from the rest of the pipeline (see the Cross-Tool QA Rule in `AGENTS.md` — this step
alone introduces well over 2 new coupled variables: permission flags × override targets × path
resolution, and should be implemented by one tool/person and security-reviewed by a different one,
per that rule).
- **Rollback plan:** ship with `can_override_definitions` hardcoded to `false` for an initial
  release (i.e., mods can only add, never override, no matter what the manifest requests) as a
  reduced-scope fallback if the override + path-safety logic can't be fully verified in review.
  This still delivers additive modding (the lower-risk 80% of the value) while deferring the
  higher-risk override capability to a later, separately-reviewed batch.

---

## Step 5 — Add Mod Validation Pipeline (CatalogIntegrityValidator After Merge)

### Goal
Run the existing `CatalogIntegrityValidator` against the merged catalog (core + mods) to ensure no mod introduces dangling references, duplicate IDs, or invalid ranges.

### Correction — the validator has no per-source attribution mechanism today
Verified against `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`: `Validate(dataDirectory,
files)` takes a single directory, enumerates every `*.json` file in it with
`Directory.GetFiles`, and produces one flat `CatalogIntegrityReport` with error strings that
embed the *source JSON file name* (via the `ctx.File` walk context) but have **no field or
structure representing "which mod owns this file."** The claimed error format
(`[mod_foo] TIER-2: unresolved reference '...' in mod catalog 'mod_foo_items.json'`) is
achievable, but only by adding a *new* wrapping layer that maps "file name" → "mod_id" after
the fact — the validator itself must not be modified to preserve its existing single-directory
contract (used today by `--data-integrity-selftest` against core data only). The implementation
approach below is corrected to reflect this.

### Implementation
- Add `ModIntegrityPipeline` class in `Ashfall.Core.Modding`:
  - Materializes the merged catalog (core + all approved mods in load order) into a **temporary
    merged data directory** (or an equivalent in-memory `IFileIO` view, if one is built for
    testing) so `CatalogIntegrityValidator.Validate()` can run against it completely unmodified —
    do not fork or edit the validator itself.
  - Maintains its own `Dictionary<string /* source file name */, string /* mod_id */>` built
    during the merge step, so that after `Validate()` returns its flat error list, the pipeline
    can post-process each error string to prepend `[mod_id]` by matching the embedded file name.
    This is a string-based attribution shim, not a structural change to the validator — call this
    out explicitly in the implementation so a future maintainer doesn't assume the validator has
    native mod-awareness.
  - Runs this against the merged result.
- Error attribution: if a new ID from `mod_foo` references `item_nonexistent`, the error message includes `[mod_foo] TIER-2: unresolved reference 'item_nonexistent' in mod catalog 'mod_foo_items.json'` — achieved via the post-processing shim above, not a validator change.
- If any mod causes validation failures, that mod is rejected (not loaded) and the merge is re-run without it.
- Cascade check: rejecting mod A might fix mod B's errors (if B depended on A's broken data). Re-validate iteratively until stable or all failing mods removed. **Add an explicit iteration cap** (e.g. 2× the number of loaded mods) — without one, a pathological mod set (e.g. a cycle of mods that each "fix" and "break" each other's validity across merge orders) could loop indefinitely. Log and hard-fail if the cap is hit rather than looping forever.
- Final merged catalog that passes all five tiers is the runtime catalog.

### Verification
- Unit test: mod with valid additions → passes all 5 tiers.
- Unit test: mod with dangling reference → TIER-2 failure, mod rejected, core catalog intact.
- Unit test: mod with duplicate ID → UNIQUENESS tier failure, mod rejected.
- Unit test: cascade rejection (mod B depends on mod A which fails) → both rejected.
- **New:** unit test: pathological mod set that would loop without the iteration cap → pipeline hard-fails with a clear "cascade limit reached" error instead of hanging.
- `godot --headless --path . -- --data-integrity-selftest` still passes (no mods loaded = same as before) — this specific command only validates core data and is unaffected by this step; it is not a substitute for testing the mod-aware pipeline itself.

### Done when
- `ModIntegrityPipeline.cs` implemented with an iterative validation loop that has a documented, tested iteration cap.
- `CatalogIntegrityValidator.cs` itself is unmodified — confirmed via `git diff` showing zero changes to that file, since this pipeline is a wrapper, not an edit to the existing single-directory contract.
- Error messages clearly attribute violations to the offending mod via the post-processing shim.
- Every one of the four verification unit tests above passes; "guaranteed regardless of mod quality" is not an assertable Done-when criterion (it is unfalsifiable) — replaced with the concrete, testable claims above.

---

## Step 6 — Add Mod-Aware Save System (Save Records Active Mods)

### Goal
Extend the save format so it records which mods (and which versions) were active when the save was created — enabling safe load/migration when mod lists change.

### Correction — this must not touch every save store uniformly, and the "backward-compatible" claim needs a precise definition
Verified against `src/Host/NarrativeSaveStore.cs` (confirmed representative of its four sibling
stores per `AGENTS.md`'s SAVE/LOAD section — `ExpeditionSaveStore`, `MedicalSaveStore`,
`WorldSaveStore`, `JournalSaveStore`): these five stores already went through a recent hardening
pass that makes `TryLoad` **reject** any new-format envelope whose `Checksum` field is null or
empty, specifically to close a hole where a corrupt new-format save was silently treated as
"legacy." This has a direct interaction with adding `mods_active`:
- Since `SaveChecksum.Compute()` hashes reflectively over all public fields of the envelope type
  by name (confirmed at `Assets/Ashfall.Core/SaveChecksum.cs`), adding a `mods_active` field to an
  envelope DTO and recomputing the checksum on save is safe and requires no special-casing — the
  hash naturally covers the new field. This part of the plan is sound.
- **However**, "backward-compatible: saves without `mods_active` field load as legacy path" is
  ambiguous given the two coexisting formats already in play (pre-checksum bare-state saves vs.
  checksummed envelope saves). Precisely: an *old checksummed envelope* (has `Checksum`, has no
  `mods_active`) must still verify correctly once `mods_active` defaults to an empty list on
  deserialize — confirm the DTO's default matches what `SaveChecksum` would have hashed for a
  "no mods" state at save time, or old saves will fail checksum verification after this change
  ships (a save written before this migration, then loaded after, would compute a *different*
  hash than what's stored, because the object shape changed even if a default is supplied —
  test this exact case explicitly, don't assume it "just works").
- This is a **save-format version bump**, not a purely additive change, given the checksum
  interaction above. Per `AGENTS.md`'s SAVE/LOAD section, versioned migration is the existing
  pattern (`HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec` all support V1→V2→V3
  with "throw on future, migrate on past" semantics) — this step should follow that same codec
  migration pattern explicitly, and the plan should say which of the 5+ existing save stores are
  actually in scope (probably not all of them need `mods_active` — e.g. does `DoseLedgerSaveCodec`
  need to know about mods? Scope this per-store, not as one blanket "the save envelope").

### Implementation
- Add a `mods_active` section to save envelopes that are expected to be affected by mod content
  (scope this explicitly per-store in the design doc from Step 2 update, rather than assuming all
  stores need it uniformly):
  ```json
  {
    "save_version": 2,
    "checksum": "...",
    "mods_active": [
      { "mod_id": "mod_extra_items", "version": "1.2.0" },
      { "mod_id": "mod_new_events", "version": "0.9.1" }
    ],
    "state": { ... }
  }
  ```
- On load:
  - If a save lists a mod that is no longer installed → warn user, allow load with degraded state (orphan IDs logged).
  - If a save lists a mod at version X but installed version is Y → warn user if major version differs; allow if minor/patch differs.
  - If current session has mods not in the save → fine (new content just wasn't available before).
- `SaveChecksum` computation includes `mods_active` list (sorted by `mod_id` for determinism) — automatic via the existing reflection-based hash, no special code needed for the hash itself.
- Implement `ModSaveMetadata` DTO: `List<ModSaveEntry>` with `mod_id` + `version`.
- Wire into existing `CaptureState` flow: `ModLoader` exposes `GetActiveModMetadata()`.
- Follow the existing versioned-codec migration pattern (see `HoldfastSaveCodec` for precedent) rather than inventing a new migration mechanism for this field.

### Verification
- Unit test: save with mods → load → `mods_active` correctly deserialized.
- Unit test: save with mod removed → load warns but succeeds.
- Unit test: save with no mods → load in modded session succeeds (no warnings).
- Unit test: checksum changes when mod list changes.
- **New:** unit test: a save file written by the pre-`mods_active` code (i.e., an envelope
  serialized without that field at all) loads successfully post-migration and its checksum still
  verifies — this is the concrete backward-compatibility claim, made testable instead of asserted.
- Existing `SaveStoreChecksumSweepTests` still pass (no mods = empty `mods_active` list).
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass.

### Done when
- Save envelope includes `mods_active` metadata, scoped to the specific save stores identified as mod-relevant (not asserted as "all of them" without justification).
- Load gracefully handles mod list mismatches (warn, not crash).
- Checksum integrity maintained with mod metadata included, proven by the pre-migration-save-file test above, not just asserted.
- The strict "reject null/empty checksum on new-format envelope" guard already shipped for `NarrativeSaveStore`/`ExpeditionSaveStore`/`MedicalSaveStore`/`WorldSaveStore`/`JournalSaveStore` is preserved and not weakened by this change — confirmed by running `SaveStoreChecksumSweepTests` (12 existing tests) unmodified and green.

### Risk / Rollback
Risk: **High** — save format changes are the hardest category of change to roll back safely,
because player save files persist across the change indefinitely; a mistake here can strand
players with saves that silently fail to load or, worse, silently pass a checksum on corrupted
data. Combined with this being layered on top of the already-high-risk mod-merge pipeline
(Steps 3–5), this step should not be attempted until Steps 3–5 have shipped and been exercised
for at least one release cycle without incident — bundling save-format changes into the same
release as the first-ever mod pipeline compounds two high-risk changes at once.
- **Rollback plan:** since `mods_active` is additive to the envelope schema and the hash is
  computed automatically, a revert of this step's code is a normal `git revert` — but any save
  file written *with* `mods_active` populated, if loaded by a reverted (pre-Step-6) build, will
  fail checksum verification because the old code doesn't know about the new field and would
  compute a different hash. Document this explicitly as a one-way migration once players who use
  mods save at least once, and warn users accordingly rather than assuming rollback is free.

---

## Step 7 — Write Mod Loading Tests (Valid, Conflicting, Missing Dependency, Corrupt)

### Goal
Comprehensive test class `ModLoaderTests.cs` covering the full mod pipeline: discovery, validation, dependency resolution, sandbox, integrity, and save integration.

### Implementation
- Namespace: `Ashfall.Core.Tests`.
- Test fixture provides in-memory `IFileIO` with configurable mod directories. **Correction:**
  this requires the `IFileIO` extension from Step 3 to exist first (directory listing) — an
  "in-memory `IFileIO`" test double must implement the new `ListDirectories`/`ListFiles` methods
  too, or this fixture cannot be built as described.
- Test cases:
  1. **Valid single mod** — discovers, validates, loads, merges catalog additions.
  2. **Valid chain dependency** (A → B → C) — loads in correct order.
  3. **Missing required dependency** — mod rejected with descriptive error.
  4. **Optional missing dependency** — mod loads, optional features disabled.
  5. **Circular dependency** (A → B → A) — both rejected, clear error.
  6. **Conflicting mods** — lower-priority mod rejected.
  7. **Corrupt manifest** (invalid JSON) — mod rejected, others unaffected.
  8. **Sandbox violation** (override without permission) — mod rejected.
  9. **Sandbox granted** (override with permission) — override applied correctly.
  10. **Integrity failure** (dangling reference) — mod rejected post-merge.
  11. **Deterministic ordering** — same mods always produce same load order.
  12. **Empty mod directory** — returns empty result, no errors.
  13. **Save round-trip with mods** — save + load preserves mod metadata.
  14. **Load with missing mod** — warns, loads without crash.
  15. **New — path traversal in `data_additions`** — manifest declares a `../`-escaping path → rejected before any file read (see Step 4 correction).
  16. **New — absolute path in `data_additions`** — rejected.
  17. **New — cascade iteration cap reached** — pathological mod set that would loop forever without the cap (see Step 5 correction) → hard-fails with a clear error instead of hanging the test run.
  18. **New — pre-`mods_active` save loads and re-verifies** (see Step 6 correction) — a save envelope serialized without the `mods_active` field still passes checksum verification after the migration.

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all 18+ new tests pass (revised from 14 to include the security and migration cases added in this review).
- Each test is independent (no shared mutable state between tests).
- Tests run quickly with in-memory IO (no disk) — do not assert a specific wall-clock number like "< 2 seconds" as a Done-when criterion; that depends on CI hardware and is not a meaningful correctness signal. Assert "no test touches real disk I/O" instead, which is mechanically checkable by code review of the fixture.
- No engine references in test file.

### Done when
- `ModLoaderTests.cs` committed with at least 18 test methods (14 original + 4 security/migration cases identified in this review).
- All pass green.
- Edge cases (circular deps, cascading rejection, corrupt data, path traversal, cascade-loop, save-migration) are covered.
- Test names are descriptive and follow existing `Ashfall.Core.Tests` conventions.

---

## Step 8 — Security Review Gate (New Step — Added in This Review)

### Goal
Because this batch introduces the project's first third-party/untrusted-content trust boundary,
add an explicit, mandatory security review gate before this batch can be considered done — this
was entirely absent from the original draft, which treated the whole batch as routine engineering
with a flat "Medium" risk rating.

### Implementation
- A reviewer *other than* the implementer(s) of Steps 3–6 (per `AGENTS.md`'s Cross-Tool QA Rule,
  which already requires this for any system introducing ≥2 coupled variables — this batch
  introduces far more than 2) walks the merged diff against a fixed checklist:
  - Path traversal: every place a mod-supplied relative path is turned into a real file path goes
    through `ModSandboxValidator.ValidatePathIsWithinModRoot` — no exceptions.
  - Resource exhaustion: dependency graph size, manifest file size, and `data_additions` catalog
    size all have documented upper bounds enforced during validation (the original draft has no
    bounds at all — an unbounded number of mods or an unbounded dependency chain is a valid input
    today and could hang or OOM the loader).
  - JSON deserialization: confirm `SystemTextJsonSerializer`'s configured `JsonSerializerOptions`
    (`Assets/Ashfall.Core/HostDefaults.cs`) don't enable polymorphic type resolution or anything
    that could let a manifest instantiate arbitrary types — verify this explicitly rather than
    assuming JSON deserialization is inherently safe.
  - Confirm the cascade-rejection iteration cap (Step 5) and dependency-cycle detection (Step 2)
    both have hard upper bounds independent of mod count, so a crafted mod set cannot cause
    unbounded loader work.
- Reviewer signs off in the PR description with the specific checklist items verified, not just
  "LGTM."

### Done when
- Security checklist above is fully addressed with a named reviewer's sign-off, distinct from the implementer.
- Any finding from the checklist has either been fixed or explicitly accepted as a documented, scoped risk (not silently dropped).

### Risk / Rollback
Risk: N/A (this step is the review gate itself). Rollback: if the review finds a blocking issue, the batch does not ship Steps 3–6 until resolved — this step is intentionally a hard gate, not a formality.

---

## Summary Table

| Step | Title | Key Deliverable | Risk | Dependencies |
|------|-------|-----------------|------|--------------|
| 1 | Design mod manifest format | `docs/modding/manifest_schema.md` + 3 examples | Low | — |
| 2 | Design load order algorithm | `docs/modding/load_order.md` | Low | Step 1 |
| 3 | Extend `IFileIO` + implement `ModLoader` discovery/ordering | `IFileIO` directory-listing methods + `Assets/Ashfall.Core/Modding/ModLoader.cs` + supporting classes | **Medium-High** (interface change to a shared port) | Steps 1, 2 |
| 4 | Add mod sandbox + gated catalog merge | `ModSandboxValidator.cs` + permission model + path-traversal guard | **High** (first untrusted-content merge into runtime catalog; real path-traversal attack surface) | Step 3 |
| 5 | Add mod integrity pipeline | `ModIntegrityPipeline.cs` (wraps `CatalogIntegrityValidator` via a temp-directory + attribution shim; cascade loop has a hard cap) | Medium | Steps 3, 4 |
| 6 | Mod-aware save system | Save envelope `mods_active`, following the existing versioned-codec pattern | **High** (save-format change; one-way migration risk for player saves) | Steps 3, 5 |
| 7 | Mod loading tests | `ModLoaderTests.cs` (18+ tests, including security/migration cases) | Low | Steps 3–6 |
| 8 | Security review gate | Signed-off security checklist | N/A (review gate) | Steps 3–7 |

**Corrected scope: 8 steps, not 7** — the original "7 steps" estimate omitted the `IFileIO`
extension and the security review gate, both of which are load-bearing for the plan to be
implementable and safe as designed.

---

## Exit Criteria (Batch 72 Complete)

- [ ] Mod manifest schema is documented, versioned, and has working examples.
- [ ] Load order algorithm is deterministic, handles all edge cases, and is documented.
- [ ] `IFileIO` extended with directory-listing methods; all existing implementers still build.
- [ ] `ModLoader` discovers, validates, and orders mods purely in Core (no engine coupling) — merging is a separate, permission-gated step (4), not part of discovery.
- [ ] Sandbox prevents accidental overrides of core data (default-deny) AND rejects path-traversal attempts in `data_additions`/`data_overrides` before any file is read.
- [ ] `ModIntegrityPipeline` wraps the unmodified `CatalogIntegrityValidator` and rejects invalid mods, with a bounded cascade-rejection loop (documented iteration cap, not unbounded retry).
- [ ] Save format records active mods for the specific save stores identified as mod-relevant; load handles mismatches gracefully; a save written before this migration still verifies its checksum after the migration ships (tested, not assumed).
- [ ] 18+ tests cover the full pipeline (happy path + every failure mode + the path-traversal, cascade-cap, and save-migration cases added in this review).
- [ ] A named reviewer distinct from the implementer has signed off on the Step 8 security checklist.
- [ ] Full test suite passes: `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.
- [ ] Godot host builds cleanly: `dotnet build Ashfall.csproj`.
- [ ] Data integrity selftest passes: `godot --headless --path . -- --data-integrity-selftest`.
- [ ] No gameplay behavior change for players who install zero mods — mods are opt-in infrastructure only. (This claim is falsifiable and should be verified by running the full existing test suite and the five canonical verification commands with zero mods present and confirming no output changes, not just asserted.)

---

## Future Work (Out of Scope for Batch 72)

- Godot `.pck` packaging for mod distribution (host-layer work).
- Workshop/store integration (requires external service).
- Mod scripting (Lua/GDScript sandbox) — significant security surface.
- Mod UI (in-game mod browser, enable/disable, load order editor).
- Mod creation tools (editor/wizard for generating manifests).
- Mod compatibility matrix (automated testing of mod combinations).


---

## Review Notes (Corrected)

This batch was adversarially reviewed against the real codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`, with particular attention to
whether a plan for a HIGH-risk, high-scope feature (mod support) was being treated with
appropriate caution. The original draft rated this batch **Risk: Medium** and **7 steps** — both
of those top-line numbers were wrong, and the body of the plan did not contain a single mention
of security, attack surface, or save-file migration risk anywhere. The following is the full list
of what was found and fixed in place above.

### Top-level risk/scope re-rating
1. **Risk was Medium; corrected to High.** This batch is the first feature in the project that
   accepts and merges untrusted third-party content into the runtime catalog and (per Step 6)
   the save format. That is categorically different from "architectural decision with long-term
   consequences" — it's a new trust boundary. Re-rated with justification in the header table.
2. **Scope was "7 steps"; corrected to 8 (with the original 7 restructured).** The original
   7-step plan omitted: (a) extending `IFileIO` with directory-listing methods, which is a
   load-bearing prerequisite the plan's own Step 3 cannot be implemented without (see #4 below),
   and (b) a dedicated security review gate appropriate to a first-of-its-kind trust boundary,
   added as new Step 8. `TuningConfig` dependency (Batch 71) was previously listed but Batch 71
   itself doesn't strictly block mod loading — kept as a soft dependency, not a hard blocker.

### Factual errors (verified against source, cited file:line)
3. **`CatalogIntegrityValidator`'s `IdPrefixes` array has 239 entries**, confirmed by direct
   count against `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — "200+" was directionally
   fine but the real number is now cited.
4. **`IFileIO` (`Assets/Ashfall.Core/Ports.cs`) has no method to list directory contents or
   enumerate files** — only `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`,
   `Combine`. The plan's Step 3 claims `ModLoader` will discover mods "purely in Core... no
   `Godot.*`... no direct `System.IO`" — that is not achievable against the current port. Notably,
   `CatalogIntegrityValidator` itself already has this exact problem today and solves it by
   calling `System.IO.Directory.GetFiles` directly, bypassing `IFileIO` — i.e. there is existing
   precedent for the shortcut this batch's own verification bullets explicitly forbid. Fixed by
   adding the `IFileIO` extension as an explicit, separately-reviewed prerequisite in Step 3.
5. **`CatalogIntegrityValidator` has no per-mod attribution mechanism.** It is a single-pass
   scanner over one directory producing a flat error list; the file name is embedded in each
   error's path string, but there is no structural "which mod owns this" concept. Step 5's claim
   that it will "report errors attributed to the specific mod" needs a wrapping/shim layer built
   on top of the *unmodified* validator (a directory-to-mod-id lookup table maintained by the
   pipeline, with string post-processing on the validator's output) — this was not explained in
   the original draft and is now spelled out in Step 5's correction, including an explicit
   instruction not to fork the validator itself.
6. **No existing `CatalogLoader` does directory discovery of unknown files** — every one reads a
   fixed, known filename (confirmed for `YearOfAshCatalogLoader`, `VerdictCatalogLoader`). The
   motivation's claim that "CatalogLoaders already discover and merge multiple JSON files from
   disk" overstated how close this pattern already is to what `ModLoader` needs; corrected to be
   precise about what's actually reusable (the merge-after-load pattern) versus what's new
   (unknown-directory discovery).
7. **`SaveChecksum` (`Assets/Ashfall.Core/SaveChecksum.cs`) hashes reflectively over public
   fields sorted by name** — confirmed. This means Step 6's claim that the checksum will
   naturally include `mods_active` is correct, BUT the plan missed a real interaction with the
   already-shipped stricter checksum guard on `NarrativeSaveStore`/`ExpeditionSaveStore`/
   `MedicalSaveStore`/`WorldSaveStore`/`JournalSaveStore` (confirmed at `src/Host/
   NarrativeSaveStore.cs`): these stores now hard-reject any new-format envelope with a null/empty
   checksum. Adding a field to the envelope DTO changes what gets hashed, so a save written before
   this migration must be proven — not assumed — to still verify correctly after the field is
   added with a default value. Added an explicit test case and reframed this step as a
   save-format version bump following the project's existing versioned-codec pattern
   (`HoldfastSaveCodec` et al.), rather than a purely additive change.

### Security gaps (absent from the original draft entirely)
8. **No path-traversal protection was specified anywhere**, despite `data_additions` and
   `data_overrides.target_file` being untrusted, mod-supplied relative paths that get resolved to
   real file reads. This is one of the most common real-world modding-framework vulnerability
   classes. Added a mandatory `ModSandboxValidator.ValidatePathIsWithinModRoot` gate in Step 4,
   plus two new test cases in Step 7, plus a checklist item in the new Step 8 security gate.
9. **No resource-exhaustion bounds were specified** for dependency graph size, manifest size, or
   cascade-rejection re-validation iterations. Step 5's "re-validate iteratively until stable" has
   no termination guarantee as written — a crafted or accidentally pathological mod set could
   loop indefinitely. Added an explicit, tested iteration cap.
10. **No mention of JSON-deserialization safety** (e.g. confirming the serializer config doesn't
    allow type-polymorphic deserialization that a manifest could exploit). Added as a Step 8
    checklist item.
11. **`AGENTS.md`'s own Cross-Tool QA Rule was not invoked**, even though it explicitly requires
    "any system introducing ≥2 new coupled variables" to be implemented and reviewed by different
    tools — this batch introduces far more than 2 (permission flags × override targets × path
    resolution × dependency graph × load order × save schema). Added as justification for the new
    Step 8 and referenced directly in Step 4's risk note.

### Step-ordering error
12. **Step 3 specified catalog merging (including override application) before Step 4 defined the
    permission model that's supposed to gate overrides.** As originally sequenced, there was a
    window where merge logic could exist and run without any sandbox check. Fixed by moving all
    merge logic into Step 4, after the permission model, and having Step 3 stop at
    discovery/ordering only.

### Vague, unfalsifiable Done-when criteria tightened
13. Step 1: "Schema review confirms no ambiguity in conflict resolution or load ordering" →
    replaced with five specific written questions the doc must answer.
14. Step 2: "Peer review confirms no ambiguity or undefined behavior in edge cases" → replaced
    with four specific edge cases the doc must resolve in writing, each with a worked example.
15. Step 5: "Core catalog integrity is guaranteed regardless of mod quality" → this is an absolute,
    unfalsifiable claim; replaced with the four concrete unit-test assertions that are actually
    checkable.
16. Step 7: "Tests run in < 2 seconds total" as a Done-when criterion → replaced with "no test
    touches real disk I/O," since wall-clock thresholds are CI-hardware-dependent and not a
    correctness signal.

### What was already correct (not changed)
- The five canonical verification commands (`dotnet build`/`dotnet test`/`dotnet build
  Ashfall.csproj`/`godot --headless ... --data-integrity-selftest`) are real and runnable,
  confirmed against `HostCli.cs`.
- `SystemTextJsonSerializer` is a real class implementing `IJsonSerializer`
  (`Assets/Ashfall.Core/HostDefaults.cs:32`) — the plan's references to it are accurate.
- The "Future Work (Out of Scope)" section correctly excludes `.pck` packaging, workshop
  integration, and mod scripting (Lua/GDScript) as separate, larger efforts — this scoping
  judgment was sound and is unchanged. Mod scripting in particular is flagged in the original
  draft as "significant security surface," which is correct — it should be noted that even
  *without* scripting, this reviewed batch (JSON-only mods) already had unaddressed security
  surface, which is the main substantive finding of this review.
