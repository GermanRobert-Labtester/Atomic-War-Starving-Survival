# PLANS 66–69 FLAGSHIP RECONNAISSANCE (Wave 0)

**Branch:** `feat/asset-pipeline-flagship` (heavy concurrent modification — see Baseline)
**Status:** Reconnaissance complete. Implementation NOT started.
**Verdict:** ⚠️ The plan requires re-scoping before implementation. Plan numbers 66–69 are already taken, and large parts of the proposed mechanics already exist in shipped systems.

---

## 1. CRITICAL FINDING — Plan number collision

Plan numbers 66–69 are **already consumed** by shipped/closeout work:

| Number | Already means | Evidence |
|---|---|---|
| 66 | Guilt sources expansion | `docs/psych/PLAN66_CLOSEOUT.md`, `docs/plans/66-guilt-sources-expansion.md` |
| 67 | Cassette sets expansion | `docs/narrative/PLAN_67_CASSETTE_SETS_EXPANSION_CLOSEOUT.md` |
| 68 | Wall carving templates | `docs/shelter/PLAN68_CLOSEOUT.md`, `docs/plans/68-wall-carving-templates-expansion.md` |
| 69 | Grave epitaphs | `docs/memorials/PLAN69_BASELINE.md`, `docs/plans/69-grave-epitaphs-expansion.md` |

**Decision required from owner:** renumber this flagship wave (suggest **Plans 166–169**, next free flagship block after 162–165) or explicitly retire the old numbering. All closeout docs, catalogs, and test classes below use `166–169` placeholders.

---

## 2. Authority inventory vs. plan assumptions

### 2.1 Foundry / metallurgy (Plan 66) — MUCH already exists

The plan's Phase 0.1 gate ("extend rather than create a competing MetallurgySystem") triggers immediately:

| Existing authority | Location | Coverage |
|---|---|---|
| `SilentFoundrySystem` (688 lines) + `SilentFoundrySystem.Heat` partial (525) + `SilentFoundryTypes` | `Assets/Ashfall.Core/Foundry/` | **Already owns**: process heat, `refractoryLining` wear with critical-spall warnings, water-slag steam-vapor explosion hazard, mold reuse degradation (`moldReuseCount`), quality modifiers from lining/mold state, hearth tuyeres |
| `PowderMetallurgySystem` (328) + `powder_metallurgy_catalog.json` | same | feedstock costs, batch records, quality modifiers |
| `CupolaFoundryEngine` (444) + `cupola_foundry_catalog.json` | `Assets/Ashfall.Core/Shelter/` | secondary foundry loop |
| `CrucibleFoundryCatalog` / `MetallurgyToolingCatalog` | `Assets/Ashfall.Core/Narrative/` | crucible/tooling definitions |
| `foundry_items.json`, `foundry_production.json`, `foundry_accords.json`, `foundry_faction.json`, `foundry_treaty_consequences.json` | `Assets/StreamingAssets/Data/` | data authority already rich |
| `ExcavationSystem.StructuralBeamItemId = "item_foundry_t_beam"` | `Assets/Ashfall.Core/ExcavationSystem.cs` | **Cross-plan Scenario A (beam → deep-strata reinforcement) already shipped via Plans 90–93** (`reinforcedBeams` field) |

**True gaps for Plan 166 (metallurgy):**
- No `metallurgy_recipes.json`; no 12-recipe heavy roster (I-beams beyond `item_foundry_t_beam`, armor plate, spring steel, gear blanks, solder stock).
- No persistent multi-phase crucible state machine (`Charging→Heating→Refining→ReadyToPour→Pouring→Cooling`) — SilentFoundry has heat/hazard but not the full phase machine.
- No aggregate `slag_level` maintenance mechanic (slag appears only as flavor/hazard text).
- No dedicated ventilation-load handoff from foundry batches into `VentilationSystem` (verify before building).

**Architecture decision (per plan 66.1):** **Option A — extend `SilentFoundrySystem`** (new partial `SilentFoundrySystem.Metallurgy.cs` + new catalog), NOT a new `MetallurgySystem`.

### 2.2 Radio / cryptanalysis (Plan 67) — SUBSTANTIALLY exists

| Existing | Location | Coverage |
|---|---|---|
| **`radio_intercepts.json` — 16 authored intercepts** with `frequency_khz`, `band`, `base_signal_strength`, `encryption.scheme/difficulty`, `required_skill_ids` (`skill_signal_ear`, `skill_cold_analysis`), `triangulation.required_bearings: 3`, `revealed_location_id` | `Assets/StreamingAssets/Data/` | This IS Plan 67's core content, already authored (incl. decoy: `radio_intercept_spoofed_distress_trap_08`) |
| `SignalTriangulationSystem` (455 lines) | `Assets/Ashfall.Core/Radio/` | `RadioObservation` (bearing ± error, weather, operator skill), `TriangulationCandidate` (confidence, uncertainty radius), save DTO — matches plan 67.4/67.5/67.6 |
| `AcousticDirectionFindingCatalog`, `SignalIntelligenceCatalog`, `CipherQuestChainEngine`, `RadioSignalLog`, `RadioRecordingSystem` | `Assets/Ashfall.Core/Radio/`, `Narrative/` | ciphers/logs/sigint |
| `ShelterRadioStationSystem`, `RadioHostSession`, `RadioSaveStore`, `FactionRadioEngine` | Core + `src/Host` | station runtime + save |
| Consumers | `src/Main.Plans46_49.cs`, `ContentUtilizationScanner.cs` | already wired as **Plans 46–49** |

**True gaps for Plan 167 (radio):** audit `SignalTriangulationSystem` against the 16 intercepts' `triangulation` blocks; verify decoy → encounter handoff (`ExpeditionEncounterBridge`-style); verify solar/weather interference consumes `WeatherGate*` authority; verify map reveals go through `WastelandMapSystem` exactly-once. Possibly small; possibly already done — needs the targeted audit before any new class is written. **Do not create `radio_ciphers.json` before confirming `SignalIntelligenceCatalog` + `encryption.scheme/difficulty` don't already cover the 16-codebook requirement.**

### 2.3 Seismic / geology (Plan 68) — thinnest area, but frameworks exist

| Existing | Location |
|---|---|
| `GeologicalStrataCatalog`, `HydroGeologyCatalog` | `Assets/Ashfall.Core/Narrative/` |
| `ExcavationSystem` (cave-in, `structuralRisk`, shoring, beams) + `ExcavationHazardSystem` + save stores | `Assets/Ashfall.Core/` + `src/Host/` |
| `OrbitalHarrowTelemetrySystem` (330 lines) | `Assets/Ashfall.Core/` — orbital impact authority for coupling |
| `BoreholeSeismographPanel` | `src/UI/` — **UI-06 fake-success prototype**, not a real monitor |
| `PowerGridSystem` / `PowerDistributionSubgridSystem` | `Assets/Ashfall.Core/Shelter/` |
| `VentilationSystem`, `ShelterRoomConditionSystem` | Core |

**Missing:** no `geological_faults.json`, no `SeismicSimulationEngine`, no dampener/geophone mechanics, no P/S warning window, no fault-warning state. **Plan 168 is the genuine greenfield build** — but it must emit impulses into `ExcavationSystem`/`ShelterRoomConditionSystem`, never own damage.

### 2.4 Cryo preservation (Plan 69) — partial overlap

| Existing | Location |
|---|---|
| `SeedBankPreservationCatalog` (237) — ampoules, germination viability %, desiccation, heirloom viability | `Assets/Ashfall.Core/Narrative/` |
| `CryoPreservationCatalog` | `Assets/Ashfall.Core/Narrative/` |
| `CryogenicAirSeparationSystem` + host session + `cryogenic_air_separation.json` | Core + host — industrial gas/coolant production authority likely |
| `GreenhouseExpansionCatalog` (13 crops), `PharmaLabSystem`, `greenhouse_items.json` | canonical consumption endpoints |
| `shelter_insulation_catalog.json` | insulation authority (currently has **unresolved `insul_*` IDs** — see baseline) |

**Missing:** no `cryo_cultivars.json` (18 samples), no `CryoVaultSystem` canister/thermal/viability loop, no breach/triage. Same rule: preserve, don't duplicate — greenhouse owns cultivation, pharma owns medicine, cryo only stores.

---

## 3. Baseline regression (recorded 2026-09-06, this branch)

| Check | Result |
|---|---|
| `dotnet build Ashfall.csproj` | ✅ PASS — 0 warnings, 0 errors |
| `dotnet build Ashfall.Core.Tests` | ✅ PASS — 0 errors, 5 warnings (xUnit analyzer, pre-existing) |
| `dotnet test` | ⚠️ 8750 / 8755 passed — **5 pre-existing failures, all one root cause:** unresolved `insul_fiberglass_batts` / `insul_aerogel_composite` in `shelter_insulation_catalog.json` (`ExpeditionLootIntegrityTests`, `IndependentBranchCatalogTests`, `RebelBranchCatalogTests`, `MilitaryBranchCatalogTests`, `CatalogIntegrityValidatorTests.AllCatalogIdsCrossReferenceCleanly`) |
| Working tree | Heavily modified (test csproj + ~20 test files + agent rule files) — concurrent streams in flight |

**Gate:** the 5 failures are concurrent-stream damage, not ours — but they mask any cross-reference regressions this wave would introduce. **They must be fixed (or the insulation IDs registered) before PR 2 of this plan merges.** Note `shelter_insulation_catalog.json` is also the natural shielding/insulation authority for Plan 169.

---

## 4. Revised execution recommendation

| Wave | Content | Risk |
|---|---|---|
| 0 | ✅ this reconnaissance | done |
| 0.5 | Fix `insul_*` catalog breakage; owner decision on renumbering | blocker |
| 1 | **Plan 166 metallurgy**: `metallurgy_recipes.json` (12 recipes) + `SilentFoundrySystem.Metallurgy.cs` partial (phase machine, slag, ventilation handoff) + tests | medium |
| 2 | **Plan 168 seismic** (greenfield): `geological_faults.json` + `SeismicSimulationEngine` + dampeners/geophones + tests | high |
| 3 | **Plan 167 radio audit-then-extend**: reconcile `SignalTriangulationSystem`/16 intercepts vs. plan; fill gaps only | low-medium |
| 4 | **Plan 169 cryo**: `cryo_cultivars.json` + `CryoVaultSystem` + greenhouse/pharma handoffs + tests | medium-high |
| 5 | Cross-plan scenarios A–G + CI closure | — |

Order changed from the pasted plan (66→68→67→69) because radio is mostly built (audit only) while seismic is fully greenfield and blocks the cross-plan event fabric.

---

## 5. Questions blocking implementation

1. **Renumber to Plans 166–169?** (66–69 are taken — see §1)
2. **12-recipe roster item IDs:** reuse existing foundry item taxonomy (`item_foundry_t_beam` exists) — confirm or provide the new item list before authoring `metallurgy_recipes.json`.
3. **Radio:** is extending the existing Plans 46–49 intercept grid acceptable in place of the plan's new `RadioInterceptDef`/`radio_ciphers.json` architecture?
4. **The 5 red tests:** fix under this wave, or owned by the insulation stream?
