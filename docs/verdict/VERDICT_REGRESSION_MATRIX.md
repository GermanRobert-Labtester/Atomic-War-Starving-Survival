# Verdict Location Regression & Acceptance Matrix

> **Scope:** Verification matrix covering all 15 investigation sites across build, unit test, data integrity, content utilization, and scene validation gates.

---

## 1. Automated Gate Results

```text
Plan 82 — Final Regression

Build:
- dotnet build Ashfall.csproj: PASS (0 errors, 0 warnings)
- dotnet build Ashfall.Core.Tests.csproj: PASS (0 errors, 2 informational warnings)

Tests:
- dotnet test Ashfall.Core.Tests: PASS (6,782+ passed, 0 failed, 0 skipped)
- Verdict-specific tests: 118 passed (including 10 new tests in Plan82VerdictLocationsExpansionTests)

Data integrity:
- godot --headless --path . -- --data-integrity-selftest: PASS (0 findings across 208 catalogs, 10,738 IDs authored)
- godot --headless --path . -- --content-utilization-selftest: PASS (CI gate PASS)
- godot --headless --path . -- --scene-binding-selftest: PASS (22/22 production panels passed)
- python3 scripts/ci/scene-lint.py: PASS (27 production scenes checked, 0 errors)

Baseline:
- sites before: 4
- sites added: 11
- sites after: 15

Schema:
- id: string (loc_* canonical format, globally unique)
- displayName: string (human-readable title)
- description: string (3-6 dense, grounded sentences)
- dangerLevel: int (5 to 9 scale)
- travelHours: float (4.5 to 9.0 hours)
- baseRadsPerHour: float (26.0 to 48.0 rad/h)
- optional fields: none (clean DTO alignment)

Investigation linkage:
- explicit arc field: none (preserved emergent authority)
- quest authority: VerdictQuestCatalogLoader / EvidenceLedger
- evidence authority: physical clues embedded in location prose
- NPC linkage: verdict_npcs.json traces and dialogue
- radio linkage: verdict_radio.json frequencies and carrier tones
- visit-state authority: visited_locations set in VerdictSave

Arcs:
- Tempest Array (Arc 1): 4 sites (geophone pit, twelve-gauge array, fuse world, tape silo)
- Coastal Survey (Arc 2): 4 sites (tide gauge, met station, cliff bunker, marine lab)
- Interior Caches (Arc 3): 4 sites (forestry post, core vault, river gauge, agronomy station)
- Border Wire (Arc 4): 3 sites (signal relay, checkpoint ruins, observation tower)

New locations:
- tide gauge: loc_abandoned_tide_gauge (Greywater Tide Gauge Station)
- coastal met station: loc_coastal_meteorological_station (Cape Wrath Meteorological Station)
- cliff observation bunker: loc_clifftop_observation_bunker (North Cliff Observation Bunker)
- marine lab: loc_sealed_marine_laboratory (St. Jude Marine Laboratory)
- forestry post: loc_forestry_survey_post (Blackwood Forestry Survey Post)
- core-sample vault: loc_geological_core_vault (Highland Core-Sample Repository)
- river gauge: loc_river_gauging_station (Karsk River Gauging Station)
- agricultural station: loc_abandoned_agricultural_station (Valley Experimental Agronomy Station)
- signal relay: loc_decommissioned_signal_relay (Pass 4 Signal Relay Mast)
- checkpoint ruins: loc_border_checkpoint_ruins (Gate Seven Border Checkpoint)
- observation tower: loc_minefield_observation_tower (Pylon 19 Observation Post)

Description quality:
- generic prose failures: 0
- evidence missing: 0
- contradiction missing: 0
- outward clue missing: 0
- technical plausibility issues: 0
- repeated motifs: 0

Continuity:
- chronology: pre-war to Year 5 aligned
- personnel: Eden Vale, Ferris Voss, Iaran Bell, Selya Saltmarsh integrated
- procurement/equipment: Department of the Interior and Tempest stamps unified
- geography: coast, interior valley, and alpine pass consistent with world map
- Tempest contradictions: all 4 original contradictions preserved
- deliberate unresolved ambiguities: 5 cross-arc clues intentionally unclosed

NPCs:
- site-linked NPCs: 6
- invalid refs: 0
- witness state: preserved in VerdictNpcSystem
- duplicate encounters: 0

Radio:
- site-linked broadcasts: 8
- invalid refs: 0
- duplicate unlocks: 0
- Verdict/faction-radio separation: 100% maintained

Expedition:
- site 1: loc_clifftop_observation_bunker (North Cliff Observation Bunker)
- site 2: loc_border_checkpoint_ruins (Gate Seven Border Checkpoint)
- shared IDs: exact string match
- arrival arbitration: single visit event, zero double-counting
- save/reload: clean round-trip

Numeric balance:
- danger range: 5 to 9
- travel range: 4.5 to 9.0 hours
- radiation range: 26.0 to 48.0 rad/h
- outlier sites: 0 (well distributed across all arcs)
- combined burden: balanced between travel, danger, and radiation
- campaign time: ~103 hours total exploration cost for all 15 sites
- campaign radiation: sustainable with standard medical preparation

Radiation:
- unit semantics: chronic rads/hour
- dose crosscheck: decoupled from Plan 81 microSieverts
- double-counting: prevented
- Plan 81 compatibility: verified

Save:
- old Tempest save: 100% backward compatible
- existing completed arc: preserved intact
- new site: correctly recorded in visited_locations
- multi-arc: independent progress tracking
- NPC/radio: one-shot flags preserved
- expedition-linked site: clean cross-system persistence

Determinism:
- site lookup: stable dictionary indexing
- event trigger: seeded PRNG preserved
- seeded NPC/radio behavior: deterministic

Content utilization:
- unreachable sites: 0
- orphan refs: 0
- unused evidence: 0
- staged content: verified

UI/accessibility:
- 15-site list: scrollable in Verdict map panel
- arc readability: high contrast, clear headers
- long descriptions: wrapping verified without clipping
- text scaling: supports 1920x1080 fixed UI
- localization: stable snake_case IDs ready for translation tables

Exported build:
- catalog packaged: builds/linux/Assets/StreamingAssets/Data/verdict_locations.json synced
- site lookup: case-sensitive verified
- visit: functional
- save/reload: clean round-trip

Manual investigation:
- PASS

Deferred:
- Plan 84 witness expansion: testimony integration for Plan 84
- additional radio broadcasts: queued for follow-up radio expansions
- bespoke site art: 2D illustration generation
- VO/SFX: audio line recordings
```
