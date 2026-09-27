# FOUR-TRACK EXPANSION BATCH 4 — Common Table Nutrition / Emergency Muster / Soil Reclamation / Campaign Action Log

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**STATUS: APPROVED BY USER**
**Authorized by:** user directive ("Please find 4 plans to fully integrate, don't leave as
partials, don't commit an don't overly test!") — 2026-09-26.
**Claim:** `claim-four-track-expansion-batch4-2026-09-26`

## Bounded outcome

Convert four committed Core host-orphans into fully wired host features using the
established session / save / day-owner / probe pattern. No new architecture: each
Core system remains the sole authority for its concern; the host owns only the
record roster and supplies the facts the engine consumes.

| Track | Core authority (Expansion) | Save section | Day owner | Probe |
|---|---|---|---|---|
| Common-table nutrition | `CommonTableRationingEngine` (Exp. 26) | `common_table_rationing` | phase 5 | 8/8 |
| Emergency muster readiness | `EmergencyMusterReadinessEngine` (Exp. 23 The Alarm) | `emergency_muster_readiness` | phase 5 | 8/8 |
| Soil reclamation profile | `SoilReclamationProfileEngine` (Exp. 15 The Deep Root) | `soil_reclamation_profile` | phase 5 | 7/7 |
| Campaign action log | `CampaignActionLog` (PlayerCommand) | `campaign_action_log` | phase 5 | 6/6 |

## Selection method — a real reachability audit, not a name grep

Previous batches selected candidates by grepping class names, which produced false
positives and duplicate-authority risk. For this batch I built
`scripts/ci/find-orphan-core-candidates.py`, which performs a **file-level transitive
reachability closure** over the type-reference graph:

1. A Core file is reachable when a reachable file mentions **any** type it declares.
2. Every type mentioned by a reachable file is itself reachable.
3. Repeat to fixpoint from `src/`.

A candidate is accepted only when **every type in its declaring file** is unreachable.
Enumerating all types (not just the class name) matters: a file where some types are
wired and others are not is a duplicate-authority trap, not an integration seam.

Fixing the closure direction removed **57 false positives** (113 unreachable types at
first pass → 56 real ones), and corrected a genuine miss where an engine-agnostic
state record consumed by a reachable partial class had been mis-flagged as orphaned.

### Candidates rejected with current evidence

| Rejected | Reason (Rule 5 / Rule 7 / Rule 10) |
|---|---|
| `ProstheticConditionWearEngine` | **Decision-blocked.** Its own header tags it `F14-D / UNBLOCK-01`; `docs/plans/unblockers/UNBLOCK-01_BODY-INTEGRITY_SCHEMA_F14_XP06.md` is a signature bundle the foreman has **not signed**, and `AGENTS.md` lists XP-06 body-integrity (F14) as "never start without the named signature". Wiring it would touch F14-D's blast radius (`items.json`, crafting catalogs, `EquipmentConditionSystem` consumers). Reported as a blocker instead. |
| `AcousticDirectionFindingCatalog` | **Parallel authority.** `acoustic_triangulation_catalog.json` has six consumer-free types, but `sound_ranging` (section + `SoundRangingHostSession` + `SoundRangingThreatEngine` + `sound_ranging_catalog.json`) already owns acoustic early-warning array profiles. A second loader would be a second acoustic authority. |
| `OilseedPressingEngine` | **Parallel authority.** Its `ConfitPreservationResult` leg is oil/salt curing; the `food_preservation` section already owns "Food spoilage, curing, and cryogenic preservation". Rejecting the whole engine rather than leaving a half-wired island. |
| `ChemicalPlumeDispersionEngine` | **Parallel authority.** `chem_warfare` already owns "CBRN hazard warfare and toxic contamination" (`ChemicalPlumeState`, `PlumeToxicityTier`). |
| `VerticalAscentCatalog` | **No authored data.** No `vertical_ascent` JSON exists in `Assets/StreamingAssets/Data/`; wiring it would require inventing rig ids and fuel item ids that resolve nowhere. |
| `KnowledgeAcquisitionSource` | **Parallel authority.** `ResearchSystem` + the `research` section already own research knowledge unlocking. |
| `CaravanAtomicTrader` | **Parallel authority.** Its own doc says it "wraps an existing trade session"; `caravan`, `caravan_trade_network`, and `trade_routes` already own caravan trade. |
| `SpiritualRitualCalendarEngine` | **Parallel authority.** The `spiritual_meaning` section already owns ritual cooldowns and memorial rites (`MemorialRiteDefinition`, `SpiritualRitualDefinition`). |
| `RepositoryClassificationPolicy`, `StringFreezePolicy`, `LedgerTruthIntegrityGate`, `BootstrapLifecycleGate` | Not player-facing gameplay seams; `StringFreezePolicy` is additionally decision-blocked (D22 string freeze). |

## Files changed

- **Core:** `SaveSectionRegistry` (+4 sections + filenames); `HostCliRegistry`
  (+4 actions + descriptors); `DayEventVocabulary` (+4 heartbeats).
- **Host (new):** `CommonTableRationingHostSession.cs`,
  `EmergencyMusterReadinessHostSession.cs`,
  `SoilReclamationProfileHostSession.cs`,
  `CampaignActionLogHostSession.cs` (each with its checksummed save store);
  `HostCli.CommonTableRationing.cs`, `HostCli.EmergencyMusterReadiness.cs`,
  `HostCli.SoilReclamationProfile.cs`, `HostCli.CampaignActionLog.cs`;
  `Main.CommonTableRationing.cs`, `Main.EmergencyMusterReadiness.cs`,
  `Main.SoilReclamationProfile.cs`, `Main.CampaignActionLog.cs`.
- **Host (edited):** `HostCli.cs` (enum/parse/help), `Main.Application.cs`
  (dispatch), `Main.SaveOrchestrator.cs` (setup/save), `Main.Lifecycle.cs`
  (reset), `Main.CampaignOwners.cs` (4 new phase-5 day owners).
- **Tests:** `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
  (section pin 292 → 297).
- **Generated:** `generate-architecture-map.py` (+4 nodes),
  `EVENT_SEMANTIC_PARITY_MATRIX.md` (+4 rows), `SELFTEST_MANIFEST.json`,
  `HOST_CLI_COMMAND_CATALOG.md`, `SAVE_STORE_CONTRACT_MATRIX.md`.
- **Tooling (new):** `scripts/ci/find-orphan-core-candidates.py` (reachability audit).

## Authority boundaries (Rule 5)

- `CommonTableRationingEngine` owns dietary diversity tiers, deficiency risk, ration
  policy effects, grievance, and cook-duty waste reduction. `kitchen_nutrition`
  ("Rationing recipes and caloric balance") and `grain_processing` keep recipe/calorie
  and milling authority; `agriculture` keeps plot medium and pest authority. The host
  owns only the meal-session ledger. Note: `agriculture`'s description mentions
  "dietary diversity", but no code path computes it — the audit confirmed
  `FoodCategory`/`DiversityTier`/`RationLevel` exist nowhere else.
- `EmergencyMusterReadinessEngine` owns the composite readiness score, evacuation
  timing, missing-survivor risk, cascade intervention margins, and drill compliance
  fatigue. `shelter_fire` keeps actual fire incidents/smoke/brigade response; the
  `muster` section ("The Muster military rally & conflict state") is an unrelated
  military authority. The host owns only the drill ledger.
- `SoilReclamationProfileEngine` owns amendment chemistry, fertility evaluation,
  germination viability, and mutation risk. `agriculture`, `hydroponic_biomes`,
  `aeroponics`, `aquaponics`, `fungi_cultivation`, and `bio_fermentation` keep their
  own cultivation authorities; `SoilQualityTier`/`SoilAmendment` exist nowhere else.
  The host owns only the plot ledger.
- `CampaignActionLog` owns sequence assignment, ordering, capture, and restore.
  It is recorder-only: it never decides a gameplay outcome, so no existing authority
  is displaced. The host owns only which commands are recorded. All four systems are
  deterministic — no `System.Random`; the Core log derives sequences purely from
  append order.

## Verification (focused)

- Host build: **0 errors / 0 warnings**.
- Test-project build: **0 errors**.
- Probes: **29/29** (common-table 8/8, muster 8/8, soil 7/7, action-log 6/6).
- Save pin: **1784/1784** (297 sections).
- `DayEventParitySourceGateTests` 2/2; `HostCliHelpContractTests` 2/2.
- Regenerated artifacts in sync: manifest 241, CLI catalog 305, save-store matrix 299.
- `generate-architecture-map.py` validates all four new nodes clean.

## NOT integrated / deferred (concurrent — do not touch)

- `generate-architecture-map.py --check` still reports concurrent sections
  `chronic_condition`, `item_lore`, and `letter_delivery` missing from
  `ARCHITECTURE_GRAPH`. Those belong to active concurrent packages.
- `HostCliActionParityGateTests.UnmanifestedHostSelfTests_MatchDocumentedBaseline`
  reports `YoaIceRoadSelfTest` as dispatched but absent from
  `docs/ci/SELFTEST_MANIFEST.json` (Plan 146, concurrent agent's file). This batch's
  four actions are correctly cataloged and verified present.
- No commit, per the user directive.
