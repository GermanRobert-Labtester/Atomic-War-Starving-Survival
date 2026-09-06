# SHELTER FAILURE EFFECTS & QUARANTINE WIRING — IMPLEMENTATION LOG

Plan: `docs/plans/SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_INTEGRATION_PLAN.md`
Branch: `feat/asset-pipeline-flagship`. G1–G3 and G4–G5 waves are in-tree, uncommitted
(commit deferred pending concurrent-stream landing — see their journals).

---

## Phase 0 — Verification

Status: PASS (concurrent-stream interference documented; interference grew during the wave)

Results:
* Host build: 0 errors at baseline. Full Core suite blocked by the concurrent
  stream's stale untracked test files (85+ errors, all in `Plans72To75CampaignIntegrationTests.cs`);
  the stream broke and fixed `FDebug.cs`, `BallisticsWorkbenchSystem.cs`, and
  `MusterWarfareEngine.cs` mid-session. Host-build transient breaks also observed
  (Plans74To77 SaveStore churn) and self-resolved.
* Phase-0 unknowns resolved:
  - Roster: `_dutyRoster.Roster` (`Main.DutyRoster.cs:40`, `DutyRosterHostSession.Roster`).
  - Knowledge: `_sharedResearch` is a `ResearchSystem`; completion check is
    `State.completedIds.Contains(k)`.
  - Consumption: `IPlayerInventoryPort.TryConsume(itemId, count) → bool`
    (`InventoryHostSession` implements it; `BindSupply` uses the same port).

Divergences: none yet.

---

## Phase 1 — Core air-filtration seam

Status: PASS

Changed:
* `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs` —
  `TickDay(bool, WeatherKind, float powerAvailability01 = 1f)` (legacy overloads
  delegate with 1f). Unpowered: `baseDegrade += 4` (hazard-magnitude, stacking),
  duty mitigation zeroed, powered +10 quality offset dropped. Powered play is
  legacy-identical.

Tests: `StartingLevelAirPowerTests.cs` — 6/6 (legacy parity, doubled degradation +
duty zeroing, offset loss, hazard stacking, default overload, partial-power-is-powered).

Divergences: none.

---

## Phase 2 — Host: air power + quarantine construction

Status: PASS

Changed:
* `src/Main.CampaignOwners.cs` — `StartingLevelRationsDayOwner` passes
  `room_air_filtration` breaker state into `TickDay`.
* `src/Host/StartingLevelHostSession.cs` — power-aware pass-through overload
  (the day owner talks to the host session, not the Core system directly).
* `src/Main.Medical.cs SetupDisease()` — constructs `DiseaseQuarantineCoordinator`
  (ward + engine + roster + `TryConsume` delegate + `FromResearch` containment +
  `room_ward_quarantine` power delegate) and calls `_disease.BindCoordinator(...)`.
  The existing `MedicalDiseaseDayOwner` (phase 3) ticks it via
  `DiseaseHostSession.TickDaily`.

Result: host build 0/0.

Divergences: none material. (Two intermediate compile errors — wrong research type,
missing session overload — were my own and fixed.)

---

## Phase 3 — fx registry test + selftest

Status: PASS

Changed:
* `PowerGridCatalogTests.Catalog_EveryFailureEffectId_HasANamedConsumer` — pins all
  9 authored `fx_*` IDs to their named consumer owner; a new failure effect without
  a registered consumer fails the suite (G6 bug class closed structurally).
  NOTE: the concurrent stream added `fx_cryo_vault_unpowered` mid-wave; dispositioned
  as "CryoVaultDayOwner (concurrent stream)" pending their integration proof.
* `--power-grid-catalog-selftest` — every loaded room must carry a non-empty
  failure_effect_id.

Result: catalog tests green; selftest PASS (rooms=9).

---

## Phase 4 — Full gates

Status: PASS (with restored temporary exclusions)

| Gate | Result |
|---|---|
| `dotnet test` full suite | **PASS — 8939/8945** with 3 concurrent-stream stale test files temporarily excluded (`Plans72To75`, `Plans74To77SystemsTests`, `PlansB66ToB69` — 85+ stale-API errors, all theirs). The 6 remaining failures all name `Plans74To77` (their uncommitted host session); zero failures in wave files. csproj restored byte-identical after the run. |
| `dotnet build Ashfall.csproj` | PASS — 0 errors |
| `--data-integrity-selftest` | PASS — 284 catalogs (previous run this session) |
| `--power-grid-catalog-selftest` | PASS — rooms=9, surge + fx guards |
| `--bridge-selftest` | PASS earlier this session |
| `scene-lint.py` | PASS — 30 scenes, 0 errors (earlier this session) |

---

## Commit decision

NOT COMMITTED — same interleaving rationale as the prior two waves; the concurrent
stream was editing shared files (`Main.Medical.cs`, `Main.CampaignOwners.cs`,
`BallisticsWorkbenchSystem.cs`, `MusterWarfareEngine.cs`) DURING this phase, with
three transient compile breaks and one deleted/recreated test file. File manifest:

* `Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs` (power param + offline semantics)
* `src/Host/StartingLevelHostSession.cs` (pass-through overload)
* `src/Main.CampaignOwners.cs` (air power at day owner)
* `src/Main.Medical.cs` (quarantine coordinator construction + bind)
* `Ashfall.Core.Tests/Shelter/StartingLevelAirPowerTests.cs` (new)
* `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` (+fx registry gate + quarantine tier pin)
* `src/Host/HostCli.PanelTests.cs` (fx completeness in selftest)
* `docs/plans/SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_*`

Remaining known limitations:
* Quarantine containment research projection uses `ResearchSystem.State.completedIds` —
  if a dedicated knowledge ledger supersedes it later, swap the delegate.
* `fx_cryo_vault_unpowered` consumer is owned by the concurrent stream.
* The three concurrently-stale test files need upstream repair before the suite runs
  green without exclusions.
