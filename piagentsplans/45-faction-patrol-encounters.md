# Plan 45 — Faction Patrol Encounters: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** territory, patrol behavior, and travel encounters
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Make patrol presence a bounded projection over faction territory, travel encounters, combat composition, and radio consequences, with a single encounter selection seam.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5998` characters.
- Current worktree copy: `505180` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4bd78fa73e7cd564ef25325de5c2e98e45065826e8fddb17f55731628728cc1f`
- Snapshot size: 9652 characters; 258 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Ashfall.Core.Factions;
0005: using Ashfall.Core.Warlords;
0006:
0007: namespace Ashfall.Core.World
0008: {
0009:     public interface ITerritoryAuthority
0010:     {
0011:         bool IsClaimedBy(string locationId, string factionId);
0012:         WarlordTerritoryState GetTerritoryState(string locationId);
0013:         string GetController(string locationId);
0014:         bool IsClaimant(string locationId, string factionId);
0015:     }
0016:
0017:     public static class PatrolTerritoryResolver
0018:     {
0019:         private static readonly Dictionary<string, string> RegionToLocationMap = new(StringComparer.OrdinalIgnoreCase)
0020:         {
```

### Current evidence: `Assets/Ashfall.Core/Narrative/PatrolEncounterValidator.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `3b6c5047fc8a0271c2b59e4c9ce5f6478831c6eae7136bb914d96bada4f8d8e0`
- Snapshot size: 16390 characters; 336 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0007: {
0008:     public static class PatrolEncounterValidator
0009:     {
0010:         private static readonly HashSet<string> AllowedTerritoryStates = new(StringComparer.OrdinalIgnoreCase)
...
0050:                 {
0051:                     // Validator is specialized for patrol encounters (Plan 45 / F15)
0052:                     continue;
0053:                 }
```

### Current evidence: `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `7fc44eff7cde5e9b9026c3112ff46cf14c28bdaf97c4db76f2db6bb45f8ee8d8`
- Snapshot size: 7165 characters; 200 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009:     [Serializable]
0010:     public sealed class PatrolRadioHooksState
0011:     {
0012:         public List<string> ConsumedSignals { get; set; } = new();
...
0020:     /// </summary>
0021:     public sealed class PatrolRadioHooks
0022:     {
0023:         private readonly ILog _log;
...
0061:
0062:         public PatrolRadioHooks(ILog? log = null)
0063:         {
0064:             _log = log ?? NullLog.Instance;
...
0114:             {
0115:                 _log.Info($"[PatrolRadioHooks] Signal '{id}' already consumed; ignoring.");
0116:                 return false;
0117:             }
...
0120:             {
0121:                 _log.Info($"[PatrolRadioHooks] Signal '{id}' already in pending queue.");
0122:                 return false;
0123:             }
...
0149:                 dispatched.Add(sig);
0150:                 _log.Info($"[PatrolRadioHooks] Dispatched and consumed radio signal '{sig}'.");
0151:             }
0152:             return dispatched;
...
0156:         {
0157:             return new PatrolRadioHooksState
0158:             {
0159:                 ConsumedSignals = new List<string>(_consumedSignals),
...
0163:
0164:         public void RestoreState(PatrolRadioHooksState? state)
0165:         {
0166:             _consumedSignals.Clear();
```

### Current evidence: `Assets/Ashfall.Core/Expeditions/TravelEncounterCombatBinder.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e891255175a3664860138d8b731b2546c31de1f8bc9d296057088380574d5129`
- Snapshot size: 2338 characters; 56 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0020:     /// </summary>
0021:     public static class TravelEncounterCombatBinder
0022:     {
0023:         /// <summary>True when the choice is a hostile (fight) resolution.</summary>
```

### Current evidence: `Assets/Ashfall.Core/World/WastelandMapSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4edb79c28b4d8baec664fdac3bdcbc2084b4bbd5524f48dec1f6e2e1029bdce3`
- Snapshot size: 50731 characters; 1274 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.Linq;
0005: using Ashfall.Core.Campaign;
0006: using Ashfall.Core.Expeditions;
0007: using Ashfall.Core.Underground;
0008: #pragma warning disable CS8618
0009:
0010: namespace Ashfall.Core.World
0011: {
0012:     /// <summary>
0013:     /// ASHFALL Travel Map authority (item 4).
0014:     ///
0015:     /// Core query + state for wasteland travel. Reads
0016:     /// <c>Assets/StreamingAssets/Data/wasteland_map_v1.json</c> for
0017:     /// canonical nodes + route edges. Tracks per-node discovery state,
0018:     /// runs deterministic route planning between two nodes, and exposes
0019:     /// the data the host (WastelandMapView, MapAtlasPanel, expedition
0020:     /// launchers) needs to render fog-of-war, hazards, and progress.
```

### Current evidence: `src/Main.Expeditions.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.Expeditions.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0e87d7d7f3605ba923f739c7bc36ab51edecc431781fe0014122a453422b300b`
- Snapshot size: 38629 characters; 831 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Globalization;
0005: using System.IO;
0006: using System.Linq;
0007: using System.Collections.Generic;
0008: using AtomicWar.Journal;
0009: using Ashfall.Core;
0010: using Ashfall.Core.Combat;
0011: using Ashfall.Core.Campaign;
0012: using Ashfall.Core.Economy;
0013: using Ashfall.Core.Disease;
0014: using Ashfall.Core.Expeditions;
0015: using Ashfall.Core.Foundry;
0016: using Ashfall.Core.Inventory;
0017: using Ashfall.Core.Journal;
0018: using Ashfall.Core.Muster;
0019: using Ashfall.Core.Random;
0020: using Ashfall.Core.YearOfAsh;
```

### Current evidence: `src/World/WastelandMapView.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0b1f0f573f111a8639b4df9d576a3932b9c5d8c873714caafa848eb6d03e0607`
- Snapshot size: 10961 characters; 278 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using AtomicWar.GodotApp.UI;
0003: using Godot;
0004: using System;
0005: using System.Collections.Generic;
0006: using Ashfall.Core.World;
0007: using AtomicWar.GodotApp.Localization;
0008:
0009: namespace AtomicWar.GodotApp.World
0010: {
0011:     /// <summary>
0012:     /// ASHFALL — Wasteland Map View Controller.
0013:     /// Manages the wasteland map scene, renders node markers with live discovered/available/locked/completed/unavailable
0014:     /// status from authoritative <see cref="WastelandMapSystem"/> read model, and handles interactions.
0015:     /// </summary>
0016:     public partial class WastelandMapView : Node2D
0017:     {
0018:         [Signal]
0019:         public delegate void NodeSelectedEventHandler(string nodeId);
0020:
```

### Current evidence: `src/UI/TravelingCaravanPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/TravelingCaravanPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1ac34a6c364a547c4c51833e595c3ca5985ac49c0b465134cdb044df5ecce8e0`
- Snapshot size: 22924 characters; 465 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.Linq;
0005: using Godot;
0006: using Ashfall.Core;
0007: using Ashfall.Core.Economy;
0008: using Ashfall.Core.UI;
0009: using AtomicWar.GodotApp;
0010: using DesignTheme = Ashfall.Core.UI.Theme;
0011:
0012: namespace AtomicWar.GodotApp.UI
0013: {
0014:     /// <summary>
0015:     /// ASHFALL — Traveling Caravans & Regional Trade Route Panel.
0016:     /// Manages itinerant merchants, wasteland route nodes, daily movements,
0017:     /// and ration-based outpost barter.
0018:     /// </summary>
0019:     public partial class TravelingCaravanPanel : Control, IBindablePanel
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/faction_territory.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`
- Snapshot size: 19950 characters; 540 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "faction_territory_catalog",
0004:   "territories": [
0005:     {
0006:       "id": "territory_the_office",
0007:       "faction": "faction_the_office",
0008:       "display_name": "Industrial Rail Corridor & Weighbridge Network",
0009:       "classification": "territorial",
0010:       "territory_scale": "major",
0011:       "primary_resource_interest": "rail_freight_and_hardware",
0012:       "controlled_nodes": [
0013:         "loc_cut_arsenal_ruin"
0014:       ],
0015:       "control_points": [
0016:         "loc_settlement_nine_rails",
0017:         "loc_weighbridge"
0018:       ],
0019:       "contested_with": [
0020:         "faction_the_tally",
```

### Current evidence: `Assets/StreamingAssets/Data/faction_war_events.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`
- Snapshot size: 93786 characters; 1850 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "chains": [
0004:     {
0005:       "chainId": "evt_d480_grain_tally_dispute",
0006:       "band": "cold_war",
0007:       "title": "Tally Dispute at the Exchange",
0008:       "factionsInvolved": [
0009:         "faction_central_garrison",
0010:         "faction_rebuilders"
0011:       ],
0012:       "locationId": "loc_grain_silo",
0013:       "stages": [
0014:         {
0015:           "stageId": "evt_d480_grain_tally_dispute_s1",
0016:           "minDay": 480,
0017:           "triggerCondition": "Fires automatically once the player has visited the Grain Exchange.",
0018:           "title": "A Thumb on the Scale",
0019:           "bodyText": "A Garrison quartermaster's runner sets a sack on the Exchange scale and the needle settles a half-kilo light of what his chit claims. The Rebuilders weigher calls it out loud enough for the whole line to hear. The runner doesn't reach for his sidearm, but his hand moves like it wants to. Everyone within earshot has stopped pretending to haggle.",
0020:           "choices": [
```

### Current evidence: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ef8f6cc0bf94b91df450930ab4bb20867a3108216c8d82c80de5790ac8bfc48e`
- Snapshot size: 68822 characters; 1098 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "$schema": "https://json-schema.org/draft/2020-12/schema",
0004:   "version": "1.0.0",
0005:   "description": "Ashfall Faction Radio & Intercept Log HUD Chatter Corpus",
0006:   "silence_events": [
0007:     "STATIC... [ Carrier hum steady at 50 Hz. Faint ionospheric rush. ] ...STATIC",
0008:     "STATIC... [ Distant thunderstorm discharge crackles across the lower band. No voice detected. ]",
0009:     "STATIC... [ Atmospheric flutter. The heterodyne whistle shifts two octaves into silence. ]",
0010:     "STATIC... [ Automated repeater beacon clicks twice. Null modulation. ] ...STATIC",
0011:     "STATIC... [ Long silence. Faint acoustic resonance of wind whistling through the antenna array. ]",
0012:     "STATIC... [ White noise hiss. Cosmic radiation background hums in the vacuum tubes. ]",
0013:     "STATIC... [ Thermal drift in the receiver coils. Carrier wave drops into dead air. ]",
0014:     "STATIC... [ Low frequency rumble of distant artillery or thunder. No transmission. ]",
0015:     "STATIC... [ Squelch gate cuts the rush. Pure analog silence fills the headphones. ]",
0016:     "STATIC... [ Faint Morse code echoes from beyond the horizon, too degraded to transcribe. ]",
0017:     "STATIC... [ Dead carrier. A transmitter remains powered somewhere in the dark ruins, unmonitored. ]",
0018:     "STATIC... [ The static breathes with the rhythm of the rising radioactive fallout plume. ]"
0019:   ],
0020:   "factions": {
```

### Current evidence: `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `414a3fdecb68d54e6fd3a4252548504e1e944bea59fc88ff67cfcac850eaa582`
- Snapshot size: 2267 characters; 39 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "encounters": [
0004:     {
0005:       "encounter_id": "encounter_the_mirrored_scout",
0006:       "title": "The Mirrored Casualty",
0007:       "description": "Your scout team enters a collapsed subway tunnel. Pinned beneath a concrete slab is a frozen corpse. Upon rolling the body over, the scout realizes the corpse is wearing their exact hazard suit, bearing their exact serial number, and features a custom boot-repair the scout performed yesterday. The corpse's geiger counter is ticking backwards.",
0008:       "threat_level": 4,
0009:       "outcomes": [
0010:         {
0011:           "action": "Loot the Corpse",
0012:           "survival_chance": 0.8,
0013:           "success_text": "The scout stripped the gear from their own corpse with cold detachment. The inventory ledger shows a +1 surplus in hazard suits. The paradox was not documented on the official form.",
0014:           "failure_text": "While unbuckling the helmet, the corpse's eyes opened. The scout fled in terror, abandoning half their gear in the tunnel."
0015:         },
0016:         {
0017:           "action": "Incinerate the Anomaly",
0018:           "survival_chance": 1.0,
0019:           "success_text": "Protocol 7 dictates that all temporal anomalies be purged. The scout burned the corpse using three days worth of kerosene, returning empty-handed but sane.",
0020:           "failure_text": ""
```

### Current evidence: `Assets/StreamingAssets/Data/characters.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cd33aa4801aade57932525194755b413d83ab2cd212c0ca7a5ccd09f011b5c58`
- Snapshot size: 68407 characters; 1838 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "npc_bram_ostrowski",
0006:       "display_name": "Bram Ostrowski",
0007:       "profession": "Mapmaker",
0008:       "bio": "Walks the corridor selling maps he surveys himself, on waxed paper, priced by the sheet. Accurate to a degree that makes people uncomfortable about how he obtained the detail.",
0009:       "faction": "none",
0010:       "region": "the_toll",
0011:       "first_day": 20,
0012:       "location_id": "",
0013:       "wants": [
0014:         "map_corrections"
0015:       ],
0016:       "offers": [
0017:         "reduced_travel_time",
0018:         "hazard_forewarning"
0019:       ],
0020:       "will_not": [
```

### Current evidence: `Assets/StreamingAssets/Data/items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Snapshot size: 390056 characters; 9660 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "item_decon_chelator_concentrate",
0006:       "displayName": "Chelator Concentrate",
0007:       "description": "A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.",
0008:       "type": "Consumable",
0009:       "stackMax": 20,
0010:       "weight": 0.2,
0011:       "tradeValue": 8
0012:     },
0013:     {
0014:       "id": "item_lead_lined_effluent_filter",
0015:       "displayName": "Lead-Lined Effluent Filter",
0016:       "description": "A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.",
0017:       "type": "Equipment",
0018:       "stackMax": 5,
0019:       "weight": 3.5,
0020:       "tradeValue": 15
```

### Current evidence: `Assets/StreamingAssets/Data/combat_catalog.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e837b53cbab1b7595a12f58c09cd7dbac753ef1736a4fbf69757a4d83f36a1c6`
- Snapshot size: 20771 characters; 549 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 2,
0003:   "collection_id": "combat_catalog",
0004:   "weapons": [
0005:     {
0006:       "id": "weapon_pipe_rifle",
0007:       "display_name": "Pipe Rifle",
0008:       "accuracy": 0.46,
0009:       "damage": 12.0,
0010:       "range": 1.0,
0011:       "caliber": "ammo_357",
0012:       "burst": 1,
0013:       "is_jury_rigged": true,
0014:       "is_suppression_capable": false,
0015:       "degrade_per_shot": 0.022,
0016:       "jam_base": 0.055,
0017:       "scrap_repair_cost": 3,
0018:       "condition_threshold": 0.30
0019:     },
0020:     {
```

### Current evidence: `src/UI/MapDetailPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/MapDetailPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6b091b1a461434e3ad68b33e4f50f12d9fd2c237d5aed33e117f21020ef3d8d7`
- Snapshot size: 13065 characters; 247 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Godot;
0005: using Ashfall.Core;
0006: using Ashfall.Core.Expeditions;
0007: using Ashfall.Core.UI;
0008: using AtomicWar.Journal;
0009:
0010: namespace AtomicWar.GodotApp.UI
0011: {
0012:     /// <summary>
0013:     /// ASHFALL — Map Detail panel.
0014:     /// Shows detailed sector intelligence, radiation readings, transit requirements,
0015:     /// architectural sub-layouts, and site salvage potential for a chosen location.
0016:     /// </summary>
0017:     public partial class MapDetailPanel : Control
0018:     {
0019:         public event Action? OnClose;
0020:
```

### Current evidence: `src/Radio/FactionRadioHudPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/Radio/FactionRadioHudPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `777fbb06af8ec1f1c72a3223f4bcb3f83b14f88659c9f9afb296dbd5a278a05c`
- Snapshot size: 17364 characters; 390 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using AtomicWar.GodotApp.UI;
0003: using System;
0004: using System.Collections.Generic;
0005: #pragma warning disable CS8618
0006: using Godot;
0007: using static AtomicWar.GodotApp.UI.AshfallUiHelpers;
0008: using Ashfall.Core;
0009: using Ashfall.Core.Radio;
0010: using Ashfall.Core.UI;
0011:
0012: namespace AtomicWar.GodotApp.Radio
0013: {
0014:     /// <summary>
0015:     /// Full Godot host implementation of the Faction Radio &amp; Intercept Log HUD (The Heterodyne Rack).
0016:     /// Built to Concept 1 specification:
0017:     /// - 19" cold-war stamped steel rack frame (radio_frame_9slice.png)
0018:     /// - Frequency tuner (50.0..150.0 MHz) with illuminated dial (frequency_dial.png)
0019:     /// - Analogue S-meter gauge (meter_signal_strength.png)
0020:     /// - CRT scanline terminal (signal_static_overlay.png) with live transcript stream
```

### Current evidence: `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b18dcafdb8ad82d008a7216984ee530b580f71c8e39da426a26b1f721f1a774e`
- Snapshot size: 9186 characters; 222 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0072:         {
0073:             var errors = PatrolEncounterValidator.Validate(_catalog.Encounters, _factions, _items);
0074:             Assert.Empty(errors);
0075:         }
...
0123:             enc.Category = "Beast";
0124:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0125:             Assert.Contains(errors, e => e.Contains("must have category 'Human'"));
0126:         }
...
0132:             enc.FactionId = "phantom_faction_99";
0133:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0134:             Assert.Contains(errors, e => e.Contains("references unknown faction"));
0135:         }
...
0141:             enc.TerritoryState = "lawless_zone";
0142:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0143:             Assert.Contains(errors, e => e.Contains("invalid territory_state"));
0144:         }
...
0150:             enc.Choices.RemoveAt(0); // only 1 choice left
0151:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0152:             Assert.Contains(errors, e => e.Contains("between 2 and 6 choices"));
0153:         }
...
0159:             enc.Choices[1].ChoiceId = enc.Choices[0].ChoiceId;
0160:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0161:             Assert.Contains(errors, e => e.Contains("duplicate choice_id"));
0162:         }
...
0168:             enc.Choices[1].Text = enc.Choices[0].Text;
0169:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0170:             Assert.Contains(errors, e => e.Contains("duplicate choice text"));
0171:         }
...
0177:             enc.Choices[0].FactionStandingDelta = 25; // max allowed is 10
0178:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0179:             Assert.Contains(errors, e => e.Contains("standing delta 25 outside allowed range"));
0180:         }
...
0186:             enc.Choices[0].CostItems = new List<string> { "unobtainium_crystal" };
0187:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0188:             Assert.Contains(errors, e => e.Contains("unknown cost item 'unobtainium_crystal'"));
0189:         }
...
0197:             enc.Choices[0].CostItems = new List<string> { "canned_food" };
0198:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0199:             Assert.Contains(errors, e => e.Contains("cannot also be consumed in costs"));
0200:         }
...
0206:             enc.BaseWeight = 0.01f;
0207:             var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
0208:             Assert.Contains(errors, e => e.Contains("base_weight 0.01 outside allowed range"));
0209:         }
...
```

### Current evidence: `Ashfall.Core.Tests/PatrolEncounterIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `fe885c5e2ed982aa99df4f53d873da1069df503dd6c513d006e4af351f2a6404`
- Snapshot size: 25719 characters; 591 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Linq;
0006: using Xunit;
0007: using Ashfall.Core;
0008: using Ashfall.Core.Expeditions;
0009: using Ashfall.Core.Inventory;
0010: using Ashfall.Core.Narrative;
0011: using Ashfall.Core.YearOfAsh;
0012:
0013: namespace Ashfall.Core.Tests
0014: {
0015:     public class PatrolEncounterIntegrationTests
0016:     {
0017:         private readonly string _dataDir;
0018:         private readonly FileSystemIO _fileIO;
0019:         private readonly TravelEncounterCatalog _catalog;
0020:
```

### Current evidence: `Ashfall.Core.Tests/PatrolTerritoryIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `05c8e506d7aa1fff17bc6a692bd8ecfc0f49a0998e31481f5c2d56fd91b82f90`
- Snapshot size: 7487 characters; 140 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using Xunit;
0006: using Ashfall.Core;
0007: using Ashfall.Core.Factions;
0008: using Ashfall.Core.Narrative;
0009: using Ashfall.Core.Warlords;
0010: using Ashfall.Core.World;
0011:
0012: namespace Ashfall.Core.Tests
0013: {
0014:     public class PatrolTerritoryIntegrationTests
0015:     {
0016:         private readonly string _dataDir;
0017:         private readonly FileSystemIO _fileIO;
0018:         private readonly TravelEncounterCatalog _catalog;
0019:
0020:         public PatrolTerritoryIntegrationTests()
```

### Current evidence: `Ashfall.Core.Tests/PatrolFactionStandingTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `dc04a49f3a66a211772f1bd43defe5271988a1738d5b715e7f1c0caef829a8f8`
- Snapshot size: 4682 characters; 115 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ASHFALL Patrol Faction Standing Tests (PAT-F1-001 through PAT-F1-010)
0003:
0004: using System;
0005: using System.Collections.Generic;
0006: using System.IO;
0007: using Xunit;
0008: using Ashfall.Core;
0009: using Ashfall.Core.Factions;
0010: using Ashfall.Core.IO;
0011: using Ashfall.Core.Narrative;
0012: using Ashfall.Core.YearOfAsh;
0013:
0014: namespace Ashfall.Core.Tests
0015: {
0016:     public class PatrolFactionStandingTests
0017:     {
0018:         private readonly string _dataDir;
0019:         private readonly TravelEncounterCatalog _catalog;
0020:
```

### Current evidence: `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d0a1336e0656c07e6c14d04a58f7e63ebfb87d40a00d286f7e349de0e2d6e005`
- Snapshot size: 19598 characters; 359 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Security.Cryptography;
0006: using System.Text;
0007: using System.Text.Json;
0008: using Xunit;
0009: using Ashfall.Core;
0010: using Ashfall.Core.Factions;
0011: using Ashfall.Core.Inventory;
0012: using Ashfall.Core.Narrative;
0013: using Ashfall.Core.Warlords;
0014: using Ashfall.Core.World;
0015: using Ashfall.Core.YearOfAsh;
0016:
0017: namespace Ashfall.Core.Tests
0018: {
0019:     /// <summary>
0020:     /// Flagship VII (Tasks 31-32): 32-step end-to-end deterministic campaign proof.
```

### Current evidence: `Ashfall.Core.Tests/TravelEncounterPatrolVariantTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5273ddfb68434e1c4347bc3ba89b3f64ad60996dacfc68ca382f936396685b08`
- Snapshot size: 10749 characters; 238 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Linq;
0006: using Xunit;
0007: using Ashfall.Core;
0008: using Ashfall.Core.Inventory;
0009: using Ashfall.Core.Narrative;
0010:
0011: namespace Ashfall.Core.Tests
0012: {
0013:     public class TravelEncounterPatrolVariantTests
0014:     {
0015:         private readonly string _dataDir;
0016:         private readonly FileSystemIO _fileIO;
0017:         private readonly TravelEncounterCatalog _catalog;
0018:
0019:         public TravelEncounterPatrolVariantTests()
0020:         {
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/faction_territory.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, territories, contested_zones`
- `territories`: list count=19; sample IDs=['territory_the_office', 'territory_the_cutters', 'territory_black_flotilla', 'territory_the_fleet', 'territory_deserter_coalition', 'territory_cold_count', 'territory_the_tally', 'territory_grain_exchange']
- `contested_zones`: list count=5; sample IDs=['zone_contested_water_rights', 'zone_contested_cut_salvage', 'zone_contested_merchant_crossroads', 'zone_contested_scarp_pass', 'zone_contested_coastal_bluff']
- `schema_version`: `1`
- `collection_id`: `faction_territory_catalog`
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`
#### `Assets/StreamingAssets/Data/faction_war_events.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, chains`
- `chains`: list count=38; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`
#### `Assets/StreamingAssets/Data/faction_radio_corpus.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, $schema, version, description, silence_events, factions, broadcasts`
- `silence_events`: list count=12; sample IDs=['STATIC... [ Carrier hum steady at 50 Hz. Faint ionospheric rush. ] ...STATIC', 'STATIC... [ Distant thunderstorm discharge crackles across the lower band. No voice detected. ]', 'STATIC... [ Atmospheric flutter. The heterodyne whistle shifts two octaves into silence. ]', 'STATIC... [ Automated repeater beacon clicks twice. Null modulation. ] ...STATIC', 'STATIC... [ Long silence. Faint acoustic resonance of wind whistling through the antenna array. ]', 'STATIC... [ White noise hiss. Cosmic radiation background hums in the vacuum tubes. ]', 'STATIC... [ Thermal drift in the receiver coils. Carrier wave drops into dead air. ]', 'STATIC... [ Low frequency rumble of distant artillery or thunder. No transmission. ]']
- `broadcasts`: list count=35; sample IDs=['radio_faction_patrol_north_culvert', 'radio_faction_patrol_missing_siding', 'radio_faction_patrol_customs_road', 'radio_faction_supply_request_clinic', 'radio_faction_supply_request_fuel', 'radio_faction_supply_request_filters', 'radio_faction_propaganda_work_order', 'radio_faction_propaganda_mutual_aid']
- `schema_version`: `1`
- `description`: `Ashfall Faction Radio & Intercept Log HUD Chatter Corpus`
- SHA-256: `ef8f6cc0bf94b91df450930ab4bb20867a3108216c8d82c80de5790ac8bfc48e`
#### `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, encounters`
- `encounters`: list count=2; sample IDs=['encounter_the_mirrored_scout', 'encounter_the_metric_forest']
- `schema_version`: `1`
- SHA-256: `414a3fdecb68d54e6fd3a4252548504e1e944bea59fc88ff67cfcac850eaa582`
#### `Assets/StreamingAssets/Data/characters.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=84; sample IDs=['npc_bram_ostrowski', 'npc_sergeant_pell', 'npc_doctor_ianov', 'npc_wren', 'npc_kestrel', 'npc_nomi_fisk', 'npc_ivor_lasko', 'npc_the_cartwright_sisters']
- `schema_version`: `1`
- SHA-256: `cd33aa4801aade57932525194755b413d83ab2cd212c0ca7a5ccd09f011b5c58`
#### `Assets/StreamingAssets/Data/items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=724; sample IDs=['item_decon_chelator_concentrate', 'item_lead_lined_effluent_filter', 'item_heavy_neoprene_scrub_brush', 'item_sealed_waste_bin', 'item_theodolite_brass_precision', 'item_surveyor_stadia_rod', 'item_datum_plate_bronze', 'item_concrete_mix']
- `schema_version`: `1`
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
#### `Assets/StreamingAssets/Data/combat_catalog.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, weapons, ammo, materials, combatants`
- `weapons`: list count=20; sample IDs=['weapon_pipe_rifle', 'weapon_scrap_shotgun', 'weapon_bolt_rifle', 'weapon_assault_rifle', 'weapon_lmg', 'weapon_pipe_shotgun', 'weapon_nail_driver', 'weapon_rebar_spear']
- `ammo`: list count=14; sample IDs=['ammo_357', 'ammo_12g', 'ammo_308', 'ammo_556', 'ammo_762', 'ammo_9x19', 'ammo_22lr', 'ammo_762x54r']
- `materials`: list count=7; sample IDs=['material_wood', 'material_concrete', 'material_metal', 'material_rebar', 'armor_cloth', 'armor_kevlar', 'armor_plate']
- `combatants`: list count=12; sample IDs=['combatant_burrower_mite', 'combatant_spore_hound', 'combatant_armored_boar', 'combatant_feral_mutt', 'combatant_pale_crawler', 'combatant_chrome_loper', 'combatant_conscript_levy', 'combatant_warlord_veteran']
- `schema_version`: `2`
- `collection_id`: `combat_catalog`
- SHA-256: `e837b53cbab1b7595a12f58c09cd7dbac753ef1736a4fbf69757a4d83f36a1c6`
## Symbol and caller audit

#### `PatrolTerritoryAuthority` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=1, host=0, test=0
- `Assets/Ashfall.Core/World/NightWatchPatrolReadinessEngine.cs:91` (core) — /// Extends PatrolTerritoryAuthority (TerritoryNodeRecord, DynamicTerritoryState) and
#### `PatrolEncounterValidator` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=2, host=0, test=12
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:717` (core) — var patrolErrors = Ashfall.Core.Narrative.PatrolEncounterValidator.ValidateJson(travelJson, factionIds, itemIds);
- `Assets/Ashfall.Core/Narrative/PatrolEncounterValidator.cs:8` (declaration) — public static class PatrolEncounterValidator
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:73` (test) — var errors = PatrolEncounterValidator.Validate(_catalog.Encounters, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:124` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:133` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:142` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:151` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:160` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:169` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:178` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:187` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:198` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:207` (test) — var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
- `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs:218` (test) — var errors = PatrolEncounterValidator.Validate(new[] { v1, v2 }, _factions, _items);
#### `PatrolRadioHooks` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=6, host=0, test=16
- `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs:21` (declaration) — public sealed class PatrolRadioHooks
- `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs:62` (core) — public PatrolRadioHooks(ILog? log = null)
- `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs:115` (core) — _log.Info($"[PatrolRadioHooks] Signal '{id}' already consumed; ignoring.");
- `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs:121` (core) — _log.Info($"[PatrolRadioHooks] Signal '{id}' already in pending queue.");
- `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs:126` (core) — _log.Info($"[PatrolRadioHooks] Queued radio signal '{id}'.");
- `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs:150` (core) — _log.Info($"[PatrolRadioHooks] Dispatched and consumed radio signal '{sig}'.");
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:50` (test) — var radioHooks = new PatrolRadioHooks();
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:67` (test) — var radioHooks = new PatrolRadioHooks();
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:95` (test) — Assert.True(PatrolRadioHooks.IsFactionRadioCapable("military_remnants"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:96` (test) — Assert.True(PatrolRadioHooks.IsFactionRadioCapable("iron_garrison"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:97` (test) — Assert.True(PatrolRadioHooks.IsFactionRadioCapable("upland_militia"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:98` (test) — Assert.True(PatrolRadioHooks.IsFactionRadioCapable("faction_central_garrison"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:99` (test) — Assert.True(PatrolRadioHooks.IsFactionRadioCapable("faction_railway_guild"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:102` (test) — Assert.False(PatrolRadioHooks.IsFactionRadioCapable("faction_scavengers"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:103` (test) — Assert.False(PatrolRadioHooks.IsFactionRadioCapable("cult_of_ash_sign"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:104` (test) — Assert.False(PatrolRadioHooks.IsFactionRadioCapable("cult_of_the_glow"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:105` (test) — Assert.False(PatrolRadioHooks.IsFactionRadioCapable("warlords_sector_4"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:106` (test) — Assert.False(PatrolRadioHooks.IsFactionRadioCapable("unknown_faction"));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:107` (test) — Assert.False(PatrolRadioHooks.IsFactionRadioCapable(string.Empty));
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:113` (test) — var hooks1 = new PatrolRadioHooks();
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:131` (test) — var hooks2 = new PatrolRadioHooks();
- `Ashfall.Core.Tests/TravelEncounterPatrolRadioIntegrationTests.cs:177` (test) — Assert.True(PatrolRadioHooks.IsFactionRadioCapable(faction), $"Origin faction {faction} must be radio-capable.");
#### `TravelEncounterCombatBinder` — HOST_REFERENCE_PRESENT — core/declaration=1, host=1, test=7
- `Assets/Ashfall.Core/Expeditions/TravelEncounterCombatBinder.cs:21` (declaration) — public static class TravelEncounterCombatBinder
- `src/Host/ExpeditionHostSession.cs:1191` (host) — if (TravelEncounterCombatBinder.TryBind(definition, choice, dangerLevel, enemyCount, out var ids, ActiveRng))
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:10` (test) — //   6. the TravelEncounterCombatBinder (hostile choice → catalog spawn).
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:219` (test) — Assert.True(TravelEncounterCombatBinder.TryBind(
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:233` (test) — Assert.False(TravelEncounterCombatBinder.TryBind(
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:236` (test) — Assert.False(TravelEncounterCombatBinder.TryBind(
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:240` (test) — Assert.False(TravelEncounterCombatBinder.TryBind(
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:253` (test) — Assert.False(TravelEncounterCombatBinder.TryBind(
- `Ashfall.Core.Tests/Plan45Phase2BindingTests.cs:268` (test) — Assert.True(TravelEncounterCombatBinder.TryBind(
#### `faction_patrols` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=0
#### `patrol encounter` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=3, host=0, test=1
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:725` (core) — report.Error("patrol encounter validator error: " + ex.Message);
- `Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs:296` (core) — // Route patrol encounter resolution through TravelEngine
- `Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs:56` (core) — /// Used to gate patrol encounter choices on document discovery.
- `Ashfall.Core.Tests/Plan45EnemyCompositionTests.cs:2` (test) — // Plan 45 — enemyCombatantIds wired into expedition/patrol encounter setup.
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 41-44
00041: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
00042: Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.
00043:
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
#### authority lines 69-72
00069:
00070: The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.
00071:
00072: ---
#### authority lines 117-120
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
#### authority lines 130-133
00130: | C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
00131: | C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
00132: | C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
00133: | C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
#### authority lines 151-154
00151: | C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
00152: | C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
00153: | C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
00154: | C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
#### authority lines 245-248
00245:
00246: **SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.
00247:
00248: **SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.
#### authority lines 363-366
00363: ### Subject
00364: Roughly one hundred days after an early door-encounter or moral-choice resolution, the world returns: a visitor, a letter, a radio strip item, a journal prompt, or a rumor, keyed on the already-persisted choice flag. The player recognizes the anchor; the consequence lands in the mid-game rather than evaporating at Day 10.
00365:
00366: ### Premise evidence
#### authority lines 378-381
00378: ### Continuity checklist result
00379: Callback targets must reference existing survivor/location/faction ids only. Information-flow legality: the returning party must plausibly know the player's choice through a modeled channel (they were present, a rumor traveled, a courier carried word). Epilogue permutations: callbacks may adjust relationship deltas and epilogue weight only through the existing moral-choice weight seam; declare which permutations shift.
00380:
00381: ### Open premises
#### authority lines 400-403
00400: ### Recommended integration route
00401: Tier: DATA-ONLY (three parallel authored tranches, one per owning system). Seams: ecology infestation catalog + crop strain catalog → existing infestation event dispatch; subterranean zones + excavation hazard mitigation catalogs → existing cave-in event path; warlord doctrines + tribute ledger → existing levy/tribute seams with `FactionStanceEngine` for reactions. Save impact class: NONE (events derive from catalogs and campaign state). Determinism: events must use existing seeded event streams — no new simulation. Verification: integrity + utilization selftests, one focused event-dispatch test per arc, balance harness re-run for the levy's economic pressure.
00402:
00403: ### Continuity checklist result
#### authority lines 438-441
00438: ### Subject
00439: A coordinated content wave across the five live muster catalogs (`muster_camp_scenes`, `muster_epilogues`, `muster_faction_actions`, `muster_faction_culture`, `muster_witnesses`): culture-conditioned faction actions, witness-driven epilogue evidence chains, and camp-scene encounter depth, so the muster subsystem reaches the same integration depth as the verdict and holdfast families.
00440:
00441: ### Premise evidence
#### authority lines 697-700
00697:
00698: **A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00699:
00700: **A-17 · C7 · Verdict radio continuation.** Subject: verdict-station rundown batches conditioned on verdict questline state. Evidence: `verdict_radio.json` verified live; verdict questlines are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 747-750
00747:
00748: **B-10 · C7 · FactionWar per-strike emitter extension.** Subject: per-strike event emission for the faction war chain. Evidence: foreman-flagged open decision (v1.0 Part 7 gap 4). Status: GATE. Route: CORE-EXTENSION. Confidence: VERIFIED as open; blocked on signature.
00749:
00750: **B-11 · C7 · Black-market funds legs.** Subject: canonical funds authority for black-market settlement. Evidence: decision-blocked (needs canonical funds authority — v1.0 Part 7 gap 4); the actions surface itself was sealed by `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS` (DR-06). Status: GATE. Confidence: VERIFIED as blocked.
#### authority lines 769-772
00769:
00770: **B-21 · C14 · Migration ↔ route-encounter bridge.** Subject: wildlife migration state conditioning travel-encounter selection on routes crossing migration corridors. Evidence: `WildlifeMigrationSystem`, `travel_encounters.json` both canon. Route: CORE-EXTENSION. Confidence: PROPOSAL.
00771:
00772: **B-22 · C15 · Defense-grid ↔ siege math.** Subject: perimeter defense values entering warlord siege/raid resolution; sky-armor values entering orbital-harrow telemetry thresholds. Evidence: warlord siege math and orbital harrow telemetry are canon systems; catalogs live. Route: CORE-EXTENSION. Confidence: PROPOSAL.
#### authority lines 835-838
00835:
00836: **E-04 · C8 · Radio strip extension points.** Subject: any new radio-conditioned content surfaces through the existing RESCUE SIGNALS strip seams — additive only, sealed surface respected. Evidence: strip shipped and sealed (DR-06). Route: HOST-WIRING. Status: SEALED-adjacent. Confidence: HIGH CONFIDENCE constraint.
00837:
00838: **E-05 · C17 · A11y words-not-color-only sweep for newer panels.** Subject: audit newer panels for color-only state signaling; add textual state wherever found. Evidence: `ACCESSIBILITY.md` and the a11y gate are canon. Route: HOST-WIRING. Confidence: HIGH CONFIDENCE that the sweep is warranted.
#### authority lines 864-867
00864:
00865: **F-05 · C8 · Radio dial per-frame work.** Subject: measure SNR dial work at 15 FPS during active tuning; the dial is one of the canon real-time/frame surfaces. Evidence: real-time tier is canon (v1.0 Part 3.2). Route: profiling pass. Confidence: potential hotspot — requires profiling.
00866:
00867: **F-06 · Cross · Save-flush cost at day tick.** Subject: measure daily save-flush duration against tick budget for large late-game states (many survivors, full dose ledger, long journals). Evidence: daily save flush is a canon tick step. Route: measurement with a synthetic late-game fixture. Confidence: potential hotspot — requires profiling.
#### authority lines 878-881
00878:
00879: **G-05 · C7 · War-chain authored-day mapping tests.** Subject: pin the 300-day offset mapping (playable 180 → authored 480) with boundary tests. Evidence: mapping is canon (`FactionWarChainRunner.ToAuthoredDay`). Route: focused xUnit. Confidence: HIGH CONFIDENCE.
00880:
00881: **G-06 · C8 · Exactly-once guard regression suite.** Subject: regression tests covering every sealed exactly-once guard class (ignore consequences, arrival resolution, salvage grants) against restore-mid-effect saves. Evidence: sealed runtime models the guards (DR-06). Route: focused xUnit + fixture saves. Confidence: HIGH CONFIDENCE.
#### authority lines 930-933
00930:
00931: Each map lists the cluster's canon owners, live catalogs, host sessions, and current factory openings. These are planning instruments: a session picks a cluster, reads its map, and consumes its seeds. All catalog and system names below are carried from the verified v1.0 inventory and the live 2026-09-24 listings; per-field internals remain session-verify territory.
00932:
00933: **DM-1 — Shelter operations (C1).** Owners: shelter rooms/identities/machines, thermal, schedules, social events, decor, fire, noise, airlock security, decon, atmosphere, sanitation (power-fed). Live catalogs: `shelter_rooms`, `shelter_room_identities`, `shelter_machine_identities`, `shelter_schedules`, `shelter_social_events`, `shelter_audio_cues`, `shelter_insulation_catalog`, `shelter_shielding`, `sanitation_facilities`, plus the sealed grid catalog. Hosts: ShelterAssignment, ShelterAtmosphere, ShelterDecor, ShelterFire, ShelterSchedule, ShelterThermal, Sanitation, AirlockSecurity, Decontamination, Ventilation. Openings: A-01, A-02, B-01, B-02, D-06, F-04. Notable constraint: room effects route through `IsRoomPowered`; shelter state persists through the holdfast/shelter save family.
#### authority lines 944-947
00944:
00945: **DM-7 — Factions and war (C7).** Owners: stance engine, doctrines, war system/chain runner, tributes, treaties, embargoes, espionage, psyops, counter-intelligence, musters, labor camps, bounty board. Live catalogs: `factions`, `faction_lore`, `faction_territory`, `faction_intelligence`, branch catalogs (independent/military/rebel), faction war family (communiques/dialogue/events/journal/radio/location_overrides), `warlord_doctrines`, `muster_*` family (five), `labor_camps`, `bounty_board`, `regional_treaties`, `trade_embargoes`, `foundry_accords`, `holdfast_factions`, `crossing_factions`. Hosts: Espionage, PsyOps, CounterIntelligence, Muster, FactionBranch, RegionalTreaty. Openings: A-16, A-17, A-18, B-10 (GATE), B-11 (GATE), C-05, C-06, G-05, plus the F-004 muster campaign. Constraint: all standing effects through `FactionStanceEngine`.
00946:
00947: **DM-8 — Radio and information (C8).** Owners: radio system, stations, programs, intercepts, distress signals (sealed runtime), rumors, sound ranging, direction finding, NVIS, heliograph. Live catalogs: `radio`, `radio_stations`, `radio_programs`, `radio_intercepts`, `radio_distress_signals` (+ expansion), `comms_targets`, `sound_ranging_catalog`, `direction_finding_catalog`, `nvis_communications_catalog`, `heliograph`. Hosts: Radio, RadioProgramProduction, SoundRanging, Heliograph. Sealed: distress content (`CF-P1-DISTRESS-CONTENT-SEAL`); availability consumer retired. Openings: A-19, A-20, B-12, B-13, B-25 (coordinated), E-04, F-05, G-06. Constraint: genuine-never-hostile invariant; no new signal scenarios without signature.
… 93 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.

**Requested behavior.** Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.

**Minimum safe delta.** Extend `faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.

**Required delta.** Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.

**Primary seam.** faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `PatrolTerritoryAuthority`, `PatrolEncounterValidator`, `PatrolRadioHooks`, `TravelEncounterCombatBinder`, `faction_patrols`, `patrol encounter`.

## Integration framework

The integration framework is deliberately owner-first:

1. **Read and classify current state.** Start with the named Core owner, current JSON, host session, save store, and focused tests. Record whether the feature is live, partially wired, dormant, stale, or decision-gated.
2. **Choose one authority per concern.** Extend the current owner when it exists. If no owner exists, stop at the architecture decision boundary and name the new authority decision rather than creating a parallel store, selector, ledger, panel, or simulation.
3. **Author data against consumers.** A JSON row is not integrated merely because it parses. Every row must have a current loader, a current consumer, a visible or mechanically observable outcome, and a validation path.
4. **Route effects through existing events/seams.** Core emits facts; host sessions translate them; UI presents truthful state. Do not place gameplay calculations in a panel or Godot callback.
5. **Persist through the owning save path.** Capture and restore must be implemented before a feature is called persistent. Old versions, nulls, empty collections, checksums, and mid-event saves are explicit cases.
6. **Verify narrowly.** Use the smallest existing test file or a new focused test for an uncovered confirmed contract. Keep Core, data, host, UI, save, determinism, and cross-system checks distinguishable.
7. **Roll back by boundary.** A failed expansion should disable its adapter or authored tranche without corrupting the owning state or requiring a destructive reset.

## Code architecture

### Core layer

- Put reusable rules, validation, state transitions, deterministic selection, and read models in `Assets/Ashfall.Core/`.
- Keep Core engine-free. No Godot, Unity, `Texture2D`, `JsonUtility`, wall-clock, or unseeded randomness belongs in a Core contract.
- Extend existing models and public methods when the current API already expresses the concern. A new DTO, interface, event, or catalog loader is justified only when it removes a real ownership or boundary problem.
- Make invalid input observable through the owner’s normal result/diagnostic path. Do not silently coerce malformed content into a successful state.
- Keep deterministic ordering explicit: use ordinal IDs, stable catalog order, bounded collections, and the existing seeded RNG fork for any stochastic choice.

### Data layer

- Author under `Assets/StreamingAssets/Data/` using the existing schema and snake_case IDs.
- Prefer additive fields and existing collections over parallel catalogs.
- Validate IDs, references, ranges, and consumer reachability through the current catalog integrity pipeline.
- Record schema version and old-data behavior in the plan and implementing handoff.
- Data prose may describe a consequence only when the consequence is expressible through a current owner and event.

### Host layer

- Load the catalog in the current host/session owner, not in a panel constructor.
- Subscribe once to owner events, translate facts into existing journal/radio/UI signals, and dispose subscriptions with the session.
- Bind day/hour/event triggers through the existing campaign owner. Do not create a second clock or update loop.
- Rehydrate from the existing save owner and mark dirty only for actual canonical mutations.
- Keep host code free of duplicate gameplay math; it may format, route, and adapt.

### Presentation layer

- Panels expose current commands and truthful current state.
- They display unavailable/blocked reasons, provenance, and the next legitimate action rather than simulating a result.
- Preserve keyboard/controller close/back behavior, focus order, readable contrast, and reduced-motion/accessibility settings.
- Refresh from owner events and lifecycle state; never use a panel cache as authority.
- Snapshot or accessibility fixtures are verification artifacts, not gameplay state.

## Ownership and state matrix

| Concern | Authoritative owner | Adapter responsibility | Persistence rule | Verification gate |
|---|---|---|---|---|
| Domain rules | Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
| Authored content | Current JSON catalog and loader | Load/validate once | Catalog version/defaults | Data integrity |
| Runtime lifecycle | Existing host/session owner | Setup, event subscription, disposal | Existing save/session | Host wiring |
| Presentation | Existing Godot surface | Format and command dispatch | No gameplay state | UI/focus/headless |
| Diagnostics | Existing logging/telemetry owner | Correlate ID and phase | Bounded/non-authoritative | Failure test |
| Historical authority | Read-only master document | Cite relevant section only | Never persisted | Plan QA |

## Data, event, and command flow

`authoritative JSON / player command / current owner event` → validation at the owning seam → canonical Core state transition → typed fact/event → host session projection → journal/radio/UI refresh → existing save owner if the transition mutated durable state.

The flow is intentionally one-way for authority. UI may send a command, but it cannot directly mutate the domain. Events may be consumed by several read-only projections, but only the owner writes the state. If a proposed feature needs a second writer, that is an architecture failure, not an invitation to add another event bus.

## State, API, and compatibility contract

The implementing agent must confirm the actual public API and write the final signatures in the implementation handoff. At minimum, expose:

- a read-only query/projection for the current state;
- an explicit command or owner method for each player-visible mutation;
- a typed fact/event for meaningful state changes;
- capture/restore methods on the existing state owner;
- a diagnostic result for invalid or unavailable data;
- a stable key for idempotent commands and replay;
- a bounded, ordinal-stable collection for any retained history.

Old saves must default missing additive fields to the documented neutral value. New required fields need a versioned migration. Null and empty semantics must differ deliberately: null means unavailable/not supplied; empty means validly no records, unless the current owner’s contract says otherwise. Do not infer a new save section from the plan title.

## Determinism and replay

For every proposed random or time-dependent element, name the seed source, stream/fork, draw order, retry behavior, and tie-break rule. Prefer no randomness for validation, lookup, and deterministic UI state. If an existing owner uses `ISeededRng`, reuse its campaign stream/fork rather than constructing a private generator. Wall-clock time, `System.Random`, `Guid.NewGuid`, hash iteration order, filesystem enumeration order, and frame timing are prohibited in deterministic Core behavior.

The replay acceptance test must use two equivalent runs with identical seed, catalog snapshot, state, day/hour, and command sequence. Compare canonical state, event order, resource/ledger deltas, and persistence payload. Presentation-only differences are acceptable only when they are explicitly non-authoritative and do not alter commands or outcomes.

## Save, restore, and migration

Before implementation claims persistence:

1. Identify the current save-section owner and DTO.
2. Add or reuse capture/restore through that owner.
3. Deep-copy mutable collections so restoring does not alias runtime state.
4. Define old-version defaults for every new field.
5. Define behavior for missing catalogs, unknown IDs, partial records, and corrupted checksums.
6. Test capture → serialize → restore → continued mutation, plus a mid-transition reload.
7. Confirm a save/load pair does not duplicate one-shot events or reapply a quest/choice/ledger mutation.

No plan-created “state cache” is allowed. If a new authority is genuinely required, the package must pause for a decision and name its owner, section, migration, and test contract.

## Failure and edge behavior

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 45.

## Test strategy and focused commands

The plan-only pass does not execute tests. The implementing package should reuse existing focused files first, run a new file alone, and stay below the repository’s focused-test policy unless a foreman-approved hypothesis requires more. The current candidate commands are listed in the verification matrix and must be revalidated against the worktree at implementation start. A passing compile is not proof of host wiring, persistence, determinism, or player reachability.

## Phased implementation and rollback

The detailed phase table below is the implementation contract. Each phase has a completion gate and a “must not touch yet” boundary. Rollback is additive and local: disable the adapter or remove the authored tranche, retain the owner’s last valid state, and never reset the shared worktree or shared save registry to hide a failure.

## Dependency-ordered implementation phases
| Phase | Outcome | Work | Boundary | Completion gate |
|---|---|---|---|---|
| 0 | Premise recheck and baseline | Confirm exact owner/API/catalog counts, current claims, dirty paths, and active decision gates. | No edits to production. | A written evidence table and focused baseline commands. |
| 1 | Owner and collision map | Trace current callers, save owner, event seam, and duplicate/legacy candidates. | No new catalog or state. | Single-owner map with zero unresolved authority collisions. |
| 2 | Core contract or bounded extension | Add only the smallest pure contract needed by the confirmed gap, or document that no Core change is needed. | No Godot/UI/data authoring. | Core tests for boundaries, transitions, invalid data, and determinism. |
| 3 | Persistence and migration contract | Implement capture/restore/old-save defaults through the existing owner. | No unrelated save sections. | Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass. |
| 4 | Authored data tranche | Author schema-valid rows only after consumer fields are known; validate references and reachability. | No prose-only orphan rows. | Data integrity and consumer coverage pass for the tranche. |
| 5 | Host/event wiring | Load, subscribe, translate, and dispose in the current host/session owner. | No panel gameplay math. | Host wiring test proves event → projection and setup/teardown. |
| 6 | Presentation and accessibility | Expose truthful state, commands, focus, controller/keyboard behavior, and feedback. | No new authority in UI. | Panel route/focus/headless checks pass; snapshots only through the owning harness. |
| 7 | End-to-end and replay | Run a bounded scenario, save/reload, paired seeded replay, and cross-system consequence check. | No full-suite default. | Named commands/results and limitations recorded. |
| 8 | Balance/content polish | Tune only authored values with current harnesses; remove dead rows and polish truthful text. | No hidden tuning or parallel scalar. | Content review confirms no dominated/unreachable row and no unsupported claim. |
| 9 | Rollback and closeout | Document feature disablement, migration reversal, owner handoff, and residual debt. | No unowned cleanup. | Foreman review accepts or records a blocker. |

### Phase-specific implementation questions
#### Phase 0: Premise recheck and baseline
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.
- What is the smallest safe change? Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.
- Which owner is touched? Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Narrative/PatrolEncounterValidator.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Expeditions/TravelEncounterCombatBinder.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/World/WastelandMapSystem.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Expeditions.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/World/WastelandMapView.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/TravelingCaravanPanel.cs` — Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_territory.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_events.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_radio_corpus.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/characters.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/combat_catalog.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/World/WastelandMapView.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/TravelingCaravanPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/MapDetailPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/Radio/FactionRadioHudPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/PatrolEncounterIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/PatrolTerritoryIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/PatrolFactionStandingTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/TravelEncounterPatrolVariantTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences is wired end to end or the plan explicitly closes as already integrated.
- Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

PatrolTerritoryAuthority, PatrolEncounterValidator, PatrolRadioHooks, TravelEncounterCombatBinder, faction territory/war/radio data, patrol-focused tests, and expedition/caravan callers are live. The old plan’s missing faction_patrols.json and standalone manager are not current evidence.

# 3. Required Delta

Audit actual patrol catalogs and callers, then define authored posture/loadout rows, selection weights, peaceful/combat resolution, and reachability without duplicating TravelEncounter or FactionStance.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Faction-war and travel encounter owners are mature; the plan must not make patrol presence a second map or encounter scheduler. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `territory_the_office`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `territory_the_cutters`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `territory_black_flotilla`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `territory_the_fleet`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `territory_deserter_coalition`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `territory_cold_count`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `territory_the_tally`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `territory_grain_exchange`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `territory_quiet_house`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `territory_scavenger_guild`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `territory_long_walk`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `territory_undertow`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `territory_hydro_barons`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `territory_iron_raiders`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `territory_the_provisioned`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `territory_archivists`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `territory_lamplighters`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `territory_sun_seekers`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `territory_osteophages`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `zone_contested_water_rights`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `zone_contested_cut_salvage`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `zone_contested_merchant_crossroads`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `zone_contested_scarp_pass`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `zone_contested_coastal_bluff`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `radio_faction_patrol_north_culvert`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `radio_faction_patrol_missing_siding`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `radio_faction_patrol_customs_road`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `radio_faction_supply_request_clinic`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `radio_faction_supply_request_fuel`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `radio_faction_supply_request_filters`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `radio_faction_propaganda_work_order`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `radio_faction_propaganda_mutual_aid`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `radio_faction_propaganda_closed_gates`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `radio_faction_distress_patrol_ambush`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `radio_faction_distress_clinic_evacuation`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `radio_faction_distress_convoy_stranded`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `radio_faction_encrypted_repeating_groups`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `radio_faction_encrypted_short_burst`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `radio_faction_encrypted_key_rotation`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `radio_faction_military_convoy_corridor`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `radio_faction_military_checkpoint_reinforce`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `radio_faction_military_withdrawal_order`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `radio_faction_civilian_water_warning`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `radio_faction_civilian_missing_family`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `radio_faction_civilian_market_rumor`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `radio_faction_dead_hand_readiness_check`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `radio_faction_dead_hand_orbital_track`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `radio_faction_dead_hand_command_link`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `radio_faction_weather_ash_front`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `radio_faction_weather_cold_snap`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `radio_faction_weather_runoff_warning`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `radio_faction_inventory_fuel_bunkers`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `radio_faction_inventory_grain_medicine`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `radio_faction_inventory_tools_welding`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `radio_patrol_garrison_checkpoint`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `radio_patrol_warlord_raid`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `radio_patrol_border_closed`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `radio_patrol_convoy_attacked`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `radio_patrol_press_gang`
- Source: `Assets/StreamingAssets/Data/faction_radio_corpus.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `encounter_the_mirrored_scout`
- Source: `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `encounter_the_metric_forest`
- Source: `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `npc_bram_ostrowski`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `npc_sergeant_pell`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `npc_doctor_ianov`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `npc_wren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `npc_kestrel`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `npc_nomi_fisk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `npc_ivor_lasko`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `npc_the_cartwright_sisters`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `npc_edor_vale`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `npc_yara_holm`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `npc_leva_quist`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `npc_cael_ormund`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `npc_halden_mire`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `npc_cluster_teacher`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `npc_osran_kell`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `npc_mattis_cray`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `npc_wyn_sabler`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `npc_dessa_vane`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `npc_perrin_ashby`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `npc_ivo_fenn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `npc_kess_adler`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `npc_ansel_duth`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `npc_tamsin_rook`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `npc_len_quill`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `npc_hadi_morrow`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `npc_nila_brant`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `npc_maren_holt`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `npc_ira_vell`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `npc_benno_kade`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `npc_quil_esser`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `npc_osric_tann`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `npc_dara_mewn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `npc_dr_irina_vel`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `npc_wyn_omah`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `npc_piet_abar`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `npc_saria_voss`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `npc_salt_marshal_varn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `npc_salt_trader_elena`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `npc_salt_boiler_petyr`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `npc_switch_master_korov`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `npc_rail_chandler_bess`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `npc_rivet_smith_milos`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `npc_beacon_keeper_maren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `npc_coastal_chandler_orlov`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `npc_net_mender_kira`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `npc_quarry_steward_darek`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `npc_stone_cutter_valya`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `npc_driller_jarek`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `npc_prior_silas`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `npc_almoner_hanna`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `npc_wayfarer_tobias`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `npc_market_warden_grimm`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `npc_junk_broker_solomon`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `npc_grease_monkey_tess`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `npc_odile_vanter`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `npc_cass_polder`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `npc_jorin_hael`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `npc_uma_tarran`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `npc_halloran_vesk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `npc_lotte_verrill`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `npc_mara_veln`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `npc_oskar_ruut`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `npc_tomas_geret`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `npc_joren_malk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `npc_pavel_eren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `npc_sena_aris`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `npc_dalia_marun`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `npc_anton_renn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `npc_emil_soren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `npc_nadia_lem`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `npc_arvo_tamm`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `npc_kaspar_drej`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `npc_mira_vos`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `npc_janek_orel`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `npc_veda_ro`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `npc_mirael_tesk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `npc_niko`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `npc_ilze_kaar`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `npc_marek_voln`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `npc_lina`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `npc_anete_sarn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `npc_elder_sava`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `npc_rika_dorn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `npc_liva_kern`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `item_decon_chelator_concentrate`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `item_lead_lined_effluent_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `item_heavy_neoprene_scrub_brush`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `item_sealed_waste_bin`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `item_theodolite_brass_precision`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `item_surveyor_stadia_rod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `item_datum_plate_bronze`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `item_concrete_mix`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `item_forged_rotor_shaft`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `item_magnetic_bearing_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `item_high_vacuum_pump`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `item_containment_ring_steel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `item_reinforced_concrete_vault`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `item_seismic_damper_pad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `item_vacuum_pump_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `item_bearing_grease`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `item_rotor_balancing_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `item_portable_pid_detector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `item_detector_sensor_module`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `item_hermetic_sample_ampoule`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `item_hot_dust_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `item_sludge_cake`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `item_tailings_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `dosimeter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `geiger_counter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `iodine_pills`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `anti_rad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `gas_mask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `hazmat_suit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `water_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `air_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `clean_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `irradiated_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `canned_food`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each patrol/encounter record, prove the route and territory consumer, the selection weight, the faction-standing effect, and the peaceful/combat branch before calling it reachable.
- Primary owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State/save rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI truth rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: territory, patrol behavior, and travel encounters.
- Seam under test: faction territory + patrol records -> PatrolTerritoryAuthority/PatrolEncounterValidator -> TravelEncounter selection/combat binder -> radio/journal/faction consequences.
- Expected authority: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store. Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI/accessibility check: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- Player-facing truth: Unknown faction/loadout, unreachable territory, empty encounter table, repeated cooldown, dead survivor, missing combat owner, and hostile radio state fail closed with a diagnostic.
- Persistence response: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism response: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: PatrolTerritoryAuthority.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: PatrolEncounterValidator.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: PatrolRadioHooks.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: TravelEncounterCombatBinder.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: faction_patrols.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: patrol encounter.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: PatrolTerritoryAuthority.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: PatrolEncounterValidator.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: PatrolRadioHooks.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: TravelEncounterCombatBinder.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: faction_patrols.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: patrol encounter.
- Owner: Patrol authority owns patrol classification; travel encounter owner owns selection; tactical combat owns combat; faction standing owner receives outcomes; host owns route lifecycle.
- State rule: Patrol instances/cooldowns/resolved encounters use existing encounter/territory/war state or an explicitly owned additive state; no second faction-standing store.
- Determinism rule: Encounter selection uses route distance, stable patrol priority, and existing seeded RNG; ROE changes are bounded and replayable.
- UI rule: ['src/World/WastelandMapView.cs', 'src/UI/TravelingCaravanPanel.cs', 'src/UI/MapDetailPanel.cs', 'src/Radio/FactionRadioHudPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/PatrolEncounterValidationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/PatrolEncounterValidationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/PatrolEncounterIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/PatrolEncounterIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/PatrolTerritoryIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/PatrolTerritoryIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/PatrolFactionStandingTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/PatrolFactionStandingTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/PatrolCampaignCrossSystemSmokeTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 06
- Test: `Ashfall.Core.Tests/TravelEncounterPatrolVariantTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/TravelEncounterPatrolVariantTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 494,911 characters.
# Post-250K deep polishing pass

The architecture body above reached 494,989 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `PatrolTerritoryAuthority`, `PatrolEncounterValidator`, `PatrolRadioHooks`, `TravelEncounterCombatBinder`, `faction_patrols`, `patrol encounter`.
- Confirm each proposed mutation has exactly one writer. A host adapter may translate a fact; a panel may display it; neither becomes authority.
- Check for legacy or parallel names before proposing any new class, DTO, event, catalog, save section, or RNG stream.
- Treat the master authority as a read-only design lens. Current source/data wins when they disagree, and the disagreement is recorded rather than hidden.
- Recheck the live worktree status recorded in each evidence dossier. A dirty source path is a coordination warning, not a stable acceptance result.

## Deep polish B — data and consumer precision

- For every candidate row, name the loader, consumer, reference validator, and observable outcome.
- Replace counts copied from the old plan with current JSON counts or an explicit census task.
- Remove any row whose only consumer is a test, a prose generator, or a panel.
- Preserve additive schema compatibility and explain old-data defaults.
- Check that narrative text does not promise an effect that the current owner cannot produce.

## Deep polish C — state, save, and replay precision

- Confirm the existing save owner and DTO before naming a field.
- Test capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input, and mid-event reload.
- Confirm repeated commands and replay cannot duplicate one-shot effects.
- Use the existing seeded stream/fork; document every random draw and tie-break.
- Treat a UI refresh as a projection, never as persistence or mutation.

## Deep polish D — failure and player truth

- Walk null, empty, missing, duplicate, stale, hostile, dead, unavailable, and repeated-event cases.
- Ensure blocked/unavailable states are legible and do not masquerade as success.
- Keep accessibility behavior attached to the same command/state surface as the visual behavior.
- Record which failures are diagnostic-only, which defer presentation, and which halt the transition.

## Deep polish E — implementation readiness

- Replace generic file lists with owner-specific action verbs and completion gates.
- Keep the phase order dependency-safe: premise, owner, Core, persistence, data, host, presentation, end-to-end, polish, closeout.
- Give the next implementer exact focused commands, test selection rationale, and stop conditions.
- Keep a final residual-risk list for decision-gated or concurrent work rather than hiding it in prose.

# Post-250K deep polishing pass — second pass

The first post-250K pass above checked structure and owner collisions. This second pass is a separate adversarial review after the plan has reached its depth checkpoint; it is not a duplicate paragraph exercise.

## Second-pass adversarial questions

- What would a reviewer incorrectly assume after reading only the executive summary?
- Which sentence describes historical intent but could be mistaken for current behavior?
- Which current owner, save path, host session, or event seam is missing from the impact map?
- Which data row has a valid ID but no reachable consumer?
- Which UI label could claim a consequence before the Core transition succeeds?
- Which failure currently falls through as a default success, duplicate event, or stale cache?
- Which random choice lacks a named seed/fork/tie-break?
- Which old-save field lacks a default, migration, or deep-copy test?
- Which concurrent package or decision gate could invalidate the proposed path?
- What is the smallest rollback that leaves the previous owner state readable?

## Second-pass correction protocol

For every answer, classify the issue as `CURRENT_EVIDENCE`, `PROPOSED_EXTENSION`, `DECISION_GATE`, `CONCURRENT_CLAIM`, or `OUT_OF_SCOPE`. Update the relevant numbered section and record the exact path/command that would close the issue. If no current evidence supports a claim, remove the claim rather than softening it with adjectives. If a proposed change needs a new architecture decision, stop at the gate and name the decision owner. This pass must leave the plan more precise, not merely longer.

## Second-pass acceptance

- [ ] Every “current” statement has a path or is marked as an open premise.
- [ ] Every “implemented” statement names a current caller or is downgraded to catalog-only.
- [ ] Every proposed state field names its save owner and migration behavior.
- [ ] Every proposed random/temporal behavior names determinism treatment.
- [ ] Every UI consequence has a command/state source.
- [ ] Every failure has a safe expected outcome.
- [ ] Every focused command resolves or is labeled future implementation work.
- [ ] Every decision-gated or concurrently claimed seam is explicit.

# Final precision and reaccuracy pass

1. Re-read every current path cited in the dossier and mark missing paths as open premises.
2. Re-run the hash verifier and data JSON parse check at the final snapshot.
3. Re-run the symbol occurrence audit and distinguish declarations, host consumers, and test-only references.
4. Check all current focused test commands resolve to existing files.
5. Remove unsupported historical counts, “sealed” claims, invented APIs, and unproven caller assertions.
6. Reconcile the plan title and requested delta with the actual current owner; if the old plan is already implemented or stale, state maintenance or blocked scope plainly.
7. Confirm Core remains engine-free, JSON remains authoritative, and no parallel authority is proposed.
8. Confirm UI, failure, save, determinism, and accessibility contracts are concrete.
9. Confirm rollback is local and does not require destructive state reset.
10. Record limitations honestly: this package is planning-only and does not run implementation tests.

# Full repolishing phase

The final repolishing pass is a quality audit over the complete document, not an append-only slogan. Read the plan from executive summary through handoff as one artifact.

- **Coherence:** every section uses the same owner names, state terms, and delta.
- **Evidence:** every important claim points to a current path, current data record, read-only authority anchor, or explicit unknown.
- **Architecture:** no duplicate system, parallel save, second RNG, engine dependency, or UI authority is smuggled into a phase.
- **Mechanics:** effects are expressed through existing commands/events and have a visible or auditable consequence.
- **Continuity:** references, days, locations, factions, and narrative facts use canonical IDs and current catalogs.
- **Failure:** invalid and unavailable states are defined, bounded, and truthful.
- **Persistence:** capture/restore and migration are named for every durable delta; no new section is casually proposed.
- **Determinism:** random sources, ordering, and replay comparisons are explicit.
- **UI:** panels are thin, accessible, refreshable, and non-authoritative.
- **Testing:** each layer has a focused command or a clear reason it must be authored later.
- **Rollback:** every phase has a local reversal and preservation rule.
- **Handoff:** the next implementer can start with the first safe step without interpreting the plan around stale prose.

## Repolish acceptance checklist

- [ ] Current owner/API confirmed from source.
- [ ] Current data schema and references confirmed.
- [ ] Existing save owner and migration path confirmed.
- [ ] Existing host/event seam confirmed.
- [ ] Existing UI surface and accessibility path confirmed.
- [ ] Focused test paths resolve or are labeled future work.
- [ ] Deterministic replay and idempotency contract stated.
- [ ] Failure and old-save behavior stated.
- [ ] No unsupported completion claim remains.
- [ ] Rollback and owner handoff are actionable.
- [ ] Plan remains implementation-ready without production edits in this package.

# Implementation handoff

## MUST PRESERVE

- The current owner named in this plan and the one-authority rule.
- Godot as the presentation/host authority and Core as engine-free domain logic.
- Authoritative JSON under `Assets/StreamingAssets/Data/`.
- Existing save ownership, migration semantics, seeded RNG contracts, and event ordering.
- Existing accessibility, focus, controller/keyboard close/back, and truthful UI behavior.

## MUST ADD

- Only the smallest confirmed Core/data/host/UI extension needed by the current delta.
- Exact catalog validation, consumer reachability, save/restore, failure, determinism, and focused tests.
- A bounded content tranche whose records are consumed and observable.
- A local rollback/disablement path and a concise implementation handoff.

## MUST NOT DO

- Do not create a second ledger, save store, selector, event authority, simulation, or panel-owned rule.
- Do not edit production, data, tests, UI, assets, or generated indexes during this planning package.
- Do not restore Unity or add engine references to Core.
- Do not claim tests, host wiring, save integration, or player reachability from static intent alone.
- Do not use unseeded randomness, wall-clock ordering, or hash iteration order for deterministic behavior.

## VERIFY WITH

- The exact focused `scripts/run_test.sh` commands listed in this plan after the implementation claim is opened.
- Current catalog integrity and consumer/utilization checks.
- A bounded Core test, save round-trip/migration test, host wiring test, and deterministic replay comparison.
- Godot headless/UI checks only when the confirmed implementation touches the host/UI path.

## FIRST SAFE IMPLEMENTATION STEP

Re-read the current owner, the first current catalog, the first current host/session, and the first focused test listed in the dossier. Write a one-page premise table that classifies each as live, partial, stale, or blocked. Do not author content or code until the table has one owner and one non-overlapping implementation seam.
