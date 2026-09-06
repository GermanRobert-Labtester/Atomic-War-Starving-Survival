# DAILY BRIEFING PRODUCER & SOURCE TABLE
## Authoritative Intelligence Collectors for the Settlement Dawn Briefing

**Document Version:** 1.0.0
**Scope:** Plan 75 (Daily Briefing & Dawn Intelligence Report)
**Status:** Canonical Intelligence Collector Matrix

---

## 1. Producer Integration Matrix

| Subsystem Domain | Authoritative Producer System | Output Fact Type | Default Taxonomy | Default Deep Link |
|---|---|---|---|---|
| **Vital Needs** | `NeedsSystem` | `fact_survivor_starvation`, `fact_survivor_dehydration` | `CRITICAL` / `WARNING` | `panel:medical` |
| **Power Grid** | `PowerGridSystem` | `fact_power_blackout`, `fact_fuel_depletion` | `CRITICAL` / `WARNING` | `panel:power_grid` |
| **Water Logistics** | `WaterTreatmentSystem` | `fact_water_brine_crisis`, `fact_filter_depleted` | `WARNING` | `panel:water_treatment` |
| **Medical / Epidemic** | `DiseaseSystem` | `fact_infection_outbreak`, `fact_quarantine_breach` | `CRITICAL` / `WARNING` | `panel:medical` |
| **Atmosphere** | `WeatherSystem` | `fact_weather_acid_rain`, `fact_weather_blizzard` | `WARNING` / `INTEL` | `panel:wasteland_map` |
| **Faction Warfare** | `MusterWarfareEngine` | `fact_muster_skirmish`, `fact_frontline_shift` | `WARNING` / `INTEL` | `panel:muster` |
| **Technology** | `ResearchSystem` | `fact_research_breakthrough` | `INTEL` | `panel:research` |
| **Cartography** | `WastelandMapSystem` | `fact_node_surveyed`, `fact_radio_rumor_acquired` | `INTEL` | `panel:wasteland_map` |

---

## 2. Dawn Execution Sequence

At each dawn advance ($t \to t+1$):
1. **Simulation Tick:** Core simulation updates all stateful domains.
2. **Fact Collection:** `IBriefingFactCollector` implementations scan their respective systems and emit `BriefingFact` records.
3. **Cadence Evaluation:** `DailyBriefingCadenceFilter` suppresses unchanged persistent warnings and repeats aged critical threats.
4. **Report Assembly:** `DailyBriefingReportBuilder` groups active facts into structured, prioritized sections.
5. **Cadence Ledger Update:** New severity and timestamps are saved to the persistent cadence envelope.
6. **Presentation:** Dawn modal surfaces the report to the player with clickable deep-links.
