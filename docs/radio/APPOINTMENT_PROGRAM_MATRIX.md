# Appointment Program Matrix

> **Document Status:** Authoritative Recurring Broadcast Specification
> **Authority:** Plan 24 (Task 24F, 24G, 24AM–24AQ)
> **Total Recurring Programs:** 6 Canonical Formats

---

## 1. The Six Appointment Programs

| Program ID | Program Name | Broadcaster / Station | Frequency | Recurring Schedule | Authoritative System Input | Player Value / Consequence |
|---|---|---|---|---|---|---|
| `prog_morning_weather` | **Morning Meteorological & Fallout Forecast** | Central Civil Defense Service | `88.50 MHz` | Daily (Window: Morning / Day start) | `WeatherSystem` (Plan 19) | Diegetic forecast of temperature, rain/snow, and impending fallout storms; prevents blind expedition exposure. |
| `prog_lost_and_found` | **Missing Persons & Survivor Message Roll** | Central Civil Defense & Open Airwaves | `88.50 MHz` / `91.30 MHz` | Every 3 Days (Evening) | `SurvivorCatalog` / Narrative Graph | Humanizes the world; occasionally provides clues to recruit locations or confirms civilian deaths. |
| `prog_market_caravan` | **Regional Market & Caravan Exchange Bulletin** | The Works Public Council | `101.50 MHz` | Every 4 Days (Midday) | `EconomySystem` / `CaravanSystem` | Announces caravan arrival dates, price embargoes, and regional goods scarcity in Sector 4. |
| `prog_route_conditions` | **Waystation & Road Condition Service** | The Lineman's Loop / Garrison | `142.50 MHz` / `88.40 MHz` | Daily (Afternoon) | `WastelandMapSystem` (Plan 16) | Reports blown bridges, blocked tunnels, radiation hot zones, and repaired waystation nodes. |
| `prog_public_health` | **Public Health & Outbreak Advisory** | Bureau of Public Health / Deep Vault | `88.50 MHz` / `104.70 MHz` | Weekly (Day 7, 14, 21...) | `DiseaseSystem` (Plan 09) | Warns of contaminated cisterns, airborne spore plumes, and vector protocol deadlines before outbreak spread. |
| `prog_industrial_foundry` | **Foundry & Labor Dispatch** | Central Garrison / The Works | `88.40 MHz` / `101.50 MHz` | Bi-weekly (Day 10, 24...) | `FoundryLaborSystem` (Plan 22) | Reports casting schedules, fuel shortfalls, labor strikes, and metal alloy exchange rates. |

---

## 2. Dynamic Integration Rules

1. **Weather Forecast Handoff:** When `WeatherSystem` schedules a `FalloutStorm` or `AcidRain` event within 24–48 hours, `prog_morning_weather` automatically elevates from a routine briefing to an `Urgent` weather advisory on `88.50 MHz`.
2. **Disease Vector Handoff:** When a shelter cistern is contaminated or a regional outbreak occurs in `DiseaseSystem`, `prog_public_health` broadcasts specific hygiene directives (e.g. "Purify water with chlorine titration; seal ventilation gates").
3. **Route Interruption Handoff:** When an expedition node or highway is collapsed in `WastelandMapSystem`, `prog_route_conditions` advises caravans to divert to secondary bypass routes.
4. **No Mandatory Micromanagement:** Missing an appointment broadcast never breaks campaign progression—information is always obtainable through secondary channels (physical scouting, trade emissaries, or recorded cassettes).
