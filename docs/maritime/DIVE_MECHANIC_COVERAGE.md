# Dive Mechanic Coverage (Plan 23 / Task 23B)

Live maritime systems audited against every site. Baseline finding (Phase 1): the
runtime had **zero authored consumers** for safes, site loot tables, contamination,
gear gates, and the catalog room grammar (the host seeded 4 hardcoded rooms and 4
hardcoded loot nodes). Plan 23 closes those gaps with authored data — no new runtimes.

## Mechanic coverage per site (L = live consumer, — = not authored for this site)

| Site | Catalog rooms drive dive | Gear gate | Safe runtime | Contamination | Site loot table | Fleet hook | Repeatability |
|---|---|---|---|---|---|---|---|
| ss_sovereign | L | — | ✅ d4 | — | ✅ trade cargo | keeper thread | scavenge + safe |
| ferry_terminal | L | — | — | — | ✅ | — | scavenge |
| barge_flotilla | L | — | — | — | ✅ | Salvage Fleet home | scavenge |
| naval_patrol | L | — | — | ✅ stare | ✅ | Escort war grave | scavenge |
| sunken_submarine | L | sealed dive lamp | — | thousand-yard stare | ✅ | Deep Fleet | scavenge |
| flooded_metro | L | — | — | — | ✅ | — | scavenge |
| submerged_convoy | L | — | — | — | ✅ | — | scavenge |
| drowned_fuel_depot | L | cutting tool | — | — | ✅ | fuel gate discipline | scavenge |
| offshore_relay | L | — | — | — | ✅ | relay/codekeeper | scavenge |
| flooded_field_hospital | L | — | — | stare + phantom smell | ✅ | medical | scavenge |
| wrecked_patrol_craft | L | — | — | stare | ✅ | war grave (bell Relic) | scavenge |
| submerged_siphon | L | — | — | — | ✅ | Hydro-Barons | scavenge |
| payroll_strongroom | L | salvage cutting tool | ✅✅ d5+d3 | — | ✅ | payroll/Office | scavenge + safes |
| brine_cistern | L | rebreather canister | — | disgust cascade + phantom smell | ✅ | abort-decision dive | scavenge |

## Coverage targets (Plan 23E audit basis)

- **Safe cracking: 3 sites** (sovereign purser, picket armory, payroll strongroom — 2
  required, 3 delivered), all through the real `SafeCrackingSystem` (registered from
  catalog definitions, combination derived from seed+id, open/jam/loot persisted).
- **Psychological contamination: 4 sites** (quarantine barge, brine cistern, Barrik,
  picket craft) — grounded effects only (stare, phantom smell, disgust cascade);
  site keys never attach to walkable locations (test-pinned).
- **Stealth/noise: all 14** via the dive noise ladder; `base_noise_floor` recorded on
  site start for the storm-masking model.
- **Gear gates: 2 hard gates** (cutting tool, rebreather canister) + recommended gear;
  no all-or-nothing gating of the shallow tier (metro/ferry/barge need nothing).
- **Variable/procedural loot: all 14 sites** (visit decay + world-phase degradation).
- Air pressure: catalog oxygen budgets (70–120) seed the dive machine per site.
- Tide windows: authored in Task 23C (see `TIDE_WINDOW_MATRIX.md`).

## Deliberately not duplicated

No second air/noise engine (MaritimeDiveSystem owns state), no site-local sanity
counters (PsychologicalContaminationSystem owns effects), no bespoke safe logic
(SafeCrackingSystem owns combinations), no site-id special cases in code (all
behavior is data-driven through the optional site fields).
