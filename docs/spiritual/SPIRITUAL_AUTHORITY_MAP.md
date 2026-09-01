# Spiritual, Faith & Meaning — Authority Map

---

## 1. Non-Negotiable Invariant: No Parallel Piety or Faith Meters

Plan 30 explicitly forbids adding a `Faith`, `Spirituality`, `Piety`, or `Devotion` numeric resource. Every spiritual, ritual, or cultural action routes directly through existing project authorities.

---

## 2. Domain & Authority Mapping

| Concept / Experience | Single Source of Truth | Mechanism / Method |
| :--- | :--- | :--- |
| **Ritual Comfort & Morale** | `NeedsSystem` / `ShelterMorale` | Capped, small morale delta (`+1.0f` to `+4.0f`) with cooldowns |
| **Grief Mitigation** | `MemorialSystem` | `DeathQuality`, `MemorialOutcome`, and `IGriefSink` scaling |
| **Guilt & Regret** | `GuiltInsomniaSystem` | `RecordGuilt()`, `ResolveGuilt()` on broken promises or skipped rites |
| **Ideological Friction** | `IdeologicalFrictionSystem` | `ConflictGroups`, `GetRoommateCompatibilityMultiplier()` |
| **Leadership Challenges** | `LeadershipSystem` | `OnLeaderStressIncreased`, `OnLeaderBreakRisk` |
| **Child Lore Transmission** | `CohortSystem` | Cohort presence, child generation age filtering |
| **Dying Wishes & Promises** | `FinalWishSystem` | `OnFinalWishCompleted`, `OnFinalWishFailed` hooks |
| **Belief Movement Data** | `belief_movements.json` | Authoritative definition in JSON catalog |
| **Ritual & Superstition Data** | `spiritual_rituals.json` | Authoritative definition in JSON catalog |
| **Memorial Rites Data** | `memorial_rites.json` | Authoritative definition in JSON catalog |
