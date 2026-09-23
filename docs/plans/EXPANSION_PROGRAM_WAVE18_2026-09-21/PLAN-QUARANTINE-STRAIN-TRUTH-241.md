# PLAN-QUARANTINE-STRAIN-TRUTH-241 — Isolation Execution & Strain Variants

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-MICROFLUIDIC-DIAGNOSTIC-TRUTH-182, PLAN-SHELTER-CAPACITY-AUTHORITY-103.
**Non-goals:** no outbreak model (Plan 47), no test engine (Plan 182), no
occupancy model (Plan 103).

## 1. Outcome
Two reachable systems are unaddressed: `Disease/DiseaseQuarantineCoordinator.cs`
(**376**) and `Disease/PathogenStrainSystem.cs` (**361**). Plan 47 owns outbreak
response; what it does **through** — which rooms isolate whom, how a strain
varies, and when isolation ends — is unowned, so quarantine is either a button
or an invisible penalty.

| Deliverable | Detail |
|---|---|
| Isolation model | isolation zones from rooms (Plan 103); a quarantined person is visibly placed and occupied |
| Strain variants | strains with documented traits (severity band, transmission band); variants derive from Plan 47's model, never a parallel one |
| Entry/exit rules | admission by diagnosis (Plan 182) or exposure; release by cleared condition only |
| Cost truth | isolation consumes room capacity and supplies; effects route to owners |
| Save truth | placements and strain state restore; no re-roll on load |

## 2. Evidence
- The two files above (unaddressed — Wave 18 audit).
- Plan 47 owns the outbreak; Plan 182 supplies confirmation.
- Plan 103 supplies rooms; Plan 93 verifies supplies.

## 3. Packages
- **QST-241A** isolation model + placement tests.
- **QST-241B** strain variant table (bounded) + trait tests.
- **QST-241C** admission/release rules + cleared-condition fixture.
- **QST-241D** capacity/supply cost routing.
- **QST-241E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Placements occupy rooms exactly; release requires its condition.
- Strain traits stay within Plan 47's bands; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Disease/` (create if absent).

## 5. Risks
Invisible penalty → placement and occupancy are visible.
Model duplication → traits derive from Plan 47; the boundary is asserted.

---

## 6. Expanded census (11 files · 4,411 lines)

Scope: `Assets/Ashfall.Core/Disease/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Demo 1 · Save 1 · Support 4 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DiseaseCatalog.cs` | 632 | Catalog | — | 0 | 0 | 0 |
| `DiseaseHeadlessDemo.cs` | 490 | Demo | — | 0 | 0 | 11 |
| `DiseaseQuarantineCoordinator.cs` | 376 | System | **yes** | 0 | 0 | 0 |
| `DiseaseSystem.cs` | 1707 | System | — | 1 | 0 | 3 |
| `DiseaseTriage.cs` | 263 | Support | — | 0 | 0 | 0 |
| `IDiseaseOutbreakSource.cs` | 80 | Support | — | 0 | 0 | 0 |
| `PathogenStrainCatalog.cs` | 71 | Catalog | — | 0 | 0 | 0 |
| `PathogenStrainSave.cs` | 113 | Save | — | 0 | 0 | 0 |
| `PathogenStrainSystem.cs` | 361 | System | **yes** | 0 | 0 | 2 |
| `DiseaseAfflictionHandler.cs` | 177 | Support | — | 0 | 0 | 0 |
| `DiseaseProtocolHandler.cs` | 141 | Support | — | 0 | 0 | 0 |

**Totals:** 1 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `pathogens.json` | object[3 keys] |
| `crop_strains.json` | object[4 keys] |
| `disease_catalog.json` | object[5 keys] |

**State surfaces:** `DiseaseHeadlessDemo.cs`, `DiseaseSystem.cs`, `PathogenStrainSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Disease/` (create if absent) |
| Test references | 74 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 11. Other plans referencing their names: **9**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 11 |
| `PLAN-TRIO-FAMILY-TRUTH-280` | 9 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 3 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-ECOLOGY-WILDLIFE-26` | 1 |
| `PLAN-WATER-AGRICULTURE-46` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `QST-241A` | no name match — resolve at claim time |
| `QST-241B` | `PathogenStrainCatalog.cs`, `PathogenStrainSave.cs`, `PathogenStrainSystem.cs` |
| `QST-241C` | no name match — resolve at claim time |
| `QST-241D` | no name match — resolve at claim time |
| `QST-241E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 11; intra-domain edges: **14**; isolated files:
**1**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DiseaseAfflictionHandler` | `DiseaseCatalog` |
| `DiseaseAfflictionHandler` | `DiseaseSystem` |
| `DiseaseCatalog` | `DiseaseSystem` |
| `DiseaseHeadlessDemo` | `DiseaseCatalog` |
| `DiseaseHeadlessDemo` | `DiseaseSystem` |
| `DiseaseProtocolHandler` | `DiseaseSystem` |
| `DiseaseQuarantineCoordinator` | `DiseaseSystem` |
| `DiseaseSystem` | `DiseaseCatalog` |
| `DiseaseSystem` | `DiseaseTriage` |
| `DiseaseSystem` | `IDiseaseOutbreakSource` |
| `DiseaseTriage` | `DiseaseSystem` |
| `IDiseaseOutbreakSource` | `DiseaseSystem` |
| `PathogenStrainCatalog` | `DiseaseCatalog` |
| `PathogenStrainSystem` | `DiseaseSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `DiseaseSystem` | 8 |
| `DiseaseCatalog` | 4 |
| `DiseaseTriage` | 1 |
| `IDiseaseOutbreakSource` | 1 |
| `DiseaseAfflictionHandler` | 0 |
| `DiseaseHeadlessDemo` | 0 |
| `DiseaseProtocolHandler` | 0 |
| `DiseaseQuarantineCoordinator` | 0 |
| `PathogenStrainCatalog` | 0 |
| `PathogenStrainSave` | 0 |

**Class split:** hub 4 · sink 0 · source 6 · isolated 1.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 11. Host files: **18** · Test files: **33** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 18 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Disease/DiseaseHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/DiseaseOutbreakHostAdapter.cs` |
| Tests (`Ashfall.Core.Tests/`) | 33 | `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/CrisisPresentationCoordinatorTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/DiseaseCatalogExpansionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `disease` |
| `pathogen_strains` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--disease-expansion-selftest` |
| `--disease-selftest` |
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnBlightOutbreak` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnOutbreakContained` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnOutbreakDeclared` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnPathogenExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnQuarantineEnded` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnQuarantineStarted` | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` |
| `OnStrainPlanted` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/decontamination_protocol_catalog.json` |
| `Assets/StreamingAssets/Data/disease_catalog.json` |
| `Assets/StreamingAssets/Data/documents/vel_triage_log_names.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **13**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DiseaseOutbreakHostAdapter.cs` |
| `src/Host/DiseaseSaveStore.cs` |
| `src/Host/PathogenStrainSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.MedicalTriage.cs` |
| `src/Main.PathogenStrains.cs` |
| `src/UI/AmputationTriagePanel.cs` |
| `src/UI/OpeningProtocolModal.cs` |
| `src/UI/OpeningProtocolModalContent.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `disease` | no |
| `pathogen_strains` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `aquaponics_disease` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(GAMEPLAY_CONSUMED 2, OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `decontamination_protocol_catalog.json` | GAMEPLAY_CONSUMED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `documents/vel_triage_log_names.json` | OPTIONAL |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 2 (laddered 0) · RNG streams 2 · host files 14 · catalogs 7 · test regions 0 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-QUARANTINE-STRAIN-TRUTH-241
wave: 18
status: PROPOSED — foreman claim required
packages: QST-241A, QST-241B, QST-241C, QST-241D, QST-241E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DiseaseOutbreakHostAdapter.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/decontamination_protocol_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --disease-expansion-selftest
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
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

**Pre-claim actions:** none — claim-ready.
