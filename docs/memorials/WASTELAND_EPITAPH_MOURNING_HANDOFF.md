# Wasteland Grave Epitaphs — Mourning System Handoff

**Authorities:**
- `Ashfall.Core.Memorial.MemorialSystem`
- `Ashfall.Core.Memorial.IGriefSink` (`RelationsGriefSink`)
- `Ashfall.Core.Survivors.SurvivorRelationsSystem`

---

## 1. System Boundary

1. **Epitaph Scope:** The epitaph text in `wasteland_grave_epitaphs.json` provides purely narrative and atmospheric texture. It owns **zero** mechanical simulation effects.
2. **Mourning & Grief Scope:** Grief dispersion, morale penalties, vigil attendance, and survivor mourning reactions are owned by `MemorialSystem` and `RelationsGriefSink`.
3. **No Dynamic Interference:** Modifying or expanding the epitaph pool does not alter `DeathQuality`, `MoraleDelta`, or relationship trust calculations.
