# ASHFALL — Task 120: Persisted First-Hour Onboarding Journey

**Plan ID:** `onboarding_journey_v1`
**Goal:** Convert the static Tutorial panel into a stateful, persisted onboarding
journey whose objectives are detected from *real* runtime commands and state, so a
new player reaches Day 2 through real systems without developer knowledge, and the
journey survives interruption (save/load).

**Dependency note (123):** the referenced task's spec was not resolvable from the
repository (external tracker). This task is self-contained; the Opening Protocol
is reused as the first objective as the spec directs.

---

## Baseline (reported before implementation)

- `dotnet test` (Core): **4330/4330 pass**.
- `dotnet build Ashfall.csproj`: **FAILED on HEAD** — pre-existing, unrelated to this
  task. Repaired minimally (see §Plan Divergences): `PhantomMemoryPanel` missing brace;
  `DailyBriefingModal` undeclared `footer`; `Main.Lifecycle` refs to removed dirty flags;
  `ExpeditionHostSession`/`FactionBranchHostSession` `+= RaiseStateChanged` (now returns
  `bool`); missing `using System.Linq` (×2); wrong `.OutdoorRadModifier` path.
- After repair: **0 errors (1 pre-existing null-derf warning)**, `--bridge-selftest`,
  `--data-integrity-selftest`, `--day1-to-day2-selftest` all PASS.

---

## Architecture (Core = authority, host thin)

```
Assets/Ashfall.Core/Onboarding/
  OnboardingEnums.cs        stage keys, AssistanceLevel, signal vocabulary
  OnboardingProgress.cs     primitive progress snapshot + stage catalog (completion conditions)
  OnboardingJourney.cs      stateful stage machine (record, skip/replay, dismiss, contextual hints, save)
  OnboardingSaveState.cs    serializable DTO
src/
  Host/OnboardingSaveStore.cs        SaveStoreHub.Checksummed<OnboardingSaveState>
  Host/OnboardingProgressTracker.cs  maps REAL host state → OnboardingProgress
  Main.Onboarding.cs                 Setup/Save/Restore + command routing + hint wiring
  UI/OnboardingHintPanel.cs          accessible hint surface (skip/replay/show-me-where)
```

**No fabrication:** the tracker only *reads* live state and *records* observations;
it never mutates inventory/survivors/duty/weather. Completion is declared against the
real signaling state of the authoritative hosts.

---

## Stage order (all satisfiable via real systems)

1. `protocol`       — resolve all 3 Opening Protocol directives (ration, maintenance, radio)
2. `inspect`        — inspect ≥3 shelter rooms
3. `rationing`      — ration policy set **and** stores opened
4. `assignment`     — ≥1 duty-roster assignment
5. `weather`        — weather read (forecast/history/weather panel observed)
6. `inventory_use`  — ≥1 item equipped/consumed
7. `day_advance`    — day 1 → 2 committed (terminal objective)

Assistance level (`minimal/standard/guided`) persists in **both**: journey save state
and `UserSettingsData` (so it survives even pre-first-save).

---

## Persistence

- `onboarding` save section added to `SaveSectionRegistry` (`onboarding_save.json`),
  captured/restored through the existing `CampaignEnvelopeBuilder` / `SaveAll` pipeline
  so the journey resumes at the correct step after save/load with zero new persistence
  architecture.
- `OnboardingSaveState` uses public fields (checksum-compatible), `schemaVersion = 1`,
  legacy-bare-state load **disabled** (no pre-existing format).

---

## Verification ladder

1. `dotnet test` (new `OnboardingJourneyTests` + full suite)
2. `dotnet build Ashfall.csproj` (0 errors)
3. `godot --headless --path . -- --onboarding-journey-selftest` (new Day1→Day2 journey:
   real commands, mid-run save/load resume, no-fabrication assertion)
4. `godot --headless --path . -- --data-integrity-selftest`
5. `godot --headless --path . -- --bridge-selftest`