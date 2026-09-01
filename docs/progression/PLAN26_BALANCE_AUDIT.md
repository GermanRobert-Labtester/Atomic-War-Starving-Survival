# Plan 26 Balance Audit

> **Document Status:** Authoritative Economy & Pacing Balance Audit
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. Pacing Curves & Progression Rates

1. **Research Lab Velocity:**
   - A single researcher operating at base productivity completes ~3-4 Tier 1 nodes or 1-2 Tier 3 breakthroughs per 30-day survival campaign cycle.
   - Dual researchers or assigned `Polymath` survivors accelerate day ticks by up to 35%, allowing deep specialization in 2 branches (e.g. Medical + Survival).

2. **Skill XP Economy & Atrophy:**
   - Action XP gain is clamped per shift, requiring ~5-10 dedicated shifts in a discipline to unlock Tier-Threshold action skills (`xpThreshold = 50.0`).
   - Expert skills (`xpThreshold = 120.0`) require sustained focus over several weeks.
   - `SkillAtrophySystem` gracefully transitions neglected skills to `dormantSkillIds` after 14 days of inactivity, which reactivate upon the first recorded action.

3. **Trade Specialty Progression:**
   - Requiring 3 specific item category crafts per trade ensures mastery feels earned and tied to shelter expansion priorities rather than instant day-1 perks.
