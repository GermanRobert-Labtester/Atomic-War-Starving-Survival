# Plan 57 — Shelter Incident Expansion: Final Implementation Report

## Repository facts

| Question | Answer (verified) |
|---|---|
| Baseline count | **5** incidents (plan's verified baseline held) |
| Schema | `{ schema_version: 1, incidents: [{ id, title, bodyText, weight, minDay }] }` |
| Loader / consumer | `src/Host/EventsHostSession.cs` — a **text-only read-model** (`IncidentsRoot` → Events Log panel); no Core incident system exists |
| Scheduler / selection / RNG | **None.** No shelter-tick incident firing, no weighted selection, no RNG stream |
| Consequences | **None** — text-only entries |
| Choices | **None** — Choice classification: **Case D** |
| History / cooldown / once-only | **None** — "this session intentionally has no save state" (per the session's own doc comment) |
| `category` / `maxDay` / faction fields | **Not supported** — DTO binds only the five fields; serializer (default STJ options) ignores unknown fields → authoring them would be dead data (forbidden, §33) |
| Day semantics | Timeline display uses `minDay` as the day label; `events.json` (same shape, 220 entries) spans days 1–240 — campaign pacing: early <30, mid 30–70, late ≥70 |
| Weight semantics | Field bound by DTO but unused by the read model; authored as future-scheduler data per the frequency-intent bands (all originals 1.0) |

## Choice classification: **Case D**

Per §4.3 Case D: *"Do not expand runtime architecture casually. Implement the 20 text incidents and
validate cadence; document consequence wiring as follow-on."* No choice engine, no scheduler, no
consequence dispatch, no history schema was added. Choice concepts in §31/§39–42 remain
follow-on-compatible.

## Changed files

- `Assets/StreamingAssets/Data/incidents.json` — 5 → 25 entries (data only)
- `Ashfall.Core.Tests/Plan57IncidentTests.cs` — 12 new contract tests
- `docs/incidents/PLAN57_FINAL_REPORT.md` — this report

No runtime, validator, or tooling changes were required.

## Incident roster (20 new)

| ID | Category (documented) | weight | minDay | Faction link | Repeatability (future) |
|---|---|---|---|---|---|
| `incident_fallout_storm_approach` | environmental | 0.5 | 45 | — | cooldown-limited |
| `incident_contaminated_water_table` | environmental | 0.7 | 40 | — | state-dependent |
| `incident_ground_tremor` | environmental | 0.6 | **70** | — | cooldown-limited |
| `incident_perimeter_breach_attempt` | security | 0.7 | 35 | — | cooldown-limited |
| `incident_unknown_visitor` | security | 1.2 | 10 | **The Rebuilders** (prose) | moderate repeat |
| `incident_local_signal_intercept` | security | 0.7 | 30 | faction-adjacent (organized transmitter) | cooldown-limited |
| `incident_shelter_disease_outbreak` | medical | 0.5 | 45 | — | cooldown-limited |
| `incident_chemical_exposure` | medical | 0.8 | 25 | — | cooldown-limited |
| `incident_survivor_collapse` | medical | 0.8 | 15 | — | state-dependent |
| `incident_ration_dispute` | social | 1.3 | 6 | — | repeatable |
| `incident_ideological_friction` | social | 0.6 | 50 | — | cooldown-limited |
| `incident_grief_episode` | social | 0.6 | 35 | — | conditional (death-gated in future) |
| `incident_generator_failure` | equipment | 1.0 | 22 | — | repeatable |
| `incident_air_filter_breakdown` | equipment | 0.7 | 40 | — | cooldown-limited |
| `incident_water_pipe_burst` | equipment | 1.3 | 5 | — | repeatable |
| `incident_nearby_cache_discovered` | supply | 0.8 | 12 | — | once-only (future) |
| `incident_supply_drop_near_shelter` | supply | 0.4 | **75** | — | rare / once-only |
| `incident_faction_patrol_nearby` | external | 0.5 | 50 | **The Iron Garrison** (prose) | cooldown-limited |
| `incident_refugees_approaching` | external | 0.4 | **75** | — | rare / once-only |
| `incident_exchange_anniversary` | psychological | 0.3 | **90** | — | per-anniversary |

**Phase gates:** 7 early (<30), 8 mid (30–69), 5 late (≥70) — exceeds the 5-incident minimum.
**Faction links:** 3 (Iron Garrison patrol, Rebuilders visitor, organized-transmitter intercept) —
all grounded in real `faction_lore.json` identities; no invented factions.

## Semantic dedup vs. the original 5

| Original | New | Differentiation |
|---|---|---|
| `incident_radiation_spike` (cloud arrives over vents) | `incident_fallout_storm_approach` | the **warning/approach** phase, not the arrival |
| `incident_bunker_breach` (armed assault) | `incident_perimeter_breach_attempt` | covert **tampering** — tool marks, no contact |
| `incident_water_contamination` (purifier breach) | `incident_contaminated_water_table` | **upstream ground-water** contamination, equipment blameless |
| `incident_radio_interference` (ghost numbers station) | `incident_local_signal_intercept` | **nearby scheduled transmitter**, direction-finding |
| `incident_ambush_sector_4` | — | no overlap authored |

All pinned by `New_incidents_do_not_duplicate_original_semantics`.

## Deviations from the plan

1. **Assumption:** incidents fire from a shelter-tick scheduler. **Evidence:** the only consumer is
   `EventsHostSession`, a read-model with no tick hook, RNG, or state. **Adaptation:** Plan 57
   executed as pure data expansion (the plan's own Case D path); selection/cadence rules are
   authored as *future-scheduler* data (weights, minDays) and documented, not runtime.
2. **Assumption:** `category`, `maxDay`, faction/system links are schema fields. **Evidence:** the
   DTO binds five fields; unknown fields are silently ignored. **Adaptation:** categories and links
   are pinned in tests + this report instead of dead JSON (§33, §47).
3. **Assumption:** faction-linked incidents carry `faction_*` data refs. **Evidence:** no faction
   field exists. **Adaptation:** faction presence is grounded in body prose using real lore names.
4. **Assumption:** cache/supply incidents grant items. **Evidence:** no consequence mechanism
   exists. **Adaptation:** authored as narrative discovery events; reward wiring is follow-on work
   alongside the choice schema.

## Verification

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # PASS 0/0
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # PASS 6,558/6,558 (12 new Plan-57)
dotnet build Ashfall.csproj                                 # PASS 0 errors
godot --headless -- --data-integrity-selftest               # PASS 0 findings / 208 catalogs
godot --headless -- --content-utilization-selftest          # PASS
godot --headless -- --bridge-selftest                       # PASS exit 0
```

## Follow-on (choice-ready concepts, §56/§31)

When an incident scheduler + consequence grammar land: weighted selection over the existing
`weight` field, `maxDay`/category fields, cooldown/once-only history (Save contract §37), 2–3
choice responses for suitable incidents (storm sealing, filter replacement, pipe valve, patrol
response), and consequence dispatch into the existing power/water/medical/morale/faction/door
authorities. The 25-entry catalog and its phase/weight profile were authored to drop directly into
that runtime without content rework.
