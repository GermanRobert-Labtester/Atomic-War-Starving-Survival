# Unified Radio Programming Grid

> **Document Status:** Authoritative Broadcast Scheduling Matrix
> **Authority:** Plan 24 (Task 24D)
> **Temporal Resolution:** Day / Window Based (Continuous Simulation Day Clock)

---

## 1. Programming Grid Across Campaign Phases

The airwaves evolve dynamically across four campaign phases:

### Phase 1: Early Survival & Shelter Sealing (Days 1–45)
- **Primary Audible Stations:** `station_civil_defense` (88.5 MHz), `station_garrison_overlord` (88.4 MHz), `station_emergency_relay` (102.1 MHz), `station_numbers_sigint` (7.325/14.487 MHz).
- **Core Themes:** Fallout survival, water boiling directives, martial law declarations, initial distress beacons.
- **Atmosphere:** Shock, bureaucratic confusion, militarized order, desperate survivor queries.

### Phase 2: Regional Consolidation & Scarcity (Days 46–180)
- **Primary Audible Stations:** `station_open_classroom` (91.3/142.5 MHz), `station_vitrified_crater` (104.2 MHz), `station_works_allotment` (101.5 MHz), `station_scavenger_net` (98.5 MHz).
- **Core Themes:** Infrastructure repair (copper wire, guy lines), agricultural seed trade, ideological religious sermons, barter disputes.
- **Atmosphere:** Hardened routine, cold pragmatism, growing faction rivalries.

### Phase 3: Year of Ash & Deep Thaw (Days 181–360)
- **Primary Audible Stations:** `station_automated_relay` (142.85 MHz), `station_deep_vault_zero` (104.7 MHz), `station_garrison_overlord` (88.4 MHz), `station_verdict_census` (99.0 MHz).
- **Core Themes:** Maritime rescue countdown (*Aurora Borealis* departure Day 360), geological radon gas warnings, orbital kinetic decay, Census carrier opening.
- **Atmosphere:** Escalating urgency, mechanical inevitability, harsh winter / toxic thaw dichotomy.

### Phase 4: The Verdict & Faction Reckoning (Days 361+)
- **Primary Audible Stations:** `station_verdict_census` (99.0 MHz), `faction_war_radio` (88.4 / 104.2 / 96.1 MHz), `station_numbers_sigint` (resolving).
- **Core Themes:** The Census Reckoning, Tribunal evidence broadcasts, full-scale faction war bulletins, terminal cipher solutions.
- **Atmosphere:** Heavy historical reckoning, empty frequencies where destroyed factions once spoke.

---

## 2. Schedule Window Resolution Pipeline

```text
1. Player tunes to Frequency F at Day D
2. Find candidate stations whose assigned frequency is within tolerance (|F - F_station| <= 0.5 MHz)
3. Check station state (Normal, Degraded, Jammed, Silent)
   - If Silent: return authored dead air / static hiss
   - If Jammed: return high static + fragmented carrier
4. Evaluate active Appointment Programs for Day D and matching frequency
   - If an Urgent / Emergency bulletin is active (e.g. Severe Weather / Orbital Warning), it takes top priority
5. Evaluate Phase-bound & One-Shot Broadcasts for Day D
6. Fallback to Station Routine Intercept Chatter or Silence Event
7. Compute VU strength based on tuning offset and receiver quality
```
