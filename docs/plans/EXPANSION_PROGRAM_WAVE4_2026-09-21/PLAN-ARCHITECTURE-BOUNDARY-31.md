# PLAN-ARCHITECTURE-BOUNDARY-31 — Core Purity, IO Boundary & Dependency Direction

**Wave:** 4 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-INTEGRATION-KIT-02 (gates), PLAN-ORPHAN-SEAL-01.
**Expanded appendix:** [`PLAN-ARCHITECTURE-BOUNDARY-31_APPENDIX-A_IO_INVENTORY.md`](PLAN-ARCHITECTURE-BOUNDARY-31_APPENDIX-A_IO_INVENTORY.md)
— every direct IO call in Core: **672 call sites across 127 files**, grouped by
family (loader/demo/other) with the APIs used and a verdict
(`MIGRATE` · `REVIEW` · `KEEP` · `ALLOWED`). AB-31A/31B migrate from this
table and the gate allowlist starts from its KEEP/ALLOWED rows.
**Non-goals:** no rewrite of the Core assembly layout, no new abstraction layer
where one exists (`IFileSystem`), no Unity work.

---

## 1. Outcome

`AGENTS.md` invariant 2: "Core stays engine-free." The audit confirms **zero
real engine references** in `Assets/Ashfall.Core` — the 26 grep hits are all
comments asserting the rule. The boundary is therefore clean at the engine
level and **porous at the IO/JSON level**:

- **260 Core files** import `System.IO` or `System.Text.Json`;
- **487 direct `File.*` / `Directory.*` / `Path.*` calls** exist in Core;
- only **62 Core files** use the `IFileSystem`/`FileSystemIO` abstraction, and
  the host `CatalogPath` routing policy exists precisely so path access has one
  owner.

This plan makes the boundary *measurable and uniform* without a big move: a
classification of every Core IO site, a rule for which sites must go through
the port, and a gate that stops new direct-path growth in the wrong places.

Deliverables:

1. an **IO inventory** for Core: file, call, purpose (catalog load, dev tool,
   save codec, path resolution), and allowed/forbidden verdict;
2. a **port rule**: catalog and save IO goes through `IFileSystem`/`CatalogPath`;
   pure computation stays engine-free and IO-free;
3. **migration** of the forbidden sites (bounded, file-family by file-family);
4. a **boundary gate** covering engine refs, IO rule, namespace direction, and
   `netstandard2.1` surface.

---

## 2. Evidence (2026-09-21)

| Fact | Value | Command |
|---|---:|---|
| Real Godot/UnityEngine/UnityEditor refs in Core | 0 | `grep -rn "using Godot\|using UnityEngine" Assets/Ashfall.Core` → comments only |
| Core files importing IO/JSON | 260 | `grep -rln "using System.IO\|using System.Text.Json"` |
| Direct `File.*`/`Directory.*`/`Path.*` sites | 487 | `grep -rn` count |
| Core files using `IFileSystem`/`FileSystemIO` | 62 | grep |
| `JsonSerializer`/`System.Text.Json` mentions | 1,328 | grep (includes options/DTO attributes) |
| Path policy owner | `src/Host/CatalogPath.cs` + forbidden-path gate | gates |
| Engine-shim artifact | `MathfCompat` (no engine refs) | `Assets/Ashfall.Core/MathfCompat.cs` |
| Related gate | "Forbidden Core API Source Gate" | `CI_GATE_MANIFEST.json` |

**Classification of the 487 sites (target):**
- `ALLOWED_PORT` — already takes an `IFileSystem`/stream and merely uses
  `Path.Combine` for a display or test path;
- `MIGRATE` — direct read from a path built inside Core (catalog loaders);
- `KEEP_WITH_REASON` — deterministic helpers (e.g. checksum over a stream),
  dev-only tooling in Core headless demos, or codec code that receives bytes.

---

## 3. Packages

### AB-31A — Boundary inventory and rulebook
- Generate `docs/architecture/CORE_IO_BOUNDARY.md` from the audit: per file,
  per call site, verdict.
- Write the rule: Core may depend on `System.*`, `netstandard2.1`, and its own
  ports; Core must not build user/asset paths, must not read `res://`, must not
  reference Godot or Unity types; only host adapters own `user://`.
- **Acceptance:** every site has a verdict; the doc is linked from
  `AGENTS.md`'s source-of-truth table by the foreman.
- **Verify:** the generator `--check`.

### AB-31B — Catalog loader migration
- Migrate `MIGRATE` sites family by family (economy, medical, world, narrative)
  to the `IFileSystem` port; loaders keep the same error collection contract.
- **Acceptance:** no direct `File.ReadAllText` in the migrated families;
  focused catalog tests stay green; no behavior change.
- **Verify:** `python3 scripts/ci/generate-authority-reachability.py --check`
  + per-family `bash scripts/run_test.sh`.

### AB-31C — Namespace direction
- Enforce dependency direction: `Ashfall.Core` may not reference
  `AtomicWar.GodotApp` (host), and host partials may not be required by Core.
  Verify no `Core → host` type references exist and no host type leaks into a
  Core public signature (interfaces/ports are Core-owned).
- **Acceptance:** zero Core→host type references; ports are the only seam.
- **Verify:** a new direction gate script.

### AB-31D — Boundary gate
- One fast-tier gate: engine refs (hard fail), Core→host refs (hard fail),
  new direct-path sites outside an allowlist (fail with file:line), and
  `netstandard2.1` API validation (no net8-only APIs in Core).
- **Acceptance:** intentionally adding `File.ReadAllText("/tmp/x")` to a Core
  non-loader fails the gate with the reason.

### AB-31E — Serialization boundary
- Confirm Core owns DTO shapes but never chooses host serializers; hosts pass
  options (e.g. `SystemTextJsonSerializer`) in. Record the one allowed pattern
  and flag exceptions.
- **Acceptance:** Core APIs do not instantiate engine or host serializer types;
  codecs accept streams/strings.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Migration churn across 487 sites | classify first; migrate only `MIGRATE`; many sites will be `ALLOWED` |
| Gate blocks legitimate dev tooling | `KEEP_WITH_REASON` allowlist with owner + reason |
| Loader error behavior changes | same collected-errors contract; focused suites per family |
| Core headless demos break | they may keep direct IO as dev-only with the reason recorded |

## 5. Verification

```bash
python3 scripts/ci/generate-core-io-boundary.py --check
python3 scripts/ci/generate-authority-reachability.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
bash scripts/run_test.sh Ashfall.Core.Tests/World/
bash scripts/ci/forbidden-api-gate.sh
```

---

## 6. Expanded census (bespoke: IO boundary surface)

This plan guards the Core/host boundary, so the census finds every file that
touches file IO or serialization.

| Metric | Value |
|---|---:|
| Files with IO calls | 740 |
| IO call sites | 2262 |

**Top IO files:**

| File | IO sites |
|---|---:|
| `host:Host/HostCli.PanelTests.cs` | 64 |
| `host:Host/HoldfastTradeSaveStoreSelfTest.cs` | 41 |
| `host:Host/ContentUtilizationRuntimeCollector.cs` | 39 |
| `host:Host/SaveLoadHostSession.cs` | 32 |
| `host:Host/SaveLoadUiFailureSelfTest.cs` | 26 |
| `HostDefaults.cs` | 24 |
| `YearOfAsh/YearOfAshCatalogLoader.cs` | 16 |
| `host:Host/GodotFileIO.cs` | 16 |
| `Content/ContentUtilizationScanner.cs` | 15 |
| `host:Main.Plans50_53.cs` | 15 |
| `Inventory/ItemCatalogLoader.cs` | 14 |
| `Narrative/BlackProjectsCatalog.cs` | 13 |
| `Narrative/HydroGeologyCatalog.cs` | 13 |
| `host:Host/HostCli.ExportParity.cs` | 13 |
| `host:Audio/AudioSelfTest.cs` | 13 |
| `Narrative/AbyssalAnomaliesCatalog.cs` | 12 |
| `Narrative/GrainMillingCatalog.cs` | 12 |
| `host:Host/HostCli.Mods.cs` | 12 |
| `host:Host/HostCli.WorldPlaytest.cs` | 11 |
| `Narrative/ApicultureBeeCatalog.cs` | 10 |

## 7. Expanded surface: boundary contract

| Rule | Detail |
|---|---|
| Core engine-free | Core takes values/DTOs; no `File`, `Directory`, engine serializers |
| IO owner | host adapters own all disk access through the documented loader path |
| Injection | Core paths accept streams/strings from the host, never open files |
| Gate | a static scan fails on new IO calls inside `Assets/Ashfall.Core` |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Static scan | IO sites outside the allowlist = 0 |
| Loader path | every catalog load test binds to the host loader |
| Fixture | injected IO in Core fails the scan |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) and allowlist (loader files only).
2. Move stray IO behind host adapters.
3. Wire the static boundary gate.
4. Regression: scan + focused loader tests.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Core file | no IO/serializer references (scan clean) |
| Loader | owns its IO; documented path |
| Gate | fails on injected IO in Core |
| Fixture | loader test passes through the host seam |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not move files between projects.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 15. Other plans referencing them: **26**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 3 |
| `PLAN-LAUNCH-FACE-06` | 2 |
| `PLAN-RUNTIME-PERF-16` | 2 |
| `PLAN-RELEASE-OPS-20` | 2 |
| `PLAN-INPUT-HARDENING-25` | 2 |
| `PLAN-FIELD-DISCOVERY-TRUTH-237` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `AGENTS.md` |
| `Assets/Ashfall.Core/MathfCompat.cs` |
| `CI_GATE_MANIFEST.json` |
| `Content/ContentUtilizationScanner.cs` |
| `HostDefaults.cs` |
| `Inventory/ItemCatalogLoader.cs` |
| `Narrative/AbyssalAnomaliesCatalog.cs` |
| `Narrative/ApicultureBeeCatalog.cs` |
| `Narrative/BlackProjectsCatalog.cs` |
| `Narrative/GrainMillingCatalog.cs` |
| `Narrative/HydroGeologyCatalog.cs` |
| `PLAN-ARCHITECTURE-BOUNDARY-31_APPENDIX-A_IO_INVENTORY.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `AB-31A` | `PLAN-ARCHITECTURE-BOUNDARY-31_APPENDIX-A_IO_INVENTORY.md`, `docs/architecture/CORE_IO_BOUNDARY.md` |
| `AB-31B` | `Inventory/ItemCatalogLoader.cs`, `YearOfAsh/YearOfAshCatalogLoader.cs`, `Narrative/AbyssalAnomaliesCatalog.cs` |
| `AB-31C` | no name match — resolve at claim time |
| `AB-31D` | `PLAN-ARCHITECTURE-BOUNDARY-31_APPENDIX-A_IO_INVENTORY.md`, `docs/architecture/CORE_IO_BOUNDARY.md` |
| `AB-31E` | `PLAN-ARCHITECTURE-BOUNDARY-31_APPENDIX-A_IO_INVENTORY.md`, `docs/architecture/CORE_IO_BOUNDARY.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 15. Host files: **97** · Test files: **74** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 97 | `src/Audio/AudioCueCatalog.cs`, `src/Audio/AudioSelfTest.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/CatalogPath.cs`, `src/Host/ChemicalReconHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 74 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/ApicultureBeeCatalogTests.cs`, `Ashfall.Core.Tests/BlackProjectsCatalogTests.cs`, `Ashfall.Core.Tests/BoneHornRuntimeActivationTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/standing_gates.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **14** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `black_market` |
| `black_projects_archive` |
| `campaign` |
| `campaign_day` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `foundry` |
| `grain_milling_archive` |
| `grain_processing` |
| `inventory` |
| `recon_telemetry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **17** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--black-flotilla-selftest` |
| `--campaign-journey-selftest` |
| `--chemical-dependency-save-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--selftest-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemDegraded` | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` |
| `OnItemRemoved` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnStandingCalled` | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` |
| `OnStandingPenalty` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/anomalies.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (182 files, 1391 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Factions` | 10 | 72 |
| `Flagship11` | 7 | 63 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 1391 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Campaign`, `Factions`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **233**
(23 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **38**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `crossing` | no |
| `disease` | no |
| `encounters` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |
| `disease` |
| `foundry` |
| `mineral_chemical` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **148**
(CODEX_ONLY 76, GAMEPLAY_CONSUMED 50, OPTIONAL 4, UNRESOLVED 18).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |

**Verdict:** 18 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** hub · **Coupling (incoming plans):** 26
**Surface:** save sections 38 (laddered 1) · RNG streams 9 · host files 24 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ARCHITECTURE-BOUNDARY-31
wave: —
status: PROPOSED — foreman claim required
packages: AB-31A, AB-31B, AB-31C, AB-31D, AB-31E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/anomalies.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 26 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: wave.
