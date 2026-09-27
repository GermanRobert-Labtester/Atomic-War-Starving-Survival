# FOUR-TRACK EXPANSION-BATCH 3 — Palliative Care / Water Quality / Forecast Reliability / Curriculum

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**STATUS: APPROVED BY USER**
**Authorized by:** user directive ("Please find 4 plans to fully integrate, don't leave as
partials, don't commit and don't overly test!") — 2026-09-26.
**Claim:** `claim-four-track-batch3-2026-09-26`

## Bounded outcome

Convert four committed Core host-orphans into fully wired host features using the
established session / save / day-owner / probe pattern. No new architecture: each
Core system remains the sole authority for its concern; the host owns only the
record roster and supplies the facts the engine consumes.

| Track | Core engine (Expansion) | Save section | Day owner | Probe |
|---|---|---|---|---|
| Palliative care | `PalliativeCareDignityEngine` (24 — The Long Goodbye) | `palliative_care` | phase 5 | 7/7 |
| Water quality profile | `WaterQualityProfileEngine` | `water_quality_profile` | phase 5 | 7/7 |
| Forecast reliability | `WeatherForecastReliabilityEngine` | `weather_forecast_reliability` | phase 5 | 6/6 |
| Apprenticeship curriculum | `ApprenticeshipCurriculumEngine` | `apprenticeship_curriculum` | phase 5 | 6/6 |

## Premise audit (current evidence)

Each candidate was checked for **reachability**, not just direct references:

- **No type from the engine's file is referenced anywhere in `src/`** (stronger than a
  class-name grep: it also catches DTO/enum siblings).
- **No Core consumer of those types is itself reachable from `src/`** (a transitive
  reachability pass over every Core file that mentions the types).
- **No existing save section, host session, or catalog loader owns the concern.**

Candidates rejected during the audit with the reason (no partial integration, no
parallel authority created):

| Rejected | Reason (Rule 5) |
|---|---|
| `TraumaBondSystem`, `RationConflictSystem` | Owned and persisted by `SurvivorSocialCoordinator`, already composed by `Main.SurvivorSocial` — wired. |
| `CloudSeedingSystem` | Composed by `WeatherIntelligenceCoordinator`, already composed by `WorldHostSession` — wired. |
| `SurvivorLetterDeliverySystem` | Parallel authority: `LetterDeliverySystem` + `LetterDeliveryHostSession` already own the `letter_delivery` section. |
| `SecondGenerationMilestoneEngine` | Parallel authority: `ChildDevelopmentSystem` already owns milestones (`OnMilestoneAchieved`, `MilestoneHistory`). |
| `SpiritualRitualCalendarEngine` | Parallel authority: the `spiritual_meaning` section already owns "ritual cooldowns, memorial rites". |
| `ChemicalPlumeDispersionEngine` | Parallel authority: the `chem_warfare` section already owns "CBRN hazard warfare and toxic contamination". |
| `RailwayInterlockEngine` | Third rail authority on top of `railway` and `rail_track_maintenance`. |
| `MaritimeExplorationSystem`, `VerdictAccusationSystem` | Already integrated by concurrent-host `MaritimeHostSession` / `VerdictHostSession`. |
| `BallisticsSystem` | Reachable through the wired `TacticalCombatSystem`. |
| `LyophilizationEngine` | Directly referenced from `src/` — already wired. |

## Files changed

- **Core:** `SaveSectionRegistry` (+4 sections + filenames); `HostCliRegistry`
  (+4 actions + descriptors); `DayEventVocabulary` (+4 heartbeats).
- **Host (new):** `PalliativeCareHostSession.cs`, `WaterQualityProfileHostSession.cs`,
  `WeatherForecastReliabilityHostSession.cs`,
  `ApprenticeshipCurriculumHostSession.cs` (each with its checksummed save store);
  `HostCli.PalliativeCare.cs`, `HostCli.WaterQualityProfile.cs`,
  `HostCli.WeatherForecastReliability.cs`,
  `HostCli.ApprenticeshipCurriculum.cs`;
  `Main.PalliativeCare.cs`, `Main.WaterQualityProfile.cs`,
  `Main.WeatherForecastReliability.cs`, `Main.ApprenticeshipCurriculum.cs`.
- **Host (edited):** `HostCli.cs` (enum/parse/help), `Main.Application.cs`
  (dispatch), `Main.SaveOrchestrator.cs` (setup/save), `Main.Lifecycle.cs`
  (reset), `Main.CampaignOwners.cs` (4 new phase-5 day owners).
- **Tests:** `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
  (section pin 283 → 292).
- **Generated:** `generate-architecture-map.py` (+4 nodes),
  `EVENT_SEMANTIC_PARITY_MATRIX.md` (+4 rows), `SELFTEST_MANIFEST.json`,
  `HOST_CLI_COMMAND_CATALOG.md`, `SAVE_STORE_CONTRACT_MATRIX.md`.

## Authority boundaries (Rule 5)

- `PalliativeCareDignityEngine` owns pain, lucidity, dignity, grief-stage progression,
  and memorial echoes. The host owns only the patient roster and supplies the
  medicine-availability and caregiver-skill facts; `Medical` keeps disease authority.
- `WaterQualityProfileEngine` owns contaminant profiles, purity-tier derivation,
  treatment yield, filter wear, and health risk. `water_treatment` keeps the filtration
  plant and `water_condenser` keeps the condensation array; the host owns only the
  per-source assay ledger.
- `WeatherForecastReliabilityEngine` owns the reliability score, confidence grade, and
  dispatch-safety verdict. `weather_cascade` keeps weather→gameplay effects and
  `radio_station` keeps signal triangulation.
- `ApprenticeshipCurriculumEngine` owns literacy progression, comprehension gain,
  fatigue onset, and certification readiness. `apprenticeship` keeps mentorship
  pairings; the host owns only the learner roster.

All four engines are deterministic — no `System.Random`, no unseeded RNG. The
curriculum session seed is derived from the day and survivor id so replays stay
stable; the palliative grief roll uses the Core `StableHash` contract with a
day-derived seed.

## Verification (focused)

- Host build: **0 errors / 0 warnings**.
- Probes: **26/26** (palliative 7/7, water 7/7, forecast 6/6, curriculum 6/6).
- Save pin: **1754/1754** (292 sections).
- `HostCliActionParityGateTests` 4/4, `DayEventParitySourceGateTests` 2/2,
  `HostCliHelpContractTests` 2/2.
- Regenerated artifacts in sync: manifest 233, CLI catalog 297, save-store matrix 294.

## NOT integrated / deferred

- `generate-architecture-map.py --check` still reports three concurrent sections
  missing from `ARCHITECTURE_GRAPH`: `chronic_condition`, `item_lore`,
  `letter_delivery`. Those sections belong to active concurrent packages; adding their
  nodes here would race their owners. This batch's own four nodes validate clean.
- No commit, per the user directive.
