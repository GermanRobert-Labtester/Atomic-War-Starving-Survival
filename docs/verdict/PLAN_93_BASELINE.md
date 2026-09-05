# Plan 93 — Verdict NPCs Expansion Baseline & Forensics

> **Catalog Authority:** `Assets/StreamingAssets/Data/verdict_npcs.json`
> **Core Loader & Runtime:** `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`
> **Persistence Authority:** `Assets/Ashfall.Core/Verdict/VerdictSave.cs`
> **Location Authority:** `Assets/StreamingAssets/Data/verdict_locations.json` (Plan 82)

---

## 1. Verified Baseline Reconnaissance

### 1.1 Catalog State
- Prior to Plan 93, `verdict_npcs.json` contained **9 verified NPC definitions** (`schema_version: 1`).
- The 6 original baseline entries were authored in the initial Verdict expansion:
  1. `npc_eden_vale` (Amateur radio operator, comm-array bleed)
  2. `npc_ferris_voss` (Fire-control acceptance engineer, last human in the fuse world)
  3. `npc_iran_bell` (Tempest maintenance supervisor, the valve-touch hand)
  4. `npc_selya_saltmarsh` (Census clerk, the only human with an opinion about the count)
  5. `npc_maro_veen` (The machine's own voice — the census-window tape loop)
  6. `npc_whisper_cipher` (The relay network's aggregate readings — univocal, procedural)
- Plan 18 (commit `7738facc`) added 3 defense/tribunal clerk entries to deepen courtroom procedure:
  7. `npc_tomas_reid` (Defense clerk, tribunal appeals and admissibility)
  8. `npc_elena_vane` (Machine-cult deaconess, Voice of the Standard)
  9. `npc_kasper_holt` (Chief Archival Custodian, chain of custody keeper)
- Plan 93 expands this catalog with **9 dedicated investigation-site NPCs** to populate all expanded Plan 82 sites, bringing the total to **18 distinct NPCs**.

### 1.2 Accepted Kind Taxonomy
Documented in `VerdictNpcSystem.cs` (line 15):
- `tape_echo`: Preserved audio, telemetry recording, looped automated transmission.
- `paper_ghost`: Written logs, annotated charts, linen shift charters, margin notes.
- `living`: Surviving specialist, active clerk, resident deaconess.
- `readings`: Automated procedural instruments, relay telemetry, signal bursts.

### 1.3 Selector & Eligibility Semantics
`VerdictNpcSystem.GetAvailable(IReadOnlyCollection<string> setFlags, int phase, string? locationId = null)`:
```csharp
if (e.phaseMin > 1 && phase < e.phaseMin) continue;
if (!string.IsNullOrEmpty(e.gatingFlag) &&
    (setFlags == null || !ContainsFlag(setFlags, e.gatingFlag))) continue;
if (!string.IsNullOrEmpty(locationId) && e.locationId != locationId) continue;
```
- **Phase comparison:** `phase < e.phaseMin` excludes entries. When `phase >= e.phaseMin`, the phase check passes.
- **Flag comparison:** Case-insensitive check via `StringComparison.OrdinalIgnoreCase`.
- **Location filtering:** Exact match against `e.locationId`.
- **Statelessness:** Availability is dynamically derived; no seen-state or availability state is persisted. Only `spokenNpcIds` is persisted in `VerdictNpcState`.
- **One-shot speech:** `Speak(npcId)` records the ID in `spokenNpcIds` and returns `false` on subsequent calls.

---

## 2. Plan 82 Location Coverage Context

Plan 82 established 15 investigation sites in `verdict_locations.json`.
The 9 new investigation-site NPCs directly provide human residue for the 9 newly authored physical installations:
- Coastal Survey (4 sites): `loc_abandoned_tide_gauge`, `loc_coastal_meteorological_station`, `loc_clifftop_observation_bunker`, `loc_sealed_marine_laboratory`.
- Interior Caches (4 sites): `loc_forestry_survey_post`, `loc_geological_core_vault`, `loc_river_gauging_station`, `loc_abandoned_agricultural_station`.
- Border Wire (1 site): `loc_decommissioned_signal_relay`.
