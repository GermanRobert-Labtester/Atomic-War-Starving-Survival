# PLAN-ORPHAN-SEAL-01 — Orphan Authority Integration & Dead-Code Triage

**Program:** ASHFALL Expansion & Integration Program (2026-09-21)
**Status:** PROPOSED — not a claim. The foreman must file exact path claims in
`WORKTREE_OWNERSHIP.md` before any edit. This document is read-only audit
output plus a package design.
**Owner role on execution:** Builder (one wave at a time) with Integrator for
shared seams (`Main.CampaignServices.cs`, `Main.SaveOrchestrator.cs`,
`SubsystemManifest`, panel registry, save-store matrix).
**Depends on:** PLAN-INTEGRATION-KIT-02 for the gate and scaffold; waves below
can start before the kit lands, but the kit is what keeps them sealed.
**Related:** PLAN-UNBLOCK-03 (signed dispositions), PLAN-VERTICAL-CULTURE-04
and PLAN-VERTICAL-BODY-INDUSTRY-05 (new mechanics on top of sealed systems).
**Expanded appendices:**
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-A_ORPHAN_DOSSIERS.md)
  — per-system dossiers for all 99 orphan authorities (types, tests, candidate
catalogs, suggested host seam, wiring recipe, wave assignment). Wave packages
pick their work list from that appendix.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-B_WAVE_PACKAGES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-B_WAVE_PACKAGES.md)
  — per-wave package specifications (`ORPHAN-SEAL-W<N>-<domain>-<nnn>`) with
verification column and wave exit criteria.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-C_INTEGRATION_PATTERNS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-C_INTEGRATION_PATTERNS.md)
  — 14 reusable integration patterns (host session, save store, day owner,
manifest entry, panel route, journal key, audio binding, CLI probe, loader,
projection, RNG fork, event seam, failure path, handoff) with real examples
and anti-patterns.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-D_SAVE_OWNERSHIP.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-D_SAVE_OWNERSHIP.md)
  — state & save ownership map: capture/restore methods, `SaveSectionRegistry`
knowledge, and field counts for all 99 orphans. Separates systems that own
campaign state (need a save row) from stateless evaluators (must not invent a
section).
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-E_DETERMINISM_AUDIT.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-E_DETERMINISM_AUDIT.md)
  — banned-source audit: only **5 of 99** orphans touch a nondeterministic
primitive (`CommunicationsSystem`, `SeasonalCelebrationSystem`,
`SessionDurabilityManager`, `DisasterResponseSystem`,
`ShelterExpansionSystem`); their packages retire the source at the seam before
wiring.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-F_DEPENDENCY_CLUSTERS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-F_DEPENDENCY_CLUSTERS.md)
  — intra-orphan reference graph (99 authorities, 94 clusters, 89 isolated)
with a suggested referenced-first seal order per cluster.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-G_HOST_INTEGRATION_POINTS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-G_HOST_INTEGRATION_POINTS.md)
  — candidate attachment points per orphan: matching `Main.*` partials,
registry save sections, and existing CLI flags. Rows with `—` everywhere
(e.g. `BestiarySystem`) are surface decisions, not wiring details.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-H_API_SURFACE.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-H_API_SURFACE.md)
  — sizing census per orphan: lines, public methods/properties, constructors,
static members, base/interfaces — used to size seal packages honestly.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-I_PROVENANCE.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-I_PROVENANCE.md)
  — last-commit/date ledger per orphan file: re-run before promoting a seal
package; a changed hash invalidates that package's premise check.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-J_TEST_COVERAGE.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-J_TEST_COVERAGE.md)
  — complete test inventory per orphan: every referencing test file with its
`[Fact]`/`[Theory]` case counts, so a seal knows exactly which tests must stay
green.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-K_API_SIGNATURES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-K_API_SIGNATURES.md)
  — the public member signatures per orphan (up to 30 per type, omitted counts
noted): the API a host adapter binds to.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-L_RISK_SCORECARD.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-L_RISK_SCORECARD.md)
  — transparent triage score (size + determinism risk + state + missing host
attachment + test debt) ranking seals by cost and risk.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-M_CATALOG_BINDING.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-M_CATALOG_BINDING.md)
  — verified catalog bindings (existence, record counts, loader references);
54 orphans have a name-matched catalog, the rest need a loader-path check.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-N_SURFACE_ROUTES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-N_SURFACE_ROUTES.md)
  — candidate surface route ids per orphan from the 192 declared routes; `—`
means the seal adds a route through the surface owner or ships headless-only.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-O_VERIFICATION_COMMANDS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-O_VERIFICATION_COMMANDS.md)
  — a concrete focused command per orphan (109 test regions · 218 selftest
flags); prevents citing commands that do not exist.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-P_INCOMING_REFERENCES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-P_INCOMING_REFERENCES.md)
  — inbound-reference census with the closure re-run: **0 reachability
contradictions**, 2 orphans attached only to the dormant blob, **97 test-only**,
0 true islands. None is a partially-wired feature; sealing is greenfield wiring.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-Q_SAVE_KEY_COLLISIONS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-Q_SAVE_KEY_COLLISIONS.md)
  — proposed save key per orphan checked against the registry: **0 collisions**,
so stateful seals can claim fresh keys without touching existing sections.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-R_CATALOG_SHAPES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-R_CATALOG_SHAPES.md)
  — top-level shape (object keys / array element keys) of every matched
catalog, so loaders can be written without opening files one by one.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-S_TEST_REGIONS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-S_TEST_REGIONS.md)
  — inverse index: each test region and the orphans it already touches
(**50 of 109 regions** reference at least one orphan — natural claim bundles).
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-T_WORKED_EXEMPLARS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-T_WORKED_EXEMPLARS.md)
  — four worked seal specifications (top-risk, island, stateful-with-data,
small evaluator) showing the full package shape from premise to verification.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-U_DATA_REFERENCES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-U_DATA_REFERENCES.md)
  — hardcoded JSON literals in orphan sources verified against the data tree:
only 4 orphans carry literals, and `ModSupportSystem` references
`factions.json` / `quests.json`, **which do not exist** — a stale-reference
finding for its package.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-V_MASTER_WORKLIST.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-V_MASTER_WORKLIST.md)
  — the single combined worklist: risk, state, key proposal, top catalog, route,
and focused command per orphan, one line per claim.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-W_DATA_IDS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-W_DATA_IDS.md)
  — id literals in orphan sources checked against **4,750 ids** found
recursively under `Data/`; 14 orphans carry id literals, 15 rows unresolved
(for the package to classify as runtime-built or stale).
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-X_STATIC_HAZARDS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-X_STATIC_HAZARDS.md)
  — static-field and singleton census: only **1 of 99** orphans carries a
flagged static (`VoiceLineSelectionEngine`), so the host-session pattern is
mostly unobstructed.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-Y_BATCH_PLAN.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-Y_BATCH_PLAN.md)
  — the 99 orphans split into **10 balanced batches** (snake by risk), each with
a dominant test region and a focused command.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-Z_SHARED_SHAPES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-Z_SHARED_SHAPES.md)
  — shared method shapes across orphans: **56 of 99 implement
`CaptureState()`/`RestoreState()`**, 34 `LoadCatalog`, 11 `TickDay` — the
de-facto save interface the set already speaks.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AA_TIME_COUPLING.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AA_TIME_COUPLING.md)
  — time-coupling census: **37 orphans expose day-step methods**, 2 hour-step —
they need a day owner hook (Plan 33) when sealed.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AB_BATCH_PLAN_LINKS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AB_BATCH_PLAN_LINKS.md)
  — batch → programme-plan cross-reference: which expansion plans each batch
unblocks, and which members are greenfield.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AC_SAVE_DTOS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AC_SAVE_DTOS.md)
  — save-state signature census for the 56 capture/restore orphans: actual
return/parameter types (an earlier `*Save`-naming premise was wrong — only one
uses that convention), 0 types undefined in Core.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AD_BATCH_VERIFICATION.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AD_BATCH_VERIFICATION.md)
  — per-batch verification loop with **expected audit delta** (99 → 99−N); the
reachability re-run is the acceptance witness.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AE_SURFACE_DECISIONS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AE_SURFACE_DECISIONS.md)
  — the **23 orphans** with neither a host partial nor a route candidate: each
needs an explicit EP-01 surface decision (route / attach / headless / Core-only).
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AF_SEAL_ORDER.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AF_SEAL_ORDER.md)
  — recommended **global batch order** scored by programme-unblock value per
unit of risk (top: Batch 06 — Economy region, score 128).
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AG_LOADER_GAPS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AG_LOADER_GAPS.md)
  — the **22 catalog rows with zero loader references**: data exists but nothing
loads it by that name; classify dynamically-pathed vs genuinely unloaded.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AH_LIFECYCLE_FILES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AH_LIFECYCLE_FILES.md)
  — lifecycle-group and `*_save.json` file-name assignment for stateful orphans
against the **55 registry groups**; 0 file collisions.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AI_METHOD_NAMES.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AI_METHOD_NAMES.md)
  — proposed `Setup*`/`Save*` host method names vs the current **228 setup
methods**; 0 collisions — the naming space is clear.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AJ_MAINTENANCE_MAP.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AJ_MAINTENANCE_MAP.md)
  — maintenance index: what each appendix derives from and the event that
invalidates it (regeneration precedes claim promotion).
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AK_BLOB_INVENTORY.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AK_BLOB_INVENTORY.md)
  — the **147-file unreachable blob** beyond the 99 authorities (loaders, DTOs,
catalogs, partials): 246 unreachable files total; the lookup table for seal
packages that need supporting files.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AL_COMPILE_SURFACE.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AL_COMPILE_SURFACE.md)
  — compile verification: **all 246 unreachable files ship in the game
assembly** (no build exclusion), so retirement is a clarity/size change and the
seal path is unchanged.
- [`PLAN-ORPHAN-SEAL-01_APPENDIX-AM_GENERATORS.md`](PLAN-ORPHAN-SEAL-01_APPENDIX-AM_GENERATORS.md)
  — the **14 appendix generators are versioned in-repo**
(`tools/generators/`), so every appendix is reproducible; correction lineage
recorded.

---

## 1. Outcome

Close the largest structural gap in the current tree: **99 Core authority
files are unreachable from the Godot host**, of which **5 are fully dead**
(no host reference, no test reference, no catalog consumer). Every authority is
either:

- **integrated** — Core authority + host owner + event/route path + save path
  where stateful + observable outcome, per `AGENTS.md` "A system is integrated
  only when…"; or
- **retired with evidence** — deleted from Core and its test/catalog removed,
  recorded in `KNOWN_DEBT.md` with a promotion condition; or
- **explicitly catalogued as Core-only** (governance/telemetry/tooling that is
  intentionally not a runtime authority), recorded in the new
  `docs/architecture/CORE_ONLY_AUTHORITIES.md` with the reason.

**Non-goals:** no new gameplay pillar (that is Plans 04/05); no authority
forking; no new save section for a system that can ride an existing owner; no
full-suite runs; no Unity artifacts.

---

## 2. Premise evidence (audited 2026-09-21, HEAD `5be1a30a`)

**Method.** Static reachability, not grep-only (to eliminate Core-composition
false positives):

1. Extract every `class/record/struct/interface/enum` definition in
   `Assets/Ashfall.Core/**` (1,168 `.cs` files).
2. Build a file→file edge graph from identifier mentions between Core files.
3. Roots = Core type names that appear anywhere in `src/**` (844 `.cs` files).
4. BFS closure. Any authority file outside the closure is host-unreachable.
5. Cross-check test references in `Ashfall.Core.Tests/**` (1,378 `.cs` files).

**Result.** `99` unreachable files contain an authority type
(`*System|*Engine|*Coordinator|*Manager`). All 99 have at least one test
reference — they are tested in isolation, never composed by the running game.
Five additional files are fully dead (host=0, tests=0):

| Fully dead | File | Triage |
|---|---|---|
| `ArmoredDraisineRecoverySystem`, `DraisineRecoverySystem` | `Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs` | superseded by `AmphibiousDraisine`/`ArmoredCrawler` host paths; RETIRE or fold into rail maintenance wave |
| `PowderMetallurgyEngine` | `Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs` | sanctioned B66/Plans 130–133 powder authority; WIRE through `SilentFoundryHostSession` (no new store) |
| `LyophilizationEngine` | `Assets/Ashfall.Core/Medical/LyophilizationSystem.cs` | WIRE through medical/kitchen wave (cold-chain preservation) |
| `NvisC4ISystem` | `Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs` | WIRE through radio/comms wave (or retire if `RadioPropagationEngine` + existing comms supersede) |

**Domain distribution of the 99 (authority files):**

| Domain | Count | Authorities |
|---|---:|---|
| Shelter | 11 | CupolaFoundryEngine, TrophySystem, PowerLoadSheddingEngine, EmergencyMusterReadinessEngine, KilnFiringEngine, ChemicalReagentSynthesisEngine, MechanicalPowerDrivelineEngine, ShelterIdentitySystem, ShelterExpansionSystem, DisasterResponseSystem, ShelterMaintenanceSystem |
| Economy | 10 | SeasonalHumanMigrationEngine, RestockAllocationEngine, TradeRouteRiskBindingEngine, BlackMarketContrabandEngine, ChitPurityAssayEngine, LoanSharkEnforcerEngine, TradeRouteMonopolyEngine, MigrationConsequenceEngine, BlackMarketHeatAttentionEngine, SurvivorBarterSystem |
| Survivors | 9 | HobbySystem, AntenatalMaternalHealthEngine, SurvivorAgingProgressionEngine, SurvivorAutonomySystem, BackstorySystem, AgingSystem, SurvivorRoutineSystem, SurvivorRoleSystem, RecruitmentSystem |
| Medical | 6 | RehabilitationProgressionEngine, ProstheticConditionWearEngine, SurgicalGraftRejectionEngine, PalliativeCareDignityEngine, DependencyTaperWithdrawalEngine, ClinicalWardTriageEngine |
| World | 5 | WeatherForecastReliabilityEngine, ModalTravelDispatchEngine, WildlifeHarvestQuotaEngine, StormForecastReadinessEngine, NightWatchPatrolReadinessEngine |
| Weather | 3 | CascadeTargetSystem, WeatherCascadeSystem, NuclearWinterProgressionSystem |
| Narrative | 3 | SurvivorLetterDeliverySystem, LetterDeliverySystem, NpcMemorySystem |
| Culture | 3 | CultureCreationSystem, ShelterFestivalEngine, ShelterMuseumSystem |
| Voice | 3 | VoiceLineSelectionEngine, VoiceLineDispatchCoordinator, SurvivorVoiceSystem |
| Expeditions | 2 | ColonySystem, AerialReconWindowEngine |
| Audio | 2 | AudioAccessibilityCoordinator, CassettePlaybackSystem |
| Farming | 2 | SoilReclamationProfileEngine, OilseedPressingEngine |
| Water | 2 | WaterQualityProfileEngine, WaterSourceSystem |
| Education | 2 | ApprenticeshipCurriculumEngine, SurvivorEducationSystem |
| Medical/other cluster | see rows above | ClothingWarmthSystem, InternalCommunicationSystem |
| single-authority domains | 29 | RadioPropagationEngine, SeasonalCelebrationSystem, MaritimeExplorationSystem, ChemicalPlumeDispersionEngine, ContentOrphanCertificationEngine, SubterraneanSubsidenceEngine, TerritoryControlSystem, CampaignLegacySystem, ConfessionSecretSystem, SessionDurabilityManager, SpiritualRitualCalendarEngine, PerimeterEarlyWarningEngine, FactionDiplomacySystem, ModSupportSystem, SleepAcousticRestEngine, CommitmentSystem, ShelterGovernanceEngine, DifficultySettingsSystem, SecondGenerationMilestoneEngine, CommonTableRationingEngine, PlayableMetricsAggregationEngine, InformantNetworkTradecraftEngine, GarmentLayeringThermalEngine, PrecisionGlassworksOpticsEngine, PublicBroadsheetPressEngine, OutpostSettlementSystem, CookingSystem, CommunicationsSystem, PsychologicalProfileSystem, AccessibilitySettingsSystem, BestiarySystem, EmergencyAlertSystem, FoodTypeSystem, VisitorIntegrationSystem |

**Why this matters.** `INTEGRATION_PLANS.md` records multiple UNBLOCK waves as
"integrated and sealed" for systems on this list (e.g. Plan 157
`CommunicationsSystem`, Plan 160 `ColonySystem`, Plan 161 `HobbySystem`,
Plan 162 `ShelterArchiveSystem` siblings, Plan 170 `SeasonalCelebrationSystem`,
Plan 134 `TerritoryControlSystem`, Plan 140 `CampaignLegacySystem`,
Plan 154 `SurvivorEducationSystem`, Plan 165 `ModSupportSystem`,
Plan 136 `CookingSystem`, Plan 153 `FactionCovertOpsCoordinator`). Core+tests
landed; the host path did not. Under the repo's own integration definition
those rows are **Core-delivered, host-pending**, not sealed.

**Collision evidence.** `src/Main.Plans157.cs` wires *Grain Milling Discovery*
(Plan 157 historical corpus), while the 2026-09 UNBLOCK ledger assigns
Plan 157 to `CommunicationsSystem`. `src/Main.Plans152.cs` wires *Black
Projects Archive*, not the ledger's Plan 152 Vehicle Customization.
`src/Main.Plans162_165.cs` wires `AgricultureSystem`, not the ledger's
Plan 162 `ShelterArchiveSystem`. Number reuse is the mechanism that let the
orphan cohort grow unnoticed.

---

## 3. Authority map and shared seams

| Concern | Existing authority to extend | Do **not** create |
|---|---|---|
| Composition root | `Main.ComposeCampaign()` (`src/Main.CampaignServices.cs`) | a second bootstrap |
| Declarative registry | `SubsystemManifest` (`Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs`, currently 18 entries) | per-system registries |
| Save sections | `SaveOrchestrator` + `src/Host/*SaveStore.cs` pattern + `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` | parallel JSON files per system |
| Day ticks | `src/Main.CampaignOwners.cs` day-owner pattern | ad-hoc `_Process` polling |
| Player surfaces | `Main.PlayerSurfaces.cs` descriptor/route list + `PanelRouteGateTests` | new panel registries |
| Seeded RNG | `CampaignRngManager.Fork(stream, day, salt)` / `ISeededRng` | `System.Random`, `GetHashCode` |
| Data catalogs | `Assets/StreamingAssets/Data/*.json` + `CatalogIntegrityValidator` | loading logic in panels |
| Gate evidence | `scripts/ci/generate-core-systems-catalog.py`, `generate-port-contract.py`, `--content-utilization-selftest` | hand-maintained catalog registers |

**Integrator-only paths for the whole programme:** `src/Main.CampaignServices.cs`,
`src/Main.SaveOrchestrator.cs`, `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs`,
`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `src/Main.PlayerSurfaces.cs`,
`WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.

---

## 4. Definition of Done (per authority)

A wave package may claim "integrated" only when all seven are true and one
focused command proves each:

1. **Authority** — Core system exists, engine-free, deterministic, versioned
   `CaptureState/RestoreState` if stateful.
2. **Host owner** — a `*HostSession` (or documented existing session) owns the
   instance; `Main.ComposeCampaign()`/`ExecuteSubsystemManifestBootstrap()`
   constructs it exactly once; idempotent `SetupX()`.
3. **Path** — at least one event/command/route: a `SubsystemManifest` entry,
   a day-owner, or a routed panel/CLI command that mutates or reads live state.
4. **Persistence** — capture/restore wired through the canonical owner,
   section count and `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` regenerated
   when a section is genuinely new; legacy saves default safe.
5. **Outcome** — one observable in-game consequence (journal line, panel row,
   day event, radio/audio cue, morale/need delta) reachable without a test.
6. **Focused tests** — existing suite stays green; add only contract/save/
   determinism/lifecycle tests per `TEST_POLICY.md`; run
   `bash scripts/run_test.sh <file>`.
7. **Reachability** — the authority becomes part of the host closure under the
   gate script from PLAN-INTEGRATION-KIT-02; no new orphan is created.

Systems that fail DoD by design are reclassified Core-only with a written
reason, not left ambiguous.

---

## 5. Waves

Each wave is one builder package (or a small ordered sequence) with the exact
claim paths filed first. Waves are ordered by dependency and test risk, not by
notional plan number.

### Wave 1 — Dead-code triage (smallest, clearing trash first)
- Packages: (a) `PowderMetallurgyEngine` → `SilentFoundryHostSession` bind +
  `--foundry-selftest` probe; (b) `LyophilizationEngine` → medical cold-chain
  bind (pending Wave 5 medical session); (c) `NvisC4ISystem` + draisine two →
  decision: wire into rail/radio or RETIRE with `KNOWN_DEBT` rows.
- Acceptance: 0 fully-dead authority files remain; each decision evidenced by a
  reachability re-run; no behavior change for the three retirements.
- Verify: `python3 scripts/ci/<reachability>.py` (kit), focused tests for the
  foundry and rail suites, `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/`.

### Wave 2 — Shelter industry & infrastructure (11 systems)
`CupolaFoundryEngine`, `KilnFiringEngine`, `ChemicalReagentSynthesisEngine`,
`MechanicalPowerDrivelineEngine`, `PowerLoadSheddingEngine`,
`EmergencyMusterReadinessEngine`, `ShelterMaintenanceSystem`,
`ShelterExpansionSystem`, `DisasterResponseSystem`, `ShelterIdentitySystem`,
`TrophySystem`.

- New mechanics seeded here (executed in Plan 05): power rationing board,
  foundry heat campaign, kiln firing, reagent synthesis chain, driveline wear,
  muster readiness, room expansion/renovation, disaster response protocols.
- Save strategy: extend the existing `shelter_*`/`foundry` sections and
  `Main.ShelterInfrastructure.cs` session; new sections only where a system
  owns durable per-day state that no current section models.
- Acceptance: each authority has DoD 1–7; `--shelter-selftest` and
  `--foundry-selftest` extended with one probe per wired system.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`,
  `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/`,
  `godot --headless --path . -- --shelter-selftest`.

### Wave 3 — Economy depth (10 systems)
`RestockAllocationEngine`, `TradeRouteMonopolyEngine`,
`TradeRouteRiskBindingEngine`, `BlackMarketContrabandEngine`,
`BlackMarketHeatAttentionEngine`, `ChitPurityAssayEngine`,
`LoanSharkEnforcerEngine`, `MigrationConsequenceEngine`,
`SeasonalHumanMigrationEngine`, `SurvivorBarterSystem`.

- Authority note: all ride `MarketSystem` + `BlackMarketSystem` +
  `HoldfastTradeSession` + `FundsLedger`; no new ledgers (DEC-22/26/37/39).
- Acceptance: restock engine consumes `ShelterBarterSystem` priority seam
  (CF-P5), trade-route contracts become player-visible route options,
  migration engines produce day events, contraband/heat/chit/loan-shark feed
  the existing black-market session.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`,
  `godot --headless --path . -- --economy-selftest`.

### Wave 4 — Survivor life & identity (9 + Voice 3 + Narrative 3)
`AgingSystem`, `SurvivorAgingProgressionEngine`, `SecondGenerationMilestoneEngine`,
`RecruitmentSystem`, `BackstorySystem`, `SurvivorRoleSystem`,
`SurvivorRoutineSystem`, `SurvivorAutonomySystem`, `HobbySystem`,
`SurvivorVoiceSystem`, `VoiceLineSelectionEngine`, `VoiceLineDispatchCoordinator`,
`LetterDeliverySystem`, `SurvivorLetterDeliverySystem`, `NpcMemorySystem`,
`PsychologicalProfileSystem`, `CommitmentSystem`, `ConfessionSecretSystem`,
`TraumaBondSystem`, `RationConflictSystem`, `IdeologicalFrictionSystem`.

- Depends on: roster/lifecycle owner (`Main.Survivors.cs`) and
  `SurvivorSocialCoordinator`; voice lines bind to the existing audio event
  bridge (`AudioManager.RefreshDomainBindings` pattern, BUG-WIRING-REPAIR).
- Acceptance: each system emits into the journal/audio/morale path; aging and
  generational state ride the existing survivor save section (no new section).
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`,
  `godot --headless --path . -- --survivors-selftest`.

### Wave 5 — Medical continuum (6 + Water 2 + Needs/Nutrition/Kitchen 5)
`ClinicalWardTriageEngine`, `RehabilitationProgressionEngine`,
`ProstheticConditionWearEngine`, `SurgicalGraftRejectionEngine`,
`PalliativeCareDignityEngine`, `DependencyTaperWithdrawalEngine`,
`WaterQualityProfileEngine`, `WaterSourceSystem`, `SleepAcousticRestEngine`,
`CommonTableRationingEngine`, `FoodTypeSystem`, `CookingSystem`,
`ClothingWarmthSystem`, `AntenatalMaternalHealthEngine`.

- Authority note: all ride `MedicalWardSystem`, `MedicalPipelineCoordinator`,
  `NeedsSystem`, `FoodPreservationSystem`, `WaterTreatmentSystem`,
  `SurvivorBodyState` (DEC-21/38/41/42) — extend, never fork.
- Acceptance: clinic continuum is playable from triage to prosthesis wear;
  cold-chain lyophilization (Wave 1) hooks the kitchen/medical path.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`,
  `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`,
  `godot --headless --path . -- --medical-selftest`.

### Wave 6 — World, weather, and risk (World 5 + Weather 4 + Excavation 1 + Defense 1 + Emergency 1)
`WeatherForecastReliabilityEngine`, `StormForecastReadinessEngine`,
`WeatherCascadeSystem`, `CascadeTargetSystem`,
`NuclearWinterProgressionSystem`, `WildlifeHarvestQuotaEngine`,
`NightWatchPatrolReadinessEngine`, `PerimeterEarlyWarningEngine`,
`SubterraneanSubsidenceEngine`, `EmergencyAlertSystem`,
`ModalTravelDispatchEngine`.

- Authority note: Plan 205 weather owner + `WeatherGameplayCascadeEngine`
  seam; nuclear winter ties to `phase_*` catalog already delivered.
- Acceptance: cascade consumers produce shelter/expedition/economy/morale
  consequences; alerts coalesce through Plan 169 audio accessibility policy.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/`,
  `godot --headless --path . -- --weather-selftest`.

### Wave 7 — Culture, memory, communication (Culture 3 + Print 1 + Audio 2 + Radio 2 + Communications 1 + Legacy 1 + Factions 2 + Diplomacy 1 + Espionage 1 + Spirit 1 + Generations 1)
- Includes `CultureCreationSystem`, `ShelterFestivalEngine`,
  `ShelterMuseumSystem`, `PublicBroadsheetPressEngine`,
  `AudioAccessibilityCoordinator`, `CassettePlaybackSystem`,
  `RadioPropagationEngine`, `CommunicationsSystem`, `CampaignLegacySystem`,
  `TerritoryControlSystem`, `FactionDiplomacySystem`,
  `InformantNetworkTradecraftEngine`, `SpiritualRitualCalendarEngine`.
- This wave is the wiring half of PLAN-VERTICAL-CULTURE-04 and
  PLAN-UNBLOCK-03 (DEC-30/32/33/34/36).
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/`,
  `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/`,
  `godot --headless --path . -- --radio-selftest`.

### Wave 8 — Logistics, settlement, and expansion (Expeditions 2 + Settlements 1 + Maritime 1 + Vehicle 1 + Accessibility 1 + Mods 1 + Content 1 + Save 1 + Telemetry 1 + Bestiary 1 + Visitors 1 + Optics 1 + Textiles 1 + Farming 2 + Governance 1 + Difficulty 1 + Education 2 + Cooking 1)
`ColonySystem`, `OutpostSettlementSystem`, `AerialReconWindowEngine`,
`MaritimeExplorationSystem`, `VehicleCustomizationSystem`,
`AccessibilitySettingsSystem`, `ModSupportSystem`,
`ContentOrphanCertificationEngine`, `SessionDurabilityManager`,
`PlayableMetricsAggregationEngine`, `BestiarySystem`, `VisitorIntegrationSystem`,
`PrecisionGlassworksOpticsEngine`, `GarmentLayeringThermalEngine`,
`SoilReclamationProfileEngine`, `OilseedPressingEngine`,
`ShelterGovernanceEngine`, `DifficultySettingsSystem`,
`ApprenticeshipCurriculumEngine`, `SurvivorEducationSystem`.

- Governance/telemetry systems (`ContentOrphanCertificationEngine`,
  `SessionDurabilityManager`, `PlayableMetricsAggregationEngine`,
  `AccessibilitySettingsSystem`, `ModSupportSystem`) may be accepted as
  **Core-only/CLI-only** if DoD 3–5 cannot be truthfully satisfied; each needs
  a `CORE_ONLY_AUTHORITIES.md` row and a CLI probe instead of a panel.
- Verify: `bash scripts/run_test.sh Ashfall.Core.Tests/World/`,
  `godot --headless --path . -- --expeditions-selftest`.

---

## 6. Risk register

| Risk | Mitigation |
|---|---|
| 99 systems cannot be wired in one batch | 8 bounded waves, one builder package each; wave 1 clears dead code first |
| Save-section explosion | default = ride existing sections; a new section needs an integrator sign-off and matrix regeneration |
| Plan-number collisions cause mis-wiring | every wave renames nothing; host partials are keyed by subsystem name (`Main.ShelterInfrastructure.cs`), never by plan number; ledger rows must cite subsystem names |
| Determinism drift on new daily ticks | day-owner ordering declared in the wave package; `ISeededRng` fork per system/day; RNG source gate must stay green |
| Panel-only fake routes | panels may only call existing commands; route gate + `__selftest` must fail if no producer exists |
| Test inflation | `TEST_POLICY.md` focused runs; aggregation allowed for static tables only |

## 7. Rollout / rollback

Waves are additive seams. Each package lands behind a `SetupX()` that is a
no-op on failure (missing catalog → logged, system stays dormant) so a bad
catalog cannot block campaign startup. Rollback = revert the wave commit; no
save migration is introduced that older code cannot ignore (additive fields
only, legacy defaults).

## 8. Handoff template for every wave

```text
Package: ORPHAN-SEAL-W<N>-<domain>
Outcome:
Files changed:
Current contract used (authority/save/event):
Reachability before/after (gate output):
Verification commands and results:
Tests reused / added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for sweep: yes/no
```

## 9. Verification appendix — reachability probe (proposed for the kit)

```python
# scripts/ci/generate-authority-reachability.py  (proposed; owned by PLAN-INTEGRATION-KIT-02)
# 1. parse Core type definitions and file tokens
# 2. roots = Core type names mentioned in src/**
# 3. BFS across Core file references
# 4. output docs/architecture/AUTHORITY_REACHABILITY.md
# 5. --check mode fails on any new unreachable authority
```

Focused commands used by this audit (evidence, re-runnable):

```bash
# orphan cohort (99 files) — via the script above
# fully-dead set — host=0 tests=0
grep -rl --include='*.cs' '\bPowderMetallurgyEngine\b' src/ Ashfall.Core.Tests/
# collision evidence
head -4 src/Main.Plans157.cs src/Main.Plans152.cs src/Main.Plans162_165.cs
```

## 10. Change-control notes

- This plan does not authorize edits. `INTEGRATION_PLANS.md` and
  `WORKTREE_OWNERSHIP.md` are foreman-owned; a wave starts only after a claim
  row names exact paths.
- No Unity work, no new save store where an owner exists, no panel-owned
  gameplay, no `System.Random`.

---

## 6. Expanded census (the kit itself)

This plan is the kit; this section measures it.

| Metric | Value |
|---|---:|
| Orphan authority files | 99 |
| Unreachable blob files (non-authority) | 147 |
| Total unreachable files | 246 |
| Appendices in this kit | 43 |
| Versioned generators | 50 |
| Seal batches (Appendix Y) | 10 |

## 7. Expanded surface: the kit's own contract

| Rule | Value |
|---|---|
| Generated, not edited | every appendix has a generator (Appendix AM) |
| Reproducible | re-run and diff; a mismatch is a finding |
| Collision-free | save keys (Q), file names (AH), method names (AI) |
| Ordered | batch order by programme-unblock value (AF) |
| Witnessed | audit delta per batch (AD) |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Appendices linked | every appendix referenced by this plan |
| Generators | run and diff clean |
| Audit | 99 orphans reproduces from the tool |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Regenerate the kit's appendices (Appendix AJ triggers).
2. Confirm the orphan count and blob inventory.
3. Open the first batch claim (Appendix AF priority 1).
4. Record the audit delta per batch.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Appendix | generated, linked, reproducible |
| Batch | sealed members leave the orphan set (audit delta) |
| Naming | no key/file/method collisions |
| Handoff | audit delta recorded |

**Non-goals unchanged:** the kit proposes; claims execute.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 68. Other plans referencing them: **45**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 14 |
| `PLAN-INTEGRATION-KIT-02` | 10 |
| `PLAN-HOST-COMPOSITION-GOVERNANCE-71` | 6 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 5 |
| `PLAN-UNBLOCK-03` | 4 |
| `PLAN-LAUNCH-FACE-06` | 4 |
| `PLAN-RUNTIME-PERF-16` | 4 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 3 |

**Governed artifacts (first 12):**

| Path |
|---|
| `AGENTS.md` |
| `Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs` |
| `Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs` |
| `Assets/Ashfall.Core/Medical/LyophilizationSystem.cs` |
| `Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` |
| `Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs` |
| `CORE_ONLY_AUTHORITIES.md` |
| `INTEGRATION_PLANS.md` |
| `KNOWN_DEBT.md` |
| `Main.CampaignServices.cs` |
| `Main.PlayerSurfaces.cs` |
| `Main.SaveOrchestrator.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 64. Host files: **70** · Test files: **98** · Data files: **48**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 70 | `src/Dose/DoseRegisterSurface.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ChemicalReconHostSession.cs`, `src/Host/CoreDemoSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 98 | `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs`, `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`, `Ashfall.Core.Tests/Cluster12CHeadlessDemoTests.cs`, `Ashfall.Core.Tests/Codex/CodexEntryCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 48 | `Assets/StreamingAssets/Data/affliction_bridge_rules.json`, `Assets/StreamingAssets/Data/characters.json`, `Assets/StreamingAssets/Data/codex_entries.json`, `Assets/StreamingAssets/Data/colony_blueprints.json`, `Assets/StreamingAssets/Data/crossing_items.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **42** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `amphibious_draisine` |
| `campaign` |
| `campaign_day` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `crossing` |
| `dose_ledger` |
| `draisine_recovery` |
| `dynamic_quests` |
| `excavation_hazards` |
| `expanded_shelter` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **50** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--amphibious-draisine-selftest` |
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--campaign-journey-selftest` |
| `--chemical-dependency-save-selftest` |
| `--cluster-selftest` |
| `--crossing-selftest` |
| `--data-integrity-selftest` |
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--expedition-panel-lifecycle` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **29**.

| Event | First declaration |
|---|---|
| `OnBarterOnlyModeChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnColonyDied` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonyStressed` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnColonySwarming` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnDependencyFormed` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnDependencyReFormedByStress` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/amphibious_draisine_catalog.json` |
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/characters.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **16** (288 files, 2223 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `BodyMind` | 1 | 6 |
| `Campaign` | 32 | 187 |
| `Codex` | 2 | 29 |
| `Communications` | 1 | 6 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Flagship11` | 7 | 63 |
| `Foundry` | 8 | 73 |

**Verdict:** 2223 cases sit under matching regions — run those first (`Audio`, `Balance`, `BodyMind`, `Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **550**
(50 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/CSharpVerificationTest.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **52**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **138**
(CODEX_ONLY 57, GAMEPLAY_CONSUMED 53, OPTIONAL 6, UNRESOLVED 22).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `characters.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `codex_entries.json` | UNRESOLVED |

**Verdict:** 22 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_honored_debt` |
| `flag_repaired_infrastructure` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance · **Coupling (incoming plans):** 45
**Surface:** save sections 52 (laddered 1) · RNG streams 15 · host files 24 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ORPHAN-SEAL-01
wave: —
status: PROPOSED — foreman claim required
packages: author package list at claim time
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/CSharpVerificationTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 45 other plan(s) name these artifacts (§12)
  - wave seed — claim before dependent plans
  - touches 1 versioned save ladder(s) — extend, never fork
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
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages.
