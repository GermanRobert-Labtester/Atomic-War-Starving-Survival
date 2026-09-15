# Plan 194 — Emergency alerts producer inventory + authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_194_*`. `EmergencyResponseHud` already exists. HUD is **not** gameplay authority.

---

## 1. Premise — producer inventory

`CrisisPresentationCoordinator.EvaluateCrisisState()` is a hard-coded if/else. `AcknowledgeCurrentCrisis()` is in-memory only. **No `emergency_alert` save.**

| Producer | Fact | Wired to crisis coordinator? |
|---|---|---|
| `PowerGridSystem` | brownout / generation ≤ 0 | **Yes** (prio 1) |
| `StartingLevelSystem` | `airHazardWarning` | **Yes** (prio 2) |
| `DiseaseSystem` | `outbreak_active` | **Yes** (prio 3) |
| `WeatherSystem` | Fallout/EMP/Blizzard | **Yes** (prio 4) |
| `RadiationSystem` | Bind param | **API only** |
| `SurvivorFateSystem` | Bind param | **API only** |
| `ShelterFireHazardSystem` | fire/alarm | **No** (own panel/save) |
| `SumpFloodingSystem` | flood | **No** |
| `WeatherStationSystem` | forecast warning | **No** |
| `SeasonalEventSystem` | `OnEventTriggered` | **No** |
| `RadioScheduleCoordinator` | `BroadcastGenre.EmergencyAlert` + inject APIs | **No** (host never injects) |
| `DailyBriefingReportBuilder` | daily report | **No** |

---

## 2. Ownership (proposed)

Typed facts stay on **component owners**. Presentation aggregator (`CrisisPresentationCoordinator` + HUD) **reads** them. Ack is presentation-only unless a later amendment adds persist.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| New `EmergencyAlertSystem` / alert save / evacuation owner | **OUT** |
| HUD as authority | **OUT** |
| Wire unused Bind params (radiation, fate) + fire/flood into the existing coordinator | **IN** (implement later) |
| Ack remains presentation-only (no persist) | **IN** |
| Radio inject APIs stay radio schedule, not shelter alerts | **IN** |

**Next implement:** `DEBT-194-CRISIS-PRODUCER-WIRE` — extend `EvaluateCrisisState` to include fire + flood + existing unused Bind facts. No new Core controller.

---

## 4. Evidence paths

`UI/CrisisPresentationCoordinator.cs`, `CrisisPresentationSnapshot.cs`, `src/UI/EmergencyResponseHud.cs`, `src/Main.UiPanels.cs`, `src/Main.PlayerSurfaces.cs`, `ShelterFireHazardSystem.cs`, `RadioScheduleCoordinator.cs`, `CrisisPresentationCoordinatorTests.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
