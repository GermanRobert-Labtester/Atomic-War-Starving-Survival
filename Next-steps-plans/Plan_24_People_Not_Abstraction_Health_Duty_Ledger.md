# Plan 24 — People Are Not Abstraction: Health, Fatigue, Skill, and Duty Are One Ledger

> **Wave:** Continuity Wave 2 — *The Bunker Machine* (closing plan)
> **Depends on:** 22 (meals and meds are the payload), 23 (light/heat/sleep are loads),
> 20 (dose is what overwork costs you). Plan 16B's authority fix is a hard prerequisite for 24B.
>
> **Theme:** the roster and the bodies are separate facts. You can assign a starving, irradiated,
> quarantined survivor to the night shift and the game will not object — because the duty roster
> never asks the needs system anything. Fatigue has a schedule model with a recovery modifier that
> only a label prints. Trapping has a skill input that nobody feeds. And one host session is still
> constructing its own copy of the skill system. The survivors are the game; right now they are
> decoration around a spreadsheet.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | **Duty assignment ignores condition entirely** | `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs:59–74 ValidateAssign` checks only: known role → non-empty id → row exists → `CanAssign(row)` → not already assigned elsewhere. `grep -niE "health\|afflict\|sick\|quarantin\|injur\|fatigue\|needs" Assets/Ashfall.Core/DutyRoster/*.cs` → **0 matches in the whole folder** |
| 2 | The roster row has no condition to check | `DutyRosterSystem.cs:19–27 DutyRosterRow { survivorId, displayName, occupationObserved, status, script, lastSleptDay }` — `lastSleptDay` is tracked and **never read** by anything |
| 3 | Needs are a closed system with exactly one external input | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:73,84` — only `Func<SurvivorNeedsState,bool> _isNearHeatSource` is injectable; `NeedKind` = Hunger, Thirst, **Fatigue**, Warmth, Morale, Health, **Hygiene** (`:9–18`), yet no work-load, sleep-quality, or meal modifier can reach the tick |
| 4 | A full sleep/schedule model exists and changes nothing | `ShelterScheduleSystem.cs:10–19` — `fatigueRecoveryModifier`, `lightingDemand`, `curfewActive`, `emergencyOverride`, `List<SleepAssignment>`; the only external reader of the recovery modifier is a label: `src/UI/ShelterSchedulePanel.cs:113` prints `Fatigue Recovery Rate: {…:P0}`. Host session at `src/Main.ShelterInfrastructure.cs:112` |
| 5 | Illness can't reach labour | `SickListSystem` is exposed only inside the dose ledger session (`src/Host/DoseLedgerHostSession.cs:22`) and read by dose surfaces; `DiseaseSystem` quarantine events exist (`OnQuarantineStarted/Ended`, wired to audio in 7B) — no consumer removes a quarantined survivor from a shift |
| 6 | The sick/ward loop is closed inside itself | `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` models "patient/staff assignments, procedure definitions, supply costs" (`:11`) but has no roster or needs seam; discharge returns nobody to duty |
| 7 | **Skill can't reach production** | `WildlifeTrappingSystem.cs:109–116` — `SetHunterSkill(float)` with `SkillMultiplier => 0.5f + (skill/100)*1.0f`, plus per-quarry `minSkillLevel` (`:65`). `grep -rn "SetHunterSkill"` → **only the declaration**: trapping runs forever at the 0.5× floor and the skill gates are dead |
| 8 | A skill system is being duplicated again | `src/Host/ApprenticeshipHostSession.cs:20` — `var skills = new SkillProgressionSystem();` (fresh instance, not the campaign authority — the same defect class Plan 16B fixes for panels) |
| 9 | Skill loss is modelled and invisible | `SkillAtrophySystem` is live inside `SurvivorSocialCoordinator` (`Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs:103`) alongside `LeadershipSystem`, `IdeologicalFrictionSystem`, `RationConflictSystem`, `TraumaBondSystem` — all reachable only through the coordinator's properties; `skill_matrix` panel binds a **new** `SkillProgressionSystem` (`src/Main.PlayerSurfaces.cs`) |
| 10 | Morale is fed by things, starved of the obvious ones | morale channels exist (decor/schedule/vinyl per Wave-1 notes; `KitchenNutritionSystem` serves with a `nutrition` score) but overwork, hunger, cold, and a colleague's death have no morale path into needs (NeedsSystem has no morale modifier hook — row 3) |
| 11 | Guilt already connects mind to body | `docs/ASHFALL_EXPANSION_CONTEXT_ATLAS.md` §11 lists *Guilt Records & Insomnia* as **High-Leverage State** — "Written: Moral Choices/Deaths · Read: `GuiltInsomniaSystem`, `NeedsSystem`", i.e. the one existing bridge from psychology into needs. It is the template for everything else here |
| 12 | The medical depth just landed | recent commits: `feat(memorial): Plan 09 9C Core — DeathQuality + MemorialOutcome + IGriefSink`, `feat(disease): IDiseaseOutbreakSource port + TriggerOutbreak`, `feat(dependency): OnStressReported API + relapse rules table` + detox items/taper recipe content. The pieces exist; the labour loop does not use them |

---

## Task 24A — Fitness for duty: the roster must ask the bodies

**Goal:** one shared "can this person work, and at what cost" verdict, consumed by the roster, the
expedition party picker, the kitchen, and the ward — computed in Core from live state.

**Files:** new `Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs`,
`DutyRoster/DutyRosterAssignmentEngine.cs:59–74`, `DutyRosterSystem.cs:19–27`,
`Expeditions/ExpeditionSystem.cs` (party validation), `Medical/SickListSystem.cs`,
`Medical/DiseaseSystem.cs`, `Survivors/NeedsSystem.cs`, `AfflictionContracts.cs`,
`src/UI/DutyRosterPanel.cs`, `src/UI/ExpeditionPanel.cs`, role data JSON.

### Substeps

1. **Author the verdict DTO** in Core: `FitnessVerdict { survivorId, level, blockingReasons[],
   degradedFactors[], recommendedMaxHours, affectedNeeds[] }` with an enum level
   (`Fit / Impaired / Unfit / Incapacitated`). No UI text in Core — reasons are ids.
2. **Compute from the authorities that already exist**: `NeedsSystem` (hunger/thirst/fatigue/
   warmth/health/hygiene), `RadiationSystem` (dose + ARS phase), afflictions
   (`AfflictionContracts`), `DiseaseSystem` quarantine state, `SickListSystem` bands,
   `ChemicalDependencySystem` withdrawal, `CombatTraumaSystem`, and `SurvivorNeedsState.IsAlive`.
3. **Inject it as a collaborator, not a re-implementation**:
   `DutyRosterAssignmentEngine` takes an optional `Func<string, FitnessVerdict>` (same
   constructor-injection idiom as `RadiationSystem`'s delegates) so the roster stays testable and
   Core stays engine-free.
4. **Extend `ValidateAssign`** with the new blocking codes alongside the existing
   `unknown_role` / `unknown_survivor` / `cannot_assign` / `already_assigned`, each with its own
   message key so the UI can say *why* in the game's voice.
5. **Warn, don't always forbid**: `Impaired` must be assignable with an explicit consequence
   preview ("she will work, and she will make mistakes") — the genre's whole point is choosing to
   spend people. `Incapacitated` (dead, quarantined, unconscious) blocks.
6. **Role requirements in data**: per-role minimums (dose ceiling for hot work, fatigue ceiling for
   precision work, skill floors) authored in the duty-roster/role catalog, validated by
   `CatalogIntegrityValidator`, so designers add roles without adding code.
7. **Same verdict for expeditions**: `ExpeditionSystem`'s party validation must call the identical
   model — two definitions of "fit enough" is how the roster and the field diverge.
8. **Same verdict for the kitchen and the ward**: cooks (`22B`) and nursing staff (`caregiving`)
   consume the verdict; a sick cook ruins a meal, an exhausted nurse botches a procedure.
9. **Make `lastSleptDay` mean something**: read it in the verdict (row 2) so "hasn't slept in three
   days" is a blocking/degrading factor rather than a dead field.
10. **Quarantine must remove people from shifts** — and, when a shift is abandoned mid-campaign,
    raise an event so production notices (emit `duty_vacated`, 17A vocabulary).
11. **Surface it honestly in the UI**: duty roster + expedition party screens show per-person
    condition with the reason list; colour is reinforced by words and numbers
    (`ashfall-ui-access`), and assignments that ignore the warning require confirmation and are
    recorded.
12. **Tests**: verdict per factor, boundary tests per level, block-vs-warn matrix, role-requirement
    data resolution, save round-trip (the verdict must be derivable from persisted state alone),
    determinism, and one integration test: assign a starving survivor to precision work → mistake
    → consequence.
13. **Run the checklist** + triad gate.

**DoD:** you cannot assign a corpse to the night shift, and you *can* assign a dying woman to it —
on purpose, with the cost shown.

---

## Task 24B — Fatigue, sleep, and morale: the needs loop has to be steerable from outside

**Goal:** give `NeedsSystem` the modifier seams it lacks (row 3), so schedule, meals, cold,
darkness, grief, and overwork all move the same numbers the player is watching.

**Files:** `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`, `NeedsProfile`,
`ShelterScheduleSystem.cs`, `KitchenNutritionSystem.cs`, `ShelterThermalSystem.cs`,
`PowerGridSystem.cs` (via 23), `GuiltInsomniaSystem.cs`, `Memorial/*` + `DeathQuality`,
`TraumaBondSystem.cs`, `src/Main.SurvivorSocial.cs`, needs data JSON.

### Substeps

1. **Design the seam once**: a `NeedsModifierStack` in Core — contributors register
   `{ need, amount, source, window }`, the tick aggregates them deterministically (ordinal by
   source id, documented), and every contributor can be blamed later. Keep the existing
   `_isNearHeatSource` behaviour intact as a special case or fold it in deliberately.
2. **Sleep restores fatigue through the schedule**, not by fiat: `fatigueRecoveryModifier` and
   `SleepAssignment` become real inputs (row 4), with `lightingDemand` and curfew affecting sleep
   quality and therefore next-day fitness (24A reads it).
3. **Wire `lastSleptDay`** into the sleep path so a missed shift or a night alarm shows up as
   fatigue, closing row 2's dead field.
4. **Meals apply morale and health modifiers** with quality tiers from `KitchenNutritionSystem`'s
   `nutrition` score (22B), and *skipping* a meal is a distinct, visible event.
5. **Cold and hygiene**: unheated rooms and no water raise hygiene/fatigue penalties (needs already
   include `Hygiene`, which nothing currently worsens except base decay) — route 23's thermal
   failures here.
6. **Grief into morale**: memorial/`DeathQuality`/`IGriefSink` (just landed, Plan 09 9C) plus
   `TraumaBondSystem` must apply morale/fatigue penalties when someone dies — the atlas already
   treats guilt as a body-affecting state (row 11); extend the same shape to grief.
7. **Leadership and friction reach output**: `LeadershipSystem`,
   `IdeologicalFrictionSystem`, `RationConflictSystem` (all live behind
   `SurvivorSocialCoordinator`) register modifiers on the stack, so camp politics has a
   production consequence instead of being flavour text.
8. **Stress reaches dependency**: `ChemicalDependencySystem.OnStressReported` (Plan 09 9B) is the
   consumer for overwork/quarantine/grief stress — call it from the new emitters rather than
   inventing a second stress track.
9. **Use the pattern that already works**: `RadiationSystem`'s constructor delegates and
   `SurvivorsHostSession`'s `_fallback*` behaviour (H1's resolution) are the house style for
   host→Core inputs.
10. **Fix the duplicate skill authority**: `src/Host/ApprenticeshipHostSession.cs:20`'s
    `new SkillProgressionSystem()` must take the campaign instance (same defect as Plan 16B; do it
    here because this is where skills become labour).
11. **Feed skill into labour**: `WildlifeTrappingSystem.SetHunterSkill` (row 7) is called with the
    assigned worker's actual skill; add the equivalent for greenhouse, workshop/foundry, medical,
    and cooking through one shared "who is doing this work" parameter on the day owners — do **not**
    add per-system setters beyond the one already present.
12. **Legibility**: needs panels show the top contributors ("fatigue: +2/night — no sleep 3 days,
    night shift"), reusing 17A's attribution. A modifier stack that can't be explained is just a
    hidden spreadsheet.
13. **Balance**: `ashfall-telemetry-playtest` on a 30-day run — fatigue pressure must create
    rest/rotation decisions, not a death spiral; record the curves.
14. **Tests**: stack aggregation determinism, per-contributor tests (sleep, meal, cold, grief,
    friction), blame/attribution test, save round-trip (stack is derived, never saved — assert
    that), and skill→yield per producer.
15. **Run the checklist.**

**DoD:** seven outside forces visibly move the six need bars, and every movement is attributable.

---

## Task 24C — A life spent: injury, illness, recovery, and death as roster events

**Goal:** make what happens to a body *after* the bad day matter — ward → sick list → light duty →
back to work, or to the memorial — with the labour and ration accounting that implies.

**Files:** `Medical/MedicalWardSystem.cs`, `Medical/AfflictionContracts.cs`,
`Medical/DiagnosisKnowledgeStore.cs`, `SickListSystem.cs`, `DiseaseSystem.cs`,
`SurvivorFateSystem.cs`, `Memorial/*`, `CaregivingSystem` (locate via coordinator),
`DutyRosterSystem.cs`, `RationConflictSystem.cs`, `src/UI/MedicalWardPanel.cs`,
`src/UI/CaregivingPanel.cs`.

### Substeps

1. **Map the patient journey as it exists today** (admission → bed class → procedures → discharge
   → fate → memorial) and mark each seam that currently ends inside its own panel. Publish the map
   before writing code.
2. **Admission vacates the shift**: ward admission and quarantine both trigger 24A's verdict
   change and emit `duty_vacated`, so production notices without a scripted event.
3. **Discharge returns them to duty** — as `Impaired`, not `Fit`: the recovery ramp is authored
   per affliction so "back on the roster" is a real, imperfect outcome.
4. **Light duty as a first-class role set**: define reduced-hours roles (data, not code) that
   impaired survivors can hold, so the sick aren't either exploited or erased.
5. **The sick eat too**: sick-list and convalescent rations route through 22's single consumption
   authority, and unequal shares feed `RationConflictSystem` grievances — the health/economy/social
   triangle in one mechanism.
6. **Caregiving is a duty**: nursing consumes roster hours and produces fatigue on the carer
   (24B), so care competes with production. That is the genre's actual decision.
7. **Diagnosis uncertainty**: `DiagnosisKnowledgeStore` gates what the player is told — an
   undiagnosed affliction still has effects, and the autopsy chain (`AutopsySystem`) is how the
   player learns. Keep the information economy honest rather than hiding mechanics.
8. **Death is a roster and economy event**: `SurvivorFateSystem.OnSurvivorFate` + `OnDied` already
   exist (`src/Host/SurvivorsHostSession.cs:111` subscribes); extend the handler to release the
   shift, re-split rations, apply memorial morale (24B), and write the memorial line with
   `DeathQuality`.
9. **Funerals cost something**: a day of mourning as an authored effect (morale up, hours lost) —
   an optional response, not a forced modal.
10. **Emissions into 17A** for `admitted`, `discharged`, `quarantined`, `died`, `mourned`,
    so the briefing narrates the human cost rather than a bar value.
11. **Audio parity**: reuse existing `med_*` cues (quarantine seal/clear and `med_survivor_death`
    are already wired) — no new family; confirm no double-fire with 16C.
12. **Tests**: journey transitions, light-duty hours, ration re-split arithmetic, carer fatigue,
    death releasing obligations, save round-trip mid-illness, determinism of a 30-day illness run,
    and one snapshot per changed panel.
13. **Docs**: `docs/systems/SURVIVOR_JOURNEY_OWNERSHIP.md` naming the single authority for each
    stage — and correct `AGENTS.md` while you're there: **H5** (Utility AI forked between Core and
    the Godot host) is resolved — `src/UtilityAI/` now contains only `UtilityAiPanel.cs`; **H11**
    (JournalSystem untested) is resolved — `Ashfall.Core.Tests/JournalSystemTests.cs` and
    `JournalSystemCoreBehaviorTests.cs` exist. Stale rows are how agents re-fix dead bugs.
14. **Run the checklist** + `verify-fast.sh` as the wave close.

**DoD:** a survivor's illness and death reorganise the shift table, the ration list, and the mood
of the shelter — and the player reads all three in the briefing.

---

## Cross-Task Dependencies

```
16B (panels on campaign authority) ──► 24B step 10 (same defect, host sessions)
22  (meals/meds)   ──► 24B steps 4,7 │ 24C step 5
23  (light/heat)   ──► 24B steps 2,5 │
20  (dose)         ──► 24A step 2    │
                    ┌────────────────┴───────────────┐
24A (fitness verdict) ──► 24B (modifiers) ──► 24C (journey) ──► Wave 3 (people as story)
        └──────────────────────────────────────► 24C step 2/3
```

**Execution order:** 24A → 24B → 24C. 24A is the contract the other two consume; landing 24B first
would mean wiring modifiers into a system that still can't refuse an impossible shift.

**Wave-2 overall order:** 20A → 20B → 22A → 21A → 23A (with 20B/22B) → 22B → 21B → 23B → 24A →
24B → 21C → 22C → 20C → 23C → 24C. If capacity is short, the three highest-value tasks in this
wave are **22A, 24A, 23A** — eating works, the roster stops lying, and the machine needs watts.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --survivors-selftest             # needs/dose probes
7. bash scripts/ci/triad-drift-gate.sh
8. ashfall-telemetry-playtest (30-day fatigue/morale/mortality KPIs)
9. ashfall-ui-access + ashfall-snapshot-diff on changed panels
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 24A | 1 new + 2 | 1 | 1 (roles) | 2 | 12–15 | Medium | MEDIUM (assignment UX friction if too strict) |
| 24B | 1 new + 4 | 2 | 1 | 2 | 14–18 | **High** (touches every need) | **HIGH — balance-wide** |
| 24C | 3–4 | 2 | 1 | 2 | 12–16 | Medium–High | MEDIUM |

**Guardrails:** no new need kinds, no second morale track, no new stress model, no per-system skill
setters beyond the existing one, no scripted tragedy events — the cascade must come from state.
And keep the tone: cold, exhausted, human, restrained. A survivor spent on a shift should read as a
decision, never as a stat line or a lecture.
