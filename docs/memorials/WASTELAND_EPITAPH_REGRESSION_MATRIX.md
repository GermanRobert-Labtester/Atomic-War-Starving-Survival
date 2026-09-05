# Wasteland Grave Epitaphs — Regression Matrix

**Verification Scope:** Data Integrity, Catalog Schema, Cause Resolution, Determinism, Save Round-Trip, Content Utilization.

---

## 1. Scenario Matrix

| ID | Scenario | Target Behavior | Verified Status |
|---|---|---|---|
| REG-01 | Baseline Preserved | Existing 8 entries retained byte-for-byte in original sequence | VERIFIED |
| REG-02 | Total Count Target | Total records in catalog equals exactly 30 | VERIFIED |
| REG-03 | Radiation Set | 3 radiation epitaphs available (1 baseline + 2 new) | VERIFIED |
| REG-04 | Combat Set | 3 combat epitaphs available (1 baseline + 2 new) | VERIFIED |
| REG-05 | Starvation Set | 3 starvation epitaphs available (1 baseline + 2 new) | VERIFIED |
| REG-06 | Exhaustion Set | 2 exhaustion epitaphs available (1 baseline + 1 new) | VERIFIED |
| REG-07 | Disease Set | 2 disease epitaphs available (1 baseline + 1 new) | VERIFIED |
| REG-08 | Expedition Set | 2 expedition epitaphs available (1 baseline + 1 new) | VERIFIED |
| REG-09 | Trauma Set | 2 trauma epitaphs available (1 baseline + 1 new) | VERIFIED |
| REG-10 | Exposure Set | 2 exposure epitaphs available (2 new) | VERIFIED |
| REG-11 | Suicide Set | 2 suicide epitaphs available (2 new, non-romanticizing) | VERIFIED |
| REG-12 | Infection Set | 1 infection epitaph available (1 new) | VERIFIED |
| REG-13 | Old Age Set | 2 old_age epitaphs available (2 new) | VERIFIED |
| REG-14 | Drowning Set | 1 drowning epitaph available (1 new) | VERIFIED |
| REG-15 | Frostbite Set | 1 frostbite epitaph available (1 new) | VERIFIED |
| REG-16 | Poisoning Set | 1 poisoning epitaph available (1 new) | VERIFIED |
| REG-17 | Execution Set | 1 execution epitaph available (1 new) | VERIFIED |
| REG-18 | Unknown / Fallback | 1 unknown + 1 unspecified fallback available | VERIFIED |
| REG-19 | Seeded Determinism | Same cause + seed produces identical string | VERIFIED |
| REG-20 | Seed Variety | Different seeds select distinct entries in multi-candidate pools | VERIFIED |
| REG-21 | Candidate Reachability | All 30 entries reachable under uniform PRNG rolls | VERIFIED |
| REG-22 | Save Round-Trip | MemorialEntry persists selected epitaph string across save/load | VERIFIED |
| REG-23 | DataRule Compliance | No real-world countries/alliances referenced | VERIFIED |
| REG-24 | Length & Sentence | All 22 new entries are 1 sentence and 5–20 words | VERIFIED |
| REG-25 | Duplicate Audit | Zero exact duplicate strings across all 30 entries | VERIFIED |
