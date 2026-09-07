# Plan 47 — The Mod & Content-Pack Contract: Write Down the Boundary

> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
> **Depends on:** 45A (acceptance ladder), 26A (path resolver), 40B (tags), 25C (locale overlays),
> 27A (fixture fidelity).
>
> **Theme:** ASHFALL is already moddable by accident. `ASHFALL_DATA` lets any build load a different
> data directory, `schema_version` is on every catalog, `CatalogIntegrityValidator` can arbitrate a
> content pack, `expansion_item_tags` + the 40B tag layer are exactly an extension vocabulary, and the
> locale overlay pattern from 25C is a working example of data-only augmentation. None of it is
> written down as a contract, so the project has a mod surface it can neither promise nor protect —
> and every future breaking data change is silently a modding break.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | A data-pack override already exists | `src/Host/CatalogPath.cs:19–22` — precedence **1** is `ASHFALL_DATA` env override when the directory exists; `CreateFileIOForDataDir` (`:66–71`) switches between `GodotFileIO` (PCK) and `FileSystemIO` (on disk), so an external directory is a first-class source |
| 2 | Selftests already rely on alternate data dirs | `src/Host/SevenDayDeterministicSmokeTest.cs:73` and `PanelBindLifecycleSelfTest.cs:114` create temp data directories — the mechanism to load "other content" is exercised in CI, just not as a supported use |
| 3 | Every catalog declares a version | 411 JSON files carry `schema_version`, presence enforced by `CatalogIntegrityValidator` (per `AGENTS.md`: the root-object rule, gated by `CatalogIntegrityValidatorTests`) |
| 4 | A validator exists that a pack could be run through | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — five tiers: registry, TIER-1 prefix resolution, TIER-2 reference keys, ranges, uniqueness; `--data-integrity-selftest` currently reports **138 catalogs / 5563 ids / 0 errors** |
| 5 | The extension vocabulary landed in Wave 6 | `expansion_item_tags.json` (67 `item_id → tags`) with `ExpansionEnrichmentCatalog` queries, wired by 40B — behaviour keys off declared properties, so a pack can declare a tagged item without code |
| 6 | Data-only augmentation already has a proven shape | Wave 3's 25C chose per-locale JSON **overlays keyed by definition id** ("keeps the mod-safe authority intact… lets translators ship deltas") — a content pack is the same mechanism with a different prefix |
| 7 | The whitelist concept exists but is thin | `Assets/StreamingAssets/Data/whitelists/` contains exactly one file (`orphan_knocks.json`); `docs/data/CATALOG_REGISTRY.md` classifies it `OPTIONAL` — infrastructure precedent, not a policy |
| 8 | No mod documentation exists | no `docs/modding/`; `ashfall-mod-contract` exists only as a skill; `docs/ENGINE_SUPPORT_POLICY.md` covers engines, not content boundaries |
| 9 | Save compatibility is the sharp edge | `SaveChecksum` + envelope whitelist (`CampaignEnvelopeBuilder.cs:14,70` rejects unknown keys), 62-store contract matrix, `SaveWireContract` tests — a pack that changes persisted shapes breaks saves, and nothing today tells an author that |
| 10 | Determinism is the other edge | Invariant 4 (`ISeededRng`, no `Guid.NewGuid()`, culture-invariant formatting) — a pack that introduces unseeded behaviour or unordered id iteration breaks replays and the 46A sweeps |
| 11 | The project's own docs already drift | Waves 3 and 5 recorded `AGENTS.md`/registry claims contradicted by source — the same failure mode will hit a mod doc within a month unless the contract is **generated** from the validator's rules, not written beside them |

---

## Task 47A — Define the boundary: what a pack may and may not touch

**Goal:** one generated, versioned contract: the mod-safe surface, the forbidden surface, and the
stability promise for each.

**Files:** new `docs/modding/CONTRACT.md` (generated), new
`scripts/ci/generate-mod-contract.py`, `CatalogIntegrityValidator.cs` (rule source),
`SaveSectionRegistry.cs`, `docs/data/CATALOG_REGISTRY.md`, `Assets/StreamingAssets/Data/whitelists/`,
`Ashfall.Core.Tests/ModContractTests.cs`, `AGENTS.md` (one row pointing at the contract).

### Substeps

1. **Write the layer table** — for each of: JSON catalogs (per-family), tags, locale overlays,
   save-schema, checksums, RNG streams, code, scenes: `STABLE` (packs may rely on it), `INTERNAL`
   (may break), `FORBIDDEN`. Generate it from existing authorities rather than hand-maintaining it.
2. **Declare the supported pack mechanism** explicitly: an external data directory (already
   precedence #1) **or** a drop-in overlay folder scanned by `CatalogPath`, with load order, override
   semantics (whole-file replace vs per-definition overlay — pick per-definition, matching 25C) and a
   rejection path for malformed packs.
3. **Freeze the identity rules** as the public contract they already are: snake_case ids, prefix list
   (`item_`, `loc_`, `quest_`, `flag_`, `echo_`, `radio_`, …), uniqueness, reference resolution —
   published from `CatalogIntegrityRules`, not paraphrased.
4. **Publish the stability promise for saves**: packs must not change persisted shapes; a pack that
   renames an id referenced by a save is unsupported by design (the envelope whitelist already rejects
   unknown sections — say so in the contract).
5. **Publish the determinism promise**: no new unseeded sources, ids must iterate in declared order,
   numbers must be culture-invariant — the same invariants the project's own tests enforce (make the
   test list the contract's teeth).
6. **Define what packs may add** with today's rails: items (with tags), locations/map nodes (32A
   graph tier), encounter and echo entries (18A), voice lines (42A schema), policies (43B catalogue),
   memorials/epitaphs (41A) — every one of these became pack-able in Waves 4–6; **list only schemas
   that exist**, so the contract can't over-promise.
7. **Define what packs may not add**: code execution, assemblies, scene replacement, shader or input
   map hijack, and any save-section key not in `SaveSectionRegistry`.
8. **Version the contract itself** and cite the exact gates a pack must pass
   (`--data-integrity-selftest`, acceptance ladder tiers from 45A, `--content-utilization-selftest`).
9. **Add a compatibility matrix** to the contract: game version ↔ data schema ↔ save schema —
   generated from `VersionReport` (`Assets/Ashfall.Core/VersionReport.cs`, surfaced via
   `HostCli.cs:521–527`) so the promise is computed, not typed.
10. **Document the author loop**: pack scaffold, local run with `ASHFALL_DATA`, validator command,
    and how to read a rejection.
11. **Generate, never hand-edit**: register `generate-mod-contract.py --check` as a Tier-2 gate so
    the contract cannot drift from the validator (the exact failure Waves 3 and 5 documented in the
    project's own docs).
12. **Tests**: contract-vs-validator consistency test, and a fixture-pack test that a legal pack
    loads and an illegal one is rejected with an actionable message.
13. **Run the checklist** + both content gates.

**DoD:** a public, generated contract stating what a content pack can rely on — and the gates that
enforce it.

---

## Task 47B — Build the loader path: pack discovery, precedence, and validation

**Goal:** make "install a content pack" a supported code path with defined precedence and a hard
validation gate, using the resolver that already exists.

**Files:** `src/Host/CatalogPath.cs`, `Assets/Ashfall.Core/CatalogFileSystem.cs`,
`Assets/Ashfall.Core/IO/CatalogBootValidator.cs`, the `*CatalogLoader.cs` family (10 files),
`ContentExemption.cs`, `SaveSlotRoot.cs` (user paths), new
`src/Host/ContentPackService.cs`, new `Ashfall.Core.Tests/ContentPackLoadTests.cs`,
`docs/modding/INSTALL.md`.

### Substeps

1. **Add pack discovery** to the resolution order (base PCK/dir → pack overlays → explicit env
   override), with a documented, deterministic precedence: base → official expansion packs (by
   declared order) → user packs (ordinal by id) — never filesystem order.
2. **Implement per-definition overlay merge** in Core (engine-free), keyed by `id`, with three legal
   operations: add, modify, and retire — and an explicit `schema_version` compatibility check before
   merging.
3. **Validate every merged pack through the existing tiers** (registry/prefix/references/ranges/
   uniqueness) *before* boot reaches the menu; a failing pack is reported and skipped (or the boot
   refuses, with a clear choice) rather than partially applied.
4. **Report the effective content set** at boot: base count, per-pack counts, merged/overridden ids —
   into the version report and `--content-utilization-selftest` evidence, so "did my pack load?" is
   answerable without a debugger.
5. **Reject hostile input**: id collisions with reserved namespaces (`flag_`, internal `whitelists/`),
   absurd sizes, non-object roots where objects are required, and `..` path traversal in pack
   manifests.
6. **Keep determinism**: merged iteration order must be stable and independent of load timing; add a
   paired-seed replay test proving a pack doesn't perturb the base campaign's digest when disabled.
7. **Save isolation**: assert a save made with packs enabled is *readable* with them disabled
   (graceful defaults for missing ids) — the property that prevents "modded save bricked" tickets.
8. **Performance**: overlay merging must not add measurable boot cost to the base game (26C budget);
   measure and record.
9. **Version pinning in pack manifests**: a pack declares supported game/data ranges; out-of-range
   packs warn and are disabled by default.
10. **Deduplicate loaders** while touching them: `CatalogBootValidator` + 10 `*CatalogLoader.cs`
    share too much shape — extract the common read/parse/warn path so packs flow through one door
    (and Wave 3's 27A fixture fidelity applies to all of them).
11. **Tests**: precedence, add/modify/retire, unknown-id handling, disabled-pack save load,
    traversal rejection, determinism, boot cost.
12. **Docs**: `docs/modding/INSTALL.md` + a `fixtures/content_pack_example/` in the test project — the
    contract made concrete, because docs without a working example rot immediately.
13. **Run the checklist** + `--data-integrity-selftest` with the example pack enabled and disabled.

**DoD:** install, disable, and validate a content pack without touching the game's code or saves.

---

## Task 47C — Keep the promise: pack compatibility in CI and in releases

**Goal:** the contract becomes a regression suite — packs (real or fixture) are tested on every
release, and breaking data changes are labelled as such automatically.

**Files:** new `Ashfall.Core.Tests/PackRegressionTests.cs`, fixture packs under
`Ashfall.Core.Tests/Fixtures/Packs/`, `docs/ci/CI_GATE_MANIFEST.json`, `docs/balance/DECISIONS.md`
(46A) — breaking-change entries, `CHANGELOG.md` (48A) generated sections,
`scripts/ci/verify-capability-claims.py` (29B), `SaveSectionRegistry.cs`,
`VersionReport.cs`.

### Substeps

1. **Ship fixture packs as first-class test inputs**: minimal legal pack, additive pack, overriding
   pack, illegal pack (bad refs), out-of-range pack, and a pack touching tags only.
2. **Gate them in CI**: each fixture pack runs load + validate + 30-day deterministic soak +
   save/reload with and without the pack; the results are a named fast-tier gate.
3. **Detect breaking data changes automatically**: diff the generated contract's surface (id prefixes,
   item fields, tag vocabulary, section keys, schema versions) between the release and the previous
   tag, and require a `BREAKING:` changelog line when it changes (feeds Plan 48A).
4. **Publish a supported-changes list per release** — packs depend on *these* schemas, *these* were
   deprecated, *these* are gone — generated from the diff, not from memory.
5. **Deprecation policy**: fields/prefixes are deprecated with a documented window (version count or
   calendar), warnings during the window, removal only at a major version — and the window is enforced
   by a test that fails when a deprecated field is removed early.
6. **Version-compat matrix in the app**: a UI line (or the version report) showing the mod-relevant
   versions, so a support thread can start from data rather than screenshots.
7. **Guard the invariants packs can break**: keep the determinism, save-shape, and reference tests in
   the same tier as the pack gate, so a change that quietly breaks packs fails the build rather than
   the community.
8. **Instrument reachability**: 46B's metrics can report base-vs-pack content usage — so "does anyone
   install packs?" is answerable honestly after release (local-only, per the privacy stance).
9. **Write the escalation path**: what an author does when a pack breaks (issue template fields:
   game sha, pack manifest, effective content report, validator output) — the same
   self-describing-artifact idea as Wave 5's 31C step 11.
10. **Decide the non-goals explicitly** in the doc: no code mods, no assembly loading, no workshop
    integration, no encrypted packs — stated so future contributors don't rediscover the debate.
11. **Tests**: this task *is* the tests, plus a meta-test asserting the diff tool flags a deliberately
    breaking fixture.
12. **Run the checklist** + the release gate (39A) with packs enabled.

**DoD:** breaking data changes are detected, labelled, and blocked without anyone remembering to ask.

---

## Cross-Task Dependencies

```
26A (resolver) ──► 47B (pack precedence)          45A (ladder) ──► 47A step 6 (only real schemas)
40B (tags)     ──► 47A step 6, 47B step 2         25C (overlays) ──► 47B step 2 (merge semantics)
27A (fixtures) ──► 47C step 1                     VersionReport/SaveSectionRegistry ──► 47A step 9
46A (sweeps)   ──► 47C step 2 (30-day soak harness is shared)
48A (changelog)◄── 47C step 3 (breaking-change detection feeds release notes)
```

**Execution order:** 45A → 47A → 47B → 47C, and 47A must precede any public statement about modding
(in a store page, a Discord pinned message, or a `README.md` line) — an undocumented promise is the
one commitment you can't take back.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # base + each fixture pack
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-mod-contract.py --check           # contract not drifted from rules
7. ASHFALL_DATA=<fixture pack> boot → effective content report shows merged ids
8. pack-disabled save load of a pack-enabled save (graceful defaults)
9. paired-seed replay: base digest unchanged when packs are absent
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Tooling | Docs | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 47A | 0 | 0 | 1 generator | 1 | 3–5 | Low–Med | LOW |
| 47B | 1–2 | 2 | 0 | 1 | 12–16 | Medium–High | **MEDIUM — touches content loading for the base game too** |
| 47C | 0 | 0 | diff tool | 1 | 8–12 (fixture packs) | Medium | LOW |

**Guardrails:** no code execution or assembly loading, no Steam Workshop coupling, no per-pack save
sections, no unversioned promise, no hand-written contract text where a generator exists (the project
has now documented five stale claims it wrote by hand), and never let a pack path weaken the base
game's determinism or save-compat guarantees — those invariants exist because of Waves 1–6, not
around them.
