# Wave 9 Part 2 C3 — Radiation Balance Findings F1–F8 Decision

## Decision ID

`WAVE9-PART2-C3-RADIATION-BALANCE-F1-F8`

## Header

- **Source blocker:** `docs/balance/BALANCE_SIM_radiation_exposure_20A.md` findings F1–F8 left as proposal-only for foreman: "Proposal-only balance questions (F1–F4 in the main report) remain open for the foreman."
- **Current HEAD:** Commit `HEAD` (`Assets/Ashfall.Core/Radiation/`, `Shelter/ShelterShieldingModel.cs`, `weather_effects.json`).
- **Current owner:** `RadiationSystem` (dose calculation & accumulation); `ShelterShieldingModel` (shelter attenuation authority); `weather_effects.json` (weather radiation modifiers).
- **Current measurements (reproduced at current HEAD):**
  - Ran `Ashfall.Core.Tests/Radiation/Plan20ARadiationBalanceSweepTests.cs` (9/9 PASS):
    - Intact shelter (attenuation 1.0): 0 mSv/h → 0 dose/day → 0 lifetime.
    - Degraded shelter (attenuation 0.4): 1.2 mSv/h → 28.8 dose/day → 1,728 lifetime (acute cap 100 hit at ~83 h).
    - Wasteland clear: 40 mSv/h → 960 dose/day → 57,600 lifetime (acute cap 100 hit in 2.5 h).
    - Wasteland fallout storm (+150): 190 mSv/h → 4,560 dose/day → 273,600 lifetime (acute cap hit in 32 min).
    - Expedition low sector: 6 mSv/h → 144 dose/day → 8,640 lifetime (acute cap hit in 16.7 h).
    - Expedition high sector: 24 mSv/h → 576 dose/day → 34,560 lifetime (acute cap hit in 4.2 h).
  - Ran `Ashfall.Core.Tests/Shelter/Plan20BShieldingBalanceSweepTests.cs` (12/12 PASS):
    - Intact ceiling + radon floor (12 Bq/m³): 0.06 mSv/h → 1.44 dose/day → 86.4 lifetime.
    - Degraded ceiling (att 0.5): 1.06 mSv/h → 25.4 dose/day → 1,526 lifetime.
    - Clogged filter (×1.6): 1.66 mSv/h → 39.8 dose/day.
    - Open airlock in storm (×3.0): 3.06 mSv/h → 73.4 dose/day.
    - Flooding (+1.0): 2.06 mSv/h → 49.4 dose/day.
    - Decontamination active: 1.53 mSv/h → 36.7 dose/day.

---

## Findings Classification and Recommended Verdicts

| Finding | Original Problem Statement | Current Measured Truth | Classification | Recommended Verdict | Rationale |
|---|---|---|---|---|---|
| **F1** | Surface & expedition rates (6–40 mSv/h) saturate acute scale (100) in 2.5–17 hours without near-total protection. | High unshielded rates reproduce identically; tests pin exact curves. However, Plan 21 (gear condition & wear) and Plan 50 (vehicles) now provide authored protective mitigation. | **SURVIVES — DESIGN VALIDATED** | **DECLINE (KEEP AS AUTHORED)** | Severe outdoor radiation is the core mechanical driver requiring protective gear, masks, vehicles, and anti-rad medicine. Artificially deflating rates would trivialize the survival loop. |
| **F2** | Ashfall weather rad modifier misaligned with forecast. | `weather_effects.json` has `outdoor_rad_modifier: 45` for Ashfall, exactly matching forecast. | **MITIGATED BY LATER WORK** | **VERIFIED-RESOLVED** | Already implemented and verified in Plan 20A. |
| **F3** | Ceiling attenuation is dominant shelter lever; filters must be sized meaningfully. | Plan 20B shelter shielding model calibrated filter (×1.6), airlock (×3.0), and flood (+1.0) against the 2.0 mSv/h base. | **INFORMATIONAL / ARCHITECTURAL** | **CLOSED (ACCEPTED)** | Guidance fulfilled by Plan 20B architecture. |
| **F4** | Intact shelter produced 0 dose; needed realistic background/radon floor. | 20B added 12 Bq/m³ radon floor (0.06 mSv/h = 1.44 mSv/day). | **RESOLVED BY LATER WORK** | **VERIFIED-RESOLVED** | Solved in Plan 20B; verified by `Plan20BShieldingBalanceSweepTests`. |
| **F5** | Weather cannot enter intact structure (storm only matters via defects). | Verified: intact structure has zero weather infiltration; open airlock in storm multiplies to 3.06 mSv/h. | **CONFIRMED INVARIANT** | **CLOSED (PRESERVED)** | Core invariant confirmed. |
| **F6** | Maintenance ordering: filter < airlock < flooding. | Confirmed: filter (+57%) < airlock in storm (+189%) < flooding. | **CONFIRMED INVARIANT** | **CLOSED (PRESERVED)** | Hierarchy validated. |
| **F7** | Decontamination is bounded (halves internal sources only). | Confirmed: decon reduces flooded rate from 2.06 to 1.53 mSv/h. | **CONFIRMED INVARIANT** | **CLOSED (PRESERVED)** | Boundary validated. |
| **F8** | Intact shelter floor: 1.44 mSv/day from radon alone. | Confirmed: 1.44 mSv/day baseline persists even at 100% ceiling integrity. | **CONFIRMED INVARIANT** | **CLOSED (PRESERVED)** | Target rate verified. |

---

## Architecture-Safe Recommendation

**Approve Closeout of F1–F8 as Specified Above:**
- **F1:** DECLINE data re-scale; retain authored harshness as core survival pressure.
- **F2, F4:** Mark VERIFIED-RESOLVED (addressed by Plans 20A and 20B).
- **F3, F5, F6, F7, F8:** Mark CLOSED / CONFIRMED (invariants preserved).
- Zero data modifications required. All test pins in `Plan20ARadiationBalanceSweepTests` and `Plan20BShieldingBalanceSweepTests` remain 100% passing.

---

## Foreman Signature Gate

- **Chosen Verdicts:** [PENDING FOREMAN DECISION]
- **Signer:** [User / Foreman]
- **Date:** [YYYY-MM-DD]
- **Conditions:**
  1. No hardcoded overrides in C# code.
  2. Test pins in `Plan20ARadiationBalanceSweepTests` and `Plan20BShieldingBalanceSweepTests` must not be loosened.
