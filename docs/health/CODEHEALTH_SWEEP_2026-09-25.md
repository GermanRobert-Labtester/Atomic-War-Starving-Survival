# ASHFALL Codehealth & Hardening Sweep — 2026-09-25 (findings only)

> **Mode:** read-only findings sweep. Repairs after this report are delegated to
> the concurrent deepseek v4.1 flash session. Claim:
> `claim-debloat-harden-deep-sweep-2026-09-25` (`DEBLOAT-HARDEN-DEEP-SWEEP`).
> Base SHA `2549b37d`, dirty integration worktree (500+ files from the
> 2026-09-24 packages). All times EEST.

## 0. Split of work observed in this session

| Lane | Owner | Status |
|---|---|---|
| Dead-type debloat (30 Core types), `ResolvedFeedbackMessage.TimestampTick` (wall-clock) removal, CatalogPath migration (20 files), port-contract 6 seams (301→307), parity-matrix 2 rows | this sweep (landed + verified before mode switch) | DONE |
| Catch-policy docs (7 sites), `MemorialSystem` seed hardening, `ChildDevelopment` triad allowlist, kilnworks determinism test, `skills.json`/`shelter_construction.json` data edits, `HostCli.cs`/`HostCliHelpContractTests.cs` rewrite | deepseek v4.1 flash (concurrent) | in flight |

No file-level overlap occurred. `Tooling` 123/123 green at 00:30 after all
interleaved edits.

---

## 1. ERRORS (build / gate failures — current, reproducible)

### ERR-01 — Host build RED: 24 × CS0117 in `Main.Application.cs`
**Severity: HIGH. Confidence: HIGH.**
- `dotnet build Ashfall.csproj` → 0 warnings / **24 errors**,
  `src/Main.Application.cs(743..819)`: `'HostCliAction' does not contain a
  definition for '<X>SelfTest'` for 24 probe actions
  (DependencyTaper, Antenatal, ClinicalWard, ChemicalReagent, MechanicalDriveline,
  SleepAcoustic, ShelterArchive, DreamSystem, AccessibilitySettings, MemoryDecay,
  InterpersonalConflict, Exercise, AfflictionBridge, RadiationMutation,
  RadioProduction, WorkingAnimals, BlackMarket, CultureCreation,
  PsychologicalProfile, SkillCertification, ChildDevelopment, Bestiary,
  HealthHistory, LeadershipSuccession).
- Cause: `src/Host/HostCli.cs` was rewritten at 00:11:40 (now only +35 lines vs
  HEAD); its `AtomicWar.GodotApp.HostCliAction` enum lost the 24 members that
  the (older, uncommitted) `Main.Application.cs` switch still references.
  `Ashfall.Core.Tests/HostCliHelpContractTests.cs` was restored to HEAD in the
  same window.
- **Secondary runtime gap:** the matching `--*-selftest` flag parsing is absent
  from `HostCli.cs` (spot-checked ≥12 flags: `--shelter-archive-selftest`,
  `--dream-system-selftest`, `--accessibility-settings-selftest`,
  `--memory-decay`, `--interpersonal-conflict`, `--exercise`, `--bestiary`,
  `--health-history`, `--leadership`, `--child-development`, `--black-market`,
  `--radio-production` …). Those probes are unreachable even after recompiling.
- **Structural note (Rule 5):** two enums named `HostCliAction` exist —
  `Assets/Ashfall.Core/HostCliRegistry.cs:15` (`namespace Ashfall.Core`, has
  all members, mtime 09-24 21:03) and `src/Host/HostCli.cs:32`
  (`namespace AtomicWar.GodotApp`). This looks like an in-flight consolidation
  by deepseek (Core registry is the likely single authority); until host enum,
  `Main.Application.cs` switch, and `Parse()` agree, the build stays red and
  25+ selftest flags stay dead.
- **Action for deepseek:** complete or revert the consolidation; verify with
  `dotnet build Ashfall.csproj` 0/0, then a probe smoke (`--shelter-archive-selftest`).

### ERR-02 — `agent-fast-verify.py` 9/10: DocsIndexDrift FAIL
**Severity: MED. Confidence: HIGH.**
- `docs/INDEX.md` is stale. Unindexed markdown (measured 00:29):
  - `docs/plans/FIFTEEN_PARTIAL_AUTHORITY_INTEGRATION_PLANS_16_30_CLOSEOUT_2026-09-24.md`
  - `docs/plans/FIFTEEN_PARTIAL_AUTHORITY_INTEGRATION_PLANS_CLOSEOUT_2026-09-24.md`
  - `docs/plans/TEN_CORE_ONLY_MEDICAL_RAIL_DEFENSE_TROPHY_INTEGRATION_CLOSEOUT_2026-09-24.md`
  - `docs/plans/TEN_ORPHAN_BRANCH_AND_WARD_INTEGRATION_PLANS_CLOSEOUT_2026-09-24.md`
  - `docs/plans/wave4_integration/.w4e1ah.md`, `.w4e1x.md`, `.w4e1y.md`
    (hidden temp drafts — decide: delete/quarantine or exclude dotfiles in the generator)
  - plus this report (`docs/health/CODEHEALTH_SWEEP_2026-09-25.md`) once counted.
- Fix = run `python3 scripts/ci/generate-docs-index.py` (generator, never hand-edit).
- All 9 other fast gates PASS (SceneLint, DocLinkPortability, AgentRulebooksSync,
  CoreSystemsCatalog, CatalogRegistry, UiPanelCatalog, ExpansionsCatalog,
  AudioCatalog, AgentSkillsCatalog).

---

## 2. GAPS (silent/unreachable — no build signal)

### GAP-01 — Authored data orphan: `acoustic_triangulation_catalog.json`
**Severity: MED-HIGH. Confidence: HIGH.** No `*.cs` in `Assets/Ashfall.Core`
or `src` references the file name. The whole
`AcousticDirectionFindingCatalog` cluster (`…CatalogDto`, loader) has zero
production references (loader was removed by this sweep only after zero-ref
proof). The authored JSON is dead content until a consumer is wired or the
file is retired.

### GAP-02 — `cupola_foundry_catalog.json` unreachable in production
**Severity: MED. Confidence: HIGH.** Zero code consumers of the file name;
`CupolaFoundryEngine` is constructed only from
`Ashfall.Core.Tests/Shelter/CupolaFoundryEngineTests.cs` (tests). The 09-24
Expansion 31 claim explicitly **deferred** the kiln→CupolaFoundry handoff
("needs a public material-application seam that does not exist today"), so this
is a known-but-open deferment — the catalog + engine remain test-only.

### GAP-03 — `VerticalAscent` cluster fully dead
**Severity: LOW-MED. Confidence: HIGH.** `VerticalAscentCatalog` has no
external consumers; `VerticalAscentCatalogDto` is now declaration-only
(orphaned when this sweep removed the dead `VerticalAscentCatalogLoader`).
Pre-existing unreachable cluster, now cleanly deletable as a unit.

### GAP-04 — 27 remaining zero-reference Core types (classified for the next lane)
Measured after this sweep's 30 removals (occurrence census over every `.cs`,
plus non-`.cs`/docs/scripts scan):
- **Do NOT delete — extension containers with live methods** (class name never
  appears, methods are called): `MineFlailCatalogMapping` (`ToModuleDefs` ×3),
  `RailGrindingCatalogMapping` (`ToHeadDefs` ×2), `MicrofluidicDiagnosticCatalogMapping`
  (`ToAssayDefs` ×3), `EbPvdCoatingCatalogMapping` (`ToCoatingDefs`/`ToFailureProfiles`/
  `TryGetSubstrateResultItem`), `RailwayInterlockExtensions` (`HasValueSafe` ×2),
  `PsyOpsCounterSaveEntryExtensions` (`ToState` ×1).
- **Do NOT delete:** `IsExternalInit` (compiler polyfill for `init` on netstandard2.1).
- **Port-contract cited:** `CatalogIntegrityDefinitionChecker` — cited in
  `docs/architecture/port-contract.json` AND `docs/ci/port_contract_policy.json`;
  note the contract contains **duplicate entries** `ports[25]` and `ports[26]`
  for the same class+file (see GAP-06).
- **Docs-cited dead seams (foreman/foredoc decision before removal):**
  `IWeatherSeverityProvider` (own file!), `WaterRequestContracts`,
  `EducationLevel`, `DebtRouteAccessResolver`, `RouteAvailabilityPresentation`,
  `WeatherGateResult`, `HoldfastQuests` (file is 1184 lines), `IBriefingFactCollector`,
  `PerfTestMarker`, `ICampaignSaveSection`, `PortContractDefinition`, `IPortValidator`,
  `CurrentsPamphletCatalog`, `WireConfessionCatalog`, `NarrativeDiscoveryManifestFile`,
  `EquippedGearData`.
- **Collateral from this sweep's deletions (freshly orphaned):**
  `VerticalAscentCatalogDto`, `AcousticDirectionFindingCatalogDto`,
  `CupolaFoundryCatalogDto` — see GAP-01/02/03.

### GAP-05 — Data authority: open known debt unchanged
`DEBT-ENRICHMENT-KEEPSAKE-ORPHANS` still open: 65 of 76 authored
`personal_keepsake_item_id` values resolve to no item catalog row (measured by
`--origin-mechanics-selftest` per KNOWN_DEBT). Also open:
`DEBT-PLAN181-DIFFICULTY-RUNTIME-UI` (panel-route seams),
`DEBT-PLAN24-SNAPSHOT-REBASELINE` (needs renderer-capable session).

### GAP-06 — Port-contract generator emits duplicate seam entries
`docs/architecture/port-contract.json` `ports[25]` and `ports[26]` are both
`CatalogIntegrityDefinitionChecker` / `Assets/Ashfall.Core/CatalogIntegrityCheckers.cs`
(identical reason text). Policy JSON has no duplicate (generator de-dupes
policy-side keys but apparently emitted the Core seam twice — likely two
detected methods mapping to one class row, or double `Bind*`/`Register*`
matches). Cosmetic today; will inflate seam counts over time.

---

## 3. WARNINGS / ERROR-HYGIENE STATUS

- **Compiler warnings:** Core 0, tests 0, host 0 (host fails on ERR-01 before
  warnings matter).
- **Bare/undocumented catches:** catch-policy gate GREEN (123/123) after
  deepseek documented the 7 HostCli probe catches. Remaining92
  comment-only catches are annotated fallbacks; the save-path ones were
  reviewed and are policy-compliant:
  `SaveEnvelopeHelper.cs:200` (legacy-envelope fallback), `:240`
  (deserialization failure → legacy path), `CatalogDiagnostics.cs:62`
  (last-resort log-sink swallow, documented). No undocumented empty catch
  remains in Core or src.
- **Core purity / determinism scan (this sweep):** clean. All `DateTime.UtcNow`
  hits are the `DETERMINISM_ALLOWLIST`-tagged `IWallClock` port adapter;
  `Directory.GetCurrentDirectory` hits are sanctioned data-directory discovery
  (`CatalogLocator`/headless demos); no `System.Random`, no `Guid.NewGuid`, no
  untagged wall-clock in Core. `Environment.TickCount64` (dead
  `TimestampTick`) removed by this sweep.

---

## 4. VERIFICATION EVIDENCE (this sweep)

| Check | Result |
|---|---|
| `dotnet build Ashfall.Core` | 0 warnings / 0 errors |
| `dotnet build Ashfall.Core.Tests` | 0 warnings / 0 errors |
| `dotnet build Ashfall.csproj` (host) | **24 errors — ERR-01** |
| `scripts/run_test.sh Ashfall.Core.Tests/Tooling` (123 gates) | **123/123 PASS** (00:30) |
| `CatalogIntegrityValidatorTests` | 10/10 PASS (transient data race during deepseek's `skills.json` edit resolved) |
| `CoreInvariantSourceTests` | 4/4 PASS |
| `Content/ContentOrphanCertificationEngineTests` | 4/4 PASS |
| `Lifecycle/SessionLifecycleRegistryTests` | 5/5 PASS |
| `Launch/Plan57StoreKitTruthIntegrationTests` | 5/5 PASS |
| `python3 scripts/ci/generate-port-contract.py --check` | PASS — 307 seams, 0 errors, 201 host-required bound |
| `agent-fast-verify.py` | 9/10 (FAIL: DocsIndexDrift — ERR-02) |

**Not run (policy):** full test suite (needs explicit foreman/user reason);
host CLI selftests (`--*-selftest`) — blocked by ERR-01 until the host builds.

## 5. RACE / CONCURRENCY NOTES FOR THE REPAIR LANE

- Concurrent deepseek v4.1 flash edits observed 00:09–00:30: 7 catch docs,
  `MemorialSystem.cs:246` seed → `StableHash.Combine`, `MainTriadDriftGateTests`
  allowlist, `KilnFiringLedgerTests` kilnworks determinism case, `skills.json`
  + `shelter_construction.json` (`skill_crafting` added), and the
  `HostCli.cs`/`HostCliHelpContractTests.cs` rewrite (ERR-01).
- This sweep's landed edits remain intact at 00:30 (verified:
  `CatalogPath.ResolveDataDir` present in migrated files, Tooling 123/123).
- Recommended order for deepseek: **ERR-01 first** (host build + flag parse),
  then ERR-02 (regenerate docs index), then GAP-01/02 content-custody decisions.
