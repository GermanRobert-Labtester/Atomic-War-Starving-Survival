# MUSTER CONFLICT STATE MACHINE
## Lifecycle, Transitions, and Conservation Invariants

**Document Version:** 1.0.0
**Scope:** Plan 72 (Muster Warfare & Rally Depth)
**Status:** Authoritative Specification

---

## 1. State Machine Overview

The Muster conflict lifecycle models the active mobilization, deployment, combat engagement, and resolution of real settlement forces in wasteland faction wars.

```text
               ┌─────────────┐
               │    Idle     │◄──────────────────────────────────┐
               └──────┬──────┘                                   │
                      │ Mobilize()                               │
                      ▼                                          │
               ┌─────────────┐                                   │
       ┌───────┤ Mobilizing  ├───────┐                           │
       │       └──────┬──────┘       │                           │
       │              │ Readiness=1  │ Cancel()                  │
       │              ▼              │                           │
       │       ┌─────────────┐       │                           │
       │       │    Ready    ├───────┤                           │
       │       └──────┬──────┘       │                           │
       │              │ Deploy()     │                           │
       │              ▼              │                           │
       │       ┌─────────────┐       │                           │
       │       │  Deployed   │       │                           │
       │       └──────┬──────┘       │                           │
       │              │ Clash        │                           │
       │              ▼              │                           │
       │       ┌─────────────┐       │                           │
       │       │   Engaged   │       │                           │
       │       └──────┬──────┘       │                           │
       │              │ Resolve()    │                           │
Withdraw()            ▼              ▼                           │
       │       ┌─────────────┐ ┌───────────┐                     │
       │       │  Resolving  │ │ Cancelled ├─────────────────────┤
       │       └──────┬──────┘ └───────────┘                     │
       │              │                                          │
       │              ▼                                          │
       │       ┌─────────────┐                                   │
       │       │  Aftermath  │                                   │
       │       └──────┬──────┘                                   │
       │              │ Route Consequences                       │
       │              ▼                                          │
       │       ┌─────────────┐                                   │
       └──────►│ Recovering  ├───────────────────────────────────┘
               └─────────────┘ (Cooldown / Healing expired)
```

---

## 2. State Definitions

| Phase | Description | Allowed Player Actions | Permitted Next States |
|---|---|---|---|
| **Idle** | No force mobilized. Resources and survivors at baseline. | `Mobilize()` | `Mobilizing` |
| **Mobilizing** | Real survivors and equipment assembled; supplies reserved from inventory. | `Cancel()`, `AddRoster()`, `RemoveRoster()`, `AssignGear()` | `Ready`, `Cancelled`, `Idle` |
| **Ready** | Force roster is complete, equipped, fed, and standing at full readiness. | `Deploy()`, `Cancel()` | `Deployed`, `Cancelled` |
| **Deployed** | Force is en route or positioned in target contested sector. | `Withdraw()`, `Engage()` | `Engaged`, `Withdrawn` |
| **Engaged** | Active combat resolution underway through combat/pressure authority. | `Withdraw()`, `Resolve()` | `Resolving`, `Routed`, `Withdrawn` |
| **Resolving** | Dice/tactics resolved; casualty and outcome calculations finalized. | Internal computation | `Aftermath` |
| **Aftermath** | Casualties routed to Memorial, wounded to Clinic, standing/escalation updated. | `AcknowledgeReport()` | `Recovering` |
| **Recovering** | Surviving personnel in transit or post-battle fatigue; equipment returned. | None (auto ticks) | `Idle` (when recovery ticks reach 0) |
| **Withdrawn** | Force broke off before catastrophic collapse; supplies partially returned. | Internal transition | `Recovering` |
| **Cancelled** | Mobilization cancelled prior to engagement; all reserved supplies restored. | Internal transition | `Idle` |
| **Routed** | Force broke under overwhelming pressure; heavy casualties and equipment loss. | Internal transition | `Aftermath` |

---

## 3. Conservation Invariants

1. **Roster Conservation:**
   - Every combatant corresponds to an alive, non-incapacitated survivor ID from `ISurvivorRoster`.
   - `SurvivingSoldiers + FallenCasualties = MobilizedSoldiers`.
   - Fallen combatants are marked deceased in the survivor roster and registered into `MemorialSystem` exactly once.

2. **Supply & Equipment Conservation:**
   - Supplies are atomically reserved upon `Mobilizing`.
   - Conservation equation:
     $$\text{Issued} + \text{Remaining} + \text{Lost} + \text{Returned} = \text{InitialReserved}$$
   - Cancelling mobilization restores 100% of unconsumed reserved goods to shelter inventory.
   - Wounded or deceased combatants drop equipment based on doctrine recovery rules; salvaged items return to inventory.

3. **Persistence Rule:**
   - Mid-conflict saves preserve `CurrentPhase`, `ActiveRoster`, `ReservedSupplies`, `EngagementSeed`, and `TargetSector`.
   - Reload restores the exact conflict state without double-deducting supplies or regenerating combatants.
