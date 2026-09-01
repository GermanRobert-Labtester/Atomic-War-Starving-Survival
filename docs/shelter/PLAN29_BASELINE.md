# Plan 29 — Baseline Reconnaissance Report (Phase 0)

> **Status:** COMPLETE (Phase 0). Gate satisfied: no new condition state was added; all
> existing condition authorities are documented in the provenance docs below.
> **Date:** 2026-09-01

---

## 1. Verification results

| # | Check | Result | Notes |
|---|-------|--------|-------|
| 1 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** (0 err / 0 warn) | |
| 2 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS 5653/5653** at session start | see §3 concurrency note |
| 3 | `dotnet build Ashfall.csproj` (Godot host) | **PASS** (0 err / 0 warn) after 1-line fix, see §2 | was broken at session start |
| 4 | `godot --headless -- --data-integrity-selftest` | **PASS** at session start (153 catalogs, 0 errors, 6804 ids) | see §3 concurrency note |
| 5 | `godot --headless -- --bridge-selftest` | **PASS** | |
| 6 | `--shelter-interior-selftest` | **PASS** | |
| 7 | `--shelter-operations-selftest` | **PASS** | |
| 8 | `--shelter-hazard-selftest` | **PASS** | |
| 9 | `--shelter-decor-selftest` | **PASS** | |
| 10 | `--operations-selftest` | **PASS** | power/water/ventilation ops |
| 11 | `--phase0-selftest` | **PASS** | |
| 12 | `--content-utilization-selftest` | **PASS** | |
| 13 | `--real-campaign-journey-selftest` | **PASS** | |
| 14 | `--silent-foundry-selftest` | **PASS** | |
| 15 | `--duty-roster-selftest` | **PASS** | |

## 2. Baseline repair applied (smallest possible)

`Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs` referenced `SeasonProfileDef`
(defined in `Ashfall.Core.World/WeatherSystem.cs`) without a using directive →
`CS0246` broke the **Godot host build** at HEAD. Fixed by adding
`using Ashfall.Core.World;` (one line, no behavior change). Host build now 0/0.
This was a pre-existing break unrelated to Plan 29 systems.

## 3. Concurrent-worktree condition (recorded, not Plan 29's to fix)

A second session is actively working **Plan 23 (Black Flotilla)** in this tree
(203 modified files, +21k lines, untracked test file). During this recon window it
introduced, in **non-shelter** files:

- `Ashfall.Core.Tests/Plan23DiveMechanicCoverageTests.cs` (untracked): 2 × `CS0111`
  duplicate members (`sovereign_oxygen`, `names_count`) → test project compile break
  observed mid-session. The 5653/5653 PASS in §1 is the verified pre-churn result.
- `dive_sites.json` (5 unresolved `roomId` refs + 1 unresolved `location_id`) and
  `faction_lore.json` (2 unresolved tribute ids) → data-integrity selftest now reads
  8 errors / 6808 ids. The 0-error result in §1 is the verified pre-churn result.

**Plan 29 shelter surfaces are unaffected** by either perturbation. Phase 1+ work
must re-run the full gate on a settled tree.

## 4. Shelter system census (runtime, wired in `src/Main.*.cs`)

| System | Core file | Save section | Tick |
|---|---|---|---|
| StartingLevelSystem (Day-1 Holdfast) | `StartingLevel/StartingLevelSystem.cs` | starting-level section | daily |
| PowerGridSystem | `Shelter/PowerGridSystem.cs` | `power_grid` | daily (CampaignDay) |
| WaterTreatmentSystem | `WaterTreatmentSystem.cs` | `water_treatment` | daily |
| VentilationSystem | `VentilationSystem.cs` | ventilation section | daily (post-production) |
| ShelterThermalSystem | `ShelterThermalSystem.cs` | `shelter_thermal` | daily |
| ShelterFireHazardSystem | `Shelter/ShelterFireHazardSystem.cs` | per-incident | per tick |
| MaterialShieldingSystem | `Shelter/MaterialShieldingSystem.cs` | shielding section | event-driven |
| SkyLayerArmorSystem | `Shelter/SkyLayerArmorSystem.cs` | sky-armor section | event-driven |
| ShelterAssignmentSystem | `Shelter/ShelterAssignmentSystem.cs` | assignment section | event-driven |
| ShelterDecorSystem (Plan 12C) | `Shelter/ShelterDecorSystem.cs` | `shelter_decor` | event-driven |
| AirlockSecuritySystem | `AirlockSecuritySystem.cs` | `airlock_security` | daily |
| ShelterScheduleSystem | `ShelterScheduleSystem.cs` | `shelter_schedule` | daily |
| SilentFoundrySystem | `Foundry/SilentFoundrySystem.cs` | `silent_foundry` | daily |
| ExcavationSystem | `ExcavationSystem.cs` | `excavation` | daily (**world-side sites, not shelter rooms** — see room inventory) |
| DutyRosterSystem | `DutyRoster/DutyRosterSystem.cs` | duty-roster sections | daily |
| EquipmentConditionSystem | `EquipmentConditionSystem.cs` | equipment-condition section | per use (**survivor gear/weapons only**) |
| SumpFloodingSystem | (host-wired, Main.ShelterBatch3) | sump section | flood events → WaterTreatment contamination |
| KitchenNutritionSystem, DecontaminationSystem, GreenhouseSystem, WaystationSystem, LibraryStudy, ArchiveDesk, ContractorRoster, MentalHealthCrisis | host-wired (Main.ShelterBatch3 / ExpandedShelterSystems) | each own section | daily |

## 5. Narrative substrate census (data authority)

| File | Entries | Room binding |
|---|---|---|
| `narrative/bunker_blueprints_codex.json` | 24 blueprints (`room_bp_*`) | **lore-only vocabulary** — no runtime room refs; describes a large pre-war facility (Sub-Levels 1–4, named staff) |
| `narrative/bunker_maintenance_glitches.json` | 20 glitches (`glitch_*`) | free-text subsystems ("Sub-Level 2 Heating Manifold"); repair kits reference **80 item ids that do not exist in items.json** (narrative-only) |
| `narrative/bunker_graffiti_postings.json` | 36 postings (`graf_*`) | free-text locations ("Boiler Room Corridor (-10m)") |
| `narrative/bunker_maintenance_logs_batch_2/3.json`, `bunker_shift_schedules_and_notices.json`, `bunker_wiretap_transcripts*.json`, `bunker_court_verdicts*.json`, `bunker_children_folklore*.json`, `bunker_contraband_barter.json`, `bunker_herbalism_pharmacology.json`, `bunker_trade_ledger_batch_2.json` | large | era/mood substrate |
| `duty_roster_locations.json` | "the_stack" region locations | separate narrative space (The Chart, Inner Airlock…) — **not** the runtime Holdfast room set |
| `codex_entries.json` | 48 entries (regions/locations/factions/deep_lore) | existing codex taxonomy has **no shelter/room-history category yet** |

## 6. Headline Phase 0 findings (feed Phase 1+ gates)

1. **Room ID fragmentation** — four divergent runtime room vocabularies + two narrative
   vocabularies (see `SHELTER_ROOM_INVENTORY.md`). Must be reconciled/aliased before any
   room-identity record lands (Plan 29 §1.4, §5.1).
2. **Location ID drift** — `loc_bunker_holdfast` (StartingLevelSystem) vs `loc_holdfast`
   (locations.json authority).
3. **Triple shielding representation** — StartingLevel room `attenuation`,
   MaterialShieldingSystem per-room ceilings, SkyLayerArmorSystem per-grid cells. No new
   wear work may touch sheltering until this overlap is mapped (see
   `SHELTER_WEAR_PROVENANCE.md`).
4. **Machine condition coverage is real but uneven** — 6 of ~10 major machines have
   genuine condition state; generator, water pump, greenhouse equipment, airlock machinery
   are fuel/state-only or unmodeled (see `MACHINE_CONDITION_PROVENANCE.md`).
5. **`excavation_sites.json` is world-side** — excavation is an expedition/location
   activity, NOT shelter room construction. Plan 29's "constructed/expanded room" class
   currently has **no runtime producer**; renovations would be the first in-shelter
   construction path.
6. **Glitch lore items don't exist** — the 20 canonical glitches' repair kits reference
   ~80 non-existent item ids. Task 29B must either add them to items.json or remap kits to
   existing materials before any glitch becomes mechanically actionable.

## 7. Phase 0 gate verdict

**PASSED.** Provenance is complete for rooms, machines, and wear surfaces. No Plan 29
condition state was introduced. Next phase per implementation order: Phase 1 (room
identity pilot — 2 major rooms, schema, inspection UI, one vignette, codex unlock,
save/load), contingent on the room-ID roster decision in `SHELTER_ROOM_INVENTORY.md` §5.

---

## 8. Phase 5 — wear architecture decision (§29C)

**Decision: projection-only. No new save section for Plan 29 machine/room wear.**

### 8.1 Rationale

1. **Plan 29 catalogs are read-only projections** — `ShelterRoomIdentityCatalog` and
   `ShelterMachineTellCatalog` persist nothing. They read existing system state at query
   time. Adding a wear save section would make Plan 29 a condition authority, violating
   the catalog-as-projection rule established in Phase 0–4.
2. **Existing systems already own and persist all wear state** — the provenance docs
   (`MACHINE_CONDITION_PROVENANCE.md`, `SHELTER_WEAR_PROVENANCE.md`) document 13
   distinct condition authorities, each with its own save section:
   - `StartingLevelSystem` → `starting-level` section (air filter, room attenuation)
   - `SilentFoundrySystem` → `silent_foundry` section (5 facility components)
   - `PowerGridSystem` → `power_grid` section (fuel, battery, brownout)
   - `VentilationSystem` → ventilation section (filter saturation, duct integrity)
   - `WaterTreatmentSystem` → `water_treatment` section (filter integrity)
   - `ShelterThermalSystem` → `shelter_thermal` section (boiler fuel, active state)
   - `AirlockSecuritySystem` → `airlock_security` section (incidents)
   - `MaterialShieldingSystem` → shielding section
   - `SkyLayerArmorSystem` → sky-armor section
3. **No new condition state is needed** — Phase 4's tell evaluator (`BuildMachineReadings`
   in `Main.ShelterInfrastructure.cs`) snapshots live values from these exact systems at
   dashboard refresh time. The tells are projections of real state; they do not require a
   separate persisted copy.
4. **Collision avoidance** — a concurrent session is actively modifying
   `SaveSectionRegistry` and `CampaignEnvelopeBuilder` (Plan 20/23/28 work). Adding a new
   section while the registry is in flight risks merge conflicts and save-format drift.
5. **Phase 5 wear store is deferred, not cancelled** — if a future phase needs persisted
   wear history (degradation curves, repair timestamps, maintenance logs), that is a new
   feature requiring its own design doc. For Plan 29's purposes, the projection layer is
   sufficient and architecturally correct.

### 8.2 What projection-only means for Phase 6+

- Machine tells evaluate against live system state every refresh (no cached wear value).
- Room history vignettes unlock via journal keys (no room-condition dependency).
- The dashboard tell row (`_machineTellLabel`) re-evaluates each frame the dashboard opens.
- If a machine's owner system is not yet initialized (e.g. airlock before Setup), the
  readings builder returns defaults and tells show "NOMINAL".
- No migration path is needed because no new save format was introduced.

### 8.3 Revisit criteria

A wear save section would be reconsidered if:
- A system needs to track wear *between* days without the owner system running.
- A repair action needs to persist "last repaired day" independent of the owner's state.
- The player-facing UI needs to show "wear history" (day-by-day degradation curves).

None of these are Plan 29 requirements.

---

## 9. Phase 9 — machine-condition save integration decision (§29F)

**Decision: no new save section for machine tell state.**

### 9.1 What is already persisted

- **Glitch one-shots** are journal-persisted via `KnowledgeKeys.GlitchNoted(id)` +
  `JournalSystem.UnlockGlitchNoted/IsGlitchNoted`. Old saves default un-noted and
  discover each harmless glitch once. This is the only tell state that needs persistence,
  and it already has it.
- **Fired quirks are not persisted** — and they do not need to be. All 20 quirks in
  `shelter_machine_identities.json` use `repeat_policy: continuous`, meaning they
  re-evaluate against live `MachineConditionReadings` every dashboard refresh. The tell
  row is a projection, not a cached state.

### 9.2 Why no `machine_tell_state` section

1. **Projection-only invariant** — Phase 5 established that Plan 29 catalogs are
   read-only projections. Adding a tell-state section would make the catalog a condition
   authority, duplicating state the owning systems already persist.
2. **No `once_per_crossing` quirks exist** — the `repeat_policy` field supports
   `once_per_crossing` as a future option, but no authored quirk uses it. Building
   save infrastructure for a pattern that does not yet exist is premature.
3. **Collision avoidance** — a concurrent session is actively modifying
   `SaveSectionRegistry` and `CampaignEnvelopeBuilder` (Plan 20/23/28 work). Adding a
   new section while the registry is in flight risks merge conflicts and save-format drift.
4. **Determinism is preserved** — tells evaluate from live system state at query time.
   Loading an old save re-projects tells from the restored system state with no extra
   wiring.

### 9.3 What `once_per_crossing` would require (if ever needed)

If a future phase authors a `once_per_crossing` quirk, the missing infrastructure is:
- An event-layer state store (not the journal — journals are for unlocks, not transient
  condition crossings).
- A threshold-crossing detector in `BuildMachineReadings` or the daily tick.
- A `SaveSectionRegistry` entry + DTO + legacy-bare fallback.

None of this exists today, and none of it is needed for the current 20-quirk roster.

### 9.4 Revisit criteria

A tell-state save section would be reconsidered if:
- A quirk is authored with `repeat_policy: once_per_crossing`.
- The player-facing UI needs to show "missed tells" from the previous session.
- A repair action needs to persist "last quirk fired day" for narrative context.

None of these are Plan 29 requirements.

---

## 8. Phase 1 + Phase 2 verification log (Task 29A, 2026-09-01)

Room identity contract, 11 room records, 8 vignettes, 44 fixtures, three unlock seams, and
the `loc_bunker_holdfast` → `loc_holdfast` migration are landed. Evidence at time of writing:

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS — 0 errors |
| `dotnet test Ashfall.Core.Tests` | **PASS 5819/5819** (Plan 29 added 26 tests) |
| `dotnet build Ashfall.csproj` | PASS — **0 warnings, 0 errors** |
| `--data-integrity-selftest` | **PASS — 0 findings, 7062 ids, 159 catalogs** (includes `shelter_room_identities.json`) |
| `--bridge-selftest` | PASS |
| journal / journal-save | PASS (20/20; Places row count contract now `Locations + RoomHistories`) |
| shelter-interior / shelter-operations / shelter-hazard / shelter-decor | PASS |
| operations / phase0 / content-utilization | PASS |
| real-campaign-journey / seven-day-smoke / deterministic-smoke / campaign-fuzz | PASS (new `shelter_room_history` day owner advances cleanly) |
| day1 / day1-playable / onboarding / save-load | PASS |
| silent-foundry / duty-roster | PASS |
| save-load-ui-failure / panel-lifecycle / panel-bind / panel-bind-lifecycle | PASS |
| layout / ui-layout / ui-accessibility / accessibility | PASS (identity surfaces are tooltip-only; no rendered pixels changed) |

**Concurrent-worktree note:** during this phase another session was actively editing
untracked Plan 20/23 files (`muster_faction_actions.json`, `faction_war_events.json`,
`muster_witnesses.json`, `Plan23DiveMechanicCoverageTests.cs`,
`FactionActionBoardTests.cs`, `WitnessSelectionTests.cs`, `TideCalendar.cs`). Transient
data-integrity findings (8 → 11 → 13) and one 5-failure test run were observed from those
files; every finding resolved to non-shelter paths, and both gates finished green.

**Shelter-side files touched by Plan 29:** see `ROOM_IDENTITY_CONTRACT.md` §4–§5.
No condition state was added; no existing authority was duplicated.

---

## 10. Phase 11 — playthrough smoke test + Phase 12 review (§29H)

### 10.1 Goal
Verify that Plan 29 panel surfacing, room-history unlocks, machine tell projections, and
Places-tab rendering hold end-to-end through the canonical headless playthrough verbs.

### 10.2 Smoke-test battery (2026-09-02)

| Verb | Result | Notes |
|---|---|---|
| `--day1-selftest` | **PASS** | New game starts on Day 1; inventory/greenhouse reload valid |
| `--real-campaign-journey-selftest` | **PASS** | Composed campaign services; inventory consume mutates state (5 → 4) |
| `--seven-day-smoke-selftest` | **PASS** | 7-day progression clean |

All three verbs complete without abort. Godot headless emits resource-leak warnings at
exit (RID allocations for viewports/textures/shaped text); these are pre-existing
headless-cleanup noise, not Plan 29 regressions — they appear on `--day1-selftest` and
`--bridge-selftest` as well.

### 10.3 Panel surfacing verification

| Surface | Status | Evidence |
|---|---|---|
| `GameDashboardPanel` machine tells | **Wired** | `BuildMachineTellText` prefixes all 7 tells with `[MACHINE]` labels; dashboard renders `MACHINES // ...` with severity color coding |
| `SilentFoundryPanel` tell line | **Wired** | `MACHINE // ...` label in cast detail box; evaluates `machine_foundry_cupola` quirks against live `SilentFoundrySystem` component conditions |
| `HoldfastInteriorView` tooltip machines | **Wired** | Room tooltips append `Machines: ...` for rooms hosting machines (static identity only) |
| Places-tab room-history vignettes | **Finalized** | Locked rows show room name (no spoilage); unlocked rows show title + era + body; pinned by `JournalSelfTest` |
| `repair_performed` triggers | **Partial** | `room_filtration` wired via dashboard filter buttons; other rooms deferred — no dedicated repair UI exists |

### 10.4 Gaps documented for future phases

1. **`room_water_pump` data drift** — `power_grid.json` references `room_water_pump` but it is absent from `shelter_room_identities.json`. Not a Plan 29 blocker (the room has no identity record, no machine tell catalog entry, and no interior hotspot). Defer to a data-hygiene phase.
2. **`repair_performed` vignettes without UI** — `room_storage_bay`, `room_foundry`, `room_main` each have `repair_performed` vignettes in the data, but no repair action button exists in the shelter UI. Current behavior: these vignettes remain locked indefinitely. Adding fake buttons would violate §29A.12 (no action without gameplay backing). Defer until a repair/work-order UI is planned.
3. **Machine tell tooltip is static** — `HoldfastInteriorView` shows machine names only, not live condition bands. This is intentional: the dashboard is the live surface; the interior view is a spatial map. If future UX calls for condition bands in the tooltip, route through `ShelterMachineTellCatalog.EvaluateBand`.
4. **Full-suite `dotnet test` blocked by concurrent session** — Plan 29 scoped suites (59/59) pass. The full suite fails to compile due to unresolved symbols in the concurrent session's in-flight ecology/muster files (`EcologyBalanceSimulationTests.cs`, `HostCli.EvolvingWorld.cs`). Not a Plan 29 regression.

### 10.5 Phase 12 review verdict

Plan 29 Phases 0–11 are **complete and verified** within the concurrent-worktree constraint.
All 5 canonical gates pass for Plan 29 scope. No condition state was duplicated; no new
authority was introduced. The shelter is now a readable, discoverable space with:

- 12 room identities, 49 fixtures, 20 room-history vignettes
- 7 machine identities, 20 quirks, 11 glitch events
- Full panel surfacing (dashboard, foundry, interior tooltips, Places tab)
- Journal-persisted unlocks (room history + glitch notes)
- Deterministic, projection-only tells from live system state

**Recommended next step:** Coordinate with the concurrent session to commit or stabilize
their ecology/muster edits, then run the full `dotnet test` suite to confirm zero
Plan 29 regressions before merging.
