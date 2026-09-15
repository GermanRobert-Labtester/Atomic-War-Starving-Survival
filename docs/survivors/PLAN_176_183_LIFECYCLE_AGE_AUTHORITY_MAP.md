# Plans 176 / 183 — Campaign age + child-phase authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Rebase:** `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md` (176 Aging; 183 child development)  
**Date:** 2026-09-12

**Stale historical prose:** `Next-steps-plans/Plan_176_Aging_Elderly_Survivor_System.md`, `Plan_183_Child_Development_Stages_System.md`. Do not treat as current evidence. `src/Main.Plans178_181.cs` is **number drift** (not Plans 178–181).

---

## 1. Premise

| Concern | Owner | Notes |
|---|---|---|
| Join / deploy / die / memorialize | `SurvivorLifecycle` | Location/death SM only. **No age.** |
| Living roster | `SurvivorRosterSystem` | `joinedDay`. **No age.** |
| Child phases (Infant → AdultTransitioned) | **`GenerationalSystem` / `ChildDevelopment`** | Daily `GrowthTick`. Save `child_development`. UI `NurseryPanel`. Catalog `development_traits.json`. Progress 0–100, not calendar years. |
| Dose second-generation baseline | `CohortSystem` inside `dose_ledger` | `birthDay` + `isMatured`. **Not nursery phases.** |
| New Game starting party | `StartingCohortCatalog` | Setup profiles. Members have **no age**. |
| Expansion-12 chapter years | `GenerationalSuccessionEngine.inGameAgeYears` | Hub-only. **Not campaign aging.** |
| `AgingSystem` / adult `LifeStage` | **ABSENT** | |

Live campaign adult age clock: **none**. Child owner already exists; forensic “no child system” is stale.

---

## 2. Ownership (proposed)

| Concern | Authority |
|---|---|
| Adult/elder campaign chronology | **One clock** derived from `joinedDay` / optional `birthDay` on the roster aggregate — not a new `AgingSystem` ledger |
| Child phases | **`GenerationalSystem` only** |
| Dose maturation | `CohortSystem` stays dose-only |
| Expansion years | Stay on expansion hub |
| Care / apprenticeship | Existing owners; may *read* the clock later; no age field of their own |

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| New `AgingSystem` / `ChildDevelopmentSystem` / `development_stages.json` | **OUT** |
| Starting cohorts as childhood | **OUT** |
| `CohortSystem.TryMaturation` as development | **OUT** |
| Gameplay aging via `GenerationalSuccessionEngine` | **OUT** |
| Single campaign age clock (days from join/birth) readable by `GenerationalSystem` + later elder consumers | **IN** (implement later) |
| Keep child `DevelopmentPhase` progress-based until a signed calendar-threshold amendment | **IN** |

**Honesty:** roster without a clock is not “elderly.” Nursery progress is not years.

**Next implement (after sign-off, separate claim):** `DEBT-176-CAMPAIGN-AGE-CLOCK` — add the clock on the existing roster/lifecycle aggregate; `GenerationalSystem` may read it. No second child save.

---

## 4. Exact paths (read-only evidence)

`SurvivorLifecycle.cs`, `GenerationalSystem.cs`, `CohortSystem.cs`, `GenerationalSuccessionEngine.cs`, `StartingCohortCatalog.cs`, `src/Host/GenerationalSaveStore.cs`, `development_traits.json`, `src/Main.Plans178_181.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
