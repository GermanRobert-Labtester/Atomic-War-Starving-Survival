# PLAYER-FACING TRIAD B — EXERCISE · DREAM · SANITATION — INTEGRATION PLAN

**Package ID:** `PFGL-TRIAD-B-2026-09-25`
**Evidence HEAD:** `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1`
**Save pin (evidence):** `266` — this program adds **no** save sections
**Mode:** Planning only until approval + claims in `WORKTREE_OWNERSHIP.md`
**Revision:** R1 — authored with live-verified contracts, pre-integration gate, seal-quality framework (2026-09-25)
**Family:** sibling of `PLAYER_FACING_GAMEPLAY_LOOPS_MASTER_INTEGRATION_PLAN.md` (R5) and `PLAYER_FACING_REALTIME_COMBAT_PHYSICS_AI_INTEGRATION_PLAN.md`
**Production code changed by this document:** none

---

## 0. Executive summary

Triad B covers three **host-sealed, player-thin** survivor-wellbeing loops whose Core authorities, host sessions, save sections, day owners, and CLI probes already exist (Plans **216 Exercise**, **177 Dream**, **210 Sanitation** — all SEALED/COMPLETE), but whose **mechanics do not reach the player and whose results do not reach the game**:

1. **Exercise** — players cannot train anyone (no panel), workout **fatigue cost is computed then dropped** (`WorkoutResult.FatigueIncurred` has zero `src/` consumers), and conditioning never feeds fatigue economy (`GetFatigueResistanceMultiplier` is called only by the CLI probe).
2. **Dream** — dreams generate nightly and Morale/stress deltas are ported, but the **rest bonus is dropped** (`EffectiveRestBonus` is CLI-only), there is **no journal/interpretation surface**, and the nightmare-interpretation relief is a **dead consequence**: `InterpretDream` mutates `record.trauma_delta/morale_delta` *after* the tick already applied the originals, and `OnDreamInterpreted` has **zero subscribers**.
3. **Sanitation** — hygiene bands and pathogen exposure already feed the disease tick, but the panel is **read-only** and the host session exposes **no player verbs** (`ApplyCleaning` / `InstallFacility` exist in Core and are unreachable).

### Three packages (dependency order)

| # | Package ID | Mechanic focus | One-line outcome |
|---|---|---|---|
| W1 | `PFGL-TRIAD-B-W1-EXERCISE-COMPOSITION-BOARD` | Training board + fatigue economy + conditioning composition | Players can assign/execute routines; workouts cost real Fatigue; conditioning measurably resists fatigue |
| W2 | `PFGL-TRIAD-B-W2-DREAM-JOURNAL-INTERPRET` | Dream journal + interpretation desk + rest port | Players read last night's dreams, interpret nightmares for real relief; dream rest quality reaches Fatigue/Morale |
| W3 | `PFGL-TRIAD-B-W3-SANITATION-OPS` | Cleaning + facility ops | Players assign cleaners, install facilities, answer spills; hygiene is an operated system, not a scoreboard |

**Hard gate:** W2's interpretation-relief seam (F5) needs one signed decision — proposed name `DEC-TRIADB-DREAM-RELIEF` — choosing late-delta application vs record re-evaluation, because both naive paths double-apply or lose the relief on save/load. Everything else in this program is unblocked EXPAND/WIRE work.

**Parallel-safety:** claims are disjoint from the real-time combat tetrad per its own coordination note (“Triad B owns sanitation/exercise/dream — disjoint if claims honored”).

---

## 1. Objective

Make Triad B **player-operable and mechanically real** by extending the three existing owners — never by adding parallel ledgers:

1. A training surface where players pick survivors and routines and see gains, streaks, injury, and **felt fatigue cost**.
2. A dream journal where players read last night’s dreams, track nightmare streaks, and interpret dreams with **real psychological consequence**.
3. A sanitation ops surface where players spend labor on cleaning, install facilities from the authored catalog, and resolve spills **before** pathogen exposure bites.

### Non-goals (program-wide)

- Re-opening sealed Plan 216 / 177 / 210 host wiring (Setup/Save/Reset/day owners stay as shipped).
- New Core authorities: no `FitnessManagerSystem`, no `DreamJournalSystem`, no `HygieneManagerSystem`.
- Parallel wellbeing ledgers — Fatigue/Morale/stress/hygiene stay on `NeedsSystem`, the mental-health owner, and derived `SanitationSystem` state respectively.
- Changing dream generation math, exercise gain math, or hygiene band thresholds (balance patches are follow-ups, not this program).
- Full-suite-as-default testing; Unity; mass refactors; shared-hub edits without claims.
- Bedroom/dormitory presentation polish (see PFGL W10 Sleep Acoustic for sleep environment work).
- Wildland/mobile-clinic/public-works greenfield hypotheses.

---

## 2. Current reality (evidence 2026-09-25, HEAD `1678c074`)

### 2.1 Architecture

| Layer | Path | Role |
|---|---|---|
| Core | `Assets/Ashfall.Core/` | Engine-free authority (netstandard2.1) |
| Host | `src/` | Thin Godot adapters, panels, CLI (net8.0) |
| Data | `Assets/StreamingAssets/Data/` | JSON catalogs (snake_case, schema-gated) |
| Tests | `Ashfall.Core.Tests/` | Focused xUnit |

### 2.2 Reachability matrix (measured this revision)

| Concern | Core owner | Host | Main | Save key | Day owner | CLI | Player surface | Class |
|---|---|---|---|---|---|---|---|---|
| Exercise | `Assets/Ashfall.Core/Survivors/ExerciseSystem.cs` | `src/Host/ExerciseHostSession.cs` | `src/Main.Exercise.cs` | `exercise` (`SaveSectionRegistry.cs:318,608`) | `exercise` phase 5 (`CampaignOwners.cs:188`) | `--exercise-selftest` (`HostCliRegistry.cs:1315`) | **none** | HOST-OK / UI MISSING + composition gap |
| Dream | `Assets/Ashfall.Core/Survivors/DreamSystem.cs` | `src/Host/DreamHostSession.cs` | `src/Main.DreamSystem.cs` (+ `src/Main.SleepNarrative.cs`) | `survivor_dreams` (`:314,604`) | `survivor_dreams` phase 5 (`:180`) | `--dream-system-selftest` / `--dreams-selftest` (`:951`) | **none** | HOST-OK / UI MISSING + dead seam (F5) |
| Sanitation | `Assets/Ashfall.Core/Shelter/SanitationSystem.cs` (+ `SanitationConsequenceRules.cs`, `SanitationFacilityCatalog.cs`) | `src/Host/SanitationHostSession.cs` | `src/Main.Sanitation.cs` | `sanitation` (`:69,378`) | `hygiene` phase 3 (`:1373`, before disease tick) | **none** | `sanitation` panel — **ReadOnlyObservational** | HOST-OK / UI THIN + verb gap |

All three save sections are **already inside pin 266**. This program expects **zero** `SaveSectionRegistry` changes.

### 2.3 Catalogs (authored, live)

| Catalog | Path | Notes |
|---|---|---|
| Exercise routines | `Assets/StreamingAssets/Data/exercise_routines.json` | `ExerciseRoutineDefinition` rows: `routine_type`, `duration_hours`, `intensity`, `required_equipment`, `min_fitness_level`, per-attribute gains, `fatigue_cost`, `injury_chance_base` |
| Dream templates | `Assets/StreamingAssets/Data/dream_templates.json` | Loaded by `SetupSurvivorDreams` via `CatalogPath.ResolveCatalog` |
| Sanitation facilities | `Assets/StreamingAssets/Data/sanitation_facilities.json` | 9 facilities; closed waste-type + room-tag vocabularies (Plan 210 closeout) |

### 2.4 What is already right (do not redo)

- Determinism: dream tick forks `_campaignDay.Rng.Fork(CampaignStreamIds.Psychology, day, 77)` (`Main.DreamSystem.cs:86-89`); exercise injury rolls accept `ISeededRng`; no `System.Random` found on these paths.
- Dream tick already ports consequences: `TraumaDelta → mental.AddStress/ReduceStress("dream.trauma")` and `MoraleDelta → Needs.Modify(Morale)` (`Main.DreamSystem.cs:100-110`).
- Sanitation already feeds gameplay: `GetPathogenExposureModifier(roomId)` is consumed by the disease tick (`CampaignOwners.cs:1441`) and hygiene events journal `GetShelterHygienePermille()` (`:1409`).
- Panels bind host sessions, not Core (`SanitationPanel : IBindablePanel` binds `SanitationHostSession`).
- Save/restore + pre-day snapshot restore exist on all three day owners (`IPreDaySnapshotRestore`).

---

## 3. Forensic findings (verified this revision — the gap set)

| ID | Severity | Finding | Evidence | Package |
|---|---|---|---|---|
| **F1** | **P0 mechanic** | Workout fatigue cost is computed then dropped: `WorkoutResult.FatigueIncurred` (e.g. `12f * intensity`, `20f * intensity`) has **zero** `src/` consumers | `rg -n 'FatigueIncurred' src` → no matches outside Core | W1 |
| **F2** | **P0 mechanic** | Conditioning is inert: `GetFatigueResistanceMultiplier(survivorId)` is consumed **only** by `HostCli.Exercise.cs:108` | `rg -n 'GetFatigueResistanceMultiplier' src` | W1 |
| **F3** | **P1 feature** | No exercise/fitness surface exists | `player_surface_manifest.json` has no exercise row; `src/UI` has no exercise panel | W1 |
| **F4** | **P0 mechanic** | Dream rest quality is dropped: `DreamSleepResult.EffectiveRestBonus` is consumed only by CLI checks; `TickSurvivorDreams` ports Trauma/Morale but not the rest bonus | `Main.DreamSystem.cs:100-110`; `HostCli.DreamSystem.cs:65,105,118` | W2 |
| **F5** | **P0 mechanic (dead seam)** | Nightmare-interpretation relief never lands: `InterpretDream` mutates `record.trauma_delta -= 3 / morale_delta += 5` **after** the tick already applied the originals, and `OnDreamInterpreted` / `OnDreamInterpretedSeam` have **zero subscribers** | `DreamSystem.cs:243-266`; `rg -n 'OnDreamInterpreted' src` → no consumers | W2 |
| **F6** | **P1 feature** | No dream journal/interpretation surface | no dream panel; `InterpretSurvivorDream` called only from CLI | W2 |
| **F7** | **P0 feature** | Sanitation host exposes no player verbs: `ApplyCleaning(roomId, workers, skill01, priority, day)` and `InstallFacility(facilityId, roomId)` exist in Core but not on `SanitationHostSession` (only `TickDay`/`StatusLine`/capture) | `SanitationHostSession.cs` verb sweep | W3 |
| **F8** | **P1 feature** | `SanitationPanel` is read-only (hygiene band + room list; zero buttons) and manifest classifies `sanitation` ReadOnlyObservational | `src/UI/SanitationPanel.cs`; manifest row | W3 |
| **F9** | **P2 observability** | Sanitation has no CLI probe (exercise and dreams do) | `HostCliRegistry.cs` sweep | W3 (optional) |

This is the classic family defect class: **host-complete / player-thin**, plus a second class unique to Triad B — **computed-then-dropped** (F1, F4) and **written-then-unread** (F5) seams that make good math invisible to the player.

---

## 4. Required delta (per system)

### 4.1 Exercise → W1
1. Interactive training board (or survivor-detail extension) calling Main → host → `ExecuteRoutine` / `ExecuteWorkout`.
2. **Fatigue cost port:** on successful workout, apply `WorkoutResult.FatigueIncurred` to the existing Needs owner (`Needs.Modify(id, NeedKind.Fatigue, +incurred)` — higher=worse) via a thin adapter; one-shot per command, attributed (`ApplyAttributedDelta(..., attribute: "exercise")` when available).
3. **Conditioning composition:** route `GetFatigueResistanceMultiplier` into the **existing** fatigue accrual path (candidate seams: `SurvivorsNeedsDayOwner` phase-3 tick or `NeedsPerformanceDayOwner` phase-5 — PIR-4 picks one owner and documents it). The multiplier **modulates a rate**; it is never stored.
4. Readouts: profiles (Cardio/Strength/Flexibility/Endurance, streak, last workout), routine catalog, workout results incl. injury reason.

### 4.2 Dream → W2
1. Dream journal surface: last night’s `DreamRecord`s (type, summary, deltas), history per survivor, consecutive-nightmare count.
2. Interpretation desk: `InterpretSurvivorDream(survivorId, recordId, interpretationChoice)` from UI; one-shot per record (Core enforces `is_interpreted`).
3. **Rest port:** apply `EffectiveRestBonus` into the existing rest/fatigue owners on tick (exact target decided in W2 P1 — candidates: Fatigue via `Needs.Modify` or the sleep-quality path used by PFGL W10; **no second rest ledger**).
4. **Relief seam (DEC-gated):** make interpretation relief real without double-apply — see §6.3.

### 4.3 Sanitation → W3
1. Host verbs on `SanitationHostSession` mirroring Core: `ApplyCleaning`, `InstallFacility`, plus read `FindRoom`/`GetRoomBurden`/spill state for the board.
2. Panel promote to InteractiveCommands: assign workers to clean (workers/skill/priority), install facility from `sanitation_facilities.json` (inventory-closed), respond to active spills (`CleaningPriority.Critical` resolves a spill — `SanitationSystem.cs:529-538`).
3. Keep hygiene derived (never stored) — the board displays `GetShelterHygienePermille`/band/room burdens only.
4. Optional `--sanitation-selftest` CLI probe (F9) for package evidence.

---

## 5. Architecture (one authority per concern)

```
[Training board]  → Main.TrainSurvivor → ExerciseHostSession.ExecuteRoutine/ExecuteWorkout
                        → ExerciseSystem (sole fitness authority)
                            → WorkoutResult → fatigue-cost adapter → NeedsSystem (existing Fatigue)
                            → FitnessProfile persisted in `exercise` section
[Dream journal]   → Main.InterpretSurvivorDream → DreamHostSession.InterpretDream
[Night tick]      → SurvivorDreamsDayOwner → Main.TickSurvivorDreams (forked rng)
                        → DreamSystem.ProcessSleepCycle
                            → TraumaDelta → mental-health owner (existing)
                            → MoraleDelta → NeedsSystem (existing Morale)
                            → EffectiveRestBonus → rest/fatigue owner (NEW thin adapter — F4)
                            → relief (interpretation) → same owners via DEC-gated seam — F5
[Sanitation ops]  → Main commands → SanitationHostSession.ApplyCleaning/InstallFacility (NEW verbs)
                        → SanitationSystem (sole waste/hygiene authority)
                            → hygiene stays derived; pathogen modifier flows to disease tick (existing)
```

**Rules (non-negotiable):**
1. Panels call Main/host only; they never compute chances, costs, or eligibility offline.
2. Every consequence lands in an **existing** owner (`NeedsSystem`, mental-health, disease tick). Adapters are thin and attributed; they never store.
3. Facades save nothing; all three sections keep their current `CaptureState`/`RestoreState` shapes (schema-gated restore, missing section → defaults, never crash).
4. Hygiene, conditioning, and rest quality remain **derived/read models** — no stored duplicates.
5. RNG stays forked (`CampaignStreamIds.Psychology` for dreams; exercise injury rolls receive the forked rng from the host seam); never `System.Random`, never wall-clock seeds.

### Completion chain (PLAYER-OPERABLE, not CLI-complete)

`DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → MUTATES → OBSERVED → PERSISTED → RESTORED → VERIFIED → PLAYER-OPERABLE`

CLI selftest alone stops at VERIFIED. Every package in this program must reach PLAYER-OPERABLE.

### Collision map (resolve before coding)

| Concern | Extend | Forbidden parallel | DEC? |
|---|---|---|---|
| Fitness | `ExerciseSystem` | second conditioning store; needs-side fitness cache | No |
| Workout fatigue | `NeedsSystem` Fatigue | storing FatigueIncurred in `ExerciseSystemState` | No |
| Conditioning → fatigue | existing fatigue accrual owner | a `FatigueResistanceSystem` | No (one-owner pick documented) |
| Dream records | `DreamSystem` | a journal-side dream ledger | No |
| Dream rest | existing rest/fatigue owner | `DreamRestSystem` | No |
| Interpretation relief | mental-health + Needs via one seam | applying relief twice (record + owners) | **YES** (`DEC-TRIADB-DREAM-RELIEF`) |
| Waste/hygiene | `SanitationSystem` | second hygiene counter in UI/needs | No |
| Cleaning labor | survivors roster / duty owner (read) | spending workers without roster truth | No |
| Facilities | `SanitationFacilityCatalog` | runtime facility invention | No |
| Spill events | `SanitationSystem.activeSpill` | parallel incident queue | No |

---

## 6. API / contracts (all signatures verified live at HEAD `1678c074`)

> **Binding rule:** verbs below were re-verified against live source this revision (§12). `(core)` = Core owner, `(host)` = host session, `(main)` = Main wrapper. If live code drifts by claim time, **live code wins** — reconcile in a revision note (Rule 7); never invent bypass wrappers.

### 6.1 Exercise — `Assets/Ashfall.Core/Survivors/ExerciseSystem.cs` / `src/Host/ExerciseHostSession.cs`
| Verb (live) | Signature / role |
|---|---|
| `ExecuteRoutine` | `(host) WorkoutResult? ExecuteRoutine(survivorId, routineId, currentDay, float intensityMultiplier = 1.0f)` — catalog-driven |
| `ExecuteWorkout` | `(host) WorkoutResult ExecuteWorkout(survivorId, WorkoutRoutineType routine, currentDay, float intensity = 1.0f)` — typed path; `ISeededRng?` available on Core for injury rolls |
| `GetOrCreateProfile` / `TryGetProfile` | `FitnessProfile(survivorId, initialBase = 30f)` — Cardio/Strength/Flexibility/Endurance + `OverallConditioning` |
| `GetAvailableRoutines` / `GetRoutine` | catalog reads (`ExerciseRoutineDefinition`) |
| `TickDay(currentDay)` | streak reset after >2 inactive days; deconditioning after >3 (`DeconditioningRatePerDay`, floor `BaselineFitnessFloor`); `OnDeconditioned` |
| `GetFatigueResistanceMultiplier(survivorId)` | **F2 target** — conditioning → fatigue resistance |
| `GetCensus` / `CaptureState` / `RestoreState` | `ExerciseCensus` / `ExerciseSystemState` |

`WorkoutResult`: `CardioGain`, `StrengthGain`, `FlexibilityGain`, `EnduranceGain`, `FatigueIncurred` (**F1 target**), `InjuryOccurred`, `InjuryReason`.
`WorkoutRoutineType`: `Calisthenics` | `CardioDrill` | `StrengthTraining` | `FlexibilityStretching` | `CombatDrill`.

### 6.2 Dream — `Assets/Ashfall.Core/Survivors/DreamSystem.cs` / `src/Host/DreamHostSession.cs` / `src/Main.DreamSystem.cs`
| Verb (live) | Signature / role |
|---|---|
| `ProcessSleepCycle` | `(host) DreamSleepResult ProcessSleepCycle(survivorId, trauma, morale, currentDay, ISeededRng rng, bool forceDream = false)` — called per alive survivor by `TickSurvivorDreams` |
| `InterpretDream` | `(core) bool InterpretDream(survivorId, recordId, interpretationChoice)` — one-shot (`is_interpreted`); nightmare relief currently mutates the record (**F5**) |
| `GetDreamHistory` / `GetConsecutiveNightmares` | journal reads |
| `GetAllTemplates` / `GetTemplate` / `RegisterTemplate` | catalog |
| `GetCensus` / `CaptureState` / `RestoreState` | `DreamCensus` / `DreamSystemState` |
| Main wrappers (live) | `ProcessSurvivorDreamCycle` · `InterpretSurvivorDream` · `GetSurvivorDreamHistory` · `GetSurvivorConsecutiveNightmares` · `TickSurvivorDreams(day)` · `GetDreamCensus` |

`DreamSleepResult`: `HadDream`, `Record`, `EffectiveRestBonus` (**F4 target**), `TraumaDelta`, `MoraleDelta`, `Summary`.
Tick inputs (do not change): trauma proxy = mental-health `stressPermille / 10`; morale = `Needs.Get(id).Morale`.

### 6.3 Dream relief seam (the one DEC in this program)
`InterpretDream` today retro-edits `record.trauma_delta/morale_delta` for nightmares **after** `TickSurvivorDreams` already applied them — the edit reaches no owner. Two sound designs:

| Option | Mechanism | Save/restore | Double-apply guard |
|---|---|---|---|
| **A. Late-delta apply (recommended)** | Keep record immutable after apply; at interpret time apply `Δ = (-3 stress-scaled, +5 morale)` directly to mental-health + Needs through the same attributed adapter used by the tick | relief is owner state (already saved); record keeps `is_interpreted` as the one-shot guard | `is_interpreted` flag is the guard; adapter is one call site |
| **B. Re-evaluation** | Let the tick re-apply a record’s full deltas when `is_interpreted` changes, diffing against an `applied` marker | needs an `applied` marker in the record (schema change) | marker required; higher complexity |

`DEC-TRIADB-DREAM-RELIEF` must pick one. W2 will not implement relief before the signature. (Option A avoids a `DreamRecord` schema change — aligned with §5 rule 3.)

### 6.4 Sanitation — `Assets/Ashfall.Core/Shelter/SanitationSystem.cs` / `src/Host/SanitationHostSession.cs`
| Verb (live core) | Signature / role |
|---|---|
| `ApplyCleaning` | `CleaningResult ApplyCleaning(roomId, int workers, float skill01, CleaningPriority priority, int day)` — priorities `Normal` / `High` / `Critical`; Critical can resolve `activeSpill`; radioactive waste is never hand-cleaned |
| `InstallFacility` | `InstalledFacilityState? InstallFacility(facilityId, roomId)` — catalog-gated |
| `EmitWaste` | `bool EmitWaste(roomId, WasteType, float amount)` — game-driven emission (not a player verb) |
| `EnsureRoom` / `FindRoom` / `GetRoomBurden` | room state reads (`RoomWasteState`: organic/chemical/… ) |
| `GetRoomHygienePermille` / `GetShelterHygienePermille` / `GetShelterHygieneBand` / `BandForPermille` | derived hygiene (**never stored**) |
| `GetPathogenExposureModifier(roomId)` | disease-tick coupling (live) |
| `TickDaily(int day, int population)` | daily waste/compost/spill evolution (host `TickDay`) |
| `CaptureState` / `RestoreState` | `SanitationState` |

Host today: `Create(dataDir, system?)` · `TickDay(day, population)` · `StatusLine()` · `CaptureSave`/`RestoreSave`. **W3 adds the player verbs** mirroring Core one-for-one (`ApplyCleaning`, `InstallFacility`) — no new semantics in the host.

---

## 7. Save / determinism / state rules

1. **No new sections, no pin bump.** `exercise`, `survivor_dreams`, `sanitation` already live in pin 266; adapters store nothing.
2. Schema-gated restore on all three; old saves → defaults, never crash.
3. Derived presentation (hygiene bands, conditioning readouts, journal groupings) is never saved.
4. Determinism: dream generation stays on the `CampaignStreamIds.Psychology` fork; exercise injury rolls must receive a forked rng from the day/command seam; sanitation tick is input-deterministic. No `System.Random`, no wall-clock seeds (guard test style per `ashfall-determinism-guard`).
5. Idempotency: fatigue-cost application is one-shot per workout command (no day-port guard needed); dream relief is one-shot per record (`is_interpreted`); conditioning multiplier is a modulator, never stored.

---

## 8. Wave packages

### W1 `PFGL-TRIAD-B-W1-EXERCISE-COMPOSITION-BOARD` — Training board + fatigue economy + conditioning composition

**Class:** EXPAND + MECHANIC · **Depends on:** none · **Priority:** P1

**Player value:** A daily training loop with real trade-offs — workouts consume Fatigue (F1), injuries happen on seeded rolls, and conditioning pays off by resisting fatigue (F2). Composes with PFGL W9 routines (training blocks), W2 recruitment (new recruits start unfit), and duty assignment (fit survivors endure shifts).

**Required delta:** §4.1 items 1–4.

**Ownership:** Domain `ExerciseSystem`; host `ExerciseHostSession`; Main `src/Main.Exercise.cs` + new thin wrappers; save `exercise` (unchanged); Needs writes only through `NeedsSystem` adapters; roster/duty reads only.

**Phases:**
| Phase | Work | Exit gate |
|---|---|---|
| P0 Premise | Re-rg F1–F3; paste live signatures into claim; claim UI hubs + Needs adapter files in `WORKTREE_OWNERSHIP.md` | PIR record complete (§9) |
| P1 Ports | Fatigue-cost adapter (attributed) + conditioning-multiplier seam pick (one fatigue accrual owner, documented); polarity notes (Fatigue higher=worse) | one-authority sign-off |
| P2 Main commands | `TrainSurvivor`, board snapshot DTO, journal facts (`exercise.completed`, `exercise.injured`) | compile + existing selftest green |
| P3 UI | Training board or Detail extension; manifest InteractiveCommands entry; accessibility rows | route opens; commands mutate Core |
| P4 Verify | `--exercise-selftest`; focused port tests (fatigue rises after workout; multiplier lowers accrual on paired ticks); manual script | DoD |

**Failure modes:** workout on dead/unknown survivor → reject; double-tap submit → one-shot per command with idempotent UI disable; missing catalog rows → catalog-list-only UI; port owner missing → fail-closed skip + journal fact; injury rng absent → deterministic fallback documented.

**Tests (focused):** fatigued-after-workout polarity unit; conditioning-multiplier paired-tick unit (same inputs, different conditioning → different accrual); persistence round-trip unchanged; host command mutation test; `--exercise-selftest` stays 12/12-style green; no full suite.

**Files (impact map):**
| Action | File | Risk |
|---|---|---|
| MODIFY | `src/Host/ExerciseHostSession.cs` (optional thin verb aliases only) | LOW |
| MODIFY | `src/Main.Exercise.cs` (commands, adapters, journal) | MED |
| CREATE | `src/UI/TrainingBoardPanel.cs` **or** extend `src/UI/SurvivorDetailPanel.cs` | MED |
| MODIFY | `src/UI/PanelRegistryBootstrap.cs`, `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs`, `docs/player_surface_manifest.json` (only if new route) | HIGH (shared hubs — claim) |
| MODIFY | fatigue accrual owner file (exact path from P1; e.g. `src/Main.CampaignOwners.cs` needs owner) | MED |
| **Do not touch** | `ExerciseSystem` math, `SaveSectionRegistry`, dream/sanitation paths | — |

**Rollback:** revert UI + Main adapters; Core/host/save untouched.

**DoD:** player assigns + completes a routine from UI · Fatigue visibly rises (F1 sealed) · conditioning visibly moderates accrual (F2 sealed) · injury path shows reason · save/load round-trips · selftest green · manifest updated · claims released.

---

### W2 `PFGL-TRIAD-B-W2-DREAM-JOURNAL-INTERPRET` — Dream journal + interpretation desk + rest port

**Class:** EXPAND + MECHANIC · **Depends on:** `DEC-TRIADB-DREAM-RELIEF` (interpretation relief only; journal + rest port unblocked) · **Priority:** P1

**Player value:** Night becomes legible and actionable — read what survivors dreamed, watch nightmare streaks compound, and interpret nightmares for measurable psychological relief. Composes with PFGL W9 sleep scheduling and W10 sleep acoustics (dreams + environment decide how the night felt).

**Required delta:** §4.2 items 1–4.

**Ownership:** Domain `DreamSystem`; host `DreamHostSession`; Main `src/Main.DreamSystem.cs`; save `survivor_dreams` (unchanged); stress writes to the mental-health owner; morale/rest writes to `NeedsSystem`; `src/Main.SleepNarrative.cs` prose stays the journal voice.

**Phases:**
| Phase | Work | Exit gate |
|---|---|---|
| P0 Premise | Re-rg F4–F6; confirm zero `OnDreamInterpreted` subscribers; claim hubs | PIR record complete |
| P1 Rest port | `EffectiveRestBonus` → existing rest/fatigue owner (attributed; polarity: positive bonus improves) | one-authority sign-off |
| P2 Journal UI | Dream journal + history + nightmare streak readout; manifest entry | route opens; reads Core truth |
| P3 Interpret | Interpretation desk bound to `InterpretSurvivorDream`; relief implemented **per signed DEC** | consequence observable |
| P4 Verify | `--dream-system-selftest`; focused rest-port + relief tests (interpretation of nightmare moves stress/morale exactly once); manual script | DoD |

**Failure modes:** interpret unknown/already-interpreted record → `false` + UI refresh; relief applied twice on save/load → guarded by `is_interpreted` (option A); dream tick before roster/needs setup → fail-closed skip (existing behavior); empty catalog → template-less generation must not throw (existing).

**Tests (focused):** rest-bonus polarity unit (peaceful > 0 improves; nightmare streak < 0 worsens); relief one-shot unit under option A; save/load does not re-apply relief; `--dream-system-selftest` green; manual script.

**Files (impact map):**
| Action | File | Risk |
|---|---|---|
| MODIFY | `src/Main.DreamSystem.cs` (rest port + relief adapter) | MED |
| CREATE | `src/UI/DreamJournalPanel.cs` (or SurvivorDetail extension) | MED |
| MODIFY | UI registry hubs + manifest (if new route) | HIGH (claim) |
| MODIFY (post-DEC only) | `Assets/Ashfall.Core/Survivors/DreamSystem.cs` **only if option B** | MED |
| **Do not touch** | `SaveSectionRegistry`, dream generation math, `SleepAcoustic` paths | — |

**Rollback:** revert UI + adapters; Core unchanged (option A) or revert the guarded Core delta (option B).

**DoD:** player reads last night’s dreams from UI · interprets a nightmare and **sees relief land once** (F5 sealed) · rest bonus measurably improves/worsens rest outcomes (F4 sealed) · save/load safe · selftest green · manifest updated.

---

### W3 `PFGL-TRIAD-B-W3-SANITATION-OPS` — Cleaning + facility operations

**Class:** EXPAND (host verbs) + FEATURE→MECHANIC · **Depends on:** none · **Priority:** P2 (mechanic is already load-bearing via pathogen exposure — operability is the seal)

**Player value:** Hygiene becomes a managed operation: assign limited worker-hours to rooms (cleaning effort `workers × skill01 × priorityMult`), install authored facilities, and answer spills with `Critical` priority before pathogen exposure feeds disease. Labor spent cleaning is labor not spent elsewhere — the trade-off is the loop.

**Required delta:** §4.3 items 1–4.

**Ownership:** Domain `SanitationSystem`; host `SanitationHostSession` (verbs mirror Core); Main `src/Main.Sanitation.cs` commands; save `sanitation` (unchanged); labor sourced from roster truth (read-only query + fail-closed); disease coupling untouched.

**Phases:**
| Phase | Work | Exit gate |
|---|---|---|
| P0 Premise | Re-rg F7–F9; confirm panel binding + `hygiene` day-owner order (phase 3, before disease tick); claim hubs | PIR record complete |
| P1 Host verbs | `ApplyCleaning` / `InstallFacility` on host (thin mirrors) + read models for board | compile + capture/restore unchanged |
| P2 Panel ops | Promote `SanitationPanel` to InteractiveCommands: clean-room (workers/skill/priority), install facility, spill response; manifest update | commands mutate Core |
| P3 Observability | Journal facts (`sanitation.cleaned`, `sanitation.facility_installed`, `sanitation.spill_resolved`); optional `--sanitation-selftest` | evidence path |
| P4 Verify | focused cleaning-effort + spill-resolution tests; manual script; hygiene band readout unchanged in truthfulness | DoD |

**Failure modes:** unknown room/facility → reject with reason (`unknown_room`, catalog-list-only UI); zero workers → `no_workers` surfaced; cleaning without roster truth → fail-closed; radioactive waste expectations → UI copy must state sealed-waste rule (never hand-cleaned); concurrent spill + clean → Critical path resolves exactly one spill.

**Tests (focused):** cleaning effort math from Core stays authoritative (UI never computes); facility install catalog-gated; spill resolution one-shot; save/load round-trip; optional CLI probe.

**Files (impact map):**
| Action | File | Risk |
|---|---|---|
| MODIFY | `src/Host/SanitationHostSession.cs` (verbs) | LOW |
| MODIFY | `src/Main.Sanitation.cs` (commands, journal) | MED |
| MODIFY | `src/UI/SanitationPanel.cs` (interactive promotion) | MED |
| CREATE (optional) | `src/Host/HostCli.Sanitation.cs` selftest | LOW |
| MODIFY | manifest row `sanitation` → InteractiveCommands | LOW |
| **Do not touch** | `SanitationSystem` math/thresholds, `SaveSectionRegistry`, disease tick coupling | — |

**Rollback:** revert host verbs + panel; Core untouched.

**DoD:** player cleans a room and sees burden/hygiene move · installs a catalog facility · resolves a spill via Critical cleaning · hygiene remains derived · save/load round-trips · claims released.

---

## 9. Pre-Integration Readiness Gate (PIR) — smooth, precise integration before code

No wave code lands before its PIR record (one row per gate, pasted into the claim/handoff) passes. A failed row stops the wave — no “fix while integrating”.

| # | Gate | Evidence required | On fail |
|---|---|---|---|
| PIR-1 | Evidence freshness | `git rev-parse HEAD` recorded; if HEAD moved past `1678c074`, re-run the §12 probes for the wave | re-verify verb tables |
| PIR-2 | Ownership | exact paths claimed in `WORKTREE_OWNERSHIP.md`; shared hubs free or mutex-held; never claim two hub-holding waves at once | wait / hand to integrator |
| PIR-3 | API copy | paste live signatures from the owner file into the claim; divergence from §6 → revision note first (Rule 7) | reconcile before code |
| PIR-4 | One-owner pick | for every adapter, the receiving owner is named (needs fatigue accrual owner; rest owner; mental-health owner; disease tick read-only) | redesign |
| PIR-5 | Save plan | confirmed: reuse-only; adapters persist nothing; restore paths untouched | redesign |
| PIR-6 | Determinism plan | rng streams named (`CampaignStreamIds.Psychology` fork for dreams; forked rng for injury rolls); guards named (`is_interpreted`; one-shot commands) | redesign |
| PIR-7 | Test plan | focused target files named; new files run alone first; `scripts/run_test.sh` under TEST_POLICY caps | narrow scope |
| PIR-8 | UI plan | bind-vs-new-route decided; if new route → panel + `PanelRegistryBootstrap` + `Main.GameFlow` + `Main.PlayerSurfaces` + manifest all listed; keyboard/controller close-back + focus + contrast rows included | split wave |
| PIR-9 | Baseline green | pre-change compile + existing focused tests/selftest pass before edits | establish baseline |
| PIR-10 | Rollback slice | first commit slice small and reversible; rollback section executable as written | shrink slice |

**Per-wave hot spots:** W1 → PIR-4 (which fatigue accrual owner absorbs the multiplier) and PIR-6 (injury rng); W2 → PIR-3/6 around `DEC-TRIADB-DREAM-RELIEF` (relief seam); W3 → PIR-2 (panel + manifest are shared-hub class) and roster-labor fail-closed design.

**Pre-integration dry run (last step, UI waves):** ① dotnet build Core+host → ② focused tests → ③ CLI selftest (`godot --headless --path . -- --exercise-selftest` / `--dream-system-selftest`; 15 FPS for manual sessions) → ④ panel route gates if UI touched → ⑤ save round-trip → ⑥ manual player script.

---

## 10. Seal quality framework — gap / feature / mechanic

| Tier | Meaning | Required evidence |
|---|---|---|
| **GAP SEAL** | host-invisible authority becomes reachable (session + save + day owner + probe agree) | host_files ≥ 1 · save round-trip · selftest · chain to VERIFIED |
| **FEATURE SEAL** | player-operable in a normal session on the **right** owner | + interactive surface · UI-bound mutation test · manifest entry |
| **MECHANIC SEAL** | the loop changes decisions: pressure, consequence in existing owners, counterplay | + consequence port/event · polarity/determinism test · manual cause→effect proof |

**Meaningfulness pentad** (all five for FEATURE; + counterplay for MECHANIC): **Agency** (player initiates) · **Legibility** (state readable) · **Consequence** (Core-observable) · **Persistence** (save/load) · **Feedback** (journal/readout) · **Counterplay** (pressure can be acted against).

**Declared tiers:** W1 **MECHANIC** (fatigue cost + conditioning counterplay) · W2 **MECHANIC** (nightmare pressure, interpretation counterplay, rest consequence) · W3 **FEATURE → MECHANIC** (MECHANIC once cleaning-labor trade-off is demonstrable in the manual script — pathogen exposure already supplies pressure and counterplay).

**Seal checklists:**
- Gap: session constructed in Setup · section captured/restored · day owner wired · probe green · port contract classified.
- Feature: surface registered · every control maps to a §6 verb · UI shows Core truth only · manifest updated · manual script passes.
- Mechanic: consequence lands in an existing owner · idempotency + clamps documented · polarity/determinism focused test · counterplay proven · no parallel ledger (`rg` assertion).

**Anti-patterns that fail the seal:** CLI-complete sold as player-operable · read-only panel counted as a board · offline UI math · shadow wellbeing store · journal prose as sole proof of effect · **computed-then-dropped results** (F1/F4 class) · **written-then-unread mutations** (F5 class).

---

## 11. Integration smoothness contract

1. **Landing order per wave:** Core gaps (only if proven) → host verbs → Main commands/adapters → registries → UI → verify. Never UI before the command exists.
2. One seam per commit slice; compile + focused tests green between slices.
3. Shared hubs (`PanelRegistryBootstrap`, `Main.GameFlow`, `Main.PlayerSurfaces`, `docs/player_surface_manifest.json`, `HostCliRegistry.cs`) edited in one owned burst per wave; never two hub-holding waves in parallel.
4. Fail-closed everywhere: unknown ids reject with reason; missing owners skip with a journal fact; no swallowed exceptions.
5. Port-contract classification of new public seams in the same change; 0 unbound HOST_REQUIRED at exit.
6. Exit re-assertion: `rg` shows no second fitness/dream/hygiene ledger; adapters store nothing.

---

## 12. Live verification log (this revision)

| Probe | Result | Implication |
|---|---|---|
| `rg -l` host refs for all three types | Exercise 3 files · Dream 5 files · Sanitation 4 files | all host-sealed; class is player-thin |
| save sections `exercise`/`survivor_dreams`/`sanitation` | rows + filenames live (`SaveSectionRegistry.cs:69,314,318,378,604,608`) | no pin work |
| day owners | `exercise` phase 5 · `survivor_dreams` phase 5 · `hygiene` phase 3 (before disease tick) | wiring complete; do not re-order |
| CLI probes | `--exercise-selftest` · `--dream-system-selftest`/`--dreams-selftest` · none for sanitation | F9 confirmed |
| `FatigueIncurred` consumers | zero in `src/` | **F1 confirmed** |
| `GetFatigueResistanceMultiplier` consumers | `HostCli.Exercise.cs:108` only | **F2 confirmed** |
| exercise/dream panels | none in `src/UI` or manifest | **F3/F6 confirmed** |
| `EffectiveRestBonus` consumers | `HostCli.DreamSystem.cs` only | **F4 confirmed** |
| `OnDreamInterpreted` subscribers | zero outside `DreamSystem`/`DreamHostSession` | **F5 confirmed** |
| `TickSurvivorDreams` port review | TraumaDelta→stress, MoraleDelta→Needs applied; rest bonus absent | F4 precise scope |
| `InterpretDream` semantics | one-shot via `is_interpreted`; nightmare relief retro-edits record | F5 mechanism documented |
| sanitation host verbs | `TickDay`/`StatusLine`/capture only | **F7 confirmed** |
| `SanitationPanel` | binds `SanitationHostSession`; zero buttons | **F8 confirmed** |
| `GetPathogenExposureModifier` | consumed by disease tick (`CampaignOwners.cs:1441`) | mechanic pressure already live |
| dream rng | `Rng.Fork(CampaignStreamIds.Psychology, day, 77)` | determinism preserved |
| catalogs | `exercise_routines.json` · `dream_templates.json` · `sanitation_facilities.json` present | no data prerequisites |
| prior plan statuses | Plan 216 SEALED · Plan 177 SEALED · Plan 210 COMPLETE | do not re-open host seams |

**Residual uncertainties (honest):** exact rest-port target for `EffectiveRestBonus` (W2 P1 decides: Fatigue vs sleep-quality path — PIR-4); whether W3 cleaning labor should hard-fail or soft-clamp when roster truth is thin (W3 P0 note); whether the exercise board is a new route or a Detail extension (PIR-8 decision).

---

## 13. Coordination & claims

**Claim template:**
```
claim-triad-b-wN-<slug>-YYYY-MM-DD
Owner: <integrator>
Paths: <exact files from the wave impact map>
Non-goals: <wave section>
Acceptance: <commands from §12/DoD>
Status: ACTIVE
```

**Never-claim-together pairs:** W1×W2 (both extend SurvivorDetail/panel hubs), W2×W3 (both touch manifest/registry bursts), this program × `PFGL-RT-*` combat tetrad while either holds `Main.GameFlow`/`Main.PlayerSurfaces`.

**Sibling plans:** the PFGL master plan (R5) lists Exercise/Dream as Tier-B runner-ups and sleep work as W10 — this Triad B plan is the promoted, sealed execution of the Exercise/Dream rows and the sanitation row; keep their appendix cross-references consistent when both are edited. Real-time combat tetrad owns `TacticalCombatSystem` paths — disjoint claims per its coordination note.

**Handoff format (per `AI_AGENT_WORKFLOW.md`):** Outcome · Files touched · Files intentionally untouched · Contract · Verification (commands + results) · Limitations · WORKTREE status · Next agent.

---

## 14. Program Definition of Done

1. W1 sealed at MECHANIC tier: fatigue cost and conditioning composition are felt in play.
2. W2 sealed at MECHANIC tier: journal + interpretation relief land exactly once; rest bonus ported.
3. W3 sealed at FEATURE→MECHANIC: cleaning/facilities/spills operable; hygiene stays derived.
4. All PIR records attached per wave; no new save sections; pin stays 266 unless unrelated work lands.
5. Manifest reflects three honest surfaces (new interactive entries; `sanitation` promoted).
6. Focused verifications + manual scripts attached per handoff; no full-suite runs.
7. Zero parallel ledgers; zero unbound ports; zero computed-then-dropped results remain in Triad B.

---

## Appendix A — Manual player scripts (15 FPS sessions)

**W1 Training:** open training board → assign survivor `routine_type: cardio` session → confirm Fatigue readout rises after completion → advance two idle days → confirm streak resets/decay begins → run paired ticks on fit vs unfit survivor → unfit accrues less fatigue.
**W2 Dreams:** advance a night after high-stress events → open dream journal → read last night’s record + streak → interpret a nightmare → confirm stress/morale relief lands once → save/load → confirm no re-apply.
**W3 Sanitation:** open sanitation panel → check room burdens → assign 2 workers High priority to dirtiest room → confirm burden/hygiene band moves → install a catalog facility → force a spill path (or wait) → resolve with Critical cleaning.

## Appendix B — Focused test matrix (create only when implementing)

| Test file (suggested) | Covers | Run |
|---|---|---|
| `Ashfall.Core.Tests/Survivors/ExerciseFatiguePortTests.cs` | F1 polarity + one-shot | alone first |
| `Ashfall.Core.Tests/Survivors/ExerciseFatigueResistanceCompositionTests.cs` | F2 paired-tick | alone first |
| `Ashfall.Core.Tests/Survivors/DreamRestBonusPortTests.cs` | F4 polarity | alone first |
| `Ashfall.Core.Tests/Survivors/DreamInterpretationReliefTests.cs` | F5 one-shot + save/load | alone first |
| `Ashfall.Core.Tests/Shelter/SanitationOpsVerbTests.cs` | F7 host verbs mirror Core | alone first |
| existing `Plan216*` / `Plan177*` / `Plan210*` | regression | scoped |

## Appendix C — Drift watchlist (re-rg before each claim)

| Wave | Re-verify |
|---|---|
| W1 | `ExecuteRoutine`/`ExecuteWorkout`/`GetFatigueResistanceMultiplier` names; Needs mutator polarity |
| W2 | `ProcessSleepCycle`/`InterpretDream`/`EffectiveRestBonus`; `is_interpreted` semantics; relief DEC state |
| W3 | `ApplyCleaning`/`InstallFacility`/`CleaningPriority`; `hygiene` day-owner ordering |

---

# END OF PFGL-TRIAD-B-2026-09-25 R1
