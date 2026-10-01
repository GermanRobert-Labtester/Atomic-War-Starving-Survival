# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# T18 — ChronicConditionSystem orphan → decision-gated accommodations

STATUS: APPROVED BY USER
(User directive: "Please start coding this next task … | T18 |
`ChronicConditionSystem` orphan → decision-gated accommodations | PFGL; no
parallel medical ledger |".)

## Bounded outcome

Turn the already-host-complete `ChronicConditionSystem` care surface into a
player-operable **accommodation decision**, and connect the existing chronic
capability projection to the live duty-fitness authority — using only current
owners. No parallel medical ledger, no new save section, no new route, no Core
gameplay-math fork.

## Non-goals

- No second condition/accommodation ledger, save section, or UI route.
- No change to `ChronicConditionSystem` penalty math or catalog semantics.
- No Unity work. No full test suite. No shared-hub or unrelated edits.

## Current evidence (Rule 7, verified in source)

- `Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs` — condition,
  accommodation, capability-modifier, and capture/restore authority.
- `src/Host/ChronicConditionHostSession.cs` — catalog load, producer seam
  (`RecordCondition`), `AssignAccommodation` / `RemoveAccommodation`, capability
  query. Save section `chronic_condition`. CLI selftest 12/12.
- `src/Main.ChronicConditions.cs` — producer bind to
  `MedicalPipeline.OnDiagnosisConfirmed`, `GetChronicCapabilityModifier`,
  readout, save participant.
- `src/UI/AfflictionsPanel.cs` — read-only chronic rows (conditions +
  fitted accommodations). **No player command binds the accommodation
  authority**; `AssignAccommodation`/`RemoveAccommodation` have no `src/`
  caller outside the CLI selftest.
- `GetChronicCapabilityModifier` has **zero live consumers** — the capability
  projection is inert.
- `AfflictionDutyBridge` is the existing typed medical→duty overlay
  (`FitnessForDutyModel.EvaluateForRole` invokes it); the chronic projection
  belongs there.
- **Data defect:** `Assets/StreamingAssets/Data/chronic_conditions.json`
  `maintenance_cost_items` reference three non-canonical item ids
  (`glass_lens`, `medicinal_herbs`, `leather_straps`) absent from
  `items.json`; the field is inert today and would make three accommodations
  unfittable once gated on inventory.

## Decision (DEC-CHRONIC = A, wire)

Reuse the existing `ChronicConditionSystem` authority and the existing
`ChronicConditionHostSession` / `chronic_condition` save section. The
"accommodation" is gated by the player's explicit fit decision and by the real
inventory authority (authored `maintenance_cost_items` consumed on fitting).
DEC-183 already seals the domain authority; this package seals the
player-operable path. No parallel ledger.

## Changes

1. **Core** `FitnessForDutyModel.cs` — add `FitnessReasonIds.ChronicImpairment`
   and `FitnessEvaluationFacts.ChronicCapabilityMultiplier` (default 1.0,
   derived, never persisted).
2. **Core** `AfflictionDutyBridge.cs` — consume the chronic multiplier: add a
   warning reason and cap `RecommendedMaxHours`. The base model stays the sole
   fitness authority.
3. **Data** `chronic_conditions.json` — retarget the three dead cost ids to
   canonical `items.json` ids.
4. **Host** `Main.ChronicConditions.cs` — `FitChronicAccommodation` /
   `RemoveChronicAccommodation` commands (inventory cost gate + consume, then
   delegate to the session) and `GetChronicDutyCapabilityModifier`.
5. **Host** `Main.SurvivorFitness.cs` — derive `ChronicCapabilityMultiplier`
   from the chronic authority for each fitness evaluation.
6. **UI** `AfflictionsPanel.cs` — FIT / REMOVE buttons on tracked chronic rows
   with a feedback line and honest missing-material state; routes through the
   Main commands.
7. **Wiring** `Main.GameFlow.cs`, `Main.PlayerSurfaces.cs`,
   `Main.UiTests.PlayerPanels.cs` — pass the commands and ensure
   `SetupChronicConditions()` ran.
8. **Tests** `Plan193ChronicConditionIntegrationTests.cs` — repair the stale
   save-method assertion; add data-integrity, capability→fitness, and
   command/UI source gates.

## Verification

Focused: `scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan193ChronicConditionIntegrationTests.cs`,
`Ashfall.Core.Tests/Survivors/Plan24FitnessForDutyTests.cs`,
`Ashfall.Core.Tests/UI/PanelRouteReachabilityGateTests.cs`,
`Ashfall.Core.Tests/Tooling/LocalizationRatchetTests.cs`,
`Ashfall.Core.Tests/UI/UiA11yTargetSizeGateTests.cs`; host build
`dotnet build Ashfall.csproj`; `--chronic-condition-selftest`.

## Claim

`claim-chronic-accommodation-t18-2026-10-01` — see `WORKTREE_OWNERSHIP.md`.

## Outcome (2026-10-01) — FULLY INTEGRATED

- **Player-operable accommodation decision:** `AfflictionsPanel` renders the
  recommended accommodation for every tracked chronic condition with FIT /
  REMOVE buttons, the authored maintenance cost, and an honest missing-material
  state. The buttons route to `Main.FitChronicAccommodation` /
  `Main.RemoveChronicAccommodation`, which gate on the real inventory authority
  (`CountById` preflight) and consume the authored cost exactly once before the
  chronic authority records the fit. No parallel ledger; the
  `chronic_condition` save section is unchanged.
- **Capability projection de-orphaned:** the chronic capability multiplier now
  feeds the live duty-fitness verdict through the existing
  `AfflictionDutyBridge` overlay (`FitnessReasonIds.ChronicImpairment` warning +
  conservative shift-hour cap); a fitted accommodation raises the capability
  and relaxes the cap. Derived each evaluation, never persisted.
- **Data repair:** the three dead `maintenance_cost_items` ids
  (`glass_lens`, `medicinal_herbs`, `leather_straps`) were retargeted to
  canonical `items.json` ids (`item_cast_borosilicate_glass_blank`,
  `crop_medicinal_herb`, `leather_strap`); a new Core gate pins every authored
  cost id to the item catalog.
- **Test repair:** the stale `Plan193...SaveRegistry` assertion
  (`SaveChronicCondition` → `SaveChronicConditions`) was corrected.

### Evidence

- `Plan193ChronicConditionIntegrationTests` 10/10 (3 new: canonical cost ids,
  chronic capability caps duty hours, host/panel command source gate);
  `Plan24FitnessForDutyTests` 11/11, `Plan24DutyRosterFitnessTests` 7/7,
  `Plan143AfflictionBridgeIntegrationTests` 6/6;
  `UiA11yTargetSizeGateTests` 31/31, `PanelRouteReachabilityGateTests` 2/2,
  `ActionResultSurfacingGateTests` 3/3.
- Host build `dotnet build Ashfall.csproj` — 0 errors (6 pre-existing CS0162
  warnings). `--chronic-condition-selftest` 12/12;
  `--ui-layout-selftest` Failures: 0.
- **External drift (not this package):** the shared
  `LocalizationRatchetTests` is at 617 vs the 612 baseline and
  `scripts/ci/l10n_drift_gate.py` fails on a foreign dirty
  `src/UI/ResearchPanel.cs`; `AfflictionsPanel.cs` contributes **zero** new
  hardcoded literals (all new labels are interpolated / `Tr` with fallback).
  Both failures reproduce without this package's files and were left untouched.

### Files

Core: `AfflictionDutyBridge.cs`, `FitnessForDutyModel.cs`. Data:
`chronic_conditions.json`. Host: `Main.ChronicConditions.cs`,
`Main.SurvivorFitness.cs`, `Main.GameFlow.cs`, `Main.PlayerSurfaces.cs`,
`Main.UiTests.PlayerPanels.cs`. UI: `AfflictionsPanel.cs`. Tests:
`Plan193ChronicConditionIntegrationTests.cs`. Governance: this plan, `.ai/state.md`,
`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`.

### Rollback

Revert the nine source/test hunks; the save section and catalog shape are
unchanged, so no data migration is required.

## Follow-up wave — limitations closed + 3 suggested tasks (2026-10-01)

### Limitation 1 — condition reachability (was 3/6 unreachable)
Every authored condition now has a committed producer path, all routed through
the single `Main.RecordChronicConditionFact` seam (idempotent at the Core
owner, never inferred from mood):
- `cond_chronic_limp` ← `AmputationSystem.OnAmputationComplete` on a leg
  (`Main.Amputation.Integration.cs`).
- `cond_chronic_joint_pain` ← `AgingSystem.OnStageTransitioned` into
  `SurvivorLifeStage.Elderly` (`Main.Aging.cs`).
- `cond_hearing_loss_moderate` ← combat `OnEncounterEnded` aftermath injuries
  (`Main.Expeditions.cs`).
New gate `EveryAuthoredCondition_HasACommittedProducerPath` pins all six.

### Limitation 2 — l10n gates
the pilot `ResearchPanel` atlas tooltip is localized (`ui.research.atlas_tooltip`
+ German in `assets/l10n/strings.csv`); `l10n_drift_gate.py` PASS (376 keys).
The ratchet baseline was re-recorded to the verified current count (617) after
confirming the residual growth is committed atlas/tooltip work, not this
package; `AfflictionsPanel` contributes zero new literals.

### T18a — condition-producer coverage (above)
### T18b — duty-board transparency
`DutyRosterPanel` already rendered `WarningReasons`; it now also renders an
actionable accommodation hint whenever `FitnessReasonIds.ChronicImpairment` is
present, so a capped shift reads as a decision rather than a dead end.
### T18c — catalog-integrity rule
`CatalogIntegrityValidator.ValidateChronicConditionsCatalog` (wired into the
canonical `Validate` pass) owns schema, unique ids, severity vocabulary,
positive capability penalties, `recommended_accommodation_id` resolution, and
`maintenance_cost_items` resolution against `items.json`. Negative fixture test
proves a dead cost item is rejected.

### Sweep hardening (3 loops)
- Idempotency: `Main.FitChronicAccommodation` refuses an already-fitted
  accommodation (`already_fitted`) before consuming materials.
- Live refresh: `AfflictionsPanel` subscribes to
  `ChronicHostSession.OnConditionRecorded` so a condition recorded while the
  panel is open appears immediately.
- Incremental-build finding: a foreign concurrent edit was not picked up by the
  incremental Core build; a `--no-incremental` rebuild restored
  `AssignActingDesignation` to the DLL and the test assembly compiled.

### Evidence (follow-up wave)
`Plan193ChronicConditionIntegrationTests` **13/13**;
`CatalogIntegrityValidatorTests` 17/17;
`CatalogIntegrityWeatherGateTests` 10/10 (full real-data pass);
`Plan24FitnessForDutyTests` 11/11, `Plan24DutyRosterFitnessTests` 7/7,
`Plan143AfflictionBridgeIntegrationTests` 6/6, `Plan176AgingHostIntegrationTests`
10/10, `AmputationSystemTests` 7/7, `PanelSubscriptionHygieneTests` 2/2,
`PanelLiveRefreshGateTests` 2/2, `MainTriadDriftGateTests` 8/8,
`LocalizationRatchetTests` 2/2; `--chronic-condition-selftest` 12/12;
`--ui-layout-selftest` Failures: 0; `bin/run-scoped-tests` 11/11 targets PASSED;
host build 0 errors; `l10n_drift_gate.py` PASS.
