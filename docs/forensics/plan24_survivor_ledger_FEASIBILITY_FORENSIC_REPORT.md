# Plan 24 Survivor Ledger — Feasibility Forensic Report

## 1. Target

**Target plan:** `C-integration-plans/C1_planintegration[5].md`

**Plan:** 24 — “One Survivor Ledger — Fitness, Fatigue, Skill, Duty, Illness & Recovery”

**Requested outcome:** determine whether the plan can be integrated against the current repository, and identify the smallest safe integration boundary.

**Scope of this report:** read-only implementation feasibility assessment. No production code, gameplay data, save schema, or governance ledger was changed.

## 2. Executive Finding

**Verdict: NOT integrable as written. PARTIALLY INTEGRABLE after a dependency-aware rebase and split.**

[Evidence] The plan’s central `FitnessVerdict` and `NeedsModifierStack` types do not exist in the repository, while duty roster, needs, schedule, skills, caregiving, medical, quarantine, fate, social, save, and UI systems already exist with established owners.

[Inference] Plan 24 is not a greenfield build. Its safe shape is an extension of existing authorities, with 24A first, a narrowly scoped 24B design/pilot, and a 24C wiring/repair package. The plan’s original “build all three workstreams” shape is too broad to accept as one implementation package.

[Evidence] Current focused checks pass for duty, apprenticeship, caregiving, quarantine, fate, and skill progression, but the focused needs characterization target has one baseline failure: 14 passed and 1 failed because the test expects 7 restored need events while the current nine-need model emits 9.

[Evidence] Governance currently presents the distress-signals batch for acceptance and lists future waves as planned/not claimed. There is no Plan 24 ownership claim, and several likely Plan 24 paths are already claim-protected by existing or completed claims.

## 3. Evidence Summary

| Area | Current evidence | Feasibility consequence |
|---|---|---|
| Fitness projection | No `FitnessForDutyModel` or `FitnessVerdict` implementation found | 24A has a genuine greenfield center |
| Duty validation | Existing assignment engine checks roster, reservation, eligibility, status, and duplicate role only | Fitness can be added at this seam; do not replace the roster |
| Role requirements | Existing five duty roles are code-defined; no fitness requirement catalog was found | Requires an explicit data-authority decision |
| Needs | `NeedsSystem` owns simulation; `NeedsComponentStore` owns detached persistence; no modifier stack | 24B needs an effect inventory and pilot before broad conversion |
| Schedule | Sleep state and recovery modifier exist, but schedule does not feed Needs | A bounded schedule-to-needs bridge is possible |
| Skills | Campaign-shared progression is already live in the main route | Remove duplicate fallback construction; do not rebuild skill authority |
| Trapping | Actual hunter identity is already used to project skill | Plan claim is partially satisfied |
| Quarantine | Existing coordinator admits ward patients and clears conflicting duty | Extend this coordinator; do not add another isolation path |
| Death | `SurvivorFateSystem` already fans death through roster, needs, duty, care, medical, social, memorial, journal, flags, and briefing | 24C should repair and extend the funnel |
| Caregiving | Core tick has fatigue/recovery hooks, but `Main.SetupCaregiving` does not wire them | Genuine host integration gap |
| Medical admission | `MedicalWardSystem.Admit` does not reject a second active admission for the same patient | Genuine idempotence gap |
| Typography/local UI | Existing panels and accessibility/snapshot infrastructure exist | Validate touched surfaces; avoid a second UI authority |

## 4. Architecture Placement

[Evidence] Core logic lives in `Assets/Ashfall.Core/` and current project rules require it to remain engine-free. Godot hosts and adapters live in `src/`. JSON authority lives under `Assets/StreamingAssets/Data/`.

[Inference] A safe Plan 24 implementation should use these ownership boundaries:

```text
Core Fitness projection
  → receives survivor facts + role requirements
  → returns derived verdict/reasons
  → never persists the verdict

Host/day owner
  → supplies live survivor, needs, disease, medical, schedule, and duty facts
  → routes the verdict to existing duty/expedition/producer seams

Existing NeedsSystem
  → remains the numeric needs simulation owner
  → may gain a typed effect/modifier seam only after effect semantics are resolved

Existing Quarantine/Fate/Caregiving/Medical owners
  → remain the event and lifecycle authorities
  → emit or consume the new facts through their current seams
```

[Inference] A new generic survivor god-object, parallel death system, parallel quarantine system, or parallel needs store would violate current repository rules and would make this plan unsafe.

## 5. Current Implementation

### 5.1 Duty and survivor eligibility

[Evidence] `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs:42-82` validates assignment through external reservation, candidate eligibility, row existence, role availability, survivor status, and duplicate-role checks. It does not evaluate needs, dose, disease, quarantine, fatigue, or other fitness conditions.

[Evidence] `src/Main.DutyRoster.cs` binds candidate eligibility to the existing cohort work-eligibility rule. The current bridge is age/cohort-oriented, not a full medical or fitness projection.

[Evidence] `DutyRosterRow` already stores `lastSleptDay`, and duty capture/restore persists it. `BuildHomeOccupantSnapshot` currently marks every living roster occupant as `sleptHere = true`, which is not a reliable source for a fitness calculation.

[Inference] 24A should add a derived projection and feed it into `DutyRosterAssignmentEngine` and its auto-assignment path. It should first replace or correct the sleep observation seam instead of treating the current snapshot value as authoritative.

### 5.2 Needs and schedule

[Evidence] `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:10-20` defines nine needs with mixed polarity: Hunger, Thirst, Fatigue, Morale, Numbness, and RadiationAnxiety are worse when higher; Warmth, Health, and Hygiene are worse when lower.

[Evidence] `NeedsSystem.cs:192-240` applies direct base drift, heat behavior, and direct external `Modify` calls. There is no `NeedsModifierStack` implementation.

[Evidence] `Assets/Ashfall.Core/Survivors/NeedsComponentStore.cs:108-121` identifies the component store as a detached persistence boundary and explicitly leaves simulation ownership with `NeedsSystem`.

[Evidence] `Assets/Ashfall.Core/ShelterScheduleSystem.cs:18-22,188-244` persists sleep assignments and calculates recovery/lighting state, but its day tick does not update a survivor’s Needs state. The host ticks schedule and Needs through separate owners.

[Inference] The schedule recovery modifier is a real integration seam, but 24B cannot safely convert all social, thermal, ration, stress, overwork, and caregiving effects in one pass until each direct write is inventoried by cadence, polarity, source, and expected stacking behavior.

### 5.3 Skills and worker identity

[Evidence] `src/Main.CampaignServices.cs:140-183` owns one campaign-shared `SkillProgressionSystem`, registers the catalog, and ticks living survivors through that shared instance. `src/Main.PlayerSurfaces.cs:604` binds the skill panel to the same shared instance.

[Evidence] `src/Main.ShelterSocial.cs:494-505` passes the shared skill system into apprenticeship and restores its state. `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs` passes its focused target.

[Evidence] `src/Host/ApprenticeshipHostSession.cs:17-28` still constructs `new SkillProgressionSystem()` in a default/fallback path along with fallback roster/relations. The live main setup supplies the shared instance, but the default constructor remains a duplicate-authority risk.

[Evidence] `WildlifeTrappingSystem` uses the assigned hunter identity to obtain projected skill during selection. The plan’s trapping-to-skill claim is therefore partly already implemented.

[Inference] 24B should begin with a small identity/constructor gate and a producer inventory. It should not reimplement the campaign skill system or repeat already-closed catalog externalization work.

## 6. Runtime Wiring

[Evidence] The host day flow already ticks duty, needs, skill, apprenticeship, caregiving, thermal, and schedule systems from `Main` partials. This is an existing integration graph, not an empty host.

[Evidence] `Main.SetupCaregiving` creates/restores the Core caregiving system and binds its panel, but no assignments were found wiring `CanProvideCare`, `NeedsCare`, `ApplyFatigueDelta`, or `ApplyHealthRecoveryBonus` in `src/Main.ShelterSocial.cs:523-536`.

[Evidence] `CaregivingSystem.cs:64-82` exposes those host hooks, and `CaregivingSystem.cs:248-276` invokes them during active care ticks.

[Inference] Caregiving is a concrete 24C host-wiring gap. The fix belongs at the existing setup seam and should route fatigue/recovery through the existing Needs and health owners.

[Evidence] The current medical/quarantine setup constructs a `DiseaseQuarantineCoordinator` around the existing medical ward, disease system, duty roster, inventory, containment, and power authorities.

## 7. Data Flow

[Evidence] `Assets/StreamingAssets/Data/duty_roster_seasons.json` contains seasonal encounter data but no role fitness requirements. Existing duty roles are represented by `DutyRosterIds.AssignmentRoles` in Core.

[Evidence] `skills.json`, `disease_catalog.json`, and `shelter_schedules.json` are existing data authorities consumed by current loaders.

[Inference] Role fitness requirements require a new authority decision before code work: either extend the existing duty-role data contract or create a narrowly scoped role-requirements catalog. The plan’s examples are insufficient to decide whether requirements apply to all five roster roles, to producer jobs, to expeditions, or to a separate labor taxonomy.

[Inference] Producer worker identity should be integrated one consumer at a time. Kitchen already accepts a cook identity; workshop/foundry/expedition consumers have different contracts. A universal worker context should not be invented without an inventory of those contracts.

## 8. State Ownership

[Evidence] Existing state owners are:

| State | Current owner |
|---|---|
| Needs values | `NeedsSystem` |
| Needs persistence | `NeedsComponentStore` through the existing survivor/save path |
| Duty assignments and sleep record | `DutyRosterSystem` |
| Schedule assignments/recovery state | `ShelterScheduleSystem` |
| Skill progression | campaign-shared `SkillProgressionSystem` |
| Care assignments/bonds | `CaregivingSystem` |
| Ward admissions/procedures | `MedicalWardSystem` |
| Isolation workflow | `DiseaseQuarantineCoordinator` plus disease/ward authorities |
| Death cascade | `SurvivorFateSystem` |
| Social/ration/leadership effects | existing social authorities and Needs hooks |

[Inference] `FitnessVerdict` should be a pure projection with no save section. A modifier stack may need state only if modifiers are not reconstructible from existing authorities; that decision must be made before adding persistence.

## 9. Save/Load

[Evidence] Duty roster save captures assignments and `lastSleptDay`; schedule, medical ward, caregiving, survivor fate, social, needs, and shared skill each already have established save/restore routes.

[Evidence] `SaveSectionRegistry` already registers the survivors-related state and existing systems’ sections. No Plan 24-specific fitness or generic modifier section exists.

[Inference] Derived fitness must be recomputed after load from restored survivor facts and current data. Introducing a saved verdict would create stale-state risk.

[Inference] A broad modifier stack should not receive a new save section until a reconstruction test proves whether its active modifiers are fully derivable from duty, schedule, disease, social, ration, and caregiving state. If it stores only current source facts, those facts belong to their existing owners.

## 10. Determinism

[Evidence] Current target systems use persisted state and existing deterministic ordering/RNG conventions; no new `System.Random` requirement was identified in the Plan 24 seams reviewed.

[Inference] Fitness evaluation must be pure and order-independent. Role and survivor iteration should use stable IDs/ordinal ordering where an ordering is required.

[Inference] Needs modifiers must apply at a specified simulation cadence, not from render refresh or frame timing. Any recovery or fatigue conversion must have a paired continuous-versus-interrupted save test.

[Required verification] Same seed and actions under the normal locale/settings and any diagnostic/pseudo mode must produce identical gameplay state, save checksum, IDs, RNG streams, and event ordering. Only presentation may differ.

## 11. UI / Player Feedback

[Evidence] Duty roster, skill matrix, caregiving, medical ward, survivor, and survivor-detail panels already exist and are bound through the current player-surface wiring.

[Evidence] The duty panel already displays roster/sleep-related information, while no fitness verdict UI or reason projection was found.

[Inference] 24A UI should add a read model to the existing duty/survivor surfaces, showing verdict, reason, and whether a condition warns or blocks. It should not recompute fitness in the panel.

[Inference] 24B attribution belongs in the existing needs/social/producer read models. A new panel should be justified by an existing player decision; it should not become a second simulation view.

[Required verification] Touched panels need focus/back behavior, readable status at the fixed 1920×1080 target, expanded-text checks, and snapshot review. Shelved panels should be excluded unless the same player-facing text is live elsewhere.

## 12. Tests & Verification

Focused checks run during this assessment:

| Target | Result |
|---|---:|
| `DutyRosterIntegrationTests.cs` | PASS — 40/40 |
| `ApprenticeshipIntegrationTests.cs` | PASS — 3/3 |
| `CaregivingSystemTests.cs` | PASS — 28/28 |
| `Medical/DiseaseQuarantineCoordinatorTests.cs` | PASS — 20/20 |
| `SurvivorFateSystemTests.cs` | PASS — 25/25 |
| `SkillProgressionSystemTests.cs` | PASS — 12/12 |
| `SurvivorNeedsCharacterizationTests.cs` | FAIL — 14 passed, 1 failed |

[Evidence] The needs failure is `NeedsSystem_NotifyNeedsRestoredFiresAllEvents` at `Ashfall.Core.Tests/SurvivorNeedsCharacterizationTests.cs:161`: expected 7 events, actual 9. The current Core enum has nine needs, including `Numbness` and `RadiationAnxiety`.

[Inference] This failure is a baseline contract mismatch that must be triaged before 24B acceptance. It is not evidence that the proposed modifier stack is needed, and it should not be silently changed as part of a broad Plan 24 migration.

[Evidence] The plan lists several verification names (`ashfall-telemetry-playtest`, `ashfall-ui-access`, `ashfall-snapshot-diff`) that were not found as matching `scripts/ci` command files in the repository inventory. Existing repository scripts found include `scripts/run_test.sh`, `scripts/ci/verify-fast.sh`, and `scripts/ci/triad-drift-gate.sh`.

[Inference] The plan’s verification section needs to be rebased to repository-canonical commands and bounded focused targets before it can be accepted.

## 13. Duplicates / Legacy / Forks

[Evidence] Apprenticeship has a default/fallback constructor path that can create a second skill system, while the main production route already passes the campaign-shared authority.

[Evidence] Social, ration, thermal, caregiving, and Phase 0 paths already apply direct effects to Needs through callbacks or `Modify`. These are potential duplicate-application points for a new stack.

[Evidence] Trapping already receives assigned hunter identity for skill projection.

[Evidence] Quarantine already clears conflicting duty, and fate already clears duty, caregiving, ward admission, and expedition state on death.

[Inference] Plan 24’s largest integration risk is not absent functionality; it is two authorities applying the same effect. Every migrated effect needs a before/after ledger that proves the old write is removed or intentionally retained.

## 14. Existing Extension Seams

The following seams are suitable for bounded extension:

1. `DutyRosterAssignmentEngine.ValidateAssign` / `CanAssign` for role fitness checks.
2. Existing duty candidate eligibility and auto-assignment evaluation for the same verdict.
3. Existing Main day-owner ordering for one derived fitness refresh per simulation tick.
4. `ShelterScheduleSystem.TickDay` plus the existing Needs owner for a schedule recovery bridge.
5. `Main.SetupCaregiving` for wiring existing Core caregiving hooks.
6. `MedicalWardSystem.Admit` / `MedicalWardState.NormalizeAndValidate` for active-admission idempotence.
7. `DiseaseQuarantineCoordinator` for quarantine duty-release and discharge behavior.
8. `SurvivorFateSystem.RunCascade` for death cleanup and downstream events.
9. Existing shared skill setup and `ApprenticeshipHostSession` constructor contract for identity hardening.
10. Existing panels/read models for player-facing verdict and attribution.

[Inference] These seams are preferable to creating a new `SurvivorLedgerSystem` that owns all body, labor, medical, and social state.

## 15. Functional Equivalents

| Plan intent | Existing functional equivalent | Status |
|---|---|---|
| One skill authority | Shared campaign skill progression in Main | Mostly live; fallback path remains |
| Skill-driven trapping | Assigned hunter skill projection | Partly live |
| Quarantine releases labor | Quarantine coordinator clears duty assignment | Live for this path |
| Death releases labor/medical/care | Survivor fate cascade | Live, requires semantic review |
| Medical admission lifecycle | Medical ward + quarantine coordinator | Live, admission idempotence incomplete |
| Caregiving fatigue/recovery | Core hooks and tick logic | Core live, host hooks unbound |
| Schedule affects fatigue | Schedule computes recovery modifier | State live, Needs bridge absent |
| Needs persistence | Needs component store and survivor save route | Live |
| Fitness projection | None found | Missing |
| Modifier aggregation | None found | Missing |
| Role fitness data | None found | Missing |

## 16. Confirmed Gaps

These are supported by current source evidence and are appropriate candidates for implementation:

1. A pure Core fitness projection and reason model.
2. A validated source of role fitness requirements.
3. Duty assignment and auto-assignment consumption of fitness verdicts.
4. Correct sleep observation input for the duty/fitness calculation.
5. Caregiving host hook wiring through existing Needs/health/social owners.
6. Medical ward protection against duplicate active admissions for one patient.
7. A defined recovery/light-duty state and its owner, if current disease/medical state cannot express it.
8. A carefully scoped needs-effect aggregation seam, beginning with one pilot source.
9. Apprenticeship identity hardening so fallback construction cannot become a live duplicate authority.
10. Explicit attribution/read-model output for any newly routed effects.

[Inference] Death-quality ordering also warrants a separate repair review: `SurvivorFateSystem.RunCascade` clears caregiving and medical admission before it derives `DeathQuality`, while `DeriveDeathQuality` checks those authorities. This should be validated and repaired as a focused semantic issue, not hidden inside a general 24C rewrite.

## 17. Risks

| Risk | Evidence-based concern | Control |
|---|---|---|
| Duplicate needs effects | Many existing direct `Modify`/callback paths | Effect inventory, one pilot, no-double-application tests |
| Duplicate skill authority | Apprenticeship fallback constructs a new progression system | Require injected shared instance or explicit test-only factory |
| Stale sleep facts | Host snapshot marks every living occupant as slept | Use an authoritative sleep observation/state seam |
| Role taxonomy drift | Existing roles are code-defined and producer jobs differ | Decide role scope and data authority before implementation |
| Death semantic regression | Existing fate cleanup and memorial quality ordering are coupled | Focused fate tests before/after any 24C change |
| Save contamination | New derived state could be serialized | Recompute verdict; prove modifier reconstruction before persisting |
| Governance collision | Main/duty/shared paths are claim-protected | Foreman-owned package claim and explicit integrator ownership |
| Verification noise | Plan names commands not currently present | Replace with canonical repository commands |
| Baseline needs drift | Focused characterization test currently fails | Resolve/record contract before 24B acceptance |

## 18. Constraints for Planning

Before code implementation, the foreman should record decisions for:

- Exact role scope: the five duty roles only, producer jobs, expedition roles, or a shared labor taxonomy.
- Fitness thresholds and policy: warn versus block, including quarantine, incapacitation, precision work, medical staffing, and expedition departure.
- Fact source and polarity for hunger, thirst, fatigue, warmth, health, hygiene, disease, radiation, sleep, and age/cohort eligibility.
- Needs modifier semantics: flat value versus rate, cadence, ordering, clamping, attribution, and replacement/removal behavior.
- Whether schedule, caregiving, social, ration, thermal, and overwork effects are migrated together or in separate pilots.
- Recovery/light-duty ownership and whether current medical/disease data is sufficient.
- Caregiving capacity, duty conflict, and whether caregiving is a labor role or a separate assignment channel.
- Exact store/checksum invariance tests for any state touched by the change.

[Inference] Without these decisions, 24B and 24C are architecture-expansion packages rather than implementation-ready tasks.

## 19. Evidence Index

Primary plan anchors:

- `C-integration-plans/C1_planintegration[5].md:1-15` — Plan 24 scope, dependencies, order, and guardrails.
- `C-integration-plans/C1_planintegration[5].md:21-52` — source diagnosis and claimed current gaps.
- `C-integration-plans/C1_planintegration[5].md:224-252` — full-wave definition of done.
- `C-integration-plans/C1_planintegration[5].md:369-411` — proposed fitness model.
- `C-integration-plans/C1_planintegration[5].md:880-972` — proposed needs modifier stack.
- `C-integration-plans/C1_planintegration[5].md:1416-1518` — proposed illness/quarantine/recovery integration.
- `C-integration-plans/C1_planintegration[5].md:2448-2470` — proposed verification commands.

Current implementation anchors:

- `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs:42-174` — assignment validation and auto-assignment.
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:10-20,192-240` — current need model and direct simulation updates.
- `Assets/Ashfall.Core/Survivors/NeedsComponentStore.cs:108-131,225-250` — detached persistence boundary.
- `Assets/Ashfall.Core/ShelterScheduleSystem.cs:18-22,188-244` — schedule state and day tick.
- `src/Main.CampaignServices.cs:140-183` — shared skill authority.
- `src/Main.ShelterSocial.cs:494-505,523-536` — apprenticeship injection and caregiving setup.
- `src/Host/ApprenticeshipHostSession.cs:17-28` — duplicate fallback skill construction.
- `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs:64-82,248-276` — existing host hooks and tick effects.
- `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs:50-87,247-255` — admission and normalization behavior.
- `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs:166-225` — quarantine admission, duty release, and discharge.
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:259-334,413-430` — death cascade and death-quality derivation.
- `src/Main.DutyRoster.cs:126-147` — current home-occupant sleep snapshot.
- `Ashfall.Core.Tests/SurvivorNeedsCharacterizationTests.cs:152-161` — current focused needs baseline failure.

Governance anchors:

- `INTEGRATION_PLANS.md:1-30` — current batch acceptance state and completed Plan 22/19 entries.
- `WORKTREE_OWNERSHIP.md:1-40` — foreman/claim rules and active claim context.
- `TEST_POLICY.md` — focused verification and quarantine policy.

## 20. Confidence & Unknowns

**Confidence: High** for the central finding that Plan 24 cannot be integrated unchanged and that 24A has a real missing implementation center.

**Confidence: Medium-high** for the conclusion that 24B/24C are partially implemented, because the relevant Core and host seams were inspected and focused tests were run, but not every producer, UI panel, or data consumer was exhaustively audited.

Remaining unknowns requiring a foreman-owned P0 evidence freeze:

- Complete list of player-facing producer roles and their current worker identity contracts.
- Whether an existing hidden/partial fitness or light-duty implementation exists outside the searched paths.
- Exact authoritative sleep event/state source for `lastSleptDay`.
- Full direct-needs write inventory, including all host callbacks and data-authored effects.
- Intended medical recovery semantics and whether health recovery already has another owner.
- Whether current Plan 23 power/schedule acceptance is sufficient for Plan 24 dependency closure; Plan 23 documentation records remaining debt/open decisions.
- Exact command names for UI accessibility, snapshot, telemetry, and layout checks in the current repository.

## Recommended Integration Decision

Do **not** claim the full Plan 24 document as one implementation package.

Accept only a rebased sequence:

1. **P0 evidence freeze:** authority map, producer/role inventory, needs-effect inventory, and decision record.
2. **24A.1:** Core fitness projection, role requirement contract, validator, and focused tests.
3. **24A.2:** Duty roster and existing producer/expedition consumers use the projection; no new save section.
4. **24B.1:** remove or gate duplicate apprenticeship fallback construction and prove shared skill identity.
5. **24B.2:** pilot one typed needs modifier source with attribution and no-double-application tests before any broad migration.
6. **24C.1:** wire caregiving hooks and make ward admission idempotent.
7. **24C.2:** define and integrate recovery/light duty through existing medical/quarantine/fate owners, including the death-quality ordering review.
8. **Closure:** targeted balance, UI, accessibility, save, and determinism verification using repository-canonical commands.

[Evidence] This sequence preserves current authorities, addresses confirmed gaps, and respects the current governance requirement that a new package have an explicit claim and focused acceptance.
