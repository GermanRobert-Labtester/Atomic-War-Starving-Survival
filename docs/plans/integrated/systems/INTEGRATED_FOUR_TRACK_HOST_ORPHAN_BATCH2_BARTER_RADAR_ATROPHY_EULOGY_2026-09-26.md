# FOUR-TRACK HOST-ORPHAN BATCH 2 — Survivor Barter / Perimeter Radar / Skill Atrophy / Procedural Eulogy

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

**STATUS: APPROVED BY USER**
**Authorized by:** user directive ("Please find 4 plans to fully integrate, don't leave as partials, don't commit and don't overly test!") — 2026-09-26.
**Claim:** `claim-four-track-batch2-2026-09-26`

## Bounded outcome

Convert four committed Core host-orphans into fully wired host features using the
established session / save / probe pattern. No new architecture: each system
keeps its own authority; the host composes catalog + save section + commands +
diagnostic probe + day owner.

| Track | Core system | Save section | Day owner | Probe |
|---|---|---|---|---|
| Survivor barter | `SurvivorBarterSystem` (Plan 213) | `survivor_barter` | phase 5 | 7/7 |
| Perimeter radar | `PerimeterEarlyWarningEngine` | `perimeter_early_warning` | phase 5 | 7/7 |
| Skill atrophy | `SkillAtrophySystem` | `skill_atrophy` | phase 5 | 7/7 |
| Procedural eulogy | `ProceduralEulogyEngine` | `procedural_eulogy` | — | 6/6 |

## Premise audit (current evidence)

- All four had **0 references in `src/`**: `SurvivorBarterSystem`,
  `PerimeterEarlyWarningEngine`, `SkillAtrophySystem`, `ProceduralEulogyEngine`.
- Authored catalogs exist where applicable: `barter_rules.json` (3 rules).
  The radar, atrophy, and eulogy systems are self-contained pure logic with no
  catalog — catalog loaders correctly report "ready" without inventing data.
- No save section, no CLI probe, no day owner, no host session.
- `ShelterBarterSaveStore` (`shelter_barter`, `ShelterBarterSystem`) is a
  **different** authority (caravan/broker) and is untouched.

## Files changed

- **Core:** `SaveSectionRegistry` (+4 sections + filenames); `HostCliRegistry`
  (+4 actions + descriptors); `DayEventVocabulary` (+3 heartbeats).
- **Host (new):** `SurvivorBarterHostSession.cs`, `PerimeterEarlyWarningHostSession.cs`,
  `SkillAtrophyHostSession.cs` (incl. `HostedSkillActor` built on the shared
  `SimpleSkillActor`), `ProceduralEulogyHostSession.cs`; `HostCli.Barter.cs`,
  `HostCli.PerimeterEarlyWarning.cs`, `HostCli.SkillAtrophy.cs`,
  `HostCli.ProceduralEulogy.cs`; `Main.SurvivorBarter.cs`,
  `Main.PerimeterEarlyWarning.cs`, `Main.SkillAtrophy.cs`,
  `Main.ProceduralEulogy.cs`.
- **Host (edited):** `HostCli.cs` (enum/parse/help), `Main.Application.cs`
  (dispatch), `Main.SaveOrchestrator.cs` (setup/save), `Main.Lifecycle.cs`
  (reset), `Main.CampaignOwners.cs` (3 new phase-5 day owners).
- **Tests:** `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
  (section pin 278 → 283).
- **Generated:** `generate-architecture-map.py` (+4 nodes), `EVENT_SEMANTIC_PARITY_MATRIX.md`
  (+3 rows), `SELFTEST_MANIFEST.json`, `HOST_CLI_COMMAND_CATALOG.md`,
  `SAVE_STORE_CONTRACT_MATRIX.md`.

## Authority boundaries (Rule 5)

- `SurvivorBarterSystem` owns offers/trades/reputation/favors; `Inventory` keeps
  item custody.
- `PerimeterEarlyWarningEngine` owns contact classification; `WeatherSystem` and
  `PowerGridSystem` remain the storm/dust and power authorities (the host only
  supplies the fact and exposes the draw).
- `SkillAtrophySystem` owns decay decisions; `NeedsSystem` owns morale/health.
- `ProceduralEulogyEngine` owns composition; the journal owner owns the archive.

## Acceptance

1. Host build 0 errors / 0 warnings (concurrent-package warnings excluded).
2. All four probes green (27/27).
3. Save pin reconciled and green (283 sections).
4. `SELFTEST_MANIFEST` and `EVENT_SEMANTIC_PARITY_MATRIX` regenerated and in sync.
5. Architecture graph registers all four nodes.
6. No commit (user directive).

## Deferred (concurrent, not this batch)

- `generate-architecture-map.py --check` fails only on the concurrent
  `chronic_condition` save section missing from `ARCHITECTURE_GRAPH`.
- `HostCliActionParityGateTests` fails only on the concurrent
  `SurgicalGraftSelfTest` probe missing from the manifest.
Both belong to active concurrent packages.
