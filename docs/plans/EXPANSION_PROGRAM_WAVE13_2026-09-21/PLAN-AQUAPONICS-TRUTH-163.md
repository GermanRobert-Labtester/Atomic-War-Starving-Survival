# PLAN-AQUAPONICS-TRUTH-163 — Closed-Loop Food Production: Fish, Beds & Water

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WATER-AGRICULTURE-46, PLAN-FOOD-CUISINE-39, PLAN-WATER-QUALITY (Plan 46 packages), PLAN-PRESERVATION-TRUTH-118.
**Implementation scaffold:** [`PLAN-AQUAPONICS-TRUTH-163_APPENDIX-A_SCAFFOLD.md`](PLAN-AQUAPONICS-TRUTH-163_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-WATER-AGRICULTURE-46` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no soil farming (Plan 46), no cooking (Plan 39), no new fish
catalog beyond existing data.

## 1. Outcome
`Shelter/AquaponicsSystem.cs` (813 lines) and `Shelter/HydroponicBiomeSystem.cs`
(441 lines) are reachable, unaddressed production systems. Aquaponics is the
one loop where **water, nutrients, and food** are the same input — so its
contract must state mass balance with the water and food owners, or it becomes
a free-food machine.

| Deliverable | Detail |
|---|---|
| Loop state | fish stock, bed biomass, water volume/quality — each with its owner (inventory for physical stock, water owner for quality) |
| Mass balance | feed in → fish growth → waste → plant uptake → output; a balance test proves no free matter |
| Water coupling | consumption and quality effects route through Plan 46's water owner; no private water pool |
| Failure modes | die-off, contamination, power loss (pump) with documented partial-loss outcomes — never total silent loss |
| Save truth | loop state restores; a load never re-rolls growth or die-off |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs` (813 lines) and `Shelter/HydroponicBiomeSystem.cs` (441 lines): both unmentioned in every plan body — Wave 13 audit.
- `Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs` (528 lines) is the third sibling and is folded into this plan's scope; if a claim separates it, it must carry the same mass-balance and water-coupling rules.
- Plan 46 owns water quality and agriculture; Plan 118 owns preservation of outputs.
- Plan 48 owns power; a pump dependency routes there.
- Inventory remains the physical stock authority (Plan 93).

## 3. Packages
- **AQT-163A** loop-state model + owner table.
- **AQT-163B** mass-balance test over a scripted month (inputs vs outputs within tolerance).
- **AQT-163C** water coupling through Plan 46's owner (no private pool proof).
- **AQT-163D** failure-mode table + one fixture per mode.
- **AQT-163E** save round-trip + no re-roll on load.

## 4. Acceptance & verification
- Mass balance holds; a deliberately injected leak fails the test.
- Water effects are visible in Plan 46's values only.
- Die-off and contamination produce the documented partial outcomes.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Free-food loop → mass balance is the permanent guard.
Overlap with 46 → this is a production facility; water quality stays with its owner.

---

## 6. Expanded census (4 files · 1,931 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AeroponicsSystem.cs` | 528 | System | **yes** | 0 | 0 | 2 |
| `AquaponicsSystem.cs` | 813 | System | **yes** | 0 | 0 | 2 |
| `HydroponicBiomeSystem.cs` | 441 | System | **yes** | 0 | 0 | 2 |
| `HydroponicCropCatalog.cs` | 149 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `aeroponics_nutrient_catalog.json` | object[2 keys] |
| `aquaponics_system_catalog.json` | object[7 keys] |
| `hydroponic_crops.json` | object[2 keys] |

**State surfaces:** `AeroponicsSystem.cs`, `AquaponicsSystem.cs`, `HydroponicBiomeSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 10 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
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

Domain files: 4. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `EVIDENCE` | 1 |
| `PLAN-ENERGY-NUCLEAR-48` | 1 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AQT-163A` | no name match — resolve at claim time |
| `AQT-163B` | no name match — resolve at claim time |
| `AQT-163C` | no name match — resolve at claim time |
| `AQT-163D` | no name match — resolve at claim time |
| `AQT-163E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **4** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.PlansB86_B89.cs`, `src/Host/Plans74To77HostSessions.cs`, `src/Main.AdvancedShelterSystems.cs`, `src/Main.PlansB86_B89.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/ContentUtilizationGraphTests.cs`, `Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs`, `Ashfall.Core.Tests/Plans74To77SystemsTests.cs`, `Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs`, `Ashfall.Core.Tests/Shelter/AquaponicsSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **1**; isolated: **2**.

| From | → To |
|---|---|
| `HydroponicBiomeSystem` | `HydroponicCropCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `aeroponics` |
| `aquaponics` |
| `hydroponic_biomes` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--aquaponics-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnCropFailed` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCropHarvested` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCropMatured` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |
| `OnCropPlanted` | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/aeroponics_nutrient_catalog.json` |
| `Assets/StreamingAssets/Data/aquaponics_system_catalog.json` |
| `Assets/StreamingAssets/Data/crop_strains.json` |
| `Assets/StreamingAssets/Data/hydroponic_crops.json` |
| `Assets/StreamingAssets/Data/narrative/crop_experiment_logs.json` |
| `Assets/StreamingAssets/Data/narrative/crop_genome_degradation_reports.json` |

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

Host files (`src/`) whose names share a domain token: **2**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/HydroponicBiomeSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `aeroponics` | no |
| `aquaponics` | no |
| `hydroponic_biomes` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `aquaponics_disease` |
| `aquaponics_fry_survival` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `hydroponic_crops.json` | GAMEPLAY_CONSUMED |
| `narrative/crop_experiment_logs.json` | CODEX_ONLY |
| `narrative/crop_genome_degradation_reports.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 3 (laddered 0) · RNG streams 2 · host files 4 · catalogs 9 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AQUAPONICS-TRUTH-163
wave: 13
status: PROPOSED — foreman claim required
packages: AQT-163A, AQT-163B, AQT-163C, AQT-163D, AQT-163E
claim paths:
  - src/Host/AquaponicsSaveStore.cs  # §19 candidate host surface
  - src/Host/HydroponicBiomeSaveStore.cs  # §19 candidate host surface
  - aquaponics_disease  # §19 candidate host surface
  - aquaponics_fry_survival  # §19 candidate host surface
  - Assets/StreamingAssets/Data/aeroponics_nutrient_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/aquaponics_system_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --aquaponics-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
