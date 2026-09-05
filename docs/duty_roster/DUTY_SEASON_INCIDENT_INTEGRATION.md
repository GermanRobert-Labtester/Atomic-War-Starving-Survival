# Duty Season Incident Integration

> **Incident Seams:** Interaction between duty roster encounter pressure and Plan 57 incidents (`incidents.json`).

---

## 1. Single Application Principle

- **Encounter Weight Role:** `encounterWeight` scales shelter-internal visitor and character encounter frequency within `ShelterEncounterSystem`.
- **No Double Scaling:** Incident generation algorithms (Plan 57) must NOT multiply their base occurrence rate by `encounterWeight` if the encounter system already incorporates it.
- **Incident Gating:** Specific incident categories (e.g. frozen pipe leaks during winter, perimeter raids during siege) query their own prerequisite flags, room conditions, and dates. Season data provides background pressure, not direct event triggers.
