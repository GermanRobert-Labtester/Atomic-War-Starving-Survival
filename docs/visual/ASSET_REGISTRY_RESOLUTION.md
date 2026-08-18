# ASHFALL AssetRegistry — Resolution Semantics

**Source of truth:** `src/Host/AssetRegistry.cs`
**Audit baseline:** Phase 13 (this phase) — re-conducted after empirical wiring
reconciliation found that the prevailing prefix-drift theory was largely
incorrect.
**Last revised:** this turn.

This document is the canonical description of how the AssetRegistry maps a
catalog content ID to an on-disk texture asset. Future contributors should
follow this resolution model unless they file a Phase-N+1 amendment that
finds a verified completeness or safety gap.

---

## Resolution policy (deterministic, in priority order)

Given a `requestedId` and a `kind` (one of `item`, `portrait`, `location`,
`faction`), the registry produces an ordered list of `(stem, origin)`
candidate pairs. The first candidate that resolves to a real file on disk
wins; later candidates are not consulted.

| Priority | Origin | Source of the candidate stem |
| --- | --- | --- |
| 1 | `literal` | the requested ID, unmodified |
| 2 | `semantic-alias` | resolved from `ItemIdAliases` (item-only) — see below |
| 3 | `prefix-add` | for each `(category, prefix)` in `PrefixAddRules` whose category matches, prepend `prefix` to the requested ID — **only if the prefix is not already present** in the requested ID, to avoid duplicate candidate stems |

The list is built once per resolve call (`ResolveStemCandidates`), then
passed to `ResolveByCandidates` which iterates the list and consults
`ResourceLoader.Exists` against the four category-specific search-root
arrays.

### Why this order?

Direct-stem-first is the strongest evidence that the referenced ID is the
canonical one. Alias second because those map are hand-curated semantic
aliases (e.g. `mechanical_components` → `scrap_mechanical`). Prefix-add
last because prefix-add is a normalization heuristic and we want any
*direct* mapping to win over it.

---

## Search roots

Four arrays of `res://…` patterns, one per kind:

| Kind | Roots |
| --- | --- |
| `item` | `assets/art/{0}.jpg`, `assets/art/{0}.png`, `assets/sprites/Items/{0}.png`, `assets/sprites/items/{0}.png` |
| `portrait` | `assets/art/{0}.jpg`, `assets/art/{0}.png`, `assets/sprites/Portraits/{0}.png`, `assets/sprites/portraits/{0}.png` |
| `location` | `assets/art/{0}.jpg`, `assets/art/{0}.png`, `assets/sprites/Locations/{0}.png`, `assets/sprites/locations/{0}.png` |
| `faction` | `assets/art/{0}.jpg`, `assets/art/{0}.png`, `assets/sprites/Factions/{0}.png`, `assets/sprites/factions/{0}.png` |

The first occurrence of any root that `ResourceLoader.Exists` confirms is
loaded. JPG before PNG because JPG is the dominant storage format in this
codebase.

---

## Category-aware prefix-add (`PrefixAddRules`)

```csharp
private static readonly (string category, string prefix)[] PrefixAddRules = new[]
{
    ("item",     "item_"),
    ("portrait", "survivor_"),
    ("portrait", "npc_"),
    ("location", "loc_"),
    ("faction",  "faction_"),
};
```

Each rule is `(category, prefix)`. The candidate is added **only** when the
kind matches and the requested ID does not already start with that prefix.

### Why no prefix-strip?

The Phase-12 audit hypothesised that catalog IDs carry `item_X` / `weapon_X`
prefixes while the on-disk asset is bare-stem `X.jpg`. A faithful re-mirror
of the production pipeline against the actual filesystem layout (Phase-13
empirical reconciliation) showed the opposite in 192 of 192 cases: the
filesystem *also* uses the prefixed form. Stripping the prefix would
therefore *break* the cases where stripping is unnecessary (because the
filesystem already uses the prefixed name) in addition to finding the
bare-stem minority. Prefix-add is the correct direction — and it is
applied only as a fall-back, after the direct stem has already failed.

### Why these prefixes specifically?

- `item_` — the dominant port in `Assets/StreamingAssets/Data/items.json`
  (catalog IDs like `item_blood_bag` paired with `item_blood_bag.jpg`).
- `survivor_` and `npc_` — both prefixes appear in `characters.json`;
  either is acceptable as a fall-back.
- `loc_` — present in `crossing_locations.json` / `holdfast_locations.json`.
- `faction_` — appears in `holdfast_factions.json` and others.

Candidate prefixes NOT enabled by default (added only if evidence emerges):

- `weapon_` — only 1 catalog row uses it directly; not necessary.
- `med_` — only 10 art files use this prefix; catalog use is rare.
  The pre-existing `mechanical_components → scrap_mechanical` semantic
  alias is the cleaner approach when a true semantic mismatch exists.

---

## Semantic aliases (`ItemIdAliases`)

```csharp
private static readonly Dictionary<string, string> ItemIdAliases = new(StringComparer.Ordinal)
{
    { "mechanical_components", "scrap_mechanical" },
    { "mechanical_parts",      "scrap_mechanical" },
    { "scrap_mechanical",      "scrap_mechanical" }, // self-alias for safety
};
```

A semantic alias is a *hand-curated* mapping for the case where a catalog
ID has been renamed, refactored, or never finalised. **These should remain
distinct from prefix-add heuristics** because semantic aliases require
human judgement about which art is appropriate.

### When to add a new alias vs. rely on prefix-add

Add a new `ItemIdAliases` entry only when:

- the requested ID and the on-disk stem refer to different *concepts*, not
  just different spellings, OR
- the on-disk stem would be ambiguous if found by direct lookup (e.g.
  there are two equally-valid `scrap_X.jpg` and the right one depends on
  game-side context).

Otherwise rely on `PrefixAddRules` to add the production stem.

---

## Case sensitivity

All AssetRegistry candidate generation is **case-sensitive**. The default
`IFileSystem` and `ResourceLoader` paths used here rely on Godot's
canonical-form paths, which are case-sensitive on Linux (the project's
target platform). A bare stem match against `iodine_pills` will not match
a file named `Iodine_pills.jpg`. The repository should be canonical-form:
all lowercase, snake-case where multi-word, no trailing case drift.

This is enforced at the registry level by:
- `StringComparison.Ordinal` lookups in `ItemIdAliases`.
- No case-insensitive variants in `PrefixAddRules`.
- Direct filesystem existence checks via `ResourceLoader.Exists`.

The codebase should keep the file tree canonical. Migration to fix any
deviation is by `git mv` on the on-disk file, not by softening the
registry.

---

## Fallback behaviour

If no candidate stem exists in any search root:

1. The id is added once to `_loggedMissing` (deduped logging).
2. If `SetFallbackTexture` has been called with a non-null texture, that
   texture is returned with `AssetLoadResult.FallbackUsed`.
3. Otherwise `AssetResult.Missing` is returned (texture is `null`).

Production uses a fallback texture (set up in the Godot host bootstrap).
Phase-12 / Phase-13 selftests confirmed that **0 production-rendered
content triggers the fallback** — fallback is reachable only in
non-rendering contexts (e.g. headless `--asset-registry-selftest` runs
that intentionally probe non-existent IDs as negative cases).

---

## Collision protection

The resolution order guarantees no silent mis-resolution between two
distinct content IDs that share the same bare stem:

- Two catalog IDs `item_X` and `Y` (both bare-stem `X` and `Y`) where the
  asset file at `item_X.jpg` exists and `Y.jpg` does not — only the
  `item_X` request resolves; the `Y` request continues to the prefix-add
  step and will eventually MISS unless `item_Y.jpg` also exists.

- Two catalog IDs `X` and `item_X` for the same game item — both resolve
  to `item_X.jpg` (the prefix-add step for the first request adds it,
  and the second request's literal stem already matches).

A prefix-strip rule (not currently enabled) could allow a request like
`item_X` to resolve to *either* `item_X.jpg` (literal) or to a different
stem `X.jpg` (stripped) — that's why prefix-strip is intentionally NOT
enabled.

---

## Performance

`ResolveStemCandidates` returns a list of at most `1 + 1 + N` candidates
per request, where `N` is the number of matching prefix rules (1–5 in
practice). For each candidate, four search-roots are probed via
`ResourceLoader.Exists` (one cheap filesystem stat). Total: at most
`6 × 4 = 24` filesystem stats per ID resolution, all sequential.

This is acceptable: registry resolution is invoked from UI surface binding
once per panel per session, not per frame. Even at 1 ms per stat × 24 stats
× ~1000 items in worst-case catalogue load, that's <100 ms of one-time
work.

---

## What this resolver does NOT do

- Does not normalise `faction_war_*` catalogue row IDs as visual entities.
  These are filtered out as F.REFERENCE_ONLY at the audit level.
- Does not strip prefixes. (See above.)
- Does not recurse into subdirectories beyond the four canonical search
  roots.
- Does not perform case-insensitive matching. Linux-canonical.
- Does not load sprite atlases or `AtlasTexture` resources (no such
  resources exist in the project as of Phase 13).
- Does not maintain an in-memory cache. If two consecutive calls ask for
  the same ID, the second observes identical cost. (If we ever need a
  cache, add one — but it would have to invalidate on file changes.)

---

## Public surface

| API | Behaviour |
| --- | --- |
| `AssetRegistry.GetItem(string id)` | Resolve as item; returns `AssetResult`. |
| `AssetRegistry.GetPortrait(string id)` | Resolve as portrait; returns `AssetResult`. |
| `AssetRegistry.GetLocation(string id)` | Resolve as location; returns `AssetResult`. |
| `AssetRegistry.GetFaction(string id)` | Resolve as faction; returns `AssetResult`. |
| `AssetRegistry.GetByPath(string absolutePath)` | Bypasses all normalization. Use only for direct load sites that already know the absolute file. |
| `AssetRegistry.ResolveItemPath(string id)` | Path-only (no load). Used by tests. |
| `AssetRegistry.ResolvePortraitPath(string id)` | Path-only. |
| `AssetRegistry.ResolveLocationPath(string id)` | Path-only. |
| `AssetRegistry.SetFallbackTexture(Texture2D?)` | Set fallback. |
| `AssetRegistry.ClearMissingLog()` | For tests. |
| `AssetRegistry.MissingAssetCount` | For tests. |

`AssetRegistry.ResolveStemCandidates(string id, string kind)` — `internal`
— is the single normalisation entry point. Future Normalize-style
extensions should be added here.

---

## Tested invariants

The `--asset-registry-selftest` binary (logic in `AssetRegistrySelfTest`
in the same C# file) verifies the following in addition to the older
top-N-from-catalogs checks:

**Positive normalization probes** (each verifies the *actual resolved
path* contains the *expected file stem*, not just that something loaded):

| ID | Kind | Expected stem in resolved path |
| --- | --- | --- |
| `mechanical_components` | item | `scrap_mechanical` |
| `mechanical_parts` | item | `scrap_mechanical` |
| `blood_bag` | item | `item_blood_bag` |
| `encrypted_drive` | item | `item_encrypted_drive` |
| `faraday_pack` | item | `item_faraday_pack` |
| `cigarette_pack_sealed` | item | `item_cigarette_pack_sealed` |
| `iodine_pills` | item | `iodine_pills` (direct) |
| `geiger_counter` | item | `geiger_counter` (direct) |

**Negative probes** (a probe ID that should NOT resolve — verifies the
registry cannot be tricked by prefix-add):

| ID | Kind |
| --- | --- |
| `__definitely_not_a_real_asset_xyzzy__` | item |
| `__non_existent_portrait_xyzzy__` | portrait |
| `__non_existent_location_xyzzy__` | location |

If any negative probe resolves, the selftest fails with a clear error
message identifying which probe leaked through.

---

## When NOT to add to `PrefixAddRules`

If a single catalogue ID consistently misses despite clear evidence that
the corresponding asset exists on disk, the right answer is **NOT** to add
a new prefix rule. It's to:

1. Add a targeted `ItemIdAliases` entry if the meaning differs.
2. Run a recheck to confirm the asset actually exists at the expected
   location under the expected filename.
3. If neither, escalate to the art-replacement queue.

A faulty prefix rule can mask legitimate failures and induce silent
mis-resolution (collision with another stem that happens to share the
prefix). The existing rules are narrow on purpose.
