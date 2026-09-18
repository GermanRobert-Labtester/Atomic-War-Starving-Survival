# WAVE 11 PART 1 — TASK A2 IMPLEMENTATION LOG
## C1[12] Plan 41 — Memory That Acts: Heirlooms, Eulogies, and What the Dead Leave Behind

### 1. Overview & Verification Summary
Task A2 reconciles and wires memory systems into concrete, decision-relevant behavior without creating parallel memory architectures or duplicate save stores:
1. **Memorials & Eulogies**: Wired `ProceduralEulogyEngine` into `MemorialSystem.Memorialize` to compose literary funeral eulogies preserved on `MemorialEntry.EulogyText`, round-tripping through save/load.
2. **Heirlooms**: Added `GetHolderMoraleModifier` and `GetHolderFatigueRelief` on `HeirloomSystem`, directly deriving bounded behavioral modifiers from held heirlooms and their memories.
3. **Place Memory**: Added visit tracking (`RecordSiteVisit`, `HasVisited`, `GetSiteVisitCount`, `GetLastVisitDay`) to `LocationMemorySystem` and persisted in `LocationMemoryState`.
4. **Generational Continuity**: Added calendar-driven maturation check `CheckCohortMaturation` to `CohortSystem` to mature eligible living children once they reach the required age threshold.

### 2. Implementation Matrix Centerpiece
| Memory | Owner | Persisted? | Existing consumer | Plan-required behavior | Bounded wiring | Effect | Test |
|---|---|---|---|---|---|---|---|
| Funeral Eulogies | `MemorialSystem` | Yes (`MemorialEntry.EulogyText`) | Memorial UI, Funeral Vigil | Compose bespoke literary inscriptions from life records | Optional `ProceduralEulogyEngine` on `MemorialSystem.Memorialize` | Persisted eulogy text on memorial entry | `Plan41MemoryActsTests.MemorialSystem_WithEulogyEngine_ComposesAndPersistsEulogy` |
| Heirloom Morale & Relief | `HeirloomSystem` | Yes (via held heirloom state) | Morale / Fatigue stack | Behavioral value derived from held heirlooms | `GetHolderMoraleModifier` (-20 to +20) and `GetHolderFatigueRelief` (0 to 2) | Dynamic morale modifier and fatigue relief | `Plan41MemoryActsTests.HeirloomSystem_HolderModifiers_CalculatedAndBounded` |
| Place Memory / Revisit | `LocationMemorySystem` | Yes (`visitCounts`, `lastVisitDays`) | Gazetteer / Travel / Map | Track visit counts and last visit day | `RecordSiteVisit`, `HasVisited`, `GetSiteVisitCount`, `GetLastVisitDay` | Site history and visit awareness | `Plan41MemoryActsTests.LocationMemorySystem_TracksAndPersistsVisits` |
| Cohort Maturation | `CohortSystem` | Yes (`isMatured`, `maturationDay`) | Duty Roster, Schooling, Ending | Calendar-driven cohort child maturation | `CheckCohortMaturation(currentDay, maturationAgeDays)` | Transitions eligible children to mature status | `Plan41MemoryActsTests.CohortSystem_CheckCohortMaturation_MaturesEligibleChildren` |

### 3. Test Evidence
- `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs`: 4/4 PASS
- `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`: 10/10 PASS
- `Ashfall.Core.Tests/HeirloomSystemTests.cs`: 9/9 PASS
- `Ashfall.Core.Tests/CohortSystemTests.cs`: 10/10 PASS
