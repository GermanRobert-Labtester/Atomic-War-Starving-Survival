# C1 Plan 24 — Implementation Log

User-authorized integrator package, 2026-09-16.

## Package contract

- Outcome: integrate the survivor fitness, needs-effect, worker-identity, medical-journey, caregiving, and death-adjacent seams against existing authorities.
- Non-goals: no survivor god-object, no parallel needs/duty/medical/death authority, no Unity restoration, no UI-side gameplay computation, no speculative producer rewrite.
- Policy: the existing five duty roles are the initial role set; thresholds and role requirements are data-authored; dead, incapacitated, and quarantined survivors are blocked; impaired survivors remain assignable with an explicit warning; derived fitness is not persisted.
- Shared paths: this package owns the listed Plan 24 seams as the user-authorized integrator. Unrelated existing edits are preserved.

## Phase 0 — Premise and baseline

Status: PASS

Evidence and baseline are recorded in `docs/forensics/plan24_survivor_ledger_FEASIBILITY_FORENSIC_REPORT.md`.

Focused baseline:

- Duty roster: 40/40 PASS.
- Apprenticeship: 3/3 PASS.
- Caregiving: 28/28 PASS.
- Disease quarantine coordinator: 20/20 PASS.
- Survivor fate: 25/25 PASS.
- Skill progression: 12/12 PASS.
- Needs characterization: 14/15 PASS; existing event-count contract expects 7 while the live nine-need model emits 9.

## Phase 24A — Fitness for duty

Status: IMPLEMENTED FOR CURRENT WORK CONTRACTS; WARD STAFFING SUBSCOPE —
DECISION MEMO PRESENTED (TASK A2, WAVE 8); LABOR CLUSTER PARTIALLY CLOSED

Changed:

- Added engine-free `FitnessForDutyModel`, stable reason IDs, base and
  role-specific verdicts, affected-need attribution, deterministic reason
  ordering, recommended hours, and confirmed-warning metadata.
- Added validated, data-authored thresholds and the five current duty-role
  contracts in `Assets/StreamingAssets/Data/duty_roles.json`. Thresholds cover
  needs, sleep recency, discharge recovery, active illness bands, and dose.
- Bound host facts to the existing survivor needs/radiation, dose ledger,
  disease/quarantine, illness sick-list, medical ward, dependency, trauma,
  duty sleep-day, and shared skill authorities. Fitness itself is not saved.
- Made the duty assignment engine re-evaluate fitness at preview and commit;
  hard blocks fail closed, impaired commits require explicit confirmation, and
  auto-assignment never silently accepts an impaired warning.
- Routed medical admission, quarantine/invalid-condition, caregiving, and death
  vacancy through existing roster/event owners. Added daily boundary
  invalidation for assignments that become role-ineligible.
- Bound role-aware fitness to expedition dispatch and kitchen cook admission;
  expedition dispatch now asks for explicit confirmation when impaired.
- Added fitness/reason readouts to duty, expedition, and survivor-detail
  surfaces; added the current authority map at
  `docs/systems/SURVIVOR_STATE_AUTHORITY_MATRIX.md`.
- Recent discharge uses the existing persisted ward discharge day as a
  data-authored two-day impaired projection; no new recovery save state was
  added.

Tests:

- `Plan24FitnessForDutyTests.cs`: 11/11 PASS, including stable repeated
  evaluation, sleep attribution, active illness-band escalation, discharge
  recovery, and catalog loading.
- `Plan24DutyRosterFitnessTests.cs`: 7/7 PASS, including hard blocks,
  deterministic auto-assignment, and explicit warning confirmation.
- `DutyRosterIntegrationTests.cs`: 40/40 PASS.
- `KitchenNutritionSystemTests.cs`: 12/12 PASS.
- `dotnet build Ashfall.csproj --no-restore`: PASS, 0 warnings/errors.
- `--data-integrity-selftest`: PASS, 332 catalogs, 0 errors, 5 existing
  primary-wins override warnings.
- `--ui-accessibility-selftest`: PASS, all 5 gates; Godot emitted resource/RID
  leak warnings at process shutdown after the successful test.

Result: The existing roster, expedition, kitchen, caregiving-eligibility, and
survivor-detail paths use the shared projection. The repository has no ward
staff/procedure-worker authority, so that planned consumer was not fabricated.
The roster panel is currently a read surface; explicit warning confirmation is
available through the host command API and Core commit contract, not a new
assignment UI.

Divergences: role scope is bounded to the current five actual duty roles. The
plan's examples did not define numeric defaults, so initial conservative
thresholds are authored in `duty_roles.json` and remain tunable. The
`intake_sleeper` role is the only currently authored light-duty exception.

### Task A2 (Wave 8) — duty-hour accumulator, overwork, skill-to-yield

CLOSED portions (2026-09-17):

- **Duty-hour accumulator** — `Assets/Ashfall.Core/DutyRoster/DutyHourLedger.cs`
  (new): a derived, never-persisted projection over the canonical assignment
  state (one full-day role per survivor — the model's actual granularity), the
  role's data-authored shift load (`maximum_hours`), and the live fitness
  verdict. Overwork = committed > recommended: the stale-assignment window
  (assigned fit at 12h, fitness recedes to an 8h recommendation, morning
  validation only vacates hard blocks). Stable reason id
  `duty_hours_overwork` (24A vocabulary pattern); deterministic ordinal
  ordering. Host-bound in `Main.SurvivorFitness.EnsureDutyHourLedger()` and
  surfaced read-only through `DutyRosterSystem.DutyHourResolver` /
  `DutyRosterHostSession.PreviewDutyHours`.
- **Measured overwork consequences** — data-authored in `duty_roles.json`'s
  additive `overwork` block (fatigue_per_excess_hour 0.5, morale_per_excess_hour
  −0.25, yield_penalty_permille 150; loader + CatalogIntegrityValidator range
  rules). The daily boundary (`Main.CampaignOwners` shelter + medical/disease
  owners, after assignment validation) routes per-day totals as per-hour rates
  through the shared needs modifier seam via `SetExternalModifier`
  (`overwork.fatigue` / `overwork.morale`, the A1 overwork source slot now
  live) — replace-on-Set, no accumulation, removed when the assignment no
  longer overworks. This is a flagged behavior addition (A1 documented
  overwork as absent); no balance tuning accompanied it.
- **Shared skill-to-yield contract** —
  `Assets/Ashfall.Core/Survivors/LaborProductivity.cs` (new):
  `WorkerProductivityContract` is the ONE worker-context seam (plan §24B.23 —
  no per-producer setters): the host binds the campaign skill authority, the
  fitness projection, and the overwork flag once; producers resolve
  (survivorId, skillId) → bounded permille verdict. Band data-authored in
  `duty_roles.json`'s additive `worker_yield` block (floor 750 / cap 1050 /
  impaired −100‰, matching the CVD operator precedent 0.75+0.25×skill;
  absolute floor 500). Unbound/unknown-worker resolves null ⇒ producer keeps
  exact legacy behavior (the parity path).
- **Producer wiring** (each a flagged behavior addition, bounded):
  kitchen — `CookProductivityResolver` bound from the mess role's authored
  `skill_iron_chef`; the cook's verdict stamps prep batches
  (`PrepJob.cookQualityPermille` / `PantryItem.qualityPermille`, additive save
  fields with legacy default 0 = unstamped): degraded batches waste portions
  (bounded 1..recipe) and scale the serve-time safe-meal chance (bounded
  [0.7, 0.98]; unstamped batches keep exactly 0.9); ingredient costs untouched
  (§24B.28 honored). Workshop — the craft-time leg resolves through a NEW
  composed slot `CraftingSystem.SetCrafterProductivityTimeMultiplier`
  (multiplies with — never replacing — the Phase0 penalty slot; bounded
  [0.8, 1.3]×) using the authored `skill_workshop_sense` crafting-discipline
  skill.
- **Assignment UI** — `DutyRosterPanel` is no longer read-only: candidate
  assignment actions, a VACATE action, and the impaired-warning confirmation
  dialog (warning reasons from the fitness model's own ids, explicit
  CONFIRM/CANCEL, exactly one confirmed mutation, commit-time revalidation,
  keyboard-operable buttons, text-first states). The Core contract's
  `fitness_warning_confirmation_required` block is now player-visible.

Producer rows NOT wired (premise-corrected, documented):

- **Greenhouse** — no worker identity exists (flat `BaseYield`, no assignment
  model); wiring requires a greenhouse worker-assignment design. Open row.
- **Workshop output/foundry** — the forging model is deliberately
  operator-free (Plan 213 sequence-fidelity quality, zero RNG); the workshop's
  legacy trait evaluator (`BindSkillEvaluator`) and Phase0 craft-time
  penalties already implement producer-local skill/fitness effects, and
  migrating them onto the campaign-skill contract would change their numeric
  basis — that migration needs its own parity evidence. The composed slot is
  shipped and tested, ready for it. Open row.
- **Medical procedures** — gated on the ward-staffing decision below (the
  ward invents no outcomes; §24B.27 says use duration/efficiency/warning
  metadata, no arbitrary rolls).

### WARD STAFFING DECISION MEMO (blocks the medical producer row — foreman signature required)

Implementation-selective options per the A2 contract:

| Option | Canonical owner | Data contract | Command/read-model boundary | Save impact | 24A fitness compatibility | UI consequence | Test consequence | Duty-role vocabulary |
|---|---|---|---|---|---|---|---|---|
| (a) medical-ward staffing requirements | `MedicalWardSystem` (extend) | additive `min_staff`/`staff_skill_id` fields on `MedicalProcedureDef` + bed/staff rows | `RunProcedure` preflights staffing; read model exposes coverage | procedure defs are data (no save); staffing state derivable | reuses the existing per-survivor verdicts for staff eligibility | ward panel shows coverage + blocked reasons | procedure staffing preflight tests | unchanged (staff are normal role assignees) |
| (b) ward duty-role contract | `DutyRosterSystem` (extend) | additive `ward` role row in `duty_roles.json` (thresholds data-authored, the 24A pattern) | admission/discharge preflight asks the roster for eligible ward staff | no new section (assignments already persist) | direct — the fitness engine already gates roles | roster panel gains a ward role column | role-gate tests via the existing battery | expands by one authored role |
| (c) defer staffing | — | — | — | — | — | ward procedures remain staffing-free | none | unchanged |

Recommendation: **(b)** — it is the 24A pattern (data-authored role contract
through the existing roster engine), needs no new authority, and makes ward
staffing visible on the duty surface. (a) is viable but forks a staffing
model beside the roster; (c) leaves the 24A subscope permanently open.

**AWAITING FOREMAN SIGNATURE — no staffing authority was fabricated.**

Divergences: the duty-hour accumulator reflects the model's actual
granularity (full-day role assignment, no minute-level precision). The
30-day policy simulation remains open (owned by Task A4's verification
battery).

## Phase 24B — Needs modifiers, sleep, morale, stress, and skill-to-work

Status: PARTIAL — SHARED MECHANISM, TWO PILOT SOURCES, THE NINE-FAMILY
SOURCE MIGRATION (TASK A1) AND THE LABOR CLUSTER CORE (TASK A2) COMPLETE;
GREENHOUSE/WORKSHOP-OUTPUT/MEDICAL PRODUCER ROWS + 30-DAY SIMULATION REMAIN
OPEN AS DOCUMENTED

Changed:

- Added deterministic per-survivor/source/need contributions with time windows,
  source refresh/removal, stable aggregation, top-contributor ranking, and
  recent applied attribution. The stack remains derived and is owned by
  `NeedsSystem`.
- Connected shelter sleep eligibility/recovery to fatigue through the shared
  needs modifier seam; the schedule remains the sleep authority.
- Routed caregiving fatigue and recovery adjustments through the existing
  needs owner with stable source IDs; caregiving also vacates a competing duty.
- Removed the apprenticeship host's private skill/roster/relations fallback;
  the campaign composer supplies the shared progression authority. The
  existing trapping path already projects the assigned hunter identity.
- Added bounded top/recent contributor display in survivor detail.
- Task A1 (Wave 8): migrated the nine stranded source families onto the
  shared attributed seam — grief (`SurvivorFateSystem`), thermal room warmth
  (`ShelterThermalSystem`), meal quality/unsafe meals
  (`KitchenNutritionSystem`), self-care hygiene withdrawal (host arc-behavior
  sink), leadership and ration-conflict morale (`SurvivorSocialCoordinator`
  sinks; `RationConflictSystem.OnMoraleDelta` now carries its cause's source
  id), and contagion stress (`MoraleContagionPorts.ApplyMoraleDelta` now
  carries isolation/pressure distinction; host binding routes through the
  seam). Numeric deltas and trigger authorities unchanged.

Tests:

- `Plan24NeedsModifierStackTests.cs`: 6/6 PASS.
- `Plan24NeedsSourceMigrationTests.cs`: 9/9 PASS (Task A1 parity +
  attribution).
- `CaregivingSystemTests.cs`: 28/28 PASS.
- `ApprenticeshipIntegrationTests.cs`: 3/3 PASS (baseline and shared authority
  route).
- `SkillProgressionSystemTests.cs`: 12/12 PASS (baseline authority target).

Result: The modifier mechanism is integrated, and the broad source migration
(the nine stranded families) is complete as of Wave 8 Task A1. The
labor-output portion is closed as of Task A2 (duty-hour accumulator, measured
overwork, the shared skill-to-yield contract with kitchen+workshop wiring,
and the assignment UI); the remaining producer rows (greenhouse,
workshop-output/foundry, medical) and the 30-day policy simulation are
documented open rows above and in Task A4's scope.

### Task A1 — nine-family needs-source migration (Wave 8)

Migration mechanism: `NeedsSystem.ApplyAttributedDelta` — the same clamped
`Modify` mutation the legacy direct path applied, plus stable source
attribution (recent-contribution record + `OnAttributedContribution`). No
persistent rates were added; the stack stays derived and unsaved. The domain
authority that decides WHEN each effect applies is unchanged; only the
mutation sink moved. Source granularity is cause-distinct where the family
has multiple causes (§24B.15):
`RationConflictSystem.OnMoraleDelta` and `MoraleContagionPorts.ApplyMoraleDelta`
now carry a source-id argument (signature extension; all binders updated).

| Family | Owner (unchanged) | Old direct call site | New attributed source id(s) |
|---|---|---|---|
| Grief | `SurvivorFateSystem` death cascade | `SurvivorFateSystem.cs` shelter-wide morale loop | `grief.shelter_loss` (−8, once per death) |
| Thermal/cold | `ShelterThermalSystem` daily tick | room-warmth propagation loop | `thermal.room_warmth` (+modifier×24/day) |
| Water/hygiene | `PsychologicalArcSystem` behavior sink (host) | `Main.Plans162_165.cs` arc binding | `hygiene.self_care_withdrawn` (+8) |
| Meal quality | `KitchenNutritionSystem.ServeMeal` | morale/health lines (hunger stays direct per §24B.10) | `kitchen.meal_quality` (±5), `kitchen.meal_unsafe` (−5 health) |
| Ration conflict | `RationConflictSystem` → coordinator sink | `SurvivorSocialCoordinator` morale wiring | `ration.confrontation` (−10/−5), `ration.theft` (−15) |
| Leadership | `LeadershipSystem` → coordinator sink | coordinator morale wiring | `leadership.crisis_aura` (+10), `leadership.morale` |
| General stress | `MoraleContagionSystem` ports → host sink | `Main.MoraleContagion.cs` port binding | `contagion.isolation` (+1/day), `contagion.pressure` (channel) |
| Missed meal | — | **absent**: no missed-meal effect exists (hunger decay models it; §24B.11 "if product data supports") | not fabricated |
| Ideological friction | `IdeologicalFrictionSystem` | **absent from needs**: affects relations/affinity only (§24B.17 "if authored") | not fabricated |
| Overwork | — | **absent**: no duty-hour need effect exists | not fabricated; owned by Task A2 |

Parity evidence: `Ashfall.Core.Tests/Survivors/Plan24NeedsSourceMigrationTests.cs`
(9/9) pins per family — the legacy numeric delta as the exact expected need
value, plus source id, target need, effective delta, and attribution presence;
the contagion test proves the attributed seam applies exactly the port's delta
stream (spy-sum parity). Stack integrity: migrated one-shot sources do not
persist stack rates.

Regression evidence (all green): Survivors 243/243 (incl. stack 6/6,
fate 25/25, coordinator, fitness 11/11); Flagship11 65/65 (contagion + smoke);
kitchen 12/12 + Plan22 kitchen 6/6 + culinary 2/2; ration 7/7; thermal suites
13+3+4+4+6+8; caregiving 28/28 + commands 3/3; apprenticeship 3/3; skill
progression 12/12; duty roster 40/40; campaign integration 30-day 4/4
(includes day-11 save/reload shock); Plan12D 21/21 + 12E 17/17 + 194 6/6 +
54_57 2/2 + 194_197 3/3 + apiculture 5/5; save checksums 24/24 + 27/27;
DeterminismSeedSweep 199/199; DeterminismGuard 2/2. Needs characterization
remains at the inherited 14/15 baseline (the seven-vs-nine restore assertion —
left to Task A4 unchanged). Zero save-section/DTO changes; data-integrity
selftest PASS (332 catalogs, 0 errors, 5 known warnings); build 0 errors /
0 warnings; fast gates `survivors_selftest`, `forbidden_core_apis`,
`compiler_warning_baseline`, `triad_drift` PASS; architecture map --check
PASS (193 subsystems, no drift).

Divergences: worker-hours and the shared skill-to-yield contract were closed
by Task A2 (see the 24A section's Task A2 block for the full evidence and the
remaining producer rows); water-outage hygiene decay (§24B.13) has no current
model and was not fabricated; the needs characterization 14/15 baseline is
inherited debt (Task A4).

### Task A2 verification evidence (Wave 8, 2026-09-17)

- `Ashfall.Core.Tests/DutyRoster/Plan24DutyHourTests.cs`: 10/10 PASS — ledger
  derivation (committed/recommended/overwork window), unassigned never
  overworked, deterministic ordinal overwork ordering, overwork rate totals
  through a zero-drift 24h tick + removal-on-resolution (no accumulation),
  contract unbound/unknown-skill ⇒ null (legacy parity), authored band at
  skill 0/100, impaired+overwork penalties with absolute-floor clamp, kitchen
  unstamped legacy parity (0.9 safe chance, 3 portions) vs stamped degraded
  batch (500‰ → 2 portions, safe chance clamped to 0.7 → unsafe), crafting
  productivity slot composed with the Phase0 penalty slot (2f × penalty /
  productivity, preview == actual path).
- Regression: DutyRoster 28/28; Survivors 243/243; crafting 14/14 + 20/20;
  kitchen 12/12 + Plan22 6/6 + culinary 2/2; comprehensive save suite
  1160/1160 (additive pantry/prep fields round-trip, no new section);
  ExpandedShelterSaveChecksum 24/24.
- `--data-integrity-selftest` PASS (332 catalogs, 0 errors — duty_roles.json
  additive blocks validated); `--panel-bind-lifecycle-selftest` PASS;
  `--ui-accessibility-selftest` PASS 5/5 (the roster panel's new interactive
  controls included); compiler-warning baseline PASS (one transient CS8602 in
  the new panel code fixed in-scope); triad drift PASS.
- Save rule: `PrepJob.cookQualityPermille` / `PantryItem.qualityPermille` are
  additive fields riding the kitchen state's existing JSON round-trip with
  legacy default 0 (unstamped ⇒ byte-identical legacy behavior). The ledger,
  the contract, and the overwork rates are derived — zero new save sections.

## Phase 24C — Illness, recovery, care, and death

Status: PARTIAL — JOURNEY TRANSITIONS WIRED; TASK A3 (WAVE 8) CLOSED THE
GRIEF-TO-NEEDS PROJECTION, THE MOURNING VIGIL, AND THE RATION RE-SPLIT
JOURNEY VERIFICATION; THE AFFLICTION-SPECIFIC RECOVERY RAMP IS A DESIGN NOTE
AWAITING SIGNATURE; WARD STAFFING REMAINS THE A2 MEMO

Changed:

- Made medical ward admission idempotent for an already-active patient and
  exposed one canonical ward-change signal.
- Wired ward admission/discharge to the host's day-event queue and briefing
  vocabulary; admission releases any active duty through the existing roster.
- Kept quarantine, fate/death, ration, memorial, and relationship authorities;
  used their current transition seams rather than adding a new survivor ledger.
- Added the bounded discharge-to-impaired recovery projection described in
  24A, based on the saved ward discharge day.
- Connected caregiving eligibility and fatigue/health effects through the
  campaign's existing needs and relationship owners.

Tests:

- `Plan24SurvivorJourneyTests.cs`: 3/3 PASS, including one admission and one
  discharge transition.
- `MedicalWardSystemTests.cs`: 13/13 PASS.
- `DiseaseQuarantineCoordinatorTests.cs`: 20/20 PASS (baseline/retained path).
- `DailyBriefingReportBuilderTests.cs`: 13/13 PASS.
- `SurvivorFateSystemTests.cs`: 25/25 PASS (baseline/retained cascade).

Result: Admission, quarantine, vacancy, discharge, and death continue through
their existing owners; complete illness-to-treatment-to-light-duty journeys
with mid-illness/recovery/death save-load parity have not been demonstrated.

Divergences (updated by Task A3, Wave 8, 2026-09-17):

### Task A3 closed portions

- **Grief-to-needs (the named fatigue/morale source)** — the production grief
  bridge (`RelationsGriefSink`, the one IGriefSink bound in the host) now
  stamps each affected relationship's `grief_since_day` (additive persisted
  field, legacy default −1) when a memorialized death applies relationship
  grief, and the host's daily projection (`Main.SurvivorFitness.
  ApplyGriefNeedsModifiers`, beside the overwork projection) derives each
  mourner's fatigue/morale rates from those persisted canonical facts alone:
  linear decay to zero across the data-authored 10-day window
  (`RelationsGriefSink.BondGriefDurationDays`; intensity = relationship grief
  0..100, which already absorbed the authored death-quality scale). Source id
  `grief.bond_loss`; replace-on-Set; expiry/removal handled at the boundary.
  Fully derived — a reload recomputes identical rates; no new save section.
- **Mourning vigil** — `MemorialSystem.Mourn(deceasedId, day)`: a bounded,
  once-per-death player command through the memorial owner (the entry's
  additive persisted `MournedDay` is the exactly-once gate; rejections are
  explicit results — `already_mourned`, `unknown_memorial`). The host
  subscription applies one attributed morale recovery (+3,
  `memorial.mourning` — restrained against the −8 shelter-wide grief hit) to
  each living survivor and one restrained journal line. Player route: the
  cenotaph panel's vigil button is now real (truthful memorial status, the
  most-recent-unmourned read model, result-text feedback); the panel's other
  fixture decor is pre-existing placeholder state, not claimed here.
- **Ration re-split + grievance journey (verification, not construction)** —
  `Plan24RecoveryGriefTests.
  DeathToRationResplitToGrievance_RunsThroughExistingOwners`: death through
  the real fate cascade → shelter-wide grief on the attributed seam → the
  next-day re-split from the reduced living roster through the social
  coordinator → the unequal-service grievance keeps building toward the
  leader through the existing ration-conflict authority. All owners unchanged.

### AFFLICTION RECOVERY-RAMP DESIGN NOTE (foreman signature required)

Source-verified at HEAD: the medical domain's recovery timing is already
data-authored per disease (`illness_days`, immunity windows) and per duty
contract (`discharge_recovery_days: 2` in `duty_roles.json`); the sick-list
bands release by authored day; admissions carry NO cause field; no authored
recovery-rate field sits unconsumed anywhere. A "per-affliction recovery
curve" therefore cannot be wired as a consumer — it requires new clinical
semantics, which is a product decision, not an unblock:

| Option | Shape | Cost | Consequence |
|---|---|---|---|
| (i) cause-tracked discharge ramp | admissions gain an additive `cause` field; `duty_roles.json` gains per-cause recovery windows the fitness projection reads | admission call sites + save field + data authoring | affliction-specific light-duty windows; legacy default preserves the uniform 2-day window |
| (ii) declare the uniform data-authored ramp complete | the existing `discharge_recovery_days` projection IS the ramp (uniform by authoring) | none | 24C's "affliction-specific" wording is marked satisfied-by-uniform-window (or deferred) by signature |
| (iii) defer | record the ramp as an open gate with owner | none | Plan 24 closes with the named deferral |

Recommendation: (i) if affliction-specific recovery is a design goal;
otherwise (iii). **No ramp authority was fabricated.**

### Task A3 verification evidence (Wave 8, 2026-09-17)

- `Ashfall.Core.Tests/Medical/Plan24RecoveryGriefTests.cs`: 6/6 PASS —
  onset stamping (legacy grief chain unchanged), per-pair onset with
  re-anchor semantics, the pure decay function (full/half/zero/expired),
  reload parity (pure function of persisted facts), mourning exactly-once
  with explicit rejections, and the full death → re-split → grievance journey.
- Regression: memorial/relations/fate/social suites green (see handoff);
  needs characterization stays at the inherited 14/15 baseline.

Divergences: there is no ward staffing roster (A2's decision memo) and no
affliction-specific recovery-ramp owner (the design note above); duty-hour
accounting closed under Task A2. Grief now reaches a named fatigue/morale
modifier source. The complete death/admission ration re-split and
unequal-service grievance journey is verified through the existing owners.

## Closure

Status: CLOSED-WITH-DEFERRALS — PENDING TWO FOREMAN SIGNATURES (Wave 8,
2026-09-17). Closeout document: `docs/plans/PLAN_24_CLOSEOUT.md`.

Every originally open acceptance item now has a terminal state with evidence:

- **Closed-with-evidence (Tasks A1–A4):** nine-family needs-source migration;
  duty-hour accumulator + measured overwork + the shared skill-to-yield
  contract (kitchen/workshop wired; greenhouse/foundry-output/medical rows
  documented with named blockers); the roster assignment UI with the
  impaired-warning confirmation flow; grief-to-needs (bond-scaled, decaying,
  reload-safe); the mourning vigil (once-per-death, attributed, routed on the
  cenotaph); the death → ration re-split → grievance journey; full save/load
  journey parity (treatment + death/grief, continuous vs interrupted); the
  30-day policy simulation (same-seed identity + day-15 mid-reload suffix
  identity + per-day impossible-state exclusions); the needs-characterization
  baseline repaired to 15/15 (nine distinct restore emissions — model proven,
  not test-weakened).
- **Open decision items (signature queue):** ward staffing (A2 memo: options
  a/b/c; recommendation b) and the affliction-specific recovery ramp (A3
  design note: options i/ii/iii). Neither authority was fabricated; both are
  presented in this log for the foreman's signature. On signature, option
  (c)/(iii) makes them signed deferrals and this status becomes CLOSED; the
  other options become new implementation packages.
- **Environment-blocked (documented):** snapshot review — the headless
  renderer cannot capture SubViewport images in this environment; no golden
  snapshots were regenerated without review, and the intended render changes
  are enumerated in the closeout for the first renderer-capable rebaseline.
- **Known non-Plan-24 debt routed:** the Godot shutdown resource/RID warning
  (reproduced during this wave's a11y selftest) is routed to Task D3.

Final Wave 8 evidence: all Plan 24 suites green; sanctioned aggregate
`verify-fast.sh` **ALL 47 GATES PASSED CLEANLY**; build 0/0; data-integrity
PASS (332 catalogs, 0 errors); content-utilization PASS; panel lifecycle PASS;
a11y 5/5. Run targeted tests per `TEST_POLICY.md`; the aggregate run is the
wave's sanctioned close, not a default.
