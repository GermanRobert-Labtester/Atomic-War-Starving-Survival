# ORPHAN-SEAL-PRIORITY-W1 — Duplicate-Authority Boundaries & Wiring Record

**Date:** 2026-09-23 · **Owner:** Integrator (user-authorized) · **Claim:**
`claim-orphan-seal-priority-w1-2026-09-23` in `WORKTREE_OWNERSHIP.md`.

This record closes the five duplicate-authority questions raised in the
2026-09-22 audit and documents the host wiring of the ten priority orphan
authorities. It is a disposition record, not a new plan: no new gameplay
authority was created.

## 1. Prisoners — `PrisonerSystem` vs `ShelterPrisonerSystem` (two save sections)

**Decision:** `PrisonerSystem` (Plan 179, Factions) is the single captive
authority (§6.13–6.14); it is panel-backed (`PrisonerPanel`), host-backed
(`DefenseHostSession`), and owns `prisoner_management`.

**Evidence:** Plan 63's `ShelterPrisonerSystem` duplicated custody,
interrogation, escape, and recruitment while the Plan 179 comment already
claimed single authority; both were live and both wrote sections.

**Action:** the Plan 63 construction/tick/capture was removed from
`Main.Plans62_65.cs`; the `shelter_prisoners` registry row and filename map
entry were removed; the class and its tests remain for history. On upgrade,
`Main.MigrateLegacyShelterPrisoners()` imports non-terminal legacy records
into `PrisonerSystem` once (only when no canonical save exists) and journals
the count. `ShelterPrisonerSaveStore` is read-only migration input (private
section const, no live section).

## 2. Settlements — `ColonySystem` vs `OutpostSettlementSystem`

**Decision:** not duplicates. `ColonySystem` = player-founded colonies
(buildings, supply lines); `OutpostSettlementSystem` = authored-garrison
outposts (`outposts.json`); the newer `SettlementCatalog` /
`TerritoryControlSystem` (Wave 42) own the world-settlement catalog and
territory. `ColonySystem` is now host-wired with section `colony`.

**Deferred with named blocker (superseded 2026-09-23 by UNBLOCK-OLDEST-BATCH5-PLANS):** `OutpostSettlementSystem` has no
`CaptureState`/`RestoreState`; sealing it required a Core state DTO and an
explicit custody decision against `SettlementCatalog` so no third settlement
ledger is created. It stayed an island until that decision was signed.

**Resolved 2026-09-23 (UNBLOCK-OLDEST-BATCH5-PLANS, signed by the foreman/user).**
The blocker is sealed and the custody decision is signed as follows:

- `OutpostSettlementSystem` is the **single authority for authored outposts and
  secondary positions**. It gained `OutpostInstanceState` / `OutpostSettlementState`
  with `CaptureState` / `RestoreState`. Definitions are **never** persisted — they
  are re-derived from the authored `outposts.json` on restore, so a definition edit
  can never be frozen into a save. A phantom outpost id, a wrong schema version,
  an over-full garrison, or a missing authored row is handled deterministically.
- `ColonySystem` (section `colony`) remains the **player-founded colony** authority.
- `WaystationSystem` (section `waystation`) remains the **holdfast S2 waystation**
  authority. It is *not* merged with outposts.
- `SettlementCatalog` / `TerritoryControlSystem` remain the **world-settlement
  catalog and territory** authorities; an outpost stores only its `graph_node_id`
  as a read-only geography reference.

The outpost state section therefore stores establishment, condition, garrison
**ids**, supply reserve, and starvation/overrun flags only. Population, food,
inventory and the settlement catalog stay with their canonical owners, so no
second population, food, or settlement ledger is created.

## 3. Celebrations — `SeasonalCelebrationSystem` vs `ShelterFestivalEngine`

**Decision:** not duplicates. `SeasonalCelebrationSystem` = calendar
(holidays, anniversaries, scales) — section `seasonal_celebration`;
`ShelterFestivalEngine` = player-scheduled festivals that consume authored
commodities — section `shelter_festival`. Both are now host-wired and tick on
the campaign day; neither duplicates the other's state.

## 4. Covert operations — `EspionageSystem` vs `FactionCovertOpsCoordinator`

**Decision:** not duplicates. `EspionageSystem` = player-deployed agent
missions (existing host session); `FactionCovertOpsCoordinator` = rival
faction operations and the suspicion ladder — now host-wired with section
`faction_covert_ops`, catalog `espionage_operations.json`.

**Defect fixed on the way:** `RestoreState` silently dropped every in-flight
operation although `CaptureState` serialized them. Fixed and pinned by
`ActiveOperations_SurviveCaptureRestore` in the Plan 153 suite.

## 5. Communications — `CommunicationsSystem` vs `NvisCommunicationsSystem` vs `CommsArraySystem`

**Decision:** three distinct layers, not duplicates.
`CommunicationsSystem` = antenna/intercept/broadcast station (now host-wired,
section `communications`); `NvisCommunicationsSystem` = regional status and
recall queue (already wired, `nvis_communications`); `CommsArraySystem` =
long-range array and satellite telemetry (already wired, `comms_array`).

## Wired authorities (ORPHAN-SEAL-PRIORITY-W1)

| Section | Core authority | Catalog | Tick | Probe checks |
|---|---|---|---|---|
| `survivor_autonomy` | `SurvivorAutonomySystem` | `autonomy_actions.json` | daily evaluation per survivor | 3 |
| `nuclear_winter_progression` | `NuclearWinterProgressionSystem` | `nuclear_winter_phases.json` | daily advance + phase journal | 3 |
| `seasonal_celebration` | `SeasonalCelebrationSystem` | `shelter_celebrations.json` | holiday reminder | 3 |
| `disaster_response` | `DisasterResponseSystem` | `disaster_templates.json` | active-disaster mitigation | 3 |
| `communications` | `CommunicationsSystem` | `communications_networks.json` | daily antenna wear | 3 |
| `colony` | `ColonySystem` | `colony_blueprints.json` | daily colony tick | 3 |
| `hobby` | `HobbySystem` | `hobby_definitions.json` | session-driven | 3 |
| `survivor_education` | `SurvivorEducationSystem` | `education_curriculum.json` | session-driven | 3 |
| `shelter_expansion` | `ShelterExpansionSystem` | `shelter_construction.json` | labor-driven | 3 |
| `confession_secret` | `ConfessionSecretSystem` | `confession_secrets.json` | discovery-driven | 4 |
| `shelter_festival` | `ShelterFestivalEngine` | — | daily festival tick | 2 |
| `faction_covert_ops` | `FactionCovertOpsCoordinator` | `espionage_operations.json` | daily rival ops | 3 |

**Verification:** `godot --headless --path . -- --orphan-seal-wave1-selftest`
(36/36), `Ashfall.Core.Tests/Integration/OrphanSealPriorityWave1Tests.cs`
(15/15), `LoaderWiringGateTests` (catalog bindings pinned, 4/4),
`MainTriadDriftGateTests` (7/7), `PersistentFilenameRegistryGateTests` (4/4).

**Surface note:** the package ships headless-first (journal facts + CLI probe)
with an empty `ui`/`routes` map, which the sanctioned pattern permits with a
recorded reason (Appendix C.5: "ship headless-only with a recorded reason").
Follow-up panels are a separate, smaller package per authority.

**Known limitation (2026-09-23 update):** the ~74 remaining orphan authorities
are outside this package.
