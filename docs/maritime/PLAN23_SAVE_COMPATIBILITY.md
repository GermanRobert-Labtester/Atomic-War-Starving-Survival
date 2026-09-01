# Plan 23 — Save Compatibility

## Derived versus persisted maritime state

| State | Owner | Derived/Persisted | Save section | Old-save default | Migration rule |
|---|---|---|---|---|---|
| Dive active state (air, room, noise, decompression) | `MaritimeDiveSystem` | Persisted | `maritime` (`MaritimeHostSave.Dive`) | inactive dive | unchanged |
| Dive outcomes history | MaritimeDiveSystem | Persisted | `maritime` | empty | additive |
| Site explored flags | MaritimeDiveSystem | Persisted | `maritime` | false | unchanged for old sites |
| Safe instances (opened/jammed/lootTransferred) | `SafeCrackingSystem` | Persisted | `maritime` (`SafeCrack`) | no safes registered | combinations re-derived from seed+safeId; open/jam/looted flags additive |
| Procedural scavenge visit counts | ProceduralScavengeSystem | Persisted | `maritime` | empty | unchanged |
| Psychological contamination | PsychologicalContaminationSystem | Persisted | `maritime` | empty | site-scoped keys are additive dictionary keys |
| Tide phase | `TideCalendar` | **Derived** (campaign day) | — (never serialized) | n/a | pure function; no drift possible |
| Storm surge active/day | `District8DeepCoastSystem` | Persisted | deep-coast state (HoldfastSave envelope) | `surgeActiveDay=-1`, `surgeLastStormDay=-1` | additive fields; missing → no surge |
| Surge aftermath map events | `WorldEvolutionEngine` | Persisted (triggered-event registry) | world save | none triggered | day+flag gated, no fabricated history |
| Flotilla standing | `FactionStanceEngine` (host-registered) | Persisted by host stance owner | economy/host sections | 0 (Tolerated) | never fabricated |
| Discovered Flotilla NPCs / radio bands / items | catalogs | n/a (registry data) | — | available immediately | no fabricated state |
| Radio delivery history | `FactionRadioEngine` (stateless selection) | Derived per (faction, kind, day, seed) | n/a | n/a | deterministic, no persisted delivery state |
| Variable loot resolution | SafeCrackingSystem / ProceduralScavengeSystem | Persisted (per resolved node) | `maritime` | unresolved | resolved nodes never reroll |

## Old-save invariants (all test-pinned)

- Pre-Plan-23 campaigns load without fabricated Flotilla standing (restores at 0 → Tolerated).
- No duplicate NPCs, no invalid dive-site ids, no rerolled active dives or safes.
- Catalog additions (items, NPCs, broadcasts, sites, currents audit, texts) require no
  fabricated historical state — old saves load with additive content available forward.
- `MaritimeHostSave` shape unchanged (Dive/Scavenge/Psychology/SafeCrack + checksum);
  new Plan 23 site data lives in the catalog, not the save.
