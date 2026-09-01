# Cadence, Priority & Suppression Matrix

---

## 1. Contextual Suppression Matrix

| High-Priority Shelter State | Plan 30 Spiritual / Cultural Behavior |
| :--- | :--- |
| **Lethal Shelter Crisis (Generator drop, breached seal)** | Suppress all ambient folklore and optional rituals |
| **Active Fallout Storm / Combat Raid** | Suppress all non-essential events; allow only emergency folklore comfort (e.g. blackout freeze) |
| **Immediate Post-Death (0–24h)** | Enable acute grief and empty-bunk rites; suppress unrelated belief disputes |
| **Expedition Departure Window** | Permit door-tap and roster touchstone rituals |
| **Quiet Recovery Downtime** | Best window for memorial wall reading, apprentice labor, and still-hour events |

---

## 2. Cooldown & Anti-Exploit Enforcement

1. **Rituals:** Bounded to 1 to 5 days cooldown in `SpiritualMeaningCoordinator`. Repeated attempts within cooldown yield no morale gain.
2. **Memorial Rites:** Strictly single-execution per deceased survivor ID. Idempotent across save/load cycles.
3. **Belief Friction:** Superstitions trigger interpersonal friction only when an operational collision actually occurs (e.g. assigning a bed near a vent, ordering night maintenance).
